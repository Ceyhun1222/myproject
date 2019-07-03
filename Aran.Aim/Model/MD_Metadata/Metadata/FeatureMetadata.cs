using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Metadata;

namespace Aran.Aim.Metadata
{
	public class FeatureMetadata : MDMetadata
	{
		public override ObjectType ObjectType
		{
			get { return ObjectType.FeatureMetadata; }
		}
		
		public ResponsibleParty Contact
		{
			get { return (ResponsibleParty ) GetValue ((int) PropertyFeatureMetadata.Contact); }
			set { SetValue ((int) PropertyFeatureMetadata.Contact, value); }
		}
		
		public IdentificationFeature FeatureIdentificationInfo
		{
			get { return GetObject <IdentificationFeature> ((int) PropertyFeatureMetadata.FeatureIdentificationInfo); }
			set { SetValue ((int) PropertyFeatureMetadata.FeatureIdentificationInfo, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyFeatureMetadata
	{
		Contact = PropertyMDMetadata.NEXT_CLASS,
		FeatureIdentificationInfo,
		NEXT_CLASS
	}
	
	public static class MetadataFeatureMetadata
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataFeatureMetadata ()
		{
			PropInfoList = MetadataMDMetadata.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyFeatureMetadata.Contact, (int) DataType.ResponsibleParty, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyFeatureMetadata.FeatureIdentificationInfo, (int) ObjectType.IdentificationFeature, PropertyTypeCharacter.Nullable);
		}
	}
}
