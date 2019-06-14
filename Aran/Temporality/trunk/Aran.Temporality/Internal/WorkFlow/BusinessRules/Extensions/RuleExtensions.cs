using Aran.Temporality.Internal.WorkFlow.BusinessRules.Abstract;

namespace Aran.Temporality.Internal.WorkFlow.BusinessRules.Extensions
{
    internal static class RuleExtensions
    {
        public static bool CheckLocationPosition(this AbstractBusinessRule rule, object obj)
        {
            if (obj == null) return false;
            var item = (dynamic)obj;
            return  !double.IsNaN(item.Location?.Geo.X ?? double.NaN) && !double.IsNaN(item.Location?.Geo.Y ?? double.NaN);
        }

        public static bool CheckPosition(this AbstractBusinessRule rule, object obj)
        {
            if (obj == null) return false;
            var item = (dynamic)obj;
            return !double.IsNaN(item.Geo?.X ?? double.NaN) && !double.IsNaN(item.Geo?.Y ?? double.NaN);
        }

    }
}