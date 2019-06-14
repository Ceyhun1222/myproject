using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Xml.Serialization;
///////////////////////////////////////////
using ARENA.Project;
using ARENA.Util;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
////////////////////////////////////////////

using ARINC_DECODER_CORE.AIRTRACK_Objects;
using ARINC_DECODER_CORE;
using Accent.MapElements;
using EsriWorkEnvironment;
using PDM;
using System.Data;
using ZzArchivator;
using System.Threading;
using System.ComponentModel;
using Aran.Temporality.CommonUtil.Context;
using Aran.Temporality.Common.Aim.Extension.Property;
using Aran.Temporality.Common.ArcGis;
using Aran.Aim.PropertyEnum;
using Aran.Temporality.Common.Aim.MetaData;
using ESRI.ArcGIS.Display;



namespace ARENA
{


    public sealed partial class MainForm : Form
    {

        public static MainForm Instance;

        public Environment.Environment Environment;

    
        #region Class constructor
        public MainForm()
        {
            InitializeComponent();
            Instance = this;
        }
        #endregion

        #region On Load Form 

        private void InitEnvironment()
        {

            Environment = new Environment.Environment
            {
                PlayButton = PlayButton,
                FeatureTreeView = treeView1,
                MapControl = axMapControl1,
                ReadOnlyPropertyGrid = readOnlyPropertyGrid,
                TreeViewImageList = TreeViewImageList,
                FeatureTreeViewContextMenuStrip = FeatureTreeViewContextMenu,
                EnvironmentToolStrip = CustomToolStrip,
                mapControlContextMenuStrip = mapControlContextMenuStrip,
                FeatureTreeViewToolStrip = FeatureTreeViewToolStrip,
                statusStrip = statusStrip,
                MaimMenu = menuStrip1,
                PandaToolBox = panel3,
            };

        }

        private void InitUi()
        {
            //get the MapControl
            axTOCControl1.SetBuddyControl(axMapControl1);

            //disable the Save menu (since there is no document yet)
            menuSaveDoc.Enabled = false;
            menuSaveDocAS.Enabled = false;

            comboBox1.SelectedIndex = 0;
            splitContainer2.Panel2Collapsed = true;
            nOTAMToolStripMenuItem.Enabled = NotamProject.RunNotamConfig != null;

            treeView1.Tag = new Dictionary<string, TreeNode>();
            Text = Application.ProductName+" v:"+Application.ProductVersion;
        }

        private void MainFormLoad(object sender, EventArgs e)
        {

            InitEnvironment();
            InitUi();
            ClearTempFolder();

            //if (this.Tag != null)
            //{
            //    OpenSelectedFile((string)this.Tag);
            //}
        }

