using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Globalization;

namespace Aran.PANDA.Common
{
	public class UnitConverter
	{
		public UnitConverter(Settings settings)
		{
			//#warning Language Thread

			_settings = settings;

			Thread.CurrentThread.CurrentUICulture = new CultureInfo(_settings.Language);
			Resources.Resource.Culture = Thread.CurrentThread.CurrentUICulture;

			//========================================================================

			var uiInterfaceData = settings.UIIntefaceData;
			DistanceUnitIndex = (int)uiInterfaceData.DistanceUnit;
			HeightUnitIndex = (int)uiInterfaceData.HeightUnit;
			SpeedUnitIndex = (int)uiInterfaceData.SpeedUnit;
			DSpeedUnitIndex = HeightUnitIndex;

			DistancePrecision = uiInterfaceData.DistancePrecision;
			HeightPrecision = uiInterfaceData.HeightPrecision;
			SpeedPrecision = uiInterfaceData.SpeedPrecision;
			DSpeedPrecision = uiInterfaceData.DSpeedPrecision;
			GradientPrecision = uiInterfaceData.GradientPrecision;
			AnglePrecision = uiInterfaceData.AnglePrecision;


			//"км" '"kM"
			DistanceConverter = new TypeConvert[2];
			DistanceConverter[0] = new TypeConvert
			{
				Multiplier = 0.001,
				Rounding = DistancePrecision,
				Unit = Resources.Resource.DistanceUnitKm
			};

			//"ММ" '"NM"
			DistanceConverter[1] = new TypeConvert
			{
				Multiplier = 1.0 / 1852.0,
				Rounding = DistancePrecision,
				Unit = Resources.Resource.DistanceUnitNM
			};

			//"meter"
			HeightConverter = new TypeConvert[2];
			HeightConverter[0] = new TypeConvert
			{
				Multiplier = 1.0,
				Rounding = HeightPrecision,
				Unit = Resources.Resource.HeightUnitM
			};

			//"фт" '"feet"
			HeightConverter[1] = new TypeConvert
			{
				Multiplier = 1.0 / 0.3048,
				Rounding = HeightPrecision,
				Unit = Resources.Resource.HeightUnitFt
			};

			//"км/ч" '"km/h"
			SpeedConverter = new TypeConvert[2];
			SpeedConverter[0] = new TypeConvert
			{
				Multiplier = 3.6,
				Rounding = SpeedPrecision,
				Unit = Resources.Resource.SpeedUnitKm_H
			};

			//"узлы" '"Kt"
			SpeedConverter[1] = new TypeConvert
			{
				Multiplier = 3.6 / 1.852,
				Rounding = SpeedPrecision,
				Unit = Resources.Resource.SpeedUnitKt
			};

			DSpeedConverter = new TypeConvert[2];
			DSpeedConverter[0] = new TypeConvert
			{
				Multiplier = 60,
				Rounding = DSpeedPrecision,
				Unit = Resources.Resource.DSpeedUnitM_Min
			};

			DSpeedConverter[1] = new TypeConvert
			{
				Multiplier = 60.0 / 0.3048,
				Rounding = DSpeedPrecision,
				Unit = Resources.Resource.DSpeedUnitFt_Min
			};

			GradientConverter = new TypeConvert[1];
			GradientConverter[0] = new TypeConvert
			{
				Multiplier = 100.0,
				Rounding = GradientPrecision,
				Unit = "%"
			};

			AngleConverter = new TypeConvert[1];
			AngleConverter[0] = new TypeConvert
			{
				Multiplier = 180.0 / Math.PI,
				Rounding = AnglePrecision,
				Unit = "°"
			};

			AzimuthConverter = new TypeConvert[1];
			AzimuthConverter[0] = new TypeConvert
			{
				Multiplier = 1.0,
				Rounding = AnglePrecision,
				Unit = "°"
			};
		}

