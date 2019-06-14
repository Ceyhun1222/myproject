using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace ANCOR.MapCore
{
    [Serializable()]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class AncorSymbol : AbstractChartClass
    {
        private string _text;

        public string Text
        {
            get { return _text; }
            set { _text = value; }
        }
        private AncorFont _textFont;

        public AncorFont TextFont
        {
            get { return _textFont; }
            set { _textFont = value; }
        }

        public AncorSymbol()
        {
        }

        public AncorSymbol(string _text)
        {
            this.Text = _text;
            this.TextFont = new AncorFont { Bold = false, FontColor = new AncorColor(0, 0, 0), Italic = false, Name = "Courier New", Size = 8, UnderLine = false };
        }

        public AncorSymbol(string _text, AncorFont _font)
        {
            this.Text = _text;
            this.TextFont = _font;
        }

        public override string ToString()
        {
            string AnnoTXT = this.Text;

            try
            {
                if (this.Text.Length > 0)
                {
                    AnnoTXT = "<CLR red=\"" + this.TextFont.FontColor.Red + "\"" + " green=\"" + this.TextFont.FontColor.Green + "\"" + " blue=\"" + this.TextFont.FontColor.Blue + "\"" + ">" + this.Text + "</CLR>";

                    AnnoTXT = "<FNT name=\"" + this.TextFont.Name + "\" size =\"" + this.TextFont.Size.ToString() + "\">" + AnnoTXT + "</FNT>";

                    if (this.TextFont.Bold) AnnoTXT = "<BOL>" + AnnoTXT + "</BOL>";
                    if (this.TextFont.Italic) AnnoTXT = "<ITA>" + AnnoTXT + "</ITA>";
                    if (this.TextFont.UnderLine) AnnoTXT = "<UND>" + AnnoTXT + "</UND>";


                }
            }
            catch
            { AnnoTXT = ""; }

            return AnnoTXT;
        }

        public override object Clone()
        {
            AncorSymbol obj = (AncorSymbol)base.Clone();
            obj.TextFont = (AncorFont)obj.TextFont.Clone();
            return obj;
        }
    }

}
