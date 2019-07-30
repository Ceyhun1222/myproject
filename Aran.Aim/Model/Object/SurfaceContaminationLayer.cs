using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Objects;
using Aran.Aim.Enums;
using Aran.Aim.Features;

namespace Aran.Aim.Features
{
	public class SurfaceContaminationLayer : AObject
	{
		public override ObjectType ObjectType
		{
			get { return ObjectType.SurfaceContaminationLayer; }
		}
		
		public uint? LayerOrder
		{
			get { return GetNullableFieldValue <uint> ((int) PropertySurfaceContaminationLayer.LayerOrder); }
			set { SetNullableFieldValue <uint> ((int) PropertySurfaceContaminationLayer.LayerOrder, value); }
		}
		
		public CodeContamination? Type
		{
			get { return GetNullableFieldValue <CodeContamination> ((int) PropertySurfaceContaminationLayer.Type); }
			set { SetNullableFieldValue <CodeContamination> ((int) PropertySurfaceContaminationLayer.Type, value); }
		}
		
		public List <ElevatedSurface> Extent
		{
			get { return GetObjectList <ElevatedSurface> ((int) PropertySurfaceContaminationLayer.Extent); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertySurfaceContaminationLayer
	{
		LayerOrder = PropertyAObject.NEXT_CLASS,
		Type,
		Extent,
		NEXT_CLASS
	}
	
	public static class MetadataSurfaceContaminationLayer
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataSurfaceContaminationLayer ()
		{
			PropInfoList = MetadataAObject.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertySurfaceContaminationLayer.LayerOrder, (int) AimFieldType.SysUInt32, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertySurfaceContaminationLayer.Type, (int) EnumType.CodeContamination, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertySurfaceContaminationLayer.Extent, (int) ObjectType.ElevatedSurface, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyDBEntity.Annotation, (int) ObjectType.Note, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
		}
	}
}
