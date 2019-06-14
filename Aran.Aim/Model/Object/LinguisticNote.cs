using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Objects;
using Aran.Aim.DataTypes;

namespace Aran.Aim.Features
{
	public class LinguisticNote : AObject
	{
		public override ObjectType ObjectType
		{
			get { return ObjectType.LinguisticNote; }
		}
		
		public TextNote Note
		{
			get { return (TextNote ) GetValue ((int) PropertyLinguisticNote.Note); }
			set { SetValue ((int) PropertyLinguisticNote.Note, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyLinguisticNote
	{
		Note = PropertyAObject.NEXT_CLASS,
		NEXT_CLASS
	}
	
	public static class MetadataLinguisticNote
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataLinguisticNote ()
		{
			PropInfoList = MetadataAObject.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyLinguisticNote.Note, (int) DataType.TextNote, PropertyTypeCharacter.Nullable);
		}
	}
}
