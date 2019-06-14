using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Globalization;

namespace ChoosePointNS
{
	public enum EarthDirection
	{
		N,
		S,
		E,
		W
	}

	public partial class DD_DMS : UserControl
	{
		private const int minWidth = 210;
		private const int minHeight = 26;
		private bool _isX;
		private bool _isDD;
		private double m_val;
        private double _minutOldValue;
        private EarthDirection _lastEarthDir = EarthDirection.N;

		private int _ddAccuracy;
		private int _dmsAccuracy;

		public event EventHandler ValueChanged;
		public event KeyPressEventHandler KeyDown;

		public DD_DMS()
		{
			InitializeComponent();

            _isDD = false;
			_isX = false;

			IsDD = true;
			IsX = true;

			ntDecimals.Left = ntDegrees.Left;
			ntDecimals.Top = ntDegrees.Top;

			//ntDecimals.Width = (ntSeconds.Left + ntSeconds.Width) - ntDegrees.Left;
			Width = cbNSEW.Left + cbNSEW.Width - ntDecimals.Left + 5;
			Height = ntDecimals.Height;

			lbDecDegrees.Top = lbDegrees.Top;
			ntDegrees.Accuracy = 0;
			ntMinutes.Accuracy = 0;

			//cbNSEW.SelectedIndex = 0;

            base.KeyDown += DD_DMS_KeyDown;

            
        }

        //=====
        public double Value
		{
			get
			{
				if (_isDD)
				{
					int Sgn = (cbNSEW.SelectedIndex << 1) - 1;
					m_val = Sgn * ntDecimals.Value;
				}
				else
				{
					m_val = GetNtValue();
				}
				return m_val;
			}

			set
			{
                if (!double.IsNaN (value)  && m_val != value)
				{
					int Sgn = Math.Sign(value);
					cbNSEW.SelectedIndex = (Sgn + 1) >> 1;
					m_val = value;

                    if (IsDD)
					    ntDecimals.Value = Math.Abs(m_val);
                    else
					    SetNtValue ( m_val );

					if (ValueChanged != null)
						ValueChanged(this, new EventArgs());
				}
			}
		}

		public bool IsDD
		{
			get { return _isDD; }
			set
			{
				if (_isDD != value)
				{
					_isDD = value;
					SetNtVisible(!_isDD);

					ntDecimals.Visible = _isDD;
					lbDecDegrees.Visible = _isDD;

					if (_isDD)
					{
						int sgn = Math.Sign(m_val);
						ntDecimals.Value = Math.Abs(m_val);
						cbNSEW.SelectedIndex = (sgn + 1) >> 1;
					}
					else
						SetNtValue(m_val);
				}
			}
		}

		public bool IsX
		{
			get { return _isX; }

			set
			{
				if (_isX != value)
				{
					_isX = value;
					int i = cbNSEW.SelectedIndex;

					cbNSEW.Items.Clear();
					if (_isX)
					{
						cbNSEW.Items.Add(EarthDirection.W);
						cbNSEW.Items.Add(EarthDirection.E);
						ntDegrees.Maximum = 180;
						ntDecimals.Maximum = 180;
					}
					else
					{
						cbNSEW.Items.Add(EarthDirection.S);
						cbNSEW.Items.Add(EarthDirection.N);
						ntDegrees.Maximum = 90;
						ntDecimals.Maximum = 90;
					}

					ntDegrees.SetForDMS(true, _isX);
					ntMinutes.SetForDMS(true, false);
					ntSeconds.SetForDMS(true, false);

					ntDecimals.SetForDMS(true, _isX);

                    var str = CommonUtils.Config.ReadConfig<string>("RISK/ARAN", "DdDmsEarthDir_" + (_isX ? "X" : "Y" ), string.Empty);
                    if (Enum.TryParse<EarthDirection>(str, out EarthDirection ed))
                        _lastEarthDir = ed;

                    cbNSEW.SelectedIndex = (_isX ? 
                        (_lastEarthDir == EarthDirection.W ? 0 : 1) : 
                        (_lastEarthDir == EarthDirection.S ? 0 : 1));
                    //cbNSEW.SelectedIndex = i;
				}
			}
		}

