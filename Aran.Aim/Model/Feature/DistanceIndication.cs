using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Features;
using Aran.Aim.DataTypes;
using Aran.Aim.Enums;

namespace Aran.Aim.Features
{
	public class DistanceIndication : Feature
	{
		public override FeatureType FeatureType
		{
			get { return FeatureType.DistanceIndication; }
		}
		
		public ValDistance Distance
		{
			get { return (ValDistance ) GetValue ((int) PropertyDistanceIndication.Distance); }
			set { SetValue ((int) PropertyDistanceIndication.Distance, value); }
		}
		
		public ValDistanceVertical MinimumReceptionAltitude
		{
			get { return (ValDistanceVertical ) GetValue ((int) PropertyDistanceIndication.MinimumReceptionAltitude); }
			set { SetValue ((int) PropertyDistanceIndication.MinimumReceptionAltitude, value); }
		}
		
		public CodeDistanceIndication? Type
		{
			get { return GetNullableFieldValue <CodeDistanceIndication> ((int) PropertyDistanceIndication.Type); }
			set { SetNullableFieldValue <CodeDistanceIndication> ((int) PropertyDistanceIndication.Type, value); }
		}
		
		public FeatureRef Fix
		{
			get { return (FeatureRef ) GetValue ((int) PropertyDistanceIndication.Fix); }
			set { SetValue ((int) PropertyDistanceIndication.Fix, value); }
		}
		
		public SignificantPoint PointChoice
		{
			get { return GetObject <SignificantPoint> ((int) PropertyDistanceIndication.PointChoice); }
			set { SetValue ((int) PropertyDistanceIndication.PointChoice, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyDistanceIndication
	{
		Distance = PropertyFeature.NEXT_CLASS,
		MinimumReceptionAltitude,
		Type,
		Fix,
		PointChoice,
		NEXT_CLASS
	}
	
	public static class MetadataDistanceIndication
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataDistanceIndication ()
		{
			PropInfoList = MetadataFeature.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyDistanceIndication.Distance, (int) DataType.ValDistance, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyDistanceIndication.MinimumReceptionAltitude, (int) DataType.ValDistanceVertical, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyDistanceIndication.Type, (int) EnumType.CodeDistanceIndication, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyDistanceIndication.Fix, (int) FeatureType.DesignatedPoint, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyDistanceIndication.PointChoice, (int) ObjectType.SignificantPoint, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyDBEntity.Annotation, (int) ObjectType.Note, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyFeature.Metadata, (int) ObjectType.MdMetadata, PropertyTypeCharacter.Nullable);
		}
	}
}
