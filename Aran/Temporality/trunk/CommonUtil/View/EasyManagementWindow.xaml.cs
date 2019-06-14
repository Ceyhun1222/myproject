using System.Windows;
using Aran.Temporality.CommonUtil.ViewModel;

namespace Aran.Temporality.CommonUtil.View
{
    /// <summary>
    /// Interaction logic for EasyManagementWindow.xaml
    /// </summary>
    public partial class EasyManagementWindow : Window
    {
        public EasyManagementWindow()
        {
            InitializeComponent();
        }

        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            var model = EasyUserManagementViewControl.DataContext as  EasyUserManagementViewModel;
            if (model==null)
            {
                model=new EasyUserManagementViewModel();
                EasyUserManagementViewControl.DataContext = model;
            }
            model.Load();
        }
    }
}
