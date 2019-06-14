using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using ESRI.ArcGIS.ArcMapUI;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Framework;
using ESRI.ArcGIS.Geometry;
using stdole;
namespace SigmaCallout
{

    [ProgId(PolygonCallout.id), Guid("9f0d2f81-a484-4666-a8a1-f24ed68b29d8"), ComVisible(true)]
    public class PolygonCallout : ITextBackground, ICallout, IPersistVariant, IClone, IQueryGeometry, IDisplayName
    {
        private IEnvelope _textBox;
        private IPoint _anchorPoint;
        private List<IPoint> prevAnchorPoints;
        private IPoint pnt;
        private IGeometry _geometry;
        private ISimpleFillSymbol _fillSymbol;
        private IPoint _textBoxCenterPt;
        private ITextSymbol _textSym;
        public const string id = "Sigma.PolygonCallout";
        private int _hdc;
        private ITransformation _transform;



        #region ITextBackground Members

        public void Draw(int hDC, ITransformation transform)
        {
            _hdc = hDC;
            _transform = transform;

            IDisplayTransformation pDisplayTransform = (IDisplayTransformation)transform;

            
            _geometry = CreateGeometry(_textBoxCenterPt, hDC, pDisplayTransform);
          

            if (_geometry == null) return;

            //=============================================================================
            ISymbol pSymbol;
            IGeometryCollection geom = (IGeometryCollection)_geometry;
            //===================Draw backgraund==========================================================

            _fillSymbol.Color = BackColor;
            _fillSymbol.Outline = null;
            _fillSymbol.Style = this.calloutFillStyle;

            pSymbol = (ISymbol)_fillSymbol;
            pSymbol.SetupDC(hDC, transform);
            pSymbol.Draw(geom.get_Geometry(0)); // []
            pSymbol.ResetDC();

            //===================Draw lines ==========================================================

            pSymbol = (ISymbol)LineSymbol;
            pSymbol.SetupDC(hDC, transform);

            if (geom.get_Geometry(1) != null && (geom.get_Geometry(1) as IPointCollection).PointCount > 0)
                pSymbol.Draw(geom.get_Geometry(1)); // [
            if (geom.get_Geometry(2) != null && (geom.get_Geometry(2) as IPointCollection).PointCount > 0)
                pSymbol.Draw(geom.get_Geometry(2)); // ]

            pSymbol.ResetDC();


        }

        public void QueryBoundary(int hDC, ITransformation transform, ESRI.ArcGIS.Geometry.IPolygon boundary)
        {
            if (_textSym == null) return;

            ISymbol pSymbol = (ISymbol)FillSymbol;

            _geometry = CreateGeometry(_textBoxCenterPt, hDC, transform);
           

            IGeometryCollection geomColl = (IGeometryCollection)_geometry;
            if (geomColl == null) return;

            //pSymbol.QueryBoundary ( hDC, transform, geomColl.get_Geometry ( 0 ).Envelope, boundary );
            if (pSymbol == null) return;
            pSymbol.QueryBoundary(hDC, transform, geomColl.get_Geometry(0), boundary);


            ITopologicalOperator2 pTopoOp = (ITopologicalOperator2)boundary;
            pTopoOp.IsKnownSimple_2 = false;
            pTopoOp.Simplify();

        }

        public IEnvelope TextBox
        {
            set
            {
                _textBox = value;
                if (value == null)
                    return;

                _textBoxCenterPt = new PointClass();
                _textBoxCenterPt.X = 0.5 * (_textBox.XMin + _textBox.XMax);
                _textBoxCenterPt.Y = 0.5 * (_textBox.YMin + _textBox.YMax);
            }
        }

        ITextSymbol ITextBackground.TextSymbol
        {
            get
            {
                return _textSym;
            }
            set
            {
                if (value == null)
                    return;
                if (value.Equals(_textSym))
                    return;
                _textSym = value;
            }
        }

        #endregion

        #region ICallout Members

        public IPoint AnchorPoint
        {
            get
            {
                if (_anchorPoint == null)
                    return null;
                IClone p = (IClone)_anchorPoint;
                return (IPoint)p.Clone();
            }
            set
            {
                if (value == null)
                {
                    _anchorPoint = null;
                    return;
                }
                if (_anchorPoint == null)
                    _anchorPoint = new PointClass();

                pnt = new Point();
                if (!_anchorPoint.IsEmpty)
                {
                    pnt.PutCoords(_anchorPoint.X, _anchorPoint.Y);
                    prevAnchorPoints.Add(pnt);
                }

                _anchorPoint.PutCoords(value.X, value.Y);
                if (value.SpatialReference != null) _anchorPoint.SpatialReference = value.SpatialReference;


            }
        }

