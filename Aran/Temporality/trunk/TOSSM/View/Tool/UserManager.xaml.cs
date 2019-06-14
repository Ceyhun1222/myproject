using System.Windows;
using System.Windows.Controls;
using Aran.Temporality.CommonUtil.ViewModel;

namespace TOSSM.View.Tool
{
    /// <summary>
    /// Interaction logic for UserManager.xaml
    /// </summary>
    public partial class UserManager : UserControl
    {
        public UserManager()
        {
            InitializeComponent();
        }

        private void UserManager_OnLoaded(object sender, RoutedEventArgs e)
        {
            var model = EasyUserManagementViewControl.DataContext as EasyUserManagementViewModel;
            if (model == null)
            {
                model = new EasyUserManagementViewModel();
                EasyUserManagementViewControl.DataContext = model;
            }
            model.Load();
        }
    }
}
