using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Drawing;


namespace ANCOR.MapCore
{
    [Serializable()]
    //[TypeConverter(typeof(ExpandableObjectConverter))]
    [TypeConverter(typeof(PropertySorter))]
    public class AncorChartElementWord : AbstractChartClass
    {
        private AncorDataSource _dataSource;
       // [Browsable(false)]
        [PropertyOrder(200)]
        public AncorDataSource DataSource
        {
            get { return _dataSource; }
            set { _dataSource = value; }
        }

        private AncorSymbol _startSymbol;
        [PropertyOrder(10)]
        public AncorSymbol StartSymbol
        {
            get { return _startSymbol; }
            set { _startSymbol = value; }
        }
        
        private AncorSymbol _endSymbol;
        [PropertyOrder(20)]
        public AncorSymbol EndSymbol
        {
            get { return _endSymbol; }
            set { _endSymbol = value; }
        }
       
        private textCase _textCase;
        [Browsable(false)]
        [PropertyOrder(30)]
        public textCase TextCase
        {
            get { return _textCase; }
            set { _textCase = value; }
        }

        private string _textValue;
        //[ReadOnly(true)]
        [PropertyOrder(40)]
        public string TextValue
        {
            get { return _textValue; }
            set { _textValue = value; }
        }

        private AncorFont _font;
        [PropertyOrder(50)]
        public AncorFont Font
        {
            get { return _font; }
            set { _font = value; }
        }

        private double _characterSpacing;
        [Browsable(false)]
        [PropertyOrder(60)]
        public double CharacterSpacing
        {
            get { return _characterSpacing; }
            set { _characterSpacing = value; }
        }

        private double _characterWidth;
        [Browsable(false)]
        [PropertyOrder(70)]
        public double CharacterWidth
        {
            get { return _characterWidth; }
            set { _characterWidth = value; }
        }

        private double _leading;
        [Browsable(false)]
        [PropertyOrder(80)]
        public double Leading
        {
            get { return _leading; }
            set { _leading = value; }
        }

        private textPosition _textPosition;
        [PropertyOrder(90)]
        public textPosition TextPosition
        {
            get { return _textPosition; }
            set { _textPosition = value; }
        }

        private double _wordSpacing;
        [Browsable(false)]
        [PropertyOrder(100)]
        public double WordSpacing
        {
            get { return _wordSpacing; }
            set { _wordSpacing = value; }
        }

        private bool _morse;
        [PropertyOrder(110)]
        [Browsable(false)]
        public bool Morse
        {
            get { return _morse; }
            set { _morse = value; }
        }

        private bool _visible;
        [PropertyOrder(120)]
        public bool Visible
        {
            get { return _visible; }
            set { _visible = value; }
        }

        public AncorChartElementWord()
        {
        }

        public AncorChartElementWord(string text)
        {
            this.DataSource = new AncorDataSource { Condition = "", Link = "", Value = "" };
            this.EndSymbol = new AncorSymbol("");
            this.StartSymbol = new AncorSymbol("");
            this.TextCase = textCase.Normal;
            this.CharacterSpacing = 0;
            this.CharacterWidth = 0;
            this.Font = new AncorFont(false, new AncorColor(0, 0, 0), false, "Courier New", 8, false);
            this.Leading = 0;
            this.TextPosition = textPosition.Normal;
            this.WordSpacing = 0;
            this.TextValue = text;
            this.Visible = true;
            
        }

        public AncorChartElementWord(string text, AncorFont fnt)
        {
            this.DataSource = new AncorDataSource { Condition = "", Link = "",Value ="" };
            this.EndSymbol = new AncorSymbol("");
            this.StartSymbol = new AncorSymbol("");
            this.TextCase = textCase.Normal;
            this.CharacterSpacing = 0;
            this.CharacterWidth = 0;
            this.Font = new AncorFont(fnt.Bold, new AncorColor(0, 0, 0), fnt.Italic, fnt.Name, fnt.Size, fnt.UnderLine);
            this.Leading = 0;
            this.TextPosition = textPosition.Normal;
            this.WordSpacing = 0;
            this.TextValue = text;
            this.Visible = true;

        }


