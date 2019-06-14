using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Aran.Controls;

namespace MapEnv
{
    public partial class FeatureLayerPropertyForm : Form
    {

        public FeatureLayerPropertyForm ()
        {
            InitializeComponent ();

            //ui_filterControl.FeatureDescription += Globals.AimPropertyControl_FeatureDescription;
            //ui_filterControl.FillDataGridColumnsHandler = Globals.AimPropertyControl_FillDataGridColumn;
            //ui_filterControl.SetDataGridRowHandler = Globals.AimPropertyControl_SetRow;
            ui_filterControl.LoadFeatureListByDependHandler = Globals.GetFeatureListByDepend;
        }


        public string LayerName
        {
            get { return ui_layerNameTB.Text; }
            set { ui_layerNameTB.Text = value; }
        }

        public FeatureStyleControl StyleControl
        {
            get { return ui_styleControl; }
        }

        public FilterControl FilterControl
        {
            get { return ui_filterControl; }
        }


        private void OK_Click (object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        private void Cancel_Click (object sender, EventArgs e)
        {
            Close ();
        }
    }
}
