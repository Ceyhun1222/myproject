using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Features;
using Aran.Aim.Enums;
using Aran.Aim.DataTypes;

namespace Aran.Aim.Features
{
	public class Airspace : Feature
	{
		public override FeatureType FeatureType
		{
			get { return FeatureType.Airspace; }
		}
		
		public CodeAirspace? Type
		{
			get { return GetNullableFieldValue <CodeAirspace> ((int) PropertyAirspace.Type); }
			set { SetNullableFieldValue <CodeAirspace> ((int) PropertyAirspace.Type, value); }
		}
		
		public string Designator
		{
			get { return GetFieldValue <string> ((int) PropertyAirspace.Designator); }
			set { SetFieldValue <string> ((int) PropertyAirspace.Designator, value); }
		}
		
		public string LocalType
		{
			get { return GetFieldValue <string> ((int) PropertyAirspace.LocalType); }
			set { SetFieldValue <string> ((int) PropertyAirspace.LocalType, value); }
		}
		
		public string Name
		{
			get { return GetFieldValue <string> ((int) PropertyAirspace.Name); }
			set { SetFieldValue <string> ((int) PropertyAirspace.Name, value); }
		}
		
		public bool? DesignatorICAO
		{
			get { return GetNullableFieldValue <bool> ((int) PropertyAirspace.DesignatorICAO); }
			set { SetNullableFieldValue <bool> ((int) PropertyAirspace.DesignatorICAO, value); }
		}
		
		public CodeMilitaryOperations? ControlType
		{
			get { return GetNullableFieldValue <CodeMilitaryOperations> ((int) PropertyAirspace.ControlType); }
			set { SetNullableFieldValue <CodeMilitaryOperations> ((int) PropertyAirspace.ControlType, value); }
		}
		
		public ValFL UpperLowerSeparation
		{
			get { return (ValFL ) GetValue ((int) PropertyAirspace.UpperLowerSeparation); }
			set { SetValue ((int) PropertyAirspace.UpperLowerSeparation, value); }
		}
		
		public List <AirspaceLayerClass> Class
		{
			get { return GetObjectList <AirspaceLayerClass> ((int) PropertyAirspace.Class); }
		}
		
		public FeatureRef ProtectedRoute
		{
			get { return (FeatureRef ) GetValue ((int) PropertyAirspace.ProtectedRoute); }
			set { SetValue ((int) PropertyAirspace.ProtectedRoute, value); }
		}
		
		public List <AirspaceGeometryComponent> GeometryComponent
		{
			get { return GetObjectList <AirspaceGeometryComponent> ((int) PropertyAirspace.GeometryComponent); }
		}
		
		public List <AirspaceActivation> Activation
		{
			get { return GetObjectList <AirspaceActivation> ((int) PropertyAirspace.Activation); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyAirspace
	{
		Type = PropertyFeature.NEXT_CLASS,
		Designator,
		LocalType,
		Name,
		DesignatorICAO,
		ControlType,
		UpperLowerSeparation,
		Class,
		ProtectedRoute,
		GeometryComponent,
		Activation,
		NEXT_CLASS
	}
	
	public static class MetadataAirspace
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataAirspace ()
		{
			PropInfoList = MetadataFeature.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyAirspace.Type, (int) EnumType.CodeAirspace, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyAirspace.Designator, (int) AimFieldType.SysString, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyAirspace.LocalType, (int) AimFieldType.SysString, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyAirspace.Name, (int) AimFieldType.SysString, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyAirspace.DesignatorICAO, (int) AimFieldType.SysBool, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyAirspace.ControlType, (int) EnumType.CodeMilitaryOperations, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyAirspace.UpperLowerSeparation, (int) DataType.ValFL, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyAirspace.Class, (int) ObjectType.AirspaceLayerClass, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyAirspace.ProtectedRoute, (int) FeatureType.Route, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyAirspace.GeometryComponent, (int) ObjectType.AirspaceGeometryComponent, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyAirspace.Activation, (int) ObjectType.AirspaceActivation, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyDBEntity.Annotation, (int) ObjectType.Note, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyFeature.Metadata, (int) ObjectType.MdMetadata, PropertyTypeCharacter.Nullable);
		}
	}
}
