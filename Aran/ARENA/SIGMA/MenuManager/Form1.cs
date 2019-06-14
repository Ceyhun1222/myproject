using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MenuManager
{
    public partial class Form1 : Form
    {

        string[] alfabet = { "D", "2","S", "A", "F","3", "G", "H","4", "J", "K", "L", "P", "O","5", "I", "U", "Y","6", "T", "R", "E","7", "W", "Q", "V", "B", "N", "M", "C", "X", "Z","8" };

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string gg = Guid.NewGuid().ToString();

            textBox1.Text="";

            string[] ar = gg.Split('-');
            ar[0] = ar[0].Substring(0, ar[0].Length - 1) + SetCode(checkedListBox1);
            ar[1] = ar[1].Substring(0, ar[1].Length - 1) + SetCode(checkedListBox2);

            for (int i = 0; i <= ar.Length-2; i++)
            {
                textBox1.Text = textBox1.Text + ar[i] + "-";
            }
            textBox1.Text = textBox1.Text + "-" + ar[ar.Length - 1];
        }

        public string ConvertToBinaryString(int x)
        {
            char[] bits = new char[32];
            int i = 0;

            while (x != 0)
            {
                bits[i++] = (x & 1) == 1 ? '1' : '0';
                x >>= 1;
            }

            //Array.Reverse(bits, 0, i);
            return new string(bits);
        }

        public string SetCode(CheckedListBox lstbx)
        {

            double res = 0;

            for (int i = 0; i < lstbx.Items.Count; i++)
            {
                if (lstbx.GetItemChecked(i)) res = res + Math.Pow(2, i);
            }

            return alfabet[Convert.ToInt32(res)];
        }

        public string DeCode(string Code)
        {
            char[] arr = Code.ToCharArray();
            List<string> ALFB= alfabet.ToList();
            string res = "";

            foreach (char item in arr)
            {
                int indx = ALFB.IndexOf(item.ToString());
                MessageBox.Show( ConvertToBinaryString(indx));
            }

            return res;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DeCode(textBox1.Text);
        }
    }
}
