using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Aran.PANDA.Common;
using BKSystem.IO;

namespace FAS.Model
{
    public class InputData : BaseModel
    {
        private int _operationType;
        private int _serviceProviderIdentifier;
        private string _airportIdentifier;
        private string _runway;
        private int _runwayType;
        private int _approachPerformanceDesignator;
        private char _routeIndicator;
        private int _referencePathDataSelector;
        private string _referencePathIdentifier;
        private System.Windows.Point _ltpFtpCoordinate;
        private double _ltpFtpEllipsoidalHeight;
        private System.Windows.Point _fpapCoordinate;
        private double _thresholdCrossingHeight;
        private int _thresholdCrossingHeightUom;
        private double _glidepathAngle;
        private double _courseWidth;
        private double _lengthOffset;
        private double _hal;
        private double _val;
        private string _crc;
        private string _dataBlock;
        private string _crcText;
        private string _icaoCode;
        private double _ltpFtpOrthometricHeight;
        private double _fpapOrthometricHeight;


        public InputData()
        {
            LtpFtpEllipsoidalHeight = 0D;
            LtpFtpOrthometricHeight = 0D;
        }


        public int OperationType
        {
            get
            {
                return _operationType;
            }
            set
            {
                if (_operationType == value)
                    return;

                _operationType = value;
                NotifyPropertyChanged("OperationType");
            }
        }
        public int ServiceProviderIdentifier
        {
            get
            {
                return _serviceProviderIdentifier;
            }
            set
            {
                if (_serviceProviderIdentifier == value)
                    return;

                _serviceProviderIdentifier = value;
                NotifyPropertyChanged("ServiceProviderIdentifier");
            }
        }
        public string AirportIdentifier
        {
            get
            {
                return _airportIdentifier;
            }
            set
            {
                if (_airportIdentifier == value)
                    return;

                _airportIdentifier = value;
                NotifyPropertyChanged("AirportIdentifier");
            }
        }
        public string Runway
        {
            get
            {
                return _runway;
            }
            set
            {
                if (_runway == value)
                    return;
                
                _runway = value;
                NotifyPropertyChanged("Runway");
            }
        }
        public int RunwayType
        {
            get
            {
                return _runwayType;
            }
            set
            {
                if (_runwayType == value)
                    return;

                _runwayType = value;
                NotifyPropertyChanged("RunwayType");
            }
        }
        public int ApproachPerformanceDesignator
        {
            get
            {
                return _approachPerformanceDesignator;
            }
            set
            {
                if (_approachPerformanceDesignator == value)
                    return;

                _approachPerformanceDesignator = value;
                NotifyPropertyChanged("ApproachPerformanceDesignator");
            }
        }
        public char RouteIndicator
        {
            get
            {
                return _routeIndicator;
            }
            set
            {
                if (_routeIndicator == value)
                    return;

                _routeIndicator = value;
                NotifyPropertyChanged("RouteIndicator");
            }
        }
        public int ReferencePathDataSelector
        {
            get
            {
                return _referencePathDataSelector;
            }
            set
            {
                if (_referencePathDataSelector == value)
                    return;

                _referencePathDataSelector = value;
                NotifyPropertyChanged("ReferencePathDataSelector");
            }
        }
        public string ReferencePathIdentifier
        {
            get
            {
                return _referencePathIdentifier;
            }
            set
            {
                if (_referencePathIdentifier == value)
                    return;

                _referencePathIdentifier = value;
                NotifyPropertyChanged("ReferencePathIdentifier");
            }
        }
        public System.Windows.Point LtpFtpCoordinate
        {
            get
            {
                return _ltpFtpCoordinate;
            }
            set
            {
                if (_ltpFtpCoordinate == value)
                    return;

                _ltpFtpCoordinate = value;
                NotifyPropertyChanged("LtpFtpCoordinate");
            }
        }
        public double LtpFtpEllipsoidalHeight
        {
            get
            {
                return _ltpFtpEllipsoidalHeight;
            }
            set
            {
                if (_ltpFtpEllipsoidalHeight == value)
                    return;

                _ltpFtpEllipsoidalHeight = value;
                NotifyPropertyChanged("LtpFtpEllipsoidalHeight");
            }
        }
        public System.Windows.Point FpapCoordinate
        {
            get
            {
                return _fpapCoordinate;
            }
            set
            {
                if (_fpapCoordinate == value)
                    return;

                _fpapCoordinate = value;
                NotifyPropertyChanged("FpapCoordinate");
            }
        }
        //*** Metres.
        public double ThresholdCrossingHeight
        {
            get
            {
                return _thresholdCrossingHeight;
            }
            set
            {
                if (_thresholdCrossingHeight == value)
                    return;

                _thresholdCrossingHeight = value;
                NotifyPropertyChanged("ThresholdCrossingHeight");
            }
        }
        public int ThresholdCrossingHeightUom
        {
            get
            {
                return _thresholdCrossingHeightUom;
            }
            set
            {
                if (_thresholdCrossingHeightUom == value)
                    return;

                _thresholdCrossingHeightUom = value;
                NotifyPropertyChanged("ThresholdCrossingHeightUom");
            }
        }
        //*** Degrees.
        public double GlidepathAngle
        {
            get
            {
                return _glidepathAngle;
            }
            set
            {
                if (_glidepathAngle == value)
                    return;

                _glidepathAngle = value;
                NotifyPropertyChanged("GlidepathAngle");
            }
        }
        //*** Metres.
        public double CourseWidth
        {
            get
            {
                return _courseWidth;
            }
            set
            {
                if (_courseWidth == value)
                    return;

                _courseWidth = value;
                NotifyPropertyChanged("CourseWidth");
            }
        }
        //*** Metres.
        public double LengthOffset
        {
            get
            {
                return _lengthOffset;
            }
            set
            {
                if (_lengthOffset == value)
                    return;

                _lengthOffset = value;
                NotifyPropertyChanged("LengthOffset");
            }
        }
        //*** Metres.
        public double HAL
        {
            get
            {
                return _hal;
            }
            set
            {
                if (_hal == value)
                    return;

                _hal = value;
                NotifyPropertyChanged("HAL");
            }
        }
        public double VAL
        {
            get
            {
                return _val;
            }
            set
            {
                if (_val == value)
                    return;

                _val = value;
                NotifyPropertyChanged("VAL");
            }
        }
        public string CRC
        {
            get
            {
                return _crc;
            }
            set
            {
                if (_crc == value)
                    return;

                _crc = value;
                NotifyPropertyChanged("CRC");
            }
        }
        public string IcaoCode
        {
            get
            {
                return _icaoCode;
            }
            set
            {
                if (_icaoCode == value)
                    return;

                _icaoCode = value;
                NotifyPropertyChanged("IcaoCode");
            }
        }
        public double LtpFtpOrthometricHeight
        {
            get
            {
                return _ltpFtpOrthometricHeight;
            }
            set
            {
                if (_ltpFtpOrthometricHeight == value)
                    return;

                _ltpFtpOrthometricHeight = value;
                NotifyPropertyChanged("LtpFtpOrthometricHeight");
            }
        }
        public double FpapOrthometricHeight
        {
            get
            {
                return _fpapOrthometricHeight;
            }
            set
            {
                if (_fpapOrthometricHeight == value)
                    return;

                _fpapOrthometricHeight = value;
                NotifyPropertyChanged("FpapOrthometricHeight");
            }
        }
        



