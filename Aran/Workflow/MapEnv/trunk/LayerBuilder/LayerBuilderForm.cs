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
using Aran.Aim.Env.Layers;
using Aran.Aim.Features;
using Aran.Aim.Data;
using Aran.Aim.Data.Filters;
using Aran.Aim.Enums;
using Aran.Aim.Metadata.Geo;

namespace MapEnv
{
    public partial class LayerBuilderForm : Form
    {
        private int _currentPageIndex;
        private string _layerName;
        private LayerType _layerType;
		private User _user;

        public LayerBuilderForm ()
        {
            InitializeComponent ();
            CustomInitComponent ();

            _currentPageIndex = -1;
        }

		public LayerBuilderForm ( User SelectedUser )
			: this ( )
		{
			// TODO: Complete member initialization
			_user = SelectedUser;
		}


        private void CustomInitComponent ()
        {
            ui_sheetPanel.Controls.Clear ();
            ui_sheetPanel.Controls.Add (ui_featureTypesPanel);
            ui_sheetPanel.Controls.Add (ui_featureStyleControl);
            ui_sheetPanel.Controls.Add (ui_filterControl);

            ui_filterControl.FeatureDescription += Globals.AimPropertyControl_FeatureDescription;
            ui_filterControl.FillDataGridColumnsHandler = Globals.AimPropertyControl_FillDataGridColumn;
            ui_filterControl.SetDataGridRowHandler = Globals.AimPropertyControl_SetRow;
            ui_filterControl.LoadFeatureListByDependHandler = Globals.GetFeatureListByDepend;

            foreach (Control cont in ui_sheetPanel.Controls)
                cont.Dock = DockStyle.Fill;

            ui_layerTypeCB.Items.Add (LayerType.Geography);
            ui_layerTypeCB.Items.Add (LayerType.Table);
            
        }

        private void LayerBuilderForm_Load (object sender, EventArgs e)
        {
            ui_layerTypeCB.SelectedIndex = 0;
        }

        
        private void uiEvent_cancelButton_Click (object sender, EventArgs e)
        {
            Close ();
        }

        private void uiEvents_featureTypesListView_SelectedIndexChanged (object sender, EventArgs e)
        {
            bool b = (ui_featureTypesListView.SelectedItems.Count > 0);

            ui_nextButton.Enabled = b;
            ui_nextButton.Tag = false;

            if (b && ui_layerNameTB.Tag == null)
            {
                FeatureType featureType = (FeatureType) ui_featureTypesListView.SelectedItems [0].Tag;
                ui_layerNameTB.Text = featureType.ToString ();
            }
        }

        private void uiEvents_nextButton_Click (object sender, EventArgs e)
        {
            if (true.Equals (ui_nextButton.Tag))
            {
                FinishClicked ();
                return;
            }

            if (_currentPageIndex == ui_sheetPanel.Controls.Count - 1)
                return;

            switch (_currentPageIndex)
            {
                case 0:
                    {
                        if (!PageChanged_FeatyreTypeToShapeInfo ())
                            return;

                        if (_layerType == LayerType.Table)
                        {
                            _currentPageIndex = 1;

                            if (!PageChanged_ShapeInfoToFilter ())
                                return;
                        }
                    }
                    break;
                case 1:
                    if (!PageChanged_ShapeInfoToFilter ())
                        return;
                    break;
            }

            CurrentPageIndex++;
        }

