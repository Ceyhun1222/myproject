using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Objects;
using Aran.Aim.DataTypes;

namespace Aran.Aim.Objects
{
	public class AbstractProcedureRefObject : AObject
	{
		public override ObjectType ObjectType
		{
			get { return ObjectType.AbstractProcedureRefObject; }
		}
		
		public AbstractProcedureRef Feature
		{
			get { return (AbstractProcedureRef ) GetValue ((int) PropertyAbstractProcedureRefObject.Feature); }
			set { SetValue ((int) PropertyAbstractProcedureRefObject.Feature, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyAbstractProcedureRefObject
	{
		Feature = PropertyAObject.NEXT_CLASS,
		NEXT_CLASS
	}
	
	public static class MetadataAbstractProcedureRefObject
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataAbstractProcedureRefObject ()
		{
			PropInfoList = MetadataAObject.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyAbstractProcedureRefObject.Feature, (int) DataType.AbstractProcedureRef);
		}
	}
}