        public byte[] GetBuffer()
        {
            var bs = new BitStream();

            var bsCrc = new BitStream();
            bsCrc.Write(Convert.ToByte(OperationType));
            bsCrc.Write(Convert.ToByte(ServiceProviderIdentifier));
            foreach (var c in AirportIdentifier)
                bsCrc.Write(Convert.ToByte(c));

            bs.Write(ServiceProviderIdentifier, 0, 4);
            bs.Write(OperationType, 0, 4);

            #region AirportIdentifier

            for (int i = AirportIdentifier.Length - 1; i >= 0; i--)
            {
                var c = AirportIdentifier[i];
                bs.Write((byte)(((byte)c) & 0x3F), 0, 8);
            }

            #endregion

            byte runwayNumber = byte.Parse(Runway.Substring(2));

            bs.Write(RunwayType, 0, 2);
            bs.Write(runwayNumber, 0, 6);
            bs.Write(RouteIndicator, 0, 5);
            bs.Write(ApproachPerformanceDesignator, 0, 3);
            bs.Write(ReferencePathDataSelector, 0, 8);

            bsCrc.Write(runwayNumber, 0, 6);
            bsCrc.Write(RunwayType, 0, 2);
            bsCrc.Write(ApproachPerformanceDesignator, 0, 3);
            bsCrc.Write(RouteIndicator, 0, 5);
            bsCrc.Write(ReferencePathDataSelector);
            foreach (var c in ReferencePathIdentifier)
                bsCrc.Write(Convert.ToByte(c));

            #region ReferencePathIdentifier

            for (int i = ReferencePathIdentifier.Length - 1; i >= 0; i--)
            {
                var c = ReferencePathIdentifier[i];
                bs.Write((byte)(((byte)c) & 0x3F), 0, 8);
            }

            #endregion

            double xDeg, xMin, xSec;
            int x;
            int ltpXVal, ltpYVal;

            #region LTP/FTP

            DmsControll.Functions.DD2DMS(LtpFtpCoordinate.Y, out xDeg, out xMin, out xSec, Math.Sign(LtpFtpCoordinate.Y));
            x = Convert.ToInt32((xDeg * 3600 + xMin * 60 + xSec) / 0.0005);
            ltpYVal = x;
            var data = BitConverter.GetBytes(x);
            foreach (var b in data)
                bs.Write(b, 0, 8);

            bsCrc.Write(x);

            DmsControll.Functions.DD2DMS(LtpFtpCoordinate.X, out xDeg, out xMin, out xSec, Math.Sign(LtpFtpCoordinate.X));
            x = Convert.ToInt32((xDeg * 3600 + xMin * 60 + xSec) / 0.0005);
            ltpXVal = x;
            data = BitConverter.GetBytes(x);
            foreach (var b in data)
                bs.Write(b, 0, 8);

            bsCrc.Write(x);

            #endregion

            #region LtpFtpEllipsoidalHeight
            var xUInt16 = Convert.ToUInt16((LtpFtpEllipsoidalHeight + 512.0) / 0.1);

            bsCrc.Write(xUInt16);

            data = BitConverter.GetBytes(xUInt16);
            bs.Write(data[0]);
            bs.Write(data[1]);
            #endregion

            #region FPAP

            DmsControll.Functions.DD2DMS(FpapCoordinate.Y, out xDeg, out xMin, out xSec, Math.Sign(FpapCoordinate.Y));
            x = Convert.ToInt32((xDeg * 3600 + xMin * 60 + xSec) / 0.0005);
            x = x - ltpYVal;
            data = BitConverter.GetBytes(x);
            bs.Write(data[0]);
            bs.Write(data[1]);
            bs.Write(data[2]);

            bsCrc.Write(x, 0, 24);

            DmsControll.Functions.DD2DMS(FpapCoordinate.X, out xDeg, out xMin, out xSec, Math.Sign(FpapCoordinate.X));
            x = Convert.ToInt32((xDeg * 3600 + xMin * 60 + xSec) / 0.0005);
            x = x - ltpXVal;
            data = BitConverter.GetBytes(x);
            bs.Write(data[0]);
            bs.Write(data[1]);
            bs.Write(data[2]);

            bsCrc.Write(x, 0, 24);

            #endregion

            #region ThresholdCrossingHeight (Units)

            x = Convert.ToInt32(ThresholdCrossingHeight / (ThresholdCrossingHeightUom == 0 ? 0.1 : 0.05));
            data = BitConverter.GetBytes(x);
            bs.Write(data[0]);
            bs.Write(ThresholdCrossingHeightUom, 0, 1);
            bs.Write(data[1], 0, 7);

            bsCrc.Write(x, 0, 15);
            bsCrc.Write(ThresholdCrossingHeightUom, 0, 1);

            #endregion


            xUInt16 = Convert.ToUInt16(GlidepathAngle / 0.01);
            data = BitConverter.GetBytes(xUInt16);
            bs.Write(data[0]);
            bs.Write(data[1]);

            bsCrc.Write(xUInt16);


            var xByte = Convert.ToByte((CourseWidth - 80) / 0.25);
            bs.Write(xByte);

            bsCrc.Write(xByte);


            xByte = Convert.ToByte(LengthOffset / 8);
            bs.Write(xByte);

            bsCrc.Write(xByte);


            xByte = Convert.ToByte(HAL / 0.2);
            bs.Write(xByte);

            bsCrc.Write(xByte);

            xByte = Convert.ToByte(VAL / 0.2);
            bs.Write(xByte);

            bsCrc.Write(xByte);

            data = bs.ToByteArray();

            var crcData = new byte[data.Length];
            for (int i = 0; i < data.Length; i++)
                crcData[i] = ReverseByte(data[i]);

            var crcUInt32 = Aran.PANDA.Common.CRC32.CalcCRC32_2(crcData);

            var crcValBytes = BitConverter.GetBytes(crcUInt32);
            crcValBytes = crcValBytes.Reverse().ToArray();

            _crcText = string.Empty;

            for (int i = 0; i < crcValBytes.Length; i++)
            {
                crcValBytes[i] = ReverseByte(crcValBytes[i]);
                _crcText += crcValBytes[i].ToString("X2") + " ";

                bs.Write(crcValBytes[i]);
            }

            data = bs.ToByteArray();

            _dataBlock = string.Empty;
            foreach (var b in data)
                _dataBlock += b.ToString("X2") + " ";

            return data;
        }

