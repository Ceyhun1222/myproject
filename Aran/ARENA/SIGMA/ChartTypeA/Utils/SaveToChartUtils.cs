using ESRI.ArcGIS.Carto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ANCOR.MapElements;
using Aran.PANDA.Common;
using ChartTypeA.Models;
using EsriWorkEnvironment;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using PDM;

namespace ChartTypeA.Utils
{
    class SaveToChartUtils
    {
        const double ClearwayWidth = 180;

        public static void SaveObstacleArea(List<TakeOffClimb> takeOffArea)
        {
            ILayer layer = EsriUtils.getLayerByName(GlobalParams.Map, "ObstacleArea");
            if (layer == null) return;

            var fc = ((IFeatureLayer)layer).FeatureClass;
            ITable table = fc as ITable;
            table.DeleteSearchedRows(null);

            foreach (var takeOffClimb in takeOffArea)
            {
                if (takeOffClimb.GeoForChart != null)
                {
                    IFeature feat = fc.CreateFeature();

                    var geo = GlobalParams.SpatialRefOperation.ToEsriGeo(takeOffClimb.GeoForChart);
                    feat.set_Value(1, geo);
                    feat.set_Value(2, takeOffClimb.Name);
                    feat.set_Value(3, "OLS");
                    feat.set_Value(4, "TAKEOFF");
                    feat.Store();
                }
            }
        }

        public static void SaveRunwayElement(PDM.Runway runway, List<RwyDirWrapper> rwyDirList)
        {
            try
            {


                IFeatureLayer layer = EsriUtils.getLayerByName(GlobalParams.Map, "RwyElement") as IFeatureLayer;
                if (layer == null)
                {
                    // MessageBox.Show("RunwayElement Layer is not found!", "TypeB", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
                    return;
                }

                var featClass = layer.FeatureClass;

                ITable table = featClass as ITable;
                table.DeleteSearchedRows(null);


                double runwayElementWidth = 45;
                if (runway.Width.HasValue && !double.IsNaN(runway.Width.Value))
                    runwayElementWidth = runway.Width.Value;

                double dir = rwyDirList[0].Direction;
                var rwyDir = rwyDirList[0];

                var geoPrj = RwyDepictionUtil.GetRunwayElementWithTaxiway();

                if (geoPrj == null || geoPrj.IsEmpty)
                {

                    var tmpPt1 = EsriFunctions.LocalToPrj(rwyDirList[0].EndPt, dir, 0, runwayElementWidth / 2);
                    var tmpPt2 = EsriFunctions.LocalToPrj(rwyDirList[0].EndPt, dir, 0, -runwayElementWidth / 2);
                    var tmpPt3 = EsriFunctions.LocalToPrj(rwyDirList[1].EndPt, dir, 0, -runwayElementWidth / 2);
                    var tmpPt4 = EsriFunctions.LocalToPrj(rwyDirList[1].EndPt, dir, 0, runwayElementWidth / 2);

                    IPointCollection ring = (IPointCollection) new ESRI.ArcGIS.Geometry.Ring();
                    ring.AddPoint(tmpPt1);
                    ring.AddPoint(tmpPt2);
                    ring.AddPoint(tmpPt3);
                    ring.AddPoint(tmpPt4);
                    ring.AddPoint(tmpPt1);

                    geoPrj = new PolygonClass();
                    (geoPrj as IGeometryCollection).AddGeometry((IGeometry) ring);
                //    EsriFunctions.SimplifyGeometry((IGeometry) geoPrj);

                }

                EsriFunctions.SimplifyGeometry(geoPrj);
                var resultGeo = GlobalParams.SpatialRefOperation.ToEsriGeo((IGeometry) geoPrj);

                IZAware zAware = resultGeo as IZAware;
                zAware.ZAware = false;

                var fc = featClass.CreateFeature();
                fc.set_Value(1, resultGeo);
                fc.set_Value(2, runway.Designator);
                fc.set_Value(3, "NORM");
                fc.set_Value(4, "NORMAL");
                fc.Store();


                ILayer annoLayer = EsriUtils.getLayerByName(GlobalParams.Map, "ObstacleAnnoTypeA");
                if (annoLayer == null) return;

                var annoFc = ((IFeatureLayer) annoLayer).FeatureClass;

                var rwyElementText = rwyDir.TORA + " X " + Common.DeConvertDistance(runwayElementWidth);
                ChartElement_SimpleText rwyWidthAnno = new ChartElement_SimpleText(rwyElementText);
                rwyWidthAnno.TextContents[0][0].Font.Size = 7;
                rwyWidthAnno.TextContents[0][0].Font.Bold = false;
                rwyWidthAnno.TextContents[0][0].Font.Name = "Arial";
                rwyWidthAnno.Slope = ARANMath.RadToDeg(GlobalParams.RotateVal);

                var toraInM = Common.DeConvertDistance(rwyDir.TORA);
                var textGeo = EsriFunctions.LocalToPrj((IPoint) rwyDir.Start, dir, toraInM / 2, 0);

                IElement El_s = (IElement) rwyWidthAnno.ConvertToIElement();
                El_s.Geometry = GlobalParams.SpatialRefOperation.ToEsriGeo(textGeo);

                StoreSingleElementToDataSet(El_s, annoFc);

                string composition = "ASPHALT";
                if (runway.CodeComposition.ToUpper() == "ASPH")
                    composition = "Asphalt";
                else if (runway.CodeComposition.ToUpper() == "CONC")
                    composition = "	CONCRETE";

                ChartElement_SimpleText rwyCompositeTypeAnno = new ChartElement_SimpleText(composition);
                rwyCompositeTypeAnno.TextContents[0][0].Font.Size = 7;
                rwyCompositeTypeAnno.TextContents[0][0].Font.Bold = true;
                rwyCompositeTypeAnno.TextContents[0][0].Font.Name = "Arial";
                rwyCompositeTypeAnno.Slope = ARANMath.RadToDeg(GlobalParams.RotateVal);

                var rwyCompositeGeo = EsriFunctions.LocalToPrj((IPoint) rwyDir.Start, dir, toraInM / 3, 0);

                IElement rwyCompositeTypeAnnoElem = (IElement) rwyCompositeTypeAnno.ConvertToIElement();
                rwyCompositeTypeAnnoElem.Geometry = GlobalParams.SpatialRefOperation.ToEsriGeo(rwyCompositeGeo);

                StoreSingleElementToDataSet(rwyCompositeTypeAnnoElem, annoFc);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                MessageBox.Show(e.Message);
            }
        }

