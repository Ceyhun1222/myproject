using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Aran.Aim.AixmMessage;

namespace Aran.Aim.CAWProvider.Configuration
{
    public class GetConfiguration : BaseRequest
    {
        public override void WriteXml (XmlWriter writer)
        {
            writer.WriteElementString(CadasNamespaces.SDP, "GetConfiguration", null);
        }
    }
}
