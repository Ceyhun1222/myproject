using System;

using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Carto;

using Aran.Aim.Enums;
using System.Collections.Generic;

namespace ETOD
{
	[System.Runtime.InteropServices.ComVisible(false)]
	public class Win32Window : System.Windows.Forms.IWin32Window
	{
		private IntPtr _handle;

		public Win32Window(Int32 handle)
		{
			_handle = new IntPtr(handle);
		}

		public IntPtr Handle
		{
			get { return _handle; }
		}
	}

	[System.Runtime.InteropServices.ComVisible(false)]
	public class Matrix
	{
		public float m00, m01, m02,
						m10, m11, m12,
						m20, m21, m22;

		static public Matrix operator *(Matrix m1, Matrix m2)
		{
			Matrix mat = new Matrix();
			mat.m00 = m1.m00 * m2.m00 + m1.m01 * m2.m10 + m1.m02 * m2.m20;
			mat.m01 = m1.m00 * m2.m01 + m1.m01 * m2.m11 + m1.m02 * m2.m21;
			mat.m02 = m1.m00 * m2.m02 + m1.m01 * m2.m12 + m1.m02 * m2.m22;
			mat.m10 = m1.m10 * m2.m00 + m1.m11 * m2.m10 + m1.m12 * m2.m20;
			mat.m11 = m1.m10 * m2.m01 + m1.m11 * m2.m11 + m1.m12 * m2.m21;
			mat.m12 = m1.m10 * m2.m02 + m1.m11 * m2.m12 + m1.m12 * m2.m22;
			mat.m20 = m1.m20 * m2.m00 + m1.m21 * m2.m10 + m1.m22 * m2.m20;
			mat.m21 = m1.m20 * m2.m01 + m1.m21 * m2.m11 + m1.m22 * m2.m21;
			mat.m22 = m1.m20 * m2.m02 + m1.m21 * m2.m12 + m1.m22 * m2.m22;
			return mat;
		}

		public Matrix()
		{
			Identity();
		}

		public Matrix(Matrix m)
		{
			m00 = m.m00; m01 = m.m01; m02 = m.m02;
			m10 = m.m10; m11 = m.m11; m12 = m.m12;
			m20 = m.m20; m21 = m.m21; m22 = m.m22;
		}

		public void Identity()
		{
			m01 = m02 = m10 = m12 = m20 = m21 = 0.0f;
			m00 = m11 = m22 = 1.0f;
		}

		public void Zero()
		{
			m00 = m01 = m02 = m10 = m11 = m12 = m20 = m21 = m22 = 0.0f;
		}

		public void Translation(float tx, float ty)
		{
			Identity();
			m02 = tx;
			m12 = ty;
		}

		public void Rotation(float angle)
		{
			float fsin = (float)(Math.Sin(angle));
			float fcos = (float)(Math.Cos(angle));
			Identity();
			m00 = m11 = fcos;
			m10 = fsin;
			m01 = -fsin;
		}

		public void Shear(float sx, float sy)
		{
			Identity();
			m01 = sx;
			m10 = sy;
		}

		public void Scaling(float sx, float sy)
		{
			Identity();
			m00 = sx;
			m11 = sy;
		}

		static private void swapf(ref  float f1, ref  float f2)
		{
			float tmp = f1;
			f1 = f2;
			f2 = tmp;
		}

		public float Det()
		{
			return m00 * m11 * m22 - m02 * m11 * m20 + m10 * m21 * m02 - m12 * m21 * m00 + m20 * m01 * m12 - m22 * m01 * m10;
		}

		//public Matrix Transpose()
		//{
		//    swapf(ref m01, ref m10);
		//    swapf(ref m02, ref m20);
		//    swapf(ref m21, ref m12);
		//    return this;
		//}

		public Matrix Transpose()
		{
			Matrix mat = new Matrix(this);
			swapf(ref mat.m01, ref mat.m10);
			swapf(ref mat.m02, ref mat.m20);
			swapf(ref mat.m21, ref mat.m12);
			return mat;
		}

