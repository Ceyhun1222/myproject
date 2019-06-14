using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace ARENA.Settings
{
	public enum HorizantalDistanceType { KM, NM };
	public enum VerticalDistanceType { M, Ft };
	public enum HorizantalSpeedType { KMInHour, Knot };
	public enum VerticalSpeedType { MeterInMin, FeetInMin };
    public enum LanguageCode { English = 1033, Russian = 1049 };

	public class ArenaSettings
	{
		public const string SettingsKey = "PANDA Settings";

		public ArenaSettings()
		{
			DistanceUnit = HorizantalDistanceType.KM;
			HeightUnit = VerticalDistanceType.M;
			SpeedUnit = HorizantalSpeedType.KMInHour;
            Language = LanguageCode.English;
		}

		public Int32 VersionNumber
		{
			get
			{
				return _versionNumber;
			}
		}

        [Browsable(false)]
        public LanguageCode Language { get; set; }
		public HorizantalDistanceType DistanceUnit { get; set; }
		public VerticalDistanceType HeightUnit { get; set; }
		public HorizantalSpeedType SpeedUnit { get; set; }

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

		public Double SpeedPrecision
		{
			get { return _speedPrecision; }
			set { _speedPrecision = value; }
		}

		public Double DSpeedPrecision
		{
			get { return _dSpeedPrecision; }
			set { _dSpeedPrecision = value; }
		}

		public Double AnglePrecision
		{
			get { return _anglePrecision; }
			set { _anglePrecision = value; }
		}

		public Double GradientPrecision
		{
			get { return _gradientPrecision; }
			set { _gradientPrecision = value; }
		}

		public Double Radius
		{
			get { return _radius; }
			set { _radius = value; }
		}

        //public Guid Organization
        //{
        //    get { return _organization; }
        //    set { _organization = value; }
        //}

        //public Guid Aeroport
        //{
        //    get { return _aeroport; }
        //    set { _aeroport = value; }
        //}

		public void Load()
		{
            //if (aranExtension.HasKey(SettingsKey))
            //{
            //    aranExtension.GetData(SettingsKey, p);
            //}
		}

		public void Store()
		{
            //aranExtension.PutData(SettingsKey, p);
		}


		private const Int32 _versionNumber = 2;

		private Double _distancePrecision = 0.1;
		private Double _heightPrecision = 1.0;
		private Double _speedPrecision = 1.0;
		private Double _dSpeedPrecision = 1.0;
		private Double _anglePrecision = 1.0;
		private Double _gradientPrecision = 0.001;

		private Double _radius = 100000.0;
        //private Guid _organization = Guid.Empty;
        //private Guid _aeroport = Guid.Empty;
	}
}


