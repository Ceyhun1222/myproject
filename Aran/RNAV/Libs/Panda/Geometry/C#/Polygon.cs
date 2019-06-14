using System;
using System.Collections.Generic;
using System.Text;

namespace ARAN.GeometryClasses
{
	public class Polygon : GeometryList<Ring>
	{

		public override object Clone()
		{
			Polygon polygon = new Polygon();
			polygon.Assign(this);
			return polygon;
		}

		public override GeometryType GetGeometryType()
		{
			return GeometryType.Polygon;
		}

		public void AddRing(Ring ring)
		{
			Add(ring);
		}

		public void Insert(Ring ring)
		{
			Insert(ring);
		}

		public override Ring this[int index]
		{
			get
			{
				return base[index];
			}

		}

		protected Ring GetProtetType()
		{
			Ring ring = new Ring();
			return ring;
		}

		protected Ring GetrIng(int index)
		{
			return base[index];
		}

	}
}
