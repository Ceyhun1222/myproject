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
using ESRI.ArcGIS.Catalog;

namespace SigmaChart.CmdsMenu
{
    public partial class TerminalChartUpdateForm : Form
    {

    
        public TerminalChartUpdateForm()
        {
        }

        public TerminalChartUpdateForm(List<PDMObject> _List, string _mapPath, string _pdmProjectFolder)
        {
            ESRI.ArcGIS.RuntimeManager.BindLicense(ESRI.ArcGIS.ProductCode.Desktop);

            InitializeComponent();


            treeView_oldObjLst.Tag = new Dictionary<string, TreeNode>();
            treeView_newObjLst.Tag = new Dictionary<string, TreeNode>();

            try
            {

                FillObjectsTree(treeView_newObjLst);
                FillObjectsTree(treeView_oldObjLst, _List);

                double mapSize_Height;
                double mapSize_Width;
                ListBox listBox1 = new ListBox();
                ChartsHelperClass.SelectChartTemplate(ref axPageLayoutControl1, _mapPath, ref listBox1, out mapSize_Height, out mapSize_Width);
                ChartsHelperClass.SetMapGridVisibilityState(axPageLayoutControl1.ActiveView, false);


                AddLayerFromFile(axPageLayoutControl1.ActiveView, System.IO.Path.Combine(System.IO.Path.GetDirectoryName(_mapPath), "ARENA.lyr" ), visibleFlag: true);
                AddLayerFromFile(axPageLayoutControl1.ActiveView, System.IO.Path.Combine(_pdmProjectFolder, "ARENA.lyr"), "ARENA_PDM", true);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);


            }

        }


        private void TerminalChartUpdateForm_Load(object sender, EventArgs e)
        {
            axToolbarControl1.SetBuddyControl(axPageLayoutControl1);
            axToolbarControl2.SetBuddyControl(axPageLayoutControl1);

        }

