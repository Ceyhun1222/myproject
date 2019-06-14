using Aran.PANDA.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aran.Delta.Model
{
    class NavaidType
    {
        public Aran.Geometries.Point GeoPrj { get; set; }

        public eNavaidType TypeCode { get; set; }

        public double HorAccuracy { get; set; }
    }
}
