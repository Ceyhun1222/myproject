using System;

namespace Aran.Aim
{
    public enum AimObjectType
    {
        Field = AllAimObjectType._1_FIELDTYPE,
        DataType = AllAimObjectType._2_DATATYPE,
        Object = AllAimObjectType._3_OBJECT,
        Feature = AllAimObjectType._4_FEATURE
    }

    public enum AimFieldType
    {
        SysGuid = AllAimObjectType.SysGuid,
        SysDouble = AllAimObjectType.SysDouble,
        SysString = AllAimObjectType.SysString,
        SysUInt32 = AllAimObjectType.SysUInt32,
        SysInt32 = AllAimObjectType.SysInt32,
        SysInt64 = AllAimObjectType.SysInt64,
        SysBool = AllAimObjectType.SysBool,
        SysDateTime = AllAimObjectType.SysDateTime,
        GeoPoint = AllAimObjectType.GeoPoint,
        GeoPolyline = AllAimObjectType.GeoPolyline,
        GeoPolygon = AllAimObjectType.GeoPolygon,
        SysEnum = AllAimObjectType.SysEnum
    }

    [Flags]
    public enum PropertyTypeCharacter
    {
        None = 0,
        Nullable = 0x8000,
        List = 0x4000,
        Mask = 0xC000
    }

    public enum AimSubClassType
    {
        None,
        Choice,
        AbstractFeatureRef,
        ValClass,
        Enum
    }

    public enum AbstractFeatureType
    {
        GroundLightSystem = AllAimObjectType.GroundLightSystem,
        Marking = AllAimObjectType.Marking,
        AirportHeliportProtectionArea = AllAimObjectType.AirportHeliportProtectionArea,
        RadarEquipment = AllAimObjectType.RadarEquipment,
        SurveillanceRadar = AllAimObjectType.SurveillanceRadar,
        Service = AllAimObjectType.Service,
        TrafficSeparationService = AllAimObjectType.TrafficSeparationService,
        AirportGroundService = AllAimObjectType.AirportGroundService,
        NavaidEquipment = AllAimObjectType.NavaidEquipment,
        NavigationSystemCheckpoint = AllAimObjectType.NavigationSystemCheckpoint,
        Procedure = AllAimObjectType.Procedure,
        SegmentLeg = AllAimObjectType.SegmentLeg,
        ApproachLeg = AllAimObjectType.ApproachLeg,
    }

    public enum CodeValDistanceOtherValue
    {
        UNL,
        GND,
        FLOOR,
        CEILING
    }

	public enum NilReason
	{
		inapplicable,
		missing,
		template,
		unknown,
		withheld
	}
}

namespace Aran.Aim.Enums
{
    public enum language { ENG, LV }

    public enum CodeCommunicationChannel { }
}