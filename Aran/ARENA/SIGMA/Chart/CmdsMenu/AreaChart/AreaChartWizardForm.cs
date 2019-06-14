using ArenaStatic;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using EsriWorkEnvironment;
using ESRI.ArcGIS.DataSourcesGDB;
using PDM;
using ARENA;
using CheckComboBox;
using ESRI.ArcGIS.Geometry;

namespace SigmaChart.CmdsMenu
{
    public partial class AreaChartWizardForm : Form
    {

        public string _FolderName;
        public string _ProjectName;
        public string _TemplatetName;
       // public double _Scale;
        public double mapSize_Height;
        public double mapSize_Width;
        public int Level =0;
        public bool _RNAVflag;
        public bool _AllVOR_DMEflag = true;
        public int _AiracCircle;
        public IAOIBookmark _bookmark;
        public double _mapScale;



        private List<AirportHeliport> AdhpList;
        private List<RadioCommunicationChanel> cnlList = null;
        //private AirportHeliport selectedADHP = null;
        private IFeatureLayer2 _LayerProc;
        private IFeatureLayer2 _LayerTMA_Arspc;
        private IFeatureLayer2 _Layer;
        private List<PDMObject> tma_list;

        public List<string> _selectedChanels = null;
        public int arspBufWidth = 2;
        public bool arspSign = false;
        public UOM_DIST_VERT vertUom = UOM_DIST_VERT.FT;
        public UOM_DIST_HORZ distUom = UOM_DIST_HORZ.NM;
        public AirportHeliport selectedADHP = null;
        public List<PDMObject> selectedProcedures = new List<PDMObject>();
        public List<PDMObject> selectedTmaList = new List<PDMObject>();
        public List<Airspace> tmaLst = new List<Airspace>();

        public int VS_Radius = 5000;
        public int VS_Min_Elev = 0;
        public IEnvelope _ext = null;
        public AreaChartWizardForm()
        {
            ESRI.ArcGIS.RuntimeManager.BindLicense(ESRI.ArcGIS.ProductCode.Desktop);

            InitializeComponent();


            _FolderName = "";
            TemplateListComboBox.Items.Clear();
            var tmp = ArenaStaticProc.GetPathToTemplate() + @"\AreaChart\";

           

            string[] FN = Directory.GetFiles(tmp, "*.mxd");
            foreach (var fl in FN)
            {

                TemplateListComboBox.Items.Add(System.IO.Path.GetFileName(fl));
            }

            TemplateListComboBox.SelectedIndex = 0;
            _FolderName = ArenaStaticProc.GetPathToMapFolder();

           

            textBox2.Text = _FolderName;
            //AnnotationScaleComboBox.SelectedIndex = 7;
            comboBox1.SelectedIndex = 0;
            //numericUpDown1.Value = AiracCircle;

        }

        public AreaChartWizardForm(int AiracCircle)
        {
            ESRI.ArcGIS.RuntimeManager.BindLicense(ESRI.ArcGIS.ProductCode.Desktop);

            InitializeComponent();


            _FolderName = "";
            TemplateListComboBox.Items.Clear();
            var tmp = ArenaStaticProc.GetPathToTemplate() + @"\AreaChart\";



            string[] FN = Directory.GetFiles(tmp, "*.mxd");
            foreach (var fl in FN)
            {

                TemplateListComboBox.Items.Add(System.IO.Path.GetFileName(fl));
            }

            TemplateListComboBox.SelectedIndex = 0;
            _FolderName = ArenaStaticProc.GetPathToMapFolder();

            AdhpList = DataCash.GetAirportlist();

            //////////////////////////////////////////////////


            var ArspcList_TMA = DataCash.GetAirspaceList(AirspaceType.TMA);
            var ArspcList_TMA_P = DataCash.GetAirspaceList(AirspaceType.TMA_P);

            tma_list = new List<PDMObject>();
            if (ArspcList_TMA != null) tma_list.AddRange(ArspcList_TMA);
            if (ArspcList_TMA_P != null) tma_list.AddRange(ArspcList_TMA_P);

            if (tma_list != null && tma_list.Count > 0)
            {
                int i = 0;
                foreach (var item in tma_list)
                {
                    CCBoxItem ccItem = new CCBoxItem(((Airspace)item).GetObjectLabel(), i);
                    checkedComboBoxTMA.Items.Add(ccItem);
                    i++;
                }
            }




            /////////////////////////////////////////////////
            _selectedChanels = new List<string>();

            _AiracCircle = AiracCircle;

            textBox2.Text = _FolderName;
            comboBox1.SelectedIndex = 0;
            airacControl.AiracCircleValue = AiracCircle > 1408 ? AiracCircle : 1408;

            if (_LayerProc != null)
            {
                IFeatureLayerDefinition FD = (ESRI.ArcGIS.Carto.IFeatureLayerDefinition)_LayerProc;
                if (FD == null) return;

                FD.DefinitionExpression = "ID_Transition IN ('0')";
                ((IActiveView)axPageLayoutControl1.ActiveView.FocusMap).Refresh();
            }

        }