        public double LeaderTolerance
        {
            get;
            set;
        }


        #endregion

        #region IPersistVariant Members

        UID IPersistVariant.ID
        {
            get
            {
                var uid = new UIDClass();
                uid.Value = id;
                return uid;
            }
        }

        void IPersistVariant.Load(IVariantStream Stream)
        {

            Version = Convert.ToInt32(Stream.Read());
            AnchorPoint = (IPoint)Stream.Read();
            LeaderTolerance = Convert.ToDouble(Stream.Read());
            BackColor = (IRgbColor)Stream.Read();
            LineSymbol = (ISimpleLineSymbol)Stream.Read();
            Snap = Convert.ToInt32(Stream.Read());
            Width = Convert.ToDouble(Stream.Read());
            Length = Convert.ToDouble(Stream.Read());
            Depth = Convert.ToDouble(Stream.Read());
            ILSStyle = Convert.ToInt32(Stream.Read());
            Slope = Convert.ToDouble(Stream.Read());
            calloutFillStyle = (Stream.Read() as ISimpleFillSymbol).Style;
        }

        void IPersistVariant.Save(IVariantStream Stream)
        {
            Stream.Write(Version);
            Stream.Write(AnchorPoint);
            Stream.Write(LeaderTolerance);
            Stream.Write(BackColor);
            Stream.Write(LineSymbol);
            Stream.Write(Snap);
            Stream.Write(Width);
            Stream.Write(Length);
            Stream.Write(Depth);
            Stream.Write(ILSStyle);
            Stream.Write(Slope);
            Stream.Write(_fillSymbol);

        }

        #endregion

        #region IClone Members

        public void Assign(IClone src)
        {
            // Make sure src is pointing to a valid object.
            if (src == null)
                throw new System.Runtime.InteropServices.COMException("Invalid object");

            if (!(src is PolygonCallout))
                throw new System.Runtime.InteropServices.COMException("Bad object type");


            var srcClonable = (PolygonCallout)src;
            _anchorPoint = srcClonable.AnchorPoint;
            BackColor = srcClonable.BackColor;
            LineSymbol = srcClonable.LineSymbol;
            //_leaderSymbol = srcClonable._leaderSymbol;
            Snap = srcClonable.Snap;
            Width = srcClonable.Width;
            Length = srcClonable.Length;
            Depth = srcClonable.Depth;
            ILSStyle = srcClonable.ILSStyle;
            Slope = srcClonable.Slope;
            LeaderTolerance = srcClonable.LeaderTolerance;
            calloutFillStyle = srcClonable.calloutFillStyle;
            _fillSymbol.Style = srcClonable.calloutFillStyle;
            Version = srcClonable.Version;


        }

        public IClone Clone()
        {
            PolygonCallout result = new PolygonCallout();
            result.Assign(this);
            return result;
        }


        public bool IsEqual(IClone other)
        {
            if (other == null)
                throw new System.Runtime.InteropServices.COMException("Invalid object");

            if (!(other is PolygonCallout))
                throw new System.Runtime.InteropServices.COMException("Bad object type");

            var otherClonable = (PolygonCallout)other;
            if (Version == otherClonable.Version &&
                LineSymbol == otherClonable.LineSymbol &&
                Snap == otherClonable.Snap &&
                Width == otherClonable.Width &&
                Length == otherClonable.Length && 
                Depth == otherClonable.Depth &&
                ILSStyle == otherClonable.ILSStyle &&
                Slope == otherClonable.Slope &&
                LeaderTolerance == otherClonable.LeaderTolerance &&
                NameString == otherClonable.NameString)
            {
                IClone tmp = (IClone)_anchorPoint;
                IClone tmp2 = (IClone)otherClonable.AnchorPoint;
                if (tmp.IsEqual(tmp2))
                {
                    tmp = (IClone)LineSymbol;
                    tmp2 = (IClone)otherClonable.LineSymbol;
                    if (tmp.IsEqual(tmp2))
                    {
                        tmp = (IClone)BackColor;
                        tmp2 = (IClone)otherClonable.BackColor;
                        if (tmp.IsEqual(tmp2))
                        {
                                return true;
                        }
                    }
                }
            }
            return false;
        }

