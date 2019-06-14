using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using Aerodrome.DataType;
using Aerodrome.Enums;
using ESRI.ArcGIS.Geometry;
using Framework.Attributes;
using Newtonsoft.Json;

namespace Aerodrome.Features
{
    [Description("RunwayElement")]
    [CrudFeatureConfiguration(FeatureCategory = FeatureCategories.RunwayFeatureTypes)]
    public partial class AM_RunwayElement : AM_FeatureVerticalQuality
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

        private AM_Runway _associatedRunway;
        [CrudPropertyConfiguration("Associated Runway", PropertyCategory = PropertyCategories.Relational, DisplayInGrid = false, PropertyRequirement = PropertyRequirements.Ignore)]
        public AM_Runway AssociatedRunway
        {
            get { return _associatedRunway; }
            set
            {
                _associatedRunway = value;
                SendPropertyChanged("AssociatedRunway");
                SendPropertyChanged("idrwy");
            }
        }

        [RelationalProperty]
        [CrudPropertyConfiguration("idrwy ", PropertyCategory = PropertyCategories.Relational, SetterPropertyNames = new string[] { nameof(AssociatedRunway) }, PropertyRequirement = PropertyRequirements.Mandatory)]
        public string idrwy
        {
            get
            {
                return AssociatedRunway?.Name;
            }
        }
        [CrudPropertyConfiguration]
        public AM_Nullable<string> pcn { get; set; }

        [CrudPropertyConfiguration]
        public AM_Nullable<string> restacft { get; set; }

        [CrudPropertyConfiguration(PropertyRequirement = PropertyRequirements.Mandatory)]
        public DataType<UomDistance> width { get; set; }

        [CrudPropertyConfiguration(PropertyRequirement = PropertyRequirements.Mandatory)]
        public DataType<UomDistance> length { get; set; }

        [CrudPropertyConfiguration(DisplayInGrid = false)]
        public AM_Nullable<SurfaceComposition> surftype { get; set; }

        [CrudPropertyConfiguration]
        public AM_Nullable<Status> status { get; set; }

        [CrudPropertyConfiguration(DisplayInGrid = false)]
        public AM_Nullable<LowVisibilityOper_Type> lvo { get; set; }

        [CrudPropertyConfiguration(DisplayInGrid = false, PropertyRequirement = PropertyRequirements.Mandatory)]
        [JsonConverter(typeof(GeometryJsonConverter))]
        public IPolygon geopoly { get; set; }


        public override Feat_Type Feattype => Feat_Type.Runway_Element;

        [Browsable(false)]
        public override string Descriptor => idrwy;
    }
}
