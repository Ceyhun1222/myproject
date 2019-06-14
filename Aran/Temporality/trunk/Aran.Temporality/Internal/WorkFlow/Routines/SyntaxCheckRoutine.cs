using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Aran.Aim;
using Aran.Aim.Features;
using Aran.Aim.Objects;
using Aran.Temporality.Common.Aim.MetaData;
using Aran.Temporality.Common.Entity;
using Aran.Temporality.Common.Entity.Util;
using Aran.Temporality.Common.Validation;

namespace Aran.Temporality.Internal.WorkFlow.Routines
{
    internal class SyntaxCheckRoutine : AbstractCheckRoutine
    {
        public override int GetReportType()
        {
            return (int) ReportType.SyntaxReport;
        }

        public override bool CheckFeature(AimFeature aimFeature, List<ProblemReportUtil> problems)
        {
            CurrentOperationStatus.NextOperation();
            NextFeature();
            var result = true;
            CheckFeatureInternal(ref result, aimFeature.Feature, aimFeature.Feature, problems, new List<string>(), null);
            if (!result)
            {
                NextError();
            }
            return result;
        }

        #region private logic
        private static void CheckValueInternal(ref bool result, Feature origin, object value, AimPropInfo propInfo, List<ProblemReportUtil> problems, List<string> propInfoList)
        {
            if (propInfo?.Restriction == null || (propInfo.Restriction.Min == null && propInfo.Restriction.Max == null &&
                                                  propInfo.Restriction.Pattern == null))
            {
                return;
            }

            var stringValue = value as string;
            if (stringValue != null)
            {
                var stringValueValidationRule = new StringValueValidationRule();

                if (propInfo.Restriction.Max != null)
                {
                    stringValueValidationRule.MaximumLength = (int)propInfo.Restriction.Max;
                }

                if (propInfo.Restriction.Min != null)
                {
                    stringValueValidationRule.MinimumLength = (int)propInfo.Restriction.Min;
                }

                if (propInfo.Restriction.Pattern != null)
                {
                    stringValueValidationRule.Pattern = propInfo.Restriction.Pattern;
                }

                var validationResult = stringValueValidationRule.Validate(stringValue, CultureInfo.CurrentCulture);

                if (!validationResult.IsValid)
                {
                    result = false;
                    problems.Add(new SyntaxProblemReportUtil
                    {
                        FeatureType = origin.FeatureType,
                        Guid = origin.Identifier,
                        StringValue = stringValue,
                        Violation = validationResult.ErrorContent.ToString(),
                        PropertyPath = string.Join(".",propInfoList)
                    });
                }
                return;
            }

            if (value is decimal || value is int || value is uint || value is float || value is double)
            {
                var decimalValue = Convert.ToDouble(value);
                var numericValueValidationRule = new NumericValueValidationRule();

                if (propInfo.Restriction.Max != null)
                {
                    numericValueValidationRule.Maximum = propInfo.Restriction.Max;
                }

                if (propInfo.Restriction.Min != null)
                {
                    numericValueValidationRule.Minimum = propInfo.Restriction.Min;
                }


                var validationResult = numericValueValidationRule.Validate(decimalValue, CultureInfo.CurrentCulture);

                if (!validationResult.IsValid)
                {
                    result = false;
                    problems.Add(new SyntaxProblemReportUtil
                    {
                        FeatureType = origin.FeatureType,
                        Guid = origin.Identifier,
                        StringValue = decimalValue.ToString(CultureInfo.CurrentCulture),
                        Violation = validationResult.ErrorContent.ToString(),
                        PropertyPath = string.Join(".", propInfoList)
                    });
                }
            }
        }

        private static void CheckFeatureInternal(ref bool result, Feature origin, IAimObject aimObject, List<ProblemReportUtil> problems, List<string> propInfoList, AimPropInfo lastInfo)
        {

            if (lastInfo != null)
            {
                object myValue = aimObject;
                if (aimObject is IEditAimField)
                {
                    myValue = ((IEditAimField) myValue).FieldValue;
                    if (myValue is uint || myValue is int || myValue is Int64)
                    {
                        myValue = Convert.ToDecimal(myValue);
                    }
                }

                if (myValue is IEditChoiceClass)
                {
                    myValue = (myValue as IEditChoiceClass).RefValue;
                }

                CheckValueInternal(ref result, origin, myValue, lastInfo, problems, propInfoList);
            }


            var classInfo = AimMetadata.GetClassInfoByIndex(aimObject);
            if (classInfo==null) return;


            foreach (var propInfo in classInfo.Properties)
            {
                var aimPropVal = aimObject.GetValue(propInfo.Index);
                if (aimPropVal == null) continue;
                switch (aimPropVal.PropertyType)
                {
                    case AimPropertyType.List:
                    {
                        var list = aimPropVal as IList;
                        if (list != null)
                        {
                            for (var i = 0; i < list.Count; i++)
                            {
                                var aimObjectItem = list[i] as IAimObject;
                                var newPropInfoList = new List<string>(propInfoList) {propInfo.Name, "[" + i + "]"};
                                CheckFeatureInternal(ref result, origin, aimObjectItem, problems, newPropInfoList, propInfo);
                            }
                        }
                    }
                        break;
                    case AimPropertyType.AranField:
                    case AimPropertyType.DataType:
                    case AimPropertyType.Object:
                    {
                        var newPropInfoList = new List<string>(propInfoList) {propInfo.Name};
                        CheckFeatureInternal(ref result, origin, aimPropVal as IAimObject, problems, newPropInfoList,propInfo);
                    }
                    break;
                }
            }
        }
        #endregion
    }
}
