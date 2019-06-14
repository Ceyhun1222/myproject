using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aran.AranEnvironment;
using Aran.AranEnvironment.Symbols;
using Aran.Geometries;
using Aran.Geometries.Operators;
using Aran.Geometries.SpatialReferences;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Display;
using Aran.Converters;

namespace Aran.Omega.TypeBEsri.Models
{
    public class AranGraphics : IAranGraphics
    {
        public AranGraphics()
        {
            _drawedElements = new Dictionary<int, IElement>();
            _index = 0;
            _spatialRefConverter = new SpatRefConverter();
        }

        public int DrawPoint(Aran.Geometries.Point point, Aran.AranEnvironment.Symbols.PointSymbol symbol, Boolean isVisible = true, Boolean isLocked = true)
        {
            IPoint esriPoint = ConvertToEsriGeom.FromPoint(point);
            ISimpleMarkerSymbol pMarkerSym = null;
            IMarkerElement pMarkerShpElement = ((ESRI.ArcGIS.Carto.IMarkerElement)(new MarkerElement()));
            IElement pElementofpPoint = ((ESRI.ArcGIS.Carto.IElement)(pMarkerShpElement));

            pElementofpPoint.Geometry = esriPoint;

            IRgbColor pRGB = new RgbColor();
            pRGB.RGB = symbol.Color;

            pMarkerSym = new SimpleMarkerSymbol();
            pMarkerSym.Color = pRGB;
            pMarkerSym.Size = symbol.Size;
            pMarkerSym.Style = (esriSimpleMarkerStyle)symbol.Style;

            pMarkerShpElement.Symbol = pMarkerSym;

            if (isVisible)
            {
                IGraphicsContainer pGraphics = GlobalParams.ActiveView.GraphicsContainer;
                pGraphics.AddElement(pElementofpPoint, 0);
                
                if (isLocked)
                    pElementofpPoint.Locked = true;
                GlobalParams.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);
            }
            return GetHandle(pElementofpPoint);
        }

        public int DrawPoint(Aran.Geometries.Point point, int color, Boolean isVisible = true, Boolean isLocked = true)
        {
            PointSymbol pointSymbol = new Aran.AranEnvironment.Symbols.PointSymbol();
            if (color < 0)
            {
                Random rnd = new Random();
                color = rnd.Next(256) | (rnd.Next(256) << 8) | (rnd.Next(256) << 16);
            }

            pointSymbol.Color = color;
            //pointSymbol.Size = 8;
            pointSymbol.Style = ePointStyle.smsCircle;
            return DrawPoint(point, pointSymbol, isVisible, isLocked);
        }

        public int DrawPoint(Aran.Geometries.Point point, int color, Aran.AranEnvironment.Symbols.ePointStyle style, Boolean isVisible = true, Boolean isLocked = true)
        {
            PointSymbol pointSymbol = new Aran.AranEnvironment.Symbols.PointSymbol();
            if (color < 0)
            {
                Random rnd = new Random();
                color = rnd.Next(256) | (rnd.Next(256) << 8) | (rnd.Next(256) << 16);
            }

            pointSymbol.Color = color;
            pointSymbol.Size = 8;
            pointSymbol.Style = style;
            return DrawPoint(point, pointSymbol, isVisible, isLocked);
        }

        public int DrawPointWithText(Aran.Geometries.Point point, PointSymbol symbol, string text, Boolean isVisible = true, Boolean isLocked = true)
        {
            IPoint esriPoint = Aran.Converters.ConvertToEsriGeom.FromPoint(point);
            ITextElement pTextElement = ((ESRI.ArcGIS.Carto.ITextElement)(new TextElement()));
            IElement pElementOfText = ((ESRI.ArcGIS.Carto.IElement)(pTextElement));

            ITextSymbol pTextSymbol = new TextSymbol();
            pTextSymbol.HorizontalAlignment = esriTextHorizontalAlignment.esriTHALeft;
            pTextSymbol.VerticalAlignment = esriTextVerticalAlignment.esriTVABottom;

            pTextElement.Text = text;
            pTextElement.ScaleText = false;
            pTextElement.Symbol = pTextSymbol;

            pElementOfText.Geometry = esriPoint;

            IMarkerElement pMarkerShpElement = ((ESRI.ArcGIS.Carto.IMarkerElement)(new MarkerElement()));

            IElement pElementofpPoint = ((ESRI.ArcGIS.Carto.IElement)(pMarkerShpElement));
            pElementofpPoint.Geometry = esriPoint;


            IRgbColor pRGB = new RgbColor();

            pRGB.RGB = (int)symbol.Color;

            ISimpleMarkerSymbol pMarkerSym = new SimpleMarkerSymbol();
            pMarkerSym.Color = pRGB;
            pMarkerSym.Size = symbol.Size;
            pMarkerSym.Style = (esriSimpleMarkerStyle)symbol.Style;
            pMarkerShpElement.Symbol = pMarkerSym;

            IGroupElement pGroupElement = ((ESRI.ArcGIS.Carto.IGroupElement)(new GroupElement()));
            pGroupElement.AddElement(pElementofpPoint);
            pElementofpPoint.Locked = true;
            pGroupElement.AddElement(pTextElement as IElement);
            
            IElement pCommonElement = ((ESRI.ArcGIS.Carto.IElement)(pGroupElement));
            

            if (isVisible)
            {
                IGraphicsContainer pGraphics = GlobalParams.ActiveView.GraphicsContainer;
                pGraphics.AddElement(pCommonElement, 0);
                if (isLocked)
                    pCommonElement.Locked = true;
                GlobalParams.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);
            }

