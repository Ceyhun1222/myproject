using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace AIP.BaseLib.Airac
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
				if ( _airacList == null ) {
					var list = new List<AiracCycle> ( );

                    DateTime dateTime = new DateTime(2015, 12, 10);
                    int year = 15;
                    int incCycle = 0;
                    for (int i = 0; i < 500; i++)
                    {
                        if (i != 0) dateTime = dateTime.AddDays(28);
                        int curAiracYear = dateTime.AddDays(28).Year % 100;
                        if (year != curAiracYear)
                        {
                            year = curAiracYear;
                            incCycle = 0;
                        }
                        else
                        {
                            incCycle++;
                        }
                        //if (DateTime.Now <= dateTime)
                        //{
                            list.Add(new AiracCycle()
                            {
                                Cycle = incCycle + 1,
                                Ident = curAiracYear * 100 + incCycle + 1,
                                RadCutOff = dateTime.AddDays(-28),
                                IssueDate = dateTime, 
                                Airac = dateTime.AddDays(28)
                            });
                        //}
                    }
                    _airacList = new ReadOnlyCollection<AiracCycle>(list);
                }
                return _airacList;
            }
        }

        public static AiracCycle GetAiracCycle(DateTime dateTime)
        {
            return AiracCycleList.FirstOrDefault(ac => ac.RadCutOff.Year == dateTime.Year && 
                ac.RadCutOff.Month == dateTime.Month && 
                ac.RadCutOff.Day == dateTime.Day);
        }

        public static AiracDateTime CreateAiracDateTime(DateTime dateTime)
        {
            var tmpAC = GetAiracCycle(dateTime);

            if (tmpAC != null)
                return new AiracDateTime() { Mode = AiracSelectionMode.Airac, Value = tmpAC.RadCutOff };
            else
                return new AiracDateTime() { Mode = AiracSelectionMode.Custom, Value = dateTime };
        }


        private static ReadOnlyCollection<AiracCycle> _airacList;
    }
}
