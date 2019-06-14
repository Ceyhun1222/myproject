using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.ServiceProcess;
using System.Threading.Tasks;

namespace AranUpdServerWinService
{
    [RunInstaller(true)]
    public partial class AUServiceInstaller : Installer
    {
        public AUServiceInstaller()
        {
            InitializeComponent();

            //# Service Account Information
            var serviceProcessInstaller = new ServiceProcessInstaller();
            serviceProcessInstaller.Account = ServiceAccount.LocalSystem;
            serviceProcessInstaller.Username = null;
            serviceProcessInstaller.Password = null;

            //# Service Information
            var serviceInstaller = new ServiceInstaller();
            serviceInstaller.DisplayName = "Aran Update Server Service";
            serviceInstaller.StartType = ServiceStartMode.Automatic;
            serviceInstaller.ServiceName = "AranUpdateServerService";

            Installers.Add(serviceProcessInstaller);
            Installers.Add(serviceInstaller);
        }
    }
}
