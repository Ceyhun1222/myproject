using System;
using System.Collections.Generic;
using System.Text;
using ARAN.Common;

namespace ARAN.AIXMTypes
{
	public class DesignatedPoint : SignificanPoint
	{
		public DesignatedPoint()
			: base(AIXMType.DesignatedPoint)
		{
		}

		public override Object Clone()
		{
			DesignatedPoint cln = new DesignatedPoint();
			cln.Assign(this);
			return cln;
		}
	}
}
