
using Aran.Aim.DataTypes;
using Aran.Aim.Enums;
using System;
using System.Linq;

namespace PVT.Model
{
    public class SegmentLeg : Feature
    {
        public Aran.Aim.Features.SegmentLeg Original { get; }
        public bool NotVaild { get; private set; }
        public TerminalSegmentPoint StartPoint { get; }
        public TerminalSegmentPoint EndPoint { get; }
        public CodeSegmentPath? LegTypeARINC { get; }
        public string WaypointName { get; }
        public string WaypointRole { get; }
        public bool FlyOver { get; }
        public double? Course { get; }
        public ValDistance Distance { get; }
        public string TurnDirection { get; }
        public ValDistanceVertical Altitude { get; }
        public double? VerticalAngle { get; }
        public ValSpeed Speed { get; }
        public GeoPoint Geo { get; }
        public string Type { get; }

        public Aran.Geometries.MultiLineString NominalTrack => Original.Trajectory?.Geo;
        public ObstacleAssessmentArea PrimaryProttectedArea { get; }
        public ObstacleAssessmentArea SecondaryProttectedArea { get; }

        public SegmentLeg(Aran.Aim.Features.SegmentLeg leg) : base(leg)
        {
            
            Original = leg;
            Type = Original.SegmentLegType.ToString();
            if (leg.StartPoint != null)
            {
                StartPoint = new TerminalSegmentPoint(leg.StartPoint);
            }
            if (leg.EndPoint != null)
            {
                EndPoint = new TerminalSegmentPoint(leg.EndPoint);
                WaypointName = EndPoint.Name;
                FlyOver = EndPoint.FlyOver;
                Geo = EndPoint.Geo;
                WaypointRole = EndPoint.Role;
            }

            LegTypeARINC = leg.LegTypeARINC;
            Course = leg.Course;
            Distance = leg.Length;
            TurnDirection = leg.TurnDirection != null && leg.TurnDirection != CodeDirectionTurn.EITHER ? leg.TurnDirection.ToString() : null;
            if (leg.LowerLimitAltitude != null)
                Altitude = leg.LowerLimitAltitude;
            if (leg.SpeedLimit != null)
                Speed = leg.SpeedLimit;
            VerticalAngle = leg.VerticalAngle;

            var areas = Original.DesignSurface;
            foreach (var t in areas.Where(t => t.Surface?.Geo != null))
            {
                if (t.Type == CodeObstacleAssessmentSurface.PRIMARY)
                    PrimaryProttectedArea = new ObstacleAssessmentArea(t);
                if (t.Type == CodeObstacleAssessmentSurface.SECONDARY)
                    SecondaryProttectedArea = new ObstacleAssessmentArea(t);
            }

            NotVaild = !(coexist(NominalTrack, PrimaryProttectedArea) || coexist(NominalTrack, SecondaryProttectedArea));
        }

        public void SetValid()
        {
            NotVaild = false;
        }

        private bool coexist(Object o1, Object o2)
        {
            if (o1 != null && o2 == null)
                return false;

            return o2 == null || o1 != null;
        }
    }

 

    public enum LegType
    {
        Final,
        Intermediate,
        Initial,
        Missed,
        Departure,
        Arrival
    }
}
