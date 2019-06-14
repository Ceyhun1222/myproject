using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESRI.ArcGIS.Geometry;
using Aerodrome.DataType;
using Aerodrome.Enums;
using System.ComponentModel.DataAnnotations;
using Framework.Attributes;
using Newtonsoft.Json;
using System.ComponentModel;

namespace Aerodrome.Features
{
    [Description("RunwayCenterlinePoint")]
    [CrudFeatureConfiguration(FeatureCategory = FeatureCategories.RunwayFeatureTypes)]
    public partial class AM_RunwayCenterlinePoint : AM_FeatureVerticalQuality
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
        [CrudPropertyConfiguration(PropertyCategory = PropertyCategories.Relational, DisplayInGrid = false, PropertyRequirement = PropertyRequirements.Ignore)]
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
        [CrudPropertyConfiguration(PropertyCategory = PropertyCategories.Relational, SetterPropertyNames = new string[] { nameof(AssociatedRunway) }, PropertyRequirement = PropertyRequirements.Mandatory)]
        public string idrwy
        {
            get
            {
                return AssociatedRunway?.Name;
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
            }
        }

        [CrudPropertyConfiguration(PropertyRequirement = PropertyRequirements.Mandatory)]
        public DataType<UomDistance> ellipse { get; set; }

        [CrudPropertyConfiguration(PropertyRequirement = PropertyRequirements.Mandatory)]
        public DataType<UomDistance> geound { get; set; }

        [CrudPropertyConfiguration(PropertyRequirement = PropertyRequirements.Mandatory)]
        public DataType<UomDistance> elev { get; set; }

        [CrudPropertyConfiguration(PropertyCategory = PropertyCategories.General, DisplayInGrid = false, PropertyRequirement = PropertyRequirements.Mandatory)]
        [JsonConverter(typeof(GeometryJsonConverter))]
        public IPoint geopnt { get; set; }

        public override Enums.Feat_Type Feattype => Enums.Feat_Type.Runway_Centerline_Point;

    }
}
