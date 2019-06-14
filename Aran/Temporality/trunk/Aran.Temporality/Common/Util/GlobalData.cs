using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aran.Temporality.Common.Util
{
    public class GlobalData<T>
    {
        public static GlobalData<T> Instance { get; } = new GlobalData<T>();

        public T this[string key]
        {
            get => key == null || !_values.ContainsKey(key) ? default(T) : _values[key];
            set => _values[key] = value;
        }

        private GlobalData() { }
        private Dictionary<string, T> _values { get; } = new Dictionary<string, T>();
    }
}
