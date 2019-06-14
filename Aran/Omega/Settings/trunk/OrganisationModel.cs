using Aran.Aim.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aran.Omega.SettingsUI
{
    public enum OrganisationType
    {
        All,
        FromDatabase
    }

    public class OrganisationModel
    {
        public OrganisationModel(OrganisationAuthority organisation,OrganisationType organisationType)
        {
            Organisation = organisation;
            OrganisationType = organisationType;
        }

        public OrganisationType OrganisationType { get; set; }

        public string Name => OrganisationType ==OrganisationType.All?"All":Organisation.Name;

        public OrganisationAuthority Organisation { get; }
    }
}
