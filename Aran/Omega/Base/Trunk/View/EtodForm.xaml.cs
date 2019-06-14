using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Aran.Aim.Features;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using Aran.PANDA.Constants;
using Aran.PANDA.Common;
using Aran.AranEnvironment.Symbols;
using Aran.Omega.ViewModels;
using MahApps.Metro.Controls;

namespace Aran.Omega.View
{
    /// <summary>
    /// Interaction logic for Window.xaml
    /// </summary>
    public partial class EtodForm : MetroWindow
    {
        public EtodForm()
        {
            InitializeComponent();
           var viewModel = new EtodViewModel();
           
            if (viewModel.CloseAction == null)
                viewModel.CloseAction += new Action(() => this.Close());
            Closing += viewModel.OnWindowClosing;
            
            viewModel.Init();
            DataContext = viewModel;
            
            //Uri iconUri = new Uri("pack://omega:/icon1.ico", UriKind.RelativeOrAbsolute);

            //var uriSource = new Uri(@"pack://application:,,,/Omega;component/Resources/Icon1.ico");
            //this.Icon = BitmapFrame.Create(uriSource);
            //this.Icon = BitmapFrame.Create(new Uri(@"D:\AirNav\Aran\Omega\Trunk\Resources\panda.ico"));

        }

        private static bool IsTextAllowed(string text)
        {
            Regex regex = new Regex("[^0-9.-]+"); //regex that matches disallowed text
            return !regex.IsMatch(text);
        }

        private void TxtBlInnerEdgeLength_OnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsTextAllowed(e.Text);
        }

        private void LoadingPanel_GiveFeedback(object sender, GiveFeedbackEventArgs e)
        {

        }
    }
}
