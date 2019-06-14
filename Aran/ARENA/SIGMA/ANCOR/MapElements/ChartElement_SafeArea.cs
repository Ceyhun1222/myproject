using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Xml.Serialization;
using System.Drawing.Design;
using ANCOR.MapCore;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Controls;
using GeometryFunctions;

namespace ANCOR.MapElements
{
    [XmlType]
    [Serializable()]
    public class GraphicsChartElement_SafeArea : GraphicsChartElement
    {
        private double DefaultmsaSignRadius_mm = 18;

        public GraphicsChartElement_SafeArea()
        {
        }

        public GraphicsChartElement_SafeArea(string msaName)
        {
            _msaSignRadius_mm = DefaultmsaSignRadius_mm;
            _valDistOuter = 20;
            _valDistOuterUOM = distanceUOM.Kilometers.ToString();
            _fillColor = new AncorColor (255,255,255);
            _lineColor = new AncorColor (0,0,0);
            _flStl = fillStyle.fSNull;
            _lnStl = lineStyle.lsSolid;
            //_position = new AcntPoint(10, 10);
            _msaName = "MSA NAME";
            _lineWidth = 1;
            _magVar = 0;
            _fntInner = new AncorFont { Bold = false, FontColor = new AncorColor(0, 0, 0), Italic = false, Name ="Arial", Size = 8, UnderLine = false };
            _fntArrow = new AncorFont { Bold = false, FontColor = new AncorColor(0, 0, 0), Italic = false, Name ="Arial", Size = 8, UnderLine = false };
            _fntUnder = new AncorFont { Bold = false, FontColor = new AncorColor(0, 0, 0), Italic = false, Name ="Arial", Size = 8, UnderLine = false };
            _offset = 35;
            _arrowSize_pnt = 40;
            _spaceValue = 0.2;
            _wordSpacing = 100;

            this.TextContents = new List<List<AncorChartElementWord>>();
            List<AncorChartElementWord> txtLine = new List<AncorChartElementWord>(); // создаем строку
            AncorChartElementWord wrd = new AncorChartElementWord("MsaName", _fntInner);//создаем слово
            //wrd.TextValue = "value";
            wrd.Font.Bold = true;
            wrd.StartSymbol = new AncorSymbol("");
            wrd.EndSymbol = new AncorSymbol("");
            //wrd.Morse = true;
            txtLine.Add(wrd);
            this.TextContents.Add(txtLine);  // добавим его в строку


        }

        public GraphicsChartElement_SafeArea(double valDistOuter, string valDistOuterUOM, string msaName)
        {
            _msaSignRadius_mm = DefaultmsaSignRadius_mm;
            _valDistOuter = valDistOuter;
            _valDistOuterUOM = valDistOuterUOM;
            _fillColor = new AncorColor (255,255,255);
            _lineColor = new AncorColor (0,0,0);
            _flStl = fillStyle.fSNull;
            _lnStl = lineStyle.lsSolid;
            //_position = new AcntPoint(10, 10);
            _msaName = msaName;
            _lineWidth = 1;
            _magVar = 0;
            _fntInner = new AncorFont { Bold = false, FontColor = new AncorColor(0, 0, 0), Italic = false, Name ="Arial", Size = 8, UnderLine = false };
            _fntArrow = new AncorFont { Bold = false, FontColor = new AncorColor(0, 0, 0), Italic = false, Name ="Arial", Size = 8, UnderLine = false };
            _fntUnder = new AncorFont { Bold = false, FontColor = new AncorColor(0, 0, 0), Italic = false, Name ="Arial", Size = 8, UnderLine = false };
            _offset = 20;
            _arrowSize_pnt = 40;
            _sectors = new List<SafeArea_SectorDescription>();
            _spaceValue = 0.2;
            _wordSpacing = 100;

            this.TextContents = new List<List<AncorChartElementWord>>();
            List<AncorChartElementWord> txtLine = new List<AncorChartElementWord>(); // создаем строку
            AncorChartElementWord wrd = new AncorChartElementWord(valDistOuter.ToString(), _fntInner);//создаем слово
            //wrd.TextValue = "value";
            wrd.Font.Bold = true;
            wrd.StartSymbol = new AncorSymbol("");
            wrd.EndSymbol = new AncorSymbol("");
            //wrd.Morse = true;
            txtLine.Add(wrd); // добавим его в строку

            wrd = new AncorChartElementWord(valDistOuterUOM, _fntInner);//создаем слово
            //wrd.TextValue = "value";
            wrd.Font.Bold = true;
            wrd.StartSymbol = new AncorSymbol("");
            wrd.EndSymbol = new AncorSymbol("");
            //wrd.Morse = true;
            txtLine.Add(wrd); // добавим его в строку
            this.TextContents.Add(txtLine);  


            txtLine = new List<AncorChartElementWord>(); // создаем строку

            wrd = new AncorChartElementWord(msaName, _fntInner);//создаем слово
            //wrd.TextValue = "value";
            wrd.Font.Bold = true;
            wrd.StartSymbol = new AncorSymbol("");
            wrd.EndSymbol = new AncorSymbol("");
            //wrd.Morse = true;
            txtLine.Add(wrd); // добавим его в строку
            this.TextContents.Add(txtLine);  

        }

