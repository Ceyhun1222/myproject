using Aerodrome.Enums;
using Aerodrome.Features;
using ESRI.ArcGIS.DataSourcesGDB;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using Framework.Attributes;
using Framework.Stasy;
using Framework.Stasy.Context;
using Framework.Stasy.Core;
using Framework.Stuff.Extensions;
using HelperDialog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Aerodrome.Import
{
    public class ImportHelper
    {
        public bool ImportMdbFile(string filePath)
        {
            IWorkspace2 workspace = AccessWorkspaceFromPath(filePath);

            IFeatureWorkspace featureWorkspace = (IFeatureWorkspace)workspace;

            var arpFeatureClass = GetFeatureClassByName(featureWorkspace, typeof(AM_AerodromeReferencePoint));
            if (arpFeatureClass is null)
            {
                
                return false;
            }

            foreach (var type in AerodromeDataCash.ProjectEnvironment.Context._entityContextManager.RegisteredTypes)
            {

                if (type.Name.Substring(3).Equals("Runway") || type.Name.Substring(3).Equals("RunwayDirection") || type.Name.Substring(3).Equals("Taxiway"))
                {
                    var table = GetTableByName(featureWorkspace, type);
                    if (table is null)
                        continue;

                    ISpatialFilter spatialFilterTable = new SpatialFilterClass();

                    var dbtableCursor = table.Search(spatialFilterTable, true);
                    int count = 1;
                    var tableRow = dbtableCursor.NextRow();

                    while (tableRow != null)
                    {
                        try
                        {                          
                            var amTableFeatInstance = FeatureToAmFeature(type, tableRow);
                            MessageListener.Instance.ReceiveMessage(string.Format("{0}: {1} ", type.Name.ToString().Substring(2), type.GetProperty(nameof(AM_AbstractFeature.idnumber)).GetValue(amTableFeatInstance).ToString()));
                            InsertFeatureToCollectionAndMdb(type, amTableFeatInstance);

                            tableRow = dbtableCursor.NextRow();
                            count++;
                        }
                        catch (Exception ex)
                        {
                            string message = ex.Message;
                            tableRow = dbtableCursor.NextRow();
                        }
                    }

                    continue;
                }

                var featureClass= GetFeatureClassByName(featureWorkspace, type);
                if (featureClass is null)
                    continue;

                ISpatialFilter spatialFilter = new SpatialFilterClass();

                IFeatureCursor dbFeatureCursor = featureClass.Search(spatialFilter, true);
                int featCount = 1;
                var dbFeature = dbFeatureCursor.NextFeature();

                while (dbFeature != null)
                {
                    try
                    {
                        var amTableFeatInstance = FeatureToAmFeature(type, dbFeature);

                        MessageListener.Instance.ReceiveMessage(string.Format("{0}: {1} ", type.Name.ToString().Substring(2), type.GetProperty(nameof(AM_AbstractFeature.idnumber)).GetValue(amTableFeatInstance)?.ToString()));

                        InsertFeatureToCollectionAndMdb(type, amTableFeatInstance);

                        dbFeature = dbFeatureCursor.NextFeature();
                        featCount++;

                    }
                    catch (Exception ex)
                    {
                        string message = ex.Message;
                        dbFeature = dbFeatureCursor.NextFeature();
                    }
                }
            }

            return true;
        }

        public IFeatureClass GetFeatureClassByName(IFeatureWorkspace featureWorkspace, Type type)
        {
            IFeatureClass featureClass = null;
            try
            {
                featureClass = featureWorkspace.OpenFeatureClass(type.Name.Substring(3));
                return featureClass;
            }
            catch
            {
                return null;
            }
        }

        public ITable GetTableByName(IFeatureWorkspace featureWorkspace, Type type)
        {
            ITable table = null;
            try
            {
                table = featureWorkspace.OpenTable(type.Name.Substring(3));
                return table;
            }
            catch
            {
                return null;
            }
        }

        public object FeatureToAmFeature(Type type, IRow feature)
        {
            var amFeatInstance = Activator.CreateInstance(type);

            for (int i = 0; i < feature.Fields.FieldCount; i++)
            {
                try
                {
                    var currentField = feature.Fields.Field[i];

                    var fieldValue = feature.Value[i];
                    object resultValue = null;
                    string propertyName = currentField.Name;
                    var propInfoByName = type.GetProperty(propertyName);

                    if (propInfoByName is null) continue;

                    if (propInfoByName.Name.Equals("idarpt"))
                    {
                        var relatedArpPropInfo = type.GetProperties().Where(p => p.PropertyType.Equals(typeof(AM_AerodromeReferencePoint))).FirstOrDefault();

                        var arpColl = (IEnumerable<dynamic>)AerodromeDataCash.ProjectEnvironment.Context.FeatureCollections[typeof(AM_AerodromeReferencePoint)];

                        if (relatedArpPropInfo != null && arpColl?.Count() > 0)
                        {
                            var arp = arpColl.FirstOrDefault();
                            relatedArpPropInfo.SetValue(amFeatInstance, arp);
                            continue;
                        }
                    }

                    if (feature.Value[i] == System.DBNull.Value && propInfoByName.PropertyType.Name != typeof(AM_Nullable<Type>).Name)
                    {
                        if (propInfoByName.PropertyType.Name.Equals(typeof(DateTime).Name))
                            propInfoByName.SetValue(amFeatInstance, DateTime.Now);
                        continue;
                    }
                        

                    if (propInfoByName.CanWrite)
                    {
                        if (propInfoByName.PropertyType.IsEnum)
                        {
                            int res;
                            bool isSuccess = Int32.TryParse(fieldValue.ToString(), out res);
                            if (isSuccess && res < 0)
                                continue;
                            resultValue = Enum.Parse(propInfoByName.PropertyType, fieldValue.ToString(), true);
                            propInfoByName.SetValue(amFeatInstance, resultValue);
                            continue;
                        }
                        else if (propInfoByName.PropertyType.Name == typeof(Aerodrome.DataType.DataType<Enum>).Name)
                        {
                            if (fieldValue == DBNull.Value)
                                continue;
                            if(!fieldValue.GetType().Name.Equals(typeof(double)))
                            {
                                double castedValue;
                                if (!Double.TryParse(fieldValue.ToString(), out castedValue))
                                    continue;
                                fieldValue = castedValue;
                            }
                            resultValue = Activator.CreateInstance(propInfoByName.PropertyType);
                            var props = propInfoByName.PropertyType.GetProperties();
                            props[0].SetValue(resultValue, Math.Round((double)fieldValue, 5));
                            propInfoByName.SetValue(amFeatInstance, resultValue);
                            continue;
                        }
                        else if (propInfoByName.PropertyType.Name == typeof(AM_Nullable<Type>).Name)
                        {
                            var genArgs = propInfoByName.PropertyType.GenericTypeArguments;
                            Type argType = genArgs[0];
                            resultValue = Activator.CreateInstance(propInfoByName.PropertyType);
                            var props = propInfoByName.PropertyType.GetProperties();

                            if (argType == typeof(DateTime))
                            {
                                //Здесь все варианты нужно проверить с DefaultValues и с String.Empty, DbNull


                                if (fieldValue != null && !fieldValue.Equals(DBNull.Value) && !fieldValue.ToString().Equals(string.Empty))
                                {
                                    var nilValue = AerodromeDataCash.ProjectEnvironment.DefaultValues.FirstOrDefault(d => d.ValueForDateTime.Equals((DateTime)fieldValue));
                                    if (nilValue != null)
                                    {
                                        props[0].SetValue(resultValue, nilValue.NilReasonValue);
                                        propInfoByName.SetValue(amFeatInstance, resultValue);
                                        continue;
                                    }
                                    props[1].SetValue(resultValue, fieldValue);
                                    propInfoByName.SetValue(amFeatInstance, resultValue);
                                    continue;
                                }
                                else
                                {
                                    props[0].SetValue(resultValue, NilReason.NotEntered);
                                    propInfoByName.SetValue(amFeatInstance, resultValue);
                                    continue;
                                }

                            }
                            if (argType == typeof(String))
                            {
                                if (fieldValue != null && !fieldValue.Equals(DBNull.Value) && !fieldValue.ToString().Equals(string.Empty))
                                {
                                    var nilValue = AerodromeDataCash.ProjectEnvironment.DefaultValues.FirstOrDefault(d => d.ValueForString.Equals(fieldValue.ToString()));
                                    if (nilValue != null)
                                    {
                                        props[0].SetValue(resultValue, nilValue.NilReasonValue);
                                        propInfoByName.SetValue(amFeatInstance, resultValue);
                                        continue;
                                    }
                                    props[1].SetValue(resultValue, fieldValue);
                                    propInfoByName.SetValue(amFeatInstance, resultValue);
                                    continue;
                                }
                                else
                                {
                                    props[0].SetValue(resultValue, NilReason.NotEntered);
                                    propInfoByName.SetValue(amFeatInstance, resultValue);
                                    continue;
                                }
                            }

                            if (argType == typeof(Double))
                            {
                                if (fieldValue != null && !fieldValue.Equals(DBNull.Value) && !fieldValue.ToString().Equals(string.Empty))
                                {
                                    double castedFieldValue;
                                    if (!Double.TryParse(fieldValue.ToString(), out castedFieldValue))
                                    {
                                        props[0].SetValue(resultValue, NilReason.NotApplicable);
                                        propInfoByName.SetValue(amFeatInstance, resultValue);
                                        continue;
                                    }
                                    var nilValue = AerodromeDataCash.ProjectEnvironment.DefaultValues.FirstOrDefault(d => d.ValueForDouble.Equals(castedFieldValue));
                                    if (nilValue != null)
                                    {
                                        props[0].SetValue(resultValue, nilValue.NilReasonValue);
                                        propInfoByName.SetValue(amFeatInstance, resultValue);
                                        continue;
                                    }
                                    props[1].SetValue(resultValue, fieldValue);
                                    propInfoByName.SetValue(amFeatInstance, resultValue);
                                    continue;
                                }
                                else
                                {
                                    props[0].SetValue(resultValue, NilReason.NotEntered);
                                    propInfoByName.SetValue(amFeatInstance, resultValue);
                                    continue;
                                }
                            }

                            if (argType.IsEnum)
                            {
                                if (fieldValue != null && !fieldValue.Equals(DBNull.Value) && !fieldValue.ToString().Equals(string.Empty))
                                {
                                    var nilValue = AerodromeDataCash.ProjectEnvironment.DefaultValues.FirstOrDefault(d => d.ValueForString.Equals(fieldValue.ToString()));
                                    if (nilValue != null)
                                    {
                                        props[0].SetValue(resultValue, nilValue.NilReasonValue);
                                        propInfoByName.SetValue(amFeatInstance, resultValue);
                                        continue;
                                    }
                                    object parsedValue = null;
                                    try
                                    {
                                        int res;
                                        bool isSuccess = Int32.TryParse(fieldValue.ToString(), out res);
                                        if (isSuccess && res < 0)
                                            throw new InvalidCastException();
                                        parsedValue = Enum.Parse(props[1].PropertyType, fieldValue.ToString(), true);
                                    }
                                    catch
                                    {
                                        props[0].SetValue(resultValue, NilReason.NotApplicable);
                                        propInfoByName.SetValue(amFeatInstance, resultValue);
                                        continue;
                                    }
                                    props[1].SetValue(resultValue, parsedValue);
                                    propInfoByName.SetValue(amFeatInstance, resultValue);
                                    continue;
                                }
                                else
                                {
                                    props[0].SetValue(resultValue, NilReason.NotEntered);
                                    propInfoByName.SetValue(amFeatInstance, resultValue);
                                    continue;
                                }


                            }
                        }
                        else if (propInfoByName.PropertyType == typeof(DateTime))
                        {
                            if (!fieldValue.GetType().Name.Equals(typeof(DateTime).Name))
                            {
                                DateTime result;
                                bool isSuccess = DateTime.TryParse(fieldValue.ToString(), out result);
                                if (isSuccess)
                                    propInfoByName.SetValue(amFeatInstance, result);
                                else
                                    propInfoByName.SetValue(amFeatInstance, DateTime.Now);
                                continue;
                            }
                            propInfoByName.SetValue(amFeatInstance, fieldValue);
                            continue;

                        }
                        else if (propInfoByName.PropertyType.IsPrimitive  || propInfoByName.PropertyType == typeof(String))
                        {
                           
                            propInfoByName.SetValue(amFeatInstance, fieldValue);
                            continue;
                        }

                    }
                    else
                    {
                        var customAttribute = propInfoByName.GetCustomAttribute(typeof(CrudPropertyConfigurationAttribute));
                        if (customAttribute != null)
                        {
                            var setterAttributes = ((CrudPropertyConfigurationAttribute)customAttribute).SetterPropertyNames;
                            if (setterAttributes?.Count() > 0)
                            {
                                foreach (var propName in setterAttributes)
                                {
                                    System.Reflection.PropertyInfo setterPropInfo = type.GetProperty(propName);
                                    //Если setter не лист 
                                    if (!setterPropInfo.PropertyType.IsCollection())
                                    {
                                        if (setterPropInfo.PropertyType.Name.Equals(typeof(AM_AbstractFeature).Name))
                                        {

                                            var allowableTypesAttribute = setterPropInfo.GetCustomAttribute(typeof(AllowableTypesAttribute));
                                            var allovableTypes = ((AllowableTypesAttribute)allowableTypesAttribute).AllovableTypes;
                                            foreach (Type allovType in allovableTypes)
                                            {
                                                var typeRelationalProperties = allovType.GetProperties().Where(prop => Attribute.IsDefined(prop, typeof(RelationalPropertyAttribute)));
                                                if (typeRelationalProperties.Count() > 0)
                                                {

                                                    var featureContext = (IEnumerable<dynamic>)AerodromeDataCash.ProjectEnvironment.Context.FeatureCollections[allovType];
                                                    //Здесь проверить AM_Nullable или нет
                                                    System.Reflection.PropertyInfo relationalProperty = typeRelationalProperties.FirstOrDefault();
                                                    if (relationalProperty.PropertyType.Name.Equals(typeof(AM_Nullable<string>).Name))
                                                    {
                                                        var result = featureContext.FirstOrDefault(t => relationalProperty.GetValue(t).Value != null && relationalProperty.GetValue(t).Value.Equals(fieldValue));
                                                        if (result != null)
                                                        {
                                                            setterPropInfo.SetValue(amFeatInstance, result);
                                                            break;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        var result = featureContext.FirstOrDefault(t => relationalProperty.GetValue(t) != null && relationalProperty.GetValue(t).Equals(fieldValue));

                                                        if (result != null)
                                                        {
                                                            setterPropInfo.SetValue(amFeatInstance, result);
                                                            break;
                                                        }
                                                    }
                                                }
                                            }

                                            continue;
                                        }
                                        var relationalProperties = setterPropInfo.PropertyType.GetProperties().Where(prop => Attribute.IsDefined(prop, typeof(RelationalPropertyAttribute)));
                                        if (relationalProperties.Count() > 0)
                                        {
                                            var featureContext = (IEnumerable<dynamic>)AerodromeDataCash.ProjectEnvironment.Context.FeatureCollections[setterPropInfo.PropertyType];
                                            //Здесь проверить AM_Nullable или нет
                                            System.Reflection.PropertyInfo relationalProperty = relationalProperties.FirstOrDefault();
                                            if (relationalProperty.PropertyType.Name.Equals(typeof(AM_Nullable<string>).Name))
                                            {
                                                var result = featureContext.FirstOrDefault(t => relationalProperty.GetValue(t).Value != null && relationalProperty.GetValue(t).Value.Equals(fieldValue));
                                                setterPropInfo.SetValue(amFeatInstance, result);
                                            }
                                            else
                                            {
                                                var result = featureContext.FirstOrDefault(t => relationalProperty.GetValue(t) != null && relationalProperty.GetValue(t).Equals(fieldValue));
                                                setterPropInfo.SetValue(amFeatInstance, result);
                                            }
                                        }
                                        continue;
                                    }
                                    //Если setter лист
                                    else if (setterPropInfo.PropertyType.IsCollection())
                                    {
                                        var relationalProperties = setterPropInfo.PropertyType.GetGenericArguments().FirstOrDefault().GetProperties().Where(prop => Attribute.IsDefined(prop, typeof(RelationalPropertyAttribute)));
                                        if (relationalProperties.Count() > 0)
                                        {

                                            var featureContext = (IEnumerable<dynamic>)AerodromeDataCash.ProjectEnvironment.Context.FeatureCollections[setterPropInfo.PropertyType.GetGenericArguments().FirstOrDefault()];

                                            //Здесь проверить AM_Nullable или нет
                                            System.Reflection.PropertyInfo relationalProperty = relationalProperties.FirstOrDefault();
                                            string[] relationPropertyValues = fieldValue.ToString().Split('_');

                                            List<AM_AbstractFeature> resultFeatures = new List<AM_AbstractFeature>();

                                            if (relationalProperty.PropertyType.Name.Equals(typeof(AM_Nullable<string>).Name))
                                            {
                                                foreach (string propValue in relationPropertyValues)
                                                {
                                                    var foundFeatures = featureContext.Where(p => relationalProperty.GetValue(p).Value != null && relationalProperty.GetValue(p).Value.Equals(propValue));
                                                    if (foundFeatures.Count() != 0)
                                                    {
                                                        AM_AbstractFeature feat = (AM_AbstractFeature)foundFeatures.FirstOrDefault();
                                                        resultFeatures.Add(feat);
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                foreach (string word in relationPropertyValues)
                                                {
                                                    var foundFeatures = featureContext.Where(p => relationalProperty.GetValue(p) != null && relationalProperty.GetValue(p).Equals(word));
                                                    if (foundFeatures.Count() != 0)
                                                    {
                                                        AM_AbstractFeature feat = (AM_AbstractFeature)foundFeatures.FirstOrDefault();
                                                        resultFeatures.Add(feat);
                                                    }
                                                }
                                            }

                                            if (resultFeatures.Count() > 0)
                                            {
                                                MethodInfo castMethod = typeof(Enumerable).GetMethod("Cast").MakeGenericMethod(setterPropInfo.PropertyType.GetGenericArguments().FirstOrDefault());
                                                object castedFeatures = castMethod.Invoke(null, new object[] { resultFeatures });

                                                MethodInfo toListMethod = typeof(Enumerable).GetMethod("ToList").MakeGenericMethod(setterPropInfo.PropertyType.GetGenericArguments().FirstOrDefault());
                                                var resultFeatureList = toListMethod.Invoke(null, new object[] { castedFeatures });

                                                setterPropInfo.SetValue(amFeatInstance, resultFeatureList);
                                                continue;
                                            }

                                        }
                                    }


                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    //TODO: Записывать лог
                    MessageBox.Show(ex.Message);
                }
            }
            if (feature is IFeature)
            {
                var geo = ((IFeature)feature).Shape;
                if (geo != null)
                {
                    IClone clone = (IClone)geo;
                    var clonedGeom = (IGeometry)clone.Clone();
                    if (geo is IPoint)
                    {
                        var geoPropInfo = type.GetProperty("geopnt");
                        geoPropInfo.SetValue(amFeatInstance, clonedGeom);
                    }
                    else if (geo is IPolygon)
                    {
                        var geoPropInfo = type.GetProperty("geopoly");
                        geoPropInfo.SetValue(amFeatInstance, clonedGeom);
                    }
                    else if (geo is IPolyline)
                    {
                        var geoPropInfo = type.GetProperty("geoline");
                        geoPropInfo.SetValue(amFeatInstance, clonedGeom);
                    }

                }
            }


            var idPropInfo = type.GetProperty(nameof(AM_AbstractFeature.featureID));
            idPropInfo.SetValue(amFeatInstance, Guid.NewGuid().ToString());

            return amFeatInstance;
        }

        public void InsertFeatureToCollectionAndMdb(Type type, object amTableFeatInstance)
        {
            MethodInfo method = typeof(Framework.Stasy.Context.ApplicationContext).GetMethod("PrepareEntity");
            MethodInfo generic = method.MakeGenericMethod(type);
            object[] parameters = new object[2];
            parameters[0] = amTableFeatInstance;
            parameters[1] = false;
            generic.Invoke(AerodromeDataCash.ProjectEnvironment.Context, parameters);

            System.Reflection.PropertyInfo propertyInfo = amTableFeatInstance.GetType().GetProperty("RelatedARP");

            var arp = ((CompositeCollection<AM_AerodromeReferencePoint>)AerodromeDataCash.ProjectEnvironment.Context.FeatureCollections[typeof(AM_AerodromeReferencePoint)]).FirstOrDefault();

            if (propertyInfo != null && arp != null)
                propertyInfo.SetValue(amTableFeatInstance, Convert.ChangeType(arp, propertyInfo.PropertyType), null);

            AerodromeDataCash.ProjectEnvironment.Context.FeatureCollections[type].Add(amTableFeatInstance);

            var stateChangedList = AerodromeDataCash.ProjectEnvironment.Context._entityContextManager.GetEntitiesWithState();

            var insertedList = stateChangedList.Where<KeyValuePair<SynchronizationOperation, object>>((Func<KeyValuePair<SynchronizationOperation, object>, bool>)(kvp => kvp.Key == SynchronizationOperation.Inserted)).Select<KeyValuePair<SynchronizationOperation, object>, object>((Func<KeyValuePair<SynchronizationOperation, object>, object>)(kvp => kvp.Value));

            //Add to Entity List
            AerodromeDataCash.ProjectEnvironment.Context._syncProvider.Insert(insertedList);
           
            AerodromeDataCash.ProjectEnvironment.Context._entityContextManager.ResetEntitiesState();

            AerodromeDataCash.ProjectEnvironment.GeoDbProvider.Insert(AerodromeDataCash.ProjectEnvironment.TableDictionary, (AM_AbstractFeature)amTableFeatInstance);
        }

        public IWorkspace2 AccessWorkspaceFromPath(string path)
        {
            IWorkspaceFactory2 workspaceFactory = new AccessWorkspaceFactoryClass();
            return (IWorkspace2)workspaceFactory.OpenFromFile(path, 0);
        }



        public object TableRowToAmFeature(Type type, IRow tableRow)
        {
            var amTableFeatInstance = Activator.CreateInstance(type);

            for (int i = 0; i < tableRow.Fields.FieldCount; i++)
            {
                try
                {
                    var currentField = tableRow.Fields.Field[i];

                    var fieldValue = tableRow.Value[i];
                    object resultValue = null;
                    string propertyName = currentField.Name;
                    var propInfoByName = type.GetProperty(propertyName);

                    if (propInfoByName is null) continue;

                    if (tableRow.Value[i] == System.DBNull.Value && propInfoByName.PropertyType.Name != typeof(AM_Nullable<Type>).Name)
                        continue;

                    if (propInfoByName.CanWrite)
                    {
                        if (propInfoByName.PropertyType.IsEnum)
                        {
                            //var inst = Activator.CreateInstance(propInfoByName.PropertyType);
                            resultValue = Enum.Parse(propInfoByName.PropertyType, fieldValue.ToString());
                            propInfoByName.SetValue(amTableFeatInstance, resultValue);
                            continue;
                        }
                        else if (propInfoByName.PropertyType.Name == typeof(Aerodrome.DataType.DataType<Enum>).Name)
                        {
                            resultValue = Activator.CreateInstance(propInfoByName.PropertyType);
                            var props = propInfoByName.PropertyType.GetProperties();

                            props[0].SetValue(resultValue, fieldValue);
                            propInfoByName.SetValue(amTableFeatInstance, resultValue);
                            continue;
                        }
                        else if (propInfoByName.PropertyType.Name == typeof(AM_Nullable<Type>).Name)
                        {
                            var genArgs = propInfoByName.PropertyType.GenericTypeArguments;
                            Type argType = genArgs[0];
                            resultValue = Activator.CreateInstance(propInfoByName.PropertyType);
                            var props = propInfoByName.PropertyType.GetProperties();

                            if (argType == typeof(DateTime))
                            {
                                //Здесь все варианты нужно проверить с DefaultValues и с String.Empty, DbNull


                                if (fieldValue != null && !fieldValue.Equals(DBNull.Value) && !fieldValue.ToString().Equals(string.Empty))
                                {
                                    var nilValue = AerodromeDataCash.ProjectEnvironment.DefaultValues.FirstOrDefault(d => d.ValueForDateTime.Equals((DateTime)fieldValue));
                                    if (nilValue != null)
                                    {
                                        props[0].SetValue(resultValue, nilValue.NilReasonValue);
                                        propInfoByName.SetValue(amTableFeatInstance, resultValue);
                                        continue;
                                    }
                                    props[1].SetValue(resultValue, fieldValue);
                                    propInfoByName.SetValue(amTableFeatInstance, resultValue);
                                    continue;
                                }
                                else
                                {
                                    props[0].SetValue(resultValue, NilReason.NotEntered);
                                    propInfoByName.SetValue(amTableFeatInstance, resultValue);
                                    continue;
                                }

                            }
                            if (argType == typeof(String))
                            {
                                if (fieldValue != null && !fieldValue.Equals(DBNull.Value) && !fieldValue.ToString().Equals(string.Empty))
                                {
                                    var nilValue = AerodromeDataCash.ProjectEnvironment.DefaultValues.FirstOrDefault(d => d.ValueForString.Equals(fieldValue.ToString()));
                                    if (nilValue != null)
                                    {
                                        props[0].SetValue(resultValue, nilValue.NilReasonValue);
                                        propInfoByName.SetValue(amTableFeatInstance, resultValue);
                                        continue;
                                    }
                                    props[1].SetValue(resultValue, fieldValue);
                                    propInfoByName.SetValue(amTableFeatInstance, resultValue);
                                    continue;
                                }
                                else
                                {
                                    props[0].SetValue(resultValue, NilReason.NotEntered);
                                    propInfoByName.SetValue(amTableFeatInstance, resultValue);
                                    continue;
                                }
                            }

                            if (argType == typeof(Double))
                            {
                                if (fieldValue != null && !fieldValue.Equals(DBNull.Value) && !fieldValue.ToString().Equals(string.Empty))
                                {
                                    double castedFieldValue;
                                    if (!Double.TryParse(fieldValue.ToString(), out castedFieldValue))
                                    {
                                        props[0].SetValue(resultValue, NilReason.NotApplicable);
                                        propInfoByName.SetValue(amTableFeatInstance, resultValue);
                                        continue;
                                    }
                                    var nilValue = AerodromeDataCash.ProjectEnvironment.DefaultValues.FirstOrDefault(d => d.ValueForDouble.Equals(castedFieldValue));
                                    if (nilValue != null)
                                    {
                                        props[0].SetValue(resultValue, nilValue.NilReasonValue);
                                        propInfoByName.SetValue(amTableFeatInstance, resultValue);
                                        continue;
                                    }
                                    props[1].SetValue(resultValue, fieldValue);
                                    propInfoByName.SetValue(amTableFeatInstance, resultValue);
                                    continue;
                                }
                                else
                                {
                                    props[0].SetValue(resultValue, NilReason.NotEntered);
                                    propInfoByName.SetValue(amTableFeatInstance, resultValue);
                                    continue;
                                }
                            }

                            if (argType.IsEnum)
                            {
                                if (fieldValue != null && !fieldValue.Equals(DBNull.Value) && !fieldValue.ToString().Equals(string.Empty))
                                {
                                    var nilValue = AerodromeDataCash.ProjectEnvironment.DefaultValues.FirstOrDefault(d => d.ValueForString.Equals(fieldValue.ToString()));
                                    if (nilValue != null)
                                    {
                                        props[0].SetValue(resultValue, nilValue.NilReasonValue);
                                        propInfoByName.SetValue(amTableFeatInstance, resultValue);
                                        continue;
                                    }
                                    object parsedValue = null;
                                    try
                                    {
                                        parsedValue = Enum.Parse(props[1].PropertyType, fieldValue.ToString());
                                    }
                                    catch
                                    {
                                        props[0].SetValue(resultValue, NilReason.NotApplicable);
                                        propInfoByName.SetValue(amTableFeatInstance, resultValue);
                                        continue;
                                    }
                                    props[1].SetValue(resultValue, parsedValue);
                                    propInfoByName.SetValue(amTableFeatInstance, resultValue);
                                    continue;
                                }
                                else
                                {
                                    props[0].SetValue(resultValue, NilReason.NotEntered);
                                    propInfoByName.SetValue(amTableFeatInstance, resultValue);
                                    continue;
                                }


                            }
                        }
                        else if (propInfoByName.PropertyType.IsPrimitive || propInfoByName.PropertyType == typeof(DateTime) || propInfoByName.PropertyType == typeof(String))
                        {
                            propInfoByName.SetValue(amTableFeatInstance, fieldValue);
                            continue;
                        }

                    }
                    else
                    {
                        var customAttribute = propInfoByName.GetCustomAttribute(typeof(CrudPropertyConfigurationAttribute));
                        if (customAttribute != null)
                        {
                            var setterAttributes = ((CrudPropertyConfigurationAttribute)customAttribute).SetterPropertyNames;
                            if (setterAttributes?.Count() > 0)
                            {
                                foreach (var propName in setterAttributes)
                                {
                                    System.Reflection.PropertyInfo setterPropInfo = type.GetProperty(propName);
                                    //Если setter не лист 
                                    if (!setterPropInfo.PropertyType.IsCollection())
                                    {
                                        var props = setterPropInfo.PropertyType.GetProperties().Where(prop => Attribute.IsDefined(prop, typeof(RelationalPropertyAttribute)));
                                        if (props.Count() > 0)
                                        {

                                            var concreteFeatColl = (IEnumerable<dynamic>)AerodromeDataCash.ProjectEnvironment.Context.FeatureCollections[setterPropInfo.PropertyType];

                                            System.Reflection.PropertyInfo currentPropInfo = props.FirstOrDefault();
                                            if (currentPropInfo.PropertyType.Name.Equals(typeof(AM_Nullable<string>).Name))
                                            {
                                                var result = concreteFeatColl.Where(t => currentPropInfo.GetValue(t).Value != null && currentPropInfo.GetValue(t).Value.Equals(fieldValue)).FirstOrDefault();
                                                setterPropInfo.SetValue(amTableFeatInstance, result);
                                            }
                                            else
                                            {
                                                var result = concreteFeatColl.Where(t => currentPropInfo.GetValue(t) != null && currentPropInfo.GetValue(t).Equals(fieldValue)).FirstOrDefault();
                                                setterPropInfo.SetValue(amTableFeatInstance, result);
                                            }

                                        }
                                    }
                                    //Если setter лист
                                    else if (setterPropInfo.PropertyType.IsCollection())
                                    {
                                        var props = setterPropInfo.PropertyType.GetProperties().Where(prop => Attribute.IsDefined(prop, typeof(RelationalPropertyAttribute)));
                                        if (props.Count() > 0)
                                        {

                                            var concreteFeatColl = (IEnumerable<dynamic>)AerodromeDataCash.ProjectEnvironment.Context.FeatureCollections[setterPropInfo.PropertyType];
                                            //Здесь проверить AM_Nullable или нет
                                            System.Reflection.PropertyInfo currentPropInfo = props.FirstOrDefault();
                                            string[] words = fieldValue.ToString().Split('_');
                                            if (currentPropInfo.PropertyType.Name.Equals(typeof(AM_Nullable<string>).Name))
                                            {
                                                var result = concreteFeatColl.FirstOrDefault(t => currentPropInfo.GetValue(t).Value != null && currentPropInfo.GetValue(t).Value.Equals(fieldValue));
                                                setterPropInfo.SetValue(amTableFeatInstance, result);
                                            }
                                            else
                                            {
                                                var result = concreteFeatColl.FirstOrDefault(t => currentPropInfo.GetValue(t) != null && currentPropInfo.GetValue(t).Equals(fieldValue));
                                                setterPropInfo.SetValue(amTableFeatInstance, result);
                                            }

                                        }
                                    }
                                    else if (setterPropInfo.PropertyType.Name.Equals(typeof(AM_AbstractFeature).Name))
                                    {

                                    }

                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }

            var idPropInfo = type.GetProperty(nameof(AM_AbstractFeature.featureID));
            idPropInfo.SetValue(amTableFeatInstance, Guid.NewGuid().ToString());

            return amTableFeatInstance;
        }

        public void GetTableByType(IFeatureWorkspace featureWorkspace, Type type)
        {

            //ISpatialFilter spatialFilterTable = new SpatialFilterClass();

            //var dbtableCursor = table.Search(spatialFilterTable, true);
            //int count = 1;
            //var tableRow = dbtableCursor.NextRow();

            //while (tableRow != null)
            //{
            //    try
            //    {
            //        //MessageListener.Instance.ReceiveMessage(string.Format("{0}: {1} ", type.Name.ToString().Substring(2), count));
            //        var amTableFeatInstance = FeatureToAmFeature(type, tableRow);
            //        MessageListener.Instance.ReceiveMessage(string.Format("{0}: {1} ", type.Name.ToString().Substring(2), type.GetProperty(nameof(AM_AbstractFeature.idnumber)).GetValue(amTableFeatInstance).ToString()));
            //        InsertFeatureToCollectionAndMdb(type, amTableFeatInstance);

            //        tableRow = dbtableCursor.NextRow();
            //        count++;

            //    }
            //    catch (Exception ex)
            //    {
            //        string message = ex.Message;
            //        tableRow = dbtableCursor.NextRow();
            //    }
            //}
        }

        public void GetFeatureClassByType(IFeatureClass featureClass, Type type)
        {


            ISpatialFilter spatialFilter = new SpatialFilterClass();

            IFeatureCursor dbFeatureCursor = featureClass.Search(spatialFilter, true);
            int count = 1;
            var dbFeature = dbFeatureCursor.NextFeature();

            while (dbFeature != null)
            {
                try
                {

                    var amTableFeatInstance = FeatureToAmFeature(type, dbFeature);

                    MessageListener.Instance.ReceiveMessage(string.Format("{0}: {1} ", type.Name.ToString().Substring(2), type.GetProperty(nameof(AM_AbstractFeature.idnumber)).GetValue(amTableFeatInstance)?.ToString()));

                    InsertFeatureToCollectionAndMdb(type, amTableFeatInstance);

                    dbFeature = dbFeatureCursor.NextFeature();
                    count++;

                }
                catch (Exception ex)
                {
                    string message = ex.Message;
                    dbFeature = dbFeatureCursor.NextFeature();
                }
            }
        }


    }
}
