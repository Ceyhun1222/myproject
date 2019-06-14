using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Aran.Aim.BRules.SbvrParser
{
    public class TaggedReader : ITaggedReader
    {
        private string _text;
        private int _currentIndex;
        private int _lastReadIndex;

        public TaggedReader()
        {
        }

        public void Init(string text)
        {
            _text = text;
        }

        public TaggedItem LastRead { get; private set; }

        public TaggedItem Read()
        {
            if (_text.Length <= _currentIndex)
            {
                LastRead = new TaggedItem();
                return LastRead;
            }

            SkipEmpty();

            string key = string.Empty;
            string text = string.Empty;

            if (_text[_currentIndex] == '<')
            {
                var ind1a = _text.IndexOf('>', _currentIndex);

                if (ind1a == -1)
                    throw new InvalidTaggedException();

                var ind1b = _text.IndexOf(' ', _currentIndex);
                if (ind1b == -1 || ind1b > ind1a)
                    ind1b = ind1a;

                var ind1 = Math.Min(ind1a, ind1b);

                if (ind1 == -1)
                    throw new InvalidTaggedException();
                
                key = _text.Substring(_currentIndex + 1, ind1 - _currentIndex - 1);

                ind1 = Math.Max(ind1a, ind1b);

                var ind2 = _text.IndexOfIC("</" + key + ">", ind1 - 1);
                if (ind2 == -1)
                    throw new InvalidTaggedException();

                text = _text.Substring(ind1 + 1, ind2 - ind1 - 1);
                var ind3 = _text.IndexOf('>', ind2 + 1);
                _lastReadIndex = ind3 + 1;
            }
            else
            {
                var ind = _text.IndexOf('<', _currentIndex);
                if (ind > 0)
                {
                    text = _text.Substring(_currentIndex, ind - _currentIndex);
                    _lastReadIndex = ind;
                }
                else
                {
                    text = _text.Substring(_currentIndex);
                    _lastReadIndex = _text.Length;
                }
            }

            LastRead = TaggedItem.Create(key, text);
            return LastRead;
        }

        public TaggedItem ReadAndNext()
        {
            Read();
            Next();
            return LastRead;
        }

        public void Next()
        {
            _currentIndex = _lastReadIndex;
        }

        private void SkipEmpty()
        {
            while(true)
            {
                if (_text.Length <= _currentIndex)
                    return;

                if (!char.IsWhiteSpace(_text[_currentIndex]))
                    break;

                _currentIndex++;
            }
        }
    }

    public class TaggedItem
    {
        public TaggedItem()
        {
            Key = TaggedKey.None;
            Text = string.Empty;
        }

        public TaggedKey Key { get; set; }

        public string Text { get; set; }

        public bool IsText(string text)
        {
            return (string.Compare(Text, text, true) == 0);
        }

        public bool IsEqual(TaggedKey key, string text)
        {
            return Key == key && (string.Compare(Text, text, true) == 0);
        }

        public override string ToString()
        {
            return $"[{Key}] {Text ?? string.Empty}";
        }

        public static TaggedItem Create(string key, string text)
        {
            if (!EnumEx.TryGetValueFromDescription<TaggedKey>(key, true, out TaggedKey aKey))
                aKey = TaggedKey.Unknown;

            return new TaggedItem
            {
                Key = aKey,
                Text = text.Trim()
            };
        }
    }

    public enum TaggedKey
    {
        [Description("~")]
        None,

        [Description("keyword")]
        Keyword,

        [Description("NounConcept")]
        Noun,

        [Description("Verb-concept")]
        Verb,

        [Description("Name")]
        Name,

        [Description("font")]
        Font,

        [Description("")]
        Unknown
    }
}
