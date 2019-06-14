using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using ANCOR.MapCore;
using Aran.PANDA.Common;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using PDM;
using EsriWorkEnvironment;
using ChartPApproachTerrain.Models;
using ESRI.ArcGIS.DataSourcesRaster;
using ESRI.ArcGIS.Framework;
using ArenaStatic;
using AranSupport;

namespace ChartPApproachTerrain
{
    public static class ChartHelperClass
    {

        public static void SelectChartTemplate(ref AxPageLayoutControl axPageLayoutControl, string TemplateFolder, ref ListBox lstBx, out double mapSize_Width, out double mapSize_Height)
        {
            axPageLayoutControl.LoadMxFile(TemplateFolder);
            lstBx.Items.Clear();
            var PageOrientation = "Portret";
            if (axPageLayoutControl.PageLayout.Page.Orientation == 2) PageOrientation = "Landscape";
            lstBx.Items.Add("Page orientation = " + PageOrientation);
            lstBx.Items.Add("Page Size = " + ToString(axPageLayoutControl.PageLayout.Page.FormID));

            esriUnits unts = axPageLayoutControl.PageLayout.Page.Units;
            axPageLayoutControl.PageLayout.Page.Units = esriUnits.esriMeters;

            //получить mapSize_Width и mapSize_Height строго в метрах
            IGraphicsContainer graphics = (IGraphicsContainer)axPageLayoutControl.PageLayout;
            IFrameElement frameElement = graphics.FindFrame(axPageLayoutControl.ActiveView.FocusMap);
            IMapFrame mapFrame = (IMapFrame)frameElement;
            IElement mapElement = (IElement)mapFrame;
            IGeometry frameGmtr = mapElement.Geometry;
            mapSize_Width = frameGmtr.Envelope.Width;
            mapSize_Height = frameGmtr.Envelope.Height;

            //поменять единицы измерения и вывести значения mapSize_Width и mapSize_Height на экран
            axPageLayoutControl.PageLayout.Page.Units = unts;

            frameElement = graphics.FindFrame(axPageLayoutControl.ActiveView.FocusMap);
            mapFrame = (IMapFrame)frameElement;
            mapElement = (IElement)mapFrame;
            frameGmtr = mapElement.Geometry;

            lstBx.Items.Add("Map Frame Width = " + Math.Round(frameGmtr.Envelope.Width, 2).ToString() + "cm");
            lstBx.Items.Add("Map Frame Height = " + Math.Round(frameGmtr.Envelope.Height, 2).ToString() + "cm");

            //lstBx.Items.Add("Page Units = " + ToString(axPageLayoutControl.PageLayout.Page.Units));
        }

        public static int StoreSingleElementToDataSet(IElement mapElem, IFeatureClass featureClass)
        {
            int res = 0;

            try
            {
                IAnnoClass pAnnoClass = (IAnnoClass)featureClass.Extension;

                IFeatureClass pClass = pAnnoClass.FeatureClass;
                IFeature pFeat = pClass.CreateFeature();
                IAnnotationFeature pAnnoFeat = (IAnnotationFeature)pFeat;
                pAnnoFeat.Annotation = mapElem;
                pAnnoFeat.LinkedFeatureID = 1;
                pFeat.Store();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                res = -1;
            }
            return res;
        }


        private static string ToString(esriPageFormID esriPageFormID)
        {
            string res;
            switch (esriPageFormID)
            {
                case esriPageFormID.esriPageFormA0:
                    res = "A0";
                    break;
                case esriPageFormID.esriPageFormA1:
                    res = "A1";
                    break;
                case esriPageFormID.esriPageFormA2:
                    res = "A2";
                    break;
                case esriPageFormID.esriPageFormA3:
                    res = "A3";
                    break;
                case esriPageFormID.esriPageFormA4:
                    res = "A4";
                    break;
                case esriPageFormID.esriPageFormA5:
                    res = "A5";
                    break;
                case esriPageFormID.esriPageFormC:
                    res = "C";
                    break;
                case esriPageFormID.esriPageFormCUSTOM:
                    res = "CUSTOM";
                    break;
                case esriPageFormID.esriPageFormD:
                    res = "D";
                    break;
                case esriPageFormID.esriPageFormE:
                    res = "E";
                    break;
                case esriPageFormID.esriPageFormLegal:
                    res = "Legal";
                    break;
                case esriPageFormID.esriPageFormLetter:
                    res = "Letter";
                    break;
                case esriPageFormID.esriPageFormSameAsPrinter:
                    res = "SameAsPrinter";
                    break;
                case esriPageFormID.esriPageFormTabloid:
                    res = "Tabloid";
                    break;
                default:
                    res = "A0";
                    break;
            }
            return res;
        }

