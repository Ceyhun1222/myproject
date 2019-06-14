using ArenaStatic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ARENA;
using PDM;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.DataSourcesGDB;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geometry;
using EsriWorkEnvironment;

namespace SigmaChart.CmdsMenu
{
    public partial class STARWizardForm : Form
    {

        public string _FolderName;
        public string _ProjectName;
        public string _TemplatetName;
        public double mapSize_Height;
        public double mapSize_Width;
        public bool _RNAVflag;
        public bool _AllVOR_DMEflag;
        public int _AiracCircle;
        public double _mapScale;
        public bool _firstProcDesignator;

        public IAOIBookmark _bookmark;


        private List<AirportHeliport> AdhpList;
        private IFeatureLayer2 _Layer;
        public UOM_DIST_VERT vertUom = UOM_DIST_VERT.FT;
        public UOM_DIST_HORZ distUom = UOM_DIST_HORZ.NM;
        public int arspBufWidth = 2;
        private List<PDMObject> HldList;
        public bool arspSign = false;


        public AirportHeliport selectedADHP = null;
        public List<PDMObject> selectedSTAR = new List<PDMObject>();
        public SafeAltitudeArea selectedMSA = null;
        private List<PDMObject> msa_list = null;
        public List<string> _selectedChanels = null;
        public List<PDMObject> _selectedHoldings = null;
        public int VS_Radius = 5000;
        public int VS_Min_Elev = 0;
        public bool allRwyFlag = false;
        public IEnvelope _ext = null;

        public STARWizardForm()
        {
            ESRI.ArcGIS.RuntimeManager.BindLicense(ESRI.ArcGIS.ProductCode.Desktop);

            InitializeComponent();
            _FolderName = "";
            TemplateListComboBox.Items.Clear();
            var tmp = ArenaStaticProc.GetPathToTemplate() + @"\STAR\";

            string[] FN = Directory.GetFiles(tmp, "*.mxd");
            foreach (var fl in FN)
            {
                TemplateListComboBox.Items.Add(System.IO.Path.GetFileName(fl));
            }

            TemplateListComboBox.SelectedIndex = 0;
            _FolderName = ArenaStaticProc.GetPathToMapFolder();

            textBox2.Text = _FolderName;

            AdhpList = DataCash.GetAirportlist();
            comboBox1.Items.Clear();

            foreach (var itemADHP in AdhpList)
            {
                comboBox1.Items.Add(itemADHP.Designator);
            }

            if (comboBox1.Items.Count > 0) comboBox1.SelectedIndex = 0;

            _AllVOR_DMEflag = false;

        }

        public STARWizardForm(int AiracCircle)
        {
            ESRI.ArcGIS.RuntimeManager.BindLicense(ESRI.ArcGIS.ProductCode.Desktop);

            InitializeComponent();
            _FolderName = "";
            TemplateListComboBox.Items.Clear();
            var tmp = ArenaStaticProc.GetPathToTemplate() + @"\STAR\";

            string[] FN = Directory.GetFiles(tmp, "*.mxd");
            foreach (var fl in FN)
            {
                TemplateListComboBox.Items.Add(System.IO.Path.GetFileName(fl));
            }

            TemplateListComboBox.SelectedIndex = 0;
            _FolderName = ArenaStaticProc.GetPathToMapFolder();

            textBox2.Text = _FolderName;

            AdhpList = DataCash.GetAirportlist();

            comboBox1.Items.Clear();

            foreach (var itemADHP in AdhpList)
            {
                comboBox1.Items.Add(itemADHP.Designator);
            }

            if (comboBox1.Items.Count > 0) comboBox1.SelectedIndex = 0;

            airacControl1.AiracCircleValue = AiracCircle;
            _AllVOR_DMEflag = false;


            HldList = DataCash.GetObjectsByType(PDM_ENUM.HoldingPattern);
            _selectedChanels = new List<string>();
        }


