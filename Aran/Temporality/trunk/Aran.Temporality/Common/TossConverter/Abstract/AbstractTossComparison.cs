using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Aran.Temporality.Common.Abstract.Event;
using Aran.Temporality.Common.Aim.MetaData;
using Aran.Temporality.Common.Interface;
using Aran.Temporality.Common.TossConverter.Interface;

namespace Aran.Temporality.Common.TossConverter.Abstract
{
    public abstract class AbstractTossComparison
    {
        protected abstract ITossReadableRepository GetComparisonFirst(string repositoryName);

        protected abstract ITossReadableRepository GetComparisonSecond(string repositoryName);

        public Action<MessageCauseType, string, string> Logger { get; set; } = null;

        public void Compare(string repositoryNameFirst, string repositoryNameSecond, CancellationTokenSource token = null)
        {
            var comparisonFirst = GetComparisonFirst(repositoryNameFirst);
            var comparisonSecond = GetComparisonSecond(repositoryNameSecond);

            List<int> workPackagesFirst = null;
            try
            {
                workPackagesFirst = comparisonFirst.GetWorkPackages();
            }
            catch (Exception e)
            {
                Logger(MessageCauseType.CommonError, "Error while reading workpackages int the first repository", e.ToString());
            }

            List<int> workPackagesSecond = null;
            try
            {
                workPackagesSecond = comparisonSecond.GetWorkPackages();
            }
            catch (Exception e)
            {
                Logger(MessageCauseType.CommonError, "Error while reading workpackages int the second repository", e.ToString());
            }

            if (workPackagesFirst == null || workPackagesSecond == null)
                return;


            Logger(MessageCauseType.SystemMessage, $"Compare {repositoryNameFirst} with {repositoryNameSecond} started", "");

            var commonWorkPackages = workPackagesFirst.Intersect(workPackagesSecond).ToList();

            foreach (var workPackage in workPackagesFirst.Except(commonWorkPackages))
                Logger.Invoke(MessageCauseType.CommonError, $"WorkPackage {workPackage} does not exist in the second repository", "");

            foreach (var workPackage in workPackagesSecond.Except(commonWorkPackages))
                Logger.Invoke(MessageCauseType.CommonError, $"WorkPackage {workPackage} does not exist in the first repository", "");

            for (var index = 0; index < commonWorkPackages.Count; index++)
            {
                if (token?.IsCancellationRequested == true)
                    return;

                var workPackage = commonWorkPackages[index];
                var number = 0;

                IEnumerator<AbstractEvent<AimFeature>> enumeratorFirst = null;
                try
                {
                    enumeratorFirst = comparisonFirst.GetEventStorages(workPackage).GetEnumerator();
                }
                catch (Exception e)
                {
                    Logger(MessageCauseType.CommonError,
                        $"Error while accessing the {workPackage} Event Storage in the first repository", e.ToString());
                }

                IEnumerator<AbstractEvent<AimFeature>> enumeratorSecond = null;
                try
                {
                    enumeratorSecond = comparisonSecond.GetEventStorages(workPackage).GetEnumerator();
                }
                catch (Exception e)
                {
                    Logger(MessageCauseType.CommonError,
                        $"Error while accessing the {workPackage} Event Storage in the second repository",
                        e.ToString());
                }

                if (enumeratorFirst == null || enumeratorSecond == null)
                    continue;

                Logger(MessageCauseType.SystemMessage, $"WorkPackage {workPackage} ({index + 1}/{commonWorkPackages.Count})", "");

                while (true)
                {
                    if (token?.IsCancellationRequested == true)
                        return;

                    number++;

                    bool moveNextFirst;
                    try
                    {
                        moveNextFirst = enumeratorFirst.MoveNext();
                    }
                    catch (Exception e)
                    {
                        Logger(MessageCauseType.CommonError,
                            $"Error while reading the {workPackage}.{number} event in first repository",
                            e.ToString());
                        break;
                    }

                    bool moveNextSecond;
                    try
                    {
                        moveNextSecond = enumeratorSecond.MoveNext();
                    }
                    catch (Exception e)
                    {
                        Logger(MessageCauseType.CommonError,
                            $"Error while reading the {workPackage}.{number} event in second repository",
                            e.ToString());
                        break;
                    }

                    if (!moveNextFirst || !moveNextSecond)
                    {
                        if (moveNextFirst)
                            Logger(MessageCauseType.CommonError,
                                $"WorckPackage {workPackage} in the second repository is less ({number})", "");

                        if (moveNextSecond)
                            Logger(MessageCauseType.CommonError,
                                $"WorckPackage {workPackage} in the first repository is less ({number})", "");

                        break;
                    }

                    var abstractEventFirst = enumeratorFirst.Current;
                    var abstractEventSecond = enumeratorSecond.Current;


                    ConverterUtil.ApplyTimeSlice(abstractEventFirst);
                    ConverterUtil.ApplyTimeSlice(abstractEventSecond);

                    ConverterUtil.CheckFeatures(abstractEventFirst?.Data, abstractEventSecond?.Data, workPackage,
                        number, Logger);
                }
            }

            Logger(MessageCauseType.SystemMessage, "Successfully completed!", "");
        }
    }
}
