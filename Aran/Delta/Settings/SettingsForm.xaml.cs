using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
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
using ESRI.ArcGIS.esriSystem;

namespace Aran.Delta.Settings
{
    /// <summary>
    /// Interaction logic for SettingsForm.xaml
    /// </summary>
    public partial class SettingsForm : UserControl
    {
        private SettingsViewModel _settingsViewModel;
        private System.Windows.Forms.MaskedTextBox mtbDate;
        public SettingsForm()
        {
            InitializeComponent();
            // Create the MaskedTextBox control.
            mtbDate = new System.Windows.Forms.MaskedTextBox("00/00/0000");
            _settingsViewModel = new SettingsViewModel();
            this.DataContext = _settingsViewModel;
        }

        public void LoadAll()
        {
             _settingsViewModel.Load();

            btnPoint.Content = SelectImage(_settingsViewModel.Symbols.PointSymbol);
            btnLineCourse.Content = SelectImage(_settingsViewModel.Symbols.LineCourseSymbol);
            btnLineDist.Content = SelectImage(_settingsViewModel.Symbols.LineDistanceSymbol);
            btnResultPoint.Content = SelectImage(_settingsViewModel.Symbols.ResultPointSymbol);
            btnText.Content = SelectImage(_settingsViewModel.Symbols.TextSymbol);
            btnBuffer.Content = SelectImage(_settingsViewModel.Symbols.BufferSymbol);
            // TextOptions.SetTextFormattingMode(this, TextFormattingMode.Display); 
        }

        private System.Windows.Controls.Image SelectImage(ESRI.ArcGIS.Display.ISymbol symbol)
        {
            //SolidColorBrush solidBrush = button.Background as System.Windows.Media.SolidColorBrush;
            int color = Aran.PANDA.Common.ARANFunctions.RGB(247, 245, 245);

            var imageSize = new System.Drawing.Size((int)btnBuffer.Width, (int)btnBuffer.Height);
            using (Graphics gr = mtbDate.CreateGraphics())
            {
                Bitmap bitmap = StyleFunctions.SymbolToBitmap(symbol, imageSize, gr, color);
                using (MemoryStream memory = new MemoryStream())
                {
                    bitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Png);
                    memory.Position = 0;
                    BitmapImage bitmapImage = new BitmapImage();
                    bitmapImage.BeginInit();
                    bitmapImage.StreamSource = memory;
                    bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                    bitmapImage.EndInit();
                    System.Windows.Controls.Image image = new System.Windows.Controls.Image();

                    image.Source = bitmapImage;
                    image.Stretch = Stretch.Fill;
                    return image;
                }
            }
        }

        private void btnSelect_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;

            ESRI.ArcGIS.Display.ISymbol outSymbol;
            ESRI.ArcGIS.Display.ISymbol inSymbol = new ESRI.ArcGIS.Display.TextSymbol() as ESRI.ArcGIS.Display.ISymbol;
            if (sender == btnPoint)
            {
                if (Globals.SelectSymbol(_settingsViewModel.Symbols.PointSymbol, out outSymbol))
                {
                    var image = SelectImage(outSymbol);
                    button.Content = image;
                    _settingsViewModel.Symbols.PointSymbol = outSymbol;
                }
            }
            else if (sender == btnResultPoint)
            {
                if (Globals.SelectSymbol(_settingsViewModel.Symbols.ResultPointSymbol, out outSymbol))
                {
                    var image = SelectImage(outSymbol);
                    button.Content = image;
                    _settingsViewModel.Symbols.ResultPointSymbol = outSymbol;
                }
            }
            else if (sender == btnLineDist)
            {
                if (Globals.SelectSymbol(_settingsViewModel.Symbols.LineDistanceSymbol, out outSymbol))
                {
                    var image = SelectImage(outSymbol);
                    button.Content = image;
                    _settingsViewModel.Symbols.LineDistanceSymbol = outSymbol;
                }
            }
            else if (sender == btnLineCourse)
            {
                if (Globals.SelectSymbol(_settingsViewModel.Symbols.LineCourseSymbol, out outSymbol))
                {
                    var image = SelectImage(outSymbol);
                    button.Content = image;
                    _settingsViewModel.Symbols.LineCourseSymbol = outSymbol;
                }
            }
            else if (sender == btnBuffer)
            {
                if (Globals.SelectSymbol(_settingsViewModel.Symbols.BufferSymbol, out outSymbol))
                {
                    var image = SelectImage(outSymbol);
                    button.Content = image;
                    _settingsViewModel.Symbols.BufferSymbol= outSymbol;
                }
            }
            else if (sender == btnText)
            {
                if (Globals.SelectSymbol(_settingsViewModel.Symbols.TextSymbol, out outSymbol))
                {
                    var image = SelectImage(outSymbol);
                    button.Content = image;
                    _settingsViewModel.Symbols.TextSymbol = outSymbol;
                }
            }
            
            
        }
    }
}
