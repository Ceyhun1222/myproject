using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Aran.PANDA.Common;
using Aran.AranEnvironment;
using Aran.Aim.Data;
using Aran.Aim.Enums;
using Aran.Aim;
using Aran.Aim.Features;
using Aran.Aim.Data.Filters;
using System.Threading;

namespace Aran.PANDA.SettingsUI
{
	public partial class SettingsForm : UserControl, ISettingsPage
	{
		#region Declaration

		Common.Settings settings;
		string[,] DistanceUnits = new string[,] { { "km", "NM" }, { "Km", "NM" }, { "км", "ММ" } };
		string[,] HeightUnits = new string[,] { { "meter", "feet" }, { "meter", "feet" }, { "метр", "фут" } };
		string[,] SpeedUnits = new string[,] { { "km/h", "Kt" }, { "km/h", "No" }, { "км/час", "узел" } };
		string[,] DSpeedUnits = new string[,] { { "meter/min", "feet/min" }, { "meter/min", "feet/min" }, { "метр/мин", "фут/мин" } };
		string[,] Languages = new string[,] { { "English", "Portuguese", "Russian" }, { "English", "Portuguese", "Russian" }, { "Английский", "Португалский", "Русский" } };

        string[,] reportDistanceUnits = new string[,] { { "meter","km", "NM" }, {"meter", "Km", "NM" }, { "метр", "км", "ММ" } };

        int DistanceIndex;
		int HeightIndex;
		int SpeedIndex;
		int DSpeedIndex;
		int LangIndex, LangCode;

		Double Radius = 100000.0;
		Guid Organization = Guid.Empty;
		Guid Aeroport = Guid.Empty;

		DbProvider dbProvider = null;

		OrganisationAuthority selectedOrg;
		AirportHeliport selectedADHP;

        const double _minRadius = 100000; //(100 km)


		bool InUse = false;

		#endregion

		public SettingsForm()
		{
			InitializeComponent();
		}

		public string Title
		{
			get { return "PANDA"; }
		}

		public Control Page
		{
			get { return this; }
		}

		public void OnLoad()
		{
			LoadSettings();
		}

		public bool OnSave()
		{
			SaveSettings();
			return true;
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


            SelectedIndex = cmbReportDistanceUnits.SelectedIndex;
            cmbReportDistanceUnits.Items.Clear();
            cmbReportDistanceUnits.Items.Add(reportDistanceUnits[LangIndex, 0]);
            cmbReportDistanceUnits.Items.Add(reportDistanceUnits[LangIndex, 1]);
            cmbReportDistanceUnits.Items.Add(reportDistanceUnits[LangIndex, 2]);
            cmbReportDistanceUnits.SelectedIndex = SelectedIndex;

            SelectedIndex = cmbReportElevUnits.SelectedIndex;
            cmbReportElevUnits.Items.Clear();
            cmbReportElevUnits.Items.Add(HeightUnits[LangIndex, 0]);
            cmbReportElevUnits.Items.Add(HeightUnits[LangIndex, 1]);
            cmbReportElevUnits.SelectedIndex = SelectedIndex;
        }

