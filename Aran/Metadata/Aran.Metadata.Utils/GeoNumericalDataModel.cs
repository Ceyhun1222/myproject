using Aran.Aim.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aran.Metadata.Utils
{
    public class GeoNumericalDataModel
    {
        public double Accuracy { get; set; }
        public UomDistance AccuracyDimension { get; set; } = UomDistance.M;

        public double Resolution { get; set; }
        public UomDuration ResolutionDimension { get; set; } = UomDuration.SEC;

        //todo change to enum
        //public CodeProcedureFixRole Role { get; set; }
        public string Role { get; set; }

        public string DesignatorDescription { get; set; }

        public string LegType { get; set; }

        public string GetDescription()
        {
            if (LegType == null)
                return null;

            var description = "Leg type: " + LegType.ToString() + Environment.NewLine;

            if (!String.IsNullOrWhiteSpace(Role))
                description += "Role: " + Role.ToString() + Environment.NewLine;

            if (!String.IsNullOrWhiteSpace(DesignatorDescription))
                description += "Designator point: " + DesignatorDescription.ToString() + Environment.NewLine;

            return description;
        }
    }
}
