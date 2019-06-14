using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAS.Model
{
    public class MainModel : BaseModel
    {
        private bool _isFieldsEnabled;
        private byte[] _dataBlockBuffer;


        public MainModel()
        {
            OperationTypeValues = new List<ComboBoxItem<int>>();
            OperationTypeValues.Add(new ComboBoxItem<int>(0, "Straight-in and offset"));
            for (int i = 1; i < 16; i++)
                OperationTypeValues.Add(new ComboBoxItem<int>(i, "Reserved"));
            OperationTypeValues.ForEach(cbi => cbi.IndexSymCount = 2);


            ServiceProviderValues = new List<ComboBoxItem<int>>();
            ServiceProviderValues.Add(new ComboBoxItem<int>(0, "WAAS"));
            ServiceProviderValues.Add(new ComboBoxItem<int>(1, "EGNOS"));
            ServiceProviderValues.Add(new ComboBoxItem<int>(2, "MSAS"));
            for (int i = 3; i < 13; i++)
                ServiceProviderValues.Add(new ComboBoxItem<int>(i, "Spare"));
            ServiceProviderValues.Add(new ComboBoxItem<int>(14, "Any service provider"));
            ServiceProviderValues.Add(new ComboBoxItem<int>(15, "Reserved"));
            ServiceProviderValues.ForEach(cbi => cbi.IndexSymCount = 2);


            RunwayTypeValues = new List<ComboBoxItem<int>>();
            RunwayTypeValues.Add(new ComboBoxItem<int>(0, "None"));
            RunwayTypeValues.Add(new ComboBoxItem<int>(1, "Right"));
            RunwayTypeValues.Add(new ComboBoxItem<int>(2, "Centre"));
            RunwayTypeValues.Add(new ComboBoxItem<int>(3, "Left"));


            ApproachPerformanceDesignatorValues = new List<ComboBoxItem<int>>();
            ApproachPerformanceDesignatorValues.Add(new ComboBoxItem<int>(0, "Spare"));
            ApproachPerformanceDesignatorValues.Add(new ComboBoxItem<int>(1, "Category I"));
            ApproachPerformanceDesignatorValues.Add(new ComboBoxItem<int>(2, "reserved for Category II"));
            ApproachPerformanceDesignatorValues.Add(new ComboBoxItem<int>(3, "reserved for Category III"));
            for (var i = 4; i < 8; i++)
                ApproachPerformanceDesignatorValues.Add(new ComboBoxItem<int>(i, "Spare"));


            RouteIndicatorValues = new List<ComboBoxItem<char>>();
            RouteIndicatorValues.Add(new ComboBoxItem<char>(' ', ""));
            for (char c = 'A'; c <= 'Z'; c++)
            {
                if (c != 'I' && c != 'O')
                    RouteIndicatorValues.Add(new ComboBoxItem<char>(c, c.ToString()));
            }

            ThresholdCrossingHeightValue = new List<ComboBoxItem<int>>();
            ThresholdCrossingHeightValue.Add(new ComboBoxItem<int>(0, "feet"));
            ThresholdCrossingHeightValue.Add(new ComboBoxItem<int>(1, "metres"));

            Data = new InputData();

            CalculateCommand = new RelayCommand(OnCalculateClicked);
            ReportCommand = new RelayCommand(OnReportClicked);
            SaveBinFileCommand = new RelayCommand(OnSaveFileClicked);
            _isFieldsEnabled = true;
        }


        public List<ComboBoxItem<int>> OperationTypeValues { get; private set; }

        public List<ComboBoxItem<int>> ServiceProviderValues { get; private set; }

        public List<ComboBoxItem<int>> RunwayTypeValues { get; private set; }

        public List<ComboBoxItem<int>> ApproachPerformanceDesignatorValues { get; private set; }

        public List<ComboBoxItem<char>> RouteIndicatorValues { get; private set; }

        public List<ComboBoxItem<int>> ThresholdCrossingHeightValue { get; private set; }

        public bool IsFieldEnabled
        {
            get
            {
                return _isFieldsEnabled;
            }
            set
            {
                if (_isFieldsEnabled == value)
                    return;

                _isFieldsEnabled = value;
                NotifyPropertyChanged("IsFieldEnabled");
            }
        }
        

        public RelayCommand CalculateCommand { get; private set; }

        public RelayCommand ReportCommand { get; private set; }

        public RelayCommand SaveBinFileCommand { get; private set; }

        public InputData Data { get; private set; }


        private void OnCalculateClicked(object sender)
        {
            _dataBlockBuffer = null;

            if (!IsFieldEnabled)
            {
                IsFieldEnabled = true;
                return;
            }

            #region Test - 1
            {
                //Data = new InputData
                //{
                //    OperationType = 0,
                //    ServiceProviderIdentifier = 2,
                //    AirportIdentifier = "ABCD",
                //    Runway = "RY30",
                //    RunwayType = 1,
                //    ApproachPerformanceDesignator = 0,
                //    RouteIndicator = 'B',
                //    ReferencePathDataSelector = 20,
                //    ReferencePathIdentifier = "W09D",
                //    LtpFtpCoordinate = new System.Windows.Point(
                //        DmsControll.Functions.DMS2DD(22,54,36.25,1),
                //        DmsControll.Functions.DMS2DD(106, 32, 47.8780, 1)),
                //    LtpFtpEllipsoidalHeight = 35.6,
                //    FpapCoordinate = new System.Windows.Point(
                //        DmsControll.Functions.DMS2DD(22, 54, 38.25, 1),
                //        DmsControll.Functions.DMS2DD(106, 32, 48.8780, 1)),
                //    ThresholdCrossingHeight = 55.0,
                //    ThresholdCrossingHeightUom = 0,
                //    GlidepathAngle = 2.75,
                //    CourseWidth = 106.5,
                //    LengthOffset = 424,
                //    HAL = 40.0,
                //    VAL = 50.0
                //};
            }
            #endregion

            #region Test - 2
            {
                //Data = new InputData
                //{
                //    OperationType = 0,
                //    ServiceProviderIdentifier = 1,
                //    AirportIdentifier = "LZKZ",
                //    Runway = "RW19",
                //    RunwayType = 0,
                //    ApproachPerformanceDesignator = 0,
                //    RouteIndicator = ' ',
                //    ReferencePathDataSelector = 0,
                //    ReferencePathIdentifier = "E19A",
                //    LtpFtpCoordinate = new System.Windows.Point(
                //        DmsControll.Functions.DMS2DD(48, 40, 35.8065, 1),
                //        DmsControll.Functions.DMS2DD(21, 14, 45.2485, 1)),
                //    LtpFtpEllipsoidalHeight = 269.8,
                //    FpapCoordinate = new System.Windows.Point(
                //        DmsControll.Functions.DMS2DD(48, 38, 58.0125, 1),
                //        DmsControll.Functions.DMS2DD(21, 14, 11.0615, 1)),
                //    ThresholdCrossingHeight = 15.0,
                //    ThresholdCrossingHeightUom = 1,
                //    GlidepathAngle = 3,
                //    CourseWidth = 105.0,
                //    LengthOffset = 0,
                //    HAL = 40.0,
                //    VAL = 50.0
                //};
            }
            #endregion

            #region Test - 3
            {
                //Data = new InputData
                //{
                //    OperationType = 0,
                //    ServiceProviderIdentifier = 15,
                //    AirportIdentifier = "LFBO",
                //    Runway = "RW15",
                //    RunwayType = 1,
                //    ApproachPerformanceDesignator = 1,
                //    RouteIndicator = 'C',
                //    ReferencePathDataSelector = 3,
                //    ReferencePathIdentifier = "GTBS",
                //    LtpFtpCoordinate = new System.Windows.Point(-1.345940, 43.6441075),
                //    LtpFtpEllipsoidalHeight = 197.3,
                //    FpapCoordinate = new System.Windows.Point(0.026175 - 1.345940, 43.6441075 - 0.025145),
                //    ThresholdCrossingHeight = 17.05,
                //    ThresholdCrossingHeightUom = 1,
                //    GlidepathAngle = 3,
                //    CourseWidth = 105.0,
                //    LengthOffset = 0,
                //    HAL = 10.0,
                //    VAL = 40.0
                //};
            }
            #endregion

            try
            {
                var buffer = Data.GetBuffer();
                _dataBlockBuffer = buffer;
                IsFieldEnabled = false;
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void OnReportClicked(object sender)
        {
            string title;
            string dataBlock;
            string crcText;
            List<List<string[]>> linesList = Data.GetReportData(out title, out dataBlock, out crcText);

            var sfd = new Microsoft.Win32.SaveFileDialog();
            sfd.Filter = "HTML (*.html)|*.html|All Files (*.*)|(*.*)";

            if (sfd.ShowDialog() != true)
                return;

            var fileName = sfd.FileName;
            using (var sw = File.CreateText(sfd.FileName))
            {
                sw.WriteLine("<!DOCTYPE html>");
                sw.WriteLine("<html>");
                sw.WriteLine("<head>");
                sw.WriteLine("<style>");

                sw.WriteLine("table, th, td {");
                sw.WriteLine("    border: 1px solid black;");
                sw.WriteLine("    border-collapse: collapse;");
                sw.WriteLine("}");
                sw.WriteLine("th, td {");
                sw.WriteLine("    padding: 5px;");
                sw.WriteLine("}");

                sw.WriteLine("h1 { text-align: center; }");

                sw.WriteLine("p.result {");
	            sw.WriteLine("border: 1px solid black;");
	            sw.WriteLine("padding: 10px 10px 10px 10px;");
	            sw.WriteLine("max-width: 300px;");
                sw.WriteLine("word-spacing: 8px;");
                sw.WriteLine("font-family: monospace;");
                sw.WriteLine("word-wrap: break-word; }");

                sw.WriteLine("</style>");
                sw.WriteLine("</head>");
                sw.WriteLine("<body>");

                sw.WriteLine("");
                sw.WriteLine("<h1>" + title + "</h1><br>");
                //sw.WriteLine("<table style=\"width:100%\">");
                //sw.WriteLine("  <tr>");
                //sw.WriteLine("    <th>Parameter</th>");
                //sw.WriteLine("    <th>Value</th>");
                //sw.WriteLine("  </tr>");

                var writeAddData = false;

                foreach (var lines in linesList)
                {
                    sw.WriteLine("<table style=\"width:100%\">");
                    sw.WriteLine("  <tr>");
                    sw.WriteLine("    <th>Parameter</th>");
                    sw.WriteLine("    <th>Value</th>");
                    sw.WriteLine("  </tr>");

                    foreach (var sa in lines)
                    {
                        sw.WriteLine("<tr>");
                        sw.WriteLine("<td><b>" + sa[0] + "</b></td>");
                        sw.WriteLine("<td>" + sa[1] + "</td>");
                        sw.WriteLine("</tr>");
                    }

                    sw.WriteLine("</table>");

                    if (!writeAddData)
                    {
                        writeAddData = true;
                        sw.Write("<h4>Non-FAS Datablock fields:</h4>");
                    }
                }
                

                sw.Write("<h4>Data Block:</h4>" +
                    "<p class=\"result\">" + dataBlock + "</p>");

                sw.Write("<h4>CRC Value:</h4>" +
                    "<p class=\"result\">" + crcText + "</p>");

                sw.WriteLine("</body>");
                sw.WriteLine("</html>");

                sw.Close();
            }
        }

        private void OnSaveFileClicked(object sender)
        {
            if (_dataBlockBuffer == null)
                return;

            var sfd = new Microsoft.Win32.SaveFileDialog();
            if (true != sfd.ShowDialog())
                return;

            if (File.Exists(sfd.FileName))
            {
                try
                {
                    File.Delete(sfd.FileName);
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show(string.Format("Could not ovverrite file: {0}\nError: {1}", sfd.FileName, ex.Message));
                    return;
                }
            }

            var file = new FileStream(sfd.FileName, FileMode.Create);
            file.Write(_dataBlockBuffer, 0, _dataBlockBuffer.Length);
            file.Close();
        }


    }
}
