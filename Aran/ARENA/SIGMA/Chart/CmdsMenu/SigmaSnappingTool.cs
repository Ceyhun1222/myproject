using System;
using System.Linq;
using System.Drawing;
using System.Runtime.InteropServices;
using ESRI.ArcGIS.ADF.BaseClasses;
using ESRI.ArcGIS.ADF.CATIDs;
using ESRI.ArcGIS.Controls;
using System.Windows.Forms;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Carto;
using EsriWorkEnvironment;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.esriSystem;
using System.Collections.Generic;
using ESRI.ArcGIS.Framework;
using ESRI.ArcGIS.Display;
using ANCOR.MapElements;
using DataModule;
using ARENA.Enums_Const;
using ArenaStatic;
using ESRI.ArcGIS.ArcMapUI;
using ANCOR.MapCore;
using ESRI.ArcGIS.Editor;
using ChartCodingTable;
using ARENA;
using PDM;

namespace SigmaChart.CmdsMenu
{
    /// <summary>
    /// Summary description for ChartElementSelector.
    /// </summary>
    [Guid("7600501c-0248-474f-a367-bb2f2a0941d7")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("SigmaChart.SigmaSnappingTool")]
    public sealed class SigmaSnappingTool : BaseTool
    {
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
            MxCommands.Register(regKey);
            ControlsCommands.Register(regKey);
        }
        /// <summary>
        /// Required method for ArcGIS Component Category unregistration -
        /// Do not modify the contents of this method with the code editor.
        /// </summary>
        private static void ArcGISCategoryUnregistration(Type registerType)
        {
            string regKey = string.Format("HKEY_CLASSES_ROOT\\CLSID\\{{{0}}}", registerType.GUID);
            MxCommands.Unregister(regKey);
            ControlsCommands.Unregister(regKey);
        }

        #endregion
        #endregion

        private IHookHelper m_hookHelper = null;
        private IApplication m_application;
        private IHookHelper2 hookHelper2 = null;
        IExtensionManager extensionManager = null;

        public IPointSnapper Snapper { get; private set; }
        public ISnappingFeedback SnappingFeedback { get; private set; }
        public ISnappingEnvironment SnapEnvoirment { get; set; }
        bool _isSnapped = false;
        ISnappingResult snapResult = null;
        IPoint mapPoint = null;
        IFeatureClass Anno_DecorPointCartography_featClass = null;

        public SigmaSnappingTool()
        {
            //
            // TODO: Define values for the public properties
            //
            base.m_category = "SigmaChart"; //localizable text
            base.m_caption = "snapping";  //localizable text 
            base.m_message = "snapping";  //localizable text
            base.m_toolTip = "snapping";  //localizable text
            base.m_name = "snapping";   //unique id, non-localizable (e.g. "MyCategory_MyCommand")
            try
            {
                //
                // TODO: change resource name if necessary 
                //
                base.m_bitmap = global::SigmaChart.Properties.Resources.LayerLockedData16;
                //base.m_cursor = new System.Windows.Forms.Cursor(GetType(), GetType().Name + ".cur");

            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(ex.Message, "Invalid Bitmap");
            }
        }

        #region Overridden Class Methods

        /// <summary>
        /// Occurs when this tool is created
        /// </summary>
        /// <param name="hook">Instance of the application</param>
        public override void OnCreate(object hook)
        {
            try
            {
                m_hookHelper = new HookHelperClass();
                m_hookHelper.Hook = hook;
                if (m_hookHelper.ActiveView == null)
                {
                    m_hookHelper = null;
                }

                m_application = hook as IApplication;

            }
            catch
            {
                m_hookHelper = null;
            }

            if (m_hookHelper == null)
                base.m_enabled = false;
            else
                base.m_enabled = true;


            hookHelper2 = (IHookHelper2)m_hookHelper;
            extensionManager = hookHelper2.ExtensionManager;
            // TODO:  Add other initialization code
        }

