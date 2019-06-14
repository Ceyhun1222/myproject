using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AranUpdateManager
{
    class AranException : Exception
    {
        public AranException(string message)
            : base(message)
        {
        }
    }
}
