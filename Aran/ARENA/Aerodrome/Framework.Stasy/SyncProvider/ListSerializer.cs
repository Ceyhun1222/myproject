// Decompiled with JetBrains decompiler
// Type: Technewlogic.Stasy.SyncProvider.ListSerializer
// Assembly: Technewlogic.Stasy, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B911826E-2508-4686-A374-AE5BC883E603
// Assembly location: C:\Users\AliguluR\Desktop\CrudGenerator_src\Dependencies\Technewlogic.Stasy-1\Technewlogic.Stasy.dll

using Aerodrome.Features;
using ESRI.ArcGIS.Geometry;
using Framework.Stasy.Context;
using Framework.Stuff.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Serialization;

namespace Framework.Stasy.SyncProvider
{
    public class ListSerializer
    {
        public string Serialize(IEnumerable list, Type type)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All
            };

            IEnumerable<FieldInfo> fields = this.GetFields(type);
            List<TypeSerializationInfo> serializationInfoList = new List<TypeSerializationInfo>();

            Dictionary<Type, IEnumerable> resultDict = new Dictionary<Type, IEnumerable>();
            resultDict.Add(type, list);

            string json = JsonConvert.SerializeObject(resultDict, Formatting.Indented, settings);

            return json;
        }

        public IList Deserialize(string textReader, Type type)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All
            };

            IEnumerable<Type> types = ObjectExtensions.GetInheritanceHierarchy(type);

            List<PropertyInfo> properties = new List<PropertyInfo>();//this.GetFields(type);

            //foreach (var item in types)
            //{
            //    fields.AddRange(this.GetFields(item));
            //}
            properties.AddRange(type.GetProperties());



            Dictionary<Type, IEnumerable> dict = JsonConvert.DeserializeObject<Dictionary<Type, IEnumerable>>(textReader, settings);
            if (dict[type] is null)
                throw new ApplicationException("Type mismatch");

            ArrayList arrayList = new ArrayList();
            foreach (var serializationInfo in (IEnumerable)dict[type])
            {
                //
                TypeRelationInfo relInfo = new TypeRelationInfo();


                object instance = Activator.CreateInstance(type);

                relInfo.RootFeature = (AM_AbstractFeature)instance;

                foreach (PropertyInfo propInfo in properties)
                {
                    if (propInfo == null)
                        throw new ApplicationException("The field " + propInfo.Name + " was not found on type " + type + ".");

                    //yesli fieldInfo  AbstractFeature: 
                    //to nayti etot obyekt po idnumber v collection-e 
                    var generArg = propInfo.PropertyType.GetGenericArguments().FirstOrDefault();
                    if (ObjectExtensions.GetInheritanceHierarchy(propInfo.PropertyType).Contains(typeof(AM_AbstractFeature)) || generArg != null && ObjectExtensions.GetInheritanceHierarchy(generArg).Contains(typeof(AM_AbstractFeature)))
                    {
                        PropertyRelationInfo propRelInfo = new PropertyRelationInfo();
                        propRelInfo.RelatedPropertyInfo = propInfo;
                        if (!propInfo.PropertyType.IsCollection())
                        {
                            //Те property у которых нет value тоже нужно добавлять в list 
                            //чтобы показать пользователью у каких feature нет связей. 
                            //Своего рода валидация при загрузке проекта
                            propRelInfo.RelatedFeatIdList.Add(((AM_AbstractFeature)propInfo.GetValue(serializationInfo))?.featureID);
                            propRelInfo.IsCollection = false;
                        }
                        else
                        {
                            var featList = propInfo.GetValue(serializationInfo);
                            IList collection = (IList)featList;
                            foreach (var item in collection)
                            {
                                propRelInfo.RelatedFeatIdList.Add(((AM_AbstractFeature)item).featureID);
                            }
                            propRelInfo.IsCollection = true;
                        }

                        relInfo.Relations.Add(propRelInfo);
                        //continue;
                    }
                    if (propInfo.CanWrite)
                        propInfo.SetValue(instance, propInfo.GetValue(serializationInfo));
                    //}
                }
                arrayList.Add(instance);

                AerodromeDataCash.ProjectEnvironment.Context.FeatureRelations.Add(relInfo);
            }
            return (IList)arrayList;
        }

        private IEnumerable<FieldInfo> GetFields(Type t)
        {
            return ((IEnumerable<FieldInfo>)t.GetFields(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic));/*.Where<FieldInfo>((Func<FieldInfo, bool>) (it => ((IEnumerable<object>) it.GetCustomAttributes(typeof (IncludeInSerializationAttribute), true)).Count<object>() > 0))*/;
        }
        private IEnumerable<PropertyInfo> GetProperties(Type t)
        {
            return ((IEnumerable<PropertyInfo>)t.GetProperties(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic));/*.Where<FieldInfo>((Func<FieldInfo, bool>) (it => ((IEnumerable<object>) it.GetCustomAttributes(typeof (IncludeInSerializationAttribute), true)).Count<object>() > 0))*/;
        }
    }

}
