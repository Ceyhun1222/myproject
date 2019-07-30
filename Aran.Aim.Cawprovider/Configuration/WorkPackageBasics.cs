using Aran.Aim.AixmMessage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Aran.Aim.CAWProvider.Configuration
{
    public class WorkPackageBasics : AbstractResponse
    {
        public WorkPackageBasics ()
        {
            ApplicableOperations = new List<string> ();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
        public DateTime EffectiveDate { get; set; }
        public bool OperationPending { get; set; }
        public List<string> ApplicableOperations { get; private set; }

        public override void ReadXml (XmlReader reader)
        {
            ReaderHelper rh = new ReaderHelper (reader, 2);
            rh.ElementReading += new ElementReadingHandle (ElementReading);
            rh.Read ();
        }

        protected virtual bool ElementReading (XmlReader reader, int depth)
        {
            if (depth == 1)
                return (reader.LocalName == GetType ().Name);

            switch (reader.LocalName)
            {
                case "id":
                    Id = int.Parse (reader.ReadString ());
                    break;
                case "name":
                    Name = reader.ReadString ();
                    break;
                case "status":
                    Status = reader.ReadString ();
                    break;
                case "effectiveDate":
                    EffectiveDate = CommonXmlReader.GetDateTime (reader.ReadString ());
                    break;
                case "operationPending":
                    OperationPending = bool.Parse (reader.ReadString ());
                    break;
                case "applicableOperations":
                    ApplicableOperations.Add (reader.ReadString ());
                    break;
            }
            return true;
        }
    }
}
