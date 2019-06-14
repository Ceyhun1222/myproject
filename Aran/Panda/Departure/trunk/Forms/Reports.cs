using Aran.PANDA.Departure.Properties;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geometry;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Aran.PANDA.Departure
{
	[System.Runtime.InteropServices.ComVisible(false)]
	public partial class CReports : Form
	{
		#region declerations
		const int Page01SortIndex = 3;
		const int Page02SortIndex = 2;
		const int Page03SortIndex = 2;
		const int Page04SortIndexH = 4;
		const int Page04SortIndexF = 3;
		const int Page05SortIndex = 2;
		const int Page06SortIndex = 3;

		private ObstacleContainer _obstaclesPage1;
		private ObstacleContainer _obstaclesPage2;
		private ObstacleContainer _obstaclesPage3;
		private ObstacleContainer _obstaclesPage4;
		private ObstacleContainer _obstaclesPage5;
		private ObstacleContainer _obstaclesPage6;
		//private ObstacleContainer[] _obstacleContainers;

		private int _page1Procedure;
		private long _page1DetID;
		private long _page3IndexID;
		private bool _page4AtHeight;

		private IElement _pointElem;
		private IElement _geomElem;

		private CheckBox _reportBtn;

		private TabPage[] _tapPages;
		private bool[] _pageVisible;

		private int _helpContextID;
		private TabPage _previousTab;
		private ListViewColumnSorter _columnSorter;
		private ObstacleComparer _arrayComparer; 

		#endregion

		public CReports()
		{
			InitializeComponent();

			_obstaclesPage1.Obstacles = new Obstacle[0];
			_obstaclesPage2.Obstacles = new Obstacle[0];
			_obstaclesPage3.Obstacles = new Obstacle[0];
			_obstaclesPage4.Obstacles = new Obstacle[0];
			_obstaclesPage5.Obstacles = new Obstacle[0];
			_obstaclesPage6.Obstacles = new Obstacle[0];

			_obstaclesPage1.Parts = new ObstacleData[0];
			_obstaclesPage2.Parts = new ObstacleData[0];
			_obstaclesPage3.Parts = new ObstacleData[0];
			_obstaclesPage4.Parts = new ObstacleData[0];
			_obstaclesPage5.Parts = new ObstacleData[0];
			_obstaclesPage6.Parts = new ObstacleData[0];

			this.Text = Resources.str40105;

			saveButton.Text = Resources.str00007;
			closeButton.Text = Resources.str00008;

			label01.Text = "";
			//Label02.Text = Resources.str15087 + (PANS_OPS_DataBase.dpPDG_Nom.Value * 100.0).ToString() + Resources.str15088;

			_previousTab = null;
			_tapPages = new TabPage[] { tabPage0, tabPage1, tabPage2, tabPage3, tabPage4, tabPage5 };

			_pageVisible = new bool[_tapPages.Length];
			mainTabControl.Controls.Clear();

			_tapPages[0].Text = Resources.str00400;
			_tapPages[1].Text = Resources.str00410;
			_tapPages[2].Text = Resources.str00420;
			_tapPages[3].Text = Resources.str00430;
			_tapPages[4].Text = Resources.str00440;
			_tapPages[5].Text = Resources.str00450;

			listView1.Columns[0].Text = Resources.str40001;
			listView1.Columns[1].Text = Resources.str40000;
			listView1.Columns[2].Text = Resources.str40007;
			listView1.Columns[3].Text = Resources.str40011;
			listView1.Columns[4].Text = Resources.str40031 + ", " + GlobalVars.HeightConverter[GlobalVars.HeightUnit].Unit;
			listView1.Columns[5].Text = Resources.str40002 + ", " + GlobalVars.DistanceConverter[GlobalVars.DistanceUnit].Unit;
			listView1.Columns[6].Text = Resources.str40003 + ", " + GlobalVars.DistanceConverter[GlobalVars.DistanceUnit].Unit;
			listView1.Columns[7].Text = Resources.str40005 + ", " + GlobalVars.HeightConverter[GlobalVars.HeightUnit].Unit;
			listView1.Columns[8].Text = Resources.str40008 + ", " + GlobalVars.HeightConverter[GlobalVars.HeightUnit].Unit;
			listView1.Columns[9].Text = Resources.str40009 + ", " + GlobalVars.HeightConverter[GlobalVars.HeightUnit].Unit;
			listView1.Columns[10].Text = Resources.str40012;
			listView1.Columns[11].Text = Resources.str40013 + ", " + GlobalVars.DistanceConverter[GlobalVars.DistanceUnit].Unit;
			listView1.Columns[12].Text = Resources.str40014 + ", " + GlobalVars.HeightConverter[GlobalVars.HeightUnit].Unit;
			listView1.Columns[13].Text = Resources.str40010;
			listView1.Columns[14].Text = Resources.str40004 + ", " + GlobalVars.HeightConverter[GlobalVars.HeightUnit].Unit;
			listView1.Columns[15].Text = Resources.str40006 + ", " + GlobalVars.HeightConverter[GlobalVars.HeightUnit].Unit;

			listView2.Columns[0].Text = Resources.str40001;
			listView2.Columns[1].Text = Resources.str40000;
			listView2.Columns[2].Text = Resources.str40027 + ", " + GlobalVars.HeightConverter[GlobalVars.HeightUnit].Unit;
			listView2.Columns[3].Text = Resources.str40005 + ", " + GlobalVars.HeightConverter[GlobalVars.HeightUnit].Unit;
			listView2.Columns[4].Text = Resources.str40032 + ", " + GlobalVars.DistanceConverter[GlobalVars.DistanceUnit].Unit;
			listView2.Columns[5].Text = Resources.str40033 + ", " + GlobalVars.DistanceConverter[GlobalVars.DistanceUnit].Unit;
			listView2.Columns[6].Text = Resources.str40103 + ", " + GlobalVars.HeightConverter[GlobalVars.HeightUnit].Unit;
			listView2.Columns[7].Text = Resources.str40104 + ", " + GlobalVars.HeightConverter[GlobalVars.HeightUnit].Unit;
			listView2.Columns[8].Text = Resources.str40026 + ", " + GlobalVars.HeightConverter[GlobalVars.HeightUnit].Unit;
			listView2.Columns[9].Text = Resources.str40004 + ", " + GlobalVars.HeightConverter[GlobalVars.HeightUnit].Unit;
			listView2.Columns[10].Text = Resources.str40006 + ", " + GlobalVars.HeightConverter[GlobalVars.HeightUnit].Unit;

			listView3.Columns[0].Text = Resources.str40001;
			listView3.Columns[1].Text = Resources.str40000;
			listView3.Columns[2].Text = Resources.str40027 + ", " + GlobalVars.HeightConverter[GlobalVars.HeightUnit].Unit;
			listView3.Columns[3].Text = Resources.str40005 + ", " + GlobalVars.HeightConverter[GlobalVars.HeightUnit].Unit;
			listView3.Columns[4].Text = Resources.str40032 + ", " + GlobalVars.DistanceConverter[GlobalVars.DistanceUnit].Unit;
			listView3.Columns[5].Text = Resources.str40033 + ", " + GlobalVars.DistanceConverter[GlobalVars.DistanceUnit].Unit;
			listView3.Columns[6].Text = Resources.str40103 + ", " + GlobalVars.HeightConverter[GlobalVars.HeightUnit].Unit;
			listView3.Columns[7].Text = Resources.str40104 + ", " + GlobalVars.HeightConverter[GlobalVars.HeightUnit].Unit;
			listView3.Columns[8].Text = Resources.str40026 + ", " + GlobalVars.HeightConverter[GlobalVars.HeightUnit].Unit;
			listView3.Columns[9].Text = Resources.str40004 + ", " + GlobalVars.HeightConverter[GlobalVars.HeightUnit].Unit;
			listView3.Columns[10].Text = Resources.str40006 + ", " + GlobalVars.HeightConverter[GlobalVars.HeightUnit].Unit;

			listView4.Columns[0].Text = Resources.str40001;
			listView4.Columns[1].Text = Resources.str40000;
			listView4.Columns[2].Text = Resources.str40007;
			listView4.Columns[3].Text = Resources.str40011;
			listView4.Columns[4].Text = Resources.str40031 + ", " + GlobalVars.HeightConverter[GlobalVars.HeightUnit].Unit;
			listView4.Columns[5].Text = Resources.str40002 + ", " + GlobalVars.DistanceConverter[GlobalVars.DistanceUnit].Unit;
			listView4.Columns[6].Text = Resources.str40003 + ", " + GlobalVars.DistanceConverter[GlobalVars.DistanceUnit].Unit;
			listView4.Columns[7].Text = Resources.str40005 + ", " + GlobalVars.HeightConverter[GlobalVars.HeightUnit].Unit;
			listView4.Columns[8].Text = Resources.str40008 + ", " + GlobalVars.HeightConverter[GlobalVars.HeightUnit].Unit;
			listView4.Columns[9].Text = Resources.str40009 + ", " + GlobalVars.HeightConverter[GlobalVars.HeightUnit].Unit;
			listView4.Columns[10].Text = Resources.str40012;
			listView4.Columns[11].Text = Resources.str40010;
			listView4.Columns[12].Text = Resources.str40004 + ", " + GlobalVars.HeightConverter[GlobalVars.HeightUnit].Unit;
			listView4.Columns[13].Text = Resources.str40006 + ", " + GlobalVars.HeightConverter[GlobalVars.HeightUnit].Unit;

			listView5.Columns[0].Text = Resources.str40001;
			listView5.Columns[1].Text = Resources.str40000;
			listView5.Columns[2].Text = Resources.str40027 + ", " + GlobalVars.HeightConverter[GlobalVars.HeightUnit].Unit;
			listView5.Columns[3].Text = Resources.str40005 + ", " + GlobalVars.HeightConverter[GlobalVars.HeightUnit].Unit;
			listView5.Columns[4].Text = Resources.str40032 + ", " + GlobalVars.DistanceConverter[GlobalVars.DistanceUnit].Unit;
			listView5.Columns[5].Text = Resources.str40033 + ", " + GlobalVars.DistanceConverter[GlobalVars.DistanceUnit].Unit;
			listView5.Columns[6].Text = Resources.str40034 + ", " + GlobalVars.HeightConverter[GlobalVars.HeightUnit].Unit;
			listView5.Columns[7].Text = Resources.str40004 + ", " + GlobalVars.HeightConverter[GlobalVars.HeightUnit].Unit;
			listView5.Columns[8].Text = Resources.str40006 + ", " + GlobalVars.HeightConverter[GlobalVars.HeightUnit].Unit;

			listView6.Columns[0].Text = Resources.str40001;
			listView6.Columns[1].Text = Resources.str40000;
			listView6.Columns[2].Text = Resources.str40028;
			listView6.Columns[3].Text = Resources.str40029 + ", " + GlobalVars.HeightConverter[GlobalVars.HeightUnit].Unit;
			listView6.Columns[4].Text = Resources.str40005 + ", " + GlobalVars.HeightConverter[GlobalVars.HeightUnit].Unit;
			listView6.Columns[5].Text = Resources.str40008 + ", " + GlobalVars.HeightConverter[GlobalVars.HeightUnit].Unit;
			listView6.Columns[6].Text = Resources.str40009 + ", " + GlobalVars.HeightConverter[GlobalVars.HeightUnit].Unit;
			listView6.Columns[7].Text = Resources.str40026 + ", " + GlobalVars.HeightConverter[GlobalVars.HeightUnit].Unit;


			//listView1.Tag = new ListViewsTag { obstacles = _obstaclesPage1, SortField = 0 };
			//listView2.Tag = new ListViewsTag { obstacles = _obstaclesPage2, SortField = 0 };
			//listView3.Tag = new ListViewsTag { obstacles = _obstaclesPage3, SortField = 0 };
			//listView4.Tag = new ListViewsTag { obstacles = _obstaclesPage4, SortField = 0 };
			//listView5.Tag = new ListViewsTag { obstacles = _obstaclesPage5, SortField = 0 };
			//listView6.Tag = new ListViewsTag { obstacles = _obstaclesPage6, SortField = 0 };

			_arrayComparer = new ObstacleComparer();

			listView1.Tag = listView2.Tag = listView3.Tag = listView4.Tag = listView5.Tag = listView6.Tag = 0;

			_columnSorter = new ListViewColumnSorter();
			listView1.ListViewItemSorter = _columnSorter;
			listView2.ListViewItemSorter = _columnSorter;
			listView3.ListViewItemSorter = _columnSorter;
			listView4.ListViewItemSorter = _columnSorter;
			listView5.ListViewItemSorter = _columnSorter;
			listView6.ListViewItemSorter = _columnSorter;
		}

		private void CReportsFrm_KeyUp(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.F1)
			{
				NativeMethods.HtmlHelp(0, GlobalVars.HelpFile, GlobalVars.HH_HELP_CONTEXT, _helpContextID);
				e.Handled = true;
			}
		}

		private void CReportsFrm_FormClosing(object eventSender, FormClosingEventArgs eventArgs)
		{
			Functions.DeleteGraphicsElement(_pointElem);
			Functions.DeleteGraphicsElement(_geomElem);

			if (eventArgs.CloseReason == CloseReason.UserClosing)
			{
				eventArgs.Cancel = true;
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

		protected override void WndProc(ref System.Windows.Forms.Message m)
		{
			base.WndProc(ref m);

			if ((m.Msg == GlobalVars.WM_SYSCOMMAND) && ((int)m.WParam == GlobalVars.SYSMENU_ABOUT_ID))
			{
				AboutForm about = new AboutForm();
				about.ShowDialog(this);
				about = null;
			}
		}

		public void SetBtn(CheckBox Btn, int HelpContext)
		{
			_reportBtn = Btn;
			_helpContextID = HelpContext;
		}

		public string GetTabPageText(int index)
		{
			return _tapPages[index].Text;
		}

		public void SetTabVisible(int index, bool visible)
		{
			int x;

			if (visible)
				mainTabControl.Controls.Clear();

			if (index < 0)
			{
				for (int i = 0; i < _tapPages.Length; i++)
				{
					_pageVisible[i] = visible;

					if (visible)
						mainTabControl.Controls.Add(_tapPages[i]);
					else if (mainTabControl.Controls.Contains(_tapPages[i]))
						mainTabControl.Controls.Remove(_tapPages[i]);
				}
			}
			else if (index < _tapPages.Length)
			{
				_pageVisible[index] = visible;

				if (visible)
				{
					for (int i = 0; i < _tapPages.Length; i++)
						if (_pageVisible[i])
							mainTabControl.Controls.Add(_tapPages[i]);

					x = mainTabControl.Controls.GetChildIndex(_tapPages[index]);
					mainTabControl.SelectedIndex = x;
				}
				else if (mainTabControl.Controls.Contains(_tapPages[index]))
					mainTabControl.Controls.Remove(_tapPages[index]);
			}
		}

		static IDictionary<int, int> GetUnicalObstales(ObstacleContainer obstacleList)
		{
			IDictionary<int, int> result = new Dictionary<int, int>();

			int i, n = obstacleList.Parts.Length;

			for (i = 0; i < n; i++)
			{
				int owner = obstacleList.Parts[i].Owner;

				if (!result.ContainsKey(owner))
					result.Add(owner, i);
			}

			return result;
		}

		public void SortForSave()
		{
			int currIndex, prevIndex, sortF;

			//Page 1 ========================================================================================
			sortF = (int)listView1.Tag;

			currIndex = Page01SortIndex;
			prevIndex = Math.Abs(sortF) - 1;

			if (currIndex != prevIndex)
				if (prevIndex >= 0)
					listView1.Columns[prevIndex].ImageIndex = 2;

			listView1.Tag = currIndex + 1;
			UpdateListView01(currIndex);
			//ListView1_SelectedIndexChanged(listView1, new EventArgs());
			//Page 2 ========================================================================================
			sortF = (int)listView2.Tag;
			currIndex = Page02SortIndex;
			prevIndex = Math.Abs(sortF) - 1;

			if (currIndex != prevIndex)
				if (prevIndex >= 0) listView2.Columns[prevIndex].ImageIndex = 2;

			listView2.Tag = currIndex + 1;
			UpdateListView02(currIndex);

			//Page 3 ========================================================================================
			sortF = (int)listView3.Tag;
			currIndex = Page03SortIndex;
			prevIndex = Math.Abs(sortF) - 1;

			if (currIndex != prevIndex)
				if (prevIndex >= 0)
					listView3.Columns[prevIndex].ImageIndex = 2;

			listView3.Tag = currIndex + 1;
			UpdateListView03(currIndex);
			//ListView3_SelectedIndexChanged(listView3, new EventArgs())
			//Page 4 ========================================================================================
			sortF = (int)listView4.Tag;
			currIndex = Page04SortIndexF;
			if (_page4AtHeight)
				currIndex = Page04SortIndexH;

			prevIndex = Math.Abs(sortF) - 1;

			if (currIndex != prevIndex)
				if (prevIndex >= 0) listView4.Columns[prevIndex].ImageIndex = 2;

			listView4.Tag = currIndex + 1;
			UpdateListView04(currIndex);

			//Page 5 ========================================================================================
			sortF = (int)listView5.Tag;
			currIndex = Page05SortIndex;
			prevIndex = Math.Abs(sortF) - 1;

			if (currIndex != prevIndex)
				if (prevIndex >= 0) listView5.Columns[prevIndex].ImageIndex = 2;

			listView5.Tag = currIndex + 1;
			UpdateListView05(currIndex);

			//Page 6 ========================================================================================
			sortF = (int)listView6.Tag;
			currIndex = Page06SortIndex;
			prevIndex = Math.Abs(sortF) - 1;

			if (currIndex != prevIndex)
				if (prevIndex >= 0) listView6.Columns[prevIndex].ImageIndex = 2;

			listView6.Tag = currIndex + 1;
			UpdateListView06(currIndex);
			//ListView6_SelectedIndexChanged(listView6, new EventArgs())
		}

		private void UpdateListView01(int sortByField)     //, bool preClear = false)
		{
			double NomPDG = 100.0 * PANS_OPS_DataBase.dpPDG_Nom.Value;
			double displayPDG;
			int i, n = _obstaclesPage1.Parts.Length;

			if (sortByField >= 0)
			{
				int sortF = (int)listView1.Tag;
				if (Math.Abs(sortF) - 1 == sortByField)
					sortF = -sortF;

				//if (preClear || (Math.Abs(_sortF1) - 1 != sortByField))
				//if ((Math.Abs(_sortF1) - 1 != sortByField))
				//{
				else //if (Math.Abs(_sortF1) - 1 != sortByField)
				{
					if (sortF != 0)
						listView1.Columns[Math.Abs(sortF) - 1].ImageIndex = 2;

					sortF = -sortByField - 1;
				}

				listView1.Tag = sortF;

				if (sortByField <= 1 || sortByField == 13 || sortByField == 16)
					for (i = 0; i < n; i++)
						switch (sortByField)
						{
							case 0:
								_obstaclesPage1.Parts[i].sSort = _obstaclesPage1.Obstacles[_obstaclesPage1.Parts[i].Owner].TypeName; break;
							case 1:
								_obstaclesPage1.Parts[i].sSort = _obstaclesPage1.Obstacles[_obstaclesPage1.Parts[i].Owner].UnicalName; break;
							case 13:
								if (_obstaclesPage1.Parts[i].PDG <= PANS_OPS_DataBase.dpPDG_Nom.Value)
									_obstaclesPage1.Parts[i].sSort = "";
								else if (_obstaclesPage1.Parts[i].Ignored)
									_obstaclesPage1.Parts[i].sSort = Resources.str40030;
								else
									_obstaclesPage1.Parts[i].sSort = Resources.str40010;
								break;
							case 16:
								if (_page1Procedure == 3)
								{
									if (_obstaclesPage1.Parts[i].Prima)
										_obstaclesPage1.Parts[i].sSort = Resources.str15080;
									else
										_obstaclesPage1.Parts[i].sSort = Resources.str15081;
								}
								break;
						}
				else
					for (i = 0; i < n; i++)
						switch (sortByField)
						{
							case 2:
								_obstaclesPage1.Parts[i].fSort = _obstaclesPage1.Parts[i].PDGToTop; break;
							case 3:
								_obstaclesPage1.Parts[i].fSort = _obstaclesPage1.Parts[i].PDG; break;
							case 4:
								_obstaclesPage1.Parts[i].fSort = _obstaclesPage1.Parts[i].ReqTNH; break;
							case 5:
								_obstaclesPage1.Parts[i].fSort = _obstaclesPage1.Parts[i].CLShift; break;
							case 6:
								_obstaclesPage1.Parts[i].fSort = _obstaclesPage1.Parts[i].Dist; break;
							case 7:
								_obstaclesPage1.Parts[i].fSort = _obstaclesPage1.Parts[i].Height; break;
							case 8:
								_obstaclesPage1.Parts[i].fSort = _obstaclesPage1.Parts[i].MOC; break;
							case 9:
								_obstaclesPage1.Parts[i].fSort = _obstaclesPage1.Parts[i].hPenet; break;
							case 10:
								displayPDG = Math.Round(100.0 * _obstaclesPage1.Parts[i].PDG + 0.04999, 1);
								if (displayPDG < NomPDG)
									displayPDG = NomPDG;
								_obstaclesPage1.Parts[i].fSort = displayPDG;
								break;
							case 11:
								_obstaclesPage1.Parts[i].fSort = _obstaclesPage1.Parts[i].NomPDGDist; break;
							case 12:
								_obstaclesPage1.Parts[i].fSort = _obstaclesPage1.Parts[i].CourseAdjust; break;
							case 14:
								_obstaclesPage1.Parts[i].fSort = _obstaclesPage1.Obstacles[_obstaclesPage1.Parts[i].Owner].HorAccuracy; break;
							case 15:
								_obstaclesPage1.Parts[i].fSort = _obstaclesPage1.Obstacles[_obstaclesPage1.Parts[i].Owner].VertAccuracy; break;
						}
				//}

				//ObstacleComparer comparer = new ObstacleComparer(sortF, sortByField <= 1 || sortByField == 13 || sortByField == 16);
				//Array.Sort(_obstaclesPage1.Parts, comparer);

				_arrayComparer.SortOrder = sortF;
				_arrayComparer.TextField = sortByField <= 1 || sortByField == 13 || sortByField == 16;
				Array.Sort(_obstaclesPage1.Parts, _arrayComparer);

				//if (sortByField <= 1 || sortByField == 13 || sortByField == 16)
				//{
				//	if (sortF > 0)
				//		Functions.shall_SortsSort(_obstaclesPage1.Parts);
				//	else
				//		Functions.shall_SortsSortD(_obstaclesPage1.Parts);
				//}
				//else if (sortF > 0)
				//	Functions.shall_SortfSort(_obstaclesPage1.Parts);
				//else
				//	Functions.shall_SortfSortD(_obstaclesPage1.Parts);

				if (sortF > 0)
					listView1.Columns[sortByField].ImageIndex = 0;
				else
					listView1.Columns[sortByField].ImageIndex = 1;
			}

			IDictionary<int, int> unicalObstacleList = GetUnicalObstales(_obstaclesPage1);
			listView1.BeginUpdate();
			//if (preClear)
			listView1.Items.Clear();

			//for (i = 0; i < n; i++)
			foreach (KeyValuePair<int, int> item in unicalObstacleList)
			{
				ListViewItem itmX;

				//if (preClear)
				//{
				itmX = listView1.Items.Add(_obstaclesPage1.Obstacles[item.Key].TypeName);
				itmX.SubItems.Insert(1, new ListViewItem.ListViewSubItem(null, _obstaclesPage1.Obstacles[item.Key].UnicalName));

				i = item.Value;
				itmX.Tag = i;

				displayPDG = Math.Round(100.0 * _obstaclesPage1.Parts[i].PDG + 0.04999, 1);
				if (displayPDG < NomPDG)
					displayPDG = NomPDG;

				string str13;

				if (_obstaclesPage1.Parts[i].PDG <= PANS_OPS_DataBase.dpPDG_Nom.Value)
					str13 = Resources.str39014;
				else if (_obstaclesPage1.Parts[i].Ignored)
					str13 = Resources.str40030;
				else
					str13 = Resources.str39015;

				itmX.SubItems.Insert(2, new ListViewItem.ListViewSubItem(null, Math.Round(100.0 * _obstaclesPage1.Parts[i].PDGToTop, 3).ToString()));
				itmX.SubItems.Insert(3, new ListViewItem.ListViewSubItem(null, Math.Round(100.0 * _obstaclesPage1.Parts[i].PDG, 3).ToString()));
				itmX.SubItems.Insert(4, new ListViewItem.ListViewSubItem(null, Functions.ConvertHeight(_obstaclesPage1.Parts[i].ReqTNH, eRoundMode.CEIL).ToString()));
				itmX.SubItems.Insert(5, new ListViewItem.ListViewSubItem(null, Functions.ConvertDistance(_obstaclesPage1.Parts[i].CLShift).ToString()));
				itmX.SubItems.Insert(6, new ListViewItem.ListViewSubItem(null, Functions.ConvertDistance(_obstaclesPage1.Parts[i].Dist).ToString()));
				itmX.SubItems.Insert(7, new ListViewItem.ListViewSubItem(null, Functions.ConvertHeight(_obstaclesPage1.Parts[i].Height).ToString()));
				itmX.SubItems.Insert(8, new ListViewItem.ListViewSubItem(null, Functions.ConvertHeight(_obstaclesPage1.Parts[i].MOC).ToString()));
				itmX.SubItems.Insert(9, new ListViewItem.ListViewSubItem(null, Functions.ConvertHeight(_obstaclesPage1.Parts[i].hPenet, eRoundMode.CEIL).ToString()));
				itmX.SubItems.Insert(10, new ListViewItem.ListViewSubItem(null, displayPDG.ToString()));
				itmX.SubItems.Insert(11, new ListViewItem.ListViewSubItem(null, Functions.ConvertDistance(_obstaclesPage1.Parts[i].NomPDGDist).ToString()));
				itmX.SubItems.Insert(12, new ListViewItem.ListViewSubItem(null, Functions.ConvertHeight(_obstaclesPage1.Parts[i].CourseAdjust, eRoundMode.CEIL).ToString()));
				itmX.SubItems.Insert(13, new ListViewItem.ListViewSubItem(null, str13));
				itmX.SubItems.Insert(14, new ListViewItem.ListViewSubItem(null, Functions.ConvertHeight(_obstaclesPage1.Obstacles[item.Key].HorAccuracy).ToString()));
				itmX.SubItems.Insert(15, new ListViewItem.ListViewSubItem(null, Functions.ConvertHeight(_obstaclesPage1.Obstacles[item.Key].VertAccuracy).ToString()));

				if (_page1Procedure == 3)
				{
					if (_obstaclesPage1.Parts[i].Prima)
						itmX.SubItems.Insert(16, new ListViewItem.ListViewSubItem(null, Resources.str15080));
					else
						itmX.SubItems.Insert(16, new ListViewItem.ListViewSubItem(null, Resources.str15081));
				}

				//}
				//else
				//{
				//	itmX = listView1.Items[i];

				//	itmX.Text = _obstaclesPage1.Obstacles[_obstaclesPage1.Parts[i].Owner].TypeName;
				//	itmX.SubItems[1].Text = _obstaclesPage1.Obstacles[_obstaclesPage1.Parts[i].Owner].UnicalName;

				//	itmX.SubItems[2].Text = Math.Round(100.0 * _obstaclesPage1.Parts[i].PDGToTop, 3).ToString();
				//	itmX.SubItems[3].Text = Math.Round(100.0 * _obstaclesPage1.Parts[i].PDG, 3).ToString();

				//	itmX.SubItems[4].Text = Functions.ConvertHeight(_obstaclesPage1.Parts[i].ReqTNH, eRoundMode.CEIL).ToString();
				//	itmX.SubItems[5].Text = Functions.ConvertDistance(_obstaclesPage1.Parts[i].CLShift).ToString();
				//	itmX.SubItems[6].Text = Functions.ConvertDistance(_obstaclesPage1.Parts[i].Dist).ToString();
				//	itmX.SubItems[7].Text = Functions.ConvertHeight(_obstaclesPage1.Parts[i].Height).ToString();
				//	itmX.SubItems[8].Text = Functions.ConvertHeight(_obstaclesPage1.Parts[i].MOC).ToString();
				//	itmX.SubItems[9].Text = Functions.ConvertHeight(_obstaclesPage1.Parts[i].hPenet, eRoundMode.CEIL).ToString();
				//	itmX.SubItems[10].Text = displayPDG.ToString();

				//	itmX.SubItems[11].Text = Functions.ConvertDistance(_obstaclesPage1.Parts[i].NomPDGDist).ToString();
				//	itmX.SubItems[12].Text = Functions.ConvertHeight(_obstaclesPage1.Parts[i].CourseAdjust, eRoundMode.CEIL).ToString();
				//	itmX.SubItems[13].Text = str13;
				//	itmX.SubItems[14].Text = Functions.ConvertHeight(_obstaclesPage1.Obstacles[_obstaclesPage1.Parts[i].Owner].HorAccuracy).ToString();
				//	itmX.SubItems[15].Text = Functions.ConvertHeight(_obstaclesPage1.Obstacles[_obstaclesPage1.Parts[i].Owner].VertAccuracy).ToString();

				//	if (_page1Procedure == 3)
				//	{
				//		if (_obstaclesPage1.Parts[i].Prima)
				//			itmX.SubItems[16].Text = Resources.str15080;
				//		else
				//			itmX.SubItems[16].Text = Resources.str15081;
				//	}
				//}

				FontStyle RecordFontStyle = FontStyle.Regular;
				Color RecordColor = Color.Black;

				if (_obstaclesPage1.Obstacles[item.Key].ID == _page1DetID)
				{
					RecordFontStyle = FontStyle.Bold;
					RecordColor = Color.Red;
				}

				itmX.Font = new Font(itmX.Font, RecordFontStyle);
				itmX.ForeColor = RecordColor;

				for (int j = 1; j < itmX.SubItems.Count; j++)
				{
					itmX.SubItems[j].Font = itmX.Font;
					itmX.SubItems[j].ForeColor = RecordColor;
				}

				itmX.SubItems[3].Font = new Font(itmX.SubItems[3].Font, FontStyle.Bold);
			}

			_columnSorter.Order = SortOrder.Ascending;
			ListViews_ColumnClick(listView1, new ColumnClickEventArgs(sortByField));
			listView1.EndUpdate();
		}

		private void UpdateListView02(int sortByField)
		{
			int i;

			if (sortByField >= 0)
			{
				int sortF = (int)listView2.Tag;

				if (Math.Abs(sortF) - 1 == sortByField)
					sortF = -sortF;
				else
				{
					if (sortF != 0)
						listView2.Columns[Math.Abs(sortF) - 1].ImageIndex = 2;
					sortF = -sortByField - 1;
				}

				listView2.Tag = sortF;
				int n = _obstaclesPage2.Parts.Length;

				if (sortByField <= 1)
					for (i = 0; i < n; i++)
					{
						switch (sortByField)
						{
							case 0:
								_obstaclesPage2.Parts[i].sSort = _obstaclesPage2.Obstacles[_obstaclesPage2.Parts[i].Owner].TypeName; break;
							case 1:
								_obstaclesPage2.Parts[i].sSort = _obstaclesPage2.Obstacles[_obstaclesPage2.Parts[i].Owner].UnicalName; break;
						}
					}
				else
					for (i = 0; i < n; i++)
					{
						switch (sortByField)
						{
							case 2:
								_obstaclesPage2.Parts[i].fSort = _obstaclesPage2.Parts[i].CourseAdjust; break;
							case 3:
								_obstaclesPage2.Parts[i].fSort = _obstaclesPage2.Parts[i].Height; break;
							case 4:
								_obstaclesPage2.Parts[i].fSort = _obstaclesPage2.Parts[i].Dist; break;
							case 5:
								_obstaclesPage2.Parts[i].fSort = _obstaclesPage2.Parts[i].DistStar; break;
							case 6:
								_obstaclesPage2.Parts[i].fSort = _obstaclesPage2.Parts[i].MOC; break;
							case 7:
								_obstaclesPage2.Parts[i].fSort = _obstaclesPage2.Parts[i].ReqH; break;
							case 8:
								_obstaclesPage2.Parts[i].fSort = _obstaclesPage2.Parts[i].CLShift; break;
							case 9:
								_obstaclesPage2.Parts[i].fSort = _obstaclesPage2.Obstacles[_obstaclesPage2.Parts[i].Owner].HorAccuracy; break;
							case 10:
								_obstaclesPage2.Parts[i].fSort = _obstaclesPage2.Obstacles[_obstaclesPage2.Parts[i].Owner].VertAccuracy; break;
						}
					}

				if (sortF > 0)
					listView2.Columns[sortByField].ImageIndex = 0;
				else
					listView2.Columns[sortByField].ImageIndex = 1;

				//ObstacleComparer comparer = new ObstacleComparer(sortF, sortByField <= 1);
				//Array.Sort(_obstaclesPage2.Parts, comparer);
				_arrayComparer.SortOrder = sortF;
				_arrayComparer.TextField = sortByField <= 1;
				Array.Sort(_obstaclesPage2.Parts, _arrayComparer);

				//if (sortByField <= 1)
				//{
				//	if (sortF > 0)
				//		Functions.shall_SortsSort(_obstaclesPage2.Parts);
				//	else
				//		Functions.shall_SortsSortD(_obstaclesPage2.Parts);
				//}
				//else if (sortF > 0)
				//	Functions.shall_SortfSort(_obstaclesPage2.Parts);
				//else
				//	Functions.shall_SortfSortD(_obstaclesPage2.Parts);
			}

			IDictionary<int, int> unicalObstacleList = GetUnicalObstales(_obstaclesPage2);
			listView2.BeginUpdate();
			listView2.Items.Clear();

			foreach (KeyValuePair<int, int> item in unicalObstacleList)
			{
				ListViewItem itmX = listView2.Items.Add(_obstaclesPage2.Obstacles[item.Key].TypeName);
				itmX.SubItems.Insert(1, new ListViewItem.ListViewSubItem(null, _obstaclesPage2.Obstacles[item.Key].UnicalName));
				i = item.Value;
				itmX.Tag = i;

				itmX.SubItems.Insert(2, new ListViewItem.ListViewSubItem(null, Functions.ConvertHeight(_obstaclesPage2.Parts[i].CourseAdjust).ToString()));
				itmX.SubItems[2].Font = new Font(itmX.Font, FontStyle.Bold);

				itmX.SubItems.Insert(3, new ListViewItem.ListViewSubItem(null, Functions.ConvertHeight(_obstaclesPage2.Parts[i].Height).ToString()));
				itmX.SubItems.Insert(4, new ListViewItem.ListViewSubItem(null, Functions.ConvertDistance(_obstaclesPage2.Parts[i].Dist).ToString()));
				itmX.SubItems.Insert(5, new ListViewItem.ListViewSubItem(null, Functions.ConvertDistance(_obstaclesPage2.Parts[i].DistStar).ToString()));
				itmX.SubItems.Insert(6, new ListViewItem.ListViewSubItem(null, Functions.ConvertHeight(_obstaclesPage2.Parts[i].MOC).ToString()));
				itmX.SubItems.Insert(7, new ListViewItem.ListViewSubItem(null, Functions.ConvertHeight(_obstaclesPage2.Parts[i].ReqH).ToString()));
				itmX.SubItems.Insert(8, new ListViewItem.ListViewSubItem(null, Functions.ConvertHeight(_obstaclesPage2.Parts[i].CLShift).ToString()));
				itmX.SubItems.Insert(9, new ListViewItem.ListViewSubItem(null, Functions.ConvertHeight(_obstaclesPage2.Obstacles[item.Key].HorAccuracy).ToString()));
				itmX.SubItems.Insert(10, new ListViewItem.ListViewSubItem(null, Functions.ConvertHeight(_obstaclesPage2.Obstacles[item.Key].VertAccuracy).ToString()));
			}

			_columnSorter.Order = SortOrder.Ascending;
			ListViews_ColumnClick(listView2, new ColumnClickEventArgs(sortByField));
			listView2.EndUpdate();
		}

		private void UpdateListView03(int sortByField)
		{
			int i;

			if (sortByField >= 0)
			{
				int sortF = (int)listView3.Tag;

				if (Math.Abs(sortF) - 1 == sortByField)
					sortF = -sortF;
				else
				{
					if (sortF != 0)
						listView3.Columns[Math.Abs(sortF) - 1].ImageIndex = 2;
					sortF = -sortByField - 1;
				}

				listView3.Tag = sortF;
				int n = _obstaclesPage3.Parts.Length;

				if (sortByField <= 1)
					for (i = 0; i < n; i++)
						switch (sortByField)
						{
							case 0:
								_obstaclesPage3.Parts[i].sSort = _obstaclesPage3.Obstacles[_obstaclesPage3.Parts[i].Owner].TypeName; break;
							case 1:
								_obstaclesPage3.Parts[i].sSort = _obstaclesPage3.Obstacles[_obstaclesPage3.Parts[i].Owner].UnicalName; break;
						}
				else
					for (i = 0; i < n; i++)
						switch (sortByField)
						{
							case 2:
								_obstaclesPage3.Parts[i].fSort = _obstaclesPage3.Parts[i].CourseAdjust; break;
							case 3:
								_obstaclesPage3.Parts[i].fSort = _obstaclesPage3.Parts[i].Height; break;
							case 4:
								_obstaclesPage3.Parts[i].fSort = _obstaclesPage3.Parts[i].Dist; break;
							case 5:
								_obstaclesPage3.Parts[i].fSort = _obstaclesPage3.Parts[i].DistStar; break;
							case 6:
								_obstaclesPage3.Parts[i].fSort = _obstaclesPage3.Parts[i].MOC; break;
							case 7:
								_obstaclesPage3.Parts[i].fSort = _obstaclesPage3.Parts[i].ReqH; break;
							case 8:
								_obstaclesPage3.Parts[i].fSort = _obstaclesPage3.Parts[i].CLShift; break;
							case 9:
								_obstaclesPage3.Parts[i].fSort = _obstaclesPage3.Obstacles[_obstaclesPage3.Parts[i].Owner].HorAccuracy; break;
							case 10:
								_obstaclesPage3.Parts[i].fSort = _obstaclesPage3.Obstacles[_obstaclesPage3.Parts[i].Owner].VertAccuracy; break;
						}

				if (sortF > 0)
					listView3.Columns[sortByField].ImageIndex = 0;
				else
					listView3.Columns[sortByField].ImageIndex = 1;

				//ObstacleComparer comparer = new ObstacleComparer(sortF, sortByField <= 1);
				//Array.Sort(_obstaclesPage3.Parts, comparer);

				_arrayComparer.SortOrder = sortF;
				_arrayComparer.TextField = sortByField <= 1;
				Array.Sort(_obstaclesPage3.Parts, _arrayComparer);

				//if (sortByField <= 1)
				//{
				//	if (sortF > 0)
				//		Functions.shall_SortsSort(_obstaclesPage3.Parts);
				//	else
				//		Functions.shall_SortsSortD(_obstaclesPage3.Parts);
				//}
				//else if (sortF > 0)
				//	Functions.shall_SortfSort(_obstaclesPage3.Parts);
				//else
				//	Functions.shall_SortfSortD(_obstaclesPage3.Parts);
			}

			IDictionary<int, int> unicalObstacleList = GetUnicalObstales(_obstaclesPage3);
			listView3.BeginUpdate();
			listView3.Items.Clear();

			foreach (KeyValuePair<int, int> item in unicalObstacleList)
			{
				ListViewItem itmX = listView3.Items.Add(_obstaclesPage3.Obstacles[item.Key].TypeName);
				itmX.SubItems.Insert(1, new ListViewItem.ListViewSubItem(null, _obstaclesPage3.Obstacles[item.Key].UnicalName));

				i = item.Value;
				itmX.Tag = i;

				itmX.SubItems.Insert(2, new ListViewItem.ListViewSubItem(null, Functions.ConvertHeight(_obstaclesPage3.Parts[i].CourseAdjust).ToString()));
				itmX.SubItems.Insert(3, new ListViewItem.ListViewSubItem(null, Functions.ConvertHeight(_obstaclesPage3.Parts[i].Height).ToString()));
				itmX.SubItems.Insert(4, new ListViewItem.ListViewSubItem(null, Functions.ConvertDistance(_obstaclesPage3.Parts[i].Dist).ToString()));
				itmX.SubItems.Insert(5, new ListViewItem.ListViewSubItem(null, Functions.ConvertDistance(_obstaclesPage3.Parts[i].DistStar).ToString()));
				itmX.SubItems.Insert(6, new ListViewItem.ListViewSubItem(null, Functions.ConvertHeight(_obstaclesPage3.Parts[i].MOC).ToString()));
				itmX.SubItems.Insert(7, new ListViewItem.ListViewSubItem(null, Functions.ConvertHeight(_obstaclesPage3.Parts[i].ReqH).ToString()));
				itmX.SubItems.Insert(8, new ListViewItem.ListViewSubItem(null, Functions.ConvertHeight(_obstaclesPage3.Parts[i].CLShift).ToString()));
				itmX.SubItems.Insert(9, new ListViewItem.ListViewSubItem(null, Functions.ConvertHeight(_obstaclesPage3.Obstacles[item.Key].HorAccuracy).ToString()));
				itmX.SubItems.Insert(10, new ListViewItem.ListViewSubItem(null, Functions.ConvertHeight(_obstaclesPage3.Obstacles[item.Key].VertAccuracy).ToString()));

				itmX.SubItems[2].Font = new Font(itmX.Font, FontStyle.Bold);

				FontStyle RecordFontStyle;
				Color RecordColor;

				if (_obstaclesPage3.Obstacles[item.Key].ID == _page3IndexID)
				{
					RecordFontStyle = FontStyle.Bold;
					RecordColor = Color.Red;
				}
				else
				{
					RecordFontStyle = FontStyle.Regular;
					RecordColor = Color.Black;
				}

				itmX.Font = new Font(itmX.Font, RecordFontStyle);
				itmX.ForeColor = RecordColor;

				for (int j = 1; j <= 9; j++)
				{
					itmX.SubItems[j].Font = itmX.Font;
					itmX.SubItems[j].ForeColor = RecordColor;
				}

				itmX.SubItems[2].Font = new Font(itmX.SubItems[2].Font, FontStyle.Bold);
			}

			_columnSorter.Order = SortOrder.Ascending;
			ListViews_ColumnClick(listView3, new ColumnClickEventArgs(sortByField));
			listView3.EndUpdate();
		}

		private void UpdateListView04(int sortByField)
		{
			int i;

			if (sortByField >= 0)
			{
				int sortF = (int)listView4.Tag;
				if (Math.Abs(sortF) - 1 == sortByField)
					sortF = -sortF;
				else
				{
					if (sortF != 0)
						listView4.Columns[Math.Abs(sortF) - 1].ImageIndex = 2;
					sortF = -sortByField - 1;
				}

				listView4.Tag = sortF;
				int n = _obstaclesPage4.Parts.Length;

				if (sortByField <= 1 || sortByField == 11)
					for (i = 0; i < n; i++)
					{
						_obstaclesPage4.Parts[i].hPenet = _obstaclesPage4.Parts[i].MOC + _obstaclesPage4.Parts[i].Height;
						switch (sortByField)
						{
							case 0:
								_obstaclesPage4.Parts[i].sSort = _obstaclesPage4.Obstacles[_obstaclesPage4.Parts[i].Owner].TypeName; break;
							case 1:
								_obstaclesPage4.Parts[i].sSort = _obstaclesPage4.Obstacles[_obstaclesPage4.Parts[i].Owner].UnicalName; break;
							case 11:
								if (_obstaclesPage4.Parts[i].PDG > PANS_OPS_DataBase.dpPDG_Nom.Value)
								{
									if (_obstaclesPage4.Parts[i].Ignored)
										_obstaclesPage4.Parts[i].sSort = Resources.str40030; // "???????????????????"
									else
										_obstaclesPage4.Parts[i].sSort = Resources.str40010; // "????????????"
								}
								else
									_obstaclesPage4.Parts[i].sSort = " ";
								break;
						}
					}
				else
					for (i = 0; i < n; i++)
					{
						_obstaclesPage4.Parts[i].hPenet = _obstaclesPage4.Parts[i].MOC + _obstaclesPage4.Parts[i].Height;
						switch (sortByField)
						{
							case 2:
								_obstaclesPage4.Parts[i].fSort = _obstaclesPage4.Parts[i].PDGToTop; break;
							case 3:
								_obstaclesPage4.Parts[i].fSort = _obstaclesPage4.Parts[i].PDG; break;
							case 4:
								_obstaclesPage4.Parts[i].fSort = _obstaclesPage4.Parts[i].ReqTNH; break;
							case 5:
								_obstaclesPage4.Parts[i].fSort = _obstaclesPage4.Parts[i].CLShift; break;
							case 6:
								_obstaclesPage4.Parts[i].fSort = _obstaclesPage4.Parts[i].Dist; break;
							case 7:
								_obstaclesPage4.Parts[i].fSort = _obstaclesPage4.Parts[i].Height; break;
							case 8:
								_obstaclesPage4.Parts[i].fSort = _obstaclesPage4.Parts[i].MOC; break;
							case 9:
								_obstaclesPage4.Parts[i].fSort = _obstaclesPage4.Parts[i].hPenet; break;
							case 10:
								_obstaclesPage4.Parts[i].fSort = Math.Round(100.0 * _obstaclesPage4.Parts[i].PDG + 0.04999, 1); break;
							case 12:
								_obstaclesPage4.Parts[i].fSort = _obstaclesPage4.Obstacles[_obstaclesPage4.Parts[i].Owner].HorAccuracy; break;
							case 13:
								_obstaclesPage4.Parts[i].fSort = _obstaclesPage4.Obstacles[_obstaclesPage4.Parts[i].Owner].VertAccuracy;
								break;
						}
					}

				if (sortF > 0)
					listView4.Columns[sortByField].ImageIndex = 0;
				else
					listView4.Columns[sortByField].ImageIndex = 1;

				//ObstacleComparer comparer = new ObstacleComparer(sortF, sortByField <= 1 || sortByField == 11);
				//Array.Sort(_obstaclesPage4.Parts, comparer);
				_arrayComparer.SortOrder = sortF;
				_arrayComparer.TextField = sortByField <= 1 || sortByField == 11;
				Array.Sort(_obstaclesPage4.Parts, _arrayComparer);


				//if (sortByField <= 1 || sortByField == 11)
				//{
				//	if (sortF > 0)
				//		Functions.shall_SortsSort(_obstaclesPage4.Parts);
				//	else
				//		Functions.shall_SortsSortD(_obstaclesPage4.Parts);
				//}
				//else if (sortF > 0)
				//	Functions.shall_SortfSort(_obstaclesPage4.Parts);
				//else
				//	Functions.shall_SortfSortD(_obstaclesPage4.Parts);
			}

			IDictionary<int, int> unicalObstacleList = GetUnicalObstales(_obstaclesPage4);
			listView4.BeginUpdate();
			listView4.Items.Clear();

			foreach (KeyValuePair<int, int> item in unicalObstacleList)
			{
				ListViewItem itmX = listView4.Items.Add(_obstaclesPage4.Obstacles[item.Key].TypeName);
				itmX.SubItems.Insert(1, new ListViewItem.ListViewSubItem(null, _obstaclesPage4.Obstacles[item.Key].UnicalName));

				i = item.Value;
				itmX.Tag = i;

				itmX.SubItems.Insert(2, new ListViewItem.ListViewSubItem(null, Math.Round(100.0 * _obstaclesPage4.Parts[i].PDGToTop, 3).ToString()));
				itmX.SubItems.Insert(3, new ListViewItem.ListViewSubItem(null, Math.Round(100.0 * _obstaclesPage4.Parts[i].PDG, 3).ToString()));
				itmX.SubItems[3].Font = new Font(itmX.SubItems[3].Font, FontStyle.Bold);
				//itmX.SubItems[3].Font = new Font(itmX.Font, FontStyle.Bold);

				itmX.SubItems.Insert(4, new ListViewItem.ListViewSubItem(null, Functions.ConvertHeight(_obstaclesPage4.Parts[i].ReqTNH, eRoundMode.CEIL).ToString()));
				itmX.SubItems.Insert(5, new ListViewItem.ListViewSubItem(null, Functions.ConvertDistance(_obstaclesPage4.Parts[i].CLShift).ToString()));
				itmX.SubItems.Insert(6, new ListViewItem.ListViewSubItem(null, Functions.ConvertDistance(_obstaclesPage4.Parts[i].Dist).ToString()));
				itmX.SubItems.Insert(7, new ListViewItem.ListViewSubItem(null, Functions.ConvertHeight(_obstaclesPage4.Parts[i].Height).ToString()));
				itmX.SubItems.Insert(8, new ListViewItem.ListViewSubItem(null, Functions.ConvertHeight(_obstaclesPage4.Parts[i].MOC).ToString()));
				itmX.SubItems.Insert(9, new ListViewItem.ListViewSubItem(null, Functions.ConvertHeight(_obstaclesPage4.Parts[i].hPenet, eRoundMode.CEIL).ToString()));
				itmX.SubItems.Insert(10, new ListViewItem.ListViewSubItem(null, Math.Round(100.0 * _obstaclesPage4.Parts[i].PDG + 0.04999, 1).ToString()));

				if (_obstaclesPage4.Parts[i].PDG <= PANS_OPS_DataBase.dpPDG_Nom.Value)
					itmX.SubItems.Insert(11, new ListViewItem.ListViewSubItem(null, "-"));
				else if (_obstaclesPage4.Parts[i].Ignored)
					itmX.SubItems.Insert(11, new ListViewItem.ListViewSubItem(null, Resources.str40030));
				else
					itmX.SubItems.Insert(11, new ListViewItem.ListViewSubItem(null, Resources.str40010));

				itmX.SubItems.Insert(12, new ListViewItem.ListViewSubItem(null, Functions.ConvertHeight(_obstaclesPage4.Obstacles[item.Key].HorAccuracy).ToString()));
				itmX.SubItems.Insert(13, new ListViewItem.ListViewSubItem(null, Functions.ConvertHeight(_obstaclesPage4.Obstacles[item.Key].VertAccuracy).ToString()));
			}

			_columnSorter.Order = SortOrder.Ascending;
			ListViews_ColumnClick(listView4, new ColumnClickEventArgs(sortByField));
			listView4.EndUpdate();
		}

		private void UpdateListView05(int sortByField)
		{
			int i;

			if (sortByField >= 0)
			{
				int sortF = (int)listView5.Tag;
				if (Math.Abs(sortF) - 1 == sortByField)
					sortF = -sortF;
				else
				{
					if (sortF != 0)
						listView5.Columns[Math.Abs(sortF) - 1].ImageIndex = 2;

					sortF = -sortByField - 1;
				}

				listView5.Tag = sortF;
				int n = _obstaclesPage5.Parts.Length;

				if (sortByField <= 1)
					for (i = 0; i < n; i++)
						switch (sortByField)
						{
							case 0:
								_obstaclesPage5.Parts[i].sSort = _obstaclesPage5.Obstacles[_obstaclesPage5.Parts[i].Owner].TypeName; break;
							case 1:
								_obstaclesPage5.Parts[i].sSort = _obstaclesPage5.Obstacles[_obstaclesPage5.Parts[i].Owner].UnicalName; break;
						}
				else
					for (i = 0; i < n; i++)
						switch (sortByField)
						{
							case 2:
								_obstaclesPage5.Parts[i].fSort = _obstaclesPage5.Parts[i].hPenet; break;
							case 3:
								_obstaclesPage5.Parts[i].fSort = _obstaclesPage5.Parts[i].Height; break;
							case 4:
								_obstaclesPage5.Parts[i].fSort = _obstaclesPage5.Parts[i].Dist; break;
							case 5:
								_obstaclesPage5.Parts[i].fSort = _obstaclesPage5.Parts[i].DistStar; break;
							case 6:
								_obstaclesPage5.Parts[i].fSort = _obstaclesPage5.Parts[i].MOC; break;
							case 7:
								_obstaclesPage5.Parts[i].fSort = _obstaclesPage5.Obstacles[_obstaclesPage5.Parts[i].Owner].HorAccuracy; break;
							case 8:
								_obstaclesPage5.Parts[i].fSort = _obstaclesPage5.Obstacles[_obstaclesPage5.Parts[i].Owner].VertAccuracy; break;
						}

				if (sortF > 0)
					listView5.Columns[sortByField].ImageIndex = 0;
				else
					listView5.Columns[sortByField].ImageIndex = 1;

				//ObstacleComparer comparer = new ObstacleComparer(sortF, sortByField <= 1);
				//Array.Sort(_obstaclesPage5.Parts, comparer);

				_arrayComparer.SortOrder = sortF;
				_arrayComparer.TextField = sortByField <= 1;
				Array.Sort(_obstaclesPage5.Parts, _arrayComparer);

				//if (sortByField <= 1)
				//{
				//	if (sortF > 0)
				//		Functions.shall_SortsSort(_obstaclesPage5.Parts);
				//	else
				//		Functions.shall_SortsSortD(_obstaclesPage5.Parts);
				//}
				//else if (sortF > 0)
				//	Functions.shall_SortfSort(_obstaclesPage5.Parts);
				//else
				//	Functions.shall_SortfSortD(_obstaclesPage5.Parts);
			}

			IDictionary<int, int> unicalObstacleList = GetUnicalObstales(_obstaclesPage5);
			listView5.BeginUpdate();
			listView5.Items.Clear();

			foreach (KeyValuePair<int, int> item in unicalObstacleList)
			{
				ListViewItem itmX = listView5.Items.Add(_obstaclesPage5.Obstacles[item.Key].TypeName);
				itmX.SubItems.Insert(1, new ListViewItem.ListViewSubItem(null, _obstaclesPage5.Obstacles[item.Key].UnicalName));

				i = item.Value;
				itmX.Tag = i;

				itmX.SubItems.Insert(2, new ListViewItem.ListViewSubItem(null, Functions.ConvertHeight(_obstaclesPage5.Parts[i].hPenet).ToString()));
				itmX.SubItems[2].Font = new Font(itmX.SubItems[2].Font, FontStyle.Bold);

				itmX.SubItems.Insert(3, new ListViewItem.ListViewSubItem(null, Functions.ConvertHeight(_obstaclesPage5.Parts[i].Height).ToString()));
				itmX.SubItems.Insert(4, new ListViewItem.ListViewSubItem(null, Functions.ConvertDistance(_obstaclesPage5.Parts[i].Dist).ToString()));
				itmX.SubItems.Insert(5, new ListViewItem.ListViewSubItem(null, Functions.ConvertDistance(_obstaclesPage5.Parts[i].DistStar).ToString()));
				itmX.SubItems.Insert(6, new ListViewItem.ListViewSubItem(null, Functions.ConvertHeight(_obstaclesPage5.Parts[i].MOC).ToString()));
				itmX.SubItems.Insert(7, new ListViewItem.ListViewSubItem(null, Functions.ConvertHeight(_obstaclesPage5.Obstacles[item.Key].HorAccuracy).ToString()));
				itmX.SubItems.Insert(8, new ListViewItem.ListViewSubItem(null, Functions.ConvertHeight(_obstaclesPage5.Obstacles[item.Key].VertAccuracy).ToString()));
			}

			_columnSorter.Order = SortOrder.Ascending;
			ListViews_ColumnClick(listView5, new ColumnClickEventArgs(sortByField));
			listView5.EndUpdate();
		}

		private void UpdateListView06(int sortByField)
		{
			int i;

			if (sortByField >= 0)
			{
				int sortF = (int)listView6.Tag;

				if (Math.Abs(sortF) - 1 == sortByField)
					sortF = -sortF;
				else
				{
					if (sortF != 0)
						listView6.Columns[Math.Abs(sortF) - 1].ImageIndex = 2;
					sortF = -sortByField - 1;
				}

				listView6.Tag = sortF;
				int n = _obstaclesPage6.Parts.Length;

				if (sortByField <= 1)
					for (i = 0; i < n; i++)
						switch (sortByField)
						{
							case 0:
								_obstaclesPage6.Parts[i].sSort = _obstaclesPage6.Obstacles[_obstaclesPage6.Parts[i].Owner].TypeName;
								break;
							case 1:
								_obstaclesPage6.Parts[i].sSort = _obstaclesPage6.Obstacles[_obstaclesPage6.Parts[i].Owner].UnicalName;
								break;
						}
				else
					for (i = 0; i < n; i++)
						switch (sortByField)
						{
							case 2:
								_obstaclesPage6.Parts[i].fSort = _obstaclesPage6.Parts[i].CLShift;
								break;
							case 3:
								_obstaclesPage6.Parts[i].fSort = _obstaclesPage6.Parts[i].hPenet;
								break;
							case 4:
								_obstaclesPage6.Parts[i].fSort = _obstaclesPage6.Parts[i].Height;
								break;
							case 5:
								_obstaclesPage6.Parts[i].fSort = _obstaclesPage6.Parts[i].MOC;
								break;
							case 6:
								_obstaclesPage6.Parts[i].fSort = _obstaclesPage6.Parts[i].ReqH;
								break;
							case 7:
								_obstaclesPage6.Parts[i].fSort = _obstaclesPage6.Parts[i].CourseAdjust;
								break;
						}


				if (sortF > 0)
					listView6.Columns[sortByField].ImageIndex = 0;
				else
					listView6.Columns[sortByField].ImageIndex = 1;

				_arrayComparer.SortOrder = sortF;
				_arrayComparer.TextField = sortByField <= 1;
				Array.Sort(_obstaclesPage6.Parts, _arrayComparer);

				//if (sortByField <= 1)
				//{
				//	if (sortF > 0)
				//		Functions.shall_SortsSort(_obstaclesPage6.Parts);
				//	else
				//		Functions.shall_SortsSortD(_obstaclesPage6.Parts);
				//}
				//else if (sortF > 0)
				//	Functions.shall_SortfSort(_obstaclesPage6.Parts);
				//else
				//	Functions.shall_SortfSortD(_obstaclesPage6.Parts);
			}

			IDictionary<int, int> unicalObstacleList = GetUnicalObstales(_obstaclesPage6);
			listView6.BeginUpdate();
			listView6.Items.Clear();

			foreach (KeyValuePair<int, int> item in unicalObstacleList)
			{
				ListViewItem itmX = listView6.Items.Add(_obstaclesPage6.Obstacles[item.Key].TypeName);
				itmX.SubItems.Insert(1, new ListViewItem.ListViewSubItem(null, _obstaclesPage6.Obstacles[item.Key].UnicalName));

				i = item.Value;
				itmX.Tag = i;

				itmX.SubItems.Insert(2, new ListViewItem.ListViewSubItem(null, _obstaclesPage6.Parts[i].CLShift.ToString()));
				itmX.SubItems.Insert(3, new ListViewItem.ListViewSubItem(null, Functions.ConvertHeight(_obstaclesPage6.Parts[i].hPenet).ToString()));
				itmX.SubItems.Insert(4, new ListViewItem.ListViewSubItem(null, Functions.ConvertHeight(_obstaclesPage6.Parts[i].Height).ToString()));
				itmX.SubItems.Insert(5, new ListViewItem.ListViewSubItem(null, Functions.ConvertHeight(_obstaclesPage6.Parts[i].MOC).ToString()));
				itmX.SubItems.Insert(6, new ListViewItem.ListViewSubItem(null, Functions.ConvertHeight(_obstaclesPage6.Parts[i].ReqH).ToString()));
				itmX.SubItems.Insert(7, new ListViewItem.ListViewSubItem(null, Functions.ConvertHeight(_obstaclesPage6.Parts[i].CourseAdjust).ToString()));
			}

			_columnSorter.Order = SortOrder.Ascending;
			ListViews_ColumnClick(listView6, new ColumnClickEventArgs(sortByField));
			listView6.EndUpdate();
		}

		public void FillPage1(ObstacleContainer Obstacles, double PDG, ObstacleData DetObs, string RWYName, int Procedure, bool bZNR = false)
		{
			label01.Text = Resources.str01001 + " " + RWYName;

			_page1Procedure = Procedure;
			if (DetObs.Owner == -1)
				_page1DetID = -1;
			else
				_page1DetID = Obstacles.Obstacles[DetObs.Owner].ID;

			switch (Procedure)
			{
				case 1:
					//Text = Resources.str15069;
					if (bZNR)
						_tapPages[0].Text = Resources.str15090;
					else
						_tapPages[0].Text = Resources.str15075;
					break;
				case 2:
				case 3:
					_tapPages[0].Text = Resources.str15079;

					//if (Procedure == 2)
					//{
					//	//Text = Resources.str15076;
					//}
					//else

					if (Procedure == 3)
					{
						if (listView1.Columns.Count < 17)
							listView1.Columns.Add(Resources.str15072, 67, HorizontalAlignment.Right);
						//Text = Resources.str15077)
					}
					break;
			}

			int m = Obstacles.Obstacles.Length;
			int n = Obstacles.Parts.Length;

			_obstaclesPage1.Obstacles = new Obstacle[m];
			_obstaclesPage1.Parts = new ObstacleData[n];

			Array.Copy(Obstacles.Obstacles, _obstaclesPage1.Obstacles, m);
			Array.Copy(Obstacles.Parts, _obstaclesPage1.Parts, n);

			//int Page1Det = -1;
			for (int i = 0; i < n; i++)
			{
				_obstaclesPage1.Parts[i] = Obstacles.Parts[i];

				//if (_obstaclesPage1.Obstacles[_obstaclesPage1.Parts[i].Owner].ID == _page1DetID)	Page1Det = i;
				_obstaclesPage1.Parts[i].hPenet = _obstaclesPage1.Parts[i].MOC + _obstaclesPage1.Parts[i].Height;
				_obstaclesPage1.Parts[i].CourseAdjust = PANS_OPS_DataBase.dpOIS_abv_DER.Value + _obstaclesPage1.Parts[i].NomPDGDist * PDG;
				//_obstaclesPage1.Parts[i].IsInteresting = Page1Det > -1;
			}

			int sortF = (int)listView1.Tag;
			/** /
			int sortByField = Page01SortIndex;
			if (sortF != 0)
			{
				sortByField = Math.Abs(sortF) - 1;
				listView1.Tag = -sortF;
			}

			UpdateListView01(sortByField);
			/* */
			int prevIndex = Math.Abs(sortF) - 1;

			if (prevIndex >= 0)// && prevIndex != Page01SortIndex)
				listView1.Columns[prevIndex].ImageIndex = 2;

			listView1.Tag = Page01SortIndex + 1;
			UpdateListView01(Page01SortIndex);

			/* */

			//lblCountNumber.Text = listView1.Items.Count;

			SetTabVisible(0, true);
			if (_reportBtn.Checked && !Visible)
				this.Show(GlobalVars.Win32Window);
		}

		public void FillPage2(ObstacleContainer Obstacles, double PDG_ZR, double TNA_H, double MOCLimit)
		{
			int m = Obstacles.Obstacles.Length;
			int n = Obstacles.Parts.Length;

			_obstaclesPage2.Obstacles = new Obstacle[m];
			_obstaclesPage2.Parts = new ObstacleData[n];

			Array.Copy(Obstacles.Obstacles, _obstaclesPage2.Obstacles, m);

			int j = 0;

			for (int i = 0; i < n; i++)
			{
				double hPenet = Obstacles.Parts[i].Height - (TNA_H + PDG_ZR * Obstacles.Parts[i].Dist - Obstacles.Parts[i].MOC);
				if (hPenet > -MOCLimit)
				{
					_obstaclesPage2.Parts[j] = Obstacles.Parts[i];

					_obstaclesPage2.Parts[j].CourseAdjust = hPenet;
					_obstaclesPage2.Parts[j].CLShift = TNA_H + PDG_ZR * _obstaclesPage2.Parts[j].Dist;
					_obstaclesPage2.Parts[j].ReqH = _obstaclesPage2.Parts[j].Height + _obstaclesPage2.Parts[j].MOC;
					j++;
				}
			}

			Array.Resize(ref _obstaclesPage2.Parts, j);

			int sortF = (int)listView2.Tag;
			int prevIndex = Math.Abs(sortF) - 1;

			if (prevIndex >= 0)
				listView2.Columns[prevIndex].ImageIndex = 2;

			listView2.Tag = Page02SortIndex + 1;
			UpdateListView02(Page02SortIndex);
			//lblCountNumber.Text = listView2.Items.Count;

			SetTabVisible(1, true);

			if (_reportBtn.Checked && !(Visible))
				Show(GlobalVars.Win32Window);
		}

		public void FillPage3(ObstacleContainer Obstacles, double PDG_ZR, double TNA_H,  double MOCLimit, int Index = -1)
		{
			if (Index >= 0)
				_page3IndexID = Obstacles.Obstacles[Obstacles.Parts[Index].Owner].ID;
			else
				_page3IndexID = -1;

			int m = Obstacles.Obstacles.Length;
			int n = Obstacles.Parts.Length;

			_obstaclesPage3.Obstacles = new Obstacle[m];
			_obstaclesPage3.Parts = new ObstacleData[n];

			Array.Copy(Obstacles.Obstacles, _obstaclesPage3.Obstacles, m);

			int j = 0;
			for (int i = 0; i < n; i++)
			{
				double hPenet = Obstacles.Parts[i].Height - (TNA_H + PDG_ZR * Obstacles.Parts[i].Dist - Obstacles.Parts[i].MOC);
				if (hPenet > -MOCLimit)
				{
					_obstaclesPage3.Parts[j] = Obstacles.Parts[i];

					_obstaclesPage3.Parts[j].CourseAdjust = hPenet;
					_obstaclesPage3.Parts[j].CLShift = TNA_H + PDG_ZR * _obstaclesPage3.Parts[j].Dist;
					_obstaclesPage3.Parts[j].ReqH = _obstaclesPage3.Parts[j].Height + _obstaclesPage3.Parts[j].MOC;
					j++;
				}
			}

			Array.Resize(ref _obstaclesPage3.Parts, j);

			int sortF = (int)listView3.Tag;
			int prevIndex = Math.Abs(sortF) - 1;

			if (prevIndex >= 0)
				listView3.Columns[prevIndex].ImageIndex = 2;

			listView3.Tag = Page03SortIndex + 1;
			UpdateListView03(Page03SortIndex);
			//lblCountNumber.Text = listView3.Items.Count;

			SetTabVisible(2, true);
			if (_reportBtn.Checked && !(Visible))
				Show(GlobalVars.Win32Window);
		}

		public void FillPage4(ObstacleContainer Obstacles, bool bAtHeight)
		{
			_page4AtHeight = bAtHeight;

			if (bAtHeight)
				_tapPages[3].Text = Resources.str15090;
			else
				_tapPages[3].Text = Resources.str00430;

			int m = Obstacles.Obstacles.Length;
			int n = Obstacles.Parts.Length;

			_obstaclesPage4.Obstacles = new Obstacle[m];
			_obstaclesPage4.Parts = new ObstacleData[n];

			Array.Copy(Obstacles.Obstacles, _obstaclesPage4.Obstacles, m);
			Array.Copy(Obstacles.Parts, _obstaclesPage4.Parts, n);

			int sortF = (int)listView4.Tag;
			int prevIndex = Math.Abs(sortF) - 1;

			if (prevIndex >= 0)
				listView4.Columns[prevIndex].ImageIndex = 2;

			int sortByField = Page04SortIndexF;
			if (_page4AtHeight)
				sortByField = Page04SortIndexH;

			listView4.Tag = sortByField + 1;
			UpdateListView04(sortByField);
			//lblCountNumber.Text = listView4.Items.Count;

			SetTabVisible(3, true);
			if (_reportBtn.Checked && !Visible)
				Show(GlobalVars.Win32Window);
		}

		public void FillPage5(ObstacleContainer Obstacles)
		{
			int m = Obstacles.Obstacles.Length;
			int n = Obstacles.Parts.Length;

			_obstaclesPage5.Obstacles = new Obstacle[m];
			_obstaclesPage5.Parts = new ObstacleData[n];

			Array.Copy(Obstacles.Obstacles, _obstaclesPage5.Obstacles, m);
			Array.Copy(Obstacles.Parts, _obstaclesPage5.Parts, n);

			int sortF = (int)listView5.Tag;
			int prevIndex = Math.Abs(sortF) - 1;

			if (prevIndex >= 0)
				listView5.Columns[prevIndex].ImageIndex = 2;

			listView5.Tag = Page05SortIndex + 1;
			UpdateListView05(Page05SortIndex);
			//lblCountNumber.Text = listView5.Items.Count;

			SetTabVisible(4, true);
			if (_reportBtn.Checked && !Visible)
				Show(GlobalVars.Win32Window);
		}

		public void FillPage6(ObstacleContainer Obstacles, double TNA_H, double PDG_ZR, double AZT_DIR)
		{
			int m = Obstacles.Obstacles.Length;
			int n = Obstacles.Parts.Length;

			_obstaclesPage6.Obstacles = new Obstacle[m];
			_obstaclesPage6.Parts = new ObstacleData[n];

			Array.Copy(Obstacles.Obstacles, _obstaclesPage6.Obstacles, m);

			int i, j = 0;
			for (i = 0; i < n; i++)
			{
				if (!Obstacles.Parts[i].IsInteresting)
					continue;

				_obstaclesPage6.Parts[j] = Obstacles.Parts[i];

				_obstaclesPage6.Parts[j].CLShift = NativeMethods.Modulus(Math.Round(90.0 - AZT_DIR + _obstaclesPage6.Parts[j].SectorAngle, 2));
				_obstaclesPage6.Parts[j].CourseAdjust = TNA_H + PDG_ZR * _obstaclesPage6.Parts[j].Dist;
				_obstaclesPage6.Parts[j].ReqH = _obstaclesPage6.Parts[j].Height + _obstaclesPage6.Parts[j].MOC;
				j++;
			}

			Array.Resize<ObstacleData>(ref _obstaclesPage6.Parts, j);

			int sortF = (int)listView6.Tag;
			int prevIndex = Math.Abs(sortF) - 1;

			if (prevIndex >= 0)
				listView6.Columns[prevIndex].ImageIndex = 2;

			listView6.Tag = Page06SortIndex + 1;
			UpdateListView06(Page06SortIndex);
			//lblCountNumber.Text = listView6.Items.Count;

			SetTabVisible(5, true);
			if (_reportBtn.Checked && !Visible)
				Show(GlobalVars.Win32Window);
		}

		private void ListViews_ColumnClick(object eventSender, ColumnClickEventArgs eventArgs)
		{
			_columnSorter.SortListView(eventArgs.Column, (ListView)eventSender);
		}

		//private void ListView1_ColumnClick(object eventSender, ColumnClickEventArgs eventArgs)
		//{
		//	UpdateListView01(eventArgs.Column);

		//	if (_reportBtn.Checked)
		//		ListView1_SelectedIndexChanged(listView1, new EventArgs());
		//}

		//private void ListView2_ColumnClick(object eventSender, ColumnClickEventArgs eventArgs)
		//{
		//	UpdateListView02(eventArgs.Column);

		//	if (_reportBtn.Checked)
		//		ListView2_SelectedIndexChanged(listView2, new EventArgs());
		//}

		//private void ListView3_ColumnClick(object eventSender, ColumnClickEventArgs eventArgs)
		//{
		//	UpdateListView03(eventArgs.Column);

		//	if (_reportBtn.Checked)
		//		ListView3_SelectedIndexChanged(listView3, new EventArgs());
		//}

		//private void ListView4_ColumnClick(object eventSender, ColumnClickEventArgs eventArgs)
		//{
		//	UpdateListView04(eventArgs.Column);

		//	if (_reportBtn.Checked)
		//		ListView4_SelectedIndexChanged(listView4, new EventArgs());
		//}

		//private void ListView5_ColumnClick(object eventSender, ColumnClickEventArgs eventArgs)
		//{
		//	UpdateListView05(eventArgs.Column);

		//	if (_reportBtn.Checked)
		//		ListView5_SelectedIndexChanged(listView5, new EventArgs());
		//}

		//private void ListView6_ColumnClick(object eventSender, ColumnClickEventArgs eventArgs)
		//{
		//	UpdateListView06(eventArgs.Column);

		//	if (_reportBtn.Checked)
		//		ListView6_SelectedIndexChanged(listView6, new EventArgs());
		//}

		private void ListView1_SelectedIndexChanged(object sender, EventArgs e)
		{
			Functions.DeleteGraphicsElement(_pointElem);
			Functions.DeleteGraphicsElement(_geomElem);

			if (((ListView)sender).SelectedItems.Count == 0 || _obstaclesPage1.Parts.Length == 0 || !Visible)
				return;

			ListViewItem item = ((ListView)sender).SelectedItems[0];

			if (item == null)
				return;

			int index = (int)item.Tag;
			int owner = _obstaclesPage1.Parts[index].Owner;
			IGeometry pGeometry = _obstaclesPage1.Obstacles[owner].pGeomPrj;

			if (pGeometry.GeometryType == esriGeometryType.esriGeometryPolyline)
			{
				_geomElem = Functions.DrawPolyline((IPolyline)pGeometry, 255, 2);
				_geomElem.Locked = true;
			}
			else if (pGeometry.GeometryType == esriGeometryType.esriGeometryPolygon)
			{
				_geomElem = Functions.DrawPolygon((IPolygon)pGeometry, 255, ESRI.ArcGIS.Display.esriSimpleFillStyle.esriSFSDiagonalCross);
				_geomElem.Locked = true;
			}

			_pointElem = Functions.DrawPointWithText(_obstaclesPage1.Parts[index].pPtPrj, _obstaclesPage1.Obstacles[owner].UnicalName, 255);
			_pointElem.Locked = true;
		}

		private void ListView2_SelectedIndexChanged(object sender, EventArgs e)
		{
			Functions.DeleteGraphicsElement(_pointElem);
			Functions.DeleteGraphicsElement(_geomElem);

			if (((ListView)sender).SelectedItems.Count == 0 || _obstaclesPage2.Parts.Length == 0 || !Visible)
				return;

			ListViewItem item = ((ListView)sender).SelectedItems[0];

			if (item == null)
				return;

			int index = (int)item.Tag;
			int owner = _obstaclesPage2.Parts[index].Owner;
			IGeometry pGeometry = _obstaclesPage2.Obstacles[owner].pGeomPrj;

			if (pGeometry.GeometryType == esriGeometryType.esriGeometryPolyline)
			{
				_geomElem = Functions.DrawPolyline((IPolyline)pGeometry, 255, 2);
				_geomElem.Locked = true;
			}
			else if (pGeometry.GeometryType == esriGeometryType.esriGeometryPolygon)
			{
				_geomElem = Functions.DrawPolygon((IPolygon)pGeometry, 255, ESRI.ArcGIS.Display.esriSimpleFillStyle.esriSFSDiagonalCross);
				_geomElem.Locked = true;
			}

			_pointElem = Functions.DrawPointWithText(_obstaclesPage2.Parts[index].pPtPrj, _obstaclesPage2.Obstacles[owner].UnicalName, 255);
			_pointElem.Locked = true;
		}

		private void ListView3_SelectedIndexChanged(object sender, EventArgs e)
		{
			Functions.DeleteGraphicsElement(_pointElem);
			Functions.DeleteGraphicsElement(_geomElem);

			if (((ListView)sender).SelectedItems.Count == 0 || _obstaclesPage3.Parts.Length == 0 || !Visible)
				return;

			ListViewItem item = ((ListView)sender).SelectedItems[0];

			if (item == null)
				return;

			int index = (int)item.Tag;
			int owner = _obstaclesPage3.Parts[index].Owner;
			IGeometry pGeometry = _obstaclesPage3.Obstacles[owner].pGeomPrj;

			if (pGeometry.GeometryType == esriGeometryType.esriGeometryPolyline)
			{
				_geomElem = Functions.DrawPolyline((IPolyline)pGeometry, 255, 2);
				_geomElem.Locked = true;
			}
			else if (pGeometry.GeometryType == esriGeometryType.esriGeometryPolygon)
			{
				_geomElem = Functions.DrawPolygon((IPolygon)pGeometry, 255, ESRI.ArcGIS.Display.esriSimpleFillStyle.esriSFSDiagonalCross);
				_geomElem.Locked = true;
			}

			_pointElem = Functions.DrawPointWithText(_obstaclesPage3.Parts[index].pPtPrj, _obstaclesPage3.Obstacles[owner].UnicalName, 255);
			_pointElem.Locked = true;
		}

		private void ListView4_SelectedIndexChanged(object sender, EventArgs e)
		{
			Functions.DeleteGraphicsElement(_pointElem);
			Functions.DeleteGraphicsElement(_geomElem);

			if (((ListView)sender).SelectedItems.Count == 0 || _obstaclesPage4.Parts.Length == 0 || !Visible)
				return;

			ListViewItem item = ((ListView)sender).SelectedItems[0];

			if (item == null)
				return;

			int index = (int)item.Tag;
			int owner = _obstaclesPage4.Parts[index].Owner;
			IGeometry pGeometry = _obstaclesPage4.Obstacles[owner].pGeomPrj;

			if (pGeometry.GeometryType == esriGeometryType.esriGeometryPolyline)
			{
				_geomElem = Functions.DrawPolyline((IPolyline)pGeometry, 255, 2);
				_geomElem.Locked = true;
			}
			else if (pGeometry.GeometryType == esriGeometryType.esriGeometryPolygon)
			{
				_geomElem = Functions.DrawPolygon((IPolygon)pGeometry, 255, ESRI.ArcGIS.Display.esriSimpleFillStyle.esriSFSDiagonalCross);
				_geomElem.Locked = true;
			}

			_pointElem = Functions.DrawPointWithText(_obstaclesPage4.Parts[index].pPtPrj, _obstaclesPage4.Obstacles[owner].UnicalName, 255);
			_pointElem.Locked = true;
		}

		private void ListView5_SelectedIndexChanged(object sender, EventArgs e)
		{
			Functions.DeleteGraphicsElement(_pointElem);
			Functions.DeleteGraphicsElement(_geomElem);

			if (((ListView)sender).SelectedItems.Count == 0 || _obstaclesPage5.Parts.Length == 0 || !Visible)
				return;

			ListViewItem item = ((ListView)sender).SelectedItems[0];

			if (item == null)
				return;

			int index = (int)item.Tag;
			int owner = _obstaclesPage5.Parts[index].Owner;
			IGeometry pGeometry = _obstaclesPage5.Obstacles[owner].pGeomPrj;

			if (pGeometry.GeometryType == esriGeometryType.esriGeometryPolyline)
			{
				_geomElem = Functions.DrawPolyline((IPolyline)pGeometry, 255, 2);
				_geomElem.Locked = true;
			}
			else if (pGeometry.GeometryType == esriGeometryType.esriGeometryPolygon)
			{
				_geomElem = Functions.DrawPolygon((IPolygon)pGeometry, 255, ESRI.ArcGIS.Display.esriSimpleFillStyle.esriSFSDiagonalCross);
				_geomElem.Locked = true;
			}

			_pointElem = Functions.DrawPointWithText(_obstaclesPage5.Parts[index].pPtPrj, _obstaclesPage5.Obstacles[owner].UnicalName, 255);
			_pointElem.Locked = true;
		}

		private void ListView6_SelectedIndexChanged(object sender, EventArgs e)
		{
			Functions.DeleteGraphicsElement(_pointElem);
			Functions.DeleteGraphicsElement(_geomElem);

			if (((ListView)sender).SelectedItems.Count == 0 || _obstaclesPage6.Parts.Length == 0 || !Visible)
				return;

			ListViewItem item = ((ListView)sender).SelectedItems[0];

			if (item == null)
				return;

			int index = (int)item.Tag;
			int owner = _obstaclesPage6.Parts[index].Owner;
			IGeometry pGeometry = _obstaclesPage6.Obstacles[owner].pGeomPrj;

			if (pGeometry.GeometryType == esriGeometryType.esriGeometryPolyline)
			{
				_geomElem = Functions.DrawPolyline((IPolyline)pGeometry, 255, 2);
				_geomElem.Locked = true;
			}
			else if (pGeometry.GeometryType == esriGeometryType.esriGeometryPolygon)
			{
				_geomElem = Functions.DrawPolygon((IPolygon)pGeometry, 255, ESRI.ArcGIS.Display.esriSimpleFillStyle.esriSFSDiagonalCross);
				_geomElem.Locked = true;
			}

			_pointElem = Functions.DrawPointWithText(_obstaclesPage6.Parts[index].pPtPrj, _obstaclesPage6.Obstacles[owner].UnicalName, 255);
			_pointElem.Locked = true;
		}

		private void mainTabControl_SelectedIndexChanged(object eventSender, EventArgs eventArgs)
		{
			TabPage SelectedTab = mainTabControl.SelectedTab;

			if (SelectedTab == _previousTab)
				return;

			_previousTab = SelectedTab;

			if (this.Visible)
			{
				if (SelectedTab == _tapPages[0])
					ListView1_SelectedIndexChanged(listView1, new EventArgs());
				else if (SelectedTab == _tapPages[1])
					ListView2_SelectedIndexChanged(listView2, new EventArgs());
				else if (SelectedTab == _tapPages[2])
					ListView3_SelectedIndexChanged(listView3, new EventArgs());
				else if (SelectedTab == _tapPages[3])
					ListView4_SelectedIndexChanged(listView4, new EventArgs());
				else if (SelectedTab == _tapPages[4])
					ListView5_SelectedIndexChanged(listView5, new EventArgs());
				else if (SelectedTab == _tapPages[5])
					ListView6_SelectedIndexChanged(listView6, new EventArgs());
			}
		}

		private void SaveBtn_Click(object eventSender, EventArgs eventArgs)
		{
			if (SaveDialog1.ShowDialog() != DialogResult.OK)
				return;

			StreamWriter stwr = new StreamWriter(SaveDialog1.FileName);
			TabPage tapPage = mainTabControl.SelectedTab;
			ListView lListView = (ListView)tapPage.Controls[0];

			stwr.WriteLine(Text);
			stwr.WriteLine("\t" + tapPage.Text);
			stwr.WriteLine();

			int n = lListView.Columns.Count;
			string[] HeadersText = new string[n];
			int[] HeadersLen = new int[n];

			int i, j, m, maxLen = 0;

			for (i = 0; i < n; i++)
			{
				ColumnHeader hdrX = lListView.Columns[i];
				HeadersText[i] = @"""" + hdrX.Text + @"""";
				HeadersLen[i] = HeadersText[i].Length;
				if (HeadersLen[i] > maxLen)
					maxLen = HeadersLen[i];
			}

			string StrOut = "", tmpStr;

			for (i = 0; i < n; i++)
			{
				j = maxLen - HeadersLen[i];
				m = j / 2;
				if (i < n)
					tmpStr = string.Empty.PadLeft(m) + "|";
				else
					tmpStr = "";

				StrOut = StrOut + string.Empty.PadLeft(j - m) + HeadersText[i] + tmpStr;
			}

			stwr.WriteLine(StrOut);
			StrOut = "";
			tmpStr = new string('-', maxLen) + "+";
			for (i = 1; i < n; i++)
				StrOut = StrOut + tmpStr;

			StrOut = StrOut + new string('-', maxLen);
			stwr.WriteLine(StrOut);

			m = lListView.Items.Count;
			for (i = 0; i < m; i++)
			{
				ListViewItem itmX = lListView.Items[i];

				int TmpLen = itmX.Text.Length;
				if (TmpLen > maxLen)
					StrOut = itmX.Text.Substring(0, maxLen - 1) + "*";
				else
					StrOut = string.Empty.PadLeft(maxLen - TmpLen) + itmX.Text;

				for (j = 1; j < n; j++)
				{
					tmpStr = itmX.SubItems[j].Text;

					TmpLen = tmpStr.Length;

					if (TmpLen > maxLen)
						tmpStr = tmpStr.Substring(0, maxLen - 1) + "*";
					else if (j < n - 1 || TmpLen > 0)
						tmpStr = string.Empty.PadLeft(maxLen - TmpLen) + tmpStr;

					StrOut = StrOut + "|" + tmpStr;
				}
				stwr.WriteLine(StrOut);
			}
			stwr.Flush();
			stwr.Dispose();
		}

		private void CloseBtn_Click(object eventSender, EventArgs eventArgs)
		{
			IGraphicsContainer pGraphics = null;

			pGraphics = GlobalVars.GetActiveView().GraphicsContainer;
			Functions.DeleteGraphicsElement(_pointElem);

			GlobalVars.GetActiveView().PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);

			_reportBtn.Checked = false;
			Hide();
		}

		private void HelpBtn_Click(object eventSender, EventArgs eventArgs)
		{
			NativeMethods.HtmlHelp(0, GlobalVars.HelpFile, GlobalVars.HH_HELP_CONTEXT, _helpContextID);
		}
	}

	class ObstacleComparer : System.Collections.IComparer
	{
		public int SortOrder { get; set; }
		public bool TextField { get; set; }

		public ObstacleComparer(int sortDirection, bool textFielt)
		{
			SortOrder = sortDirection;
			TextField = textFielt;
		}

		public ObstacleComparer()
		{
			SortOrder = 1;
			TextField = false;
		}

		public int Compare(object x, object y)
		{
			ObstacleData od0 = (ObstacleData)x;
			ObstacleData od1 = (ObstacleData)y;

			int compareResult;

			if (TextField)
				compareResult = od0.sSort.CompareTo(od1.sSort);
			else
				compareResult = od0.fSort.CompareTo(od1.fSort);

			if (SortOrder > 0)
				return compareResult;

			return -compareResult;
		}
	}
}
