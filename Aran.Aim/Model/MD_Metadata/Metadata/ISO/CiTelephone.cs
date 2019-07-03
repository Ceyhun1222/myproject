using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Metadata.ISO;

namespace Aran.Aim.Metadata.ISO
{
	public class CiTelephone : BtAbstractObject
	{
		public override ObjectType ObjectType
		{
			get { return ObjectType.CiTelephone; }
		}
		
		public List <BtString> Voice
		{
			get { return GetObjectList <BtString> ((int) PropertyCiTelephone.Voice); }
		}
		
		public List <BtString> Favsimile
		{
			get { return GetObjectList <BtString> ((int) PropertyCiTelephone.Favsimile); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyCiTelephone
	{
		Voice = PropertyBtAbstractObject.NEXT_CLASS,
		Favsimile,
		NEXT_CLASS
	}
	
	public static class MetadataCiTelephone
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataCiTelephone ()
		{
			PropInfoList = MetadataBtAbstractObject.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyCiTelephone.Voice, (int) ObjectType.BtString, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyCiTelephone.Favsimile, (int) ObjectType.BtString, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
		}
	}
}
