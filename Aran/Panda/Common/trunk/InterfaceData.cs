using Aran.Package;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aran.PANDA.Common
{
    internal class InterfaceData : IInterfaceData, IPackable
    {

        public InterfaceData(InterfaceUnitType iUnitType)
        {
            if (iUnitType == InterfaceUnitType.UI)
            {
                DistanceUnit = HorizantalDistanceType.NM;
                DistancePrecision = 0.0001;
                HeightUnit = VerticalDistanceType.Ft;
                HeightPrecision = 0.1;
                SpeedUnit = HorizantalSpeedType.Knot;
                SpeedPrecision = 1;
                AnglePrecision = 0.1;
                GradientPrecision = 0.001;
            }
            else
            {
                DistanceUnit = HorizantalDistanceType.KM;
                DistancePrecision = 0.0001;
                HeightUnit = VerticalDistanceType.M;
                HeightPrecision = 0.1;
                SpeedUnit = HorizantalSpeedType.KMInHour;
                SpeedPrecision = 1;
                AnglePrecision = 0.1;
                GradientPrecision = 0.001;
            }
        }


        private double _distancePrecision = 1.0;
        private double _heightPrecision = 1.0;
        private double _speedPrecision = 1.0;
        private double _dSpeedPrecision = 1.0;
        private double _anglePrecision = 1.0;
        private double _gradientPrecision = 0.001;


        public HorizantalDistanceType DistanceUnit { get; set; }
        public VerticalDistanceType HeightUnit { get; set; }
        public HorizantalSpeedType SpeedUnit { get; set; }

        public double DistancePrecision
        {
            get { return _distancePrecision; }
            set { _distancePrecision = value; }
        }

        public double HeightPrecision
        {
            get { return _heightPrecision; }
            set { _heightPrecision = value; }
        }

        public double SpeedPrecision
        {
            get { return _speedPrecision; }
            set { _speedPrecision = value; }
        }

        public double DSpeedPrecision
        {
            get { return _dSpeedPrecision; }
            set { _dSpeedPrecision = value; }
        }

        public double AnglePrecision
        {
            get { return _anglePrecision; }
            set { _anglePrecision = value; }
        }

        public double GradientPrecision { get { return _gradientPrecision; } set { _gradientPrecision = value; } }

        public void Pack(PackageWriter writer)
        {
            writer.PutInt32((int)DistanceUnit);
            writer.PutDouble(_distancePrecision);

            writer.PutInt32((int)HeightUnit);
            writer.PutDouble(_heightPrecision);

            writer.PutInt32((int)SpeedUnit);
            writer.PutDouble(_speedPrecision);
            writer.PutDouble(_dSpeedPrecision);

            writer.PutDouble(_anglePrecision);
            writer.PutDouble(_gradientPrecision);
        }

        public void Unpack(PackageReader reader)
        {
            DistanceUnit = (HorizantalDistanceType)reader.GetInt32();
            _distancePrecision = reader.GetDouble();

            HeightUnit = (VerticalDistanceType)reader.GetInt32();
            _heightPrecision = reader.GetDouble();

            SpeedUnit = (HorizantalSpeedType)reader.GetInt32();
            _speedPrecision = reader.GetDouble();
            _dSpeedPrecision = reader.GetDouble();

            _anglePrecision = reader.GetDouble();
            _gradientPrecision = reader.GetDouble();
        }
    }
}
