using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATRACC.Converter.Template.AtsRoutes
{
    partial class AtsRoutesTemplate
    {
        public List<RouteInfo> Routes { get; set; }
    }

    public class RouteInfo
    {
        public List<string> PointList { get; set; }

        public string Start { get; set; }
        public string End { get; set; }
        public string Name { get; set; }
    }
}
