#region

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Aran.Temporality.Common.Abstract.Event;
using Aran.Temporality.Common.Enum;
using Aran.Temporality.Common.Id;
using Aran.Temporality.Internal.Interface.Util;

#endregion

namespace Aran.Temporality.Internal.MetaData.Util
{
    //collection of meta data for single feature in single workpackage
    internal class EventMetaSet<T> : IHasActual, IEnumerable<T> where T : AbstractEventMetaData
    {
        #region Lifetime

        public DateTime? LifeTimeBegin;
        public DateTime? LifeTimeEnd;
        public bool LifeTimeBeginSet;
        public bool LifeTimeEndSet;


        #endregion

        public bool IsActual { get; set; }

        private readonly IComparer<KeyValuePair<DateTime, T>> _comparer = new KeyComparer<DateTime, T>();

        public readonly List<KeyValuePair<DateTime, T>> Values = new List<KeyValuePair<DateTime, T>>();


        private void UpdateLifeTimeAdd(T meta)
        {
            if (!IsActual) return;
            if (meta == null) return;

            if (meta.LifeTimeBeginSet)
            {
                LifeTimeBegin = meta.LifeTimeBegin;
                LifeTimeBeginSet = true;
            }

            if (meta.LifeTimeEndSet)
            {
                LifeTimeEnd = meta.LifeTimeEnd;
                LifeTimeEndSet = true;
            }
        }

        private void UpdateLifeTimeRemove(T meta)
        {
            if (!IsActual) return;
            if (meta == null) return;

            if (meta.LifeTimeBeginSet)
            {
                LifeTimeBeginSet = false;
            }

            if (meta.LifeTimeEndSet)
            {
                LifeTimeEndSet = false;
            }
        }

        public void Add(T meta)
        {
            var pair = new KeyValuePair<DateTime, T>(meta.TimeSlice.BeginPosition, meta);
            int i = Values.BinarySearch(pair, _comparer);
            if (i < 0)
            {
                //keep it sorted
                Values.Insert(~i, pair);
            }
            else
            {
                Values.Insert(i, pair);
            }

            UpdateLifeTimeAdd(meta);
        }

        public int GetLastSequenceNumber(bool actualOnly = false)
        {
            if (Values.Count == 0) return 0;

            if (actualOnly && !IsActual)
            {
                return 0;
            }

            return Values.Max(t => t.Value.Version.SequenceNumber);
        }

        public int GetLastSequenceNumber(Interpretation interpretation, bool actualOnly = false)
        {
            if (Values.Count(t => t.Value.Interpretation == interpretation) == 0) return 0;

            if (actualOnly && !IsActual)
            {
                return 0;
            }

            return Values.Where(t => t.Value.Interpretation == interpretation).Max(t => t.Value.Version.SequenceNumber);
        }

        public bool ContainsVersion(TimeSliceVersion version)
        {
            if (version == null) return false;
            return ContainsVersion(version.SequenceNumber, version.CorrectionNumber);
        }
        public bool ContainsVersion(TimeSliceVersion version, Interpretation interpretation)
        {
            if (version == null) return false;
            return ContainsVersion(version.SequenceNumber, version.CorrectionNumber, interpretation);
        }

        public bool ContainsVersion(int sequenceNumber, int correctionNumber)
        {
            return Values.Any(t => t.Value.Version.SequenceNumber == sequenceNumber
                                      && t.Value.Version.CorrectionNumber == correctionNumber);
        }

        public bool ContainsVersion(int sequenceNumber, int correctionNumber, Interpretation interpretation)
        {
            return Values.Any(t => t.Value.Version.SequenceNumber == sequenceNumber
                                   && t.Value.Version.CorrectionNumber == correctionNumber && t.Value.Interpretation == interpretation);
        }

        public int GetLastCorrectionNumber(int sequenceNumber)
        {
            return
                Values.Where(t => t.Value.Version.SequenceNumber == sequenceNumber).Select(
                    t => t.Value.Version.CorrectionNumber).Max();
        }

