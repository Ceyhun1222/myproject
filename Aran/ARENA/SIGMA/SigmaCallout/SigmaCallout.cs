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
	[ProgId ( SigmaCallout.id ), Guid ( "5ba0c712-66d2-44a2-ae2f-8e8628c493fa" ), ComVisible ( true )]
	public class SigmaCallout : ITextBackground, ICallout, IPersistVariant, IClone, IQueryGeometry, IDisplayName
	{
		private IEnvelope _textBox;
		private IPoint _anchorPoint;
		private List<IPoint> prevAnchorPoints;
        private IPoint pnt;
		private IGeometry _geometry;
        private ISimpleFillSymbol _fillSymbol;
		private IPoint _textBoxCenterPt;
		private ITextSymbol _textSym;
		private List<string> _strLines;
		//private ILineSymbol _leaderSymbol;
		public const string id = "Sigma.Callout";
		private string _captionText;
		private string _footerText;
		private string _morseTxt;//, MorseFullText;
		//private object _hasVerticalMorse;
		//private int _morseTagIndex;

		public delegate void MorseDelegate ( );
		public MorseDelegate MorseHandler;
		private int _hdc;
		private ITransformation _transform;

	

		#region ITextBackground Members

        public void Draw(int hDC, ITransformation transform)
        {
            _hdc = hDC;
            _transform = transform;

            IDisplayTransformation pDisplayTransform = (IDisplayTransformation)transform;

            if (this.Version < 4)
                _geometry = CreateGeometry(_textBoxCenterPt, hDC, pDisplayTransform);
            else
                _geometry = CreateGeometryAccentbar(_textBoxCenterPt, hDC, pDisplayTransform); // AccentBarCallout

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

            if (ShadowBackColor == null)
            {
                ShadowBackColor = new RgbColorClass();
                ShadowBackColor.RGB = 0;
                //ShadowBackColor.Green = 0;
                //ShadowBackColor.Blue = 0;
            }

            ISimpleFillSymbol _ShadowfillSymbol = new SimpleFillSymbolClass();
            _ShadowfillSymbol.Color = ShadowBackColor;
            _ShadowfillSymbol.Outline = LineSymbol; // shadow outline
            _ShadowfillSymbol.Style = esriSimpleFillStyle.esriSFSSolid;

            pSymbol = (ISymbol)_ShadowfillSymbol;
            pSymbol.SetupDC(hDC, transform);
            if (geom.GeometryCount >=5 && geom.get_Geometry(5) != null)
                pSymbol.Draw(geom.get_Geometry(5));
            int geomIndex = 6;
            if (HasFooter)
            {
                pSymbol.Draw(geom.get_Geometry(geomIndex));
                geomIndex++;
            }

            if (!string.IsNullOrEmpty(MorseFullText) && verticalMorse)
            {
                TextSymbolClass MorseSym = new TextSymbolClass();
                MorseSym.Leading = MorseLeading;
                MorseSym.Text = MorseFullText;
                MorseSym.XOffset = MorseLocationShiftOnX;
                MorseSym.YOffset = MorseLocationShiftOnY;
                MorseSym.HorizontalAlignment = ESRI.ArcGIS.Display.esriTextHorizontalAlignment.esriTHALeft;
                MorseSym.Angle = _textSym.Angle;
                MorseSym.SetupDC(hDC, transform);
                MorseSym.Draw(geom.get_Geometry(geomIndex));
                geomIndex++;
            }
            pSymbol.ResetDC();

            #region leader line

            IPointCollection pPolyline = new PolylineClass();
            pPolyline = (PolylineClass)geom.get_Geometry(3);

            IPoint p1 = pPolyline.Point[0];
            IPoint p2 = pPolyline.Point[1];
            IPointCollection pLidLine = new PolylineClass();


            ((IFillSymbol)pSymbol).Outline = LineSymbol;
            pSymbol.SetupDC(hDC, transform);

            if (DrawLeader && geom.get_Geometry(3) != null)
                pSymbol.Draw(geom.get_Geometry(3));

            pSymbol.ResetDC();



            #endregion


            #region  airspace symbol

            if (this.AirspaceSignSize > 0)
            {
                TextSymbolClass textSymbolMarkerSign = new TextSymbolClass();

                textSymbolMarkerSign.Angle = 0;//_textSym.Angle;

                // цвет текста
                IRgbColor Clr = new RgbColorClass();
                Clr.Red = this.AirspaceFontColorRed;
                Clr.Green = this.AirspaceFontColorGreen;
                Clr.Blue = this.AirspaceFontColorBlue;
                textSymbolMarkerSign.Color = Clr;
                // textSymbolMarkerSign.WordSpacing = 50;


                IMarkerTextBackground TxtBackGrndMarkerSign = new MarkerTextBackground();
                TxtBackGrndMarkerSign.ScaleToFit = this.AirspaceSignScaleToFit;

                ICharacterMarkerSymbol characterMarkerSymbolMarkerSign = new CharacterMarkerSymbolClass();


                stdole.IFontDisp stdFont = new stdole.StdFontClass() as stdole.IFontDisp;
                stdFont.Name = "AeroSigma";

                characterMarkerSymbolMarkerSign.Font = stdFont;
                characterMarkerSymbolMarkerSign.Angle = _textSym.Angle;
                characterMarkerSymbolMarkerSign.CharacterIndex = 199; // 

                //размер знака символа
                characterMarkerSymbolMarkerSign.Size = this.AirspaceSignSize;


                //цвет заливки
                //IRgbColor FillClr = new RgbColorClass();
                //FillClr.Red = this.AirspaceBackColor.Red;
                //FillClr.Green = this.AirspaceBackColor.Green;
                //FillClr.Blue = this.AirspaceBackColor.Blue;

                characterMarkerSymbolMarkerSign.Color = this.AirspaceBackColor;
                characterMarkerSymbolMarkerSign.XOffset = 0;
                characterMarkerSymbolMarkerSign.YOffset = 0;
                characterMarkerSymbolMarkerSign.Angle = 0;// _textSym.Angle;

                TxtBackGrndMarkerSign.Symbol = characterMarkerSymbolMarkerSign;
                textSymbolMarkerSign.Background = TxtBackGrndMarkerSign;


                ISimpleTextSymbol simpleTextSymbolMarkerSign = (ISimpleTextSymbol)textSymbolMarkerSign;

                simpleTextSymbolMarkerSign.HorizontalAlignment = esriTextHorizontalAlignment.esriTHACenter;
                simpleTextSymbolMarkerSign.VerticalAlignment = esriTextVerticalAlignment.esriTVACenter;
                simpleTextSymbolMarkerSign.Angle = _textSym.Angle;


                // размер и характеристика шрифта для внутреннего текста
                stdole.IFontDisp pFontDisp = new stdole.StdFont() as stdole.IFontDisp;
                pFontDisp.Name = this.AirsapceFontName;
                pFontDisp.Italic = this.AirspaceFontItalic;
                pFontDisp.Bold = this.AirspaceFontBold;
                pFontDisp.Underline = this.AirspaceFontUnderLine;
                pFontDisp.Size = (decimal)this.AirspaceFontSize;
                simpleTextSymbolMarkerSign.Font = pFontDisp;


                ITextElement pTextEl_MarkerSign = new TextElementClass();
                string[] arspcClasse = AirspaceSymbols.Split(',');

                if (arspcClasse != null && arspcClasse.Length > 0)
                {
                    foreach (var item in arspcClasse)
                    {
                        pTextEl_MarkerSign.Text = pTextEl_MarkerSign.Text + item + (char)13 + (char)10;
                    }
                    pTextEl_MarkerSign.Text = pTextEl_MarkerSign.Text.Remove(pTextEl_MarkerSign.Text.Length - 2, 2);
                }
                else
                    pTextEl_MarkerSign.Text = "-";

                pTextEl_MarkerSign.ScaleText = false;
                pTextEl_MarkerSign.Symbol = simpleTextSymbolMarkerSign;
                pTextEl_MarkerSign.Symbol.VerticalAlignment = esriTextVerticalAlignment.esriTVACenter;
                pTextEl_MarkerSign.Symbol.HorizontalAlignment = esriTextHorizontalAlignment.esriTHACenter;

                ISymbol CharSym = (ISymbol)pTextEl_MarkerSign.Symbol;
                CharSym.SetupDC(hDC, transform);
                if (geom.get_Geometry(4) != null)
                    CharSym.Draw(geom.get_Geometry(4));
                CharSym.ResetDC();
            }

            #endregion


        }

		public void QueryBoundary ( int hDC, ITransformation transform, ESRI.ArcGIS.Geometry.IPolygon boundary )
		{
            if (_textSym == null) return;
           

            ISymbol pSymbol = (ISymbol) FillSymbol;


            if (this.Version < 4)
                _geometry = CreateGeometry(_textBoxCenterPt, hDC, transform);
            else
                _geometry = CreateGeometryAccentbar(_textBoxCenterPt, hDC, transform); // AccentBarCallout

            IGeometryCollection geomColl = ( IGeometryCollection ) _geometry;
            if (geomColl == null) return;

            if (pSymbol != null)
            {
                pSymbol.QueryBoundary(hDC, transform, geomColl.get_Geometry(0), boundary);

                ITopologicalOperator2 pTopoOp = (ITopologicalOperator2)boundary;
                pTopoOp.IsKnownSimple_2 = false;
                pTopoOp.Simplify();
            }

		}

		public IEnvelope TextBox
		{
			set
			{
				_textBox = value;
				if ( value == null )
					return;

				_textBoxCenterPt = new PointClass ( );
				_textBoxCenterPt.X = 0.5 * ( _textBox.XMin + _textBox.XMax );
				_textBoxCenterPt.Y = 0.5 * ( _textBox.YMin + _textBox.YMax );
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
				if ( value == null )
					return;
				if ( value.Equals ( _textSym ) )
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
				if ( _anchorPoint == null )
					return null;
				IClone p = ( IClone ) _anchorPoint;
				return ( IPoint ) p.Clone ( );
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
				var uid = new UIDClass ( );
				uid.Value = id;
				return uid;
			}
		}

		void IPersistVariant.Load ( IVariantStream Stream )
		{

			Version = Convert.ToInt32 ( Stream.Read ( ) );
			//if ( version != 1 )
			//	throw new Exception ( "Failed to read from stream" );

			AnchorPoint = ( IPoint ) Stream.Read ( );
			LeaderTolerance = Convert.ToDouble ( Stream.Read ( ) );
			BackColor = ( IRgbColor ) Stream.Read ( );
			LineSymbol = ( ISimpleLineSymbol ) Stream.Read ( );
			//LeaderSymbol = ( ISimpleLineSymbol ) Stream.Read ( );

			Shadow = Convert.ToInt32 ( Stream.Read ( ) );
			ShadowBackColor = ( IRgbColor ) Stream.Read ( );
			AirspaceBackColor = ( IRgbColor ) Stream.Read ( );
			AirspaceSymbols = Convert.ToString ( Stream.Read ( ) );
			AirspaceOnLeftSide = Convert.ToBoolean ( Stream.Read ( ) );
			Snap = Convert.ToInt32 ( Stream.Read ( ) );
			Width = Convert.ToDouble ( Stream.Read ( ) );
			HasHeader = Convert.ToBoolean ( Stream.Read ( ) );
			HasFooter = Convert.ToBoolean ( Stream.Read ( ) );
			TopMargin = Convert.ToDouble ( Stream.Read ( ) );
			HeaderHorizontalMargin = Convert.ToDouble ( Stream.Read ( ) );
			FooterHorizontalMargin = Convert.ToDouble ( Stream.Read ( ) );
			BottomMargin = Convert.ToDouble ( Stream.Read ( ) );
			//_hasVerticalMorse = Convert.ToBoolean ( Stream.Read ( ) );
			MorseLocationShiftOnX = Convert.ToDouble ( Stream.Read ( ) );
			MorseLocationShiftOnY = Convert.ToDouble ( Stream.Read ( ) );
			MorseLeading = Convert.ToDouble ( Stream.Read ( ) );
			DrawLeader = Convert.ToBoolean ( Stream.Read ( ) );
            calloutFillStyle = (Stream.Read() as ISimpleFillSymbol).Style;
            AirspaceSignSize = Convert.ToInt32(Stream.Read());
            AirsapceFontName = (Stream.Read() as string);
            AirspaceFontSize = Convert.ToDouble(Stream.Read()); 
            AirspaceFontColorRed = Convert.ToInt32(Stream.Read());
            AirspaceFontColorBlue = Convert.ToInt32(Stream.Read());
            AirspaceFontColorGreen = Convert.ToInt32(Stream.Read());
            AirspaceFontItalic = (bool)Stream.Read();
            AirspaceFontUnderLine = (bool)Stream.Read();
            AirspaceFontBold = (bool)Stream.Read();
            AirspaceSignScaleToFit = (bool)Stream.Read();
            verticalMorse = Convert.ToBoolean(Stream.Read());
            AccentBarPosition = Convert.ToInt32(Stream.Read());
        }

        void IPersistVariant.Save(IVariantStream Stream)
        {
            Stream.Write(Version);
            Stream.Write(AnchorPoint);
            Stream.Write(LeaderTolerance);
            Stream.Write(BackColor);
            Stream.Write(LineSymbol);
            //Stream.Write ( LeaderSymbol );
            Stream.Write(Shadow);
            Stream.Write(ShadowBackColor);
            Stream.Write(AirspaceBackColor);
            Stream.Write(AirspaceSymbols);
            Stream.Write(AirspaceOnLeftSide);
            Stream.Write(Snap);
            Stream.Write(Width);
            Stream.Write(HasHeader);
            Stream.Write(HasFooter);
            Stream.Write(TopMargin);
            Stream.Write(HeaderHorizontalMargin);
            Stream.Write(FooterHorizontalMargin);
            Stream.Write(BottomMargin);
            //Stream.Write ( _hasVerticalMorse );
            Stream.Write(MorseLocationShiftOnX);
            Stream.Write(MorseLocationShiftOnY);
            Stream.Write(MorseLeading);
            Stream.Write(DrawLeader);
            Stream.Write(_fillSymbol);

            Stream.Write(AirspaceSignSize);
            Stream.Write(AirsapceFontName);
            Stream.Write(AirspaceFontSize);
            Stream.Write(AirspaceFontColorRed);
            Stream.Write(AirspaceFontColorBlue);
            Stream.Write(AirspaceFontColorGreen);
            Stream.Write(AirspaceFontItalic);
            Stream.Write(AirspaceFontUnderLine);
            Stream.Write(AirspaceFontBold);
            Stream.Write(AirspaceSignScaleToFit);
            Stream.Write(verticalMorse);
            Stream.Write(AccentBarPosition);

        }

		#endregion

		#region IClone Members

		public void Assign ( IClone src )
		{
			// Make sure src is pointing to a valid object.
			if ( src == null )
				throw new System.Runtime.InteropServices.COMException ( "Invalid object" );

			if ( !( src is SigmaCallout ) )
				throw new System.Runtime.InteropServices.COMException ( "Bad object type" );


			var srcClonable = ( SigmaCallout ) src;
			_anchorPoint = srcClonable.AnchorPoint;
			BackColor = srcClonable.BackColor;
			LineSymbol = srcClonable.LineSymbol;
			AirspaceBackColor = srcClonable.AirspaceBackColor;
			AirspaceSymbols = srcClonable.AirspaceSymbols;
			AirspaceOnLeftSide = srcClonable.AirspaceOnLeftSide;
			//_leaderSymbol = srcClonable._leaderSymbol;
			Snap = srcClonable.Snap;
			Width = srcClonable.Width;
			Shadow = srcClonable.Shadow;
			ShadowBackColor = srcClonable.ShadowBackColor;
			HasHeader = srcClonable.HasHeader;
			HasFooter = srcClonable.HasFooter;
			LeaderTolerance = srcClonable.LeaderTolerance;
			TopMargin = srcClonable.TopMargin;
			HeaderHorizontalMargin = srcClonable.HeaderHorizontalMargin;
			FooterHorizontalMargin = srcClonable.FooterHorizontalMargin;
			BottomMargin = srcClonable.BottomMargin;
			//MorseVertical = srcClonable.MorseVertical;
			MorseLocationShiftOnX = srcClonable.MorseLocationShiftOnX;
			MorseLocationShiftOnY = srcClonable.MorseLocationShiftOnY;
			MorseLeading = srcClonable.MorseLeading;
			DrawLeader = srcClonable.DrawLeader;

            AirspaceFontSize = srcClonable.AirspaceFontSize;
            AirsapceFontName = srcClonable.AirsapceFontName;
            AirspaceFontColorRed = srcClonable.AirspaceFontColorRed;
            AirspaceFontColorGreen = srcClonable.AirspaceFontColorGreen;
            AirspaceFontColorBlue = srcClonable.AirspaceFontColorBlue;
            AirspaceFontItalic = srcClonable.AirspaceFontItalic;
            AirspaceFontBold = srcClonable.AirspaceFontBold;
            AirspaceFontUnderLine = srcClonable.AirspaceFontUnderLine;
            calloutFillStyle = srcClonable.calloutFillStyle;
            AirspaceSignSize = srcClonable.AirspaceSignSize;
            AirspaceSignScaleToFit = srcClonable.AirspaceSignScaleToFit;
            _fillSymbol.Style = srcClonable.calloutFillStyle;
            verticalMorse = srcClonable.verticalMorse;
            Version = srcClonable.Version;
            AccentBarPosition = srcClonable.AccentBarPosition;


        }

		public IClone Clone ( )
		{
			SigmaCallout result = new SigmaCallout ( );
			result.Assign ( this );
			return result;
		}
        

		public bool IsEqual ( IClone other )
		{
			if ( other == null )
				throw new System.Runtime.InteropServices.COMException ( "Invalid object" );

			if ( !( other is SigmaCallout ) )
				throw new System.Runtime.InteropServices.COMException ( "Bad object type" );

			var otherClonable = ( SigmaCallout ) other;
			if ( Version == otherClonable.Version &&
				LineSymbol == otherClonable.LineSymbol &&
				Snap == otherClonable.Snap &&
				Width == otherClonable.Width &&
				LeaderTolerance == otherClonable.LeaderTolerance &&
				TopMargin == otherClonable.TopMargin &&
				HeaderHorizontalMargin == otherClonable.HeaderHorizontalMargin &&
				FooterHorizontalMargin == otherClonable.FooterHorizontalMargin &&
				AirspaceOnLeftSide == otherClonable.AirspaceOnLeftSide &&
				BottomMargin == otherClonable.BottomMargin &&
				NameString == otherClonable.NameString &&
				HasHeader == otherClonable.HasHeader &&
				HasFooter == otherClonable.HasFooter &&
				DrawLeader == otherClonable.DrawLeader &&
				//MorseVertical == otherClonable.MorseVertical &&
				MorseLocationShiftOnX == otherClonable.MorseLocationShiftOnX &&
				MorseLocationShiftOnY == otherClonable.MorseLocationShiftOnY &&
				MorseLeading == otherClonable.MorseLeading &&
				Shadow == otherClonable.Shadow &&
                AccentBarPosition == otherClonable.AccentBarPosition)
			{
				IClone tmp = ( IClone ) _anchorPoint;
				IClone tmp2 = ( IClone ) otherClonable.AnchorPoint;
				if ( tmp.IsEqual ( tmp2 ) )
				{
					tmp = ( IClone ) LineSymbol;
					tmp2 = ( IClone ) otherClonable.LineSymbol;
					if ( tmp.IsEqual ( tmp2 ) )
					{
						tmp = ( IClone ) BackColor;
						tmp2 = ( IClone ) otherClonable.BackColor;
						if ( tmp.IsEqual ( tmp2 ) )
						{
							tmp = ( IClone ) ShadowBackColor;
							tmp2 = ( IClone ) otherClonable.ShadowBackColor;
							if ( tmp.IsEqual ( tmp2 ) )
								return true;
						}
					}
				}
			}
			return false;
		}

		public bool IsIdentical ( IClone other )
		{
			if ( other == null )
				throw new System.Runtime.InteropServices.COMException ( "Invalid object" );

			if ( !( other is SigmaCallout ) )
				throw new System.Runtime.InteropServices.COMException ( "Bad object type" );

			if ( ( ( SigmaCallout ) other ) == this )
				return true;
			return false;
		}

		#endregion

		#region IQueryGeometry Members

		public IGeometry GetGeometry ( int hDC, ITransformation displayTransform, IGeometry drawGeometry )
		{
            //return CreateGeometry ( _textBoxCenterPt, hDC, displayTransform );

            if (this.Version < 4)
                return CreateGeometry(_textBoxCenterPt, hDC, displayTransform);
            else
                return CreateGeometryAccentbar(_textBoxCenterPt, hDC, displayTransform); // AccentBarCallout
        }

		public void QueryEnvelope ( int hDC, ITransformation displayTransform, IGeometry drawGeometry, IEnvelope envelope )
		{
            //envelope = CreateGeometry ( _textBoxCenterPt, hDC, displayTransform ).Envelope;
            if (this.Version < 4)
                envelope = CreateGeometry(_textBoxCenterPt, hDC, displayTransform).Envelope;
            else
                envelope = CreateGeometryAccentbar(_textBoxCenterPt, hDC, displayTransform).Envelope; // AccentBarCallout
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

		private IPointCollection FindOutsidePoint ( IPoint _textBoxCenterPt, int hDC, ITransformation transform )
		{
			IDisplayTransformation pDisplayTransformation = ( IDisplayTransformation ) transform;
			esriUnits units = pDisplayTransformation.Units;
			double scasle, scasleRatio;

			if ( units != esriUnits.esriUnknownUnits )
			{
				scasle = pDisplayTransformation.ReferenceScale;
				scasleRatio = pDisplayTransformation.ScaleRatio;
			}
			else
				scasleRatio = 1.0;

			TextSymbol textSym = ( TextSymbol ) _textSym;
			_strLines = SplitByEnter ( _textSym.Text );
			_morseTxt = string.Empty;
			double morseElemWidth = double.NaN, morseElemHeight = double.NaN;
			if ( _strLines[ _strLines.Count - 1 ].StartsWith ( "\r<Sigma" ) )
				CreateMorseText ( _strLines[ _strLines.Count - 1 ], hDC, transform, out morseElemWidth, out morseElemHeight );

			// Create polygon =====================================
			IPoint BasePoint;
			double Hcap = 0, Wcap = 0, Hcont = 0, Wcont = 0, Hbot = 0, Wbot = 0, Wmax;
			int iCap = 0, iCont = 0, iBot = 0;
			int contStartIndex = 0, contEndIndex = _strLines.Count - 1;
			double leading = pDisplayTransformation.FromPoints ( textSym.Leading );
			double morseX, morseY;
			if ( HasHeader )
			{
				iCap = Text_Pars ( hDC, transform, new string[] { _strLines[ 0 ] }, leading, out Wcap, out Hcap, out morseX, out morseY );
				contStartIndex = 1;
			}
			else
				HeaderHorizontalMargin = 0;

			if ( HasFooter )
			{
				if ( string.IsNullOrEmpty ( _morseTxt ) )
					iBot = Text_Pars ( hDC, transform, new string[] { _strLines[ _strLines.Count - 1 ] }, leading, out Wbot, out Hbot, out morseX, out morseY );
				else
					iBot = Text_Pars ( hDC, transform, new string[] { _strLines[ _strLines.Count - 2 ] }, leading, out Wbot, out Hbot, out morseX, out morseY );
				contEndIndex--;
			}
			else
				FooterHorizontalMargin = 0;
			List<string> resCont = new List<string> ( );
			for ( int k = contStartIndex; k <= contEndIndex; k++ )
			{
				resCont.Add ( _strLines[ k ] );
			}
			iCont = Text_Pars ( hDC, transform, resCont.ToArray ( ), leading, out Wcont, out Hcont, out morseX, out morseY );

			double width = pDisplayTransformation.FromPoints ( Width ); //prop
			double ShadowMargine = pDisplayTransformation.FromPoints ( this.Shadow );

			double _dHeaderMargine = pDisplayTransformation.FromPoints ( TopMargin );
			double _dFooterMargine = pDisplayTransformation.FromPoints ( BottomMargin );

			double _dHeaderTextMargine = pDisplayTransformation.FromPoints ( HeaderHorizontalMargin );
			double _dFooterTextMargine = pDisplayTransformation.FromPoints ( FooterHorizontalMargin );

            Wcont = pDisplayTransformation.FromPoints ( Wcont );
			Hcont = pDisplayTransformation.FromPoints ( Hcont );

			Wcap = pDisplayTransformation.FromPoints ( Wcap );
			Hcap = pDisplayTransformation.FromPoints ( Hcap );

			Wbot = pDisplayTransformation.FromPoints ( Wbot );
			Hbot = pDisplayTransformation.FromPoints ( Hbot );


			Wmax = Math.Max ( Wcont, Wcap );
			double textAngle = _textSym.Angle + 180.0;
			double Hsum = Hcont + Hcap + Hbot + leading * ( iCont + iCap + iBot - 1 );

			Wmax *= 1.1;
			Hsum *= 1.2;
			BasePoint = PointAlongPlane ( _textBoxCenterPt, textAngle - 90.0, 0.5 * ( Hsum - Hcap ) + _dHeaderMargine );
			IPoint MorsePnt = null;
			if ( !double.IsNaN ( morseX ) )
			{
				morseX = pDisplayTransformation.FromPoints ( morseX );
				morseY = pDisplayTransformation.FromPoints ( morseY );
				//morseElemWidth = pDisplayTransformation.FromPoints ( morseElemWidth );
				//morseElemHeight = pDisplayTransformation.FromPoints ( morseElemHeight );
				MorsePnt = PointAlongPlane ( _textBoxCenterPt, textAngle - 180, 0.7 * morseX );
				//MorsePnt = PointAlongPlane ( MorsePnt, textAngle - 90, ( Hsum - Hcap ) * 0.5 + morseY);
				//MorsePnt = _textBoxCenterPt;
			}

			IPoint pt1 = PointAlongPlane ( BasePoint, textAngle, 0.5 * Wmax + width );
			IPoint pt2 = PointAlongPlane ( pt1, textAngle + 90.0, Hsum - 0.5 * ( Hcap + Hbot ) + _dHeaderMargine + _dFooterMargine );
			IPoint pt3 = PointAlongPlane ( pt2, textAngle + 180.0, Wmax + 2 * width );
			IPoint pt4 = PointAlongPlane ( BasePoint, textAngle + 180.0, 0.5 * Wmax + width );
			IPointCollection pPoly = new ESRI.ArcGIS.Geometry.Polygon ( );

			pPoly.AddPoint ( pt1 );
			pPoly.AddPoint ( pt2 );
			pPoly.AddPoint ( pt3 );
			pPoly.AddPoint ( pt4 );
			pPoly.AddPoint ( pt1 );
			return pPoly;
		}

		private IGeometry CreateGeometry ( IPoint _textBoxCenterPt, int hDC, ITransformation transform )
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


			IDisplayTransformation pDisplayTransformation = ( IDisplayTransformation ) transform;
			esriUnits units = pDisplayTransformation.Units;
			double scasle, scasleRatio;

			if ( units != esriUnits.esriUnknownUnits )
			{
				scasle = pDisplayTransformation.ReferenceScale;
				scasleRatio = pDisplayTransformation.ScaleRatio;
			}
			else
				scasleRatio = 1.0;

			TextSymbol textSym = ( TextSymbol ) _textSym;
			_strLines = SplitByEnter ( _textSym.Text );
			_morseTxt = string.Empty;
			double morseElemWidth = double.NaN, morseElemHeight = double.NaN;
			if ( _strLines[ _strLines.Count - 1 ].StartsWith ( "\r<Sigma" ) )
				CreateMorseText ( _strLines[ _strLines.Count - 1 ], hDC, transform, out morseElemWidth, out morseElemHeight );
			if ( MorseHandler != null )
				MorseHandler ( );
			//_hasVerticalMorse = !string.IsNullOrEmpty ( _morseTxt );

			// Create polygon =====================================
			IPoint BasePoint;
			double Hcap = 0, Wcap = 0, Hcont = 0, Wcont = 0, Hbot = 0, Wbot = 0, Wmax;
			int iCap = 0, iCont = 0, iBot = 0;
			int contStartIndex = 0, contEndIndex = _strLines.Count - 1;
			double leading = pDisplayTransformation.FromPoints ( textSym.Leading );
			double morseX, morseY;
			if ( HasHeader )
			{
				iCap = Text_Pars ( hDC, transform, new string[] { _strLines[ 0 ] }, leading, out Wcap, out Hcap, out morseX, out morseY );
				contStartIndex = 1;
			}
			else
				HeaderHorizontalMargin = 0;

			if ( HasFooter )
			{
                if (string.IsNullOrEmpty(_morseTxt))
                    iBot = Text_Pars(hDC, transform, new string[] { _strLines[_strLines.Count - 1] }, leading, out Wbot, out Hbot, out morseX, out morseY);
                else
                {
                    iBot = Text_Pars(hDC, transform, new string[] { _strLines[_strLines.Count - 2] }, leading, out Wbot, out Hbot, out morseX, out morseY);
                    contEndIndex--;
                }
				contEndIndex--;
			}
			else
				FooterHorizontalMargin = 0;
			List<string> resCont = new List<string> ( );
			for ( int k = contStartIndex; k <= contEndIndex; k++ )
			{
				resCont.Add ( _strLines[ k ] );
			}
			iCont = Text_Pars ( hDC, transform, resCont.ToArray ( ), leading, out Wcont, out Hcont, out morseX, out morseY );

            //TopMargin = TopMargin + 1;
            //BottomMargin = BottomMargin + 1;

			double width = pDisplayTransformation.FromPoints ( Width ); //prop
			double ShadowMargine = pDisplayTransformation.FromPoints ( this.Shadow );

			double _dHeaderMargine = pDisplayTransformation.FromPoints ( TopMargin );
			double _dFooterMargine = pDisplayTransformation.FromPoints ( BottomMargin );

			double _dHeaderTextMargine = pDisplayTransformation.FromPoints ( HeaderHorizontalMargin );
			double _dFooterTextMargine = pDisplayTransformation.FromPoints ( FooterHorizontalMargin );

			Wcont = pDisplayTransformation.FromPoints ( Wcont );
			Hcont = pDisplayTransformation.FromPoints ( Hcont );

			Wcap = pDisplayTransformation.FromPoints ( Wcap );
			Hcap = pDisplayTransformation.FromPoints ( Hcap );

			Wbot = pDisplayTransformation.FromPoints ( Wbot );
			Hbot = pDisplayTransformation.FromPoints ( Hbot );


			Wmax = Math.Max ( Wcont, Wcap );
			double textAngle = _textSym.Angle + 180.0;
			double Hsum = Hcont + Hcap + Hbot + leading * ( iCont + iCap + iBot - 1 );

			Wmax *= 1.1;
			//Hsum *= 1.2;
			BasePoint = PointAlongPlane ( _textBoxCenterPt, textAngle - 90.0, 0.5 * ( Hsum - Hcap ) + _dHeaderMargine );
			IPoint MorsePnt = null;
			if ( !double.IsNaN ( morseX ) )
			{
				morseX = pDisplayTransformation.FromPoints ( morseX );
				morseY = pDisplayTransformation.FromPoints ( morseY );
				//morseElemWidth = pDisplayTransformation.FromPoints ( morseElemWidth );
				//morseElemHeight = pDisplayTransformation.FromPoints ( morseElemHeight );
				MorsePnt = PointAlongPlane ( _textBoxCenterPt, textAngle - 180, 0.7 * morseX );
				//MorsePnt = PointAlongPlane ( MorsePnt, textAngle - 90, ( Hsum - Hcap ) * 0.5 + morseY);
				//MorsePnt = _textBoxCenterPt;
			}

			IPoint pt1 = PointAlongPlane ( BasePoint, textAngle, 0.5 * Wmax + width );
			IPoint pt2 = PointAlongPlane ( pt1, textAngle + 90.0, Hsum - 0.5 * ( Hcap + Hbot ) + _dHeaderMargine + _dFooterMargine );
			IPoint pt3 = PointAlongPlane ( pt2, textAngle + 180.0, Wmax + 2 * width );
			IPoint pt4 = PointAlongPlane ( BasePoint, textAngle + 180.0, 0.5 * Wmax + width );
			IPointCollection pPoly = new ESRI.ArcGIS.Geometry.Polygon ( );

			pPoly.AddPoint ( pt1 );
			pPoly.AddPoint ( pt2 );
			pPoly.AddPoint ( pt3 );
			pPoly.AddPoint ( pt4 );
			pPoly.AddPoint ( pt1 );

			// ====================== End

			//======================= Shadow ================================= 
			IPointCollection pShadowPoly = new Polygon ( );
			IPointCollection pShadowPolyR = new Polygon ( );
			IPoint ptTmp;
			if ( !HasFooter )
			{
				pShadowPoly.AddPoint ( pt2 );
				pShadowPoly.AddPoint ( pt3 );
				pShadowPoly.AddPoint ( pt4 );

				ptTmp = PointAlongPlane ( pt4, textAngle + 180.0 - 45.0, ShadowMargine * Math.Sqrt ( 2.0 ) );
				pShadowPoly.AddPoint ( ptTmp );

				ptTmp = PointAlongPlane ( pt3, textAngle + 180.0 - 45.0, ShadowMargine * Math.Sqrt ( 2.0 ) );
				pShadowPoly.AddPoint ( ptTmp );

				ptTmp = PointAlongPlane ( pt2, textAngle + 180.0 - 45.0, ShadowMargine * Math.Sqrt ( 2.0 ) );
				pShadowPoly.AddPoint ( ptTmp );

				pShadowPoly.AddPoint ( pt2 );
			}
			else
			{
				pShadowPoly.AddPoint ( pt2 );
				ptTmp = PointAlongPlane ( pt2, textAngle + 180.0, width + 0.5 * ( Wmax - Wbot ) - _dFooterTextMargine );
				pShadowPoly.AddPoint ( ptTmp );
				ptTmp = PointAlongPlane ( ptTmp, textAngle + 90.0 + 45.0, ShadowMargine * Math.Sqrt ( 2.0 ) );
				pShadowPoly.AddPoint ( ptTmp );
				ptTmp = PointAlongPlane ( pt2, textAngle + 180.0 - 45.0, ShadowMargine * Math.Sqrt ( 2.0 ) );
				pShadowPoly.AddPoint ( ptTmp );

				pShadowPoly.AddPoint ( pShadowPoly.Point[ 0 ] );

				ptTmp = PointAlongPlane ( pt3, textAngle, width + 0.5 * ( Wmax - Wbot ) - _dFooterTextMargine );
				pShadowPolyR.AddPoint ( ptTmp );
				pShadowPolyR.AddPoint ( pt3 );
				pShadowPolyR.AddPoint ( pt4 );
				ptTmp = PointAlongPlane ( pt4, textAngle + 180.0 - 45.0, ShadowMargine * Math.Sqrt ( 2.0 ) );
				pShadowPolyR.AddPoint ( ptTmp );

				ptTmp = PointAlongPlane ( pt3, textAngle + 180.0 - 45.0, ShadowMargine * Math.Sqrt ( 2.0 ) );
				pShadowPolyR.AddPoint ( ptTmp );

				ptTmp = PointAlongPlane ( ptTmp, textAngle, width + 0.5 * ( Wmax - Wbot ) - _dFooterTextMargine );
				pShadowPolyR.AddPoint ( ptTmp );

				pShadowPolyR.AddPoint ( pShadowPolyR.Point[ 0 ] );

			}

			//======================= End Shadow =====================================

			// ====================== AirSpace Class Marker  // сделать if
			IPoint AirClass;
			if ( AirspaceOnLeftSide )
				AirClass = CreatePoint ( ( pt1.X + pt2.X ) / 2, ( pt1.Y + pt2.Y ) / 2 );
			else
				AirClass = CreatePoint ( ( pt3.X + pt4.X ) / 2, ( pt3.Y + pt4.Y ) / 2 );
			//AirClass = _textBoxCenterPt;
			// ====================== End

			//======================= LeftLine ===================
			IPointCollection pOutLineL = new Polyline ( );
			if ( iCap == 0 )
				_dHeaderTextMargine = 0;
			pt1 = PointAlongPlane ( BasePoint, textAngle, 0.5 * Wcap + _dHeaderTextMargine );//======= убрать
            pt2 = PointAlongPlane ( BasePoint, textAngle, 0.5 * Wmax + width ); //======= оставить
			pt3 = PointAlongPlane ( pt2, textAngle + 90.0, Hsum - 0.5 * ( Hcap + Hbot ) + _dHeaderMargine + _dFooterMargine );//======= оставить
            if ( HasFooter )
				pt4 = PointAlongPlane ( pt3, textAngle + 180.0, 0.5 * ( Wmax - Wbot ) + width - _dFooterTextMargine );//======= убрать
            else
				pt4 = PointAlongPlane ( pt3, textAngle + 180.0, 0.6 * ( Wmax - Wbot ) + width - _dFooterTextMargine );//======= убрать

            pOutLineL.AddPoint ( pt1 );
			pOutLineL.AddPoint ( pt2 );
			pOutLineL.AddPoint ( pt3 );
			pOutLineL.AddPoint ( pt4 );
			//======================= End LeftLine ===================


			//======================= RightLine ===================
			IPointCollection pOutLineR = new Polyline ( );
			pt1 = PointAlongPlane ( BasePoint, textAngle + 180.0, 0.5 * Wcap + _dHeaderTextMargine );//======= убрать
            pt2 = PointAlongPlane ( BasePoint, textAngle + 180.0, 0.5 * Wmax + width );//======= оставить
            pt3 = PointAlongPlane ( pt2, textAngle + 90.0, Hsum - 0.5 * ( Hcap + Hbot ) + _dHeaderMargine + _dFooterMargine );//======= оставить
            if ( HasFooter )
				pt4 = PointAlongPlane ( pt3, textAngle, 0.5 * ( Wmax - Wbot ) + width - _dFooterTextMargine );//======= убрать
            else
				pt4 = PointAlongPlane ( pt3, textAngle, 0.6 * ( Wmax - Wbot ) + width - _dFooterTextMargine );//======= убрать

            pOutLineR.AddPoint ( pt1 );
			pOutLineR.AddPoint ( pt2 );
			pOutLineR.AddPoint ( pt3 );
			pOutLineR.AddPoint ( pt4 );
            //======================= End RightLine ===================

            //======================= Leader Line  // сделать if

            double _gap = pDisplayTransformation.FromPoints(LeaderTolerance);


            ITopologicalOperator pTopoOp = ( ITopologicalOperator ) pPoly;
            IPoint prevAnchorPoint = _anchorPoint;


            //if (prevAnchorPoint == null) return null;
			IPointCollection pLiderLine = new Polyline ( );
			IPoint LiderPt = CalcLeaderPoint ( Snap, pPoly, Hcap, Hbot, prevAnchorPoint ); //   1 Parametr Snap Position Leader Line (0 - 9)  left =7, right = 9

            ILine gapLine = new LineClass();
            gapLine.FromPoint = prevAnchorPoint;
            gapLine.ToPoint = LiderPt;

            if (gapLine.Length < _gap) _gap = gapLine.Length * 0.8;

            double anglGap = gapLine.Angle * 180 / Math.PI;
            IPoint ptgap = PointAlongPlane(prevAnchorPoint, anglGap, _gap);


            pLiderLine.AddPoint(ptgap);
            pLiderLine.AddPoint(LiderPt);



            pTopoOp.Simplify ( );


            
            var intersectPntColl = (IPointCollection)pTopoOp.Intersect((IGeometry)pLiderLine, esriGeometryDimension.esriGeometry0Dimension);
            double tmp;
            if (intersectPntColl != null && intersectPntColl.PointCount > 0)
            {
                IPoint CutPoint = pLiderLine.Point[1];
                CutPoint = intersectPntColl.Point[0];
                if (intersectPntColl.PointCount == 2)
                {
                    tmp = Math.Sqrt((intersectPntColl.Point[0].X - prevAnchorPoint.X) * (intersectPntColl.Point[0].X - prevAnchorPoint.X) + (intersectPntColl.Point[0].Y - prevAnchorPoint.Y) * (intersectPntColl.Point[0].Y - prevAnchorPoint.Y));
                    var t = Math.Sqrt((intersectPntColl.Point[1].X - prevAnchorPoint.X) * (intersectPntColl.Point[1].X - prevAnchorPoint.X) + (intersectPntColl.Point[1].Y - prevAnchorPoint.Y) * (intersectPntColl.Point[1].Y - prevAnchorPoint.Y));
                    if (tmp > t)
                    {
                        CutPoint = intersectPntColl.Point[1];
                    }
                }
                pLiderLine.ReplacePoints(1, 1, 1, ref CutPoint);
            }

			LiderPt = pLiderLine.Point[ 1 ];
            IRelationalOperator relOp = (IRelationalOperator)prevAnchorPoint;
            DrawLeader = DrawLeader && !relOp.Within((IPolygon)pPoly);

            //======================= End Leader Line

			IGeometryCollection GeoBag = new GeometryBagClass ( );
			GeoBag.AddGeometry ( ( IPolygon ) pPoly );
			GeoBag.AddGeometry ( ( IPolyline ) pOutLineL );
			GeoBag.AddGeometry ( ( IPolyline ) pOutLineR );


			GeoBag.AddGeometry ( ( IGeometry ) pLiderLine );
			GeoBag.AddGeometry ( AirClass );

			GeoBag.AddGeometry ( ( IGeometry ) pShadowPoly );
			if ( HasFooter )
				GeoBag.AddGeometry ( ( IGeometry ) pShadowPolyR );
			if ( MorsePnt != null )
				GeoBag.AddGeometry ( MorsePnt );
			return ( IGeometry ) GeoBag;
		}

        private IGeometry CreateGeometryAccentbar(IPoint _textBoxCenterPt, int hDC, ITransformation transform)
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

            TextSymbol textSym = (TextSymbol)_textSym;
            _strLines = SplitByEnter(_textSym.Text);
            _morseTxt = string.Empty;


            // Create polygon =====================================
            IPoint BasePoint;
            double Hcap = 0, Wcap = 0, Hcont = 0, Wcont = 0, Hbot = 0, Wbot = 0, Wmax;
            int iCap = 0, iCont = 0, iBot = 0;
            int contStartIndex = 0, contEndIndex = _strLines.Count - 1;
            double leading = pDisplayTransformation.FromPoints(textSym.Leading);
            double morseX, morseY;

                HeaderHorizontalMargin = 0;

                FooterHorizontalMargin = 0;
            List<string> resCont = new List<string>();
            for (int k = contStartIndex; k <= contEndIndex; k++)
            {
                resCont.Add(_strLines[k]);
            }
            iCont = Text_Pars(hDC, transform, resCont.ToArray(), leading, out Wcont, out Hcont, out morseX, out morseY);

            TopMargin = TopMargin + 1;
            BottomMargin = BottomMargin + 1;

            double width = pDisplayTransformation.FromPoints(Width); //prop
            double ShadowMargine = pDisplayTransformation.FromPoints(this.Shadow);

            double _dHeaderMargine = pDisplayTransformation.FromPoints(TopMargin);
            double _dFooterMargine = pDisplayTransformation.FromPoints(BottomMargin);

            double _dHeaderTextMargine = pDisplayTransformation.FromPoints(HeaderHorizontalMargin);
            double _dFooterTextMargine = pDisplayTransformation.FromPoints(FooterHorizontalMargin);

            Wcont = pDisplayTransformation.FromPoints(Wcont);
            Hcont = pDisplayTransformation.FromPoints(Hcont);

            Wcap = pDisplayTransformation.FromPoints(Wcap);
            Hcap = pDisplayTransformation.FromPoints(Hcap);

            Wbot = pDisplayTransformation.FromPoints(Wbot);
            Hbot = pDisplayTransformation.FromPoints(Hbot);


            Wmax = Math.Max(Wcont, Wcap);
            double textAngle = _textSym.Angle + 180.0;
            double Hsum = Hcont + Hcap + Hbot + leading * (iCont + iCap + iBot - 1);

            Wmax *= 1.1;
            BasePoint = PointAlongPlane(_textBoxCenterPt, textAngle - 90.0, 0.5 * (Hsum - Hcap) + _dHeaderMargine);

            IPoint pt1 = PointAlongPlane(BasePoint, textAngle, 0.5 * Wmax + width);
            IPoint pt2 = PointAlongPlane(pt1, textAngle + 90.0, Hsum - 0.5 * (Hcap + Hbot) + _dHeaderMargine + _dFooterMargine);
            IPoint pt3 = PointAlongPlane(pt2, textAngle + 180.0, Wmax + 2 * width);
            IPoint pt4 = PointAlongPlane(BasePoint, textAngle + 180.0, 0.5 * Wmax + width);
            IPointCollection pPoly = new ESRI.ArcGIS.Geometry.Polygon();

            pPoly.AddPoint(pt1);
            pPoly.AddPoint(pt2);
            pPoly.AddPoint(pt3);
            pPoly.AddPoint(pt4);
            pPoly.AddPoint(pt1);

            // ====================== End


            //======================= Left Right Line ===================

            IPointCollection pOutLineL = new Polyline();
            IPointCollection pOutLineR = new Polyline();

            if (this.AccentBarPosition == 0)
            {
                //======================= LeftLine ===================

                if (iCap == 0)
                    _dHeaderTextMargine = 0;
                pt2 = PointAlongPlane(BasePoint, textAngle, 0.5 * Wmax + width); //======= оставить
                pt3 = PointAlongPlane(pt2, textAngle + 90.0, Hsum - 0.5 * (Hcap + Hbot) + _dHeaderMargine + _dFooterMargine);//======= оставить
                pOutLineL.AddPoint(pt2);
                pOutLineL.AddPoint(pt3);
                //======================= End LeftLine ===================

                //pOutLineR = null;
            }

            else
            {
                //======================= RightLine ===================
                pt2 = PointAlongPlane(BasePoint, textAngle + 180.0, 0.5 * Wmax + width);//======= оставить
                pt3 = PointAlongPlane(pt2, textAngle + 90.0, Hsum - 0.5 * (Hcap + Hbot) + _dHeaderMargine + _dFooterMargine);//======= оставить
                pOutLineR.AddPoint(pt2);
                pOutLineR.AddPoint(pt3);
                //======================= End RightLine ===================

                //pOutLineL = null;
            }

            //======================= Leader Line  // сделать if

            double _gap = pDisplayTransformation.FromPoints(LeaderTolerance);

            ITopologicalOperator pTopoOp = (ITopologicalOperator)pPoly;
            IPoint prevAnchorPoint = _anchorPoint;


            ////if (prevAnchorPoint == null) return null;
            //IPointCollection pLiderLine = new Polyline();
            //pLiderLine.AddPoint(prevAnchorPoint);
            //IPoint LiderPt = CalcLeaderPoint(Snap, pPoly, Hcap, Hbot, prevAnchorPoint); //   1 Parametr Snap Position Leader Line (0 - 9)  left =7, right = 9
            //pLiderLine.AddPoint(LiderPt);
            //if (prevAnchorPoint == null) return null;

            IPointCollection pLiderLine = new Polyline();
            IPoint LiderPt = CalcLeaderPoint(Snap, pPoly, Hcap, Hbot, prevAnchorPoint); //   1 Parametr Snap Position Leader Line (0 - 9)  left =7, right = 9

            ILine gapLine = new LineClass();
            gapLine.FromPoint = prevAnchorPoint;
            gapLine.ToPoint = LiderPt;

            if (gapLine.Length < _gap) _gap = gapLine.Length * 0.8;

            double anglGap = gapLine.Angle * 180 / Math.PI;
            IPoint ptgap = PointAlongPlane(prevAnchorPoint, anglGap, _gap);


            pLiderLine.AddPoint(ptgap);
            pLiderLine.AddPoint(LiderPt);




            pTopoOp.Simplify();



            var intersectPntColl = (IPointCollection)pTopoOp.Intersect((IGeometry)pLiderLine, esriGeometryDimension.esriGeometry0Dimension);
            double tmp;
            if (intersectPntColl != null && intersectPntColl.PointCount > 0)
            {
                IPoint CutPoint = pLiderLine.Point[1];
                CutPoint = intersectPntColl.Point[0];
                if (intersectPntColl.PointCount == 2)
                {
                    tmp = Math.Sqrt((intersectPntColl.Point[0].X - prevAnchorPoint.X) * (intersectPntColl.Point[0].X - prevAnchorPoint.X) + (intersectPntColl.Point[0].Y - prevAnchorPoint.Y) * (intersectPntColl.Point[0].Y - prevAnchorPoint.Y));
                    var t = Math.Sqrt((intersectPntColl.Point[1].X - prevAnchorPoint.X) * (intersectPntColl.Point[1].X - prevAnchorPoint.X) + (intersectPntColl.Point[1].Y - prevAnchorPoint.Y) * (intersectPntColl.Point[1].Y - prevAnchorPoint.Y));
                    if (tmp > t)
                    {
                        CutPoint = intersectPntColl.Point[1];
                    }
                }
                pLiderLine.ReplacePoints(1, 1, 1, ref CutPoint);
            }

            LiderPt = pLiderLine.Point[1];
            IRelationalOperator relOp = (IRelationalOperator)prevAnchorPoint;
            DrawLeader = DrawLeader && !relOp.Within((IPolygon)pPoly);

            //======================= End Leader Line

            IGeometryCollection GeoBag = new GeometryBagClass();
            GeoBag.AddGeometry((IPolygon)pPoly);
            GeoBag.AddGeometry((IPolyline)pOutLineL);
            GeoBag.AddGeometry((IPolyline)pOutLineR);


            GeoBag.AddGeometry((IGeometry)pLiderLine);
            return (IGeometry)GeoBag;
        }


        private void CreateMorseText ( string text, int hDC, ITransformation transform, out double morseWidth, out double morseHeight )
		{
			// example 
			// <FNT name="Morse" size ="8"><CLR red="0" green="0" blue="0">ABC</CLR></FNT></
			MorseFullText = @"<FNT name=""Morse""";
			int index = text.IndexOf ( "size" );
			string tmp;
			if ( index > -1 )
			{
				tmp = "";
				for ( int i = index + "size = ".Length+1; i < text.Length; i++ )
				{
					if ( Char.IsDigit ( text[ i ] ) )
						tmp += text[ i ];
					else
						break;
				}
				MorseFullText += @" size=""" + tmp + @""">";
			}
			MorseFullText += "<CLR ";
			index = text.IndexOf ( "red" );
			if ( index > -1 )
			{
				tmp = "";
				for ( int i = index + "red = ".Length + 1; i < text.Length; i++ )
				{
					if ( Char.IsDigit ( text[ i ] ) )
						tmp += text[ i ];
					else
						break;
				}
				MorseFullText += @"red=""" + tmp + @""" ";
			}
			index = text.IndexOf ( "green" );
			if ( index > -1 )
			{
				tmp = "";
				for ( int i = index + "green = ".Length + 1; i < text.Length; i++ )
				{
					if ( Char.IsDigit ( text[ i ] ) )
						tmp += text[ i ];
					else
						break;
				}
				MorseFullText += @"green=""" + tmp + @""" ";
			}
			index = text.IndexOf ( "blue" );
			if ( index > -1 )
			{
				tmp = "";
				for ( int i = index + "blue = ".Length + 1; i < text.Length; i++ )
				{
					if ( Char.IsDigit ( text[ i ] ) )
						tmp += text[ i ];
					else
						break;
				}
				MorseFullText += @"blue=""" + tmp + @""" ";
			}
			MorseFullText += ">";
			index = text.IndexOf ( "value" );
			if ( index > -1 )
			{
				tmp = "";
				_morseTxt = "";
				for ( int i = index + "value=".Length + 1; i < text.Length; i++ )
				{
					if ( Char.IsLetter ( text[ i ] ) )
					{
						_morseTxt += text[ i ];
						tmp += MorseFullText + text[ i ] + "</CLR></FNT>\r\n";
					}
					else
						break;
				}
				MorseFullText = tmp;
			}

			TextSymbolClass textSym = new TextSymbolClass ( );
			textSym.Text = MorseFullText;
			ITextParserSupport pTextParserSupport = textSym;
			ITextParser pTextParser = pTextParserSupport.TextParser;
			pTextParser.Text = textSym.Text;
			pTextParser.TextSymbol = textSym;

			pTextParser.Reset ( );
			pTextParser.TextSymbol.Text = "TEST";
			pTextParser.Next ( );

			double w, y;
			morseWidth = 0;
			morseHeight = 0;
			int j = 0;
			while ( j < _morseTxt.Length )
			{
				if ( pTextParser.TextSymbol.Text != "\r\n" )
				{
					pTextParser.TextSymbol.GetTextSize ( hDC, transform, pTextParser.TextSymbol.Text, out w, out y );
					morseHeight += y;
					if ( w > morseWidth )
						morseWidth = w;
					j++;
				}
				pTextParser.Next ( );
			}

		}

		public int Text_Pars ( int hdc, ITransformation transform, string[] strLines, double leading, out double Wmax, out double Hsum, out double morseX, out double morseY )
		{
			TextSymbolClass textSym = new TextSymbolClass ( );
            //textSym.Size = 8; 
			double W0 = 0, Hi, H0, Wi = 0;
			int n, i;
			Wmax = 0;
			Hsum = 0;
			morseX = double.NaN;
			morseY = double.NaN;
			n = strLines.Length;

			string sKey = "QWERTYUIOPASDFGHJKLZXCVBNM";
			string Prob = " ";
			int j;
			for ( i = 0; i < n; i++ )
			{
				textSym.Text = strLines[ i ];
				ITextParserSupport pTextParserSupport = textSym;
				ITextParser pTextParser = pTextParserSupport.TextParser;
				pTextParser.Text = textSym.Text;
				pTextParser.TextSymbol = textSym;

				bool bHasTags = false;
				pTextParser.HasTags ( ref bHasTags );
				j = 0;
				if ( bHasTags )
				{
					Wi = 0;
					Hi = 0;

					pTextParser.Reset ( );
					pTextParser.TextSymbol.Text = sKey;
					pTextParser.Next ( );

					while ( pTextParser.TextSymbol.Text != sKey )
					{
                        if (pTextParser.TextSymbol.Text == Prob || pTextParser.TextSymbol.Text == "\r\n " || pTextParser.TextSymbol.Text == "\r\n")
						{
							pTextParser.TextSymbol.Text = sKey;
							pTextParser.Next ( );
						}
						else
						{
							pTextParser.TextSymbol.GetTextSize ( hdc, transform, pTextParser.TextSymbol.Text, out W0, out H0 );
							if ( pTextParser.TextSymbol.Text.Trim() == _morseTxt )
							{
								morseX = W0;
								morseY = H0;
							}
							pTextParser.TextSymbol.Text = sKey;
							Wi += W0;
							Hi = Math.Max ( Hi, H0 );
							if ( j > 0 )
							{
								pTextParser.TextSymbol.GetTextSize ( hdc, transform, " ", out W0, out H0 );
								Wi += W0;
							}
							j++;
							pTextParser.Next ( );
						}
					}
				}
				else
					textSym.GetTextSize ( hdc, transform, strLines[ i ], out Wi, out Hi );
				if ( Wi > Wmax )
					Wmax = Wi;
				Hsum = Hsum + Hi;

			}
			return i;
		}

		private IPoint CreatePoint ( double x, double y )
		{
			IPoint result = new PointClass ( );
			result.PutCoords ( x, y );
			return result;
		}

		private List<string> SplitByEnter ( string text )
		{
			string curRow = "";
			List<string> result = new List<string> ( );
			for ( int i = 0; i < text.Length; i++ )
			{
				if ( text[ i ] == ( ( Char ) 13 ) )
				{
					result.Add ( curRow );
					curRow = "";
				}

				if ( text[ i ] != ( ( Char ) 10 ) )
				{
					curRow += text[ i ];
				}
			}
			result.Add ( curRow );
			return result;
		}

		public IPoint PointAlongPlane ( IPoint ptGeo, double dirAngle, double Dist )
		{
			dirAngle = DegToRad ( dirAngle );
			IClone pClone = ( IClone ) ptGeo;
			IPoint result = ( IPoint ) pClone.Clone ( );
			result.PutCoords ( ptGeo.X + Dist * System.Math.Cos ( dirAngle ), ptGeo.Y + Dist * System.Math.Sin ( dirAngle ) );
			return result;
		}

		public IPoint CalcLeaderPoint ( int LeadPos, IPointCollection pPoly, double yTop, double yBot, IPoint _anchorPoint )
		{
			IPoint pt1 = pPoly.Point[ 0 ];
			IPoint pt2 = pPoly.Point[ 1 ];
			IPoint pt3 = pPoly.Point[ 2 ];
			IPoint pt4 = pPoly.Point[ 3 ];

			IEnvelope pEnv = new EnvelopeClass ( );

			var dl = new List<IPoint> ( );
			dl.Add ( pt1 );
			dl.Add ( pt2 );
			dl.Add ( pt3 );
			dl.Add ( pt4 );

			var xMax = dl.Max ( pt => pt.X );
			var yMax = dl.Max ( pt => pt.Y );

			var xMin = dl.Min ( pt => pt.X );
			var yMin = dl.Min ( pt => pt.Y );

			IPoint LiderPt = ( CreatePoint ( ( xMin + xMax ) / 2, ( yMin + yMax ) / 2 ) );


			IPointCollection ptColl = new Polyline ( );

			switch ( LeadPos )
			{
				case 1:
					return pt1;

				case 4:
					return CreatePoint ( ( pt1.X + pt2.X ) / 2, ( pt1.Y + pt2.Y ) / 2 );

				case 7:
					return pt2;

				case 3:
					return pt4;

				case 6:
					return CreatePoint ( ( pt3.X + pt4.X ) / 2, ( pt3.Y + pt4.Y ) / 2 );

				case 9:
					return pt3;

				case 2:
					return CreatePoint ( ( pt1.X + pt4.X ) / 2, ( pt1.Y + pt4.Y ) / 2 + yTop / 2 );

				case 8:
					if ( HasFooter )
						return CreatePoint ( ( pt2.X + pt3.X ) / 2, ( pt2.Y + pt3.Y ) / 2 - yBot / 2 );
					else
						return CreatePoint ( ( pt2.X + pt3.X ) / 2, ( pt2.Y + pt3.Y ) / 2 );

				case 5:
					IPointCollection pLidLine = new Polyline ( );

					IPointCollection ptTmp;
					IPolyline pLine = new PolylineClass ( );

					pLine.FromPoint = _anchorPoint;
					pLine.ToPoint = LiderPt;

					ITopologicalOperator pTopoOp;
					pTopoOp = ( ITopologicalOperator ) pPoly;

					ptTmp = ( IPointCollection ) pTopoOp.Intersect ( ( IGeometry ) pLine, esriGeometryDimension.esriGeometry0Dimension );

					if ( ptTmp.PointCount > 0 )
						return ptTmp.Point[ 0 ];
					break;
				case 0:

					IProximityOperator pProxy = ( IProximityOperator ) pPoly;
					IPoint pt = pProxy.ReturnNearestPoint ( _anchorPoint, esriSegmentExtension.esriNoExtension );
					return pt;
			}
			return LiderPt;
		}

		public double DegToRad ( double deg )
		{
			return deg * Math.PI / 180.0;
		}

		public double RadToDeg ( double rad )
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

		public double HeaderHorizontalMargin
		{
			get;
			set;
		}

		public double TopMargin
		{
			get;
			set;
		}

		public double FooterHorizontalMargin
		{
			get;
			set;
		}

		public double BottomMargin
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

		public int Shadow
		{
			get;
			set;
		}

		public bool HasHeader
		{
			get;
			set;
		}

		public bool HasFooter
		{
			get;
			set;
		}

		public string CaptionText
		{
			get
			{
				return _captionText;
			}

			set
			{
				_captionText = value;
				HasHeader = !string.IsNullOrEmpty ( _captionText );
			}
		}

		public string FooterText
		{
			get
			{
				return _footerText;
			}
			set
			{
				_footerText = value;
				HasFooter = !string.IsNullOrEmpty ( _footerText );
			}
		}

		public IColor ShadowBackColor
		{
			get;
			set;
		}

		public IColor AirspaceBackColor
		{
			get;
			set;
		}

		public string AirspaceSymbols
		{
			get;
			set;
		}
        
        public string AirsapceFontName
        {
            get;
            set;
        }
        public double AirspaceFontSize { get; set; }
        public int AirspaceFontColorRed { get; set; }
        public int AirspaceFontColorBlue { get; set; }
        public int AirspaceFontColorGreen { get; set; }
        public bool AirspaceFontItalic { get; set; }
        public bool AirspaceFontUnderLine { get; set; }
        public bool AirspaceFontBold { get; set; }
        public int AirspaceSignSize { get; set; }
        public bool AirspaceSignScaleToFit { get; set; }
        public bool verticalMorse { get; set; }

        public bool AirspaceOnLeftSide
		{
			get;
			set;
		}

		public double MorseLocationShiftOnX
		{
			get;
			set;
		}

		public double MorseLocationShiftOnY
		{
			get;
			set;
		}

		public double MorseLeading
		{
			get;
			set;
		}

		public string MorseFullText
		{
			get;
			set;
		}

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

        public bool DrawLeader
		{
			get;
			set;
        }

        public int AccentBarPosition { get; set; }

        #endregion

        public SigmaCallout()
        {
            prevAnchorPoints = new List<IPoint>();
            _fillSymbol = new SimpleFillSymbolClass(); // property
            LineSymbol = new SimpleLineSymbol(); // property
            
        }


    }

}