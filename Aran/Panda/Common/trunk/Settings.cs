using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aran.AranEnvironment;
using Aran.Package;

namespace Aran.PANDA.Common
{
	public enum HorizantalDistanceType { KM, NM };
	public enum VerticalDistanceType { M, Ft };
	public enum HorizantalSpeedType { KMInHour, Knot };
	public enum VerticalSpeedType { MeterInMin, FeetInMin };

    public class Settings : IPackable
    {
        public const string SettingsKey = "PANDA Settings";

        public Settings()
        {
            Language = 1033;
            AnnexObstalce = false;
            UIIntefaceData = new InterfaceData(InterfaceUnitType.UI);
            ReportIntefaceData = new InterfaceData(InterfaceUnitType.Report);
        }

        public Int32 VersionNumber
		{
			get { return _versionNumber; }
		}

        //#warning set's public olmali deyil
        public int Language { get; set; }

        public IInterfaceData UIIntefaceData { get; private set; }
        public IInterfaceData ReportIntefaceData { get; private set; }

        public HorizantalDistanceType DistanceUnit { get { return UIIntefaceData.DistanceUnit; } }
        public VerticalDistanceType HeightUnit { get { return UIIntefaceData.HeightUnit; } }
        public HorizantalSpeedType SpeedUnit { get { return UIIntefaceData.SpeedUnit; } }
        public bool AnnexObstalce { get; set; }

        public double DistancePrecision { get { return UIIntefaceData.DistancePrecision; } }

        public double HeightPrecision { get { return UIIntefaceData.HeightPrecision; } }

        public double SpeedPrecision { get { return UIIntefaceData.SpeedPrecision; } }

        public double DSpeedPrecision { get { return UIIntefaceData.DSpeedPrecision; } }

        public double AnglePrecision { get { return UIIntefaceData.AnglePrecision; } }

        public double GradientPrecision { get { return UIIntefaceData.GradientPrecision; } }

        public double Radius
		{
			get { return _radius; }
			set { _radius = value; }
		}

		public Guid Organization
		{
			get { return _organization; }
			set { _organization = value; }
		}

		public Guid Aeroport
		{
			get { return _aeroport; }
			set { _aeroport = value; }
		}

		public void Load(IAranEnvironment aranExtension)
		{
			if (aranExtension.HasExtKey(SettingsKey))
			{
				IPackable p = this;
				aranExtension.GetExtData(SettingsKey, p);
			}
		}

        public void Store(IAranEnvironment aranExtension)
		{
			IPackable p = this;
			aranExtension.PutExtData(SettingsKey, p);
		}

		#region IPackable Members

		public void Pack(PackageWriter writer)
		{
			writer.PutInt32(_versionNumber);
			writer.PutInt32((int)Language);

            (UIIntefaceData as IPackable)?.Pack(writer);

            writer.PutDouble(_radius);
			writer.PutString(_organization.ToString());
			writer.PutString(_aeroport.ToString());

            writer.PutBool(AnnexObstalce);

            (ReportIntefaceData as IPackable)?.Pack(writer);
		}

        public void Unpack(PackageReader reader)
        {
            int versionNumber = reader.GetInt32();

            Language = reader.GetInt32();

            (UIIntefaceData as IPackable)?.Unpack(reader);

            _radius = reader.GetDouble();
            _organization = Guid.Parse(reader.GetString());
            _aeroport = Guid.Parse(reader.GetString());

            AnnexObstalce = false;

            if (versionNumber >= 3)
                AnnexObstalce = reader.GetBool();

            if (versionNumber > 3)
                (ReportIntefaceData as IPackable)?.Unpack(reader);

        }
		#endregion

		private const Int32 _versionNumber = 4;

		private double _radius = 100000.0;
		private Guid _organization = Guid.Empty;
		private Guid _aeroport = Guid.Empty;
	}
}


