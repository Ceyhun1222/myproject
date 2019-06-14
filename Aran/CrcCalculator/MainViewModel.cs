using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using GalaSoft.MvvmLight;
using Aran.PANDA.Common;
using GalaSoft.MvvmLight.Command;
using Microsoft.Win32;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.Drawing.Imaging;
using System.Collections.Generic;
using System.Globalization;
using System.Security.Permissions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Threading;
using GalaSoft.MvvmLight.Threading;
using Point = System.Drawing.Point;

namespace CrcCalculator
{
    public class MainViewModel : ViewModelBase
    {
        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        private static extern IntPtr GetWindowRect(IntPtr hWnd, ref Rect rect);

        [StructLayout(LayoutKind.Sequential)]
        private struct Rect
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        private class DataStruct
        {
            public string Name;
            public string Latitude;
            //public string LatSign;
            public string Longitude;
            //public string LongSign;
            public double Elev;
            public string CalcStr;
            public string Crc;
            public byte[] Image;
        }

        private double _elev;
        private double _latitude;
        private double _longitude;
        private bool _calc1By1;
        private RelayCommand _sourceBrowseCommand;
        private bool _calcGroup;
        private bool _withScreenShots;
        private string[] _sourceFile;
        private RelayCommand _calculateCommand;
        private readonly List<DataStruct> _dataList;
        private string _sheetName;
        private int _rowCount;
        private string _sourceFile4View;

        public MainViewModel()
        {
            //SourceFile = @"C:\Users\AbuzarH\Desktop\test - Copy.xlsx";
            //WithScreenShots = true;
            //Latitude = 0;
            //Longitude = 0;
            //ScreenIsReady = false;
            CalcGroup = true;
            _dataList = new List<DataStruct>();
        }

        public double Elev
        {
            get => _elev;
            set
            {
                Set(() => Elev, ref _elev, value);
                RaisePropertyChanged(nameof(CalculationText));
                RaisePropertyChanged(nameof(CrcValue));
            }
        }

        public double Latitude
        {
            get => _latitude;
            set
            {
                Set(() => Latitude, ref _latitude, value);
                RaisePropertyChanged(nameof(CalculationText));
                RaisePropertyChanged(nameof(CrcValue));
            }
        }

        public double Longitude
        {
            get => _longitude;
            set
            {
                Set(() => Longitude, ref _longitude, value);
                RaisePropertyChanged(nameof(CalculationText));
                RaisePropertyChanged(nameof(CrcValue));
            }
        }

        public bool CalcGroup
        {
            get => _calcGroup;
            set { Set(() => CalcGroup, ref _calcGroup, value); }
        }

        public bool Calc1By1
        {
            get => _calc1By1;
            set { Set(() => Calc1By1, ref _calc1By1, value); }
        }

        private bool _isDdInFile;

        public bool IsDdInFile
        {
            get => _isDdInFile;
            set { Set(() => IsDdInFile, ref _isDdInFile, value); }
        }

        private bool _isDms1By1;

        public bool IsDms1By1
        {
            get => _isDms1By1;
            set { Set(() => IsDms1By1, ref _isDms1By1, value); }
        }

        public string[] SourceFiles
        {
            get => _sourceFile;
            set
            {
                Set(() => SourceFiles, ref _sourceFile, value);
                SourceFile4View = string.Join("; ", SourceFiles);
            }
        }

        public bool WithScreenShots
        {
            get => _withScreenShots;
            set { Set(() => WithScreenShots, ref _withScreenShots, value); }
        }

        private string _progressStr;
        private string _col0Name, _col1Name, _col2Name, _col3Name;

        public string ProgressStr
        {
            get => _progressStr;
            set { Set(() => ProgressStr, ref _progressStr, value); }
        }

