using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OmsApi.Dto
{
    public class RequestDto4Client : RequestRegistrationDto
    {
        public bool Submitted { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}