        private void LoadSettings()
        {
            ui_mainTabControl.SelectedIndex = 0;

            dbProvider = (DbProvider)SettingsGlobals.AranEnvironment.DbProvider;

            if (dbProvider == null)
                return;

            settings = new Common.Settings();
            settings.Load(SettingsGlobals.AranEnvironment);
            LangCode = (int)settings.Language;

            LangIndex = 0;

            if (LangCode == 1033)
                LangIndex = 0;
            else if (LangCode == 1046)
                LangIndex = 1;
            else if (LangCode == 1049)
                LangIndex = 2;

            comboBox5.SelectedIndex = LangIndex;

            LoadCaptions();

            DistanceIndex = (int)settings.UIIntefaceData.DistanceUnit;
            comboBox1.SelectedIndex = DistanceIndex;

            textBox1.Text = settings.UIIntefaceData.DistancePrecision.ToString();

            HeightIndex = (int)settings.UIIntefaceData.HeightUnit;
            comboBox2.SelectedIndex = HeightIndex;

            textBox2.Text = settings.UIIntefaceData.HeightPrecision.ToString();

            SpeedIndex = (int)settings.UIIntefaceData.SpeedUnit;
            comboBox3.SelectedIndex = SpeedIndex;

            textBox3.Text = settings.UIIntefaceData.SpeedPrecision.ToString();

            //DSpeedIndex = settings.DSpeedUnit;
            DSpeedIndex = HeightIndex;
            comboBox4.SelectedIndex = DSpeedIndex;

            textBox4.Text = settings.UIIntefaceData.DSpeedPrecision.ToString();

            textBox5.Text = settings.UIIntefaceData.AnglePrecision.ToString();

            textBox6.Text = settings.UIIntefaceData.GradientPrecision.ToString();


            //Initializing Report Interface Data
            cmbReportDistanceUnits.SelectedIndex = (int)settings.ReportIntefaceData.DistanceUnit;
            txtReportDistancePrecision.Text = settings.ReportIntefaceData.DistancePrecision.ToString();

            cmbReportElevUnits.SelectedIndex = (int)settings.ReportIntefaceData.HeightUnit;
            txtReportElevPrecision.Text = settings.ReportIntefaceData.HeightPrecision.ToString();

            cmbReportSpeedUnits.SelectedIndex = (int)settings.ReportIntefaceData.SpeedUnit;
            txtReportSpeedPrecision.Text = settings.ReportIntefaceData.SpeedPrecision.ToString();

            //DSpeedIndex = settings.DSpeedUnit;
            cmbReportDescentRate.SelectedIndex = cmbReportElevUnits.SelectedIndex;
            txtReportDescentRatePrecision.Text = settings.ReportIntefaceData.DSpeedPrecision.ToString();

            txtReportAnglePrecision.Text = settings.ReportIntefaceData.AnglePrecision.ToString();

            txtReportGradientPrecision.Text = settings.ReportIntefaceData.GradientPrecision.ToString();

            //

            Radius = settings.Radius;
            Organization = settings.Organization;
            Aeroport = settings.Aeroport;

            textBox7.Text = Radius.ToString();

            var result = dbProvider.GetVersionsOf(FeatureType.OrganisationAuthority, TimeSliceInterpretationType.BASELINE);
            if (!result.IsSucceed)
            {
                MessageBox.Show(result.Message);
                return;
            }

            var cbiList = result.GetListAs<OrganisationAuthority>()
                .Select(org => new ComboBoxItem(org))
                .Where(cbi => !string.IsNullOrEmpty(cbi.ToString())).ToList();

            comboBox6.Items.Clear();
			var emptyOrgItem = new ComboBoxItem ( "All" );
            comboBox6.Items.Add(emptyOrgItem);
            cbiList.ForEach(cbi => comboBox6.Items.Add(cbi));

            if (Organization != Guid.Empty)
                comboBox6.SelectedItem = cbiList.Find(cbi => cbi.Item.Identifier == Organization);
            else
                comboBox6.SelectedItem = emptyOrgItem;

            chkAnnexObstacles.Checked = settings.AnnexObstalce;
        }

		private int FeatureSorter(Feature f1, Feature f2)
		{
			if (f1 is OrganisationAuthority)
			{
				OrganisationAuthority oa1 = f1 as OrganisationAuthority;
				OrganisationAuthority oa2 = f2 as OrganisationAuthority;
				return string.Compare(oa1.Designator, oa2.Designator);
			}
			else
			{
				AirportHeliport ah1 = f1 as AirportHeliport;
				AirportHeliport ah2 = f2 as AirportHeliport;
				return string.Compare(ah1.Designator, ah2.Designator);
			}
		}

		private void textBox1_Leave(object sender, EventArgs e)
		{
            Double fTmp;
			if (!Double.TryParse(textBox1.Text, out fTmp))
			    textBox1.Text = settings.UIIntefaceData.DistancePrecision.ToString();
			else
			    settings.UIIntefaceData.DistancePrecision = fTmp;
		}

		private void textBox2_Leave(object sender, EventArgs e)
		{
		    Double fTmp;
		    if (!Double.TryParse(textBox2.Text, out fTmp))
		        textBox2.Text = settings.UIIntefaceData.HeightPrecision.ToString();
		    else
		        settings.UIIntefaceData.HeightPrecision = fTmp;
        }

		private void textBox3_Leave(object sender, EventArgs e)
		{
		    Double fTmp;
		    if (!Double.TryParse(textBox3.Text, out fTmp))
		        textBox3.Text = settings.UIIntefaceData.SpeedPrecision.ToString();
		    else
		        settings.UIIntefaceData.SpeedPrecision = fTmp;
		}

		private void textBox4_Leave(object sender, EventArgs e)
		{
		    Double fTmp;
		    if (!Double.TryParse(textBox4.Text, out fTmp))
		        textBox4.Text = settings.UIIntefaceData.DSpeedPrecision.ToString();
		    else
		        settings.UIIntefaceData.DSpeedPrecision = fTmp;
		}