		public double DistanceToDisplayUnits(double value, eRoundMode roundMode = eRoundMode.NEAREST)
		{
			if (roundMode < eRoundMode.NONE || roundMode > eRoundMode.CEIL)
				roundMode = eRoundMode.NONE;

			switch (roundMode)
			{
				case eRoundMode.NONE:
					return value * DistanceConverter[DistanceUnitIndex].Multiplier;
				case eRoundMode.FLOOR:
					return System.Math.Round(value * DistanceConverter[DistanceUnitIndex].Multiplier / DistanceConverter[DistanceUnitIndex].Rounding - 0.4999) *
							DistanceConverter[DistanceUnitIndex].Rounding;
				case eRoundMode.NEAREST:
					return System.Math.Round(value * DistanceConverter[DistanceUnitIndex].Multiplier / DistanceConverter[DistanceUnitIndex].Rounding) * DistanceConverter[DistanceUnitIndex].Rounding;
				case eRoundMode.CEIL:
					return System.Math.Round(value * DistanceConverter[DistanceUnitIndex].Multiplier / DistanceConverter[DistanceUnitIndex].Rounding + 0.4999) * DistanceConverter[DistanceUnitIndex].Rounding;
			}
			return value;
		}

		public double HeightToDisplayUnits(double value, eRoundMode roundMode = eRoundMode.NEAREST)
		{
			if (roundMode < eRoundMode.NONE || roundMode > eRoundMode.SPECIAL_CEIL)
				roundMode = eRoundMode.NONE;

			switch (roundMode)
			{
				case eRoundMode.NONE:
					return value * HeightConverter[HeightUnitIndex].Multiplier;
				case eRoundMode.FLOOR:
					return System.Math.Round(value * HeightConverter[HeightUnitIndex].Multiplier / HeightConverter[HeightUnitIndex].Rounding - 0.4999) * HeightConverter[HeightUnitIndex].Rounding;
				case eRoundMode.NEAREST:
					return System.Math.Round(value * HeightConverter[HeightUnitIndex].Multiplier / HeightConverter[HeightUnitIndex].Rounding) * HeightConverter[HeightUnitIndex].Rounding;
				case eRoundMode.CEIL:
					return System.Math.Round(value * HeightConverter[HeightUnitIndex].Multiplier / HeightConverter[HeightUnitIndex].Rounding + 0.4999) * HeightConverter[HeightUnitIndex].Rounding;
				default:
					if (HeightUnitIndex > 1)
						return System.Math.Round(value * HeightConverter[HeightUnitIndex].Multiplier / HeightConverter[HeightUnitIndex].Rounding) * HeightConverter[HeightUnitIndex].Rounding;

					double roundingFactor = 50.0 + 50.0 * HeightUnitIndex;

					switch (roundMode)
					{
						case eRoundMode.SPECIAL_FLOOR:
							return System.Math.Round(value * HeightConverter[HeightUnitIndex].Multiplier / roundingFactor - 0.4999) * roundingFactor;
						case eRoundMode.SPECIAL_NEAREST:
							return System.Math.Round(value * HeightConverter[HeightUnitIndex].Multiplier / roundingFactor) * roundingFactor;
						case eRoundMode.SPECIAL_CEIL:
							return System.Math.Round(value * HeightConverter[HeightUnitIndex].Multiplier / roundingFactor + 0.4999) * roundingFactor;
					}

					return value;
			}
		}

