using System.Runtime.Serialization;

namespace Aran.Temporality.Common.Exceptions
{
    public class OperationException: System.Exception
    {
        public OperationException()
        {
        }

        public OperationException(string message) : base(message)
        {
        }

        public OperationException(string message, System.Exception innerException) : base(message, innerException)
        {
        }

        protected OperationException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}