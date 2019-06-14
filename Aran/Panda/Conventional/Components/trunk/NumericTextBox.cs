using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ChoosePointNS
{
	/*

	public class MinOrMaxEventArgs: EventArgs
	{
	}

	public delegate void MinOrMaxEventHandler(object sender, MinOrMaxEventArgs e);

	 */

	public partial class NumericTextBox : UserControl
	{
        
		private bool m_valIsAngle;
		private bool m_IsClockwise;
		private bool m_forDMS;
		private bool m_forDMS_isDegX;

		private int m_Accuracy;

		private double m_minVal;
		private double m_maxVal;
		private double m_val;
		private double m_oldVal;

		public event EventHandler MinOrMaxChanged;
		public event EventHandler ValueChanged;
		public event KeyPressEventHandler KeyPressChanged;

		public NumericTextBox()
		{
			InitializeComponent();

			m_maxVal = 100;
			m_minVal = 0;

			m_Accuracy = 0;

			tbNumricText.Text = "0";
			m_valIsAngle = false;
			m_IsClockwise = true;
			m_forDMS = false;
		}

		public TextBox MainTextBox
		{
			get { return tbNumricText; }
		}

		public double Minimum
		{
			get { return m_minVal; }
			set
			{
				double oldVal = m_minVal;
				m_minVal = value;
				if (!m_valIsAngle && m_minVal > m_maxVal)
					m_maxVal = m_minVal;

				if (oldVal != m_minVal)
					MinOrMaxOrValueChanged();
			}
		}

		public double Maximum
		{
			get { return m_maxVal; }
			set
			{
				double oldVal = m_maxVal;
				m_maxVal = value;

				if (!m_valIsAngle && m_minVal > m_maxVal)
					m_minVal = m_maxVal;

				if (Math.Abs(oldVal - m_maxVal) > 0.0001)
					MinOrMaxOrValueChanged();
			}
		}

		public double Value
		{
			get { return m_val; }
            set
            {
                double newVal = value;
                
                if (m_valIsAngle)
                    newVal = Mod360(newVal);

                newVal = AdjustValInsideMinMax(newVal);

                //if (m_val == newVal)
                //    return;

                //if (m_val != newVal)
                //{
                //    m_oldVal = m_val;
                m_val = newVal;
                DisplayVal();
                if (ValueChanged != null)
                    ValueChanged(this, new EventArgs());
                //}
            }
		}

		public bool ValueIsAngle
		{
			get { return m_valIsAngle; }
			set
			{
				if (m_valIsAngle != value)
				{
					m_valIsAngle = value;
					Value = m_val;
				}
			}
		}

		public bool IsClockwise
		{
			get { return m_IsClockwise; }
			set
			{
				if (m_IsClockwise != value)
				{
					m_IsClockwise = value;
					Value = m_val;
				}
			}
		}

		public int Accuracy
		{
			get { return m_Accuracy; }
			set
			{
				if (m_Accuracy != value)
				{
					m_Accuracy = value;
					if (m_forDMS_isDegX && m_Accuracy < 0)
						m_Accuracy = 0;
					DisplayVal();
				}
			}
		}

		public bool ReadOnly
		{
            get { return tbNumricText.ReadOnly; }
            set { tbNumricText.ReadOnly = value; }
		}

		//=================================================
		public void SetForDMS(bool forDMS, bool IsDegX)
		{
			m_forDMS = forDMS;
			m_forDMS_isDegX = IsDegX;

			Value = m_val;

			if (m_forDMS && m_Accuracy < 0)
				Accuracy = 0;
		}

		public override void Refresh()
		{
			DisplayVal();
			base.Refresh();
		}

	    public void Reset()
	    {
	        DoValueChanged();
	    }

	    //=====

        private void DisplayVal()
        {
            try
            {
                string format = "0";

                if (m_forDMS)
                {
                    if (m_forDMS_isDegX)
                        format = "000";
                    else
                        format = "00";

                    if (m_Accuracy > 0)
                        format = format + '.' + new string('0', m_Accuracy);
                }
                else if (m_Accuracy > 0)
                    format = "0." + new string('0', m_Accuracy);
                else if (Accuracy < 0)
                    format = "0" + new string('#', m_Accuracy);

                tbNumricText.Text = m_val.ToString(format);
                m_oldVal = m_val;
            }
            catch (Exception)
            {
                //var aa = 4;
            }
        }

		private bool IsNumeric(string text)
		{
			double f;
			return double.TryParse(text, out f);
		}

		private double Mod360(double x)
		{
			x -= Math.Floor(x / 360.0) * 360.0;
			if (x < 0.0) x += 360.0;
			if (x == 360.0) x = 0.0;
			return x;
		}

		private double SubtractAnglesWithSign(double StRad, double EndRad, int Turn)
		{
			double res = Mod360((EndRad - StRad) * Turn);
			if (res > 180)
				res -= 360;
			return res;
		}

		private double AdjustInsideWithDirection(double fromAngle, double toAngle,
			double val, bool IsClockwise)
		{
			double ab, cb;
			int Direction = IsClockwise ? 1 : -1;

			ab = Mod360(SubtractAnglesWithSign(fromAngle, toAngle, Direction));
			cb = Mod360(SubtractAnglesWithSign(val, toAngle, Direction));

			if (cb > ab)
			{
				if (Mod360(SubtractAnglesWithSign(val, fromAngle, Direction)) <
					Mod360(SubtractAnglesWithSign(val, toAngle, -Direction)))
					return fromAngle;
				else
					return toAngle;
			}

			return val;
		}

		private double AdjustValInsideMinMax(double val)
		{
			if (m_valIsAngle)
			{
				if (m_minVal > 360.0 || m_minVal < 0.0 || m_maxVal > 360.0 || m_maxVal < 0.0)
					return val;

				return AdjustInsideWithDirection(m_minVal, m_maxVal, val, m_IsClockwise);
			}
			else
			{
				if (val > m_maxVal)
					return m_maxVal;
				else if (val < m_minVal)
					return m_minVal;
			}
			return val;
		}

		private void MinOrMaxOrValueChanged()
		{
			double newVal = m_val;
			if (m_valIsAngle)
				newVal = Mod360(newVal);

			Value = AdjustValInsideMinMax(newVal);

			if (MinOrMaxChanged != null)
				MinOrMaxChanged(this, new EventArgs());
			DisplayVal();
		}

		private void DoValueChanged()
		{
			if (!IsNumeric(tbNumricText.Text))
			{
				DisplayVal();
				return;
			}

			double oldValue = m_val;

			Value = double.Parse(tbNumricText.Text);
			if (oldValue == m_val)
				DisplayVal();
		}

		private void tbNumricText_TextChanged(object sender, EventArgs e)
		{
        //    double numValue = 0;
        //    if (IsNumeric(tbNumricText.Text))
        //        numValue = double.Parse(tbNumricText.Text);
            
        ////   DoValueChanged();

        //    if (m_valIsAngle)
        //        m_val = Mod360(numValue);

        //    m_val = AdjustValInsideMinMax(m_val);

            //m_val = double.Parse(tbNumricText.Text);
		}

		private void tbNumricText_Leave(object sender, EventArgs e)
		{
			DoValueChanged();
		}

		private void tbNumricText_Validating(object sender, CancelEventArgs e)
		{
			DoValueChanged();
		}

		private void tbNumricText_KeyUp(object sender, KeyEventArgs e)
		{
			if (ReadOnly)
				return;

			if (e.KeyCode == Keys.Enter)
				DoValueChanged();
		}

		private void tbNumricText_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (e.KeyChar == 8 || e.KeyChar == 46 || e.KeyChar == 45)
				return;

			if ((e.KeyChar < '0' || e.KeyChar > '9') && e.KeyChar != '\r')
				e.KeyChar = '\0';

			if (KeyPressChanged != null)
				KeyPressChanged(this, e);
		}

		private void NumericTextBox_Resize(object sender, EventArgs e)
		{
			if (Height < 20)
				Height = 20;

			tbNumricText.Width = Width;
		}
	}

}
