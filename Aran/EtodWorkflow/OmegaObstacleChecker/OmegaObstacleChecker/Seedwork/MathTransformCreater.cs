using System.Collections.Generic;
using GeoAPI.CoordinateSystems;
using GeoAPI.CoordinateSystems.Transformations;
using ObstacleChecker.API.Model;
using ProjNet.CoordinateSystems;
using ProjNet.CoordinateSystems.Transformations;
using System.Threading.Tasks;
using static ProjNet.CoordinateSystems.ProjectedCoordinateSystem;

namespace ObstacleChecker.API.Utils
{
    public class MathTransformCreater: IMathTranformCreater
    {
        private readonly ITossHttpClient _tossClient;

        public MathTransformCreater(ITossHttpClient tossClient)
        {
            _tossClient = tossClient;
        }
       
        public async Task<IMathTransform> CreateTransformOnAdhpAsync(string adhpIdentifier, int workPackage)
        {
            var adhp = await _tossClient.GetAirportHeliportDtoByIdentifier(adhpIdentifier, workPackage)
                .ConfigureAwait(false);

            var coordSystemFactory = new CoordinateSystemFactory();
            List<ProjectionParameter> parameters = new List<ProjectionParameter>();
            parameters.Add(new ProjectionParameter("latitude_of_origin", 0));
            parameters.Add(new ProjectionParameter("central_meridian", (int)adhp.Geo.Coordinates.Longitude));
            parameters.Add(new ProjectionParameter("scale_factor", 0.9996));
            parameters.Add(new ProjectionParameter("false_easting", 500000));
            parameters.Add(new ProjectionParameter("false_northing", 0));
            IProjection projection = coordSystemFactory.CreateProjection("Mercator_1SP", "Mercator_1SP", parameters);

            GeographicCoordinateSystem WGS84 = (ProjNet.CoordinateSystems.GeographicCoordinateSystem)GeographicCoordinateSystem.WGS84;

            IProjectedCoordinateSystem coordsys = coordSystemFactory.CreateProjectedCoordinateSystem("Makassar / NEIEZ", WGS84, projection,
                LinearUnit.Metre,
                new AxisInfo("East",
                    AxisOrientationEnum.East),
                new AxisInfo("North",
                    AxisOrientationEnum.
                        North));

            CoordinateTransformationFactory ctfac = new CoordinateTransformationFactory();
            ICoordinateTransformation trans = ctfac.CreateFromCoordinateSystems(WGS84, coordsys);
            return trans.MathTransform;
        }
    }
}