        private void FillObjectsTree(TreeView _treeView,List<PDMObject> _sourceList = null,  bool SortFlag = true)
        {
            List<PDMObject> _dataList = _sourceList == null ? DataCash.ProjectEnvironment.Data.PdmObjectList : _sourceList;

            _treeView.BeginUpdate();
            _treeView.Nodes.Clear();

            DataCash.ProjectEnvironment.Data.LayersSymbolInImageList = new Dictionary<string, int>();
            var ilsNode = new TreeNode("ILS") { Name = "ILS", Checked = false };


            #region ARP/RWY/RDN/ILS


            var arpList = (from element in _dataList where (element != null) && (element is AirportHeliport) select element).ToList();
            var adhpRwyNode = new TreeNode("ARP ") { Name = "ARP", Checked = false };

            if (SortFlag) arpList = arpList.OrderBy(x => x.GetObjectLabel()).ThenBy(x => x.GetObjectLabel()).ToList();


            foreach (AirportHeliport arp in arpList)
            {


                var arpNd = CreatArenaTreeNode(arp, _treeView);
                arpNd.Name = arp.Designator;

                #region MyRegion

                //if (arp.RunwayList != null)
                //{
                //    foreach (var rwy in arp.RunwayList)
                //    {
                //        var rwyNd = CreatArenaTreeNode(rwy, _treeView);
                //        rwyNd.Name = rwy.Designator;

                //        if (rwy.RunwayDirectionList != null)
                //        {
                //            foreach (var thr in rwy.RunwayDirectionList)
                //            {
                //                var rdnNode = CreatArenaTreeNode(thr, _treeView);
                //                rdnNode.Name = thr.Designator;

                //                if (thr.Related_NavaidSystem != null)
                //                {
                //                    foreach (NavaidSystem ns in thr.Related_NavaidSystem)
                //                    {
                //                        var navNode = CreatArenaTreeNode(ns, _treeView);
                //                        navNode.Name = ns.Designator;

                //                        if (ns.Components != null)
                //                        {
                //                            foreach (var comp in ns.Components)
                //                            {
                //                                var compNode = CreatArenaTreeNode(comp, _treeView);

                //                                navNode.Nodes.Add(compNode);
                //                            }
                //                        }

                //                        rdnNode.Nodes.Add(navNode);

                //                        ilsNode.Nodes.Add((TreeNode)navNode.Clone());

                //                    }

                //                }

                //                if (thr.CenterLinePoints != null)
                //                {

                //                    int i = 0;
                //                    foreach (RunwayCenterLinePoint clp in thr.CenterLinePoints)
                //                    {
                //                        i++;
                //                        var clpNode = CreatArenaTreeNode(clp, "clp" + i.ToString() + " " + clp.Role.ToString(), _treeView);
                //                        clpNode.Name = clp.Designator;
                //                        if (clpNode != null) rdnNode.Nodes.Add(clpNode);



                //                    }


                //                }

                //                rwyNd.Nodes.Add(rdnNode);
                //            }
                //        }
                //        arpNd.Nodes.Add(rwyNd);
                //    }
                //}

                #endregion


                if (adhpRwyNode.Nodes.Find(arpNd.Name, false).Length <=0)
                    adhpRwyNode.Nodes.Add(arpNd);
            }

            #endregion

            #region Navaids

            var vorDmeNode = new TreeNode("VOR/DME") { Name = "VOR/DME", Checked = false };
            var vorTacanNode = new TreeNode("VOR/TACAN") { Name = "VOR/TACAN", Checked = false };
            var vorNode = new TreeNode("VOR") { Name = "VOR", Checked = false };
            var dmeNode = new TreeNode("DME") { Name = "DME", Checked = false };
            var tacanNode = new TreeNode("TACAN") { Name = "TACAN", Checked = false };
            var ndbNode = new TreeNode("NDB") { Name = "NDB", Checked = true };
            var ndbMarkerNode = new TreeNode("NDB/Marker") { Name = "NDB/Marker", Checked = false };
            var markerNode = new TreeNode("Marker") { Name = "Marker", Checked = false };





            var navList = (from element in _dataList where (element != null) && (element is NavaidSystem) select element).ToList();
            var navaidsNode = new TreeNode("Navaids ") { Name = "Navaids", Checked = false };

            if (SortFlag) navList = navList.OrderBy(x => x.GetObjectLabel()).ThenBy(x => x.GetObjectLabel()).ToList();


            foreach (NavaidSystem ns in navList)
            {

                var nsNode = CreatArenaTreeNode(ns, _treeView);

                //if (ns.Components != null)
                //{
                //    //var nsNode = CreatArenaTreeNode(ns);
                //    nsNode.Name = ns.Designator;

                //    if (ns.Components.Count >= 1)
                //    {
                //        foreach (PDMObject comp in ns.Components)
                //        {
                //            var compNode = CreatArenaTreeNode(comp, _treeView);

                //            nsNode.Nodes.Add(compNode);
                //        }
                //    }

                //    if (ns.CodeNavaidSystemType == NavaidSystemType.VOR_DME) vorDmeNode.Nodes.Add(nsNode);
                //    if (ns.CodeNavaidSystemType == NavaidSystemType.VORTAC) vorTacanNode.Nodes.Add(nsNode);
                //    if (ns.CodeNavaidSystemType == NavaidSystemType.VOR) vorNode.Nodes.Add(nsNode);
                //    if (ns.CodeNavaidSystemType == NavaidSystemType.DME) dmeNode.Nodes.Add(nsNode);
                //    if (ns.CodeNavaidSystemType == NavaidSystemType.TACAN) tacanNode.Nodes.Add(nsNode);
                //    if (ns.CodeNavaidSystemType == NavaidSystemType.NDB) ndbNode.Nodes.Add(nsNode);
                //    if (ns.CodeNavaidSystemType == NavaidSystemType.NDB_MKR) ndbMarkerNode.Nodes.Add(nsNode);
                //    //if (ns.CodeNavaidSystemType == NavaidSystemType.ILS_DME) ilsDMENode.Nodes.Add(nsNode);
                //    if (ns.CodeNavaidSystemType == NavaidSystemType.ILS)
                //        ilsNode.Nodes.Add(nsNode);
                //    if (ns.CodeNavaidSystemType == NavaidSystemType.MKR) markerNode.Nodes.Add(nsNode);
                //}
            }

            if (vorDmeNode.Nodes.Count > 0) navaidsNode.Nodes.Add(vorDmeNode);
            if (vorTacanNode.Nodes.Count > 0) navaidsNode.Nodes.Add(vorTacanNode);
            if (vorNode.Nodes.Count > 0) navaidsNode.Nodes.Add(vorNode);
            if (dmeNode.Nodes.Count > 0) navaidsNode.Nodes.Add(dmeNode);
            if (tacanNode.Nodes.Count > 0) navaidsNode.Nodes.Add(tacanNode);
            if (ndbNode.Nodes.Count > 0) navaidsNode.Nodes.Add(ndbNode);
            //if (ilsDMENode.Nodes.Count > 0) navaidsNode.Nodes.Add(ilsDMENode);
            if (ilsNode.Nodes.Count > 0) navaidsNode.Nodes.Add(ilsNode);
            if (markerNode.Nodes.Count > 0) navaidsNode.Nodes.Add(markerNode);
            if (ndbMarkerNode.Nodes.Count > 0) navaidsNode.Nodes.Add(ndbMarkerNode);

            navaidsNode.Text = navaidsNode.Text + "(" + (navList.Count + ilsNode.Nodes.Count).ToString() + ")";

            #endregion

            #region WayPoints


            var wypList = (from element in _dataList where (element != null) && (element is WayPoint) select element).ToList();
            var wayPointNode = new TreeNode("WayPoints ") { Name = "WayPoints", Checked = false };

            if (SortFlag) wypList = wypList.OrderBy(x => x.GetObjectLabel()).ThenBy(x => x.GetObjectLabel()).ToList();


            foreach (WayPoint wyp in wypList)
            {
                var wypNd = CreatArenaTreeNode(wyp, _treeView);
                wypNd.Name = wyp.Designator;
                wayPointNode.Nodes.Add(wypNd);

            }

            #endregion

            #region Procedures

            var iapNode = new TreeNode(PROC_TYPE_code.Approach.ToString()) { Name = "Approach", Checked = false };
            var sidNode = new TreeNode(PROC_TYPE_code.SID.ToString()) { Name = "SID", Checked = false };
            var starNode = new TreeNode(PROC_TYPE_code.STAR.ToString()) { Name = "STAR", Checked = false }; ;
            var multipleNode = new TreeNode(PROC_TYPE_code.Multiple.ToString()) { Name = "Multiple", Checked = false }; ;

            var procList = (from element in _dataList where (element != null) && (element is Procedure) select element).ToList();
            var proceduresNode = new TreeNode("Procedures ") { Name = "Procedures", Checked = false };
            var procHldngList = new TreeNode();
            if (SortFlag) procList = procList.OrderBy(x => x.GetObjectLabel()).ThenBy(x => x.GetObjectLabel()).ToList();
            List<string> hldsId = new List<string>();

            foreach (Procedure prc in procList)
            {
                var procNd = CreatArenaTreeNode(prc, prc.Airport_ICAO_Code + " " + prc.GetObjectLabel(), _treeView);
                procNd.Name = prc.ProcedureIdentifier;
                procNd.ImageIndex = 0;


                #region MyRegion
                
                //if (prc.Transitions != null)
                //{
                //    foreach (var trans in prc.Transitions)
                //    {
                //        var transNode = CreatArenaTreeNode(trans, trans.RouteType.ToString(), _treeView);
                //        transNode.Name = trans.FeatureGUID;
                //        transNode.ImageIndex = 0;

                //        if (trans.Legs != null)
                //        {
                //            foreach (var leg in trans.Legs)
                //            {
                //                var legNode = (CreatArenaTreeNode(leg, leg.GetObjectLabel() + " (" + leg.LegSpecialization.ToString() + ")", _treeView));
                //                legNode.Name = leg.ID;
                //                legNode.ImageIndex = 0;

                //                if (leg.StartPoint != null)
                //                {
                //                    var sPointNode = CreateSignificantPointNode(leg.StartPoint, _treeView);
                //                    if (sPointNode != null) legNode.Nodes.Add(sPointNode);
                //                }
                //                if (leg.ArcCentre != null)
                //                {
                //                    var sPointNode = CreateSignificantPointNode(leg.ArcCentre, _treeView);
                //                    if (sPointNode != null) legNode.Nodes.Add(sPointNode);
                //                }
                //                if (leg.EndPoint != null)
                //                {
                //                    var sPointNode = CreateSignificantPointNode(leg.EndPoint, _treeView);
                //                    if (sPointNode != null) legNode.Nodes.Add(sPointNode);
                //                }

                //                if (leg.HoldingUse != null && hldsId.IndexOf(leg.HoldingUse.ID) < 0)
                //                {
                //                    System.Diagnostics.Debug.WriteLine(prc.GetObjectLabel());
                //                    var hldNd = CreatArenaTreeNode(leg.HoldingUse, _treeView);
                //                    if (hldNd != null)
                //                    {
                //                        hldNd.Name = leg.HoldingUse.HoldingPoint != null ? leg.HoldingUse.HoldingPoint.SegmentPointDesignator : "Holding";

                //                        hldNd.Text = hldNd.Text;

                //                        legNode.Nodes.Add(hldNd);
                //                        procHldngList.Nodes.Add((TreeNode)hldNd.Clone());
                //                        hldsId.Add(leg.HoldingUse.ID);
                //                    }
                //                }

                //                if (leg.AngleIndication != null)
                //                {
                //                    string angleIndNodeText = GenerateDistAngleNodeText(leg.AngleIndication.FixID, leg.AngleIndication.SignificantPointID);
                //                    TreeNode angleIndNode = CreatArenaTreeNode(leg.AngleIndication, "Angle Indication " + angleIndNodeText, _treeView);
                //                    if (angleIndNode != null) legNode.Nodes.Add(angleIndNode);
                //                }

                //                if (leg.DistanceIndication != null)
                //                {
                //                    string distIndNodeText = GenerateDistAngleNodeText(leg.DistanceIndication.FixID, leg.DistanceIndication.SignificantPointID);
                //                    TreeNode distIndNode = CreatArenaTreeNode(leg.DistanceIndication, "Distance Indication " + distIndNodeText, _treeView);
                //                    if (distIndNode != null) legNode.Nodes.Add(distIndNode);
                //                }

                //                transNode.Nodes.Add(legNode);
                //                transNode.ImageIndex = 0;

                //            }
                //        }
                //        procNd.Nodes.Add(transNode);
                //    }
                //}

                #endregion

                if (prc.ProcedureType == PROC_TYPE_code.Approach) iapNode.Nodes.Add(procNd);
                if (prc.ProcedureType == PROC_TYPE_code.Multiple) multipleNode.Nodes.Add(procNd);
                if (prc.ProcedureType == PROC_TYPE_code.SID) sidNode.Nodes.Add(procNd);
                if (prc.ProcedureType == PROC_TYPE_code.STAR) starNode.Nodes.Add(procNd);
            }

            if (iapNode.Nodes.Count > 0) proceduresNode.Nodes.Add(iapNode);
            if (sidNode.Nodes.Count > 0) proceduresNode.Nodes.Add(sidNode);
            if (starNode.Nodes.Count > 0) proceduresNode.Nodes.Add(starNode);
            if (multipleNode.Nodes.Count > 0) proceduresNode.Nodes.Add(multipleNode);


            #endregion
  
            #region Airspaces

            var airspacesList = (from element in _dataList where (element != null) && (element is Airspace) select element).ToList();
            var airspacesNode = new TreeNode("Airspaces " ) { Name = "Airspaces", Checked = false };


            if (SortFlag) airspacesList = airspacesList.OrderBy(x => x.GetObjectLabel()).ThenBy(x => x.GetObjectLabel()).ToList();



            foreach (Airspace arsps in airspacesList)
            {
                var arspsNd = CreatArenaTreeNode(arsps, (arsps.TxtName != null && arsps.TxtName.Length > 0) ? arsps.TxtName : arsps.CodeID, _treeView);
                arspsNd.Name = arsps.CodeID;

                //if (arsps.AirspaceVolumeList != null)
                //{
                //    foreach (var vol in arsps.AirspaceVolumeList)
                //    {
                //        var arspsVolNd = CreatArenaTreeNode(vol, vol.TxtName, _treeView);
                //        arspsVolNd.Name = vol.TxtName;

                //        arspsNd.Nodes.Add(arspsVolNd);
                //    }
                //}

                //if (arsps.ProtectedRoute != null)
                //{
                //    var arspsrt = CreatArenaTreeNode(arsps.ProtectedRoute, "protected Route " + arsps.ProtectedRoute.TxtDesig, _treeView);
                //    arspsrt.Name = arsps.ProtectedRoute.TxtDesig;

                //    arspsNd.Nodes.Add(arspsrt);

                //}
                airspacesNode.Nodes.Add(arspsNd);
            }

            #endregion

            #region Obstacles

            var obstaclesList = (from element in _dataList where (element != null) && (element is VerticalStructure) select element).ToList();
            var verticalStrNode = new TreeNode("VerticalStructure ") { Name = "VerticalStructure", Checked = false };

            if (SortFlag) obstaclesList = obstaclesList.OrderBy(x => x.GetObjectLabel()).ThenBy(x => x.GetObjectLabel()).ToList();


            foreach (VerticalStructure obs in obstaclesList)
            {

                System.Diagnostics.Debug.WriteLine(obs.ID);
                var obsNd = CreatArenaTreeNode(obs, obs.Name, _treeView);
                obsNd.Name = obs.Name;
                //foreach (var prt in obs.Parts)
                //{

                //    var partNd = CreatArenaTreeNode(prt, prt.Designator, _treeView);
                //    partNd.Name = prt.Designator;
                //    obsNd.Nodes.Add(partNd);
                //}

                verticalStrNode.Nodes.Add(obsNd);
            }

            #endregion

            #region MSA

            var msaList = (from element in _dataList where (element != null) && (element is SafeAltitudeArea) select element).ToList();
            var msaNode = new TreeNode("Safe Area ") { Name = "SafeArea", Checked = false };


            if (SortFlag) msaList = msaList.OrderBy(x => x.GetObjectLabel()).ThenBy(x => x.GetObjectLabel()).ToList();



            foreach (SafeAltitudeArea msa in msaList)
            {
                var parentNd = CreatArenaTreeNode(msa, "Safe Area", _treeView);
                parentNd.Name = msa.ID;

                //if (msa.SafeAltitudeAreaSector != null)
                //{
                //    foreach (var sector in msa.SafeAltitudeAreaSector)
                //    {
                //        var childNd = CreatArenaTreeNode(sector, "Sector", _treeView);
                //        childNd.Name = sector.ID;

                //        parentNd.Nodes.Add(childNd);
                //    }
                //}


                msaNode.Nodes.Add(parentNd);
            }

            #endregion
     
            #region Holding


            var hldngList = (from element in _dataList where (element != null) && (element is HoldingPattern) select element).ToList();
            var hlngNode = new TreeNode("HoldingPattern " ) { Name = "Holding", Checked = false };

            if (SortFlag) hldngList = hldngList.OrderBy(x => x.GetObjectLabel()).ThenBy(x => x.GetObjectLabel()).ToList();


            foreach (HoldingPattern hld in hldngList)
            {
                if (hldsId.IndexOf(hld.ID) < 0)
                {
                    var hldNd = CreatArenaTreeNode(hld, _treeView);
                    hldNd.Name = hld.HoldingPoint != null ? hld.HoldingPoint.SegmentPointDesignator : "Holding";

                    hldNd.Text = hldNd.Text;

                    hlngNode.Nodes.Add(hldNd);
                    hldsId.Add(hld.ID);
                }
            }



            TreeNode[] myTreeNodeArray = new TreeNode[procHldngList.Nodes.Count];
            procHldngList.Nodes.CopyTo(myTreeNodeArray, 0);
            hlngNode.Nodes.AddRange(myTreeNodeArray);
            hlngNode.Text = "HoldingPattern ";

            #endregion


            if (adhpRwyNode.Nodes.Count > 0) _treeView.Nodes.Add(adhpRwyNode);
            if (navaidsNode.Nodes.Count > 0) _treeView.Nodes.Add(navaidsNode);
            if (wayPointNode.Nodes.Count > 0) _treeView.Nodes.Add(wayPointNode);
            if (proceduresNode.Nodes.Count > 0) _treeView.Nodes.Add(proceduresNode);
            if (airspacesNode.Nodes.Count > 0) _treeView.Nodes.Add(airspacesNode);
            if (verticalStrNode.Nodes.Count > 0) _treeView.Nodes.Add(verticalStrNode);
            if (msaNode.Nodes.Count > 0) _treeView.Nodes.Add(msaNode);
            if (hlngNode.Nodes.Count > 0) _treeView.Nodes.Add(hlngNode);



            _treeView.EndUpdate();

        }


