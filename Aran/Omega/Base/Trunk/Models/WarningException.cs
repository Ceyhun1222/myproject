using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aran.Omega.Models
{
    public enum ExceptionType
    { 
        Warning,
        Critical
    }

    public enum ExceptionMessageType
    {
        DecDistanceNotAvailable,
        ThresholdNotAvailable,
        ClearWayNotAvailable,
        StopwayNotAvailable
    }
    public class OmegaException:Exception
    {
        public OmegaException(ExceptionType exceptionType,ExceptionMessageType messageType,string message="")
            : base(message)
        {
            ExType = exceptionType;
            MessageType = messageType;
        }

        public ExceptionType ExType { get; set; }
        public ExceptionMessageType MessageType { get; set; }
    }

    public class CritycalException : Exception
    { 
        
    }
}
