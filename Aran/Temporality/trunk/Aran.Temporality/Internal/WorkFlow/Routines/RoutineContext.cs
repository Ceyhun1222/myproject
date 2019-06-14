using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Aran.Aim;
using Aran.Aim.DataTypes;
using Aran.Aim.Features;
using Aran.Aim.Objects;
using Aran.Temporality.Common.Aim.MetaData;
using Aran.Temporality.Common.Entity;
using Aran.Temporality.Common.Id;
using Aran.Temporality.Internal.Service;

namespace Aran.Temporality.Internal.WorkFlow.Routines
{
    internal class RoutineContext : IDisposable
    {

        public AimTemporalityService Service;

        public PrivateSlot PrivateSlot;

        public DateTime EffectiveDate { get; set; }

        private readonly IDictionary<int, AimFeature[]> _loadedStates = new ConcurrentDictionary<int, AimFeature[]>();

        public AimFeature[] LoadStates(FeatureType type)
        {
            AimFeature[] result;

            if (!_loadedStates.TryGetValue((int)type, out result))
            {
                result = Service.GetActualDataByDate(new FeatureId
                {
                    FeatureTypeId = (int)type,
                    WorkPackage = PrivateSlot.Id
                }, false, EffectiveDate).Select(t => t.Data).ToArray();
                _loadedStates[(int)type] = result;
            }

            return result;
        }

        
        public AimFeature[] LoadFeatureInRange(FeatureType type, Guid guid, DateTime dateTimeStartParam)
        {

            return Service.GetStatesInRange(new FeatureId
            {
                FeatureTypeId = (int)type,
                WorkPackage = PrivateSlot.Id,
                Guid = guid
            }, false, dateTimeStartParam, EffectiveDate).Select(t => t.Data).ToArray();

        }

        public static IEnumerable<FeatureRef> GetDirectLinkReferences(Feature feature, string propertyPath)
        {
            var current = new List<object> { feature };

            while (propertyPath.Length > 0)
            {
                var i = propertyPath.IndexOf('/');
                var currentProperty = propertyPath;
                if (i == -1)
                {
                    propertyPath = string.Empty;
                }
                else
                {
                    currentProperty = propertyPath.Substring(0, i);
                    propertyPath = propertyPath.Substring(i + 1);
                }
                //
                var next = new List<object>();
                foreach (var value in current.Select(item => item.GetType().GetProperty(currentProperty).GetValue(item, null)))
                {
                    if (value == null)
                    {
                    }
                    else if (value is IList)
                    {
                        next.AddRange((value as IList).Cast<object>());
                    }
                    else
                    {
                        next.Add(value);
                    }
                }
                current = next;
            }


            var result = new List<FeatureRef>();

            foreach (var item in current)
            {
                if (item is FeatureRef)
                {
                    var reference = item as FeatureRef;
                    result.Add(reference);
                }
                else if (item is FeatureRefObject)
                {
                    result.Add((item as FeatureRefObject).Feature);
                }
                else
                {
                    throw new Exception("bad link");
                }
            }
            return result;
        }

        public AimFeature LoadFeature(FeatureType type, Guid guid)
        {
            return LoadStates(type).FirstOrDefault(t => t.Identifier == guid);
        }

        public IEnumerable<AimFeature> Load(FeatureType type)
        {
            return LoadStates(type);
        }

        public IEnumerable<AimFeature> Load()
        {
            List<AimFeature> features = new List<AimFeature>();
            foreach (FeatureType featureType in Enum.GetValues(typeof(FeatureType)))
            {
               features.AddRange(Load(featureType));
            }
            return features;
        }

        private List<Feature> GetDirectLinks(FeatureType featureType, Feature feature, string propertyPath)
        {
            var references = GetDirectLinkReferences(feature, propertyPath);

            var result = new List<Feature>();
            foreach (var reference in references)
            {
                var f = LoadFeature(featureType, reference.Identifier);
                if (f?.Feature != null)
                {
                    result.Add(f.Feature);
                }
            }

            return result;
        }

        private List<Feature> GetReverseLinks(FeatureType featureType, Feature feature, string propertyPath)
        {

            var sourceFeatures = LoadStates(featureType);
            return sourceFeatures.Where(
                t => GetDirectLinkReferences(t.Feature, propertyPath).Any(t2 => t2.Identifier == feature.Identifier)).
                Select(t3 => t3.Feature).ToList();
        }

        public List<Feature> GetLinks(bool isDirect, FeatureType featureType, Feature feature, string propertyPath)
        {
            return isDirect ? GetDirectLinks(featureType, feature, propertyPath) :
                GetReverseLinks(featureType, feature, propertyPath);
        }


        public void Dispose()
        {
            _loadedStates.Clear();
        }

    }
}