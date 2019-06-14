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
    [Description("ServiceRoad")]
    [CrudFeatureConfiguration(FeatureCategory = FeatureCategories.ApronFeatureTypes)]
    public partial class AM_ServiceRoad : AM_FeatureVerticalQuality
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

        private AM_AbstractFeature _relatedFeature;
        [AllowableTypes(typeof(AM_ApronElement), typeof(AM_TaxiwayElement), typeof(AM_ParkingStandArea))]
        [CrudPropertyConfiguration("RelatedFeature", PropertyCategory = PropertyCategories.Relational, DisplayInGrid = false, PropertyRequirement = PropertyRequirements.Ignore)]
        public AM_AbstractFeature RelatedFeature
        {
            get { return _relatedFeature; }
            set
            {
                _relatedFeature = value;
                SendPropertyChanged("RelatedFeature");
                SendPropertyChanged("idbase");
                SendPropertyChanged("featbase");

            }
        }

        [CrudPropertyConfiguration(PropertyCategory = PropertyCategories.Relational, SetterPropertyNames = new string[] { nameof(RelatedFeature) })]
        public AM_Nullable<string> idbase
        {
            get
            {
                if (RelatedFeature != null)
                {
                    if (RelatedFeature is AM_ApronElement)
                        return ((AM_ApronElement)RelatedFeature).idapron;
                    else if (RelatedFeature is AM_TaxiwayElement)
                        return ((AM_TaxiwayElement)RelatedFeature).idlin;                  
                    else if (RelatedFeature is AM_ParkingStandArea)
                        return ((AM_ParkingStandArea)RelatedFeature).idstd;
                }

                AM_Nullable<string> result = new AM_Nullable<string>();
                result.NilReason = NilReason.NotEntered;
                return result;
            }
        }

        //Элемент на котором стоит эта дорога
        // ApronElement, TaxiwayElement, ParkingStandArea
        [CrudPropertyConfiguration(PropertyCategory = PropertyCategories.Relational)]
        public AM_Nullable<Feat_Base> featbase
        {
            get
            {
                if (RelatedFeature != null)
                {
                    Feat_Base outValue;
                    bool isSuccess = Enum.TryParse(RelatedFeature.Feattype.ToString(), out outValue);
                    if (isSuccess)
                        return outValue;
                    else
                    {
                        AM_Nullable<Feat_Base> unknownRes = new AM_Nullable<Feat_Base>();
                        unknownRes.NilReason = NilReason.Unknown;
                        return unknownRes;
                    }
                }

                AM_Nullable<Feat_Base> result = new AM_Nullable<Feat_Base>();
                result.NilReason = NilReason.NotEntered;
                return result;
            }
        }

        [CrudPropertyConfiguration]
        public AM_Nullable<SurfaceComposition> surftype { get; set; }

        [CrudPropertyConfiguration(PropertyCategory = PropertyCategories.General, DisplayInGrid = false, PropertyRequirement = PropertyRequirements.Mandatory)]
        [JsonConverter(typeof(GeometryJsonConverter))]
        public IPolygon geopoly { get; set; }

        public override Enums.Feat_Type Feattype => Enums.Feat_Type.Service_Road;

        [Browsable(false)]
        public override string Descriptor => idbase?.Value;
    }
}