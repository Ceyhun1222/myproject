using System;
using System.Collections.Generic;
using System.Text;
using ARAN.Contracts.Registry;
using ARAN.Common;
using ARAN.GeometryClasses;

namespace ARAN.Contracts.GeometryOperators
{
	#region Enums

	public enum SpatialReferenceGeoType
	{
		srgtWGS1984 = 1,
		srgtKrasovsky1940,
		srgtNAD1983
	};

	public enum SpatialReferenceType
	{
		srtGeographic = 1,
		srtMercator,
		srtTransverse_Mercator,
		srtGauss_Krueger
	};

	public enum SpatialReferenceParamType
	{
		srptFalseEasting = 1,
		srptFalseNorthing,
		srptScaleFactor,
		srptAzimuth,
		srptCentralMeridian,
		srptLatitudeOfOrigin,
		srptLongitudeOfCenter
	};

	public enum SpatialReferenceUnit
	{
		sruMeter = 1,
		sruFoot,
		sruNauticalMile,
		sruKilometer
	};

	public enum GeometryCommands
	{
		gcUnion = 1,
		gcConvexHull,		// 2
		gcCut,				// 3
		gcIntersect,		// 4
		gcBoundary,			// 5
		gcBuffer,			// 6

		gcDifference,		// 7

		gcGetNearestPoint,	// 8
		gcGetDistance,		// 9

		gcContains,			// 10
		gcCrosses,			// 11
		gcDisjoint,			// 12
		gcEquals,			// 13
		gcGeoTransformations
	};

	#endregion

    public class Ellipsoid
    {
        public SpatialReferenceGeoType SrGeoType { get; set; }
        public double SemiMajorAxis { get; set; }
        public double Flattening { get; set; }
        public bool IsValid { get; set; }

        public Ellipsoid()
        {
            SrGeoType = SpatialReferenceGeoType.srgtWGS1984;
            SemiMajorAxis = 0;
            Flattening = 0;
            IsValid = false;
        }

        public void Pack(int handle)
        {
            Registry_Contract.PutInt32(handle, (int)SrGeoType);
            Registry_Contract.PutDouble(handle, SemiMajorAxis);
            Registry_Contract.PutDouble(handle, Flattening);
            Registry_Contract.PutBool(handle, IsValid);
        }

        public void Unpack(int handle)
        {
            SrGeoType = (SpatialReferenceGeoType)(Registry_Contract.GetInt32(handle));
            SemiMajorAxis = Registry_Contract.GetDouble(handle);
            Flattening = Registry_Contract.GetDouble(handle);
            IsValid = Registry_Contract.GetBool(handle);
        }

    }

	public class SpatialReferenceParam : PandaItem
	{
		public SpatialReferenceParam()
			: base()
		{
			SrParamType = SpatialReferenceParamType.srptFalseEasting;
			Value = 0;
		}

		public SpatialReferenceParam(SpatialReferenceParamType aSRParamType, double aValue)
			: base()
		{
			SrParamType = aSRParamType;
			Value = aValue;
		}

		public void Pack(int handle)
		{
			Registry_Contract.PutInt32(handle, (int)SrParamType);
			Registry_Contract.PutDouble(handle, Value);
		}

		public void UnPack(int handle)
		{
			SrParamType = (SpatialReferenceParamType)(Registry_Contract.GetInt32(handle));
			Value = Registry_Contract.GetDouble(handle);
		}

		public void Assign(PandaItem src)
		{
			SrParamType = ((SpatialReferenceParam)src).SrParamType;
			Value = ((SpatialReferenceParam)src).Value;
		}

		public object Clone()
		{
			SpatialReferenceParam result = new SpatialReferenceParam();
			result.Assign(this);
			return result;
		}

		public SpatialReferenceParamType SrParamType{get;set;}
        public double Value { get; set; }
	}

