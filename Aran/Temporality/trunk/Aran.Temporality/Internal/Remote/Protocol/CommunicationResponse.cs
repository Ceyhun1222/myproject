#region

using System;
using System.Runtime.Serialization;
using Aran.Temporality.Common.OperationResult;
using Aran.Temporality.Common.Util;

#endregion

namespace Aran.Temporality.Internal.Remote.Protocol
{
    [Serializable]
    internal sealed class CommunicationResponse : ISerializable
    {
        #region Properties

        private CommonOperationResult _operationResult = new CommonOperationResult();
        public object ResultObject { get; set; }

        public CommonOperationResult OperationResult
        {
            get { return _operationResult; }
            set { _operationResult = value; }
        }

        #endregion

        public CommunicationResponse()
        {
        }


        #region Implementation of ISerializable

        public CommunicationResponse(SerializationInfo info, StreamingContext context)
        {
            foreach (var entry in info)
            {
                var bytes = entry.Value as byte[];
                if (bytes != null)
                {
                    switch (entry.Name)
                    {
                        case "OperationResult":
                            OperationResult = FormatterUtil.ObjectFromBytes<CommonOperationResult>(bytes);
                            break;
                        case "ResultObject":
                            ResultObject = FormatterUtil.ObjectFromBytes<object>(bytes);
                            break;
                    }
                }
            }
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("OperationResult", FormatterUtil.ObjectToBytes(OperationResult), typeof(byte[]));
            info.AddValue("ResultObject", FormatterUtil.ComunicationObjectToBytes(ResultObject), typeof(byte[]));
        }

        #endregion
    }
}