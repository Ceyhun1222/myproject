using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aerodrome.Enums;
using Aerodrome.DataType;
using ESRI.ArcGIS.Geometry;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Framework.Attributes;
using Newtonsoft.Json;

namespace Aerodrome.Features
{
    [Description("VerticalPolygonalStructure")]
    [CrudFeatureConfiguration(FeatureCategory = FeatureCategories.VerticalStructureFeatureTypes)]
    public partial class AM_VerticalPolygonalStructure : AM_FeatureVerticalQuality
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

        [CrudPropertyConfiguration]
        public AM_Nullable<VSPolygon_Type> plysttyp { get; set; }

        [CrudPropertyConfiguration]
        public AM_Nullable<Material_Type> material { get; set; }

        [CrudPropertyConfiguration(PropertyRequirement = PropertyRequirements.Mandatory)]
        public DataType<UomDistance> height { get; set; }

        [RelationalProperty]
        [CrudPropertyConfiguration]
        public AM_Nullable<string> ident { get; set; }

        [CrudPropertyConfiguration(PropertyRequirement = PropertyRequirements.Mandatory)]
        public DataType<UomDistance> elev { get; set; }

        [CrudPropertyConfiguration(PropertyCategory = PropertyCategories.General, DisplayInGrid = false, PropertyRequirement = PropertyRequirements.Mandatory)]
        [JsonConverter(typeof(GeometryJsonConverter))]
        public IPolygon geopoly { get; set; }

        public override Enums.Feat_Type Feattype => Enums.Feat_Type.Vertical_Polygonal_Structure;

        [Browsable(false)]
        public override string Descriptor => ident?.Value;
    }
}