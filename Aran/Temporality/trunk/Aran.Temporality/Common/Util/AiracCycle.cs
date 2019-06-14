using System;

namespace Aran.Temporality.Common.Util
{
    public static class AiracCycle
    {
        public static DateTime GetAiracCycleByIndex(int index)
        {
            if (index < 1408) throw new Exception("Can not set past date");
            //1408 cycle = 24 july 2014
            return new DateTime(2014, 7, 24, 0, 0, 0).AddDays((index - 1408) * 28);
        }

        public static int GetAiracCycleByStrictDate(DateTime dateTime)
        {
            var d = dateTime.Subtract(new DateTime(2014, 7, 24, 0, 0, 0));
            if (d.Seconds > 0) return -1;
            if (d.Minutes > 0) return -1;
            if (d.Hours > 0) return -1;

            if (d.Days % 28 != 0) return -1;

            return 1408 + (d.Days / 28);
        }


        public static int GetAiracCycleNextToDate(DateTime dateTime)
        {
            var r = 1408 + Convert.ToInt32(Math.Ceiling(dateTime.Date.Subtract(new DateTime(2014, 7, 24, 0, 0, 0)).Days / (double)28));
            return r < 1408 ? 1408 : r;
        }

        public static int GetAiracCycleByDate(DateTime dateTime)
        {
            var r = 1408 + dateTime.Date.Subtract(new DateTime(2014, 7, 24, 0, 0, 0)).Days / 28;
            return r < 1408 ? 1408 : r;
        }

        public static int GetCurrentAiracCycle()
        {
            return GetAiracCycleByDate(DateTime.Today);
        }

        public static int GetNextAiracCycle()
        {
            return GetAiracCycleNextToDate(DateTime.Today);
        }

        public static DateTime GetCurrentAiracCycleDate()
        {
            return GetAiracCycleByIndex(GetCurrentAiracCycle());
        }

        public static DateTime GetNextAiracCycleDate()
        {
            return GetAiracCycleByIndex(GetNextAiracCycle());
        }

        public static int GetPermittedAiracCycle()
        {
            var r = 1408 + Convert.ToInt32(Math.Ceiling((DateTime.Today.AddDays(0 /*56*/).Subtract(new DateTime(2014, 7, 24, 0, 0, 0)).Days) / (double)28));
            return r < 1408 ? 1408 : r;
        }

        public static DateTime GetPermittedAiracCycleDate()
        {
            var r = 1408 + Convert.ToInt32(Math.Ceiling((DateTime.Today.AddDays(0 /*56*/).Subtract(new DateTime(2014, 7, 24, 0, 0, 0)).Days) / (double)28));
            return GetAiracCycleByIndex(r < 1408 ? 1408 : r);
        }


        public static DateTime GetPermittedEffectiveDate(bool perm)
        {
            return DateTime.Today; //perm?DateTime.Today.AddDays(56):DateTime.Today;
        }

        public static bool CanCreateCycle(DateTime dateTime, bool perm)
        {
            return GetPermittedEffectiveDate(perm).CompareTo(dateTime) <= 0;
        }

        public static DateTime GetPermittedPublishDate(bool perm)
        {
            return DateTime.Today; //perm?DateTime.Today.AddDays(42):DateTime.Today;
        }


        public static bool CanPublish(DateTime dateTime, bool perm)
        {
            return GetPermittedPublishDate(perm).CompareTo(dateTime) <= 0;
        }
    }
}