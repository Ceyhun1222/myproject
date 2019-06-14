using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using Aerodrome.Enums;
using Framework.Attributes;

namespace Aerodrome.Features
{
    [Description("Runway")]
    [CrudFeatureConfiguration(FeatureCategory = FeatureCategories.RunwayFeatureTypes)]
    public class AM_Runway : AM_AbstractFeature
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
        [CrudPropertyConfiguration(SetterPropertyNames = new string[] {nameof(RelatedARP) }, PropertyRequirement = PropertyRequirements.Mandatory)]
        public string idarpt
        {
            get
            {
                return RelatedARP?.idarpt.Value;
            }
        }

        [IncludeInSerialization]
        private string _name;

        [RelationalProperty]
        [CrudPropertyConfiguration(PropertyRequirement = PropertyRequirements.Mandatory)]
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
                SendPropertyChanged("Name");
            }
        }

        //[CrudPropertyConfiguration]
        //public AM_Nullable<RwyType> Type { get; set; }

        public override Enums.Feat_Type Feattype => Enums.Feat_Type.Runway;

        [Browsable(false)]
        public override string Descriptor => Name;
    }
}