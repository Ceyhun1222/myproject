using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using ARINC_DECODER_CORE;
using ARINC_Types;
using Microsoft.Win32;
using AreaManager;
//using cExcellUtils;
using ARINC_DECODER_CORE.AIRTRACK_Objects;
using ARINC_DECODER_CORE.ARTRACK_Static;
using ESRI.ArcGIS.Geometry;
using System.Reflection;
using ESRI.ArcGIS.Carto;
using System.Drawing;


namespace ARINC_DECODER_UI
{
    public partial class Form1 : Form
    {
        List<ARINC_OBJECT> ListOfObjects = null;
        List<Object_AIRTRACK> ListOf_Object_AIRTRACK = null;
        private string SelAreaCode;
        private string SelObjType;
        public string ARINC_SPECIFICATION_FILE;
        //public string ProjectFileLocation;

        public Form1()
        {
            InitializeComponent();

            label_Step1.Text = tabControl1.TabPages[0].Text;
            label_Step2.Text = tabControl1.TabPages[1].Text;
            label_Step3.Text = tabControl1.TabPages[2].Text;
            label_Step4.Text = tabControl1.TabPages[3].Text;
            treeView1.Nodes.Clear();
            treeView3.Nodes[0].Expand();
            treeView3.Nodes[1].Expand();
            treeView3.Nodes[2].Expand();
            treeView3.Nodes[3].Expand();
           
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Tag = ListOf_Object_AIRTRACK;
            //if (!checkBox7.Checked)
            //{
            //    if (SaveARINCResults()) Close();
            //}
            //else if (SaveJepesenObstaclesResults()) Close();
            //else MessageBox.Show("Select area!");
            
        }

        private string GetPathToAreaFile()
        {

            const string userRoot = "HKEY_CURRENT_USER";
            const string subkey = "Software\\RISK\\ARENA\\ARINC";
            const string keyName = userRoot + "\\" + subkey;
            string Res = (string)Registry.GetValue(keyName, "AreaFile", "Area not exist");

            return Res;

        }

        //private bool SaveJepesenObstaclesResults()
        //{
        //    bool res = false;

        //    try
        //    {
        //        string startupPath = Application.StartupPath;
        //        using (FolderBrowserDialog folderDialog = new FolderBrowserDialog())
        //        {
        //            folderDialog.Description = "Select folder";
        //            folderDialog.ShowNewFolderButton = true;
        //            folderDialog.RootFolder = Environment.SpecialFolder.MyComputer;

        //            if (folderDialog.ShowDialog() == DialogResult.OK)
        //            {
        //                string folder = folderDialog.SelectedPath;

        //                foreach (TreeNode Nd in treeView1.Nodes)
        //                {
        //                    if ((Nd.Checked) && (Nd.Tag != null))
        //                    {
        //                        dataGridView1.Visible = false;

        //                        List<string> lst = (List<string>)Nd.Tag;
        //                        FilldataView(lst.ToArray(), dataGridView1);

        //                        DataTable tbl = dataGridView1.DataSource as DataTable;
        //                        tbl.TableName = "Table";
        //                        cExcel.DataTableToExcelFile(tbl, folder + @"\"+Nd.Text + ".xls", false);

        //                        res = true;

        //                        dataGridView1.Visible = true;
        //                    }
        //                }

        //            }
        //        }

        //    }
        //    catch (Exception exc)
        //    {
        //        MessageBox.Show("Import failed because " + exc.Message + " , please try again later.");
        //    }

        //    return res;

        //}

