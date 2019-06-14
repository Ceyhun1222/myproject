using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Aran.Aim;
using Aran.Aim.Enums;
using Aran.Aim.Features;
using Aran.Aim.InputFormLib;
using Aran.Queries.Common;
using Aran.Queries.Viewer;
using Aran.Aim.Metadata.UI;
using Aran.Aim.Data;
using Aran.Controls;
using Aran.AranEnvironment;
using System.Reflection;

namespace Aran.Aim.InputForm
{
    public partial class MainForm : Form
    {
        private InputFormController _controller;
        private string _currentFeatureName;
        private bool _quickSearchTextTyped;
        private bool _externalgDbProvider;
        private Settings _settings;
        private Connection _connection;
        private PolyCreatorControl _lastPolyControl;
        private int ui_FeaturesDGVSortedColumnIndex = -1;
        private int ui_FeaturesDGVSelectedRowIndex = -1;
        private SortOrder? ui_FeaturesDGVSortdOrder = null;


        public MainForm()
        {
            InitializeComponent();

            _externalgDbProvider = false;

            _controller = new InputFormController();
            _controller.FeaturesLoaded += Controller_FeaturesLoaded;
            _controller.ClosedEventHandler = DbEntityControl_Closed;
            _controller.SavedEventHandler = DbEntityControl_Saved;

            _settings = new Settings();

            var pi = ui_FeaturesDGV.GetType().GetProperty("DoubleBuffered",
                BindingFlags.NonPublic | BindingFlags.Instance);
            pi.SetValue(ui_FeaturesDGV, true, null);

            ui_FeaturesDGV.SortCompare += UIUtilities.DGV_SortCompare;
        }


        public void SetDbProvider(Connection connection, IAranEnvironment environment)
        {
            Globals.Environment = environment;

            _connection = connection;
            _controller.DbProvider = environment.DbProvider as DbProvider;
            _externalgDbProvider = true;

            ui_exitToolStripMenuItem.Text = "&Close";

            ui_effectiveDateTimePicker.Value = _controller.DbProvider.DefaultEffectiveDate;
        }

        public CoordinateFormat CoordinateFormat
        {
            get
            {
                return
                    (Aran.Queries.Common.Settings.Instance.CoordinateFormatIsDMS ?
                    InputForm.CoordinateFormat.DMS : InputForm.CoordinateFormat.DD);
            }
            set { Aran.Queries.Common.Settings.Instance.CoordinateFormatIsDMS = (value == InputForm.CoordinateFormat.DMS); }
        }

        public int CoordinateFormatRound
        {
            get { return Aran.Queries.Common.Settings.Instance.CoordinateFormatAccuracy; }
            set { Aran.Queries.Common.Settings.Instance.CoordinateFormatAccuracy = value; }
        }

        public DateTime EffectiveDate
        {
            get { return ui_effectiveDateTimePicker.Value; }
            set
            {
                if (ui_effectiveDateTimePicker.Value != value)
                    ui_effectiveDateTimePicker.Value = value;
            }
        }

        public GetFeatureListHandler GetFeatureListHandler { get; set; }


