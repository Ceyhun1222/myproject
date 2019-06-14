using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aran.Aim.Features;
using Aran.Converters;
using Aran.Omega.Models;
using Aran.PANDA.Constants;

namespace Aran.Omega.Validation
{
    public class Area1Validation:IEtodSufaceValidation
    {
        private readonly List<VerticalStructure> _area1VsList;

        /// <exception cref="ArgumentNullException"><paramref name="Surfaces is empty!"/> is <see langword="null"/></exception>
        public Area1Validation(List<DrawingSurface> surfaceList)
        {
            var vsList = GlobalParams.AdhpObstacleList;

            _area1VsList = vsList.ToList<VerticalStructure>();
            if (surfaceList==null)
                throw  new ArgumentNullException("Surfaces is empty!");

            foreach (var surface in surfaceList)
            {
                if (surface.EtodSurfaceType == EtodSurfaceType.Area1 && surface.EtodSurfaceType == EtodSurfaceType.Area2D)
                    continue;
                foreach (var reportItem in surface.SurfaceBase.GetReport)
                {
                    _area1VsList.Remove(reportItem.Obstacle);
                }
            }
        }

        public string GetHtmlReport(DrawingSurface surface)
        {
            var area1 = surface;
            if (area1 == null)
                return "";

            var area1Html = "<div class='caption'>Area 1 Obstacles Validation</div>" +
                            "<div>" +
                            "<h4>Mandatory attributes</h4>" +
                            "<div>Area of coverage(Geometry)-cannot be empty or null</div>" +
                            "<div>Organisation -cannot be empty or null</div>" +
                            "<div>Vertical Extent (Height) -cannot be empty or null</div>" +
                            "<div>Vertical Accuracy <=" + Common.ConvertDistance(Area1.VerticalBufferWidth, Aran.Omega.Enums.RoundType.RealValue) + "</div>" +
                            "<div>Horizontal Accuracy <=" + Common.ConvertHeight(Area1.HorizontalBufferWidth, Aran.Omega.Enums.RoundType.RealValue) + "</div>" +
                            "</div>";
           
            area1Html += "<table border='1' cellpadding='2' width='100%' cellspacing='0' >" +
                         "<tr>" +
                         "<td>Id</td>" +
                         "<td>Name</td>" +
                         "<td>Type</td>" +
                         "<td>Geometry</td>" +
                         "<td>Organisation</td>" +
                         "<td>Vertical Extent (Height)</td>" +
                         "<td>Vertical Accuracy</td>" +
                         "<td>Horizontal Accuracy</td>" +
                         "</tr>";
            int i = 0;
            int badDataCount = 0;
            foreach (var vs in _area1VsList)
            {
                bool vsIsWrong = false;
                i++;

                int partNumber = -1;
                foreach (var vsPart in vs.Part)
                {
                    bool partIsWrong = false;
                    partNumber++;

                    string partHtml = "<tr>" +
                                      "<td>" + vs.Id + "</td>" +
                                      "<td>" + vs.Name + "</td>";
                    if (vsPart.Type == null)
                    {
                        if (vs.Type == null)
                            partHtml += "<td>-</td>";
                        else
                            partHtml += "<td>" + vs.Type + "</td>";
                    }
                    else
                        partHtml += "<td>" + vsPart.Type + "</td>";


                    var partGeom = vs.GetGeom(partNumber);
                    if (partGeom == null)
                    {
                        partHtml += "<td style='background-color:red;color:white'>Geometry cannot be null</td>";
                        partIsWrong = true;
                    }
                    else
                    {
                        if (partGeom.Type == Aran.Geometries.GeometryType.Point)
                            partHtml += "<td>Point</td>";
                        else if (partGeom.Type == Aran.Geometries.GeometryType.MultiPolygon)
                            partHtml += "<td>Polygon</td>";
                        else
                            partHtml += "<td>Polyline</td>";
                    }

                    if (vs.HostedOrganisation.Count == 0)
                    {
                        partHtml += "<td style='background-color:red;color:white'>Cannot be empty</td>";
                        partIsWrong = true;
                    }
                    else
                    {
                        partHtml += "<td>" + GlobalParams.Database.Organisation.Name + "</td>";
                    }

                    double tmpVerAccuracy = -1, tmpHorAccuracy = -1;

                    VerticalStructurePartGeometry horizontalProj = vsPart.HorizontalProjection;
                    var obstacleElev = ConverterToSI.Convert(vsPart.VerticalExtent, -1);
                    var tmpVerExtentAccuracy = ConverterToSI.Convert(vsPart.VerticalExtentAccuracy, -1);

                    CommonFunctions.GetVerticalHorizontalAccuracy(horizontalProj, ref tmpVerAccuracy, ref tmpHorAccuracy);

                    if (obstacleElev <= 0)
                    {
                        partHtml += "<td>-</td>";
                        //partIsWrong = true;
                    }
                    else if (obstacleElev < 100)
                    {
                        continue;
                    }
                    else
                    {
                        //partHtml += "<td style='background-color:red'>Cannot be empty</td>";   
                        partHtml += "<td>" + Common.ConvertHeight(obstacleElev, Aran.Omega.Enums.RoundType.ToNearest) + "</td>";
                    }

                    if (tmpVerExtentAccuracy < 0)
                    {
                        if (tmpVerAccuracy < 0)
                        {
                            partHtml += "<td style='background-color:red;color:white'>Cannot be empty</td>";
                            partIsWrong = true;
                        }
                        else if (tmpVerAccuracy > Area1.VerticalBufferWidth)
                        {
                            partHtml += "<td style='background-color:red;color:white'>" + Common.ConvertDistance(tmpVerAccuracy, Aran.Omega.Enums.RoundType.ToNearest) + " (Must be smaller than " + Common.ConvertDistance(Area1.VerticalBufferWidth, Aran.Omega.Enums.RoundType.ToNearest) + ")</td>";
                            partIsWrong = true;
                        }
                        else
                        {
                            partHtml += "<td>" + Common.ConvertDistance(tmpVerAccuracy, Aran.Omega.Enums.RoundType.ToNearest) + "</td>";
                        }

                    }
                    else if (tmpVerExtentAccuracy > Area1.VerticalBufferWidth)
                    {
                        partHtml += "<td style='background-color:red;color:white'>" + Common.ConvertDistance(tmpVerAccuracy, Aran.Omega.Enums.RoundType.ToNearest) + " (Must be smaller than " + Common.ConvertDistance(Area1.VerticalBufferWidth, Aran.Omega.Enums.RoundType.ToNearest) + "M)</td>";
                        partIsWrong = true;
                    }
                    else
                    {
                        partHtml += "<td>" + Common.ConvertDistance(tmpVerExtentAccuracy, Aran.Omega.Enums.RoundType.ToNearest) + "</td>";
                    }

                    if (tmpHorAccuracy < 0)
                    {
                        partHtml += "<td style='background-color:red;color:white'>Cannot be empty</td>";
                        partIsWrong = true;
                    }
                    else if (tmpHorAccuracy > Area1.HorizontalBufferWidth)
                    {
                        partHtml += "<td style='background-color:red;color:white'>" + Common.ConvertHeight(tmpHorAccuracy, Aran.Omega.Enums.RoundType.ToNearest) + " (Must be smaller than " + Common.ConvertHeight(Area1.HorizontalBufferWidth, Aran.Omega.Enums.RoundType.ToNearest) + ")</td>";
                        partIsWrong = true;
                    }
                    else
                    {
                        partHtml += "<td>" + Common.ConvertHeight(tmpHorAccuracy, Aran.Omega.Enums.RoundType.ToNearest) + "</td>";
                    }
                    if (partIsWrong)
                    {
                        vsIsWrong = true;
                        area1Html += partHtml + "</tr>";
                    }
                }
                if (vsIsWrong)
                    badDataCount++;
            }
            area1Html += "</table><div>Valid Obstacles Count :" + (_area1VsList.Count - badDataCount).ToString() + "</div>";
            area1Html += "<div>Invalid obstacles Count :" + badDataCount + "</div>";
            return area1Html;
        }
      
    }
}
