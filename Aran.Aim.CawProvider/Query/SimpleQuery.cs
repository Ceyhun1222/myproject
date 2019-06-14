using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using Aran.Aim;
using Aran.Aim.AixmMessage;

namespace Aran.Aim.CAWProvider
{
    public class SimpleQuery : AbstractAixmQuery
    {
        public SimpleQuery ()
        {
            IdentifierList = new List<Guid> ();
        }

        public FeatureType FeatureType { get; set; }

        public List<Guid> IdentifierList { get; private set; }
        
        public InterpretationType Interpretation { get; set; }
        
        public TemporalTimeslice TemproalTimeslice { get; set; }
        
        public DataTypes.TimePeriod InsertionPeriod { get; set; }

        public DateTime? TechnicalTime { get; set; }

        public Filter Filter { get; set; }

        public override void WriteXml (XmlWriter writer)
        {
            //--- Begin SimpleQuery.
            writer.WriteStartElement(CadasNamespaces.CAW, "SimpleQuery");
            writer.WriteAttributeString ("outputFormat", "xml/aixm");
            writer.WriteElementString(CadasNamespaces.CAW, "featureType", FeatureType.ToString());

            foreach (Guid identifier in IdentifierList)
            {
                writer.WriteStartElement (AimDbNamespaces.GML, "identifier");
                writer.WriteAttributeString("codeSpace", "http://www.comsoft.aero/cadas-aimdb/caw");
                writer.WriteString (CommonXmlWriter.GetString (identifier));
                writer.WriteEndElement ();
            }

            writer.WriteElementString (AimDbNamespaces.AIXM51, "interpretation", Interpretation.ToString ());

            if (TemproalTimeslice != null)
            {
                TemproalTimeslice.WriteXml (writer);
            }

            if (InsertionPeriod != null)
            {
                CommonXmlWriter.WriteElement(InsertionPeriod, writer, CadasNamespaces.CAW, "insertionTime");
            }

            if (TechnicalTime != null)
            {
                writer.WriteElementString(CadasNamespaces.CAW, "technicalTime", CommonXmlWriter.GetString(TechnicalTime.Value));
            }

            if (_propertyNames != null)
            {
                foreach (string propertyName in _propertyNames)
                {
                    writer.WriteElementString(CadasNamespaces.CAW, "PropertyName", propertyName);
                }
            }

            if (Filter != null)
                Filter.WriteXml (writer);

            //--- End SimpleQuery.
            writer.WriteEndElement ();
        }

        public void SetProperties<TEnum> (params TEnum [] propEnumItems)
        {
            InitPropertyNames ();

            AimClassInfo classInfo = AimMetadata.GetClassInfoByIndex ((int) FeatureType);

            foreach (object enumItem in propEnumItems)
            {
                AimPropInfo propInfo = classInfo.Properties [enumItem.ToString ()];
                if (propInfo != null)
                    _propertyNames.Add (propInfo.AixmName);
            }
        }

        public void SetPropertyNames (IEnumerable<string> propList)
        {
            InitPropertyNames ();

            var classInfo = AimMetadata.GetClassInfoByIndex ((int) FeatureType);
            foreach (var propInfo in classInfo.Properties)
            {
                foreach (string propName in propList)
                {
                    if (propName.ToLower () == propInfo.Name.ToLower ())
                    {
                        _propertyNames.Add (propInfo.AixmName);
                        break;
                    }
                }
            }
        }

        private void InitPropertyNames ()
        {
            if (_propertyNames == null)
            {
                _propertyNames = new List<string> ();
                _propertyNames.AddRange (new string [] {
                    "identifier",
                    "validTime",
                    "interpretation",
                    "sequenceNumber",
                    "correctionNumber",
                    "featureLifetime"});
            }
        }

        private List<string> _propertyNames;
    }
}
