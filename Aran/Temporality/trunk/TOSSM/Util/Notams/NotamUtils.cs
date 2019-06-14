using Aran.Temporality.Common.Entity;
using Aran.Temporality.CommonUtil.Context;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;


namespace TOSSM.Util.Notams
{
    static class NotamUtils
    {
        public static List<string> GetCoordinates(Notam notam)
        {
            var itemE = notam.ItemE;
            List<string> coordinates = new List<string>();
            try
            {
                var matches = Regex.Matches(itemE, @"[0-9]{1,6}[NS][0-9]{1,7}[EW]");
                foreach (Match match in matches)
                {
                    coordinates.Add(match.Value);
                }

            }
            catch
            {
                //ignore
            }

            if (coordinates.Count > 3 && coordinates.First().Equals(coordinates.Last()))
            {
                coordinates.RemoveAt(coordinates.Count - 1);
            }

            return coordinates;
        }

        public static Notam GetNotamToCancel(Notam notam)
        {
            var notams = CurrentDataContext.CurrentNoAixmDataService.GetAllNotams()
                .Where(t => NotamConverter.GetName(t).Equals(notam.RefNotam))
                .ToList();
            if (notams.Count == 0)
            {
                return null;
            }
            return notams.First();
        }
    }


}
