using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Metadata.ISO;

namespace Aran.Aim.Metadata.ISO
{
	public class CiSeries : BtAbstractObject
	{
		public override ObjectType ObjectType
		{
			get { return ObjectType.CiSeries; }
		}
		
		public string Name
		{
			get { return GetFieldValue <string> ((int) PropertyCiSeries.Name); }
			set { SetFieldValue <string> ((int) PropertyCiSeries.Name, value); }
		}
		
		public string IssueIdentification
		{
			get { return GetFieldValue <string> ((int) PropertyCiSeries.IssueIdentification); }
			set { SetFieldValue <string> ((int) PropertyCiSeries.IssueIdentification, value); }
		}
		
		public string Page
		{
			get { return GetFieldValue <string> ((int) PropertyCiSeries.Page); }
			set { SetFieldValue <string> ((int) PropertyCiSeries.Page, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyCiSeries
	{
		Name = PropertyBtAbstractObject.NEXT_CLASS,
		IssueIdentification,
		Page,
		NEXT_CLASS
	}
	
	public static class MetadataCiSeries
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataCiSeries ()
		{
			PropInfoList = MetadataBtAbstractObject.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyCiSeries.Name, (int) AimFieldType.SysString, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyCiSeries.IssueIdentification, (int) AimFieldType.SysString, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyCiSeries.Page, (int) AimFieldType.SysString, PropertyTypeCharacter.Nullable);
		}
	}
}
