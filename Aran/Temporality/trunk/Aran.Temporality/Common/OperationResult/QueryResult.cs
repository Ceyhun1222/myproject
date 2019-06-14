using System;
using System.Collections.Generic;
using System.Linq;
using Aran.Temporality.Common.Enum;

namespace Aran.Temporality.Common.OperationResult
{
    [Serializable]
    public class QueryResult<T> : CommonOperationResult
    {
        public IDictionary<int, IList<T>> Results { get; set; }

        public int Count()
        {
            if (Results == null)
                return 0;
            return Results.Values.Sum(t => t.Count);
        }

        public override string ToString()
        {
            return base.ToString() + (Results == null
                                          ? string.Empty
                                          : " " + (Count() + " results in " + Results.Count) + " classes");
        }

        public void Combine(QueryResult<T> subResult, LogicalCondition logicalCondition)
        {
            if (logicalCondition == LogicalCondition.Or)
            {
                foreach (var pair in subResult.Results)
                {
                    if (!Results.ContainsKey(pair.Key))
                    {
                        Results[pair.Key] = new List<T>(pair.Value);
                    }
                    else
                    {
                        IList<T> list = Results[pair.Key];
                        foreach (T t in pair.Value)
                        {
                            if (!list.Contains(t))
                            {
                                list.Add(t);
                            }
                        }
                    }
                }
            }
            else if (logicalCondition == LogicalCondition.And)
            {
                //remove uncommon keys
                var needToRemove = new List<int>(Results.Keys.Except(subResult.Results.Keys));
                foreach (int key in needToRemove)
                {
                    Results.Remove(key);
                }
                //intersect values for common keys
                foreach (var pair in Results)
                {
                    Results[pair.Key] = (IList<T>) subResult.Results[pair.Key].Intersect(pair.Value);
                }
            }
        }
    }
}