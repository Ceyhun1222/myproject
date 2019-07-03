using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Xml.Schema;
using System.Xml;
using Aran.Aim.AixmMessage;

namespace Aran.Aim.CAWProvider
{
    public abstract class BaseResponse : AbstractResponse
    {
        public DateTime TimeStamp { get; set; }

        public override void ReadXml (XmlReader reader)
        {
            //Comsoft bug.
            string timeStampValue = reader.GetAttribute ("timestamp");
            
            if (timeStampValue == null)
                timeStampValue = reader.GetAttribute ("timeStamp");

            TimeStamp = CommonXmlReader.GetDateTime (timeStampValue);
        }
    }
}
