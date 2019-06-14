using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aran.Aim.BRules.SbvrParser
{
    public interface ILogger
    {
        void SetError(string message, Exception innerEx = null);

        void SetWarning(string message, Exception innerEx = null);
    }

    public class SimpleLogger : ILogger
    {
        public void SetError(string message, Exception innerEx = null)
        {
            Console.WriteLine($"Error: {message}");
        }

        public void SetWarning(string message, Exception innerEx = null)
        {
            Console.WriteLine($"Warning: {message}");
        }
    }
}
