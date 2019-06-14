using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aerodrome.Enums;
using Aerodrome.DataType;
using ESRI.ArcGIS.Geometry;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using ESRI.ArcGIS.esriSystem;

using Framework.Attributes;
using System.ComponentModel;

namespace Aerodrome.Features
{
    [Description("AerodromeReferencePoint")]
    [CrudFeatureConfiguration(FeatureCategory = FeatureCategories.ApronFeatureTypes)]
    public partial class AM_AerodromeReferencePoint : AM_FeatureVerticalQuality
    {


        private AM_Nullable<string> _idarpt;
        [MaxLength(5)]
        [RelationalProperty]
        [CrudPropertyConfiguration(PropertyRequirement = PropertyRequirements.Mandatory)]
        public AM_Nullable<string> idarpt
        {
            get { return _idarpt; }
            set
            {
                _idarpt = value;
                SendPropertyChanged("idarpt");
            }
        }

        private string _name;
        [CrudPropertyConfiguration(PropertyRequirement = PropertyRequirements.Mandatory)]
        public string name
        {
            get { return _name; }
            set
            {
                _name = value;
                SendPropertyChanged("name");              
            }
        }

        private string _organization;
        [CrudPropertyConfiguration(PropertyRequirement = PropertyRequirements.Mandatory)]
        public string Organization
        {
            get
            {
                return _organization;
            }
            set
            {
                _organization = value;               
            }
        }

        [MaxLength(4)]
        [CrudPropertyConfiguration]
        public AM_Nullable<string> iata { get; set; }

        [CrudPropertyConfiguration(PropertyRequirement = PropertyRequirements.Mandatory)]
        public DataType<UomDistance> elev { get; set; }

        [CrudPropertyConfiguration]
        public AM_Nullable<OperSystem_Type> systems { get; set; }

        [CrudPropertyConfiguration(PropertyRequirement = PropertyRequirements.Mandatory)]
        public DataType<UomDistance> mintelev { get; set; }

        [CrudPropertyConfiguration(PropertyRequirement = PropertyRequirements.Mandatory)]
        public DataType<UomDistance> maxtelev { get; set; }

        [CrudPropertyConfiguration(PropertyRequirement = PropertyRequirements.Mandatory)]
        public DataType<UomDistance> devtelev { get; set; }

        [CrudPropertyConfiguration(PropertyRequirement = PropertyRequirements.Mandatory)]
        public DataType<UomSlope> maxtgrad { get; set; }

        [CrudPropertyConfiguration(PropertyRequirement = PropertyRequirements.Mandatory)]
        public DataType<UomSlope> devtgrad { get; set; }

        [CrudPropertyConfiguration(PropertyRequirement = PropertyRequirements.Mandatory)]
        public DataType<UomDistance> avgtelev { get; set; }

        [CrudPropertyConfiguration("geopnt", DisplayInGrid = false, PropertyRequirement = PropertyRequirements.Mandatory)]
        [JsonConverter(typeof(GeometryJsonConverter))]
        public IPoint geopnt { get; set; }

        public override Feat_Type Feattype => Enums.Feat_Type.Aerodrome_Reference_Point;

        [Browsable(false)]
        public override string Descriptor => idarpt?.Value;


    }

}