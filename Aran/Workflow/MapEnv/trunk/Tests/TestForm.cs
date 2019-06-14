using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Aran.Aim;
using Aran.Aim.Data.Filters;

namespace MapEnv
{
    public partial class TestForm : Form
    {
        public TestForm ()
        {
            InitializeComponent ();

            filterControl1.FeatureDescription += Globals.AimPropertyControl_FeatureDescription;
            filterControl1.FillDataGridColumnsHandler = Globals.AimPropertyControl_FillDataGridColumn;
            filterControl1.SetDataGridRowHandler = Globals.AimPropertyControl_SetRow;
            filterControl1.LoadFeatureListByDependHandler = Globals.GetFeatureListByDepend;

            filterControl1.SetFilter (FeatureType.RunwayCentrelinePoint);
        }

        private void ShowNew_Click (object sender, EventArgs e)
        {
            var filter = filterControl1.GetFilter ();

            TestForm newForm = new TestForm ();
            newForm.filterControl1.SetFilter (filterControl1.FeatureType, filter);
            newForm.Show ();
        }
    }
}
