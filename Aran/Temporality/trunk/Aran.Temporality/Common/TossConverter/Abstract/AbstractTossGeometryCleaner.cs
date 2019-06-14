using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Aran.Temporality.Common.Abstract.Event;
using Aran.Temporality.Common.Aim.MetaData;
using Aran.Temporality.Common.TossConverter.Interface;

namespace Aran.Temporality.Common.TossConverter.Abstract
{
    public abstract class AbstractTossGeometryCleaner
    {
        protected abstract ITossGeometryCleaner GetGeometryCleaner(string repositoryName);

        public Action<MessageCauseType, string, string> Logger { get; set; } = null;

        public void Clean(string repositoryName, CancellationTokenSource token = null)
        {
            var cleaner = GetGeometryCleaner(repositoryName);

            List<int> workPackages = null;
            try
            {
                workPackages = cleaner.GetWorkPackages();
            }
            catch (Exception e)
            {
                Logger(MessageCauseType.CommonError, "Error while reading workpackages int the first repository", e.ToString());
            }

            if (workPackages == null)
                return;

            Logger(MessageCauseType.SystemMessage, $"Cleaning of the {repositoryName} started", "");

            var index = 1;
            foreach (var workPackage in workPackages)
            {
                Logger(MessageCauseType.SystemMessage, $"WorkPackage {workPackage} ({index++}/{workPackages.Count})", "");

                try
                {
                    var errors = cleaner.Clean(workPackage);

                    if (token?.IsCancellationRequested == true)
                        return;

                    foreach (var error in errors)
                    {
                        if (token?.IsCancellationRequested == true)
                            return;

                        Logger(error.Item1, error.Item2, error.Item3);
                    }
                }
                catch (Exception ex)
                {
                    Logger(MessageCauseType.SystemMessage, "Unfortunately, geometry problems were not fixed.", ex.Message);
                    return;
                }
            }

            Logger(MessageCauseType.SystemMessage, "Successfully completed!", "");
        }
    }
}
