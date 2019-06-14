
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
//using System.Data;
//using System.Text;
using System.Linq;
using System.Windows.Forms;
using ESRI.ArcGIS.ADF.CATIDs;
using ESRI.ArcGIS.ArcMapUI;
using ESRI.ArcGIS.Framework;
using ESRI.ArcGIS.Carto;
using System.Runtime.InteropServices;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Geodatabase;
//using ESRI.ArcGIS.Controls;
using ANCOR.MapElements;
using EsriWorkEnvironment;
using System.IO;
using ArenaStatic;
using ANCOR.MapCore;
using ANCOR;
using System.Xml.Serialization;
//using PDM;
using ANCORTOCLayerView;
using ARENA;
//using System.Media;
//using DataModule;
//using ARENA.Enums_Const;
//using ESRI.ArcGIS.Display;
//using AranSupport;

namespace SigmaChart
{
    [Guid("e311b82a-131f-4a33-b25b-2b96d6270957")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("ANCOR.ANCORTOCLayerView")]
    public partial class ANCORTOCLayerView : UserControl, IContentsView3
    {
        private IApplication m_application;
        //private bool m_visible;
        private object m_contextItem;
        private bool m_isProcessEvents;
        private TreeNode prevNode;
        private bool FontChangedFlag;

        List<TreeNode> selectedNodes = new List<TreeNode>();
        TreeNode previousNode;

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
        public ANCORTOCLayerView()
        {
            InitializeComponent();

            SigmaColorEdotor.FontColorChanged = textFontColorChanged;
            TextContestEditorForm.objectTextChanged = objectTextChanged;
            //TextContestEditorForm.ObjectTextContensChanged = objectTextContensChanged;
            MyTextContextEditor.ObjectTextContensChanged = objectTextContensChanged;

            ArenaStaticProc.SetEnvironmentPath();
        }

        private void objectTextContensChanged()
        {
            try
            {
                AbstractChartElement cartoEl = (AbstractChartElement)propertyGrid1.SelectedObject;
                if (cartoEl != null && cartoEl.LogTxt != null)
                {

                    string LoggingPath = System.IO.Path.GetDirectoryName((m_application.Document as IDocumentInfo2).Path) + @"\SIGMA_LOG.txt";

                    StreamWriter log;

                    if (!File.Exists(LoggingPath))
                    {
                        log = new StreamWriter(LoggingPath);
                    }
                    else
                    {
                        log = File.AppendText(LoggingPath);
                    }

                    log.WriteLine(cartoEl.LogTxt);
                    log.Close();

                    cartoEl.LogTxt = "";
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.InnerException);
            }

        }

        #region "IContentsView3 Implementations"

        public int Bitmap
        {
            get
            {

                System.Drawing.Bitmap bmp = global::ANCORTOCLayerView.Properties.Resources.sigma;

                return bmp.GetHbitmap().ToInt32();
            }
        }

        public string Tooltip
        {
            get { return "Sigma TOC"; }
        }

        //public bool Visible
        //{
        //    get { return m_visible; }
        //    set { m_visible = value; }
        //}
        string IContentsView3.Name { get { return "ANCORTOCLayerView"; } }
        public int hWnd { get { return this.Handle.ToInt32(); } }

        public object ContextItem //last right-clicked item
        {
            get { return m_contextItem; }
            set { }//Not implemented
        }

        private IMap _map;
        public IMap pMap
        {
            get { return _map; }
            set { _map = value; }
        }

        private IGraphicsContainer _graphicsContainer;
        public IGraphicsContainer pGraphicsContainer
        {
            get { return _graphicsContainer; }
            set { _graphicsContainer = value; }
        }

