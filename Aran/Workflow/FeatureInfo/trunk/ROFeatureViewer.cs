using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aran.Aim.Features;
using System.Windows.Forms;

namespace Aran.Aim.FeatureInfo
{
    public class ROFeatureViewer : IDisposable
    {
        public event EventHandler FormHiden;

        public ROFeatureViewer ()
        {
            _mapInfoForm = new FeatureInfoTransparentForm ();
            _mapInfoForm.VisibleChanged += MapInfoForm_VisibleChanged;
        }

        public GetFeatureHandler GettedFeature
        {
            get { return Global.GettedFeature; }
            set { Global.GettedFeature = value; }
        }

        public void Dispose ()
        {
            _mapInfoForm.Close ();
            _mapInfoForm.Dispose ();
        }

        public void SetOwner (IWin32Window owner)
        {
            _owner = owner;
        }

        public void MapControl_VisibleChanged (object sender, EventArgs e)
        {
            Control mapControl = sender as Control;
            if (!mapControl.Visible)
            {
                if (_mapInfoForm.Visible)
                {
                    _mapInfoForm.Visible = false;
                    _hidedByMapVisible = true;
                }
            }
            else if (_hidedByMapVisible)
            {
                _mapInfoForm.Visible = true;
                _hidedByMapVisible = false;
            }

        }

        public void ShowMapFeatures (IEnumerable<Feature> featureList, System.Drawing.Point foothold)
        {
            Form ownerForm = (_owner as Form);
            ownerForm.LocationChanged += new EventHandler (OwnerForm_LocationChanged);
            ownerForm.SizeChanged += new EventHandler (OwnerForm_SizeChanged);
            _mapInfoForm.Location = ownerForm.Location;
            _mapInfoForm.Size = ownerForm.Size;

            _mapInfoForm.FootHold = foothold;
            _mapInfoForm.Visible = false;
            _mapInfoForm.SetFeatures (featureList);
            _mapInfoForm.Show (_owner);
        }

        public bool ShowFeaturesForm(IEnumerable<Feature> featureList, bool inNewForm = true, bool showOkButton = false)
        {
            FeatureInfoForm infoForm;

            if (inNewForm) {
                infoForm = new FeatureInfoForm();
                infoForm.CurrentFeatureChanged = _mapInfoForm.CurrentFeatureChanged;
                infoForm.FormClosed += InfoForm_VisibleChanged;
            }
            else {
                if (_infoForm == null || _infoForm.IsDisposed) {
                    _infoForm = new FeatureInfoForm();
                    _infoForm.CurrentFeatureChanged = _mapInfoForm.CurrentFeatureChanged;
                    //_infoForm.VisibleChanged += MapInfoForm_VisibleChanged;
                    _infoForm.FormClosed += InfoForm_VisibleChanged;
                }

                infoForm = _infoForm;
            }

            infoForm.SetFeatures(featureList, showOkButton);

            if (showOkButton) {
                infoForm.ShowDialog();
                return infoForm.OKClicked;
            }
            else if (!infoForm.Visible) {
                infoForm.Show(_owner);
            }
            return false;
        }

        public event EventHandler CurrentFeatureChanged
        {
            add
            {
                _mapInfoForm.CurrentFeatureChanged = new EventHandler (value);
            }
            remove
            {
                _mapInfoForm.CurrentFeatureChanged = null;
            }
        }

        private void OwnerForm_LocationChanged (object sender, EventArgs e)
        {
            _mapInfoForm.Location = (sender as Form).Location;
        }

        private void OwnerForm_SizeChanged (object sender, EventArgs e)
        {
            _mapInfoForm.Size = (sender as Form).Size;
        }

        private void MapInfoForm_VisibleChanged (object sender, EventArgs e)
        {
			var form = sender as Form;

            if (!form.Visible)
            {
                if (FormHiden != null)
                    FormHiden (sender, e);
            }
        }

		private void InfoForm_VisibleChanged (object sender, EventArgs e)
		{
			if (FormHiden != null)
				FormHiden (sender, e);
		}

        private FeatureInfoTransparentForm _mapInfoForm;
        private IWin32Window _owner;
        private bool _hidedByMapVisible;
        private FeatureInfoForm _infoForm;
    }

    public delegate Feature GetFeatureHandler (FeatureType featureType, Guid identifier);
}
