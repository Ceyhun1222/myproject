using System;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using PVT.Model;
using PVT.Utils;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing.Printing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace PVT.UI.ViewModel
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class ScreenshotViewModel : BaseViewModel
    {
        /// <summary>
        /// Initializes a new instance of the ReportViewModel class.
        /// </summary>
        /// 

        public ObservableCollection<Screenshot> Screenshots { get; set; }
        private readonly Feature _feature;

        private Screenshot _currentImages;

        public Screenshot CurrentImages
        {
            get => _currentImages;
            set
            {
                Set(() => CurrentImages, ref _currentImages, value);
                if (CurrentImages.Images.Count > 0)
                {
                    Image = CurrentImages.Images[0].BitmapImage;
                    _imageSourceIndex = 0;
                }
                else
                {
                    Image = null;
                    _imageSourceIndex = -1;
                }

                SetCanNextProperty();
                SetCanPreviousProperty();
            }
        }
        public ScreenshotViewModel(List<Screenshot> screenshots, Feature feature)
        {
            _feature = feature;
            Screenshots = new ObservableCollection<Screenshot>();
            foreach (var screenshot in screenshots)
            {
                Screenshots.Add(screenshot);
            }
            if (Screenshots.Count > 0)
                CurrentImages = Screenshots[0];

        }

        private bool _canNext;

        public bool CanNext
        {
            get => _canNext;
            set
            {
                Set(() => CanNext, ref _canNext, value, true);
            }
        }

        private bool _canPrevious;

        public bool CanPrevious
        {
            get => _canPrevious;
            set
            {
                Set(() => CanPrevious, ref _canPrevious, value, true);
            }
        }

        private int _imageSourceIndex = -1;
        private BitmapImage _image;

        public BitmapImage Image
        {
            get => _image;
            set
            {
                Set(() => Image, ref _image, value, true);
            }
        }

        private RelayCommand _nextCommand;

        public RelayCommand NextCommand => _nextCommand ?? (_nextCommand = new RelayCommand(
                                               NextExecute,
                                               CanNextExecute));

        private void NextExecute()
        {
            Image = CurrentImages.Images[++_imageSourceIndex].BitmapImage;
            SetCanNextProperty();
            SetCanPreviousProperty();
        }

        private void SetCanNextProperty()
        {
            CanNext = _imageSourceIndex < CurrentImages.Images.Count - 1;
            _nextCommand?.RaiseCanExecuteChanged();
        }

        private bool CanNextExecute()
        {
            return CanNext;
        }

        private RelayCommand _previousCommand;
        public RelayCommand PreviousCommand => _previousCommand ?? (_previousCommand = new RelayCommand(
                                                   PreviousExecute,
                                                   CanPreviousExecute));

        private void PreviousExecute()
        {
            Image = CurrentImages.Images[--_imageSourceIndex].BitmapImage;
            SetCanPreviousProperty();
            SetCanNextProperty();
        }

        private void SetCanPreviousProperty()
        {
            CanPrevious = _imageSourceIndex > 0;
            _previousCommand?.RaiseCanExecuteChanged();
        }

        private bool CanPreviousExecute()
        {
            return CanPrevious;
        }

        private RelayCommand _printCommand;

        public RelayCommand PrintCommand => _printCommand ?? (_printCommand = new RelayCommand(
                                                Print,
                                                CanPrint));


        private RelayCommand _printAllCommand;

        public RelayCommand PrintAllCommand => _printAllCommand ?? (_printAllCommand = new RelayCommand(
                                                   PrintAll,
                                                   CanPrint));

        private RelayCommand _pdfGenerateCommand;


        public RelayCommand PdfGenerateCommand => _pdfGenerateCommand
                                                  ?? (_pdfGenerateCommand = new RelayCommand(
                                                      GeneratePDF,
                                                      CanPrint));

        private void GeneratePDF()
        {
            try
            {
                var ofd = new Microsoft.Win32.SaveFileDialog() { Filter = "PDF Files (*.pdf)|*.pdf" };
                var result = ofd.ShowDialog();
                if (result == false) return;
                var fileName = ofd.FileName;
                var doc = PDFDocument.CreateDocument(fileName);
                var procedure = _feature as ProcedureBase;
                string text = "";

                if (procedure != null)
                    text = $"Aerodrome: {procedure.Aerodrome.FullName}\n" +
                           $"Runway|Direction: {procedure.RunwayDirection.FullName}\n" +
                           $"Procedure Name: {procedure.Name}\n" + $"Procedure Designator: {procedure.Designator}\n" +
                           $"Begin Date: {_feature.BeginDate}\n" + $"End Date: {_feature.EndDate}\n" +
                           $"Date of creation: {_currentImages.Date}";
                else
                {
                    var holding = _feature as HoldingPattern;
                    if (holding != null)
                        text = $"Holding point: {holding.HoldingPoint.Value}\n" +
                               $"Begin Date: {_feature.BeginDate}\n" + $"End Date: {_feature.EndDate}\n" +
                               $"Date of creation: {_currentImages.Date}";
                }
                
                doc.AddPage().AddH2().SetAlignment(PDFDocument.ParagraphAlignment.Center).
                    AddText(text);

                for (var i = 0; i < _currentImages.Images.Count; i++)
                {
                    doc.AddPage().AddImage(_currentImages.Images[i].Image);
                }
                BlockerModel.BlockForAction(() =>
                {
                    try
                    {
                        doc.Save();
                    }
                    catch (Exception ex)
                    {
                        Engine.Environment.Current.Logger.Error(ex, "PDF export failed");
                        MessageBox.Show("Pdf export failed.");

                    }
                });
            }
            catch (Exception ex)
            {
                Engine.Environment.Current.Logger.Error(ex, "PDF export failed");
                MessageBox.Show("Pdf export failed.");
            }
        }


        private void PrintAll()
        {
            var doc = new PrintDocument();
            doc.PrintPage += pd_PrintPage;
            doc.BeginPrint += pd_BeginPrint;
            var dlgSettings = new System.Windows.Forms.PrintDialog();

            dlgSettings.Document = doc;

            if (dlgSettings.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                BlockerModel.BlockForAction(() => { doc.Print(); });
            }
        }

        public int PageCounter { get; private set; }
        private void pd_PrintPage(object sender, PrintPageEventArgs e)
        {
            e.Graphics.DrawImage(CurrentImages.Images[PageCounter].Image, 10, 10);
            PageCounter++;
            e.HasMorePages = PageCounter != CurrentImages.Images.Count;
        }
        private void pd_BeginPrint(object sender, PrintEventArgs e)
        {
            PageCounter = 0;
        }


        private void Print()
        {
            var vis = new DrawingVisual();
            var dc = vis.RenderOpen();
            dc.DrawImage(Image, new Rect { Width = Image.Width, Height = Image.Height });
            dc.Close();

            var pdialog = new PrintDialog();
            if (pdialog.ShowDialog() == true)
            {
                pdialog.PrintVisual(vis, "Design History");
            }
        }

        private bool CanPrint()
        {
            if (Image == null)
                return false;
            return true;
        }



        private ObservableCollection<T> convert<T>(List<T> source)
        {
            var result = new ObservableCollection<T>();
            foreach (var item in source)
                source.Add(item);
            return result;
        }
    }
}