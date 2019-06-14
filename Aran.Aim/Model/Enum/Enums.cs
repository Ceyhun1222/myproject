using System.ComponentModel;

namespace Aran.Aim.Enums
{
	public enum UomDistance
	{
		NM,
		KM,
		M,
		FT,
		MI,
		CM
	}
	public enum UomDistanceVertical
	{
		FT,
		M,
		FL,
		SM
	}
	public enum UomDuration
	{
		HR,
		MIN,
		SEC
	}
	public enum UomFL
	{
		FL,
		SM
	}
	public enum UomFrequency
	{
		HZ,
		KHZ,
		MHZ,
		GHZ
	}
	public enum UomPressure
	{
		PA,
		MPA,
		PSI,
		BAR,
		TORR,
		ATM,
		HPA
	}
	public enum UomSpeed
	{
		KM_H,
		KT,
		MACH,
		M_MIN,
		FT_MIN,
		M_SEC,
		FT_SEC,
		MPH
	}
	public enum UomTemperature
	{
		C,
		F,
		K
	}
	public enum UomWeight
	{
		KG,
		T,
		LB,
		TON
	}
	public enum UomDepth
	{
		MM,
		CM,
		IN,
		FT
	}
	public enum UomLightIntensity
	{
		CD
	}
	public enum CodeRunwayProtectionArea
	{
		CWY,
		RESA,
		OFZ,
		IOFZ,
		POFZ,
		ILS,
		VGSI,
		STOPWAY
	}
	public enum CodeStatusOperations
	{
		NORMAL,
		DOWNGRADED,
		UNSERVICEABLE,
		WORK_IN_PROGRESS
	}
	public enum CodeDirectionTurn
	{
		LEFT,
		RIGHT,
		EITHER
	}
	public enum CodeRunwayMarking
	{
		PRECISION,
		NONPRECISION,
		BASIC,
		NONE,
		RUNWAY_NUMBERS,
		NON_STANDARD,
		HELIPORT
	}
	public enum CodeMarkingCondition
	{
		GOOD,
		FAIR,
		POOR,
		EXCELLENT
	}
	public enum CodeLightingJAR
	{
		FALS,
		IALS,
		BALS,
		NALS
	}
	public enum CodeApproachGuidance
	{
		NON_PRECISION,
		ILS_PRECISION_CAT_I,
		ILS_PRECISION_CAT_II,
		ILS_PRECISION_CAT_IIIA,
		ILS_PRECISION_CAT_IIIB,
		ILS_PRECISION_CAT_IIIC,
		ILS_PRECISION_CAT_IIID,
		MLS_PRECISION
	}
	public enum CodeRunwayPointRole
	{
		START,
		THR,
		DISTHR,
		TDZ,
		MID,
		END,
		START_RUN,
		LAHSO,
		ABEAM_GLIDESLOPE,
		ABEAM_PAR,
		ABEAM_ELEVATION,
		ABEAM_TDR,
		ABEAM_RER
	}
	public enum CodeRunway
	{
		RWY,
		FATO
	}
	public enum CodeArrestingGearEngageDevice
	{
		_61QSII,
		_62NI,
		_63PI,
		NET_A30,
		NET_A40,
		BAK_11_STRUT,
		BAK_12,
		BAK_14_HOOK,
		BAK_15_STANCHION_NET,
		BAK_15_HOOK,
		EMAS,
		HOOK_CABLE,
		HP_NET,
		J_BAR,
		JET_BARRIER,
		MA_1_NET,
		MA_1A_HOOK_CABLE,
		NET,
		HOOK_H
	}
	public enum CodeArrestingGearEnergyAbsorb
	{
		ROTARY_1300,
		ROTARY_2800,
		ROTARY_34B_1A,
		ROTARY_34B_1B,
		ROTARY_34B_1C,
		ROTARY_34D_1F,
		ROTARY_44B_2C,
		ROTARY_44B_2D,
		ROTARY_44B_2E,
		ROTARY_44B_2F,
		ROTARY_44B_2H,
		ROTARY_44B_2I,
		ROTARY_44B_2L,
		ROTARY_44B_3A,
		ROTARY_44B_3H,
		ROTARY_44B_3L,
		ROTARY_44B_4C,
		ROTARY_44B_4E,
		ROTARY_44B_4H,
		ROTARY_500S,
		ROTARY_500S_4,
		ROTARY_500S_6,
		ROTARY_500S_8,
		ROTARYTRANS_500S_8,
		ROTARY_AAE_64,
		ROTARY_BAK_12A,
		ROTARY_BAK_12B,
		ROTARY_BAK_13,
		LINEAR_BAK_6,
		ROTARY_BAK_9,
		DISK_BEFAB_12_3,
		DISK_BEFAB_20_4,
		DISK_BEFAB_21_2,
		DISK_BEFAB_24_4,
		DISK_BEFAB_56_2,
		DISK_BEFAB_6_3,
		DISK_BEFAB_60_2,
		DISK_BEFAB_8_3,
		CHAIN_CHAG,
		ROTARY_DUAL_BAK_12,
		ROTARY_E15,
		ROTARY_E27,
		ROTARY_E28,
		CHAIN_E5,
		CHAIN_E5_1,
		CHAIN_E5_2,
		CHAIN_E5_3,
		CHAIN_E6,
		ROTARY_CHAIN_JETSTOP,
		MOBILROTARY_M21,
		MOBILROTARY_MAAS,
		MOBILROTARY_MAG_I,
		MOBILROTARY_MAG_II,
		MOBILROTARY_MAG_III,
		MOBILROTARY_MAG_IV,
		MOBILROTARY_MAG_IX,
		MOBILROTARY_MAG_VI,
		MOBILROTARY_MAG_VII,
		MOBILROTARY_MAG_VIII,
		MOBILROTARY_MAG_X,
		TEXTILE_MB_100,
		TEXTILE_MB_60,
		MOBILROTARY_HYDRAULIC_WATER,
		ROTARY_PUAG_MK_21,
		DISK_RAF_MK_12A,
		DISK_RAF_MK_6,
		RAF_PORTABLE_AGEAR,
		DISK_RAF_TYPEA_BEFAB_6_3,
		DISK_RAF_TYPEB_BEFAB_12_3,
		ROTARY_RHAG_MK_1,
		ROTARY_HYDRAULIC_WATER,
		BARRIER_DISK_SAFELAND,
		LINEAR_SPRAG_MK_1
	}
	public enum CodeRunwayElement
	{
		NORMAL,
		INTERSECTION,
		DISPLACED,
		SHOULDER
	}
	public enum CodeGradeSeparation
	{
		UNDERPASS,
		OVERPASS
	}
	public enum CodeVASIS
	{
		PAPI,
		APAPI,
		HAPI,
		VASIS,
		AVASIS,
		TVASIS,
		ATVASIS,
		_3B_VASIS,
		_3B_AVASIS,
		_3B_ATVASIS,
		PVASI,
		TRCV,
		PNI,
		ILU,
		OLS,
		LCVASI
	}
	public enum CodeSide
	{
		LEFT,
		RIGHT,
		BOTH
	}
	public enum CodeRVRReading
	{
		TDZ,
		MID,
		TO
	}
	public enum CodeDeclaredDistance
	{
		LDA,
		TORA,
		TODA,
		ASDA,
		DTHR,
		TODAH,
		RTODAH,
		LDAH
	}
	public enum CodeStatusAirport
	{
		NORMAL,
		LIMITED,
		CLOSED
	}
	public enum CodeAirportWarning
	{
		WIP,
		EQUIP,
		BIRD,
		ANIMAL,
		RUBBER_REMOVAL,
		PARKED_ACFT,
		RESURFACING,
		PAVING,
		PAINTING,
		INSPECTION,
		GRASS_CUTTING,
		CALIBRATION
	}
	public enum CodeOperationManoeuvringArea
	{
		LANDING,
		TAKEOFF,
		TOUCHGO,
		TRAIN_APPROACH,
		TAXIING,
		CROSSING,
		AIRSHOW,
		ALL
	}
	public enum CodeHoldingCategory
	{
		NON_PRECISION,
		CAT_I,
		CAT_II_III
	}
	public enum CodeTaxiway
	{
		AIR,
		GND,
		EXIT,
		FASTEXIT,
		STUB,
		TURN_AROUND,
		PARALLEL,
		BYPASS
	}
	public enum CodeTaxiwayElement
	{
		NORMAL,
		INTERSECTION,
		SHOULDER,
		HOLDING_BAY
	}
	public enum CodeGuidanceLine
	{
		RWY,
		TWY,
		APRON,
		GATE_TLANE,
		LI_TLANE,
		LO_TLANE,
		AIR_TLANE
	}

	[System.FlagsAttribute]     //[Flags]
	public enum CodeDirection
	{
		FORWARD,
		BACKWARD,
		BOTH
	}

