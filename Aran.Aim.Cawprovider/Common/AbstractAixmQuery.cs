using System.Xml;

namespace Aran.Aim.CAWProvider
{
    public abstract class AbstractAixmQuery : AbstractRequest
    {
        public AbstractAixmQuery ()
        {
            FeatureVersion = SupportedFeatureVersionType.Aixm50;
            OutputFormatType = OutputFormatType.TextXmlAixm;
        }

        public SupportedFeatureVersionType FeatureVersion { get; set; }
        
        public OutputFormatType OutputFormatType { get; set; }

        public override void WriteXml (XmlWriter writer)
        {
        }
    }
}
