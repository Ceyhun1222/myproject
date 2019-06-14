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
    [Description("VerticalPointStructure")]
    [CrudFeatureConfiguration(FeatureCategory = FeatureCategories.VerticalStructureFeatureTypes)]
    public partial class AM_VerticalPointStructure : AM_FeatureVerticalQuality
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
        public AM_Nullable<VSPoint_Type> pntsttyp { get; set; }

        [CrudPropertyConfiguration]
        public AM_Nullable<Material_Type> material { get; set; }

        [CrudPropertyConfiguration(PropertyRequirement = PropertyRequirements.Mandatory)]
        public DataType<UomDistance> height { get; set; }

        [CrudPropertyConfiguration(PropertyRequirement = PropertyRequirements.Mandatory)]
        public DataType<UomDistance> elev { get; set; }

        [CrudPropertyConfiguration]
        public AM_Nullable<Availability> lighting { get; set; }

        [CrudPropertyConfiguration(PropertyRequirement = PropertyRequirements.Mandatory)]
        public DataType<UomDistance> radius { get; set; }

        [CrudPropertyConfiguration]
        public AM_Nullable<Availability> marking { get; set; }

        [CrudPropertyConfiguration(PropertyCategory = PropertyCategories.General, DisplayInGrid = false, PropertyRequirement = PropertyRequirements.Mandatory)]
        [JsonConverter(typeof(GeometryJsonConverter))]
        public IPoint geopnt { get; set; }

        public override Enums.Feat_Type Feattype => Enums.Feat_Type.Vertical_Point_Structure;

    }
}
