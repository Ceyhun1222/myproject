using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Aran.Temporality.Common.Mongo
{
    internal class MongoWrapper<T>
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public T Value { get; set; }

        public MongoWrapper(T value)
        {
            Value = value;
            Id = ObjectId.GenerateNewId().ToString();
        }
    }
}
