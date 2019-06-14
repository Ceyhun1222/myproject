using Aerodrome.DataType;
using Aerodrome.Enums;
using Aerodrome.Features;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using Framework.Stasy.Context;
using Framework.Stasy.Helper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Framework.Stasy.SyncProvider
{
    public class GeoDbSyncProvider
    {
        public GeoDbSyncProvider()
        {
            ILayer _Layer = EsriUtils.getLayerByName(AerodromeDataCash.ProjectEnvironment.pMap, "AerodromeReferencePoint");
            var fc = ((IFeatureLayer)_Layer).FeatureClass;

            ISpatialReference sp = ((IGeoDataset)fc).SpatialReference;
            //// Workspace Geo
            //var workspaceEdit = (IWorkspaceEdit)fc.FeatureDataset.Workspace;

            var ws = (IWorkspaceEdit)((IDataset)AerodromeDataCash.ProjectEnvironment.pMap.Layer[0]).Workspace;

            AerodromeDataCash.ProjectEnvironment.FillAirtrackTableDic(ws);

        }

        public bool Insert(Dictionary<Type, ITable> AIRTRACK_TableDic, AM_AbstractFeature feature)
        {
            bool res = true;
            try
            {

                ITable tbl = AIRTRACK_TableDic[feature.GetType()];

                if (EsriUtils.RowExist(tbl, feature.featureID)) return true;

                IRow row = tbl.CreateRow();

                System.Reflection.PropertyInfo[] propInfos = feature.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);


                var geoProp = propInfos.FirstOrDefault(p => p.PropertyType.GetInterfaces().FirstOrDefault(i => i.FullName == typeof(IGeometry).FullName) != null);
                if (geoProp != null)
                {
                    var geo = geoProp.GetValue(feature);
                    if (geo != null)
                    {
                        IZAware toZAware = geo as IZAware;
                        if (toZAware != null)
                            toZAware.ZAware = false;
                        var geoIndx = row.Fields.FindField("Shape"); if (geoIndx >= 0) row.set_Value(geoIndx, geo);
                    }
                }

                int findx = -1;
                foreach (var prop in propInfos)
                {
                    var propType = prop.PropertyType;
                    var propName = prop.Name;

                    if (propType.Name == typeof(AM_Nullable<Type>).Name)
                    {
                        var instanceProps = propType.GetProperties();
                        
                        if (propType.GetGenericArguments()[0].IsEnum)
                        {
                            findx = row.Fields.FindField(propName);

                            if (prop.GetValue(feature) is null)
                            {
                                if (findx >= 0) row.set_Value(findx, AerodromeDataCash.ProjectEnvironment.DefaultValues.FirstOrDefault(d => d.NilReasonValue.Equals(NilReason.NotEntered)).ValueForString);
                                continue;
                            }

                            var nilValue = instanceProps[0].GetValue(prop.GetValue(feature));
                            var value = instanceProps[1].GetValue(prop.GetValue(feature));

                            if (nilValue != null)
                            {
                                if (findx >= 0) row.set_Value(findx, AerodromeDataCash.ProjectEnvironment.DefaultValues.FirstOrDefault(d => d.NilReasonValue.Equals(nilValue)).ValueForString);
                                continue;
                            }
                            if (findx >= 0) row.set_Value(findx, value.ToString());
                            continue;
                        }
                        if (propType.GetGenericArguments()[0].Name.Equals(typeof(DateTime).Name))
                        {
                            findx = row.Fields.FindField(propName);

                            if (prop.GetValue(feature) is null)
                            {
                                if (findx >= 0) row.set_Value(findx, AerodromeDataCash.ProjectEnvironment.DefaultValues.FirstOrDefault(d => d.NilReasonValue.Equals(NilReason.NotEntered)).ValueForDateTime.ToShortDateString());
                                continue;
                            }
                            var nilValue = instanceProps[0].GetValue(prop.GetValue(feature));
                            var value = instanceProps[1].GetValue(prop.GetValue(feature));

                            if (nilValue != null)
                            {
                                if (findx >= 0) row.set_Value(findx, AerodromeDataCash.ProjectEnvironment.DefaultValues.FirstOrDefault(d => d.NilReasonValue.Equals(nilValue)).ValueForDateTime.ToShortDateString());
                                continue;
                            }
                            if (findx >= 0) row.set_Value(findx, (DateTime)value);
                            continue;
                        }
                        if (propType.GetGenericArguments()[0].Name.Equals(typeof(string).Name))
                        {
                            findx = row.Fields.FindField(propName);

                            if (prop.GetValue(feature) is null)
                            {
                                if (findx >= 0) row.set_Value(findx, AerodromeDataCash.ProjectEnvironment.DefaultValues.FirstOrDefault(d => d.NilReasonValue.Equals(NilReason.NotEntered)).ValueForString);
                                continue;
                            }

                            var nilValue = instanceProps[0].GetValue(prop.GetValue(feature));
                            var value = instanceProps[1].GetValue(prop.GetValue(feature));
                          
                            if (nilValue != null)
                            {
                                if (findx >= 0) row.set_Value(findx, AerodromeDataCash.ProjectEnvironment.DefaultValues.FirstOrDefault(d => d.NilReasonValue.Equals(nilValue)).ValueForString);
                                continue;
                            }
                            if (findx >= 0) row.set_Value(findx, value);
                            continue;
                        }
                        if (propType.GetGenericArguments()[0].Name.Equals(typeof(double).Name))
                        {
                            findx = row.Fields.FindField(propName);

                            if (prop.GetValue(feature) is null)
                            {
                                if (findx >= 0) row.set_Value(findx, AerodromeDataCash.ProjectEnvironment.DefaultValues.FirstOrDefault(d => d.NilReasonValue.Equals(NilReason.NotEntered)).ValueForDouble);
                                continue;
                            }

                            var nilValue = instanceProps[0].GetValue(prop.GetValue(feature));
                            var value = instanceProps[1].GetValue(prop.GetValue(feature));
                           
                            if (nilValue != null)
                            {
                                if (findx >= 0) row.set_Value(findx, AerodromeDataCash.ProjectEnvironment.DefaultValues.FirstOrDefault(d => d.NilReasonValue.Equals(nilValue)).ValueForDouble);
                                continue;
                            }
                            if (findx >= 0) row.set_Value(findx, value);
                            continue;
                        }
                        //findx = row.Fields.FindField(propName); if (findx >= 0) row.set_Value(findx, value);
                        continue;
                    }


                    if (propType.Name == typeof(DataType<Enum>).Name)
                    {
                        var instanceProps = propType.GetProperties();
                        var dataTypeValue = prop.GetValue(feature);
                        if (dataTypeValue != null)
                        {
                            var value = instanceProps[0].GetValue(dataTypeValue);
                            if (value != null)
                                findx = row.Fields.FindField(propName); if (findx >= 0) row.set_Value(findx, value);
                        }
                        continue;
                    }

                    var simpleValue = prop.GetValue(feature);
                    if (propType.IsEnum)
                    {
                        findx = row.Fields.FindField(propName); if (findx >= 0) row.set_Value(findx, simpleValue.ToString());
                        continue;
                    }
                    if (propType == typeof(DateTime))
                    {
                        findx = row.Fields.FindField(propName); if (findx >= 0) row.set_Value(findx, ((DateTime)simpleValue).ToShortDateString());
                        continue;
                    }
                    findx = row.Fields.FindField(propName); if (findx >= 0) row.set_Value(findx, simpleValue);


                }

                //CompileRow(ref row);

                row.Store();


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            return res;
        }

        public bool Update(Dictionary<Type, ITable> AIRTRACK_TableDic, AM_AbstractFeature feature)
        {
            bool res = true;
            try
            {

                ITable tbl = AIRTRACK_TableDic[feature.GetType()];
                
                IRow row = EsriUtils.GetRowByID(tbl, feature.featureID);
                GC.Collect();
                if (row == null)
                    return false;

                System.Reflection.PropertyInfo[] propInfos = feature.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);


                var geoProp = propInfos.FirstOrDefault(p => p.PropertyType.GetInterfaces().FirstOrDefault(i => i.FullName == typeof(IGeometry).FullName) != null);
                if (geoProp != null)
                {
                    var geo = geoProp.GetValue(feature);
                    if(geo is null)
                    {
                        var geoIndx = row.Fields.FindField("Shape"); if (geoIndx >= 0) row.set_Value(geoIndx, DBNull.Value);
                    }
                    else
                    {
                        IZAware toZAware = geo as IZAware;
                        if (toZAware != null)
                            toZAware.ZAware = false;
                        var geoIndx = row.Fields.FindField("Shape"); if (geoIndx >= 0) row.set_Value(geoIndx, geo);
                    }

                }

                int findx = -1;
                foreach (var prop in propInfos)
                {
                    var propType = prop.PropertyType;
                    var propName = prop.Name;

                    if (propType.Name == typeof(AM_Nullable<Type>).Name)
                    {
                        var instanceProps = propType.GetProperties();
                        var nilValue= instanceProps[0].GetValue(prop.GetValue(feature));
                        var value = instanceProps[1].GetValue(prop.GetValue(feature));

                        if (propType.GetGenericArguments()[0].IsEnum)
                        {
                            findx = row.Fields.FindField(propName);

                            if (nilValue!=null)
                            {
                                if (findx >= 0) row.set_Value(findx, AerodromeDataCash.ProjectEnvironment.DefaultValues.FirstOrDefault(d=>d.NilReasonValue.Equals(nilValue)).ValueForString);
                                continue;
                            }
                            if (findx >= 0) row.set_Value(findx, value.ToString());
                            continue;
                        }
                        if (propType.GetGenericArguments()[0].Name.Equals(typeof(DateTime).Name))
                        {
                            findx = row.Fields.FindField(propName);
                            if (nilValue != null)
                            {
                                if (findx >= 0) row.set_Value(findx, AerodromeDataCash.ProjectEnvironment.DefaultValues.FirstOrDefault(d => d.NilReasonValue.Equals(nilValue)).ValueForDateTime.ToShortDateString());
                                continue;
                            }
                            if (findx >= 0) row.set_Value(findx, (DateTime)value);
                            continue;
                        }
                        if (propType.GetGenericArguments()[0].Name.Equals(typeof(string).Name))
                        {
                            findx = row.Fields.FindField(propName);
                            if (nilValue != null)
                            {
                                if (findx >= 0) row.set_Value(findx, AerodromeDataCash.ProjectEnvironment.DefaultValues.FirstOrDefault(d => d.NilReasonValue.Equals(nilValue)).ValueForString);
                                continue;
                            }
                            if (findx >= 0) row.set_Value(findx, value);
                            continue;
                        }
                        if (propType.GetGenericArguments()[0].Name.Equals(typeof(double).Name))
                        {
                            findx = row.Fields.FindField(propName);
                            if (nilValue != null)
                            {
                                if (findx >= 0) row.set_Value(findx, AerodromeDataCash.ProjectEnvironment.DefaultValues.FirstOrDefault(d => d.NilReasonValue.Equals(nilValue)).ValueForDouble);
                                continue;
                            }
                            if (findx >= 0) row.set_Value(findx, value);
                            continue;
                        }
                        findx = row.Fields.FindField(propName); if (findx >= 0) row.set_Value(findx, value);
                        continue;
                    }


                    if (propType.Name == typeof(DataType<Enum>).Name)
                    {
                        var instanceProps = propType.GetProperties();
                        var dataTypeValue = prop.GetValue(feature);
                        if (dataTypeValue is null)
                        {
                            findx = row.Fields.FindField(propName); if (findx >= 0) row.set_Value(findx, DBNull.Value);
                            continue;
                        }
                        var value = instanceProps[0].GetValue(dataTypeValue);
                        findx = row.Fields.FindField(propName); if (findx >= 0) row.set_Value(findx, value);

                        continue;
                    }

                    var simpleValue = prop.GetValue(feature);
                    if (propType.IsEnum)
                    {
                        findx = row.Fields.FindField(propName); if (findx >= 0) row.set_Value(findx, simpleValue.ToString());
                        continue;
                    }
                    if (propType ==typeof(DateTime))
                    {
                        findx = row.Fields.FindField(propName); if (findx >= 0) row.set_Value(findx, ((DateTime)simpleValue).ToShortDateString());
                        continue;
                    }
                    findx = row.Fields.FindField(propName); if (findx >= 0) row.set_Value(findx, simpleValue);


                }

                //CompileRow(ref row);

                row.Store();


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            return res;
        }

        public bool UpdateSelectedRows(Type type, IEnumerable<string> idList, List<System.Reflection.PropertyInfo> changedProps, object feature)
        {
            try
            {
                ITable tbl = AerodromeDataCash.ProjectEnvironment.TableDictionary[type];

                #region Fill buffer with changed properties

                IRowBuffer buffer = tbl.CreateRowBuffer();
                
                int findx = -1;
                foreach (var prop in changedProps)
                {
                    var propType = prop.PropertyType;
                    var propName = prop.Name;

                    if (propType.Name == typeof(AM_Nullable<Type>).Name)
                    {
                        var instanceProps = propType.GetProperties();
                        var nilValue = instanceProps[0].GetValue(prop.GetValue(feature));
                        var value = instanceProps[1].GetValue(prop.GetValue(feature));

                        if (propType.GetGenericArguments()[0].IsEnum)
                        {
                            findx = buffer.Fields.FindField(propName);

                            if (nilValue != null)
                            {
                                if (findx >= 0) buffer.set_Value(findx, AerodromeDataCash.ProjectEnvironment.DefaultValues.FirstOrDefault(d => d.NilReasonValue.Equals(nilValue)).ValueForString);
                                continue;
                            }
                            if (findx >= 0) buffer.set_Value(findx, value.ToString());
                            continue;
                        }
                        if (propType.GetGenericArguments()[0].Name.Equals(typeof(DateTime).Name))
                        {
                            findx = buffer.Fields.FindField(propName);
                            if (nilValue != null)
                            {
                                if (findx >= 0) buffer.set_Value(findx, AerodromeDataCash.ProjectEnvironment.DefaultValues.FirstOrDefault(d => d.NilReasonValue.Equals(nilValue)).ValueForDateTime.ToShortDateString());
                                continue;
                            }
                            if (findx >= 0) buffer.set_Value(findx, (DateTime)value);
                            continue;
                        }
                        if (propType.GetGenericArguments()[0].Name.Equals(typeof(string).Name))
                        {
                            findx = buffer.Fields.FindField(propName);
                            if (nilValue != null)
                            {
                                if (findx >= 0) buffer.set_Value(findx, AerodromeDataCash.ProjectEnvironment.DefaultValues.FirstOrDefault(d => d.NilReasonValue.Equals(nilValue)).ValueForString);
                                continue;
                            }
                            if (findx >= 0) buffer.set_Value(findx, value);
                            continue;
                        }
                        if (propType.GetGenericArguments()[0].Name.Equals(typeof(double).Name))
                        {
                            findx = buffer.Fields.FindField(propName);
                            if (nilValue != null)
                            {
                                if (findx >= 0) buffer.set_Value(findx, AerodromeDataCash.ProjectEnvironment.DefaultValues.FirstOrDefault(d => d.NilReasonValue.Equals(nilValue)).ValueForDouble);
                                continue;
                            }
                            if (findx >= 0) buffer.set_Value(findx, value);
                            continue;
                        }
                        findx = buffer.Fields.FindField(propName); if (findx >= 0) buffer.set_Value(findx, value);
                        continue;
                    }


                    if (propType.Name == typeof(DataType<Enum>).Name)
                    {
                        var instanceProps = propType.GetProperties();
                        var dataTypeValue = prop.GetValue(feature);
                        if (dataTypeValue is null)
                        {
                            findx = buffer.Fields.FindField(propName); if (findx >= 0) buffer.set_Value(findx, DBNull.Value);
                            continue;
                        }
                        var value = instanceProps[0].GetValue(dataTypeValue);
                        findx = buffer.Fields.FindField(propName); if (findx >= 0) buffer.set_Value(findx, value);

                        continue;
                    }

                    var simpleValue = prop.GetValue(feature);
                    if (propType.IsEnum)
                    {
                        findx = buffer.Fields.FindField(propName); if (findx >= 0) buffer.set_Value(findx, simpleValue.ToString());
                        continue;
                    }
                    if (propType == typeof(DateTime))
                    {
                        findx = buffer.Fields.FindField(propName); if (findx >= 0) buffer.set_Value(findx, ((DateTime)simpleValue).ToShortDateString());
                        continue;
                    }
                    findx = buffer.Fields.FindField(propName); if (findx >= 0) buffer.set_Value(findx, simpleValue);

                }

                #endregion

                IQueryFilter queryFilter = new QueryFilterClass();
                queryFilter.SubFields = string.Join(",", changedProps.Where(t => tbl.Fields.FindField(t.Name) >= 0).Select(p => p.Name));
                List<string> selectedFeatureIdList = new List<string>();
                int selectedCount = idList.Count();
                int iteration = 0;
                int step = 250;
                while (selectedCount / (step + iteration * step) > 0)
                {
                    selectedFeatureIdList = new List<string>();
                    var iterIdList = idList.Skip(iteration * step).Take(step);
                    foreach (string id in iterIdList)
                    {
                        selectedFeatureIdList.Add(nameof(AM_AbstractFeature.featureID) + " =" + "'" + id + "'");
                    }
                    var resultIdList = string.Join(" OR ", selectedFeatureIdList);
                    queryFilter.WhereClause = resultIdList;
                    
                    tbl.UpdateSearchedRows(queryFilter,buffer);
                    iteration += 1;
                }

                selectedFeatureIdList = new List<string>();
                var idLastList = idList.Skip(iteration * step).Take(step);
                foreach (string id in idLastList)
                {
                    selectedFeatureIdList.Add(nameof(AM_AbstractFeature.featureID) + " =" + "'" + id + "'");
                }
                var result = string.Join(" OR ", selectedFeatureIdList);
                queryFilter.WhereClause = result;
                tbl.UpdateSearchedRows(queryFilter, buffer);
  
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool DeleteSelectedRows(Type type,IEnumerable<string> idList)
        {
            try
            {
                ITable tbl = AerodromeDataCash.ProjectEnvironment.TableDictionary[type];
                IQueryFilter queryFilter = new QueryFilterClass();
                List<string> selectedFeatureIdList = new List<string>();
                int selectedCount = idList.Count();
                int iteration = 0;
                int step = 250;
                while (selectedCount / (step + iteration * step) > 0)
                {
                    selectedFeatureIdList = new List<string>();
                    var iterIdList = idList.Skip(iteration * step).Take(step);
                    foreach (string id in iterIdList)
                    {
                        selectedFeatureIdList.Add(nameof(AM_AbstractFeature.featureID) + " =" + "'" + id + "'");

                    }
                    var resultIdList = string.Join(" OR ", selectedFeatureIdList);
                    queryFilter.WhereClause = resultIdList;
                    tbl.DeleteSearchedRows(queryFilter);
                    //ICursor cursor = tbl.Update(queryFilter, false);
                    //Delete(cursor);
                    iteration += 1;
                }

                selectedFeatureIdList = new List<string>();
                var idLastList = idList.Skip(iteration * step).Take(step);
                foreach (string id in idLastList)
                {
                    selectedFeatureIdList.Add(nameof(AM_AbstractFeature.featureID) + " =" + "'" + id + "'");
                }
                var result = string.Join(" OR ", selectedFeatureIdList);
                queryFilter.WhereClause = result;
                tbl.DeleteSearchedRows(queryFilter);
                //ICursor lastCursor = tbl.Update(queryFilter, false);
                //Delete(lastCursor);

                return true;
            }
            catch (Exception)
            {

                return false;
            }
           
        }

        public bool Delete(ICursor featureCursor)
        {
            try
            {
                var row = featureCursor.NextRow();

                while(row!=null)
                {
                    row.Delete();
                    //GC.Collect();
                    row = featureCursor.NextRow();
                }
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public bool Delete(Dictionary<Type, ITable> AIRTRACK_TableDic, string idnumber, Type feattype)
        {
            bool res = true;
            try
            {
                ITable tbl = AIRTRACK_TableDic[feattype];

                IRow row = EsriUtils.GetRowByID(tbl, idnumber);
                GC.Collect();
                if (row == null)
                    return false;

                row.Delete();
                return true;
            }
            catch
            {
                return false;
            }
        }

    }

}
