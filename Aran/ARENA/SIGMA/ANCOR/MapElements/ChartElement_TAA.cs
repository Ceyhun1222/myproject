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

namespace ANCOR.MapElements
{
    [XmlType]
    [Serializable()]
    public class GraphicsChartElement_TAA : GraphicsChartElement
    {
        public GraphicsChartElement_TAA()
        {
            //_tAASignWIDTH = DefaultTSASignWIDTH;
            //_valDistOuter = 20;
            //_valDistOuterUOM = distanceUOM.Kilometers.ToString();
            _fillColor = new AncorColor(255,255,255);
            _lineColor = new AncorColor (0,0,0);
            _flStl = fillStyle.fSNull;
            _lnStl = lineStyle.lsSolid;
            //_position = new AcntPoint(10, 10);
            //_msaName = "MSA NAME";
            _lineWidth = 1;
            //_magVar = 0;
            _fntInner = new AncorFont { Bold = false, FontColor = new AncorColor(0, 0, 0), Italic = false, Name = "Courier New", Size = 8, UnderLine = false };
            //_fntArrow = new AcntFont();
            //_fntUnder = new AcntFont();
            //_offset = 35;
            _iF_SIGN_pnt = 20;
            _iAF_DESIGNATOR = "COMNG";
            _arrowSize_pnt = 7;
            _ang = 0;
            this.HaloMaskSize = 0.1;

        }


        private double _Distance_IF_IAF = 1;
        public double _course_TO_IF = 15;
        public double _course_TO_IAF = 265;
        public string _innerHeight = "5600";
        public string _TAA_TEXT = "24 NM to DESIGNATOR";

        //private double _tAASignWIDTH;
        //private double _valDistOuter;
        //private int _offset;
        //private string _valDistOuterUOM;
        //private double _magVar;
        private double _ang = 90;
        private AncorColor _fillColor;
        private AncorColor _lineColor;
        private fillStyle _flStl;
        private lineStyle _lnStl;
        private int _lineWidth;
        //private AcntPoint _position;
        //private string _msaName;
        private AncorFont _fntInner;
        //private AcntFont _fntArrow;
        //private AcntFont _fntUnder;
        //private List<clstMsaSectorDescription> _sectors;
        private int _iF_SIGN_pnt;
        protected Guid _id;
        private string _iAF_DESIGNATOR;
        private int _arrowSize_pnt;

        private double _haloMaskSize;
        public double HaloMaskSize
        {
            get { return _haloMaskSize; }
            set { _haloMaskSize = value; }
        }


        //[XmlElement]
        //public double TAASignWIDTH
        //{
        //    get { return _tAASignWIDTH; }
        //    set { _tAASignWIDTH = value; }
        //}

        //[XmlElement]
        //[ReadOnly(true)]
        //public double ValDistOuter
        //{
        //    get { return _valDistOuter; }
        //    set { _valDistOuter = value; }
        //}

        //[XmlElement]
        //public int InnerTextOffset
        //{
        //    get { return _offset; }
        //    set { _offset = value; }
        //}

        //[XmlElement]
        //[ReadOnly(true)]
        //public string ValDistOuterUOM
        //{
        //    get { return _valDistOuterUOM; }
        //    set { _valDistOuterUOM = value; }
        //}

        [XmlElement]
        public double Distance_IF_IAF
        {
            get { return _Distance_IF_IAF; }
            set { _Distance_IF_IAF = value; }
        }

        [XmlElement]
        [Editor(typeof(SigmaColorEdotor), typeof(UITypeEditor))]
        public AncorColor FillColor
        {
            get { return _fillColor; }
            set { _fillColor = value; }
        }

        [XmlElement]
        [Editor(typeof(SigmaColorEdotor), typeof(UITypeEditor))]
        public AncorColor LineColor
        {
            get { return _lineColor; }
            set { _lineColor = value; }
        }

        [XmlElement]
        public fillStyle FilllStyle
        {
            get { return _flStl; }
            set { _flStl = value; }
        }

        [XmlElement]
        [Editor(typeof(LineStyleEditor), typeof(UITypeEditor))]
        public lineStyle LinelStyle
        {
            get { return _lnStl; }
            set { _lnStl = value; }
        }

        [XmlElement]
        public int LineWidth
        {
            get { return _lineWidth; }
            set { _lineWidth = value; }
        }

