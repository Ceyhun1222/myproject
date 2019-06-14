using Aran.Temporality.Common.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace AIMSLServiceClient.Services.Formatter
{
    class UnsecureInspector: IClientMessageInspector
    {


        public object BeforeSendRequest(ref Message request, IClientChannel channel)
        {
            return request;
        }

        public void AfterReceiveReply(ref Message reply, object correlationState)
        {
            XmlDocument doc = new XmlDocument();
            MemoryStream ms = new MemoryStream();
            XmlWriter writer = XmlWriter.Create(ms);
            reply.WriteMessage(writer);
            writer.Flush();
            ms.Position = 0;
            doc.Load(ms);

            StringWriter stringWriter = new StringWriter();
            XmlTextWriter xmlTextWriter = new XmlTextWriter(stringWriter);

            doc.WriteTo(xmlTextWriter);

            

            //XmlNamespaceManager nsManager = new XmlNamespaceManager(doc.NameTable);
            //nsManager.AddNamespace("s", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd");
            //XmlNode node = doc.SelectSingleNode("//s:Security", nsManager);
             LogManager.GetLogger(typeof(UnsecureInspector)).Trace(stringWriter.ToString());
        }
    }
}
