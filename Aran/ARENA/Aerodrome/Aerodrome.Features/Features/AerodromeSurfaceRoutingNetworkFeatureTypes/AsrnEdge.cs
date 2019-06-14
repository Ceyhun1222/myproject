using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using Aerodrome.DataType;
using Aerodrome.Enums;
using Aerodrome.Features;
using ESRI.ArcGIS.Geometry;
using Framework.Attributes;
using Newtonsoft.Json;

namespace Aerodrome.Features
{
    [Description("AsrnEdge")]
    [CrudFeatureConfiguration(FeatureCategory = FeatureCategories.AerodromeSurfaceRoutingNetworkFeatureTypes)]
    public partial class AM_AsrnEdge : AM_AsrnBase
    {
        public AM_AsrnEdge()
        {
            direc = new AM_Nullable<AM_Direction> { NilReason = NilReason.NotEntered };
            edgederv = new AM_Nullable<Edgederv> { NilReason = NilReason.NotEntered };
            edgetype = new AM_Nullable<EdgeType> { NilReason = NilReason.NotEntered };
            idbase = new AM_Nullable<string> { NilReason = NilReason.NotEntered };
            pcn = new AM_Nullable<string> { NilReason = NilReason.NotEntered };
            restacft= new AM_Nullable<string> { NilReason = NilReason.NotEntered };
            idnetwrk= new AM_Nullable<string> { NilReason = NilReason.NotEntered };
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

        public override Enums.Feat_Type Feattype => Enums.Feat_Type.Asrn_Edge;

        public AM_Nullable<AM_Direction> direc { get; set; }

        [CrudPropertyConfiguration]       
        public string node1ref { get; set; }

        [CrudPropertyConfiguration]        
        public string node2ref { get; set; }

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

        [CrudPropertyConfiguration]
        public AM_Nullable<Edgederv> edgederv { get; set; }

        [CrudPropertyConfiguration]
        public AM_Nullable<EdgeType> edgetype { get; set; }

        [CrudPropertyConfiguration]
        public AM_Nullable<string> idbase { get; set; }

        [CrudPropertyConfiguration]
        public AM_Nullable<string> pcn { get; set; }

        [CrudPropertyConfiguration]
        public AM_Nullable<string> restacft { get; set; }

        [CrudPropertyConfiguration]
        public DataType<UomDistance> wingspan { get; set; }

        [CrudPropertyConfiguration]
        public DataType<UomBearing> curvatur { get; set; }

        [CrudPropertyConfiguration]
        public DataType<UomDistance> edgelen { get; set; }

        [CrudPropertyConfiguration(DisplayInGrid = false)]
        [JsonConverter(typeof(GeometryJsonConverter))]
        public IPolyline geoline { get; set; }
    }
}