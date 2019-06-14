using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Drawing;
using System.Xml.Serialization;
using System.Drawing.Design;
namespace Accent.MapCore
{
    [Serializable()]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class AcntFont
    {
        private bool _bold;

        public bool Bold
        {
            get { return _bold; }
            set { _bold = value; }
        }
        private AcntColor _fontColor;
        [XmlElement]
        [Editor(typeof(MyColorEdotor), typeof(UITypeEditor))]
        public AcntColor FontColor
        {
            get { return _fontColor; }
            set { _fontColor = value; }
        }
        private bool _italic;

        public bool Italic
        {
            get { return _italic; }
            set { _italic = value; }
        }
        private string _name;
        [TypeConverter(typeof(enuInstalledFontsList))]
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        private double _size;

        public double Size
        {
            get { return _size; }
            set { _size = value; }
        }
        private bool _underLine;

        public bool UnderLine
        {
            get { return _underLine; }
            set { _underLine = value; }
        }
    
        public AcntFont()
        {
            this.Bold = false;
            this.FontColor = new AcntColor(0,0,0);
            this.Italic = false;
            this.Name = "Arial";
            this.Size = 12;
            this.UnderLine = false;
        }

        public AcntFont(string _name, double _size)
        {
            this.Bold = false;
            this.FontColor = new AcntColor();
            this.Italic = false;
            this.Name = _name;
            this.Size = _size;
            this.UnderLine = false;
        }

        public AcntFont(bool _bold, bool _italic, bool _underline, AcntColor _color)
        {
            this.Bold = _bold;
            this.FontColor = _color;
            this.Italic = _italic;
            this.Name = _name;
            this.Size = _size;
            this.UnderLine = _underline;
        }

        public AcntFont(bool _bold, AcntColor _color, bool _italic, string _name, double _size, bool _underline)
        {
            this.Bold = _bold;
            this.FontColor = _color;
            this.Italic = _italic;
            this.Name = _name;
            this.Size = _size;
            this.UnderLine = _underline;
        }

        public override string ToString()
        {
            string res = Name + " Size:" + Size.ToString();
            if (Bold) res = res + " Bold";
            if (Italic) res = res + " Italic";
            if (UnderLine) res = res + " Underline";

            return res;
        }
    }

  
}