	public enum CodeApronElement
	{
		NORMAL,
		PARKING,
		RAMP,
		CARGO,
		FUEL,
		HARDSTAND,
		MAINT,
		MILITARY,
		LOADING,
		TAXILANE,
		TURNAROUND,
		TEMPORARY,
		STAIRS
	}
	public enum CodeAircraftStand
	{
		NI,
		ANG_NI,
		ANG_NO,
		PARL,
		RMT,
		ISOL
	}
	public enum CodeVisualDockingGuidance
	{
		AGNIS,
		PAPA,
		SAFE_GATE,
		SAFE_DOC,
		APIS,
		A_VDGS,
		AGNIS_STOP,
		AGNIS_PAPA
	}
	public enum CodeRoad
	{
		SERVICE,
		PUBLIC
	}
	public enum CodeLoadingBridge
	{
		ARM,
		MOVABLE_ARM,
		PORTABLE_RAMP,
		PORTABLE_STAIRS
	}
	public enum CodeHelicopterPerformance
	{
		_1,
		_2,
		_3
	}
	public enum CodeLightIntensity
	{
		LIL,
		LIM,
		LIH,
		LIL_LIH,
		PREDETERMINED
	}
	public enum CodeColour
	{
		YELLOW,
		RED,
		WHITE,
		BLUE,
		GREEN,
		PURPLE,
		ORANGE,
		AMBER,
		BLACK,
		BROWN,
		GREY,
		LIGHT_GREY,
		MAGENTA,
		PINK,
		VIOLET
	}
	public enum CodeApronSection
	{
		EDGE
	}
	public enum CodeTaxiwaySection
	{
		CL,
		EDGE,
		END,
		RWY_INT,
		TWY_INT
	}
	public enum CodeRunwaySection
	{
		TDZ,
		AIM,
		CL,
		EDGE,
		THR,
		DESIG,
		AFT_THR,
		DTHR,
		END,
		TWY_INT,
		RPD_TWY_INT,
		_1_THIRD,
		_2_THIRD,
		_3_THIRD
	}
	public enum CodeTLOFSection
	{
		AIM,
		EDGE
	}
	public enum CodeProtectAreaSection
	{
		EDGE,
		END,
		CL
	}
	public enum CodeLightHoldingPosition
	{
		STOP_BAR,
		SIGN
	}
	public enum CodeSystemActivation
	{
		ON,
		ON_OR_OFF,
		OFF
	}
	public enum CodeApproachLightingICAO
	{
		SIMPLE,
		CAT1,
		CAT23,
		CIRCLING,
		LEADIN,
		NONE
	}
	public enum CodeApproachLighting
	{
		ALSAF,
		MALS,
		MALSR,
		SALS,
		SSALS,
		SSALR,
		LDIN,
		ODALS,
		AFOVRN,
		MILOVRN,
		CALVERT
	}
	public enum CodeMarkingStyle
	{
		SOLID,
		DASHED,
		DOTTED
	}
	public enum CodeFrictionEstimate
	{
		GOOD,
		MEDIUM_GOOD,
		MEDIUM,
		MEDIUM_POOR,
		POOR,
		UNRELIABLE
	}
	public enum CodeFrictionDevice
	{
		BRD,
		GRT,
		MUM,
		RFT,
		SFH,
		SFL,
		SKH,
		SKL,
		TAP
	}
	public enum CodeContamination
	{
		NONE,
		DAMP,
		WATER,
		FROST,
		DRY_SNOW,
		WET_SNOW,
		SLUSH,
		ICE,
		COMPACT_SNOW,
		RUT,
		ASH,
		SAND,
		OIL,
		RUBBER,
		GRAS
	}
	public enum CodeBuoy
	{
		BLACK_RED_FL2,
		GREEN,
		GREEN_RED_GFL,
		Q3_VQ3,
		Q6_VQ6,
		Q9_VQ9,
		Q_VQ,
		RED,
		RED_GREEN_RFL,
		RED_WHITE,
		WHITE,
		YELLOW
	}
	public enum CodeWorkArea
	{
		CONSTRUCTION,
		SURFACEWORK,
		PARKED
	}
	public enum CodeSurfaceComposition
	{
		ASPH,
		ASPH_GRASS,
		CONC,
		CONC_ASPH,
		CONC_GRS,
		GRASS,
		SAND,
		WATER,
		BITUM,
		BRICK,
		MACADAM,
		STONE,
		CORAL,
		CLAY,
		LATERITE,
		GRAVEL,
		EARTH,
		ICE,
		SNOW,
		MEMBRANE,
		METAL,
		MATS,
		PIERCED_STEEL,
		WOOD,
		NON_BITUM_MIX
	}
	public enum CodeSurfacePreparation
	{
		NATURAL,
		ROLLED,
		COMPACTED,
		GRADED,
		GROOVED,
		OILED,
		PAVED,
		PFC,
		AFSC,
		RFSC,
		NON_GROOVED
	}
	public enum CodeSurfaceCondition
	{
		GOOD,
		FAIR,
		POOR,
		UNSAFE,
		DEFORMED
	}
	public enum CodePCNPavement
	{
		RIGID,
		FLEXIBLE
	}
	public enum CodePCNSubgrade
	{
		A,
		B,
		C,
		D
	}
	public enum CodePCNTyrePressure
	{
		W,
		X,
		Y,
		Z
	}
	public enum CodePCNMethod
	{
		TECH,
		ACFT
	}
	public enum CodeUsageLimitation
	{
		PERMIT,
		CONDITIONAL,
		FORBID,
		RESERV
	}
	public enum CodeAirportHeliportCollocation
	{
		FULL,
		RWY,
		PARTIAL,
		UNILATERAL,
		SEPARATED
	}
	public enum CodeAirportHeliport
	{
		AD,
		AH,
		HP,
		LS
	}
	public enum CodeMilitaryOperations
	{
		CIVIL,
		MIL,
		JOINT
	}
	public enum CodeVerticalDatum
	{
		EGM_96,
		AHD,
		NAVD88,
		OTHER_EGM_08
	}
	public enum CodeAuthorityRole
	{
		OWN,
		OPERATE,
		SUPERVISE
	}
	public enum CodeLogicalOperator
	{
		AND,
		OR,
		NOT,
		NONE
	}
	public enum CodeOperationAirportHeliport
	{
		LANDING,
		TAKEOFF,
		TOUCHGO,
		TRAIN_APPROACH,
		ALTN_LANDING,
		AIRSHOW,
		ALL
	}
	public enum CodeAuthority
	{
		OWN,
		DLGT,
		AIS
	}
	public enum CodeAirspace
	{
		NAS,
		FIR,
		FIR_P,
		UIR,
		UIR_P,
		CTA,
		CTA_P,
		OCA_P,
		OCA,
		UTA,
		UTA_P,
		TMA,
		TMA_P,
		CTR,
		CTR_P,
		OTA,
		SECTOR,
		SECTOR_C,
		TSA,
		CBA,
		RCA,
		RAS,
		AWY,
		MTR,
		P,
		R,
		D,
		ADIZ,
		NO_FIR,
		PART,
		CLASS,
		POLITICAL,
		D_OTHER,
		TRA,
		A,
		W,
		PROTECT,
		AMA,
		ASR,
		ADV,
		UADV,
		ATZ,
		ATZ_P,
		HTZ,
		NAS_P
	}
	public enum CodeAirspaceAggregation
	{
		BASE,
		UNION,
		INTERS,
		SUBTR
	}
	public enum CodeVerticalReference
	{
		SFC,
		MSL,
		W84,
		STD,
		OTHER_QFE,
		OTHER_QNH
	}
	public enum CodeAirspaceActivity
	{
		AD_TFC,
		HELI_TFC,
		TRAINING,
		AEROBATICS,
		AIRSHOW,
		SPORT,
		ULM,
		GLIDING,
		PARAGLIDER,
		HANGGLIDING,
		PARACHUTE,
		AIR_DROP,
		BALLOON,
		RADIOSONDE,
		SPACE_FLIGHT,
		UAV,
		AERIAL_WORK,
		CROP_DUSTING,
		FIRE_FIGHTING,
		MILOPS,
		REFUEL,
		JET_CLIMBING,
		EXERCISE,
		TOWING,
		NAVAL_EXER,
		MISSILES,
		AIR_GUN,
		ARTILLERY,
		SHOOTING,
		BLASTING,
		WATER_BLASTING,
		ANTI_HAIL,
		BIRD,
		BIRD_MIGRATION,
		FIREWORK,
		HI_RADIO,
		HI_LIGHT,
		LASER,
		NATURE,
		FAUNA,
		NO_NOISE,
		ACCIDENT,
		POPULATION,
		VIP,
		VIP_PRES,
		VIP_VICE,
		OIL,
		GAS,
		REFINERY,
		CHEMICAL,
		NUCLEAR,
		TECHNICAL,
		ATS,
		PROCEDURE
	}
	public enum CodeStatusAirspace
	{
		AVBL_FOR_ACTIVATION,
		ACTIVE,
		IN_USE,
		INACTIVE,
		INTERMITTENT
	}
	public enum CodeAirspaceClassification
	{
		A,
		B,
		C,
		D,
		E,
		F,
		G
	}
	public enum CodeGeoBorder
	{
		STATE,
		WATER,
		COAST,
		RIVER,
		BANK
	}
	public enum CodeAirspaceDependency
	{
		FULL_GEOMETRY,
		HORZ_PROJECTION
	}
	public enum CodePAR
	{
		FPN16,
		FPN62,
		GPN22,
		GPN25,
		MPN14K,
		TPN19
	}
	public enum CodePrimaryRadar
	{
		ASR,
		ARSR
	}
	public enum CodeStandbyPower
	{
		BATTERY,
		COMMERCIAL,
		GENERATOR,
		UNKNOWN,
		NONE
	}
	public enum CodeRadarService
	{
		PAR,
		ARSR,
		ASR,
		SSR
	}
	public enum CodeReflector
	{
		TOUCHDOWN,
		RUNWAY_END,
		REFERENCE
	}
	public enum CodeTransponder
	{
		MODE_1,
		MODE_2,
		MODE_3A,
		MODE_4,
		MODE_5,
		MODE_C,
		MODE_S
	}
	public enum CodeObstacleAssessmentSurface
	{
		_40_TO_1,
		_72_TO_1,
		MA,
		FINAL,
		PT_ENTRY_AREA,
		PRIMARY,
		SECONDARY,
		ZONE1,
		ZONE2,
		ZONE3,
		AREA1,
		AREA2,
		AREA3,
		TURN_INITIATION,
		TURN,
		DER
	}
	public enum CodeObstructionIdSurfaceZone
	{
		APPROACH,
		CONICAL,
		HORIZONTAL,
		PRIMARY,
		TRANSITION
	}
	public enum CodeAltitudeAdjustment
	{
		RA,
		AS,
		AT,
		AC,
		SI,
		XL,
		PR,
		HAA,
		MA,
		PT,
		DG,
		GS,
		CA,
		MT,
		MAH,
		SA,
		AAO,
		VA
	}
	public enum CodeLevelTableDesignator
	{
		IFR,
		IFR_METRES,
		VFR,
		VFR_METRES,
		IFR_RVSM,
		IFR_METRES_RVSM,
		VFR_RVMS,
		VFR_METRES_RVSM
	}
	public enum CodeFlightRule
	{
		IFR,
		VFR,
		ALL
	}
	public enum CodeNorthReference
	{
		TRUE,
		MAG,
		GRID
	}
	public enum CodeLevelSeries
	{
		ODD,
		EVEN,
		ODD_HALF,
		EVEN_HALF
	}
	public enum CodeRVSM
	{
		RVSM,
		NON_RVSM
	}
	public enum CodeRadioFrequencyArea
	{
		COV,
		T_COV,
		SCL,
		RHG,
		UNREL,
		RES,
		UUS,
		OUT,
		ESV
	}
	public enum CodeRadioSignal
	{
		AZIMUTH,
		DISTANCE,
		BEAM,
		VOICE,
		DATALINK
	}
	public enum CodeTelecomNetwork
	{
		AFTN,
		AMHS,
		INTERNET,
		SITA,
		ACARS,
		ADNS
	}
	public enum CodeAircraft
	{
		LANDPLANE,
		SEAPLANE,
		AMPHIBIAN,
		HELICOPTER,
		GYROCOPTER,
		TILT_WING,
		STOL,
		GLIDER,
		HANGGLIDER,
		PARAGLIDER,
		ULTRA_LIGHT,
		BALLOON,
		UAV,
		ALL
	}
	public enum CodeAircraftEngine
	{
		JET,
		PISTON,
		TURBOPROP,
		ALL
	}
	public enum CodeAircraftEngineNumber
	{
		_1,
		_2,
		_3,
		_4,
		_6,
		_8,
		_2C
	}
	public enum CodeAircraftCategory
	{
		A,
		B,
		C,
		D,
		E,
		H,
		ALL
	}
	public enum CodeValueInterpretation
	{
		ABOVE,
		AT_OR_ABOVE,
		AT_OR_BELOW,
		BELOW
	}
	public enum CodeAircraftWingspanClass
	{
		I,
		II,
		III,
		IV,
		V,
		VI
	}
	public enum CodeWakeTurbulence
	{
		LOW,
		MEDIUM,
		HIGH
	}
	public enum CodeNavigationEquipment
	{
		DME,
		VOR_DME,
		DME_DME,
		TACAN,
		ILS,
		MLS,
		GNSS,
		WAAS,
		LORAN,
		INS,
		FMS
	}
	public enum CodeNavigationSpecification
	{
		RNAV_10,
		RNAV_5,
		RNAV_2,
		RNAV_1,
		RNP_4,
		RNP_2,
		BASIC_RNP_1,
		ADVANCED_RNP_1,
		RNP_APCH,
		RNP_AR_APCH
	}
	public enum CodeEquipmentAntiCollision
	{
		ACAS_I,
		ACAS_II,
		GPWS
	}
	public enum CodeCommunicationMode
	{
		HF,
		VHF,
		VDL1,
		VDL2,
		VDL4,
		AMSS,
		ADS_B,
		ADS_B_VDL,
		HFDL,
		VHF_833,
		UHF
	}
	public enum CodeFlight
	{
		OAT,
		GAT,
		ALL
	}
	public enum CodeFlightStatus
	{
		HEAD,
		STATE,
		HUM,
		HOSP,
		SAR,
		ALL,
		EMERGENCY
	}
	public enum CodeMilitaryStatus
	{
		MIL,
		CIVIL,
		ALL
	}
	public enum CodeFlightOrigin
	{
		NTL,
		INTL,
		ALL,
		HOME_BASED
	}
	public enum CodeFlightPurpose
	{
		SCHEDULED,
		NON_SCHEDULED,
		PRIVATE,
		AIR_TRAINING,
		AIR_WORK,
		ALL,
		PARTICIPANT
	}
	public enum CodeLightSource
	{
		FLOOD,
		STROBE
	}
	public enum CodeAltitudeUse
	{
		ABOVE_LOWER,
		BELOW_UPPER,
		AT_LOWER,
		BETWEEN,
		RECOMMENDED,
		EXPECT_LOWER,
		AS_ASSIGNED
	}
	public enum CodeArcDirection
	{
		CWA,
		CCA
	}
	public enum CodeBearing
	{
		TRUE,
		MAG,
		RDL,
		TRK,
		HDG
	}
	public enum CodeDirectionReference
	{
		TO,
		FROM,
		OTHER_CW,
		OTHER_CCW
	}
	public enum CodeTimeReference
	{
		UTC,
		UTC_minus_12,
		UTC_minus_11,
		UTC_minus_10,
		UTC_minus_9,
		UTC_minus_8,
		UTC_minus_7,
		UTC_minus_6,
		UTC_minus_5,
		UTC_minus_4,
		UTC_minus_3,
		UTC_minus_2,
		UTC_minus_1,
		UTC_plus_1,
		UTC_plus_2,
		UTC_plus_3,
		UTC_plus_4,
		UTC_plus_5,
		UTC_plus_6,
		UTC_plus_7,
		UTC_plus_8,
		UTC_plus_9,
		UTC_plus_10,
		UTC_plus_11,
		UTC_plus_12,
		UTC_plus_13,
		UTC_plus_14
	}
	public enum CodeDay
	{
		MON,
		TUE,
		WED,
		THU,
		FRI,
		SAT,
		SUN,
		WORK_DAY,
		BEF_WORK_DAY,
		AFT_WORK_DAY,
		HOL,
		BEF_HOL,
		AFT_HOL,
		ANY,
		BUSY_FRI
	}
	public enum CodeTimeEvent
	{
		SR,
		SS
	}
	public enum CodeTimeEventCombination
	{
		EARLIEST,
		LATEST
	}
	public enum CodeSpecialDate
	{
		HOL,
		BUSY_FRI
	}
	public enum CodeMeteoConditions
	{
		IMC,
		VMC,
		ALL
	}
	public enum CodeFlightDestination
	{
		ARR,
		DEP,
		OVERFLY,
		ALL
	}
	public enum CodeFacilityRanking
	{
		PRIMARY,
		SECONDARY,
		ALTERNATE,
		EMERG,
		GUARD
	}
	public enum CodeRadioEmission
	{
		A2,
		A3A,
		A3B,
		A3E,
		A3H,
		A3J,
		A3L,
		A3U,
		J3E,
		NONA1A,
		NONA2A,
		PON,
		A8W,
		A9W,
		NOX,
		G1D
	}
	public enum CodeCommunicationDirection
	{
		UPLINK,
		DOWNLINK,
		BIDIRECTIONAL,
		UPCAST,
		DOWNCAST
	}
	public enum CodePilotControlledLighting
	{
		STANDARD_FAA,
		NON_STANDARD
	}
	public enum CodeIntensityStandBy
	{
		OFF,
		LOW
	}
	public enum CodeServiceInformation
	{
		AFIS,
		AIS,
		ATIS,
		BRIEFING,
		FIS,
		OFIS_VHF,
		OFIS_HF,
		NOTAM,
		INFO,
		RAF,
		METAR,
		SIGMET,
		TWEB,
		TAF,
		VOLMET,
		ALTIMETER,
		ASOS,
		AWOS,
	    // Added By EminS for AIP
	    // Basis is: Eurocontrol specification AIPtoAIXM5.1Mapping.doc
        OTHER_CARTO_SERVICE
    }
	public enum CodeServiceGroundControl
	{
		TWR,
		SMGCS,
		TAXI
	}
	public enum CodeServiceATC
	{
		ACS,
		UAC,
		OACS,
		APP,
		TWR,
		ADVS,
		EFAS,
		CTAF
	}
	public enum CodeServiceATFM
	{
		FPL,
		FPLV,
		ATFM,
		CLEARANCE,
		SCHED
	}
	public enum CodeServiceSAR
	{
		ALRS,
		SAR,
		RCC
	}
	public enum CodePassengerService
	{
		CUST,
		SAN,
		SECUR,
		VET,
		HOTEL,
		TRANSPORT,
		REST,
		INFO,
		BANK,
		POST,
		MEDIC
	}
	public enum CodeAircraftGroundService
	{
		DEICE,
		HAND,
		HANGAR,
		REPAIR,
		REMOVE,
        OTHER
	}
	public enum CodeFireFighting
	{
		H1,
		H2,
		H3,
		A1,
		A2,
		A3,
		A4,
		A5,
		A6,
		A7,
		A8,
		A9,
		A10
	}
	public enum CodeAviationStandards
	{
		ICAO,
		IATA,
		NATO,
		FAA
	}
	public enum CodeFuel
	{
		AVGAS,
		AVGAS_LL,
		OCT73,
		OCT80,
		OCT82UL,
		OCT80_87,
		OCT91_98,
		OCT100_130,
		OCT108_135,
		OCT115_145,
		MOGAS,
		JET,
		A,
		A1,
		A1_PLUS,
		B,
		JP1,
		JP2,
		JP3,
		JP4,
		JP5,
		JP6,
		JPTS,
		JP7,
		JP8,
		JP8_HIGHER,
		JP9,
		JP10,
		F18,
		F34,
		F35,
		F40,
		F44,
		TR0,
		TR4,
		TS1,
		RT,
		ALL
	}
	public enum CodeNitrogen
	{
		LPNG,
		HPNG,
		LHNG,
		LNG,
		NGRB,
		HNGRB,
		LNGRB,
		NG
	}
	public enum CodeOil
	{
		PISTON,
		TURBO,
		HYDRAULIC
	}
	public enum CodeOxygen
	{
		LPOX,
		HPOX,
		LHOX,
		LOX,
		OXRB,
		HOXRB,
		LOXRB,
		OX
	}
	public enum CodeStatusService
	{
		NORMAL,
		LIMITED,
		ONTEST,
		UNSERVICEABLE
	}
	public enum CodeReferenceRole
	{
		INTERSECTION,
		RECNAV,
		ATD,
		OTHER_OVERHEAD,
		RAD_DME
	}
	public enum CodeATCReporting
	{
		COMPULSORY,
		ON_REQUEST,
		NO_REPORT,
#if QATAR
        OTHER_MET_COMPULSORY
#endif
	}
	public enum CodeProcedureFixRole
	{
		IAF,
		IF,
		IF_IAF,
		FAF,
		VDP,
		SDF,
		FPAP,
		FTP,
		FROP,
		TP,
		MAPT,
		MAHF,
		OTHER_WPT
	}
	public enum CodeFreeFlight
	{
		PITCH,
		CATCH
	}
	public enum CodeRVSMPointRole
	{
		IN,
		OUT,
		IN_OUT
	}
	public enum CodeMilitaryRoutePoint
	{
		S,
		T,
		X,
		AS,
		AX,
		ASX
	}
	public enum CodeCardinalDirection
	{
		N,
		NE,
		E,
		SE,
		S,
		SW,
		W,
		NW,
		NNE,
		ENE,
		ESE,
		SSE,
		SSW,
		WSW,
		WNW,
		NNW
	}
	public enum CodeDistanceIndication
	{
		DME,
		GEODETIC
	}
	public enum CodeMLSAzimuth
	{
		FWD,
		BWD
	}
	public enum CodeMLSChannel
	{
		_500,
		_501,
		_502,
		_503,
		_504,
		_505,
		_506,
		_507,
		_508,
		_509,
		_510,
		_511,
		_512,
		_513,
		_514,
		_515,
		_516,
		_517,
		_518,
		_519,
		_520,
		_521,
		_522,
		_523,
		_524,
		_525,
		_526,
		_527,
		_528,
		_529,
		_530,
		_531,
		_532,
		_533,
		_534,
		_535,
		_536,
		_537,
		_538,
		_539,
		_540,
		_541,
		_542,
		_543,
		_544,
		_545,
		_546,
		_547,
		_548,
		_549,
		_550,
		_551,
		_552,
		_553,
		_554,
		_555,
		_556,
		_557,
		_558,
		_559,
		_560,
		_561,
		_562,
		_563,
		_564,
		_565,
		_566,
		_567,
		_568,
		_569,
		_570,
		_571,
		_572,
		_573,
		_574,
		_575,
		_576,
		_577,
		_578,
		_579,
		_580,
		_581,
		_582,
		_583,
		_584,
		_585,
		_586,
		_587,
		_588,
		_589,
		_590,
		_591,
		_592,
		_593,
		_594,
		_595,
		_596,
		_597,
		_598,
		_599,
		_600,
		_601,
		_602,
		_603,
		_604,
		_605,
		_606,
		_607,
		_608,
		_609,
		_610,
		_611,
		_612,
		_613,
		_614,
		_615,
		_616,
		_617,
		_618,
		_619,
		_620,
		_621,
		_622,
		_623,
		_624,
		_625,
		_626,
		_627,
		_628,
		_629,
		_630,
		_631,
		_632,
		_633,
		_634,
		_635,
		_636,
		_637,
		_638,
		_639,
		_640,
		_641,
		_642,
		_643,
		_644,
		_645,
		_646,
		_647,
		_648,
		_649,
		_650,
		_651,
		_652,
		_653,
		_654,
		_655,
		_656,
		_657,
		_658,
		_659,
		_660,
		_661,
		_662,
		_663,
		_664,
		_665,
		_666,
		_667,
		_668,
		_669,
		_670,
		_671,
		_672,
		_673,
		_674,
		_675,
		_676,
		_677,
		_678,
		_679,
		_680,
		_681,
		_682,
		_683,
		_684,
		_685,
		_686,
		_687,
		_688,
		_689,
		_690,
		_691,
		_692,
		_693,
		_694,
		_695,
		_696,
		_697,
		_698,
		_699
	}
	public enum CodeDME
	{
		NARROW,
		PRECISION,
		WIDE
	}
	public enum CodeDMEChannel
	{
		_1X,
		_1Y,
		_2X,
		_2Y,
		_3X,
		_3Y,
		_4X,
		_4Y,
		_5X,
		_5Y,
		_6X,
		_6Y,
		_7X,
		_7Y,
		_8X,
		_8Y,
		_9X,
		_9Y,
		_10X,
		_10Y,
		_11X,
		_11Y,
		_12X,
		_12Y,
		_13X,
		_13Y,
		_14X,
		_14Y,
		_15X,
		_15Y,
		_16X,
		_16Y,
		_17X,
		_17Y,
		_17Z,
		_18X,
		_18W,
		_18Y,
		_18Z,
		_19X,
		_19Y,
		_19Z,
		_20X,
		_20W,
		_20Y,
		_20Z,
		_21X,
		_21Y,
		_21Z,
		_22X,
		_22W,
		_22Y,
		_22Z,
		_23X,
		_23Y,
		_23Z,
		_24X,
		_24W,
		_24Y,
		_24Z,
		_25X,
		_25Y,
		_25Z,
		_26X,
		_26W,
		_26Y,
		_26Z,
		_27X,
		_27Y,
		_27Z,
		_28X,
		_28W,
		_28Y,
		_28Z,
		_29X,
		_29Y,
		_29Z,
		_30X,
		_30W,
		_30Y,
		_30Z,
		_31X,
		_31Y,
		_31Z,
		_32X,
		_32W,
		_32Y,
		_32Z,
		_33X,
		_33Y,
		_33Z,
		_34X,
		_34W,
		_34Y,
		_34Z,
		_35X,
		_35Y,
		_35Z,
		_36X,
		_36W,
		_36Y,
		_36Z,
		_37X,
		_37Y,
		_37Z,
		_38X,
		_38W,
		_38Y,
		_38Z,
		_39X,
		_39Y,
		_39Z,
		_40X,
		_40W,
		_40Y,
		_40Z,
		_41X,
		_41Y,
		_41Z,
		_42X,
		_42W,
		_42Y,
		_42Z,
		_43X,
		_43Y,
		_43Z,
		_44X,
		_44W,
		_44Y,
		_44Z,
		_45X,
		_45Y,
		_45Z,
		_46X,
		_46W,
		_46Y,
		_46Z,
		_47X,
		_47Y,
		_47Z,
		_48X,
		_48W,
		_48Y,
		_48Z,
		_49X,
		_49Y,
		_49Z,
		_50X,
		_50W,
		_50Y,
		_50Z,
		_51X,
		_51Y,
		_51Z,
		_52X,
		_52W,
		_52Y,
		_52Z,
		_53X,
		_53Y,
		_53Z,
		_54X,
		_54W,
		_54Y,
		_54Z,
		_55X,
		_55Y,
		_55Z,
		_56X,
		_56W,
		_56Y,
		_56Z,
		_57X,
		_57Y,
		_58X,
		_58Y,
		_59X,
		_59Y,
		_60X,
		_60Y,
		_61X,
		_61Y,
		_62X,
		_62Y,
		_63X,
		_63Y,
		_64X,
		_64Y,
		_65X,
		_65Y,
		_66X,
		_66Y,
		_67X,
		_67Y,
		_68X,
		_68Y,
		_69X,
		_69Y,
		_70X,
		_70Y,
		_71X,
		_71Y,
		_72X,
		_72Y,
		_73X,
		_73Y,
		_74X,
		_74Y,
		_75X,
		_75Y,
		_76X,
		_76Y,
		_77X,
		_77Y,
		_78X,
		_78Y,
		_79X,
		_79Y,
		_80X,
		_80Y,
		_80Z,
		_81X,
		_81Y,
		_81Z,
		_82X,
		_82Y,
		_82Z,
		_83X,
		_83Y,
		_83Z,
		_84X,
		_84Y,
		_84Z,
		_85X,
		_85Y,
		_85Z,
		_86X,
		_86Y,
		_86Z,
		_87X,
		_87Y,
		_87Z,
		_88X,
		_88Y,
		_88Z,
		_89X,
		_89Y,
		_89Z,
		_90X,
		_90Y,
		_90Z,
		_91X,
		_91Y,
		_91Z,
		_92X,
		_92Y,
		_92Z,
		_93X,
		_93Y,
		_93Z,
		_94X,
		_94Y,
		_94Z,
		_95X,
		_95Y,
		_95Z,
		_96X,
		_96Y,
		_96Z,
		_97X,
		_97Y,
		_97Z,
		_98X,
		_98Y,
		_98Z,
		_99X,
		_99Y,
		_99Z,
		_100X,
		_100Y,
		_100Z,
		_101X,
		_101Y,
		_101Z,
		_102X,
		_102Y,
		_102Z,
		_103X,
		_103Y,
		_103Z,
		_104X,
		_104Y,
		_104Z,
		_105X,
		_105Y,
		_105Z,
		_106X,
		_106Y,
		_106Z,
		_107X,
		_107Y,
		_107Z,
		_108X,
		_108Y,
		_108Z,
		_109X,
		_109Y,
		_109Z,
		_110X,
		_110Y,
		_110Z,
		_111X,
		_111Y,
		_111Z,
		_112X,
		_112Y,
		_112Z,
		_113X,
		_113Y,
		_113Z,
		_114X,
		_114Y,
		_114Z,
		_115X,
		_115Y,
		_115Z,
		_116X,
		_116Y,
		_116Z,
		_117X,
		_117Y,
		_117Z,
		_118X,
		_118Y,
		_118Z,
		_119X,
		_119Y,
		_119Z,
		_120X,
		_120Y,
		_121X,
		_121Y,
		_122X,
		_122Y,
		_123X,
		_123Y,
		_124X,
		_124Y,
		_125X,
		_125Y,
		_126X,
		_126Y
	}
	public enum CodeILSBackCourse
	{
		YES,
		NO,
		RSTR
	}
	public enum CodeMarkerBeaconSignal
	{
		FAN,
		LOW_PWR_FAN,
		Z,
		BONES
	}
	public enum CodeNavaidService
	{
		VOR,
		DME,
		NDB,
		TACAN,
		MKR,
		ILS,
		ILS_DME,
		MLS,
		MLS_DME,
		VORTAC,
		VOR_DME,
		NDB_DME,
		TLS,
		LOC,
		LOC_DME,
		NDB_MKR,
		DF,
		SDF
	}
	public enum CodeNavaidPurpose
	{
		TERMINAL,
		ENROUTE,
		ALL
	}
	public enum CodeSignalPerformanceILS
	{
		I,
		II,
		III
	}
	public enum CodeCourseQualityILS
	{
		A,
		B,
		C,
		D,
		E,
		T
	}
	public enum CodeIntegrityLevelILS
	{
		_1,
		_2,
		_3,
		_4
	}
	public enum CodePositionInILS
	{
		OUTER,
		MIDDLE,
		NNER,
		BACKCOURSE
	}
	public enum CodeCheckpointCategory
	{
		A,
		G
	}
	public enum CodeNDBUsage
	{
		ENR,
		L,
		MAR
	}
	public enum CodeEmissionBand
	{
		U,
		H,
		M
	}
	public enum CodeSpecialNavigationStation
	{
		MASTER,
		SLAVE,
		RED_SLAVE,
		GREEN_SLAVE,
		PURPLE_SLAVE
	}
	public enum CodeVOR
	{
		VOR,
		DVOR,
		VOT
	}
	public enum CodeTACANChannel
	{
		_1X,
		_1Y,
		_2X,
		_2Y,
		_3X,
		_3Y,
		_4X,
		_4Y,
		_5X,
		_5Y,
		_6X,
		_6Y,
		_7X,
		_7Y,
		_8X,
		_8Y,
		_9X,
		_9Y,
		_10X,
		_10Y,
		_11X,
		_11Y,
		_12X,
		_12Y,
		_13X,
		_13Y,
		_14X,
		_14Y,
		_15X,
		_15Y,
		_16X,
		_16Y,
		_17X,
		_17Y,
		_17Z,
		_18X,
		_18W,
		_18Y,
		_18Z,
		_19X,
		_19Y,
		_19Z,
		_20X,
		_20W,
		_20Y,
		_20Z,
		_21X,
		_21Y,
		_21Z,
		_22X,
		_22W,
		_22Y,
		_22Z,
		_23X,
		_23Y,
		_23Z,
		_24X,
		_24W,
		_24Y,
		_24Z,
		_25X,
		_25Y,
		_25Z,
		_26X,
		_26W,
		_26Y,
		_26Z,
		_27X,
		_27Y,
		_27Z,
		_28X,
		_28W,
		_28Y,
		_28Z,
		_29X,
		_29Y,
		_29Z,
		_30X,
		_30W,
		_30Y,
		_30Z,
		_31X,
		_31Y,
		_31Z,
		_32X,
		_32W,
		_32Y,
		_32Z,
		_33X,
		_33Y,
		_33Z,
		_34X,
		_34W,
		_34Y,
		_34Z,
		_35X,
		_35Y,
		_35Z,
		_36X,
		_36W,
		_36Y,
		_36Z,
		_37X,
		_37Y,
		_37Z,
		_38X,
		_38W,
		_38Y,
		_38Z,
		_39X,
		_39Y,
		_39Z,
		_40X,
		_40W,
		_40Y,
		_40Z,
		_41X,
		_41Y,
		_41Z,
		_42X,
		_42W,
		_42Y,
		_42Z,
		_43X,
		_43Y,
		_43Z,
		_44X,
		_44W,
		_44Y,
		_44Z,
		_45X,
		_45Y,
		_45Z,
		_46X,
		_46W,
		_46Y,
		_46Z,
		_47X,
		_47Y,
		_47Z,
		_48X,
		_48W,
		_48Y,
		_48Z,
		_49X,
		_49Y,
		_49Z,
		_50X,
		_50W,
		_50Y,
		_50Z,
		_51X,
		_51Y,
		_51Z,
		_52X,
		_52W,
		_52Y,
		_52Z,
		_53X,
		_53Y,
		_53Z,
		_54X,
		_54W,
		_54Y,
		_54Z,
		_55X,
		_55Y,
		_55Z,
		_56X,
		_56W,
		_56Y,
		_56Z,
		_57X,
		_57Y,
		_58X,
		_58Y,
		_59X,
		_59Y,
		_60X,
		_60Y,
		_61X,
		_61Y,
		_62X,
		_62Y,
		_63X,
		_63Y,
		_64X,
		_64Y,
		_65X,
		_65Y,
		_66X,
		_66Y,
		_67X,
		_67Y,
		_68X,
		_68Y,
		_69X,
		_69Y,
		_70X,
		_70Y,
		_71X,
		_71Y,
		_72X,
		_72Y,
		_73X,
		_73Y,
		_74X,
		_74Y,
		_75X,
		_75Y,
		_76X,
		_76Y,
		_77X,
		_77Y,
		_78X,
		_78Y,
		_79X,
		_79Y,
		_80X,
		_80Y,
		_80Z,
		_81X,
		_81Y,
		_81Z,
		_82X,
		_82Y,
		_82Z,
		_83X,
		_83Y,
		_83Z,
		_84X,
		_84Y,
		_84Z,
		_85X,
		_85Y,
		_85Z,
		_86X,
		_86Y,
		_86Z,
		_87X,
		_87Y,
		_87Z,
		_88X,
		_88Y,
		_88Z,
		_89X,
		_89Y,
		_89Z,
		_90X,
		_90Y,
		_90Z,
		_91X,
		_91Y,
		_91Z,
		_92X,
		_92Y,
		_92Z,
		_93X,
		_93Y,
		_93Z,
		_94X,
		_94Y,
		_94Z,
		_95X,
		_95Y,
		_95Z,
		_96X,
		_96Y,
		_96Z,
		_97X,
		_97Y,
		_97Z,
		_98X,
		_98Y,
		_98Z,
		_99X,
		_99Y,
		_99Z,
		_100X,
		_100Y,
		_100Z,
		_101X,
		_101Y,
		_101Z,
		_102X,
		_102Y,
		_102Z,
		_103X,
		_103Y,
		_103Z,
		_104X,
		_104Y,
		_104Z,
		_105X,
		_105Y,
		_105Z,
		_106X,
		_106Y,
		_106Z,
		_107X,
		_107Y,
		_107Z,
		_108X,
		_108Y,
		_108Z,
		_109X,
		_109Y,
		_109Z,
		_110X,
		_110Y,
		_110Z,
		_111X,
		_111Y,
		_111Z,
		_112X,
		_112Y,
		_112Z,
		_113X,
		_113Y,
		_113Z,
		_114X,
		_114Y,
		_114Z,
		_115X,
		_115Y,
		_115Z,
		_116X,
		_116Y,
		_116Z,
		_117X,
		_117Y,
		_117Z,
		_118X,
		_118Y,
		_118Z,
		_119X,
		_119Y,
		_119Z,
		_120X,
		_120Y,
		_121X,
		_121Y,
		_122X,
		_122Y,
		_123X,
		_123Y,
		_124X,
		_124Y,
		_125X,
		_125Y,
		_126X,
		_126Y
	}
	public enum CodeSpecialNavigationSystem
	{
		LORANA,
		LORANC,
		LORAND,
		DECCA,
		GNSS
	}
	public enum CodeStatusNavaid
	{
		OPERATIONAL,
		UNSERVICEABLE,
		ONTEST,
		INTERRUPT,
		PARTIAL,
		CONDITIONAL,
		FALSE_INDICATION,
		FALSE_POSSIBLE,
		DISPLACED,
		IN_CONSTRUCTION
	}
	public enum CodeDesignatedPoint
	{
		ICAO,
		COORD,
		CNF,
		DESIGNED,
		MTR,
		TERMINAL,
		BRG_DIST
		//,OTHER_VF
	}
	public enum CodeAirspacePointRole
	{
		ENTRY,
		EXIT,
		ENTRY_EXIT
	}
	public enum CodeAirspacePointPosition
	{
		IN,
		OUT,
		BORDER
	}
	public enum CodeGroundLighting
	{
		BCN,
		IBN,
		HEL_BCN,
		ABN,
		MAR_BCN,
		RSP_BCN,
		TWR_BCN,
		HAZ_BCN
	}
	public enum CodeNotePurpose
	{
		DESCRIPTION,
		REMARK,
		WARNING,
		DISCLAIMER
#if QATAR
        , OTHER_CLASS
#endif
	    ,OTHER_OBS_AREA1_ELIST_AVAILABILITY // Require for AIXM 2 AIP mapping for section ENR 5.4 (For any AIXM2AIP questions contact EminS@risk.az), other sections below:
	    ,OTHER_PAX_FAC_FURTHER_DETAILS // AD 2.5
	    ,OTHER_FIRE_SERVICE_DESC // AD 2.6
	    ,OTHER_FIRE_EQPT_DESC // AD 2.6
	    ,OTHER_RESCUE_SERVICE_DESC // AD 2.6
	    ,OTHER_RESCUE_EQPT_DESC // AD 2.6
	    ,OTHER_SEASONAL_AVAILABILITY // AD 2.7
	    ,OTHER_OBS_AREA2_ELIST_AVAILABILITY //  AD 2.10
	    ,OTHER_OBS_AREA3_ELIST_AVAILABILITY //  AD 2.10
	    ,OTHER_BRIEFING_CONSULTATION_METHOD //  AD 2.11
	    ,OTHER_WING_BAR_DESC //  AD 2.14
        ,OTHER_HELIPORT_TLOF_AREA_TYPE // AD 3.12
        ,OTHER_ICAO_DOC_BASIS // GEN 3.2
        ,OTHER_HOURS_OF_SERVICE // GEN 3.4
        ,OTHER_SIGMET_VALIDITY_PERIODS // GEN 3.5
        ,OTHER_AIRSPACE_ADIZ_ACTIVATION_PROCS // ENR 5.2
        ,OTHER_ADIZ_INTERCEPTION_RISK // ENR 5.2
        ,OTHER_AIRSPACE_ADVISORY_MEASURES // ENR 5.3
    }
	public enum CodeOrganisation
	{
		STATE,
		STATE_GROUP,
		ORG,
		INTL_ORG,
		ACFT_OPR,
		HANDLING_AGENCY,
		NTL_AUTH,
		ATS,
		COMMERCIAL,
	    // Added By EminS for AIP
	    // Basis is: Eurocontrol specification AIPtoAIXM5.1Mapping.doc
        OTHER_TELECOM_NAV_REGULATOR
    }
    public enum CodeOrganisationHierarchy
	{
		MEMBER,
		OWNED_BY,
		SUPERVISED_BY
	}
	public enum CodeUnit
	{
		ACC,
		ADSU,
		ADVC,
		ALPS,
		AOF,
		APP,
		APP_ARR,
		APP_DEP,
		ARO,
		ATCC,
		ATFMU,
		ATMU,
		ATSU,
		BOF,
		BS,
		COM,
		FCST,
		FIC,
		GCA,
		MET,
		MWO,
		NOF,
		OAC,
		PAR,
		RAD,
		RAFC,
		RCC,
		RSC,
		SAR,
		SMC,
		SMR,
		SRA,
		SSR,
		TAR,
		TWR,
		UAC,
		UDF,
		UIC,
		VDF,
		WAFC,
		ARTCC,
		FSS,
		TRACON,
		MIL,
		MILOPS,
	    // Added By EminS for AIP
	    // Basis is: Eurocontrol specification AIPtoAIXM5.1Mapping.doc
        OTHER_CARTO,
        OTHER_AERO_CHART_SALES_UNIT,
        OTHER_TOPO_CHART_SALES_UNIT
    }
	public enum CodeUnitDependency
	{
		OWNER,
		PROVIDER,
		ALTERNATE
	}
	public enum CodeVerticalStructure
	{
		AG_EQUIP,
		ANTENNA,
		ARCH,
		BRIDGE,
		BUILDING,
		CABLE_CAR,
		CATENARY,
		COMPRESSED_AIR_SYSTEM,
		CONTROL_MONITORING_SYSTEM,
		CONTROL_TOWER,
		COOLING_TOWER,
		CRANE,
		DAM,
		DOME,
		ELECTRICAL_EXIT_LIGHT,
		ELECTRICAL_SYSTEM,
		ELEVATOR,
		FENCE,
		FUEL_SYSTEM,
		GATE,
		GENERAL_UTILITY,
		GRAIN_ELEVATOR,
		HEAT_COOL_SYSTEM,
		INDUSTRIAL_SYSTEM,
		LIGHTHOUSE,
		MONUMENT,
		NATURAL_GAS_SYSTEM,
		NATURAL_HIGHPOINT,
		NAVAID,
		NUCLEAR_REACTOR,
		POLE,
		POWER_PLANT,
		REFINERY,
		RIG,
		SALTWATER_SYSTEM,
		SIGN,
		SPIRE,
		STACK,
		STADIUM,
		STORM_SYSTEM,
		TANK,
		TETHERED_BALLOON,
		TOWER,
		TRAMWAY,
		TRANSMISSION_LINE,
		TREE,
		URBAN,
		VEGETATION,
		WALL,
		WASTEWATER_SYSTEM,
		WATER_SYSTEM,
		WATER_TOWER,
		WINDMILL,
		WINDMILL_FARMS
	}
	public enum CodeStatusConstruction
	{
		IN_CONSTRUCTION,
		COMPLETED,
		DEMOLITION_PLANNED,
		IN_DEMOLITION
	}
	public enum CodeVerticalStructureMarking
	{
		MONOCOLOUR,
		CHEQUERED,
		HBANDS,
		VBANDS,
		FLAG,
		MARKERS
	}
	public enum CodeVerticalStructureMaterial
	{
		ADOBE_BRICK,
		ALUMINIUM,
		BRICK,
		CONCRETE,
		FIBREGLASS,
		GLASS,
		IRON,
		MASONRY,
		METAL,
		MUD,
		PLANT,
		PRESTRESSED_CONCRETE,
		REINFORCED_CONCRETE,
		SOD,
		STEEL,
		STONE,
		TREATED_TIMBER,
		WOOD
	}
	public enum CodeObstacleArea
	{
		AREA1,
		AREA2,
		AREA3,
		AREA4,
		OLS,
		FAR77,
		MANAGED,
        OTHER_INNERHORIZONTAL,
        OTHER_CONICAL,
        OTHER_OUTERHORIZONTAL,
        OTHER_APPROACH,
        OTHER_INNERAPPROACH,
        OTHER_TAKEOFFCLIMB,
        OTHER_STRIP,
        OTHER_TRANSITIONAL,
        OTHER_INNERTRANSITIONAL,
        OTHER_BALKEDLANDING,
        OTHER_AREA2A,
        OTHER_TAKEOFFFLIGHTPATHAREA,
        OTHER_AREA2B,
        OTHER_AREA2C,
        OTHER_AREA2D,

	}
	public enum CodeMinimumAltitude
	{
		OCA,
		DA,
		MDA
	}
	public enum CodeMinimumHeight
	{
		DH,
		OCH,
		MDH
	}
	public enum CodeHeightReference
	{
		HAT,
		HAA,
		HAL,
		HAS
	}
	public enum CodeEquipmentUnavailable
	{
		STANDARD,
		NONSTANDARD
	}
	public enum CodeApproach
	{
		ASR,
		ARA,
		ARSR,
		PAR,
		ILS,
		ILS_DME,
		ILS_PRM,
		LDA,
		LDA_DME,
		LOC,
		LOC_BC,
		LOC_DME,
		LOC_DME_BC,
		MLS,
		MLS_DME,
		NDB,
		NDB_DME,
		SDF,
		TLS,
		VOR,
		VOR_DME,
		TACAN,
		VORTAC,
		DME,
		DME_DME,
		RNP,
		GPS,
		GLONASS,
		GALILEO,
		RNAV,
		IGS
	}
	public enum CodeTAA
	{
		LEFT_BASE,
		RIGHT_BASE,
		STRAIGHT_IN
	}
	public enum CodeProcedureDistance
	{
		HAT,
		OM,
		MM,
		IM,
		PFAF,
		GSANT,
		FAF,
		MAP,
		THLD,
		VDP,
		RECH,
		OTHER_SDF
	}
	public enum CodeApproachPrefix
	{
		HI,
		COPTER,
		CONVERGING
	}
	public enum CodeUpperAlpha
	{
		A,
		B,
		C,
		D,
		E,
		F,
		G,
		H,
		I,
		J,
		K,
		L,
		M,
		N,
		O,
		P,
		Q,
		R,
		S,
		T,
		U,
		V,
		W,
		X,
		Y,
		Z
	}
	public enum CodeApproachEquipmentAdditional
	{
		ADF,
		DME,
		RADAR,
		RADARDME,
		DUALVORDME,
		DUALADF,
		ADFMA,
		SPECIAL,
		DUALVHF,
		GPSRNP3,
		ADFILS,
		DUALADF_DME,
		RADAR_RNAV
	}
	public enum CodeMinimaFinalApproachPath
	{
		STRAIGHT_IN,
		CIRCLING,
		SIDESTEP
	}
	public enum CodeNavigationArea
	{
		PNA,
		OMNI,
		DVA
	}
	public enum CodeProcedureAvailability
	{
		USABLE,
		UNUSABLE
	}
	public enum CodeDesignStandard
	{
		PANS_OPS,
		TERPS,
		CANADA_TERPS,
		NATO
	}
	public enum CodeProcedureCodingStandard
	{
		PANS_OPS,
		ARINC_424_15,
		ARINC_424_18,
		ARINC_424_19
	}
	public enum CodeNavigationAreaRestriction
	{
		VECTOR,
		OMNIDIRECTIONAL
	}
	public enum CodeSegmentTermination
	{
		ALTITUDE,
		DISTANCE,
		DURATION,
		INTERCEPT
	}
	public enum CodeTrajectory
	{
		STRAIGHT,
		ARC,
		PT,
		BASETURN,
		HOLDING
	}
	public enum CodeSegmentPath
	{
		AF,
		HF,
		HA,
		HM,
		IF,
		PI,
		PT,
		TF,
		CA,
		CD,
		CI,
		CR,
		CF,
		DF,
		FA,
		FC,
		FT,
		FM,
		VM,
		FD,
		VR,
		VD,
		VI,
		VA,
		RF
	}
	public enum CodeCourse
	{
		TRUE_TRACK,
		MAG_TRACK,
		TRUE_BRG,
		MAG_BRG,
		HDG,
		RDL
	}
	public enum CodeSpeedReference
	{
		IAS,
		TAS,
		GS
	}
	public enum CodeProcedurePhase
	{
		RWY,
		COMMON,
		EN_ROUTE,
		APPROACH,
		FINAL,
		MISSED,
		MISSED_P,
		MISSED_S,
		ENGINE_OUT
	}
	public enum CodeHoldingUse
	{
		PT,
		ARRIVAL,
		MISSED_APPROACH,
		CLIMB,
		ATC
	}
	public enum CodeFinalGuidance
	{
		LPV,
		LNAV_VNAV,
		LNAV,
		GLS,
		ASR,
		ARA,
		ARSR,
		PAR,
		ILS,
		ILS_DME,
		ILS_PRM,
		LDA,
		LDA_DME,
		LOC,
		LOC_BC,
		LOC_DME,
		LOC_DME_BC,
		MLS,
		MLS_DME,
		NDB,
		NDB_DME,
		SDF,
		TLS,
		VOR,
		VOR_DME,
		TACAN,
		VORTAC,
		DME,
		LP
	}
	public enum CodeRelativePosition
	{
		BEFORE,
		AT,
		AFTER
	}
	public enum CodeMissedApproach
	{
		PRIMARY,
		SECONDARY,
		ALTERNATE,
		TACAN,
		TACANALT,
		ENGINEOUT
	}
	public enum CodeSafeAltitude
	{
		MSA,
		ESA
	}
	public enum CodeHoldingUsage
	{
		ENR,
		TER
	}
	public enum CodeApproval
	{
		APPROVED,
		DISAPPROVED,
		NOT_AUTHORISED,
		RESTRICTED
	}
	public enum CodeFlowConditionOperation
	{
		AND,
		ANDNOT,
		OR,
		SEQ,
		NONE
	}
	public enum CodeLocationQualifier
	{
		DEP,
		XNG,
		ARR,
		ACT,
		AVBL
	}
	public enum CodeFlightRestriction
	{
		FORBID,
		MANDATORY,
		CLSD,
		ALLOWED
	}
	public enum CodeComparison
	{
		LESS,
		LESS_OR_EQUAL,
		EQUAL,
		GREATER_OR_EQUAL,
		GREATER
	}
	public enum CodeLevel
	{
		UPPER,
		LOWER,
		BOTH
	}
	public enum CodeRouteSegmentPath
	{
		GRC,
		RHL,
		GDS
	}
	public enum CodeRouteNavigation
	{
		CONV,
		RNAV,
		TACAN
	}
	public enum CodeRouteDesignatorSuffix
	{
		F,
		G
	}
	public enum CodeRouteAvailability
	{
		OPEN,
		COND,
		CLSD
	}
	public enum CodeRouteDesignatorPrefix
	{
		K,
		U,
		S,
		T
	}
	public enum CodeRouteDesignatorLetter
	{
		A,
		B,
		G,
		H,
		J,
		L,
		M,
		N,
		P,
		Q,
		R,
		T,
		V,
		W,
		Y,
		Z
	}
	public enum CodeRoute
	{
		ATS,
		NAT
	}
	public enum CodeRouteOrigin
	{
		INTL,
		DOM,
		BOTH
	}
	public enum CodeMilitaryTraining
	{
		IR,
		VR,
		SR
	}
	public enum CodeAerialRefuellingPrefix
	{
		AR
	}
	public enum CodeAerialRefuelling
	{
		TRACK,
		ANCHOR,
		BOTH
	}
	public enum CodeAerialRefuellingPoint
	{
		INITIAL,
		CONTROL,
		CHECK,
		EXIT,
		ENTRY,
		ANCHOR,
		PATTERN
	}
	public enum CodeRuleProcedure
	{
		RULE,
		LAW,
		PROCEDURE,
		PRACTICE,
		ICAO_DIFF
	}
	public enum CodeRuleProcedureTitle
	{
        ENTRY_TRANSIT_DEPARTURE_OF_AIRCRAFT,
        ENTRY_TRANSIT_DEPARTURE_SCHEDULED,
        ENTRY_TRANSIT_DEPARTURE_NON_SCHEDULED,
        ENTRY_TRANSIT_DEPARTURE_PRIVATE,
        PUBLIC_HEALTH_MEASURES_AIRCRAFT,
        CUSTOMS_REQUIREMENTS,
        IMMIGRATION_REQUIREMENTS,
        PUBLIC_HEALTH_MEASURES_PASSENGERS,
        ENTRY_TRANSIT_DEPARTURE_CARGO,
        AIRCRAFT_INSTRUMENTS_EQUIPMENT_FLIGHT_DOCUMENTS,
        NATIONAL_REGULATIONS,
        INTERNATIONAL_AGREEMENTS_CONVENTIONS,
        DIFFERENCES_ICAO_STANDARDS_RECOMMENDED_PRACTICES_PROCEDURES,
        MEASURING_SYSTEM_AIRCRAFT_MARKINGS_HOLIDAYS,
        ABBREVIATIONS_AIS_PUBLICATIONS,
        AERODROME_HELIPORT_CHARGES,
	    [Description("GEN 4.2, AIR_NAVIGATION_SERVICES_CHARGES")]
	    AIR_NAVIGATION_SERVICES_CHARGES,
        FLIGHT_RULES_GENERAL,
	    [Description("ENR 1.2, VISUAL_FLIGHT_RULES")]
	    VISUAL_FLIGHT_RULES,
        [Description("ENR 1.3, INSTRUMENT_FLIGHT_RULES")]
	    INSTRUMENT_FLIGHT_RULES,
        [Description("ENR 1.4.1, ATS_AIRSPACE_CLASSIFICATION")]
	    ATS_AIRSPACE_CLASSIFICATION,
        HOLDING_APPROACH_DEPARTURE_PROCEDURES,
        ATS_SURVEILLANCE_SERVICES_PROCEDURES,
	    [Description("ENR 1.7, ALTIMETER_SETTING_PROCEDURES")]
	    ALTIMETER_SETTING_PROCEDURES,
	    [Description("ENR 1.8, REGIONAL_SUPPLEMENTARY_PROCEDURES")]
	    REGIONAL_SUPPLEMENTARY_PROCEDURES,
        AIR_TRAFFIC_FLOW_MANAGEMENT,
	    [Description("ENR 1.10, FLIGHT_PLANNING")]
	    FLIGHT_PLANNING,
        ADDRESSING_FLIGHT_PLAN_MESSAGES,
        INTERCEPTION_CIVIL_AIRCRAFT,
	    [Description("ENR 1.13, UNLAWFUL_INTERFERENCE")]
	    UNLAWFUL_INTERFERENCE,
        [Description("ENR 1.14, AIR_TRAFFIC_INCIDENTS")]
	    AIR_TRAFFIC_INCIDENTS,
        AERODROME_HELIPORT_AVAILABILITY,
        LOCAL_TRAFFIC_REGULATIONS,
	    [Description("AD 2.21, AD 3.20, NOISE_ABATEMENT_PROCEDURES")]
	    NOISE_ABATEMENT_PROCEDURES,
        AERODROME_FLIGHT_PROCEDURES,
        AERODROME_BIRD_CONCENTRATION,
        // Added By EminS for AIP
        // Basis is: Eurocontrol specification AIPtoAIXM5.1Mapping.doc
        // Basis is: LGS Fix Excel table
        [Description("GEN 0.1, OTHER:PREFACE")]
        OTHER_PREFACE,
        [Description("GEN 0.5, OTHER:LIST_OF_HAND_AMENDMENTS_TO_THE_AIP")]
        OTHER_LIST_OF_HAND_AMENDMENTS_TO_THE_AIP,
        [Description("GEN 1.2, OTHER:ENTRY_TRANSIT_AND_DEPARTURE_OF_AIRCRAFT")]
        OTHER_ENTRY_TRANSIT_AND_DEPARTURE_OF_AIRCRAFT,
        [Description("GEN 1.3, OTHER:ENTRY_TRANSIT_AND_DEPARTURE_OF_PASSENGERS_AND_CREW")]
        OTHER_ENTRY_TRANSIT_AND_DEPARTURE_OF_PASSENGERS_AND_CREW,
        [Description("GEN 1.4, OTHER:ENTRY_TRANSIT_AND_DEPARTURE_OF_CARGO")]
        OTHER_ENTRY_TRANSIT_AND_DEPARTURE_OF_CARGO,
        [Description("GEN 1.5, OTHER:AIRCRAFT_INSTRUMENTS_EQUIPMENT_AND_FLIGHT_DOCUMENTS")]
        OTHER_AIRCRAFT_INSTRUMENTS_EQUIPMENT_AND_FLIGHT_DOCUMENTS,
        [Description("GEN 1.6, OTHER:SUMMARY_OF_NATIONAL_REGULATIONS_AND_INTERNATIONAL_AGREEMENTS_CONVENTIONS")]
        OTHER_SUMMARY_OF_NATIONAL_REGULATIONS_AND_INTERNATIONAL_AGREEMENTS_CONVENTIONS,
        [Description("GEN 1.7, OTHER:DIFFERENCES_FROM_ICAO_STANDARDS_RECOMMENDED_PRACTICES_AND_PROCEDURES")]
        OTHER_DIFFERENCES_FROM_ICAO_STANDARDS_RECOMMENDED_PRACTICES_AND_PROCEDURES,
        [Description("GEN 2.1.1, OTHER:UNITS_OF_MEASUREMENT")]
        OTHER_UNITS_OF_MEASUREMENT,
        [Description("GEN 2.1.2, OTHER:TEMPORAL_REFERENCE_SYSTEM")]
        OTHER_TEMPORAL_REFERENCE_SYSTEM,
        [Description("GEN 2.1.3, OTHER:HORIZONTAL_REFERENCE_SYSTEM")]
        OTHER_HORIZONTAL_REFERENCE_SYSTEM,
        [Description("GEN 2.1.4, OTHER:VERTICAL_REFERENCE_SYSTEM")]
        OTHER_VERTICAL_REFERENCE_SYSTEM,
        [Description("GEN 2.1.5, OTHER:AIRCRAFT_NATIONALITY_AND_REGISTRATION_MARKS")]
        OTHER_AIRCRAFT_NATIONALITY_AND_REGISTRATION_MARKS,
        [Description("GEN 2.2, OTHER:ABBREVIATIONS_USED_IN_AIS_PUBLICATIONS")]
        OTHER_ABBREVIATIONS_USED_IN_AIS_PUBLICATIONS,
        [Description("GEN 2.3, OTHER:CHART_SYMBOLS")]
        OTHER_CHART_SYMBOLS,
	    [Description("GEN 2.6, OTHER:CONVERSION_OF_UNITS_OF_MEASUREMENT")]
	    OTHER_CONVERSION_OF_UNITS_OF_MEASUREMENT,
        [Description("GEN 2.7, OTHER:SUNRISE_SUNSET")]
        OTHER_SUNRISE_SUNSET,
        [Description("GEN 3.1.1, OTHER:RESPONSIBLE_SERVICE")]
        OTHER_RESPONSIBLE_SERVICE,
        [Description("GEN 3.1.2, OTHER:AREA_OF_RESPONSIBILITY")]
        OTHER_GEN_3_1_2_AREA_OF_RESPONSIBILITY,
        [Description("GEN 3.1.3, OTHER:AERONAUTICAL_PUBLICATIONS")]
        OTHER_AERONAUTICAL_PUBLICATIONS,
        [Description("GEN 3.1.4, OTHER:AIRAC_SYSTEM")]
        OTHER_AIRAC_SYSTEM,
        [Description("GEN 3.1.5, OTHER:PRE_FLIGHT_INFORMATION_SERVICE_AT_AERODROMES_HELIPORTS")]
        OTHER_PRE_FLIGHT_INFORMATION_SERVICE_AT_AERODROMES_HELIPORTS,
        [Description("GEN 3.2.2, OTHER:MAINTENANCE_OF_CHARTS")]
        OTHER_MAINTENANCE_OF_CHARTS,
        [Description("GEN 3.2.4, OTHER:AERONAUTICAL_CHART_SERIES_AVAILABLE")]
        OTHER_AERONAUTICAL_CHART_SERIES_AVAILABLE,
        [Description("GEN 3.2.5, OTHER:LIST_OF_AERONAUTICAL_CHARTS_AVAILABLE")]
        OTHER_LIST_OF_AERONAUTICAL_CHARTS_AVAILABLE,
        [Description("GEN 3.2.6, OTHER:INDEX_TO_THE_WORLD_AERONAUTICAL_CHART_WAC")]
        OTHER_INDEX_TO_THE_WORLD_AERONAUTICAL_CHART_WAC,
        [Description("GEN 3.2.8, OTHER:CORRECTIONS_TO_CHARTS_NOT_CONTAINED_IN_THE_AIP")]
        OTHER_CORRECTIONS_TO_CHARTS_NOT_CONTAINED_IN_THE_AIP,
        [Description("GEN 3.3.2, OTHER:AREA_OF_RESPONSIBILITY")]
        OTHER_GEN_3_3_2_AREA_OF_RESPONSIBILITY,
        [Description("GEN 3.3.3, OTHER:TYPES_OF_SERVICES")]
        OTHER_GEN_3_3_3_TYPES_OF_SERVICES,
        [Description("GEN 3.3.4, OTHER:COORDINATION_BETWEEN_THE_OPERATOR_AND_ATS")]
        OTHER_COORDINATION_BETWEEN_THE_OPERATOR_AND_ATS,
        [Description("GEN 3.3.5, OTHER:MINIMUM_FLIGHT_ALTITUDE")]
        OTHER_MINIMUM_FLIGHT_ALTITUDE,
        [Description("GEN 3.4.2, OTHER:AREA_OF_RESPONSIBILITY")]
        OTHER_GEN_3_4_2_AREA_OF_RESPONSIBILITY,
        [Description("GEN 3.4.3, OTHER:TYPES_OF_SERVICES")]
        OTHER_GEN_3_4_3_TYPES_OF_SERVICES,
        [Description("GEN 3.4.4, OTHER:REQUIREMENTS_AND_CONDITIONS")]
        OTHER_REQUIREMENTS_AND_CONDITIONS,
        [Description("GEN 3.4.5, OTHER:MISCELLANEOUS")]
        OTHER_MISCELLANEOUS,
        [Description("GEN 3.5.2, OTHER:AREA_OF_RESPONSIBILITY")]
        OTHER_GEN_3_5_2_AREA_OF_RESPONSIBILITY,
        [Description("GEN 3.5.3, OTHER:METEOROLOGICAL_OBSERVATIONS_AND_REPORTS")]
        OTHER_METEOROLOGICAL_OBSERVATIONS_AND_REPORTS,
        [Description("GEN 3.5.4, OTHER:TYPES_OF_SERVICES")]
        OTHER_GEN_3_5_4_TYPES_OF_SERVICES,
        [Description("GEN 3.5.5, OTHER:NOTIFICATION_REQUIRED_FROM_OPERATORS")]
        OTHER_NOTIFICATION_REQUIRED_FROM_OPERATORS,
        [Description("GEN 3.5.6, OTHER:AIRCRAFT_REPORTS")]
        OTHER_AIRCRAFT_REPORTS,
	    [Description("GEN 3.5.8, OTHER_SIGMET_AND_AIRMET_SERVICE")]
	    OTHER_SIGMET_AND_AIRMET_SERVICE,
        [Description("GEN 3.5.9, OTHER:OTHER_AUTOMATED_METEOROLOGICAL_SERVICES")]
        OTHER_OTHER_AUTOMATED_METEOROLOGICAL_SERVICES,
        [Description("GEN 3.6.2, OTHER:AREA_OF_RESPONSIBILITY")]
        OTHER_GEN_3_6_2_AREA_OF_RESPONSIBILITY,
        [Description("GEN 3.6.3, OTHER:TYPES_OF_SERVICES")]
        OTHER_GEN_3_6_3_TYPES_OF_SERVICES,
        [Description("GEN 3.6.4, OTHER:SEARCH_AND_RESCUE_AGREEMENTS")]
        OTHER_SEARCH_AND_RESCUE_AGREEMENTS,
        [Description("GEN 3.6.5, OTHER:CONDITIONS_OF_AVAILABILITY")]
        OTHER_CONDITIONS_OF_AVAILABILITY,
        [Description("GEN 3.6.6, OTHER:PROCEDURES_AND_SIGNALS_USED")]
        OTHER_PROCEDURES_AND_SIGNALS_USED,
        [Description("GEN 4.1, OTHER:AERODROME_HELIPORT_CHARGES")]
        OTHER_AERODROME_HELIPORT_CHARGES,
        [Description("ENR 1.1, OTHER:GENERAL_RULES")]
        OTHER_GENERAL_RULES,
        [Description("ENR 1.4.2, OTHER:ATS_AIRSPACE_DESCRIPTION")]
        OTHER_ATS_AIRSPACE_DESCRIPTION,
        [Description("ENR 1.5.1, OTHER:GENERAL")]
        OTHER_GENERAL,
        [Description("ENR 1.5.2, OTHER:ARRIVING_FLIGHTS")]
        OTHER_ARRIVING_FLIGHTS,
        [Description("ENR 1.5.3, OTHER:DEPARTING_FLIGHTS")]
        OTHER_DEPARTING_FLIGHTS,
        [Description("ENR 1.5.4, OTHER:OTHER_RELEVANT_INFORMATION_AND_PROCEDURES")]
        OTHER_ENR_1_5_4_OTHER_RELEVANT_INFORMATION_AND_PROCEDURES,
        [Description("ENR 1.6.1, OTHER:PRIMARY_RADAR")]
        OTHER_PRIMARY_RADAR,
        [Description("ENR 1.6.2, OTHER:SECONDARY_SURVEILLANCE_RADAR_SSR")]
        OTHER_SECONDARY_SURVEILLANCE_RADAR_SSR,
        [Description("ENR 1.6.3, OTHER:AUTOMATIC_DEPENDENT_SURVEILLANCE_BROADCAST_ADS_B")]
        OTHER_AUTOMATIC_DEPENDENT_SURVEILLANCE_BROADCAST_ADS_B,
        [Description("ENR 1.6.4, OTHER:OTHER_RELEVANT_INFORMATION_AND_PROCEDURES")]
        OTHER_ENR_1_6_4_OTHER_RELEVANT_INFORMATION_AND_PROCEDURES,
        [Description("ENR 1.9, OTHER:AIR_TRAFFIC_FLOW_MANAGEMENT_AND_AIRSPACE_MANAGEMENT")]
        OTHER_AIR_TRAFFIC_FLOW_MANAGEMENT_AND_AIRSPACE_MANAGEMENT,
        [Description("ENR 1.11, OTHER:ADRESSING_OF_FLIGHT_PLAN_MESSAGES")]
        OTHER_ADRESSING_OF_FLIGHT_PLAN_MESSAGES,
        [Description("ENR 1.12, OTHER:INTERCEPTION_OF_CIVIL_AIRCRAFT")]
        OTHER_INTERCEPTION_OF_CIVIL_AIRCRAFT,
        [Description("ENR 2.2, OTHER:OTHER_REGULATED_AIRSPACE")]
        OTHER_OTHER_REGULATED_AIRSPACE,
	    [Description("ENR 3.5, OTHER:OTHER_ROUTES")]
	    OTHER_OTHER_ROUTES,
        [Description("ENR 4.3, OTHER:GLOBAL_NAVIGATION_SATELLITE_SYSTEM_GNSS")]
        OTHER_GLOBAL_NAVIGATION_SATELLITE_SYSTEM_GNSS,
	    [Description("ENR 5.2, OTHER:MILITARY_EXERCISE_AND_TRAINING_AREAS_AND_ADIZ")]
	    OTHER_MILITARY_EXERCISE_AND_TRAINING_AREAS_AND_ADIZ,
        [Description("ENR 5.3.2, OTHER:OTHER_POTENTIAL_HAZARDS")]
        OTHER_OTHER_POTENTIAL_HAZARDS,
        [Description("ENR 5.6, OTHER:BIRD_MIGRATION_AND_AREAS_WITH_SENSITIVE_FAUNA")]
        OTHER_BIRD_MIGRATION_AND_AREAS_WITH_SENSITIVE_FAUNA,
        [Description("AD 1.1.1, OTHER:GENERAL_CONDITIONS")]
        OTHER_GENERAL_CONDITIONS,
        [Description("AD 1.1.2, OTHER:USE_OF_MILITARY_AIR_BASES")]
        OTHER_USE_OF_MILITARY_AIR_BASES,
        [Description("AD 1.1.3, OTHER:LOW_VISIBILITY_PROCEDURES_LVP")]
        OTHER_LOW_VISIBILITY_PROCEDURES_LVP,
        [Description("AD 1.1.4, OTHER:AERODROME_OPERATING_MINIMA")]
        OTHER_AERODROME_OPERATING_MINIMA,
        [Description("AD 1.1.5, OTHER:OTHER_INFORMATION")]
        OTHER_OTHER_INFORMATION,
        [Description("AD 1.2.1, OTHER:RESCUE_AND_FIRE_FIGHTING_SERVICES")]
        OTHER_RESCUE_AND_FIRE_FIGHTING_SERVICES,
        [Description("AD 1.2.2, OTHER:SNOW_PLAN")]
        OTHER_SNOW_PLAN,
        [Description("AD 1.4, OTHER:GROUPING_OF_AERODROMES_HELIPORTS")]
        OTHER_GROUPING_OF_AERODROMES_HELIPORTS,
        [Description("AD 2.20, OTHER:LOCAL_AERODROME_REGULATIONS")]
        OTHER_LOCAL_AERODROME_REGULATIONS,
        [Description("AD 2.22, AD 3.21, OTHER:FLIGHT_PROCEDURES")]
        OTHER_FLIGHT_PROCEDURES,
        [Description("AD 2.23, AD 3.22, OTHER:ADDITIONAL_INFORMATION")]
        OTHER_ADDITIONAL_INFORMATION,
        [Description("AD 3.19, OTHER:LOCAL_HELIPORT_REGULATIONS")]
        OTHER_LOCAL_HELIPORT_REGULATIONS,
	    [Description("ENR 5.5, OTHER:AERIAL_SPORTING_AND_RECREATIONAL_ACTIVITIES")]
	    OTHER_AERIAL_SPORTING_AND_RECREATIONAL_ACTIVITIES
    }
    public enum CIRoleCode
	{
		ResourceProvider,
		Custodian,
		Owner,
		User,
		Distributor,
		Originator,
		PointOfContact,
		PrincipalInvestigator,
		Processor,
		Publisher,
		Author
	}
	public enum PhoneCodeType
	{
		Phone,
		Fax,
		Mobile,
		Other
	}
	public enum LanguageCodeType
	{
		English,
		Russian,
		Latvian
	}
	public enum RestrictionCode
	{
		Copyright,
		License,
		IntellectualPropertyRight,
		Restricted,
		OtherRestrictions
	}
	public enum ClassificationCode
	{
		Unclassified,
		Restricted,
		Confidential,
		Secret,
		TopSecret,
		OtherHandles
	}
	public enum CIDateTypeCode
	{
		Creation,
		Publication,
		Revision
	}
	public enum MeasureClassCode
	{
		Defined,
		Calculated,
		Derived,
		Surveyed
	}
	public enum DQEvaluationMethodTypeCode
	{
		DirectInternal,
		DirectExternal,
		Indirect
	}
	public enum MDProgressCode
	{
		Completed,
		HistoricalArchive,
		Obsolete,
		OnGoing,
		Planned,
		Required,
		UnderDevelopment
	}
	public enum TimeSliceInterpretationType
	{
        [Description("Baseline")]
		BASELINE,

