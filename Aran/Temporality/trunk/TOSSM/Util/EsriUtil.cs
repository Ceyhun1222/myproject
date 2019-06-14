using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.CatalogUI;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Geometry;

namespace TOSSM.Util
{
    public class EsriUtil
    {
        public static IPoint GetScreenCoordinatesFromMapCoorindates(
            IPoint mapPoint, 
            IActiveView activeView)
        {
            if (mapPoint == null || mapPoint.IsEmpty || activeView == null)
            {
                return null;
            }

            IScreenDisplay screenDisplay = activeView.ScreenDisplay;
            IDisplayTransformation displayTransformation =
              screenDisplay.DisplayTransformation;

            Int32 x;
            Int32 y;
            displayTransformation.FromMapPoint(mapPoint, out x, out y);
            IPoint returnPoint = new PointClass();
            returnPoint.PutCoords(x, y);
            return returnPoint;
        }


        public static IPoint GetMapCoordinatesFromScreenCoordinates(
            IPoint screenPoint, IActiveView activeView)
        {

            if (screenPoint == null || screenPoint.IsEmpty || activeView == null)
            {
                return null;
            }

            IScreenDisplay screenDisplay = activeView.ScreenDisplay;
            IDisplayTransformation displayTransformation =
              screenDisplay.DisplayTransformation;

            return displayTransformation.ToMapPoint((Int32)screenPoint.X,(Int32)screenPoint.Y); // Explicit Cast
        }

        ///<summary>Use the SpatialReferenceDialog to change the coordinate system or spatial reference of the map.</summary>
        ///
        ///<param name="hWnd">The application window handle.</param>
        ///<param name="map">An IMap interface.</param>
        /// 
        ///<remarks></remarks>
        public void ChangeMapSpatialReference(System.Int32 hWnd, ESRI.ArcGIS.Carto.IMap map)
        {
            if (map == null)
            {
                return;
            }

            ESRI.ArcGIS.CatalogUI.ISpatialReferenceDialog2 spatialReferenceDialog = new ESRI.ArcGIS.CatalogUI.SpatialReferenceDialogClass();
            ESRI.ArcGIS.Geometry.ISpatialReference spatialReference = spatialReferenceDialog.DoModalCreate(true, false, false, hWnd);
            if ((!(map.SpatialReferenceLocked)))
            {
                map.SpatialReference = spatialReference;
            }
        }

        ///<summary>Create Gradient Fill Symbol by specifying the starting and ending colors and the number of levels to make in between.</summary>
        ///  
        ///<param name="startRgbColor">An IRgbColor interface that is the beginning color for the ramp.</param>
        ///<param name="endRgbColor">An IRgbColor interface that is the ending color for the ramp.</param>
        ///<param name="numberOfIntervals">A System.Int32 that is the number of color level gradiations that the color ramp will make.</param>
        ///   
        ///<returns>An IGradientFillSymbol interface.</returns>
        ///   
        ///<remarks></remarks>
        public ESRI.ArcGIS.Display.IGradientFillSymbol CreateGradientFillSymbol(ESRI.ArcGIS.Display.IRgbColor startRgbColor, ESRI.ArcGIS.Display.IRgbColor endRgbColor, System.Int32 numberOfIntervals)
        {

            if (startRgbColor == null || endRgbColor == null || numberOfIntervals < 0)
            {
                return null;
            }
            // Create the Ramp for the Gradient Fill
            ESRI.ArcGIS.Display.IAlgorithmicColorRamp algorithmicColorRamp = new ESRI.ArcGIS.Display.AlgorithmicColorRampClass();
            algorithmicColorRamp.FromColor = startRgbColor;
            algorithmicColorRamp.ToColor = endRgbColor;
            algorithmicColorRamp.Algorithm = ESRI.ArcGIS.Display.esriColorRampAlgorithm.esriHSVAlgorithm;

            // Create the Gradient Fill
            ESRI.ArcGIS.Display.IGradientFillSymbol gradientFillSymbol = new ESRI.ArcGIS.Display.GradientFillSymbolClass();
            gradientFillSymbol.ColorRamp = algorithmicColorRamp;
            gradientFillSymbol.GradientAngle = 45;
            gradientFillSymbol.GradientPercentage = 0.9;
            gradientFillSymbol.IntervalCount = numberOfIntervals;
            gradientFillSymbol.Style = ESRI.ArcGIS.Display.esriGradientFillStyle.esriGFSLinear;

            return gradientFillSymbol;
        }

