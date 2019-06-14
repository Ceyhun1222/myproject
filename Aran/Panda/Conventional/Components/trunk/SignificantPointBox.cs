using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Aran.Aim.Features;

namespace ChoosePointNS
{
	public partial class SignificantPointBox : UserControl
	{
		private const int GroupBoxLeft = 3;
		private const int GroupBoxTop = 0;

		private const int LabelLeft = 6;
		private const int ComboLeft = 89;

		private const int ControlMid = 28;
		private const int ControlDist = 28;

		public event EventHandler OrganisationAuthorityChanged;
		public event EventHandler AirportHeliportChanged;
		public event EventHandler SignificantPointChanged;
		public event EventHandler PointChanged;

		#region List properties

		/// <summary> 
		///	Lists
		/// </summary>

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public List<OrganisationAuthority> OrganisationAuthorityList
		{
			set
			{
				if (value != null && value != cbOrganisation.DataSource)
				{
					cbOrganisation.DataSource = value;
					cbOrganisation.DisplayMember = "designator";
					if (cbOrganisation.Items.Count > 0)
						cbOrganisation.SelectedIndex = 0;
				}
			}
		}

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public List<AirportHeliport> AirportHeliportList
		{
			set
			{
				if (value != null && value != cbAirport.DataSource)
				{

					cbAirport.DataSource = value;
					cbAirport.DisplayMember = "designator";
					if (cbAirport.Items.Count > 0)
						cbAirport.SelectedIndex = 0;
				}
			}
		}

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public List<SignificantPoint> SignificantPointList
		{
			set
			{
				if (value != null && value != cbPointsList.DataSource)
				{
					for (int i = 0; i < value.Count; i++)
						cbPointsList.Items.Add((value[i]));

					cbPointsList.DisplayMember = "designator";

					if (cbPointsList.Items.Count > 0)
						cbPointsList.SelectedIndex = 0;
				}
			}
		}
		#endregion

		#region Selected Features properties

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public OrganisationAuthority SelectedOrganisationAuthority
		{
			get
			{
				return cbOrganisation.SelectedItem as OrganisationAuthority;
			}
		}

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public AirportHeliport SelectedAirportHeliport
		{
			get
			{
				return cbAirport.SelectedItem as AirportHeliport;
			}
		}

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public SignificantPoint SelectedSignificantPoint
		{
			get
			{
				return cbPointsList.SelectedItem as SignificantPoint;
			}
		}

		#endregion

		private SignificantPointChoice GetSelectedSignificantPointType()
		{
			for (int choiceElement = 1, i = 0, j = 0; choiceElement <= (int)SignificantPointChoiceList.Point; choiceElement += choiceElement, i++)
			{
				if (((int)_choices & choiceElement) != 0)
				{
					if (j == cbSignificantPoint.SelectedIndex)
						return (SignificantPointChoice)i;
					j++;
				}
			}

			return SignificantPointChoice.Point;
		}

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		public SignificantPointChoice SelectedSignificantPointType
		{
			get
			{
				return GetSelectedSignificantPointType();
			}
		}

		[EditorAttribute(typeof(SignificantPointChoiceEditor), typeof(System.Drawing.Design.UITypeEditor))]
		public SignificantPointChoiceList SignificantPointChoiceList
		{
			get
			{
				return _choices;
			}
			set
			{
				_choices = value;

				cbSignificantPoint.Items.Clear();
				for (int choiceElement = 1; choiceElement <= (int)SignificantPointChoiceList.Point; choiceElement += choiceElement)
					if (((int)value & choiceElement) != 0)
						cbSignificantPoint.Items.Add((SignificantPointChoiceList)choiceElement);

				if (cbSignificantPoint.Items.Count > 0)
					cbSignificantPoint.SelectedIndex = 0;
			}
		}

		public ComboBox AirportHeliportComboBox
		{
			get
			{
				return cbAirport;
			}
		}

		public ComboBox OrganisationComboBox
		{
			get
			{
				return cbOrganisation;
			}
		}

		public ComboBox SignificantPointComboBox
		{
			get
			{
				return cbSignificantPoint;
			}
		}

