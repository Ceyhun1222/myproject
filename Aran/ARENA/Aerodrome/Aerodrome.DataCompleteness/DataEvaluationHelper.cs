using Aerodrome.DataType;
using Aerodrome.Features;
using Framework.Attributes;
using Framework.Stasy;
using Framework.Stasy.Context;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Aerodrome.DataCompleteness
{
    public class DataEvaluationHelper
    {
        string _fileName;
        public DataEvaluationHelper(string fileName)
        {
            _fileName = fileName;
        }
        public Dictionary<Type, Dictionary<PropertyInfo, PropertyRequirementInfo>> CompletenessReport { get; set; }

        public Dictionary<Type, Dictionary<string, int>> QualityReport { get; set; }

        public void EvaluateData()
        {
            CompletenessReport = new Dictionary<Type, Dictionary<PropertyInfo, PropertyRequirementInfo>>();

            QualityReport = new Dictionary<Type, Dictionary<string, int>>();

            foreach (var featTypeRegistration in AerodromeDataCash.ProjectEnvironment.Context.RegisteredTypes)
            {
                var currentTypeProperties = featTypeRegistration.FeatureProperties.Where(t => t.Value.PropertyRequirement != PropertyRequirements.Ignore);

                Dictionary<PropertyInfo, PropertyRequirementInfo> currentTypeCompletenessReport = new Dictionary<PropertyInfo, PropertyRequirementInfo>();

                Dictionary<string, int> currentTypeQualityReport = new Dictionary<string, int>();
                currentTypeQualityReport.Add(nameof(AM_FeatureBase.hacc), 0);
                currentTypeQualityReport.Add(nameof(AM_FeatureVerticalQuality.vacc), 0);

                foreach (var prop in currentTypeProperties)
                {
                    currentTypeCompletenessReport.Add(prop.Key, new PropertyRequirementInfo { PropertyRequirement = prop.Value.PropertyRequirement });
                }
                var currentType = featTypeRegistration.Type;
                var concreteCollection = (IEnumerable<dynamic>)AerodromeDataCash.ProjectEnvironment.Context.FeatureCollections[currentType];



                foreach (var feature in concreteCollection)
                {
                    foreach (var featureProperty in currentTypeProperties)
                    {
                        var propValue = featureProperty.Key.GetValue(feature);

                        if (propValue is null)
                        {
                            currentTypeCompletenessReport[featureProperty.Key].Count++;
                            continue;
                        }

                        if (featureProperty.Key.PropertyType.Name == typeof(AM_Nullable<Type>).Name)
                        {

                            var nilValue = featureProperty.Key.PropertyType.GetProperty(nameof(AM_Nullable<Type>.NilReason)).GetValue(propValue);
                            if (nilValue != null)
                                currentTypeCompletenessReport[featureProperty.Key].Count++;

                            continue;
                        }

                    }

                    if (AerodromeDataCash.ProjectEnvironment.VaccRequirementsValues.Keys.Contains(currentType))
                    {
                        var vaccPropInfo = currentTypeProperties.FirstOrDefault(t => t.Key.Name.Equals(nameof(AM_FeatureVerticalQuality.vacc)));
                        var propValue = vaccPropInfo.Key.GetValue(feature);

                        if (propValue != null)
                        {
                            var vaccValue = (double)vaccPropInfo.Key.PropertyType.GetProperty(nameof(DataType<Enum>.Value)).GetValue(propValue);

                            if (vaccValue > AerodromeDataCash.ProjectEnvironment.VaccRequirementsValues[currentType])
                            {
                                currentTypeQualityReport[nameof(AM_FeatureVerticalQuality.vacc)]++;
                            }
                        }

                    }

                    if (AerodromeDataCash.ProjectEnvironment.HaccRequirementsValues.Keys.Contains(currentType))
                    {
                        var haccPropInfo = currentTypeProperties.FirstOrDefault(t => t.Key.Name.Equals(nameof(AM_FeatureBase.hacc)));
                        var propValue = haccPropInfo.Key.GetValue(feature);

                        if (propValue != null)
                        {
                            var haccValue = (double)haccPropInfo.Key.PropertyType.GetProperty(nameof(DataType<Enum>.Value)).GetValue(propValue);

                            if (haccValue > AerodromeDataCash.ProjectEnvironment.HaccRequirementsValues[currentType])
                            {
                                currentTypeQualityReport[nameof(AM_FeatureBase.hacc)]++;
                            }
                        }
                    }
                }

                CompletenessReport.Add(currentType, currentTypeCompletenessReport);
                QualityReport.Add(currentType, currentTypeQualityReport);
            }
           
        }

        public void CreateHtmlCompletenessReport()
        {
            
            using (var sw = File.CreateText(_fileName))
            {
                sw.WriteLine("<!DOCTYPE html>");
                sw.WriteLine("<html>");
                sw.WriteLine("<head>");
                sw.WriteLine("<style>");
                sw.WriteLine("table, th, td {");
                sw.WriteLine("    border: 1px solid black;");
                sw.WriteLine("    border-collapse: collapse;");
                sw.WriteLine("}");
                sw.WriteLine("th, td {");
                sw.WriteLine("    padding: 5px;");
                sw.WriteLine("}");
                sw.WriteLine("</style>");
                sw.WriteLine("</head>");
                sw.WriteLine("<body>");
                sw.WriteLine("");
                sw.WriteLine("<p align='center'><b><font size='4'>" + "AMDB Data Completeness Report " + "</font></b></p>");
                sw.WriteLine("<p><font size='4'>" + "Report Generated: " + DateTime.Now + "</font></p>");
                sw.WriteLine("<p><font size='4'>" + "Airport/Heliport: " + ((CompositeCollection<AM_AerodromeReferencePoint>)AerodromeDataCash.ProjectEnvironment.Context.FeatureCollections[typeof(AM_AerodromeReferencePoint)]).FirstOrDefault().name + "</font></p>");
                sw.WriteLine("<br>");


                foreach (var keyValue in CompletenessReport)
                {
                    if (keyValue.Value.Where(t => t.Value.Count > 0).Count() == 0)
                        continue;

                    //sw.WriteLine("<tr>");
                    //sw.WriteLine("<td columnspan='3'><b><font size='Normal'>" + keyValue.Key.Name.Substring(3) + "</font></b></td>");
                    //sw.WriteLine("</tr>");
                    sw.WriteLine("<p><b><font size='4'>" + keyValue.Key.Name.Substring(3) + "(" + ((IEnumerable<dynamic>)AerodromeDataCash.ProjectEnvironment.Context.FeatureCollections[keyValue.Key]).Count() + ")" + "</font></b></p>");

                    var mandatoryProps = keyValue.Value.Where(t => t.Value.PropertyRequirement == PropertyRequirements.Mandatory && t.Value.Count > 0);
                    if (mandatoryProps.Count() > 0)
                    {
                        //sw.WriteLine("<tr>");
                        //sw.WriteLine("<td columnspan='3'><b>" + PropertyRequirements.Mandatory.ToString() + ":" + "</b></td>");
                        //sw.WriteLine("</tr>");
                        sw.WriteLine("<p><b>" + PropertyRequirements.Mandatory.ToString() + ":" + "</b></p>");
                        sw.WriteLine("<table style=\"width:100%\">");


                        foreach (var requirementInfo in mandatoryProps)
                        {
                            // var row = new object[] { requirementInfo.Key.Name, "Not entered in " + "<b>" + requirementInfo.Value.Count + "</b>" + " feature(s)" };

                            sw.WriteLine("<tr>");
                            //foreach (var cell in row)
                            //    sw.WriteLine("<td>" + cell + "</td>");
                            sw.WriteLine("<td width='30%'>" + requirementInfo.Key.Name + "</td>");
                            sw.WriteLine("<td>" + "Not entered in " + "<b>" + requirementInfo.Value.Count + "</b>" + " feature(s)" + "</td>");
                            sw.WriteLine("</tr>");
                        }

                        sw.WriteLine("</table>");
                    }
                    var optionalProps = keyValue.Value.Where(t => t.Value.PropertyRequirement == PropertyRequirements.Optional && t.Value.Count > 0);
                    if (optionalProps.Count() > 0)
                    {
                        //sw.WriteLine("<tr>");
                        //sw.WriteLine("<td columnspan='3'><b>" + PropertyRequirements.Optional.ToString() + ":" + "</b></td>");
                        //sw.WriteLine("</tr>");
                        sw.WriteLine("<p><b>" + PropertyRequirements.Optional.ToString() + ":" + "</b></p>");
                        sw.WriteLine("<table style=\"width:100%\">");

                        foreach (var requirementInfo in optionalProps)
                        {
                            //var row = new object[] { requirementInfo.Key.Name, "Not entered in " + "<b>" + requirementInfo.Value.Count + "</b>" + " feature(s)" };

                            sw.WriteLine("<tr>");
                            //foreach (var cell in row)
                            //    sw.WriteLine("<td>" + cell + "</td>");
                            sw.WriteLine("<td width='30%'>" + requirementInfo.Key.Name + "</td>");
                            sw.WriteLine("<td>" + "Not entered in " + "<b>" + requirementInfo.Value.Count + "</b>" + " feature(s)" + "</td>");
                            sw.WriteLine("</tr>");
                        }

                        sw.WriteLine("</table>");
                    }
                }


                sw.WriteLine("</body>");
                sw.WriteLine("</html>");

                sw.Close();
            }
        }

        public void CreateHtmlQualityReport()
        {            
            using (var sw = File.CreateText(_fileName))
            {
                sw.WriteLine("<!DOCTYPE html>");
                sw.WriteLine("<html>");
                sw.WriteLine("<head>");
                sw.WriteLine("<style>");
                sw.WriteLine("table, th, td {");
                sw.WriteLine("    border: 1px solid black;");
                sw.WriteLine("    border-collapse: collapse;");
                sw.WriteLine("}");
                sw.WriteLine("th, td {");
                sw.WriteLine("    padding: 5px;");
                sw.WriteLine("}");
                sw.WriteLine("</style>");
                sw.WriteLine("</head>");
                sw.WriteLine("<body>");
                sw.WriteLine("");
                sw.WriteLine("<p align='center'><b><font size='4'>" + "AMDB Data Quality Report " + "</font></b></p>");
                sw.WriteLine("<p><font size='4'>" + "Report Generated: " + DateTime.Now + "</font></p>");
                sw.WriteLine("<p><font size='4'>" + "Airport/Heliport: " + ((CompositeCollection<AM_AerodromeReferencePoint>)AerodromeDataCash.ProjectEnvironment.Context.FeatureCollections[typeof(AM_AerodromeReferencePoint)]).FirstOrDefault().name + "</font></p>");
                sw.WriteLine("<br>");


                foreach (var keyValue in QualityReport)
                {
                    if (keyValue.Value.Where(t => t.Value > 0).Count() == 0)
                        continue;
                   
                    sw.WriteLine("<p><b><font size='4'>" + keyValue.Key.Name.Substring(3) + "(" + ((IEnumerable<dynamic>)AerodromeDataCash.ProjectEnvironment.Context.FeatureCollections[keyValue.Key]).Count() + ")" + "</font></b></p>");
                    sw.WriteLine("<table style=\"width:100%\">");

                    foreach(var item in keyValue.Value)
                    {
                        if(item.Value>0)
                        {
                            sw.WriteLine("<tr>");
                            sw.WriteLine("<td width='30%'>" + item.Key + "</td>");
                            sw.WriteLine("<td><b>" + item.Value + "</b>" + " feature(s) out of range" + "</td>");
                            sw.WriteLine("</tr>");
                        }
                    }
                   
                    sw.WriteLine("</table>");
                }


                sw.WriteLine("</body>");
                sw.WriteLine("</html>");

                sw.Close();
            }
        }
    }

    public class PropertyRequirementInfo
    {       
        public PropertyRequirements PropertyRequirement { get; set; }

        public int Count { get; set; }
    }
}
