using Aran.Geometries;
using Aran.PANDA.Common;
using Aran.PANDA.RNAV.PBNAPVBaroVNav.Properties;
using System;
using System.IO;
using System.Windows.Forms;

namespace Aran.PANDA.RNAV.PBNAPVBaroVNav
{
	public partial class ReportForm : Form
	{
		const int _helpContextID = 3700;
		//private int _helpContextID;

		const int Page01SortIndex = 8;
		const int Page02SortIndex = 4;

		private int _sortF1;
		private int _sortF2;

		private ObstacleContainer _obstaclesPage1;
		private ObstacleContainer _obstaclesPage2;

		private int _pointElem;
		private int _geomElem;

		private TabPage[] _tapPages;
		private bool[] _pageVisible;

		private TabPage _previousTab;

		public static string[] APVAreaNames = { "", " Side" };

		private CheckBox _reportBtn;

		public ReportForm()
		{
			InitializeComponent();

			_sortF1 = _sortF2 = 0;
			//_sortF3 = 0;

			this.Text = Resources.str10000;

			_pointElem = -1;
			_geomElem = -1;

			_previousTab = null;
			_tapPages = new TabPage[] { tsAPVSegment, tsIntermediateApproach};

			_pageVisible = new bool[_tapPages.Length];

			mainTabControl.Controls.Clear();

			_tapPages[0].Text = Resources.str11000;
			_tapPages[1].Text = Resources.str12000;

			listView1.Columns[0].Text = Resources.str11001;
			listView1.Columns[1].Text = Resources.str11002;
			listView1.Columns[2].Text = Resources.str11003 + " (" + GlobalVars.unitConverter.HeightUnit + ")";
			listView1.Columns[3].Text = Resources.str11004 + " (" + GlobalVars.unitConverter.HeightUnit + ")";
			listView1.Columns[4].Text = Resources.str11005 + " (" + GlobalVars.unitConverter.HeightUnit + ")";
			listView1.Columns[5].Text = Resources.str11006 + " (" + GlobalVars.unitConverter.HeightUnit + ")";
			listView1.Columns[6].Text = Resources.str11007;
			listView1.Columns[7].Text = Resources.str11008 + " (" + GlobalVars.unitConverter.HeightUnit + ")";
			listView1.Columns[8].Text = Resources.str11009 + " (" + GlobalVars.unitConverter.HeightUnit + ")";
			listView1.Columns[9].Text = Resources.str11010 + " (" + GlobalVars.unitConverter.HeightUnitM +")";
			listView1.Columns[10].Text = Resources.str11011 + " (" + GlobalVars.unitConverter.HeightUnitM + ")";
			listView1.Columns[11].Text = Resources.str11012;
			listView1.Columns[12].Text = Resources.str11013;


			saveButton.Text = Resources.str00007;
			closeButton.Text = Resources.str00008;
		}

		private void ReportForm_KeyUp(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.F1)
			{
				NativeMethods.HtmlHelp(0, GlobalVars.HelpFile, GlobalVars.HH_HELP_CONTEXT, _helpContextID);
				e.Handled = true;
			}
		}

		private void ReportForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			GlobalVars.gAranGraphics.SafeDeleteGraphic(_pointElem);
			GlobalVars.gAranGraphics.SafeDeleteGraphic(_geomElem);

