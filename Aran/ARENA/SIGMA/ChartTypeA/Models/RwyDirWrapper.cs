using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ESRI.ArcGIS.Geometry;
using PDM;
using Aran.PANDA.Common;

namespace ChartTypeA.Models
{
    public class RwyDirWrapper:NotifyClass
    {
        private PDM.RunwayDirection _rwyDir;
        private double _length;
        private double _clearWay;
        private double _tora;
        private double _toda;
        private double _lda;
        private double _asda;
        private bool _checked;

        public RwyDirWrapper(PDM.RunwayDirection rwyDirection)
        {
            _rwyDir = rwyDirection;
            Checked = true;
            Name = _rwyDir.Designator;
            
            IsEligible = false;
            
            if (_rwyDir == null) return;

            IsEligible = false;
            if (_rwyDir.CenterLinePoints != null)
            {
                var thrCntlPt = _rwyDir.CenterLinePoints.FirstOrDefault(cnt => cnt.Role == CodeRunwayCenterLinePointRoleType.THR);
                var endCntlPt = _rwyDir.CenterLinePoints.FirstOrDefault(cnt => cnt.Role == CodeRunwayCenterLinePointRoleType.END);
                var starCntlPt =
                    _rwyDir.CenterLinePoints.FirstOrDefault(cntl => cntl.Role == CodeRunwayCenterLinePointRoleType.START);

                if (starCntlPt != null)
                    starCntlPt.RebuildGeo();
                else
                    starCntlPt = thrCntlPt;

                if (starCntlPt.Geo != null)
                    Start = (IPoint)GlobalParams.SpatialRefOperation.ToEsriPrj(starCntlPt.Geo);

                if (thrCntlPt != null && endCntlPt != null)
                {
                    thrCntlPt.RebuildGeo();
                    endCntlPt.RebuildGeo();

                    ThresholdGeo =(IPoint) thrCntlPt.Geo;
                    EndGeo =(IPoint) endCntlPt.Geo;
                    var thrPt = GlobalParams.SpatialRefOperation.ToEsriPrj(thrCntlPt.Geo);
                    var endPt = GlobalParams.SpatialRefOperation.ToEsriPrj(endCntlPt.Geo);

                    Threshold = (IPoint)thrPt;
                    EndPt = (IPoint)endPt;

                    double inverseAzimuth, directionAzimuth;
                    EsriFunctions.ReturnGeodesicAzimuth(thrCntlPt.Geo as IPoint, endCntlPt.Geo as IPoint, out directionAzimuth,
                        out inverseAzimuth);
                    Aziumuth = directionAzimuth;

                    Direction =EsriFunctions.ReturnAngleInRadians((IPoint)thrPt, (IPoint)endPt);

                    _length = EsriFunctions.ReturnDistanceAsMetr(thrPt, endPt);

                    CodeNumber = 1;

                    if (Common.ConvertDistance(_length, roundType.toNearest) < 800)
                        CodeNumber = 1;
                    else if (Common.ConvertDistance(_length, roundType.toNearest) < 1200)
                        CodeNumber = 2;
                    else if (Common.ConvertDistance(_length, roundType.toNearest) < 1800)
                        CodeNumber = 3;
                    else
                        CodeNumber = 4;
                    CalculateClearAndStopWay();
                }

                if (Start != null || EndPt != null)
                    IsEligible = true;
            }
        }

		public List<PDM.RunwayCenterLinePoint> CenterLinePoints
		{
			get
			{
				return _rwyDir.CenterLinePoints;
			}
		}

        public bool IsEligible { get; set; }

        public bool Checked
        {
            get { return _checked; }
            set
            {
                _checked = value;
                
                if (RwyCheckedIsChanged != null)
                    RwyCheckedIsChanged(this, new EventArgs());
                
                NotifyPropertyChanged("Checked");
            }
        }

        public string Name { get; set; }

        public double Length
        {
            get { return Common.ConvertDistance(_length, roundType.toNearest); }
            set
            {
                _length = Common.DeConvertDistance(value);
            }
        }

        public int CodeNumber { get; set; }

        public double ClearWay
        {
            get { return Common.ConvertDistance(_clearWay,roundType.toNearest); }
            set
            {
                _clearWay = Common.DeConvertDistance(value);
            }
        }

        public double StopWay
        {
            get { return Common.ConvertDistance(_stopWay,roundType.toNearest); }
            set
            {
                _stopWay = Common.DeConvertDistance(value);
            }
        }

        public PDM.RunwayDirection RwyDir { get { return _rwyDir; } }

        public double TORA
        {
            get { return Common.ConvertDistance(_tora,roundType.toNearest); }
            set
            {
                _tora = Common.DeConvertDistance(value);
            }
        }

        public double TODA
        {
            get { return Common.ConvertDistance(_toda, roundType.toNearest); }
            set
            {
                _toda = Common.DeConvertDistance(value);
            }
        }

        public double LDA
        {
            get { return Common.ConvertDistance(_lda, roundType.toNearest); ; }
            set
            {
                _lda = Common.DeConvertDistance(value);
            }
        }

        public double ASDA
        {
            get { return Common.ConvertDistance(_asda, roundType.toNearest); }
            set
            {
                _asda = Common.DeConvertDistance(value);
            }
        }

        public double Direction { get; set; }

        public double Dir {
            get {return Aran.PANDA.Common.ARANMath.RadToDeg(Direction); }
        }

        public double Azim {
            get { return Aziumuth; }
        }

        public double Aziumuth
        {
            get { return Math.Round(_aziumuth); }
            set
            {
                _aziumuth = value;
            }
        }

        private void CalculateClearAndStopWay()
        {
            if (_rwyDir.RdnDeclaredDistance == null || _rwyDir.RdnDeclaredDistance.Count == 0) return;

            foreach (var declaredDistance in _rwyDir.RdnDeclaredDistance)
            {
                var valInM = EsriFunctions.FromDistanceVerticalM(declaredDistance.DistanceUOM,
                    declaredDistance.DistanceValue);
                if (declaredDistance.DistanceType == CodeDeclaredDistance.TORA)
                {
                    _tora = valInM;
                }
                else if (declaredDistance.DistanceType == CodeDeclaredDistance.TODA)
                {
                    _toda = valInM;
                }
                else if (declaredDistance.DistanceType == CodeDeclaredDistance.LDA)
                {
                    _lda = valInM;
                }
                else if (declaredDistance.DistanceType == CodeDeclaredDistance.ASDA)
                {
                    _asda = valInM;
                }
            }
            _clearWay = _toda - _tora;
            _stopWay = _asda - _tora;
        }

        public IPoint Threshold { get; set; }

        public IPoint Start { get; set; }

        public IPoint EndPt { get; set; }

        public IPoint ThresholdGeo { get; set; }

        public IPoint EndGeo { get; set; }

        public double OffsetWithDeg { get; set; }

        public event EventHandler RwyCheckedIsChanged;
        private double _stopWay;
        private double _aziumuth;
    }
}
