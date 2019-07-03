using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aran.Aim.Data.Filters;
using Aran.Aim.Utilities;
using Aran.Package;

namespace Aran.Aim.Data
{
    public enum ProjectionType { Exclude, Include }

    [Serializable]
    public class Projection : IPackable
    {
        public ProjectionType Type { get; private set; }

        public HashSet<string> Patterns { get; private set; }

        private Projection(ProjectionType type, IEnumerable<string> patterns = null)
        {
            Type = type;
            Patterns = patterns != null ? new HashSet<string>(patterns) : new HashSet<string>();
        }

        public void IncludeFilterPaths(Filter filter)
        {
            if (filter == null)
                return;

            var paths = filter.GetPaths();

            if (Type == ProjectionType.Include)
                Patterns.UnionWith(paths);
            else
                Patterns.ExceptWith(paths);
        }

        #region Static methods

        public static Projection Include(IEnumerable<string> patterns = null)
        {
            return new Projection(ProjectionType.Include, patterns);
        }

        public static Projection Include(params string[] patterns)
        {
            return new Projection(ProjectionType.Include, patterns);
        }

        public static Projection Exclude(params string[] patterns)
        {
            return new Projection(ProjectionType.Exclude, patterns);
        }

        public static Projection Exclude(IEnumerable<string> patterns = null)
        {
            return new Projection(ProjectionType.Exclude, patterns);
        }

        public static Projection WithoutGeometry(int featureTypeId)
        {
            var projection = new Projection(ProjectionType.Exclude);
            AimMetadataUtility.FindGeometryPathes(featureTypeId, string.Empty, projection.Patterns.ToList());
            return projection;
        }

        public static Projection OnlyGeometry(int featureTypeId)
        {
            var projection = new Projection(ProjectionType.Include);
            AimMetadataUtility.FindGeometryPathes(featureTypeId, string.Empty, projection.Patterns.ToList());
            return projection;
        }

        public static Projection WithoutGeometry(FeatureType featureType)
        {
            return WithoutGeometry((int)featureType);
        }

        public static Projection OnlyGeometry(FeatureType featureType)
        {
            return OnlyGeometry((int)featureType);
        }

        #endregion

        public override string ToString()
        {
            return $"{Type}, {Patterns.Count} items";
        }

        #region IPackable

        public void Pack(PackageWriter writer)
        {
            writer.PutEnum(Type);
            writer.PutInt32(Patterns.Count);
            foreach (var pattern in Patterns)
                writer.PutString(pattern);
        }

        public void Unpack(PackageReader reader)
        {
            Type = reader.GetEnum<ProjectionType>();
            Patterns = new HashSet<string>();
            int count = reader.GetInt32();
            for (int i = 0; i < count; i++)
                Patterns.Add(reader.GetString());
        }

        #endregion
    }
}
