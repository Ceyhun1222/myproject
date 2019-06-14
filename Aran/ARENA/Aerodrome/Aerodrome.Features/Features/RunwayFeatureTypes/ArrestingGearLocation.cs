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
    [Description("ArrestingGearLocation")]
    [CrudFeatureConfiguration(FeatureCategory = FeatureCategories.RunwayFeatureTypes)]
    public partial class AM_ArrestingGearLocation : AM_FeatureVerticalQuality
	{
        public AM_ArrestingGearLocation()
        {
            ServicedRunwayDirections = new List<AM_RunwayDirection>();
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

        private List<AM_RunwayDirection> _servicedRunwayDirections;
        //В AIXM ArrestingGear хранит лист RunwayDirection-ов которых он обслуживает
        [CrudPropertyConfiguration(PropertyCategory = PropertyCategories.Relational, DisplayInGrid = false, PropertyRequirement = PropertyRequirements.Ignore)]
        public List<AM_RunwayDirection> ServicedRunwayDirections
        {
            get { return _servicedRunwayDirections; }
            set
            {
                _servicedRunwayDirections = value;
                SendPropertyChanged("ServicedRunwayDirections");
                SendPropertyChanged("idthr");
            }
        }

        [CrudPropertyConfiguration(PropertyCategory = PropertyCategories.Relational, SetterPropertyNames = new string[] { nameof(ServicedRunwayDirections) }, PropertyRequirement = PropertyRequirements.Mandatory)]
        public string idthr
        { get
            {
                string result = string.Join("_", ServicedRunwayDirections?.Select(r => r.Name));
                return result;
            }
        }

        [CrudPropertyConfiguration]
        public AM_Nullable<Status> status { get; set; }

        [CrudPropertyConfiguration(PropertyCategory = PropertyCategories.General, DisplayInGrid = false, PropertyRequirement = PropertyRequirements.Mandatory)]
        [JsonConverter(typeof(GeometryJsonConverter))]
        public IPolyline geoline { get; set; }

        public override Enums.Feat_Type Feattype => Enums.Feat_Type.Arresting_Gear_Location;

    }
}
