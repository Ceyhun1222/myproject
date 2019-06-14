using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Features;
using Aran.Aim.DataTypes;
using Aran.Aim.Enums;

namespace Aran.Aim.Features
{
	public class VerticalStructurePart : PropertiesWithSchedule
	{
		public override ObjectType ObjectType
		{
			get { return ObjectType.VerticalStructurePart; }
		}
		
		public ValDistance VerticalExtent
		{
			get { return (ValDistance ) GetValue ((int) PropertyVerticalStructurePart.VerticalExtent); }
			set { SetValue ((int) PropertyVerticalStructurePart.VerticalExtent, value); }
		}
		
		public ValDistance VerticalExtentAccuracy
		{
			get { return (ValDistance ) GetValue ((int) PropertyVerticalStructurePart.VerticalExtentAccuracy); }
			set { SetValue ((int) PropertyVerticalStructurePart.VerticalExtentAccuracy, value); }
		}
		
		public CodeVerticalStructure? Type
		{
			get { return GetNullableFieldValue <CodeVerticalStructure> ((int) PropertyVerticalStructurePart.Type); }
			set { SetNullableFieldValue <CodeVerticalStructure> ((int) PropertyVerticalStructurePart.Type, value); }
		}
		
		public CodeStatusConstruction? ConstructionStatus
		{
			get { return GetNullableFieldValue <CodeStatusConstruction> ((int) PropertyVerticalStructurePart.ConstructionStatus); }
			set { SetNullableFieldValue <CodeStatusConstruction> ((int) PropertyVerticalStructurePart.ConstructionStatus, value); }
		}
		
		public CodeVerticalStructureMarking? MarkingPattern
		{
			get { return GetNullableFieldValue <CodeVerticalStructureMarking> ((int) PropertyVerticalStructurePart.MarkingPattern); }
			set { SetNullableFieldValue <CodeVerticalStructureMarking> ((int) PropertyVerticalStructurePart.MarkingPattern, value); }
		}
		
		public CodeColour? MarkingFirstColour
		{
			get { return GetNullableFieldValue <CodeColour> ((int) PropertyVerticalStructurePart.MarkingFirstColour); }
			set { SetNullableFieldValue <CodeColour> ((int) PropertyVerticalStructurePart.MarkingFirstColour, value); }
		}
		
		public CodeColour? MarkingSecondColour
		{
			get { return GetNullableFieldValue <CodeColour> ((int) PropertyVerticalStructurePart.MarkingSecondColour); }
			set { SetNullableFieldValue <CodeColour> ((int) PropertyVerticalStructurePart.MarkingSecondColour, value); }
		}
		
		public bool? Mobile
		{
			get { return GetNullableFieldValue <bool> ((int) PropertyVerticalStructurePart.Mobile); }
			set { SetNullableFieldValue <bool> ((int) PropertyVerticalStructurePart.Mobile, value); }
		}
		
		public bool? Frangible
		{
			get { return GetNullableFieldValue <bool> ((int) PropertyVerticalStructurePart.Frangible); }
			set { SetNullableFieldValue <bool> ((int) PropertyVerticalStructurePart.Frangible, value); }
		}
		
		public CodeVerticalStructureMaterial? VisibleMaterial
		{
			get { return GetNullableFieldValue <CodeVerticalStructureMaterial> ((int) PropertyVerticalStructurePart.VisibleMaterial); }
			set { SetNullableFieldValue <CodeVerticalStructureMaterial> ((int) PropertyVerticalStructurePart.VisibleMaterial, value); }
		}
		
		public string Designator
		{
			get { return GetFieldValue <string> ((int) PropertyVerticalStructurePart.Designator); }
			set { SetFieldValue <string> ((int) PropertyVerticalStructurePart.Designator, value); }
		}
		
		public VerticalStructurePartGeometry HorizontalProjection
		{
			get { 
                return GetObject <VerticalStructurePartGeometry> ((int) PropertyVerticalStructurePart.HorizontalProjection); }
			set { SetValue ((int) PropertyVerticalStructurePart.HorizontalProjection, value); }
		}
		
		public List <LightElement> Lighting
		{
			get { 
                return GetObjectList <LightElement> ((int) PropertyVerticalStructurePart.Lighting); 
            }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyVerticalStructurePart
	{
		VerticalExtent = PropertyPropertiesWithSchedule.NEXT_CLASS,
		VerticalExtentAccuracy,
		Type,
		ConstructionStatus,
		MarkingPattern,
		MarkingFirstColour,
		MarkingSecondColour,
		Mobile,
		Frangible,
		VisibleMaterial,
		Designator,
		HorizontalProjection,
		Lighting,
		NEXT_CLASS
	}
	
	public static class MetadataVerticalStructurePart
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataVerticalStructurePart ()
		{
			PropInfoList = MetadataPropertiesWithSchedule.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyVerticalStructurePart.VerticalExtent, (int) DataType.ValDistance, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyVerticalStructurePart.VerticalExtentAccuracy, (int) DataType.ValDistance, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyVerticalStructurePart.Type, (int) EnumType.CodeVerticalStructure, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyVerticalStructurePart.ConstructionStatus, (int) EnumType.CodeStatusConstruction, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyVerticalStructurePart.MarkingPattern, (int) EnumType.CodeVerticalStructureMarking, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyVerticalStructurePart.MarkingFirstColour, (int) EnumType.CodeColour, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyVerticalStructurePart.MarkingSecondColour, (int) EnumType.CodeColour, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyVerticalStructurePart.Mobile, (int) AimFieldType.SysBool, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyVerticalStructurePart.Frangible, (int) AimFieldType.SysBool, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyVerticalStructurePart.VisibleMaterial, (int) EnumType.CodeVerticalStructureMaterial, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyVerticalStructurePart.Designator, (int) AimFieldType.SysString, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyVerticalStructurePart.HorizontalProjection, (int) ObjectType.VerticalStructurePartGeometry, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyVerticalStructurePart.Lighting, (int) ObjectType.LightElement, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
		}
	}
}
