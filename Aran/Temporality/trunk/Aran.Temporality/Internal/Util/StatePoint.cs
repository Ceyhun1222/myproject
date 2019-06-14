#region

using System;
using Aran.Temporality.Common.Config;
using Aran.Temporality.Common.Id;
using Aran.Temporality.Common.Interface;

#endregion

namespace Aran.Temporality.Internal.Util
{
    [Serializable]
    internal class StatePoint : IFeatureId
    {
        public DateTime DateTime { get; set; }

        public FeatureId FeatureId { get; set; }

        public override string ToString()
        {
            return (FeatureId != null ? FeatureId + " at " : String.Empty) +
                   String.Format(ConfigUtil.StatePointDateFormat, DateTime);
        }

        #region Implementation of IFeatureId

        public Guid? Guid
        {
            get { return FeatureId.Guid; }
        }

        public int FeatureTypeId
        {
            get { return FeatureId.FeatureTypeId; }
        }

        public int WorkPackage
        {
            get { return FeatureId.WorkPackage; }
        }

        #endregion
    }
}