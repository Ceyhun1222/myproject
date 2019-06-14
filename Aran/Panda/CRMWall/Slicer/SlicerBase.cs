using System;
using System.Collections.Generic;
using Aran.Geometries;

namespace Aran.PANDA.CRMWall
{
	public abstract class SlicerBase
	{
		#region struct Span

		public struct Span
		{
			public double X0, X1, Y, value;
			public Point p0, p1;
			public int owner;

			public Span(double x0, double x1, double y, double value, int owner)
			{
				if (x0 > x1)
				{
					double fTmp = x1;
					x1 = x0;
					x0 = fTmp;
				}

				X0 = x0;
				X1 = x1;
				Y = y;
				this.value = value;
				p0 = new Point(x0, y);
				p1 = new Point(x1, y);
				this.owner = owner;
			}
		}

		#endregion

		#region struct Edge

		public struct Edge
		{
			public Point v0, v1;
			public double X, Xi, dXdY;
		}
		#endregion

		protected SpanTable dataTable;

		virtual public SpanTable Data { get { return dataTable; } }

		public SlicerBase()
		{
			dataTable = new SpanTable();
		}

		virtual protected void putdata(Span newData)
		{
			dataTable.Add(newData);
		}

		virtual protected void putdata(double x0, double x1, double y, double val, Point _p0, Point _p1)
		{
			Span newData = new Span()
			{
				X0 = x0,
				X1 = x1,
				Y = y,
				value = val,
				p0 = _p0,
				p1 = _p1
			};
			dataTable.Add(newData);
		}

		abstract public void SliceGeometry(Geometry geometry, Point ptO, double dir, double val, double step, int owner, bool accumulate = true);
		abstract public void SlicePolygon(MultiPolygon mPolygon, Point ptO, double dir, double val, double step, int owner, bool accumulate = true);
		abstract public void SlicePolyline(MultiLineString mPolyline, Point ptO, double dir, double val, double step, int owner, bool accumulate = true);
	}

}