        public static void SaveClearAndStopWay(PDM.AirportHeliport adhp, PDM.Runway rwy, List<RwyDirWrapper> rwyDirList)
        {
            IFeatureLayer layer = EsriUtils.getLayerByName(GlobalParams.Map, "RwyProtectArea") as IFeatureLayer;
            //IFeatureClass featureClass= _featureWorkspace.OpenFeatureClass("Aerodrom");
            if (layer == null)
            {
                // MessageBox.Show("RunwayElement Layer is not found!", "TypeB", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
                return;
            }

            var featClass = layer.FeatureClass;

            ITable table = featClass as ITable;
            table.DeleteSearchedRows(null);

            ILayer annoLayer = EsriUtils.getLayerByName(GlobalParams.Map, "ObstacleAnnoTypeA");
            if (annoLayer == null) return;

            var annoFc = ((IFeatureLayer)annoLayer).FeatureClass;

            int side = 1;

            double magVariation = 0;
            if (adhp.MagneticVariation.HasValue)
                magVariation = adhp.MagneticVariation.Value;

            foreach (var rwyDirWrapper in rwyDirList)
            {
                var endPt = rwyDirWrapper.EndPt;


                if (rwyDirWrapper.ClearWay > 0.1)
                {
                    var clearWayInM = Common.DeConvertDistance(rwyDirWrapper.ClearWay);
                    var clearWayGeo = RectancleCreater.CreateRectancle(endPt, rwyDirWrapper.Direction, clearWayInM,
                        ClearwayWidth);

                    var clearwayFeat = featClass.CreateFeature();
                    clearwayFeat.set_Value(1, clearWayGeo);
                    clearwayFeat.set_Value(2, "CWY-" + rwyDirWrapper.Name);
                    clearwayFeat.set_Value(3, "CWY");
                    clearwayFeat.Store();


                    //Clearwya Annotation
                    ChartElement_SimpleText clearwayAnno = new ChartElement_SimpleText(rwyDirWrapper.ClearWay.ToString() + " " + InitChartTypeA.DistanceConverter.Unit + " Clearway");
                    clearwayAnno.TextContents[0][0].Font.Size = 10;
                    clearwayAnno.TextContents[0][0].Font.Bold = false;
                    clearwayAnno.TextContents[0][0].Font.Name = "Arial";
                    clearwayAnno.Slope = ARANMath.RadToDeg(GlobalParams.RotateVal);

                    var textGeo = EsriFunctions.LocalToPrj((IPoint)endPt, rwyDirWrapper.Direction, clearWayInM / 2, side * (ClearwayWidth + 50));

                    IElement El_s = (IElement)clearwayAnno.ConvertToIElement();
                    El_s.Geometry = GlobalParams.SpatialRefOperation.ToEsriGeo(textGeo);

                    StoreSingleElementToDataSet(El_s, annoFc);


                    //   var directAnnoSymbolGeo = EsriFunctions.LocalToPrj((IPoint)rwyDirWrapper.Start, rwyDirWrapper.Direction, 400, side * (clearwayWidth / 2));


                    //ChartElement_SimpleText dirAnno = new ChartElement_SimpleText("test");
                    ////dirAnno.MarkerBackGround.CharacterIndex = 36625;
                    ////dirAnno.MarkerBackGround.InnerCharacterIndex = 36625;
                    ////dirAnno.MarkerBackGround.CharacterSize = 12;
                    //dirAnno.Font.Name = "ESRI Arrowhead";
                    //dirAnno.TextContents[0][0].TextValue = "<";
                    //dirAnno.TextContents[0][0].Font.Name = "ESRI Arrowhead";
                    //dirAnno.Slope = ARANMath.RadToDeg(GlobalParams.RotateVal);
                    //IElement dirAnnoEl = (IElement)dirAnno.ConvertToIElement();
                    //dirAnnoEl.Geometry = GlobalParams.SpatialRefOperation.ToEsriGeo(directAnnoSymbolGeo);
                    //StoreSingleElementToDataSet(dirAnnoEl, annoFc);


                    var azimuthText = System.Math.Round(rwyDirWrapper.Aziumuth - magVariation) + "°" + " M";
                    //if (side == 1)
                    //    azimuthText += ">";
                    //else
                    //    azimuthText = "<"+azimuthText;

                    ChartElement_SimpleText directAnno = new ChartElement_SimpleText(azimuthText);
                    directAnno.TextContents[0][0].Font.Size = 12;
                    directAnno.TextContents[0][0].Font.Bold = true;
                    directAnno.TextContents[0][0].Font.Name = "Arial";
                    directAnno.Slope = ARANMath.RadToDeg(GlobalParams.RotateVal);

                    var directAnnoGeo = EsriFunctions.LocalToPrj((IPoint)rwyDirWrapper.Start, rwyDirWrapper.Direction, 200, side * (ClearwayWidth / 2));

                    IElement direct_el = (IElement)directAnno.ConvertToIElement();
                    direct_el.Geometry = GlobalParams.SpatialRefOperation.ToEsriGeo(directAnnoGeo);
                    //      dirAnnoEl.Geometry = GlobalParams.SpatialRefOperation.ToEsriGeo(directAnnoGeo);

                    //IGroupElement grElement =new GroupElementClass();
                    //grElement.AddElement(direct_el);
                    //grElement.AddElement(dirAnnoEl);

                    StoreSingleElementToDataSet(direct_el, annoFc);
                    // return GetHandle(pCommonElement);
                }

                double stopwayWidth = 40;
                if (rwyDirWrapper.StopWay > 0.1)
                {
                    var stopwayInM = Common.DeConvertDistance(rwyDirWrapper.StopWay);
                    var stopWayGeo =RectancleCreater.CreateRectancle(endPt, rwyDirWrapper.Direction, stopwayInM, stopwayWidth);
                    var stopWayFeat = featClass.CreateFeature();
                    stopWayFeat.set_Value(1, stopWayGeo);
                    stopWayFeat.set_Value(2, "SWY-" + rwyDirWrapper.Name);
                    stopWayFeat.set_Value(3, "STOPWAY");
                    stopWayFeat.Store();

                    ChartElement_SimpleText stopwayAnno = new ChartElement_SimpleText(rwyDirWrapper.StopWay.ToString() + " " + InitChartTypeA.DistanceConverter.Unit + " Stopway");
                    stopwayAnno.TextContents[0][0].Font.Size = 10;
                    stopwayAnno.TextContents[0][0].Font.Bold = false;
                    stopwayAnno.TextContents[0][0].Font.Name = "Arial";
                    stopwayAnno.Slope = ARANMath.RadToDeg(GlobalParams.RotateVal);

                    var textGeo = EsriFunctions.LocalToPrj((IPoint)endPt, rwyDirWrapper.Direction, stopwayInM / 2, side * (stopwayWidth / 2 + 100));

                    IElement El_s = (IElement)stopwayAnno.ConvertToIElement();
                    El_s.Geometry = GlobalParams.SpatialRefOperation.ToEsriGeo(textGeo);

                    StoreSingleElementToDataSet(El_s, annoFc);
                }
                side = -1;

            }


        }

