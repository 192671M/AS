using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Security.Cryptography;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Net;
using System.IO;
using System.Web.Script.Serialization;
using System.Web.Services;

namespace SITCONNECT
{
    public partial class Login : System.Web.UI.Page
    {

        string MYDBConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MYDBConnection"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btn_login_Click(object sender, EventArgs e)
        {
            HttpUtility.HtmlEncode(tb_email.Text);
            HttpUtility.HtmlEncode(tb_pwd.Text);

            string email = tb_email.Text.ToString().Trim();
            string pwd = tb_pwd.Text.ToString().Trim();

            SHA512Managed hashing = new SHA512Managed();
            string dbHash = getDBHash(email);
            string dbSalt = getDBSalt(email);
            if (ValidateCaptcha())
            {
                try
                {
                    if (dbSalt != null && dbSalt.Length > 0 && dbHash != null && dbHash.Length > 0)
                    {
                        string pwdWithSalt = pwd + dbSalt;
                        byte[] hashWithSalt = hashing.ComputeHash(Encoding.UTF8.GetBytes(pwdWithSalt));
                        string userHash = Convert.ToBase64String(hashWithSalt);

                        SqlConnection con = new SqlConnection(MYDBConnectionString);
                        con.Open();
                        SqlCommand cmd = new SqlCommand("Select Id, isLocked, AttemptCount from Account where Email=@Email", con);
                        cmd.Parameters.AddWithValue("@Email", tb_email.Text);
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataSet ds = new DataSet();
                        da.Fill(ds);
                        if (ds != null)
                        {
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                DataRow row = ds.Tables[0].Rows[0];  // Sql command returns only one record
                                int attempts = Convert.ToInt32(row["AttemptCount"].ToString());
                                int isLocked = Convert.ToInt32(row["IsLocked"].ToString());
                                if (userHash.Equals(dbHash) && isLocked == 0)
                                {
                                    Session["Email"] = email;

                                    string guid = Guid.NewGuid().ToString();
                                    Session["AuthToken"] = guid;

                                    Response.Cookies.Add(new HttpCookie("AuthToken", guid));

                                    cmd = new SqlCommand("Update Account set IsLocked=0, AttemptCount=0 where Email=@Email", con);
                                    cmd.Parameters.AddWithValue("@Email", tb_email.Text);
                                    cmd.ExecuteNonQuery();

                                    Response.Redirect("Success.aspx", false);
                                }
                                else
                                {
                                    if (attempts == 3)
                                    {

                                        lbl_error.Text = "Your account is locked";
                                        lbl_error.ForeColor = Color.Red;

                                        cmd = new SqlCommand("Update Account set IsLocked=1,AttemptCount=3 where Email=@Email", con);
                                        cmd.Parameters.AddWithValue("@Email", tb_email.Text);
                                        cmd.ExecuteNonQuery();
                                    }
                                    else
                                    {
                                        attempts += 1;

                                        lbl_error.Text = "Email or password is not valid. Please try again." + attempts;
                                        lbl_error.ForeColor = Color.Red;

                                        cmd = new SqlCommand("Update Account set AttemptCount+=1 where Email=@Email", con);
                                        cmd.Parameters.AddWithValue("@Email", tb_email.Text);
                                        cmd.ExecuteNonQuery();
                                    }
                                }
                            }
                        }
                        con.Close();
                    }
                    else
                    {
                        lbl_error.Text = "Email or password is not valid. Please try again.";
                        lbl_error.ForeColor = Color.Red;
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.ToString());
                }
                finally { }

            }


            else
            {
                lbl_error.Text = "Invalid Captcha. Please try again.";
                lbl_error.ForeColor = Color.Red;
            }
        }

        protected string getDBHash(string email)
        {
            string h = null;

            SqlConnection connection = new SqlConnection(MYDBConnectionString);
            string sql = "select PasswordHash FROM Account WHERE Email=@EMAIL";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@EMAIL", email);

            try
            {
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        if (reader["PasswordHash"] != null)
                        {
                            if (reader["PasswordHash"] != DBNull.Value)
                            {
                                h = reader["PasswordHash"].ToString();
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally { connection.Close(); }
            return h;
        }
        protected string getDBSalt(string email)
        {
            string s = null;
            SqlConnection connection = new SqlConnection(MYDBConnectionString);
            string sql = "select PASSWORDSALT FROM ACCOUNT WHERE Email=@EMAIL";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@EMAIL", email);
            try
            {
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader["PASSWORDSALT"] != null)
                        {
                            if (reader["PASSWORDSALT"] != DBNull.Value)
                            {
                                s = reader["PASSWORDSALT"].ToString();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally { connection.Close(); }
            return s;
        }
        public class MyObject
        {
            public string success { get; set; }
            public List<string> ErrorMessage { get; set; }
        }
        public bool ValidateCaptcha()
        {
            bool result = true;

            //When user submits the recaptcha form, the user gets a response POST parameter.
            //captchaResponse consist of the user click pattern, Behaviour analytics AI :)
            string captchaResponse = Request.Form["g-recaptcha-response"];

            //To send a GET request to Google along with the response and Secret key.
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(" &response=" + captchaResponse);

            try
            {
                //Codes to receive the Response in Json format from Google Server
                using (WebResponse wResponse = req.GetResponse())
                {
                    using (StreamReader readStream = new StreamReader(wResponse.GetResponseStream()))
                    {
                        //The response in JSON format
                        string jsonResponse = readStream.ReadToEnd();

                        JavaScriptSerializer js = new JavaScriptSerializer();

                        lbl_gScore.Text = jsonResponse.ToString();
                        //Create jsonObject to handle the response e.g. success or Error
                        //Deserialize Json
                        MyObject jsonObject = js.Deserialize<MyObject>(jsonResponse);

                        //Convert the string "False" to bool false or "True" to bool true
                        result = Convert.ToBoolean(jsonObject.success);
                    }
                }
                return result;
            }
            catch (WebException ex)
            {
                throw ex;
            }
        }

        protected void btn_register_click(object sender, EventArgs e)
        {
            Response.Redirect("Registration.aspx", false);
        }

        protected void btn_resetPwd_Click(object sender, EventArgs e)
        {
            Response.Redirect("ResetPassword.aspx", false);
        }
    }
}