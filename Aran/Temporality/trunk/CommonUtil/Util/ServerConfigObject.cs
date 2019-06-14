using System;
using System.Xml;
using System.Xml.Serialization;
using Aran.Temporality.Common.Enum;
using System.Linq;

namespace Aran.Temporality.CommonUtil.Util
{
    [Serializable, XmlRoot("Config")]
    public class ServerConfigObject
    {
        [XmlAttribute("name")]
        public string ConfigName { get; set; } = "Default";

        public string Serial { get; set; }
        public string MongoSerial { get; set; }
        public string DllRepo { get; set; } = "C:\\DllRepo";

        [XmlAnyElement("LicenseComment")]
        public XmlComment LicenseComment { get => new XmlDocument().CreateComment(string.Join(", ", Enum.GetNames(typeof(EsriLicense)))); set { } }
        public EsriLicense License { get; set; } = EsriLicense.Basic;

        public SystemType SystemType { get; set; } = SystemType.Production;
        public int ServicePort { get; set; } = 8523;
        public int HelperPort { get; set; } = 8524;
        public int ExternalPort { get; set; } = 8525;

        [XmlAnyElement("RepositoryTypeComment")]
        public XmlComment RepositoryTypeComment { get => new XmlDocument().CreateComment(string.Join(", ", Enum.GetNames(typeof(RepositoryType)))); set { } }
        public RepositoryType RepositoryType { get; set; } = RepositoryType.MongoWithBackupRepository;

        public string RedisConnectionString { get; set; } = "localhost:6379";
        public bool UseRedisForMetaCache { get; set; } = false;

        [XmlIgnore]
        public string DbName { get; set; } = "temporality";
        [XmlIgnore]
        public string DbAddress { get; set; } = "127.0.0.1";
        [XmlIgnore]
        public short DbPort { get; set; } = 5432;
        [XmlIgnore]
        public string DbUser { get; set; }
        [XmlIgnore]
        public string DbPassword { get; set; }
        [XmlIgnore]
        public string ConnectionString { get; private set; }

        [XmlIgnore]
        public string MongoServerAddress { get; set; } = "localhost";
        [XmlIgnore]
        public int MongoServerPort { get; set; } = 27017;
        [XmlIgnore]
        public string MongoUser { get; set; }
        [XmlIgnore]
        public string MongoPassword { get; set; }

        public bool MongoCreateGeoIndex { get; set; } = false;

        private const string Secret = "secret1";

        public void EncryptDbConnection()
        {
            Serial = Crypto.EncryptStringAes($"{DbAddress}|{DbPort}|{DbUser}|{DbPassword}|{DbName}", Secret);
            MongoSerial = Crypto.EncryptStringAes($"{MongoServerAddress}|{MongoServerPort}|{MongoUser}|{MongoPassword}", Secret);
        }

        public void DecryptDbConnection()
        {
            try
            {
                ConnectionString = Crypto.DecryptStringAes(Serial ?? "", Secret);

                var parts = ConnectionString?.Split('|');
                if (parts == null || parts.Length != 5)
                    return;

                DbAddress = parts[0];
                DbPort = short.Parse(parts[1]);
                DbUser = parts[2];
                DbPassword = parts[3];
                DbName = parts[4];
            }
            catch
            {
                // ignored
            }

            try
            {
                var mongoParts = Crypto.DecryptStringAes(MongoSerial ?? "", Secret)?.Split('|');
                if (mongoParts == null || mongoParts.Length != 4)
                    return;

                MongoServerAddress = mongoParts[0];
                MongoServerPort = int.Parse(mongoParts[1]);
                MongoUser = mongoParts[2];
                MongoPassword = mongoParts[3];
            }
            catch
            { 
                // ignored
            }

        }
    }
}
