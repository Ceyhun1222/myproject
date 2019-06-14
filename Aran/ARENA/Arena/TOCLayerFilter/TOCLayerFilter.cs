// Copyright 2010 ESRI
// 
// All rights reserved under the copyright laws of the United States
// and applicable international laws, treaties, and conventions.
// 
// You may freely redistribute and use this sample code, with or
// without modification, provided you include the original copyright
// notice and use restrictions.
// 
// See the use restrictions at http://help.arcgis.com/en/sdk/10.0/usageRestrictions.htm
// 

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using ESRI.ArcGIS.ADF.CATIDs;
using ESRI.ArcGIS.ArcMapUI;
using ESRI.ArcGIS.Framework;
using ESRI.ArcGIS.Carto;
using System.Runtime.InteropServices;
using ESRI.ArcGIS.esriSystem;
using ArenaToolBox;
using PDM;
using ArenaStatic;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Geodatabase;
using EsriWorkEnvironment;
using AranSupport;
using ESRI.ArcGIS.Controls;
using ARENA.Util;
using System.Xml.Serialization;
using System.IO;
using System.Collections;
using ChartCodingTable;

namespace ARENA.TOCLayerView
{
    [Guid("aede0664-ef97-4c6c-94e6-b3c6216eedc5")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("TOCLayerFilterCS.TOCLayerFilter")]
    public partial class TOCLayerFilter : UserControl, IContentsView3
    {
        private IApplication m_application;
        //private bool m_visible;
        private object m_contextItem;
        private bool m_isProcessEvents;

        #region COM Registration Function(s)
        [ComRegisterFunction()]
        [ComVisible(false)]
        static void RegisterFunction(Type registerType)
        {
            // Required for ArcGIS Component Category Registrar support
            ArcGISCategoryRegistration(registerType);

            //
            // TODO: Add any COM registration code here
            //
        }

        [ComUnregisterFunction()]
        [ComVisible(false)]
        static void UnregisterFunction(Type registerType)
        {
            // Required for ArcGIS Component Category Registrar support
            ArcGISCategoryUnregistration(registerType);

            //
            // TODO: Add any COM unregistration code here
            //
        }

        #region ArcGIS Component Category Registrar generated code
        /// <summary>
        /// Required method for ArcGIS Component Category registration -
        /// Do not modify the contents of this method with the code editor.
        /// </summary>
        private static void ArcGISCategoryRegistration(Type registerType)
        {
            string regKey = string.Format("HKEY_CLASSES_ROOT\\CLSID\\{{{0}}}", registerType.GUID);
            ContentsViews.Register(regKey);

        }
        /// <summary>
        /// Required method for ArcGIS Component Category unregistration -
        /// Do not modify the contents of this method with the code editor.
        /// </summary>
        private static void ArcGISCategoryUnregistration(Type registerType)
        {
            string regKey = string.Format("HKEY_CLASSES_ROOT\\CLSID\\{{{0}}}", registerType.GUID);
            ContentsViews.Unregister(regKey);

        }

        #endregion
        #endregion
        public TOCLayerFilter()
        {
            InitializeComponent();
        }

        #region "IContentsView3 Implementations"

        public int Bitmap
        {
          get
          {
              //string bitmapResourceName = GetType().Name + ".bmp";
              //System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(GetType(), bitmapResourceName);
              //bmp.MakeTransparent(bmp.GetPixel(1, 1)); //alternatively use a .png with transparency
              //return bmp.GetHbitmap().ToInt32();
              System.Drawing.Bitmap bmp = global ::TOCLayerFilterCS.Properties.Resources.ArenaTOC;
              
              return bmp.GetHbitmap().ToInt32();
          }
        }

        public string Tooltip
        {
          get { return "Arena TOC"; }
        }

        //public bool Visible
        //{
        //    get { return m_visible; }
        //    set { m_visible = value; }
        //}
        string IContentsView3.Name { get { return "ArenaTOCContentsView"; } }
        public int hWnd { get { return this.Handle.ToInt32(); } }

        public object ContextItem //last right-clicked item
        {
            get { return m_contextItem; }
            set { }//Not implemented
        }

        public void Activate(int parentHWnd, IMxDocument Document)
        {
            
            if (m_application == null)
            {
                m_application = ((IDocument)Document).Parent;
                //if (cboLayerType.SelectedIndex < 0)
                //    cboLayerType.SelectedIndex = 0; //this should refresh the list automatically
                //else
                    RefreshList();

                SetUpDocumentEvent(Document as IDocument);
            }
        }

        public void BasicActivate(int parentHWnd, IDocument Document)
        {
        }
        
        public void Refresh(object item)
        {
            if (item != this)
            {
                //when items are added, removed, reordered
                FeatureTreeView.SuspendLayout();
                RefreshList();
                FeatureTreeView.ResumeLayout();
            }
        }
        public void Deactivate()
        {
            //Any clean up? 
        }

        public void AddToSelectedItems(object item) { }
        public object SelectedItem
        {
            get
            {
                //No Multiselect so return selected node
                if (FeatureTreeView.SelectedNode != null)
                    return FeatureTreeView.SelectedNode.Tag;   //Layer
                return null;
            }
            set
            {
                //Not implemented
            }
        }
        public bool ProcessEvents { set { m_isProcessEvents = value; } }
        public void RemoveFromSelectedItems(object item) { }
        public bool ShowLines
        {
            get { return FeatureTreeView.ShowLines; }
            set { FeatureTreeView.ShowLines = value; }
        }
        #endregion

        private void RefreshList()
        {
            
            if (m_application == null) return;

            FeatureTreeView.Tag = new Dictionary<string, TreeNode>();
            IMxDocument document = (IMxDocument)m_application.Document;
            IMaps maps = document.Maps;
            if (DataCash.ProjectEnvironment != null ) 
                FillObjectsTree(false);
            else
            {
                ClearObjectsTree();
            }
            
        }

        private void ClearObjectsTree()
        {
            FeatureTreeView.BeginUpdate();
            FeatureTreeView.Nodes.Clear();
            FeatureTreeView.EndUpdate();
        }

        //private void cboLayerType_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    FeatureTreeView.SuspendLayout();
        //    RefreshList();
        //    FeatureTreeView.ResumeLayout();
        //}
        ////private void tvwLayer_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        //{
        //    if (e.Button == MouseButtons.Right)
        //    {
        //        //Set item for context menu commands to work with
        //        m_contextItem = e.Node.Tag;

        //        //Show context menu
        //        UID menuID = new UIDClass();
        //        if (e.Node.Tag is IMap) //data frame
        //        {
        //            menuID.Value = "{F42891B5-2C92-11D2-AE07-080009EC732A}"; //Data Frame Context Menu (TOC) 
        //        }
        //        else //Layer - custom menu
        //        {
        //            menuID.Value = "{30cb4a78-6eba-4f60-b52e-38bc02bacc89}";
        //        }
        //        ICommandBar cmdBar = (ICommandBar)m_application.Document.CommandBars.Find(menuID, false, false);
        //        cmdBar.Popup(0, 0);
        //    }
        //}


        #region ARENA TOC Treeview

