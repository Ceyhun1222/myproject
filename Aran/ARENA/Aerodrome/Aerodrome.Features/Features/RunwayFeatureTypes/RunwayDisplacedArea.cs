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
    [Description("RunwayDisplacedArea")]
    [CrudFeatureConfiguration(FeatureCategory = FeatureCategories.RunwayFeatureTypes)]
    public partial class AM_RunwayDisplacedArea : AM_FeatureVerticalQuality
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

        private AM_RunwayDirection _relatedRunwayDirection;
        [CrudPropertyConfiguration(PropertyCategory = PropertyCategories.Relational, DisplayInGrid = false, PropertyRequirement = PropertyRequirements.Ignore)]
        public AM_RunwayDirection RelatedRunwayDirection
        {
            get { return _relatedRunwayDirection; }
            set
            {
                _relatedRunwayDirection = value;
                SendPropertyChanged("RelatedRunwayDirection");
                SendPropertyChanged("idthr");
            }
        }

        [RelationalProperty]
        [CrudPropertyConfiguration(PropertyCategory = PropertyCategories.Relational, SetterPropertyNames = new string[] { nameof(RelatedRunwayDirection) }, PropertyRequirement = PropertyRequirements.Mandatory)]
        public string idthr
        {
            get
            {
                return RelatedRunwayDirection?.Name;
            }
        }

        [CrudPropertyConfiguration]
        public AM_Nullable<Status> status { get; set; }

        [CrudPropertyConfiguration]
        public AM_Nullable<SurfaceComposition> surftype { get; set; }

        [CrudPropertyConfiguration]
        public AM_Nullable<string> pcn { get; set; }

        [CrudPropertyConfiguration]
        public AM_Nullable<string> restacft { get; set; }

        [CrudPropertyConfiguration]
        public AM_Nullable<LowVisibilityOper_Type> lvo { get; set; }

        [CrudPropertyConfiguration(PropertyCategory = PropertyCategories.General, DisplayInGrid = false, PropertyRequirement = PropertyRequirements.Mandatory)]
        [JsonConverter(typeof(GeometryJsonConverter))]
        public IPolygon geopoly { get; set; }

        public override Enums.Feat_Type Feattype => Enums.Feat_Type.Runway_Displaced_Area;

    }
}