		public Matrix Inverse()
		{
			float det = Det();
			if (det == 0) return null;

			double k = 1.0 / det;
			Matrix mat = new Matrix();

			mat.m00 = (float)((m11 * m22 - m12 * m21) * k);
			mat.m01 = (float)((m02 * m21 - m01 * m22) * k);
			mat.m02 = (float)((m01 * m12 - m02 * m11) * k);
			mat.m10 = (float)((m12 * m20 - m10 * m22) * k);
			mat.m11 = (float)((m00 * m22 - m02 * m20) * k);
			mat.m12 = (float)((m02 * m10 - m00 * m12) * k);
			mat.m20 = (float)((m10 * m21 - m11 * m20) * k);
			mat.m21 = (float)((m01 * m20 - m00 * m21) * k);
			mat.m22 = (float)((m00 * m11 - m01 * m10) * k);
			return mat;
		}

		static public Matrix Transpose(Matrix m)
		{
			Matrix mat = new Matrix(m);
			swapf(ref mat.m01, ref mat.m10);
			swapf(ref mat.m02, ref mat.m20);
			swapf(ref mat.m21, ref mat.m12);
			return mat;
		}

		static public Matrix Inverse(Matrix m)
		{
			float det = m.Det();
			if (det == 0) return null;

			double k = 1.0 / det;

			Matrix mat = new Matrix();
			mat.m00 = (float)((m.m11 * m.m22 - m.m12 * m.m21) * k);
			mat.m01 = (float)((m.m02 * m.m21 - m.m01 * m.m22) * k);
			mat.m02 = (float)((m.m01 * m.m12 - m.m02 * m.m11) * k);
			mat.m10 = (float)((m.m12 * m.m20 - m.m10 * m.m22) * k);
			mat.m11 = (float)((m.m00 * m.m22 - m.m02 * m.m20) * k);
			mat.m12 = (float)((m.m02 * m.m10 - m.m00 * m.m12) * k);
			mat.m20 = (float)((m.m10 * m.m21 - m.m11 * m.m20) * k);
			mat.m21 = (float)((m.m01 * m.m20 - m.m00 * m.m21) * k);
			mat.m22 = (float)((m.m00 * m.m11 - m.m01 * m.m10) * k);
			return mat;
		}
	}

	[System.Runtime.InteropServices.ComVisible(false)]
	public class Vector2
	{
		public float x, y, w;
		public Vector2()
		{
			x = y = 0.0f;
			w = 1.0f;
		}

		public Vector2(float x, float y)
		{
			this.x = x;
			this.y = y;
			w = 1.0f;
		}

		static public Vector2 Transform(Vector2 p, Matrix m)
		{
			Vector2 pt = new Vector2();
			pt.x = m.m00 * p.x + m.m01 * p.y + m.m02 * p.w;
			pt.y = m.m10 * p.x + m.m11 * p.y + m.m12 * p.w;
			pt.w = m.m20 * p.x + m.m21 * p.y + m.m22 * p.w;
			return pt;
		}

		public void TransformMe(Vector2 p, Matrix m)
		{
			x = m.m00 * p.x + m.m01 * p.y + m.m02 * p.w;
			y = m.m10 * p.x + m.m11 * p.y + m.m12 * p.w;
			w = m.m20 * p.x + m.m21 * p.y + m.m22 * p.w;
		}

		public void TransformMe(Matrix m)
		{
			float _x, _y;

			_x = m.m00 * x + m.m01 * y + m.m02 * w;
			_y = m.m10 * x + m.m11 * y + m.m12 * w;
			w = m.m20 * x + m.m21 * y + m.m22 * w;
			x = _x; y = _y;
		}

		public float Length()
		{
			return (float)(Math.Sqrt(x * x + y * y));
		}

		public void Normalize()
		{
			float len = Length();
			if (len == 0)
				return;
			double k = 1.0 / len;

			x = (float)(x * k);
			y = (float)(y * k);
		}

		public float DotProduct(Vector2 vec)
		{
			return x * vec.x + y * vec.y;
		}
	};

	[System.Runtime.InteropServices.ComVisible(false)]
	public class LocalCoordinatSystem
	{
		double _dir;
		IPoint _ptCenter;

		public LocalCoordinatSystem(double dir, IPoint ptCenter)
		{
			_dir = 2 * Math.PI - Functions.DegToRad(dir);

			IClone pClone = ptCenter as IClone;
			_ptCenter = pClone.Clone() as IPoint;
		}