        private void DoWizardsWork(int p)
        {
            AlertForm alrtForm = new AlertForm();
            alrtForm.FormBorderStyle = FormBorderStyle.None;
            alrtForm.Opacity = 0.5;
            alrtForm.BackgroundImage = ARINC_DECODER_UI.Properties.Resources.ArenaSplash;
            alrtForm.TopMost = true;

            switch (p)
            {
                case (0): DefineArea();
                    break;
                case (1):
                    SaveAreaXmlfile();

                    if (!System.Diagnostics.Debugger.IsAttached) alrtForm.Show();

                    DecodeARINCFile(textBox1.Text);
                    //Static_Proc.CloseProgressBar();
                    if (treeView1.Nodes.Count > 0)
                    {
                        treeView1.Nodes[0].Checked = true;
                    }

                    alrtForm.Close();

                    break;
                case (2):
                    if (!treeView3.Nodes[3].Checked)
                    {
                        //////////////
                        if (checkBox10.Checked) SaveARINCResults();
                        //if (!SaveARINCResults()) MessageBox.Show("Select area!");
                        //break;
                        /////////////


                        AreaInfo Area = new AreaInfo();
                        Area = AreaManager.AreaUtils.GetArea(GetPathToAreaFile());
                        double CM = (Area.AreaPolygon as IArea).Centroid.X;

                       
                        CreateAIRTRACKObjects(Area);

                        #region создание ключевых связей


                        Dictionary<string, Object_AIRTRACK> ARPDictionary = new Dictionary<string, Object_AIRTRACK>();
                        List<string> RouteNameList = new List<string>();
                        List<Object_AIRTRACK> RDN_LIST = (from element in ListOf_Object_AIRTRACK where (element != null) && (element is RunWay_THR_AIRTRACK) select element).ToList();
                        List<Object_AIRTRACK> ILS_LIST = (from element in ListOf_Object_AIRTRACK where (element != null) && (element is ILS_AIRTRACK) select element).ToList();
                        List<Object_AIRTRACK> ARP_LIST = (from element in ListOf_Object_AIRTRACK where (element != null) && (element is AIRPORT_AIRTRACK) select element).ToList();
                        List<Object_AIRTRACK> MKR_LIST = (from element in ListOf_Object_AIRTRACK where (element != null) && (element is Marker_AIRTRACK) select element).ToList();
                        List<ProcedureBranch_AIRTRACK> ProcBranches_LIST = null;
                        List<Procedure_AIRTRACK> ProcList = null;
                        List<Object_AIRTRACK> SegmentPointList = (from element in ListOf_Object_AIRTRACK where (element != null) && (element is SEGMENT_POINT_AIRTRACK) select element).ToList();

                        // ARPDictionary
                        foreach (AIRPORT_AIRTRACK ARP in ARP_LIST)
                        {
                            ARINC_Airport_Primary_Record ARINC_Airport = (ARINC_Airport_Primary_Record)ARP.ARINC_OBJ;
                            if (ARPDictionary.ContainsKey(ARINC_Airport.Airport_ICAO_Identifier)) continue;
                            ARPDictionary.Add(ARINC_Airport.Airport_ICAO_Identifier, (AIRPORT_AIRTRACK)ARP);
                        }

                        // LegList
                        List<Object_AIRTRACK> LEG_LIST = (from element in ListOf_Object_AIRTRACK
                                                          where (element != null) && (element is Leg_AIRTRACK) && (ARP_LIST != null) &&
                                                              (ARPDictionary.ContainsKey((element as Leg_AIRTRACK).ARINC_ProcedureLeg.Airport_Identifier))
                                                          select element).ToList();

                        if (LEG_LIST != null)
                        {
                            ProcBranches_LIST = GetProceduresBranchesList(LEG_LIST); // все процедуры "ветви"

                            ProcList = GetprocedureLis(ProcBranches_LIST);
                        }

                        //RDN_ILS
                        foreach (ILS_AIRTRACK ILS in ILS_LIST)
                        {
                            ARINC_LocalizerGlideSlope_Primary_Record ARINC_ILS = (ARINC_LocalizerGlideSlope_Primary_Record)ILS.ARINC_OBJ;

                            DME_AIRTRACK dme = (from element in ListOf_Object_AIRTRACK
                                                where (element != null) && (element is DME_AIRTRACK)
                                                    && (((((DME_AIRTRACK)element)).ARINC_OBJ as ARINC_Navaid_VHF_Primary_Record).Navaid_Identifier.Trim() == (ILS.ARINC_OBJ as ARINC_LocalizerGlideSlope_Primary_Record).Localizer_Identifier.Trim())
                                                select element).FirstOrDefault() as DME_AIRTRACK;

                            if (dme != null)
                            {
                                ILS.RelatedDME = dme;
                                dme.VHF_code = VHF_NAVAID_AIRTRACK.VHF_NAVAID_code.DME_ILS;
                            }



                            TACAN_AIRTRACK tacan = (from element in ListOf_Object_AIRTRACK
                                                    where (element != null) && (element is TACAN_AIRTRACK)
                                                    && (((((TACAN_AIRTRACK)element)).ARINC_OBJ as ARINC_Navaid_VHF_Primary_Record).Navaid_Identifier.Trim() == (ILS.ARINC_OBJ as ARINC_LocalizerGlideSlope_Primary_Record).Localizer_Identifier.Trim())
                                                    select element).FirstOrDefault() as TACAN_AIRTRACK;

                            if (tacan != null)
                            {
                                ILS.RelatedDME = tacan;
                                tacan.VHF_code = VHF_NAVAID_AIRTRACK.VHF_NAVAID_code.TACAN_ILS;
                            }




                            RunWay_THR_AIRTRACK rdn = (from element in RDN_LIST
                                                       where (element != null) && ((((RunWay_THR_AIRTRACK)element).ARINC_OBJ as ARINC_Runway_Primary_Records).Airport_ICAO_Identifier == ARINC_ILS.Airport_Identifier)
                                                        && (((RunWay_THR_AIRTRACK)element).RDN_TXT_DESIG == ARINC_ILS.Runway_Identifier)
                                                       select element).FirstOrDefault() as RunWay_THR_AIRTRACK;

                            if (rdn != null)
                            {
                                rdn.RelatedIls = ILS;
                            }

                            List<Object_AIRTRACK> mkr = (from element in MKR_LIST
                                                         where (element != null)
                                                          //&& (((((Marker_AIRTRACK)element)).ARINC_OBJ as ARINC_Airport_Marker).Localizer_Identifier.Trim() == (ILS.ARINC_OBJ as ARINC_LocalizerGlideSlope_Primary_Record).Localizer_Identifier.Trim())
                                                          && (((((Marker_AIRTRACK)element)).ARINC_OBJ as ARINC_Airport_Marker).Runway_Identifier.Trim() == (rdn.ARINC_OBJ as ARINC_Runway_Primary_Records).Runway_Identifier.Trim())
                                                          && (((((Marker_AIRTRACK)element)).ARINC_OBJ as ARINC_Airport_Marker).Airport_Identifier.Trim() == (rdn.ARINC_OBJ as ARINC_Runway_Primary_Records).Airport_ICAO_Identifier.Trim())
                                                         select element).ToList();

                            if (mkr != null)
                            {
                                rdn.RelatedMarker = mkr;
                            }
                        }



                        #region привязка RDN RWY ADHP

                        if ((ARPDictionary.Count > 0) && (RDN_LIST.Count > 0))
                        {
                            foreach (KeyValuePair<string, Object_AIRTRACK> ARP in ARPDictionary)
                            {

                                List<Object_AIRTRACK> RNDList = (from element in RDN_LIST
                                                                 where (element != null) &&
                                                                 (element is RunWay_THR_AIRTRACK) &&
                                                                 ((((RunWay_THR_AIRTRACK)element).ARINC_OBJ as ARINC_Runway_Primary_Records).Airport_ICAO_Identifier == ARP.Key)
                                                                 select element).ToList();
                                if (RNDList.Count > 0)
                                {
                                    foreach (RunWay_THR_AIRTRACK RDN in RNDList)
                                    {
                                        RDN.True_Bearing = RDN.Magnetic_Bearing + ((AIRPORT_AIRTRACK)ARP.Value).Magnetic_Variation;
                                    }

                                    ((AIRPORT_AIRTRACK)ARP.Value).LinkedRWY = GetRWYs(RNDList);
                                }

                                if (ProcList != null)
                                {
                                    List<Procedure_AIRTRACK> ProcList_ARP = (from element in ProcList
                                                                             where (element != null) && (element is Procedure_AIRTRACK)
                                                                                 && (((Procedure_AIRTRACK)element).AirportIdentifier == ARP.Key)
                                                                             select element).ToList();

                                    if (ProcList_ARP.Count > 0) ((AIRPORT_AIRTRACK)ARP.Value).LinkedProceduresList = ProcList_ARP;

                                }

                            }
                        }

                        #endregion

                        #region формирование Route

                        // RouteNameList
                        foreach (SEGMENT_POINT_AIRTRACK segPnt in SegmentPointList)
                        {
                            if ((segPnt.ARINC_OBJ == null) || (((ARINC_Enroute_Airways_Primary_Record)segPnt.ARINC_OBJ).Fix_Identifier.Trim().Length <= 0)) continue;

                            string fix = ((ARINC_Enroute_Airways_Primary_Record)segPnt.ARINC_OBJ).Fix_Identifier;
                            Object_AIRTRACK objFix = null;
                            switch (segPnt.PointType)
                            {
                                case ("D "):
                                    objFix = (from element in ListOf_Object_AIRTRACK
                                              where (element is VHF_NAVAID_AIRTRACK) &&
                                                  ((((VHF_NAVAID_AIRTRACK)element).ARINC_OBJ as ARINC_Navaid_VHF_Primary_Record).Navaid_Identifier.Trim().CompareTo(fix.Trim()) == 0)
                                              select element).FirstOrDefault();
                                    break;
                                case ("DB"):
                                case ("PN"):
                                    objFix = (from element in ListOf_Object_AIRTRACK
                                              where (element is NDB_AIRTRACK) &&
                                                  ((((NDB_AIRTRACK)element).ARINC_OBJ as ARINC_Navaid_NDB_Primary_Record).Navaid_Identifier.Trim().CompareTo(fix.Trim()) == 0)
                                              select element).FirstOrDefault();
                                    break;
                                case ("EA"):
                                case ("PC"):
                                    objFix = (from element in ListOf_Object_AIRTRACK
                                              where (element is WayPoint_AIRTRACK) &&
                                                  ((((WayPoint_AIRTRACK)element).ARINC_OBJ as ARINC_WayPoint_Primary_Record).Waypoint_Identifier.Trim().CompareTo(fix.Trim()) == 0)
                                              select element).FirstOrDefault();
                                    break;
                            }

                            if (objFix != null)
                            {
                                segPnt.SegmentStartEndPoint = objFix;
                                segPnt.Shape = objFix.Shape;

                                if (RouteNameList.IndexOf(segPnt.Route_Identifier) >= 0) continue;
                                RouteNameList.Add(segPnt.Route_Identifier);
                            }

                        }

                        // Routes
                        foreach (string rteId in RouteNameList)
                        {
                            List<Object_AIRTRACK> Route = (from element in ListOf_Object_AIRTRACK
                                                           where (element != null) && (element is SEGMENT_POINT_AIRTRACK) &&
                                                               ((element as SEGMENT_POINT_AIRTRACK).Route_Identifier.CompareTo(rteId) == 0) &&
                                                               ((element as SEGMENT_POINT_AIRTRACK).SegmentStartEndPoint != null)
                                                           select element).ToList();

                            if (Route.Count > 1)
                            {
                                ROUTE_AIRTRACK Enrt = new ROUTE_AIRTRACK(Route);

                                ListOf_Object_AIRTRACK.Add(Enrt);
                            }
                        }

                        #endregion

                        #region формирование Airspace
                        List<Object_AIRTRACK> ARSPS_LIST = (from element in ListOf_Object_AIRTRACK where (element != null) && (element is AIRTRACK_Airspace_Segment) select element).ToList();
                        if ((ARSPS_LIST != null) && (ARSPS_LIST.Count > 0))
                        {
                            string ID_Arsps = ARSPS_LIST[0].INFO_AIRTRACK;
                            List<AIRTRACK_Airspace_Segment> segments = new List<AIRTRACK_Airspace_Segment>();

                            List<Object_AIRTRACK> arspVolList = new List<Object_AIRTRACK>();
                            List<string> arspNames = new List<string>();

                            foreach (AIRTRACK_Airspace_Segment arspSeg in ARSPS_LIST)
                            {

                                if (arspSeg.INFO_AIRTRACK.StartsWith(ID_Arsps)) segments.Add(arspSeg);
                                else
                                {

                                    if (segments.Count > 0)
                                    {
                                        AirspaceVolume_AIRTRACK arspsVol = new AirspaceVolume_AIRTRACK(segments);
                                        arspVolList.Add(arspsVol);

                                        if (arspNames.IndexOf(arspsVol.AirspaceVolumeID) < 0) arspNames.Add(arspsVol.AirspaceVolumeID);
                                        segments = new List<AIRTRACK_Airspace_Segment>();
                                    }
                                    ID_Arsps = arspSeg.INFO_AIRTRACK;
                                    segments = new List<AIRTRACK_Airspace_Segment>();
                                    segments.Add(arspSeg);


                                }


                            }

                            // добавим самый последний
                            if (segments.Count > 0)
                            {
                                AirspaceVolume_AIRTRACK arspsVol = new AirspaceVolume_AIRTRACK(segments);
                                if (arspsVol != null)
                                {
                                    arspVolList.Add(arspsVol);
                                    if (arspNames.IndexOf(arspsVol.AirspaceVolumeID) < 0) arspNames.Add(arspsVol.AirspaceVolumeID);

                                }
                            }

                            if ((arspVolList.Count > 0) && (arspNames.Count > 0))
                            {

                                foreach (string arspId in arspNames)
                                {
                                    ARSPS_LIST = (from element in arspVolList where (element != null) && (element is AirspaceVolume_AIRTRACK) && (element as AirspaceVolume_AIRTRACK).AirspaceVolumeID.StartsWith(arspId) select element).ToList();

                                    if ((ARSPS_LIST != null) && (ARSPS_LIST.Count > 0))
                                    {
                                        List<AirspaceVolume_AIRTRACK> volums = new List<AirspaceVolume_AIRTRACK>();
                                        foreach (Object_AIRTRACK objArsps in ARSPS_LIST)
                                        {
                                            volums.Add(objArsps as AirspaceVolume_AIRTRACK);
                                        }
                                        Airspace_AIRTRACK arsps = new Airspace_AIRTRACK(volums);

                                        ListOf_Object_AIRTRACK.Add(arsps);
                                    }
                                }
                            }

                        }

                        #endregion


                        #endregion

                        ListOf_Object_AIRTRACK.Add(new AREA_AIRTRACK(Area.AreaPolygon, Area.CountryName));
                        FillResultsTree(ListOf_Object_AIRTRACK, treeView2);

                    }
                    break;

                case (3):
                    //System.Diagnostics.Debug.WriteLine("");
                    break;
                case(11):
                   
                    break;
                case (10):

                    if (!System.Diagnostics.Debugger.IsAttached) alrtForm.Show();

                    DecodeOBSTCLE_File(textBox1.Text);

                    alrtForm.Close();
                    //Static_Proc.CloseProgressBar();
                    //tabControl1.SelectedIndex++;
                    break;
                case (12):

 
                    foreach (TreeNode nd in treeView1.Nodes)
                    {
                        if ((nd.Checked)&& (nd.Tag!=null))
                        {
                            List<string> result = (List<string>)nd.Tag;
                            string[] headers = result[0].Split('\t');

                            DataTable dataTable1 = new DataTable();

                            foreach (string header in headers)
                            {
                                dataTable1.Columns.Add(header, typeof(string), null);
                                //System.Diagnostics.Debug.WriteLine(header);
                            }


                            for (int i = 1; i < result.ToArray().Length; i++)
                                dataTable1.Rows.Add(result[i].Split('\t'));

                            CreateAIRTRACKObstacles(dataTable1);
                            FillResultsTree(ListOf_Object_AIRTRACK, treeView2);

                        }
                    }
                    break;
            }

        }


        
        public static void FillResultsTree(List<Object_AIRTRACK> AIRTRACK_Obj_list, TreeView TargetTreeView)
        {
            TargetTreeView.Nodes.Clear();
            TargetTreeView.BeginUpdate();

            #region ARP/RWY/RDN/ILS

            List<Object_AIRTRACK> ARP_LIST = (from element in AIRTRACK_Obj_list where (element != null) && (element is AIRPORT_AIRTRACK) select element).ToList();

            TreeNode rwyNode = null;
            TreeNode rdnNode = null;

            TreeNode ADHP_RWY_Node = new TreeNode("ARP/RWY/RDN/ILS");


            foreach (AIRPORT_AIRTRACK ARP in ARP_LIST)
            {
                ARINC_Airport_Primary_Record ARINC_Airport = (ARINC_Airport_Primary_Record)ARP.ARINC_OBJ;
                TreeNode arpNode = new TreeNode("ARP " + ARINC_Airport.Airport_ICAO_Identifier);
                arpNode.Tag = ARP;
                arpNode.Name = ARINC_Airport.Airport_ICAO_Identifier;

                foreach (RunWay_AIRTRACK rwy in ARP.LinkedRWY)
                {
                    rwyNode = new TreeNode("RWY " + rwy.RWY_Designator);
                    rwyNode.Tag = rwy;
                    rwyNode.Name = rwy.RWY_Designator;

                    foreach (RunWay_THR_AIRTRACK rdn in rwy.LinkedTHR)
                    {
                        rdnNode = new TreeNode(rdn.RDN_TXT_DESIG);
                        rdnNode.Tag = rdn;
                        rdnNode.Name = rdn.RDN_TXT_DESIG;

                        if (rdn.RelatedIls != null)
                        {
                            ARINC_LocalizerGlideSlope_Primary_Record ARINC_ILS = (ARINC_LocalizerGlideSlope_Primary_Record)rdn.RelatedIls.ARINC_OBJ;

                            TreeNode ilsNode = new TreeNode("ILS " + ARINC_ILS.Localizer_Identifier);
                            ilsNode.Tag = rdn.RelatedIls;
                            ilsNode.Name = ARINC_ILS.Localizer_Identifier;

                            if (rdn.RelatedIls.Shape == null) rdn.RelatedIls.Shape = new Shape_AIRTRACK(ARINC_ILS.Localizer_Latitude, ARINC_ILS.Localizer_Longitude);
                            if (rdn.RelatedIls.GlideSlope_Shape == null) rdn.RelatedIls.GlideSlope_Shape = new Shape_AIRTRACK(ARINC_ILS.Glide_Slope_Latitude, ARINC_ILS.Glide_Slope_Longitude);

                            TreeNode llzNode = new TreeNode("LLZ X: " + rdn.RelatedIls.Shape.GeoLong + "; Y: " + rdn.RelatedIls.Shape.GeoLat);

                            TreeNode igpNode = new TreeNode("IGP X: " + rdn.RelatedIls.GlideSlope_Shape.GeoLong + "; Y: " + rdn.RelatedIls.GlideSlope_Shape.GeoLat);

                            ilsNode.Nodes.Add(llzNode);
                            ilsNode.Nodes.Add(igpNode);

                            rdnNode.Nodes.Add(ilsNode);
                        }

                        if (rdn.RelatedMarker != null)
                        {
                            foreach (var item in rdn.RelatedMarker)
                            {
                                TreeNode mkrNode = new TreeNode("MarkerBeacon " + ((ARINC_Airport_Marker)item.ARINC_OBJ).Marker_Type);
                                mkrNode.Tag = item.ARINC_OBJ;
                                mkrNode.Name = ((ARINC_Airport_Marker)item.ARINC_OBJ).Marker_Type;
                                rdnNode.Nodes.Add(mkrNode);
                            }

                        }

                        rwyNode.Nodes.Add(rdnNode);
                    }


                }

                if (rwyNode != null) arpNode.Nodes.Add(rwyNode);
                rwyNode = null;
                rdnNode = null;



                if (ARP.LinkedProceduresList != null)
                {
                    TreeNode iapNode = new TreeNode("IAP");
                    TreeNode sidNode = new TreeNode("SID");
                    TreeNode starNode = new TreeNode("STAR");

                    foreach (Procedure_AIRTRACK proc in ARP.LinkedProceduresList)
                    {

                        TreeNode procNode = new TreeNode(proc.Proc_Identifier);
                        procNode.Tag = proc;

                        //if ((proc.DebugMessages!=null) && (proc.DebugMessages.Count>0))
                        //    procNode.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
                        //else
                        //    procNode.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));

                        foreach (ProcedureBranch_AIRTRACK branch in proc.Branches)
                        {
                            TreeNode branchNode = new TreeNode(branch.Proc_Branch_Identifier + " " + branch.Route_type.ToString());
                            branchNode.Tag = branch;

                            foreach (Leg_AIRTRACK leg in branch.ProcedureLegs)
                            {
                                TreeNode legNode = new TreeNode(leg.Path_and_Termination);
                                legNode.Tag = leg.ARINC_ProcedureLeg;
                                branchNode.Nodes.Add(legNode);
                            }

                            procNode.Nodes.Add(branchNode);
                        }
                        if (proc.Proc_type == PROC_TYPE_code.Approach) iapNode.Nodes.Add(procNode);
                        if (proc.Proc_type == PROC_TYPE_code.SID) sidNode.Nodes.Add(procNode);
                        if (proc.Proc_type == PROC_TYPE_code.STAR) starNode.Nodes.Add(procNode);



                    }

                    TreeNode procAllNode = new TreeNode("Procedures");

                    iapNode.Text = iapNode.Text + " (" + iapNode.Nodes.Count.ToString() + ")";
                    sidNode.Text = sidNode.Text + " (" + sidNode.Nodes.Count.ToString() + ")";
                    starNode.Text = starNode.Text + " (" + starNode.Nodes.Count.ToString() + ")";

                    procAllNode.Nodes.Add(iapNode);
                    procAllNode.Nodes.Add(sidNode);
                    procAllNode.Nodes.Add(starNode);

                    arpNode.Nodes.Add(procAllNode);
                }

                ADHP_RWY_Node.Nodes.Add(arpNode);


            }

            #endregion

            #region NAVAIDS

            TreeNode NAVAIDS_Node = new TreeNode("NAVAIDS");

            #region NDB

            TreeNode Ndb_Node = new TreeNode("NDB");

            List<Object_AIRTRACK> NDB_LIST = (from element in AIRTRACK_Obj_list where (element != null) && (element is NDB_AIRTRACK) select element).ToList();

            TreeNode Ndb_Enroute_Node = new TreeNode("NDB Enroute");
            TreeNode Ndb_Terminal_Node = new TreeNode("NDB Terminal");

            foreach (NDB_AIRTRACK NDB in NDB_LIST)
            {
                ARINC_Navaid_NDB_Primary_Record ARINC_Navaid_NDB = ((ARINC_Navaid_NDB_Primary_Record)NDB.ARINC_OBJ);

                TreeNode ndb_nd = new TreeNode("NDB " + ARINC_Navaid_NDB.Navaid_Identifier);
                ndb_nd.Tag = NDB;
                ndb_nd.Name = ARINC_Navaid_NDB.Navaid_Identifier;

                if (ARINC_Navaid_NDB.Object_Type.CompareTo("EnrouteNavaidNDB") == 0)
                    Ndb_Enroute_Node.Nodes.Add(ndb_nd);
                else if (ARINC_Navaid_NDB.Object_Type.CompareTo("TerminalNavaidNDB") == 0)
                    Ndb_Terminal_Node.Nodes.Add(ndb_nd);
            }

            Ndb_Node.Nodes.Add(Ndb_Terminal_Node);
            Ndb_Node.Nodes.Add(Ndb_Enroute_Node);

            NAVAIDS_Node.Nodes.Add(Ndb_Node);

            #endregion

            #region VHF

            //TreeNode VHF_Node = new TreeNode("VHF");
            List<Object_AIRTRACK> VHF_LIST = (from element in AIRTRACK_Obj_list where (element != null) && (element is VHF_NAVAID_AIRTRACK) select element).ToList();

            TreeNode VOR_Node = new TreeNode("VOR");
            TreeNode VOR_DME_Node = new TreeNode("VOR/DME");
            TreeNode VOR_TACAN_Node = new TreeNode("VOR/TACAN");
            TreeNode DME_Node = new TreeNode("DME");
            TreeNode TACAN_Node = new TreeNode("TACAN");

            foreach (VHF_NAVAID_AIRTRACK VHF_NAV in VHF_LIST)
            {
                TreeNode vhf_Node = new TreeNode("VHF");
                ARINC_Navaid_VHF_Primary_Record ARINC_Navaid_VOR = (ARINC_Navaid_VHF_Primary_Record)VHF_NAV.ARINC_OBJ;
                if (VHF_NAV.VHF_code == VHF_NAVAID_AIRTRACK.VHF_NAVAID_code.VOR)
                {
                    vhf_Node.Text = "VOR " + ARINC_Navaid_VOR.Navaid_Identifier + " (" + ARINC_Navaid_VOR.VHF_Name.Trim() + ")";
                    vhf_Node.Tag = ((VOR_AIRTRACK)VHF_NAV);
                    VOR_Node.Nodes.Add(vhf_Node);
                }
                else if (VHF_NAV.VHF_code == VHF_NAVAID_AIRTRACK.VHF_NAVAID_code.VOR_DME)
                {
                    vhf_Node.Text = "VOR/DME " + ARINC_Navaid_VOR.Navaid_Identifier + " (" + ARINC_Navaid_VOR.VHF_Name.Trim() + ")"; ;
                    vhf_Node.Tag = ((VOR_AIRTRACK)VHF_NAV);
                    VOR_DME_Node.Nodes.Add(vhf_Node);
                }
                else if (VHF_NAV.VHF_code == VHF_NAVAID_AIRTRACK.VHF_NAVAID_code.VOR_TACAN)
                {
                    vhf_Node.Text = "VOR/TACAN " + ARINC_Navaid_VOR.Navaid_Identifier + " (" + ARINC_Navaid_VOR.VHF_Name.Trim() + ")"; ;
                    vhf_Node.Tag = ((VOR_AIRTRACK)VHF_NAV);
                    VOR_TACAN_Node.Nodes.Add(vhf_Node);
                }
                else if (VHF_NAV.VHF_code == VHF_NAVAID_AIRTRACK.VHF_NAVAID_code.DME)
                {
                    vhf_Node.Text = "DME " + ARINC_Navaid_VOR.Navaid_Identifier + " (" + ARINC_Navaid_VOR.VHF_Name.Trim() + ")"; ;
                    vhf_Node.Tag = ((DME_AIRTRACK)VHF_NAV);
                    DME_Node.Nodes.Add(vhf_Node);
                }
                else if (VHF_NAV.VHF_code == VHF_NAVAID_AIRTRACK.VHF_NAVAID_code.TACAN)
                {
                    vhf_Node.Text = "TACAN " + ARINC_Navaid_VOR.Navaid_Identifier + " (" + ARINC_Navaid_VOR.VHF_Name.Trim() + ")"; ;
                    vhf_Node.Tag = ((TACAN_AIRTRACK)VHF_NAV);
                    TACAN_Node.Nodes.Add(vhf_Node);
                }
                vhf_Node.Name = ARINC_Navaid_VOR.Navaid_Identifier;
            }


            NAVAIDS_Node.Nodes.Add(VOR_Node);
            NAVAIDS_Node.Nodes.Add(VOR_DME_Node);
            NAVAIDS_Node.Nodes.Add(VOR_TACAN_Node);
            NAVAIDS_Node.Nodes.Add(DME_Node);
            NAVAIDS_Node.Nodes.Add(TACAN_Node);

            //TargetTreeView.Nodes.Add(VOR_Node);
            //TargetTreeView.Nodes.Add(VOR_DME_Node);
            //TargetTreeView.Nodes.Add(VOR_TACAN_Node);
            //TargetTreeView.Nodes.Add(DME_Node);
            #endregion


            #region Waypoints

            TreeNode Waypoint_Node = new TreeNode("Waypoints");

            List<Object_AIRTRACK> Waypoint_LIST = (from element in AIRTRACK_Obj_list where (element != null) && (element is WayPoint_AIRTRACK) select element).ToList();

            TreeNode Waypoint_Enroute_Node = new TreeNode("Enroute");
            TreeNode Waypoint_Terminal_Node = new TreeNode("Terminal");


            foreach (WayPoint_AIRTRACK WYPNT in Waypoint_LIST)
            {
                ARINC_WayPoint_Primary_Record ARINC_WayPoint = ((ARINC_WayPoint_Primary_Record)WYPNT.ARINC_OBJ);
                TreeNode wyp_nd = new TreeNode("WayPoints " + ARINC_WayPoint.Waypoint_Identifier);
                wyp_nd.Name = ARINC_WayPoint.Waypoint_Identifier;
                wyp_nd.Tag = WYPNT;


                if (ARINC_WayPoint.Object_Type.CompareTo("WaypointEnroute") == 0)
                    Waypoint_Enroute_Node.Nodes.Add(wyp_nd);
                else if (ARINC_WayPoint.Object_Type.CompareTo("WaypointTerminal") == 0)
                    Waypoint_Terminal_Node.Nodes.Add(wyp_nd);
            }

            Waypoint_Node.Nodes.Add(Waypoint_Terminal_Node);
            Waypoint_Node.Nodes.Add(Waypoint_Enroute_Node);

            #endregion

  
            #endregion

            #region Enroute

            TreeNode Enroute_Node = new TreeNode("Enroute");

            List<Object_AIRTRACK> Enrout_LIST = (from element in AIRTRACK_Obj_list where (element != null) && (element is ROUTE_AIRTRACK) select element).ToList();


            foreach (ROUTE_AIRTRACK rte in Enrout_LIST)
            {
                TreeNode rte_nd = new TreeNode("Route  " + rte.RouteName);
                rte_nd.Tag = rte;
                rte_nd.Name = rte.RouteName;

                foreach (SEGMENT_AIRTRACK seg in rte.Segments)
                {
                    TreeNode seg_nd = new TreeNode((seg.StartPnt as SEGMENT_POINT_AIRTRACK).SegmentPoint_Designator + " : " + (seg.EndPnt as SEGMENT_POINT_AIRTRACK).SegmentPoint_Designator);
                    rte_nd.Nodes.Add(seg_nd);
                }

                Enroute_Node.Nodes.Add(rte_nd);
            }

            #endregion


            #region Airspaces
            TreeNode Airspace_Node = new TreeNode("Airspace");



            List<Object_AIRTRACK> FIR_AIRSPACE_LIST = (from element in AIRTRACK_Obj_list where (element != null) && (element is AirspaceVolume_AIRTRACK) select element).ToList();
            TreeNode fir_nd = new TreeNode("FIR/UIR Airspace");
            TreeNode cntrl_nd = new TreeNode("Controlled Airspace");
            TreeNode rest_nd = new TreeNode("Restrictive Airspace");

            foreach (AirspaceVolume_AIRTRACK arsps in FIR_AIRSPACE_LIST)
            {
                TreeNode arsps_nd = new TreeNode(arsps.CodeId + " " + arsps.CodeType);
                arsps_nd.Tag = arsps.ARINC_OBJ;
                arsps_nd.Name = arsps.CodeId + " " + arsps.CodeType;

                if (arsps.GeomComponent[0].Tag is ARINC_FIR_UIR_Primary_Records) fir_nd.Nodes.Add(arsps_nd);
                if (arsps.GeomComponent[0].Tag is ARINC_Controlled_Airspace_Primary_Records) cntrl_nd.Nodes.Add(arsps_nd);
                if (arsps.GeomComponent[0].Tag is ARINC_Restrictive_Airspace_Primary_Records) rest_nd.Nodes.Add(arsps_nd);

            }
            Airspace_Node.Nodes.Add(fir_nd);
            Airspace_Node.Nodes.Add(cntrl_nd);
            Airspace_Node.Nodes.Add(rest_nd);

            #endregion

            #region Ostacles

            TreeNode obstacles_Node = new TreeNode("Obstacles");

            List<Object_AIRTRACK> obs_LIST = (from element in AIRTRACK_Obj_list where (element != null) && (element is Obstacle_AIRTRACK) select element).ToList();


            foreach (Obstacle_AIRTRACK obs in obs_LIST)
            {
                TreeNode obs_nd = new TreeNode("Obs  " + obs.ID);
                obs_nd.Tag = obs;
                obs_nd.Name = obs.ID;

                obstacles_Node.Nodes.Add(obs_nd);
            }

            #endregion


            TargetTreeView.Nodes.Add(ADHP_RWY_Node);
            TargetTreeView.Nodes.Add(NAVAIDS_Node);
            TargetTreeView.Nodes.Add(Waypoint_Node);
            TargetTreeView.Nodes.Add(Enroute_Node);
            TargetTreeView.Nodes.Add(Airspace_Node);
            TargetTreeView.Nodes.Add(obstacles_Node);

            TargetTreeView.EndUpdate();
        }

  
        private List<RunWay_AIRTRACK> GetRWYs(List<Object_AIRTRACK> RNDList)
        {
            RunWay_AIRTRACK rwy = new RunWay_AIRTRACK();
            List<RunWay_AIRTRACK> RWYList = new List<RunWay_AIRTRACK>();
            List<RunWay_THR_AIRTRACK> THRs = new List<RunWay_THR_AIRTRACK>();

            List<string> rwyDESIGNATOR_List = new List<string>();

            foreach (Object_AIRTRACK RDN in RNDList)
            {
                THRs.Add(RDN as RunWay_THR_AIRTRACK);

                var OPPOSITE_RDN = from element in RNDList where (element != null) && (((RunWay_THR_AIRTRACK)element).RDN_TXT_DESIG.Trim() == ((RunWay_THR_AIRTRACK)RDN).OPPOSITE_RDN_TXT_DESIG.Trim()) select element;

                if (OPPOSITE_RDN.Count() > 0)
                {
                    THRs.Add((RunWay_THR_AIRTRACK)OPPOSITE_RDN.First());
                }

                if (rwyDESIGNATOR_List.IndexOf(rwy.RWY_Designator) < 0)
                {
                    rwy = new RunWay_AIRTRACK(THRs);
                    rwy.RWY_Length_M = (RDN as RunWay_THR_AIRTRACK).RWY_LENGTH_M;
                    rwy.RWY_Width_M = (RDN as RunWay_THR_AIRTRACK).RWY_WIDTH_M;

                    RWYList.Add(rwy);
                    rwyDESIGNATOR_List.Add(rwy.RWY_Designator);
                }

                THRs.Clear();


            }

            return RWYList;
        }

