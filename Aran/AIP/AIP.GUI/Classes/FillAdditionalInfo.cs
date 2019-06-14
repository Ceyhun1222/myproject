using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AIP.DB;

namespace AIP.GUI.Classes
{
    public class AdditionalInfo
    {
        public bool? runXmlGeneration;
        public List<SectionName> Sections;

        public AdditionalInfo(bool? _xmlGenerate, List<SectionName> _sections)
        {
            runXmlGeneration = _xmlGenerate;
            Sections = _sections;
        }
    }
}
