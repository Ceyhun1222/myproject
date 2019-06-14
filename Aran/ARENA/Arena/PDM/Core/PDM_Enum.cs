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
        RunwayElement,
        RunwayCenterLinePoint,
        Taxiway,
        TaxiwayElement,
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
        HoldingPattern,
        SafeAltitudeArea,
        SafeAltitudeAreaSector,
        GeoBorder,
        RadioCommunicationChanel,
        AircraftStand,
        AirportHeliportProtectionArea,
        AirportHotSpot,
        Marking,
        LightSystem,
        Apron,
        ApronElement,
        ApronLightSystem,
        ApronMarking,
        DeicingArea,
        DeicingAreaMarking,
        GuidanceLine,
        GuidanceLineLightSystem,
        GuidanceLineMarking,
        RunwayMarking,
        RunwayProtectArea,
        RunwayProtectAreaLightSystem,
        RunwayVisualRange,
        StandMarking,
        SurfaceCharacteristics,
        TaxiHoldingPosition,
        TaxiHoldingPositionLightSystem,
        TaxiHoldingPositionMarking,
        TaxiwayLightSystem,
        TaxiwayMarking,
        TouchDownLiftOff,
        TouchDownLiftOffLightSystem,
        TouchDownLiftOffMarking,
        TouchDownLiftOffSafeArea,
        VisualGlideSlopeIndicator,
        WorkArea,
        ApproachLightingSystem,
        CheckpointVOR,
        CheckpointINS,
        Road,
        Unit,
        NonMovementArea,
        RadioFrequencyArea

    }

    public enum NavaidSystemType
    {
        OTHER,
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
        SDF,
        

    }

    public enum CodeDayBase
    {
        OTHER,
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
        
    }

    public enum UOM_DIST_HORZ
    {
        OTHER = 0,
        M = 1,
        NM = 2,
        KM = 3,
        FT = 4,
        MI = 5,
        SM = 6,


    }

    public enum UOM_DIST_VERT
    {
        OTHER = 0,
        FT = 1,
        M = 2,
        FL = 3,
        SM = 4,
        KM = 5,
        NM = 6,

    }

    public enum UOM_FREQ
    {
        OTHER,
        HZ,
        KHZ,
        MHZ,
        GHZ
    }

    public enum SegmentPointType
    {
        OTHER,
        NAVAID_VHF,
        NDB,
        WayPoint,
        
    }

    public enum CODE_ROUTE_SEGMENT_DIR
    {
        OTHER = 0,
        BOTH = 1,
        FORWARD = 2,
        BACKWARD = 3,

    }

    public enum CODE_ROUTE_SEGMENT_CODE_LVL
    {
        OTHER = 0,
        LOWER = 1,
        BOTH = 2,
        UPPER = 3
    }

    public enum CODE_ROUTE_SEGMENT_CODE_INTL
    {
        OTHER = 0,
        INTERNATIONAL = 1,
        DOMESTIC = 2,
        
    }

    public enum CODE_DIST_VER
    {
        OTHER,
        SFC,
        MSL,
        W84,
        STD,       
        HEI,
        ALT,
        QFE,
        QNH,
    }

    public enum AirspaceType
    {
        NAS = 0,
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
        OTHER = 45,
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
        OTHER = 0,
    }

    public enum ProcedurePhaseType
    {
        OTHER = 0,
        RWY = 9,
        COMMON = 1,
        EN_ROUTE = 2,
        APPROACH = 3,
        FINAL = 4,
        MISSED = 5,
        MISSED_P = 6,
        MISSED_S = 7,
        ENGINE_OUT = 8,
        
    }

    public enum PointChoice
    {
        OTHER = 0,
        DesignatedPoint = 6,
        Navaid = 1,
        TouchDownLiftOff = 2,
        RunwayCentrelinePoint = 3,
        AirportHeliport = 4,
        NONE = 5,
        
    }

    public enum BearingType
    {
        OTHER = 0,        
        MAG = 1,
        RDL = 2,
        TRK = 3,
        HDG = 4,
        TRUE = 5,
    }

    public enum DesignatorType
    {
        OTHER = 0,        
        COORD = 1,
        CNF = 2,
        DESIGNED = 3,
        MTR = 4,
        TERMINAL = 5,
        BRG_DIST = 6,
        ICAO = 7,

    }

    public enum ProcedureFixRoleType
    {
        OTHER=0,        
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
        IAF = 12,
        ENRT = 13,
        OTHER_WPT = 14,
        ENRT_HLDNG = 15,
    }

    public enum AltitudeUseType
    {
        OTHER = 0,        
        BELOW_UPPER = 1,
        AT_LOWER = 2,
        BETWEEN = 3,
        RECOMMENDED = 4,
        EXPECT_LOWER = 5,
        AS_ASSIGNED = 6,
        ABOVE_LOWER = 7,

    }

    public enum DurationType
    {
        OTHER = 0,
        HR = 3,
        MIN = 1,
        SEC = 2,
       
    }

    public enum TrajectoryType
    {
        STRAIGHT = 5,
        ARC = 1,
        PT = 2,
        BASETURN = 3,
        HOLDING = 4,
        OTHER = 0,
    }

    public enum DirectionTurnType
    {
        OTHER = 0,
        LEFT = 1,
        RIGHT = 2,
        EITHER = 3,

    }

    public enum ApproachType
    {
        OTHER = 0,        
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
        ASR = 31,

    }

    public enum UpperAlphaType
    {
        OTHER = 0,
        A = 26,
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
        
    }

    public enum CodeRouteNavigation
    {
        CONV = 3,
        RNAV = 1,
        TACAN = 2,
        OTHER = 0,
    }

    public enum SpeedType
    {
        KM_H = 8,
        KT = 1,
        MACH = 2,
        M_MIN = 3,
        FT_MIN = 4,
        M_SEC = 5,
        FT_SEC = 6,
        MPH = 7,
        OTHER = 0,
    }

    public enum ProcedureSegmentPointUse
    {
        OTHER=0,
        ARC_CENTER = 3,
        START_POINT = 1,
        END_POINT = 2,
    }

    public enum VerticalStructureType
    {
        OTHER = 0,        
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
        AG_EQUIP = 54,

    }

    public enum StatusConstructionType
    {
        IN_CONSTRUCTION = 4,
        COMPLETED = 1,
        DEMOLITION_PLANNED = 2,
        IN_DEMOLITION = 3,
        OTHER = 0,
    }

    public enum ColourType
    {
        OTHER = 0,        
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
        YELLOW = 15,

    }

    public enum VerticalStructureMaterialType
    {
        OTHER = 0,       
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
        ADOBE_BRICK = 18,

    }

    public enum VerticalStructureMarkingType
    {
        OTHER = 0,        
        CHEQUERED = 1,
        HBANDS = 2,
        VBANDS = 3,
        FLAG = 4,
        MARKERS = 5,
        MONOCOLOUR = 6,

    }

    public enum CodeStatusAirspaceType
    {
        AVBL_FOR_ACTIVATION = 5,
        ACTIVE = 1,
        IN_USE = 2,
        INACTIVE = 3,
        INTERMITTENT = 4,
        OTHER = 0,

    }

    public enum CodeRunwayCenterLinePointRoleType
    {
        nilReason = 14,
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
        OTHER = 0,

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
        LEFT = 1,
        RIGHT = 2,
        BOTH = 3,
    }

    public enum CodeRelativePosition
    {
        OTHER = 0,
        BEFORE = 1,
        AT = 2,
        AFTER = 3,
    }

    public enum CodeMinimaFinalApproachPath
    {
        OTHER = 0,
        STRAIGHT_IN = 1,
        CIRCLING = 2,
        SIDESTEP = 3,
    }

    public enum CodeMinimumAltitude
    {
        OTHER = 0,
        OCA = 1,
        DA = 2,
        MDA = 3,
    }

    public enum CodeVerticalReference
    {
        OTHER = 0,
        SFC = 6,
        MSL = 1,
        W84 = 2,
        STD = 3,
        OTHER_QFE = 4,
        OTHER_QNH = 5,
    }

    public enum CodeMinimumHeight
    {
        OTHER = 0,
        DH = 1,
        OCH = 2,
        MDH = 3,
    }

    public enum CodeHeightReference
    {
        OTHER = 0,
        HAT = 1,
        HAA = 2,
        HAL = 3,
        HAS = 4,
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
        APPROACH = 1,
        CONICAL = 2,
        HORIZONTAL = 3,
        PRIMARY = 4,
        TRANSITION = 5,
    }

    public enum CodeCourse
    {
        OTHER,
        TRUE_TRACK,
        MAG_TRACK,
        TRUE_BRG,
        MAG_BRG,
        HDG,
        RDL
    }

    public enum CodeDirectionReference
    {
        OTHER = 0,
        TO = 1,
        FROM = 2,
        OTHER_CW = 3,
        OTHER_CCW = 4
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
        GEODETIC = 2,
    }

    public enum CodeApproachPrefix
    {
        OTHER = 0,
        HI = 1,
        COPTER = 2,
        CONVERGING = 3
    }

    public enum CodeApproachEquipmentAdditional
    {
        OTHER = 0,
        ADF = 1,
        DME = 2,
        RADAR = 3,
        RADARDME = 4,
        DUALVORDME = 5,
        DUALADF = 6,
        ADFMA = 7,
        SPECIAL = 8,
        DUALVHF = 9,
        GPSRNP3 = 10,
        ADFILS = 11,
        DUALADF_DME = 12,
        RADAR_RNAV = 13
    }

    public enum CodeMissedApproach
    {
        OTHER = 0,
        PRIMARY = 1,
        SECONDARY = 2,
        ALTERNATE = 3,
        TACAN = 4,
        TACANALT = 5,
        ENGINEOU = 6, T
    }

    public enum SegmentLegSpecialization
    {
        OTHER = 0,
        DepartureLeg = 8,
        ArrivalLeg = 1,
        ApproachLeg = 2,
        InitialLeg = 3,
        ArrivalFeederLeg = 4,
        IntermediateLeg = 5,
        FinalLeg = 6,
        MissedApproachLeg = 7,

    }

    public enum CodeSegmentTermination
    {
        OTHER = 0,
        ALTITUDE = 1,
        DISTANCE = 2,
        DURATION = 3,
        INTERCEPT = 4
    }

    public enum CodeATCReporting
    {
        COMPULSORY = 2,
        ON_REQUEST = 1,
        OTHER = 0,
        NO_REPORT = 3,
    }

    public enum CodeReferenceRole
    {
        OTHER = 0,
        INTERSECTION = 1,
        RECNAV = 2,
        ATD = 3,
        OTHER_OVERHEAD = 4,
        RAD_DME = 5
    }

    public enum CodeRouteOrigin
    {
        OTHER = 0,
        INTL = 1,
        DOM = 2,
        BOTH = 3
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
        OTHER,
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

    public enum Uom_Temperature
    {
        OTHER = 0,
        C = 1,
        F = 2,
        K = 3,
    }

    public enum AircraftCategoryType
    {
        A = 7,
        B = 1,
        C = 2,
        D = 3,
        E = 4,
        H = 5,
        ALL = 6,
        OTHER = 0,
    }

    public enum CodeSpeedReference
    {
        OTHER=0,
        IAS = 3,
        TAS = 1,
        GS = 2,
    }

    public enum CodeProcedureDistance
    {
        OTHER = 0,
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
        OTHER = 0,
        FAN = 1,
        LOW_PWR_FAN = 2,
        Z = 3,
        BONES = 4
    }

    public enum CodeHoldingUsage
    {
        OTHER = 0,
        ENR = 1,
        TER = 2
    }

    public enum CodeSafeAltitude
    {
        OTHER = 0,
        MSA = 1,
        ESA = 2
    }

    public enum CodeArcDirection
    {
        OTHER = 0,
        CWA = 1,
        CCA = 2
    }

    public enum CodeDME
    {
        OTHER = 0,
        NARROW = 1,
        PRECISION = 2,
        WIDE = 3
    }

    public enum CodeGeoBorder
    {
        OTHER = 0,
        STATE = 1,
        WATER = 2,
        COAST = 3,
        RIVER = 4,
        BANK = 5
    }

    public enum CodeVOR
    {
        OTHER = 0,
        VOR,
        DVOR,
        VOT
    }

    public enum CodeTaxiwayType
    {
        OTHER = 0,
        AIR = 1,
        GND = 2,
        EXIT = 3,
        FASTEXIT = 4,
        STUB = 5,
        TURN_AROUND = 6,
        PARALLEL = 7,
        BYPASS = 8
    }

    public enum CodeRunwayElementType
    {
        OTHER = 0,
        NORMAL = 1,
        INTERSECTION = 2,
        SHOULDER = 3,
        DISPLACED = 4,
    }

    public enum CodeTaxiwayElementType
    {
        OTHER = 0,
        NORMAL = 1,
        INTERSECTION = 2,
        SHOULDER = 3,
        HOLDING_BAY = 4,
    }

    public enum CodeFacilityRanking
    {
        OTHER = 0,
        PRIMARY = 5,
        SECONDARY = 1,
        ALTERNATE = 2,
        EMERG = 3,
        GUARD = 4,


    }

    public enum CodeObstacleAreaType
    {
        OTHER = 0,
        AREA1 = 1,
        AREA2 = 2,
        AREA3 = 3,
        AREA4 = 4,
        OLS = 5,
        FAR77 = 6,
        MANAGED = 7,
        AREA2A = 8,
        AREA2B = 9,
        AREA2C = 10,
        AREA2D = 11,
        TAKEOFFCLIMB = 12,
        TAKEOFFFLIGHTPATHAREA = 13,
    }

    public enum CodeValueInterpretation
    {
        OTHER = 0,
        ABOVE = 1,
        AT_OR_ABOVE = 2,
        AT_OR_BELOW = 3,
        BELOW = 4,
    }

    public enum CodeSignalPerformanceILS
    {
        OTHER = 0,
        I = 1,
        II = 2,
        III = 3,
    }

    public enum AirportHeliportType
    {
        OTHER = 0,
        AD = 1,
        AH = 2,
        HP = 3,
        LS = 4,
    }

    public enum CodeLightIntensity
    {
        OTHER = 0,
        LIL = 1,
        LIM = 2,
        LIH = 3,
        LIL_LIH = 4,
        PREDETERMINED = 5
    }

    public enum CodeLightSource
    {
        OTHER = 0,
        FLOOD = 1,
        STROBE = 2
    }

    public enum CodeApronSectionType
    {
        OTHER = 0,
        EDGE = 1
    }

    public enum CodeMarkingConditionType
    {
        OTHER = 0,
        GOOD = 1,
        FAIR = 2,
        POOR = 3,
        EXCELLENT = 4
    }


    public enum CodeMarkingStyleType
    {
        OTHER = 0,
        SOLID = 1,
        DASHED = 2,
        DOTTED = 3
    }

    public enum CodeApronElementType
    {
        OTHER = 0,
        NORMAL = 1,
        PARKING = 2,
        RAMP = 3,
        CARGO = 4,
        FUEL = 5,
        HARDSTAND = 6,
        MAINT = 7,
        MILITARY = 8,
        LOADING = 9,
        TAXILANE = 10,
        TURNAROUND = 11,
        TEMPORARY = 12,
        STAIRS = 13,

    }

    public enum CodeVerticalDatumType
    {
        OTHER = 0,
        EGM_96 = 1,
        AHD = 2,
        NAVD88 = 3
    }

    public enum CodeRunwaySectionType
    {
        OTHER = 0,
        TDZ = 1,
        AIM = 2,
        CL = 3,
        EDGE = 4,
        THR = 5,
        DESIG = 6,
        AFT_THR = 7,
        DTHR = 8,
        END = 9,
        TWY_INT = 10,
        RPD_TWY_INT = 11,
        _1_THIRD = 12,
        _2_THIRD = 13,
        _3_THIRD = 14
    }

    public enum CodeGuidanceLineType
    {
        OTHER = 0,
        RWY = 1,
        TWY = 2,
        APRON = 3,
        GATE_TLANE = 4,
        LI_TLANE = 5,
        LO_TLANE = 6,
        AIR_TLANE = 7
    }

    public enum CodeDirectionType
    {
        OTHER = 0,
        BOTH = 1,
        FORWARD = 2,
        BACKWARD = 3,
    }

    public enum CodeAircraftStandType
    {
        OTHER = 0,
        NI = 1,
        ANG_NI = 2,
        ANG_NO = 3,
        PARL = 4,
        RMT = 5,
        ISOL = 6
    }

    public enum CodeVisualDockingGuidanceType
    {
        OTHER = 0,
        AGNIS = 1,
        PAPA = 2,
        SAFE_GATE = 3,
        SAFE_DOC = 4,
        APIS = 5,
        A_VDGS = 6,
        AGNIS_STOP = 7,
        AGNIS_PAPA = 8
    }

    public enum CodeSurfaceCompositionType
    {
        OTHER = 0,
        ASPH = 1,
        ASPH_GRASS = 2,
        CONC = 3,
        CONC_ASPH = 4,
        CONC_GRS = 5,
        GRASS = 6,
        SAND = 7,
        WATER = 8,
        BITUM = 9,
        BRICK = 10,
        MACADAM = 11,
        STONE = 12,
        CORAL = 13,
        CLAY = 14,
        LATERITE = 15,
        GRAVEL = 16,
        EARTH = 17,
        ICE = 18,
        SNOW = 19,
        MEMBRANE = 20,
        METAL = 21,
        MATS = 22,
        PIERCED_STEEL = 23,
        WOOD = 24,
        NON_BITUM_MIX = 25
    }

    public enum CodeSurfacePreparationType
    {
        OTHER,
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

    public enum CodeSurfaceConditionType
    {
        OTHER,
        GOOD,
        FAIR,
        POOR,
        UNSAFE,
        DEFORMED
    }

    public enum CodePCNPavementType
    {
        OTHER,
        RIGID,
        FLEXIBLE
    }

    public enum CodePCNSubgradeType
    {
        OTHER,
        A,
        B,
        C,
        D
    }

    public enum CodePCNTyrePressureType
    {
        OTHER,
        W,
        X,
        Y,
        Z       
    }

    public enum CodePCNMethodType
    {
        OTHER,
        TECH,
        ACFT
    }

    public enum CodeCommunicationModeType
    {
        OTHER = 0,
        HF = 1,
        VHF = 2,
        VDL1 = 3,
        VDL2 = 4,
        VDL4 = 5,
        AMSS = 6,
        ADS_B = 7,
        ADS_B_VDL = 8,
        HFDL = 9,
        VHF_833 = 10,
        UHF = 11
    }

    public enum CodeRadioEmissionType
    {
        OTHER = 0,
        A2 = 1,
        A3A = 2,
        A3B = 3,
        A3E = 4,
        A3H = 5,
        A3J = 6,
        A3L = 7,
        A3U = 9,
        J3E = 10,
        NONA1A = 11,
        NONA2A = 12,
        PON = 13,
        A8W = 14,
        A9W = 15,
        NOX = 16,
        G1D = 17

    }

    public enum CodeCommunicationDirectionType
    {
        OTHER = 0,
        UPLINK = 1,
        DOWNLINK = 2,
        BIDIRECTIONAL = 3,
        UPCAST = 4,
        DOWNCAST = 5
    }

    public enum CodeTaxiwaySectionType
    {
        OTHER = 0,
        CL = 1,
        EDGE = 2,
        END = 3,
        RWY_INT = 4,
        TWY_INT = 5,

    }

    public enum CodeYesNoType
    {
        OTHER = 0,
        YES = 1,
        NO = 2,
    }

    public enum CodeRunwayProtectionAreaType
    {
        OTHER=0,
        CWY = 1,
        RESA = 2,
        OFZ = 3,
        IOFZ = 4,
        POFZ = 5,
        ILS = 6,
        VGSI = 7,
        STOPWAY = 8,
        STRIP
    }

    public enum CodeStatusOperationsType
    {
        OTHER = 0,
        NORMAL = 1,
        DOWNGRADED = 2,
        UNSERVICEABLE = 3,
        WORK_IN_PROGRESS = 4,
    }

    public enum CodeProtectAreaSectionType
    {
        OTHER = 0,
        CL = 1,
        EDGE = 2,
        END = 3,
    }

    public enum CodeRVRReadingType
    {
        OTHER=0,
        TDZ=1,
        MID=2,
        TO=3,
    }

    public enum CodeHoldingCategoryType
    {
        OTHER,
        NON_PRECISION,
        CAT_I,
        CAT_II_III
    }

    public enum CodeLightHoldingPositionType
    {
        OTHER,
        STOP_BAR,
        SIGN,
    }

    public enum CodeVASISType
    {
        OTHER,
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
        LCVASI,
    }

    public enum CodeTLOFSectionType
    {
        OTHER,
        AIM,
        EDGE,
    }

    public enum CodeWorkAreaType
    {
        OTHER,
        CONSTRUCTION,
        SURFACEWORK,
        PARKED
    }

    public enum CodeMilitaryOperationsType
    {
        OTHER,
        CIVIL,
        MIL,
    }

    public enum CodeRoadType
    {
        OTHER,
        SERVICE,
        PUBLIC
    }

    public enum CodeUnitType
    {
        OTHER,
        VDF,
        MET,
        AOF,
    }

    public enum CodeRVRDirectionType
    {
        LEFT,
        RIGHT,
    }

    public enum VerticalStructureGeoType
    {
        OTHER = 0,
        POINT = 1,
        LINE = 2,
        POLYGON = 3,
    }

    public enum CodeRadioFrequencyAreaType
    {
        OTHER = 0,
        COV =1,
        T_COV =2,
        SCL =3,
        RHG =4,
        UNREL =5,
        RES =6,
        UUS =7,
        OUT =8,
        ESV =9,       
    }

    public enum CodeRadioSignalType
    {
        OTHER =0,
        AZIMUTH =1,
        DISTANCE =2,
        BEAM =3,
        VOICE =4,
        DATALINK =5

    }

    public enum CodeAirspaceActivity
    {
        OTHER = 0,
        AD_TFC = 1,
        HELI_TFC = 2,
        TRAINING = 3,
        AEROBATICS = 4,
        AIRSHOW = 5,
        SPORT = 6,
        ULM = 7,
        GLIDING = 8,
        PARAGLIDER = 9,
        HANGGLIDING = 10,
        PARACHUTE = 11,
        AIR_DROP = 12,
        BALLOON = 13,
        RADIOSONDE = 14,
        SPACE_FLIGHT = 15,
        UAV = 16,
        AERIAL_WORK = 17,
        CROP_DUSTING = 18,
        FIRE_FIGHTING = 19,
        MILOPS = 20,
        REFUEL = 21,
        JET_CLIMBING = 22,
        EXERCISE = 23,
        TOWING = 24,
        NAVAL_EXER = 25,
        MISSILES = 26,
        AIR_GUN = 27,
        ARTILLERY = 28,
        SHOOTING = 29,
        BLASTING = 30,
        WATER_BLASTING = 31,
        ANTI_HAIL = 32,
        BIRD = 33,
        BIRD_MIGRATION = 34,
        FIREWORK = 35,
        HI_RADIO = 36,
        HI_LIGHT = 37,
        LASER = 38,
        NATURE = 39,
        FAUNA = 40,
        NO_NOISE = 41,
        ACCIDENT = 42,
        POPULATION = 43,
        VIP = 44,
        VIP_PRES = 45,
        VIP_VICE = 46,
        OIL = 47,
        GAS = 48,
        REFINERY = 49,
        CHEMICAL = 50,
        NUCLEAR = 51,
        TECHNICAL = 52,
        ATS = 53,
        PROCEDURE =54
    }

    public enum CodeAirspaceAggregation
    {
        OTHER =0,
        BASE =1,
        UNION =2,
        INTERS =3,
        SUBTR =4
    }

    public enum dataInterpretation
    {
        SNAPSHOT = 0,
        BASELINE = 1,
        PERMDELTA = 2,
        TEMPDELTA = 3,
    }

    public enum CodeAirspaceDependency
    {
        FULL_GEOMETRY = 0,
        HORZ_PROJECTION = 1,
        OTHER = 2,
    }

}
