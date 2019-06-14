using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CDOTMA.Drawings;
using CDOTMA.CoordinatSystems;
using CDOTMA.Geometries;

namespace CDOTMA.Controls
{
	public interface ILayer
	{
		CoordinatSystem CS { get; set; }

		string Name { get; set; }
		int Count { get; }
		GeoAPI.Geometries.OgcGeometryType GeometryType { get; }
		GeoAPI.Geometries.OgcGeometryType GetGeometryType();
		GeoAPI.Geometries.Envelope Extend { get; }

		NetTopologySuite.Geometries.Geometry GetGeometry(int i);

		bool Visible { get; set; }
		IElement Element { get; set; }
		CoordinatSystem ViewCS { get; set; }
		NetTopologySuite.Geometries.Geometry GetPrjGeometry(int i);
	}

	//public class Layer<T> : List<T> where T : NetTopologySuite.Geometries.Geometry, new() 
	//{
	//    protected virtual string Name { get; set; }
	//    protected virtual GeoAPI.Geometries.OgcGeometryType GeometryType { get; }
	//    protected virtual GeoAPI.Geometries.OgcGeometryType GetGeometryType();
	//}
	//public class GeometryLayer<T> : Layer<T> where T : new()

	//public class GeometryLayer<T> :  List<T> where T : NetTopologySuite.Geometries.Geometry, new()

	//public class GeomList<T> : List<T> where T : NetTopologySuite.Geometries.Geometry, new()
	//{
	//    public GeoAPI.Geometries.OgcGeometryType GetGeometryType()
	//    {
	//        T ject = new T();
	//        return ((NetTopologySuite.Geometries.Geometry)ject).OgcGeometryType;
	//    }
	//}

	//public abstract class LayerBase<T> : IList<T>, ILayer where T : NetTopologySuite.Geometries.Geometry
	//{

	//}

	public class Layer<T> : List<T>, ILayer where T : NetTopologySuite.Geometries.Geometry
	{
		public Layer(string name)
		{
			_name = name;
			_extend = new GeoAPI.Geometries.Envelope();
			_projected = new List<T>();

			CS = null;
			_ViewCS = null;
		}

		string _name;
		public string Name
		{
			get { return _name; }
			set { _name = value; }
		}

		List<T> _projected;

		public CoordinatSystem CS { get; set; }

		CoordinatSystem _ViewCS;
		public CoordinatSystem ViewCS
		{
			get { return _ViewCS; }
			set
			{
				if (_ViewCS != value)
				{
					_ViewCS = value;
					_projected.Clear();
					base.ForEach(AddProcjected);
				}
			}
		}

		void AddProcjected(T geom)
		{
			T prj = (T)geom.Clone();
			((NetTopologySuite.Geometries.Geometry)prj).Project(CS, _ViewCS);
			_projected.Add(prj);
		}

		public bool Visible { get; set; }

		public GeoAPI.Geometries.OgcGeometryType GeometryType
		{
			get
			{
				return GetGeometryType();
			}
		}

		//int Count { get { return base.Count;} }

		public IElement Element { get; set; }

		GeoAPI.Geometries.Envelope _extend;
		public GeoAPI.Geometries.Envelope Extend { get { return _extend; } }

		public GeoAPI.Geometries.OgcGeometryType GetGeometryType()
		{
			if (base.Count > 0)
			{
				T ject = base[0];
				return ((NetTopologySuite.Geometries.Geometry)ject).OgcGeometryType;
			}

			if (typeof(T) == typeof(NetTopologySuite.Geometries.LineString))
				return GeoAPI.Geometries.OgcGeometryType.LineString;

			if (typeof(T) == typeof(NetTopologySuite.Geometries.Point))
				return GeoAPI.Geometries.OgcGeometryType.Point;

			return GeoAPI.Geometries.OgcGeometryType.Point;
		}

		public new void Add(T item)
		{
			if (item == null)
				return;

			_extend.ExpandToInclude(item.EnvelopeInternal);
			base.Add(item);
			AddProcjected(item);
		}

		public NetTopologySuite.Geometries.Geometry GetGeometry(int i)
		{
			if (i < 0 || i >= base.Count)
				return null;

			return base[i];
		}

		public NetTopologySuite.Geometries.Geometry GetPrjGeometry(int i)
		{
			if (i < 0 || i >= _projected.Count)
				return null;

			return _projected[i];
		}
	}

	public class LegLayer : List<LegPoint>, ILayer 	//public class LegLayer<T> : List<T>, ILayer where T : TraceLeg
	{
		public LegLayer(string name)
		{
			_name = name;
			_extend = new GeoAPI.Geometries.Envelope();
			_projected = new List<LegPoint>();

			CS = null;
			_ViewCS = null;
		}

		string _name;
		public string Name
		{
			get { return _name; }
			set { _name = value; }
		}

		List<LegPoint> _projected;

		public CoordinatSystem CS { get; set; }

		CoordinatSystem _ViewCS;
		public CoordinatSystem ViewCS
		{
			get { return _ViewCS; }
			set
			{
				if (_ViewCS != value)
				{
					_ViewCS = value;
					_projected.Clear();
					base.ForEach(AddProjcected);
				}
			}
		}

		void AddProjcected(LegPoint geom)
		{
			LegPoint prj = geom.Clone();

			prj.pPtPrj = (NetTopologySuite.Geometries.Point)prj.pPtGeo.Clone();
			prj.pPtPrj.Project(CS, _ViewCS);

			foreach (var trLeg in prj.legs)
			{
				if (trLeg.pProtectArea != null)
					trLeg.pProtectArea.Project(CS, _ViewCS);

				if (trLeg.PathGeomPrj != null)
				{
					trLeg.PathGeomPrj = (NetTopologySuite.Geometries.MultiLineString)trLeg.PathGeomGeo.Clone(); 
					trLeg.PathGeomPrj.Project(CS, _ViewCS);
				}
			}

			//((NetTopologySuite.Geometries.Geometry)prj).Project(CS, _ViewCS);
			//_projected.Add(prj);
		}

		public bool Visible { get; set; }

		public GeoAPI.Geometries.OgcGeometryType GeometryType
		{
			get
			{
				return GetGeometryType();
			}
		}

		//int Count { get { return base.Count;} }

		public IElement Element { get; set; }

		GeoAPI.Geometries.Envelope _extend;
		public GeoAPI.Geometries.Envelope Extend { get { return _extend; } }

		public GeoAPI.Geometries.OgcGeometryType GetGeometryType()
		{
			return GeoAPI.Geometries.OgcGeometryType.GeometryCollection;
		}

		public new void Add(LegPoint item)
		{
			_extend.ExpandToInclude(item.pPtGeo.Coordinate);
			base.Add(item);
			AddProjcected(item);
		}

		public NetTopologySuite.Geometries.Geometry GetGeometry(int i)
		{
			return null;
		}

		public NetTopologySuite.Geometries.Geometry GetPrjGeometry(int i)
		{
			return null;
		}
	}

}
