using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Aran.Aim.Data.Filters;
using Aran.Aim;
using Aran.Aim.Env.Layers;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Geometry;
using Aran.Package;
using ESRI.ArcGIS.esriSystem;

namespace MapEnv
{
    public partial class TestFilterForm : Form
    {
        public TestFilterForm ()
        {
            InitializeComponent ();
        }

        public Filter CreateFilter ()
        {
            ComparisonOps compOper = new ComparisonOps (ComparisonOpType.EqualTo,
                "associatedAirportHeliport", new Guid ());
            OperationChoice operChoice = new OperationChoice (compOper);
            
            return new Filter (operChoice);
        }

        private void TestFilterForm_Load (object sender, EventArgs e)
        {
            filterControl1.SetFilter (FeatureType.Runway);
        }

        private void TEST (object sender, EventArgs e)
        {
            Filter filter = filterControl1.GetFilter ();
            
        }
    }
}
