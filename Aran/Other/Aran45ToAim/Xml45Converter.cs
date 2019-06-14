using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Aran45ToAixm
{
    public class Xml45Converter : ConverterToAixm51
    {
        private XmlDocument _xmlDoc;

        public Xml45Converter ()
        {
            _xmlDoc = new XmlDocument ();
        }

        public override void OpenFile (string fileName)
        {
            _xmlDoc.Load (fileName);
        }

        public override List<Aran.Aim.Features.Feature> ConvertFeature<TypeField> (List<string> errorList)
        {
            throw new NotImplementedException ();
        }

        public override List<Type> GetFeaturesList ()
        {
            var list = new List<Type> ();

            foreach (var type in Global.Supported45Features)
            {
                try
                {
                    var assocName = GetTypeAssocName (type);

                    var elem = _xmlDoc.DocumentElement.SelectSingleNode (assocName);
                    if (elem != null)
                        list.Add (type);
                }
                catch { }
            }

            return list;
        }

        private string GetTypeAssocName (Type type)
        {
            var name = Global.GetFeatureTypeName (type);

            if (name == "Airspace")
                return "Ase";
            if (name == "VerticalStructure")
                return "Obs";
            if (name == "DesignatedPoint")
                return "Dpn";

            return null;
        }

        public override List<List<Aran.Aim.Features.Feature>> PostConvert<TypeField> (List<Aran.Aim.Features.Feature> featList, List<string> errorList)
        {
            return null;
        }
    }
}
