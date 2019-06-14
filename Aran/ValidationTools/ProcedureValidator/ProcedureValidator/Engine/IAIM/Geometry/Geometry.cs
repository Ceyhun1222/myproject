using System;
using PVT.Engine.Common.Geometry;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Carto;
using Aran.PANDA.Common;
using Aran.AranEnvironment;
using ESRI.ArcGIS.Geometry;
using IGeometry = PVT.Engine.Common.Geometry.IGeometry;

namespace PVT.Engine.IAIM.Geometry
{
    class Geometry :CommonGeometry, IGeometry
    {

        private IHookHelper _hookHelper;
        private IMap _map;
        private ISpatialReference _spartialReferenceProjection;
        private IProjectedCoordinateSystem _projectedCoordinateSystem;
        private IGeographicCoordinateSystem _geographicCoordinateSystem;
        private ISpheroid _spheroid;
        private ISpatialReference _spartialReferenceSpheroid;
        public SpatialReferenceOperation SpartialReferenceOpration { get; private set; }
        private IAranEnvironment _aranEnv;

        public void Init()
        {
            if (IsInit) return;

            _aranEnv = ((IAIMEnvironment)Environment.Current).AranEnv;
            _hookHelper = new HookHelper {Hook = _aranEnv.HookObject};

            _map = GetMap();
            _spartialReferenceProjection = _map.SpatialReference;
            _spartialReferenceProjection.SetZDomain(-2000.0, 14000.0);
            _spartialReferenceProjection.SetMDomain(-2000.0, 14000.0);

            if (_spartialReferenceProjection == null)
                throw new Exception("Map projection is not defined.");

            _projectedCoordinateSystem = _spartialReferenceProjection as IProjectedCoordinateSystem;

            if (_projectedCoordinateSystem == null)
                _geographicCoordinateSystem = _spartialReferenceProjection as IGeographicCoordinateSystem;
            else
                _geographicCoordinateSystem = _projectedCoordinateSystem.GeographicCoordinateSystem;

            if (_geographicCoordinateSystem == null)
                throw new Exception("Invalid Map projection.");

            _spheroid = _geographicCoordinateSystem.Datum.Spheroid;
            NativeMethods.InitEllipsoid(_spheroid.SemiMajorAxis, 1.0 / _spheroid.Flattening);

            if (_projectedCoordinateSystem != null)
                NativeMethods.InitProjection(_projectedCoordinateSystem.CentralMeridian[true], 0.0, _projectedCoordinateSystem.ScaleFactor, _projectedCoordinateSystem.FalseEasting, _projectedCoordinateSystem.FalseNorthing);
            else
                throw new Exception("Invalid Map projection.");

            var spartialReferenseFactory = new SpatialReferenceEnvironment() as ISpatialReferenceFactory2;
            _spartialReferenceSpheroid = spartialReferenseFactory.CreateGeographicCoordinateSystem((int)esriSRGeoCSType.esriSRGeoCS_WGS1984) as IGeographicCoordinateSystem;

            _spartialReferenceSpheroid.SetZDomain(-2000.0, 14000.0);
            _spartialReferenceSpheroid.SetMDomain(-2000.0, 14000.0);
            _spartialReferenceSpheroid.SetDomain(-360.0, 360.0, -360.0, 360.0);

            SpartialReferenceOpration = new SpatialReferenceOperation(_aranEnv);
            IsInit = true;
        }

        public IMap GetMap()
        {
            return _hookHelper.FocusMap;
        }

        public T ToPrj<T>(T geoGeom) where T : Aran.Geometries.Geometry
        {
            return SpartialReferenceOpration.ToPrj<T>(geoGeom);
        }

        public T ToGeo<T>(T prjGeom) where T : Aran.Geometries.Geometry
        {
            return SpartialReferenceOpration.ToGeo<T>(prjGeom);
        }
    }
}
