using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Metadata.ISO;

namespace Aran.Aim.Metadata.ISO
{
	public class LiSource : BtAbstractObject
	{
		public override ObjectType ObjectType
		{
			get { return ObjectType.LiSource; }
		}
		
		public string Description
		{
			get { return GetFieldValue <string> ((int) PropertyLiSource.Description); }
			set { SetFieldValue <string> ((int) PropertyLiSource.Description, value); }
		}
		
		public int? ScaleDenominator
		{
			get { return GetNullableFieldValue <int> ((int) PropertyLiSource.ScaleDenominator); }
			set { SetNullableFieldValue <int> ((int) PropertyLiSource.ScaleDenominator, value); }
		}
		
		public RsIdentifier ReferenceSystem
		{
			get { return GetObject <RsIdentifier> ((int) PropertyLiSource.ReferenceSystem); }
			set { SetValue ((int) PropertyLiSource.ReferenceSystem, value); }
		}
		
		public CiCitation SourceCitation
		{
			get { return GetObject <CiCitation> ((int) PropertyLiSource.SourceCitation); }
			set { SetValue ((int) PropertyLiSource.SourceCitation, value); }
		}
		
		public List <ExExtent> SourceExtent
		{
			get { return GetObjectList <ExExtent> ((int) PropertyLiSource.SourceExtent); }
		}
		
		public List <LiProcessStep> SourceStep
		{
			get { return GetObjectList <LiProcessStep> ((int) PropertyLiSource.SourceStep); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyLiSource
	{
		Description = PropertyBtAbstractObject.NEXT_CLASS,
		ScaleDenominator,
		ReferenceSystem,
		SourceCitation,
		SourceExtent,
		SourceStep,
		NEXT_CLASS
	}
	
	public static class MetadataLiSource
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataLiSource ()
		{
			PropInfoList = MetadataBtAbstractObject.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyLiSource.Description, (int) AimFieldType.SysString, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyLiSource.ScaleDenominator, (int) AimFieldType.SysInt32, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyLiSource.ReferenceSystem, (int) ObjectType.RsIdentifier, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyLiSource.SourceCitation, (int) ObjectType.CiCitation, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyLiSource.SourceExtent, (int) ObjectType.ExExtent, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyLiSource.SourceStep, (int) ObjectType.LiProcessStep, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
		}
	}
}
