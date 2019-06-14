using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ARAN.Common;
using ARAN.AIXMTypes;
using Aran.Aim.Data.Filters;
using Aran.Aim.Data;
using Aran.Aim.Enums;
using ARAN.Contracts.Settings;
using ARAN.Contracts.GeometryOperators;
using ARAN.Collection;
using Aran.Converters;
using Aran.Aim;

namespace ObjectDirectory
{
    public class Service
    {
        private IDbProvider _dbProvider;
        private TimeSliceFilter _timeSliceFilter;
        private TimeSliceInterpretationType _interPretation;
        private AimToAixmConverter _aimToAixmConverter;
        private ARAN.Contracts.GeometryOperators.GeometryOperators _geomOperators;
       

        public Service()
        {
            _timeSliceFilter = new TimeSliceFilter(DateTime.Now);
            _timeSliceFilter.QueryType = QueryType.ByEffectiveDate;
            _aimToAixmConverter = new AimToAixmConverter();
            _interPretation = TimeSliceInterpretationType.BASELINE;
            GeoSp = Global.GeoSp;
            PrjSp = _aimToAixmConverter.ToSpatialReference(Global.Env.Graphics.ViewProjection);
            _dbProvider = Global.Env.DbProvider as IDbProvider;
            _geomOperators = new GeometryOperators();
        }

        public void Connect()
        {
            //string connectionString =
            //    "Server =" + connectionInfo.PgConnection.Host +
            //    "; Port = 5432" +
            //    "; Database =" + connectionInfo.PgConnection.DbName +
            //    "; User Id = " + connectionInfo.PgConnection.User +
            //    "; Password =" + connectionInfo.PgConnection.Password + ";";

            //_dbProvider = PgProviderFactory.Create ();
            // _dbProvider.TimeSliceFilter = _timeSliceFilter;
            //_dbProvider.Open(connectionString);
        }

        public void Disconnect()
        {
            //_dbProvider.Close();
        }

        public PandaList<Ahp> GetAeodomeList(List<string> icaoPrefixList)
        {
            try
            {
                GettingResult gettinResult = null;
                PandaList<Ahp> result = new PandaList<Ahp>();
                if (icaoPrefixList.Count == 0)
                {
                    gettinResult = _dbProvider.GetVersionsOf(
                        FeatureType.AirportHeliport,
                        _interPretation,
                        default(Guid), true, null, null, null);
                }
                else
                {
                    //must be look again
                    ComparisonOps compOper = new ComparisonOps(ComparisonOpType.EqualTo, "designator", icaoPrefixList[0]);
                    OperationChoice operChoice = new OperationChoice(compOper);
                    Filter filter = new Filter(operChoice);
                    gettinResult = _dbProvider.GetVersionsOf(Aran.Aim.FeatureType.OrganisationAuthority, _interPretation, default(Guid), false, null, null, filter);

                    if (gettinResult.IsSucceed)
                    {
                        if (gettinResult.List.Count > 0)
                        {
                            compOper.PropertyName = "ResponsibleOrganisation.TheOrganisationAuthority";
                            compOper.Value = (gettinResult.List[0] as Aran.Aim.Features.OrganisationAuthority).Identifier;
                            gettinResult = _dbProvider.GetVersionsOf(Aran.Aim.FeatureType.AirportHeliport, _interPretation, default(Guid), false, null, null, filter);
                        }

                    }
                    else
                        return result;
                }
                if (gettinResult.IsSucceed)
                {
                    for (int i = 0; i < gettinResult.List.Count; i++)
                    {
                        result.Add(_aimToAixmConverter.ToAirportHeliport(gettinResult.List[i] as Aran.Aim.Features.AirportHeliport));
                    }
                }
                return result;
            }

            catch (Exception e)
            {

                throw e;
            }
        }

