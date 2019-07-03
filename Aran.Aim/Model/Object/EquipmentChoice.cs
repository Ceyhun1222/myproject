using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Objects;
using Aran.Aim.DataTypes;
using Aran.Aim.Model.Attribute;

namespace Aran.Aim.Features
{
	public class EquipmentChoice : ChoiceClass
	{
		public override ObjectType ObjectType
		{
			get { return ObjectType.EquipmentChoice; }
		}
		
		public EquipmentChoiceChoice Choice
		{
			get { return (EquipmentChoiceChoice) RefType; }
		}

       public AbstractNavaidEquipmentRef NavaidEquipment
		{
			get { return (AbstractNavaidEquipmentRef) RefAbstractFeature; }
			set
			{
				RefAbstractFeature = value;
				RefType = (int) EquipmentChoiceChoice.AbstractNavaidEquipmentRef;
			}
		}

        [LinkedFeature(FeatureType.RadioCommunicationChannel)]
		public FeatureRef Frequency
		{
			get { return RefFeature; }
			set
			{
				RefFeature = value;
				RefType = (int) EquipmentChoiceChoice.RadioCommunicationChannel;
			}
		}

          [LinkedFeature(FeatureType.SpecialNavigationStation)]
		public FeatureRef SpecialNavigationStation
		{
			get { return RefFeature; }
			set
			{
				RefFeature = value;
				RefType = (int) EquipmentChoiceChoice.SpecialNavigationStation;
			}
		}


          [LinkedFeature(FeatureType.PrecisionApproachRadar)]
		public FeatureRef PrecisionApproachRadar
		{
			get { return RefFeature; }
			set
			{
				RefFeature = value;
				RefType = (int) EquipmentChoiceChoice.PrecisionApproachRadar;
			}
		}



          [LinkedFeature(FeatureType.SecondarySurveillanceRadar)]
		public FeatureRef Radar
		{
			get { return RefFeature; }
			set
			{
				RefFeature = value;
				RefType = (int) EquipmentChoiceChoice.SecondarySurveillanceRadar;
			}
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyEquipmentChoice
	{
		NavaidEquipment = PropertyAObject.NEXT_CLASS,
		Frequency,
		SpecialNavigationStation,
		PrecisionApproachRadar,
		Radar,
		NEXT_CLASS
	}
	
	public static class MetadataEquipmentChoice
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataEquipmentChoice ()
		{
			PropInfoList = MetadataAObject.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyEquipmentChoice.NavaidEquipment, (int) DataType.AbstractNavaidEquipmentRef, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyEquipmentChoice.Frequency, (int) FeatureType.RadioCommunicationChannel, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyEquipmentChoice.SpecialNavigationStation, (int) FeatureType.SpecialNavigationStation, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyEquipmentChoice.PrecisionApproachRadar, (int) FeatureType.PrecisionApproachRadar, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyEquipmentChoice.Radar, (int) FeatureType.SecondarySurveillanceRadar, PropertyTypeCharacter.Nullable);
		}
	}
}
