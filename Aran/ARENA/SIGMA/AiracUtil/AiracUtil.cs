using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AiracUtil
{
    public class AiracUtil
    {
        public static int GetAiracCycleByDate(DateTime dateTime)
        {
            DateTime n = dateTime;
            DateTime a = new DateTime(2014, 01, 09);//1401

            int totaldays = (int)(n - a).TotalDays;
            int airacCnt = totaldays / 28;
            int ost = totaldays % 28;

            //airacCnt = (airacCnt % 13) == 0 ? airacCnt - 1 : airacCnt;

            int yearPrefix = 14 + airacCnt / 13;
            int monthPrefix = 1 + (airacCnt % 13);

            return (yearPrefix * 100) + monthPrefix;
        }

        public static void GetAiracCirclePeriod(int AiracNumber, ref DateTime _start, ref DateTime _end)
        {
            if (AiracNumber < 1604)
            {
                //throw new Exception("Can not set past date");
                AiracNumber = 1605;
            }


            int yStart = 16;
            int circleStart = 4;

            int y1 = AiracNumber / 100;
            int airac1 = AiracNumber % 100;

            if (circleStart > airac1)
            {
                y1--;
                airac1 += 13;
            }

            int res = (y1 - yStart) * 13 + (airac1 - circleStart);

            double dayCount = res * 28;
            DateTime a = new DateTime(2016, 03, 31);//1604
            _start = a.AddDays(dayCount);
            _end = _start.AddDays(27);


        }

        public static IList<AiracModel> GetAiracList(int airacCycleCount)
        {
            var result = new List<AiracModel>();

            DateTime initialCycleDate = default(DateTime), endDate = default(DateTime);
            var initialCycleNumber = AiracUtil.GetAiracCycleByDate(DateTime.Now.AddDays(-airacCycleCount * 28));
            AiracUtil.GetAiracCirclePeriod(initialCycleNumber, ref initialCycleDate, ref endDate);
            //var cycle = AiracUtil.AiracUtil.GetAiracCycleByDate();

            for (var i = 1; i <= 2 * airacCycleCount; i++)
            {
                result.Add(new AiracModel
                {
                    DateTime = initialCycleDate.AddDays(i * 28)
                });
            }
            return result;
        }
    }
}