        private string CalcAndPrintCrc(byte[] data, uint polynom, bool reverse, bool reverseByte, uint initValue)
        {
            var result = string.Format("polynom:\t\t{0:X}\nreverse:\t\t{1}\nreverseByte:\t{2}\ninitValue:\t\t{3:X}\n\n", 
                polynom, reverse, reverseByte, initValue);

            if (polynom > 0)
                Aran.PANDA.Common.CRC32.Polynom = polynom;

            if (reverse)
                data = data.Reverse().ToArray();

            if (reverseByte)
            {
                for (int i = 0; i < data.Length; i++)
                    data[i] = ReverseByte(data[i]);
            }

            var xx = Aran.PANDA.Common.CRC32.CalcCRC32_2(data, initValue);
            var crcData = BitConverter.GetBytes(xx);

            var ss = string.Empty;
            foreach (var b in crcData)
                ss += b.ToString("X2") + " ";

            result += "CRC:\n\t\t" + ss;
            result += "\nEUC:\tA9 EB 53 65";

            ss = string.Empty;
            foreach (var b in crcData)
                ss += Convert.ToString(b, 2).PadLeft(8, '0') + " ";

            result += "\n\t\t" + ss;
            result += "\nEUC:\t10101001 11101011 01010011 01100101";
            return result;
        }

