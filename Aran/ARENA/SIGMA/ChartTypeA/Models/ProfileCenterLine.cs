using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ESRI.ArcGIS.Geometry;
using PDM;

namespace ChartTypeA.Models
{
    public class ProfileCenterLine
    {
        private int _startHandle;
        private int _endHandle;
        private double _length;
        private RunwayCenterLinePoint _start;
        private RunwayCenterLinePoint _end;
        private double _startElev;
        private double _endElev;
        public double Index { get; set; }

        public RunwayCenterLinePoint Start
        {
            get { return _start; }
            set
            {
                _start = value;
                var tmpValue = _start.ConvertValueToMeter(_start.Elev, _start.Elev_UOM.ToString());
                if (tmpValue.HasValue)
                    _startElev = tmpValue.Value;
            }
        }

        public RunwayCenterLinePoint End
        {
            get { return _end; }
            set
            {
                _end = value;
                var tmpValue = _end.ConvertValueToMeter(_end.Elev, _end.Elev_UOM.ToString());
                if (tmpValue.HasValue)
                    _endElev = tmpValue.Value;

            }
        }

        public double Percent { get; set; }

        public double StartElev
        {
            get { return  Common.ConvertHeight(_startElev,roundType.toNearest); }
            set { _startElev = Common.DeConvertHeight(value);}
        }

        public double EndElev
        {
            get { return Common.ConvertHeight(_endElev,roundType.toNearest); }
            set { _endElev = Common.DeConvertHeight(value); }
        }

        internal void Clear()
        {
            GlobalParams.UI.SafeDeleteGraphic(_startHandle);
            GlobalParams.UI.SafeDeleteGraphic(_endHandle);
        }

        internal void Draw()
        {
            Clear();
            var ptStart = GlobalParams.SpatialRefOperation.ToEsriPrj(Start.Geo);
                var ptEnd = GlobalParams.SpatialRefOperation.ToEsriPrj(End.Geo);

            _startHandle = GlobalParams.UI.DrawEsriPoint((IPoint) ptStart);
              _endHandle = GlobalParams.UI.DrawEsriPoint((IPoint) ptEnd);
        }

        public double Length
        {
            get { return Common.ConvertDistance(_length,roundType.toNearest); }
            set { _length = Common.DeConvertDistance(value); }
        }
    }
}