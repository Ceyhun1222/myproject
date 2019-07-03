using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Features;
using Aran.Aim.DataTypes;

namespace Aran.Aim.Features
{
	public class TaxiwayContamination : SurfaceContamination
	{
		public override ObjectType ObjectType
		{
			get { return ObjectType.TaxiwayContamination; }
		}
		
		public ValDistance ClearedWidth
		{
			get { return (ValDistance ) GetValue ((int) PropertyTaxiwayContamination.ClearedWidth); }
			set { SetValue ((int) PropertyTaxiwayContamination.ClearedWidth, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyTaxiwayContamination
	{
		ClearedWidth = PropertySurfaceContamination.NEXT_CLASS,
		NEXT_CLASS
	}
	
	public static class MetadataTaxiwayContamination
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataTaxiwayContamination ()
		{
			PropInfoList = MetadataSurfaceContamination.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyTaxiwayContamination.ClearedWidth, (int) DataType.ValDistance, PropertyTypeCharacter.Nullable);
		}
	}
}
