using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using Aran.Omega.Models;
using ChoosePointNS;
using MahApps.Metro.Controls;

namespace Aran.Omega.View
{
    /// <summary>
    /// Interaction logic for Window.xaml
    /// </summary>
    public partial class OmegaMainForm : MetroWindow
    {
        public OmegaMainForm()
        {
            InitializeComponent();
            var olsViewModel = new ViewModels.OLSViewModel();
            if (olsViewModel.CloseAction == null)
                olsViewModel.CloseAction += this.Close;
            //Closing += olsViewModel.OnWindowClosing;
            Closing += CloseForm;
            olsViewModel.Init();
            DataContext = olsViewModel;

            lblElevationDatum.Content = "Elevation Datum (" + InitOmega.HeightConverter.Unit+" )";
            dGridRwy.Columns[3].Header = "Length (" + InitOmega.DistanceConverter.Unit + " )";
            TxtBlInnerEdgeLength.Text = "Strip Width (" + InitOmega.DistanceConverter.Unit + " )";

            this.SourceInitialized += (x, y) =>
            {
                this.HideMinimizeAndMaximizeButtons();
            };
            GlobalParams.OlsViewModel = olsViewModel;
        }

        private void CloseForm(object sender, EventArgs e)
        {
            Closing -= CloseForm;
            if (GlobalParams.OlsViewModel!=null)
                GlobalParams.OlsViewModel.OnWindowClosing(null,null);
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

    }
}
