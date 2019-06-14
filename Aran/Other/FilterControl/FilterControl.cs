using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Aran.Aim;
using Aran.Aim.Data.Filters;
using Aran.Queries.Common;

namespace Aran.Controls
{
    public partial class FilterControl : UserControl
    {
        private List<FilterInfo> _filters;
        private FilterInfo _currentFilterInfo;
        private Dictionary<Control, Control> _filterOrTextDict;


        public FilterControl()
        {
            InitializeComponent();

            _filters = new List<FilterInfo>();
            _filterOrTextDict = new Dictionary<Control, Control>();
            this.DoubleBuffered = true;
        }


        public Filter GetFilter()
        {
            if (_filters.Count == 0)
                return null;

            OperationChoice oc = null;

            if (_filters.Count == 1) {
                oc = GetOperChoiceInFilterInfo(_filters[0]);
            }
            else {
                var blo = new BinaryLogicOp();
                blo.Type = BinaryLogicOpType.Or;

                foreach (var filter in _filters) {
                    var childOC = GetOperChoiceInFilterInfo(filter);
                    blo.OperationList.Add(childOC);
                }

                oc = new OperationChoice(blo);
            }

            if (oc != null)
                return new Filter(oc);

            return null;
        }

        public void SetFilter(FeatureType featureType, Filter filter = null)
        {
            FeatureType = featureType;
            _classInfo = AimMetadata.GetClassInfoByIndex((int)featureType);

            if (filter == null)
                return;

            OperationChoice oc = filter.Operation;

            if (oc.Choice == OperationChoiceType.Logic) {
                var blo = oc.LogicOps as BinaryLogicOp;

                if (blo.Type == BinaryLogicOpType.Or) {

                    foreach (var orOperChoiceItem in blo.OperationList) {
                        if (orOperChoiceItem.Choice == OperationChoiceType.Logic) {
                            var andBlo = orOperChoiceItem.LogicOps as BinaryLogicOp;
                            if (andBlo.Type == BinaryLogicOpType.And)
                                FillAndOpers(andBlo);
                        }
                        else {
                            FillConditions(null, orOperChoiceItem);
                        }
                    }
                }
                else {
                    FillAndOpers(blo);
                }
            }
            else {
                FillConditions(null, oc);
            }
        }

        public FeatureType FeatureType { get; private set; }

        public FeatureListByDependEventHandler LoadFeatureListByDependHandler
        {
            get { return InternalGlobal.LoadFeatureListByDependHandler; }
            set { InternalGlobal.LoadFeatureListByDependHandler = value; }
        }


        private AimClassInfo _classInfo
        {
            get { return ui_propSelControl.ClassInfo; }
            set { ui_propSelControl.ClassInfo = value; }
        }

        private void FillConditions(FilterInfo filterInfo, OperationChoice operChoice)
        {
            var isCreated = false;

            if (filterInfo == null) {
                filterInfo = new FilterInfo();
                filterInfo.Name = "Filter - " + (_filters.Count + 1);
                isCreated = true;
            }

            var condInfo = new ConditionInfo();
            condInfo.PropInfoArr = Aim.Utilities.AimMetadataUtility.GetInnerProps((int)FeatureType, operChoice.PropertyName);

            if (condInfo.PropInfoArr == null || condInfo.PropInfoArr.Length == 0)
                return;

            condInfo.OperationChoice = operChoice;
            filterInfo.Conditions.Add(condInfo);

            if (isCreated)
                AddNewFilter(filterInfo);
        }

        private void FillAndOpers(BinaryLogicOp blo)
        {
            var filterInfo = new FilterInfo();
            filterInfo.Name = "Filter - " + (_filters.Count + 1);

            foreach (var andOperChoiceItem in blo.OperationList)
                FillConditions(filterInfo, andOperChoiceItem);

            AddNewFilter(filterInfo);
        }

        private OperationChoice GetOperChoiceInFilterInfo(FilterInfo filterInfo)
        {
            if (filterInfo.Conditions.Count == 0)
                return null;

            if (filterInfo.Conditions.Count == 1)
                return filterInfo.Conditions[0].OperationChoice;

            var blo = new BinaryLogicOp();
            blo.Type = BinaryLogicOpType.And;

            foreach (var condInfo in filterInfo.Conditions)
                blo.OperationList.Add(condInfo.OperationChoice);

            return new OperationChoice(blo);
        }

        private void NewFilter_Click(object sender, EventArgs e)
        {
            var filterInfo = new FilterInfo();
            filterInfo.Name = "Filter - " + (_filters.Count + 1);

            AddNewFilter(filterInfo);
        }