        public bool IsIdentical(IClone other)
        {
            if (other == null)
                throw new System.Runtime.InteropServices.COMException("Invalid object");

            if (!(other is PolygonCallout))
                throw new System.Runtime.InteropServices.COMException("Bad object type");

            if (((PolygonCallout)other) == this)
                return true;
            return false;
        }

        #endregion

        #region IQueryGeometry Members

        public IGeometry GetGeometry(int hDC, ITransformation displayTransform, IGeometry drawGeometry)
        {

                return CreateGeometry(_textBoxCenterPt, hDC, displayTransform);

        }

        public void QueryEnvelope(int hDC, ITransformation displayTransform, IGeometry drawGeometry, IEnvelope envelope)
        {

                envelope = CreateGeometry(_textBoxCenterPt, hDC, displayTransform).Envelope;

        }

        #endregion

        #region IDisplayName Members

        public string NameString
        {
            get
            {
                return "Sigma Callout";
            }
        }

        #endregion

        //private IPointCollection FindOutsidePoint(IPoint _textBoxCenterPt, int hDC, ITransformation transform)
        //{
        //    IDisplayTransformation pDisplayTransformation = (IDisplayTransformation)transform;
        //    esriUnits units = pDisplayTransformation.Units;
        //    double scasle, scasleRatio;

        //    if (units != esriUnits.esriUnknownUnits)
        //    {
        //        scasle = pDisplayTransformation.ReferenceScale;
        //        scasleRatio = pDisplayTransformation.ScaleRatio;
        //    }
        //    else
        //        scasleRatio = 1.0;

        //    TextSymbol textSym = (TextSymbol)_textSym;

        //    // Create polygon =====================================
        //    IPoint BasePoint;
        //    double Hcap = 0, Wcap = 0, Hcont = 0, Wcont = 0, Hbot = 0, Wbot = 0, Wmax;
        //    int iCap = 0, iCont = 0, iBot = 0;
        //    int contStartIndex = 0, contEndIndex = _strLines.Count - 1;
        //    double leading = pDisplayTransformation.FromPoints(textSym.Leading);
        //    double morseX, morseY;
        //    if (HasHeader)
        //    {
        //        iCap = Text_Pars(hDC, transform, new string[] { _strLines[0] }, leading, out Wcap, out Hcap, out morseX, out morseY);
        //        contStartIndex = 1;
        //    }
        //    else
        //        HeaderHorizontalMargin = 0;

        //    if (HasFooter)
        //    {
        //        if (string.IsNullOrEmpty(_morseTxt))
        //            iBot = Text_Pars(hDC, transform, new string[] { _strLines[_strLines.Count - 1] }, leading, out Wbot, out Hbot, out morseX, out morseY);
        //        else
        //            iBot = Text_Pars(hDC, transform, new string[] { _strLines[_strLines.Count - 2] }, leading, out Wbot, out Hbot, out morseX, out morseY);
        //        contEndIndex--;
        //    }
        //    else
        //        FooterHorizontalMargin = 0;
        //    List<string> resCont = new List<string>();
        //    for (int k = contStartIndex; k <= contEndIndex; k++)
        //    {
        //        resCont.Add(_strLines[k]);
        //    }
        //    iCont = Text_Pars(hDC, transform, resCont.ToArray(), leading, out Wcont, out Hcont, out morseX, out morseY);

        //    double width = pDisplayTransformation.FromPoints(Width); //prop
        //    double ShadowMargine = pDisplayTransformation.FromPoints(this.Shadow);

        //    double _dHeaderMargine = pDisplayTransformation.FromPoints(TopMargin);
        //    double _dFooterMargine = pDisplayTransformation.FromPoints(BottomMargin);

        //    double _dHeaderTextMargine = pDisplayTransformation.FromPoints(HeaderHorizontalMargin);
        //    double _dFooterTextMargine = pDisplayTransformation.FromPoints(FooterHorizontalMargin);

        //    Wcont = pDisplayTransformation.FromPoints(Wcont);
        //    Hcont = pDisplayTransformation.FromPoints(Hcont);

        //    Wcap = pDisplayTransformation.FromPoints(Wcap);
        //    Hcap = pDisplayTransformation.FromPoints(Hcap);

        //    Wbot = pDisplayTransformation.FromPoints(Wbot);
        //    Hbot = pDisplayTransformation.FromPoints(Hbot);