        private List<ProcedureBranch_AIRTRACK> GetProceduresBranchesList(List<Object_AIRTRACK> ProcLegList)
        {

            List<ProcedureBranch_AIRTRACK> result = new List<ProcedureBranch_AIRTRACK>();

            Dictionary<string, List<Leg_AIRTRACK>> ProcDictionary = new Dictionary<string, List<Leg_AIRTRACK>>();
            List<Leg_AIRTRACK> legs = new List<Leg_AIRTRACK>();

            foreach (Object_AIRTRACK LEG in ProcLegList)
            {
                Leg_AIRTRACK currentLeg = (Leg_AIRTRACK)LEG;
                if (ProcDictionary.ContainsKey(currentLeg.ARINC_ProcedureLeg.Airport_Identifier + currentLeg.Procedure_Identifier + currentLeg.Transition_Identifier))
                {
                    legs = ProcDictionary[currentLeg.ARINC_ProcedureLeg.Airport_Identifier + currentLeg.Procedure_Identifier + currentLeg.Transition_Identifier];
                }
                else
                {
                    ProcDictionary.Add(currentLeg.ARINC_ProcedureLeg.Airport_Identifier + currentLeg.Procedure_Identifier + currentLeg.Transition_Identifier, new List<Leg_AIRTRACK>());
                    legs = ProcDictionary[currentLeg.ARINC_ProcedureLeg.Airport_Identifier + currentLeg.Procedure_Identifier + currentLeg.Transition_Identifier];
                }

                legs.Add(currentLeg);
            }



            foreach (KeyValuePair<string, List<Leg_AIRTRACK>> pair in ProcDictionary)
            {
                ProcedureBranch_AIRTRACK Procedure = null;
                Procedure = new ProcedureBranch_AIRTRACK(pair.Value[0].ARINC_ProcedureLeg.Name);
                Procedure.Proc_Branch_Identifier = pair.Value[0].Procedure_Identifier;
                Procedure.AirportIdentifier = pair.Value[0].ARINC_ProcedureLeg.Airport_Identifier;
                Procedure.Transition_Identifier = pair.Value[0].Transition_Identifier;
                Procedure.Proc_Identifier = pair.Value[0].ARINC_ProcedureLeg.SID_STAR_Approach_Identifier;

                Procedure.ProcedureLegs = new List<Leg_AIRTRACK>();
                Procedure.ProcedureLegs.AddRange(pair.Value);

                Procedure.DefineType();

                LegsSequenceCheck(Procedure);
                ProcedureBeginingEndingCheck(Procedure);

                result.Add(Procedure);

               
            }


            
            

            return result;
        }

