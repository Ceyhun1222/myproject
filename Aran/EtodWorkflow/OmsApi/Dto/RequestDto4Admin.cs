using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OmsApi.Dto
{
    public class RequestDto4Admin : RequestDto4Client
    {
        public bool Checked { get; set; }

        public string UserFullname { get; set; }

        public long UserId { get; set; }

        public bool Submitted2Aixm { get; set; }

        public DateTime Submitted2AixmAt { get; set; }

        public string Submitted2AixmPrivateSlotName { get; set; }

        public string Submitted2AixmPublicSlotName { get; set; }
    }

    public enum SortOrder
    {
        Asc,
        Desc
    }
}
