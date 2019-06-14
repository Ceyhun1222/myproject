using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsterixReader
{
	class Program
	{
		static void Main ( string[] args )
		{
		    //if ( args.Length != 3 )
			//	return;
			//sourceFileName = args[ 0 ];
			//if ( !DateTime.TryParse ( args[ 1 ], out dateTime ) )
			//	return;
			//resultFeatClass = args[ 2 ];

			var sourceFileName = System.IO.Path.Combine ( AppDomain.CurrentDomain.BaseDirectory, "track20130628_0400_A.atx" );
			var dateTime = DateTime.Now.Date;
			var resultFeatClass = System.IO.Path.Combine ( AppDomain.CurrentDomain.BaseDirectory, @"res.gdb\feat" );
		    Console.WriteLine("Writing ...");
            MainViewModel mainVm = new MainViewModel ( sourceFileName, dateTime, resultFeatClass );
		    Console.WriteLine("Done");
		    Console.Read();
		}
	}
}