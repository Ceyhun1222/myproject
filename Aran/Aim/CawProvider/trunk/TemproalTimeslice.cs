using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using Aran.Package;
using Aran.Aim.AixmMessage;

namespace Aran.Aim.CAWProvider
{
    public class TemporalTimeslice :
        IXmlSerializable,
        IPackable
    {
        public TemporalTimeslice (DateTime effectiveDate)
        {
            EffectiveDate = effectiveDate;
        }

        public TemporalTimeslice (DataTypes.TimePeriod period)
        {
            Period = period;
        }

        public TemporalTimeslice (uint sequenceNumber)
        {
            SequenceNumber = sequenceNumber;
        }

        public TemproalTimesliceChoiceType Choice
        {
            get { return _choice; }
        }

        public DateTime EffectiveDate
        {
            get { return _effectiveDate; }
            set
            {
                _effectiveDate = value;
                _choice = TemproalTimesliceChoiceType.EffectiveDate;
            }
        }

        public DataTypes.TimePeriod Period
        {
            get { return _period; }
            set
            {
                _period = value;
                _choice = TemproalTimesliceChoiceType.Period;
            }
        }

        public uint SequenceNumber
        {
            get { return _sequenceNumber; }
            set
            {
                _sequenceNumber = value;
                _choice = TemproalTimesliceChoiceType.SequenceNumber;
            }
        }

        #region IXmlSerializable Members

        public XmlSchema GetSchema ()
        {
            return null;
        }

        public void ReadXml (XmlReader reader)
        {
        }

        public void WriteXml (XmlWriter writer)
        {
            switch (Choice)
            {
                case TemproalTimesliceChoiceType.EffectiveDate:
                    writer.WriteElementString(CadasNamespaces.CAW, "effectiveDate", CommonXmlWriter.GetString(EffectiveDate));
                    break;
                case TemproalTimesliceChoiceType.Period:
                    CommonXmlWriter.WriteElement(_period, writer, CadasNamespaces.CAW, "period");
                    break;
                case TemproalTimesliceChoiceType.SequenceNumber:
                    writer.WriteElementString (AimDbNamespaces.AIXM51, "sequenceNumber", SequenceNumber.ToString ());
                    break;
            }
        }

        #endregion

        #region IPackable Members

        public void Pack (PackageWriter writer)
        {
            writer.PutEnum<TemproalTimesliceChoiceType> (Choice);
            if (Choice == TemproalTimesliceChoiceType.EffectiveDate)
                writer.PutDateTime (EffectiveDate);
            else if (Choice == TemproalTimesliceChoiceType.Period)
                (Period as IPackable).Pack (writer);
            else
                writer.PutUInt32 (SequenceNumber);
        }

        public void Unpack (PackageReader reader)
        {
            _choice = reader.GetEnum<TemproalTimesliceChoiceType> ();
            if (_choice == TemproalTimesliceChoiceType.EffectiveDate)
                EffectiveDate = reader.GetDateTime ();
            else if (_choice == TemproalTimesliceChoiceType.Period)
                (Period as IPackable).Unpack (reader);
            else
                SequenceNumber = reader.GetUInt32 ();
        }

        #endregion

        private TemproalTimesliceChoiceType _choice;
        private DateTime _effectiveDate;
        private DataTypes.TimePeriod _period;
        private uint _sequenceNumber;
    }

    public class MQTemproalTimeclie : TemporalTimeslice
    {
        public MQTemproalTimeclie (DateTime effectiveDate)
            : base (effectiveDate)
        {
        }

        public MQTemproalTimeclie (DataTypes.TimePeriod period)
            : base (period)
        { 
        }
    }
}
