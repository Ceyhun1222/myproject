using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;


namespace Accent.MapCore
{
    [Serializable()]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class AcntChartElementWord
    {
        private AcntDataSource _dataSource;

        public AcntDataSource DataSource
        {
            get { return _dataSource; }
            set { _dataSource = value; }
        }
        private AcntSymbol _endSymbol;

        public AcntSymbol EndSymbol
        {
            get { return _endSymbol; }
            set { _endSymbol = value; }
        }
        private AcntSymbol _startSymbol;

        public AcntSymbol StartSymbol
        {
            get { return _startSymbol; }
            set { _startSymbol = value; }
        }
        private textCase _textCase;

        public textCase TextCase
        {
            get { return _textCase; }
            set { _textCase = value; }
        }

        private string _textValue;

        public string TextValue
        {
            get { return _textValue; }
            set { _textValue = value; }
        }

        private AcntFont _font;

        public AcntFont Font
        {
            get { return _font; }
            set { _font = value; }
        }

        private double _characterSpacing;

        public double CharacterSpacing
        {
            get { return _characterSpacing; }
            set { _characterSpacing = value; }
        }

        private double _characterWidth;

        public double CharacterWidth
        {
            get { return _characterWidth; }
            set { _characterWidth = value; }
        }

        private double _leading;

        public double Leading
        {
            get { return _leading; }
            set { _leading = value; }
        }

        private textPosition _textPosition;

        public textPosition TextPosition
        {
            get { return _textPosition; }
            set { _textPosition = value; }
        }

        private double _wordSpacing;

        public double WordSpacing
        {
            get { return _wordSpacing; }
            set { _wordSpacing = value; }
        }

        private bool _morse;

        public bool Morse
        {
            get { return _morse; }
            set { _morse = value; }
        }


        public AcntChartElementWord()
        {
            this.DataSource = new AcntDataSource();
            this.EndSymbol = new AcntSymbol();
            this.StartSymbol = new AcntSymbol();
            this.TextCase = textCase.Normal;
            this.CharacterSpacing = 0;
            this.CharacterWidth = 100;
            this.Font = new AcntFont(false,false,false,new AcntColor(0,0,0));
            this.Leading = 0;
            this.TextPosition = textPosition.Normal;
            this.WordSpacing = 100;
            this.TextValue = "Value";
        }

        public override string ToString()
        {
            string AnnoTXT = this.TextValue;

            try
            {
                if (this.Morse) AnnoTXT = this.ToMorse(this.TextValue);

                AnnoTXT = this.StartSymbol.ToString() + AnnoTXT;

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
                
                AnnoTXT = AnnoTXT + this.EndSymbol.ToString();

                if (this.Font.Bold) AnnoTXT = "<BOL>" + AnnoTXT + "</BOL>";
                if (this.Font.Italic) AnnoTXT = "<ITA>" + AnnoTXT + "</ITA>";
                if (this.Font.UnderLine) AnnoTXT = "<UND>" + AnnoTXT + "</UND>";

                
            }
            catch
            { AnnoTXT = ""; }

            return AnnoTXT;
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

            for (int i = 0; i <= Txt.Length - 1; i++)
            {
                res = res + MorseTable[Txt[i].ToString().ToUpper()] + "   ";
            }


            MorseTable = null;

            if (res.Trim().Length <= 0) res = Txt;

            return res;


        }

   
        
    }

}
