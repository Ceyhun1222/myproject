using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Metadata;

namespace Aran.Aim.Metadata
{
	public class MessageMetadata : MDMetadata
	{
		public override ObjectType ObjectType
		{
			get { return ObjectType.MessageMetadata; }
		}
		
		public ResponsibleParty Contact
		{
			get { return (ResponsibleParty ) GetValue ((int) PropertyMessageMetadata.Contact); }
			set { SetValue ((int) PropertyMessageMetadata.Contact, value); }
		}
		
		public List <MDConstraints> MetadataConstraints
		{
			get { return GetObjectList <MDConstraints> ((int) PropertyMessageMetadata.MetadataConstraints); }
		}
		
		public IdentificationMessage MessageIdentificationInfo
		{
			get { return GetObject <IdentificationMessage> ((int) PropertyMessageMetadata.MessageIdentificationInfo); }
			set { SetValue ((int) PropertyMessageMetadata.MessageIdentificationInfo, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyMessageMetadata
	{
		Contact = PropertyMDMetadata.NEXT_CLASS,
		MetadataConstraints,
		MessageIdentificationInfo,
		NEXT_CLASS
	}
	
	public static class MetadataMessageMetadata
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataMessageMetadata ()
		{
			PropInfoList = MetadataMDMetadata.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyMessageMetadata.Contact, (int) DataType.ResponsibleParty, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyMessageMetadata.MetadataConstraints, (int) ObjectType.MDConstraints, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyMessageMetadata.MessageIdentificationInfo, (int) ObjectType.IdentificationMessage, PropertyTypeCharacter.Nullable);
		}
	}
}
