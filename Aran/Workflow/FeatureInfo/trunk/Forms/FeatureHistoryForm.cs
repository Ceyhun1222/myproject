using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Aran.Aim.Features;

namespace Aran.Aim.FeatureInfo
{
	public partial class FeatureHistoryForm : Form
	{
		private FeatureHistoryControlW _featHC;

		public FeatureHistoryForm ()
		{
			InitializeComponent ();

			_featHC = elementHost1.Child as FeatureHistoryControlW;
			_featHC.CloseClicked += FeatHC_CloseClicked;
		}

		public GetFeatureHandler GettedFeature
		{
			get { return Global.GettedFeature; }
			set { Global.GettedFeature = value; }
		}

		private void FeatHC_CloseClicked (object sender, EventArgs e)
		{
			Close ();
		}

		public void Open (List<Feature> features)
		{
			_featHC.Model.Open (features);
			_featHC.SelectFirst ();
		}
	}
}
