using Aran.Aim;
using Aran.Aim.CAWProvider;
using Aran.Aim.CAWProvider.Configuration;
using Aran.Aim.Data;
using MapEnv.Layers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MapEnv
{
    public partial class NewWorkPackageLayerForm : Form
    {
        private CawDbProvider _cawDbPro;
        private List<WorkPackageOutline> _wpOutlineList;
        private WorkPackageOutline _selectedWpOutline;
        private List<Control> _pageControls;
        private int _prevPageIndex;
        private int _currentIndex;
        private DefaultStyleLoader _styleLoader;


        public NewWorkPackageLayerForm()
        {
            InitializeComponent();

            _cawDbPro = Globals.MainForm.DbProvider as CawDbProvider;
            Debug.Assert(_cawDbPro != null, "DbProvider is not CawDbProvider.");

            Globals.SetDGV_DoubleBuffered(ui_wpDgv);

            _wpOutlineList = new List<WorkPackageOutline>();
            ui_wpDgv.RowCount = _wpOutlineList.Count;

            _pageControls = new List<Control>();
            _prevPageIndex = 0;
            _currentIndex = 0;

            _styleLoader = DefaultStyleLoader.Instance;

            HideTableControl();
        }


        public WorkPackageLayerInfo GetWorkPackageLayerInfo()
        {
            if (_selectedWpOutline == null)
                return null;

            var wpli = new WorkPackageLayerInfo();
            wpli.Id = _selectedWpOutline.Content.Id;
            wpli.EffektiveDate = _selectedWpOutline.Content.EffectiveDate;

            var dict = new Dictionary<FeatureType, List<Guid>>();

            foreach (var tso in _selectedWpOutline.TimeSlices) {

                if (tso.Content.FeatureType == null || 
                    !dict.TryGetValue(tso.Content.FeatureType.Value, out List<Guid> guidList)) {

                    guidList = new List<Guid>();
                    dict.Add(tso.Content.FeatureType.Value, guidList);
                }
                guidList.Add(tso.Content.Identifier);
            }

            foreach (var item in dict) {
                var wpFeatTypeInfo = new WorkPackageFeatureTypeInfo();
                wpFeatTypeInfo.FeatureType = item.Key;
                wpFeatTypeInfo.IdentifierList.AddRange(item.Value);

                var shapeInfos = ui_featTypesStyleControl.GetShapeInfo(item.Key);
                if (shapeInfos != null)
                    wpFeatTypeInfo.ShapeInfos.AddRange(shapeInfos);

                wpli.WPFeatureTypeInfos.Add(wpFeatTypeInfo);
            }
            

            return wpli;
        }

        
        private void HideTableControl()
        {
            foreach (TabPage page in ui_hiddenTabControl.TabPages) {
                var pageControl = page.Controls[0];
                pageControl.Visible = false;
                ui_pageContainerPanel.Controls.Add(pageControl);

                _pageControls.Add(pageControl);
            }
            ui_hiddenTabControl.Visible = false;

            _pageControls[_prevPageIndex].Visible = false;
            _pageControls[_currentIndex].Visible = true;
        }

        private void NewWorkPackageLayerForm_Load(object sender, EventArgs e)
        {
            _wpOutlineList = _cawDbPro.GetWorkPackages(Aran.Aim.CAWProvider.WorksPackageStatusType.Open);
            ui_wpDgv.RowCount = _wpOutlineList.Count;
            ui_wpDgv.Refresh();
        }

        private void WpDgv_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex > _wpOutlineList.Count - 1)
                return;

            var wpOutline = _wpOutlineList[e.RowIndex];
            if (wpOutline.Content == null)
                return;

            var wpBasic = wpOutline.Content;

            switch (e.ColumnIndex) {
                case 0:
                    e.Value = wpBasic.Id;
                    break;
                case 1:
                    e.Value = wpBasic.Name;
                    break;
                case 2:
                    e.Value = GetStatusString(wpBasic.Status);
                    break;
                case 3:
                    e.Value = wpBasic.EffectiveDate.ToString("yyyy-MM-dd");
                    break;
                case 4:
                    e.Value = wpOutline.TimeSlices.Count;
                    break;
            }
        }

        private void WpDgv_CurrentCellChanged(object sender, EventArgs e)
        {
            ui_featuresDgv.RowCount = 0;
            _selectedWpOutline = null;

            if (ui_wpDgv.CurrentCell == null)
                return;

            _selectedWpOutline = _wpOutlineList[ui_wpDgv.CurrentCell.RowIndex];
            ui_featuresDgv.RowCount = _selectedWpOutline.TimeSlices.Count;

            ui_featuresDgv.Refresh();
        }

        private string GetStatusString(string value)
        {
            return value;
        }

        private void FeaturesDgv_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
        {
            if (_selectedWpOutline == null || e.RowIndex < 0 || e.RowIndex > _selectedWpOutline.TimeSlices.Count - 1)
                return;

            var tsr = _selectedWpOutline.TimeSlices[e.RowIndex].Content;

            switch (e.ColumnIndex) {
                case 0:
                    e.Value = tsr.Identifier;
                    break;
                case 1:
                    e.Value = tsr.FeatureType;
                    break;
            }
        }

        private void Next_Click(object sender, EventArgs e)
        {
            //*** Finish Clicked.
            if (_currentIndex == 1) {
                DialogResult = DialogResult.OK;
                return;
            }

            _prevPageIndex = _currentIndex;
            _currentIndex++;

            //*** FeatureTypes Style Page.
            if (_currentIndex == 1) {
                ui_nextButton.Text = "Finish";

                if (_selectedWpOutline != null) {

                    foreach(var tso in _selectedWpOutline.TimeSlices){
                        if (tso.Content.FeatureType != null)
                        {
                            var featType = tso.Content.FeatureType.Value;

                            if (_styleLoader.Dict.ContainsKey(featType))
                            {
                                var shapeInfos = _styleLoader.Dict[featType];
                                ui_featTypesStyleControl.AddFeatureType(featType, shapeInfos);
                            }
                            else
                            {
                                ui_featTypesStyleControl.AddFeatureType(featType);
                            }
                        }
                    }
                }
            }

            _pageControls[_prevPageIndex].Visible = false;
            _pageControls[_currentIndex].Visible = true;

            ui_prevButton.Visible = (_currentIndex > 0);
        }

        private void Back_Click(object sender, EventArgs e)
        {
            _prevPageIndex = _currentIndex;
            _currentIndex--;

            _pageControls[_prevPageIndex].Visible = false;
            _pageControls[_currentIndex].Visible = true;
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }

    public class WorkPackageLayerInfo
    {
        public WorkPackageLayerInfo()
        {
            WPFeatureTypeInfos = new List<WorkPackageFeatureTypeInfo>();
        }

        public int Id { get; set; }

        public DateTime EffektiveDate { get; set; }

        public List<WorkPackageFeatureTypeInfo> WPFeatureTypeInfos { get; private set; }
    }

    public class WorkPackageFeatureTypeInfo
    {
        public WorkPackageFeatureTypeInfo()
        {
            ShapeInfos = new List<TableShapeInfo>();
            IdentifierList = new List<Guid>();
        }

        public FeatureType FeatureType { get; set; }

        public List<TableShapeInfo> ShapeInfos { get; private set; }

        public List<Guid> IdentifierList { get; private set; }
    }
}
