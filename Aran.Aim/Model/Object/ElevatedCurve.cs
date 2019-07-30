using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Features;
using Aran.Aim.DataTypes;
using Aran.Aim.Enums;

namespace Aran.Aim.Features
{
	public partial class ElevatedCurve : Curve
	{
		public override ObjectType ObjectType
		{
			get { return ObjectType.ElevatedCurve; }
		}
		
		public ValDistanceVertical Elevation
		{
			get { return (ValDistanceVertical ) GetValue ((int) PropertyElevatedCurve.Elevation); }
			set { SetValue ((int) PropertyElevatedCurve.Elevation, value); }
		}
		
		public ValDistanceSigned GeoidUndulation
		{
			get { return (ValDistanceSigned ) GetValue ((int) PropertyElevatedCurve.GeoidUndulation); }
			set { SetValue ((int) PropertyElevatedCurve.GeoidUndulation, value); }
		}
		
		public CodeVerticalDatum? VerticalDatum
		{
			get { return GetNullableFieldValue <CodeVerticalDatum> ((int) PropertyElevatedCurve.VerticalDatum); }
			set { SetNullableFieldValue <CodeVerticalDatum> ((int) PropertyElevatedCurve.VerticalDatum, value); }
		}
		
		public ValDistance VerticalAccuracy
		{
			get { return (ValDistance ) GetValue ((int) PropertyElevatedCurve.VerticalAccuracy); }
			set { SetValue ((int) PropertyElevatedCurve.VerticalAccuracy, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyElevatedCurve
	{
		Elevation = PropertyCurve.NEXT_CLASS,
		GeoidUndulation,
		VerticalDatum,
		VerticalAccuracy,
		NEXT_CLASS
	}
	
	public static class MetadataElevatedCurve
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataElevatedCurve ()
		{
			PropInfoList = MetadataCurve.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyElevatedCurve.Elevation, (int) DataType.ValDistanceVertical, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyElevatedCurve.GeoidUndulation, (int) DataType.ValDistanceSigned, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyElevatedCurve.VerticalDatum, (int) EnumType.CodeVerticalDatum, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyElevatedCurve.VerticalAccuracy, (int) DataType.ValDistance, PropertyTypeCharacter.Nullable);
		}
	}
}
