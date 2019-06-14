using ANCOR.MapElements;
using ArenaStatic;
using ESRI.ArcGIS.ArcMapUI;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using EsriWorkEnvironment;
using SigmaChart;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;


namespace ChartPApproachTerrain
{
    public class PrecisionApproachChartClass : AbstaractSigmaChart
    {
        private IMap FocusMap = null;
        private ISpatialReference pSpatialReference = null;

        private IFeatureClass Anno_IsoLinesGeo_featClass = null;
        public string PName = "";
        //private IFeatureClass Anno_DesignatedGeo_featClass = null;
        //private IFeatureClass Anno_NavaidGeo_featClass = null;
        //private IFeatureClass Anno_ObstacleGeo_featClass = null;
        //private IFeatureClass AnnoAirspaceGeo_featClass = null;
        //private IFeatureClass AnnoRWYGeo_featClass = null;
        //private IFeatureClass AnnoFacilitymakeUpGeo_featClass = null;
        //private IFeatureClass Anno_HoldingGeo_featClass = null;
        //private IFeatureClass Anno_GeoBorder_Geo_featClass = null;
        //private IFeatureClass Anno_Airport_Geo_featClass = null;

        public PrecisionApproachChartClass()
        {
        }

        public override void CreateChart()
        {
            
            double AnnoScale = SigmaHookHelper.ActiveView.FocusMap.MapScale;
            FocusMap = SigmaHookHelper.ActiveView.FocusMap;

            var pathToTemplateFileXML = System.IO.Path.Combine(ArenaStaticProc.GetPathToTemplate() + @"\PATC\", "patc.sce");
            SigmaDataCash.SigmaChartType = (int)SigmaChartTypes.PATChart_Type;

            if (!File.Exists(pathToTemplateFileXML)) { MessageBox.Show("File not exist!"); return; }

            SigmaDataCash.environmentWorkspaceEdit = InitEnvironment_Workspace(pathToTemplateFileXML, FocusMap);

            SigmaDataCash.environmentWorkspaceEdit.StartEditing(false);
            SigmaDataCash.environmentWorkspaceEdit.StartEditOperation();


            string qry = Anno_IsoLinesGeo_featClass.OIDFieldName + " >= 0";
            IQueryFilter featFilter = new QueryFilterClass();

            featFilter.WhereClause = qry;

            IFeatureCursor featCur = Anno_IsoLinesGeo_featClass.Search(featFilter, false);

            IFeature _Feature = null;
            while ((_Feature = featCur.NextFeature()) != null)
            {
                ChartElement_SimpleText chrtEl_isigonalLine = (ChartElement_SimpleText)ChartsHelperClass.getPrototypeChartElement(SigmaDataCash.prototype_anno_lst, "IsogonalLines");
                chrtEl_isigonalLine.Slope = 0;//slope;
                chrtEl_isigonalLine.LinckedGeoId = _Feature.Value[_Feature.Fields.FindField("FeatureGUID")].ToString();
                chrtEl_isigonalLine.TextContents[0][0].TextValue = _Feature.Value[_Feature.Fields.FindField("elevValue")].ToString();

                chrtEl_isigonalLine.Slope = (GlobalParams.RotateVal * 180) / Math.PI * -1;

                int PN = ((_Feature.Shape as IPointCollection).PointCount / 2);
                IGeometry gm = (_Feature.Shape as IPointCollection).Point[PN];//_Feature.Shape;

                IElement el = (IElement)chrtEl_isigonalLine.ConvertToIElement();
                el.Geometry = gm;
                ChartElementsManipulator.StoreSingleElementToDataSet(chrtEl_isigonalLine.Name, chrtEl_isigonalLine.LinckedGeoId, el, ref chrtEl_isigonalLine, chrtEl_isigonalLine.Id, FocusMap.MapScale);
            }

            Marshal.ReleaseComObject(featCur);

           

            SigmaDataCash.environmentWorkspaceEdit.StopEditOperation();
            SigmaDataCash.environmentWorkspaceEdit.StopEditing(true);



            ChartsHelperClass.ChartsFinalisation(SigmaApplication, FocusMap, SigmaDataCash.SigmaChartType, PName);
            ChartElementsManipulator.RefreshChart((IMxDocument)SigmaApplication.Document);


        }

        public override IWorkspaceEdit InitEnvironment_Workspace(string pathToTemplateFileXML, IMap ActiveMap)
        {
            SigmaDataCash.GetProtoTypeAnnotation(pathToTemplateFileXML);

            SigmaDataCash.ChartElementList = new List<object>();
            SigmaDataCash.AnnotationFeatureClassList = null;

            ILayer _Layer = EsriUtils.getLayerByName(ActiveMap, "AirportHeliport") ?? EsriUtils.getLayerByName(ActiveMap, "AirportCartography");

            var fc = ((IFeatureLayer)_Layer).FeatureClass;
            var workspaceEdit = (IWorkspaceEdit)fc.FeatureDataset.Workspace;
            if (SigmaDataCash.AnnotationFeatureClassList == null || SigmaDataCash.AnnotationFeatureClassList.Count == 0)
                ChartElementsManipulator.LoadAnnotation_LinkedGeomFeatureClasses((int)SigmaChartTypes.PATChart_Type, workspaceEdit, (int)ActiveMap.MapScale);

            Application.DoEvents();


            try
            {
                if (SigmaDataCash.AnnotationLinkedGeometryList.ContainsKey("IsogonalLinesCartography")) Anno_IsoLinesGeo_featClass = SigmaDataCash.AnnotationLinkedGeometryList["IsogonalLinesCartography"];
                else
                {
                    ChartElementsManipulator.LoadAnnotation_LinkedGeomFeatureClasses((int)SigmaChartTypes.PATChart_Type, workspaceEdit, (int)ActiveMap.MapScale);
                    Anno_IsoLinesGeo_featClass = SigmaDataCash.AnnotationLinkedGeometryList["IsogonalLinesCartography"];
                }

                
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
            }
            return workspaceEdit;
        }
    }
}
