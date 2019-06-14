using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Accent.MapCore
{
    [Serializable()]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class AcntSymbol
    {
        private string _text;

        public string Text
        {
            get { return _text; }
            set { _text = value; }
        }
        private AcntFont _textFont;

        public AcntFont TextFont
        {
            get { return _textFont; }
            set { _textFont = value; }
        }

        public AcntSymbol()
        {
            this.Text = "";
            this.TextFont = new AcntFont();
        }

        public AcntSymbol(string _text)
        {
            this.Text = _text;
            this.TextFont = new AcntFont();
        }

        public AcntSymbol(string _text, AcntFont _font)
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
    }

}
