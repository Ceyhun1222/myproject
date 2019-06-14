using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aran.Aim.BusinessRules.Parser.Document
{
    public class SbvrDocument
    {
        private int _currentIndex;
        private string _text;

        public SbvrDocument()
        {
            _currentIndex = 0;
        }

        public void Init(string text)
        {
            _text = text;
        }

        public TokenItem Next()
        {
            if (_text.Length <= _currentIndex)
                return null;

            return null;
        }

        public void Forward()
        {

        }
    }
}
