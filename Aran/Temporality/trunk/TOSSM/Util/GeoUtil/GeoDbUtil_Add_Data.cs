using System;
using System.Collections.Generic;
using System.Linq;
using Aran.Aim;
using Aran.Aim.DataTypes;
using Aran.Converters;
using Aran.Temporality.Common.ArcGis;
using Aran.Temporality.Common.Entity.Util;
using Aran.Temporality.CommonUtil.Control.TreeViewDragDrop;
using Aran.Temporality.CommonUtil.ViewModel;
using ESRI.ArcGIS.Geodatabase;
using TOSSM.ViewModel.Document;

namespace TOSSM.Util
{
    public class TableInfo
    {
        public string TableName;
        public string DatasetName;
        public string Path;
    }

    public class AddDataContext
    {
        public string StatusPrefix;
        public List<TableInfo> RelatedTableInfo;
        public IFeatureWorkspace FeatureWorkspace;
        public Action<string> StatusChangedAction;
        public Dictionary<FeatureType, Dictionary<Guid, LightFeature>> DataToBeAdded = new Dictionary<FeatureType, Dictionary<Guid, LightFeature>>(); 

        public void SetStatus(string s)
        {
            if (StatusChangedAction != null)
            {
                if (!string.IsNullOrEmpty(StatusPrefix))
                {
                    s = StatusPrefix+s;
                }
                StatusChangedAction(s);
            }
        }

        public void ProcessLightFeature(LightFeature lightFeature)
        {
            Dictionary<Guid, LightFeature> lightDictionary;
            if (!DataToBeAdded.TryGetValue((FeatureType)lightFeature.FeatureType, out lightDictionary))
            {
                lightDictionary = new Dictionary<Guid, LightFeature>();
                DataToBeAdded[(FeatureType) lightFeature.FeatureType] = lightDictionary;
            }
            LightFeature oldLightFeature;
            if (!lightDictionary.TryGetValue(lightFeature.Guid, out oldLightFeature))
            {
                lightDictionary[lightFeature.Guid] = lightFeature;
            }
            else
            {
                //merge
                lightDictionary[lightFeature.Guid] = MergeLightFeatures(oldLightFeature,lightFeature);
            }
            
            foreach (var link in lightFeature.Links??new LightLink[0])
            {
                if ((link.Flag & LightData.Missing)==0)//not missing
                {
                    ProcessLightFeature(link.Value);
                }
            }
        }

        private LightFeature MergeLightFeatures(LightFeature oldLightFeature, LightFeature lightFeature)
        {
            if (oldLightFeature == null && lightFeature == null) return null;
            if (oldLightFeature == null) return lightFeature;
            if (lightFeature == null) return oldLightFeature;

            //merge simple fields
            var newSimpleFields = (lightFeature.Fields ?? new LightField[0]).Where(newField => (oldLightFeature.Fields ?? new LightField[0]).
                All(t => t.Name != newField.Name)).ToList();
            var fields = (oldLightFeature.Fields??new LightField[0]).ToList();
            fields.AddRange(newSimpleFields);
            oldLightFeature.Fields = fields.ToArray();

            //merge complex fieilds
            var newComplexFields = new List<LightComplexField>();
            foreach (var newComplexField in lightFeature.ComplexFields??new LightComplexField[0])
            {
                var correspondingOldComplexField =
                    (oldLightFeature.ComplexFields??new LightComplexField[0]).FirstOrDefault(t => t.Name == newComplexField.Name);

                newComplexFields.Add(correspondingOldComplexField == null
                    ? newComplexField
                    : MergeComplexField(correspondingOldComplexField, newComplexField));
            }
            oldLightFeature.ComplexFields = newComplexFields.ToArray();

            //merge links
            var newLinks = (lightFeature.Links??new LightLink[0]).Where(newLink => (oldLightFeature.Links??new LightLink[0]).All(t => t.Name != newLink.Name)).ToList();
            var links = (oldLightFeature.Links??new LightLink[0]).ToList();
            links.AddRange(newLinks);
            oldLightFeature.Links = links.ToArray();
            return oldLightFeature;
        }

        private LightComplexField MergeComplexField(LightComplexField correspondingOldComplexField, LightComplexField newComplexField)
        {
            if (newComplexField == null && correspondingOldComplexField == null) return null;
            if (correspondingOldComplexField == null) return newComplexField;
            if (newComplexField == null) return correspondingOldComplexField;
         
            correspondingOldComplexField.Value = MergeLightFeatures(correspondingOldComplexField.Value,
                newComplexField.Value);
            return correspondingOldComplexField;
        }
    }

