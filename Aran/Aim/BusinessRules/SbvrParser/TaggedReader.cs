using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aran.Aim.BusinessRules.SbvrParser
{
    public class TaggedReader
    {
        private string _text;
        private int _readIndex;

        public TaggedReader()
        {
            Current = new TaggedItem();
        }

        public void Init(string text)
        {
            _text = text;
        }

        public TaggedItem Current { get; private set; }

        public bool Next()
        {
            Current = Read();
            return Current.Key != TaggedKey.None;
        }


        private TaggedItem Read()
        {
            if (_text.Length <= _readIndex)
                return new TaggedItem();

            SkipEmpty();

            string key = string.Empty;
            string text = string.Empty;

            if (_text[_readIndex] == '<')
            {
                var ind1a = _text.IndexOf('>', _readIndex);

                if (ind1a == -1)
                    throw new InvalidTaggedException();

                var ind1b = _text.IndexOf(' ', _readIndex);
                if (ind1b == -1 || ind1b > ind1a)
                    ind1b = ind1a;

                var ind1 = Math.Min(ind1a, ind1b);

                if (ind1 == -1)
                    throw new InvalidTaggedException();

                key = _text.Substring(_readIndex + 1, ind1 - _readIndex - 1);

                ind1 = Math.Max(ind1a, ind1b);

                var ind2 = _text.IndexOfIC("</" + key + ">", ind1 - 1);
                if (ind2 == -1)
                    throw new InvalidTaggedException();

                text = _text.Substring(ind1 + 1, ind2 - ind1 - 1);
                var ind3 = _text.IndexOf('>', ind2 + 1);
                _readIndex = ind3 + 1;
            }
            else
            {
                var ind = _text.IndexOf('<', _readIndex);
                if (ind > 0)
                {
                    text = _text.Substring(_readIndex, ind - _readIndex);
                    _readIndex = ind;
                }
                else
                {
                    text = _text.Substring(_readIndex);
                    _readIndex = _text.Length;
                }
            }

            return TaggedItem.Create(key, text);
        }

        private void SkipEmpty()
        {
            while (true)
            {
                if (_text.Length <= _readIndex)
                    return;

                if (!char.IsWhiteSpace(_text[_readIndex]))
                    break;

                _readIndex++;
            }
        }
    }
}
