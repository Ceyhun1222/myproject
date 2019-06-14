using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Objects;
using Aran.Aim.DataTypes;

namespace Aran.Aim.Features
{
	public class StandardLevel : AObject
	{
		public override ObjectType ObjectType
		{
			get { return ObjectType.StandardLevel; }
		}
		
		public ValDistanceVertical VerticalDistance
		{
			get { return (ValDistanceVertical ) GetValue ((int) PropertyStandardLevel.VerticalDistance); }
			set { SetValue ((int) PropertyStandardLevel.VerticalDistance, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyStandardLevel
	{
		VerticalDistance = PropertyAObject.NEXT_CLASS,
		NEXT_CLASS
	}
	
	public static class MetadataStandardLevel
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataStandardLevel ()
		{
			PropInfoList = MetadataAObject.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyStandardLevel.VerticalDistance, (int) DataType.ValDistanceVertical, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyDBEntity.Annotation, (int) ObjectType.Note, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
		}
	}
}
