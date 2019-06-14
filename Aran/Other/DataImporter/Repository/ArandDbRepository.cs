using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aran.Aim;
using Aran.Aim.Data;
using Aran.Aim.Features;
using Aran.AranEnvironment;
using Aran.Queries.Omega;

namespace DataImporter.Repository
{
    class ArandDbRepository : IRepository
    {
        private IAranEnvironment _aranEnv;
        private IOmegaQPI _omegaQpi;
        private List<AirportHeliport> _airportHeliportList;
        

        /// <exception cref="ArgumentNullException"><paramref name=""/> is <see langword="null"/></exception>
        public ArandDbRepository(IAranEnvironment aranEnv)
        {
            _aranEnv = aranEnv;
            if (_aranEnv==null)
                throw new ArgumentNullException($"Aran Envoirment is Empty");
            Open();
        }

        private void Open()
        {
            _omegaQpi = Aran.Queries.Omega.OmegaQpiFactory.Create();
            var dbProvider = _aranEnv.DbProvider as DbProvider;
            if (dbProvider==null)
                throw  new ArgumentNullException($"Database provider is empty");

            _omegaQpi.Open(dbProvider);
        }

        public List<AirportHeliport> AirportHeliportList
        {
            get
            {
                if (_airportHeliportList == null)
                {
                   var featList = _omegaQpi.GetFeatureList(FeatureType.AirportHeliport);
                   _airportHeliportList = featList?.Cast<AirportHeliport>().ToList();
                }
                return _airportHeliportList;
            }
        }

        public List<Runway> GetRunwayList(Guid adhpIdentifier)
        {
            return _omegaQpi.GetRunwayList(adhpIdentifier);
        }

        public List<RunwayDirection> GetRunwayDirList(Guid rwyIdentifier)
        {
            return _omegaQpi.GetFeatureList(Aran.Aim.FeatureType.RunwayDirection)?.
                                    Cast<RunwayDirection>().
                                    Where(rwyDir => rwyDir.UsedRunway.Identifier == rwyIdentifier).
                                    ToList();
        }

        public List<RunwayCentrelinePoint> GetRunwayCntList(Guid rwyDirIdentifier)
        {
            return _omegaQpi.GetFeatureList(Aran.Aim.FeatureType.RunwayCentrelinePoint).
                                    Cast<RunwayCentrelinePoint>().
                                    Where(allRwy => allRwy.OnRunway.Identifier == rwyDirIdentifier)
                                    .ToList();
        }

        public void SetFeature(Feature feat)
        {
            _omegaQpi.SetFeature(feat);
        }

        public bool Save(bool showTimePanel)
        {
            return _omegaQpi.Commit(showTimePanel);
        }

        public VerticalStructure CreateVs()
        {
            return _omegaQpi.CreateFeature<Aran.Aim.Features.VerticalStructure>();
        }

        public RunwayCentrelinePoint CreateRwyCnt()
        {
            return _omegaQpi.CreateFeature<Aran.Aim.Features.RunwayCentrelinePoint>();
        }


    }
}
