using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Globalization;
using CDOTMA;

namespace Converters
{
	public class UnitConverter
	{
		public UnitConverter(Settings settings)
		{
			_settings = settings;

			//Thread.CurrentThread.CurrentUICulture = new CultureInfo(settings.Language);
			//Resources.Resource.Culture = Thread.CurrentThread.CurrentUICulture;

			//========================================================================

			DistanceUnitIndex = (int)settings.DistanceUnit;
			HeightUnitIndex = (int)settings.HeightUnit;
			SpeedUnitIndex = (int)settings.SpeedUnit;
			DSpeedUnitIndex = HeightUnitIndex;

			DistancePrecision = settings.DistancePrecision;
			HeightPrecision = settings.HeightPrecision;
			SpeedPrecision = settings.SpeedPrecision;
			DSpeedPrecision = settings.DSpeedPrecision;

			//"км" '"kM"
			DistanceConverter[0].Multiplier = 0.001;
			DistanceConverter[0].Rounding = DistancePrecision;
			DistanceConverter[0].Unit = "Km";			// Resources.Resource.DistanceUnitKm;

			//"ММ" '"NM"
			DistanceConverter[1].Multiplier = 1.0 / 1852.0;
			DistanceConverter[1].Rounding = DistancePrecision;
			DistanceConverter[1].Unit = "NM";			// Resources.Resource.DistanceUnitNM;

			//"meter"
			HeightConverter[0].Multiplier = 1.0;
			HeightConverter[0].Rounding = HeightPrecision;
			HeightConverter[0].Unit = "meter";			// Resources.Resource.HeightUnitM;

			//"фт" '"feet"
			HeightConverter[1].Multiplier = 1.0 / 0.3048;
			HeightConverter[1].Rounding = HeightPrecision;
			HeightConverter[1].Unit = "feet";			// Resources.Resource.HeightUnitFt;

			//"км/ч" '"km/h"
			SpeedConverter[0].Multiplier = 3.6;
			SpeedConverter[0].Rounding = SpeedPrecision;
			SpeedConverter[0].Unit = "Km/h";			// Resources.Resource.SpeedUnitKm_H;

			//"узлы" '"Kt"
			SpeedConverter[1].Multiplier = 3.6 / 1.852;
			SpeedConverter[1].Rounding = SpeedPrecision;
			SpeedConverter[1].Unit = "Kt";				// Resources.Resource.SpeedUnitKt;

			DSpeedConverter[0].Multiplier = 60;
			DSpeedConverter[0].Rounding = DSpeedPrecision;
			DSpeedConverter[0].Unit = "m/min";			// Resources.Resource.DSpeedUnitM_Min;

			DSpeedConverter[1].Multiplier = 60.0 / 0.3048;
			DSpeedConverter[1].Rounding = DSpeedPrecision;
			DSpeedConverter[1].Unit = "feet/min";		// Resources.Resource.DSpeedUnitFt_Min;
		}

