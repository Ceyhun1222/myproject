using Aran.Aim.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TossWebApi.Models.DTO;

namespace TossWebApi.DataAccess.Interfaces
{
    public interface IRunwayDirectionRepository
    {
        IEnumerable<RunwayDirectionDto> GetRunwayDirectionDtos(int workPackage, Guid rwyIdentifier);

        IEnumerable<RunwayDirection> GetRunwayDirections(int workPackage, Guid rwyIdentifier);

        RunwayDirection GetRunwayDirection(int workPackage, Guid rwyDirectionIdentifier);

        List<RunwayDirection> GetRunwayDirections(int workPackage, List<Guid> runwayIdentifiers);
    }
}