		private void RearrangeControls()
		{
			int lx, ly;
			if (gbMain.Visible)
			{
				lx = 0; ly = 0;

				gbMain.Controls.Add(lbOrganisation);
				gbMain.Controls.Add(lbAirport);
				gbMain.Controls.Add(lbSignificantPoint);

			}
			else
			{
				lx = GroupBoxLeft;
				ly = GroupBoxTop;

				Controls.Add(lbOrganisation);
				Controls.Add(lbAirport);
				Controls.Add(lbSignificantPoint);
			}

			int currY = ControlMid;

			if (cbOrganisation.Visible)
			{
				lbOrganisation.Location = new System.Drawing.Point(lx + LabelLeft, ly + currY - (lbOrganisation.Height >> 1));
				cbOrganisation.Location = new System.Drawing.Point(ComboLeft, currY - (cbOrganisation.Height >> 1));
				currY += ControlDist;
			}

			if (cbAirport.Visible)
			{
				lbAirport.Location = new System.Drawing.Point(lx + LabelLeft, ly + currY - (lbAirport.Height >> 1));
				cbAirport.Location = new System.Drawing.Point(ComboLeft, currY - (cbAirport.Height >> 1));
				currY += ControlDist;
			}
			currY -= ControlDist;

			lbSignificantPoint.Location = new System.Drawing.Point(lx + LabelLeft, ly + currY - (lbSignificantPoint.Height >> 1));
			cbSignificantPoint.Location = new System.Drawing.Point(ComboLeft, currY - (cbSignificantPoint.Height >> 1));
			currY += ControlDist;

			lbSignificantPoint.Location = new System.Drawing.Point(lx + LabelLeft, ly + currY - (lbSignificantPoint.Height >> 1));
			cbSignificantPoint.Location = new System.Drawing.Point(ComboLeft, currY - (cbSignificantPoint.Height >> 1));
			currY += ControlDist;

			cbPointsList.Location = new System.Drawing.Point(ComboLeft, currY - (cbPointsList.Height >> 1));
			currY += ControlDist;
		}

		public bool ShowFrame
		{
			get { return gbMain.Visible; }
			set
			{
				gbMain.Visible = value;
				RearrangeControls();
			}
		}

		public bool ShowOrganisationAuthority
		{
			get { return cbOrganisation.Visible; }
			set
			{
				cbOrganisation.Visible = value;
				lbOrganisation.Visible = value;
				RearrangeControls();
			}
		}

		public bool ShowAirportHeliport
		{
			get { return cbAirport.Visible; }
			set
			{
				cbAirport.Visible = value;
				lbAirport.Visible = value;
				RearrangeControls();
			}
		}

		public SignificantPointBox()
		{
			InitializeComponent();
			ShowFrame = true;
		}

		private SignificantPointChoiceList _choices;

		#region Event handling

		private void cbOrganisation_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (OrganisationAuthorityChanged != null)
				OrganisationAuthorityChanged(this, new EventArgs());
		}

		private void cbAerodrome_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (AirportHeliportChanged != null)
				AirportHeliportChanged(this, new EventArgs());
		}

		private void cbSignificantPoint_SelectedIndexChanged(object sender, EventArgs e)
		{
			SignificantPointChoice spc = GetSelectedSignificantPointType();

			cbPointsList.Enabled = (spc != SignificantPointChoice.AirportHeliport) & (spc != SignificantPointChoice.Point);
			if (SignificantPointChanged != null)
				SignificantPointChanged(this, new EventArgs());
		}

		private void cbPointsList_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (PointChanged != null)
				PointChanged(this, new EventArgs());
		}
		#endregion
	}

	public enum SignificantPointChoice
	{
		Navaid,
		RunwayCentrelinePoint,
		AirportHeliport,
		TouchDownLiftOff,
		DesignatedPoint,
		Point
	}

	[FlagsAttribute]
	public enum SignificantPointChoiceList
	{
		None = 0,
		Navaid = 1,
		RunwayCentrelinePoint = 2,
		AirportHeliport = 4,
		TouchDownLiftOff = 8,
		DesignatedPoint = 16,
		Point = 32
	}
}

