using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using Aran.Aim;
using Aran.Aim.Features;
using Xunit;

namespace Toss.Tests.Temporality
{
    [Collection(nameof(Names.ServiceCollection))]
    [TestCaseOrderer("Toss.Tests.PriorityOrderer", "Toss.Tests")]
    public class DatabaseStressTests : TemporalityTests
    {
        public DatabaseStressTests(DataFixture noAixmFixture) : base(noAixmFixture)
        {
        }

        public static int NumberOfTests = 1;
        public static string ReportPath = @"C:\TOSS\test\logs\reports\";
        public static string ReportFileName;
        public static bool WriteToFile;

        static DatabaseStressTests()
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

                    if (!Directory.Exists(ReportPath))
                        Directory.CreateDirectory(ReportPath);

                    ReportFileName = ReportPath + "raport_" + DateTime.Now.ToString("yy.MM.dd_H.mm.ss") + ".log";

                    Log((conf.AppSettings?.Settings["EventStorageRepository"].Value ?? "Default") + ", " +
                        (conf.AppSettings?.Settings["CachedEventStorageRepository"].Value ?? "Default"));

                    Log($"NumberOfTests = {NumberOfTests}, FeatureTypesCount = {FeatureTypesCount}, " +
                        $"FeatureDuplicatsCount = {FeatureDuplicatsCount}");
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
        public void _01_ReadConsecutiveFeaturesFromOneSlot()
        {
            var timer = new Stopwatch();
            StringBuilder log = new StringBuilder("\n*********************\n");
            log.Append("_01_ReadConsecutiveFeaturesFromOneSlot\n");

            for (int testNumber = 1; testNumber <= NumberOfTests; testNumber++)
            {
                log.Append($"\nTest number {testNumber}\n");

                List<Feature> features = new List<Feature>();

                for (int i = 0; i < FeatureDuplicatsCount; i++)
                {
                    var feature = CreateFeature(FeatureType.AirportHeliport);
                    ApplyTimeSlice(feature);
                    timer.Start();
                    Commit(feature);
                    timer.Stop();
                    features.Add(feature);
                }

                log.Append($"Write: {timer.Elapsed:g}\n");

                timer.Reset();
                foreach (var feature in features)
                {
                    timer.Start();
                    var list = _dataFixture.GetFeatures(feature.Identifier, feature.FeatureType);
                    timer.Stop();
                    AssertResult(feature, list);
                }

                log.Append($"Read: {timer.Elapsed:g}\n");
            }
            Log(log);
        }

        [Fact]
        public void _02_ReadRandomFeaturesFromOneSlot()
        {
            var timer = new Stopwatch();
            StringBuilder log = new StringBuilder("\n*********************\n");
            log.Append("_02_ReadRandomFeaturesFromOneSlot\n");

            for (int testNumber = 1; testNumber <= NumberOfTests; testNumber++)
            {
                log.Append($"\nTest number {testNumber}\n");

                var features = new List<Feature>();

                for (var i = 0; i < FeatureDuplicatsCount; i++)
                {
                    var feature = CreateFeature(FeatureType.AirportHeliport);
                    ApplyTimeSlice(feature);
                    timer.Start();
                    Commit(feature);
                    timer.Stop();
                    features.Add(feature);
                }

                log.Append($"Write: {timer.Elapsed:g}\n");

                timer.Reset();
                Random rnd = new Random();
                for (var i = 0; i < FeatureDuplicatsCount; i++)
                {
                    var feature = features[rnd.Next(FeatureDuplicatsCount)];
                    timer.Start();
                    var list = _dataFixture.GetFeatures(feature.Identifier, feature.FeatureType);
                    timer.Stop();
                    AssertResult(feature, list);
                }

                log.Append($"Read: {timer.Elapsed:g}\n");
            }

            Log(log);
        }

        [Fact]
        public void _03_ReadConsecutiveFeaturesFromManySlots()
        {
            var timer = new Stopwatch();
            StringBuilder log = new StringBuilder("\n*********************\n");
            log.Append("_03_ReadConsecutiveFeaturesFromManySlots\n");

            for (int testNumber = 1; testNumber <= NumberOfTests; testNumber++)
            {
                log.Append($"\nTest number {testNumber}\n");

                Dictionary<Feature, int> features = new Dictionary<Feature, int>();
                int oneSlotFeaturesCount = FeatureDuplicatsCount / FeatureTypesCount;
                for (int i = 0; i < FeatureTypesCount; i++)
                {
                    int privateSlotId = _dataFixture.AddPrivateSlot();
                    for (int j = 0; j < oneSlotFeaturesCount; j++)
                    {
                        var feature = CreateFeature(FeatureType.AirportHeliport);
                        ApplyTimeSlice(feature);
                        timer.Start();
                        Commit(feature);
                        timer.Stop();
                        features.Add(feature, privateSlotId);
                    }
                }

                log.Append($"Write: {timer.Elapsed:g}\n");

                timer.Reset();
                foreach (var feature in features)
                {
                    timer.Start();
                    var list = _dataFixture.GetFeatures(feature.Key.Identifier, feature.Key.FeatureType, workPackage: feature.Value);
                    timer.Stop();
                    AssertResult(feature.Key, list);
                }

                log.Append($"Read: {timer.Elapsed:g}\n");
            }

            Log(log);
        }

        [Fact]
        public void _04_ReadRandomFeaturesFromManySlots()
        {
            var timer = new Stopwatch();
            StringBuilder log = new StringBuilder("\n*********************\n");
            log.Append("ReadRandomFeaturesFromManySlots\n");

            for (int testNumber = 1; testNumber <= NumberOfTests; testNumber++)
            {
                log.Append($"\nTest number {testNumber}\n");

                Dictionary<Feature, int> features = new Dictionary<Feature, int>();
                int oneSlotFeaturesCount = FeatureDuplicatsCount / FeatureTypesCount;
                for (int i = 0; i < FeatureTypesCount; i++)
                {
                    int privateSlotId = _dataFixture.AddPrivateSlot();
                    for (int j = 0; j < oneSlotFeaturesCount; j++)
                    {
                        var feature = CreateFeature(FeatureType.AirportHeliport);
                        ApplyTimeSlice(feature);
                        timer.Start();
                        Commit(feature);
                        timer.Stop();
                        features.Add(feature, privateSlotId);
                    }
                }

                log.Append($"Write: {timer.Elapsed:g}\n");

                Random rnd = new Random();

                timer.Restart();
                for (var i = 0; i < features.Count; i++)
                {
                    var feature = features.ElementAt(rnd.Next(features.Count));
                    timer.Start();
                    var list = _dataFixture.GetFeatures(feature.Key.Identifier, feature.Key.FeatureType, workPackage: feature.Value);
                    timer.Stop();
                    AssertResult(feature.Key, list);
                }
                timer.Stop();
                log.Append($"Read: {timer.Elapsed:g}\n");
            }

            Log(log);
        }

    }
}