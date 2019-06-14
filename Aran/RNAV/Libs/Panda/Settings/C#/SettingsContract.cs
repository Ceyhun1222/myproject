using System;
using System.Collections.Generic;
using System.Text;
using ARAN.Contracts.Registry;
//using ARAN.Contracts.
using ARAN.Common;

namespace ARAN.Contracts.Settings
{
	#region Enums

	public enum SettingsCommands
	{
        settingsGetFileName,
        settingsIsFileNameEmpty,
        settingsGetLanguageCode,
        settingsGetDistanceUnit,
        settingsGetElevationUnit,
        settingsGetSpeedUnit,
        settingsGetDistanceAccuracy,
        settingsGetElevationAccuracy,
        settingsGetSpeedAccuracy,
        settingsGetAngleAccuracy,
        settingsGetGradientAccuracy,
	};

	public enum HorisontalDistanceUnit { hduMeter, hduKM, hduNM };
	public enum VerticalDistanceUnit { vduMeter, vduFeet, vduFL, vduSM };
	public enum HorisontalSpeedUnit { hsuMeterInSec, hsuKMInHour, hsuKnot };
	public enum VerticalSpeedUnit { vsuMeterInMin, vsuFeetInMin };
	public enum LanguageCode  {langCodeEng = 1033, langCodeRus = 1049, langCodePortuguese = 1046};

	#endregion

	public class ConstClass
	{
		static readonly string[] GHorisontalDistanceUnitName = new string[] { "m", "km", "NM" };

		static readonly string[] GVerticalDistanceUnitName = new string[] { "m", "feet", "FL", "SM" };

		static readonly string[] GHorisontalSpeedUnitName = new string[] { "meter/sec", "km/h", "knot" };

		static readonly string[] GVerticalSpeedUnit = new string[] { "m/min", "feet/min" };
	}

	public class ARANSettings
	{
		public ARANSettings()
		{
			_handle = Registry_Contract.GetInstance("SettingsService");
		}

		~ARANSettings()
		{
			Registry_Contract.FreeInstance((int)_handle);
		}

		public bool IsValid()
		{
			return (_handle != 0);
		}

        public double GetDistanceAccuracy()
        {
            Registry_Contract.BeginMessage(_handle, (int)SettingsCommands.settingsGetDistanceAccuracy);
            Registry_Contract.EndMessage(_handle);
            return Registry_Contract.GetDouble(_handle);
        }

        public HorisontalDistanceUnit GetDistanceUnit()
        {
            Registry_Contract.BeginMessage(_handle, (int)SettingsCommands.settingsGetDistanceUnit);
            Registry_Contract.EndMessage(_handle);
            return (HorisontalDistanceUnit)Registry_Contract.GetInt32(_handle);
        }

        public double GetElevationAccuracy()
        {
            Registry_Contract.BeginMessage(_handle, (int)SettingsCommands.settingsGetElevationAccuracy);
            Registry_Contract.EndMessage(_handle);
            return Registry_Contract.GetDouble(_handle);
        }

        public VerticalDistanceUnit GetElevationUnit()
        {
            Registry_Contract.BeginMessage(_handle, (int)SettingsCommands.settingsGetElevationUnit);
            Registry_Contract.EndMessage(_handle);
            return (VerticalDistanceUnit)Registry_Contract.GetInt32(_handle);
        }

        public double GetSpeedAccuracy()
        {
            Registry_Contract.BeginMessage(_handle, (int)SettingsCommands.settingsGetSpeedAccuracy);
            Registry_Contract.EndMessage(_handle);
            return Registry_Contract.GetDouble(_handle);
        }

        public HorisontalSpeedUnit GetSpeedUnit()
        {
            Registry_Contract.BeginMessage(_handle, (int)SettingsCommands.settingsGetSpeedUnit);
            Registry_Contract.EndMessage(_handle);
            return (HorisontalSpeedUnit)Registry_Contract.GetInt32(_handle);
        }

        public LanguageCode GetLanguageCode()
        {
            Registry_Contract.BeginMessage(_handle, (int)SettingsCommands.settingsGetLanguageCode);
            Registry_Contract.EndMessage(_handle);
            return (LanguageCode)Registry_Contract.GetInt32(_handle);
        }

