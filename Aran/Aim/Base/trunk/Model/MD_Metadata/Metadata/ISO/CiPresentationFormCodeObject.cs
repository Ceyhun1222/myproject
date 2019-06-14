using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Objects;
using Aran.Aim.Enums;

namespace Aran.Aim.Metadata.ISO
{
	public class CiPresentationFormCodeObject : AObject
	{
		public override ObjectType ObjectType
		{
			get { return ObjectType.CiPresentationFormCodeObject; }
		}
		
		public CiPresentationFormCode? Value
		{
			get { return GetNullableFieldValue <CiPresentationFormCode> ((int) PropertyCiPresentationFormCodeObject.Value); }
			set { SetNullableFieldValue <CiPresentationFormCode> ((int) PropertyCiPresentationFormCodeObject.Value, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyCiPresentationFormCodeObject
	{
		Value = PropertyAObject.NEXT_CLASS,
		NEXT_CLASS
	}
	
	public static class MetadataCiPresentationFormCodeObject
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataCiPresentationFormCodeObject ()
		{
			PropInfoList = MetadataAObject.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyCiPresentationFormCodeObject.Value, (int) EnumType.CiPresentationFormCode, PropertyTypeCharacter.Nullable);
		}
	}
}
