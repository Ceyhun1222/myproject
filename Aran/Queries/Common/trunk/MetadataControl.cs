using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;

namespace Aran.Queries.Common
{
    public partial class MetadataControl : UserControl
    {
        public MetadataControl ()
        {
            InitializeComponent ();
        }

        public Aim.Metadata.FeatureTimeSliceMetadata Metadata
        {
			get
			{
				return ui_featureTSMDCont.GetValue ();
			}
            set
            {
				ui_featureTSMDCont.SetValue (value);
            }
        }
    }
}