        //    Wmax = Math.Max(Wcont, Wcap);
        //    double textAngle = _textSym.Angle + 180.0;
        //    double Hsum = Hcont + Hcap + Hbot + leading * (iCont + iCap + iBot - 1);

        //    Wmax *= 1.1;
        //    Hsum *= 1.2;
        //    BasePoint = PointAlongPlane(_textBoxCenterPt, textAngle - 90.0, 0.5 * (Hsum - Hcap) + _dHeaderMargine);
        //    IPoint MorsePnt = null;
        //    if (!double.IsNaN(morseX))
        //    {
        //        morseX = pDisplayTransformation.FromPoints(morseX);
        //        morseY = pDisplayTransformation.FromPoints(morseY);
        //        //morseElemWidth = pDisplayTransformation.FromPoints ( morseElemWidth );
        //        //morseElemHeight = pDisplayTransformation.FromPoints ( morseElemHeight );
        //        MorsePnt = PointAlongPlane(_textBoxCenterPt, textAngle - 180, 0.7 * morseX);
        //        //MorsePnt = PointAlongPlane ( MorsePnt, textAngle - 90, ( Hsum - Hcap ) * 0.5 + morseY);
        //        //MorsePnt = _textBoxCenterPt;
        //    }

        //    IPoint pt1 = PointAlongPlane(BasePoint, textAngle, 0.5 * Wmax + width);
        //    IPoint pt2 = PointAlongPlane(pt1, textAngle + 90.0, Hsum - 0.5 * (Hcap + Hbot) + _dHeaderMargine + _dFooterMargine);
        //    IPoint pt3 = PointAlongPlane(pt2, textAngle + 180.0, Wmax + 2 * width);
        //    IPoint pt4 = PointAlongPlane(BasePoint, textAngle + 180.0, 0.5 * Wmax + width);
        //    IPointCollection pPoly = new ESRI.ArcGIS.Geometry.Polygon();

        //    pPoly.AddPoint(pt1);
        //    pPoly.AddPoint(pt2);
        //    pPoly.AddPoint(pt3);
        //    pPoly.AddPoint(pt4);
        //    pPoly.AddPoint(pt1);
        //    return pPoly;
        //}

