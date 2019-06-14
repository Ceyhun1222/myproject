using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAS.Model
{
    public class ComboBoxItem<T>
    {
        public ComboBoxItem() :
            this(default(T), string.Empty)
        {
        }

        public ComboBoxItem(T value, string text, int indexSymCount = 0)
        {
            Value = value;
            Text = text;
            IndexSymCount = indexSymCount;
        }

        public T Value { get; set; }

        public string ValueText
        {
            get { return Value.ToString().PadLeft(IndexSymCount, '0'); }
        }

        public string Text { get; set; }

        public int IndexSymCount { get; set; }
    }
}
