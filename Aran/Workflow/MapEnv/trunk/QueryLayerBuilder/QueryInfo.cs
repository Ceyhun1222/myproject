using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aran.Aim;
using Aran.Aim.Data.Filters;
using Aran.Aim.Env2.Layers;
using System.Collections;
using Aran.Package;

namespace MapEnv.QueryLayer
{
    public class QueryInfo_OLD : IPackable
    {
        public QueryInfo_OLD ()
        {
            ShapeInfoList = new List<TableShapeInfo> ();
            SubQueries = new List<SubQueryInfo_OLD> ();
        }

        public string Name { get; set; }

        public FeatureType FeatureType { get; set; }

        public Filter Filter { get; set; }

        public List<TableShapeInfo> ShapeInfoList { get; private set; }

        public List<SubQueryInfo_OLD> SubQueries { get; private set; }

        public IList FeatureList { get; set; }

        //Filter not cloned.
        public QueryInfo_OLD Clone ()
        {
            QueryInfo_OLD qi = new QueryInfo_OLD ();
            qi.Name = Name;
            qi.FeatureType = FeatureType;
            qi.Filter = Filter;

            foreach (var shapeInfo in ShapeInfoList)
                qi.ShapeInfoList.Add (shapeInfo.Clone () as TableShapeInfo);

            foreach (var sqi in SubQueries)
                qi.SubQueries.Add (sqi.Clone ());

            return qi;
        }

        public void Pack (PackageWriter writer)
        {
            writer.PutString (Name);
            writer.PutEnum<FeatureType> (FeatureType);

            bool filterNotNull = (Filter != null);
            writer.PutBool (filterNotNull);
            if (filterNotNull)
                Filter.Pack (writer);

            writer.PutInt32 (ShapeInfoList.Count);
            foreach (var shapeInfo in ShapeInfoList)
                (shapeInfo as IPackable).Pack (writer);

            writer.PutInt32 (SubQueries.Count);
            foreach (var sqi in SubQueries)
                (sqi as IPackable).Pack (writer);
        }

        public void Unpack (PackageReader reader)
        {
            Name = reader.GetString ();
            FeatureType = reader.GetEnum<FeatureType> ();

            bool filterNotNull = reader.GetBool ();
            if (filterNotNull)
                Filter = LayerPackage.UnpackFilter (reader);

            ShapeInfoList.Clear ();
            int count = reader.GetInt32 ();
            for (int i = 0; i < count; i++)
            {
                TableShapeInfo shapeInfo = new TableShapeInfo ();
                (shapeInfo as IPackable).Unpack (reader);
                ShapeInfoList.Add (shapeInfo);
            }

            SubQueries.Clear ();
            count = reader.GetInt32 ();
            for (int i = 0; i < count; i++)
            {
                var sqi = new SubQueryInfo_OLD ();
                (sqi as IPackable).Unpack (reader);
                SubQueries.Add (sqi);
            }
        }
    }

    public class SubQueryInfo_OLD : IPackable
    {
        public SubQueryInfo_OLD ()
        {
        }

        public SubQueryInfo_OLD (string propertyPath, QueryInfo_OLD queryInfo)
        {
            QueryInfo = queryInfo;
            PropertyPath = propertyPath;
        }

        public QueryInfo_OLD QueryInfo { get; set; }

        public string PropertyPath { get; set; }

        public SubQueryInfo_OLD Clone ()
        {
            SubQueryInfo_OLD sqi = new SubQueryInfo_OLD ();
            sqi.PropertyPath = PropertyPath;
            if (QueryInfo != null)
                sqi.QueryInfo = QueryInfo.Clone ();
            return sqi;
        }

        public void Pack (PackageWriter writer)
        {
            bool notNull = (QueryInfo != null);
            writer.PutBool (notNull);
            if (notNull)
            {
                QueryInfo.Pack (writer);
            }
            writer.PutString (PropertyPath);
        }

        public void Unpack (PackageReader reader)
        {
            bool notNull = reader.GetBool ();
            if (notNull)
            {
                QueryInfo = new QueryInfo_OLD ();
                QueryInfo.Unpack (reader);
            }
            PropertyPath = reader.GetString ();
        }
    }
}
