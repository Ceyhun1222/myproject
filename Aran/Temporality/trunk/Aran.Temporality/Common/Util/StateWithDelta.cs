using System;

namespace Aran.Temporality.Common.Util
{
    [Serializable]
    public class StateWithDelta<T> where T : class
    {
        public T StateBeforeDelta { get; set; }
        public T Delta { get; set; }
        public T StateAfterDelta { get; set; }
    }
}
