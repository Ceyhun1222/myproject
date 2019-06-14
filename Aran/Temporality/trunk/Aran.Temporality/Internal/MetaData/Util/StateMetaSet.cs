#region

using System;
using System.Collections.Generic;
using System.Linq;
using Aran.Temporality.Internal.Interface.Util;
using Aran.Temporality.Internal.MetaData.Offset;

#endregion

namespace Aran.Temporality.Internal.MetaData.Util
{
    internal static class ReverseExtension
    {
        public static IEnumerable<T> FastReverse<T>(this IList<T> items)
        {
            for (int i = items.Count - 1; i >= 0; i--)
            {
                yield return items[i];
            }
        }
    }

    //collection of meta data for single feature
    internal class StateMetaSet<T, TOffsetType> : IHasActual
        where T : OffsetStateMetaData<TOffsetType>
    {
        public bool IsActual { get; set; }

        private readonly List<T> _values = new List<T>();

        public T GetLastItemBefore(DateTime dateTime, bool equalDateIsOk)
        {
            T result = null;
            DateTime? clearStatesAfter = null;
            foreach (var value in _values.FastReverse())
            {
                if (value.ClearStatesAfter != null)
                {
                    //correct clearStatesAfter if needed
                    if (clearStatesAfter == null)
                    {
                        clearStatesAfter = value.ClearStatesAfter;
                    }
                    else if (value.ClearStatesAfter < clearStatesAfter)
                    {
                        clearStatesAfter = value.ClearStatesAfter;
                    }
                }
                else
                {
                    if (clearStatesAfter == null || value.TimeSlice.BeginPosition < clearStatesAfter)
                    {
                        //if actual
                        if (value.TimeSlice.BeginPosition < dateTime ||
                            (equalDateIsOk && dateTime == value.TimeSlice.BeginPosition)) //it is before
                            if (result == null || result.TimeSlice.BeginPosition < value.TimeSlice.BeginPosition)
                                //if later than current result
                            {
                                result = value;
                            }
                    }
                }
            }

            return result;
        }

        public IList<T> GetValid()
        {
            var result = new List<T>();
            DateTime? clearStatesAfter = null;
            foreach (var value in _values.FastReverse())
            {
                if (value.ClearStatesAfter != null)
                {
                    //correct clearStatesAfter if needed
                    if (clearStatesAfter == null)
                    {
                        clearStatesAfter = value.ClearStatesAfter;
                    }
                    else if (value.ClearStatesAfter < clearStatesAfter)
                    {
                        clearStatesAfter = value.ClearStatesAfter;
                    }
                }
                else
                {
                    if (clearStatesAfter == null || value.TimeSlice.BeginPosition < clearStatesAfter)
                    {
                        //if actual
                        result.Add(value);
                    }
                }
            }

            return result;
        }

        //returns true if add was performed
        public bool Add(T item)
        {
            if (item.ClearStatesAfter == null)
            {
                //we want to add normal state, not delete operation
                _values.Add(item); //just add state
                return true;
            }
            //we want to add delete operation
            if (_values.Count == 0)
            {
                //no data so nothing to delete
                return false;
            }
            if (_values.Last().ClearStatesAfter == null)
            {
                //last item was state
                _values.Add(item); //just add delete operation
                return true;
            }
            //last item was delete operation
            if (item.ClearStatesAfter < _values.Last().ClearStatesAfter)
            {
                //new ClearStatesAfter is before
                _values.Add(item); //just add delete operation
                return true;
            }
            //new delete operation does not change anything
            return false;
        }

        public T GetFirstInvalid()
        {
            DateTime? clearStatesAfter = null;
            foreach (var value in _values.FastReverse())
            {
                if (value.ClearStatesAfter != null)
                {
                    //correct clearStatesAfter if needed
                    if (clearStatesAfter == null)
                    {
                        clearStatesAfter = value.ClearStatesAfter;
                    }
                    else if (value.ClearStatesAfter < clearStatesAfter)
                    {
                        clearStatesAfter = value.ClearStatesAfter;
                    }
                }
                else
                {
                    if (clearStatesAfter != null && value.TimeSlice.BeginPosition >= clearStatesAfter)
                    {
                        return value;
                    }
                }
            }

            return null;
        }

        public void RemoveAllCSA()
        {
            _values.RemoveAll(t => t.ClearStatesAfter != null);
        }


        public T PokeAny()
        {
            T result = _values.LastOrDefault();
            if (result != null)
            {
                _values.Remove(result);
            }
            return result;
        }

        public T PokeInvalid()
        {
            T result = GetFirstInvalid();
            if (result != null)
            {
                _values.Remove(result);
            }
            else
            {
                RemoveAllCSA();
            }
            return result;
        }
    }
}