        /// <summary>
        /// Occurs when this tool is clicked
        /// </summary>
        public override void OnClick()
        {
            if (extensionManager != null)
            {
                UID guid = new UIDClass();
                guid.Value = "{E07B4C52-C894-4558-B8D4-D4050018D1DA}"; //Snapping extension.
                IExtension extension = extensionManager.FindExtension(guid);
                SnapEnvoirment = extension as ISnappingEnvironment;

                Snapper = SnapEnvoirment.PointSnapper;

                SnappingFeedback = new SnappingFeedbackClass();
                SnapEnvoirment.SnappingType = esriSnappingType.esriSnappingTypePoint | esriSnappingType.esriSnappingTypeVertex;
                SnapEnvoirment.ShowSnapTips = false;
                SnapEnvoirment.SnapTipSymbol.Text = "";
                SnapEnvoirment.TextSnapping = false;
                SnapEnvoirment.Tolerance = 5;
                SnappingFeedback.Initialize(m_hookHelper.Hook, SnapEnvoirment, true);

                if (SigmaDataCash.AnnotationLinkedGeometryList.ContainsKey("DecorPointCartography"))
                    Anno_DecorPointCartography_featClass = SigmaDataCash.AnnotationLinkedGeometryList["DecorPointCartography"];
                //else
                //{
                //    Anno_DecorPointCartography_featClass = CreateAnno_DecorPointCartography_featClass();
                //    SigmaDataCash.AnnotationLinkedGeometryList.Add("DecorPointCartography", Anno_DecorPointCartography_featClass);


                //    IFeatureClass Anno_Nav_featClass = (IFeatureClass)SigmaDataCash.AnnotationFeatureClassList["NavaidsAnno"];
                //    CreateFeatureClass(Anno_DecorPointCartography_featClass.FeatureDataset, "FreeAnno", m_hookHelper.ActiveView.FocusMap.SpatialReference, 
                //                       Anno_Nav_featClass.Fields,esriFeatureType.esriFTAnnotation, esriGeometryType.esriGeometryAny);


                //}


            }
            _isSnapped = false;

        }

