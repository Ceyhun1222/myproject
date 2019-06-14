using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using Aerodrome.Enums;
using ESRI.ArcGIS.Geometry;
using Framework.Attributes;
using Newtonsoft.Json;

namespace Aerodrome.Features
{
    [Description("AerodromeSurfaceLighting")]
    [CrudFeatureConfiguration(FeatureCategory = FeatureCategories.AerodromeSurfaceLightingFeatureTypes)]
    public partial class AM_AerodromeSurfaceLighting : AM_FeatureVerticalQuality
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

        //для этого типа создать userControl. Выбор типа и выбор элемента. 
        [AllowableTypes(typeof(AM_Apron), typeof(AM_Taxiway), typeof(AM_TaxiwayHoldingPosition),typeof(AM_RunwayDirection))]
        [CrudPropertyConfiguration("ConnectedFeature", PropertyCategory = PropertyCategories.Relational, DisplayInGrid = false, PropertyRequirement = PropertyRequirements.Ignore)]
        public AM_AbstractFeature ConnectedFeature { get; set; }

        [CrudPropertyConfiguration]
        public AM_Nullable<Status> status { get; set; }

        [CrudPropertyConfiguration]
        public AM_Nullable<AM_Color> color { get; set; }

        [CrudPropertyConfiguration]
        public AM_Nullable<LightUse_Type> lgtuse { get; set; }

        [CrudPropertyConfiguration]
        public AM_Nullable<LightSource_Type> lstype { get; set; }

        [CrudPropertyConfiguration(PropertyCategory = PropertyCategories.General, DisplayInGrid = false, PropertyRequirement = PropertyRequirements.Mandatory)]
        [JsonConverter(typeof(GeometryJsonConverter))]
        public IPoint geopnt { get; set; }

        public override Enums.Feat_Type Feattype => Enums.Feat_Type.Aerodrome_Surface_Lighting;
    }
}
