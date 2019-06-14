using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Aran.Aim;

namespace MapEnv.Plugin
{
    public partial class FeatureTypeSelectorForm : Form
    {
        private List<FeatureType> _selectedFeatureTypes;

        public FeatureTypeSelectorForm ()
        {
            InitializeComponent ();

            _selectedFeatureTypes = new List<FeatureType> ();
        }

        public List<FeatureType> SelectedFeatureTypes
        {
            get { return _selectedFeatureTypes; }
        }

        public void AddFeatureType (FeatureType featureType, int count)
        {
            ui_listView.Items.Add (string.Format ("{0} ({1})", featureType, count)).Tag = featureType;
        }

        private void okButton_Click (object sender, EventArgs e)
        {
            _selectedFeatureTypes.Clear ();

            foreach (ListViewItem lvi in ui_listView.CheckedItems)
                _selectedFeatureTypes.Add ((FeatureType) lvi.Tag);

            DialogResult = DialogResult.OK;
        }
    }
}
