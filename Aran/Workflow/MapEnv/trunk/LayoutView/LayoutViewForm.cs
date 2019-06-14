using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.SystemUI;
using System.Drawing.Printing;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Output;
using ESRI.ArcGIS.Display;
using System.Collections;
using ESRI.ArcGIS.CartoUI;
using System.Diagnostics;
using Aran.AranEnvironment;

namespace MapEnv.LayoutView
{
    public partial class LayoutViewForm : Form
    {
        private PrintDialog _printDialog;
        private PrintDocument _printDocument;
        private PrintPreviewDialog _printPreviewDialog;
        private PageSetupDialog _pageSetupDialog;
        private bool _printerInitialised;
        private short _currentPrintPage;
        private ITrackCancel _trackCancel;
        private IAranLayoutViewGraphics _layouGraphics;

        private IActiveView ActiveView
        {
            get
            {
                return ui_esriLayoutControl.ActiveView;
            }
        }


        public LayoutViewForm(IAranLayoutViewGraphics layoutGraphics)
        {
            InitializeComponent();

            _layouGraphics = layoutGraphics;
            (_layouGraphics as AranLayoutViewGraphics).setLayoutControl(ui_esriLayoutControl);
            var cmd = new CustomCommand();
            cmd.Caption = "Refresh";
            cmd.Image = Properties.Resources.refresh_24;
            cmd.Clicked += (o, e) => { RefreshMap(); };
            ui_esriLayoutToolbarControl.AddItem(cmd, 0, 0, false, 0, esriCommandStyles.esriCommandStyleIconAndText);

            _trackCancel = new CancelTrackerClass();

            ui_esriMapToolbarControl.BackColor = BackColor;
            ui_esriLayoutToolbarControl.BackColor = BackColor;
        }


        private void LayoutViewForm_Load(object sender, EventArgs e)
        {
            Globals.MapDocument.Save(false, false);
            ui_esriLayoutControl.LoadMxFile(Globals.MapDocument.DocumentFilename);
            InitializePrint();
            _layouGraphics.PrepareLayout();
        }

        private void InitializePrint()
        {
            _currentPrintPage = 0;

            if (_printerInitialised)
                return;

            _printerInitialised = true;

            _printDialog = new PrintDialog();
            _printDocument = new PrintDocument();

            _printDocument.BeginPrint += PrintDocument_BeginPrint;
            _printDocument.PrintPage += PrintDocument_PrintPage;

            #region Print Preview
            _printPreviewDialog = new PrintPreviewDialog();
            //set the size, location, name and the minimum size the dialog can be resized to
            _printPreviewDialog.ClientSize = new System.Drawing.Size(800, 600);
            _printPreviewDialog.Location = new System.Drawing.Point(29, 29);
            _printPreviewDialog.Name = "PrintPreviewDialog";
            _printPreviewDialog.MinimumSize = new System.Drawing.Size(375, 250);
            //set UseAntiAlias to true to allow the operating system to smooth fonts
            _printPreviewDialog.UseAntiAlias = true;
            #endregion

            #region Page Setup
            _pageSetupDialog = new PageSetupDialog();
            _pageSetupDialog.PageSettings = new System.Drawing.Printing.PageSettings();
            _pageSetupDialog.PrinterSettings = new System.Drawing.Printing.PrinterSettings();
            //_pageSetupDialog.ShowNetwork = false;
            #endregion
        }

        public void RefreshMap()
        {
            IObjectCopy objectCopy = new ObjectCopy();
            object toCopyMap = Globals.MainForm.Map;
            IMap map = toCopyMap as IMap;
            map.IsFramed = false;
            var copiedMap = objectCopy.Copy(toCopyMap);
            object toOverwriteMap = ui_esriLayoutControl.ActiveView.FocusMap;
            objectCopy.Overwrite(copiedMap, ref toOverwriteMap);
            ui_esriLayoutControl.PageLayout.HorizontalSnapGuides.DrawLevel = esriViewDrawPhase.esriViewAll;
            ui_esriLayoutControl.PageLayout.HorizontalSnapGuides.AreVisible = true;
            _layouGraphics.Refresh();
            ActiveView.Refresh();
        }



        private void Print_Click(object sender, EventArgs e)
        {
            _printDialog.AllowSomePages = true;
            _printDialog.ShowHelp = true;
            _printDialog.Document = _printDocument;

            if (_printDialog.ShowDialog() == DialogResult.OK)
                _printDocument.Print();
        }

        private void PrintPreview_Click(object sender, EventArgs e)
        {
            if (ui_esriLayoutControl.DocumentFilename == null)
                return;

            _printDocument.DocumentName = Globals.Environment.DocumentFileName;
            _printPreviewDialog.Document = _printDocument;
            _printPreviewDialog.ShowDialog();
        }

        private void PrintDocument_BeginPrint(object sender, PrintEventArgs e)
        {
            _currentPrintPage = 0;
        }