        public override string ToString()
        {
            string AnnoTXT ="";
            string startTXT = "";
            try
            {
                AnnoTXT = this.EndSymbol.Text.Length >0 ? this.TextValue.Trim() : this.TextValue;
                if (AnnoTXT.Trim().ToUpper().CompareTo("NAN") == 0) this.Font.FontColor = new AncorColor(255, 0, 0);

                startTXT = this.StartSymbol.ToString();

                FormatWord(ref AnnoTXT);

                AnnoTXT = startTXT + AnnoTXT;

                if (this.Morse)
                {

                    this.Font.Name = "Morse";
                    this.TextPosition = textPosition.Superscript;
                    string morseAnnoTXT = this.StartSymbol.ToString() + this.TextValue;
                    string Bufer = "  ";
                    for (int i = 0; i < morseAnnoTXT.Length - 1; i++)
                    {
                        Bufer = Bufer + " ";
                    }
                    //morseAnnoTXT = ToColumn(morseAnnoTXT, Bufer);

                    FormatWord(ref morseAnnoTXT);
                    //AnnoTXT = AnnoTXT + morseAnnoTXT;                    
                    AnnoTXT ="\r" + "\n" + morseAnnoTXT;


                }
                


            }
            catch
            { AnnoTXT = ""; }

            return AnnoTXT;
        }

  
        private void FormatWord(ref string AnnoTXT)
        {
            if (this.Font.Bold) AnnoTXT = "<BOL>" + AnnoTXT + "</BOL>";
            if (this.Font.Italic) AnnoTXT = "<ITA>" + AnnoTXT + "</ITA>";

            ///временная затычка для белорусии. убарть!!!!
            ///
            //this.Font.FontColor.AncorColor_CMYK = 100;


            AnnoTXT = "<CLR red=\"" + this.Font.FontColor.Red + "\"" + " green=\"" + this.Font.FontColor.Green + "\"" + " blue=\"" + this.Font.FontColor.Blue + "\"" + ">" + AnnoTXT + "</CLR>";

            AnnoTXT = "<FNT name=\"" + this.Font.Name + "\" size =\"" + this.Font.Size.ToString() + "\">" + AnnoTXT + "</FNT>";
            if (this.CharacterSpacing != 0) AnnoTXT = "<CHR spacing=\"" + this.CharacterSpacing + "\">" + AnnoTXT + "</CHR>";
            if (this.CharacterWidth != 0) AnnoTXT = "<CHR width=\"" + this.CharacterWidth + "\">" + AnnoTXT + "</CHR>";
            if (this.Leading != 0) AnnoTXT = "<LIN leading=\"" + this.Leading + "\">" + AnnoTXT + "</LIN>";
            if (this.TextCase == textCase.AllCaps) AnnoTXT = "<ACP>" + AnnoTXT + "</ACP>";
            if (this.TextCase == textCase.SmallCaps) AnnoTXT = "<SCP>" + AnnoTXT + "</SCP>";
            if (this.TextPosition == textPosition.Subscript) AnnoTXT = "<SUB>" + AnnoTXT + "</SUB>";
            if (this.TextPosition == textPosition.Superscript) AnnoTXT = "<SUP>" + AnnoTXT + "</SUP>";
            if (this.WordSpacing != 0) AnnoTXT = "<WRD spacing=\"" + this.WordSpacing + "\">" + AnnoTXT + "</WRD>";

            AnnoTXT =  AnnoTXT + this.EndSymbol.ToString();

            if (this.Font.UnderLine) AnnoTXT = "<UND>" + AnnoTXT + "</UND>";
           
        }