        private void MainForm_Load(object sender, EventArgs e)
        {
            _settings.Load(Application.LocalUserAppDataPath + "\\InputFormSettings.config");

            try
            {
                if (!_externalgDbProvider)
                {
                    var conn = _settings.Connection;

                    if (conn == null)
                    {
                        Options_Click(null, null);
                    }
                    else
                    {
                        _controller.DbProvider = CreateDbProvider(conn.ConnectionType);
                        _controller.DbProvider.Open(conn.GetConnectionString());

                        var loginForm = new LoginForm();
                        loginForm.OKClicked += LoginForm_OKClicked;
                        if (loginForm.ShowDialog(this) != DialogResult.OK)
                        {
                            Close();
                            return;
                        }
                    }

                    EffectiveDate = _settings.EffectiveDate;
                }
            }
            catch (Exception ex)
            {
                ui_MainSplitContainer.Enabled = false;
                MessageBox.Show(ex.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (_controller.DbProvider == null)
            {
                return;
            }

            if (_settings.FormSettings.Width != 0)
                this.Width = _settings.FormSettings.Width;
            if(_settings.FormSettings.Height != 0)
                this.Height = _settings.FormSettings.Height;

            this.SizeChanged += (o, args) => { _settings.FormSettings.Height = ((Form) o).Height; _settings.FormSettings.Width = ((Form)o).Width; };
            ui_FeatureTypesDGV.CurrentCellChanged += new EventHandler(ui_FeatureTypesDGV_CurrentCellChanged);

            var featureNames = _controller.GetFeaturesByDepends(null);
            FillFeatureTypesDGV(featureNames);
        }

        private void LoginForm_OKClicked(object sender, EventArgs e)
        {
            var loginForm = sender as LoginForm;

            if (_controller.DbProvider.Login(
                    loginForm.UserName,
                    Aran.Aim.Data.DbUtility.GetMd5Hash(loginForm.Password)))
            {

                loginForm.DialogResult = DialogResult.OK;
            }
            else
            {
                loginForm.ErrorMessage = "Invalid User Name or Password!";
            }
        }

        private void Refresh_Click(object sender, EventArgs e)
        {
            LoadFeatures();
        }

        private void ui_FeatureTypesDGV_CurrentCellChanged(object sender, EventArgs e)
        {
            if (ui_FeatureTypesDGV.CurrentCell == null)
                return;


            //ui_newTSB.Enabled = (_controller.DbProvider.CurrentUser.Privilege != Privilige.prReadOnly);
            ui_nextTSB.Enabled = false;

            _currentFeatureName = ui_FeatureTypesDGV.CurrentCell.Value.ToString();
            ui_statusLabel.Text = "Features count: 0";


            LoadFeatures();

            if (ui_quickSearchTextBox.Text == "Quick Search")
            {
                SetAllRowVisible(true);
                if (_quickSearchTextTyped)
                    ui_FeatureTypesDGV.FirstDisplayedScrollingRowIndex = ui_FeatureTypesDGV.SelectedRows[0].Index;
                _quickSearchTextTyped = false;
                ui_FeatureTypesDGV.Refresh();
            }


        }

        private void ui_nextButton_Click(object sender, EventArgs e)
        {
            if (ui_FeaturesDGV.CurrentRow != null)
            {
                Feature selectedFeature = ui_FeaturesDGV.CurrentRow.Tag as Feature;
                AddNavigationControl(selectedFeature);
            }

            var featureNames = _controller.GetFeaturesByDepends(_currentFeatureName);
            FillFeatureTypesDGV(featureNames);
        }

        private void ui_FeaturesDGV_CurrentCellChanged(object sender, EventArgs e)
        {
            bool b = (ui_FeaturesDGV.CurrentCell != null);

            var featureNames = _controller.GetFeaturesByDepends(_currentFeatureName);
            bool contains = false;
            foreach (string featureName in featureNames)
            {
                if (_controller.DbProvider.CurrentUser.ContainsFeatType(featureName))
                {
                    contains = true;
                    break;
                }
            }

            if (_controller.IsFeatureClassified)
                ui_nextTSB.Enabled = (b && featureNames.Count != 0 && contains);

            ui_viewTSSB.Enabled = b;

            if (b)
            {
                if (_controller.DbProvider.CurrentUser.Privilege == Privilige.prReadOnly)
                    b = false;
            }

            ui_editCorrTSB.Enabled = b;
            ui_editNewSeqTSB.Enabled = b;
            ui_deleteTSMI.Enabled = b;
        }

        private void ui_exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void ui_aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Feature [] featArr = _controller.GetFeatures (FeatureType.Airspace, 
            //    InterpretationType.BASELINE, DateTime.Now, null);
            //return;

            AboutForm aboutForm = new AboutForm();
            aboutForm.ShowDialog(this);
        }

        private void ui_FeaturesDGV_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            FeaturesCountChanged();
        }

        private void ui_FeaturesDGV_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            FeaturesCountChanged();
        }

        private void ui_quickFeatTypeSearchTB_Enter(object sender, EventArgs e)
        {
            ui_quickSearchTextBox.Text = "";
            _quickSearchTextTyped = true;
            ui_quickSearchTextBox.Font = new Font(ui_quickSearchTextBox.Font, FontStyle.Regular);
            ui_quickSearchTextBox.ForeColor = SystemColors.ControlText;
            ui_quickSearchTextBox.TextChanged += new EventHandler(ui_quickSearchTextBox_TextChanged);
        }

        private void ui_quickFeatTypeSearchTB_Leave(object sender, EventArgs e)
        {
            ui_quickSearchTextBox.TextChanged -= new EventHandler(ui_quickSearchTextBox_TextChanged);
            ui_quickSearchTextBox.Text = "Quick Search";
            ui_quickSearchTextBox.Font = new Font(ui_quickSearchTextBox.Font, FontStyle.Italic);
            ui_quickSearchTextBox.ForeColor = Color.Gray;
        }

