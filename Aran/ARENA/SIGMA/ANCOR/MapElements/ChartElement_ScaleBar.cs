using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Xml.Serialization;
using ANCOR.MapCore;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Controls;
//using Accent.MapCore;

namespace ANCOR.MapElements
{
    [XmlType]
    [Serializable()]
    public class GraphicsChartElement_ScaleBar : GraphicsChartElement
    {
        private short _numberOfDivision;
        private short _numberOfSubDivision;
        private int _distanceBetweenNumbersAndScale;
        private AncorPoint _distanceBetweenCaptionAndScale;
        private double _divisionHeight;
        private double _subDivisionHeight;
        private AncorFont _font;
        private double _maxValue;
        private distanceUOM _maxValueUOM;
        //private AcntPoint _position;
        private double _distanceBetweenScales;
        private scaleBarPos _scaleBarPosition;
        private double _scaleWidth;
        protected Guid _id;
        private bool _isiTVerticalBar;
        private double _currentMapScale;
        private IGraphicsContainer _graphicsContainer;
        private IActiveView _activeView;


        [XmlIgnore]
        public IActiveView ActiveView
        {
            get { return _activeView; }
            set { _activeView = value; }
        }
        [XmlIgnore]
        public IGraphicsContainer GraphicsContainer
        {
            get { return _graphicsContainer; }
            set { _graphicsContainer = value; }
        }

 

        public GraphicsChartElement_ScaleBar(IGraphicsContainer _GraphicsContainer,IActiveView _ActiveView, bool itIsVerticalBar)
        {
            _numberOfDivision = 2;
            _numberOfSubDivision = 4;
            _distanceBetweenNumbersAndScale = 0;
            _divisionHeight = 10;
            _subDivisionHeight = 5;
            _maxValue = 50000;
            _maxValueUOM = distanceUOM.Kilometers;
            _font = new AncorFont("Courier New", 10);
            //_position = new AcntPoint(2, 3);
            _distanceBetweenCaptionAndScale = new AncorPoint(3.2, 0);
            _distanceBetweenScales = 2;
            _scaleBarPosition = scaleBarPos.ScaleBarAbove;
            _scaleWidth = 7;
            _isiTVerticalBar = itIsVerticalBar;
            _font = new AncorFont { Bold = false, FontColor = new AncorColor(0, 0, 0), Italic = false, Name = "Courier New", Size = 8, UnderLine = false };
            _graphicsContainer = _GraphicsContainer;
            _activeView = _ActiveView;
        }

        public GraphicsChartElement_ScaleBar(IGraphicsContainer _GraphicsContainer, IActiveView _ActiveView, double x, double y, bool itIsVerticalBar)
        {
            _numberOfDivision = 2;
            _numberOfSubDivision = 4;
            _distanceBetweenNumbersAndScale = 0;
            _divisionHeight = 10;
            _subDivisionHeight = 5;
            _maxValue = 50000;
            _maxValueUOM = distanceUOM.Kilometers;
            _font = new AncorFont("Courier New", 10);
            //_position = new AcntPoint(x, y);
            _distanceBetweenCaptionAndScale = new AncorPoint(3, 3);
            if (itIsVerticalBar) _distanceBetweenScales = 1;
            else _distanceBetweenScales = 1.4;
            _scaleBarPosition = scaleBarPos.ScaleBarAbove;
            _scaleWidth = 7;
            _isiTVerticalBar = itIsVerticalBar;
            _graphicsContainer = _GraphicsContainer;
            _activeView = _ActiveView;

        }

        public GraphicsChartElement_ScaleBar()
        {
        }

        [XmlElement]
        public short NumberOfDivision
        {
            set { _numberOfDivision = value; }
            get { return _numberOfDivision; }
        }

        [XmlElement]
        public short NumberOfSubDivision
        {
            set { _numberOfSubDivision = value; }
            get { return _numberOfSubDivision; }
        }

        [XmlElement]
        public int DistanceBetweenNumbersAndScale
        {
            set { _distanceBetweenNumbersAndScale = value; }
            get { return _distanceBetweenNumbersAndScale; }
        }

        [XmlElement]
        public AncorPoint DistanceBetweenCaptionAndScale
        {
            set { _distanceBetweenCaptionAndScale = value; }
            get { return _distanceBetweenCaptionAndScale; }
        }

