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
	public class TaxiHoldingPosition : Feature
	{
		public override FeatureType FeatureType
		{
			get { return FeatureType.TaxiHoldingPosition; }
		}
		
		public CodeHoldingCategory? LandingCategory
		{
			get { return GetNullableFieldValue <CodeHoldingCategory> ((int) PropertyTaxiHoldingPosition.LandingCategory); }
			set { SetNullableFieldValue <CodeHoldingCategory> ((int) PropertyTaxiHoldingPosition.LandingCategory, value); }
		}
		
		public CodeStatusOperations? Status
		{
			get { return GetNullableFieldValue <CodeStatusOperations> ((int) PropertyTaxiHoldingPosition.Status); }
			set { SetNullableFieldValue <CodeStatusOperations> ((int) PropertyTaxiHoldingPosition.Status, value); }
		}
		
		public FeatureRef AssociatedGuidanceLine
		{
			get { return (FeatureRef ) GetValue ((int) PropertyTaxiHoldingPosition.AssociatedGuidanceLine); }
			set { SetValue ((int) PropertyTaxiHoldingPosition.AssociatedGuidanceLine, value); }
		}
		
		public List <FeatureRefObject> ProtectedRunway
		{
			get { return GetObjectList <FeatureRefObject> ((int) PropertyTaxiHoldingPosition.ProtectedRunway); }
		}
		
		public ElevatedPoint Location
		{
			get { return GetObject <ElevatedPoint> ((int) PropertyTaxiHoldingPosition.Location); }
			set { SetValue ((int) PropertyTaxiHoldingPosition.Location, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyTaxiHoldingPosition
	{
		LandingCategory = PropertyFeature.NEXT_CLASS,
		Status,
		AssociatedGuidanceLine,
		ProtectedRunway,
		Location,
		NEXT_CLASS
	}
	
	public static class MetadataTaxiHoldingPosition
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataTaxiHoldingPosition ()
		{
			PropInfoList = MetadataFeature.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyTaxiHoldingPosition.LandingCategory, (int) EnumType.CodeHoldingCategory, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyTaxiHoldingPosition.Status, (int) EnumType.CodeStatusOperations, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyTaxiHoldingPosition.AssociatedGuidanceLine, (int) FeatureType.GuidanceLine, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyTaxiHoldingPosition.ProtectedRunway, (int) FeatureType.Runway, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyTaxiHoldingPosition.Location, (int) ObjectType.ElevatedPoint, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyDBEntity.Annotation, (int) ObjectType.Note, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyFeature.Metadata, (int) ObjectType.MdMetadata, PropertyTypeCharacter.Nullable);
		}
	}
}
