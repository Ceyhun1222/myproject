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

namespace AUSCreator
{
    public partial class MainForm : Form
    {
        private bool _rtfChanged;

        public MainForm()
        {
            InitializeComponent();
        }


        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (ui_changesRTB.Text.Length > 0 && _rtfChanged)
            {
                e.Cancel = (MessageBox.Show("Do you want to exit?",
                    Text, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) != DialogResult.Yes);
            }
        }

        private void Select7zBinFile_Click(object sender, EventArgs e)
        {
            var ofd = new OpenFileDialog();
            ofd.Filter = "Compressed File (*.7z)|*.7z";

            if (ofd.ShowDialog() != DialogResult.OK)
                return;

            ui_bin7zFileTB.Text = ofd.FileName;
        }

        private void Create_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(ui_versionNameTB.Text))
            {
                MessageBox.Show("Version Name is empty!",
                    Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!File.Exists(ui_bin7zFileTB.Text))
            {
                MessageBox.Show("Compressed Bin File is not exists!",
                    Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var sfd = new SaveFileDialog();
            sfd.Filter = "Aran Update Source (*.aus)|*.aus";
            sfd.FileName = ui_versionNameTB.Text;

            if (sfd.ShowDialog() != DialogResult.OK)
                return;

            var aus = new Aus();
            aus.VersionName = ui_versionNameTB.Text;
            aus.ReleaseDate = ui_releaseDatePiker.Value;
            aus.ChangesRTF = ui_changesRTB.Rtf;
            aus.BinFile = File.ReadAllBytes(ui_bin7zFileTB.Text);

            if (File.Exists(sfd.FileName))
                File.Delete(sfd.FileName);

            using (var fs = File.Create(sfd.FileName))
            {
                aus.Pack(fs);
                fs.Close();
            }
            
            _rtfChanged = false;

            MessageBox.Show("Aran Update Source file successfully created.\nFile name: " + sfd.FileName,
                Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void Close_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Open_Click(object sender, EventArgs e)
        {
            var ofd = new OpenFileDialog();
            ofd.Filter = "Aran Update Source (*.aus)|*.aus";
            if (ofd.ShowDialog() != DialogResult.OK)
                return;

            var binfileName = Path.Combine(Path.GetDirectoryName(ofd.FileName), Path.GetFileNameWithoutExtension(ofd.FileName) + "_bin.7z");
            if (File.Exists(binfileName))
            {
                MessageBox.Show("Bin file already exists: " + binfileName,
                    Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (var fs = File.Open(ofd.FileName, FileMode.Open))
            {
                var aus = new Aus();
                aus.Unpack(fs);
                fs.Close();

                ui_versionNameTB.Text = aus.VersionName;
                ui_releaseDatePiker.Value = aus.ReleaseDate;
                ui_changesRTB.Rtf = aus.ChangesRTF;

                File.WriteAllBytes(binfileName, aus.BinFile);
            }

            _rtfChanged = false;
        }

        private void ChangesRTB_TextChanged(object sender, EventArgs e)
        {
            _rtfChanged = true;
        }
    }
}
