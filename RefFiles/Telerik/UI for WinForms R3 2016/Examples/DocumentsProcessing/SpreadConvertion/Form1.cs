using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls.UI;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.TextBased.Csv;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.TextBased.Txt;
using Telerik.Windows.Documents.Spreadsheet.Model;

namespace SpreadConvertion
{
    public partial class Form1 : RadForm
    {
        private static readonly string SampleDocumentFile = "SpreadConvertion.Resources.SampleDocument.xlsx";
        private List<IWorkbookFormatProvider> providers;

        public Form1()
        {
            InitializeComponent();

            if (Program.themeName != "") //set the example theme to the same theme QSF uses
            {
                Telerik.WinControls.ThemeResolutionService.ApplicationThemeName = Program.themeName;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.fileExtensionsDropDownList.SelectedIndex = 1;
            this.providers = new List<IWorkbookFormatProvider>()
            {
                new XlsxFormatProvider(),
                new CsvFormatProvider(),
                new TxtFormatProvider()
            };            
        }

        private void loadCustomDocumentButton_Click(object sender, EventArgs args)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Xlsx files|*.xlsx|Csv files|*.Csv|Text files|*.txt|All files (*.*)|*.*";
            dialog.FilterIndex = 1;
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                string extension = Path.GetExtension(dialog.FileName);
                IWorkbookFormatProvider provider = this.providers
                    .FirstOrDefault(p => p.SupportedExtensions
                        .Any(e => string.Compare(extension, e, StringComparison.InvariantCultureIgnoreCase) == 0));

                if (provider != null)
                {
                    try
                    {
                        using (Stream stream = dialog.OpenFile())
                        {
                            this.Workbook = provider.Import(stream);
                            this.FileName = Path.GetFileName(dialog.FileName);
                            this.saveButton.Enabled = true;
                        }
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Could not open file.");
                        this.Workbook = null;
                        this.FileName = null;
                    }
                }
                
                else
                {
                    MessageBox.Show("Could not open file.");
                }
            }
        }

        private Workbook workbook;
        public Workbook Workbook
        {
            get
            {
                return this.workbook;
            }
            set
            {
                if (this.workbook != value)
                {
                    this.workbook = value;
                    this.IsDocumentLoaded = value != null;                    
                }
            }
        }

        private bool isDocumentLoaded;
        public bool IsDocumentLoaded
        {
            get
            {
                return this.isDocumentLoaded;
            }
            set
            {
                if (this.isDocumentLoaded != value)
                {
                    this.isDocumentLoaded = value;                 
                }
            }
        }

        private string fileName;
        public string FileName
        {
            get
            {
                return this.fileName;
            }
            set
            {
                if (this.fileName != value)
                {
                    this.fileName = value;
                    this.fileNameLabel.Text = value;
                }
            }
        }

        private IEnumerable<string> exportFormats;
        public IEnumerable<string> ExportFormats
        {
            get
            {
                if (exportFormats == null)
                {
                    exportFormats = new string[] { "Xlsx", "Csv", "Txt" };
                }

                return exportFormats;
            }
        }

        private void loadSampleDocumentButton_Click(object sender, EventArgs e)
        {
            using (Stream stream = FileHelper.GetSampleResourceStream(SampleDocumentFile))
            {
                this.Workbook = new XlsxFormatProvider().Import(stream);
                this.FileName = Path.GetFileName(SampleDocumentFile);
                this.saveButton.Enabled = true;
            }
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            string selectedFromat = this.fileExtensionsDropDownList.Text;
            FileHelper.SaveDocument(this.Workbook, selectedFromat);
        }

        private static IWorkbookFormatProvider GetFormatProvider(string extension)
        {
            IWorkbookFormatProvider formatProvider;
            switch (extension)
            {
                case "Xlsx":
                    formatProvider = WorkbookFormatProvidersManager.GetProviderByName("XlsxFormatProvider");
                    break;
                case "Csv":
                    formatProvider = WorkbookFormatProvidersManager.GetProviderByName("CsvFormatProvider");
                    (formatProvider as CsvFormatProvider).Settings.HasHeaderRow = true;
                    break;
                case "Txt":
                    formatProvider = WorkbookFormatProvidersManager.GetProviderByName("TxtFormatProvider");
                    break;
                default:
                    formatProvider = null;
                    break;
            }

            return formatProvider;
        }
    }
}