        private List<Procedure_AIRTRACK> GetprocedureLis(List<ProcedureBranch_AIRTRACK> BranchList)
        {
            List<Procedure_AIRTRACK> result = new List<Procedure_AIRTRACK>();

            Dictionary<string, List<ProcedureBranch_AIRTRACK>> ProcDictionary = new Dictionary<string, List<ProcedureBranch_AIRTRACK>>();
            List<ProcedureBranch_AIRTRACK> legs = new List<ProcedureBranch_AIRTRACK>();

            foreach (ProcedureBranch_AIRTRACK _branch in BranchList)
            {
                ProcedureBranch_AIRTRACK currenBranch = (ProcedureBranch_AIRTRACK)_branch;
                if (ProcDictionary.ContainsKey(currenBranch.Proc_Identifier))
                {
                    legs = ProcDictionary[currenBranch.Proc_Identifier];
                }
                else
                {
                    ProcDictionary.Add(currenBranch.Proc_Identifier, new List<ProcedureBranch_AIRTRACK>());
                    legs = ProcDictionary[currenBranch.Proc_Identifier];
                }

                legs.Add(currenBranch);
            }

            foreach (KeyValuePair<string, List<ProcedureBranch_AIRTRACK>> pair in ProcDictionary)
            {
                Procedure_AIRTRACK Procedure = null;
                Procedure = new Procedure_AIRTRACK(pair.Value);
                result.Add(Procedure);
            }

            return result;
        }

