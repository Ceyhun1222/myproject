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
    [Description("ParkingStandArea")]
    [CrudFeatureConfiguration(FeatureCategory = FeatureCategories.ApronFeatureTypes)]
    public partial class AM_ParkingStandArea : AM_FeatureVerticalQuality
	{
        public AM_ParkingStandArea()
        {
            RelatedStands = new List<AM_ParkingStandLocation>();
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

        private List<AM_ParkingStandLocation> _relatedStands;
        
        [CrudPropertyConfiguration(DisplayInGrid =false, PropertyCategory = PropertyCategories.Relational, PropertyRequirement = PropertyRequirements.Ignore)]
        public List<AM_ParkingStandLocation> RelatedStands
        {
            get { return _relatedStands; }
            set
            {
                _relatedStands = value;
                SendPropertyChanged("RelatedStands");
                SendPropertyChanged("idstd");

            }
        }

        [RelationalProperty]
        [CrudPropertyConfiguration("idstd", PropertyCategory = PropertyCategories.Relational, SetterPropertyNames = new string[] { nameof(RelatedStands) }, PropertyRequirement = PropertyRequirements.Mandatory)]
        public string idstd
        {
            get
            {
                string result = string.Join("_", RelatedStands?.Select(r => r.idstd));
                return result;
            }
        }

        [CrudPropertyConfiguration]
        public AM_Nullable<SurfaceComposition> gsurftyp { get; set; }

        [CrudPropertyConfiguration]
        public AM_Nullable<string> pcn { get; set; }

        [CrudPropertyConfiguration]
        public AM_Nullable<string> restacft { get; set; }

        [CrudPropertyConfiguration]
        public AM_Nullable<Availability> jetway { get; set; }

        [CrudPropertyConfiguration]
        public AM_Nullable<FuelType> fuel { get; set; }

        [CrudPropertyConfiguration]
        public AM_Nullable<Availability> towing { get; set; }

        [CrudPropertyConfiguration]
        public AM_Nullable<Availability> docking { get; set; }

        [CrudPropertyConfiguration]
        public AM_Nullable<Availability> gndpower { get; set; }

        private AM_Apron _associatedApron;
        [CrudPropertyConfiguration(DisplayInGrid = false, PropertyCategory = PropertyCategories.Relational, PropertyRequirement = PropertyRequirements.Ignore)]
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

        [CrudPropertyConfiguration(PropertyCategory = PropertyCategories.Relational, SetterPropertyNames = new string[] { nameof(AssociatedApron) }, PropertyRequirement = PropertyRequirements.Mandatory)]
        public string idapron
        {
            get
            {
                return AssociatedApron?.Name;
            }
        }

        private AM_VerticalPolygonalStructure _polygonalStructure;

        [CrudPropertyConfiguration(DisplayInGrid = false, PropertyCategory = PropertyCategories.Relational)]
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

        [CrudPropertyConfiguration]
        public AM_Nullable<Status> status { get; set; }

        [CrudPropertyConfiguration(PropertyCategory = PropertyCategories.General, DisplayInGrid = false, PropertyRequirement = PropertyRequirements.Mandatory)]
        [JsonConverter(typeof(GeometryJsonConverter))]
        public IPolygon geopoly { get; set; }

        public override Enums.Feat_Type Feattype => Enums.Feat_Type.Parking_Stand_Area;

        [Browsable(false)]
        public override string Descriptor => idstd;

    }
}
