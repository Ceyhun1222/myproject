using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aran.Package;

namespace Aran.Geometries
{
	public class LineSegment : Geometry
	{
		public LineSegment()
		{

		}

		public LineSegment(Point startPoint, Point endPoint)
		{
			Start = startPoint;
			End = endPoint;
		}

		public Point Start
		{
			get
			{
				return _start;
			}
			set
			{
				_start = value;
				_lengthCalculated = false;
			}
		}

		public Point End
		{
			get
			{
				return _end;
			}
			set
			{
				_end = value;
				_lengthCalculated = false;
			}
		}

		public double Length
		{
			get
			{
				if (!_lengthCalculated)
					CalculcateLength();
				return _length;
			}
		}

		public override GeometryType Type
		{
			get
			{
				return GeometryType.LineSegment;
			}
		}

		public void Reverse()
		{
			Point ptTmp = new Point(_start);
			_start.Assign(_end);
			_end.Assign(ptTmp);
		}

		public override bool IsEmpty
		{
			get
			{
				return _start == null || _start.IsEmpty || _end == null || _end.IsEmpty;
			}
		}

		private void CalculcateLength()
		{
			_length = Math.Sqrt((_end.X - _start.X) * (_end.X - _start.X) + (_end.Y - _start.Y) * (_end.Y - _start.Y));
			_lengthCalculated = true;
		}

		public override AranObject Clone()
		{
			return (this.MemberwiseClone() as LineSegment);
		}

		public override void Pack(PackageWriter writer)
		{
			Start.Pack(writer);
			End.Pack(writer);
		}

		public override void Unpack(PackageReader reader)
		{
			Start.Unpack(reader);
			End.Unpack(reader);
			_lengthCalculated = false;
		}

		public override void Assign(AranObject source)
		{
			LineSegment srcLineSeg = source as LineSegment;
			_lengthCalculated = srcLineSeg._lengthCalculated;
			_length = srcLineSeg.Length;

			Start.Assign(srcLineSeg.Start);
			End.Assign(srcLineSeg.End);
		}

		public override void SetConstantZ(double val)
		{
			if (_start != null)
				_start.Z = val;
			if (_end != null)
				_end.Z = val;
		}

		public override void SetConstantM(double val)
		{
			if (_start != null)
				_start.M = val;
			if (_end != null)
				_end.M = val;
		}

		public override void SetConstantT(double val)
		{
			if (_start != null)
				_start.T = val;
			if (_end != null)
				_end.T = val;
		}

		public override Point Centroid
		{
			get
			{
				return new Point(0.5 * (Start.X + End.X), 0.5 * (Start.Y + End.Y));
			}
		}

		public override MultiPoint ToMultiPoint()
		{
			MultiPoint multiPoint = new MultiPoint();
			multiPoint.Add(_start);
			multiPoint.Add(_end);

			return multiPoint;
		}

		private Point _start;
		private Point _end;
		private double _length;
		private bool _lengthCalculated;
	}
}
