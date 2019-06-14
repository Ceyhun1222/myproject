using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aran.Temporality.CommonUtil.Util
{
    public interface IChangedHandler<in T> where T:class
    {
        void OnChanged(T source);
    }
}
