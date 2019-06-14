#region

using System;
using Aran.Temporality.Common.Abstract.Event;

#endregion

namespace Aran.Temporality.Internal.MetaData
{
    [Serializable]
    public class EventMetaData : AbstractEventMetaData
    {
        public EventMetaData()
        {
        }

        public EventMetaData(AbstractEventMetaData other) : base(other)
        {
        }

        #region Overrides of AbstractEventMetaData

        public override Guid? Guid { get; set; }

        public override int FeatureTypeId { get; set; }


        #endregion
    }
}