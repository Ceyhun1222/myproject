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
    [Description("RunwayExitLine")]
    [CrudFeatureConfiguration(FeatureCategory = FeatureCategories.RunwayFeatureTypes)]
    public partial class AM_RunwayExitLine : AM_FeatureVerticalQuality
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

        [CrudPropertyConfiguration(PropertyCategory = PropertyCategories.Relational, SetterPropertyNames = new string[] { nameof(AssociatedRunway) }, PropertyRequirement = PropertyRequirements.Mandatory)]
        public string idrwy
        {
            get
            {
                return AssociatedRunway?.Name;
            }
        }

        private AM_Taxiway _associatedTwy;
        [CrudPropertyConfiguration("Associated Taxiway", PropertyCategory = PropertyCategories.Relational, DisplayInGrid = false, PropertyRequirement = PropertyRequirements.Ignore)]
        public AM_Taxiway AssociatedTaxiway
        {
            get
            {
                return _associatedTwy;
            }
            set
            {
                _associatedTwy = value;
                SendPropertyChanged("AssociatedTaxiway");
                SendPropertyChanged("idlin");

            }
        }


        [CrudPropertyConfiguration("idlin ", PropertyCategory = PropertyCategories.Relational, SetterPropertyNames = new string[] { nameof(AssociatedTaxiway) }, PropertyRequirement = PropertyRequirements.Mandatory)]
        public string idlin
        {
            get
            {
                return AssociatedTaxiway?.Name;
            }
        }

        [CrudPropertyConfiguration]
        public AM_Nullable<Status> status { get; set; }

        [CrudPropertyConfiguration]
        public AM_Nullable<AM_Color> color { get; set; }

        [CrudPropertyConfiguration]
        public AM_Nullable<AM_Style> style { get; set; }

        [CrudPropertyConfiguration]
        public AM_Nullable<AM_Direction> direc { get; set; }

        [CrudPropertyConfiguration]
        public AM_Nullable<Rwy_Exit_Type> exittype { get; set; }

        [CrudPropertyConfiguration(PropertyCategory = PropertyCategories.General, DisplayInGrid = false, PropertyRequirement = PropertyRequirements.Mandatory)]
        [JsonConverter(typeof(GeometryJsonConverter))]
        public IPolyline geoline { get; set; }

        public override Enums.Feat_Type Feattype => Enums.Feat_Type.Runway_ExitLine;

    }
}
