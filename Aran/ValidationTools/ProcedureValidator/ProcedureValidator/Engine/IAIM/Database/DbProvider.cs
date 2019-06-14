using System;
using PVT.Engine.Common.Database;
using PVT.Utils;
using Aran.Queries.Panda_2;
using Aran.Aim.Features;
using System.Collections.Generic;
using Aran.Aim;
using System.Drawing;
using System.IO.Compression;
using System.IO;
using System.Linq;

namespace PVT.Engine.IAIM.Database
{
    internal class DbProvider : CommonDbProvider, IDbProvider
    {
        readonly Aran.Aim.Data.DbProvider _dbprovider;
        private static IPandaSpecializedQPI _connection;
        private static Dictionary<Guid, List<RunwayCentrelinePoint>> _runwayCentreLinePoints;
        private static Dictionary<FeatureType, Dictionary<Guid, Feature>> _features;
        private static Dictionary<FeatureType, bool> _featuresLoad;

        public string Username => _dbprovider.CurrentUser.Name;

        public DbProvider()
        {
            var aranEnv = ((IAIMEnvironment)Environment.Current).AranEnv;
            _dbprovider = (Aran.Aim.Data.DbProvider)aranEnv.DbProvider;
            _features = new Dictionary<FeatureType, Dictionary<Guid, Feature>>();
            _featuresLoad = new Dictionary<FeatureType, bool>();
            _runwayCentreLinePoints = new Dictionary<Guid, List<RunwayCentrelinePoint>>();
        }

        public void Open()
        {
            if (IsOpen) return;
            _connection = PandaSQPIFactory.Create();
            Aran.Queries.ExtensionFeature.CommonQPI = _connection;

            _connection.Open(_dbprovider);
            IsOpen = true;
        }

        public void Load()
        {
            Load(FeatureType.InitialLeg);
            Load(FeatureType.IntermediateLeg);
            Load(FeatureType.FinalLeg);
            Load(FeatureType.MissedApproachLeg);
            Load(FeatureType.ArrivalLeg);
            Load(FeatureType.DepartureLeg);
            Load(FeatureType.DesignatedPoint);
            Load(FeatureType.DME);
            Load(FeatureType.VOR);
            Load(FeatureType.Localizer);
            Load(FeatureType.Navaid);
            Load(FeatureType.DistanceIndication);
            Load(FeatureType.AngleIndication);
            Load(FeatureType.Runway);
            Load(FeatureType.RunwayDirection);
            Load(FeatureType.AirportHeliport);
            if (_featuresLoad.ContainsKey(FeatureType.RunwayDirection))
                foreach (var runway in _features[FeatureType.RunwayDirection])
                {
                    GetRunwayCentreLinePoints(runway.Value.Identifier);
                }
        }


        public List<T> GetProcedures<T>(Guid airportIdentifier) where T : Procedure
        {
            return _connection.GetProcedureList<T>(airportIdentifier, null);
        }

        public List<HoldingAssessment> GetHoldingAssessments()
        {
            return _connection.GetFeatureList(FeatureType.HoldingAssessment)?.Cast<HoldingAssessment>().ToList();
        }

        public List<HoldingPattern> GetHoldingPatterns()
        {
            return _connection.GetFeatureList(FeatureType.HoldingPattern)?.Cast<HoldingPattern>().ToList();
        }

        public AirportHeliport GetAirport(Guid identifier)
        {
            return GetFeature(FeatureType.AirportHeliport, identifier) as AirportHeliport;
        }

        public List<RunwayCentrelinePoint> GetRunwayCentreLinePoints(Guid identifier)
        {
            if (_runwayCentreLinePoints.ContainsKey(identifier)) return _runwayCentreLinePoints[identifier];
            var runwayPoints = _connection.GetRunwayCentrelinePointList(identifier);
            if (runwayPoints == null)
                return null;
            _runwayCentreLinePoints.Add(identifier, runwayPoints);
            return _runwayCentreLinePoints[identifier];
        }

        public RunwayDirection GetRunwayDirection(Guid identifier)
        {
            return GetFeature(FeatureType.RunwayDirection, identifier) as RunwayDirection;
        }

        public Runway GetRunway(Guid identifier)
        {
            return GetFeature(FeatureType.Runway, identifier) as Runway;
        }

        public Feature GetFeature(FeatureType type, Guid identifier)
        {
            if (!_features.ContainsKey(type))
            {
                _features.Add(type, new Dictionary<Guid, Feature>());
            }
            if (!_features[type].ContainsKey(identifier) )
            {
                if (_featuresLoad.ContainsKey(type))
                    return null;
                var result = _connection.GetFeature(type, identifier);
                if (result == null)
                    return null;
                _features[type].Add(identifier, result);
            }
            return _features[type][identifier];
        }

        private void Load(FeatureType type)
        {
            if (!_features.ContainsKey(type))
            {
                _features.Add(type, new Dictionary<Guid, Feature>());
            }
            var result = _connection.GetFeatureList(type);
            if(result == null)
                return;

            var features = result.Cast<Feature>().ToList();
            foreach (var feature in features)
            {
                if (!_features[type].ContainsKey(feature.Identifier))
                {
                    _features[type].Add(feature.Identifier, feature);
                }
            }
            if(!_featuresLoad.ContainsKey(type))
                _featuresLoad.Add(type, true);
        }

        public List<Aran.Aim.Data.FeatureReport> GetFeatureReport(Guid identifier)
        {
            return _dbprovider.GetFeatureReport(identifier);
        }

        public List<Model.Screenshot> GetScreenShots(Guid uuid)
        {
            var current = _connection.GetScreenshots(uuid);
            return current.Select(t => new Model.Screenshot { Date = t.DateTime, uuid = t.Identifier.ToString(), Images = Convert(t.Images) }).ToList();
        }

        private List<Model.ScreenImage> Convert(byte[] zipped)
        {
            var result = new List<Model.ScreenImage>();
            using (var stream = new MemoryStream(zipped))
            {
                stream.Position = 0;
                using (var archive = new ZipArchive(stream))
                {
                    var entries = archive.Entries;
                    foreach (var t in entries)
                    {
                        var img = Image.FromStream(t.Open());
                        var image = new Bitmap(img);
                        result.Add(new Model.ScreenImage { BitmapImage = image.ToBitmapImage(), Image = img });
                    }
                }
            }

            return result;
        }

        public List<VerticalStructure> GetVerticalStructures(List<Guid> uuids)
        {
            FillVerticalStructures();
            var result = new List<VerticalStructure>();
            foreach (var t in uuids)
            {
                if (_features[FeatureType.VerticalStructure].ContainsKey(t))
                    result.Add((VerticalStructure)_features[FeatureType.VerticalStructure][t]);
                else
                {
                    var vertStructure = GetFeature(FeatureType.VerticalStructure, t) as VerticalStructure;
                    if (vertStructure == null) continue;
                    result.Add(vertStructure);
                }
            }
            return result;
        }

        private void FillVerticalStructures()
        {
            if (_features.ContainsKey(FeatureType.VerticalStructure)) return;
            _features.Add(FeatureType.VerticalStructure, new Dictionary<Guid, Feature>());
            var verticalStructuresList = _connection.GetVerticalStructureList();
            foreach (var t in verticalStructuresList)
                _features[FeatureType.VerticalStructure].Add(t.Identifier, t);
        }
    }


}
