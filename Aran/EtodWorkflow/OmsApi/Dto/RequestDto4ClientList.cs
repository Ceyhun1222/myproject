using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OmsApi.Dto
{
    public class RequestDto4ClientList
    {
        public IList<RequestDto4Client> Data { get; set; }

        public long Count { get; set; }
    }
}
