using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Objects;
using Aran.Aim.Enums;

namespace Aran.Aim.Features
{
	public class Nitrogen : AObject
	{
		public override ObjectType ObjectType
		{
			get { return ObjectType.Nitrogen; }
		}
		
		public CodeNitrogen? Type
		{
			get { return GetNullableFieldValue <CodeNitrogen> ((int) PropertyNitrogen.Type); }
			set { SetNullableFieldValue <CodeNitrogen> ((int) PropertyNitrogen.Type, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyNitrogen
	{
		Type = PropertyAObject.NEXT_CLASS,
		NEXT_CLASS
	}
	
	public static class MetadataNitrogen
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataNitrogen ()
		{
			PropInfoList = MetadataAObject.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyNitrogen.Type, (int) EnumType.CodeNitrogen, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyDBEntity.Annotation, (int) ObjectType.Note, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
		}
	}
}
