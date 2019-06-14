using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aran.Aim.Enums;
using Aran.Aim.Features;
using DataImporter.Repository;

namespace DataImporter.Models
{
    public interface IRunwayCntDbSaver
    {
        void Save(List<RwyCenterPoint> rwyCenterPoints,Runway rwy);
        void DecommitCenterPoints(Runway rwy);
    }

    class RunwayCntDbSaver : IRunwayCntDbSaver
    {
        private readonly IRepository _repository;

        public RunwayCntDbSaver(IRepository repository)
        {
            _repository = repository;
        }

        public void DecommitCenterPoints(Runway rwy)
        {
            var rwyDirList = _repository.GetRunwayDirList(rwy.Identifier);

            foreach (var rwyDir in rwyDirList)
            {
                var rwyCenterList = _repository.GetRunwayCntList(rwyDir.Identifier);

                foreach (var rwyCenter in rwyCenterList)
                {
                    rwyCenter.TimeSlice.FeatureLifetime.EndPosition = rwyCenter.TimeSlice.FeatureLifetime.BeginPosition;
                    _repository.SetFeature(rwyCenter);
                }
            }

            if (!_repository.Save(false))
                throw new Exception("Error when decommit runway centerline points to Database");

        }

        public void Save(List<RwyCenterPoint> rwyCenterPoints,Runway rwy)
        {
            var rwyDirList = _repository.GetRunwayDirList(rwy.Identifier);

            foreach (var runwayDirection in rwyDirList)
            {
                foreach (var rwyCenterPoint in rwyCenterPoints)
                {
                    if (!rwyCenterPoint.Checked) continue;
                    var rwyCenter = _repository.CreateRwyCnt();
                    rwyCenter.OnRunway = new Aran.Aim.DataTypes.FeatureRef(runwayDirection.Identifier);
                    rwyCenter.Role = CodeRunwayPointRole.MID;


                    if (rwyCenterPoint.ID != null && rwyCenterPoint.ID.ToLower().Contains("porog"))
                    {
                        if (runwayDirection.Designator.Contains(rwyCenterPoint.ID.Substring(rwyCenterPoint.ID.Length - 2, 2)))
                            rwyCenter.Role = CodeRunwayPointRole.THR;
                        else
                            rwyCenter.Role = CodeRunwayPointRole.END;
                    }
                    rwyCenter.Designator = rwyCenterPoint.ID;

                    rwyCenter.Location = new ElevatedPoint();
                    rwyCenter.Location.Geo.X = rwyCenterPoint.Geo.X;
                    rwyCenter.Location.Geo.Y = rwyCenterPoint.Geo.Y;
                    rwyCenter.Location.Elevation =
                        new Aran.Aim.DataTypes.ValDistanceVertical(rwyCenterPoint.Elev,
                            UomDistanceVertical.M);
                    rwyCenter.Location.GeoidUndulation =
                        new Aran.Aim.DataTypes.ValDistanceSigned(rwyCenterPoint.Geoid, UomDistance.M);
                    if (rwyCenter.Role == CodeRunwayPointRole.THR)
                    {
                        var rwyStart = _repository.CreateRwyCnt();
                        rwyStart.Role = CodeRunwayPointRole.START;
                        rwyStart.OnRunway = rwyCenter.OnRunway;
                        rwyStart.Designator = rwyCenter.Designator;
                        rwyStart.Location = rwyCenter.Location;
                    }
                   // AddGeoidUndulation(_airportType, rwyCenter);
                    //part.Annotation.Add(new Note { })
                }
            }
        }
    }
}
