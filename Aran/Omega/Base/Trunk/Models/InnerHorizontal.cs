using System;
using System.Collections.Generic;
using System.Globalization;
using Aran.Aim.Features;
using Aran.Geometries;
using Aran.Omega.Enums;
using System.Threading.Tasks;
using System.Windows.Threading;
using System.Collections.Concurrent;
using Aran.Omega.Strategy.ObstacleCalculation;
using Aran.Omega.Strategy.UI;

namespace Aran.Omega.Models
{
    public class InnerHorizontal : SurfaceBase,IStraightSurface
    {
        public InnerHorizontal()
        {
            SurfaceType = Aran.PANDA.Constants.SurfaceType.InnerHorizontal;
        }

        public double Radius { get; set; }
        public double Elevation { get; set; }
        public double Height { get; set; }

        private IList<Info> _propList = new List<Info>();

        public override IList<Info> PropertyList
        {
            get
            {
                _propList.Clear();
                _propList.Add(new Info("Radius", Common.ConvertDistance(Radius, RoundType.ToNearest).ToString(CultureInfo.InvariantCulture),
                    InitOmega.DistanceConverter.Unit));
                _propList.Add(new Info("Height", Common.ConvertHeight(Height, RoundType.ToNearest).ToString(CultureInfo.InvariantCulture),
                    InitOmega.HeightConverter.Unit));
                return _propList;
            }
        
            set => _propList = value;
        }

        public override PointPenetrateModel GetManualReport(Geometries.Point pt)
        {
            if (GeoPrj.IsPointInside(pt)) 
            {
                var result = new PointPenetrateModel();
                result.Surface = "Inner Horizontal";
                result.Penetration =Common.ConvertHeight(pt.Z - Elevation,RoundType.ToNearest);
                result.Elevation = Common.ConvertHeight(pt.Z,RoundType.ToNearest);
                result.Plane = "Z = " + Common.ConvertHeight(Elevation, RoundType.ToNearest);
                return result;
            }
            return null;
        }
    }
}