        ///<summary>Flash geometry on the display. The geometry type could be polygon, polyline, point, or multipoint.</summary>
        ///
        ///<param name="geometry"> An IGeometry interface</param>
        ///<param name="color">An IRgbColor interface</param>
        ///<param name="display">An IDisplay interface</param>
        ///<param name="delay">A System.Int32 that is the time im milliseconds to wait.</param>
        /// 
        ///<remarks></remarks>
        public void FlashGeometry(ESRI.ArcGIS.Geometry.IGeometry geometry, ESRI.ArcGIS.Display.IRgbColor color, ESRI.ArcGIS.Display.IDisplay display, System.Int32 delay)
        {
            if (geometry == null || color == null || display == null)
            {
                return;
            }

            display.StartDrawing(display.hDC, (System.Int16)ESRI.ArcGIS.Display.esriScreenCache.esriNoScreenCache); // Explicit Cast


            switch (geometry.GeometryType)
            {
                case ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryPolygon:
                    {
                        //Set the flash geometry's symbol.
                        ESRI.ArcGIS.Display.ISimpleFillSymbol simpleFillSymbol = new ESRI.ArcGIS.Display.SimpleFillSymbolClass();
                        simpleFillSymbol.Color = color;
                        ESRI.ArcGIS.Display.ISymbol symbol = simpleFillSymbol as ESRI.ArcGIS.Display.ISymbol; // Dynamic Cast
                        symbol.ROP2 = ESRI.ArcGIS.Display.esriRasterOpCode.esriROPNotXOrPen;

                        //Flash the input polygon geometry.
                        display.SetSymbol(symbol);
                        display.DrawPolygon(geometry);
                        System.Threading.Thread.Sleep(delay);
                        display.DrawPolygon(geometry);
                        break;
                    }

                case ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryPolyline:
                    {
                        //Set the flash geometry's symbol.
                        ESRI.ArcGIS.Display.ISimpleLineSymbol simpleLineSymbol = new ESRI.ArcGIS.Display.SimpleLineSymbolClass();
                        simpleLineSymbol.Width = 4;
                        simpleLineSymbol.Color = color;
                        ESRI.ArcGIS.Display.ISymbol symbol = simpleLineSymbol as ESRI.ArcGIS.Display.ISymbol; // Dynamic Cast
                        symbol.ROP2 = ESRI.ArcGIS.Display.esriRasterOpCode.esriROPNotXOrPen;

                        //Flash the input polyline geometry.
                        display.SetSymbol(symbol);
                        display.DrawPolyline(geometry);
                        System.Threading.Thread.Sleep(delay);
                        display.DrawPolyline(geometry);
                        break;
                    }

                case ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryPoint:
                    {
                        //Set the flash geometry's symbol.
                        ESRI.ArcGIS.Display.ISimpleMarkerSymbol simpleMarkerSymbol = new ESRI.ArcGIS.Display.SimpleMarkerSymbolClass();
                        simpleMarkerSymbol.Style = ESRI.ArcGIS.Display.esriSimpleMarkerStyle.esriSMSCircle;
                        simpleMarkerSymbol.Size = 12;
                        simpleMarkerSymbol.Color = color;
                        ESRI.ArcGIS.Display.ISymbol symbol = simpleMarkerSymbol as ESRI.ArcGIS.Display.ISymbol; // Dynamic Cast
                        symbol.ROP2 = ESRI.ArcGIS.Display.esriRasterOpCode.esriROPNotXOrPen;

                        //Flash the input point geometry.
                        display.SetSymbol(symbol);
                        display.DrawPoint(geometry);
                        System.Threading.Thread.Sleep(delay);
                        display.DrawPoint(geometry);
                        break;
                    }

                case ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryMultipoint:
                    {
                        //Set the flash geometry's symbol.
                        ESRI.ArcGIS.Display.ISimpleMarkerSymbol simpleMarkerSymbol = new ESRI.ArcGIS.Display.SimpleMarkerSymbolClass();
                        simpleMarkerSymbol.Style = ESRI.ArcGIS.Display.esriSimpleMarkerStyle.esriSMSCircle;
                        simpleMarkerSymbol.Size = 12;
                        simpleMarkerSymbol.Color = color;
                        ESRI.ArcGIS.Display.ISymbol symbol = simpleMarkerSymbol as ESRI.ArcGIS.Display.ISymbol; // Dynamic Cast
                        symbol.ROP2 = ESRI.ArcGIS.Display.esriRasterOpCode.esriROPNotXOrPen;

                        //Flash the input multipoint geometry.
                        display.SetSymbol(symbol);
                        display.DrawMultipoint(geometry);
                        System.Threading.Thread.Sleep(delay);
                        display.DrawMultipoint(geometry);
                        break;
                    }
            }
            display.FinishDrawing();
        }


