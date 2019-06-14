using Aran.Omega.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aran.Omega.Strategy.ObstacleCalculation
{
    public interface IObstacleCalculation
    {
        MtObservableCollection<ObstacleReport> CalculateReport(SurfaceBase surface);

        MtObservableCollection<ObstacleReport> CalculateReportSync(SurfaceBase surface);
    }
}
