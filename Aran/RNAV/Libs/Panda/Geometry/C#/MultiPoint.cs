using System;
using System.Collections.Generic;
using System.Text;
using ARAN.Common;
using ARAN.Contracts.Registry;
using System.Runtime.InteropServices;

namespace ARAN.GeometryClasses
{
	public class MultiPoint : GeometryList<Point>
	{
		public MultiPoint()
			: base()
		{

		}

		public override GeometryType GetGeometryType()
		{
			return GeometryType.MultiPoint;
		}

		protected Point GetPoint(int index)
		{
			return base[index];
		}

		public void AddPoint(Point point)
		{
			Add(point);
		}

		public void AddMultiPoint(MultiPoint multiPoint)
		{
			int i;
			for (i = 0; i <= multiPoint.Count - 1; i++)
				Add(multiPoint.GetPoint(i));
		}

		public void InsertPoint(int index, Point point)
		{
			Insert(index, point);
		}


		public override Object Clone()
		{
			MultiPoint mp = new MultiPoint();
			mp.Assign(this);
			return mp;
		}

		/* public override void Assign (PandaItem source)
		 {
			 int i;
			 MultiPoint dst;
			 dst = (MultiPoint)source;
			 dst.Clear();
			 for (i = 0; i <= GetCount - 1; i++)
				 dst.AddPoint(GetPoint(i));
		 }
		 */


		public override Point this[int index]
		{
			get
			{
				return GetPoint(index);
			}

		}
		
}

}