        private IFeatureClass CreateFeatureClass(object objectWorkspace , string name, ISpatialReference spatialReference, IFields fields, esriFeatureType featureType = esriFeatureType.esriFTSimple,
                                           esriGeometryType geometryType = esriGeometryType.esriGeometryPoint, UID uidCLSID = null, UID uidCLSEXT = null)
        {
            if (objectWorkspace == null) return null;
            if (!((objectWorkspace is IWorkspace) ||(objectWorkspace is IFeatureDataset))) return null;
            if (name == "") return null;
            if ((objectWorkspace is IWorkspace) && (spatialReference == null)) return null;

            // Set ClassID (if Null)
            if (uidCLSID == null)
            {
                uidCLSID = new UIDClass();
                switch (featureType)
                {
                    case (esriFeatureType.esriFTSimple):
                        uidCLSID.Value = "{52353152-891A-11D0-BEC6-00805F7C4268}";
                        break;
                    case (esriFeatureType.esriFTSimpleJunction):
                        geometryType = esriGeometryType.esriGeometryPoint;
                        uidCLSID.Value = "{CEE8D6B8-55FE-11D1-AE55-0000F80372B4}";
                        break;
                    case (esriFeatureType.esriFTComplexJunction):
                        uidCLSID.Value = "{DF9D71F4-DA32-11D1-AEBA-0000F80372B4}";
                        break;
                    case (esriFeatureType.esriFTSimpleEdge):
                        geometryType = esriGeometryType.esriGeometryPolyline;
                        uidCLSID.Value = "{E7031C90-55FE-11D1-AE55-0000F80372B4}";
                        break;
                    case (esriFeatureType.esriFTComplexEdge):
                        geometryType = esriGeometryType.esriGeometryPolyline;
                        uidCLSID.Value = "{A30E8A2A-C50B-11D1-AEA9-0000F80372B4}";
                        break;
                    case (esriFeatureType.esriFTAnnotation):
                        geometryType = esriGeometryType.esriGeometryPolygon;
                        uidCLSID.Value = "{E3676993-C682-11D2-8A2A-006097AFF44E}";
                        break;
                    case (esriFeatureType.esriFTDimension):
                        geometryType = esriGeometryType.esriGeometryPolygon;
                        uidCLSID.Value = "{496764FC-E0C9-11D3-80CE-00C04F601565}";
                        break;
                }
            }

            // Set uidCLSEXT (if Null)
            if (uidCLSEXT == null)
            {
                switch (featureType)
                {
                    case (esriFeatureType.esriFTAnnotation):
                        uidCLSEXT = new UIDClass();
                        uidCLSEXT.Value = "{24429589-D711-11D2-9F41-00C04F6BC6A5}";
                        break;
                    case (esriFeatureType.esriFTDimension):
                        uidCLSEXT = new UIDClass();
                        uidCLSEXT.Value = "{48F935E2-DA66-11D3-80CE-00C04F601565}";
                        break;
                }
            }

            // Add Fields
            if (fields == null)
            {
                // Create fields collection
                fields = new FieldsClass();
                IFieldsEdit fieldsEdit = (IFieldsEdit)fields;

                // Create the geometry field
                IGeometryDef geometryDef = new GeometryDefClass();
                IGeometryDefEdit geometryDefEdit = (IGeometryDefEdit)geometryDef;

                // Assign Geometry Definition
                geometryDefEdit.GeometryType_2 = geometryType;
                geometryDefEdit.GridCount_2 = 1;
                geometryDefEdit.set_GridSize(0, 0.5);
                geometryDefEdit.AvgNumPoints_2 = 2;
                geometryDefEdit.HasM_2 = false;
                geometryDefEdit.HasZ_2 = true;
                if (objectWorkspace is IWorkspace)
                {
                    // If this is a STANDALONE FeatureClass then add spatial reference.
                    geometryDefEdit.SpatialReference_2 = spatialReference;
                }

                // Create OID Field
                IField fieldOID = new FieldClass();
                IFieldEdit fieldEditOID = (IFieldEdit)fieldOID;
                fieldEditOID.Name_2 = "OBJECTID";
                fieldEditOID.AliasName_2 = "OBJECTID";
                fieldEditOID.Type_2 = esriFieldType.esriFieldTypeOID;
                fieldsEdit.AddField(fieldOID);

                // Create Geometry Field
                IField fieldShape = new FieldClass();
                IFieldEdit fieldEditShape = (IFieldEdit)fieldShape;
                fieldEditShape.Name_2 = "SHAPE";
                fieldEditShape.AliasName_2 = "SHAPE";
                fieldEditShape.Type_2 = esriFieldType.esriFieldTypeGeometry;
                fieldEditShape.GeometryDef_2 = geometryDef;
                fieldsEdit.AddField(fieldShape);
            }

            // Locate Shape Field
            string stringShapeFieldName = "";
            for (int i = 0; i <= fields.FieldCount - 1; i++)
            {
                if (fields.get_Field(i).Type == esriFieldType.esriFieldTypeGeometry)
                {
                    stringShapeFieldName = fields.get_Field(i).Name;
                    break;
                }
            }
            if (stringShapeFieldName == "")
            {
                throw (new Exception("Cannot locate geometry field in FIELDS"));
            }

            IFeatureClass featureClass = null;

            if (objectWorkspace is IWorkspace)
            {
                // Create a STANDALONE FeatureClass
                IWorkspace workspace = (IWorkspace)objectWorkspace;
                IFeatureWorkspace featureWorkspace = (IFeatureWorkspace)workspace;

                featureClass = featureWorkspace.CreateFeatureClass(name, fields, uidCLSID, uidCLSEXT,
                                                                   featureType, stringShapeFieldName,
                                                                   "");
            }
            else if (objectWorkspace is IFeatureDataset)
            {
                IFeatureDataset featureDataset = (IFeatureDataset)objectWorkspace;
                featureClass = featureDataset.CreateFeatureClass(name, fields, uidCLSID, uidCLSEXT,
                                                                 featureType, stringShapeFieldName,
                                                                 "");
            }

            // Return FeatureClass
            return featureClass;
        }

