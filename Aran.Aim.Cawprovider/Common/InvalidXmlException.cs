using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aran.Aim.CAWProvider
{
    public class InvalidXmlException : Exception
    {
        public InvalidXmlException () 
            : base ()
        {
        }

        public InvalidXmlException (string message) 
            : base (message)
        {
        }
    }
}
