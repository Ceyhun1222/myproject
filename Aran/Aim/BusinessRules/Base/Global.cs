using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aran.Aim.BusinessRules
{
    public static class Global
    {
        public static bool IsObjectEquals(object obj1, object obj2)
        {
            if (obj1 is System.Collections.IList)
                return IsListEquals(obj1 as System.Collections.IList, obj2 as System.Collections.IList);

            var n1 = obj1 == null ? 0 : 1;
            var n2 = obj2 == null ? 0 : 1;
            var n = n1 + n2;

            if (n == 0)
                return true;

            if (n < 2)
                return false;

            return obj1.Equals(obj2);
        }

        public static bool IsObjectsEquals(params object[] objs)
        {
            for (var i = 0; i < objs.Length - 1; i += 2)
            {
                if (!IsObjectEquals(objs[i], objs[i + 1]))
                    return false;
            }
            return true;
        }

        private static bool IsListEquals(
            System.Collections.IList list1,
            System.Collections.IList list2)
        {
            var n1 = (list1 == null || list1.Count == 0) ? 0 : 1;
            var n2 = (list2 == null || list2.Count == 0) ? 0 : 1;
            var n = n1 + n2;

            if (n == 0)
                return true;

            if (n < 2)
                return false;

            if (list1.Count != list2.Count)
                return false;

            for (var i = 0; i < list1.Count; i++)
            {
                if (!IsObjectEquals(list1[i], list2[i]))
                    return false;
            }

            return true;
        }
    }
}