        private void AddNewFilter(FilterInfo filterInfo)
        {
            var rb = ToRadioButton(filterInfo);
            rb.Checked = (ui_filtersFLP.Controls.Count == 0);
            ui_filtersFLP.Controls.Add(rb);

            _filters.Add(filterInfo);

            ui_newConditionButton.Enabled = (_filters.Count > 0);
        }

        private void AddNewCondition_Click(object sender, EventArgs e)
        {
            if (_currentFilterInfo == null)
                return;

            ui_propSelControl.PerformSelection(sender as Control, 300);
        }

        private void PropSelControl_ValueChanged(object sender, EventArgs e)
        {
            if (_currentFilterInfo == null)
                return;

            var condInfo = new ConditionInfo { PropInfoArr = ui_propSelControl.Value };
            var fic = ToFilterItem(condInfo);
            ui_filterItemsFLP.Controls.Add(fic);
            _currentFilterInfo.Conditions.Add(condInfo);
        }

        private void PropSelControl_AfterSelect(object sender, PropertySelectedEventArgs e)
        {
            var propInfo = e.SelectedProp.LastOrDefault();

            if (propInfo == null) {
                e.Cancel = true;
                return;
            }

            if (propInfo.PropType.AimObjectType == AimObjectType.Object) {
                if (propInfo.PropType.Index == (int)ObjectType.FeatureRefObject) {
                    var tmp = propInfo.PropType.Properties["Feature"].Clone();
                    tmp.ReferenceFeature = propInfo.ReferenceFeature;
                    propInfo = tmp;
                }
                else {
                    e.Cancel = true;
                    return;
                }
            }

            //ui_propSelControl.PropInfo = propInfo;

            //bool isGeo;
            //var compOpTypeItems = GetComparisonOpersByPropInfo(propInfo, out isGeo);

            //ui_operTypeCB.Items.Clear();

            //if (isGeo) {
            //    ui_operTypeCB.Items.Add(new OperTypeItem(GeoDWithinText));
            //    ui_operTypeCB.Items.Add(new OperTypeItem(GeoWithinText));
            //}
            //else {
            //    foreach (ComparisonOpType compOperType in compOpTypeItems)
            //        ui_operTypeCB.Items.Add(new OperTypeItem(compOperType));
            //}

            //if (ui_operTypeCB.Items.Count > 0)
            //    ui_operTypeCB.SelectedIndex = 0;

            //ui_operValuePanel.Visible = (ui_operTypeCB.Items.Count > 0);
        }

        private FilterItemControl ToFilterItem(ConditionInfo condInfo)
        {
            var fic = new FilterItemControl();
            fic.Tag = condInfo;

            fic.ClassInfo = _classInfo;
            fic.PropInfoArr = condInfo.PropInfoArr;

            fic.ValueChanged += (sender, e) => {
                var pFic = sender as FilterItemControl;
                var pCondInfo = pFic.Tag as ConditionInfo;
                pCondInfo.OperationChoice = pFic.GetOperation();
            };

            if (condInfo.OperationChoice != null)
                fic.SetOperation(condInfo.OperationChoice);

            condInfo.OperationChoice = fic.GetOperation();

            return fic;
        }

        private RadioButton ToRadioButton(FilterInfo filterInfo)
        {
            var rb = new RadioButton();
            rb.AutoSize = true;
            rb.Tag = filterInfo;
            rb.Appearance = Appearance.Button;
            rb.FlatStyle = FlatStyle.Flat;
            rb.FlatAppearance.CheckedBackColor = Color.LightBlue;
            rb.Text = filterInfo.Name;

            rb.CheckedChanged += (sender, e) => {
                var rbb = sender as RadioButton;
                if (!rbb.Checked)
                    return;

                SetCurrentFilterInfo(rbb.Tag as FilterInfo);
            };

            var cms = new ContextMenuStrip();
            var tsmi = new ToolStripMenuItem("Remove Filter");
            tsmi.Tag = rb;
            cms.Items.Add(tsmi);
            rb.ContextMenuStrip = cms;

            tsmi.Click += (sender, e) => {
                var rbb = (sender as ToolStripItem).Tag as RadioButton;
                var isChecked = rbb.Checked;
                var filterInfoo = rbb.Tag as FilterInfo;

                ui_filtersFLP.Controls.Remove(rbb);
                _filters.Remove(filterInfoo);

                if (isChecked && ui_filtersFLP.Controls.Count > 0)
                    (ui_filtersFLP.Controls[0] as RadioButton).Checked = true;
                else
                    SetCurrentFilterInfo(null);

                ui_newConditionButton.Enabled = (_filters.Count > 0);
            };

            var renameTsmi = new ToolStripMenuItem("Rename:");
            var renameTB = new ToolStripTextBox();
            renameTB.Tag = rb;
            renameTB.Text = filterInfo.Name;
            renameTsmi.DropDownItems.Add(renameTB);
            
            renameTB.KeyUp += (sender, e) => {
                if (e.KeyData == Keys.Enter) {
                    var tsTB = sender as ToolStripTextBox;
                    var rbb = tsTB.Tag as RadioButton;
                    rbb.Text = tsTB.Text;
                    rbb.ContextMenuStrip.Hide();
                }
            };

            renameTsmi.DropDownOpened += (sender, e) => {
                var tsmii = sender as ToolStripMenuItem;
                tsmii.DropDownItems[0].Text = (tsmii.DropDownItems[0].Tag as Control).Text;
            };

            cms.Items.Add(renameTsmi);

            return rb;
        }