        private void ProcedureBeginingEndingCheck(ProcedureBranch_AIRTRACK Procedure)
        {
            //try
            //{
            //    if (Procedure.ProcedureLegs.Count >= 2)
            //    {
            //        if (Procedure.DebugMessages==null) Procedure.DebugMessages = new List<string>();

                    
            //            string begining_leg = Procedure.ProcedureLegs[0].Path_and_Termination;
            //            string ending_leg = Procedure.ProcedureLegs[Procedure.ProcedureLegs.Count-1].Path_and_Termination;

            //            switch (Procedure.Proc_type)
            //            {
            //                case(ARINC_DECODER_CORE.AIRTRACK_Objects.Procedure_AIRTRACK.PROC_TYPE_code.SID):

            //                    #region

            //                        if ((Procedure.Route_type.Length>0) && ("014FT".IndexOf(Procedure.Route_type) >= 0))
            //                        {
            //                            if ("CACDCFCICRDFFAFCFDFMIFVAVDVIVMVR".IndexOf(begining_leg) < 0)
            //                            {
            //                                Procedure.DebugMessages.Add("WARNING. SID. Route type is " + Procedure.Route_type + " First Leg is " + begining_leg);
            //                            }

            //                            if ("AFCFDFFMHAHMRFTFVM".IndexOf(ending_leg) < 0)
            //                            {
            //                                Procedure.DebugMessages.Add("WARNING. SID. Route type is " + Procedure.Route_type + " Final Leg is " + ending_leg);
            //                            }
            //                        }

            //                        else if ((Procedure.Route_type.Length > 0) && ("25M".IndexOf(Procedure.Route_type) >= 0))
            //                        {
            //                            if ("CACDCFCICRDFFAFCFDFMIFVAVDVIVMVR".IndexOf(begining_leg) < 0)
            //                            {
            //                                Procedure.DebugMessages.Add("WARNING. SID. Route type is " + Procedure.Route_type + " First Leg is " + begining_leg);
            //                            }

            //                            if ("AFCFDFFMHAIFTFRFVM".IndexOf(ending_leg) < 0)
            //                            {
            //                                Procedure.DebugMessages.Add("WARNING. SID. Route type is " + Procedure.Route_type + " Final Leg is " + ending_leg);
            //                            }
            //                        }

            //                        else if ((Procedure.Route_type.Length > 0) && ("36SV".IndexOf(Procedure.Route_type) >= 0))
            //                        {
            //                            if ("FAFCFDIF".IndexOf(begining_leg) < 0)
            //                            {
            //                                Procedure.DebugMessages.Add("WARNING. SID. Route type is " + Procedure.Route_type + " First Leg is " + begining_leg);
            //                            }

            //                            if ("AFCFDFHARFTF".IndexOf(ending_leg) < 0)
            //                            {
            //                                Procedure.DebugMessages.Add("WARNING. SID. Route type is " + Procedure.Route_type + " Final Leg is " + ending_leg);
            //                            }
            //                        }

            //                    #endregion

            //                    break;

            //                case (ARINC_DECODER_CORE.AIRTRACK_Objects.Procedure_AIRTRACK.PROC_TYPE_code.STAR):

            //                    #region

            //                        if ((Procedure.Route_type.Length > 0) && ("147F".IndexOf(Procedure.Route_type) >= 0))
            //                        {
            //                            if ("FDFCIF".IndexOf(begining_leg) < 0)
            //                            {
            //                                Procedure.DebugMessages.Add("WARNING. STAR. Route type is " + Procedure.Route_type + " First Leg is " + begining_leg);
            //                            }

            //                            if ("AFCFDFHMRFTF".IndexOf(ending_leg) < 0)
            //                            {
            //                                Procedure.DebugMessages.Add("WARNING. STAR. Route type is " + Procedure.Route_type + " Final Leg is " + ending_leg);
            //                            }
                                    
            //                        }

            //                        else if ((Procedure.Route_type.Length > 0) && ("258M".IndexOf(Procedure.Route_type) >= 0))
            //                        {
            //                            if ("FDFCIF".IndexOf(begining_leg) < 0)
            //                            {
            //                                Procedure.DebugMessages.Add("WARNING. STAR. Route type is " + Procedure.Route_type + " First Leg is " + begining_leg);
            //                            }

            //                            if ("AFCFDFFMIFRFTFVM".IndexOf(ending_leg) < 0)
            //                            {
            //                                Procedure.DebugMessages.Add("WARNING. STAR. Route type is " + Procedure.Route_type + " Final Leg is " + ending_leg);
            //                            }

            //                        }

            //                        else if ((Procedure.Route_type.Length > 0) && ("369S".IndexOf(Procedure.Route_type) >= 0))
            //                        {
            //                            if ("FDFCHFIF".IndexOf(begining_leg) < 0)
            //                            {
            //                                Procedure.DebugMessages.Add("WARNING. STAR. Route type is " + Procedure.Route_type + " First Leg is " + begining_leg);
            //                            }

            //                            if ("AFCFFMHFHMRFTFVM".IndexOf(ending_leg) < 0)
            //                            {
            //                                Procedure.DebugMessages.Add("WARNING. STAR. Route type is " + Procedure.Route_type + " Final Leg is " + ending_leg);
            //                            }

            //                        }

            //                    #endregion

            //                    break;
            //                case (ARINC_DECODER_CORE.AIRTRACK_Objects.Procedure_AIRTRACK.PROC_TYPE_code.IAP):

            //                    #region

            //                        if ((Procedure.Route_type.Length > 0) && ("A".IndexOf(Procedure.Route_type) >= 0))
            //                        {
            //                            if ("FCFDHFIFPI".IndexOf(begining_leg) < 0)
            //                            {
            //                                Procedure.DebugMessages.Add("WARNING. Approach. Route type is " + Procedure.Route_type + " First Leg is " + begining_leg);
            //                            }

            //                            if ("AFCFCIHFHMPIRFTFVI".IndexOf(ending_leg) < 0)
            //                            {
            //                                Procedure.DebugMessages.Add("WARNING. Approach. Route type is " + Procedure.Route_type + " Final Leg is " + ending_leg);
            //                            }

            //                        }

            //                        else if ((Procedure.Route_type.Length > 0) && ("Z".IndexOf(Procedure.Route_type) >= 0))
            //                        {
            //                            if ("AFCACDCFCICRDFFAFCFDFMHAHMRFVAVDVIVMVR".IndexOf(begining_leg) < 0)
            //                            {
            //                                Procedure.DebugMessages.Add("WARNING. Approach. Route type is " + Procedure.Route_type + " First Leg is " + begining_leg);
            //                            }

            //                            if ("AFCACFDFFMHMRFTFVAVM".IndexOf(ending_leg) < 0)
            //                            {
            //                                Procedure.DebugMessages.Add("WARNING. Approach. Route type is " + Procedure.Route_type + " Final Leg is " + ending_leg);
            //                            }

            //                        }

            //                        else if (Procedure.Route_type.Length > 0)
            //                        {
            //                            if ("IF".IndexOf(begining_leg) < 0)
            //                            {
            //                                Procedure.DebugMessages.Add("WARNING. Approach. Route type is " + Procedure.Route_type + " First Leg is " + begining_leg);
            //                            }

            //                            if ("CFRFTF".IndexOf(ending_leg) < 0)
            //                            {
            //                                Procedure.DebugMessages.Add("WARNING. Approach. Route type is " + Procedure.Route_type + " Final Leg is " + ending_leg);
            //                            }

            //                        }



            //                    #endregion

            //                    break;
            //            }



            //    }

            //}
            //catch { }
        }

