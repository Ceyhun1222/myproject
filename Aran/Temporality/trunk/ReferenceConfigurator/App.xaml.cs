using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace ReferenceConfigurator
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            ToolTipService.ShowDurationProperty.OverrideMetadata(typeof(DependencyObject),
                                                                 new FrameworkPropertyMetadata(Int32.MaxValue));
            ToolTipService.ShowOnDisabledProperty.OverrideMetadata(typeof(DependencyObject),
                                                                 new FrameworkPropertyMetadata(true));

            var d = Aran.Aim.Metadata.UI.UIMetadata.Instance.ClassInfoList;
            if (d.Count == 0)
            {
            }

        }
    }
}
