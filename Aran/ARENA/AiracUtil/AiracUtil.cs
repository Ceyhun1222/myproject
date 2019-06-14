using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AiracUtil
{
    public class AiracSelectorViewModel
    {
        public static string AiracMessage(DateTime date)
        {
            var cycle = GetAiracCycleByDate(date);
            var airacMessage = (cycle > -1) ? "; AIRAC: " + cycle : "; custom AIRAC";
            return airacMessage;
        }

        public static DateTime GetAiracCycleByIndex(int index)
        {
            if (index < 1408) throw new Exception("Can not set past date");
            return new DateTime(2014, 7, 24, 0, 0, 0).AddDays((index - 1408) * 28);
        }

        public static int GetAiracCycleByDate(DateTime dateTime)
        {
            var d = dateTime.Subtract(new DateTime(2014, 7, 24, 0, 0, 0));
            if (d.Seconds > 0) return -1;
            if (d.Minutes > 0) return -1;
            if (d.Hours > 0) return -1;

            if (d.Days % 28 != 0) return -1;

            return 1408 + (d.Days / 28);
        }

        public static int GetCurrentAiracCycle()
        {
            var r = 1408 + DateTime.Now.Subtract(new DateTime(2014, 7, 24, 0, 0, 0)).Days / 28;
            return r < 1408 ? 1408 : r;
        }
    }
}
