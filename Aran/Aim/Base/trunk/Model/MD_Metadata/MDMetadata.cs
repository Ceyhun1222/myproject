//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Xml;

//namespace Aran.Aim.Metadata
//{
//    public class MDMetadata
//    {
//        public string XmlText { get; set; }

//        public void ReadXml (XmlReader reader)
//        {
//            XmlText = reader.ReadInnerXml ();
//            XmlText = XmlText.Trim ();
//        }

//        public void WriterXml (XmlWriter writer)
//        {
//            if (XmlText == null)
//                return;

//            System.IO.MemoryStream ms = new System.IO.MemoryStream ();
//            byte [] ba = Encoding.UTF8.GetBytes (XmlText);
//            ms.Write (ba, 0, ba.Length);
//            ms.Seek (0, System.IO.SeekOrigin.Begin);

//            XmlReader reader = XmlReader.Create (ms);

//            writer.WriteNode (reader, true);

//            reader.Close ();
//            ms.Close ();
//        }
//    }
//}
