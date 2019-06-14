using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aran.PANDA.Common;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Geometry;

namespace ChartTypeA.Models
{
	class GridCreater:AbstractGridCreater
	{
		private IGroupElement _leftGridGrp, _textGrp, _profileGrp, _allElementGrp, _rightGridGrp;
		private IActiveView _dataView, _pageLayoutView;
		private IGraphicsContainer _pageLayGraphics;
		private double _cellWidth, _cellHeight;
	
		private int fontSize = 6;
		private IPoint _pnt1Lower, _pnt2Lower;
		private double _verticalStepInM;
        private double _tickLengthInScale;
        private LineElementCreater _lineElementCreater;
        private TextCreater _textCreater;

        public GridCreater ()
		{
			CreateElements();

            AssignLocalVariables();
		    _lineElementCreater =new LineElementCreater(Color);
            _textCreater = new TextCreater(fontSize);

		}

	    public void CreateElements()
	    {
	        _textGrp = (IGroupElement)new GroupElementClass();
	        _profileGrp = (IGroupElement)new GroupElementClass();

            _allElementGrp = (IGroupElement)new GroupElementClass();
	        ((IElementProperties3) _allElementGrp).Name = "ProfileElem";
        }

	    public override void ReCreate()
        {
            try
            {
                Clear();

                int x, y;

                _verticalStepInM = Common.DeConvertHeight(VerticalStep);

                HorScale = GlobalParams.Map.MapScale;
                VerScale = HorScale / 10;

                LengthRwy = Math.Round(LengthRwy);
                _cellWidth = (HorizontalStep * 100) / HorScale;
                _cellHeight = (_verticalStepInM * 100) / VerScale;
                _tickLengthInScale = (TickLength * 100) / HorScale;

                IPoint pnt1PgLayout, pnt2PgLayout;
                if (Pnt1 != null)
                {
                    _dataView.ScreenDisplay.DisplayTransformation.FromMapPoint(Pnt1, out x, out y);
                    pnt1PgLayout = _pageLayoutView.ScreenDisplay.DisplayTransformation.ToMapPoint(x, y);
                    pnt1PgLayout.Y = pnt1PgLayout.Y + FrameHeight;
                    //CreateCells(pnt1PgLayout, ARANMath.C_PI, RowCount1, ColumnCount1, _cellWidth, _cellHeight, false);
                    CreateCells(pnt1PgLayout,  RowCount1, ColumnCount1, _cellWidth, _cellHeight, true);
                }
                else
                {
                    var rwyCntPnt = CenterlinePoints.FirstOrDefault(pdm => pdm.Role == PDM.CodeRunwayCenterLinePointRoleType.END);
                    if (rwyCntPnt == null)
                        return;
                    rwyCntPnt.RebuildGeo();
                    IPoint prjPnt = (IPoint)GlobalParams.SpatialRefOperation.ToEsriPrj(rwyCntPnt.Geo);
                    _dataView.ScreenDisplay.DisplayTransformation.FromMapPoint(prjPnt, out x, out y);
                    pnt1PgLayout = _pageLayoutView.ScreenDisplay.DisplayTransformation.ToMapPoint(x, y);
                    pnt1PgLayout.Y = pnt1PgLayout.Y + FrameHeight;
                }


                if (Pnt2 != null)
                {
                    _dataView.ScreenDisplay.DisplayTransformation.FromMapPoint(Pnt2, out x, out y);
                    pnt2PgLayout = _pageLayoutView.ScreenDisplay.DisplayTransformation.ToMapPoint(x, y);
                    pnt2PgLayout.Y = pnt2PgLayout.Y + FrameHeight;
                    CreateCells(pnt2PgLayout, RowCount2, ColumnCount2, _cellWidth, _cellHeight, false);
                }
                else
                {
                    //we will take end because centerline always is same and direction of centerline2
                    var rwyCntPnt = CenterlinePoints.FirstOrDefault(pdm => pdm.Role == PDM.CodeRunwayCenterLinePointRoleType.START || pdm.Role == PDM.CodeRunwayCenterLinePointRoleType.THR);
                    if (rwyCntPnt == null)
                        return;
                   // rwyCntPnt.RebuildGeo();
                    IPoint prjPnt = (IPoint)GlobalParams.SpatialRefOperation.ToEsriPrj(rwyCntPnt.Geo);
                    _dataView.ScreenDisplay.DisplayTransformation.FromMapPoint(prjPnt, out x, out y);
                    pnt2PgLayout = _pageLayoutView.ScreenDisplay.DisplayTransformation.ToMapPoint(x, y);
                    pnt2PgLayout.Y = pnt2PgLayout.Y + FrameHeight;
                }

                _allElementGrp.AddElement(_lineElementCreater.CreateLineElement(pnt1PgLayout, pnt2PgLayout));
                CreateProfile();
                pnt1PgLayout.Y = pnt1PgLayout.Y - FrameHeight;

                if (_leftGridGrp != null)
                    _allElementGrp.AddElement((IElement)_leftGridGrp);
                if (_rightGridGrp != null)
                    _allElementGrp.AddElement((IElement)_rightGridGrp);
                if (_textGrp.ElementCount > 0)
                    _allElementGrp.AddElement((IElement)_textGrp);
                if (_profileGrp.ElementCount > 0)
                    _allElementGrp.AddElement((IElement)_profileGrp);
                if (ObstacleElements != null && ObstacleElements.ElementCount > 0)
                    _allElementGrp.AddElement((IElement)ObstacleElements);
                _pageLayGraphics.AddElement((GroupElement)_allElementGrp, 0);
             //   _pageLayoutView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);

                GlobalParams.GrCreater.AllElements = _allElementGrp;
                var env = (_allElementGrp as IElement).Geometry.Envelope;
                YMin = env.YMin;
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public override void Clear()
        {
            if (_allElementGrp != null)
            {
                _pageLayGraphics.DeleteElement(_allElementGrp as IElement);
                _textGrp.ClearElements();
                _profileGrp.ClearElements();
                _allElementGrp.ClearElements();
            }

            if (ObstacleElements != null)
                _pageLayGraphics.DeleteElement(ObstacleElements as IElement);

            _pageLayoutView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);
        }

        public void AssignLocalVariables()
        {
            if (GlobalParams.HookHelper != null)
            {
                var focusMap = GlobalParams.HookHelper.FocusMap;
                _dataView = (IActiveView)focusMap;
                var pageLayout = GlobalParams.HookHelper.PageLayout;
                _pageLayoutView = (IActiveView)pageLayout;
                _pageLayGraphics = (IGraphicsContainer)pageLayout;
               // _oldScale = GlobalParams.Map.MapScale;
            }

        }

		private void CreateProfile ( )
		{
			int x, y;
			var list = new List<IPoint> ( );
			double? elev;
			double differ;
			string text;
			List<PDM.RunwayCenterLinePoint> centerPntList = CenterlinePoints;

            foreach ( var rwyCentPnt in centerPntList )
			{
				IPoint pnt = ( IPoint ) GlobalParams.SpatialRefOperation.ToEsriPrj ( rwyCentPnt.Geo );
				_dataView.ScreenDisplay.DisplayTransformation.FromMapPoint ( pnt, out x, out y );
				IPoint pntPgLay = _pageLayoutView.ScreenDisplay.DisplayTransformation.ToMapPoint ( x, y );
				pntPgLay.Y = pntPgLay.Y + FrameHeight; 
				if ( rwyCentPnt.Elev.HasValue )
				{
					if ( InitChartTypeA.HeightConverter.Unit== "M" )
						elev = rwyCentPnt.ConvertValueToMeter ( rwyCentPnt.Elev.Value, rwyCentPnt.Elev_UOM.ToString ( ) );
					else
					{
						if ( rwyCentPnt.Elev_UOM == PDM.UOM_DIST_VERT.FT )
							elev = rwyCentPnt.Elev;
						else
							elev = rwyCentPnt.Elev * 3.28084;
					}						
					if ( elev.HasValue )
					{
						differ = elev.Value - BaseElevation;
						IPoint tmp = EsriFunctions.LocalToPrj ( pntPgLay, ARANMath.C_PI_2, ( _cellHeight * differ ) / VerticalStep );
						var docTextElement = new TextElementClass ( );
						var docElement = docTextElement as IElement;

						TextSymbolClass pTextSymbol = new TextSymbolClass ( );
						pTextSymbol.Angle = 90;
					    pTextSymbol.Size = fontSize;
						docTextElement.Symbol = pTextSymbol;
						if ( InitChartTypeA.DistanceConverter.Unit == "M" )
						{
							differ = Math.Round ( differ, 1 );
							differ = Math.Round ( differ * 2, MidpointRounding.AwayFromZero ) / 2;
						}
						else
							differ = Math.Round ( differ );
						text = differ.ToString ( );
						docTextElement.Text = text;
						IPoint txtPnt = new PointClass ( );
						if ( rwyCentPnt.Role == PDM.CodeRunwayCenterLinePointRoleType.START || rwyCentPnt.Role == PDM.CodeRunwayCenterLinePointRoleType.THR )
						{
							if ( Pnt1 == null )
								txtPnt.PutCoords ( tmp.X + 0.4, tmp.Y + 0.2 + 0.1 * ( text.Length - 2 ) );
							else
								txtPnt.PutCoords ( tmp.X - 0.4, tmp.Y + 0.2 + 0.1 * ( text.Length - 2 ) );
						}
						else if ( rwyCentPnt.Role == PDM.CodeRunwayCenterLinePointRoleType.END )
						{
							if ( Pnt1 == null )
								txtPnt.PutCoords ( tmp.X - 0.2, tmp.Y + 0.2 + 0.1 * ( text.Length - 2 ) );
							else
								txtPnt.PutCoords ( tmp.X + 0.2, tmp.Y + 0.2 + 0.1 * ( text.Length - 2 ) );
						}
						else
						{
							txtPnt.PutCoords ( tmp.X, tmp.Y + 0.2 + 0.1 * ( text.Length - 2 ) );
						}						
						docElement.Geometry = txtPnt;
						_textGrp.AddElement ( docElement );

						list.Add ( tmp );
					}
				}
			}
			_profileGrp.AddElement ( GlobalParams.UI.GetPolylineElement ( list, Color ) );
			
			List<IPoint> pntList = new List<IPoint> ( );
			double lengthReminder, linePrjWidth, height;
			if ( Pnt2 != null )
			{
				pntList.Add ( list[ 0 ] );
				IPoint endPnt2 = EsriFunctions.LocalToPrj ( pntList[ 0 ], ARANMath.C_PI, ( ClearWay2 * 100 ) / HorScale );
				pntList.Add ( endPnt2 );
				_profileGrp.AddElement ( GlobalParams.UI.GetPolylineElement ( pntList, Color, esriSimpleLineStyle.esriSLSDot ) );

				pntList.Clear ( );
				pntList.Add ( endPnt2 );
				lengthReminder = LengthRwy % 100;
                linePrjWidth = ColumnCount2 * HorizontalStep - lengthReminder - ClearWay2;
				height = Slope * linePrjWidth;
				_pnt2Lower.Y = endPnt2.Y + ( height / VerScale );
				pntList.Add ( _pnt2Lower );
				_profileGrp.AddElement ( GlobalParams.UI.GetPolylineElement ( pntList, Color, esriSimpleLineStyle.esriSLSDashDot ) );

				var docTextElement = new TextElementClass ( );
				var docElement = docTextElement as IElement;
				TextSymbolClass pTextSymbol = new TextSymbolClass ( );				
				pTextSymbol.Angle = -7;
				pTextSymbol.Size = fontSize;
				docTextElement.Symbol = pTextSymbol;
				docTextElement.Text = Slope + "% SLOPE";
				IPoint txtPnt = new PointClass ( ); //EsriFunctions.LocalToPrj ( endPnt2, Math.Atan ( Slope * 0.01 ), linePrjWidth * 0.5 );
				txtPnt.PutCoords ( ( endPnt2.X + _pnt2Lower.X ) * 0.5, ( endPnt2.Y + _pnt2Lower.Y ) * 0.5 + 0.1);
				docElement.Geometry = txtPnt;   
				_profileGrp.AddElement ( docElement );
			}

			if ( Pnt1 != null )
			{
				pntList.Clear ( );
				pntList.Add ( list[list.Count-1] );
                IPoint endPnt1 = EsriFunctions.LocalToPrj(pntList[0], 0, (ClearWay1 * 100) / HorScale);
				pntList.Add ( endPnt1 );
				_profileGrp.AddElement ( GlobalParams.UI.GetPolylineElement ( pntList, Color, esriSimpleLineStyle.esriSLSDot ) );

				pntList.Clear ( );
				pntList.Add ( endPnt1 );
				lengthReminder = LengthRwy % 100;
                linePrjWidth = ColumnCount1 * HorizontalStep - lengthReminder - ClearWay1;
				height = Slope * linePrjWidth;
				_pnt1Lower.Y = endPnt1.Y + ( height / VerScale );
				pntList.Add ( _pnt1Lower );
				_profileGrp.AddElement ( GlobalParams.UI.GetPolylineElement ( pntList, Color, esriSimpleLineStyle.esriSLSDashDot ) );

				var docTextElement = new TextElementClass ( );
				var docElement = docTextElement as IElement;
				TextSymbolClass pTextSymbol = new TextSymbolClass ( );
				pTextSymbol.Angle = 7;
				pTextSymbol.Size = fontSize;
				docTextElement.Symbol = pTextSymbol;
				docTextElement.Text = Slope + "% SLOPE";
				IPoint txtPnt = new PointClass ( ); //EsriFunctions.LocalToPrj ( endPnt2, Math.Atan ( Slope * 0.01 ), linePrjWidth * 0.5 );
				txtPnt.PutCoords ( ( endPnt1.X + _pnt1Lower.X ) * 0.5, ( endPnt1.Y + _pnt1Lower.Y ) * 0.5 + 0.1 );
				docElement.Geometry = txtPnt;
				_profileGrp.AddElement ( docElement );
			}
		}

		private void CreateCells ( IPoint pnt, int rowCount, int columnCount, double cellWidth, double cellHeight, bool toRight )
		{
			double columnHeight = rowCount * cellHeight;
			int side = toRight ? 1 : -1;
            var dir = toRight ? 0 : ARANMath.C_PI;
            double width;
			IPoint tmp;
			double tickWidth = cellWidth / TickCount;
			IPoint pnt_lower, pnt_Upper;
			double lengthReminder = LengthRwy % 100;
			string text;
			double distHor = ( cellWidth * ( HorizontalStep - lengthReminder ) ) / HorizontalStep;
			double rowWidth = distHor + ( columnCount - 1 ) * cellWidth;
			int localTickCount;
			double tickReminder;
			List<IPoint> pntList = new List<IPoint> ( );
			IGroupElement grpElement = ( IGroupElement ) new GroupElementClass ( );	
			IPoint pntEdge = null;
            
			for ( int i = 0; i < columnCount + 1; i++ )
			{
				pntList.Clear ( );
				if ( i == 0 )
				{
					text = LengthRwy.ToString ( );
					tickReminder = tickWidth - ( lengthReminder * tickWidth ) / ( HorizontalStep / TickCount ) % tickWidth;
					width = 0;
					localTickCount = ( int ) ( ( distHor * TickCount ) / cellWidth );
					pnt_lower = pnt;
				}
				else
				{
					tickReminder = 0;
					text = ( LengthRwy - lengthReminder + i * HorizontalStep ).ToString ( );
					pnt_lower = EsriFunctions.LocalToPrj ( pnt, dir, distHor + ( i - 1 ) * cellWidth );
					width = distHor + ( i - 1 ) * cellWidth;
					localTickCount = TickCount;
				}

				pnt_Upper = EsriFunctions.LocalToPrj ( pnt_lower, dir + side * ARANMath.C_PI_2, columnHeight );
				pntList.Add ( pnt_lower );
				pntList.Add ( pnt_Upper );
				grpElement.AddElement (_lineElementCreater.CreateLineElement( pnt_lower, pnt_Upper ) );
                //continue;
                grpElement.AddElement(_textCreater.CreateHorizontalText(text, pnt_lower, side));

				if ( i == columnCount )
				{
					pntEdge = pnt_lower;
					break;
				}
				for ( int j = 0; j < localTickCount; j++ )
				{
					pnt_lower = EsriFunctions.LocalToPrj ( pnt, dir, width + tickReminder + j * tickWidth );
					pnt_Upper = EsriFunctions.LocalToPrj ( pnt_lower, dir + side * ARANMath.C_PI_2, _tickLengthInScale );
					grpElement.AddElement (_lineElementCreater.CreateLineElement( pnt_lower, pnt_Upper));
				}
			}
            
			double height;
			tickWidth = cellHeight / TickCount;
			IPoint pnt1, pnt2;
			for ( int i = 0; i < rowCount + 1; i++ )
			{
				pnt1 = EsriFunctions.LocalToPrj ( pnt, dir + side * ARANMath.C_PI_2, i * cellHeight );
				pnt2 = EsriFunctions.LocalToPrj ( pnt1, dir, rowWidth );
				grpElement.AddElement (_lineElementCreater.CreateLineElement(pnt1, pnt2 ) );

				grpElement.AddElement(_textCreater.CreateVerticalText(( BaseElevation + i * VerticalStep ).ToString ( ),
                    pnt2,  side ));
				if ( i == rowCount )
					continue;
				height = i * cellHeight;
				for ( int j = 1; j < TickCount; j++ )
				{
					pnt1 = EsriFunctions.LocalToPrj ( pnt, dir + side * ARANMath.C_PI_2, height + j * tickWidth );
					pnt2 = EsriFunctions.LocalToPrj ( pnt1, dir, _tickLengthInScale );
					grpElement.AddElement (_lineElementCreater.CreateLineElement(pnt1, pnt2 ) );

					tmp = EsriFunctions.LocalToPrj ( pnt, dir, rowWidth );
					pnt1 = EsriFunctions.LocalToPrj ( tmp, dir + side * ARANMath.C_PI_2, height + j * tickWidth );
					pnt2 = EsriFunctions.LocalToPrj ( pnt1, dir, -_tickLengthInScale);
					grpElement.AddElement (_lineElementCreater.CreateLineElement(pnt1, pnt2 ) );
				}
			}
			if ( toRight )
			{
                _pnt1Lower = pntEdge;
                _leftGridGrp = grpElement;
			}
			else
			{
                _pnt2Lower = pntEdge;
                _rightGridGrp = grpElement;
            }
		}

    }
}