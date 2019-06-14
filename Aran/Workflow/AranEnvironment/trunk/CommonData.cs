using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aran.AranEnvironment
{
    public class CommonData
    {
        private Dictionary<string, object> _objectDict;

        public CommonData()
        {
            _objectDict = new Dictionary<string, object>();
        }

        public void AddObject(string key, object value)
        {
            if (_objectDict.ContainsKey(key))
                _objectDict[key] = value;
            else
                _objectDict.Add(key, value);
        }

        public object GetObject(string key)
        {
            object value;
            _objectDict.TryGetValue(key, out value);
            return value;
        }
    }
}