        public override void OnMouseDown(int Button, int Shift, int X, int Y)
        {
            System.Diagnostics.Debug.WriteLine("");

        }

        public override void OnMouseMove(int Button, int Shift, int X, int Y)
        {
            try
            {

                var activeView = m_hookHelper.ActiveView.FocusMap as IActiveView;

                mapPoint = activeView.ScreenDisplay.DisplayTransformation.ToMapPoint(X, Y);

                if (m_hookHelper.ActiveView is PageLayout) return;

                    //mapPoint = m_hookHelper.ActiveView.
                    snapResult = null;

                //Try to snap the current position

                snapResult = Snapper.Snap(mapPoint);

                SnappingFeedback.Update(snapResult, 0);

                if (snapResult != null)
                {//Snapping occurred

                    //Set the current position to the snapped location
                    _isSnapped = true;
                    mapPoint = snapResult.Location;
                }
                else
                    _isSnapped = false;


            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }

        }

        public override void OnMouseUp(int Button, int Shift, int X, int Y)
        {
            
            if (mapPoint != null && !mapPoint.IsEmpty && Anno_DecorPointCartography_featClass != null)
            {
                ILayer _Layer = EsriUtils.getLayerByName(m_hookHelper.ActiveView.FocusMap, "AirportHeliport");
                if (_Layer == null) _Layer = EsriUtils.getLayerByName(m_hookHelper.ActiveView.FocusMap, "AirportCartography");
                if (_Layer == null) return;

                var fc = ((IFeatureLayer)_Layer).FeatureClass;
                //var workspaceEdit = (IWorkspaceEdit)fc.FeatureDataset.Workspace;

                IPoint geoPoint = (IPoint)EsriUtils.ToGeo(mapPoint, m_hookHelper.ActiveView.FocusMap, (fc as IGeoDataset).SpatialReference);

                ChartElement_BorderedText_Collout_CaptionBottom chrtEl_FreeAnno = (ChartElement_BorderedText_Collout_CaptionBottom)ChartsHelperClass.getPrototypeChartElement(SigmaDataCash.prototype_anno_lst, "FreeAnno");

                chrtEl_FreeAnno.Border.FrameMargins = new AncorFrameMargins(2, -2, 2, 2);

                chrtEl_FreeAnno.Slope = ((IActiveView)m_hookHelper.ActiveView.FocusMap).ScreenDisplay.DisplayTransformation.Rotation *-1;

                chrtEl_FreeAnno.CaptionTextLine[0][0].TextValue = "free anno";
                chrtEl_FreeAnno.CaptionTextLine[0][0].Visible = false;

                PDM.PDMObject pntFix = new PDM.PDMObject();
                pntFix.Lat = mapPoint.Y.ToString();
                pntFix.Lon = mapPoint.X.ToString();

                chrtEl_FreeAnno.TextContents[0][0].TextValue = ChartsHelperClass.MakeText(pntFix, chrtEl_FreeAnno.TextContents[0][0].DataSource, chrtEl_FreeAnno.CoordType);
                chrtEl_FreeAnno.TextContents[1][0].TextValue = ChartsHelperClass.MakeText(pntFix, chrtEl_FreeAnno.TextContents[1][0].DataSource, chrtEl_FreeAnno.CoordType);

                chrtEl_FreeAnno.Anchor = new AncorPoint(mapPoint.X, mapPoint.Y);

                IElement el = (IElement)chrtEl_FreeAnno.ConvertToIElement();
                el.Geometry = mapPoint;

                string GeoID = Guid.NewGuid().ToString();

                IFeature pFeat = Anno_DecorPointCartography_featClass.CreateFeature();
                pFeat.Shape = mapPoint;

                int fID = Anno_DecorPointCartography_featClass.FindField("type");
                if (fID >= 0) pFeat.set_Value(fID, "FreeAnno");

                fID = Anno_DecorPointCartography_featClass.FindField("FeatureGuid");
                if (fID >= 0) pFeat.set_Value(fID, GeoID);

                pFeat.Store();

                ChartElementsManipulator.StoreSingleElementToDataSet("FreeAnno", GeoID, el, ref chrtEl_FreeAnno, chrtEl_FreeAnno.Id, m_hookHelper.ActiveView.FocusMap.MapScale);

                _Layer = EsriUtils.getLayerByName(m_hookHelper.ActiveView.FocusMap, "FreeAnno");
                if (_Layer == null)
                {
                    IFeatureClass pFeatureClass = ((IFeatureWorkspace)fc.FeatureDataset.Workspace).OpenFeatureClass("FreeAnno");
                    if (pFeatureClass!=null)
                    {
                        ILayer nlayer = (ILayer)(new FDOGraphicsLayer());
                        IFeatureLayer newlayer = (IFeatureLayer)nlayer;
                        newlayer.FeatureClass = pFeatureClass;
                        nlayer.Name = "FreeAnno";

                        _Layer = EsriUtils.getLayerByName(m_hookHelper.ActiveView.FocusMap, "Annotations");
                        ((IGroupLayer)_Layer).Add(nlayer);
                    }

                    

                }
                

                ChartElementsManipulator.RefreshChart((IMxDocument)m_application.Document);
                var nd = SigmaDataCash.ChartElementsTree.Nodes.Find("FreeAnno", false);

                //if (nd == null || nd.Length <=0)
                {
                    ArenaStaticProc.BringToFrontToc((IMxDocument)m_application.Document, "ANCORTOCLayerView", true);
                }

            }


        }

