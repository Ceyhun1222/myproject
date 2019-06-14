using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AIP.DataSet.Properties;
using Telerik.WinControls.UI;

namespace AIP.DataSet
{
    public partial class Splash : ShapedForm
    {
        public Splash()
        {
            InitializeComponent();

            PictureBox spashPictureBox = new PictureBox();
            spashPictureBox.Width = 370;
            spashPictureBox.Height = 100;
            spashPictureBox.Image = Resources.Splash;
            spashPictureBox.SizeMode = PictureBoxSizeMode.AutoSize;
            spashPictureBox.Dock = DockStyle.Fill;
            this.Controls.Add(spashPictureBox);

            this.StartPosition = FormStartPosition.CenterScreen;
        }
    }
}