        private int BuildProceduresTree(AirportHeliport ADHP, string _landinArea = "")
        {
            listView1.BeginUpdate();


            listView1.Items.Clear();
            listView1.Columns[0].Width = 200;
            listView1.Columns[1].Width = 100;
            listView1.Columns[2].Width = 128;

            var procList = DataCash.GetAirportProcedures(PDM.PROC_TYPE_code.STAR, ADHP.ID, checkBox1.Checked);

            if (_landinArea.CompareTo("") == 0)
            {
                comboBox3.Items.Clear();
                comboBox3.Items.Add("");
            }

            foreach (Procedure itemProc in procList)
            {
                ListViewItem item1 = new ListViewItem(itemProc.GetObjectLabel());
                if (itemProc.LandingArea != null)
                {
                    string LA = "";
                    for (int i = 0; i < itemProc.LandingArea.Count; i++)
                    {
                        LA = i != itemProc.LandingArea.Count - 1 ? LA + itemProc.LandingArea[i].ToString() + "/" : LA + itemProc.LandingArea[i].ToString();
                    }
                    if (_landinArea.CompareTo("") != 0 && LA.CompareTo(_landinArea) != 0) continue;
                    item1.SubItems.Add(LA);

                    if (!comboBox3.Items.Contains(LA) && _landinArea.CompareTo("") == 0) comboBox3.Items.Add(LA);

                }
                else item1.SubItems.Add(" ");

                if (itemProc.AircraftCharacteristic != null && itemProc.AircraftCharacteristic.Count > 0)
                {
                    string AC = "";
                    for (int i = 0; i < itemProc.AircraftCharacteristic.Count; i++)
                    {
                        AC = i != itemProc.AircraftCharacteristic.Count - 1 ? AC + itemProc.AircraftCharacteristic[i].AircraftLandingCategory.ToString() + "/" : AC + itemProc.AircraftCharacteristic[i].AircraftLandingCategory.ToString();
                    }
                    item1.SubItems.Add(AC);
                }

                item1.Tag = itemProc;

                listView1.Items.Add(item1);
            }


            listView1.EndUpdate();

            listView1_ColumnClick(listView1, new ColumnClickEventArgs(1));

            if (listView1.Items.Count <= 0 && _Layer != null)
            {
                IFeatureLayerDefinition FD = (ESRI.ArcGIS.Carto.IFeatureLayerDefinition)_Layer;

                if (FD != null)
                {
                    FD.DefinitionExpression = "ID_Transition IN ('0')";
                    ((IActiveView)axPageLayoutControl1.ActiveView.FocusMap).Refresh();
                }
            }

            return procList != null ? procList.Count : 0;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            _ProjectName = textBox1.Text;
            _FolderName = textBox2.Text;
            _TemplatetName = TemplateListComboBox.Text;
            _RNAVflag = checkBox1.Checked;
            _mapScale = axPageLayoutControl1.ActiveView.FocusMap.MapScale;
            if (_selectedChanels == null) { _selectedChanels = new List<string>(); _selectedChanels.Add(TerminalChartsUtil.getChanelsString(selectedADHP.CommunicationChanels[0])); }

            if (!TerminalChartsUtil.CheckFileExisting(_ProjectName, _FolderName)) return;

            selectedSTAR = new List<PDMObject>();
            foreach (ListViewItem item in listView1.Items)
            {
                if (item.Checked) selectedSTAR.Add((Procedure)item.Tag);
            }

            if (msa_list != null && msa_list.Count > 0 && comboBox2.SelectedIndex >= 0)
                selectedMSA = (SafeAltitudeArea)msa_list[comboBox2.SelectedIndex];

            ArenaStaticProc.SetPathToMapFolder(_FolderName);

            _AiracCircle = airacControl1.AiracCircleValue;

            _bookmark = ChartsHelperClass.CreateBookmark(axPageLayoutControl1.ActiveView.FocusMap, "SigmaSTAR_Bookmark");

            ChartsHelperClass.SetMapGridVisibilityState(axPageLayoutControl1.ActiveView, true);

            #region GetMapExtent

            ILayer _Layer = EsriUtils.getLayerByName(axPageLayoutControl1.ActiveView.FocusMap, "ProcedureLegs");
            var fc = ((IFeatureLayer)_Layer).FeatureClass;

            ISpatialReference pSpatialReference = (fc as IGeoDataset).SpatialReference;

            _ext = (axPageLayoutControl1.ActiveView.FocusMap as IActiveView).Extent;
            _ext = EsriUtils.ToGeo(_ext, axPageLayoutControl1.ActiveView.FocusMap, pSpatialReference).Envelope;


            #endregion

            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        private void button2_Validated(object sender, EventArgs e)
        {
            textBox1.Text = textBox1.Text.Length <= 0 ? "NewChart" : textBox1.Text;


        }

        private void button1_Click(object sender, EventArgs e)
        {
            var folderBrowserDialog1 = new FolderBrowserDialog
            {
                ShowNewFolderButton = true,
                SelectedPath = ArenaStaticProc.GetPathToMapFolder(),
            };
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox2.Text = folderBrowserDialog1.SelectedPath;
            }
        }

