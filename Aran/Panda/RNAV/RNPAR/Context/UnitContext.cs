using Aran.Aim.Enums;
using Aran.PANDA.Common;
using Aran.PANDA.Constants;

namespace Aran.Panda.RNAV.RNPAR.Context
{
    class UnitContext
    {
        private AppEnvironment _environment;
        public UnitConverter UnitConverter { get; }
        public Constants Constants { get;}

        public UomSpeed UomSpeed { get; }
        public UomDistance UomHDistance { get; }
        public UomDistanceVertical UomVDistance { get; }

        private UomDistance[] uomDistHorzTab = new UomDistance[] { UomDistance.KM, UomDistance.NM, UomDistance.M, UomDistance.FT, UomDistance.MI };
        private UomDistanceVertical[] uomDistVerTab = new UomDistanceVertical[] { UomDistanceVertical.M, UomDistanceVertical.FT }; //, UomDistanceVertical.OTHER, UomDistanceVertical.OTHER, UomDistanceVertical.OTHER
        private UomSpeed[] uomSpeedTab = new UomSpeed[] { UomSpeed.KM_H, UomSpeed.KT, UomSpeed.M_SEC, UomSpeed.MACH, UomSpeed.M_MIN, UomSpeed.FT_MIN, UomSpeed.FT_SEC };

        public UnitContext(AppEnvironment environment)
        {
            _environment = environment;
            UnitConverter = new UnitConverter(environment.Settings);

            if (Constants == null)
                Constants = new Constants();

            UomHDistance = uomDistHorzTab[UnitConverter.DistanceUnitIndex];
            UomVDistance = uomDistVerTab[UnitConverter.HeightUnitIndex];
            UomSpeed = uomSpeedTab[UnitConverter.SpeedUnitIndex];
        }
    }
}