        [XmlElement]
        public double DivisionHeight
        {
            set { _divisionHeight = value; }
            get { return _divisionHeight; }
        }

        [XmlElement]
        public double SubDivisionHeight
        {
            set { _subDivisionHeight = value; }
            get { return _subDivisionHeight; }
        }

        [XmlElement]
        [Browsable(true)]
        [DisplayName("Scale bar Width")]
        public double ScaleWidth
        {
            set { _scaleWidth = value; }
            get { return _scaleWidth; }
        }

        [XmlElement]
        public AncorFont Font
        {
            set { _font = value; }
            get { return _font; }
        }


        [XmlElement]
        [Browsable(false)]
        public double MaxValue
        {
            set { _maxValue = value; }
            get { return _maxValue; }
        }

        [XmlElement]
        [Browsable(true)]
        [DisplayName("UOM")]
        public distanceUOM MaxValueUOM
        {
            set { _maxValueUOM = value; }
            get { return _maxValueUOM; }
        }

        //[XmlElement]
        //[Browsable(false)]
        //public AcntPoint Position
        //{
        //    set { _position = value; }
        //    get { return _position; }
        //}

        [XmlElement]
        [Browsable(true)]
        public double DistanceBetweenScales
        {
            set { _distanceBetweenScales = value; }
            get { return _distanceBetweenScales; }
        }

        [XmlElement]
        [Browsable(false)]
        public scaleBarPos ScaleBarPosition
        {
            set { _scaleBarPosition = value; }
            get { return _scaleBarPosition; }
        }


        //[XmlAnyAttribute]
        [Browsable(false)]
        public double CurrentMapScale
        {
            get { return _currentMapScale; }
            set { _currentMapScale = value; }
        }

        [Browsable(false)]
        public bool IsiTVerticalBar
        {
            get { return _isiTVerticalBar; }
            set { _isiTVerticalBar = value; }
        }

