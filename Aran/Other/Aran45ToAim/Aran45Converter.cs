//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using ESRI.ArcGIS.Geodatabase;
//using ESRI.ArcGIS.DataSourcesGDB;
//using Aran.Aim.Features;
//using Aran.Aim;

//namespace Aran45ToAixm
//{
//    public class Aran45Converter : ConverterToAixm51
//    {
//        private IFeatureWorkspace _featWS;

//        public override void OpenFile (string fileName)
//        {
//            IWorkspaceFactory wsFact = new AccessWorkspaceFactory ();
//            IWorkspace ws = wsFact.OpenFromFile (fileName, 0);
//            _featWS = ws as IFeatureWorkspace;
//        }

//        public override List<Type> GetFeaturesList ()
//        {
//            var list = new List<Type> ();

//            foreach (var type in Global.Supported45Features)
//            {
//                try
//                {
//                    var featureName = Global.GetFeatureTypeName (type);
//                    _featWS.OpenTable (featureName);
//                    list.Add (type);
//                }
//                catch { }
//            }

//            return list;
//        }

//        public override List<Aran.Aim.Features.Feature> ConvertFeature<TypeField> (List<string> errorList)
//        {
//            var tableName = Global.GetFeatureTypeName (typeof (TypeField));
//            IFeatureClass featClass = _featWS.OpenFeatureClass (tableName);

//            IQueryFilter filter = null;
//            IFeatureCursor cursor = featClass.Search (filter, false);
//            IFeature esriFeature;
//            var list = new List<Aran.Aim.Features.Feature> ();

//            System.Array enumItems = Enum.GetValues (typeof (TypeField));
//            int [] fieldIndexes = new int [enumItems.Length];
//            foreach (object enumItem in enumItems)
//            {
//                fieldIndexes [(int) enumItem] = featClass.FindField (enumItem.ToString ());
//            }

//            var valueGetter = new EsriFieldValueGetter<TypeField> ();
//            valueGetter.FieldIndexes = fieldIndexes;

//            Global.CurrentFuncTagDict.Clear ();

//            while ((esriFeature = cursor.NextFeature ()) != null)
//            {
//                valueGetter.CurrentRowItem = esriFeature;

//                try
//                {
//                    var aimFeature = Aixm45To51.ToFeature (valueGetter);
//                    if (aimFeature != null)
//                        list.Add (aimFeature);
//                }
//                catch (Exception ex)
//                {
//                    errorList.Add ("Error, ID: " + valueGetter.GetId () + "\r\n" + ex.Message);
//                }
//            }

//            return list;
//        }

//        public override List<List<Aran.Aim.Features.Feature>> PostConvert<TypeField> (List<Aran.Aim.Features.Feature> featList, List<string> errorList)
//        {
//            if (typeof (TypeField) == typeof (RouteSegmentField))
//                return PostRouteSegmentConvert (featList, errorList);
//            return null;
//        }

        
//        private List<List<Aran.Aim.Features.Feature>> PostRouteSegmentConvert (List<Aran.Aim.Features.Feature> featList, List<string> errorList)
//        {
//            var resultDict = new List<List<Aran.Aim.Features.Feature>> ();

//            //try
//            //{
//            //    var dpList = Global.DbProvider.GetAllFeatuers (Aran.Aim.FeatureType.DesignatedPoint);

//            //    ITable table = _featWS.OpenTable ("EnrouteRoute");
//            //    IQueryFilter filter = null;
//            //    ICursor cursor = table.Search (filter, false);
//            //    IRow row;

//            //    var routeDict = new Dictionary<string, Route> ();

//            //    while ((row = cursor.NextRow ()) != null)
//            //    {
//            //        try
//            //        {
//            //            var route = new Route ();
//            //            Aixm45To51.FillTimeSlice (route);

//            //            string mid = row.get_Value (row.Fields.FindField ("R_mid")).ToString ();
//            //            route.Name = row.get_Value (row.Fields.FindField ("R_txtDesig")).ToString ();
//            //            route.LocationDesignator = row.get_Value (row.Fields.FindField ("R_txtLocDesig")).ToString ();

//            //            routeDict.Add (mid, route);
//            //        }
//            //        catch (Exception ex)
//            //        {
//            //            errorList.Add ("Error, ID: " + row.OID + "\r\n" + ex.Message);
//            //        }
//            //    }

//            //    foreach (Aran.Aim.Features.RouteSegment rs in Global.CurrentFuncTagDict.Keys)
//            //    {
//            //        var rsTag = Global.CurrentFuncTagDict [rs] as RouteSegmentTag;

//            //        Route route;
//            //        if (routeDict.TryGetValue (rsTag.RouteMid, out route))
//            //        {
//            //            rs.RouteFormed = new Aran.Aim.DataTypes.FeatureRef (route.Identifier);
//            //            route.InternationalUse = rsTag.Route.InternationalUse;
//            //            route.FlightRule = rsTag.Route.FlightRule;
//            //            route.MilitaryUse = rsTag.Route.MilitaryUse;
//            //        }

//            //        if (rs.Start == null)
//            //            rs.Start = new EnRouteSegmentPoint ();
//            //        rs.Start.PointChoice = GetDesignatedAsSignificantPoint (dpList, rsTag.StartPointCodeId);

//            //        if (rs.End == null)
//            //            rs.End = new EnRouteSegmentPoint ();
//            //        rs.End.PointChoice = GetDesignatedAsSignificantPoint (dpList, rsTag.EndPointCodeId);

//            //    }
                
//            //    resultDict.Add (new List<Aran.Aim.Features.Feature> (routeDict.Values));
//            //    resultDict.Add (new List<Aran.Aim.Features.Feature> (featList));

//            //    featList.AddRange (resultDict [0]);
//            //}
//            //catch (Exception ex)
//            //{
//            //    errorList.Add (ex.Message);
//            //}

//            return resultDict;
//        }

//        private SignificantPoint GetDesignatedAsSignificantPoint (List<Aran.Aim.Features.Feature> dpList, string designator)
//        {
//            foreach (DesignatedPoint dp in dpList)
//            {
//                if (dp.Designator == designator)
//                {
//                    var sp = new SignificantPoint ();
//                    sp.FixDesignatedPoint = new Aran.Aim.DataTypes.FeatureRef ();
//                    sp.FixDesignatedPoint.Identifier = dp.Identifier;
//                    return sp;
//                }
//            }
//            return null;
//        }

//    }
//}