        public double GetGradientAccuracy()
        {
            try
            {
                Registry_Contract.BeginMessage(_handle, (int)SettingsCommands.settingsGetGradientAccuracy);
                Registry_Contract.EndMessage(_handle);
                return Registry_Contract.GetDouble(_handle);     
            }
            catch (Exception)
            {

                return 0.1;
            }
            
        }
        /*
        public double GetSpeedAccuracy()
        {
            Registry_Contract.BeginMessage(_handle, (int)SettingsCommands.settingsGetSpeedAccuracy);
            Registry_Contract.EndMessage(_handle);
            return Registry_Contract.GetDouble(_handle);
        }

		public string GetString(string path, string defaultValue)
		{
			Registry_Contract.BeginMessage(_handle, (int)SettingsCommands.settingsGetString);
			Registry_Contract.PutString(_handle, path);
			Registry_Contract.PutString(_handle, defaultValue);
			Registry_Contract.EndMessage(_handle);
			return Registry_Contract.GetString(_handle);
		}

		public double GetDouble(string path, double defaultValue)
		{
			Registry_Contract.BeginMessage(_handle, (int)SettingsCommands.settingsGetDouble);
			Registry_Contract.PutString(_handle, path);
			Registry_Contract.PutDouble(_handle, defaultValue);
			Registry_Contract.EndMessage(_handle);
			return Registry_Contract.GetDouble(_handle);
		}

		public double GetInt32(string path, Int32 defaultValue)
		{
			Registry_Contract.BeginMessage(_handle, (int)SettingsCommands.settingsGetInt32);
			Registry_Contract.PutString(_handle, path);
			Registry_Contract.PutInt32(_handle, defaultValue);
			Registry_Contract.EndMessage(_handle);
			return Registry_Contract.GetInt32(_handle);
		}

		public bool GetBoolean(string path, bool defaultValue)
		{
			Registry_Contract.BeginMessage(_handle, (int)SettingsCommands.settingsGetBoolean);
			Registry_Contract.PutString(_handle, path);
			Registry_Contract.PutBool (_handle, defaultValue);
			Registry_Contract.EndMessage(_handle);
			return Registry_Contract.GetBool(_handle);
		}

		public void SetString(string path, string value)
		{
			Registry_Contract.BeginMessage(_handle, (int)SettingsCommands.settingsSetString);
			Registry_Contract.PutString(_handle, path);
			Registry_Contract.PutString(_handle, value);
			Registry_Contract.EndMessage(_handle);
		}

		public void SetDouble(string path, double value)
		{
			Registry_Contract.BeginMessage(_handle, (int)SettingsCommands.settingsSetDouble);
			Registry_Contract.PutString(_handle, path);
			Registry_Contract.PutDouble(_handle, value);
			Registry_Contract.EndMessage(_handle);
		}

		public void SetInt32(string path, Int32 value)
		{
			Registry_Contract.BeginMessage(_handle, (int)SettingsCommands.settingsSetDouble);
			Registry_Contract.PutString(_handle, path);
			Registry_Contract.PutInt32(_handle, value);
			Registry_Contract.EndMessage(_handle);
		}

		public void SetBoolean(string path, bool value)
		{
			Registry_Contract.BeginMessage(_handle, (int)SettingsCommands.settingsSetDouble);
			Registry_Contract.PutString(_handle, path);
			Registry_Contract.PutBool(_handle, value);
			Registry_Contract.EndMessage(_handle);
		}

		public LanguageCode GetLanguageCode ()
		{
			return (LanguageCode) (GetInt32 ("General/LangCode", 1033));
		}

        public LanguageCode GetLanguageCode(LanguageCode langCode)
        {
            return (LanguageCode)(GetInt32("General/LangCode", (int)langCode));
        }



		/*
		public string GetValue(string path, string defaultValue)
		{
			Registry_Contract.BeginMessage(_handle, (int)SettingsCommands.settingsGetValue);
			Registry_Contract.PutString(_handle, path);
			Registry_Contract.PutString(_handle, defaultValue);
			Registry_Contract.EndMessage(_handle);
			return Registry_Contract.GetString(_handle);
		}

		public void SetValue(string path, string value)
		{
			Registry_Contract.BeginMessage(_handle, (int)SettingsCommands.settingsSetValue);
			Registry_Contract.PutString(_handle, path);
			Registry_Contract.PutString(_handle, value);
			Registry_Contract.EndMessage(_handle);
		}
		*/
        /*
		public SpatialReference GetPrjSpatialReference()
		{
			SpatialReference result = new SpatialReference();
			result.spatialReferenceType = SpatialReferenceType.srtTransverse_Mercator;
			result.spatialReferenceUnit = SpatialReferenceUnit.sruMeter;
			result.Ellipsoid.srGeoType = SpatialReferenceGeoType.srgtWGS1984;
			result.name = GetString("CoordSys/Name", "");
			result.ParamList.Add(new SpatialReferenceParam(SpatialReferenceParamType.srptFalseEasting, GetDouble("CoordSys/FalseEasting", 500000.0)));
			result.ParamList.Add(new SpatialReferenceParam(SpatialReferenceParamType.srptFalseNorthing, GetDouble("CoordSys/FalseNorthing", 0.0)));
			result.ParamList.Add(new SpatialReferenceParam(SpatialReferenceParamType.srptCentralMeridian, GetDouble("CoordSys/CentralMeridian", 51.0)));
			result.ParamList.Add(new SpatialReferenceParam(SpatialReferenceParamType.srptLatitudeOfOrigin, GetDouble("CoordSys/LatitudeOrigin", 0.0)));
			result.ParamList.Add(new SpatialReferenceParam(SpatialReferenceParamType.srptScaleFactor, GetDouble("CoordSys/ScaleFactor", 0.9996)));
			return result;
		}

		public SpatialReference GetPrjSpatialReference(SpatialReference defaultValue)
		{
			SpatialReference result = new SpatialReference();
			result.spatialReferenceType = SpatialReferenceType.srtTransverse_Mercator;
			result.spatialReferenceUnit = SpatialReferenceUnit.sruMeter;
			result.Ellipsoid.srGeoType = SpatialReferenceGeoType.srgtWGS1984;
			result.name = GetString("CoordSys/Name", defaultValue.name);
			result.ParamList.Add(new SpatialReferenceParam(SpatialReferenceParamType.srptFalseEasting,
				GetDouble("CoordSys/FalseEasting", defaultValue[SpatialReferenceParamType.srptFalseEasting])));
			result.ParamList.Add(new SpatialReferenceParam(SpatialReferenceParamType.srptFalseNorthing,
				GetDouble("CoordSys/FalseNorthing", defaultValue[SpatialReferenceParamType.srptFalseNorthing])));
			result.ParamList.Add(new SpatialReferenceParam(SpatialReferenceParamType.srptCentralMeridian,
				GetDouble("CoordSys/CentralMeridian", defaultValue[SpatialReferenceParamType.srptCentralMeridian])));
			result.ParamList.Add(new SpatialReferenceParam(SpatialReferenceParamType.srptLatitudeOfOrigin,
				GetDouble("CoordSys/LatitudeOrigin", defaultValue[SpatialReferenceParamType.srptLatitudeOfOrigin])));
			result.ParamList.Add(new SpatialReferenceParam(SpatialReferenceParamType.srptScaleFactor,
				GetDouble("CoordSys/ScaleFactor", defaultValue[SpatialReferenceParamType.srptScaleFactor])));
			return result;
		}

		public HorisontalDistanceUnit GetDistanceUnit()
		{
			HorisontalDistanceUnit result;
			try
			{
				result = (HorisontalDistanceUnit)GetInt32("Distance/Unit", 1);
			}
			catch
			{
				result = HorisontalDistanceUnit.hduKM;
			}
			return result;
		}

        public HorisontalDistanceUnit GetDistanceUnit(HorisontalDistanceUnit hDistanceUnit)
        {
            HorisontalDistanceUnit result;
            try
            {
                result = (HorisontalDistanceUnit)GetInt32("Distance/Unit", (int)hDistanceUnit);
            }
            catch
            {
                result = hDistanceUnit;
            }
            return result;
        }

		public VerticalDistanceUnit GetElevationUnit()
		{
			VerticalDistanceUnit result;
			try
			{
				result = (VerticalDistanceUnit)GetInt32("Elevation/Unit", ((int)VerticalDistanceUnit.vduMeter));
			}
			catch
			{
				result = VerticalDistanceUnit.vduMeter;
			}
			return result;
		}

        public VerticalDistanceUnit GetElevationUnit(VerticalDistanceUnit vDistanceUnit)
        {
            VerticalDistanceUnit result;
            try
            {
                result = (VerticalDistanceUnit)GetInt32("Elevation/Unit", (int)vDistanceUnit);
            }
            catch
            {
                result = vDistanceUnit;
            }
            return result;
        }


		public HorisontalSpeedUnit GetSpeedUnit()
		{
			HorisontalSpeedUnit result;
			try
			{
				result = (HorisontalSpeedUnit)GetInt32("Speed/Unit", 1);
			}
			catch
			{
				result = HorisontalSpeedUnit.hsuKMInHour;
			}

			return result;
		}

        public HorisontalSpeedUnit GetSpeedUnit(HorisontalSpeedUnit hSpeedUnit)
        {
            HorisontalSpeedUnit result;
            try
            {
                result = (HorisontalSpeedUnit)GetInt32("Speed/Unit", (int)hSpeedUnit);
            }
            catch
            {
                result = hSpeedUnit;
            }

            return result;
        }

		public double GetDistanceAccuracy()
		{
			return GetDouble("Distance/Accuracy", 1);
		}
        
        public double GetDistanceAccuracy(double distanceAccuracy)
        {
            return GetDouble("Distance/Accuracy", distanceAccuracy);
        }


        public VerticalSpeedUnit GetDSpeedUnit()
        {
            VerticalSpeedUnit result;
            try
            {
                result = (VerticalSpeedUnit)GetInt32("DSpeed/Unit", 1);
            }
            catch
            {
                result = VerticalSpeedUnit.vsuMeterInMin;
            }

            return result;
        }

        public VerticalSpeedUnit GetDSpeedUnit(VerticalSpeedUnit hSpeedUnit)
        {
            VerticalSpeedUnit result;
            try
            {
                result = (VerticalSpeedUnit)GetInt32("DSpeed/Unit", (int)hSpeedUnit);
            }
            catch
            {
                result = hSpeedUnit;
            }

            return result;
        }

		public double GetElevationAccuracy()
		{
			double result;
			try
			{
				result = GetDouble("Elevation/Accuracy", 1);
			}
			catch
			{
				result = 1;
			}
			return result;
		}

        public double GetElevationAccuracy(double elevAccuracy)
        {
            double result;
            try
            {
                result = GetDouble("Elevation/Accuracy", elevAccuracy);
            }
            catch
            {
                result = elevAccuracy;
            }
            return result;
        }


		public double GetSpeedAccuracy()
		{
			double result;
			try
			{
				result = GetDouble("Speed/Accuracy", 1);
			}
			catch
			{
				result = 1;
			}
			return result;
		}

        public double GetSpeedAccuracy(double speedAccuracy)
        {
            double result;
            try
            {
                result = GetDouble("Speed/Accuracy", speedAccuracy);
            }
            catch
            {
                result = speedAccuracy;
            }
            return result;
        }

        public double GetDSpeedAccuracy()
        {
            double result;
            try
            {
                result = GetDouble("DSpeed/Accuracy", 1);
            }
            catch
            {
                result = 1;
            }
            return result;
        }

        public double GetDSpeedAccuracy(double dSpeedAccuracy)
        {
            double result;
            try
            {
                result = GetDouble("DSpeed/Accuracy", dSpeedAccuracy);
            }
            catch
            {
                result = dSpeedAccuracy;
            }
            return result;
        }

		public double GetAngleAccuracy()
		{
			double result;
			try
			{
				result = GetDouble("Angle/Accuracy", 1);
			}
			catch
			{
				result = 1;
			}
			return result;
		}

        public double GetAngleAccuracy(double angleAccuracy)
        {
            double result;
            try
            {
                result = GetDouble("Angle/Accuracy", angleAccuracy);
            }
            catch
            {
                result = angleAccuracy;
            }
            return result;
        }


		public double GetGradientAccuracy()
		{
			double result;
			try
			{
				result = GetDouble("Gradient/Accuracy", 0.1);
			}
			catch
			{
				result = 0.1;
			}
			return result;
		}

        public double GetGradientAccuracy(double gradientAccuracy)
        {
            double result;
            try
            {
                result = GetDouble("Gradient/Accuracy", gradientAccuracy);
            }
            catch
            {
                result = gradientAccuracy;
            }
            return result;
        }


		public void SetLanguageCode (LanguageCode value)
		{
			SetInt32 ("General/LangCode", (Int32)value);
		}



		public void SetConnectionInfo(ConnectionInfo value)
		{
			if (value.ConnectedType == ConnectionType.connectionTypePostgres)
			{
				SetInt32("Db/ConnectionType", 0);
				SetString("Db/Pg/Host", value.PgConnection.Host);
				SetString("Db/Pg/DbName", value.PgConnection.DbName);
				SetString("Db/Pg/User", value.PgConnection.User);
				SetString("Db/Pg/Pwd", value.PgConnection.Password);
			}
			else
			{
				SetInt32("Db/Connectiontype", 1);
				SetString("Db/MDB/FileName", value.MDBConnection.FileName);
			}
		}

		public void SetPrjSpatialReference(SpatialReference value)
		{
			if (value.spatialReferenceType != SpatialReferenceType.srtMercator)
			{
				return;
			}
			SetString("CoordSys/Name", value.name);
			SetDouble("CoordSys/FalseEasting", value[SpatialReferenceParamType.srptFalseEasting]);
			SetDouble("CoordSys/FalseNorthing", value[SpatialReferenceParamType.srptFalseNorthing]);
			SetDouble("CoordSys/CentralMeridian", value[SpatialReferenceParamType.srptCentralMeridian]);
			SetDouble("CoordSys/LatitudeOrigin", value[SpatialReferenceParamType.srptLatitudeOfOrigin]);
			SetDouble("CoordSys/ScaleFactor", value[SpatialReferenceParamType.srptScaleFactor]);
		}

		public void SetDistanceUnit(HorisontalDistanceUnit value)
		{
			SetInt32("Distance/Unit", ((int)value));
		}

		public void SetElevationUnit(VerticalDistanceUnit value)
		{
			SetInt32("Elevation/Unit", ((int)value));
		}

		public void SetSpeedUnit(HorisontalDistanceUnit value)
		{
			SetInt32("Speed/Unit", ((int)value));
		}

        public void SetDSpeedUnit(VerticalSpeedUnit value)
        { 
            SetInt32("DSpeed/Unitm",((int)value));
        }

		public void SetDistanceAccuracy(double value)
		{
			SetDouble("Distance/Accuracy", value);
		}

		public void SetElevationAccuracy(double value)
		{
			SetDouble("Elevation/Accuracy", value);
		}

		public void SetSpeedAccuracy(double value)
		{
			SetDouble("Speed/Accuracy", value);
		}

        public void SetDSpeedAccuracy(double value)
        {
            SetDouble("DSpeed/Accuracy", value);
        }

		public void SetAngleAccuracy(double value)
		{
			SetDouble("Angle/Accuracy", value);
		}

		public void SetGradientAccuracy(double value)
		{
			SetDouble("Gradient/Accuracy", value);
		}

		public bool IsFileNameEmpty
		{
			get
			{
				Registry_Contract.BeginMessage(_handle, (int)SettingsCommands.settingsIsFileNameEmpty);
				Registry_Contract.EndMessage(_handle);
				return Registry_Contract.GetBool(_handle);
			}
		}

		public string FileName
		{
			get
			{
				Registry_Contract.BeginMessage(_handle, (int)SettingsCommands.settingsGetFileName);
				Registry_Contract.EndMessage(_handle);
				return Registry_Contract.GetString(_handle);
			}
		}
        
         */ 
		private int _handle;
	}
}
