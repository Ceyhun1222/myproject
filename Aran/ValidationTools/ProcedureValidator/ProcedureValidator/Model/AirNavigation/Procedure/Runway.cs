using Aran.Aim.DataTypes;
using Aran.Aim.Enums;
using Aran.Aim.Features;
using Aran.Converters;
using Aran.PANDA.Common;
using System.Collections.Generic;

namespace PVT.Model
{
    public class RunwayDirection : Feature
    {
        public Aran.Aim.Features.RunwayDirection Original { get; }
        public string Designator { get; }
        public Runway Runway { get; }
        public GeoPoint DER { get; }

        public RunwayCentreLinePoint Start
        {
            get
            {
                if (CentreLinePoints.ContainsKey(RunwayCentreLinePointType.Start))
                    return CentreLinePoints[RunwayCentreLinePointType.Start];
                else return null;
            }
        }

        public RunwayCentreLinePoint End => CentreLinePoints.ContainsKey(RunwayCentreLinePointType.End) ? CentreLinePoints[RunwayCentreLinePointType.End] : null;

        public ValDistance TODA => CentreLinePoints.ContainsKey(RunwayCentreLinePointType.Start) ? CentreLinePoints[RunwayCentreLinePointType.Start].TODA : null;


        public ValDistanceVertical DERZ => CentreLinePoints.ContainsKey(RunwayCentreLinePointType.Start) ? CentreLinePoints[RunwayCentreLinePointType.Start].DERZ : null;


        public string FullName
        {
            get
            {
                if (Runway.Original == null)
                    return Designator;
                return Runway.Designator + "|" + Designator;
            }
        }
        public IDictionary<RunwayCentreLinePointType, RunwayCentreLinePoint> CentreLinePoints { get; }

        public RunwayDirection(Aran.Aim.Features.RunwayDirection runway) : base(runway)
        {
            CentreLinePoints = new Dictionary<RunwayCentreLinePointType, RunwayCentreLinePoint>();
            if (runway != null)
            {
                Original = runway;
                Designator = runway.Designator;
                Runway = Original.UsedRunway != null ? new Runway(Engine.Environment.Current.DbProvider.GetRunway(Original.UsedRunway.Identifier)) : new Runway(null);

                FillCentreLinePoints();
                if (TODA != null && Start != null && End != null)
                {
                    double resX, resY;
                    NativeMethods.PointAlongGeodesic(Start.Geo.X, Start.Geo.Y, ConverterToSI.Convert(TODA, 0), (double)Original.TrueBearing, out resX, out resY);
                    DER = new GeoPoint { X = resX, Y = resY, Z = End.Geo.Z };
                }
            }
        }
        private void FillCentreLinePoints()
        {
            var centreLinePoints = Engine.Environment.Current.DbProvider.GetRunwayCentreLinePoints(Original.Identifier);
            foreach (var runwayCentrelinePoint in centreLinePoints)
            {
                var point = new RunwayCentreLinePoint(runwayCentrelinePoint);
                if (!point.IsEmpty)
                    CentreLinePoints.Add(point.Type, point);
            }
            if (!CentreLinePoints.ContainsKey(RunwayCentreLinePointType.THR) && CentreLinePoints.ContainsKey(RunwayCentreLinePointType.DISTHR))
            {
                var point = CentreLinePoints[RunwayCentreLinePointType.DISTHR];
                CentreLinePoints.Remove(RunwayCentreLinePointType.DISTHR);
                CentreLinePoints.Add(RunwayCentreLinePointType.THR, point);
            }
        }
    }

    public class Runway : Feature
    {
        public Aran.Aim.Features.Runway Original { get; }
        public string Designator { get; }


        public Runway(Aran.Aim.Features.Runway runway) : base(runway)
        {
            if (runway != null)
            {
                Original = runway;
                Designator = runway.Designator;
            }
            else
            {
                Designator = "";
            }
        }
    }

    public class RunwayCentreLinePoint
    {
        public bool IsEmpty { get; }
        public RunwayCentreLinePointType Type { get; }
        public GeoPoint Geo { get; }
        public ValDistanceVertical DERZ { get; }
        public Point Projection { get; private set; }
        public ValDistance TODA { get; private set; }
        public RunwayCentrelinePoint Original { get; private set; }

        public RunwayCentreLinePoint(RunwayCentrelinePoint point)
        {
            Original = point;
            IsEmpty = true;
            if (point.Role != null && point.Location != null)
            {
                IsEmpty = false;
                Geo = new GeoPoint(point.Location) {Z = ConverterToSI.Convert(point.Location.Elevation, 0)};
                DERZ = point.Location.Elevation;
                Projection = new Point(Engine.Environment.Current.Geometry.ToPrj<Aran.Geometries.Point>(new Aran.Geometries.Point(Geo.X, Geo.Y, Geo.Z)));

                switch (point.Role.Value)
                {
                    case CodeRunwayPointRole.START:
                        Type = RunwayCentreLinePointType.Start;
                        CalcToda(point);
                        break;
                    case CodeRunwayPointRole.THR:
                        Type = RunwayCentreLinePointType.THR;
                        break;
                    case CodeRunwayPointRole.END:
                        Type = RunwayCentreLinePointType.End;
                        break;
                    case CodeRunwayPointRole.DISTHR:
                        Type = RunwayCentreLinePointType.DISTHR;
                        break;
                    default:
                        IsEmpty = true;
                        break;
                }

            }

        }
        private void CalcToda(RunwayCentrelinePoint point)
        {
            foreach (var runwayDeclaredDistance in point.AssociatedDeclaredDistance)
            {
                if (runwayDeclaredDistance.Type == CodeDeclaredDistance.TODA)
                {
                    if (runwayDeclaredDistance.DeclaredValue.Count > 0) { }
                    TODA = runwayDeclaredDistance.DeclaredValue[0].Distance;
                }
            }
        }
    }

}

public enum RunwayCentreLinePointType
{
    Start,
    THR,
    End,
    DISTHR
}

