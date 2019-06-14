using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aran.Geometries.Operators;
using Aran.PANDA.Common;
using Aran.Panda.RadarMA.Models;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Geodatabase;

namespace Aran.Panda.RadarMA
{
    public class GlobalParams
    {
        private static object _hookHelper;
        static GlobalParams()
        {
            
        }

        public static int  Handle { get; set; }
        public static IMap Map { get; set; }

        public static IPageLayout PageLayout { get; set; }
        public static IActiveView ActiveView { get; set; }

        public static Models.DbModule DbModule { get; set; }
        public static Graphics UI { get; set; }

        public static SpatialReferenceOperation SpatialRefOperation { get; set; }
        public static MapToolItem AranMapToolMenuItem { get; set; }

        public static ESRI.ArcGIS.Framework.IApplication Application { get; set; }

        public static IHookHelper HookHelper { get;set;}

        public static SnapClass FetureSnap { get; set; }

        public static UnitConverter UnitConverter { get; set; }

        public static RadarSymbols RadarSymbol { get; set; }

        public static ESRI.ArcGIS.Geometry.IGeometry RadarVectoringArea { get; set; }

        public static IRaster SelectedRaster { get; set; }
        public static ObservableCollection<Sector> SectorList { get; set; }

        public static List<State> StateList { get; set; }

        public static ObservableCollection<Sector> UnAssignedSectors { get; set; }
    }
}
