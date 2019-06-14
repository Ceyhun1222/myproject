using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OmsApi.Dto
{
    public class TossRunwayDto
    {
        public string Identifier { get; set; }

        public string Designator { get; set; }

        public IList<TossRunwayDirectionDto> RunwayDirections { get; set; }
    }
}
