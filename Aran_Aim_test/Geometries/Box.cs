using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aran.Geometries;
using Aran.Package;

namespace Aran.Geometries
{
	public class Box : SpecialGeometry
	{
		public Box()
		{
            _list = new Point[2];
			_list[0] = new Point();
			_list[1] = new Point();
		}

        public Box(double xMin, double yMin, double xMax, double yMax)
        {
            if (xMin > xMax)
            {
                var d = xMin;
                xMin = xMax;
                xMax = d;
            }
            if (yMin > yMax)
            {
                var d = yMin;
                yMin = yMax;
                yMax = d;
            }

            _list = new Point[2];
            _list[0] = new Point(xMin, yMin);
            _list[1] = new Point(xMax, yMax);


        }

		public override void Assign(AranObject source)
		{
			Box srcBox = (Box)source;
			_list[0].Assign(srcBox[0]);
			_list[1].Assign(srcBox[1]);
		}

		public override void Pack(PackageWriter writer)
		{
			_list[0].Pack(writer);
			_list[1].Pack(writer);
		}

		public override void Unpack(PackageReader reader)
		{
			_list[0].Unpack(reader);
			_list[1].Unpack(reader);
		}

		public override AranObject Clone()
		{
			Box box = new Box();
			box.Assign(this);
			return box;
		}

		public override SpecialGeometryType SpecGeomType
		{
			get
			{
				return SpecialGeometryType.Box;
			}
		}

        public override bool IsEmpty { get { return _list[0].IsEmpty; } }

		public Point this[int index]
		{
			get
			{
				//if ( index > 1 )
				//    return null;
				return _list[index];
			}

			set
			{
				//if ( index < 2 )
				_list[index] = value;
			}
		}

		public override Point Centroid
		{
			get
			{
				return new Point(0.5 * (_list[0].X + _list[1].X), 0.5 * (_list[0].Y + _list[1].Y));
			}
		}

		public double XMin
		{
			get
			{
				return _list[0].X;
			}
			set
			{
				_list[0].X = value;
			}
		}

		public double YMin
		{
			get
			{
				return _list[0].Y;
			}
			set
			{
				_list[0].Y = value;
			}
		}

		public double XMax
		{
			get
			{
				return _list[1].X;
			}
			set
			{
				_list[1].X = value;
			}
		}

		public double YMax
		{
			get
			{
				return _list[1].Y;
			}
			set
			{
				_list[1].Y = value;
			}
		}

		public override void SetConstantZ(double val)
		{
			_list[0].Z = val;
			_list[1].Z = val;
		}

		public override void SetConstantM(double val)
		{
			_list[0].M = val;
			_list[1].M = val;
		}

		public override void SetConstantT(double val)
		{
			_list[0].T = val;
			_list[1].T = val;
		}
		
        public override MultiPoint ToMultiPoint()
		{
			MultiPoint result = new MultiPoint();
			result.Add(_list[0]);
			result.Add(_list[1]);
			return result;
		}

        public void SetEmpty()
        {
            _list[0].SetEmpty();
            _list[1].SetEmpty();
        }

        public void ChangeIfOutside(Geometry otherGeom)
        {
            var box = otherGeom.Extend;

            if (double.IsNaN(XMin) || XMin > box.XMin)
                XMin = box.XMin;

            if (double.IsNaN(XMax) || XMax < box.XMax)
                XMax = box.XMax;


            if (double.IsNaN(YMin) || YMin > box.YMin)
                YMin = box.YMin;

            if (double.IsNaN(YMax) || YMax < box.YMax)
                YMax = box.YMax;
        }

        public override Box Extend
        {
            get { return this; }
        }

        public bool IsIntersected(Box other)
        {
            if (other.XMax < XMin)
                return false;
            if (XMax < other.XMin)
                return false;
            if (other.YMax < YMin)
                return false;
            if (YMax < other.YMin)
                return false;

            return true;
        }


        private Point[] _list;
	}
}