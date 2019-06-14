using System.Configuration;
using AIMSLServiceClient.Services;
using Aran.Temporality.Common.Enum;

namespace Aran.Temporality.Internal.Config
{
    public class ServerSettings: ConfigurationSection
    {
        public static ServerSettings Settings { get; } = ConfigurationManager.GetSection(nameof(ServerSettings)) as ServerSettings;

        [ConfigurationProperty("system"
            , DefaultValue = SystemType.Production
            , IsRequired = false)]
        public SystemType System
        {
            get => (SystemType)this["system"];
            set => this["system"] = value;
        }
    }
}