        private TreeNode CreatArenaTreeNode(PDMObject pdmObj, string NdText, TreeView _local_treeView)
        {
            TreeNode Nd = new TreeNode(NdText) { };

            if (!pdmObj.CreatedAutomatically) Nd.ForeColor = Color.FromName("DarkCyan");

            //if (pdmObj.Geo == null) pdmObj.RebuildGeo();
            //if (pdmObj.Geo == null) Nd.NodeFont = new System.Drawing.Font(FeatureTreeView.Font, FontStyle.Italic);
            //else Nd.NodeFont = new System.Drawing.Font(FeatureTreeView.Font, FontStyle.Regular);
            if (pdmObj.ID == null) pdmObj.ID = Guid.NewGuid().ToString();

            Nd.Tag = pdmObj;
            //Nd.ImageIndex = DataCash.ProjectEnvironment.Data.GetObjectImageIndex(pdmObj);



            Dictionary<string, TreeNode> PdmObject_TreeNode = (_local_treeView.Tag as Dictionary<string, TreeNode>);
            if (!PdmObject_TreeNode.ContainsKey(pdmObj.ID)) PdmObject_TreeNode.Add(pdmObj.ID, Nd);
            return Nd;
        }

        private TreeNode CreatArenaTreeNode(PDMObject pdmObj, TreeView _local_treeView)
        {
            TreeNode Nd = new TreeNode(pdmObj.GetObjectLabel()) { };
            if (!pdmObj.CreatedAutomatically) Nd.ForeColor = Color.FromName("DarkCyan");

            if (pdmObj.ID == null) pdmObj.ID = Guid.NewGuid().ToString();

            Nd.Tag = pdmObj;
            //Nd.ImageIndex = DataCash.ProjectEnvironment.Data.GetObjectImageIndex(pdmObj);

            Dictionary<string, TreeNode> PdmObject_TreeNode = (_local_treeView.Tag as Dictionary<string, TreeNode>);
            if (!PdmObject_TreeNode.ContainsKey(pdmObj.ID)) PdmObject_TreeNode.Add(pdmObj.ID, Nd);
            return Nd;

        }

