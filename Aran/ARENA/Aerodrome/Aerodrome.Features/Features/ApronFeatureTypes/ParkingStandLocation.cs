using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using ESRI.ArcGIS.Geometry;
using Framework.Attributes;
using Newtonsoft.Json;

namespace Aerodrome.Features
{
    [Description("ParkingStandLocation")]
    [CrudFeatureConfiguration(FeatureCategory = FeatureCategories.ApronFeatureTypes)]
    public partial class AM_ParkingStandLocation : AM_FeatureVerticalQuality
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
        public AM_Nullable<string> idstd { get; set; }

        [CrudPropertyConfiguration]
        public AM_Nullable<string> acft { get; set; }

        private AM_VerticalPolygonalStructure _polygonalStructure;
        [CrudPropertyConfiguration(DisplayInGrid = false, PropertyCategory = PropertyCategories.Relational, PropertyRequirement = PropertyRequirements.Ignore)]
        public AM_VerticalPolygonalStructure PolygonalStructure
        {
            get { return _polygonalStructure; }
            set
            {
                _polygonalStructure = value;
                SendPropertyChanged("PolygonalStructure");
                SendPropertyChanged("termref");
            }
        }

        [CrudPropertyConfiguration(PropertyCategory = PropertyCategories.Relational, SetterPropertyNames = new string[] { nameof(PolygonalStructure) }, PropertyRequirement = PropertyRequirements.Mandatory)]
        public string termref
        {
            get
            {               
                return PolygonalStructure?.ident.Value;
            }
        }

        [CrudPropertyConfiguration(PropertyCategory = PropertyCategories.General, DisplayInGrid = false, PropertyRequirement = PropertyRequirements.Mandatory)]
        [JsonConverter(typeof(GeometryJsonConverter))]
        public IPoint geopnt { get; set; }


        public override Enums.Feat_Type Feattype => Enums.Feat_Type.Parking_Stand_Location;

        [Browsable(false)]
        public override string Descriptor => idstd?.Value;
    }
}
