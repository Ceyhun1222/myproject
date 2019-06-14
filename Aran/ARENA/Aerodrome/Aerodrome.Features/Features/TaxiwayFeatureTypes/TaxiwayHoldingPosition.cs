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
    [Description("TaxiwayHoldingPosition")]
    [CrudFeatureConfiguration(FeatureCategory = FeatureCategories.TaxiwayFeatureTypes)]
    public partial class AM_TaxiwayHoldingPosition : AM_FeatureVerticalQuality
	{
        public AM_TaxiwayHoldingPosition()
        {
            ProtectedRunways = new List<AM_Runway>();
            ProtectedTaxiways = new List<AM_Taxiway>();
        }

        // Added by me
        //При генерации базы игнорировать
        //В инпут форме выбирать из листБокса нужные Runway и Taxiway
        //idarpt, idp

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

        private List<AM_Runway> _protectedRunways;
        [CrudPropertyConfiguration("Protected Runways(idp)", PropertyCategory = PropertyCategories.Relational, DisplayInGrid = false, PropertyRequirement = PropertyRequirements.Ignore)]
        public List<AM_Runway> ProtectedRunways
        {
            get { return _protectedRunways; }
            set
            {
                _protectedRunways = value;
                SendPropertyChanged("ProtectedRunways");
                SendPropertyChanged("idp");
            }
        }

        private List<AM_Taxiway> _protectedTaxiways;
        [CrudPropertyConfiguration("Protected Taxiways(idp)", PropertyCategory = PropertyCategories.Relational, DisplayInGrid = false, PropertyRequirement = PropertyRequirements.Ignore)]
        public List<AM_Taxiway> ProtectedTaxiways
        {
            get { return _protectedTaxiways; }
            set
            {
                _protectedTaxiways = value;
                SendPropertyChanged("ProtectedTaxiways");
                SendPropertyChanged("idp");
            }
        }


        [CrudPropertyConfiguration(PropertyCategory = PropertyCategories.Relational, SetterPropertyNames = new string[] { nameof(ProtectedRunways), nameof(ProtectedTaxiways) }, PropertyRequirement = PropertyRequirements.Mandatory)]
        public string idp
        {
            get
            {
                List<string> featureNames = ProtectedRunways?.Select(r => r.Name).ToList();
                featureNames.AddRange(ProtectedTaxiways?.Select(r => r.Name));
                string result = string.Join("_", featureNames);
                return result;
            }
        }

        private AM_TaxiwayGuidanceLine _associatedGuidanceLine;
        [CrudPropertyConfiguration("Associated GuidanceLine(idlin)", PropertyCategory = PropertyCategories.Relational, DisplayInGrid = false, PropertyRequirement = PropertyRequirements.Ignore)]
        public AM_TaxiwayGuidanceLine AssociatedGuidanceLine
        {
            get { return _associatedGuidanceLine; }
            set
            {
                _associatedGuidanceLine = value;
                SendPropertyChanged("AssociatedGuidanceLine");
                SendPropertyChanged("idlin");
            }
        }

        [RelationalProperty]
        [CrudPropertyConfiguration(PropertyCategory = PropertyCategories.Relational, SetterPropertyNames = new string[] { nameof(AssociatedGuidanceLine) }, PropertyRequirement = PropertyRequirements.Mandatory)]
        public string idlin
        {
            get
            {              
                return AssociatedGuidanceLine?.idlin.Value;
            }
        }

        [CrudPropertyConfiguration]
        public AM_Nullable<Status> status { get; set; }

        [CrudPropertyConfiguration]
        public AM_Nullable<Catstop> catstop { get; set; }

        [CrudPropertyConfiguration]
        public AM_Nullable<string> rwyahtxt { get; set; }

        [CrudPropertyConfiguration]
        public AM_Nullable<Holding_Direction> holddir { get; set; }

        [CrudPropertyConfiguration]
        public AM_Nullable<AipPublishedType> AipPublished { get; set; }

        [CrudPropertyConfiguration]
        public AM_Nullable<MarkingType> marktype { get; set; }

        [CrudPropertyConfiguration]
        public AM_Nullable<LowVisibilityOper_Type> lvo { get; set; }

        [CrudPropertyConfiguration]
        public AM_Nullable<RwyEntryLightInstalled> rwelight { get; set; }

        [CrudPropertyConfiguration(PropertyCategory = PropertyCategories.General, DisplayInGrid = false, PropertyRequirement = PropertyRequirements.Mandatory)]
        [JsonConverter(typeof(GeometryJsonConverter))]
        public IPolyline geoline { get; set; }

        public override Enums.Feat_Type Feattype => Enums.Feat_Type.Taxiway_Holding_Position;

        [Browsable(false)]
        public override string Descriptor => idp;

    }
}
