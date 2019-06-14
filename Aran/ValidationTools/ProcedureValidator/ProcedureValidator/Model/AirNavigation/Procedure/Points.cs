using System;
using System.Collections.Generic;
using Aran.Aim.Features;
using Aran.Aim.Objects;

namespace PVT.Model
{
    public class SegmentPoint
    {
        public SegmentPoint(Aran.Aim.Features.SegmentPoint point)
        {
            Original = point;
            FlyOver = point.FlyOver ?? false;
            if (point.PointChoice != null)
            {
                PointChoice = new SignificantPoint(point.PointChoice);
            }


            var pointRefs = point.FacilityMakeup;
            if (pointRefs != null)
            {
                foreach (var fac in pointRefs)
                {
                    if (fac.FacilityAngle != null)
                    {
                        foreach (var angleUse in fac.FacilityAngle)
                        {
                            if (angleUse.TheAngleIndication != null)
                            {
                                var angleInd = Engine.Environment.Current.DbProvider.GetFeature(Aran.Aim.FeatureType.AngleIndication, angleUse.TheAngleIndication.Identifier) as Aran.Aim.Features.AngleIndication;
                                if (angleInd?.PointChoice?.NavaidSystem != null)
                                {
                                    var nv = Engine.Environment.Current.DbProvider.GetFeature(Aran.Aim.FeatureType.Navaid, angleInd.PointChoice.NavaidSystem.Identifier) as Aran.Aim.Features.Navaid;
                                    if (nv != null)
                                        if (angleUse.AlongCourseGuidance == true)
                                            Guidance = new Navaid(nv);
                                        else
                                            Intersection = new Navaid(nv);
                                }
                            }
                        }
                    }

                    if (fac.FacilityDistance != null)
                    {
                        foreach (FeatureRefObject featureRefObject in fac.FacilityDistance)
                        {

                            var distInd = Engine.Environment.Current.DbProvider.GetFeature(Aran.Aim.FeatureType.DistanceIndication, featureRefObject.Feature.Identifier) as Aran.Aim.Features.DistanceIndication;
                            if (distInd?.PointChoice?.NavaidSystem != null)
                            {
                                var nv = Engine.Environment.Current.DbProvider.GetFeature(Aran.Aim.FeatureType.Navaid, distInd.PointChoice.NavaidSystem.Identifier) as Aran.Aim.Features.Navaid;
                                if (nv != null)
                                    Intersection = new Navaid(nv);
                            }
                        }
                    }
                }
            }

            if(Intersection != null)
                if (!Navaids.ContainsKey(Intersection.Identifier))
                    Navaids.Add(Intersection.Identifier, Intersection);
            if(Guidance != null)
                if (!Navaids.ContainsKey(Guidance.Identifier))
                    Navaids.Add(Guidance.Identifier, Guidance);

            if (point.PointChoice?.NavaidSystem != null)
            {
                var nv = Engine.Environment.Current.DbProvider.GetFeature(Aran.Aim.FeatureType.Navaid, point.PointChoice.NavaidSystem.Identifier) as Aran.Aim.Features.Navaid;
                if (nv != null)
                {
                    var navaid = new Navaid(nv);
                    if(!Navaids.ContainsKey(navaid.Identifier))
                        Navaids.Add(navaid.Identifier, navaid);
                }
            }
        }

        public Aran.Aim.Features.SegmentPoint Original { get; }
        public SignificantPoint PointChoice { get; }
        public Navaid Intersection { get; }
        public Navaid Guidance { get; }
        public bool FlyOver { get; }
        public string Name => PointChoice?.Name;
        public GeoPoint Geo => PointChoice?.Geo;
        public Dictionary<Guid, Navaid> Navaids { get; } = new Dictionary<Guid, Navaid>();

        public override bool Equals(object obj)
        {
            SegmentPoint point= obj as SegmentPoint;
            if (point == null)
                return false;
            if (Name == null)
                return point.Name == null;
            return Name.Equals(point.Name);
        }

