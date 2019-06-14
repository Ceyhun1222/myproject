using System;
using System.Collections.Generic;
using System.Linq;
using Aran.Geometries;
using PVT.Drawing.Symbols;
using PVT.Model;
using PVT.Model.Drawing;

namespace PVT.Graphics
{
    public abstract class DrawerBase
    {
        protected Dictionary<Type, Dictionary<Guid, HandlerBase>> Handlers;
        public Styles Style { get; set; }

        protected virtual PointStyles GetDefaultPointStyle()
        {
            return PointStyles.smsCircle;
        }

        public bool IsEnabled()
        {
            return Style.Enabled;
        }

        public virtual void Clean()
        {
            foreach (var handlerType in Handlers)
            {
                foreach (var handler in handlerType.Value)
                {
                    Delete(handler.Value.Handler);
                }
            }

            foreach (var handlerType in Handlers)
            {
                handlerType.Value.Clear();
            }
        }

        public virtual void Clean(Type type)
        {
            if (Handlers.ContainsKey(type))
            {
                foreach (var handler in Handlers[type])
                {
                    Delete(handler.Value.Handler);
                }
                Handlers[type].Clear();
            }
        }

        protected void Delete(DrawObject obj)
        {
            for (var i = 0; i < obj.Count; i++)
                Engine.Environment.Current.Graphics.Delete(obj.Get(i));
        }

        protected void Delete(List<DrawObject> objs)
        {
            foreach (var t in objs)
                Delete(t);
        }



        public void DrawPoint(SignificantPoint point, DrawObject drawObject)
        {
            try
            {
                if (!point.IsEmpty)
                    drawObject.Add(Engine.Environment.Current.Graphics.Draw(point.ProjectedLocation.ToAranPoint(), point.Name, Style.PointStyle.Geometry.Style, Style.PointStyle.Color.ToRGB(), Style.PointStyle.Size));
            }
            catch (Exception ex)
            {
                Engine.Environment.Current.Logger.Error(ex, "Error on drawing point");
            }
        }

        public void DrawFix(TerminalSegmentPoint point, DrawObject drawObject)
        {

            var references = point.Original.FacilityMakeup;
            for (var i = 0; i < references.Count; i++)
            {
                if (references[i].FixToleranceArea != null)
                {
                    var area = references[i].FixToleranceArea.Geo;
                    var prjArea = Engine.Environment.Current.Geometry.ToPrj<MultiPolygon>(area);
                    drawObject.Add(Engine.Environment.Current.Graphics.Draw(prjArea, Style.LineStyle.Geometry.Style, Style.LineStyle.Color.ToRGB(), Style.LineStyle.Width));
                }
            }
        }

        public DrawObject DrawCurve(MultiLineString curve, Guid identifier)
        {
            try
            {
                var drawObject = new DrawObject(identifier);

                var prjCurve = Engine.Environment.Current.Geometry.ToPrj<MultiLineString>(curve);
                drawObject.Add(Engine.Environment.Current.Graphics.Draw(prjCurve, Style.LineStyle.Geometry.Style,
                    Style.LineStyle.Color.ToRGB(), Style.LineStyle.Width));

                return drawObject;
            }
            catch (Exception ex)
            {
                Engine.Environment.Current.Logger.Error(ex, "Error on drawing curve");
                return null;
            }
        }

        public DrawObject DrawAssessmentArea(Guid identifier, ObstacleAssessmentArea area, int color, int size)
        {
            var drawObject = new DrawObject(identifier);
            if (area != null)
            {
                var prjArea = Engine.Environment.Current.Geometry.ToPrj<MultiPolygon>(area.Geo);
                drawObject.Add(Engine.Environment.Current.Graphics.Draw(prjArea, Style.LineStyle.Geometry.Style,
                    Style.LineStyle.Color.ToRGB(), Style.LineStyle.Width));
                if (((AreaStyles)Style).ObstacleStyle.Enabled)
                    for (var i = 0; i < area.Obstacles.Count; i++)
                    {
                        var multiPolys = new List<MultiPolygon>();
                        var multiStrings = new List<MultiLineString>();
                        var points = new List<Aran.Geometries.Point>();

                        for (var j = 0; j < area.Obstacles[i].Parts.Count; j++)
                        {
                            var part = area.Obstacles[i].Parts[j];
                            switch (part.Type)
                            {
                                case VerticalStructurePartGeometryChoice.ElevatedPoint:
                                    points.Add(part.Point.Geo);
                                    break;
                                case VerticalStructurePartGeometryChoice.ElevatedCurve:
                                    multiStrings.Add((MultiLineString)part.Geo);
                                    break;
                                case VerticalStructurePartGeometryChoice.ElevatedSurface:
                                    multiPolys.Add((MultiPolygon)part.Geo);
                                    break;
                                default:
                                    break;
                            }
                            multiPolys.Sort((x, y) => x.Area.CompareTo(y.Area));
                            multiStrings.Sort((x, y) => x.Length.CompareTo(y.Length));

                            for (var k = 0; k < multiPolys.Count; k++)
                            {
                                var prjGeo = Engine.Environment.Current.Geometry.ToPrj<MultiPolygon>(multiPolys[k]);
                                drawObject.Add(Engine.Environment.Current.Graphics.Draw(prjGeo, Style.LineStyle.Geometry.Style,
                                    color, size));
                            }

                            for (var k = 0; k < multiStrings.Count; k++)
                            {
                                var prjGeo = Engine.Environment.Current.Geometry.ToPrj<MultiLineString>(multiStrings[k]);
                                drawObject.Add(Engine.Environment.Current.Graphics.Draw(prjGeo, Style.LineStyle.Geometry.Style,
                                    color, size));
                            }

                            if (multiPolys.Count > 0 || multiStrings.Count > 0)
                            {
                                for (var k = 0; k < points.Count; k++)
                                {
                                    var prjPoint = Engine.Environment.Current.Geometry.ToPrj<Aran.Geometries.Point>(points[k]);
                                    drawObject.Add(Engine.Environment.Current.Graphics.Draw(prjPoint,
                                        PointStyle.Circle.Geometry.Style, color, size));
                                }

                                if (multiPolys.Count > 0)
                                {
                                    var point = Engine.Environment.Current.Geometry.ToPrj<MultiPolygon>(multiPolys.Last())
                                        .Centroid;
                                    drawObject.Add(Engine.Environment.Current.Graphics.Draw(point, area.Obstacles[i].Name,
                                        PointStyle.Circle.Geometry.Style, color, size));
                                }
                                else
                                {
                                    var point = Engine.Environment.Current.Geometry.ToPrj<MultiLineString>(multiStrings.Last())
                                        .Centroid;
                                    drawObject.Add(Engine.Environment.Current.Graphics.Draw(point, area.Obstacles[i].Name,
                                        PointStyle.Circle.Geometry.Style, color, size));
                                }
                            }
                            else if (points.Count > 0)
                            {
                                var prjPoint = Engine.Environment.Current.Geometry.ToPrj<Aran.Geometries.Point>(points[0]);
                                drawObject.Add(Engine.Environment.Current.Graphics.Draw(prjPoint, area.Obstacles[i].Name,
                                    PointStyle.Circle.Geometry.Style, color, size));

                                for (var k = 1; k < points.Count; k++)
                                {
                                    prjPoint = Engine.Environment.Current.Geometry.ToPrj<Aran.Geometries.Point>(points[k]);
                                    drawObject.Add(Engine.Environment.Current.Graphics.Draw(prjPoint,
                                        PointStyle.Circle.Geometry.Style, color, size));
                                }
                            }
                        }
                    }
            }

            return drawObject;
        }
    }
}