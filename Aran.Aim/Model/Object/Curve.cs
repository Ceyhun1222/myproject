using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Objects;
using Aran.Aim.DataTypes;

namespace Aran.Aim.Features
{
	public partial class Curve : AObject
	{
		public override ObjectType ObjectType
		{
			get { return ObjectType.Curve; }
		}
		
		public ValDistance HorizontalAccuracy
		{
			get { return (ValDistance ) GetValue ((int) PropertyCurve.HorizontalAccuracy); }
			set { SetValue ((int) PropertyCurve.HorizontalAccuracy, value); }
		}
		
	}
}
