using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace Aran.Temporality.CommonUtil.Util
{
    public class ClientConfig
    {
        public Dictionary<string, ClientConfigObject> Settings { get; private set; } = 
            new Dictionary<string, ClientConfigObject>();

        private static string ConfigFullName => ConfigFilePath + ConfigFileName;
        private static string ConfigFileName { get; set; } = "config.xml";
        private static readonly string ConfigFilePath = 
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\risk\\aran\\";

        public ClientConfig(string configFileName = null)
        {
            if(configFileName != null) 
                ConfigFileName = configFileName;
        }

        public void Load()
        {
            if (!File.Exists(ConfigFullName))
                throw new FileNotFoundException($"Client config file not found", ConfigFullName);

            Settings = new Dictionary<string, ClientConfigObject>();

            var configs = new XmlDocument();
            configs.Load(ConfigFullName);

            var configList = configs.GetElementsByTagName("Config");

            if (configList.Count == 0)
                throw new Exception("Incorrect client config file");

            var serializer = new XmlSerializer(typeof(ClientConfigObject));

            for (var i = 0; i < configList.Count; i++)
            {
                var setting = (ClientConfigObject)serializer.Deserialize(new XmlNodeReader(configList[i]));

                if (setting == null) continue;

                if (setting.ConfigName == null)
                    setting.ConfigName = "Default";

                
                if (setting.ServiceAddress.LastIndexOf(':') >= 0)
                {
                    setting.ServicePort = setting.ServiceAddress.Substring(setting.ServiceAddress.LastIndexOf(':') + 1);
                }

                if (setting.HelperAddress.LastIndexOf(':') >= 0)
                {
                    setting.HelperPort = setting.HelperAddress.Substring(setting.HelperAddress.LastIndexOf(':') + 1);
                }

                Settings[setting.ConfigName] = setting;
            }
        }

        public void Save(string fullName = null)
        {
            if (!Directory.Exists(ConfigFilePath))
                Directory.CreateDirectory(ConfigFilePath);

            GetXmlDocument().Save(ConfigFullName);
            if (File.Exists(fullName ?? ConfigFullName))
                File.Delete(fullName ?? ConfigFullName);

            GetXmlDocument().Save(fullName ?? ConfigFullName);
        }

        private XmlDocument GetXmlDocument()
        {
            var configsXml = new XmlDocument();
            var serializer = new XmlSerializer(typeof(ClientConfigObject));
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
