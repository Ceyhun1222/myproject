using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aran.Aim.Data
{
    public class User
    {
        public User()
        {
            FeatureTypes = new List<int>();
        }

        public long Id{ get; set; }

        public string Name { get; set; }
        
        public string UserName { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Position { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public string Zip { get; set; }
        public string FaxNumber { get; set; }
        public string DataOriginatorCode { get; set; }
        public string Department { get; set; }
        public string Organization { get; set; }

        public string Password { get; set; }

        public Privilige Privilege { get; set; }

        public List<int> FeatureTypes
        {
            get;
            private set;
        }

        public void AddFeatType(string featureName)
        {
            AbstractType abstractType;
            FeatureType featType;
            if (Enum.TryParse<FeatureType>(featureName, out featType))
                FeatureTypes.Add((int)featType);
            else if (Enum.TryParse<AbstractType>(featureName, out abstractType))
                FeatureTypes.Add((int)abstractType);
            else
                throw new NotImplementedException("Feature Type (" + featureName + ") is not implemented");
        }

        public bool ContainsFeatType(string featureName)
        {
            if (Privilege == Privilige.prAdmin)
                return true;
            AbstractType abstractType;
            FeatureType featType;
            int featTypeIndex;
            if (Enum.TryParse<FeatureType>(featureName, out featType))
                featTypeIndex = ((int)featType);
            else if (Enum.TryParse<AbstractType>(featureName, out abstractType))
                featTypeIndex = ((int)abstractType);
            else
                throw new NotImplementedException("Feature Type (" + featureName + ") is not implemented");
            return FeatureTypes.Contains(featTypeIndex);
        }

        public bool ContainsFeatType(FeatureType featureType)
        {
            if (Privilege == Privilige.prAdmin)
                return true;
            return FeatureTypes.Contains((int)featureType);
        }

    }

    public enum Privilige
    {
        prAdmin,
        prReadOnly,
        prReadWrite
    }
}