        public ICollection<T> GetViewBetween(DateTime lowerValue, DateTime? upperValue, bool andOneMore = false)
        {
            //            if (andOneMore == false)
            //                return Values.Where(t =>
            //                                 lowerValue < t.Value.TimeSlice.BeginPosition
            //                                 && (upperValue == null || t.Value.TimeSlice.BeginPosition < upperValue)
            //                                 //interval has start date
            //                                 ||
            //                                 (t.Value.TimeSlice.BeginPosition < lowerValue &&
            //                                  t.Value.TimeSlice.EndPosition != null &&
            //                                  lowerValue < t.Value.TimeSlice.EndPosition &&
            //                                  (upperValue == null || t.Value.TimeSlice.EndPosition < upperValue)) 
            //                //interval has end date
            //                ).
            //                Select(t => t.Value).ToList();
            //            //TODO: do it normal
            //#warning fix
            //            upperValue = null;
            //            return Values.Where(t =>
            //                             lowerValue < t.Value.TimeSlice.BeginPosition
            //                             && (upperValue == null || t.Value.TimeSlice.BeginPosition < upperValue)
            //                                 //interval has start date
            //                             ||
            //                             (t.Value.TimeSlice.BeginPosition < lowerValue &&
            //                              t.Value.TimeSlice.EndPosition != null &&
            //                              lowerValue < t.Value.TimeSlice.EndPosition &&
            //                              (upperValue == null || t.Value.TimeSlice.EndPosition < upperValue))
            //                //interval has end date
            //            ).
            //            Select(t => t.Value).ToList();


            var result = new List<T>();

            var pair = new KeyValuePair<DateTime, T>(lowerValue, null);
            int i = Values.BinarySearch(pair, _comparer);
            if (i < 0)
            {
                i = ~i;

                if (i >= Values.Count) return result;
            }


            //get first with selected date

            //get back 
            while (Values[i].Value.TimeSlice.BeginPosition >= lowerValue && i > 0)
            {
                i--;
            }

            //if we have got to far, get 1 step forward
            if (Values[i].Value.TimeSlice.BeginPosition < lowerValue)
            {
                i++;
            }


            if (upperValue == null)
            {
                for (; i < Values.Count; i++)
                {
                    result.Add(Values[i].Value);
                }
            }
            else
            {
                for (; i < Values.Count; i++)
                {
                    var data = Values[i].Value;
                    if (data.TimeSlice.BeginPosition <= upperValue)
                    {
                        result.Add(data);
                    }
                    else
                    {
                        if (andOneMore)
                        {
                            result.Add(data);
                        }
                        break;
                    }
                }
            }

            return result;
        }

        public T DeleteEventBySequence(T cancellation)
        {
            List<KeyValuePair<DateTime, T>> itemList =
                Values.Where(t => t.Value.Version.SequenceNumber == cancellation.Version.SequenceNumber
                && t.Value.Version.CorrectionNumber < cancellation.Version.CorrectionNumber
                && cancellation.Interpretation == t.Value.Interpretation).ToList();
            if (itemList.Count == 0) return null;

            KeyValuePair<DateTime, T> item = itemList.First();
            Values.Remove(item);


            UpdateLifeTimeRemove(item.Value);

            return item.Value;
        }

        public bool HasDecomissioningOn(DateTime actual)
        {
            if (IsActual)
            {
                var decommisioning = Values.Where(t => !t.Value.IsCanceled && t.Value.LifeTimeEndSet).ToList();
                if (decommisioning.Count > 0)
                {
                    if (decommisioning[0].Value.LifeTimeEnd < actual)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public bool HasActualOn(DateTime actual)
        {
            if (IsActual)
            {


                return Values.Any(t => t.Value.TimeSlice.BeginPosition <= actual &&
                                         (t.Value.TimeSlice.EndPosition == null ||
                                          t.Value.TimeSlice.EndPosition > actual));
            }


            return false;
            //return _values.Where(t => t.Value.IsCanceled == false &&
            //                         t.Value.TimeSlice.BeginPosition <= actual &&
            //                         (t.Value.TimeSlice.EndPosition == null || t.Value.TimeSlice.EndPosition > actual)).Any();
        }

        #region Implementation of IEnumerable

        public IEnumerator<T> GetEnumerator()
        {
            return Values.Select(t => t.Value).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        #region Nested type: KeyComparer

        private class KeyComparer<TKey, TValue> : IComparer<KeyValuePair<TKey, TValue>>
            where TKey : IComparable
        {
            #region Implementation of IComparer<in KeyValuePair<TKey,TValue>>

            public int Compare(KeyValuePair<TKey, TValue> x, KeyValuePair<TKey, TValue> y)
            {
                return x.Key.CompareTo(y.Key);
            }

            #endregion
        }

        #endregion


    }
}