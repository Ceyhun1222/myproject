using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Aran.Aim;
using Aran.Aim.BusinessRules;
using Aran.Temporality.Common.Aim.MetaData;
using Aran.Temporality.Common.Entity;
using Aran.Temporality.Common.Entity.Util;
using Aran.Temporality.Common.Util;
using Aran.Temporality.Internal.WorkFlow.BusinessRules;
using Aran.Temporality.Internal.WorkFlow.BusinessRules.Util;
using NHibernate.Linq;

namespace Aran.Temporality.Internal.WorkFlow.Routines
{
    internal abstract class AbstractCheckRoutine
    {
        #region Virtual methods
        public static Array AllFeatureTypes = Enum.GetValues(typeof(FeatureType));
        public virtual void InitPrivateSlot()  { }
        public virtual void InitPublicSlot() { }
        public virtual void ReleasePrivateSlot() { }
        public virtual void ReleasePublicSlot() { }
        public abstract int GetReportType();
        public virtual bool CheckPublicSlot()
        {
            return true;
        }
        public virtual List<ProblemReportUtil> CheckFeature(AimFeature aimFeature)
        {
            CurrentOperationStatus.NextOperation();
            NextFeature();
            return null;
        }

        public virtual bool CheckFeature(AimFeature aimFeature, List<ProblemReportUtil> problems)
        {
            CurrentOperationStatus.NextOperation();
            NextFeature();
            return true;
        }

        #endregion
         
        public int FeaturesProcessed;
        public int ErrorFound;

        public void NextFeature()
        {
            Interlocked.Increment(ref FeaturesProcessed);
        }

        public void NextError()
        {
            Interlocked.Increment(ref ErrorFound);
        }

        public virtual bool CheckPrivateSlot()
        {
            FeaturesProcessed = 0;
            ErrorFound = 0;
            //
            InitPrivateSlot();
            //calculated amount of features to process
            var union =
                AllFeatureTypes.Cast<FeatureType>().Aggregate((IEnumerable<AimFeature>)new List<AimFeature>(),
                    (current, featureType) => current.Union(Context.LoadStates(featureType)));

            var totalCount = union.Count();
            CurrentOperationStatus.NextTask(totalCount);
            //union all features

            var problems = new ConcurrentBag<List<ProblemReportUtil>>();
            var result = true;

            //union all features
            //union = AllFeatureTypes.Cast<FeatureType>().Aggregate((IEnumerable<AimFeature>)new List<AimFeature>(),
            //     (current, featureType) => current.Union(Context.LoadStates(featureType)));

            //perform check

            //        ParallelUtil.ProcessCollection(union,
            //            () => new List<ProblemReportUtil>(),
            //            (feature, subresult) =>
            //{
            //	result &= CheckFeature(feature, subresult);
            //            },
            //            subresult => { problems.Add(subresult); }
            //            );


            // New business rules
         
            var subres = new List<ProblemReportUtil>(); 


            union.ForEach(t => CheckFeature(t, subres));
            problems.Add(subres);
            //union = AllFeatureTypes.Cast<FeatureType>().Aggregate((IEnumerable<AimFeature>)new List<AimFeature>(),
            //  (current, featureType) => current.Union(Context.LoadStates(featureType)));

            //Parallel.ForEach(union, () => new List<ProblemReportUtil>(),
            //    (feature, loopState, subProblems) =>
            //    {
            //        CheckFeature(feature, subProblems);
            //        return subProblems;
            //    },
            //    (subProblems) =>
            //    {
            //        if (subProblems.Count > 0)
            //        {
            //            problems.Add(subProblems);
            //        }
            //    });

            result = problems.Count > 0;
            result = !result;

            ReleasePrivateSlot();
            //save result
            Context.Service.UpdateProblemReport(new ProblemReport
            {
                PrivateSlotId = Context.PrivateSlot.Id,
                PublicSlotId = 0,//just private slot
                ReportType = GetReportType(),
                ReportData = FormatterUtil.MultiCollectionToBytes(problems.ToArray(),false)
            });
            return result;
        }

        public RoutineContext Context { get; set; }
    }
}