		public EarthDirection Direction
		{
			get
			{
				return (EarthDirection)cbNSEW.SelectedItem;
			}
		}

		public int DDAccuracy
		{
			get { return _ddAccuracy; }
			set
			{
				if (_ddAccuracy != value)
				{
					_ddAccuracy = value;
					ntDecimals.Accuracy = _ddAccuracy;
				}
			}
		}

		public int DMSAccuracy
		{
			get { return _dmsAccuracy; }
			set
			{
				if (_dmsAccuracy != value)
				{
					_dmsAccuracy = value;
					ntSeconds.Accuracy = _dmsAccuracy;
				}
			}
		}

        public bool ReadOnly
        {
            get { return ntDecimals.ReadOnly; }
            set
            {
                ntDecimals.ReadOnly = value;
                ntDegrees.ReadOnly = value;
                ntSeconds.ReadOnly = value;
                ntMinutes.ReadOnly = value;
            }
        }

		//=====
		public override void Refresh()
		{
			base.Refresh();
			ntDecimals.Refresh();
			ntDegrees.Refresh();
			ntMinutes.Refresh();
			ntSeconds.Refresh();
		}

	    public void Reset()
	    {
            double tmpValue = 0;
            double.TryParse(ntDecimals.MainTextBox.Text, out tmpValue);
            if (Math.Abs(tmpValue - ntDecimals.Value) > 0.00001)
                ntDecimals.Reset();

            double.TryParse(ntDegrees.MainTextBox.Text, out tmpValue);
            if (Math.Abs(tmpValue - ntDegrees.Value) > 0.00001)
                ntDegrees.Reset();

            double.TryParse(ntMinutes.MainTextBox.Text, out tmpValue);
            if (Math.Abs(tmpValue - ntMinutes.Value) > 0.00001)
                ntMinutes.Reset();

            double.TryParse(ntSeconds.MainTextBox.Text, out tmpValue);
            if (Math.Abs(tmpValue - ntSeconds.Value) > 0.00001)
                ntSeconds.Reset();
	    }

	    //==
		private int Fix(double x)
		{
			return (int)(System.Math.Sign(x) * System.Math.Floor(System.Math.Abs(x)));
		}

		private void DD2DMS(double val, out double xDeg, out double xMin, out double xSec, int Sign)
		{
			double x;
			double dx;

			x = System.Math.Abs(System.Math.Round(System.Math.Abs(val) * Sign, 10));

			xDeg = Fix(x);
			dx = (x - xDeg) * 60;
			dx = System.Math.Round(dx, 8);
			xMin = Fix(dx);
			xSec = (dx - xMin) * 60;
			xSec = System.Math.Round(xSec, 6);
		}

		private double DMS2DD(double xDeg, double xMin, double xSec, int Sign)
		{
			double x;
			x = System.Math.Round(Sign * (System.Math.Abs(xDeg) + System.Math.Abs(xMin / 60.0) + System.Math.Abs(xSec / 3600.0)), 10);
			return System.Math.Abs(x);
		}

		private double GetNtValue()
		{
			int Sgn = (cbNSEW.SelectedIndex << 1) - 1;
			double retVal = DMS2DD(ntDegrees.Value, ntMinutes.Value, ntSeconds.Value, Sgn);
			return retVal * Sgn;
		}

		private void SetNtValue(double NewValue)
		{
			double m_Deg, m_Min, m_Sec;

			int Sgn = Math.Sign(NewValue);

			DD2DMS(NewValue, out m_Deg, out m_Min, out m_Sec, Sgn);

			if (Math.Round(m_Sec, ntSeconds.Accuracy) == 60)
			{
				m_Sec = 0;
				m_Min++;
				if (Math.Round(m_Min) == 60)
				{
					m_Min = 0;
					m_Deg++;
				}
			}

			ntDegrees.Value = m_Deg;
			ntMinutes.Value = m_Min;
			ntSeconds.Value = m_Sec;

            if (NewValue == 0)
            {
                cbNSEW.SelectedIndex = (_isX ?
                        (_lastEarthDir == EarthDirection.W ? 0 : 1) :
                        (_lastEarthDir == EarthDirection.S ? 0 : 1));
            }
            else
            {
                cbNSEW.SelectedIndex = (Sgn + 1) >> 1;
            }
		}

