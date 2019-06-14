using System;

namespace Aran.Omega.TypeB.Settings
{
    public class QueryModel : SettingsModel
    {
        public QueryModel()
        {
            Type = MenuType.Query;
        }
        public Double Radius
        {
            get { return _radius; }
            set { _radius = value; }
        }

        public Guid Organization
        {
            get { return _organization; }
            set { _organization = value; }
        }

        public Guid Aeroport
        {
            get { return _aeroport; }
            set { _aeroport = value; }
        }

        public bool ValidationReportIsCheked { get; set; }

        private Double _radius = 100000.0;
        private Guid _organization = Guid.Empty;
        private Guid _aeroport = Guid.Empty;

    }
}