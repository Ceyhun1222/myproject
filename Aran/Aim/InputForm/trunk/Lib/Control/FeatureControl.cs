using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Aran.Aim.Features;
using Aran.Queries.Viewer;
using Aran.Queries.Common;
using Aran.Aim.Enums;

namespace Aran.Aim.InputFormLib
{
    public class FeatureControl : Panel
    {
        public event FormClosingEventHandler Closed;
        public event FeatureEventHandler Saved;
        public event GetFeatureHandler GetFeature;
		public event FeatureListByDependEventHandler GetFeatListByDepend;
        public FillDataGridColumnsHandler DataGridColumnsFilled;
        public SetDataGridRowHandler DataGridRowSetted;
        
#if NEWFEATURECONTROL
        private DbEntityControl _dbEntityControl;
#else
        private FeatureViewerForm _featureViewerForm;
#endif

        public FeatureControl ()
        {
#if NEWFEATURECONTROL
            _dbEntityControl = new DbEntityControl ();
            _dbEntityControl.Width = Width;
            _dbEntityControl.Height = Height - 39;
            _dbEntityControl.Anchor =
                AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Bottom;

            _dbEntityControl.Expandable = false;

            Controls.Add (_dbEntityControl);
#else
#endif
        }

        public Feature RootFeature { get; private set; }

        public Feature GetEditingFeature ()
        {
#if NEWFEATURECONTROL
            return null;
#else
            return _featureViewerForm.GetEditingFeature ();
#endif
        }

        public bool HasChanged ( bool rootFeatInsertedAsCorrection = false)
        {
#if NEWFEATURECONTROL
            return false;
#else
            Feature editingFeature = GetEditingFeature ();
            Feature orgFeature = RootFeature;
			if ( rootFeatInsertedAsCorrection )
				orgFeature.TimeSlice.CorrectionNumber++;

			bool result = ( !AimObject.IsEquals ( orgFeature, editingFeature, true ) );

			if ( rootFeatInsertedAsCorrection )
				orgFeature.TimeSlice.CorrectionNumber--;

			return result;
#endif
        }

        public void LoadFeature (Feature feature, AimClassInfo classInfo)
        {
#if NEWFEATURECONTROL
            _dbEntityControl.LoadDbEntity (feature, classInfo);
#else
            if (_featureViewerForm != null) {
                _featureViewerForm.DefaultEffectiveDate = InputFormController.DefaultEffectiveDate;
                return;
            }

            RootFeature = feature;

            _featureViewerForm = new FeatureViewerForm ();
            _featureViewerForm.DefaultEffectiveDate = InputFormController.DefaultEffectiveDate;
            _featureViewerForm.GetFeature += GetFeature;
            _featureViewerForm.FeatureSaved += Saved;
            _featureViewerForm.GetFeatsListByDepend += GetFeatListByDepend;
            _featureViewerForm.DataGridColumnsFilled = DataGridColumnsFilled;
            _featureViewerForm.DataGridRowSetted = DataGridRowSetted;
            _featureViewerForm.SetFeature (feature);
            _featureViewerForm.HideBottomButtons ();

            Panel panel = _featureViewerForm.MainContainer;
            panel.Dock = DockStyle.Fill;
            Controls.Add (panel);

            _featureViewerForm.BackEventHandler = new EventHandler (closeButton_Click);

            return;
#endif
        }

        private void closeButton_Click (object sender, EventArgs e)
        {
            if (Closed != null)
            {
                FormClosingEventArgs args = new FormClosingEventArgs (CloseReason.UserClosing, false);
                Closed (this, args);
                if (args.Cancel)
                    return;
            }

            Dispose ();
        }

    }
}
