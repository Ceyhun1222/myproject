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
    [Description("TaxiwayIntersectionMarking")]
    [CrudFeatureConfiguration(FeatureCategory = FeatureCategories.TaxiwayFeatureTypes)]
    public partial class AM_TaxiwayIntersectionMarking : AM_FeatureVerticalQuality
	{
        public AM_TaxiwayIntersectionMarking()
        {
            ProtectedTaxiways = new List<AM_Taxiway>();
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

        private List<AM_Taxiway> _protectedTaxiways;
        //В инпут форме выбирать из листБокса нужные Runway и Taxiway
        [CrudPropertyConfiguration(PropertyCategory = PropertyCategories.Relational, DisplayInGrid = false, PropertyRequirement = PropertyRequirements.Ignore)]
        public List<AM_Taxiway> ProtectedTaxiways
        {
            get { return _protectedTaxiways; }
            set
            {
                _protectedTaxiways = value;
                SendPropertyChanged("ProtectedTaxiways");
                SendPropertyChanged("idp");
            }
        }

        [CrudPropertyConfiguration(PropertyCategory = PropertyCategories.Relational, SetterPropertyNames = new string[] { nameof(ProtectedTaxiways) }, PropertyRequirement = PropertyRequirements.Mandatory)]
        public string idp
        {
            get
            {
                string result = string.Join("_", ProtectedTaxiways?.Select(r => r.Name));

                return result;
            }
        }

        private AM_Taxiway _markedTaxiway;
        [CrudPropertyConfiguration(PropertyCategory = PropertyCategories.Relational, DisplayInGrid = false, PropertyRequirement = PropertyRequirements.Ignore)]
        public AM_Taxiway MarkedTaxiway
        {
            get { return _markedTaxiway; }
            set
            {
                _markedTaxiway = value;
                SendPropertyChanged("MarkedTaxiway");
                SendPropertyChanged("idlin");
            }
        }

        [CrudPropertyConfiguration(PropertyCategory = PropertyCategories.Relational, SetterPropertyNames = new string[] { nameof(MarkedTaxiway) })]
        public AM_Nullable<string> idlin
        {
            get
            {
                return MarkedTaxiway?.Name;
            }
        }

        [CrudPropertyConfiguration]
        public AM_Nullable<Holding_Direction> holddir { get; set; }

        [CrudPropertyConfiguration]
        public AM_Nullable<MarkingType> marktype { get; set; }

        [CrudPropertyConfiguration]
        public AM_Nullable<AipPublishedType> aippub { get; set; }

        [CrudPropertyConfiguration(PropertyCategory = PropertyCategories.General, DisplayInGrid = false, PropertyRequirement = PropertyRequirements.Mandatory)]
        [JsonConverter(typeof(GeometryJsonConverter))]
        public IPolyline geoline { get; set; }

        public override Enums.Feat_Type Feattype => Enums.Feat_Type.Taxiway_Intersection_Marking;

        [Browsable(false)]
        public override string Descriptor => idlin?.Value;
    }
}