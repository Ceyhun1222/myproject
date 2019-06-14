
using System;
namespace Aran.PANDA.Constants
{
	public enum ePANSOPSData
	{
		//=============  Departure ===================
		dpOIS, dpMOC, dpNGui_Ar1, dpNGui_Ar1_Wd, dpTr_AdjAngle,
		dpAr1_OB_TrAdj, dpAr1_IB_TrAdj, dpAr2_Bnd_TrAdj, dpT_Bank,
		dpWind_Speed, dpT_TechToleranc, dpT_Init, dpT_Init_Wd,
		dpObsClr, dpT_Gui_dist, dpH_abv_DER, dpOIS_abv_DER, dpPDG_60,
		dpPDG_Nom, dpStr_Gui_dist, dpafTrn_OSplay, dpafTrn_ISplay,
		dpOv_Nav_PDG, dpTP_by_DME_div, dpNGui_Ar1_Dist, dpOD1_ZoneAdjA,
		dpOD2_ZoneAdjA, dpGui_Ar1_Wd, dpGui_Ar1, ISA, dpSecAreaCutAngl,
		dpMaxPosPDG, dpInterMinAngle, dpInterMaxAngle, dpFlightTechTol,

		// ================ Arrival =======================
		arBufferMSA, arMSARoundThreshold, arMinInterDist, arMinInterToler,
		arStrInAlignment, arMaxRangeFAS, arCirclAprShift, arHoldAreaEdge,
		arConstantsContractBuffer, arIASegmentMOC, arISegmentMOC, arFASeg_FAF_MOC,
		arFASegmentMOC, arSOCdelayTime, arNearTerrWindSp, arISAmax,
		arISAmin, arMA_InterMOC, arMA_FinalMOC, arMAS_Climb_Min, arMAS_Climb_Max,
		arMAS_Climb_Nom, arMATurnAlt, arT_TechToleranc, arTP_by_DME_div,
		arT_Gui_dist, arafTrn_OSplay, arafTrn_ISplay, arSecAreaCutAngl,
		arMATurnTrshAngl, arFAFLenght, arMinRangeFAS, arAbv_Treshold, arFADescent_Min,
		arFADescent_Nom, arImRange_Min, arImRange_Nom, arImRange_Max, arImMaxIntercept,
		arIFHalfWidth, arMinISlen00_90, arMinISlen91_96, arMinISlen97_02,
		arMinISlen03_08, arMinISlen09_14, arMinISlen15_20, arFAFTolerance,
		arIFTolerance, arImDescent_Max, arFAPMaxRange, arOverHeadToler,
		arTrackAccuracy, arMAPilotToleran, arMA_SplayAngle, arSecAr, arOptimalFAFRang,
		arCurvatureCoeff, arFixMaxIgnorGrd, arMOCChangeDist, arAddMOCCoef,
		arFIX15PlaneRang, arFAFOptimalDist, arIAFMinDMERange, arIAFMaxTurnAngl,
		arIAFMinTurnAngl, arIAFMinGuadLen, arVisAverBank, enRouteSplayAngl, arMABankAngle,
		arIADescent_Nom, arIADescent_Max, enTechTolerance, arIBankTolerance,
		arBankAngle, arIPilotToleranc, arMSAMOC, rnvFlyOInterBank, rnvFlyByTechTol,
		rnvFlyOTechTol, rnvImMinDist, enrMaxTurnAngle, rnvIFMaxTurnAngl, enBankTolerance,
		dpPilotTolerance, dpBankTolerance, enrMOC, arArrivalMOC, arDRNomAltitude,
		arRDH, arILSSectorWidth, arMinGPAngle, arOptGPAngle, arMaxGPAngleCat1, arMaxGPAngleCat2,
		arMinMACGrad, arStdMACGrad, arMaxMACGrad,
		//=============== en-route
		enTurnIAS, enBankAngle
	};

	public class PansopsConstant : NamedConstantObject
	{
		public PansopsConstant()
			: base()
		{
			Value = 0.0;
		}

		public override AranObject Clone()
		{
			PansopsConstant result = new PansopsConstant();
			result.Assign(this);
			result.Value = Value;
			return result;
		}

		public double Value { get; protected set; }
	}
}