        private static string ToString(esriUnits esriUnits)
        {
            string res;


            switch (esriUnits)
            {
                case esriUnits.esriCentimeters:
                    res = "Centimeters";
                    break;
                case esriUnits.esriDecimalDegrees:
                    res = "DecimalDegrees";
                    break;
                case esriUnits.esriDecimeters:
                    res = "Decimeters";
                    break;
                case esriUnits.esriFeet:
                    res = "Feet";
                    break;
                case esriUnits.esriInches:
                    res = "Inches";
                    break;
                case esriUnits.esriKilometers:
                    res = "Kilometers";
                    break;
                case esriUnits.esriMeters:
                    res = "Meters";
                    break;
                case esriUnits.esriMiles:
                    res = "Miles";
                    break;
                case esriUnits.esriMillimeters:
                    res = "Millimeters";
                    break;
                case esriUnits.esriNauticalMiles:
                    res = "Nautical Miles";
                    break;
                case esriUnits.esriPoints:
                    res = "Points";
                    break;
                case esriUnits.esriUnitsLast:
                    res = "Units Last";
                    break;
                case esriUnits.esriUnknownUnits:
                    res = "Unknown Units";
                    break;
                case esriUnits.esriYards:
                    res = "Yards";
                    break;
                default:
                    res = "";
                    break;
            }

            return res;
        }

        public static void SaveRunwayElement(Runway runway, List<RwyDirWrapper> rwyDirList)
        {
            IFeatureLayer layer = EsriUtils.getLayerByName(GlobalParams.Map, "RunwayCartography") as IFeatureLayer;          
            if (layer == null)
            {               
                return;
            }

            var featClass = layer.FeatureClass;

            ITable table = featClass as ITable;
            table.DeleteSearchedRows(null);

            double runwayElementWidth = 45;
            if (runway.Width.HasValue)
                runwayElementWidth = runway.Width.Value;

            double dir = rwyDirList[0].Direction;
            var rwyDir = rwyDirList[0];

            var tmpPt1 = EsriFunctions.LocalToPrj(rwyDirList[0].EndPt, dir, 0, runwayElementWidth / 2);
            var tmpPt2 = EsriFunctions.LocalToPrj(rwyDirList[0].EndPt, dir, 0, -runwayElementWidth / 2);
            var tmpPt3 = EsriFunctions.LocalToPrj(rwyDirList[1].EndPt, dir, 0, -runwayElementWidth / 2);
            var tmpPt4 = EsriFunctions.LocalToPrj(rwyDirList[1].EndPt, dir, 0, runwayElementWidth / 2);

            IPointCollection ring = new Ring();
            ring.AddPoint(tmpPt1);
            ring.AddPoint(tmpPt2);
            ring.AddPoint(tmpPt3);
            ring.AddPoint(tmpPt4);
            ring.AddPoint(tmpPt1);

            var geoPrj = new PolygonClass();
            (geoPrj as IGeometryCollection).AddGeometry((IGeometry)ring);
            EsriFunctions.SimplifyGeometry(geoPrj);

            var resultGeo = GlobalParams.SpatialRefOperation.ToEsriGeo((IGeometry)geoPrj);

            var fc = featClass.CreateFeature();

            fc.set_Value(1, resultGeo);
            fc.set_Value(2, runway.Designator);
                
            fc.Store();

        }

        public static void SaveOtherParams(AirportHeliport adhp, RwyDirWrapper selectedRwyDir, double offset)
        {
            var graphicsContainer = GlobalParams.HookHelper.PageLayout as IGraphicsContainer;

            graphicsContainer.Reset();
            var element = graphicsContainer.Next();

            while (element != null)
            {
                if (element is IElement)
                {
                    var textElement = element as ITextElement;
                    if (textElement != null)
                    {
                        IElementProperties3 elemProperties = element as IElementProperties3;
                        if (elemProperties == null) continue;

                        if (elemProperties.Name == "ElevUnit")
                        {
                            var ftM = InitChartPAT.HeightConverter.Unit == "FT"
                                ? "FEET"
                                : "METERS";
                            textElement.Text = "ELEVATIONS IN " + ftM;
                        }
                        else if (elemProperties.Name == "OtherDimension")
                        {
                            var ftm = InitChartPAT.DistanceConverter.Unit == "ft"
                                ? "FEET"
                                : "METERS";
                            textElement.Text = "ALL OTHER DIMENSIONS IN " + ftm;
                        }

                        else if (elemProperties.Name == "ElemDate")
                            textElement.Text =GlobalParams.EffectiveDate.ToString("dd MMM yyyy");
                        else if (elemProperties.Name == "ElemAMDT")
                            textElement.Text = "AIRAC AMDT " + GlobalParams.EffectiveDate.ToString("MM/yyyy");
                        else if (elemProperties.Name == "ElemAdhp")
                        {
                            textElement.Text = adhp.ServedCity + "/" + adhp.Name + ".(" + adhp.Designator+")";
                            if (selectedRwyDir != null)
                            {
                                textElement.Text += Environment.NewLine +"RUNWAY "+ selectedRwyDir.Name;
                                if (Math.Abs(offset) > 0.01)
                                    textElement.Text += " + " + offset + "°offset";
                            }
                        }

                        if (elemProperties.Name == "NOMEKLATURA")
                        {
                           
                            textElement.Text = "AD 2." + adhp.Designator + "-CHART-09";
                        }

                    }
                }
                element = graphicsContainer.Next();
            }
        }

