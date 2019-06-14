using Aran.Temporality.Internal.Interface;
using System;

namespace Aran.Temporality.Common.Entity
{
    [Serializable]
    public class FeatureScreenshot: INHibernateEntity
    {
        public FeatureScreenshot()
        {
            DateTime = DateTime.Now;
        }
        public virtual int Id { get; set; }
        public virtual DateTime DateTime { get; set; }

        public virtual string FeatureGuid { get; set; }

        public virtual byte[] ScreenshotData { get; set; }
    }
}
