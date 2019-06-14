using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Threading.Tasks;
using Aran.AranEnvironment;
using Aran.PANDA.Common;
using Aran.Panda.RadarMA.Models;
using ESRI.ArcGIS.ArcMapUI;
using ESRI.ArcGIS.Carto;

namespace Aran.Panda.RadarMA
{
    public static class InitRadarMa
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool SetThreadLocale(int dwLangID);

        public static int ObstacleDistanceFromArp = 20000;

        public static bool InitCommand()
        {
            try
            {
                NativeMethods.InitAll();
                 GlobalParams.DbModule = new DbModule();
                var aranTool = new AranTool {Cursor = System.Windows.Forms.Cursors.Cross, Visible = true};
                GlobalParams.AranMapToolMenuItem = new MapToolItem(aranTool);

                 if (GlobalParams.UI==null)
                    GlobalParams.UI = new Graphics();

                var document = GlobalParams.Application.Document as IMxDocument;
                if (document != null)
                {
                    GlobalParams.Map = document.FocusMap;
                    GlobalParams.SpatialRefOperation = new SpatialReferenceOperation(GlobalParams.Map.SpatialReference);
                    GlobalParams.PageLayout = document.PageLayout;
                    GlobalParams.ActiveView = document.FocusMap as IActiveView;
                }

                GlobalParams.FetureSnap = new SnapClass();

                var settings = new Settings();

                settings.UIIntefaceData.DistanceUnit = HorizantalDistanceType.KM;
                settings.UIIntefaceData.DistancePrecision = 1;
                settings.UIIntefaceData.HeightUnit = VerticalDistanceType.Ft;
                settings.UIIntefaceData.HeightPrecision = 100;
                //DistanceUnit = HorizantalDistanceType.NM,
                //    DistancePrecision = 1,
                //    HeightUnit = VerticalDistanceType.Ft,
                //    HeightPrecision = 100

                //settings.UIIntefaceData.

                GlobalParams.UnitConverter = new UnitConverter(settings);
                GlobalParams.RadarSymbol = RadarSymbols.Instance();

                return true;
            }
            catch (Exception e)
            {
                return false;
            }

        }
    }
}
