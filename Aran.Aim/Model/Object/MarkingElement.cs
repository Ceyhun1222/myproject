using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Objects;
using Aran.Aim.Enums;
using Aran.Aim.Features;

namespace Aran.Aim.Features
{
	public class MarkingElement : AObject
	{
		public override ObjectType ObjectType
		{
			get { return ObjectType.MarkingElement; }
		}
		
		public CodeColour? Colour
		{
			get { return GetNullableFieldValue <CodeColour> ((int) PropertyMarkingElement.Colour); }
			set { SetNullableFieldValue <CodeColour> ((int) PropertyMarkingElement.Colour, value); }
		}
		
		public CodeMarkingStyle? Style
		{
			get { return GetNullableFieldValue <CodeMarkingStyle> ((int) PropertyMarkingElement.Style); }
			set { SetNullableFieldValue <CodeMarkingStyle> ((int) PropertyMarkingElement.Style, value); }
		}
		
		public MarkingExtent Extent
		{
			get { return GetObject <MarkingExtent> ((int) PropertyMarkingElement.Extent); }
			set { SetValue ((int) PropertyMarkingElement.Extent, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyMarkingElement
	{
		Colour = PropertyAObject.NEXT_CLASS,
		Style,
		Extent,
		NEXT_CLASS
	}
	
	public static class MetadataMarkingElement
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataMarkingElement ()
		{
			PropInfoList = MetadataAObject.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyMarkingElement.Colour, (int) EnumType.CodeColour, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyMarkingElement.Style, (int) EnumType.CodeMarkingStyle, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyMarkingElement.Extent, (int) ObjectType.MarkingExtent, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyDBEntity.Annotation, (int) ObjectType.Note, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
		}
	}
}
