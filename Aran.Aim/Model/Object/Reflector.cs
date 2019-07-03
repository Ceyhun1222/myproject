using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Objects;
using Aran.Aim.Enums;
using Aran.Aim.Features;

namespace Aran.Aim.Features
{
	public class Reflector : AObject
	{
		public override ObjectType ObjectType
		{
			get { return ObjectType.Reflector; }
		}
		
		public CodeReflector? Type
		{
			get { return GetNullableFieldValue <CodeReflector> ((int) PropertyReflector.Type); }
			set { SetNullableFieldValue <CodeReflector> ((int) PropertyReflector.Type, value); }
		}
		
		public ElevatedPoint TouchdownReflector
		{
			get { return GetObject <ElevatedPoint> ((int) PropertyReflector.TouchdownReflector); }
			set { SetValue ((int) PropertyReflector.TouchdownReflector, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyReflector
	{
		Type = PropertyAObject.NEXT_CLASS,
		TouchdownReflector,
		NEXT_CLASS
	}
	
	public static class MetadataReflector
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataReflector ()
		{
			PropInfoList = MetadataAObject.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyReflector.Type, (int) EnumType.CodeReflector, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyReflector.TouchdownReflector, (int) ObjectType.ElevatedPoint, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyDBEntity.Annotation, (int) ObjectType.Note, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
		}
	}
}