        //[XmlElement]
        //[ReadOnly(false)]
        //public AcntPoint Position
        //{
        //    get { return _position; }
        //    set { _position = value; }
        //}


        [XmlElement]
        public AncorFont InnerFont
        {
            get { return _fntInner; }
            set { _fntInner = value; }
        }

        //[XmlElement]
        //public AcntFont ArrowFont
        //{
        //    get { return _fntArrow; }
        //    set { _fntArrow = value; }
        //}

        //[XmlElement]
        //public AcntFont UnderFont
        //{
        //    get { return _fntUnder; }
        //    set { _fntUnder = value; }
        //}

        //[XmlElement]
        //[ReadOnly(true)]
        //[Browsable(false)]
        //public List<clstMsaSectorDescription> Sectors
        //{
        //    get { return _sectors; }
        //    set { _sectors = value; }
        //}

        [XmlElement]
        //[ReadOnly(true)]
        //[Browsable(false)]
        public double ANG
        {
            get { return _ang; }
            set { _ang = value; }
        }


        [XmlElement]
        public int IF_SIGN_pnt
        {
            get { return _iF_SIGN_pnt; }
            set { _iF_SIGN_pnt = value; }
        }

        [XmlElement]
        public string IAF_DESIGNATOR
        {
            get { return _iAF_DESIGNATOR; }
            set { _iAF_DESIGNATOR = value; }
        }


        [XmlElement]
        public int ArrowSize_pnt
        {
            get { return _arrowSize_pnt; }
            set { _arrowSize_pnt = value; }
        }


