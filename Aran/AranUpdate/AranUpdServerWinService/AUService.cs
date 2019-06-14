using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using AranUpd;

namespace AranUpdServerWinService
{
    public partial class AUService : ServiceBase
    {
        public AUService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            AUServerStarter.StartUpdateServer();
        }

        protected override void OnStop()
        {
            AUServerStarter.StopUpdateServer();
        }
    }
}