			if (e.CloseReason == CloseReason.UserClosing)
			{
				e.Cancel = true;
				Hide();
				_reportBtn.Checked = false;
			}
		}

		protected override void OnHandleCreated(EventArgs e)
		{
			base.OnHandleCreated(e);

			// Get a handle to a copy of this form's system (window) menu
			IntPtr hSysMenu = Functions.GetSystemMenu(this.Handle, false);
			// Add a separator
			Functions.AppendMenu(hSysMenu, GlobalVars.MF_SEPARATOR, 0, string.Empty);
			// Add the About menu item
			Functions.AppendMenu(hSysMenu, GlobalVars.MF_STRING, GlobalVars.SYSMENU_ABOUT_ID, "&About…");
		}

		protected override void WndProc(ref Message m)
		{
			base.WndProc(ref m);

			if ((m.Msg == GlobalVars.WM_SYSCOMMAND) && ((int)m.WParam == GlobalVars.SYSMENU_ABOUT_ID))
			{
				AboutForm about = new AboutForm();
				about.ShowDialog(this);
				about = null;
			}
		}

		private void mainTabControl_SelectedIndexChanged(object sender, EventArgs e)
		{
			TabPage SelectedTab = mainTabControl.SelectedTab;

			if (SelectedTab == _previousTab)
				return;

			_previousTab = SelectedTab;

			if (this.Visible)
			{
				if (SelectedTab == _tapPages[0])
					listViews_SelectedIndexChanged(listView1, new System.EventArgs());
				else if (SelectedTab == _tapPages[1])
					listViews_SelectedIndexChanged(listView2, new System.EventArgs());
			}
		}

		#region Utilities

		public void SetBtn(CheckBox Btn, int HelpContext)
		{
			_reportBtn = Btn;
			//_helpContextID = HelpContext;
		}

		public string GetTabPageText(int index)
		{
			return _tapPages[index].Text;
		}

		public void SetTabVisible(int index, bool visible)
		{
			if (index < 0)
			{
				for (int i = 0; i < _tapPages.Length; i++)
				{
					_pageVisible[i] = visible;

					if (visible)
					{
						if (!mainTabControl.Controls.Contains(_tapPages[i]))
							mainTabControl.Controls.Add(_tapPages[i]);
					}
					else if (mainTabControl.Controls.Contains(_tapPages[i]))
						mainTabControl.Controls.Remove(_tapPages[i]);
				}
			}
			else if (index < _tapPages.Length)
			{
				_pageVisible[index] = visible;

				if (visible)
				{
					if (!mainTabControl.Controls.Contains(_tapPages[index]))
						mainTabControl.Controls.Add(_tapPages[index]);

					int i = mainTabControl.Controls.GetChildIndex(_tapPages[index]);
					mainTabControl.SelectedIndex = i;
				}
				else if (mainTabControl.Controls.Contains(_tapPages[index]))
					mainTabControl.Controls.Remove(_tapPages[index]);
			}
		}

		public void SortForSave()
		{
			int currIndex, prevIndex;

			//Page 1 ========================================================================================
			currIndex = Page01SortIndex;
			prevIndex = System.Math.Abs(_sortF1) - 1;

			if (prevIndex >= 0 && currIndex != prevIndex)
				listView1.Columns[prevIndex].ImageIndex = 2;

			_sortF1 = currIndex + 1;
			UpdateListView01(currIndex);

			//Page 2 ========================================================================================
			currIndex = Page02SortIndex;
			prevIndex = System.Math.Abs(_sortF2) - 1;

			if (prevIndex >= 0 && currIndex != prevIndex)
				listView2.Columns[prevIndex].ImageIndex = 2;

			_sortF2 = currIndex + 1;
			UpdateListView02(currIndex);
		}

		#endregion

		private void UpdateListView01(int sortByField = -1, bool preClear = false)
		{
			int i, n;

			n = _obstaclesPage1.Parts.Length;

			if (sortByField >= 0)
			{
				if (Math.Abs(_sortF1) - 1 == sortByField)
					_sortF1 = -_sortF1;

				if (preClear || (Math.Abs(_sortF1) - 1 != sortByField))
				{
					if (Math.Abs(_sortF1) - 1 != sortByField)
					{
						if (_sortF1 != 0) listView1.Columns[System.Math.Abs(_sortF1) - 1].ImageIndex = 2;
						_sortF1 = -sortByField - 1;
					}

					if (sortByField <= 1 || sortByField > 10)
						for (i = 0; i < n; i++)
							switch (sortByField)
							{
								case 0:
									_obstaclesPage1.Parts[i].sSort = _obstaclesPage1.Obstacles[_obstaclesPage1.Parts[i].Owner].TypeName; break;
								case 1:
									_obstaclesPage1.Parts[i].sSort = _obstaclesPage1.Obstacles[_obstaclesPage1.Parts[i].Owner].UnicalName; break;
								case 11:
									_obstaclesPage1.Parts[i].sSort = GlobalVars.SurfaceNames[_obstaclesPage1.Parts[i].Plane]; break;
								case 12:
									_obstaclesPage1.Parts[i].sSort = APVAreaNames[_obstaclesPage1.Parts[i].Flags]; break;
							}
					else
						for (i = 0; i < n; i++)
							switch (sortByField)
							{
								case 2:
									_obstaclesPage1.Parts[i].fSort = _obstaclesPage1.Parts[i].hSurface; break;
								case 3:
									_obstaclesPage1.Parts[i].fSort = _obstaclesPage1.Parts[i].Height; break;
								case 4:
									_obstaclesPage1.Parts[i].fSort = _obstaclesPage1.Parts[i].EffectiveHeight; break;
								case 5:
									_obstaclesPage1.Parts[i].fSort = _obstaclesPage1.Parts[i].hPenet; break;
								case 6:
									_obstaclesPage1.Parts[i].fSort = _obstaclesPage1.Parts[i].fSecCoeff; break;
								case 7:
									_obstaclesPage1.Parts[i].fSort = _obstaclesPage1.Parts[i].ReqOCH; break;
								case 8:
									_obstaclesPage1.Parts[i].fSort = _obstaclesPage1.Parts[i].ReqH; break;
								case 9:
									_obstaclesPage1.Parts[i].fSort = _obstaclesPage1.Parts[i].Dist; break;
								case 10:
									_obstaclesPage1.Parts[i].fSort = _obstaclesPage1.Parts[i].CLShift; break;
							}
				}

				if (_sortF1 > 0)
					listView1.Columns[sortByField].ImageIndex = 0;
				else
					listView1.Columns[sortByField].ImageIndex = 1;

				if (sortByField <= 1 || sortByField > 10)
				{
					if (_sortF1 > 0)
						Functions.Shall_SortsSort(_obstaclesPage1.Parts);
					else
						Functions.Shall_SortsSortD(_obstaclesPage1.Parts);
				}
				else if (_sortF1 > 0)
					Functions.Shall_SortfSort(_obstaclesPage1.Parts);
				else
					Functions.Shall_SortfSortD(_obstaclesPage1.Parts);
			}

			listView1.BeginUpdate();

			if (preClear)
				listView1.Items.Clear();

			for (i = 0; i < n; i++)
			{
				ListViewItem itmX;

				if (preClear)
				{
					itmX = listView1.Items.Add(_obstaclesPage1.Obstacles[_obstaclesPage1.Parts[i].Owner].TypeName);
					itmX.SubItems.Insert(1, new ListViewItem.ListViewSubItem(null, _obstaclesPage1.Obstacles[_obstaclesPage1.Parts[i].Owner].UnicalName));

					itmX.SubItems.Insert(2, new ListViewItem.ListViewSubItem(null, GlobalVars.unitConverter.HeightToDisplayUnits(_obstaclesPage1.Parts[i].hSurface, eRoundMode.NEAREST).ToString()));
					itmX.SubItems.Insert(3, new ListViewItem.ListViewSubItem(null, GlobalVars.unitConverter.HeightToDisplayUnits(_obstaclesPage1.Parts[i].Height, eRoundMode.NEAREST).ToString()));
					itmX.SubItems.Insert(4, new ListViewItem.ListViewSubItem(null, GlobalVars.unitConverter.HeightToDisplayUnits(_obstaclesPage1.Parts[i].EffectiveHeight, eRoundMode.NEAREST).ToString()));
					itmX.SubItems.Insert(5, new ListViewItem.ListViewSubItem(null, GlobalVars.unitConverter.HeightToDisplayUnits(_obstaclesPage1.Parts[i].hPenet, eRoundMode.NEAREST).ToString()));

					itmX.SubItems.Insert(6, new ListViewItem.ListViewSubItem(null, _obstaclesPage1.Parts[i].fSecCoeff.ToString("0.000")));
					itmX.SubItems.Insert(7, new ListViewItem.ListViewSubItem(null, GlobalVars.unitConverter.HeightToDisplayUnits(_obstaclesPage1.Parts[i].ReqOCH, eRoundMode.NEAREST).ToString()));
					itmX.SubItems.Insert(8, new ListViewItem.ListViewSubItem(null, GlobalVars.unitConverter.HeightToDisplayUnits(_obstaclesPage1.Parts[i].ReqH, eRoundMode.NEAREST).ToString()));

					itmX.SubItems.Insert(9, new ListViewItem.ListViewSubItem(null, _obstaclesPage1.Parts[i].Dist.ToString("0.0")));
					itmX.SubItems.Insert(10, new ListViewItem.ListViewSubItem(null, _obstaclesPage1.Parts[i].CLShift.ToString("0.0")));

					itmX.SubItems.Insert(11, new ListViewItem.ListViewSubItem(null, GlobalVars.SurfaceNames[_obstaclesPage1.Parts[i].Plane]));
					itmX.SubItems.Insert(12, new ListViewItem.ListViewSubItem(null, APVAreaNames[_obstaclesPage1.Parts[i].Flags]));
				}
				else
				{
					itmX = listView1.Items[i];

					itmX.Text = _obstaclesPage1.Obstacles[_obstaclesPage1.Parts[i].Owner].TypeName;
					itmX.SubItems[1].Text = _obstaclesPage1.Obstacles[_obstaclesPage1.Parts[i].Owner].UnicalName;

					itmX.SubItems[2].Text = GlobalVars.unitConverter.HeightToDisplayUnits(_obstaclesPage1.Parts[i].hSurface, eRoundMode.NEAREST).ToString();
					itmX.SubItems[3].Text = GlobalVars.unitConverter.HeightToDisplayUnits(_obstaclesPage1.Parts[i].Height, eRoundMode.NEAREST).ToString();
					itmX.SubItems[4].Text = GlobalVars.unitConverter.HeightToDisplayUnits(_obstaclesPage1.Parts[i].EffectiveHeight, eRoundMode.NEAREST).ToString();
					itmX.SubItems[5].Text = GlobalVars.unitConverter.HeightToDisplayUnits(_obstaclesPage1.Parts[i].hPenet, eRoundMode.NEAREST).ToString();

					itmX.SubItems[6].Text = _obstaclesPage1.Parts[i].fSecCoeff.ToString("0.000");
					itmX.SubItems[7].Text = GlobalVars.unitConverter.HeightToDisplayUnits(_obstaclesPage1.Parts[i].ReqOCH, eRoundMode.NEAREST).ToString();
					itmX.SubItems[8].Text = GlobalVars.unitConverter.HeightToDisplayUnits(_obstaclesPage1.Parts[i].ReqH, eRoundMode.NEAREST).ToString();

					itmX.SubItems[9].Text = _obstaclesPage1.Parts[i].Dist.ToString("0.0");
					itmX.SubItems[10].Text = _obstaclesPage1.Parts[i].CLShift.ToString("0.0");

					itmX.SubItems[11].Text = GlobalVars.SurfaceNames[_obstaclesPage1.Parts[i].Plane];
					itmX.SubItems[12].Text = APVAreaNames[_obstaclesPage1.Parts[i].Flags];
				}

				itmX.SubItems[4].Font = new System.Drawing.Font(itmX.SubItems[4].Font, System.Drawing.FontStyle.Bold);
			}

			listView1.EndUpdate();
		}

		private void UpdateListView02(int sortByField = -1, bool preClear = false)
		{
			int i, n = _obstaclesPage2.Parts.Length;

			if (sortByField >= 0)
			{
				if (System.Math.Abs(_sortF2) - 1 == sortByField)
					_sortF2 = -_sortF2;

				if (preClear || (System.Math.Abs(_sortF2) - 1 != sortByField))
				{
					if (System.Math.Abs(_sortF2) - 1 != sortByField)
					{
						if (_sortF2 != 0)
							listView2.Columns[System.Math.Abs(_sortF2) - 1].ImageIndex = 2;
						_sortF2 = -sortByField - 1;
					}

					if (sortByField <= 1 || sortByField == 7)
						for (i = 0; i < n; i++)
							switch (sortByField)
							{
								case 0:
									_obstaclesPage2.Parts[i].sSort = _obstaclesPage2.Obstacles[_obstaclesPage2.Parts[i].Owner].TypeName; break;
								case 1:
									_obstaclesPage2.Parts[i].sSort = _obstaclesPage2.Obstacles[_obstaclesPage2.Parts[i].Owner].UnicalName; break;
								case 7:
									_obstaclesPage2.Parts[i].sSort = APVAreaNames[_obstaclesPage2.Parts[i].Flags]; break;
							}
					else
						for (i = 0; i < n; i++)
							switch (sortByField)
							{
								case 2:
									_obstaclesPage2.Parts[i].fSort = _obstaclesPage2.Parts[i].Height; break;
								case 3:
									_obstaclesPage2.Parts[i].fSort = _obstaclesPage2.Parts[i].MOC; break;
								case 4:
									_obstaclesPage2.Parts[i].fSort = _obstaclesPage2.Parts[i].ReqOCH; break;
								case 5:
									_obstaclesPage2.Parts[i].fSort = _obstaclesPage2.Parts[i].Dist; break;
								case 6:
									_obstaclesPage2.Parts[i].fSort = _obstaclesPage2.Parts[i].CLShift; break;
							}
				}

				if (_sortF2 > 0)
					listView2.Columns[sortByField].ImageIndex = 0;
				else
					listView2.Columns[sortByField].ImageIndex = 1;

				if (sortByField <= 1 || sortByField == 7)
				{
					if (_sortF2 > 0)
						Functions.Shall_SortsSort(_obstaclesPage2.Parts);
					else
						Functions.Shall_SortsSortD(_obstaclesPage2.Parts);
				}
				else if (_sortF2 > 0)
					Functions.Shall_SortfSort(_obstaclesPage2.Parts);
				else
					Functions.Shall_SortfSortD(_obstaclesPage2.Parts);
			}

			listView2.BeginUpdate();

			if (preClear)
				listView2.Items.Clear();

			for (i = 0; i < n; i++)
			{
				ListViewItem itmX;

				if (preClear)
				{
					itmX = listView2.Items.Add(_obstaclesPage2.Obstacles[_obstaclesPage2.Parts[i].Owner].TypeName);
					itmX.SubItems.Insert(1, new ListViewItem.ListViewSubItem(null, _obstaclesPage2.Obstacles[_obstaclesPage2.Parts[i].Owner].UnicalName));

					itmX.SubItems.Insert(2, new ListViewItem.ListViewSubItem(null, GlobalVars.unitConverter.HeightToDisplayUnits(_obstaclesPage2.Parts[i].Height, eRoundMode.NEAREST).ToString()));
					itmX.SubItems.Insert(3, new ListViewItem.ListViewSubItem(null, GlobalVars.unitConverter.HeightToDisplayUnits(_obstaclesPage2.Parts[i].MOC, eRoundMode.NEAREST).ToString()));
					itmX.SubItems.Insert(4, new ListViewItem.ListViewSubItem(null, GlobalVars.unitConverter.HeightToDisplayUnits(_obstaclesPage2.Parts[i].ReqOCH, eRoundMode.NEAREST).ToString()));

					itmX.SubItems.Insert(5, new ListViewItem.ListViewSubItem(null, _obstaclesPage2.Parts[i].Dist.ToString("0.0")));
					itmX.SubItems.Insert(6, new ListViewItem.ListViewSubItem(null, _obstaclesPage2.Parts[i].CLShift.ToString("0.0")));

					itmX.SubItems.Insert(7, new ListViewItem.ListViewSubItem(null, APVAreaNames[_obstaclesPage2.Parts[i].Flags]));
				}
				else
				{
					itmX = listView2.Items[i];

					itmX.Text = _obstaclesPage2.Obstacles[_obstaclesPage2.Parts[i].Owner].TypeName;
					itmX.SubItems[1].Text = _obstaclesPage2.Obstacles[_obstaclesPage2.Parts[i].Owner].UnicalName;

					itmX.SubItems[2].Text = GlobalVars.unitConverter.HeightToDisplayUnits(_obstaclesPage2.Parts[i].Height, eRoundMode.NEAREST).ToString();
					itmX.SubItems[3].Text = GlobalVars.unitConverter.HeightToDisplayUnits(_obstaclesPage2.Parts[i].MOC, eRoundMode.NEAREST).ToString();
					itmX.SubItems[4].Text = GlobalVars.unitConverter.HeightToDisplayUnits(_obstaclesPage2.Parts[i].ReqOCH, eRoundMode.NEAREST).ToString();

					itmX.SubItems[5].Text = _obstaclesPage2.Parts[i].Dist.ToString("0.0");
					itmX.SubItems[6].Text = _obstaclesPage2.Parts[i].CLShift.ToString("0.0");

					itmX.SubItems[7].Text = APVAreaNames[_obstaclesPage2.Parts[i].Plane];
				}

				itmX.SubItems[4].Font = new System.Drawing.Font(itmX.SubItems[4].Font, System.Drawing.FontStyle.Bold);
			}

			listView2.EndUpdate();
		}

		//private void UpdateListView03(int sortByField = -1, bool preClear = false)
		//{
		//	int i, n;

		//	n = _obstaclesPage3.Parts.Length;

		//	if (sortByField >= 0)
		//	{
		//		if (System.Math.Abs(_sortF3) - 1 == sortByField)
		//			_sortF3 = -_sortF3;

		//		if (preClear || (System.Math.Abs(_sortF3) - 1 != sortByField))
		//		{
		//			if (System.Math.Abs(_sortF3) - 1 != sortByField)
		//			{
		//				if (_sortF3 != 0) listView3.Columns[System.Math.Abs(_sortF3) - 1].ImageIndex = 2;
		//				_sortF3 = -sortByField - 1;
		//			}

		//			if (sortByField <= 1 || sortByField > 8)
		//				for (i = 0; i < n; i++)
		//					switch (sortByField)
		//					{
		//						case 0:
		//							_obstaclesPage3.Parts[i].sSort = _obstaclesPage3.Obstacles[_obstaclesPage3.Parts[i].Owner].TypeName; break;
		//						case 1:
		//							_obstaclesPage3.Parts[i].sSort = _obstaclesPage3.Obstacles[_obstaclesPage3.Parts[i].Owner].UnicalName; break;
		//						case 9:
		//							_obstaclesPage3.Parts[i].sSort = GlobalVars.SurfaceNames[_obstaclesPage3.Parts[i].Plane]; break;
		//						case 10:
		//							_obstaclesPage3.Parts[i].sSort = APVAreaNames[_obstaclesPage3.Parts[i].Flags]; break;
		//					}
		//			else
		//				for (i = 0; i < n; i++)
		//					switch (sortByField)
		//					{
		//						case 2:
		//							_obstaclesPage3.Parts[i].fSort = _obstaclesPage3.Parts[i].hSurface; break;
		//						case 3:
		//							_obstaclesPage3.Parts[i].fSort = _obstaclesPage3.Parts[i].Height; break;
		//						case 4:
		//							_obstaclesPage3.Parts[i].fSort = _obstaclesPage3.Parts[i].hPenet; break;
		//						case 5:
		//							_obstaclesPage3.Parts[i].fSort = _obstaclesPage3.Parts[i].fSecCoeff; break;
		//						case 6:
		//							_obstaclesPage3.Parts[i].fSort = _obstaclesPage3.Parts[i].ReqOCH; break;
		//						case 7:
		//							_obstaclesPage3.Parts[i].fSort = _obstaclesPage3.Parts[i].Dist; break;
		//						case 8:
		//							_obstaclesPage3.Parts[i].fSort = _obstaclesPage3.Parts[i].CLShift; break;
		//					}
		//		}

		//		if (_sortF3 > 0)
		//			listView3.Columns[sortByField].ImageIndex = 0;
		//		else
		//			listView3.Columns[sortByField].ImageIndex = 1;

		//		if (sortByField <= 1 || sortByField > 8)
		//		{
		//			if (_sortF3 > 0)
		//				Functions.Shall_SortsSort(_obstaclesPage3.Parts);
		//			else
		//				Functions.Shall_SortsSortD(_obstaclesPage3.Parts);
		//		}
		//		else if (_sortF3 > 0)
		//			Functions.Shall_SortfSort(_obstaclesPage3.Parts);
		//		else
		//			Functions.Shall_SortfSortD(_obstaclesPage3.Parts);
		//	}

		//	listView3.BeginUpdate();

		//	if (preClear)
		//		listView3.Items.Clear();

		//	for (i = 0; i < n; i++)
		//	{
		//		ListViewItem itmX;

		//		if (preClear)
		//		{
		//			itmX = listView3.Items.Add(_obstaclesPage3.Obstacles[_obstaclesPage3.Parts[i].Owner].TypeName);
		//			itmX.SubItems.Insert(1, new ListViewItem.ListViewSubItem(null, _obstaclesPage3.Obstacles[_obstaclesPage3.Parts[i].Owner].UnicalName));

		//			itmX.SubItems.Insert(2, new ListViewItem.ListViewSubItem(null, GlobalVars.unitConverter.HeightToDisplayUnits(_obstaclesPage3.Parts[i].hSurface, eRoundMode.NEAREST).ToString()));
		//			itmX.SubItems.Insert(3, new ListViewItem.ListViewSubItem(null, GlobalVars.unitConverter.HeightToDisplayUnits(_obstaclesPage3.Parts[i].Height, eRoundMode.NEAREST).ToString()));
		//			itmX.SubItems.Insert(4, new ListViewItem.ListViewSubItem(null, GlobalVars.unitConverter.HeightToDisplayUnits(_obstaclesPage3.Parts[i].hPenet, eRoundMode.NEAREST).ToString()));

		//			itmX.SubItems.Insert(5, new ListViewItem.ListViewSubItem(null, _obstaclesPage3.Parts[i].fSecCoeff.ToString("0.000")));
		//			itmX.SubItems.Insert(6, new ListViewItem.ListViewSubItem(null, GlobalVars.unitConverter.HeightToDisplayUnits(_obstaclesPage3.Parts[i].ReqOCH, eRoundMode.NEAREST).ToString()));

		//			itmX.SubItems.Insert(7, new ListViewItem.ListViewSubItem(null, _obstaclesPage3.Parts[i].Dist.ToString("0.0")));
		//			itmX.SubItems.Insert(8, new ListViewItem.ListViewSubItem(null, _obstaclesPage3.Parts[i].CLShift.ToString("0.0")));

		//			itmX.SubItems.Insert(9, new ListViewItem.ListViewSubItem(null, GlobalVars.SurfaceNames[_obstaclesPage3.Parts[i].Plane]));
		//			itmX.SubItems.Insert(10, new ListViewItem.ListViewSubItem(null, APVAreaNames[_obstaclesPage3.Parts[i].Flags]));
		//		}
		//		else
		//		{
		//			itmX = listView3.Items[i];

		//			itmX.Text = _obstaclesPage3.Obstacles[_obstaclesPage3.Parts[i].Owner].TypeName;
		//			itmX.SubItems[1].Text = _obstaclesPage3.Obstacles[_obstaclesPage3.Parts[i].Owner].UnicalName;

		//			itmX.SubItems[2].Text = GlobalVars.unitConverter.HeightToDisplayUnits(_obstaclesPage3.Parts[i].hSurface, eRoundMode.NEAREST).ToString();
		//			itmX.SubItems[3].Text = GlobalVars.unitConverter.HeightToDisplayUnits(_obstaclesPage3.Parts[i].Height, eRoundMode.NEAREST).ToString();
		//			itmX.SubItems[4].Text = GlobalVars.unitConverter.HeightToDisplayUnits(_obstaclesPage3.Parts[i].hPenet, eRoundMode.NEAREST).ToString();

		//			itmX.SubItems[5].Text = _obstaclesPage3.Parts[i].fSecCoeff.ToString("0.000");
		//			itmX.SubItems[6].Text = GlobalVars.unitConverter.HeightToDisplayUnits(_obstaclesPage3.Parts[i].ReqOCH, eRoundMode.NEAREST).ToString();

		//			itmX.SubItems[7].Text = _obstaclesPage3.Parts[i].Dist.ToString("0.0");
		//			itmX.SubItems[8].Text = _obstaclesPage3.Parts[i].CLShift.ToString("0.0");

		//			itmX.SubItems[9].Text = GlobalVars.SurfaceNames[_obstaclesPage3.Parts[i].Plane];
		//			itmX.SubItems[10].Text = APVAreaNames[_obstaclesPage3.Parts[i].Flags];
		//		}

		//		itmX.SubItems[4].Font = new System.Drawing.Font(itmX.SubItems[4].Font, System.Drawing.FontStyle.Bold);
		//	}

		//	listView3.EndUpdate();
		//}

		public void FillAPVList(ObstacleContainer Obstacles)
		{
			int m = Obstacles.Obstacles.Length;
			int n = Obstacles.Parts.Length;

			_obstaclesPage1.Obstacles = new Obstacle[m];
			_obstaclesPage1.Parts = new ObstacleData[n];

			Array.Copy(Obstacles.Obstacles, _obstaclesPage1.Obstacles, m);
			Array.Copy(Obstacles.Parts, _obstaclesPage1.Parts, n);

			int sortByField = Page01SortIndex;

			if (_sortF1 != 0)
			{
				sortByField = Math.Abs(_sortF1) - 1;
				_sortF1 = -_sortF1;
			}

			UpdateListView01(sortByField, true);

			listView1.Tag = _obstaclesPage1;

			SetTabVisible(0, true);
			if (_reportBtn.Checked && !Visible)
				this.Show(GlobalVars.Win32Window);
		}

		internal void FillInterApprList(ObstacleContainer Obstacles)
		{
			int m = Obstacles.Obstacles.Length;
			int n = Obstacles.Parts.Length;

			_obstaclesPage2.Obstacles = new Obstacle[m];
			_obstaclesPage2.Parts = new ObstacleData[n];

			Array.Copy(Obstacles.Obstacles, _obstaclesPage2.Obstacles, m);
			Array.Copy(Obstacles.Parts, _obstaclesPage2.Parts, n);

			int sortByField = Page02SortIndex;

			if (_sortF2 != 0)
			{
				sortByField = Math.Abs(_sortF2) - 1;
				_sortF2 = -_sortF2;
			}

			UpdateListView02(sortByField, true);

			listView2.Tag = _obstaclesPage2;

			SetTabVisible(1, true);
			if (_reportBtn.Checked && !Visible)
				this.Show(GlobalVars.Win32Window);
		}

		//internal void FillFMASList(ObstacleContainer Obstacles)
		//{
		//	int m = Obstacles.Obstacles.Length;
		//	int n = Obstacles.Parts.Length;

		//	_obstaclesPage3.Obstacles = new Obstacle[m];
		//	_obstaclesPage3.Parts = new ObstacleData[n];

		//	Array.Copy(Obstacles.Obstacles, _obstaclesPage3.Obstacles, m);
		//	Array.Copy(Obstacles.Parts, _obstaclesPage3.Parts, n);

		//	int sortByField = Page03SortIndex;

		//	if (_sortF3 != 0)
		//	{
		//		sortByField = Math.Abs(_sortF3) - 1;
		//		_sortF3 = -_sortF3;
		//	}

		//	UpdateListView03(sortByField, true);

		//	listView3.Tag = _obstaclesPage3;

		//	SetTabVisible(2, true);
		//	if (_reportBtn.Checked && !Visible)
		//		this.Show(GlobalVars.Win32Window);
		//}

		private void listViews_SelectedIndexChanged(object sender, EventArgs e)
		{
			GlobalVars.gAranGraphics.SafeDeleteGraphic(_pointElem);
			GlobalVars.gAranGraphics.SafeDeleteGraphic(_geomElem);
			ListView lListView = (ListView)sender;

			if (!this.Visible || lListView.Tag == null || lListView.SelectedItems.Count == 0)
				return;

			ListViewItem Item = lListView.SelectedItems[0];

			if (Item == null)
				return;

			ObstacleContainer pageObstacles = (ObstacleContainer)lListView.Tag;

			if (pageObstacles.Parts.Length == 0)
				return;

			int Index = pageObstacles.Parts[Item.Index].Owner;

			Geometry pGeometry = pageObstacles.Obstacles[Index].pGeomPrj;
			Point pPtTmp = pageObstacles.Parts[Item.Index].pPtPrj;

			if (pGeometry.Type == GeometryType.MultiLineString)
				_geomElem = GlobalVars.gAranGraphics.DrawMultiLineString((MultiLineString)pGeometry, 2, 255);
			else if (pGeometry.Type == GeometryType.MultiPolygon)
				_geomElem = GlobalVars.gAranGraphics.DrawMultiPolygon((MultiPolygon)pGeometry, AranEnvironment.Symbols.eFillStyle.sfsDiagonalCross, 255);

			_pointElem = GlobalVars.gAranGraphics.DrawPointWithText(pPtTmp, pageObstacles.Obstacles[Index].UnicalName, 255);
		}

		private void listView1_ColumnClick(object sender, ColumnClickEventArgs e)
		{
			UpdateListView01(e.Column);

			if (_reportBtn.Checked)
				listViews_SelectedIndexChanged(listView1, new System.EventArgs());
		}

		private void listView2_ColumnClick(object sender, ColumnClickEventArgs e)
		{
			UpdateListView02(e.Column);

			if (_reportBtn.Checked)
				listViews_SelectedIndexChanged(listView2, new System.EventArgs());
		}

		private void listView3_ColumnClick(object sender, ColumnClickEventArgs e)
		{
			//UpdateListView03(e.Column);

			//if (_reportBtn.Checked)
			//	listViews_SelectedIndexChanged(listView3, new System.EventArgs());
		}

		private void helpBtn_Click(object sender, EventArgs e)
		{
			//NativeMethods.HtmlHelp(0, GlobalVars.HelpFile, GlobalVars.HH_HELP_CONTEXT, _helpContextID);
		}

		private void saveButton_Click(object sender, EventArgs e)
		{
			if (SaveDialog1.ShowDialog() != System.Windows.Forms.DialogResult.OK)
				return;

			TabPage tapPage = mainTabControl.SelectedTab;
			ListView lListView = null;						// = (ListView)tapPage.Controls[0];

			switch (mainTabControl.SelectedIndex)
			{
				case 0:
					lListView = listView1;
					break;
				case 1:
					lListView = listView2;
					break;
			}

			if (SaveDialog1.FilterIndex == 1)
			{
				ReportFile report = new ReportFile();
				string fileName = Path.ChangeExtension(SaveDialog1.FileName, "");
				report.OpenFile(fileName, "");
				report.WriteHeader();
				report.WriteTab(lListView, tapPage.Text);
				report.CloseFile();
				return;
			}

			ReportFile.WriteTabToTXT(lListView, SaveDialog1.FileName, tapPage.Text);

			//StreamWriter stwr = new StreamWriter(SaveDialog1.FileName);

			//stwr.WriteLine(Text);
			//stwr.WriteLine("\t" + tapPage.Text);
			//stwr.WriteLine();

			//int n = lListView.Columns.Count;
			//string[] HeadersText = new string[n];
			//int[] HeadersLen = new int[n];

			//int i, j, m, maxLen = 0;

			//for (i = 0; i < n; i++)
			//{
			//	ColumnHeader hdrX = lListView.Columns[i];
			//	HeadersText[i] = @"""" + hdrX.Text + @"""";
			//	HeadersLen[i] = HeadersText[i].Length;
			//	if (HeadersLen[i] > maxLen)
			//		maxLen = HeadersLen[i];
			//}

			//string StrOut = "", tmpStr;

			//for (i = 0; i < n; i++)
			//{
			//	j = maxLen - HeadersLen[i];
			//	m = j / 2;
			//	if (i < n)
			//		tmpStr = System.String.Empty.PadLeft(m) + "|";
			//	else
			//		tmpStr = "";

			//	StrOut = StrOut + System.String.Empty.PadLeft(j - m) + HeadersText[i] + tmpStr;
			//}

			//stwr.WriteLine(StrOut);
			//StrOut = "";
			//tmpStr = new string('-', maxLen) + "+";
			//for (i = 1; i < n; i++)
			//	StrOut = StrOut + tmpStr;

			//StrOut = StrOut + new string('-', maxLen);
			//stwr.WriteLine(StrOut);

			//m = lListView.Items.Count;
			//for (i = 0; i < m; i++)
			//{
			//	ListViewItem itmX = lListView.Items[i];

			//	int TmpLen = itmX.Text.Length;
			//	if (TmpLen > maxLen)
			//		StrOut = itmX.Text.Substring(0, maxLen - 1) + "*";
			//	else
			//		StrOut = System.String.Empty.PadLeft(maxLen - TmpLen) + itmX.Text;

			//	for (j = 1; j < n; j++)
			//	{
			//		tmpStr = itmX.SubItems[j].Text;

			//		TmpLen = tmpStr.Length;

			//		if (TmpLen > maxLen)
			//			tmpStr = tmpStr.Substring(0, maxLen - 1) + "*";
			//		else if (j < n - 1 || TmpLen > 0)
			//			tmpStr = System.String.Empty.PadLeft(maxLen - TmpLen) + tmpStr;

			//		StrOut = StrOut + "|" + tmpStr;
			//	}
			//	stwr.WriteLine(StrOut);
			//}
			//stwr.Flush();
			//stwr.Dispose();
		}

		private void closeButton_Click(object sender, EventArgs e)
		{
			GlobalVars.gAranGraphics.SafeDeleteGraphic(_pointElem);
			GlobalVars.gAranGraphics.SafeDeleteGraphic(_geomElem);
			//GlobalVars.gAranGraphics.Refresh();

			_reportBtn.Checked = false;
			Hide();
		}
	}
}