		public double SpeedToDisplayUnits(double value, eRoundMode roundMode = eRoundMode.NEAREST)
		{
			if (roundMode < eRoundMode.NONE || roundMode > eRoundMode.SPECIAL_CEIL)
				roundMode = eRoundMode.NONE;

			double roundingFactor = 5.0;

			switch (roundMode)
			{
				case eRoundMode.NONE:
					return value * SpeedConverter[SpeedUnitIndex].Multiplier;
				case eRoundMode.FLOOR:
					return System.Math.Round(value * SpeedConverter[SpeedUnitIndex].Multiplier / SpeedConverter[SpeedUnitIndex].Rounding - 0.4999) * SpeedConverter[SpeedUnitIndex].Rounding;
				case eRoundMode.NEAREST:
					return System.Math.Round(value * SpeedConverter[SpeedUnitIndex].Multiplier / SpeedConverter[SpeedUnitIndex].Rounding) * SpeedConverter[SpeedUnitIndex].Rounding;
				case eRoundMode.CEIL:
					return System.Math.Round(value * SpeedConverter[SpeedUnitIndex].Multiplier / SpeedConverter[SpeedUnitIndex].Rounding + 0.4999) * SpeedConverter[SpeedUnitIndex].Rounding;
				case eRoundMode.SPECIAL_FLOOR:
					return System.Math.Round(value * SpeedConverter[SpeedUnitIndex].Multiplier / roundingFactor - 0.4999) * roundingFactor;
				case eRoundMode.SPECIAL_NEAREST:
					return System.Math.Round(value * SpeedConverter[SpeedUnitIndex].Multiplier / roundingFactor) * roundingFactor;
				case eRoundMode.SPECIAL_CEIL:
					return System.Math.Round(value * SpeedConverter[SpeedUnitIndex].Multiplier / roundingFactor + 0.4999) * roundingFactor;
			}

			return value;
		}

		public double GradientToDisplayUnits(double value, eRoundMode roundMode = eRoundMode.NEAREST)
		{
			//if (roundMode < eRoundMode.NONE || roundMode > eRoundMode.CEIL)				roundMode = eRoundMode.NONE;

			switch (roundMode)
			{
				case eRoundMode.FLOOR:
					return System.Math.Round(value * GradientConverter[0].Multiplier / GradientConverter[0].Rounding - 0.4999) * GradientConverter[0].Rounding;
				case eRoundMode.NEAREST:
					return System.Math.Round(value * GradientConverter[0].Multiplier / GradientConverter[0].Rounding) * GradientConverter[0].Rounding;
				case eRoundMode.CEIL:
					return System.Math.Round(value * GradientConverter[0].Multiplier / GradientConverter[0].Rounding + 0.4999) * GradientConverter[0].Rounding;
				case eRoundMode.SPECIAL_NEAREST:
					return System.Math.Round(value * GradientConverter[0].Multiplier / GradientConverter[0].Rounding) * GradientConverter[0].Rounding;
			}

			return value * GradientConverter[0].Multiplier;
		}

		public double AngleToDisplayUnits(double value, eRoundMode roundMode = eRoundMode.NEAREST)
		{
			//if (roundMode < eRoundMode.NONE || roundMode > eRoundMode.CEIL)				roundMode = eRoundMode.NONE;

			switch (roundMode)
			{
				case eRoundMode.FLOOR:
					return System.Math.Round(value * AngleConverter[0].Multiplier / AngleConverter[0].Rounding - 0.4999) * AngleConverter[0].Rounding;
				case eRoundMode.NEAREST:
					return System.Math.Round(value * AngleConverter[0].Multiplier / AngleConverter[0].Rounding) * AngleConverter[0].Rounding;
				case eRoundMode.CEIL:
					return System.Math.Round(value * AngleConverter[0].Multiplier / AngleConverter[0].Rounding + 0.4999) * AngleConverter[0].Rounding;
				case eRoundMode.SPECIAL_NEAREST:
					return System.Math.Round(value * AngleConverter[0].Multiplier / AngleConverter[0].Rounding) * AngleConverter[0].Rounding;
			}

			return value * AngleConverter[0].Multiplier;
		}

