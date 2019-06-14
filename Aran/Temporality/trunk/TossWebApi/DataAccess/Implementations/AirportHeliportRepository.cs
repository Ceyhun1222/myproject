using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Aran.Aim;
using Aran.Aim.Data;
using Aran.Aim.Data.Filters;
using Aran.Aim.Features;
using Aran.Geometries;
using Aran.Temporality.Common.Id;
using AutoMapper;
using GeoJSON.Net.Feature;
using TossWebApi.DataAccess.Interfaces;
using TossWebApi.Models.DTO;
using TossWebApi.Utils;

namespace TossWebApi.DataAccess.Implementations
{
    public class AirportHeliportRepository : IAirportHeliportRepository
    {
        private readonly ITossServicesManager _tossServiceManager;

        public AirportHeliportRepository(ITossServicesManager tossServicesManager)
        {
            _tossServiceManager = tossServicesManager;
        }

        public AirportHeliportDto GetAirportHeliportByIdentifierDtos(int workPackage, Guid adhpIdentifier)
        {
            var airportHeliport = GetAirportHeliports(workPackage, adhpIdentifier);

            if (airportHeliport == null)
                return null;

            return Mapper.Map<AirportHeliportDto>(airportHeliport.FirstOrDefault());
        }

        public IEnumerable<AirportHeliportDto> GetAirportHeliportDtos(int workPackage)
        {
            var airportHeliports = GetAirportHeliports(workPackage);

            if (airportHeliports == null)
                return null;

            return airportHeliports.Select(oa => Mapper.Map<AirportHeliportDto>(oa));
        }

        public FeatureCollection GetAirportHeliportFeatureCollection(int workPackage)
        {
            var airportHeliports = GetAirportHeliports(workPackage);

            if (airportHeliports == null)
                return null;

            var airportHeliportFeatures = airportHeliports.Select(ah => Mapper.Map<GeoJSON.Net.Feature.Feature>(ah)).ToList();

            return new FeatureCollection(airportHeliportFeatures);
        }

        public Point GetAirportHeliportPoint(int workPackage, Guid adhpIdentifier)
        {
            var airportHeliport = GetAirportHeliports(workPackage, adhpIdentifier);

            if (airportHeliport == null)
                return null;

            return airportHeliport.FirstOrDefault().ARP?.Geo;
        }

        public IEnumerable<AirportHeliport> GetAirportHeliports(int workPackage, Guid? adhpIdentifier = null)
        {
            if (_tossServiceManager == null)
                throw new Exception("Toss Service is empty");

            var tossService = _tossServiceManager.GetDefaultTemporalityService();

            var date = DateTime.Now;
            if (workPackage != 0)
            {
                var noaixmService = _tossServiceManager.GetDefaultNoAixmDataService();
                var privateSlot = noaixmService.GetPrivateSlotById(workPackage);

                if (privateSlot == null)
                    return null;

                date = privateSlot.PublicSlot.EffectiveDate;
            }

            var featureId = new FeatureId
            {
                FeatureTypeId = (int)FeatureType.AirportHeliport,
                WorkPackage = workPackage,
                Guid = adhpIdentifier
            };

            var projection = Projection.Include("Identifier", "Name", "Designator", "ARP.Geo");

            var result = tossService.GetActualDataByDate(featureId, false, date, projection: projection);

            if (result != null && result.Count > 0)
                return result.Select(t => (AirportHeliport)t.Data.Feature);

            return null;
        }
        
    }
}