        public static void SaveObstacles(List<VerticalStructure> obstacleList)
        {
            ILayer layer = EsriUtils.getLayerByName(GlobalParams.Map, "VertStructurePointPATC");
            if (layer == null) throw new Exception("Obstacle Point layer not found!");

            ILayer layerPolyline = EsriUtils.getLayerByName(GlobalParams.Map, "VertStructureCurvePATC");
            if (layerPolyline == null) throw new Exception("ObstacleCurve layer not found!");

            ILayer layerPolygon = EsriUtils.getLayerByName(GlobalParams.Map, "VertStructureSurfacePATC");
            if (layerPolygon == null) throw new Exception("ObstacleSurface layer not found!");

            var fc = ((IFeatureLayer)layer).FeatureClass;
            var fcPolyline = ((IFeatureLayer)layerPolyline).FeatureClass;
            var fcPolygon = ((IFeatureLayer)layerPolygon).FeatureClass;

            ITable table = fc as ITable;
            table.DeleteSearchedRows(null);

            ITable tablePolyline = fcPolyline as ITable;
            tablePolyline.DeleteSearchedRows(null);

            ITable tablePolgon = fcPolygon as ITable;
            tablePolgon.DeleteSearchedRows(null);

            int i = 0;

            foreach (var obstacle in obstacleList)
            {
                foreach (var obstaclePart in obstacle.Parts)
                {
                    i++;
                    
                    IGeometry partGeo = obstaclePart.Geo;

                    if (partGeo == null)
                        continue;
                    if (partGeo is IPoint)
                    {
                        
                        IFeature feat = fc.CreateFeature();
                        feat.set_Value(1, partGeo);
                        feat.set_Value(2, obstacle.Name);
                        feat.set_Value(3, Enum.GetName(typeof(VerticalStructureType), obstaclePart.Type));
                        feat.set_Value(5, obstaclePart.Elev_UOM.ToString());
                        feat.set_Value(6, obstaclePart.Elev.Value);
                        feat.Store();
                    }
                    else if (partGeo is IPolygon)
                    {
                        IFeature feat = fcPolygon.CreateFeature();
                        feat.set_Value(1, partGeo);
                        feat.set_Value(2, obstacle.Name);
                        feat.set_Value(3, Enum.GetName(typeof(VerticalStructureType), obstaclePart.Type));
                        feat.set_Value(5, obstaclePart.Elev_UOM.ToString());
                        feat.set_Value(6, obstaclePart.Elev.Value);
                        feat.Store();
                    }
                    else if (partGeo is IPolyline)
                    {
                        IFeature feat = fcPolyline.CreateFeature();
                        feat.Shape = partGeo;
                        feat.set_Value(2, obstacle.Name);
                        feat.set_Value(3, Enum.GetName(typeof(VerticalStructureType), obstaclePart.Type));
                        feat.set_Value(5, obstaclePart.Elev_UOM.ToString());
                        feat.set_Value(6, obstaclePart.Elev.Value);
                        feat.Store();
                    }

                }
            }
        }

        public static void SaveLightingSystems(List<string> lightSystemList, string lightID)
        {

            ILayer layer = EsriUtils.getLayerByName(GlobalParams.Map, "LightElement");
            if (layer == null) throw new Exception("LightElement layer not found!");

            
            var ids = string.Join(",",
                                lightSystemList.Select(item => "'" + item + "'").ToList());

            IFeatureLayerDefinition FD = (IFeatureLayerDefinition)layer;
            FD.DefinitionExpression = $"FeatureGUID IN ({ids})";

            
        }

        public static void SaveCenterLinePoints(RunwayCenterLinePoint cntrPoint)
        {
            ILayer layer = EsriUtils.getLayerByName(GlobalParams.Map, "RwyCenterlinePointPATC");

            if (layer == null) return;

            var fc = ((IFeatureLayer)layer).FeatureClass;
            ITable table = fc as ITable;
            table.DeleteSearchedRows(null);

            IFields fields = fc.Fields;

            IFeature feat = fc.CreateFeature();
            cntrPoint.RebuildGeo();

            IZAware zawar = cntrPoint.Geo as IZAware;
            zawar.ZAware = false;

            feat.Shape = (IPoint)cntrPoint.Geo;
            feat.set_Value(2, cntrPoint.Designator);
            feat.set_Value(3, cntrPoint.Role.ToString());
            feat.set_Value(5, cntrPoint.Elev_UOM.ToString());
            feat.set_Value(6, cntrPoint.Elev);
            
            feat.Store();

        }

