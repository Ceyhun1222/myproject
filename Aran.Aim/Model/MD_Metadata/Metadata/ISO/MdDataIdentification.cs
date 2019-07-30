using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Metadata.ISO;

namespace Aran.Aim.Metadata.ISO
{
	public class MdDataIdentification : MdAbstractIdentification
	{
		public override ObjectType ObjectType
		{
			get { return ObjectType.MdDataIdentification; }
		}
		
		public string Language
		{
			get { return GetFieldValue <string> ((int) PropertyMdDataIdentification.Language); }
			set { SetFieldValue <string> ((int) PropertyMdDataIdentification.Language, value); }
		}
		
		public string EnvironmentDescription
		{
			get { return GetFieldValue <string> ((int) PropertyMdDataIdentification.EnvironmentDescription); }
			set { SetFieldValue <string> ((int) PropertyMdDataIdentification.EnvironmentDescription, value); }
		}
		
		public List <ExExtent> Extent
		{
			get { return GetObjectList <ExExtent> ((int) PropertyMdDataIdentification.Extent); }
		}
		
		public string SupplementalInformation
		{
			get { return GetFieldValue <string> ((int) PropertyMdDataIdentification.SupplementalInformation); }
			set { SetFieldValue <string> ((int) PropertyMdDataIdentification.SupplementalInformation, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyMdDataIdentification
	{
		Language = PropertyMdAbstractIdentification.NEXT_CLASS,
		EnvironmentDescription,
		Extent,
		SupplementalInformation,
		NEXT_CLASS
	}
	
	public static class MetadataMdDataIdentification
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataMdDataIdentification ()
		{
			PropInfoList = MetadataMdAbstractIdentification.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyMdDataIdentification.Language, (int) AimFieldType.SysString, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyMdDataIdentification.EnvironmentDescription, (int) AimFieldType.SysString, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyMdDataIdentification.Extent, (int) ObjectType.ExExtent, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyMdDataIdentification.SupplementalInformation, (int) AimFieldType.SysString, PropertyTypeCharacter.Nullable);
		}
	}
}
