using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aran.Aim.CAWProvider
{
    public class CawException : Exception
    {
        public CawException (string message) : 
            base (message)
        {
            FaultMessages = new List<FaultMessage>();
        }

        public List<FaultMessage> FaultMessages { get; private set; }
    }
}
