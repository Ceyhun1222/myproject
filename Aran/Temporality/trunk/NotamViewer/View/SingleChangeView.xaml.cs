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
using NotamViewer.ViewModel;

namespace NotamViewer.View
{
    /// <summary>
    /// Interaction logic for SingleChangeView.xaml
    /// </summary>
    public partial class SingleChangeView : UserControl
    {
        public SingleChangeView()
        {
            InitializeComponent();
        }

        private void ChangePresenterMouseDown(object sender, MouseButtonEventArgs e)
        {
            var model = DataContext as SingleChangeViewModel;
            if (model==null) return;
            model.PopupIsOpen = true;
        }

        private void ChangePopupMouseDown(object sender, MouseButtonEventArgs e)
        {
            var model = DataContext as SingleChangeViewModel;
            if (model == null) return;
            model.PopupIsOpen = false;
        }

        private void CloseButtonClick(object sender, RoutedEventArgs e)
        {
            ChangePopupMouseDown(null, null);
        }
    }
}
