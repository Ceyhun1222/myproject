using System;
using System.Collections.Generic;
using System.Text;
using ARAN.Common;

namespace ARAN.GeometryClasses
{
	public class PolyLine : GeometryList<Part>
	{
		public override Object Clone()
		{
			PolyLine pln;
			pln = new PolyLine();
			pln.Assign(this);
			return (object)pln;
		}

		public override GeometryType GetGeometryType()
		{
			return GeometryType.Polyline;
		}

		public void AddPart(Part part)
		{
			Add(part);
		}

		public void InsertPart(int index, Part part)
		{
			Insert(index, part);
		}

		public override Part this[int index]
		{
			get { return base[index]; }
		}

		protected Part GetPrototype()
		{
			Part pt = new Part();
			return pt;
		}

	}
}
