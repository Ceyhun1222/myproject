using System.Collections.Generic;

namespace OmsApi.Dto
{
    public class RunwayDto
    {
        public string Name { get; set; }

        public IList<RunwayDirectionDto> RunwayDirections { get; set; }
    }
}