        [Description("Snapshot")]
        SNAPSHOT,

        [Description("Temporary Delta")]
        TEMPDELTA,

        [Description("Permanent Delta")]
        PERMDELTA
	}

    #region ISO METADATA

    public enum CiDateTypeCode
    {
        Creation,
        Publication,
        Revision,
        Adopted,
        Deprecated,
        Distribution,
        Expiry,
        InForce,
        LastRevision,
        LastUpdate,
        NextUpdate,
        Released,
        Superseded,
        Unavailable,
        ValidityBegins,
        ValidityExpires
    }

    public enum MdTimeIndeterminateValueType
    {
        After,
        Before,
        Now,
        Unknown
    }
    public enum MdOnLineFunctionCode
    {
        Download,
        Information,
        OfflineAccess,
        Order,
        Search
    }
    public enum CiRoleCode
    {
        ResourceProvider,
        Custodian,
        Owner,
        User,
        Distributor,
        Originator,
        PointOfContact,
        PrincipalInvestigator,
        Processor,
        Publisher,
        Author
    }
    public enum CiPresentationFormCode
    {
        DocumentDigital,
        DocumentHardcopy,
        ImageDigital,
        ImageHardcopy,
        MapDigial,
        MapHardcopy,
        ModelDigital,
        ModelHardcopy,
        ProfileDigital,
        ProfileHardcopy,
        TableDigital,
        TableHardcopy,
        VideoDigital,
        VideoHardcopy
    }
    public enum MdRestrictionCode
    {
        Copyright,
        Patent,
        PatentPending,
        Trademark,
        License,
        IntellectualPropertyRights,
        Restricted,
        OtherRestrictions
    }
    public enum MdClassificationCode
    {
        Unclassified,
        Restricted,
        Confidential,
        Secret,
        TopSecret
    }
    public enum MdProgressCode
    {
        Completed,
        HistoricalArchive,
        Obsolete,
        OnGoing,
        Planned,
        Required,
        UnderDevelopment
    }
    public enum DqEvaluationMethodTypeCode
    {
        DirectInternal,
        DirectExternal,
        Indirect
    }

    #endregion
}
