using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Objects;
using Aran.Aim.DataTypes;

namespace Aran.Aim.Features
{
	public partial class Surface : AObject
	{
		public override ObjectType ObjectType
		{
			get { return ObjectType.Surface; }
		}
		
		public ValDistance HorizontalAccuracy
		{
			get { return (ValDistance ) GetValue ((int) PropertySurface.HorizontalAccuracy); }
			set { SetValue ((int) PropertySurface.HorizontalAccuracy, value); }
		}
		
	}
}
