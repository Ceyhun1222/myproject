using Aran.Aim;
using Aran.Aim.DataTypes;
using Aran.Aim.Features;
using Aran.Aim.Metadata.UI;
using Aran.Aim.Utilities;
using Aran.Temporality.Common.Id;
using Aran.Temporality.CommonUtil.Context;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using QuickGraph;
using RazorEngine.Compilation.ImpromptuInterface.Dynamic;
using TOSSM.Report.Model;

namespace TOSSM.Util
{
    public class AccuracyReportUtil
    {
        public static List<string> GetAccuracyPathes(FeatureType featureType)
        {
            return GetAccuracyPathes((int)featureType);
        }

        private static List<string> GetAccuracyPathes(int typeIndex)
        {
            var pathes = new List<string>();
            GetAccuracyPathes(typeIndex, string.Empty, pathes);
            pathes = pathes.Select(x => x.Trim('.')).ToList();
            return pathes;
        }

        private static void GetAccuracyPathes(int typeIndex, string name, List<string> pathes, List<int> types = null)
        {
            var classInfo = AimMetadata.GetClassInfoByIndex(typeIndex);

            if (classInfo?.Properties == null) return;

            if (types == null)
                types = new List<int>();

            types.Add(typeIndex);

            foreach (var propInfo in classInfo.Properties)
            {
                if (propInfo.Name.Contains("Accuracy"))
                {
                    pathes.Add(name + "." + propInfo.Name);
                }
                else
                {
                    if (types.Contains(propInfo.TypeIndex))
                        continue;

                    GetAccuracyPathes(propInfo.TypeIndex, name + "." + propInfo.Name, pathes, types);
                }
            }

            types.Remove(typeIndex);
        }

