using Aran.AranEnvironment;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MapEnv
{
    internal class AranLayoutViewGraphics : IAranLayoutViewGraphics
    {
        public AranLayoutViewGraphics()
        {
            Text = new List<string>();
        }

        private List<int> _drawObjects = new List<int>();
        private IAranDrawing _aranDrawing;
        private AxPageLayoutControl _axPageLayoutControl;
        private bool _layoutCreated;
        private int _headersCount = 0;

        private IGraphicsContainer GraphicContainer
        {
            get
            {
                return _axPageLayoutControl.ActiveView.GraphicsContainer;
            }
        }
        private IMap Map
        {
            get
            {
                return _axPageLayoutControl.ActiveView.FocusMap;
            }
        }
        private IActiveView ActiveView
        {
            get
            {
                return _axPageLayoutControl.ActiveView;
            }
        }
        private IMapFrame MapFrame
        {
            get
            {
                return FrameElement as IMapFrame;
            }
        }
        private IFrameElement FrameElement
        {
            get
            {
                return GraphicContainer.FindFrame(Map);
            }
        }
        private IPageLayout PageLayout
        {
            get
            {
                return _axPageLayoutControl.PageLayout;
            }
        }
        private IPage Page
        {
            get
            {
                return PageLayout.Page;
            }
        }
        private IEnvelope Envelope { get; set; }

        private IElement ScaleBar { get; set; }
        private IElement Title { get; set; }

        private int _scale = 1;
        public int Scale
        {
            get
            {
                return _scale;
            }

            set
            {
                if (value < 1)
                    _scale = 1;
            }
        }

        public List<String> Text { get; set; }
        public bool Visible { get; set; }
        public void Draw()
        {
            Clean();
            if (Visible)
            {
                draw();
            }
        }
        public void Refresh()
        {
            Clean();
            PrepareLayout();
            if (Visible)
                draw();
        }
        public void PrepareLayout()
        {
            if (Visible)
            {
                createLayout();
                createMapGrid();
                createScaleBar();
            }
            else {
                //resetLayout();
                deleteMapGrids();
                deleteScaleBar();
            }
        }

        public void Clean()
        {
            if (_aranDrawing != null)
                for (int i = 0; i < _drawObjects.Count; i++)
                {
                    _aranDrawing.DeleteGraphic(_drawObjects[i]);
                }
            _drawObjects.Clear();
        }

        public void setLayoutControl(AxPageLayoutControl axLayoutControl)
        {
            _axPageLayoutControl = axLayoutControl;
            _aranDrawing = new AranDrawing(axLayoutControl);
        }

        private void createLayout()
        {
            if (_layoutCreated)
                return;
            createMapFrameGeomtry(1, 1, ActiveView.Extent.XMax - 1 + ActiveView.Extent.XMin, ActiveView.Extent.YMax - 2);
            _layoutCreated = true;
        }

        private void resetLayout()
        {
            if (!_layoutCreated)
                return;
            createMapFrameGeomtry(1, 1, ActiveView.Extent.XMax - 1 + ActiveView.Extent.XMin, ActiveView.Extent.YMax - 1.5);
            _layoutCreated = false;
        }

        private void createMapFrameGeomtry(double xMin, double yMin, double xMax, double yMax)
        {
            Envelope = new EnvelopeClass();
            Envelope.PutCoords(xMin, yMin, xMax, yMax);
            (MapFrame as IElement).Geometry = Envelope;
        }

        private void createScaleBar()
        {
            deleteScaleBar();

            IElement element = GraphicContainer.Next();
            IElement mainMapElement = MapFrame as IElement;
            IGeometry geometry = mainMapElement.Geometry;
            IEnvelope mainMapEnvelope = geometry.Envelope;
            IEnvelope envelope = new EnvelopeClass();
            double xMin = mainMapEnvelope.XMin;
            double yMin = 0.25;
            double xMax = mainMapEnvelope.XMax;
            double yMax = 0.45; ;
            var length = Math.Round((3.8/2.54), 2);
            envelope.PutCoords((xMax + xMin) / 2 - length, yMin, (xMax + xMin) / 2 + length, yMax);

            ESRI.ArcGIS.esriSystem.IUID uid = new ESRI.ArcGIS.esriSystem.UIDClass();
            uid.Value = "esriCarto.ScaleText";
            ESRI.ArcGIS.Carto.IMapSurroundFrame mapSurroundFrame = MapFrame.CreateSurroundFrame(uid as ESRI.ArcGIS.esriSystem.UID, null); // Dynamic Cast
            ScaleBar = mapSurroundFrame as ESRI.ArcGIS.Carto.IElement; // Dynamic Cast
            ScaleBar.Geometry = envelope;

            ScaleBar.Activate(ActiveView.ScreenDisplay);
            GraphicContainer.AddElement(ScaleBar, 0);

            ESRI.ArcGIS.Carto.IMapSurround mapSurround = mapSurroundFrame.MapSurround;

            IScaleText markerScaleText = ((ESRI.ArcGIS.Carto.IScaleText)(mapSurround));
            markerScaleText.MapUnits = ESRI.ArcGIS.esriSystem.esriUnits.esriMeters;
            markerScaleText.PageUnits = ESRI.ArcGIS.esriSystem.esriUnits.esriCentimeters;
            //markerScaleBar.LabelPosition = ESRI.ArcGIS.Carto.esriVertPosEnum.esriTop;
            //markerScaleBar.UseMapSettings();
        }

        private void deleteScaleBar()
        {
            if (ScaleBar != null)
                GraphicContainer.DeleteElement(ScaleBar);
        }

        private void createMapGrid()
        {
            IMapGrid mapGrid = new GraticuleClass();
            mapGrid.Name = "Map Grid";

            //Create a color.
            IColor color = new RgbColorClass();
            color.RGB = 0X000000;

            //Set the line symbol used to draw the grid.
            ICartographicLineSymbol cartographicLineSymbol = new CartographicLineSymbolClass();
            cartographicLineSymbol.Cap = esriLineCapStyle.esriLCSButt;
            cartographicLineSymbol.Width = 2;
            cartographicLineSymbol.Color = color;
            mapGrid.LineSymbol = cartographicLineSymbol as ILineSymbol;
            mapGrid.Border = null; // Clear the default frame border.

            //Set the Tick properties.
            mapGrid.TickLength = 15;
            cartographicLineSymbol = new CartographicLineSymbolClass();
            cartographicLineSymbol.Cap = esriLineCapStyle.esriLCSButt;
            cartographicLineSymbol.Width = 1;
            cartographicLineSymbol.Color = color;
            mapGrid.TickLineSymbol = cartographicLineSymbol as ILineSymbol;
            mapGrid.TickMarkSymbol = null;

            //Set the SubTick properties.
            mapGrid.SubTickCount = 5;
            mapGrid.SubTickLength = 10;
            cartographicLineSymbol = new CartographicLineSymbolClass();
            cartographicLineSymbol.Cap = esriLineCapStyle.esriLCSButt;
            cartographicLineSymbol.Width = 0.2;
            cartographicLineSymbol.Color = color;
            mapGrid.SubTickLineSymbol = cartographicLineSymbol as ILineSymbol;

            // Set the Grid label properties.
            IGridLabel gridLabel = mapGrid.LabelFormat;
            gridLabel.LabelOffset = 15;

            //Set the Tick, SubTick, and Label Visibility along the four sides of the grid.
            mapGrid.SetTickVisibility(true, true, true, true);
            mapGrid.SetSubTickVisibility(true, true, true, true);
            mapGrid.SetLabelVisibility(true, true, true, true);

            //Make the map grid visible so it gets drawn when Active View is updated.
            mapGrid.Visible = true;

            //Set the IMeasuredGrid properties.
            IMeasuredGrid measuredGrid = mapGrid as IMeasuredGrid;
            measuredGrid.FixedOrigin = true;
            measuredGrid.XIntervalSize = 1; //Meridian interval.
            //measuredGrid.XOrigin = 0; //Shift grid 5°.
            measuredGrid.YIntervalSize = 1; //Parallel interval.
                                            //measuredGrid.YOrigin = 0; //Shift grid 5°.

            IMapGrids mapGrids = MapFrame as IMapGrids;
            mapGrids.AddMapGrid(mapGrid);
        }
        private void deleteMapGrids()
        {
            IMapGrids mapGrids = MapFrame as IMapGrids;
            mapGrids.ClearMapGrids();
        }

        private void draw()
        {
            if (_aranDrawing != null && Text != null)
                for (int i = 0; i < Text.Count; i++)
                    draw(Text[i], i + 1);
        }
        private void draw(String text, int row)
        {
            if (text != null && !text.Equals(""))
                _drawObjects.Add(_aranDrawing.DrawPointWithText(new Aran.Geometries.Point(1, ActiveView.Extent.YMax - 1 - (row - 1) * 0.25), text, new Aran.AranEnvironment.Symbols.PointSymbol(0, 1)));
            else
                _drawObjects.Add(_aranDrawing.DrawPointWithText(new Aran.Geometries.Point(1, ActiveView.Extent.YMax - 1 - (row - 1) * 0.25), "", new Aran.AranEnvironment.Symbols.PointSymbol(0, 1)));
        }

    }
}
