using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSProtect
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args == null || args.Length == 0)
            {
                Console.WriteLine("Error: Please write config file(s) as argument");
            }
            else
            {
                Console.WriteLine($"{args.Length} config file(s) requested for protect...\n");
                int cnt = 0;
                for (int i = 0; i < args.Length; i++)
                {
                    string fileExe = args[i];
                    Console.Write("File ");
                    Console.Write(fileExe); 
                    if (File.Exists($@"{fileExe}.config"))
                    {
                        Encrypt.Protect(fileExe);
                        Console.WriteLine(" has been protected");
                        cnt++;
                    }
                    else
                    {
                        Console.WriteLine(" doesn`t exist");
                    }
                }
                Console.WriteLine($"\n{cnt} from {args.Length} config file(s) has been protected");
            }
        }
    }
}
