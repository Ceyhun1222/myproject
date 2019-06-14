using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
//using Aerodrome.Context;
using Aerodrome.Enums;
using Aerodrome.Features;
using ESRI.ArcGIS.Geometry;
using Framework.Attributes;
using Newtonsoft.Json;
//using Framework.Stasy;


namespace Aerodrome.Features
{
    [Description("ApronElement")]
    [CrudFeatureConfiguration(FeatureCategory = FeatureCategories.ApronFeatureTypes)]
    public partial class AM_ApronElement : AM_FeatureVerticalQuality
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

        private AM_Apron _associatedApron;
        [CrudPropertyConfiguration(DisplayInGrid =false, PropertyCategory = PropertyCategories.Relational, PropertyRequirement = PropertyRequirements.Ignore)]
        public AM_Apron AssociatedApron
        {
            get { return _associatedApron; }
            set
            {
                _associatedApron = value;
                SendPropertyChanged("AssociatedApron");
                SendPropertyChanged("idapron");
            }
        }

        [RelationalProperty]
        [CrudPropertyConfiguration("idapron", PropertyCategory = PropertyCategories.Relational, SetterPropertyNames = new string[] { nameof(AssociatedApron) }, PropertyRequirement = PropertyRequirements.Mandatory)]
        public string idapron
        {
            get
            {
                return AssociatedApron?.Name;
            }
        }

        [CrudPropertyConfiguration]
        public AM_Nullable<SurfaceComposition> gsurftyp { get; set; }

        [CrudPropertyConfiguration]
        public AM_Nullable<string> pcn { get; set; }

        [CrudPropertyConfiguration]
        public AM_Nullable<string> restacft { get; set; }

        [CrudPropertyConfiguration]
        public AM_Nullable<Status> status { get; set; }

        [CrudPropertyConfiguration]
        public AM_Nullable<Apron_Type> aprontyp { get; set; }

        [CrudPropertyConfiguration(PropertyCategory = PropertyCategories.General, DisplayInGrid = false, PropertyRequirement = PropertyRequirements.Mandatory)]
        [JsonConverter(typeof(GeometryJsonConverter))]
        public IPolygon geopoly { get; set; }

        public override Enums.Feat_Type Feattype => Enums.Feat_Type.Apron_Element;

        [Browsable(false)]
        public override string Descriptor => idapron;
    }
}
