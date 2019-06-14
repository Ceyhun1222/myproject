using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aran.Aim.Metadata.Geo
{
    public class GeoClassInfo
    {
        public AimClassInfo AimClassInfo { get; set; }
        public List<List<AimPropInfo>> PointProps { get; set; }
        public List<List<AimPropInfo>> CurveProps { get; set; }
        public List<List<AimPropInfo>> SurfaceProps { get; set; }

        public List<List<AimPropInfo>> GetProps (AimGeomType geomType)
        {
            switch (geomType)
            {
                case AimGeomType.Point: return PointProps;
                case AimGeomType.Curve: return CurveProps;
                case AimGeomType.Surface: return SurfaceProps;
            }
            return null;
        }

        public List<List<AimPropInfo>> GetProps ()
        {
            List<List<AimPropInfo>> result = new List<List<AimPropInfo>> ();
            result.AddRange (PointProps);
            result.AddRange (CurveProps);
            result.AddRange (SurfaceProps);
            return result;
        }
    }
}