        ///<summary>
        ///Generate a SpatialReference by setting it's default projection.
        ///</summary>
        ///<param name="coordinateSystem">An ESRI.ArcGIS.Geometry.esriSRProjCSType projection. Example: ESRI.ArcGIS.Geometry.esriSRProjCSType.esriSRProjCS_World_WinkelI</param>
        ///<returns>A newly created ESRI.ArcGIS.Geometry.ISpatialReference interface.</returns>
        ///<remarks>You need a SpatialReference in order to draw graphics and geometric objects in the correct location in the Map.</remarks>
        public ESRI.ArcGIS.Geometry.ISpatialReference MakeSpatialReference(ESRI.ArcGIS.Geometry.esriSRProjCSType coordinateSystem)
        {

            ESRI.ArcGIS.Geometry.ISpatialReferenceFactory spatialReferenceFactory = new ESRI.ArcGIS.Geometry.SpatialReferenceEnvironmentClass();

            //Create a projected coordinate system and define its domain, resolution, and x,y tolerance.
            ESRI.ArcGIS.Geometry.ISpatialReferenceResolution spatialReferenceResolution = spatialReferenceFactory.CreateProjectedCoordinateSystem(System.Convert.ToInt32(coordinateSystem)) as ESRI.ArcGIS.Geometry.ISpatialReferenceResolution;
            spatialReferenceResolution.ConstructFromHorizon();
            ESRI.ArcGIS.Geometry.ISpatialReferenceTolerance spatialReferenceTolerance = spatialReferenceResolution as ESRI.ArcGIS.Geometry.ISpatialReferenceTolerance;
            spatialReferenceTolerance.SetDefaultXYTolerance();
            ESRI.ArcGIS.Geometry.ISpatialReference spatialReference = spatialReferenceResolution as ESRI.ArcGIS.Geometry.ISpatialReference;

            return spatialReference;

        }

        ///<summary>Zoom in ActiveView using a ratio of the current extent.</summary>
        ///  
        ///<param name="activeView">An IActiveView interface.</param>
        ///<param name="zoomRatio">A Double that is the ratio to zoom in. Less that 1 zooms in (Example: .75), greater than 1 zooms out (Example: 2).</param>
        ///   
        ///<remarks>Both the width and height ratio of the zoomed area is preserved.</remarks>
        public void ZoomByRatio(ESRI.ArcGIS.Carto.IActiveView activeView, System.Double zoomRatio)
        {
            if (activeView == null || zoomRatio < 0)
            {
                return;
            }
            ESRI.ArcGIS.Geometry.IEnvelope envelope = activeView.Extent;
            envelope.Expand(zoomRatio, zoomRatio, true);
            activeView.Extent = envelope;
            activeView.Refresh();
        }

