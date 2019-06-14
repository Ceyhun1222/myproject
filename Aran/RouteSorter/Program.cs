using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RouteSorter
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args == null || args.Length < 1 || args.Length > 2)
            {
                Console.WriteLine("Aixm 5.1 Xml formatter, (c) R.I.S.K. 2015");
                Console.WriteLine("Usage: RouteSorter.exe input_xml_file [output_xml_file]");
                Console.WriteLine("Or simply drag xml file and drop over RouteSorter.exe");
                Console.WriteLine("");
                Console.WriteLine("Press any key to exit");
                Console.ReadKey();
                return;
            }

            var input = args[0];
            var output = args.Length == 1
                ? Path.GetDirectoryName(input) + "\\" + Path.GetFileNameWithoutExtension(input) + " (Sorted)" +
                  Path.GetExtension(input)
                : args[1];

            try
            {
                if (File.Exists(output))
                {
                    File.Delete(output);
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Error occured while trying to modify output file " + output);

                Console.WriteLine("");
                Console.WriteLine("Press any key to exit");
                Console.ReadKey();
                return;
            }
           

            try
            {
                var sorter = new Aran.XmlUtil.RouteSorter();

                if (sorter.SortRoutes(input, output, Console.WriteLine, Console.WriteLine))
                {
                    Console.WriteLine("Xml file was successfully formatted.");
                }
                else
                {
                    Console.WriteLine("There was error while processing.");
                }
                Console.WriteLine("");
                Console.WriteLine("Press any key to exit");
                Console.ReadKey();
     
            }
            catch (Exception exception)
            {
                Console.WriteLine("Error occured while processing: " + exception.Message);

                Console.WriteLine("");
                Console.WriteLine("Press any key to exit");
                Console.ReadKey();
            }
        }
    }
}