	public class SpatialReference
	{
		public SpatialReference()
		{
			_ellipsoid = new Ellipsoid();
			_paramList = new PandaList<SpatialReferenceParam>();
			SpatialReferenceType = SpatialReferenceType.srtGeographic;
			SpatialReferenceUnit = SpatialReferenceUnit.sruMeter;
		}

		public void Pack(int handle)
		{
			Registry_Contract.PutString(handle, Name);
			Registry_Contract.PutInt32(handle, (int)SpatialReferenceType);
			Registry_Contract.PutInt32(handle, (int)SpatialReferenceUnit);

			Ellipsoid.Pack(handle);
			ParamList.Pack(handle);
		}

		public void UnPack(int handle)
		{
			Name = Registry_Contract.GetString(handle);
			SpatialReferenceType = (SpatialReferenceType)Registry_Contract.GetInt32(handle);
			SpatialReferenceUnit = (SpatialReferenceUnit)Registry_Contract.GetInt32(handle);

			Ellipsoid.Unpack(handle);
			ParamList.UnPack(handle);
		}

		public Ellipsoid Ellipsoid
		{
			get
			{
				return _ellipsoid;
			}
		}

		public PandaList<SpatialReferenceParam> ParamList
		{
			get
			{
				return _paramList;
			}
		}

		public double this[SpatialReferenceParamType param]
		{

			get
			{
				SpatialReferenceParam PandaItem;
				for (int i = 0; i < ParamList.Count(); i++)
				{
					PandaItem = ParamList[i];
					if (PandaItem.SrParamType == param)
						return PandaItem.Value;
				}
				return 0;
			}

			set
			{
				SpatialReferenceParam PandaItem;
				for (int i = 0; i < ParamList.Count(); i++)
				{
					PandaItem = ParamList[i];
					if (PandaItem.SrParamType == param)
						PandaItem.Value = value;
				}
			}
		}

		public string Name{get;set;}
        public SpatialReferenceType SpatialReferenceType { get; set; }
		public SpatialReferenceUnit SpatialReferenceUnit{get;set;}

		private Ellipsoid _ellipsoid;
		private PandaList<SpatialReferenceParam> _paramList;
	}

	public class GeometryOperators
	{
		public GeometryOperators()
		{
			try
			{
				_handle = Registry_Contract.GetInstance("GeometryOperatorsService");
			}
			catch
			{
				_handle = 0;
			}
		}

		~GeometryOperators()
		{
			try
			{
				Registry_Contract.FreeInstance(_handle);
			}
			catch
			{
			}
		}

		public bool IsValid()
		{
			return (_handle != 0);
		}

		private int _handle;

		// #region Topological Operators
		public Geometry ConvexHull(Geometry geom)
		{
			Registry_Contract.BeginMessage(_handle, (Int32)GeometryCommands.gcConvexHull);
			Geometry.PackGeometry(_handle, geom);
			Registry_Contract.EndMessage(_handle);
			return Geometry.UnpackGeometry(_handle);
		}

		public Geometry Boundary(Geometry geom)
		{
			Registry_Contract.BeginMessage(_handle, (Int32)GeometryCommands.gcBoundary);
			Geometry.PackGeometry(_handle, geom);
			Registry_Contract.EndMessage(_handle);
			return Geometry.UnpackGeometry(_handle);
		}

		public Geometry Buffer(Geometry geom, double width)
		{
			Registry_Contract.BeginMessage(_handle, (Int32)GeometryCommands.gcBuffer);
			Geometry.PackGeometry(_handle, geom);
			Registry_Contract.PutDouble(_handle, width);
			Registry_Contract.EndMessage(_handle);
			return Geometry.UnpackGeometry(_handle);
		}

		public Geometry UnionGeometry(Geometry geom1, Geometry geom2)
		{
			Registry_Contract.BeginMessage(_handle, (Int32)GeometryCommands.gcUnion);
			Geometry.PackGeometry(_handle, geom1);
			Geometry.PackGeometry(_handle, geom2);
			Registry_Contract.EndMessage(_handle);
			return Geometry.UnpackGeometry(_handle);
		}

