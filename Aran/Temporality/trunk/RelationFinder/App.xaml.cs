using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;

namespace RelationFinder
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {


            var d = Aran.Aim.Metadata.UI.UIMetadata.Instance.ClassInfoList;
            if (d.Count == 0)
            {
            }

        }
    }
}
