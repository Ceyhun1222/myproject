
using System.Linq;
using Aran.Package;
using System.Collections;
using System.Collections.Generic;
namespace Aran.Geometries
{
	public class MultiPolygon : Geometry, IEnumerable // GeometryList<Polygon>
	{
		public MultiPolygon()
		{
			_polygonList = new List<Polygon>();
            _extend = new Box();
		}

		public void Add(Polygon polygon)
		{
			_polygonList.Add(polygon);

			_lengthCalculated = false;
			_areaCalculated = false;
            _extend.ChangeIfOutside(polygon);
		}

		public void Add(MultiPolygon polygon)
		{
			foreach (Polygon poly in polygon)
				_polygonList.Add(poly);

			_lengthCalculated = false;
			_areaCalculated = false;
			_extend.ChangeIfOutside(polygon);
		}

		public bool Insert(int index, Polygon polygon)
		{
			if (index > _polygonList.Count)
				return false;

			if (index == Count)
				_polygonList.Add(polygon);
			else
				_polygonList.Insert(index, polygon);

			_lengthCalculated = false;
			_areaCalculated = false;
            _extend.ChangeIfOutside(polygon);
			return true;
		}

		public bool Remove(int index)
		{
			if (index >= _polygonList.Count)
				return false;

			_polygonList.RemoveAt(index);

			_lengthCalculated = false;
			_areaCalculated = false;
            _extend.SetEmpty();
			return true;
		}

		public void Clear()
		{
			_polygonList.Clear();
			_length = _area = 0.0;
			_lengthCalculated = _areaCalculated = true;
            _extend.SetEmpty();
		}

		public bool IsPointInside(Point pnt)
		{
			foreach (Polygon plygon in this)
				if (plygon.IsPointInside(pnt))
					return true;
			return false;
		}

		private void CalculateLength()
		{
			double result = 0;
			foreach (Polygon polygon in this)
				result += polygon.Length;

			_length = result;
			_lengthCalculated = true;
		}

		private void CalculateArea()
		{
			double result = 0;
			foreach (Polygon polygon in this)
				result += polygon.Area;

			_area = result;
			_areaCalculated = true;
		}

		public override void SetConstantZ(double val)
		{
			foreach (Polygon polygon in _polygonList)
				polygon.SetConstantZ(val);
		}

		public override void SetConstantM(double val)
		{
			foreach (Polygon polygon in _polygonList)
				polygon.SetConstantM(val);
		}

		public override void SetConstantT(double val)
		{
			foreach (Polygon polygon in _polygonList)
				polygon.SetConstantT(val);
		}

		public override void Assign(AranObject source)
		{
			MultiPolygon sourceMltPolygon = (MultiPolygon)((Geometry)source).Clone();
			Clear();
			foreach (Polygon polygon in sourceMltPolygon)
				Add(polygon);
		}

		public override AranObject Clone()
		{
			MultiPolygon result = new MultiPolygon();
			foreach (Polygon polygon in _polygonList)
				result.Add(polygon);

			return result;
		}

		public override void Pack(PackageWriter writer)
		{
			writer.PutInt32(_polygonList.Count);
			foreach (Polygon polygon in _polygonList)
				polygon.Pack(writer);
		}

		public override void Unpack(PackageReader reader)
		{
			int count = reader.GetInt32();
			for (int i = 0; i < count; i++)
			{
				Polygon polygon = new Polygon();
				polygon.Unpack(reader);
				Add(polygon);
			}

			//base.Unpack ( reader );
			//_lengthCalculated = false;
			//_areaCalculated = false;
		}

		public override MultiPoint ToMultiPoint()
		{
			MultiPoint multiPoint = new MultiPoint();

			foreach (Polygon plg in this)
				multiPoint.AddMultiPoint(plg.ToMultiPoint());

			return multiPoint;
		}

		public override IEnumerator GetEnumerator()
		{
			return _polygonList.GetEnumerator();
		}
		//public new IEnumerator<Polygon> GetEnumerator()
		//{
		//	return _polygonList.GetEnumerator();
		//}


		public int Count
		{
			get
			{
				return _polygonList.Count;
			}
		}

		public double Area
		{
			get
			{
				if (!_areaCalculated)
					CalculateArea();

				return _area;
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

		public override GeometryType Type
		{
			get
			{
				return GeometryType.MultiPolygon;
			}
		}

		public override bool IsEmpty
		{
			get
			{
				foreach (Polygon item in _polygonList)
					if (!item.IsEmpty)
						return false;

				return true;
			}
		}

		public Polygon this[int index]
		{
			get
			{
				return _polygonList[index];
			}

			set
			{
				_polygonList[index] = value;
				_lengthCalculated = false;
				_areaCalculated = false;
			}
		}

		public override Point Centroid
		{
			get
			{
				return ToMultiPoint().Centroid;
			}
		}

		public bool IsClose
		{
			get
			{
				return this.Cast<Polygon>().All(poly => poly.IsClose);
			}
		}

		public void Close()
		{
			if (!IsClose)
			{
				foreach (Aran.Geometries.Polygon poly in this)
					poly.Close();
			}
		}

		private double _length;
		private bool _lengthCalculated = false;
		private double _area;
		private bool _areaCalculated = false;
		private List<Polygon> _polygonList;
	}
}
