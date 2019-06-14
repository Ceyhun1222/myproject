using Aran.Aim.DataTypes;
using Aran.Aim.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aran.Aim.BusinessRules
{
    class AimPropertyGetter
    {
        public static IEnumerable<object> GetPropValue(AimObject aimObj, string propertyPath)
        {
            var propNameArr = propertyPath.Split('.');
            if (propNameArr.Length == 0)
                return null;

            return GetPropValue(aimObj, propNameArr, 0);
        }

        private static IEnumerable<object> GetPropValue(IAimObject aimObj, string[] propNameArr, int startIndex)
        {
            if (startIndex < 0)
            {
                yield break;
            }
            else if (startIndex >= propNameArr.Length)
            {
                if (aimObj is DBEntity)
                    yield return (aimObj as DBEntity).Id;
                yield break;
            }

            var classInfo = AimMetadata.GetClassInfoByIndex(aimObj);
            var propName = propNameArr[startIndex];
            var propTypeAixmName = (startIndex < propNameArr.Length - 1 ? propNameArr[startIndex + 1] : string.Empty);

            var propInfo = classInfo.Properties[propName];
            if (propInfo == null)
                yield break;

            IAimProperty aimPropVal = null;

            if (classInfo.SubClassType == AimSubClassType.Choice)
            {
                var choiceObj = aimObj as IEditChoiceClass;

                if (propInfo.IsFeatureReference)
                {
                    if (choiceObj.RefType == (int)propInfo.ReferenceFeature)
                        aimPropVal = choiceObj.RefValue;
                }
                else if (choiceObj.RefType == propInfo.TypeIndex)
                {
                    aimPropVal = choiceObj.RefValue;
                }
            }
            else
            {
                aimPropVal = aimObj.GetValue(propInfo.Index);
            }


            if (aimPropVal == null)
                yield break;


            if (aimPropVal.PropertyType == AimPropertyType.AranField)
            {
                yield return GetFieldValue(aimPropVal, propInfo);
                yield break;
            }
            else if (aimPropVal.PropertyType == AimPropertyType.DataType)
            {
                var rv = GetDateTypeValue(aimPropVal, propInfo, propTypeAixmName);
                if (rv != null)
                    yield return rv;
                yield break;
            }
            else if (aimPropVal.PropertyType == AimPropertyType.List)
            {
                var listValue = (System.Collections.IList)aimPropVal;

                if (propInfo.IsFeatureReference)
                {
                    foreach (AObject aObj in listValue)
                    {
                        var featureRefObj = aObj as FeatureRefObject;
                        if (featureRefObj != null)
                            yield return featureRefObj.Feature == null ? Guid.Empty : featureRefObj.Feature.Identifier;
                        else
#warning implement AbstractFeatureRefObject.
                            yield return 0;
                    }
                    yield break;
                }

                if (propTypeAixmName == string.Empty)
                {
                    foreach (AObject aObj in listValue)
                        yield return (aObj as AObject).Id;
                    yield break;
                }
                else
                {
                    if (propInfo.PropType.AixmName != propTypeAixmName)
                        yield break;

                    foreach (AObject aObj in listValue)
                    {
                        var aimPropVals = GetPropValue(aObj, propNameArr, startIndex + 2);
                        foreach (var aimPropItem in aimPropVals)
                            yield return aimPropItem;
                    }
                }
            }
            else if (aimPropVal.PropertyType == AimPropertyType.Object)
            {
                if (propTypeAixmName == string.Empty)
                {
                    yield return (aimPropVal as AObject).Id;
                    yield break;
                }

                var propDelta = 2;

                if (aimPropVal is ChoiceClass)
                {
                    if (startIndex + 1 < propNameArr.Length &&
                        propNameArr[startIndex + 1].Length > 0 &&
                        char.IsLower(propNameArr[startIndex + 1][0]))
                    {
                        propDelta = 1;
                    }
                }

                if (propDelta == 2 && propInfo.PropType.AixmName != propTypeAixmName)
                {
                    yield break;
                }

                var aimPropVals = GetPropValue(aimPropVal as AimObject, propNameArr, startIndex + propDelta);
                foreach (var aimPropItem in aimPropVals)
                    yield return aimPropItem;
            }
        }

        private static object GetFieldValue(IAimProperty aimPropVal, AimPropInfo propInfo)
        {
            if (AimMetadata.IsEnum(propInfo.TypeIndex))
                return AimMetadata.GetEnumValueAsString((int)(aimPropVal as IEditAimField).FieldValue, propInfo.TypeIndex);

            return (aimPropVal as IEditAimField).FieldValue;
        }

        private static object GetDateTypeValue(IAimProperty aimPropVal, AimPropInfo propInfo, string propTypeAixmName)
        {
            if (aimPropVal is IEditValClass editValClass)
            {
                if (propTypeAixmName == string.Empty)
                {
                    return editValClass.Value;
                }
                else
                {
                    if (string.Compare(propTypeAixmName, "uom", true) == 0)
                    {
                        var aranPropInfoArr = AimMetadata.GetAimPropInfos(editValClass as IAimObject);
                        string uomValueText = AimMetadata.GetEnumValueAsString(
                            editValClass.Uom, aranPropInfoArr[1].TypeIndex);

                        return uomValueText;
                    }
                    else if (string.Compare(propTypeAixmName, "value", true) == 0)
                    {
                        return editValClass.Value;
                    }
                }
            }
            else if (aimPropVal is FeatureRef)
            {
                return (aimPropVal as FeatureRef).Identifier;
            }

            return null;
        }
    }
}
