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
using System.Windows.Shapes;
using Aran.Delta.Model;
using MahApps.Metro.Controls;

namespace Aran.Delta.View
{
    /// <summary>
    /// Interaction logic for CreateAirspace.xaml
    /// </summary>
    public partial class CreateAirspace : MetroWindow
    {
        private ViewModels.CreateAirspaceViewModel _createAirspaceViewModel;
        

        public CreateAirspace()
        {
            InitializeComponent();
            _createAirspaceViewModel = new ViewModels.CreateAirspaceViewModel();
            _createAirspaceViewModel.RequestClose += () => this.Close();
            this.DataContext = _createAirspaceViewModel;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _createAirspaceViewModel.Clear();
        }

        private void PtGrid_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _createAirspaceViewModel.SelectedPointList = PtGrid.SelectedItems.Cast<AreaPointModel>().ToList();
        }

        private void MenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            ApplicationCommands.Copy.Execute(
                     null, PtGrid);
            string result = (string)Clipboard.GetData(DataFormats.Text);
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

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
            var dmsControl = DmsControl as FrameworkElement;
            dmsControl.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
        }

        private void SupportedFormatList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           // WaterMark.Text = _waterMarkList[SupportedFormatList.SelectedIndex].ExampleText;
        }
    }
}