        private void uiEvents_prevButton_Click (object sender, EventArgs e)
        {
            CurrentPageIndex--;
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

        private void uiEvents_LayerTypeCB_SelectedIndexChanged (object sender, EventArgs e)
        {
            ui_nextButton.Enabled = false;

            _layerType = (LayerType) ui_layerTypeCB.SelectedItem;

            if (_layerType == LayerType.Geography)
            {
                List<FeatureType> featureTypeList = new List<FeatureType> ();
                foreach (var item in GeoMetadata.GeoClassInfoList)
                {
                    if (!item.AimClassInfo.IsAbstract)
                        featureTypeList.Add ((FeatureType) item.AimClassInfo.Index);
                }
                FillListViews (featureTypeList);
            }
            else
            {
                FeatureType [] featTypeArr = (FeatureType []) typeof (FeatureType).GetEnumValues ();
                FillListViews (featTypeArr);
            }
        }


        private void FinishClicked ()
        {
            IDbProvider dbProvider = Globals.MainForm.DbProvider;
            FeatureType featureType = GetFeatureType ();
            Filter filter = ui_filterControl.GetFilter ();

            AimFeatureLayer aimLayer = new AimFeatureLayer ();
            aimLayer.Create (Globals.MainForm);
            aimLayer.Open (featureType);
            aimLayer.AimTable.ShapeInfoList.AddRange (ui_featureStyleControl.GetShapeInfos ());
            aimLayer.AimTable.Filter = filter;

            IEnumerable<Feature> features = Globals.LoadFeatures (featureType, filter);

            aimLayer.AimTable.AddFeatures (features);

            ESRI.ArcGIS.Carto.ILayer layer = (ESRI.ArcGIS.Carto.ILayer) aimLayer;
            layer.Name = _layerName;

            Globals.MainForm.Map.AddLayer (layer);
            Globals.OnLayerAdded (layer);

            Close ();
        }
        
        private bool PageChanged_FeatyreTypeToShapeInfo ()
        {
            if (ui_featureTypesListView.SelectedItems.Count == 0)
                return false;

            _layerName = ui_layerNameTB.Text;

            FeatureType featureType = GetFeatureType ();
            if (ui_featureStyleControl.FeatureType != featureType)
                ui_featureStyleControl.SetShapeInfos (featureType, null);

            return true;
        }

        private bool PageChanged_ShapeInfoToFilter ()
        {
            FeatureType featureType = GetFeatureType ();
            
            ui_filterControl.SetFilter (featureType);
            
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

                for (int i = 0; i < ui_sheetPanel.Controls.Count; i++)
                {
                    ui_sheetPanel.Controls [i].Visible = (i == value);
                }

                CurrentPageChanged ();
            }
        }

        private void CurrentPageChanged ()
        {
            bool isFinish = (_currentPageIndex == ui_sheetPanel.Controls.Count - 1);
            ui_nextButton.Text = (isFinish ? "Finish" : "Next");
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
                    s = "Set Filter";
                    break;
            }
            ui_pageTextLabel.Text = s;
        }

        private FeatureType GetFeatureType ()
        {
            return (FeatureType) ui_featureTypesListView.SelectedItems [0].Tag;
        }

        private void FillListViews (IEnumerable<FeatureType> featureTypes)
        {
            ListView lv = ui_featureTypesListView;
            lv.Items.Clear ();
            lv.Groups.Clear ();

            foreach (FeatureType featType in featureTypes)
            {
				if ( !_user.ContainsFeatType ( featType ) )
					continue;
                ListViewItem lvi = new ListViewItem ();
                lvi.Tag = featType;
                lvi.Text = featType.ToString ();
                lv.Items.Add (lvi);
            }

            CurrentPageIndex = 0;

            lv.Sorting = SortOrder.Ascending;
            lv.ListViewItemSorter = new ListViewItemComparer ();

            if (lv.Items.Count > 0)
            {
                char c = lv.Items [0].Text [0];
                string s = c.ToString ();
                lv.Groups.Add (s, s);

                for (int i = 1; i < lv.Items.Count; i++)
                {
                    if (c != lv.Items [i].Text [0])
                    {
                        c = lv.Items [i].Text [0];
                        s = c.ToString ();
                        lv.Groups.Add (s, s);
                    }
                }
            }

            foreach (ListViewItem lvi in lv.Items)
            {
                lvi.Group = lv.Groups [lvi.Text [0].ToString ()];
            }

            lv.ShowGroups = true;
            lv.View = View.Details;
        }
    }

    internal class ListViewItemComparer : IComparer
    {
        public int Compare (object x, object y)
        {
            return string.Compare (x.ToString (), y.ToString ());
        }
    }

    internal enum LayerType { Geography, Table }
}