        private void PrintDocument_PrintPage(object sender, PrintPageEventArgs e)
        {
            //this code will be called when the PrintPreviewDialog.Show method is called
            //set the PageToPrinterMapping property of the Page. This specifies how the page 
            //is mapped onto the printer page. By default the page will be tiled 
            //get the selected mapping option
            string sPageToPrinterMapping = null;
            if (sPageToPrinterMapping == null)
                //if no selection has been made the default is tiling
                ui_esriLayoutControl.Page.PageToPrinterMapping = esriPageToPrinterMapping.esriPageMappingTile;
            else if (sPageToPrinterMapping.Equals("esriPageMappingTile"))
                ui_esriLayoutControl.Page.PageToPrinterMapping = esriPageToPrinterMapping.esriPageMappingTile;
            else if (sPageToPrinterMapping.Equals("esriPageMappingCrop"))
                ui_esriLayoutControl.Page.PageToPrinterMapping = esriPageToPrinterMapping.esriPageMappingCrop;
            else if (sPageToPrinterMapping.Equals("esriPageMappingScale"))
                ui_esriLayoutControl.Page.PageToPrinterMapping = esriPageToPrinterMapping.esriPageMappingScale;
            else
                ui_esriLayoutControl.Page.PageToPrinterMapping = esriPageToPrinterMapping.esriPageMappingTile;

            //get the resolution of the graphics device used by the print preview (including the graphics device)
            short dpi = (short)e.Graphics.DpiX;
            //envelope for the device boundaries
            IEnvelope devBounds = new EnvelopeClass();
            //get page
            IPage page = ui_esriLayoutControl.Page;

            //the number of printer pages the page will be printed on
            short printPageCount;
            printPageCount = ui_esriLayoutControl.get_PrinterPageCount(0);
            _currentPrintPage++;

            //the currently selected printer
            IPrinter printer = ui_esriLayoutControl.Printer;
            //get the device bounds of the currently selected printer
            page.GetDeviceBounds(printer, _currentPrintPage, 0, dpi, devBounds);

            //structure for the device boundaries
            tagRECT deviceRect;
            //Returns the coordinates of lower, left and upper, right corners
            double xmin, ymin, xmax, ymax;
            devBounds.QueryCoords(out xmin, out ymin, out xmax, out ymax);
            //initialize the structure for the device boundaries
            deviceRect.bottom = (int)ymax;
            deviceRect.left = (int)xmin;
            deviceRect.top = (int)ymin;
            deviceRect.right = (int)xmax;

            //determine the visible bounds of the currently printed page
            IEnvelope visBounds = new EnvelopeClass();
            page.GetPageBounds(printer, _currentPrintPage, 0, visBounds);

            //get a handle to the graphics device that the print preview will be drawn to
            IntPtr hdc = e.Graphics.GetHdc();

            //print the page to the graphics device using the specified boundaries 
            ui_esriLayoutControl.ActiveView.Output(hdc.ToInt32(), dpi, ref deviceRect, visBounds, _trackCancel);

            //release the graphics device handle
            e.Graphics.ReleaseHdc(hdc);

            //check if further pages have to be printed
            if (_currentPrintPage < printPageCount)
                e.HasMorePages = true; //document_PrintPage event will be called again
            else
                e.HasMorePages = false;
        }

        private void PageSetup_Click(object sender, EventArgs e)
        {
            if (_pageSetupDialog.ShowDialog() != DialogResult.OK)
                return;

            var prSettings = _pageSetupDialog.PrinterSettings;
            _printDocument.PrinterSettings = prSettings;
            _printDocument.DefaultPageSettings = _pageSetupDialog.PageSettings;

            int i;
            IEnumerator paperSizes = prSettings.PaperSizes.GetEnumerator();
            paperSizes.Reset();

            for (i = 0; i < prSettings.PaperSizes.Count; ++i)
            {
                paperSizes.MoveNext();
                if (((PaperSize)paperSizes.Current).Kind == _printDocument.DefaultPageSettings.PaperSize.Kind)
                    _printDocument.DefaultPageSettings.PaperSize = ((PaperSize)paperSizes.Current);
            }

            /////////////////////////////////////////////////////////////
            ///initialize the current printer from the printer settings selected
            ///in the page setup dialog
            /////////////////////////////////////////////////////////////
            IPaper paper = new PaperClass(); //create a paper object
            IPrinter printer = new EmfPrinterClass(); //create a printer object
            paper.Attach(
                prSettings.GetHdevmode(_pageSetupDialog.PageSettings).ToInt32(),
                prSettings.GetHdevnames().ToInt32());
            printer.Paper = paper;
            ui_esriLayoutControl.Printer = printer;
        }


        private void OpenInArcMap_Click(object sender, EventArgs e)
        {
            var arcMapFileName = Globals.Settings.ArcMapFileName;

            if (!System.IO.File.Exists(arcMapFileName))
            {
                var ofd = new OpenFileDialog();
                ofd.Title = "Select ArcMap application file path";
                ofd.Filter = "Application (*.exe)|*.exe";
                if (ofd.ShowDialog() != DialogResult.OK)
                    return;

                arcMapFileName = ofd.FileName;
                Globals.Settings.ArcMapFileName = arcMapFileName;
            }

            //Globals.MapDocument.Save(false, false);

            Globals.MapDocument.ReplaceContents(ui_esriLayoutControl.ActiveView.FocusMap as IMxdContents);

            var mapEnvDocDir = Globals.GetAndCreateMapEnvDocumentsDir();
            var newMxdFileName = string.Format("{0}\\{1}-{2}.mxd",
                mapEnvDocDir,
                System.IO.Path.GetFileNameWithoutExtension(Globals.Environment.DocumentFileName),
                DateTime.Now.ToString("yyMMdd-hhmm"));

            Globals.MapDocument.SaveAs(newMxdFileName, false, false);

            //System.IO.File.Copy(ui_esriLayoutControl.DocumentFilename, newMxdFileName, true);

            Process.Start(arcMapFileName, "\"" + newMxdFileName + "\"");
        }

        public static string ProgramFilesx86()
        {
            if (8 == IntPtr.Size
                || (!String.IsNullOrEmpty(Environment.GetEnvironmentVariable("PROCESSOR_ARCHITEW6432"))))
            {
                return Environment.GetEnvironmentVariable("ProgramFiles(x86)");
            }

            return Environment.GetEnvironmentVariable("ProgramFiles");
        }

    }
}