        public GraphicsChartElement_SafeArea(double valDistOuter, distanceUOM valDistOuterUOM, string msaName, List<SafeArea_SectorDescription> MSA_sectorsList)
        {
            _msaSignRadius_mm = DefaultmsaSignRadius_mm;
            _valDistOuter = valDistOuter;
            _valDistOuterUOM = valDistOuterUOM.ToString();
            _fillColor = new AncorColor (255,255,255);
            _lineColor = new AncorColor (0,0,0);
            _flStl = fillStyle.fSNull;
            _lnStl = lineStyle.lsSolid;
            //_position = new AcntPoint(10, 10);
            _msaName = msaName;
            _lineWidth = 1;
            _magVar = 0;
            _fntInner = new AncorFont { Bold = false, FontColor = new AncorColor(0, 0, 0), Italic = false, Name ="Arial", Size = 8, UnderLine = false };
            _fntArrow = new AncorFont { Bold = false, FontColor = new AncorColor(0, 0, 0), Italic = false, Name ="Arial", Size = 8, UnderLine = false };
            _fntUnder = new AncorFont { Bold = false, FontColor = new AncorColor(0, 0, 0), Italic = false, Name ="Arial", Size = 8, UnderLine = false };
            _offset = 35;
            _arrowSize_pnt = 40;
            _sectors = MSA_sectorsList;
            _spaceValue = 0.2;
            _wordSpacing = 100;

            this.TextContents = new List<List<AncorChartElementWord>>();
            List<AncorChartElementWord> txtLine = new List<AncorChartElementWord>(); // создаем строку
            AncorChartElementWord wrd = new AncorChartElementWord(valDistOuter.ToString(), _fntInner);//создаем слово
            //wrd.TextValue = "value";
            wrd.Font.Bold = true;
            wrd.StartSymbol = new AncorSymbol("");
            wrd.EndSymbol = new AncorSymbol("");
            //wrd.Morse = true;
            txtLine.Add(wrd); // добавим его в строку

            wrd = new AncorChartElementWord(valDistOuterUOM.ToString(), _fntInner);//создаем слово
            //wrd.TextValue = "value";
            wrd.Font.Bold = true;
            wrd.StartSymbol = new AncorSymbol("");
            wrd.EndSymbol = new AncorSymbol("");
            //wrd.Morse = true;
            txtLine.Add(wrd); // добавим его в строку
            this.TextContents.Add(txtLine);


            //txtLine = new List<AncorChartElementWord>(); // создаем строку

            wrd = new AncorChartElementWord(msaName, _fntInner);//создаем слово
            //wrd.TextValue = "value";
            wrd.Font.Bold = true;
            wrd.StartSymbol = new AncorSymbol("");
            wrd.EndSymbol = new AncorSymbol("");
            //wrd.Morse = true;
            txtLine.Add(wrd); // добавим его в строку
            this.TextContents.Add(txtLine);  

        }


        private double _msaSignRadius_mm;
        private double _valDistOuter;
        private int _offset;
        private string _valDistOuterUOM;
        private double _magVar;
        private AncorColor _fillColor;
        private AncorColor _lineColor;
        private fillStyle _flStl;
        private lineStyle _lnStl;
        private int _lineWidth;
        //private AcntPoint _position;
        private string _msaName;
        private AncorFont _fntInner;
        private AncorFont _fntArrow;
        private AncorFont _fntUnder;
        private List<SafeArea_SectorDescription> _sectors;
        private int _arrowSize_pnt;
        private double _spaceValue;
        private System.Collections.Generic.List<List<AncorChartElementWord>> _textContents;

