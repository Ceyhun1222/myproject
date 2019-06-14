using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aran.PANDA.Common;

namespace Aran.Delta.Settings.model
{
    public enum CoordinateUnitType
    {
        DD,
        DMS
    }

    public class InterfaceModel
    {
        public InterfaceModel()
        {
            AnglePrecision = 0.01;
            CoordinateUnit = CoordinateUnitType.DMS;
        }
        public HorizantalDistanceType DistanceUnit { get; set; }
        public VerticalDistanceType HeightUnit { get; set; }
        public CoordinateUnitType CoordinateUnit { get; set; }

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

        public Double CoordinatePrecision
        {
            get { return _coordinatePrecision; }
            set { _coordinatePrecision = value; }
        }

        public double AnglePrecision { get; set; }

        private Double _distancePrecision = 0.1;
        private Double _heightPrecision = 1.0;
        private Double _coordinatePrecision = 1.0;
    }
}
