using System;
using System.Xml;

namespace Aran.Aixm
{
    public interface IAixmSerializable
    {
        bool AixmDeserialize (XmlContext context);
    }
}
