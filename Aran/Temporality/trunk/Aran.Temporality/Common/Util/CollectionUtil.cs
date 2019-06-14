using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Aran.Temporality.Common.Config;

namespace Aran.Temporality.Common.Util
{
    public class CollectionUtil
    {
        public static IEnumerable<IEnumerable<T>> SplitCollection<T>(IEnumerable<T> list, int parts)
        {
            var i = 0;
            var splits = from item in list
                         group item by i++ % parts into part
                         select part.AsEnumerable();
            return splits;
        }

        public static IEnumerable<IEnumerable<T>> SplitCollection<T>(IEnumerable<T> list)
        {
            return SplitCollection(list, ConfigUtil.CoreCount());
        }


        public static IEnumerable<IEnumerable<object>> SplitCollection(IEnumerable list)
        {
            return SplitCollection(list.Cast<object>(), ConfigUtil.CoreCount());
        }

        public static List<T>[] SplitList<T>(IList<T> list)
        {
            return SplitList(list, ConfigUtil.CoreCount());
        }

        public static List<T>[] SplitList<T>(IList<T> list, int parts)
        {
            List<T>[] partitions = new List<T>[parts];

            int maxSize = (int)Math.Ceiling(list.Count / (double)parts);
            int k = 0;
            for (int i = 0; i < parts; i++)
            {
                partitions[i] = new List<T>();
                for (int j = k; j < k + maxSize; j++)
                {
                    if (j >= list.Count)
                    {
                        break;
                    }
                    partitions[i].Add(list[j]);
                }
                k += maxSize;
            }

            return partitions;
        }
    }
}
