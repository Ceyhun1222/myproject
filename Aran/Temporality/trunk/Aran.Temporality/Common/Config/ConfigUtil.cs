using Aran.Temporality.Common.Enum;
using System;

namespace Aran.Temporality.Common.Config
{
    public class ConfigUtil
    {
        public static string StatePointDateFormat = "{0:dd/MM/yyyy}";
        public static string TimeSlicedDateFormat = "{0:dd/MM/yyyy}";

        public static int OptimizerSleepTime = 500; //in ms
        public static int WorkflowSleepTime = 3000; //in ms

        public static string TemporalityServerCertificateSubject = "tempCert";
        public static string TemporalityServerCertificateIssuer = "RootCATest";
        public static string TemporalityServerAddress = "172.30.31.18:8523";
        public static string TemporalityServerEndPoint = "Temporality";

        public static string HelperServerAddress = "172.30.31.18:8524";
        public static string HelperServerEndPoint = "Helper";

        public static string ExternalServerAddress = "172.30.31.18:8525";
        public static string ExternalServerEndPoint = "External";

        public static string StoragePath = @"c:\TOSS\db";

        public static string MongoServerAddress = "localhost";
        public static int MongoServerPort = 27017;
        public static string MongoUser;
        public static string MongoPassword;

        public static bool MongoCreateGeoIndex = true;
        public static RepositoryType RepositoryType = RepositoryType.MongoWithBackupRepository;

        public static bool UseWebApiForMetadata = false;

        public static string RedisConnectionString = "localhost:6379";
        public static bool UseRedisForMetaCache = false;

        public static bool UseEsri = true;

        public static string OwnChannelName = "";
        public static string RemoteChannelName = "";

        public static string NoDataServiceAddress = @"localhost";
        public static string NoDataServicePort = @"5432";
        public static string NoDataUser = @"postgres";
        public static string NoDataPassword = @"123456";
        public static string NoDataDatabase = @"temporality";
        public static string DllRepo = @"d:\DllRepo";

        public static string ExternalApplicationUserName;
        public static string ExternalApplication;



        private const int DefaultCoreCount = 4;
        private static int _coreCount = -1;

        public static void SetCoreCount(int cores)
        {
            _coreCount = cores > 0 && cores < 128 ? cores : DefaultCoreCount;
        }

        public static int CoreCount()
        {
            if (_coreCount == -1)
            {
                _coreCount = Environment.ProcessorCount;
            }

            if (_coreCount <= 0 || _coreCount > 64)
            {
                _coreCount = DefaultCoreCount;
            }

            return _coreCount;
        }
    }
}