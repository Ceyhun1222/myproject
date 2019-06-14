using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Aran.Aim.CAWProvider
{
    public static class XmlWriterAimdbExtension
    {
        public static void WriteStartElement (this XmlWriter writer, NamespaceInfo nsInfo, string localName)
        {
            writer.WriteStartElement (nsInfo.Prefix, localName, nsInfo.Namespace);
        }

        public static void WriteElementString (this XmlWriter writer, NamespaceInfo nsInfo, string localName, string value)
        {
            writer.WriteElementString (nsInfo.Prefix, localName, nsInfo.Namespace, value);
        }

        public static void WriteAttributeString (this XmlWriter writer, NamespaceInfo nsInfo, string localName, string value)
        {
            writer.WriteAttributeString (nsInfo.Prefix, localName, nsInfo.Namespace, value);
        }

        public static void WriteQualifiedName(this XmlWriter writer, NamespaceInfo nsInfo)
        {
            writer.WriteQualifiedName(nsInfo.Prefix, nsInfo.Namespace);
        }

        
    }
}
