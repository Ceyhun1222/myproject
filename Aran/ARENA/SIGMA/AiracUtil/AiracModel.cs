using System;

namespace AiracUtil
{
    public class AiracModel
    {
        public int Index
        {
            get => AiracUtil.GetAiracCycleByDate(DateTime);
        }

        public string DisplayName => $"{Index} : {DateTime:yyyy/MM/dd}";

        public DateTime DateTime
        {
            get;set;
        }
    }
}