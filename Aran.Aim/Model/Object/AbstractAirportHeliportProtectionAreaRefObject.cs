using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Objects;
using Aran.Aim.DataTypes;

namespace Aran.Aim.Objects
{
	public class AbstractAirportHeliportProtectionAreaRefObject : AObject
	{
		public override ObjectType ObjectType
		{
			get { return ObjectType.AbstractAirportHeliportProtectionAreaRefObject; }
		}
		
		public AbstractAirportHeliportProtectionAreaRef Feature
		{
			get { return (AbstractAirportHeliportProtectionAreaRef ) GetValue ((int) PropertyAbstractAirportHeliportProtectionAreaRefObject.Feature); }
			set { SetValue ((int) PropertyAbstractAirportHeliportProtectionAreaRefObject.Feature, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyAbstractAirportHeliportProtectionAreaRefObject
	{
		Feature = PropertyAObject.NEXT_CLASS,
		NEXT_CLASS
	}
	
	public static class MetadataAbstractAirportHeliportProtectionAreaRefObject
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataAbstractAirportHeliportProtectionAreaRefObject ()
		{
			PropInfoList = MetadataAObject.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyAbstractAirportHeliportProtectionAreaRefObject.Feature, (int) DataType.AbstractAirportHeliportProtectionAreaRef);
		}
	}
}
