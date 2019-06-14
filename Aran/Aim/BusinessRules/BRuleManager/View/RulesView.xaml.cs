using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BRuleManager.View
{
    /// <summary>
    /// Interaction logic for RulesView.xaml
    /// </summary>
    public partial class RulesView : UserControl
    {
        public RulesView()
        {
            InitializeComponent();

            Loaded += RulesView_Loaded;
        }

        private void RulesView_Loaded(object sender, RoutedEventArgs e)
        {
            dynamic eo = DataContext;
            eo.RequestDelete = new Func<object, bool>(
                (arg) =>
                {
                    return MessageBox.Show(
                        eo._window as Window,
                        "Do you want to delete the selected rule?",
                        "Delete Rule",
                        MessageBoxButton.YesNoCancel,
                        MessageBoxImage.Question,
                        MessageBoxResult.No) == MessageBoxResult.Yes;
                });

            eo.GetSelectedRules = new Func<System.Collections.IList>(() => ui_rulesDataGrid.SelectedItems);
        }

        private void CheckAixmMessageFile_Click(object sender, RoutedEventArgs e)
        {
            var ofd = new OpenFileDialog
            {
                Filter = "Aixm XML Message (*.xml, *.gml)|*.xml;*.gml|All files (*.*)|*.*"
            };

            if (ofd.ShowDialog() == true)
                (DataContext as dynamic).CheckAixmMessageFile(ofd.FileName);
        }

        private void SearchTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                (DataContext as dynamic).SearchText = (sender as TextBox).Text;
        }
    }
}
