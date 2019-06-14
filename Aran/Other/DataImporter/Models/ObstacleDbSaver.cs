using System;
using System.Collections.Generic;
using Aran.Aim.Enums;
using Aran.Aim.Features;
using DataImporter.Repository;

namespace DataImporter.Models
{
    public interface IObstacleDbSaver
    {
        void Save(List<Obstacle> obstacleList);
    }

    public class ObstacleDbSaver : IObstacleDbSaver
    {
        private readonly IRepository _repository;

        public ObstacleDbSaver(IRepository repository)
        {
            _repository = repository;
        }

        public void Save(List<Obstacle> obstacleList)
        {
            foreach (var obstaclePt in obstacleList)
            {
                var vs = _repository.CreateVs();
                vs.Name = obstaclePt.Name;

                Aran.Aim.Enums.CodeVerticalStructure vsType;
                if (Enum.TryParse<Aran.Aim.Enums.CodeVerticalStructure>(obstaclePt.Type, out vsType))
                    vs.Type = vsType;

                if (obstaclePt.Lght.HasValue)
                    vs.Lighted = obstaclePt.Lght;

                if (obstaclePt.CodeGrp.HasValue)
                    vs.Group = obstaclePt.CodeGrp;

                if (obstaclePt.Markings.HasValue)
                    vs.MarkingICAOStandard = obstaclePt.Markings;

                var part = GetPart(obstaclePt);
                if (part == null)
                    throw new ArgumentNullException("Part is not recognized");
                vs.Part.Add(part);

                if (!string.IsNullOrEmpty(obstaclePt.RMK))
                {
                    var note = new Note();
                    note.TranslatedNote.Add(new LinguisticNote
                    {
                        Note = new Aran.Aim.DataTypes.TextNote {Value = obstaclePt.RMK}
                    });
                    vs.Annotation.Add(note);
                }
                //part.Annotation.Add(new Note { })
            }
        }

        private VerticalStructurePart GetPart(Obstacle obstaclePt)
        {
            var part = new VerticalStructurePart();
            if (obstaclePt.GeoType == ObstacleGeoType.Point)
            {
                part.HorizontalProjection = new VerticalStructurePartGeometry { Location = new ElevatedPoint() };
                var point = obstaclePt.Geo as Aran.Geometries.Point;
                if (point != null)
                {
                    part.HorizontalProjection.Location.Geo.X = point.X;
                    part.HorizontalProjection.Location.Geo.Y = point.Y;
                }
                part.HorizontalProjection.Location.Elevation =
                    new Aran.Aim.DataTypes.ValDistanceVertical(obstaclePt.Elev, UomDistanceVertical.M);
            }
            if (obstaclePt.GeoType == ObstacleGeoType.Polygon || obstaclePt.GeoType == ObstacleGeoType.Circle)
            {
                part.HorizontalProjection = new VerticalStructurePartGeometry { SurfaceExtent = new ElevatedSurface() };
                var poly = obstaclePt.Geo as Aran.Geometries.MultiPolygon;
                if (poly != null)
                    part.HorizontalProjection.SurfaceExtent.Geo.Add(poly[0]);

                part.HorizontalProjection.SurfaceExtent.Elevation =
                    new Aran.Aim.DataTypes.ValDistanceVertical(obstaclePt.Elev, UomDistanceVertical.M);
            }

            if (obstaclePt.GeoType == ObstacleGeoType.PolyLine)
            {
                part.HorizontalProjection = new VerticalStructurePartGeometry { LinearExtent = new ElevatedCurve() };
                var polyLine = obstaclePt.Geo as Aran.Geometries.MultiLineString;
                if (polyLine != null)
                    part.HorizontalProjection.LinearExtent.Geo.Add(polyLine[0]);

                part.HorizontalProjection.LinearExtent.Elevation =
                    new Aran.Aim.DataTypes.ValDistanceVertical(obstaclePt.Elev, UomDistanceVertical.M);
            }

            return part;
        }
    }
}
