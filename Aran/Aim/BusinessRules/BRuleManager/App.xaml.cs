using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;

namespace BRuleManager
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            dynamic rulesModel = Model.ModelFactory.Create(Model.ModelType.Rules);
            //*** set maximized windows state
            rulesModel._defaultSize = new Size(1.1, 0);

            //dynamic rulesModel = Model.ModelFactory.Create(Model.ModelType.CheckReport);

            WindowService.Instance.ShowWindow(rulesModel);
        }
    }
}
