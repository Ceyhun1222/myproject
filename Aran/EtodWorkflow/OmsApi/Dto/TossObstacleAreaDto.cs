using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OmsApi.Dto
{
    public class TossObstacleAreaDto
    {
        public Guid Identifier { get; set; }

        public string Type { get; set; }

        public string RunwayDesignator { get; set; }
    }
}
