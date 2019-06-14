using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aran.Aim.Features;
using Aran.Aim.Enums;
using Aran.Aim.Data.Filters;
using Aran.Aim.Data;
using Aran.Geometries;
using Aran.PANDA.Common;

namespace Aran.Queries.Rnav
{
    public static class RNAVSQPIFactory
    {
        public static IRNAVSpecializedQPI Create()
        {
            return new RNAVSpecializedQPI();
        }
    }

    internal class RNAVSpecializedQPI : CommonQPI,IRNAVSpecializedQPI
    {
        public List<DesignatedPoint> GetDesignatedPointList(Aran.Geometries.Polygon polygon)
        {
            GettingResult gettingResult;
            BinaryLogicOp binaryLogic = new BinaryLogicOp();
            binaryLogic.Type = BinaryLogicOpType.And;

            if (!polygon.IsClose)
                polygon.Close();

            var mp = new Aran.Geometries.MultiPolygon();
            mp.Add(polygon);

            Within within = new Within();
            within.Geometry = mp;
            within.PropertyName = "location.geo";
            OperationChoice operChoice = new OperationChoice(within);
            binaryLogic.OperationList.Add(operChoice);

            OperationChoice opChoice = new OperationChoice(binaryLogic);
            Filter filter = new Filter(operChoice);
            //Filter filter = new Filter(operChoice);
            gettingResult = base.DbProvider.GetVersionsOf(Aran.Aim.FeatureType.DesignatedPoint, Aran.Aim.Enums.TimeSliceInterpretationType.BASELINE,
                default(Guid), true, null, null, filter);

            if (gettingResult.IsSucceed)
            {
                var resultList = new List<DesignatedPoint>();
                foreach (DesignatedPoint dPoint in gettingResult.List)
                {
                    if (!string.IsNullOrEmpty(dPoint.Designator))
                        resultList.Add(dPoint);
                }
                return resultList;// gettingResult.GetListAs<DesignatedPoint> ();
            }

            throw new Exception(gettingResult.Message);
        }

        public List<DesignatedPoint> GetDesignatedPointList(Aran.Geometries.Point pt, double distance)
        {
            //BinaryLogicOp binaryLogic = new BinaryLogicOp();
            //binaryLogic.Type = BinaryLogicOpType.And;

            DWithin dWithin = new DWithin();
            dWithin.Distance = new Aran.Aim.DataTypes.ValDistance(distance, UomDistance.M);
            dWithin.Point = pt;
            dWithin.PropertyName = "location.geo"; ;
            OperationChoice operChoice = new OperationChoice(dWithin);
            //binaryLogic.OperationList.Add(operChoice);

            //ComparisonOps compOperDsg = new ComparisonOps(ComparisonOpType.NotNull, "Designator", null);
            //OperationChoice operChoiceDsg = new OperationChoice(compOperDsg);
            //binaryLogic.OperationList.Add(operChoiceDsg);

            //OperationChoice opChoice = new OperationChoice(binaryLogic);
            Filter filter = new Filter(operChoice);
            GettingResult gettingResult = DbProvider.GetVersionsOf(Aran.Aim.FeatureType.DesignatedPoint, Aran.Aim.Enums.TimeSliceInterpretationType.BASELINE, default(Guid), true, null, null, filter);
            
            if (gettingResult.IsSucceed)
            {
                return gettingResult.GetListAs<DesignatedPoint> ();
            }

			throw new Exception ( gettingResult.Message );
        }

        public List<Navaid> GetNavaidList(Aran.Geometries.Polygon polygon)
        {
            Within within = new Within();
            within.Geometry = polygon;
            within.PropertyName = "location.geo";
            OperationChoice operChoice = new OperationChoice(within);
            Filter filter = new Filter(operChoice);
            GettingResult gettingResult = DbProvider.GetVersionsOf(Aran.Aim.FeatureType.Navaid, Aran.Aim.Enums.TimeSliceInterpretationType.BASELINE, default(Guid), true, null, null,filter);
            
            if (gettingResult.IsSucceed)
            {
                var resultList = new List<Navaid>();
                foreach (Navaid nav in gettingResult.List)
                {
                    if (!string.IsNullOrEmpty(nav.Designator))
                        resultList.Add(nav);
                }

                return resultList;// gettingResult.GetListAs<DesignatedPoint> ();
            }
			
            throw new Exception ( gettingResult.Message );
        }

