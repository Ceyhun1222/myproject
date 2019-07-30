using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Metadata.ISO;

namespace Aran.Aim.Metadata.ISO
{
	public class LiLineage : BtAbstractObject
	{
		public override ObjectType ObjectType
		{
			get { return ObjectType.LiLineage; }
		}
		
		public string Statement
		{
			get { return GetFieldValue <string> ((int) PropertyLiLineage.Statement); }
			set { SetFieldValue <string> ((int) PropertyLiLineage.Statement, value); }
		}
		
		public List <LiProcessStep> ProcessStep
		{
			get { return GetObjectList <LiProcessStep> ((int) PropertyLiLineage.ProcessStep); }
		}
		
		public List <LiSource> Source
		{
			get { return GetObjectList <LiSource> ((int) PropertyLiLineage.Source); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyLiLineage
	{
		Statement = PropertyBtAbstractObject.NEXT_CLASS,
		ProcessStep,
		Source,
		NEXT_CLASS
	}
	
	public static class MetadataLiLineage
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataLiLineage ()
		{
			PropInfoList = MetadataBtAbstractObject.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyLiLineage.Statement, (int) AimFieldType.SysString, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyLiLineage.ProcessStep, (int) ObjectType.LiProcessStep, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyLiLineage.Source, (int) ObjectType.LiSource, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
		}
	}
}
