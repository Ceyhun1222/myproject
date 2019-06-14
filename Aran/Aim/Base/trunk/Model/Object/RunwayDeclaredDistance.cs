using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Objects;
using Aran.Aim.Enums;
using Aran.Aim.Features;

namespace Aran.Aim.Features
{
	public class RunwayDeclaredDistance : AObject
	{
		public override ObjectType ObjectType
		{
			get { return ObjectType.RunwayDeclaredDistance; }
		}
		
		public CodeDeclaredDistance? Type
		{
			get { return GetNullableFieldValue <CodeDeclaredDistance> ((int) PropertyRunwayDeclaredDistance.Type); }
			set { SetNullableFieldValue <CodeDeclaredDistance> ((int) PropertyRunwayDeclaredDistance.Type, value); }
		}
		
		public List <RunwayDeclaredDistanceValue> DeclaredValue
		{
			get { return GetObjectList <RunwayDeclaredDistanceValue> ((int) PropertyRunwayDeclaredDistance.DeclaredValue); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyRunwayDeclaredDistance
	{
		Type = PropertyAObject.NEXT_CLASS,
		DeclaredValue,
		NEXT_CLASS
	}
	
	public static class MetadataRunwayDeclaredDistance
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataRunwayDeclaredDistance ()
		{
			PropInfoList = MetadataAObject.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyRunwayDeclaredDistance.Type, (int) EnumType.CodeDeclaredDistance, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyRunwayDeclaredDistance.DeclaredValue, (int) ObjectType.RunwayDeclaredDistanceValue, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyDBEntity.Annotation, (int) ObjectType.Note, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
		}
	}
}
