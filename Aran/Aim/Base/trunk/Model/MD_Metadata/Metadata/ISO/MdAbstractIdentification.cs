using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Metadata.ISO;

namespace Aran.Aim.Metadata.ISO
{
	public abstract class MdAbstractIdentification : BtAbstractObject
	{
		public virtual MdAbstractIdentificationType MdAbstractIdentificationType 
		{
			get { return (MdAbstractIdentificationType) ObjectType; }
		}
		
		public CiCitation Citation
		{
			get { return GetObject <CiCitation> ((int) PropertyMdAbstractIdentification.Citation); }
			set { SetValue ((int) PropertyMdAbstractIdentification.Citation, value); }
		}
		
		public string Abstract
		{
			get { return GetFieldValue <string> ((int) PropertyMdAbstractIdentification.Abstract); }
			set { SetFieldValue <string> ((int) PropertyMdAbstractIdentification.Abstract, value); }
		}
		
		public string Purpose
		{
			get { return GetFieldValue <string> ((int) PropertyMdAbstractIdentification.Purpose); }
			set { SetFieldValue <string> ((int) PropertyMdAbstractIdentification.Purpose, value); }
		}
		
		public List <BtString> Credit
		{
			get { return GetObjectList <BtString> ((int) PropertyMdAbstractIdentification.Credit); }
		}
		
		public List <MdProgressCodeObject> Status
		{
			get { return GetObjectList <MdProgressCodeObject> ((int) PropertyMdAbstractIdentification.Status); }
		}
		
		public List <CiResponsibleParty> PointOfContact
		{
			get { return GetObjectList <CiResponsibleParty> ((int) PropertyMdAbstractIdentification.PointOfContact); }
		}
		
		public List <MdConstraintsObject> ResourceConstraints
		{
			get { return GetObjectList <MdConstraintsObject> ((int) PropertyMdAbstractIdentification.ResourceConstraints); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyMdAbstractIdentification
	{
		Citation = PropertyBtAbstractObject.NEXT_CLASS,
		Abstract,
		Purpose,
		Credit,
		Status,
		PointOfContact,
		ResourceConstraints,
		NEXT_CLASS
	}
	
	public static class MetadataMdAbstractIdentification
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataMdAbstractIdentification ()
		{
			PropInfoList = MetadataBtAbstractObject.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyMdAbstractIdentification.Citation, (int) ObjectType.CiCitation, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyMdAbstractIdentification.Abstract, (int) AimFieldType.SysString, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyMdAbstractIdentification.Purpose, (int) AimFieldType.SysString, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyMdAbstractIdentification.Credit, (int) ObjectType.BtString, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyMdAbstractIdentification.Status, (int) ObjectType.MdProgressCodeObject, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyMdAbstractIdentification.PointOfContact, (int) ObjectType.CiResponsibleParty, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyMdAbstractIdentification.ResourceConstraints, (int) ObjectType.MdConstraintsObject, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
		}
	}
}