        public static IGeometry CreateRectancle(IPoint refPoint, double direction, double length, double width)
        {

            var pt1 = EsriFunctions.LocalToPrj(refPoint, direction, 0, width / 2);
            var pt2 = EsriFunctions.LocalToPrj(refPoint, direction, 0, -width / 2);
            var pt3 = EsriFunctions.LocalToPrj(refPoint, direction, length, -width / 2);
            var pt4 = EsriFunctions.LocalToPrj(refPoint, direction, length, width / 2);

            var ptColl = new Ring() as IPointCollection;
            ptColl.AddPoint(pt1);
            ptColl.AddPoint(pt2);
            ptColl.AddPoint(pt3);
            ptColl.AddPoint(pt4);
            var poly = new Polygon() as IGeometryCollection;
            poly.AddGeometry((IGeometry)ptColl);
            EsriFunctions.SimplifyGeometry((IGeometry)poly);
            return GlobalParams.SpatialRefOperation.ToEsriGeo((IGeometry)poly);
        }

        public static IGeometry CreateRectancle(IPoint startPt, IPoint endPoint, double direction, double width)
        {

            var pt1 = EsriFunctions.LocalToPrj(startPt, direction, 0, width / 2);
            var pt2 = EsriFunctions.LocalToPrj(startPt, direction, 0, -width / 2);
            var pt3 = EsriFunctions.LocalToPrj(endPoint, direction, 0, -width / 2);
            var pt4 = EsriFunctions.LocalToPrj(endPoint, direction, 0, width / 2);

            var ptColl = new Ring() as IPointCollection;
            ptColl.AddPoint(pt1);
            ptColl.AddPoint(pt2);
            ptColl.AddPoint(pt3);
            ptColl.AddPoint(pt4);
            var poly = new Polygon() as IGeometryCollection;
            poly.AddGeometry((IGeometry)ptColl);
            EsriFunctions.SimplifyGeometry((IGeometry)poly);
            return GlobalParams.SpatialRefOperation.ToEsriGeo((IGeometry)poly);
        }

        public static void ChangeProjectionAndMeredian(double CMeridian, IMap pMap)
        {
            //IMap pMap = pDocument.FocusMap;

            ISpatialReferenceFactory2 pSpatRefFact = new SpatialReferenceEnvironmentClass();
            IProjectionGEN pProjection = pSpatRefFact.CreateProjection((int)esriSRProjectionType.esriSRProjection_TransverseMercator) as IProjectionGEN;

            IGeographicCoordinateSystem pGCS = pSpatRefFact.CreateGeographicCoordinateSystem((int)esriSRGeoCSType.esriSRGeoCS_WGS1984);
            ILinearUnit pLinearUnit = pSpatRefFact.CreateUnit((int)esriSRUnitType.esriSRUnit_Meter) as ILinearUnit;
            IProjectedCoordinateSystemEdit pProjCoordSysEdit = new ProjectedCoordinateSystemClass();
            IParameter[] pParams = pProjection.GetDefaultParameters();
            pParams[0].Value = 500000;
            pParams[1].Value = 0;
            pParams[2].Value = Math.Round(CMeridian, 6);
            pParams[3].Value = 0.9996;

            object name = "Transverse_Mercator";
            object alias = "UserDefinedAlias";
            object abbreviation = "Trans_Merc";
            object remarks = "ARAN coordinate system.";
            object usage = "";
            object CS = pGCS;
            object LU = pLinearUnit;
            object PRJ = pProjection;
            object PARAMS = pParams;

            pProjCoordSysEdit.Define(ref name, ref alias, ref abbreviation, ref remarks, ref usage, ref CS, ref LU, ref PRJ, ref PARAMS);

            ISpatialReference pPCS = (IProjectedCoordinateSystem)pProjCoordSysEdit; // pRJ
            if (pMap != null)
            {
                pMap.SpatialReference = pPCS;
            }

            GlobalParams.SpatialRefOperation = new SpatialReferenceOperation(pPCS);

        }

