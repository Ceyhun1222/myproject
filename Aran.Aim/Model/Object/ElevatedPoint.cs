using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Features;
using Aran.Aim.DataTypes;
using Aran.Aim.Enums;

namespace Aran.Aim.Features
{
	public partial class ElevatedPoint : AixmPoint
	{
		public override ObjectType ObjectType
		{
			get { return ObjectType.ElevatedPoint; }
		}
		
		public ValDistanceVertical Elevation
		{
			get { return (ValDistanceVertical ) GetValue ((int) PropertyElevatedPoint.Elevation); }
			set { SetValue ((int) PropertyElevatedPoint.Elevation, value); }
		}
		
		public ValDistanceSigned GeoidUndulation
		{
			get { return (ValDistanceSigned ) GetValue ((int) PropertyElevatedPoint.GeoidUndulation); }
			set { SetValue ((int) PropertyElevatedPoint.GeoidUndulation, value); }
		}
		
		public CodeVerticalDatum? VerticalDatum
		{
			get { return GetNullableFieldValue <CodeVerticalDatum> ((int) PropertyElevatedPoint.VerticalDatum); }
			set { SetNullableFieldValue <CodeVerticalDatum> ((int) PropertyElevatedPoint.VerticalDatum, value); }
		}
		
		public ValDistance VerticalAccuracy
		{
			get { return (ValDistance ) GetValue ((int) PropertyElevatedPoint.VerticalAccuracy); }
			set { SetValue ((int) PropertyElevatedPoint.VerticalAccuracy, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyElevatedPoint
	{
		Elevation = PropertyAixmPoint.NEXT_CLASS,
		GeoidUndulation,
		VerticalDatum,
		VerticalAccuracy,
		NEXT_CLASS
	}
	
	public static class MetadataElevatedPoint
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataElevatedPoint ()
		{
			PropInfoList = MetadataAixmPoint.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyElevatedPoint.Elevation, (int) DataType.ValDistanceVertical, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyElevatedPoint.GeoidUndulation, (int) DataType.ValDistanceSigned, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyElevatedPoint.VerticalDatum, (int) EnumType.CodeVerticalDatum, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyElevatedPoint.VerticalAccuracy, (int) DataType.ValDistance, PropertyTypeCharacter.Nullable);
		}
	}
}
