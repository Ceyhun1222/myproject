using System;
using ESRI.ArcGIS.Carto;
//using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Geometry;
using EsriWorkEnvironment;
using ANCOR.MapElements;
using ANCOR.MapCore;
using System.Collections.Generic;

namespace AranSupport
{
    public class AnnotationUtil
    {
        public static void CreateAnnoInfo(IMap  pMap, IPoint pnt, string infoText, bool clearGrafhics)
        {
            if (pnt == null) return;
            if (pnt.IsEmpty) return;

            IElementProperties docElementProperties;

            var sc = (pMap.MapScale * 100000) / 9000000;

            var simpleTextBorderCollout = new ChartElement_BorderedText_Collout(infoText.TrimEnd())
                                              {
                                                  Anchor = new AncorPoint(pnt.X, pnt.Y)
                                              };
            var o = simpleTextBorderCollout.ConvertToIElement();
            //AbstractChartElement acntElement = SimpleTextBorderCollout;

            var graphicsContainer = (IGraphicsContainer)pMap;


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

        public static void CreateAnnoInfo(IMap pMap, IPoint pnt, string infoText, bool clearGrafhics, double sc)
        {
            if (pnt == null) return;
            if (pnt.IsEmpty) return;

            IElementProperties docElementProperties;


            var simpleTextBorderCollout = new ChartElement_BorderedText_Collout(infoText.TrimEnd())
                                              {
                                                  Anchor = new AncorPoint(pnt.X, pnt.Y)
                                              };
            var o = simpleTextBorderCollout.ConvertToIElement();
            //AbstractChartElement acntElement = SimpleTextBorderCollout;

            var graphicsContainer = (IGraphicsContainer)pMap;


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

        public static void CreateAnnoInfo(IMap pMap, IPointCollection polygon, bool clearGrafhics, ISpatialReference spref)
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


                airspaceBuffer.Geometry = EsriUtils.ToProject(pointCollection as IGeometry, pMap, spref);


                var el = airspaceBuffer;

                var graphicsContainer = (IGraphicsContainer)pMap;

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
                System.Windows.Forms.MessageBox.Show(ex.Message);

                Console.WriteLine(ex.Message);
                //return null;
            }
        }