        public static IGroupElement VerticalScaleBar(double scaleBarStart, double scaleBarEnd, string position)
        {
            IGroupElement groupElement = new GroupElementClass();
            
            //ScaleBar axis
            IPoint startScaleBar = new PointClass();
            IPoint endScaleBar = new PointClass();
            if (position == "Left")
            {
                startScaleBar.PutCoords(Common.xStart, Common.yStart + scaleBarStart * 100 / Common.verScale);
                endScaleBar.PutCoords(Common.xStart, Common.yStart + scaleBarEnd * 100 / Common.verScale);
            }
            else if (position == "Right")
            {
                startScaleBar.PutCoords(Common.xStart + Common.areaWidth * 100 / Common.horScale, Common.yStart + scaleBarStart * 100 / Common.verScale);
                endScaleBar.PutCoords(Common.xStart + Common.areaWidth * 100 / Common.horScale, Common.yStart + scaleBarEnd * 100 / Common.verScale);
            }
            List<IPoint> axisPoints = new List<IPoint> {startScaleBar, endScaleBar};
            IElement axisElem = AranSupport.AnnotationUtil.GetPolylineElement_Simle(axisPoints, 0);            
            groupElement.AddElement(axisElem);

            //ScaleBar long levels
            double scaleLevel = scaleBarStart;

            while (scaleLevel <= scaleBarEnd)
            {
                IPoint longLevelStart = new PointClass();
                IPoint longLevelEnd = new PointClass();              
                if (position == "Left")
                {
                    longLevelStart.PutCoords(Common.xStart, Common.yStart + scaleLevel * 100 / Common.verScale);
                    longLevelEnd.PutCoords(Common.xStart + 8 * 100 / Common.horScale, Common.yStart + scaleLevel * 100 / Common.verScale);
                }
                else if (position == "Right")
                {
                    longLevelStart.PutCoords(Common.xStart + Common.areaWidth * 100 / Common.horScale, Common.yStart + scaleLevel * 100 / Common.verScale);
                    longLevelEnd.PutCoords(Common.xStart + (Common.areaWidth - 8) * 100 / Common.horScale, Common.yStart + scaleLevel * 100 / Common.verScale);
                }

                List<IPoint> longLevelPoints = new List<IPoint> {longLevelStart, longLevelEnd};
                IElement longLevelElem = AranSupport.AnnotationUtil.GetPolylineElement_Simle(longLevelPoints, 0);               
                groupElement.AddElement(longLevelElem);

                IElement longLvlTxtElem;
                if (position == "Left")
                {
                    longLevelStart.X -= 1 * 100 / Common.horScale;
                    longLvlTxtElem = AranSupport.AnnotationUtil.CreateFreeTextElement(longLevelStart, scaleLevel.ToString(), false, horAlligment: horizontalAlignment.Right, fontSize: 12);
                }
                else
                {
                    longLevelStart.X += 1 * 100 / Common.horScale;
                    longLvlTxtElem = AranSupport.AnnotationUtil.CreateFreeTextElement(longLevelStart, scaleLevel.ToString(), false, horAlligment: horizontalAlignment.Left, fontSize: 12);
                }                
                groupElement.AddElement(longLvlTxtElem);

                scaleLevel += 2;

            }
            //ScaleBar short levels
            scaleLevel = scaleBarStart + 1;
            while (scaleLevel <= scaleBarEnd)
            {
                IPoint smallLevelStart = new PointClass();
                IPoint smallLevelEnd = new PointClass();                
                if (position == "Left")
                {
                    smallLevelStart.PutCoords(Common.xStart, Common.yStart + scaleLevel * 100 / Common.verScale);
                    smallLevelEnd.PutCoords(Common.xStart + 4 * 100 / Common.horScale, Common.yStart + scaleLevel * 100 / Common.verScale);
                }
                else if (position == "Right")
                {
                    smallLevelStart.PutCoords(Common.xStart + Common.areaWidth * 100 / Common.horScale, Common.yStart + scaleLevel * 100 / Common.verScale);
                    smallLevelEnd.PutCoords(Common.xStart + (Common.areaWidth - 4) * 100 / Common.horScale, Common.yStart + scaleLevel * 100 / Common.verScale);
                }
                List<IPoint> smallLevelPoints = new List<IPoint> {smallLevelStart, smallLevelEnd};
                IElement smallLevelElem = AranSupport.AnnotationUtil.GetPolylineElement_Simle(smallLevelPoints, 0);
                groupElement.AddElement(smallLevelElem);
               
                scaleLevel += 2;
            }

            //Единица измерения
            
            IElement unit = AranSupport.AnnotationUtil.CreateFreeTextElement(endScaleBar, "M", false, verAlignment: verticalAlignment.Bottom, fontSize: 12, Yoffset: 8);            
            groupElement.AddElement(unit);

            return groupElement;

        }

