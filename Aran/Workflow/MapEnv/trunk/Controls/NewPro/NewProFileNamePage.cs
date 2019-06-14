using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MapEnv
{
    public partial class NewProFileNamePage : UserControl
    {
        public NewProFileNamePage()
        {
            InitializeComponent();
        }


        public string FileName
        {
            get { return ui_fileNameTB.Text; }
        }

        public string ErrorMessage
        {
            get { return ui_errorLabel.Text; }
            set
            {
                ui_errorLabel.Text = value;
                ui_errorLabel.Visible = true;
            }
        }


        private void SelectFile_Click(object sender, EventArgs e)
        {
            var sfd = new SaveFileDialog();
            sfd.Filter = Globals.OpenFileFilters;
            if (sfd.ShowDialog() != DialogResult.OK)
                return;
            ui_fileNameTB.Text = sfd.FileName;
        }
    }
}
