using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Aran.Aim.Features;
using Aran.Temporality.Common.Aim.Extension.Message;
using Aran.Temporality.Common.Aim.Extension.Property;
using Aran.Temporality.Common.Config;
using Aran.Temporality.Common.Id;
using Aran.Temporality.Common.Interface;
using Aran.Temporality.Internal.MetaData.Offset;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using Aran.Temporality.Common.Mongo.Serializer;

namespace Aran.Temporality.Common.Mongo
{
    class MongoClientCustom
    {
        private readonly MongoClient _client;

        public static MongoClientCustom Instance { get; } = new MongoClientCustom();

        private MongoClientCustom()
        {
            var settings = new MongoClientSettings
            {
                Credential = MongoCredential.CreateCredential("admin", ConfigUtil.MongoUser, ConfigUtil.MongoPassword),
                Server = new MongoServerAddress(ConfigUtil.MongoServerAddress, ConfigUtil.MongoServerPort)
            };
            _client = new MongoClient(settings);

            RegisterClassMaps();
            RegisterSerializers();
        }

        private Type[] GetTypesInNamespace(Assembly assembly, string nameSpace)
        {
            return
              assembly.GetTypes()
                      .Where(t => String.Equals(t.Namespace, nameSpace, StringComparison.Ordinal))
                      .ToArray();
        }

        private void RegisterClassMaps()
        {
            BsonClassMap.RegisterClassMap<FeatureId>();
            BsonClassMap.RegisterClassMap<UserExtension>();
            BsonClassMap.RegisterClassMap<EsriPropertyExtension>(cm =>
            {
                cm.AutoMap();
                cm.UnmapMember(o => o.EsriObject);
                cm.SetIgnoreExtraElements(true);
            });

            BsonClassMap.RegisterClassMap<ElevatedPoint>();

            BsonClassMap.RegisterClassMap<OffsetEventMetaData<string>>(cm =>
            {
                cm.AutoMap();
                cm.MapField(o => o.RelatedFeatures).SetSerializer(new SortedListSerializerByIDictionary<int, List<IFeatureId>>());
            });


            BsonClassMap.RegisterClassMap<AixmPoint>(cm =>
            {
                cm.AutoMap();
            });

            RegisterClassMap(Assembly.Load("Aran.Aim").GetTypes());
            RegisterClassMap(Assembly.Load("Aran.Geometries").GetTypes());
        }

        private void RegisterSerializers()
        {
            BsonSerializer.RegisterSerializer(typeof(DateTime), DateTimeSerializer.LocalInstance);

            BsonSerializer.RegisterSerializer(new AimEventSerializer());

            IEnumerable<Type> types = Assembly.GetAssembly(typeof(Feature)).GetTypes().Where(type => type.IsSubclassOf(typeof(Feature)));
            var featureSerializerType = typeof(FeatureSerializer<>);
            foreach (var type in types.Where(t => !t.IsInterface && !t.IsAbstract && !t.IsGenericType))
            {
                var genericType = featureSerializerType.MakeGenericType(type);
                var serializer = Activator.CreateInstance(genericType);
                BsonSerializer.RegisterSerializer(type, (IBsonSerializer)serializer);
            }
        }

        private void RegisterClassMap(params Type[] types)
        {
            var registerClassMapMethod = typeof(BsonClassMap).GetMethods().FirstOrDefault(m => m.Name == "RegisterClassMap" && m.GetParameters().Length == 0);
            if (registerClassMapMethod != null)
            {
                foreach (var type in types.Where(t => !t.IsInterface && !t.IsAbstract && !t.IsGenericType && !BsonClassMap.IsClassMapRegistered(t)))
                {
                    MethodInfo genericMethod = registerClassMapMethod.MakeGenericMethod(type);
                    genericMethod.Invoke(null, null);
                }
            }
        }

        private string GetDatabaseName(string name)
        {
            return $"risk_{name}";
        }

        public IMongoDatabase GetDatabase(string name)
        {
            return _client.GetDatabase(GetDatabaseName(name), new MongoDatabaseSettings());
        }

        public bool IsExistDatabase(string name)
        {
            return _client.GetDatabase(GetDatabaseName(name)).ListCollections().ToList().Count > 0;
        }

        public async Task DropDatabase(string name)
        {
            await _client.DropDatabaseAsync(GetDatabaseName(name));
        }
    }
}
