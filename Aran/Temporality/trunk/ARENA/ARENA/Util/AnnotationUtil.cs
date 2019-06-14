using System;
using Accent.MapCore;
using Accent.MapElements;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Geometry;
using EsriWorkEnvironment;

namespace ARENA.Util
{
    class AnnotationUtil
    {
        public static void CreateAnnoInfo(AxMapControl axMapControl1, IPoint pnt, string infoText, bool clearGrafhics)
        {
            if (pnt == null) return;
            if (pnt.IsEmpty) return;

            IElementProperties docElementProperties;

            var sc = (axMapControl1.Map.MapScale * 100000) / 9000000;

            var simpleTextBorderCollout = new ChartElement_BorderedText_Collout(infoText.TrimEnd())
                                              {
                                                  Shift = new AcntPoint(pnt.X, pnt.Y)
                                              };
            var o = simpleTextBorderCollout.ConvertToIElement();
            //AbstractChartElement acntElement = SimpleTextBorderCollout;

            var graphicsContainer = (IGraphicsContainer)axMapControl1.Map;


            if (clearGrafhics)
            {
                graphicsContainer.Reset();

                var thisElement = graphicsContainer.Next();
                while (thisElement != null)
                {
                    docElementProperties = thisElement as IElementProperties;
                    if (docElementProperties.Name.StartsWith("ARENA"))
                    {
                        graphicsContainer.DeleteElement(thisElement);
                    }
                    thisElement = graphicsContainer.Next();
                }
            }


            IPoint ppp = new PointClass();
            ppp.PutCoords(pnt.X + sc, pnt.Y + sc);


            var el = (IElement)o;

            if (el is IGroupElement)
            {
                var grEl = el as IGroupElement;
                for (int i = 0; i <= grEl.ElementCount - 1; i++)
                {
                    if (grEl.Element[i].Geometry.IsEmpty) grEl.Element[i].Geometry = pnt;
                }
            }
            else el.Geometry = ppp;

            docElementProperties = el as IElementProperties;
            docElementProperties.Name = "ARENA_" + infoText;

            graphicsContainer.AddElement(el, 0);



        }

        public static void CreateAnnoInfo(AxMapControl axMapControl1, IPoint pnt, string infoText, bool clearGrafhics, double sc)
        {
            if (pnt == null) return;
            if (pnt.IsEmpty) return;

            IElementProperties docElementProperties;


            var simpleTextBorderCollout = new ChartElement_BorderedText_Collout(infoText.TrimEnd())
                                              {
                                                  Shift = new AcntPoint(pnt.X, pnt.Y)
                                              };
            var o = simpleTextBorderCollout.ConvertToIElement();
            //AbstractChartElement acntElement = SimpleTextBorderCollout;

            var graphicsContainer = (IGraphicsContainer)axMapControl1.Map;


            if (clearGrafhics)
            {
                graphicsContainer.Reset();

                var thisElement = graphicsContainer.Next();
                while (thisElement != null)
                {
                    docElementProperties = thisElement as IElementProperties;
                    if (docElementProperties.Name.StartsWith("ARENA"))
                    {
                        graphicsContainer.DeleteElement(thisElement);
                    }
                    thisElement = graphicsContainer.Next();
                }
            }


            IPoint ppp = new PointClass();
            ppp.PutCoords(pnt.X + sc, pnt.Y + sc);


            var el = (IElement)o;

            if (el is IGroupElement)
            {
                var grEl = el as IGroupElement;
                for (var i = 0; i <= grEl.ElementCount - 1; i++)
                {
                    if (grEl.Element[i].Geometry.IsEmpty) grEl.Element[i].Geometry = pnt;
                }
            }
            else el.Geometry = ppp;

            docElementProperties = el as IElementProperties;
            docElementProperties.Name = "ARENA_" + infoText;

            graphicsContainer.AddElement(el, 0);



        }

