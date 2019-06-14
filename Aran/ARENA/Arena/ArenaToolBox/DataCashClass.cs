using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PDM;
using ARENA.Environment;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using AranSupport;
using System.Runtime.InteropServices;
using System.IO;
using AiracUtil;
using EsriWorkEnvironment;
using ESRI.ArcGIS.Carto;
using ArenaStatic;

namespace ARENA
{
    public static class DataCash
    {
        //Данная переменная статического класса будет доступна откуда угодно в пределах проекта
        public static Environment.Environment ProjectEnvironment;

        public static bool StorePDMobject(PDMObject pdmObject)
        {
            bool res = true;

            ITable tbl = DataCash.ProjectEnvironment.Data.TableDictionary[pdmObject.GetType()];

            IWorkspaceEdit workspaceEdit = (IWorkspaceEdit)(tbl as FeatureClass).Workspace;
            workspaceEdit.StartEditing(false);
            workspaceEdit.StartEditOperation();


            DataCash.ProjectEnvironment.Data.PdmObjectList.Add(pdmObject);
            res = pdmObject.StoreToDB(DataCash.ProjectEnvironment.Data.TableDictionary);

            workspaceEdit.StopEditOperation();
            workspaceEdit.StopEditing(true);

            return res;
        }

        public static List<PDMObject> GetObjectsWithinPolygon(IPolygon Poly, Type pdmObjectType,  List<PDMObject> sourceList = null)
        {
            List<PDMObject> res = new List<PDMObject>();

            List<PDMObject> _list = (sourceList == null) ? DataCash.ProjectEnvironment.Data.PdmObjectList : sourceList;

                AranSupport.Utilitys util = new AranSupport.Utilitys();

                var pdmList = (from element in _list where (element != null) && (element.GetType().Equals(pdmObjectType)) select element).ToList();
                //string ss = "";
                try
                {
                    //ss = "";
                    foreach (var item in pdmList)
                    {
                      
                        if (pdmObjectType.Equals(typeof(NavaidSystem)))
                        {
                            foreach (var navEq in ((NavaidSystem)item).Components)
                            {
                                if (navEq.Geo == null) navEq.RebuildGeo();
                                if (util.WithinPolygon(Poly, navEq.Geo))
                                {
                                    res.Add(item);
                                    break;
                                }
                            }
                        }
                        else if (pdmObjectType.Equals(typeof(VerticalStructure)))
                        {
                            foreach (var part in ((VerticalStructure)item).Parts)
                            {
                                if (part.Geo == null) part.RebuildGeo();
                                if (util.WithinPolygon(Poly, part.Geo))
                                {
                                    res.Add(item);
                                    break;
                                }
                            }
                        }
                        else
                        {
                            if (item.Geo == null) item.RebuildGeo();
                            if (util.WithinPolygon(Poly, item.Geo)) res.Add(item);
                        }

                    }
                }
                catch (Exception ex)
                {
                    //System.Diagnostics.Debug.WriteLine(ss);
                }

            return res;
        }

        public static List<PDMObject> GetObjectsWithinPolygon(IGeometry Extent, PDM_ENUM objPDMType, List<PDMObject> sourceList = null)
        {
            List<PDMObject> res = new List<PDMObject>();
            List<PDMObject> _list = (sourceList == null) ? DataCash.ProjectEnvironment.Data.PdmObjectList : sourceList;


            AranSupport.Utilitys util = new AranSupport.Utilitys();

            var pdmList = (from element in _list where (element != null) && (element.PDM_Type == objPDMType) select element).ToList();

          
            //string ss = "";
            try
            {
                //ss = "";
                foreach (var item in pdmList)
                {

                    if (objPDMType == PDM_ENUM.NavaidSystem) //(pdmObjectType.Equals(typeof(NavaidSystem)))
                    {
                        foreach (var navEq in ((NavaidSystem)item).Components)
                        {
                            if (navEq.Geo == null) navEq.RebuildGeo();
                            if (util.WithinPolygon(Extent, navEq.Geo))
                            {
                                res.Add(item);
                                break;
                            }
                        }
                    }
                    else if (objPDMType == PDM_ENUM.VerticalStructure)//(pdmObjectType.Equals(typeof(VerticalStructure)))
                    {
                        foreach (var part in ((VerticalStructure)item).Parts)
                        {
                            if (part.Geo == null) part.RebuildGeo();
                            if (util.WithinPolygon(Extent, part.Geo))
                            {
                                res.Add(item);
                                break;
                            }
                        }
                    }

                    else if (objPDMType == PDM_ENUM.Enroute)//(pdmObjectType.Equals(typeof(VerticalStructure)))
                    {
                        foreach (var rt_seg in ((Enroute)item).Routes)
                        {
                            if (rt_seg.Geo == null) rt_seg.RebuildGeo();
                            if (util.WithinPolygon(Extent, rt_seg.Geo))
                            {
                                res.Add(item);
                                break;
                            }
                        }
                    }

                    else if (objPDMType == PDM_ENUM.Airspace)//(pdmObjectType.Equals(typeof(VerticalStructure)))
                    {
                        foreach (var arsp_vol in ((Airspace)item).AirspaceVolumeList)
                        {
                            if (arsp_vol.Geo == null) arsp_vol.RebuildGeo2();
                            if (util.WithinPolygon(Extent, arsp_vol.Geo))
                            {
                                res.Add(item);
                                break;
                            }
                        }
                    }
                    else
                    {
                        if (item.Geo == null) item.RebuildGeo();
                        if (util.WithinPolygon(Extent, item.Geo)) res.Add(item);
                    }

                }
            }
            catch (Exception ex)
            {
                //System.Diagnostics.Debug.WriteLine(ss);
            }

            return res;
        }

        public static List<PDMObject> GetObjectsByType(PDM_ENUM objPDMType, List<PDMObject> sourceList = null)
        {
            List<PDMObject> _list = (sourceList == null) ? DataCash.ProjectEnvironment.Data.PdmObjectList : sourceList;

            var pdmList = (from element in _list where (element != null) && (element.PDM_Type == objPDMType) select element).ToList();

            return pdmList;
        }


        public static List<PDMObject> GetHoldingList(List<PDMObject> sourceList = null, CodeHoldingUsage hldnType = CodeHoldingUsage.TER)
        {
            List<PDMObject> _list = (sourceList == null) ? DataCash.ProjectEnvironment.Data.PdmObjectList : sourceList;

            var pdmList = (from element in _list where (element != null) && (element.PDM_Type == PDM_ENUM.HoldingPattern) && ((HoldingPattern)element).Type == hldnType select element).ToList();

            return pdmList;
        }

