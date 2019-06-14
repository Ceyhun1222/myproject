using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Encryptor
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                // step 1 Шифровать строку
                string StringToEncrypt = SourceText_Box.Text;
                byte[] rawstring = System.Text.Encoding.Unicode.GetBytes(StringToEncrypt);
                RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
                byte[] encrypted = SigmaEncryptor.Encrypt(rawstring, rsa.ExportParameters(true));
                string EncryptedString = Convert.ToBase64String(encrypted); //For passing to another page

                EncryptedText_Box.Text = EncryptedString; // результат шифрования
                RSAKeyText_Box.Text = rsa.ToXmlString(true); //RSA ключ


                // шаг 2 Сжать RSA ключ
                var cmprs = SigmaEncryptor.CompressString(RSAKeyText_Box.Text);
                CompressedRSAKeyText_Box.Text = SigmaEncryptor.ByteArrayToString(cmprs);

                // шаг 3 Расжать RSA ключ для проверки
                var decmprs = SigmaEncryptor.DecompressString(SigmaEncryptor.StringToByteArray(CompressedRSAKeyText_Box.Text));
                DeCompressedRSAKeyText_Box.Text = decmprs;

                // шаг 4 Decrypt исходного текста на базе RSA ключа из шага 3
                RSACryptoServiceProvider rsa2 = new RSACryptoServiceProvider();
                rsa2.FromXmlString(DeCompressedRSAKeyText_Box.Text);


                byte[] todecrypt = Convert.FromBase64String(EncryptedText_Box.Text);
                byte[] decrypted = SigmaEncryptor.Decrypt(todecrypt, rsa2.ExportParameters(true));
                string decryptedstring = System.Text.Encoding.Unicode.GetString(decrypted);

                CheckText_Box.Text = decryptedstring;

                if (SourceText_Box.Text.CompareTo(CheckText_Box.Text) == 0)
                {
                    FolderBrowserDialog fbD = new FolderBrowserDialog();

                    if (fbD.ShowDialog() == DialogResult.OK)
                    {
                        List<string> res = new List<string>();
                        res.Add(EncryptedText_Box.Text);
                        res.Add(CompressedRSAKeyText_Box.Text);

                        TextWriter tw = new StreamWriter(fbD.SelectedPath +  @"key.sgm");

                        foreach (String s in res)
                            tw.WriteLine(s);

                        tw.Close();

                        MessageBox.Show("File Successfully created " + fbD.SelectedPath + @"key.sgm");
                    }

                }
                else
                    MessageBox.Show("Error");

            }
            catch (ArgumentNullException)
            {
                //Catch this exception in case the encryption did
                //not succeed.
                Console.WriteLine("Encryption failed.");

            }
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            string Y = dateTimePicker1.Value.Year.ToString();
            string M = dateTimePicker1.Value.Month.ToString();
            string D = dateTimePicker1.Value.Day.ToString();

            while (M.Length < 2) M = "0" + M;
            while (D.Length < 2) D = "0" + D;

            SourceText_Box.Text = Y + M + D;
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog()
            {
                Filter = "Files|*.sgm"
            };
            if (ofd.ShowDialog() == DialogResult.OK)
            {

                textBox1.Text = SigmaEncryptor.GetEncryptedDate(ofd.FileName).ToString(); 

            }
        }
    }
}
