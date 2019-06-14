using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using KeyEventArgs = System.Windows.Input.KeyEventArgs;
using TextBox = System.Windows.Controls.TextBox;

namespace Aran.Delta.View
{
    /// <summary>
    /// Interaction logic for ThreeDIntersection.xaml
    /// </summary>
    public partial class ThreeDIntersection : Window
    {
        private ViewModels.ThreeDIntersectionViewModel _vm;
        public ThreeDIntersection()
        {
            InitializeComponent();
            _vm = new ViewModels.ThreeDIntersectionViewModel();
            _vm.RequestClose += () => this.Close();
            this.DataContext = _vm;
        }

        private static bool IsTextAllowed(string text)
        {
            Regex regex = new Regex("[^0-9.-]+"); //regex that matches disallowed text
            return !regex.IsMatch(text);
        }

        private void TxtLatDeg_OnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsTextAllowed(e.Text);
        }

        private void ThreeDIntersection_OnClosing(object sender, CancelEventArgs e)
        {
            _vm.Clear();
        }

        private void UIElement_OnGotFocus(object sender, RoutedEventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox != null) textBox.SelectAll();
        }

        private void UIElement_OnKeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                var ue = sender as FrameworkElement;
                if (e.Key == Key.Enter)
                {
                    if (ue.Tag != null && ue.Tag.ToString() == "IgnoreEnterKeyTraversal")
                    {
                        //ignore
                    }
                    else
                    {
                        e.Handled = true;
                        ue.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
                    }
                }
            }
            catch (Exception)
            {
            }
        }
    }
}
