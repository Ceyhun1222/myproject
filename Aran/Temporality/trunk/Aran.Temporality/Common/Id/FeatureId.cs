using System;
using Aran.Temporality.Common.Interface;
using Aran.Temporality.Common.Util.TypeUtil;

namespace Aran.Temporality.Common.Id
{
    [Serializable]
    public class FeatureId : IFeatureId
    {
        #region Constructors

        public FeatureId()
        {
        }

        public FeatureId(IFeatureId other)
        {
            FeatureTypeId = other.FeatureTypeId;
            Guid = other.Guid;
            WorkPackage = other.WorkPackage;
        }

        #endregion

        #region Implements FeatureIdentified

        public Guid? Guid { get; set; }
        public int FeatureTypeId { get; set; }
        public int WorkPackage { get; set; }

        #endregion

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (FeatureId)) return false;
            return Equals((FeatureId) obj);
        }

        public override string ToString()
        {
            if (FeatureTypeId == 0)
            {
                return "All Features" + (WorkPackage == 0 ? String.Empty : " in WorkPackage " + WorkPackage);
            }
            if (Guid == null)
            {
                return "All " + TypeUtil.GetTypeName(FeatureTypeId) + "s" +
                       (WorkPackage == 0 ? String.Empty : " in WorkPackage " + WorkPackage);
            }
            return TypeUtil.GetTypeName(FeatureTypeId) + " : " + Guid +
                   (WorkPackage == 0 ? String.Empty : " in WorkPackage " + WorkPackage);
        }

        public bool Equals(FeatureId other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.FeatureTypeId, FeatureTypeId) && other.Guid.Equals(Guid) &&
                   other.WorkPackage.Equals(WorkPackage);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int result = FeatureTypeId + WorkPackage*400;
                result = (result*397) ^ (Guid?.GetHashCode() ?? 0);
                return result;
            }
        }
    }
}