        private void ui_quickSearchTextBox_TextChanged(object sender, EventArgs e)
        {
            string cellValue;
            foreach (DataGridViewRow row in ui_FeatureTypesDGV.Rows)
            {
                cellValue = row.Cells[0].Value.ToString().ToLower();
                row.Visible = cellValue.Contains(ui_quickSearchTextBox.Text.ToLower());
            }
        }

        private void SetAllRowVisible(bool visible)
        {
            foreach (DataGridViewRow item in ui_FeatureTypesDGV.Rows)
            {
                item.Visible = visible;
            }
        }

        private void ui_navFlowLayoutPanel_ControlAdded(object sender, ControlEventArgs e)
        {
        }

        private void ui_navFlowLayoutPanel_ControlRemoved(object sender, ControlEventArgs e)
        {
        }

        private void ui_newButton_Click(object sender, EventArgs e)
        {
            if (_currentFeatureName == null)
                return;

            FeatureType featureType;
            if (!Enum.TryParse<FeatureType>(_currentFeatureName, out featureType))
            {
                return;
            }

            Feature newFeature = _controller.CreateNewFeature(featureType, GetLastNavFeature());
            _controller.InsertAsCorrection = false;

            ShowFeatureWindow(newFeature);


            //int newRowIndex = ui_FeaturesDGV.Rows.Add ();
            //DataGridViewRow newRow = ui_FeaturesDGV.Rows [newRowIndex];
            //newRow.Tag = newFeature;
            //ui_FeaturesDGV.CurrentCell = newRow.Cells [0];

            //ui_editas_NewSequenceTSMenuItem_Click (ui_editSequenceTSMI, null);
        }

        private void EditAs_NewSequence_Click(object sender, EventArgs e)
        {
            if (ui_FeaturesDGV.CurrentRow == null)
                return;

            var feature = ui_FeaturesDGV.CurrentRow.Tag as Feature;
            _controller.InsertAsCorrection = false;
            ShowFeatureWindow(feature);
        }

        private void ShowFeatureWindow(Feature feature)
        {
            if (feature.Id >= 0)
                feature = _controller.LoadFullFeature(feature);

            FeatureViewerForm featViewerForm = new FeatureViewerForm();
            featViewerForm.DefaultEffectiveDate = _settings.EffectiveDate;
            featViewerForm.Text = feature.FeatureType + " - Edit";
            featViewerForm.Width = 530;
            featViewerForm.StartPosition = FormStartPosition.CenterParent;
            featViewerForm.GetFeature += _controller.FeatureControl_GetFeature;
            featViewerForm.FeatureSaved += DbEntityControl_Saved;
            featViewerForm.GetFeatsListByDepend += _controller.GetFeaturesSingleThread;
            featViewerForm.DataGridColumnsFilled = _controller.FillDataGridColumns;
            featViewerForm.DataGridRowSetted = _controller.SetRow;
            featViewerForm.SetFeature(feature);
            featViewerForm.ShowInTaskbar = true;
            featViewerForm.ShowToolbar = false;
            featViewerForm.ShowGeometryClicked += FeatViewerForm_ShowGeometryClicked;
            featViewerForm.FormClosed += FeatViewerForm_FormClosed;
            featViewerForm.AsCorrection = _controller.InsertAsCorrection;

            if (GetFeatureListHandler != null)
                featViewerForm.GetFeatureListHandler = GetFeatureListHandler;

            featViewerForm.OKClicked += FeatViewerForm_OKClicked;

            featViewerForm.Show(Owner != null ? Owner : this);

            featViewerForm.HideLeftPanel();
        }

        private void FeatViewerForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (_lastPolyControl != null)
                _lastPolyControl.Clear();
        }