        public List<Navaid> GetNavaidList(Aran.Geometries.Point centerGeo, double distance)
        {
            DWithin within = new DWithin();
            within.Point = centerGeo;
            within.PropertyName = "location.geo";
            OperationChoice operChoice = new OperationChoice(within);
            Filter filter = new Filter(operChoice);
            GettingResult gettingResult = DbProvider.GetVersionsOf(Aran.Aim.FeatureType.Navaid, Aran.Aim.Enums.TimeSliceInterpretationType.BASELINE, default(Guid), true, null, null, filter);
            if (gettingResult.IsSucceed)
            {
                return gettingResult.GetListAs<Navaid> ();
            }
			throw new Exception ( gettingResult.Message );
        }

        private Navaid FindNavaid(List<Navaid> navaidList, NavaidEquipment navaidEquipment)
        {
            Navaid result = navaidList.Find(navaid => navaid.NavaidEquipment.Any(equipment => equipment.TheNavaidEquipment.Type == navaidEquipment.NavaidEquipmentType &&
                equipment.TheNavaidEquipment.Identifier == navaidEquipment.Identifier));
            return result;
        }

        public List<Navaid> GetNavaidListByTypes(Aran.Geometries.Polygon polygon, SpatialReferenceOperation spatialReferenceOperation = null, params CodeNavaidService[] navaidServiceTypes)
        {
            GettingResult gettingResult = DbProvider.GetVersionsOf(Aran.Aim.FeatureType.Navaid, Aran.Aim.Enums.TimeSliceInterpretationType.BASELINE, default(Guid), true, null, null, null);



            if (gettingResult.IsSucceed)
            {
                var result = gettingResult.GetListAs<Navaid>();

                List<Navaid> navaidList = new List<Navaid>();
                Dictionary<Aran.Aim.FeatureType, List<Guid>> featuresList4Loading = new Dictionary<Aim.FeatureType, List<Guid>>();
                Aran.Aim.FeatureType equipmentFeatType;
                foreach (var navaid in result)
                {
                    if (navaid.Location != null && !navaid.Location.Geo.IsEmpty && navaid.Type != null && navaid.Type.HasValue)
                    {
                        if (navaidServiceTypes.Contains(navaid.Type.Value))
                        {
                            var pnt = navaid.Location.Geo;
                            if (spatialReferenceOperation != null)
                                pnt = spatialReferenceOperation.ToPrj<Point>(pnt);
                            if (polygon.IsPointInside(pnt))
                                navaidList.Add(navaid);
                            continue;
                        }
                    }
                    else
                    {
                        foreach (var nav_equipment in navaid.NavaidEquipment)
                        {
                            if (nav_equipment.TheNavaidEquipment != null && navaidServiceTypes.Contains(navaid.Type.Value))
                            {
                                equipmentFeatType = (Aim.FeatureType)nav_equipment.TheNavaidEquipment.Type;
                                if (!featuresList4Loading.ContainsKey(equipmentFeatType))
                                    featuresList4Loading.Add(equipmentFeatType, new List<Guid>());
                                featuresList4Loading[equipmentFeatType].Add(nav_equipment.TheNavaidEquipment.Identifier);
                            }
                        }
                    }
                }
                if (featuresList4Loading.Count == 0)
                    return navaidList;

                foreach (KeyValuePair<Aim.FeatureType, List<Guid>> keyValue in featuresList4Loading)
                {
                    var compOper = new ComparisonOps();
                    compOper.PropertyName = "Identifier";
                    compOper.Value = keyValue.Value;
                    compOper.OperationType = ComparisonOpType.In;

                    var filter = new Filter(new OperationChoice(compOper));

                    GettingResult equipmentResult = DbProvider.GetVersionsOf(keyValue.Key, TimeSliceInterpretationType.BASELINE, default(Guid), true, null, null, filter);
                    if (equipmentResult.IsSucceed)
                    {
                        if (equipmentResult.List != null && equipmentResult.List.Count > 0)
                        {
                            foreach (var item in equipmentResult.List)
                            {
                                NavaidEquipment navEquipment = item as NavaidEquipment;
                                if (navEquipment != null && navEquipment.Location != null)
                                {
                                    var pnt = navEquipment.Location.Geo;
                                    if (spatialReferenceOperation != null)
                                        pnt = spatialReferenceOperation.ToPrj<Point>(pnt);
                                    if (polygon.IsPointInside(pnt))
                                    {
                                        var navaid = FindNavaid(result, navEquipment);
                                        if (!navaidList.Contains(navaid) && navaid != null)
                                            navaidList.Add(navaid);
                                    }
                                }

                            }
                        }
                    }
                }
                return navaidList;
            }
            throw new Exception(gettingResult.Message);
        }