		private void SetNtVisible(bool IsVisible)
		{
			ntDegrees.Visible = IsVisible;
			ntMinutes.Visible = IsVisible;
			ntSeconds.Visible = IsVisible;
			lbDegrees.Visible = IsVisible;
			lbMinutes.Visible = IsVisible;
			lbSeconds.Visible = IsVisible;

			if (IsVisible)
				cbNSEW.Left = lbSeconds.Width + lbSeconds.Left + 10;
			else
				cbNSEW.Left = lbDecDegrees.Width + lbDecDegrees.Left + 10;
		}

		private void cbNSEW_SelectedIndexChanged(object sender, EventArgs e)
		{
            ValueChanged?.Invoke(this, new EventArgs());
        }

		private void ntDegrees_ValueChanged(object sender, EventArgs e)
		{
            //var senderNum = sender as NumericTextBox;
            //if (senderNum == ntMinutes)
            //{
            //    if (senderNum.Value == _minutOldValue)
            //        return;
            //    if (senderNum.Value > 59)
            //    {
            //        senderNum.Value = _minutOldValue;
            //        return;
            //    }
            //    _minutOldValue = senderNum.Value;
            //}

			double NewVal = GetNtValue();
			if (m_val != NewVal)
			{
				m_val = NewVal;
				ntDecimals.Value = Math.Abs(m_val);

				if (ValueChanged != null)
					ValueChanged(this, new EventArgs());
			}
		}

		private void ntDecimals_ValueChanged(object sender, EventArgs e)
		{
			int Sgn = (cbNSEW.SelectedIndex << 1) - 1;
			double NewVal = Sgn * ntDecimals.Value;

			if (m_val != NewVal)
			{
				m_val = NewVal;

				if (!IsDD)
					SetNtValue(m_val);

				if (ValueChanged != null)
					ValueChanged(this, new EventArgs());
			}
		}

		private void DD_DMS_Resize(object sender, EventArgs e)
		{
			if (Width < minWidth) Width = minWidth;
			if (Height < minHeight) Height = minHeight;
		}

		private void ntDegrees_KeyPressChanged(object sender, KeyPressEventArgs e)
		{
            KeyDown?.Invoke(sender, e);
		}

        private void PasteCoordinate_Click (object sender, EventArgs e)
        {
            PasteCoordinate ();
        }

        private void PasteCoordinate ()
        {
            
            string copiedText = null;
            double d = double.NaN;

            try
            {
                copiedText = Clipboard.GetText ();
                if (IsX)
                    d = GetLONGITUDEFromAIXMString (copiedText);
                else
                    d = GetLATITUDEFromAIXMString (copiedText);

                Value = d;
            }
            catch { }
        }

