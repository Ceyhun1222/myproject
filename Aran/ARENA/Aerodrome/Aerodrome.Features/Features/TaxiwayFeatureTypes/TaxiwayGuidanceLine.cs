using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aerodrome.Enums;
using ESRI.ArcGIS.Geometry;
using Aerodrome.DataType;
using System.Collections.ObjectModel;
using Framework.Attributes;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using System.ComponentModel;

namespace Aerodrome.Features
{
    [Description("TaxiwayGuidanceLine")]
    [CrudFeatureConfiguration(FeatureCategory = FeatureCategories.TaxiwayFeatureTypes)]
    public partial class AM_TaxiwayGuidanceLine : AM_FeatureVerticalQuality
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

        private AM_Taxiway _relatedTaxiway;
        [CrudPropertyConfiguration(PropertyCategory = PropertyCategories.Relational, DisplayInGrid = false, PropertyRequirement = PropertyRequirements.Ignore)]
        public AM_Taxiway RelatedTaxiway
        {
            get { return _relatedTaxiway; }
            set
            {
                _relatedTaxiway = value;
                SendPropertyChanged("RelatedTaxiway");
                SendPropertyChanged("idlin");
            }
        }

        [RelationalProperty]
        [CrudPropertyConfiguration(PropertyCategory = PropertyCategories.Relational, SetterPropertyNames = new string[] { nameof(RelatedTaxiway) })]
        public AM_Nullable<string> idlin
        {
            get
            {
                return RelatedTaxiway?.Name;

            }
        }

        [CrudPropertyConfiguration]
        public AM_Nullable<Status> status { get; set; }

        [CrudPropertyConfiguration(PropertyRequirement = PropertyRequirements.Mandatory)]
        public DataType<UomDistance> wingspan { get; set; }

        [CrudPropertyConfiguration(PropertyRequirement = PropertyRequirements.Mandatory)]
        public DataType<UomSpeed> maxspeed { get; set; }

        [CrudPropertyConfiguration]
        public AM_Nullable<AM_Color> color { get; set; }

        [CrudPropertyConfiguration]
        public AM_Nullable<AM_Style> style { get; set; }

        [CrudPropertyConfiguration]
        public AM_Nullable<AM_Direction> direc { get; set; }

        [CrudPropertyConfiguration(PropertyCategory = PropertyCategories.General, DisplayInGrid = false, PropertyRequirement = PropertyRequirements.Mandatory)]
        [JsonConverter(typeof(GeometryJsonConverter))]
        public IPolyline geoline { get; set; }

        public override Enums.Feat_Type Feattype => Enums.Feat_Type.Taxiway_Guidance_Line;

        public override string Descriptor => idlin?.Value;

    }
}