        private void ClearTempFolder()
        {
            var tempDirName = System.IO.Path.GetTempPath();
            DirectoryInfo dInf = System.IO.Directory.CreateDirectory(tempDirName + @"\PDM");
            tempDirName = dInf.FullName;

            string[] dirs = Directory.GetDirectories(tempDirName, "*.*", SearchOption.AllDirectories);

            for (int i = dirs.Length - 1; i >= 0; i--)
            {
                string dirname = dirs[i];
                Static_Proc.DeleteFilesFromDirectory(dirname + @"\");

                Directory.Delete(dirname);
            }


            Static_Proc.DeleteFilesFromDirectory(tempDirName + @"\");

            Directory.Delete(tempDirName);
        }

        #endregion

        #region Main Menu event handlers

        private void MenuOpenDocClick(object sender, EventArgs e)
        {
            
            var openFileDialog1 = new OpenFileDialog {Filter = @"Panda type files (*.pdm)|*.pdm"};
            if (openFileDialog1.ShowDialog() != DialogResult.OK) return;

            OpenSelectedFile(openFileDialog1.FileName);

        }

        private void OpenSelectedFile(string _FileName)
        {
            var tempDirName = System.IO.Path.GetTempPath();
            var dInf = Directory.CreateDirectory(tempDirName + @"\PDM\" + System.IO.Path.GetFileNameWithoutExtension(_FileName));
            tempDirName = dInf.FullName;
            var tempPdmFilename = System.IO.Path.Combine(tempDirName, "pdm.pdm");
            var tempMxdFilename = System.IO.Path.Combine(tempDirName, "pdm.mxd");
            //string zoomedLayerName = "AirportHeliport";


            AlertForm alrtForm = new AlertForm();
            alrtForm.FormBorderStyle = FormBorderStyle.None;
            alrtForm.Opacity = 0.5;
            alrtForm.BackgroundImage = ARENA.Properties.Resources.ArenaSplash;
            alrtForm.TopMost = true;

            if (!System.Diagnostics.Debugger.IsAttached) alrtForm.Show();
           
            ZzArchivatorClass.ExtractFromArchive(Static_Proc.GetPathToTemplateFile() + @"\Utils\7z.exe", _FileName, tempDirName);

            Environment.Data.PdmObjectList.Clear();

            Environment.Data.PdmObjectList.AddRange(GetObjectsFromPdmFile(tempDirName));

            switch (Environment.Data.CurrentProjectType)
            {
                case (ArenaProjectType.ARENA):
                case (ArenaProjectType.PANDA):
                    Environment.Data.CurrentProject = new ArenaProject(Environment);
                    tempMxdFilename = System.IO.Path.Combine(tempDirName, "arena_PDM.mxd");

                    try
                    {
                        ((ArenaProject)Environment.Data.CurrentProject).ProjectSettings = GetProjectSettings(tempDirName);
                    }
                    catch { }


                    break;

                case (ArenaProjectType.NOTAM):
                    Environment.Data.CurrentProject = new NotamProject(Environment);
                    tempMxdFilename = System.IO.Path.Combine(tempDirName, "notam_PDM.mxd");
                    //zoomedLayerName = "AirspaceVolume";
                    break;
            }


            if (axMapControl1.CheckMxFile(tempMxdFilename))
            {

                axMapControl1.LoadMxFile(tempMxdFilename);
                if (Environment.Data.PdmObjectList.Count > 0) FillObjectsTree();

                menuSaveAs.Enabled = true;
                Environment.Data.CurProjectName = _FileName;//System.IO.Path.GetFileNameWithoutExtension(_FileName);

                ILayer _Layer = EsriUtils.getLayerByName(Environment.mapControl.Map, "AirportHeliport");


                if (Environment.Data.TableDictionary.Count == 0)
                {
                    var fc = ((IFeatureLayer)_Layer).FeatureClass;
                    var workspaceEdit = (IWorkspaceEdit)fc.FeatureDataset.Workspace;

                    Environment.FillAirtrackTableDic(workspaceEdit);
                }

                SetWindowTitle();

                
                alrtForm.Close();

            }
        }

        private Settings.ArenaSettings GetProjectSettings(string tempDirName)
        {
            string _file = tempDirName + @"\ProjectSettings";

            if (!File.Exists(_file)) return new Settings.ArenaSettings();
            var fs = new System.IO.FileStream(_file, FileMode.Open);
            var byteArr = new byte[fs.Length];
            fs.Position = 0;
            var count = fs.Read(byteArr, 0, byteArr.Length);
            if (count != byteArr.Length)
            {
                fs.Close();
                Console.WriteLine(@"Test Failed: Unable to read data from file");
            }


            var strmMemSer = new MemoryStream();
            strmMemSer.Write(byteArr, 0, byteArr.Length);
            strmMemSer.Position = 0;


            var xmlSer = new XmlSerializer(typeof(Settings.ArenaSettings));
            var prj = (Settings.ArenaSettings)xmlSer.Deserialize(strmMemSer);
            fs.Close();
            strmMemSer.Close();
            fs.Dispose();
            strmMemSer.Dispose();

            return prj;
        }

        private List<PDMObject> GetObjectsFromPdmFile(string fileName)
        {
            List<PDMObject> res = new List<PDMObject>();
            string[] FN = Directory. GetFiles(fileName, "*.pdm");

            for (int i = FN.Length; i > 0; i--)
            {
                string _file = FN[i-1];
                var fs = new System.IO.FileStream(_file, FileMode.Open);
                var byteArr = new byte[fs.Length];
                fs.Position = 0;
                var count = fs.Read(byteArr, 0, byteArr.Length);
                if (count != byteArr.Length)
                {
                    fs.Close();
                    Console.WriteLine(@"Test Failed: Unable to read data from file");
                }


                var strmMemSer = new MemoryStream();
                strmMemSer.Write(byteArr, 0, byteArr.Length);
                strmMemSer.Position = 0;


                var xmlSer = new XmlSerializer(typeof(PDM_ObjectsList));
                var prj = (PDM_ObjectsList)xmlSer.Deserialize(strmMemSer);
                if ((prj.PDMObject_list != null) && (prj.PDMObject_list.Count > 0)) res.AddRange(prj.PDMObject_list);

                if ((prj.ProjectType != null) && (prj.ProjectType.Length > 0)) Environment.Data.CurrentProjectType = (ArenaProjectType)Enum.Parse(typeof(ArenaProjectType), prj.ProjectType, true);
                if ((prj.FilterList != null) && (prj.FilterList.Count > 0)) Environment.Data.CurrentFiltrsList.AddRange(prj.FilterList);

                fs.Close();
                strmMemSer.Close();
                fs.Dispose();
                strmMemSer.Dispose();
            }







            return res;
        }

        private void saveProject(string ProjectName)
        {
            var tempDirName = System.IO.Path.GetTempPath();
            string mxdName = Environment.Data.MapDocumentName; ;
            string projName = System.IO.Path.GetDirectoryName(mxdName) + @"\pdm.pdm";
            string mdbName = System.IO.Path.GetDirectoryName(mxdName) + @"\pdm.mdb";

            DirectoryInfo dInf = System.IO.Directory.CreateDirectory(tempDirName + @"\PDM\" + Guid.NewGuid().ToString());
            tempDirName = dInf.FullName;

            PDM_ObjectsList Tmp_PdmObjectList = new PDM_ObjectsList(Environment.Data.PdmObjectList, Environment.Data.CurrentProjectType);
            if ((Environment.Data.CurrentFiltrsList != null) && (Environment.Data.CurrentFiltrsList.Count > 0))
            {
                Tmp_PdmObjectList.FilterList = new List<string>();
                Tmp_PdmObjectList.FilterList.AddRange(Environment.Data.CurrentFiltrsList);
            }


            string[] FN = Directory.GetFiles(System.IO.Path.GetDirectoryName(projName), "*.pdm");
            foreach (var fl in FN)
            {
                System.IO.File.Delete(fl);
            }

            #region PDM objects files

            if (Tmp_PdmObjectList.PDMObject_list.Count > 0)
            {
                int ObjCount = Tmp_PdmObjectList.PDMObject_list.Count;
                int Size = 1000;

                int Steps = ObjCount / Size;
                int part = ObjCount - Steps * Size;
                int index = 1;

                PDMObject[] newList = new PDMObject[part];
                Tmp_PdmObjectList.PDMObject_list.CopyTo(0, newList, 0, part);
                PDM_ObjectsList ResPart = new PDM_ObjectsList(newList.ToList(), Environment.Data.CurrentProjectType);

                Static_Proc.Serialize(ResPart, projName);
                index++;

                for (int i = 0; i <= Steps - 1; i++)
                {
                    newList = new PDMObject[Size];
                    Tmp_PdmObjectList.PDMObject_list.CopyTo(part + Size * i, newList, 0, Size);
                    ResPart = new PDM_ObjectsList(newList.ToList(), Environment.Data.CurrentProjectType);
                    string newFN = projName.Replace("pdm.pdm", (i + 1).ToString() + "pdm.pdm");
                    Static_Proc.Serialize(ResPart, newFN);
                    index++;
                }
                newList = null;
                ResPart = null;

            }
            Tmp_PdmObjectList = null;

            #endregion


            string DestFolder = System.IO.Path.GetDirectoryName(ProjectName);

            #region MXD File

            //create a new instance of a MapDocument
            IMapDocument mapDoc = new MapDocumentClass();
            mapDoc.Open(mxdName, string.Empty);

            //Make sure that the MapDocument is not readonly
            if (mapDoc.get_IsReadOnly(mxdName))
            {
                MessageBox.Show("Map document is read only!");
                mapDoc.Close();
                return;
            }

            //Replace its contents with the current map
            mapDoc.ReplaceContents((IMxdContents)Environment.MapControl.Map);

            //save the MapDocument in order to persist it
            mapDoc.Save(mapDoc.UsesRelativePaths, false);

            //close the MapDocument
            mapDoc.Close();

            System.IO.File.Copy(mxdName, System.IO.Path.Combine(tempDirName, System.IO.Path.GetFileName(mxdName)), true);


            #endregion


            try
            {
                #region Settings
                if (Environment.Data.CurrentProject.ProjectType != ArenaProjectType.NOTAM)

                    Static_Proc.Serialize(((ArenaProject)Environment.Data.CurrentProject).ProjectSettings, tempDirName + @"\ProjectSettings");
                #endregion
            }
            catch { }

            FN = Directory.GetFiles(System.IO.Path.GetDirectoryName(projName), "*.pdm");
            foreach (var fl in FN)
            {
                System.IO.File.Copy(fl, System.IO.Path.Combine(tempDirName, System.IO.Path.GetFileName(fl)), true);
            }


            System.IO.File.Copy(mdbName, System.IO.Path.Combine(tempDirName, System.IO.Path.GetFileName(mdbName)), true);

            string sCompressedFile = ProjectName;

            ZzArchivatorClass.AddToArchive(Static_Proc.GetPathToTemplateFile() + @"\Utils\7z.exe", tempDirName, sCompressedFile);

            Static_Proc.DeleteFilesFromDirectory(tempDirName + @"\");

            Directory.Delete(tempDirName);

            Environment.Data.CurProjectName = System.IO.Path.GetFileNameWithoutExtension(ProjectName);
            menuSaveAs.Enabled = true;

            SetWindowTitle();


            MessageBox.Show(@"Saved");
        }

        private void MenuSaveDocClick(object sender, EventArgs e)
        {
            if (!Environment.mapControl.CheckMxFile(Environment.Data.MapDocumentName)) return;

            string projname = "";

            if (Environment.Data.CurProjectName == null)
            {

                var saveFileDialog1 = new SaveFileDialog
                {
                    Filter = @"Panda type files (*.pdm)|*.pdm",
                    RestoreDirectory = true
                };

                if (saveFileDialog1.ShowDialog() != DialogResult.OK) return;
                projname = saveFileDialog1.FileName;

            }
            else
                projname = Environment.Data.CurProjectName;


            saveProject(projname);

 
        }

        private void SetWindowTitle()
        {
            this.Text = Application.ProductName + " v:" + Application.ProductVersion + " " + Environment.Data.CurrentProjectType.ToString() + " " + Environment.Data.CurProjectName;

        }

        private void MenuSaveAsClick(object sender, EventArgs e)
        {
            var folderBrowserDialog1 = new FolderBrowserDialog
                                           {
                                               ShowNewFolderButton = true
                                           };
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK )
            {
                var mxdName = Environment.Data.MapDocumentName;
                var mdbName = Environment.Data.MapDocumentName.Replace(".mxd", ".mdb");

                mdbName = System.IO.Path.GetDirectoryName(mxdName) + @"\pdm.mdb";


                var destFolder = System.IO.Path.Combine(folderBrowserDialog1.SelectedPath, Environment.Data.CurProjectName);
                if (Directory.Exists(destFolder))  Directory.Delete(destFolder,true);
                Directory.CreateDirectory(destFolder);
                File.Copy(mxdName, System.IO.Path.Combine(destFolder, System.IO.Path.GetFileName(mxdName)));
                File.Copy(mdbName, System.IO.Path.Combine(destFolder, System.IO.Path.GetFileName(mdbName)));

                MessageBox.Show(@"Saved");
            }

        }

        private void MenuExitAppClick(object sender, EventArgs e)
        {
            //exit the application
            Application.Exit();
        }

        #endregion

        //listen to MapReplaced evant in order to update the statusbar and the Save menu
        private void axMapControl1_OnMapReplaced(object sender, IMapControlEvents2_OnMapReplacedEvent e)
        {
            //get the current document name from the MapControl
            Environment.Data.MapDocumentName = Environment.mapControl.DocumentFilename;

            //if there is no MapDocument, diable the Save menu and clear the statusbar
            if (Environment.Data.MapDocumentName == string.Empty)
            {
                menuSaveDoc.Enabled = false;
                menuSaveDocAS.Enabled = false;
                statusBarXY.Text = string.Empty;
            }
            else
            {
                //enable the Save manu and write the doc name to the statusbar
                menuSaveDoc.Enabled = true;
                menuSaveDocAS.Enabled = true;
                statusBarXY.Text = System.IO.Path.GetFileName(Environment.Data.MapDocumentName);
            }
        }

        private void axMapControl1_OnMouseMove(object sender, IMapControlEvents2_OnMouseMoveEvent e)
        {
            statusBarXY.Text = string.Format("{0}, {1}  {2}", e.mapX.ToString("#######.##"), e.mapY.ToString("#######.##"), axMapControl1.MapUnits.ToString().Substring(4));
        }

        private void Button1Click(object sender, EventArgs e)
        {
            splitContainer2.Panel2Collapsed = !splitContainer2.Panel2Collapsed;

            button1.Text = splitContainer2.Panel2Collapsed ? @"<" : @">";
        }

        private void FillObjectsTree()
        {
            if (Environment.Data.CurrentProject == null) return;
            Environment.Data.CurrentProject.FillObjectsTree();
        }

        public void DeleteFeatures(IFeatureClass featureClass)
        {
            IQueryFilter queryFilter = new QueryFilterClass();
            queryFilter.WhereClause = "OBJECTID >0";

            // Use IFeatureClass.Update to populate IFeatureCursor.
            var updateCursor = featureClass.Update(queryFilter, false);

            try
            {
                while ((updateCursor.NextFeature()) != null)
                {
                    updateCursor.DeleteFeature();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.StackTrace + " ERROR " + ex.Message);

            }

            // If the cursor is no longer needed, release it.
            Marshal.ReleaseComObject(updateCursor);
        }

        private void ARenaToolStripMenuItemClick(object sender, EventArgs e)
        {
            if ((axMapControl1.LayerCount > 0) && MessageBox.Show("Save current project?", "Arena", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.Yes)
            {
                MenuSaveDocClick(sender, e);
            }

          
            Environment.CreateEmptyProject(ArenaProjectType.ARENA);
            //loadDataToolStripMenuItem.Enabled = true;

            SetWindowTitle();

        }

        private void pANDAToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Environment.CreateEmptyProject(ArenaProjectType.PANDA);
            //loadDataToolStripMenuItem.Enabled = true;
            SetWindowTitle();
            
        }

        private void LinkLabel1LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            panel1.Visible = !panel1.Visible;
        }

        private void ComboBox1SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((Environment.Data.PdmObjectList == null) || (Environment.Data.PdmObjectList.Count <= 0)) return;

            comboBox2.Items.Clear();
            comboBox2.Text = "";
            List<PDMObject> objList;

            switch (comboBox1.SelectedIndex)
            {
                case(1):
                    objList = (from element in Environment.Data.PdmObjectList where (element != null) && (element is AirportHeliport) select element).ToList();
                    foreach(AirportHeliport adhp in objList) 
                        comboBox2.Items.Add(adhp.Designator);
                    break;

                case (2):
                    objList = (from element in Environment.Data.PdmObjectList where (element != null) && (element is AirportHeliport) select element).ToList();
                     foreach (AirportHeliport adhp in objList)
                         foreach (Runway rwy in adhp.RunwayList)
                             foreach (RunwayDirection rdn in rwy.RunwayDirectionList)
                                 foreach (NavaidSystem nvds in rdn.Related_NavaidSystem)
                                     comboBox2.Items.Add(nvds.Designator);
                     objList = (from element in Environment.Data.PdmObjectList where (element != null) && (element is NavaidSystem) select element).ToList();
                     foreach (NavaidSystem nvds in objList)
                         comboBox2.Items.Add(nvds.Designator);
                    break;

                case (3):
                    objList = (from element in Environment.Data.PdmObjectList where (element != null) && (element is WayPoint) select element).ToList();
                     foreach (WayPoint wyp in objList)
                         comboBox2.Items.Add(wyp.Designator);              
                    break;
                case (4):
                    objList = (from element in Environment.Data.PdmObjectList where (element != null) && (element is VerticalStructure) select element).ToList();
                    foreach (VerticalStructure obs in objList)
                        comboBox2.Items.Add(obs.Name);
                    break;

                default:
                    comboBox2.Text = "";
                    break;

            }

        }

        private void Button2Click1(object sender, EventArgs e)
        {
            try
            {

                int nodeIndx;
                TreeNode[] res = null;
                TreeNode[] parentNode;
                switch (comboBox1.SelectedIndex)
                {
                    case 1:
                        parentNode = treeView1.Nodes.Find("ARP/RWY/RDN/ILS", true);
                        nodeIndx = treeView1.Nodes.IndexOf(parentNode[0]);
                        res = treeView1.Nodes[nodeIndx].Nodes.Find(comboBox2.Text, true);
                        break;
                    case 2:
                        parentNode = treeView1.Nodes.Find("Navaids", true);
                        nodeIndx = treeView1.Nodes.IndexOf(parentNode[0]);
                        res = treeView1.Nodes[nodeIndx].Nodes.Find(comboBox2.Text, true);
                        if (res.Length == 0)
                        {
                            //NodeIndx = 0;
                            parentNode = treeView1.Nodes.Find("ARP/RWY/RDN/ILS", true);
                            nodeIndx = treeView1.Nodes.IndexOf(parentNode[0]);
                            res = treeView1.Nodes[nodeIndx].Nodes.Find(comboBox2.Text, true);
                        }
                        break;
                    case 3:
                        parentNode = treeView1.Nodes.Find("WayPoints", true);
                        nodeIndx = treeView1.Nodes.IndexOf(parentNode[0]);
                        res = treeView1.Nodes[nodeIndx].Nodes.Find(comboBox2.Text, true);
                        break;
                    case 4:
                        parentNode = treeView1.Nodes.Find("VerticalStructure", true);
                        nodeIndx = treeView1.Nodes.IndexOf(parentNode[0]);
                        res = treeView1.Nodes[nodeIndx].Nodes.Find(comboBox2.Text, true);
                        break;
                }

                if ((res == null) || (res.Length <= 0)) return;

                treeView1.SelectedNode = res[0];
                treeView1.SelectedNode.Expand();

                var sellObj = (PDMObject)treeView1.SelectedNode.Tag;

                if (sellObj.Geo == null) LogicUtil.FillGeo(sellObj);
                Environment.ClearGraphics();

                var lyr = Environment.Data.GetLinkedLayer(sellObj);
                ShowObjectInfo(sellObj, treeView1.SelectedNode.Text, lyr);

                ((IActiveView)axMapControl1.Map).PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);

                readOnlyPropertyGrid.SelectedObject = treeView1.SelectedNode.Tag;
                treeView1.Select();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.StackTrace + " ERROR " + ex.Message);
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
                var lyr = axMapControl1.get_Layer(0);
                var fc = (lyr as IFeatureLayer).FeatureClass;
                Environment.Data.SpatialReference = (fc as IGeoDataset).SpatialReference;
                List<string>  ids = new List<string>();
                List<string> id = new List<string>();
                
                
                #region Show Annotation

                //var LinkedLayer = Environment.Data.GetLinkedLayer(sellObj);
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
                        pntPrj = EsriUtils.ToProject(pntPrj, axMapControl1.Map, Environment.Data.SpatialReference) as IPoint;

                        AnnotationUtil.CreateAnnoInfo(Environment.MapControl, pntPrj, infoText, true);
                        id = new List<string> {sellObj.ID};
                        ids = new List<string>();
                        ids.Add(sellObj.ID);


                        ShowOnMap(LinkedLayer, ids);

                        #endregion

                        break;

                    case ("NavaidSystem"):

                        #region 
                        {
                            double sc = (axMapControl1.Map.MapScale * 100000) / 9000000;

                            foreach (PDMObject comp in ((NavaidSystem)sellObj).Components)
                            {
                                pntGeo = comp.Geo as IPoint;
                                pntPrj = new PointClass();
                                pntPrj.PutCoords(pntGeo.X, pntGeo.Y);
                                pntPrj = EsriUtils.ToProject(pntPrj, axMapControl1.Map, Environment.Data.SpatialReference) as IPoint;

                                AnnotationUtil.CreateAnnoInfo(Environment.MapControl, pntPrj, infoText + "(" + comp.GetType().Name + ")", false, sc);

                                ids.Add(comp.ID);


                                //sc = sc + 100000;
                            }
                            //ShowOnMap(LinkedLayer, ids);


                        }

                        #endregion

                        break;

                    case ("RouteSegment"):

                        #region 

                        lnGeo = (IPolyline)(sellObj.Geo);
                        linePrj = new PolylineClass();
                        linePrj.FromPoint = lnGeo.FromPoint;
                        linePrj.ToPoint = lnGeo.ToPoint;
                        linePrj = EsriUtils.ToProject(linePrj, axMapControl1.Map, Environment.Data.SpatialReference) as IPolyline;

                        AnnotationUtil.CreateAnnoInfo(Environment.MapControl, linePrj, infoText, true);
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
                            linePrj = EsriUtils.ToProject(linePrj, axMapControl1.Map, Environment.Data.SpatialReference) as IPolyline;

                            AnnotationUtil.CreateAnnoInfo(Environment.MapControl, linePrj, seg.GetObjectLabel(), false);
                            ids.Add(seg.ID);

                        }

                        ShowOnMap(LinkedLayer, ids);

                        #endregion

                        break;

                    case ("AirspaceVolume"):
                    case("FacilityMakeUp"):
                        #region 
                        var poly = (sellObj.Geo as IPointCollection);
                        AnnotationUtil.CreateAnnoInfo(Environment.MapControl, poly, true, Environment.Data.SpatialReference);

                        id = new List<string> {sellObj.ID};

                        ShowOnMap(LinkedLayer, id);
                        #endregion

                        break;

                    case ("Airspace"):

                        #region 
                        
                        var lyrVol = Environment.Data.GetLinkedLayer(sellObj);
                        ids = new List<string>();
                        foreach (var vol in ((Airspace) sellObj).AirspaceVolumeList)
                        {
                            AnnotationUtil.CreateAnnoInfo(Environment.MapControl, (vol.Geo as IPointCollection), false, Environment.Data.SpatialReference);

                            ids.Add(vol.ID);
                        }

                        ShowOnMap(lyrVol, ids);

                        #endregion

                        break;

                    case ("VerticalStructurePart"):

                        #region 
                        if (sellObj.Geo != null)
                        {
                            ids = new List<string> { sellObj.ID };

                            ShowOnMap(LinkedLayer, ids);
                            CreateObstacleAnnotation(sellObj.Geo, infoText,true);

                        }
                        #endregion

                        break;

                    case ("VerticalStructure"):

                        #region 

                        foreach (var item in ((VerticalStructure)sellObj).Parts)
                        {
                            ids = new List<string> { item.ID };
                            lyr = Environment.Data.GetLinkedLayer(item);
                            ShowOnMap(lyr, ids);
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

                                lnGeo = (IPolyline)(_leg.Geo);
                                linePrj = new PolylineClass();
                                linePrj.FromPoint = lnGeo.FromPoint;
                                linePrj.ToPoint = lnGeo.ToPoint;
                                linePrj = EsriUtils.ToProject(linePrj, axMapControl1.Map, Environment.Data.SpatialReference) as IPolyline;

                                AnnotationUtil.CreateAnnoInfo(Environment.MapControl, linePrj, _leg.LegTypeARINC.ToString(), false);
                                ids.Add(_leg.ID);
                            }
                        }
                        ShowOnMap(LinkedLayer, ids);

