using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Metadata.ISO;

namespace Aran.Aim.Metadata.ISO
{
	public class MdLegalConstraints : MdConstraints
	{
		public override ObjectType ObjectType
		{
			get { return ObjectType.MdLegalConstraints; }
		}
		
		public List <MdRestrictionCodeObject> AccessConstraints
		{
			get { return GetObjectList <MdRestrictionCodeObject> ((int) PropertyMdLegalConstraints.AccessConstraints); }
		}
		
		public List <MdRestrictionCodeObject> UseConstraints
		{
			get { return GetObjectList <MdRestrictionCodeObject> ((int) PropertyMdLegalConstraints.UseConstraints); }
		}
		
		public List <BtString> OtherConstraintsField
		{
			get { return GetObjectList <BtString> ((int) PropertyMdLegalConstraints.OtherConstraintsField); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyMdLegalConstraints
	{
		AccessConstraints = PropertyMdConstraints.NEXT_CLASS,
		UseConstraints,
		OtherConstraintsField,
		NEXT_CLASS
	}
	
	public static class MetadataMdLegalConstraints
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataMdLegalConstraints ()
		{
			PropInfoList = MetadataMdConstraints.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyMdLegalConstraints.AccessConstraints, (int) ObjectType.MdRestrictionCodeObject, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyMdLegalConstraints.UseConstraints, (int) ObjectType.MdRestrictionCodeObject, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyMdLegalConstraints.OtherConstraintsField, (int) ObjectType.BtString, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
		}
	}
}
