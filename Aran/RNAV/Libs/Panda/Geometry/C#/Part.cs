using System;
using System.Collections.Generic;
using System.Text;

namespace ARAN.GeometryClasses
{
	public class Part : MultiPoint
	{
		public override GeometryType GetGeometryType()
		{
			return GeometryType.Part;
		}

		public override object Clone()
		{
			Part prt = new Part();
			prt.Assign(this);
			return (object)prt;
		}
	}
}
