using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Features;
using Aran.Aim.Enums;
using Aran.Aim.DataTypes;

namespace Aran.Aim.Features
{
	public class TerminalSegmentPoint : SegmentPoint
	{
		public override ObjectType ObjectType
		{
			get { return ObjectType.TerminalSegmentPoint; }
		}
		
		public CodeProcedureFixRole? Role
		{
			get { return GetNullableFieldValue <CodeProcedureFixRole> ((int) PropertyTerminalSegmentPoint.Role); }
			set { SetNullableFieldValue <CodeProcedureFixRole> ((int) PropertyTerminalSegmentPoint.Role, value); }
		}
		
		public double? LeadRadial
		{
			get { return GetNullableFieldValue <double> ((int) PropertyTerminalSegmentPoint.LeadRadial); }
			set { SetNullableFieldValue <double> ((int) PropertyTerminalSegmentPoint.LeadRadial, value); }
		}
		
		public ValDistance LeadDME
		{
			get { return (ValDistance ) GetValue ((int) PropertyTerminalSegmentPoint.LeadDME); }
			set { SetValue ((int) PropertyTerminalSegmentPoint.LeadDME, value); }
		}
		
		public bool? IndicatorFACF
		{
			get { return GetNullableFieldValue <bool> ((int) PropertyTerminalSegmentPoint.IndicatorFACF); }
			set { SetNullableFieldValue <bool> ((int) PropertyTerminalSegmentPoint.IndicatorFACF, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyTerminalSegmentPoint
	{
		Role = PropertySegmentPoint.NEXT_CLASS,
		LeadRadial,
		LeadDME,
		IndicatorFACF,
		NEXT_CLASS
	}
	
	public static class MetadataTerminalSegmentPoint
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataTerminalSegmentPoint ()
		{
			PropInfoList = MetadataSegmentPoint.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyTerminalSegmentPoint.Role, (int) EnumType.CodeProcedureFixRole, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyTerminalSegmentPoint.LeadRadial, (int) AimFieldType.SysDouble, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyTerminalSegmentPoint.LeadDME, (int) DataType.ValDistance, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyTerminalSegmentPoint.IndicatorFACF, (int) AimFieldType.SysBool, PropertyTypeCharacter.Nullable);
		}
	}
}
