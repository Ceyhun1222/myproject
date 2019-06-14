using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SigmaChartExamination
{
    public partial class ExaminationresultsForm : Form
    {
        public ExaminationresultsForm()
        {
            InitializeComponent();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog flD = new SaveFileDialog();
                flD.Filter = "Text file|*.txt";
                flD.Title = "Save File";
                if (flD.ShowDialog() == DialogResult.OK && flD.FileName!= "")
                {
                    List<string> list = new List<string>();

                    foreach (var itm in listBox1.Items)
                    {
                        list.Add(itm.ToString());
                    }


                    System.IO.File.WriteAllLines(flD.FileName, list.ToArray());
                    //MessageBox.Show("Saved");
                }

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }

            this.Close();
        }
    }
}