        private void FeatViewerForm_OKClicked(object sender, EventArgs e)
        {
            var featViewerForm = sender as FeatureViewerForm;
            var feature = featViewerForm.GetMainFeature();
            var isNew = (feature.Id <= 0);

            var result = _controller.SaveFeature(feature);

            if (result != string.Empty)
            {
                MessageBox.Show(result, "Error ...", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                featViewerForm.Close();

                if (!isNew)
                {
                    for (int i = 0; i < ui_FeaturesDGV.Rows.Count; i++)
                    {
                        Feature rowFeature = (Feature)ui_FeaturesDGV.Rows[i].Tag;
                        if (rowFeature.Identifier.Equals(feature.Identifier))
                        {
                            _controller.SetRow(ui_FeaturesDGV, feature, i);
                            break;
                        }
                    }
                }
                else
                {
                    int newRowIndex = ui_FeaturesDGV.Rows.Add();
                    DataGridViewRow newRow = ui_FeaturesDGV.Rows[newRowIndex];
                    newRow.Tag = feature;
                    _controller.SetRow(ui_FeaturesDGV, feature, newRowIndex);
                    ui_FeaturesDGV.CurrentCell = newRow.Cells[0];
                }
            }
        }

        private void FeatViewerForm_ShowGeometryClicked(object sender, ShowGeometryEventArgs e)
        {
            if (Globals.Environment == null)
                return;

            var polyCont = new PolyCreatorControl();
            polyCont.FeatureType = e.FeatureType;
            polyCont.OwnerForm = sender as Form;
            polyCont.PathText = e.PathText;
            polyCont.ShowGeometryEventArgs = e;
            polyCont.BackClicked += PolyControl_BackClicked;
            polyCont.OKClicked += PolyControl_OKClicked;
            polyCont.Dock = DockStyle.Fill;
            e.ReplacedControl.Visible = false;
            e.ReplacedControl.Parent.Controls.Add(polyCont);
            polyCont.GeomValue = e.GeomValue;

            _lastPolyControl = polyCont;
        }

        private void PolyControl_BackClicked(object sender, EventArgs e)
        {
            var polyCont = sender as PolyCreatorControl;
            var rc = polyCont.ShowGeometryEventArgs.ReplacedControl;
            rc.Parent.Controls.Remove(polyCont);
            polyCont.Clear();
            rc.Visible = true;
            _lastPolyControl = null;
        }

        private void PolyControl_OKClicked(object sender, EventArgs e)
        {
            var polyCont = sender as PolyCreatorControl;

            var geom = polyCont.GeomValue;
            polyCont.ShowGeometryEventArgs.GeomValue.Assign(geom);

            PolyControl_BackClicked(sender, e);
        }

        private void EditAs_Correction_Click(object sender, EventArgs e)
        {
            if (ui_FeaturesDGV.CurrentRow == null)
                return;

            var feature = ui_FeaturesDGV.CurrentRow.Tag as Feature;
            _controller.InsertAsCorrection = true;

            ShowFeatureWindow(feature);
        }

        private void ui_EditAsTSSB_Click(object sender, EventArgs e)
        {
            EditAs_Correction_Click(null, null);
        }

        private void EffectiveDate_ValueChanged(object sender, EventArgs e)
        {
            var dt = ui_effectiveDateTimePicker.Value;
            _settings.EffectiveDate = dt;
            InputFormController.DefaultEffectiveDate = dt;

            LoadFeatures();
        }

        private void FillFeatureTypesDGV(IEnumerable<string> featureNames, string selectedFeature = null)
        {
            ui_FeatureTypesDGV.CurrentCellChanged -= new EventHandler(ui_FeatureTypesDGV_CurrentCellChanged);
            ui_FeatureTypesDGV.SuspendLayout();
            ui_FeatureTypesDGV.Rows.Clear();
            bool b = true;

            foreach (string featName in featureNames)
            {
                if (_controller.DbProvider.CurrentUser.Privilege != Privilige.prAdmin)
                {
                    if (!_controller.DbProvider.CurrentUser.ContainsFeatType(featName))
                        continue;
                }
                int index = ui_FeatureTypesDGV.Rows.Add(featName);

                if (b && featName.Equals(selectedFeature))
                {
                    ui_FeatureTypesDGV.Rows[index].Selected = true;
                    b = false;
                }
            }

            ui_FeatureTypesDGV.Sort(ui_FeatureNameColumn, ListSortDirection.Ascending);
            ui_FeatureTypesDGV.CurrentCellChanged += new EventHandler(ui_FeatureTypesDGV_CurrentCellChanged);
            ui_FeatureTypesDGV.ResumeLayout(true);

            if (!b && ui_FeatureTypesDGV.SelectedRows.Count > 0)
                ui_FeatureTypesDGV.CurrentCell = ui_FeatureTypesDGV.SelectedRows[0].Cells[0];
            else if (ui_FeatureTypesDGV.Rows.Count > 0)
                ui_FeatureTypesDGV.CurrentCell = ui_FeatureTypesDGV.Rows[0].Cells[0];

            ui_FeatureTypesDGV_CurrentCellChanged(ui_FeatureTypesDGV, null);
        }

        private void LoadFeatures()
        {

            if (_currentFeatureName == null)
                return;

            FeatureType featureType;
            if (!Enum.TryParse<FeatureType>(_currentFeatureName, out featureType))
            {
                ui_FeaturesDGV.Columns.Clear();
                ui_statusLabel.Text = "Abstract Feature has not realized yet...";
                return;
            }

            var dt = ui_effectiveDateTimePicker.Value;

            ui_FeaturesDGV.Visible = false;

            _controller.GetFeatures(
                featureType,
                TimeSliceInterpretationType.BASELINE,
                dt,
                GetLastNavFeature());
        }

        private void Controller_FeaturesLoaded(object sender, FeatureListLoadedEventArgs e)
        {
            EventHandler eh = new EventHandler(DoFillFeaturesDGV);
            Invoke(eh, null, e);
        }

        private void DoFillFeaturesDGV(object sender, EventArgs e)
        {
            var ea = e as FeatureListLoadedEventArgs;

            if (ea.Exception != null)
                MessageBox.Show(ea.Exception.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
                FillFeaturesDGV(ea.FeatureList, ea.FeatureType);

            ui_FeaturesDGV.Visible = true;
        }

        private void FillFeaturesDGV(IEnumerable<Feature> featureArr, FeatureType featureType)
        {
            _controller.FillColumns(featureType, ui_FeaturesDGV);

            foreach (Feature feature in featureArr)
                _controller.SetRow(ui_FeaturesDGV, feature);

            if (ui_FeaturesDGVSortedColumnIndex > -1)
                ui_FeaturesDGV.Sort(ui_FeaturesDGV.Columns[ui_FeaturesDGVSortedColumnIndex],
                    ui_FeaturesDGVSortdOrder == SortOrder.Ascending ? ListSortDirection.Ascending : ListSortDirection.Descending);

            ui_FeaturesDGVSortedColumnIndex = -1;

            if (ui_FeaturesDGVSelectedRowIndex > -1)
            {
                ui_FeaturesDGV.ClearSelection();
                if (ui_FeaturesDGV.Rows.Count > 0)
                    ui_FeaturesDGV.Rows[Math.Min(ui_FeaturesDGV.Rows.Count - 1, ui_FeaturesDGVSelectedRowIndex)].Selected =
                        true;
            }
            ui_FeaturesDGVSelectedRowIndex = -1;
        }

        private void DbEntityControl_Closed(object sender, FormClosingEventArgs e)
        {
            FeatureControl featureControl = sender as FeatureControl;
            if (featureControl.HasChanged(_controller.InsertAsCorrection))
            {
                DialogResult dlgRes = MessageBox.Show("Do you want to save changes?", Text,
                    MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

                if (dlgRes == DialogResult.Cancel)
                {
                    e.Cancel = true;
                    return;
                }
                else if (dlgRes == DialogResult.Yes)
                {
                    Feature editingFeature = featureControl.GetEditingFeature();
                    string result = _controller.SaveFeature(editingFeature);
                    if (result != string.Empty)
                        MessageBox.Show(result, "Error ...", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    else
                        _controller.SetRow(ui_FeaturesDGV, editingFeature, ui_FeaturesDGV.CurrentRow.Index);
                }
                else if (featureControl.RootFeature.Id == -1)
                {
                    ui_FeaturesDGV.Rows.Remove(ui_FeaturesDGV.CurrentRow);
                }
            }
            else if (featureControl.RootFeature.Id == -1 && (ui_FeaturesDGV.CurrentRow.Tag as Feature).Id == -1)
            {
                ui_FeaturesDGV.Rows.Remove(ui_FeaturesDGV.CurrentRow);
            }

            ui_MainSplitContainer.Visible = true;
            ui_navFlowLayoutPanel.Enabled = true;
        }

        private void DbEntityControl_Saved(object sender, FeatureEventArgs e)
        {
            SaveFeature(e.Feature);
        }

        private void SaveFeature(Feature feature)
        {
            string result = _controller.SaveFeature(feature);

            if (result != string.Empty)
            {
                MessageBox.Show(result, "Error ...", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                for (int i = 0; i < ui_FeaturesDGV.Rows.Count; i++)
                {
                    Feature rowFeature = (Feature)ui_FeaturesDGV.Rows[i].Tag;
                    if (rowFeature.Identifier.Equals(feature.Identifier))
                    {
                        _controller.SetRow(ui_FeaturesDGV, feature, i);
                        break;
                    }
                }
            }
        }

        private void AddNavigationControl(Feature feature)
        {
            string s = _controller.GetFeatureDescription(feature);
            if (s != null)
                s = feature.FeatureType + " ( " + s + " )";
            else
                s = feature.FeatureType.ToString();

            LinkLabel linkLabel = new LinkLabel();
            linkLabel.Text = s;
            linkLabel.Tag = feature;

            linkLabel.Margin = new Padding(5);
            linkLabel.AutoSize = true;
            linkLabel.Click += new EventHandler(NavigatorLinkLabel_Click);

            ui_navFlowLayoutPanel.Controls.Add(linkLabel);
        }

        private void NavigatorLinkLabel_Click(object sender, EventArgs e)
        {
            Feature feature = ((Control)sender).Tag as Feature;

            for (int i = 0; i < ui_navFlowLayoutPanel.Controls.Count; i++)
            {
                if (feature == (Feature)ui_navFlowLayoutPanel.Controls[i].Tag)
                {
                    while (i < ui_navFlowLayoutPanel.Controls.Count)
                        ui_navFlowLayoutPanel.Controls.RemoveAt(i);
                    break;
                }
            }

            string dependsFeatureName = _controller.GetDependsFeature(feature);
            var featureNames = _controller.GetFeaturesByDepends(dependsFeatureName);
            FillFeatureTypesDGV(featureNames, feature.FeatureType.ToString());

            foreach (DataGridViewRow row in ui_FeaturesDGV.Rows)
            {
                if (feature == (Feature)row.Tag)
                {
                    ui_FeaturesDGV.CurrentCell = row.Cells[0];
                    break;
                }
            }
        }

        private Feature GetLastNavFeature()
        {
            if (ui_navFlowLayoutPanel.Controls.Count == 0)
                return null;
            return (Feature)ui_navFlowLayoutPanel.Controls[ui_navFlowLayoutPanel.Controls.Count - 1].Tag;
        }

        private void FeaturesCountChanged()
        {
            ui_statusLabel.Text = string.Format("Features count: {0}. Selected: {1}",
                ui_FeaturesDGV.Rows.Count,
                ui_FeaturesDGV.SelectedRows.Count);
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (!_externalgDbProvider && _controller.DbProvider != null)
                _controller.DbProvider.Close();

            _controller.SaveUIMetadata();
            _settings.Save();
        }

        private void ExportToXml_Click(object sender, EventArgs e)
        {
            var fef = new FeatureExporterForm();
            fef.ExportAsSeperatedFileClicked += FeatureExporterForm_ExportAsSeperatedFileClicked;
            fef.SetSelectedFeatures(_controller.ForExportingList);
            fef.SetEffectiveDate(InputFormController.DefaultEffectiveDate);

            if (fef.ShowDialog() != DialogResult.OK)
                return;

            Application.DoEvents();
            Cursor = Cursors.WaitCursor;

            List<Exception> excList = null;

            if (fef.IsAllFeatures)
            {
                excList = _controller.ExportToXML(fef.FileName, fef.IsWriteExtensions, fef.LoadFeatureAllVersion, fef.Write3DIfExists, fef.SrsType);
            }
            else
            {
                excList = _controller.ExportSelectedToXml(fef.FileName, fef.IsWriteExtensions, fef.IncludeFeatRefs, fef.Write3DIfExists, fef.SrsType);
            }

            Cursor = Cursors.Default;

            if (excList == null || excList.Count == 0)
            {
                MessageBox.Show("Successfully exported.");
            }
            else
            {
                var exportReport = new FormReport();
                exportReport.Show(Text, excList);
            }
        }

        private void FeatureExporterForm_ExportAsSeperatedFileClicked(object sender, EventArgs e)
        {
            var fef = sender as FeatureExporterForm;

            var fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog() != DialogResult.OK)
                return;

            string resultMessage;
            var isOK = _controller.ExportAsSeperate(fbd.SelectedPath, fef.IsWriteExtensions, fef.IncludeFeatRefs, fef.Write3DIfExists, fef.SrsType, out resultMessage);

            if (resultMessage != null)
                MessageBox.Show(resultMessage, Text, MessageBoxButtons.OK, (isOK ? MessageBoxIcon.Information : MessageBoxIcon.Warning));

            //fef.Close();
        }

        private void ImportFromXml_Click(object sender, EventArgs e)
        {
            var ofd = new OpenFileDialog();
            ofd.Filter = "XML File (*.xml)|*.xml|All Files (*.*)|*.*";

            if (ofd.ShowDialog() != DialogResult.OK)
                return;

            _controller.Import(ofd.FileName);
        }

        private void ui_FeaturesDGV_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right)
                return;

            DataGridView dgv = sender as DataGridView;
            DataGridView.HitTestInfo hti = dgv.HitTest(e.X, e.Y);

            if (hti.ColumnIndex == -1 && hti.RowIndex == -1)
            //if (hti.ColumnIndex == -1 && hti.RowIndex == -1 &&
            //    hti.ColumnX == 1 && hti.RowY == 1)
            {
                ShowFieldContextMenu();
            }
            else if (hti.RowIndex > -1 && hti.ColumnIndex > -1)
            {
                ui_addForExpContextMenu.Show(ui_FeaturesDGV, e.Location);
            }
        }

        private void ShowFieldContextMenu()
        {
            FeatureType featureType = GetCurrentFeatureType();

            UIUtilities.ShowFieldsContextMenu(ui_FeaturesDGV,
                UIMetadata.Instance.GetClassInfo((int)featureType), DataGridView_Refresh);
        }

        private void DataGridView_Refresh(object sender, EventArgs e)
        {
            LoadFeatures();
        }

        private FeatureType GetCurrentFeatureType()
        {
            FeatureType featureType;
            Enum.TryParse<FeatureType>(_currentFeatureName, out featureType);
            return featureType;
        }

        private void FeaturesDGV_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            EditAs_Correction_Click(null, null);
        }

        private void FeaturesDGV_SelectionChanged(object sender, EventArgs e)
        {
            FeaturesCountChanged();
        }

        private void Delete_Click(object sender, EventArgs e)
        {
            if (ui_FeaturesDGV.CurrentRow == null)
                return;


            if (ui_FeaturesDGV.SortedColumn != null)
            {
                ui_FeaturesDGVSortedColumnIndex = ui_FeaturesDGV.SortedColumn.Index;
                ui_FeaturesDGVSortdOrder = ui_FeaturesDGV.SortOrder;
            }

            List<Feature> features = new List<Feature>();
            foreach (var selectedRow in ui_FeaturesDGV.SelectedRows)
            {
                var index = ((DataGridViewRow)selectedRow).Index;
                features.Add((Feature)((DataGridViewRow)selectedRow).Tag);

                ui_FeaturesDGVSelectedRowIndex = ui_FeaturesDGVSelectedRowIndex == -1 ? index : Math.Min(index, ui_FeaturesDGVSelectedRowIndex);
            }
            //            Feature feature = (Feature)ui_FeaturesDGV.CurrentRow.Tag;

            var sdf = new SelectDateForm();
            sdf.SetMinDateTime(features.Select(t => t.TimeSlice.ValidTime.BeginPosition).Max() /*feature.TimeSlice.ValidTime.BeginPosition*/);
            if (sdf.ShowDialog() != DialogResult.OK)
                return;

            features.ForEach(feature => _controller.DeleteFeature(feature, sdf.GetDateTime()));
            //_controller.DeleteFeature(feature, sdf.GetDateTime());
            ui_FeatureTypesDGV_CurrentCellChanged(null, null);




            //if (MessageBox.Show ("Are you sure to delete ?", "Deleting feature",
            //    MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            //{
            //    Feature feature = (Feature) ui_FeaturesDGV.CurrentRow.Tag;
            //    _controller.DeleteFeature (feature);
            //    ui_FeatureTypesDGV_CurrentCellChanged (null, null);
            //}
        }

        private void View_Click(object sender, EventArgs e)
        {
            if (ui_FeaturesDGV.CurrentRow == null)
                return;

            var feature = (Feature)ui_FeaturesDGV.CurrentRow.Tag;
            _controller.ViewFeature(feature, this);
        }

        private void ViewHistory_Click(object sender, EventArgs e)
        {
            if (ui_FeaturesDGV.CurrentRow == null)
                return;

            var feature = (Feature)ui_FeaturesDGV.CurrentRow.Tag;
            _controller.ViewFeatureHistory(feature, this);
        }

        private void Options_Click(object sender, EventArgs e)
        {
            var of = new OptionsForm();
            of.OKClicked += OptionsForm_OKClicked;
            of.EnableConnectionControl = !_externalgDbProvider;
            of.SetConnection(_connection != null ? _connection : _settings.Connection);

            if (of.ShowDialog() == DialogResult.OK)
            {
                if (of.IsConnectionChanged)
                {
                    _settings.Connection = of.GetConnection();
                    if (_controller.DbProvider != null && _controller.DbProvider.State == ConnectionState.Open)
                        _controller.DbProvider.Close();

                    _controller.DbProvider = of.Tag as DbProvider;
                }
            }
        }

        private void OptionsForm_OKClicked(object sender, EventArgs e)
        {
            var of = sender as OptionsForm;
            var conn = of.GetConnection();
            var dbPro = CreateDbProvider(conn.ConnectionType);

            try
            {
                dbPro.Open(conn.GetConnectionString());

                if (!dbPro.Login(conn.UserName, DbUtility.GetMd5Hash(conn.Password)))
                {
                    dbPro.Close();
                    throw new Exception("Invalid User Name or Password!");
                }
                dbPro.DefaultEffectiveDate = EffectiveDate;
                of.Tag = dbPro;
                of.DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                MessageBox.Show(of, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private DateTime EndOfDay(DateTime dt)
        {
            return new DateTime(dt.Year, dt.Month, dt.Day, 23, 59, 00);
        }

        private void FeaturesDGV_SizeChanged(object sender, EventArgs e)
        {
            ui_loadingPictureBox.Location = ui_FeaturesDGV.Location;
            ui_loadingPictureBox.Size = ui_FeaturesDGV.Size;
        }

        private void FeaturesDGV_VisibleChanged(object sender, EventArgs e)
        {
            ui_loadingPictureBox.Visible = !ui_FeaturesDGV.Visible;
        }

        private void AddForExporting_Click(object sender, EventArgs e)
        {
            if (ui_FeaturesDGV.CurrentRow == null)
                return;

            foreach (DataGridViewRow row in ui_FeaturesDGV.SelectedRows)
            {
                var feature = (Feature)row.Tag;
                _controller.AddForExporting(feature.FeatureType, feature);
            }
        }

        private void FindFeatureType_Click(object sender, EventArgs e)
        {
            var form = new FindFeatureTypeForm();
            form.Show(this);
        }

        private void SearchPanel_Leave(object sender, EventArgs e)
        {
            ui_searchPanel.Visible = false;
        }

        private void CloseFindPanelLabel_Click(object sender, EventArgs e)
        {
            ui_searchPanel.Visible = false;
        }

        private void FindNext_Click(object sender, EventArgs e)
        {
            int rowIndex = 0;
            int colIndex = 0;

            if (ui_FeaturesDGV.CurrentCell != null)
            {
                rowIndex = ui_FeaturesDGV.CurrentCell.RowIndex;
                colIndex = ui_FeaturesDGV.CurrentCell.ColumnIndex + 1;
            }

            bool b = Search(ui_searchTB.Text, rowIndex, colIndex);

            if (!b)
            {
                rowIndex = 0;
                colIndex = 0;
                Search(ui_searchTB.Text, rowIndex, colIndex);
            }
        }

        private bool Search(string text, int rowIndex = 0, int colIndex = 0)
        {
            string a = text.ToLower();
            int colCount = ui_FeaturesDGV.Columns.GetColumnCount(DataGridViewElementStates.Visible);

            for (int i = rowIndex; i < ui_FeaturesDGV.Rows.Count; i++)
            {
                DataGridViewRow row = ui_FeaturesDGV.Rows[i];

                for (int j = colIndex; j < colCount; j++)
                {
                    DataGridViewColumn col = ui_FeaturesDGV.Columns[j];
                    if (!col.Visible)
                        continue;

                    object val = row.Cells[col.Index].Value;
                    if (val != null)
                    {
                        string b = val.ToString().ToLower();

                        if (b.IndexOf(a) >= 0)
                        {
                            //SetFindResultColor (i, j);
                            ui_FeaturesDGV.CurrentCell = row.Cells[col.Index];
                            return true;
                        }
                    }
                }

                colIndex = 0;
            }

            return false;
        }

        private void QuickSearch_Click(object sender, EventArgs e)
        {
            ui_searchPanel.Visible = true;
            ui_searchTB.Text = string.Empty;
            ui_searchTB.Focus();
        }

        private void SearchTB_TextChanged(object sender, EventArgs e)
        {
            if (ui_searchTB.Text.Length == 0)
                return;

            Search(ui_searchTB.Text);
        }


        public static DbProvider CreateDbProvider(ConnectionType connType)
        {
            if (connType == ConnectionType.Aran)
                return DbProviderFactory.Create("Aran.Aim.Data.PgDbProviderComplex");
            if (connType == ConnectionType.ComSoft)
                return DbProviderFactory.Create("Aran.Aim.CawProvider");
            if (connType == ConnectionType.XmlFile)
                return DbProviderFactory.Create("Aran.Aim.Data.XmlProvider");
            if (connType == ConnectionType.TDB)
                return DbProviderFactory.Create("Aran.Temporality.Provider");
            return null;
        }

        private void FeatTypesByClassified_CheckedChanged(object sender, EventArgs e)
        {
            _controller.IsFeatureClassified = ui_featTypesByClassifiedTSMI.Checked;

            ui_nextTSB.Visible = _controller.IsFeatureClassified;
            ui_nextButtonSeparator.Visible = _controller.IsFeatureClassified;

            var featureNames = _controller.GetFeaturesByDepends(null);
            FillFeatureTypesDGV(featureNames);
        }
    }
}