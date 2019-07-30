using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Aran.Aim.CAWProvider
{
    public delegate bool ElementReadingHandle (XmlReader reader, int depth);

    public class ReaderHelper
    {
        public ReaderHelper (XmlReader reader, int eventDepth)
        {
            Reader = reader;
            _eventDepth = eventDepth;
        }

        public void Read ()
        {
            if (ElementReading == null)
                return;

            int depth = Reader.Depth;

            while (Reader.Read ())
            {
                if (Reader.NodeType == XmlNodeType.Element)
                {
                    if (depth >= Reader.Depth - _eventDepth)
                    {
                        bool r = ElementReading (Reader, Reader.Depth - depth);
                        if (!r)
                            return;
                    }
                }
                else if (Reader.NodeType == XmlNodeType.EndElement &&
                    Reader.Depth == depth)
                {
                    break;
                }
            }
        }

        public event ElementReadingHandle ElementReading;
        public XmlReader Reader { get; private set; }
        private int _eventDepth;
    }
}