        public static List<AccuracyReportModel> GetAccuracyReportForActiveSlot(FeatureType featureType)
        {
            var publicSlot = CurrentDataContext.CurrentUser.ActivePrivateSlot.PublicSlot;
            var privateSlot = CurrentDataContext.CurrentUser.ActivePrivateSlot;
            var reports = new List<AccuracyReportModel>();

            var pathes = GetAccuracyPathes(featureType);

            if (pathes.Count == 0)
                return null;

            if (featureType == FeatureType.VerticalStructure)
            {
                pathes.RemoveAll(x => x.Contains("HorizontalProjection"));
            }

            var slotStates = CurrentDataContext.CurrentService.GetActualDataByDate(new FeatureId
            {
                FeatureTypeId = (int) featureType,
                WorkPackage = privateSlot.Id
            }, false, publicSlot.EffectiveDate);

            if (slotStates == null)
                return reports;

            foreach (var slotState in slotStates)
            {
                try
                {
                    if (slotState?.Data?.Feature is Feature feature)
                    {
                        AddReport(reports, feature, pathes);

                        if (feature is VerticalStructure verticalStructure)
                        {
                            foreach (var part in verticalStructure.Part)
                            {
                                if (part?.HorizontalProjection == null)
                                    continue;

                                List<string> vsPathes;
                                if (part.HorizontalProjection.Choice ==
                                    VerticalStructurePartGeometryChoice.ElevatedPoint)
                                {
                                    vsPathes = new List<string>
                                    {
                                        "Part.HorizontalProjection.Location.HorizontalAccuracy",
                                        "Part.HorizontalProjection.Location.VerticalAccuracy"
                                    };
                                }
                                else if (part.HorizontalProjection.Choice ==
                                         VerticalStructurePartGeometryChoice.ElevatedCurve)
                                {
                                    vsPathes = new List<string>
                                    {
                                        "Part.HorizontalProjection.LinearExtent.HorizontalAccuracy",
                                        "Part.HorizontalProjection.LinearExtent.VerticalAccuracy"
                                    };
                                }
                                else
                                {
                                    vsPathes = new List<string>
                                    {
                                        "Part.HorizontalProjection.SurfaceExtent.HorizontalAccuracy",
                                        "Part.HorizontalProjection.SurfaceExtent.VerticalAccuracy"
                                    };
                                }

                                AddReport(reports, feature, vsPathes);
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    // ignored
                }
            }

            return reports;
        }

        private static void AddReport(List<AccuracyReportModel> reports, Feature feature, List<string> pathes)
        {
            var props = pathes.Select(path => new { Path = path, Infos = AimMetadataUtility.GetInnerProps((int)feature.FeatureType, path) });

            var description = UIUtilities.GetFeatureDescription(feature, out var hasDesc);

            foreach (var prop in props)
            {
                var accuracies = AimMetadataUtility.GetInnerPropertyValue(feature, prop.Infos);
                if (!accuracies.Any())
                    accuracies.Add(null);

                foreach (var accuracy in accuracies)
                {
                    if (accuracy != null)
                    {
                        var report = new AccuracyReportModel
                        {
                            FeatureType = feature.FeatureType,
                            Identifier = feature.Identifier,
                            Description = hasDesc ? description : null,
                            SequenceNumber = feature?.TimeSlice?.SequenceNumber,
                            CorrectionNumber = feature?.TimeSlice?.CorrectionNumber,
                            BeginValidTime =
                                feature?.TimeSlice?.ValidTime?.BeginPosition.ToString("dd/MM/yyyy HH:mm",
                                    CultureInfo.InvariantCulture),
                            EndValidTime =
                                feature?.TimeSlice?.ValidTime?.EndPosition?.ToString("dd/MM/yyyy HH:mm",
                                    CultureInfo.InvariantCulture),
                            AccuracyPath = prop.Path,
                        };

                        if (accuracy is ValDistance distance)
                        {
                            report.AccuracyValue = distance.Value.ToString();
                            report.AccuracyMeasurement = distance.Uom.ToString();
                        }
                        else if (accuracy is ValDistanceVertical distanceVertical)
                        {
                            report.AccuracyValue = distanceVertical.Value.ToString();
                            report.AccuracyMeasurement = distanceVertical.Uom.ToString();
                        }
                        else if (accuracy is IEditAimField field)
                        {
                            report.AccuracyValue = field.FieldValue.ToString();
                        }
                        else
                        {
                            report.AccuracyValue = accuracy.ToString();
                        }

                        reports.Add(report);
                    }
                    else
                    {
                        var report = new AccuracyReportModel
                        {
                            FeatureType = feature.FeatureType,
                            Identifier = feature.Identifier,
                            Description = hasDesc ? description : null,
                            SequenceNumber = feature?.TimeSlice?.SequenceNumber,
                            CorrectionNumber = feature?.TimeSlice?.CorrectionNumber,
                            BeginValidTime =
                                feature?.TimeSlice?.ValidTime?.BeginPosition.ToString("dd/MM/yyyy HH:mm",
                                    CultureInfo.InvariantCulture),
                            EndValidTime =
                                feature?.TimeSlice?.ValidTime?.EndPosition?.ToString("dd/MM/yyyy HH:mm",
                                    CultureInfo.InvariantCulture),
                            AccuracyPath = prop.Path,
                        };

                        reports.Add(report);
                    }
                }
            }
        }

        public static DataTable GetWorksheet(List<AccuracyReportModel> reports)
        {
            if (reports == null)
                return null;

            Type myType = typeof(AccuracyReportModel);
            IList<PropertyInfo> props = new List<PropertyInfo>(myType.GetProperties());

            DataTable dt = new DataTable();

            foreach (PropertyInfo prop in props)
                dt.Columns.Add(prop.Name);

            if (reports?.Count > 0)
            {
                foreach (var item in reports)
                {
                    var values = new List<object> { };

                    foreach (PropertyInfo prop in props)
                        values.Add(prop.GetValue(item, null));

                    dt.Rows.Add(values.ToArray());
                }
            }

            return dt;
        }

        public static List<AccuracyReportModel> GetAccuracyReportForActiveSlot()
        {
            var publicSlot = CurrentDataContext.CurrentUser.ActivePrivateSlot.PublicSlot;
            var privateSlot = CurrentDataContext.CurrentUser.ActivePrivateSlot;
            var reports = new List<AccuracyReportModel>();
            foreach (FeatureType type in Enum.GetValues(typeof(FeatureType)))
            {
                var pathes = GetAccuracyPathes(type);

                if (pathes.Count == 0)
                    continue;

                var props = pathes.Select(path => new { Path = path, Infos = AimMetadataUtility.GetInnerProps((int)type, path) });

                var slotStates = CurrentDataContext.CurrentService.GetActualDataByDate(new FeatureId
                {
                    FeatureTypeId = (int)type,
                    WorkPackage = privateSlot.Id
                }, false, publicSlot.EffectiveDate);

                foreach (var slotState in slotStates)
                {
                    if (slotState?.Data?.Feature is Feature feature)
                    {
                        foreach (var prop in props)
                        {
                            var description = UIUtilities.GetFeatureDescription(feature, out var hasDesc);
                            var report = new AccuracyReportModel
                            {
                                FeatureType = type,
                                Identifier = feature.Identifier,
                                Description = hasDesc ? description : null,
                                SequenceNumber = feature?.TimeSlice?.SequenceNumber,
                                CorrectionNumber = feature?.TimeSlice?.CorrectionNumber,
                                BeginValidTime = feature?.TimeSlice?.ValidTime?.BeginPosition.ToString("dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture),
                                EndValidTime = feature?.TimeSlice?.ValidTime?.EndPosition?.ToString("dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture),
                                AccuracyPath = prop.Path,
                            };

                            var accuracy = AimMetadataUtility.GetInnerPropertyValue(feature, prop.Infos).LastOrDefault();
                            if (accuracy != null)
                            {
                                if (accuracy is ValDistance distance)
                                {
                                    report.AccuracyValue = distance.Value.ToString();
                                    report.AccuracyMeasurement = distance.Uom.ToString();
                                }
                                else if (accuracy is ValDistanceVertical distanceVertical)
                                {
                                    report.AccuracyValue = distanceVertical.Value.ToString();
                                    report.AccuracyMeasurement = distanceVertical.Uom.ToString();
                                }
                                else if (accuracy is IEditAimField field)
                                {
                                    report.AccuracyValue = field.FieldValue.ToString();
                                }
                                else
                                {
                                    report.AccuracyValue = accuracy.ToString();
                                }
                            }

                            reports.Add(report);
                        }
                    }
                }
            }

            return reports;
        }
    }
}