        public static void ClearMap(ESRI.ArcGIS.Carto.IMap map)
        {
            ESRI.ArcGIS.Carto.IGraphicsContainer graphicsContainer = (ESRI.ArcGIS.Carto.IGraphicsContainer)map; // Explicit Cast

            graphicsContainer.DeleteAllElements();
        }


        ///<summary>Use the SpatialReferenceDialog to change the coordinate system or spatial reference of the map.</summary>
        ///
        ///<param name="hWnd">The application window handle.</param>
        ///<param name="map">An IMap interface.</param>
        /// 
        ///<remarks></remarks>
        public void ChangeMapSpatialReference663689556(Int32 hWnd, IMap map)
        {
            if (map == null)
            {
                return;
            }

            ISpatialReferenceDialog2 spatialReferenceDialog = new SpatialReferenceDialogClass();
            ISpatialReference spatialReference = spatialReferenceDialog.DoModalCreate(true, false, false, hWnd);
            if ((!(map.SpatialReferenceLocked)))
            {
                map.SpatialReference = spatialReference;
            }
        }


        ///<summary>Create Gradient Fill Symbol by specifying the starting and ending colors and the number of levels to make in between.</summary>
        ///  
        ///<param name="startRgbColor">An IRgbColor interface that is the beginning color for the ramp.</param>
        ///<param name="endRgbColor">An IRgbColor interface that is the ending color for the ramp.</param>
        ///<param name="numberOfIntervals">A System.Int32 that is the number of color level gradiations that the color ramp will make.</param>
        ///   
        ///<returns>An IGradientFillSymbol interface.</returns>
        ///   
        ///<remarks></remarks>
        public IGradientFillSymbol CreateGradientFillSymbol91464140(IRgbColor startRgbColor, IRgbColor endRgbColor, Int32 numberOfIntervals)
        {

            if (startRgbColor == null || endRgbColor == null || numberOfIntervals < 0)
            {
                return null;
            }
            // Create the Ramp for the Gradient Fill
            IAlgorithmicColorRamp algorithmicColorRamp = new AlgorithmicColorRampClass();
            algorithmicColorRamp.FromColor = startRgbColor;
            algorithmicColorRamp.ToColor = endRgbColor;
            algorithmicColorRamp.Algorithm = esriColorRampAlgorithm.esriHSVAlgorithm;

            // Create the Gradient Fill
            IGradientFillSymbol gradientFillSymbol = new GradientFillSymbolClass();
            gradientFillSymbol.ColorRamp = algorithmicColorRamp;
            gradientFillSymbol.GradientAngle = 45;
            gradientFillSymbol.GradientPercentage = 0.9;
            gradientFillSymbol.IntervalCount = numberOfIntervals;
            gradientFillSymbol.Style = esriGradientFillStyle.esriGFSLinear;

            return gradientFillSymbol;
        }

        ///<summary>Generate an RgbColor by specifying the amount of Red, Green and Blue.</summary>
        /// 
        ///<param name="myRed">A byte (0 to 255) used to represent the Red color. Example: 0</param>
        ///<param name="myGreen">A byte (0 to 255) used to represent the Green color. Example: 255</param>
        ///<param name="myBlue">A byte (0 to 255) used to represent the Blue color. Example: 123</param>
        ///  
        ///<returns>An IRgbColor interface</returns>
        ///  
        ///<remarks></remarks>
        public static ESRI.ArcGIS.Display.IRgbColor CreateRgbColor(System.Byte transparency, System.Byte myRed, System.Byte myGreen, System.Byte myBlue)
        {
            ESRI.ArcGIS.Display.IRgbColor rgbColor = new ESRI.ArcGIS.Display.RgbColorClass();
            rgbColor.Red = myRed;
            rgbColor.Green = myGreen;
            rgbColor.Blue = myBlue;
            rgbColor.Transparency = transparency;
            //rgbColor.UseWindowsDithering = true;
            return rgbColor;
        }

