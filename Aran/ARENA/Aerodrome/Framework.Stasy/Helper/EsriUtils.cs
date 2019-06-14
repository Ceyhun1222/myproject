using Aerodrome.Enums;
using Aerodrome.Features;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using Framework.Stasy.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Stasy.Helper
{
    public static class EsriUtils
    {
        //mdb helper methods
        public static ILayer getLayerByName(IMap FocusMap, string layerName)
        {
            ILayer res = null;
            bool ok = false;
            string _name;
            try
            {

                for (int i = 0; i <= FocusMap.LayerCount - 1; i++)
                {
                    ILayer Layer1 = FocusMap.get_Layer(i);
                    _name = Layer1.Name;
                    if (Layer1 is FeatureLayer && ((IFeatureLayer)Layer1).FeatureClass != null) _name = ((IFeatureLayer)Layer1).FeatureClass.AliasName;
                    if (_name.ToUpper().CompareTo(layerName.ToUpper()) == 0)
                    {
                        res = Layer1;
                        ok = true;
                    }
                    else if (Layer1 is ICompositeLayer)
                    {
                        ICompositeLayer Complayer = (ICompositeLayer)Layer1;
                        for (int j = 0; j <= Complayer.Count - 1; j++)
                        {
                            ILayer Layer2 = Complayer.get_Layer(j);
                            _name = Layer2.Name;
                            if (Layer2 is FeatureLayer && ((IFeatureLayer)Layer2).FeatureClass != null) _name = ((IFeatureLayer)Layer2).FeatureClass.AliasName;
                            if (Layer2 is FDOGraphicsLayer) { FDOGraphicsLayer annoL = (FDOGraphicsLayer)Layer2; _name = ((IFeatureLayer)annoL).Name; }
                            if (_name.ToUpper().CompareTo(layerName.ToUpper()) == 0)
                            {
                                res = Layer2;
                                ok = true;
                                break;
                            }
                        }
                    }

                    if (ok) break;
                }
            }
            catch
            {
                res = null;
            }

            return res;

        }

        public static ILayer getLayerByName2(IMap FocusMap, string layerName, bool IgnoreCompositeLayer = false)
        {
            ILayer res = null;
            bool ok = false;
            string _name;
            try
            {
                for (int i = 0; i <= FocusMap.LayerCount - 1; i++)
                {
                    ILayer Layer1 = FocusMap.get_Layer(i);
                    _name = Layer1.Name;
                    if (Layer1 is FeatureLayer && ((IFeatureLayer)Layer1).FeatureClass != null) _name = ((IFeatureLayer)Layer1).FeatureClass.AliasName;
                    if (_name.StartsWith(layerName))
                    {
                        res = Layer1;
                        ok = true;
                    }
                    else if ((Layer1 is ICompositeLayer) && (!IgnoreCompositeLayer))
                    {
                        ICompositeLayer Complayer = (ICompositeLayer)Layer1;
                        for (int j = 0; j <= Complayer.Count - 1; j++)
                        {
                            ILayer Layer2 = Complayer.get_Layer(j);
                            _name = Layer2.Name;
                            if (Layer2 is FeatureLayer && ((IFeatureLayer)Layer2).FeatureClass != null) _name = ((IFeatureLayer)Layer2).FeatureClass.AliasName;
                            if (Layer2 is FDOGraphicsLayer) { FDOGraphicsLayer annoL = (FDOGraphicsLayer)Layer2; _name = ((IFeatureLayer)annoL).Name; }

                            if (_name.ToUpper().StartsWith(layerName.ToUpper()))
                            {
                                res = Layer2;
                                ok = true;
                                break;
                            }
                        }
                    }

                    if (ok) break;
                }
            }
            catch
            {
                res = null;
            }

            return res;

        }


        public static ITable getTableByname(IFeatureWorkspace featureWorkspace, string nameOfTable)
        {
            try
            {
                if (((IWorkspace2)featureWorkspace).NameExists[esriDatasetType.esriDTFeatureClass, nameOfTable])
                    return featureWorkspace.OpenTable(nameOfTable);
                else if (((IWorkspace2)featureWorkspace).NameExists[esriDatasetType.esriDTTable, nameOfTable])
                    return featureWorkspace.OpenTable(nameOfTable);
                else
                    return null;
            }
            catch
            {

                return null;
            }

        }

        public static void FillAirtrackTableDic(IWorkspaceEdit workspaceEdit)
        {
            try
            {
                AerodromeDataCash.ProjectEnvironment.TableDictionary = new Dictionary<Type, ITable>();
                Dictionary<string, Type> tables = new Dictionary<string, Type>();

                Assembly asm = typeof(AM_AerodromeReferencePoint).Assembly;


                foreach (var amObj in Enum.GetValues(typeof(Feat_Type)))
                {
                    string featName = amObj.ToString().Replace("_", String.Empty);
                    Type amFeature = asm.GetType(asm.GetName().Name + ".AM_" + featName);

                    tables.Add(featName, amFeature);

                }
                
                        
                     
                foreach (var table in tables)
                {
                    if (EsriUtils.getTableByname((IFeatureWorkspace)workspaceEdit, table.Key) != null)
                    {
                        AerodromeDataCash.ProjectEnvironment.TableDictionary.Add(table.Value,
                            EsriUtils.getTableByname((IFeatureWorkspace)workspaceEdit, table.Key));
                    }
                }

            }
            catch (Exception ex)
            {
              
            }

        }

        public static bool RowExist(ITable tbl, string FieldValue, string FieldName = nameof(AM_AbstractFeature.featureID))
        {
            IQueryFilter queryFilter = new QueryFilterClass();

            queryFilter.WhereClause = FieldName + " = '" + FieldValue + "'";

            ICursor crsr = tbl.Search(queryFilter, false);
            bool res = crsr.NextRow() != null;
            Marshal.ReleaseComObject(crsr);

            return res;

        }

        public static IRow GetRowByID(ITable tbl, string FieldValue, string FieldName = nameof(AM_AbstractFeature.featureID))
        {
            IQueryFilter queryFilter = new QueryFilterClass();

            queryFilter.WhereClause = FieldName + " = '" + FieldValue + "'";

            ICursor crsr = tbl.Search(queryFilter, false);

            return crsr.NextRow();

        }

        public static void ChangeProjectionAndMeredian(double CMeridian, IMap pMap)
        {
            //IMap pMap = pDocument.FocusMap;

            ISpatialReferenceFactory2 pSpatRefFact = new SpatialReferenceEnvironmentClass();
            IProjectionGEN pProjection = pSpatRefFact.CreateProjection((int)esriSRProjectionType.esriSRProjection_TransverseMercator) as IProjectionGEN;

            IGeographicCoordinateSystem pGCS = pSpatRefFact.CreateGeographicCoordinateSystem((int)esriSRGeoCSType.esriSRGeoCS_WGS1984);
            ILinearUnit pLinearUnit = pSpatRefFact.CreateUnit((int)esriSRUnitType.esriSRUnit_Meter) as ILinearUnit;
            IProjectedCoordinateSystemEdit pProjCoordSysEdit = new ProjectedCoordinateSystemClass();
            IParameter[] pParams = pProjection.GetDefaultParameters();
            pParams[0].Value = 500000;
            pParams[1].Value = 0;
            pParams[2].Value = Math.Round(CMeridian, 6);
            pParams[3].Value = (double)0.9996;

            object name = "Transverse_Mercator";
            object alias = "UserDefinedAlias";
            object abbreviation = "Trans_Merc";
            object remarks = "ARAN coordinate system.";
            object usage = "";
            object CS = pGCS;
            object LU = pLinearUnit;
            object PRJ = pProjection;
            object PARAMS = pParams;

            pProjCoordSysEdit.Define(ref name, ref alias, ref abbreviation, ref remarks, ref usage, ref CS, ref LU, ref PRJ, ref PARAMS);

            ISpatialReference pPCS = (ESRI.ArcGIS.Geometry.IProjectedCoordinateSystem)pProjCoordSysEdit; // pRJ
            if (pMap != null)
            {
                pMap.SpatialReference = pPCS;
            }

        }

        public static ISpatialReference CreateSpatialReferenceByMeridian(double longitude)
        {
            
            var utmZone = Math.Floor((longitude + 180) / 6) + 1;

            ISpatialReferenceFactory spatialReferenceFactory = new SpatialReferenceEnvironmentClass();

            ISpatialReference spatialReference =

            spatialReferenceFactory.CreateProjectedCoordinateSystem(326 * 100 + Int32.Parse(utmZone.ToString()));

            ISpatialReferenceResolution spatialReferenceResolution = (ISpatialReferenceResolution)spatialReference;

            spatialReferenceResolution.ConstructFromHorizon();

            ISpatialReferenceTolerance spatialReferenceTolerance = (ISpatialReferenceTolerance)spatialReference;

            spatialReferenceTolerance.SetDefaultXYTolerance();

            return spatialReference;


        }
    }
}
