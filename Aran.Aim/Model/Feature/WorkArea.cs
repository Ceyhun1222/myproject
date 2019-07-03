using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Features;
using Aran.Aim.Enums;
using Aran.Aim.DataTypes;

namespace Aran.Aim.Features
{
	public class WorkArea : Feature
	{
		public override FeatureType FeatureType
		{
			get { return FeatureType.WorkArea; }
		}
		
		public CodeWorkArea? Type
		{
			get { return GetNullableFieldValue <CodeWorkArea> ((int) PropertyWorkArea.Type); }
			set { SetNullableFieldValue <CodeWorkArea> ((int) PropertyWorkArea.Type, value); }
		}
		
		public DateTime? PlannedOperational
		{
			get { return GetNullableFieldValue <DateTime> ((int) PropertyWorkArea.PlannedOperational); }
			set { SetNullableFieldValue <DateTime> ((int) PropertyWorkArea.PlannedOperational, value); }
		}
		
		public FeatureRef AssociatedAirportHeliport
		{
			get { return (FeatureRef ) GetValue ((int) PropertyWorkArea.AssociatedAirportHeliport); }
			set { SetValue ((int) PropertyWorkArea.AssociatedAirportHeliport, value); }
		}
		
		public ElevatedSurface Extent
		{
			get { return GetObject <ElevatedSurface> ((int) PropertyWorkArea.Extent); }
			set { SetValue ((int) PropertyWorkArea.Extent, value); }
		}
		
		public List <WorkareaActivity> Activation
		{
			get { return GetObjectList <WorkareaActivity> ((int) PropertyWorkArea.Activation); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyWorkArea
	{
		Type = PropertyFeature.NEXT_CLASS,
		PlannedOperational,
		AssociatedAirportHeliport,
		Extent,
		Activation,
		NEXT_CLASS
	}
	
	public static class MetadataWorkArea
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataWorkArea ()
		{
			PropInfoList = MetadataFeature.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyWorkArea.Type, (int) EnumType.CodeWorkArea, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyWorkArea.PlannedOperational, (int) AimFieldType.SysDateTime, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyWorkArea.AssociatedAirportHeliport, (int) FeatureType.AirportHeliport, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyWorkArea.Extent, (int) ObjectType.ElevatedSurface, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyWorkArea.Activation, (int) ObjectType.WorkareaActivity, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyDBEntity.Annotation, (int) ObjectType.Note, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyFeature.Metadata, (int) ObjectType.MdMetadata, PropertyTypeCharacter.Nullable);
		}
	}
}