            return GetHandle(pCommonElement);
        }

        public IElement DrawPointWithText(Aran.Geometries.Point point, PointSymbol symbol, string text, double angle)
        {
            IPoint esriPoint = Aran.Converters.ConvertToEsriGeom.FromPoint(point);
            ITextElement pTextElement = ((ESRI.ArcGIS.Carto.ITextElement)(new TextElement()));
            IElement pElementOfText = ((ESRI.ArcGIS.Carto.IElement)(pTextElement));

            ITextSymbol pTextSymbol = new TextSymbol();
            pTextSymbol.HorizontalAlignment = esriTextHorizontalAlignment.esriTHALeft;
            pTextSymbol.VerticalAlignment = esriTextVerticalAlignment.esriTVABottom;
            pTextSymbol.Angle = angle;

            pTextElement.Text = text;
            pTextElement.ScaleText = false;
            pTextElement.Symbol = pTextSymbol;

            pElementOfText.Geometry = esriPoint;

            IMarkerElement pMarkerShpElement = ((ESRI.ArcGIS.Carto.IMarkerElement)(new MarkerElement()));

            IElement pElementofpPoint = ((ESRI.ArcGIS.Carto.IElement)(pMarkerShpElement));
            //pElementofpPoint.Geometry = esriPoint;


            IRgbColor pRGB = new RgbColor();

            pRGB.RGB = (int)symbol.Color;

            ISimpleMarkerSymbol pMarkerSym = new SimpleMarkerSymbol();
            pMarkerSym.Color = pRGB;
            pMarkerSym.Size = symbol.Size;
            pMarkerSym.Style = (esriSimpleMarkerStyle)symbol.Style;
            pMarkerShpElement.Symbol = pMarkerSym;

            
            IGroupElement pGroupElement = ((ESRI.ArcGIS.Carto.IGroupElement)(new GroupElement()));
            pGroupElement.AddElement(pElementofpPoint);
            pElementofpPoint.Locked = false;
            pGroupElement.AddElement(pTextElement as IElement);

            IElement pCommonElement = ((ESRI.ArcGIS.Carto.IElement)(pGroupElement));
            return pCommonElement;

            //IGraphicsContainer pGraphics = GlobalParams.ActiveView.GraphicsContainer;
            //pGraphics.AddElement(pCommonElement, 0);
            //pCommonElement.Locked = false;
            //GlobalParams.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);

            //return GetHandle(pCommonElement);
        }


        public int DrawPointWithText(Aran.Geometries.Point point, int color, string text, Boolean isVisible = true, Boolean isLocked = true)
        {
            PointSymbol pointSymbol = new Aran.AranEnvironment.Symbols.PointSymbol();
            if (color < 0)
            {
                Random rnd = new Random();
                color = rnd.Next(256) | (rnd.Next(256) << 8) | (rnd.Next(256) << 16);
            }

            pointSymbol.Color = color;
            pointSymbol.Size = 8;
            pointSymbol.Style = ePointStyle.smsCircle;
            return DrawPointWithText(point, pointSymbol, text, isVisible, isLocked);
        }

        public int DrawLineString(LineString lineString, int color, int width, Boolean isVisible = true, Boolean isLocked = true)
        {
            MultiLineString multiLineString = new MultiLineString();
            multiLineString.Add(lineString);

            LineSymbol lineSymbol = new LineSymbol();

            if (color < 0)
            {
                Random rnd = new Random();
                color = rnd.Next(256) | (rnd.Next(256) << 8) | (rnd.Next(256) << 16);
            }

            lineSymbol.Color = color;
            lineSymbol.Style = eLineStyle.slsSolid;
            lineSymbol.Width = width;
            return DrawMultiLineString(multiLineString, lineSymbol, isVisible, isLocked);
        }

        public int DrawLineString(LineString lineString, Aran.AranEnvironment.Symbols.LineSymbol symbol, Boolean isVisible = true, Boolean isLocked = true)
        {
            MultiLineString multiLineString = new MultiLineString();
            multiLineString.Add(lineString);
            return DrawMultiLineString(multiLineString, symbol, isVisible, isLocked);
        }

        public int DrawPolygon(Aran.Geometries.Polygon polygon, FillSymbol symbol, Boolean isVisible = true, Boolean isLocked = true)
        {
            MultiPolygon multiPolygon = new MultiPolygon();
            multiPolygon.Add(polygon);
            return DrawMultiPolygon(multiPolygon, symbol, isVisible, isLocked);
        }

        public int DrawPolygon(Aran.Geometries.Polygon polygon, int color, eFillStyle style, Boolean isVisible = true, Boolean isLocked = true)
        {
            if (color < 0)
            {
                Random rnd = new Random();
                color = rnd.Next(256) | (rnd.Next(256) << 8) | (rnd.Next(256) << 16);
            }

            FillSymbol fillSymbol = new FillSymbol();
            fillSymbol.Color = color;
            fillSymbol.Style = style;
            fillSymbol.Outline = new LineSymbol();
            fillSymbol.Outline.Color = color;
            fillSymbol.Outline.Size = fillSymbol.Size;
            return DrawPolygon(polygon, fillSymbol, isVisible, isLocked);
        }

        public int DrawRing(Aran.Geometries.Ring ring, int color, Aran.AranEnvironment.Symbols.eFillStyle style, Boolean isVisible = true, Boolean isLocked = true)
        {
            if (color < 0)
            {
                Random rnd = new Random();
                color = rnd.Next(256) | (rnd.Next(256) << 8) | (rnd.Next(256) << 16);
            }

            FillSymbol fillSymbol = new FillSymbol();
            fillSymbol.Color = color;
            fillSymbol.Style = style;
            fillSymbol.Outline = new LineSymbol();
            fillSymbol.Outline.Color = color;
            fillSymbol.Outline.Size = fillSymbol.Size;

            return DrawRing(ring, fillSymbol, isVisible, isLocked);
        }

        public int DrawRing(Aran.Geometries.Ring ring, Aran.AranEnvironment.Symbols.FillSymbol symbol, Boolean isVisible = true, Boolean isLocked = true)
        {
            Aran.Geometries.Polygon aranPolygon = new Aran.Geometries.Polygon();
            aranPolygon.ExteriorRing = ring;
            return DrawPolygon(aranPolygon, symbol, isVisible, isLocked);
        }

        public int DrawMultiLineString(MultiLineString multiLineString, Aran.AranEnvironment.Symbols.LineSymbol symbol, Boolean isVisible = true, Boolean isLocked = true)
        {
            IPolyline esriPolyline = ConvertToEsriGeom.FromMultiLineString(multiLineString);

            ILineElement pLineElement = ((ESRI.ArcGIS.Carto.ILineElement)(new LineElement()));
            IElement pElementOfpLine = ((ESRI.ArcGIS.Carto.IElement)(pLineElement));
            IGeometry pGeometry = ((ESRI.ArcGIS.Geometry.IGeometry)(esriPolyline));

            pElementOfpLine.Geometry = pGeometry;

            IRgbColor pRGB = new RgbColor();
            pRGB.RGB = (int)symbol.Color;
            ISimpleLineSymbol pLineSym = new SimpleLineSymbol();
            pLineSym.Color = pRGB;
            pLineSym.Style = (esriSimpleLineStyle)symbol.Style;
            pLineSym.Width = symbol.Width;

            pLineElement.Symbol = pLineSym;

            if (isVisible)
            {
                IGraphicsContainer pGraphics = GlobalParams.ActiveView.GraphicsContainer;
                pGraphics.AddElement(pElementOfpLine, 0);
                if (isLocked)
                    pElementOfpLine.Locked = true;
                GlobalParams.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);
            }
            return GetHandle(pElementOfpLine);
        }


        public IElement DrawMultiLineString(MultiLineString multiLineString, Aran.AranEnvironment.Symbols.LineSymbol symbol)
        {
            IPolyline esriPolyline = ConvertToEsriGeom.FromMultiLineString(multiLineString);

            ILineElement pLineElement = ((ESRI.ArcGIS.Carto.ILineElement)(new LineElement()));
            IElement pElementOfpLine = ((ESRI.ArcGIS.Carto.IElement)(pLineElement));
            IGeometry pGeometry = ((ESRI.ArcGIS.Geometry.IGeometry)(esriPolyline));

            pElementOfpLine.Geometry = pGeometry;

            IRgbColor pRGB = new RgbColor();
            pRGB.RGB = (int)symbol.Color;
            ISimpleLineSymbol pLineSym = new SimpleLineSymbol();
            pLineSym.Color = pRGB;
            pLineSym.Style = (esriSimpleLineStyle)symbol.Style;
            pLineSym.Width = symbol.Width;

            pLineElement.Symbol = pLineSym;
            return pElementOfpLine;
            //if (isVisible)
            //{
            //    IGraphicsContainer pGraphics = GlobalParams.ActiveView.GraphicsContainer;
            //    pGraphics.AddElement(pElementOfpLine, 0);
            //    if (isLocked)
            //        pElementOfpLine.Locked = true;
            //    GlobalParams.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);
            //}
            //return GetHandle(pElementOfpLine);
        }
     
        public int DrawMultiLineString(MultiLineString multiLineString, int color, int width, Boolean isVisible = true, Boolean isLocked = true)
        {
            if (color < 0)
            {
                Random rnd = new Random();
                color = rnd.Next(256) | (rnd.Next(256) << 8) | (rnd.Next(256) << 16);
            }

            LineSymbol lineSymbol = new LineSymbol();
            lineSymbol.Color = color;
            lineSymbol.Style = eLineStyle.slsSolid;
            lineSymbol.Width = width;
            return DrawMultiLineString(multiLineString, lineSymbol, isVisible, isLocked);
        }

        public int DrawMultiPolygon(MultiPolygon multiPolygon, Aran.AranEnvironment.Symbols.FillSymbol symbol, Boolean isVisible = true, Boolean isLocked = true)
        {
            IPolygon esriPolygon = Aran.Converters.ConvertToEsriGeom.FromMultiPolygon(multiPolygon);

            IRgbColor pRGB = null;
            IElement pElementofPoly = null;

            pRGB = new RgbColor();
            pRGB.RGB = (int)symbol.Color;
            ISimpleFillSymbol pFillSym = new SimpleFillSymbol();
            IFillShapeElement pFillShpElement = ((ESRI.ArcGIS.Carto.IFillShapeElement)(new PolygonElement()));

            pElementofPoly = ((ESRI.ArcGIS.Carto.IElement)(pFillShpElement));
            pElementofPoly.Geometry = ((ESRI.ArcGIS.Geometry.IGeometry)(esriPolygon));


            pFillSym.Color = pRGB;
            pFillSym.Style = ((esriSimpleFillStyle)(symbol.Style)); // esriSFSNull 'esriSFSDiagonalCross

            ILineSymbol pLineSimbol = new SimpleLineSymbol();

            IRgbColor lineRgb = new RgbColor();
            lineRgb.RGB = (int)symbol.Outline.Color;

            pLineSimbol.Color = lineRgb;
            pLineSimbol.Width = symbol.Outline.Size;
            pFillSym.Outline = pLineSimbol;


            pFillShpElement.Symbol = pFillSym;

            if (isVisible)
            {
                IGraphicsContainer pGraphics = GlobalParams.ActiveView.GraphicsContainer;
                pGraphics.AddElement(pElementofPoly, 0);
                if (isLocked)
                    pElementofPoly.Locked = true;
                GlobalParams.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);
            }

            return GetHandle(pElementofPoly);
        }

        public int DrawMultiPolygon(MultiPolygon multiPolygon, int color, Aran.AranEnvironment.Symbols.eFillStyle style, Boolean isVisible = true, Boolean isLocked = true)
        {
            if (color < 0)
            {
                Random rnd = new Random();
                color = rnd.Next(256) | (rnd.Next(256) << 8) | (rnd.Next(256) << 16);
            }

            FillSymbol fillSymbol = new FillSymbol();
            fillSymbol.Color = color;
            fillSymbol.Style = style;
            fillSymbol.Outline = new LineSymbol();
            fillSymbol.Outline.Color = color;
            fillSymbol.Outline.Size = fillSymbol.Size;
            return DrawMultiPolygon(multiPolygon, fillSymbol, isVisible, isLocked);
        }

        public void SetVisible(int graphicHandle, bool isVisible)
        {
            if (_drawedElements.ContainsKey(graphicHandle))
            {
                IGraphicsContainer pGraphics = GlobalParams.ActiveView.GraphicsContainer;

                if (isVisible)
                {
                    if (!IsHandleInContainer(graphicHandle))
                    {
                        GlobalParams.ActiveView.GraphicsContainer.AddElement(_drawedElements[graphicHandle], 0);
                        GlobalParams.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);
                    }
                }
                else if (IsHandleInContainer(graphicHandle))
                {
                    GlobalParams.ActiveView.GraphicsContainer.DeleteElement(_drawedElements[graphicHandle]);
                    GlobalParams.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);
                }
            }
        }

        public void ShowGraphic(int handle, bool isVisible)
        {
            if (!_drawedElements.ContainsKey(handle))
                return;

            var elem = _drawedElements[handle];

            try
            {
                if (isVisible)
                    GlobalParams.ActiveView.GraphicsContainer.AddElement(elem, 0);
                else
                    GlobalParams.ActiveView.GraphicsContainer.DeleteElement(elem);

                GlobalParams.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);
            }
            catch { }
        }

        public void DeleteGraphic(int handle)
        {
            if (_drawedElements.ContainsKey(handle))
            {
                try
                {
                    GlobalParams.ActiveView.GraphicsContainer.DeleteElement(_drawedElements[handle]);
                    GlobalParams.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);

                    _drawedElements.Remove(handle);
                }
                catch { }
            }
        }

      

        public void SafeDeleteGraphic(int handle)
        {
            if (_drawedElements.ContainsKey(handle))
            {
                try
                {
                    IGraphicsContainer pGraphics = GlobalParams.ActiveView.GraphicsContainer;
                    pGraphics.DeleteElement(_drawedElements[handle]);
                    GlobalParams.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);
                }
                catch { }
                _drawedElements.Remove(handle);
            }
        }

        public void ShowAnimation(bool show)
        {
            throw new NotImplementedException();
        }

        public void HideAnimation()
        {
            throw new NotImplementedException();
        }

        public void ShowAnimation()
        {
            throw new NotImplementedException();
        }

        public void Refresh()
        {
            GlobalParams.ActiveView.Refresh();
        }

        public PointSymbol SelectedPointSymbol { get; set; }

        public LineSymbol SelectedLineSymbol { get; set; }

        public FillSymbol SelectedFillSymbol { get; set; }

        public void GetExtent(out double xmin, out double ymin, out double xmax, out double ymax)
        {
            IEnvelope extent = GlobalParams.ActiveView.Extent;

            xmin = extent.XMin;
            xmax = extent.XMax;
            ymin = extent.YMin;
            ymax = extent.YMax;
        }

        public Box Extent
        {
            get
            {
                IEnvelope extent = GlobalParams.ActiveView.Extent;
                Box box = new Box();
                box[0] = new Aran.Geometries.Point(extent.XMin, extent.YMin);
                box[1] = new Aran.Geometries.Point(extent.XMax, extent.YMax);
                return box;
            }
            set
            {
                IEnvelope env = new Envelope() as IEnvelope;
                env.XMin = value[0].X;
                env.YMin = value[0].Y;
                env.XMax = value[1].X;
                env.YMax = value[1].Y;

                GlobalParams.ActiveView.Extent = env;
            }
        }

        public void SetExtent(double xmin, double ymin, double xmax, double ymax)
        {
            IEnvelope extent = GlobalParams.ActiveView.Extent;
            extent.XMin = xmin;
            extent.XMax = xmax;
            extent.YMin = ymin;
            extent.YMax = ymax;
        }

        //private void ConvertSpatRefToEsriSpatRef ( SpatialReference spatialReference )
        //{
        //    IGeographicCoordinateSystem geogCoordSystem;
        //    ISpatialReferenceFactory spatRefFactory = new SpatialReferenceEnvironmentClass ( ) as ISpatialReferenceFactory;
        //    IProjection projection;
        //    int paramCount;
        //    int esriParamCount;
        //    IParameter [ ] parameters = new IParameter [ 21 ];
        //    ILinearUnit linearUnit = null;
        //    IProjectedCoordinateSystem projCS;
        //    IProjectedCoordinateSystemEdit pcsEdit;
        //    IUnit projectedXYUnit;

        //    _mapControl.SpatialReference = null;
        //    if ( spatRefFactory == null )
        //        return;

        //    if ( spatialReference.SpatialReferenceType == SpatialReferenceType.srtGeographic )
        //    {
        //        geogCoordSystem = spatRefFactory.CreateGeographicCoordinateSystem ( ConvertSpatialReferenceGeoType ( spatialReference.Ellipsoid.srGeoType ) );
        //        spatRefFactory = null;
        //        _mapControl.SpatialReference = geogCoordSystem as ISpatialReference;
        //        geogCoordSystem = null;
        //    }
        //    else
        //    {
        //        projection = spatRefFactory.CreateProjection ( ConvertSpatialReferenceType ( spatialReference.SpatialReferenceType ) );

        //        if ( projection == null )
        //        {
        //            spatRefFactory = null;
        //            return;
        //        }
        //        paramCount = spatialReference.ParamList.Count;
        //        esriParamCount = paramCount;

        //        if ( esriParamCount < 20 )
        //            esriParamCount = 20;

        //        for ( int i = 0; i <= paramCount - 1; i++ )
        //        {
        //            parameters [ i ] = spatRefFactory.CreateParameter ( ConvertSpatialReferenceParamType ( spatialReference.ParamList [ i ].srParamType ) );
        //            parameters [ i ].Value = spatialReference.ParamList [ i ].value;
        //        }

        //        geogCoordSystem = spatRefFactory.CreateGeographicCoordinateSystem ( ConvertSpatialReferenceGeoType ( spatialReference.Ellipsoid.srGeoType ) );

        //        if ( geogCoordSystem == null )
        //        {
        //            SetNull ( parameters, paramCount, projection, linearUnit, spatRefFactory, geogCoordSystem );
        //            return;
        //        }

        //        projectedXYUnit = spatRefFactory.CreateUnit ( ConvertSpatialReferenceUnit ( spatialReference.SpatialReferenceUnit ) );
        //        if ( projectedXYUnit == null )
        //        {
        //            SetNull ( parameters, paramCount, projection, linearUnit, spatRefFactory, geogCoordSystem );
        //            return;
        //        }


        //        linearUnit = projectedXYUnit as ILinearUnit;

        //        if ( linearUnit == null )
        //        {
        //            SetNull ( parameters, paramCount, projection, linearUnit, spatRefFactory, geogCoordSystem );
        //            return;
        //        }

        //        projCS = new ProjectedCoordinateSystemClass ( ) as IProjectedCoordinateSystem;
        //        if ( projCS == null )
        //        {
        //            SetNull ( parameters, paramCount, projection, linearUnit, spatRefFactory, geogCoordSystem );
        //            return;
        //        }

        //        pcsEdit = projCS as IProjectedCoordinateSystemEdit;

        //        if ( pcsEdit == null )
        //        {
        //            projCS = null;
        //            SetNull ( parameters, paramCount, projection, linearUnit, spatRefFactory, geogCoordSystem );
        //            return;
        //        }

        //        pcsEdit.DefineEx ( spatialReference.name, spatialReference.name, "", "", "", geogCoordSystem, linearUnit, projection, parameters [ 0 ] );

        //        _mapControl.SpatialReference = projCS as ISpatialReference;
        //    }
        //}

        //private void SetNull ( IParameter [ ] parameters, int paramCount, IProjection projection, ILinearUnit linearUnit,
        //                            ISpatialReferenceFactory spatRefFactory, IGeographicCoordinateSystem geogCoordSystem )
        //{
        //    for ( int i = 0; i <= paramCount - 1; i++ )
        //    {
        //        parameters [ i ] = null;
        //    }
        //    projection = null;
        //    linearUnit = null;
        //    spatRefFactory = null;
        //    geogCoordSystem = null;
        //}

        //private int ConvertSpatialReferenceGeoType ( SpatialReferenceGeoType pandaSrGeoType )
        //{			
        //    esriSRGeoCSType[] esriSRGeoCsTypes = new esriSRGeoCSType[]{esriSRGeoCSType.esriSRGeoCS_WGS1984,esriSRGeoCSType.esriSRGeoCS_Krasovsky1940,esriSRGeoCSType.esriSRGeoCS_NAD1983};
        //    if ( ( int ) pandaSrGeoType < ( int ) SpatialReferenceGeoType.srgtWGS1984 ||
        //            ( int ) pandaSrGeoType > ( int ) SpatialReferenceGeoType.srgtNAD1983 )
        //        return ( int ) esriSRGeoCSType.esriSRGeoCS_WGS1984;
        //    else
        //        return ( int ) esriSRGeoCsTypes [ ( int ) pandaSrGeoType ];
        //}

        //private int ConvertSpatialReferenceType ( SpatialReferenceType pandaSpatRefType )
        //{
        //    esriSRProjectionType[] esriSpatRefProjTypes = new esriSRProjectionType[]{0, esriSRProjectionType.esriSRProjection_Mercator,esriSRProjectionType.esriSRProjection_TransverseMercator,esriSRProjectionType.esriSRProjection_GaussKruger};
        //    if ( ( int ) pandaSpatRefType < ( int ) SpatialReferenceType.srtMercator || ( int ) pandaSpatRefType > ( int ) SpatialReferenceType.srtGauss_Krueger )
        //        return 0;
        //    else
        //        return ( int ) esriSpatRefProjTypes [ ( int ) pandaSpatRefType ];
        //}

        //private int ConvertSpatialReferenceUnit ( SpatialReferenceUnit pandaSpatRefUnit )
        //{
        //    esriSRUnitType [ ] esriSpatRefUnitTypes = new esriSRUnitType [ ] { esriSRUnitType.esriSRUnit_Meter, esriSRUnitType.esriSRUnit_Foot, esriSRUnitType.esriSRUnit_NauticalMile, esriSRUnitType.esriSRUnit_Kilometer };
        //    if ( ( int ) pandaSpatRefUnit < ( int ) SpatialReferenceUnit.sruMeter || ( int ) pandaSpatRefUnit > ( int ) SpatialReferenceUnit.sruKilometer )
        //        return 0;
        //    else
        //        return ( int ) esriSpatRefUnitTypes [ ( int ) pandaSpatRefUnit ];
        //}

        //private int ConvertSpatialReferenceParamType ( SpatialReferenceParamType pandaSpatRefParamType )
        //{
        //    esriSRParameterType [ ] esriSpatRefParamTypes = new esriSRParameterType [ ]
        //        {esriSRParameterType.esriSRParameter_FalseEasting, esriSRParameterType.esriSRParameter_FalseNorthing, 
        //        esriSRParameterType.esriSRParameter_ScaleFactor, esriSRParameterType.esriSRParameter_Azimuth, 
        //        esriSRParameterType.esriSRParameter_CentralMeridian, esriSRParameterType.esriSRParameter_LatitudeOfOrigin,
        //        esriSRParameterType.esriSRParameter_LongitudeOfCenter};
        //    if ( ( int ) pandaSpatRefParamType < ( int ) SpatialReferenceParamType.srptFalseEasting || ( int ) pandaSpatRefParamType < ( int ) SpatialReferenceParamType.srptFalseEasting )
        //        return 0;
        //    else
        //        return ( int ) esriSpatRefParamTypes [ ( int ) pandaSpatRefParamType ];
        //}

        //private SpatialReference GetSpatialReference ( )
        //{
        //    if ( _spatialReference != null )
        //        return _spatialReference;
        //    _spatialReference = new SpatialReference ( );
        //    IGeographicCoordinateSystem geogCoordSystem = null;
        //    IProjection projection;
        //    ILinearUnit coordinateUnit;
        //    ISpheroid spheroid = null;
        //    ISpatialReference spatRefShp;
        //    if ( _mapControl.SpatialReference == null )
        //        return null;
        //    IProjectedCoordinateSystem projectedCoordSystem = _mapControl.SpatialReference as IProjectedCoordinateSystem;
        //    if ( projectedCoordSystem != null )
        //    {
        //        geogCoordSystem = projectedCoordSystem.GeographicCoordinateSystem;
        //        projection = projectedCoordSystem.Projection as IProjection;
        //        if ( projection != null )
        //        {
        //            if ( projectedCoordSystem.Projection.Name == "Transverse_Mercator" )
        //                _spatialReference.SpatialReferenceType = SpatialReferenceType.srtTransverse_Mercator;
        //            else if ( projectedCoordSystem.Projection.Name == "Mercator" )
        //                _spatialReference.SpatialReferenceType = SpatialReferenceType.srtMercator;
        //            else if ( projectedCoordSystem.Projection.Name == "Gauss_Krueger" )
        //                _spatialReference.SpatialReferenceType = SpatialReferenceType.srtGauss_Krueger;
        //        }
        //        coordinateUnit = projectedCoordSystem.CoordinateUnit as ILinearUnit;
        //        if ( coordinateUnit != null )
        //        {
        //            if ( projectedCoordSystem.CoordinateUnit.Name == "Meter" )
        //                _spatialReference.SpatialReferenceUnit = SpatialReferenceUnit.sruMeter;
        //        }
        //    }

        //    if ( geogCoordSystem == null )
        //    {
        //        geogCoordSystem = _mapControl.SpatialReference as IGeographicCoordinateSystem;
        //        if ( geogCoordSystem != null )
        //            _spatialReference.SpatialReferenceType = SpatialReferenceType.srtGeographic;
        //    }

        //    if ( geogCoordSystem != null )
        //    {
        //        spheroid = geogCoordSystem.Datum.Spheroid;
        //        spatRefShp = geogCoordSystem;
        //    }
        //    _spatialReference.Ellipsoid.isValid = ( spheroid != null );

        //    if ( _spatialReference.Ellipsoid.isValid )
        //    {
        //        if ( spheroid.Name == "WGS_1984" )
        //            _spatialReference.Ellipsoid.srGeoType = SpatialReferenceGeoType.srgtWGS1984;
        //        _spatialReference.Ellipsoid.semiMajorAxis = spheroid.SemiMajorAxis;
        //        _spatialReference.Ellipsoid.flattening = spheroid.Flattening;
        //    }

        //    switch ( _spatialReference.SpatialReferenceType )
        //    {
        //        case SpatialReferenceType.srtGeographic:
        //            _spatialReference.SpatialReferenceUnit = SpatialReferenceUnit.sruMeter;
        //            break;
        //        case SpatialReferenceType.srtMercator:
        //            break;
        //        case SpatialReferenceType.srtTransverse_Mercator:
        //            _spatialReference.ParamList.Add ( new SpatialReferenceParam ( SpatialReferenceParamType.srptFalseEasting, projectedCoordSystem.FalseEasting ) );
        //            _spatialReference.ParamList.Add ( new SpatialReferenceParam ( SpatialReferenceParamType.srptFalseNorthing, projectedCoordSystem.FalseNorthing ) );
        //            _spatialReference.ParamList.Add ( new SpatialReferenceParam ( SpatialReferenceParamType.srptCentralMeridian, projectedCoordSystem.CentralMeridian [ true ] ) );
        //            _spatialReference.ParamList.Add ( new SpatialReferenceParam ( SpatialReferenceParamType.srptLatitudeOfOrigin, 0.0 ) );
        //            _spatialReference.ParamList.Add ( new SpatialReferenceParam ( SpatialReferenceParamType.srptScaleFactor, projectedCoordSystem.ScaleFactor ) );
        //            break;
        //        case SpatialReferenceType.srtGauss_Krueger:
        //            break;
        //        default:
        //            break;
        //    }

        //    projectedCoordSystem = null;
        //    geogCoordSystem = null;
        //    spatRefShp = null;
        //    projection = null;
        //    coordinateUnit = null;
        //    spheroid = null;

        //    return _spatialReference;
        //}

        private int GetHandle(IElement esriElement)
        {
            _index++;
            _drawedElements.Add(_index, esriElement);
            return _index;
        }

        private bool IsHandleInContainer(int graphicHandle)
        {
            try
            {
                GlobalParams.ActiveView.GraphicsContainer.UpdateElement(_drawedElements[graphicHandle]);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private bool IsElementInContainer(IElement iElement)
        {
            IGraphicsContainer pGraphics = GlobalParams.ActiveView.GraphicsContainer;
            pGraphics.Reset();
            IElement esriElement = pGraphics.Next();
            while (esriElement != null)
            {
                if (esriElement == iElement)
                    return true;
                esriElement = pGraphics.Next();
            }
            return false;
        }

        public bool SelectSymbol(BaseSymbol inSymbol, out BaseSymbol outSymbol, int hwnd)
        {
            outSymbol = null;
            return false;
        }


        private IMap _axMapControl;
        private Dictionary<int, IElement> _drawedElements;
        private int _index;
        private SpatRefConverter _spatialRefConverter;
        private SpatialReference _spatialReference;
        private SpatialReference _wgs84SR;


        public void SetMapTool(MapTool mapTool)
        {
            throw new NotImplementedException();
        }

        public SpatialReference ViewProjection
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }


        public SpatialReference WGS84SR
        {
            get { throw new NotImplementedException(); }
        }


        public List<Geometry> GetSelectedGraphicGeometries()
        {
            throw new NotImplementedException();
        }
    }
}
