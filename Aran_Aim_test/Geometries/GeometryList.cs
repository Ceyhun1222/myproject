using System;
using System.Collections.Generic;
using System.Collections;
using Aran.Package;

namespace Aran.Geometries
{
	//public abstract class GeometryList<T> :
	//    Geometry,
	//    IEnumerable
	//    where T : Geometry, new ()
	//{
	//    public GeometryList ()
	//    {
	//        _list = new List<T> ();
	//    }

	//    public int Count
	//    {
	//        get
	//        {
	//            return _list.Count;
	//        }
	//    }

	//    public virtual T this [int index]
	//    {
	//        get
	//        {
	//            return _list [index];
	//        }
	//        set
	//        {
	//            _list [index] = value;
	//        }
	//    }

	//    public virtual void Insert (int index, T _item)
	//    {
	//        _list.Insert (index, _item as T);
	//    }

	//    public virtual void Remove (int index)
	//    {
	//        _list.RemoveAt (index);
	//    }

	//    public virtual void Clear ()
	//    {
	//        _list.Clear ();
	//    }

	//    public virtual void Reverse ()
	//    {
	//        _list.Reverse ();
	//    }

	//    public virtual void Add (T _item)
	//    {
	//        _list.Add (_item as T);
	//    }

	//    public override void Assign (AranObject source)
	//    {
	//        GeometryList<T> geomListOfT = (GeometryList<T>) source;
	//        Clear ();
	//        for (int i = 0; i < geomListOfT.Count; i++)
	//            Add (geomListOfT [i] as T);
	//    }

	//    public override AranObject Clone ()
	//    {
	//        Geometry result = null;
	//        switch (GeometryType)
	//        {
	//            case GeometryType.Null:
	//            case GeometryType.Point:
	//                break;
	//            case GeometryType.LineString:
	//                result = new LineString ();
	//                break;
	//            case GeometryType.Polygon:
	//                result = new Polygon ();
	//                break;
	//            case GeometryType.MultiPoint:
	//                result = new MultiPoint ();
	//                break;
	//            case GeometryType.MultiLineString:
	//                result = new MultiLineString ();
	//                break;
	//            case GeometryType.MultiPolygon:
	//                result = new MultiPolygon ();
	//                break;
	//            case GeometryType.Ring:
	//                result = new Ring ();
	//                break;
	//            case GeometryType.LineSegment:
	//                result = new LineSegment ();
	//                break;
	//            case GeometryType.InteriorRingList:
	//                result = new InteriorRingList ();
	//                break;
	//            default:
	//                throw new NotImplementedException ();
	//        }
	//        result.Assign (this);
	//        return result;
	//    }

	//    public T [] ToArray ()
	//    {
	//        return _list.ToArray ();
	//    }

	//    public abstract MultiPoint ToMultiPoint ( );

	//    public IEnumerator GetEnumerator ()
	//    {
	//        return _list.GetEnumerator ();
	//    }

	//    public override void Pack (PackageWriter writer)
	//    {
	//        writer.PutInt32 (_list.Count);
	//        for (int i = 0; i < _list.Count; i++)
	//        {
	//            _list [i].Pack (writer);
	//        }
	//    }

	//    public override void Unpack (PackageReader reader)
	//    {
	//        Clear ();
	//        T t;
	//        int count = 0;
	//        count = reader.GetInt32 ();

	//        for (int i = 0; i < count; i++)
	//        {
	//            t = new T ();
	//            t.Unpack (reader);
	//            Add (t);
	//        }
	//    }

	//    private List<T> _list;
	//}
}
