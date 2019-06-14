using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using CDOTMA.Utilities;

namespace CDOTMA
{
	public enum HorizantalDistanceType { KM, NM };
	public enum VerticalDistanceType { M, Ft };
	public enum HorizantalSpeedType { KMInHour, Knot };
	public enum VerticalSpeedType { MeterInMin, FeetInMin };

	public class Settings 
	{
		private const Int32 _versionNumber = 2015;

		public Settings()
		{
			_language = 1033;
			_distanceUnit = HorizantalDistanceType.NM;
			_heightUnit = VerticalDistanceType.Ft;
			_speedUnit = HorizantalSpeedType.Knot;

			_distancePrecision = 0.001;
			_heightPrecision = 1.0;
			_speedPrecision = 1.0;
			_dSpeedPrecision = 1.0;
			_anglePrecision = 1.0;
			_gradientPrecision = 0.001;
		}

		public Int32 VersionNumber
		{
			get { return _versionNumber; }
		}

		public int Language
		{
			get { return _language; }
			set { _language = value; }
		}

		public HorizantalDistanceType DistanceUnit
		{
			get { return _distanceUnit; }
			set { _distanceUnit = value; }
		}

		public VerticalDistanceType HeightUnit
		{
			get { return _heightUnit; }
			set { _heightUnit = value; }
		}

		public HorizantalSpeedType SpeedUnit
		{
			get { return _speedUnit; }
			set { _speedUnit = value; }
		}

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

		public double GradientPrecision
		{
			get { return _gradientPrecision; }
			set { _gradientPrecision = value; }
		}

		public Settings Clone()
		{
			Settings result = new Settings();

			result._language = _language;
			result._distanceUnit = _distanceUnit;
			result._heightUnit = _heightUnit;
			result._speedUnit = _speedUnit;

			result._distancePrecision = _distancePrecision;
			result._heightPrecision = _heightPrecision;
			result._speedPrecision = _speedPrecision;
			result._dSpeedPrecision = _dSpeedPrecision;
			result._anglePrecision = _anglePrecision;
			result._gradientPrecision = _gradientPrecision;

			return result;
		}

		public override string ToString()
		{
			string result;

			result = string.Format("VersionNumber={0}\r\nLanguage={1}\r\n", _versionNumber, _language);
			result = result + string.Format("DistanceUnit={0}\r\nDistancePrecision={1}\r\n", (int)_distanceUnit, _distancePrecision);
			result = result + string.Format("HeightUnit={0}\r\nHeightPrecision={1}\r\n", (int)_heightUnit, _heightPrecision);
			result = result + string.Format("SpeedUnit={0}\r\nSpeedPrecision={1}\r\nDSpeedPrecision={2}\r\n", (int)_speedUnit, _speedPrecision, _dSpeedPrecision);
			result = result + string.Format("AnglePrecision={0}\r\nGradientPrecision={1}\r\n", _anglePrecision, _gradientPrecision);

			return result;
		}

		public void Write(FileStream fs)
		{
			StreamWriter sw = new StreamWriter(fs);

			sw.Write(ToString());

			sw.Close();
			sw.Dispose();
		}

		public void Write(string FileName)
		{
			FileStream fs = new System.IO.FileStream(FileName, FileMode.OpenOrCreate);
			Write(fs);

			fs.Close();
			fs.Dispose();
		}