        public RelayCommand CalculateCommand
        {
            get => _calculateCommand ?? (_calculateCommand = new RelayCommand(() =>
            {
                for(int k = 0; k<SourceFiles.Length; k++)
                {
                    var sourceFile = SourceFiles[k];
                    if (!File.Exists(sourceFile))
                    {
                        MessageBox.Show("File doesn't exist !\r\n\r\n" + SourceFiles, "File not found", MessageBoxButton.OK,
                            MessageBoxImage.Error);
                        return;
                    }
                    if (WithScreenShots)
                    {
                        CalcGroup = false;
                        Calc1By1 = true;
                    }

                    IsDms1By1 = !IsDdInFile;
                    XSSFWorkbook xssfwb;
                    _dataList.Clear();
                    using (var file = new FileStream(sourceFile, FileMode.Open, FileAccess.ReadWrite))
                    {
                        xssfwb = new XSSFWorkbook(file);
                        var sheet = xssfwb.GetSheetAt(0);
                        _sheetName = sheet.SheetName;
                        var enumerator = sheet.GetRowEnumerator();
                        IRow rw = null;
                        _rowCount = 0;
                        if (enumerator.MoveNext())
                            rw = enumerator.Current as IRow;
                        while (rw != null && rw.Cells.Count > 0 && rw.GetCell(0) != null)
                        {
                            _rowCount++;
                            if (enumerator.MoveNext())
                                rw = enumerator.Current as IRow;
                            else
                                rw = null;

                        }
                        _rowCount--;
                        //_rowCount = sheet.LastRowNum;
                        for (var index = 0; index <= _rowCount; index++)
                        {

                            var row = sheet.GetRow(index);
                            if (row == null || row.Cells.Count == 0) continue;

                            if (index == 0)
                            {
                                _col0Name = row.GetCell(0).StringCellValue;
                                _col1Name = row.GetCell(1).StringCellValue;
                                _col2Name = row.GetCell(2).StringCellValue;
                                _col3Name = row.GetCell(3).StringCellValue;
                                continue;
                            }

                            DataStruct dataStr = new DataStruct { Name = row.GetCell(0).StringCellValue };

                            var latStr = row.GetCell(1).StringCellValue;
                            double lat;
                            if (IsDdInFile)
                            {
                                if (!double.TryParse(latStr, out lat))
                                    throw new Exception(
                                        $"Couldn't parse latitude at cell[Row:{(_rowCount + 1)};Column:2] which is {latStr}");
                            }
                            else
                            {
                                latStr = latStr.Trim();

                                int degIndex = latStr.IndexOf('°');
                                int minIndex = latStr.IndexOf(@"'");
                                int secIndex = latStr.IndexOf('"');
                                double deg, min, sec;

                                if (!double.TryParse(latStr.Substring(0, 2), out deg) || !double.TryParse(latStr.Substring(degIndex + 1, minIndex - degIndex - 1), out min) || !double.TryParse(latStr.Substring(minIndex + 1, secIndex - minIndex - 1), out sec))
                                    throw new Exception(
                                        $"Couldn't parse latitude at cell[Row:{(_rowCount + 1)};Column:2] which is {latStr}");
                                var sign = latStr[latStr.Length - 1] == 'N' ? 1 : -1;
                                lat = Dms2Dd(deg, min, sec, sign);
                            }

                            dataStr.Latitude = latStr;
                            //if (row.GetCell(2).StringCellValue.ToLower().Trim() == "s")
                            //    lat = -lat;
                            //dataStr.LatSign = row.GetCell(2).StringCellValue;

                            var longStr = row.GetCell(2).StringCellValue;
                            double longitude;
                            if (IsDdInFile)
                            {
                                if (!double.TryParse(longStr, out longitude))
                                    throw new Exception(
                                        $"Couldn't parse longitude at cell[Row:{(_rowCount + 1)};Column:4] which is {longStr}");
                            }
                            else
                            {
                                longStr = longStr.Trim();
                                int degIndex = longStr.IndexOf('°');
                                int minIndex = longStr.IndexOf(@"'");
                                int secIndex = longStr.IndexOf('"');
                                double deg, min, sec;

                                if (!double.TryParse(longStr.Substring(0, degIndex), out deg) ||
                                    !double.TryParse(longStr.Substring(degIndex + 1, minIndex - degIndex - 1), out min) ||
                                    !double.TryParse(longStr.Substring(minIndex + 1, secIndex - minIndex - 1), out sec))
                                    throw new Exception(
                                        $"Couldn't parse longitude at cell[Row:{(_rowCount + 1)};Column:4] which is {longStr}");

                                var sign = longStr[longStr.Length - 1] == 'E' ? 1 : -1;
                                longitude = Dms2Dd(deg, min, sec, sign);

                            }

                            dataStr.Longitude = longStr;
                            //if (row.GetCell(4).StringCellValue.ToLower().Trim() == "w")
                            //    longitude = -longitude;
                            //dataStr.LongSign = row.GetCell(4).StringCellValue;
                            var elev = row.GetCell(3);
                            if (!double.TryParse(elev.ToString(), out dataStr.Elev))
                                throw new Exception(
                                    $"Couldn't parse longitude at cell[Row:{(_rowCount + 1)};Column:6] which is {elev}");

                            Latitude = lat;
                            Longitude = longitude;
                            Elev = dataStr.Elev;
                            //dataStr.CalcStr = CalculationText;
                            //dataStr.Crc = CrcValue;
                            _dataList.Add(dataStr);
                            row.CreateCell(4).SetCellValue(CrcValue);
                            row.CreateCell(5).SetCellValue(CalculationText);
                            if (WithScreenShots)
                            {
                                var k1 = k;
                                Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                                {
                                    CalcGroup = false;
                                    Calc1By1 = true;

                                    Latitude = lat;
                                    Longitude = longitude;
                                    Elev = dataStr.Elev;

                                    ProgressStr = $"Calculating {_dataList.Count}/{_rowCount} row from {(k1 + 1)}/{SourceFiles.Length} file(s)";
                                    //if (_dataList.Count == 1)
                                    //    ProgressStr += "st";
                                    //else if ((_dataList.Count) == 2)
                                    //    ProgressStr += "nd";
                                    //else if ((_dataList.Count) == 3)
                                    //    ProgressStr += "rd";
                                    //else
                                    //    ProgressStr += "th";
                                    //ProgressStr += $"";

                                    Application.Current.MainWindow.Dispatcher
                                        .BeginInvoke(DispatcherPriority.ApplicationIdle, new Action(() =>
                                        {
                                            Thread.Sleep(200);
                                            WriteScreenShot(sourceFile);
                                        })).Wait();

                                }), sourceFile, DispatcherPriority.ContextIdle, null);


                                Application.Current.MainWindow.Dispatcher.BeginInvoke(DispatcherPriority.ApplicationIdle,
                                    new Action(() =>
                                    {
                                        Thread.Sleep(200);
                                    })).Wait();
                            }
                            else
                            {
                                Application.Current.MainWindow.Dispatcher.BeginInvoke(new Action(() =>
                                {
                                    int k1 = k;
                                    ProgressStr = $"Calculating {_dataList.Count}/{_rowCount} row from {(k1 + 1)}/{SourceFiles.Length} file(s)";
                                    //if (_dataList.Count == 1)
                                    //    ProgressStr += "st";
                                    //else if ((_dataList.Count) == 2)
                                    //    ProgressStr += "nd";
                                    //else if ((_dataList.Count) == 3)
                                    //    ProgressStr += "rd";
                                    //else
                                    //    ProgressStr += "th";
                                    //ProgressStr += $"/{_rowCount} row from {(k1 + 1)}/{SourceFiles.Length} file(s)";
                                }), DispatcherPriority.ContextIdle, null);

                                Application.Current.MainWindow.Dispatcher.BeginInvoke(DispatcherPriority.ApplicationIdle,
                                    new Action(() =>
                                    {
                                        if (_dataList.Count == (_rowCount))
                                        {
                                            ProgressStr = "Done";
                                        //Save();
                                    }

                                        Thread.Sleep(400);
                                    })).Wait();
                            }
                        }
                    }

                    if (!WithScreenShots)
                    {
                        using (FileStream file = new FileStream(sourceFile, FileMode.Truncate, FileAccess.Write))
                        {
                            xssfwb.Write(file);
                        }
                        //Process.Start(sourceFile);
                    }
                }                
            }));
            set => _calculateCommand = value;
        }

        private double Dms2Dd(double xDeg, double xMin, double xSec, int Sign)
        {
            var x = System.Math.Round(Sign * (System.Math.Abs(xDeg) + System.Math.Abs(xMin / 60.0) + System.Math.Abs(xSec / 3600.0)), 10);
            return System.Math.Abs(x);
        }

        private void Dd2Dms(double val, out double xDeg, out double xMin, out double xSec)
        {
            var x = System.Math.Abs(System.Math.Round(System.Math.Abs(val), 10));

            xDeg = Fix(x);
            var dx = (x - xDeg) * 60;
            dx = System.Math.Round(dx, 8);
            xMin = Fix(dx);
            xSec = (dx - xMin) * 60;
            xSec = System.Math.Round(xSec, 6);
        }

        private int Fix(double x)
        {
            return (int)(System.Math.Sign(x) * System.Math.Floor(System.Math.Abs(x)));
        }

        public void WriteScreenShot(string file)
        {
            _dataList[_dataList.Count - 1].Image = GetScreenShot();
            _dataList[_dataList.Count - 1].CalcStr = CalculationText;
            _dataList[_dataList.Count - 1].Crc = CrcValue;

            if (_dataList.Count == _rowCount)
            {
                ProgressStr = "Done";
                Save(file);
            }

        }

        private byte[] GetScreenShot()
        {
            var rect = new Rect();
            //var handler = new WindowInteropHelper(Application.Current.MainWindow);

            var foregroundWindowsHandle = GetForegroundWindow();
            GetWindowRect(foregroundWindowsHandle, ref rect);
            var k = 7;
            var bounds = new Rectangle(rect.Left+k, rect.Top+k-3, rect.Right - rect.Left-2*k, rect.Bottom - rect.Top-2*k);
            var bitmap = new Bitmap(bounds.Width, bounds.Height);
            using (var g = Graphics.FromImage(bitmap))
            {
                g.CopyFromScreen(new Point(bounds.Left, bounds.Top), Point.Empty, bounds.Size);
            }            
            using (MemoryStream ms = new MemoryStream())
            {
                //Directory.CreateDirectory(Path.Combine(Path.GetTempPath(), "CRC"));
                //string fileName = Path.Combine(Path.GetTempPath(), "CRC", CalculationText+".jpeg");
                //bitmap.Save(fileName, ImageFormat.Jpeg);
                //return fileName;
                bitmap.Save(ms, ImageFormat.Jpeg);
                return ms.ToArray();
            }

        }

        public RelayCommand SourceBrowseCommand
        {
            get
            {
                return _sourceBrowseCommand ?? (_sourceBrowseCommand = new RelayCommand(() =>
          {
              var dlg = new OpenFileDialog
              {
                  Title = "Open Excel file",
                  Multiselect = true,
                  Filter = "Excel files (*.xls,*.xlsx)|*.xlsx;*.xls|All Files|*.*"
              };

              if (dlg.ShowDialog() == true)
              {
                  SourceFiles = dlg.FileNames;
              }
          }));
            }
            set => _sourceBrowseCommand = value;
        }

        public string SourceFile4View
        {
            get => _sourceFile4View; 
            set => Set(() => SourceFile4View, ref _sourceFile4View, value);
        }


        public string CalculationText
        {
            get
            {
                if (!IsDms1By1)
                    return $"{Latitude}{Longitude}{Elev}M";
                else
                {
                    Dd2Dms(Latitude, out double deg, out double min, out double sec);
                    var res = deg.ToString(CultureInfo.InvariantCulture) + min + sec;
                    Dd2Dms(Longitude, out deg, out min, out sec);
                    res+= deg.ToString(CultureInfo.InvariantCulture) + min + sec;
                    res += Elev + "M";
                    return res;
                }
            }
        }

        public string CrcValue => CRC32.CalcCRC32(CalculationText);

        public void Save(string fileName)
        {
            using (FileStream file = new FileStream(fileName, FileMode.Open, FileAccess.Write))
            {
                var xssfwbResult = new XSSFWorkbook();                
                var sheet = xssfwbResult.CreateSheet(_sheetName);
                var row = sheet.CreateRow(0);
                row.CreateCell(0).SetCellValue(_col0Name);
                row.CreateCell(1).SetCellValue(_col1Name);
                row.CreateCell(2).SetCellValue(_col2Name);
                row.CreateCell(3).SetCellValue(_col3Name);
                row.CreateCell(4).SetCellValue("CRC");
                row.CreateCell(5).SetCellValue("Calculation value");

                for (int index = 0; index < _dataList.Count; index++)
                {
                    int pictureIndex = xssfwbResult.AddPicture(_dataList[index].Image, PictureType.JPEG);
                    var helper = xssfwbResult.GetCreationHelper();
                    IDrawing drawing = sheet.CreateDrawingPatriarch();
                    IClientAnchor anchor = helper.CreateClientAnchor();
                    anchor.Col1 = 0; //0 index based column
                    anchor.Col2 = 3;
                    anchor.Row1 = 20 * index + 2; //0 index based row
                    anchor.Row2 = 20 * index + 20;
                    drawing.CreatePicture(anchor, pictureIndex);
                    row = sheet.CreateRow(20 * index + 1);

                    var cell0 = row.CreateCell(0);
                    cell0.SetCellValue(_dataList[index].Name);

                    var cell1 = row.CreateCell(1);
                    cell1.SetCellValue(_dataList[index].Latitude);


                    var cell2 = row.CreateCell(2);
                    cell2.SetCellValue(_dataList[index].Longitude);

                    var cell5 = row.CreateCell(3);
                    cell5.SetCellValue(_dataList[index].Elev);


                    var cell6 = row.CreateCell(4);
                    cell6.SetCellValue(_dataList[index].Crc);

                    var cell7 = row.CreateCell(5);
                    cell7.SetCellValue(_dataList[index].CalcStr);
                }
                sheet.AutoSizeColumn(0);
                sheet.AutoSizeColumn(1);
                sheet.AutoSizeColumn(2);
                sheet.AutoSizeColumn(3);
                sheet.AutoSizeColumn(4);
                sheet.AutoSizeColumn(5);
                //sheet.AutoSizeColumn(6);
                //sheet.AutoSizeColumn(7);
                xssfwbResult.Write(file);
            }
            //Process.Start(SourceFiles);
        }
    }
}