		public double AzimuthToDisplayUnits(double value, eRoundMode roundMode = eRoundMode.NEAREST)
		{
			//if (roundMode < eRoundMode.NONE || roundMode > eRoundMode.CEIL)				roundMode = eRoundMode.NONE;

			switch (roundMode)
			{
				case eRoundMode.FLOOR:
					return System.Math.Round(value / AngleConverter[0].Rounding - 0.4999) * AngleConverter[0].Rounding;
				case eRoundMode.NEAREST:
					return System.Math.Round(value / AngleConverter[0].Rounding) * AngleConverter[0].Rounding;
				case eRoundMode.CEIL:
					return System.Math.Round(value / AngleConverter[0].Rounding + 0.4999) * AngleConverter[0].Rounding;
				case eRoundMode.SPECIAL_NEAREST:
					return System.Math.Round(value / AngleConverter[0].Rounding) * AngleConverter[0].Rounding;
			}

			return value;
		}

		public double DistanceToKm(double value, eRoundMode roundMode = eRoundMode.NEAREST)
		{
			if (roundMode < eRoundMode.NONE || roundMode > eRoundMode.CEIL)
				roundMode = eRoundMode.NONE;
			value *= 0.001;

			switch (roundMode)
			{
				case eRoundMode.NONE:
					return value;
				case eRoundMode.FLOOR:
					return System.Math.Round(value / DistanceConverter[DistanceUnitIndex].Rounding - 0.4999) * DistanceConverter[DistanceUnitIndex].Rounding;
				case eRoundMode.NEAREST:
					return System.Math.Round(value / DistanceConverter[DistanceUnitIndex].Rounding) * DistanceConverter[DistanceUnitIndex].Rounding;
				case eRoundMode.CEIL:
					return System.Math.Round(value / DistanceConverter[DistanceUnitIndex].Rounding + 0.4999) * DistanceConverter[DistanceUnitIndex].Rounding;
			}
			return value;
		}

		public double DistanceToNM(double value, eRoundMode roundMode = eRoundMode.NEAREST)
		{
			if (roundMode < eRoundMode.NONE || roundMode > eRoundMode.CEIL)
				roundMode = eRoundMode.NONE;
			value *= 1.0 / 1852.0;

			switch (roundMode)
			{
				case eRoundMode.NONE:
					return value;
				case eRoundMode.FLOOR:
					return System.Math.Round(value / DistanceConverter[DistanceUnitIndex].Rounding - 0.4999) * DistanceConverter[DistanceUnitIndex].Rounding;
				case eRoundMode.NEAREST:
					return System.Math.Round(value / DistanceConverter[DistanceUnitIndex].Rounding) * DistanceConverter[DistanceUnitIndex].Rounding;
				case eRoundMode.CEIL:
					return System.Math.Round(value / DistanceConverter[DistanceUnitIndex].Rounding + 0.4999) * DistanceConverter[DistanceUnitIndex].Rounding;
			}
			return value;
		}

		public double HeightToM(double value, eRoundMode roundMode = eRoundMode.NEAREST)
		{
			if (roundMode < eRoundMode.NONE || roundMode > eRoundMode.SPECIAL_CEIL)
				roundMode = eRoundMode.NONE;

			switch (roundMode)
			{
				case eRoundMode.NONE:
					return value;
				case eRoundMode.FLOOR:
					return System.Math.Round(value / HeightConverter[HeightUnitIndex].Rounding - 0.4999) * HeightConverter[HeightUnitIndex].Rounding;
				case eRoundMode.NEAREST:
					return System.Math.Round(value / HeightConverter[HeightUnitIndex].Rounding) * HeightConverter[HeightUnitIndex].Rounding;
				case eRoundMode.CEIL:
					return System.Math.Round(value / HeightConverter[HeightUnitIndex].Rounding + 0.4999) * HeightConverter[HeightUnitIndex].Rounding;
				default:
					if (HeightUnitIndex > 1)
						return System.Math.Round(value / HeightConverter[HeightUnitIndex].Rounding) * HeightConverter[HeightUnitIndex].Rounding;

					double roundingFactor = 50.0 + 50.0 * HeightUnitIndex;

					switch (roundMode)
					{
						case eRoundMode.SPECIAL_FLOOR:
							return System.Math.Round(value / roundingFactor - 0.4999) * roundingFactor;
						case eRoundMode.SPECIAL_NEAREST:
							return System.Math.Round(value / roundingFactor) * roundingFactor;
						case eRoundMode.SPECIAL_CEIL:
							return System.Math.Round(value / roundingFactor + 0.4999) * roundingFactor;
					}

					return value;
			}
		}