        private TreeNode CreateSignificantPointNode(SegmentPoint segmentPoint, TreeView _treeView)
        {
            TreeNode res = null;
            try
            {
                res = CreatArenaTreeNode(segmentPoint, segmentPoint.PointUse.ToString() + " " + segmentPoint.SegmentPointDesignator, _treeView);

                if (segmentPoint.PointFacilityMakeUp != null)
                {
                    TreeNode fMkUpNode = CreatArenaTreeNode(segmentPoint.PointFacilityMakeUp, "Facility MakeUp " + segmentPoint.PointFacilityMakeUp.Role.ToString(), _treeView);
                    if (fMkUpNode != null)
                    {
                        if (segmentPoint.PointFacilityMakeUp.AngleIndication != null)
                        {
                            string angleIndNodeText = GenerateDistAngleNodeText(segmentPoint.PointFacilityMakeUp.AngleIndication.FixID, segmentPoint.PointFacilityMakeUp.AngleIndication.SignificantPointID);
                            TreeNode angleIndNode = CreatArenaTreeNode(segmentPoint.PointFacilityMakeUp.AngleIndication, "Angle Indication " + angleIndNodeText, _treeView);
                            if (angleIndNode != null) fMkUpNode.Nodes.Add(angleIndNode);
                        }

                        if (segmentPoint.PointFacilityMakeUp.DistanceIndication != null)
                        {
                            string distIndNodeText = GenerateDistAngleNodeText(segmentPoint.PointFacilityMakeUp.DistanceIndication.FixID, segmentPoint.PointFacilityMakeUp.DistanceIndication.SignificantPointID);
                            TreeNode distIndNode = CreatArenaTreeNode(segmentPoint.PointFacilityMakeUp.DistanceIndication, "Distance Indication " + distIndNodeText, _treeView);
                            if (distIndNode != null) fMkUpNode.Nodes.Add(distIndNode);
                        }

                        res.Nodes.Add(fMkUpNode);
                    }
                }

            }
            catch
            {
                res = null;
            }
            return res;
        }

