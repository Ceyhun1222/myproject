using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Features;
using Aran.Aim.DataTypes;
using Aran.Aim.Enums;

namespace Aran.Aim.Features
{
	public class RunwayBlastPad : Feature
	{
		public override FeatureType FeatureType
		{
			get { return FeatureType.RunwayBlastPad; }
		}
		
		public ValDistance Length
		{
			get { return (ValDistance ) GetValue ((int) PropertyRunwayBlastPad.Length); }
			set { SetValue ((int) PropertyRunwayBlastPad.Length, value); }
		}
		
		public CodeStatusOperations? Status
		{
			get { return GetNullableFieldValue <CodeStatusOperations> ((int) PropertyRunwayBlastPad.Status); }
			set { SetNullableFieldValue <CodeStatusOperations> ((int) PropertyRunwayBlastPad.Status, value); }
		}
		
		public FeatureRef UsedRunwayDirection
		{
			get { return (FeatureRef ) GetValue ((int) PropertyRunwayBlastPad.UsedRunwayDirection); }
			set { SetValue ((int) PropertyRunwayBlastPad.UsedRunwayDirection, value); }
		}
		
		public ElevatedSurface Extent
		{
			get { return GetObject <ElevatedSurface> ((int) PropertyRunwayBlastPad.Extent); }
			set { SetValue ((int) PropertyRunwayBlastPad.Extent, value); }
		}
		
		public SurfaceCharacteristics SurfaceProperties
		{
			get { return GetObject <SurfaceCharacteristics> ((int) PropertyRunwayBlastPad.SurfaceProperties); }
			set { SetValue ((int) PropertyRunwayBlastPad.SurfaceProperties, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyRunwayBlastPad
	{
		Length = PropertyFeature.NEXT_CLASS,
		Status,
		UsedRunwayDirection,
		Extent,
		SurfaceProperties,
		NEXT_CLASS
	}
	
	public static class MetadataRunwayBlastPad
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataRunwayBlastPad ()
		{
			PropInfoList = MetadataFeature.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyRunwayBlastPad.Length, (int) DataType.ValDistance, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyRunwayBlastPad.Status, (int) EnumType.CodeStatusOperations, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyRunwayBlastPad.UsedRunwayDirection, (int) FeatureType.RunwayDirection, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyRunwayBlastPad.Extent, (int) ObjectType.ElevatedSurface, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyRunwayBlastPad.SurfaceProperties, (int) ObjectType.SurfaceCharacteristics, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyDBEntity.Annotation, (int) ObjectType.Note, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyFeature.Metadata, (int) ObjectType.MdMetadata, PropertyTypeCharacter.Nullable);
		}
	}
}
