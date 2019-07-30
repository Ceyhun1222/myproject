using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Features;
using Aran.Aim.DataTypes;

namespace Aran.Aim.Features
{
	public class RunwayDeclaredDistanceValue : PropertiesWithSchedule
	{
		public override ObjectType ObjectType
		{
			get { return ObjectType.RunwayDeclaredDistanceValue; }
		}
		
		public ValDistance Distance
		{
			get { return (ValDistance ) GetValue ((int) PropertyRunwayDeclaredDistanceValue.Distance); }
			set { SetValue ((int) PropertyRunwayDeclaredDistanceValue.Distance, value); }
		}
		
		public ValDistance DistanceAccuracy
		{
			get { return (ValDistance ) GetValue ((int) PropertyRunwayDeclaredDistanceValue.DistanceAccuracy); }
			set { SetValue ((int) PropertyRunwayDeclaredDistanceValue.DistanceAccuracy, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyRunwayDeclaredDistanceValue
	{
		Distance = PropertyPropertiesWithSchedule.NEXT_CLASS,
		DistanceAccuracy,
		NEXT_CLASS
	}
	
	public static class MetadataRunwayDeclaredDistanceValue
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataRunwayDeclaredDistanceValue ()
		{
			PropInfoList = MetadataPropertiesWithSchedule.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyRunwayDeclaredDistanceValue.Distance, (int) DataType.ValDistance, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyRunwayDeclaredDistanceValue.DistanceAccuracy, (int) DataType.ValDistance, PropertyTypeCharacter.Nullable);
		}
	}
}
