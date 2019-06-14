using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aran.Aim;
using Aran.Aim.Env.Layers;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Geometry;
using Aran.Aim.Data;
using Aran.Aim.Enums;
using Aran.Aim.Data.Filters;
using System.Collections;
using Aran.Aim.Utilities;
using Aran.Aim.Features;
using Aran.Aim.DataTypes;

namespace MapEnv.QueryLayer
{
    public class TestQueryLayer
    {
        private IDbProvider _dbProvider;
        private TimeSliceFilter _tsFilter;

        public TestQueryLayer ()
        {
            _tsFilter = new TimeSliceFilter (new DateTime (2020, 1, 1));
        }

        private void LoadQuery (QueryInfo_OLD qi, IEnumerable<Guid> identifiers)
        {
            Filter filter = null;

            if (identifiers != null)
            {
                ComparisonOps compOp = new ComparisonOps ();
                compOp.OperationType = ComparisonOpType.In;
                compOp.PropertyName = "Identifier";
                compOp.Value = identifiers;
                filter = new Filter (new OperationChoice (compOp));
            }

            IList featureList = GetFeatures (qi.FeatureType, filter);
            qi.FeatureList = featureList;

            foreach (SubQueryInfo_OLD si in qi.SubQueries)
            {
                AimPropInfo [] pathPropInfos = AimMetadataUtility.GetInnerProps ((int) qi.FeatureType, si.PropertyPath);

                List<Guid> subGuidList = new List<Guid> ();

                foreach (Feature feat in featureList)
                {
                    var aimPropValList = AimMetadataUtility.GetInnerPropertyValue (feat, pathPropInfos);

                    foreach (IAimProperty aimProp in aimPropValList)
                    {
                        if (aimProp is IAbstractFeatureRef)
                        {
                            var afr = aimProp as IAbstractFeatureRef;
                            if (afr.FeatureTypeIndex == (int) si.QueryInfo.FeatureType)
                                subGuidList.Add (afr.Identifier);
                        }
                        else if (aimProp is FeatureRef)
                        {
                            var featRef = aimProp as FeatureRef;
                            if (!subGuidList.Contains (featRef.Identifier))
                                subGuidList.Add (featRef.Identifier);
                        }
                    }
                }

                LoadQuery (si.QueryInfo, subGuidList);
            }
        }

        private IList GetFeatures (FeatureType featureType, Filter filter)
        {
            var gr = _dbProvider.GetVersionsOf (
                featureType,
                TimeSliceInterpretationType.BASELINE,
                Guid.Empty,
                true,
                _tsFilter,
                null,
                filter);

            if (!gr.IsSucceed)
            {
                throw new Exception (gr.Message);
            }

            return gr.List;
        }


        private static QueryInfo_OLD CreateSample1 ()
        {
            TableShapeInfo shapeInfo;

            QueryInfo_OLD qiRCP = new QueryInfo_OLD ();
            qiRCP.FeatureType = FeatureType.RunwayCentrelinePoint;

            shapeInfo = new TableShapeInfo ();
            shapeInfo.GeoProperty = "Location/Geo";
            shapeInfo.CategorySymbol.DefaultSymbol = Globals.CreateDefaultSymbol (
                esriGeometryType.esriGeometryPoint, qiRCP.FeatureType);

            qiRCP.ShapeInfoList.Add (shapeInfo);

            QueryInfo_OLD qiRwDir = new QueryInfo_OLD ();
            qiRwDir.FeatureType = FeatureType.RunwayDirection;
            qiRCP.SubQueries.Add (new SubQueryInfo_OLD ("OnRunway", qiRwDir));

            QueryInfo_OLD qiRwy = new QueryInfo_OLD ();
            qiRwy.FeatureType = FeatureType.Runway;
            qiRwDir.SubQueries.Add (new SubQueryInfo_OLD ("UsedRunway", qiRwy));

            QueryInfo_OLD qiAH = new QueryInfo_OLD ();
            qiAH.FeatureType = FeatureType.AirportHeliport;

            shapeInfo = new TableShapeInfo ();
            shapeInfo.GeoProperty = "ARP/Geo";
            shapeInfo.CategorySymbol.DefaultSymbol = Globals.CreateDefaultSymbol (
                esriGeometryType.esriGeometryPoint, qiAH.FeatureType);

            qiAH.ShapeInfoList.Add (shapeInfo);

            qiRwy.SubQueries.Add (new SubQueryInfo_OLD ("AssociatedAirportHeliport", qiAH));

            return qiRCP;
        }