        public static List<PDMObject> GetObstaclesWithinPolygon(IPolygon Poly)
        {
            List<PDMObject> res = new List<PDMObject>();
            AranSupport.Utilitys util = new AranSupport.Utilitys();
            List<string> obsIDs = new List<string>();

            try
            {

                ISpatialFilter filter = new SpatialFilterClass();

                filter.Geometry = Poly;
                filter.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
                filter.WhereClause = "FeatureGUID <> ''";


                string pntTbl = util.GetTblName(PDM_ENUM.VerticalStructure.ToString(), esriGeometryType.esriGeometryPoint);
                string lnTbl = util.GetTblName(PDM_ENUM.VerticalStructure.ToString(), esriGeometryType.esriGeometryPolyline);
                string plgTbl = util.GetTblName(PDM_ENUM.VerticalStructure.ToString(), esriGeometryType.esriGeometryPolygon);

                ITable tbl = DataCash.ProjectEnvironment.Data.TableDictionary[typeof(AirportHeliport)];
                

                IWorkspaceEdit workspaceEdit = (IWorkspaceEdit)(tbl as FeatureClass).Workspace;


                #region Point Obstacle

                IFeatureClass fCls = (IFeatureClass)EsriUtils.getTableByname((IFeatureWorkspace)workspaceEdit, pntTbl);//util.GetTableByName(pntTbl, ArenaStatic.ArenaStaticProc.GetTargetDB());

                IFeatureCursor ft_cursor = fCls.Search(filter, false);
                IFeature _Feature = null;
                while ((_Feature = ft_cursor.NextFeature()) != null)
                {
                    obsIDs.Add(Convert.ToString(_Feature.get_Value(fCls.FindField("partID"))));
                }

                #endregion


                #region Line Obstacle

                fCls = (IFeatureClass)EsriUtils.getTableByname((IFeatureWorkspace)workspaceEdit, lnTbl);//util.GetTableByName(lnTbl, ArenaStatic.ArenaStaticProc.GetTargetDB());

                ft_cursor = fCls.Search(filter, false);
                _Feature = null;
                while ((_Feature = ft_cursor.NextFeature()) != null)
                {
                    obsIDs.Add(Convert.ToString(_Feature.get_Value(fCls.FindField("partID"))));
                }

                #endregion


                #region Polygon Obstacle

                fCls = (IFeatureClass)EsriUtils.getTableByname((IFeatureWorkspace)workspaceEdit, plgTbl);//util.GetTableByName(plgTbl, ArenaStatic.ArenaStaticProc.GetTargetDB());

                ft_cursor = fCls.Search(filter, false);
                _Feature = null;
                while ((_Feature = ft_cursor.NextFeature()) != null)
                {
                    obsIDs.Add(Convert.ToString(_Feature.get_Value(fCls.FindField("partID"))));
                }

                #endregion
                

                
                var pdmList = (from element in DataCash.ProjectEnvironment.Data.PdmObjectList where (element != null) && (element is VerticalStructure) select element).ToList();
                foreach (VerticalStructure Vs in pdmList)
                {
                    foreach (VerticalStructurePart item in Vs.Parts)
                    {
                        if (obsIDs.IndexOf(item.ID) >= 0)
                        {
                            //item.RebuildGeo();
                            res.Add(Vs);
                            break;
                        }
                    }
                }

                Marshal.ReleaseComObject(ft_cursor);

                //GC.Collect();

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            util = null;
            return res;
        }

        public static void AddAssesmentAreaObjectsToObsList(ref List<PDMObject> obstacleList, List<PDMObject> selectedProc, double aboveLimit_M = 100)
        {
            foreach (Procedure proc in selectedProc)
            {
                foreach (ProcedureTransitions trans in proc.Transitions)
                {
                    foreach (ProcedureLeg leg in trans.Legs)
                    {
                        if (leg.AssessmentArea != null)
                        {
                            foreach (ObstacleAssessmentArea aseesmentArea in leg.AssessmentArea)
                            {
                                if (aseesmentArea.SignificantObstacle == null) continue;

                                System.Diagnostics.Debug.WriteLine(aseesmentArea.SignificantObstacle.Count.ToString());
                                foreach (Obstruction obs in aseesmentArea.SignificantObstacle)
                                {
                                    if (obstacleList.Contains(obs.VerticalStructure)) continue;
                                    else
                                    {
                                        if (obs.VerticalStructure!=null && obs.VerticalStructure.Parts != null && obs.VerticalStructure.Parts.Count > 0)
                                        {
                                            foreach (VerticalStructurePart prts in obs.VerticalStructure.Parts)
                                            {
                                                if (prts.Elev.HasValue &&  prts.ConvertValueToMeter(prts.Elev, prts.Elev_UOM.ToString()) >= aboveLimit_M)
                                                {
                                                    obstacleList.Add(obs.VerticalStructure);
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        public static List<PDMObject> GetObstaclesWithinPolygon(IGeometry Extent, double aboveLimit_M = -1, double belowLimit_M = -1)
        {
            List<PDMObject> res = new List<PDMObject>();
            AranSupport.Utilitys util = new AranSupport.Utilitys();
            List<string> obsIDs = new List<string>();

            try
            {

                ISpatialFilter filter = new SpatialFilterClass();

                filter.Geometry = Extent;
                filter.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
                filter.WhereClause = "OBJECTID >0";



                string pntTbl = util.GetTblName(PDM_ENUM.VerticalStructure.ToString(), esriGeometryType.esriGeometryPoint);
                string lnTbl = util.GetTblName(PDM_ENUM.VerticalStructure.ToString(), esriGeometryType.esriGeometryPolyline);
                string plgTbl = util.GetTblName(PDM_ENUM.VerticalStructure.ToString(), esriGeometryType.esriGeometryPolygon);

                ITable tbl = DataCash.ProjectEnvironment.Data.TableDictionary[typeof(AirportHeliport)];

                IWorkspaceEdit workspaceEdit = (IWorkspaceEdit)(tbl as FeatureClass).Workspace;

                #region Point Obstacle

                IFeatureClass fCls = (IFeatureClass)EsriUtils.getTableByname((IFeatureWorkspace)workspaceEdit, pntTbl);//(IFeatureClass)util.GetTableByName(pntTbl, ArenaStatic.ArenaStaticProc.GetTargetDB());

                IFeatureCursor ft_cursor = fCls.Search(filter, false);
                IFeature _Feature = null;
                while ((_Feature = ft_cursor.NextFeature()) != null)
                {
                    obsIDs.Add(Convert.ToString(_Feature.get_Value(fCls.FindField("partID"))));
                }

                #endregion


                #region Line Obstacle

                fCls = (IFeatureClass)EsriUtils.getTableByname((IFeatureWorkspace)workspaceEdit, lnTbl);//(IFeatureClass)util.GetTableByName(lnTbl, ArenaStatic.ArenaStaticProc.GetTargetDB());

                ft_cursor = fCls.Search(filter, false);
                _Feature = null;
                while ((_Feature = ft_cursor.NextFeature()) != null)
                {
                    obsIDs.Add(Convert.ToString(_Feature.get_Value(fCls.FindField("partID"))));
                }

                #endregion


                #region Polygon Obstacle

                fCls = (IFeatureClass)EsriUtils.getTableByname((IFeatureWorkspace)workspaceEdit, plgTbl);//(IFeatureClass)util.GetTableByName(plgTbl, ArenaStatic.ArenaStaticProc.GetTargetDB());

                ft_cursor = fCls.Search(filter, false);
                _Feature = null;
                while ((_Feature = ft_cursor.NextFeature()) != null)
                {
                    obsIDs.Add(Convert.ToString(_Feature.get_Value(fCls.FindField("partID"))));
                }

                #endregion


                if (obsIDs.Count > 0)
                {
                    var pdmList = (from element in DataCash.ProjectEnvironment.Data.PdmObjectList where (element != null) && (element is VerticalStructure) select element).ToList();
                    foreach (VerticalStructure Vs in pdmList)
                    {
                        foreach (VerticalStructurePart item in Vs.Parts)
                        {
                            if (obsIDs.IndexOf(item.ID) >= 0)
                            {
                                //item.RebuildGeo();
                                res.Add(Vs);

                                if (aboveLimit_M > 0 && item.Height != null && item.Height.HasValue && item.ConvertValueToMeter(item.Height.Value, item.Height_UOM.ToString()) < aboveLimit_M)
                                    res.Remove(Vs);


                                if (belowLimit_M > 0 && item.Height != null && item.Height.HasValue && item.ConvertValueToMeter(item.Height.Value, item.Height_UOM.ToString()) > belowLimit_M)
                                    res.Remove(Vs);

                                break;
                            }
                        }
                    }
                }
               // var pdmList2 = (from element in DataCash.ProjectEnvironment.Data.PdmObjectList where (element != null) && (element.ActualDate!= new DateTime(2016,5,26)) select element).ToList();


                Marshal.ReleaseComObject(ft_cursor);

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            util = null;
            return res;
        }

        public static List<PDMObject> GetAerodromeObstaclesWithinPolygon(IGeometry Extent, IMap focusMap, ISpatialReference pSpatialReference)
        {
            List<PDMObject> res = new List<PDMObject>();
            AranSupport.Utilitys util = new AranSupport.Utilitys();
            List<string> obsIDs = new List<string>();

            try
            {

                IPolygon plg = new PolygonClass();
                (plg as IPointCollection).AddPoint(((IEnvelope)Extent).LowerLeft);
                (plg as IPointCollection).AddPoint(((IEnvelope)Extent).UpperLeft);
                (plg as IPointCollection).AddPoint(((IEnvelope)Extent).UpperRight);
                (plg as IPointCollection).AddPoint(((IEnvelope)Extent).LowerRight);

                ITopologicalOperator topoOper = plg as ITopologicalOperator;
                if (!topoOper.IsKnownSimple)  topoOper.Simplify();

                IQueryFilter filter = new QueryFilterClass();
                filter.WhereClause = "OBJECTID >0 AND AirportAssociated = true";


                string vsTbl = util.GetTblName("VerticalStructure");

                ITable tbl = DataCash.ProjectEnvironment.Data.TableDictionary[typeof(AirportHeliport)];

                IWorkspaceEdit workspaceEdit = (IWorkspaceEdit)(tbl as FeatureClass).Workspace;


                ITable fCls = EsriUtils.getTableByname((IFeatureWorkspace)workspaceEdit, vsTbl);//(IFeatureClass)util.GetTableByName(pntTbl, ArenaStatic.ArenaStaticProc.GetTargetDB());

                ICursor ft_cursor = fCls.Search(filter,false);
                IRow _Feature = null;
                while ((_Feature = ft_cursor.NextRow()) != null)
                {
                    obsIDs.Add(Convert.ToString(_Feature.get_Value(fCls.FindField("FeatureGUID"))));
                }



                if (obsIDs.Count > 0)
                {
                    var pdmList = (from element in DataCash.ProjectEnvironment.Data.PdmObjectList where (element != null) && (element is VerticalStructure) && (obsIDs.IndexOf(element.ID) >= 0) select element).ToList();
                    foreach (VerticalStructure Vs in pdmList)
                    {
                        foreach (VerticalStructurePart item in Vs.Parts)
                        {

                            if (item.Geo == null) item.RebuildGeo();

                            if (item.Geo == null) continue;

                            //System.Diagnostics.Debug.WriteLine(item.Geo.GeometryType.ToString());

                            //var resultGeo = topoOper.Intersect(item.Geo, esriGeometryDimension.esriGeometry2Dimension);
                            //if (resultGeo.Envelope.IsEmpty) continue;
                            res.Add(Vs);

                            break;

                        }
                    }
                }
   
                Marshal.ReleaseComObject(ft_cursor);

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            util = null;
            return res;
        }


        public static List<PDMObject> GetAerodromeObstacles(List<PDMObject> sourceList = null)
        {
            List<PDMObject> _list = (sourceList == null) ? DataCash.ProjectEnvironment.Data.PdmObjectList : sourceList;

            var pdmList = (from element in DataCash.ProjectEnvironment.Data.PdmObjectList where (element != null) && (element is VerticalStructure) && (((VerticalStructure)element).AirportAssociated) select element).ToList();

            return pdmList;
        }

        public static List<PDMObject> FilterObstaclesWithinPolygon(IGeometry Extent, double elevationLimitIn_M, double aboveLimit_M = -1, double belowLimit_M = -1, double RadiusInMeters = 5000)
        {
            List<PDMObject> VerticalPartsList = new List<PDMObject>();
            AranSupport.Utilitys util = new AranSupport.Utilitys();
            List<string> obsIDs = new List<string>();
            List<PDMObject> res = new List<PDMObject>();

            try
            {

                ISpatialFilter filter = new SpatialFilterClass();

                filter.Geometry = Extent;
                filter.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
                filter.WhereClause = "OBJECTID >0";


                string pntTbl = util.GetTblName(PDM_ENUM.VerticalStructure.ToString(), esriGeometryType.esriGeometryPoint);
                //string lnTbl = util.GetTblName(PDM_ENUM.VerticalStructure.ToString(), esriGeometryType.esriGeometryPolyline);
                //string plgTbl = util.GetTblName(PDM_ENUM.VerticalStructure.ToString(), esriGeometryType.esriGeometryPolygon);

                #region Point Obstacle

                IFeatureClass fCls = (IFeatureClass)util.GetTableByName(pntTbl, ArenaStatic.ArenaStaticProc.GetTargetDB());

                IFeatureCursor ft_cursor = fCls.Search(filter, false);
                IFeature _Feature = null;
                while ((_Feature = ft_cursor.NextFeature()) != null)
                {
                    obsIDs.Add(Convert.ToString(_Feature.get_Value(fCls.FindField("partID"))));
                }

                #endregion


                #region Line Obstacle

                //fCls = (IFeatureClass)util.GetTableByName(lnTbl, ArenaStatic.ArenaStaticProc.GetTargetDB());

                //ft_cursor = fCls.Search(filter, false);
                //_Feature = null;
                //while ((_Feature = ft_cursor.NextFeature()) != null)
                //{
                //    obsIDs.Add(Convert.ToString(_Feature.get_Value(fCls.FindField("partID"))));
                //}

                #endregion


                #region Polygon Obstacle

                //fCls = (IFeatureClass)util.GetTableByName(plgTbl, ArenaStatic.ArenaStaticProc.GetTargetDB());

                //ft_cursor = fCls.Search(filter, false);
                //_Feature = null;
                //while ((_Feature = ft_cursor.NextFeature()) != null)
                //{
                //    obsIDs.Add(Convert.ToString(_Feature.get_Value(fCls.FindField("partID"))));
                //}

                #endregion


                if (obsIDs.Count > 0)
                {
                    var pdmList = (from element in DataCash.ProjectEnvironment.Data.PdmObjectList where (element != null) && (element is VerticalStructure) select element).ToList();
                    foreach (VerticalStructure Vs in pdmList)
                    {
                        foreach (VerticalStructurePart partVS in Vs.Parts)
                        {

                            //if (partVS.Geo == null) partVS.RebuildGeo();
                            //if (partVS.Geo == null || partVS.Geo.GeometryType != esriGeometryType.esriGeometryPoint)   continue; 
                            if (partVS.VSGeoType != VerticalStructureGeoType.POINT) continue;
                            if (obsIDs.IndexOf(partVS.ID) >= 0)
                            {
                                int flag = partVS.Height == null ? 2 : 0;
                                if (aboveLimit_M > 0 && partVS.Height != null && partVS.Height.HasValue && partVS.ConvertValueToMeter(partVS.Height.Value, partVS.Height_UOM.ToString()) > aboveLimit_M)
                                    flag++;

                                if (belowLimit_M > 0 && partVS.Height != null && partVS.Height.HasValue && partVS.ConvertValueToMeter(partVS.Height.Value, partVS.Height_UOM.ToString()) < belowLimit_M)
                                    flag++;

                                if (partVS.Elev != null && partVS.Elev.HasValue && partVS.ConvertValueToMeter(partVS.Elev.Value, partVS.Elev_UOM.ToString()) > elevationLimitIn_M)
                                    flag++;

                                if (flag == 3)
                                    VerticalPartsList.Add(partVS);

                                break;
                            }
                        }
                    }



                    VerticalPartsList = VerticalPartsList.OrderByDescending(d => d.Elev).Where(r => r.Elev.HasValue).ToList();

                    var count = VerticalPartsList.Count;

                    for (int d = 0; d < count - 1; d++)
                    {
                        if (VerticalPartsList[d].Geo == null) VerticalPartsList[d].RebuildGeo();

                        IPoint vsPoint = (IPoint)VerticalPartsList[d].Geo;

                        for (int u = d + 1; u < count; u++)
                        {
                            if (VerticalPartsList[u].Geo == null) VerticalPartsList[u].RebuildGeo();

                            IPoint nearestPoint = (IPoint)VerticalPartsList[u].Geo;

                            var distance = util.GetDistanceBetweenPoints_Elips(vsPoint, nearestPoint);

                            if (distance < RadiusInMeters)
                            {
                                VerticalPartsList[d].SourceDetail = "Group";
                                VerticalPartsList.Remove(VerticalPartsList[u]);
                                u--;
                                count--;
                            }

                        }
                    }

                    Marshal.ReleaseComObject(ft_cursor);


                    foreach (var item in VerticalPartsList)
                    {
                        var vs = GetPDMObject(((VerticalStructurePart)item).VerticalStructure_ID, PDM_ENUM.VerticalStructure);
                        if (vs != null)
                        {
                            if (item.SourceDetail != null && item.SourceDetail.StartsWith("Group"))
                                ((VerticalStructure)vs).Group = true;
                            res.Add(vs);
                            item.SourceDetail = "";
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            util = null;
            return res;
        }

        public static AirportHeliport GetAirport(List<PDMObject> sourceList = null)
        {
            List<PDMObject> _list = (sourceList == null) ? DataCash.ProjectEnvironment.Data.PdmObjectList : sourceList;

            var res = (from element in _list where (element != null) && (element is AirportHeliport) select element).FirstOrDefault();
            if (res.Geo == null) res.RebuildGeo();
            return (AirportHeliport)res;
        }

        public static DateTime GetEfectiveDate(List<PDMObject> sourceList = null)
        {
            List<PDMObject> _list = (sourceList == null) ? DataCash.ProjectEnvironment.Data.PdmObjectList : sourceList;

            var res = (from element in _list where (element != null) && (element is AirportHeliport) select element).FirstOrDefault();
            return res.ActualDate;
        }

        public static void GetEfectiveDate(int AirackCircle, ref DateTime _start, ref DateTime _end)
        {
            AiracUtil.AiracUtil.GetAiracCirclePeriod(AirackCircle, ref _start, ref _end);
        }

        public static AirportHeliport GetAirport(string AirportID, List<PDMObject> sourceList = null)
        {
            List<PDMObject> _list = (sourceList == null) ? DataCash.ProjectEnvironment.Data.PdmObjectList : sourceList;

            var res = (from element in _list where (element != null) && (element is AirportHeliport) && (element.ID.CompareTo(AirportID) == 0) select element).FirstOrDefault();
            if (res!=null && res.Geo == null) res.RebuildGeo();
            return (AirportHeliport)res;
        }

        public static List<AirportHeliport> GetAirportlist(List<PDMObject> sourceList = null)
        {
            //string PathToRegionsFile = ArenaStaticProc.GetPathToRegionsFile();
            //var d = AreaManager.AreaUtils.GetCountryICAOCodes(PathToRegionsFile);

            List<PDMObject> _list = (sourceList == null) ? DataCash.ProjectEnvironment.Data.PdmObjectList : sourceList;

            var lst = (from element in _list  where (element != null) && (element is AirportHeliport) orderby ((AirportHeliport)element).Designator select element).ToList();
            List<AirportHeliport> res = new List<AirportHeliport>();
            foreach (var item in lst)
            {
                if (item.Geo == null) item.RebuildGeo();
                res.Add((AirportHeliport)item);
            }

            //if (res != null && res.Count > 0)
            //{
            //    string ADHP_Designator = res[0].Designator;
            //    string cntrIacaoCode = ADHP_Designator.Remove(2);

            //    if (DateTime.Now > new DateTime(2019, 6, 1))
            //    {
            //        res.RemoveAll(el => el.Designator.StartsWith("OT"));
            //    }
            //}

            return res;
        }

        public static List<AirportHeliport> GetAirportlistWithProcedures(PROC_TYPE_code procType, bool rnav , List<PDMObject> sourceList = null)
        {
            List<PDMObject> _list = (sourceList == null) ? DataCash.ProjectEnvironment.Data.PdmObjectList : sourceList;

            var lst = (from element in _list where (element != null) && (element is AirportHeliport) select element).ToList();
            List<AirportHeliport> res = new List<AirportHeliport>();
            foreach (var item in lst)
            {
                AirportHeliport arp = (AirportHeliport)item;

                if (GetAirportProcedures(procType,arp.ID,rnav,_list).Count >0 )
                res.Add((AirportHeliport)item);



            }
            return res;
        }

        public static List<Procedure> GetAirportProcedures(PROC_TYPE_code procType, string AirportID, bool rnav, List<PDMObject> sourceList = null)
        {
            List<Procedure> res = new List<Procedure>();
            List<PDMObject> _list = (sourceList == null) ? DataCash.ProjectEnvironment.Data.PdmObjectList : sourceList;

            var procList = (from element in _list
                            where (element != null) && (element is Procedure)
                                && (((Procedure)element).ProcedureType == procType)
                                && (((Procedure)element).RNAV == rnav)
                                && (((Procedure)element).AirportIdentifier.CompareTo(AirportID) == 0)
                             select element).ToList();

            foreach (var item in procList)
            {
                if (item is Procedure) res.Add((Procedure)item);
            }

            return res;
        }

        public static List<PDMObject> GetEnroutes(bool removeSame, CODE_ROUTE_SEGMENT_CODE_LVL codeLevel, List<PDMObject> sourceList = null)
        {
            List<PDMObject> res = new List<PDMObject>();
            List<RouteSegment> segList = new List<RouteSegment>();
            List<PDMObject> _list = (sourceList == null) ? DataCash.ProjectEnvironment.Data.PdmObjectList : sourceList;


            var rtsList = (from element in _list
                           where (element != null) && (element is Enroute)
                           select element).ToList();


            foreach (Enroute enrt in rtsList)
            {
                //if (enrt.TxtDesig.Contains("N685") || enrt.TxtDesig.Contains("L604"))
                {

                    Enroute clnEnrt = (Enroute)enrt.Clone(true);

                    for (int i = 0; i <= enrt.Routes.Count - 1; i++)
                    {
                        if ((enrt.Routes[i].CodeLvl == codeLevel) || (codeLevel == CODE_ROUTE_SEGMENT_CODE_LVL.BOTH)) clnEnrt.Routes[i] = (RouteSegment)enrt.Routes[i].Clone(true);
                        else { clnEnrt.Routes[i] = (RouteSegment)enrt.Routes[i].Clone(true); clnEnrt.Routes[i] = null; }
                    }

                    clnEnrt.Routes.RemoveAll(x => x == null);
                    clnEnrt.Routes.RemoveAll(x => x.StartPoint == null);
                    clnEnrt.Routes.RemoveAll(x => x.EndPoint == null);

                    if (clnEnrt.Routes.Count > 0) res.Add(clnEnrt);

                }
            }



            Dictionary<string, string> _sameRouteSegmentDictionary = new Dictionary<string, string>();
            Dictionary<string, CODE_ROUTE_SEGMENT_DIR> _sameRouteSegmentDirectionDictionary = new Dictionary<string, CODE_ROUTE_SEGMENT_DIR>();

            foreach (Enroute enrt in res)
            {

                foreach (RouteSegment rsg in enrt.Routes)
                {
                    try
                    {
                        segList.Add(rsg);

                        if (removeSame)
                        {
                            if (!_sameRouteSegmentDictionary.ContainsKey(rsg.ToString()) || !_sameRouteSegmentDictionary.ContainsKey(rsg.EndStartString()))
                            {
                                if (!_sameRouteSegmentDictionary.ContainsKey(rsg.ToString())) _sameRouteSegmentDictionary.Add(rsg.ToString(), enrt.TxtDesig);
                                if (!_sameRouteSegmentDictionary.ContainsKey(rsg.EndStartString())) _sameRouteSegmentDictionary.Add(rsg.EndStartString(), enrt.TxtDesig);

                                if (!_sameRouteSegmentDirectionDictionary.ContainsKey(rsg.ToString())) _sameRouteSegmentDirectionDictionary.Add(rsg.ToString(), rsg.CodeDir);
                                if (!_sameRouteSegmentDirectionDictionary.ContainsKey(rsg.EndStartString())) _sameRouteSegmentDirectionDictionary.Add(rsg.EndStartString(), rsg.CodeDir);

                            }
                            else
                            {
                                rsg.SourceDetail = "skip";
                                if (_sameRouteSegmentDictionary.ContainsKey(rsg.ToString()))
                                {
                                    if (!_sameRouteSegmentDictionary[rsg.ToString()].Contains("/" + rsg.RouteFormed))
                                        _sameRouteSegmentDictionary[rsg.ToString()] = rsg.RouteFormed.Contains(_sameRouteSegmentDictionary[rsg.ToString()]) ?
                                            rsg.RouteFormed : _sameRouteSegmentDictionary[rsg.ToString()] + "/" + rsg.RouteFormed;

                                    if (!_sameRouteSegmentDictionary[rsg.EndStartString()].Contains("/" + rsg.RouteFormed))
                                        _sameRouteSegmentDictionary[rsg.EndStartString()] = rsg.RouteFormed.Contains(_sameRouteSegmentDictionary[rsg.EndStartString()]) ?
                                            rsg.RouteFormed : _sameRouteSegmentDictionary[rsg.EndStartString()] + "/" + rsg.RouteFormed;

                                    if (_sameRouteSegmentDirectionDictionary[rsg.ToString()] != rsg.CodeDir)
                                    {
                                        _sameRouteSegmentDirectionDictionary[rsg.ToString()] = CODE_ROUTE_SEGMENT_DIR.BOTH;
                                    }

                                    if (_sameRouteSegmentDirectionDictionary.ContainsKey(rsg.EndStartString()) && _sameRouteSegmentDirectionDictionary[rsg.EndStartString()] != rsg.CodeDir)
                                    {
                                        _sameRouteSegmentDirectionDictionary[rsg.ToString()] = CODE_ROUTE_SEGMENT_DIR.BOTH;
                                    }
                                }

                                //rsg.SourceDetail = "skip";
                                //if (_sameRouteSegmentDictionary.ContainsKey(rsg.ToString()))
                                //{
                                //    _sameRouteSegmentDictionary[rsg.ToString()] = _sameRouteSegmentDictionary[rsg.ToString()] + "/" + rsg.RouteFormed;
                                //    _sameRouteSegmentDictionary[rsg.EndStartString()] = _sameRouteSegmentDictionary[rsg.EndStartString()] + "/" + rsg.RouteFormed;

                                //    if (_sameRouteSegmentDirectionDictionary[rsg.ToString()] != rsg.CodeDir)
                                //    {
                                //        _sameRouteSegmentDirectionDictionary[rsg.ToString()] = CODE_ROUTE_SEGMENT_DIR.BOTH;
                                //    }

                                //    if (_sameRouteSegmentDirectionDictionary.ContainsKey(rsg.EndStartString()) && _sameRouteSegmentDirectionDictionary[rsg.EndStartString()] != rsg.CodeDir)
                                //    {
                                //        _sameRouteSegmentDirectionDictionary[rsg.ToString()] = CODE_ROUTE_SEGMENT_DIR.BOTH;
                                //    }
                                //}


                            }
                        }

                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine(ex.Message + enrt.TxtDesig);
                        continue;
                    }
                }



            }

            if (removeSame)
            {
                foreach (Enroute itemRt in res)
                {
                    foreach (RouteSegment itemRs in itemRt.Routes)
                    {
                        if (_sameRouteSegmentDictionary.ContainsKey(itemRs.ToString()))
                        {
                            itemRs.RouteFormed = _sameRouteSegmentDictionary[itemRs.ToString()];
                            itemRs.CodeDir = _sameRouteSegmentDirectionDictionary[itemRs.ToString()];

                            ///////////////////////////////////////////////////////////////////
                            if (itemRs.SourceDetail == null) itemRs.SourceDetail = "";

                            string[] enrtNames = itemRs.RouteFormed.Split('/');

                            for (int i = 0; i <= enrtNames.Length - 1; i++)
                            {
                                var enrtName = _list.FindAll(x => x.PDM_Type == PDM_ENUM.Enroute && ((PDM.Enroute)x).TxtDesig.StartsWith(enrtNames[i])).FirstOrDefault();
                                string detail = itemRs.SourceDetail.StartsWith("skip") ? itemRt.ID : itemRs.SourceDetail;

                                detail = enrtName != null && ((PDM.Enroute)enrtName).ID.CompareTo(detail) == 0 ? "" : detail;

                                if (!itemRs.SourceDetail.StartsWith("skip") && !detail.Contains(((PDM.Enroute)enrtName).ID))
                                    itemRs.SourceDetail = enrtName != null ? detail + ((PDM.Enroute)enrtName).ID + "/" : "";


                            }

                            if (itemRs.CodeDir == CODE_ROUTE_SEGMENT_DIR.BOTH)
                            {

                                if (itemRs.ValMagTrack.ToString().StartsWith("NaN") && !itemRs.ValReversMagTrack.ToString().StartsWith("NaN"))
                                {
                                    itemRs.ValMagTrack = itemRs.ValReversMagTrack + 180;
                                    itemRs.ValMagTrack = itemRs.ValMagTrack > 360 ? itemRs.ValMagTrack - 360 : itemRs.ValMagTrack;
                                }

                                if (itemRs.ValReversMagTrack.ToString().StartsWith("NaN") && !itemRs.ValMagTrack.ToString().StartsWith("NaN"))
                                {
                                    itemRs.ValReversMagTrack = itemRs.ValMagTrack + 180;
                                    itemRs.ValReversMagTrack = itemRs.ValReversMagTrack > 360 ? itemRs.ValReversMagTrack - 360 : itemRs.ValReversMagTrack;
                                }

                                if (itemRs.ValTrueTrack.ToString().StartsWith("NaN") && !itemRs.ValReversTrueTrack.ToString().StartsWith("NaN"))
                                {
                                    itemRs.ValTrueTrack = itemRs.ValReversTrueTrack + 180;
                                    itemRs.ValTrueTrack = itemRs.ValTrueTrack > 360 ? itemRs.ValTrueTrack - 360 : itemRs.ValTrueTrack;
                                }

                                if (itemRs.ValReversTrueTrack.ToString().StartsWith("NaN") && !itemRs.ValTrueTrack.ToString().StartsWith("NaN"))
                                {
                                    itemRs.ValReversTrueTrack = itemRs.ValTrueTrack + 180;
                                    itemRs.ValReversTrueTrack = itemRs.ValReversTrueTrack > 360 ? itemRs.ValReversTrueTrack - 360 : itemRs.ValReversTrueTrack;
                                }


                            }
                        }


                    }
                }
            }

            _sameRouteSegmentDictionary = null;
            _sameRouteSegmentDirectionDictionary = null;

            /////////////////////////


            ////////////////////////

            return res;
        }

        public static List<PDMObject> GetEnroutes(CODE_ROUTE_SEGMENT_CODE_LVL codeLevel, List<PDMObject> sourceList = null)
        {
            List<PDMObject> res = new List<PDMObject>();
            List<PDMObject> _list = (sourceList == null) ? DataCash.ProjectEnvironment.Data.PdmObjectList : sourceList;

            var rtsList = (from element in _list
                           where (element != null) && (element is Enroute)
                           select element).ToList();

            foreach (Enroute enrt in rtsList)
            {

                Enroute clnEnrt = (Enroute)enrt.Clone();
                clnEnrt.ID = enrt.ID;
                for (int i = 0; i <= enrt.Routes.Count - 1; i++)
                {
                    if ((enrt.Routes[i].CodeLvl == codeLevel) || (codeLevel == CODE_ROUTE_SEGMENT_CODE_LVL.BOTH))
                    {
                        clnEnrt.Routes[i] = (RouteSegment)enrt.Routes[i].Clone(true);
                    }
                    else
                    {
                        clnEnrt.Routes[i] = (RouteSegment)enrt.Routes[i].Clone(true);
                        clnEnrt.Routes[i] = null;
                    }
                }

                clnEnrt.Routes.RemoveAll(x => x == null);
                if (clnEnrt.Routes.Count > 0) res.Add(clnEnrt);
            }


            return res;
        }

        public static List<PDMObject> GetRouteSegmentsList(List<PDMObject> sourceList = null)
        {
            List<PDMObject> res = new List<PDMObject>();
            List<PDMObject> _list = (sourceList == null) ? DataCash.ProjectEnvironment.Data.PdmObjectList : sourceList;

            var rtsList = (from element in _list
                           where (element != null) && (element is Enroute)
                           select element).ToList();

            foreach (Enroute enrt in rtsList)
            {

                for (int i = 0; i <= enrt.Routes.Count - 1; i++)
                {

                    res.Add((RouteSegment)enrt.Routes[i].Clone(true));

                }

            }


            return res;
        }

        public static List<PDMObject> GetEnroutes( List<PDMObject> sourceList = null)
        {
            List<PDMObject> res = new List<PDMObject>();
            List<PDMObject> _list = (sourceList == null) ? DataCash.ProjectEnvironment.Data.PdmObjectList : sourceList;

            var rtsList = (from element in _list
                           where (element != null) && (element is Enroute)
                           select element).ToList();

            res.AddRange(rtsList);


            return res;
        }


        public static PDMObject GetNavaidByID(string NavaidID, List<PDMObject> sourceList = null)
        {
            PDMObject res = null;
            List<PDMObject> _list = (sourceList == null) ? DataCash.ProjectEnvironment.Data.PdmObjectList : sourceList;

            res = (from element in _list
                           where (element != null) && (element is NavaidSystem)
                           && (((NavaidSystem)element).ID.CompareTo(NavaidID) == 0 || ((NavaidSystem)element).ID_Feature.CompareTo(NavaidID) == 0)
                           select element).FirstOrDefault();

            if (res == null)
            {
                bool break_flag = false;

                foreach (var obj in _list)
                {
                    if (obj.PDM_Type != PDM_ENUM.AirportHeliport) continue;

                    AirportHeliport arp = (AirportHeliport)obj;
                    if (arp.RunwayList == null || arp.RunwayList.Count == 0) continue;
                    
                    foreach (Runway rwy in arp.RunwayList)
                    {
                        if (rwy.RunwayDirectionList == null || rwy.RunwayDirectionList.Count == 0) continue;

                        foreach (RunwayDirection rdn in rwy.RunwayDirectionList)
                        {
                            if (rdn.Related_NavaidSystem == null) continue;
                            res = rdn.Related_NavaidSystem.FindAll(x => x.ID_Feature.StartsWith(NavaidID)).FirstOrDefault();

                            if (res == null)
                            {
                                foreach (NavaidSystem nvdSys in rdn.Related_NavaidSystem)
                                {
                                   var resCmpnt = nvdSys.Components.FindAll(x => x.ID.StartsWith(NavaidID)).FirstOrDefault();
                                    if (resCmpnt!=null)
                                    {
                                        res = nvdSys;
                                        break_flag = true;
                                        break;
                                    }
                                }
                            }
                            if (res != null)
                            {
                                break_flag = true;
                                break;
                            }
                        }

                        if (break_flag) break;
                    }

                    if (break_flag) break;
                }
            }

            return res;
        }

        public static PDMObject GetNavaidComponentByID(string NavaidID, List<PDMObject> sourceList = null)
        {
            PDMObject res = null;
            List<PDMObject> _list = (sourceList == null) ? DataCash.ProjectEnvironment.Data.PdmObjectList : sourceList;

            List<PDMObject> NvdList = (from element in _list
                   where (element != null) && (element.PDM_Type == PDM_ENUM.NavaidSystem) 
                   select element).ToList();

            foreach (NavaidSystem nav in NvdList)
            {
                if (nav.Components == null || nav.Components.Count <= 0) continue;
                res = nav.Components.Where(comp => comp.ID.CompareTo(NavaidID) == 0).FirstOrDefault();
                if (res != null) return res;
            }


            if (res == null)
            {
                bool break_flag = false;

                foreach (var obj in _list)
                {
                    if (obj.PDM_Type != PDM_ENUM.AirportHeliport) continue;

                    AirportHeliport arp = (AirportHeliport)obj;
                    if (arp.RunwayList == null || arp.RunwayList.Count == 0) continue;

                    foreach (Runway rwy in arp.RunwayList)
                    {
                        if (rwy.RunwayDirectionList == null || rwy.RunwayDirectionList.Count == 0) continue;

                        foreach (RunwayDirection rdn in rwy.RunwayDirectionList)
                        {
                            if (rdn.Related_NavaidSystem == null) continue;
                            res = rdn.Related_NavaidSystem.FindAll(x => x.ID_Feature.StartsWith(NavaidID)).FirstOrDefault();

                            if (res == null)
                            {
                                foreach (NavaidSystem nvdSys in rdn.Related_NavaidSystem)
                                {
                                    var resCmpnt = nvdSys.Components.FindAll(x => x.ID.StartsWith(NavaidID)).FirstOrDefault();
                                    if (resCmpnt != null)
                                    {
                                        res = nvdSys;
                                        break_flag = true;
                                        break;
                                    }
                                }
                            }
                            if (res != null)
                            {
                                break_flag = true;
                                break;
                            }
                        }

                        if (break_flag) break;
                    }

                    if (break_flag) break;
                }
            }

            return res;
        }


        public static PDMObject GetAirportNavaidByAirportID(string AirportID, NavaidSystemType navType, List<PDMObject> sourceList = null)
        {
            PDMObject res = null;
            List<PDMObject> _list = (sourceList == null) ? DataCash.ProjectEnvironment.Data.PdmObjectList : sourceList;

            res = (from element in _list
                   where (element != null) && (element is NavaidSystem) && (((NavaidSystem)element).CodeNavaidSystemType == navType)
                   && (((NavaidSystem)element).ID_AirportHeliport.CompareTo(AirportID) == 0)
                   select element).FirstOrDefault();

            return res;
        }

        public static List<PDMObject> GetAirportNavaidByAirportID(string AirportID, List<PDMObject> sourceList = null)
        {
            List<PDMObject> res = null;
            List<PDMObject> _list = (sourceList == null) ? DataCash.ProjectEnvironment.Data.PdmObjectList : sourceList;

            res = (from element in _list
                   where (element != null) && (element is NavaidSystem) 
                   && (((NavaidSystem)element).ID_AirportHeliport.CompareTo(AirportID) == 0)
                   select element).ToList();

            return res;
        }


        private static string toStr(string[] StrArray)
        {
            string res = "('";
            for (int i = 0; i <= StrArray.Length - 1; i++)
            {
                res = res + StrArray[i] + "','";
            }
            res = res + "0')";
            return res;
        }

        public static Runway GetRWY(string AirportID, string RwyID, List<PDMObject> sourceList = null)
        {
            List<PDMObject> _list = (sourceList == null) ? DataCash.ProjectEnvironment.Data.PdmObjectList : sourceList;
            Runway res = null;

            AirportHeliport arp = (AirportHeliport)(from element in _list where (element != null) && (element.PDM_Type == PDM_ENUM.AirportHeliport) && (element.ID.CompareTo(AirportID) == 0) select element).FirstOrDefault();

            foreach (Runway item in arp.RunwayList)
            {
                if (item.ID.CompareTo(RwyID) == 0)
                {
                    res = item;
                    break;
                }
            }

            return res;
        }

        public static PDMObject GetPDMObject(string ID, PDM_ENUM pdmtype,List<PDMObject> sourceList = null)
        {
            List<PDMObject> _list = (sourceList == null) ? DataCash.ProjectEnvironment.Data.PdmObjectList : sourceList;

            var res = (from element in _list where (element != null) && (element.PDM_Type == pdmtype) && (element.ID.CompareTo(ID) == 0) select element).FirstOrDefault();
            if (res == null && pdmtype == PDM_ENUM.ProcedureLeg)
            {
                bool BrFlag = false;
                var procList = (from element in _list where (element != null) && (element is  PDM.Procedure)  select element).ToList();
                foreach (Procedure proc in procList)
                {
                    if (proc.Transitions == null) continue;
                    foreach (var trans in proc.Transitions)
                    {
                        var procLeg = (from prc in trans.Legs where (prc != null) && (prc.ID.CompareTo(ID) == 0) select prc).FirstOrDefault();
                        if (procLeg !=null)
                        {
                            res = procLeg;
                            BrFlag = true;
                            break;
                        }

                        if (BrFlag) break;
                    }

                    if (BrFlag) break;
                }
            }
            if (res!=null && res.Geo == null) res.RebuildGeo();
            return (PDMObject)res;
        }

        public static PDMObject GetPDMObject(string ID,  List<PDMObject> sourceList = null)
        {
            List<PDMObject> _list = (sourceList == null) ? DataCash.ProjectEnvironment.Data.PdmObjectList : sourceList;

            var res = (from element in _list where (element != null)  && (element.ID.CompareTo(ID) == 0) select element).FirstOrDefault();
            if (res != null && res.Geo == null) res.RebuildGeo();
            return (PDMObject)res;
        }

        public static List<RadioCommunicationChanel> GetEnroutesChanels(CODE_ROUTE_SEGMENT_CODE_LVL codeLevel, List<PDMObject> sourceList = null)
        {
            List<RadioCommunicationChanel> res = new List<RadioCommunicationChanel>();
            List<PDMObject> _list = (sourceList == null) ? DataCash.ProjectEnvironment.Data.PdmObjectList : sourceList;

            var rtsList = (from element in _list
                           where (element != null) && (element is Enroute)
                           select element).ToList();

            foreach (Enroute enrt in rtsList)
            {

                for (int i = 0; i <= enrt.Routes.Count - 1; i++)
                {
                    if ((enrt.Routes[i].CodeLvl == codeLevel) || (codeLevel == CODE_ROUTE_SEGMENT_CODE_LVL.BOTH))
                    {
                        if (enrt.CommunicationChanels != null && enrt.CommunicationChanels.Count > 0)
                        {
                            res.AddRange(enrt.CommunicationChanels.GetRange(0, enrt.CommunicationChanels.Count));
                            break;
                        }
                    }
                }

            }


            return res;
        }

        public static List<RadioCommunicationChanel> GetAirspaceChanels(List<PDMObject> sourceList = null)
        {
            List<RadioCommunicationChanel> res = new List<RadioCommunicationChanel>();
            List<PDMObject> _list = (sourceList == null) ? DataCash.ProjectEnvironment.Data.PdmObjectList : sourceList;

            var arspsList = (from element in _list
                           where (element != null) && (element is Airspace) && (((Airspace)element).CommunicationChanels != null) && (((Airspace)element).CommunicationChanels.Count >0)
                           select element).ToList();

            foreach (Airspace arsps in arspsList)
            {

                if (arsps.CommunicationChanels != null && arsps.CommunicationChanels.Count > 0)
                {
                    res.AddRange(arsps.CommunicationChanels.GetRange(0, arsps.CommunicationChanels.Count));
                    
                }


            }


            return res;
        }

        public static List<PDMObject> GetAirspaceList(AirspaceType AirspaceCodeType, List<PDMObject> sourceList = null)
        {
            
            List<PDMObject> _list = (sourceList == null) ? DataCash.ProjectEnvironment.Data.PdmObjectList : sourceList;

            var res = (from element in _list
                             where (element != null) && (element is Airspace) && (((Airspace)element).CodeType == AirspaceCodeType)
                             select element).ToList();
            return res;
        }

    }
}
