using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Objects;

namespace Aran.Aim.Features
{
	public class City : AObject
	{
		public override ObjectType ObjectType
		{
			get { return ObjectType.City; }
		}
		
		public string Name
		{
			get { return GetFieldValue <string> ((int) PropertyCity.Name); }
			set { SetFieldValue <string> ((int) PropertyCity.Name, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyCity
	{
		Name = PropertyAObject.NEXT_CLASS,
		NEXT_CLASS
	}
	
	public static class MetadataCity
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataCity ()
		{
			PropInfoList = MetadataAObject.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyCity.Name, (int) AimFieldType.SysString, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyDBEntity.Annotation, (int) ObjectType.Note, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
		}
	}
}
