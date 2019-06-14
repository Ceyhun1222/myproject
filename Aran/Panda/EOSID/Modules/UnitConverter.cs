using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EOSID
{

	public static class UnitConverter
	{
		const double GRDRounding = 0.001;

		public static double DistanceToDisplayUnits(double value, eRoundMode roundMode)
		{
			if (roundMode < eRoundMode.NONE || roundMode > eRoundMode.CEIL)//|| roundMode > eRoundMode.SPECIAL_CEIL)
				roundMode = eRoundMode.NONE;

			switch (roundMode)
			{
				case eRoundMode.NONE:
					return value * GlobalVars.DistanceConverter[GlobalVars.DistanceUnitIndex].Multiplier;
				case eRoundMode.FLOOR:
				case eRoundMode.SPECIAL_FLOOR:
					return System.Math.Round(value * GlobalVars.DistanceConverter[GlobalVars.DistanceUnitIndex].Multiplier / GlobalVars.DistanceConverter[GlobalVars.DistanceUnitIndex].Rounding - 0.4999) * GlobalVars.DistanceConverter[GlobalVars.DistanceUnitIndex].Rounding;
				case eRoundMode.NERAEST:
				case eRoundMode.SPECIAL_NERAEST:
					return System.Math.Round(value * GlobalVars.DistanceConverter[GlobalVars.DistanceUnitIndex].Multiplier / GlobalVars.DistanceConverter[GlobalVars.DistanceUnitIndex].Rounding) * GlobalVars.DistanceConverter[GlobalVars.DistanceUnitIndex].Rounding;
				case eRoundMode.CEIL:
				case eRoundMode.SPECIAL_CEIL:
					return System.Math.Round(value * GlobalVars.DistanceConverter[GlobalVars.DistanceUnitIndex].Multiplier / GlobalVars.DistanceConverter[GlobalVars.DistanceUnitIndex].Rounding + 0.4999) * GlobalVars.DistanceConverter[GlobalVars.DistanceUnitIndex].Rounding;
			}
			return value;
		}

		public static double HeightToDisplayUnits(double value, eRoundMode roundMode)
		{
			if (roundMode < eRoundMode.NONE || roundMode > eRoundMode.SPECIAL_CEIL)
				roundMode = eRoundMode.NONE;

			switch (roundMode)
			{
				case eRoundMode.NONE:
					return value * GlobalVars.HeightConverter[GlobalVars.HeightUnitIndex].Multiplier;
				case eRoundMode.FLOOR:
					return System.Math.Round(value * GlobalVars.HeightConverter[GlobalVars.HeightUnitIndex].Multiplier / GlobalVars.HeightConverter[GlobalVars.HeightUnitIndex].Rounding - 0.4999) * GlobalVars.HeightConverter[GlobalVars.HeightUnitIndex].Rounding;
				case eRoundMode.NERAEST:
					return System.Math.Round(value * GlobalVars.HeightConverter[GlobalVars.HeightUnitIndex].Multiplier / GlobalVars.HeightConverter[GlobalVars.HeightUnitIndex].Rounding) * GlobalVars.HeightConverter[GlobalVars.HeightUnitIndex].Rounding;
				case eRoundMode.CEIL:
					return System.Math.Round(value * GlobalVars.HeightConverter[GlobalVars.HeightUnitIndex].Multiplier / GlobalVars.HeightConverter[GlobalVars.HeightUnitIndex].Rounding + 0.4999) * GlobalVars.HeightConverter[GlobalVars.HeightUnitIndex].Rounding;
				default:
					if (GlobalVars.HeightUnitIndex > 1)
						return System.Math.Round(value * GlobalVars.HeightConverter[GlobalVars.HeightUnitIndex].Multiplier / GlobalVars.HeightConverter[GlobalVars.HeightUnitIndex].Rounding) * GlobalVars.HeightConverter[GlobalVars.HeightUnitIndex].Rounding;

					double roundingFactor = 50.0 + 50.0 * GlobalVars.HeightUnitIndex;

					switch (roundMode)
					{
						case eRoundMode.SPECIAL_FLOOR:
							return System.Math.Round(value * GlobalVars.HeightConverter[GlobalVars.HeightUnitIndex].Multiplier / roundingFactor - 0.4999) * roundingFactor;
						case eRoundMode.SPECIAL_NERAEST:
							return System.Math.Round(value * GlobalVars.HeightConverter[GlobalVars.HeightUnitIndex].Multiplier / roundingFactor) * roundingFactor;
						case eRoundMode.SPECIAL_CEIL:
							return System.Math.Round(value * GlobalVars.HeightConverter[GlobalVars.HeightUnitIndex].Multiplier / roundingFactor + 0.4999) * roundingFactor;
					}

					return value;
			}
		}

		public static double SpeedToDisplayUnits(double value, eRoundMode roundMode)
		{
			if (roundMode < eRoundMode.NONE || roundMode > eRoundMode.SPECIAL_CEIL)
				roundMode = eRoundMode.NONE;

			double roundingFactor = 5.0;

			switch (roundMode)
			{
				case eRoundMode.NONE:
					return value * GlobalVars.SpeedConverter[GlobalVars.SpeedUnitIndex].Multiplier;
				case eRoundMode.FLOOR:
					return System.Math.Round(value * GlobalVars.SpeedConverter[GlobalVars.SpeedUnitIndex].Multiplier / GlobalVars.SpeedConverter[GlobalVars.SpeedUnitIndex].Rounding - 0.4999) * GlobalVars.SpeedConverter[GlobalVars.SpeedUnitIndex].Rounding;
				case eRoundMode.NERAEST:
					return System.Math.Round(value * GlobalVars.SpeedConverter[GlobalVars.SpeedUnitIndex].Multiplier / GlobalVars.SpeedConverter[GlobalVars.SpeedUnitIndex].Rounding) * GlobalVars.SpeedConverter[GlobalVars.SpeedUnitIndex].Rounding;
				case eRoundMode.CEIL:
					return System.Math.Round(value * GlobalVars.SpeedConverter[GlobalVars.SpeedUnitIndex].Multiplier / GlobalVars.SpeedConverter[GlobalVars.SpeedUnitIndex].Rounding + 0.4999) * GlobalVars.SpeedConverter[GlobalVars.SpeedUnitIndex].Rounding;
				case eRoundMode.SPECIAL_FLOOR:
					return System.Math.Round(value * GlobalVars.SpeedConverter[GlobalVars.SpeedUnitIndex].Multiplier / roundingFactor - 0.4999) * roundingFactor;
				case eRoundMode.SPECIAL_NERAEST:
					return System.Math.Round(value * GlobalVars.SpeedConverter[GlobalVars.SpeedUnitIndex].Multiplier / roundingFactor) * roundingFactor;
				case eRoundMode.SPECIAL_CEIL:
					return System.Math.Round(value * GlobalVars.SpeedConverter[GlobalVars.SpeedUnitIndex].Multiplier / roundingFactor + 0.4999) * roundingFactor;
			}

			return value;
		}

		public static double GradientToDisplayUnits(double value, eRoundMode roundMode)
		{
			if (roundMode < eRoundMode.NONE || roundMode > eRoundMode.CEIL)	//|| roundMode > eRoundMode.SPECIAL_CEIL)
				roundMode = eRoundMode.NONE;
			value *= 100.0;
			switch (roundMode)
			{
				case eRoundMode.NONE:
					return value;
				case eRoundMode.FLOOR:
				case eRoundMode.SPECIAL_FLOOR:
					return System.Math.Round(value / GRDRounding - 0.4999) * GRDRounding;
				case eRoundMode.NERAEST:
				case eRoundMode.SPECIAL_NERAEST:
					return System.Math.Round(value / GRDRounding) * GRDRounding;
				case eRoundMode.CEIL:
				case eRoundMode.SPECIAL_CEIL:
					return System.Math.Round(value / GRDRounding + 0.4999) * GRDRounding;
			}
			return value;
		}

		public static double DistanceToKm(double value, eRoundMode roundMode)
		{
			if (roundMode < eRoundMode.NONE || roundMode > eRoundMode.CEIL)	//|| roundMode > eRoundMode.SPECIAL_CEIL)
				roundMode = eRoundMode.NONE;
			value *= 0.001;

			switch (roundMode)
			{
				case eRoundMode.NONE:
					return value;
				case eRoundMode.FLOOR:
				case eRoundMode.SPECIAL_FLOOR:
					return System.Math.Round(value / GlobalVars.DistanceConverter[GlobalVars.DistanceUnitIndex].Rounding - 0.4999) * GlobalVars.DistanceConverter[GlobalVars.DistanceUnitIndex].Rounding;
				case eRoundMode.NERAEST:
				case eRoundMode.SPECIAL_NERAEST:
					return System.Math.Round(value / GlobalVars.DistanceConverter[GlobalVars.DistanceUnitIndex].Rounding) * GlobalVars.DistanceConverter[GlobalVars.DistanceUnitIndex].Rounding;
				case eRoundMode.CEIL:
				case eRoundMode.SPECIAL_CEIL:
					return System.Math.Round(value / GlobalVars.DistanceConverter[GlobalVars.DistanceUnitIndex].Rounding + 0.4999) * GlobalVars.DistanceConverter[GlobalVars.DistanceUnitIndex].Rounding;
			}
			return value;
		}

		public static double DistanceToNM(double value, eRoundMode roundMode)
		{
			if (roundMode < eRoundMode.NONE || roundMode > eRoundMode.CEIL)	//|| roundMode > eRoundMode.SPECIAL_CEIL)
				roundMode = eRoundMode.NONE;
			value *= 1.0 / 1852.0;

			switch (roundMode)
			{
				case eRoundMode.NONE:
					return value;
				case eRoundMode.FLOOR:
				case eRoundMode.SPECIAL_FLOOR:
					return System.Math.Round(value / GlobalVars.DistanceConverter[GlobalVars.DistanceUnitIndex].Rounding - 0.4999) * GlobalVars.DistanceConverter[GlobalVars.DistanceUnitIndex].Rounding;
				case eRoundMode.NERAEST:
				case eRoundMode.SPECIAL_NERAEST:
					return System.Math.Round(value / GlobalVars.DistanceConverter[GlobalVars.DistanceUnitIndex].Rounding) * GlobalVars.DistanceConverter[GlobalVars.DistanceUnitIndex].Rounding;
				case eRoundMode.CEIL:
				case eRoundMode.SPECIAL_CEIL:
					return System.Math.Round(value / GlobalVars.DistanceConverter[GlobalVars.DistanceUnitIndex].Rounding + 0.4999) * GlobalVars.DistanceConverter[GlobalVars.DistanceUnitIndex].Rounding;
			}
			return value;
		}


		public static double HeightToM(double value, eRoundMode roundMode)
		{
			if (roundMode < eRoundMode.NONE || roundMode > eRoundMode.SPECIAL_CEIL)
				roundMode = eRoundMode.NONE;

			switch (roundMode)
			{
				case eRoundMode.NONE:
					return value;
				case eRoundMode.FLOOR:
					return System.Math.Round(value / GlobalVars.HeightConverter[GlobalVars.HeightUnitIndex].Rounding - 0.4999) * GlobalVars.HeightConverter[GlobalVars.HeightUnitIndex].Rounding;
				case eRoundMode.NERAEST:
					return System.Math.Round(value / GlobalVars.HeightConverter[GlobalVars.HeightUnitIndex].Rounding) * GlobalVars.HeightConverter[GlobalVars.HeightUnitIndex].Rounding;
				case eRoundMode.CEIL:
					return System.Math.Round(value / GlobalVars.HeightConverter[GlobalVars.HeightUnitIndex].Rounding + 0.4999) * GlobalVars.HeightConverter[GlobalVars.HeightUnitIndex].Rounding;
				default:
					if (GlobalVars.HeightUnitIndex > 1)
						return System.Math.Round(value / GlobalVars.HeightConverter[GlobalVars.HeightUnitIndex].Rounding) * GlobalVars.HeightConverter[GlobalVars.HeightUnitIndex].Rounding;

					double roundingFactor = 50.0 + 50.0 * GlobalVars.HeightUnitIndex;

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

		public static double HeightToFt(double value, eRoundMode roundMode)
		{
			if (roundMode < eRoundMode.NONE || roundMode > eRoundMode.SPECIAL_CEIL)
				roundMode = eRoundMode.NONE;

			value *= 1.0 / 0.3048;
			switch (roundMode)
			{
				case eRoundMode.NONE:
					return value;
				case eRoundMode.FLOOR:
					return System.Math.Round(value / GlobalVars.HeightConverter[GlobalVars.HeightUnitIndex].Rounding - 0.4999) * GlobalVars.HeightConverter[GlobalVars.HeightUnitIndex].Rounding;
				case eRoundMode.NERAEST:
					return System.Math.Round(value / GlobalVars.HeightConverter[GlobalVars.HeightUnitIndex].Rounding) * GlobalVars.HeightConverter[GlobalVars.HeightUnitIndex].Rounding;
				case eRoundMode.CEIL:
					return System.Math.Round(value / GlobalVars.HeightConverter[GlobalVars.HeightUnitIndex].Rounding + 0.4999) * GlobalVars.HeightConverter[GlobalVars.HeightUnitIndex].Rounding;
				default:
					if (GlobalVars.HeightUnitIndex > 1)
						return System.Math.Round(value / GlobalVars.HeightConverter[GlobalVars.HeightUnitIndex].Rounding) * GlobalVars.HeightConverter[GlobalVars.HeightUnitIndex].Rounding;

					double roundingFactor = 50.0 + 50.0 * GlobalVars.HeightUnitIndex;

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

		public static double SpeedToKmpH(double value, eRoundMode roundMode)
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
					return System.Math.Round(value / GlobalVars.SpeedConverter[GlobalVars.SpeedUnitIndex].Rounding - 0.4999) * GlobalVars.SpeedConverter[GlobalVars.SpeedUnitIndex].Rounding;
				case eRoundMode.NERAEST:
					return System.Math.Round(value / GlobalVars.SpeedConverter[GlobalVars.SpeedUnitIndex].Rounding) * GlobalVars.SpeedConverter[GlobalVars.SpeedUnitIndex].Rounding;
				case eRoundMode.CEIL:
					return System.Math.Round(value / GlobalVars.SpeedConverter[GlobalVars.SpeedUnitIndex].Rounding + 0.4999) * GlobalVars.SpeedConverter[GlobalVars.SpeedUnitIndex].Rounding;
				case eRoundMode.SPECIAL_FLOOR:
					return System.Math.Round(value / roundingFactor - 0.4999) * roundingFactor;
				case eRoundMode.SPECIAL_NERAEST:
					return System.Math.Round(value / roundingFactor) * roundingFactor;
				case eRoundMode.SPECIAL_CEIL:
					return System.Math.Round(value / roundingFactor + 0.4999) * roundingFactor;
			}

			return value;
		}

		public static double SpeedToKt(double value, eRoundMode roundMode)
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
					return System.Math.Round(value / GlobalVars.SpeedConverter[GlobalVars.SpeedUnitIndex].Rounding - 0.4999) * GlobalVars.SpeedConverter[GlobalVars.SpeedUnitIndex].Rounding;
				case eRoundMode.NERAEST:
					return System.Math.Round(value / GlobalVars.SpeedConverter[GlobalVars.SpeedUnitIndex].Rounding) * GlobalVars.SpeedConverter[GlobalVars.SpeedUnitIndex].Rounding;
				case eRoundMode.CEIL:
					return System.Math.Round(value / GlobalVars.SpeedConverter[GlobalVars.SpeedUnitIndex].Rounding + 0.4999) * GlobalVars.SpeedConverter[GlobalVars.SpeedUnitIndex].Rounding;
				case eRoundMode.SPECIAL_FLOOR:
					return System.Math.Round(value / roundingFactor - 0.4999) * roundingFactor;
				case eRoundMode.SPECIAL_NERAEST:
					return System.Math.Round(value / roundingFactor) * roundingFactor;
				case eRoundMode.SPECIAL_CEIL:
					return System.Math.Round(value / roundingFactor + 0.4999) * roundingFactor;
			}

			return value;
		}

		public static double DistanceToInternalUnits(double value)
		{
			return value / GlobalVars.DistanceConverter[GlobalVars.DistanceUnitIndex].Multiplier;
		}

		public static double HeightToInternalUnits(double value)
		{
			return value / GlobalVars.HeightConverter[GlobalVars.HeightUnitIndex].Multiplier;
		}

		public static double SpeedToInternalUnits(double value)
		{
			return value / GlobalVars.SpeedConverter[GlobalVars.SpeedUnitIndex].Multiplier;
		}

		public static double KmpHSpeedToInternalUnits(double value)
		{
			return value * (1 / 3.6);
		}

		public static double GradientToInternalUnits(double value)
		{
			return 0.01 * value;
		}

		public static string DistanceUnitKm
		{
			get
			{
				return "Км";	//Resources.Resource.DistanceUnitKm;
			}
		}

		public static string DistanceUnitNM
		{
			get
			{
				return "ММ";	// Resources.Resource.DistanceUnitNM;
			}
		}

		public static string DistanceUnit
		{
			get
			{
				return GlobalVars.DistanceConverter[GlobalVars.DistanceUnitIndex].Unit;
			}
		}

		public static string HeightUnitM
		{
			get
			{
				return "м";	//Resources.Resource.HeightUnitM;
			}
		}

		public static string HeightUnitFt
		{
			get
			{
				return "фт";	//Resources.Resource.HeightUnitFt;
			}
		}

		public static string HeightUnit
		{
			get
			{
				return GlobalVars.HeightConverter[GlobalVars.HeightUnitIndex].Unit;
			}
		}

		public static string SpeedUnitKm_H
		{
			get
			{
				return "Км/ч";	//Resources.Resource.SpeedUnitKm_H;
			}
		}

		public static string SpeedUnitKt
		{
			get
			{
				return "узел";	//Resources.Resource.SpeedUnitKt;
			}
		}

		public static string SpeedUnit
		{
			get
			{
				return GlobalVars.SpeedConverter[GlobalVars.SpeedUnitIndex].Unit;
			}
		}

		public static string DSpeedUnitM_Min
		{
			get
			{
				return "м/мин";	//Resources.Resource.DSpeedUnitM_Min;
			}
		}

		public static string DSpeedUnitFt_Min
		{
			get
			{
				return "фт/мин";	//Resources.Resource.DSpeedUnitFt_Min;
			}
		}

		public static string DSpeedUnit
		{
			get
			{
				return GlobalVars.DSpeedConverter[GlobalVars.HeightUnitIndex].Unit;
			}
		}
	}
}
