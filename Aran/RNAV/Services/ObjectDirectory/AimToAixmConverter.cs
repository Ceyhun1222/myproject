using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ARAN.AIXMTypes;
using Aran.Converters;
using Aran.Aim.DataTypes;
using ARAN.Contracts.GeometryOperators;
using Aran.Aim.Data.Filters;
using Aran.Aim.Data;
using ARAN.Common;

namespace ObjectDirectory
{
    public class AimToAixmConverter
    {
        private GeometryOperators _geomOperators;
        public AimToAixmConverter()
        {
            _geomOperators = new GeometryOperators();
        }

        public Ahp ToAirportHeliport(Aran.Aim.Features.AirportHeliport aimAdhp)
        {
            try
            {
                Ahp ahp = new Ahp();
                ahp.SetAIXMId(aimAdhp.Identifier.ToString());
                ahp.SetId(aimAdhp.Id.ToString());
                ahp.SetName(aimAdhp.Designator);
                if (aimAdhp.FieldElevationAccuracy !=null)
                    ahp.ElevAccuracy = ConverterFromSI.Convert(aimAdhp.FieldElevationAccuracy.Uom, aimAdhp.FieldElevationAccuracy.Value, 0);
                if (aimAdhp.FieldElevation !=null)
                    ahp.Elevation = ConverterFromSI.Convert(aimAdhp.FieldElevation.Uom, aimAdhp.FieldElevation.Value, 0);
                ahp.MagVar = aimAdhp.MagneticVariation.HasValue?aimAdhp.MagneticVariation.Value:0;
                if (aimAdhp.ReferenceTemperature!=null)
                    ahp.TemPerature = ConverterFromSI.Convert(aimAdhp.ReferenceTemperature.Uom, aimAdhp.ReferenceTemperature.Value, 0);
               
                ahp.SetRemark("");
                ahp.SetTag(0);
                if (aimAdhp.ARP != null)
                {
                    ARAN.GeometryClasses.Point ptGeo = new ARAN.GeometryClasses.Point(aimAdhp.ARP.Geo.X, aimAdhp.ARP.Geo.Y);
                    ahp.GetPtGeo().Assign(ptGeo);
                    
                }
                return ahp;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public UnicalName AdhpToUnicalName(Aran.Aim.Features.AirportHeliport aimAdhp)
        {
            try
            {
                UnicalName ahp = new UnicalName();
                ahp.Id = aimAdhp.Identifier.ToString();
                ahp.Name = aimAdhp.Designator;
                ahp.Tag = "";
                return ahp;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public UnicalName RwyToUnicalName(Aran.Aim.Features.Runway aimRwy)
        {
            try
            {
                UnicalName rwy = new UnicalName();
                rwy.Id = aimRwy.Identifier.ToString();
                rwy.Name = aimRwy.Designator;
                rwy.Tag = "";
                return rwy;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public Rwy ToRunway(Aran.Aim.Features.Runway aimRunway)
        {
            Rwy rwy = new Rwy();
            
            rwy.SetAIXMId(aimRunway.Identifier.ToString());
            rwy.SetId(aimRunway.Id.ToString());
            rwy.SetName(aimRunway.Designator);
            rwy.SetRemark("");
            rwy.SetTag(0);
            rwy.Aerodrome_id = aimRunway.AssociatedAirportHeliport.Identifier.ToString();
            if (aimRunway.NominalWidth != null)
                rwy.Width = ConverterFromSI.Convert(aimRunway.NominalWidth.Uom, aimRunway.NominalWidth.Value, 0);
            else
                rwy.Width = 0;
            if (aimRunway.NominalLength != null)
                rwy.Length = ConverterFromSI.Convert(aimRunway.NominalLength.Uom, aimRunway.NominalLength.Value, 0);
            else
                rwy.Length = 0;
            return rwy;
        }

        public RwyDirection ToRunwayDirection(Aran.Aim.Features.RunwayDirection aimRwyDirection)
        {
            RwyDirection rwyDirection = new RwyDirection();
            rwyDirection.SetAIXMId(aimRwyDirection.Identifier.ToString());
            if (aimRwyDirection.ElevationTDZ !=null)
                rwyDirection.setElevTdz(ConverterFromSI.Convert(aimRwyDirection.ElevationTDZ.Uom, aimRwyDirection.ElevationTDZ.Value,0));
            if (aimRwyDirection.ElevationTDZAccuracy!=null)
                rwyDirection.SetElevTdzAccuracy(ConverterFromSI.Convert(aimRwyDirection.ElevationTDZAccuracy.Uom, aimRwyDirection.ElevationTDZAccuracy.Value,0));
            rwyDirection.SetMagBearing(aimRwyDirection.MagneticBearing.HasValue ? aimRwyDirection.MagneticBearing.Value : 0);
            rwyDirection.SetName(aimRwyDirection.Designator);
            return rwyDirection;
        }

        public SignificanPoint ToSignificantPoint(Aran.Aim.Features.NavaidEquipment navEquipment)
        {
            SignificanPoint significantPoint = null;
            switch (navEquipment.NavaidEquipmentType)
            {
                case Aran.Aim.NavaidEquipmentType.DME:
                    Dme dme = new Dme();
                    Aran.Aim.Features.DME aimDme = (Aran.Aim.Features.DME)navEquipment;
                    dme.setChannel(Convert.ToString(aimDme.Channel));
                    dme.setType(Convert.ToString(aimDme.Type));
                    if (aimDme.Displace !=null)
                        dme.setDispalcement(ConverterFromSI.Convert(aimDme.Displace.Uom,aimDme.Displace.Value,0));
                    significantPoint = dme;
                    break;
                case Aran.Aim.NavaidEquipmentType.NDB:
                    Ndb ndb = new Ndb();
                    Aran.Aim.Features.NDB aimNdb = (Aran.Aim.Features.NDB)navEquipment;
                    if (aimNdb.Frequency!=null)
                        ndb.SetFrequency(ConverterFromSI.Convert(aimNdb.Frequency.Uom, aimNdb.Frequency.Value, 0).ToString());
                    ndb.SetMagneticVariation(aimNdb.MagneticVariation.HasValue ? aimNdb.MagneticVariation.Value : 0);
                    significantPoint = ndb;
                    break;
                case Aran.Aim.NavaidEquipmentType.VOR:
                    Vor vor = new Vor();
                    Aran.Aim.Features.VOR aimVor = (Aran.Aim.Features.VOR)navEquipment;
                    if (aimVor.Frequency !=null)
                        vor.SetFrequency(ConverterFromSI.Convert(aimVor.Frequency.Uom,aimVor.Frequency.Value,0).ToString());
                    vor.SetMagneticVariation(aimVor.MagneticVariation.HasValue?aimVor.MagneticVariation.Value:0);
                    vor.SetDeclination(aimVor.Declination.HasValue ? aimVor.Declination.Value : 0);
                    switch (aimVor.ZeroBearingDirection)
	                {
                        case Aran.Aim.Enums.CodeNorthReference.MAG:
                            vor.SetVorNorthTYpe(VORNorthType.ntMagneticNorth);
                            break;
                        case Aran.Aim.Enums.CodeNorthReference.GRID:
                            vor.SetVorNorthTYpe(VORNorthType.ntGrid);
                            break;
                        case Aran.Aim.Enums.CodeNorthReference.TRUE:
                            vor.SetVorNorthTYpe(VORNorthType.ntTrueNorth);
                            break;
                        default:
                            vor.SetVorNorthTYpe(VORNorthType.ntOther);
                            break;
	                }
                    significantPoint = vor;
                    break;
                case Aran.Aim.NavaidEquipmentType.TACAN:
                    Tacan tacan = new Tacan();
                    Aran.Aim.Features.TACAN aimTacan = (Aran.Aim.Features.TACAN)navEquipment;
                    tacan.SetChannel(aimTacan.Channel.ToString());
                    tacan.SetDeclination(aimTacan.Declination.HasValue ? aimTacan.Declination.Value : 0);
                    tacan.SetMagneticVariation(aimTacan.MagneticVariation.HasValue ? aimTacan.MagneticVariation.Value : 0);
                    break;
                default:
                    return null;
            }

            significantPoint.SetAIXMId(navEquipment.Identifier.ToString());
            significantPoint.SetName(navEquipment.Designator);
            significantPoint.getGeo().Assign(new ARAN.GeometryClasses.Point(navEquipment.Location.Geo.X, navEquipment.Location.Geo.Y));
            return significantPoint;
        }

        public SignificanPoint ToSignificantPoint(Aran.Aim.Features.DesignatedPoint aimDP)
        {
            DesignatedPoint designatedPoint = new DesignatedPoint();
            designatedPoint.SetAIXMId(aimDP.Identifier.ToString());
            designatedPoint.SetName(aimDP.Designator);
            designatedPoint.getGeo().Assign(new ARAN.GeometryClasses.Point(aimDP.Location.Geo.X, aimDP.Location.Geo.Y));
            return designatedPoint;
        }

        public Obstacle ToObstacle(Aran.Aim.Features.VerticalStructure verticalStructure)
        {
            Obstacle obstacle = new Obstacle();
            obstacle.SetAIXMId(verticalStructure.Identifier.ToString());
            if (verticalStructure.Part.Count > 0 && verticalStructure.Part[0] != null && verticalStructure.Part[0].HorizontalProjection !=null) 
            {
                Aran.Aim.Features.ElevatedPoint elevPoint = verticalStructure.Part[0].HorizontalProjection.Location;
                if (elevPoint != null)
                {
                    ARAN.GeometryClasses.Point ptGeo = new ARAN.GeometryClasses.Point(elevPoint.Geo.X,elevPoint.Geo.Y);
                    obstacle.GetGeo().Assign(ptGeo);
                    ValDistanceVertical distanceVertical = elevPoint.Elevation;
                    obstacle.SetElevation(ConverterFromSI.Convert(distanceVertical.Uom, distanceVertical.Value, 0));
                }
                if (verticalStructure.Part[0].HorizontalProjection.Location.VerticalAccuracy !=null)
                {
                    ValDistance verticalAccuracy = elevPoint.VerticalAccuracy;
                    obstacle.SetElevationWithAccuracy(obstacle.GetElevation()+ ConverterFromSI.Convert(verticalAccuracy.Uom,verticalAccuracy.Value,0));
                }
            }

            obstacle.SetAIXMId(verticalStructure.Identifier.ToString());
            obstacle.SetName(verticalStructure.Name);
            return obstacle;
        }

        public ARAN.Contracts.GeometryOperators.SpatialReference ToSpatialReference(Aran.Geometries.SpatialReferences.SpatialReference aimSp)
        {
            ARAN.Contracts.GeometryOperators.SpatialReference result = new SpatialReference();
            result.Ellipsoid.Flattening = aimSp.Ellipsoid.Flattening;
            result.Ellipsoid.SemiMajorAxis = aimSp.Ellipsoid.SemiMajorAxis;
            result.Ellipsoid.IsValid = aimSp.Ellipsoid.IsValid;
            for (int i = 0; i < aimSp.ParamList.Count; i++)
            {
                result.ParamList.Add(ToSpatialParam(aimSp.ParamList[i]));
            }
            result.SpatialReferenceType = (SpatialReferenceType)aimSp.SpatialReferenceType;
            result.SpatialReferenceUnit =(SpatialReferenceUnit) aimSp.SpatialReferenceUnit;
            result.Name = aimSp.Name;
            return result;
        }

        private ARAN.Contracts.GeometryOperators.SpatialReferenceParam ToSpatialParam(Aran.Geometries.SpatialReferences.SpatialReferenceParam aimSpParam)
        {
            ARAN.Contracts.GeometryOperators.SpatialReferenceParam result = new SpatialReferenceParam();
            result.SrParamType =(SpatialReferenceParamType)aimSpParam.SRParamType;
            result.Value = aimSpParam.Value;
            return result;

        }

        public ARAN.GeometryClasses.Point ToPoint(Aran.Geometries.Point pt)
        {
            ARAN.GeometryClasses.Point result = new ARAN.GeometryClasses.Point(pt.X, pt.Y, pt.Z);
            result.T = pt.T;
            result.M = pt.M;
            return result;
        }
      
    }
}
