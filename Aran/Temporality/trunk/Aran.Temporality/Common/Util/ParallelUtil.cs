using System;
using System.Collections.Generic;
using System.Linq;
using Aran.Temporality.Common.Config;

namespace Aran.Temporality.Common.Util
{
    public class ParallelUtil
    {
        public static void ProcessList<TItem, TSubresult>(IList<TItem> source,
            Func<TSubresult> initSubThreadAction,
            Action<TItem, TSubresult> itemProcessAction,
            Action<TSubresult> finalSubThreadAction) 
        {
            var total = source.Count;
            
            var forker=new Forker();

            var cores = ConfigUtil.CoreCount();
            //prepare forks
            for (var coreIndex=0;coreIndex<cores;coreIndex++)
            {
                var localCoreIndex = coreIndex;
                forker.Fork(() =>
                {
                    var subresult = initSubThreadAction();
                    for (var threadIndex = 0; localCoreIndex + threadIndex < total; threadIndex += cores)
                    {
                        itemProcessAction(source[localCoreIndex + threadIndex], subresult);
                    }
                    finalSubThreadAction(subresult);
                });
            }
            //fork all
            forker.Join();
        }

      

        public static void ProcessCollection<TItem, TSubresult>(IEnumerable<TItem> source,
           Func<TSubresult> initSubThreadAction,
           Action<TItem, TSubresult> itemProcessAction,
           Action<TSubresult> finalSubThreadAction)
        {
            var forker = new Forker();

            var cores = ConfigUtil.CoreCount();

            var splitedSource = CollectionUtil.SplitCollection(source, cores).ToList();

            //prepare forks
            for (var coreIndex = 0; coreIndex < cores; coreIndex++)
            {
                var localCoreIndex = coreIndex;
                forker.Fork(() =>
                {
                    var subresult = initSubThreadAction();
                    var subCollection = splitedSource[localCoreIndex];
                    foreach (var item in subCollection)
                    {
                        itemProcessAction(item, subresult);
                    }
                    finalSubThreadAction(subresult);
                });
            }
            //fork all
            forker.Join();
        }
    }
}