        public static void CreateAnnoInfo(IMap pMap, IPolyline _Line, string infoText, bool clearGrafhics)
        {

            if (_Line == null) return;
            if (_Line.IsEmpty) return;
            if (_Line.Length ==0) return;


            IElementProperties docElementProperties;

            var sc = (pMap.MapScale * 100000) / 9000000;

            var simpleText = new ChartElement_SimpleText(infoText.TrimEnd());
            simpleText.VerticalAlignment = verticalAlignment.Top;
            var o = simpleText.ConvertToIElement();


            var graphicsContainer = (IGraphicsContainer)pMap;


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

        public static void CreateAnnoInfo(IMap pMap, IPolygon polygon, bool clearGrafhics, ISpatialReference spref)
        {
            try
            {

                if (polygon == null) return;
                if (polygon.IsEmpty) return;

                var missing = Type.Missing;

                //IPointCollection4 pointCollection = new PolygonClass();
                //IPoint point = new PointClass();
                //for (int i = 0; i <= polygon.PointCount - 1; i++)
                //{
                //    point.PutCoords(polygon.Point[i].X, polygon.Point[i].Y);
                //    pointCollection.AddPoint(point, ref missing, ref missing);
                //}

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



                airspaceBuffer.Geometry = EsriUtils.ToProject(polygon as IGeometry, pMap, spref);


                var el = airspaceBuffer;

                var graphicsContainer = (IGraphicsContainer)pMap;

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

        public static void CreateAnnoInfo(IGraphicsContainer graphicsContainer, IPoint pnt, string infoText, bool clearGrafhics)
        {
            if (pnt == null) return;
            if (pnt.IsEmpty) return;

            IElementProperties docElementProperties;

            var simpleTextBorderCollout = new ChartElement_SimpleText(infoText.TrimEnd())
            {
                Anchor = new AncorPoint(0, 0),
                HorizontalAlignment = horizontalAlignment.Center,
                VerticalAlignment = verticalAlignment.Center
            };
            var o = simpleTextBorderCollout.ConvertToIElement();
            //AbstractChartElement acntElement = SimpleTextBorderCollout;

           
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
            ppp.PutCoords(pnt.X, pnt.Y);


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

        public static IElement CreateFreeTextElement(IGeometry Gm, string infoText, bool reverseTxt,bool bold = true, double fontSize = 8,
            horizontalAlignment horAlligment = horizontalAlignment.Center, verticalAlignment verAlignment = verticalAlignment.Center, 
            fillStyle FillStyle = fillStyle.fSNull, bool MultyLine = false, string FontName = "Arial", double Yoffset =0, char newline_character = ' ',
            double UseHalo = 0.1)
        {
            if (Gm == null) return null;
            if (Gm.IsEmpty) return null;

            IGeometry gmtr = Gm;

            if (reverseTxt && Gm is IPolyline)
            {
                ((IPolyline)gmtr).ReverseOrientation();

            }

            if (Gm is IPolyline && ((IPolyline)gmtr).FromPoint.X > ((IPolyline)gmtr).ToPoint.X)
            {

                ((IPolyline)gmtr).ReverseOrientation();
            }

            IElementProperties docElementProperties;

            var simpleText_el = new ChartElement_SimpleText(infoText.TrimEnd(),new AncorFont(bold, new AncorColor(0, 0, 0), false, FontName, fontSize, false))
            {
                Anchor = new AncorPoint(0, Yoffset),
                HorizontalAlignment = horAlligment,
                VerticalAlignment = verAlignment,
                FillStyle = FillStyle,
                HaloMaskSize = UseHalo,
            };

            if (MultyLine)
            {
                string[] txtLns = infoText.Split(newline_character);
                if (txtLns.Length > 0)
                {
                    /////////////////////////////////////////////////////////////////////////////////////////////
                    simpleText_el.TextContents = new List<List<AncorChartElementWord>>();

                    foreach (string _txt in txtLns)
                    {
                        List<AncorChartElementWord> txtLine = new List<AncorChartElementWord>(); // создаем строку
                        AncorChartElementWord wrd = new AncorChartElementWord(_txt);//создаем слово
                        wrd.Font.Bold = true;
                        wrd.StartSymbol = new AncorSymbol("");
                        wrd.EndSymbol = new AncorSymbol("");
                        //wrd.Morse = true;
                        txtLine.Add(wrd); // добавим его в строку

                        simpleText_el.TextContents.Add(txtLine);
                    }

                    //////////////////////////////////////////////////////////////////////////////////////////
                }
            }

            var o = simpleText_el.ConvertToIElement();


            IElement el = (IElement)o;
            if (el is IGroupElement)
            {
                var grEl = el as IGroupElement;
                for (int i = 0; i <= grEl.ElementCount - 1; i++)
                {
                    if (grEl.Element[i].Geometry.IsEmpty) grEl.Element[i].Geometry = gmtr;
                }
            }
            else el.Geometry = gmtr;

            docElementProperties = el as IElementProperties;
            docElementProperties.Name = "SIGMA" + infoText;

            //graphicsContainer.AddElement(el, 0);

            return el;

        }

 
        public static IElement GetPolylineElement_Simle(List<IPoint> points, int color, esriSimpleLineStyle style = esriSimpleLineStyle.esriSLSSolid, double lineWidth = 1)
        {
            ILineSymbol pLineSymbol = new SimpleLineSymbol();
            (pLineSymbol as ISimpleLineSymbol).Style = style;
            IRgbColor lineRgb = new RgbColor();
            lineRgb.RGB = color;

            pLineSymbol.Color = lineRgb;
            pLineSymbol.Width = lineWidth;

            IPointCollection polyLine = new ESRI.ArcGIS.Geometry.Polyline();
            foreach (var pnt in points)
            {
                polyLine.AddPoint(pnt);
            }

            IElement element = new LineElement();
            element.Geometry = (IPolyline)polyLine;
            ILineElement lineElement = element as ILineElement;
            lineElement.Symbol = pLineSymbol;

            IElementProperties docElementProperties = element as IElementProperties;
            docElementProperties.Name = "SIGMA";

            return element;
        }

        public static IElement GetPolylineElement_Cartographic(List<IPoint> points, int color, double lineWidth = 1, bool arrow = false, bool styleSolid = true)
        {
            ILineSymbol cartographicLineSymbolCls = new CartographicLineSymbolClass();
            IRgbColor lineRgb = new RgbColor();
            lineRgb.RGB = color;


            cartographicLineSymbolCls.Color = lineRgb;
            cartographicLineSymbolCls.Width = lineWidth;

            if (arrow)
            {

                IArrowMarkerSymbol pMarker = new ArrowMarkerSymbol();
                //pMarker.Length = 5;
                pMarker.Color = lineRgb;
                //pMarker.Width = 5;
                pMarker.Size = 10;
                //pMarker.XOffset = 5;

                ISimpleLineDecorationElement pLineDecElem = new SimpleLineDecorationElement();
                pLineDecElem.AddPosition((double)arrowPosition.End);
                pLineDecElem.MarkerSymbol = (IMarkerSymbol)pMarker;

                ILineProperties lineProp = (ILineProperties)cartographicLineSymbolCls;
               
                LineDecoration ld = new LineDecorationClass();
                ld.AddElement(pLineDecElem);

                lineProp.LineDecoration = ld;

                if (!styleSolid)
                {
                    ESRI.ArcGIS.Display.ITemplate templateCls = new ESRI.ArcGIS.Display.TemplateClass();
                    templateCls.Interval = 1;
                    templateCls.AddPatternElement(0, 2);
                    templateCls.AddPatternElement(2, 2);
                    templateCls.AddPatternElement(2, 2);
                    templateCls.AddPatternElement(2, 2);

                    lineProp.Template = templateCls;
                }
            }


            IPointCollection polyLine = new ESRI.ArcGIS.Geometry.Polyline();
            foreach (var pnt in points)
            {
                polyLine.AddPoint(pnt);
            }

           

            IElement element = new LineElement();
            element.Geometry = (IPolyline)polyLine;
            ILineElement lineElement = element as ILineElement;
            lineElement.Symbol = cartographicLineSymbolCls;


            IElementProperties docElementProperties = element as IElementProperties;
            docElementProperties.Name = "SIGMA";

            return element;
        }

        public static IElement GetPolygonElement(IPolygon polygon, double outLineWidth = 0.01, double MarkerSymbSize = 1, double MarkerSeparation=5, esriSimpleMarkerStyle markerFillStyle = esriSimpleMarkerStyle.esriSMSCircle, bool Random = false )
        {
            try
            {

                if (polygon == null) return null;
                if (polygon.IsEmpty) return null;


                ESRI.ArcGIS.Display.IMarkerFillSymbol _Fill = new ESRI.ArcGIS.Display.MarkerFillSymbolClass();
                _Fill.Style = Random? esriMarkerFillStyle.esriMFSRandom : esriMarkerFillStyle.esriMFSGrid;

                ISimpleMarkerSymbol simpleMarkerSymb = new SimpleMarkerSymbolClass();
                simpleMarkerSymb.Style = markerFillStyle;
                simpleMarkerSymb.Size = MarkerSymbSize;

                _Fill.MarkerSymbol = simpleMarkerSymb;
                IFillProperties fillProp = (IFillProperties)_Fill;
                fillProp.XSeparation = 3;
                fillProp.YSeparation = 3;
                ISimpleLineSymbol pSimpleLine1 = new SimpleLineSymbolClass();


                pSimpleLine1.Style = esriSimpleLineStyle.esriSLSSolid;

                IRgbColor rgbClr = new RgbColor();
                rgbClr.Red = 255;
                rgbClr.Blue = 255;
                rgbClr.Green = 255;

                pSimpleLine1.Color = rgbClr;

                pSimpleLine1.Width = outLineWidth;

                _Fill.Outline = pSimpleLine1;


                IFillShapeElement pFillShpElement1 = new PolygonElementClass();
                pFillShpElement1.Symbol = _Fill;


                var airspaceBuffer = (IElement)pFillShpElement1;
                airspaceBuffer.Geometry = polygon;

                var el = airspaceBuffer;


                IElementProperties docElementProperties;
                docElementProperties = el as IElementProperties;
                docElementProperties.Name = "SIGMA";

                return el;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
  

    }
}
