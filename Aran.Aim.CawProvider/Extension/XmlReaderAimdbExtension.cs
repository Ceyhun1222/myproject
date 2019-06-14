using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Aran.Aim.CAWProvider
{
    public static class XmlReaderAimdbExtension
    {
        public static string GetAttribute (this XmlReader reader, NamespaceInfo nsInfo, string localName)
        {
            return reader.GetAttribute (localName, nsInfo.Namespace);
        }
    }
}
