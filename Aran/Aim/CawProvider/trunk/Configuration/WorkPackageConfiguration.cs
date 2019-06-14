using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Aran.Aim.CAWProvider.Configuration
{
    public class WorkPackageConfiguration : AbstractResponse
    {
        public WorkPackageConfiguration ()
        {
            InitialOperation = new List<string> ();
            Operations = new List<string> ();
            States = new List<string> ();
        }

        public List<string> InitialOperation { get; private set; }
        public List<string> Operations { get; private set; }
        public List<string> States { get; private set; }

        public override void ReadXml (XmlReader reader)
        {
            int depth = reader.Depth;

            while (reader.Read ())
            {
                if (reader.NodeType == XmlNodeType.Element)
                {
                    switch (reader.LocalName)
                    {
                        case "initialOperation":
                            InitialOperation.Add (reader.ReadString ());
                            break;
                        case "operations":
                            Operations.Add (reader.ReadString ());
                            break;
                        case "states":
                            States.Add (reader.ReadString ());
                            break;
                    }
                }
                else if (reader.NodeType == XmlNodeType.EndElement &&
                    reader.Depth == depth)
                {
                    break;
                }
            }
        }
    }
}
