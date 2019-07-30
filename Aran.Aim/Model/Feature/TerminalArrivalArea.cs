using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Features;
using Aran.Aim.Enums;
using Aran.Aim.DataTypes;

namespace Aran.Aim.Features
{
	public class TerminalArrivalArea : Feature
	{
		public override FeatureType FeatureType
		{
			get { return FeatureType.TerminalArrivalArea; }
		}
		
		public CodeTAA? ArrivalAreaType
		{
			get { return GetNullableFieldValue <CodeTAA> ((int) PropertyTerminalArrivalArea.ArrivalAreaType); }
			set { SetNullableFieldValue <CodeTAA> ((int) PropertyTerminalArrivalArea.ArrivalAreaType, value); }
		}
		
		public ValDistance OuterBufferWidth
		{
			get { return (ValDistance ) GetValue ((int) PropertyTerminalArrivalArea.OuterBufferWidth); }
			set { SetValue ((int) PropertyTerminalArrivalArea.OuterBufferWidth, value); }
		}
		
		public ValDistance LateralBufferWidth
		{
			get { return (ValDistance ) GetValue ((int) PropertyTerminalArrivalArea.LateralBufferWidth); }
			set { SetValue ((int) PropertyTerminalArrivalArea.LateralBufferWidth, value); }
		}
		
		public SignificantPoint IF
		{
			get { return GetObject <SignificantPoint> ((int) PropertyTerminalArrivalArea.IF); }
			set { SetValue ((int) PropertyTerminalArrivalArea.IF, value); }
		}
		
		public SignificantPoint IAF
		{
			get { return GetObject <SignificantPoint> ((int) PropertyTerminalArrivalArea.IAF); }
			set { SetValue ((int) PropertyTerminalArrivalArea.IAF, value); }
		}
		
		public Surface Buffer
		{
			get { return GetObject <Surface> ((int) PropertyTerminalArrivalArea.Buffer); }
			set { SetValue ((int) PropertyTerminalArrivalArea.Buffer, value); }
		}
		
		public List <TerminalArrivalAreaSector> Sector
		{
			get { return GetObjectList <TerminalArrivalAreaSector> ((int) PropertyTerminalArrivalArea.Sector); }
		}
		
		public FeatureRef ApproachRNAV
		{
			get { return (FeatureRef ) GetValue ((int) PropertyTerminalArrivalArea.ApproachRNAV); }
			set { SetValue ((int) PropertyTerminalArrivalArea.ApproachRNAV, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyTerminalArrivalArea
	{
		ArrivalAreaType = PropertyFeature.NEXT_CLASS,
		OuterBufferWidth,
		LateralBufferWidth,
		IF,
		IAF,
		Buffer,
		Sector,
		ApproachRNAV,
		NEXT_CLASS
	}
	
	public static class MetadataTerminalArrivalArea
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataTerminalArrivalArea ()
		{
			PropInfoList = MetadataFeature.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyTerminalArrivalArea.ArrivalAreaType, (int) EnumType.CodeTAA, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyTerminalArrivalArea.OuterBufferWidth, (int) DataType.ValDistance, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyTerminalArrivalArea.LateralBufferWidth, (int) DataType.ValDistance, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyTerminalArrivalArea.IF, (int) ObjectType.SignificantPoint, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyTerminalArrivalArea.IAF, (int) ObjectType.SignificantPoint, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyTerminalArrivalArea.Buffer, (int) ObjectType.Surface, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyTerminalArrivalArea.Sector, (int) ObjectType.TerminalArrivalAreaSector, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyTerminalArrivalArea.ApproachRNAV, (int) FeatureType.InstrumentApproachProcedure, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyDBEntity.Annotation, (int) ObjectType.Note, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyFeature.Metadata, (int) ObjectType.MdMetadata, PropertyTypeCharacter.Nullable);
		}
	}
}