		public static Settings Read(FileStream input)
		{
			bool bVersionNumber = false,
					bLanguage = false,
					bDistanceUnit = false,
					bDistancePrecision = false,
					bHeightUnit = false,
					bHeightPrecision = false,
					bSpeedUnit = false,
					bSpeedPrecision = false,
					bDSpeedPrecision = false,
					bAnglePrecision = false,
					bGradientPrecision = false;

			Parser parser;
			Settings result = new Settings();
			parser = new Parser(input, 1);

			int token;
			while ((token = parser.NextToken()) != BaseToken.EOF)
			{
				//> BaseToken.NONE
				if (token >= CfgToken.VersionNumber && token < CfgToken.EQUAL)
				{
					int eq = parser.NextToken();
					if (eq != CfgToken.EQUAL)
						throw new Exception(string.Format("Invalid symbol \"{0}\" in line {1} of config file. \"=\" expected.",
							parser.StrValue, parser.Line));

					int val = parser.NextToken();
					if (val != BaseToken.NUMBER)
						throw new Exception(string.Format("Invalid symbol \"{0}\" in line {1} of config file. Number expected", parser.StrValue, parser.Line));

					switch (token)
					{
						case CfgToken.VersionNumber:
							if (bVersionNumber)
								throw new Exception(string.Format("Multiple \"VersionNumber\" definition in line {0} of config file", parser.Line));
							if (parser.valueType!= Utilities.ValueType.Int)
								throw new Exception(string.Format("Invalid symbol \"{0}\" in line {1} of config file. Integer number expected", parser.StrValue, parser.Line));
							if (result.VersionNumber != parser.IntValue)
								throw new Exception(string.Format("Invalid \"VersionNumber\" in line {0} of config file", parser.Line));
							bVersionNumber = true;
							break;
						case CfgToken.Language:
							if (bLanguage)
								throw new Exception(string.Format("Multiple \"Language\" definition in line {0} of config file", parser.Line));
							if (parser.valueType!= Utilities.ValueType.Int)
								throw new Exception(string.Format("Invalid symbol \"{0}\" in line {1} of config file. Integer number expected", parser.StrValue, parser.Line));
							result._language = parser.IntValue;
							bLanguage = true;
							break;
						case CfgToken.DistanceUnit:
							if (bDistanceUnit)
								throw new Exception(string.Format("Multiple \"DistanceUnit\" definition in line {0} of config file", parser.Line));
							if (parser.valueType!= Utilities.ValueType.Int)
								throw new Exception(string.Format("Invalid symbol \"{0}\" in line {1} of config file. Integer number expected", parser.StrValue, parser.Line));
							result._distanceUnit = (HorizantalDistanceType)parser.IntValue;
							bDistanceUnit = true;
							break;
						case CfgToken.DistancePrecision:
							if (bDistancePrecision)
								throw new Exception(string.Format("Multiple \"DistancePrecision\" definition in line {0} of config file", parser.Line));
							result.DistancePrecision = parser.DoubleValue;
							bDistancePrecision = true;
							break;
						case CfgToken.HeightUnit:
							if (bHeightUnit)
								throw new Exception(string.Format("Multiple \"HeightUnit\" definition in line {0} of config file", parser.Line));
							if (parser.valueType!= Utilities.ValueType.Int)
								throw new Exception(string.Format("Invalid symbol \"{0}\" in line {1} of config file. Integer number expected", parser.StrValue, parser.Line));
							result._heightUnit = (VerticalDistanceType)parser.IntValue;
							bHeightUnit = true;
							break;
						case CfgToken.HeightPrecision:
							if (bHeightPrecision)
								throw new Exception(string.Format("Multiple \"HeightPrecision\" definition in line {0} of config file", parser.Line));
							result.HeightPrecision = parser.DoubleValue;
							bHeightPrecision = true;
							break;
						case CfgToken.SpeedUnit:
							if (bSpeedUnit)
								throw new Exception(string.Format("Multiple \"SpeedUnit\" definition in line {0} of config file", parser.Line));
							if (parser.valueType!= Utilities.ValueType.Int)
								throw new Exception(string.Format("Invalid symbol \"{0}\" in line {1} of config file. Integer number expected", parser.StrValue, parser.Line));
							result._speedUnit = (HorizantalSpeedType)parser.IntValue;
							bSpeedUnit = true;
							break;
						case CfgToken.SpeedPrecision:
							if (bSpeedPrecision)
								throw new Exception(string.Format("Multiple \"SpeedPrecision\" definition in line {0} of config file", parser.Line));
							result.SpeedPrecision = parser.DoubleValue;
							bSpeedPrecision = true;
							break;
						case CfgToken.DSpeedPrecision:
							if (bDSpeedPrecision)
								throw new Exception(string.Format("Multiple \"DSpeedPrecision\" definition in line {0} of config file", parser.Line));
							result.DSpeedPrecision = parser.DoubleValue;
							bDSpeedPrecision = true;
							break;
						case CfgToken.AnglePrecision:
							if (bAnglePrecision)
								throw new Exception(string.Format("Multiple \"AnglePrecision\" definition in line {0} of config file", parser.Line));
							result.AnglePrecision = parser.DoubleValue;
							bAnglePrecision = true;
							break;
						case CfgToken.GradientPrecision:
							if (bGradientPrecision)
								throw new Exception(string.Format("Multiple \"GradientPrecision\" definition in line {0} of config file", parser.Line));
							result.GradientPrecision = parser.DoubleValue;
							bGradientPrecision = true;
							break;
					}
				}
				else
					throw new Exception(string.Format("Invalid symbol \"{0}\" in line {1} of config file", parser.StrValue, parser.Line));
			}

			return result;
		}

		public static Settings Read(string FileName)
		{
			FileStream fs = new System.IO.FileStream(FileName, FileMode.OpenOrCreate);
			try
			{
				Settings result = Read(fs);
				return result;
			}
			finally
			{
				fs.Close();
				fs.Dispose();
			}
		}

		protected int _language;
		protected HorizantalDistanceType _distanceUnit;
		protected VerticalDistanceType _heightUnit;
		protected HorizantalSpeedType _speedUnit;

		protected double _distancePrecision;
		protected double _heightPrecision;
		protected double _speedPrecision;
		protected double _dSpeedPrecision;
		protected double _anglePrecision;
		protected double _gradientPrecision;
	}
}


