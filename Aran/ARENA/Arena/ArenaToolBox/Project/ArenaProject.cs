using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using PDM;
using ESRI.ArcGIS.Carto;
using System.Drawing;
using ESRI.ArcGIS.Geodatabase;
using System;
using ARENA.Util;
using ESRI.ArcGIS.Controls;
using EsriWorkEnvironment;
using ESRI.ArcGIS.Geometry;
using ArenaStatic;
using System.Data;
//using ARENA.Settings;
using System.ComponentModel;
using ARENA.Enums_Const;
using ARENA.Settings;

namespace ARENA.Project
{
    public class ArenaProject : AbstractProject
    {
        //private ToolStripButton SnapToolButton;
        //private ToolStripButton FilterButton;
        //private ToolStripLabel ProjectSettingsMenu;
        ////private ArenaSnapping snp;
        //private ContextMenuStrip snapContextMenu;

        //private ToolStripMenuItem loadDataToolStripMenuItem;
        //private ToolStripMenuItem aRINCToolStripMenuItem;
        //private ToolStripMenuItem loadObstacleDataFromExcellToolStripMenuItem;
        //private ToolStripMenuItem loadDataFromTOSSMToolStripMenuItem;
        //private ToolStripMenuItem loadARANAIXM51SnapshotToolStripMenuItem;
        //private ToolStripMenuItem loadAIXM45SnapshotToolStripMenuItem;

        //private ToolStripMenuItem editToolStripMenuItem;
        //private ToolStripMenuItem airportGroupDataToolStripMenuItem;
        //private ToolStripMenuItem navaidsGroupDataToolStripMenuItem;
        //private ToolStripMenuItem waypointGroupDataToolStripMenuItem;
        //private ToolStripMenuItem enRouteGroupDataToolStripMenuItem;
        //private ToolStripMenuItem airspaceGroupDataToolStripMenuItem;
        //private ToolStripMenuItem proceduresToolStripMenuItem;
        //private ToolStripMenuItem instrumentApproachProceduresToolStripMenuItem;
        //private ToolStripMenuItem standardInstrumentArrivalToolStripMenuItem;
        //private ToolStripMenuItem standardInstrumentDepartureToolStripMenuItem;
        //private ToolStripMenuItem verticalStructureToolStripMenuItem;

        public ArenaSettings ProjectSettings { get; set; }


        #region Overrides of AbstractProject

