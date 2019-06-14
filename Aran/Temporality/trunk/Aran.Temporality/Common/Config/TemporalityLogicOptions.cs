using System;

namespace Aran.Temporality.Common.Config
{
    [Serializable]
    public class TemporalityLogicOptions
    {
        #region SortingEventField enum

        public enum SortingEventField
        {
            PublicDate,
            ValidStartDate
        }

        #endregion

        public SortingEventField SortingEventBy { get;
            //set { _sortingEventBy = value; }
        } = SortingEventField.ValidStartDate;
    }
}