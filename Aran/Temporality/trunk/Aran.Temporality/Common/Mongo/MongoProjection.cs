using System;
using Aran.Aim.Data;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Aran.Aim;
using Aran.Temporality.Common.Mongo.Serializer;

namespace Aran.Temporality.Common.Mongo
{
    internal static class MongoProjection
    {
        private static readonly List<string> _requiredPathes = new List<string> {"p1_Identifier", FeatureSerializer.AimTypeIndexField, FeatureSerializer.FeatureTypeField, FeatureSerializer.NilReasonsField};
        private static readonly string _prefixPath = "Value.p2_Feature.";
        private static readonly string _postIncludePathes = ",\"Value.p1_MetaData\":1,\"Value\":1";

        private static List<string> _mongoPathes;

        private static void AddMongoPath(string path)
        {
            if (!string.IsNullOrWhiteSpace(path) && !_mongoPathes.Contains(path))
                _mongoPathes.Add(path);
        }

        private static void AddMongoPath(List<string> pathes)
        {
            foreach (var pathe in pathes)
            {
                AddMongoPath(pathe);
            }
        }

        private static bool TryFindProperty(AimClassInfo classInfo, string propName, out AimPropInfo propInfo, out int index)
        {
            propInfo = null;
            index = -1;

            for (int i = 0; i < classInfo.Properties.Count; i++)
            {
                if (classInfo.Properties[i].Name.Equals(propName, StringComparison.OrdinalIgnoreCase))
                {
                    propInfo = classInfo.Properties[i];
                    index = i;
                    return true;
                }
            }

            return false;
        }

        private static void GenerateMongoPathes(Projection projection, int featureTypeId)
        {
            _mongoPathes = new List<string>();

            foreach (var pattern in projection.Patterns.Where(x => !string.IsNullOrWhiteSpace(x)).Distinct())
            {
                var classInfo = AimMetadata.GetClassInfoByIndex(featureTypeId);

                var res = new List<string>();
                var stringBuilder = new StringBuilder();
                foreach (var propName in pattern.Split('.'))
                {
                    if (!TryFindProperty(classInfo, propName, out var propInfo, out var index))
                        throw new ArgumentException($"Projection path is incorrect. Property '{propName}' was not found in path: '{pattern}'");

                    if (stringBuilder.Length > 0) stringBuilder.Append('.');
                    stringBuilder.Append("p").Append(index).Append('_').Append(propInfo.Name);

                    if (propInfo.PropType.AimObjectType == AimObjectType.DataType ||
                        propInfo.PropType.AimObjectType == AimObjectType.Field)
                        break;

                    if (propInfo.PropType.IsAbstract && projection.Type == ProjectionType.Include)
                        res.Add(stringBuilder + ".ai");

                    classInfo = AimMetadata.GetClassInfoByIndex(propInfo.TypeIndex);
                }

                res.Add(stringBuilder.ToString());
                AddMongoPath(res);
            }
        }

        public static string GetMongoProjection(this Projection projection, int featureTypeId)
        {
            if (projection?.Patterns == null || projection.Patterns.All(string.IsNullOrWhiteSpace))
                return "{}";

            _mongoPathes = new List<string>();

            GenerateMongoPathes(projection, featureTypeId);

            if (_mongoPathes.Count == 0)
                return "{}";

            if (projection.Type == ProjectionType.Include)
                _mongoPathes = _mongoPathes.Union(_requiredPathes).ToList();
            else
                _mongoPathes = _mongoPathes.Except(_requiredPathes).ToList();

            char type = projection.Type == ProjectionType.Exclude ? '0' : '1';
            var prefix = "\"" + _prefixPath;
            var fields = _mongoPathes.Aggregate(new StringBuilder(), (current, pattern) =>
            {
                if (current.Length > 0)
                    current.Append(",");
                return current.Append(prefix).Append(pattern).Append('"').Append(":").Append(type);
            });

            if (projection.Type == ProjectionType.Include)
                fields.Append(_postIncludePathes);

            return fields.Insert(0, '{').Append("}").ToString();
        }
    }
}