        private void LegsSequenceCheck(ProcedureBranch_AIRTRACK Procedure)
        {

            #region Sequence array
            string[] Sequence = new string[171] 
            { 
               "AFDF", "AFIF", "AFPI", 
               "CAAF", "CAHA", "CAHF", "CAHM", "CAPI", "CARF", "CATF", 
               "CDHA", "CDHF", "CDHM", "CDPI", "CDRF", "CDTF", 
               "CFIF", 
               "CICA", "CICD", "CICI", "CICR", "CIDF", "CIHA", "CIHF", "CIHM", "CIPI", "CIRF", "CITF", "CIVA", "CIVD", "CIVI", "CIVM", "CIVR", 
               "CRAF", "CRHA", "CRHF", "CRHM", "CRPI", "CRRF", "CRTF", 
               "DFIF", "DFRF", 
               "FAAF", "FAHA", "FAHF", "FAHM", "FAPI", "FARF", "FATF", 
               "FCAF", "FCFA", "FCFC", "FCFD", "FCFM", "FCHA", "FCHF", "FCHM", "FCIF", "FCPI", "FCRF", "FCTF", 
               "FDFA", "FDFD", "FDFD", "FDFM", "FDHA", "FDHF", "FDHM", "FDIF", "FDPI", "FDRF", "FDTF", 
               "FMAF", "FMHA", "FMHF", "FMHM", "FMIF", "FMPI", "FMRF", "FMTF", 
               "HAHA", "HAHF", "HAHM", "HAIF", "HAPI", 
               "HFHA", "HFHF", "HFHM", "HFIF", "HFPI", 
               "HMHA", "HMHF", "HMHM", "HMIF", "HMPI", 
               "IFIF", 
               "PIAF", "PICA", "PICD", "PICI", "PICR", "PIDF", "PIFA", "PIFC", "PIFD", "PIFM", "PIHA", "PIHF", "PIHM", "PIIF", "PIPI", "PIRF", "PITF", "PIVA", "PIVD", "PIVI", "PIVM", "PIVR", 
               "RFAF", "RFDF", "RFIF", "RFPI", "RFVA", "RFVD", "RFVI", "RFVM", "RFVR", 
               "TFIF", 
               "VAAF", "VAHA", "VAHF", "VAHM", "VAPI", "VARF", "VATF", 
               "VDHA", "VDHF", "VDHM", "VDPI", "VDRF", "VDTF", 
               "VICA", "VICD", "VICI", "VICR", "VIDF", "VIHA", "VIHF", "VIHM", "VIPI", "VIRF", "VITF", "VIVA", "VIVD", "VIVI", "VIVM", "VIVR", 
               "VMAF", "VMHA", "VMHF", "VMHM", "VMPI", "VMRF", "VMTF", 
               "VRAF", "VRHA", "VRHF", "VRHM", "VRPI", "VRRF", "VRTF"
            };
            #endregion

            try
            {
                if (Procedure.ProcedureLegs.Count >= 2) 
                {
                    if (Procedure.DebugMessages == null) Procedure.DebugMessages = new List<string>();
                    for (int i = 0; i <= Procedure.ProcedureLegs.Count - 2; i++)
                    {
                        string leg_sequence = Procedure.ProcedureLegs[i].Path_and_Termination + Procedure.ProcedureLegs[i + 1].Path_and_Termination;
                        if (System.Array.IndexOf(Sequence, leg_sequence) > 0)
                        {
                            Procedure.DebugMessages.Add("WARNING. Legs Sequence " + leg_sequence);
                        }

                        
                    }

                }
  
                // 

            }
            catch { }
            
        }

        private void CreateAIRTRACKObjects (AreaInfo Area)// (IGeometry _AreaPolygon)
        {

            IGeometry _AreaPolygon = Area.AreaPolygon;
            AranSupport.Utilitys AranUtilS = new AranSupport.Utilitys();
            ListOf_Object_AIRTRACK = new List<Object_AIRTRACK>();
            int i = 0;
            foreach (ARINC_OBJECT _OBJ in ListOfObjects)
            {

                Object_AIRTRACK AIRTRAC_Obj = AIRTRAC_Objects_Creator.AIRTRAC_Object_Create(_OBJ);


                if ((AIRTRAC_Obj != null) && (AIRTRAC_Obj.Shape !=null) && (AranUtilS.WithinPolygon(_AreaPolygon, AIRTRAC_Obj.Shape.Geometry as IPoint)))
                    ListOf_Object_AIRTRACK.Add(AIRTRAC_Obj);
                else if ((AIRTRAC_Obj != null) && (AIRTRAC_Obj is Leg_AIRTRACK)) 
                    ListOf_Object_AIRTRACK.Add(AIRTRAC_Obj);
                else if ((AIRTRAC_Obj != null) && (AIRTRAC_Obj is SEGMENT_POINT_AIRTRACK))
                    ListOf_Object_AIRTRACK.Add(AIRTRAC_Obj);
                else if ((AIRTRAC_Obj != null) && (AIRTRAC_Obj.Shape != null) && (AIRTRAC_Obj is WayPoint_AIRTRACK) && (AranUtilS.WithinPolygon(_AreaPolygon.Envelope, AIRTRAC_Obj.Shape.Geometry as IPoint)))
                    ListOf_Object_AIRTRACK.Add(AIRTRAC_Obj);
                else if ((AIRTRAC_Obj != null) && (AIRTRAC_Obj is AIRTRACK_Airspace_Segment) && (AIRTRAC_Obj as AIRTRACK_Airspace_Segment).AreaCode.StartsWith(Area.FirstLetter))
                    ListOf_Object_AIRTRACK.Add(AIRTRAC_Obj);

                i++;

            }

            ////System.Diagnostics.Debug.WriteLine("");

        }

        private void CreateAIRTRACKObstacles(DataTable JeppesenObstacles)// (IGeometry _AreaPolygon)
        {

            AranSupport.Utilitys AranUtilS = new AranSupport.Utilitys();
            ListOf_Object_AIRTRACK = new List<Object_AIRTRACK>();

            foreach (DataRow DR in JeppesenObstacles.Rows)
            {
                Obstacle_AIRTRACK AIRTRAC_Obj = new Obstacle_AIRTRACK(DR);

                if ((AIRTRAC_Obj != null) && (AIRTRAC_Obj.Shape != null))
                    ListOf_Object_AIRTRACK.Add(AIRTRAC_Obj);

            }

            ////System.Diagnostics.Debug.WriteLine("");

        }   

        private void SaveAreaXmlfile()
        {

            string PathToSpecificationFile = Static_Proc.GetPathToSpecificationFile();
            string PathToRegionsFile =Static_Proc.GetPathToRegionsFile();

            //if (textBox3.Text.Length == 4) checkBox6.Checked = true;

            AreaInfo area = new AreaInfo();
            area.CountryName = comboBox1.Text;
            area.FirstLetter = comboBox3.Text;
            area.Region = comboBox2.Text;
            area.Reference_ADHP.ICAO_CODE = textBox3.Text.Trim();
            AreaUtils.WriteToAreaFile(PathToRegionsFile, PathToSpecificationFile, area);

        }

        private void DefineArea()
        {
            if (comboBox1.Items.Count>0) return;
            comboBox1.Items.Clear();

            string PathToSpecificationFile = Static_Proc.GetPathToSpecificationFile();
            string PathToRegionsFile = Static_Proc.GetPathToRegionsFile();

            comboBox1.Items.AddRange(AreaManager.AreaUtils.GetCountryList(PathToRegionsFile).ToArray());

            comboBox1.Tag = AreaManager.AreaUtils.GetCountryICAOCodes(PathToRegionsFile);
             
            //System.Diagnostics.Debug.WriteLine("DefineArea GetArea");


            AreaInfo Area = new AreaInfo();
            Area = AreaUtils.GetArea(PathToSpecificationFile);

            comboBox1.Text = Area.CountryName;
            comboBox2.Text = Area.Region;
            comboBox3.Text = Area.FirstLetter;
            textBox3.Text = Area.Reference_ADHP.ICAO_CODE;

        }

