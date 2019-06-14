using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MapEnv
{
    public partial class AixmExportOptionsForm : Form
    {
        public AixmExportOptionsForm()
        {
            InitializeComponent();

            ui_srsNameCB.Items.Add("EPSG_4326");
            ui_srsNameCB.Items.Add("CRS84");
            ui_srsNameCB.SelectedIndex = 0;
        }

        public string FileName
        {
            get { return ui_fileNameTB.Text; }
            set { ui_fileNameTB.Text = value; }
        }

        public Aran.Aim.AixmMessage.SrsNameType SrsType
        {
            get
            {
                return ui_srsNameCB.SelectedIndex == 0 ? Aran.Aim.AixmMessage.SrsNameType.EPSG_4326 : Aran.Aim.AixmMessage.SrsNameType.CRS84;
            }
            set
            {
                ui_srsNameCB.SelectedIndex = (value == Aran.Aim.AixmMessage.SrsNameType.EPSG_4326 ? 0 : 1);
            }
        }

        public bool IsWriteExtensions
        {
            get { return ui_writeExtensionChB.Checked; }
            set { ui_writeExtensionChB.Checked = value; }
        }

        public bool Write3DIfExists
        {
            get { return ui_write3DisExistsChB.Checked; }
            set { ui_write3DisExistsChB.Checked = value; }
        }


        private void SelectFile_Click(object sender, EventArgs e)
        {
            var sfd = new SaveFileDialog();
            sfd.Filter = "XML Files (*.xml)|*.xml";
            if (sfd.ShowDialog() != DialogResult.OK)
                return;

            ui_fileNameTB.Text = sfd.FileName;
        }

        private void FileName_TextChanged(object sender, EventArgs e)
        {
            ui_okButton.Enabled = (ui_fileNameTB.Text.Length > 0);
        }

        private void OK_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }
    }
}
