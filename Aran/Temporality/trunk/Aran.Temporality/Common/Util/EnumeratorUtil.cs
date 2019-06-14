using System.Collections.Generic;

namespace Aran.Temporality.Common.Util
{
    public static class EnumeratorUtil
    {
        public static IEnumerable<T> ToIEnumerableMe<T>(this IEnumerator<T> enumerator)
        {
            while (enumerator.MoveNext())
            {
                yield return enumerator.Current;
            }
        }

        public static IEnumerable<T> ToIEnumerable<T>( IEnumerator<T> enumerator)
        {
            while (enumerator.MoveNext())
            {
                yield return enumerator.Current;
            }
        }

        public static IEnumerable<T> ToIEnumerableNotNull<T>(IEnumerator<T> enumerator)
        {
            while (enumerator.MoveNext())
            {
                if (enumerator.Current==null) continue;
                yield return enumerator.Current;
            }
        }
    }
}
