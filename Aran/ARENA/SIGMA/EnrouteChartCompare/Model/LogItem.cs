using System;

namespace EnrouteChartCompare.Model
{
    public class LogItem
    {
        public LogItem(string text, bool isError = false)
        {
            Text = text + Environment.NewLine;
            IsError = isError;
        }

        public bool IsError { get; set; }

        public string Text { get; set; }
    }
}