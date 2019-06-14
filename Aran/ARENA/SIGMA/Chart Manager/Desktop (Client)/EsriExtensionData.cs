using System;

namespace ChartManager
{
    public static class EsriExtensionData
    {
        static EsriExtensionData()
        {
            Id = Guid.Empty;
        }

        public static bool IsInitialized => (Id != Guid.Empty);

        public static bool HasUpdate => (UpdateId != default(long));

        public static Guid Id
        {
            get;
            set;
        }

        public static int ChartVersion { get; set; }

        public static bool IsReadOnly { get; set; }

        public static DateTime EffectiveDate { get; set; }
        public static int UpdateId { get; set; }

        public static void Clear()
        {
            Initialize(DateTime.MinValue);
            Id = Guid.Empty;
            UpdateId = default(int);
        }

        public static void Initialize(DateTime dateTime)
        {
            Id = Guid.NewGuid();
            ChartVersion = 0;
            IsReadOnly = false;
            EffectiveDate = dateTime;
        }
    }
}