        public VOR GetVor(Guid identifier)
        {
            GettingResult gettingResult = DbProvider.GetVersionsOf(Aran.Aim.FeatureType.VOR, TimeSliceInterpretationType.BASELINE, identifier, true, null, null, null);
            if (gettingResult.IsSucceed)
            {
                if (gettingResult.List.Count > 0)
                    return gettingResult.List[0] as VOR;
            }
			throw new Exception ( gettingResult.Message );
        }

        public DME GetDme(Guid identifier)
        {
            GettingResult gettingResult = DbProvider.GetVersionsOf(Aran.Aim.FeatureType.DME, TimeSliceInterpretationType.BASELINE, identifier, true, null, null, null);
            if (gettingResult.IsSucceed)
            {
                if (gettingResult.List.Count > 0)
                    return gettingResult.List[0] as DME;
            }
			throw new Exception ( gettingResult.Message );
        }

		public List<OrganisationAuthority> GetOrganisatioListDesignatorNotNull ( )
		{
			BinaryLogicOp binaryLogic = new BinaryLogicOp ( );
			binaryLogic.Type = BinaryLogicOpType.Or;

			ComparisonOps compOperDsg = new ComparisonOps ( ComparisonOpType.NotNull, "Designator", null );
			OperationChoice operChoiceDsg = new OperationChoice ( compOperDsg );
			binaryLogic.OperationList.Add ( operChoiceDsg );

			ComparisonOps compOperName = new ComparisonOps ( ComparisonOpType.NotNull, "Name", null );
			OperationChoice operChoiceName = new OperationChoice ( compOperName );
			binaryLogic.OperationList.Add ( operChoiceName );

			OperationChoice opChoice = new OperationChoice ( binaryLogic );
			Filter filter = new Filter ( opChoice );

			GettingResult gettingResult = DbProvider.GetVersionsOf ( Aran.Aim.FeatureType.OrganisationAuthority, Interpretation, default ( Guid ), false, null, null, filter );
			if ( gettingResult.IsSucceed )
			{
				return gettingResult.GetListAs<OrganisationAuthority> ( );
			}
			throw new Exception ( gettingResult.Message );
		}

		public OrganisationAuthority GetOrganisation ( Guid identifier )
		{
			GettingResult getResult = DbProvider.GetVersionsOf ( Aim.FeatureType.OrganisationAuthority, Interpretation, identifier );
			if ( getResult.IsSucceed )
			{
				List<OrganisationAuthority> orgList = getResult.GetListAs<OrganisationAuthority> ( );
				if ( orgList.Count > 0 )
					return orgList [ 0 ];
				throw new Exception ( "Ogranisation not found in database !" );
			}
			throw new Exception ( getResult.Message );
		}
		
