﻿using System;
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
    [Description("SurveyControlPoint")]
    [CrudFeatureConfiguration(FeatureCategory = FeatureCategories.SurveyControlPointFeatureTypes)]
    public partial class AM_SurveyControlPoint : AM_FeatureVerticalQuality
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
        [CrudPropertyConfiguration]
        public AM_Nullable<string> idsurv { get; set; }

        [CrudPropertyConfiguration]
        public AM_Nullable<string> coord { get; set; }

        [CrudPropertyConfiguration]
        public AM_Nullable<string> hdatum { get; set; }

        [CrudPropertyConfiguration]
        public AM_Nullable<string> spheroid { get; set; }

        [CrudPropertyConfiguration]
        public AM_Nullable<string> vdatum { get; set; }

        [CrudPropertyConfiguration]
        public AM_Nullable<string> project { get; set; }

        [CrudPropertyConfiguration(PropertyCategory = PropertyCategories.General, DisplayInGrid = false, PropertyRequirement = PropertyRequirements.Mandatory)]
        [JsonConverter(typeof(GeometryJsonConverter))]
        public IPoint geopnt { get; set; }

        public override Feat_Type Feattype => Enums.Feat_Type.Survey_Control_Point;

    }
}