using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aran.Temporality.Common.TossConverter.Interface
{
    public interface ITossGeometryCleaner
    {
        List<int> GetWorkPackages();
        IEnumerable<Tuple<MessageCauseType, string, string>> Clean(int workPackage);
        bool IsExist();
    }
}