        private void FillObjectsTree(bool SortFlag)
        {

            comboBox1.Tag = SortFlag;

            FeatureTreeView.BeginUpdate();
            FeatureTreeView.Nodes.Clear();
            DataCash.ProjectEnvironment.TreeViewImageList = TreeViewImageList;

            DataCash.ProjectEnvironment.Data.LayersSymbolInImageList = new Dictionary<string, int>();
            var ilsNode = new TreeNode("ILS") { Name = "ILS", Checked = true };


            #region ARP/RWY/RDN/ILS


            var arpList = (from element in DataCash.ProjectEnvironment.Data.PdmObjectList where (element != null) && (element is AirportHeliport) select element).ToList();
            var adhpRwyNode = new TreeNode("ARP/RWY/RDN/ILS (" + arpList.Count.ToString()+ ")") { Name = "ARP/RWY/RDN/ILS", Checked = true };

            if (SortFlag)  arpList= arpList.OrderBy(x => x.GetObjectLabel()).ThenBy(x => x.GetObjectLabel()).ToList();

    
            foreach (AirportHeliport arp in arpList)
            {

                if (!DataCash.ProjectEnvironment.Data.AirdromeHeliportDictionary.ContainsKey(arp.ID)) DataCash.ProjectEnvironment.Data.AirdromeHeliportDictionary.Add(arp.ID, arp);

                var arpNd = CreatArenaTreeNode(arp);
                arpNd.Name = arp.Designator;


                if (arp.RunwayList != null)
                {
                    foreach (var rwy in arp.RunwayList)
                    {
                        var rwyNd = CreatArenaTreeNode(rwy);
                        rwyNd.Name = rwy.Designator;

                        if (rwy.RunwayDirectionList != null)
                        {
                            foreach (var thr in rwy.RunwayDirectionList)
                            {
                                var rdnNode = CreatArenaTreeNode(thr);
                                rdnNode.Name = thr.Designator;

                                if (thr.Related_NavaidSystem != null)
                                {
                                    foreach (NavaidSystem ns in thr.Related_NavaidSystem)
                                    {
                                        var navNode = CreatArenaTreeNode(ns);
                                        navNode.Name = ns.Designator;

                                        if (ns.Components != null)
                                        {
                                            foreach (var comp in ns.Components)
                                            {
                                                var compNode = CreatArenaTreeNode(comp);

                                                navNode.Nodes.Add(compNode);
                                            }
                                        }

                                        rdnNode.Nodes.Add(navNode);

                                        ilsNode.Nodes.Add((TreeNode)navNode.Clone());

                                    }

                                }

                                if (thr.CenterLinePoints != null)
                                {

                                    int i = 0;
                                    foreach (RunwayCenterLinePoint clp in thr.CenterLinePoints)
                                    {
                                        i++;
                                        var clpNode = CreatArenaTreeNode(clp, "clp" + i.ToString() + " " + clp.Role.ToString());
                                        clpNode.Name = clp.Designator;
                                        if (clpNode != null) rdnNode.Nodes.Add(clpNode);

                                        //if (clp.GuidanceLineList != null)
                                        //{
                                        //    foreach (GuidanceLine GdnLine in clp.GuidanceLineList)
                                        //    {
                                        //        var gdnNode = CreatArenaTreeNode(GdnLine, "GuidanceLine " + GdnLine.Designator.ToString());
                                        //        gdnNode.Name = GdnLine.Designator;

                                        //        if (GdnLine.LightSystem != null)
                                        //        {
                                        //            GuidanceLineLightSystem lghtSys = GdnLine.LightSystem;

                                        //            foreach (var lghtEl in lghtSys.Elements)
                                        //            {
                                        //                var lghtNode = CreatArenaTreeNode(lghtEl, "LightedElement " + lghtEl.LightedElement +" " + lghtEl.LightSourceType.ToString() + " " + lghtEl.Colour.ToString());
                                        //                if (lghtNode != null) gdnNode.Nodes.Add(lghtNode);
                                        //            }

                                        //        }

                                        //        if (GdnLine.GuidanceLineMarkingList != null)
                                        //        {
                                        //            foreach (var _el in GdnLine.GuidanceLineMarkingList)
                                        //            {
                                        //                var _Node = CreatArenaTreeNode(_el, "Marking " + _el.Condition.ToString());
                                        //                if (_Node != null) gdnNode.Nodes.Add(_Node);
                                        //            }

                                        //        }

                                        //        if (gdnNode != null) clpNode.Nodes.Add(gdnNode);

                                        //    }
                                        //}


                                    }
                                    

                                }

                                //if (thr.RwyProtectArea != null)
                                //{
                                //    foreach (var prtArea in thr.RwyProtectArea)
                                //    {
                                //        var prtNode = CreatArenaTreeNode(prtArea, "Protected Area " + prtArea.Type.ToString() +" "+ prtArea.Status.ToString());

                                //        if (prtArea.LightSystem!=null && prtArea.LightSystem.Elements!=null)
                                //        {
                                //            foreach (var lghtEl in prtArea.LightSystem.Elements)
                                //            {
                                //                var lghtNode = CreatArenaTreeNode(lghtEl, "LightedElement " + lghtEl.LightedElement + " " + lghtEl.LightSourceType.ToString() + " " + lghtEl.Colour.ToString());
                                //                if (lghtNode != null) prtNode.Nodes.Add(lghtNode);
                                //            }
                                //        }

                                //        if (prtNode != null) rdnNode.Nodes.Add(prtNode);
                                //    }
                                //}

                                rwyNd.Nodes.Add(rdnNode);
                            }
                        }


                        //if (rwy.RunwayElementsList != null)
                        //{
                        //    foreach (var rwyEl in rwy.RunwayElementsList)
                        //    {
                        //        var rwyElNode = CreatArenaTreeNode(rwyEl, "Runway element " + rwyEl.Designator);
                        //        if (rwyElNode != null) rwyNd.Nodes.Add(rwyElNode);
                        //    }
                        //}

                        //if (rwy.RunwayMarkingList != null)
                        //{
                        //    foreach (var rwyEl in rwy.RunwayMarkingList)
                        //    {
                        //        var rwyElNode = CreatArenaTreeNode(rwyEl, "Runway marking " + rwyEl.MarkingLocation.ToString());
                        //        if (rwyElNode != null) rwyNd.Nodes.Add(rwyElNode);
                        //    }
                        //}


                        //if (rwy.TaxiHoldingPositionList != null)
                        //{
                        //    foreach (var holdPos in rwy.TaxiHoldingPositionList)
                        //    {
                        //        var rwyElNode = CreatArenaTreeNode(holdPos, "Taxi holding position " + holdPos.LandingCategory.ToString());

                        //    }
                        //}

                        arpNd.Nodes.Add(rwyNd);
                    }
                }
                

                //if (arp.TaxiwayList != null)
                //{
                //    foreach (Taxiway txw in arp.TaxiwayList)
                //    {
                //        var txwNode = CreatArenaTreeNode(txw, txw.Designator);
                //        if (txwNode != null) arpNd.Nodes.Add(txwNode);
                //    }
                //}

                adhpRwyNode.Nodes.Add(arpNd);
            }

            #endregion

            #region Navaids

            var vorDmeNode = new TreeNode("VOR/DME") { Name = "VOR/DME", Checked = true };
            var vorTacanNode = new TreeNode("VOR/TACAN") { Name = "VOR/TACAN", Checked = true };
            var vorNode = new TreeNode("VOR") { Name = "VOR", Checked = true };
            var dmeNode = new TreeNode("DME") { Name = "DME", Checked = true };
            var tacanNode = new TreeNode("TACAN") { Name = "TACAN", Checked = true };
            var ndbNode = new TreeNode("NDB") { Name = "NDB", Checked = true };
            var ndbMarkerNode = new TreeNode("NDB/Marker") { Name = "NDB/Marker", Checked = true };
            //var ilsDMENode = new TreeNode("ILS/DME") { Name = "ILS/DME", Checked = true };
            //var ilsNode = new TreeNode("ILS") { Name = "ILS", Checked = true };
            var markerNode = new TreeNode("Marker") { Name = "Marker", Checked = true };





            var navList = (from element in DataCash.ProjectEnvironment.Data.PdmObjectList where (element != null) && (element is NavaidSystem) select element).ToList();
            var navaidsNode = new TreeNode("Navaids ") { Name = "Navaids", Checked = true };

            if (SortFlag)  navList= navList.OrderBy(x => x.GetObjectLabel()).ThenBy(x => x.GetObjectLabel()).ToList();


            foreach (NavaidSystem ns in navList)
            {
                
                var nsNode = CreatArenaTreeNode(ns);

                if (ns.Components != null)
                {
                    //var nsNode = CreatArenaTreeNode(ns);
                    nsNode.Name = ns.Designator;

                    if (ns.Components.Count >= 1)
                    {
                        foreach (PDMObject comp in ns.Components)
                        {
                            var compNode = CreatArenaTreeNode(comp);

                            nsNode.Nodes.Add(compNode);
                        }
                    }

                    if (ns.CodeNavaidSystemType == NavaidSystemType.VOR_DME) vorDmeNode.Nodes.Add(nsNode);
                    if (ns.CodeNavaidSystemType == NavaidSystemType.VORTAC) vorTacanNode.Nodes.Add(nsNode);
                    if (ns.CodeNavaidSystemType == NavaidSystemType.VOR) vorNode.Nodes.Add(nsNode);
                    if (ns.CodeNavaidSystemType == NavaidSystemType.DME) dmeNode.Nodes.Add(nsNode);
                    if (ns.CodeNavaidSystemType == NavaidSystemType.TACAN) tacanNode.Nodes.Add(nsNode);
                    if (ns.CodeNavaidSystemType == NavaidSystemType.NDB) ndbNode.Nodes.Add(nsNode);
                    if (ns.CodeNavaidSystemType == NavaidSystemType.NDB_MKR) ndbMarkerNode.Nodes.Add(nsNode);
                    //if (ns.CodeNavaidSystemType == NavaidSystemType.ILS_DME) ilsDMENode.Nodes.Add(nsNode);
                    if (ns.CodeNavaidSystemType == NavaidSystemType.ILS)
                        ilsNode.Nodes.Add(nsNode);
                    if (ns.CodeNavaidSystemType == NavaidSystemType.MKR) markerNode.Nodes.Add(nsNode);
                }
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


            var wypList = (from element in DataCash.ProjectEnvironment.Data.PdmObjectList where (element != null) && (element is WayPoint) select element).ToList();
            var wayPointNode = new TreeNode("WayPoints (" + wypList.Count.ToString()+")") { Name = "WayPoints", Checked = true };

            if (SortFlag) wypList = wypList.OrderBy(x => x.GetObjectLabel()).ThenBy(x => x.GetObjectLabel()).ToList();


            foreach (WayPoint wyp in wypList)
            {
                var wypNd = CreatArenaTreeNode(wyp);
                wypNd.Name = wyp.Designator;
                wayPointNode.Nodes.Add(wypNd);

            }

            #endregion

            #region Procedures

            var iapNode = new TreeNode(PROC_TYPE_code.Approach.ToString()) { Name = "Approach", Checked = true };
            var sidNode = new TreeNode(PROC_TYPE_code.SID.ToString()) { Name = "SID", Checked = true };
            var starNode = new TreeNode(PROC_TYPE_code.STAR.ToString()) { Name = "STAR", Checked = true }; ;
            var multipleNode = new TreeNode(PROC_TYPE_code.Multiple.ToString()) { Name = "Multiple", Checked = true }; ;

            var procList = (from element in DataCash.ProjectEnvironment.Data.PdmObjectList where (element != null) && (element is Procedure) select element).ToList();
            //procList = procList.OrderBy(x => x.GetObjectLabel()).ThenBy(x => ((Procedure)x).AirportIdentifier).ToList();

            var proceduresNode = new TreeNode("Procedures (" + procList.Count.ToString()+")") { Name = "Procedures", Checked = true };
            var procHldngList = new TreeNode();
            if (SortFlag) procList = procList.OrderBy(x => x.GetObjectLabel()).ThenBy(x => x.GetObjectLabel()).ToList();
            List<string> hldsId = new List<string>();

            foreach (Procedure prc in procList)
            {
                var procNd = CreatArenaTreeNode(prc, prc.GetObjectLabel()+ " ("+prc.Airport_ICAO_Code + ")");
                procNd.Name = procNd.Text;
                procNd.ImageIndex = 0;

                if (prc.Transitions != null)
                {
                    foreach (var trans in prc.Transitions)
                    {
                        var transNode = CreatArenaTreeNode(trans, trans.RouteType.ToString() );
                        transNode.Name = trans.FeatureGUID;
                        transNode.ImageIndex = 0;

                        if (trans.Legs != null)
                        {
                            foreach (var leg in trans.Legs)
                            {
                                var legNode = (CreatArenaTreeNode(leg, leg.GetObjectLabel() + " (" + leg.LegSpecialization.ToString() + ")"));
                                legNode.Name = leg.ID;
                                legNode.ImageIndex = 0;



                                if (leg.StartPoint != null)
                                {
                                    var sPointNode = CreateSignificantPointNode(leg.StartPoint);
                                    if (sPointNode != null) legNode.Nodes.Add(sPointNode);
                                }
                                if (leg.ArcCentre != null)
                                {
                                    var sPointNode = CreateSignificantPointNode(leg.ArcCentre);
                                    if (sPointNode != null) legNode.Nodes.Add(sPointNode);
                                }
                                if (leg.EndPoint != null)
                                {
                                    var sPointNode = CreateSignificantPointNode(leg.EndPoint);
                                    if (sPointNode != null) legNode.Nodes.Add(sPointNode);
                                }

                                if (leg.HoldingUse != null && hldsId.IndexOf(leg.HoldingUse.ID)<0)
                                {
                                    System.Diagnostics.Debug.WriteLine(prc.GetObjectLabel());
                                    var hldNd = CreatArenaTreeNode(leg.HoldingUse);
                                    if (hldNd != null)
                                    {
                                        hldNd.Name = leg.HoldingUse.HoldingPoint != null ? leg.HoldingUse.HoldingPoint.SegmentPointDesignator : "Holding";

                                        hldNd.Text = hldNd.Text;

                                        legNode.Nodes.Add(hldNd);
                                        procHldngList.Nodes.Add((TreeNode)hldNd.Clone());
                                        hldsId.Add(leg.HoldingUse.ID);
                                    }
                                }

                                if (leg.AngleIndication != null)
                                {
                                    string angleIndNodeText = GenerateDistAngleNodeText(leg.AngleIndication.FixID, leg.AngleIndication.SignificantPointID);
                                    TreeNode angleIndNode = CreatArenaTreeNode(leg.AngleIndication, "Angle Indication " + angleIndNodeText);
                                    if (angleIndNode != null) legNode.Nodes.Add(angleIndNode);
                                }

                                if (leg.DistanceIndication != null)
                                {
                                    string distIndNodeText = GenerateDistAngleNodeText(leg.DistanceIndication.FixID, leg.DistanceIndication.SignificantPointID);
                                    TreeNode distIndNode = CreatArenaTreeNode(leg.DistanceIndication, "Distance Indication " + distIndNodeText);
                                    if (distIndNode != null) legNode.Nodes.Add(distIndNode);
                                }

                                transNode.Nodes.Add(legNode);
                                transNode.ImageIndex = 0;

                            }
                        }
                        procNd.Nodes.Add(transNode);
                    }
                }

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

            #region Enroute


            var enroutList = (from element in DataCash.ProjectEnvironment.Data.PdmObjectList where (element != null) && (element is Enroute) select element).ToList();
            var enrouteNode = new TreeNode("Enroute (" + enroutList.Count.ToString()+")") { Name = "Enroute", Checked = true };

            if (SortFlag)  enroutList= enroutList.OrderBy(x => x.GetObjectLabel()).ThenBy(x => x.GetObjectLabel()).ToList();


            foreach (Enroute rte in enroutList)
            {
                var rteNd = CreatArenaTreeNode(rte, rte.TxtDesig);
                rteNd.Name = rte.TxtDesig;

                if (rte.Routes != null)
                {
                    foreach (var seg in rte.Routes)
                    {
                        if ((seg.StartPoint != null) && (seg.EndPoint != null))
                        {
                            if (seg.StartPoint.SegmentPointDesignator == null) seg.StartPoint.SegmentPointDesignator = seg.StartPoint.ID;
                            if (seg.EndPoint.SegmentPointDesignator == null) seg.EndPoint.SegmentPointDesignator = seg.StartPoint.ID;
                            var segNd =
                                CreatArenaTreeNode(seg, seg.StartPoint.SegmentPointDesignator  + " : " + seg.EndPoint.SegmentPointDesignator);
                            segNd.Name = seg.StartPoint.SegmentPointDesignator + " : " + seg.EndPoint.SegmentPointDesignator;

                            rteNd.Nodes.Add(segNd);
                        }
                        else
                        {
                            TreeNode nd = new TreeNode("Start : End");
                            nd.Tag = seg;
                            rteNd.Nodes.Add(nd);
                        }
                    }
                }

                enrouteNode.Nodes.Add(rteNd);
            }

            #endregion

            #region Airspaces

            var airspacesList = (from element in DataCash.ProjectEnvironment.Data.PdmObjectList where (element != null) && (element is Airspace) select element).ToList();
            var airspacesNode = new TreeNode("Airspaces (" + airspacesList.Count.ToString()+")") { Name = "Airspaces", Checked = true };


            if (SortFlag)  airspacesList= airspacesList.OrderBy(x => x.GetObjectLabel()).ThenBy(x => x.GetObjectLabel()).ToList();



            foreach (Airspace arsps in airspacesList)
            {
                var arspsNd = CreatArenaTreeNode(arsps, (arsps.TxtName!=null && arsps.TxtName.Length > 0) ? arsps.TxtName : arsps.CodeID);
                arspsNd.Name = arsps.CodeID;

                if (arsps.AirspaceVolumeList != null)
                {
                    foreach (var vol in arsps.AirspaceVolumeList)
                    {
                        var arspsVolNd = CreatArenaTreeNode(vol, vol.TxtName);
                        arspsVolNd.Name = vol.TxtName;

                        arspsNd.Nodes.Add(arspsVolNd);
                    }
                }

                if (arsps.ProtectedRoute != null)
                {
                    var arspsrt = CreatArenaTreeNode(arsps.ProtectedRoute, "protected Route " + arsps.ProtectedRoute.TxtDesig);
                    arspsrt.Name = arsps.ProtectedRoute.TxtDesig;

                    arspsNd.Nodes.Add(arspsrt);
                    
                }
                airspacesNode.Nodes.Add(arspsNd);
            }

            #endregion

            #region Obstacles

            var obstaclesList = (from element in DataCash.ProjectEnvironment.Data.PdmObjectList where (element != null) && (element is VerticalStructure) select element).ToList();
            var verticalStrNode = new TreeNode("VerticalStructure (" + obstaclesList.Count()+")") { Name = "VerticalStructure", Checked = true };

            if (SortFlag)  obstaclesList= obstaclesList.OrderBy(x => x.GetObjectLabel()).ThenBy(x => x.GetObjectLabel()).ToList();


            foreach (VerticalStructure obs in obstaclesList)
            {

                System.Diagnostics.Debug.WriteLine(obs.ID);
                var obsNd = CreatArenaTreeNode(obs, obs.Name);
                obsNd.Name = obs.Name;
                foreach (var prt in obs.Parts)
                {
     
                    var partNd = CreatArenaTreeNode(prt, prt.Designator);
                    partNd.Name = prt.Designator;
                    obsNd.Nodes.Add(partNd);
                }

                verticalStrNode.Nodes.Add(obsNd);
            }

            #endregion

            #region MSA

            var msaList = (from element in DataCash.ProjectEnvironment.Data.PdmObjectList where (element != null) && (element is SafeAltitudeArea) select element).ToList();
            var msaNode = new TreeNode("Safe Area (" + msaList.Count.ToString() + ")") { Name = "SafeArea", Checked = true };


            if (SortFlag) msaList = msaList.OrderBy(x => x.GetObjectLabel()).ThenBy(x => x.GetObjectLabel()).ToList();



            foreach (SafeAltitudeArea msa in msaList)
            {
                var parentNd = CreatArenaTreeNode(msa, "Safe Area");
                parentNd.Name = msa.ID;

                if (msa.SafeAltitudeAreaSector != null)
                {
                    foreach (var sector in msa.SafeAltitudeAreaSector)
                    {
                        var childNd = CreatArenaTreeNode(sector, "Sector");
                        childNd.Name = sector.ID;

                        parentNd.Nodes.Add(childNd);
                    }
                }

               
                msaNode.Nodes.Add(parentNd);
            }

            #endregion

            #region GeoBorder

            var borderList = (from element in DataCash.ProjectEnvironment.Data.PdmObjectList where (element != null) && (element is GeoBorder) select element).ToList();
            var borderNode = new TreeNode("Geo Borders") { Name = "GeoBorder", Checked = true };


            if (SortFlag) borderList = borderList.OrderBy(x => x.GetObjectLabel()).ThenBy(x => x.GetObjectLabel()).ToList();



            foreach (GeoBorder brdr in borderList)
            {
                var parentNd = CreatArenaTreeNode(brdr, brdr.GeoBorderName + "-" + brdr.NeighborName);
                parentNd.Name = brdr.ID;

                borderNode.Nodes.Add(parentNd);
            }

            #endregion

            #region Holding


            var hldngList = (from element in DataCash.ProjectEnvironment.Data.PdmObjectList where (element != null) && (element is HoldingPattern) select element).ToList();
            var hlngNode = new TreeNode("HoldingPattern (" + (hldngList.Count + procHldngList.Nodes.Count).ToString() + ")") { Name = "Holding", Checked = true };

            if (SortFlag) hldngList = hldngList.OrderBy(x => x.GetObjectLabel()).ThenBy(x => x.GetObjectLabel()).ToList();


            foreach (HoldingPattern hld in hldngList)
            {
                if (hldsId.IndexOf(hld.ID) < 0)
                {
                    var hldNd = CreatArenaTreeNode(hld);
                    hldNd.Name = hld.HoldingPoint != null ? hld.HoldingPoint.SegmentPointDesignator : "Holding";

                    hldNd.Text = hldNd.Text;

                    hlngNode.Nodes.Add(hldNd);
                    hldsId.Add(hld.ID);
                }
            }



            TreeNode[] myTreeNodeArray = new TreeNode[procHldngList.Nodes.Count];
             procHldngList.Nodes.CopyTo(myTreeNodeArray,0);
             hlngNode.Nodes.AddRange(myTreeNodeArray);
            hlngNode.Text = "HoldingPattern (" + hlngNode.Nodes.Count.ToString() + ")";

            #endregion


            if (adhpRwyNode.Nodes.Count > 0) FeatureTreeView.Nodes.Add(adhpRwyNode);
            if (navaidsNode.Nodes.Count > 0) FeatureTreeView.Nodes.Add(navaidsNode);
            if (wayPointNode.Nodes.Count > 0) FeatureTreeView.Nodes.Add(wayPointNode);
            if (proceduresNode.Nodes.Count > 0) FeatureTreeView.Nodes.Add(proceduresNode);
            if (enrouteNode.Nodes.Count > 0) FeatureTreeView.Nodes.Add(enrouteNode);
            if (airspacesNode.Nodes.Count > 0) FeatureTreeView.Nodes.Add(airspacesNode);
            if (verticalStrNode.Nodes.Count > 0) FeatureTreeView.Nodes.Add(verticalStrNode);
            if (msaNode.Nodes.Count > 0) FeatureTreeView.Nodes.Add(msaNode);
            if (borderNode.Nodes.Count > 0) FeatureTreeView.Nodes.Add(borderNode);
            if (hlngNode.Nodes.Count > 0) FeatureTreeView.Nodes.Add(hlngNode);



            FeatureTreeView.EndUpdate();

        }

		public void SelectNode(string id)
		{
			Dictionary<string, TreeNode> PdmObject_TreeNode = (FeatureTreeView.Tag as Dictionary<string, TreeNode>);
			if ( PdmObject_TreeNode.ContainsKey ( id ) )
			{
				FeatureTreeView.SelectedNode = PdmObject_TreeNode[ id ];
				FeatureTreeView.SelectedNode.Expand ( );
				FeatureTreeView.Focus ( );
			}
				
		}

        private TreeNode CreatArenaTreeNode(PDMObject pdmObj, string NdText)
        {
            TreeNode Nd = new TreeNode(NdText) {  };

            if (!pdmObj.CreatedAutomatically) Nd.ForeColor = Color.FromName("DarkCyan");

            //if (pdmObj.Geo == null) pdmObj.RebuildGeo();
            //if (pdmObj.Geo == null) Nd.NodeFont = new System.Drawing.Font(FeatureTreeView.Font, FontStyle.Italic);
            //else Nd.NodeFont = new System.Drawing.Font(FeatureTreeView.Font, FontStyle.Regular);
            if (pdmObj.ID == null) pdmObj.ID = Guid.NewGuid().ToString();

            Nd.Tag = pdmObj;
            Nd.ImageIndex = DataCash.ProjectEnvironment.Data.GetObjectImageIndex(pdmObj);



            Dictionary<string, TreeNode> PdmObject_TreeNode = (FeatureTreeView.Tag as Dictionary<string, TreeNode>);
            if (!PdmObject_TreeNode.ContainsKey(pdmObj.ID)) PdmObject_TreeNode.Add(pdmObj.ID, Nd);
            return Nd;
        }

        private TreeNode CreatArenaTreeNode(PDMObject pdmObj)
        {
            TreeNode Nd = new TreeNode(pdmObj.GetObjectLabel()) { };

            if (!pdmObj.CreatedAutomatically) Nd.ForeColor = Color.FromName("DarkCyan");

            //if (pdmObj.Geo == null) pdmObj.RebuildGeo();
            //if (pdmObj.Geo == null) Nd.NodeFont = new System.Drawing.Font(FeatureTreeView.Font, FontStyle.Italic);
            //else Nd.NodeFont = new System.Drawing.Font(FeatureTreeView.Font, FontStyle.Regular);
            if (pdmObj.ID == null) pdmObj.ID = Guid.NewGuid().ToString();


            Nd.Tag = pdmObj;
            Nd.ImageIndex = DataCash.ProjectEnvironment.Data.GetObjectImageIndex(pdmObj);

            Dictionary<string, TreeNode> PdmObject_TreeNode = (FeatureTreeView.Tag as Dictionary<string, TreeNode>);
            if (!PdmObject_TreeNode.ContainsKey(pdmObj.ID)) PdmObject_TreeNode.Add(pdmObj.ID, Nd);
            return Nd;

        }
        private TreeNode CreateSignificantPointNode(SegmentPoint segmentPoint)
        {
            TreeNode res = null;
            try
            {
                res = CreatArenaTreeNode(segmentPoint, segmentPoint.PointUse.ToString() + " " + segmentPoint.SegmentPointDesignator);

                if (segmentPoint.PointFacilityMakeUp != null)
                {
                    TreeNode fMkUpNode = CreatArenaTreeNode(segmentPoint.PointFacilityMakeUp, "Facility MakeUp " + segmentPoint.PointFacilityMakeUp.Role.ToString());
                    if (fMkUpNode != null)
                    {
                        if (segmentPoint.PointFacilityMakeUp.AngleIndication != null)
                        {
                            string angleIndNodeText = GenerateDistAngleNodeText(segmentPoint.PointFacilityMakeUp.AngleIndication.FixID, segmentPoint.PointFacilityMakeUp.AngleIndication.SignificantPointID);
                            TreeNode angleIndNode = CreatArenaTreeNode(segmentPoint.PointFacilityMakeUp.AngleIndication, "Angle Indication " + angleIndNodeText);
                            if (angleIndNode != null) fMkUpNode.Nodes.Add(angleIndNode);
                        }

                        if (segmentPoint.PointFacilityMakeUp.DistanceIndication != null)
                        {
                            string distIndNodeText = GenerateDistAngleNodeText(segmentPoint.PointFacilityMakeUp.DistanceIndication.FixID, segmentPoint.PointFacilityMakeUp.DistanceIndication.SignificantPointID);
                            TreeNode distIndNode = CreatArenaTreeNode(segmentPoint.PointFacilityMakeUp.DistanceIndication, "Distance Indication " + distIndNodeText);
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
                    var arpList = (from element in DataCash.ProjectEnvironment.Data.PdmObjectList where (element != null) && (element is AirportHeliport) && ( ((AirportHeliport)element).RunwayList!=null) select element).ToList();
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

        #endregion

        #region "Add Event Wiring for New/Open Documents"
        // Event member variables
        private IDocumentEvents_Event m_docEvents;

        // Wiring
        private void SetUpDocumentEvent(IDocument myDocument)
        {
            m_docEvents = myDocument as IDocumentEvents_Event;

            m_docEvents.OpenDocument += new IDocumentEvents_OpenDocumentEventHandler(RefreshList);
            m_docEvents.NewDocument += new IDocumentEvents_NewDocumentEventHandler(RefreshList);
            m_docEvents.CloseDocument += new IDocumentEvents_CloseDocumentEventHandler(m_docEvents_CloseDocument);
        }

        void m_docEvents_CloseDocument()
        {
            propertyGrid1.SelectedObject = null;
            //DataCash.ProjectEnvironment = null;
            FeatureTreeView.Nodes.Clear();
        }
      
        #endregion

        private void FeatureTreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            propertyGrid1.SelectedObject = null;
            if (e.Node.Tag == null) return;
            //propertyGrid1.ReadOnly = (e.Node.Tag is Airspace);
            propertyGrid1.SelectedObject = e.Node.Tag;
        }

        private void propertyGrid1_SelectedObjectsChanged(object sender, EventArgs e)
        {
           
        }

        private void propertyGrid1_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            if (MessageBox.Show(@"Update selected property?", "Arena", MessageBoxButtons.YesNo) == DialogResult.No)
            {
                ArenaStaticProc.SetObjectValue(propertyGrid1.SelectedObject, e.ChangedItem.PropertyDescriptor.Name, e.OldValue);
            }
            else
            {
                ((PDMObject)propertyGrid1.SelectedObject).UpdateDB(DataCash.ProjectEnvironment.Data.TableDictionary[propertyGrid1.SelectedObject.GetType()]);
            }
        }

        private void showOnMapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if ((DataCash.ProjectEnvironment.Data.PdmObjectList == null) || (DataCash.ProjectEnvironment.Data.PdmObjectList.Count <= 0)) return;
                if (FeatureTreeView.SelectedNode == null) return;
                if (FeatureTreeView.SelectedNode.Tag == null) return;


                IMxDocument document = (IMxDocument)m_application.Document;
                IMap pMap = document.FocusMap;

                var sellObj = (PDMObject)FeatureTreeView.SelectedNode.Tag;

                #region MyRegion

                //object objPropVal = sellObj.GetType().GetProperty("ID").GetValue(sellObj, null);

               
                //System.Diagnostics.Debug.WriteLine(sellObj.GetType().Name + " " + objPropVal.ToString());

                //foreach (System.Reflection.PropertyInfo propInfo in sellObj.GetType().GetProperties())
                //{

                //    if (propInfo == null) continue;

                //    object objProp = sellObj.GetType().GetProperty(propInfo.Name).GetValue(sellObj, null);
                //    if (objProp is IList)
                //    {
                //        foreach (var item in (IList)objProp)
                //        {
                //           // objPropVal = sellObj.GetType().GetProperty("ID").GetValue(sellObj, null);

                //        }
                //    }

                //}

                //return;
                
                #endregion


                if (sellObj is SegmentPoint)
                {
                    if (((SegmentPoint)sellObj).PointChoice == PointChoice.DesignatedPoint)
                        sellObj = DataCash.GetPDMObject(((SegmentPoint)sellObj).PointChoiceID, PDM_ENUM.WayPoint);
                    else if (((SegmentPoint)sellObj).PointChoice == PointChoice.Navaid)
                        sellObj = DataCash.GetPDMObject(((SegmentPoint)sellObj).PointChoiceID, PDM_ENUM.NavaidSystem);
                }


                if (sellObj.Geo == null) 
                    FillGeo(sellObj);

                var lyr = DataCash.ProjectEnvironment.Data.GetLinkedLayer(sellObj);
                if ((sellObj is NavaidSystem) && (lyr == null)) 
                    lyr = DataCash.ProjectEnvironment.Data.GetLinkedLayer(((NavaidSystem)sellObj).Components[0]);
                
               

                DataCash.ProjectEnvironment.ClearGraphics();


                ShowObjectInfo(sellObj, FeatureTreeView.SelectedNode.Text, lyr);


                ((IActiveView)pMap).Refresh();


            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.StackTrace + " ERROR " + ex.Message);
            }
        }

        public string GetPdmObjectLinkedWithLayer(string layerName)
        {
            var res = "";
            switch (layerName)
            {
                case ("AirportHeliport"):
                    res = "AirportHeliport";
                    break;
                case ("RunwayDirection"):
                    res = "RunwayDirection";
                    break;
                case ("GlidePath"):
                    res = "GlidePath";
                    break;
                case ("Localizer"):
                    res = "Localizer";
                    break;
                case ("VOR"):
                    res = "VOR";
                    break;
                case ("DME"):
                    res = "DME";
                    break;
                case ("NDB"):
                    res = "NDB";
                    break;
                case ("TACAN"):
                    res = "TACAN";
                    break;
                case ("WayPoint"):
                    res = "WayPoint";
                    break;
                case ("Enroute"):
                case ("RouteSegment"):
                    res = "RouteSegment";
                    break;
                case ("AirspaceVolume"):
                    res = "AirspaceVolume";
                    break;
            }

            return res;
        }

        public void FillGeo(PDMObject sellObj)
        {
            try
            {
                var tp = sellObj.GetType().Name;
                switch (tp)
                {
                    case ("AirportHeliport"):
                    case ("RunwayDirection"):
                    case ("VOR"):
                    case ("DME"):
                    case ("NDB"):
                    case ("TACAN"):
                    case ("Localizer"):
                    case ("GlidePath"):
                    case ("RouteSegment"):
                    case ("WayPoint"):
                    case ("AirspaceVolume"):
                    case ("RunwayCenterLinePoint"):
                    case ("VerticalStructurePart"):
                    case ("Marker"):
                    case ("GeoBorder"):
                    case("HoldingPattern"):
                        sellObj.RebuildGeo();
                        break;
                    case ("NavaidSystem"):
                        foreach (PDMObject comp in ((NavaidSystem)sellObj).Components)
                        {
                            comp.RebuildGeo();
                        }

                        break;
                    case ("Enroute"):
                        foreach (var rte in ((Enroute)sellObj).Routes)
                        {
                            rte.RebuildGeo();
                        }
                        break;
                    case ("Airspace"):
                        foreach (var vol in ((Airspace)sellObj).AirspaceVolumeList)
                        {
                            
                            if (vol.Geo == null) vol.RebuildGeo2();
                           
                        }
                        break;
                    case ("VerticalStructure"):
                        foreach (var prt in ((VerticalStructure)sellObj).Parts)
                        {
                            if (prt.Geo == null) prt.RebuildGeo();
                            
                        }
                        break;

                    case ("InstrumentApproachProcedure"):
                    case ("StandardInstrumentArrival"):
                    case ("StandardInstrumentDeparture"):
                        foreach (var _trans in ((Procedure)sellObj).Transitions)
                        {
                            foreach (var _leg in _trans.Legs)
                            {
                                if (_leg.Geo == null) _leg.RebuildGeo2();
                            }
                        }

                        break;

                    case ("ProcedureTransitions"):
                        foreach (var _leg in ((ProcedureTransitions)sellObj).Legs)
                        {
                            if (_leg.Geo == null) _leg.RebuildGeo();
                        }
                        break;

                    case ("FinalLeg"):
                    case ("ProcedureLeg"):
                    case ("MissaedApproachLeg"):
                    case ("FacilityMakeUp"):
                        if (sellObj.Geo == null) sellObj.RebuildGeo();
                        break;
                }

            }
            catch (Exception)
            {
                throw;
            }

        }

        private void ShowObjectInfo(PDMObject sellObj, string infoText, ILayer LinkedLayer)
        {
            if (sellObj == null) return;
            try
            {
                var tp = sellObj.GetType().Name;
                IPoint pntGeo;
                IPoint pntPrj;
                IPolyline lnGeo;
                IPolyline linePrj;

                 IMxDocument document = (IMxDocument)m_application.Document;
                 IMap projectMap = document.FocusMap;

                //IMap projectMap = DataCash.ProjectEnvironment.pMap;
                 var lyr = EsriUtils.getLayerByName(projectMap, "AirportHeliport"); ;
                var fc = (lyr as IFeatureLayer).FeatureClass;
                DataCash.ProjectEnvironment.Data.SpatialReference = (fc as IGeoDataset).SpatialReference;
                List<string> ids = new List<string>();
                List<string> id = new List<string>();
                List<int> dbID = new List<int>();
                List<HoldingPattern> hlnglist = new List<HoldingPattern>();
                

                #region Show Annotation

                //var LinkedLayer = DataCash.ProjectEnvironment.Data.GetLinkedLayer(sellObj);
                switch (tp)
                {

                    case ("AirportHeliport"):
                    case ("RunwayDirection"):
                    case ("RunwayCenterLinePoint"):
                    case ("VOR"):
                    case ("DME"):
                    case ("NDB"):
                    case ("TACAN"):
                    case ("Localizer"):
                    case ("GlidePath"):
                    case ("WayPoint"):
                    case ("Marker"):

                        #region

                        pntGeo = sellObj.Geo as IPoint;
                        pntPrj = new PointClass();
                        pntPrj.PutCoords(pntGeo.X, pntGeo.Y);
                        pntPrj = EsriUtils.ToProject(pntPrj, projectMap, DataCash.ProjectEnvironment.Data.SpatialReference) as IPoint;

                        AnnotationUtil.CreateAnnoInfo(projectMap, pntPrj, infoText, true);
                        id = new List<string> { sellObj.ID };
                        ids = new List<string>();
                        ids.Add(sellObj.ID);


                        ShowOnMap(LinkedLayer, ids);

                        #endregion

                        break;

                    case ("NavaidSystem"):

                        #region
                        {
                            double sc = (projectMap.MapScale * 100000) / 9000000;

                            foreach (PDMObject comp in ((NavaidSystem)sellObj).Components)
                            {
                                pntGeo = comp.Geo as IPoint;
                                pntPrj = new PointClass();
                                pntPrj.PutCoords(pntGeo.X, pntGeo.Y);
                                pntPrj = EsriUtils.ToProject(pntPrj, projectMap, DataCash.ProjectEnvironment.Data.SpatialReference) as IPoint;

                                AnnotationUtil.CreateAnnoInfo(projectMap, pntPrj, infoText + "(" + comp.GetType().Name + ")", false, sc);

                                ids.Add(comp.ID);


                                //sc = sc + 100000;
                            }
                            ShowOnMap(LinkedLayer, ids);


                        }

                        #endregion

                        break;

                    case ("RouteSegment"):

                        #region

                        lnGeo = (IPolyline)(sellObj.Geo);
                        linePrj = new PolylineClass();
                        linePrj.FromPoint = lnGeo.FromPoint;
                        linePrj.ToPoint = lnGeo.ToPoint;
                        linePrj = EsriUtils.ToProject(linePrj, projectMap, DataCash.ProjectEnvironment.Data.SpatialReference) as IPolyline;

                        AnnotationUtil.CreateAnnoInfo(projectMap, linePrj, infoText, true);
                        ids.Add(sellObj.ID);
                        ShowOnMap(LinkedLayer, ids);

                        break;

                        #endregion

                    case ("Enroute"):

                        #region
                        foreach (RouteSegment seg in ((Enroute)sellObj).Routes)
                        {

                            lnGeo = (IPolyline)(seg.Geo);
                            linePrj = new PolylineClass();
                            linePrj.FromPoint = lnGeo.FromPoint;
                            linePrj.ToPoint = lnGeo.ToPoint;
                            linePrj = EsriUtils.ToProject(linePrj, projectMap, DataCash.ProjectEnvironment.Data.SpatialReference) as IPolyline;

                            AnnotationUtil.CreateAnnoInfo(projectMap, linePrj, seg.GetObjectLabel(), false);
                            ids.Add(seg.ID);

                        }

                        ShowOnMap(LinkedLayer, ids);

                        #endregion

                        break;

                    case ("AirspaceVolume"):
                    case ("FacilityMakeUp"):
                        #region
                        var poly = (sellObj.Geo as IPolygon);
                        
                        AnnotationUtil.CreateAnnoInfo(projectMap, poly, true, DataCash.ProjectEnvironment.Data.SpatialReference);

                        dbID = new List<int> { sellObj.GetIDfromDB() };

                        ShowOnMap(LinkedLayer, dbID);
                        #endregion

                        break;

                    case ("Airspace"):

                        #region
                        var lyrVol = DataCash.ProjectEnvironment.Data.GetLinkedLayer(sellObj);

                        dbID = new List<int>();

                        foreach (var vol in ((Airspace)sellObj).AirspaceVolumeList)
                        {

                            AnnotationUtil.CreateAnnoInfo(projectMap, (vol.Geo as IPointCollection), false, DataCash.ProjectEnvironment.Data.SpatialReference);

                            
                            dbID.Add(vol.GetIDfromDB());
                        }

                        ShowOnMap(lyrVol, dbID);

                        #endregion

                        break;

                    case ("VerticalStructurePart"):

                        #region
                        if (sellObj.Geo != null)
                        {
                            dbID = new List<int> { sellObj.GetIDfromDB() };

                            ShowOnMap(LinkedLayer, dbID);
                            CreateObstacleAnnotation(sellObj.Geo, infoText, true);

                        }
                        #endregion

                        break;

                    case ("VerticalStructure"):

                        #region

                        foreach (var item in ((VerticalStructure)sellObj).Parts)
                        {
                            dbID = new List<int> { item.GetIDfromDB() };
                            lyr = DataCash.ProjectEnvironment.Data.GetLinkedLayer(item);
                            ShowOnMap(lyr, dbID);
                            CreateObstacleAnnotation(item.Geo, item.Designator, false);

                        }

                        #endregion

                        break;

                    case ("InstrumentApproachProcedure"):
                    case ("StandardInstrumentArrival"):
                    case ("StandardInstrumentDeparture"):

                        #region

                        foreach (var _trans in ((Procedure)sellObj).Transitions)
                        {
                            foreach (var _leg in _trans.Legs)
                            {
                                if (_leg.HoldingUse != null) hlnglist.Add(_leg.HoldingUse);
                                if (_leg.Geo == null) continue;
                                lnGeo = (IPolyline)(_leg.Geo);
                                linePrj = new PolylineClass();
                                linePrj.FromPoint = lnGeo.FromPoint;
                                linePrj.ToPoint = lnGeo.ToPoint;
                                linePrj = EsriUtils.ToProject(linePrj, projectMap, DataCash.ProjectEnvironment.Data.SpatialReference) as IPolyline;

                                AnnotationUtil.CreateAnnoInfo(projectMap, linePrj, _leg.LegTypeARINC.ToString(), false);
                                ids.Add(_leg.ID);

                               
                            }
                        }
                        ShowOnMap(LinkedLayer, ids);

                        ids.Clear();
                        foreach (var item in hlnglist)
                        {
                            ids.Add(item.ID);
                            AnnotationUtil.CreateAnnoInfo(projectMap, (item.Geo as IPolygon), true, DataCash.ProjectEnvironment.Data.SpatialReference);
                            ShowOnMap(DataCash.ProjectEnvironment.Data.GetLinkedLayer(item), ids,false);
                        }
                          

                        break;

                    case ("ProcedureTransitions"):
                        foreach (var _leg in ((ProcedureTransitions)sellObj).Legs)
                        {
                            if (_leg.HoldingUse != null) hlnglist.Add(_leg.HoldingUse);
                            if (_leg.Geo == null) continue;
                            lnGeo = (IPolyline)(_leg.Geo);
                            linePrj = new PolylineClass();
                            linePrj.FromPoint = lnGeo.FromPoint;
                            linePrj.ToPoint = lnGeo.ToPoint;
                            linePrj = EsriUtils.ToProject(linePrj, projectMap, DataCash.ProjectEnvironment.Data.SpatialReference) as IPolyline;

                            AnnotationUtil.CreateAnnoInfo(projectMap, linePrj, _leg.LegTypeARINC.ToString(), false);
                            ids.Add(_leg.ID);
                        }

                        ShowOnMap(LinkedLayer, ids);

                        ids.Clear();
                        foreach (var item in hlnglist)
                        {
                            ids.Add(item.ID);
                            AnnotationUtil.CreateAnnoInfo(projectMap, (item.Geo as IPolygon), true, DataCash.ProjectEnvironment.Data.SpatialReference);
                            ShowOnMap(DataCash.ProjectEnvironment.Data.GetLinkedLayer(item), ids,false);
                        }

                        break;

                    case ("FinalLeg"):
                    case ("ProcedureLeg"):
                    case ("MissaedApproachLeg"):
                        //ids.Add(sellObj.ID);
                        if (sellObj.Geo != null)
                        {
                            lnGeo = (IPolyline)(sellObj.Geo);
                            linePrj = new PolylineClass();
                            linePrj.FromPoint = lnGeo.FromPoint;
                            linePrj.ToPoint = lnGeo.ToPoint;
                            linePrj = EsriUtils.ToProject(linePrj, projectMap, DataCash.ProjectEnvironment.Data.SpatialReference) as IPolyline;

                            AnnotationUtil.CreateAnnoInfo(projectMap, linePrj, ((ProcedureLeg)sellObj).LegTypeARINC.ToString(), false);
                            ids.Add(sellObj.ID);

                            ShowOnMap(LinkedLayer, ids);
                        }
                        #endregion

                        break;

                    case ("GeoBorder"):

                        ids.Add(sellObj.ID);

                        ShowOnMap(LinkedLayer, ids);
                        break;
                    case("SegmentPoint"):
                        if (((SegmentPoint)sellObj).PointChoice == PointChoice.DesignatedPoint)
                        {
                            sellObj = DataCash.GetPDMObject(((SegmentPoint)sellObj).PointChoiceID, PDM_ENUM.WayPoint);
                            pntGeo = sellObj.Geo as IPoint;
                            pntPrj = new PointClass();
                            pntPrj.PutCoords(pntGeo.X, pntGeo.Y);
                            pntPrj = EsriUtils.ToProject(pntPrj, projectMap, DataCash.ProjectEnvironment.Data.SpatialReference) as IPoint;

                            AnnotationUtil.CreateAnnoInfo(projectMap, pntPrj, infoText, true);
                            id = new List<string> { sellObj.ID };
                            ids = new List<string>();
                            ids.Add(sellObj.ID);


                            ShowOnMap(LinkedLayer, ids);
                        }
                        else
                        {
                            sellObj = DataCash.GetPDMObject(((SegmentPoint)sellObj).PointChoiceID, PDM_ENUM.NavaidSystem);

                            double sc = (projectMap.MapScale * 100000) / 9000000;

                            foreach (PDMObject comp in ((NavaidSystem)sellObj).Components)
                            {
                                pntGeo = comp.Geo as IPoint;
                                pntPrj = new PointClass();
                                pntPrj.PutCoords(pntGeo.X, pntGeo.Y);
                                pntPrj = EsriUtils.ToProject(pntPrj, projectMap, DataCash.ProjectEnvironment.Data.SpatialReference) as IPoint;

                                AnnotationUtil.CreateAnnoInfo(projectMap, pntPrj, infoText + "(" + comp.GetType().Name + ")", false, sc);

                                ids.Add(comp.ID);


                                //sc = sc + 100000;
                            }
                        }
                        
                       
                        break;
                    case ("HoldingPattern"):

                        ids.Add(sellObj.ID);
                         var hld = (sellObj.Geo as IPolygon);

                         AnnotationUtil.CreateAnnoInfo(projectMap, hld, true, DataCash.ProjectEnvironment.Data.SpatialReference);
                        ShowOnMap(LinkedLayer, ids);
                        break;
                }

                #endregion


            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.StackTrace + " ERROR " + ex.Message);
                throw;
            }



        }

        private void ShowOnMap(ILayer layer, List<string> ids, bool ClearSelectionFlag = true)
        {
 
            try
            {
                IMxDocument document = (IMxDocument)m_application.Document;
                IMap pMap = document.FocusMap;
                //var pMap = DataCash.ProjectEnvironment.pMap;
                if (ClearSelectionFlag) pMap.ClearSelection();
                var pSelect = layer as IFeatureSelection;
                if (pSelect != null)
                {
                    pSelect.CombinationMethod = esriSelectionResultEnum.esriSelectionResultAdd;
                    var s = "( ";
                    for (var i = 0; i <= ids.Count - 2; i++)
                    { s = s + "\"" + ids[i] + "\","; }
                    s = s + "\"" + ids[ids.Count - 1] + "\")";

                    IQueryFilter queryFilter = new QueryFilterClass();
                    queryFilter.WhereClause = "FeatureGUID IN " + s;

                    pSelect.SelectFeatures(queryFilter, esriSelectionResultEnum.esriSelectionResultAdd, false);

                    UID menuID = new UIDClass();

                    menuID.Value = "{AB073B49-DE5E-11D1-AA80-00C04FA37860}";

                    ICommandItem pCmdItem = m_application.Document.CommandBars.Find(menuID);
                    
                    pCmdItem.Execute();
                    Marshal.ReleaseComObject(pCmdItem);
                    Marshal.ReleaseComObject(menuID);

                    //if (pSelect.SelectionSet.Count > 0)
                    //{

                    //    var zoomToSelected = new ControlsZoomToSelectedCommandClass();
                    //    zoomToSelected.OnCreate(m_application);
                    //    zoomToSelected.OnClick();

                    //}

                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.StackTrace + " ERROR " + ex.Message);
                throw;
            }

            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");

        }

        private void ShowOnMap(ILayer layer, List<int> ids)
        {

            try
            {


                IMxDocument document = (IMxDocument)m_application.Document;
                IMap pMap = document.FocusMap;
                //var pMap = DataCash.ProjectEnvironment.pMap;
                pMap.ClearSelection();
                var pSelect = layer as IFeatureSelection;


                if (pSelect != null)
                {
                    pSelect.CombinationMethod = esriSelectionResultEnum.esriSelectionResultAdd;
                    var s = "( ";
                    for (var i = 0; i <= ids.Count - 2; i++)
                    { s = s + " " + ids[i] + ", "; }
                    s = s + ids[ids.Count - 1] + ")";

                  

                    IQueryFilter queryFilter = new QueryFilterClass();
                    queryFilter.WhereClause = "OBJECTID in " + s;

                    pSelect.SelectFeatures(queryFilter, esriSelectionResultEnum.esriSelectionResultAdd, false);

                    UID menuID = new UIDClass();

                    menuID.Value = "{AB073B49-DE5E-11D1-AA80-00C04FA37860}";

                    ICommandItem pCmdItem = m_application.Document.CommandBars.Find(menuID);
                    pCmdItem.Execute();
                    Marshal.ReleaseComObject(pCmdItem);
                    Marshal.ReleaseComObject(menuID);

                    //if (pSelect.SelectionSet.Count > 0)
                    //{

                    //    var zoomToSelected = new ControlsZoomToSelectedCommandClass();
                    //    zoomToSelected.OnCreate(m_application);
                    //    zoomToSelected.OnClick();

                    //}

                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.StackTrace + " ERROR " + ex.Message);
                throw;
            }

            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");

        }

        private void CreateObstacleAnnotation(IGeometry iGeometry, string infoText, bool cleatGrahics)
        {
            IPoint pntGeo;
            IPoint pntPrj;
            IPolyline lnGeo;
            IPolyline linePrj;
            IMxDocument document = (IMxDocument)m_application.Document;
            IMap pMap = document.FocusMap;

            switch (iGeometry.GeometryType)
            {

                case (esriGeometryType.esriGeometryPoint):
                    pntGeo = iGeometry as IPoint;
                    pntPrj = new PointClass();
                    pntPrj.PutCoords(pntGeo.X, pntGeo.Y);
                    pntPrj = EsriUtils.ToProject(pntPrj, pMap, DataCash.ProjectEnvironment.Data.SpatialReference) as IPoint;
                    AnnotationUtil.CreateAnnoInfo(pMap, pntPrj, infoText, cleatGrahics);
                    break;

                case (esriGeometryType.esriGeometryLine):
                case (esriGeometryType.esriGeometryPolyline):
                    lnGeo = (IPolyline)(iGeometry);
                    linePrj = new PolylineClass();
                    linePrj.FromPoint = lnGeo.FromPoint;
                    linePrj.ToPoint = lnGeo.ToPoint;
                    linePrj = EsriUtils.ToProject(linePrj, pMap, DataCash.ProjectEnvironment.Data.SpatialReference) as IPolyline;
                    AnnotationUtil.CreateAnnoInfo(pMap, linePrj, infoText, cleatGrahics);
                    break;

                case (esriGeometryType.esriGeometryPolygon):
                    pntGeo = ((IArea)iGeometry).Centroid;
                    pntPrj = new PointClass();
                    pntPrj.PutCoords(pntGeo.X, pntGeo.Y);
                    pntPrj = EsriUtils.ToProject(pntPrj, pMap, DataCash.ProjectEnvironment.Data.SpatialReference) as IPoint;
                    AnnotationUtil.CreateAnnoInfo(pMap, pntPrj, infoText, cleatGrahics);
                    break;

            }

        }

        private void deleteSelectedObjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if ((DataCash.ProjectEnvironment.Data.PdmObjectList == null) || (DataCash.ProjectEnvironment.Data.PdmObjectList.Count <= 0)) return;
            if (FeatureTreeView.SelectedNode == null) return;
            if (FeatureTreeView.SelectedNode.Tag == null) return;

            TreeNode selnd = FeatureTreeView.SelectedNode;

            IMxDocument document = (IMxDocument)m_application.Document;
            IMap pMap = document.FocusMap;

            var sellObj2 = (PDMObject)FeatureTreeView.SelectedNode.Tag;
            if (sellObj2 != null)
            {
                if (sellObj2.DeleteObject(DataCash.ProjectEnvironment.Data.TableDictionary) == 1)
                {
                    RemoveFeature(sellObj2);
                    RemovePdmObject(sellObj2);
                    
                    FeatureTreeView.Nodes.Remove(selnd);

                    DataCash.ProjectEnvironment.ClearGraphics();

                    pMap.ClearSelection();
                    ((IActiveView)pMap).Refresh();
                }
            }
        }

