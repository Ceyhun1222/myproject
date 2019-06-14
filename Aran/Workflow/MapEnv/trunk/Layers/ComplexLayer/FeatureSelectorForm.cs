using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Aran.Aim;
using Aran.Aim.Metadata.UI;
using Aran.Aim.Features;

namespace MapEnv.ComplexLayer
{
    public partial class FeatureSelectorForm : Form
    {
        private FeatureType _featureType;

        public FeatureSelectorForm ()
        {
            InitializeComponent ();
        }

        public FeatureType FeatureType
        {
            get { return _featureType; }
            set
            {
                _featureType = value;
                Text = "Select " + _featureType;
            }
        }

        public Feature CurrentFeature { get; private set; }

        private void FeatureSelectorForm_Load (object sender, EventArgs e)
        {
            ui_okButton.Enabled = false;

            if (FeatureType == 0)
                return;

            var classInfo = AimMetadata.GetClassInfoByIndex ((int) FeatureType);
            UIUtilities.FillColumns (classInfo, ui_dgv);

            var features = Globals.LoadFeatures (FeatureType, null);

            foreach (var feat in features)
            {
                UIUtilities.SetRow (ui_dgv, feat);
            }
        }

        private void DGV_CurrentCellChanged (object sender, EventArgs e)
        {
            ui_okButton.Enabled = false;

            if (ui_dgv.CurrentRow == null)
            {
                CurrentFeature = null;
                return;
            }

            CurrentFeature = ui_dgv.CurrentRow.Tag as Feature;

            ui_okButton.Enabled = (CurrentFeature != null);
        }

        private void OK_Click (object sender, EventArgs e)
        {
            if (CurrentFeature == null)
                return;

            DialogResult = DialogResult.OK;
        }
    }
}