                        break;

                    case ("ProcedureTransitions"):
                        foreach (var _leg in ((ProcedureTransitions)sellObj).Legs)
                        {
                            lnGeo = (IPolyline)(_leg.Geo);
                            linePrj = new PolylineClass();
                            linePrj.FromPoint = lnGeo.FromPoint;
                            linePrj.ToPoint = lnGeo.ToPoint;
                            linePrj = EsriUtils.ToProject(linePrj, axMapControl1.Map, Environment.Data.SpatialReference) as IPolyline;

                            AnnotationUtil.CreateAnnoInfo(Environment.MapControl, linePrj, _leg.LegTypeARINC.ToString(), false);
                            ids.Add(_leg.ID);
                        }

                        ShowOnMap(LinkedLayer, ids);

                        break;
                        
                    case ("FinalLeg"):
                    case ("ProcedureLeg"):
                    case ("MissaedApproachLeg"):
                        //ids.Add(sellObj.ID);
                        lnGeo = (IPolyline)(sellObj.Geo);
                        linePrj = new PolylineClass();
                        linePrj.FromPoint = lnGeo.FromPoint;
                        linePrj.ToPoint = lnGeo.ToPoint;
                        linePrj = EsriUtils.ToProject(linePrj, axMapControl1.Map, Environment.Data.SpatialReference) as IPolyline;

