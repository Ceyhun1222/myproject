using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OmsApi.Dto
{
    public class RequestDto4AdminList
    {
        public IList<RequestDto4Admin> Data { get; set; }

        public long Count { get; set; }
    }
}
