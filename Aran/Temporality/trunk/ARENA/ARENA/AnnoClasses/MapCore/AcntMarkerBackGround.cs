using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Accent.MapCore
{
    [Serializable()]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class AcntMarkerBackGround
    {
        private int _characterIndex;

        public int CharacterIndex
        {
            get { return _characterIndex; }
            set { _characterIndex = value; }
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

        public AcntMarkerBackGround(int _CharacterIndex, int _CharacterSize, string _FontName)
        {
            this.CharacterIndex = _CharacterIndex;
            this.CharacterSize = _CharacterSize;
            this.FontName = _FontName;

        }
    }

}
