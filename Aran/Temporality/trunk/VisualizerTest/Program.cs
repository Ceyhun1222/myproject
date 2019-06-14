using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualizerTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var client = new VisualizerCommon.Remote.VisualizerClient();
            client.ClearSelection();
        }
    }
}
