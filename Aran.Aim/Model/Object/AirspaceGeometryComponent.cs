using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Objects;
using Aran.Aim.Enums;
using Aran.Aim.Features;

namespace Aran.Aim.Features
{
	public class AirspaceGeometryComponent : AObject
	{
		public override ObjectType ObjectType
		{
			get { return ObjectType.AirspaceGeometryComponent; }
		}
		
		public CodeAirspaceAggregation? Operation
		{
			get { return GetNullableFieldValue <CodeAirspaceAggregation> ((int) PropertyAirspaceGeometryComponent.Operation); }
			set { SetNullableFieldValue <CodeAirspaceAggregation> ((int) PropertyAirspaceGeometryComponent.Operation, value); }
		}
		
		public uint? OperationSequence
		{
			get { return GetNullableFieldValue <uint> ((int) PropertyAirspaceGeometryComponent.OperationSequence); }
			set { SetNullableFieldValue <uint> ((int) PropertyAirspaceGeometryComponent.OperationSequence, value); }
		}
		
		public AirspaceVolume TheAirspaceVolume
		{
			get { return GetObject <AirspaceVolume> ((int) PropertyAirspaceGeometryComponent.TheAirspaceVolume); }
			set { SetValue ((int) PropertyAirspaceGeometryComponent.TheAirspaceVolume, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyAirspaceGeometryComponent
	{
		Operation = PropertyAObject.NEXT_CLASS,
		OperationSequence,
		TheAirspaceVolume,
		NEXT_CLASS
	}
	
	public static class MetadataAirspaceGeometryComponent
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataAirspaceGeometryComponent ()
		{
			PropInfoList = MetadataAObject.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyAirspaceGeometryComponent.Operation, (int) EnumType.CodeAirspaceAggregation, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyAirspaceGeometryComponent.OperationSequence, (int) AimFieldType.SysUInt32, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyDBEntity.Annotation, (int) ObjectType.Note, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyAirspaceGeometryComponent.TheAirspaceVolume, (int) ObjectType.AirspaceVolume, PropertyTypeCharacter.Nullable);
		}
	}
}
