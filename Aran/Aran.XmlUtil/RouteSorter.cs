using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Threading;
using System.Xml;
using Aran.Aim;
using Aran.Aim.AixmMessage;
using Aran.Aim.Enums;
using Aran.Aim.Features;
using OfficeOpenXml;
using AimDbNamespaces = Aran.Aim.AixmMessage.AimDbNamespaces;

namespace Aran.XmlUtil
{
    public class RouteSorter
    {
        #region Aixm Writer
        private static XmlWriter CreateXmlWriter(string fileName, Action<XmlWriter> onWriterCreation = null)
        {
            var settings = new XmlWriterSettings { Indent = true };

            var writer = XmlWriter.Create(fileName, settings);

            if (onWriterCreation != null)
            {
                onWriterCreation(writer);
            }
            else
            {
                writer.WriteStartElement(AimDbNamespaces.AIXM51Message, "AIXMBasicMessage");
                writer.WriteAttributeString(AimDbNamespaces.XSI, "schemaLocation",
                    "http://www.aixm.aero/schema/5.1/message http://www.aixm.aero/schema/5.1/message/AIXM_BasicMessage.xsd");
                writer.WriteAttributeString(AimDbNamespaces.GML, "id", CommonXmlWriter.GenerateNewGmlId());
                writer.WriteAttributeString("xmlns", AimDbNamespaces.AIXM51.Prefix, null, AimDbNamespaces.AIXM51.Namespace);
            }
           
            //if (false) //TODO:add metadata
            //{
            //    writer.WriteStartElement(AimDbNamespaces.AIXM51, "messageMetadata");
            //    {
            //    }
            //    writer.WriteEndElement();
            //}
            return writer;
        }

        private void CloseXmlWriter(XmlWriter writer)
        {
            writer.WriteEndElement();
            writer.Flush();
            writer.Close();
        }

        private static void WriteFeature(XmlWriter writer, Feature feature)
        {
            writer.WriteStartElement(AimDbNamespaces.AIXM51Message, "hasMember");
            {
                var afl = new AixmFeatureList { feature };
                afl.WriteXml(writer,
                    true,//WriteExtension
                    false,// Write3DCoordinateIfExists, 
                    SrsNameType.CRS84);
            }
            writer.WriteEndElement();
        }

        private XmlWriter _xmlWriter;

        #endregion

