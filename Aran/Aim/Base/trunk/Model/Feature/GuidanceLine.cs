using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Features;
using Aran.Aim.Enums;
using Aran.Aim.DataTypes;
using Aran.Aim.Objects;

namespace Aran.Aim.Features
{
	public class GuidanceLine : Feature
	{
		public override FeatureType FeatureType
		{
			get { return FeatureType.GuidanceLine; }
		}
		
		public string Designator
		{
			get { return GetFieldValue <string> ((int) PropertyGuidanceLine.Designator); }
			set { SetFieldValue <string> ((int) PropertyGuidanceLine.Designator, value); }
		}
		
		public CodeGuidanceLine? Type
		{
			get { return GetNullableFieldValue <CodeGuidanceLine> ((int) PropertyGuidanceLine.Type); }
			set { SetNullableFieldValue <CodeGuidanceLine> ((int) PropertyGuidanceLine.Type, value); }
		}
		
		public ValSpeed MaxSpeed
		{
			get { return (ValSpeed ) GetValue ((int) PropertyGuidanceLine.MaxSpeed); }
			set { SetValue ((int) PropertyGuidanceLine.MaxSpeed, value); }
		}
		
		public CodeDirection? UsageDirection
		{
			get { return GetNullableFieldValue <CodeDirection> ((int) PropertyGuidanceLine.UsageDirection); }
			set { SetNullableFieldValue <CodeDirection> ((int) PropertyGuidanceLine.UsageDirection, value); }
		}
		
		public List <FeatureRefObject> ConnectedTouchDownLiftOff
		{
			get { return GetObjectList <FeatureRefObject> ((int) PropertyGuidanceLine.ConnectedTouchDownLiftOff); }
		}
		
		public List <FeatureRefObject> ConnectedRunwayCentrelinePoint
		{
			get { return GetObjectList <FeatureRefObject> ((int) PropertyGuidanceLine.ConnectedRunwayCentrelinePoint); }
		}
		
		public List <FeatureRefObject> ConnectedApron
		{
			get { return GetObjectList <FeatureRefObject> ((int) PropertyGuidanceLine.ConnectedApron); }
		}
		
		public List <FeatureRefObject> ConnectedStand
		{
			get { return GetObjectList <FeatureRefObject> ((int) PropertyGuidanceLine.ConnectedStand); }
		}
		
		public ElevatedCurve Extent
		{
			get { return GetObject <ElevatedCurve> ((int) PropertyGuidanceLine.Extent); }
			set { SetValue ((int) PropertyGuidanceLine.Extent, value); }
		}
		
		public List <FeatureRefObject> ConnectedTaxiway
		{
			get { return GetObjectList <FeatureRefObject> ((int) PropertyGuidanceLine.ConnectedTaxiway); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyGuidanceLine
	{
		Designator = PropertyFeature.NEXT_CLASS,
		Type,
		MaxSpeed,
		UsageDirection,
		ConnectedTouchDownLiftOff,
		ConnectedRunwayCentrelinePoint,
		ConnectedApron,
		ConnectedStand,
		Extent,
		ConnectedTaxiway,
		NEXT_CLASS
	}
	
	public static class MetadataGuidanceLine
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataGuidanceLine ()
		{
			PropInfoList = MetadataFeature.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyGuidanceLine.Designator, (int) AimFieldType.SysString, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyGuidanceLine.Type, (int) EnumType.CodeGuidanceLine, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyGuidanceLine.MaxSpeed, (int) DataType.ValSpeed, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyGuidanceLine.UsageDirection, (int) EnumType.CodeDirection, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyGuidanceLine.ConnectedTouchDownLiftOff, (int) FeatureType.TouchDownLiftOff, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyGuidanceLine.ConnectedRunwayCentrelinePoint, (int) FeatureType.RunwayCentrelinePoint, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyGuidanceLine.ConnectedApron, (int) FeatureType.Apron, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyGuidanceLine.ConnectedStand, (int) FeatureType.AircraftStand, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyGuidanceLine.Extent, (int) ObjectType.ElevatedCurve, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyGuidanceLine.ConnectedTaxiway, (int) FeatureType.Taxiway, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyDBEntity.Annotation, (int) ObjectType.Note, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyFeature.Metadata, (int) ObjectType.MdMetadata, PropertyTypeCharacter.Nullable);
		}
	}
}
