using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Aran.Interfaces;
using Aran.Package;

namespace Aran.PANDA.Settings
{
	public class Settings : IPackable
	{
		public const string SettingsKey = "PANDA Settings";
		private Int32 _versionNumber = 1;

		private Int32 _language = 0;

		private Int32 _distanceUnit = 0;
		private Int32 _heightUnit = 0;
		private Int32 _speedUnit = 0;

		private Double _distancePrecision = 0.1;
		private Double _heightPrecision = 1.0;
		private Double _speedPrecision = 1.0;
		private Double _dSpeedPrecision = 1.0;

		private Double _radius = 100000.0;
		private Guid _organization = Guid.Empty;
		private Guid _aeroport = Guid.Empty;

		public Int32 VersionNumber { get { return _versionNumber; } }
		public Int32 Language
		{
			get { return _language; }
			set { _language = value; }
		}

		public Int32 DistanceUnit
		{
			get { return _distanceUnit; }
			set { _distanceUnit = value; }
		}
		public Int32 HeightUnit
		{
			get { return _heightUnit; }
			set { _heightUnit = value; }
		}
		public Int32 SpeedUnit
		{
			get { return _speedUnit; }
			set { _speedUnit = value; }
		}

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

		public Double Radius
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

		public void Load(IPandaAranExtension aranExtension)
		{
			if (aranExtension.HasKey(SettingsKey))
			{
				IPackable p = this;
				aranExtension.GetData(SettingsKey, p);
			}
		}

		public void Store(IPandaAranExtension aranExtension)
		{
			IPackable p = this;
			aranExtension.PutData(SettingsKey, p);
		}

		#region IPackable Members

		public void Pack(PackageWriter writer)
		{
			writer.PutInt32(_versionNumber);
			writer.PutInt32(_language);

			writer.PutInt32(_distanceUnit);
			writer.PutDouble(_distancePrecision);

			writer.PutInt32(_heightUnit);
			writer.PutDouble(_heightPrecision);

			writer.PutInt32(_speedUnit);
			writer.PutDouble(_speedPrecision);
			writer.PutDouble(_dSpeedPrecision);

			writer.PutDouble(_radius);
			writer.PutString(_organization.ToString());
			writer.PutString(_aeroport.ToString());
		}

		public void Unpack(PackageReader reader)
		{
			_versionNumber = reader.GetInt32();
			if (_versionNumber == 1)
			{
				_language = reader.GetInt32();

				_distanceUnit = reader.GetInt32();
				_distancePrecision = reader.GetDouble();

				_heightUnit = reader.GetInt32();
				_heightPrecision = reader.GetDouble();

				_speedUnit = reader.GetInt32();
				_speedPrecision = reader.GetDouble();
				_dSpeedPrecision = reader.GetDouble();

				_radius = reader.GetDouble();
				_organization = Guid.Parse(reader.GetString());
				_aeroport = Guid.Parse(reader.GetString());
			}
		}
		#endregion
	}
}

