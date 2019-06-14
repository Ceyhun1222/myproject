using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PDM
{

    public enum PDM_ENUM
    {
        PDMObject,
        AirportHeliport,
        Runway,
        RunwayDirection,
        RunwayCenterLinePoint,
        DeclaredDistance,
        NavaidSystem,
        NavaidComponent,
        Localizer,
        GlidePath,
        VOR,
        DME,
        TACAN,
        NDB,
        WayPoint,
        Marker,
        InstrumentApproachProcedure,
        StandardInstrumentArrival,
        StandardInstrumentDeparture,
        AircraftCharacteristic,
        Procedure,
        ProcedureTransitions,
        FinalLeg,
        MissaedApproachLeg,
        ProcedureLeg,
        ObstacleAssessmentArea,
        Obstruction,
        ApproachCondition,
        SegmentPoint,
        FacilityMakeUp,
        DistanceIndication,
        AngleIndication,
        Enroute,
        RouteSegment,
        RouteSegmentPoint,
        AirspaceVolume,
        Airspace,
        VerticalStructure,
        VerticalStructurePart,
        AREA_PDM,
    }

    public enum NavaidSystemType
    {
        VOR,
        VOR_TAC,
        VOR_DME,
        DME,
        TACAN,
        NDB,
        ILS,
        ILS_DME,
        ILS_TACAN,
        LOC,
        LOC_DME,
        NDB_DME,
        MKR,
        NDB_MKR,
        MLS,
        MLS_DME,
        OTHER

    }

	public enum CodeDayBase
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
		BUSY_FRI,
		OTHER
	}

    public enum UOM_DIST_HORZ
    {
        NM = 1,
        KM = 2,
        M = 0,
        FT = 3,
        MI = 4
    }

    public enum UOM_DIST_VERT
    {
        FT = 1,
        M = 0,
        FL = 2,
		SM = 3,
        KM = 4
    }

    public enum UOM_FREQ
    {
        HZ,
        KHZ,
        MHZ,
        GHZ
    }

    public enum SegmentPointType
    {
        NAVAID_VHF,
        NDB,
        WayPoint,
        OTHER
    }

    public enum CODE_ROUTE_SEGMENT_DIR
    {
        BOTH,
        FORWARD,
        BACKWARD
    }

    public enum CODE_ROUTE_SEGMENT_CODE_LVL
    {
        UPPER_AIRSPACE,
        LOWER_AIRSPACE,
        BOTH
    }

    //public enum UOM_CODE_CLASS_ACFT 
    //{
    //    Jet,
    //    Propeller,
    //    Helicopter,
    //    Helicopter_and_propeller,
    //    ALL_Jet_propeller_and_helicopter,
    //    Jet_and_propeller,
    //    OTHER
    //}

    public enum CODE_ROUTE_SEGMENT_CODE_INTL
    {
        INTERNATIONAL,
        DOMESTIC,
        OTHER
    }

    public enum CODE_DIST_VER
    {
		SFC,
		MSL,
		W84,
		STD,
        OTHER,
		HEI,
        ALT,
        QFE,
        QNH,
    }

    public enum AirspaceType
    {
        NAS = 45,
        FIR = 1,
        FIR_P = 2,
        UIR = 3,
        UIR_P = 4,
        CTA = 5,
        CTA_P = 6,
        OCA_P = 7,
        OCA = 8,
        UTA = 9,
        UTA_P = 10,
        TMA = 11,
        TMA_P = 12,
        CTR = 13,
        CTR_P = 14,
        OTA = 15,
        SECTOR = 16,
        SECTOR_C = 17,
        TSA = 18,
        CBA = 19,
        RCA = 20,
        RAS = 21,
        AWY = 22,
        MTR = 23,
        P = 24,
        R = 25,
        D = 26,
        ADIZ = 27,
        NO_FIR = 28,
        PART = 29,
        CLASS = 30,
        POLITICAL = 31,
        D_OTHER = 32,
        TRA = 33,
        A = 34,
        W = 35,
        PROTECT = 36,
        AMA = 37,
        ASR = 38,
        ADV = 39,
        UADV = 40,
        ATZ = 41,
        ATZ_P = 42,
        HTZ = 43,
        NAS_P = 44,
        OTHER = 0,
    }

    public enum PROC_TYPE_code
    {
        SID = 0,
        STAR = 1,
        Approach = 2,
        Multiple = 3,
    }

    public enum Route_Type
    {
        None,

        #region SID

        Engine_Out_SID,
        SID_Runway_Transition,
        SID_or_SID_Common_Route,
        SID_Enroute_Transition,
        RNAV_SID_Runway_Transition,
        RNAV_SID_or_SID_Common_Route,
        RNAV_SID_Enroute_Transition,
        FMS_SID_Runway_Transition,
        FMS_SID_or_SID_Common_Route,
        FMS_SID_Enroute_Transition,
        Vector_SID_Runway_Transition,
        Vector_SID_Enroute_Transition,

        #endregion

        #region STAR

        STAR_Enroute_Transition,
        STAR_or_STAR_Common_Route,
        STAR_Runway_Transition,
        RNAV_STAR_Enroute_Transition,
        RNAV_STAR_or_STAR_Common_Route,
        RNAV_STAR_Runway_Transition,
        Profile_Descent_Enroute_Transition,
        Profile_Descent_Common_Route,
        Profile_Descent_Runway_Transition,
        FMS_STAR_Enroute_Transition,
        FMS_STAR_or_STAR_Common_Route,
        FMS_STAR_Runway_Transition,

        #endregion

        #region IAP

        Approach_Transition,
        Localizer_Backcourse_Approach,
        RNAV_GPS_Required_Approach,
        Flight_Management_System_Approach,
        Instrument_Guidance_System_Approach,
        Instrument_Landing_System_Approach,
        LAAS_GPS_GLS_Approach,
        WAAS_GPS_Approach,
        Localizer_Only_Approach,
        Microwave_Landing_System_Approach,
        Non_Directional_Beacon_Approach,
        Global_Positioning_System_Approach,
        Area_Navigation_Approach,
        TACAN_Approach,
        Simplified_Directional_Facility_Approach,
        VOR_Approach,
        Microwave_Landing_System_Type_A_Approach,
        Localizer_Directional_Aid_Approach,
        Microwave_Landing_System_Type_B_and_C_Approach,
        Missed_Approach,


        #endregion

    }

    public enum ProcedureCodingStandardType
    {
        PANS_OPS = 4,
        ARINC_424_15 = 1,
        ARINC_424_18 = 2,
        ARINC_424_19 = 3,
        OTHER = 0,
    }

    public enum ProcedureDesignStandardType
    {
        PANS_OPS = 4,
        TERPS = 1,
        CANADA_TERPS = 2,
        NATO = 3,
        OTHER =0,
    }

    public enum ProcedurePhaseType
    {
        RWY = 0,
        COMMON = 1,
        EN_ROUTE = 2,
        APPROACH = 3,
        FINAL = 4,
        MISSED = 5,
        MISSED_P = 6,
        MISSED_S = 7,
        ENGINE_OUT = 8,
        OTHER = 9,

    }

    public enum PointChoice
    {
        DesignatedPoint = 0,
        Navaid = 1,
        TouchDownLiftOff = 2,
        RunwayCenterlinePoint = 3,
        AirportHeliport = 4,
        NONE = 5,
        OTHER = 6,
    }

    public enum BearingType
    {
        TRUE = 0,
        MAG = 1,
        RDL = 2,
        TRK = 3,
        HDG = 4,
        OTHER = 5,
    }

    public enum DesignatorType
    {
        ICAO=0,
        COORD=1,
        CNF=2,
        DESIGNED=3,
        MTR=4,
        TERMINAL=5,
        BRG_DIST=6,
        OTHER=7,
    }

    public enum ProcedureFixRoleType
    {
        IAF = 0,
        IF = 1,
        IF_IAF = 2,
        FAF = 3,
        VDP = 4,
        SDF = 5,
        FPAP = 6,
        FTP = 7,
        FROP = 8,
        TP = 9,
        MAPT = 10,
        MAHF = 11,
        OTHER = 12,
        ENRT = 13,
        OTHER_WPT =14,
    }

    public enum AltitudeUseType
    {
        ABOVE_LOWER = 0,
        BELOW_UPPER = 1,
        AT_LOWER = 2,
        BETWEEN = 3,
        RECOMMENDED = 4,
        EXPECT_LOWER = 5,
        AS_ASSIGNED = 6,
        OTHER = 7,
    }

    public enum DurationType
    {
        HR = 0,
        MIN = 1,
        SEC = 2,
        OTHER = 3,
    }

    public enum TrajectoryType
    {
        STRAIGHT = 0,
        ARC = 1,
        PT = 2,
        BASETURN = 3,
        HOLDING = 4,
        OTHER = 5,
    }

    public enum DirectionTurnType
    {
        LEFT = 0,
        RIGHT = 1,
        EITHER = 2,
        OTHER = 3,
    }

    public enum ApproachType
    {
        ASR = 0,
        ARA = 1,
        ARSR = 2,
        PAR = 3,
        ILS = 4,
        ILS_DME = 5,
        ILS_PRM = 6,
        LDA = 7,
        LDA_DME = 8,
        LOC = 9,
        LOC_BC = 10,
        LOC_DME = 11,
        LOC_DME_BC = 12,
        MLS = 13,
        MLS_DME = 14,
        NDB = 15,
        NDB_DME = 16,
        SDF = 17,
        TLS = 18,
        VOR = 19,
        VOR_DME = 20,
        TACAN = 21,
        VORTAC = 22,
        DME = 23,
        DME_DME = 24,
        RNP = 25,
        GPS = 26,
        GLONASS = 27,
        GALILEO = 28,
        RNAV = 29,
        IGS = 30,
        OTHER = 31,
    }

    public enum UpperAlphaType
    {
        A = 0,
        B = 1,
        C = 2,
        D = 3,
        E = 4,
        F = 5,
        G = 6,
        H = 7,
        I = 8,
        J = 9,
        K = 10,
        L = 11,
        M = 12,
        N = 13,
        O = 14,
        P = 15,
        Q = 16,
        R = 17,
        S = 18,
        T = 19,
        U = 20,
        V = 21,
        W = 22,
        X = 23,
        Y = 24,
        Z = 25,
        OTHER = 26,
    }

    public enum SpeedType
    {
        KM_H = 0,
        KT = 1,
        MACH = 2,
        M_MIN = 3,
        FT_MIN = 4,
        M_SEC = 5,
        FT_SEC = 6,
        MPH = 7,
        OTHER = 8,
    }

    public enum ProcedureSegmentPointUse
    {
        ARC_CENTER =0,
        START_POINT=1,
        END_POINT=2,
    }

    public enum VerticalStructureType
    {
        AG_EQUIP = 0,
        ANTENNA = 1,
        ARCH = 2,
        BRIDGE = 3,
        BUILDING = 4,
        CABLE_CAR = 5,
        CATENARY = 6,
        COMPRESSED_AIR_SYSTEM = 7,
        CONTROL_MONITORING_SYSTEM = 8,
        CONTROL_TOWER = 9,
        COOLING_TOWER = 10,
        CRANE = 11,
        DAM = 12,
        DOME = 13,
        ELECTRICAL_EXIT_LIGHT = 14,
        ELECTRICAL_SYSTEM = 15,
        ELEVATOR = 16,
        FENCE = 17,
        FUEL_SYSTEM = 18,
        GATE = 19,
        GENERAL_UTILITY = 20,
        GRAIN_ELEVATOR = 21,
        HEAT_COOL_SYSTEM = 22,
        INDUSTRIAL_SYSTEM = 23,
        LIGHTHOUSE = 24,
        MONUMENT = 25,
        NATURAL_GAS_SYSTEM = 26,
        NATURAL_HIGHPOINT = 27,
        NAVAID = 28,
        NUCLEAR_REACTOR = 29,
        POLE = 30,
        POWER_PLANT = 31,
        REFINERY = 32,
        RIG = 33,
        SALTWATER_SYSTEM = 34,
        SIGN = 35,
        SPIRE = 36,
        STACK = 37,
        STADIUM = 38,
        STORM_SYSTEM = 39,
        TANK = 40,
        TETHERED_BALLOON = 41,
        TOWER = 42,
        TRAMWAY = 43,
        TRANSMISSION_LINE = 44,
        TREE = 45,
        URBAN = 46,
        VEGETATION = 47,
        WALL = 48,
        WASTEWATER_SYSTEM = 49,
        WATER_SYSTEM = 50,
        WATER_TOWER = 51,
        WINDMILL = 52,
        WINDMILL_FARMS = 53,
        OTHER = 54,
    }

    public enum StatusConstructionType
    {
        IN_CONSTRUCTION = 0,
        COMPLETED = 1,
        DEMOLITION_PLANNED = 2,
        IN_DEMOLITION = 3,
        OTHER = 4,
    }

    public enum ColourType
    {
        YELLOW = 0,
        RED = 1,
        WHITE = 2,
        BLUE = 3,
        GREEN = 4,
        PURPLE = 5,
        ORANGE = 6,
        AMBER = 7,
        BLACK = 8,
        BROWN = 9,
        GREY = 10,
        LIGHT_GREY = 11,
        MAGENTA = 12,
        PINK = 13,
        VIOLET = 14,
        OTHER = 15,
    }

    public enum VerticalStructureMaterialType
    {
        ADOBE_BRICK = 0,
        ALUMINIUM = 1,
        BRICK = 2,
        CONCRETE = 3,
        FIBREGLASS = 4,
        GLASS = 5,
        IRON = 6,
        MASONRY = 7,
        METAL = 8,
        MUD = 9,
        PLANT = 10,
        PRESTRESSED_CONCRETE = 11,
        REINFORCED_CONCRETE = 12,
        SOD = 13,
        STEEL = 14,
        STONE = 15,
        TREATED_TIMBER = 16,
        WOOD = 17,
        OTHER = 18,
    }

    public enum VerticalStructureMarkingType
    {
        MONOCOLOUR = 0,
        CHEQUERED = 1,
        HBANDS = 2,
        VBANDS = 3,
        FLAG = 4,
        MARKERS = 5,
        OTHER = 6,
    }

    public enum CodeStatusAirspaceType
    {
        AVBL_FOR_ACTIVATION = 0,
        ACTIVE = 1,
        IN_USE = 2,
        INACTIVE = 3,
        INTERMITTENT = 4,
        OTHER = 5,

    }

    public enum CodeRunwayCenterLinePointRoleType
    {
        nilReason = 0,
        START = 1,
        THR = 2,
        DISTHR = 3,
        TDZ = 4,
        MID = 5,
        END = 6,
        START_RUN = 7,
        LAHSO = 8,
        ABEAM_GLIDESLOPE = 9,
        ABEAM_PAR = 10,
        ABEAM_ELEVATION = 11,
        ABEAM_TDR = 12,
        ABEAM_RER = 13,
        OTHER = 14,

    }

    public enum CodeFinalGuidance
    {
        OTHER = 0,
        LPV = 1,
        LNAV_VNAV = 2,
        LNAV = 3,
        GLS = 4,
        ASR = 5,
        ARA = 6,
        ARSR = 7,
        PAR = 8,
        ILS = 9,
        ILS_DME = 10,
        ILS_PRM = 11,
        LDA = 12,
        LDA_DME = 13,
        LOC = 14,
        LOC_BC = 15,
        LOC_DME = 16,
        LOC_DME_BC = 17,
        MLS = 18,
        MLS_DME = 19,
        NDB = 20,
        NDB_DME = 21,
        SDF = 22,
        TLS = 23,
        VOR = 24,
        VOR_DME = 25,
        TACAN = 26,
        VORTAC = 27,
        DME = 28,
        LP = 29,


    }

    public enum CodeApproachGuidance
    {
        OTHER = 0,
        NON_PRECISION = 1,
        ILS_PRECISION_CAT_I = 2,
        ILS_PRECISION_CAT_II = 3,
        ILS_PRECISION_CAT_IIIA = 4,
        ILS_PRECISION_CAT_IIIB = 5,
        ILS_PRECISION_CAT_IIIC = 6,
        ILS_PRECISION_CAT_IIID = 7,
        MLS_PRECISION = 8,

    }

    public enum CodeSide
    {
        OTHER = 0,
        LEFT =1,
        RIGHT =2,
        BOTH =3,
    }

    public enum CodeRelativePosition
    {
        OTHER = 0,
        BEFORE = 1,
        AT = 2,
        AFTER = 3,
    }

    public enum Uom_Temperature
    {
        C = 1,
        F = 2,
        K = 3,
        OTHER = 0
    }

    public enum CodeMinimaFinalApproachPath
    {
        OTHER = 0,
        STRAIGHT_IN =1,
        CIRCLING =2,
        SIDESTEP =3,
    }

    public enum CodeMinimumAltitude
    {
        OTHER =0,
        OCA =1,
        DA =2,
        MDA =3,
    }

    public enum CodeVerticalReference
    {
        SFC =0,
        MSL =1,
        W84 =2,
        STD =3,
        OTHER_QFE =4,
        OTHER_QNH =5,
    }

    public enum CodeMinimumHeight
    {
        OTHER = 0,
        DH =1,
        OCH =2,
        MDH =3,
    }

    public enum CodeHeightReference
    {
        OTHER =0,
        HAT =1,
        HAA =2,
        HAL =3,
        HAS =4,
    }

    public enum CodeObstacleAssessmentSurface
    {
        OTHER = 0,
        _40_TO_1 = 1,
        _72_TO_1 = 2,
        MA = 3,
        FINAL = 4,
        PT_ENTRY_AREA = 5,
        PRIMARY = 6,
        SECONDARY = 7,
        ZONE1 = 8,
        ZONE2 = 9,
        ZONE3 = 10,
        AREA1 = 11,
        AREA2 = 12,
        AREA3 = 13,
        TURN_INITIATION = 14,
        TURN = 15,
        DER = 16,
    }

    public enum CodeObstructionIdSurfaceZone
    {
        OTHER = 0,
        APPROACH =1,
        CONICAL =2,
        HORIZONTAL =3,
        PRIMARY =4,
        TRANSITION =5,
    }

    public enum CodeBearing
    {
        OTHER = 0,
        TRUE = 1,
        MAG =2,
        RDL =3,
        TRK =4,
        HDG =5
    }

    public enum CodeDirectionReference
    {
        OTHER = 0,
        TO =1,
        FROM =2,
        OTHER_CW =3,
        OTHER_CCW =4
    }

    public enum CodeCardinalDirection
    {
        OTHER = 0,
        N = 1,
        NE = 2,
        E = 3,
        SE = 4,
        S = 5,
        SW = 6,
        W = 7,
        NW = 8,
        NNE = 9,
        ENE = 10,
        ESE = 11,
        SSE = 12,
        SSW = 13,
        WSW = 14,
        WNW = 15,
        NNW = 16,
    }

    public enum CodeDistanceIndication
    {
        OTHER = 0,
        DME = 1,
        GEODETIC =2,
    }

    public enum CodeApproachPrefix
    {
        OTHER = 0,
        HI =1,
        COPTER =2,
        CONVERGING =3
    }

    public enum CodeApproachEquipmentAdditional
    {
        OTHER = 0,
        ADF =1,
        DME =2,
        RADAR=3,
        RADARDME=4,
        DUALVORDME=5,
        DUALADF=6,
        ADFMA=7,
        SPECIAL=8,
        DUALVHF=9,
        GPSRNP3=10,
        ADFILS=11,
        DUALADF_DME=12,
        RADAR_RNAV=13
    }

    public enum CodeMissedApproach
    {
        OTHER = 0,
        PRIMARY =1,
        SECONDARY= 2,
        ALTERNATE = 3,
        TACAN =4,
        TACANALT =5,
        ENGINEOU =6,T
    }

    public enum SegmentLegSpecialization
    {
        DepartureLeg=0,
        ArrivalLeg=1,
        ApproachLeg=2,
        InitialLeg =3,
        ArrivalFeederLeg=4,
        IntermediateLeg=5,
        FinalLeg=6,
        MissedApproachLeg=7,

    }

    public enum CodeSegmentTermination
    {
        OTHER = 0,
        ALTITUDE =1,
        DISTANCE =2,
        DURATION =3,
        INTERCEPT =4
    }

    public enum CodeATCReporting
    {
        COMPULSORY =2,
        ON_REQUEST =1,
        OTHER = 3,
        NO_REPORT = 0,
    }

    public enum CodeReferenceRole
    {
        OTHER = 0,
        INTERSECTION =1,
        RECNAV =2,
        ATD=3,
        OTHER_OVERHEAD=4,
        RAD_DME=5
    }

    public enum CodeRouteOrigin
    {
        OTHER = 0,
        INTL =1,
        DOM =2,
        BOTH =3
    }

    public enum CodeDeclaredDistance
    {
        OTHER = 0,
        LDA = 1,
        TORA = 2,
        TODA = 3,
        ASDA = 4,
        DTHR = 5,
        TODAH = 6,
        RTODAH = 7,
        LDAH = 8,
        DPLM = 9,

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

    public enum UomTemperature
    {
        C =0,
        F=1,
        K =2
    }

    public enum AircraftCategoryType
    {
        A = 0,
        B = 1,
        C = 2,
        D = 3,
        E = 4,
        H = 5,
        ALL = 6,
        OTHER = 7,
    }

    public enum CodeSpeedReference
    {
        IAS =0,
        TAS =1,
        GS =2,
    }

    public enum CodeProcedureDistance
    {
        HAT = 1,
        OM = 2,
        MM = 30,
        IM = 40,
        PFAF = 5,
        GSANT = 6,
        FAF = 7,
        MAP = 8,
        THLD = 9,
        VDP = 10,
        RECH = 11,
        OTHER_SDF = 12
    }

    public enum CodeMarkerBeaconSignal
    {
        FAN =1,
        LOW_PWR_FAN =2,
        Z =3,
        BONES =4
    }

}
