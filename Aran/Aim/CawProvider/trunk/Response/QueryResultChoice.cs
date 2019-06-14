using Aran.Aim.AixmMessage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Aran.Aim.CAWProvider
{
    public class QueryResultChoice : AbstractResponse
    {
        public QueryResultChoiceType Choice { get; private set; }

        public string AixmString
        {
            get { return (string) _value; }
            set { SetValue (value, QueryResultChoiceType.AixmString); }
        }

        public AixmBasicMessage AixmBasicMessage
        {
            get { return (AixmBasicMessage) _value; }
            set { SetValue (value, QueryResultChoiceType.AixmBasicMessage); }
        }

        public NotImplementedObject Aixm45Content
        {
            get { return (NotImplementedObject) _value; }
            set { SetValue (value, QueryResultChoiceType.Aixm45Content); }
        }

        public string NonAixmString
        {
            get { return (string) _value; }
            set { SetValue (value, QueryResultChoiceType.NonAixmString); }
        }

        public NotImplementedObject FeatureCodeIdentifierMap
        {
            get { return (NotImplementedObject) _value; }
            set { SetValue (value, QueryResultChoiceType.FeatureCodeIdentifierMap); }
        }

        public NotImplementedObject Groups
        {
            get { return (NotImplementedObject) _value; }
            set { SetValue (value, QueryResultChoiceType.Groups); }
        }

        public NotImplementedObject DeviationTimesliceReferenceMap
        {
            get { return (NotImplementedObject) _value; }
            set { SetValue (value, QueryResultChoiceType.DeviationTimesliceReferenceMap); }
        }

        public string ExportURI
        {
            get { return (string) _value; }
            set { SetValue (value, QueryResultChoiceType.ExportURI); }
        }

        public override void ReadXml (XmlReader reader)
        {
            string localName = reader.LocalName;

            if (localName == "AixmString")
            {
            }
            else if (localName == "AIXMBasicMessage")
            {
                AixmBasicMessage abm = new AixmBasicMessage (MessageReceiverType.Cadas);
                abm.ReadXml (reader);
                AixmBasicMessage = abm;
            }
            else if (localName == "Aixm4.5Content")
            {
            }
            else if (localName == "NonAixmString")
            {
            }
            else if (localName == "FeatureCodeIdentifierMap")
            {
            }
            else if (localName == "Groups")
            {
            }
            else if (localName == "DeviationTimesliceReferenceMap")
            {
            }
            else if (localName == "exportURI")
            {
            }
        }


        public static QueryResultChoice CreateFromXml (XmlReader reader)
        {
            QueryResultChoice qrc = new QueryResultChoice ();
            qrc.ReadXml (reader);
            
            if (qrc._value == null)
                return null;

            return qrc;
        }


        private void SetValue (object value, QueryResultChoiceType choiceType)
        {
            _value = value;
            Choice = choiceType;
        }

        private object _value;
    }

    public enum QueryResultChoiceType
    {
        AixmString,
        AixmBasicMessage,
        Aixm45Content,
        NonAixmString,
        FeatureCodeIdentifierMap,
        Groups,
        DeviationTimesliceReferenceMap,
        ExportURI
    }
}
