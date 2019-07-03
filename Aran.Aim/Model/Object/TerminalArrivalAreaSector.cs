using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Objects;
using Aran.Aim.Enums;
using Aran.Aim.Features;

namespace Aran.Aim.Features
{
	public class TerminalArrivalAreaSector : AObject
	{
		public override ObjectType ObjectType
		{
			get { return ObjectType.TerminalArrivalAreaSector; }
		}
		
		public bool? FlyByCode
		{
			get { return GetNullableFieldValue <bool> ((int) PropertyTerminalArrivalAreaSector.FlyByCode); }
			set { SetNullableFieldValue <bool> ((int) PropertyTerminalArrivalAreaSector.FlyByCode, value); }
		}
		
		public bool? ProcedureTurnRequired
		{
			get { return GetNullableFieldValue <bool> ((int) PropertyTerminalArrivalAreaSector.ProcedureTurnRequired); }
			set { SetNullableFieldValue <bool> ((int) PropertyTerminalArrivalAreaSector.ProcedureTurnRequired, value); }
		}
		
		public CodeAltitudeUse? AltitudeDescription
		{
			get { return GetNullableFieldValue <CodeAltitudeUse> ((int) PropertyTerminalArrivalAreaSector.AltitudeDescription); }
			set { SetNullableFieldValue <CodeAltitudeUse> ((int) PropertyTerminalArrivalAreaSector.AltitudeDescription, value); }
		}
		
		public CircleSector SectorDefinition
		{
			get { return GetObject <CircleSector> ((int) PropertyTerminalArrivalAreaSector.SectorDefinition); }
			set { SetValue ((int) PropertyTerminalArrivalAreaSector.SectorDefinition, value); }
		}
		
		public Surface Extent
		{
			get { return GetObject <Surface> ((int) PropertyTerminalArrivalAreaSector.Extent); }
			set { SetValue ((int) PropertyTerminalArrivalAreaSector.Extent, value); }
		}
		
		public List <Obstruction> SignificantObstacle
		{
			get { return GetObjectList <Obstruction> ((int) PropertyTerminalArrivalAreaSector.SignificantObstacle); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyTerminalArrivalAreaSector
	{
		FlyByCode = PropertyAObject.NEXT_CLASS,
		ProcedureTurnRequired,
		AltitudeDescription,
		SectorDefinition,
		Extent,
		SignificantObstacle,
		NEXT_CLASS
	}
	
	public static class MetadataTerminalArrivalAreaSector
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataTerminalArrivalAreaSector ()
		{
			PropInfoList = MetadataAObject.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyTerminalArrivalAreaSector.FlyByCode, (int) AimFieldType.SysBool, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyTerminalArrivalAreaSector.ProcedureTurnRequired, (int) AimFieldType.SysBool, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyTerminalArrivalAreaSector.AltitudeDescription, (int) EnumType.CodeAltitudeUse, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyTerminalArrivalAreaSector.SectorDefinition, (int) ObjectType.CircleSector, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyTerminalArrivalAreaSector.Extent, (int) ObjectType.Surface, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyTerminalArrivalAreaSector.SignificantObstacle, (int) ObjectType.Obstruction, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyDBEntity.Annotation, (int) ObjectType.Note, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
		}
	}
}
