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
    [Description("PositionMarking")]
    [CrudFeatureConfiguration(FeatureCategory = FeatureCategories.TaxiwayFeatureTypes)]
    public partial class AM_PositionMarking : AM_FeatureBase
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

        private AM_TaxiwayHoldingPosition _markedTaxiHold;
        [CrudPropertyConfiguration("Marked TaxiwayHoldingPosition", PropertyCategory = PropertyCategories.Relational, DisplayInGrid = false, PropertyRequirement = PropertyRequirements.Ignore)]
        public AM_TaxiwayHoldingPosition MarkedTaxiHold
        {
            get { return _markedTaxiHold; }
            set
            {
                _markedTaxiHold = value;
                SendPropertyChanged("MarkedTaxiHold");
                SendPropertyChanged("pmfeat");
            }
        }


        [CrudPropertyConfiguration(PropertyCategory = PropertyCategories.Relational, SetterPropertyNames = new string[] { nameof(MarkedTaxiHold) })]
        public AM_Nullable<string> pmfeat
        {
            get
            {
                //TODO:На самом деле здесь надо возвращать idnumber
                return MarkedTaxiHold?.idlin;
            }
        }

        [CrudPropertyConfiguration]
        public AM_Nullable<string> idpm { get; set; }//свой Designator

        [CrudPropertyConfiguration]
        public AM_Nullable<PmType> pmtyp { get; set; }

        [CrudPropertyConfiguration]
        public AM_Nullable<LowVisibilityOper_Type> lvo { get; set; }

        [CrudPropertyConfiguration(PropertyCategory = PropertyCategories.General, DisplayInGrid = false, PropertyRequirement = PropertyRequirements.Mandatory)]
        [JsonConverter(typeof(GeometryJsonConverter))]
        public IPoint geopnt { get; set; }

        public override Enums.Feat_Type Feattype => Enums.Feat_Type.Position_Marking;

        [Browsable(false)]
        public override string Descriptor => idpm?.Value;

    }
}