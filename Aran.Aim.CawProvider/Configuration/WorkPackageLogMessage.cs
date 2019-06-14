using Aran.Aim.AixmMessage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Aran.Aim.CAWProvider.Configuration
{
    public class WorkPackageLogMessage : AbstractResponse
    {
        public DateTime CreationTime { get; set; }
        public LogMessageCategoryType Category { get; set; }
        public string Message { get; set; }

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
                case "creationTime":
                    CreationTime = CommonXmlReader.GetDateTime (reader.ReadString ());
                    break;
                case "category":
                    Category = (LogMessageCategoryType) Enum.Parse (typeof (LogMessageCategoryType), reader.ReadString ());
                    break;
                case "message":
                    Message = reader.ReadString ();
                    break;
            }
            return true;
        }
    }

    public enum LogMessageCategoryType
    {
        INFO,
        WARNING,
        ERROR,
        FATAL
    }
}