        private void treeView1_AfterCheck(object sender, TreeViewEventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            button2.Enabled = false;
            int pc = 0;
            comboBox2.Items.Clear();
            comboBox3.Items.Clear();
            comboBox3.Text = "";
            if (AdhpList != null && AdhpList.Count > 0)
            {
                listView1.Items.Clear();
                pc = BuildProceduresTree(AdhpList[comboBox1.SelectedIndex]);
                selectedADHP = AdhpList[comboBox1.SelectedIndex];
                linkLabel1.Text = selectedADHP.CommunicationChanels != null && selectedADHP.CommunicationChanels.Count > 0 ? "Communication chanels ....." : "";

                VS_Min_Elev = selectedADHP.Elev.HasValue ? (int)selectedADHP.ConvertValueToMeter(selectedADHP.Elev, selectedADHP.Elev_UOM.ToString()) : 0;

                msa_list = ChartsHelperClass.BuildMSAList(AdhpList[comboBox1.SelectedIndex]);


                if (pc > -1 && msa_list != null && msa_list.Count > 0)
                {
                    foreach (var item in msa_list)
                    {
                        string ss = (((SafeAltitudeArea)item).CentrePoint as SegmentPoint).SegmentPointDesignator;
                        comboBox2.Items.Add(ss);
                    }
                }

                if (comboBox2.Items.Count > 0) comboBox2.SelectedIndex = 0;

                if (selectedADHP != null)
                {
                    if (selectedADHP.Geo == null) selectedADHP.RebuildGeo();

                    if (selectedADHP.Geo != null)
                    {
                        double newX = ((IPoint)selectedADHP.Geo).X;
                        EsriUtils.ChangeProjectionAndMeredian(newX, axPageLayoutControl1.ActiveView.FocusMap);
                    }
                }
            }
        }


        private void listView1_ColumnClick(object sender, ColumnClickEventArgs e)
        {

            ListViewItemComparer sorter = GetListViewSorter(e.Column);

            listView1.ListViewItemSorter = sorter;
            listView1.Sort();
        }

        private ListViewItemComparer GetListViewSorter(int columnIndex)
        {
            ListViewItemComparer sorter = listView1.ListViewItemSorter as ListViewItemComparer;
            if (sorter == null)
            {
                sorter = new ListViewItemComparer();
            }

            sorter.ColumnIndex = columnIndex;

            if (sorter.SortDirection == SortOrder.Ascending)
            {
                sorter.SortDirection = SortOrder.Descending;
            }
            else
            {
                sorter.SortDirection = SortOrder.Ascending;
            }

            return sorter;
        }

        private void TemplateListComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ChartsHelperClass.SelectChartTemplate(ref axPageLayoutControl1, ArenaStaticProc.GetPathToTemplate() + @"\STAR\" + TemplateListComboBox.Text, ref listBox1, out mapSize_Height, out mapSize_Width);

            try
            {
                string ConString = ArenaStaticProc.GetTargetDB();

                IWorkspaceFactory2 workspaceFactory = (IWorkspaceFactory2)new AccessWorkspaceFactoryClass();
                IWorkspace Wksp = workspaceFactory.OpenFromFile(ConString, 0);
                IFeatureWorkspace fWksp = (IFeatureWorkspace)Wksp;
                ITable table = fWksp.OpenTable("ProcedureLegs");

                IFeatureClass pFeatureClass = (IFeatureClass)table;

                _Layer = (IFeatureLayer2)EsriUtils.getLayerByName(axPageLayoutControl1.ActiveView.FocusMap, "ProcedureLegs");

                ChartsHelperClass.SetMapGridVisibilityState(axPageLayoutControl1.ActiveView, false);


                if (_Layer != null)
                {
                    _Layer.FeatureClass = pFeatureClass;

                    ((IActiveView)axPageLayoutControl1.ActiveView.FocusMap).Extent = ((ILayer)_Layer).AreaOfInterest;
                    ((IActiveView)axPageLayoutControl1.ActiveView.FocusMap).Refresh();

                    IFeatureLayer newlayer = (IFeatureLayer)_Layer;
                    newlayer.Visible = true;

                    axPageLayoutControl1.ZoomToWholePage();
                }
                listView1_ItemChecked(sender, null);

            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Source);
            }

        }