        public override string ToString()
        {
            return Name;
        }
    }

    public class TerminalSegmentPoint : SegmentPoint
    {
        public string Role { get; }
        public TerminalSegmentPoint(Aran.Aim.Features.TerminalSegmentPoint point) : base(point)
        {
            var original = Original as Aran.Aim.Features.TerminalSegmentPoint;
            if (original?.Role != null)
                Role = original.Role.ToString();
        }

    }

    public class SignificantPoint
    {

        public GeoPoint Geo { get; }
        public Point ProjectedLocation { get; }
        public Aran.Aim.Features.SignificantPoint Original { get; }
        public DesignatedPoint DesignatedPoint { get; }
        public Guid Identifier { get; }
        public Navaid Navaid { get; }
        public bool FlyOver { get; private set; }
        public double X { get;  }
        public double Y { get;  }
        public double Z { get;  }
        public SignificantPointType Type { get; }
        public bool IsEmpty { get; }

        public string Name => DesignatedPoint != null ? DesignatedPoint.Name : Navaid?.FullName;

        public SignificantPoint(Aran.Aim.Features.SignificantPoint point)
        {
            IsEmpty = true;
            Original = point;
            if (point.FixDesignatedPoint != null)
            {
                Identifier = point.FixDesignatedPoint.Identifier;
                var dp = Engine.Environment.Current.DbProvider.GetFeature(Aran.Aim.FeatureType.DesignatedPoint, point.FixDesignatedPoint.Identifier) as Aran.Aim.Features.DesignatedPoint;
                if (dp != null)
                {
                    DesignatedPoint = new DesignatedPoint(dp);
                    Geo = new GeoPoint(DesignatedPoint.Original.Location.Geo);
                    Type = SignificantPointType.DesignatedPoint;
                }
            }
            else if (point.NavaidSystem != null)
            {
                Identifier = point.NavaidSystem.Identifier;
                var dp = Engine.Environment.Current.DbProvider.GetFeature(Aran.Aim.FeatureType.Navaid, point.NavaidSystem.Identifier) as Aran.Aim.Features.Navaid;
                if (dp != null)
                {
                    Navaid = new Navaid(dp);
                    Geo = new GeoPoint(Navaid.Original.Location.Geo);
                    Type = SignificantPointType.Navaid;
                }
            }
            if (Geo != null)
            {
                IsEmpty = false;
                ProjectedLocation = new Point(Engine.Environment.Current.Geometry.ToPrj<Aran.Geometries.Point>(Geo.ToAranPoint()));
            }
        }
    }

    public enum SignificantPointType
    {
        Navaid,
        DesignatedPoint
    }
    public class DesignatedPoint : Feature
    {

        public Aran.Aim.Features.DesignatedPoint Original { get; }
        public string Name { get; }
        public GeoPoint Geo { get; }
        public DesignatedPoint(Aran.Aim.Features.DesignatedPoint point):base(point)
        {
            Original = point;
            if (Original.Designator != null)
                Name = Original.Designator;
            else if (Original.Name != null)
                Name = Original.Name;
            if (Original.Location?.Geo != null)
                Geo = new GeoPoint { X = Original.Location.Geo.X, Y = Original.Location.Geo.Y, Z = Original.Location.Geo.Z };

        }
    }

    public class Navaid:Feature
    {
        public Aran.Aim.Features.Navaid Original { get; }
        public string Name { get; }
        public string FullName { get; }
        public GeoPoint Geo { get; }
        public Navaid(Aran.Aim.Features.Navaid navaid):base(navaid)
        {
            Original = navaid;
            if (Original.Designator != null)
                Name = Original.Designator;
            else if (Original.Name != null)
                Name = Original.Name;
            if (Name != null)
                FullName = Name + "\\" + Original.Type;
            if (Original.Location?.Geo != null)
                Geo = new GeoPoint { X = Original.Location.Geo.X, Y = Original.Location.Geo.Y, Z = Original.Location.Geo.Z };

        }
    }
}
