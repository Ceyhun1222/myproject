using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Objects;

namespace Aran.Aim.Features
{
	public abstract class DirectFlight : AObject
	{
		public virtual DirectFlightType DirectFlightType 
		{
			get { return (DirectFlightType) ObjectType; }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyDirectFlight
	{
        NEXT_CLASS = PropertyAObject.NEXT_CLASS
	}
	
	public static class MetadataDirectFlight
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataDirectFlight ()
		{
			PropInfoList = MetadataAObject.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyDBEntity.Annotation, (int) ObjectType.Note, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
		}
	}
}