        private double _wordSpacing;
        //[Browsable(false)]
        [SkipAttribute(false)]
        public virtual double WordSpacing
        {
            get { return _wordSpacing; }
            set { _wordSpacing = value; }
        }


        [Editor(typeof(MyTextContextEditor), typeof(UITypeEditor))]
        //[Browsable(false)]
        //[Category("Text")]
        [SkipAttribute(true)]
        public List<List<AncorChartElementWord>> TextContents
        {
            get { return _textContents; }
            set { _textContents = value; }
        }


        [SkipAttribute(false)]
        public double SpaceValue
        {
            get { return _spaceValue; }
            set { _spaceValue = value; }
        }


        [SkipAttribute(false)]
        public double MsaSignRadius
        {
            get { return _msaSignRadius_mm; }
            set { _msaSignRadius_mm = value; }
        }

        //[XmlElement]
        [ReadOnly(true)]
        [SkipAttribute(true)]
        public double ValDistOuter
        {
            get { return _valDistOuter; }
            set { _valDistOuter = value; }
        }

        ////[XmlElement]
        //public int InnerTextOffset
        //{
        //    get { return _offset; }
        //    set { _offset = value; }
        //}

        //[XmlElement]
        [ReadOnly(true)]
        [SkipAttribute(true)]
        public string ValDistOuterUOM
        {
            get { return _valDistOuterUOM; }
            set { _valDistOuterUOM = value; }
        }

        //[XmlElement]
        [ReadOnly(true)]
        [SkipAttribute(true)]
        public double MagVar
        {
            get { return _magVar; }
            set { _magVar = value; }
        }

        //[XmlElement]
        [Editor(typeof(SigmaColorEdotor), typeof(UITypeEditor))]
        [SkipAttribute(false)]
        public AncorColor FillColor
        {
            get { return _fillColor; }
            set { _fillColor = value; }
        }

        //[XmlElement]
        [DisplayName("BorderLineColor")]
        [Editor(typeof(SigmaColorEdotor), typeof(UITypeEditor))]
        [SkipAttribute(false)]
        public AncorColor LineColor
        {
            get { return _lineColor; }
            set { _lineColor = value; }
        }

        //[XmlElement]
        [SkipAttribute(false)]
        public fillStyle FilllStyle
        {
            get { return _flStl; }
            set { _flStl = value; }
        }

        //[XmlElement]
        [DisplayName("BorderLineStyle")]
        [Editor(typeof(LineStyleEditor), typeof(UITypeEditor))]
        [SkipAttribute(false)]
        public lineStyle LinelStyle
        {
            get { return _lnStl; }
            set { _lnStl = value; }
        }

        //[XmlElement]
        [DisplayName("BorderLineWidth")]
        [SkipAttribute(false)]
        public int LineWidth
        {
            get { return _lineWidth; }
            set { _lineWidth = value; }
        }

        ////[XmlElement]
        //[ReadOnly(false)]
        //public AcntPoint Position
        //{
        //    get { return _position; }
        //    set { _position = value; }
        //}

        //[XmlElement]
        [ReadOnly(true)]
        [SkipAttribute(true)]
        public string MsaName
        {
            get { return _msaName; }
            set { _msaName = value; }
        }

        //[XmlElement]
        [SkipAttribute(false)]
        public AncorFont InnerFont
        {
            get { return _fntInner; }
            set { _fntInner = value; }
        }

        //[XmlElement]
        [SkipAttribute(false)]
        public AncorFont ArrowFont
        {
            get { return _fntArrow; }
            set { _fntArrow = value; }
        }

        //[XmlElement]
        [Browsable(false)]
        [SkipAttribute(false)]
        public AncorFont UnderFont
        {
            get { return _fntUnder; }
            set { _fntUnder = value; }
        }

        //[XmlElement]
        //[ReadOnly(true)]
        [Browsable(false)]
        [SkipAttribute(true)]
        public List<SafeArea_SectorDescription> Sectors
        {
            get { return _sectors; }
            set { _sectors = value; }
        }

        [SkipAttribute(false)]
        public int ArrowLen_pnt
        {
            get { return _arrowSize_pnt; }
            set { _arrowSize_pnt = value; }
        }


