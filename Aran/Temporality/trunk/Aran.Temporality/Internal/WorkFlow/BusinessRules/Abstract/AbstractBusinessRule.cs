using System;
using System.Linq;
using Aran.Aim;
using Aran.Aim.DataTypes;
using Aran.Aim.Features;
using Aran.Aim.Objects;
using Aran.Temporality.Internal.WorkFlow.Routines;
using Aran.Temporality.Internal.WorkFlow.Util;
using System.Collections.Generic;
using System.Text;
//using ICSharpCode.SharpZipLib.Checksums;
using Crc32 = Aran.Temporality.Internal.WorkFlow.BusinessRules.Util.Crc32;

namespace Aran.Temporality.Internal.WorkFlow.BusinessRules.Abstract
{
    internal abstract class AbstractBusinessRule
    {
        public static double Epsilon()
        {
            return 0.0001;
        }

        #region Additional calcullations

        //        public static double DistanceInMeters(ElevatedPoint location, ElevatedPoint arp)
        //        {
        //#warning implement
        //            //TODO: implement

        //            return 0;
        //        })


        public List<Feature> LoadAll(FeatureType featureType, Guid guid)
        {
            if (guid == Guid.Empty) return null;

            var f = Context.LoadFeatureInRange(featureType, guid, new DateTime(2014, 7, 24, 0, 0, 0));

            return f?.Select(t => t.Feature).ToList();
        }

        public List<Feature> Load(FeatureType featureType)
        {
            return Context.LoadStates(featureType).Select(s => s.Feature).ToList();
        }

        public Feature Load(FeatureType featureType, FeatureRef reference)
        {
            if (reference == null) return null;
            
            var f = Context.LoadFeature(featureType, reference.Identifier);

            return f?.Feature;
        }

        public Feature Load(FeatureType featureType, FeatureRefObject reference)
        {
            return reference?.Feature == null ? null : Context.LoadFeature(featureType, reference.Feature.Identifier).Feature;
        }

        public List<AimPropInfo> GetProperties(object obj)
        {
            var result = new List<AimPropInfo>();
            if (GetApplicableProperty() == null) return result;
            var aimObject  = obj as IAimObject;
            return aimObject == null ? result : ChildFinder.GetApplicablePropertiesList(aimObject, GetApplicableProperty());
        }


        #endregion

        public bool CheckFeature(Feature feature)
        {
            return GetApplicableType() != null ?
                ChildFinder.GetChildrenByType(feature, GetApplicableType()).All(CheckChild):
                ChildFinder.GetParentsByPropertyName(feature, GetApplicableProperty()).All(CheckParent);
        }

        public virtual bool CheckChild(object obj)
        {
            return true;
        }

        public virtual bool CheckParent(object obj)
        {
            return true;
        }

        public abstract Type GetApplicableType();
        public abstract string GetApplicableProperty();
        public virtual Type GetApplicablePropertyType() => null;
        public abstract string Source();
        public abstract string Svbr();
        public abstract string Comments();
        public abstract string Name();
        public abstract string Category();
        public abstract string Level();
        public int Id => unchecked((int)Crc32.Instance.ComputeChecksum(Encoding.UTF8.GetBytes(UID)));
        public virtual string UID => $"AXM-5.1_RULE-{GetType().Name.Substring(12)}";


        public RoutineContext Context { get; set; }
    }
}