        ///<summary>Flash geometry on the display. The geometry type could be polygon, polyline, point, or multipoint.</summary>
        ///
        ///<param name="geometry"> An IGeometry interface</param>
        ///<param name="color">An IRgbColor interface</param>
        ///<param name="display">An IDisplay interface</param>
        ///<param name="delay">A System.Int32 that is the time im milliseconds to wait.</param>
        /// 
        ///<remarks></remarks>
        public void FlashGeometry2094776712(IGeometry geometry, IRgbColor color, IDisplay display, Int32 delay)
        {
            if (geometry == null || color == null || display == null)
            {
                return;
            }

            display.StartDrawing(display.hDC, (Int16)esriScreenCache.esriNoScreenCache); // Explicit Cast


            switch (geometry.GeometryType)
            {
                case esriGeometryType.esriGeometryPolygon:
                    {
                        //Set the flash geometry's symbol.
                        ISimpleFillSymbol simpleFillSymbol = new SimpleFillSymbolClass();
                        simpleFillSymbol.Color = color;
                        ISymbol symbol = simpleFillSymbol as ISymbol; // Dynamic Cast
                        symbol.ROP2 = esriRasterOpCode.esriROPNotXOrPen;

                        //Flash the input polygon geometry.
                        display.SetSymbol(symbol);
                        display.DrawPolygon(geometry);
                        Thread.Sleep(delay);
                        display.DrawPolygon(geometry);
                        break;
                    }

                case esriGeometryType.esriGeometryPolyline:
                    {
                        //Set the flash geometry's symbol.
                        ISimpleLineSymbol simpleLineSymbol = new SimpleLineSymbolClass();
                        simpleLineSymbol.Width = 4;
                        simpleLineSymbol.Color = color;
                        ISymbol symbol = simpleLineSymbol as ISymbol; // Dynamic Cast
                        symbol.ROP2 = esriRasterOpCode.esriROPNotXOrPen;

                        //Flash the input polyline geometry.
                        display.SetSymbol(symbol);
                        display.DrawPolyline(geometry);
                        Thread.Sleep(delay);
                        display.DrawPolyline(geometry);
                        break;
                    }

                case esriGeometryType.esriGeometryPoint:
                    {
                        //Set the flash geometry's symbol.
                        ISimpleMarkerSymbol simpleMarkerSymbol = new SimpleMarkerSymbolClass();
                        simpleMarkerSymbol.Style = esriSimpleMarkerStyle.esriSMSCircle;
                        simpleMarkerSymbol.Size = 12;
                        simpleMarkerSymbol.Color = color;
                        ISymbol symbol = simpleMarkerSymbol as ISymbol; // Dynamic Cast
                        symbol.ROP2 = esriRasterOpCode.esriROPNotXOrPen;

                        //Flash the input point geometry.
                        display.SetSymbol(symbol);
                        display.DrawPoint(geometry);
                        Thread.Sleep(delay);
                        display.DrawPoint(geometry);
                        break;
                    }

                case esriGeometryType.esriGeometryMultipoint:
                    {
                        //Set the flash geometry's symbol.
                        ISimpleMarkerSymbol simpleMarkerSymbol = new SimpleMarkerSymbolClass();
                        simpleMarkerSymbol.Style = esriSimpleMarkerStyle.esriSMSCircle;
                        simpleMarkerSymbol.Size = 12;
                        simpleMarkerSymbol.Color = color;
                        ISymbol symbol = simpleMarkerSymbol as ISymbol; // Dynamic Cast
                        symbol.ROP2 = esriRasterOpCode.esriROPNotXOrPen;

                        //Flash the input multipoint geometry.
                        display.SetSymbol(symbol);
                        display.DrawMultipoint(geometry);
                        Thread.Sleep(delay);
                        display.DrawMultipoint(geometry);
                        break;
                    }
            }
            display.FinishDrawing();
        }