    public partial class GeoDbUtil
    {
        public static void AddDataToGeoDb(string fileName,
            ConfigurationViewModel selectedConfiguration, List<LightFeature> list,
            Action<string> onStatusChanged = null)
        {
            var extension = System.IO.Path.GetExtension(fileName);

            var folder = System.IO.Path.GetDirectoryName(fileName);

            if (extension==null) throw new Exception("Abnormal file name.");

            

            var workspace = extension.ToLower() == ".mdb"?OpenAccessWorkspace(fileName)
                : OpenFileGdbWorkspace(folder);

            IFeatureWorkspace featureWorkspace = workspace as IFeatureWorkspace;
            if (featureWorkspace == null)
            {
                throw new Exception("Abnormal workspace.");
            }

            var configurationModel = new FeatureDependencyConfigurationViewModel();
            configurationModel.Deserialize(selectedConfiguration.Entity.Data);

            ITable tableInfo = featureWorkspace.OpenTable("Table_Info");
            if (tableInfo == null)
            {
                throw new Exception("Bad mdb file format");
            }

            IQueryFilter queryFilter = new QueryFilterClass();
            int tableNameIndex = tableInfo.FindField("TableName");
            int datasetNameIndex = tableInfo.FindField("DatasetName");
            int pathIndex = tableInfo.FindField("Path");

            List<TableInfo> tableInfoList = new List<TableInfo>();

            ICursor cursor = tableInfo.Search(queryFilter, true);
            IRow row = null;
            while ((row = cursor.NextRow()) != null)
            {
                tableInfoList.Add(new TableInfo
                {
                    TableName = Convert.ToString(row.Value[tableNameIndex]),
                    DatasetName = Convert.ToString(row.Value[datasetNameIndex]),
                    Path = Convert.ToString(row.Value[pathIndex])
                });
            }

            var context = new AddDataContext
            {
                FeatureWorkspace = featureWorkspace,
                RelatedTableInfo = tableInfoList,
                StatusChangedAction = onStatusChanged
            };

            //save fields
            context.StatusPrefix = "Processing data";
            var count = 0;
            foreach (var lightFeature in list)
            {
                count++;
                context.SetStatus(" " + count + " from " + list.Count);
                context.ProcessLightFeature(lightFeature);
            }
            SaveFields(context);

            //save links
            SaveLinks(context);
         

            context.StatusPrefix = null;
            context.SetStatus("Done");
        }


        private static void SaveLinks(AddDataContext context)
        {
            //save links
            context.StatusPrefix = "Processing links";
            var count = 0;

            foreach (var pair in context.DataToBeAdded)
            {
                count++;
                context.StatusPrefix = " " + count + " from " + pair.Value.Count;

                foreach (var VARIABLE in pair.Value)
                {
                    
                }

               // AddLightData(context, 0, null, configurationModel.FirstGeneration[0], lightFeature);
            }

        }

       
        private static IRow GetRowByIdentifier(ITable table, Guid guid, bool forceDelete, out bool rowWasCreated)
        {
            IQueryFilter queryFilter = new QueryFilterClass();
            queryFilter.WhereClause = "FeatureId = '{" +guid+ "}'";
            ICursor cursor = table.Search(queryFilter, true);
            var row=cursor.NextRow();
            if (row == null)
            {
                row=table.CreateRow();
                rowWasCreated = true;
            }
            else
            {
                if (forceDelete)
                {
                    row.Delete();
                    row = table.CreateRow();
                    rowWasCreated = true;
                }
                else
                {
                    rowWasCreated = false;
                }
            }
            return row;
        }

        private static void SaveFields(AddDataContext context)
        {
            foreach (var pair in context.DataToBeAdded)
            {
                context.StatusPrefix = "Saving " + pair.Key;

                var featureType = pair.Key;

                var realatedTableInfo = context.RelatedTableInfo.FirstOrDefault(t => t.Path == featureType.ToString());
                if (realatedTableInfo == null)
                {
                    throw new Exception("Bad file structure, can not find table for path " + featureType);
                }
                var className = realatedTableInfo.TableName;

                ITable table = context.FeatureWorkspace.OpenTable(className);
                var idFieldIndex = table.FindField("OBJECTID");
                var guidFieldIndex = table.FindField("FeatureId");

                var count = 0;
                foreach (var subpair in pair.Value)
                {
                    count++;
                    context.SetStatus(" " + count + " from " + pair.Value.Count);


                    //get row, id, set guid
                    bool rowWasCreated;
                    var row = GetRowByIdentifier(table, subpair.Key, false, out rowWasCreated);
                    if (rowWasCreated)
                    {
                        var rowId = Convert.ToInt64(row.Value[idFieldIndex]);
                        row.Value[guidFieldIndex] = "{" + subpair.Key + "}";

                        var lightFeature = subpair.Value;

                        lightFeature.RowId = rowId;
                        AddDataToRow(context, lightFeature, row, table, AimMetadata.GetAimPropInfos(lightFeature.FeatureType), featureType.ToString(), featureType.ToString(), subpair.Key);

                        row.Store();
                    }
                }
            }
        }

