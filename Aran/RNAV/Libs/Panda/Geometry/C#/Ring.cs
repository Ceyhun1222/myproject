using System;
using System.Collections.Generic;
using System.Text;
using ARAN.Common;

namespace ARAN.GeometryClasses
{
	public class Ring : MultiPoint
	{
		public override Object Clone()
		{
			Ring rn;
			rn = new Ring();
			rn.Assign(this);
			return rn;
		}
		public override GeometryType GetGeometryType()
		{
			return GeometryType.Ring;
		}
	}
}
