using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Aran.Aim;
using FluentNHibernate.Conventions;

namespace Aran.Temporality.Internal.WorkFlow.Util
{
    public class ChildFinder
    {


        private static void GetChildrenByTypeR(IAimObject aimObject, Type childType, List<object> result, HashSet<object> accountedObjects)
        {
            if (aimObject == null) return;

            if (!accountedObjects.Add(aimObject)) return;

            if (aimObject.GetType() == childType)
            {
                result.Add(aimObject);
                return;
            }
            var classInfo = AimMetadata.GetClassInfoByIndex(aimObject);

            foreach (var propInfo in classInfo.Properties)
            {
                IAimProperty aimPropVal = aimObject.GetValue(propInfo.Index);
                if (aimPropVal != null)
                {
                    switch (aimPropVal.PropertyType)
                    {
                        case AimPropertyType.AranField:
                            {
                                GetChildrenByTypeR(aimPropVal as IAimObject, childType, result, accountedObjects);
                            }
                            break;
                        case AimPropertyType.DataType:
                            {
                                GetChildrenByTypeR(aimPropVal as IAimObject, childType, result, accountedObjects);
                            }
                            break;
                        case AimPropertyType.List:
                            {
                                var list = aimPropVal as IList;
                                if (list != null)
                                {
                                    foreach (var t in list)
                                    {
                                        var aimObjectItem = t as IAimObject;
                                        GetChildrenByTypeR(aimObjectItem, childType, result, accountedObjects);
                                    }
                                }

                            }
                            break;
                        case AimPropertyType.Object:
                            {
                                GetChildrenByTypeR(aimPropVal as IAimObject, childType, result, accountedObjects);
                            }
                            break;
                    }
                }
            }
        }

        public static List<object> GetChildrenByType(IAimObject feature, Type childType)
        {
            var result = new List<object>();
            GetChildrenByTypeR(feature, childType, result, new HashSet<object>());
            return result;
        }


        public static List<object> GetParentsByPropertyName(IAimObject feature, string applicableProperty)
        {
            var result = new List<object>();
            GetParentsByPropertyNameR(feature, applicableProperty, result, new HashSet<object>());
            return result;
        }

        public static List<AimPropInfo> GetApplicablePropertiesList(IAimObject aimObject, string applicableProperty)
        {
            var classInfo = AimMetadata.GetClassInfoByIndex(aimObject);

            return classInfo.Properties.Where(t => Regex.IsMatch(applicableProperty, "^\\w+$")
                    ? t.Name == applicableProperty
                    : Regex.IsMatch(t.Name, applicableProperty)).ToList();
        }

        private static bool HasApplicableProperties(AimClassInfo classInfo, string applicableProperty)
        {

            return classInfo.Properties.Any(t => Regex.IsMatch(applicableProperty, "^\\w+$")
                ? t.Name == applicableProperty
                : Regex.IsMatch(t.Name, applicableProperty));
        }

        private static void GetParentsByPropertyNameR(IAimObject aimObject, string applicableProperty, List<object> result, HashSet<object> accountedObjects)
        {
            if (aimObject == null) return;

            if (!accountedObjects.Add(aimObject)) return;

            var classInfo = AimMetadata.GetClassInfoByIndex(aimObject);

            if (HasApplicableProperties(classInfo, applicableProperty))
            {
                result.Add(aimObject);
                CheckProperties(aimObject, applicableProperty, result, accountedObjects, classInfo, false);
                return;
            }

            CheckProperties(aimObject, applicableProperty, result, accountedObjects, classInfo);
        }

        private static void CheckProperties(IAimObject aimObject, string applicableProperty, List<object> result,
            HashSet<object> accountedObjects, AimClassInfo classInfo, bool all = true)
        {
            foreach (var propInfo in classInfo.Properties)
            {
                IAimProperty aimPropVal = aimObject.GetValue(propInfo.Index);
                if (aimPropVal != null)
                {
                    switch (aimPropVal.PropertyType)
                    {
                        case AimPropertyType.AranField:
                            {
                                if (!all)
                                    break;
                                GetParentsByPropertyNameR(aimPropVal as IAimObject, applicableProperty, result, accountedObjects);
                            }
                            break;
                        case AimPropertyType.DataType:
                            {
                                GetParentsByPropertyNameR(aimPropVal as IAimObject, applicableProperty, result, accountedObjects);
                            }
                            break;
                        case AimPropertyType.List:
                            {
                                var list = aimPropVal as IList;
                                if (list != null)
                                {
                                    foreach (var t in list)
                                    {
                                        var aimObjectItem = t as IAimObject;
                                        GetParentsByPropertyNameR(aimObjectItem, applicableProperty, result, accountedObjects);
                                    }
                                }
                            }
                            break;
                        case AimPropertyType.Object:
                            {
                                GetParentsByPropertyNameR(aimPropVal as IAimObject, applicableProperty, result, accountedObjects);
                            }
                            break;
                    }
                }
            }
        }
    }
}