		private void textBox5_Leave(object sender, EventArgs e)
		{
		    Double fTmp;
		    if (!Double.TryParse(textBox5.Text, out fTmp))
		        textBox4.Text = settings.UIIntefaceData.AnglePrecision.ToString();
		    else
		        settings.UIIntefaceData.AnglePrecision = fTmp;
		}

		private void textBox6_Leave(object sender, EventArgs e)
		{
		    Double fTmp;
		    if (!Double.TryParse(textBox5.Text, out fTmp))
		        textBox4.Text = settings.UIIntefaceData.GradientPrecision.ToString();
		    else
		        settings.UIIntefaceData.GradientPrecision = fTmp;
		}

		private void textBox7_Leave(object sender, EventArgs e)
		{
			Double fTmp;
            if (Double.TryParse(textBox7.Text, out fTmp))
            {
                if (fTmp < _minRadius)
                {
                    textBox7.Text = Radius.ToString();
                    MessageBox.Show("Entered radius must equal or greater than " + _minRadius, Title);
                }
                else
                {
                    Radius = fTmp;
                }
            }
            else
            {
                textBox7.Text = Radius.ToString();
            }
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

        private void comboBox6_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox7.Items.Clear();
            selectedOrg = null;

            var orgCbi = comboBox6.SelectedItem as ComboBoxItem;

            if (dbProvider == null || orgCbi == null)
                return;

            Filter filter = null;
            if (orgCbi.Item != null)
            {
                selectedOrg = orgCbi.Item as OrganisationAuthority;

                var comops = new ComparisonOps(ComparisonOpType.EqualTo, "ResponsibleOrganisation.TheOrganisationAuthority", selectedOrg.Identifier);
                var choice = new OperationChoice(comops);
                filter = new Filter(choice);
            }
            
            var result = dbProvider.GetVersionsOf(FeatureType.AirportHeliport, TimeSliceInterpretationType.BASELINE, default(Guid), false, null, null, filter);

            if (!result.IsSucceed)
            {
                MessageBox.Show(result.Message);
                return;
            }

            var cbiList = result.GetListAs<AirportHeliport>()
                .Select(ah => new ComboBoxItem(ah))
                .Where(cbi => !string.IsNullOrEmpty(cbi.ToString())).ToList();
            cbiList.ForEach(cbi => comboBox7.Items.Add(cbi));

            if (Aeroport != Guid.Empty)
                comboBox7.SelectedItem = cbiList.Find(cbi => cbi.Item.Identifier == Aeroport);
        }

		private void comboBox7_SelectedIndexChanged(object sender, EventArgs e)
		{
            var cbi = comboBox7.SelectedItem as ComboBoxItem;

            if (dbProvider == null || cbi == null)
				return;

            selectedADHP = cbi.Item as AirportHeliport;
		}

        private void txtReportDistancePrecision_Leave(object sender, EventArgs e)
        {
            Double fTmp;
            if (!Double.TryParse(txtReportDistancePrecision.Text, out fTmp))
                txtReportDistancePrecision.Text = settings.ReportIntefaceData.DistancePrecision.ToString();
            else
                settings.ReportIntefaceData.DistancePrecision =fTmp;
        }

        private void txtReportElevPrecision_Leave(object sender, EventArgs e)
        {
            Double fTmp;
            if (!Double.TryParse(txtReportElevPrecision.Text, out fTmp))
                txtReportElevPrecision.Text = settings.ReportIntefaceData.HeightPrecision.ToString();
            else
                settings.ReportIntefaceData.HeightPrecision = fTmp;
        }

        private void txtReportSpeedPrecision_Leave(object sender, EventArgs e)
        {
            Double fTmp;
            if (!Double.TryParse(txtReportSpeedPrecision.Text, out fTmp))
                txtReportSpeedPrecision.Text = settings.ReportIntefaceData.SpeedPrecision.ToString();
            else
                settings.ReportIntefaceData.SpeedPrecision = fTmp;
        }

        private void txtReportDescentRatePrecision_Leave(object sender, EventArgs e)
        {
            Double fTmp;
            if (!Double.TryParse(txtReportDescentRatePrecision.Text, out fTmp))
                txtReportDescentRatePrecision.Text = settings.ReportIntefaceData.DSpeedPrecision.ToString();
            else
                settings.ReportIntefaceData.DSpeedPrecision = fTmp;
        }

