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
    [Description("Stopway")]
    [CrudFeatureConfiguration(FeatureCategory = FeatureCategories.RunwayFeatureTypes)]
    public partial class AM_Stopway : AM_FeatureVerticalQuality
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

        private AM_RunwayDirection _protectedRunwayDirection;
        [CrudPropertyConfiguration(PropertyCategory = PropertyCategories.Relational, DisplayInGrid = false, PropertyRequirement = PropertyRequirements.Ignore)]
        public AM_RunwayDirection ProtectedRunwayDirection
        {
            get { return _protectedRunwayDirection; }
            set
            {
                _protectedRunwayDirection = value;
                SendPropertyChanged();
                SendPropertyChanged(nameof(idthr));
            }
        }

        [CrudPropertyConfiguration(PropertyCategory = PropertyCategories.Relational, SetterPropertyNames = new string[] { nameof(ProtectedRunwayDirection) }, PropertyRequirement = PropertyRequirements.Mandatory)]
        public string idthr
        {
            get
            {
                return ProtectedRunwayDirection?.Name;
            }
        }

        [CrudPropertyConfiguration]
        public AM_Nullable<StatusOperation> status { get; set; }

        [CrudPropertyConfiguration]
        public AM_Nullable<SurfaceComposition> surftype { get; set; }

        [CrudPropertyConfiguration(PropertyCategory = PropertyCategories.General, DisplayInGrid = false, PropertyRequirement = PropertyRequirements.Mandatory)]
        [JsonConverter(typeof(GeometryJsonConverter))]
        public IPolygon geopoly { get; set; }

        public override Enums.Feat_Type Feattype => Enums.Feat_Type.Stopway;

    }
}
