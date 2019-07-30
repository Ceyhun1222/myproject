using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Schema;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Objects;
using Aran.Aim.DataTypes;
using Aran.Aim.Features;
using Aran.Aim.Model.Attribute;

namespace Aran.Aim.Features
{

    public class SignificantPoint : ChoiceClass
	{
		public override ObjectType ObjectType
		{
			get { return ObjectType.SignificantPoint; }
		}
		
		public SignificantPointChoice Choice
		{
			get { return (SignificantPointChoice) RefType; }
		}

        [LinkedFeature(FeatureType.DesignatedPoint)]
		public FeatureRef FixDesignatedPoint
		{
			get { return RefFeature; }
			set
			{
				RefFeature = value;
				RefType = (int) SignificantPointChoice.DesignatedPoint;
			}
		}

        [LinkedFeature(FeatureType.Navaid)]
		public FeatureRef NavaidSystem
		{
			get { return RefFeature; }
			set
			{
				RefFeature = value;
				RefType = (int) SignificantPointChoice.Navaid;
			}
		}

        [LinkedFeature(FeatureType.TouchDownLiftOff)]
		public FeatureRef AimingPoint
		{
			get { return RefFeature; }
			set
			{
				RefFeature = value;
				RefType = (int) SignificantPointChoice.TouchDownLiftOff;
			}
		}

        [LinkedFeature(FeatureType.RunwayCentrelinePoint)]
		public FeatureRef RunwayPoint
		{
			get { return RefFeature; }
			set
			{
				RefFeature = value;
				RefType = (int) SignificantPointChoice.RunwayCentrelinePoint;
			}
		}

        [LinkedFeature(FeatureType.AirportHeliport)]
		public FeatureRef AirportReferencePoint
		{
			get { return RefFeature; }
			set
			{
				RefFeature = value;
				RefType = (int) SignificantPointChoice.AirportHeliport;
			}
		}
		
		public AixmPoint Position
		{
			get { return (AixmPoint) RefValue; }
			set
			{
				RefValue = value;
				RefType = (int) SignificantPointChoice.AixmPoint;
			}
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertySignificantPoint
	{
		FixDesignatedPoint = PropertyAObject.NEXT_CLASS,
		NavaidSystem,
		AimingPoint,
		RunwayPoint,
		AirportReferencePoint,
		Position,
		NEXT_CLASS
	}
	
	public static class MetadataSignificantPoint
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataSignificantPoint ()
		{
			PropInfoList = MetadataAObject.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertySignificantPoint.FixDesignatedPoint, (int) FeatureType.DesignatedPoint, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertySignificantPoint.NavaidSystem, (int) FeatureType.Navaid, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertySignificantPoint.AimingPoint, (int) FeatureType.TouchDownLiftOff, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertySignificantPoint.RunwayPoint, (int) FeatureType.RunwayCentrelinePoint, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertySignificantPoint.AirportReferencePoint, (int) FeatureType.AirportHeliport, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertySignificantPoint.Position, (int) ObjectType.AixmPoint, PropertyTypeCharacter.Nullable);
		}
	}
}
