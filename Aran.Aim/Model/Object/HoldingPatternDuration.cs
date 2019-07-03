using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Objects;
using Aran.Aim.DataTypes;

namespace Aran.Aim.Features
{
	public class HoldingPatternDuration : AObject
	{
		public override ObjectType ObjectType
		{
			get { return ObjectType.HoldingPatternDuration; }
		}
		
		public ValDuration Duration
		{
			get { return (ValDuration ) GetValue ((int) PropertyHoldingPatternDuration.Duration); }
			set { SetValue ((int) PropertyHoldingPatternDuration.Duration, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyHoldingPatternDuration
	{
		Duration = PropertyAObject.NEXT_CLASS,
		NEXT_CLASS
	}
	
	public static class MetadataHoldingPatternDuration
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataHoldingPatternDuration ()
		{
			PropInfoList = MetadataAObject.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyHoldingPatternDuration.Duration, (int) DataType.ValDuration, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyDBEntity.Annotation, (int) ObjectType.Note, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
		}
	}
}
