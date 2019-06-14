using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Aran.Aim;
using Aran.Aim.Features;
using Aran.Temporality.Common.Aim.Extension.Message;
using Aran.Temporality.Common.Aim.Extension.Property;
using Aran.Temporality.Common.ArcGis;
using Aran.Temporality.Common.Config;
using Aran.Temporality.Common.Interface;
using Aran.Temporality.Common.Util;

namespace Aran.Temporality.Common.Aim.MetaData
{
    [Serializable]
    public class AimFeature : ISerializable
    {
        public static implicit operator AimFeature(Feature feature) { return new AimFeature { Feature = feature }; }

        private List<MessageExtension> _messageExtensions;
        public List<MessageExtension> MessageExtensions
        {
            get => _messageExtensions ?? (_messageExtensions = new List<MessageExtension>());
            set => _messageExtensions = value;
        }


        private List<PropertyExtension> _propertyExtensions;
        public List<PropertyExtension> PropertyExtensions
        {
            get => _propertyExtensions ?? (_propertyExtensions = new List<PropertyExtension>());
            set => _propertyExtensions = value;
        }

        public void InitEsriExtension()
        {
            try
            {
                if (ConfigUtil.UseEsri)
                {
                    PropertyExtensions.RemoveAll(t => t is EsriPropertyExtension);

                    GeometryFormatter.InitEsriExtension(this);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
                    

        private Feature _feature;
        public Feature Feature
        {
            get => _feature;
            set
            {
                _feature = value;
                if (Feature != null)
                {
                    FeatureType = Feature.FeatureType;
                    Identifier = Feature.Identifier;
                }
            }
        }

        public AimFeature()
        {
        }

        public AimFeature(IFeatureId featureId)
        {
            FeatureType = (FeatureType)featureId.FeatureTypeId;
            if (featureId.Guid != null)
            {
                Identifier = (Guid)featureId.Guid;
            }
        }

        public Guid Identifier { get; set; }

        #region Overrides of Feature

        public FeatureType FeatureType { get; private set; }

        #endregion

        #region Implementation of ISerializable

        public AimFeature(SerializationInfo info, StreamingContext context)
        {
            foreach(var entry in info)
            {
                var bytes = entry.Value as byte[];
                if (bytes != null)
                {
                    switch (entry.Name)
                    {
                        case "Feature":
                            Feature = FormatterUtil.ObjectFromBytes<Feature>(bytes);
                            break;
                        case "PropertyExtensions":
                            PropertyExtensions = FormatterUtil.ObjectFromBytes<List<PropertyExtension>>(bytes);
                            break;
                        case "MessageExtensions":
                            MessageExtensions = FormatterUtil.ObjectFromBytes<List<MessageExtension>>(bytes);
                            break;
                    }
                }
            }
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Feature", FormatterUtil.ObjectToBytes(Feature), typeof(byte[]));

            info.AddValue("PropertyExtensions", FormatterUtil.ObjectToBytes(PropertyExtensions), typeof(byte[]));

            info.AddValue("MessageExtensions", FormatterUtil.ObjectToBytes(MessageExtensions), typeof(byte[]));
        }

        #endregion
    }
}