        private void RemovePdmObject(PDMObject sellObj2)
        {
           

            switch (sellObj2.PDM_Type)
            {
                case PDM_ENUM.AirportHeliport:
                    DataCash.ProjectEnvironment.Data.PdmObjectList.Remove(sellObj2);
                    break;
                case PDM_ENUM.Runway:
                    var lstRunway = (from element in DataCash.ProjectEnvironment.Data.PdmObjectList
                                     where (element != null) && (element is AirportHeliport) 
                                    && (element.ID.StartsWith(((Runway)sellObj2).ID_AirportHeliport)) select element).FirstOrDefault();
                    if (lstRunway != null)
                     {
                         ((AirportHeliport)lstRunway).RunwayList.Remove((Runway)sellObj2);
                     }
                    break;
                case PDM_ENUM.RunwayDirection:
                     var lstRunwayDirection = (from element in DataCash.ProjectEnvironment.Data.PdmObjectList where (element != null) && (element is AirportHeliport) 
                                    && (element.ID.StartsWith(((Runway)sellObj2).ID_AirportHeliport)) select element).FirstOrDefault();
                     if (lstRunwayDirection != null)
                     {
                         ((AirportHeliport)lstRunwayDirection).RunwayList.Remove((Runway)sellObj2);
                     }
                    break;
                case PDM_ENUM.RunwayCenterLinePoint:
                    break;
                case PDM_ENUM.DeclaredDistance:
                    break;
                case PDM_ENUM.NavaidSystem:
                    DataCash.ProjectEnvironment.Data.PdmObjectList.Remove(sellObj2);
                    break;
                case PDM_ENUM.NavaidComponent:
                    DataCash.ProjectEnvironment.Data.PdmObjectList.Remove(sellObj2);
                    break;
                case PDM_ENUM.Localizer:
                    break;
                case PDM_ENUM.GlidePath:
                    break;
                case PDM_ENUM.VOR:
                    break;
                case PDM_ENUM.DME:
                    break;
                case PDM_ENUM.TACAN:
                    break;
                case PDM_ENUM.NDB:
                    break;
                case PDM_ENUM.WayPoint:
                    DataCash.ProjectEnvironment.Data.PdmObjectList.Remove(sellObj2);
                    break;
                case PDM_ENUM.Marker:
                    break;
                case PDM_ENUM.InstrumentApproachProcedure:
                    DataCash.ProjectEnvironment.Data.PdmObjectList.Remove(sellObj2);
                    break;
                case PDM_ENUM.StandardInstrumentArrival:
                    DataCash.ProjectEnvironment.Data.PdmObjectList.Remove(sellObj2);
                    break;
                case PDM_ENUM.StandardInstrumentDeparture:
                    DataCash.ProjectEnvironment.Data.PdmObjectList.Remove(sellObj2);
                    break;
                case PDM_ENUM.AircraftCharacteristic:
                    DataCash.ProjectEnvironment.Data.PdmObjectList.Remove(sellObj2);
                    break;
                case PDM_ENUM.Procedure:
                    DataCash.ProjectEnvironment.Data.PdmObjectList.Remove(sellObj2);
                    break;
                case PDM_ENUM.ProcedureTransitions:
                    DataCash.ProjectEnvironment.Data.PdmObjectList.Remove(sellObj2);
                    break;
                case PDM_ENUM.FinalLeg:
                    break;
                case PDM_ENUM.MissaedApproachLeg:
                    break;
                case PDM_ENUM.ProcedureLeg:
                    break;
                case PDM_ENUM.ObstacleAssessmentArea:
                    break;
                case PDM_ENUM.Obstruction:
                    break;
                case PDM_ENUM.ApproachCondition:
                    break;
                case PDM_ENUM.SegmentPoint:
                    break;
                case PDM_ENUM.FacilityMakeUp:
                    break;
                case PDM_ENUM.DistanceIndication:
                    break;
                case PDM_ENUM.AngleIndication:
                    break;
                case PDM_ENUM.Enroute:
                    DataCash.ProjectEnvironment.Data.PdmObjectList.Remove(sellObj2);
                    break;
                case PDM_ENUM.RouteSegment:
                    var enrt = (from element in DataCash.ProjectEnvironment.Data.PdmObjectList where (element != null) && (element is Enroute) 
                                    && (element.ID.StartsWith(((RouteSegment)sellObj2).ID_Route)) select element).FirstOrDefault();
                    if (enrt != null)
                    {
                        ((Enroute)enrt).Routes.Remove((RouteSegment)sellObj2);
                    }
                    
                    break;
                case PDM_ENUM.RouteSegmentPoint:
                    break;
                case PDM_ENUM.AirspaceVolume:
                    var arsp = (from element in DataCash.ProjectEnvironment.Data.PdmObjectList where (element != null) && (element is Airspace)
                                    && (element.ID.StartsWith(((AirspaceVolume)sellObj2).ID_Airspace)) select element).FirstOrDefault();
                    if (arsp != null)
                    {
                        ((Airspace)arsp).AirspaceVolumeList.Remove((AirspaceVolume)sellObj2);
                    }
                    break;
                case PDM_ENUM.Airspace:
                    DataCash.ProjectEnvironment.Data.PdmObjectList.Remove(sellObj2);
                    break;
                case PDM_ENUM.VerticalStructure:
                    DataCash.ProjectEnvironment.Data.PdmObjectList.Remove(sellObj2);
                    break;
                case PDM_ENUM.VerticalStructurePart:
                     var vertStr = (from element in DataCash.ProjectEnvironment.Data.PdmObjectList where (element != null) && (element is VerticalStructure)
                                    && (element.ID.StartsWith(((VerticalStructurePart)sellObj2).VerticalStructure_ID))
                                    select element).FirstOrDefault();
                     if (vertStr != null)
                    {
                        ((VerticalStructure)vertStr).Parts.Remove((VerticalStructurePart)sellObj2);
                    }
                    break;
                case PDM_ENUM.AREA_PDM:
                    break;
                case PDM_ENUM.HoldingPattern:
                    break;
                default:
                    DataCash.ProjectEnvironment.Data.PdmObjectList.Remove(sellObj2);
                    break;
            }

            
        }

