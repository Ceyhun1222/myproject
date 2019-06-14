using Aran.Aim.Features;
using GeoJSON.Net.Feature;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TossWebApi.Models.DTO;

namespace TossWebApi.DataAccess.Interfaces
{
    public interface IRunwayRepository
    {
        IEnumerable<RunwayDto> GetRunwayDtos(int workPackage, Guid ahdpIdentifier);    

        IEnumerable<Runway> GetRunways(int workPackage, Guid ahdpIdentifier);

        Runway GetRunway(int workPackage, Guid rwyIdentifier);
    }
}
