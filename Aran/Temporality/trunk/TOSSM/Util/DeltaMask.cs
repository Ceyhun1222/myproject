using System.Reflection;
using Aran.Aim;
using Aran.Aim.Features;
using Aran.Temporality.Common.Aim.MetaData;

namespace TOSSM.Util
{
    public class DeltaMask
    {
        public AimFeature StateBeforeDelta { get; set; }
        public AimFeature Delta { get; set; }

        public bool IsMasked(AimPropInfo aimPropInfo)
        {
            if (Delta == null) return false;

            try
            {
                var reason=(Delta.Feature as Feature).GetNilReason(aimPropInfo.Index);
                if (reason != null) return true;

                var value = (Delta.Feature as IAimObject).GetValue(aimPropInfo.Index);
                return value != null;
            }
            catch
            {
            }

            return false;
        }

        public bool IsMasked(PropertyInfo propInfo)
        {
            if (Delta == null) return false;

            try
            {
                var value = propInfo.GetValue(Delta.Feature, null);
                return value != null;
            }
            catch
            {
            }

            return false;
        }

        public object GetPreviousValue(AimPropInfo aimPropInfo)
        {
            if (StateBeforeDelta == null) return null;

            try
            {
                var value = (StateBeforeDelta.Feature as IAimObject).GetValue(aimPropInfo.Index);
                return value;
            }
            catch
            {
            }

            return null;
        }
    }
}
