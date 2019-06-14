using System;
using Aran.PANDA.Common;

namespace Aran.Omega.SettingsUI
{
    public class InterfaceModel : SettingsModel
    {
        public InterfaceModel()
        {
            Type = MenuType.Interface;
        }
        public VerticalDistanceType DistanceUnit { get; set; }
        public VerticalDistanceType HeightUnit { get; set; }

        public Double DistancePrecision
        {
            get { return _distancePrecision; }
            set { _distancePrecision = value; }
        }

        public Double HeightPrecision
        {
            get { return _heightPrecision; }
            set { _heightPrecision = value; }
        }

        private Double _distancePrecision = 0.1;
        private Double _heightPrecision = 1.0;
    }
}