        public ArenaProject(Environment.Environment environment) : base(environment)
        {

            //EnvironmentToolStrip.Items.Clear();
            //while (environment.MaimMenu.Items.Count >1) environment.MaimMenu.Items.RemoveAt(1);

            //#region BranchToolStripMenuItems

            //ToolStripMenuItem hidedBranchToolStripMenuItem = new ToolStripMenuItem(@"Hide Branch")
            //{
            //    Name = "hideBranchToolStripMenuItem",
            //    Size = new System.Drawing.Size(162, 22),
            //};
            //hidedBranchToolStripMenuItem.Click += new System.EventHandler(hideBranchToolStripMenuItem_Click);
            //environment.FeatureTreeViewContextMenuStrip.Items.Add(hidedBranchToolStripMenuItem);

            //ToolStripMenuItem showdBranchToolStripMenuItem = new ToolStripMenuItem(@"UnHide Branch")
            //{
            //    Name = "uNHideBranchToolStripMenuItem",
            //    Size = new System.Drawing.Size(162, 22),
            //};

            //showdBranchToolStripMenuItem.Click += new System.EventHandler(showdBranchToolStripMenuItem_Click);
            //environment.FeatureTreeViewContextMenuStrip.Items.Add(showdBranchToolStripMenuItem);
            
            //#endregion

            //#region SnapToolButton

            //if (EnvironmentToolStrip.Items.Find("SnapToolButton", true).Length == 0)
            //{
            //    SnapToolButton = new System.Windows.Forms.ToolStripButton("SnapToolButton")
            //    {
            //        DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image,
            //        Image = global::ARENA.Properties.Resources.info,
            //        ImageTransparentColor = System.Drawing.Color.Magenta,
            //        Size = new System.Drawing.Size(23, 27),
            //        Text = "Object info",
            //        Name = "SnapToolButton"
            //    };

            //    SnapToolButton.Click += new System.EventHandler(SnapToolButton_Click);
            //    EnvironmentToolStrip.Items.Add(SnapToolButton);
            //}
            //#endregion

            //#region FilterButton

            //if (EnvironmentToolStrip.Items.Find("FilterButton", true).Length == 0)
            //{
            //    FilterButton = new System.Windows.Forms.ToolStripButton("FilterButton")
            //    {
            //        DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image,
            //        Image = global::ARENA.Properties.Resources.filter,
            //        ImageTransparentColor = System.Drawing.Color.Magenta,
            //        Size = new System.Drawing.Size(23, 27),
            //        Text = "Filter",
            //        Name = "FilterButton"
            //    };

            //    FilterButton.Click += new System.EventHandler(FilterButton_Click);
            //    EnvironmentToolStrip.Items.Add(FilterButton);
            //}
            //#endregion

            //#region ProjectSettings

            //this.ProjectSettingsMenu = new System.Windows.Forms.ToolStripLabel();
            //this.ProjectSettingsMenu.Image = global::ARENA.Properties.Resources.configure1;
            //this.ProjectSettingsMenu.Name = "toolStripLabel1";
            //this.ProjectSettingsMenu.Size = new System.Drawing.Size(99, 27);
            //this.ProjectSettingsMenu.Text = "Project Settings";
            //this.ProjectSettingsMenu.Click += new System.EventHandler(this.ProjectSettingsMenu_Click);
            //EnvironmentToolStrip.Items.Add(ProjectSettingsMenu);

            //#endregion

            #region main menu

            //#region loadDataToolStripMenuItem

            //loadDataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ////loadDataToolStripMenuItem.Enabled = false;
            //loadDataToolStripMenuItem.Name = "loadDataToolStripMenuItem";
            //loadDataToolStripMenuItem.Size = new System.Drawing.Size(42, 20);
            //loadDataToolStripMenuItem.Text = "Data";

            //#region SubMenu

            //aRINCToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            //aRINCToolStripMenuItem.Image = global::ARENA.Properties.Resources.MetadataCreateUpdate16;
            //aRINCToolStripMenuItem.Name = "aRINCToolStripMenuItem";
            //aRINCToolStripMenuItem.Size = new System.Drawing.Size(219, 22);
            //aRINCToolStripMenuItem.Text = "load ARINC / JEPPESEN file";
            //aRINCToolStripMenuItem.Click += new System.EventHandler(ArincToolStripMenuItemClick);

            //loadObstacleDataFromExcellToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            //loadObstacleDataFromExcellToolStripMenuItem.Image = global::ARENA.Properties.Resources.TableGroupRows16;
            //loadObstacleDataFromExcellToolStripMenuItem.Name = "loadObstacleDataFromExcellToolStripMenuItem";
            //loadObstacleDataFromExcellToolStripMenuItem.Size = new System.Drawing.Size(219, 22);
            //loadObstacleDataFromExcellToolStripMenuItem.Text = "load Obstacle data from Excell";
            //loadObstacleDataFromExcellToolStripMenuItem.Click += new System.EventHandler(loadObstacleDataFromExcellToolStripMenuItem_Click);

            //loadDataFromTOSSMToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            //loadDataFromTOSSMToolStripMenuItem.Image = global::ARENA.Properties.Resources.ServerWMS16;
            //loadDataFromTOSSMToolStripMenuItem.Name = "loadDataFromTOSSMToolStripMenuItem";
            //loadDataFromTOSSMToolStripMenuItem.Size = new System.Drawing.Size(219, 22);
            //loadDataFromTOSSMToolStripMenuItem.Text = "load data from TOSSM";
            //loadDataFromTOSSMToolStripMenuItem.Click += new System.EventHandler(loadDataFromTOSSMToolStripMenuItem_Click);

            //loadARANAIXM51SnapshotToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            //loadARANAIXM51SnapshotToolStripMenuItem.Image = global::ARENA.Properties.Resources.GeoprocessingScript16;
            //loadARANAIXM51SnapshotToolStripMenuItem.Name = "loadARANAIXM51SnapshotToolStripMenuItem";
            //loadARANAIXM51SnapshotToolStripMenuItem.Size = new System.Drawing.Size(219, 22);
            //loadARANAIXM51SnapshotToolStripMenuItem.Text = "load AIXM 5.1 snapshot";
            //loadARANAIXM51SnapshotToolStripMenuItem.Click += new System.EventHandler(loadARANAIXM51SnapshotToolStripMenuItem_Click);

            //loadAIXM45SnapshotToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            //loadAIXM45SnapshotToolStripMenuItem.Image = global::ARENA.Properties.Resources.GeoprocessingScript16;
            //loadAIXM45SnapshotToolStripMenuItem.Name = "loadAIXM45SnapshotToolStripMenuItem";
            //loadAIXM45SnapshotToolStripMenuItem.Size = new System.Drawing.Size(219, 22);
            //loadAIXM45SnapshotToolStripMenuItem.Text = "load ARAN 4.5 data";
            //loadAIXM45SnapshotToolStripMenuItem.Click += new System.EventHandler(loadAIXM45SnapshotToolStripMenuItem_Click);
            
            //#endregion

            //loadDataToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            //aRINCToolStripMenuItem,
            //loadObstacleDataFromExcellToolStripMenuItem,
            //loadDataFromTOSSMToolStripMenuItem,
            //loadARANAIXM51SnapshotToolStripMenuItem,
            //loadAIXM45SnapshotToolStripMenuItem});

            //environment.MaimMenu.Items.Add(loadDataToolStripMenuItem);

            //#endregion

            //#region editToolStripMenuItem

            //this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ////editToolStripMenuItem.Enabled = false;
            //editToolStripMenuItem.Name = "editToolStripMenuItem";
            //editToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            //editToolStripMenuItem.Text = "Edit";


            //#region SubMenu

            //airportGroupDataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            //this.airportGroupDataToolStripMenuItem.Name = "airportGroupDataToolStripMenuItem";
            //this.airportGroupDataToolStripMenuItem.Size = new System.Drawing.Size(214, 22);
            //this.airportGroupDataToolStripMenuItem.Tag = "Airport";
            //this.airportGroupDataToolStripMenuItem.Text = "Airport Group data";
            //this.airportGroupDataToolStripMenuItem.Click += new System.EventHandler(this.DataToolStripMenuItemClick);

            //this.navaidsGroupDataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            //this.navaidsGroupDataToolStripMenuItem.Name = "navaidsGroupDataToolStripMenuItem";
            //this.navaidsGroupDataToolStripMenuItem.Size = new System.Drawing.Size(214, 22);
            //this.navaidsGroupDataToolStripMenuItem.Tag = "Navaid";
            //this.navaidsGroupDataToolStripMenuItem.Text = "Navaids Group data";
            //this.navaidsGroupDataToolStripMenuItem.Click += new System.EventHandler(this.DataToolStripMenuItemClick);

            //this.waypointGroupDataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            //this.waypointGroupDataToolStripMenuItem.Name = "waypointGroupDataToolStripMenuItem";
            //this.waypointGroupDataToolStripMenuItem.Size = new System.Drawing.Size(214, 22);
            //this.waypointGroupDataToolStripMenuItem.Tag = "Waypoint";
            //this.waypointGroupDataToolStripMenuItem.Text = "Waypoint Group data";
            //this.waypointGroupDataToolStripMenuItem.Click += new System.EventHandler(this.DataToolStripMenuItemClick);

            //this.enRouteGroupDataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            //this.enRouteGroupDataToolStripMenuItem.Name = "enRouteGroupDataToolStripMenuItem";
            //this.enRouteGroupDataToolStripMenuItem.Size = new System.Drawing.Size(214, 22);
            //this.enRouteGroupDataToolStripMenuItem.Tag = "EnRoute";
            //this.enRouteGroupDataToolStripMenuItem.Text = "EnRoute Group data ";
            //this.enRouteGroupDataToolStripMenuItem.Click += new System.EventHandler(this.DataToolStripMenuItemClick);

            //this.airspaceGroupDataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            //this.airspaceGroupDataToolStripMenuItem.Name = "airspaceGroupDataToolStripMenuItem";
            //this.airspaceGroupDataToolStripMenuItem.Size = new System.Drawing.Size(214, 22);
            //this.airspaceGroupDataToolStripMenuItem.Tag = "Airspace";
            //this.airspaceGroupDataToolStripMenuItem.Text = "Airspace Group data";
            //this.airspaceGroupDataToolStripMenuItem.Click += new System.EventHandler(this.DataToolStripMenuItemClick);

            //#region ProcedureMenu

            //this.instrumentApproachProceduresToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            //this.standardInstrumentArrivalToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            //this.standardInstrumentDepartureToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            //// 
            //// instrumentApproachProceduresToolStripMenuItem
            //// 
            //this.instrumentApproachProceduresToolStripMenuItem.Name = "instrumentApproachProceduresToolStripMenuItem";
            //this.instrumentApproachProceduresToolStripMenuItem.Size = new System.Drawing.Size(233, 22);
            //this.instrumentApproachProceduresToolStripMenuItem.Tag = "IAP";
            //this.instrumentApproachProceduresToolStripMenuItem.Text = "Instrument Approach Procedures";
            //this.instrumentApproachProceduresToolStripMenuItem.Click += new System.EventHandler(this.DataToolStripMenuItemClick);
            //// 
            //// standardInstrumentArrivalToolStripMenuItem
            //// 
            //this.standardInstrumentArrivalToolStripMenuItem.Name = "standardInstrumentArrivalToolStripMenuItem";
            //this.standardInstrumentArrivalToolStripMenuItem.Size = new System.Drawing.Size(233, 22);
            //this.standardInstrumentArrivalToolStripMenuItem.Tag = "STAR";
            //this.standardInstrumentArrivalToolStripMenuItem.Text = "Standard Instrument Arrival";
            //this.standardInstrumentArrivalToolStripMenuItem.Click += new System.EventHandler(this.DataToolStripMenuItemClick);
            //// 
            //// standardInstrumentDepartureToolStripMenuItem
            //// 
            //this.standardInstrumentDepartureToolStripMenuItem.Name = "standardInstrumentDepartureToolStripMenuItem";
            //this.standardInstrumentDepartureToolStripMenuItem.Size = new System.Drawing.Size(233, 22);
            //this.standardInstrumentDepartureToolStripMenuItem.Tag = "SID";
            //this.standardInstrumentDepartureToolStripMenuItem.Text = "Standard Instrument Departure";
            //this.standardInstrumentDepartureToolStripMenuItem.Click += new System.EventHandler(this.DataToolStripMenuItemClick);


            //this.proceduresToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            //this.proceduresToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            //this.instrumentApproachProceduresToolStripMenuItem,
            //this.standardInstrumentArrivalToolStripMenuItem,
            //this.standardInstrumentDepartureToolStripMenuItem});
            //this.proceduresToolStripMenuItem.Name = "proceduresToolStripMenuItem";
            //this.proceduresToolStripMenuItem.Size = new System.Drawing.Size(214, 22);
            //this.proceduresToolStripMenuItem.Tag = "Procedures";
            //this.proceduresToolStripMenuItem.Text = "Procedures Group data";

            //#endregion


            //this.verticalStructureToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            //this.verticalStructureToolStripMenuItem.Name = "verticalStructureToolStripMenuItem";
            //this.verticalStructureToolStripMenuItem.Size = new System.Drawing.Size(214, 22);
            //this.verticalStructureToolStripMenuItem.Tag = "VerticalStructure";
            //this.verticalStructureToolStripMenuItem.Text = "Vertical Structure Group data";
            ////this.verticalStructureToolStripMenuItem.Click += new System.EventHandler(this.DataToolStripMenuItemClick);
            //#endregion

            //editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            //airportGroupDataToolStripMenuItem,
            //navaidsGroupDataToolStripMenuItem,
            //waypointGroupDataToolStripMenuItem,
            //enRouteGroupDataToolStripMenuItem,
            //airspaceGroupDataToolStripMenuItem,
            //proceduresToolStripMenuItem,
            //verticalStructureToolStripMenuItem});

            ////editToolStripMenuItem.Click += new System.EventHandler(editToolStripMenuItem_Click);

            //environment.MaimMenu.Items.Add(editToolStripMenuItem);

            //#endregion

            #endregion


            //Environment.PandaToolBox.Controls.Clear();

            try
            {

                //snapContextMenu = new ContextMenuStrip();

                //ReadOnlyPropertyGrid.ReadOnly = false;
                //ReadOnlyPropertyGrid.PropertyValueChanged += new PropertyValueChangedEventHandler(arenaObjects_PropertyValueChanged);

                //editToolStripMenuItem.Enabled = Environment.Data.PdmObjectList.Count > 0;

                ProjectSettings = new ArenaSettings();
            }
            catch {
               // MessageBox.Show(ex.Message);
            }

        }

