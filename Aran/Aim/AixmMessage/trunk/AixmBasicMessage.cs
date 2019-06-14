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
    public class AixmBasicMessage : AbstractXmlSerializable
    {
        private MessageReceiverType _messageReceiverType;
        private NamespaceInfo _caw;
        private NamespaceInfo _cae;
        private string _identifierCodeSpace;


        public AixmBasicMessage(MessageReceiverType messageReceiverType, SrsNameType srsType = SrsNameType.EPSG_4326)
        {
            HasMember = new ObservableCollection<AixmFeatureList>();
            HasMember.CollectionChanged += HasMember_CollectionChanged;
            WriteExtension = true;

            MessageReceiverType = messageReceiverType;
            SrsType = srsType;
            ErrorInfoList = new List<DeserializedErrorInfo>();
        }
        
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

        public ObservableCollection<AixmFeatureList> HasMember { get; private set; }

        public List<DeserializedErrorInfo> ErrorInfoList { get; private set; }

        public void ReadXmlAndNotify(XmlReader reader, Action<AixmFeatureList,ObservableCollection<AixmFeatureList>> onNextMember = null)
        {
            ErrorInfoList.Clear();

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

                                if (afl.ErrorInfoList.Count > 0)
                                    ErrorInfoList.AddRange(afl.ErrorInfoList);

                                HasMember.Add(afl);

                                if (onNextMember != null)
                                    onNextMember(afl, HasMember);
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

        public override void ReadXml (XmlReader reader)
        {
            ReadXmlAndNotify(reader);
        }

        public override void WriteXml (XmlWriter writer)
        {
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
                    {
                        MessageMetadata_WriteXml (writer);
                    }
                    writer.WriteEndElement ();
                }

                try
                {
                    //This form remov object reference for GC remove object
                    AixmFeatureList afl;
                    for (int i = 0; i < this.HasMember.Count; i++)
                    {
                        afl = this.HasMember[i];
                        writer.WriteStartElement(AimDbNamespaces.AIXM51Message, "hasMember");
                        {
                            afl.WriteXml(writer, WriteExtension, Write3DCoordinateIfExists, SrsType);
                        }
                        writer.WriteEndElement();
                        this.HasMember[i] = null;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            writer.WriteEndElement ();
        }

        private void MessageMetadata_ReadXml (XmlReader reader)
        {
            var xmlText = reader.ReadInnerXml ();
        }

        private void MessageMetadata_WriteXml (XmlWriter writer)
        {

        }

        private void HasMember_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add) {
                foreach (AixmFeatureList afl in e.NewItems) {
                    afl.CAW = _caw;
                    afl.CAE = _cae;
                    afl.IdentifierCodeSpace = _identifierCodeSpace;
                }
            }
        }
    }

    public enum MessageReceiverType { Panda, Cadas, Eurocontrol }

    public enum SrsNameType
    {
        CRS84,
        EPSG_4326
    }
}
