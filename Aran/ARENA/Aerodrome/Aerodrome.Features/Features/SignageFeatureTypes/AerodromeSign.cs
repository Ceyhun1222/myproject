using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aerodrome.Enums;
using Aerodrome.DataType;
using ESRI.ArcGIS.Geometry;
using System.ComponentModel;
using Framework.Attributes;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Aerodrome.Features
{
    [Description("AerodromeSign")]
    [CrudFeatureConfiguration(FeatureCategory = FeatureCategories.SignageFeatureTypes)]
    public partial class AM_AerodromeSign : AM_FeatureVerticalQuality
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

        [CrudPropertyConfiguration(PropertyRequirement = PropertyRequirements.Mandatory)]
        public DataType<UomDistance> height { get; set; }

        [CrudPropertyConfiguration]
        public AM_Nullable<Status> status { get; set; }

        [CrudPropertyConfiguration]
        public AM_Nullable<SignType> signtype { get; set; }

        [CrudPropertyConfiguration]
        public AM_Nullable<string> msgfront { get; set; }

        [CrudPropertyConfiguration]
        public AM_Nullable<string> msgback { get; set; }

        [CrudPropertyConfiguration(PropertyRequirement = PropertyRequirements.Mandatory)]
        public DataType<UomSlope> signdir { get; set; }

        [CrudPropertyConfiguration(PropertyCategory = PropertyCategories.General, DisplayInGrid = false, PropertyRequirement = PropertyRequirements.Mandatory)]
        [JsonConverter(typeof(GeometryJsonConverter))]
        public IPoint geopnt { get; set; }

        public override Enums.Feat_Type Feattype => Enums.Feat_Type.Aerodrome_Sign;

    }
}