		public List<AirportHeliport> GetAdhpListDesignatorNotNull ( Guid orgGuid )
        {
            ComparisonOps compOper1 = new ComparisonOps(ComparisonOpType.EqualTo, "responsibleOrganisation", orgGuid);
            OperationChoice operChoice1 = new OperationChoice(compOper1);

            ComparisonOps compOper2 = new ComparisonOps(ComparisonOpType.NotNull, "designator", null);
            OperationChoice operChoice2 = new OperationChoice(compOper2);
            BinaryLogicOp binaryLogicOper = new BinaryLogicOp();
            binaryLogicOper.Type = BinaryLogicOpType.And;
            binaryLogicOper.OperationList.Add(operChoice2);
            binaryLogicOper.OperationList.Add(operChoice2);

            OperationChoice operChoice = new OperationChoice(binaryLogicOper);
            Filter filter = new Filter(operChoice);
            GettingResult gettingResult = DbProvider.GetVersionsOf(Aran.Aim.FeatureType.AirportHeliport, TimeSliceInterpretationType.BASELINE, default(Guid), true, null, null, filter);

            if (gettingResult.IsSucceed)
            {
                return gettingResult.GetListAs<AirportHeliport> ();
            }

			throw new Exception ( gettingResult.Message );
        }

        public AirportHeliport GetAirportHeliport(Guid identifier)
        {
            GettingResult getResult = DbProvider.GetVersionsOf(Aim.FeatureType.AirportHeliport, Interpretation, identifier);
            if (getResult.IsSucceed)
            {
                List<AirportHeliport> adhpList = getResult.GetListAs<AirportHeliport>();
                if (adhpList.Count > 0)
                    return adhpList[0];
                throw new Exception("Airport not found in database !");
            }
            throw new Exception(getResult.Message);
        }

        public List<VerticalStructure> GetVerticalStructureList(Aran.Geometries.Polygon polygon)
        {
            var mp = new Aran.Geometries.MultiPolygon();
            mp.Add(polygon);

            var withinLocation = new Within
            {
                Geometry = mp,
                PropertyName = "part.horizontalProjection.location.geo"
            };
            var operChoiceLocation = new OperationChoice(withinLocation);

            var withinSurface = new Within
            {
                Geometry = mp,
                PropertyName = "part.horizontalProjection.surfaceExtent.geo"
            };

            var operChoiceSurface = new OperationChoice(withinSurface);

            var withinLinear = new Within
            {
                Geometry = mp,
                PropertyName = "part.horizontalProjection.linearExtent.geo"
            };
            var operChoiceLinear = new OperationChoice(withinLinear);

            var logicOp = new BinaryLogicOp();
            logicOp.OperationList.Add(operChoiceLocation);
            logicOp.OperationList.Add(operChoiceSurface);
            logicOp.OperationList.Add(operChoiceLinear);
            logicOp.Type = BinaryLogicOpType.Or;

            var operChoice = new OperationChoice(logicOp);

            var filter = new Filter(operChoice);

            GettingResult gettingResult;
            if (DbProvider.ProviderType == DbProviderType.ComSoft)
            {
                //All obstacles
                gettingResult = DbProvider.GetVersionsOf(Aran.Aim.FeatureType.VerticalStructure,
                    TimeSliceInterpretationType.BASELINE, default(Guid), true, null, null, null);
            }
            else
            {
                gettingResult = DbProvider.GetVersionsOf(Aran.Aim.FeatureType.VerticalStructure,
                    TimeSliceInterpretationType.BASELINE, default(Guid), true, null, null, filter);
            }

            if (gettingResult.IsSucceed)
            {
                var terrainDataList = DoTerrainDataReader(mp);
                if (terrainDataList == null)
                    return gettingResult.GetListAs<VerticalStructure>();

                var result = new List<VerticalStructure>();
                result.AddRange(gettingResult.GetListAs<VerticalStructure>());
                result.AddRange(terrainDataList);
                return result;
            }
            throw new Exception(gettingResult.Message);
        }

        public List<VerticalStructure> GetVerticalStructureList()
        {
            GettingResult gettingResult = DbProvider.GetVersionsOf(Aran.Aim.FeatureType.VerticalStructure, TimeSliceInterpretationType.BASELINE, default(Guid), true, null, null);
            if (gettingResult.IsSucceed)
            {
                var terrainDataList = DoTerrainDataReader(null);
                if (terrainDataList == null)
                    return gettingResult.GetListAs<VerticalStructure>();

                var result = new List<VerticalStructure>();
                result.AddRange(gettingResult.GetListAs<VerticalStructure>());
                result.AddRange(terrainDataList);
                return result;
            }
            throw new Exception(gettingResult.Message);
        }
	}
}