        public override ArenaProjectType ProjectType
        {
            get { return ArenaProjectType.ARENA; }
        }

        #endregion

        public override void FillObjectsTree()
        {

            FeatureTreeView.BeginUpdate();
            FeatureTreeView.CheckBoxes = true;
            FeatureTreeView.Nodes.Clear();
            //Dictionary<string, AirportHeliport> result = new Dictionary<string, AirportHeliport>();

            var adhpRwyNode = new TreeNode("ARP/RWY/RDN/ILS") { Name = "ARP/RWY/RDN/ILS", Checked = true };
            var navaidsNode =  new TreeNode("Navaids") {Name = "Navaids",Checked = true};
            var proceduresNode =  new TreeNode("Procedures") { Name = "Procedures", Checked = true };
            var wayPointNode =  new TreeNode("WayPoints") { Name = "WayPoints", Checked = true };
            var enrouteNode =  new TreeNode("Enroute") { Name = "Enroute", Checked = true };
            var airspacesNode =  new TreeNode("Airspaces") { Name = "Airspaces", Checked = true };
            var verticalStrNode =  new TreeNode("VerticalStructure") { Name = "VerticalStructure", Checked = true };

            Environment.Data.LayersSymbolInImageList = new Dictionary<string, int>();

            #region ARP/RWY/RDN/ILS

            var arpList = (from element in PdmObjectList where (element != null) && (element is AirportHeliport) select element).ToList();

            foreach (AirportHeliport arp in arpList)
            {

                if (!Environment.Data.AirdromeHeliportDictionary.ContainsKey(arp.ID)) Environment.Data.AirdromeHeliportDictionary.Add(arp.ID, arp);

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
                                    }
                                    //if (CenterLnNd != null) rdnNode.Nodes.Add(CenterLnNd);

                                }

