using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Objects;
using Aran.Aim.Enums;
using Aran.Aim.Features;

namespace Aran.Aim.Features
{
	public class Note : AObject
	{
		public override ObjectType ObjectType
		{
			get { return ObjectType.Note; }
		}
		
		public string PropertyName
		{
			get { return GetFieldValue <string> ((int) PropertyNote.PropertyName); }
			set { SetFieldValue <string> ((int) PropertyNote.PropertyName, value); }
		}
		
		public CodeNotePurpose? Purpose
		{
			get { return GetNullableFieldValue <CodeNotePurpose> ((int) PropertyNote.Purpose); }
			set { SetNullableFieldValue <CodeNotePurpose> ((int) PropertyNote.Purpose, value); }
		}
		
		public List <LinguisticNote> TranslatedNote
		{
			get { return GetObjectList <LinguisticNote> ((int) PropertyNote.TranslatedNote); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyNote
	{
		PropertyName = PropertyAObject.NEXT_CLASS,
		Purpose,
		TranslatedNote,
		NEXT_CLASS
	}
	
	public static class MetadataNote
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataNote ()
		{
			PropInfoList = MetadataAObject.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyNote.PropertyName, (int) AimFieldType.SysString, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyNote.Purpose, (int) EnumType.CodeNotePurpose, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyNote.TranslatedNote, (int) ObjectType.LinguisticNote, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
		}
	}
}
