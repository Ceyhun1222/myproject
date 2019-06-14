using Aran.Delta.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Aran.Delta.View.Save
{
    /// <summary>
    /// Interaction logic for SaveDesigningArea.xaml
    /// </summary>
    public partial class SaveDesigningArea : Window
    {
        private Model.DesigningArea _designingArea;
        private readonly List<string> _units;
        public SaveDesigningArea(Model.DesigningArea designingArea)
        {
            InitializeComponent();
            _designingArea = designingArea;
            TxtLowerLimit.Text = "0";
            TxtUpperLimit.Text = "0";

            var designer = "";
            var windowsIdentity = System.Security.Principal.WindowsIdentity.GetCurrent();
            if (windowsIdentity != null)
                designer = windowsIdentity.Name;
            
            if (GlobalParams.DesigningAreaReader != null)
            {
                var itemList = GlobalParams.DesigningAreaReader.GetDesigningAreas();
                if (itemList.Count > 0)
                {
                    var item = itemList[itemList.Count - 1];
                    if (!String.IsNullOrEmpty(item.Designer))
                        designer = item.Designer;
                }
            }

            TxtDesignerName.Text =designer;

            _units = new List<string> { "FL", "FT", "M"};
            CmbLowerUom.ItemsSource = _units;
            CmbUpperUom.ItemsSource = _units;

            CmbUpperUom.SelectedIndex = 0;
            CmbLowerUom.SelectedIndex = 0;

            CmbType.ItemsSource = Enum.GetNames(typeof(PDM.AirspaceType));
            CmbType.SelectedIndex = 1;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TxtName.Text))
            {
                Messages.Warning("Name can not be empty!");
                return;
            }

            _designingArea.Name = TxtName.Text;
            _designingArea.Designer = TxtDesignerName.Text;
            _designingArea.LowerLimit =Convert.ToDouble(TxtLowerLimit.Text);
            _designingArea.UpperLimit = Convert.ToDouble(TxtUpperLimit.Text);
            _designingArea.UomLowerLimit =(string)CmbLowerUom.SelectedItem;
            _designingArea.UomUpperLimit = (string)CmbUpperUom.SelectedItem;
            _designingArea.CodeType =(string)CmbType.SelectedItem;
            this.DialogResult = true;
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