        private string GenerateDistAngleNodeText(string FixID, string SignificantPointID)
        {
            string res = "";
            if ((FixID != null) && (FixID.Length > 0))
            {
                var fix = (from element in DataCash.ProjectEnvironment.Data.PdmObjectList where (element != null) && (element is WayPoint) && (((WayPoint)element).ID.CompareTo(FixID) == 0) select element).FirstOrDefault();
                if (fix != null) res = " Fix:" + ((WayPoint)fix).Designator;
            }

            if ((SignificantPointID != null) && (SignificantPointID.Length > 0))
            {
                var nav = (from element in DataCash.ProjectEnvironment.Data.PdmObjectList where (element != null) && (element is NavaidSystem) && (((NavaidSystem)element).ID.CompareTo(SignificantPointID) == 0) select element).FirstOrDefault();
                if (nav != null) res = res + " Nav:" + ((NavaidSystem)nav).Designator;
                else
                {
                    bool flag = false;
                    var arpList = (from element in DataCash.ProjectEnvironment.Data.PdmObjectList where (element != null) && (element is AirportHeliport) && (((AirportHeliport)element).RunwayList != null) select element).ToList();
                    foreach (AirportHeliport arp in arpList)
                    {
                        if (flag) break;
                        foreach (Runway rwy in arp.RunwayList)
                        {
                            if (rwy.RunwayDirectionList == null) continue;
                            foreach (RunwayDirection rdn in rwy.RunwayDirectionList)
                            {
                                if (rdn.Related_NavaidSystem == null) continue;
                                var ILS_NAV = (from element in rdn.Related_NavaidSystem where (element != null) && (element is NavaidSystem) && (((NavaidSystem)element).ID.CompareTo(SignificantPointID) == 0) select element).FirstOrDefault();
                                if (ILS_NAV != null)
                                {
                                    res = res + " Nav:" + ((NavaidSystem)ILS_NAV).Designator;
                                    flag = true;
                                    break;
                                }

                            }
                        }
                    }
                }
            }

            return res;
        }

