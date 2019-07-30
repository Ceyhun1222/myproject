using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Objects;
using Aran.Aim.Enums;
using Aran.Aim.DataTypes;
using Aran.Aim.Features;

namespace Aran.Aim.Features
{
	public abstract class UsageCondition : AObject
	{
		public virtual UsageConditionType UsageConditionType 
		{
			get { return (UsageConditionType) ObjectType; }
		}
		
		public CodeUsageLimitation? Type
		{
			get { return GetNullableFieldValue <CodeUsageLimitation> ((int) PropertyUsageCondition.Type); }
			set { SetNullableFieldValue <CodeUsageLimitation> ((int) PropertyUsageCondition.Type, value); }
		}
		
		public ValDuration PriorPermission
		{
			get { return (ValDuration ) GetValue ((int) PropertyUsageCondition.PriorPermission); }
			set { SetValue ((int) PropertyUsageCondition.PriorPermission, value); }
		}
		
		public List <ContactInformation> Contact
		{
			get { return GetObjectList <ContactInformation> ((int) PropertyUsageCondition.Contact); }
		}
		
		public ConditionCombination Selection
		{
			get { return GetObject <ConditionCombination> ((int) PropertyUsageCondition.Selection); }
			set { SetValue ((int) PropertyUsageCondition.Selection, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyUsageCondition
	{
		Type = PropertyAObject.NEXT_CLASS,
		PriorPermission,
		Contact,
		Selection,
		NEXT_CLASS
	}
	
	public static class MetadataUsageCondition
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataUsageCondition ()
		{
			PropInfoList = MetadataAObject.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyUsageCondition.Type, (int) EnumType.CodeUsageLimitation, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyUsageCondition.PriorPermission, (int) DataType.ValDuration, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyUsageCondition.Contact, (int) ObjectType.ContactInformation, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyUsageCondition.Selection, (int) ObjectType.ConditionCombination, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyDBEntity.Annotation, (int) ObjectType.Note, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
		}
	}
}