        public IElement ConvertToIElement_Horizontal(IGraphicsContainer graphicsContainer, IActiveView _ActiveView)
        {
            try
            {
                GraphicsChartElement_ScaleBar AccentSclBr = this;

                double ScSize = 0;

                if (AccentSclBr.MaxValueUOM == distanceUOM.Metres) ScSize = AccentSclBr.MaxValue;
                if (AccentSclBr.MaxValueUOM == distanceUOM.NauticalMiles) ScSize = (AccentSclBr.MaxValue * 3.28083);
                IFrameElement frameElement = graphicsContainer.FindFrame(_ActiveView.FocusMap);
                IMapFrame mapFrame = frameElement as IMapFrame;

                //this.ScaleWidth = 70 * this.MaxValue / mapFrame.MapScale;
                this.MaxValue = this.ScaleWidth * mapFrame.MapScale / 70;

                IEnvelope envelopeDn = new EnvelopeClass();
                envelopeDn.PutCoords(AccentSclBr.Position.X, AccentSclBr.Position.Y, AccentSclBr.Position.X + ScaleWidth, AccentSclBr.Position.Y);
                IEnvelope envelopeUp = new EnvelopeClass();
                envelopeUp.PutCoords(AccentSclBr.Position.X, AccentSclBr.Position.Y + this._distanceBetweenScales, AccentSclBr.Position.X + ScaleWidth, AccentSclBr.Position.Y + this._distanceBetweenScales); // Specify the location and size of the scalebar



                ESRI.ArcGIS.esriSystem.IUID uid = new ESRI.ArcGIS.esriSystem.UIDClass();
                uid.Value = "esriCarto.AlternatingScaleBar";


                IMapSurroundFrame mapSurroundFrameUp = mapFrame.CreateSurroundFrame(uid as ESRI.ArcGIS.esriSystem.UID, null);
                IMapSurroundFrame mapSurroundFrameDn = mapFrame.CreateSurroundFrame(uid as ESRI.ArcGIS.esriSystem.UID, null);

                IElement elementUp = mapSurroundFrameUp as IElement;
                elementUp.Geometry = envelopeUp;
                elementUp.Activate(_ActiveView.ScreenDisplay);

                IElement elementDn = mapSurroundFrameDn as IElement;
                elementDn.Geometry = envelopeDn;
                elementDn.Activate(_ActiveView.ScreenDisplay);


                IStyleGallery SG = new ServerStyleGalleryClass();
                IScaleBar pScaleBarUp = null;
                IScaleBar pScaleBarDn = null;

                IStyleGalleryStorage pSGS = SG as IStyleGalleryStorage;
                string pStylePath = pSGS.DefaultStylePath + "ESRI.ServerStyle";
                pSGS.AddFile(pStylePath);
                
                IEnumStyleGalleryItem pItems = SG.get_Items("Scale Bars", pStylePath, "ScaleLine");
                pItems.Reset();
                IStyleGalleryItem pSI = pItems.Next();
                while (pSI != null)
                {
                    if (pSI.Name == "Scale Line 1")
                    {
                        pScaleBarUp = pSI.Item as IScaleBar;
                    }
                    if (pSI.Name == "Scale Line 3")
                    {
                        pScaleBarDn = pSI.Item as IScaleBar;
                    }
                    pSI = pItems.Next();
                }

                System.Runtime.InteropServices.Marshal.ReleaseComObject(pItems);

                ITextElement pTextScale = new TextElementClass();
                pTextScale.Text = (char)10+(char)13+"SCALE 1:" + Math.Round(this._currentMapScale).ToString();//mapFrame.MapScale.ToString();


                TextSymbolClass pTextSymbolScale = new TextSymbolClass();
                pTextSymbolScale.HorizontalAlignment = esriTextHorizontalAlignment.esriTHACenter;
                pTextSymbolScale.VerticalAlignment = esriTextVerticalAlignment.esriTVACenter;

                stdole.IFontDisp pFontDisp = new stdole.StdFont() as stdole.IFontDisp;
                pFontDisp.Name = AccentSclBr.Font.Name;
                pFontDisp.Bold = AccentSclBr.Font.Bold;
                pFontDisp.Italic = AccentSclBr.Font.Italic;
                pFontDisp.Underline = AccentSclBr.Font.UnderLine;
                pFontDisp.Size = (decimal)AccentSclBr.Font.Size;

                IRgbColor FntClr = new RgbColorClass();
                FntClr.Red = this.Font.FontColor.Red;
                FntClr.Green = this.Font.FontColor.Green;
                FntClr.Blue = this.Font.FontColor.Blue;

                pTextSymbolScale.Color = FntClr;

                IPoint Pnt = new PointClass();
                Pnt.PutCoords(AccentSclBr.Position.X + AccentSclBr.DistanceBetweenCaptionAndScale.X, AccentSclBr.Position.Y + AccentSclBr.DistanceBetweenCaptionAndScale.Y);

                pTextSymbolScale.Font = pFontDisp;
                pTextScale.Symbol = pTextSymbolScale;


                IElement pTextElementScale = pTextScale as IElement;
                pTextElementScale.Geometry = Pnt;

                IScaleMarks Sclmrks = (IScaleMarks)pScaleBarUp;
                Sclmrks.DivisionMarkHeight = AccentSclBr.DivisionHeight;
                Sclmrks.SubdivisionMarkHeight = AccentSclBr.SubDivisionHeight;

                Sclmrks = (IScaleMarks)pScaleBarDn;
                Sclmrks.DivisionMarkHeight = AccentSclBr.DivisionHeight;
                Sclmrks.SubdivisionMarkHeight = AccentSclBr.SubDivisionHeight;


                TextSymbol pTextSymbol = new TextSymbolClass();
                pTextSymbol.HorizontalAlignment = esriTextHorizontalAlignment.esriTHACenter;
                pTextSymbol.VerticalAlignment = esriTextVerticalAlignment.esriTVACenter;

                pTextSymbol.Font = pFontDisp;
                pTextSymbol.Color = FntClr;

                pScaleBarUp.LabelSymbol = pTextSymbol;
                pScaleBarDn.LabelSymbol = pTextSymbol;

                pScaleBarUp.Divisions = AccentSclBr.NumberOfDivision;
                pScaleBarUp.Subdivisions = AccentSclBr.NumberOfSubDivision;
                pScaleBarUp.LabelGap = AccentSclBr.DistanceBetweenNumbersAndScale;
                pScaleBarUp.LabelPosition = esriVertPosEnum.esriAbove;
                pScaleBarUp.Map = _ActiveView.FocusMap;
                pScaleBarUp.UnitLabelGap = 10;

                if (this.ScaleBarPosition == scaleBarPos.ScaleBarAbove)
                {
                    pScaleBarUp.UnitLabelPosition = esriScaleBarPos.esriScaleBarAbove;
                    pScaleBarDn.UnitLabelPosition = esriScaleBarPos.esriScaleBarBelow;
                }

                if (this.MaxValueUOM == distanceUOM.Kilometers)
                {
                    pScaleBarUp.Units = ESRI.ArcGIS.esriSystem.esriUnits.esriKilometers;
                    pScaleBarUp.UnitLabel = "Km";
                    pScaleBarDn.UnitLabel = "NM";
                    pScaleBarDn.Units = ESRI.ArcGIS.esriSystem.esriUnits.esriNauticalMiles;
                }
                if (this.MaxValueUOM == distanceUOM.Metres)
                {
                    pScaleBarUp.Units = ESRI.ArcGIS.esriSystem.esriUnits.esriMeters;
                    pScaleBarUp.UnitLabel = "METRES";
                    pScaleBarDn.UnitLabel = "FEET";
                    pScaleBarDn.Units = ESRI.ArcGIS.esriSystem.esriUnits.esriFeet;
                }
                if (this.MaxValueUOM == distanceUOM.Feets)
                {
                    pScaleBarUp.Units = ESRI.ArcGIS.esriSystem.esriUnits.esriFeet;
                    pScaleBarUp.UnitLabel = "FEET";
                    pScaleBarDn.UnitLabel = "METRES";
                    pScaleBarDn.Units = ESRI.ArcGIS.esriSystem.esriUnits.esriMeters;
                }
                if (this.MaxValueUOM == distanceUOM.NauticalMiles)
                {
                    pScaleBarUp.Units = ESRI.ArcGIS.esriSystem.esriUnits.esriNauticalMiles;
                    pScaleBarUp.UnitLabel = "NM";
                    pScaleBarDn.UnitLabel = "Km";
                    pScaleBarDn.Units = ESRI.ArcGIS.esriSystem.esriUnits.esriKilometers;
                }

                pScaleBarUp.UnitLabelSymbol = pTextSymbol;
                pScaleBarDn.UnitLabelSymbol = pTextSymbol;


                pScaleBarDn.LabelSymbol.Font = pFontDisp;
                pScaleBarUp.LabelSymbol.Font = pFontDisp;
                pScaleBarUp.LabelSymbol.Color = FntClr;

                pScaleBarUp.LabelFrequency = esriScaleBarFrequency.esriScaleBarDivisionsAndFirstMidpoint;

                pScaleBarDn.Divisions = AccentSclBr.NumberOfDivision;
                pScaleBarDn.Subdivisions = AccentSclBr.NumberOfSubDivision;
                //pScaleBarDn.Divisions = 1;
                pScaleBarDn.LabelGap = AccentSclBr.DistanceBetweenNumbersAndScale;
                pScaleBarDn.LabelPosition = esriVertPosEnum.esriBelow;
                pScaleBarDn.Map = _ActiveView.FocusMap;
                pScaleBarDn.UnitLabelGap = 20;
                pScaleBarDn.LabelFrequency = esriScaleBarFrequency.esriScaleBarDivisionsAndFirstMidpoint;



                IMapSurround mapSurroundUp = pScaleBarUp;
                mapSurroundFrameUp.MapSurround = mapSurroundUp;

                IMapSurround mapSurroundDn = pScaleBarDn;
                mapSurroundFrameDn.MapSurround = mapSurroundDn;


                IGroupElement GrpEl = new GroupElementClass();
                GrpEl.AddElement(pTextElementScale);

                IGraphicsComposite graphicsComposite = mapSurroundUp as IGraphicsComposite;
                IEnumElement enumElemUP = graphicsComposite.get_Graphics(_ActiveView.ScreenDisplay, (mapSurroundFrameUp as IElement).Geometry.Envelope);

                enumElemUP.Reset();
                IElement element = enumElemUP.Next();
                while (element != null)
                {
                    GrpEl.AddElement(element);
                    element = enumElemUP.Next();
                }


                graphicsComposite = mapSurroundDn as IGraphicsComposite;
                IEnumElement enumElemDN = graphicsComposite.get_Graphics(_ActiveView.ScreenDisplay, (mapSurroundFrameDn as IElement).Geometry.Envelope);

                enumElemDN.Reset();
                element = enumElemDN.Next();
                while (element != null)
                {
                    GrpEl.AddElement(element);
                    element = enumElemDN.Next();
                }




                return (IElement)GrpEl;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public IElement ConvertToIElement_Vertical(IGraphicsContainer graphicsContainer, IActiveView _ActiveView)
        {
            try
            {
                GraphicsChartElement_ScaleBar AccentSclBr = this;


                double _MScl = _ActiveView.FocusMap.MapScale;
                _ActiveView.FocusMap.MapScale = _ActiveView.FocusMap.MapScale / 10;

                double ScSize = 0;

                if (AccentSclBr.MaxValueUOM == distanceUOM.Metres) ScSize = AccentSclBr.MaxValue;
                if (AccentSclBr.MaxValueUOM == distanceUOM.NauticalMiles) ScSize = (AccentSclBr.MaxValue * 3.28083);
                IFrameElement frameElement = graphicsContainer.FindFrame(_ActiveView.FocusMap);
                IMapFrame mapFrame = frameElement as IMapFrame;

                this.ScaleWidth = 7 * 10 * this.MaxValue / mapFrame.MapScale;
                IEnvelope envelopeDn = new EnvelopeClass();
                envelopeDn.PutCoords(AccentSclBr.Position.X - ScaleWidth / 2, AccentSclBr.Position.Y, AccentSclBr.Position.X + ScaleWidth / 2, AccentSclBr.Position.Y);
                IEnvelope envelopeUp = new EnvelopeClass();
                envelopeUp.PutCoords(AccentSclBr.Position.X - ScaleWidth / 2, AccentSclBr.Position.Y + this._distanceBetweenScales, AccentSclBr.Position.X + ScaleWidth / 2, AccentSclBr.Position.Y + this._distanceBetweenScales); // Specify the location and size of the scalebar



                ESRI.ArcGIS.esriSystem.IUID uid = new ESRI.ArcGIS.esriSystem.UIDClass();
                uid.Value = "esriCarto.AlternatingScaleBar";


                IMapSurroundFrame mapSurroundFrameUp = mapFrame.CreateSurroundFrame(uid as ESRI.ArcGIS.esriSystem.UID, null);
                IMapSurroundFrame mapSurroundFrameDn = mapFrame.CreateSurroundFrame(uid as ESRI.ArcGIS.esriSystem.UID, null);


                IElement elementUp = mapSurroundFrameUp as IElement;
                elementUp.Geometry = envelopeUp;
                elementUp.Activate(_ActiveView.ScreenDisplay);

                IElement elementDn = mapSurroundFrameDn as IElement;
                elementDn.Geometry = envelopeDn;
                elementDn.Activate(_ActiveView.ScreenDisplay);

                IStyleGallery SG = new ServerStyleGalleryClass();
                IScaleBar pScaleBarUp = null;
                IScaleBar pScaleBarDn = null;

                IStyleGalleryStorage pSGS = SG as IStyleGalleryStorage;
                string pStylePath = pSGS.DefaultStylePath + "ESRI.ServerStyle";
                pSGS.AddFile(pStylePath);

                IEnumStyleGalleryItem pItems = SG.get_Items("Scale Bars", pStylePath, "ScaleLine");
                pItems.Reset();
                IStyleGalleryItem pSI = pItems.Next();
                while (pSI != null)
                {
                    if (pSI.Name == "Scale Line 1")
                    {
                        pScaleBarUp = pSI.Item as IScaleBar;
                    }
                    if (pSI.Name == "Scale Line 3")
                    {
                        pScaleBarDn = pSI.Item as IScaleBar;
                    }
                    pSI = pItems.Next();
                }

                System.Runtime.InteropServices.Marshal.ReleaseComObject(pItems);

                ITextElement pTextScale = new TextElementClass();
                pTextScale.Text = "1:";


                TextSymbol pTextSymbolScale = new TextSymbolClass();
                pTextSymbolScale.HorizontalAlignment = esriTextHorizontalAlignment.esriTHACenter;
                pTextSymbolScale.VerticalAlignment = esriTextVerticalAlignment.esriTVACenter;

                stdole.IFontDisp pFontDisp = new stdole.StdFont() as stdole.IFontDisp;
                pFontDisp.Name = AccentSclBr.Font.Name;
                pFontDisp.Bold = AccentSclBr.Font.Bold;
                pFontDisp.Italic = AccentSclBr.Font.Italic;
                pFontDisp.Underline = AccentSclBr.Font.UnderLine;
                pFontDisp.Size = (decimal)AccentSclBr.Font.Size;


                IRgbColor FntClr = new RgbColorClass();
                FntClr.Red = this.Font.FontColor.Red;
                FntClr.Green = this.Font.FontColor.Green;
                FntClr.Blue = this.Font.FontColor.Blue;

                pTextSymbolScale.Color = FntClr;
                pTextSymbolScale.Font = pFontDisp;
                pTextScale.Symbol = pTextSymbolScale;

                IPoint Pnt = new PointClass();
                Pnt.PutCoords(0, 0);


                IElement pTextElementScale = pTextScale as IElement;
                pTextElementScale.Geometry = Pnt;

                ITransform2D trans = (ITransform2D)pTextElementScale;
                trans.Move(AccentSclBr.Position.X + AccentSclBr.DistanceBetweenCaptionAndScale.X, AccentSclBr.Position.Y + envelopeDn.YMin + AccentSclBr.DistanceBetweenCaptionAndScale.Y);

                trans = (ITransform2D)pTextElementScale;


                IScaleMarks Sclmrks = (IScaleMarks)pScaleBarUp;
                Sclmrks.DivisionMarkHeight = AccentSclBr.DivisionHeight;
                Sclmrks.SubdivisionMarkHeight = AccentSclBr.SubDivisionHeight;

                Sclmrks = (IScaleMarks)pScaleBarDn;
                Sclmrks.DivisionMarkHeight = AccentSclBr.DivisionHeight;
                Sclmrks.SubdivisionMarkHeight = AccentSclBr.SubDivisionHeight;


                TextSymbolClass pTextSymbol = new TextSymbolClass();
                pTextSymbol.HorizontalAlignment = esriTextHorizontalAlignment.esriTHACenter;
                pTextSymbol.VerticalAlignment = esriTextVerticalAlignment.esriTVACenter;

                pTextSymbol.Font = pFontDisp;
                pTextSymbol.Color = FntClr;


                pTextSymbol.Angle = 270;

                pTextSymbol.HorizontalAlignment = esriTextHorizontalAlignment.esriTHACenter;
                pTextSymbol.VerticalAlignment = esriTextVerticalAlignment.esriTVABaseline;
                pTextSymbol.XOffset = -15;
                pTextSymbol.YOffset = -3;

                pScaleBarUp.LabelSymbol = pTextSymbol;


                pTextSymbol.XOffset = 15;
                pTextSymbol.YOffset = 6;
                pScaleBarDn.LabelSymbol = pTextSymbol;
                pScaleBarDn.LabelSymbol.Color = FntClr;

                pScaleBarUp.Divisions = AccentSclBr.NumberOfDivision;
                pScaleBarUp.Subdivisions = AccentSclBr.NumberOfSubDivision;

                pScaleBarUp.LabelGap = AccentSclBr.DistanceBetweenNumbersAndScale;
                pScaleBarUp.LabelPosition = esriVertPosEnum.esriTop;
                pScaleBarUp.Map = _ActiveView.FocusMap;
                pScaleBarUp.UnitLabelGap = 12;


                pScaleBarUp.UnitLabelPosition = esriScaleBarPos.esriScaleBarAfterBar;
                pScaleBarDn.UnitLabelPosition = esriScaleBarPos.esriScaleBarAfterBar;

                pScaleBarUp.Units = ESRI.ArcGIS.esriSystem.esriUnits.esriMeters;
                pScaleBarUp.UnitLabel = "METRES";
                pScaleBarDn.UnitLabel = "FEET";
                pScaleBarDn.Units = ESRI.ArcGIS.esriSystem.esriUnits.esriFeet;

                pTextSymbol.XOffset = -50;
                pScaleBarUp.UnitLabelSymbol = pTextSymbol;

                pTextSymbol.XOffset = 5;
                pScaleBarDn.UnitLabelSymbol = pTextSymbol;
                pScaleBarUp.LabelFrequency = esriScaleBarFrequency.esriScaleBarDivisionsAndFirstMidpoint;

                pScaleBarDn.Divisions = AccentSclBr.NumberOfDivision;
                pScaleBarDn.Subdivisions = AccentSclBr.NumberOfSubDivision;
                pScaleBarDn.LabelGap = AccentSclBr.DistanceBetweenNumbersAndScale;
                pScaleBarDn.LabelPosition = esriVertPosEnum.esriBelow;
                pScaleBarDn.Map = _ActiveView.FocusMap;
                pScaleBarDn.UnitLabelGap = 10;
                pScaleBarDn.LabelFrequency = esriScaleBarFrequency.esriScaleBarDivisionsAndFirstMidpoint;

                IMapSurround mapSurroundUp = pScaleBarUp;
                mapSurroundFrameUp.MapSurround = mapSurroundUp;

                IMapSurround mapSurroundDn = pScaleBarDn;
                mapSurroundFrameDn.MapSurround = mapSurroundDn;

                IGroupElement GrpEl = new GroupElementClass();

                IGraphicsComposite graphicsComposite = mapSurroundUp as IGraphicsComposite;


                IEnumElement enumElemUP = graphicsComposite.get_Graphics(_ActiveView.ScreenDisplay, (mapSurroundFrameUp as IElement).Geometry.Envelope);

                enumElemUP.Reset();
                IElement element = enumElemUP.Next();
                while (element != null)
                {
                    if (element is IGroupElement)
                    {
                        for (int i = 0; i <= (element as IGroupElement).ElementCount - 1; i++)
                        {
                            IElement elll = (element as IGroupElement).get_Element(i);
                            ITransform2D TT2 = (ITransform2D)elll;
                            IPoint AnchorPoint = new PointClass();
                            AnchorPoint.PutCoords(this.Position.X, this.Position.Y);

                            TT2.Rotate(AnchorPoint, -3 * Math.PI / 2);
                            GrpEl.AddElement(elll);
                        }
                    }
                    else
                    {
                        ITransform2D TT2 = (ITransform2D)element;
                        IPoint AnchorPoint = new PointClass();
                        AnchorPoint.PutCoords(this.Position.X, this.Position.Y);

                        TT2.Rotate(AnchorPoint, -3 * Math.PI / 2);
                        GrpEl.AddElement(element);

                    }
                    element = enumElemUP.Next();
                }


                graphicsComposite = mapSurroundDn as IGraphicsComposite;
                IEnumElement enumElemDN = graphicsComposite.get_Graphics(_ActiveView.ScreenDisplay, (mapSurroundFrameDn as IElement).Geometry.Envelope);

                enumElemDN.Reset();
                element = enumElemDN.Next();
                while (element != null)
                {
                    if (element is IGroupElement)
                    {
                        for (int i = 0; i <= (element as IGroupElement).ElementCount - 1; i++)
                        {
                            IElement elll = (element as IGroupElement).get_Element(i);
                            ITransform2D TT2 = (ITransform2D)elll;
                            IPoint AnchorPoint = new PointClass();
                            AnchorPoint.PutCoords(this.Position.X, this.Position.Y);

                            TT2.Rotate(AnchorPoint, -3 * Math.PI / 2);
                            GrpEl.AddElement(elll);
                        }
                    }
                    else
                    {
                        ITransform2D TT2 = (ITransform2D)element;
                        IPoint AnchorPoint = new PointClass();
                        AnchorPoint.PutCoords(this.Position.X, this.Position.Y);

                        TT2.Rotate(AnchorPoint, -3 * Math.PI / 2);
                        GrpEl.AddElement(element);

                    }
                    element = enumElemDN.Next();
                }


                _ActiveView.FocusMap.MapScale = _MScl;

                return (IElement)GrpEl;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }



        public override object ConvertToIElement()
        {
            IElement res = null;
            try
            {
                if (this.IsiTVerticalBar) res = ConvertToIElement_Vertical(this.GraphicsContainer, this.ActiveView);
                else res = ConvertToIElement_Horizontal(this.GraphicsContainer, this.ActiveView);

                return res;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }


    }

}