                        AnnotationUtil.CreateAnnoInfo(Environment.MapControl, linePrj, ((ProcedureLeg)sellObj).LegTypeARINC.ToString(), false);
                        ids.Add(sellObj.ID);

                        ShowOnMap(LinkedLayer, ids);

                        #endregion

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

        private void CreateObstacleAnnotation(IGeometry iGeometry, string infoText, bool cleatGrahics)
        {
            IPoint pntGeo;
            IPoint pntPrj;
            IPolyline lnGeo;
            IPolyline linePrj;

            switch (iGeometry.GeometryType)
            {

                case (esriGeometryType.esriGeometryPoint):
                    pntGeo = iGeometry as IPoint;
                    pntPrj = new PointClass();
                    pntPrj.PutCoords(pntGeo.X, pntGeo.Y);
                    pntPrj = EsriUtils.ToProject(pntPrj, axMapControl1.Map, Environment.Data.SpatialReference) as IPoint;
                    AnnotationUtil.CreateAnnoInfo(Environment.MapControl, pntPrj, infoText, cleatGrahics);
                    break;

                case (esriGeometryType.esriGeometryLine):
                case (esriGeometryType.esriGeometryPolyline):
                    lnGeo = (IPolyline)(iGeometry);
                    linePrj = new PolylineClass();
                    linePrj.FromPoint = lnGeo.FromPoint;
                    linePrj.ToPoint = lnGeo.ToPoint;
                    linePrj = EsriUtils.ToProject(linePrj, axMapControl1.Map, Environment.Data.SpatialReference) as IPolyline;
                    AnnotationUtil.CreateAnnoInfo(Environment.MapControl, linePrj, infoText, cleatGrahics);
                    break;

                case (esriGeometryType.esriGeometryPolygon):
                    pntGeo = ((IArea)iGeometry).Centroid;
                    pntPrj = new PointClass();
                    pntPrj.PutCoords(pntGeo.X, pntGeo.Y);
                    pntPrj = EsriUtils.ToProject(pntPrj, axMapControl1.Map, Environment.Data.SpatialReference) as IPoint;
                    AnnotationUtil.CreateAnnoInfo(Environment.MapControl, pntPrj, infoText, cleatGrahics);
                    break;

            }

        }

