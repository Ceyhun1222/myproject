using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Aran.Aim.Features;
using Aran.Queries.Viewer;
using Aran.Queries.Common;

namespace Aran.Aim.InputFormLib
{
    public class FeatureControl : Panel
    {
        public event FormClosingEventHandler Closed;
        public event FeatureEventHandler Saved;
        public event FeatureEventHandler Opened;
        public event GetFeatureHandler GetFeature;
        public event GetFeatureListHandler GetFeatureList;
        public FillDataGridColumnsHandler DataGridColumnsFilled;
        public SetDataGridRowHandler DataGridRowSetted;
        //private DbEntityControl _dbEntityControl;
        private FeatureViewerForm _featureViewerForm;

        public FeatureControl ()
        {
            //_dbEntityControl = new DbEntityControl ();
            //_dbEntityControl.Width = Width;
            //_dbEntityControl.Height = Height - 39;
            //_dbEntityControl.Anchor =
            //    AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Bottom;

            //_dbEntityControl.Expandable = false;

            //Controls.Add (_dbEntityControl);
        }

        public Feature RootFeature { get; private set; }

        public Feature GetEditingFeature ()
        {
            return _featureViewerForm.GetEditingFeature ();
        }

        public bool HasChanged ()
        {
            Feature editingFeature = GetEditingFeature ();
            Feature orgFeature = RootFeature;

            return (!AimObject.IsEquals (orgFeature, editingFeature));
        }

        public void LoadFeature (Feature feature, AimClassInfo classInfo)
        {
            if (_featureViewerForm != null)
                return;

            RootFeature = feature;

            _featureViewerForm = new FeatureViewerForm ();
            _featureViewerForm.GetFeature += new GetFeatureHandler (_featureViewerForm_GetFeature);
            _featureViewerForm.FeatureSaved += new FeatureEventHandler (_featureViewerForm_FeatureSaved);
            _featureViewerForm.OpenedFeature += new FeatureEventHandler (_featureViewerForm_OpenedFeature);
            _featureViewerForm.GetFeatureList += new GetFeatureListHandler (_featureViewerForm_GetFeatureList);
            _featureViewerForm.DataGridColumnsFilled = DataGridColumnsFilled;
            _featureViewerForm.DataGridRowSetted = DataGridRowSetted;
            _featureViewerForm.SetFeature (feature);
            _featureViewerForm.HideBottomButtons ();

            Panel panel = _featureViewerForm.MainContainer;
            panel.Dock = DockStyle.Fill;
            Controls.Add (panel);

            _featureViewerForm.BackEventHandler = new EventHandler (closeButton_Click);

            return;

            Panel closeContainerPanel = new Panel ();
            closeContainerPanel.Height = 25;
            closeContainerPanel.Dock = DockStyle.Bottom;
            Controls.Add (closeContainerPanel);

            Button button = new Button ();
            button.Text = "<< Back";
            button.Width = 100;
            button.Dock = DockStyle.Right;
            button.Click += new EventHandler (closeButton_Click);
            closeContainerPanel.Controls.Add (button);
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

        private Feature _featureViewerForm_GetFeature (FeatureType featureType, Guid identifier)
        {
            if (GetFeature != null)
                return GetFeature (featureType, identifier);
            return null;
        }

        private IEnumerable<Feature> _featureViewerForm_GetFeatureList (FeatureType featureType)
        {
            if (GetFeatureList != null)
                return GetFeatureList (featureType);
            return null;
        }

        private void _featureViewerForm_OpenedFeature (object sender, FeatureEventArgs e)
        {
            if (Opened != null)
                Opened (this, e);
        }


        private void _featureViewerForm_FeatureSaved (object sender, FeatureEventArgs e)
        {
            if (Saved != null)
                Saved (this, e);
        }
    }
}
