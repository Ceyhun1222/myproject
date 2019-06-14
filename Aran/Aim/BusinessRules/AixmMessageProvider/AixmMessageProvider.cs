using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aran.Aim;
using Aran.Aim.AixmMessage;
using Aran.Aim.BusinessRules;
using Aran.Aim.Features;

namespace Aran.Aim.BRules.Data
{
    public class AixmMessageProvider : IFeatureProvider
    {
        private Dictionary<FeatureType, Dictionary<Guid, Feature>> _dict;

        public AixmMessageProvider()
        {
            _dict = new Dictionary<FeatureType, Dictionary<Guid, Feature>>();

            Aran.Geometries.Operators.ArcGisInitializer.Init();
        }

        public void Open(string xmlFileName)
        {
            var parseXmlFile = new ParseXmlFile();
            parseXmlFile.Parse(xmlFileName);

            foreach(var feat in parseXmlFile.Features)
            {
                if (!_dict.TryGetValue(feat.FeatureType, out Dictionary<Guid, Feature> featDict))
                {
                    featDict = new Dictionary<Guid, Feature>();
                    _dict.Add(feat.FeatureType, featDict);
                }

                featDict[feat.Identifier] = feat;
            }
        }

        public Feature GetFeature(FeatureType featType, Guid identifier)
        {
            if (_dict.TryGetValue(featType, out Dictionary<Guid, Feature> featDict) &&
                featDict.TryGetValue(identifier, out Feature feat))
                return feat;

            return null;
        }

        public int GetFeatureCount(FeatureType featType, Guid identifier)
        {
            if (_dict.TryGetValue(featType, out Dictionary<Guid, Feature> featDict) &&
                    featDict.ContainsKey(identifier))
                return 1;
            return 0;
        }

        public IEnumerable<Feature> GetList(List<FeatureType> typeFilter)
        {
            if (typeFilter != null)
            {
                foreach (var featType in typeFilter)
                {
                    if (_dict.TryGetValue(featType, out Dictionary<Guid, Feature> featDict))
                    {
                        foreach (var feat in featDict.Values)
                        {
                            yield return feat;
                        }
                    }
                }
            }
            else
            {
                foreach (var featDictPair in _dict)
                {
                    foreach (var pair in featDictPair.Value)
                        yield return pair.Value;
                }
            }
        }

        public IEnumerable<Tuple<FeatureType, Guid>> GetAllIdentifiers(List<FeatureType> typeFilter)
        {
            if (typeFilter != null)
            {
                foreach (var featType in typeFilter)
                {
                    if (_dict.TryGetValue(featType, out Dictionary<Guid, Feature> featDict))
                    {
                        foreach (var feat in featDict.Values)
                        {
                            yield return  new Tuple<FeatureType, Guid>(featType, feat.Identifier);
                        }
                    }
                }
            }
            else
            {
                foreach (var featDictPair in _dict)
                {
                    foreach (var pair in featDictPair.Value)
                        yield return new Tuple<FeatureType, Guid>(featDictPair.Key, pair.Key);
                }
            }
        }
    }
}
