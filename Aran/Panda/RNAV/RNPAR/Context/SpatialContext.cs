using Aran.Geometries.SpatialReferences;
using Aran.PANDA.Common;
using System;

namespace Aran.Panda.RNAV.RNPAR.Context
{
    class SpatialContext
    {
        private AppEnvironment _environment;
        public SpatialReferenceOperation SpatialReferenceOperation { get;}
        public SpatialReference SpatialReferenceProjection { get; }
        public SpatialReference SpatialReferenceGeo { get; }
        public Ellipsoid Ellipsoid { get; }
        public double MeanEarthRadius { get; }

        public SpatialContext(AppEnvironment environment)
        {
            _environment = environment;
            SpatialReferenceOperation = new SpatialReferenceOperation(environment.AranEnv);
            SpatialReferenceProjection = SpatialReferenceOperation.SpRefPrj;
            SpatialReferenceGeo = SpatialReferenceOperation.SpRefGeo;
            Ellipsoid = SpatialReferenceGeo.Ellipsoid;
            ARANFunctions.InitEllipsoid(Ellipsoid.SemiMajorAxis, 1.0 / Ellipsoid.Flattening);
            MeanEarthRadius = Math.Sqrt(1.0 - Ellipsoid.Flattening) * Ellipsoid.SemiMajorAxis;
        }
    }
}