        private double GetLONGITUDEFromAIXMString (string AIXM_COORD)
        {
            // Џ…ђ…‚Ћ„ „Ћ‹ѓЋ’› Ё§ AIXM ў д®а¬ в DD.MM
            // A string of "digits" (plus, optionally, a period) followed by one of the
            // "Simple Latin upper case letters" E or W, in the forms DDDMMSS.ssY, DDDMMSSY, DDDMM.mm...Y, DDDMMY, and DDD.dd...Y . 
            // The Y stands for either E (= East) or W (= West), DDD represents whole degrees, MM whole minutes, and SS whole seconds. 
            // The period indicates that there are decimal fractions present; whether these are fractions of seconds, minutes,
            // or degrees can easily be deduced from the position of the period. The number of digits representing the fractions
            // of seconds is 1 = s... <= 4; the relevant number for fractions of minutes and degrees is 1 <= d.../m... <= 8.
            string DD = "";
            string MM = "";
            string SS = "";
            string STORONA_SVETA = "";
            int SIGN = 1;
            double Coord = 0;
            double Gradusy = 0.0;
            double Minuty = 0.0;
            double Sekundy = 0.0;
            string Coordinata = "";

            try
            {
                NumberFormatInfo nfi = new NumberFormatInfo ();
                nfi.NumberGroupSeparator = " ";
                nfi.PositiveSign = "+";
                STORONA_SVETA = AIXM_COORD.Substring (AIXM_COORD.Length - 1, 1);

                if (STORONA_SVETA == "W") SIGN = -1;

                AIXM_COORD = AIXM_COORD.Substring (0, AIXM_COORD.Length - 1);
                int PntPos = AIXM_COORD.LastIndexOf (".");
                int CommPos = AIXM_COORD.LastIndexOf (",");
                int SepPos;

                if (PntPos == -1 && CommPos > -1)
                {
                    nfi.NumberDecimalSeparator = ",";
                    SepPos = CommPos;
                }
                else
                {
                    nfi.NumberDecimalSeparator = ".";
                    SepPos = PntPos;
                }

                if (SepPos > 0) //DDDMMSS.ss...X, DDDMM.mm...X, and DDD.dd...X
                {
                    Coordinata = AIXM_COORD.Substring (0, SepPos);
                    switch (Coordinata.Length)
                    {
                        case 3: //DDD.dd...
                            Coord = Convert.ToDouble (AIXM_COORD, nfi) * SIGN;
                            break;
                        case 5: //DDDMM.mm... 
                            DD = AIXM_COORD.Substring (0, 3);
                            MM = AIXM_COORD.Substring (3, AIXM_COORD.Length - 3);
                            Gradusy = Convert.ToDouble (DD, nfi);
                            Minuty = Convert.ToDouble (MM, nfi);
                            Coord = (Gradusy + (Minuty / 60)) * SIGN;
                            break;
                        case 7: //DDDMMSS.ss... 
                            DD = AIXM_COORD.Substring (0, 3);
                            MM = AIXM_COORD.Substring (3, 2);
                            SS = AIXM_COORD.Substring (5, AIXM_COORD.Length - 5);
                            Gradusy = Convert.ToDouble (DD, nfi);
                            Minuty = Convert.ToDouble (MM, nfi);
                            Sekundy = Convert.ToDouble (SS, nfi);
                            Coord = (Gradusy + (Minuty / 60) + (Sekundy / 3600)) * SIGN;
                            break;
                    }
                }
                else //DDDMMSSX and DDDMMX
                {
                    Coordinata = AIXM_COORD;
                    switch (Coordinata.Length)
                    {
                        case 5: //DDDMM
                            DD = AIXM_COORD.Substring (0, 3);
                            MM = AIXM_COORD.Substring (3, AIXM_COORD.Length - 3);
                            Gradusy = Convert.ToDouble (DD, nfi);
                            Minuty = Convert.ToDouble (MM, nfi);
                            Coord = (Gradusy + (Minuty / 60)) * SIGN;
                            break;
                        case 7: //DDDMMSS
                            DD = AIXM_COORD.Substring (0, 3);
                            MM = AIXM_COORD.Substring (3, 2);
                            SS = AIXM_COORD.Substring (5, AIXM_COORD.Length - 5);
                            Gradusy = Convert.ToDouble (DD, nfi);
                            Minuty = Convert.ToDouble (MM, nfi);
                            Sekundy = Convert.ToDouble (SS, nfi);
                            Coord = (Gradusy + (Minuty / 60) + (Sekundy / 3600)) * SIGN;
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                Coord = 0;
                throw ex;
            }
            return Coord;
        }

        private double GetLATITUDEFromAIXMString (string AIXM_COORD)
        {
            // Џ…ђ…‚Ћ„ €ђЋ’› Ё§ AIXM ў д®а¬ в DD.MM
            //A string of "digits" (plus, optionally, a period) followed by one of the "Simple Latin upper case letters" N or S,
            //in the forms DDMMSS.ss...X, DDMMSSX, DDMM.mm...X, DDMMX, and DD.dd...X. The X stands for either N (= North) or S (= South),
            //DD represents whole degrees, MM whole minutes, and SS whole seconds. The period indicates that there are decimal
            //fractions present; whether these are fractions of seconds, minutes, or degrees can easily be deduced from the position
            //of the period. The number of digits representing the fractions of seconds is 1<= s... <= 4; the relevant number for
            //fractions of minutes and degrees is 1 <= d.../m... <= 8.
            string DD = "";
            string MM = "";
            string SS = "";
            string STORONA_SVETA = "";

            int SIGN = 1;
            double Coord = 0;
            double Gradusy = 0.0;
            double Minuty = 0.0;
            double Sekundy = 0.0;
            string Coordinata = "";
            try
            {
                NumberFormatInfo nfi = new NumberFormatInfo ();

                nfi.NumberGroupSeparator = " ";
                nfi.PositiveSign = "+";
                STORONA_SVETA = AIXM_COORD.Substring (AIXM_COORD.Length - 1, 1);
                if (STORONA_SVETA == "S") SIGN = -1;
                AIXM_COORD = AIXM_COORD.Substring (0, AIXM_COORD.Length - 1);
                int PntPos = AIXM_COORD.LastIndexOf (".");
                int CommPos = AIXM_COORD.LastIndexOf (",");
                int SepPos;

                if (PntPos == -1 && CommPos > -1)
                {
                    nfi.NumberDecimalSeparator = ",";
                    SepPos = CommPos;
                }
                else
                {
                    nfi.NumberDecimalSeparator = ".";
                    SepPos = PntPos;
                }

                if (SepPos > 0) //DDMMSS.ss...X, DDMM.mm...X, and DD.dd...X
                {
                    Coordinata = AIXM_COORD.Substring (0, SepPos);
                    switch (Coordinata.Length)
                    {
                        case 2: //DD.dd...
                            Coord = Convert.ToDouble (AIXM_COORD, nfi) * SIGN;
                            break;
                        case 4: //DDMM.mm... 
                            DD = AIXM_COORD.Substring (0, 2);
                            MM = AIXM_COORD.Substring (2, AIXM_COORD.Length - 2);
                            Gradusy = Convert.ToDouble (DD, nfi);
                            Minuty = Convert.ToDouble (MM, nfi);
                            Coord = (Gradusy + (Minuty / 60)) * SIGN;
                            break;
                        case 6: //DDMMSS.ss... 
                            DD = AIXM_COORD.Substring (0, 2);
                            MM = AIXM_COORD.Substring (2, 2);
                            SS = AIXM_COORD.Substring (4, AIXM_COORD.Length - 4);
                            Gradusy = Convert.ToDouble (DD, nfi);
                            Minuty = Convert.ToDouble (MM, nfi);
                            Sekundy = Convert.ToDouble (SS, nfi);
                            Coord = (Gradusy + (Minuty / 60) + (Sekundy / 3600)) * SIGN;
                            break;
                    }
                }
                else //DDMMSSX and DDMMX
                {
                    Coordinata = AIXM_COORD;
                    switch (Coordinata.Length)
                    {
                        case 4: //DDMM
                            DD = AIXM_COORD.Substring (0, 2);
                            MM = AIXM_COORD.Substring (2, AIXM_COORD.Length - 2);
                            Gradusy = Convert.ToDouble (DD, nfi);
                            Minuty = Convert.ToDouble (MM, nfi);
                            Coord = (Gradusy + (Minuty / 60)) * SIGN;
                            break;
                        case 6: //DDMMSS
                            DD = AIXM_COORD.Substring (0, 2);
                            MM = AIXM_COORD.Substring (2, 2);
                            SS = AIXM_COORD.Substring (4, AIXM_COORD.Length - 4);
                            Gradusy = Convert.ToDouble (DD, nfi);
                            Minuty = Convert.ToDouble (MM, nfi);
                            Sekundy = Convert.ToDouble (SS, nfi);
                            Coord = (Gradusy + (Minuty / 60) + (Sekundy / 3600)) * SIGN;
                            break;
                    }
                }

            }
            catch (Exception ex)
            {
                Coord = 0;
                throw ex;
            }
            return Coord;
        }

        private void DD_DMS_KeyDown (object sender, KeyEventArgs e)
        {
            if ((e.Modifiers == Keys.Control && e.KeyCode == Keys.V) ||
                (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Insert))
            {
                PasteCoordinate ();
            }
        }

        private void cbNSEW_Leave(object sender, EventArgs e)
        {
            if (Enum.TryParse<EarthDirection>(cbNSEW.SelectedItem.ToString(), out EarthDirection tmp))
            {
                _lastEarthDir = tmp;
                CommonUtils.Config.WriteConfig("RISK/ARAN", "DdDmsEarthDir_" + (_isX ? "X" : "Y"), _lastEarthDir.ToString());
            }
        }
    }
}