        public override object ConvertToIElement()
        {
            try
            {
                ITransform2D transformTSA_SIGN;

                object Missing = Type.Missing;

                GraphicsChartElement_TAA AcntTAA = this;

                IGroupElement3 GrpEl = new GroupElementClass();
                stdole.IFontDisp pFontDisp;
                IMask haloMsk;

                IPoint CntPnt = new PointClass();
                CntPnt.PutCoords(this.Position.X, this.Position.Y);

                IPoint frmPnt = new PointClass();
                //frmPnt.PutCoords(pCenter.X + dblWidth, pCenter.Y);
                frmPnt.PutCoords(CntPnt.X + 3, CntPnt.Y);

                IPoint toPnt = new PointClass();
                //toPnt.PutCoords(pCenter.X, pCenter.Y - dblHeight);
                toPnt.PutCoords(CntPnt.X, CntPnt.Y - 2);

                //IRgbColor fntColor = new RgbColor();
                //fntColor.Red = AcntTAA.InnerFont.FontColor.Red;
                //fntColor.Blue = AcntTAA.InnerFont.FontColor.Blue;
                //fntColor.Green = AcntTAA.InnerFont.FontColor.Green;

                //IRgbColor LnColor = new RgbColor();
                //LnColor.Red = AcntTAA.LineColor.Red;
                //LnColor.Blue = AcntTAA.LineColor.Blue;
                //LnColor.Green = AcntTAA.LineColor.Green;

                double slope = ANG;
                slope = Math.PI * ANG / 180;

                IPolygon TSA_Poly = ConstructEllipsePolygon(CntPnt, frmPnt, toPnt, 3, 2);

                ISegmentCollection SegCol = (ISegmentCollection)TSA_Poly;
                ISegment arc = SegCol.get_Segment(1);

                #region построение внешней окружности. На выходе имеем "кусок пирога" знака TAA (IElement TAA_SIGN)


                ISimpleFillSymbol smplFill = new SimpleFillSymbol();

                IRgbColor rgbClr = new RgbColor();  //цвет заполнения
                rgbClr.Red = AcntTAA.FillColor.Red;
                rgbClr.Blue = AcntTAA.FillColor.Blue;
                rgbClr.Green = AcntTAA.FillColor.Green;

                smplFill.Color = rgbClr;

                //стиль заполнения
                if (AcntTAA.FilllStyle == fillStyle.fSBackwardDiagonal) smplFill.Style = esriSimpleFillStyle.esriSFSBackwardDiagonal;
                if (AcntTAA.FilllStyle == fillStyle.fSCross) smplFill.Style = esriSimpleFillStyle.esriSFSCross;
                if (AcntTAA.FilllStyle == fillStyle.fSDiagonalCross) smplFill.Style = esriSimpleFillStyle.esriSFSDiagonalCross;
                if (AcntTAA.FilllStyle == fillStyle.fSForwardDiagonal) smplFill.Style = esriSimpleFillStyle.esriSFSForwardDiagonal;
                if (AcntTAA.FilllStyle == fillStyle.fSHollow) smplFill.Style = esriSimpleFillStyle.esriSFSHollow;
                if (AcntTAA.FilllStyle == fillStyle.fSHorizontal) smplFill.Style = esriSimpleFillStyle.esriSFSHorizontal;
                if (AcntTAA.FilllStyle == fillStyle.fSNull) smplFill.Style = esriSimpleFillStyle.esriSFSNull;
                if (AcntTAA.FilllStyle == fillStyle.fSSolid) smplFill.Style = esriSimpleFillStyle.esriSFSSolid;
                if (AcntTAA.FilllStyle == fillStyle.fSVertical) smplFill.Style = esriSimpleFillStyle.esriSFSVertical;


                ISimpleLineSymbol pSimpleLine = new SimpleLineSymbolClass();

                pSimpleLine.Color = AcntTAA.LineColor.GetColor();//LnColor;

                if (AcntTAA.LinelStyle == lineStyle.lsDash) pSimpleLine.Style = esriSimpleLineStyle.esriSLSDash;
                if (AcntTAA.LinelStyle == lineStyle.lsDashDot) pSimpleLine.Style = esriSimpleLineStyle.esriSLSDashDot;
                if (AcntTAA.LinelStyle == lineStyle.lsDashDotDot) pSimpleLine.Style = esriSimpleLineStyle.esriSLSDashDotDot;
                if (AcntTAA.LinelStyle == lineStyle.lsDot) pSimpleLine.Style = esriSimpleLineStyle.esriSLSDot;
                if (AcntTAA.LinelStyle == lineStyle.lsInsideFrame) pSimpleLine.Style = esriSimpleLineStyle.esriSLSInsideFrame;
                if (AcntTAA.LinelStyle == lineStyle.lsNull) pSimpleLine.Style = esriSimpleLineStyle.esriSLSNull;
                if (AcntTAA.LinelStyle == lineStyle.lsSolid) pSimpleLine.Style = esriSimpleLineStyle.esriSLSSolid;

                pSimpleLine.Width = AcntTAA.LineWidth;


                smplFill.Outline = pSimpleLine;

                IFillShapeElement pFillShpElement = new PolygonElementClass();
                pFillShpElement.Symbol = smplFill;

                IElement TAA_SIGN = (IElement)pFillShpElement;
                TAA_SIGN.Geometry = (IGeometry)TSA_Poly; ;

                transformTSA_SIGN = TAA_SIGN as ITransform2D;
                transformTSA_SIGN.Rotate(CntPnt, slope);

                GrpEl.AddElement(TAA_SIGN as IElement);

                #endregion

                #region построение внутренней структуры TAA


                #region надпись IF

                ITextElement pText_IF = new TextElementClass();
                pText_IF.Text = "IF";

                TextSymbolClass pTextSymbol_IF = new TextSymbolClass();
                pTextSymbol_IF.HorizontalAlignment = esriTextHorizontalAlignment.esriTHACenter;
                pTextSymbol_IF.VerticalAlignment = esriTextVerticalAlignment.esriTVACenter;
                pTextSymbol_IF.XOffset = -6;
                pTextSymbol_IF.YOffset = 6;
                pTextSymbol_IF.Angle = 0;

                pFontDisp = new stdole.StdFont() as stdole.IFontDisp;
                pFontDisp.Name = AcntTAA.InnerFont.Name;
                pFontDisp.Bold = AcntTAA.InnerFont.Bold;
                pFontDisp.Italic = AcntTAA.InnerFont.Italic;
                pFontDisp.Underline = AcntTAA.InnerFont.UnderLine;
                pFontDisp.Size = (decimal)AcntTAA.InnerFont.Size;

                pTextSymbol_IF.Color = AcntTAA.InnerFont.FontColor.GetColor();//fntColor;
                pTextSymbol_IF.Font = pFontDisp;
                pText_IF.Symbol = pTextSymbol_IF;

                IElement pTextElement_IF = pText_IF as IElement;
                pTextElement_IF.Geometry = CntPnt;

                GrpEl.AddElement(pTextElement_IF as IElement);

                #endregion

                #region значек IF

                ITextElement pTextElement_IF_SIGN = new TextElementClass();
                int value = Convert.ToInt32("F04B", 16);


                pTextElement_IF_SIGN.Text = Char.ConvertFromUtf32(value);

                TextSymbolClass pTextSymbol_IF_SIGN = new TextSymbolClass();

                pTextSymbol_IF_SIGN.HorizontalAlignment = esriTextHorizontalAlignment.esriTHACenter;
                pTextSymbol_IF_SIGN.VerticalAlignment = esriTextVerticalAlignment.esriTVACenter;


                // параметры шрифта
                stdole.IFontDisp pFontDispAr = new stdole.StdFont() as stdole.IFontDisp;
                pFontDispAr.Name = "AeroSigma";
                pFontDispAr.Size = AcntTAA.IF_SIGN_pnt;
                pTextSymbol_IF_SIGN.Font = pFontDispAr;


                pTextSymbol_IF_SIGN.Color = AcntTAA.LineColor.GetColor();//LnColor;

                if (this.HaloMaskSize > 0)
                {
                    haloMsk = (IMask)pTextSymbol_IF_SIGN;
                    haloMsk.MaskStyle = esriMaskStyle.esriMSHalo;
                    haloMsk.MaskSize = this.HaloMaskSize;
                }

                //pTextSymbol_IF_SIGN.Angle = AcntMsa.Sectors[i].ValAngleTo + 90;
                pTextElement_IF_SIGN.Symbol = pTextSymbol_IF_SIGN;


                IElement pEl_IF_SIGN = (IElement)pTextElement_IF_SIGN;
                pEl_IF_SIGN.Geometry = CntPnt;

                GrpEl.AddElement(pEl_IF_SIGN as IElement);


                #endregion


                if (AcntTAA.IAF_DESIGNATOR.Trim().Length > 0)
                {
                    #region значек IAF

                    ITextElement pTextElement_IAF_SIGN = new TextElementClass();
                    value = Convert.ToInt32("F04B", 16);


                    pTextElement_IAF_SIGN.Text = Char.ConvertFromUtf32(value);

                    TextSymbolClass pTextSymbol_IAF_SIGN = new TextSymbolClass();

                    pTextSymbol_IAF_SIGN.HorizontalAlignment = esriTextHorizontalAlignment.esriTHACenter;
                    pTextSymbol_IAF_SIGN.VerticalAlignment = esriTextVerticalAlignment.esriTVACenter;


                    //// параметры шрифта
                    //stdole.IFontDisp pFontDispAr = new stdole.StdFont() as stdole.IFontDisp;
                    pFontDispAr.Name = "AeroSigma";
                    pFontDispAr.Size = AcntTAA.IF_SIGN_pnt;
                    pTextSymbol_IAF_SIGN.Font = pFontDispAr;
                    

                    pTextSymbol_IAF_SIGN.Color = AcntTAA.LineColor.GetColor();//LnColor;

                    if (this.HaloMaskSize > 0)
                    {
                        haloMsk = (IMask)pTextSymbol_IAF_SIGN;
                        haloMsk.MaskStyle = esriMaskStyle.esriMSHalo;
                        haloMsk.MaskSize = this.HaloMaskSize;
                    }
                    //pTextSymbol_IAF_SIGN.Angle = AcntMsa.Sectors[i].ValAngleTo + 90;
                    pTextElement_IAF_SIGN.Symbol = pTextSymbol_IAF_SIGN;


                    IElement pEl_IAF_SIGN = (IElement)pTextElement_IAF_SIGN;
                    IPoint iAF_PNT = new PointClass();
                    iAF_PNT.PutCoords(CntPnt.X + Distance_IF_IAF, CntPnt.Y);

                    pEl_IAF_SIGN.Geometry = iAF_PNT;

                    transformTSA_SIGN = pEl_IAF_SIGN as ITransform2D;
                    transformTSA_SIGN.Rotate(CntPnt, slope);

                    GrpEl.AddElement(pEl_IAF_SIGN as IElement);


                    #endregion

                    #region надпись IAF Designator

                    ITextElement pText_IAF = new TextElementClass();
                    pText_IAF.Text = AcntTAA.IAF_DESIGNATOR;

                    TextSymbolClass pTextSymbol_IAF = new TextSymbolClass();
                    pTextSymbol_IAF.HorizontalAlignment = esriTextHorizontalAlignment.esriTHALeft;
                    pTextSymbol_IAF.VerticalAlignment = esriTextVerticalAlignment.esriTVACenter;
                    pTextSymbol_IAF.XOffset = 5;
                    pTextSymbol_IAF.YOffset = 8;
                    pTextSymbol_IAF.Angle = 0;

                    pFontDisp = new stdole.StdFont() as stdole.IFontDisp;
                    pFontDisp.Name = AcntTAA.InnerFont.Name;
                    pFontDisp.Bold = AcntTAA.InnerFont.Bold;
                    pFontDisp.Italic = AcntTAA.InnerFont.Italic;
                    pFontDisp.Underline = AcntTAA.InnerFont.UnderLine;
                    pFontDisp.Size = (decimal)AcntTAA.InnerFont.Size;

                    pTextSymbol_IAF.Font = pFontDisp;
                    pTextSymbol_IAF.Color = AcntTAA.InnerFont.FontColor.GetColor();//fntColor;
                    pText_IAF.Symbol = pTextSymbol_IAF;

                    IElement pElement_IAF = pText_IAF as IElement;
                    pElement_IAF.Geometry = iAF_PNT;

                    transformTSA_SIGN = pElement_IAF as ITransform2D;
                    transformTSA_SIGN.Rotate(CntPnt, slope);

                    GrpEl.AddElement(pElement_IAF as IElement);

                    #endregion

                }


                #region формирование стрелок

                #region стрелка к IF

                IPointCollection4 pointCollection4 = new PolylineClass();

                // The GeometryEnvironmentClass is a singleton object.
                IPoint pp1 = new PointClass();
                pp1.PutCoords(CntPnt.X, CntPnt.Y - 0.2);
                pointCollection4.AddPoint(toPnt);
                pointCollection4.AddPoint(pp1);

                IPolyline polyline = pointCollection4 as IPolyline;

                ILineSymbol LLine = new CartographicLineSymbol();
                LLine.Color = AcntTAA.LineColor.GetColor();//LnColor;
                LLine.Width = AcntTAA.LineWidth;

                IArrowMarkerSymbol pMarker = new ArrowMarkerSymbol();
                pMarker.Length = AcntTAA.ArrowSize_pnt;
                pMarker.Color = AcntTAA.LineColor.GetColor();//LnColor;
                pMarker.Width = AcntTAA.ArrowSize_pnt;

                ISimpleLineDecorationElement pLineDecElem = new SimpleLineDecorationElement();
                pLineDecElem.AddPosition((double)arrowPosition.End);
                pLineDecElem.MarkerSymbol = (IMarkerSymbol)pMarker;


                ILineProperties lineProp = (ILineProperties)LLine;
                LineDecoration ld = new LineDecorationClass();
                ld.AddElement(pLineDecElem);
                lineProp.LineDecoration = ld;


                ESRI.ArcGIS.Carto.ILineElement lineElement = new ESRI.ArcGIS.Carto.LineElementClass();
                lineElement.Symbol = LLine;


                IElement ElLineTOIF = (ESRI.ArcGIS.Carto.IElement)lineElement;
                ElLineTOIF.Geometry = polyline as IGeometry;


                transformTSA_SIGN = ElLineTOIF as ITransform2D;
                transformTSA_SIGN.Rotate(CntPnt, slope);

                GrpEl.AddElement(ElLineTOIF as IElement);


                #endregion

                #region стрелка к IAF
                pointCollection4 = new PolylineClass();

                // The GeometryEnvironmentClass is a singleton object.
                pp1 = new PointClass();
                pp1.PutCoords(CntPnt.X + AcntTAA.Distance_IF_IAF + 0.2, CntPnt.Y);
                pointCollection4.AddPoint(frmPnt);
                pointCollection4.AddPoint(pp1);

                polyline = pointCollection4 as IPolyline;

                LLine = new CartographicLineSymbol();
                LLine.Color = AcntTAA.LineColor.GetColor();//LnColor;
                LLine.Width = AcntTAA.LineWidth;

                pMarker = new ArrowMarkerSymbol();
                pMarker.Length = AcntTAA.ArrowSize_pnt;
                pMarker.Color = AcntTAA.LineColor.GetColor();//LnColor;
                pMarker.Width = AcntTAA.ArrowSize_pnt;

                pLineDecElem = new SimpleLineDecorationElement();
                pLineDecElem.AddPosition((double)arrowPosition.End);
                pLineDecElem.MarkerSymbol = (IMarkerSymbol)pMarker;


                lineProp = (ILineProperties)LLine;
                ld = new LineDecorationClass();
                ld.AddElement(pLineDecElem);
                lineProp.LineDecoration = ld;


                lineElement = new ESRI.ArcGIS.Carto.LineElementClass();
                lineElement.Symbol = LLine;


                IElement ElLineTOAIF = (ESRI.ArcGIS.Carto.IElement)lineElement;
                ElLineTOAIF.Geometry = polyline as IGeometry;


                transformTSA_SIGN = ElLineTOAIF as ITransform2D;
                transformTSA_SIGN.Rotate(CntPnt, slope);

                GrpEl.AddElement(ElLineTOAIF as IElement);


                #endregion

                #endregion;

                #region формирование надписи курс к IF

                ITextElement pText_Cours_IF = new TextElementClass();
                pText_Cours_IF.Text = _course_TO_IF.ToString();

                while (pText_Cours_IF.Text.Length < 3) pText_Cours_IF.Text = "0" + pText_Cours_IF.Text;
                pText_Cours_IF.Text = pText_Cours_IF.Text + "º";

                TextSymbolClass pTextSymbol_Cours_IF = new TextSymbolClass();
                pTextSymbol_Cours_IF.HorizontalAlignment = esriTextHorizontalAlignment.esriTHACenter;
                pTextSymbol_Cours_IF.VerticalAlignment = esriTextVerticalAlignment.esriTVACenter;

                pp1.PutCoords(CntPnt.X, toPnt.Y + (CntPnt.Y - toPnt.Y) / 2);


                pTextSymbol_Cours_IF.XOffset = 0;
                pTextSymbol_Cours_IF.YOffset = 0;
                pTextSymbol_Cours_IF.Angle = 90;

                pFontDisp = new stdole.StdFont() as stdole.IFontDisp;
                pFontDisp.Name = AcntTAA.InnerFont.Name;
                pFontDisp.Bold = AcntTAA.InnerFont.Bold;
                pFontDisp.Italic = AcntTAA.InnerFont.Italic;
                pFontDisp.Underline = AcntTAA.InnerFont.UnderLine;
                pFontDisp.Size = (decimal)AcntTAA.InnerFont.Size;

                pTextSymbol_Cours_IF.Color = AcntTAA.InnerFont.FontColor.GetColor();//fntColor;;

                if (this.HaloMaskSize > 0)
                {
                    haloMsk = (IMask)pTextSymbol_Cours_IF;
                    haloMsk.MaskStyle = esriMaskStyle.esriMSHalo;
                    haloMsk.MaskSize = this.HaloMaskSize;
                }

                pTextSymbol_Cours_IF.Font = pFontDisp;
                pText_Cours_IF.Symbol = pTextSymbol_Cours_IF;

                IElement pTextElement_Cours_IF = pText_Cours_IF as IElement;
                pTextElement_Cours_IF.Geometry = pp1;

                transformTSA_SIGN = pTextElement_Cours_IF as ITransform2D;
                transformTSA_SIGN.Rotate(CntPnt, slope);

                GrpEl.AddElement(pTextElement_Cours_IF as IElement);

                #endregion

                #region формирование надписи курс к IAF

                ITextElement pText_Cours_IAF = new TextElementClass();
                pText_Cours_IAF.Text = _course_TO_IAF.ToString();

                while (pText_Cours_IAF.Text.Length < 3) pText_Cours_IAF.Text = "0" + pText_Cours_IAF.Text;
                pText_Cours_IAF.Text = pText_Cours_IAF.Text + "º";



                TextSymbolClass pTextSymbol_Cours_IAF = new TextSymbolClass();
                pTextSymbol_Cours_IAF.HorizontalAlignment = esriTextHorizontalAlignment.esriTHACenter;
                pTextSymbol_Cours_IAF.VerticalAlignment = esriTextVerticalAlignment.esriTVACenter;

                pp1.PutCoords(CntPnt.X + AcntTAA.Distance_IF_IAF + (frmPnt.X - CntPnt.X - AcntTAA.Distance_IF_IAF) / 2, CntPnt.Y);


                pTextSymbol_Cours_IAF.XOffset = 0;
                pTextSymbol_Cours_IAF.YOffset = 0;
                pTextSymbol_Cours_IAF.Angle = 0;

                pFontDisp = new stdole.StdFont() as stdole.IFontDisp;
                pFontDisp.Name = AcntTAA.InnerFont.Name;
                pFontDisp.Bold = AcntTAA.InnerFont.Bold;
                pFontDisp.Italic = AcntTAA.InnerFont.Italic;
                pFontDisp.Underline = AcntTAA.InnerFont.UnderLine;
                pFontDisp.Size = (decimal)AcntTAA.InnerFont.Size;


                pTextSymbol_Cours_IAF.Color = AcntTAA.InnerFont.FontColor.GetColor();//fntColor;;

                if (this.HaloMaskSize > 0)
                {
                    haloMsk = (IMask)pTextSymbol_Cours_IAF;
                    haloMsk.MaskStyle = esriMaskStyle.esriMSHalo;
                    haloMsk.MaskSize = this.HaloMaskSize;
                }

                pTextSymbol_Cours_IAF.Font = pFontDisp;
                pText_Cours_IAF.Symbol = pTextSymbol_Cours_IAF;

                IElement pTextElement_Cours_IAF = pText_Cours_IAF as IElement;
                pTextElement_Cours_IAF.Geometry = pp1;

                transformTSA_SIGN = pTextElement_Cours_IAF as ITransform2D;
                transformTSA_SIGN.Rotate(CntPnt, slope);

                GrpEl.AddElement(pTextElement_Cours_IAF as IElement);

                #endregion

                #region формирование надписи Высоты

                ITextElement pText_Height = new TextElementClass();
                pText_Height.Text = _innerHeight;

                TextSymbolClass pTextSymbol_Height = new TextSymbolClass();
                pTextSymbol_Height.HorizontalAlignment = esriTextHorizontalAlignment.esriTHACenter;
                pTextSymbol_Height.VerticalAlignment = esriTextVerticalAlignment.esriTVACenter;

                pp1 = (TSA_Poly as IArea).Centroid;


                pTextSymbol_Height.XOffset = 0;
                pTextSymbol_Height.YOffset = 0;
                pTextSymbol_Height.Angle = 0;

                pFontDisp = new stdole.StdFont() as stdole.IFontDisp;
                pFontDisp.Name = AcntTAA.InnerFont.Name;
                pFontDisp.Bold = AcntTAA.InnerFont.Bold;
                pFontDisp.Italic = AcntTAA.InnerFont.Italic;
                pFontDisp.Underline = AcntTAA.InnerFont.UnderLine;
                pFontDisp.Size = (decimal)AcntTAA.InnerFont.Size;

                pTextSymbol_Height.Color = AcntTAA.InnerFont.FontColor.GetColor();//fntColor;;

                if (this.HaloMaskSize > 0)
                {
                    haloMsk = (IMask)pTextSymbol_Height;
                    haloMsk.MaskStyle = esriMaskStyle.esriMSHalo;
                    haloMsk.MaskSize = this.HaloMaskSize;
                }

                pTextSymbol_Height.Font = pFontDisp;
                pText_Height.Symbol = pTextSymbol_Height;

                IElement pTextElement_Height = pText_Height as IElement;
                pTextElement_Height.Geometry = pp1;

                transformTSA_SIGN = pTextElement_Height as ITransform2D;
                transformTSA_SIGN.Rotate(CntPnt, slope);

                GrpEl.AddElement(pTextElement_Height as IElement);

                #endregion

                #region  фомирование подписи ТАА

                #region PsevdoGeometry

                //=====================

                IPolyline m_Polyline = new PolylineClass();

                IPointCollection m_PointCollection = (IPointCollection)m_Polyline;
                IPoint tangentPoint = new PointClass();

                for (double step = 0; step <= 1; step = step + 0.05)
                {

                    arc.QueryPoint(esriSegmentExtension.esriExtendAtFrom, step, true, tangentPoint);
                    m_PointCollection.AddPoint(tangentPoint, ref Missing, ref Missing);
                }

                //===============================================

                if (m_Polyline.FromPoint.X > m_Polyline.ToPoint.X)
                    m_Polyline.ReverseOrientation();

                #endregion

                TextSymbolClass textSymbol = new TextSymbolClass();
                textSymbol.XOffset = 0;
                textSymbol.YOffset = 0;

                textSymbol.Color = AcntTAA.InnerFont.FontColor.GetColor();//fntColor;

                pFontDisp = new stdole.StdFont() as stdole.IFontDisp;
                pFontDisp.Name = AcntTAA.InnerFont.Name;
                pFontDisp.Bold = AcntTAA.InnerFont.Bold;
                pFontDisp.Italic = AcntTAA.InnerFont.Italic;
                pFontDisp.Underline = AcntTAA.InnerFont.UnderLine;
                pFontDisp.Size = (decimal)AcntTAA.InnerFont.Size;

                //Create a text path and grab hold of the ITextPath interface
                ITextPath textPath = new BezierTextPathClass();  //to spline the text
                textPath.XOffset = 0;
                textPath.YOffset = 0;

                if (this.HaloMaskSize > 0)
                {
                    haloMsk = (IMask)pTextSymbol_Height;
                    haloMsk.MaskStyle = esriMaskStyle.esriMSHalo;
                    haloMsk.MaskSize = this.HaloMaskSize;
                }

                //Grab hold of the ISimpleTextSymbol interface through the ITextSymbol interface
                ISimpleTextSymbol simpleTextSymbol = (ISimpleTextSymbol)textSymbol;
                //Set the text path of the simple text symbol
                simpleTextSymbol.TextPath = textPath;
                simpleTextSymbol.Font = pFontDisp;

                simpleTextSymbol.Angle = 0;

                simpleTextSymbol.HorizontalAlignment = esriTextHorizontalAlignment.esriTHACenter;

                //simpleTextSymbol.VerticalAlignment = esriTextVerticalAlignment.esriTVABaseline;
                //simpleTextSymbol.VerticalAlignment = esriTextVerticalAlignment.esriTVABottom;
                //simpleTextSymbol.VerticalAlignment = esriTextVerticalAlignment.esriTVACenter;
                simpleTextSymbol.VerticalAlignment = esriTextVerticalAlignment.esriTVATop;

                //Draw the line object and spline the user text around the line
                ITextElement pTextEl = new TextElementClass();
                pTextEl.Text = _TAA_TEXT;

                pTextEl.Symbol = simpleTextSymbol;
                //pTextEl.Symbol.Angle = this.Slope;

                IElement El = (IElement)pTextEl;


                El.Geometry = (IGeometry)m_Polyline;


                transformTSA_SIGN = El as ITransform2D;
                transformTSA_SIGN.Rotate(CntPnt, slope);

                GrpEl.AddElement(El as IElement);
                #endregion


                #endregion


                (GrpEl as GroupElementClass).AnchorPoint = esriAnchorPointEnum.esriCenterPoint;


                return (IElement)GrpEl;


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public IPolygon ConstructEllipsePolygon(IPoint pCenter, IPoint frmPnt, IPoint toPnt, double dblWidth, double dblHeight)
        {
            object Missing = Type.Missing;

            //If pCenter Is Nothing Then Exit Function



            ILine ln1 = new LineClass();
            ln1.PutCoords(pCenter, frmPnt);

            ILine ln2 = new LineClass();
            ln2.PutCoords(toPnt, pCenter);

            //'Determine extent of ellipse
            IEnvelope pEnv = pCenter.Envelope;
            //If pEnv Is Nothing Then Exit Function
            //If pEnv.IsEmpty Then Exit Function

            //Set pEnv.SpatialReference = pSR
            pEnv.Expand(dblWidth, dblHeight, false);

            //'Create Ellipse Elements
            IEllipticArc pEllipticArc = new EllipticArc();
            //Set pEllipticArc.SpatialReference = pSR
            IConstructEllipticArc pConstructEllipticArc = pEllipticArc as IConstructEllipticArc;

            //'Draw Ellipse
            pConstructEllipticArc.ConstructTwoPointsEnvelope(frmPnt, toPnt, pEnv, esriArcOrientation.esriArcClockwise);  // pConstructEllipticArc.ConstructEnvelope(pEnv);

            IPolygon pPolygon = new PolygonClass();
            ISegmentCollection SegCol = (ISegmentCollection)pPolygon;

            SegCol.AddSegment(ln1 as ISegment, ref Missing, ref Missing);

            SegCol.AddSegment(pEllipticArc as ISegment, ref Missing, ref Missing);


            SegCol.AddSegment(ln2 as ISegment, ref Missing, ref Missing);

            pPolygon.Close();



            return pPolygon;

        }
    }

}
