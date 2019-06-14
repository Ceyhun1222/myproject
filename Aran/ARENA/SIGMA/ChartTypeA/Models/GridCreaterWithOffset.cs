using Aran.PANDA.Common;
using ChartTypeA.ViewModels;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ChartTypeA.Models
{
    class GridCreaterWithOffset : AbstractGridCreater
    {
       private IGroupElement _leftGridGrp, _textGrp, _profileGrp, _allElementGrp, _rightGridGrp;
        private IActiveView _dataView, _pageLayoutView;
        private IGraphicsContainer _pageLayGraphics;
        private double _cellWidth, _cellHeight;
        private int fontSize = 6;
        private IPoint _pntLower;
        private double _verticalStepInM;
        private double _oldScale;
        private double _tickLengthInScale;
        private LineElementCreater _lineElementCreater;
        private TextCreater _textCreater;

        public GridCreaterWithOffset()
        {
            CreateElements();

            AssignLocalVariables();
            _lineElementCreater = new LineElementCreater(Color);
            _textCreater = new TextCreater(fontSize);
        }

        public void CreateElements()
        {
            _textGrp = (IGroupElement)new GroupElementClass();
            _profileGrp = (IGroupElement)new GroupElementClass();

            //_scaleGrp = (IGroupElement)new GroupElementClass();
            //(_scaleGrp as IElementProperties).Name = "ScaleElem";

            _allElementGrp = (IGroupElement)new GroupElementClass();
            (_allElementGrp as IElementProperties).Name = "ProfileElem";
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
                _oldScale = GlobalParams.Map.MapScale;
            }
        }

        public override void ReCreate()
        {
            try
            {
                if (Pnt2 != null)
                {
                    Pnt1 = Pnt2;
                    RowCount1 = RowCount2;
                    ColumnCount1 = ColumnCount2;
                    ClearWay1 = ClearWay2;
                }


                _verticalStepInM = Common.DeConvertHeight(VerticalStep);

                HorScale = GlobalParams.Map.MapScale;
                VerScale = HorScale / 10;

                LengthRwy = Math.Round(LengthRwy);
                _cellWidth = (HorizontalStep * 100) / HorScale;
                _cellHeight = (_verticalStepInM * 100) / VerScale;
                _tickLengthInScale = (TickLength * 100) / HorScale;
                //_tickLengthInM = ( _tickLengthInM * 100 ) / HorScale;

                IPoint layoutPt1, layoutPt2;
                CreateProfile(out layoutPt1, out layoutPt2);

                double length1 = ColumnCount1 * _cellWidth;
                double length2 = ColumnCount2 * _cellWidth;

                //Create base runwayLine
                _allElementGrp.AddElement(_lineElementCreater.CreateLineElement(layoutPt1, layoutPt2));

                //IPoint verPt = new PointClass();
                //verPt.X = 10;
                //verPt.Y = layoutPt1.Y - FrameHeight + 5;

                //if (createScale)
                //    CreateScale(verPt);

                if (_leftGridGrp != null)
                    _allElementGrp.AddElement((IElement)_leftGridGrp);
                if (_rightGridGrp != null)
                    _allElementGrp.AddElement((IElement)_rightGridGrp);
                if (_textGrp.ElementCount > 0)
                    _allElementGrp.AddElement((IElement)_textGrp);
                if (_profileGrp.ElementCount > 0)
                    _allElementGrp.AddElement((IElement)_profileGrp);
                //if (_scaleGrp.ElementCount > 0)
                //    _pageLayGraphics.AddElement((GroupElement)_scaleGrp, 0);
                if (ObstacleElements != null && ObstacleElements.ElementCount > 0)
                    _allElementGrp.AddElement((IElement)ObstacleElements);
                _pageLayGraphics.AddElement((GroupElement)_allElementGrp, 0);
                _pageLayoutView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);

                GlobalParams.GrCreater.AllElements = _allElementGrp;
                var env = (_allElementGrp as IElement).Geometry.Envelope;
                YMin = env.YMin;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
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
                //if (deleteScale)
                //    _scaleGrp.ClearElements();
            }

            if (ObstacleElements != null)
                _pageLayGraphics.DeleteElement(ObstacleElements as IElement);

            _pageLayoutView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);
        }

        private void CreateProfile(out IPoint layoutPtStart, out IPoint layoutPtEnd)
        {
            int x, y;
            var list = new List<IPoint>();
            double? elev;
            double differ;
            string text;
            List<PDM.RunwayCenterLinePoint> centerPntList = CenterlinePoints;

            var thr = centerPntList.FirstOrDefault(center => center.Role == PDM.CodeRunwayCenterLinePointRoleType.THR || center.Role == PDM.CodeRunwayCenterLinePointRoleType.START);
            var end = centerPntList.FirstOrDefault(center => center.Role == PDM.CodeRunwayCenterLinePointRoleType.END);

            layoutPtStart = null;
            layoutPtEnd = null;
            if (thr == null || end == null)
                return;

            thr.RebuildGeo();
            var thrPrj = (IPoint)GlobalParams.SpatialRefOperation.ToEsriPrj(thr.Geo);
            _dataView.ScreenDisplay.DisplayTransformation.FromMapPoint(thrPrj, out x, out y);
            IPoint pntThrPgLay = _pageLayoutView.ScreenDisplay.DisplayTransformation.ToMapPoint(x, y);
            var thrElevation = thr.ConvertValueToMeter(thr.Elev.Value, thr.Elev_UOM.ToString());

            end.RebuildGeo();
            var endPrj = (IPoint)GlobalParams.SpatialRefOperation.ToEsriPrj(end.Geo);
            _dataView.ScreenDisplay.DisplayTransformation.FromMapPoint(endPrj, out x, out y);
            IPoint pntEndPgLay = _pageLayoutView.ScreenDisplay.DisplayTransformation.ToMapPoint(x, y);

            double distanceFromThrToEnd = EsriFunctions.ReturnDistanceAsMetr(thrPrj, endPrj);
            double layoutDistance = pntEndPgLay.X - pntThrPgLay.X;

            pntThrPgLay.Y += FrameHeight+ (thrElevation-BaseElevation)/VerticalStep;
            foreach (var rwyCentPnt in centerPntList)
            {
                if (rwyCentPnt.Geo == null)
                    rwyCentPnt.RebuildGeo();


                IPoint pntPrj = (IPoint)GlobalParams.SpatialRefOperation.ToEsriPrj(rwyCentPnt.Geo);

                var distFromThr = EsriFunctions.ReturnDistanceAsMetr(thrPrj, pntPrj);


                IPoint pntPgLay = new ESRI.ArcGIS.Geometry.Point();
                pntPgLay.X = pntThrPgLay.X + (distFromThr * layoutDistance) / distanceFromThrToEnd;

                if (rwyCentPnt.Elev.HasValue)
                {
                    if (InitChartTypeA.HeightConverter.Unit == "M")
                        elev = rwyCentPnt.ConvertValueToMeter(rwyCentPnt.Elev.Value, rwyCentPnt.Elev_UOM.ToString());
                    else
                    {
                        if (rwyCentPnt.Elev_UOM == PDM.UOM_DIST_VERT.FT)
                            elev = rwyCentPnt.Elev;
                        else
                            elev = rwyCentPnt.Elev * 3.28084;
                    }
                    if (elev.HasValue)
                    {
                        
                        differ = elev.Value - thrElevation;
                        pntPgLay.Y = pntThrPgLay.Y+ differ / VerticalStep;
                        var docTextElement = new TextElementClass();
                        var docElement = docTextElement as IElement;

                        TextSymbolClass pTextSymbol = new TextSymbolClass();
                        pTextSymbol.Angle = 90;
                        pTextSymbol.Size = fontSize;
                        docTextElement.Symbol = pTextSymbol;

                        var centerPointDiffer = elev.Value - BaseElevation;
                        if (InitChartTypeA.DistanceConverter.Unit == "M")
                        {
                            centerPointDiffer = Math.Round(centerPointDiffer, 1);
                            centerPointDiffer = Math.Round(centerPointDiffer * 2, MidpointRounding.AwayFromZero) / 2;
                        }
                        else
                            centerPointDiffer = Math.Round(centerPointDiffer);
                        text = centerPointDiffer.ToString();
                        docTextElement.Text = text;
                        IPoint txtPnt = new PointClass();
                        if (rwyCentPnt.Role == PDM.CodeRunwayCenterLinePointRoleType.START || rwyCentPnt.Role == PDM.CodeRunwayCenterLinePointRoleType.THR)
                        {
                            if (Pnt1 == null)
                                txtPnt.PutCoords(pntPgLay.X + 0.4, pntPgLay.Y + 0.2 + 0.1 * (text.Length - 2));
                            else
                                txtPnt.PutCoords(pntPgLay.X - 0.4, pntPgLay.Y + 0.2 + 0.1 * (text.Length - 2));
                        }
                        else if (rwyCentPnt.Role == PDM.CodeRunwayCenterLinePointRoleType.END)
                        {
                            if (Pnt1 == null)
                                txtPnt.PutCoords(pntPgLay.X - 0.2, pntPgLay.Y + 0.2 + 0.1 * (text.Length - 2));
                            else
                                txtPnt.PutCoords(pntPgLay.X + 0.2, pntPgLay.Y + 0.2 + 0.1 * (text.Length - 2));
                        }
                        else
                        {
                            txtPnt.PutCoords(pntPgLay.X, pntPgLay.Y + 0.2 + 0.1 * (text.Length - 2));
                        }
                        docElement.Geometry = txtPnt;
                        _textGrp.AddElement(docElement);

                        list.Add(pntPgLay);
                    }
                }
            }

            layoutPtStart = list[0];
            layoutPtEnd = list[list.Count - 1];

            _profileGrp.AddElement(GlobalParams.UI.GetPolylineElement(list, Color));


            //Create Cells
            if (this.Side == TakeoffSide.Left)
                CreateCells(layoutPtEnd, ARANMath.C_PI, RowCount1, ColumnCount1, _cellWidth, _cellHeight, false);
            else
                CreateCells(layoutPtEnd, 0, RowCount1, ColumnCount1, _cellWidth, _cellHeight, true);


            CreateProfileLine(list);
            //}
        }

        private void CreateProfileLine(List<IPoint> list)
        {
            List<IPoint> pntList = new List<IPoint>();
            double lengthReminder, linePrjWidth, height;

            pntList.Clear();
            if (Pnt2!=null)
                pntList.Add(list[0]);
            else
                pntList.Add(list[list.Count - 1]);

            var dirProfile = (this.Side == TakeoffSide.Left ? 1 : 0) * ARANMath.C_PI;
            IPoint endPnt = EsriFunctions.LocalToPrj(pntList[0], dirProfile, (ClearWay1 * 100) / HorScale);
            pntList.Add(endPnt);
            _profileGrp.AddElement(GlobalParams.UI.GetPolylineElement(pntList, Color, esriSimpleLineStyle.esriSLSDot));

            pntList.Clear();
            pntList.Add(endPnt);
            lengthReminder = LengthRwy % 100;
            linePrjWidth = ColumnCount1 * HorizontalStep - lengthReminder - ClearWay1;
            height = Slope * linePrjWidth;
            _pntLower.Y = endPnt.Y + (height / VerScale);
            pntList.Add(_pntLower);
            _profileGrp.AddElement(GlobalParams.UI.GetPolylineElement(pntList, Color, esriSimpleLineStyle.esriSLSDashDot));

            var docTextElement = new TextElementClass();
            var docElement = docTextElement as IElement;
            TextSymbolClass pTextSymbol = new TextSymbolClass();
           
            pTextSymbol.Angle = (this.Side == TakeoffSide.Right ? 1 : -1) * -7;
            pTextSymbol.Size = fontSize;
            docTextElement.Symbol = pTextSymbol;
            docTextElement.Text = Slope + "% SLOPE";
            
            IPoint txtPnt = new PointClass();
            txtPnt.PutCoords((endPnt.X + _pntLower.X) * 0.5, (endPnt.Y + _pntLower.Y) * 0.5 + 0.1);
            docElement.Geometry = txtPnt;
            _profileGrp.AddElement(docElement);
        }

        private void CreateCells(IPoint pnt, double dir, int rowCount, int columnCount, double cellWidth, double cellHeight, bool toRight)
        {
            double columnHeight = rowCount * cellHeight;
            int side = toRight ? 1 : -1;
            double width;
            IPoint tmp;
            double tickWidth = cellWidth / TickCount;
            IPoint pnt_lower, pnt_Upper;
            double lengthReminder = LengthRwy % 100;
            string text;
            double distHor = (cellWidth * (HorizontalStep - lengthReminder)) / HorizontalStep;
            double rowWidth = distHor + (columnCount - 1) * cellWidth;
            int localTickCount;
            double tickReminder;
            List<IPoint> pntList = new List<IPoint>();
            IGroupElement grpElement = (IGroupElement)new GroupElementClass();
            IPoint pntEdge = null;
            for (int i = 0; i < columnCount + 1; i++)
            {
                pntList.Clear();
                if (i == 0)
                {
                    text = LengthRwy.ToString();
                    tickReminder = tickWidth - (lengthReminder * tickWidth) / (HorizontalStep / TickCount) % tickWidth;
                    width = 0;
                    localTickCount = (int)((distHor * TickCount) / cellWidth);
                    pnt_lower = pnt;
                }
                else
                {
                    tickReminder = 0;
                    text = (LengthRwy - lengthReminder + i * HorizontalStep).ToString();
                    pnt_lower = EsriFunctions.LocalToPrj(pnt, dir, distHor + (i - 1) * cellWidth);
                    width = distHor + (i - 1) * cellWidth;
                    localTickCount = TickCount;
                }

                pnt_Upper = EsriFunctions.LocalToPrj(pnt_lower, dir + side * ARANMath.C_PI_2, columnHeight);
                pntList.Add(pnt_lower);
                pntList.Add(pnt_Upper);
                grpElement.AddElement(_lineElementCreater.CreateLineElement(pnt_lower, pnt_Upper));

                grpElement.AddElement(_textCreater.CreateHorizontalText(text, pnt_lower, side));

                if (i == columnCount)
                {
                    pntEdge = pnt_lower;
                    break;
                }
                for (int j = 0; j < localTickCount; j++)
                {
                    pnt_lower = EsriFunctions.LocalToPrj(pnt, dir, width + tickReminder + j * tickWidth);
                    pnt_Upper = EsriFunctions.LocalToPrj(pnt_lower, dir + side * ARANMath.C_PI_2, _tickLengthInScale);
                    grpElement.AddElement(_lineElementCreater.CreateLineElement(pnt_lower, pnt_Upper));
                }
            }

            double height;
            tickWidth = cellHeight / TickCount;
            IPoint pnt1, pnt2;
            for (int i = 0; i < rowCount + 1; i++)
            {
                pnt1 = EsriFunctions.LocalToPrj(pnt, dir + side * ARANMath.C_PI_2, i * cellHeight);
                pnt2 = EsriFunctions.LocalToPrj(pnt1, dir, rowWidth);
                grpElement.AddElement(_lineElementCreater.CreateLineElement(pnt1, pnt2));

                grpElement.AddElement(_textCreater.CreateVerticalText((BaseElevation + i * VerticalStep).ToString(), pnt2, side));
                if (i == rowCount)
                    continue;
                height = i * cellHeight;
                for (int j = 1; j < TickCount; j++)
                {
                    pnt1 = EsriFunctions.LocalToPrj(pnt, dir + side * ARANMath.C_PI_2, height + j * tickWidth);
                    pnt2 = EsriFunctions.LocalToPrj(pnt1, dir, _tickLengthInScale);
                    grpElement.AddElement(_lineElementCreater.CreateLineElement(pnt1, pnt2));

                    tmp = EsriFunctions.LocalToPrj(pnt, dir, rowWidth);
                    pnt1 = EsriFunctions.LocalToPrj(tmp, dir + side * ARANMath.C_PI_2, height + j * tickWidth);
                    pnt2 = EsriFunctions.LocalToPrj(pnt1, dir, -_tickLengthInScale);
                    grpElement.AddElement(_lineElementCreater.CreateLineElement(pnt1, pnt2));
                }
            }
            _rightGridGrp = null;
            _leftGridGrp = null;
            if (Side == TakeoffSide.Right)
            {
                _pntLower = pntEdge;
                _rightGridGrp = grpElement;
            }
            else
            {
                _pntLower = pntEdge;
                _leftGridGrp = grpElement;
            }
        }

    }
}
