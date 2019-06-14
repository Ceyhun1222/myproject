using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using Aerodrome.Enums;
using BusinessCore.Validation;
using BusinessCore.Validation.Exceptions;
using ESRI.ArcGIS.Geometry;
using Framework.Attributes;

//using Framework.Stasy.SyncProvider;
using Newtonsoft.Json;

namespace Aerodrome.Features
{
    [Description("TaxiwayElement")]
    [CrudFeatureConfiguration(FeatureCategory = FeatureCategories.TaxiwayFeatureTypes)]
    public partial class AM_TaxiwayElement : AM_FeatureVerticalQuality
    {
        public AM_TaxiwayElement()
        {
            geopoly = new PolygonClass();
            CrossedTaxiways = new List<AM_Taxiway>();
            AttachedTaxiways = new List<AM_Taxiway>();

            //AddRule(new ValidationRule(
            //    new ValidationException("Please fill in a idlin."),
            //    ValidateIdlinRequired));
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


        [RelationalProperty]
        [CrudPropertyConfiguration("idlin ", PropertyCategory = PropertyCategories.Relational, SetterPropertyNames = new string[] { nameof(AssociatedTaxiway) }, PropertyRequirement = PropertyRequirements.Mandatory)]
        public string idlin
        {
            get
            {
                return AssociatedTaxiway?.Name;
            }
        }

        private List<AM_Taxiway> _crossedTwyList;
        //Nullable

        [CrudPropertyConfiguration("Crossed Taxiways", PropertyCategory = PropertyCategories.Relational, DisplayInGrid = false, DisplayInModification = true, PropertyRequirement = PropertyRequirements.Ignore)]
        public List<AM_Taxiway> CrossedTaxiways
        {
            get
            {
                return _crossedTwyList;
            }
            set
            {
                _crossedTwyList = value;
                SendPropertyChanged("CrossedTwyList");
                SendPropertyChanged("idcross");
            }
        }
        [CrudPropertyConfiguration("idcross", PropertyCategory = PropertyCategories.Relational, SetterPropertyNames = new string[] { nameof(CrossedTaxiways) })]
        public AM_Nullable<string> idcross
        {
            get
            {
                string result = string.Join("_", CrossedTaxiways?.Select(r => r.Name));
                return result;
            }
        }

        private List<AM_Taxiway> _attachedTaxiways;
        [CrudPropertyConfiguration("Attached Taxiways", PropertyCategory = PropertyCategories.Relational, DisplayInGrid = false, PropertyRequirement = PropertyRequirements.Ignore)]
        public List<AM_Taxiway> AttachedTaxiways
        {
            get { return _attachedTaxiways; }
            set
            {
                _attachedTaxiways = value;
                SendPropertyChanged("AttachedTaxiways");
                SendPropertyChanged("idinter");
            }
        }

        [CrudPropertyConfiguration("idinter", PropertyCategory = PropertyCategories.Relational, SetterPropertyNames =new string[] { nameof(AttachedTaxiways) })]
        public AM_Nullable<string> idinter
        {
            get
            {
                string result = string.Join("_", AttachedTaxiways?.Select(r => r.Name));
                return result;
            }
        }


        private AM_Apron _associatedApron;
        //Nullable        
        [CrudPropertyConfiguration("Associated Apron", PropertyCategory = PropertyCategories.Relational, DisplayInGrid = false, PropertyRequirement = PropertyRequirements.Ignore)]
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

        [CrudPropertyConfiguration("idapron", PropertyCategory = PropertyCategories.Relational, SetterPropertyNames = new string[] { nameof(AssociatedApron) })]
        public AM_Nullable<string> idapron
        {
            get
            {
                return AssociatedApron?.Name;
            }
        }

        
        [CrudPropertyConfiguration(PropertyCategory =PropertyCategories.General)]
        public AM_Nullable<SurfaceComposition> gsurftyp { get; set; }

        [CrudPropertyConfiguration(PropertyCategory = PropertyCategories.General)]
        public AM_Nullable<string> pcn { get; set; }

        [CrudPropertyConfiguration(DisplayInGrid = false)]
        public AM_Nullable<BridgeType> bridge { get; set; }

        [CrudPropertyConfiguration("Restacft")]
        public AM_Nullable<string> restacft { get; set; }

        [CrudPropertyConfiguration("Status")]
        public AM_Nullable<Status> status { get; set; }

        [CrudPropertyConfiguration("LVO",DisplayInGrid = false)]
        public AM_Nullable<LowVisibilityOper_Type> lvo { get; set; }

        private AM_RunwayElement _relatedRunway;
        [CrudPropertyConfiguration("RelatedFeature", PropertyCategory = PropertyCategories.Relational, DisplayInGrid = false, PropertyRequirement = PropertyRequirements.Ignore)]
        public AM_RunwayElement RelatedFeature
        {
            get { return _relatedRunway; }
            set
            {
                _relatedRunway = value;
                SendPropertyChanged("RelatedFeature");
                SendPropertyChanged("idbase");
                SendPropertyChanged("featbase");
            }
        }

        [CrudPropertyConfiguration("idbase", PropertyCategory = PropertyCategories.Relational, SetterPropertyNames = new string[] { nameof(RelatedFeature) })]
        public AM_Nullable<string> idbase
        {
            get
            {
                if (RelatedFeature != null)
                    return RelatedFeature.idrwy;
                AM_Nullable<string> result = new AM_Nullable<string>();
                result.NilReason = NilReason.NotEntered;
                return result;
            }
        }
        [CrudPropertyConfiguration("featbase", PropertyCategory = PropertyCategories.Relational)]
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


        [CrudPropertyConfiguration(PropertyCategory = PropertyCategories.General,DisplayInGrid =false, PropertyRequirement = PropertyRequirements.Mandatory)]
        [JsonConverter(typeof(GeometryJsonConverter))]
        public IPolygon geopoly
        {
            get;set;
        }

        public override Enums.Feat_Type Feattype => Enums.Feat_Type.Taxiway_Element;

        [Browsable(false)]
        public override string Descriptor => idlin;

        private bool ValidateIdlinRequired()
        {
            return ValidateStringNotEmpty(idlin);
        }


    }
}
