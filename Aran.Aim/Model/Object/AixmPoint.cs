using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Objects;
using Aran.Aim.DataTypes;

namespace Aran.Aim.Features
{
	public partial class AixmPoint : AObject
	{
		public override ObjectType ObjectType
		{
			get { return ObjectType.AixmPoint; }
		}
		
		public ValDistance HorizontalAccuracy
		{
			get { return (ValDistance ) GetValue ((int) PropertyAixmPoint.HorizontalAccuracy); }
			set { SetValue ((int) PropertyAixmPoint.HorizontalAccuracy, value); }
		}
		
	}
}