        public static IGroupElement VerticalScaleBarFT(double scaleBarStart, double scaleBarEnd)
        {
            double shift = 1;
            IGroupElement groupElement = new GroupElementClass();            
            IPoint startScaleBar = new PointClass();
            IPoint endScaleBar = new PointClass();
            if (GlobalParams.SelectedRwyDirection.TrueBearing.Value < 180)
            {
                startScaleBar.PutCoords(Common.xStart - shift, Common.yStart + scaleBarStart * 0.3048 * 100 / Common.verScale);
                endScaleBar.PutCoords(Common.xStart - shift, Common.yStart + scaleBarEnd * 0.3048 * 100 / Common.verScale);
            }
            else
            {
                startScaleBar.PutCoords(Common.xStart + shift + Common.areaWidth * 100 / Common.horScale, Common.yStart + scaleBarStart * 0.3048 * 100 / Common.verScale);
                endScaleBar.PutCoords(Common.xStart + shift + Common.areaWidth * 100 / Common.horScale, Common.yStart + scaleBarEnd * 0.3048 * 100 / Common.verScale);
            }
            List<IPoint> axisPoints = new List<IPoint> {startScaleBar, endScaleBar};
            IElement axisElem = AranSupport.AnnotationUtil.GetPolylineElement_Simle(axisPoints, 0);
            groupElement.AddElement(axisElem);

            //ScaleBar long levels
            double scaleLevel = scaleBarStart;

            while (scaleLevel <= scaleBarEnd)
            {
                IPoint longLevelStart = new PointClass();
                IPoint longLevelEnd = new PointClass();                
                if (GlobalParams.SelectedRwyDirection.TrueBearing.Value < 180)
                {
                    longLevelStart.PutCoords(Common.xStart - shift, Common.yStart + scaleLevel * 0.3048 * 100 / Common.verScale);
                    longLevelEnd.PutCoords(Common.xStart - shift + 8 * 100 / Common.horScale, Common.yStart + scaleLevel * 0.3048 * 100 / Common.verScale);
                }
                else
                {
                    longLevelStart.PutCoords(Common.xStart + shift + Common.areaWidth * 100 / Common.horScale, Common.yStart + scaleLevel * 0.3048 * 100 / Common.verScale);
                    longLevelEnd.PutCoords(Common.xStart + shift + (Common.areaWidth - 8) * 100 / Common.horScale, Common.yStart + scaleLevel * 0.3048 * 100 / Common.verScale);
                }

                List<IPoint> longLevelPoints = new List<IPoint> {longLevelStart, longLevelEnd};
                IElement longLevelElem = AranSupport.AnnotationUtil.GetPolylineElement_Simle(longLevelPoints, 0);
                groupElement.AddElement(longLevelElem);

                IElement longLvlTxtElem;
                if (GlobalParams.SelectedRwyDirection.TrueBearing.Value >180)
                {
                    longLevelStart.X += 1 * 100 / Common.horScale;
                    longLvlTxtElem = AranSupport.AnnotationUtil.CreateFreeTextElement(longLevelStart, scaleLevel.ToString(), false, horAlligment: horizontalAlignment.Left, fontSize: 12);
                }
                else
                {
                    longLevelStart.X -= 2 * 100 / Common.horScale;
                    longLvlTxtElem = AranSupport.AnnotationUtil.CreateFreeTextElement(longLevelStart, scaleLevel.ToString(), false, horAlligment: horizontalAlignment.Right, fontSize: 12);
                }
                groupElement.AddElement(longLvlTxtElem);

                scaleLevel += 10;

            }
            //ScaleBar short levels
            scaleLevel = scaleBarStart + 5;
            while (scaleLevel <= scaleBarEnd)
            {
                IPoint smallLevelStart = new PointClass();
                IPoint smallLevelEnd = new PointClass();
                ILine smallLevelLine = new LineClass();
                if (GlobalParams.SelectedRwyDirection.TrueBearing.Value < 180)
                {
                    smallLevelStart.PutCoords(Common.xStart - shift, Common.yStart + scaleLevel * 0.3048 * 100 / Common.verScale);
                    smallLevelEnd.PutCoords(Common.xStart - shift + 4 * 100 / Common.horScale, Common.yStart + scaleLevel * 0.3048 * 100 / Common.verScale);
                }
                else
                {
                    smallLevelStart.PutCoords(Common.xStart + shift + Common.areaWidth * 100 / Common.horScale, Common.yStart + scaleLevel * 0.3048 * 100 / Common.verScale);
                    smallLevelEnd.PutCoords(Common.xStart + shift + (Common.areaWidth - 4) * 100 / Common.horScale, Common.yStart + scaleLevel * 0.3048 * 100 / Common.verScale);
                }

                List<IPoint> smallLevelPoints = new List<IPoint> {smallLevelStart, smallLevelEnd};
                IElement smallLevelElem = AranSupport.AnnotationUtil.GetPolylineElement_Simle(smallLevelPoints, 0);
                groupElement.AddElement(smallLevelElem);

                scaleLevel += 10;
            }

            //Единица измерения
            IElement unit = AranSupport.AnnotationUtil.CreateFreeTextElement(endScaleBar, "FT", false, verAlignment: verticalAlignment.Bottom, fontSize: 12, Yoffset: 8);
            groupElement.AddElement(unit);

            return groupElement;

        }

        public static IGroupElement HorizontalScaleBar(double thrX, double thrY, double thrDir, double horLevelHeight)
        {
            IGroupElement groupElement = new GroupElementClass();
            
            // Horizontal ScaleBar axis through Threshold elevation
            double axisX, axisY;           
            IPoint axisStartPoint = new PointClass();
            IPoint axisEndPoint = new PointClass();
            NativeMethods.PointAlongGeodesic(thrX, thrY, Common.areaWidth, thrDir + 180, out axisX, out axisY);
            axisStartPoint.PutCoords(Common.xStart, Common.yStart);
            axisEndPoint.PutCoords(Common.xStart + Common.areaWidth * 100 / Common.horScale, Common.yStart);
            List<IPoint> axisPoints = new List<IPoint> {axisStartPoint, axisEndPoint};

            IElement axisElem = AranSupport.AnnotationUtil.GetPolylineElement_Simle(axisPoints, 0); 
            groupElement.AddElement(axisElem);

            //Horizontal ScaleBar levels
            double scaleDistance, scaleStep;
            scaleDistance = scaleStep = 25;
            while (scaleDistance < Common.areaWidth)
            {
                IPoint startPoint = new PointClass();
                IPoint endPoint = new PointClass();
                startPoint.PutCoords(Common.xStart + scaleDistance * 100 / Common.horScale, Common.yStart);
                endPoint.PutCoords(Common.xStart + scaleDistance * 100 / Common.horScale, Common.yStart + horLevelHeight * 100 / Common.verScale);

                List<IPoint> horLvlPoints = new List<IPoint> {startPoint, endPoint};
                IElement horLvlElem = AranSupport.AnnotationUtil.GetPolylineElement_Simle(horLvlPoints, 0);
                groupElement.AddElement(horLvlElem);

                double forLblText = 0;
                if (GlobalParams.SelectedRwyDirection.TrueBearing.Value < 180)
                    forLblText = Common.areaWidth;
                //Horizontal ScaleBar Labels
                if ((scaleDistance % 100) == 0)
                {                  
                    IElement horLvlTxt = AranSupport.AnnotationUtil.CreateFreeTextElement(startPoint, Math.Abs(forLblText - scaleDistance).ToString(), false, fontSize: 12,verAlignment:verticalAlignment.Top,Yoffset:-4);

                    groupElement.AddElement(horLvlTxt);
                }

                scaleDistance += scaleStep;
            }
          
            return groupElement;

        }

