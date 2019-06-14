using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aran.AranEnvironment;
using System.Windows.Forms;
using Aran.PANDA.Common;
using Aran.Omega.SettingsUI;

namespace Europe_ICAO015
{
    public class Starter : Aran.AranEnvironment.AranPlugin
    {
        private AranTool _aranToolItems;

        public override Guid Id => new Guid("7B10894C-D235-4BA6-9807-0AC63D74738C");

        public override string Name => "ICAO015";

        public override void Startup(IAranEnvironment aranEnv)
        {
            _aranToolItems = new AranTool { Visible = false };
            _aranToolItems.Cursor = Cursors.Cross;
            aranEnv.AranUI.AddMapTool(_aranToolItems);

            var menuItem = new ToolStripMenuItem { Text = @"ICAO015" };
            menuItem.Click += MenuItem_Click;
            aranEnv.AranUI.AddMenuItem(AranMapMenu.Applications, menuItem);

            GlobalParams.AranEnvironment = aranEnv;
            GlobalParams.SpatialRefOperation = new SpatialReferenceOperation(GlobalParams.AranEnvironment);

            if (GlobalParams.Settings == null)
                GlobalParams.Settings = new OmegaSettings();

            GlobalParams.Settings.Load(GlobalParams.AranEnvironment);
            var settings = GlobalParams.Settings;

            _distanceUnit = settings.OLSInterface.DistanceUnit;
            _distancePrecision = settings.OLSInterface.DistancePrecision;

            _heightUnit = settings.OLSInterface.HeightUnit;
            _heightPrecision = settings.OLSInterface.HeightPrecision;

            double multiplier;
            string unit;
            if (_distanceUnit == VerticalDistanceType.M)
            {
                multiplier = 1;
                unit = ICAO015.Properties.Resources.str111;
            }
            else
            {
                multiplier = 1.0 / 1852.0;
                unit = ICAO015.Properties.Resources.str520;
            }
            DistanceConverter = new TypeConvert { MultiPlier = multiplier, Rounding = _distancePrecision, Unit = unit };

            if (_heightUnit == VerticalDistanceType.M)
            {
                multiplier = 1.0;
                unit = ICAO015.Properties.Resources.str111;
            }
            else if (_heightUnit == VerticalDistanceType.Ft)
            {
                multiplier = 1.0 / 0.3048;
                unit = ICAO015.Properties.Resources.str112;
            }

            HeightConverter = new TypeConvert { MultiPlier = multiplier, Rounding = _heightPrecision, Unit = unit };

            //var unitConverter = new UnitConverter(settings);

            //var win32Window = new Win32Windows(aranEnv.Win32Window.Handle.ToInt32());

        }

        private void MenuItem_Click(object sender, EventArgs e)
        {
            //DBModule IcaO015 = new DBModule();
            //IcaO015.ConnectionDB();
            Form1 frm = new Form1(_aranToolItems);
            //frm.Text = "Omega(ICAO015) - Airport / Heliport: " + IcaO015.GetAirportDesignator();
            GlobalParams.SpatialRefOperation = new SpatialReferenceOperation(GlobalParams.AranEnvironment);
            frm.Show(GlobalParams.AranEnvironment.Win32Window);
        }

        public static TypeConvert DistanceConverter { get; private set; }

        public static TypeConvert HeightConverter { get; private set; }

        public static TypeConvert SpeedConverter { get; private set; }

        public static TypeConvert DSpeedConverter { get; private set; }

        private static double _distancePrecision;
        private static double _heightPrecision;
        private static VerticalDistanceType _distanceUnit;
        private static VerticalDistanceType _heightUnit;
    }
}
