using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Objects;
using Aran.Aim.Enums;
using Aran.Aim.Features;

namespace Aran.Aim.Features
{
	public class ProcedureTransition : AObject
	{
		public override ObjectType ObjectType
		{
			get { return ObjectType.ProcedureTransition; }
		}
		
		public string TransitionId
		{
			get { return GetFieldValue <string> ((int) PropertyProcedureTransition.TransitionId); }
			set { SetFieldValue <string> ((int) PropertyProcedureTransition.TransitionId, value); }
		}
		
		public CodeProcedurePhase? Type
		{
			get { return GetNullableFieldValue <CodeProcedurePhase> ((int) PropertyProcedureTransition.Type); }
			set { SetNullableFieldValue <CodeProcedurePhase> ((int) PropertyProcedureTransition.Type, value); }
		}
		
		public string Instruction
		{
			get { return GetFieldValue <string> ((int) PropertyProcedureTransition.Instruction); }
			set { SetFieldValue <string> ((int) PropertyProcedureTransition.Instruction, value); }
		}
		
		public double? VectorHeading
		{
			get { return GetNullableFieldValue <double> ((int) PropertyProcedureTransition.VectorHeading); }
			set { SetNullableFieldValue <double> ((int) PropertyProcedureTransition.VectorHeading, value); }
		}
		
		public LandingTakeoffAreaCollection DepartureRunwayTransition
		{
			get { return GetObject <LandingTakeoffAreaCollection> ((int) PropertyProcedureTransition.DepartureRunwayTransition); }
			set { SetValue ((int) PropertyProcedureTransition.DepartureRunwayTransition, value); }
		}
		
		public Curve Trajectory
		{
			get { return GetObject <Curve> ((int) PropertyProcedureTransition.Trajectory); }
			set { SetValue ((int) PropertyProcedureTransition.Trajectory, value); }
		}
		
		public List <ProcedureTransitionLeg> TransitionLeg
		{
			get { return GetObjectList <ProcedureTransitionLeg> ((int) PropertyProcedureTransition.TransitionLeg); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyProcedureTransition
	{
		TransitionId = PropertyAObject.NEXT_CLASS,
		Type,
		Instruction,
		VectorHeading,
		DepartureRunwayTransition,
		Trajectory,
		TransitionLeg,
		NEXT_CLASS
	}
	
	public static class MetadataProcedureTransition
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataProcedureTransition ()
		{
			PropInfoList = MetadataAObject.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyProcedureTransition.TransitionId, (int) AimFieldType.SysString, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyProcedureTransition.Type, (int) EnumType.CodeProcedurePhase, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyProcedureTransition.Instruction, (int) AimFieldType.SysString, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyProcedureTransition.VectorHeading, (int) AimFieldType.SysDouble, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyProcedureTransition.DepartureRunwayTransition, (int) ObjectType.LandingTakeoffAreaCollection, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyProcedureTransition.Trajectory, (int) ObjectType.Curve, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyProcedureTransition.TransitionLeg, (int) ObjectType.ProcedureTransitionLeg, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyDBEntity.Annotation, (int) ObjectType.Note, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
		}
	}
}