        public static bool CheckContain(IRaster2 raster, IPoint thresholdPoint)
        {
            if (GlobalParams.SelectedRwyDirection == null)
                return false;

            double area4XUp, area4YUp;
            double area4XDown, area4YDown;
            double distance = 0;
            double step = NativeMethods.ReturnGeodesicDistance(raster.ToMapX(0), raster.ToMapY(0), raster.ToMapX(1), raster.ToMapY(0));
            bool check = true;

            NativeMethods.PointAlongGeodesic(thresholdPoint.X, thresholdPoint.Y, Common.areaHeight / 2 - step/2, Convert.ToDouble(GlobalParams.SelectedRwyDirection.TrueBearing.Value) + 90, out area4XUp, out area4YUp);

            NativeMethods.PointAlongGeodesic(thresholdPoint.X, thresholdPoint.Y, Common.areaHeight / 2 - step / 2, Convert.ToDouble(GlobalParams.SelectedRwyDirection.TrueBearing.Value) - 90, out area4XDown, out area4YDown);

            double stepX, stepY;
            int pixelCol, pixelRow;
            while (distance <= Common.areaWidth)
            {

                NativeMethods.PointAlongGeodesic(area4XUp, area4YUp, step, Convert.ToDouble(GlobalParams.SelectedRwyDirection.TrueBearing.Value) + 180, out stepX, out stepY);
                pixelCol = raster.ToPixelColumn(stepX);
                pixelRow = raster.ToPixelRow(stepY);
                var pointElev = raster.GetPixelValue(0, pixelCol, pixelRow);
                if (pointElev == null)
                {
                    check = false;
                    break;
                }
                NativeMethods.PointAlongGeodesic(area4XDown, area4YDown, step, Convert.ToDouble(GlobalParams.SelectedRwyDirection.TrueBearing.Value) + 180, out stepX, out stepY);
                pixelCol = raster.ToPixelColumn(stepX);
                pixelRow = raster.ToPixelRow(stepY);
                pointElev = raster.GetPixelValue(0, pixelCol, pixelRow);
                if (pointElev == null)
                {
                    check = false;
                    break;
                }

                distance += step;

            }

            double area4XRight, area4YRight;

            NativeMethods.PointAlongGeodesic(area4XUp, area4YUp, Common.areaWidth, Convert.ToDouble(GlobalParams.SelectedRwyDirection.TrueBearing.Value) + 180, out area4XRight, out area4YRight);

            while (distance <= Common.areaHeight && check)
            {

                NativeMethods.PointAlongGeodesic(area4XUp, area4YUp, step, Convert.ToDouble(GlobalParams.SelectedRwyDirection.TrueBearing.Value) - 90, out stepX, out stepY);
                pixelCol = raster.ToPixelColumn(stepX);
                pixelRow = raster.ToPixelRow(stepY);
                var pointElev = raster.GetPixelValue(0, pixelCol, pixelRow);
                if (pointElev == null)
                {
                    check = false;
                    break;
                }
                NativeMethods.PointAlongGeodesic(area4XRight, area4YRight, step, Convert.ToDouble(GlobalParams.SelectedRwyDirection.TrueBearing.Value) - 90, out stepX, out stepY);
                pixelCol = raster.ToPixelColumn(stepX);
                pixelRow = raster.ToPixelRow(stepY);
                pointElev = raster.GetPixelValue(0, pixelCol, pixelRow);
                if (pointElev == null)
                {
                    check = false;
                    break;
                }

                distance += step;

            }

            return check;

        }