        private bool SaveARINCResults()
        {
            string area = "";
            string objtp="";
            bool res = false;
            Static_Proc.CloseProgressBar();
            string FilePath = ARINC_SPECIFICATION_FILE + "\\Results\\";


            if (!Directory.Exists(FilePath))
            {
                Directory.CreateDirectory(FilePath);
            }


            string[] filePaths = Directory.GetFiles(FilePath);
            foreach (string filePath in filePaths) File.Delete(filePath);

            // сохраним результаты в файл
            foreach (TreeNode parNd in treeView1.Nodes)
            {
                if (parNd.Checked)
                {

                    area = parNd.Text;
                    foreach (TreeNode chldNd in parNd.Nodes)
                    {
                        objtp = chldNd.Tag.ToString();
                        string[] words = chldNd.Text.Split(' ');
                        string objName = words[1].Trim().Remove(words[1].Length - 1, 1);

                        List<ARINC_OBJECT> ListToSave = (from element in ListOfObjects
                                                          where (element != null) &&
                                                                (element.CustomerArea == area) &&
                                                                (element.Object_Type == objtp) &&
                                                                (element.Name == objName)
                                                          select element).ToList();

                        if ((ListToSave!=null) && (ListToSave.Count>0))
                        {
                            int ObjCount = ListToSave.Count;
                            int Size = 1000;

                            int Steps = ObjCount / Size;
                            int part = ObjCount - Steps * Size;
                            int index = 1;

                            ARINC_OBJECT[] newList =  new ARINC_OBJECT[part];
                            ListToSave.CopyTo(0, newList, 0, part);
                            ARINC_DECODED_OBJECTS Res = new ARINC_DECODED_OBJECTS(textBox1.Text, newList.ToList());

                            string FN = area + "_" + objtp + "_" + objName + "Part" + index.ToString() + ".xml";
                            Static_Proc.Serialize(Res, FilePath + FN);
                            res = true;
                            index++;

                            for (int i = 0; i <= Steps - 1; i++)
                            {
                                newList = new ARINC_OBJECT[Size];
                                ListToSave.CopyTo(part + Size * i, newList, 0, Size);
                                Res = new ARINC_DECODED_OBJECTS(textBox1.Text, newList.ToList());

                                FN = area + "_" + objtp + "_" + objName + "Part" + index.ToString() + ".xml";
                                Static_Proc.Serialize(Res, FilePath + FN);
                                res = true;
                                index++;
                            }


                        }
                    }

                    break;
                }

            }

            return res;
        }

        private void DecodeARINCFile(string PathToArincFile)
        {

            ARINC_SPECIFICATION_FILE = Static_Proc.GetPathToARINCSpecificationFile();

            string PathToArincSpecificationFile = ARINC_SPECIFICATION_FILE + "\\Specification.xml";


             

                List<string> ListOfSectionsType = new List<string>();
                //выбор типов записей
                
                foreach (TreeNode Nd in treeView3.Nodes)
                {
                    if ((Nd.Checked) && (Nd.Tag != null) && (Nd.Tag.ToString().Length > 0))
                    {
                        string[] TAGS = Nd.Tag.ToString().Split(',');

                        foreach (string TAG in TAGS)
                            if (!ListOfSectionsType.Contains(TAG)) ListOfSectionsType.Add(TAG);
                    }


                    if (Nd.Nodes.Count > 0)
                    {
                        foreach (TreeNode NdTags in Nd.Nodes)
                        {
                            if ((NdTags.Checked) && (NdTags.Tag != null) && (NdTags.Tag.ToString().Length > 0))
                            {
                                string[] TAGS = NdTags.Tag.ToString().Split(',');

                                foreach (string TAG in TAGS)
                                    if (!ListOfSectionsType.Contains(TAG)) ListOfSectionsType.Add(TAG);
                            }
                        }
                    }
                }


                //декодируем файл
                Static_Proc.ShowProgressBar =!System.Diagnostics.Debugger.IsAttached;
                Static_Proc.PathToSpecificationFile = PathToArincSpecificationFile;

                DateTime start = DateTime.Now;


                    ListOfObjects = Static_Proc.DecodeARINCFile(System.IO.File.ReadAllLines(PathToArincFile), ListOfSectionsType,textBox3.Text.Trim());


                DateTime end = DateTime.Now;

                MessageBox.Show((start - end).Hours.ToString() + "h : " + (start - end).Minutes.ToString() + "min : " + (start - end).Seconds.ToString() + "sec : " + (start - end).Milliseconds.ToString());
            



                treeView1.Nodes.Clear();
                Application.DoEvents();


                if (ListOfObjects.Count == 0) return;

                #region выведем статистику в treeView1

                TreeNode AreaNode = new TreeNode();

                var queryObjectsGroup = from MyGroup in ListOfObjects group MyGroup by new { Area = MyGroup.CustomerArea, Type = MyGroup.Object_Type, Name = MyGroup.Name } into GroupOfObjects orderby GroupOfObjects.Key.Area select GroupOfObjects;

                string CurArea = queryObjectsGroup.First().Key.Area.ToString(); ;
                AreaNode.Text = CurArea;
                treeView1.BeginUpdate();

                foreach (var ARINCGroup in queryObjectsGroup)
                {
                    Console.WriteLine("{0},{1},{2}", ARINCGroup.Key.Area.ToString(), ARINCGroup.Key.Type.ToString(), ARINCGroup.Count().ToString());

                    if (CurArea.CompareTo(ARINCGroup.Key.Area.ToString()) == 0)
                    {
                        TreeNode SecndNode = new TreeNode();
                        SecndNode.Text = ARINCGroup.Key.Type.ToString() + " " + ARINCGroup.Key.Name.ToString() + ", " + ARINCGroup.Count().ToString();
                        SecndNode.Tag = ARINCGroup.Key.Type.ToString();
                        AreaNode.Nodes.Add(SecndNode);
                    }
                    else
                    {
                        treeView1.Nodes.Add(AreaNode);
                        AreaNode = new TreeNode();
                        AreaNode.Text = ARINCGroup.Key.Area.ToString();
                        TreeNode SecndNode = new TreeNode();
                        SecndNode.Text = ARINCGroup.Key.Type.ToString() + " " + ARINCGroup.Key.Name.ToString() + ", " + ARINCGroup.Count().ToString();
                        SecndNode.Tag = ARINCGroup.Key.Type.ToString();
                        AreaNode.Nodes.Add(SecndNode);
                    }
                    CurArea = ARINCGroup.Key.Area.ToString();

                }
                treeView1.Nodes.Add(AreaNode);
                treeView1.EndUpdate();

                treeView1.Select();

                #endregion;



                ///////////////////////////////////////////////////

                //string filePath = string.Format(@"C:\test\ARINC_ResultsInfo.txt");
                string filePath = ARINC_SPECIFICATION_FILE   + @"\ARINC_ResultsInfo.txt";

                string[] textData = System.IO.File.ReadAllLines(filePath);
                FilldataView(textData, dataGridView2);

            ///////////////////////////////////////////////////
               
        }

        private void FilldataView(string[] textData, DataGridView dgView)
        {
            string[] headers = textData[0].Split('\t');

            DataTable dataTable1 = new DataTable();

            foreach (string header in headers)
            {
                dataTable1.Columns.Add(header, typeof(string), null);
            }


            for (int i = 1; i < textData.Length; i++)
                dataTable1.Rows.Add(textData[i].Split('\t'));

            //Set the DataSource of DataGridView to the DataTable
            dgView.DataSource = dataTable1;

            foreach (DataGridViewColumn clmn in dgView.Columns)
            {
                clmn.AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
            }

        }