        #region Segment Sorter
        private Guid? GetPointId(EnRouteSegmentPoint point)
        {
            if (point != null)
            {
                switch (point.PointChoice.Choice)
                {
                    case SignificantPointChoice.DesignatedPoint:
                        return point.PointChoice.FixDesignatedPoint.Identifier;
                    case SignificantPointChoice.Navaid:
                        return point.PointChoice.NavaidSystem.Identifier;
                    case SignificantPointChoice.TouchDownLiftOff:
                        return point.PointChoice.AimingPoint.Identifier;
                    case SignificantPointChoice.RunwayCentrelinePoint:
                        return point.PointChoice.RunwayPoint.Identifier;
                    case SignificantPointChoice.AirportHeliport:
                        return point.PointChoice.AirportReferencePoint.Identifier;
                    case SignificantPointChoice.AixmPoint:
                        //no id here
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            return null;
        }


        private List<RouteSegment> SortSegments(List<RouteSegment> relatedSegments)
        {
            if (relatedSegments == null || relatedSegments.Count < 2) return relatedSegments;

            //first is the one that not second
            var endIds = relatedSegments.Select(t => GetPointId(t.End)).ToList();

            var independent = relatedSegments.Where(t => !endIds.Contains(GetPointId(t.Start))).ToList();
            if (independent.Count == 0)
            {
                throw new Exception("Can not find first segment");
            }
            if (independent.Count > 1)
            {
                throw new Exception("Several first segments detected");
            }

            var result = new List<RouteSegment>();
            var current = independent.First();
            while (true)
            {
                result.Add(current);
                var id = GetPointId(current.End);

                var next = relatedSegments.Where(t => GetPointId(t.Start) == id).ToList();
                if (next.Count == 0)
                {
                    break;
                }

                if (next.Count > 1)
                {
                    throw new Exception("Several next segments detected");
                }

                current = next.First();
            }

            if (result.Count != relatedSegments.Count)
            {
                throw new Exception("Can not recreate segment sequence");
            }

            return result;
        }

        public bool SortRoutes(string inputFile, string outputFile,
            Action<string> onErrorAction = null, 
            Action<string> onStatusAction = null, 
            Action<XmlWriter> onWriterCreation =null,
            bool generateExcel=false)
        {
            try
            {
                Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
                Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;

                _features.Clear();
                _xmlWriter = CreateXmlWriter(outputFile, onWriterCreation);

                AixmHelper aimHelper = new AixmHelper();
                int totalCount = 0;
                int count = 0;

                aimHelper.Open(inputFile,
                                          () =>
                                          {
                                              totalCount++;
                                              if (onStatusAction != null)
                                              {
                                                  onStatusAction(totalCount + " features loaded...");
                                              }
                                          },
                                          () =>
                                          {
                                              if (onStatusAction != null)
                                              {
                                                  onStatusAction("Cleaning memory...");
                                              }
                                          },
                    (aixmFeatureList, collection) =>
                    {
                        foreach (Feature feature in aixmFeatureList)
                        {
                            ProcessFeature(feature);
                            count++;

                            if (onStatusAction != null)
                            {
                                onStatusAction("Processed " + count + " features from " + totalCount + "...");
                            }
                        }
                        collection.Clear(); //clear memory
                    });

                if (aimHelper.IsOpened)
                {
                    //sort routes
                    if (_features.ContainsKey(FeatureType.Route) && _features.ContainsKey(FeatureType.RouteSegment))
                    {
                        //both routes and segments are present
                        var routes = _features[FeatureType.Route].Cast<Route>().OrderBy(t2 => t2.Name).ToList();
                        var routeSegments = _features[FeatureType.RouteSegment].Cast<RouteSegment>().ToList();

                        var segmentIds = new SortedSet<Guid>();

                        foreach (var route in routes)
                        {
                            var routeId = route.Identifier;

                            var relatedSegments =
                                routeSegments.Where(
                                    t2 => t2.RouteFormed != null && t2.RouteFormed.Identifier == routeId).ToList();

                            try
                            {
                                if (relatedSegments.Count == 0)
                                {
                                    throw new Exception("No Segments");
                                }
                                if (relatedSegments.Any(t2 => t2.Start == null))
                                {
                                    throw new Exception("No start point");
                                }
                                if (relatedSegments.Any(t2 => t2.End == null))
                                {
                                    throw new Exception("No end point");
                                }

                                relatedSegments = SortSegments(relatedSegments);
                            }
                            catch (Exception exception)
                            {
                                if (onErrorAction != null)
                                {
                                    onErrorAction("Problem with Route " + route.Name + " [" + route.Identifier+"]: "+exception.Message);
                                }
                                //problem with route, write it as is
                            }

                            WriteFeature(_xmlWriter, route);
                            foreach (var feature in relatedSegments)
                            {
                                segmentIds.Add(feature.Identifier);
                                WriteFeature(_xmlWriter, feature);
                            }
                        }

                        _xmlWriter.WriteComment("Route Segments with no links to Route");

                        foreach (var segment in routeSegments.Where(t2 => !segmentIds.Contains(t2.Identifier)))
                        {
                            WriteFeature(_xmlWriter, segment);
                        }
                    }
                    else
                    {
                        //not both routes and segments are present, just write as is
                        if (_features.ContainsKey(FeatureType.Route))
                        {
                            foreach (var feature in _features[FeatureType.Route])
                            {
                                WriteFeature(_xmlWriter, feature);
                            }
                        }
                        if (_features.ContainsKey(FeatureType.RouteSegment))
                        {
                            foreach (var feature in _features[FeatureType.RouteSegment])
                            {
                                WriteFeature(_xmlWriter, feature);
                            }
                        }
                    }

                   


                    CloseXmlWriter(_xmlWriter);

                    var headers = new string[]
                    {
                        "Name/Designator",
                        "ICAO",
                        "Holding point",
                        "Touch-down point",
                        "Final approach point",
                        "Coordination point OLDI",
                        "FIR Entry point",
                        "FIR Exit point",
                        "FIR Entry/ Exit point",
                        "FIR boundary",
                        "Non-RVSM point"
                    };
	



                    //create new xls file
                    string file = Path.GetDirectoryName(inputFile) + "\\" + Path.GetFileNameWithoutExtension(inputFile) + ".xlsx";
                    if (File.Exists(file))
                    {
                        File.Delete(file);
                    }

                    using (var package = new ExcelPackage(new FileInfo(file)))
                    {

                        var sheet = package.Workbook.Worksheets.Add("Fase-0");
                        for (int i = 0; i < headers.Length; i++)
                        {
                            sheet.SetValue(1, i + 1, headers[i]);
                        }
                      
                        sheet.Cells[1, 1, 1, headers.Length].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        sheet.Cells[1, 1, 1, headers.Length].Style.Fill.BackgroundColor.SetColor(Color.BlanchedAlmond);
                        sheet.Cells[1, 1, 1, headers.Length].Style.Font.Bold=true;

                        int currentPoint = 2;


                        var holdingPatterns = _features.ContainsKey(FeatureType.HoldingPattern) ? _features[FeatureType.HoldingPattern].Cast<HoldingPattern>().ToList() :
                            new List<HoldingPattern>();

                        var patternIds=holdingPatterns.Where(
                            t2 =>
                                t2.HoldingPoint != null && t2.HoldingPoint.PointChoice != null &&
                                t2.HoldingPoint.PointChoice.Choice == SignificantPointChoice.DesignatedPoint)
                            .Select(t2 => t2.HoldingPoint.PointChoice.FixDesignatedPoint.Identifier).ToList();

                        var significantPoints = _features.ContainsKey(FeatureType.SignificantPointInAirspace) ?
                               _features[FeatureType.SignificantPointInAirspace].Cast<SignificantPointInAirspace>().ToList() :
                               new List<SignificantPointInAirspace>();

                        try
                        {
                            var designatedPonts = _features.ContainsKey(FeatureType.DesignatedPoint) ?
                                _features[FeatureType.DesignatedPoint].Cast<DesignatedPoint>().OrderBy(t2 => t2.Name).ToList():
                                new List<DesignatedPoint>();

                            foreach (var point in designatedPonts)
                            {
                                sheet.SetValue(currentPoint, 1, point.Name+"/"+point.Designator);
                                sheet.SetValue(currentPoint, 2, point.Type == CodeDesignatedPoint.ICAO);
                                
                                var pattern =
                                   holdingPatterns.FirstOrDefault(
                                       t2 =>
                                           t2.HoldingPoint != null && t2.HoldingPoint.PointChoice != null &&
                                            t2.HoldingPoint.PointChoice.Choice == SignificantPointChoice.DesignatedPoint&&
                                           t2.HoldingPoint.PointChoice.FixDesignatedPoint.Identifier == point.Identifier);

                                if (pattern == null)
                                {
                                    sheet.SetValue(currentPoint, 3, false);
                                }
                                else
                                {
                                    sheet.SetValue(currentPoint, 3, true);
                                }


                                var significantPoint =
                                    significantPoints.FirstOrDefault(
                                        t2 =>
                                            t2.Location != null &&
                                            t2.Location.Choice == SignificantPointChoice.DesignatedPoint &&
                                            t2.Location.FixDesignatedPoint.Identifier == point.Identifier);


                                if (significantPoint != null)
                                {
                                    sheet.SetValue(currentPoint, 7, significantPoint.Type == CodeAirspacePointRole.ENTRY);
                                    sheet.SetValue(currentPoint, 8, significantPoint.Type == CodeAirspacePointRole.EXIT);
                                    sheet.SetValue(currentPoint, 8, significantPoint.Type == CodeAirspacePointRole.ENTRY_EXIT);

                                    sheet.SetValue(currentPoint, 9, significantPoint.RelativeLocation == CodeAirspacePointPosition.BORDER);
                                }

                                currentPoint++;
                            }
                        }
                        catch (Exception)
                        {
                            
                        }
                       


                        for (int i = 0; i < headers.Length; i++)
                        {
                            sheet.Column(i+1).AutoFit();
                        }
                        package.Save();
                    }

               

                    //int i = 0;
                    //foreach (var header in headers)
                    //{
                    //    worksheet.Cells[0, i] = new Cell(header);
                    //    i++;
                    //}



                    return true;
                }

            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }

            return false;
        }


        #endregion

        #region Features

        private readonly Dictionary<FeatureType, List<Feature>> _features = new Dictionary<FeatureType, List<Feature>>();
        private readonly List<FeatureType> _acceptedTypes = new List<FeatureType>
        {
            FeatureType.Route,
            FeatureType.RouteSegment,
        };


        private readonly List<FeatureType> _importantTypes = new List<FeatureType>
        {
            FeatureType.Route,
            FeatureType.RouteSegment,
            FeatureType.HoldingPattern,
            FeatureType.SignificantPointInAirspace,
            FeatureType.DesignatedPoint,
            FeatureType.Navaid
        };

        private void ProcessFeature(Feature feature)
        {
            if (!_acceptedTypes.Contains(feature.FeatureType))
            {
                WriteFeature(_xmlWriter, feature);

                if (!_importantTypes.Contains(feature.FeatureType))
                {
                    return;
                }
            }

            List<Feature> list;
            if (!_features.TryGetValue(feature.FeatureType, out list))
            {
                list = new List<Feature>();
                _features[feature.FeatureType] = list;
            }

            list.Add(feature);
        }

        #endregion
    }
}
