using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aran.Aim.Features;
using Aran.Omega.Models;
using Aran.PANDA.Constants;

namespace Aran.Omega.Validation
{
    class Area4Validation : IEtodSufaceValidation
    {
        private readonly Dictionary<Guid, VerticalStructure> _vsList;

        /// <exception cref="ArgumentNullException"><paramref name="Surfaces is empty!"/> is <see langword="null"/></exception>
        public Area4Validation(List<DrawingSurface> surfaceList)
        {
            if (surfaceList == null)
                throw new ArgumentNullException("Surfaces is empty!");

            _vsList = new Dictionary<Guid, VerticalStructure>();
            foreach (var surface in surfaceList)
            {
                if (surface.SurfaceBase.EtodSurfaceType != EtodSurfaceType.Area4)
                    continue;
                foreach (var reportItem in surface.SurfaceBase.GetReport)
                {
                    if (_vsList.ContainsKey(reportItem.Obstacle.Identifier))
                        continue;
                    _vsList.Add(reportItem.Obstacle.Identifier, reportItem.Obstacle);
                }
            }
        }

        public string GetHtmlReport(DrawingSurface surface)
        {

            var area4 = surface;

            if (area4 == null)
                return "";

            var resultHtml = "<div class='caption'>Area 4 Obstacles Validation</div>" +
                             "<div>" +
                             "<h4>Mandatory attributes</h4>" +
                             "<div>Area of coverage(Geometry)-cannot b empty or null</div>" +
                             "<div>Elevation -cannot be empty or null</div>" +
                             "<div>Vertical Accuracy <=" +
                             Common.ConvertDistance(Area4.VerticalBufferWidth, Aran.Omega.Enums.RoundType.ToNearest) +
                             "</div>" +
                             "<div>Horizontal Accuracy <=" +
                             Common.ConvertHeight(Area4.HorizontalBufferWidth, Aran.Omega.Enums.RoundType.ToNearest) +
                             "</div>" +
                             "</div>";


            resultHtml += "<table border='1' cellpadding='2' width='100%' cellspacing='0' >" +
                          "<tr>" +
                          "<td>Id</td>" +
                          "<td>Name</td>" +
                          "<td>Type</td>" +
                          "<td>Geometry</td>" +
                          "<td>Vertical Extent (Height)</td>" +
                          "<td>Vertical Accuracy</td>" +
                          "<td>Horizontal Accuracy</td>" +
                          "</tr>";
            int i = 0;

            int badDataCount = 0;
            foreach (var vsDict in _vsList)
            {
                bool vsIsWrong = false;
                i++;
                var vs = vsDict.Value;

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

                    double tmpVerAccuracy = -1, tmpHorAccuracy = -1;

                    VerticalStructurePartGeometry horizontalProj = vsPart.HorizontalProjection;
                    CommonFunctions.GetVerticalHorizontalAccuracy(horizontalProj, ref tmpVerAccuracy,
                        ref tmpHorAccuracy);
                    var obstacleElev = vs.GetElevation(partNumber);

                    if (obstacleElev <= 0)
                    {
                        partHtml += "<td style='background-color:red;color:white'>Cannot be empty</td>";
                        partIsWrong = true;
                    }
                    else
                    {
                        //partHtml += "<td style='background-color:red'>Cannot be empty</td>";   
                        partHtml += "<td>" + Common.ConvertHeight(obstacleElev, Aran.Omega.Enums.RoundType.ToNearest) +
                                    "</td>";
                    }

                    if (tmpVerAccuracy < 0)
                    {
                        partHtml += "<td style='background-color:red;color:white'>Cannot be empty</td>";
                        partIsWrong = true;
                    }
                    else if (tmpVerAccuracy > Area4.VerticalBufferWidth)
                    {
                        partHtml += "<td style='background-color:red;color:white'>" +
                                    Common.ConvertDistance(tmpVerAccuracy, Aran.Omega.Enums.RoundType.ToNearest) +
                                    " (Must be smaller than " + Area3.VerticalBufferWidth + "M)</td>";
                        partIsWrong = true;
                    }
                    else
                    {
                        partHtml +=
                            "<td>" + Common.ConvertDistance(tmpVerAccuracy, Aran.Omega.Enums.RoundType.ToNearest) +
                            "</td>";
                    }

                    if (tmpHorAccuracy < 0)
                    {
                        partHtml += "<td style='background-color:red;color:white'>Cannot be empty</td>";
                        partIsWrong = true;
                    }
                    else if (tmpHorAccuracy > Area4.HorizontalBufferWidth)
                    {
                        partHtml += "<td style='background-color:red;color:white'>" +
                                    Common.ConvertHeight(tmpHorAccuracy, Aran.Omega.Enums.RoundType.ToNearest) +
                                    " (Must be smaller than " + Common.ConvertHeight(Area4.HorizontalBufferWidth,
                                        Aran.Omega.Enums.RoundType.ToNearest) + ")</td>";
                        partIsWrong = true;
                    }
                    else
                    {
                        partHtml +=
                            "<td>" + Common.ConvertHeight(tmpHorAccuracy, Aran.Omega.Enums.RoundType.ToNearest) +
                            "</td>";
                    }
                    if (partIsWrong)
                    {
                        vsIsWrong = true;
                        resultHtml += partHtml + "</tr>";
                    }
                }
                if (vsIsWrong)
                    badDataCount++;
            }
            resultHtml += "</table><div>Valid Obstacles Count :" + (_vsList.Count - badDataCount).ToString() + "</div>";
            resultHtml += "<div>Invalid obstacles Count :" + badDataCount + "</div>";
            return resultHtml;
        }
    }
}
