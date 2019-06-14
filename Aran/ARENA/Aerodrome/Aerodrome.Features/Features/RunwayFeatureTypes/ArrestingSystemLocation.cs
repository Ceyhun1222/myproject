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
    [Description("ArrestingSystemLocation")]
    [CrudFeatureConfiguration(FeatureCategory = FeatureCategories.RunwayFeatureTypes)]
    public partial class AM_ArrestingSystemLocation : AM_FeatureVerticalQuality
	{
        public AM_ArrestingSystemLocation()
        {
            RelatedRunwayDirections = new List<AM_RunwayDirection>();
        }

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

        private List<AM_RunwayDirection> _relatedRunwayDirections;
        [CrudPropertyConfiguration(PropertyCategory = PropertyCategories.Relational, DisplayInGrid = false, PropertyRequirement = PropertyRequirements.Ignore)]
        public List<AM_RunwayDirection> RelatedRunwayDirections
        {
            get { return _relatedRunwayDirections; }
            set
            {
                _relatedRunwayDirections = value;
                SendPropertyChanged("RelatedRunwayDirections");
                SendPropertyChanged("idthr");

            }
        }
        [CrudPropertyConfiguration(PropertyCategory = PropertyCategories.Relational, SetterPropertyNames = new string[] { nameof(RelatedRunwayDirections) }, PropertyRequirement = PropertyRequirements.Mandatory)]
        public string idthr
        {
            get
            {
                string result = string.Join("_", RelatedRunwayDirections?.Select(r => r.Name));
                return result;
            }
        }

        [CrudPropertyConfiguration(PropertyRequirement = PropertyRequirements.Mandatory)]
        public DataType<UomDistance> setback { get; set; }

        [CrudPropertyConfiguration(PropertyRequirement = PropertyRequirements.Mandatory)]
        public DataType<UomDistance> aslength { get; set; }

        [CrudPropertyConfiguration(PropertyRequirement = PropertyRequirements.Mandatory)]
        public DataType<UomDistance> aslwidth { get; set; }

        [CrudPropertyConfiguration]
        public AM_Nullable<Status> status { get; set; }

        [CrudPropertyConfiguration(PropertyCategory = PropertyCategories.General, DisplayInGrid = false, PropertyRequirement = PropertyRequirements.Mandatory)]
        [JsonConverter(typeof(GeometryJsonConverter))]
        public IPolygon geopoly { get; set; }

        public override Enums.Feat_Type Feattype => Enums.Feat_Type.Arresting_System_Location;
    }
}
