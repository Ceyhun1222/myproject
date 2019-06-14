using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aran.Omega.Models
{
    class FilterLatv
    {
        private ObservableCollection<DrawingSurface> _surfaceList;

        public FilterLatv(ObservableCollection<DrawingSurface> surfaceList)
        {
            _surfaceList = surfaceList;
            IgnoredObstacleList = new Dictionary<string,IgnoredObstaclePair>();
            AnalyzeByNote();
            AnalyzeByAccuracy();
            AnalyzeByHeight();
            
        }

        public Dictionary<string,IgnoredObstaclePair> IgnoredObstacleList { get; set; }

        private void AnalyzeByAccuracy()
        {
            double area2VerticalAccuracy = Area2A.VerticalBufferWidth,
            area2HorAccuracy = Area2A.HorizontalBufferWidth;
            foreach (var surface in _surfaceList)
            {
                var report = surface.SurfaceBase.GetReport;
                var count =report.Count;

                for (int i = 0; i < count; i++)
                {
                    if (!IgnoredObstacleList.ContainsKey(report[i].Name))
                    {
                        if (report[i].HorizontalAccuracy > area2HorAccuracy ||
                            report[i].VerticalAccuracy > area2VerticalAccuracy)
                        {
                            IgnoredObstacleList.Add(report[i].Name, new IgnoredObstaclePair
                            {
                                Obst1 = report[i],
                                Reasen = "Accuracy does not match to accuracy requirements of Area2."
                            });
                            // report.RemoveAt(i);
                            //ignoredObstacleCount++;
                        }
                    }
                }
            }
        }

        private void AnalyzeByHeight()
        {
            int k = 0;
            var allSurfaceReport = new List<ObstacleReport>();
            foreach (var surface in _surfaceList)
                allSurfaceReport.AddRange(surface.SurfaceBase.GetReport);

            allSurfaceReport =
                allSurfaceReport.Where(obs => obs.Penetrate > 0).
              //  GroupBy(x => x.Name, (key, group) => group.First()).
                OrderBy(obs => obs.Id).ToList<ObstacleReport>();

            var count = allSurfaceReport.Count;

            for (int i = 0; i < count - 1; i++)
            {
                double elev = Common.DeConvertDistance(allSurfaceReport[i].Elevation);

                if (IgnoredObstacleList.ContainsKey(allSurfaceReport[i].Name)) continue;
                for (int j = i + 1; j < count; j++)
                {
                    if (IgnoredObstacleList.ContainsKey(allSurfaceReport[j].Name)) continue;

                    if (allSurfaceReport[i].Name == allSurfaceReport[j].Name)
                        continue;

                    var tmpElev = Common.DeConvertDistance(allSurfaceReport[j].Elevation);
                    var min = Math.Max(elev - allSurfaceReport[i].VerticalAccuracy,
                        tmpElev - allSurfaceReport[j].VerticalAccuracy);
                    var max = Math.Min(elev + allSurfaceReport[i].VerticalAccuracy,
                        tmpElev + allSurfaceReport[j].VerticalAccuracy);

                    if (max - min > 0)
                    {
                        if (allSurfaceReport[i].BufferPrj != null && allSurfaceReport[j].BufferPrj != null)
                            if (
                                !GlobalParams.GeomOperators.Disjoint(allSurfaceReport[i].BufferPrj,
                                    allSurfaceReport[j].BufferPrj))
                            {
                                if (IgnoredObstacleList.ContainsKey(allSurfaceReport[i].Name + allSurfaceReport[j].Name) ||
                                    IgnoredObstacleList.ContainsKey(allSurfaceReport[j].Name + allSurfaceReport[i].Name))
                                    continue;

                                IgnoredObstacleList.Add(allSurfaceReport[i].Name + allSurfaceReport[j].Name,
                                    new IgnoredObstaclePair
                                    {
                                        Obst1 = allSurfaceReport[i],
                                        Obst2 = allSurfaceReport[j],
                                        Reasen = "Their geometry intersect with each other.They might be same obstacle."
                                    });
                                k++;
                                continue;
                            }
                    }
                }
            }
        }

        private void AnalyzeByNote()
        {
            var allSurfaceReport = new List<ObstacleReport>();
            foreach (var surface in _surfaceList)
            {
                if (surface.SurfaceBase.GetReport!=null)
                    allSurfaceReport.AddRange(surface.SurfaceBase.GetReport);
            }

            allSurfaceReport =
                allSurfaceReport.Where(obs => obs.Penetrate > 0).OrderBy(obs => obs.Id).ToList<ObstacleReport>();

            foreach (var obstacleReport in allSurfaceReport)
            {
                if (IgnoredObstacleList.ContainsKey(obstacleReport.Name)) continue;
                if (obstacleReport.Obstacle?.Part.Count > 0)
                {
                    foreach (var part in obstacleReport.Obstacle.Part)
                    {
                        if (part.Annotation.Count > 0)
                        {
                            var statusAnno =
                                part.Annotation.FirstOrDefault(anno => anno.PropertyName?.ToLower() == "status");
                            if (statusAnno?.TranslatedNote[0].Note.Value == "AREA1" ||
                                statusAnno?.TranslatedNote[0].Note.Value == "AREA4")
                            {
                                IgnoredObstacleList.Add(obstacleReport.Name, new IgnoredObstaclePair
                                {
                                    Obst1 = obstacleReport,
                                    Reasen = "Obstacle marked as " + statusAnno?.TranslatedNote[0].Note.Value,
                                    Checked = false
                                });
                            }
                        }
                    }
                }
            }
        }
    }
}
