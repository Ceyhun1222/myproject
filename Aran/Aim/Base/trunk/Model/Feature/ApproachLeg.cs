using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Features;
using Aran.Aim.DataTypes;

namespace Aran.Aim.Features
{
	public abstract class ApproachLeg : SegmentLeg
	{
		public virtual ApproachLegType ApproachLegType 
		{
			get { return (ApproachLegType) FeatureType; }
		}
		
		public FeatureRef Approach
		{
			get { return (FeatureRef ) GetValue ((int) PropertyApproachLeg.Approach); }
			set { SetValue ((int) PropertyApproachLeg.Approach, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyApproachLeg
	{
		Approach = PropertySegmentLeg.NEXT_CLASS,
		NEXT_CLASS
	}
	
	public static class MetadataApproachLeg
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataApproachLeg ()
		{
			PropInfoList = MetadataSegmentLeg.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyApproachLeg.Approach, (int) FeatureType.InstrumentApproachProcedure, PropertyTypeCharacter.Nullable);
		}
	}
}
