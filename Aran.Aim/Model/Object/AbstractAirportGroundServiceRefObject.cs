using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Objects;
using Aran.Aim.DataTypes;

namespace Aran.Aim.Objects
{
	public class AbstractAirportGroundServiceRefObject : AObject
	{
		public override ObjectType ObjectType
		{
			get { return ObjectType.AbstractAirportGroundServiceRefObject; }
		}
		
		public AbstractAirportGroundServiceRef Feature
		{
			get { return (AbstractAirportGroundServiceRef ) GetValue ((int) PropertyAbstractAirportGroundServiceRefObject.Feature); }
			set { SetValue ((int) PropertyAbstractAirportGroundServiceRefObject.Feature, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyAbstractAirportGroundServiceRefObject
	{
		Feature = PropertyAObject.NEXT_CLASS,
		NEXT_CLASS
	}
	
	public static class MetadataAbstractAirportGroundServiceRefObject
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataAbstractAirportGroundServiceRefObject ()
		{
			PropInfoList = MetadataAObject.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyAbstractAirportGroundServiceRefObject.Feature, (int) DataType.AbstractAirportGroundServiceRef);
		}
	}
}