        private void button4_Click(object sender, EventArgs e)
        {
            SidSettingsForm frm = new SidSettingsForm();
            frm.checkBox1.Checked = _AllVOR_DMEflag;

            frm.comboBoxVert.SelectedIndex = 0;
            frm.comboBoxDist.SelectedIndex = 0;
            frm.checkBox5.Checked = _firstProcDesignator;

            if (this.vertUom != UOM_DIST_VERT.OTHER) frm.comboBoxVert.Text = this.vertUom.ToString();
            if (this.distUom != UOM_DIST_HORZ.OTHER) frm.comboBoxDist.Text = this.distUom.ToString();

            frm.ShowDialog();
            _AllVOR_DMEflag = frm.checkBox1.Checked;
            this.vertUom = frm.comboBoxVert.SelectedIndex == 0 ? UOM_DIST_VERT.FT : UOM_DIST_VERT.M;
            this.distUom = frm.comboBoxDist.SelectedIndex == 0 ? UOM_DIST_HORZ.NM : UOM_DIST_HORZ.KM;
            this.arspBufWidth = (int)frm.numericUpDown1.Value;
            this.arspSign = frm.checkBox2.Checked;

            this.VS_Radius = frm.checkBox4.Checked ? (int)frm.numericUpDown2.Value : -1;
            this.VS_Min_Elev = frm.checkBox4.Checked ? (int)frm.numericUpDown3.Value : -1;

            this.allRwyFlag = frm.checkBox3.Checked;
            this._firstProcDesignator = frm.checkBox5.Checked;

        }

        private void listView1_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            if (_Layer == null) return;

            try
            {
                string filter = "";
                IFeatureLayerDefinition FD = (ESRI.ArcGIS.Carto.IFeatureLayerDefinition)_Layer;
                if (FD == null) return;

                FD.DefinitionExpression = "ID_Transition IN ('0')";

                foreach (ListViewItem item in listView1.Items)
                {
                    if (item == null) continue; // мистика

                    if (item.Checked)
                    {
                        Procedure prc = (Procedure)item.Tag;

                        foreach (var tr in prc.Transitions)
                        {
                            filter = filter + "'" + tr.FeatureGUID + "',";
                        }

                    }
                }


                FD.DefinitionExpression = "ID_Transition IN (" + filter + "'0')";
                (axPageLayoutControl1.ActiveView.FocusMap as IActiveView).Extent = ChartsHelperClass.GetLayerExtent(_Layer, FD.DefinitionExpression);

            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.StackTrace);
            }

            button2.Enabled = false;
            foreach (ListViewItem item in listView1.Items)
            {
                if (item == null) continue; // мистика


                if (item.Checked)
                {
                    button2.Enabled = true;
                    break;
                }
            }
            ((IActiveView)axPageLayoutControl1.ActiveView.FocusMap).Refresh();
        }

        private void STARWizardForm_Load(object sender, EventArgs e)
        {
            //if (!System.Diagnostics.Debugger.IsAttached)
            {
                axToolbarControl1.SetBuddyControl(axPageLayoutControl1);
                axToolbarControl2.SetBuddyControl(axPageLayoutControl1);
            }
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

            RadioCommunicationsChanelsListForm frm = new RadioCommunicationsChanelsListForm(selectedADHP.CommunicationChanels, _selectedChanels);
            frm.ShowDialog();
            _selectedChanels.Clear();
            if (frm.SelectedChanelsList != null) _selectedChanels.AddRange(frm.SelectedChanelsList);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            CalendarForm frm = new CalendarForm();
            if (frm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                airacControl1.AiracCircleValue = AiracUtil.AiracUtil.GetAiracCycleByDate(frm.selectedDate);
            }
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            _selectedHoldings = new List<PDMObject>();
            HoldingsListForm frm = new HoldingsListForm(HldList);
            frm.ShowDialog();

            if (frm.SelectedHoldingsList != null) _selectedHoldings.AddRange(frm.SelectedHoldingsList);

        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            BuildProceduresTree(AdhpList[comboBox1.SelectedIndex], comboBox3.Text);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            string InitialFoldername = ArenaStaticProc.GetMainFolder() + @"\UserTemplates";

            OpenFileDialog ofd = new OpenFileDialog()
            {
                Filter = "Files|*.mxd",
                InitialDirectory = InitialFoldername
            };

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                var tmpFolder = ArenaStaticProc.GetPathToTemplate() + @"\STAR\";
                var tmpFileName = ArenaStaticProc.GetPathToTemplate() + @"\STAR\" + ofd.SafeFileName;

                if (File.Exists(tmpFileName))
                {
                    if (MessageBox.Show("There is already a file with the same name in this location! " + Environment.NewLine
                        + "Replace the file in the destination folder with the file you are copying?", "Warning", MessageBoxButtons.YesNo,MessageBoxIcon.Warning) == DialogResult.No) return;
                }

                File.Copy(ofd.FileName, tmpFileName, true);

                TemplateListComboBox.Items.Add(System.IO.Path.GetFileName(ofd.FileName));
            }
        }
    } 
}
