using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Metadata.ISO;

namespace Aran.Aim.Metadata.ISO
{
	public class CiCitation : BtAbstractObject
	{
		public override ObjectType ObjectType
		{
			get { return ObjectType.CiCitation; }
		}
		
		public string Title
		{
			get { return GetFieldValue <string> ((int) PropertyCiCitation.Title); }
			set { SetFieldValue <string> ((int) PropertyCiCitation.Title, value); }
		}
		
		public List <BtString> AlternativeTitle
		{
			get { return GetObjectList <BtString> ((int) PropertyCiCitation.AlternativeTitle); }
		}
		
		public List <BtDateTime> Date
		{
			get { return GetObjectList <BtDateTime> ((int) PropertyCiCitation.Date); }
		}
		
		public string Edition
		{
			get { return GetFieldValue <string> ((int) PropertyCiCitation.Edition); }
			set { SetFieldValue <string> ((int) PropertyCiCitation.Edition, value); }
		}
		
		public DateTime? EditionDate
		{
			get { return GetNullableFieldValue <DateTime> ((int) PropertyCiCitation.EditionDate); }
			set { SetNullableFieldValue <DateTime> ((int) PropertyCiCitation.EditionDate, value); }
		}
		
		public List <MdIdentifier> Identifier
		{
			get { return GetObjectList <MdIdentifier> ((int) PropertyCiCitation.Identifier); }
		}
		
		public List <CiResponsibleParty> CitedResponsibleParty
		{
			get { return GetObjectList <CiResponsibleParty> ((int) PropertyCiCitation.CitedResponsibleParty); }
		}
		
		public List <CiPresentationFormCodeObject> PresentationForm
		{
			get { return GetObjectList <CiPresentationFormCodeObject> ((int) PropertyCiCitation.PresentationForm); }
		}
		
		public CiSeries Series
		{
			get { return GetObject <CiSeries> ((int) PropertyCiCitation.Series); }
			set { SetValue ((int) PropertyCiCitation.Series, value); }
		}
		
		public string OtherCitationDetails
		{
			get { return GetFieldValue <string> ((int) PropertyCiCitation.OtherCitationDetails); }
			set { SetFieldValue <string> ((int) PropertyCiCitation.OtherCitationDetails, value); }
		}
		
		public string CollectiveTitle
		{
			get { return GetFieldValue <string> ((int) PropertyCiCitation.CollectiveTitle); }
			set { SetFieldValue <string> ((int) PropertyCiCitation.CollectiveTitle, value); }
		}
		
		public string ISBN
		{
			get { return GetFieldValue <string> ((int) PropertyCiCitation.ISBN); }
			set { SetFieldValue <string> ((int) PropertyCiCitation.ISBN, value); }
		}
		
		public string ISSN
		{
			get { return GetFieldValue <string> ((int) PropertyCiCitation.ISSN); }
			set { SetFieldValue <string> ((int) PropertyCiCitation.ISSN, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyCiCitation
	{
		Title = PropertyBtAbstractObject.NEXT_CLASS,
		AlternativeTitle,
		Date,
		Edition,
		EditionDate,
		Identifier,
		CitedResponsibleParty,
		PresentationForm,
		Series,
		OtherCitationDetails,
		CollectiveTitle,
		ISBN,
		ISSN,
		NEXT_CLASS
	}
	
	public static class MetadataCiCitation
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataCiCitation ()
		{
			PropInfoList = MetadataBtAbstractObject.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyCiCitation.Title, (int) AimFieldType.SysString, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyCiCitation.AlternativeTitle, (int) ObjectType.BtString, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyCiCitation.Date, (int) ObjectType.BtDateTime, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyCiCitation.Edition, (int) AimFieldType.SysString, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyCiCitation.EditionDate, (int) AimFieldType.SysDateTime, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyCiCitation.Identifier, (int) ObjectType.MdIdentifier, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyCiCitation.CitedResponsibleParty, (int) ObjectType.CiResponsibleParty, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyCiCitation.PresentationForm, (int) ObjectType.CiPresentationFormCodeObject, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyCiCitation.Series, (int) ObjectType.CiSeries, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyCiCitation.OtherCitationDetails, (int) AimFieldType.SysString, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyCiCitation.CollectiveTitle, (int) AimFieldType.SysString, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyCiCitation.ISBN, (int) AimFieldType.SysString, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyCiCitation.ISSN, (int) AimFieldType.SysString, PropertyTypeCharacter.Nullable);
		}
	}
}
