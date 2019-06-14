using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aran.Aim.BusinessRules.SbvrParser
{
    public class TaggedDocument
    {
        private static string[] _keywords;
        private List<TaggedItem> _items;

        public TaggedDocument()
        {
            _items = new List<TaggedItem>();
        }

        public void Init(string text)
        {
            _items.Clear();

            var reader = new TaggedReader();
            reader.Init(text);

            while (reader.Next())
                _items.Add(reader.Current);

            SplitKeywords();
        }

        public TaggedItem this[int index]
        {
            get
            {
                if (index >= 0 && index < _items.Count)
                    return _items[index];
                return new TaggedItem();
            }
        }

        public TaggedItem Current
        {
            get
            {
                if (CurrentIndex < _items.Count)
                    return _items[CurrentIndex];
                return new TaggedItem();
            }
        }

        public bool Next()
        {
            CurrentIndex++;
            return CurrentIndex < _items.Count;
        }

        public int CurrentIndex { get; set; } = -1;

        public bool IsLast
        {
            get { return CurrentIndex == _items.Count ; }
        }


        public static void SplitKeyword(string keyword, List<string> keywordList)
        {
            if (_keywords == null)
            {
                var s =
                    "a , " +
                    "an , " +
                    "and , " +
                    "value , " +
                    "with , " +
                    "assigned , " +
                    "not , " +
                    "or , " +
                    "resolved-into , " +
                    "shall , " +
                    "it is obligatory that , " +
                    "it is prohibited that , " +
                    "each , " +
                    "have ";

                _keywords = s.Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
            }

            foreach (var s in _keywords)
            {
                if (keyword.StartsWithIC(s))
                {
                    //Keyword last symbol is space.
                    var newKeyword = keyword.Substring(0, s.Length - 1);
                    keywordList.Add(newKeyword);

                    var restKeyword = keyword.Substring(s.Length).Trim();

                    if (restKeyword.Length > 0)
                        SplitKeyword(restKeyword, keywordList);

                    return;
                }
            }

            keywordList.Add(keyword);
        }

        public static List<TaggedItem> ParseTaggedItems(string text)
        {
            var td = new TaggedDocument();
            td.Init(text);
            return td._items;
        }


        private void SplitKeywords()
        {
            for (var i = 0; i < _items.Count; i++)
            {
                var item = _items[i];

                if (item.Key == TaggedKey.Keyword)
                {
                    var keywordList = new List<string>();
                    SplitKeyword(item.Text, keywordList);
                    if (keywordList.Count > 1)
                    {
                        item.Text = keywordList[0];
                        for (var j = 1; j < keywordList.Count; j++)
                        {
                            _items.Insert(i + j, new TaggedItem { Key = TaggedKey.Keyword, Text = keywordList[j] });
                        }
                        i += keywordList.Count - 1;
                    }
                }
            }
        }

    }
}