        private IGeometry CreateGeometry(IPoint _textBoxCenterPt, int hDC, ITransformation transform)
        {
            /////////////////////////////////////////////////////////

            Type Tp = Type.GetTypeFromCLSID(typeof(AppRefClass).GUID);

            try
            {
                if (_anchorPoint != null && _anchorPoint.SpatialReference != null && _anchorPoint.SpatialReference.Name.Contains("1984"))
                {
                    System.Object obj = Activator.CreateInstance(Tp);
                    IApplication app = obj as IApplication;
                    IMxDocument MxDoc = app.Document as IMxDocument;
                    IActiveView view = MxDoc.ActiveView;
                    IMap pMap = view.FocusMap;
                    _anchorPoint.Project(pMap.SpatialReference);
                }
            }
            catch
            { }


            ////////////////////////////////////////////////////////


            IDisplayTransformation pDisplayTransformation = (IDisplayTransformation)transform;
            esriUnits units = pDisplayTransformation.Units;
            double scasle, scasleRatio;

            if (units != esriUnits.esriUnknownUnits)
            {
                scasle = pDisplayTransformation.ReferenceScale;
                scasleRatio = pDisplayTransformation.ScaleRatio;
            }
            else
                scasleRatio = 1.0;

            _textSym.Text = "";
            TextSymbol textSym = (TextSymbol)_textSym;

            // Create polygon =====================================
            IPoint BasePoint;

            double L = pDisplayTransformation.FromPoints(Length); //prop
            double W = pDisplayTransformation.FromPoints(Width);
            double D = pDisplayTransformation.FromPoints(Depth);

            _textSym.Angle = Slope;
            double textAngle = _textSym.Angle;
            IPoint pt1 = PointAlongPlane(_anchorPoint, textAngle, L);
            IPoint pt2 = PointAlongPlane(pt1, textAngle - 180.0, D);
            IPoint pt3 = PointAlongPlane(pt1, textAngle + 90.0, W / 2);
            IPoint pt4 = PointAlongPlane(pt1, textAngle - 90.0, W / 2);
            IGeometryCollection GeoBag = new GeometryBagClass();

            IRgbColor myColor = new RgbColorClass();
            myColor.RGB = BackColor.RGB;
            //myColor.Blue = 0;
            //myColor.Green = 0;

            //IColor emptyColor = null;

            _fillSymbol.Color.RGB = 0;
            _fillSymbol.Outline.Width = 1.0;
            _fillSymbol.Outline.Color = myColor;
            _fillSymbol.Style = this.calloutFillStyle;

            ILineFillSymbol lineFill = new LineFillSymbol();

            lineFill.Angle = _textSym.Angle - Math.Atan2(W / 2, D) * 180 / Math.PI;

            lineFill.Separation = 5;
            lineFill.Offset = 5;
            lineFill.Color = myColor;
            lineFill.Outline.Width = -1;
            lineFill.Outline = LineSymbol;

            IPointCollection pPolyL = new ESRI.ArcGIS.Geometry.Polygon();
            IPointCollection pPolyR = new ESRI.ArcGIS.Geometry.Polygon();

            int style;
            style = ILSStyle;
            switch (style)
            {
                case 1:
                    IPointCollection pPoly = new ESRI.ArcGIS.Geometry.Polygon();
                    IPointCollection pPolyLine = new ESRI.ArcGIS.Geometry.Polyline();


                    pPoly.AddPoint(_anchorPoint);
                    pPoly.AddPoint(pt3);
                    pPoly.AddPoint(pt2);
                    pPoly.AddPoint(pt4);
                    pPoly.AddPoint(_anchorPoint);
                    GeoBag.AddGeometry((IPolygon)pPoly);

                    LineSymbol.Width = 0;
                    lineFill.Outline = LineSymbol;
                    ISymbol pSymbol = (ISymbol)lineFill;

                    pSymbol.SetupDC(hDC, transform);
                    pSymbol.Draw(GeoBag.get_Geometry(0)); // []

                    pSymbol.ResetDC();

                    pPolyLine.AddPoint(_anchorPoint);
                    pPolyLine.AddPoint(pt3);
                    GeoBag.AddGeometry((IPolyline)pPolyLine);

                    LineSymbol.Color = myColor;
                    LineSymbol.Width = 1.5;
                    pSymbol = (ISymbol)LineSymbol;

                    pSymbol.SetupDC(hDC, transform);
                    pSymbol.Draw(GeoBag.get_Geometry(1)); // []

                    pSymbol.ResetDC();

                    return GeoBag.get_Geometry(0);
                case 2:

                    pPolyL.AddPoint(_anchorPoint);
                    pPolyL.AddPoint(pt3);
                    pPolyL.AddPoint(pt2);
                    pPolyL.AddPoint(_anchorPoint);
                    GeoBag.AddGeometry((IPolygon)pPolyL);



                    pPolyR.AddPoint(_anchorPoint);
                    pPolyR.AddPoint(pt2);
                    pPolyR.AddPoint(pt4);
                    pPolyR.AddPoint(_anchorPoint);
                    GeoBag.AddGeometry((IPolygon)pPolyR);


                    //myColor.Red = 0;
                    //myColor.Blue = 0;
                    //myColor.Green = 0;
                    myColor.RGB = BackColor.RGB;
                    _fillSymbol.Color = myColor;
                    pSymbol = (ISymbol)_fillSymbol;

                    pSymbol.SetupDC(hDC, transform);
                    pSymbol.Draw(GeoBag.get_Geometry(1)); // []
                    pSymbol.ResetDC();

                    //_fillSymbol.Color.NullColor = true;
                    myColor.Red = 255;
                    myColor.Blue = 255;
                    myColor.Green = 255;
                    _fillSymbol.Color = myColor;
                    //_fillSymbol.Color = emptyColor;
                    _fillSymbol.Style = esriSimpleFillStyle.esriSFSNull;
                    pSymbol = (ISymbol)_fillSymbol;

                    pSymbol.SetupDC(hDC, transform);
                    pSymbol.Draw(GeoBag.get_Geometry(0));
                    pSymbol.ResetDC();


                    return GeoBag.get_Geometry(1);
                case 3:

                    pPolyL.AddPoint(_anchorPoint);
                    pPolyL.AddPoint(pt3);
                    pPolyL.AddPoint(pt2);
                    pPolyL.AddPoint(_anchorPoint);
                    GeoBag.AddGeometry((IPolygon)pPolyL);


                    //myColor.Red = 255;
                    //myColor.Blue = 255;
                    //myColor.Green = 255;
                    myColor.RGB = BackColor.RGB;
                    _fillSymbol.Color = myColor;
                    pSymbol = (ISymbol)_fillSymbol;

                    pSymbol.SetupDC(hDC, transform);
                    pSymbol.Draw(GeoBag.get_Geometry(0)); // []
                    pSymbol.ResetDC();

                    pPolyR.AddPoint(_anchorPoint);
                    pPolyR.AddPoint(pt2);
                    pPolyR.AddPoint(pt4);
                    pPolyR.AddPoint(_anchorPoint);
                    GeoBag.AddGeometry((IPolygon)pPolyR);

                    pSymbol.SetupDC(hDC, transform);
                    pSymbol.Draw(GeoBag.get_Geometry(1));
                    pSymbol.ResetDC();


                    return GeoBag.get_Geometry(1);

                case 4:
                    pPoly = new ESRI.ArcGIS.Geometry.Polygon();
                    pPolyLine = new ESRI.ArcGIS.Geometry.Polyline();


                    pPoly.AddPoint(_anchorPoint);
                    pPoly.AddPoint(pt3);
                    pPoly.AddPoint(pt2);
                    pPoly.AddPoint(pt4);
                    pPoly.AddPoint(_anchorPoint);
                    GeoBag.AddGeometry((IPolygon)pPoly);

                    LineSymbol.Width = 0;
                    lineFill.Outline = LineSymbol;
                    pSymbol = (ISymbol)lineFill;

                    pSymbol.SetupDC(hDC, transform);
                    pSymbol.Draw(GeoBag.get_Geometry(0)); // []

                    pSymbol.ResetDC();

                    pPolyLine.AddPoint(_anchorPoint);
                    pPolyLine.AddPoint(pt3);
                    GeoBag.AddGeometry((IPolyline)pPolyLine);

                    LineSymbol.Color = myColor;
                    LineSymbol.Width = 0;
                    pSymbol = (ISymbol)LineSymbol;

                    pSymbol.SetupDC(hDC, transform);
                    pSymbol.Draw(GeoBag.get_Geometry(1)); // []

                    pSymbol.ResetDC();

                    return GeoBag.get_Geometry(0);

                    //case 4:
                    //    return pt4;
            }
            //===================Draw lines ==========================================================


            // ====================== End


            return (IGeometry)GeoBag;
        }