        private void button2_Click(object sender, EventArgs e)
        {
            _ProjectName = textBox1.Text;
            _FolderName = textBox2.Text;
            _TemplatetName = TemplateListComboBox.Text;
            if (!TerminalChartsUtil.CheckFileExisting(_ProjectName, _FolderName)) return;
            _mapScale = axPageLayoutControl1.ActiveView.FocusMap.MapScale;
            _RNAVflag = checkBox1.Checked;

            _AiracCircle = airacControl.AiracCircleValue;

            ArenaStaticProc.SetPathToMapFolder(_FolderName);

            selectedProcedures = new List<PDMObject>();
            foreach (ListViewItem item in listView1.Items)
            {
                if (item.Checked) selectedProcedures.Add((Procedure)item.Tag);
            }

            selectedTmaList = new List<PDMObject>();
            foreach (CCBoxItem item in checkedComboBoxTMA.CheckedItems)
            {
                selectedTmaList.Add( tma_list[item.Value]);
            }


            _bookmark = ChartsHelperClass.CreateBookmark(axPageLayoutControl1.ActiveView.FocusMap, "SigmaAreaChart_Bookmark");


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

        private void TemplateListComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ChartsHelperClass.SelectChartTemplate(ref axPageLayoutControl1, ArenaStaticProc.GetPathToTemplate() + @"\AreaChart\" + TemplateListComboBox.Text, ref listBox1, out mapSize_Height, out mapSize_Width);


            try
            {
                string ConString = ArenaStaticProc.GetTargetDB();

                IWorkspaceFactory2 workspaceFactory = (IWorkspaceFactory2)new AccessWorkspaceFactoryClass();
                IWorkspace Wksp = workspaceFactory.OpenFromFile(ConString, 0);
                IFeatureWorkspace fWksp = (IFeatureWorkspace)Wksp;
                ITable table = fWksp.OpenTable("RouteSegment");

                IFeatureClass pFeatureClass = (IFeatureClass)table;

                _Layer = (IFeatureLayer2)EsriUtils.getLayerByName(axPageLayoutControl1.ActiveView.FocusMap, "RouteSegment");
                _Layer.FeatureClass = pFeatureClass;



                table = fWksp.OpenTable("ProcedureLegs");

                pFeatureClass = (IFeatureClass)table;

                _LayerProc = (IFeatureLayer2)EsriUtils.getLayerByName(axPageLayoutControl1.ActiveView.FocusMap, "ProcedureLegs");
                _LayerProc.FeatureClass = pFeatureClass;

                IFeatureLayer newlayer = (IFeatureLayer)_LayerProc;
                newlayer.Visible = true;

                table = fWksp.OpenTable("AirspaceVolume");

                pFeatureClass = (IFeatureClass)table;

                _LayerTMA_Arspc = (IFeatureLayer2)EsriUtils.getLayerByName(axPageLayoutControl1.ActiveView.FocusMap, "AirspaceVolume");
                _LayerTMA_Arspc.FeatureClass = pFeatureClass;

                IFeatureLayer newlayer2 = (IFeatureLayer)_LayerTMA_Arspc;
                newlayer2.Visible = true;

                BuildProceduresTree();

                axPageLayoutControl1.ZoomToWholePage();

                ((IActiveView)axPageLayoutControl1.ActiveView.FocusMap).Extent = ((ILayer)_Layer).AreaOfInterest;
                ((IActiveView)axPageLayoutControl1.ActiveView.FocusMap).Refresh();

                comboBox1_SelectedIndexChanged(sender, e);

            }
            catch { }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Level = comboBox1.SelectedIndex;

            if (_Layer == null) return;
            IFeatureLayerDefinition FD = (ESRI.ArcGIS.Carto.IFeatureLayerDefinition)_Layer;
            
            switch (Level)
            {
                case(0):
                    FD.DefinitionExpression = "[codeLvl] ='LOWER'";
                    cnlList = DataCash.GetEnroutesChanels(CODE_ROUTE_SEGMENT_CODE_LVL.LOWER);
                    break;
                case (1):
                    FD.DefinitionExpression = "[codeLvl] ='BOTH'";
                    cnlList = DataCash.GetEnroutesChanels(CODE_ROUTE_SEGMENT_CODE_LVL.BOTH);
                    break;
                case (2):
                    FD.DefinitionExpression = "[codeLvl] ='UPPER'";
                    cnlList = DataCash.GetEnroutesChanels(CODE_ROUTE_SEGMENT_CODE_LVL.UPPER);
                    break;
                case (3):
                    FD.DefinitionExpression = "[codeLvl] ='OTHER'";
                    cnlList = DataCash.GetEnroutesChanels(CODE_ROUTE_SEGMENT_CODE_LVL.OTHER);

                    break;
                default:
                    FD.DefinitionExpression = "[codeLvl] ='BOTH'";
                    break;
            }

            List<RadioCommunicationChanel> arspsChanels = DataCash.GetAirspaceChanels();
            if (cnlList == null) cnlList = new List<RadioCommunicationChanel>();
            if (arspsChanels != null) cnlList.AddRange(arspsChanels);

            ((IActiveView)axPageLayoutControl1.ActiveView.FocusMap).Refresh();

            linkLabel1.Visible = cnlList != null;

        }

       
        private void button4_Click(object sender, EventArgs e)
        {

            SidSettingsForm frm = new SidSettingsForm();

            frm.comboBoxVert.SelectedIndex = 0;
            frm.comboBoxDist.SelectedIndex = 0;
            if (this.vertUom != UOM_DIST_VERT.OTHER) frm.comboBoxVert.Text = this.vertUom.ToString();
            if (this.distUom != UOM_DIST_HORZ.OTHER) frm.comboBoxDist.Text = this.distUom.ToString();
            _AllVOR_DMEflag = true;
            frm.checkBox1.Checked = _AllVOR_DMEflag;
            frm.ShowDialog();
            _AllVOR_DMEflag = frm.checkBox1.Checked;

            this.vertUom = frm.comboBoxVert.SelectedIndex == 0 ? UOM_DIST_VERT.FT : UOM_DIST_VERT.M;
            this.distUom = frm.comboBoxDist.SelectedIndex == 0 ? UOM_DIST_HORZ.NM : UOM_DIST_HORZ.KM;
            this.arspBufWidth = (int)frm.numericUpDown1.Value;
            this.arspSign = frm.checkBox2.Checked;

            this.VS_Radius = (int)frm.numericUpDown2.Value;
            this.VS_Min_Elev = (int)frm.numericUpDown3.Value;

        }

        private void AreaChartForm_Load(object sender, EventArgs e)
        {
            axToolbarControl1.SetBuddyControl(axPageLayoutControl1);
            axToolbarControl2.SetBuddyControl(axPageLayoutControl1);

        }

        private void button5_Click(object sender, EventArgs e)
        {
            CalendarForm frm = new CalendarForm();
            if (frm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                airacControl.AiracCircleValue = AiracUtil.AiracUtil.GetAiracCycleByDate(frm.selectedDate);
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            RadioCommunicationsChanelsListForm frm = new RadioCommunicationsChanelsListForm(cnlList, _selectedChanels);
            frm.ShowDialog();
            _selectedChanels.Clear();
            if (frm.SelectedChanelsList != null && frm.SelectedChanelsList.Count > 0) _selectedChanels.AddRange(frm.SelectedChanelsList);
        }

        private List<PDMObject> BuildTMAList(AirportHeliport airportHeliport)
        {
            List<PDMObject> res = new List<PDMObject>();

            if (airportHeliport.CommunicationChanels == null || airportHeliport.CommunicationChanels.Count <= 0) return res;

            var ArspcList_TMA = DataCash.GetAirspaceList(AirspaceType.TMA);
            var ArspcList_TMA_P = DataCash.GetAirspaceList(AirspaceType.TMA_P);

            List<PDMObject> ArspcList = new List<PDMObject>();
            if (ArspcList_TMA != null) ArspcList.AddRange(ArspcList_TMA);
            if (ArspcList_TMA_P != null) ArspcList.AddRange(ArspcList_TMA_P);

            foreach (var item in ArspcList)
            {
                Airspace arsp = ((Airspace)item);
                if (arsp.CommunicationChanels == null) continue;

                if (arsp.CommunicationChanels.Intersect(airportHeliport.CommunicationChanels) != null) res.Add(arsp);

            }
       
            return res;
        }

        private List<PDMObject> BuildAirportList(List<PDM.Airspace> tma_list)
        {
            List<PDMObject> res = new List<PDMObject>();

            if (AdhpList == null || AdhpList.Count < 0) return null;
            

            foreach (var item in tma_list)
            {
                Airspace arsp = ((Airspace)item);
                if (arsp.CommunicationChanels == null) continue;

                foreach (AirportHeliport airportHeliport in AdhpList)
                {
                    if (arsp.CommunicationChanels.Intersect(airportHeliport.CommunicationChanels) != null)
                    {
                        res.Add(airportHeliport);
                    }
                }
               

            }

            return res;
        }


        private int BuildProceduresTree()
        {
            listView1.BeginUpdate();


            listView1.Columns[0].Width = 200;
            listView1.Columns[1].Width = 100;
            listView1.Columns[2].Width = 128;

           
            List<Procedure> procList = new List<Procedure>();

            if (AdhpList != null && AdhpList.Count > 0)
            {
                for (int i = 0; i <= (checkedListBox1.Items.Count - 1); i++)
                {
                    List<Procedure> procListSid = new List<Procedure>();
                    List<Procedure> procListStar = new List<Procedure>();

                    if (checkedListBox1.GetItemChecked(i))
                    {
                        if (checkBox_SID.Checked)
                            procListSid = DataCash.GetAirportProcedures(PROC_TYPE_code.SID, AdhpList[i].ID, checkBox1.Checked);
                        if (checkBox_STAR.Checked)
                            procListStar = DataCash.GetAirportProcedures(PROC_TYPE_code.STAR, AdhpList[i].ID, checkBox1.Checked);

                        if (procListSid != null && procListSid.Count > 0)
                            procList.AddRange(procListSid);
                        if (procListStar != null && procListStar.Count > 0)
                            procList.AddRange(procListStar);
                    }

                }
            }

            int grIndx = 0;

            foreach (Procedure itemProc in procList)

            {
                grIndx++;

                ListViewItem item1 = new ListViewItem(itemProc.ProcedureType.ToString() + " " + itemProc.Airport_ICAO_Code + " " + itemProc.GetObjectLabel());
                if (itemProc.LandingArea != null) item1.SubItems.Add(itemProc.LandingArea.ToString());
                else item1.SubItems.Add(" ");
                if (itemProc.AircraftCharacteristic != null && itemProc.AircraftCharacteristic.Count > 0) item1.SubItems.Add(itemProc.AircraftCharacteristic[0].AircraftLandingCategory.ToString());

                item1.Tag = itemProc;
                listView1.Items.Add(item1);
            }

            listView1.EndUpdate();


            listView1_ColumnClick(listView1, new ColumnClickEventArgs(1));

            if (listView1.Items.Count <= 0 && _LayerProc != null)
            {
                IFeatureLayerDefinition FD = (ESRI.ArcGIS.Carto.IFeatureLayerDefinition)_LayerProc;

                if (FD != null)
                {
                    FD.DefinitionExpression = "ID_Transition IN ('0')";
                    ((IActiveView)axPageLayoutControl1.ActiveView.FocusMap).Refresh();
                }
            }

            return procList != null ? procList.Count : 0;
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

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (AdhpList != null && AdhpList.Count > 0)
            {
                listView1.Items.Clear();
                int pc = BuildProceduresTree();

            }
        }

        private void listView1_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            if (_LayerProc == null) return;

            try
            {
                string filter = "";
                IFeatureLayerDefinition FD = (ESRI.ArcGIS.Carto.IFeatureLayerDefinition)_LayerProc;
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


            }
            catch (Exception ex)
            {
            }


            ((IActiveView)axPageLayoutControl1.ActiveView.FocusMap).Refresh();
        }


        private void checkedComboBoxTMA_DropDownClosed(object sender, EventArgs e)
        {
            toolTip1.SetToolTip(checkedComboBoxTMA, checkedComboBoxTMA.Text);


            if (_LayerTMA_Arspc == null) return;
            IFeatureLayerDefinition FD = (ESRI.ArcGIS.Carto.IFeatureLayerDefinition)_LayerTMA_Arspc;
            if (FD == null) return;
            checkedListBox1.Items.Clear();

            try
            {
                string filter = "";


                FD.DefinitionExpression = "AirspaceID IN ('0')";


                if (checkedComboBoxTMA.Items.Count != 0)
                {
                    tmaLst = new List<Airspace>();
                    foreach (CCBoxItem item in checkedComboBoxTMA.CheckedItems)
                    {
                        Airspace arspc = (Airspace)tma_list[item.Value];
                        filter = filter + "'" + arspc.ID + "',";
                        tmaLst.Add(arspc);
                    }

                    FD.DefinitionExpression = "AirspaceID IN (" + filter + "'0')";

                    var arpLst = BuildAirportList(tmaLst);
                    if (arpLst != null)
                    {
                        foreach (var item in arpLst)
                        {
                            checkedListBox1.Items.Add(item, false);
                        }
                    }

                }

                listView1.Items.Clear();
                BuildProceduresTree();


            }
            catch (Exception ex)
            {
            }

            (axPageLayoutControl1.ActiveView.FocusMap as IActiveView).Extent = ChartsHelperClass.GetLayerExtent(_LayerTMA_Arspc, FD.DefinitionExpression);
            ((IActiveView)axPageLayoutControl1.ActiveView.FocusMap).Refresh();

            button2.Enabled = checkedComboBoxTMA.Text.Length > 0;

        }


        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (AdhpList != null && AdhpList.Count > 0)///
            {
                listView1.Items.Clear();
                int pc = BuildProceduresTree();

            }
        }

        private void checkedListBox1_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (AdhpList != null && AdhpList.Count > 0)
            {
                listView1.Items.Clear();
                int pc = BuildProceduresTree();

            }
        }

        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (AdhpList != null && AdhpList.Count > 0)
            {
                listView1.Items.Clear();
                int pc = BuildProceduresTree();

            }
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void checkedComboBoxTMA_SelectedIndexChanged(object sender, EventArgs e)
        {

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
                var tmpFolder = ArenaStaticProc.GetPathToTemplate() + @"\AreaChart\";
                var tmpFileName = ArenaStaticProc.GetPathToTemplate() + @"\AreaChart\" + ofd.SafeFileName;

                if (File.Exists(tmpFileName))
                {
                    if (MessageBox.Show("There is already a file with the same name in this location! " + Environment.NewLine
                        + "Replace the file in the destination folder with the file you are copying?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No) return;
                }

                File.Copy(ofd.FileName, tmpFileName, true);

                TemplateListComboBox.Items.Add(System.IO.Path.GetFileName(ofd.FileName));
            }
        }
    }
}
