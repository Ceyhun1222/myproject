using Aran.Aim.AixmMessage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Aran.Aim.CAWProvider.Configuration
{
    public class WorkPackageAttachmentBasics : AbstractResponse
    {
        public long Id { get; set;}
        public string UserName { get; set;}
        public DateTime CreationTime { get; set;}
        public string Description { get; set;}

        public override void ReadXml (XmlReader reader)
        {
            ReaderHelper rh = new ReaderHelper (reader, 1);
            rh.ElementReading += new ElementReadingHandle (ElementReading);
            rh.Read ();
        }

        private bool ElementReading (XmlReader reader, int depth)
        {
            switch (reader.LocalName)
            {
                case "id":
                    Id = long.Parse (reader.ReadString ());
                    break;
                case "username":
                    UserName = reader.ReadString ();
                    break;
                case "creationTime":
                    CreationTime = CommonXmlReader.GetDateTime (reader.ReadString ());
                    break;
                case "description":
                    Description = reader.ReadString ();
                    break;
            }
            return true;
        }
    }
}
