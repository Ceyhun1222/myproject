using Aran.Delta.Model;
using System;
using System.Collections.Generic;
using System.Linq;
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
    /// Interaction logic for SaveDesigningBuffer.xaml
    /// </summary>
    public partial class SaveDesigningBuffer : Window
    {
        private Model.DesigningBuffer _designingBuffer;
        private List<string> _units;
        public SaveDesigningBuffer(Model.DesigningBuffer designingBuffer)
        {
            InitializeComponent();
            _designingBuffer = designingBuffer;

            _units =new List<string> { "FL", "FT", "M"};
            CmbLowerUom.ItemsSource = _units;
            CmbUpperUom.ItemsSource = _units;

            var designer = "";
            var windowsIdentity = System.Security.Principal.WindowsIdentity.GetCurrent();
            if (windowsIdentity != null)
                designer = windowsIdentity.Name;

            if (GlobalParams.DesigningAreaReader != null)
            {
                var itemList = GlobalParams.DesigningAreaReader.GetDesigningBuffers();
                if (itemList.Count > 0)
                {
                    var item = itemList[itemList.Count - 1];
                    if (!String.IsNullOrEmpty(item.Designer))
                        designer = item.Designer;
                }
            }

            TxtDesignerName.Text = designer;

            if (designingBuffer.Name != null)
                TxtName.Text = designingBuffer.Name;

            TxtLowerLimit.Text = designingBuffer.LowerLimit.ToString();

            TxtUpperLimit.Text = designingBuffer.UpperLimit.ToString();

            if (designingBuffer.UomLowerLimit != null)
            {
                int index = _units.FindIndex(a => a.ToUpper() == designingBuffer.UomLowerLimit.ToUpper());
                if (index > -1)
                    CmbLowerUom.SelectedIndex = index;
            }
            else
                CmbLowerUom.SelectedIndex = 0;

            if (designingBuffer.UomUpperLimit != null)
            {
                int index = _units.FindIndex(a => a.ToUpper() == designingBuffer.UomUpperLimit.ToUpper());
                if (index > -1)
                    CmbUpperUom.SelectedIndex = index;
            }
            else
                CmbUpperUom.SelectedIndex = 0;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TxtName.Text))
            {
                Messages.Warning("Name can not be empty!");
                return;
            }

            _designingBuffer.Name = TxtName.Text;
            _designingBuffer.Designer = TxtDesignerName.Text;
            _designingBuffer.LowerLimit =Convert.ToDouble(TxtLowerLimit.Text);
            _designingBuffer.UpperLimit = Convert.ToDouble(TxtUpperLimit.Text);
            _designingBuffer.UomLowerLimit =(string)CmbLowerUom.SelectedItem;
            _designingBuffer.UomUpperLimit = (string)CmbUpperUom.SelectedItem;
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
