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
    public partial class Registration : System.Web.UI.Page
    {
        string MYDBConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MYDBConnection"].ConnectionString;
        static string finalHash;
        static string salt;
        byte[] Key;
        byte[] IV;
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btn_login_Click(object sender, EventArgs e)
        {
            Response.Redirect("Login.aspx", false);
        }

        protected void btn_register_Click(object sender, EventArgs e)
        {
            HttpUtility.HtmlEncode(tb_fname.Text);
            HttpUtility.HtmlEncode(tb_lname.Text);
            HttpUtility.HtmlEncode(tb_bdate.Text);
            HttpUtility.HtmlEncode(tb_cardNo.Text);
            HttpUtility.HtmlEncode(tb_expiryDate.Text);
            HttpUtility.HtmlEncode(tb_cvc.Text);
            HttpUtility.HtmlEncode(tb_email.Text);

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

            string pwd = tb_pwd.Text.ToString();

            //Generate random "salt"
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            byte[] saltByte = new byte[8];


            //Fills array of bytes with a cryptogtaphically strong sequence of random values.
            rng.GetBytes(saltByte);
            salt = Convert.ToBase64String(saltByte);

            SHA512Managed hashing = new SHA512Managed();

            string pwdWithSalt = pwd + salt;
            byte[] plainHash = hashing.ComputeHash(Encoding.UTF8.GetBytes(pwd));
            byte[] hashWithSalt = hashing.ComputeHash(Encoding.UTF8.GetBytes(pwdWithSalt));

            finalHash = Convert.ToBase64String(hashWithSalt);

            RijndaelManaged cipher = new RijndaelManaged();
            cipher.GenerateKey();
            Key = cipher.Key;
            IV = cipher.IV;
            createAccount();

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

        public void createAccount()
        {
            int attempts = 0;
            int isLocked = 0;
            try
            {
                using (SqlConnection con = new SqlConnection(MYDBConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("INSERT INTO Account VALUES(@Firstname, @Lastname, @Birthdate, @CardNo, @ExpiryDate, @Cvc, @Email, @PasswordHash, @PasswordSalt, @IV, @Key, @IsLocked, @AttemptCount)"))
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            cmd.CommandType = CommandType.Text;
                            cmd.Parameters.AddWithValue("@FirstName", tb_fname.Text.Trim());
                            cmd.Parameters.AddWithValue("@LastName", tb_lname.Text.Trim());
                            cmd.Parameters.AddWithValue("@Birthdate", tb_bdate.Text.Trim());
                            cmd.Parameters.AddWithValue("@CardNo", Convert.ToBase64String(encryptData(tb_cardNo.Text.Trim())));
                            cmd.Parameters.AddWithValue("@ExpiryDate", Convert.ToBase64String(encryptData(tb_expiryDate.Text.Trim())));
                            cmd.Parameters.AddWithValue("@Cvc", Convert.ToBase64String(encryptData(tb_cvc.Text.Trim())));
                            cmd.Parameters.AddWithValue("@Email", tb_email.Text.Trim());
                            cmd.Parameters.AddWithValue("@PasswordHash", finalHash);
                            cmd.Parameters.AddWithValue("@PasswordSalt", salt);
                            cmd.Parameters.AddWithValue("@IV", Convert.ToBase64String(IV));
                            cmd.Parameters.AddWithValue("@Key", Convert.ToBase64String(Key));
                            cmd.Parameters.AddWithValue("@IsLocked", isLocked);
                            cmd.Parameters.AddWithValue("@AttemptCount", attempts);
                            cmd.Connection = con;
                            con.Open();
                            cmd.ExecuteNonQuery();
                            con.Close();

                            lbl_msg.Text = "Registration completed sucessfully!";
                            lbl_msg.ForeColor = Color.Green;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
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