        private void RemoveFeature(PDMObject sellObj)
        {
            try
            {
                AranSupport.Utilitys ArnUtil = new AranSupport.Utilitys();
                ArnUtil.DeleteObject(DataCash.ProjectEnvironment.Data.TableDictionary[sellObj.GetType()], sellObj.ID);
                var tp = sellObj.GetType().Name;
                switch (tp)
                {
                    case ("InstrunentApproachProcedure"):
                    case ("StandardInstrumentArrival"):
                    case ("StandardInstrumentDeparture"):
                        ArnUtil.DeleteObject(DataCash.ProjectEnvironment.Data.TableDictionary[sellObj.GetType()], sellObj.ID);

                        break;
                }

                if (sellObj is Procedure)
                    ArnUtil.DeleteObject(DataCash.ProjectEnvironment.Data.TableDictionary[typeof(Procedure)], sellObj.ID);
            }
            catch (Exception)
            {
                throw;
            }



        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            panel2.Visible = !panel2.Visible;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((DataCash.ProjectEnvironment.Data.PdmObjectList == null) || (DataCash.ProjectEnvironment.Data.PdmObjectList.Count <= 0)) return;

            comboBox2.Items.Clear();
            comboBox2.Text = "";
            List<PDMObject> objList;
            bool SortFlag = (bool)comboBox1.Tag;

            switch (comboBox1.SelectedIndex)
            {
                case (1):
                    objList = (from element in DataCash.ProjectEnvironment.Data.PdmObjectList where (element != null) && (element is AirportHeliport) select element).ToList();
                    if (SortFlag) objList = objList.OrderBy(x => x.GetObjectLabel()).ThenBy(x => x.GetObjectLabel()).ToList();

                    foreach (AirportHeliport adhp in objList)
                        if (adhp.Designator!=null) comboBox2.Items.Add(adhp.Designator);
                    break;

                case (2):

                    objList = (from element in DataCash.ProjectEnvironment.Data.PdmObjectList where (element != null) && (element is NavaidSystem) select element).ToList();
                    if (SortFlag) objList = objList.OrderBy(x => x.GetObjectLabel()).ThenBy(x => x.GetObjectLabel()).ToList();
                    
                    foreach (NavaidSystem nvds in objList)
                        if (nvds.Designator != null) comboBox2.Items.Add(nvds.Designator);
                    break;

                case (3):
                    objList = (from element in DataCash.ProjectEnvironment.Data.PdmObjectList where (element != null) && (element is WayPoint) select element).ToList();
                    if (SortFlag) objList = objList.OrderBy(x => x.GetObjectLabel()).ThenBy(x => x.GetObjectLabel()).ToList();
                    
                    foreach (WayPoint wyp in objList)
                        if (wyp.Designator != null) comboBox2.Items.Add(wyp.Designator);
                    break;
                case (4):
                    objList = (from element in DataCash.ProjectEnvironment.Data.PdmObjectList where (element != null) && (element is VerticalStructure) select element).ToList();
                    if (SortFlag) objList = objList.OrderBy(x => x.GetObjectLabel()).ThenBy(x => x.GetObjectLabel()).ToList();
                    
                    foreach (VerticalStructure obs in objList)
                        if (obs.Name != null) comboBox2.Items.Add(obs.Name);
                    break;
                case (5):
                    objList = (from element in DataCash.ProjectEnvironment.Data.PdmObjectList where (element != null) && (element is Airspace) select element).ToList();
                    if (SortFlag) objList = objList.OrderBy(x => x.GetObjectLabel()).ThenBy(x => x.GetObjectLabel()).ToList();
                    
                    foreach (Airspace obj in objList)
                        if (obj.CodeID != null) comboBox2.Items.Add(obj.CodeID);
                    break;
                case (6):
                    objList = (from element in DataCash.ProjectEnvironment.Data.PdmObjectList where (element != null) && (element is Enroute) select element).ToList();
                    if (SortFlag) objList = objList.OrderBy(x => x.GetObjectLabel()).ThenBy(x => x.GetObjectLabel()).ToList();
                    
                    foreach (Enroute obj in objList)
                        if (obj.TxtDesig != null)
                        {
                            comboBox2.Items.Add(obj.TxtDesig);
                            foreach (RouteSegment item in obj.Routes)
                            {
                                comboBox2.Items.Add(item.GetObjectLabel());
                            }
                        }
                    break;
                case (7):
                    objList = (from element in DataCash.ProjectEnvironment.Data.PdmObjectList where (element != null) && (element is InstrumentApproachProcedure) select element).ToList();
                    if (SortFlag) objList = objList.OrderBy(x => x.GetObjectLabel()).ThenBy(x => x.GetObjectLabel()).ToList();

                    foreach (InstrumentApproachProcedure prc in objList)
                    {
                        comboBox2.Items.Add(prc.GetObjectLabel() + " (" + prc.Airport_ICAO_Code + ")");

                    }
                    break;
                case (8):
                    objList = (from element in DataCash.ProjectEnvironment.Data.PdmObjectList where (element != null) && (element is StandardInstrumentDeparture) select element).ToList();
                    if (SortFlag) objList = objList.OrderBy(x => x.GetObjectLabel()).ThenBy(x => x.GetObjectLabel()).ToList();

                    foreach (StandardInstrumentDeparture prc in objList)
                    {
                        comboBox2.Items.Add(prc.GetObjectLabel() + " (" + prc.Airport_ICAO_Code + ")");

                    }
                    break;
                case (9):
                    objList = (from element in DataCash.ProjectEnvironment.Data.PdmObjectList where (element != null) && (element is StandardInstrumentArrival) select element).ToList();
                    if (SortFlag) objList = objList.OrderBy(x => x.GetObjectLabel()).ThenBy(x => x.GetObjectLabel()).ToList();

                    foreach (StandardInstrumentArrival prc in objList)
                    {
                        comboBox2.Items.Add(prc.GetObjectLabel() + " (" + prc.Airport_ICAO_Code + ")");

                    }
                    break;
                default:
                    comboBox2.Text = "";
                    break;

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {


            
            try
            {
                IMxDocument document = (IMxDocument)m_application.Document;
                IMap projectMap = document.FocusMap;

                //////////////////////////////////////////////////////////////////////////////////////////////
                #region Demo

                //Airspace arsp = new Airspace { CodeID = "test", TxtName = "test", AirspaceVolumeList = new List<AirspaceVolume>() };
                //AirspaceVolume arspvol = new AirspaceVolume { CodeActivity = "CodeActivity", CodeId = "test" };


                //object missing = Type.Missing;
                //IPoint pnt = new PointClass();
                //pnt.PutCoords(23, 56);
                //ICircularArc circle = new CircularArcClass();
                //IConstructCircularArc const_circle = (IConstructCircularArc)circle;
                //const_circle.ConstructCircle(pnt, 0.7, true);
                //circle.SpatialReference = ((IMxDocument)m_application.Document).FocusMap.SpatialReference;



                //PolygonClass plgn = new PolygonClass();
                //ISegmentCollection segcol = (ISegmentCollection)plgn;
                //segcol.AddSegment((ISegment)circle, ref missing, ref missing);


                //var zAware = plgn as IZAware;
                //zAware.ZAware = true;
                //plgn.Simplify();
                //plgn.SetConstantZ(0);
                //pdmObj.ConvertValueToMeter(pdmObj.ValDistVerUpper.ToString(), pdmObj.UomDistVerUpper.ToString());


                //var mAware = plgn as IMAware;
                //mAware.MAware = true;

                //arspvol.Geo = plgn as IGeometry;

                //zAware = arspvol.Geo as IZAware;
                //zAware.ZAware = true;


                //mAware = arspvol.Geo as IMAware;
                //mAware.MAware = true;

                
                //arsp.AirspaceVolumeList.Add(arspvol);

                //DataCash.StorePDMobject(arsp);

                //((IActiveView)projectMap).Refresh();
                //FillObjectsTree();

                //return;

                #endregion
                ////////////////////////////////////////////////////////////////////////////////////////////


                int nodeIndx =-1;
                TreeNode[] res = null;
                TreeNode[] parentNode;
                switch (comboBox1.SelectedIndex)
                {
                    case 1:
                        parentNode = FeatureTreeView.Nodes.Find("ARP/RWY/RDN/ILS", true);
                        nodeIndx = FeatureTreeView.Nodes.IndexOf(parentNode[0]);
                        res = FeatureTreeView.Nodes[nodeIndx].Nodes.Find(comboBox2.Text, true);
                        break;
                    case 2:
                        parentNode = FeatureTreeView.Nodes.Find("Navaids", true);
                        nodeIndx = FeatureTreeView.Nodes.IndexOf(parentNode[0]);
                        res = FeatureTreeView.Nodes[nodeIndx].Nodes.Find(comboBox2.Text, true);
                        if (res.Length == 0)
                        {
                            //NodeIndx = 0;
                            parentNode = FeatureTreeView.Nodes.Find("ARP/RWY/RDN/ILS", true);
                            nodeIndx = FeatureTreeView.Nodes.IndexOf(parentNode[0]);
                            res = FeatureTreeView.Nodes[nodeIndx].Nodes.Find(comboBox2.Text, true);
                        }
                        break;
                    case 3:
                        parentNode = FeatureTreeView.Nodes.Find("WayPoints", true);
                        nodeIndx = FeatureTreeView.Nodes.IndexOf(parentNode[0]);
                        res = FeatureTreeView.Nodes[nodeIndx].Nodes.Find(comboBox2.Text, true);
                        break;
                    case 4:
                        parentNode = FeatureTreeView.Nodes.Find("VerticalStructure", true);
                        nodeIndx = FeatureTreeView.Nodes.IndexOf(parentNode[0]);
                        res = FeatureTreeView.Nodes[nodeIndx].Nodes.Find(comboBox2.Text, true);
                        break;
                    case 5:
                        parentNode = FeatureTreeView.Nodes.Find("Airspaces", true);
                        nodeIndx = FeatureTreeView.Nodes.IndexOf(parentNode[0]);
                        res = FeatureTreeView.Nodes[nodeIndx].Nodes.Find(comboBox2.Text, true);
                        break;
                    case 6:
                        parentNode = FeatureTreeView.Nodes.Find("Enroute", true);
                        nodeIndx = FeatureTreeView.Nodes.IndexOf(parentNode[0]);
                        if (comboBox2.Text.Contains(" : "))
                        {
                            res = parentNode[0].Nodes.Find(comboBox2.Text, true);
                        }
                        else
                            res = FeatureTreeView.Nodes[nodeIndx].Nodes.Find(comboBox2.Text, true);
                        break;
                    case 7:
                        parentNode = FeatureTreeView.Nodes.Find("Procedures", true);
                        nodeIndx = FeatureTreeView.Nodes.IndexOf(parentNode[0]);
                        
                        res = parentNode[0].Nodes.Find("Approach", true);

                        nodeIndx =  parentNode[0].Nodes.IndexOf(res[0]);
                        res = res[0].Nodes.Find(comboBox2.Text, false);

                        break;

                    case 8:
                        parentNode = FeatureTreeView.Nodes.Find("Procedures", true);
                        nodeIndx = FeatureTreeView.Nodes.IndexOf(parentNode[0]);

                        res = parentNode[0].Nodes.Find("SID", true);

                        nodeIndx = parentNode[0].Nodes.IndexOf(res[0]);
                        res = res[0].Nodes.Find(comboBox2.Text, false);

                        break;
                    case 9:
                        parentNode = FeatureTreeView.Nodes.Find("Procedures", true);
                        nodeIndx = FeatureTreeView.Nodes.IndexOf(parentNode[0]);

                        res = parentNode[0].Nodes.Find("STAR", true);

                        nodeIndx = parentNode[0].Nodes.IndexOf(res[0]);
                        res = res[0].Nodes.Find(comboBox2.Text, false);

                        break;
                }

                if ((res == null) || (res.Length <= 0)) return;

                FeatureTreeView.SelectedNode = res[0];
                FeatureTreeView.SelectedNode.Expand();

                var sellObj = (PDMObject)FeatureTreeView.SelectedNode.Tag;

                if (sellObj.Geo == null) FillGeo(sellObj);
                DataCash.ProjectEnvironment.ClearGraphics();

                var lyr = DataCash.ProjectEnvironment.Data.GetLinkedLayer(sellObj);
                ShowObjectInfo(sellObj, FeatureTreeView.SelectedNode.Text, lyr);

                ((IActiveView)projectMap).PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);

                propertyGrid1.SelectedObject = FeatureTreeView.SelectedNode.Tag;
                FeatureTreeView.Select();
                FeatureTreeView.Focus();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.StackTrace + " ERROR " + ex.Message);
            }
        }