        public PandaList<UnicalName> GetUnicalAeodomeList(List<string> icaoPrefixList)
        {
            try
            {
                GettingResult gettinResult = null;
                PandaList<UnicalName> result = new PandaList<UnicalName>();
                if (icaoPrefixList.Count == 0)
                {
                    gettinResult = _dbProvider.GetVersionsOf(
                        FeatureType.AirportHeliport,
                        _interPretation,
                        default(Guid), true, null, null, null);
                }
                else
                {
                    //must be look again
                    ComparisonOps compOper = new ComparisonOps(ComparisonOpType.EqualTo, "designator", icaoPrefixList[0]);
                    OperationChoice operChoice = new OperationChoice(compOper);
                    Filter filter = new Filter(operChoice);
                    gettinResult = _dbProvider.GetVersionsOf(Aran.Aim.FeatureType.AirportHeliport, _interPretation, default(Guid), false, null, null, filter);
                    
                    if (gettinResult.IsSucceed)
                    {
                        if (gettinResult.List.Count > 0)
                        {
                            compOper.PropertyName = "ResponsibleOrganisation.TheOrganisationAuthority";
                            compOper.Value = (gettinResult.List[0] as Aran.Aim.Features.OrganisationAuthority).Identifier;
                            gettinResult = _dbProvider.GetVersionsOf(Aran.Aim.FeatureType.AirportHeliport, _interPretation, default(Guid), false, null, null, filter);
                        }

                    }
                    else
                        return result;
                }
                if (gettinResult.IsSucceed)
                {
                    for (int i = 0; i < gettinResult.List.Count; i++)
                    {
                        result.Add(_aimToAixmConverter.AdhpToUnicalName(gettinResult.List[i] as Aran.Aim.Features.AirportHeliport));
                    }
                }
                return result;
            }

            catch (Exception e)
            {

                throw e;
            }
        }

        public Ahp GetAedrome(string guid)
        {
            try
            {
                List<Aran.Aim.Features.AirportHeliport> adhpList = new List<Aran.Aim.Features.AirportHeliport>();
                GettingResult gettingResult = _dbProvider.GetVersionsOf(Aran.Aim.FeatureType.AirportHeliport, _interPretation, new Guid(guid), true, null, null);
                if (gettingResult.IsSucceed)
                {

                    if (gettingResult.List.Count > 0)
                    {
                        Ahp result = _aimToAixmConverter.ToAirportHeliport(gettingResult.List[0] as Aran.Aim.Features.AirportHeliport);
                        ARAN.GeometryClasses.Point prjPt = _geomOperators.GeoTransformations(result.GetPtGeo(), GeoSp, PrjSp) as ARAN.GeometryClasses.Point;
                        result.GetPtPrj().Assign(prjPt);
                        return result;
                    }
                }
                return null;
            }
            catch (Exception e)
            {
                throw e;
            }

        }