		public Geometry Intersect(Geometry geom1, Geometry geom2)
		{
			Registry_Contract.BeginMessage(_handle, (Int32)GeometryCommands.gcIntersect);
			Geometry.PackGeometry(_handle, geom1);
			Geometry.PackGeometry(_handle, geom2);
			Registry_Contract.EndMessage(_handle);
			return Geometry.UnpackGeometry(_handle);
		}

		public Geometry Difference(Geometry geom1, Geometry geom2)
		{
			Registry_Contract.BeginMessage(_handle, (Int32)GeometryCommands.gcDifference);
			Geometry.PackGeometry(_handle, geom1);
			Geometry.PackGeometry(_handle, geom2);
			Registry_Contract.EndMessage(_handle);
			return Geometry.UnpackGeometry(_handle);
		}

		public void Cut(Geometry geometry, PolyLine cutter, ref Geometry geomLeft, ref Geometry geomRight)
		{
			Registry_Contract.BeginMessage(_handle, (Int32)GeometryCommands.gcCut);
			Geometry.PackGeometry(_handle, geometry);
			Geometry.PackGeometry(_handle, cutter);
			Registry_Contract.EndMessage(_handle);

			geomLeft = Geometry.UnpackGeometry(_handle);
			geomRight = Geometry.UnpackGeometry(_handle);
		}
		// #endregion

		// #region Proximity Operators
		public Point GetNearestPoint(Geometry geom, Point point)
		{
			Registry_Contract.BeginMessage(_handle, (Int32)GeometryCommands.gcGetNearestPoint);
			Geometry.PackGeometry(_handle, geom);
			Geometry.PackGeometry(_handle, point);
			Registry_Contract.EndMessage(_handle);
			return Geometry.UnpackGeometry(_handle).AsPoint;
		}

		public double GetDistance(Geometry geom, Geometry other)
		{
			Registry_Contract.BeginMessage(_handle, (Int32)GeometryCommands.gcGetDistance);
			Geometry.PackGeometry(_handle, geom);
			Geometry.PackGeometry(_handle, other);
			Registry_Contract.EndMessage(_handle);
			return Registry_Contract.GetDouble(_handle);
		}
		// #endregion

		// #region Relational Operators
		Boolean RelationalOperator(Geometry geom, Geometry other, GeometryCommands Command)
		{
			Registry_Contract.BeginMessage(_handle, (Int32)Command);
			Geometry.PackGeometry(_handle, geom);
			Geometry.PackGeometry(_handle, other);
			Registry_Contract.EndMessage(_handle);
			return Registry_Contract.GetBool(_handle);
		}

		public Boolean Contains(Geometry geom, Geometry other)
		{
			return RelationalOperator(geom, other, GeometryCommands.gcContains);
		}

		public Boolean Crosses(Geometry geom, Geometry other)
		{
			return RelationalOperator(geom, other, GeometryCommands.gcCrosses);
		}

		public Boolean Disjoint(Geometry geom, Geometry other)
		{
			return RelationalOperator(geom, other, GeometryCommands.gcDisjoint);
		}

		public Boolean Equals(Geometry geom, Geometry other)
		{
			return RelationalOperator(geom, other, GeometryCommands.gcEquals);
		}

		// #endregion

		public Geometry GeoTransformations(Geometry geom, SpatialReference fromSR, SpatialReference toSR)
		{
			Registry_Contract.BeginMessage(_handle, (Int32)GeometryCommands.gcGeoTransformations);
			Geometry.PackGeometry(_handle, geom);
			fromSR.Pack(_handle);
			toSR.Pack(_handle);
			Registry_Contract.EndMessage(_handle);
			return Geometry.UnpackGeometry(_handle);
		}
	}
}
