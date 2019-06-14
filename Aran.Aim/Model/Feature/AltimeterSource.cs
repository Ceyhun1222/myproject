using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Features;

namespace Aran.Aim.Features
{
	public class AltimeterSource : Feature
	{
		public override FeatureType FeatureType
		{
			get { return FeatureType.AltimeterSource; }
		}
		
		public bool? IsRemote
		{
			get { return GetNullableFieldValue <bool> ((int) PropertyAltimeterSource.IsRemote); }
			set { SetNullableFieldValue <bool> ((int) PropertyAltimeterSource.IsRemote, value); }
		}
		
		public bool? IsPrimary
		{
			get { return GetNullableFieldValue <bool> ((int) PropertyAltimeterSource.IsPrimary); }
			set { SetNullableFieldValue <bool> ((int) PropertyAltimeterSource.IsPrimary, value); }
		}
		
		public List <AltimeterSourceStatus> Availability
		{
			get { return GetObjectList <AltimeterSourceStatus> ((int) PropertyAltimeterSource.Availability); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyAltimeterSource
	{
		IsRemote = PropertyFeature.NEXT_CLASS,
		IsPrimary,
		Availability,
		NEXT_CLASS
	}
	
	public static class MetadataAltimeterSource
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataAltimeterSource ()
		{
			PropInfoList = MetadataFeature.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyAltimeterSource.IsRemote, (int) AimFieldType.SysBool, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyAltimeterSource.IsPrimary, (int) AimFieldType.SysBool, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyAltimeterSource.Availability, (int) ObjectType.AltimeterSourceStatus, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyDBEntity.Annotation, (int) ObjectType.Note, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyFeature.Metadata, (int) ObjectType.MdMetadata, PropertyTypeCharacter.Nullable);
		}
	}
}
