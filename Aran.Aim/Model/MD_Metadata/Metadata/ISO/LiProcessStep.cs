using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Metadata.ISO;

namespace Aran.Aim.Metadata.ISO
{
	public class LiProcessStep : BtAbstractObject
	{
		public override ObjectType ObjectType
		{
			get { return ObjectType.LiProcessStep; }
		}
		
		public string Description
		{
			get { return GetFieldValue <string> ((int) PropertyLiProcessStep.Description); }
			set { SetFieldValue <string> ((int) PropertyLiProcessStep.Description, value); }
		}
		
		public string Rationale
		{
			get { return GetFieldValue <string> ((int) PropertyLiProcessStep.Rationale); }
			set { SetFieldValue <string> ((int) PropertyLiProcessStep.Rationale, value); }
		}
		
		public DateTime? DateTime
		{
			get { return GetNullableFieldValue <DateTime> ((int) PropertyLiProcessStep.DateTime); }
			set { SetNullableFieldValue <DateTime> ((int) PropertyLiProcessStep.DateTime, value); }
		}
		
		public List <CiResponsibleParty> Processor
		{
			get { return GetObjectList <CiResponsibleParty> ((int) PropertyLiProcessStep.Processor); }
		}
		
		public List <LiSource> Source
		{
			get { return GetObjectList <LiSource> ((int) PropertyLiProcessStep.Source); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyLiProcessStep
	{
		Description = PropertyBtAbstractObject.NEXT_CLASS,
		Rationale,
		DateTime,
		Processor,
		Source,
		NEXT_CLASS
	}
	
	public static class MetadataLiProcessStep
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataLiProcessStep ()
		{
			PropInfoList = MetadataBtAbstractObject.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyLiProcessStep.Description, (int) AimFieldType.SysString, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyLiProcessStep.Rationale, (int) AimFieldType.SysString, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyLiProcessStep.DateTime, (int) AimFieldType.SysDateTime, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyLiProcessStep.Processor, (int) ObjectType.CiResponsibleParty, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyLiProcessStep.Source, (int) ObjectType.LiSource, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
		}
	}
}