        private void button4_Click(object sender, EventArgs e)
        {

        }

        public void AddLayerFromFile(ESRI.ArcGIS.Carto.IActiveView activeView, string layerPathFile, string layerName = "ARENA", bool visibleFlag = true)
        {

            //Create a new GxLayer.
            IGxLayer gxLayerCls = new ESRI.ArcGIS.Catalog.GxLayer();
            IGxFile gxFile = (IGxFile)gxLayerCls; //Explicit Cast.

            //Set the path for where the layer file is located on disk.
            gxFile.Path = layerPathFile;

            //Test if you have a valid layer and add it to the map.
            if (!(gxLayerCls.Layer == null))
            {
                ESRI.ArcGIS.Carto.IMap map = activeView.FocusMap;
                gxLayerCls.Layer.Visible = visibleFlag;
                gxLayerCls.Layer.Name = layerName;
                map.AddLayer(gxLayerCls.Layer);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (treeView_oldObjLst.SelectedNode == null || treeView_oldObjLst.SelectedNode.Tag == null) return;
            checkNode(treeView_oldObjLst.SelectedNode);

        }

        private void treeView_oldObjLst_AfterCheck(object sender, TreeViewEventArgs e)
        {
            //if (treeView_oldObjLst.SelectedNode == null || treeView_oldObjLst.SelectedNode.Tag == null) return;
            //checkNode(treeView_oldObjLst.SelectedNode);
        }

        private void checkNode(TreeNode _SelectedNode)
        {
            

        }
        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {

            try
            {
                if (treeView_oldObjLst.SelectedNode == null || treeView_oldObjLst.SelectedNode.Tag == null) return;

                PDMObject sellObj = (PDMObject)treeView_oldObjLst.SelectedNode.Tag;
                if (sellObj == null || sellObj.PDM_Type == PDM_ENUM.SafeAltitudeArea) return;

                ILayer _grLayer = EsriUtils.getLayerByName(axPageLayoutControl1.ActiveView.FocusMap, "ARENA");

                ILayer _Layer = null;
                if (sellObj is VerticalStructure)
                {
                    ((VerticalStructure)sellObj).Parts[0].RebuildGeo();
                    if (((VerticalStructure)sellObj).Parts[0].Geo != null)
                        _Layer = EsriUtils.getLayerByName((ICompositeLayer)_grLayer, DataCash.ProjectEnvironment.Data.GetLinkedLayer(((VerticalStructure)sellObj).Parts[0]).Name);
                }
                else if (sellObj is NavaidSystem)
                    _Layer = EsriUtils.getLayerByName((ICompositeLayer)_grLayer, DataCash.ProjectEnvironment.Data.GetLinkedLayer(((NavaidSystem)sellObj).Components[0]).Name);
                else
                    _Layer = EsriUtils.getLayerByName((ICompositeLayer)_grLayer, DataCash.ProjectEnvironment.Data.GetLinkedLayer(sellObj).Name);


                if (_Layer == null) return;

                string filter = "'" + sellObj.ID + "',";
                IFeatureLayerDefinition FD = (ESRI.ArcGIS.Carto.IFeatureLayerDefinition)_Layer;
                if (FD == null) return;


                _Layer.Visible = true;

                if (sellObj.PDM_Type == PDM_ENUM.Airspace)
                    foreach (var _item in ((Airspace)sellObj).AirspaceVolumeList)
                    {
                        filter = filter + "'" + _item.ID + "',";
                    }

                if (sellObj.PDM_Type == PDM_ENUM.VerticalStructure)
                    foreach (var _item in ((VerticalStructure)sellObj).Parts)
                    {
                        filter = filter + "'" + _item.ID + "',";
                    }

                if (sellObj.PDM_Type == PDM_ENUM.StandardInstrumentArrival || sellObj.PDM_Type == PDM_ENUM.StandardInstrumentDeparture || sellObj.PDM_Type == PDM_ENUM.InstrumentApproachProcedure)
                    foreach (var _item in ((Procedure)sellObj).Transitions)
                    {
                        foreach (var leg in _item.Legs)
                        {
                            filter = filter + "'" + leg.ID + "',";

                        }
                    }


                if (filter.Length > 0) FD.DefinitionExpression = "FeatureGUID IN (" + filter + "'0')";
                (axPageLayoutControl1.ActiveView.FocusMap as IActiveView).Extent = ChartsHelperClass.GetLayerExtent((IFeatureLayer2)_Layer, FD.DefinitionExpression);


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.StackTrace);
            }


            ((IActiveView)axPageLayoutControl1.ActiveView.FocusMap).Refresh();
        }

        private void treeView_oldObjLst_MouseClick(object sender, MouseEventArgs e)
        {
            if (treeView_oldObjLst.SelectedNode == null) return;
            var _SelectedNode = treeView_oldObjLst.SelectedNode;

            _SelectedNode.Checked = !_SelectedNode.Checked;

            if (_SelectedNode.Checked)
                _SelectedNode.NodeFont = new System.Drawing.Font(new FontFamily("Times New Roman"), 10, FontStyle.Strikeout);
            else
                _SelectedNode.NodeFont = new System.Drawing.Font(new FontFamily("Times New Roman"), 10, FontStyle.Regular);
        }
    }
}