        public static IGeometry GetArea4Rectangle(RunwayCenterLinePoint thrPoint, RwyDirWrapper runwayDir)
        {
            IPointCollection area4PtColl = new Polygon();
            double area4X;
            double area4Y;

            NativeMethods.PointAlongGeodesic(thrPoint.X.Value, thrPoint.Y.Value, Common.areaHeight / 2, Convert.ToDouble(runwayDir.RwyDir.TrueBearing.Value) + 90, out area4X, out area4Y);
            IPoint area4Point = new PointClass();
            area4Point.X = area4X;
            area4Point.Y = area4Y;
            area4PtColl.AddPoint(area4Point);
            NativeMethods.PointAlongGeodesic(area4X, area4Y, Common.areaWidth, Convert.ToDouble(runwayDir.RwyDir.TrueBearing.Value) + 180, out area4X, out area4Y);
            IPoint area4Point1 = new PointClass();
            area4Point1.X = area4X;
            area4Point1.Y = area4Y;
            area4PtColl.AddPoint(area4Point1);
            NativeMethods.PointAlongGeodesic(area4X, area4Y, Common.areaHeight, Convert.ToDouble(runwayDir.RwyDir.TrueBearing.Value) - 90, out area4X, out area4Y);
            IPoint area4Point2 = new PointClass();
            area4Point2.X = area4X;
            area4Point2.Y = area4Y;
            area4PtColl.AddPoint(area4Point2);
            NativeMethods.PointAlongGeodesic(area4X, area4Y, Common.areaWidth, Convert.ToDouble(runwayDir.RwyDir.TrueBearing.Value), out area4X, out area4Y);
            IPoint area4Point3 = new PointClass();
            area4Point3.X = area4X;
            area4Point3.Y = area4Y;
            area4PtColl.AddPoint(area4Point3);
            area4PtColl.AddPoint(area4Point);

            return (IGeometry)area4PtColl;

        }

        public static IRasterDataset OpenFileRasterDataset(string folderName, string datasetName)
        {
            //Open raster file workspace.
            IWorkspaceFactory workspaceFactory = new RasterWorkspaceFactoryClass();
            IRasterWorkspace rasterWorkspace = (IRasterWorkspace)workspaceFactory.OpenFromFile(folderName, 0);

            //Open file raster dataset. 
            IRasterDataset rasterDataset = rasterWorkspace.OpenRasterDataset(datasetName);
            return rasterDataset;
        }

        public static IElement CreateLineElement(params IPoint[] points)
        {
            List<IPoint> list = new List<IPoint>();
            foreach (var item in points)
            {
                list.Add(item);
            }
            return GlobalParams.UI.GetObstaclePolylineElement(list, 235);          
        }

        public static ISymbol LoadStyleSymbol()
        {
            try
            {
                var mainFolder = ArenaStaticProc.GetMainFolder();
                IStyleGallery styleGallery = new StyleGalleryClass();
                IStyleGalleryStorage styleStorage = styleGallery as IStyleGalleryStorage;
                styleStorage.TargetFile = mainFolder + @"\Model\Fonts\Aero_Sigma.style";
                styleStorage.AddFile(mainFolder + @"\Model\Fonts\Aero_Sigma.style");
                IEnumStyleGalleryItem enumStyleGalleryItem = styleGallery.Items["Line Symbols", mainFolder + @"\Model\Fonts\Aero_Sigma.style", ""];
                enumStyleGalleryItem.Reset();
                IStyleGalleryItem styleItem = enumStyleGalleryItem.Next();

                while (styleItem != null)
                {
                    if (styleItem.Name == "Light System") break;
                    styleItem = enumStyleGalleryItem.Next();
                }

                ISymbol pSymbol = styleItem.Item as ISymbol;

                return pSymbol;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Load Style, " + ex.Message);
                return null;
            }
        }

        public static void UpdateMapFrame()
        {
            IGraphicsContainer pGraphicsContainer = GlobalParams.HookHelper.PageLayout as IGraphicsContainer;
            pGraphicsContainer.Reset();
            IElement pElement = pGraphicsContainer.Next();
            while (pElement != null)
            {
                if (pElement is IMapFrame)
                {
                    var mapChartHeight = Common.areaHeight * 100 / Common.horScale;
                    var mapChartWidth = Common.areaWidth * 100 / Common.horScale;
                    IEnvelope pEnvelope = new EnvelopeClass();
                    pEnvelope.PutCoords(Common.xStart, pElement.Geometry.Envelope.YMin,
                    Common.xStart + mapChartWidth, pElement.Geometry.Envelope.YMin + mapChartHeight);
                    pElement.Geometry = pEnvelope;

                    var mapFrame = pElement as IMapFrame;

                    var activeView = GlobalParams.HookHelper.FocusMap as IActiveView;
                    if (activeView != null)
                    {
                        var spRefOperation = new SpatialReferenceOperation(GlobalParams.Map.SpatialReference);
                        var areaGeo = spRefOperation.ToEsriPrj(GlobalParams.Area4Rectangle);
                        var env = areaGeo.Envelope;

                        activeView.Extent = env;
                        GlobalParams.HookHelper.FocusMap.MapScale = Common.horScale;

                        var envel = mapFrame.MapBounds.Envelope;
                        mapFrame.MapBounds = envel;
                        mapFrame.ExtentType = esriExtentTypeEnum.esriExtentBounds;

                    }
                    break;
                }
                pElement = pGraphicsContainer.Next();
            }
            GlobalParams.HookHelper.ActiveView.Refresh();

        }

    }
}
