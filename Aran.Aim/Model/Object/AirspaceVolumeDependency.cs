using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Objects;
using Aran.Aim.Enums;
using Aran.Aim.DataTypes;

namespace Aran.Aim.Features
{
	public class AirspaceVolumeDependency : AObject
	{
		public override ObjectType ObjectType
		{
			get { return ObjectType.AirspaceVolumeDependency; }
		}
		
		public CodeAirspaceDependency? Dependency
		{
			get { return GetNullableFieldValue <CodeAirspaceDependency> ((int) PropertyAirspaceVolumeDependency.Dependency); }
			set { SetNullableFieldValue <CodeAirspaceDependency> ((int) PropertyAirspaceVolumeDependency.Dependency, value); }
		}
		
		public FeatureRef TheAirspace
		{
			get { return (FeatureRef ) GetValue ((int) PropertyAirspaceVolumeDependency.TheAirspace); }
			set { SetValue ((int) PropertyAirspaceVolumeDependency.TheAirspace, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyAirspaceVolumeDependency
	{
		Dependency = PropertyAObject.NEXT_CLASS,
		TheAirspace,
		NEXT_CLASS
	}
	
	public static class MetadataAirspaceVolumeDependency
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataAirspaceVolumeDependency ()
		{
			PropInfoList = MetadataAObject.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyAirspaceVolumeDependency.Dependency, (int) EnumType.CodeAirspaceDependency, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyDBEntity.Annotation, (int) ObjectType.Note, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyAirspaceVolumeDependency.TheAirspace, (int) FeatureType.Airspace, PropertyTypeCharacter.Nullable);
		}
	}
}
