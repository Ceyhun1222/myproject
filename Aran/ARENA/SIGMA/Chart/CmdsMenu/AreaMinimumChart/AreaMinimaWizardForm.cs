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
    public partial class AreaMinimaWizardForm : Form
    {

        public string _FolderName;
        public string _ProjectName;
        public string _TemplatetName;
        public double mapSize_Height;
        public double mapSize_Width;
        //public bool _AllVOR_DMEflag;
        public int _AiracCircle;
        public double _mapScale;
        public IAOIBookmark _bookmark;
        

        private List<AirportHeliport> AdhpList;
        private List<NavaidSystem> navList;
        private IFeatureLayer2 _AirspcLayer;
        private IFeatureLayer2 _WpntLayer;

        public AirportHeliport selectedADHP = null;
        public NavaidSystem selectedVORDME = null;
        public List<string> _selectedChanels = null;
        public List<SegmentPoint> _selectedWYPNT = null;
        public List<AirspaceVolume> _selectedArspVol = null;

        public UOM_DIST_VERT vertUom = UOM_DIST_VERT.FT;
        public UOM_DIST_HORZ distUom = UOM_DIST_HORZ.NM;
        public int arspBufWidth = 2;
        public bool arspSign = false;
        public double VS_Radius = 5000;
        public int VS_Min_Elev = 0;
        public bool allRwyFlag = true;
        public int circleStep = 5;
        public int maxRadius = 30;
        
        public AreaMinimaWizardForm(int AiracCircle)
        {
            InitializeComponent();

            try
            {
                _FolderName = "";
                TemplateListComboBox.Items.Clear();
                var tmp = ArenaStaticProc.GetPathToTemplate() + @"\MinimumAltitudeChart\";

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




                _AiracCircle = AiracCircle;
                airacControl1.AiracCircleValue = AiracCircle;

                if (comboBox1.Items.Count > 0)
                {
                    comboBox1.SelectedIndex = 0;
                    //MessageBox.Show("comboBox1.SelectedIndex = 0;");
                }

                BuildSectorAirspace();
                if (AdhpList!=null) BuildDnpTree(AdhpList[0]);

                _selectedChanels = new List<string>();


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);


            }

        }

        private void BuildDnpTree(AirportHeliport adhp)
        {
            listView3.BeginUpdate();

            listView3.Items.Clear();

            #region DPN

            List<PDMObject> pdmLst = new List<PDMObject>();
            List<string> ids = new List<string>();

            #region Star RNAV


            var procLst = DataCash.GetAirportProcedures(PROC_TYPE_code.STAR, adhp.ID, true);
            foreach (Procedure prc in procLst)
            {
                ProcedureTransitions trans = prc.Transitions.Where(tr => tr.RouteType == ProcedurePhaseType.APPROACH && tr.Legs != null && tr.Legs.Count > 0).FirstOrDefault();
                if (trans != null && trans.Legs[0].StartPoint != null)
                {
                    if (ids.IndexOf(trans.Legs[0].StartPoint.PointChoiceID) < 0)
                    {
                        pdmLst.Add(trans.Legs[0].StartPoint);
                        ids.Add(trans.Legs[0].StartPoint.PointChoiceID);
                    }
                }
                if (trans != null && trans.Legs[trans.Legs.Count - 1].EndPoint != null)
                {
                    if (ids.IndexOf(trans.Legs[trans.Legs.Count - 1].EndPoint.PointChoiceID) < 0)
                    {
                        pdmLst.Add(trans.Legs[trans.Legs.Count - 1].EndPoint);
                        ids.Add(trans.Legs[trans.Legs.Count - 1].EndPoint.PointChoiceID);
                    }
                }
            }

            #endregion

            #region IAP RNAV // qatar Version


            procLst = DataCash.GetAirportProcedures(PROC_TYPE_code.Approach, adhp.ID, true);
            foreach (Procedure prc in procLst)
            {
                var transList = prc.Transitions.Where(tr => (tr.RouteType == ProcedurePhaseType.MISSED || tr.RouteType == ProcedurePhaseType.FINAL) && tr.Legs != null && tr.Legs.Count > 0).ToList();

                foreach (var trans in transList)
                {

                    if (trans != null && trans.Legs[0].StartPoint != null && trans.Legs[0].LegSpecialization == SegmentLegSpecialization.FinalLeg)
                    {
                        if (ids.IndexOf(trans.Legs[0].StartPoint.PointChoiceID) < 0)
                        {
                            pdmLst.Add(trans.Legs[0].StartPoint);
                            ids.Add(trans.Legs[0].StartPoint.PointChoiceID);
                        }
                    }

                    if (trans != null && trans.Legs[trans.Legs.Count - 1].EndPoint != null && trans.Legs[trans.Legs.Count - 1].LegSpecialization == SegmentLegSpecialization.MissedApproachLeg)
                    {
                        if (ids.IndexOf(trans.Legs[trans.Legs.Count - 1].EndPoint.PointChoiceID) < 0)
                        {
                            pdmLst.Add(trans.Legs[trans.Legs.Count - 1].EndPoint);
                            ids.Add(trans.Legs[trans.Legs.Count - 1].EndPoint.PointChoiceID);
                        }
                    }
                }
            }

            #endregion


            if (pdmLst.Count > 0)
            {
                ILayer _LayerADHP = EsriUtils.getLayerByName(axPageLayoutControl1.ActiveView.FocusMap, "AirportHeliport");
                var fc = ((IFeatureLayer)_AirspcLayer).FeatureClass;


                var selectedRec = EsriUtils.ToGeo((axPageLayoutControl1.ActiveView.FocusMap as IActiveView).Extent, axPageLayoutControl1.ActiveView.FocusMap, (fc as IGeoDataset).SpatialReference);
                var dpnList = DataCash.GetObjectsWithinPolygon(selectedRec, PDM_ENUM.SegmentPoint, pdmLst);

                if (dpnList != null && dpnList.Count > 0)
                {
                    listView3.Items.Clear();
                    listView3.Columns[0].Width = 130;
                    listView3.Columns[1].Width = 146;


                    foreach (SegmentPoint dpn in dpnList)
                    {
                        ListViewItem item1 = new ListViewItem(dpn.SegmentPointDesignator);
                        item1.SubItems.Add(dpn.PointUse.ToString());
                        item1.Tag = dpn;
                        listView3.Items.Add(item1);
                    }


                }
            }

            #endregion

            listView3.EndUpdate();

        }

        private int BuildNavaidsTree(AirportHeliport ADHP)
        {
            try
            {
                navList = new List<NavaidSystem>();
                NavaidSystem arp_NAV = (NavaidSystem)DataCash.GetAirportNavaidByAirportID(ADHP.ID, NavaidSystemType.VOR_DME);
                if (arp_NAV != null)
                {
                    comboBox2.Items.Add(arp_NAV.GetObjectLabel());
                    navList.Add(arp_NAV);
                }

                arp_NAV = (NavaidSystem)DataCash.GetAirportNavaidByAirportID(ADHP.ID, NavaidSystemType.VORTAC);
                if (arp_NAV != null)
                {
                    comboBox2.Items.Add(arp_NAV.GetObjectLabel());
                    navList.Add(arp_NAV);

                }

                return 0;

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
                return 0;
            }


        }

        private int BuildSectorAirspace()
        {
            try
            {
                listView2.BeginUpdate();

                listView2.Items.Clear();
                listView2.Columns[0].Width = 140;
                listView2.Columns[1].Width = 100;
                listView2.Columns[2].Width = 240;

                var arspc_Lst = DataCash.GetAirspaceList(AirspaceType.SECTOR);
                if (arspc_Lst != null)
                {

                    foreach (Airspace arsps in arspc_Lst)
                    {
                        foreach (var vol in arsps.AirspaceVolumeList)
                        {
                            ListViewItem item1 = new ListViewItem(vol.GetObjectLabel());
                            item1.SubItems.Add(vol.CodeType.ToString());
                            if (vol.TxtName != null && vol.TxtName.Length > 0) item1.SubItems.Add(vol.TxtName);
                            item1.Tag = vol;
                            listView2.Items.Add(item1);
                        }
                    }
                  
                }

                arspc_Lst = DataCash.GetAirspaceList(AirspaceType.SECTOR_C);
                if (arspc_Lst != null)
                {
                    foreach (Airspace arsps in arspc_Lst)
                    {
                        foreach (var vol in arsps.AirspaceVolumeList)
                        {
                            ListViewItem item1 = new ListViewItem(vol.GetObjectLabel());
                            item1.SubItems.Add(vol.CodeType.ToString());
                            if (vol.TxtName != null && vol.TxtName.Length > 0) item1.SubItems.Add(vol.TxtName);
                            item1.Tag = vol;
                            listView2.Items.Add(item1);
                        }
                    }

                }

                listView2.EndUpdate();


                return 0;

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
                return 0;
            }


        }

        private void button2_Click(object sender, EventArgs e)
        {
            _ProjectName = textBox1.Text;
            _FolderName = textBox2.Text;
            _TemplatetName = TemplateListComboBox.Text;
            _mapScale = axPageLayoutControl1.ActiveView.FocusMap.MapScale;
            //_extent = axPageLayoutControl1.ActiveView.Extent;


            if (!TerminalChartsUtil.CheckFileExisting(_ProjectName, _FolderName)) return;

           
            ArenaStaticProc.SetPathToMapFolder(_FolderName);
            this._AiracCircle = airacControl1.AiracCircleValue;

           
           _selectedArspVol = new List<AirspaceVolume>();
            foreach (ListViewItem item in listView2.Items)
            {
                if (item == null) continue; // мистика

                if (item.Checked)
                {
                    _selectedArspVol.Add((AirspaceVolume)item.Tag);
                }
            }


            _selectedWYPNT = new List<SegmentPoint>();
            foreach (ListViewItem item in listView3.Items)
            {
                if (item == null) continue; // мистика

                if (item.Checked)
                {
                    _selectedWYPNT.Add((SegmentPoint)item.Tag);
                }
            }


            #region Create a new bookmark

            _bookmark = ChartsHelperClass.CreateBookmark(axPageLayoutControl1.ActiveView.FocusMap, "AREAMINIMA_Bookmark");

            #endregion

            ChartsHelperClass.SetMapGridVisibilityState(axPageLayoutControl1.ActiveView, true);

            //button2.DialogResult = System.Windows.Forms.DialogResult.OK;
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

     
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                button2.Enabled = false;

                if (AdhpList != null && AdhpList.Count > 0)
                {
                    comboBox2.Items.Clear();
                    BuildNavaidsTree(AdhpList[comboBox1.SelectedIndex]);

                    selectedADHP = AdhpList[comboBox1.SelectedIndex];
                    linkLabel1.Text = selectedADHP.CommunicationChanels != null && selectedADHP.CommunicationChanels.Count > 0 ? "Communication chanels ....." : "";
                    VS_Min_Elev = selectedADHP.Elev.HasValue ? (int)selectedADHP.ConvertValueToMeter(selectedADHP.Elev, selectedADHP.Elev_UOM.ToString()) : 0;


                    BuildDnpTree(selectedADHP);

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
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

       
        private ListViewItemComparer GetListViewSorter(int columnIndex)
        {
            ListViewItemComparer sorter = listView2.ListViewItemSorter as ListViewItemComparer;
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
            ChartsHelperClass.SelectChartTemplate(ref axPageLayoutControl1, ArenaStaticProc.GetPathToTemplate() + @"\MinimumAltitudeChart\" + TemplateListComboBox.Text, ref listBox1, out mapSize_Height, out mapSize_Width);

            try
            {
                string ConString =  ArenaStaticProc.GetTargetDB();

                IWorkspaceFactory2 workspaceFactory = (IWorkspaceFactory2)new AccessWorkspaceFactoryClass();
                IWorkspace Wksp = workspaceFactory.OpenFromFile(ConString, 0);
                IFeatureWorkspace fWksp = (IFeatureWorkspace)Wksp;
                ITable table = fWksp.OpenTable("AirspaceVolume");

                IFeatureClass pFeatureClass = (IFeatureClass)table;

                _AirspcLayer = (IFeatureLayer2)EsriUtils.getLayerByName(axPageLayoutControl1.ActiveView.FocusMap, "AirspaceVolume"); //
                _WpntLayer = (IFeatureLayer2)EsriUtils.getLayerByName(axPageLayoutControl1.ActiveView.FocusMap, "WayPoint"); //

                ChartsHelperClass.SetMapGridVisibilityState(axPageLayoutControl1.ActiveView, false);


                if (_AirspcLayer != null)
                {
                    _AirspcLayer.FeatureClass = pFeatureClass;

                    ((IActiveView)axPageLayoutControl1.ActiveView.FocusMap).Extent = ((ILayer)_AirspcLayer).AreaOfInterest;
                    ((IActiveView)axPageLayoutControl1.ActiveView.FocusMap).Refresh();

                    IFeatureLayer newlayer = (IFeatureLayer)_AirspcLayer;
                    newlayer.Visible = true;

                    axPageLayoutControl1.ZoomToWholePage();

                }


                if (_WpntLayer != null)
                {
                    table = fWksp.OpenTable("WayPoint");

                    pFeatureClass = (IFeatureClass)table;

                    _WpntLayer.FeatureClass = pFeatureClass;

                    IFeatureLayer newlayer = (IFeatureLayer)_WpntLayer;
                    newlayer.Visible = true;


                }

                listView2_ItemChecked(sender, null);
                listView3_ItemChecked(sender, null);
            }
            catch(Exception ex) {
                //MessageBox.Show(ex.Source);
            }

        }

        private void button4_Click(object sender, EventArgs e)
        {
            AreaMinimaSettingsForm frm = new AreaMinimaSettingsForm();
            //frm.checkBox1.Checked = _AllVOR_DMEflag;

            frm.comboBoxVert.SelectedIndex = 0;
            frm.comboBoxDist.SelectedIndex = 0;
            if (this.vertUom != UOM_DIST_VERT.OTHER) frm.comboBoxVert.Text = this.vertUom.ToString();
            if (this.distUom != UOM_DIST_HORZ.OTHER) frm.comboBoxDist.Text = this.distUom.ToString();

            frm.ShowDialog();
            //_AllVOR_DMEflag = frm.checkBox1.Checked;

            this.vertUom = frm.comboBoxVert.SelectedIndex == 0 ? UOM_DIST_VERT.FT : UOM_DIST_VERT.M;
            this.distUom = frm.comboBoxDist.SelectedIndex == 0 ? UOM_DIST_HORZ.NM : UOM_DIST_HORZ.KM;
            this.arspBufWidth = (int)frm.numericUpDown1.Value;
            this.arspSign = frm.checkBox2.Checked;

            this.VS_Radius = this.vertUom == UOM_DIST_VERT.KM?  (double)frm.numericUpDown2.Value *1000 : (double)frm.numericUpDown2.Value * 1852;
            this.VS_Min_Elev = (int)frm.numericUpDown3.Value;
            this.circleStep = Convert.ToInt32(frm.comboBox1.Text);
            this.maxRadius = (int)frm.numericUpDown4.Value;

        }

        private void listView1_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            
        }

        private void SIDWizardForm_Load(object sender, EventArgs e)
        {
            axToolbarControl1.SetBuddyControl(axPageLayoutControl1);
            axToolbarControl2.SetBuddyControl(axPageLayoutControl1);

        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void axPageLayoutControl1_OnExtentUpdated(object sender, ESRI.ArcGIS.Controls.IPageLayoutControlEvents_OnExtentUpdatedEvent e)
        {
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
            
        }


        private void listView2_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            if (_AirspcLayer == null) return;
            if (AdhpList == null) return;

            try
            {
                #region Airspace

                string filter = "";

                IFeatureLayerDefinition FD = (ESRI.ArcGIS.Carto.IFeatureLayerDefinition)_AirspcLayer;
                if (FD == null) return;

                FD.DefinitionExpression = "FeatureGUID IN ('0') and [codeType] ='SECTOR'";

                foreach (ListViewItem item in listView2.Items)
                {

                    if (item == null) continue; // мистика

                    if (item.Checked)
                    {

                        AirspaceVolume vol = (AirspaceVolume)item.Tag;
                        filter = filter + "'" + vol.ID + "',";


                    }
                }


                if (FD == null) return;
                if (filter.Length > 0) FD.DefinitionExpression = "FeatureGUID IN (" + filter + "'0') and [codeType] ='SECTOR'";

                #endregion

                (axPageLayoutControl1.ActiveView.FocusMap as IActiveView).Extent = ChartsHelperClass.GetLayerExtent(_AirspcLayer, FD.DefinitionExpression);


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.StackTrace);
            }

            button2.Enabled = false;
            foreach (ListViewItem item in listView2.Items)
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

        private void listView2_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            try
            {

                ListViewItemComparer sorter = GetListViewSorter(e.Column);

                listView2.ListViewItemSorter = sorter;
                listView2.Sort();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }
        }

        private void listView2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void listView3_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            if (_WpntLayer == null) return;
            //if (AdhpList == null) return;

            try
            {
                #region WayPoints

                string filter = "";

                IFeatureLayerDefinition FD = (ESRI.ArcGIS.Carto.IFeatureLayerDefinition)_WpntLayer;
                if (FD == null) return;

                FD.DefinitionExpression = "FeatureGUID IN ('0')";

                foreach (ListViewItem item in listView3.Items)
                {

                    if (item == null) continue; // мистика

                    if (item.Checked)
                    {

                        SegmentPoint vol = (SegmentPoint)item.Tag;
                        filter = filter + "'" + vol.ID + "',";


                    }
                }


                if (FD == null) return;
                if (filter.Length > 0) FD.DefinitionExpression = "FeatureGUID IN (" + filter + "'0')";

                #endregion

                //(axPageLayoutControl1.ActiveView.FocusMap as IActiveView).Extent = 
                ChartsHelperClass.GetLayerExtent(_WpntLayer, FD.DefinitionExpression);


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.StackTrace);
            }

           
            ((IActiveView)axPageLayoutControl1.ActiveView.FocusMap).Refresh();
        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (navList!=null && navList.Count >0)
            {
                selectedVORDME = navList[comboBox2.SelectedIndex];
            }
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
                var tmpFolder = ArenaStaticProc.GetPathToTemplate() + @"\MinimumAltitudeChart\";
                var tmpFileName = ArenaStaticProc.GetPathToTemplate() + @"\MinimumAltitudeChart\" + ofd.SafeFileName;

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
