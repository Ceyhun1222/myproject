using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Objects;
using Aran.Aim.Features;

namespace Aran.Aim.Features
{
	public abstract class PropertiesWithSchedule : AObject
	{
		public virtual PropertiesWithScheduleType PropertiesWithScheduleType 
		{
			get { return (PropertiesWithScheduleType) ObjectType; }
		}
		
		public List <Timesheet> TimeInterval
		{
			get { return GetObjectList <Timesheet> ((int) PropertyPropertiesWithSchedule.TimeInterval); }
		}
		
		public List <FeatureRefObject> SpecialDateAuthority
		{
			get { return GetObjectList <FeatureRefObject> ((int) PropertyPropertiesWithSchedule.SpecialDateAuthority); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyPropertiesWithSchedule
	{
		TimeInterval = PropertyAObject.NEXT_CLASS,
		SpecialDateAuthority,
		NEXT_CLASS
	}
	
	public static class MetadataPropertiesWithSchedule
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataPropertiesWithSchedule ()
		{
			PropInfoList = MetadataAObject.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyPropertiesWithSchedule.TimeInterval, (int) ObjectType.Timesheet, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyDBEntity.Annotation, (int) ObjectType.Note, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyPropertiesWithSchedule.SpecialDateAuthority, (int) FeatureType.OrganisationAuthority, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
		}
	}
}
