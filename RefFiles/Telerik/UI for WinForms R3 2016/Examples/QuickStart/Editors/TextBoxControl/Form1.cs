using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using Telerik.QuickStart.WinControls;

namespace Telerik.Examples.WinControls.Editors.TextBoxControl
{
    public partial class Form1 : ExamplesForm
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void radBtnSetBackgroundImage_Click(object sender, System.EventArgs e)
        {
            using (OpenFileDialog fileDialog = new OpenFileDialog())
            {
                fileDialog.Multiselect = false;
                fileDialog.Filter = "Images (*.BMP;*.JPG;*.GIF)|*.BMP;*.JPG;*.GIF;*.PNG;";

                if (fileDialog.ShowDialog() == DialogResult.OK)
                {
                    Image image = Image.FromStream(fileDialog.OpenFile());
                    this.radTextBoxControl1.TextBoxElement.ViewElement.BackgroundImage = image;
                }
            }
        }

        //protected override void OnSizeChanged(System.EventArgs e)
        //{
        //    base.OnSizeChanged(e);

        //    if (this.radPanel1 != null)
        //    {
        //        float xCoord = ((float)(this.Width - this.radPanel1.Width)) / 2;
        //        float yCoord = ((float)(this.Height - this.radPanel1.Height)) / 2;
        //        this.radPanel1.Location = Point.Round(new PointF(xCoord, yCoord));
        //    }
        //}

        private void radButtonSearch_Click(object sender, System.EventArgs e)
        {
            string text = this.radTextBoxControl1.Text;

            if (!string.IsNullOrEmpty(text))
            {
                string query = string.Format("http://www.bing.com/search?q={0}", text);
                Process.Start(query);
            }
        }

        protected override void WireEvents()
        {
            this.radBtnSetBackgroundImage.Click += new System.EventHandler(this.radBtnSetBackgroundImage_Click);
            this.radButtonSearch.Click += new System.EventHandler(this.radButtonSearch_Click);
        }
    }
}
