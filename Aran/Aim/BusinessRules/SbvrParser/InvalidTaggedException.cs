using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aran.Aim.BusinessRules.SbvrParser
{
    public class InvalidTaggedException : Exception
    {
        public InvalidTaggedException() :
            base("InvalidTaggedException")
        {
        }

        public InvalidTaggedException(string message) : 
            base(message)
        {
        }
    }
}
