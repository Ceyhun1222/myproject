using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.DataTypes;

namespace Aran.Aim.DataTypes
{
	public class AbstractProcedureRef : AbstractFeatureRef <ProcedureType>
	{
		public AbstractProcedureRef ()
		{
		}
		
		public AbstractProcedureRef (ProcedureRefType procedureRef, FeatureRef feature)
		 : base (procedureRef, feature)
		{
		}
		
		public override DataType DataType
		{
			get { return DataType.AbstractProcedureRef; }
		}
		
		public int Type
		{
			get { return GetFieldValue <int> ((int) PropertyAbstractProcedureRef.Type); }
			set { SetFieldValue <int> ((int) PropertyAbstractProcedureRef.Type, value); }
		}
		
		public FeatureRef Feature
		{
			get { return (FeatureRef ) GetValue ((int) PropertyAbstractProcedureRef.Feature); }
			set { SetValue ((int) PropertyAbstractProcedureRef.Feature, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyAbstractProcedureRef
	{
		Type,
		Feature,
		NEXT_CLASS
	}
	
	public static class MetadataAbstractProcedureRef
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataAbstractProcedureRef ()
		{
			PropInfoList = MetadataAimObject.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyAbstractProcedureRef.Type, (int) AimFieldType.Sysint);
			PropInfoList.Add (PropertyAbstractProcedureRef.Feature, (int) DataType.FeatureRef);
		}
	}
}
