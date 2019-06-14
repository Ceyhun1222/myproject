using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aran.Aim.BusinessRules.SbvrParser
{
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
}
