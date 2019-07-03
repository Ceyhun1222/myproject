using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Features;
using Aran.Aim.Enums;
using Aran.Aim.DataTypes;

namespace Aran.Aim.Features
{
	public class EnRouteSegmentPoint : SegmentPoint
	{
		public override ObjectType ObjectType
		{
			get { return ObjectType.EnRouteSegmentPoint; }
		}
		
		public CodeFreeFlight? RoleFreeFlight
		{
			get { return GetNullableFieldValue <CodeFreeFlight> ((int) PropertyEnRouteSegmentPoint.RoleFreeFlight); }
			set { SetNullableFieldValue <CodeFreeFlight> ((int) PropertyEnRouteSegmentPoint.RoleFreeFlight, value); }
		}
		
		public CodeRVSMPointRole? RoleRVSM
		{
			get { return GetNullableFieldValue <CodeRVSMPointRole> ((int) PropertyEnRouteSegmentPoint.RoleRVSM); }
			set { SetNullableFieldValue <CodeRVSMPointRole> ((int) PropertyEnRouteSegmentPoint.RoleRVSM, value); }
		}
		
		public ValDistance TurnRadius
		{
			get { return (ValDistance ) GetValue ((int) PropertyEnRouteSegmentPoint.TurnRadius); }
			set { SetValue ((int) PropertyEnRouteSegmentPoint.TurnRadius, value); }
		}
		
		public CodeMilitaryRoutePoint? RoleMilitaryTraining
		{
			get { return GetNullableFieldValue <CodeMilitaryRoutePoint> ((int) PropertyEnRouteSegmentPoint.RoleMilitaryTraining); }
			set { SetNullableFieldValue <CodeMilitaryRoutePoint> ((int) PropertyEnRouteSegmentPoint.RoleMilitaryTraining, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyEnRouteSegmentPoint
	{
		RoleFreeFlight = PropertySegmentPoint.NEXT_CLASS,
		RoleRVSM,
		TurnRadius,
		RoleMilitaryTraining,
		NEXT_CLASS
	}
	
	public static class MetadataEnRouteSegmentPoint
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataEnRouteSegmentPoint ()
		{
			PropInfoList = MetadataSegmentPoint.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyEnRouteSegmentPoint.RoleFreeFlight, (int) EnumType.CodeFreeFlight, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyEnRouteSegmentPoint.RoleRVSM, (int) EnumType.CodeRVSMPointRole, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyEnRouteSegmentPoint.TurnRadius, (int) DataType.ValDistance, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyEnRouteSegmentPoint.RoleMilitaryTraining, (int) EnumType.CodeMilitaryRoutePoint, PropertyTypeCharacter.Nullable);
		}
	}
}
