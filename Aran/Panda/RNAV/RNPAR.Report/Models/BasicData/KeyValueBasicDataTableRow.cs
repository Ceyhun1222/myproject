using System;

namespace Aran.Panda.RNAV.RNPAR.Report.Models.BasicData
{
    public class KeyValueBasicDataTableRow : AbstractBasicDataTableRow
    {
        public string Key { get; }

        public string Value { get; }

        public KeyValueBasicDataTableRow(string key, string value)
        {
            Key = key ?? throw new ArgumentNullException(nameof(key));
            Value = value ?? throw new ArgumentNullException(nameof(value));
        }
    }
}
