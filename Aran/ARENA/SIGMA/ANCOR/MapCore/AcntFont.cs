using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Drawing;
using System.Xml.Serialization;
using System.Drawing.Design;
namespace ANCOR.MapCore
{
    [Serializable()]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class AncorFont : AbstractChartClass
    {
        private bool _bold;
        public bool Bold
        {
            get { return _bold; }
            set { _bold = value; }
        }

        private AncorColor _fontColor;
        //[XmlElement]
        [Editor(typeof(SigmaColorEdotor), typeof(UITypeEditor))]
        public AncorColor FontColor
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
    
        public AncorFont()
        {
           
        }

        public AncorFont(string _name, double _size)
        {
            
            this.Bold = false;
            this.FontColor = new AncorColor (0,0,0);
            
            this.Italic = false;
            this.Name = _name;
            this.Size = _size;
            this.UnderLine = false;
        }

        public AncorFont(bool _bold, bool _italic, bool _underline, AncorColor _color)
        {
            this.Bold = _bold;
            this.FontColor = _color;
            this.Italic = _italic;
            this.Name = _name;
            this.Size = _size;
            this.UnderLine = _underline;
        }

        public AncorFont(bool _bold, AncorColor _color, bool _italic, string _name, double _size, bool _underline)
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

        public override object Clone()
        {
            AncorFont o = (AncorFont)base.Clone();
            o.FontColor = (AncorColor)o.FontColor.Clone();
            o.Name = o.Name;
            o.Size = o.Size;
            return o;
        }
    }

  
}
