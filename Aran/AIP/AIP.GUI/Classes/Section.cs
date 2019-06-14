using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AIP.DB;

namespace AIP.GUI.Classes
{
    public static class CurrentSection
    {
        // Section name
        public static SectionName Name { get; set; }
        public static PathCategory PathCategory { get; set; }
        public static string TextName { get; set; }
        public static string Common { get; set; }
        public static string Class { get; set; }
        public static string HtmlArg { get; set; }
        public static List<string> PdfArg = new List<string>();

        public static void Clear()
        {
            Name = SectionName.None;
            PathCategory = PathCategory.eAIP;
            TextName = "";
            Common = "";
            Class = "";
            HtmlArg = "";
            PdfArg.Clear();
            Path.Clear();
        }

        public static class Path
        {
            public static string Xml { get; set; }
            public static string Html { get; set; }
            public static string Pdf { get; set; }

            public static void Clear()
            {
                Html = "";
                Xml = "";
                Pdf = "";
            }
        }
    }
}
