using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.ServiceProcess;

namespace AranUpdWinService
{
    [RunInstaller(true)]
    public partial class WindowsServiceInstaller : Installer
    {
        public WindowsServiceInstaller()
        {
            InitializeComponent();

            //# Service Account Information
            var serviceProcessInstaller = new ServiceProcessInstaller();
            serviceProcessInstaller.Account = ServiceAccount.LocalSystem;
            serviceProcessInstaller.Username = null;
            serviceProcessInstaller.Password = null;

            //# Service Information
            var serviceInstaller = new ServiceInstaller();
            serviceInstaller.DisplayName = "Aran Update Service";
            serviceInstaller.StartType = ServiceStartMode.Automatic;
            serviceInstaller.ServiceName = "AranUpdateService";
            //serviceInstaller.ServicesDependedOn = new string[] { "postgresql-x64-9.4" };

            Installers.Add(serviceProcessInstaller);
            Installers.Add(serviceInstaller);
        }
    }
}
