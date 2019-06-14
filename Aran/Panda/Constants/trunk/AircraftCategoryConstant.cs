using Aran.PANDA.Common;
namespace Aran.PANDA.Constants
{
	public enum aircraftCategory
	{
		acA,
		acB,
		acC,
		acD,
		acDL,
		acE,
		acH
	};

	public enum aircraftCategoryData
	{
		VatMin, VatMax, ViafMin, ViafMax,
		ViafStar, VfafMin, VfafMax, Vva,
		VmaInter, VmaFaf, arStraightSegmen, arObsClearance,
		arMinOCH, arMinVisibility, arFAMinOCH15, arFAMinOCH30,
		arMaxInterAngle, arT45_180, arMaxOutBoundDes, arMaxInBoundDesc,
		arMinHV_FAS, arMaxHV_FAS, arFADescent_Max, arImHorSegLen,
		arMinISlen00_15p, arMinISlen16_30p, arMinISlen31_60p, arMinISlen61_90p,
		arSemiSpan, arVerticalSize, enIAS, 
        hldIASUpTo4250MinNormalTerminal, hldIASUpTo4250MaxNormalTerminal,
        hldIASUpTo6100MinNormalTerminal, hldIASUpTo6100MaxNormalTerminal,
        hldIASUpTo10350MinNormalTerminal, hldIASUpTo10350MaxNormalTerminal,
        hldIASUpTo4250MinTurbulenceTerminal, hldIASUpTo4250MaxTurbulenceTerminal,
        hldIASMinInitialApproachTerminal, hldIASMaxInitialApproachTerminal,
        hldIASEnroute, RacetrckIASMin, RacetrckIASMax
	};

	public class AircraftCategoryConstant : NamedConstantObject
	{
		public AircraftCategoryConstant()
		{
			Value = new EnumArray<double, aircraftCategory>();
			for (int i = 0; i < Value.Length; i++)
				Value[i] = 0.0;
		}

		public double this[int index]
		{
			get
			{
				return Value[index];
			}
		}

		public double this[aircraftCategory airCraftCategory]
		{
			get
			{
				return Value[airCraftCategory];
			}
		}

		public override AranObject Clone()
		{
			AircraftCategoryConstant result = new AircraftCategoryConstant();
			result.Assign(this);
			for (int i = 0; i < Value.Length; i++)
				result.Value[i] = Value[i];
			return result;
		}

		public EnumArray<double, aircraftCategory> Value { get; protected set; }
	}
}
