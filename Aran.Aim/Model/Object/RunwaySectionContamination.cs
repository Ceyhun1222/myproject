using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Features;
using Aran.Aim.Enums;

namespace Aran.Aim.Features
{
	public class RunwaySectionContamination : SurfaceContamination
	{
		public override ObjectType ObjectType
		{
			get { return ObjectType.RunwaySectionContamination; }
		}
		
		public CodeRunwaySection? Section
		{
			get { return GetNullableFieldValue <CodeRunwaySection> ((int) PropertyRunwaySectionContamination.Section); }
			set { SetNullableFieldValue <CodeRunwaySection> ((int) PropertyRunwaySectionContamination.Section, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyRunwaySectionContamination
	{
		Section = PropertySurfaceContamination.NEXT_CLASS,
		NEXT_CLASS
	}
	
	public static class MetadataRunwaySectionContamination
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataRunwaySectionContamination ()
		{
			PropInfoList = MetadataSurfaceContamination.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyRunwaySectionContamination.Section, (int) EnumType.CodeRunwaySection, PropertyTypeCharacter.Nullable);
		}
	}
}