        ///<summary>Obtain the real world (map) coordinates from the device (screen) coordinates.</summary>
        /// 
        ///<param name="screenPoint">An IPoint interface that contains the X,Y values from the device (screen) in your Windows application.</param>
        ///<param name="activeView">An IActiveView interface</param>
        ///  
        ///<returns>An IPoint interface containing the real world (map) coordinates is returned.</returns>
        ///  
        ///<remarks></remarks>
        public IPoint GetMapCoordinatesFromScreenCoordinates1652865865(IPoint screenPoint, IActiveView activeView)
        {
            if (screenPoint == null || screenPoint.IsEmpty || activeView == null)
            {
                return null;
            }
            IScreenDisplay screenDisplay = activeView.ScreenDisplay;
            IDisplayTransformation displayTransformation = screenDisplay.DisplayTransformation;

            return displayTransformation.ToMapPoint((Int32)screenPoint.X, (Int32)screenPoint.Y); // Explicit Cast
        }

        ///<summary>
        ///Generate a SpatialReference by setting it's default projection.
        ///</summary>
        ///<param name="coordinateSystem">An ESRI.ArcGIS.Geometry.esriSRProjCSType projection. Example: ESRI.ArcGIS.Geometry.esriSRProjCSType.esriSRProjCS_World_WinkelI</param>
        ///<returns>A newly created ESRI.ArcGIS.Geometry.ISpatialReference interface.</returns>
        ///<remarks>You need a SpatialReference in order to draw graphics and geometric objects in the correct location in the Map.</remarks>
        public ISpatialReference MakeSpatialReference804430846(esriSRProjCSType coordinateSystem)
        {

            ISpatialReferenceFactory spatialReferenceFactory = new SpatialReferenceEnvironmentClass();

            //Create a projected coordinate system and define its domain, resolution, and x,y tolerance.
            ISpatialReferenceResolution spatialReferenceResolution = spatialReferenceFactory.CreateProjectedCoordinateSystem(Convert.ToInt32(coordinateSystem)) as ISpatialReferenceResolution;
            spatialReferenceResolution.ConstructFromHorizon();
            ISpatialReferenceTolerance spatialReferenceTolerance = spatialReferenceResolution as ISpatialReferenceTolerance;
            spatialReferenceTolerance.SetDefaultXYTolerance();
            ISpatialReference spatialReference = spatialReferenceResolution as ISpatialReference;

            return spatialReference;

        }


        ///<summary>Zoom in ActiveView using a ratio of the current extent and re-center based upon supplied x,y map coordinates.</summary>
        ///
        ///<param name="activeView">An IActiveView interface.</param>
        ///<param name="zoomRatio">A System.Double that is the ratio to zoom in. Less that 1 zooms in (Example: .75), greater than 1 zooms out (Example: 2).</param>
        ///<param name="xMap">A System.Double that is the x portion of a point in map units to re-center on.</param>
        ///<param name="yMap">A System.Double that is the y portion of a point in map units to re-center on.</param>
        /// 
        ///<remarks>Both the width and height ratio of the zoomed area is preserved.</remarks>
        public static void ZoomByRatioAndRecenter(ESRI.ArcGIS.Carto.IActiveView activeView, System.Double zoomRatio, System.Double xMap, System.Double yMap)
        {
            if (activeView == null || zoomRatio < 0)
            {
                return;
            }
            ESRI.ArcGIS.Geometry.IEnvelope envelope = activeView.Extent;
            ESRI.ArcGIS.Geometry.IPoint point = new ESRI.ArcGIS.Geometry.PointClass();
            point.X = xMap;
            point.Y = yMap;
            envelope.CenterAt(point);
            envelope.Expand(zoomRatio, zoomRatio, true);
            activeView.Extent = envelope;
            activeView.Refresh();
        }

        ///<summary>Zoom in ActiveView using a ratio of the current extent.</summary>
        ///  
        ///<param name="activeView">An IActiveView interface.</param>
        ///<param name="zoomRatio">A Double that is the ratio to zoom in. Less that 1 zooms in (Example: .75), greater than 1 zooms out (Example: 2).</param>
        ///   
        ///<remarks>Both the width and height ratio of the zoomed area is preserved.</remarks>
        public void ZoomByRatio1480014685(IActiveView activeView, Double zoomRatio)
        {
            if (activeView == null || zoomRatio < 0)
            {
                return;
            }
            IEnvelope envelope = activeView.Extent;
            envelope.Expand(zoomRatio, zoomRatio, true);
            activeView.Extent = envelope;
            activeView.Refresh();
        }