        public void Activate(int parentHWnd, IMxDocument Document)
        {

            if (m_application == null)
            {
                m_application = ((IDocument)Document).Parent;

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

        public void RefreshList()
        {
            if (m_application == null) return;

            FeatureTreeView.Tag = new Dictionary<string, TreeNode>();
            IMxDocument document = (IMxDocument)m_application.Document;
            IMaps maps = document.Maps;


            FillObjectsTree(false);



        }

        private void textFontColorChanged()
        {
            FontChangedFlag = true;

            System.Diagnostics.Debug.WriteLine("textFontColorChanged");
        }

        private void objectTextChanged()
        {

            linkLabel2_LinkClicked(null, null);
        }

        #region SIGMA TOC Treeview


        private void FillObjectsTree(bool SortFlag)
        {
            FeatureTreeView.Nodes.Clear();
            FeatureTreeView.Tag = SortFlag;
            ILayer _ADHPLayer = null;

            if (SigmaDataCash.ChartElementList == null) { FeatureTreeView.Nodes.Clear(); return; }
            //if (SigmaDataCash.ChartElementList != null && SigmaDataCash.ChartElementList.Count == 0)
            {

                pMap = ((IMxDocument)m_application.Document).FocusMap;

                IMxDocument document = (IMxDocument)m_application.Document;
                IMaps maps = document.Maps;


                for (int i = 0; i <= maps.Count - 1; i++)
                {

                    IMap map = maps.get_Item(i);

                    if (map.Description.Trim().Length > 0)
                    {
                        int d = Convert.ToInt32(map.Description);

                        if (d > 0)
                        {
                            pMap = map;
                        }
                        break;
                    }
                }


                pGraphicsContainer = ((IMxDocument)m_application.Document).ActiveView.GraphicsContainer;
                if (pMap.Description.Trim().Length > 0) SigmaDataCash.SigmaChartType = Convert.ToInt32(pMap.Description);

                 _ADHPLayer = EsriUtils.getLayerByName(pMap, ChartElementsManipulator.DefinelayerName("AirportHeliport"));
                if (_ADHPLayer == null) _ADHPLayer = EsriUtils.getLayerByName(pMap, ChartElementsManipulator.DefinelayerName("AirportCartography"));


                string activeFoder = ((ESRI.ArcGIS.Carto.IDocumentInfo2)(m_application.Document)).Folder; 
                if (SigmaDataCash.environmentWorkspaceEdit == null || (activeFoder!=null && !((IWorkspace)SigmaDataCash.environmentWorkspaceEdit).PathName.StartsWith(activeFoder)))
                {
                    if (_ADHPLayer == null) return;
                    var fc = ((IFeatureLayer)_ADHPLayer).FeatureClass;
                    if (fc == null) return;
                    var workspaceEdit = (IWorkspaceEdit)fc.FeatureDataset.Workspace;

                    SigmaDataCash.environmentWorkspaceEdit = workspaceEdit;

                    

                }



            }

            if (SigmaDataCash.prototype_anno_lst == null || SigmaDataCash.prototype_anno_lst.Count <= 0)
            {

                string pathToTemplateFileXML = "";
                switch (SigmaDataCash.SigmaChartType)
                {
                    case (1):
                        pathToTemplateFileXML = System.IO.Path.Combine(ArenaStaticProc.GetPathToTemplate() + @"\Enroute\", "enroute.sce");
                        break;
                    case (2):
                        pathToTemplateFileXML = System.IO.Path.Combine(ArenaStaticProc.GetPathToTemplate() + @"\SID\", "sid.sce");
                        break;
                    case (4):
                        pathToTemplateFileXML = System.IO.Path.Combine(ArenaStaticProc.GetPathToTemplate() + @"\STAR\", "star.sce");
                        break;
                    case (5):
                        pathToTemplateFileXML = System.IO.Path.Combine(ArenaStaticProc.GetPathToTemplate() + @"\IAP\", "iap.sce");
                        break;
                    case (6):
                        pathToTemplateFileXML = System.IO.Path.Combine(ArenaStaticProc.GetPathToTemplate() + @"\PATC\", "patc.sce");
                        break;
                    case (7):
                        pathToTemplateFileXML = System.IO.Path.Combine(ArenaStaticProc.GetPathToTemplate() + @"\AreaChart\", "areachart.sce");
                        break;
                    case (13):
                        pathToTemplateFileXML = System.IO.Path.Combine(ArenaStaticProc.GetPathToTemplate() + @"\MinimumAltitudeChart\", "minalt.sce");
                        break;
                    case (8):
                    case (9):
                    case (10):
                    case (11):
                    case (12):
                        pathToTemplateFileXML = System.IO.Path.Combine(ArenaStaticProc.GetPathToTemplate() + @"\AerodromeElectronicChart\", "aerodrome.sce");
                        break;

                }


                if (File.Exists(pathToTemplateFileXML)) SigmaDataCash.GetProtoTypeAnnotation(pathToTemplateFileXML);
            }

            #region Update Feature Classes


            for (int i = 0; i < pMap.LayerCount; i++)
            {
                ILayer lyr = pMap.Layer[i];

                if (lyr is FeatureLayer && ((IFeatureLayer)lyr).FeatureClass != null && ((IFeatureLayer)lyr).FeatureClass.Fields.FindField("VisibleFlag") < 0)
                {
                    string _AliasName = ((IFeatureLayer)lyr).FeatureClass.AliasName;

                    switch (_AliasName)
                    {
                        case ("AirspaceC"):
                        case ("AirspaceB"):
                        case ("DesignatedPointCartography"):
                        case ("NavaidsCartography"):
                        case ("ProcedureLegsCartography"):
                        case ("HoldingCartography"):
                        case ("HoldingPath"):
                        case ("VerticalStructurePointCartography"):
                        case ("RunwayCartography"):
                        case ("FacilityMakeUpCartography"):
                        case ("AirportCartography"):
                        case ("AMEA"):
                        case ("GlidePathCartography"):
                        case ("DecorPointCartography"):
                        case ("DecorLineCartography"):
                        case ("DecorPolygonCartography"):

                            ChartElementsManipulator.UpdateFeatereClass_AddField(((IFeatureLayer)lyr).FeatureClass, _AliasName, "VisibleFlag", esriFieldType.esriFieldTypeSmallInteger);

                            break;
                        default: continue;
                    }

                }

            }


            #endregion

            ChartElementsManipulator.LoadChartElements(SigmaDataCash.environmentWorkspaceEdit);




            if (SigmaDataCash.ChartElementList.Count == 0) return;

            FeatureTreeView.BeginUpdate();
            FeatureTreeView.Nodes.Clear();

            var _items = from a in SigmaDataCash.ChartElementList group a by (a as AbstractChartElement).Name;
            if (SortFlag)
                _items = from a in SigmaDataCash.ChartElementList group a by (a as AbstractChartElement).RelatedFeature;



            foreach (var _itemGroup in _items)
            {
                ILayer _Layer = EsriUtils.getLayerByName2(pMap, ChartElementsManipulator.DefinelayerName(_itemGroup.Key));
                if (_Layer == null && (_itemGroup.Key.StartsWith("HoldingPatternInboundCource") || _itemGroup.Key.StartsWith("HoldingPatternOutboundCource")))
                {
                    _Layer = EsriUtils.getLayerByName2(pMap, ChartElementsManipulator.DefinelayerName("ProcedureLegCourse"));
                }
                //if (_Layer == null) continue;

                var FeatureNode = new TreeNode(_itemGroup.Key + " (" + _itemGroup.Count().ToString() + ")") { Name = _itemGroup.Key, Checked = true };
                foreach (var protoEl in SigmaDataCash.prototype_anno_lst)
                {
                    if (protoEl.Name.CompareTo(_itemGroup.Key) == 0)
                    {
                        FeatureNode.Tag = protoEl;
                        break;
                    }
                    else if (protoEl.RelatedFeature.CompareTo("Graphics") ==0)
                    {
                        if (_itemGroup.Key.StartsWith(protoEl.Name))
                        {
                            FeatureNode.Tag = protoEl;
                            break;
                        }
                    }
                }


                foreach (var _item in _itemGroup)
                {

                    if (_item is ChartElement_SimpleText)
                    {

                        if (((ChartElement_SimpleText)_item).TextContents.Count == 0)
                        {
                            ((ChartElement_SimpleText)_item).TextContents = new List<List<AncorChartElementWord>>();

                            List<AncorChartElementWord> txtLine = new List<AncorChartElementWord>(); // создаем строку
                            AncorChartElementWord wrd = new AncorChartElementWord("text", ((ChartElement_SimpleText)_item).Font);//создаем слово
                            wrd.Font.Bold = true;
                            wrd.StartSymbol = new AncorSymbol("");
                            wrd.EndSymbol = new AncorSymbol("");
                            //wrd.Morse = true;
                            txtLine.Add(wrd); // добавим его в строку

                            ((ChartElement_SimpleText)_item).TextContents.Add(txtLine);

                        }
                        TreeNode Nd = new TreeNode(((ChartElement_SimpleText)_item).TextContents[0][0].TextValue) { };

                        if (Nd.Text.StartsWith("__") && ((ChartElement_SimpleText)_item).TextContents.Count > 1)
                            //Nd = new TreeNode(((ChartElement_SimpleText)_item).TextContents[1][0].TextValue) { };
                            Nd = new TreeNode(GetText(((ChartElement_SimpleText)_item).TextContents)) { };
                        if (_item is ChartElement_BorderedText_Collout_CaptionBottom)
                        {
                            if (((ChartElement_BorderedText_Collout_CaptionBottom)_item).CaptionTextLine.Count > 0)
                                //Nd = new TreeNode(((ChartElement_BorderedText_Collout_CaptionBottom)_item).CaptionTextLine[0][0].TextValue) { };
                                Nd = new TreeNode(GetText(((ChartElement_BorderedText_Collout_CaptionBottom)_item).CaptionTextLine)) { };
                            else
                                Nd = new TreeNode("");
                            if (Nd.Text.Trim().Length == 0)
                                //Nd.Text = ((ChartElement_BorderedText_Collout_CaptionBottom)_item).TextContents[0][0].TextValue;
                                Nd = new TreeNode(GetText(((ChartElement_BorderedText_Collout_CaptionBottom)_item).TextContents)) { };


                            if (!((ChartElement_BorderedText_Collout_CaptionBottom)_item).CaptionTextLine[0][0].Visible && ((ChartElement_BorderedText_Collout_CaptionBottom)_item).CaptionTextLine.Count > 1 &&
                               ((ChartElement_BorderedText_Collout_CaptionBottom)_item).CaptionTextLine[1][0].Visible)
                                //Nd = new TreeNode(((ChartElement_BorderedText_Collout_CaptionBottom)_item).CaptionTextLine[1][0].TextValue) { };
                                Nd = new TreeNode(GetText(((ChartElement_BorderedText_Collout_CaptionBottom)_item).CaptionTextLine)) { };

                        }

                        if (((ChartElement_SimpleText)_item).Name.StartsWith("RouteSegment_UpperLowerLimit") || ((ChartElement_SimpleText)_item).Name.StartsWith("RouteSegment_sign") ||
                             ((ChartElement_SimpleText)_item).Name.StartsWith("RouteSegment_ValReversMagTrack") || ((ChartElement_SimpleText)_item).Name.StartsWith("RouteSegment_ValMagTrack"))
                        {
                            string R_NAME = GetParentNameById(((ChartElement_SimpleText)_item).LinckedGeoId, "RouteSegmentCartography", "designator");
                            Nd = new TreeNode(GetText(((ChartElement_SimpleText)_item).TextContents) + R_NAME) { };
                        }

                        if (((ChartElement_SimpleText)_item).Name.StartsWith("ProcedureLegLength") || ((ChartElement_SimpleText)_item).Name.StartsWith("ProcedureLegSpeed") ||
                            ((ChartElement_SimpleText)_item).Name.StartsWith("ProcedureLegCourse") || ((ChartElement_SimpleText)_item).Name.StartsWith("ProcedureLegName") ||
                            ((ChartElement_SimpleText)_item).Name.StartsWith("ProcedureLegHeight"))
                        {
                            string stDesig = GetParentNameById(((ChartElement_SimpleText)_item).LinckedGeoId, "ProcedureLegsCartography", "StartPontDesignator");
                            string enDesig = GetParentNameById(((ChartElement_SimpleText)_item).LinckedGeoId, "ProcedureLegsCartography", "EndPontDesignator");

                            string R_NAME = stDesig.Trim().CompareTo("()")!=0 && enDesig.Trim().CompareTo("()") != 0 ? stDesig + enDesig :  GetParentNameById(((ChartElement_SimpleText)_item).LinckedGeoId, "ProcedureLegsCartography", "seqNumberARINC");
                            Nd = new TreeNode(GetText(((ChartElement_SimpleText)_item).TextContents) + R_NAME) { };
                        }


                        if (((ChartElement_SimpleText)_item).Name.StartsWith("SigmaCollout_Airspace"))
                        {
                            string R_NAME = GetParentNameById(((ChartElement_SimpleText)_item).LinckedGeoId, "AirspaceC", "codeId");
                            //Nd = new TreeNode(((ChartElement_SimpleText)_item).TextContents[0][0].TextValue) { };
                            Nd = new TreeNode(GetText(((ChartElement_SimpleText)_item).TextContents)) { };

                        }
                        if (((ChartElement_SimpleText)_item).Name.StartsWith("Airspace_Simple"))
                        {
                            string R_NAME = GetParentNameById(((ChartElement_SimpleText)_item).LinckedGeoId, "AirspaceC", "codeId");
                            //Nd = new TreeNode(((ChartElement_SimpleText)_item).TextContents[0][0].TextValue) { };
                            Nd = new TreeNode(GetText(((ChartElement_SimpleText)_item).TextContents)) { };

                        }

                        if (_item is ChartElement_MarkerSymbol && ((ChartElement_MarkerSymbol)_item).TextContents[0][0].DataSource.Value != null && !((ChartElement_SimpleText)_item).Name.StartsWith("ProcedureLegLength"))
                            //Nd = new TreeNode(((ChartElement_MarkerSymbol)_item).TextContents[0][0].TextValue) { };
                            Nd = new TreeNode(GetText(((ChartElement_MarkerSymbol)_item).TextContents)) { };

                        if (((ChartElement_SimpleText)_item).Name.StartsWith("HoldingPattern"))
                        {
                            //Nd = new TreeNode(((ChartElement_SimpleText)_item).TextContents[0][0].TextValue) { };
                            Nd = new TreeNode(GetText(((ChartElement_SimpleText)_item).TextContents)) { };
                            string stDesig = GetParentNameById(((ChartElement_SimpleText)_item).LinckedGeoId, "DesignatedPointCartography", "SegmentPointDesignator");
                            if (stDesig != null && stDesig.Length > 0) Nd.Text = stDesig +  Nd.Text;


                        }

                        if (_item is ChartElement_Radial && ((ChartElement_Radial)_item).TextContents.Count > 1)
                        {

                            Nd = ((ChartElement_Radial)_item).TextContents[1].Count > 1 ? new TreeNode(((ChartElement_Radial)_item).TextContents[1][0].TextValue + " " + ((ChartElement_Radial)_item).TextContents[1][1].TextValue) { } :
                                ((ChartElement_Radial)_item).TextContents[0].Count > 1 ? new TreeNode(((ChartElement_Radial)_item).TextContents[0][0].TextValue + " " + ((ChartElement_Radial)_item).TextContents[0][1].TextValue) { } : new TreeNode(((ChartElement_Radial)_item).TextContents[0][0].TextValue);
                        }

                        if (_item is ChartElement_MarkerSymbol && ((ChartElement_MarkerSymbol)_item).TextContents[0][0].DataSource.Value != null && ((ChartElement_MarkerSymbol)_item).TextContents[0][0].DataSource.Condition.StartsWith("NoneScale"))
                            Nd.Text = "NoneScale";

                        if (((ChartElement_SimpleText)_item).Tag != null && ((ChartElement_SimpleText)_item).Tag.StartsWith("Clone"))
                            Nd.Text = Nd.Text + "©";

                        if (((AbstractChartElement)_item).Placed && !((AbstractChartElement)_item).ReflectionHidden)
                            Nd.NodeFont = new System.Drawing.Font(new FontFamily("Times New Roman"), 10, FontStyle.Regular);
                        else if (((AbstractChartElement)_item).Placed && ((AbstractChartElement)_item).ReflectionHidden)
                            Nd.NodeFont = new System.Drawing.Font(new FontFamily("Times New Roman"), 10, FontStyle.Italic);
                        else if (!((AbstractChartElement)_item).Placed && !((AbstractChartElement)_item).ReflectionHidden)
                            Nd.NodeFont = new System.Drawing.Font(new FontFamily("Times New Roman"), 10, FontStyle.Strikeout);
                        else
                            Nd.NodeFont = new System.Drawing.Font(new FontFamily("Times New Roman"), 10, FontStyle.Strikeout | FontStyle.Italic);

                        if (Nd.NodeFont.Strikeout) Nd.ForeColor = System.Drawing.Color.Coral;
                        else Nd.ForeColor = System.Drawing.SystemColors.ControlText;

                        Nd.Tag = _item;
                        Nd.Name = ((AbstractChartElement)_item).Id.ToString();
                        FeatureNode.Nodes.Add(Nd);

                    }
                    else if (_item is GraphicsChartElement)
                    {
                        TreeNode Nd = new TreeNode(_item.GetType().Name) { };
                        if (((AbstractChartElement)_item).Placed)
                            Nd.NodeFont = new System.Drawing.Font(new FontFamily("Times New Roman"), 10, FontStyle.Regular);
                        else
                            Nd.NodeFont = new System.Drawing.Font(new FontFamily("Times New Roman"), 10, FontStyle.Italic | FontStyle.Strikeout);

                        Nd.Tag = _item;
                        Nd.Name = ((AbstractChartElement)_item).Name.ToString();
                        FeatureNode.Text = ((GraphicsChartElement)_item).GraphicsChartElementName;
                        FeatureNode.Nodes.Add(Nd);
                    }
                    else if (_item is ChartElement_ILSCollout)
                    {
                        TreeNode Nd = new TreeNode(_item.GetType().Name) { };
                        if (((AbstractChartElement)_item).Placed)
                            Nd.NodeFont = new System.Drawing.Font(new FontFamily("Times New Roman"), 10, FontStyle.Regular);
                        else
                            Nd.NodeFont = new System.Drawing.Font(new FontFamily("Times New Roman"), 10, FontStyle.Italic | FontStyle.Strikeout);

                        Nd.Tag = _item;
                        Nd.Name = ((AbstractChartElement)_item).Id.ToString();
                        FeatureNode.Text = "ILS symbol";
                        FeatureNode.Nodes.Add(Nd);
                    }

                }

                if (FeatureNode.Nodes.Count > 0)
                    FeatureTreeView.Nodes.Add(FeatureNode);
            }

            FeatureTreeView.EndUpdate();

            FeatureTreeView.Sort();

            SigmaDataCash.ChartElementsTree = FeatureTreeView;
            SigmaDataCash.AncorPropertyGrid = propertyGrid1;
            SigmaDataCash.MirrorPropertyGrid = propertyGrid2;

           

        }

        private string GetParentNameById(string linckedGeoId, string Table_Name, string FieldName)
        {
            ICursor rowCur = null;
            string res = "";
            try
            {
                ITable tbl = EsriUtils.getTableByname((IFeatureWorkspace)SigmaDataCash.environmentWorkspaceEdit, Table_Name);
                if (tbl != null)
                {


                    IQueryFilter _Filter = new QueryFilterClass();
                    _Filter.WhereClause = "FeatureGUID = '" + linckedGeoId + "'";
                    rowCur = tbl.Search(_Filter, true);

                    IRow _row = rowCur.NextRow();
                    if (_row != null)
                    {
                        res = _row.Value[_row.Fields.FindField(FieldName)].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            finally
            {
                if (rowCur != null) Marshal.ReleaseComObject(rowCur);


            }
            return res!=null && res.Length > 0 ? " (" + res + ")" : "";



        }

        private string GetText(List<List<AncorChartElementWord>> list)
        {
            string res = "";
            for (int i = 0; i <= list.Count - 1; i++)
            {
                var Ln = list[i];
                foreach (var wrd in Ln)
                {
                    if (!wrd.Visible) continue;
                    if (wrd.TextValue.StartsWith("_")) continue;
                    // if (!wrd.TextValue.Trim().All(char.IsLetterOrDigit)) continue;
                    if (wrd.TextValue.Trim().Length > 0) res = res + wrd.TextValue;
                }
                if (i < list.Count - 1) res = res + "/";
            }

            if (res.EndsWith("/")) res = res.Remove(res.Length - 1, 1);
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
            FeatureTreeView.Nodes.Clear();
        }

        #endregion

        private void FeatureTreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {


            if (prevNode != null && !prevNode.NodeFont.Strikeout) prevNode.ForeColor = System.Drawing.SystemColors.ControlText;
            // else prevNode.ForeColor = System.Drawing.Color.Coral;

            propertyGrid1.SelectedObject = null;
            linkLabel2.Text = "";
            if (e.Node.Tag == null) return;
            //propertyGrid1.ReadOnly = (e.Node.Tag is Airspace);
            propertyGrid1.SelectedObject = e.Node.Tag;
            linkLabel2.Text = "Update object " + e.Node.Text;

            if (((AbstractChartElement)e.Node.Tag).Placed && !((AbstractChartElement)e.Node.Tag).ReflectionHidden)
                FeatureTreeView.SelectedNode.NodeFont = new System.Drawing.Font(new FontFamily("Times New Roman"), 10, FontStyle.Regular);
            else if (((AbstractChartElement)e.Node.Tag).Placed && ((AbstractChartElement)e.Node.Tag).ReflectionHidden)
                FeatureTreeView.SelectedNode.NodeFont = new System.Drawing.Font(new FontFamily("Times New Roman"), 10, FontStyle.Italic);
            else if (!((AbstractChartElement)e.Node.Tag).Placed && !((AbstractChartElement)e.Node.Tag).ReflectionHidden)
                FeatureTreeView.SelectedNode.NodeFont = new System.Drawing.Font(new FontFamily("Times New Roman"), 10, FontStyle.Strikeout);
            else
                FeatureTreeView.SelectedNode.NodeFont = new System.Drawing.Font(new FontFamily("Times New Roman"), 10, FontStyle.Strikeout | FontStyle.Italic);

            //if (e.Node.NodeFont.Strikeout) e.Node.ForeColor = System.Drawing.Color.Coral;
            //else 
            e.Node.ForeColor = System.Drawing.Color.Coral;



            prevNode = e.Node;

            tabControl1.SelectedIndex = 0;

            //if (propertyGrid1.SelectedObject is ChartElement_SimpleText)
            //{
            //    ((ChartElement_SimpleText)propertyGrid1.SelectedObject).onColorChanged +=ANCORTOCLayerView_onColorChanged;
            //}
        }

        void ANCORTOCLayerView_onColorChanged()
        {
            // MessageBox.Show("ANCORTOCLayerView_onColorChanged");
        }


        private void refreshTreeViewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FillObjectsTree((bool)FeatureTreeView.Tag);
        }

        private void FeatureTreeView_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F5) FillObjectsTree(false);
            if (e.KeyCode == Keys.F6) FillObjectsTree(true);
        }

        private void sortTreeViewToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            FillObjectsTree(true);
        }

        private void sortByAnnotationTypeToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            FillObjectsTree(false);
        }

        private void showOnMapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (FeatureTreeView.SelectedNode == null) return;
            if (FeatureTreeView.SelectedNode.Tag == null) return;
            if (FeatureTreeView.SelectedNode.Parent.Tag == null)
                return;


            AbstractChartElement chartEl = ((AbstractChartElement)FeatureTreeView.SelectedNode.Tag);
            if (IsPrototypeElement(chartEl)) return;

            showOnMap(chartEl);
            ZoomToMap();

        }

        private IGeometry showOnMap(AbstractChartElement chartEl)
        {
            ActivateMainFrame();

            ILayer _Layer = EsriUtils.getLayerByName2(pMap, ChartElementsManipulator.DefinelayerName(chartEl.Name));
            if (_Layer == null)
            {
                string lName = "";
                switch (chartEl.Name)
                {
                    case ("HoldingPatternInboundCource"):
                    case ("HoldingPatternOutboundCource"):
                        lName = "ProcedureLegsAnnoCourseCartography";
                        break;
                    default:
                        break;
                }

                _Layer = EsriUtils.getLayerByName2(pMap, lName);

            }


            if (_Layer == null) return null;

            IGeometry res = null;
            try
            {
                pMap.ClearSelection();
                _Layer.Visible = true;

                IFeatureClass _fc = ((IFeatureLayer)_Layer).FeatureClass;
                IFeature feat = ChartElementsManipulator.SearchBySpatialFilter(_fc, "AncorUID =  " + "\"" + chartEl.Id + "\"");
                if (feat != null)
                {
                    ChartElementsManipulator.SelectFeature(_Layer, feat, false);
                    res = feat.Shape;
                }

                return res;



            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.StackTrace + " ERROR " + ex.Message);
                throw;
            }
        }

        public void FlashGeometry(ESRI.ArcGIS.Geometry.IGeometry geometry, ESRI.ArcGIS.Display.IDisplay display, IEnvelope Extnt, System.Int32 delay, int flashCount = 4)
        {
            if (geometry == null || display == null)
            {
                return;
            }

            ESRI.ArcGIS.Display.IRgbColor color = new ESRI.ArcGIS.Display.RgbColorClass();
            //Color Properties  
            color.Red = 255;
            color.Green = 0;
            color.Blue = 0;

            display.StartDrawing(display.hDC, (System.Int16)ESRI.ArcGIS.Display.esriScreenCache.esriNoScreenCache); // Explicit Cast  


            switch (geometry.GeometryType)
            {
                case ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryPolygon:
                    {
                        //Set the flash geometry's symbol.  
                        ESRI.ArcGIS.Display.ISimpleFillSymbol simpleFillSymbol = new ESRI.ArcGIS.Display.SimpleFillSymbolClass();
                        ESRI.ArcGIS.Display.ISimpleLineSymbol lnSym = new ESRI.ArcGIS.Display.SimpleLineSymbolClass();
                        simpleFillSymbol.Color = color;
                        lnSym.Color = color;
                        lnSym.Width = 3;

                        ESRI.ArcGIS.Display.ISymbol symbolPlgn = simpleFillSymbol as ESRI.ArcGIS.Display.ISymbol; // Dynamic Cast  
                        ESRI.ArcGIS.Display.ISymbol symbolLN = lnSym as ESRI.ArcGIS.Display.ISymbol; // Dynamic Cast  
                        symbolPlgn.ROP2 = ESRI.ArcGIS.Display.esriRasterOpCode.esriROPNotXOrPen;
                        symbolLN.ROP2 = ESRI.ArcGIS.Display.esriRasterOpCode.esriROPNotXOrPen;

                        IPolyline ln1 = new PolylineClass { FromPoint = Extnt.UpperLeft, ToPoint = Extnt.LowerRight};
                        IPolyline ln2 = new PolylineClass { FromPoint = Extnt.UpperRight, ToPoint = Extnt.LowerLeft};

                        for (int i = 0; i < flashCount; i++)
                        {
                            display.SetSymbol(symbolPlgn);
                            display.DrawPolygon(geometry);

                            display.SetSymbol(symbolLN);
                            display.DrawPolyline(ln1);
                            display.DrawPolyline(ln2);

                            System.Threading.Thread.Sleep(delay);

                            display.SetSymbol(symbolPlgn);
                            display.DrawPolygon(geometry);

                            display.SetSymbol(symbolLN);
                            display.DrawPolyline(ln1);
                            display.DrawPolyline(ln2);

                        }
                        break;
                    }

            }
            display.FinishDrawing();
        }

        private void ZoomToMap()
        {
            try
            {


                UID menuID = new UIDClass();

                menuID.Value = "{AB073B49-DE5E-11D1-AA80-00C04FA37860}";

                ICommandItem pCmdItem = m_application.Document.CommandBars.Find(menuID);
                pCmdItem.Execute();
                Marshal.ReleaseComObject(pCmdItem);
                Marshal.ReleaseComObject(menuID);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.StackTrace + " ERROR " + ex.Message);
                throw;
            }

            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");

        }

        public void HighlightChartElement(string ChartElementId)
        {
        }

        private void updateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (FeatureTreeView.SelectedNode == null) return;
            if (FeatureTreeView.SelectedNode.Tag == null) return;


            SigmaDataCash.environmentWorkspaceEdit.StartEditing(false);
            SigmaDataCash.environmentWorkspaceEdit.StartEditOperation();

            if (tabControl1.SelectedIndex == 0)
            {
                //TreeNode pn = FeatureTreeView.SelectedNode.Parent;
                //foreach (TreeNode nd in pn.Nodes)
                //{
                //    UpdateElement(nd);
                //}
                //System.Windows.Forms.MessageBox.Show("");

                UpdateElement(FeatureTreeView.SelectedNode);
                UpdateExistingNote((AbstractChartElement)FeatureTreeView.SelectedNode.Tag);


            }
            else if (tabControl1.SelectedIndex == 1 && propertyGrid2.SelectedObject != null && propertyGrid2.SelectedObject is AbstractChartElement)
            {
                UpdateElementInFrame((AbstractChartElement)propertyGrid2.SelectedObject);
            }

            SigmaDataCash.environmentWorkspaceEdit.StopEditOperation();
            SigmaDataCash.environmentWorkspaceEdit.StopEditing(true);




            //(pMap as IActiveView).Refresh();
            ChartElementsManipulator.RefreshChart((IMxDocument)m_application.Document);

        }

        private void UpdateElementInFrame(AbstractChartElement cartoEl)
        {
            if (cartoEl is ChartElement_SimpleText)
            {
                IElement el = cartoEl.ConvertToIElement() as IElement;

                string objSer = "";

                switch (cartoEl.GetType().Name.ToString())
                {
                    case ("ChartElement_SimpleText"):
                        objSer = ChartElementsManipulator.SerializeObject(cartoEl as ChartElement_SimpleText);
                        break;
                    case ("ChartElement_RouteDesignator"):
                        objSer = ChartElementsManipulator.SerializeObject(cartoEl as ChartElement_RouteDesignator);
                        break;
                    case ("ChartElement_BorderedText"):
                        objSer = ChartElementsManipulator.SerializeObject(cartoEl as ChartElement_BorderedText);
                        break;
                    case ("ChartElement_BorderedText_Collout_CaptionBottom"):
                        objSer = ChartElementsManipulator.SerializeObject(cartoEl as ChartElement_BorderedText_Collout_CaptionBottom);
                        break;
                    case ("ChartElement_BorderedText_Collout"):
                        objSer = ChartElementsManipulator.SerializeObject(cartoEl as ChartElement_BorderedText_Collout);
                        break;
                    case ("ChartElement_SigmaCollout_Navaid"):
                        objSer = ChartElementsManipulator.SerializeObject(cartoEl as ChartElement_SigmaCollout_Navaid);
                        break;
                    case ("ChartElement_MarkerSymbol"):
                        objSer = ChartElementsManipulator.SerializeObject(cartoEl as ChartElement_MarkerSymbol);
                        break;
                    case ("ChartElement_TextArrow"):
                        objSer = ChartElementsManipulator.SerializeObject(cartoEl as ChartElement_TextArrow);
                        break;
                    case ("ChartElement_Radial"):
                        objSer = ChartElementsManipulator.SerializeObject(cartoEl as ChartElement_Radial);
                        break;
                    case ("ChartElement_SigmaCollout_Designatedpoint"):
                        objSer = ChartElementsManipulator.SerializeObject(cartoEl as ChartElement_SigmaCollout_Designatedpoint);
                        break;
                    case ("ChartElement_SigmaCollout_Airspace"):
                        objSer = ChartElementsManipulator.SerializeObject(cartoEl as ChartElement_SigmaCollout_Airspace);
                        break;
                    case ("ChartElement_SigmaCollout_AccentBar"):
                        objSer = ChartElementsManipulator.SerializeObject(cartoEl as ChartElement_SigmaCollout_AccentBar);
                        break;
                    case ("ChartElement_ILSCollout"):
                        objSer = ChartElementsManipulator.SerializeObject(cartoEl as ChartElement_ILSCollout);
                        break;
                    default:
                        break;

                }

                ChartElementsManipulator.UpdateMirror(el, cartoEl.Id.ToString(), objSer);


            }
        }

        private void UpdateExistingNote(AbstractChartElement ChartElement)
        {
            if (ChartElement is ChartElement_MarkerSymbol)
            {
                ChartElement_MarkerSymbol SelectedElement = (ChartElement_MarkerSymbol)ChartElement;

                string NoteName = SelectedElement.Name + "_" + SelectedElement.Id.ToString() + "_" + SelectedElement.RelatedFeature;
                IElement note = FindNote(NoteName);

                if (note != null)
                {
                    IElement el_new = (IElement)SelectedElement.ConvertToIElement();

                    for (int i = 0; i <= (note as IGroupElement).ElementCount - 1; i++)
                    {
                        IElement el = (note as IGroupElement).get_Element(i);
                        IElementProperties3 prp = (IElementProperties3)el;
                        if (prp.Name.StartsWith("Note"))
                        {
                            IGeometry gm = (el as IGroupElement).get_Element(0).Geometry;

                            IGroupElement GrEl = el_new as IGroupElement;
                            for (int j = 0; j <= GrEl.ElementCount - 1; j++)
                            {
                                GrEl.get_Element(j).Geometry = gm;
                            }

                            (note as IGroupElement).DeleteElement(el);

                            prp = (IElementProperties3)el_new;
                            prp.Name = "Note";
                            (note as IGroupElement).AddElement(el_new);


                            break;
                        }

                    }

                }

            }
        }

        private void UpdateElement(TreeNode nd)
        {
            AbstractChartElement cartoEl = (AbstractChartElement)nd.Tag;

            //#region MyRegion

            //var _list = DataCash.GetObjectsByType(PDM.PDM_ENUM.WayPoint).OrderBy(l => ((PDM.WayPoint)l).Designator).ToList();
            //string dpn = "";
            //if (cartoEl is ChartElement_BorderedText_Collout_CaptionBottom)
            //{
            //    if (((ChartElement_BorderedText_Collout_CaptionBottom)cartoEl).CaptionTextLine.Count > 0)
            //        dpn = (((ChartElement_BorderedText_Collout_CaptionBottom)cartoEl).CaptionTextLine[0][0].TextValue).Trim();

            //    var desig = (from element in _list where (element != null) && ((PDM.WayPoint)element).Designator.StartsWith(dpn) select element).FirstOrDefault();
            //    cartoEl.LinckedGeoId = desig.ID;
            //}



            //#endregion

            IElement el = cartoEl.ConvertToIElement() as IElement;

            if (cartoEl is ChartElement_ILSCollout)
            {

                ChartElement_ILSCollout chrtEl_ils = (ChartElement_ILSCollout)cartoEl;

                IPoint aPt = new PointClass();
                if (chrtEl_ils.IlsAnchorPoint == SigmaIlsAnchorPoint.LOC) { aPt.PutCoords(chrtEl_ils.locX, chrtEl_ils.locY); chrtEl_ils.Length = Convert.ToInt32(chrtEl_ils.DistToLOC); }
                if (chrtEl_ils.IlsAnchorPoint == SigmaIlsAnchorPoint.GP) { aPt.PutCoords(chrtEl_ils.gpX, chrtEl_ils.gpY); chrtEl_ils.Length = Convert.ToInt32(chrtEl_ils.DistToGP); }
                chrtEl_ils.Anchor = new AncorPoint(aPt.X, aPt.Y);

                el = cartoEl.ConvertToIElement() as IElement;

                ChartElementsManipulator.UpdateILSGlidePathElementToDataSet(cartoEl.Name, cartoEl.Id.ToString(), el, aPt, ref cartoEl, false);


            }



            if (cartoEl is ChartElement_SimpleText)
            {
                ChartElementsManipulator.UpdateSingleElementToDataSet(cartoEl.Name, cartoEl.Id.ToString(), el, ref cartoEl, false);

            }
            else if (cartoEl is GraphicsChartElement)
            {
                ChartElementsManipulator.UpdateGraphicsElement(pGraphicsContainer, cartoEl, ref el);
                ChartElementsManipulator.UpdateGraphicsElementToDataSet(cartoEl.Name, cartoEl.Id.ToString(), el, ref cartoEl);
                ((IActiveView)pGraphicsContainer).Refresh();

            }
            

        }



        private void UpdateElement(TreeNode nd, bool UpdateMirroFlag)
        {

            AbstractChartElement cartoEl = (AbstractChartElement)nd.Tag;
            IElement el = cartoEl.ConvertToIElement() as IElement;

            if (cartoEl is ChartElement_SimpleText)
            {
                ChartElementsManipulator.UpdateSingleElementToDataSet(cartoEl.Name, cartoEl.Id.ToString(), el, ref cartoEl, UpdateMirroFlag);

            }
            else if (cartoEl is GraphicsChartElement)
            {
                ChartElementsManipulator.UpdateGraphicsElement(pGraphicsContainer, cartoEl, ref el);
                ChartElementsManipulator.UpdateGraphicsElementToDataSet(cartoEl.Name, cartoEl.Id.ToString(), el, ref cartoEl);
                ((IActiveView)pGraphicsContainer).Refresh();

            }


        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {


            if (SigmaDataCash.SelectedChartElements == null) SigmaDataCash.SelectedChartElements = new List<AbstractChartElement>();

            if (FeatureTreeView.SelectedNode == null) return;

            AbstractChartElement cartoEl = (AbstractChartElement)FeatureTreeView.SelectedNode.Tag;

            if (tabControl1.SelectedIndex == 1)
                cartoEl = (AbstractChartElement)propertyGrid2.SelectedObject;


            ///Update one elements
            ///
            if (!IsPrototypeElement(cartoEl))
            {
                /// Update single child elements
                ///
                if (SigmaDataCash.SelectedChartElements == null || SigmaDataCash.SelectedChartElements.Count <= 1)
                {
                    // to do change Fonts prop
                    if (FontChangedFlag) SetTheSameFontsToTextContents(cartoEl);
                    updateToolStripMenuItem_Click(sender, e);
                }
                else
                {


                    /// Update selected elements
                    ///
                    SigmaDataCash.environmentWorkspaceEdit.StartEditing(false);
                    SigmaDataCash.environmentWorkspaceEdit.StartEditOperation();

                    Type tt = cartoEl.GetType();
                    PropertyDescriptorCollection pdc = TypeDescriptor.GetProperties(cartoEl);
                    // to do change Fonts prop
                    if (FontChangedFlag) SetTheSameFontsToTextContents(cartoEl);

                    bool noteFlag = false;

                    foreach (AbstractChartElement item in SigmaDataCash.SelectedChartElements)
                    {
                        AbstractChartElement childEl = item;


                        if (childEl is ChartElement_MarkerSymbol && 
                            (childEl.RelatedFeature.CompareTo("ProcedureLeg")!=0 && (childEl.RelatedFeature.CompareTo("NoneScale") != 0)))
                        {
                            noteFlag = true;
                            childEl = (AbstractChartElement)UnPackFromNote(childEl);
                            if (childEl == null) childEl = item;
                        }


                        // to do change Fonts prop
                        if (FontChangedFlag) SetTheSameFontsToTextContents(cartoEl, childEl);

                        foreach (PropertyDescriptor pd in pdc)
                        {
                            Attribute attributeBrowsable = pd.Attributes[typeof(BrowsableAttribute)];
                            Attribute attributeSkip = pd.Attributes[typeof(SkipAttribute)];
                            if (((BrowsableAttribute)attributeBrowsable).Browsable && !((SkipAttribute)attributeSkip).SkipFlag)
                            {
                                object ObjpropVal = ArenaStaticProc.GetObjectValue(cartoEl, pd.Name);
                                if (ObjpropVal != null)
                                {
                                    if (ObjpropVal is AbstractChartClass) ObjpropVal = ((AbstractChartClass)ObjpropVal).Clone();
                                }
                                ArenaStaticProc.SetObjectValue(childEl, pd.Name, ObjpropVal);

                            }

                        }




                        IElement el = childEl.ConvertToIElement() as IElement;

                        if (!noteFlag)
                            ChartElementsManipulator.UpdateSingleElementToDataSet(childEl.Name, childEl.Id.ToString(), el, ref childEl);
                        else
                        {
                            SendToNote(childEl, true);
                        }

                        noteFlag = false;
                    }

                    SigmaDataCash.environmentWorkspaceEdit.StopEditOperation();
                    SigmaDataCash.environmentWorkspaceEdit.StopEditing(true);
                }
            }
            else
            {
                // to do change Fonts prop


                /// Update ALL child elements
                ///
                SigmaDataCash.environmentWorkspaceEdit.StartEditing(false);
                SigmaDataCash.environmentWorkspaceEdit.StartEditOperation();

                Type tt = cartoEl.GetType();
                PropertyDescriptorCollection pdc = TypeDescriptor.GetProperties(cartoEl);
                // to do change Fonts prop
                if (FontChangedFlag)
                    SetTheSameFontsToTextContents(cartoEl);

                bool noteFlag = false;

                foreach (TreeNode nd in FeatureTreeView.SelectedNode.Nodes)
                {
                    AbstractChartElement childEl = (AbstractChartElement)nd.Tag;
                    if (childEl is ChartElement_SimpleText)
                    {
                        if (((ChartElement_SimpleText)childEl).TextContents[0][0].DataSource.Condition.StartsWith("NoneScale")) continue;
                        if (childEl is ChartElement_MarkerSymbol && childEl.RelatedFeature.StartsWith("MarkerNote_"))
                        {
                            noteFlag = true;
                            childEl = (AbstractChartElement)UnPackFromNote(childEl);

                        }
                        // to do change Fonts prop
                        if (FontChangedFlag)
                            SetTheSameFontsToTextContents(cartoEl, childEl);
                    }

                    string morseTxt = "";
                    if (childEl is ChartElement_SigmaCollout && cartoEl is ChartElement_SigmaCollout && ((ChartElement_SigmaCollout)childEl).MorseTextLine != null)
                        morseTxt = ((ChartElement_SigmaCollout)childEl).MorseTextLine.MorseText;

                    string arspstxt = "";
                    if (childEl is ChartElement_SigmaCollout_Airspace && cartoEl is ChartElement_SigmaCollout_Airspace && ((ChartElement_SigmaCollout_Airspace)childEl).AirspaceSign != null)
                        arspstxt = ((ChartElement_SigmaCollout_Airspace)childEl).AirspaceSign.AirspaceSymbols;

                    int i = 0;
                    int c = 0;
                    if (childEl is ChartElement_MarkerSymbol && cartoEl is ChartElement_MarkerSymbol && ((ChartElement_MarkerSymbol)childEl).MarkerBackGround != null)
                    {
                        i = ((ChartElement_MarkerSymbol)childEl).MarkerBackGround.InnerCharacterIndex;
                        c = ((ChartElement_MarkerSymbol)childEl).MarkerBackGround.CharacterIndex;
                    }

                    foreach (PropertyDescriptor pd in pdc)
                    {
                        Attribute attributeBrowsable = pd.Attributes[typeof(BrowsableAttribute)];
                        Attribute attributeSkip = pd.Attributes[typeof(SkipAttribute)];
                        if (attributeSkip != null && ((BrowsableAttribute)attributeBrowsable).Browsable && !((SkipAttribute)attributeSkip).SkipFlag)
                        {
                            object ObjpropVal = ArenaStaticProc.GetObjectValue(cartoEl, pd.Name);

                            if (ObjpropVal is AbstractChartClass) ObjpropVal = ((AbstractChartClass)ObjpropVal).Clone();
                            ArenaStaticProc.SetObjectValue(childEl, pd.Name, ObjpropVal);


                        }

                    }

                    if (childEl is ChartElement_SimpleText && cartoEl is ChartElement_SimpleText)
                        UpdateCoordStyle((ChartElement_SimpleText)childEl, ((ChartElement_SimpleText)cartoEl).CoordType);

                    if (childEl is ChartElement_SigmaCollout && cartoEl is ChartElement_SigmaCollout && ((ChartElement_SigmaCollout)childEl).MorseTextLine != null)
                        ((ChartElement_SigmaCollout)childEl).MorseTextLine.MorseText = morseTxt;

                    if (childEl is ChartElement_SigmaCollout_Airspace && cartoEl is ChartElement_SigmaCollout_Airspace && ((ChartElement_SigmaCollout_Airspace)childEl).AirspaceSign != null)
                        ((ChartElement_SigmaCollout_Airspace)childEl).AirspaceSign.AirspaceSymbols = arspstxt;

                    if (childEl is ChartElement_MarkerSymbol && cartoEl is ChartElement_MarkerSymbol && ((ChartElement_MarkerSymbol)childEl).MarkerBackGround != null)
                    {
                        ((ChartElement_MarkerSymbol)childEl).MarkerBackGround.InnerCharacterIndex = i;
                        ((ChartElement_MarkerSymbol)childEl).MarkerBackGround.CharacterIndex = c;
                    }


                    IElement el = childEl.ConvertToIElement() as IElement;

                    if (!noteFlag)
                        ChartElementsManipulator.UpdateSingleElementToDataSet(childEl.Name, childEl.Id.ToString(), el, ref childEl);
                    else
                    {
                        SendToNote(childEl, true);
                    }


                    noteFlag = false;
                }

                SigmaDataCash.environmentWorkspaceEdit.StopEditOperation();
                SigmaDataCash.environmentWorkspaceEdit.StopEditing(true);

                // 
                SavePrototypesElements();

            }

            //(pMap as IActiveView).Refresh();
            ChartElementsManipulator.RefreshChart((IMxDocument)m_application.Document);



            FontChangedFlag = false;





        }

        private void SavePrototypesElements()
        {
            #region сохранить на диске новые шаблоны прототипов


            string FN = System.IO.Path.Combine(ArenaStaticProc.GetPathToTemplate() + @"\Enroute\", "enroute.sce");
            string chrtTp = "enrt";
            if (SigmaDataCash.SigmaChartType == 2)
            {
                FN = System.IO.Path.Combine(ArenaStaticProc.GetPathToTemplate() + @"\SID\", "sid.sce");
                chrtTp = "sid";
            }
            if (SigmaDataCash.SigmaChartType == 4)
            {
                FN = System.IO.Path.Combine(ArenaStaticProc.GetPathToTemplate() + @"\STAR\", "star.sce");
                chrtTp = "star";
            }
            if (SigmaDataCash.SigmaChartType == 5)
            {
                FN = System.IO.Path.Combine(ArenaStaticProc.GetPathToTemplate() + @"\IAP\", "iap.sce");
                chrtTp = "iap";
            }
            if (SigmaDataCash.SigmaChartType == 6)
            {
                FN = System.IO.Path.Combine(ArenaStaticProc.GetPathToTemplate() + @"\PATC\", "patc.sce");
                chrtTp = "patc";
            }
            if (SigmaDataCash.SigmaChartType == 7)
            {
                FN = System.IO.Path.Combine(ArenaStaticProc.GetPathToTemplate() + @"\AreaChart\", "areachart.sce");
                chrtTp = "area";
            }
            if (SigmaDataCash.SigmaChartType == 8 || SigmaDataCash.SigmaChartType == 9 || SigmaDataCash.SigmaChartType == 10 || SigmaDataCash.SigmaChartType == 11 || SigmaDataCash.SigmaChartType == 12)
            {
                FN = System.IO.Path.Combine(ArenaStaticProc.GetPathToTemplate() + @"\AerodromeElectronicChart\", "aerodrome.sce");
                chrtTp = "aerd";
            }
            if (SigmaDataCash.SigmaChartType == 13)
            {
                FN = System.IO.Path.Combine(ArenaStaticProc.GetPathToTemplate() + @"\MinimumAltitudeChart\", "minalt.sce");
                chrtTp = "areamin";
            }


            Chart_ObjectsList obj = new Chart_ObjectsList { ChartType = chrtTp, List = SigmaDataCash.prototype_anno_lst };


            //Object obj = acntElement;

            XmlSerializer xmlSer = new XmlSerializer(obj.GetType());
            MemoryStream strmMemSer = new MemoryStream();
            xmlSer.Serialize(strmMemSer, obj);



            byte[] byteArr = new byte[strmMemSer.Length];
            strmMemSer.Position = 0;
            int count = strmMemSer.Read(byteArr, 0, byteArr.Length);
            if (count != byteArr.Length)
            {
                strmMemSer.Close();
                Console.WriteLine("Test Failed: Unable to read data from file");
                return;
            }


            if (File.Exists(FN)) File.Delete(FN);
            System.IO.FileStream strmFileCompressed = new System.IO.FileStream(FN, FileMode.CreateNew);
            strmMemSer.WriteTo(strmFileCompressed);
            strmMemSer.Close();
            strmFileCompressed.Close();
            #endregion

        }

        private void SetTheSameFontsToTextContents(AbstractChartElement prototypeCartoElement)
        {
            if (!(prototypeCartoElement is ChartElement_SimpleText)) return;

            if (prototypeCartoElement.Name.CompareTo("AMA_Text") == 0) return;

            if (ArenaStaticProc.GetObjectValue(prototypeCartoElement, "TextContents") == null) return;

            ChartElement_SimpleText simpleText = (ChartElement_SimpleText)prototypeCartoElement;

            #region TextContents

            foreach (var Line in simpleText.TextContents)
            {
                foreach (var word in Line)
                {
                    if (!word.Morse)
                    {
                        word.Font.Name = simpleText.Font.Name;
                        //word.Font.Bold = simpleText.Font.Bold;
                        //word.Font.UnderLine = simpleText.Font.UnderLine;
                        //word.Font.Italic = simpleText.Font.Italic;
                    }
                    word.Font.Size = simpleText.Font.Size;

                    if (word.TextValue.ToUpper().CompareTo("NAN") != 0)
                    {
                        word.Font.FontColor.AncorColor_CMYK = simpleText.Font.FontColor.AncorColor_CMYK;
                        word.Font.FontColor.AncorColor_NullColor = simpleText.Font.FontColor.AncorColor_NullColor;
                        word.Font.FontColor.AncorColor_RGB = simpleText.Font.FontColor.AncorColor_RGB;
                        word.Font.FontColor.AncorColor_Transparency = simpleText.Font.FontColor.AncorColor_Transparency;
                        word.Font.FontColor.AncorColor_UseWindowsDithering = simpleText.Font.FontColor.AncorColor_UseWindowsDithering;

                    }

                    if (word.StartSymbol != null)
                    {
                        AncorSymbol smbl = word.StartSymbol;
                        if (!smbl.TextFont.Name.StartsWith("AeroSigma")) smbl.TextFont.Name = simpleText.Font.Name;
                        //smbl.TextFont.Bold = simpleText.Font.Bold;
                        //smbl.TextFont.UnderLine = simpleText.Font.UnderLine;
                        //smbl.TextFont.Italic = simpleText.Font.Italic;
                        smbl.TextFont.Size = simpleText.Font.Size;

                        smbl.TextFont.FontColor.AncorColor_CMYK = simpleText.Font.FontColor.AncorColor_CMYK;
                        smbl.TextFont.FontColor.AncorColor_NullColor = simpleText.Font.FontColor.AncorColor_NullColor;
                        smbl.TextFont.FontColor.AncorColor_RGB = simpleText.Font.FontColor.AncorColor_RGB;
                        smbl.TextFont.FontColor.AncorColor_Transparency = simpleText.Font.FontColor.AncorColor_Transparency;
                        smbl.TextFont.FontColor.AncorColor_UseWindowsDithering = simpleText.Font.FontColor.AncorColor_UseWindowsDithering;

                    }

                    if (word.EndSymbol != null)
                    {
                        AncorSymbol smbl = word.EndSymbol;
                        if (!smbl.TextFont.Name.StartsWith("AeroSigma")) smbl.TextFont.Name = simpleText.Font.Name;
                        //smbl.TextFont.Bold = simpleText.Font.Bold;
                        //smbl.TextFont.UnderLine = simpleText.Font.UnderLine;
                        //smbl.TextFont.Italic = simpleText.Font.Italic;
                        smbl.TextFont.Size = simpleText.Font.Size;

                        smbl.TextFont.FontColor.AncorColor_CMYK = simpleText.Font.FontColor.AncorColor_CMYK;
                        smbl.TextFont.FontColor.AncorColor_NullColor = simpleText.Font.FontColor.AncorColor_NullColor;
                        smbl.TextFont.FontColor.AncorColor_RGB = simpleText.Font.FontColor.AncorColor_RGB;
                        smbl.TextFont.FontColor.AncorColor_Transparency = simpleText.Font.FontColor.AncorColor_Transparency;
                        smbl.TextFont.FontColor.AncorColor_UseWindowsDithering = simpleText.Font.FontColor.AncorColor_UseWindowsDithering;

                    }
                }
            }

            #endregion

            #region ChartElement_BorderedText_Collout_CaptionBottom

            if (prototypeCartoElement is ChartElement_BorderedText_Collout_CaptionBottom)
            {

                ChartElement_BorderedText_Collout_CaptionBottom CapTopnText = (ChartElement_BorderedText_Collout_CaptionBottom)prototypeCartoElement;

                #region CaptionTextLine

                if (CapTopnText.CaptionTextLine != null)
                {
                    foreach (var Line in CapTopnText.CaptionTextLine)
                    {
                        foreach (var word in Line)
                        {
                            if (!word.Morse)
                            {
                                word.Font.Name = simpleText.Font.Name;
                                //word.Font.Bold = simpleText.Font.Bold;
                                //word.Font.UnderLine = simpleText.Font.UnderLine;
                                //word.Font.Italic = simpleText.Font.Italic;
                            }
                            word.Font.Size = simpleText.Font.Size;

                            if (word.TextValue.ToUpper().CompareTo("NAN") != 0)
                            {
                                word.Font.FontColor.AncorColor_CMYK = simpleText.Font.FontColor.AncorColor_CMYK;
                                word.Font.FontColor.AncorColor_NullColor = simpleText.Font.FontColor.AncorColor_NullColor;
                                word.Font.FontColor.AncorColor_RGB = simpleText.Font.FontColor.AncorColor_RGB;
                                word.Font.FontColor.AncorColor_Transparency = simpleText.Font.FontColor.AncorColor_Transparency;
                                word.Font.FontColor.AncorColor_UseWindowsDithering = simpleText.Font.FontColor.AncorColor_UseWindowsDithering;

                            }

                        }
                    }
                }

                #endregion

                #region BottomTextLine

                if (CapTopnText != null)
                {
                    foreach (var Line in CapTopnText.BottomTextLine)
                    {
                        foreach (var word in Line)
                        {
                            if (!word.Morse)
                            {
                                word.Font.Name = simpleText.Font.Name;
                                //word.Font.Bold = simpleText.Font.Bold;
                                //word.Font.UnderLine = simpleText.Font.UnderLine;
                                //word.Font.Italic = simpleText.Font.Italic;
                            }
                            word.Font.Size = simpleText.Font.Size;

                            if (word.TextValue.ToUpper().CompareTo("NAN") != 0)
                            {
                                word.Font.FontColor.AncorColor_CMYK = simpleText.Font.FontColor.AncorColor_CMYK;
                                word.Font.FontColor.AncorColor_NullColor = simpleText.Font.FontColor.AncorColor_NullColor;
                                word.Font.FontColor.AncorColor_RGB = simpleText.Font.FontColor.AncorColor_RGB;
                                word.Font.FontColor.AncorColor_Transparency = simpleText.Font.FontColor.AncorColor_Transparency;
                                word.Font.FontColor.AncorColor_UseWindowsDithering = simpleText.Font.FontColor.AncorColor_UseWindowsDithering;
                            }

                        }
                    }
                }

                #endregion


            }

            #endregion

            #region ChartElement_RouteDesignator

            if (prototypeCartoElement is ChartElement_RouteDesignator)
            {

                ChartElement_RouteDesignator routDesigText = (ChartElement_RouteDesignator)prototypeCartoElement;

                #region CaptionTextLine

                if (routDesigText.RouteDesignatorSource != null)
                {
                    foreach (var Line in routDesigText.RouteDesignatorSource)
                    {
                        foreach (var word in Line)
                        {
                            if (!word.Morse)
                            {
                                word.Font.Name = simpleText.Font.Name;
                                //word.Font.Bold = simpleText.Font.Bold;
                                //word.Font.UnderLine = simpleText.Font.UnderLine;
                                //word.Font.Italic = simpleText.Font.Italic;
                            }
                            word.Font.Size = simpleText.Font.Size;

                            if (word.TextValue.ToUpper().CompareTo("NAN") != 0)
                            {
                                word.Font.FontColor.AncorColor_CMYK = simpleText.Font.FontColor.AncorColor_CMYK;
                                word.Font.FontColor.AncorColor_NullColor = simpleText.Font.FontColor.AncorColor_NullColor;
                                word.Font.FontColor.AncorColor_RGB = simpleText.Font.FontColor.AncorColor_RGB;
                                word.Font.FontColor.AncorColor_Transparency = simpleText.Font.FontColor.AncorColor_Transparency;
                                word.Font.FontColor.AncorColor_UseWindowsDithering = simpleText.Font.FontColor.AncorColor_UseWindowsDithering;

                            }

                        }
                    }
                }

                #endregion




            }

            #endregion


        }

        private void SetTheSameFontsToTextContents(AbstractChartElement prototypeCartoElement, AbstractChartElement CartoElement)
        {
            if (!(prototypeCartoElement is ChartElement_SimpleText)) return;
            if (!(CartoElement is ChartElement_SimpleText)) return;

            if (ArenaStaticProc.GetObjectValue(prototypeCartoElement, "TextContents") == null) return;
            if (ArenaStaticProc.GetObjectValue(CartoElement, "TextContents") == null) return;

            ChartElement_SimpleText ProtoSimpleText = (ChartElement_SimpleText)prototypeCartoElement;
            ChartElement_SimpleText SimpleText = (ChartElement_SimpleText)CartoElement;

            SimpleText.Font.Name = ProtoSimpleText.Font.Name;
            SimpleText.Font.Size = ProtoSimpleText.Font.Size;
            SimpleText.Font.Bold = ProtoSimpleText.Font.Bold;
            //SimpleText.Font.Italic = ProtoSimpleText.Font.Italic;
            //SimpleText.Font.UnderLine = ProtoSimpleText.Font.UnderLine;

            if (!ProtoSimpleText.Name.StartsWith("AMA_Text"))
            {

                #region TextContents
                int lIndex = 0;

                foreach (var Line in SimpleText.TextContents)
                {
                    int wIndex = 0;

                    foreach (var word in Line)
                    {
                        if (!word.Morse)
                        {
                            word.Font.Name = ProtoSimpleText.Font.Name;
                            word.Font.Bold = ProtoSimpleText.Font.Bold;
                            //word.Font.UnderLine = ProtoSimpleText.Font.UnderLine;
                            word.Font.Italic = ProtoSimpleText.Font.Italic;
                        }
                        word.Font.Size = ProtoSimpleText.Font.Size;


                        if (word.TextValue.ToUpper().CompareTo("NAN") != 0)
                        {
                            word.Font.FontColor.AncorColor_CMYK = ProtoSimpleText.Font.FontColor.AncorColor_CMYK;
                            word.Font.FontColor.AncorColor_NullColor = ProtoSimpleText.Font.FontColor.AncorColor_NullColor;
                            word.Font.FontColor.AncorColor_RGB = ProtoSimpleText.Font.FontColor.AncorColor_RGB;
                            word.Font.FontColor.AncorColor_UseWindowsDithering = ProtoSimpleText.Font.FontColor.AncorColor_UseWindowsDithering;
                            word.Font.FontColor.AncorColor_Transparency = ProtoSimpleText.Font.FontColor.AncorColor_Transparency;
                        }

                        if (word.StartSymbol != null)
                        {
                            AncorSymbol smbl = word.StartSymbol;
                            if (!smbl.TextFont.Name.StartsWith("AeroSigma"))
                                smbl.TextFont.Name = ProtoSimpleText.Font.Name;
                            smbl.TextFont.Bold = ProtoSimpleText.Font.Bold;
                            smbl.TextFont.UnderLine = ProtoSimpleText.Font.UnderLine;
                            smbl.TextFont.Italic = ProtoSimpleText.Font.Italic;
                            smbl.TextFont.Size = ProtoSimpleText.Font.Size;

                            smbl.TextFont.FontColor.AncorColor_CMYK = ProtoSimpleText.Font.FontColor.AncorColor_CMYK;
                            smbl.TextFont.FontColor.AncorColor_NullColor = ProtoSimpleText.Font.FontColor.AncorColor_NullColor;
                            smbl.TextFont.FontColor.AncorColor_RGB = ProtoSimpleText.Font.FontColor.AncorColor_RGB;
                            smbl.TextFont.FontColor.AncorColor_Transparency = ProtoSimpleText.Font.FontColor.AncorColor_Transparency;
                            smbl.TextFont.FontColor.AncorColor_UseWindowsDithering = ProtoSimpleText.Font.FontColor.AncorColor_UseWindowsDithering;
                        }

                        if (word.EndSymbol != null)
                        {
                            AncorSymbol smbl = word.EndSymbol;
                            if (!smbl.TextFont.Name.StartsWith("AeroSigma"))
                                smbl.TextFont.Name = ProtoSimpleText.Font.Name;
                            smbl.TextFont.Bold = ProtoSimpleText.Font.Bold;
                            smbl.TextFont.UnderLine = ProtoSimpleText.Font.UnderLine;
                            smbl.TextFont.Italic = ProtoSimpleText.Font.Italic;
                            smbl.TextFont.Size = ProtoSimpleText.Font.Size;

                            smbl.TextFont.FontColor.AncorColor_CMYK = ProtoSimpleText.Font.FontColor.AncorColor_CMYK;
                            smbl.TextFont.FontColor.AncorColor_NullColor = ProtoSimpleText.Font.FontColor.AncorColor_NullColor;
                            smbl.TextFont.FontColor.AncorColor_RGB = ProtoSimpleText.Font.FontColor.AncorColor_RGB;
                            smbl.TextFont.FontColor.AncorColor_Transparency = ProtoSimpleText.Font.FontColor.AncorColor_Transparency;
                            smbl.TextFont.FontColor.AncorColor_UseWindowsDithering = ProtoSimpleText.Font.FontColor.AncorColor_UseWindowsDithering;
                        }

                        if (ProtoSimpleText.TextContents.Count > lIndex)
                        {
                            if (ProtoSimpleText.TextContents[lIndex].Count > wIndex)
                                word.Visible = ProtoSimpleText.TextContents[lIndex][wIndex].Visible;
                        }
                        wIndex++;

                    }


                    lIndex++;

                }

                #endregion


                if (CartoElement is ChartElement_BorderedText_Collout_CaptionBottom)
                {

                    ChartElement_BorderedText_Collout_CaptionBottom CapTopnText = (ChartElement_BorderedText_Collout_CaptionBottom)CartoElement;

                    #region CaptionTextLine

                    if (CapTopnText.CaptionTextLine != null)
                    {
                        foreach (var Line in CapTopnText.CaptionTextLine)
                        {
                            foreach (var word in Line)
                            {
                                if (!word.Morse)
                                {
                                    word.Font.Name = ProtoSimpleText.Font.Name;
                                    word.Font.Bold = ProtoSimpleText.Font.Bold;
                                    word.Font.UnderLine = ProtoSimpleText.Font.UnderLine;
                                    word.Font.Italic = ProtoSimpleText.Font.Italic;
                                }
                                word.Font.Size = ProtoSimpleText.Font.Size;

                                if (word.TextValue.ToUpper().CompareTo("NAN") != 0)
                                {
                                    word.Font.FontColor.AncorColor_CMYK = ProtoSimpleText.Font.FontColor.AncorColor_CMYK;
                                    word.Font.FontColor.AncorColor_NullColor = ProtoSimpleText.Font.FontColor.AncorColor_NullColor;
                                    word.Font.FontColor.AncorColor_RGB = ProtoSimpleText.Font.FontColor.AncorColor_RGB;
                                    word.Font.FontColor.AncorColor_Transparency = ProtoSimpleText.Font.FontColor.AncorColor_Transparency;
                                    word.Font.FontColor.AncorColor_UseWindowsDithering = ProtoSimpleText.Font.FontColor.AncorColor_UseWindowsDithering;
                                }

                            }
                        }
                    }

                    #endregion

                    #region BottomTextLine

                    if (CapTopnText != null)
                    {
                        foreach (var Line in CapTopnText.BottomTextLine)
                        {
                            foreach (var word in Line)
                            {
                                if (!word.Morse)
                                {
                                    word.Font.Name = ProtoSimpleText.Font.Name;
                                    word.Font.Bold = ProtoSimpleText.Font.Bold;
                                    word.Font.UnderLine = ProtoSimpleText.Font.UnderLine;
                                    word.Font.Italic = ProtoSimpleText.Font.Italic;
                                }
                                word.Font.Size = ProtoSimpleText.Font.Size;

                                if (word.TextValue.ToUpper().CompareTo("NAN") != 0)
                                {
                                    word.Font.FontColor.AncorColor_CMYK = ProtoSimpleText.Font.FontColor.AncorColor_CMYK;
                                    word.Font.FontColor.AncorColor_NullColor = ProtoSimpleText.Font.FontColor.AncorColor_NullColor;
                                    word.Font.FontColor.AncorColor_RGB = ProtoSimpleText.Font.FontColor.AncorColor_RGB;
                                    word.Font.FontColor.AncorColor_Transparency = ProtoSimpleText.Font.FontColor.AncorColor_Transparency;
                                    word.Font.FontColor.AncorColor_UseWindowsDithering = ProtoSimpleText.Font.FontColor.AncorColor_UseWindowsDithering;
                                }

                            }
                        }
                    }

                    #endregion


                }

                if (CartoElement is ChartElement_RouteDesignator)
                {
                    #region

                    ChartElement_RouteDesignator _RouteDesignatorSource = (ChartElement_RouteDesignator)CartoElement;

                    if (_RouteDesignatorSource.RouteDesignatorSource != null)
                    {
                        foreach (var Line in _RouteDesignatorSource.RouteDesignatorSource)
                        {
                            foreach (var word in Line)
                            {
                                if (!word.Morse)
                                {
                                    word.Font.Name = ProtoSimpleText.Font.Name;
                                    word.Font.Bold = ProtoSimpleText.Font.Bold;
                                    word.Font.UnderLine = ProtoSimpleText.Font.UnderLine;
                                    word.Font.Italic = ProtoSimpleText.Font.Italic;
                                }
                                word.Font.Size = ProtoSimpleText.Font.Size;

                                if (word.TextValue.ToUpper().CompareTo("NAN") != 0)
                                {
                                    word.Font.FontColor.AncorColor_CMYK = ProtoSimpleText.Font.FontColor.AncorColor_CMYK;
                                    word.Font.FontColor.AncorColor_NullColor = ProtoSimpleText.Font.FontColor.AncorColor_NullColor;
                                    word.Font.FontColor.AncorColor_RGB = ProtoSimpleText.Font.FontColor.AncorColor_RGB;
                                    word.Font.FontColor.AncorColor_Transparency = ProtoSimpleText.Font.FontColor.AncorColor_Transparency;
                                    word.Font.FontColor.AncorColor_UseWindowsDithering = ProtoSimpleText.Font.FontColor.AncorColor_UseWindowsDithering;
                                }

                            }
                        }
                    }
                    #endregion

                }
            }
            else
            {
                #region AMA Text

                #region TextContents

                //for (int lnIndx  SimpleText.TextContents)
                for (int lnIndx = 0; lnIndx < SimpleText.TextContents.Count; lnIndx++)
                {
                    var txtLine = SimpleText.TextContents[lnIndx];
                    //foreach (var word in Line)
                    for (int wrdIndx = 0; wrdIndx < txtLine.Count; wrdIndx++)
                    {
                        AncorChartElementWord word = txtLine[wrdIndx];
                        if (!word.Morse)
                        {
                            word.Font.Name = ProtoSimpleText.TextContents[lnIndx][wrdIndx].Font.Name;
                            word.Font.Bold = ProtoSimpleText.TextContents[lnIndx][wrdIndx].Font.Bold;
                            word.Font.UnderLine = ProtoSimpleText.TextContents[lnIndx][wrdIndx].Font.UnderLine;
                            word.Font.Italic = ProtoSimpleText.TextContents[lnIndx][wrdIndx].Font.Italic;
                        }
                        word.Font.Size = ProtoSimpleText.TextContents[lnIndx][wrdIndx].Font.Size;


                        if (word.TextValue.ToUpper().CompareTo("NAN") != 0)
                        {
                            word.Font.FontColor.AncorColor_CMYK = ProtoSimpleText.TextContents[lnIndx][wrdIndx].Font.FontColor.AncorColor_CMYK;
                            word.Font.FontColor.AncorColor_NullColor = ProtoSimpleText.TextContents[lnIndx][wrdIndx].Font.FontColor.AncorColor_NullColor;
                            word.Font.FontColor.AncorColor_RGB = ProtoSimpleText.TextContents[lnIndx][wrdIndx].Font.FontColor.AncorColor_RGB;
                            word.Font.FontColor.AncorColor_Transparency = ProtoSimpleText.TextContents[lnIndx][wrdIndx].Font.FontColor.AncorColor_Transparency;
                            word.Font.FontColor.AncorColor_UseWindowsDithering = ProtoSimpleText.TextContents[lnIndx][wrdIndx].Font.FontColor.AncorColor_UseWindowsDithering;
                        }

                        if (word.StartSymbol != null)
                        {
                            AncorSymbol smbl = word.StartSymbol;
                            smbl.TextFont.Name = ProtoSimpleText.TextContents[lnIndx][wrdIndx].StartSymbol.TextFont.Name;
                            smbl.TextFont.Bold = ProtoSimpleText.TextContents[lnIndx][wrdIndx].StartSymbol.TextFont.Bold;
                            smbl.TextFont.UnderLine = ProtoSimpleText.TextContents[lnIndx][wrdIndx].StartSymbol.TextFont.UnderLine;
                            smbl.TextFont.Italic = ProtoSimpleText.TextContents[lnIndx][wrdIndx].StartSymbol.TextFont.Italic;
                            smbl.TextFont.Size = ProtoSimpleText.TextContents[lnIndx][wrdIndx].StartSymbol.TextFont.Size;

                            smbl.TextFont.FontColor.AncorColor_CMYK = ProtoSimpleText.TextContents[lnIndx][wrdIndx].StartSymbol.TextFont.FontColor.AncorColor_CMYK;
                            smbl.TextFont.FontColor.AncorColor_NullColor = ProtoSimpleText.TextContents[lnIndx][wrdIndx].StartSymbol.TextFont.FontColor.AncorColor_NullColor;
                            smbl.TextFont.FontColor.AncorColor_RGB = ProtoSimpleText.TextContents[lnIndx][wrdIndx].StartSymbol.TextFont.FontColor.AncorColor_RGB;
                            smbl.TextFont.FontColor.AncorColor_Transparency = ProtoSimpleText.TextContents[lnIndx][wrdIndx].StartSymbol.TextFont.FontColor.AncorColor_Transparency;
                            smbl.TextFont.FontColor.AncorColor_UseWindowsDithering = ProtoSimpleText.TextContents[lnIndx][wrdIndx].StartSymbol.TextFont.FontColor.AncorColor_UseWindowsDithering;
                        }

                        if (word.EndSymbol != null)
                        {
                            AncorSymbol smbl = word.EndSymbol;
                            smbl.TextFont.Name = ProtoSimpleText.TextContents[lnIndx][wrdIndx].EndSymbol.TextFont.Name;
                            smbl.TextFont.Bold = ProtoSimpleText.TextContents[lnIndx][wrdIndx].EndSymbol.TextFont.Bold;
                            smbl.TextFont.UnderLine = ProtoSimpleText.TextContents[lnIndx][wrdIndx].EndSymbol.TextFont.UnderLine;
                            smbl.TextFont.Italic = ProtoSimpleText.TextContents[lnIndx][wrdIndx].EndSymbol.TextFont.Italic;
                            smbl.TextFont.Size = ProtoSimpleText.TextContents[lnIndx][wrdIndx].EndSymbol.TextFont.Size;

                            smbl.TextFont.FontColor.AncorColor_CMYK = ProtoSimpleText.TextContents[lnIndx][wrdIndx].EndSymbol.TextFont.FontColor.AncorColor_CMYK;
                            smbl.TextFont.FontColor.AncorColor_NullColor = ProtoSimpleText.TextContents[lnIndx][wrdIndx].EndSymbol.TextFont.FontColor.AncorColor_NullColor;
                            smbl.TextFont.FontColor.AncorColor_RGB = ProtoSimpleText.TextContents[lnIndx][wrdIndx].EndSymbol.TextFont.FontColor.AncorColor_RGB;
                            smbl.TextFont.FontColor.AncorColor_Transparency = ProtoSimpleText.TextContents[lnIndx][wrdIndx].EndSymbol.TextFont.FontColor.AncorColor_Transparency;
                            smbl.TextFont.FontColor.AncorColor_UseWindowsDithering = ProtoSimpleText.TextContents[lnIndx][wrdIndx].EndSymbol.TextFont.FontColor.AncorColor_UseWindowsDithering;
                        }
                    }
                }

                #endregion

                #endregion
            }

        }


        private bool IsPrototypeElement(AbstractChartElement cartoEl)
        {
            var obj = (from element in SigmaDataCash.prototype_anno_lst where (element != null) && (element.Id.Equals(cartoEl.Id)) select element).FirstOrDefault();
            return (obj != null);


        }

        private void hideSelectedObjectToolStripMenuItem_Click(object sender, EventArgs e)
        {

            UnselectFeatureClasses();

            if (FeatureTreeView.SelectedNode == null || FeatureTreeView.SelectedNode.Tag == null) return;

            if (FeatureTreeView.SelectedNode.Tag is ChartElement_SimpleText) HideElement();
            else if (FeatureTreeView.SelectedNode.Tag is GraphicsChartElement) HideElement((GraphicsChartElement)FeatureTreeView.SelectedNode.Tag);
            else if (FeatureTreeView.SelectedNode.Tag is ChartElement_ILSCollout) HideElement();

            //(pMap as IActiveView).Refresh();
            ChartElementsManipulator.RefreshChart((IMxDocument)m_application.Document);

        }

        private void UnselectFeatureClasses()
        {
            foreach (KeyValuePair<string, object> item in SigmaDataCash.AnnotationFeatureClassList)
            {
                if (!(item.Value is IFeatureClass)) continue;


                IFeatureClass Anno_featClass = (IFeatureClass)item.Value;
                ILayer _Layer = EsriUtils.getLayerByName(pMap, Anno_featClass.AliasName);

                if (_Layer == null || !_Layer.Visible) continue;

                /////////////
                IFeatureSelection pSelect = (IFeatureSelection)_Layer;
                if (pSelect != null) pSelect.Clear();

                ////////////

            }
        }

        private void hideReflectionMenuItem_Click(object sender, EventArgs e)
        {
            IMxDocument document = (IMxDocument)m_application.Document;
            IMaps maps = document.Maps;



            AbstractChartElement cartoEl = (AbstractChartElement)FeatureTreeView.SelectedNode.Tag;

            cartoEl.ReflectionHidden = !cartoEl.ReflectionHidden;
            int status = cartoEl.ReflectionHidden ? 1 : 0;


            ChartElementsManipulator.HideReflection(cartoEl.Id.ToString(), status);

            for (int i = 0; i <= maps.Count - 1; i++)
            {

                IMap map = maps.get_Item(i);
                IFrameElement frameElement = pGraphicsContainer.FindFrame(maps.get_Item(i));
                IMapFrame mapFrame = frameElement as IMapFrame;

                
                (mapFrame.Map as IActiveView).Refresh();
                (map as IActiveView).Refresh();
            }


            //(pMap as IActiveView).Refresh();

            ChartElementsManipulator.RefreshChart((IMxDocument)m_application.Document);

            (pGraphicsContainer as IActiveView).Refresh();

            updateToolStripMenuItem_Click(sender, e);

            if (cartoEl.Placed && !cartoEl.ReflectionHidden)
                FeatureTreeView.SelectedNode.NodeFont = new System.Drawing.Font(new FontFamily("Times New Roman"), 10, FontStyle.Regular);
            else if (cartoEl.Placed && cartoEl.ReflectionHidden)
                FeatureTreeView.SelectedNode.NodeFont = new System.Drawing.Font(new FontFamily("Times New Roman"), 10, FontStyle.Italic);
            else if (!cartoEl.Placed && !cartoEl.ReflectionHidden)
                FeatureTreeView.SelectedNode.NodeFont = new System.Drawing.Font(new FontFamily("Times New Roman"), 10, FontStyle.Strikeout);
            else
                FeatureTreeView.SelectedNode.NodeFont = new System.Drawing.Font(new FontFamily("Times New Roman"), 10, FontStyle.Strikeout | FontStyle.Italic);

            //ChartElementsManipulator.GetClickedChartElement(((IMxDocument)m_application.Document).FocusMap, 0, 0, "SigmaFrame");


        }

        private void HideElement(ChartElement_SimpleText chartElement_SimpleText)
        {
            AbstractChartElement cartoEl = (AbstractChartElement)FeatureTreeView.SelectedNode.Tag;

            ILayer SelLayer = EsriUtils.getLayerByName2(pMap, ChartElementsManipulator.DefinelayerName(cartoEl.Name));
            if (SelLayer == null && !IsPrototypeElement(cartoEl)) return;
            if (!SelLayer.Visible && !IsPrototypeElement(cartoEl)) return;
            SigmaDataCash.environmentWorkspaceEdit.StartEditing(false);
            SigmaDataCash.environmentWorkspaceEdit.StartEditOperation();

            if (!IsPrototypeElement(cartoEl))
            {
                HideElementNode_SimpleText(FeatureTreeView.SelectedNode);
            }
            else
            {
                if (FeatureTreeView.SelectedNode.Nodes.Count > 0)
                {
                    HideElementNode_SimpleText(FeatureTreeView.SelectedNode);

                    foreach (TreeNode item in FeatureTreeView.SelectedNode.Nodes)
                    {
                        HideElementNode_SimpleText(item);
                    }
                }
            }

            SigmaDataCash.environmentWorkspaceEdit.StopEditOperation();
            SigmaDataCash.environmentWorkspaceEdit.StopEditing(true);
        }

        private void HideElement(GraphicsChartElement chartElement_GraphicsElement)
        {
            AbstractChartElement cartoEl = (AbstractChartElement)FeatureTreeView.SelectedNode.Tag;

            SigmaDataCash.environmentWorkspaceEdit.StartEditing(false);
            SigmaDataCash.environmentWorkspaceEdit.StartEditOperation();

            if (!IsPrototypeElement(cartoEl))
            {
                HideElementNode_GraphicsElement(FeatureTreeView.SelectedNode);
            }
            else
            {
                if (FeatureTreeView.SelectedNode.Nodes.Count > 0)
                {
                    HideElementNode_SimpleText(FeatureTreeView.SelectedNode);

                    foreach (TreeNode item in FeatureTreeView.SelectedNode.Nodes)
                    {
                        HideElementNode_GraphicsElement(item);
                    }
                }
            }

            SigmaDataCash.environmentWorkspaceEdit.StopEditOperation();
            SigmaDataCash.environmentWorkspaceEdit.StopEditing(true);
        }

        private void HideElement()
        {
            AbstractChartElement cartoEl = (AbstractChartElement)FeatureTreeView.SelectedNode.Tag;

            ILayer SelLayer = EsriUtils.getLayerByName2(pMap, ChartElementsManipulator.DefinelayerName(cartoEl.Name));
            if (SelLayer == null && (cartoEl.Name.CompareTo("HoldingPatternOutboundCource") == 0 || cartoEl.Name.CompareTo("HoldingPatternInboundCource") == 0))
            {
                //ProcedureLegsAnnoCourseCartography
                SelLayer = EsriUtils.getLayerByName2(pMap, "ProcedureLegsAnnoCourseCartography");
            }

            if (SelLayer == null && !IsPrototypeElement(cartoEl)) return;
            SelLayer.Visible = true;
            if (!SelLayer.Visible && !IsPrototypeElement(cartoEl)) return;
            SigmaDataCash.environmentWorkspaceEdit.StartEditing(false);
            SigmaDataCash.environmentWorkspaceEdit.StartEditOperation();

            if (!IsPrototypeElement(cartoEl))
            {
                HideElementNode_SimpleText(FeatureTreeView.SelectedNode);
            }
            else
            {
                if (FeatureTreeView.SelectedNode.Nodes.Count > 0)
                {
                    HideElementNode_SimpleText(FeatureTreeView.SelectedNode);

                    foreach (TreeNode item in FeatureTreeView.SelectedNode.Nodes)
                    {
                        HideElementNode_SimpleText(item);
                    }
                }
            }

            SigmaDataCash.environmentWorkspaceEdit.StopEditOperation();
            SigmaDataCash.environmentWorkspaceEdit.StopEditing(true);
        }



  
        private void HideElementNode_SimpleText(TreeNode nd)
        {

            if (SigmaDataCash.SelectedChartElements == null || SigmaDataCash.SelectedChartElements.Count <= 1)
            {

                AbstractChartElement cartoEl = (AbstractChartElement)nd.Tag;


                int status = cartoEl.Placed ? 1 : 0;
                cartoEl.Placed = !cartoEl.Placed;
                //ChartElementsManipulator.HideSingleElement(cartoEl.Name, cartoEl.Id.ToString(), status, ref cartoEl);
                ChartElementsManipulator.HideSingleElement(status, ref cartoEl);

                if (cartoEl.Placed && !cartoEl.ReflectionHidden)
                    nd.NodeFont = new System.Drawing.Font(new FontFamily("Times New Roman"), 10, FontStyle.Regular);
                else if (cartoEl.Placed && cartoEl.ReflectionHidden)
                    nd.NodeFont = new System.Drawing.Font(new FontFamily("Times New Roman"), 10, FontStyle.Italic);
                else if (!cartoEl.Placed && !cartoEl.ReflectionHidden)
                    nd.NodeFont = new System.Drawing.Font(new FontFamily("Times New Roman"), 10, FontStyle.Strikeout);
                else
                    nd.NodeFont = new System.Drawing.Font(new FontFamily("Times New Roman"), 10, FontStyle.Strikeout | FontStyle.Italic);

                if (nd.NodeFont.Strikeout) nd.ForeColor = System.Drawing.Color.Coral;
                else nd.ForeColor = System.Drawing.SystemColors.ControlText;
            }
            else
            {
                foreach (var item in SigmaDataCash.SelectedChartElements)
                {
                    AbstractChartElement cartoEl = item;

                    int status = cartoEl.Placed ? 1 : 0;
                    cartoEl.Placed = !cartoEl.Placed;
                    //ChartElementsManipulator.HideSingleElement(cartoEl.Name, cartoEl.Id.ToString(), status, ref cartoEl);
                    ChartElementsManipulator.HideSingleElement(status, ref cartoEl);

                    foreach (TreeNode ParNd in FeatureTreeView.Nodes)
                    {
                        TreeNode[] res = ParNd.Nodes.Find(cartoEl.Id.ToString(), true);
                        if ((res == null) || (res.Length <= 0)) continue;

                        TreeNode SelectedNode = res[0];

                        if (cartoEl.Placed && !cartoEl.ReflectionHidden)
                            FeatureTreeView.SelectedNode.NodeFont = new System.Drawing.Font(new FontFamily("Times New Roman"), 10, FontStyle.Regular);
                        else if (cartoEl.Placed && cartoEl.ReflectionHidden)
                            FeatureTreeView.SelectedNode.NodeFont = new System.Drawing.Font(new FontFamily("Times New Roman"), 10, FontStyle.Italic);
                        else if (!cartoEl.Placed && !cartoEl.ReflectionHidden)
                            FeatureTreeView.SelectedNode.NodeFont = new System.Drawing.Font(new FontFamily("Times New Roman"), 10, FontStyle.Strikeout);
                        else
                            FeatureTreeView.SelectedNode.NodeFont = new System.Drawing.Font(new FontFamily("Times New Roman"), 10, FontStyle.Strikeout | FontStyle.Italic);

                        if (FeatureTreeView.SelectedNode.NodeFont.Strikeout) FeatureTreeView.SelectedNode.ForeColor = System.Drawing.Color.Coral;
                        else FeatureTreeView.SelectedNode.ForeColor = System.Drawing.SystemColors.ControlText;

                    }


                }

                SigmaDataCash.SelectedChartElements.Clear();
            }
        }


        private void HideElementNode_GraphicsElement(TreeNode nd)
        {

            if (SigmaDataCash.SelectedChartElements == null || SigmaDataCash.SelectedChartElements.Count <= 1)
            {

                AbstractChartElement cartoEl = (AbstractChartElement)nd.Tag;

                int status = cartoEl.Placed ? 1 : 0;
                cartoEl.Placed = !cartoEl.Placed;
                //ChartElementsManipulator.HideSingleElement(cartoEl.Name, cartoEl.Id.ToString(), status, ref cartoEl);
                UpdateElement(nd);

                if (cartoEl.Placed)
                    nd.NodeFont = new System.Drawing.Font(new FontFamily("Times New Roman"), 10, FontStyle.Regular);
                else
                    nd.NodeFont = new System.Drawing.Font(new FontFamily("Times New Roman"), 10, FontStyle.Italic | FontStyle.Strikeout);
            }
            else
            {
                foreach (var item in SigmaDataCash.SelectedChartElements)
                {
                    AbstractChartElement cartoEl = item;

                    int status = cartoEl.Placed ? 1 : 0;
                    cartoEl.Placed = !cartoEl.Placed;
                    //ChartElementsManipulator.HideSingleElement(cartoEl.Name, cartoEl.Id.ToString(), status, ref cartoEl);
                    ChartElementsManipulator.HideSingleElement(status, ref cartoEl);

                    foreach (TreeNode ParNd in FeatureTreeView.Nodes)
                    {
                        TreeNode[] res = ParNd.Nodes.Find(cartoEl.Id.ToString(), true);
                        if ((res == null) || (res.Length <= 0)) continue;

                        TreeNode SelectedNode = res[0];

                        if (cartoEl.Placed)
                            SelectedNode.NodeFont = new System.Drawing.Font(new FontFamily("Times New Roman"), 10, FontStyle.Regular);
                        else
                            SelectedNode.NodeFont = new System.Drawing.Font(new FontFamily("Times New Roman"), 10, FontStyle.Italic | FontStyle.Strikeout);

                    }


                }

                SigmaDataCash.SelectedChartElements.Clear();
            }
        }

        private void contextMenuDummy_Opening(object sender, CancelEventArgs e)
        {
            if (FeatureTreeView.SelectedNode == null || FeatureTreeView.SelectedNode.Tag == null) { contextMenuDummy.Items["hideClauseToolStripMenuItem"].Enabled = false; return; }

            IMxDocument document = (IMxDocument)m_application.Document;
            IMaps maps = document.Maps;

            AbstractChartElement cartoEl = (AbstractChartElement)FeatureTreeView.SelectedNode.Tag;
            contextMenuDummy.Items["hideSelectedObjectToolStripMenuItem"].Enabled = true;//!IsPrototypeElement(cartoEl);
            if (cartoEl.Placed)
            {
                contextMenuDummy.Items["hideSelectedObjectToolStripMenuItem"].Text = "Hide selected annotation";
            }
            else
            {
                contextMenuDummy.Items["hideSelectedObjectToolStripMenuItem"].Text = "Unhide selected annotation";
            }



            contextMenuDummy.Items["hideReflectionMenuItem"].Visible = maps.Count > 1;

            if (cartoEl.ReflectionHidden)
            {
                contextMenuDummy.Items["hideReflectionMenuItem"].Text = "Unhide selected annotations reflection";
            }
            else
            {
                contextMenuDummy.Items["hideReflectionMenuItem"].Text = "Hide selected annotations reflection";
            }

            if (cartoEl.RelatedFeature.StartsWith("MarkerNote_"))
            {
                contextMenuDummy.Items["sendToNoteToolStripMenuItem"].Text = "Restore from Note";
            }
            else
            {
                contextMenuDummy.Items["sendToNoteToolStripMenuItem"].Text = "Send to Note";
            }

            contextMenuDummy.Items["sendToNoteToolStripMenuItem"].Enabled = (!IsPrototypeElement(cartoEl));
            contextMenuDummy.Items["coordinatesStyleToolStripMenuItem"].Enabled = false;


            if (cartoEl is ChartElement_SimpleText)
            {
                foreach (var Ln in ((ChartElement_SimpleText)cartoEl).TextContents)
                {
                    foreach (var wrd in Ln)
                    {
                        if (wrd.DataSource.Value.StartsWith("Lat") || wrd.DataSource.Value.StartsWith("Lon"))
                        {
                            contextMenuDummy.Items["coordinatesStyleToolStripMenuItem"].Enabled = true;
                        }
                    }
                }
            }

            if (cartoEl is ChartElement_BorderedText_Collout_CaptionBottom || (!IsPrototypeElement(cartoEl)))
            {
                contextMenuDummy.Items["hideClauseToolStripMenuItem"].Enabled = false;
            }
            else
                contextMenuDummy.Items["hideClauseToolStripMenuItem"].Enabled = true;


            if (!IsPrototypeElement(cartoEl))
            {
                contextMenuDummy.Items["hToolStripMenuItem"].Enabled = false;
            }
            else
                contextMenuDummy.Items["hToolStripMenuItem"].Enabled = true;


            if (cartoEl is ChartElement_RouteDesignator)
            {

                contextMenuDummy.Items["attachRouteToolStripMenuItem"].Text = ((ChartElement_RouteDesignator)cartoEl).HideDesignatorText ? "Attach Route DesignatorText" : "Detach Route DesignatorText";
                contextMenuDummy.Items["attachRouteToolStripMenuItem"].Visible = true;

            }
            else
                contextMenuDummy.Items["attachRouteToolStripMenuItem"].Visible = false;

            rotateToolStripMenuItem.Visible = cartoEl is ChartElement_MarkerSymbol && cartoEl.Name.StartsWith("ProcedureLegLength") && (!IsPrototypeElement(cartoEl));


            contextMenuDummy.Items["cloneToolStripMenuItem"].Visible = true;//cartoEl.Name.StartsWith("Airspace_Class") || cartoEl.Name.StartsWith("GeoBorder_name") || cartoEl.Name.StartsWith("IsogonalLines") || cartoEl.Name.StartsWith("FreeAnno");
            contextMenuDummy.Items["deleteCloneToolStripMenuItem"].Visible = (cartoEl.Name.StartsWith("Airspace_Class") || cartoEl.Name.StartsWith("GeoBorder_name") || cartoEl.Name.StartsWith("IsogonalLines")) && cartoEl.Tag != null && cartoEl.Tag.StartsWith("Clone");
            contextMenuDummy.Items["deleteCloneToolStripMenuItem"].Visible = (cartoEl.Name.StartsWith("FreeAnno"));

            length01ToolStripMenuItem.Visible = cartoEl.Name.StartsWith("ProcedureLegLength");
            heightFLToolStripMenuItem.Visible = false;//System.Diagnostics.Debugger.IsAttached;
            textContextEditorToolStripMenuItem.Visible = cartoEl is ChartElement_SimpleText;
        }

        private void FeatureTreeView_MouseDown(object sender, MouseEventArgs e)
        {
            return;
            TreeNode currentNode = FeatureTreeView.GetNodeAt(e.Location);
            if (currentNode == null) return;
            currentNode.BackColor = FeatureTreeView.BackColor;
            currentNode.ForeColor = FeatureTreeView.ForeColor;

            bool control = (ModifierKeys == Keys.Control);
            bool shift = (ModifierKeys == Keys.Shift);

            if (control)
            {

                // the node clicked with control button pressed:
                // invert selection of the current node
                List<TreeNode> addedNodes = new List<TreeNode>();
                List<TreeNode> removedNodes = new List<TreeNode>();
                if (!selectedNodes.Contains(currentNode))
                {
                    addedNodes.Add(currentNode);
                    previousNode = currentNode;
                }
                else
                {
                    removedNodes.Add(currentNode);
                }
                changeSelection(addedNodes, removedNodes);
            }
            else if (shift && previousNode != null)
            {
                if (currentNode.Parent == previousNode.Parent)
                {
                    // the node clicked with shift button pressed:
                    // if current node and previously selected node
                    // belongs to the same parent,
                    // select range of nodes between these two
                    List<TreeNode> addedNodes = new List<TreeNode>();
                    List<TreeNode> removedNodes = new List<TreeNode>();
                    bool selection = false;
                    bool selectionEnd = false;

                    TreeNodeCollection nodes = null;
                    if (previousNode.Parent == null)
                    {
                        nodes = FeatureTreeView.Nodes;
                    }
                    else
                    {
                        nodes = previousNode.Parent.Nodes;
                    }
                    foreach (TreeNode n in nodes)
                    {
                        if (n == currentNode || n == previousNode)
                        {
                            if (selection)
                            {
                                selectionEnd = true;
                            }
                            if (!selection)
                            {
                                selection = true;
                            }
                        }
                        if (selection && !selectedNodes.Contains(n))
                        {
                            addedNodes.Add(n);
                        }
                        if (selectionEnd)
                        {
                            break;
                        }
                    }

                    if (addedNodes.Count > 0)
                    {
                        changeSelection(addedNodes, removedNodes);
                    }
                }
            }
            else
            {
                if (currentNode.NodeFont != null && !currentNode.NodeFont.Bold)
                {
                    // single click:
                    // remove all selected nodes
                    // and add current node
                    List<TreeNode> addedNodes = new List<TreeNode>();
                    List<TreeNode> removedNodes = new List<TreeNode>();
                    removedNodes.AddRange(selectedNodes);
                    if (removedNodes.Contains(currentNode))
                    {
                        removedNodes.Remove(currentNode);
                    }
                    else
                    {
                        addedNodes.Add(currentNode);
                    }
                    changeSelection(addedNodes, removedNodes);
                    previousNode = currentNode;
                }
            }



        }

        protected void changeSelection(List<TreeNode> addedNodes, List<TreeNode> removedNodes)
        {
            return;
            foreach (TreeNode n in addedNodes)
            {
                if (!n.NodeFont.Bold)
                {
                    n.BackColor = SystemColors.Highlight;
                    n.ForeColor = SystemColors.HighlightText;
                    selectedNodes.Add(n);
                }
            }
            foreach (TreeNode n in removedNodes)
            {
                n.BackColor = FeatureTreeView.BackColor;
                n.ForeColor = FeatureTreeView.ForeColor;
                selectedNodes.Remove(n);
            }
        }

        private void FeatureTreeView_BeforeSelect(object sender, TreeViewCancelEventArgs e)
        {
            return;
            e.Cancel = true;
        }

        private void coordinatesStyleToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void _nDDMMSSToolStripMenuItem_Click(object sender, EventArgs e)
        {

            AbstractChartElement cartoEl = (AbstractChartElement)FeatureTreeView.SelectedNode.Tag;
            if (cartoEl is ChartElement_SimpleText)
            {
                coordtype crdtp = (coordtype)int.Parse(((ToolStripMenuItem)sender).Tag.ToString());

                foreach (KeyValuePair<string, object> item in SigmaDataCash.AnnotationFeatureClassList)
                {
                    if (!(item.Value is IFeatureClass)) continue;

                    IFeatureClass Anno_featClass = (IFeatureClass)item.Value;
                    ILayer _Layer = EsriUtils.getLayerByName(pMap, Anno_featClass.AliasName);

                    if (_Layer == null || !_Layer.Visible) continue;

                    /////////////
                    IFeatureSelection pSelect = (IFeatureSelection)_Layer;
                    if (pSelect != null) pSelect.Clear();

                    ////////////
                }

                if (FeatureTreeView.SelectedNode == null || FeatureTreeView.SelectedNode.Tag == null) return;

                SigmaDataCash.environmentWorkspaceEdit.StartEditing(false);
                SigmaDataCash.environmentWorkspaceEdit.StartEditOperation();

                if (!IsPrototypeElement(cartoEl))
                {
                    UpdateCoordStyle((ChartElement_SimpleText)cartoEl, crdtp);

                    updateToolStripMenuItem_Click(sender, e);
                }
                else
                {
                    if (FeatureTreeView.SelectedNode.Nodes.Count > 0)
                    {
                        UpdateCoordStyle((ChartElement_SimpleText)cartoEl, crdtp);

                        foreach (TreeNode item in FeatureTreeView.SelectedNode.Nodes)
                        {
                            UpdateCoordStyle((ChartElement_SimpleText)item.Tag, crdtp);
                            UpdateElement(item);
                        }
                    }
                }

                SigmaDataCash.environmentWorkspaceEdit.StopEditOperation();
                SigmaDataCash.environmentWorkspaceEdit.StopEditing(true);
                //(pMap as IActiveView).Refresh();
                ChartElementsManipulator.RefreshChart((IMxDocument)m_application.Document);
            }

        }

        private void nDDMMSSToolStripMenuItem_Click(object sender, EventArgs e)
        {

            AbstractChartElement cartoEl = (AbstractChartElement)FeatureTreeView.SelectedNode.Tag;
            if (cartoEl is ChartElement_SimpleText)
            {
                coordtype crdtp = (coordtype)int.Parse(((ToolStripMenuItem)sender).Tag.ToString());

                foreach (KeyValuePair<string, object> item in SigmaDataCash.AnnotationFeatureClassList)
                {
                    if (!(item.Value is IFeatureClass)) continue;

                    IFeatureClass Anno_featClass = (IFeatureClass)item.Value;
                    ILayer _Layer = EsriUtils.getLayerByName(pMap, Anno_featClass.AliasName);

                    if (_Layer == null || !_Layer.Visible) continue;

                    /////////////
                    IFeatureSelection pSelect = (IFeatureSelection)_Layer;
                    if (pSelect != null) pSelect.Clear();

                    ////////////
                }

                if (FeatureTreeView.SelectedNode == null || FeatureTreeView.SelectedNode.Tag == null) return;

                SigmaDataCash.environmentWorkspaceEdit.StartEditing(false);
                SigmaDataCash.environmentWorkspaceEdit.StartEditOperation();

                if (!IsPrototypeElement(cartoEl))
                {
                    UpdateCoordStyle((ChartElement_SimpleText)cartoEl, crdtp);
                    updateToolStripMenuItem_Click(sender, e);

                    for (int i = 0; i < SigmaDataCash.SelectedChartElements.Count; i++)
                    {
                        var item = SigmaDataCash.SelectedChartElements[i];
                        if (item.Id.ToString().StartsWith(cartoEl.Id.ToString())) continue;

                        UpdateCoordStyle((ChartElement_SimpleText)item, crdtp);
                        IElement el = item.ConvertToIElement() as IElement;
                        ChartElementsManipulator.UpdateSingleElementToDataSet(item.Name, item.Id.ToString(), el, ref item);
                    }

                }
                else
                {
                    if (FeatureTreeView.SelectedNode.Nodes.Count > 0)
                    {
                        UpdateCoordStyle((ChartElement_SimpleText)cartoEl, crdtp);

                        foreach (TreeNode item in FeatureTreeView.SelectedNode.Nodes)
                        {
                            UpdateCoordStyle((ChartElement_SimpleText)item.Tag, crdtp);
                            UpdateElement(item);
                        }
                    }
                }

                SigmaDataCash.environmentWorkspaceEdit.StopEditOperation();
                SigmaDataCash.environmentWorkspaceEdit.StopEditing(true);
                //(pMap as IActiveView).Refresh();
                ChartElementsManipulator.RefreshChart((IMxDocument)m_application.Document);
            }

        }

        private void UpdateCoordStyle(ChartElement_SimpleText chartElement_SimpleText, coordtype crdtp)
        {

            AncorPoint pntAncr = chartElement_SimpleText.Anchor;
            chartElement_SimpleText.CoordType = crdtp;

            foreach (var Ln in chartElement_SimpleText.TextContents)
            {
                foreach (var wrd in Ln)
                {
                    if (wrd.DataSource.Value.StartsWith("Lat"))
                    {
                        wrd.TextValue = ArenaStaticProc.LatToDDMMSS(pntAncr.Y.ToString(), crdtp);
                    }
                    if (wrd.DataSource.Value.StartsWith("Lon"))
                    {
                        wrd.TextValue = ArenaStaticProc.LonToDDMMSS(pntAncr.X.ToString(), crdtp);
                    }
                }
            }
        }

        private void propertyGrid1_SelectedObjectsChanged(object sender, EventArgs e)
        {
            //System.Diagnostics.Debug.WriteLine(sender.ToString());
        }

        private void propertyGrid1_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            FontChangedFlag = e.ChangedItem.Parent != null
                            && e.ChangedItem.Parent.PropertyDescriptor != null
                            && e.ChangedItem.Parent.PropertyDescriptor.Name.StartsWith("Font");
        }

        private void applyStyleToAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (FeatureTreeView.SelectedNode.Tag == null) return;
            if (FeatureTreeView.SelectedNode.Parent != null && FeatureTreeView.SelectedNode.Parent.Tag == null) return;


            AbstractChartElement cartoEl = (AbstractChartElement)FeatureTreeView.SelectedNode.Tag;
            TreeNode selNd = FeatureTreeView.SelectedNode;

            if (!cartoEl.RelatedFeature.StartsWith("MarkerNote_"))
            {
                ApplayElementStyle(cartoEl);
                FeatureTreeView.SelectedNode = FeatureTreeView.SelectedNode.Parent;
                linkLabel2_LinkClicked(sender, new LinkLabelLinkClickedEventArgs(linkLabel2.Links[0], System.Windows.Forms.MouseButtons.Left));

            }
            else
            {
                ApplyNoteStyle(cartoEl);
                //(pMap as IActiveView).Refresh();
                ChartElementsManipulator.RefreshChart((IMxDocument)m_application.Document);
                (pGraphicsContainer as IActiveView).Refresh();

            }

            FeatureTreeView.SelectedNode = selNd;
        }

        private void ApplyNoteStyle(AbstractChartElement cartoEl)
        {
            Type tt = cartoEl.GetType();
            PropertyDescriptorCollection pdc = TypeDescriptor.GetProperties(cartoEl);
            // to do change Fonts prop
            //SetTheSameFontsToTextContents(cartoEl);

            SigmaDataCash.SelectedChartElements.Clear();
            var notes = (from element in SigmaDataCash.ChartElementList
                         where (element != null) && (element is ChartElement_MarkerSymbol)
                             && (!((ChartElement_MarkerSymbol)element).Id.ToString().StartsWith(cartoEl.Id.ToString()))
                             && (((ChartElement_MarkerSymbol)element).RelatedFeature.StartsWith("MarkerNote_"))
                         select element).ToList();

            /// Update selected elements
            ///
            SigmaDataCash.environmentWorkspaceEdit.StartEditing(false);
            SigmaDataCash.environmentWorkspaceEdit.StartEditOperation();

            tt = cartoEl.GetType();
            pdc = TypeDescriptor.GetProperties(cartoEl);
            // to do change Fonts prop
            //if (FontChangedFlag) SetTheSameFontsToTextContents(cartoEl);


            foreach (AbstractChartElement item in notes)
            {
                AbstractChartElement childEl = item;

 
                foreach (PropertyDescriptor pd in pdc)
                {
                    Attribute attributeBrowsable = pd.Attributes[typeof(BrowsableAttribute)];
                    Attribute attributeSkip = pd.Attributes[typeof(SkipAttribute)];
                    if (((BrowsableAttribute)attributeBrowsable).Browsable && !((SkipAttribute)attributeSkip).SkipFlag)
                    {
                        object ObjpropVal = ArenaStaticProc.GetObjectValue(cartoEl, pd.Name);
                        if (ObjpropVal != null)
                        {
                            if (ObjpropVal is AbstractChartClass) ObjpropVal = ((AbstractChartClass)ObjpropVal).Clone();
                        }
                        ArenaStaticProc.SetObjectValue(childEl, pd.Name, ObjpropVal);

                    }

                }

                IElement el = childEl.ConvertToIElement() as IElement;
                ChartElementsManipulator.UpdateSingleElementToDataSet(childEl.Name, childEl.Id.ToString(), el, ref childEl);

                UpdateExistingNote(childEl);
            }

            SigmaDataCash.environmentWorkspaceEdit.StopEditOperation();
            SigmaDataCash.environmentWorkspaceEdit.StopEditing(true);


            #region сохранить на диске новые шаблоны прототипов

            SigmaDataCash.prototype_anno_lst[10] = cartoEl;


            string FN = System.IO.Path.Combine(ArenaStaticProc.GetPathToTemplate() + @"\Enroute\", "enroute.sce");
            string chrtTp = "enrt";
            if (SigmaDataCash.SigmaChartType == 2)
            {
                FN = System.IO.Path.Combine(ArenaStaticProc.GetPathToTemplate() + @"\SID\", "sid.sce");
                chrtTp = "sid";
            }
            if (SigmaDataCash.SigmaChartType == 4)
            {
                FN = System.IO.Path.Combine(ArenaStaticProc.GetPathToTemplate() + @"\STAR\", "star.sce");
                chrtTp = "star";
            }
            if (SigmaDataCash.SigmaChartType == 5)
            {
                FN = System.IO.Path.Combine(ArenaStaticProc.GetPathToTemplate() + @"\IAP\", "iap.sce");
                chrtTp = "iap";
            }
            if (SigmaDataCash.SigmaChartType == 6)
            {
                FN = System.IO.Path.Combine(ArenaStaticProc.GetPathToTemplate() + @"\PATC\", "patc.sce");
                chrtTp = "patc";
            }
            if (SigmaDataCash.SigmaChartType == 7)
            {
                FN = System.IO.Path.Combine(ArenaStaticProc.GetPathToTemplate() + @"\AreaChart\", "areachart.sce");
                chrtTp = "area";
            }
            if (SigmaDataCash.SigmaChartType == 8 || SigmaDataCash.SigmaChartType == 9 || SigmaDataCash.SigmaChartType == 10 || SigmaDataCash.SigmaChartType == 11 || SigmaDataCash.SigmaChartType == 12)
            {
                FN = System.IO.Path.Combine(ArenaStaticProc.GetPathToTemplate() + @"\AerodromeElectronicChart\", "aerodrome.sce");
                chrtTp = "aerd";
            }
            if (SigmaDataCash.SigmaChartType == 13)
            {
                FN = System.IO.Path.Combine(ArenaStaticProc.GetPathToTemplate() + @"\MinimumAltitudeChart\", "minalt.sce");
                chrtTp = "minAlt";
            }


            Chart_ObjectsList obj = new Chart_ObjectsList { ChartType = chrtTp, List = SigmaDataCash.prototype_anno_lst };


            //Object obj = acntElement;

            XmlSerializer xmlSer = new XmlSerializer(obj.GetType());
            MemoryStream strmMemSer = new MemoryStream();
            xmlSer.Serialize(strmMemSer, obj);



            byte[] byteArr = new byte[strmMemSer.Length];
            strmMemSer.Position = 0;
            int count = strmMemSer.Read(byteArr, 0, byteArr.Length);
            if (count != byteArr.Length)
            {
                strmMemSer.Close();
                Console.WriteLine("Test Failed: Unable to read data from file");
                return;
            }


            if (File.Exists(FN)) File.Delete(FN);
            System.IO.FileStream strmFileCompressed = new System.IO.FileStream(FN, FileMode.CreateNew);
            strmMemSer.WriteTo(strmFileCompressed);
            strmMemSer.Close();
            strmFileCompressed.Close();
            #endregion



        }


        private void ApplayElementStyle(AbstractChartElement cartoEl)
        {
            if (FeatureTreeView.SelectedNode.Parent == null || FeatureTreeView.SelectedNode.Parent.Tag == null) return;


            Type tt = cartoEl.GetType();
            PropertyDescriptorCollection pdc = TypeDescriptor.GetProperties(cartoEl);
            // to do change Fonts prop
            SetTheSameFontsToTextContents(cartoEl);


            AbstractChartElement parentEl = (AbstractChartElement)FeatureTreeView.SelectedNode.Parent.Tag;

            // to do change Fonts prop
            SetTheSameFontsToTextContents(cartoEl, parentEl);
            FontChangedFlag = true;
            string morseTxt = "";
            string arspstxt = "";
            int _chrInx = 0;
            int _InnerInx = 0;

            if (cartoEl is ChartElement_SigmaCollout && parentEl is ChartElement_SigmaCollout && ((ChartElement_SigmaCollout)parentEl).MorseTextLine != null)
                morseTxt = ((ChartElement_SigmaCollout)parentEl).MorseTextLine.MorseText;
            if (cartoEl is ChartElement_SigmaCollout_Airspace && parentEl is ChartElement_SigmaCollout_Airspace && ((ChartElement_SigmaCollout_Airspace)parentEl).AirspaceSign != null)
                arspstxt = ((ChartElement_SigmaCollout_Airspace)parentEl).AirspaceSign.AirspaceSymbols;
            if (cartoEl is ChartElement_MarkerSymbol && parentEl is ChartElement_MarkerSymbol && ((ChartElement_MarkerSymbol)parentEl).MarkerBackGround != null)
            {
                _chrInx = ((ChartElement_MarkerSymbol)cartoEl).MarkerBackGround.CharacterIndex;
                _InnerInx = ((ChartElement_MarkerSymbol)cartoEl).MarkerBackGround.InnerCharacterIndex;
            }


            foreach (PropertyDescriptor pd in pdc)
            {
                Attribute attributeBrowsable = pd.Attributes[typeof(BrowsableAttribute)];
                Attribute attributeSkip = pd.Attributes[typeof(SkipAttribute)];

                if (((BrowsableAttribute)attributeBrowsable).Browsable && attributeSkip != null && !((SkipAttribute)attributeSkip).SkipFlag)
                {
                    object ObjpropVal = ArenaStaticProc.GetObjectValue(cartoEl, pd.Name);
                    if (ObjpropVal != null)
                    {
                        if (ObjpropVal is AbstractChartClass) ObjpropVal = ((AbstractChartClass)ObjpropVal).Clone();

                    }
                    ArenaStaticProc.SetObjectValue(parentEl, pd.Name, ObjpropVal);

                }

            }

            if (cartoEl is ChartElement_SigmaCollout && parentEl is ChartElement_SigmaCollout && ((ChartElement_SigmaCollout)cartoEl).MorseTextLine != null)
                ((ChartElement_SigmaCollout)parentEl).MorseTextLine.MorseText = morseTxt;
            if (cartoEl is ChartElement_SigmaCollout_Airspace && parentEl is ChartElement_SigmaCollout_Airspace && ((ChartElement_SigmaCollout_Airspace)parentEl).AirspaceSign != null)
                ((ChartElement_SigmaCollout_Airspace)parentEl).AirspaceSign.AirspaceSymbols = arspstxt;

            if (cartoEl is ChartElement_MarkerSymbol && parentEl is ChartElement_MarkerSymbol && ((ChartElement_MarkerSymbol)parentEl).MarkerBackGround != null)
            {
                ((ChartElement_MarkerSymbol)cartoEl).MarkerBackGround.InnerCharacterIndex = _InnerInx;
                ((ChartElement_MarkerSymbol)cartoEl).MarkerBackGround.CharacterIndex = _chrInx;
            }


            if (cartoEl is ChartElement_SimpleText && parentEl is ChartElement_SimpleText)
            {
                ((ChartElement_SimpleText)parentEl).CoordType = ((ChartElement_SimpleText)cartoEl).CoordType;
            }

            ////////////////////////


        }

        private void showOnMapToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (FeatureTreeView.SelectedNode == null) return;
            if (FeatureTreeView.SelectedNode.Tag == null) return;
            if (FeatureTreeView.SelectedNode.Parent == null) return;
            if (FeatureTreeView.SelectedNode.Parent.Tag == null) return;

            AbstractChartElement chartEl = ((AbstractChartElement)FeatureTreeView.SelectedNode.Tag);
            if (IsPrototypeElement(chartEl)) return;

            IGeometry gm = showOnMap(chartEl);
            if (gm == null || gm.IsEmpty) return;
            ISpatialReference spRef = SepMapCenter(gm, chartEl.Name);
            //ExpandGeometry(pMap, gm, spRef);


            ChartElementsManipulator.RefreshChart((IMxDocument)m_application.Document);
            Application.DoEvents();

            ESRI.ArcGIS.Display.IDisplay pDisplay = (pMap as IActiveView).ScreenDisplay;
            gm = EsriUtils.ToProject(gm, (pMap as IActiveView).FocusMap, spRef);

            if (!gm.IsEmpty)
                FlashGeometry(gm, pDisplay, (pMap as IActiveView).Extent, 300);


            //UAE_Update("AirspaceAnno");
            //UAE_Update("RouteSegmentAnnoSign");
            //UAE_Update_Track("RouteSegmentAnnoMagTrack");
            //UAE_Update_Track("RouteSegmentAnnoReverseMagTrack");
            //UAE_Update("RouteSegmentAnnoLimits");


            //var lll = (from element in SigmaDataCash.ChartElementList where (element != null) && (element is ChartElement_SimpleText) && (element as ChartElement_SimpleText).Name.StartsWith("RouteSegment_UpperLowerLimit")  select element).ToList();

            // foreach (var item in lll)
            // {
            //     UAE_UpdateMirror(((ChartElement_SimpleText)item).Id.ToString(), "ChartElement_SimpleText", ((ChartElement_SimpleText)item).LinckedGeoId);
            // }

            // MessageBox.Show(lll.Count.ToString());
        }

        public static int UAE_UpdateMirror(string chartElID, string ObjType, string geoId)
        {
            IFeatureCursor featCur = null;
            ChartElement_SimpleText chartEl = null;

            try
            {

                IFeatureClass featureClass = (IFeatureClass)SigmaDataCash.AnnotationFeatureClassList["Mirror"];


                IQueryFilter featFilter = new QueryFilterClass();

                featFilter.WhereClause = "AncorUID= '" + chartElID.ToString() + "'";

                featCur = featureClass.Update(featFilter, false);

                IFeature _Feature = featCur.NextFeature();

                if (_Feature != null)
                {

                    int fID1 = _Feature.Fields.FindField("OBJ");

                    //DeserializeObject
                    switch (ObjType) //!
                    {
                        case ("ChartElement_SimpleText"):
                            chartEl = ChartElementsManipulator.DeserializeObject<ChartElement_SimpleText>(_Feature.get_Value(fID1).ToString()) as ChartElement_SimpleText;
                            break;
                        case ("ChartElement_RouteDesignator"):
                            chartEl = ChartElementsManipulator.DeserializeObject<ChartElement_RouteDesignator>(_Feature.get_Value(fID1).ToString()) as ChartElement_SimpleText;
                            break;
                        case ("ChartElement_BorderedText"):
                            chartEl = ChartElementsManipulator.DeserializeObject<ChartElement_BorderedText>(_Feature.get_Value(fID1).ToString()) as ChartElement_SimpleText;
                            break;
                        case ("ChartElement_BorderedText_Collout_CaptionBottom"):
                            chartEl = ChartElementsManipulator.DeserializeObject<ChartElement_BorderedText_Collout_CaptionBottom>(_Feature.get_Value(fID1).ToString()) as ChartElement_SimpleText;
                            break;
                        case ("ChartElement_BorderedText_Collout"):
                            chartEl = ChartElementsManipulator.DeserializeObject<ChartElement_BorderedText_Collout>(_Feature.get_Value(fID1).ToString()) as ChartElement_SimpleText;
                            break;
                        case ("ChartElement_SigmaCollout_Navaid"):
                            chartEl = ChartElementsManipulator.DeserializeObject<ChartElement_SigmaCollout_Navaid>(_Feature.get_Value(fID1).ToString()) as ChartElement_SimpleText;
                            break;
                        case ("ChartElement_MarkerSymbol"):
                            chartEl = ChartElementsManipulator.DeserializeObject<ChartElement_MarkerSymbol>(_Feature.get_Value(fID1).ToString()) as ChartElement_SimpleText;
                            break;
                        case ("ChartElement_TextArrow"):
                            chartEl = ChartElementsManipulator.DeserializeObject<ChartElement_TextArrow>(_Feature.get_Value(fID1).ToString()) as ChartElement_SimpleText;
                            break;
                        case ("ChartElement_Radial"):
                            chartEl = ChartElementsManipulator.DeserializeObject<ChartElement_Radial>(_Feature.get_Value(fID1).ToString()) as ChartElement_SimpleText;
                            break;
                        case ("ChartElement_SigmaCollout_Designatedpoint"):
                            chartEl = ChartElementsManipulator.DeserializeObject<ChartElement_SigmaCollout_Designatedpoint>(_Feature.get_Value(fID1).ToString()) as ChartElement_SimpleText;
                            break;
                        case ("ChartElement_SigmaCollout_Airspace"):
                            chartEl = ChartElementsManipulator.DeserializeObject<ChartElement_SigmaCollout_Airspace>(_Feature.get_Value(fID1).ToString()) as ChartElement_SimpleText;
                            break;
                        case ("ChartElement_SigmaCollout_AccentBar"):
                            chartEl = ChartElementsManipulator.DeserializeObject<ChartElement_SigmaCollout_AccentBar>(_Feature.get_Value(fID1).ToString()) as ChartElement_SimpleText;
                            break;
                        case ("ChartElement_ILSCollout"):
                            chartEl = ChartElementsManipulator.DeserializeObject<ChartElement_ILSCollout>(_Feature.get_Value(fID1).ToString()) as ChartElement_SimpleText;
                            break;
                        default:
                            break;
                    }

                    chartEl.LinckedGeoId = geoId;
                    string objSer = "";
                    //SerializeObject
                    switch (ObjType)
                    {
                        case ("ChartElement_SimpleText"):
                            objSer = ChartElementsManipulator.SerializeObject(chartEl as ChartElement_SimpleText);
                            break;
                        case ("ChartElement_RouteDesignator"):
                            objSer = ChartElementsManipulator.SerializeObject(chartEl as ChartElement_RouteDesignator);
                            break;
                        case ("ChartElement_BorderedText"):
                            objSer = ChartElementsManipulator.SerializeObject(chartEl as ChartElement_BorderedText);
                            break;
                        case ("ChartElement_BorderedText_Collout_CaptionBottom"):
                            objSer = ChartElementsManipulator.SerializeObject(chartEl as ChartElement_BorderedText_Collout_CaptionBottom);
                            break;
                        case ("ChartElement_BorderedText_Collout"):
                            objSer = ChartElementsManipulator.SerializeObject(chartEl as ChartElement_BorderedText_Collout);
                            break;
                        case ("ChartElement_SigmaCollout_Navaid"):
                            objSer = ChartElementsManipulator.SerializeObject(chartEl as ChartElement_SigmaCollout_Navaid);
                            break;
                        case ("ChartElement_MarkerSymbol"):
                            objSer = ChartElementsManipulator.SerializeObject(chartEl as ChartElement_MarkerSymbol);
                            break;
                        case ("ChartElement_TextArrow"):
                            objSer = ChartElementsManipulator.SerializeObject(chartEl as ChartElement_TextArrow);
                            break;
                        case ("ChartElement_Radial"):
                            objSer = ChartElementsManipulator.SerializeObject(chartEl as ChartElement_Radial);
                            break;
                        case ("ChartElement_SigmaCollout_Designatedpoint"):
                            objSer = ChartElementsManipulator.SerializeObject(chartEl as ChartElement_SigmaCollout_Designatedpoint);
                            break;
                        case ("ChartElement_SigmaCollout_Airspace"):
                            objSer = ChartElementsManipulator.SerializeObject(chartEl as ChartElement_SigmaCollout_Airspace);
                            break;
                        case ("ChartElement_SigmaCollout_AccentBar"):
                            objSer = ChartElementsManipulator.SerializeObject(chartEl as ChartElement_SigmaCollout_AccentBar);
                            break;

                        default:
                            break;
                    }

                    _Feature.set_Value(fID1, objSer);

                    _Feature.Store();

                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            finally
            {
                Marshal.ReleaseComObject(featCur);




            }


            return 1;
        }


        private void UAE_Update(string FCN)
        {
            IFeatureClass Anno_featClass = (IFeatureClass)SigmaDataCash.AnnotationFeatureClassList[FCN];
            ChartElement_SimpleText chartEl = null;

            IQueryFilter featFilter = new QueryFilterClass();

            featFilter.WhereClause = "OBJECTID >0";

            IFeatureCursor featCur = Anno_featClass.Update(featFilter, false);

            IFeature _Feature = null;
            while ((_Feature = featCur.NextFeature()) != null)
            {

                int fID1 = _Feature.Fields.FindField("OBJ");
                int fID2 = _Feature.Fields.FindField("ObjectType");
                int fID3 = _Feature.Fields.FindField("PdmUID");
                string objSer = "";

                if (fID1 >= 0 && fID2 >= 0)
                {
                    string ObjType = _Feature.get_Value(fID2).ToString();

                    //DeserializeObject
                    switch (ObjType) //!
                    {
                        case ("ChartElement_SimpleText"):
                            chartEl = ChartElementsManipulator.DeserializeObject<ChartElement_SimpleText>(_Feature.get_Value(fID1).ToString()) as ChartElement_SimpleText;
                            break;
                        case ("ChartElement_RouteDesignator"):
                            chartEl = ChartElementsManipulator.DeserializeObject<ChartElement_RouteDesignator>(_Feature.get_Value(fID1).ToString()) as ChartElement_SimpleText;
                            break;
                        case ("ChartElement_BorderedText"):
                            chartEl = ChartElementsManipulator.DeserializeObject<ChartElement_BorderedText>(_Feature.get_Value(fID1).ToString()) as ChartElement_SimpleText;
                            break;
                        case ("ChartElement_BorderedText_Collout_CaptionBottom"):
                            chartEl = ChartElementsManipulator.DeserializeObject<ChartElement_BorderedText_Collout_CaptionBottom>(_Feature.get_Value(fID1).ToString()) as ChartElement_SimpleText;
                            break;
                        case ("ChartElement_BorderedText_Collout"):
                            chartEl = ChartElementsManipulator.DeserializeObject<ChartElement_BorderedText_Collout>(_Feature.get_Value(fID1).ToString()) as ChartElement_SimpleText;
                            break;
                        case ("ChartElement_SigmaCollout_Navaid"):
                            chartEl = ChartElementsManipulator.DeserializeObject<ChartElement_SigmaCollout_Navaid>(_Feature.get_Value(fID1).ToString()) as ChartElement_SimpleText;
                            break;
                        case ("ChartElement_MarkerSymbol"):
                            chartEl = ChartElementsManipulator.DeserializeObject<ChartElement_MarkerSymbol>(_Feature.get_Value(fID1).ToString()) as ChartElement_SimpleText;
                            break;
                        case ("ChartElement_TextArrow"):
                            chartEl = ChartElementsManipulator.DeserializeObject<ChartElement_TextArrow>(_Feature.get_Value(fID1).ToString()) as ChartElement_SimpleText;
                            break;
                        case ("ChartElement_Radial"):
                            chartEl = ChartElementsManipulator.DeserializeObject<ChartElement_Radial>(_Feature.get_Value(fID1).ToString()) as ChartElement_SimpleText;
                            break;
                        case ("ChartElement_SigmaCollout_Designatedpoint"):
                            chartEl = ChartElementsManipulator.DeserializeObject<ChartElement_SigmaCollout_Designatedpoint>(_Feature.get_Value(fID1).ToString()) as ChartElement_SimpleText;
                            break;
                        case ("ChartElement_SigmaCollout_Airspace"):
                            chartEl = ChartElementsManipulator.DeserializeObject<ChartElement_SigmaCollout_Airspace>(_Feature.get_Value(fID1).ToString()) as ChartElement_SimpleText;
                            break;
                        case ("ChartElement_SigmaCollout_AccentBar"):
                            chartEl = ChartElementsManipulator.DeserializeObject<ChartElement_SigmaCollout_AccentBar>(_Feature.get_Value(fID1).ToString()) as ChartElement_SimpleText;
                            break;
                        case ("ChartElement_ILSCollout"):
                            chartEl = ChartElementsManipulator.DeserializeObject<ChartElement_ILSCollout>(_Feature.get_Value(fID1).ToString()) as ChartElement_SimpleText;
                            break;
                        default:
                            break;
                    }

                    chartEl.LinckedGeoId = _Feature.get_Value(fID3).ToString();

                    //SerializeObject
                    switch (ObjType)
                    {
                        case ("ChartElement_SimpleText"):
                            objSer = ChartElementsManipulator.SerializeObject(chartEl as ChartElement_SimpleText);
                            break;
                        case ("ChartElement_RouteDesignator"):
                            objSer = ChartElementsManipulator.SerializeObject(chartEl as ChartElement_RouteDesignator);
                            break;
                        case ("ChartElement_BorderedText"):
                            objSer = ChartElementsManipulator.SerializeObject(chartEl as ChartElement_BorderedText);
                            break;
                        case ("ChartElement_BorderedText_Collout_CaptionBottom"):
                            objSer = ChartElementsManipulator.SerializeObject(chartEl as ChartElement_BorderedText_Collout_CaptionBottom);
                            break;
                        case ("ChartElement_BorderedText_Collout"):
                            objSer = ChartElementsManipulator.SerializeObject(chartEl as ChartElement_BorderedText_Collout);
                            break;
                        case ("ChartElement_SigmaCollout_Navaid"):
                            objSer = ChartElementsManipulator.SerializeObject(chartEl as ChartElement_SigmaCollout_Navaid);
                            break;
                        case ("ChartElement_MarkerSymbol"):
                            objSer = ChartElementsManipulator.SerializeObject(chartEl as ChartElement_MarkerSymbol);
                            break;
                        case ("ChartElement_TextArrow"):
                            objSer = ChartElementsManipulator.SerializeObject(chartEl as ChartElement_TextArrow);
                            break;
                        case ("ChartElement_Radial"):
                            objSer = ChartElementsManipulator.SerializeObject(chartEl as ChartElement_Radial);
                            break;
                        case ("ChartElement_SigmaCollout_Designatedpoint"):
                            objSer = ChartElementsManipulator.SerializeObject(chartEl as ChartElement_SigmaCollout_Designatedpoint);
                            break;
                        case ("ChartElement_SigmaCollout_Airspace"):
                            objSer = ChartElementsManipulator.SerializeObject(chartEl as ChartElement_SigmaCollout_Airspace);
                            break;
                        case ("ChartElement_SigmaCollout_AccentBar"):
                            objSer = ChartElementsManipulator.SerializeObject(chartEl as ChartElement_SigmaCollout_AccentBar);
                            break;

                        default:
                            break;
                    }

                    _Feature.set_Value(fID1, objSer);

                    _Feature.Store();

                    //ChartElementsManipulator.UpdateMirror(mapElem, pdmElementID, objSer);

                }
            }

            Marshal.ReleaseComObject(featCur);
        }

        private void UAE_Update_Track(string FCN)
        {
            IFeatureClass Anno_featClass = (IFeatureClass)SigmaDataCash.AnnotationFeatureClassList[FCN];
            ChartElement_SimpleText chartEl = null;

            IQueryFilter featFilter = new QueryFilterClass();

            //featFilter.WhereClause = "OBJECTID >0 AND PdmUID = '127ec310-7510-449c-8d98-64d4883ab661'";
            featFilter.WhereClause = "OBJECTID >0";

            IFeatureCursor featCur = Anno_featClass.Update(featFilter, false);

            IFeature _Feature = null;
            while ((_Feature = featCur.NextFeature()) != null)
            {

                int fID1 = _Feature.Fields.FindField("OBJ");
                int fID2 = _Feature.Fields.FindField("ObjectType");
                int fID3 = _Feature.Fields.FindField("PdmUID");
                int fID4 = _Feature.Fields.FindField("PdmUID_OLD");
                string objSer = "";

                if (fID1 >= 0 && fID2 >= 0)
                {
                    string ObjType = _Feature.get_Value(fID2).ToString();

                    //DeserializeObject
                    switch (ObjType) //!
                    {
                        case ("ChartElement_SimpleText"):
                            chartEl = ChartElementsManipulator.DeserializeObject<ChartElement_SimpleText>(_Feature.get_Value(fID1).ToString()) as ChartElement_SimpleText;
                            break;
                        case ("ChartElement_RouteDesignator"):
                            chartEl = ChartElementsManipulator.DeserializeObject<ChartElement_RouteDesignator>(_Feature.get_Value(fID1).ToString()) as ChartElement_SimpleText;
                            break;
                        case ("ChartElement_BorderedText"):
                            chartEl = ChartElementsManipulator.DeserializeObject<ChartElement_BorderedText>(_Feature.get_Value(fID1).ToString()) as ChartElement_SimpleText;
                            break;
                        case ("ChartElement_BorderedText_Collout_CaptionBottom"):
                            chartEl = ChartElementsManipulator.DeserializeObject<ChartElement_BorderedText_Collout_CaptionBottom>(_Feature.get_Value(fID1).ToString()) as ChartElement_SimpleText;
                            break;
                        case ("ChartElement_BorderedText_Collout"):
                            chartEl = ChartElementsManipulator.DeserializeObject<ChartElement_BorderedText_Collout>(_Feature.get_Value(fID1).ToString()) as ChartElement_SimpleText;
                            break;
                        case ("ChartElement_SigmaCollout_Navaid"):
                            chartEl = ChartElementsManipulator.DeserializeObject<ChartElement_SigmaCollout_Navaid>(_Feature.get_Value(fID1).ToString()) as ChartElement_SimpleText;
                            break;
                        case ("ChartElement_MarkerSymbol"):
                            chartEl = ChartElementsManipulator.DeserializeObject<ChartElement_MarkerSymbol>(_Feature.get_Value(fID1).ToString()) as ChartElement_SimpleText;
                            break;
                        case ("ChartElement_TextArrow"):
                            chartEl = ChartElementsManipulator.DeserializeObject<ChartElement_TextArrow>(_Feature.get_Value(fID1).ToString()) as ChartElement_SimpleText;
                            break;
                        case ("ChartElement_Radial"):
                            chartEl = ChartElementsManipulator.DeserializeObject<ChartElement_Radial>(_Feature.get_Value(fID1).ToString()) as ChartElement_SimpleText;
                            break;
                        case ("ChartElement_SigmaCollout_Designatedpoint"):
                            chartEl = ChartElementsManipulator.DeserializeObject<ChartElement_SigmaCollout_Designatedpoint>(_Feature.get_Value(fID1).ToString()) as ChartElement_SimpleText;
                            break;
                        case ("ChartElement_SigmaCollout_Airspace"):
                            chartEl = ChartElementsManipulator.DeserializeObject<ChartElement_SigmaCollout_Airspace>(_Feature.get_Value(fID1).ToString()) as ChartElement_SimpleText;
                            break;
                        case ("ChartElement_SigmaCollout_AccentBar"):
                            chartEl = ChartElementsManipulator.DeserializeObject<ChartElement_SigmaCollout_AccentBar>(_Feature.get_Value(fID1).ToString()) as ChartElement_SimpleText;
                            break;
                        case ("ChartElement_ILSCollout"):
                            chartEl = ChartElementsManipulator.DeserializeObject<ChartElement_ILSCollout>(_Feature.get_Value(fID1).ToString()) as ChartElement_SimpleText;
                            break;
                        default:
                            break;
                    }

                    chartEl.LinckedGeoId = UAE_Update_getGeoID( _Feature.get_Value(fID4).ToString());

                    //SerializeObject
                    switch (ObjType)
                    {
                        case ("ChartElement_SimpleText"):
                            objSer = ChartElementsManipulator.SerializeObject(chartEl as ChartElement_SimpleText);
                            break;
                        case ("ChartElement_RouteDesignator"):
                            objSer = ChartElementsManipulator.SerializeObject(chartEl as ChartElement_RouteDesignator);
                            break;
                        case ("ChartElement_BorderedText"):
                            objSer = ChartElementsManipulator.SerializeObject(chartEl as ChartElement_BorderedText);
                            break;
                        case ("ChartElement_BorderedText_Collout_CaptionBottom"):
                            objSer = ChartElementsManipulator.SerializeObject(chartEl as ChartElement_BorderedText_Collout_CaptionBottom);
                            break;
                        case ("ChartElement_BorderedText_Collout"):
                            objSer = ChartElementsManipulator.SerializeObject(chartEl as ChartElement_BorderedText_Collout);
                            break;
                        case ("ChartElement_SigmaCollout_Navaid"):
                            objSer = ChartElementsManipulator.SerializeObject(chartEl as ChartElement_SigmaCollout_Navaid);
                            break;
                        case ("ChartElement_MarkerSymbol"):
                            objSer = ChartElementsManipulator.SerializeObject(chartEl as ChartElement_MarkerSymbol);
                            break;
                        case ("ChartElement_TextArrow"):
                            objSer = ChartElementsManipulator.SerializeObject(chartEl as ChartElement_TextArrow);
                            break;
                        case ("ChartElement_Radial"):
                            objSer = ChartElementsManipulator.SerializeObject(chartEl as ChartElement_Radial);
                            break;
                        case ("ChartElement_SigmaCollout_Designatedpoint"):
                            objSer = ChartElementsManipulator.SerializeObject(chartEl as ChartElement_SigmaCollout_Designatedpoint);
                            break;
                        case ("ChartElement_SigmaCollout_Airspace"):
                            objSer = ChartElementsManipulator.SerializeObject(chartEl as ChartElement_SigmaCollout_Airspace);
                            break;
                        case ("ChartElement_SigmaCollout_AccentBar"):
                            objSer = ChartElementsManipulator.SerializeObject(chartEl as ChartElement_SigmaCollout_AccentBar);
                            break;

                        default:
                            break;
                    }

                    _Feature.set_Value(fID1, objSer);
                    _Feature.set_Value(fID3, chartEl.LinckedGeoId);

                    _Feature.Store();

                    //ChartElementsManipulator.UpdateMirror(mapElem, pdmElementID, objSer);

                }
            }

            Marshal.ReleaseComObject(featCur);
        }


        private string UAE_Update_getGeoID(string PdmUID_OLD)
        {
            string res = "";

            IFeatureClass featureClass2 = SigmaDataCash.AnnotationLinkedGeometryList["RouteSegmentCartography"]; //SigmaDataCash.AnnotationLinkedGeometryList["AirspaceC"];

            IQueryFilter featFilter2 = new QueryFilterClass();

            featFilter2.WhereClause = "FeatureGUID_OLD= '" + PdmUID_OLD + "'";

            IFeatureCursor featCur2 = featureClass2.Update(featFilter2, false);

            IFeature feat2 = featCur2.NextFeature();

            if (feat2 != null)
            {


                int fID2 = feat2.Fields.FindField("FeatureGUID");

                res = feat2.Value[fID2].ToString();
                
                if (featCur2 != null) Marshal.ReleaseComObject(featCur2);

            }

            return res;


           
        }

        private void ExpandGeometry(IMap pMap, IGeometry pGeom, ISpatialReference pSpatialReference)
        {
            IEnvelope pEnv = new EnvelopeClass();
            //Set envelope to the geometry collection from the property below  
            pEnv = pGeom.Envelope;

            //Create a new envelope to pass the values to from the geometry   
            IEnvelope envelope = new EnvelopeClass();
            envelope.XMax = pEnv.Envelope.XMax;
            envelope.YMax = pEnv.Envelope.YMax;
            envelope.XMin = pEnv.Envelope.XMin;
            envelope.YMin = pEnv.Envelope.YMin;

            envelope = (IEnvelope)EsriUtils.ToProject(envelope, (pMap as IActiveView).FocusMap, pSpatialReference);


            int d = 10000;
            if (!envelope.IsEmpty)
            {
                envelope.Expand(d, d, false);

                (pMap as IActiveView).Extent = envelope;
            }
        }

        private void ActivateMainFrame()
        {
            for (int i = 0; i < ((IMxDocument)m_application.Document).Maps.Count; i++)
            {
                pMap = ((IMxDocument)m_application.Document).Maps.get_Item(i);
                pGraphicsContainer = ((IMxDocument)m_application.Document).ActiveView.GraphicsContainer;

                IFrameElement frameElement = pGraphicsContainer.FindFrame(pMap);
                if (frameElement != null)
                {
                    IElement el = (IElement)frameElement;
                    IElementProperties3 prop = (IElementProperties3)el;
                    if (prop.Name.StartsWith("Layers")) break;
                }
            }
        }

        private ISpatialReference  SepMapCenter(IGeometry mapCntr, string cartoEl_Name)
        {
            if (mapCntr.IsEmpty) return null;
            IEnvelope newEnv = (pMap as IActiveView).Extent;
            IPoint centrePoint = new PointClass();
            centrePoint.PutCoords((mapCntr as IArea).Centroid.X, (mapCntr as IArea).Centroid.Y);


            ILayer _Layer = EsriUtils.getLayerByName2(pMap, ChartElementsManipulator.DefinelayerName(cartoEl_Name));
            if (_Layer == null && (cartoEl_Name.StartsWith("HoldingPatternInboundCource") || cartoEl_Name.StartsWith("HoldingPatternOutboundCource")))
                _Layer = EsriUtils.getLayerByName2(pMap, "ProcedureLegsAnnoCourseCartography");
            if (_Layer == null && cartoEl_Name.StartsWith("Airport"))
                _Layer = EsriUtils.getLayerByName2(pMap, "AirportHeliportCartography");

            if (_Layer == null) return null;

            var fc = ((IFeatureLayer)_Layer).FeatureClass;
            _Layer.Visible = true;

            ISpatialReference pSpatialReference = (fc as IGeoDataset).SpatialReference;
            centrePoint = (IPoint)EsriUtils.ToProject(centrePoint, (pMap as IActiveView).FocusMap, pSpatialReference);

            if (!centrePoint.IsEmpty)
            {
                newEnv.CenterAt(centrePoint);//
                (pMap as IActiveView).Extent = newEnv;
            }
            return pSpatialReference;
        }

        private void sendToNoteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (FeatureTreeView.SelectedNode == null) return;

            AbstractChartElement SelectedElement = (AbstractChartElement)FeatureTreeView.SelectedNode.Tag;

            if (SelectedElement.RelatedFeature.StartsWith("MarkerNote_"))
            {
                RestoreFromNote(SelectedElement);
            }

            else
            {
                SendToNote(SelectedElement);
            }

            updateToolStripMenuItem_Click(sender, e);

        }

        private void SendToNote(AbstractChartElement SelectedElement, bool KeepName = false)
        {
            // создаем Note элемент


            ChartElement_MarkerSymbol NoteElement = (ChartElement_MarkerSymbol)ChartsHelperClass.getPrototypeChartElement(SigmaDataCash.prototype_anno_lst, "MarkerNote_Airspace");

            if (KeepName)
                NoteElement = (ChartElement_MarkerSymbol)(from element in SigmaDataCash.ChartElementList where (element != null) && (element is ChartElement_MarkerSymbol) && (((ChartElement_MarkerSymbol)element).Id.ToString().StartsWith(SelectedElement.Id.ToString())) select element).FirstOrDefault();
                

            NoteElement.Name = SelectedElement.Name;
            NoteElement.Id = SelectedElement.Id;
            NoteElement.RelatedFeature = "MarkerNote_" + SelectedElement.RelatedFeature;
            NoteElement.LinckedGeoId = SelectedElement.LinckedGeoId;

            string ObjType = SelectedElement.GetType().Name;
            NoteElement.TextContents[0][0].DataSource.Condition = ObjType;

            FeatureTreeView.SelectedNode.Text = FeatureTreeView.SelectedNode.Text + " '";

            if (!KeepName)
                NoteElement.TextContents[0][0].DataSource.Value = FeatureTreeView.SelectedNode.Text;

            string serObj = "";
            switch (ObjType)
            {
                case ("ChartElement_SimpleText"):
                    serObj = ChartElementsManipulator.SerializeObject(SelectedElement as ChartElement_SimpleText);
                    break;
                case ("ChartElement_RouteDesignator"):
                    serObj = ChartElementsManipulator.SerializeObject(SelectedElement as ChartElement_RouteDesignator);
                    break;
                case ("ChartElement_BorderedText"):
                    serObj = ChartElementsManipulator.SerializeObject(SelectedElement as ChartElement_BorderedText);
                    break;
                case ("ChartElement_BorderedText_Collout_CaptionBottom"):
                    serObj = ChartElementsManipulator.SerializeObject(SelectedElement as ChartElement_BorderedText_Collout_CaptionBottom);
                    break;
                case ("ChartElement_BorderedText_Collout"):
                    serObj = ChartElementsManipulator.SerializeObject(SelectedElement as ChartElement_BorderedText_Collout);
                    break;
                case ("ChartElement_SigmaCollout_Navaid"):
                    serObj = ChartElementsManipulator.SerializeObject(SelectedElement as ChartElement_SigmaCollout_Navaid);
                    break;
                case ("ChartElement_MarkerSymbol"):
                    serObj = ChartElementsManipulator.SerializeObject(SelectedElement as ChartElement_MarkerSymbol);
                    break;
                case ("ChartElement_TextArrow"):
                    serObj = ChartElementsManipulator.SerializeObject(SelectedElement as ChartElement_TextArrow);
                    break;
                case ("ChartElement_Radial"):
                    serObj = ChartElementsManipulator.SerializeObject(SelectedElement as ChartElement_Radial);
                    break;
                case ("ChartElement_SigmaCollout_Designatedpoint"):
                    serObj = ChartElementsManipulator.SerializeObject(SelectedElement as ChartElement_SigmaCollout_Designatedpoint);
                    break;
                case ("ChartElement_SigmaCollout_Airspace"):
                    serObj = ChartElementsManipulator.SerializeObject(SelectedElement as ChartElement_SigmaCollout_Airspace);
                    break;
                case ("ChartElement_SigmaCollout_AccentBar"):
                    serObj = ChartElementsManipulator.SerializeObject(SelectedElement as ChartElement_SigmaCollout_AccentBar);
                    break;
                default:
                    break;
            }

            NoteElement.Tag = serObj;

            NoteElement.TextContents[0][0].TextValue = "A";

            if (!KeepName)
            {
                FeatureTreeView.SelectedNode.Tag = NoteElement;

                TreeNode nd = FeatureTreeView.SelectedNode;
                FeatureTreeView.SelectedNode = FeatureTreeView.Nodes[0];
                FeatureTreeView.SelectedNode = nd;

            }



            //разместим на граф. контейнере оригинал элемента

            IElement originalElement = (IElement)SelectedElement.ConvertToIElement();
            IElement ntEl = (IElement)NoteElement.ConvertToIElement();

            IElementProperties3 docElementProperties = ntEl as IElementProperties3;
            docElementProperties.Name = "Note";

            docElementProperties = originalElement as IElementProperties3;
            docElementProperties.Name = "Object";


            IPoint pp = new PointClass();
            pp.PutCoords(1, 1);

            #region MyRegion
            //this.pGraphicsContainer.Reset();
            //IElementProperties docElementProperties2;
            //IElement dinamic_el = this.pGraphicsContainer.Next();
            //bool flag = false;
            //string elementname = "sigma_noteframe";

            //while (dinamic_el != null)
            //{
            //    docElementProperties2 = dinamic_el as IElementProperties;
            //    string curName = docElementProperties2.Name;
            //    if (curName.CompareTo(elementname) == 0)
            //    {
            //        flag = true;
            //        break;

            //    }
            //    dinamic_el = this.pGraphicsContainer.Next();

            //}

            //if (flag)
            //{
            //    pp.X = (((IPolygon)dinamic_el.Geometry) as IArea).Centroid.X;
            //    pp.Y = (((IPolygon)dinamic_el.Geometry) as IArea).Centroid.Y;
            //}
            #endregion




            IGroupElement3 GrpEl = new GroupElementClass();
            GrpEl.AddElement(originalElement);


            for (int i = 0; i <= (ntEl as IGroupElement).ElementCount - 1; i++)
            {
                (ntEl as IGroupElement).get_Element(i).Geometry = pp;
            }


            GrpEl.AddElement(ntEl);

            pp = new PointClass();
            pp.PutCoords(2.5, 1);

            if (originalElement is IGroupElement)
            {
                for (int i = 0; i <= (originalElement as IGroupElement).ElementCount - 1; i++)
                {
                    (originalElement as IGroupElement).get_Element(i).Geometry = pp;
                }
            }
            else
            originalElement.Geometry = pp;


            docElementProperties = GrpEl as IElementProperties3;
            docElementProperties.Name = NoteElement.Name + "_" + NoteElement.Id.ToString() + "_" + NoteElement.RelatedFeature;
            docElementProperties.AnchorPoint = esriAnchorPointEnum.esriCenterPoint;

            RemoveSameNote(docElementProperties.Name);


            //удаляем элемент на который он ссылается из списка
            var obj = (from element in SigmaDataCash.ChartElementList where (element != null) && (element is ChartElement_SimpleText) && (((ChartElement_SimpleText)element).Id.ToString().StartsWith(SelectedElement.Id.ToString())) select element).FirstOrDefault();
            if (obj != null)
            {
                SigmaDataCash.ChartElementList.Remove(obj);
            }

            SigmaDataCash.ChartElementList.Add(NoteElement);

            this.pGraphicsContainer.AddElement((IElement)GrpEl, 0);
            if (!KeepName) (pGraphicsContainer as IActiveView).Refresh();

        }

        private void RemoveSameNote(string NoteName)
        {
            IElement el = FindNote(NoteName);
            if (el != null)
                pGraphicsContainer.DeleteElement(el);
        }

        private IElement FindNote(string NoteName)
        {
            IElement res = null;

            pGraphicsContainer.Reset();
            IElementProperties3 docElementProperties;
            IElement sigma_el = pGraphicsContainer.Next();
            while (sigma_el != null)
            {
                docElementProperties = sigma_el as IElementProperties3;
                if (docElementProperties.Name.StartsWith(NoteName))
                {
                    res = sigma_el;
                    break;
                }
                sigma_el = pGraphicsContainer.Next();

            }

            return res;
        }

        private void RestoreFromNote(AbstractChartElement SelectedElement)
        {
            if ((SelectedElement is ChartElement_MarkerSymbol) !=true) return;

            string NoteName = SelectedElement.Name + "_" + SelectedElement.Id.ToString() + "_" + SelectedElement.RelatedFeature;
            RemoveSameNote(NoteName);


            string ObjType = ((ChartElement_MarkerSymbol)SelectedElement).TextContents[0][0].DataSource.Condition;

            object restObj = UnPackFromNote(SelectedElement);
            

            FeatureTreeView.SelectedNode.Tag = restObj;


            TreeNode nd = FeatureTreeView.SelectedNode;
            FeatureTreeView.SelectedNode = FeatureTreeView.Nodes[0];
            FeatureTreeView.SelectedNode = nd;

            FeatureTreeView.SelectedNode.Text = FeatureTreeView.SelectedNode.Text.Replace(" '", "");


            //удаляем элемент на который он ссылается из списка
            var obj = (from element in SigmaDataCash.ChartElementList where (element != null) && (element is ChartElement_SimpleText) && (((ChartElement_SimpleText)element).Id.ToString().StartsWith(SelectedElement.Id.ToString())) select element).FirstOrDefault();
            if (obj != null)
            {
                SigmaDataCash.ChartElementList.Remove(obj);
            }

            SigmaDataCash.ChartElementList.Add(restObj);

            
        }

        private object UnPackFromNote(AbstractChartElement SelectedElement)
        {
            string ObjType = ((ChartElement_MarkerSymbol)SelectedElement).TextContents[0][0].DataSource.Condition;

            object restObj = null;

            switch (ObjType)
            {
                case ("ChartElement_SimpleText"):
                    restObj = ChartElementsManipulator.DeserializeObject<ChartElement_SimpleText>(SelectedElement.Tag);
                    break;
                case ("ChartElement_RouteDesignator"):
                    restObj = ChartElementsManipulator.DeserializeObject<ChartElement_RouteDesignator>(SelectedElement.Tag);
                    break;
                case ("ChartElement_BorderedText"):
                    restObj = ChartElementsManipulator.DeserializeObject<ChartElement_BorderedText>(SelectedElement.Tag);
                    break;
                case ("ChartElement_BorderedText_Collout_CaptionBottom"):
                    restObj = ChartElementsManipulator.DeserializeObject<ChartElement_BorderedText_Collout_CaptionBottom>(SelectedElement.Tag);
                    break;
                case ("ChartElement_BorderedText_Collout"):
                    restObj = ChartElementsManipulator.DeserializeObject<ChartElement_BorderedText_Collout>(SelectedElement.Tag);
                    break;
                case ("ChartElement_SigmaCollout_Navaid"):
                    restObj = ChartElementsManipulator.DeserializeObject<ChartElement_SigmaCollout_Navaid>(SelectedElement.Tag);
                    break;
                case ("ChartElement_MarkerSymbol"):
                    restObj = ChartElementsManipulator.DeserializeObject<ChartElement_MarkerSymbol>(SelectedElement.Tag);
                    break;
                case ("ChartElement_TextArrow"):
                    restObj = ChartElementsManipulator.DeserializeObject<ChartElement_TextArrow>(SelectedElement.Tag);
                    break;
                case ("ChartElement_Radial"):
                    restObj = ChartElementsManipulator.DeserializeObject<ChartElement_Radial>(SelectedElement.Tag);
                    break;
                case ("ChartElement_SigmaCollout_Designatedpoint"):
                    restObj = ChartElementsManipulator.DeserializeObject<ChartElement_SigmaCollout_Designatedpoint>(SelectedElement.Tag);
                    break;
                case ("ChartElement_SigmaCollout_Airspace"):
                    restObj = ChartElementsManipulator.DeserializeObject<ChartElement_SigmaCollout_Airspace>(SelectedElement.Tag);
                    break;
                case ("ChartElement_SigmaCollout_AccentBar"):
                    restObj = ChartElementsManipulator.DeserializeObject<ChartElement_SigmaCollout_AccentBar>(SelectedElement.Tag);
                    break;
                default:
                    break;
            }

            return restObj;
        }

        private void zoomToLayerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string ChartElemenName = ((AbstractChartElement)FeatureTreeView.SelectedNode.Tag).Name;
            string LayerName =  ChartElementsManipulator.GetLinkedGeometryFeatureClassName(ChartElemenName);
            if (LayerName.StartsWith("GeoBorderCartography")) LayerName = "GeoBorder";
            ILayer _Layer = EsriUtils.getLayerByName2(pMap, LayerName);
            _Layer.Visible = true;



            ChartsHelperClass.ZoomToAreaOfInterestLayer(m_application, pMap, LayerName);
        }

        private void propertyGrid2_ControlAdded(object sender, ControlEventArgs e)
        {
            
        }

        private void propertyGrid2_SelectedObjectsChanged(object sender, EventArgs e)
        {
            if (propertyGrid2.SelectedObject != null)
            {
                tabControl1.SelectedIndex = 1;
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            panel2.Visible = !panel2.Visible;

            if (SigmaDataCash.ChartElementList != null && SigmaDataCash.ChartElementList.Count > 0 && panel2.Visible)
            {
                //string selectedChart = ((IMapDocument)m_application.Document).DocumentFilename;
                var ids = SigmaDataCash.ChartElementList.Select(y => ((AbstractChartElement)y).Name).Distinct().ToList();
                comboBox1.Items.Clear();
                foreach (var item in ids)
                {
                    //if (item.ToString().StartsWith("Holding")) continue;
                    //if (item.ToString().StartsWith("Graphics")) continue;
                    comboBox1.Items.Add(item.ToString());
                }


                if (comboBox1.Items.Count > 0) comboBox1.SelectedIndex = 0;
            }
            else
            {
                comboBox2.Items.Clear();
                comboBox2.Tag = null;
                comboBox1.Items.Clear();
            }

        }


        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

            comboBox2.Items.Clear();
            var fNodes = FeatureTreeView.Nodes.Find(comboBox1.Text, false);

            if (fNodes!=null && fNodes.Count() >0)
            {
                foreach (var nd in fNodes)
                {
                    if (nd.Nodes.Count >0)
                    {
                        foreach (TreeNode subND in nd.Nodes)
                        {
                            comboBox2.Items.Add(subND.Text);

                        }
                    }
                }
            }

            if (comboBox2.Items.Count > 0) comboBox2.SelectedIndex = 0;

        }


        private void button1_Click(object sender, EventArgs e)
        {
            var fNodes = FeatureTreeView.Nodes.Find(comboBox1.Text, false);

            foreach (TreeNode fND in fNodes[0].Nodes)
            {
                if (fND.Text.CompareTo(comboBox2.Text) == 0)
                {
                    FeatureTreeView.SelectedNode = fND;
                    break;
                }
            }

            showOnMapToolStripMenuItem1_Click(sender, e);



        }

        private void hideClauseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (FeatureTreeView.SelectedNode == null || FeatureTreeView.SelectedNode.Tag == null) return;


            string FN = System.IO.Path.GetDirectoryName((m_application.Document as IDocumentInfo2).Path);
            chartInfo ci = EsriUtils.GetChartIno(FN);

            ChartElement_SimpleText cartoEl = (ChartElement_SimpleText)FeatureTreeView.SelectedNode.Tag;

            HideClauseForm frm = ci == null ? new HideClauseForm("NM") :  new HideClauseForm(ci.uomDist);

            if (!cartoEl.RelatedFeature.Contains("ProcedureLeg")) frm.tabControl1.TabPages.Remove(frm.tabControl1.TabPages[1]);

            var dResult = frm.ShowDialog();
            if (dResult == DialogResult.OK)
            {
                UnselectFeatureClasses();

                string _clause = frm.Clause;
                bool _matchFlag = frm.Match;
                int _len = frm.legLength; 
                string _lenUom = frm.lengthUOM;
                //List<TreeNode> ListToHide = new List<TreeNode>();

                SigmaDataCash.environmentWorkspaceEdit.StartEditing(false);
                SigmaDataCash.environmentWorkspaceEdit.StartEditOperation();

                
                if (cartoEl.TextContents == null) return;

                // int i = 0;


                foreach (TreeNode _childNode in FeatureTreeView.SelectedNode.Nodes)
                {

                    ChartElement_SimpleText cartoSubEl = (ChartElement_SimpleText)_childNode.Tag;
                    if(!cartoSubEl.Placed) continue;
                    if (_clause != null)
                    {
                        foreach (var _Line in cartoSubEl.TextContents)
                        {
                            bool breakFlag = false;
                            foreach (var _word in _Line)
                            {
                                breakFlag = _matchFlag ? (_word.TextValue.Trim().CompareTo(_clause) == 0) : (_word.TextValue.Contains(_clause));
                                if (breakFlag)
                                {
                                    HideElementNode_SimpleText(_childNode);
                                    break;
                                }
                            }

                            if (breakFlag) break;
                        }
                    }
                    else if (_len >= 1)
                    {
                        ILayer _Layer = EsriUtils.getLayerByName2(pMap, "ProcedureLegsCarto");
                        ITable Tbl = ((IFeatureLayer)_Layer).FeatureClass as ITable;
                        IQueryFilter queryFilter = new QueryFilterClass();


                        string gId = cartoSubEl.LinckedGeoId;


                        queryFilter.WhereClause = "FeatureGUID = '" + gId + "'";

                        ICursor rowCur = Tbl.Search(queryFilter, true);

                        IRow _row = rowCur.NextRow();
                        if (_row != null)
                        {
                            double _l = (double)_row.Value[_row.Fields.FindField("length")];
                            //string _uom = (string)_row.Value[_row.Fields.FindField("lengthUOM")];

                            if (Math.Round(_l) <= _len)
                            {
                                HideElementNode_SimpleText(_childNode);
                        
                            }
                        }


                        Marshal.ReleaseComObject(queryFilter);
                    }
                    //i++;

                }
                

                SigmaDataCash.environmentWorkspaceEdit.StopEditOperation();
                SigmaDataCash.environmentWorkspaceEdit.StopEditing(true);

                //(pMap as IActiveView).Refresh();
                ChartElementsManipulator.RefreshChart((IMxDocument)m_application.Document);

            }
            else if (dResult == DialogResult.Abort)
            {
                SigmaDataCash.environmentWorkspaceEdit.StartEditing(false);
                SigmaDataCash.environmentWorkspaceEdit.StartEditOperation();


                if (cartoEl.TextContents == null) return;

                foreach (TreeNode _childNode in FeatureTreeView.SelectedNode.Nodes)
                {

                    ChartElement_SimpleText cartoSubEl = (ChartElement_SimpleText)_childNode.Tag;
                    if (cartoSubEl.Placed) continue;

                    HideElementNode_SimpleText(_childNode);
                }


                SigmaDataCash.environmentWorkspaceEdit.StopEditOperation();
                SigmaDataCash.environmentWorkspaceEdit.StopEditing(true);

                //(pMap as IActiveView).Refresh();
                ChartElementsManipulator.RefreshChart((IMxDocument)m_application.Document);
            }
        }

        private void attachRouteToolStripMenuItem_Click(object sender, EventArgs e)
        {

            SigmaDataCash.environmentWorkspaceEdit.StartEditOperation();
            SigmaDataCash.environmentWorkspaceEdit.StartEditing(true);


            if (IsPrototypeElement((AbstractChartElement)FeatureTreeView.SelectedNode.Tag))
            {
                ChartElement_RouteDesignator protoCartoEl = (ChartElement_RouteDesignator)FeatureTreeView.SelectedNode.Tag;
                protoCartoEl.HideDesignatorText = !protoCartoEl.HideDesignatorText;


                foreach (TreeNode nd in FeatureTreeView.SelectedNode.Nodes)
                {
                    if (nd == null) continue;
                    if (nd.Tag is ChartElement_RouteDesignator)
                        ChangeDesignatorTextState(nd, protoCartoEl.HideDesignatorText);

                }

                SavePrototypesElements();
            }
            else
            {
                ChangeDesignatorTextState(FeatureTreeView.SelectedNode,!((ChartElement_RouteDesignator)FeatureTreeView.SelectedNode.Tag).HideDesignatorText);
            }
            
            SigmaDataCash.environmentWorkspaceEdit.StopEditOperation();
            SigmaDataCash.environmentWorkspaceEdit.StopEditing(true);


            FeatureTreeView.Refresh();
            
            
            ChartElementsManipulator.RefreshChart((IMxDocument)m_application.Document);

        }

        private void ChangeDesignatorTextState(TreeNode _Node, bool _HideDesignatorText = true)
        {
            if (_Node.Tag == null) return;

            ChartElement_RouteDesignator cartoEl = (ChartElement_RouteDesignator)_Node.Tag;

            cartoEl.HideDesignatorText = _HideDesignatorText;

            IElement el = cartoEl.ConvertToIElement() as IElement;

            ChartElementsManipulator.UpdateSingleElementToDataSet(cartoEl.Name, cartoEl.Id.ToString(), el, ref cartoEl);


            if (_HideDesignatorText)
            {
                ChartElement_SimpleText chrtDesignatorText = ChartElementsManipulator.ConstructDesignatorElement(cartoEl);
                chrtDesignatorText.Tag = cartoEl.Id.ToString();
                IElement el_rtDesignatorText = (IElement)chrtDesignatorText.ConvertToIElement();


                el_rtDesignatorText.Geometry = ((IGroupElement)el).Element[0].Geometry;
                ChartElementsManipulator.StoreSingleElementToDataSet(chrtDesignatorText.Name, cartoEl.LinckedGeoId.ToString(), el_rtDesignatorText, ref chrtDesignatorText, chrtDesignatorText.Id, pMap.MapScale);

                TreeNode Nd = new TreeNode(((ChartElement_SimpleText)chrtDesignatorText).TextContents[0][0].TextValue) { };
                Nd.Tag = chrtDesignatorText;
                _Node.Parent.Nodes.Add(Nd);

            }
            else
            {

                string ChartElemenName = cartoEl.Name;
                ILayer _Layer = EsriUtils.getLayerByName2(pMap, ChartElementsManipulator.DefinelayerName(ChartElemenName));
                ITable Tbl = ((IFeatureLayer)_Layer).FeatureClass as ITable;



                var chartEl = (from obj in SigmaDataCash.ChartElementList
                               where (obj != null) && (obj is ChartElement_SimpleText) &&
                               ((ChartElement_SimpleText)obj).Tag != null &&
                               ((ChartElement_SimpleText)obj).Tag.CompareTo(cartoEl.Id.ToString()) == 0 &&
                               ((ChartElement_SimpleText)obj).Name != null &&
                          ((ChartElement_SimpleText)obj).Name.CompareTo(cartoEl.Name) == 0 &&
                          ((ChartElement_SimpleText)obj).RelatedFeature != null &&
                          ((ChartElement_SimpleText)obj).RelatedFeature.CompareTo(cartoEl.RelatedFeature) == 0 &&
                          ((ChartElement_SimpleText)obj).LinckedGeoId != null &&
                          ((ChartElement_SimpleText)obj).LinckedGeoId.CompareTo(cartoEl.LinckedGeoId) == 0
                               select obj).FirstOrDefault();


                if (chartEl != null)
                {

                    IQueryFilter queryFilter = new QueryFilterClass();
                    queryFilter.WhereClause = "PdmUid = '" + cartoEl.LinckedGeoId + "' AND AncorUID = '" + ((ChartElement_SimpleText)chartEl).Id.ToString() + "'";

                    Tbl.DeleteSearchedRows(queryFilter);

                    IFeatureClass featureClass = ChartElementsManipulator.GetLinkedFeatureClass("Mirror");
                    Tbl = featureClass as ITable;
                    queryFilter.WhereClause = "AncorUID = '" + ((ChartElement_SimpleText)chartEl).Id.ToString() + "'";

                    Tbl.DeleteSearchedRows(queryFilter);

                    TreeNode _node = getChildNode(_Node.Parent, (AbstractChartElement)chartEl);
                    if (_node != null) _Node.Parent.Nodes.Remove(_node);
                    SigmaDataCash.ChartElementList.Remove(chartEl);
                }

                UpdateElement(FeatureTreeView.SelectedNode);

            }

        }

        private TreeNode getChildNode(TreeNode ParentNode, AbstractChartElement ChartEl)
        {
            TreeNode res = null;

            foreach (TreeNode nd in ParentNode.Nodes)
            {
                if (nd.Tag == null) continue;
                if (nd.Tag is ChartElement_SimpleText)
                {
                    if (((ChartElement_SimpleText)nd.Tag).Id.Equals(ChartEl.Id))
                    {
                        res = nd;
                        break;
                    }
                }


            }

            return res; ;

        }

        private void rotateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (FeatureTreeView.SelectedNode.Tag == null) return;
            TreeNode nd = FeatureTreeView.SelectedNode;

            if (nd.Tag is ChartElement_MarkerSymbol && !((ChartElement_MarkerSymbol)nd.Tag).Name.StartsWith("Airspace_Class"))
            {
                ChartElement_MarkerSymbol cartoEl = (ChartElement_MarkerSymbol)nd.Tag;

                if (cartoEl.MarkerBackGround.CharacterIndex == 118)
                {
                    cartoEl.MarkerBackGround.CharacterIndex = 94;
                    cartoEl.MarkerBackGround.InnerCharacterIndex = 95;
                }
                else
                {
                    cartoEl.MarkerBackGround.CharacterIndex = 118;
                    cartoEl.MarkerBackGround.InnerCharacterIndex = 119;
                }

            }


            SigmaDataCash.environmentWorkspaceEdit.StartEditOperation();
            SigmaDataCash.environmentWorkspaceEdit.StartEditing(true);

            UpdateElement(FeatureTreeView.SelectedNode);

            SigmaDataCash.environmentWorkspaceEdit.StopEditOperation();
            SigmaDataCash.environmentWorkspaceEdit.StopEditing(true);


            ChartElementsManipulator.RefreshChart((IMxDocument)m_application.Document);
        }

        private void cloneToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (FeatureTreeView.SelectedNode == null || FeatureTreeView.SelectedNode.Tag == null) return;

            AbstractChartElement cartoEl = (AbstractChartElement)FeatureTreeView.SelectedNode.Tag;
            TreeNode parentNd = FeatureTreeView.SelectedNode.Parent;
            AbstractChartElement newCloneElement = ChartElementsManipulator.DoCloneElement(cartoEl);

            IGeometry geom = ChartElementsManipulator.GetLinkedGeometry(cartoEl.Name, cartoEl.LinckedGeoId);

            if (cartoEl.Name.StartsWith("Airspace_Class"))
                geom = ((IPointCollection)geom).get_Point(0);

            IElement newIElement = (IElement)newCloneElement.ConvertToIElement();

            if (newIElement is IGroupElement)
            {
                for (int i = 0; i < ((IGroupElement)newIElement).ElementCount; i++)
                {
                    ((IGroupElement)newIElement).get_Element(i).Geometry = geom;
                }
            }
            else
                newIElement.Geometry = geom;

            newCloneElement.Tag = "Clone";

            ChartElementsManipulator.StoreSingleElementToDataSet(newCloneElement.Name, newCloneElement.LinckedGeoId.ToString(), newIElement, ref newCloneElement, newCloneElement.Id, pMap.MapScale);



            string ndTxt = FeatureTreeView.SelectedNode.Text.EndsWith("©") ? FeatureTreeView.SelectedNode.Text : FeatureTreeView.SelectedNode.Text + "©";
            TreeNode newNode = new TreeNode(ndTxt);
            newNode.Tag = newCloneElement;
            parentNd.Nodes.Add(newNode);
            FeatureTreeView.Refresh();
            ChartElementsManipulator.RefreshChart((IMxDocument)m_application.Document);

        }

        private void deleteCloneToolStripMenuItem_Click(object sender, EventArgs e)
        {

            UnselectFeatureClasses();

            if (FeatureTreeView.SelectedNode == null || FeatureTreeView.SelectedNode.Tag == null || FeatureTreeView.SelectedNode.Nodes.Count >0) return;

            AbstractChartElement cartoEl = (AbstractChartElement)FeatureTreeView.SelectedNode.Tag;

            ILayer SelLayer = EsriUtils.getLayerByName2(pMap, ChartElementsManipulator.DefinelayerName(cartoEl.Name));
            if (SelLayer == null) return;
            ITable Tbl = ((IFeatureLayer)SelLayer).FeatureClass as ITable;


            SigmaDataCash.environmentWorkspaceEdit.StartEditing(false);
            SigmaDataCash.environmentWorkspaceEdit.StartEditOperation();

            IQueryFilter queryFilter = new QueryFilterClass();
            queryFilter.WhereClause = "PdmUid = '" + cartoEl.LinckedGeoId + "' AND AncorUID = '" + ((ChartElement_SimpleText)cartoEl).Id.ToString() + "'";

            Tbl.DeleteSearchedRows(queryFilter);

            Marshal.ReleaseComObject(queryFilter);

            var prnNd = FeatureTreeView.SelectedNode.Parent;
            FeatureTreeView.SelectedNode.Parent.Nodes.Remove(FeatureTreeView.SelectedNode);
            if (prnNd.Text.StartsWith("FreeAnno") )
            {
                if (prnNd.Nodes.Count <= 0)
                    FeatureTreeView.Nodes.Remove(prnNd);
                else
                    prnNd.Text = "FreeAnno" + " (" + prnNd.Nodes.Count.ToString() + ")";
            }

            SigmaDataCash.ChartElementList.Remove(cartoEl);
           

            SigmaDataCash.environmentWorkspaceEdit.StopEditOperation();
            SigmaDataCash.environmentWorkspaceEdit.StopEditing(true);

            //(pMap as IActiveView).Refresh();
            ChartElementsManipulator.RefreshChart((IMxDocument)m_application.Document);
            FeatureTreeView.Refresh();
        }

        private void updateAnnotationReflectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (FeatureTreeView.SelectedNode.Tag == null) return;


            SigmaDataCash.environmentWorkspaceEdit.StartEditing(false);
            SigmaDataCash.environmentWorkspaceEdit.StartEditOperation();

            UpdateElementInFrame((AbstractChartElement)propertyGrid1.SelectedObject);
           
            SigmaDataCash.environmentWorkspaceEdit.StopEditOperation();
            SigmaDataCash.environmentWorkspaceEdit.StopEditing(true);

            //(pMap as IActiveView).Refresh();
            ChartElementsManipulator.RefreshChart((IMxDocument)m_application.Document);
        }

        private void updateAnnotationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (FeatureTreeView.SelectedNode.Tag == null) return;

            SigmaDataCash.environmentWorkspaceEdit.StartEditing(false);
            SigmaDataCash.environmentWorkspaceEdit.StartEditOperation();

            if (tabControl1.SelectedIndex == 0)
            {
                UpdateElement(FeatureTreeView.SelectedNode, false);
                UpdateExistingNote((AbstractChartElement)FeatureTreeView.SelectedNode.Tag);


            }
            else if (tabControl1.SelectedIndex == 1 && propertyGrid2.SelectedObject != null && propertyGrid2.SelectedObject is AbstractChartElement)
            {
                UpdateElementInFrame((AbstractChartElement)propertyGrid2.SelectedObject);
            }

            SigmaDataCash.environmentWorkspaceEdit.StopEditOperation();
            SigmaDataCash.environmentWorkspaceEdit.StopEditing(true);




            //(pMap as IActiveView).Refresh();
            ChartElementsManipulator.RefreshChart((IMxDocument)m_application.Document);
        }

        private void hToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (FeatureTreeView.SelectedNode == null) return;


            UnselectFeatureClasses();


            SigmaDataCash.environmentWorkspaceEdit.StartEditing(false);
            SigmaDataCash.environmentWorkspaceEdit.StartEditOperation();

            foreach (TreeNode _childNode in FeatureTreeView.SelectedNode.Nodes)
            {

                HideElementNode_SimpleText(_childNode);

            }


            SigmaDataCash.environmentWorkspaceEdit.StopEditOperation();
            SigmaDataCash.environmentWorkspaceEdit.StopEditing(true);

            //(pMap as IActiveView).Refresh();
            ChartElementsManipulator.RefreshChart((IMxDocument)m_application.Document);


        }

        private void propertyGrid1_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
           
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {

        }

        private void toolStripTextBox1_Click(object sender, EventArgs e)
        {

        }

        private void createNewAnnotationToolStripMenuItem_Click(object sender, EventArgs e)
        {


        }

        private void navaidAnnotationToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void desegnatedPointAnnotationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (DataCash.ProjectEnvironment!=null && DataCash.ProjectEnvironment.Data!=null && DataCash.ProjectEnvironment.Data.PdmObjectList !=null)
            {
                var _procList = (from element in DataCash.ProjectEnvironment.Data.PdmObjectList where (element != null) && 
                                (element.PDM_Type == PDM.PDM_ENUM.WayPoint)  select element).ToList();

                if (_procList != null && _procList.Count > 0)
                {


                    AddNewAnnoForm frm = new AddNewAnnoForm(_procList);
                    var res = frm.ShowDialog();

                    if (res == DialogResult.Cancel || frm.Selected_PDMList == null || frm.Selected_PDMList.Count == 0) return;




                }



            }
        }

        private void length01ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (FeatureTreeView.SelectedNode.Tag == null) return;

            AbstractChartElement cartoEl = (AbstractChartElement)FeatureTreeView.SelectedNode.Tag;
            if (!IsPrototypeElement(cartoEl)) return;

            ARENA.ArenaInputForm frm = new ArenaInputForm("Precision", "Number of digits after decimal", 1, 0);

            int dn = 0;
            if (frm.ShowDialog() == DialogResult.OK)
                dn = frm.IntValue;
            else return;

            SigmaDataCash.environmentWorkspaceEdit.StartEditing(false);
            SigmaDataCash.environmentWorkspaceEdit.StartEditOperation();

            foreach (TreeNode item in FeatureTreeView.SelectedNode.Nodes)
            {

                var SelectedElement = (AbstractChartElement)item.Tag;
                //var val = ChartElementsManipulator.GetLinkedProperty(SelectedElement.Name, SelectedElement.LinckedGeoId,"ProcName");

                PDM.PDMObject objPdm = DataCash.GetPDMObject(SelectedElement.LinckedGeoId, PDM.PDM_ENUM.ProcedureLeg);
                ChartElement_MarkerSymbol chrtEl_legLength = (ChartElement_MarkerSymbol)SelectedElement;
                chrtEl_legLength.TextContents[0][0].TextValue = ChartsHelperClass.MakeText_UOM(objPdm, chrtEl_legLength.TextContents[0][0].DataSource, "NM", dn);
                if (chrtEl_legLength.TextContents.Count > 1)
                    chrtEl_legLength.TextContents[1][0].TextValue = ChartsHelperClass.MakeText_UOM(objPdm, chrtEl_legLength.TextContents[1][0].DataSource, "NM", dn);


                if (tabControl1.SelectedIndex == 0)
                {
                    UpdateElement(item, false);
                    UpdateExistingNote((AbstractChartElement)item.Tag);


                }
                else if (tabControl1.SelectedIndex == 1 && propertyGrid2.SelectedObject != null && propertyGrid2.SelectedObject is AbstractChartElement)
                {
                    UpdateElementInFrame((AbstractChartElement)propertyGrid2.SelectedObject);
                }
            }

            SigmaDataCash.environmentWorkspaceEdit.StopEditOperation();
            SigmaDataCash.environmentWorkspaceEdit.StopEditing(true);




            //(pMap as IActiveView).Refresh();
            ChartElementsManipulator.RefreshChart((IMxDocument)m_application.Document);
        }

        private void heightFLToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //ARENA.ArenaInputForm frm = new ArenaInputForm("Transition Altitude", "Please, input Transition Altitude value", 10000);

            //if (frm.ShowDialog() == DialogResult.OK)
            //{ int i = frm.IntValue; }
            //return;

            if (FeatureTreeView.SelectedNode.Tag == null) return;
            List<TreeNode> _nodes = new List<TreeNode>();

            AbstractChartElement cartoEl = (AbstractChartElement)FeatureTreeView.SelectedNode.Tag;
            if (!IsPrototypeElement(cartoEl))
                _nodes.Add(FeatureTreeView.SelectedNode);
            else
                foreach (TreeNode item in FeatureTreeView.SelectedNode.Nodes) _nodes.Add(item);

            SigmaDataCash.environmentWorkspaceEdit.StartEditing(false);
            SigmaDataCash.environmentWorkspaceEdit.StartEditOperation();

            foreach (TreeNode item in _nodes)
            {
                int lnIndx = 0;
                int wrdIndx = 0;
                var SelectedElement = (AbstractChartElement)item.Tag;

                ChartElement_TextArrow chrtEl_legHeight = (ChartElement_TextArrow)SelectedElement;

                foreach (var line in chrtEl_legHeight.TextContents)
                {
                    foreach (var wrd in line)
                    {
                        if (!wrd.Visible || !IsNumeric(wrd.TextValue)) continue;
                        
                        double h = Convert.ToDouble(wrd.TextValue);

                        if (h >= 10000)
                        {
                            double h1 = h / 1000;
                            h1 = Math.Round(h1 * 2) / 2;
                            chrtEl_legHeight.TextContents[lnIndx][wrdIndx].TextValue = "FL " + (h1*10).ToString();
                        }

                        wrdIndx++;
                    }

                    lnIndx++;
                }

                if (tabControl1.SelectedIndex == 0)
                {
                    UpdateElement(item, false);
                    UpdateExistingNote((AbstractChartElement)item.Tag);


                }
                else if (tabControl1.SelectedIndex == 1 && propertyGrid2.SelectedObject != null && propertyGrid2.SelectedObject is AbstractChartElement)
                {
                    UpdateElementInFrame((AbstractChartElement)propertyGrid2.SelectedObject);
                }
            }

            SigmaDataCash.environmentWorkspaceEdit.StopEditOperation();
            SigmaDataCash.environmentWorkspaceEdit.StopEditing(true);

            ChartElementsManipulator.RefreshChart((IMxDocument)m_application.Document);
        }

        private static bool IsNumeric(string anyString)
        {
            if (anyString == null)
            {
                anyString = "";
            }
            if (anyString.Length > 0)
            {
                double dummyOut = new double();
                System.Globalization.CultureInfo cultureInfo = new System.Globalization.CultureInfo("en-US", true);
                bool res = Double.TryParse(anyString, out dummyOut) && !Double.IsNaN(dummyOut);
                return res;
            }
            else
            {
                return false;
            }
        }

        private void textContextEditorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (SigmaDataCash.AncorPropertyGrid.SelectedObject == null) return;

            TextContestEditorForm txtFrm = new TextContestEditorForm();

            if (!ArenaStaticProc.HasProperty(SigmaDataCash.AncorPropertyGrid.SelectedObject, "TextContents"))
                txtFrm.TextContents = null;
            else
                txtFrm.TextContents = ((ChartElement_SimpleText)SigmaDataCash.AncorPropertyGrid.SelectedObject).TextContents;

            if (!ArenaStaticProc.HasProperty(SigmaDataCash.AncorPropertyGrid.SelectedObject, "CaptionTextLine"))
                txtFrm.CaptionTextContest = null;
            else
                txtFrm.CaptionTextContest = ((ChartElement_BorderedText_Collout_CaptionBottom)SigmaDataCash.AncorPropertyGrid.SelectedObject).CaptionTextLine;

            if (!ArenaStaticProc.HasProperty(SigmaDataCash.AncorPropertyGrid.SelectedObject, "BottomTextLine"))
                txtFrm.BottomTextContest = null;
            else
                txtFrm.BottomTextContest = ((ChartElement_BorderedText_Collout_CaptionBottom)SigmaDataCash.AncorPropertyGrid.SelectedObject).BottomTextLine;


            txtFrm.ShowDialog();
        }

        private void FeatureTreeView_DoubleClick(object sender, EventArgs e)
        {
            textContextEditorToolStripMenuItem_Click(sender, e);
        }

        private void flipTextToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (FeatureTreeView.SelectedNode.Tag == null) return;
            TreeNode nd = FeatureTreeView.SelectedNode;
            if (!(nd.Tag is ChartElement_SimpleText)) return;

            if (nd.Tag is ChartElement_MarkerSymbol && !((ChartElement_MarkerSymbol)nd.Tag).Name.StartsWith("Airspace_Class"))
            {
                ChartElement_MarkerSymbol cartoEl = (ChartElement_MarkerSymbol)nd.Tag;

                if (cartoEl.MarkerBackGround.CharacterIndex == 118)
                {
                    cartoEl.MarkerBackGround.CharacterIndex = 94;
                    cartoEl.MarkerBackGround.InnerCharacterIndex = 95;
                }
                else
                {
                    cartoEl.MarkerBackGround.CharacterIndex = 118;
                    cartoEl.MarkerBackGround.InnerCharacterIndex = 119;
                }

            }

            (nd.Tag as ChartElement_SimpleText).Slope += 180; 

            if ((nd.Tag as ChartElement_SimpleText).Slope > 360)
                (nd.Tag as ChartElement_SimpleText).Slope -=360;

            SigmaDataCash.environmentWorkspaceEdit.StartEditOperation();
            SigmaDataCash.environmentWorkspaceEdit.StartEditing(true);

            UpdateElement(FeatureTreeView.SelectedNode);

            SigmaDataCash.environmentWorkspaceEdit.StopEditOperation();
            SigmaDataCash.environmentWorkspaceEdit.StopEditing(true);


            ChartElementsManipulator.RefreshChart((IMxDocument)m_application.Document);
        }

        private void findAndReplaceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // TODO: Add Command1.OnClick implementation
            if (SigmaDataCash.ChartElementList == null) return;
            if (FeatureTreeView.SelectedNode == null) return;
            if (FeatureTreeView.SelectedNode.Tag == null) return;
            if (FeatureTreeView.SelectedNode.Parent == null) return;
            if (FeatureTreeView.SelectedNode.Parent.Tag == null) return;

            AbstractChartElement chartEl = ((AbstractChartElement)FeatureTreeView.SelectedNode.Tag);

            FindReplaceForm fr = new FindReplaceForm();

            fr.comboBox1.Items.Clear();

            foreach (var item in SigmaDataCash.prototype_anno_lst)
            {
                if (item.RelatedFeature.Contains("Graphics")) continue;

                if (SigmaDataCash.ChartElementList.FindAll(el => (el is AbstractChartElement) && ((AbstractChartElement)el).Name.CompareTo(item.Name) == 0).Count <= 0) continue;

                if (!fr.comboBox1.Items.Contains(item.Name))
                    fr.comboBox1.Items.Add(item.Name);
            }


            fr.oldTextBox.Text = "";
            fr.newTextBox.Text = "";

            fr.comboBox1.Text = chartEl.Name;
            fr.comboBox1.Enabled = false;

            if (fr.ShowDialog() != System.Windows.Forms.DialogResult.OK) return;

            string _oldTxt = fr.oldTextBox.Text;
            string _newTxt = fr.newTextBox.Text;
            bool MatchCase = fr.CaseCheckBox.Checked;
            bool MatchWholeWord = fr.WordCheckBox.Checked;
            bool ignoreAnno = fr.AnnoCheckBox.Checked;
            bool ignoreGraphics = fr.GraphicsElementsCheckBox.Checked;
            string FelFeature = fr.comboBox1.Text;

            string Message = ChartElementsManipulator.FindAndReplaceUtil(_oldTxt, _newTxt, MatchCase, MatchWholeWord, ignoreAnno, ignoreGraphics, FelFeature, pGraphicsContainer);


            if (Message.Length > 0)
            {
                Message = Message + Environment.NewLine + "Please refresh SIGMA TOC";
                ChartElementsManipulator.RefreshChart((IMxDocument)m_application.Document);
                //RefreshList();

            }

            MessageBox.Show(Message, "Sigma",MessageBoxButtons.OK,MessageBoxIcon.Information);


        }
    }
}
