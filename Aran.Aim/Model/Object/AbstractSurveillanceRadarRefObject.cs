using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Objects;
using Aran.Aim.DataTypes;

namespace Aran.Aim.Objects
{
	public class AbstractSurveillanceRadarRefObject : AObject
	{
		public override ObjectType ObjectType
		{
			get { return ObjectType.AbstractSurveillanceRadarRefObject; }
		}
		
		public AbstractSurveillanceRadarRef Feature
		{
			get { return (AbstractSurveillanceRadarRef ) GetValue ((int) PropertyAbstractSurveillanceRadarRefObject.Feature); }
			set { SetValue ((int) PropertyAbstractSurveillanceRadarRefObject.Feature, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyAbstractSurveillanceRadarRefObject
	{
		Feature = PropertyAObject.NEXT_CLASS,
		NEXT_CLASS
	}
	
	public static class MetadataAbstractSurveillanceRadarRefObject
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataAbstractSurveillanceRadarRefObject ()
		{
			PropInfoList = MetadataAObject.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyAbstractSurveillanceRadarRefObject.Feature, (int) DataType.AbstractSurveillanceRadarRef);
		}
	}
}