        ///<summary>Draw a specified graphic on the map using the supplied colors.</summary>
        ///      
        ///<param name="map">An IMap interface.</param>
        ///<param name="geometry">An IGeometry interface. It can be of the geometry type: esriGeometryPoint, esriGeometryPolyline, or esriGeometryPolygon.</param>
        ///<param name="rgbColor">An IRgbColor interface. The color to draw the geometry.</param>
        ///<param name="outlineRgbColor">An IRgbColor interface. For those geometry's with an outline it will be this color.</param>
        ///      
        ///<remarks>Calling this function will not automatically make the graphics appear in the map area. Refresh the map area after after calling this function with Methods like IActiveView.Refresh or IActiveView.PartialRefresh.</remarks>
        public static void AddGraphicToMap(ESRI.ArcGIS.Carto.IMap map, ESRI.ArcGIS.Geometry.IGeometry geometry, ESRI.ArcGIS.Display.IRgbColor rgbColor, ESRI.ArcGIS.Display.IRgbColor outlineRgbColor)
        {
            ESRI.ArcGIS.Carto.IGraphicsContainer graphicsContainer = (ESRI.ArcGIS.Carto.IGraphicsContainer)map; // Explicit Cast
            ESRI.ArcGIS.Carto.IElement element = null;
            if ((geometry.GeometryType) == esriGeometryType.esriGeometryPoint)
            {
                // Marker symbols
                ESRI.ArcGIS.Display.ISimpleMarkerSymbol simpleMarkerSymbol = new ESRI.ArcGIS.Display.SimpleMarkerSymbolClass();
                simpleMarkerSymbol.Color = rgbColor;
                simpleMarkerSymbol.Outline = true;
                simpleMarkerSymbol.OutlineColor = outlineRgbColor;
                simpleMarkerSymbol.Size = 15;
                simpleMarkerSymbol.Style = esriSimpleMarkerStyle.esriSMSCircle;

                ESRI.ArcGIS.Carto.IMarkerElement markerElement = new ESRI.ArcGIS.Carto.MarkerElementClass();
                markerElement.Symbol = simpleMarkerSymbol;
                element = (ESRI.ArcGIS.Carto.IElement)markerElement; // Explicit Cast
            }
            else if ((geometry.GeometryType) == esriGeometryType.esriGeometryPolyline)
            {
                //  Line elements
                ESRI.ArcGIS.Display.ISimpleLineSymbol simpleLineSymbol = new ESRI.ArcGIS.Display.SimpleLineSymbolClass();
                simpleLineSymbol.Color = rgbColor;
                simpleLineSymbol.Style = esriSimpleLineStyle.esriSLSSolid;
                simpleLineSymbol.Width = 5;

                ESRI.ArcGIS.Carto.ILineElement lineElement = new ESRI.ArcGIS.Carto.LineElementClass();
                lineElement.Symbol = simpleLineSymbol;
                element = (ESRI.ArcGIS.Carto.IElement)lineElement; // Explicit Cast
            }
            else if ((geometry.GeometryType) == esriGeometryType.esriGeometryPolygon)
            {
                // Polygon elements
                ESRI.ArcGIS.Display.ISimpleFillSymbol simpleFillSymbol = new ESRI.ArcGIS.Display.SimpleFillSymbolClass();
                simpleFillSymbol.Color = rgbColor;
                simpleFillSymbol.Style = esriSimpleFillStyle.esriSFSForwardDiagonal;
                ESRI.ArcGIS.Carto.IFillShapeElement fillShapeElement = new ESRI.ArcGIS.Carto.PolygonElementClass();
                fillShapeElement.Symbol = simpleFillSymbol;
                element = (ESRI.ArcGIS.Carto.IElement)fillShapeElement; // Explicit Cast
            }
            if (!(element == null))
            {
                element.Geometry = geometry;
                graphicsContainer.AddElement(element, 0);


            }
        }
    }
}
