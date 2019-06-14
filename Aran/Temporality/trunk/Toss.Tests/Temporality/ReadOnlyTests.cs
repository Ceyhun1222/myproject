using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using Aran.Aim;
using Aran.Aim.Features;
using Aran.Aim.Utilities;
using Aran.Temporality.Common.Entity;
using Aran.Temporality.Common.Entity.Util;
using Aran.Temporality.Common.Enum;
using Aran.Temporality.Common.Id;
using Aran.Temporality.Common.Util;
using Aran.Temporality.CommonUtil.Context;
using Xunit;

namespace Toss.Tests.Temporality
{
    [Collection(nameof(Names.ServiceCollection))]
    [TestCaseOrderer("Toss.Tests.PriorityOrderer", "Toss.Tests")]
    public class ReadOnlyTests
    {
        protected readonly DataFixture _dataFixture;

        public ReadOnlyTests(DataFixture noAixmFixture)
        {
            _dataFixture = noAixmFixture;
        }

        public static int NumberOfTests = 1;
        public static string ReportPath = @"C:\TOSS\test\logs\reports\";
        public static string ReportFileName;
        public static bool WriteToFile;

        static ReadOnlyTests()
        {
            try
            {
                var conf = ConfigurationManager.OpenExeConfiguration("Toss.Tests.dll");

                if (int.TryParse(conf.AppSettings?.Settings["NumberOfTests"]?.Value, out var intResult))
                    NumberOfTests = intResult;

                if (bool.TryParse(conf.AppSettings?.Settings["WriteToFile"].Value, out var boolResult))
                    WriteToFile = boolResult;

                if (WriteToFile)
                {
                    if (conf.AppSettings?.Settings["ReportPath"].Value != null)
                        ReportPath = conf.AppSettings.Settings["ReportPath"].Value;

                    ReportFileName = ReportPath + "report_" + DateTime.Now.ToString("yy.MM.dd_H.mm.ss") + ".log";

                    Log(conf.AppSettings?.Settings["EventStorageRepository"].Value ?? "Default");
                }
            }
            catch
            {
                // ignored
            }
        }

        private static void Log(object text)
        {
            if (WriteToFile)
                File.AppendAllText(ReportFileName, text.ToString());
        }

        [Fact]
        public void GetActualFeatures()
        {
#if !TestByTossm
            return;
#endif
            StringBuilder log = new StringBuilder("\n**********GetActualFeatures**********\n\n");
            var timer = new Stopwatch();
            foreach (FeatureType featType in Enum.GetValues(typeof(FeatureType)))
            {
                log.Append($"\n{Enum.GetName(typeof(FeatureType), featType)},");

                for (int testNumber = 1; testNumber <= NumberOfTests; testNumber++)
                {
                    timer.Restart();
                    var res = _dataFixture.GetActualDataByFeatureType(featType, 0, true, DateTime.Now);
                    timer.Stop();

                    log.Append($"{timer.Elapsed.TotalSeconds},");
                }
            }

            Log(log);
        }

        [Fact]
        public void GetActualFeaturesRemote()
        {
#if !TestByTossm
            return;
#endif
            StringBuilder log = new StringBuilder("\n**********GetActualFeaturesRemote**********\n\n");
            var timer = new Stopwatch();
            CurrentDataContext.ServiceAddress = "127.0.0.1:8523";
            CurrentDataContext.UserId = 1;
            CurrentDataContext.CurrentPassword = null;
            CurrentDataContext.StorageName = "toss";
            CurrentDataContext.Login();

            foreach (FeatureType featType in Enum.GetValues(typeof(FeatureType)))
            {
                log.Append($"\n{Enum.GetName(typeof(FeatureType), featType)},");

                for (int testNumber = 1; testNumber <= NumberOfTests; testNumber++)
                {
                    timer.Restart();
                    CurrentDataContext.CurrentService.GetActualDataByDate(new FeatureId { FeatureTypeId = (int)featType, WorkPackage = 0 }, false, DateTime.Now);
                    timer.Stop();

                    log.Append($"{timer.Elapsed.TotalSeconds},");
                }
            }

            Log(log);
        }

        [Fact]
        public void GetActualVerticalStructures()
        {
#if !TestByTossm
            return;
#endif
            StringBuilder log = new StringBuilder("\n**********GetActualVerticalStructures**********\n\n");
            var timer = new Stopwatch();
            var featType = FeatureType.VerticalStructure;
            log.Append($"\n{Enum.GetName(typeof(FeatureType), featType)},");

            for (int testNumber = 1; testNumber <= NumberOfTests; testNumber++)
            {
                timer.Restart();
                _dataFixture.GetActualDataByFeatureType(featType, 0, true, DateTime.Now);
                timer.Stop();

                log.Append($"{timer.Elapsed.TotalSeconds},");
            }

        }

        [Fact]
        public void ClearMissingLinks()
        {
#if !TestByTossm
            return;
#endif
            var problemBytes = _dataFixture.NoAixmDataService.GetProblemReport(0, 621, 0, ReportType.MissingLinkReport);
            var problems = FormatterUtil.ObjectFromBytes<List<ProblemReportUtil>>(problemBytes.ReportData)
                .Cast<LinkProblemReportUtil>().ToList();

            var features = new List<Feature>();

            foreach (var featureId in problems.Select(x => new {x.Guid, x.FeatureType}).Distinct())
            {
                Feature feature = features.FirstOrDefault(x => x.Identifier == featureId.Guid);
                if (feature == null)
                {
                    var res = _dataFixture.GetActualDataByDate(new FeatureId
                    {
                        WorkPackage = 621,
                        Guid = featureId.Guid,
                        FeatureTypeId = (int) featureId.FeatureType
                    }, false, DateTime.MaxValue);
                    
                    if (res.Count == 0)
                        continue;

                    feature = res.First().Data.Feature;
                    features.Add(feature);
                }

                var featureProblems = problems.Where(x => x.Guid == featureId.Guid).ToList();

                var oldReferences = new List<RefFeatureProp>();
                AimMetadataUtility.GetReferencesFeatures(feature, oldReferences);

                var needReferences = oldReferences.Where(oldReference =>
                    !featureProblems.Select(problemReference => problemReference.ReferenceFeatureIdentifier).Contains(oldReference.RefIdentifier));

                foreach (var problem in featureProblems)
                {
                    Assert.Contains(problem.ReferenceFeatureIdentifier, oldReferences.Select(x => x.RefIdentifier));
                }

                AimMetadataUtility.ClearMissingLinks(feature, featureProblems
                    .GroupBy(x => x.PropertyPath, x => x.ReferenceFeatureIdentifier)
                    .ToDictionary(x => x.Key, x => x.ToList()));

                var newReferences = new List<RefFeatureProp>();
                AimMetadataUtility.GetReferencesFeatures(feature, newReferences);
                
                foreach (var problem in featureProblems)
                {
                    Assert.DoesNotContain(problem.ReferenceFeatureIdentifier, newReferences.Select(x => x.RefIdentifier));
                }
                foreach (var reference in needReferences)
                {
                    Assert.Contains(reference.RefIdentifier, newReferences.Select(x => x.RefIdentifier));
                }
            }
        }
    }
}