using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;
using System.Drawing;
using System.Security.Cryptography;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace SITCONNECT
{
    public partial class ResetPassword : System.Web.UI.Page
    {
        string MYDBConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MYDBConnection"].ConnectionString;
        static string finalHash;
        static string salt;
        byte[] Key;
        byte[] IV;

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btn_register_Click(object sender, EventArgs e)
        {
            int scores = checkPassword(tb_pwd.Text);
            string status = "";
            switch (scores)
            {
                case 1:
                    status = "Very Weak";
                    break;
                case 2:
                    status = "Weak";
                    break;
                case 3:
                    status = "Medium";
                    break;
                case 4:
                    status = "Strong";
                    break;
                case 5:
                    status = "Excellent";
                    break;
                default:
                    break;
            }
            lbl_pwdchecker.Text = "Status:" + status;
            if (scores < 4)
            {
                lbl_pwdchecker.ForeColor = Color.Red;
                return;
            }
            lbl_pwdchecker.ForeColor = Color.Green;

            string email = tb_email.Text.ToString();
            string pwd = tb_pwd.Text.ToString();
            SqlConnection con = new SqlConnection(MYDBConnectionString);
            con.Open();
            SqlCommand cmd = new SqlCommand("Select * from Account where Email=@Email", con);
            cmd.Parameters.AddWithValue("@Email", tb_email.Text);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            if (ds != null)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataRow row = ds.Tables[0].Rows[0];  // Sql command returns only one record
                    string Email = row["Email"].ToString();

                    if (Email.Equals(email))
                    {
                        lbl_email.Text = Email + email;
                    //    RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
                    //    byte[] saltByte = new byte[8];

                    //    rng.GetBytes(saltByte);
                    //    salt = Convert.ToBase64String(saltByte);

                    //    SHA512Managed hashing = new SHA512Managed();

                    //    string pwdWithSalt = pwd + salt;
                    //    byte[] plainHash = hashing.ComputeHash(Encoding.UTF8.GetBytes(pwd));
                    //    byte[] hashWithSalt = hashing.ComputeHash(Encoding.UTF8.GetBytes(pwdWithSalt));

                    //    finalHash = Convert.ToBase64String(hashWithSalt);

                    //    lbl_error.Text = "K" + Key + "IV" + IV + "FH" + finalHash + "Salt" + salt;
                    //    RijndaelManaged cipher = new RijndaelManaged();
                    //    cipher.GenerateKey();
                    //    Key = cipher.Key;
                    //    IV = cipher.IV;
                    //    string VIV = Convert.ToBase64String(IV);
                    //    string VKey = Convert.ToBase64String(Key);

                    //    cmd = new SqlCommand("Update Account set PasswordHash=" + finalHash + ",PasswordSalt=" + salt + ",IV=" + VIV + ",Key=" + VKey + " where Email=@Email", con);
                    //    cmd.ExecuteNonQuery();

                    //    lbl_error.Text = "Password changed successfully";
                    //    lbl_error.ForeColor = Color.Green;
                    //}
                    //else
                    //{
                    //    lbl_error.Text = "Email or password is not valid. please try again.";
                    //    lbl_error.ForeColor = Color.Red;
                    }
                }
            }
        }
        protected string getDBHash(string email)
        {
            string h = null;

            SqlConnection connection = new SqlConnection(MYDBConnectionString);
            string sql = "select PasswordHash FROM Account WHERE Email=@Email";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@Email", email);

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

        protected void btn_login_Click(object sender, EventArgs e)
        {
            Response.Redirect("Login.aspx", false);
        }

        private int checkPassword(string password)
        {
            int score = 0;

            //include your impementation here


            //if password length is less than 8 characters
            if (password.Length < 8)
            {
                return 1;
            }
            else
            {
                score = 1;
            }
            //if password contains lowercase letter(s)
            if (Regex.IsMatch(password, "[a-z]"))
            {
                score++;
            }
            //if password contains uppercase letter(s)
            if (Regex.IsMatch(password, "[A-Z]"))
            {
                score++;
            }
            //if password contains numeral(s)
            if (Regex.IsMatch(password, "[0-9]"))
            {
                score++;
            }
            //if password contains special character(s)
            if (Regex.IsMatch(password, "[^A-Za-z0-9_]"))
            {
                score++;
            }
            return score;
        }

        protected byte[] encryptData(string data)
        {
            byte[] cipherText = null;
            try
            {
                RijndaelManaged cipher = new RijndaelManaged();
                cipher.IV = IV;
                cipher.Key = Key;
                ICryptoTransform encryptTransform = cipher.CreateEncryptor();
                //ICryptoTransform decryptTransform = cipher.CreateDecryptor();
                byte[] plainText = Encoding.UTF8.GetBytes(data);
                cipherText = encryptTransform.TransformFinalBlock(plainText, 0, plainText.Length);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally { }
            return cipherText;
        }
    }
}