        private void refreshTreeViewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FillObjectsTree(false);
        }

        private void sortTreeViewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FillObjectsTree(true);
        }

        private void FeatureTreeView_KeyPress(object sender, KeyPressEventArgs e)
        {
           
        }

        private void FeatureTreeView_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F5) FillObjectsTree(false);
            if (e.KeyCode == Keys.F6) FillObjectsTree(true); 
        }

        private void cloneObjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if ((DataCash.ProjectEnvironment.Data.PdmObjectList == null) || (DataCash.ProjectEnvironment.Data.PdmObjectList.Count <= 0)) return;
            if (FeatureTreeView.SelectedNode == null) return;
            if (FeatureTreeView.SelectedNode.Tag == null) return;

            TreeNode selnd = FeatureTreeView.SelectedNode;

            //IMxDocument document = (IMxDocument)m_application.Document;
            //IMap pMap = document.FocusMap;

           
            var sellObj2 = (PDMObject)FeatureTreeView.SelectedNode.Tag;
            if (sellObj2 != null)
            {

                PDMObject pdmClone = (PDMObject)sellObj2.Clone();
                DataCash.StorePDMobject(pdmClone);
                FillObjectsTree(false);
            }
        }

        private void contextMenuDummy_Opening(object sender, CancelEventArgs e)
        {
            if (FeatureTreeView.SelectedNode == null) return;

            string[] ndsnames = new string[] { "ARP/RWY/RDN/ILS", "Navaids", "Procedures", "WayPoints", "Enroute", "Airspaces", "VerticalStructure", "VOR/DME", "VOR/TACAN",
                                                "VOR","DME","TACAN","NDB","NDB/Marker","ILS/DME","ILS","Marker"};
            TreeNode selnd = FeatureTreeView.SelectedNode;


            cloneObjectToolStripMenuItem.Enabled = selnd.Parent != null && !(selnd.Tag == null) && (selnd.Tag is PDMObject) && System.Array.IndexOf(ndsnames, selnd.Parent.Text) >= 0 && !(selnd.Tag is Procedure);
            deleteSelectedObjectToolStripMenuItem.Enabled = System.Diagnostics.Debugger.IsAttached;
            //procTABToolStripMenuItem.Visible = System.Diagnostics.Debugger.IsAttached; 

        }

        private void clearSelectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataCash.ProjectEnvironment.ClearGraphics();

            UID menuID = new UIDClass();

            menuID.Value = "{37C833F3-DBFD-11D1-AA7E-00C04FA37860}";

            ICommandItem pCmdItem = m_application.Document.CommandBars.Find(menuID);
            pCmdItem.Execute();
            Marshal.ReleaseComObject(pCmdItem);
            Marshal.ReleaseComObject(menuID);


            IMxDocument document = (IMxDocument)m_application.Document;
            IMap pMap = document.FocusMap;

            ((IActiveView)pMap).Refresh();

            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");

        }

        private void procTABToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var pdmProc = (PDMObject)FeatureTreeView.SelectedNode.Tag;

            var arp = DataCash.GetAirport(((Procedure)pdmProc).AirportIdentifier);

            string TempName = @"\SID\";
            if (pdmProc.PDM_Type == PDM_ENUM.InstrumentApproachProcedure) TempName = @"\IAP\";
            else if (pdmProc.PDM_Type == PDM_ENUM.StandardInstrumentArrival) TempName = @"\STAR\";

            ProcedureTabulation cdnTbl = new ProcedureTabulation
            {
                MagVar = ((AirportHeliport)arp).MagneticVariation.Value,
                TemplateName = System.IO.Path.Combine(ArenaStaticProc.GetPathToTemplate() + TempName, "TABULAR DESCRIPTION.xls"),
                NewCodingTableName = System.IO.Path.Combine(@"D:\", pdmProc.GetObjectLabel() +".xls"),
                AltitudeUOM = "FT",
                DistanceUOM = "NM",

            };


            var listprocOrderedByName = new List<PDMObject>();
            listprocOrderedByName.Add(pdmProc);
            cdnTbl.CreateCodingTable(listprocOrderedByName);

            MessageBox.Show("Saved on " + cdnTbl.NewCodingTableName);
        }
    }
}
