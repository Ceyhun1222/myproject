using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aran.Aim.Features;
using Aran.PANDA.Constants;

namespace Aran.Omega.Models
{
    public class IgnoredObstaclePair
    {
        public bool Checked { get; set; }
        public string Name => Obst1?.Name??"";
        public double HAcc => Obst1?.HorizontalAccuracy ?? 0;
        public double VAcc => Obst1?.VerticalAccuracy ?? 0;
        public string Surface => Obst1?.SurfaceType.ToString() ?? "";

        public string Reasen { get; set; }

        public string ConfObsName => Obst2?.Name??"";
        public double? HAccConf => Obst2?.HorizontalAccuracy ?? null;
        public double? VAccConf => Obst2?.VerticalAccuracy??null;
        public string SurfaceConf => Obst2?.SurfaceType.ToString() ?? "";
        public bool CheckedConf { get; set; }

        public ObstacleReport Obst1 { get; set; }
        public ObstacleReport Obst2 { get; set; }
        
        
    }
}
