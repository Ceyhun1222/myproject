using Aerodrome.Enums;
using Framework.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Aerodrome.Features
{
    [Description("RunwayDirection")]
    [CrudFeatureConfiguration(FeatureCategory = FeatureCategories.RunwayFeatureTypes)]
    public class AM_RunwayDirection : AM_AbstractFeature
	{
        private AM_AerodromeReferencePoint _relatedARP;
        public AM_AerodromeReferencePoint RelatedARP
        {
            get { return _relatedARP; }
            set
            {
                _relatedARP = value;
                SendPropertyChanged("RelatedARP");
                SendPropertyChanged("idarpt");

            }
        }

        [MaxLength(5)]
        [CrudPropertyConfiguration(SetterPropertyNames = new string[] { nameof(RelatedARP) }, PropertyRequirement = PropertyRequirements.Mandatory)]
        public string idarpt
        {
            get
            {
                return RelatedARP?.idarpt.Value;
            }
        }

        [RelationalProperty]
        [CrudPropertyConfiguration(PropertyRequirement = PropertyRequirements.Mandatory)]
        public string Name { get; set; }

        private AM_Runway _associatedRunway;
        [CrudPropertyConfiguration(PropertyCategory = PropertyCategories.Relational, DisplayInGrid = false, PropertyRequirement = PropertyRequirements.Ignore)]
        public AM_Runway AssociatedRunway
        {
            get { return _associatedRunway; }
            set
            {
                _associatedRunway = value;
                SendPropertyChanged();
                SendPropertyChanged(nameof(idrwy));
            }
        }

        [CrudPropertyConfiguration(PropertyCategory = PropertyCategories.Relational, SetterPropertyNames = new string[] { nameof(AssociatedRunway) }, PropertyRequirement = PropertyRequirements.Mandatory)]
        public string idrwy
        {
            get
            {
                return AssociatedRunway?.Name;
            }
        }

        public override Feat_Type Feattype => Enums.Feat_Type.Runway_Direction;

        [Browsable(false)]
        public override string Descriptor => Name;
    }
}