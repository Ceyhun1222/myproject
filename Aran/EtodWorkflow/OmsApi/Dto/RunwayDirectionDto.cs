using System.Collections.Generic;

namespace OmsApi.Dto
{
    public class RunwayDirectionDto
    {
        public string Name { get; set; }

        public IList<ObstacleAreaDto> ObstacleAreas { get; set; }
    }
}