        public override object ConvertToIElement()
        {
            try
            {

                IElement Result = null;

                object Missing = Type.Missing;

                GraphicsChartElement_SafeArea AcntMsa = this;

                IGroupElement3 GrpEl = new GroupElementClass();
                stdole.IFontDisp pFontDisp;

                IPoint CntPnt = new PointClass();
                //CntPnt.PutCoords(0, 0);
                CntPnt.PutCoords(this.Position.X, this.Position.Y);
                double delta = this.SpaceValue;

                double scaleKoef = DefaultmsaSignRadius_mm / AcntMsa.MsaSignRadius;


                #region построение внешней окружности. На выходе имеем внешний круг знака MSA (IElement MSA_OuterRing)

                ICircularArc pArc = new CircularArcClass();
                IConstructCircularArc ArcCnstr = (IConstructCircularArc)pArc;
                //ArcCnstr.ConstructCircle(CntPnt, AcntMsa.MsaSignRadius / 10, true);
                ArcCnstr.ConstructCircle(CntPnt, DefaultmsaSignRadius_mm / 10, true);

                Polygon Poly = new PolygonClass();
                ISegmentCollection SegCol = (ISegmentCollection)Poly;
                SegCol.AddSegment((ISegment)pArc, ref Missing, ref Missing);

                ISimpleFillSymbol smplFill = new SimpleFillSymbol();

                //IRgbColor rgbClr = new RgbColor();  //цвет заполнения
                //rgbClr.Red = AcntMsa.FillColor.Red;
                //rgbClr.Blue = AcntMsa.FillColor.Blue;
                //rgbClr.Green = AcntMsa.FillColor.Green;

                smplFill.Color = AcntMsa.FillColor.GetColor();

                //стиль заполнения
                if (AcntMsa.FilllStyle == fillStyle.fSBackwardDiagonal) smplFill.Style = esriSimpleFillStyle.esriSFSBackwardDiagonal;
                if (AcntMsa.FilllStyle == fillStyle.fSCross) smplFill.Style = esriSimpleFillStyle.esriSFSCross;
                if (AcntMsa.FilllStyle == fillStyle.fSDiagonalCross) smplFill.Style = esriSimpleFillStyle.esriSFSDiagonalCross;
                if (AcntMsa.FilllStyle == fillStyle.fSForwardDiagonal) smplFill.Style = esriSimpleFillStyle.esriSFSForwardDiagonal;
                if (AcntMsa.FilllStyle == fillStyle.fSHollow) smplFill.Style = esriSimpleFillStyle.esriSFSHollow;
                if (AcntMsa.FilllStyle == fillStyle.fSHorizontal) smplFill.Style = esriSimpleFillStyle.esriSFSHorizontal;
                if (AcntMsa.FilllStyle == fillStyle.fSNull) smplFill.Style = esriSimpleFillStyle.esriSFSNull;
                if (AcntMsa.FilllStyle == fillStyle.fSSolid) smplFill.Style = esriSimpleFillStyle.esriSFSSolid;
                if (AcntMsa.FilllStyle == fillStyle.fSVertical) smplFill.Style = esriSimpleFillStyle.esriSFSVertical;


                ISimpleLineSymbol pSimpleLine = new SimpleLineSymbolClass();

                //IRgbColor rgbClr1 = new RgbColor();  //цвет линии
                //rgbClr1.Red = AcntMsa.LineColor.Red;
                //rgbClr1.Blue = AcntMsa.LineColor.Blue;
                //rgbClr1.Green = AcntMsa.LineColor.Green;
                pSimpleLine.Color = AcntMsa.LineColor.GetColor();

                if (AcntMsa.LinelStyle == lineStyle.lsDash) pSimpleLine.Style = esriSimpleLineStyle.esriSLSDash;
                if (AcntMsa.LinelStyle == lineStyle.lsDashDot) pSimpleLine.Style = esriSimpleLineStyle.esriSLSDashDot;
                if (AcntMsa.LinelStyle == lineStyle.lsDashDotDot) pSimpleLine.Style = esriSimpleLineStyle.esriSLSDashDotDot;
                if (AcntMsa.LinelStyle == lineStyle.lsDot) pSimpleLine.Style = esriSimpleLineStyle.esriSLSDot;
                if (AcntMsa.LinelStyle == lineStyle.lsInsideFrame) pSimpleLine.Style = esriSimpleLineStyle.esriSLSInsideFrame;
                if (AcntMsa.LinelStyle == lineStyle.lsNull) pSimpleLine.Style = esriSimpleLineStyle.esriSLSNull;
                if (AcntMsa.LinelStyle == lineStyle.lsSolid) pSimpleLine.Style = esriSimpleLineStyle.esriSLSSolid;

                pSimpleLine.Width = AcntMsa.LineWidth;


                smplFill.Outline = pSimpleLine;

                IFillShapeElement pFillShpElement = new PolygonElementClass();
                pFillShpElement.Symbol = smplFill;

                IElement MSA_OuterRing = (IElement)pFillShpElement;
                MSA_OuterRing.Geometry = (IGeometry)Poly;

                GrpEl.AddElement(MSA_OuterRing as IElement);

                #endregion

                #region построение внутренней структуры MSA


                IElement[] TmpArray = new IElement[AcntMsa.Sectors.Count];


                for (int i = 0; i <= AcntMsa.Sectors.Count - 1; i++)
                {

                    #region надпись ValDistVer

                    ITextElement pText = new TextElementClass();
                    pText.Text = AcntMsa.Sectors[i].ValDistVer.ToString() + " " + AcntMsa.Sectors[i].ValDistVerUOM;

                    TextSymbolClass pTextSymbol = new TextSymbolClass();
                    pTextSymbol.HorizontalAlignment = esriTextHorizontalAlignment.esriTHACenter;
                    pTextSymbol.VerticalAlignment = esriTextVerticalAlignment.esriTVACenter;


                    if (AcntMsa.Sectors.Count == 2) pTextSymbol.YOffset = 0;
                    pTextSymbol.Angle = 0;

                    pFontDisp = new stdole.StdFont() as stdole.IFontDisp;
                    //stdole.IFontDisp pFontDisp = new stdole.StdFont() as stdole.IFontDisp;
                    pFontDisp.Name = AcntMsa.InnerFont.Name;
                    pFontDisp.Bold = AcntMsa.InnerFont.Bold;
                    pFontDisp.Italic = AcntMsa.InnerFont.Italic;
                    pFontDisp.Underline = AcntMsa.InnerFont.UnderLine;
                    pFontDisp.Size = (decimal)AcntMsa.InnerFont.Size;

                    pTextSymbol.Font = pFontDisp;


                    //IRgbColor FntClr = new RgbColorClass();
                    //FntClr.Red = this.InnerFont.FontColor.Red;
                    //FntClr.Green = this.InnerFont.FontColor.Green;
                    //FntClr.Blue = this.InnerFont.FontColor.Blue;

                    pTextSymbol.Color = this.InnerFont.FontColor.GetColor();
                    pText.Symbol = pTextSymbol;

                    IElement pTextElement = pText as IElement;
                    pTextElement.Geometry = CntPnt;

                    TmpArray[i] = pTextElement as IElement;

                    ////////////////
                    GeometryFunctions.TapFunctions GF = new GeometryFunctions.TapFunctions();

                    IPoint frmpnt2 = new PointClass();
                    double a1 = -90 - AcntMsa.Sectors[i].ValAngleTo;
                    double a2 = -90 - AcntMsa.Sectors[i].ValAngleFm;
                    if (AcntMsa.Sectors[i].ValAngleFm == 0)
                        a1 = AcntMsa.Sectors[i].ValAngleTo + (a1 - a2) / 2;
                    else if ((AcntMsa.Sectors[i].ValAngleFm >= 90) && (AcntMsa.Sectors[i].ValAngleFm <= 270))
                        a1 = AcntMsa.Sectors[i].ValAngleTo + (a1 - a2) / 2;
                    else
                        a1 = AcntMsa.Sectors[i].ValAngleTo + (Math.Abs(a2) - Math.Abs(a1)) / 2;

                    if (AcntMsa.Sectors[i].ValAngleFm > AcntMsa.Sectors[i].ValAngleTo)
                        a1 += 180;

                    frmpnt2 = GF.PointAlongPlane(CntPnt, -90 - a1, DefaultmsaSignRadius_mm / 10);

                    ILine ln = new LineClass();
                    ln.FromPoint = CntPnt;
                    ln.ToPoint = frmpnt2;

                    if (AcntMsa.Sectors.Count > 1)
                        pTextElement.Geometry = (ln.Envelope as IArea).Centroid; //(pntcoll as IArea).Centroid;
                    else
                        pTextElement.Geometry = CntPnt;
                    //////////////

                    #endregion

                    #region формирование стрелки и надписи ValAngleTo

                    if (AcntMsa.Sectors.Count > 1)
                    {
                        ITextElement pTextElementArrow = new TextElementClass();
                        //int value = Convert.ToInt32("F06E", 16);


                        //pTextElementArrow.Text = Char.ConvertFromUtf32(value);


                        pTextElementArrow.Text = pTextElementArrow.Text + "<FNT name=\"" + AcntMsa.ArrowFont.Name + "\" size =\"" +
                                                AcntMsa.ArrowFont.Size.ToString() + "\">" + (AcntMsa.Sectors[i].ValAngleTo).ToString() + "°" + "</FNT>";


                        if (AcntMsa.ArrowFont.Bold) pTextElementArrow.Text = "<BOL>" + pTextElementArrow.Text + "</BOL>";
                        if (AcntMsa.ArrowFont.Italic) pTextElementArrow.Text = "<ITA>" + pTextElementArrow.Text + "</ITA>";
                        if (AcntMsa.ArrowFont.UnderLine) pTextElementArrow.Text = "<UND>" + pTextElementArrow.Text + "</UND>";

                        TextSymbolClass pTextSymbolArrow = new TextSymbolClass();

                        pTextSymbolArrow.HorizontalAlignment = esriTextHorizontalAlignment.esriTHALeft;
                        pTextSymbolArrow.VerticalAlignment = esriTextVerticalAlignment.esriTVACenter;


                        // параметры шрифта
                        stdole.IFontDisp pFontDispAr = new stdole.StdFont() as stdole.IFontDisp;
                        pFontDispAr.Name = "AeroSigma";
                        pFontDispAr.Size = AcntMsa.ArrowLen_pnt;
                        pTextSymbolArrow.Font = pFontDispAr;


                        //IRgbColor myColor = new RgbColor();

                        //myColor.Red = AcntMsa.ArrowFont.FontColor.Red;
                        //myColor.Blue = AcntMsa.ArrowFont.FontColor.Green;
                        //myColor.Green = AcntMsa.ArrowFont.FontColor.Blue;

                        pTextSymbolArrow.Color = AcntMsa.ArrowFont.FontColor.GetColor();

                        pTextSymbolArrow.Angle = -90 - AcntMsa.Sectors[i].ValAngleTo;


                        pTextSymbolArrow.Angle = pTextSymbolArrow.Angle < 0 ? pTextSymbolArrow.Angle + 360 : pTextSymbolArrow.Angle;

                        if (pTextSymbolArrow.Angle > 90 && pTextSymbolArrow.Angle <= 270)
                        {
                            pTextSymbolArrow.Angle += 180;
                            pTextSymbolArrow.Angle = pTextSymbolArrow.Angle > 360 ? pTextSymbolArrow.Angle - 360 : pTextSymbolArrow.Angle;

                            pTextSymbolArrow.HorizontalAlignment = esriTextHorizontalAlignment.esriTHARight;
                            pTextElementArrow.Text = pTextElementArrow.Text + "<FNT name=\"" + "AeroSigma" + "\" size =\"" +
                                                ArrowLen_pnt.ToString() + "\">" + "m" + "</FNT>";
                        }
                        else
                        {
                            pTextElementArrow.Text = "<FNT name=\"" + "AeroSigma" + "\" size =\"" + ArrowLen_pnt.ToString() + "\">" + "n" + "</FNT>" +
                                pTextElementArrow.Text;
                        }

                        pTextElementArrow.Symbol = pTextSymbolArrow;

                        IElement pElArrow = (IElement)pTextElementArrow;


                        pElArrow.Geometry = CntPnt;

                        GrpEl.AddElement(pElArrow as IElement);

                    }
                    #endregion;
      
                }

                //////// povernut
                ITransform2D transformScaleTMP = GrpEl as ITransform2D;
                double rotateAngle = this.MagVar * -1 * Math.PI / 180;
                transformScaleTMP.Rotate(CntPnt, rotateAngle);

                for (int i = 0; i <= AcntMsa.Sectors.Count - 1; i++) GrpEl.AddElement(TmpArray[i] as IElement);

                //// konec povernut

                #endregion

                #region подпись под значком MSA

                ITextElement pTextMsaUnder = new TextElementClass();
                pTextMsaUnder.Text = HelperClass.TextConstructor(this.TextContents);

                TextSymbolClass pTextSymbolMsaUnder = new TextSymbolClass();


                // форматирование текста
                HelperClass.FormatText(ref pTextSymbolMsaUnder, textPosition.Normal, -2, textCase.Normal, horizontalAlignment.Center, verticalAlignment.Center, 0, 0, this.WordSpacing);
                HelperClass.CreateFont(ref pTextSymbolMsaUnder, this.UnderFont);

                pTextMsaUnder.Symbol = pTextSymbolMsaUnder;


                IPoint Pnt1 = new PointClass();
                Pnt1.PutCoords(CntPnt.X, CntPnt.Y - AcntMsa.MsaSignRadius / 10 - delta);
                Pnt1.PutCoords(CntPnt.X, CntPnt.Y - DefaultmsaSignRadius_mm / 10 - delta);

                IElement pTextElementMsaUnder = pTextMsaUnder as IElement;
                pTextElementMsaUnder.Geometry = Pnt1;

                GrpEl.AddElement(pTextElementMsaUnder as IElement);




                #endregion


                #region Border

                ISimpleFillSymbol simpleFillSymbol = new SimpleFillSymbolClass();
                //IRgbColor FillrgbClrBorder = new RgbColor();  //цвет заполнения
                //FillrgbClrBorder.Red = AcntMsa.FillColor.Red;
                //FillrgbClrBorder.Blue = AcntMsa.FillColor.Blue;
                //FillrgbClrBorder.Green = AcntMsa.FillColor.Green;


                simpleFillSymbol.Color = AcntMsa.FillColor.GetColor();//FillrgbClrBorder;

                if (AcntMsa.FilllStyle == fillStyle.fSBackwardDiagonal) simpleFillSymbol.Style = esriSimpleFillStyle.esriSFSBackwardDiagonal;
                if (AcntMsa.FilllStyle == fillStyle.fSCross) simpleFillSymbol.Style = esriSimpleFillStyle.esriSFSCross;
                if (AcntMsa.FilllStyle == fillStyle.fSDiagonalCross) simpleFillSymbol.Style = esriSimpleFillStyle.esriSFSDiagonalCross;
                if (AcntMsa.FilllStyle == fillStyle.fSForwardDiagonal) simpleFillSymbol.Style = esriSimpleFillStyle.esriSFSForwardDiagonal;
                if (AcntMsa.FilllStyle == fillStyle.fSHollow) simpleFillSymbol.Style = esriSimpleFillStyle.esriSFSHollow;
                if (AcntMsa.FilllStyle == fillStyle.fSHorizontal) simpleFillSymbol.Style = esriSimpleFillStyle.esriSFSHorizontal;
                if (AcntMsa.FilllStyle == fillStyle.fSNull) simpleFillSymbol.Style = esriSimpleFillStyle.esriSFSNull;
                if (AcntMsa.FilllStyle == fillStyle.fSSolid) simpleFillSymbol.Style = esriSimpleFillStyle.esriSFSSolid;
                if (AcntMsa.FilllStyle == fillStyle.fSVertical) simpleFillSymbol.Style = esriSimpleFillStyle.esriSFSVertical;

                ISimpleLineSymbol pSimpleLineBorder = new SimpleLineSymbolClass();

                //IRgbColor rgbClrBorderLine = new RgbColor();  //цвет линии
                //rgbClrBorderLine.Red = AcntMsa.LineColor.Red;
                //rgbClrBorderLine.Blue = AcntMsa.LineColor.Blue;
                //rgbClrBorderLine.Green = AcntMsa.LineColor.Green;

                pSimpleLineBorder.Color = AcntMsa.LineColor.GetColor();//rgbClrBorderLine;


                if (AcntMsa.LinelStyle == lineStyle.lsDash) pSimpleLineBorder.Style = esriSimpleLineStyle.esriSLSDash;
                if (AcntMsa.LinelStyle == lineStyle.lsDashDot) pSimpleLineBorder.Style = esriSimpleLineStyle.esriSLSDashDot;
                if (AcntMsa.LinelStyle == lineStyle.lsDashDotDot) pSimpleLineBorder.Style = esriSimpleLineStyle.esriSLSDashDotDot;
                if (AcntMsa.LinelStyle == lineStyle.lsDot) pSimpleLineBorder.Style = esriSimpleLineStyle.esriSLSDot;
                if (AcntMsa.LinelStyle == lineStyle.lsInsideFrame) pSimpleLineBorder.Style = esriSimpleLineStyle.esriSLSInsideFrame;
                if (AcntMsa.LinelStyle == lineStyle.lsNull) pSimpleLineBorder.Style = esriSimpleLineStyle.esriSLSNull;
                if (AcntMsa.LinelStyle == lineStyle.lsSolid) pSimpleLineBorder.Style = esriSimpleLineStyle.esriSLSSolid;


                pSimpleLineBorder.Width = AcntMsa.LineWidth;
                simpleFillSymbol.Outline = pSimpleLineBorder;

                IFillShapeElement fillShapeElement = new PolygonElementClass();
                fillShapeElement.Symbol = simpleFillSymbol;
                IElement BorderElement = (IElement)fillShapeElement;

                object mis = Type.Missing;
                IPoint pnt = new PointClass();

                IPointCollection plg = new PolygonClass();

                pnt.PutCoords(GrpEl.get_Element(0).Geometry.Envelope.UpperLeft.X - delta, GrpEl.get_Element(0).Geometry.Envelope.UpperLeft.Y + delta);
                plg.AddPoint(pnt, ref mis, ref mis);

                pnt.PutCoords(GrpEl.get_Element(0).Geometry.Envelope.UpperRight.X + delta, GrpEl.get_Element(0).Geometry.Envelope.UpperRight.Y + delta);
                plg.AddPoint(pnt, ref mis, ref mis);

                pnt.PutCoords(GrpEl.get_Element(0).Geometry.Envelope.UpperRight.X + delta, GrpEl.get_Element(GrpEl.ElementCount - 1).Geometry.Envelope.LowerRight.Y - delta);
                plg.AddPoint(pnt, ref mis, ref mis);

                pnt.PutCoords(GrpEl.get_Element(0).Geometry.Envelope.UpperLeft.X - delta, GrpEl.get_Element(GrpEl.ElementCount - 1).Geometry.Envelope.LowerRight.Y - delta);
                plg.AddPoint(pnt, ref mis, ref mis);

                pnt.PutCoords(GrpEl.get_Element(0).Geometry.Envelope.UpperLeft.X - delta, GrpEl.get_Element(0).Geometry.Envelope.UpperLeft.Y + delta);
                plg.AddPoint(pnt, ref mis, ref mis);


                BorderElement.Geometry = plg as PolygonClass;

                GrpEl.AddElement(BorderElement as IElement);

                #endregion



                ITransform2D transformScale = GrpEl as ITransform2D;
                transformScale.Scale(CntPnt, AcntMsa.MsaSignRadius / DefaultmsaSignRadius_mm, AcntMsa.MsaSignRadius / DefaultmsaSignRadius_mm);


                (GrpEl as GroupElementClass).AnchorPoint = esriAnchorPointEnum.esriCenterPoint;
                //(GrpEl as GroupElementClass).Geometry = Poly as IGeometry;


                IGroupElement3 GrpElResult = new GroupElementClass();

                GrpElResult.AddElement(GrpEl.get_Element(GrpEl.ElementCount - 1));

                for (int j = 0; j <= GrpEl.ElementCount - 2; j++)
                {
                    GrpElResult.AddElement(GrpEl.get_Element(j));
                }
                return Result = (IElement)GrpElResult;

                // return Result = (IElement)GrpEl;


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

       
 
    }

    [XmlType]
    [Serializable()]
    public class SafeArea_SectorDescription : AbstractChartClass
    {

        public SafeArea_SectorDescription()
        {

        }

        public SafeArea_SectorDescription(double valAngleFm, double valAngleTo, double valDistLower, string valDistLowerUOM)
        {
            _valAngleFm = valAngleFm;
            _valAngleTo = valAngleTo;
            _valDistVer = valDistLower;
            _valDistVerUOM = valDistLowerUOM;
        }

        private double _valAngleFm;
        private double _valAngleTo;
        private double _valDistVer;
        private string _valDistVerUOM;

        //[XmlElement]
        [ReadOnly(true)]
        public double ValAngleFm
        {
            get { return _valAngleFm; }
            set { _valAngleFm = value; }
        }

        //[XmlElement]
        [ReadOnly(true)]
        public double ValAngleTo
        {
            get { return _valAngleTo; }
            set { _valAngleTo = value; }
        }

        //[XmlElement]
        [ReadOnly(true)]
        public double ValDistVer
        {
            get { return _valDistVer; }
            set { _valDistVer = value; }
        }

        //[XmlElement]
        [ReadOnly(true)]
        public string ValDistVerUOM
        {
            get { return _valDistVerUOM; }
            set { _valDistVerUOM = value; }
        }

    }

}
