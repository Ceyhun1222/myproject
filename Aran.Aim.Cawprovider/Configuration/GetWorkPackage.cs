using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Aran.Aim.AixmMessage;

namespace Aran.Aim.CAWProvider.Configuration
{
    public class GetWorkPackage : BaseRequest
    {
        public GetWorkPackage ()
        {
            WorkPackageIdList = new List<int> ();
        }

        public List<int> WorkPackageIdList { get; private set; }

        public override void WriteXml (XmlWriter writer)
        {
            writer.WriteStartElement(CadasNamespaces.SDP, "GetWorkPackage");

            foreach (int workPackageId in WorkPackageIdList)
            {
                writer.WriteElementString(CadasNamespaces.SDP, "workPackageId", workPackageId.ToString());
            }

            writer.WriteEndElement ();
        }
    }
}
