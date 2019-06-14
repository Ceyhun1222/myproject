using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Objects;
using Aran.Aim.Enums;
using Aran.Aim.DataTypes;

namespace Aran.Aim.Features
{
	public class UnitDependency : AObject
	{
		public override ObjectType ObjectType
		{
			get { return ObjectType.UnitDependency; }
		}
		
		public CodeUnitDependency? Type
		{
			get { return GetNullableFieldValue <CodeUnitDependency> ((int) PropertyUnitDependency.Type); }
			set { SetNullableFieldValue <CodeUnitDependency> ((int) PropertyUnitDependency.Type, value); }
		}
		
		public FeatureRef TheUnit
		{
			get { return (FeatureRef ) GetValue ((int) PropertyUnitDependency.TheUnit); }
			set { SetValue ((int) PropertyUnitDependency.TheUnit, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyUnitDependency
	{
		Type = PropertyAObject.NEXT_CLASS,
		TheUnit,
		NEXT_CLASS
	}
	
	public static class MetadataUnitDependency
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataUnitDependency ()
		{
			PropInfoList = MetadataAObject.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyUnitDependency.Type, (int) EnumType.CodeUnitDependency, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyDBEntity.Annotation, (int) ObjectType.Note, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyUnitDependency.TheUnit, (int) FeatureType.Unit, PropertyTypeCharacter.Nullable);
		}
	}
}
