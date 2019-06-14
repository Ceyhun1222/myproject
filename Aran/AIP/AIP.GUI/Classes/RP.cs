using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aran.Aim.Enums;

namespace AIP.GUI.Classes
{
    class RP
    {
        public string Title { get; set; }
        public List<RPHelper> RPHelperList { get; set; }
        public bool AutoGenerate { get; set; }

        public RP(string title, List<RPHelper> rpHelperList, bool autoGenerate = true)
        {
            Title = title;
            RPHelperList = rpHelperList;
            AutoGenerate = autoGenerate;
        }
    }

    public struct RPHelper
    {
        public CodeRuleProcedureTitle? RPTitle;
        public List<CodeRuleProcedure?> RPCategory;
        public string Title;

        public RPHelper(CodeRuleProcedureTitle p1, List<CodeRuleProcedure?> p2 = null, string p3 = null)
        {
            RPTitle = p1;
            RPCategory = p2;
            Title = p3;
        }
    }
    
}