        private void ShowOnMapToolStripMenuItemClick(object sender, EventArgs e)
        {
            try
            {
                if ((Environment.Data.PdmObjectList == null) || (Environment.Data.PdmObjectList.Count <= 0)) return;
                if (treeView1.SelectedNode == null) return;
                if (treeView1.SelectedNode.Tag == null) return;

                var sellObj = (PDMObject) treeView1.SelectedNode.Tag;
                if (sellObj.Geo == null) LogicUtil.FillGeo(sellObj);

                var lyr = Environment.Data.GetLinkedLayer(sellObj);
                if ((sellObj is NavaidSystem) && (lyr == null))
                    lyr = Environment.Data.GetLinkedLayer(((NavaidSystem) sellObj).Components[0]);

                //((IActiveView)axMapControl1.Map).Extent = lyr.AreaOfInterest;
                
                Environment.ClearGraphics();

                //HighLightObject(sellObj, lyr);

                ShowObjectInfo(sellObj, treeView1.SelectedNode.Text, lyr);

                
                ((IActiveView) axMapControl1.Map).Refresh(); 
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.StackTrace + " ERROR " + ex.Message);
            }
        }

        private void HighLightObject(PDMObject sellObj, ILayer lyr)
        {
            IFeatureSelection pSelect = lyr as IFeatureSelection;

            if (pSelect == null) return;

            pSelect.CombinationMethod = esriSelectionResultEnum.esriSelectionResultAdd;

            IQueryFilter queryFilter = new QueryFilterClass();
            queryFilter.WhereClause = "FEATUREGUID = '" + sellObj.ID+"'";
            if (pSelect != null)
            {
                //IColor selColor = new RgbColor();
                //((RgbColor)selColor).Red = 255;
                //((RgbColor)selColor).Green = 0;
                //((RgbColor)selColor).Blue = 0;

                //ISimpleFillSymbol smplFill1 = new SimpleFillSymbol();

                //smplFill1.Style = esriSimpleFillStyle.esriSFSSolid;

                //ISimpleLineSymbol pSimpleLine1 = new SimpleLineSymbolClass();

                //pSimpleLine1.Style = esriSimpleLineStyle.esriSLSSolid;
                //pSimpleLine1.Color = selColor;
                //pSimpleLine1.Width = 5;
                //smplFill1.Outline = pSimpleLine1;

                //smplFill1.Color = selColor;


                //pSelect.SetSelectionSymbol = true;

                //pSelect.SelectionSymbol = (ISymbol)smplFill1;

                //if (MultiSelect) pSelect.SelectFeatures(queryFilter, esriSelectionResultEnum.esriSelectionResultAdd, false);
                //else 
                    
                pSelect.SelectFeatures(queryFilter, esriSelectionResultEnum.esriSelectionResultNew, false);

            }

        }

