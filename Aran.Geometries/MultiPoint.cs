using System.Collections;
using System.Collections.Generic;
using Aran.Package;

namespace Aran.Geometries
{
	public class MultiPoint : Geometry, IEnumerable, IEnumerable<Aran.Geometries.Point>
	{
		public MultiPoint()
		{
			_pointList = new List<Point>();
			_extend = new Box();
		}

		public virtual void Add(Point point)
		{
			_pointList.Add(point);
			_IsCentroidValid = false;
			_extend.ChangeIfOutside(point);
		}

		public virtual bool Insert(int index, Point point)
		{
			if (index > _pointList.Count || index < 0)
				return false;

			if (index == _pointList.Count)
				_pointList.Add(point);
			else
				_pointList.Insert(index, point);

			_IsCentroidValid = false;
			_extend.ChangeIfOutside(point);
			return true;
		}

		public virtual bool Remove(int index)
		{
			if (index >= _pointList.Count || index < 0)
				return false;
			_pointList.RemoveAt(index);
			_IsCentroidValid = false;
			_extend.SetEmpty();
			return true;
		}

		public virtual void Clear()
		{
			_pointList.Clear();
			_IsCentroidValid = false;
			_extend.SetEmpty();
		}

		public virtual void Reverse()
		{
			_pointList.Reverse();
		}

		public virtual void AddMultiPoint(MultiPoint mltPnt)
		{
			if (mltPnt == null)
				return;

			for (int i = 0; i < mltPnt.Count; i++)
				Add(mltPnt[i]);

			_IsCentroidValid = false;
		}

		public virtual void AddReverse(MultiPoint mltPnt)
		{
			for (int i = mltPnt.Count - 1; i >= 0; i--)
				Add(mltPnt[i]);

			_IsCentroidValid = false;
		}

		public override MultiPoint ToMultiPoint()
		{
			return this;
		}

		public override void Assign(AranObject source)
		{
			MultiPoint sourceMltPnt = (MultiPoint)((Geometry)source).ToMultiPoint().Clone();

			Clear();
			_IsCentroidValid = false;

			foreach (Point pnt in sourceMltPnt)
			{
				Add(pnt);
			}
		}

		public override AranObject Clone()
		{
			MultiPoint result = new MultiPoint();
			foreach (Point pnt in this)
				result.Add(pnt);

			return result;
		}

		public new IEnumerator GetEnumerator()
		{
			return _pointList.GetEnumerator();
		}

		public override void Pack(Package.PackageWriter writer)
		{
			writer.PutInt32(_pointList.Count);
			foreach (Point point in _pointList)
			{
				point.Pack(writer);
			}
		}

		public override void Unpack(Package.PackageReader reader)
		{
			int count = reader.GetInt32();
			Clear();
			_IsCentroidValid = false;

			for (int i = 0; i < count; i++)
			{
				Point point = new Point();
				point.Unpack(reader);
				Add(point);
			}
		}

		public Point this[int index]
		{
			get
			{
				return _pointList[index];
			}

			set
			{
				_pointList[index] = value;
				_IsCentroidValid = false;
			}
		}

		public int Count
		{
			get
			{
				return _pointList.Count;
			}
		}

		public override GeometryType Type
		{
			get
			{
				return GeometryType.MultiPoint;
			}
		}

		public override bool IsEmpty
		{
			get
			{
				foreach (Point pnt in _pointList)
					if (!pnt.IsEmpty)
						return false;

				return true;
			}
		}

		public override void SetConstantZ(double val)
		{
			for (int i = 0; i < _pointList.Count; i++)
				_pointList[i].Z = val;
		}

		public override void SetConstantM(double val)
		{
			for (int i = 0; i < _pointList.Count; i++)
				_pointList[i].M = val;
		}

		public override void SetConstantT(double val)
		{
			for (int i = 0; i < _pointList.Count; i++)
				_pointList[i].T = val;
		}

		public override Point Centroid
		{
			get
			{
				if (!_IsCentroidValid)
				{
					int count = _pointList.Count;

					if (this.Type == GeometryType.Ring && count > 1)
					{
						double dx = _pointList[0].X - _pointList[count - 1].X,
								dy = _pointList[0].Y - _pointList[count - 1].Y,
								dist = dx * dx + dy * dy;

						if (dist <= epsilon)
							count--;
					}

					int sumCnt = 0;
					double sumX = 0, sumY = 0;

					for (int i = 0; i < count; i++)
					{
						if (_pointList[i].IsEmpty)
							continue;

						sumX += _pointList[i].X;
						sumY += _pointList[i].Y;
						sumCnt++;
					}

					double X = double.NaN, Y = double.NaN;

					if (sumCnt > 0)
					{
						double k = 1.0 / sumCnt;
						X = k * sumX;
						Y = k * sumY;
					}

					_Centroid = new Point(X, Y);
					_IsCentroidValid = true;
				}

				return _Centroid;
			}
		}

		IEnumerator<Point> IEnumerable<Point>.GetEnumerator()
		{
			return _pointList.GetEnumerator();
		}

		protected const double epsilon = 0.0000001;

		Point _Centroid;
		bool _IsCentroidValid = false;
		private List<Point> _pointList;
	}
}
