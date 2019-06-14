#region

using Aran.Temporality.Common.Aim.MetaData;

#endregion

namespace Aran.Temporality.Common.Util
{
    public class CloneUtil
    {
        public static T DeepClone<T>(T from) where T : class
        {
            if (from is AimState)
            {
                var state = (AimState) (object) @from;
                return (T) (object) state.Clone();
            }

            return FormatterUtil.ObjectFromBytes<T>(FormatterUtil.ObjectToBytes(from));
        }
    }
}