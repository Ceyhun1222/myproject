using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Aerodrome.Enums
{
	public enum OperSystem_Type
	{
		[Description(" Reserved")]
		Reserved,
		
		[Description(" True if Runway Status Lights are published  in AIP for the aerodrome ")]
		RWSL,

		[Description ( "  True if Surface Movement Radar such as ASDE_X is published in AIP for the aerodrome " )]
		SMR,

		[Description ( " True if Low Visibility Operations is publish in AIP for the aerodrome " )]
		LVO,

		[Description ( " True if LVO is supported by a Surface Movement Guidance and Control System is published in AIP for the aerodrome " )]
		SMGCS,

		[Description ( " True if Individual Lamp Control and Monitoring Systems (Follow-the-Green) are published in AIP for the aerodrome " )]
		ILCMS,

		[Description ( " True if the aerodrome is published in the AIP with a Remote Tower " )]
		r_TWR,
		
		[Description(" True if D-TAXI is published in AIP for the aerodrome ")]
		d_TAXI
	}

    public enum FeatureNames
    {
        AM_AerodromeReferencePoint,      
        AM_TaxiwayElement,
        AM_RunwayCenterlinePoint,
        AM_TaxiwayIntersectionMarking,
        AM_TaxiwayHoldingPosition,
        AM_TaxiwayGuidanceLine,


    }

	public enum UomFrequency
	{
		MHZ
	}

	public enum RwyType
	{
		RWY,
		FATO
	}

	public enum UomSlope
	{
		percent
	}

	public enum UomBearing
	{
		degree
	}

	//public enum UomHorizontal
	//{
	//	meters,
	//	ft
	//}

	public enum UomHorResolution
	{
		Decimal_Degrees
	}

	public enum UomVerResolution
	{
		meters,
	}
	
	public enum UomSpeed
	{
		knots
	}

	public enum UomDistance
	{
		meters,
		ft
	}

	public enum Catstop
	{
		[Description ( "No_low_visibility_operation_supported" )]
		None = 0,
		[Description("Supports_ILS_CAT_I_low_visibility_operations")]
		CAT_I = 1,
		[Description("Supports_ILS_CAT_II_III_low_visibility_operations")] 
		CAT_II_III = 2,
		[Description("Supports_ILS_CAT_II_low_visibility_operations")]
		CAT_II = 3,
		[Description("Supports_ILS_CAT_III_low_visibility_operations")]
		CAT_III = 4,
		[Description("Supports_ILS_CAT_I_II_III_low_visibility_operations")] 
		CAT_I_II_III = 5,
		[Description("Supports_ILS_CAT_I_II_low_visibility_operations")]
		CAT_I_II = 6,
		[Description("Supports_ILS_CAT_I_III_low_visibility_operations")]
		CAT_I_III = 7
	}

	public enum Node_Type
	{
		Taxiway, //Node_where_two_taxiways_meet = 0,
		TaxiwayHoldingPosition, //Node_on_taxiway_holding_position = 1,
        RunwayEntryExit, //Node_where_a_runway_and_taxiway_meet = 2,
		RunwayExitLine, //Node_where_a_runway_exit_line_and_runway_intersection_meet = 3,
		RunwayIntersection, //Node_where_two_runways_meet = 4,
        ParkingEntryExit, //Node_joining_a_parking_area_to_a_taxiway = 5,
		ApronEntryExit, //Node_joining_an_apron_area_to_a_taxiway = 6,
		TaxiwayLink, //Node_on_a_taxiway_abeam_to_a_Parking_or_Apron_Entry_Exit_node = 7,
		Deicing, //Node_where_a_deicing_operation_may_be_performed = 8,
		Stand, //Node_where_a_stand_is_located = 9
	}

	public enum AM_Direction
	{
		Bidirectional = 0,
		[Description("One_way_from_start_to_endpoint_of_line")]
		Start_To_EndPoint,
		[Description("One_way_from_end_to_startpoint_of_line")]
		End_To_StartPoint
	}

	public enum VerticalDatum
	{
		EGM_96,
		AHD,
		NAVD88,
		OTHER_EGM_08,
		OTHER
	}

	public enum SignType
	{
		//Sign applies to something other than an aircraft or vehicle
		Other_Aircraft_Vehicle = 0,
		//Sign applies to both aircraft and vehicles
		Both_Aircraft_Vehicle = 1,
		//Sign applies to aircraft only
		Only_Aircraft = 2,
		//Sign applies to vehicles only
		Only_Vehicle = 3
	}

	public enum Edgederv
	{
		[Description ( "Defined without using existing line_data" )]
		Fully_abstracted,
		[Description ( "Partially derived from existing data and partially abstracted, for example in the cases where gaps exist " )]
		Partially_abstracted,
		[Description ( " Fully derived from existing data " )]
		Derived
	}

	public enum EdgeType
	{
		[Description ( " Edge along taxiway(s)  ")]
		Taxiway,

		[Description ( " Edge along runway(s)  " )]
		Runway,
		
		[Description ( " Edge connecting runway and taxiway  " )]
		Runway_Exit,
		
		[Description ( " Edge connecting Taxiway and parking entry/exit point " )]
		Parking,

		[Description ( "Reserved" )]
		Reserved,

		[Description ( " Edge connecting Deicing and taxiway / parking / apron " )]
		Deicing,
		
		[Description ( "  Edge along apron(s) " )]
		Apron,

		[Description ( "  Edge connecting Stand and Parking Entry/Exit " )]
		Stand
	}

	public enum AM_Color
	{
		Yellow = 0,
		Orange = 1,
		Blue = 2,
		White = 3,
        Green = 4,
        Red = 5
	}

	public enum Status
	{
		[Description("Closed or unusable or nonoperational")]
		Closed = 0,
		[Description("Open or usable or operational")]
		Open = 1
	}

	public enum StatusOperation
	{
		NORMAL,
		DOWNGRADED,
		UNSERVICEABLE,
		WORK_IN_PROGRES,
		OTHER
	}

	public enum Apron_Type
	{
		[Description ( "Reserved" )]
		Reserved = 0,

		[Description ( "General" )]
		General = 1,

		[Description ( " Penalty Box, Hold Pad, Holding Bay " )]
		Holding_Bay  = 2,

		[Description ( " Runway Turnaround Pad " )]
		Turnaround = 3
	}

	public enum NilReason
	{        
		NotEntered,
		Null,
		Unknown,
		NotApplicable,
        //None
	}

	public enum AM_InterpretationType
	{
		[Description ( "The snapshot contains the state of a feature at a time instant, merging any permanent changes and temporary changes" )]
		Snapshot,

		[Description ( "The baseline contains the state of a feature during a time period or a time instant, merging any permanent changes" )]
		Baseline,

		[Description ( "The data describes the temporary change of a feature" )]
		TEMPDELTA,

		[Description ( " 	The data describes the permanent change of a feature. Note: the integration of a permanent change creates a new baseline" )]
		PERMDELTA,

		[Description ( "The stream contains the state of a feature during a time period, merging any permanent changes and temporary changes" )]
		Stream
	}

	public enum Availability
	{
		Unavailable = 0,
		Available = 1
	}

	public enum FuelType
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

	public enum SurfaceComposition
	{
        Reserved,
        CONC_ASPH,
        CONC,
		Concrete_Grooved,
		Concrete_Non_Grooved,
		Asphalt_Grooved,
		Asphalt_Non_Grooved,
		Desert_or_Sand_or_Dirt,
		Bare_Earth,
		Snow_or_Ice,
		Water,
		Grass_or_Turf,
		Aggregate_Friction_Seal_Coat,
		Gravel_or_Cinders,
		Porous_Friction_Courses,
		Pierced_Steel_Planks,
		Rubberized_Friction_Seal_Coat,
		Bitumen,
		Brick,
		Macadam,
		Stone,
		Coral,
		Clay,
		Laterite,
		Landing_Mats,
		Membrane,
		Wood
	}

	public enum LowVisibilityOper_Type
	{
		Not_Used,
		Used
	}

	public enum Feat_Base
	{
		None = 0,
		Service_Road = 1,
		Apron_Element = 2,
		Taxiway_Element = 3,
		Parking_Stand_Area = 4,
		Stopway = 5,		
		Runway_Element = 7,
		Runway_Intersection = 8,
		Runway_Shoulder = 9,
        Final_Approach_And_Take_Off_Area = 10,
        TouchDown_Lift_Off_Area = 11,
		Taxiway_Shoulder = 12,
		Deicing_Area = 13,
		Construction_Area = 14,
		Blastpad = 15,
		Runway_Displaced_Area = 16,
	}

	public enum Feat_Type
	{
		Runway_Element = 0,
		Runway_Intersection = 1,
		Runway_Threshold = 2,
		Runway_Marking = 3,
		Painted_Centerline = 4,
		Land_And_Hold_Short_Operation_Location = 5, 
		Arresting_Gear_Location = 6,
		Runway_Shoulder = 7,
		Stopway = 8,
		Runway_Displaced_Area = 9,       
        Final_Approach_And_Take_Off_Area = 11,
		TouchDown_Lift_Off_Area = 12,
		Helipad_Threshold = 13,
		Taxiway_Element = 14,
		Taxiway_Shoulder = 15,
		Taxiway_Guidance_Line = 16,
		Taxiway_Intersection_Marking = 17,
		Taxiway_Holding_Position = 18,
		Runway_ExitLine = 19,
		Frequency_Area = 20,
		Apron_Element = 21,
		Stand_Guidance_Line = 22,
		Parking_Stand_Location = 23,
		Parking_Stand_Area = 24,
		Deicing_Area = 25,
		Aerodrome_Reference_Point = 26,
		Vertical_Polygonal_Structure = 27,
		Vertical_Point_Structure = 28,
		Vertical_Line_Structure = 29,
		Construction_Area = 30,
		Survey_Control_Point = 31,
		Aerodrome_Surface_Lighting = 32,
		Blastpad = 33,
		Service_Road = 34,
		Water = 35,
		Hotspot = 36,
		Runway_Centerline_Point = 37,
		Arresting_System_Location = 38,
		Asrn_Edge = 39,
		Asrn_Node = 40,
		Bridge_Side = 41,
		ATC_Blind_Spot = 42,
		Aerodrome_Sign = 43,
		Position_Marking = 44,
		Deicing_Group = 45,

		//Organisation = 250,
		Runway_Direction = 252,
		Runway = 253,
		Taxiway = 254,
		Apron = 255,
	}

	public enum AM_Style
	{
		Solid = 0,
		Dashed = 1,
		Dotted = 2,
		[Description ( "Enhanced Taxiway Centerline Marking ( leading up to holding position)" )]
		Enhanced
	}

	public enum VSPolygon_Type
	{
		Reserved = 0,
		Terminal_Building = 1,
		Hangar = 2,
		Control_Tower = 3,
		Non_Terminal_Building = 4,
		Tank = 5,
		Tree = 6,
		Bush = 7,
		Forest = 8,
		Earthen_Works = 9,
		Navaid = 10,
		Sign = 11,
		Fixed_Base_Operator = 12,
		Fire_Station = 13,
		Other
	}

	//Predominant surface material of the vertical structure
	public enum Material_Type
	{
		Reserved = 0,
		Concrete = 1,
		Metal = 2,
		Stone_or_Brick = 3,
		Composition = 4,
		Rock = 5,
		Earthen_Works = 6,
		Wood = 7
	}

	public enum VSPoint_Type
	{
		Reserved = 0,
		Smokestack = 1,
		Power_Line_Pylon = 2,
		Antenna = 3,
		Windsock = 4,
		Tree = 5,
		Lightpole = 6,
		Light_Stanchion = 7,
		Airport_Beacon = 8,
		Navaid = 9,
		Sign = 10
	}

	public enum VSLine_Type
	{
		Reserved = 0,
		Power_Line = 1,
		Cable_Railway = 2,
		Bushes_or_Trees = 3,
		Wall = 4,
		Navaid = 5,
		Sign = 6,
		Fence = 7,
		Blastfence = 8
	}

	public enum DropSide
	{
		//Drop is to the left
		Drop_Left = 0,
		//Drop is to the right
		Drop_Right = 1
	}

	public enum PmType
	{
		[Description ( " Generic position marking, low visibility hold point, geographic hold position marking, checkpoint" )]
		Position_Marking = 0,
		[Description(" Reporting point, taxi position fix")]
		Reporting_Point = 1,
		[Description(" Holding position, taxi hold spot, hold spot, hold circle, aircraft hold point")]
		Taxi_Hold_Point = 2,
		[Description(" UPS holding spot, spot")]
		Ramp_Control_Spot = 3,
		[Description(" Apron exit point, apron entrance point")]
		Apron_Exit_Point = 4
	}

	public enum BridgeType
	{
		[Description("No bridge")]
		None = 0,
		[Description("Passes under a bridge")]
		Underpass = 1,
		[Description ( "Bridge" )]
		Overpass = 2
	}

	public enum Holding_Direction
	{
		[Description ( "Non-directional or bi-directional holding line" )]
		None = 0,
		[Description ( "Holding side is on the left" )]
		VecLeft = 1,
		[Description ( "Holding side is on the right" )]
		VecRight = 2
	}

	public enum MarkingType
	{
		[Description("Conventional runway holding position marking (two solid and two dashed lines)")]
		Pattern_A = 0,
		[Description ( "ILS/MLS holding position marking (ladder)" )]
		Pattern_B = 1,
		[Description ( " ICAO intermediate holding position marking (single dashed line) " )]
		Intermediate = 2,
		[Description ( " FAA non-movement area boundary marking (single-solid and single-dashed lines) " )]
		Non_movement_Area = 3
	}

	public enum RwyEntryLightInstalled
	{
		[Description("Not_installed")]
		Not_installed = 0,
		[Description("Installed")]
		Installed
	}

	public enum AipPublishedType
	{
		[Description("Location is not published")]
		No = 0,
		[Description("Location is published")]
		Yes = 1
	}

	public enum LAHSO_Type
	{
		//Other
		Other = 0,
		[Description ( "A runway is being protected" )]
		Runway,
		[Description ( "A taxiway is being protected" )]
		Taxiway,
		[Description ( "A taxiway and a runway are being protected" )]
		Runway_Taxiway = 3,
		[Description ( "A flight path is being protected" )]
		Flight_Path = 4
	}

	public enum Rwy_Exit_Type
	{
		Reserved,
		[Description("General (not High Speed)")]
		General,
		[Description("High Speed exit")]
		High_Speed
	}

	public enum RwyMarking_Type
	{
		Reserve = 0,
		[Description ( "Threshold stripes" )]
		Threshold,
		[Description("Runway designation (e.g., 20L)")]
		Designation,
		Side_Stripes,
		Aiming_Point,
		Centerline,
		[Description("Touchdown zone")]
		TDZ,
		[Description("Stopway/blastpad chevrons")]
		Chevron,
		[Description("Displaced runway arrow")]
		Arrow,
		[Description("Displaced runway arrowheads")]
		Arrowhead
	}

	public enum Cat_Approach_Type
	{
		[Description("Non-precision approach runway")]
		NPA,
		[Description ( "Precision approach runway, category I" )] 
		CAT_I,
		[Description ( "Precision approach runway, category II" )]
		CAT_II,
		[Description ( "Precision approach runway, category III A" )]
		CAT_III_A,
		[Description ( "Precision approach runway, category III B" )]
		CAT_III_B,
		[Description ( "Precision approach runway, category III C" )] 
		CAT_III_C
	}

	public enum Vasis_Type
	{
		[Description("No visual slope indicator system")]
		None,
		PAPI,
		APAPI,
		VASIS,
		AVASIS,
	}

	public enum Thr_Type
	{
		Threshold,
		[Description("Displaced Threshold")]
		Displaced_Threshold
	}

	public enum TOHL_Installed_Type
	{
		Not_installed,
		Installed
	}

	public enum AproachLightSystem_Type
	{
		[Description("Approach Lighting System with Sequenced Flashing Lights configuration 1")]
		ALSF_1,
		[Description("Approach Lighting System with Sequence Flashing Lights configuration 2")] 
		ALSF_2,
		[Description("Medium Intensity Approach Lighting System")] 
		MALS,
		[Description("Medium Intensity Approach Lighting System with Sequenced Flashers")] 
		MALSF,
		[Description("Medium Approach Light System with Runway Alignment Indicator Lights")] 
		MALSR,
		[Description("Simple Approach Lighting System")] 
		SALS,
		[Description("Simplified Short Approach Lighting System")]
		SSALS,
		[Description("Simplified Short Approach Lighting System with Sequenced Flashing Lights")]
		SSALF,
		[Description("Simplified Short Approach Lighting System with Runway Alignment Indicator Lights")]
		SSALR,
		[Description("Omni Directional Approach Lighting System")] 
		ODALS,
		[Description("ICAO-compliant configuration 1 High Intensity Approach Lighting System (HIALS)")]
		CALVERT_I,
		[Description("ICAO-compliant configuration 2 High Intensity Approach Lighting System (HIALS)")]
		CALVERT_II,
		[Description("Lead-In Lighting System")]
		LDIN,
		[Description("Runway End Identifier Lights")]
		REIL,
		[Description("Runway Alignment Indicator Lights")]
		RAIL,
		[Description("US Air Force Overrun 1000 Foot Standard Approach Lighting System")]
		AFOVRN,
		[Description("Military Overrun")]
		MILOVRN,
		[Description ( "High Intensity Approach Lighting System" )]
		HIALS
	}

	public enum LightUse_Type
	{
		// Taxiway Edge Lights
		Taxiway_Edge_Lights,
		// Taxiway Centerline Lighting
		Taxiway_Centerline_Lighting,
		// Runway Edge Lighting 
		Runway_Edge_Lighting,
		// Runway Centerline Lighting 
		Runway_Centerline_Lighting,
		//   Threshold Lights 
		Threshold_Lights,
		// Touchdown Zone Lights 
		Touchdown_Zone_Lights,
		//   Helipad Lighting 
		  Helipad_Lighting,
		//   Runway Guard Lights 
		  Runway_Guard_Lights,
		//   Runway End Identification Lights 
		Runway_End_Identification_Lights,
		Approach_Light_Centerline,
		Approach_Light_Crossbar,

	}

	public enum LightSource_Type
	{
		Incandescent,
		LED,
		Both
	}
}