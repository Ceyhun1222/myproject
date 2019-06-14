using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    [Description("StandGuidanceLine")]
    [CrudFeatureConfiguration(FeatureCategory = FeatureCategories.ApronFeatureTypes)]
    public partial class AM_StandGuidanceLine : AM_FeatureVerticalQuality
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

        private AM_ParkingStandLocation _connectedStand;
        [CrudPropertyConfiguration("Connected Stand", PropertyCategory = PropertyCategories.Relational, DisplayInGrid = false, PropertyRequirement = PropertyRequirements.Ignore)]
        public AM_ParkingStandLocation ConnectedStand
        {
            get { return _connectedStand; }
            set
            {
                _connectedStand = value;
                SendPropertyChanged("ConnectedStand");
                SendPropertyChanged("idstd");
            }
        }

        [CrudPropertyConfiguration(PropertyCategory = PropertyCategories.Relational, SetterPropertyNames = new string[] { nameof(ConnectedStand) }, PropertyRequirement = PropertyRequirements.Mandatory)]
        public string idstd
        {
            get
            {                
                return ConnectedStand?.idstd.Value;
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

        [CrudPropertyConfiguration(PropertyRequirement = PropertyRequirements.Mandatory)]
        public DataType<UomDistance> wingspan { get; set; }

        //Nullable
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
        public IPolyline geoline { get; set; }

        public override Enums.Feat_Type Feattype => Enums.Feat_Type.Stand_Guidance_Line;

        [Browsable(false)]
        public override string Descriptor => idstd;


    }
}
