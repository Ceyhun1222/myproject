using System.Linq;
using System.Runtime.InteropServices;
using Aran.Package;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;

namespace Aran.Geometries
{
    public class Polygon : Geometry
    {
        public Polygon()
        {
            _exteriorRing = new Ring();
            _interiorRingList = new BindingList<Ring>();
            _interiorRingList.ListChanged += new ListChangedEventHandler(_interiorRingList_ListChanged);

            _area = _length = 0.0;
            _areaCalculated = _lengthCalculated = true;
        }

        public void RemoveInteriorRingAt(int index)
        {
            _interiorRingList.RemoveAt(index);
            //_areaCalculated = _lengthCalculated = false;

            //base.Remove ( index );
            //if ( base [ index ].IsExterior )
            //{
            //    _exteriorRing = null;
            //    _extRingIndex = -1;
            //    _hasExteriorRing = false;
            //}
            //else
            //{
            //    if ( index > _extRingIndex )
            //        _interiorRingList.Remove ( index - 1 );
            //    else
            //        _interiorRingList.Remove ( index );
            //}
        }

        public void Clear()
        {
            //_exteriorRing = null;
            //_ringList.Clear ( );
            //_interiorRingList.Clear ( );

            ExteriorRing.Clear();
            InteriorRingList.Clear();

            _area = _length = 0.0;
            _areaCalculated = _lengthCalculated = true;
        }

        private void CalculateLength()
        {
            double result = 0;
            foreach (Ring rng in _interiorRingList)
                result += rng.Length;

            _length = result;
            _lengthCalculated = true;
        }

        public bool IsPointInside(Point pnt)
        {
            if (ExteriorRing.IsPointInside(pnt))
            {
                for (int i = 0; i < _interiorRingList.Count; i++)
                    if (_interiorRingList[i].IsPointInside(pnt))
                        return false;

                return true;
            }
            return false;

            //if ( ExteriorRing.IsPointInside ( pnt ) )
            //{
            //    foreach ( Ring rng in _ringList )
            //        if ( rng.IsPointInside ( pnt ) )
            //            return false;
            //    return true;
            //}
            //return false;
        }

        private void CalculateArea()
        {
            //double result = 0;
            //foreach (Ring rng in _ringList)
            //{
            //    result += rng.SignedArea;
            //}
            //_area = result;
            //_areaCalculated = true;

            //Polygon tmp = new Polygon ( );
            double result = ExteriorRing.SignedArea;
            foreach (Ring rng in InteriorRingList)
                result += rng.SignedArea;

            _area = result;
            _areaCalculated = true;
        }

        public override void SetConstantZ(double val)
        {
            _exteriorRing.SetConstantZ(val);

            foreach (Ring rng in _interiorRingList)
                rng.SetConstantZ(val);
        }

        public override void SetConstantM(double val)
        {
            _exteriorRing.SetConstantM(val);

            foreach (Ring rng in _interiorRingList)
                rng.SetConstantM(val);
        }

        public override void SetConstantT(double val)
        {
            _exteriorRing.SetConstantT(val);

            foreach (Ring rng in _interiorRingList)
                rng.SetConstantT(val);
        }

        public override void Assign(AranObject source)
        {
            Polygon srcPolygon = (Polygon)source;

            _exteriorRing.Assign(srcPolygon._exteriorRing);
            _interiorRingList = new BindingList<Ring>(srcPolygon._interiorRingList);

            //_interiorRingList.Clear();
            //foreach (Ring rng in srcPolygon._interiorRingList)
            //	_interiorRingList.Add(rng);

            _area = srcPolygon._area;
            _length = srcPolygon._length;
            _areaCalculated = srcPolygon._areaCalculated;
            _lengthCalculated = srcPolygon._lengthCalculated;
        }

        public override AranObject Clone()
        {
            Polygon result = new Polygon();
            result.Assign(this);
            return result;

            //result.ExteriorRing = ExteriorRing;
            //result._hasExteriorRing = true;
            //result.InteriorRingList = InteriorRingList;
            //return result;
        }

        public override void Pack(PackageWriter writer)
        {
            if (IsEmpty)
                writer.PutInt32(0);
            else
            {
                writer.PutInt32(InteriorRingList.Count + 1);

                ExteriorRing.Pack(writer);

                foreach (Ring rng in InteriorRingList)
                {
                    rng.Pack(writer);
                }
            }

            //ExteriorRing.Pack(writer);
            //InteriorRingList.Pack(writer);
        }

        public override void Unpack(PackageReader reader)
        {
            int count = reader.GetInt32();
            if (count == 0)
                return;

            ExteriorRing.Unpack(reader);

            for (int i = 1; i < count; i++)
            {
                Ring rng = new Ring();
                rng.Unpack(reader);
                InteriorRingList.Add(rng);
            }

            //_lengthCalculated = false;
            //_areaCalculated = false;
            //_IsCentroidValid = false;
            //ExteriorRing.Unpack ( reader );
            //InteriorRingList.Unpack ( reader );
        }

        public override MultiPoint ToMultiPoint()
        {
            MultiPoint multiPoint = new MultiPoint();
            multiPoint.AddMultiPoint(ExteriorRing);

            foreach (Ring ring in InteriorRingList)
            {
                multiPoint.AddMultiPoint(ring);
            }
            return multiPoint;
        }

        private void _interiorRingList_ListChanged(object sender, ListChangedEventArgs e)
        {
            _lengthCalculated = false;
            _areaCalculated = false;
            _IsCentroidValid = false;
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

        public double Area
        {
            get
            {
                if (!_areaCalculated)
                    CalculateArea();

                return _area;
            }
        }

        public override GeometryType Type
        {
            get
            {
                return GeometryType.Polygon;
            }
        }

        public override bool IsEmpty
        {
            get
            {
                return _exteriorRing == null || _exteriorRing.IsEmpty;
            }
        }

        public Ring ExteriorRing
        {
            get
            {
                return _exteriorRing;
            }

            set
            {
				if (value == null)
					_exteriorRing.Clear();
				else
					_exteriorRing.Assign(value);

				_exteriorRing.IsExterior = true;
                _lengthCalculated = false;
                _areaCalculated = false;
                _IsCentroidValid = false;
            }
        }

        public override Point Centroid
        {
            get
            {
                if (!_IsCentroidValid)
                {
                    _Centroid = ToMultiPoint().Centroid;
                    _IsCentroidValid = true;
                }
                return _Centroid;
            }
        }

        public bool IsClose
        {
            get
            {
                if (!ExteriorRing.IsClose)
                    return false;

                return InteriorRingList.All(interiroRing => interiroRing.IsClose);
            }
        }

        public void Close()
        {
            if (!IsClose)
            {
                ExteriorRing.Close();
                foreach (var ring in InteriorRingList)
                    ring.Close();
            }
        }

        public override Box Extend
        {
            get { return ExteriorRing.Extend; }
        }

        /// <summary>
        /// Returns independent list which doesn't affect to polygon when change accurs on it
        /// </summary>
        public BindingList<Ring> InteriorRingList
        {
            get
            {
                return _interiorRingList;
            }
        }

        private double _length;
        private bool _lengthCalculated;
        private double _area;
        private bool _areaCalculated;
        Point _Centroid;
        bool _IsCentroidValid = false;
        private Ring _exteriorRing;
        private BindingList<Ring> _interiorRingList;
    }
}
