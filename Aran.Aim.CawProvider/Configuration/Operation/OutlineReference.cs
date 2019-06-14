using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Aran.Aim.CAWProvider.Configuration
{
    public class OutlineReference : AbstractRequest
    {
        public OutlineReference()
        {
            EadPublicSlotId = new List<int>();
            WorkPackageId = new List<int>();
        }

        public List<int> EadPublicSlotId { get; private set; }

        public List<int> WorkPackageId { get; private set; }

        public override void WriteXml(XmlWriter writer)
        {
#warning Implement OutlineReference.WriteXml
        }
    }
}
