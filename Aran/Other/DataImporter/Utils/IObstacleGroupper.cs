using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataImporter.Utils
{
    interface IObstacleGroupper
    {
        Task<List<Obstacle>> GroupAsync(List<Obstacle> obstacles);
        Task<List<Obstacle>> GroupLinesAsync(List<Obstacle> obstacles);
        Task<List<Obstacle>> GroupPolygonesAsync(List<Obstacle> obstacles);
    }
}
