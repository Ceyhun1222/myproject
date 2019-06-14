using ChartingManagerWeb.ServiceReference1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace ChartingManagerWeb.Models
{
    public class LoginControl
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public LoginControl CheckFunction(string username, string password, ChartManagerServiceClient client)
        {
            //SqlConnection con = new SqlConnection(@"Data Source=NASIMIA\SQLEXPRESS;Initial Catalog=RiskUsers;Integrated Security=true");
            //LoginControl log = new LoginControl();
            //con.Open();
            //SqlCommand command = new SqlCommand("select Username,UserPassword from UserControl where Username='" + username + "' and UserPassword='" + password + "'", con);
            //SqlDataReader read = command.ExecuteReader();
            LoginControl log = new LoginControl();
            try
            {
                client.ClientCredentials.UserName.UserName = username;
                client.ClientCredentials.UserName.Password = Sha256_hash(password);
                client.Login();
                log.Username = username;
                //log.Password = password;
            }
            catch (Exception ex)
            {
                //return null;
                log.Username = null;
            }

            //log.Password = Convert.ToString(read[1]);

            return log;
        }
        public static String Sha256_hash(String value)
        {
            StringBuilder Sb = new StringBuilder();

            using (SHA256 hash = SHA256.Create())
            {
                Encoding enc = Encoding.UTF8;
                Byte[] result = hash.ComputeHash(enc.GetBytes(value));

                foreach (Byte b in result)
                    Sb.Append(b.ToString("x2"));
            }

            return Sb.ToString();
        }
    }
}