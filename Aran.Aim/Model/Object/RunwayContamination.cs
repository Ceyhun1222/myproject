using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Features;
using Aran.Aim.DataTypes;
using Aran.Aim.Enums;

namespace Aran.Aim.Features
{
	public class RunwayContamination : SurfaceContamination
	{
		public override ObjectType ObjectType
		{
			get { return ObjectType.RunwayContamination; }
		}
		
		public ValDistance ClearedLength
		{
			get { return (ValDistance ) GetValue ((int) PropertyRunwayContamination.ClearedLength); }
			set { SetValue ((int) PropertyRunwayContamination.ClearedLength, value); }
		}
		
		public ValDistance ClearedWidth
		{
			get { return (ValDistance ) GetValue ((int) PropertyRunwayContamination.ClearedWidth); }
			set { SetValue ((int) PropertyRunwayContamination.ClearedWidth, value); }
		}
		
		public CodeSide? ClearedSide
		{
			get { return GetNullableFieldValue <CodeSide> ((int) PropertyRunwayContamination.ClearedSide); }
			set { SetNullableFieldValue <CodeSide> ((int) PropertyRunwayContamination.ClearedSide, value); }
		}
		
		public ValDistance FurtherClearanceLength
		{
			get { return (ValDistance ) GetValue ((int) PropertyRunwayContamination.FurtherClearanceLength); }
			set { SetValue ((int) PropertyRunwayContamination.FurtherClearanceLength, value); }
		}
		
		public ValDistance FurtherClearanceWidth
		{
			get { return (ValDistance ) GetValue ((int) PropertyRunwayContamination.FurtherClearanceWidth); }
			set { SetValue ((int) PropertyRunwayContamination.FurtherClearanceWidth, value); }
		}
		
		public CodeSide? ObscuredLightsSide
		{
			get { return GetNullableFieldValue <CodeSide> ((int) PropertyRunwayContamination.ObscuredLightsSide); }
			set { SetNullableFieldValue <CodeSide> ((int) PropertyRunwayContamination.ObscuredLightsSide, value); }
		}
		
		public ValDistance ClearedLengthBegin
		{
			get { return (ValDistance ) GetValue ((int) PropertyRunwayContamination.ClearedLengthBegin); }
			set { SetValue ((int) PropertyRunwayContamination.ClearedLengthBegin, value); }
		}
		
		public bool? TaxiwayAvailable
		{
			get { return GetNullableFieldValue <bool> ((int) PropertyRunwayContamination.TaxiwayAvailable); }
			set { SetNullableFieldValue <bool> ((int) PropertyRunwayContamination.TaxiwayAvailable, value); }
		}
		
		public bool? ApronAvailable
		{
			get { return GetNullableFieldValue <bool> ((int) PropertyRunwayContamination.ApronAvailable); }
			set { SetNullableFieldValue <bool> ((int) PropertyRunwayContamination.ApronAvailable, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyRunwayContamination
	{
		ClearedLength = PropertySurfaceContamination.NEXT_CLASS,
		ClearedWidth,
		ClearedSide,
		FurtherClearanceLength,
		FurtherClearanceWidth,
		ObscuredLightsSide,
		ClearedLengthBegin,
		TaxiwayAvailable,
		ApronAvailable,
		NEXT_CLASS
	}
	
	public static class MetadataRunwayContamination
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataRunwayContamination ()
		{
			PropInfoList = MetadataSurfaceContamination.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyRunwayContamination.ClearedLength, (int) DataType.ValDistance, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyRunwayContamination.ClearedWidth, (int) DataType.ValDistance, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyRunwayContamination.ClearedSide, (int) EnumType.CodeSide, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyRunwayContamination.FurtherClearanceLength, (int) DataType.ValDistance, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyRunwayContamination.FurtherClearanceWidth, (int) DataType.ValDistance, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyRunwayContamination.ObscuredLightsSide, (int) EnumType.CodeSide, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyRunwayContamination.ClearedLengthBegin, (int) DataType.ValDistance, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyRunwayContamination.TaxiwayAvailable, (int) AimFieldType.SysBool, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyRunwayContamination.ApronAvailable, (int) AimFieldType.SysBool, PropertyTypeCharacter.Nullable);
		}
	}
}
