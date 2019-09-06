using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aran.Geometries
{
	//public class InteriorRingList : // GeometryList<Ring>
	//{
	//    public InteriorRingList ( )
	//    { 
	//        _lengthCalculated = false;
	//        _areaCalculated = false;
	//    }

	//    public double Length
	//    {
	//        get
	//        {
	//            if ( !_lengthCalculated )
	//                CalculateLength ();
	//            return _length;
	//        }
	//    }

	//    public double Area
	//    {
	//        get
	//        {
	//            if ( !_areaCalculated )
	//            {
	//                CalculateArea ();
	//            }
	//            return _area;
	//        }
	//    }

	//    public override GeometryType GeometryType
	//    {
	//        get
	//        {
	//            return GeometryType.InteriorRingList;
	//        }
	//    }

	//    public override void Assign ( AranObject source )
	//    {
	//        InteriorRingList srcInteriorRngList = ( InteriorRingList ) source;
	//        Clear ();
	//        for ( int i = 0; i < srcInteriorRngList.Count; i++ )
	//        {
	//            Add ( srcInteriorRngList [i]);
	//        }
	//    }

	//    private void CalculateLength ( )
	//    {
	//        double result = 0;
	//        foreach ( Ring rng in this )
	//        {
	//            result += rng.Length;
	//        }
	//        _length = result;
	//        _lengthCalculated = true;
	//    }

	//    private void CalculateArea ( )
	//    {
	//        double result = 0;
	//        foreach ( Ring rng in this )
	//        {
	//            result += rng.SignedArea;
	//        }
	//        _area = result;
	//        _areaCalculated = true;
	//    }

	//    public override void Add ( Ring rng )
	//    {
	//        base.Add ( rng );
	//        this [Count-1].IsExterior = false;
	//        _lengthCalculated = false;
	//        _areaCalculated = false;
	//    }

	//    public override void Insert ( int index, Ring rng )
	//    {
	//        base.Insert ( index, rng );
	//        _lengthCalculated = false;
	//        _areaCalculated = false;
	//    }

	//    public override void Remove ( int index )
	//    {
	//        base.Remove ( index );
	//        _lengthCalculated = false;
	//        _areaCalculated = false;
	//    }

	//    public override void Clear ( )
	//    {
	//        base.Clear ();
	//        _lengthCalculated = false;
	//        _areaCalculated = false;
	//    }

	//    public override MultiPoint ToMultiPoint ( )
	//    {
	//        MultiPoint multiPoint = new MultiPoint ( );
	//        foreach ( Ring ring in this )
	//        {
	//            multiPoint.AddMultiPoint ( ring.ToMultiPoint ( ) );
	//        }
	//        return multiPoint;
	//    }

	//    private bool _lengthCalculated, _areaCalculated;
	//    private double _length, _area;
	//}
}