		public double HeightToFt(double value, eRoundMode roundMode = eRoundMode.NEAREST)
		{
			if (roundMode < eRoundMode.NONE || roundMode > eRoundMode.SPECIAL_CEIL)
				roundMode = eRoundMode.NONE;

			value *= 1.0 / 0.3048;
			switch (roundMode)
			{
				case eRoundMode.NONE:
					return value;
				case eRoundMode.FLOOR:
					return System.Math.Round(value / HeightConverter[HeightUnitIndex].Rounding - 0.4999) * HeightConverter[HeightUnitIndex].Rounding;
				case eRoundMode.NEAREST:
					return System.Math.Round(value / HeightConverter[HeightUnitIndex].Rounding) * HeightConverter[HeightUnitIndex].Rounding;
				case eRoundMode.CEIL:
					return System.Math.Round(value / HeightConverter[HeightUnitIndex].Rounding + 0.4999) * HeightConverter[HeightUnitIndex].Rounding;
				default:
					if (HeightUnitIndex > 1)
						return System.Math.Round(value / HeightConverter[HeightUnitIndex].Rounding) * HeightConverter[HeightUnitIndex].Rounding;

					double roundingFactor = 50.0 + 50.0 * HeightUnitIndex;

					switch (roundMode)
					{
						case eRoundMode.SPECIAL_FLOOR:
							return System.Math.Round(value / roundingFactor - 0.4999) * roundingFactor;
						case eRoundMode.SPECIAL_NEAREST:
							return System.Math.Round(value / roundingFactor) * roundingFactor;
						case eRoundMode.SPECIAL_CEIL:
							return System.Math.Round(value / roundingFactor + 0.4999) * roundingFactor;
					}

					return value;
			}
		}

		public double SpeedToKmpH(double value, eRoundMode roundMode = eRoundMode.NEAREST)
		{
			if (roundMode < eRoundMode.NONE || roundMode > eRoundMode.SPECIAL_CEIL)
				roundMode = eRoundMode.NONE;
			value *= 3.6;

			double roundingFactor = 5.0;

			switch (roundMode)
			{
				case eRoundMode.NONE:
					return value;
				case eRoundMode.FLOOR:
					return System.Math.Round(value / SpeedConverter[SpeedUnitIndex].Rounding - 0.4999) * SpeedConverter[SpeedUnitIndex].Rounding;
				case eRoundMode.NEAREST:
					return System.Math.Round(value / SpeedConverter[SpeedUnitIndex].Rounding) * SpeedConverter[SpeedUnitIndex].Rounding;
				case eRoundMode.CEIL:
					return System.Math.Round(value / SpeedConverter[SpeedUnitIndex].Rounding + 0.4999) * SpeedConverter[SpeedUnitIndex].Rounding;
				case eRoundMode.SPECIAL_FLOOR:
					return System.Math.Round(value / roundingFactor - 0.4999) * roundingFactor;
				case eRoundMode.SPECIAL_NEAREST:
					return System.Math.Round(value / roundingFactor) * roundingFactor;
				case eRoundMode.SPECIAL_CEIL:
					return System.Math.Round(value / roundingFactor + 0.4999) * roundingFactor;
			}

			return value;
		}

