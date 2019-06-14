using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace ANCOR.MapCore
{
    [Serializable()]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class AncorMarkerBackGround : AbstractChartClass
    {
        private int _characterIndex;
        public int CharacterIndex
        {
            get { return _characterIndex; }
            set { _characterIndex = value; }
        }

        private int _innerCharacterIndex;
        public int InnerCharacterIndex
        {
            get { return _innerCharacterIndex; }
            set { _innerCharacterIndex = value; }
        }

        private int _characterSize;

        public int CharacterSize
        {
            get { return _characterSize; }
            set { _characterSize = value; }
        }
        private string _fontName;

        public string FontName
        {
            get { return _fontName; }
            set { _fontName = value; }
        }

        public AncorMarkerBackGround()
        {
        }

        public AncorMarkerBackGround(int _CharacterIndex, int _CharacterSize, string _FontName)
        {
            this.CharacterIndex = _CharacterIndex;
            this.CharacterSize = _CharacterSize;
            this.FontName = _FontName;

        }

        public AncorMarkerBackGround(int _CharacterIndex, int _IntCharacterSize, int _CharacterSize, string _FontName)
        {
            this.CharacterIndex = _CharacterIndex;
            this.CharacterSize = _CharacterSize;
            this.InnerCharacterIndex = _IntCharacterSize;
            this.FontName = _FontName;

        }
    }

}
