using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ParseMDL
{
    public static class ClassExtensions
    {
        public static UmlClass GetClassById (this List<UmlClass> umlClassList, string id)
        {
            foreach (var item in umlClassList)
            {
                if (item.Id == id)
                    return item;
            }
            return null;
        }
    }
}