        public static void SaveStrip(PDM.Runway rwy, List<RwyDirWrapper> rwyDirList)
        {
            IFeatureLayer layer = EsriUtils.getLayerByName(GlobalParams.Map, "RwyProtectArea") as IFeatureLayer;
            //IFeatureClass featureClass= _featureWorkspace.OpenFeatureClass("Aerodrom");
            if (layer == null)
            {
                // MessageBox.Show("RunwayElement Layer is not found!", "TypeB", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
                return;
            }

            var featClass = layer.FeatureClass;

            var list = new List<IPoint>();
            foreach (var rwyDirWrapper in rwyDirList)
            {
                var distanceFromThreshold = 60;
                double minDistanceFromThreshold = Common.DeConvertDistance(rwyDirWrapper.ClearWay);

                if (distanceFromThreshold < minDistanceFromThreshold)
                    minDistanceFromThreshold = distanceFromThreshold;

                var pt1 = EsriFunctions.LocalToPrj(rwyDirWrapper.EndPt, rwyDirWrapper.Direction, minDistanceFromThreshold, 0);
                list.Add(pt1);
            }

            double stripWidth = 300;
            var firstRwyPt = rwyDirList[0];
            var stripGeo =RectancleCreater.CreateRectancle(list[0], list[1], firstRwyPt.Direction, stripWidth);

            //Load taxiway element and find intersection with rwyprotect area aixm features 

            //var stripPrj =GlobalParams.SpatialRefOperation.ToEsriPrj(stripGeo);

            //var differ = EsriFunctions.Difference(taxiwayResult, stripPrj);

            //var resultStripPrj =EsriFunctions.Union(stripPrj, taxiwayResult);
            //if (differ != null)
            //    resultStripPrj =EsriFunctions.Difference(resultStripPrj, differ);
            //resultStripPrj = EsriFunctions.Intersect(resultStripPrj, stripPrj);
            //var resultStripGeo = GlobalParams.SpatialRefOperation.ToEsriGeo(taxiwayResult);

            //


            var stripFeat = featClass.CreateFeature();
            stripFeat.set_Value(1, stripGeo);
            stripFeat.set_Value(2, "Strip-" + rwy.Designator);
            stripFeat.set_Value(3, "OTHER_RUNWAY_STRIP");
            stripFeat.Store();

        }

