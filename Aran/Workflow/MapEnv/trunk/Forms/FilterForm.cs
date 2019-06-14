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
using Aran.Queries.Common;

namespace MapEnv.Forms
{
	public partial class FilterForm : Form
	{
		public FilterForm ()
		{
			InitializeComponent ();
		}

		public void SetFilter (FeatureType featType, Filter value, FeatureListByDependEventHandler loadFeatListByDepend)
		{
			ui_filterControl.SetFilter (featType, value);
            ui_filterControl.LoadFeatureListByDependHandler = loadFeatListByDepend;
		}

		public Filter GetFilter ()
		{
			return ui_filterControl.GetFilter ();
		}

		private void OK_Click (object sender, EventArgs e)
		{
			DialogResult = DialogResult.OK;
		}
	}
}