        private static void AddDataToRow(AddDataContext context, LightFeature lightFeature, IRow row, ITable table, IList<AimPropInfo> aimProps, 
            string tableName, string path, Guid featureId)
        {

            //write simple fields
            foreach (var simpleField in lightFeature.Fields ?? new LightField[0])
            {
                if ((simpleField.Flag & LightData.Missing) != 0)
                {
                    //we do not process missing fields
                    continue;
                }

                if ((lightFeature.ComplexFields ?? new LightComplexField[0]).Any(t => t.Name == simpleField.Name))
                {
                    continue;
                }

                var correspondingAimProp = aimProps.FirstOrDefault(t => t.Name == simpleField.Name);
                if (correspondingAimProp == null)
                {
                    throw new Exception("Bad field name " + simpleField.Name);
                }

                if (!IsSimple(correspondingAimProp) || correspondingAimProp.IsList)
                {
                    //we do not process complex values
                    continue;
                }

                var value = simpleField.Value;
                if (value is Aran.Geometries.Geometry)
                {
                    value = GeometryFormatter.MakeZAwareGeometry(ConvertToEsriGeom.FromGeometry(value as Aran.Geometries.Geometry));
                }
                var valClass = value as IEditValClass;
                if (valClass != null)
                {
                    var uom = ((dynamic)value).Uom.ToString();

                    var uomFieldName = GetValTypeNameForUom(simpleField.Name);
                    var uomFieldIndex = table.FindField(uomFieldName);
                    if (uomFieldIndex == -1)
                    {
                        throw new Exception("Bad field name");
                    }
                    row.Value[uomFieldIndex] = uom;

                    var val = valClass.Value;

                    var valFieldName = GetValTypeNameForValue(simpleField.Name);
                    var avlFieldIndex = table.FindField(valFieldName);
                    if (avlFieldIndex == -1)
                    {
                        throw new Exception("Bad field name");
                    }
                    row.Value[avlFieldIndex] = val;
                }
                else
                {
                    var fieldName = GetEsriName(simpleField.Name);
                    var fieldIndex = table.FindField(fieldName);
                    if (fieldIndex == -1)
                    {
                        throw new Exception("Bad field name");
                        //continue;
                    }
                    row.Value[fieldIndex] = value;
                }
            }

            //write complex fields
            foreach (var complexField in lightFeature.ComplexFields ?? new LightComplexField[0])
            {
                if ((complexField.Flag & LightData.Missing) == 0 && complexField.Value!=null)
                {
                    var correspondingAimProp = aimProps.FirstOrDefault(t => t.Name == complexField.Name);
                    if (correspondingAimProp == null)
                    {
                        throw new Exception("Bad structure, can not find complex filed "+complexField.Name);
                    }
                    AddComplexField(context, complexField, correspondingAimProp.PropType.Properties,
                        GetForeinKeyName(tableName),
                        lightFeature.RowId, path, featureId); 
                }
            }
        }

        private static void AddComplexField(AddDataContext context, LightComplexField complexField, IList<AimPropInfo> aimPropInfos, string parentKey, 
            long parentId, string path, Guid featureId)
        {
            path = path + "." + complexField.Name;
            var realatedTableInfo = context.RelatedTableInfo.FirstOrDefault(t => t.Path == path);
            if (realatedTableInfo == null)
            {
                throw new Exception("Bad file structure, can not find table for path " + path);
            }
            var className = realatedTableInfo.TableName;

            ITable table = context.FeatureWorkspace.OpenTable(className);
            var idFieldIndex = table.FindField("OBJECTID");
            var guidFieldIndex = table.FindField("FeatureId");

            var parentFieldIndex = table.FindField(parentKey);

            var row = table.CreateRow();
            var rowId = Convert.ToInt64(row.Value[idFieldIndex]);
            row.Value[parentFieldIndex] = parentId;

            row.Value[guidFieldIndex] = "{" + featureId + "}";

            LightFeature value=complexField.Value;
            value.RowId = rowId;

            AddDataToRow(context, value, row, table, aimPropInfos, complexField.Name, path, featureId);

            row.Store();
        }

        private static void AddLightData(AddDataContext context, long parentId, string parentKey, FeatureTreeViewItemViewModel configurationModel, LightFeature lightFeature)
        {
            //FeatureType featureType = (FeatureType)configurationModel.FeatureType;

            //var tableInfo = context.RelatedTableInfo.FirstOrDefault(t => t.Path == featureType.ToString());
            //if (tableInfo == null)
            //{
            //    throw new Exception("Bad file structure, can not find table for path " + featureType);
            //}

            //var className = tableInfo.TableName;
            //var aimPropInfoArr = AimMetadata.GetAimPropInfos((int)featureType);

            //ITable table = context.FeatureWorkspace.OpenTable(className);
            //var idFieldIndex = table.FindField("OBJECTID");
            //var guidFieldIndex = table.FindField("Identifier");

            //if (table == null) throw new Exception("Bad class name");


            //var actualLinks = (lightFeature.Links ?? new LightLink[0]).Where(link => (link.Flag & LightData.Missing) == 0).ToList();

            //foreach (FeatureTreeViewItemViewModel child in configurationModel.Children)
            //{
            //    var correspondingLinks = actualLinks.Where(t => t.FeatureType == (int)child.FeatureType &&
            //        (child.MandatoryLinks ?? new HashSet<String>()).Union(child.OptionalLinks ?? new HashSet<String>()).Contains(t.Name)).ToList();
            //    foreach (var link in correspondingLinks)
            //    {
            //        AddLightData(context, rowId, GetForeinKeyName(featureType.ToString()), child, link.Value);
            //    }
            //}



            //row.Store();
        }






    }
}