        public static void SaveObstacles(List<ObstacleReport> obstacleList)
        {
            ILayer layer = EsriUtils.getLayerByName(GlobalParams.Map, "VerticalStructurePoint");
            if (layer == null) throw new Exception("Obstacle Point layer not found!");

            ILayer layerPolyline = EsriUtils.getLayerByName(GlobalParams.Map, "VertStructureCurve");
            if (layerPolyline == null) throw new Exception("ObstacleCurve layer not found!");

            ILayer layerPolygon = EsriUtils.getLayerByName(GlobalParams.Map, "VertStructureSurface");
            if (layerPolygon == null) throw new Exception("ObstacleSurface layer not found!");

            var fc = ((IFeatureLayer)layer).FeatureClass;
            var fcPolyline = ((IFeatureLayer)layerPolyline).FeatureClass;
            var fcPolygon = ((IFeatureLayer)layerPolygon).FeatureClass;

            ILayer annoLayer = EsriUtils.getLayerByName(GlobalParams.Map, "ObstacleAnnoTypeA");
            if (annoLayer == null) return;

            var annoFc = ((IFeatureLayer)annoLayer).FeatureClass;
            //ITable annoTable = annoFc as ITable;
            //annoTable.DeleteSearchedRows(null);


            ITable table = fc as ITable;
            table.DeleteSearchedRows(null);

            ITable tablePolyline = fcPolyline as ITable;
            tablePolyline.DeleteSearchedRows(null);

            ITable tablePolgon = fcPolygon as ITable;
            tablePolgon.DeleteSearchedRows(null);

            var graphicsContainer = GlobalParams.HookHelper.PageLayout as IGraphicsContainer;
            graphicsContainer.Reset();
            var element = graphicsContainer.Next();

            ITextSymbol symbol = null;
            while (element != null)
            {
                IElementProperties3 elemproperties = element as IElementProperties3;
                if (elemproperties.Name == "ObstacleVerSymbol")
                {
                    var tmpElement = element as ITextElement;
                    if (tmpElement != null)
                    {
                        symbol = tmpElement.Symbol;

                    }
                }
                element = graphicsContainer.Next();
            }

            if (symbol != null)
                symbol.Angle = ARANMath.RadToDeg(GlobalParams.RotateVal);

            int i = 0;

            foreach (var obstacle in obstacleList)
            {
                i++;
                IPoint geoForAnnoPrj = null;
                if (obstacle.GeomPrj.GeometryType == esriGeometryType.esriGeometryPoint)
                {
                    IFeature feat = fc.CreateFeature();

                    var geo = GlobalParams.SpatialRefOperation.ToEsriGeo(obstacle.GeomPrj);
                    geoForAnnoPrj = obstacle.GeomPrj as IPoint;

                    IZAware zawar = geo as IZAware;
                    zawar.ZAware = false;

                    feat.set_Value(1, geo);
                    feat.set_Value(2, obstacle.Name);
                    feat.set_Value(3, obstacle.Obstacle.Type);
                    feat.set_Value(5, InitChartTypeA.HeightConverter.Unit);// GlobalParams.HeightUnitConverter.HeightUnit.ToString());
                    feat.set_Value(6, obstacle.Elevation);
                    feat.Store();
                }
                else if (obstacle.GeomPrj.GeometryType == esriGeometryType.esriGeometryPolygon)
                {
                    IFeature feat = fcPolygon.CreateFeature();

                    var geo = GlobalParams.SpatialRefOperation.ToEsriGeo(obstacle.GeomPrj);

                    IArea area = obstacle.GeomPrj as IArea;
                    if (area == null) return;
                    geoForAnnoPrj = area.Centroid;

                    IZAware zawar = geo as IZAware;
                    zawar.ZAware = false;

                    feat.set_Value(1, geo);
                    feat.set_Value(2, obstacle.Name);
                    feat.set_Value(3, obstacle.Obstacle.Type);
                    feat.set_Value(5, InitChartTypeA.HeightConverter.Unit);// GlobalParams.HeightUnitConverter.HeightUnit.ToString());
                    feat.set_Value(6, obstacle.Elevation);
                    feat.Store();
                }
                else if (obstacle.GeomPrj.GeometryType == esriGeometryType.esriGeometryPolyline)
                {
                    IFeature feat = fcPolyline.CreateFeature();

                    var geo = GlobalParams.SpatialRefOperation.ToEsriGeo(obstacle.GeomPrj);

                    geoForAnnoPrj = (obstacle.GeomPrj as IPointCollection).Point[0];

                    IZAware zawar = geo as IZAware;
                    zawar.ZAware = false;

                    feat.set_Value(1, geo);
                    feat.set_Value(2, obstacle.Name);
                    feat.set_Value(3, obstacle.Obstacle.Type);
                    feat.set_Value(5, InitChartTypeA.HeightConverter.Unit);// GlobalParams.HeightUnitConverter.HeightUnit.ToString());
                    feat.set_Value(6, obstacle.Elevation);
                    feat.Store();
                }

                #region Anno
                ChartElement_SimpleText simpleAnno = new ChartElement_SimpleText(Math.Round(obstacle.Elevation).ToString());
                simpleAnno.TextContents[0][0].Font.Size = 6;
                simpleAnno.TextContents[0][0].Font.Bold = false;
                simpleAnno.TextContents[0][0].Font.Name = "Arial";
                simpleAnno.Slope = ARANMath.RadToDeg(GlobalParams.RotateVal);


                var textGeo = EsriFunctions.LocalToPrj(geoForAnnoPrj, GlobalParams.RotateVal, 0, 70);

            
                // simpleAnno.Anchor = new AncorPoint(((IPoint)geo).X, ((IPoint)geo).Y);
                IElement El_s = (IElement)simpleAnno.ConvertToIElement();
                El_s.Geometry = GlobalParams.SpatialRefOperation.ToEsriGeo(textGeo);
                StoreSingleElementToDataSet(El_s, annoFc);

                #endregion

                #region Circle Anno
                var geoForCircleAnno = GlobalParams.SpatialRefOperation.ToEsriGeo(geoForAnnoPrj);

                ChartElement_MarkerSymbol CircleAnno = new ChartElement_MarkerSymbol((i).ToString());
                CircleAnno.MarkerBackGround.CharacterIndex = 106;
                CircleAnno.MarkerBackGround.InnerCharacterIndex = 106;
                CircleAnno.MarkerBackGround.CharacterSize = 20;
                CircleAnno.Slope = ARANMath.RadToDeg(GlobalParams.RotateVal);
                IElement El = (IElement)CircleAnno.ConvertToIElement();

                

                if (symbol != null)
                {
                    ITextElement circleTextElement = new TextElementClass();
                    circleTextElement.Symbol = symbol;
                    circleTextElement.Text = i.ToString();
                    circleTextElement.Symbol.Angle = GlobalParams.RotateVal;
                    circleTextElement.Symbol.Text = i.ToString();
                    IElement circleElem = circleTextElement as IElement;
                    circleElem.Geometry = geoForCircleAnno;
                    StoreSingleElementToDataSet(circleElem, annoFc);
                }
                else
                {


                    if (El is IGroupElement)
                    {
                        IGroupElement GrEl = El as IGroupElement;
                        for (int j = 0; j <= GrEl.ElementCount - 1; j++)
                            GrEl.get_Element(j).Geometry = geoForCircleAnno;

                        GrEl.AddElement(El_s);
                    }
                    else
                        El.Geometry = geoForCircleAnno;

                    StoreSingleElementToDataSet(El, annoFc);
                }



                #endregion
            }
        }

        public static void SaveCenterLinePoints(List<RunwayCenterLinePoint> cntrList)
        {
            ILayer layer = EsriUtils.getLayerByName(GlobalParams.Map, "RwyPoint");
            if (layer == null) return;

            var fc = ((IFeatureLayer)layer).FeatureClass;
            ITable table = fc as ITable;
            table.DeleteSearchedRows(null);

            foreach (var cntr in cntrList)
            {
                IFeature feat = fc.CreateFeature();
                cntr.RebuildGeo();

                IZAware zawar = cntr.Geo as IZAware;
                zawar.ZAware = false;

                feat.Shape = (IPoint)cntr.Geo;
                feat.set_Value(2, cntr.Designator);
                feat.set_Value(3, cntr.Role.ToString());
                feat.set_Value(5, cntr.Elev_UOM.ToString());
                feat.set_Value(6, cntr.Elev);
                feat.Store();
            }
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
    }
}
