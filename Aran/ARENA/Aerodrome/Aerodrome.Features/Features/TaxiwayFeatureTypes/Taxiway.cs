using BusinessCore.Validation;
using BusinessCore.Validation.Exceptions;
using Framework.Attributes;
//using Framework.Stasy.SyncProvider;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;


namespace Aerodrome.Features
{
    [Description("Taxiway")]
    [CrudFeatureConfiguration(FeatureCategory = FeatureCategories.TaxiwayFeatureTypes)]
    public class AM_Taxiway : AM_AbstractFeature
    {
        public AM_Taxiway()
        {
            //AddRule(new ValidationRule(
            //    new BusinessCore.Validation.Exceptions.ValidationException("Please fill in a Name."),
            //    ValidateNameRequired));
        }
       
        public AM_AerodromeReferencePoint RelatedARP { get; set; }

        [MaxLength(5)]
        [CrudPropertyConfiguration(SetterPropertyNames = new string[] { nameof(RelatedARP) }, PropertyRequirement = PropertyRequirements.Mandatory)]
        public virtual string idarpt
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

        public override Enums.Feat_Type Feattype => Enums.Feat_Type.Taxiway;

        [Browsable(false)]
        public override string Descriptor => Name;

        private bool ValidateNameRequired()
        {
            return ValidateStringNotEmpty(Name);
        }
    }
}