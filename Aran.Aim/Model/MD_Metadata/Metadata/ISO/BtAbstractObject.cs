using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Objects;

namespace Aran.Aim.Metadata.ISO
{
	public abstract class BtAbstractObject : AObject
	{
		public virtual BtAbstractObjectType BtAbstractObjectType 
		{
			get { return (BtAbstractObjectType) ObjectType; }
		}

        public string MetadataId
        {
            get { return GetFieldValue<string>((int)PropertyBtAbstractObject.MetadataId); }
            set { SetFieldValue<string>((int)PropertyBtAbstractObject.MetadataId, value); }
        }

        public Guid? Uuid
        {
            get { return GetNullableFieldValue<Guid>((int)PropertyBtAbstractObject.Uuid); }
            set { SetNullableFieldValue<Guid>((int)PropertyBtAbstractObject.Uuid, value); }
        }

        public Guid? UuidRef
        {
            get { return GetNullableFieldValue<Guid>((int)PropertyBtAbstractObject.UuidRef); }
            set { SetNullableFieldValue<Guid>((int)PropertyBtAbstractObject.UuidRef, value); }
        }

    }
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyBtAbstractObject
	{
		MetadataId = PropertyAObject.NEXT_CLASS,
		Uuid,
		UuidRef,
		NEXT_CLASS
	}
	
	public static class MetadataBtAbstractObject
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataBtAbstractObject ()
		{
			PropInfoList = MetadataAObject.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyBtAbstractObject.MetadataId, (int) AimFieldType.SysString, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyBtAbstractObject.Uuid, (int) AimFieldType.SysGuid, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyBtAbstractObject.UuidRef, (int) AimFieldType.SysGuid, PropertyTypeCharacter.Nullable);
		}
	}
}