        public static void CreateAnnoInfo(AxMapControl axMapControl1, IPointCollection polygon, bool clearGrafhics, ISpatialReference spref)
        {
            try
            {

                if (polygon == null) return;
                if (polygon.PointCount ==0) return;

                var missing = Type.Missing;

                IPointCollection4 pointCollection = new PolygonClass();
                IPoint point = new PointClass();
                for (int i = 0; i <= polygon.PointCount - 1; i++)
                {
                    point.PutCoords(polygon.Point[i].X, polygon.Point[i].Y);
                    pointCollection.AddPoint(point, ref missing, ref missing);
                }

                ISimpleFillSymbol smplFill1 = new SimpleFillSymbol();

                IRgbColor rgbClr1 = new RgbColor();
                rgbClr1.Red = 255;
                rgbClr1.Blue = 30;
                rgbClr1.Green = 30;

                smplFill1.Color = rgbClr1;

                //стиль заполнения
                smplFill1.Style = esriSimpleFillStyle.esriSFSBackwardDiagonal;
                //smplFill1.Style = esriSimpleFillStyle.esriSFSCross;
                //smplFill1.Style = esriSimpleFillStyle.esriSFSDiagonalCross;
                //smplFill1.Style = esriSimpleFillStyle.esriSFSForwardDiagonal;
                //smplFill1.Style = esriSimpleFillStyle.esriSFSHollow;
                //smplFill1.Style = esriSimpleFillStyle.esriSFSHorizontal;
                //smplFill1.Style = esriSimpleFillStyle.esriSFSNull;
                //smplFill1.Style = esriSimpleFillStyle.esriSFSSolid;
                //smplFill1.Style = esriSimpleFillStyle.esriSFSVertical;

                ISimpleLineSymbol pSimpleLine1 = new SimpleLineSymbolClass();


                pSimpleLine1.Style = esriSimpleLineStyle.esriSLSSolid;

                IRgbColor rgbClr = new RgbColor();
                rgbClr.Red = 255;
                rgbClr.Blue = 255;
                rgbClr.Green = 255;

                pSimpleLine1.Color = rgbClr;

                pSimpleLine1.Width = 0.01;

                smplFill1.Outline = pSimpleLine1;


                IFillShapeElement pFillShpElement1 = new PolygonElementClass();
                pFillShpElement1.Symbol = smplFill1;


                var airspaceBuffer = (IElement)pFillShpElement1;



                airspaceBuffer.Geometry = EsriUtils.ToProject(pointCollection as IGeometry, axMapControl1.Map, spref);


                var el = airspaceBuffer;

                var graphicsContainer = (IGraphicsContainer)axMapControl1.Map;

                IElementProperties docElementProperties;

                if (clearGrafhics)
                {
                    
                    graphicsContainer.Reset();

                    var thisElement = graphicsContainer.Next();
                    while (thisElement != null)
                    {
                        docElementProperties = thisElement as IElementProperties;
                        if (docElementProperties.Name.StartsWith("ARENA"))
                        {
                            graphicsContainer.DeleteElement(thisElement);
                        }
                        thisElement = graphicsContainer.Next();
                    }

                   
                }
                docElementProperties = el as IElementProperties;
                docElementProperties.Name = "ARENA_Airspace" + Guid.NewGuid();

                graphicsContainer.AddElement(el, 0);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                //return null;
            }
        }

        public static void CreateAnnoInfo(AxMapControl axMapControl1, IPolyline _Line, string infoText, bool clearGrafhics)
        {

            if (_Line == null) return;
            if (_Line.IsEmpty) return;
            if (_Line.Length ==0) return;


            IElementProperties docElementProperties;

            var sc = (axMapControl1.Map.MapScale * 100000) / 9000000;

            var simpleText = new ChartElement_SimpleText(infoText.TrimEnd());
            simpleText.VerticalAlignment = verticalAlignment.Top;
            var o = simpleText.ConvertToIElement();


            var graphicsContainer = (IGraphicsContainer)axMapControl1.Map;


            if (clearGrafhics)
            {
                graphicsContainer.Reset();

                var thisElement = graphicsContainer.Next();
                while (thisElement != null)
                {
                    docElementProperties = thisElement as IElementProperties;
                    if (docElementProperties.Name.StartsWith("ARENA"))
                    {
                        graphicsContainer.DeleteElement(thisElement);
                    }
                    thisElement = graphicsContainer.Next();
                }
            }


            IPolyline ln = new PolylineClass();

            ln.FromPoint = _Line.FromPoint;
            ln.ToPoint = _Line.ToPoint;

            if (_Line.FromPoint.X > _Line.ToPoint.X)
            {
                ln.FromPoint = _Line.ToPoint;
                ln.ToPoint = _Line.FromPoint;
            }


            var el = (IElement)o;

            if (el is IGroupElement)
            {
                var grEl = el as IGroupElement;
                for (int i = 0; i <= grEl.ElementCount - 1; i++)
                {
                    if (grEl.Element[i].Geometry.IsEmpty) grEl.Element[i].Geometry = ln;
                }
            }
            else el.Geometry = ln;

            docElementProperties = el as IElementProperties;
            docElementProperties.Name = "ARENA_" + infoText;

            graphicsContainer.AddElement(el, 0);



        }

    }
}
