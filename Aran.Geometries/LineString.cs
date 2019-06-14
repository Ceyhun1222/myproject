using System;
using Aran.Package;


namespace Aran.Geometries
{
	public class LineString : MultiPoint
	{
		public LineString()
		{
			_lengthCalculated = false;
		}

		public Point Start
		{
			get
			{
				if (_start == null && base.Count > 0)
					_start = this[0];

				return _start;
			}
			set
			{
				if (Count == 0)
					Add(value);
				else if (_start == null)
					Insert(0, value);
				else
					this[0] = value;

				_start = value;
				_lengthCalculated = false;
			}
		}

		public Point End
		{
			get
			{
				if (_end == null && Count > 1)
					_end = this[Count - 1];

				return _end;
			}

			set
			{
				if (_end != null)
					this[Count - 1] = _end;
				else
					Add(_end);

				_end = value;
				_lengthCalculated = false;
			}
		}

		public override void Add(Point pnt)
		{
			base.Add(pnt);
			if (Count == 0)
				_start = pnt;
			else
				_end = pnt;

			_lengthCalculated = false;
		}

		public override bool Insert(int index, Point pnt)
		{
			bool result = base.Insert(index, pnt);

			if (result)
				_lengthCalculated = false;

			_start = this[0];

			if (Count > 1)
				_end = this[Count - 1];
			else
				_end = this[0];

			return result;
		}

		public override bool Remove(int index)
		{
			bool result = base.Remove(index);

			if (result)
			{
				_lengthCalculated = false;
				if (Count > 0)
				{
					_start = this[0];
					_end = this[Count - 1];
				}
				else
				{
					_start = _end = null;

					_length = 0.0;
					_lengthCalculated = true;
				}
			}

			return result;
		}

		public override void Clear()
		{
			base.Clear();
			_start = null;
			_end = null;

			_length = 0.0;
			_lengthCalculated = true;
		}

		public override void Reverse()
		{
			base.Reverse();
			Point ptTmp = _start;
			_start = _end;
			_end = ptTmp;
		}

		public override void AddMultiPoint(MultiPoint mltPnt)
		{
			base.AddMultiPoint(mltPnt);
			_lengthCalculated = false;

			_start = this[0];
			if (Count > 1) _end = this[Count - 1];
			else _end = this[0];
		}

		public override void AddReverse(MultiPoint mltPnt)
		{
			base.AddReverse(mltPnt);
			_lengthCalculated = false;
			_start = this[0];
			if (Count > 1) _end = this[Count - 1];
			else _end = this[0];
		}

		public override MultiPoint ToMultiPoint()
		{
			return this;
		}

		public override bool IsEmpty
		{
			get { return Length == 0.0; }
		}

		public double CalculateLengthSquare()
		{
			double dx, dy;

			int start = 0;
			while (start < this.Count && this[start].IsEmpty)
				start++;

			if (start >= this.Count)
				return 0.0;

			Point pt0 = this[start];

			double result = 0.0;
			for (int i = start + 1; i < this.Count; i++)
			{
				if (this[i].IsEmpty)
					continue;

				dx = this[i].X - pt0.X;
				dy = this[i].Y - pt0.Y;
				pt0 = this[i];

				result += dx * dx + dy * dy;
			}

			return result;
		}

		private void CalculateLength()
		{
			_length = 0.0;
			_lengthCalculated = true;

			int start = 0;
			while (start < this.Count && this[start].IsEmpty)
				start++;

			if (start >= this.Count)
				return;

			Point pt0 = this[start];

			for (int i = start + 1; i < this.Count; i++)
			{
				if (this[i].IsEmpty)
					continue;

				double dx = this[i].X - pt0.X;
				double dy = this[i].Y - pt0.Y;
				pt0 = this[i];

				_length += Math.Sqrt(dx * dx + dy * dy);
			}
		}

		public override GeometryType Type
		{
			get
			{
				return GeometryType.LineString;
			}
		}

		public virtual double Length
		{
			get
			{
				if (!_lengthCalculated)
					CalculateLength();
				return _length;
			}
		}

		public override AranObject Clone()
		{
			LineString lnString = new LineString();
			lnString.AddMultiPoint((MultiPoint)base.Clone());
			return lnString;
		}

		//public static implicit operator Ring ( LineString lnString )
		//{
		//    return CastToRing ( lnString );
		//}

		public static explicit operator Ring(LineString lnString)
		{
			Ring rng = new Ring();
			rng.AddMultiPoint((MultiPoint)lnString.ToMultiPoint());

			return rng;
		}

		private double _length;
		private bool _lengthCalculated = false;
		private Point _start;
		private Point _end;
	}
}
