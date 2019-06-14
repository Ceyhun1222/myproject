using Aran.Aim;
using Aran.Aim.Features;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aran.Aim.Data.InputRepository
{
    public class InputDataRepository : IInputDataRepository
    {
        public InputDataRepository()
        {
            FeatureList = new ConcurrentDictionary<Guid, Feature>();
        }

        public InputDataRepository(string projectName) : this()
        {
            ProjectName = projectName;
        }

        public void AddFeatures(List<Feature> featList)
        {
            if (FeatureList == null) return;

            if (featList == null) throw new ArgumentNullException("Argument cannot be null.Input Repository add features");

            foreach (var feat in featList)
                FeatureList.TryAdd(feat.Identifier, feat);
        }

        public void AddFeature(Feature feat)
        {
            if (feat == null) throw new ArgumentNullException("Argument cannot be null.Input Repository add feature");
            FeatureList.TryAdd(feat.Identifier, feat);

        }

        public List<Feature> GetFeatures(FeatureType featType)
        {
            var result = new List<Feature>();
            foreach (var feature in FeatureList.ToArray())
            {
                if (feature.Value.FeatureType== featType)
                    result.Add(feature.Value);
            }
            return result;
        }

        public void ToXml(DateTime effectiveDate, string fileName,FeatureType featType)
        {
            var featList = FeatureList.Values.
                Where(feat => feat.FeatureType == featType).ToList();
            ToXml(effectiveDate, fileName, featList);
        }

        public void ToXml(DateTime effectiveDate,string fileName)
        {
            ToXml(effectiveDate, fileName, FeatureList.Values.ToList());
        }

        public byte[] ToXml(DateTime effectiveDate,FeatureType featType)
        {
            var featList = FeatureList.Values.
                Where(feat => feat.FeatureType == featType).ToList();
            return ToXml(effectiveDate, featList);
        }

        public byte[] ToXml(DateTime effectiveDate)
        {
            return ToXml(effectiveDate, FeatureList.Values.ToList());
        }

        private void ToXml(DateTime effectiveDate,string fileName,List<Feature> featList)
        {
            WriteToXml.WriteAllFeatureToXML(featList, fileName, false, true, effectiveDate, AixmMessage.SrsNameType.EPSG_4326);
        }

        private byte[] ToXml(DateTime effectiveDate,List<Feature> featList)
        {
           return  WriteToXml.WriteAllFeatureToStream(featList, false, true, effectiveDate, AixmMessage.SrsNameType.EPSG_4326);
        }

        public ConcurrentDictionary<Guid, Feature> FeatureList { get; private set; }

        public string ProjectName { get; private set; }
    }
}
