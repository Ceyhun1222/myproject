using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Metadata.ISO;

namespace Aran.Aim.Metadata.ISO
{
	public class MdMetadata : BtAbstractObject
	{
		public override ObjectType ObjectType
		{
			get { return ObjectType.MdMetadata; }
		}
		
		public string FileIdentifier
		{
			get { return GetFieldValue <string> ((int) PropertyMdMetadata.FileIdentifier); }
			set { SetFieldValue <string> ((int) PropertyMdMetadata.FileIdentifier, value); }
		}
		
		public string Language
		{
			get { return GetFieldValue <string> ((int) PropertyMdMetadata.Language); }
			set { SetFieldValue <string> ((int) PropertyMdMetadata.Language, value); }
		}
		
		public string ParentIdentifier
		{
			get { return GetFieldValue <string> ((int) PropertyMdMetadata.ParentIdentifier); }
			set { SetFieldValue <string> ((int) PropertyMdMetadata.ParentIdentifier, value); }
		}
		
		public List <CiResponsibleParty> Contact
		{
			get { return GetObjectList <CiResponsibleParty> ((int) PropertyMdMetadata.Contact); }
		}
		
		public DateTime? DateStamp
		{
			get { return GetNullableFieldValue <DateTime> ((int) PropertyMdMetadata.DateStamp); }
			set { SetNullableFieldValue <DateTime> ((int) PropertyMdMetadata.DateStamp, value); }
		}
		
		public string MetadataStandardName
		{
			get { return GetFieldValue <string> ((int) PropertyMdMetadata.MetadataStandardName); }
			set { SetFieldValue <string> ((int) PropertyMdMetadata.MetadataStandardName, value); }
		}
		
		public string MetadataStandardVersion
		{
			get { return GetFieldValue <string> ((int) PropertyMdMetadata.MetadataStandardVersion); }
			set { SetFieldValue <string> ((int) PropertyMdMetadata.MetadataStandardVersion, value); }
		}
		
		public string DataSetUri
		{
			get { return GetFieldValue <string> ((int) PropertyMdMetadata.DataSetUri); }
			set { SetFieldValue <string> ((int) PropertyMdMetadata.DataSetUri, value); }
		}
		
		public List <RsIdentifier> ReferenceSystemInfo
		{
			get { return GetObjectList <RsIdentifier> ((int) PropertyMdMetadata.ReferenceSystemInfo); }
		}
		
		public List <MdAbstractIdentificationObject> IdentificationInfo
		{
			get { return GetObjectList <MdAbstractIdentificationObject> ((int) PropertyMdMetadata.IdentificationInfo); }
		}
		
		public List <DqDataQuality> DataQualityInfo
		{
			get { return GetObjectList <DqDataQuality> ((int) PropertyMdMetadata.DataQualityInfo); }
		}
		
		public List <MdConstraintsObject> MetadataConstraints
		{
			get { return GetObjectList <MdConstraintsObject> ((int) PropertyMdMetadata.MetadataConstraints); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyMdMetadata
	{
		FileIdentifier = PropertyBtAbstractObject.NEXT_CLASS,
		Language,
		ParentIdentifier,
		Contact,
		DateStamp,
		MetadataStandardName,
		MetadataStandardVersion,
		DataSetUri,
		ReferenceSystemInfo,
		IdentificationInfo,
		DataQualityInfo,
		MetadataConstraints,
		NEXT_CLASS
	}
	
	public static class MetadataMdMetadata
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataMdMetadata ()
		{
			PropInfoList = MetadataBtAbstractObject.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyMdMetadata.FileIdentifier, (int) AimFieldType.SysString, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyMdMetadata.Language, (int) AimFieldType.SysString, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyMdMetadata.ParentIdentifier, (int) AimFieldType.SysString, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyMdMetadata.Contact, (int) ObjectType.CiResponsibleParty, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyMdMetadata.DateStamp, (int) AimFieldType.SysDateTime, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyMdMetadata.MetadataStandardName, (int) AimFieldType.SysString, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyMdMetadata.MetadataStandardVersion, (int) AimFieldType.SysString, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyMdMetadata.DataSetUri, (int) AimFieldType.SysString, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyMdMetadata.ReferenceSystemInfo, (int) ObjectType.RsIdentifier, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyMdMetadata.IdentificationInfo, (int) ObjectType.MdAbstractIdentificationObject, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyMdMetadata.DataQualityInfo, (int) ObjectType.DqDataQuality, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyMdMetadata.MetadataConstraints, (int) ObjectType.MdConstraintsObject, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
		}
	}
}
