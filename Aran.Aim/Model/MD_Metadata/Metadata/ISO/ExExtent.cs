using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Metadata.ISO;

namespace Aran.Aim.Metadata.ISO
{
	public class ExExtent : BtAbstractObject
	{
		public override ObjectType ObjectType
		{
			get { return ObjectType.ExExtent; }
		}
		
		public string Description
		{
			get { return GetFieldValue <string> ((int) PropertyExExtent.Description); }
			set { SetFieldValue <string> ((int) PropertyExExtent.Description, value); }
		}
		
		public List <ExTemporalExtent> TemporalElement
		{
			get { return GetObjectList <ExTemporalExtent> ((int) PropertyExExtent.TemporalElement); }
		}
		
		public List <ExVerticalExtent> VerticalElement
		{
			get { return GetObjectList <ExVerticalExtent> ((int) PropertyExExtent.VerticalElement); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyExExtent
	{
		Description = PropertyBtAbstractObject.NEXT_CLASS,
		TemporalElement,
		VerticalElement,
		NEXT_CLASS
	}
	
	public static class MetadataExExtent
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataExExtent ()
		{
			PropInfoList = MetadataBtAbstractObject.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyExExtent.Description, (int) AimFieldType.SysString, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyExExtent.TemporalElement, (int) ObjectType.ExTemporalExtent, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyExExtent.VerticalElement, (int) ObjectType.ExVerticalExtent, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
		}
	}
}
