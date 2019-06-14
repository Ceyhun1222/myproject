using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OmsApi.Dto
{
    public class RequestTreeViewDto
    {
        public IList<RunwayDto> Runways { get; set; }

        public IList<ObstacleAreaDto> ObstacleAreas { get; set; }

        public string AirportName { get; set; }
    }
}
