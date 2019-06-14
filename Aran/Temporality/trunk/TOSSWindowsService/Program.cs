using Aran.Temporality.Common.Config;
using CommonUtils;
using System;
using System.Collections.Generic;
using System.ServiceProcess;
using System.Text;

namespace TOSSWindowsService
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            ConfigUtil.OwnChannelName = "Toss";
            ConfigUtil.RemoteChannelName = "TossWebApi";

            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[] 
			{ 
				new TossService() 
			};
            ServiceBase.Run(ServicesToRun);
        }
    }
}
