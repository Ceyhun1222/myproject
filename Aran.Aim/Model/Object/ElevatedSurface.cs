using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Features;
using Aran.Aim.DataTypes;
using Aran.Aim.Enums;

namespace Aran.Aim.Features
{
	public partial class ElevatedSurface : Surface
	{
		public override ObjectType ObjectType
		{
			get { return ObjectType.ElevatedSurface; }
		}
		
		public ValDistanceVertical Elevation
		{
			get { return (ValDistanceVertical ) GetValue ((int) PropertyElevatedSurface.Elevation); }
			set { SetValue ((int) PropertyElevatedSurface.Elevation, value); }
		}
		
		public ValDistanceSigned GeoidUndulation
		{
			get { return (ValDistanceSigned ) GetValue ((int) PropertyElevatedSurface.GeoidUndulation); }
			set { SetValue ((int) PropertyElevatedSurface.GeoidUndulation, value); }
		}
		
		public CodeVerticalDatum? VerticalDatum
		{
			get { return GetNullableFieldValue <CodeVerticalDatum> ((int) PropertyElevatedSurface.VerticalDatum); }
			set { SetNullableFieldValue <CodeVerticalDatum> ((int) PropertyElevatedSurface.VerticalDatum, value); }
		}
		
		public ValDistance VerticalAccuracy
		{
			get { return (ValDistance ) GetValue ((int) PropertyElevatedSurface.VerticalAccuracy); }
			set { SetValue ((int) PropertyElevatedSurface.VerticalAccuracy, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyElevatedSurface
	{
		Elevation = PropertySurface.NEXT_CLASS,
		GeoidUndulation,
		VerticalDatum,
		VerticalAccuracy,
		NEXT_CLASS
	}
	
	public static class MetadataElevatedSurface
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataElevatedSurface ()
		{
			PropInfoList = MetadataSurface.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyElevatedSurface.Elevation, (int) DataType.ValDistanceVertical, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyElevatedSurface.GeoidUndulation, (int) DataType.ValDistanceSigned, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyElevatedSurface.VerticalDatum, (int) EnumType.CodeVerticalDatum, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyElevatedSurface.VerticalAccuracy, (int) DataType.ValDistance, PropertyTypeCharacter.Nullable);
		}
	}
}