		public double SpeedToKt(double value, eRoundMode roundMode = eRoundMode.NEAREST)
		{
			if (roundMode < eRoundMode.NONE || roundMode > eRoundMode.SPECIAL_CEIL)
				roundMode = eRoundMode.NONE;

			value *= 3.6 / 1.852;

			double roundingFactor = 5.0;

			switch (roundMode)
			{
				case eRoundMode.NONE:
					return value;
				case eRoundMode.FLOOR:
					return System.Math.Round(value / SpeedConverter[SpeedUnitIndex].Rounding - 0.4999) * SpeedConverter[SpeedUnitIndex].Rounding;
				case eRoundMode.NEAREST:
					return System.Math.Round(value / SpeedConverter[SpeedUnitIndex].Rounding) * SpeedConverter[SpeedUnitIndex].Rounding;
				case eRoundMode.CEIL:
					return System.Math.Round(value / SpeedConverter[SpeedUnitIndex].Rounding + 0.4999) * SpeedConverter[SpeedUnitIndex].Rounding;
				case eRoundMode.SPECIAL_FLOOR:
					return System.Math.Round(value / roundingFactor - 0.4999) * roundingFactor;
				case eRoundMode.SPECIAL_NEAREST:
					return System.Math.Round(value / roundingFactor) * roundingFactor;
				case eRoundMode.SPECIAL_CEIL:
					return System.Math.Round(value / roundingFactor + 0.4999) * roundingFactor;
			}

			return value;
		}

		public double DistanceToInternalUnits(double value)
		{
			return value / DistanceConverter[DistanceUnitIndex].Multiplier;
		}

		public double DistanceFromKm(double value)
		{
			return value * 1000.0;
		}

		public double DistanceFromNM(double value)
		{
			return value * 1852.0;
		}

		public double HeightToInternalUnits(double value)
		{
			return value / HeightConverter[HeightUnitIndex].Multiplier;
		}

		public double HeightFromFt(double value)
		{
			return value * 0.3048;
		}

		public double SpeedToInternalUnits(double value)
		{
			return value / SpeedConverter[SpeedUnitIndex].Multiplier;
		}

		public double GradientToInternalUnits(double value)
		{
			return 0.01 * value;
			//return value / GradientConverter[0].Multiplier;
		}

		public double AngleToInternalUnits(double value)
		{
			return Math.PI / 180.0 * value;
			//return value / AngleConverter[0].Multiplier;
		}

		public double AzimuthToInternalUnits(double value)
		{
			return value;
		}

		#region UnitIndices

		public int DistanceUnitIndex
		{
			get;
			set;
		}

		public int HeightUnitIndex
		{
			get;
			set;
		}

		public int SpeedUnitIndex
		{
			get;
			private set;
		}

		public int DSpeedUnitIndex
		{
			get;
			private set;
		}

		#endregion

		#region Precisions

		public double DistancePrecision
		{
			get;
			private set;
		}

		public double HeightPrecision
		{
			get;
			private set;
		}

		public double SpeedPrecision
		{
			get;
			private set;
		}

		public double DSpeedPrecision
		{
			get;
			private set;
		}

		public double GradientPrecision
		{
			get;
			private set;
		}

		public double AnglePrecision
		{
			get;
			private set;
		}
		#endregion

		#region Units

		public string DistanceUnit
		{
			get
			{
				return DistanceConverter[DistanceUnitIndex].Unit;
			}
		}

		public string HeightUnit
		{
			get
			{
				return HeightConverter[HeightUnitIndex].Unit;
			}
		}

		public string SpeedUnit
		{
			get
			{
				return SpeedConverter[SpeedUnitIndex].Unit;
			}
		}

		public string DSpeedUnit
		{
			get
			{
				return DSpeedConverter[DSpeedUnitIndex].Unit;
			}
		}

		public string DistanceUnitKm
		{
			get
			{
				return Resources.Resource.DistanceUnitKm;
			}
		}

		public string DistanceUnitNM
		{
			get
			{
				return Resources.Resource.DistanceUnitNM;
			}
		}

