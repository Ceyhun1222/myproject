using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Aran.Aim;
using System.Collections;
using Aran.Aim.Features;
using Aran.Aim.Data;
using Aran.Aim.Data.Filters;
using Aran.Aim.Enums;
using Aran.Aim.Metadata.Geo;
using MapEnv.Layers;

namespace MapEnv
{
    public partial class SimpleLayerBuilderForm : Form
    {
        private int _currentPageIndex;
		private User _user;
        private List<Control> _pageControls;


		public event EventHandler FinishClicked;


        public SimpleLayerBuilderForm ()
        {
            InitializeComponent ();
            CustomInitComponent ();

            _currentPageIndex = -1;
        }

        public SimpleLayerBuilderForm(User SelectedUser)
            : this()
        {
            // TODO: Complete member initialization
            _user = SelectedUser;
        }


        public bool GetResult(
            out string layerName,
            out FeatureType featType,
            out Filter filter,
            out TableShapeInfo[] shapeInfos)
        {
            layerName = ui_layerNameTB.Text;
            featType = ui_featureTypeSelector.SelectedType;
            filter = ui_filterControl.GetFilter();
            shapeInfos = ui_featureStyleControl.GetShapeInfos();

            if (string.IsNullOrWhiteSpace(layerName))
                layerName = featType.ToString();

            return true;
        }


        private void CustomInitComponent()
        {
            ui_hiddenTabControl.Visible = false;
            ui_featureTypesPanel.Parent = ui_containerPanel;
            ui_featureStyleControl.Parent = ui_containerPanel;
            ui_filterControl.Parent = ui_containerPanel;

            _pageControls = new List<Control>();
            _pageControls.Add(ui_featureTypesPanel);
            _pageControls.Add(ui_featureStyleControl);
            _pageControls.Add(ui_filterControl);

            ui_filterControl.LoadFeatureListByDependHandler = Globals.GetFeatureListByDepend;
        }

        private void LayerBuilderForm_Load (object sender, EventArgs e)
        {
            ui_featureTypeSelector.Init(LayerType.Geography);
            CurrentPageIndex = 0;
        }
        
        private void CancelButton_Click (object sender, EventArgs e)
        {
            Close ();
        }

        private void NextButton_Click(object sender, EventArgs e)
        {
            if (true.Equals(ui_nextButton.Tag))
            {
                OnFinishClick();
                return;
            }

            var pageIndex = CurrentPageIndex;

            switch (_currentPageIndex)
            {
                case 0:
                    {
                        if (!PageChanged_FeatyreTypeToShapeInfo())
                            return;

                        if (ui_featureTypeSelector.LayerType == LayerType.Table)
                        {
                            pageIndex++;
                            ui_filterControl.SetFilter(ui_featureTypeSelector.SelectedType);
                        }
                    }
                    break;
                case 1:
                    ui_filterControl.SetFilter(ui_featureTypeSelector.SelectedType);
                    break;
            }

            CurrentPageIndex = pageIndex + 1;
        }

        private void PrevButton_Click (object sender, EventArgs e)
        {
            var pageIndex = CurrentPageIndex;

            if (ui_featureTypeSelector.LayerType == LayerType.Table)
                pageIndex--;

            CurrentPageIndex = pageIndex - 1;
        }

        private void uiEvents_layerNameTB_KeyDown (object sender, KeyEventArgs e)
        {
            ui_layerNameTB.Tag = true;
        }

        private void uiEvents_layerNameTB_TextChanged (object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace (ui_layerNameTB.Text))
                ui_layerNameTB.Tag = null;
        }

        private void OnFinishClick ()
        {
			if (FinishClicked != null)
			{
				FinishClicked (this, null);
				Close ();
				return;
			}
        }
        
        private bool PageChanged_FeatyreTypeToShapeInfo ()
        {
            if ((int)ui_featureTypeSelector.SelectedType == 0)
                return false;

            if (!true.Equals(ui_layerNameTB.Tag))
                ui_layerNameTB.Text = ui_featureTypeSelector.SelectedType.ToString();

            var featureType = ui_featureTypeSelector.SelectedType;
            var shapeInfos = DefaultStyleLoader.Instance.GetShapeInfo (featureType);

            if (ui_featureStyleControl.FeatureType != featureType)
                ui_featureStyleControl.SetShapeInfos (featureType, shapeInfos);

            return true;
        }

        private int CurrentPageIndex
        {
            get
            {
                return _currentPageIndex;
            }
            set
            {
                if (_currentPageIndex == value)
                    return;

                _currentPageIndex = value;

                for (int i = 0; i < _pageControls.Count; i++)
                    _pageControls[i].Visible = (i == value);

                CurrentPageChanged();
            }
        }

        private void CurrentPageChanged ()
        {
            bool isFinish = (_currentPageIndex == _pageControls.Count - 1);
            ui_nextButton.Text = (isFinish ? "Finish" : "Next >");
            ui_nextButton.Tag = isFinish;

            ui_prevButton.Visible = (_currentPageIndex > 0);

            string s = "";
            switch (_currentPageIndex)
            {
                case 0:
                    s = "Select Feature Type";
                    break;
                case 1:
                    s = "Configure Shape Styles";
                    break;
                case 2:
                    s = "Set Filter for " + ui_featureTypeSelector.SelectedType;
                    break;
            }
            ui_pageTextLabel.Text = s;
        }

        private void FeatureTypeSelector_TypeSelected(object sender, EventArgs e)
        {
            ui_nextButton.Enabled = ((int)ui_featureTypeSelector.SelectedType != 0);
        }
    }

    public enum LayerType { Geography, Table }
}
