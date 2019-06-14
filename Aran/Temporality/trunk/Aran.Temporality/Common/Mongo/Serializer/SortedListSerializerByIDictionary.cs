using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aran.Temporality.Common.Interface;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Options;
using MongoDB.Bson.Serialization.Serializers;

namespace Aran.Temporality.Common.Mongo.Serializer
{
    class SortedListSerializerByIDictionary<T, K> : SerializerBase<IDictionary<T, K>>
    {
        private DictionaryRepresentation _dictionaryRepresentation;

        public SortedListSerializerByIDictionary(DictionaryRepresentation dictionaryRepresentation = DictionaryRepresentation.ArrayOfDocuments)
        {
            _dictionaryRepresentation = dictionaryRepresentation;
        }

        public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, IDictionary<T, K> value)
        {
            var newValue = (SortedList<T, K>)value;
            var serializer = new DictionaryInterfaceImplementerSerializer<SortedList<T, K>, T, K>(_dictionaryRepresentation);
            serializer.Serialize(context, newValue);
        }

        public override IDictionary<T, K> Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        {
            args.NominalType = typeof(SortedList<T, K>);
            var serializer = new DictionaryInterfaceImplementerSerializer<SortedList<T, K>, T, K>(_dictionaryRepresentation);
            return serializer.Deserialize(context);
        }
    }
}
