using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Aran.Aim.Features;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace Aran.Aim.AixmMessage
{
    public class AixmBasicMessageForward : AbstractXmlSerializable
    {
        private MessageReceiverType _messageReceiverType;
        private NamespaceInfo _caw;
        private NamespaceInfo _cae;
        private string _identifierCodeSpace;
        private IEnumerable<AixmFeatureList> _hasMember;


        public AixmBasicMessageForward(MessageReceiverType messageReceiverType, SrsNameType srsType = SrsNameType.EPSG_4326)
        {
            WriteExtension = true;

            MessageReceiverType = messageReceiverType;
            SrsType = srsType;
        }

        public BasicMessageHasMemberReadHandler HasMemberReaded;
        public BasicMessageHasMemberWriteHandler HasMemberWrited;
        
        public MessageReceiverType MessageReceiverType
        {
            get { return _messageReceiverType; }
            private set
            {
                _messageReceiverType = value;

                _caw = new NamespaceInfo() { Prefix = "caw", Namespace = "http://www.pandanavigation.com/aran-aimdb" };
                _cae = new NamespaceInfo() { Prefix = "cae", Namespace = "http://www.pandanavigation.com/aran-aimdb/extension" };
                //_identifierCodeSpace = "http://www.pandanavigation.com/aran-aimdb";
                _identifierCodeSpace = "urn:uuid:";

                switch (_messageReceiverType) {
                    case AixmMessage.MessageReceiverType.Cadas:
                        _caw = new NamespaceInfo() { Prefix = "caw", Namespace = "http://www.comsoft.aero/cadas-aimdb/caw" };
                        _cae = new NamespaceInfo() { Prefix = "cae", Namespace = "http://www.comsoft.aero/cadas-aimdb/extension" };
                        //_identifierCodeSpace = "http://www.comsoft.aero/cadas-aimdb/caw";
                        _identifierCodeSpace = "urn:uuid:";
                        break;
                    case AixmMessage.MessageReceiverType.Eurocontrol:
                        _identifierCodeSpace = "urn:uuid:";
                        break;
                }
            }
        }

        public SrsNameType SrsType { get; private set; }

        public bool WriteExtension { get; set; }

        public bool Write3DCoordinateIfExists { get; set; }

        public DateTime? EffectiveDate { get; set; }

        public Aran.Aim.Metadata.MessageMetadata MessageMetadata { get; set; }

        public void SetHasMember(IEnumerable<AixmFeatureList> hasMember)
        {
            _hasMember = hasMember;
        }

        public override void ReadXml (XmlReader reader)
        {
            if (HasMemberReaded == null)
                throw new ArgumentNullException(nameof(HasMemberReaded));

            int depth = reader.Depth;

            //--- must be derived from AbstractAIXMMessage class.

            bool firstTime = true;

            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element)
                {
                    if (firstTime && reader.LocalName == "messageMetadata")
                    {
                        firstTime = false;

                        MessageMetadata = new Metadata.MessageMetadata();
                        MessageMetadata_ReadXml(reader);
                    }
                    else if (reader.LocalName == "hasMember")
                    {
                        while (reader.Read())
                        {
                            if (reader.NodeType == XmlNodeType.Element)
                            {
                                var afl = new AixmFeatureList();
                                afl.ReadXml(reader);

                                var eventArgs = new BasicMessageHasMemberReadEventArgs { Features = afl };
                                HasMemberReaded(this, eventArgs);

                                if (eventArgs.IsStop)
                                    return;
                            }
                            else if (reader.NodeType == XmlNodeType.EndElement)
                            {
                                break;
                            }
                        }
                    }
                }
                else if (reader.NodeType == XmlNodeType.EndElement &&
                    depth == reader.Depth)
                {
                    break;
                }
            }
        }

        public override void WriteXml (XmlWriter writer)
        {
            WriteCount = 0;

            if (_hasMember == null && HasMemberWrited == null)
                throw new ArgumentNullException("HasMember Or HasMemberWrited");

            if (EffectiveDate != null)
                writer.WriteComment(string.Format("effective-date: {0}", EffectiveDate.Value.ToString("yyyy-MM-dd")));

            writer.WriteStartElement (AimDbNamespaces.AIXM51Message, "AIXMBasicMessage");
            {
                writer.WriteAttributeString(AimDbNamespaces.XSI, "schemaLocation",
                    "http://www.aixm.aero/schema/5.1/message http://www.aixm.aero/schema/5.1/message/AIXM_BasicMessage.xsd");
                writer.WriteAttributeString (AimDbNamespaces.GML, "id", AranAixmWriter.GenerateNewGmlId ());
                writer.WriteAttributeString("xmlns", AimDbNamespaces.AIXM51.Prefix, null, AimDbNamespaces.AIXM51.Namespace);

                if (MessageMetadata != null)
                {
                    writer.WriteStartElement (AimDbNamespaces.AIXM51, "messageMetadata");
                    MessageMetadata_WriteXml (writer);
                    writer.WriteEndElement ();
                }

                try
                {
                    if (_hasMember != null)
                    {
                        foreach(var hm in _hasMember)
                        {
                            hm.CAW = _caw;
                            hm.CAE = _cae;
                            hm.IdentifierCodeSpace = _identifierCodeSpace;

                            writer.WriteStartElement(AimDbNamespaces.AIXM51Message, "hasMember");
                            hm.WriteXml(writer, WriteExtension, Write3DCoordinateIfExists, SrsType);
                            writer.WriteEndElement();

                            WriteCount++;
                        }
                    }
                    else
                    {
                        while (true)
                        {
                            var hasMemberEventArg = new BasicMessageHasMemberWriteEventArgs();
                            HasMemberWrited?.Invoke(this, hasMemberEventArg);

                            if (hasMemberEventArg.Features == null || hasMemberEventArg.Features.Count == 0)
                                break;

                            hasMemberEventArg.Features.CAW = _caw;
                            hasMemberEventArg.Features.CAE = _cae;
                            hasMemberEventArg.Features.IdentifierCodeSpace = _identifierCodeSpace;

                            writer.WriteStartElement(AimDbNamespaces.AIXM51Message, "hasMember");
                            hasMemberEventArg.Features.WriteXml(writer, WriteExtension, Write3DCoordinateIfExists, SrsType);
                            writer.WriteEndElement();

                            WriteCount++;
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            writer.WriteEndElement ();

            _hasMember = null;
        }

        public int WriteCount { get; private set; }

        private void MessageMetadata_ReadXml (XmlReader reader)
        {
            var xmlText = reader.ReadInnerXml ();
        }

        private void MessageMetadata_WriteXml (XmlWriter writer)
        {

        }
    }

    public delegate void BasicMessageHasMemberReadHandler(object sender, BasicMessageHasMemberReadEventArgs e);
    public delegate void BasicMessageHasMemberWriteHandler(object sender, BasicMessageHasMemberWriteEventArgs e);

    public class BasicMessageHasMemberReadEventArgs : EventArgs
    {
        public AixmFeatureList Features { get; set; }
        public bool IsStop { get; set; }
    }

    public class BasicMessageHasMemberWriteEventArgs : EventArgs
    {
        public AixmFeatureList Features { get; set; }
    }
}
