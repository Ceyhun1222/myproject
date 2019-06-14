using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace AIMSLServiceClient.Services.Encoder
{
    public class UnsecureMessageEncoder:MessageEncoder
    {
        private readonly UnsecureMessageEncoderFactory _factory;
        private readonly XmlWriterSettings _writerSettings;
        private string contentType;
        private byte[] key;

        public UnsecureMessageEncoder (UnsecureMessageEncoderFactory factory)
        {
            this._factory = factory;

            this._writerSettings = new XmlWriterSettings();
        }

        public override string ContentType => "text/xml";

        public override string MediaType => "text/xml";

        public override MessageVersion MessageVersion => this._factory.MessageVersion;

        public override Message ReadMessage(ArraySegment<byte> buffer, BufferManager bufferManager, string contentType)
        {
            byte[] msgContents = new byte[buffer.Count];
            Array.Copy(buffer.Array, buffer.Offset, msgContents, 0, msgContents.Length);
            bufferManager.ReturnBuffer(buffer.Array);

            MemoryStream stream = new MemoryStream(msgContents);
            return ReadMessage(stream, int.MaxValue);
        }

        public override Message ReadMessage(Stream stream, int maxSizeOfHeaders, string contentType)
        {
            var sr = new StreamReader(stream);
            var wireResponse = sr.ReadToEnd();

            var logicalResponse = DeleteSeucrityNode(wireResponse);

            XmlReader reader = XmlReader.Create(new StringReader(logicalResponse));
            return Message.CreateMessage(reader, maxSizeOfHeaders, MessageVersion.Soap11);

            //XmlReader reader = XmlReader.Create(stream);
            //return Message.CreateMessage(reader, maxSizeOfHeaders, this.MessageVersion);
        }


        private string DeleteSeucrityNode(string message)
        {
            var doc = new XmlDocument();
            doc.LoadXml(message);
            XmlNamespaceManager nsManager = new XmlNamespaceManager(doc.NameTable);
            nsManager.AddNamespace("s", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd");
            XmlNode node = null;
            do
            {
                node = doc.SelectSingleNode("//s:Security", nsManager);
                node?.ParentNode?.RemoveChild(node);
            } while (node != null);
            

            using (var stringWriter = new StringWriter())
            using (var xmlTextWriter = XmlWriter.Create(stringWriter))
            {
                doc.WriteTo(xmlTextWriter);
                xmlTextWriter.Flush();
                return stringWriter.GetStringBuilder().ToString();
            }
        }


        public override ArraySegment<byte> WriteMessage(Message message, int maxMessageSize, BufferManager bufferManager, int messageOffset)
        {
            MemoryStream stream = new MemoryStream();
            XmlWriter writer = XmlWriter.Create(stream, this._writerSettings);
            message.WriteMessage(writer);
            writer.Close();

            byte[] messageBytes = stream.GetBuffer();
            int messageLength = (int)stream.Position;
            stream.Close();

            int totalLength = messageLength + messageOffset;
            byte[] totalBytes = bufferManager.TakeBuffer(totalLength);
            Array.Copy(messageBytes, 0, totalBytes, messageOffset, messageLength);

            ArraySegment<byte> byteArray = new ArraySegment<byte>(totalBytes, messageOffset, messageLength);
            return byteArray;
        }


        public override void WriteMessage(Message message, Stream stream)
        {
            XmlWriter writer = XmlWriter.Create(stream, this._writerSettings);
            message.WriteMessage(writer);
            writer.Close();
        }
    }

}