        byte ReverseByte(byte b)
        {
            b = (byte)((b & 0xF0) >> 4 | (b & 0x0F) << 4);
            b = (byte)((b & 0xCC) >> 2 | (b & 0x33) << 2);
            b = (byte)((b & 0xAA) >> 1 | (b & 0x55) << 1);
            return b;
        }

        protected override string Validate(string propertyName)
        {
            var mi = typeof(InputValidator).GetMethod(propertyName, BindingFlags.Public | BindingFlags.Static);

            if (mi != null)
            {
                var pi = GetType().GetProperty(propertyName);
                var val = pi.GetValue(this);
                if (val != null)
                    return (string)mi.Invoke(null, new object[] { val });
            }

            return base.Validate(propertyName);
        }

        public List<List<string[]>> GetReportData(out string title, out string dataBlock, out string crcText)
        {
            title = "SBAS FAS Data Block";
            dataBlock = _dataBlock;
            crcText = _crcText;

            var sa = new object[] {
                "Operation Type" , OperationType,
                "Service Provider Identifier", ServiceProviderIdentifier,
                "Airport Identifier", AirportIdentifier,
                "Runway", Runway + "  (Direction: " + RunwayType + ")",
                "Approach Performance Designator", ApproachPerformanceDesignator,
                "Route Indicator", RouteIndicator,
                "Reference Path Data Selector", ReferencePathDataSelector,
                "Reference Path Identifier", ReferencePathIdentifier,
                "LTP/FTP Latitude", ToDmsText(LtpFtpCoordinate.Y, true),
                "LTP/FTP Longitude", ToDmsText(LtpFtpCoordinate.X, false),
                "LTP/FTP Ellipsoidal Height", LtpFtpEllipsoidalHeight,
                "FPAP Latitude", ToDmsText(FpapCoordinate.Y, true),
                "FPAP Longitude", ToDmsText(FpapCoordinate.X, false),
                "Threshold Crossing Height (TCH)", ThresholdCrossingHeight + "  (Unit: " + ThresholdCrossingHeightUom + ")",
                "Glide Path Angle (GPA)", GlidepathAngle.ToString("0.00"),
                "Course width at THR", CourseWidth.ToString("0.00"),
                "Length offset", LengthOffset.ToString("0.00"),
                "Horizontal Alert Limit (HAL)", HAL.ToString("0.00"),
                "Vertical Alert Limit (VAL)", VAL.ToString("0.00")
            };

            var line = new List<string[]>();

            for (int i = 0; i < sa.Length - 1; i += 2)
                line.Add(new string[] { sa[i].ToString(), (sa[i + 1] ?? "").ToString() });

            var lineList = new List<List<string[]>>();
            lineList.Add(line);


            sa = new object[] {
                "ICAO Code",IcaoCode,
                "LTP FTP Orthometric Height", LtpFtpOrthometricHeight,
                "FPAP Orthometric Height", FpapOrthometricHeight
            };

            line = new List<string[]>();

            for (int i = 0; i < sa.Length - 1; i += 2)
                line.Add(new string[] { sa[i].ToString(), (sa[i + 1] ?? "").ToString() });

            lineList.Add(line);

            return lineList;
        }