                                rwyNd.Nodes.Add(rdnNode);
                            }
                        }

                        arpNd.Nodes.Add(rwyNd);
                    }
                }

                adhpRwyNode.Nodes.Add(arpNd);
            }

            #endregion

            #region Navaids

            var vorDmeNode = new TreeNode("VOR/DME") { Name = "VOR/DME", Checked = true }; 
            var vorTacanNode =  new TreeNode("VOR/TACAN") { Name = "VOR/TACAN", Checked = true }; 
            var vorNode =  new TreeNode("VOR") { Name = "VOR", Checked = true }; 
            var dmeNode =  new TreeNode("DME") { Name = "DME", Checked = true }; 
            var tacanNode =  new TreeNode("TACAN") { Name = "TACAN", Checked = true }; 
            var ndbNode =  new TreeNode("NDB") { Name = "NDB", Checked = true };
            var ndbMarkerNode = new TreeNode("NDB/Marker") { Name = "NDB/Marker", Checked = true };
            var ilsDMENode = new TreeNode("ILS/DME") { Name = "ILS/DME", Checked = true };
            var ilsNode = new TreeNode("ILSE") { Name = "ILSE", Checked = true };
            var markerNode = new TreeNode("Marker") { Name = "Marker", Checked = true };





            var navList = (from element in PdmObjectList where (element != null) && (element is NavaidSystem) select element).ToList();

            foreach (NavaidSystem ns in navList)
            {
                if (ns.Components != null)
                {
                    var nsNode = CreatArenaTreeNode(ns);
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
                    if (ns.CodeNavaidSystemType == NavaidSystemType.ILS_DME) ilsDMENode.Nodes.Add(nsNode);
                    if (ns.CodeNavaidSystemType == NavaidSystemType.ILS) ilsNode.Nodes.Add(nsNode);
                    if (ns.CodeNavaidSystemType == NavaidSystemType.MKR) markerNode.Nodes.Add(nsNode);
                }
            }

            if (vorDmeNode.Nodes.Count > 0) navaidsNode.Nodes.Add(vorDmeNode);
            if (vorTacanNode.Nodes.Count > 0) navaidsNode.Nodes.Add(vorTacanNode);
            if (vorNode.Nodes.Count > 0) navaidsNode.Nodes.Add(vorNode);
            if (dmeNode.Nodes.Count > 0) navaidsNode.Nodes.Add(dmeNode);
            if (tacanNode.Nodes.Count > 0) navaidsNode.Nodes.Add(tacanNode);
            if (ndbNode.Nodes.Count > 0) navaidsNode.Nodes.Add(ndbNode);
            if (ilsDMENode.Nodes.Count > 0) navaidsNode.Nodes.Add(ilsDMENode);
            if (ilsNode.Nodes.Count > 0) navaidsNode.Nodes.Add(ilsNode);
            if (markerNode.Nodes.Count > 0) navaidsNode.Nodes.Add(markerNode);
            if (ndbMarkerNode.Nodes.Count > 0) navaidsNode.Nodes.Add(ndbMarkerNode);

            #endregion

            #region WayPoints


            var wypList = (from element in PdmObjectList where (element != null) && (element is WayPoint) select element).ToList();

            foreach (WayPoint wyp in wypList)
            {
                wayPointNode.Nodes.Add(CreatArenaTreeNode(wyp));
                wayPointNode.Name = wyp.Designator;

            }

            #endregion

            #region Procedures

            var iapNode =  new TreeNode(PROC_TYPE_code.Approach.ToString()) { Name = "Approach", Checked = true }; 
            var sidNode =  new TreeNode(PROC_TYPE_code.SID.ToString()) { Name = "SID", Checked = true };
            var starNode =  new TreeNode(PROC_TYPE_code.STAR.ToString()) { Name = "STAR", Checked = true }; ;
            var multipleNode =  new TreeNode(PROC_TYPE_code.Multiple.ToString()) { Name = "Multiple", Checked = true }; ;

            var procList = (from element in PdmObjectList where (element != null) && (element is Procedure) select element).ToList();
            foreach (Procedure prc in procList)
            {
                var procNd = CreatArenaTreeNode(prc, prc.Airport_ICAO_Code + " " + prc.ProcedureIdentifier);
                procNd.Name = prc.ProcedureIdentifier;
                procNd.ImageIndex = 0;

                if (prc.Transitions != null)
                {
                    foreach (var trans in prc.Transitions)
                    {
                        var transNode = CreatArenaTreeNode(trans, trans.TransitionIdentifier + " (" + trans.RouteType.ToString() + ")");
                        transNode.Name = trans.FeatureGUID;
                        transNode.ImageIndex = 0;

                        if (trans.Legs != null)
                        {
                            foreach (var leg in trans.Legs)
                            {
                                var legNode = (CreatArenaTreeNode(leg, leg.LegTypeARINC + " (" + leg.LegSpecialization.ToString() + ")"));
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


            var enroutList = (from element in PdmObjectList where (element != null) && (element is Enroute) select element).ToList();


            foreach (Enroute rte in enroutList)
            {
                var rteNd = CreatArenaTreeNode(rte, "Route  " + rte.TxtDesig);
                rteNd.Name = rte.TxtDesig;

                if (rte.Routes != null)
                {
                    foreach (var seg in rte.Routes)
                    {
                        if ((seg.StartPoint != null) && (seg.EndPoint != null))
                        {
                            var segNd =
                                CreatArenaTreeNode(seg, seg.StartPoint.SegmentPointDesignator + " : " + seg.EndPoint.SegmentPointDesignator);
                            segNd.Name = seg.StartPoint.SegmentPointDesignator + seg.EndPoint.SegmentPointDesignator;

                            rteNd.Nodes.Add(segNd);
                        }
                        else
                        {
                            TreeNode nd = new TreeNode("Start : End");
                            rteNd.Nodes.Add(nd);
                        }
                    }
                }

                enrouteNode.Nodes.Add(rteNd);
            }

            #endregion

            #region Airspaces

             var airspacesList = (from element in PdmObjectList where (element != null) && (element is Airspace) select element).ToList();


             foreach (Airspace arsps in airspacesList)
             {
                 var arspsNd = CreatArenaTreeNode(arsps, "Airspace  " + arsps.CodeID);
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
                 airspacesNode.Nodes.Add(arspsNd);
             }

            #endregion

            #region Obstacles

            var obstaclesList = (from element in PdmObjectList where (element != null) && (element is VerticalStructure) select element).ToList();


            foreach (VerticalStructure obs in obstaclesList)
            {

                System.Diagnostics.Debug.WriteLine(obs.ID);
                var obsNd = CreatArenaTreeNode(obs,"VerticalStructure  " + obs.Name);
                obsNd.Name = obs.Name;
                foreach (var prt in obs.Parts)
                {
                    System.Diagnostics.Debug.WriteLine("    "+prt.ID);

                    var partNd = CreatArenaTreeNode(prt,prt.Designator);
                    partNd.Name = prt.Designator;
                    obsNd.Nodes.Add(partNd);
                }

                verticalStrNode.Nodes.Add(obsNd);
            }

            #endregion


            if (adhpRwyNode.Nodes.Count > 0) FeatureTreeView.Nodes.Add(adhpRwyNode);
            if (navaidsNode.Nodes.Count > 0) FeatureTreeView.Nodes.Add(navaidsNode);
            if (wayPointNode.Nodes.Count > 0) FeatureTreeView.Nodes.Add(wayPointNode);
            if (proceduresNode.Nodes.Count > 0) FeatureTreeView.Nodes.Add(proceduresNode);
            if (enrouteNode.Nodes.Count > 0) FeatureTreeView.Nodes.Add(enrouteNode);
            if (airspacesNode.Nodes.Count > 0) FeatureTreeView.Nodes.Add(airspacesNode);
            if (verticalStrNode.Nodes.Count > 0) FeatureTreeView.Nodes.Add(verticalStrNode);



            FeatureTreeView.EndUpdate();

}

        private TreeNode CreateSignificantPointNode(SegmentPoint segmentPoint)
        {
            TreeNode res = null;
            try
            {
                res = CreatArenaTreeNode(segmentPoint, segmentPoint.PointUse.ToString());

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
                var fix = (from element in PdmObjectList where (element != null) && (element is WayPoint) && (((WayPoint)element).ID.CompareTo(FixID) == 0) select element).FirstOrDefault();
                if (fix!=null) res = " Fix:" + ((WayPoint)fix).Designator;
            }

            if ((SignificantPointID != null) && (SignificantPointID.Length > 0))
            {
                var nav = (from element in PdmObjectList where (element != null) && (element is NavaidSystem) && (((NavaidSystem)element).ID.CompareTo(SignificantPointID) == 0) select element).FirstOrDefault();
                if (nav != null) res = res + " Nav:" + ((NavaidSystem)nav).Designator;
            }

            return res;
        }

        public override void TreeViewAfterSelect(object sender, TreeViewEventArgs e)
        {
            ReadOnlyPropertyGrid.SelectedObject = null;
            if (e.Node.Tag == null) return;
            ReadOnlyPropertyGrid.ReadOnly = (e.Node.Tag is Airspace);
            ReadOnlyPropertyGrid.SelectedObject = e.Node.Tag;

        }

        public override void TreeViewAfterCheck(object sender, TreeViewEventArgs e)
        {
            try
            {
                
                if ((PdmObjectList == null) || (PdmObjectList.Count <= 0)) return;
                if (e.Node == null) return;
                if (e.Node.Tag == null) return;
                if (!(e.Node.Tag is PDMObject)) return;


                IMap pMap = Environment.pMap;
                pMap.ClearSelection();
                Environment.ClearGraphics();

                PDMObject sellObj = (PDMObject)e.Node.Tag;
                bool objVisibilityFlag = e.Node.Checked;//!sellObj.VisibilityFlag;

                //Environment.SetObjectVisibility(sellObj, objVisibilityFlag);

                (pMap as IActiveView).Refresh();//PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.StackTrace+ " ERROR "+ex.Message);
            }

        }




        private void hideBranchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetBranchVisibility(false);
            
        }

        private void showdBranchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetBranchVisibility(true);
        }

        private void SetBranchVisibility(bool visibility)
        {
            try
            {


                if ((PdmObjectList == null) || (PdmObjectList.Count <= 0)) return;
                if (FeatureTreeView.SelectedNode == null) return;
                if (FeatureTreeView.SelectedNode.Tag == null) return;
                if (!(FeatureTreeView.SelectedNode.Tag is PDMObject)) return;

                PDMObject sellObj = (PDMObject)FeatureTreeView.SelectedNode.Tag;


                FeatureTreeView.SelectedNode.Checked = visibility;// !sellObj.VisibilityFlag;

                Dictionary<string, TreeNode> PdmObject_TreeNode = (FeatureTreeView.Tag as Dictionary<string, TreeNode>);
                List<string> IDS = sellObj.GetBranch(Environment.Data.TableDictionary);

                for (int i = 1; i <= IDS.Count - 1; i++)
                {
                    string id = IDS[i];
                    if (PdmObject_TreeNode.ContainsKey(id))
                    {
                        TreeNode nd = PdmObject_TreeNode[id];
                        nd.Checked = visibility;//sellObj.VisibilityFlag; 
                    }
                }



            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.StackTrace + " ERROR " + ex.Message);
            }
        }

        //private void StopSnappingProcess()
        //{
        //    try
        //    {
        //        snp.StopSnapping();
        //        Environment.pMap.ActiveView.FocusMap.ClearSelection();
        //        ((IActiveView)Environment.pMap.ActiveView.FocusMap).Refresh();
        //        snp = null;
        //    }
        //    catch { }
        //}

        //public void StartSnappingProcess()
        //{

        //    try
        //    {
        //        IEnumLayer pElayer;

        //        IMap map = new MapClass();
        //        ESRI.ArcGIS.esriSystem.UID pUID = new ESRI.ArcGIS.esriSystem.UIDClass();

        //        pUID.Value = "{40A9E885-5533-11d0-98BE-00805F7CED21}";

        //        List<string> layersnames = EsriUtils.getMapLayerNames(Environment.pMap.ActiveView.FocusMap);

        //        for (int i = layersnames.Count - 1; i >= 0; i--)
        //        {
        //            string Nm = layersnames[i];
        //            ILayer lyr = EsriUtils.getLayerByName(Environment.pMap.ActiveView.FocusMap, Nm);
        //            if (lyr != null && lyr.Visible) map.AddLayer(lyr);

        //        }


        //        pElayer = map.get_Layers(pUID, true);

        //        ISpatialReference spatRefGeo = Environment.pMap.ActiveView.FocusMap.SpatialReference;

        //        ESRI.ArcGIS.Display.IDisplayTransformation dspTrans = (Environment.pMap.ActiveView.FocusMap as ESRI.ArcGIS.Carto.IActiveView).ScreenDisplay.DisplayTransformation;

        //        ESRI.ArcGIS.Display.IDisplayTransformation dspTrans2 = (Environment.pMap.ActiveView as ESRI.ArcGIS.Carto.IActiveView).ScreenDisplay.DisplayTransformation;

        //        snp = new ArenaSnapping(Environment.pMap, pElayer, 500, spatRefGeo, dspTrans);

        //        snp.Tolerance = Environment.pMap.ActiveView.FocusMap.MapScale * 0.00025 * 3;

        //        snp.ax_ActiveView_DispTransformation = dspTrans2;
        //        snp.ax_ActiveView_FocusMap_DispTransformation = dspTrans;
        //        snp.StartSnapping();

        //    }
        //    catch (Exception ex)
        //    {
        //        System.Diagnostics.Debug.WriteLine(ex.StackTrace + " ERROR " + ex.Message);
        //    }
        //}

        private TreeNode CreatArenaTreeNode(PDMObject pdmObj, string NdText)
        {
            TreeNode Nd = new TreeNode(NdText) { };
            //Environment.SetObjectVisibility(pdmObj, pdmObj.VisibilityFlag);

            Nd.Tag = pdmObj;
            Nd.ImageIndex = Environment.Data.GetObjectImageIndex(pdmObj);

            

            Dictionary<string, TreeNode> PdmObject_TreeNode = (FeatureTreeView.Tag as Dictionary<string, TreeNode>);
            if (!PdmObject_TreeNode.ContainsKey(pdmObj.ID)) PdmObject_TreeNode.Add(pdmObj.ID, Nd);
            return Nd;
        }

        private TreeNode CreatArenaTreeNode(PDMObject pdmObj)
        {
            TreeNode Nd = new TreeNode(pdmObj.GetObjectLabel()) { };
            //Environment.SetObjectVisibility(pdmObj, pdmObj.CreatedAutomatically);
 
            Nd.Tag = pdmObj;
            Nd.ImageIndex = Environment.Data.GetObjectImageIndex(pdmObj);

            Dictionary<string, TreeNode> PdmObject_TreeNode = (FeatureTreeView.Tag as Dictionary<string, TreeNode>);
            if (!PdmObject_TreeNode.ContainsKey(pdmObj.ID)) PdmObject_TreeNode.Add(pdmObj.ID, Nd);
            return Nd;

        }

        //private void SnapToolButton_Click(object sender, EventArgs e)
        //{
        //    if (SnapToolButton.Checked) { StopSnappingProcess(); SnapToolButton.Checked = false; return; }
        //    SnapToolButton.Checked = true;
        //    StartSnappingProcess();
        //}

        public override void OnLoad()
        {
            base.OnLoad();
        }

        //public override void MapControl_OnMouseDown(object sender, IMapControlEvents2_OnMouseDownEvent e)
        //{
        //    //MessageBox.Show("YES");
        //    if ((snp != null) && (snp.SnappedFeatureId.Count >0))
        //    {
        //        snapContextMenu.Items.Clear();
        //        if ((snp != null) && (snp.SnappedFeatureId.Count > 0))
        //        {
        //            Dictionary<string, TreeNode> PdmObject_TreeNode = (FeatureTreeView.Tag as Dictionary<string, TreeNode>);

        //            foreach (string IDS in snp.SnappedFeatureId)
        //            {
        //                ToolStripMenuItem snapMenuItem1 = new ToolStripMenuItem(IDS);

        //                snapMenuItem1.Name = "toolStripMenuItem" + Guid.NewGuid().ToString();
        //                snapMenuItem1.Size = new System.Drawing.Size(170, 22);

        //                if (PdmObject_TreeNode.ContainsKey(IDS))
        //                {
        //                    TreeNode nd = PdmObject_TreeNode[IDS];
        //                    snapMenuItem1.Text = ((PDMObject)nd.Tag).GetObjectLabel();
        //                    snapMenuItem1.Tag = nd;
        //                }


        //                snapMenuItem1.Click += new EventHandler(snapMenuItem1_Click);




        //                snapContextMenu.Items.Add(snapMenuItem1);
        //            }
        //            snapContextMenu.Show(Environment.MapControl, e.x, e.y);
        //        }

        //        return;
        //    }
        //    base.MapControl_OnMouseDown(sender, e);
        //}

        void snapMenuItem1_Click(object sender, EventArgs e)
        {
            TreeNode nd = (TreeNode)((ToolStripMenuItem)sender).Tag;
            if (nd != null)
            {
                FeatureTreeView.SelectedNode = nd;
                FeatureTreeView.SelectedNode.ExpandAll();
                Environment.FeatureTreeViewContextMenuStrip.Items["editObjectToolStripMenuItem"].PerformClick();
            }

        }

        private void arenaObjects_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            //PropertyDescriptorCollection pdc = TypeDescriptor.GetProperties(ReadOnlyPropertyGrid.SelectedObject);


            //for (int i = pdc.Count - 1; i >= 0; i--)
            //{

            //    bool readOnlyFlag = false;

            //    PropertyDescriptor pd = pdc[i];
            //    Attribute readOnlyAtr = pd.Attributes[typeof(ReadOnlyAttribute)];
            //    readOnlyFlag = (readOnlyAtr != null) && ((ReadOnlyAttribute)readOnlyAtr).IsReadOnly;
            //    if (readOnlyFlag)
            //    {
            //        Static_Proc.SetObjectValue(ReadOnlyPropertyGrid.SelectedObject, e.ChangedItem.PropertyDescriptor.Name, e.OldValue);
            //        break;
            //    }
                
            //}

            if (MessageBox.Show(@"Update selected property?", "Arena", MessageBoxButtons.YesNo) == DialogResult.No)
            {
                ArenaStaticProc.SetObjectValue(ReadOnlyPropertyGrid.SelectedObject, e.ChangedItem.PropertyDescriptor.Name, e.OldValue);
            }
            else
            {
                ((PDMObject)ReadOnlyPropertyGrid.SelectedObject).UpdateDB(Environment.Data.TableDictionary[ReadOnlyPropertyGrid.SelectedObject.GetType()]);
            }
        }

        //private void FilterButton_Click(object sender, EventArgs e)
        //{


        //    FilterForm filter = new FilterForm();
        //    filter.comboBoxObjectTypes.Items.Clear();
        //    filter.FilterList.Clear();
        //    foreach (KeyValuePair<Type, ITable> pair in Environment.Data.TableDictionary)
        //    {
        //        if ((pair.Key.Namespace.StartsWith("PDM")) && (!pair.Key.Name.StartsWith("AREA")))
        //            filter.comboBoxObjectTypes.Items.Add(pair.Key.Name);
        //    }

        //    string mdbName = Environment.Data.MapDocumentName.Replace(".mxd", ".mdb");
        //    mdbName = System.IO.Path.GetDirectoryName(mdbName) + @"\pdm.mdb";

        //    ARENA_DataReader.PathToPDM_DB = mdbName;
        //    List<string> FilteredIDs = new List<string>();

        //    if (filter.comboBoxObjectTypes.Items.Count > 0) filter.comboBoxObjectTypes.SelectedIndex = 0;

        //    if ((Environment.Data.CurrentFiltrsList != null) && (Environment.Data.CurrentFiltrsList.Count > 0))
        //    { filter.FilterList.AddRange(Environment.Data.CurrentFiltrsList); }
        //    Environment.Data.CurrentFiltrsList.Clear();


        //    switch (filter.ShowDialog())
        //    {
        //        case (DialogResult.Abort):
        //            break;
        //        case (DialogResult.Cancel):

        //            #region Cancel

        //            List<ILayer> layerlst = new List<ILayer>();

        //            if ((FeatureTreeView.Tag != null) && (FeatureTreeView.Tag is Dictionary<string, TreeNode>))
        //            {
        //                Dictionary<string, TreeNode> PdmObject_TreeNode = (Dictionary<string, TreeNode>)FeatureTreeView.Tag;

        //                foreach (var item in PdmObject_TreeNode)
        //                {
        //                    TreeNode nd = item.Value;
        //                    nd.Checked = true;
        //                }

        //            }
        //            foreach (PDMObject item in Environment.Data.PdmObjectList)
        //            {
        //                ILayer lyr = Environment.Data.GetLinkedLayer(item);

        //                if (lyr == null) return;
        //                if (layerlst.IndexOf(lyr) >= 0) continue;

        //                layerlst.Add(lyr);

        //                IFeatureLayer2 FL = (ESRI.ArcGIS.Carto.IFeatureLayer2)lyr;
        //                IFeatureLayerDefinition pFlyrDef = (IFeatureLayerDefinition)FL;
        //                pFlyrDef.DefinitionExpression = "";
        //            }


        //            layerlst = null;

        //            #endregion

        //            break;

        //        case (DialogResult.OK):

        //            #region OK


        //            if (filter.FilterList.Count > 0)
        //            {

        //                foreach (string filterStr in filter.FilterList)
        //                {
        //                    string sqlCmd = filterStr.Replace("OBJECTS", "FeatureGUID");
        //                    DataTable _Tbl = ARENA_DataReader.GetTable(sqlCmd);

        //                    if ((_Tbl != null) && (_Tbl.Rows.Count > 0))
        //                    {
        //                        foreach (DataRow DR in _Tbl.Rows)
        //                        {
        //                            string val = DR["FeatureGUID"].ToString();
        //                            if (FilteredIDs.IndexOf(val) < 0) FilteredIDs.Add(val);
        //                        }
        //                    }
        //                }

        //                //FilteredIDs = Util.LogicUtil.ApplyFilterList(filter.FilterList);

        //                if ((FeatureTreeView.Tag != null) && (FeatureTreeView.Tag is Dictionary<string, TreeNode>) && (FilteredIDs.Count >0))
        //                {
        //                    Dictionary<string, TreeNode> PdmObject_TreeNode = (Dictionary<string, TreeNode>)FeatureTreeView.Tag;

        //                    foreach (var item in PdmObject_TreeNode)
        //                    {
        //                        TreeNode nd = item.Value;
        //                        nd.Checked = FilteredIDs.IndexOf(item.Key) >= 0;
        //                    }

        //                }

        //            }
        //            #endregion

        //            break;

        //    }


            
        //    Environment.Data.CurrentFiltrsList.AddRange(filter.FilterList);
        //    FeatureTreeView.Refresh();

            

        //    filter = null;

        //}

        //private void ArincToolStripMenuItemClick(object sender, EventArgs e)
        //{
        //    var fn = Environment.MapControl.DocumentFilename;

        //    if (Environment.MapControl.CheckMxFile(fn))
        //    {

        //        var lyr = Environment.MapControl.get_Layer(0);
        //        var fc = ((IFeatureLayer)lyr).FeatureClass;
        //        ARENA.DataLoaders.IARENA_DATA_Converter _data_loader = new ARENA.DataLoaders.ARINC_DataConverter(Environment);
        //        _data_loader.Convert_Data(fc);

        //        if (Environment.Data.PdmObjectList.Count > 0)
        //        {
        //            FillObjectsTree();
        //            editToolStripMenuItem.Enabled = true;
        //        }
        //        Environment.SetCenter_and_Projection();
        //        Environment.SaveLog();

        //    }

            
        //}

        //private void loadObstacleDataFromExcellToolStripMenuItem_Click(object sender, EventArgs e)
        //{

        //    var frm = new DataViewerForm { Tag = null };

        //    string mdbName = Environment.Data.MapDocumentName.Replace(".mxd", ".mdb");

        //    mdbName = System.IO.Path.GetDirectoryName(mdbName) + @"\pdm.mdb";

        //    ARENA_DataReader.PathToPDM_DB = mdbName;

        //    string Caption = "";

        //    var lyr = Environment.MapControl.get_Layer(0);
        //    var fc = ((IFeatureLayer)lyr).FeatureClass;
        //    IWorkspaceEdit workspaceEdit = (IWorkspaceEdit)fc.FeatureDataset.Workspace;
        //    if (Environment.Data.TableDictionary.Count == 0)
        //    {
        //        Environment.FillAirtrackTableDic(workspaceEdit);
        //    }
        //    workspaceEdit.StartEditing(false);
        //    workspaceEdit.StartEditOperation();

        //    Control cntrl = new VerticalStructureLoader();
        //    ((VerticalStructureLoader)cntrl)._AIRTRACK_TableDic = Environment.Data.TableDictionary;
        //    ((VerticalStructureLoader)cntrl)._PdmObjectList = Environment.Data.PdmObjectList;

        //    cntrl.Dock = DockStyle.Fill;
        //    cntrl.Location = new System.Drawing.Point(0, 0);
        //    cntrl.Name = "DataView1";
        //    cntrl.Size = new Size(1130, 718);

        //    frm.SuspendLayout();
        //    frm.Controls.Add(cntrl);

        //    frm.ResumeLayout(false);
        //    frm.Text = Caption;
        //    frm.WindowState = FormWindowState.Maximized;

        //    frm.ShowDialog();
        //    FillObjectsTree();
        //    Environment.ClearGraphics();


        //    workspaceEdit.StopEditOperation();
        //    workspaceEdit.StopEditing(true);


        //    if (Environment.Data.PdmObjectList.Count > 0)
        //    {
        //        FillObjectsTree();
        //        editToolStripMenuItem.Enabled = true;
        //    }
        //    Environment.SetCenter_and_Projection();
        //    Environment.SaveLog();

        //    ((IActiveView)Environment.MapControl.Map).Refresh();
        //}

        //private void loadDataFromTOSSMToolStripMenuItem_Click(object sender, EventArgs e)
        //{
        //    var lyr = Environment.MapControl.get_Layer(0);
        //    var fc = ((IFeatureLayer)lyr).FeatureClass;
        //    ARENA.DataLoaders.IARENA_DATA_Converter _data_loader = new ARENA.DataLoaders.TOOSM_DataConverter(Environment);
        //    _data_loader.Convert_Data(fc);

        //    if (Environment.Data.PdmObjectList.Count > 0) FillObjectsTree();
        //    if (Environment.Data.PdmObjectList.Count > 0)
        //    {
        //        FillObjectsTree();
        //        editToolStripMenuItem.Enabled = true;
        //    }
        //    Environment.SetCenter_and_Projection();
        //    Environment.SaveLog();

        //}

        //private void loadARANAIXM51SnapshotToolStripMenuItem_Click(object sender, EventArgs e)
        //{
        //    var lyr = Environment.MapControl.get_Layer(0);
        //    var fc = ((IFeatureLayer)lyr).FeatureClass;
        //    ARENA.DataLoaders.IARENA_DATA_Converter _data_loader = new ARENA.DataLoaders.AIXM51_DataConverter(Environment);
        //    _data_loader.Convert_Data(fc);

        //    if (Environment.Data.PdmObjectList.Count > 0)
        //    {
        //        FillObjectsTree();
        //        editToolStripMenuItem.Enabled = true;
        //    }
        //    Environment.SetCenter_and_Projection();
        //    Environment.SaveLog();

        //}

        //private void loadAIXM45SnapshotToolStripMenuItem_Click(object sender, EventArgs e)
        //{
        //    var lyr = Environment.MapControl.get_Layer(0);
        //    var fc = ((IFeatureLayer)lyr).FeatureClass;
        //    ARENA.DataLoaders.IARENA_DATA_Converter _data_loader = new ARENA.DataLoaders.AIXM45_DataConverter(Environment);
        //    _data_loader.Convert_Data(fc);

        //    if (Environment.Data.PdmObjectList.Count > 0)
        //    {
        //        FillObjectsTree();
        //        editToolStripMenuItem.Enabled = true;
        //    }
        //    Environment.SetCenter_and_Projection();
        //    Environment.SaveLog();
        //}

        //private void DataToolStripMenuItemClick(object sender, EventArgs e)
        //{
        //    if (Environment.Data.TableDictionary.Count == 0)
        //    {
        //        var lyr = Environment.MapControl.get_Layer(0);
        //        var fc = ((IFeatureLayer)lyr).FeatureClass;
        //        var workspaceEdit = (IWorkspaceEdit)fc.FeatureDataset.Workspace;
        //        Environment.FillAirtrackTableDic(workspaceEdit);
        //    }

        //    var frm = new DataViewerForm { Tag = null };

        //    string mdbName = Environment.Data.MapDocumentName.Replace(".mxd", ".mdb");

        //    mdbName = System.IO.Path.GetDirectoryName(mdbName) + @"\pdm.mdb";

        //    ARENA_DataReader.PathToPDM_DB = mdbName;

        //    string Caption = ((ToolStripMenuItem)sender).Tag.ToString();
        //    Control cntrl = GetDataViewControl(Caption);


        //    frm.SuspendLayout();
        //    frm.Controls.Add(cntrl);

        //    frm.ResumeLayout(false);
        //    frm.Text = Caption;

        //    frm.ShowDialog();
        //    FillObjectsTree();

        //    Environment.ClearGraphics();
        //    Environment.MapControl.Map.ClearSelection();
        //    ((IActiveView)Environment.MapControl.Map).Refresh();
        //}

        //private Control GetDataViewControl(string ViewType)
        //{
        //    Control _DataView = null; ;

        //    try
        //    {
        //        switch (ViewType)
        //        {
        //            case ("Airport"):

        //                #region

        //                _DataView = new AirportDataView();

        //                _DataView.Dock = DockStyle.Fill;
        //                _DataView.Location = new System.Drawing.Point(0, 0);
        //                _DataView.Name = "DataView1";
        //                _DataView.Size = new Size(1130, 718);
        //                _DataView.TabIndex = 0;
        //                ((AirportDataView)_DataView)._AIRTRACK_TableDic = Environment.Data.TableDictionary;

        //                ((AirportDataView)_DataView).ARP_splitContainer.Panel2Collapsed = true;
        //                ((AirportDataView)_DataView).rwy_splitContainer.Panel2Collapsed = true;
        //                ((AirportDataView)_DataView).navaids_splitContainer.Panel2Collapsed = true;

        //                _DataView.Tag = Environment.Data.PdmObjectList;
        //                ((AirportDataView)_DataView).LoadData(false);
        //                #endregion

        //                break;

        //            case ("Navaid"):

        //                #region

        //                _DataView = new NavaidsDataView();

        //                _DataView.Dock = DockStyle.Fill;
        //                _DataView.Location = new System.Drawing.Point(0, 0);
        //                _DataView.Name = "DataView1";
        //                _DataView.Size = new Size(1130, 718);
        //                _DataView.TabIndex = 0;
        //                ((NavaidsDataView)_DataView)._AIRTRACK_TableDic = Environment.Data.TableDictionary;

        //                _DataView.Tag = Environment.Data.PdmObjectList;
        //                ((NavaidsDataView)_DataView).LoadData();

        //                #endregion

        //                break;

        //            case ("Waypoint"):

        //                #region

        //                _DataView = new DataTableViewControl();

        //                _DataView.Dock = DockStyle.Fill;
        //                _DataView.Location = new System.Drawing.Point(0, 0);
        //                _DataView.Name = "DataView1";
        //                _DataView.Size = new Size(1130, 718);
        //                _DataView.TabIndex = 0;

        //                ((DataTableViewControl)_DataView)._AIRTRACK_TableDic = Environment.Data.TableDictionary;

        //                _DataView.Tag = Environment.Data.PdmObjectList;
        //                var waypointTbl = ARENA_DataReader.Table_GetWaypointList();

        //                ((DataTableViewControl)_DataView).LoadData(waypointTbl);

        //                #endregion

        //                break;

        //            case ("EnRoute"):

        //                #region

        //                _DataView = new EnrouteDataView();

        //                _DataView.Dock = DockStyle.Fill;
        //                _DataView.Location = new System.Drawing.Point(0, 0);
        //                _DataView.Name = "DataView1";
        //                _DataView.Size = new Size(1130, 718);
        //                _DataView.TabIndex = 0;
        //                ((EnrouteDataView)_DataView)._AIRTRACK_TableDic = Environment.Data.TableDictionary;

        //                _DataView.Tag = Environment.Data.PdmObjectList;
        //                ((EnrouteDataView)_DataView).LoadData(true);
        //                #endregion

        //                break;

        //            case ("Airspace"):

        //                #region

        //                _DataView = new AirspaceDataView();

        //                _DataView.Dock = DockStyle.Fill;
        //                _DataView.Location = new System.Drawing.Point(0, 0);
        //                _DataView.Name = "DataView1";
        //                _DataView.Size = new Size(1130, 718);
        //                _DataView.TabIndex = 0;
        //                ((AirspaceDataView)_DataView)._AIRTRACK_TableDic = Environment.Data.TableDictionary;

        //                _DataView.Tag = Environment.Data.PdmObjectList;
        //                ((AirspaceDataView)_DataView).LoadData(true);

        //                #endregion

        //                break;

        //            case ("IAP"):
        //            case ("SID"):
        //            case ("STAR"):

        //                #region

        //                _DataView = new ProcedureListDataView();

        //                _DataView.Dock = DockStyle.Fill;
        //                _DataView.Location = new System.Drawing.Point(0, 0);
        //                _DataView.Name = ViewType;
        //                _DataView.Size = new Size(1130, 718);
        //                _DataView.TabIndex = 0;
        //                ((ProcedureListDataView)_DataView).splitContainerLegs.Panel2Collapsed = true;
        //                ((ProcedureListDataView)_DataView)._AIRTRACK_TableDic = Environment.Data.TableDictionary;

        //                _DataView.Tag = Environment.Data.PdmObjectList;

        //                ((ProcedureListDataView)_DataView).LoadData(true);

        //                #endregion

        //                break;

        //            case ("VerticalStructure"):

        //                #region

        //                _DataView = new VerticalStructureDataView();

        //                _DataView.Dock = DockStyle.Fill;
        //                _DataView.Location = new System.Drawing.Point(0, 0);
        //                _DataView.Name = "DataView1";
        //                _DataView.Size = new Size(1130, 718);

        //                _DataView.TabIndex = 0;
        //                ((VerticalStructureDataView)_DataView)._AIRTRACK_TableDic = Environment.Data.TableDictionary;

        //                _DataView.Tag = Environment.Data.PdmObjectList;

        //                ((VerticalStructureDataView)_DataView).LoadData(true);

        //                #endregion

        //                break;
        //        }


        //    }
        //    catch (Exception ex)
        //    {
        //        System.Diagnostics.Debug.WriteLine(ex.StackTrace + " ERROR " + ex.Message);

        //    }

        //    return _DataView;
        //}


        //private void ProjectSettingsMenu_Click(object sender, EventArgs e)
        //{
  
        //    var frm = new InputForm
        //    {
        //        LinkedObject = ((ArenaProject)Environment.Data.CurrentProject).ProjectSettings!=null? ((ArenaProject)Environment.Data.CurrentProject).ProjectSettings : new Settings.ArenaSettings(),
        //    };
        //    frm.ShowDialog();
        //}

    }
}
