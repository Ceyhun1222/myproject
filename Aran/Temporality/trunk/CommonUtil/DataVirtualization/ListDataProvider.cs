using System.Collections.Generic;
using System.Diagnostics;

namespace Aran.Temporality.CommonUtil.DataVirtualization
{
    public class ListDataProvider<T> : IItemsProvider<T>
    {
        private readonly List<T> _data;

        public ListDataProvider(List<T> data)
        {
            _data = data;
        }

        public int FetchCount()
        {
            Trace.WriteLine("FetchCount");
            return _data == null ? 0 : _data.Count;
        }

        /// <summary>
        /// Fetches a range of items.
        /// </summary>
        /// <param name="startIndex">The start index.</param>
        /// <param name="count">The number of items to fetch.</param>
        /// <returns></returns>
        public IList<T> FetchRange(int startIndex, int count)
        {
            Trace.WriteLine("FetchRange: "+startIndex+","+count);

            var list = new List<T>();

            for( var i=startIndex; i<startIndex+count; i++ )
            {
                if (i>=_data.Count) break;
                list.Add(_data[i]);
            }

            return list;
        }
    }
}
