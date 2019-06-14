using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Objects;
using Aran.Aim.Enums;

namespace Aran.Aim.Features
{
	public class Oxygen : AObject
	{
		public override ObjectType ObjectType
		{
			get { return ObjectType.Oxygen; }
		}
		
		public CodeOxygen? Type
		{
			get { return GetNullableFieldValue <CodeOxygen> ((int) PropertyOxygen.Type); }
			set { SetNullableFieldValue <CodeOxygen> ((int) PropertyOxygen.Type, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyOxygen
	{
		Type = PropertyAObject.NEXT_CLASS,
		NEXT_CLASS
	}
	
	public static class MetadataOxygen
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataOxygen ()
		{
			PropInfoList = MetadataAObject.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyOxygen.Type, (int) EnumType.CodeOxygen, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyDBEntity.Annotation, (int) ObjectType.Note, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
		}
	}
}