		public double DistanceToDisplayUnits(double value, eRoundMode roundMode = eRoundMode.NERAEST)
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
				case eRoundMode.NERAEST:
					return System.Math.Round(value * DistanceConverter[DistanceUnitIndex].Multiplier / DistanceConverter[DistanceUnitIndex].Rounding) * DistanceConverter[DistanceUnitIndex].Rounding;
				case eRoundMode.CEIL:
					return System.Math.Round(value * DistanceConverter[DistanceUnitIndex].Multiplier / DistanceConverter[DistanceUnitIndex].Rounding + 0.4999) * DistanceConverter[DistanceUnitIndex].Rounding;
			}
			return value;
		}

		public double HeightToDisplayUnits(double value, eRoundMode roundMode = eRoundMode.NERAEST)
		{
			if (roundMode < eRoundMode.NONE || roundMode > eRoundMode.SPECIAL_CEIL)
				roundMode = eRoundMode.NONE;

			switch (roundMode)
			{
				case eRoundMode.NONE:
					return value * HeightConverter[HeightUnitIndex].Multiplier;
				case eRoundMode.FLOOR:
					return System.Math.Round(value * HeightConverter[HeightUnitIndex].Multiplier / HeightConverter[HeightUnitIndex].Rounding - 0.4999) * HeightConverter[HeightUnitIndex].Rounding;
				case eRoundMode.NERAEST:
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
						case eRoundMode.SPECIAL_NERAEST:
							return System.Math.Round(value * HeightConverter[HeightUnitIndex].Multiplier / roundingFactor) * roundingFactor;
						case eRoundMode.SPECIAL_CEIL:
							return System.Math.Round(value * HeightConverter[HeightUnitIndex].Multiplier / roundingFactor + 0.4999) * roundingFactor;
					}

					return value;
			}
		}

		public double SpeedToDisplayUnits(double value, eRoundMode roundMode = eRoundMode.NERAEST)
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
				case eRoundMode.NERAEST:
					return System.Math.Round(value * SpeedConverter[SpeedUnitIndex].Multiplier / SpeedConverter[SpeedUnitIndex].Rounding) * SpeedConverter[SpeedUnitIndex].Rounding;
				case eRoundMode.CEIL:
					return System.Math.Round(value * SpeedConverter[SpeedUnitIndex].Multiplier / SpeedConverter[SpeedUnitIndex].Rounding + 0.4999) * SpeedConverter[SpeedUnitIndex].Rounding;
				case eRoundMode.SPECIAL_FLOOR:
					return System.Math.Round(value * SpeedConverter[SpeedUnitIndex].Multiplier / roundingFactor - 0.4999) * roundingFactor;
				case eRoundMode.SPECIAL_NERAEST:
					return System.Math.Round(value * SpeedConverter[SpeedUnitIndex].Multiplier / roundingFactor) * roundingFactor;
				case eRoundMode.SPECIAL_CEIL:
					return System.Math.Round(value * SpeedConverter[SpeedUnitIndex].Multiplier / roundingFactor + 0.4999) * roundingFactor;
			}

			return value;
		}

		public double GradientToDisplayUnits(double value, eRoundMode roundMode = eRoundMode.NERAEST)
		{
			if (roundMode < eRoundMode.NONE || roundMode > eRoundMode.CEIL)
				roundMode = eRoundMode.NONE;

			switch (roundMode)
			{
				case eRoundMode.NONE:
					return value * 100.0;
				case eRoundMode.FLOOR:
					return System.Math.Round(value * 1000.0 - 0.4999) * 0.1;
				case eRoundMode.NERAEST:
					return System.Math.Round(value * 1000.0) * 0.1;
				case eRoundMode.CEIL:
					return System.Math.Round(value * 1000.0 + 0.4999) * 0.1;
			}

			return value;
		}

		public double DistanceToKm(double value, eRoundMode roundMode = eRoundMode.NERAEST)
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
				case eRoundMode.NERAEST:
					return System.Math.Round(value / DistanceConverter[DistanceUnitIndex].Rounding) * DistanceConverter[DistanceUnitIndex].Rounding;
				case eRoundMode.CEIL:
					return System.Math.Round(value / DistanceConverter[DistanceUnitIndex].Rounding + 0.4999) * DistanceConverter[DistanceUnitIndex].Rounding;
			}
			return value;
		}

		public double DistanceToNM(double value, eRoundMode roundMode = eRoundMode.NERAEST)
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
				case eRoundMode.NERAEST:
					return System.Math.Round(value / DistanceConverter[DistanceUnitIndex].Rounding) * DistanceConverter[DistanceUnitIndex].Rounding;
				case eRoundMode.CEIL:
					return System.Math.Round(value / DistanceConverter[DistanceUnitIndex].Rounding + 0.4999) * DistanceConverter[DistanceUnitIndex].Rounding;
			}
			return value;
		}

		public double HeightToM(double value, eRoundMode roundMode = eRoundMode.NERAEST)
		{
			if (roundMode < eRoundMode.NONE || roundMode > eRoundMode.SPECIAL_CEIL)
				roundMode = eRoundMode.NONE;

			switch (roundMode)
			{
				case eRoundMode.NONE:
					return value;
				case eRoundMode.FLOOR:
					return System.Math.Round(value / HeightConverter[HeightUnitIndex].Rounding - 0.4999) * HeightConverter[HeightUnitIndex].Rounding;
				case eRoundMode.NERAEST:
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
						case eRoundMode.SPECIAL_NERAEST:
							return System.Math.Round(value / roundingFactor) * roundingFactor;
						case eRoundMode.SPECIAL_CEIL:
							return System.Math.Round(value / roundingFactor + 0.4999) * roundingFactor;
					}

					return value;
			}
		}

		public double HeightToFt(double value, eRoundMode roundMode = eRoundMode.NERAEST)
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
				case eRoundMode.NERAEST:
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
						case eRoundMode.SPECIAL_NERAEST:
							return System.Math.Round(value / roundingFactor) * roundingFactor;
						case eRoundMode.SPECIAL_CEIL:
							return System.Math.Round(value / roundingFactor + 0.4999) * roundingFactor;
					}

					return value;
			}
		}

		public double SpeedToKmpH(double value, eRoundMode roundMode = eRoundMode.NERAEST)
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
				case eRoundMode.NERAEST:
					return System.Math.Round(value / SpeedConverter[SpeedUnitIndex].Rounding) * SpeedConverter[SpeedUnitIndex].Rounding;
				case eRoundMode.CEIL:
					return System.Math.Round(value / SpeedConverter[SpeedUnitIndex].Rounding + 0.4999) * SpeedConverter[SpeedUnitIndex].Rounding;
				case eRoundMode.SPECIAL_FLOOR:
					return System.Math.Round(value / roundingFactor - 0.4999) * roundingFactor;
				case eRoundMode.SPECIAL_NERAEST:
					return System.Math.Round(value / roundingFactor) * roundingFactor;
				case eRoundMode.SPECIAL_CEIL:
					return System.Math.Round(value / roundingFactor + 0.4999) * roundingFactor;
			}

			return value;
		}

		public double SpeedToKt(double value, eRoundMode roundMode = eRoundMode.NERAEST)
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
				case eRoundMode.NERAEST:
					return System.Math.Round(value / SpeedConverter[SpeedUnitIndex].Rounding) * SpeedConverter[SpeedUnitIndex].Rounding;
				case eRoundMode.CEIL:
					return System.Math.Round(value / SpeedConverter[SpeedUnitIndex].Rounding + 0.4999) * SpeedConverter[SpeedUnitIndex].Rounding;
				case eRoundMode.SPECIAL_FLOOR:
					return System.Math.Round(value / roundingFactor - 0.4999) * roundingFactor;
				case eRoundMode.SPECIAL_NERAEST:
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

		public double HeightToInternalUnits(double value)
		{
			return value / HeightConverter[HeightUnitIndex].Multiplier;
		}

		public double SpeedToInternalUnits(double value)
		{
			return value / SpeedConverter[SpeedUnitIndex].Multiplier;
		}

		public double GradientToInternalUnits(double value)
		{
			return 0.01 * value;
		}

		#region UnitIndices

		public int DistanceUnitIndex
		{
			get;
			private set;
		}

		public int HeightUnitIndex
		{
			get;
			private set;
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
				return "Km";	// Resources.Resource.DistanceUnitKm;
			}
		}

		public string DistanceUnitNM
		{
			get
			{
				return "NM";	// Resources.Resource.DistanceUnitNM;
			}
		}

		public string HeightUnitM
		{
			get
			{
				return "meter";	// Resources.Resource.HeightUnitM;
			}
		}

		public string HeightUnitFt
		{
			get
			{
				return "feet";	// Resources.Resource.HeightUnitFt;
			}
		}

		public string SpeedUnitKm_H
		{
			get
			{
				return "Km/h";	// Resources.Resource.SpeedUnitKm_H;
			}
		}

		public string SpeedUnitKt
		{
			get
			{
				return "Kt";	// Resources.Resource.SpeedUnitKt;
			}
		}

		public string DSpeedUnitM_Min
		{
			get
			{
				return "meter/min";	// Resources.Resource.DSpeedUnitM_Min;
			}
		}

		public string DSpeedUnitFt_Min
		{
			get
			{
				return "feet/min";	// Resources.Resource.DSpeedUnitFt_Min;
			}
		}

		#endregion

		public Settings GetSettings
		{
			get { return _settings; }
		}

		private TypeConvert[] DistanceConverter = new TypeConvert[2];
		private TypeConvert[] HeightConverter = new TypeConvert[2];
		private TypeConvert[] SpeedConverter = new TypeConvert[2];
		private TypeConvert[] DSpeedConverter = new TypeConvert[2];
		private Settings _settings;

		private struct TypeConvert
		{
			public double Multiplier;
			public double Rounding;
			public string Unit;
		}
	}
}
