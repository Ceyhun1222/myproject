namespace VisualizerEnvironment.Util
{
    public class EsriUtil
    {
      

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
            if ((geometry.GeometryType) == ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryPoint)
            {
                // Marker symbols
                ESRI.ArcGIS.Display.ISimpleMarkerSymbol simpleMarkerSymbol = new ESRI.ArcGIS.Display.SimpleMarkerSymbolClass();
                simpleMarkerSymbol.Color = rgbColor;
                simpleMarkerSymbol.Outline = true;
                simpleMarkerSymbol.OutlineColor = outlineRgbColor;
                simpleMarkerSymbol.Size = 15;
                simpleMarkerSymbol.Style = ESRI.ArcGIS.Display.esriSimpleMarkerStyle.esriSMSCircle;

                ESRI.ArcGIS.Carto.IMarkerElement markerElement = new ESRI.ArcGIS.Carto.MarkerElementClass();
                markerElement.Symbol = simpleMarkerSymbol;
                element = (ESRI.ArcGIS.Carto.IElement)markerElement; // Explicit Cast
            }
            else if ((geometry.GeometryType) == ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryPolyline)
            {
                //  Line elements
                ESRI.ArcGIS.Display.ISimpleLineSymbol simpleLineSymbol = new ESRI.ArcGIS.Display.SimpleLineSymbolClass();
                simpleLineSymbol.Color = rgbColor;
                simpleLineSymbol.Style = ESRI.ArcGIS.Display.esriSimpleLineStyle.esriSLSSolid;
                simpleLineSymbol.Width = 5;

                ESRI.ArcGIS.Carto.ILineElement lineElement = new ESRI.ArcGIS.Carto.LineElementClass();
                lineElement.Symbol = simpleLineSymbol;
                element = (ESRI.ArcGIS.Carto.IElement)lineElement; // Explicit Cast
            }
            else if ((geometry.GeometryType) == ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryPolygon)
            {
                // Polygon elements
                ESRI.ArcGIS.Display.ISimpleFillSymbol simpleFillSymbol = new ESRI.ArcGIS.Display.SimpleFillSymbolClass();
                simpleFillSymbol.Color = rgbColor;
                simpleFillSymbol.Style = ESRI.ArcGIS.Display.esriSimpleFillStyle.esriSFSForwardDiagonal;
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
