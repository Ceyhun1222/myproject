using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace Aran.Temporality.CommonUtil.Util
{
    public class ServerConfig
    {
        public Dictionary<string, ServerConfigObject> Settings { get; private set; } =
            new Dictionary<string, ServerConfigObject>();

        private static string ConfigFullName => ConfigFilePath + ConfigFileName;
        private static string ConfigFileName { get; set; } = "server.xml";
        private static readonly string ConfigFilePath = 
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\risk\\aran\\";

        public ServerConfig(string configFileName = null)
        {
            if(configFileName != null) 
                ConfigFileName = configFileName;
        }

        public void Load()
        {
            if (!File.Exists(ConfigFullName))
                throw new FileNotFoundException($"Server config file not found", ConfigFullName);

            Settings = new Dictionary<string, ServerConfigObject>();

            var configs = new XmlDocument();
            configs.Load(ConfigFullName);

            var configList = configs.GetElementsByTagName("Config");

            if (configList.Count == 0)
                throw new Exception("Incorrect server config file");

            var serializer = new XmlSerializer(typeof(ServerConfigObject));
            
            for (var i = 0; i < configList.Count; i++)
            {
                var setting = (ServerConfigObject)serializer.Deserialize(new XmlNodeReader(configList[i]));

                if (setting == null) continue;

                if (setting.ConfigName == null)
                    setting.ConfigName = "Default";

                setting.DecryptDbConnection();

                Settings[setting.ConfigName] = setting;
            }
        }

        public void Save(string fullName = null)
        {
            if (!Directory.Exists(ConfigFilePath))
                Directory.CreateDirectory(ConfigFilePath);

            foreach (var setting in Settings)
            {
                setting.Value.EncryptDbConnection();
            }

            GetXmlDocument().Save(ConfigFullName);
            if (File.Exists(fullName ?? ConfigFullName))
                File.Delete(fullName ?? ConfigFullName);

            GetXmlDocument().Save(fullName ?? ConfigFullName);
        }

        private XmlDocument GetXmlDocument()
        {
            var configsXml = new XmlDocument();
            var serializer = new XmlSerializer(typeof(ServerConfigObject));
            var emptyNamepsaces = new XmlSerializerNamespaces(new[] {
                XmlQualifiedName.Empty
            });

            configsXml.AppendChild(configsXml.CreateXmlDeclaration("1.0", "UTF-8", null));

            var rootNode = configsXml.AppendChild(configsXml.CreateElement("Configs"));
            var nav = rootNode.CreateNavigator();

            using (var writer = nav.AppendChild())
            {
                writer.WriteWhitespace("");
                foreach (var configObject in Settings)
                {
                    serializer.Serialize(writer, configObject.Value, emptyNamepsaces);
                }
            }

            return configsXml;
        }
        
    }
}