		public string HeightUnitM
		{
			get
			{
				return Resources.Resource.HeightUnitM;
			}
		}

		public string HeightUnitFt
		{
			get
			{
				return Resources.Resource.HeightUnitFt;
			}
		}

		public string SpeedUnitKm_H
		{
			get
			{
				return Resources.Resource.SpeedUnitKm_H;
			}
		}

		public string SpeedUnitKt
		{
			get
			{
				return Resources.Resource.SpeedUnitKt;
			}
		}

		public string DSpeedUnitM_Min
		{
			get
			{
				return Resources.Resource.DSpeedUnitM_Min;
			}
		}

		public string DSpeedUnitFt_Min
		{
			get
			{
				return Resources.Resource.DSpeedUnitFt_Min;
			}
		}

		public string AzimuthUnit
		{
			get { return "°"; }
		}

		public string GradientUnit
		{
			get { return "%"; }
		}

		#endregion

	    public Settings GetSettings {
	        get { return _settings; }
	    }

	    public TypeConvert[] DistanceConverter { get; set; }
	    public TypeConvert[] HeightConverter { get; set; }
	    public TypeConvert[] SpeedConverter { get; set; }
	    private TypeConvert[] DSpeedConverter { get; set; }
		private TypeConvert[] GradientConverter { get; set; }
		private TypeConvert[] AngleConverter { get; set; }
		private TypeConvert[] AzimuthConverter { get; set; }

		private Settings _settings;

	    public class TypeConvert
		{
	        public double Multiplier { get; set; }
			public double Rounding { get; set; }	//Precision
	        public string Unit { get; set; }
		}
	}
}


/*
─────────────────────────▄▀▄  
─────────────────────────█─█  
─────────────────────────█─█  
─────────────────────────█─█  
─────────────────────────█─█  
─────────────────────────█─█  
─────────────────────────█─▀█▀█▄  
─────────────────────────█──█──█  
─────────────────────────█▄▄█──▀█  
────────────────────────▄█──▄█▄─▀█  
────────────────────────█─▄█─█─█─█  
────────────────────────█──█─█─█─█  
────────────────────────█──█─█─█─█  
────▄█▄──▄█▄────────────█──▀▀█─█─█  
──▄█████████────────────▀█───█─█▄▀  
─▄███████████────────────██──▀▀─█  
▄█████████████────────────█─────█  
██████████───▀▀█▄─────────▀█────█  
████████───▀▀▀──█──────────█────█  
██████───────██─▀█─────────█────█  
████──▄──────────▀█────────█────█ Look dude,
███──█──────▀▀█───▀█───────█────█ a good code!
███─▀─██──────█────▀█──────█────█  
███─────────────────▀█─────█────█  
███──────────────────█─────█────█  
███─────────────▄▀───█─────█────█  
████─────────▄▄██────█▄────█────█  
████────────██████────█────█────█  
█████────█──███████▀──█───▄█▄▄▄▄█  
██▀▀██────▀─██▄──▄█───█───█─────█  
██▄──────────██████───█───█─────█  
─██▄────────────▄▄────█───█─────█  
─███████─────────────▄█───█─────█  
──██████─────────────█───█▀─────█  
──▄███████▄─────────▄█──█▀──────█  
─▄█─────▄▀▀▀█───────█───█───────█  
▄█────────█──█────▄███▀▀▀▀──────█  
█──▄▀▀────────█──▄▀──█──────────█  
█────█─────────█─────█──────────█  
█────────▀█────█─────█─────────██  
█───────────────█──▄█▀─────────█  
█──────────██───█▀▀▀───────────█  
█───────────────█──────────────█  
█▄─────────────██──────────────█  
─█▄────────────█───────────────█  
──██▄────────▄███▀▀▀▀▀▄────────█  
─█▀─▀█▄────────▀█──────▀▄──────█  
─█────▀▀▀▀▄─────█────────▀─────█
*/