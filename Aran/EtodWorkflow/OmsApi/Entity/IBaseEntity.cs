using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OmsApi.Entity
{
    interface IBaseEntity : IBase
    {
        DateTime CreatedAt { get; set; }

        DateTime LastModifiedAt { get; set; }
    }
}
