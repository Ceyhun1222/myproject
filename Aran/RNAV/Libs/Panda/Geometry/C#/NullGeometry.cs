using System;
using System.Collections.Generic;
using System.Text;
using ARAN.Common;

namespace ARAN.GeometryClasses
{
	public class NullGeometry : Geometry
	{

		public override void Pack(int handle)
		{
		}

		public override void UnPack(int handle)
		{
		}

		public override Object Clone()
		{
			return new NullGeometry();
		}

		public override void Assign(PandaItem source)
		{
		}

		public override GeometryType GetGeometryType()
		{
			return GeometryType.Null;
		}
	}
}
