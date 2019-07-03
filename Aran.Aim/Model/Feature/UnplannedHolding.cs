using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Features;
using Aran.Aim.Enums;
using Aran.Aim.DataTypes;

namespace Aran.Aim.Features
{
	public class UnplannedHolding : Feature
	{
		public override FeatureType FeatureType
		{
			get { return FeatureType.UnplannedHolding; }
		}
		
		public CodeApproval? UnplannedHoldingP
		{
			get { return GetNullableFieldValue <CodeApproval> ((int) PropertyUnplannedHolding.UnplannedHoldingP); }
			set { SetNullableFieldValue <CodeApproval> ((int) PropertyUnplannedHolding.UnplannedHoldingP, value); }
		}
		
		public ValDistanceVertical AuthorizedAltitude
		{
			get { return (ValDistanceVertical ) GetValue ((int) PropertyUnplannedHolding.AuthorizedAltitude); }
			set { SetValue ((int) PropertyUnplannedHolding.AuthorizedAltitude, value); }
		}
		
		public CodeVerticalReference? AltitudeReference
		{
			get { return GetNullableFieldValue <CodeVerticalReference> ((int) PropertyUnplannedHolding.AltitudeReference); }
			set { SetNullableFieldValue <CodeVerticalReference> ((int) PropertyUnplannedHolding.AltitudeReference, value); }
		}
		
		public bool? ControlledAirspace
		{
			get { return GetNullableFieldValue <bool> ((int) PropertyUnplannedHolding.ControlledAirspace); }
			set { SetNullableFieldValue <bool> ((int) PropertyUnplannedHolding.ControlledAirspace, value); }
		}
		
		public SegmentPoint HoldingPoint
		{
			get { return (SegmentPoint ) GetAbstractObject ((int) PropertyUnplannedHolding.HoldingPoint, AbstractType.SegmentPoint); }
			set { SetValue ((int) PropertyUnplannedHolding.HoldingPoint, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyUnplannedHolding
	{
		UnplannedHoldingP = PropertyFeature.NEXT_CLASS,
		AuthorizedAltitude,
		AltitudeReference,
		ControlledAirspace,
		HoldingPoint,
		NEXT_CLASS
	}
	
	public static class MetadataUnplannedHolding
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataUnplannedHolding ()
		{
			PropInfoList = MetadataFeature.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyUnplannedHolding.UnplannedHoldingP, (int) EnumType.CodeApproval, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyUnplannedHolding.AuthorizedAltitude, (int) DataType.ValDistanceVertical, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyUnplannedHolding.AltitudeReference, (int) EnumType.CodeVerticalReference, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyUnplannedHolding.ControlledAirspace, (int) AimFieldType.SysBool, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyUnplannedHolding.HoldingPoint, (int) AbstractType.SegmentPoint, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyDBEntity.Annotation, (int) ObjectType.Note, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyFeature.Metadata, (int) ObjectType.MdMetadata, PropertyTypeCharacter.Nullable);
		}
	}
}
