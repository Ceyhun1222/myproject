using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Features;
using Aran.Aim.Enums;
using Aran.Aim.DataTypes;

namespace Aran.Aim.Features
{
	public class TaxiwayMarking : Marking
	{
		public override FeatureType FeatureType
		{
			get { return FeatureType.TaxiwayMarking; }
		}
		
		public CodeTaxiwaySection? MarkingLocation
		{
			get { return GetNullableFieldValue <CodeTaxiwaySection> ((int) PropertyTaxiwayMarking.MarkingLocation); }
			set { SetNullableFieldValue <CodeTaxiwaySection> ((int) PropertyTaxiwayMarking.MarkingLocation, value); }
		}
		
		public FeatureRef MarkedTaxiway
		{
			get { return (FeatureRef ) GetValue ((int) PropertyTaxiwayMarking.MarkedTaxiway); }
			set { SetValue ((int) PropertyTaxiwayMarking.MarkedTaxiway, value); }
		}
		
		public FeatureRef MarkedElement
		{
			get { return (FeatureRef ) GetValue ((int) PropertyTaxiwayMarking.MarkedElement); }
			set { SetValue ((int) PropertyTaxiwayMarking.MarkedElement, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyTaxiwayMarking
	{
		MarkingLocation = PropertyMarking.NEXT_CLASS,
		MarkedTaxiway,
		MarkedElement,
		NEXT_CLASS
	}
	
	public static class MetadataTaxiwayMarking
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataTaxiwayMarking ()
		{
			PropInfoList = MetadataMarking.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyTaxiwayMarking.MarkingLocation, (int) EnumType.CodeTaxiwaySection, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyTaxiwayMarking.MarkedTaxiway, (int) FeatureType.Taxiway, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyTaxiwayMarking.MarkedElement, (int) FeatureType.TaxiwayElement, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyFeature.Metadata, (int) ObjectType.MdMetadata, PropertyTypeCharacter.Nullable);
		}
	}
}
