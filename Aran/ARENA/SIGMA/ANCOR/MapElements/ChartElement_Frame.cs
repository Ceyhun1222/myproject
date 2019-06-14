using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms.Design;
using System.Xml.Serialization;
using System.Windows.Forms;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Display;
using ANCOR.MapCore;



namespace ANCOR.MapElements
{
    [XmlType]
    [Serializable()]
    public class   ChartElement_Frame : GraphicsChartElement 
    {

        public  ChartElement_Frame()
        {
        }

        public ChartElement_Frame(AncorPoint mapCntr)
        {
           // base.Id = Guid.NewGuid();
            MapCntr = new AncorPoint(mapCntr.X, mapCntr.Y);
            _frameWidth = 5;
            _frameHeight = 5;
            _frameLineWidth = 1;
            _frameName = "";
            _frameLineColor = new AncorColor(0,0,0);
            _position = new AncorPoint(0,0);
        }


        private double _frameLineWidth;
        private double _frameWidth;
        private double _frameHeight;
        private string _frameName;
        public AncorPoint MapCntr;
        private AncorColor _frameLineColor;
        private AncorPoint _position;




        [XmlElement]
        [ReadOnly(false)]
        [Browsable(false)]
        public AncorPoint Position
        {
            get { return _position; }
            set { _position = value; }
        }


        [XmlElement]
        public AncorColor FrameLineColor
        {
            get { return _frameLineColor; }
            set { _frameLineColor = value; }
        }

        [XmlElement]
        [Browsable(false)]
        public string FrameName
        {
            get { return _frameName; }
            set { _frameName = value; }
        }

        [XmlElement]
        public double FrameHeight
        {
            get { return _frameHeight; }
            set { _frameHeight = value; }
        }

        [XmlElement]
        public double FrameWidth
        {
            get { return _frameWidth; }
            set { _frameWidth = value; }
        }


        [XmlElement]
        public double FrameLineWidth
        {
            get { return _frameLineWidth; }
            set { _frameLineWidth = value; }
        }


        public object ConvertToIElement(IMap FocusMap, IEnvelope envelope, bool LoadAllLayers = true)
        {

            IMap pMap = new MapClass();
            pMap.Name = this.FrameName;

            if (LoadAllLayers)
            {
                for (int i = FocusMap.LayerCount - 1; i >= 0; i--)
                {
                    ILayer L = FocusMap.get_Layer(i);
                    if (L is ICompositeLayer)
                    {
                        pMap.AddLayer(L);
                        for (int j = 0; j <= ((ICompositeLayer)L).Count - 1; j++)
                        {

                            ILayer L1 = ((ICompositeLayer)L).get_Layer(j);

                            if (L1 is ICompositeLayer)
                            {
                                pMap.AddLayer(L1);
                                for (int k = 0; k <= ((ICompositeLayer)L1).Count - 1; k++)
                                {
                                    ILayer L2 = ((ICompositeLayer)L1).get_Layer(k);
                                    L2.Visible = ((ICompositeLayer)L1).get_Layer(k).Visible;
                                    pMap.AddLayer(L2);
                                }
                            }
                            else
                            {
                                L1.Visible = L.Visible;
                                pMap.AddLayer(L1);
                            }
                        }
                    }
                    else
                    {
                        pMap.AddLayer(L);
                    }

                }
            }


            pMap.MapScale = FocusMap.MapScale;
            pMap.SpatialReference = FocusMap.SpatialReference;
            pMap.DistanceUnits = FocusMap.DistanceUnits;

            IMapFrame pMapFrame = new MapFrameClass();
            pMapFrame.Map = pMap;
            pMapFrame.MapScale = FocusMap.MapScale;
           

            #region Background

            ISymbolBackground smlBckGrnd = new SymbolBackgroundClass();

            ISimpleFillSymbol smplFill = new SimpleFillSymbol();

            IRgbColor rgbClr = new RgbColor();  //цвет заполнения
            rgbClr.Red = 255;
            rgbClr.Blue = 255;
            rgbClr.Green = 255;

            smplFill.Color = rgbClr;

            //стиль заполнения
            smplFill.Style = esriSimpleFillStyle.esriSFSSolid;

            smlBckGrnd.FillSymbol = smplFill as IFillSymbol;
            pMapFrame.Background = smlBckGrnd;

            #endregion

            #region border

            IBorder pBorder = pMapFrame.Border;
            ISymbolBorder pSymBorder = pMapFrame.Border as ISymbolBorder;
            ILineSymbol pLineSymbol = pSymBorder.LineSymbol;
            pLineSymbol.Width = this.FrameLineWidth;


            IRgbColor rgbClrBorderLine = new RgbColor();  //цвет линии
            rgbClrBorderLine.Red = this.FrameLineColor.Red;
            rgbClrBorderLine.Blue = this.FrameLineColor.Blue;
            rgbClrBorderLine.Green = this.FrameLineColor.Green;

            pLineSymbol.Color = rgbClrBorderLine;
            pSymBorder.LineSymbol = pLineSymbol;
            pMapFrame.Border = (IBorder)pSymBorder;


            #endregion


            IPoint cntr = new PointClass();
            cntr.PutCoords(MapCntr.X, MapCntr.Y);


            #region

            (pMapFrame.Map as ESRI.ArcGIS.Carto.IActiveView).ScreenDisplay.DisplayTransformation.Rotation = (FocusMap as ESRI.ArcGIS.Carto.IActiveView).ScreenDisplay.DisplayTransformation.Rotation;

            #endregion

            // Create an element object to hold the frame 
            IElement pElement = (IElement)pMapFrame;
            //IEnvelope pEnv = new EnvelopeClass();
            //pEnv.PutCoords(this.Position.X, this.Position.Y, this.Position.X + this._frameWidth, this.Position.Y + this._frameHeight);

            pElement.Geometry = envelope;//pEnv;

            (pElement as IElementProperties3).Name = this.FrameName;
            pElement.Activate((pMapFrame.Map as IActiveView).ScreenDisplay);
            //(pElement as IElementProperties3).Name = this.ID.ToString();
            return pElement;

        }
    }
}