        public PandaList<Rwy> GetRwyList(string adhpGuid)
        {
            try
            {
                ComparisonOps compOper = new ComparisonOps(ComparisonOpType.EqualTo, "associatedAirportHeliport", new Guid(adhpGuid));
                OperationChoice opChoise = new OperationChoice(compOper);
                Filter filter = new Filter(opChoise);
                GettingResult gettingResult = _dbProvider.GetVersionsOf(Aran.Aim.FeatureType.Runway, _interPretation, default(Guid), false, null, null, filter);
                PandaList<Rwy> rwyList = new PandaList<Rwy>();
                if (gettingResult.IsSucceed)
                {
                    for (int i = 0; i < gettingResult.List.Count; i++)
                    {
                        Rwy rwy = _aimToAixmConverter.ToRunway(gettingResult.List[i] as Aran.Aim.Features.Runway);
                        if (rwy != null)
                            rwyList.Add(rwy);
                    }

                }
                return rwyList;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public PandaList<UnicalName> GetUnicalRwyList(string adhpGuid)
        {
            try
            {
                ComparisonOps compOper = new ComparisonOps(ComparisonOpType.EqualTo, "associatedAirportHeliport", new Guid(adhpGuid));
                OperationChoice opChoise = new OperationChoice(compOper);
                Filter filter = new Filter(opChoise);
                GettingResult gettingResult = _dbProvider.GetVersionsOf(Aran.Aim.FeatureType.Runway, _interPretation, default(Guid), false, null, null, filter);
                PandaList<UnicalName> rwyList = new PandaList<UnicalName>();
                if (gettingResult.IsSucceed)
                {
                    for (int i = 0; i < gettingResult.List.Count; i++)
                    {
                        UnicalName rwy = _aimToAixmConverter.RwyToUnicalName(gettingResult.List[i] as Aran.Aim.Features.Runway);
                        if (rwy != null)
                            rwyList.Add(rwy);
                    }

                }
                return rwyList;
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        public Rwy GetRWY(string guid) 
        {
            try
            {
                GettingResult gettingResult = _dbProvider.GetVersionsOf(Aran.Aim.FeatureType.Runway, _interPretation, new Guid(guid), true, null, null);
                if (gettingResult.IsSucceed)
                {
                    if (gettingResult.List.Count > 0)
                        return _aimToAixmConverter.ToRunway(gettingResult.List[0] as Aran.Aim.Features.Runway);
                }
                return null;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);                
            }
        }

        public PandaList<RwyDirection> GetRWYDirectionList(string rwyGuid)
        {
            try
            {
                ComparisonOps compOper = new ComparisonOps(ComparisonOpType.EqualTo, "usedRunway", rwyGuid);
                OperationChoice operChoise = new OperationChoice(compOper);
                Filter filter = new Filter(operChoise);
                GettingResult gettingResult = _dbProvider.GetVersionsOf(Aran.Aim.FeatureType.RunwayDirection, _interPretation, default(Guid), false, null, null, filter);
                PandaList<RwyDirection> result = new PandaList<RwyDirection>();
                if (gettingResult.IsSucceed)
                {
                    foreach (Aran.Aim.Features.RunwayDirection rwyDirection in gettingResult.List)
                    {
                        if (rwyDirection != null)
                        {
                            RwyDirection tmpRwyDirection = _aimToAixmConverter.ToRunwayDirection(rwyDirection);
                            if (tmpRwyDirection.getElevTdz() == 0) {
                               List<Aran.Aim.Features.RunwayCentrelinePoint> rwyCenterLinePointList = GetRunwayCentrelinePointList(rwyDirection.Identifier);
                               double elevMax = 0;
                               double elevAccuracy = 0;
                               foreach (Aran.Aim.Features.RunwayCentrelinePoint rwyCenterLinePoint in rwyCenterLinePointList)
                               {
                                   double tmpElev = ConverterToSI.Convert(rwyCenterLinePoint.Location.Elevation, 0);
                                   double tmpElevAccuracy = ConverterToSI.Convert(rwyCenterLinePoint.Location.VerticalAccuracy, 0);
                                   if (tmpElev > elevMax)
                                       elevMax = tmpElev;
                                   if (tmpElevAccuracy > elevAccuracy)
                                       elevAccuracy = tmpElevAccuracy;
                                   if (rwyCenterLinePoint.Role == CodeRunwayPointRole.START)
                                       if (rwyCenterLinePoint.Location != null && rwyCenterLinePoint.Location.Geo != null)
                                       {
                                           ARAN.GeometryClasses.Point geoPt = _aimToAixmConverter.ToPoint(rwyCenterLinePoint.Location.Geo);
                                           tmpRwyDirection.GetGeo().Assign(geoPt);
                                           ARAN.GeometryClasses.Point prjPt = _geomOperators.GeoTransformations(geoPt, GeoSp, PrjSp) as ARAN.GeometryClasses.Point;
                                           tmpRwyDirection.GetPrj().Assign(prjPt);
                                           
                                       }
                               }
                               tmpRwyDirection.setElevTdz(elevMax);
                               tmpRwyDirection.SetElevTdzAccuracy(elevAccuracy);
                            }
                            result.Add(tmpRwyDirection);
                        }
                    }
                }
                return result;
            }
            catch (Exception e)
            {

                throw new Exception(e.Message);
            }
            
        }

        public PandaList<Obstacle> GetObstacleList(ARAN.GeometryClasses.Point ptCenter, double range)
        {
            try
            {
                DWithin dWithin = new DWithin();
                dWithin.Distance = new Aran.Aim.DataTypes.ValDistance(range, UomDistance.M);
                dWithin.Geometry = new Aran.Geometries.Point(ptCenter.X, ptCenter.Y);
                dWithin.PropertyName = "part.horizontalProjection.location.geo";
                OperationChoice operChoice = new OperationChoice(dWithin);
                Filter filter = new Filter(operChoice);
                GettingResult gettingResult = _dbProvider.GetVersionsOf(Aran.Aim.FeatureType.VerticalStructure, _interPretation, default(Guid), true, null, null, filter);
                PandaList<Obstacle> result = new PandaList<Obstacle>();
                if (gettingResult.IsSucceed)
                {
                    for (int i = 0; i < gettingResult.List.Count; i++)
                    {
                        Obstacle obstacle = _aimToAixmConverter.ToObstacle(gettingResult.List[i] as Aran.Aim.Features.VerticalStructure);
                        result.Add(obstacle);
                    }
                }
                return result;
            }
            catch (Exception e)
            {

                throw new Exception(e.Message);
            }
            
        }

        public PandaCollection GetSignificantPointsList(string adhpIdentifier,Aran.Geometries.Geometry centerGeo,double range) 
        {
            PandaCollection navequipmentList = GetNavaidEquipmentList(centerGeo, range);
            PandaCollection dsgntPtList = GetDesignatedPointList(centerGeo, range);
            PandaCollection sgfList = new PandaCollection();
            sgfList.Assign(navequipmentList);
            sgfList.Assign(dsgntPtList);
            return sgfList;
        }

        private PandaCollection GetNavaidEquipmentList(Aran.Geometries.Geometry centerGeo, double distance)
        {
            try
            {
                DWithin dWithin = new DWithin();
                dWithin.Geometry = centerGeo;
                dWithin.Distance = new Aran.Aim.DataTypes.ValDistance(distance, UomDistance.M);
                dWithin.PropertyName = "location.geo";
                OperationChoice operChoice = new OperationChoice(dWithin);
                Filter filter = new Filter(operChoice);
                GettingResult gettingResult = _dbProvider.GetVersionsOf(Aran.Aim.AbstractFeatureType.NavaidEquipment, _interPretation, true, _timeSliceFilter, null, filter);
                PandaCollection result = new PandaCollection();
                if (gettingResult.IsSucceed)
                {
                    for (int i = 0; i < gettingResult.List.Count; i++)
                    {
                        SignificanPoint sgfPoint = _aimToAixmConverter.ToSignificantPoint(gettingResult.List[i] as Aran.Aim.Features.NavaidEquipment);
                        if (sgfPoint !=null)
                            result.Add(sgfPoint);
                    }
                }
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private PandaCollection GetDesignatedPointList(Aran.Geometries.Geometry centerGeo, double distance)
        {
            BinaryLogicOp binaryLogic = new BinaryLogicOp();
            //binaryLogic.Type = BinaryLogicOpType.And;
            //ComparisonOps adhpComp = new ComparisonOps(ComparisonOpType.EqualTo, "airportHeliport", orgIdentifier);
            //OperationChoice adhpOperChoice = new OperationChoice(adhpComp);
            //binaryLogic.OperationList.Add(adhpOperChoice);

            DWithin dWithin = new DWithin();
            dWithin.Geometry = centerGeo;
            dWithin.Distance = new Aran.Aim.DataTypes.ValDistance(distance, UomDistance.M);
            dWithin.PropertyName = "location.geo";
            OperationChoice dWithinOper = new OperationChoice(dWithin);
            binaryLogic.OperationList.Add(dWithinOper);

            OperationChoice operChoice = new OperationChoice(binaryLogic);
            Filter filter = new Filter(operChoice);
            GettingResult gettingResult =_dbProvider.GetVersionsOf(Aran.Aim.FeatureType.DesignatedPoint, _interPretation, default(Guid), true, null, null, filter);
            PandaCollection result = new PandaCollection();
            if (gettingResult.IsSucceed)
            {
                for (int i = 0; i < gettingResult.List.Count; i++)
                {
                    SignificanPoint dsgPoint = _aimToAixmConverter.ToSignificantPoint(gettingResult.List[i] as Aran.Aim.Features.DesignatedPoint);
                    result.Add(dsgPoint);
                }
            }
            return result;
        }

        public List<Aran.Aim.Features.RunwayCentrelinePoint> GetRunwayCentrelinePointList(Guid rwyDirIdentifier)
        {
            ComparisonOps compOper = new ComparisonOps(ComparisonOpType.EqualTo, "onRunway", rwyDirIdentifier);
            OperationChoice operChoise = new OperationChoice(compOper);
            Filter filter = new Filter(operChoise);
            GettingResult gettingResult = _dbProvider.GetVersionsOf(Aran.Aim.FeatureType.RunwayCentrelinePoint, _interPretation, default(Guid), false, null, null, filter);
            List<Aran.Aim.Features.RunwayCentrelinePoint> result = new List<Aran.Aim.Features.RunwayCentrelinePoint>();
            if (gettingResult.IsSucceed)
            {
                foreach (Aran.Aim.Features.RunwayCentrelinePoint rwyCenterLinePoint in gettingResult.List)
                {
                    result.Add(rwyCenterLinePoint);
                }
            }
            return result;
        }

      



        public SpatialReference GeoSp { get; set; }
        public SpatialReference PrjSp { get; set; }

    }
    
    public class UnicalName
    {
    	public string Name { get; set; }
    }
}