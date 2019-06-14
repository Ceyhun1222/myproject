using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESRI.ArcGIS.Geometry;
using Aran.Aim;
using Aran;
using Aran.Aim.Features;

namespace ARENA
{
    public class Intermediate_AIXM51_Arena
    {
        private Aran.Aim.Features.Feature _AIXM51_Feature;
        public Aran.Aim.Features.Feature AIXM51_Feature
        {
            get { return _AIXM51_Feature; }
            set { _AIXM51_Feature = value; }
        }

        private List<IGeometry> aixmGeo;
        public List<IGeometry> AixmGeo
        {
            get { return aixmGeo; }
            set { aixmGeo = value; }
        }

        public Intermediate_AIXM51_Arena()
        {
        }
    }
}
