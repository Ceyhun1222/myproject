using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using Aran.Aim.Features;
using Aran.Geometries;

namespace Aran.Queries
{
    public class PointInPolygon
    {
        #region PolygonClass
        internal enum MyRingType { Circle = 0, PointSeq = 1 };

        [StructLayout (LayoutKind.Sequential, Pack = 8)]
        internal struct MyPoint
        {
            public double x;
            public double y;
        }

        [StructLayout (LayoutKind.Sequential, Pack = 8)]
        internal struct MyPolygon
        {
            public int ringNum;
            public IntPtr Rings;
        };

        [StructLayout (LayoutKind.Explicit) ]
        internal struct MyRing
        {
            [FieldOffset(0)] public int ringType;

            [FieldOffset(8)] public MyPoint ptCenter;
            [FieldOffset(24)] public double radius;

            [FieldOffset(8)] public int pointNum;
            [FieldOffset(12)] public IntPtr Points;
        };
        #endregion

        [DllImport ("06.dll", EntryPoint = "#101", CallingConvention = CallingConvention.StdCall)]
        unsafe static extern bool IsPointInPoly (ref MyPoint testPoint, ref MyPolygon testPoly);

        public PointInPolygon (Polygon gmlPolygon)
        {
            if ( gmlPolygon == null )
                return;

            Dictionary<MyRing, List<MyPoint>> polygonDictionary = new Dictionary<MyRing, List<MyPoint>> ();
            _myPointArrayList = new List<MyPoint []> ();

			foreach (Ring surfacePatch in gmlPolygon)
			{
//				if (!(surfacePatch.IsExterior))
//					continue;

				MyRing myRing = new MyRing();
				myRing.ringType = (int)MyRingType.PointSeq;

				List<MyPoint> myPointList = new List<MyPoint>();

				foreach (Point gmlPoint in surfacePatch)
				{
					MyPoint myPoint = new MyPoint();
					myPoint.x = gmlPoint.X;
					myPoint.y = gmlPoint.Y;
					myPointList.Add(myPoint);
				}

				myRing.pointNum = myPointList.Count;
				polygonDictionary.Add(myRing, myPointList);
				_myPointArrayList.Add(myPointList.ToArray());

			}

            _myPolygon = new MyPolygon ();
            _myRingArray = new MyRing [polygonDictionary.Count];
            _myPolygon.ringNum = _myRingArray.Length;
            polygonDictionary.Keys.CopyTo (_myRingArray, 0);
            _checkPoint = new MyPoint ();
        }

        public bool IsInside (Point gmlPoint)
        {
            if (gmlPoint == null)
                return false;
            bool b = IsInside (gmlPoint.X, gmlPoint.Y);
            return b;
        }

        public bool IsInside (double x, double y)
        {
            if ( _myPointArrayList == null )
                return true;

            _checkPoint.x = x;
            _checkPoint.y = y;

            for (int i = 0; i < _myPointArrayList.Count; i++)
            {
                if (_myPointArrayList [i] != null && _myPointArrayList [i].Length > 0)
                {
                    unsafe
                    {
                        MyPoint [] myPointArr = _myPointArrayList [i];
                        fixed (MyPoint* myPointPtr = myPointArr)
                            _myRingArray [i].Points = new IntPtr ((void*) (&myPointPtr [0]));
                    }
                }
            }
            unsafe
            {
                if (_myRingArray.Length > 0)
                {
                    fixed (MyRing* myRingPtr = _myRingArray)
                        _myPolygon.Rings = new IntPtr ((void*) (&myRingPtr [0]));
                }
            }

            return IsPointInPoly (ref _checkPoint, ref _myPolygon);
        }

        private MyPolygon _myPolygon;
        private MyRing [] _myRingArray;
        private List<MyPoint []> _myPointArrayList;
        private MyPoint _checkPoint;
    }
}