        private void ZoomToObjectToolStripMenuItemClick(object sender, EventArgs e)
        {
            if ((Environment.Data.PdmObjectList == null) || (Environment.Data.PdmObjectList.Count <= 0)) return;
            if (treeView1.SelectedNode == null) return;
            if (treeView1.SelectedNode.Tag == null) return;

            var sellObj = (PDMObject)treeView1.SelectedNode.Tag;

            var lyr = Environment.Data.GetLinkedLayer(sellObj);
            if ((sellObj is NavaidSystem) && (lyr == null)) lyr = Environment.Data.GetLinkedLayer(((NavaidSystem)sellObj).Components[0]);
            var id = new List<string> {sellObj.ID};
            ShowOnMap(lyr, id);
            ((IActiveView) axMapControl1.Map).Refresh();
        }

        private void ShowOnMap(ILayer layer, List<string> ids)
        {
            try
            {
                var pMap = axMapControl1.Map;
                pMap.ClearSelection();
                var pSelect = layer as IFeatureSelection;
                if (pSelect != null)
                {
                    pSelect.CombinationMethod = esriSelectionResultEnum.esriSelectionResultAdd;
                    var s = "( ";
                    for (var i = 0; i <= ids.Count - 2; i++) 
                    { s = s + "'" + ids[i] + "',"; }
                    s = s + "'" + ids[ids.Count-1] + "')"; 

                    IQueryFilter queryFilter = new QueryFilterClass();
                    //queryFilter.WhereClause = "FeatureGUID = '" + _ID + "'";
                    queryFilter.WhereClause = "FeatureGUID IN " +s;

                    pSelect.SelectFeatures(queryFilter, esriSelectionResultEnum.esriSelectionResultAdd, false);

                    if (pSelect.SelectionSet.Count > 0)
                    {

                        var zoomToSelected = new ControlsZoomToSelectedCommandClass();
                        zoomToSelected.OnCreate(axMapControl1.Object);
                        zoomToSelected.OnClick();

                    }

                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.StackTrace + " ERROR " + ex.Message);
                throw;
            }
        }