        private IFeatureClass CreateAnno_DecorPointCartography_featClass()
        {
            ILayer _Layer = EsriUtils.getLayerByName(m_hookHelper.ActiveView.FocusMap, "DesignatedPointCartography");
            if (_Layer == null) return null;

            IFeatureClass fc = null;
            try
            {
                fc = ((IFeatureLayer)_Layer).FeatureClass;
                

                #region DecorPointCartography


                IFields fields = new FieldsClass();

                IFieldsEdit fieldsEdit = (IFieldsEdit)fields;

                fieldsEdit.FieldCount_2 = 4;

                IField textField = new FieldClass();
                IFieldEdit textFieldEdit = (IFieldEdit)textField;
                textFieldEdit.Length_2 = 30;
                textFieldEdit.Name_2 = "FeatureGUID";
                textFieldEdit.Type_2 = esriFieldType.esriFieldTypeString;

                IField typeField = new FieldClass();
                IFieldEdit typeFieldEdit = (IFieldEdit)typeField;
                typeFieldEdit.Length_2 = 30;
                typeFieldEdit.Name_2 = "Type";
                typeFieldEdit.Type_2 = esriFieldType.esriFieldTypeString;

                fieldsEdit.set_Field(0, fc.Fields.Field[0]);
                fieldsEdit.set_Field(1, fc.Fields.Field[1]);

                fieldsEdit.set_Field(2, textField);
                fieldsEdit.set_Field(3, typeField);

                fc.FeatureDataset.CreateFeatureClass("DecorPointCartography", fields, null, null, esriFeatureType.esriFTSimple, "SHAPE", "");

                #endregion

                #region FreeAnno




                #endregion
            }
            catch (Exception)
            {

                return fc;
            }
            

            return fc;

        }






        #endregion
    }
}
