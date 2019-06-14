using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Aran.Aim.Env.Layers;

namespace MapEnv
{
    public partial class FeatureLayerPropertyControl : UserControl
    {
        private AimFeatureLayer _aimFeatureLayer;

        public event EventHandler Saved;
        public event CommandEventHandler CancelClicked;


        public FeatureLayerPropertyControl ()
        {
            InitializeComponent ();
        }

        public FeatureStyleControl StyleControl
        {
            get { return ui_styleControl; }
        }

        public FilterControl FilterControl
        {
            get { return ui_filterControl; }
        }

        public AimFeatureLayer AimFeatureLayer
        {
            get
            {
                return _aimFeatureLayer;
            }
            set
            {
                _aimFeatureLayer = value;
                AimTable aimTable = value.AimTable;

                ui_styleControl.SetShapeInfos (
                    aimTable.FeatureType,
                    aimTable.ShapeInfoList);

                ui_filterControl.SetFilter (aimTable.FeatureType, aimTable.Filter);
            }
        }


        private void ui_saveButton_Click (object sender, EventArgs e)
        {
            if (Saved != null)
                Saved (this, null);
        }

        private void ui_cancelButton_Click (object sender, EventArgs e)
        {
            if (CancelClicked != null)
                CancelClicked ();
        }
    }
}
