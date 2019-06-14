using Aran.AranEnvironment;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Aran.Controls.Airac
{
    public class AiracCycle
    {
        public int Cycle { get; set; }

        public int Ident { get; set; }

        public DateTime RadCutOff { get; set; }

        public DateTime IssueDate { get; set; }

        public DateTime Airac { get; set; }


        public string GetInfo()
        {
            return "Ident:\t" + Ident +
                "\nCycle:\t" + Cycle +
                "\nIssue:\t" + IssueDate.ToString("dd-MMM") +
                "\nAIRAC:\t" + Airac.ToString("dd-MMM");

        }


        public static ReadOnlyCollection<AiracCycle> AiracCycleList
        {
            get
            {
                if (_airacList == null)
                {
					var list = new List<AiracCycle> ( );

                    var cicle = GetAiracCycleNextToDate(DateTime.Today);

                    for(var i = 0; i < 20; i++)
                    {
                        var dateTime = GetAiracCycleByIndex(cicle);

						list.Add ( new AiracCycle ( )
						{
							Cycle = GetAiracCycleByDate(dateTime),
							Ident = (dateTime.Year - 2000) * 100 + dateTime.Month,
							RadCutOff = dateTime.AddDays(-2 * 28),
							IssueDate = dateTime.AddDays(-28),
							Airac = dateTime
						} );

                        cicle = GetAiracCycleNextToDate(dateTime.AddDays(1));
                    }

					_airacList = new ReadOnlyCollection<AiracCycle>(list);
                }

                return _airacList;
            }
        }

        public static AiracCycle GetAiracCycle(DateTime dateTime)
        {
            return AiracCycle.AiracCycleList.Where(ac => 
                ac.RadCutOff.Year == dateTime.Year && 
                ac.RadCutOff.Month == dateTime.Month && 
                ac.RadCutOff.Day == dateTime.Day).FirstOrDefault();
        }

        public static AiracDateTime CreateAiracDateTime(DateTime dateTime)
        {
            var tmpAC = GetAiracCycle(dateTime);

            if (tmpAC != null)
                return new AiracDateTime() { Mode = AiracSelectionMode.Airac, Value = tmpAC.RadCutOff };
            else
                return new AiracDateTime() { Mode = AiracSelectionMode.Custom, Value = dateTime };
        }


        public static DateTime GetAiracCycleByIndex(int index)
        {
            if (index < 1408) throw new Exception("Can not set past date");
            //1408 cycle = 24 july 2014
            return new DateTime(2014, 7, 24, 0, 0, 0).AddDays((index - 1408) * 28);
        }

        public static DateTime GetCurrentAiracCycleDate()
        {
            return GetAiracCycleByIndex(GetCurrentAiracCycle());
        }

        public static DateTime GetNextAiracCycleDate()
        {
            return GetAiracCycleByIndex(GetNextAiracCycle());
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


        private static ReadOnlyCollection<AiracCycle> _airacList;
    }
}
