using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using GeoAPI.CoordinateSystems.Transformations;
using GeoAPI.Geometries;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NetTopologySuite.Geometries;
using Newtonsoft.Json;
using ObstacleCalculator;
using ObstacleCalculator.Domain.Enums;
using ObstacleCalculator.Domain.Models;
using ObstacleCalculator.Domain.PlaneCalculator;
using ObstacleChecker.API.Model;
using ObstacleChecker.API.Utils;
using Swashbuckle.AspNetCore.Annotations;
using Plane = NetTopologySuite.Mathematics.Plane;

namespace ObstacleChecker.API.Controllers
{
    [ApiController]
    [Route("api/obstacle-checker")]
    public class ObstacleCheckerController : ControllerBase
    {
        private readonly IMathTranformCreater _transformCreater;
        private readonly ITossHttpClient _tossClient;

        /// <exception cref="ArgumentNullException"><paramref name="transformCreater"/> is <see langword="null"/></exception>
        public ObstacleCheckerController(IMathTranformCreater transformCreater,ITossHttpClient tossClient)
        {
            _transformCreater = transformCreater;
            if (_transformCreater==null)
                throw new ArgumentNullException(nameof(transformCreater));

            _tossClient = tossClient;
        }

        /// <exception cref="HttpRequestException">The request failed due to an underlying issue such as network connectivity, DNS failure, server certificate validation or timeout.</exception>
        [HttpPost]
        [SwaggerResponse(StatusCodes.Status400BadRequest)]
        [SwaggerResponse(StatusCodes.Status200OK)]
        [Route("surfaces")]
        public async Task<ActionResult<IEnumerable<ObstacleReport>>> ObstacleReport(
            ObstacleReportRequest obstacleReportRequest)
        {
            var surfaces = await _tossClient
                .GetObstacleAreas(obstacleReportRequest.AdhpIdentifier, obstacleReportRequest.WorkPackage)
                .ConfigureAwait(false);

            if (surfaces == null)
                return BadRequest("ObstacleAreas not found");

            surfaces = GetAnnex14Surfaces(surfaces);
            var tranform = await _transformCreater.CreateTransformOnAdhpAsync(obstacleReportRequest.AdhpIdentifier,
                    obstacleReportRequest.WorkPackage)
                .ConfigureAwait(false);
            var planeCreaterFactory = new PlaneCreaterFactory(surfaces, tranform);

            var geometryFactory = new Factory.GeometryFactory(tranform);

            IGeometry obstacleGeometryInPrj = geometryFactory.CreateGeometryInPrj(obstacleReportRequest);
            if (obstacleReportRequest.HorizontalAccuracy>0)
                obstacleGeometryInPrj =obstacleGeometryInPrj.Buffer(obstacleReportRequest.HorizontalAccuracy);

            var calculationParametrs = new CalculationRequestParametrs()
            {
                Geo = obstacleGeometryInPrj,
                Elevation = obstacleReportRequest.Elevation,
                Height = obstacleReportRequest.Height,
                VerticalAccuracy = obstacleReportRequest.VerticalAccuracy,
                HorizontalAccuracy = obstacleReportRequest.HorizontalAccuracy
            };
            return Ok(ObstacleReports(planeCreaterFactory.GetPlanes(), calculationParametrs));
        }

        private IEnumerable<SurfaceBase> GetAnnex14Surfaces(IEnumerable<SurfaceBase> surfaces)
        {
            return surfaces.Where(surface =>
            {
                switch (surface.Type)
                {
                    case CodeObstacleArea.OTHER_INNERHORIZONTAL:
                    case CodeObstacleArea.OTHER_CONICAL:
                    case CodeObstacleArea.OTHER_OUTERHORIZONTAL:
                    case CodeObstacleArea.OTHER_APPROACH:
                    case CodeObstacleArea.OTHER_INNERAPPROACH:
                    case CodeObstacleArea.OTHER_TAKEOFFCLIMB:
                    case CodeObstacleArea.OTHER_TRANSITIONAL:
                    case CodeObstacleArea.OTHER_INNERTRANSITIONAL:
                    case CodeObstacleArea.OTHER_BALKEDLANDING:
                    case CodeObstacleArea.OTHER_AREA2A:
                    case CodeObstacleArea.OTHER_TAKEOFFFLIGHTPATHAREA:
                    case CodeObstacleArea.OTHER_AREA2B:
                    case CodeObstacleArea.OTHER_AREA2C:
                    case CodeObstacleArea.OTHER_AREA2D:
                        return true;
                    default:
                        return false;
                }
            });
        }

        private List<ObstacleReport> ObstacleReports(
            IEnumerable<IPlaneSurface> surfacePlanes,
            CalculationRequestParametrs calculationRequestParametrs)
        {
            var reports = new List<ObstacleReport>();
            foreach (var surfacePlane in surfacePlanes)
            {
                try
                {
                    var obsReport =
                        surfacePlane.CalculateObstacleReport(calculationRequestParametrs);
                    if (obsReport != null)
                        reports.Add(obsReport);
                }
                catch (Exception e)
                {
                    //Should log the exception
                    Console.WriteLine(e);
                    //throw;
                }
            }

            return reports;
        }

    }
}