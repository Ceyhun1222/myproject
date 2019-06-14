using Aran.Aim;

namespace Holding.Models
{
    public class WayPointChoice
    {
        public WayPointChoice(SignificantPointChoice choice,string text)
        {
            Choice = choice;
            Text = text;
        }
        public SignificantPointChoice Choice { get; set; }
        public string Text { get; set; }
    }
}