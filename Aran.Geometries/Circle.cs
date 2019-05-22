using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aran.Geometries;
using Aran.Package;

namespace Aran.Geometries
{
	public class Circle : SpecialGeometry
	{
		public Circle ( )
		{
			_radius = 0;
			CenterPoint = new Point ( );
		}

		public Circle ( double radius, Point centerPnt )
		{
			_radius = radius;
			CenterPoint = centerPnt;
		}

		public override void Assign ( AranObject source )
		{
			Circle sourceCircle = source as Circle;
			Radius = sourceCircle.Radius;
			CenterPoint.Assign ( sourceCircle.CenterPoint );
		}

		public override void Pack ( Package.PackageWriter writer )
		{
			writer.PutDouble ( _radius );
			CenterPoint.Pack ( writer );
		}

		public override void Unpack ( Package.PackageReader reader )
		{
			Radius = reader.GetDouble ( );
			CenterPoint.Unpack ( reader );
		}

		public override AranObject Clone ( )
		{
			Circle result = new Circle ( );
			result.Assign ( this );
			return result;
		}

		public override SpecialGeometryType SpecGeomType
		{
			get
			{
				return SpecialGeometryType.Circle;
			}
		}

		public override bool IsEmpty
		{
			get
			{
				throw new NotImplementedException ( );
			}
		}

		public Point CenterPoint
		{
			get
			{
				return _centerPnt;
			}
			set
			{
				_centerPnt = value.Clone ( ) as Point;
			}
		}

		public double Radius
		{
			get
			{
				return _radius;
			}
			set
			{
				_radius = value;
			}
		}

		public override Point Centroid
		{
			get
			{
				return CenterPoint;
			}
		}

		public override void SetConstantZ(double val)
		{
			_centerPnt.Z = val;
		}

		public override void SetConstantM(double val)
		{
			_centerPnt.M = val;
		}

		public override void SetConstantT(double val)
		{
			_centerPnt.T = val;
		}

		public override MultiPoint ToMultiPoint()
		{
			MultiPoint result = new MultiPoint();
			double AngleStep = Math.PI / 180.0;

			for (int i = 0; i < 360; i++)
			{
				double iInRad = i * AngleStep;
				double X = _centerPnt.X + _radius * Math.Cos(iInRad);
				double Y = _centerPnt.Y + _radius * Math.Sin(iInRad);

				Point Pt = new Point(X, Y, _centerPnt.Z);
				Pt.M = _centerPnt.M;
				Pt.T = _centerPnt.T;

				result.Add(Pt);
			}

			return result;
		}

		private Point _centerPnt;
		private double _radius;
	}
}