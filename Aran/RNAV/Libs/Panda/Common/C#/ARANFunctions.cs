using System.ComponentModel;
using System.Threading;
using System.IO;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Xml;
using System;
using System.Collections;
using System.Diagnostics;
using ARAN.GeometryClasses;
using System.Collections.Generic;
//using System.Runtime.InteropServices;
using Microsoft.Win32;
using ARAN.Contracts.GeometryOperators;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace ARAN.Common
{
	public  static class ARANFunctions 
	{

		[StructLayout(LayoutKind.Explicit)]

		private struct IntDouble
		{
			[FieldOffset(0)]
			public Double AsDouble;
			[FieldOffset(0)]
			public Int32 AsInteger0;
			[FieldOffset(4)]
			public Int32 AsInteger1;
			[FieldOffset(0)]
			public Int64 AsInt64;
		}

		[StructLayout(LayoutKind.Explicit)]
		private struct IntSingle
		{
			[FieldOffset(0)]
			public Single AsSingle;
			[FieldOffset(0)]
			public int AsInteger;
		}
		
		
		
		
		public static void ShowError( string msg)
		{
			
			MessageBox.Show(msg,"",MessageBoxButtons.OK,MessageBoxIcon.Error);
		}
		
		
		/*
        procedure ShowError (exc: Exception);
        begin
	        if exc is EARANError then
		        MessageDlg (exc.Message, EARANError (exc).ErrorType, [mbOK], 0)
	        else
		        ShowError (exc.Message);
        end;

        function GeoOper(): TGeometryOperators;
        begin
	        result := _geoOper;
        end;
        */

		[DllImport("MathRNAV.dll", SetLastError = true, EntryPoint = "_ReturnGeodesicDistance@32",CallingConvention = CallingConvention.StdCall)]
		public static  extern   double ReturnGeodesicDistance(double X0,double Y0,double X1, double Y1) ;


		[DllImport("MathRNAV.dll", SetLastError = true, EntryPoint = "_PointAlongGeodesic@40",CallingConvention =CallingConvention.StdCall)]
		static extern long PointAlongGeodesic_MathFunctions(double X,double Y,double Dist,double Azimuth,out double resx,out double resy);
		[DllImport("MathRNAV.dll", EntryPoint = "_ReturnGeodesicAzimuth@40",CallingConvention = CallingConvention.StdCall)]
		static extern long ReturnGeodesicAzimuth_MathFunctions(double X0, double Y0, double X1, double Y1, out double DirectAzimuth,out double InverseAzimuth);

		[DllImport("HHCTRL.OCX",SetLastError = true)]
		static extern Int32 HtmlHelp(Int32 hwndCaller,string pszFile,UInt32 uCommand,byte dwData );	 //Yeniden baxilmalidi

		[DllImport("MathRNAV.dll",EntryPoint = "_InitAll@0")]
		static extern void InitAll ();

		public static long ReturnGeodesicAzimuth( Point Point0,Point Point1, out double DirectAzimuth,out double InverseAzimuth)
		{
		    long Result;
    		Result = ReturnGeodesicAzimuth_MathFunctions(Point0.X, Point0.Y, Point1.X, Point1.Y, out DirectAzimuth,out InverseAzimuth);
		    return Result;
	    }
		
		public static Point PointAlongGeodesic( Point point, double azimuth, double distance)
		{
			double	resX = 0, resY = 0;
			Point  Result;
		
			Result = new Point();
			
			PointAlongGeodesic_MathFunctions (point.X, point.Y, distance, azimuth,out resX, out resY);
			Result.SetCoords (resX, resY);
			return Result;
		}


		
		
		        /*
        function AztToDirection (pointGeo: Point; AzimuthInDeg: Double; geoSR, prjSR: TSpatialReference): Double;
        var
	        pointToGeo:	 	Point;
	        pointFromPrj:	Point;
	        pointToPrj:		Point;
        begin
	        pointToGeo := PointAlongGeodesic (pointGeo, AzimuthInDeg, 10.0);

	        pointFromPrj := nil;
	        pointToPrj := nil;

	        try
		        pointFromPrj := _geoOper.geoTransformations (pointGeo, geoSR, prjSR).AsPoint;
		        pointToPrj := _geoOper.geoTransformations (pointToGeo, geoSR, prjSR).AsPoint;
		        result := ReturnAngleAsRadian (pointFromPrj, pointToPrj);
	        finally
		        pointToGeo.Free;
		        pointFromPrj.Free;
		        pointToPrj.Free;
	        end;
        end;

        function DirToAzimuth (pointPrj: Point; DirInRadian: Double; prjSR, geoSR: TSpatialReference): Double;
        var
	        directAzumuth:	Double;
	        inverseAzumuth:	Double;
	        pointToPrj:		Point;
	        pointToGeo:		Point;
	        pointFromGeo:	Point;
        begin
	        pointFromGeo := nil;
	        pointToGeo := nil;
	        pointToPrj := PointAlongPlane (pointPrj, DirInRadian, 10.0);

	        try
		        pointFromGeo := _geoOper.geoTransformations (pointPrj, prjSR, geoSR).AsPoint;
		        pointToGeo := _geoOper.geoTransformations (pointToPrj, prjSR, geoSR).AsPoint;
		        ReturnGeodesicAzimuth_MathFunctions(pointFromGeo.X, pointFromGeo.Y, pointToGeo.X, pointToGeo.Y, directAzumuth, inverseAzumuth);
		        result := directAzumuth;
	        finally
		        pointFromGeo.Free;
		        pointToGeo.Free;
		        pointToPrj.Free;
	        end;
        end;
        */
		public static  double ReturnAngleAsDegree( Point pointFrom,Point pointTo)
		{
            return RadToDeg(System.Math.Atan2(pointTo.Y - pointFrom.Y, pointTo.X - pointFrom.X));
		    
		}
        public static double RadToDeg(double rad)
        {
            return rad * 180 / System.Math.PI;
        }
		
        public static double ReturnAngleAsRadian( Point pointFrom, Point pointTo)
		{
            return System.Math.Atan2(pointTo.Y - pointFrom.Y, pointTo.X - pointFrom.X);
		 
		}
		
		
		// ==============================================================================
		public static Point LocalToPrj( Point Center, double DirInRadian,double X,double Y)
		{
			double  Xnew, Ynew;
		    Point Result;
            double SinA = Math.Sin(DirInRadian);
            double CosA = Math.Cos(DirInRadian);
			Xnew = Center.X + X * CosA - Y * SinA;
			Ynew = Center.Y + X * SinA + Y * CosA;
			Result = new Point(Xnew, Ynew);
		    return Result;
		}
		
		public static Point LocalToPrj( Point Center, double DirInRadian, Point ptPrj)
		{
			double		Xnew, Ynew;
		    Point Result;
		
            double SinA = Math.Sin(DirInRadian);
            double CosA = Math.Cos(DirInRadian);
			Xnew = Center.X + ptPrj.X * CosA - ptPrj.Y * SinA;
			Ynew = Center.Y + ptPrj.X * SinA + ptPrj.Y * CosA;
			Result = new Point(Xnew, Ynew);
		    return Result;
		}
				
		public static void LocalToPrj( Point Center, double DirInRadian,double X,double Y, Point res)
		{
            double SinA = Math.Sin(DirInRadian);
            double CosA = Math.Cos(DirInRadian);
			res.X = Center.X + X * CosA - Y * SinA;
			res.Y = Center.Y + X * SinA + Y * CosA;
		}
		
		
		
		public static void LocalToPrj( Point Center, double DirInRadian, Point ptPrj, Point res)
		{
			double	X, Y;
            double SinA = Math.Sin(DirInRadian);
            double CosA = Math.Cos(DirInRadian);
			X = ptPrj.X;
			Y = ptPrj.Y;
			res.X = Center.X + X * CosA - Y * SinA;
			res.Y = Center.Y + X * SinA + Y * CosA;
		}
		
		
		// ==================================
		public static Point LocalToPrj( Line AxisLine, double X,double Y)
		{
		    double		SinA, CosA,Xnew, Ynew;
		    Point Result;
		
			AxisLine.DirVector.Length = 1.0;
			CosA = AxisLine.DirVector.GetComponent(0);
			SinA = AxisLine.DirVector.GetComponent(1);
		
			Xnew = AxisLine.RefPoint.X + X * CosA - Y * SinA;
			Ynew = AxisLine.RefPoint.Y + X * SinA + Y * CosA;
            Result = new Point(Xnew, Ynew);
		return Result;
		}
		
		
		
		public static Point LocalToPrj( Line AxisLine, Point ptPrj)
		{
            double SinA, CosA;
			double Xnew, Ynew;
		    Point Result;
		
			AxisLine.DirVector.Length = 1.0;
			CosA = AxisLine.DirVector.GetComponent(0);
			SinA = AxisLine.DirVector.GetComponent(1);
		
			Xnew = AxisLine.RefPoint.X + ptPrj.X * CosA - ptPrj.Y * SinA;
			Ynew = AxisLine.RefPoint.Y + ptPrj.X * SinA + ptPrj.Y * CosA;
			Result = new Point(Xnew,Ynew);
		    return Result;
		}
		
		
		
		public static void LocalToPrj( Line AxisLine, double X,double Y, ref Point res)
		{
		    double  SinA, CosA;
			AxisLine.DirVector.Length = 1.0;
			CosA = AxisLine.DirVector.GetComponent(0);
			SinA = AxisLine.DirVector.GetComponent(1);
		
			res.X = AxisLine.RefPoint.X + X * CosA - Y * SinA;
			res.Y = AxisLine.RefPoint.Y + X * SinA + Y * CosA;
		}
		
		public static void LocalToPrj( ref Line AxisLine, Point ptPrj, ref Point res)
		{
		    double		SinA, CosA;
			double			X, Y;
			AxisLine.DirVector.Length = 1.0;
			CosA = AxisLine.DirVector.GetComponent(0);
			SinA = AxisLine.DirVector.GetComponent(1);
			X = ptPrj.X;	Y = ptPrj.Y;
		
			res.X = AxisLine.RefPoint.X + X * CosA - Y * SinA;
			res.Y = AxisLine.RefPoint.Y + X * SinA + Y * CosA;
		}
		
		
		// ==============================================================================
		
		public static Point PrjToLocal( Point Center, double DirInRadian,double X,double Y)
		{
			 double		Xnew, Ynew;
			 double			dX, dY;
		     Point Result;
            
             double SinA = Math.Sin(DirInRadian);
             double CosA = Math.Cos(DirInRadian);
			 dX = X - Center.X;
			 dY = Y - Center.Y;
    		 Xnew = dX * CosA + dY * SinA;
			 Ynew = -dX * SinA + dY * CosA;
			 Result = new Point(Xnew, Ynew);
		    return Result;
		}
		
		public static void PrjToLocal( Point Center, double DirInRadian,double X,double Y, Point res)
		{
			double  dX, dY;
            double SinA = Math.Sin(DirInRadian);
            double CosA = Math.Cos(DirInRadian);
			dX = X - Center.X;
			dY = Y - Center.Y;
		
			res.X = dX * CosA + dY * SinA;
			res.Y = -dX * SinA + dY * CosA;
		}
		
		public static void PrjToLocal( Point Center, double DirInRadian,double X,double Y,ref double resX,ref double resY)
		{
			double  dX, dY;
            double SinA = Math.Sin(DirInRadian);
            double CosA = Math.Cos(DirInRadian);
            dX = X - Center.X;
			dY = Y - Center.Y;
		
			resX = dX * CosA + dY * SinA;
			resY = -dX * SinA + dY * CosA;
		}
		
		public static Point PrjToLocal( Point Center, double DirInRadian, Point ptPrj)
		{
			double		Xnew, Ynew;
			double			dX, dY;
            Point Result;
			double SinA = Math.Sin(DirInRadian);
			double CosA = Math.Cos(DirInRadian);
			dX = ptPrj.X - Center.X;
			dY = ptPrj.Y - Center.Y;
		
			Xnew = dX * CosA + dY * SinA;
			Ynew = -dX * SinA + dY * CosA;
			Result = new Point(Xnew, Ynew);
		return Result;
		}
		
		public static void PrjToLocal( Point Center, double DirInRadian, Point ptPrj, Point res)
		{
			double			dX, dY;
			double SinA = Math.Sin(DirInRadian);
			double CosA = Math.Cos(DirInRadian);
			dX = ptPrj.X - Center.X;
			dY = ptPrj.Y - Center.Y;
		
			res.X = dX * CosA + dY * SinA;
			res.Y = -dX * SinA + dY * CosA;
		}
					
		public static void PrjToLocal( Point Center, double DirInRadian, Point ptPrj, double resX,double resY)
		{
			double	dX, dY;
			double SinA = Math.Sin(DirInRadian);
			double CosA = Math.Cos(DirInRadian);
			dX = ptPrj.X - Center.X;
			dY = ptPrj.Y - Center.Y;
		
			resX = dX * CosA + dY * SinA;
			resY = -dX * SinA + dY * CosA;
		}
		
		// ==============================================================================
		public static Point PrjToLocal( Line AxisLine, double X,double Y)
		{
			double SinA, CosA;
			double Xnew, Ynew;
			double	dX, dY;
			Point Result;

			AxisLine.DirVector.Length = 1;
			CosA = AxisLine.DirVector.GetComponent(0);
			SinA = AxisLine.DirVector.GetComponent(1);
		
			dX = X - AxisLine.RefPoint.X;
			dY = Y - AxisLine.RefPoint.Y;
		
			Xnew = dX * CosA + dY * SinA;
			Ynew = -dX * SinA + dY * CosA;
			Result = new Point(Xnew, Ynew);
			
			return Result;
		}
		
		public static Point PrjToLocal( Line AxisLine, Point ptPrj)
		{
			double SinA, CosA;
			double Xnew, Ynew;
			double	dX, dY;
			Point Result;
		
			AxisLine.DirVector.Length = 1.0;
			CosA = AxisLine.DirVector.GetComponent(0);
			SinA = AxisLine.DirVector.GetComponent(1);
		
			dX = ptPrj.X - AxisLine.RefPoint.X;
			dY = ptPrj.Y - AxisLine.RefPoint.Y;
		
			Xnew = dX * CosA + dY * SinA;
			Ynew = -dX * SinA + dY * CosA;
			Result = new Point(Xnew, Ynew);
			return Result;
		}
		
		public static void PrjToLocal( ref Line AxisLine, double X,double Y, ref Point res)
		{
			double		SinA, CosA;
			double			dX, dY;
			AxisLine.DirVector.Length = 1.0;
			CosA = AxisLine.DirVector.GetComponent(0);
			SinA = AxisLine.DirVector.GetComponent(1);
		
			dX = X - AxisLine.RefPoint.X;
			dY = Y - AxisLine.RefPoint.Y;
		
			res.X = dX * CosA + dY * SinA;
			res.Y = -dX * SinA + dY * CosA;
		}
	
		public static void PrjToLocal( Line AxisLine, double X,double Y, double resX,double resY)
		{
			double	SinA, CosA;
			double	dX, dY;
			AxisLine.DirVector.Length = 1.0;
			CosA = AxisLine.DirVector.GetComponent(0);
			SinA = AxisLine.DirVector.GetComponent(1);
		
			dX = X - AxisLine.RefPoint.X;
			dY = Y - AxisLine.RefPoint.Y;
		
			resX = dX * CosA + dY * SinA;
			resY = -dX * SinA + dY * CosA;
		}
												
		public static void PrjToLocal( ref Line AxisLine, Point ptPrj, ref Point res)
		{
			double		SinA, CosA;
			double			dX, dY;
			AxisLine.DirVector.Length = 1.0;
			CosA = AxisLine.DirVector.GetComponent(0);
			SinA = AxisLine.DirVector.GetComponent(1);
		
			dX = ptPrj.X - AxisLine.RefPoint.X;
			dY = ptPrj.Y - AxisLine.RefPoint.Y;
		
			res.X = dX * CosA + dY * SinA;
			res.Y = -dX * SinA + dY * CosA;
		}
			
		public static void PrjToLocal( Line AxisLine, Point ptPrj, ref double resX,ref double resY)
		{
			double	SinA, CosA;
			double	dX, dY;
			AxisLine.DirVector.Length = 1.0;
			CosA = AxisLine.DirVector.GetComponent(0);
			SinA = AxisLine.DirVector.GetComponent(1);
		
			dX = ptPrj.X - AxisLine.RefPoint.X;
			dY = ptPrj.Y - AxisLine.RefPoint.Y;
		
			resX = dX * CosA + dY * SinA;
			resY = -dX * SinA + dY * CosA;
		}
			
				
		// ==============================================================================
		public static double Point2LineDistancePrj( Point point,Point poinLine, double DirInRadian)
		{
			double Result;
		
			double CosA = Math.Cos(DirInRadian);
			double SinA = Math.Sin(DirInRadian);
			Result = System.Math.Abs((point.Y - poinLine.Y) * CosA - (point.X - poinLine.X) * SinA);
			return Result;
		}
				
		public static double PointToLineDistance( Point RefPoint,Point LinePoint, double DirInRadian)
		{
			double Result;
			double CosA = Math.Cos(DirInRadian);
			double SinA = Math.Sin(DirInRadian);
			Result = (RefPoint.Y - LinePoint.Y) * CosA - (RefPoint.X - LinePoint.X) * SinA;
			return Result;
		}
				 				 		
		public static double PointToLineDistance( Point RefPoint, Line Line)
		{
			double		CosA, SinA;
			double Result;

			CosA = Line.DirVector.GetComponent(0);
			SinA = Line.DirVector.GetComponent(0);
		
			Result = (RefPoint.Y - Line.RefPoint.Y) * CosA - (RefPoint.X - Line.RefPoint.X) * SinA;
			return Result;
		}
		
		public static double PointToSegmentDistance( Point RefPoint,Point SegPoint1,Point SegPoint2)
		{
			double       dx, dy,
			dx0, dy0,
			dx1, dy1;
		// 	locX, locY,
		// 	fTmp:			Extended;
			 double   SegLen,
			// LineDist,
			Dist0, Dist1;
		/*	CosA, SinA:		Double;*/;
			double Result;
		
		
			dx = SegPoint2.X - SegPoint1.X;
			dy = SegPoint2.Y - SegPoint1.Y;
		
			dx0 = SegPoint1.X - RefPoint.X;
			dy0 = SegPoint1.X - RefPoint.X;
		
			dx1 = SegPoint2.X - RefPoint.X;
			dy1 = SegPoint2.X - RefPoint.X;

			SegLen = Hypot(dy, dx);
			Dist0 = Hypot(dy0, dx0);
			Dist1 = Hypot(dy1, dx1);
		
			if( SegLen < ARANMath.EpsilonDistance )
			{
				if (Dist0 < Dist1)
					return Dist0;
				else
					return Dist1;
			}
		
		/*	fTmp := 1.0/SegLen;
	CosA := dx * fTmp;
	SinA := dy * fTmp;
*/
		
			dx = RefPoint.X - SegPoint1.X;
			dy = RefPoint.Y - SegPoint1.Y;
		
		// 	locY := dX * CosA + dY * SinA;
		// 	locX := -dX * SinA + dY * CosA;
		
			Result = ((RefPoint.Y - SegPoint1.Y) * dx - (RefPoint.X - SegPoint1.X) * dy) / SegLen;
			return Result;
		}
		
		// ==============================================================================
		
		public static Point PointAlongPlane( Point ptOrigin, double DirRad,double Dist)
		{
			Point Result;
			double CosA = Math.Cos(DirRad);	  //I changed it
			double SinA = Math.Sin(DirRad);
			Result = new Point(ptOrigin.X + Dist * CosA, ptOrigin.Y + Dist * SinA);
			return Result;
		}
		
		public static Geometry LineLineIntersect( Point point1, double DirInRadian1, Point point2, double DirInRadian2)
		{
			const double Eps =  0.0001;
			double	d, Ua, Ub, k,
			cosF1, sinF1,
			cosF2, sinF2;
			Geometry Result;
		
			cosF1 = Math.Cos(DirInRadian1);
			sinF1 = Math.Sin(DirInRadian1);
		
			cosF2 = Math.Cos (DirInRadian2);
			sinF2 = Math.Sin (DirInRadian2);
		
			d = sinF2 * cosF1 - cosF2 * sinF1;
			Ua = cosF2 * (point1.Y - point2.Y) - sinF2 * (point1.X - point2.X);
			Ub = cosF1 * (point1.Y - point2.Y) - sinF1 * (point1.X - point2.X);
		
			if( System.Math.Abs(d) < Eps )
			{
				if( System.Math.Abs(Ua) + System.Math.Abs(Ub) < 2*Eps )
					Result = new Line(point1, DirInRadian1);
				else
					Result = new NullGeometry();
		
				return Result;
			}
		
			Result = new Point();
			k = Ua/d;
			Result.AsPoint.X = 2000234;
			Result.AsPoint.X = point1.X + k * cosF1;
			Result.AsPoint.Y = point1.Y + k * sinF1;
			return Result;
		}
		
		public static Geometry LineLineIntersect( Line Line1,Line Line2)
		{
			const double Eps =  0.0001;  double	d, Ua, Ub, k,
			cosF1, sinF1,
			cosF2, sinF2;
			Geometry Result;
		
			cosF1 = Line1.DirVector.GetComponent(0);
			sinF1 = Line1.DirVector.GetComponent(1);
		
			cosF2 = Line2.DirVector.GetComponent(0);
			sinF2 = Line2.DirVector.GetComponent(1);
		
			d = sinF2 * cosF1 - cosF2 * sinF1;
		
			Ua = cosF2 * (Line1.RefPoint.Y - Line2.RefPoint.Y) -
				  sinF2 * (Line1.RefPoint.X - Line2.RefPoint.X);
		
			Ub = cosF1 * (Line1.RefPoint.Y - Line2.RefPoint.Y) -
				  sinF1 * (Line1.RefPoint.X - Line2.RefPoint.X);
		
			if( Math.Abs(d) < Eps )
			{
				if( System.Math.Abs(Ua) + System.Math.Abs(Ub) < 2*Eps  )
					Result = (Geometry)Line1.Clone();
				else
					Result = new NullGeometry();
				return Result;
			}
		
			k = Ua/d;
			Result = new Point();
			Result.AsPoint.X = Line1.RefPoint.X + k * cosF1;
			Result.AsPoint.Y = Line1.RefPoint.Y + k * sinF1;
			return Result;
		}
		
		public static Point RingVectorIntersect( Ring Ring,  Line Line,  double d)
        {
            return RingVectorIntersect(Ring, Line, d, false);
        }
		
		public static Point RingVectorIntersect( Ring Ring, Line Line, double d, bool FindNearest)
		{
			int	I, J, N;
			double SinA, CosA;
			double dXE, dYE, X;
			double X0, Y0, X1, Y1;
			bool HaveIntersection;
			Point PE;
			Point Result;
		
			SinA = Line.DirVector.GetComponent(1);
			CosA = Line.DirVector.GetComponent(0);
			N = Ring.Count;
			Result = null;
			if( N < 2 ) return Result;

			PE = Ring[0]; ;
			X1 = (PE.X - Line.RefPoint.X) * CosA + (PE.Y - Line.RefPoint.Y) * SinA;
			Y1 = -(PE.X - Line.RefPoint.X) * SinA + (PE.Y - Line.RefPoint.Y) * CosA;
		
			HaveIntersection = false;
		
			for( I = 1;I <= N + 1;I++)
			{
				X0 = X1;
				Y0 = Y1;
		
				J = I & (0-Convert.ToInt32(I < N));
				PE = Ring[J];
				X1 = (PE.X - Line.RefPoint.X) * CosA + (PE.Y - Line.RefPoint.Y) * SinA;
				Y1 = -(PE.X - Line.RefPoint.X) * SinA + (PE.Y - Line.RefPoint.Y) * CosA;
		
				if( (Y0 * Y1 > 0) || ((X0 < 0)&&(X1 < 0)) )
					continue;
		
				dXE = X1 - X0;
				dYE = Y1 - Y0;

				if (System.Math.Abs(dYE) < ARANMath.EpsilonDistance) X = X0;
				else								X = X0 - Y0*dXE/dYE;
		
				if((! HaveIntersection)||(FindNearest && (X < d))||((! FindNearest) && (X > d)) )
					d = X;
		
				HaveIntersection = true;
			}
		
			if( HaveIntersection )
				Result = LocalToPrj(Line, d, 0);
			return Result;
		}
		
		public static Point RingVectorIntersect( Ring Ring,  Point ptVector,  double Direction, double d)
		{
			return RingVectorIntersect(Ring, ptVector, Direction, d, false); 
		}
		
		public static Point RingVectorIntersect( Ring ring, Point ptVector, double Direction, double d, bool FindNearest)
		{
			int	I, J, N;
			double	dXE, dYE, X;
			double X0, Y0, X1, Y1;
			bool HaveIntersection;
			Point PE; 
			Point Result;
			double	SinA = Math.Sin(Direction);
			double CosA = Math.Cos(Direction);
			Result = null;
			N = ring.Count;
			if( N < 2 ) return null;
		
			PE = ring[0];
			X1 = (PE.X - ptVector.X) * CosA + (PE.Y - ptVector.Y) * SinA;
			Y1 = -(PE.X - ptVector.X) * SinA + (PE.Y - ptVector.Y) * CosA;
		
			HaveIntersection = false;
		
			for( I = 1;I <= N;I++)
			{
				X0 = X1;
				Y0 = Y1;

                J = I < N ? I : 0;
		
				PE = ring[J];
		
				X1 = (PE.X - ptVector.X) * CosA + (PE.Y - ptVector.Y) * SinA;
				Y1 = -(PE.X - ptVector.X) * SinA + (PE.Y - ptVector.Y) * CosA;
		
				if( (Y0 * Y1 > 0) || ((X0 < 0)&&(X1 < 0)) )
					continue;
		
				dXE = X1 - X0;
				dYE = Y1 - Y0;
		
				if( System.Math.Abs(dYE) < ARANMath.EpsilonDistance )	X = X0;
				else								X = X0 - Y0 * dXE/dYE;
		
				if((! HaveIntersection) ||
					(FindNearest && (X < d))||
				((! FindNearest) && (X > d)) )
					d = X;
		
				HaveIntersection = true;
			}
		
			if( HaveIntersection )
				Result = LocalToPrj(ptVector, Direction, d, 0);
			return Result;
		}
		
		public static Point PolygonVectorIntersect( Polygon Polygon,  Line Line,  double d) {return PolygonVectorIntersect(Polygon, Line, d, false); }
		
		public static Point PolygonVectorIntersect( Polygon Polygon, Line Line, double d, bool FindNearest)
		{
			int	I, N;
			double	Dist = 0;
			Point ptTmp;
			Point Result;
		
			N = Polygon.Count;
			Result = null;
			if( N == 0 ) return Result;
		
			Result = RingVectorIntersect (Polygon[0], Line, d, FindNearest);
		
			for( I = 1;I <= N - 1;I++)
			{
				ptTmp = RingVectorIntersect (Polygon[I], Line, Dist, FindNearest);
		
				if( ptTmp == null )
				{
					if( ( Result!=null) || (FindNearest && (Dist< d)) || ((Dist > d) && (! FindNearest )) )
					{
						Result = ptTmp;
						d = Dist;
					}
				}
			}
			return Result;
		}

        public static Point PolygonVectorIntersect(Polygon Polygon, Point ptVector, double Direction, double d) { return PolygonVectorIntersect(Polygon, ptVector, Direction, d, false); }

		public static Point PolygonVectorIntersect( Polygon polygon, Point ptVector, double Direction, double d, bool FindNearest)
		{
			int	i, N;
			double	Dist = 0;
			Point ptTmp;
			Point Result;
		
			N = polygon.Count;
			Result = null;
			if( N == 0 )	return Result;
		
			Result = RingVectorIntersect (polygon[0], ptVector, Direction, d, FindNearest);
		
			for( i = 1;i <= N - 1;i++)
			{
				ptTmp = RingVectorIntersect (polygon[i], ptVector, Direction, Dist, FindNearest);
		
				if( ptTmp ==null )
				{
					if( ( Result !=null) || (FindNearest && (Dist< d)) || ((Dist > d) && (! FindNearest )) )
					{
						Result = ptTmp;
						d = Dist;
					}
				}
			}
		return Result;
		}
		
		public static int CuRingByNNLine( Ring ring, Part NNLine, Ring LefRing,Ring RighRing)
		{
			int	I, J, N, M,
			IxA, IxB;
		
			double		dA, dB = 0, dX, dY,
			dXE, dYE, X,
			Len, K,
			SinA, CosA, SinB, CosB,
			X0A, Y0A, X1A, Y1A,
			X0B, Y0B, X1B, Y1B;
			Point PE,pt0, pt1,ptCenter;
			int Result;
		
			N = ring.Count;
			Result = 0;
			LefRing = null;
			RighRing = null;
			dA = 0; // for remove compiler warning;
		
			if( N < 2 ) return Result;
		
			ptCenter = NNLine[1];
		
			dX = NNLine[0].X - ptCenter.X;
			dY = NNLine[0].Y - ptCenter.Y;
		
			Len = Math.Sqrt(dY *dY+dX*dX);	K = 1.0 / Len;
		
			SinA = dY*K;
			CosA = dX*K;
		
			dX = NNLine[2].X - ptCenter.X;
			dY = NNLine[2].Y - ptCenter.Y;
		
			Len = Math.Sqrt(dY*dY+dX*dX);	K = 1.0 / Len;
		
			SinB = dY*K;
			CosB = dX*K;
		
			PE = ring[0];
		
			X1A = (PE.X - ptCenter.X) * CosA + (PE.Y - ptCenter.Y) * SinA;
			Y1A = -(PE.X - ptCenter.X) * SinA + (PE.Y - ptCenter.Y) * CosA;
		
			X1B = (PE.X - ptCenter.X) * CosB + (PE.Y - ptCenter.Y) * SinB;
			Y1B = -(PE.X - ptCenter.X) * SinB + (PE.Y - ptCenter.Y) * CosB;
		
			IxA = -1;	IxB = -1;
		
			for( I = 0;I <= N;I++)
			{
				X0A = X1A;
				Y0A = Y1A;
		
				X0B = X1B;
				Y0B = Y1B;
		
				J = (I + 1) & (0 - Convert.ToInt32(I + 1 < N));
				PE = ring[J];
		
				X1A = (PE.X - ptCenter.X) * CosA + (PE.Y - ptCenter.Y) * SinA;
				Y1A = -(PE.X - ptCenter.X) * SinA + (PE.Y - ptCenter.Y) * CosA;
		
				X1B = (PE.X - ptCenter.X) * CosB + (PE.Y - ptCenter.Y) * SinB;
				Y1B = -(PE.X - ptCenter.X) * SinB + (PE.Y - ptCenter.Y) * CosB;
		
				if( (Y0A * Y1A < 0) && ((X0A > 0)||(X1A > 0)) )
				{
					dXE = X1A - X0A;
					dYE = Y1A - Y0A;
		
					if( System.Math.Abs(dYE) < ARANMath.EpsilonDistance )	X = X0A;
					else								X = X0A - Y0A*dXE/dYE;
		
					if((IxA < 0)||(X < dA) )
					{
						dA = X;
						IxA = I;
					}
				}
		
				if( (Y0B * Y1B < 0) && ((X0B > 0)||(X1B > 0)) )
				{
					dXE = X1B - X0B;
					dYE = Y1B - Y0B;
		
					if( System.Math.Abs(dYE) < ARANMath.EpsilonDistance )	X = X0B;
					else								X = X0B - Y0B*dXE/dYE;
		
					if((IxB < 0)||(X < dB) )
					{
						dB = X;
						IxB = I;
					}
				}
			}
		
			if( (IxA >= 0)&& (IxB >= 0))
			{
				X0A = ptCenter.X + dA * CosA;
				Y0A = ptCenter.Y + dA * SinA;
		
				X0B = ptCenter.X + dB * CosB;
				Y0B = ptCenter.Y + dB * SinB;
		
				if( IxA  == IxB )
				{
					Result = 2;
					return Result;
				}
		
				LefRing = new Ring();
				RighRing = new Ring();
		
				pt0 = new Point(X0A, Y0A);
				pt1 = new Point(X0B, Y0B);
				M = (IxA - IxB) % N;
				if( M <= 0 ) M = M + N;
		
				RighRing.AddPoint(pt0);
				RighRing.AddPoint(ptCenter);
				RighRing.AddPoint(pt1);
		
				for( I = 1;I <= M;I++)
				{
					J = (I + IxB) % N;
					RighRing.AddPoint(ring[J]);
				}
		
				M = N - M;
		
		// 		M := (IxB - IxA) Mod N;
		// 		if M < 0 then Inc(M, N);
		
				LefRing.AddPoint(pt1);
				LefRing.AddPoint(ptCenter);
				LefRing.AddPoint(pt0);
		
				for( I = 1;I <= M;I++)
				{
					J = (I + IxA) % N;
					LefRing.AddPoint(ring[J]);
				}
				Result = 2;
			}
			return Result;
		}
					
		public static  double ReturnDistanceAsMeter( Point pointFrom,Point pointTo)
		{
			double Result;
			Result = Hypot(pointTo.X - pointFrom.X, pointTo.Y - pointFrom.Y); 
			return Result;
		}
		
		public static SideDirection SideDef( Point pointOnLine, double lineAngleInRadian, Point testPoint)
		{
			double	Angle12, rAngle;
			SideDirection Result;
		
			Angle12 = ReturnAngleAsRadian (pointOnLine, testPoint);
			rAngle = ARANMath.Modulus (lineAngleInRadian - Angle12, 2 * Math.PI);

			double fdY = pointOnLine.Y - testPoint.Y;
			double fdX = pointOnLine.X - testPoint.X;
		    double fDist = fdY * fdY + fdX * fdX;
			if (fDist <  ARANMath.Epsilon_2Distance) 
				return SideDirection.sideOn;
			if ((System.Math.Abs(rAngle) < ARANMath.EpsilonRadian) || (System.Math.Abs(rAngle - Math.PI) < ARANMath.EpsilonRadian))
				Result = SideDirection.sideOn;
			else if( rAngle < Math.PI )
				Result = SideDirection.sideRight;
			else
				Result = SideDirection.sideLeft;
			return Result;
		}

		public static double SpiralTouchAngle(double r0, double coef0, double nominalDir, double touchDir, SideDirection turnDir)
		{
			int I;
			 double	turnAngle,
			TouchAngle,
			d, delta, coef;
			double Result;
		
            TouchAngle = ARANMath.Modulus ( ( nominalDir - touchDir - ARANMath.C_PI_2*( int ) turnDir ) * ( int ) ( turnDir ), 2*Math.PI );
			turnAngle = TouchAngle;
			coef = RadToDeg(coef0);
		
			for( I = 0;I <= 9;I++)
			{
				d = coef / (r0 + coef * turnAngle);
				delta = (turnAngle - TouchAngle - Math.Atan(d)) / (2 - 1 / (d * d + 1));
				turnAngle = turnAngle - delta;
				if( (System.Math.Abs(delta) < ARANMath.EpsilonRadian) )	break;
			}
			Result = turnAngle;
			if( Result < 0.0 )
                Result = ARANMath.Modulus ( Result, 2*Math.PI );
			return Result;
		}
				
		public static MultiPoint CreateWindSpiral( Point Pt, double radianNominalDir,double radianStartDir,double radianEndDir,double startRadius,double coefficient, SideDirection turnSide)
		{
			//			Need to Review   this function espcially turndir
			
			int	I, N, TurnDir;
			double	dAlpha, azt0,
			startDphi,
			endDphi,
			TurnAng, R;
			Point PtCnt, ptCur; 
			MultiPoint Result;
		
			TurnDir = (int) (turnSide);
			Result = new MultiPoint();
		
			PtCnt = PointAlongPlane (Pt, radianNominalDir + 0.5*Math.PI*TurnDir, startRadius);   //??????????
		
			if( ARANMath.SubtractAngles (radianNominalDir, radianEndDir) < ARANMath.EpsilonRadian )
				radianEndDir = radianNominalDir;
		
			startDphi = ARANMath.Modulus((radianStartDir - radianNominalDir) * TurnDir, 2*Math.PI);

			if (startDphi < ARANMath.EpsilonRadian) startDphi = 0;
			else								
				startDphi = SpiralTouchAngle (startRadius, coefficient,
														radianNominalDir, radianStartDir, turnSide);
		
			endDphi = SpiralTouchAngle (startRadius, coefficient, radianNominalDir, radianEndDir, turnSide);
			TurnAng = endDphi - startDphi;
			azt0 = ARANMath.Modulus(radianNominalDir - (startDphi - 0.5 * Math.PI) * TurnDir, 2 * Math.PI);
		
			if( TurnAng >= 0 )
			{
				N = (int)Math.Floor(RadToDeg(TurnAng));
		
				if( N == 0 ) 
					N = 1;
				else if( N < 10 )
					N = 10;
		
				dAlpha = TurnAng / N;
		
				for( I = 0;I <= N;I++)
				{
					R = startRadius + (RadToDeg (dAlpha) * coefficient * I) + RadToDeg (startDphi) * coefficient;
					ptCur = PointAlongPlane (PtCnt, azt0 - I * dAlpha * TurnDir, R);
					Result.AddPoint (ptCur);
				}
			}
			return Result;
		}

        public static bool IsPointInPoly(Point point, Polygon polygon)
        {
            int I, J,  N;
            bool C;
            double  Eps;
            Point P0, P1;
            Ring CRing;
            Double dX;
            Double dY;


            C = false;
            Eps = ARANMath.EpsilonDistance * ARANMath.EpsilonDistance;

            for (J = 0; J <= polygon.Count - 1; J++)
            {
                CRing = polygon[J];
                N = CRing.Count;
                if (N < 3) continue;

                P0 = CRing[N-1];
                P1 = CRing[0];

                dX = P1.X - P0.X;
                dY = P1.Y - P0.Y;

                if( dX *dX + dY*dY < Eps)
                    N--;
                if( N < 3 )	continue;


                int  j;
                for (I = 0, j = N - 1; I < N; j = I++)
                {
                    if (((CRing[I].Y > point.Y) != (CRing[j].Y > point.Y)) &&
                         (point.X < (CRing[j].X - CRing[I].X) * (point.Y - CRing[I].Y) / (CRing[j].Y - CRing[I].Y) + CRing[I].X))
                        C = !C;
                }

                if(C)  break;
            }

            return C;
        }
		
		/*
function PointToRingDistanceF(Point: Point; Ring: Ring): Double;
var
	I, Ix, J,
	N, M, Mn:		Integer;
	distMin:		Double;
	dX, dY, dXY,
	dXMin, dYMin,
	dXYMin, Len, K,
	dXp, dYp,
	X, d0, Eps:		Double;
	PtTmp:			Point;
	Pt0, Pt1:		Point;
	IArray:			Array [0..2] of Integer;
	IxArray:		Array [0..2] of Integer;
begin
	N := Ring.Count;
	Eps := EpsilonDistance*EpsilonDistance;

	IArray[0] := 0;
	IArray[1] := 0;
	IArray[2] := 0;

	PtTmp := Ring[0];
	dXMin := Abs(PtTmp.X - Point.X);
	dYMin := Abs(PtTmp.Y - Point.Y);
	dXYMin := dXMin + dYMin;
	distMin := Hypot(dXMin, dYMin);

	for I := 1 to N-1 do
	begin
		PtTmp := Ring[I];
		dX := Abs(PtTmp.X - Point.X);
		dY := Abs(PtTmp.Y - Point.Y);
		dXY := dX + dY;

		if dX < dXMin then
		begin
			dXMin := dX;
			IArray[0] := I;
		end;

		if dY < dYMin then
		begin
			dYMin := dY;
			IArray[1] := I;
		end;

		if dXY < dXYMin then
		begin
			dXYMin := dXY;
			IArray[2] := I;
		end;
	end;

	Mn := 2;
	I := 0;
	while I < Mn -1 do
	begin
		J := I + 1;
		while J < Mn do
		begin
			if IArray[I] = IArray[J] then
			begin
				if J < 2 then
					IArray[J] := IArray[J+1];
				Dec(Mn);
			end
			else
				Inc(J);
		end;
		Inc(I);
	end;

	for M := 0 to Mn do
	begin
		IxArray[1] := IArray[M];

		I := IxArray[1];
		J := (I + N - 1) mod N;

		Pt0 := Ring[I];
		Pt1 := Ring[J];

		dX := Pt0.X - Pt1.X;
		dY := Pt0.Y - Pt1.Y;
		Len := dY*dY + dX*dX;

		while Len < Eps do
		begin
			J := (J + N - 1) mod N;
			Pt1 := Ring[J];
			dX := Pt0.X - Pt1.X;
			dY := Pt0.Y - Pt1.Y;
			Len := dY*dY + dX*dX;
		end;
		IxArray[0] := J;

		J := (I + 1) mod N;
		Pt1 := Ring[J];
		dX := Pt0.X - Pt1.X;
		dY := Pt0.Y - Pt1.Y;
		Len := dY*dY + dX*dX;

		while Len < Eps do
		begin
			J := (J + 1) mod N;
			Pt1 := Ring[J];
			dX := Pt0.X - Pt1.X;
			dY := Pt0.Y - Pt1.Y;
			Len := dY*dY + dX*dX;
		end;
		IxArray[2] := J;

		for Ix := 0 to 1 do
		begin
			I := IxArray[Ix];
			J := IxArray[Ix + 1];

			Pt0 := Ring[I];
			Pt1 := Ring[J];

			dX := Pt1.X - Pt0.X;
			dY := Pt1.Y - Pt0.Y;

			Len := Hypot(dY, dX);
			K := 1/Len;

			dXp := Point.X - Pt0.X;
			dYp := Point.Y - Pt0.Y;

			X := K*(dXp * dX + dYp * dY);

			if X < 0 then
			begin
				d0 := Hypot(dYp, dXp);
				if d0 < distMin then distMin := d0;
			end
			else if X > Len then
			begin
				dX := Pt1.X - Point.X;
				dY := Pt1.Y - Point.Y;
				d0 := Hypot(dY, dX);
				if d0 < distMin then distMin := d0;
			end
			else
			begin
				d0 := Abs(K*(-dXp * dY + dYp * dX));
				if d0 < distMin then distMin := d0;
			end
		end;
	end;

	result := distMin;
end;

function PointToRingDistanceOld(Point: Point; Ring: Ring): Double;
var
	P0, P1:			Point;
	I, J, N:		Integer;
	distMin, Eps,
	Len2, dist,
	dX, dY, X0, X1,
	dX0, dY0,
	dX1, dY1:		Double;
	d0:				Double;
begin
	N := Ring.Count;
	J := N - 1;
	Eps := EpsilonDistance*EpsilonDistance;

	P0 := Ring[J];
	dX0 := P0.X - Point.X;
	dY0 := P0.Y - Point.Y;
	distMin := Hypot(dY0, dX0);

	repeat
		J := (J + 1) and (0 - Integer(J + 1 < N));
		if J = N - 1 then
		begin
			result := distMin;
			exit;
		end;
		P1 := Ring[J];
		dX := P1.X - P0.X;
		dY := P1.Y - P0.Y;
		Len2 := dX*dX + dY*dY
	until Len2 >= Eps;

	dX1 := P1.X - Point.X;
	dY1 := P1.Y - Point.Y;
	X1 := dX1 * dX + dY1 * dY;

	I := J;
	while I < N do
	begin
		P0 := P1;
		repeat
			J := (J + 1) and (0 - Integer(J + 1 < N));
			if J = I then
			begin
				result := distMin;
				exit;
			end;
			P1 := Ring[J];
			dX := P1.X - P0.X;
			dY := P1.Y - P0.Y;
			Len2 := dX*dX + dY*dY
		until Len2 >= Eps;

		dX0 := dX1;
		dY0 := dY1;
		d0 := X1;

		dX1 := P1.X - Point.X;
		dY1 := P1.Y - Point.Y;

		X0 := dX0 * dX + dY0 * dY;
		X1 := dX1 * dX + dY1 * dY;

		if X0 * X1 <= 0 then
		begin
			dist := Abs(dX0 * dY - dY0 * dX)/Math.Sqrt(Len2);
			if distMin > dist then distMin := dist;
		end
		else if(d0 > 0)and(X0 < 0)then
		begin
			dist := Hypot(dY0, dX0);
			if distMin > dist then distMin := dist;
		end;

		if J < I then	break;
		I := J;
	end;
	result := distMin;
end;
*/
		
		public static double CalcArea( Ring Ring)
		{
			int	I, N = 0;
			double	S = 0;
			double Result;

			N = Ring.Count;
			Result = 0;
						   		
			if( N < 3 )	return Result;
		
			S = Ring[N-1].X * Ring[0].Y - Ring[0].X * Ring[N - 1].Y;
		
			for( I = 0;I <= Ring.Count - 2;I++)
				S = S + Ring[I].X * Ring[I + 1].Y - Ring[I + 1].X * Ring[I].Y;
		
			Result = S;
			return Result;
		}

		public static double CalcArea(Polygon Polygon)
		{
			int	I;
			double Result;
			Result = 0;
			for( I = 0;I <=Polygon.Count - 1;I++)
				Result = Result + CalcArea(Polygon[I]);
			return Result;
		}

		public static double CalcAbsArea(Ring Ring)
		{
			int	I, N;
			double	S = 0;
			double Result;
			
			Result = 0;
			N = Ring.Count;
		
			if( N < 3 )	return Result;
		
			S = Ring[N-1].X * Ring[0].Y - Ring[0].X * Ring[N - 1].Y;
		
			for( I = 0;I <= Ring.Count - 2;I++)
				S = S + Ring[I].X * Ring[I + 1].Y - Ring[I + 1].X * Ring[I].Y;
		
			Result = System.Math.Abs(S);
			return Result;
		}

		public static double CalcAbsArea(Polygon Polygon)
		{
			int	I;
			double Result;
		
			Result = 0;
			for( I = 0;I <= Polygon.Count - 1;I++)
				Result = Result + CalcAbsArea(Polygon[I]);
			return Result;
		}

		public static double PointToRingDistance(Point Point, Ring Ring)
		{
			int		I, J, N;
			double		Len2,
			distMin,
			X0, X, Eps,
			dXp, dYp,
			dX, dY, d0;
			Point P0, P1;
			 
			double Result;
		
			Eps = Math.Pow(ARANMath.EpsilonDistance,2);
		
			N = Ring.Count;
			I = N - 1;
		
			P0 = Ring[I];
			dXp = Point.X - P0.X;
			dYp = Point.Y - P0.Y;
			distMin = Hypot(dYp, dXp);
		
			do
			{
				I = (I + 1) & (0 - Convert.ToInt32(I + 1 < N));
				if( I == N - 1 )
				{
					Result = distMin;
					return Result;
				}
				P1 = Ring[I];
				dX = P1.X - P0.X;
				dY = P1.Y - P0.Y;
				Len2 = dY * dY + dX * dX;
			}
			while( Len2 >= Eps);
		
			dXp = Point.X - P0.X;
			dYp = Point.Y - P0.Y;
		
			X0 = (dXp * dX + dYp * dY) - Len2;
		
			while( I < N )
			{
				P0 = P1;
				J = I;
				do
			{
					J = (J + 1) & (0 - Convert.ToInt32(J + 1 < N));
					if( J == I )
					{
						Result = distMin;
						return Result;
					}
					P1 = Ring[J];
					dX = P1.X - P0.X;
					dY = P1.Y - P0.Y;
					Len2 = dY * dY + dX * dX;
				}
			while( Len2 >= Eps);
		
				dXp = Point.X - P0.X;
				dYp = Point.Y - P0.Y;
				X = dXp * dX + dYp * dY;
		
				if( X > 0 )
				{
					if( X < Len2 )
					{
						d0 = System.Math.Abs(dYp * dX - dXp * dY)/Math.Sqrt(Len2);
						if( d0 < distMin ) distMin = d0;
					}
				}
				else if( X0 > 0 )
				{
					d0 = Hypot(dYp, dXp);
					if( d0 < distMin ) distMin = d0;
				}
		
				X0 = X - Len2;
		
				if( J < I ) break;
				I = J;
			}
		
			Result = distMin;
			return Result;
		}

		public static Point CircleVectorIntersect(Point CentPoint, double Radius, Line Line, double d)
		{
		 	double	distToVect;
			Point Result;
			Line Line1;
			Geometry geom;
			Result = null;
			Line1 = new Line (CentPoint, Line.DirVector.direction + 0.5*Math.PI);
		
			geom = LineLineIntersect (Line, Line1);
			Line1 = null;
		
			if( geom.GetGeometryType() != GeometryType.Point )
			{
				return Result;
			}
		
			distToVect = Hypot (CentPoint.X - geom.AsPoint.X,  CentPoint.Y - geom.AsPoint.Y);
			d =  -1.0;
		
			if( distToVect < Radius )
			{
				d =  Math.Sqrt(Math.Pow(Radius,2) - Math.Pow(distToVect,2));
				Result = PointAlongPlane (geom.AsPoint, Line.DirVector.direction, d);
			}
		
			geom = null;
			return Result;
		}

		public static Point CircleVectorIntersect(Point CenterPoint, double Radius, Point ptVector, double Direction, double d)
		{
		 
			double	distToVect;
			Point Result;
			Geometry geom;

			Result = null;
			geom = LineLineIntersect (ptVector, Direction, CenterPoint, Direction + 0.5 * Math.PI);
		
			if( geom.GetGeometryType() != GeometryType.Point )
				return Result;
			
		
			distToVect = Hypot (CenterPoint.X - geom.AsPoint.X,  CenterPoint.Y - geom.AsPoint.Y);
			d =  -1.0;
		
			if( distToVect < Radius )
			{
				d =  Math.Sqrt (Math.Pow(Radius,2) - Math.Pow(distToVect,2));
				Result = PointAlongPlane (geom.AsPoint, Direction, d);
			}
		
			return Result;
		}

		public static void CircleVectorIntersect(Point centPoint, double radius, Line Line, double d, Point Result)
		{
			double	distToVect;
			Line Line1;
			Geometry geom;
			Point ptTmp;
			Result.SetEmpty();
		
			Line1 = new Line (centPoint, Line.DirVector.direction + 0.5*Math.PI);
		
			geom = LineLineIntersect (Line, Line1);
		
			if( geom.GetGeometryType() != GeometryType.Point )
				return;
		
			distToVect = Hypot (centPoint.X - geom.AsPoint.X,  centPoint.Y - geom.AsPoint.Y);
			d =  -1.0;
		
			if( distToVect < radius )
			{
				d =  Math.Sqrt(Math.Pow(radius,2) - Math.Pow(distToVect,2));
				ptTmp = PointAlongPlane (geom.AsPoint, Line.DirVector.direction, d);
				Result.Assign(ptTmp);
			}
		
		}

		public static void CircleVectorIntersect(Point CenterPoint, double Radius, Point ptVector, double Direction, double d, Point Result)
		{
		 
			double	distToVect;
			Geometry geom;
			Result.SetEmpty();
			Point ptTmp;
		
			geom = LineLineIntersect (ptVector, Direction, CenterPoint, Direction + 0.5 * Math.PI);
		
			if( geom.GetGeometryType() != GeometryType.Point )
				return;
		
			distToVect = Hypot (CenterPoint.X - geom.AsPoint.X,  CenterPoint.Y - geom.AsPoint.Y);
			d =  -1.0;
		
			if( distToVect < Radius )
			{
				d =  Math.Sqrt(Math.Pow(Radius,2) - Math.Pow(distToVect,2));
				ptTmp = PointAlongPlane (geom.AsPoint, Direction, d);
				Result.Assign(ptTmp);
			}
		
		}

		public static Ring CreateCirclePrj(Point PtCnt, double Radius)
		{
			int	I;
			double AngleStep,iInRad;
			Point Pt; 
			Ring Result;
		
			AngleStep = DegToRad(1.0);
			Pt = new Point();
			Result = new Ring();
		
			for( I = 0;I <= 359;I++)
			{
				iInRad = I * AngleStep;
				Pt.X = PtCnt.X + Radius * Math.Cos(iInRad);
				Pt.Y = PtCnt.Y + Radius * Math.Sin(iInRad);
				Result.AddPoint(Pt);
				
			}
			return Result;
		}

        public static Part CreateCircleAsPartPrj ( Point ptCnt, double radius )
        {
            Part result = new Part ();
            double angleStep, iInRad;
            Point pnt;

            angleStep = DegToRad ( 1.0 );
            pnt = new Point ();

            for ( int i = 0; i<=360; i++ )
            {
                iInRad = i*angleStep;
                pnt.X = ptCnt.X + radius*Math.Cos ( iInRad );
                pnt.Y = ptCnt.Y + radius*Math.Sin ( iInRad );
                result.Add ( pnt );
            }

            return result;
        }

		public static Ring CreateArcPrj(Point PtCnt, Point ptFrom, Point ptTo, SideDirection Direction)
		{
			int			N, I;
			double			AngleStep, fDir,
			R, dX, dY,
			AztFrom, AztTo,
			iInRad;
			double			dAz;
			Point Pt;
			Ring Result;
		
			dX = ptFrom.X - PtCnt.X;
			dY = ptFrom.Y - PtCnt.Y;
			R = Math.Sqrt(dX * dX + dY * dY);
			Pt = new Point();

			AztFrom = Math.Atan2(dY, dX);//Math.Atan2(dY, dX);
			AztTo = Math.Atan2(ptTo.Y - PtCnt.Y, ptTo.X - PtCnt.X);
		
			fDir = -(int)(Direction);						  
			dAz = ARANMath.Modulus((AztTo - AztFrom) * fDir, 2 * Math.PI);
		
			N = (int)Math.Floor(RadToDeg(dAz));
			if( N == 0 ) 	
				N = 1;
			else if( N < 10 )
				N = 10;
		
			AngleStep = dAz / N;
			Result = new Ring();
			Result.AddPoint(ptFrom);
		
			for( I = 1;I <= N - 1;I++)
			{
				iInRad = AztFrom + I * AngleStep * fDir;
				Pt.X = PtCnt.X + R * Math.Cos(iInRad);
				Pt.Y = PtCnt.Y + R * Math.Sin(iInRad);
				Result.AddPoint(Pt);
			}
		
			Result.AddPoint(ptTo);
			return Result;
		}

		public static Part CreateArcAsPartPrj(Point PtCnt, Point ptFrom, Point ptTo, SideDirection Direction)
		{
			int N, I;
			double AngleStep, fDir,
			R, dX, dY,
			AztFrom, AztTo,
			iInRad;
			double dAz;
			Point Pt;
			Part Result;

			dX = ptFrom.X - PtCnt.X;
			dY = ptFrom.Y - PtCnt.Y;
			R = Math.Sqrt(dX * dX + dY * dY);
			Pt = new Point();

			AztFrom = Math.Atan2(dY, dX);//Math.Atan2(dY, dX);
			AztTo = Math.Atan2(ptTo.Y - PtCnt.Y, ptTo.X - PtCnt.X);

			fDir = -(int)(Direction);
			dAz = ARANMath.Modulus((AztTo - AztFrom) * fDir, 2 * Math.PI);

			N = (int)Math.Floor(RadToDeg(dAz));
			if (N == 0)
				N = 1;
			else if (N < 10)
				N = 10;

			AngleStep = dAz / N;
			Result = new Part();
			Result.AddPoint(ptFrom);

			for (I = 1; I <= N - 1; I++)
			{
				iInRad = AztFrom + I * AngleStep * fDir;
				Pt.X = PtCnt.X + R * Math.Cos(iInRad);
				Pt.Y = PtCnt.Y + R * Math.Sin(iInRad);
				Result.AddPoint(Pt);
			}

			Result.AddPoint(ptTo);
			return Result;
		}

		public static Part RingToPart(Ring ring)
		{
			Part part = new Part();
			foreach (Point pt in ring)
			{
				part.Add(pt);	
			}
			return part;
		}

		public static Ring PartToRing(Part part)
		{
			Ring ring = new Ring();
			foreach (Point  pt in part)
			{
				ring.Add(pt);
			}
			return ring;
		}

		public static Polygon CreatePolygonFromRings(params Ring[] ringList)
		{
			Polygon geom = new Polygon();
			foreach (Ring ring in ringList)
			{
				geom.Add(ring);	
			}
			return geom;
		}

		public static PolyLine CreatePolyLineFromParts(params Part[] partList)
		{
			PolyLine geom = new PolyLine();
			foreach (Part part in partList)
			{
				geom.Add(part);	
			}
			return geom;
		}

		public static PolyLine PolygonToPolyLine(Polygon geom)
		{
			PolyLine polyLine = new PolyLine();
			foreach (Ring ring in geom)
			{
				polyLine.Add(RingToPart(ring));
			}
			return polyLine;			
		}

		public static Polygon PolyLineToPolygon(PolyLine geom)
		{
			Polygon polygon = new Polygon();
			foreach (Part part in geom)
			{
				polygon.Add(PartToRing(part));
			}
			return polygon;
		}


					  					  		
		public static  PolyLine DrawArcPrj( Point PtCnt,Point ptFrom,Point ptTo, int ClWise)
		{
			int			N, I;
			double			AngStep,
			R, dX, dY,
			AztFrom, AztTo,
			iInRad;
			double daz;
			Point Pt;
			Part part;
			 
			PolyLine Result;
		
			dX = ptFrom.X - PtCnt.X;
			dY = ptFrom.Y - PtCnt.Y;
			R = Math.Sqrt(dX * dX + dY * dY);
		
			Pt = new Point();
		
			AztFrom = ARANMath.Modulus(Math.Atan2(dY, dX), 2 *Math.PI);
			AztTo = ARANMath.Modulus(Math.Atan2(ptTo.Y - PtCnt.Y, ptTo.X - PtCnt.X), 2 *Math.PI);
		
			daz = (double)Math.Ceiling(ARANMath.Modulus((AztTo - AztFrom) * ClWise, 2 *Math.PI)* 180/Math.PI);
		
			AngStep = 1;
			N = (int)Math.Floor(daz/ AngStep);
		
			if( N == 0 ) 		N = 1;
			else if( N < 10 ) N = 10;
		
			AngStep = daz / N;
		
			part = new Part();
			part.AddPoint (ptFrom);
		
			for( I = 1;I <= N - 1;I++)
			{
				iInRad = AztFrom + I * AngStep * ClWise*Math.PI/180;
				Pt.X = PtCnt.X + R * Math.Cos(iInRad);
				Pt.Y = PtCnt.Y + R * Math.Sin(iInRad);
				part.AddPoint (Pt);
			}
		
			part.AddPoint (ptTo);
			Result = new PolyLine();
			Result.AddPart (part);
		
			return Result;
		}

		public static void AddArcToMultiPoint(Point PtCnt, Point ptFrom, Point ptTo, int ClWise,ref MultiPoint result)
		{
			int N, I;
			double AngStep,
			R, dX, dY,
			AztFrom, AztTo,
			iInRad;
			double daz;
			Point Pt;
			
			dX = ptFrom.X - PtCnt.X;
			dY = ptFrom.Y - PtCnt.Y;
			R = Math.Sqrt(dX * dX + dY * dY);

			Pt = new Point();

			AztFrom = ARANMath.Modulus(Math.Atan2(dY, dX), 2 * Math.PI);
			AztTo = ARANMath.Modulus(Math.Atan2(ptTo.Y - PtCnt.Y, ptTo.X - PtCnt.X), 2 * Math.PI);

			daz = (double)Math.Ceiling(ARANMath.Modulus((AztTo - AztFrom) * ClWise, 2 * Math.PI) * 180 / Math.PI);

			AngStep = 1;
			N = (int)Math.Floor(daz / AngStep);

			if (N == 0) N = 1;
			else if (N < 10) N = 10;

			AngStep = daz / N;

			result.AddPoint(ptFrom);

			for (I = 1; I <= N - 1; I++)
			{
				iInRad = AztFrom + I * AngStep * ClWise * Math.PI / 180;
				Pt.X = PtCnt.X + R * Math.Cos(iInRad);
				Pt.Y = PtCnt.Y + R * Math.Sin(iInRad);
				result.AddPoint(Pt);
			}

			result.AddPoint(ptTo);
		}

		public static PolyLine ConvertPointsToTrackLIne(MultiPoint MultiPoint)
		{
			int		I, J, N = 0;
			double		fTmp, fE;
			PolyLine Result;
			Point CntPt,FromPt = null,ToPt = null;
			Part Part;
			Geometry tmpGeometry;
			SideDirection Side;
			PolyLine arcPolyline;
			fE = DegToRad(0.5);
			N = MultiPoint.Count - 2;
		
			CntPt = new Point();
			Part = new Part();
		
			Part.AddPoint(MultiPoint[0]);
		
			for( I = 0;I <= N;I++)
			{
				FromPt = MultiPoint[I];
				ToPt = MultiPoint[I + 1];
				fTmp = FromPt.M - ToPt.M;
		
				if( (System.Math.Abs(Math.Sin(fTmp)) <= fE) && (Math.Cos(fTmp) > 0) )
					Part.AddPoint(ToPt);
				else
				{
					if( System.Math.Abs(Math.Sin(fTmp)) > fE )
					{
						tmpGeometry = LineLineIntersect(FromPt, FromPt.M + 0.5*Math.PI, ToPt, ToPt.M + 0.5 * Math.PI);
		// 				if tmpGeometry.GeometryType = gtPoint then
						CntPt.Assign(tmpGeometry);
		// 				else
					}
					else
						CntPt.SetCoords(0.5 * (FromPt.X + ToPt.X), 0.5 * (FromPt.Y + ToPt.Y));
		
					Side = SideDef(FromPt, FromPt.M, ToPt);
					arcPolyline = DrawArcPrj(CntPt, FromPt, ToPt, -(int)(Side));
		
					for( J = 0;J <= arcPolyline[0].Count - 1;J++)
						Part.AddPoint (arcPolyline[0][J]);
		
				}
			}
		
			Result = new PolyLine();
			Result.AddPart(Part);
			return Result;
		}

		public static PolyLine CalcTrajectoryFromMultiPoint(Line[] lineArray)
		{
			int		i, n, j;
			double		fTmp, fE;
			 
			PolyLine Result;
			Line fromLine, toLine;
			Part part;
			part = new Part();
			Point centrePoint;
			SideDirection sideDir;
			PolyLine arcPolyline;
		
			fE = DegToRad(0.5);
			n = lineArray.Length - 2;
		
			part.AddPoint (lineArray [0].RefPoint);
		
			for( i = 0;i <= n;i++)
			{
				fromLine = lineArray[i];
				toLine = lineArray [i+1];
				fTmp = fromLine.DirVector.direction - toLine.DirVector.direction;
		
				if( (System.Math.Abs (Math.Sin (fTmp)) <= fE) && (Math.Cos (fTmp) > 0) )
					part.AddPoint (toLine.RefPoint);
				else
				{
					if( System.Math.Abs (Math.Sin(fTmp)) > fE )
						centrePoint = LineLineIntersect (
										fromLine.RefPoint, fromLine.DirVector.direction + 0.5*Math.PI,
										toLine.RefPoint, toLine.DirVector.direction + 0.5 * Math.PI).AsPoint;
					else
						centrePoint = new Point (
											0.5 * (fromLine.RefPoint.X + toLine.RefPoint.X),
											0.5 * (fromLine.RefPoint.Y + toLine.RefPoint.Y));
		
					sideDir = SideDef(fromLine.RefPoint, fromLine.DirVector.direction, toLine.RefPoint);
					arcPolyline = DrawArcPrj (centrePoint, fromLine.RefPoint, toLine.RefPoint, -1 * (int)(sideDir));
					for( j=0;j <= arcPolyline[0].Count - 1;j++)
					{
						part.AddPoint (arcPolyline[0][j]);
					}
				}
			}
		
			Result = new PolyLine();
			Result.AddPart (part);
			return Result;
		}

		public static PolyLine CalcTrajectoryFromMultiPoint(MultiPoint mPoint)
		{
			int i, n, j;
			double fTmp, fE;

			PolyLine Result;
			Point fromLine, toLine;
			Part part;
			part = new Part();
			Point centrePoint;
			SideDirection sideDir;
			PolyLine arcPolyline;

			fE = DegToRad(0.5);
			n = mPoint.Count - 2;

			part.AddPoint(mPoint[0]);

			for (i = 0; i <= n; i++)
			{
				fromLine = mPoint[i];
				toLine = mPoint[i + 1];
				fTmp = fromLine.M - toLine.M;

				if ((System.Math.Abs(Math.Sin(fTmp)) <= fE) && (Math.Cos(fTmp) > 0))
					part.AddPoint(toLine);
				else
				{
					if (System.Math.Abs(Math.Sin(fTmp)) > fE)
						centrePoint = LineLineIntersect(
										fromLine, fromLine.M + 0.5 * Math.PI,
										toLine, toLine.M + 0.5 * Math.PI).AsPoint;
					else
						centrePoint = new Point(
											0.5 * (fromLine.X + toLine.X),
											0.5 * (fromLine.Y + toLine.Y));

					sideDir = SideDef(fromLine, fromLine.M, toLine);
					arcPolyline = DrawArcPrj(centrePoint, fromLine, toLine, (int)(sideDir));
					for (j = 0; j <= arcPolyline[0].Count - 1; j++)
					{
						part.AddPoint(arcPolyline[0][j]);
					}
				}
			}

			Result = new PolyLine();
			Result.AddPart(part);
			return Result;
		}

		public static Point TangentCyrcleIntersectPoint(Point centrePoint, double radius, Point outPoint, SideDirection side)
		{
			double	dirLine, dirRadius,
			distance, alpha;
			int			turnVal;
			Point Result;
		
			Result = null;
		
			distance = ReturnDistanceAsMeter(centrePoint, outPoint);
			if( distance < radius )
				return Result;
		
			turnVal = (int) (side);
			alpha =  Math.Asin (radius / distance);
			dirLine = ReturnAngleAsRadian (outPoint, centrePoint);
			dirRadius = dirLine - turnVal * (alpha + 0.5*Math.PI);
			dirLine = dirLine - turnVal * alpha;
			Result = LineLineIntersect (outPoint, dirLine, centrePoint, dirRadius).AsPoint;
			return Result;
		}

		public static double FixToTouchSprial(Line starLine, Line endLine, double coefficient, double turnRadius, SideDirection turnSide)
		{
			double	tmpCoef,
			theta0, d, d2, r,
			f, F1, theta1,
			x1, y1, centreTheta,
			fixTheta, dTheta,
			theta1New;
			double			sinT, cosT;
			int			turnVal, i;
			Point spiralCentrePoint,outPoint;

			 
			double Result;
		
			Result = 4; //  greater than Math.PI.
			turnVal = (int) (turnSide);
			turnVal = -turnVal;
		
			tmpCoef =  RadToDeg (coefficient);
		
			theta0 = ARANMath.Modulus(starLine.DirVector.direction - 0.5*Math.PI*turnVal, 2*Math.PI);
			spiralCentrePoint = PointAlongPlane (starLine.RefPoint, starLine.DirVector.direction + 0.5*Math.PI*turnVal, turnRadius);
			d = ReturnDistanceAsMeter (spiralCentrePoint, endLine.RefPoint);
			fixTheta = ReturnAngleAsRadian (spiralCentrePoint, endLine.RefPoint);
			dTheta = ARANMath.Modulus ( (fixTheta - theta0) * turnVal, 2*Math.PI);
			r = turnRadius + RadToDeg (dTheta) * coefficient;
			if (d < r) return Result ;
		
			x1 =  endLine.RefPoint.X - spiralCentrePoint.X;
			y1 = endLine.RefPoint.Y - spiralCentrePoint.Y;
			centreTheta = SpiralTouchAngle (turnRadius, coefficient, starLine.DirVector.direction, endLine.DirVector.direction, turnSide);
			centreTheta = ARANMath.Modulus (theta0 + centreTheta * turnVal, 2*Math.PI);
		
			// ---Variant Firdowsy
		
			theta1 = centreTheta;
			for( i=0;i <= 20;i++)
			{
				dTheta = ARANMath.Modulus ((theta1 - theta0) * turnVal, 2*Math.PI);
				sinT = Math.Sin(theta1);
				cosT = Math.Cos(theta1);
				r = turnRadius + RadToDeg (dTheta) * coefficient;
				f = Math.Pow(r,2) - (y1 * r + x1 * tmpCoef * turnVal) * sinT - (x1 * r - y1 * tmpCoef * turnVal) * cosT;
				F1 = 2 * r * tmpCoef * turnVal - (y1 * r + 2 * x1 * tmpCoef * turnVal) * cosT + (x1 * r - 2 * y1 * tmpCoef * turnVal) * sinT;
				theta1New = theta1 - (f / F1);
		
				d = ARANMath.SubtractAngles (theta1New, theta1);
				theta1 = theta1New;
				if( d < DegToRad (0.0001) ) break;
			}
		
			dTheta = ARANMath.Modulus ((theta1 - theta0) * turnVal, 2*Math.PI);
			r = turnRadius + RadToDeg (dTheta) * coefficient;
			outPoint = PointAlongPlane (spiralCentrePoint, theta1, r);
		
			d = ReturnAngleAsRadian (outPoint, endLine.RefPoint);
			centreTheta = SpiralTouchAngle (turnRadius, coefficient, starLine.DirVector.direction, d, turnSide);
			centreTheta = ARANMath.Modulus (theta0 + centreTheta * turnVal, 2*Math.PI);
			d2 = ARANMath.SubtractAngles (centreTheta, theta1);
			if( d2 < 0.0001 )		Result = d;
			return Result;
		}

		public static double DegToRad(double p)
		{
			return p * (Math.PI / 180);
		}

		public static IList<int> SortArray(IList<double> Items)
		{
			
			int i, j,count;
			double minVal ;
			IList<int> resultIndexList,searchIndexList;
			int minIndex,curIndex;

			if (Items.Count == 0) 
				return null;
			count = Items.Count;
			resultIndexList = new List<int>(count);
			searchIndexList = new List<int>(count);

			if (count  == 1)		
			{
				resultIndexList[0] = 0;
				return resultIndexList;
			}
			for (i=0;i<count;i++)
				searchIndexList.Add(i);

			for (i=0;i<count;i++) 
			{
				minVal = Items[searchIndexList[0]];
				minIndex = searchIndexList[0];
				curIndex = 0;
				for (j=0;j<searchIndexList.Count;j++)
				{
					if (Items[searchIndexList[j]]<minVal)
					{
						minVal=Items[searchIndexList[j]];
						minIndex =searchIndexList[j];
						curIndex=j;
			
					}
				}
								
				searchIndexList.Remove(curIndex);
				resultIndexList[i]= minIndex;
			}
			return resultIndexList;
			
		}

		public static MultiPoint SortPoints(Point basePoint, MultiPoint multiPoint)
		{
			int	i;
			IList<double> distanceList;
			IList<int> indexList;
			MultiPoint Result;
		
			Result = null;
		
			if( multiPoint.Count == 0 )
				return Result;
		
			Result = new MultiPoint();
		
			if( multiPoint.Count < 1 )
			{
		// 		result.AddPoint (multiPoint.Point [0]);		//Invalid
				return Result;
			}
		
			distanceList = new List<double>(multiPoint.Count);
		
			for( i = 0;i <= multiPoint.Count - 1;i++)
				distanceList [i] = ReturnDistanceAsMeter (basePoint, multiPoint[i]);
		
			indexList = SortArray (distanceList);
		
			for( i = 0;i <= multiPoint.Count - 1;i++)
				Result.AddPoint (multiPoint[indexList[i]]);
			return Result;
		}
																						
		/*
function GeoToPrj (geoGeometry: Geometry): Geometry;
begin
	result := GeoOper.geoTransformations (geoGeometry, GGeoSR, GPrjSR);
end;

function PrjToGeo (prjGeometry: Geometry): Geometry;
begin
	result := GeoOper.geoTransformations (prjGeometry, GPrjSR, GGeoSR);
end;
*/
		//public void FreeObject(object sender, EventArgs e)
		//{
		
		//    vObject.Dispose();
		//    vObject = null;
		//}
		
		public static string IfThen( bool AValue, string ATrue, string AFalse)
		{
					  		
		  if (AValue) 
			return ATrue;
		  else
			return AFalse;
		}
		
		//public static int ShowHelp( long callerHWND, int helpContextId)
		//{
		//    int Result;
		//    Result = HtmlHelp (callerHWND, Application.HelpF, 0xF, helpContextId);
		//    return Result;
		//}

		public static double Hypot(double x, double y)
		{
			double tmp;
			x = Math.Abs(x);
			y = Math.Abs(y);
			if (x > y)
			{
				tmp = x;
				x = y;
				y = tmp;
			}
			if (x == 0)
				return y;
			else         // Y > X, X <> 0, so Y > 0
				return y * Math.Sqrt(1 + Math.Pow(x / y, 2));
		}

		public static int AnglesSideDef(double X, double Y)
		{
			double Z;
			Z = ARANMath.Modulus(X - Y, 360.0);
			if (Z == 0.0)
			{
				return 0;
			}
			else if (Z > 180.0)
			{
				return -1;
			}
			else if (Z < 180.0)
			{
				return 1;
			}
			else
			{
				return 2;
			}
		}

		public static Object RegRead(RegistryKey HKey, string key, string valueName)
		{
			RegistryKey regKey;
			try
			{					
				regKey = HKey.OpenSubKey(key);
				if (key != null)
				  return regKey.GetValue(valueName);

			}
			catch (Exception)
			{
				
			}
			return null;
		}

        public static string RegRead(RegistryKey HKey, string key, string valueName,string defaultValue)
        {
            RegistryKey regKey;
            try
            {
                regKey = HKey.OpenSubKey(key);
                if (key != null)
                    return regKey.GetValue(valueName) as string;

            }
            catch (Exception)
            {
                
            }
            return defaultValue;
        }

        public static int RegRead(RegistryKey HKey, string key, string valueName,int defaultValue)
        {
            RegistryKey regKey;
            try
            {
                regKey = HKey.OpenSubKey(key);
                if (key != null)
                    return (int)regKey.GetValue(valueName);

            }
            catch (Exception)
            {
            }
            return defaultValue;
        }

        public static double RegRead(RegistryKey HKey, string key, string valueName, double defaultValue)
        {
            RegistryKey regKey;
            try
            {
                regKey = HKey.OpenSubKey(key);
                if (key != null)
                    return (double)regKey.GetValue(valueName);

            }
            catch (Exception)
            {
                
            }
            return defaultValue;
        }

        public static int RegWrite(Microsoft.Win32.RegistryKey HKey, string Key, string ValueName, object Value)
        {
            try
            {
                Microsoft.Win32.RegistryKey regKey = HKey.OpenSubKey(Key, true);
                if (regKey == null)
                {
                    return -1;
                }

                regKey.SetValue(ValueName, Value);
                return 0;
            }
            catch { }

            return -1;
        }

		public static Ring ToleranceArea(Point fixPoint, double aTTDistance, double xTTDistance, double radian,SideDirection turn)
		{
			Point  pt1, pt2, pt3, pt4;
			Ring toleranceArea = new Ring();

			pt1 = LocalToPrj(fixPoint, radian, -aTTDistance,(int)turn * xTTDistance);
			pt2 = LocalToPrj(fixPoint, radian, -aTTDistance,(int)(ARANMath.ChangeDirection(turn)) * xTTDistance);
			pt3 = LocalToPrj(fixPoint, radian, aTTDistance,(int)(ARANMath.ChangeDirection(turn)) * xTTDistance);
			pt4 = LocalToPrj(fixPoint, radian, aTTDistance,(int)turn * xTTDistance);
			toleranceArea.Add(pt1);
			toleranceArea.Add(pt2);
			toleranceArea.Add(pt3);
			toleranceArea.Add(pt4);
			return toleranceArea;

		}

		public static double AztToDirection (Point pointGeo,double azimuthInRad,SpatialReference geoSR,SpatialReference prjSR)
		{
			Point	pointToGeo,pointFromPrj,pointToPrj;
			GeometryOperators _geoOper = new GeometryOperators();
			pointToGeo = PointAlongGeodesic(pointGeo, ARANFunctions.RadToDeg(azimuthInRad), 10.0);
			pointFromPrj = (Point)_geoOper.GeoTransformations ((Geometry)pointGeo, geoSR, prjSR);
			pointToPrj = (Point)_geoOper.GeoTransformations ((Geometry)pointToGeo, geoSR, prjSR);
			return  ReturnAngleAsRadian (pointFromPrj, pointToPrj);
		}

		public static double DirToAzimuth(Point pointPrj, double DirInRadian, SpatialReference prjSR, SpatialReference geoSR)
		{
				double directAzumuth = 0;
				double inverseAzumuth = 0;
				Point pointToPrj, pointToGeo, pointFromGeo;
				GeometryOperators _geoOper = new GeometryOperators();
				pointToPrj = PointAlongPlane(pointPrj, DirInRadian, 10.0);
				pointFromGeo = (Point)_geoOper.GeoTransformations((Geometry)pointPrj, prjSR, geoSR);
				pointToGeo = (Point)_geoOper.GeoTransformations((Geometry)pointToPrj, prjSR, geoSR);
				InitAll();
				ReturnGeodesicAzimuth_MathFunctions(pointFromGeo.X, pointFromGeo.Y, pointToGeo.X, pointToGeo.Y, out directAzumuth,out inverseAzumuth);
				return DegToRad(directAzumuth);
						
		}

		
	}
	
}

