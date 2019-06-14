using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Aran.Aim.Features;
using System.IO;

namespace Aran.Aim.InputFormLib
{
    public partial class FeatureExporterForm : Form
    {
        private Dictionary<FeatureType, List<Feature>> _selectedFeatures;


        public FeatureExporterForm ()
        {
            InitializeComponent ();

            ui_srsNameCB.Items.Add("EPSG_4326");
            ui_srsNameCB.Items.Add("CRS84");
            ui_srsNameCB.SelectedIndex = 0;
        }

        public event EventHandler ExportAsSeperatedFileClicked;


        public bool IsWriteExtensions
        {
            get { return ui_writeExtensionChB.Checked; }
        }

		public bool IncludeFeatRefs
		{
			get
			{
				return chckBxIncludeFeatRefs.Checked;
			}
		}

        public bool Write3DIfExists { get { return ui_write3DisExistsChB.Checked; } }

        public void SetSelectedFeatures (Dictionary <FeatureType, List<Feature>> selectedFeatures)
        {
            _selectedFeatures = selectedFeatures;
        }

        public void SetEffectiveDate(DateTime dt)
        {
            var adt = Aran.Controls.Airac.AiracCycle.CreateAiracDateTime(dt);

            ui_effectiveDateTB.Text = string.Format("{0}: {1:yyyy - MM - dd HH:mm}{2}",
                adt.Mode == AranEnvironment.AiracSelectionMode.Airac ? "AIRAC" : "Custom",
                adt.Value,
                adt.Mode == AranEnvironment.AiracSelectionMode.Airac ? "" : "  UTC");
        }

        public string FileName { get { return ui_fileNameTB.Text; } }

        public bool IsAllFeatures { get { return ui_exportTypeAllFeaturesRB.Checked; } }

		public bool LoadFeatureAllVersion
		{
			get { return ui_loadFeatAllVersion.Checked; }
		}

        public Aran.Aim.AixmMessage.SrsNameType SrsType
        {
            get
            {
                return ui_srsNameCB.SelectedIndex == 0 ? Aran.Aim.AixmMessage.SrsNameType.EPSG_4326 : Aran.Aim.AixmMessage.SrsNameType.CRS84;
            }
        }


        private void FeatureExporterForm_Load(object sender, EventArgs e)
        {
            ui_exportTypeSelFeaturesRB.Enabled = (_selectedFeatures != null && _selectedFeatures.Count > 0);
        }

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

			ui_loadFeatAllVersion.Visible = ui_exportTypeAllFeaturesRB.Checked;
            ui_expAsSeperateButton.Visible = ui_exportTypeSelFeaturesRB.Checked && ui_FeaturesDGV.Rows.Count > 0;
        }

        private void FillExportingFeatures ()
        {
            if (ui_FeaturesDGV.Rows.Count > 0)
                return;

            foreach (FeatureType featType in _selectedFeatures.Keys)
            {
                var featList = _selectedFeatures [featType];

                foreach (var feat in featList)
                {
                    var index = ui_FeaturesDGV.Rows.Add ();
                    var row = ui_FeaturesDGV.Rows [index];
                    row.Cells [ui_colFeatureType.Index].Value = feat.FeatureType;
                    row.Cells [ui_colIdentifier.Index].Value = feat.Identifier;
                    
                    bool hasDesc;
                    var str = Aran.Aim.Metadata.UI.UIUtilities.GetFeatureDescription(feat, out hasDesc);
                    row.Cells[ui_colDesc.Index].Value = (hasDesc ? str : "");
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

            ui_expAsSeperateButton.Visible = false;
        }

        private void ExportAsSeperate_Click(object sender, EventArgs e)
        {
            if (ExportAsSeperatedFileClicked != null)
                ExportAsSeperatedFileClicked(this, e);
        }

        
    }
}
