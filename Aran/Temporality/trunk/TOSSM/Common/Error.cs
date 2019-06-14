using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.Features;

namespace TOSSM.Common
{
    internal class Error
    {
        public String Message { get; set; }
        public virtual ErrorType Type { get; set; }
    }

    internal class DataError<T> : Error
    {
        public List<T> Data { get; } = new List<T>();
    }

    internal class ExceptionError : Error
    {
        public Exception Exception { get; set; }
        public override ErrorType Type => ErrorType.Exception;
    }

    internal enum ErrorType
    {
        Exception,
        InvalidData,
        NullReference
    }

    internal class FeatureError : DataError<Feature>
    {
        public FeatureType FeatureType { get; set; }
    }
}