        private void SetCurrentFilterInfo(FilterInfo filterInfo)
        {
            //--- Set null to _currentFilterInfo todo nothing in FilterItemsFLP_ControlRemoved.
            _currentFilterInfo = null;

            ui_filterItemsFLP.SuspendLayout();
            ui_filterItemsFLP.Controls.Clear();

            if (filterInfo == null)
                return;

            _currentFilterInfo = filterInfo;
            _currentFilterInfo.Conditions.ForEach(ci => ui_filterItemsFLP.Controls.Add(ToFilterItem(ci)));

            ui_filterItemsFLP.ResumeLayout();
        }

        private void FilterItemsFLP_ControlRemoved(object sender, ControlEventArgs e)
        {
            if (!(e.Control is FilterItemControl))
                return;

            if (_currentFilterInfo == null)
                return;
            
            var condInfo = e.Control.Tag as ConditionInfo;
            if (condInfo == null)
                return;

            _currentFilterInfo.Conditions.Remove(condInfo);

            var flp = sender as FlowLayoutPanel;
            if (flp.Controls.Count > 0)
            {
                var fic = e.Control as FilterItemControl;
                if (fic.AndTextControl == null)
                    fic = flp.Controls[1] as FilterItemControl;
                flp.Controls.Remove(fic.AndTextControl);
            }
        }

        private void FilterItemsFLP_ControlAdded(object sender, ControlEventArgs e)
        {
            var fic = e.Control as FilterItemControl;
            if (fic == null)
                return;

            var flp = sender as FlowLayoutPanel;
            if (flp.Controls.Count >= 2)
            {
                flp.SuspendLayout();
                var andCont = MakeAndOrControl(true);
                flp.Controls.Add(andCont);
                flp.Controls.SetChildIndex(andCont, flp.Controls.Count - 2);
                fic.AndTextControl = andCont;
                flp.ResumeLayout();
            }
        }

        private Control MakeAndOrControl(bool isAnd)
        {
            var lab = new Label();
            lab.AutoSize = true;
            lab.Text = (isAnd ? "AND" : "OR");
            if (isAnd)
            {
                lab.Height = 26;
                lab.Margin = new Padding(10, 0, 0, 0);
            }
            else
            {
                lab.Margin = new Padding(4, 10, 4, 4);
            }
            lab.Font = new System.Drawing.Font(lab.Font, FontStyle.Bold);
            lab.TextAlign = ContentAlignment.MiddleLeft;
            
            return lab;
        }

        private void FiltersFLP_ControlAdded(object sender, ControlEventArgs e)
        {
            if (!(e.Control is RadioButton))
                return;

            var flp = sender as FlowLayoutPanel;
            if (flp.Controls.Count >= 2)
            {
                flp.SuspendLayout();
                var orCont = MakeAndOrControl(false);
                flp.Controls.Add(orCont);
                flp.Controls.SetChildIndex(orCont, flp.Controls.Count - 2);

                _filterOrTextDict.Add(e.Control, orCont);

                flp.ResumeLayout();
            }
        }

        private void FiltersFLP_ControlRemoved(object sender, ControlEventArgs e)
        {
            if (!(e.Control is RadioButton))
                return;

            var flp = sender as FlowLayoutPanel;
            if (flp.Controls.Count > 0)
            {
                Control orCont;
                if(!_filterOrTextDict.TryGetValue(e.Control, out orCont))
                    _filterOrTextDict.TryGetValue(flp.Controls[1], out orCont);
                flp.Controls.Remove(orCont);
            }
        }
    }

    internal class FilterInfo
    {
        public FilterInfo()
        {
            Conditions = new List<ConditionInfo>();
        }

        public string Name { get; set; }

        public List<ConditionInfo> Conditions { get; private set; }
    }

    internal class ConditionInfo
    {
        public AimPropInfo[] PropInfoArr { get; set; }

        public OperationChoice OperationChoice { get; set; }
    }
}
