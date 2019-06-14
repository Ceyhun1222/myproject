using System;
using System.Collections.Generic;
using System.Text;
using Aran.Geometries.Operators;
using Aran.Geometries.SpatialReferences;

namespace Aran.PANDA.Conventional.Settings
{
	public class ARANSettings
	{
		public ARANSettings ( )
		{

		}

		public void Save ()
		{
			throw new Exception ( );
		}

		public void SaveAs(string FileName)
		{
			throw new Exception ( );
		}

		public string GetString(string path, string defaultValue)
		{
			throw new Exception ( );
		}

		public double GetDouble(string path, double defaultValue)
		{
			throw new Exception ( );
		}

		public int GetInt32 ( string path, Int32 defaultValue)
		{
			int result;
			int.TryParse ( GetString ( path, defaultValue.ToString ( ) ), out result );
			return result;
		}

		public bool GetBoolean(string path, bool defaultValue)
		{
			throw new Exception ( );
		}

		public void SetString(string path, string value)
		{
			throw new Exception ( );
		}

		public void SetDouble(string path, double value)
		{
			throw new Exception ( );
		}

		public void SetInt32(string path, Int32 value)
		{
			throw new Exception ( );
		}

		public void SetBoolean(string path, bool value)
		{
			throw new Exception ( );
		}

		public LanguageCode GetLanguageCode ()
		{
			return (LanguageCode) (GetInt32 ("General/LangCode", 1033));
		}

        public LanguageCode GetLanguageCode(LanguageCode langCode)
        {
            return (LanguageCode)(GetInt32("General/LangCode", (int)langCode));
        }

		//public ConnectionInfo GetConnectionInfo()
		//{
		//    ConnectionInfo result = new ConnectionInfo();
		//    if (GetInt32("Db/ConnectionType", 1) == 0)
		//    {
		//        result.ConnectedType = ConnectionType.connectionTypePostgres;
		//        result.PgConnection.Host = GetString("Db/Pg/Host", "");
		//        result.PgConnection.DbName = GetString("Db/Pg/DbName", "");
		//        result.PgConnection.User = GetString("Db/Pg/User", "");
		//        result.PgConnection.Password = GetString("Db/Pg/Pwd", "");
		//    }
		//    else
		//    {
		//        result.ConnectedType = ConnectionType.connectionTypeMDB;
		//        result.MDBConnection.FileName = GetString("Db/MDB/FileName", "");
		//    }
		//    return result;
		//}

		public SpatialReference GetPrjSpatialReference()
		{
			SpatialReference result = new SpatialReference();
			result.SpatialReferenceType = SpatialReferenceType.srtTransverse_Mercator;
			result.SpatialReferenceUnit = SpatialReferenceUnit.sruMeter;
			result.Ellipsoid.SrGeoType = SpatialReferenceGeoType.srgtWGS1984;
			result.Name = GetString("CoordSys/Name", "");
			result.ParamList.Add ( new SpatialReferenceParam ( SpatialReferenceParamType.srptFalseEasting, GetDouble ( "CoordSys/FalseEasting", 500000.0 ) ) );
			result.ParamList.Add ( new SpatialReferenceParam ( SpatialReferenceParamType.srptFalseNorthing, GetDouble ( "CoordSys/FalseNorthing", 0.0 ) ) );
			result.ParamList.Add ( new SpatialReferenceParam ( SpatialReferenceParamType.srptCentralMeridian, GetDouble ( "CoordSys/CentralMeridian", 51.0 ) ) );
			result.ParamList.Add ( new SpatialReferenceParam ( SpatialReferenceParamType.srptLatitudeOfOrigin, GetDouble ( "CoordSys/LatitudeOrigin", 0.0 ) ) );
			result.ParamList.Add ( new SpatialReferenceParam ( SpatialReferenceParamType.srptScaleFactor, GetDouble ( "CoordSys/ScaleFactor", 0.9996 ) ) );
			return result;
		}

		public SpatialReference GetPrjSpatialReference(SpatialReference defaultValue)
		{
			SpatialReference result = new SpatialReference();
			result.SpatialReferenceType = SpatialReferenceType.srtTransverse_Mercator;
			result.SpatialReferenceUnit = SpatialReferenceUnit.sruMeter;
			result.Ellipsoid.SrGeoType = SpatialReferenceGeoType.srgtWGS1984;
			result.Name = GetString("CoordSys/Name", defaultValue.Name);
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

		//public void SetConnectionInfo(ConnectionInfo value)
		//{
		//    if (value.ConnectedType == ConnectionType.connectionTypePostgres)
		//    {
		//        SetInt32("Db/ConnectionType", 0);
		//        SetString("Db/Pg/Host", value.PgConnection.Host);
		//        SetString("Db/Pg/DbName", value.PgConnection.DbName);
		//        SetString("Db/Pg/User", value.PgConnection.User);
		//        SetString("Db/Pg/Pwd", value.PgConnection.Password);
		//    }
		//    else
		//    {
		//        SetInt32("Db/Connectiontype", 1);
		//        SetString("Db/MDB/FileName", value.MDBConnection.FileName);
		//    }
		//}

		public void SetPrjSpatialReference(SpatialReference value)
		{
			if (value.SpatialReferenceType != SpatialReferenceType.srtMercator)
			{
				return;
			}
			SetString("CoordSys/Name", value.Name);
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
				throw new Exception ( );
			}
		}

		public string FileName
		{
			get
			{
				throw new Exception ( );
			}
		}
	}
}
