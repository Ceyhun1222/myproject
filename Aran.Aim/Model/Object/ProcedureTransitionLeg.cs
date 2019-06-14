using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Objects;
using Aran.Aim.DataTypes;

namespace Aran.Aim.Features
{
	public class ProcedureTransitionLeg : AObject
	{
		public override ObjectType ObjectType
		{
			get { return ObjectType.ProcedureTransitionLeg; }
		}
		
		public uint? SeqNumberARINC
		{
			get { return GetNullableFieldValue <uint> ((int) PropertyProcedureTransitionLeg.SeqNumberARINC); }
			set { SetNullableFieldValue <uint> ((int) PropertyProcedureTransitionLeg.SeqNumberARINC, value); }
		}
		
		public AbstractSegmentLegRef TheSegmentLeg
		{
			get { return (AbstractSegmentLegRef ) GetValue ((int) PropertyProcedureTransitionLeg.TheSegmentLeg); }
			set { SetValue ((int) PropertyProcedureTransitionLeg.TheSegmentLeg, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyProcedureTransitionLeg
	{
		SeqNumberARINC = PropertyAObject.NEXT_CLASS,
		TheSegmentLeg,
		NEXT_CLASS
	}
	
	public static class MetadataProcedureTransitionLeg
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataProcedureTransitionLeg ()
		{
			PropInfoList = MetadataAObject.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyProcedureTransitionLeg.SeqNumberARINC, (int) AimFieldType.SysUInt32, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyDBEntity.Annotation, (int) ObjectType.Note, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyProcedureTransitionLeg.TheSegmentLeg, (int) DataType.AbstractSegmentLegRef, PropertyTypeCharacter.Nullable);
		}
	}
}