        private string ToDmsText(double val, bool isLatitude)
        {
            double xDeg, xMin, xSec;
            DmsControll.Functions.DD2DMS(val, out xDeg, out xMin, out xSec, Math.Sign(val));
            if (isLatitude)
                return string.Format("{0:00}{1:00}{2:#.0000}{3}", xDeg, xMin, xSec, (Math.Sign(val) > 0 ? "N" : "S"));
            else
                return string.Format("{0:000}{1:00}{2:#.0000}{3}", xDeg, xMin, xSec, (Math.Sign(val) > 0 ? "E" : "N"));
        }
    }

    public class InputValidator
    {
        public static string Runway(object value)
        {
            var text = value.ToString();
            
            if (text == null)
                return string.Empty;

            if (!text.StartsWith("RW"))
                return "Invalid value format.";

            int cource;
            if (!int.TryParse(text.Substring(2), out cource))
                return "Invalid value format.";

            if (cource > 36)
                return "Runway cource is creater than 36.";

            return null;
        }

        public static string ReferencePathIdentifier(object value)
        {
            var text = value.ToString();
            if (text != null)
            {
                var isOk = false;

                if (text.Length > 0)
                {
                    if (Regex.Match(text[0].ToString(), "[WEM]").Success)
                        isOk = true;

                    if (text.Length > 2)
                    {
                        if (Regex.Match(text.Substring(1, 2), "[0-9]{2}").Success)
                            isOk = true;
                    }

                    if (text.Length > 3)
                    {
                        if (Regex.Match(text[3].ToString(), "[A-Z]").Success)
                            isOk = true;
                    }
                }

                if (text.Length != 4)
                    isOk = false;

                if (!isOk)
                    return "Invalid Format";
            }
            return null;
        }
    }
}
