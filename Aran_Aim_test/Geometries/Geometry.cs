using System;
using System.Collections;
using Aran.Package;

namespace Aran.Geometries
{
    public abstract class Geometry :
        AranObject,
        IPackable,
        IAranCloneable, IEnumerable
    {
        public Geometry()
        {
        }

        public static Geometry Create(GeometryType geomType)
        {
            Geometry src = null;
            switch (geomType)
            {
                case GeometryType.Null:
                    src = null;
                    break;
                case GeometryType.Point:
                    src = new Point();
                    break;
                case GeometryType.LineString:
                    src = new LineString();
                    break;
                case GeometryType.Polygon:
                    src = new Polygon();
                    break;
                case GeometryType.MultiPoint:
                    src = new MultiPoint();
                    break;
                case GeometryType.MultiLineString:
                    src = new MultiLineString();
                    break;
                case GeometryType.MultiPolygon:
                    src = new MultiPolygon();
                    break;
                case GeometryType.Ring:
                    src = new Ring();
                    break;
                case GeometryType.LineSegment:
                    src = new LineSegment();
                    break;
                //case GeometryType.InteriorRingList:
                //    src = new InteriorRingList ();
                //    break;
                default:
                    throw new NotImplementedException();
            }
            return src;
        }

        public static void PackGeometry(PackageWriter writer, Geometry geom)
        {
            writer.PutInt32((Int32)(geom.Type));
            ((IPackable)geom).Pack(writer);
        }

        public static Geometry UnpackGeometry(PackageReader reader)
        {
            GeometryType geomType = (GeometryType)reader.GetInt32();
            Geometry result;
            if (geomType == GeometryType.SpecialGeometry)
                result = SpecialGeometry.Create((SpecialGeometryType)reader.GetInt32());
            else
                result = Geometry.Create(geomType);
            ((IPackable)result).Unpack(reader);
            return result;
        }

        #region ISpecialGeometry Members

        internal virtual void PackSpecGeometry(PackageWriter writer, Geometry geom)
        {
            writer.PutInt32((Int32)(geom.Type));
            geom.Pack(writer);
        }

        internal virtual Geometry UnPackSpecGeometry(PackageReader reader)
        {
            return null;
        }

        #endregion

		//public static implicit operator MultiPoint(Geometry geom)
		//{
		//	return geom.ToMultiPoint();
		//}

        public abstract void Assign(AranObject source);
        public abstract AranObject Clone();
        public abstract void Pack(PackageWriter writer);
        public abstract void Unpack(PackageReader reader);

        public abstract MultiPoint ToMultiPoint();

        public abstract void SetConstantZ(double val);

        public abstract void SetConstantM(double val);

        public abstract void SetConstantT(double val);

        public abstract GeometryType Type
        {
            get;
        }
        public abstract bool IsEmpty
        {
            get;
        }
        public abstract Point Centroid
        {
            get;
        }

        public virtual IEnumerator GetEnumerator()
        {
            return null;
        }

        public static bool IsNullOrEmpty(Geometry geom)
        {
            return (geom == null || geom.IsEmpty);
        }

        public virtual Box Extend
        {
            get
            {
                if (_extend == null)
                    _extend = new Box();
                return _extend;
            }
        }

        protected Box _extend;
    }
}