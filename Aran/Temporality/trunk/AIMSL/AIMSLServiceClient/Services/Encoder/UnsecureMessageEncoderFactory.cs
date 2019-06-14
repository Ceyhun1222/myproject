using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;

namespace AIMSLServiceClient.Services.Encoder
{
    public class UnsecureMessageEncoderFactory : MessageEncoderFactory
    {
        internal UnsecureMessageEncoderFactory()
        {
            this.Encoder = new UnsecureMessageEncoder(this);
        }

        public override MessageEncoder Encoder { get; }

        public override MessageVersion MessageVersion => MessageVersion.Soap11WSAddressing10;
    }
}
