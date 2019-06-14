using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Aran.Aim.Features;
using System.Windows.Interop;
using System.Windows.Media;

namespace Aran.Aim.FeatureInfo
{
    public partial class FeatureInfoForm : Form
    {
		internal EventHandler CurrentFeatureChanged;


        public FeatureInfoForm ()
        {
            InitializeComponent ();

            OKClicked = false;
            ui_featureContainerCont.HideTopPanel ();
			ui_featureContainerCont.PropertyChanged += FeatureContainer_PropertyChanged;
        }


		public void SetFeatures (IEnumerable<Feature> featureList, bool showOkButton = false)
        {
            ui_featureContainerCont.SetFeature(featureList);

            if (showOkButton) {
                ui_okButton.Visible = true;
                ui_closeButton.Text = "Cancel";
            }
        }

        public bool OKClicked { get; private set; }


        private void Close_Click (object sender, EventArgs e)
        {
            Close ();
        }

        private void OK_Click (object sender, EventArgs e)
        {
            OKClicked = true;
            Close ();
        }

		private void FeatureContainer_PropertyChanged (object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == "CurrentFeature")
			{
                if (ui_featureContainerCont.CurrentFeaure != null)
                    Text = ui_featureContainerCont.CurrentFeaure.FeatureType + " - Info";

				if (CurrentFeatureChanged != null)
					CurrentFeatureChanged (ui_featureContainerCont.CurrentFeaure, null);
			}
		}
    }
}
