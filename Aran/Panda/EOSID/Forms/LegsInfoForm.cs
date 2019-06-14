using System;
using System.Windows.Forms;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geometry;

namespace EOSID
{
	public partial class LegsInfoForm : Form
	{
		private CheckBox _reportBtn;
		private int _helpContextID;

		private IElement pTraceSelectElem = null;
		private IElement pProtectSelectElem = null;

		private int maxLegs;
		private int legCnt;
		private TrackLeg[] _legs;

		public LegsInfoForm()
		{
			InitializeComponent();
			maxLegs = 256;
			_legs = new TrackLeg[maxLegs];
			legCnt = 0;

			ListView001.Columns[2].Text += " (" + UnitConverter.HeightUnit + ")";
			ListView001.Columns[3].Text += " (" + UnitConverter.HeightUnit + ")";
			ListView001.Columns[4].Text += " (" + UnitConverter.DistanceUnit + ")";
			ListView001.Columns[6].Text += " (" + UnitConverter.DistanceUnit + ")";
		}

		public void Init(CheckBox Btn, int HelpContext)
		{
			_reportBtn = Btn;
			_helpContextID = HelpContext;
		}

		public void AddLeg(TrackLeg leg)
		{
			if (legCnt >= maxLegs)
			{
				maxLegs += 256;
				Array.Resize<TrackLeg>(ref _legs, maxLegs);
			}

			_legs[legCnt++] = leg;
			ReListTrace();
		}

		public void RemoveLastLeg()
		{
			if (legCnt > 0)
			{
				legCnt--;
				ReListTrace();
			}
		}

		public void DeleteNLegs(int N)
		{
			legCnt -= N;
			if (legCnt < 0)
				legCnt = 0;
			ReListTrace();
		}

		public void RemoveAllLegs()
		{
			DeleteNLegs(legCnt);
		}

		private void ReListTrace()
		{
			ListView001.Items.Clear();
			for (int i = 0; i < legCnt; i++)
			{
				ListViewItem itmX = ListView001.Items.Add((i + 1).ToString());
				itmX.Tag = _legs[i];

				switch (_legs[i].SegmentCode)
				{
					case eLegType.straight:
						itmX.SubItems.Add(new ListViewItem.ListViewSubItem(itmX, "Прямой сегмент"));		// "Прямой сегмент"
						break;
					case eLegType.toHeading:
						itmX.SubItems.Add(new ListViewItem.ListViewSubItem(itmX, "На заданный курс"));		// "На заданный курс"
						break;
					case eLegType.courseIntercept:
						itmX.SubItems.Add(new ListViewItem.ListViewSubItem(itmX, "Перехват курса"));		// "Перехват курса"
						break;
					case eLegType.directToFIX:
						itmX.SubItems.Add(new ListViewItem.ListViewSubItem(itmX, "На заданную WPT"));		// "На заданную WPT"
						break;
					case eLegType.turnAndIntercept:
						itmX.SubItems.Add(new ListViewItem.ListViewSubItem(itmX, "На курс заданной РНС"));	// "На курс заданной РНС"
						break;
					case eLegType.arcIntercept:
						itmX.SubItems.Add(new ListViewItem.ListViewSubItem(itmX, "Arc intercept"));			// "Arc intercept"
						break;
					case eLegType.arcPath:
						itmX.SubItems.Add(new ListViewItem.ListViewSubItem(itmX, "Arc path"));				// "Arc path"
						break;
					default:
						itmX.SubItems.Add(new ListViewItem.ListViewSubItem(itmX, ""));						// ""
						break;
				}

				itmX.SubItems.Add(new ListViewItem.ListViewSubItem(itmX, UnitConverter.HeightToDisplayUnits(_legs[i].ptEnd.BestCase.NetHeight, eRoundMode.NERAEST).ToString()));
				itmX.SubItems.Add(new ListViewItem.ListViewSubItem(itmX, UnitConverter.HeightToDisplayUnits(_legs[i].ptEnd.BestCase.GrossHeight, eRoundMode.NERAEST).ToString()));
				itmX.SubItems.Add(new ListViewItem.ListViewSubItem(itmX, UnitConverter.DistanceToDisplayUnits(_legs[i].BestCase.Length, eRoundMode.NERAEST).ToString()));
				itmX.SubItems.Add(new ListViewItem.ListViewSubItem(itmX, Math.Round(_legs[i].BestCase.FlightTime, 2).ToString()));

				if (_legs[i].SegmentCode == eLegType.straight)
					itmX.SubItems.Add(new ListViewItem.ListViewSubItem(itmX, ""));
				else
					itmX.SubItems.Add(new ListViewItem.ListViewSubItem(itmX, UnitConverter.DistanceToDisplayUnits(_legs[i].BestCase.Turn[0].Radius, eRoundMode.NERAEST).ToString()));

				itmX.SubItems.Add(new ListViewItem.ListViewSubItem(itmX, System.Math.Round(NativeMethods.Modulus(Functions.Dir2Azt(GlobalVars.m_SelectedRWY.pPtPrj[eRWY.PtDER], _legs[i].ptEnd.BestCase.Direction) - GlobalVars.m_CurrADHP.MagVar), 2).ToString()));

				IPoint ptGeoFin = (IPoint)Functions.ToGeo(_legs[i].ptEnd.BestCase.pPoint);
				string FinCoords = Functions.Degree2String(System.Math.Abs(ptGeoFin.Y), Degree2StringMode.DMSLat) + ", " + Functions.Degree2String(System.Math.Abs(ptGeoFin.X), Degree2StringMode.DMSLon);

				itmX.SubItems.Add(new ListViewItem.ListViewSubItem(itmX, FinCoords));
				itmX.SubItems.Add(new ListViewItem.ListViewSubItem(itmX, _legs[i].Comment));
			}
		}

		private void ReSelectTrace()
		{
			Graphics.DeleteElement(pTraceSelectElem);
			Graphics.DeleteElement(pProtectSelectElem);
			pTraceSelectElem = null;
			pProtectSelectElem = null;

			TrackLeg selectedLeg = new TrackLeg();
			bool selected = false;

			for (int i = 0; i < ListView001.Items.Count; i++)
				if (ListView001.Items[i].Selected)
				{
					selectedLeg = (TrackLeg)ListView001.Items[i].Tag;
					selected = true;
					break;
				}

			if (selected)
			{
				pTraceSelectElem = Graphics.DrawPolyline(selectedLeg.BestCase.pNominalPoly, 255);		// RGB(0, 255, 255))
				pTraceSelectElem.Locked = true;

				pProtectSelectElem = Graphics.DrawPolygon(selectedLeg.pProtectionArea, 255);	// RGB(0, 255, 255))
				pProtectSelectElem.Locked = true;
			}
		}

		private void ListView001_SelectedIndexChanged(object sender, EventArgs e)
		{
			ReSelectTrace();
		}

		private void LegsInfoForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			Graphics.DeleteElement(pTraceSelectElem);
			Graphics.DeleteElement(pProtectSelectElem);

			pTraceSelectElem = null;
			pProtectSelectElem = null;

			for (int i = 0; i < ListView001.Items.Count; i++)
				ListView001.Items[i].Selected = false;

			if (e.CloseReason == CloseReason.UserClosing)
			{
				e.Cancel = true;
				Hide();
				_reportBtn.Checked = false;
			}
		}
	}
}
