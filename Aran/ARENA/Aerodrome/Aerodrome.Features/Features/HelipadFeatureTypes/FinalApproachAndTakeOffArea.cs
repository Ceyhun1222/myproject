using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    [Description("FATO")]
    [CrudFeatureConfiguration(FeatureCategory = FeatureCategories.HelipadFeatureTypes)]
    public partial class AM_FinalApproachAndTakeOffArea : AM_FeatureVerticalQuality
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
        public string idrwy { get; set; }

        [CrudPropertyConfiguration]
        public AM_Nullable<SurfaceComposition> surftype { get; set; }

        [CrudPropertyConfiguration]
        public AM_Nullable<Status> status { get; set; }

        [CrudPropertyConfiguration(PropertyCategory = PropertyCategories.General, DisplayInGrid = false, PropertyRequirement = PropertyRequirements.Mandatory)]
        [JsonConverter(typeof(GeometryJsonConverter))]
        public IPolygon geopoly { get; set; }

        public override Enums.Feat_Type Feattype => Enums.Feat_Type.Final_Approach_And_Take_Off_Area;

    }
}