		public IGeometry TransformForward(IGeometry prjGeom)
		{
			IClone pClone = prjGeom as IClone;
			IGeometry pGeom = pClone.Clone() as IGeometry;

			ITransform2D transform2D = pGeom as ITransform2D;
			transform2D.Rotate(_ptCenter, _dir);
			transform2D.Move(-_ptCenter.X, -_ptCenter.Y);

			return pGeom;
		}

		public IGeometry TransformReverse(IGeometry prjGeom)
		{
			IClone pClone = prjGeom as IClone;
			IGeometry pGeom = pClone.Clone() as IGeometry;

			ITransform2D transform2D = pGeom as ITransform2D;
			transform2D.Move(_ptCenter.X, _ptCenter.Y);
			transform2D.Rotate(_ptCenter, 2 * Math.PI - _dir);

			return pGeom;
		}
	}

	[System.Runtime.InteropServices.ComVisible(false)]
	public class D3DPlane
	{
		public IPoint pPt;
		public double X;
		public double Y;
		public double Z;

		public double A;
		public double B;
		public double C;
		public double D;
		public D3DPlane()
		{
			pPt = new ESRI.ArcGIS.Geometry.Point() as IPoint;
			X = 0.0;
			Y = 0.0;
			Z = 0.0;

			A = 0.0;
			B = 0.0;
			C = 0.0;
			D = 0.0;
		}
	}

	[System.Runtime.InteropServices.ComVisible(false)]
	public class D3DConus
	{
		public IPoint ptCentre;
		public double Slope;

		public double A;
		public double B;
		public double C;	//a*x**2 + b*y**2 - c*z**2

		public D3DConus()
		{
			ptCentre = new ESRI.ArcGIS.Geometry.Point() as IPoint;
			Slope = 0.5;

			A = 0.0;
			B = 0.0;
			C = 0.0;
		}
	}

	[System.Runtime.InteropServices.ComVisible(false)]
	public class D3DClinder
	{
		public IPoint ptCentre;
		public double Length, Radius;

		public double u;
		public double v;
		public double w;	//a*x**2 + b*y**2 - c*z**2

		public D3DClinder()
		{
			ptCentre = new ESRI.ArcGIS.Geometry.Point() as IPoint;
			Length = 0.0;

			u = 0.0;
			v = 0.0;
			w = 1.0;
		}

		public D3DClinder(IPoint center, double lenght, double radius)
		{
			IClone pClone = center as IClone;
			ptCentre = pClone.Clone() as IPoint;
			Length = lenght;
			Radius = radius;
			u = 0.0;
			v = 0.0;
			w = 1.0;
		}
	}

	[System.Runtime.InteropServices.ComVisible(false)]
	public class D3DPolygon
	{
		public IPolygon Polygon;
		public D3DPlane Plane;
		public CodeObstacleArea codeObstacleArea;
		public ObstacleArea2 obstacleArea2;

		public bool IsComplex;

		public D3DPolygon()
		{
			Polygon = new Polygon() as IPolygon;
			Plane = new D3DPlane();
			codeObstacleArea = CodeObstacleArea.AREA2;
			obstacleArea2 = ObstacleArea2.NotArea2;
			IsComplex = false;
		}
	}

	[System.Runtime.InteropServices.ComVisible(false)]
	public class D3DComplex : D3DPolygon
	{
		public IPolygon ConusPolygon;
		public D3DConus Conus;

		public D3DComplex()
		{
			ConusPolygon = new Polygon() as IPolygon;
			Conus = new D3DConus();
			IsComplex = true;
		}
	}

	[System.Runtime.InteropServices.ComVisible(false)]
	public struct Area2DATA
	{
		public string RWYName;
		public List<D3DPolygon> Area2;
		public List<IElement> Area2elements;
		public List<ObstacleType> ObstacleList;
	}

	[System.Runtime.InteropServices.ComVisible(false)]
	public class ObstaclePlaneReleation
	{
		public D3DPlane LocalPlane;
		public double Hsurface;
		public double hPent;
		public CodeObstacleArea codeObstacleArea;
		public ObstacleArea2 obstacleArea2;
		public bool Ignored;
	}
}
