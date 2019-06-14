using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aran.Temporality.Common.Entity;

namespace Aran.Temporality.Internal.Interface.Storage
{
    internal interface ILogEntryStorage : ICrudStorage<LogEntry>
    {
        IList<LogEntry> GetLogByIds(List<int> ids);

        IList<int> GetLogIds(DateTime fromDate, DateTime toDate, string storageMask, string applicationMask,
            string userMask, string addressMask, string actionMask, string parameterMask, bool? accessGranted);

        IList<object> GetLogValues(string field);
    }
}
