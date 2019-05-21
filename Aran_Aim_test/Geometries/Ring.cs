using System;
using Aran.Package;


namespace Aran.Geometries
{
	public class Ring : MultiPoint
	{
		public Ring()
		{
			_isExterior = _areaCalculated = _lengthCalculated = false;
		}

		public override void Add(Point pnt)
		{
			base.Add(pnt);
			_lengthCalculated = _areaCalculated = false;
		}

		public override bool Insert(int index, Point pnt)
		{
			if (base.Insert(index, pnt))
			{
				_lengthCalculated = _areaCalculated = false;
				return true;
			}
			return false;
		}

		public override bool Remove(int index)
		{
			if (base.Remove(index))
			{
				_lengthCalculated = _areaCalculated = false;
				return true;
			}
			return false;
		}

		public override void Clear()
		{
			base.Clear();
			_length = _area = 0.0;
			_lengthCalculated = _areaCalculated = true;
		}

		public override void Reverse()
		{
			base.Reverse();

			if (_areaCalculated)
			{
				_area = -_area;
				_isExterior = (_area > 0);
			}
		}

		public override void AddMultiPoint(MultiPoint mltPnt)
		{
			base.AddMultiPoint(mltPnt);
			_lengthCalculated = _areaCalculated = false;
		}

		public override void AddReverse(MultiPoint mltPnt)
		{
			base.AddReverse(mltPnt);
			_lengthCalculated = _areaCalculated = false;
		}

		public override MultiPoint ToMultiPoint()
		{
			return this;
		}

		public override bool IsEmpty
		{
			get { return Length == 0.0 && _area == 0.0; }
		}

		public bool IsPointInside(Point p)
		{
			if (p.IsEmpty)
				return false;

			int count = Count;

			if (count > 1)
			{
				double dx = this[0].X - this[Count - 1].X;
				double dy = this[0].Y - this[Count - 1].Y;
				double dist = dx * dx + dy * dy;

				if (dist < epsilon)
					count--;
			}

			while (count > 0 && this[count - 1].IsEmpty)    //skip empty points
				count--;

			if (count < 3)
				return false;

			double x = p.X;
			double y = p.Y;

			double x1 = this[count - 1].X;		// Start with the last edge of p
			double y1 = this[count - 1].Y;

			int numOfCrossings = 0;				// Count of poly's edges crossed

			//  For each edge e of polygon p, see if the ray from (x, y) to (infinity, y)
			//  crosses e:

			for (int i = 0; i < count; i++)
			{
				if (this[i].IsEmpty)                //skip empty points
					continue;

				double x2 = this[i].X;
				double y2 = this[i].Y;

				// If y is between (y1, y2] (e's y-range),
				// and (x,y) is to the left of e, then
				//     the ray crosses e:

				if ((((y2 <= y) && (y < y1)) || ((y1 <= y) && (y < y2)))
				&& (x < x2 + (x1 - x2) * (y - y2) / (y1 - y2)))
					numOfCrossings++;

				x1 = x2;
				y1 = y2;
			}

			return (numOfCrossings & 1) == 1;
		}

		private void CalculateArea()
		{
			_area = 0.0;
			_areaCalculated = true;

			int count = this.Count;

			if (count > 1)
			{
				double dx = this[0].X - this[Count - 1].X;
				double dy = this[0].Y - this[Count - 1].Y;
				double dist = dx * dx + dy * dy;

				if (dist <= epsilon)
					count--;
			}

			if (count < 3)
				return;

			int start = 0;
			while (start < count && this[start].IsEmpty)
				start++;

			if(start >= count)
				return;

			Point pt0 = this[start];

			double result = 0.0;

			for (int  i = start + 1; i <= count; i++)
			{
				int j = i;
				if (i == count)
					j = start;

				if (this[j].IsEmpty)
					continue;

				result += (this[j].X - pt0.X) * (this[j].Y + pt0.Y);
				pt0 = this[j];
			}

			_area = 0.5 * result;
			_isExterior = _area >= 0;
		}

		public double SignedArea
		{
			get
			{
				if (!_areaCalculated)
					CalculateArea();

				return _area;
			}
		}

		public double Area
		{
			get
			{
				if (!_areaCalculated)
					CalculateArea();

				return Math.Abs(_area);
			}
		}

		public double Length
		{
			get
			{
				if (!_lengthCalculated)
					CalculateLength();

				return _length;
			}
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
				if (this[i].IsEmpty)		// skip empty points
					continue;

				double dx = this[i].X - pt0.X;
				double dy = this[i].Y - pt0.Y;
				pt0 = this[i];

				_length += Math.Sqrt(dx * dx + dy * dy);
			}
		}

		public bool IsExterior
		{
			get
			{
				if (!_areaCalculated)
					CalculateArea();

				return _isExterior;
			}

			set
			{
				if (!_areaCalculated)
					CalculateArea();

				if (value != _isExterior)
				{
					base.Reverse();
					_area = -_area;
					_isExterior = value;
				}
			}
		}

		public override GeometryType Type
		{
			get
			{
				return GeometryType.Ring;
			}
		}

		public override void Assign(AranObject source)
		{
			base.Assign(source);

			_area = ((Ring)source)._area;
			_length = ((Ring)source)._length;
			_isExterior = ((Ring)source)._isExterior;
			_areaCalculated = ((Ring)source)._areaCalculated;
			_lengthCalculated = ((Ring)source)._lengthCalculated;
		}

        public override AranObject Clone()
		{
			Ring rng = new Ring();
			rng.AddMultiPoint((MultiPoint)base.Clone());
			return rng;
		}

        public bool IsClose
        {
            get
            {
                if (this.Count < 3)
                    return false;

                if (!this[0].Equals2D(this[this.Count - 1], epsilon))
                    return false;

                return true;
            }
        }

        public void Close()
        {
            if (!IsClose)
                this.Add(this[0]);
        }

		public static explicit operator LineString(Ring rng)
		{
			LineString lnString = new LineString();

			foreach (Point item in rng)
				lnString.Add(item);

			return lnString;
		}

		private bool _isExterior;
		private double _area;
		private double _length;
		private bool _areaCalculated, _lengthCalculated;
	}
}
