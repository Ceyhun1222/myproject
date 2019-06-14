using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CDOTMA
{
	public partial class SettingsDlg : Form
	{
		#region Declarations

        string [,] DistanceUnits = new string [,] { { "km", "NM" }, { "Km", "NM" }, { "км", "ММ" } };
        string [,] HeightUnits = new string [,] { { "meter", "feet" }, { "meter", "feet" }, { "метр", "фут" } };
        string [,] SpeedUnits = new string [,] { { "km/h", "Kt" }, { "km/h", "No" }, { "км/час", "узел" } };
        string [,] DSpeedUnits = new string [,] { { "meter/min", "feet/min" }, { "meter/min", "feet/min" }, { "метр/мин", "фут/мин" } };
        string [,] Languages = new string [,] { { "English", "Portuguese", "Russian" }, { "English", "Portuguese", "Russian" }, { "Английский", "Португалский", "Русский" } };

		Settings _settings;

		int DistanceIndex;
        int HeightIndex;
        int SpeedIndex;
        int DSpeedIndex;
        int LangIndex, LangCode;

        double DistancePrecision;
        double HeightPrecision;
        double SpeedPrecision;
        double DSpeedPrecision;
		double AnglePrecision;
		double GradientPrecision;

        bool InUse = false;

        #endregion

		public SettingsDlg()
		{
			InitializeComponent();
		}

		public static System.Windows.Forms.DialogResult ShowDialog(out Settings result, Settings initial = null)
		{
			SettingsDlg dlg = new SettingsDlg();

			if (initial == null)
				result = new Settings();
			else
				result = initial.Clone();

			dlg._settings = result.Clone();
			dlg.InitFromSettings();

			System.Windows.Forms.DialogResult dialogResult = dlg.ShowDialog();

			if (dialogResult == System.Windows.Forms.DialogResult.OK)
			{
				dlg.GetFromDialog();
				result = dlg._settings.Clone();
			}

			dlg = null;
			return dialogResult;
		}

		private void LoadCaptions()
		{
			if (InUse)
				return;
			InUse = true;

			int SelectedIndex = comboBox1.SelectedIndex;
			comboBox1.Items.Clear();
			comboBox1.Items.Add(DistanceUnits[LangIndex, 0]);
			comboBox1.Items.Add(DistanceUnits[LangIndex, 1]);
			comboBox1.SelectedIndex = SelectedIndex;

			SelectedIndex = comboBox2.SelectedIndex;
			comboBox2.Items.Clear();
			comboBox2.Items.Add(HeightUnits[LangIndex, 0]);
			comboBox2.Items.Add(HeightUnits[LangIndex, 1]);
			comboBox2.SelectedIndex = SelectedIndex;

			SelectedIndex = comboBox3.SelectedIndex;
			comboBox3.Items.Clear();
			comboBox3.Items.Add(SpeedUnits[LangIndex, 0]);
			comboBox3.Items.Add(SpeedUnits[LangIndex, 1]);
			comboBox3.SelectedIndex = SelectedIndex;

			SelectedIndex = comboBox4.SelectedIndex;
			comboBox4.Items.Clear();
			comboBox4.Items.Add(DSpeedUnits[LangIndex, 0]);
			comboBox4.Items.Add(DSpeedUnits[LangIndex, 1]);
			comboBox4.SelectedIndex = SelectedIndex;

			SelectedIndex = comboBox5.SelectedIndex;
			comboBox5.Items.Clear();
			comboBox5.Items.Add(Languages[LangIndex, 0]);
			comboBox5.Items.Add(Languages[LangIndex, 1]);
			comboBox5.Items.Add(Languages[LangIndex, 2]);
			comboBox5.SelectedIndex = SelectedIndex;
			InUse = false;
		}

		private void InitFromSettings()
		{
			//_settings = value;
			LangCode = (int)_settings.Language;

			LangIndex = 0;

			if (LangCode == 1033)
				LangIndex = 0;
			else if (LangCode == 1046)
				LangIndex = 1;
			else if (LangCode == 1049)
				LangIndex = 2;

			comboBox5.SelectedIndex = LangIndex;

			DistanceIndex = (int)_settings.DistanceUnit;
			comboBox1.SelectedIndex = DistanceIndex;

			DistancePrecision = _settings.DistancePrecision;
			textBox1.Text = DistancePrecision.ToString();

			HeightIndex = (int)_settings.HeightUnit;
			comboBox2.SelectedIndex = HeightIndex;

			HeightPrecision = _settings.HeightPrecision;
			textBox2.Text = HeightPrecision.ToString();

			SpeedIndex = (int)_settings.SpeedUnit;
			comboBox3.SelectedIndex = SpeedIndex;

			SpeedPrecision = _settings.SpeedPrecision;
			textBox3.Text = SpeedPrecision.ToString();

			//DSpeedIndex = settings.DSpeedUnit;
			DSpeedIndex = HeightIndex;
			comboBox4.SelectedIndex = DSpeedIndex;

			DSpeedPrecision = _settings.DSpeedPrecision;
			textBox4.Text = DSpeedPrecision.ToString();

			AnglePrecision = _settings.AnglePrecision;
			textBox5.Text = AnglePrecision.ToString();

			GradientPrecision = _settings.GradientPrecision;
			textBox6.Text = GradientPrecision.ToString();
		}

		private void LoadSettings(string FileName)
		{
			_settings = Settings.Read(FileName);
			InitFromSettings();
			LoadCaptions();
		}

		private void GetFromDialog()
		{
			_settings.DistanceUnit = (HorizantalDistanceType)comboBox1.SelectedIndex;
			_settings.DistancePrecision = DistancePrecision;

			_settings.HeightUnit = (VerticalDistanceType)comboBox2.SelectedIndex;
			_settings.HeightPrecision = HeightPrecision;

			_settings.SpeedUnit = (HorizantalSpeedType)comboBox3.SelectedIndex;
			_settings.SpeedPrecision = SpeedPrecision;

			//_settings.DSpeedUnit = comboBox4.SelectedIndex;
			_settings.DSpeedPrecision = DSpeedPrecision;

			_settings.AnglePrecision = AnglePrecision;
			_settings.GradientPrecision = GradientPrecision;

			_settings.Language = LangCode;
		}

		private void SaveSettings(string FileName)
		{
			GetFromDialog();
			_settings.Write(FileName);
		}

		private void textBox1_Leave(object sender, EventArgs e)
		{
			double fTmp;
			if (double.TryParse(textBox1.Text, out fTmp))
				DistancePrecision = fTmp;
			else
				textBox1.Text = DistancePrecision.ToString();
		}

		private void textBox2_Leave(object sender, EventArgs e)
		{
			double fTmp;
			if (double.TryParse(textBox2.Text, out fTmp))
				HeightPrecision = fTmp;
			else
				textBox2.Text = HeightPrecision.ToString();
		}

		private void textBox3_Leave(object sender, EventArgs e)
		{
			double fTmp;
			if (double.TryParse(textBox3.Text, out fTmp))
				SpeedPrecision = fTmp;
			else
				textBox3.Text = SpeedPrecision.ToString();
		}

		private void textBox4_Leave(object sender, EventArgs e)
		{
			double fTmp;
			if (double.TryParse(textBox4.Text, out fTmp))
				DSpeedPrecision = fTmp;
			else
				textBox4.Text = DSpeedPrecision.ToString();
		}

		private void textBox5_Leave(object sender, EventArgs e)
		{
			double fTmp;
			if (double.TryParse(textBox5.Text, out fTmp))
				AnglePrecision = fTmp;
			else
				textBox5.Text = AnglePrecision.ToString();
		}

		private void textBox6_Leave(object sender, EventArgs e)
		{
			double fTmp;
			if (double.TryParse(textBox6.Text, out fTmp))
				GradientPrecision = fTmp;
			else
				textBox6.Text = GradientPrecision.ToString();
		}


		private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
		{
			comboBox4.SelectedIndex = comboBox2.SelectedIndex;
		}

		private void comboBox5_SelectedIndexChanged(object sender, EventArgs e)
		{
			LangIndex = comboBox5.SelectedIndex;
			if (LangIndex < 0)
				return;

			if (LangIndex == 0)
				LangCode = 1033;
			else if (LangIndex == 1)
				LangCode = 1046;
			else if (LangIndex == 2)
				LangCode = 1049;

			LoadCaptions();
		}

	}
}