        private static QueryInfo_OLD CreateSample2 ()
        {
            var trajectoryShapeInfo = new TableShapeInfo ();
            trajectoryShapeInfo.GeoProperty = "Trajectory/Geo";
            trajectoryShapeInfo.CategorySymbol.DefaultSymbol = Globals.CreateDefaultSymbol (
                esriGeometryType.esriGeometryPolyline, 0);

            var dpShapeInfo = new TableShapeInfo ();
            dpShapeInfo.GeoProperty = "Location/Geo";
            dpShapeInfo.CategorySymbol.DefaultSymbol = Globals.CreateDefaultAimFeatCharacterSymbol (
                FeatureType.DesignatedPoint) as ISymbol;


            QueryInfo_OLD qiIAP = new QueryInfo_OLD ();
            qiIAP.FeatureType = FeatureType.InstrumentApproachProcedure;
            qiIAP.ShapeInfoList.Add (trajectoryShapeInfo);

            QueryInfo_OLD qiFinal = new QueryInfo_OLD ();
            qiFinal.FeatureType = FeatureType.FinalLeg;
            qiFinal.ShapeInfoList.Add (trajectoryShapeInfo);
            qiIAP.SubQueries.Add (new SubQueryInfo_OLD ("FlightTransition/TransitionLeg/TheSegmentLeg", qiFinal));

            QueryInfo_OLD qiInitial = new QueryInfo_OLD ();
            qiInitial.FeatureType = FeatureType.InitialLeg;
            qiInitial.ShapeInfoList.Add (trajectoryShapeInfo);
            qiIAP.SubQueries.Add (new SubQueryInfo_OLD ("FlightTransition/TransitionLeg/TheSegmentLeg", qiInitial));

            QueryInfo_OLD qiIntermediate = new QueryInfo_OLD ();
            qiIntermediate.FeatureType = FeatureType.IntermediateLeg;
            qiIntermediate.ShapeInfoList.Add (trajectoryShapeInfo);
            qiIAP.SubQueries.Add (new SubQueryInfo_OLD ("FlightTransition/TransitionLeg/TheSegmentLeg", qiIntermediate));

            QueryInfo_OLD qiMissApp = new QueryInfo_OLD ();
            qiMissApp.FeatureType = FeatureType.MissedApproachLeg;
            qiMissApp.ShapeInfoList.Add (trajectoryShapeInfo);
            qiIAP.SubQueries.Add (new SubQueryInfo_OLD ("FlightTransition/TransitionLeg/TheSegmentLeg", qiMissApp));


            QueryInfo_OLD qiStartPoint = new QueryInfo_OLD ();
            qiStartPoint.FeatureType = FeatureType.DesignatedPoint;
            qiStartPoint.ShapeInfoList.Add (dpShapeInfo);

            qiFinal.SubQueries.Add (new SubQueryInfo_OLD (
                "StartPoint/PointChoice/FixDesignatedPoint", qiStartPoint.Clone ()));

            qiInitial.SubQueries.Add (new SubQueryInfo_OLD (
                "StartPoint/PointChoice/FixDesignatedPoint", qiStartPoint.Clone ()));

            qiIntermediate.SubQueries.Add (new SubQueryInfo_OLD (
                "StartPoint/PointChoice/FixDesignatedPoint", qiStartPoint.Clone ()));

            qiMissApp.SubQueries.Add (new SubQueryInfo_OLD (
                "StartPoint/PointChoice/FixDesignatedPoint", qiStartPoint.Clone ()));

            return qiIAP;
        }

        public static void Test (IDbProvider dbProvider)
        {
            QueryInfoBuilderForm qibf = new QueryInfoBuilderForm ();
            qibf.ShowDialog ();

            //TestQueryLayer tql = new TestQueryLayer ();
            //tql._dbProvider = dbProvider;

            //var qi = CreateSample2 ();

            //tql.LoadQuery (qi, null);
        }
    }
}
