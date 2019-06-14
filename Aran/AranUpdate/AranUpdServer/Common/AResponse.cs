using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace AranUpdater
{
    [DataContract]
    public class AResponse
    {
        public AResponse(string errorMessage)
        {
            IsOk = false;
            ErrorMessage = errorMessage;
        }

        public AResponse()
        {
            IsOk = true;
        }

        [DataMember]
        public bool IsOk { get; set; }

        [DataMember]
        public string ErrorMessage { get; set; }
    }

    [DataContract]
    public class AResponse<T> : AResponse
    {
        public AResponse(T result)
            : base()
        {
            Result = result;
        }

        [DataMember]
        public T Result { get; set; }

        public static AResponse<T> CreateError(string errorMessage)
        {
            return new AResponse<T>(default(T)) { IsOk = false, ErrorMessage = errorMessage };
        }
    }
}
