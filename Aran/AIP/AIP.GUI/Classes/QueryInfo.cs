using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aran.Package;
using System.Collections;
using Aran.Aim.Data.Filters;
using Aran.Aim;
using Aran.Aim.Features;

namespace AIP.GUI
{
    public class QueryInfo : IPackable
    {
        public QueryInfo()
        {
            SubQueries = new List<SubQueryInfo>();
            RefQueries = new List<RefQueryInfo>();
            IsVisible = true;
        }

        public QueryInfo(FeatureType featType)
            : this()
        {
            FeatureType = featType;
            Name = featType.ToString();
        }

        public string Name { get; set; }

        public bool IsVisible { get; set; }

        public FeatureType FeatureType { get; set; }

        public Filter Filter { get; set; }
        

        public List<SubQueryInfo> SubQueries { get; internal set; }

        public List<RefQueryInfo> RefQueries { get; internal set; }

        //Filter not cloned.
        public QueryInfo Clone()
        {
            QueryInfo qi = new QueryInfo();
            qi.Name = Name;
            qi.IsVisible = IsVisible;
            qi.FeatureType = FeatureType;
            qi.Filter = Filter;

            foreach (var sqi in SubQueries)
                qi.SubQueries.Add(sqi.Clone());

            return qi;
        }

        public void Pack(PackageWriter writer)
        {
            writer.PutString(Name);
            writer.PutBool(IsVisible);
            writer.PutEnum<FeatureType>(FeatureType);

            bool filterNotNull = (Filter != null);
            writer.PutBool(filterNotNull);
            if (filterNotNull)
                Filter.Operation.Pack(writer);

            
            writer.PutInt32(SubQueries.Count);
            foreach (var sqi in SubQueries)
                (sqi as IPackable).Pack(writer);

            writer.PutInt32(RefQueries.Count);
            foreach (var rqi in RefQueries)
                rqi.Pack(writer);
        }

        public void Unpack(PackageReader reader)
        {
            Name = reader.GetString();
            IsVisible = reader.GetBool();
            FeatureType = reader.GetEnum<FeatureType>();

            bool filterNotNull = reader.GetBool();
            if (filterNotNull) {
                var oc = OperationChoice.UnpackOperationChoice(reader);
                Filter = new Filter(oc);
            }
            
            int count = reader.GetInt32();

            SubQueries.Clear();
            count = reader.GetInt32();
            for (int i = 0; i < count; i++) {
                var sqi = new SubQueryInfo();
                (sqi as IPackable).Unpack(reader);
                SubQueries.Add(sqi);
            }

            RefQueries.Clear();
            count = reader.GetInt32();
            for (int i = 0; i < count; i++) {
                var rqi = new RefQueryInfo();
                rqi.Unpack(reader);
                RefQueries.Add(rqi);
            }
        }

        //public List<Feature> LoadDataFromQI(QueryInfo qi, List<Feature> featureList)
        //{
        //    var tmp = Globals.dbPro.GetAllFeatuers(qi.FeatureType);


        //    if (qi.SubQueries.Count() > 0)
        //    {

        //    }

        //    return null;
        //}

        //public List<Feature> LoadDataFromSubQI(QueryInfo qi, List<Feature> featureList)
        //{
        //    var tmp = Globals.dbPro.GetAllFeatuers(qi.FeatureType);


        //    if (qi.SubQueries.Count() > 0)
        //    {

        //    }

        //    return null;
        //}
    }

    public interface ILinkQueryInfo
    {
        LinkQueryType LinkType { get; }
        string PropertyPath { get; }
        QueryInfo QueryInfo { get; }
    }

    public class SubQueryInfo : IPackable, ILinkQueryInfo
    {
        public SubQueryInfo()
        {
        }

        public SubQueryInfo(string propertyPath, QueryInfo queryInfo)
        {
            QueryInfo = queryInfo;
            PropertyPath = propertyPath;
        }

        public LinkQueryType LinkType
        {
            get { return LinkQueryType.Sub; }
        }

        public QueryInfo QueryInfo { get; set; }

        public string PropertyPath { get; set; }

        public SubQueryInfo Clone()
        {
            SubQueryInfo sqi = new SubQueryInfo();
            sqi.PropertyPath = PropertyPath;
            if (QueryInfo != null)
                sqi.QueryInfo = QueryInfo.Clone();
            return sqi;
        }

        public void Pack(PackageWriter writer)
        {
            bool notNull = (QueryInfo != null);
            writer.PutBool(notNull);
            if (notNull) {
                QueryInfo.Pack(writer);
            }
            writer.PutString(PropertyPath);
        }

        public void Unpack(PackageReader reader)
        {
            bool notNull = reader.GetBool();
            if (notNull) {
                QueryInfo = new QueryInfo();
                QueryInfo.Unpack(reader);
            }
            PropertyPath = reader.GetString();
        }
    }

    public class RefQueryInfo : IPackable, ILinkQueryInfo
    {
        public RefQueryInfo()
        {

        }

        public LinkQueryType LinkType
        {
            get { return LinkQueryType.Sub; }
        }

        public QueryInfo QueryInfo { get; set; }

        public string PropertyPath { get; set; }

        public RefQueryInfo Clone()
        {
            var rqi = new RefQueryInfo();
            if (QueryInfo != null)
                rqi.QueryInfo = QueryInfo.Clone();
            rqi.PropertyPath = PropertyPath;
            return rqi;
        }

        public void Pack(PackageWriter writer)
        {
            bool notNull = (QueryInfo != null);
            writer.PutBool(notNull);
            if (notNull) {
                QueryInfo.Pack(writer);
            }
            writer.PutString(PropertyPath);
        }

        public void Unpack(PackageReader reader)
        {
            bool notNull = reader.GetBool();
            if (notNull) {
                QueryInfo = new QueryInfo();
                QueryInfo.Unpack(reader);
            }
            PropertyPath = reader.GetString();
        }
    }

    public enum LinkQueryType { Ref, Sub }


    
}