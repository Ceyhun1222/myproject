using Aran.Package;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Aran.Geometries
{
    public class MultiLineString : Geometry, IEnumerable<LineString>
	{
		public MultiLineString()
		{
			_lnStringList = new List<LineString>();
            _extend = new Box();
		}

		public void Add(LineString lnString)
		{
			_lnStringList.Add(lnString);
			_lengthCalculated = false;
            _extend.ChangeIfOutside(lnString.Extend);
		}

		public void Add(MultiLineString mltLineString)
		{
			foreach (LineString lnString in mltLineString)
			{
				_lnStringList.Add(lnString);
				_extend.ChangeIfOutside(mltLineString.Extend);
			}

			_lengthCalculated = false;
		}

		public bool Insert(int index, LineString lnString)
		{
			if (index > _lnStringList.Count)
				return false;

			if (index == _lnStringList.Count)
				_lnStringList.Add(lnString);
			else
				_lnStringList.Insert(index, lnString);

			_lengthCalculated = false;
            _extend.ChangeIfOutside(lnString.Extend);

			return true;
		}

		public bool Remove(int index)
		{
			if (index >= _lnStringList.Count)
				return false;

			_lnStringList.RemoveAt(index);
			_lengthCalculated = false;
            _extend.SetEmpty();
			return true;
		}

		public void Clear()
		{
			_lnStringList.Clear();
			_length = 0.0;
			_lengthCalculated = true;
            _extend.SetEmpty();
		}

		private void CalculateLength()
		{
			double result = 0.0;
			foreach (LineString lnString in this)
				result += lnString.Length;

			_length = result;
			_lengthCalculated = true;
		}

		public override MultiPoint ToMultiPoint()
		{
			MultiPoint multiPoint = new MultiPoint();
			foreach (LineString lineString in this)
				multiPoint.AddMultiPoint(lineString);

			return multiPoint;
		}

		public override void Assign(AranObject source)
		{
			MultiLineString sourceMltLineString = (MultiLineString)source;
			Clear();
			foreach (LineString item in sourceMltLineString)
				Add(item);
		}

		public override AranObject Clone()
		{
			MultiLineString result = new MultiLineString();
			foreach (var lnString in _lnStringList)
				result.Add(lnString);

			return result;
		}

		public override void Pack(PackageWriter writer)
		{
			writer.PutInt32(_lnStringList.Count);
			foreach (LineString lnString in _lnStringList)
				lnString.Pack(writer);
		}

		public override void Unpack(PackageReader reader)
		{
			int count = reader.GetInt32();
			for (int i = 0; i < count; i++)
			{
				LineString lnString = new LineString();
				lnString.Unpack(reader);
				Add(lnString);
			}

			//base.Unpack ( reader );
			//_lengthCalculated = false;
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

		public int Count
		{
			get
			{
				return _lnStringList.Count;
			}
		}

		public override GeometryType Type
		{
			get
			{
				return GeometryType.MultiLineString;
			}
		}

		public override void SetConstantZ(double val)
		{
			foreach (LineString lnString in _lnStringList)
				lnString.SetConstantZ(val);
		}

		public override void SetConstantM(double val)
		{
			foreach (LineString lnString in _lnStringList)
				lnString.SetConstantM(val);
		}

		public override void SetConstantT(double val)
		{
			foreach (LineString lnString in _lnStringList)
				lnString.SetConstantT(val);
		}

		public override Point Centroid
		{
			get
			{
				MultiPoint mltPnt = ToMultiPoint();

				double sumX = 0, sumY = 0;
				int count = 0;

				foreach (Point item in mltPnt)
				{
					if (item.IsEmpty)
						continue;

					sumX += item.X;
					sumY += item.Y;
					count++;
				}

				if (count == 0)
					return new Point();

				double invCount = 1.0 / count;
				return new Point(invCount * sumX, invCount * sumY);
			}
		}

		public Point HalfPoint
		{
			get
			{
				double halfLenght = 0.5 * Length, sum = 0.0;

				foreach (LineString lnString in _lnStringList)
				{
					double currSumLength = sum + lnString.Length;

					if (currSumLength >= halfLenght)
					{
						int start = 0;

						while (start < lnString.Count && lnString[start].IsEmpty)
							++start;

						if (start >= lnString.Count)
							continue;

						Point pt0 = lnString[start];

						for (int i = start + 1; i < lnString.Count; i++)
						{
							if (lnString[i].IsEmpty)
								continue;

							double dX = lnString[i].X - pt0.X;
							double dY = lnString[i].Y - pt0.Y;
							double segLenght = Math.Sqrt(dX * dX + dY * dY);

							pt0 = lnString[i];

							if (currSumLength + segLenght >= halfLenght)
							{
								double k;
								if (segLenght == 0.0)
									k = 0.0;
								else
									k = (halfLenght - sum) / segLenght;

								return new Point(pt0.X + k * dX, pt0.Y + k * dY);
							}
						}

						return pt0;
					}

					sum = currSumLength;
				}

				return new Point();
			}
		}

		public override bool IsEmpty
		{
			get
			{
				foreach (LineString item in _lnStringList)
				{
					if (!item.IsEmpty)
						return false;
				}

				return true;
			}
		}

		public new IEnumerator<LineString> GetEnumerator()
		{
			return _lnStringList.GetEnumerator();
		}

		public LineString this[int index]
		{
			get
			{
				return _lnStringList[index];
			}
			set
			{
				_lnStringList[index] = value;
				_lengthCalculated = false;
			}
		}

		protected const double epsilon = 0.0000001;
		private double _length;
		private bool _lengthCalculated = false;

		//private bool _IsCentroidValid = false;
		private List<LineString> _lnStringList;
	}
}
