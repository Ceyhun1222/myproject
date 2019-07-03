using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Objects;
using Aran.Aim.DataTypes;

namespace Aran.Aim.Features
{
	public class HoldingPatternDistance : AObject
	{
		public override ObjectType ObjectType
		{
			get { return ObjectType.HoldingPatternDistance; }
		}
		
		public ValDistance Length
		{
			get { return (ValDistance ) GetValue ((int) PropertyHoldingPatternDistance.Length); }
			set { SetValue ((int) PropertyHoldingPatternDistance.Length, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyHoldingPatternDistance
	{
		Length = PropertyAObject.NEXT_CLASS,
		NEXT_CLASS
	}
	
	public static class MetadataHoldingPatternDistance
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataHoldingPatternDistance ()
		{
			PropInfoList = MetadataAObject.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyHoldingPatternDistance.Length, (int) DataType.ValDistance, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyDBEntity.Annotation, (int) ObjectType.Note, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
		}
	}
}
