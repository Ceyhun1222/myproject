using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Objects;
using Aran.Aim.Enums;

namespace Aran.Aim.Features
{
	public class Oil : AObject
	{
		public override ObjectType ObjectType
		{
			get { return ObjectType.Oil; }
		}
		
		public CodeOil? Category
		{
			get { return GetNullableFieldValue <CodeOil> ((int) PropertyOil.Category); }
			set { SetNullableFieldValue <CodeOil> ((int) PropertyOil.Category, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyOil
	{
		Category = PropertyAObject.NEXT_CLASS,
		NEXT_CLASS
	}
	
	public static class MetadataOil
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataOil ()
		{
			PropInfoList = MetadataAObject.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyOil.Category, (int) EnumType.CodeOil, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyDBEntity.Annotation, (int) ObjectType.Note, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
		}
	}
}
