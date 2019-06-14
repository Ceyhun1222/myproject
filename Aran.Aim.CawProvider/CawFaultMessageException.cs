using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aran.Aim.CAWProvider
{
    public class CawFaultMessageException : Exception
    {
        public CawFaultMessageException (List<FaultMessage> messages)
        {
            Messages = messages;
        }

        public List<FaultMessage> Messages { get; private set; }
    }
}