        private void txtReportAnglePrecision_Leave(object sender, EventArgs e)
        {
            Double fTmp;
            if (!Double.TryParse(txtReportAnglePrecision.Text, out fTmp))
                txtReportAnglePrecision.Text = settings.ReportIntefaceData.AnglePrecision.ToString();
            else
                settings.ReportIntefaceData.AnglePrecision = fTmp;
        }

        private void txtReportGradientPrecision_Leave(object sender, EventArgs e)
        {
            Double fTmp;
            if (!Double.TryParse(txtReportGradientPrecision.Text, out fTmp))
                txtReportGradientPrecision.Text = settings.ReportIntefaceData.GradientPrecision.ToString();
            else
                settings.ReportIntefaceData.GradientPrecision = fTmp;
        }

        private void SaveSettings()
		{
			settings.UIIntefaceData.DistanceUnit = (HorizantalDistanceType)comboBox1.SelectedIndex;

			settings.UIIntefaceData.HeightUnit = (VerticalDistanceType)comboBox2.SelectedIndex;

			settings.UIIntefaceData.SpeedUnit = (HorizantalSpeedType)comboBox3.SelectedIndex;

			//settings.DSpeedUnit = comboBox4.SelectedIndex;

            settings.ReportIntefaceData.DistanceUnit = (HorizantalDistanceType)cmbReportDistanceUnits.SelectedIndex;
            settings.ReportIntefaceData.HeightUnit = (VerticalDistanceType)cmbReportElevUnits.SelectedIndex;
            settings.ReportIntefaceData.SpeedUnit = (HorizantalSpeedType)cmbReportSpeedUnits.SelectedIndex;

            settings.Language = (int)(Aran.Aim.Enums.language)LangCode;
			settings.Radius = Radius;

            settings.Organization = (selectedOrg != null ? selectedOrg.Identifier : Guid.Empty);
            settings.Aeroport = (selectedADHP != null ? selectedADHP.Identifier : Guid.Empty);

            settings.AnnexObstalce = chkAnnexObstacles.Checked;

			settings.Store(SettingsGlobals.AranEnvironment);
		}
		//#region SettingsHelper

		//public class SettingsHelper 
		//{
		//    public int Language
		//    {
		//        set { _language = value; }
		//    }

		//    public HorizantalDistanceType DistanceUnit
		//    {
		//        set { _distanceUnit = value; }
		//    }

		//    public VerticalDistanceType HeightUnit
		//    {
		//        set { _heightUnit = value; }
		//    }

		//    public HorizantalSpeedType SpeedUnit
		//    {
		//        set { _speedUnit = value; }
		//    }

		//    public double DistancePrecision
		//    {
		//        set { _distancePrecision = value; }
		//    }

		//    public double HeightPrecision
		//    {
		//        set { _heightPrecision = value; }
		//    }

		//    public double SpeedPrecision
		//    {
		//        set { _speedPrecision = value; }
		//    }

		//    public double DSpeedPrecision
		//    {
		//        set { _dSpeedPrecision = value; }
		//    }

		//    public double AnglePrecision
		//    {
		//        set { _anglePrecision = value; }
		//    }

		//    public double GradientPrecision
		//    {
		//        set { _gradientPrecision = value; }
		//    }

		//    protected int _language;
		//    protected HorizantalDistanceType _distanceUnit;
		//    protected VerticalDistanceType _heightUnit;
		//    protected HorizantalSpeedType _speedUnit;

		//    protected double _distancePrecision;
		//    protected double _heightPrecision;
		//    protected double _speedPrecision;
		//    protected double _dSpeedPrecision;
		//    protected double _anglePrecision;
		//    protected double _gradientPrecision;
		//}
		//#endregion
	}

	internal class ComboBoxItem
	{
        private string _text;

        public ComboBoxItem(string text)
        {
            _text = text;
        }
		
        public ComboBoxItem(Feature item)
		{
			Item = item;

            if (item is OrganisationAuthority)
            {
                var oa = item as OrganisationAuthority;

                if (!string.IsNullOrEmpty(oa.Name))
                    _text = oa.Name;
                else if (!string.IsNullOrEmpty(oa.Designator))
                    _text = oa.Designator;
            }
            else if (item is AirportHeliport)
            {
                var oa = item as AirportHeliport;

                if (!string.IsNullOrEmpty(oa.Name))
                    _text = oa.Name;
                else if (!string.IsNullOrEmpty(oa.Designator))
                    _text = oa.Designator;
            }

            if (string.IsNullOrWhiteSpace(_text))
                _text = "[" + item.FeatureType + "]";

		}

		public Feature Item { get; set; }

		public override string ToString()
		{
            return _text;
		}
	}
}