        private bool checkWizardState(int p)
        {
            bool res = false;
            switch(p)
            {
                case (0): 
                    res = textBox1.Text.Length > 0;
                    if (!res)
                    {
                        MessageBox.Show("Select source file", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        button2.Focus();
                    }
                    break;
                case(1):
                    res = comboBox1.Text.Length>0;
                    res = res && comboBox2.Text.Length>0;
                    res = res && comboBox3.Text.Length > 0;
                    if (comboBox1.Text.Length <= 0) MessageBox.Show("Select country");
                    if (comboBox2.Text.Length <= 0) MessageBox.Show("Select region");
                    if (comboBox3.Text.Length <= 0) MessageBox.Show("Set Country ICAO CODE");

                    break;
                case (2):
                    res = true;
                    break;

                default:
                    res = false;
                    break;
            }
            return res;
        }

        private void Prev_button_Click(object sender, EventArgs e)
        {
            HideLabelStep(tabControl1.SelectedIndex);


            int Flag = tabControl1.SelectedIndex;
            if (treeView3.Nodes[3].Checked) Flag = Flag + 10;

            switch (Flag)
            {
                case (3):
                case (2):
                case (1):
                case (10):
                case (11):
                case (13):
                    tabControl1.SelectedIndex--;
                    break;
                case (12):
                    tabControl1.SelectedIndex -= 2;
                    break;
            }

            Prev_button.Enabled = !(tabControl1.SelectedIndex == 0);
            Next_button.Enabled = true;
            button1.Enabled = false;

            ShowLabelStep(tabControl1.SelectedIndex);

        }

  
        private void Next_button_Click(object sender, EventArgs e)
        {
            if (!checkWizardState(tabControl1.SelectedIndex)) return;


            HideLabelStep(tabControl1.SelectedIndex);

            int Flag = tabControl1.SelectedIndex;
            if (treeView3.Nodes[3].Checked) Flag = Flag + 10;
           

            DoWizardsWork(Flag);

            switch (Flag)
            {
                case (0):
                case (1):
                case (2):
                case (11):
                case (12):
                    tabControl1.SelectedIndex++;
                    break;
                case (10):
                    tabControl1.SelectedIndex += 2;
                    break;
            }


            Next_button.Enabled = !(tabControl1.SelectedIndex == tabControl1.TabPages.Count - 1);
            Prev_button.Enabled = true;
            ShowLabelStep(tabControl1.SelectedIndex);

            button1.Enabled = !Next_button.Enabled;
        }

        private void ShowLabelStep(int p)
        {
            foreach (Control cntrl in panel1.Controls)
            {
                if ((cntrl.Tag != null) && (cntrl is Label) && (Convert.ToInt32(cntrl.Tag) == p))
                {
                    (cntrl as Label).Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
                    (cntrl as Label).ForeColor = System.Drawing.Color.DarkOrange;

                }
            }
        }

        private void HideLabelStep(int p)
        {
            foreach (Control cntrl in panel1.Controls)
            {
                if ((cntrl.Tag != null) && (cntrl is Label) && (Convert.ToInt32(cntrl.Tag) == p))
                {
                    (cntrl as Label).Font = new System.Drawing.Font("Times New Roman", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
                    (cntrl as Label).ForeColor = System.Drawing.Color.White;
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();

            if (fileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                textBox1.Text = fileDialog.FileName;
            }

            Next_button.Enabled = SetEnabledState();
        }

        private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node == null) return;

            if (!treeView3.Nodes[3].Checked)
                ShowARINCObj(e.Node);
            else
                ShowObstacleList(e.Node);

        }

        private void ShowObstacleList(TreeNode Nd)
        {
            if ((Nd.Tag != null) && (Nd.Checked))
            {
                List<string> lst = (List<string>)Nd.Tag;
                FilldataView(lst.ToArray(), dataGridView1);
            }
        }

        private void ShowARINCObj(TreeNode Nd)
        {
            if (Nd.Parent != null)
            {
                SelAreaCode = Nd.Parent.Text;
                SelObjType = Nd.Tag.ToString();
                if (Nd.Checked) Nd.Parent.Checked = true;
                else
                {
                    bool ch = Nd.Parent.Nodes[0].Checked;
                    foreach (TreeNode nd in Nd.Parent.Nodes)
                    {
                        ch = ch || nd.Checked;
                    }

                    Nd.Parent.Checked = ch;
                }

                List<ARINC_OBJECT> ElementList = (from element in ListOfObjects
                                                  where (element != null) &&
                                                        (element.CustomerArea == SelAreaCode) &&
                                                        (element.Object_Type == SelObjType)
                                                  select element).ToList();

                dataGridView1.DataSource = ElementList;
            }
            else
            {
                // if (e.Node.Checked)
                {
                    foreach (TreeNode nd in Nd.Nodes) nd.Checked = Nd.Checked;
                }
            }
        }

        private void showDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<ARINC_OBJECT> ElementList = (from element in ListOfObjects
                                                    where (element != null) &&
                                                          (element.CustomerArea ==SelAreaCode)&&
                                                          (element.Object_Type == SelObjType)
                                                    select element).ToList();

            dataGridView1.DataSource = ElementList;
        }

        private void contextMenuStrip2_Click(object sender, EventArgs e)
        {
            ARINC_OBJECT Element = (from element in ListOfObjects
                         where (element != null) &&
                            (element.CustomerArea == SelAreaCode) &&
                            (element.Object_Type == SelObjType) &&
                            (element.ID.CompareTo(dataGridView1.SelectedRows[0].Cells["ID"].Value.ToString()) == 0)
                         select element).First();


            propertyGrid1.SelectedObject = Element;

        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (treeView3.Nodes[3].Checked) return;

            if (dataGridView1.SelectedRows.Count <= 0) return;

            ARINC_OBJECT Element = (from element in ListOfObjects
                                    where (element != null) &&
                                       (element.CustomerArea == SelAreaCode) &&
                                       (element.Object_Type == SelObjType) &&
                                       (element.ID.CompareTo(dataGridView1.SelectedRows[0].Cells["ID"].Value.ToString()) == 0)
                                    select element).First();


            propertyGrid1.SelectedObject = Element;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Static_Proc.CloseProgressBar();


            if ((treeView1.Nodes.Count>0) && (MessageBox.Show("Exit without saving results?", "Save", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.Yes))
            {
                Close();
            }

            if (treeView1.Nodes.Count == 0) Close();
        }

        private void DecodeOBSTCLE_File(string FilePath)
        {

            Static_Proc.ShowProgressBar = !System.Diagnostics.Debugger.IsAttached;

            DateTime start = DateTime.Now;

           // string[] obsList = System.IO.File.ReadAllLines(FilePath);

            List<string> result = new List<string>();
            //string[] columns = obsList[0].Split((char)9);

            //Dictionary<string, List<string>> dic = Static_Proc.DecodeJeppesenOBSTCLFile(obsList);
            Dictionary<string, List<string>> dic = Static_Proc.DecodeJeppesenOBSTCLFile(FilePath);
            
            treeView1.Nodes.Clear();

            if (dic.ContainsKey("COLUMNS_HEADER"))
            {
                string[] columns = dic["COLUMNS_HEADER"].ToArray();

                foreach (KeyValuePair<string, List<string>> pair in dic.OrderBy(key => key.Key))
                {
                    if (pair.Key.StartsWith("COLUMNS_HEADER")) continue;
                    result = pair.Value;

                    result.Insert(0, columns[0] + (char)9 + columns[1] + (char)9 + columns[2] + (char)9 + columns[3] + (char)9 + columns[4] + (char)9 + columns[5] + (char)9 + columns[6] + (char)9 + columns[7] + (char)9 +
                        columns[9] + (char)9 + columns[10] + (char)9 + columns[12] + (char)9 + columns[23] + (char)9 + columns[24] + (char)9);
 
                    TreeNode nd = new TreeNode(pair.Key);
                    nd.Tag = result;
                    treeView1.Nodes.Add(nd);

                }
            }

        }



        private void selectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (TreeNode nd in treeView1.Nodes) nd.Checked = true;
        }

        private void changeSelectionStateToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            foreach (TreeNode nd in treeView1.Nodes) nd.Checked = !nd.Checked;
        }

        private void uncheckAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (TreeNode nd in treeView1.Nodes) nd.Checked = false;

        }

        private void comboBox1_TextChanged(object sender, EventArgs e)
        {
            Dictionary<string, List<string>> icao_codes = (Dictionary<string, List<string>>)comboBox1.Tag;
            comboBox3.Items.Clear();
            comboBox3.Text = "";
            if (icao_codes.ContainsKey(comboBox1.Text))
            {
                comboBox3.Items.AddRange(((List<string>)icao_codes[comboBox1.Text]).ToArray());
            }

            if (comboBox3.Items.Count>0) comboBox3.SelectedIndex = 0;

        }

        private void treeView2_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            
            if (e.Node == null) return;
            if (e.Node.Tag == null)
            {
                propertyGrid2.SelectedObject = null;
                return;
            }
            propertyGrid2.SelectedObject = e.Node.Tag;

            listView1.Items.Clear();
            if (e.Node.Tag is ProcedureBranch_AIRTRACK)
            {

                //    listView1.Visible = (((Procedure_AIRTRACK)e.Node.Tag).DebugMessages!=null) && ((Procedure_AIRTRACK)e.Node.Tag).DebugMessages.Count > 0;

                //}
                //else
                //    listView1.Visible = false;

                if ((((ProcedureBranch_AIRTRACK)e.Node.Tag).DebugMessages != null) && (((ProcedureBranch_AIRTRACK)e.Node.Tag).DebugMessages.Count > 0))
                {
                    if (((ProcedureBranch_AIRTRACK)e.Node.Tag).DebugMessages.Count > 0)
                    {
                        int n = 1;
                        foreach (string message in ((ProcedureBranch_AIRTRACK)e.Node.Tag).DebugMessages)
                        {
                            ListViewItem itm = new ListViewItem(n.ToString(), 0);
                            itm.SubItems.Add(message);
                            listView1.Items.AddRange(new ListViewItem[] { itm });
                            n++;
                        }
                    }
                }
            }
           

        }

        private void button4_Click(object sender, EventArgs e)
        {


            WayPoint_AIRTRACK wyp= new WayPoint_AIRTRACK();

            Fix_type ft = (Fix_type)Enum.Parse(typeof(Fix_type), "A");


            //object objVal = new ARINC_Navaid_VHF_Primary_Record();

            //string tblName = "ARINC_Navaid_VHF";
            //string cmd = "CREATE TABLE " + tblName + " (OBJECTID INTEGER NOT NULL, ";


            //    PropertyInfo[] propInfoArr = objVal.GetType().GetProperties();
            //    if (propInfoArr == null) return;
            //    foreach (PropertyInfo prop in propInfoArr)
            //    {
            //        cmd = cmd + prop.Name + " TEXT(50),";


            //    }

            
            ////System.Diagnostics.Debug.WriteLine(cmd);

        }



        private void treeView3_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
           

            bool f = true;
            if (e.Node.GetNodeCount(true) > 0)
            {
                foreach (TreeNode nd in e.Node.Nodes) nd.Checked = e.Node.Checked;
            }

            if (e.Node.Parent != null)
            {
                
                foreach (TreeNode nd in e.Node.Parent.Nodes)
                    f = nd.Checked && f;

                e.Node.Parent.Checked = f;
            }

            int indx = e.Node.Index;
            f = e.Node.Checked;

            if (e.Node.Index !=3) treeView3.Nodes[3].Checked = false;

            Next_button.Enabled = SetEnabledState();

        }

        private bool SetEnabledState()
        {

            bool res = false;

            foreach (TreeNode Nd in treeView3.Nodes)
            {
                if (Nd.Checked)
                {
                    res = true;
                    break;
                }


                if (Nd.Nodes.Count > 0)
                {
                    foreach (TreeNode NdTags in Nd.Nodes)
                    {
                        if (NdTags.Checked) 
                        {
                            res = true;
                            break;
                        }
                    }
                }
            }

            return res && textBox1.Text.Trim().Length>0;
        }
        
        private void treeView3_AfterCheck(object sender, TreeViewEventArgs e)
        {
            if ((e.Node.Index == 4) && (e.Node.Checked))
            {
                treeView3.Nodes[0].Checked = false;
                    foreach (TreeNode nd in treeView3.Nodes[0].Nodes) nd.Checked = false;
                treeView3.Nodes[1].Checked = false;
                    foreach (TreeNode nd in treeView3.Nodes[1].Nodes) nd.Checked = false;
                treeView3.Nodes[2].Checked = false;
                    foreach (TreeNode nd in treeView3.Nodes[2].Nodes) nd.Checked = false;
                treeView3.Nodes[3].Checked = false;
                    foreach (TreeNode nd in treeView3.Nodes[3].Nodes) nd.Checked = false;

            }
            

        }

        private void treeView3_AfterCollapse(object sender, TreeViewEventArgs e)
        {

        }

        private void treeView3_AfterExpand(object sender, TreeViewEventArgs e)
        {

        }

        private void treeView3_AfterSelect(object sender, TreeViewEventArgs e)
        {
            
        }

        private void treeView3_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

       












    }
}
