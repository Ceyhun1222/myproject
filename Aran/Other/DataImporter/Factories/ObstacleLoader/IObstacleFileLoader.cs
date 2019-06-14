using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataImporter.Factories.ObstacleLoader
{
    public interface IObstacleFileLoader
    {
        List<Obstacle> Obstacles { get;}
    }
}
