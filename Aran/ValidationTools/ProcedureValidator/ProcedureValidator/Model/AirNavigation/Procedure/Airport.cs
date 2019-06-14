using Aran.Aim.Features;

namespace PVT.Model
{
    public class Airport : Feature
    {
        public AirportHeliport Original { get; private set; }
        public string Name { get; }
        public string Designator { get; }
        public string FullName
        {
            get
            {
                if (Name == null && Designator == null)
                    return string.Empty;
                if (Name == null)
                    return Designator;
                if (Designator == null)
                    return Name;
                return Name + "\\" + Designator;
            }
        }

        public Airport(AirportHeliport airport) : base(airport)
        {
            Original = airport;
            Name = airport.Name;
            Designator = airport.Designator;
        }
    }
}