        private IPoint CreatePoint(double x, double y)
        {
            IPoint result = new PointClass();
            result.PutCoords(x, y);
            return result;
        }

        private List<string> SplitByEnter(string text)
        {
            string curRow = "";
            List<string> result = new List<string>();
            for (int i = 0; i < text.Length; i++)
            {
                if (text[i] == ((Char)13))
                {
                    result.Add(curRow);
                    curRow = "";
                }

                if (text[i] != ((Char)10))
                {
                    curRow += text[i];
                }
            }
            result.Add(curRow);
            return result;
        }

        public IPoint PointAlongPlane(IPoint ptGeo, double dirAngle, double Dist)
        {
            dirAngle = DegToRad(dirAngle);
            IClone pClone = (IClone)ptGeo;
            IPoint result = (IPoint)pClone.Clone();
            result.PutCoords(ptGeo.X + Dist * System.Math.Cos(dirAngle), ptGeo.Y + Dist * System.Math.Sin(dirAngle));
            return result;
        }

        public double DegToRad(double deg)
        {
            return deg * Math.PI / 180.0;
        }

        public double RadToDeg(double rad)
        {
            return rad * 180.0 / Math.PI;
        }

        #region public properties   

        public IColor BackColor
        {
            get;
            set;
        }
        public esriSimpleFillStyle calloutFillStyle { get; set; }

        public double Width
        {
            get;
            set;
        }

     
        public int Snap
        {
            get;
            set;
        }

        public int Version
        {
            get;
            set;
        }

        public double Length { get; set; }
        public double Depth { get; set; }

        public double Slope { get; set; }
        public ISimpleLineSymbol LineSymbol
        {
            get;
            set;
        }
        public ISimpleFillSymbol FillSymbol
        {
            get;
            set;
        }


        public int fillColorRed { get; set; }
        public int fillColorBlue { get; set; }
        public int fillColorGreen { get; set; }


        public int ILSStyle { get; set; }
        #endregion

        public PolygonCallout()
        {
            prevAnchorPoints = new List<IPoint>();
            _fillSymbol = new SimpleFillSymbolClass(); // property
            LineSymbol = new SimpleLineSymbol(); // property
            ILSStyle = 1;
        }


    }
}
