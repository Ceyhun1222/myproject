using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Objects;
using Aran.Aim.Enums;
using Aran.Aim.DataTypes;

namespace Aran.Aim.Features
{
	public class Ridge : AObject
	{
		public override ObjectType ObjectType
		{
			get { return ObjectType.Ridge; }
		}
		
		public CodeSide? Side
		{
			get { return GetNullableFieldValue <CodeSide> ((int) PropertyRidge.Side); }
			set { SetNullableFieldValue <CodeSide> ((int) PropertyRidge.Side, value); }
		}
		
		public ValDistance Distance
		{
			get { return (ValDistance ) GetValue ((int) PropertyRidge.Distance); }
			set { SetValue ((int) PropertyRidge.Distance, value); }
		}
		
		public ValDepth Depth
		{
			get { return (ValDepth ) GetValue ((int) PropertyRidge.Depth); }
			set { SetValue ((int) PropertyRidge.Depth, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyRidge
	{
		Side = PropertyAObject.NEXT_CLASS,
		Distance,
		Depth,
		NEXT_CLASS
	}
	
	public static class MetadataRidge
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataRidge ()
		{
			PropInfoList = MetadataAObject.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyRidge.Side, (int) EnumType.CodeSide, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyRidge.Distance, (int) DataType.ValDistance, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyRidge.Depth, (int) DataType.ValDepth, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyDBEntity.Annotation, (int) ObjectType.Note, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
		}
	}
}