        private void LayerPropertiesToolStripMenuItemClick(object sender, EventArgs e)
        {
            
            FormLayerProperties FrmLP = new FormLayerProperties();


            FrmLP.mapControl = Environment.MapControl.Map;
            FrmLP.panel3.Visible = true;

            FrmLP.ShowDialog();


            Environment.TreeViewImageList.Images.Clear();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            Environment.TreeViewImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("TreeViewImageList.ImageStream")));
            Environment.TreeViewImageList.TransparentColor = System.Drawing.Color.Transparent;
            Environment.TreeViewImageList.Images.SetKeyName(0, "Flag.bmp");


            Environment.Data.LayersSymbolInImageList.Clear();

            for (int i = 0; i < Environment.MapControl.Map.LayerCount; i++)
            {
                ILayer selectedLayer = Environment.MapControl.Map.get_Layer(i);
                PDMObject pdmObj =  Environment.Data.GetObjectLinkedWithLayer(selectedLayer.Name);
                if (pdmObj == null) continue;
                if (!Environment.Data.LayersSymbolInImageList.ContainsKey(pdmObj.GetType().Name))
                {
                    var bmp = Environment.Data.ConvertLayersSymbolToBitmap(selectedLayer);
                    Environment.TreeViewImageList.Images.Add(bmp);
                    Environment.Data.LayersSymbolInImageList.Add(pdmObj.GetType().Name, Environment.TreeViewImageList.Images.Count - 1);
                }

            }


            FillObjectsTree();
            //treeView1.Refresh();
            axMapControl1.Refresh(esriViewDrawPhase.esriViewGeography, null, null);
            axTOCControl1.Refresh();
        }

        private void LayerPropertiesEdit(ILayer selectedLayer)
        {
            EsriUtils._LayerPropertiesEdit(selectedLayer, Environment.mapControl.Map as IActiveView, axTOCControl1.hWnd);
            var indx = EsriUtils.getLayerIndex(Environment.mapControl.Map, ((IFeatureLayer) selectedLayer).FeatureClass.AliasName);   //GetPDMObjectLinkedWithLayer();
            Environment.TreeViewImageList.Images[indx + 1] = Environment.Data.ConvertLayersSymbolToBitmap(selectedLayer);
        }

        private void CoordinatSystemToolStripMenuItemClick(object sender, EventArgs e)
        {
            axMapControl1.Map.SpatialReference = EsriUtils._spatialReferenceDialog(axMapControl1.Map);
        }

        private void ToolStripDropDownButton1DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            switch (e.ClickedItem.Text)
            {
                case ("Inches"):
                    Environment.mapControl.MapUnits = esriUnits.esriInches;
                    break;
                case ("Yards"):
                    Environment.mapControl.MapUnits = esriUnits.esriYards;
                    break;
                case ("Points"):
                    Environment.mapControl.MapUnits = esriUnits.esriPoints;
                    break;
                case ("NauticalMiles"):
                    Environment.mapControl.MapUnits = esriUnits.esriNauticalMiles;
                    break;
                case ("Millimeters"):
                    Environment.mapControl.MapUnits = esriUnits.esriMillimeters;
                    break;
                case ("Miles"):
                    Environment.mapControl.MapUnits = esriUnits.esriMiles;
                    break;
                case ("Meters"):
                    Environment.mapControl.MapUnits = esriUnits.esriMeters;
                    break;
                case ("Kilometers"):
                    Environment.mapControl.MapUnits = esriUnits.esriKilometers;
                    break;
                case ("Feet"):
                    Environment.mapControl.MapUnits = esriUnits.esriFeet;
                    break;
                case ("Decimeters"):
                    Environment.mapControl.MapUnits = esriUnits.esriDecimeters;
                    break;
                case ("DecimalDegrees"):
                    Environment.mapControl.MapUnits = esriUnits.esriDecimalDegrees;
                    break;
                case ("Centimeters"):
                    Environment.mapControl.MapUnits = esriUnits.esriCentimeters;
                    break;
                //case (""):
                //    MapControl.MapUnits = esriUnits.;
                //    break;

            }

            axMapControl1.Refresh(esriViewDrawPhase.esriViewGeography, null, null);
        }

        private void TreeView1KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F2)
            {
                Button1Click(sender, e);
            }
        }

        private void EditObjectToolStripMenuItemClick(object sender, EventArgs e)
        {
            var selnd = treeView1.SelectedNode;
            if (treeView1.SelectedNode == null) return;

            if (Environment.Data.TableDictionary.Count ==0)
            {
                var lyr = axMapControl1.get_Layer(0);
                var fc = ((IFeatureLayer) lyr).FeatureClass;
                var workspaceEdit = (IWorkspaceEdit)fc.FeatureDataset.Workspace;
                Environment.FillAirtrackTableDic(workspaceEdit);
            }



            var sellObj2 = (PDMObject)treeView1.SelectedNode.Tag;
            if (sellObj2 == null) return;
            var frm = new InputForm
                          {
                              LinkedObject = sellObj2
                          };
            frm.ShowDialog();

            var tbl = Environment.Data.TableDictionary[sellObj2.GetType()];
            sellObj2.UpdateDB(tbl);

            readOnlyPropertyGrid.Refresh();

            selnd.Text = sellObj2.GetObjectLabel();
            ((IActiveView) axMapControl1.Map).Refresh();

            treeView1.SelectedNode = selnd;
        }

        private void DeleteObjectToolStripMenuItemClick(object sender, EventArgs e)
        {

            
            TreeNode selnd = treeView1.SelectedNode;
            if (treeView1.SelectedNode == null) return;

            if (Environment.Data.TableDictionary.Count == 0)
            {
                var lyr = axMapControl1.get_Layer(0);
                var fc = ((IFeatureLayer) lyr).FeatureClass;
                var workspaceEdit = (IWorkspaceEdit)fc.FeatureDataset.Workspace;
                Environment.FillAirtrackTableDic(workspaceEdit);
            }



            var sellObj2 = (PDMObject)treeView1.SelectedNode.Tag;
            if (sellObj2 != null)
            {
                if (sellObj2.DeleteObject(Environment.Data.TableDictionary) == 1)
                {
                    LogicUtil.RemoveFeature(sellObj2);
                    Environment.Data.PdmObjectList.Remove(sellObj2);
                    treeView1.Nodes.Remove(selnd);

                    Environment.ClearGraphics();

                    var pMap = axMapControl1.Map;
                    pMap.ClearSelection();
                    ((IActiveView) axMapControl1.Map).Refresh();
                }
                //FeatureTreeView.SelectedNode = selnd;
            }
        }


        private void axTOCControl1_OnMouseDown(object sender, ITOCControlEvents_OnMouseDownEvent e)
        {

            //IBasicMap map = new MapClass();
            //ILayer selectedLayer = new FeatureLayerClass();
            //var legendGroup = new object();
            //var index = new object();
            //var item = new esriTOCControlItem();

            //axTOCControl1.GetSelectedItem(ref item, ref map, ref selectedLayer, ref legendGroup, ref index);
            //if (item != esriTOCControlItem.esriTOCControlItemLayer) return;


            //if (Environment.Data.CurrentProject.ProjectType != ArenaProjectType.ARENA) return;
            //if (((ArenaProject)Environment.Data.CurrentProject).snp != null)  ((ArenaProject)Environment.Data.CurrentProject).StartSnappingProcess();
        }



        private void axTOCControl1_OnDoubleClick(object sender, ITOCControlEvents_OnDoubleClickEvent e)
        {
            IBasicMap map = new MapClass();
            ILayer selectedLayer = new FeatureLayerClass();
            var legendGroup = new object();
            var index = new object();
            var item = new esriTOCControlItem();

            axTOCControl1.GetSelectedItem(ref item, ref map, ref selectedLayer, ref legendGroup, ref index);
            if (item != esriTOCControlItem.esriTOCControlItemLayer) return;

            //System.Windows.Forms.MessageBox.Show("Map: " + map.Name + " Layer: " + layer.Name);
            LayerPropertiesEdit(selectedLayer);

            treeView1.Refresh();

            axMapControl1.Refresh(esriViewDrawPhase.esriViewGeography, null, null);
            axTOCControl1.Refresh();
        }

        private void NotamToolStripMenuItemClick(object sender, EventArgs e)
        {
            Environment.CreateEmptyProject(ArenaProjectType.NOTAM);
            //loadDataToolStripMenuItem.Enabled = false;
            SetWindowTitle();


        }

        private void readOnlyPropertyGrid_SelectedObjectsChanged(object sender, EventArgs e)
        {
            if (readOnlyPropertyGrid.SelectedObject == null)
            {
                label3.Text = "";
                label3.Visible = false;
                return;
            }
            label3.Text = readOnlyPropertyGrid.SelectedObject.GetType().Name + " " + ((PDMObject)readOnlyPropertyGrid.SelectedObject).GetObjectLabel();
            label3.Visible = true;

        }

        private void button3_Click(object sender, EventArgs e)
        {
            splitContainer1.Panel1Collapsed = !splitContainer1.Panel1Collapsed;

            button3.Text = splitContainer1.Panel1Collapsed ? @">" : @"<";
        }
 
        public Action<bool> OnAnimationAction { get; set; }

        private void PlayButtonClick(object sender, EventArgs e)
        {
            if (PlayButton.Checked)
            {
                PlayButton.ToolTipText = @"Stop Animation";
            }
            else
            {
                PlayButton.ToolTipText = @"Play Animation";
            }
            if (OnAnimationAction!=null)
            {
                OnAnimationAction(PlayButton.Checked);
            }
        }

        private void menuSaveDocAS_Click(object sender, EventArgs e)
        {
            if (!Environment.mapControl.CheckMxFile(Environment.Data.MapDocumentName)) return;

                var saveFileDialog1 = new SaveFileDialog
                {
                    Filter = @"Panda type files (*.pdm)|*.pdm",
                    RestoreDirectory = true
                };

                if (saveFileDialog1.ShowDialog() != DialogResult.OK) return;


                saveProject(saveFileDialog1.FileName);
        }

        private void aran45DBToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //ARAN45.Form1 frm = new ARAN45.Form1();
            //frm.ShowDialog();
        }

        private void englishToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Settings.LanguageCode LangCode;

            Enum.TryParse<Settings.LanguageCode>(((ToolStripMenuItem)sender).Tag.ToString(), out LangCode); 

            Static_Proc.SetLanguageCode(LangCode.ToString());
            if (Environment.Data.CurrentProject.ProjectType != ArenaProjectType.NOTAM)
                ((ArenaProject)Environment.Data.CurrentProject).ProjectSettings.Language = LangCode;

            // TODO: Change UI language
        }

        private void PlayButton_Click(object sender, EventArgs e)
        {
            
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            if (this.Tag != null)
            {
                OpenSelectedFile((string)this.Tag);
            }

            
        }

  
  
        
    }

   
           
}