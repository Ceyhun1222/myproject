//using Aerodrome.Context;
using ESRI.ArcGIS.Geometry;
using Framework.Attributes;
using Newtonsoft.Json;
//using Framework.Stasy;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;


namespace Aerodrome.Features
{
    [Description("Apron")]
    [CrudFeatureConfiguration(FeatureCategory = FeatureCategories.ApronFeatureTypes)]
    public class AM_Apron : AM_AbstractFeature
	{
        
        public AM_AerodromeReferencePoint RelatedARP { get; set; }

        [MaxLength(5)]
        [CrudPropertyConfiguration(SetterPropertyNames = new string[] { nameof(RelatedARP) }, PropertyRequirement = PropertyRequirements.Mandatory)]
        public string idarpt
        {
            get
            {
                return RelatedARP?.idarpt.Value;
            }
        }

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

        [CrudPropertyConfiguration(PropertyCategory = PropertyCategories.General, DisplayInGrid = false, PropertyRequirement = PropertyRequirements.Mandatory)]
        [JsonConverter(typeof(GeometryJsonConverter))]
        public IPolygon geopoly { get; set; }

        public override Enums.Feat_Type Feattype => Enums.Feat_Type.Apron;

        [Browsable(false)]
        public override string Descriptor => Name;

    }
}
