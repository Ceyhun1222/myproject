using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Objects;
using Aran.Aim.Enums;

namespace Aran.Aim.Features
{
	public class Fuel : AObject
	{
		public override ObjectType ObjectType
		{
			get { return ObjectType.Fuel; }
		}
		
		public CodeFuel? Category
		{
			get { return GetNullableFieldValue <CodeFuel> ((int) PropertyFuel.Category); }
			set { SetNullableFieldValue <CodeFuel> ((int) PropertyFuel.Category, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyFuel
	{
		Category = PropertyAObject.NEXT_CLASS,
		NEXT_CLASS
	}
	
	public static class MetadataFuel
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataFuel ()
		{
			PropInfoList = MetadataAObject.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyFuel.Category, (int) EnumType.CodeFuel, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyDBEntity.Annotation, (int) ObjectType.Note, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
		}
	}
}
