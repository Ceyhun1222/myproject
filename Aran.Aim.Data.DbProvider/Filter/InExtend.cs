using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aran.Aim.Data.Filters;

namespace Aran.Aim.Data.Filters
{
    [Serializable]
    public class InExtend : SpatialOps
    {
        public double MinX
        {
            get;
            set;
        }

        public double MinY
        {
            get;
            set;
        }

        public double MaxX
        {
            get;
            set;
        }

        public double MaxY
        {
            get;
            set;
        }
    }
}
