#region

using System;

#endregion

namespace Aran.Temporality.Internal.Remote.Protocol
{
    [Serializable]
    internal sealed class CommunicationRequest
    {
        #region Properties

        public string Storage { get; set; }
        public string MethodName { get; set; }
        public object[] Params { get; set; }

        #endregion
    }
}