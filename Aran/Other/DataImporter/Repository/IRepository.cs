using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aran.Aim.Features;

namespace DataImporter.Repository
{
    public interface IRepository
    {
        List<AirportHeliport> AirportHeliportList { get; }
        List<Runway> GetRunwayList(Guid identifier);
        List<RunwayDirection> GetRunwayDirList(Guid rwyIdentifier);
        List<RunwayCentrelinePoint> GetRunwayCntList(Guid rwyDirIdentifier);

        void SetFeature(Feature feat);
        bool Save(bool showTimePanel);
        VerticalStructure CreateVs();
        RunwayCentrelinePoint CreateRwyCnt();
    }
}
