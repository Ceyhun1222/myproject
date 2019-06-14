using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Aran.Aim.Metadata;

namespace Aran.Aim.FeatureInfo
{
	public partial class MetadataViewerForm : Form
	{
		public MetadataViewerForm ()
		{
			InitializeComponent ();
		}

		public void SetValue (FeatureTimeSliceMetadata value)
		{
			ui_featureTSMDCont.SetValue (value);
		}
	}
}
