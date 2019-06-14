using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OmsApi.Dto
{
    public class LoginResult : Tokens
    {
        public long Id { get; set; }

        public string FullName { get; set; }

        public bool IsDefinedSlot { get; set; }
    }
}
