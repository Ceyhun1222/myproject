using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;

namespace SigmaChart
{
    public class GlobalParams
    {
        public static IHookHelper  HookHelper{ get; set; }
        public static IActiveView ActiveView {
            get { return HookHelper.ActiveView as IActiveView; }
        }

        public static DbModule DbModule { get; set; }


        public static Aran.PANDA.Common.UnitConverter UnitConverter { get; set; }
    }
}
