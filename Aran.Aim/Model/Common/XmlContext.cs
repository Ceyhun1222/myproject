using System.Xml;

namespace Aran.Aixm
{
    public struct XmlContext
    {
        public XmlContext (XmlElement element)
        {
            this.Element = element;
            ElementIndex = new IndexIterval ();
            ElementIndex.End = -1;
            PropertyIndex = new IndexIterval ();
            PropertyIndex.End = -1;
        }

        public XmlElement Element;
        public IndexIterval ElementIndex;
        public IndexIterval PropertyIndex;
    }

    public struct IndexIterval
    {
        public int Start;
        public int End;
    }
}
