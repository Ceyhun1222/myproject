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
    //All properties from GuidanceLines
    [Description("AsrnNode")]
    [CrudFeatureConfiguration(FeatureCategory = FeatureCategories.AerodromeSurfaceRoutingNetworkFeatureTypes)]
    public partial class AM_AsrnNode : AM_AsrnBase
    {
        public AM_AsrnNode()
        {
            nodetype = new AM_Nullable<Node_Type> { NilReason = NilReason.NotEntered };
            catstop = new AM_Nullable<Catstop> { NilReason = NilReason.NotEntered };
            featref = new AM_Nullable<string> { NilReason = NilReason.NotEntered };
            idnetwrk = new AM_Nullable<string> { NilReason = NilReason.NotEntered };
            endfeat = new AM_Nullable<DateTime> { NilReason = NilReason.NotEntered };
            endvalid = new AM_Nullable<DateTime> { NilReason = NilReason.NotEntered };
            source = new AM_Nullable<string> { NilReason = NilReason.NotEntered };
            interp = new AM_Nullable<AM_InterpretationType> { NilReason = NilReason.NotEntered };
            revdate = new AM_Nullable<DateTime> { NilReason = NilReason.NotEntered };
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
        [CrudPropertyConfiguration(SetterPropertyNames = new string[] { nameof(RelatedARP) })]
        public string idarpt
        {
            get
            {
                return RelatedARP?.idarpt.Value;
            }
        }

        public override Feat_Type Feattype => Feat_Type.Asrn_Node;

        [CrudPropertyConfiguration]
        public string idthr { get; set; }

        [CrudPropertyConfiguration]
        public AM_Nullable<Node_Type> nodetype { get; set; }

        //Nullable
        [CrudPropertyConfiguration]
        public string deicegrp
        {
            get
            {
                if (RelatedDeicGroup is null || RelatedDeicGroup.ident.IsNull)
                    return null;
                return RelatedDeicGroup.ident.Value;
            }
        }

        //Nullable
        public AM_VerticalPolygonalStructure PolygonalStructure { get; set; }

        [CrudPropertyConfiguration]
        public string termref
        {
            get
            {
                if (PolygonalStructure is null || PolygonalStructure.ident.IsNull)
                    return null;
                return PolygonalStructure.ident.Value;
            }
        }

        [CrudPropertyConfiguration]
        public AM_Nullable<Catstop> catstop { get; set; }

        [CrudPropertyConfiguration]
        public AM_Nullable<string> featref { get; set; }

        [CrudPropertyConfiguration(DisplayInGrid = false)]
        [JsonConverter(typeof(GeometryJsonConverter))]
        public IPoint geopnt { get; set; }
    }
}