        public string ToMorse(string Txt)
        {
            System.Collections.Hashtable MorseTable = new System.Collections.Hashtable();

            MorseTable.Add("A", Char.ConvertFromUtf32(Convert.ToInt32("25CF", 16)) + " " + Char.ConvertFromUtf32(Convert.ToInt32("25AC", 16)));
            MorseTable.Add("B", Char.ConvertFromUtf32(Convert.ToInt32("25AC", 16)) + " " + Char.ConvertFromUtf32(Convert.ToInt32("25CF", 16)) + " " + Char.ConvertFromUtf32(Convert.ToInt32("25CF", 16)) + " " + Char.ConvertFromUtf32(Convert.ToInt32("25CF", 16)));
            MorseTable.Add("C", Char.ConvertFromUtf32(Convert.ToInt32("25AC", 16)) + " " + Char.ConvertFromUtf32(Convert.ToInt32("25CF", 16)) + " " + Char.ConvertFromUtf32(Convert.ToInt32("25AC", 16)) + " " + Char.ConvertFromUtf32(Convert.ToInt32("25CF", 16)));//
            MorseTable.Add("D", Char.ConvertFromUtf32(Convert.ToInt32("25AC", 16)) + " " + Char.ConvertFromUtf32(Convert.ToInt32("25CF", 16)) + " " + Char.ConvertFromUtf32(Convert.ToInt32("25CF", 16)));//
            MorseTable.Add("E", Char.ConvertFromUtf32(Convert.ToInt32("25CF", 16)));//
            MorseTable.Add("F", Char.ConvertFromUtf32(Convert.ToInt32("25CF", 16)) + " " + Char.ConvertFromUtf32(Convert.ToInt32("25CF", 16)) + " " + Char.ConvertFromUtf32(Convert.ToInt32("25AC", 16)) + " " + Char.ConvertFromUtf32(Convert.ToInt32("25CF", 16)));//
            MorseTable.Add("G", Char.ConvertFromUtf32(Convert.ToInt32("25AC", 16)) + " " + Char.ConvertFromUtf32(Convert.ToInt32("25AC", 16)) + " " + Char.ConvertFromUtf32(Convert.ToInt32("25CF", 16)));//
            MorseTable.Add("H", Char.ConvertFromUtf32(Convert.ToInt32("25CF", 16)) + " " + Char.ConvertFromUtf32(Convert.ToInt32("25CF", 16)) + " " + Char.ConvertFromUtf32(Convert.ToInt32("25CF", 16)) + " " + Char.ConvertFromUtf32(Convert.ToInt32("25CF", 16)));//
            MorseTable.Add("I", Char.ConvertFromUtf32(Convert.ToInt32("25CF", 16)) + " " + Char.ConvertFromUtf32(Convert.ToInt32("25CF", 16)));//
            MorseTable.Add("J", Char.ConvertFromUtf32(Convert.ToInt32("25CF", 16)) + " " + Char.ConvertFromUtf32(Convert.ToInt32("25AC", 16)) + " " + Char.ConvertFromUtf32(Convert.ToInt32("25AC", 16)) + " " + Char.ConvertFromUtf32(Convert.ToInt32("25AC", 16)));//
            MorseTable.Add("K", Char.ConvertFromUtf32(Convert.ToInt32("25AC", 16)) + " " + Char.ConvertFromUtf32(Convert.ToInt32("25CF", 16)) + " " + Char.ConvertFromUtf32(Convert.ToInt32("25AC", 16)));//
            MorseTable.Add("L", Char.ConvertFromUtf32(Convert.ToInt32("25CF", 16)) + " " + Char.ConvertFromUtf32(Convert.ToInt32("25AC", 16)) + " " + Char.ConvertFromUtf32(Convert.ToInt32("25CF", 16)) + " " + Char.ConvertFromUtf32(Convert.ToInt32("25CF", 16)));//
            MorseTable.Add("M", Char.ConvertFromUtf32(Convert.ToInt32("25AC", 16)) + " " + Char.ConvertFromUtf32(Convert.ToInt32("25AC", 16)));//
            MorseTable.Add("N", Char.ConvertFromUtf32(Convert.ToInt32("25AC", 16)) + " " + Char.ConvertFromUtf32(Convert.ToInt32("25CF", 16)));//
            MorseTable.Add("O", Char.ConvertFromUtf32(Convert.ToInt32("25AC", 16)) + " " + Char.ConvertFromUtf32(Convert.ToInt32("25AC", 16)) + " " + Char.ConvertFromUtf32(Convert.ToInt32("25AC", 16)));//
            MorseTable.Add("P", Char.ConvertFromUtf32(Convert.ToInt32("25CF", 16)) + " " + Char.ConvertFromUtf32(Convert.ToInt32("25AC", 16)) + " " + Char.ConvertFromUtf32(Convert.ToInt32("25AC", 16)) + " " + Char.ConvertFromUtf32(Convert.ToInt32("25CF", 16)));//
            MorseTable.Add("Q", Char.ConvertFromUtf32(Convert.ToInt32("25AC", 16)) + " " + Char.ConvertFromUtf32(Convert.ToInt32("25AC", 16)) + " " + Char.ConvertFromUtf32(Convert.ToInt32("25CF", 16)) + " " + Char.ConvertFromUtf32(Convert.ToInt32("25AC", 16)));//
            MorseTable.Add("R", Char.ConvertFromUtf32(Convert.ToInt32("25CF", 16)) + " " + Char.ConvertFromUtf32(Convert.ToInt32("25AC", 16)) + " " + Char.ConvertFromUtf32(Convert.ToInt32("25CF", 16)));//
            MorseTable.Add("S", Char.ConvertFromUtf32(Convert.ToInt32("25CF", 16)) + " " + Char.ConvertFromUtf32(Convert.ToInt32("25CF", 16)) + " " + Char.ConvertFromUtf32(Convert.ToInt32("25CF", 16)));//
            MorseTable.Add("T", Char.ConvertFromUtf32(Convert.ToInt32("25AC", 16)));//
            MorseTable.Add("U", Char.ConvertFromUtf32(Convert.ToInt32("25CF", 16)) + " " + Char.ConvertFromUtf32(Convert.ToInt32("25CF", 16)) + " " + Char.ConvertFromUtf32(Convert.ToInt32("25AC", 16)));//
            MorseTable.Add("V", Char.ConvertFromUtf32(Convert.ToInt32("25CF", 16)) + " " + Char.ConvertFromUtf32(Convert.ToInt32("25CF", 16)) + " " + Char.ConvertFromUtf32(Convert.ToInt32("25CF", 16)) + " " + Char.ConvertFromUtf32(Convert.ToInt32("25AC", 16)));//
            MorseTable.Add("W", Char.ConvertFromUtf32(Convert.ToInt32("25CF", 16)) + " " + Char.ConvertFromUtf32(Convert.ToInt32("25AC", 16)) + " " + Char.ConvertFromUtf32(Convert.ToInt32("25AC", 16)));//
            MorseTable.Add("X", Char.ConvertFromUtf32(Convert.ToInt32("25AC", 16)) + " " + Char.ConvertFromUtf32(Convert.ToInt32("25CF", 16)) + " " + Char.ConvertFromUtf32(Convert.ToInt32("25CF", 16)) + " " + Char.ConvertFromUtf32(Convert.ToInt32("25AC", 16)));//
            MorseTable.Add("Y", Char.ConvertFromUtf32(Convert.ToInt32("25AC", 16)) + " " + Char.ConvertFromUtf32(Convert.ToInt32("25CF", 16)) + " " + Char.ConvertFromUtf32(Convert.ToInt32("25AC", 16)) + " " + Char.ConvertFromUtf32(Convert.ToInt32("25AC", 16)));//
            MorseTable.Add("Z", Char.ConvertFromUtf32(Convert.ToInt32("25AC", 16)) + " " + Char.ConvertFromUtf32(Convert.ToInt32("25AC", 16)) + " " + Char.ConvertFromUtf32(Convert.ToInt32("25CF", 16)) + " " + Char.ConvertFromUtf32(Convert.ToInt32("25CF", 16)));//



            string res = "";


            Image _fakeImage = new Bitmap(1, 1);
            Graphics _graphics = Graphics.FromImage(_fakeImage);
            System.Drawing.Font fnt = new System.Drawing.Font(this.Font.Name,(float)this.Font.Size);
            SizeF l = _graphics.MeasureString(Txt, fnt);
            SizeF sp = _graphics.MeasureString(" ", fnt);
            int cntr = (int)(l.Width / sp.Width) + Txt.Length;
            string Bufer = "   ";
            for (int i = 0; i < cntr+1; i++)
            {
                Bufer = Bufer + " ";
            }

            res = res + MorseTable[Txt[0].ToString().ToUpper()] + "\r" + "\n";

            for (int i = 1; i <= Txt.Length - 2; i++)
            {
                res = res + Bufer + MorseTable[Txt[i].ToString().ToUpper()] + "\r" + "\n";
            }
            res = res + Bufer + MorseTable[Txt[Txt.Length - 1].ToString().ToUpper()];

            MorseTable = null;

            if (res.Trim().Length <= 0) res = Txt;

            return res;

        }

   
        public string ToColumn(string Txt, string Bufer)
        {
            string res = "";


            res = res + Txt[0].ToString().ToUpper() + "\r" + "\n";

            for (int i = 1; i <= Txt.Length - 2; i++)
            {
                res = res + Txt[i].ToString().ToUpper() + Bufer + "\r" + "\n";
            }
            res = res + Txt[Txt.Length - 1].ToString().ToUpper() + Bufer;

          

            //if (res.Trim().Length <= 0) res = Txt;

            return res;
        }


        public override object Clone()
        {
            AncorChartElementWord wrd = (AncorChartElementWord) base.Clone();
            wrd.DataSource = (AncorDataSource)wrd.DataSource.Clone();
            wrd.StartSymbol = (AncorSymbol)wrd.StartSymbol.Clone();
            wrd.EndSymbol = (AncorSymbol)wrd.EndSymbol.Clone();
            wrd.Font = (AncorFont)wrd.Font.Clone();

            return wrd;
        }
        
    }

}
