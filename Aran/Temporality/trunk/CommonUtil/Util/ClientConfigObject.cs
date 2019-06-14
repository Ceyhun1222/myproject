using Aran.Temporality.Common.Enum;
using System;
using System.Configuration;
using System.Xml;
using System.Xml.Serialization;

namespace Aran.Temporality.CommonUtil.Util
{
    [Serializable, XmlRoot("Config")]
    public class ClientConfigObject
    {
        [XmlAttribute("name")]
        public string ConfigName { get; set; } = "Default";
        
        public string ServiceAddress { get; set; }
        public string HelperAddress { get; set; }
        public string StorageName { get; set; }
        public int UserId { get; set; } = 1;

        [XmlAnyElement("LicenseComment")]
        public XmlComment LicenseComment { get => new XmlDocument().CreateComment(string.Join(", ", Enum.GetNames(typeof(EsriLicense)))); set { } }
        public EsriLicense License { get; set; } = EsriLicense.Basic;

        [XmlIgnore]
        public string ServicePort { get; set; } = "8523";
        [XmlIgnore]
        public string HelperPort { get; set; } = "8524";

        public bool UseWebApiForMetadata { get; set; }

        public string WebApiAddress { get; set; }

        public string GetServiceAddress()
        {
            if (ServiceAddress != null && ServiceAddress?.IndexOf(':') >= 0)
            {
                return ServiceAddress.Remove(ServiceAddress.IndexOf(':'));
            }

            return ServiceAddress;
        }

        private static readonly string DefaultWebApiAddress;
        static ClientConfigObject()
        {
            try
            {
                // read default address
                DefaultWebApiAddress = ConfigurationSettings.AppSettings["serverAddress"].Trim(' ', '/');
            }
            catch
            {
                // ignored
            }
        }

        public ClientConfigObject()
        {
#if UseWebApi
            UseWebApiForMetadata = true;
            WebApiAddress = DefaultWebApiAddress;
#endif
        }
    }
}
