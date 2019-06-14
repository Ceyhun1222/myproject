using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aran.PANDA.Common;

namespace Aran.Delta.Model
{
    public class IntersectResultPoint
    {
        private Aran.Geometries.Point _geo;
        public IntersectResultPoint()
        {
            
        }

        public string Header { get; set; }

        public Aran.Geometries.Point Geo
        {
            get { return _geo; }
            set
            {
                _geo = value;
                if (_geo != null)
                {
                    string lat, longtitude;

                    Long = ARANFunctions.Degree2String(_geo.X, Degree2StringMode.DMSLon,
                     Convert.ToInt32(GlobalParams.Settings.DeltaInterface.CoordinatePrecision));

                    Lat = ARANFunctions.Degree2String(_geo.Y, Degree2StringMode.DMSLat,
                     Convert.ToInt32(GlobalParams.Settings.DeltaInterface.CoordinatePrecision));

					//Aran.PANDA.Common.ARANFunctions.Dd2DmsStr(_geo.X, _geo.Y, ".", "E", "N", 1,
					//    Convert.ToInt32(GlobalParams.Settings.DeltaInterface.CoordinatePrecision), out lat, out longtitude);
					//Lat = lat;
					//Long = longtitude;
				}
			}
        }

        public string Lat { get; set; }
        public string Long { get; set; }

        public override string ToString()
        {
            return Lat + Environment.NewLine + Long;
        }
    }
}
