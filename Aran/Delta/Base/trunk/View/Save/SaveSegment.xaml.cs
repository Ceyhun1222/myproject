using Aran.Delta.Model;
using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace Aran.Delta.View.Save
{
    /// <summary>
    /// Interaction logic for SaveSegment.xaml
    /// </summary>
    public partial class SaveSegment : Window
    {
        private Model.DesigningRoute _designingRoute;

        public SaveSegment(Model.DesigningRoute designingPoint)
        {
            InitializeComponent();
            _designingRoute = designingPoint;

            var designer = "";
            var windowsIdentity = System.Security.Principal.WindowsIdentity.GetCurrent();
            if (windowsIdentity != null)
                designer = windowsIdentity.Name;

            if (GlobalParams.DesigningAreaReader != null)
            {
                var itemList = GlobalParams.DesigningAreaReader.DesigningRoutes;
                if (itemList.Count > 0)
                {
                    var item = itemList[itemList.Count - 1];
                    if (!String.IsNullOrEmpty(item.Designer))
                        designer = item.Designer;
                }
            }

            TxtDesignerName.Text = designer;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TxtName.Text))
            {
                Messages.Warning("Name can not be empty!");
                return;
            }

            _designingRoute.Name = TxtName.Text;
            _designingRoute.Designer = TxtDesignerName.Text;
            this.DialogResult = true;
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
