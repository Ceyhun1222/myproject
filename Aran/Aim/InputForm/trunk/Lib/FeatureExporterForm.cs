using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Aran.Aim.Features;

namespace Aran.Aim.InputFormLib
{
    public partial class FeatureExporterForm : Form
    {
        private Dictionary<FeatureType, List<Guid>> _selectedFeatures;

        public FeatureExporterForm ()
        {
            InitializeComponent ();
        }

        public void SetSelectedFeatures (Dictionary <FeatureType, List<Guid>> selectedFeatures)
        {
            _selectedFeatures = selectedFeatures;
        }

        public string FileName { get { return ui_fileNameTB.Text; } }

        public bool IsAllFeatures { get { return ui_exportTypeAllFeaturesRB.Checked; } }


        private void SelectFile_Click (object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog ();
            sfd.Filter = "XML Files (*.xml)|*.xml";
            if (sfd.ShowDialog () != DialogResult.OK)
            {
                return;
            }

            ui_fileNameTB.Text = sfd.FileName;
        }

        private void OK_Click (object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        private void ExportType_CheckedChanged (object sender, EventArgs e)
        {
            bool isVisible = (sender == ui_exportTypeSelFeaturesRB);
            ui_FeaturesDGV.Visible = isVisible;

            if (isVisible)
                FillExportingFeatures ();

            ChangeOKEnabled ();

            ui_clearList.Visible = isVisible;
        }

        private void FillExportingFeatures ()
        {
            if (ui_FeaturesDGV.Rows.Count > 0)
                return;

            foreach (FeatureType featType in _selectedFeatures.Keys)
            {
                List<Guid> identifierList = _selectedFeatures [featType];

                foreach (Guid identifier in identifierList)
                {
                    var index = ui_FeaturesDGV.Rows.Add ();
                    var row = ui_FeaturesDGV.Rows [index];
                    row.Cells [ui_colFeatureType.Index].Value = featType;
                    row.Cells [ui_colIdentifier.Index].Value = identifier;
                }
            }
        }

        private void FileNameTB_TextChanged (object sender, EventArgs e)
        {
            ChangeOKEnabled ();
        }

        private void ChangeOKEnabled ()
        {
            if (string.IsNullOrWhiteSpace (ui_fileNameTB.Text))
            {
                ui_okButton.Enabled = false;
                return;
            }

            if (ui_exportTypeAllFeaturesRB.Checked)
            {
                ui_okButton.Enabled = true;
                return;
            }

            ui_okButton.Enabled = (ui_FeaturesDGV.Rows.Count > 0);
        }

        private void ClearList_Click (object sender, EventArgs e)
        {
            _selectedFeatures.Clear ();
            ui_FeaturesDGV.Rows.Clear ();
            ChangeOKEnabled ();
        }
    }
}
