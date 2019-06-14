﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Aran.PANDA.Common;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Interop;
using Aran.Geometries;
using Aran.Panda.RNAV.RNPAR.Utils;
using Aran.PANDA.Constants;
using Env = Aran.Panda.RNAV.RNPAR.Context.AppEnvironment;

namespace Aran.Panda.RNAV.RNPAR.UI.Forms
{
	public partial class RNP_ARReportForm : Form
	{
		public const int MaxLegsCount = 20;

	    [DllImport("user32.dll")]
	    static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        #region Variable declarations
        //private const int HelpContextID = 11200;

        private int pPointElem;
		private int pGeomElem;

		private ObstacleContainer _page01Obstacles;
		private int SortF01;

		private ObstacleContainer _page02Obstacles;
		private int SortF02;

		private ObstacleContainer _page03Obstacles;
		private int SortF03;

		//private ObstacleContainer _page04Obstacles;
		//private int SortF04;

		//private ObstacleContainer _page05Obstacles;
		//private int SortF05;

		//private ObstacleContainer _page06Obstacles;
		//private int SortF06;
		//private int PrecFlg06;

		private ObstacleContainer _page07Obstacles;

		private ObstacleContainer[] _page08Obstacles;
		private ObstacleData[] _page08DetObs;
		private int _page08Count = 0;
		private int _page08Index = -1;

		private System.Windows.Forms.CheckBox ReportBtn;
		#endregion

		#region Form
		public RNP_ARReportForm()
		{
			InitializeComponent();

			SortF01 = 0;
			SortF02 = 0;
			SortF03 = 0;
			//SortF04 = 0;
			//SortF05 = 0;
			//SortF06 = 0;

			// ListView03.ToolTipText = "*-Obstacle + Final MOC is above the missed approach surface at the SOC" + Chr(9) + "**-Obstacle does not penetrate the extension of the missed approach surface"

			SaveBtn.Text = "Save...";
			CloseBtn.Text = "Close";
			this.Text = "Clearance calculation";

			mainTabControl.TabPages[0].Text = "Obstacle Free Zone";
			mainTabControl.TabPages[1].Text = "OAS";
			mainTabControl.TabPages[2].Text = "FAS";
			mainTabControl.TabPages[3].Text = "Current leg";
			mainTabControl.TabPages[4].Text = "Legs";

			mainTabControl.SelectedIndex = 0;

			ListView01.Columns[0].Text = "Type";	//type
			ListView01.Columns[1].Text = "Name";	//name
			ListView01.Columns[2].Text = "H Surface" + " (" + Env.Current.UnitContext.UnitConverter.HeightUnit + ")";	//H Surface
			ListView01.Columns[3].Text = "Height above THR" + " (" + Env.Current.UnitContext.UnitConverter.HeightUnit + ")";	//H Abv. Tresh.
			ListView01.Columns[4].Text = "H penet." + " (" + Env.Current.UnitContext.UnitConverter.HeightUnit + ")";	//Penetrate
			ListView01.Columns[5].Text = "X (" + Env.Current.UnitContext.UnitConverter.HeightUnitM + ")";		//LoadResString() 'X
			ListView01.Columns[6].Text = "Y (" + Env.Current.UnitContext.UnitConverter.HeightUnitM + ")";		//LoadResString() 'Y
			ListView01.Columns[7].Text = "Surface";							//Surface

			ListView02.Columns[0].Text = "Type";	//type
			ListView02.Columns[1].Text = "Name";	//name
			ListView02.Columns[2].Text = "H Surface" + " (" + Env.Current.UnitContext.UnitConverter.HeightUnit + ")";	//H Surface
			ListView02.Columns[3].Text = "Height above THR" + " (" + Env.Current.UnitContext.UnitConverter.HeightUnit + ")";	//H Abv. Tresh.
			ListView02.Columns[4].Text = "Equvalent height (" + Env.Current.UnitContext.UnitConverter.HeightUnit + ")";	//Equvalent H
			ListView02.Columns[5].Text = "H penet." + " (" + Env.Current.UnitContext.UnitConverter.HeightUnit + ")";	//Penetrate
			ListView02.Columns[6].Text = "Req OCH" + " (" + Env.Current.UnitContext.UnitConverter.HeightUnit + ")";	//Req. OCH
			ListView02.Columns[7].Text = "X (" + Env.Current.UnitContext.UnitConverter.HeightUnitM + ")";	//LoadResString() 'X
			ListView02.Columns[8].Text = "Y (" + Env.Current.UnitContext.UnitConverter.HeightUnitM + ")";	//LoadResString() 'Y
			ListView02.Columns[9].Text = "Surface";	//Surface

			ListView03.Columns[0].Text = "Type";	//type
			ListView03.Columns[1].Text = "Name";	//name
			ListView03.Columns[2].Text = "H Surface" + " (" + Env.Current.UnitContext.UnitConverter.HeightUnit + ")";	//H Surface
			ListView03.Columns[3].Text = "Height above THR" + " (" + Env.Current.UnitContext.UnitConverter.HeightUnit + ")";	//H Abv. Tresh.
			ListView03.Columns[4].Text = "H penet." + " (" + Env.Current.UnitContext.UnitConverter.HeightUnit + ")"; //Penetrate
			ListView03.Columns[5].Text = "X (" + Env.Current.UnitContext.UnitConverter.HeightUnitM + ")";    //LoadResString() 'X


			//ListView04.Columns[0].Text = "Type";	//type
			//ListView04.Columns[1].Text = "Name";	//name
			//ListView04.Columns[2].Text = "Height" + " (" + Env.Current.UnitContext.UnitConverter.HeightUnit + ")";	//height
			//ListView04.Columns[3].Text = "MOC" + " (" + Env.Current.UnitContext.UnitConverter.HeightUnit + ")";	//moc
			//ListView04.Columns[4].Text = "Req. H" + " (" + Env.Current.UnitContext.UnitConverter.HeightUnit + ")";	//req h
			//ListView04.Columns[5].Text = "Area";	//area
			//ListView04.Columns[6].Text = "X from FAP (" + Env.Current.UnitContext.UnitConverter.HeightUnitM + ")";	//
			//ListView04.Columns[7].Text = "Y (" + Env.Current.UnitContext.UnitConverter.HeightUnitM + ")";	//LoadResString() 'Y

			//ListView05.Columns[0].Text = "Type";	//type
			//ListView05.Columns[1].Text = "Name";	//name
			//ListView05.Columns[2].Text = "H Surface" + " (" + Env.Current.UnitContext.UnitConverter.HeightUnit + ")";	//H Surface
			//ListView05.Columns[3].Text = "Height above THR" + " (" + Env.Current.UnitContext.UnitConverter.HeightUnit + ")";	//H Abv. Tresh.
			//ListView05.Columns[4].Text = "Req OCH" + " (" + Env.Current.UnitContext.UnitConverter.HeightUnit + ")";	//Req. OCH
			//ListView05.Columns[5].Text = "X (" + Env.Current.UnitContext.UnitConverter.HeightUnitM + ")";	//LoadResString() X
			//ListView05.Columns[6].Text = "Y (" + Env.Current.UnitContext.UnitConverter.HeightUnitM + ")";	//LoadResString() 'Y
			//ListView05.Columns[7].Text = "Surface";	//Surface
			//ListView05.Columns[8].Text = "Category";	//SBAS Category
			//ListView05.Columns[9].Text = "Area";	//Area

			//ListView06.Columns[0].Text = "Type";	//type
			//ListView06.Columns[1].Text = "Name";	//Name
			//ListView06.Columns[2].Text = "Height" + " (" + Env.Current.UnitContext.UnitConverter.HeightUnit + ")";	//Height
			//ListView06.Columns[3].Text = "MOC" + " (" + Env.Current.UnitContext.UnitConverter.HeightUnit + ")";	//MOC
			//ListView06.Columns[4].Text = "Req. H" + " (" + Env.Current.UnitContext.UnitConverter.HeightUnit + ")";	//Req.H
			//ListView06.Columns[5].Text = "Req OCH" + " (" + Env.Current.UnitContext.UnitConverter.HeightUnit + ")";	//Req OCH
			//ListView06.Columns[6].Text = "H penet." + " (" + Env.Current.UnitContext.UnitConverter.HeightUnit + ")";	//hPnet
			//ListView06.Columns[7].Text = "X (" + Env.Current.UnitContext.UnitConverter.HeightUnitM + ")";	//LoadResString() 'X
			//ListView06.Columns[8].Text = "Avoid Dist" + " (" + Env.Current.UnitContext.UnitConverter.DistanceUnit + ")";	//Avoid Dist
			//ListView06.Columns[9].Text = "Avoid angle" + " (°)";	//Avoid angle
			//ListView06.Columns[10].Text = "Area";	//Area

			dataGridView07.Columns[0].HeaderText = "Type";
			dataGridView07.Columns[1].HeaderText = "Name";
			dataGridView07.Columns[2].HeaderText = "Height above THR" + " (" + Env.Current.UnitContext.UnitConverter.HeightUnit+")";
			dataGridView07.Columns[3].HeaderText = "Elevation" + " (" + Env.Current.UnitContext.UnitConverter.HeightUnit + ")";
			dataGridView07.Columns[4].HeaderText = "MOC" + " (" + Env.Current.UnitContext.UnitConverter.HeightUnit + ")";
			dataGridView07.Columns[5].HeaderText = "Req. H" + " (" + Env.Current.UnitContext.UnitConverter.HeightUnit+")";
			//dataGridView07.Columns[2].HeaderText = "d0"+" (" + Env.Current.UnitContext.UnitConverter.DistanceUnit + ")";
			//dataGridView07.Columns[3].HeaderText = "d0Sum" + " (" + Env.Current.UnitContext.UnitConverter.DistanceUnit+")";
			//dataGridView07.Columns[8].HeaderText = "H penet." + " (" + Env.Current.UnitContext.UnitConverter.HeightUnit + ")";
			//dataGridView07.Columns[9].HeaderText = "Area";

			dataGridView08.Columns[0].HeaderText = "Type";
			dataGridView08.Columns[1].HeaderText = "Name";
			dataGridView08.Columns[2].HeaderText = "Height above THR" + " (" + Env.Current.UnitContext.UnitConverter.HeightUnit + ")";
			dataGridView08.Columns[3].HeaderText = "Elevation" + " (" + Env.Current.UnitContext.UnitConverter.HeightUnit + ")";
			dataGridView08.Columns[4].HeaderText = "MOC" + " (" + Env.Current.UnitContext.UnitConverter.HeightUnit+")";
			dataGridView08.Columns[5].HeaderText = "Req. H" + " (" + Env.Current.UnitContext.UnitConverter.HeightUnit + ")";
			//dataGridView08.Columns[2].HeaderText = "d0" + " (" + Env.Current.UnitContext.UnitConverter.DistanceUnit + ")";
			//dataGridView08.Columns[3].HeaderText = "d0Sum" + " (" + Env.Current.UnitContext.UnitConverter.DistanceUnit + ")";
			//dataGridView08.Columns[8].HeaderText = "H penet." + " (" + Env.Current.UnitContext.UnitConverter.HeightUnit + ")";
			//dataGridView08.Columns[9].HeaderText = "Area";

			_page08Obstacles = new ObstacleContainer[MaxLegsCount];
			_page08DetObs = new ObstacleData[MaxLegsCount];
		}

		private void RNP_ARReportForm_KeyUp(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.F1)
			{
				//NativeMethods.HtmlHelp(0, GlobalVars.HelpFile, GlobalVars.HH_HELP_CONTEXT, HelpContextID);
				//e.Handled = true;
			}
		}

		private void RNP_ARReportForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			Env.Current.AranGraphics.SafeDeleteGraphic(pPointElem);
			Env.Current.AranGraphics.SafeDeleteGraphic(pGeomElem);

			if (e.CloseReason == System.Windows.Forms.CloseReason.UserClosing)
			{
				Hide();
				ReportBtn.Checked = false;
				e.Cancel = true;
			}
		}

		private void RNP_ARReportForm_FormClosed(object sender, FormClosedEventArgs e)
		{
			_page01Obstacles.Parts = null;
			_page02Obstacles.Parts = null;
			_page03Obstacles.Parts = null;
			//_page04Obstacles.Parts = null;
			//_page05Obstacles.Parts = null;
			//_page06Obstacles.Parts = null;
			_page07Obstacles.Parts = null;
			//_page08Obstacles.Parts = null;

			_page01Obstacles.Obstacles = null;
			_page02Obstacles.Obstacles = null;
			_page03Obstacles.Obstacles = null;
			//_page04Obstacles.Obstacles = null;
			//_page05Obstacles.Obstacles = null;
			//_page06Obstacles.Obstacles = null;
			_page07Obstacles.Obstacles = null;
			//_page08Obstacles.Obstacles = null;
		}

		protected override void OnHandleCreated(EventArgs e)
		{
			base.OnHandleCreated(e);

			// Get a handle to a copy of this form's system (window) menu
			IntPtr hSysMenu = Functions.GetSystemMenu(this.Handle, false);
			// Add a separator
			Functions.AppendMenu(hSysMenu, Env.Current.SystemContext.MF_SEPARATOR, 0, string.Empty);
			// Add the About menu item
			Functions.AppendMenu(hSysMenu, Env.Current.SystemContext.MF_STRING, Env.Current.SystemContext.SYSMENU_ABOUT_ID, "&About…");
		}

		protected override void WndProc(ref Message m)
		{
			base.WndProc(ref m);

			if ((m.Msg == Env.Current.SystemContext.WM_SYSCOMMAND) && ((int)m.WParam == Env.Current.SystemContext.SYSMENU_ABOUT_ID))
			{
				AboutForm about = new AboutForm();
				about.ShowDialog(this);
				about = null;
			}
		}
		#endregion

		#region Utilities

		public void Init(CheckBox Btn)//, int HelpContext
		{
			ReportBtn = Btn;
			//_helpContextID = HelpContext;
		}

		public void SaveTabAsHTML(System.Windows.Forms.ListView lListView, string FileName)
		{
			if (lListView == null)
				return;

			int n, m, i, j;

			StreamWriter sw = File.CreateText(FileName);

			sw.WriteLine("<html>");
			sw.WriteLine("<head>");
			sw.WriteLine("<title>PANDA - " + Convert.ToChar(9) + mainTabControl.TabPages[mainTabControl.SelectedIndex].Text + "</title>");
			sw.WriteLine("<meta http-equiv=\"content-type\" content=\"text/html; charset=utf-8\" />");
			sw.WriteLine("<style>");
			sw.WriteLine("body {font-family: Arial, Sans-Serif; font-size:12;}");
			sw.WriteLine("table {font-family: Arial, Sans-Serif; font-size:10;}");
			sw.WriteLine("</style>");
			sw.WriteLine("</head>");
			sw.WriteLine("<body>");

			sw.WriteLine(System.Net.WebUtility.HtmlEncode(Text) + "<br>");
			sw.WriteLine(System.Net.WebUtility.HtmlEncode(Convert.ToChar(9) + mainTabControl.TabPages[mainTabControl.SelectedIndex].Text) + "<br>");
			sw.WriteLine("<br>");

			sw.WriteLine("<table border='1' cellspacing='0' cellpadding='1'>");

			n = lListView.Columns.Count;
			m = lListView.Items.Count;

			sw.WriteLine("<tr>");
			for (i = 0; i < n; i++)
				sw.WriteLine("<td><b>" + System.Net.WebUtility.HtmlEncode(lListView.Columns[i].Text) + "</b></td>");

			sw.WriteLine("</tr>");

			for (i = 0; i < m; i++)
			{
				System.Windows.Forms.ListViewItem row = lListView.Items[i];

				for (j = 0; j < n; j++)
					sw.WriteLine("<td>" + System.Net.WebUtility.HtmlEncode(row.SubItems[j].Text) + "</td>");

				sw.WriteLine("</tr>");
			}

			sw.WriteLine("</table>");
			sw.WriteLine("</body>");
			sw.WriteLine("</html>");

			sw.Flush();
			sw.Dispose();
			sw = null;
		}

		public void SaveTabAsTXT(System.Windows.Forms.ListView lListView, string FileName)
		{
			if (lListView == null)
				return;

			int i, n = lListView.Columns.Count, m, maxLen = 0;

			string[] headersText = new string[n];
			int[] headersLen = new int[n];

			StreamWriter sw = File.CreateText(FileName);

			sw.WriteLine(Text);
			sw.WriteLine(Convert.ToChar(9) + mainTabControl.TabPages[mainTabControl.SelectedIndex].Text);
			sw.WriteLine();


			for (i = 0; i < n; i++)
			{
				System.Windows.Forms.ColumnHeader columnHeader = lListView.Columns[i];
				headersText[i] = @"""" + columnHeader.Text + @"""";
				headersLen[i] = headersText[i].Length;
				if (headersLen[i] > maxLen)
					maxLen = headersLen[i];
			}

			string strOut = "", tmpStr;

			for (i = 0; i < n; i++)
			{
				int j = maxLen - headersLen[i];
				m = Convert.ToInt32(j / 2);
				tmpStr = "";

				if (i < n)
					tmpStr = String.Empty.PadLeft(m) + "|";

				strOut = strOut + String.Empty.PadLeft(j - m) + headersText[i] + tmpStr;
			}

			sw.WriteLine(strOut);
			strOut = "";

			tmpStr = new string('-', maxLen) + "+";
			for (i = 1; i < n; i++)
				strOut = strOut + tmpStr;

			strOut = strOut + new string('-', maxLen);

			sw.WriteLine(strOut);

			m = lListView.Items.Count;
			for (i = 0; i < m; i++)
			{
				System.Windows.Forms.ListViewItem row = lListView.Items[i];

				tmpStr = row.SubItems[0].Text;
				int tmpLen = tmpStr.Length;

				if (tmpLen > maxLen)
					strOut = tmpStr.Substring(0, maxLen - 1) + "*";
				else
					strOut = String.Empty.PadLeft(maxLen - tmpLen) + tmpStr;

				for (int j = 1; j < n; j++)
				{
					tmpStr = row.SubItems[j].Text;

					tmpLen = tmpStr.Length;

					if (tmpLen > maxLen)
						tmpStr = tmpStr.Substring(0, maxLen - 1) + "*";
					else if (j < n - 1 || tmpLen > 0)
						tmpStr = String.Empty.PadLeft(maxLen - tmpLen) + tmpStr;

					strOut = strOut + "|" + tmpStr;
				}
				sw.WriteLine(strOut);
			}

			sw.Flush();
			sw.Dispose();
			sw = null;
		}

		#endregion

		#region Another cobtrols
		private void CloseBtn_Click(object sender, EventArgs e)
		{
			Env.Current.AranGraphics.SafeDeleteGraphic(pPointElem);
			Env.Current.AranGraphics.SafeDeleteGraphic(pGeomElem);
			Hide();
			ReportBtn.Checked = false;
		}

		private void SaveBtn_Click(object sender, EventArgs e)
		{
			if (SaveDlg.ShowDialog() != DialogResult.OK)
				return;

			System.Windows.Forms.ListView lListView = null;
			switch (mainTabControl.SelectedIndex)
			{
				case 0:
					lListView = ListView01;
					break;
				case 1:
					lListView = ListView02;
					break;
				case 2:
					lListView = ListView02;
					break;
				case 3:
					lListView = ListView03;
					break;
				//case 4:
				//	lListView = ListView04;
				//	break;
				//case 5:
				//	lListView = ListView05;
				//	break;
				//case 6:
				//	lListView = ListView06;
				//	break;
				//case 7:
				//	//lListView = ListView07;
				//	break;
				//case 8:
				//	//lListView = ListView08;
				//	break;
				default:
					return;
			}

			bool bHtml;

			int pos = SaveDlg.FileName.LastIndexOf('.');

			if (pos <= 0)
				bHtml = SaveDlg.FilterIndex > 1;
			else
			{
				string sExt = SaveDlg.FileName.Substring(pos);
				bHtml = (sExt.ToUpper() == "HTM") || (sExt.ToUpper() == "HTML");
			}

			if (bHtml)
				SaveTabAsHTML(lListView, SaveDlg.FileName);
			else
				SaveTabAsTXT(lListView, SaveDlg.FileName);
		}

		private int _previousTab = -1;

		private void mainTabControl_SelectedIndexChanged(object sender, EventArgs e)
		{
			int SelectedTab = mainTabControl.SelectedIndex;

			numericUpDown1.Visible = SelectedTab == 7;

			if (SelectedTab == _previousTab)
				return;

			switch (SelectedTab)
			{
				case 0:
					ListViews_SelectedIndexChanged(ListView01, null);
					break;
				case 1:
					ListViews_SelectedIndexChanged(ListView02, null);
					break;
				case 2:
					ListViews_SelectedIndexChanged(ListView03, null);
					break;
				case 3:
				//	ListViews_SelectedIndexChanged(ListView04, null);
				//	break;
				//case 4:
				//	ListViews_SelectedIndexChanged(ListView05, null);
				//	break;
				//case 5:
				//	ListViews_SelectedIndexChanged(ListView06, null);
				//	break;
				//case 6:
					if (dataGridView07.SelectedRows.Count > 0)
						dataGridView3_RowEnter(dataGridView07, new DataGridViewCellEventArgs(0, dataGridView07.SelectedRows[0].Index));
					break;
				case 4:
					//if (numericUpDown1.Maximum == 1)
					//	FillPage08(0);

					if (dataGridView08.SelectedRows.Count > 0)
						dataGridView4_RowEnter(dataGridView08, new DataGridViewCellEventArgs(0, dataGridView08.SelectedRows[0].Index));
					break;
			}
			_previousTab = SelectedTab;
		}

		private void ListViews_SelectedIndexChanged(object sender, EventArgs e)
		{
			Env.Current.AranGraphics.SafeDeleteGraphic(pPointElem);
			Env.Current.AranGraphics.SafeDeleteGraphic(pGeomElem);
			System.Windows.Forms.ListView lListView = (System.Windows.Forms.ListView)sender;

			if (!this.Visible || lListView.SelectedItems.Count == 0 || lListView.Tag == null)
				return;

			System.Windows.Forms.ListViewItem Item = lListView.SelectedItems[0];
			if (Item == null)
				return;

			ObstacleContainer pageObstacles = (ObstacleContainer)lListView.Tag;
			if (pageObstacles.Parts.Length == 0  )
				return;

			int Index = pageObstacles.Parts[Item.Index].Owner;

			Geometry pGeometry;
			Aran.Geometries.Point pPtTmp;
			pGeometry = pageObstacles.Obstacles[Index].pGeomPrj;
			pPtTmp = pageObstacles.Parts[Item.Index].pPtPrj;

			if (pGeometry.Type == GeometryType.MultiLineString)
				pGeomElem = Env.Current.AranGraphics.DrawMultiLineString((MultiLineString)pGeometry, 2, 255);
			else if (pGeometry.Type == GeometryType.MultiPolygon)
				pGeomElem = Env.Current.AranGraphics.DrawMultiPolygon((MultiPolygon)pGeometry, AranEnvironment.Symbols.eFillStyle.sfsDiagonalCross, 255);

			pPointElem = Env.Current.AranGraphics.DrawPointWithText(pPtTmp, pageObstacles.Obstacles[Index].UnicalName, 255);
		}

		private void dataGridView_SortCompare(object sender, DataGridViewSortCompareEventArgs e)
		{
			switch (e.Column.Name[0])
			{
				case 'f':
					double f1 = double.Parse(e.CellValue1.ToString()), f2 = double.Parse(e.CellValue2.ToString());
					if (f1 > f2)
						e.SortResult = 1;
					else if (f1 < f2)
						e.SortResult = -1;
					else
						e.SortResult = 0;
					break;
				case 'i':
					int i1 = int.Parse(e.CellValue1.ToString()), i2 = int.Parse(e.CellValue2.ToString());
					if (i1 > i2)
						e.SortResult = 1;
					else if (i1 < i2)
						e.SortResult = -1;
					else
						e.SortResult = 0;
					break;
				//case 't':
				default:
					e.SortResult = System.String.Compare(e.CellValue1.ToString(), e.CellValue2.ToString());
					break;
			}

			e.Handled = true;
		}

		#endregion

		#region Page I

		public void FillPage01(ObstacleContainer Obstacles)
		{
			Env.Current.AranGraphics.SafeDeleteGraphic(pPointElem);
			Env.Current.AranGraphics.SafeDeleteGraphic(pGeomElem);

			int m = Obstacles.Obstacles.Length;
			int n = Obstacles.Parts.Length;

			_page01Obstacles.Obstacles = new Obstacle[m];
			_page01Obstacles.Parts = new ObstacleData[n];

			ListView01.BeginUpdate();

			ListView01.Items.Clear();

			if (n <= 0)
			{
				ListView01.EndUpdate();
				return;
			}

			int i;

			for (i = 0; i < m; i++)
				_page01Obstacles.Obstacles[i] = Obstacles.Obstacles[i];

			for (i = 0; i < n; i++)
			{
				_page01Obstacles.Parts[i] = Obstacles.Parts[i];
				System.Windows.Forms.ListViewItem itmX = ListView01.Items.Add(Obstacles.Obstacles[Obstacles.Parts[i].Owner].TypeName);
				itmX.SubItems.Insert(1, new System.Windows.Forms.ListViewItem.ListViewSubItem(null, Obstacles.Obstacles[Obstacles.Parts[i].Owner].UnicalName));
				itmX.SubItems.Insert(2, new System.Windows.Forms.ListViewItem.ListViewSubItem(null, Env.Current.UnitContext.UnitConverter.HeightToDisplayUnits(Obstacles.Parts[i].hSurface, eRoundMode.NEAREST).ToString()));
				itmX.SubItems.Insert(3, new System.Windows.Forms.ListViewItem.ListViewSubItem(null, Env.Current.UnitContext.UnitConverter.HeightToDisplayUnits(Obstacles.Parts[i].Height, eRoundMode.NEAREST).ToString()));
				itmX.SubItems.Insert(4, new System.Windows.Forms.ListViewItem.ListViewSubItem(null, Env.Current.UnitContext.UnitConverter.HeightToDisplayUnits(Obstacles.Parts[i].hPenet, eRoundMode.NEAREST).ToString()));
				itmX.SubItems.Insert(5, new System.Windows.Forms.ListViewItem.ListViewSubItem(null, Obstacles.Parts[i].Dist.ToString("0.0")));
				itmX.SubItems.Insert(6, new System.Windows.Forms.ListViewItem.ListViewSubItem(null, Obstacles.Parts[i].DistStar.ToString("0.0")));
				itmX.SubItems.Insert(7, new System.Windows.Forms.ListViewItem.ListViewSubItem(null, Env.Current.RNPContext.OFZPlaneNames[(int)Obstacles.Parts[i].Plane]));

				if (Obstacles.Parts[i].hPenet > 0.0)
				{
					itmX.Font = new Font(itmX.Font, FontStyle.Bold);
					itmX.ForeColor = Color.Red;
					for (int j = 0; j < itmX.SubItems.Count; j++)
					{
						itmX.SubItems[j].Font = itmX.Font;
						itmX.SubItems[j].ForeColor = Color.Red;
					}
				}
			}

			ListView01.Tag = _page01Obstacles;
			ListView01.EndUpdate();

			if (SortF01 != 0)
			{
				System.Windows.Forms.ColumnHeader pColumnHeader = ListView01.Columns[System.Math.Abs(SortF01) - 1];
				SortF01 = -SortF01;
				ListView01_ColumnClick(ListView01, new System.Windows.Forms.ColumnClickEventArgs(pColumnHeader.Index));
			}

			//SetVisible(9)
			mainTabControl.SelectedIndex = 0;
			if (ReportBtn.Checked && !this.Visible)
			    Show(Env.Current.SystemContext.Win32Window);
		}

		private void ListView01_ColumnClick(object sender, ColumnClickEventArgs e)
		{
			System.Windows.Forms.ColumnHeader ColumnHeader = ListView01.Columns[e.Column];

			ListView01.Sorting = SortOrder.None;
			int n = _page01Obstacles.Parts.Length;

			if (System.Math.Abs(SortF01) - 1 == ColumnHeader.Index)
				SortF01 = -SortF01;
			else
			{
				if (SortF01 != 0)
					ListView01.Columns[System.Math.Abs(SortF01) - 1].ImageIndex = 2;
				SortF01 = ColumnHeader.Index + 1;
			}

			if (SortF01 > 0)
				ColumnHeader.ImageIndex = 0;
			else
				ColumnHeader.ImageIndex = 1;

			int i;
			if (ColumnHeader.Index >= 2 && ColumnHeader.Index <= 6)
			{
				for (i = 0; i < n; i++)
					switch (ColumnHeader.Index)
					{
						case 2:
							_page01Obstacles.Parts[i].fSort = _page01Obstacles.Parts[i].hSurface;
							break;
						case 3:
							_page01Obstacles.Parts[i].fSort = _page01Obstacles.Parts[i].Height;
							break;
						case 4:
							_page01Obstacles.Parts[i].fSort = _page01Obstacles.Parts[i].hPenet;
							break;
						case 5:
							_page01Obstacles.Parts[i].fSort = _page01Obstacles.Parts[i].Dist;
							break;
						case 6:
							_page01Obstacles.Parts[i].fSort = _page01Obstacles.Parts[i].DistStar;
							break;
					}


				if (SortF01 > 0)
					Functions.shall_SortfSort(_page01Obstacles);
				else
					Functions.shall_SortfSortD(_page01Obstacles);
			}
			else
			{
				for (i = 0; i < n; i++)
					switch (ColumnHeader.Index)
					{
						case 0:
							_page01Obstacles.Parts[i].sSort = _page01Obstacles.Obstacles[_page01Obstacles.Parts[i].Owner].TypeName;
							break;
						case 1:
							_page01Obstacles.Parts[i].sSort = _page01Obstacles.Obstacles[_page01Obstacles.Parts[i].Owner].UnicalName;
							break;
						case 7:
							_page01Obstacles.Parts[i].sSort = Env.Current.RNPContext.OFZPlaneNames[(int)_page01Obstacles.Parts[i].Plane & 15];
							break;
					}

				if (SortF01 > 0)
					Functions.shall_SortsSort(_page01Obstacles);
				else
					Functions.shall_SortsSortD(_page01Obstacles);
			}

			ListView01.BeginUpdate();

			for (i = 0; i < n; i++)
			{
				System.Windows.Forms.ListViewItem itmX = ListView01.Items[i];

				itmX.Text = _page01Obstacles.Obstacles[_page01Obstacles.Parts[i].Owner].TypeName;
				itmX.SubItems[1].Text = _page01Obstacles.Obstacles[_page01Obstacles.Parts[i].Owner].UnicalName;
				itmX.SubItems[2].Text = Env.Current.UnitContext.UnitConverter.HeightToDisplayUnits(_page01Obstacles.Parts[i].hSurface, eRoundMode.NEAREST).ToString();
				itmX.SubItems[3].Text = Env.Current.UnitContext.UnitConverter.HeightToDisplayUnits(_page01Obstacles.Parts[i].Height, eRoundMode.NEAREST).ToString();
				itmX.SubItems[4].Text = Env.Current.UnitContext.UnitConverter.HeightToDisplayUnits(_page01Obstacles.Parts[i].hPenet, eRoundMode.NEAREST).ToString();
				itmX.SubItems[5].Text = _page01Obstacles.Parts[i].Dist.ToString("0.0");
				itmX.SubItems[6].Text = _page01Obstacles.Parts[i].DistStar.ToString("0.0");
				//itmX.SubItems[5].Text = Env.Current.UnitContext.UnitConverter.DistanceToDisplayUnits(LObstaclesPage01.Parts[i].Dist, eRoundMode.NERAEST).ToString();
				//itmX.SubItems[6].Text = Env.Current.UnitContext.UnitConverter.DistanceToDisplayUnits(LObstaclesPage01.Parts[i].DistStar, eRoundMode.NERAEST).ToString();

				itmX.SubItems[7].Text = Env.Current.RNPContext.OFZPlaneNames[(int)_page01Obstacles.Parts[i].Plane & 15];

				if (_page01Obstacles.Parts[i].hPenet > 0.0)
				{
					itmX.ForeColor = Color.Red;
					itmX.Font = new Font(itmX.Font, FontStyle.Bold);
				}
				else
				{
					itmX.ForeColor = Color.Black;
					itmX.Font = new Font(itmX.Font, FontStyle.Regular);
				}

				for (int j = 0; j < itmX.SubItems.Count; j++)
				{
					itmX.SubItems[j].ForeColor = itmX.ForeColor;
					itmX.SubItems[j].Font = itmX.Font;
				}
			}

			ListView01.EndUpdate();

			if (ReportBtn.Checked && Visible)
				ListViews_SelectedIndexChanged(ListView01, null);
		}
		#endregion

		#region Page II

		public void FillPage02(ObstacleContainer Obstacles)
		{
			Env.Current.AranGraphics.SafeDeleteGraphic(pPointElem);
			Env.Current.AranGraphics.SafeDeleteGraphic(pGeomElem);

			int m = Obstacles.Obstacles.Length;
			int n = Obstacles.Parts.Length;

			_page02Obstacles.Obstacles = new Obstacle[m];
			_page02Obstacles.Parts = new ObstacleData[n];

			ListView02.BeginUpdate();

			ListView02.Items.Clear();

			if (n <= 0)
			{
				ListView02.EndUpdate();
				return;
			}
			int i;

			for (i = 0; i < m; i++)
				_page02Obstacles.Obstacles[i] = Obstacles.Obstacles[i];

			for (i = 0; i < n; i++)
			{
				_page02Obstacles.Parts[i] = Obstacles.Parts[i];
				System.Windows.Forms.ListViewItem itmX = ListView02.Items.Add(Obstacles.Obstacles[Obstacles.Parts[i].Owner].TypeName);
				itmX.SubItems.Insert(1, new System.Windows.Forms.ListViewItem.ListViewSubItem(null, Obstacles.Obstacles[Obstacles.Parts[i].Owner].UnicalName));
				itmX.SubItems.Insert(2, new System.Windows.Forms.ListViewItem.ListViewSubItem(null, Env.Current.UnitContext.UnitConverter.HeightToDisplayUnits(Obstacles.Parts[i].hSurface, eRoundMode.NEAREST).ToString()));
				itmX.SubItems.Insert(3, new System.Windows.Forms.ListViewItem.ListViewSubItem(null, Env.Current.UnitContext.UnitConverter.HeightToDisplayUnits(Obstacles.Parts[i].Height, eRoundMode.NEAREST).ToString()));

				if ((int)Obstacles.Parts[i].Plane  == 2)
					itmX.SubItems.Insert(4, new System.Windows.Forms.ListViewItem.ListViewSubItem(null, Env.Current.UnitContext.UnitConverter.HeightToDisplayUnits(Obstacles.Parts[i].EffectiveHeight, eRoundMode.NEAREST).ToString()));
				else
					itmX.SubItems.Insert(4, new System.Windows.Forms.ListViewItem.ListViewSubItem(null, "-"));

				itmX.SubItems.Insert(5, new System.Windows.Forms.ListViewItem.ListViewSubItem(null, Env.Current.UnitContext.UnitConverter.HeightToDisplayUnits(Obstacles.Parts[i].hPenet, eRoundMode.NEAREST).ToString()));
				itmX.SubItems.Insert(6, new System.Windows.Forms.ListViewItem.ListViewSubItem(null, Env.Current.UnitContext.UnitConverter.HeightToDisplayUnits(Obstacles.Parts[i].ReqOCH, eRoundMode.NEAREST).ToString()));
				itmX.SubItems.Insert(7, new System.Windows.Forms.ListViewItem.ListViewSubItem(null, Obstacles.Parts[i].Dist.ToString("0.0")));
				itmX.SubItems.Insert(8, new System.Windows.Forms.ListViewItem.ListViewSubItem(null, Obstacles.Parts[i].DistStar.ToString("0.0")));
				itmX.SubItems.Insert(9, new System.Windows.Forms.ListViewItem.ListViewSubItem(null, Env.Current.RNPContext.RNPARPlaneNames[(int)Obstacles.Parts[i].Plane & 15]));

				if (Obstacles.Parts[i].hPenet > 0.0)
				{
					itmX.ForeColor = Color.Red;
					itmX.Font = new Font(itmX.Font, FontStyle.Bold);

					for (int j = 0; j < itmX.SubItems.Count; j++)
					{
						itmX.SubItems[j].ForeColor = Color.Red;
						itmX.SubItems[j].Font = itmX.Font;
					}
				}
			}

			ListView02.Tag = _page02Obstacles;
			ListView02.EndUpdate();

			if (SortF02 != 0)
			{
				System.Windows.Forms.ColumnHeader pColumnHeader = ListView02.Columns[System.Math.Abs(SortF02) - 1];
				SortF02 = -SortF02;
				ListView02_ColumnClick(ListView02, new System.Windows.Forms.ColumnClickEventArgs(pColumnHeader.Index));
			}

			//SetVisible (11)
			mainTabControl.SelectedIndex = 1;
			if (ReportBtn.Checked && !this.Visible)
			    Show(Env.Current.SystemContext.Win32Window);
        }

		private void ListView02_ColumnClick(object sender, ColumnClickEventArgs e)
		{
			System.Windows.Forms.ColumnHeader ColumnHeader = ListView02.Columns[e.Column];

			ListView02.Sorting = SortOrder.None;
			int n = _page02Obstacles.Parts.Length;

			if (System.Math.Abs(SortF02) - 1 == ColumnHeader.Index)
				SortF02 = -SortF02;
			else
			{
				if (SortF02 != 0)
					ListView02.Columns[System.Math.Abs(SortF02) - 1].ImageIndex = 2;
				SortF02 = ColumnHeader.Index + 1;
			}

			if (SortF02 > 0)
				ColumnHeader.ImageIndex = 0;
			else
				ColumnHeader.ImageIndex = 1;

			int i;
			if (ColumnHeader.Index >= 2 && ColumnHeader.Index <= 8)
			{
				for (i = 0; i < n; i++)
					switch (ColumnHeader.Index)
					{
						case 2:
							_page02Obstacles.Parts[i].fSort = _page02Obstacles.Parts[i].hSurface;
							break;
						case 3:
							_page02Obstacles.Parts[i].fSort = _page02Obstacles.Parts[i].Height;
							break;
						case 4:
							_page02Obstacles.Parts[i].fSort = _page02Obstacles.Parts[i].EffectiveHeight;
							break;
						case 5:
							_page02Obstacles.Parts[i].fSort = _page02Obstacles.Parts[i].hPenet;
							break;
						case 6:
							_page02Obstacles.Parts[i].fSort = _page02Obstacles.Parts[i].ReqOCH;
							break;
						case 7:
							_page02Obstacles.Parts[i].fSort = _page02Obstacles.Parts[i].Dist;
							break;
						case 8:
							_page02Obstacles.Parts[i].fSort = _page02Obstacles.Parts[i].DistStar;
							break;
					}

				if (SortF02 > 0)
					Functions.shall_SortfSort(_page02Obstacles);
				else
					Functions.shall_SortfSortD(_page02Obstacles);
			}
			else
			{
				for (i = 0; i < n; i++)
					switch (ColumnHeader.Index)
					{
						case 0:
							_page02Obstacles.Parts[i].sSort = _page02Obstacles.Obstacles[_page02Obstacles.Parts[i].Owner].TypeName;
							break;
						case 1:
							_page02Obstacles.Parts[i].sSort = _page02Obstacles.Obstacles[_page02Obstacles.Parts[i].Owner].UnicalName;
							break;
						case 9:
							_page02Obstacles.Parts[i].sSort = Env.Current.RNPContext.RNPARPlaneNames[(int)_page02Obstacles.Parts[i].Plane & 15];
							break;
					}

				if (SortF02 > 0)
					Functions.shall_SortsSort(_page02Obstacles);
				else
					Functions.shall_SortsSortD(_page02Obstacles);
			}

			ListView02.BeginUpdate();

			for (i = 0; i < n; i++)
			{
				System.Windows.Forms.ListViewItem itmX = ListView02.Items[i];
				itmX.Text = _page02Obstacles.Obstacles[_page02Obstacles.Parts[i].Owner].TypeName;
				itmX.SubItems[1].Text = _page02Obstacles.Obstacles[_page02Obstacles.Parts[i].Owner].UnicalName;
				itmX.SubItems[2].Text = Env.Current.UnitContext.UnitConverter.HeightToDisplayUnits(_page02Obstacles.Parts[i].hSurface, eRoundMode.NEAREST).ToString();
				itmX.SubItems[3].Text = Env.Current.UnitContext.UnitConverter.HeightToDisplayUnits(_page02Obstacles.Parts[i].Height, eRoundMode.NEAREST).ToString();

				if ((int)_page02Obstacles.Parts[i].Plane == 2)
					itmX.SubItems.Insert(4, new System.Windows.Forms.ListViewItem.ListViewSubItem(null, Env.Current.UnitContext.UnitConverter.HeightToDisplayUnits(_page02Obstacles.Parts[i].EffectiveHeight, eRoundMode.NEAREST).ToString()));
				else
					itmX.SubItems.Insert(4, new System.Windows.Forms.ListViewItem.ListViewSubItem(null, "-"));

				itmX.SubItems[5].Text = Env.Current.UnitContext.UnitConverter.HeightToDisplayUnits(_page02Obstacles.Parts[i].hPenet, eRoundMode.NEAREST).ToString();
				itmX.SubItems[6].Text = Env.Current.UnitContext.UnitConverter.HeightToDisplayUnits(_page02Obstacles.Parts[i].ReqOCH, eRoundMode.NEAREST).ToString();
				itmX.SubItems[7].Text = _page02Obstacles.Parts[i].Dist.ToString("0.0");
				itmX.SubItems[8].Text = _page02Obstacles.Parts[i].DistStar.ToString("0.0");
				itmX.SubItems[9].Text = Env.Current.RNPContext.RNPARPlaneNames[(int)_page02Obstacles.Parts[i].Plane & 15];

				if (_page02Obstacles.Parts[i].hPenet > 0.0)
				{
					itmX.Font = new Font(itmX.Font, FontStyle.Bold);
					itmX.ForeColor = Color.Red;
				}
				else
				{
					itmX.Font = new Font(itmX.Font, FontStyle.Regular);
					itmX.ForeColor = Color.Black;
				}

				for (int j = 0; j < itmX.SubItems.Count; j++)
				{
					itmX.SubItems[j].Font = itmX.Font;
					itmX.SubItems[j].ForeColor = itmX.ForeColor;
				}
			}

			ListView02.EndUpdate();

			if (ReportBtn.Checked && Visible)
				ListViews_SelectedIndexChanged(ListView02, null);
		}
		#endregion

		#region Page III

		public void FillPage03(ObstacleContainer Obstacles)
		{
			int m = Obstacles.Obstacles.Length;
			int n = Obstacles.Parts.Length;

			_page03Obstacles.Obstacles = new Obstacle[m];
			_page03Obstacles.Parts = new ObstacleData[n];

			Array.Copy(Obstacles.Obstacles, _page03Obstacles.Obstacles, m);
			Array.Copy(Obstacles.Parts, _page03Obstacles.Parts, n);

			ListView03.Tag = _page03Obstacles;

			ListView03.BeginUpdate();
			ListView03.Items.Clear();
			ListView03.EndUpdate();

			if (SortF03 == 0)
				SortF03 = 2;
			else
				SortF03 = -SortF03;

			System.Windows.Forms.ColumnHeader pColumnHeader = ListView03.Columns[System.Math.Abs(SortF03) - 1];
			ListView03_ColumnClick(ListView03, new System.Windows.Forms.ColumnClickEventArgs(pColumnHeader.Index));

			//SetVisible (11)
			mainTabControl.SelectedIndex = 2;

		    if (ReportBtn.Checked && !this.Visible)
		        Show(Env.Current.SystemContext.Win32Window);

        }

		private void ListView03_ColumnClick(object sender, ColumnClickEventArgs e)
		{
			System.Windows.Forms.ColumnHeader ColumnHeader = ListView03.Columns[e.Column];

			ListView03.Sorting = SortOrder.None;
			int n = _page03Obstacles.Parts.Length;

			if (System.Math.Abs(SortF03) - 1 == ColumnHeader.Index)
				SortF03 = -SortF03;
			else
			{
				if (SortF03 != 0)
					ListView03.Columns[System.Math.Abs(SortF03) - 1].ImageIndex = 2;
				SortF03 = ColumnHeader.Index + 1;
			}

			if (SortF03 > 0)
				ColumnHeader.ImageIndex = 0;
			else
				ColumnHeader.ImageIndex = 1;

			int i;
			if (ColumnHeader.Index >= 2 )
			{
				for (i = 0; i < n; i++)
					switch (ColumnHeader.Index)
					{
						case 2:
							_page03Obstacles.Parts[i].fSort = _page03Obstacles.Parts[i].hSurface;
							break;
						case 3:
							_page03Obstacles.Parts[i].fSort = _page03Obstacles.Parts[i].Height;
							break;
						case 4:
							_page03Obstacles.Parts[i].fSort = _page03Obstacles.Parts[i].hPenet;
							break;
						case 5:
							_page03Obstacles.Parts[i].fSort = _page03Obstacles.Parts[i].Dist;
							break;
					}

				if (SortF03 > 0)
					Functions.shall_SortfSort(_page03Obstacles);
				else
					Functions.shall_SortfSortD(_page03Obstacles);
			}
			else
			{
				for (i = 0; i < n; i++)
					switch (ColumnHeader.Index)
					{
						case 0:
							_page03Obstacles.Parts[i].sSort = _page03Obstacles.Obstacles[_page03Obstacles.Parts[i].Owner].TypeName;
							break;
						case 1:
							_page03Obstacles.Parts[i].sSort = _page03Obstacles.Obstacles[_page03Obstacles.Parts[i].Owner].UnicalName;
							break;
					}

				if (SortF03 > 0)
					Functions.shall_SortsSort(_page03Obstacles);
				else
					Functions.shall_SortsSortD(_page03Obstacles);
			}


			int m = ListView03.Items.Count;

			ListView03.BeginUpdate();
			for (i = n + 1; i < m; i++)
				ListView03.Items.RemoveAt(0);

			System.Windows.Forms.ListViewItem itmX;
			for (i = 0; i < n; i++)
			{
				if (ListView03.Items.Count <= i)
					itmX = ListView03.Items.Add(_page03Obstacles.Obstacles[_page03Obstacles.Parts[i].Owner].TypeName);
				else
				{
					itmX = ListView03.Items[i];
					itmX.Text = _page03Obstacles.Obstacles[_page03Obstacles.Parts[i].Owner].TypeName;
				}

				if (itmX.SubItems.Count > 1)
					itmX.SubItems[1].Text = _page03Obstacles.Obstacles[_page03Obstacles.Parts[i].Owner].UnicalName;
				else
					itmX.SubItems.Insert(1, new System.Windows.Forms.ListViewItem.ListViewSubItem(null, _page03Obstacles.Obstacles[_page03Obstacles.Parts[i].Owner].UnicalName));

				if (itmX.SubItems.Count > 2)
					itmX.SubItems[2].Text = Env.Current.UnitContext.UnitConverter.HeightToDisplayUnits(_page03Obstacles.Parts[i].hSurface, eRoundMode.NEAREST).ToString();
				else
					itmX.SubItems.Insert(2, new System.Windows.Forms.ListViewItem.ListViewSubItem(null, Env.Current.UnitContext.UnitConverter.HeightToDisplayUnits(_page03Obstacles.Parts[i].hSurface, eRoundMode.NEAREST).ToString()));

				if (itmX.SubItems.Count > 3)
					itmX.SubItems[3].Text = Env.Current.UnitContext.UnitConverter.HeightToDisplayUnits(_page03Obstacles.Parts[i].Height, eRoundMode.NEAREST).ToString();
				else
					itmX.SubItems.Insert(3, new System.Windows.Forms.ListViewItem.ListViewSubItem(null, Env.Current.UnitContext.UnitConverter.HeightToDisplayUnits(_page03Obstacles.Parts[i].Height, eRoundMode.NEAREST).ToString()));

				if (itmX.SubItems.Count > 4)
					itmX.SubItems[4].Text = Env.Current.UnitContext.UnitConverter.HeightToDisplayUnits(_page03Obstacles.Parts[i].hPenet, eRoundMode.NEAREST).ToString();
				else
					itmX.SubItems.Insert(4, new System.Windows.Forms.ListViewItem.ListViewSubItem(null, Env.Current.UnitContext.UnitConverter.HeightToDisplayUnits(_page03Obstacles.Parts[i].hPenet, eRoundMode.NEAREST).ToString()));

				if (itmX.SubItems.Count > 5)
					itmX.SubItems[5].Text = _page03Obstacles.Parts[i].Dist.ToString("0.0"); 
				else
					itmX.SubItems.Insert(5, new System.Windows.Forms.ListViewItem.ListViewSubItem(null, _page03Obstacles.Parts[i].Dist.ToString("0.0")));

				if (_page03Obstacles.Parts[i].hPenet > 0.0)
				{
					itmX.Font = new Font(itmX.Font, FontStyle.Bold);
					itmX.ForeColor = Color.Red;
				}
				else
				{
					itmX.Font = new Font(itmX.Font, FontStyle.Regular);
					itmX.ForeColor = Color.Black;
				}

				for (int j = 0; j < itmX.SubItems.Count; j++)
				{
					itmX.SubItems[j].Font = itmX.Font;
					itmX.SubItems[j].ForeColor = itmX.ForeColor;
				}
			}

			ListView03.EndUpdate();
			if (ReportBtn.Checked && Visible)
				ListViews_SelectedIndexChanged(ListView03, null);
		}

		#endregion

		//#region Page IV

		//public void FillPage04(ObstacleContainer Obstacles)
		//{
		//	int m = Obstacles.Obstacles.Length;
		//	int n = Obstacles.Parts.Length;

		//	_page04Obstacles.Obstacles = new Obstacle[m];
		//	_page04Obstacles.Parts = new ObstacleData[n];

		//	Env.Current.AranGraphics.SafeDeleteGraphic(pPointElem);
		//	Env.Current.AranGraphics.SafeDeleteGraphic(pGeomElem);
		//	ListView04.Items.Clear();

		//	if (n <= 0)
		//		return;

		//	int i;

		//	for (i = 0; i < m; i++)
		//		_page04Obstacles.Obstacles[i] = Obstacles.Obstacles[i];

		//	for (i = 0; i < n; i++)
		//	{
		//		_page04Obstacles.Parts[i] = Obstacles.Parts[i];
		//		System.Windows.Forms.ListViewItem itmX = ListView04.Items.Add(Obstacles.Obstacles[Obstacles.Parts[i].Owner].TypeName);
		//		itmX.SubItems.Insert(1, new System.Windows.Forms.ListViewItem.ListViewSubItem(null, Obstacles.Obstacles[Obstacles.Parts[i].Owner].UnicalName));
		//		itmX.SubItems.Insert(2, new System.Windows.Forms.ListViewItem.ListViewSubItem(null, Env.Current.UnitContext.UnitConverter.HeightToDisplayUnits(Obstacles.Parts[i].Height, eRoundMode.NERAEST).ToString()));
		//		itmX.SubItems.Insert(3, new System.Windows.Forms.ListViewItem.ListViewSubItem(null, Env.Current.UnitContext.UnitConverter.HeightToDisplayUnits(Obstacles.Parts[i].MOC, eRoundMode.NERAEST).ToString()));
		//		itmX.SubItems.Insert(4, new System.Windows.Forms.ListViewItem.ListViewSubItem(null, Env.Current.UnitContext.UnitConverter.HeightToDisplayUnits(Obstacles.Parts[i].ReqH, eRoundMode.NERAEST).ToString()));

		//		if (Obstacles.Parts[i].Prima)
		//			itmX.SubItems.Insert(5, new System.Windows.Forms.ListViewItem.ListViewSubItem(null, "Primary"));
		//		else
		//			itmX.SubItems.Insert(5, new System.Windows.Forms.ListViewItem.ListViewSubItem(null, "Secondary"));

		//		itmX.SubItems.Insert(6, new System.Windows.Forms.ListViewItem.ListViewSubItem(null, Obstacles.Parts[i].Dist.ToString("0.0")));
		//		itmX.SubItems.Insert(7, new System.Windows.Forms.ListViewItem.ListViewSubItem(null, Obstacles.Parts[i].DistStar.ToString("0.0")));

		//		//itmX.SubItems.Insert(6, new System.Windows.Forms.ListViewItem.ListViewSubItem(null, CStr(ConvertDistance(Obstacles.Parts[i].Dist, eRoundMode.NERAEST))))
		//		//itmX.SubItems.Insert(7, new System.Windows.Forms.ListViewItem.ListViewSubItem(null, CStr(ConvertDistance(Obstacles.Parts[i].DistStar, eRoundMode.NERAEST))))
		//	}

		//	ListView04.Tag = _page04Obstacles;

		//	if (SortF04 != 0)
		//	{
		//		System.Windows.Forms.ColumnHeader pColumnHeader = ListView04.Columns[System.Math.Abs(SortF04) - 1];
		//		SortF04 = -SortF04;
		//		ListView04_ColumnClick(ListView04, new System.Windows.Forms.ColumnClickEventArgs(pColumnHeader.Index));
		//	}

		//	//SetVisible(3);
		//	mainTabControl.SelectedIndex = 3;
		//	if (ReportBtn.Checked && !this.Visible)
		//		Show(GlobalVars.Win32Window);
		//}

		//private void ListView04_ColumnClick(object sender, ColumnClickEventArgs e)
		//{
		//	System.Windows.Forms.ColumnHeader ColumnHeader = ListView04.Columns[e.Column];

		//	ListView04.Sorting = SortOrder.None;
		//	int n = _page04Obstacles.Parts.Length;

		//	if (System.Math.Abs(SortF04) - 1 == ColumnHeader.Index)
		//		SortF04 = -SortF04;
		//	else
		//	{
		//		if (SortF04 != 0)
		//			ListView04.Columns[System.Math.Abs(SortF04) - 1].ImageIndex = 2;
		//		SortF04 = ColumnHeader.Index + 1;
		//	}

		//	if (SortF04 > 0)
		//		ColumnHeader.ImageIndex = 0;
		//	else
		//		ColumnHeader.ImageIndex = 1;

		//	int i;
		//	if (ColumnHeader.Index != 5 && ColumnHeader.Index >= 2 && ColumnHeader.Index <= 7)
		//	{
		//		for (i = 0; i < n; i++)
		//			switch (ColumnHeader.Index)
		//			{
		//				case 2:
		//					_page04Obstacles.Parts[i].fSort = _page04Obstacles.Parts[i].Height;
		//					break;
		//				case 3:
		//					_page04Obstacles.Parts[i].fSort = _page04Obstacles.Parts[i].MOC;
		//					break;
		//				case 4:
		//					_page04Obstacles.Parts[i].fSort = _page04Obstacles.Parts[i].ReqH;
		//					break;
		//				case 6:
		//					_page04Obstacles.Parts[i].fSort = _page04Obstacles.Parts[i].Dist;
		//					break;
		//				case 7:
		//					_page04Obstacles.Parts[i].fSort = _page04Obstacles.Parts[i].DistStar;
		//					break;
		//			}

		//		if (SortF04 > 0)
		//			Functions.shall_SortfSort(_page04Obstacles);
		//		else
		//			Functions.shall_SortfSortD(_page04Obstacles);
		//	}
		//	else
		//	{
		//		for (i = 0; i < n; i++)
		//			switch (ColumnHeader.Index)
		//			{
		//				case 0:
		//					_page04Obstacles.Parts[i].sSort = _page04Obstacles.Obstacles[_page04Obstacles.Parts[i].Owner].TypeName;
		//					break;
		//				case 1:
		//					_page04Obstacles.Parts[i].sSort = _page04Obstacles.Obstacles[_page04Obstacles.Parts[i].Owner].UnicalName;
		//					break;
		//				case 5:
		//					if (_page04Obstacles.Parts[i].Prima)
		//						_page04Obstacles.Parts[i].sSort = "Primary";
		//					else
		//						_page04Obstacles.Parts[i].sSort = "Secondary";
		//					break;
		//			}

		//		if (SortF04 > 0)
		//			Functions.shall_SortsSort(_page04Obstacles);
		//		else
		//			Functions.shall_SortsSortD(_page04Obstacles);
		//	}

		//	for (i = 0; i < n; i++)
		//	{
		//		System.Windows.Forms.ListViewItem itmX = ListView04.Items[i];

		//		itmX.Text = _page04Obstacles.Obstacles[_page04Obstacles.Parts[i].Owner].TypeName;
		//		itmX.SubItems[1].Text = _page04Obstacles.Obstacles[_page04Obstacles.Parts[i].Owner].UnicalName;
		//		itmX.SubItems[2].Text = Env.Current.UnitContext.UnitConverter.HeightToDisplayUnits(_page04Obstacles.Parts[i].Height, eRoundMode.NERAEST).ToString();
		//		itmX.SubItems[3].Text = Env.Current.UnitContext.UnitConverter.HeightToDisplayUnits(_page04Obstacles.Parts[i].MOC, eRoundMode.NERAEST).ToString();
		//		itmX.SubItems[4].Text = Env.Current.UnitContext.UnitConverter.HeightToDisplayUnits(_page04Obstacles.Parts[i].ReqH, eRoundMode.NERAEST).ToString();

		//		if (_page04Obstacles.Parts[i].Prima)
		//			itmX.SubItems[5].Text = "Primary";
		//		else
		//			itmX.SubItems[5].Text = "Secondary";

		//		itmX.SubItems[6].Text = _page04Obstacles.Parts[i].Dist.ToString("0.0");
		//		itmX.SubItems[7].Text = _page04Obstacles.Parts[i].DistStar.ToString("0.0");
		//	}

		//	if (ReportBtn.Checked && Visible)
		//		ListViews_SelectedIndexChanged(ListView04, null);
		//}

		//#endregion

		//#region Page V

		//public void FillPage05(ObstacleContainer Obstacles)
		//{
		//	int m = Obstacles.Obstacles.Length;
		//	int n = Obstacles.Parts.Length;

		//	_page05Obstacles.Obstacles = new Obstacle[m];
		//	_page05Obstacles.Parts = new ObstacleData[n];

		//	Array.Copy(Obstacles.Obstacles, _page05Obstacles.Obstacles, m);
		//	Array.Copy(Obstacles.Parts, _page05Obstacles.Parts, n);

		//	ListView05.Tag = _page05Obstacles;

		//	ListView05.Items.Clear();

		//	if (SortF05 == 0)
		//		SortF05 = 6;
		//	else
		//		SortF05 = -SortF05;

		//	System.Windows.Forms.ColumnHeader pColumnHeader = ListView05.Columns[System.Math.Abs(SortF05) - 1];
		//	ListView05_ColumnClick(ListView05, new System.Windows.Forms.ColumnClickEventArgs(pColumnHeader.Index));

		//	//SetVisible (11)
		//	mainTabControl.SelectedIndex = 4;

		//	if (ReportBtn.Checked && !this.Visible)
		//		Show(GlobalVars.Win32Window);
		//}

		//private void ListView05_ColumnClick(object sender, ColumnClickEventArgs e)
		//{
		//	System.Windows.Forms.ColumnHeader ColumnHeader = ListView05.Columns[e.Column];

		//	ListView05.Sorting = SortOrder.None;
		//	int n = _page05Obstacles.Parts.Length;

		//	if (System.Math.Abs(SortF05) - 1 == ColumnHeader.Index)
		//		SortF05 = -SortF05;
		//	else
		//	{
		//		if (SortF05 != 0)
		//			ListView05.Columns[System.Math.Abs(SortF05) - 1].ImageIndex = 2;
		//		SortF05 = ColumnHeader.Index + 1;
		//	}

		//	if (SortF05 > 0)
		//		ColumnHeader.ImageIndex = 0;
		//	else
		//		ColumnHeader.ImageIndex = 1;

		//	int i;
		//	string[] ZoneNames = new string[] { "Approach", "Missed approach" };
		//	//ZoneNames = new string[] {"No", "Yes"};

		//	if (ColumnHeader.Index >= 2 && ColumnHeader.Index < 7)
		//	{
		//		for (i = 0; i < n; i++)
		//			switch (ColumnHeader.Index)
		//			{
		//				case 2:
		//					_page05Obstacles.Parts[i].fSort = _page05Obstacles.Parts[i].hSurface;
		//					break;
		//				case 3:
		//					_page05Obstacles.Parts[i].fSort = _page05Obstacles.Parts[i].Height;
		//					break;
		//				case 4:
		//					_page05Obstacles.Parts[i].fSort = _page05Obstacles.Parts[i].ReqOCH;
		//					break;
		//				case 5:
		//					_page05Obstacles.Parts[i].fSort = _page05Obstacles.Parts[i].Dist;
		//					break;
		//				case 6:
		//					_page05Obstacles.Parts[i].fSort = _page05Obstacles.Parts[i].DistStar;
		//					break;
		//			}

		//		if (SortF05 > 0)
		//			Functions.shall_SortfSort(_page05Obstacles);
		//		else
		//			Functions.shall_SortfSortD(_page05Obstacles);
		//	}
		//	else
		//	{
		//		for (i = 0; i < n; i++)
		//			switch (ColumnHeader.Index)
		//			{
		//				case 0:
		//					_page05Obstacles.Parts[i].sSort = _page05Obstacles.Obstacles[_page05Obstacles.Parts[i].Owner].TypeName;
		//					break;
		//				case 1:
		//					_page05Obstacles.Parts[i].sSort = _page05Obstacles.Obstacles[_page05Obstacles.Parts[i].Owner].UnicalName;
		//					break;
		//				case 7:
		//					_page05Obstacles.Parts[i].sSort = GlobalVars.OASPlaneNames[(int)_page05Obstacles.Parts[i].Plane & 15];
		//					break;
		//				case 8:
		//					if (((int)_page05Obstacles.Parts[i].Plane & 32) != 0)
		//						_page05Obstacles.Parts[i].sSort = "Cat 2";
		//					else
		//						_page05Obstacles.Parts[i].sSort = "Cat 1";
		//					break;
		//				case 9:
		//					_page05Obstacles.Parts[i].sSort = ZoneNames[_page05Obstacles.Parts[i].Flags];
		//					break;
		//			}

		//		if (SortF05 > 0)
		//			Functions.shall_SortsSort(_page05Obstacles);
		//		else
		//			Functions.shall_SortsSortD(_page05Obstacles);
		//	}

		//	int m = ListView05.Items.Count;
		//	for (i = n + 1; i < m; i++)
		//		ListView05.Items.RemoveAt(0);

		//	System.Windows.Forms.ListViewItem itmX;
		//	for (i = 0; i < n; i++)
		//	{
		//		if (ListView05.Items.Count <= i)
		//			itmX = ListView05.Items.Add(_page05Obstacles.Obstacles[_page05Obstacles.Parts[i].Owner].TypeName);
		//		else
		//		{
		//			itmX = ListView05.Items[i];
		//			itmX.Text = _page05Obstacles.Obstacles[_page05Obstacles.Parts[i].Owner].TypeName;
		//		}

		//		if (itmX.SubItems.Count > 1)
		//			itmX.SubItems[1].Text = _page05Obstacles.Obstacles[_page05Obstacles.Parts[i].Owner].UnicalName;
		//		else
		//			itmX.SubItems.Insert(1, new System.Windows.Forms.ListViewItem.ListViewSubItem(null, _page05Obstacles.Obstacles[_page05Obstacles.Parts[i].Owner].UnicalName));

		//		if (itmX.SubItems.Count > 2)
		//			itmX.SubItems[2].Text = Env.Current.UnitContext.UnitConverter.HeightToDisplayUnits(_page05Obstacles.Parts[i].hSurface, eRoundMode.NERAEST).ToString();
		//		else
		//			itmX.SubItems.Insert(2, new System.Windows.Forms.ListViewItem.ListViewSubItem(null, Env.Current.UnitContext.UnitConverter.HeightToDisplayUnits(_page05Obstacles.Parts[i].hSurface, eRoundMode.NERAEST).ToString()));

		//		if (itmX.SubItems.Count > 3)
		//			itmX.SubItems[3].Text = Env.Current.UnitContext.UnitConverter.HeightToDisplayUnits(_page05Obstacles.Parts[i].Height, eRoundMode.NERAEST).ToString();
		//		else
		//			itmX.SubItems.Insert(3, new System.Windows.Forms.ListViewItem.ListViewSubItem(null, Env.Current.UnitContext.UnitConverter.HeightToDisplayUnits(_page05Obstacles.Parts[i].Height, eRoundMode.NERAEST).ToString()));

		//		if (itmX.SubItems.Count > 4)
		//			itmX.SubItems[4].Text = Env.Current.UnitContext.UnitConverter.HeightToDisplayUnits(_page05Obstacles.Parts[i].ReqOCH, eRoundMode.NERAEST).ToString();
		//		else
		//			itmX.SubItems.Insert(4, new System.Windows.Forms.ListViewItem.ListViewSubItem(null, Env.Current.UnitContext.UnitConverter.HeightToDisplayUnits(_page05Obstacles.Parts[i].ReqOCH, eRoundMode.NERAEST).ToString()));

		//		if (itmX.SubItems.Count > 5)
		//			itmX.SubItems[5].Text = _page05Obstacles.Parts[i].Dist.ToString("0.0");
		//		else
		//			itmX.SubItems.Insert(5, new System.Windows.Forms.ListViewItem.ListViewSubItem(null, _page05Obstacles.Parts[i].Dist.ToString("0.0")));

		//		if (itmX.SubItems.Count > 6)
		//			itmX.SubItems[6].Text = _page05Obstacles.Parts[i].DistStar.ToString("0.0");
		//		else
		//			itmX.SubItems.Insert(6, new System.Windows.Forms.ListViewItem.ListViewSubItem(null, _page05Obstacles.Parts[i].DistStar.ToString("0.0")));

		//		if (itmX.SubItems.Count > 7)
		//			itmX.SubItems[7].Text = GlobalVars.OASPlaneNames[(int)_page05Obstacles.Parts[i].Plane & 15];
		//		else
		//			itmX.SubItems.Insert(7, new System.Windows.Forms.ListViewItem.ListViewSubItem(null, GlobalVars.OASPlaneNames[(int)_page05Obstacles.Parts[i].Plane & 15]));

		//		if (((int)_page05Obstacles.Parts[i].Plane & 32) != 0)
		//		{
		//			if (itmX.SubItems.Count > 8)
		//				itmX.SubItems[8].Text = "Cat 2";
		//			else
		//				itmX.SubItems.Insert(8, new System.Windows.Forms.ListViewItem.ListViewSubItem(null, "Cat 2"));
		//		}
		//		else if (itmX.SubItems.Count > 8)
		//			itmX.SubItems[8].Text = "Cat 1";
		//		else
		//			itmX.SubItems.Insert(8, new System.Windows.Forms.ListViewItem.ListViewSubItem(null, "Cat 1"));

		//		if (itmX.SubItems.Count > 9)
		//			itmX.SubItems[9].Text = ZoneNames[(int)_page05Obstacles.Parts[i].Flags];
		//		else
		//			itmX.SubItems.Insert(9, new System.Windows.Forms.ListViewItem.ListViewSubItem(null, ZoneNames[_page05Obstacles.Parts[i].Flags]));
		//	}

		//	if (ReportBtn.Checked && Visible)
		//		ListViews_SelectedIndexChanged(ListView05, null);
		//}

		//#endregion

		//#region Page VI

		//public void FillPage06(ObstacleContainer Obstacles, int PrecFlg = -1)
		//{
		//	PrecFlg06 = PrecFlg;
		//	//UseILSPlanes07 = UseILSPlanes;

		//	int m = Obstacles.Obstacles.Length;
		//	int n = Obstacles.Parts.Length;

		//	_page06Obstacles.Obstacles = new Obstacle[m];
		//	_page06Obstacles.Parts = new ObstacleData[n];

		//	Env.Current.AranGraphics.SafeDeleteGraphic(pPointElem);
		//	Env.Current.AranGraphics.SafeDeleteGraphic(pGeomElem);
		//	ListView06.Items.Clear();

		//	if (n <= 0)
		//		return;

		//	int i;

		//	for (i = 0; i < m; i++)
		//		_page06Obstacles.Obstacles[i] = Obstacles.Obstacles[i];

		//	for (i = 0; i < n; i++)
		//	{
		//		_page06Obstacles.Parts[i] = Obstacles.Parts[i];
		//		System.Windows.Forms.ListViewItem itmX = ListView06.Items.Add(Obstacles.Obstacles[Obstacles.Parts[i].Owner].TypeName);
		//		itmX.SubItems.Insert(1, new System.Windows.Forms.ListViewItem.ListViewSubItem(null, Obstacles.Obstacles[Obstacles.Parts[i].Owner].UnicalName));
		//		itmX.SubItems.Insert(2, new System.Windows.Forms.ListViewItem.ListViewSubItem(null, Env.Current.UnitContext.UnitConverter.HeightToDisplayUnits(Obstacles.Parts[i].Height, eRoundMode.NERAEST).ToString()));
		//		itmX.SubItems.Insert(3, new System.Windows.Forms.ListViewItem.ListViewSubItem(null, "-"));
		//		itmX.SubItems.Insert(4, new System.Windows.Forms.ListViewItem.ListViewSubItem(null, Env.Current.UnitContext.UnitConverter.HeightToDisplayUnits(Obstacles.Parts[i].ReqH, eRoundMode.NERAEST).ToString()));
		//		itmX.SubItems.Insert(5, new System.Windows.Forms.ListViewItem.ListViewSubItem(null, Env.Current.UnitContext.UnitConverter.HeightToDisplayUnits(Obstacles.Parts[i].ReqOCH, eRoundMode.NERAEST).ToString()));
		//		itmX.SubItems.Insert(6, new System.Windows.Forms.ListViewItem.ListViewSubItem(null, Env.Current.UnitContext.UnitConverter.HeightToDisplayUnits(Obstacles.Parts[i].hPenet, eRoundMode.NERAEST).ToString()));
		//		itmX.SubItems.Insert(7, new System.Windows.Forms.ListViewItem.ListViewSubItem(null, Obstacles.Parts[i].Dist.ToString("0.0")));

		//		if (Obstacles.Parts[i].TurnDistL > 0.0)
		//		{
		//			itmX.SubItems.Insert(8, new System.Windows.Forms.ListViewItem.ListViewSubItem(null, Env.Current.UnitContext.UnitConverter.DistanceToDisplayUnits(Obstacles.Parts[i].TurnDistL, eRoundMode.NERAEST).ToString()));
		//			itmX.SubItems.Insert(9, new System.Windows.Forms.ListViewItem.ListViewSubItem(null, ARANMath.RadToDeg(Obstacles.Parts[i].TurnAngleL).ToString("0.0")));
		//		}
		//		else
		//		{
		//			itmX.SubItems.Insert(8, new System.Windows.Forms.ListViewItem.ListViewSubItem(null, "-"));
		//			itmX.SubItems.Insert(9, new System.Windows.Forms.ListViewItem.ListViewSubItem(null, "-"));
		//		}

		//		if (PrecFlg < 0)
		//		{
		//			if (Obstacles.Parts[i].Flags != 0)
		//				itmX.SubItems.Insert(10, new System.Windows.Forms.ListViewItem.ListViewSubItem(null, "Primary"));
		//			else
		//				itmX.SubItems.Insert(10, new System.Windows.Forms.ListViewItem.ListViewSubItem(null, "Secondary"));
		//		}
		//		else
		//		{
		//			if (Obstacles.Parts[i].Plane == eOAS.NonPrec)
		//				itmX.SubItems.Insert(6, new System.Windows.Forms.ListViewItem.ListViewSubItem(null, "-"));

		//			//if (UseILSPlanes)
		//			//    itmX.SubItems.Insert(10, new System.Windows.Forms.ListViewItem.ListViewSubItem(null, GlobalVars.ILSPlaneNames[(int)Obstacles.Parts[i].Plane]));
		//			//else
		//			itmX.SubItems.Insert(10, new System.Windows.Forms.ListViewItem.ListViewSubItem(null, GlobalVars.OASPlaneNames[(int)Obstacles.Parts[i].Plane & 15]));
		//		}
		//	}

		//	ListView06.Tag = _page06Obstacles;

		//	if (SortF06 != 0)
		//	{
		//		System.Windows.Forms.ColumnHeader pColumnHeader = ListView06.Columns[System.Math.Abs(SortF06) - 1];
		//		SortF06 = -SortF06;
		//		ListView06_ColumnClick(ListView06, new System.Windows.Forms.ColumnClickEventArgs(pColumnHeader.Index));
		//	}

		//	//SetVisible (5);
		//	mainTabControl.SelectedIndex = 5;
		//	if (ReportBtn.Checked && !this.Visible)
		//		Show(GlobalVars.Win32Window);
		//}

		//private void ListView06_ColumnClick(object sender, ColumnClickEventArgs e)
		//{
		//	System.Windows.Forms.ColumnHeader ColumnHeader = ListView06.Columns[e.Column];

		//	ListView06.Sorting = SortOrder.None;
		//	int n = _page06Obstacles.Parts.Length;

		//	if (System.Math.Abs(SortF06) - 1 == ColumnHeader.Index)
		//		SortF06 = -SortF06;
		//	else
		//	{
		//		if (SortF06 != 0)
		//			ListView06.Columns[System.Math.Abs(SortF06) - 1].ImageIndex = 2;
		//		SortF06 = ColumnHeader.Index + 1;
		//	}

		//	if (SortF06 > 0)
		//		ColumnHeader.ImageIndex = 0;
		//	else
		//		ColumnHeader.ImageIndex = 1;

		//	int i;
		//	if (ColumnHeader.Index != 3 && ColumnHeader.Index >= 2 && ColumnHeader.Index <= 9)
		//	{
		//		for (i = 0; i < n; i++)
		//			switch (ColumnHeader.Index)
		//			{
		//				case 2:
		//					_page06Obstacles.Parts[i].fSort = _page06Obstacles.Parts[i].Height;
		//					break;
		//				case 4:
		//					_page06Obstacles.Parts[i].fSort = _page06Obstacles.Parts[i].ReqH;
		//					break;
		//				case 5:
		//					_page06Obstacles.Parts[i].fSort = _page06Obstacles.Parts[i].ReqOCH;
		//					break;
		//				case 6:
		//					_page06Obstacles.Parts[i].fSort = _page06Obstacles.Parts[i].hPenet;
		//					break;
		//				case 7:
		//					_page06Obstacles.Parts[i].fSort = _page06Obstacles.Parts[i].Dist;
		//					break;
		//				case 8:
		//					_page06Obstacles.Parts[i].fSort = _page06Obstacles.Parts[i].TurnDistL;
		//					break;
		//				case 9:
		//					_page06Obstacles.Parts[i].fSort = _page06Obstacles.Parts[i].TurnAngleL;
		//					break;
		//			}

		//		if (SortF06 > 0)
		//			Functions.shall_SortfSort(_page06Obstacles);
		//		else
		//			Functions.shall_SortfSortD(_page06Obstacles);
		//	}
		//	else
		//	{
		//		for (i = 0; i < n; i++)
		//			switch (ColumnHeader.Index)
		//			{
		//				case 0:
		//					_page06Obstacles.Parts[i].sSort = _page06Obstacles.Obstacles[_page06Obstacles.Parts[i].Owner].TypeName;
		//					break;
		//				case 1:
		//					_page06Obstacles.Parts[i].sSort = _page06Obstacles.Obstacles[_page06Obstacles.Parts[i].Owner].UnicalName;
		//					break;
		//				case 10:
		//					if (PrecFlg06 < 0)
		//					{
		//						if (_page06Obstacles.Parts[i].Plane != 0)
		//							_page06Obstacles.Parts[i].sSort = "Primary";
		//						else
		//							_page06Obstacles.Parts[i].sSort = "Secondary";
		//					}
		//					else
		//					{
		//						//if (UseILSPlanes07)
		//						//    LObstaclesPage07.Parts[i].sSort = GlobalVars.ILSPlaneNames[(int)LObstaclesPage07.Parts[i].Plane];
		//						//else
		//						_page06Obstacles.Parts[i].sSort = GlobalVars.OASPlaneNames[(int)_page06Obstacles.Parts[i].Plane & 15];
		//					}
		//					break;
		//			}

		//		if (SortF06 > 0)
		//			Functions.shall_SortsSort(_page06Obstacles);
		//		else
		//			Functions.shall_SortsSortD(_page06Obstacles);
		//	}

		//	for (i = 0; i < n; i++)
		//	{
		//		System.Windows.Forms.ListViewItem itmX = ListView06.Items[i];

		//		itmX.Text = _page06Obstacles.Obstacles[_page06Obstacles.Parts[i].Owner].TypeName;
		//		itmX.SubItems[1].Text = _page06Obstacles.Obstacles[_page06Obstacles.Parts[i].Owner].UnicalName;
		//		itmX.SubItems[2].Text = Env.Current.UnitContext.UnitConverter.HeightToDisplayUnits(_page06Obstacles.Parts[i].Height, eRoundMode.NERAEST).ToString();
		//		itmX.SubItems[3].Text = "-";
		//		itmX.SubItems[4].Text = Env.Current.UnitContext.UnitConverter.HeightToDisplayUnits(_page06Obstacles.Parts[i].ReqH, eRoundMode.NERAEST).ToString();
		//		itmX.SubItems[5].Text = Env.Current.UnitContext.UnitConverter.HeightToDisplayUnits(_page06Obstacles.Parts[i].ReqOCH, eRoundMode.NERAEST).ToString();
		//		itmX.SubItems[6].Text = Env.Current.UnitContext.UnitConverter.HeightToDisplayUnits(_page06Obstacles.Parts[i].hPenet, eRoundMode.NERAEST).ToString();
		//		itmX.SubItems[7].Text = _page06Obstacles.Parts[i].Dist.ToString("0.0");

		//		if (_page06Obstacles.Parts[i].TurnDistL > 0.0)
		//		{
		//			itmX.SubItems[8].Text = Env.Current.UnitContext.UnitConverter.DistanceToDisplayUnits(_page06Obstacles.Parts[i].TurnDistL, eRoundMode.NERAEST).ToString();
		//			itmX.SubItems[9].Text = ARANMath.RadToDeg(_page06Obstacles.Parts[i].TurnAngleL).ToString("0.0");
		//		}
		//		else
		//		{
		//			itmX.SubItems[8].Text = "-";
		//			itmX.SubItems[9].Text = "-";
		//		}

		//		if (PrecFlg06 < 0)
		//		{
		//			if (_page06Obstacles.Parts[i].Flags != 0)
		//				itmX.SubItems[10].Text = "Primary";
		//			else
		//				itmX.SubItems[10].Text = "Secondary";
		//		}
		//		else
		//		{
		//			if (_page06Obstacles.Parts[i].Plane == eOAS.NonPrec)
		//				itmX.SubItems[6].Text = "-";

		//			//if (UseILSPlanes07)
		//			//    itmX.SubItems[10].Text = GlobalVars.ILSPlaneNames[(int)LObstaclesPage07.Parts[i].Plane & 15];
		//			//else
		//			itmX.SubItems[10].Text = GlobalVars.OASPlaneNames[(int)_page06Obstacles.Parts[i].Plane & 15];
		//		}
		//	}

		//	if (ReportBtn.Checked && Visible)
		//		ListViews_SelectedIndexChanged(ListView06, null);
		//}
		//#endregion

		#region Page VII

		int GetIndex(ObstacleData[] Obstacles, ObstacleData TestObs)
		{
			int i, n = Obstacles.Length;

			for (i = 0; i < n; i++)
				if (Obstacles[i].pPtPrj.X == TestObs.pPtPrj.X && Obstacles[i].pPtPrj.Y == TestObs.pPtPrj.Y && Obstacles[i].pPtPrj.Z == TestObs.pPtPrj.Z)
					return i;

			return -1;
		}

        public void FillPage07(ObstacleContainer Obstacles)//, ObstacleData DetObs
		{
			Env.Current.AranGraphics.SafeDeleteGraphic(pPointElem);
			Env.Current.AranGraphics.SafeDeleteGraphic(pGeomElem);

			dataGridView07.Tag = Obstacles;

			if (Obstacles.Parts == null)
				return;

			int m = Obstacles.Obstacles.Length;
			int n = Obstacles.Parts.Length;

			_page07Obstacles.Obstacles = new Obstacle[m];
			_page07Obstacles.Parts = new ObstacleData[n];

			dataGridView07.RowCount = 0;

			if (n <= 0)
				return;

			int i;

			for (i = 0; i < m; i++)
				_page07Obstacles.Obstacles[i] = Obstacles.Obstacles[i];

			dataGridView07.Tag = _page07Obstacles;
			//if (mainTabControl.SelectedIndex == 2)
			//	Env.Current.AranGraphics.SafeDeleteGraphic(_pointElem);
			//double NomPDG = 100.0 * GlobalVars.constants.Pansops[ePANSOPSData.dpPDG_Nom].Value;

			if (_page08Count > 0)
				for (i = 0; i < _page08Obstacles[_page08Count - 1].Parts.Length; i++)
					_page08Obstacles[_page08Count - 1].Parts[i].IsExcluded = GetIndex(Obstacles.Parts, _page08Obstacles[_page08Count - 1].Parts[i]) >= 0;

			for (i = 0; i < n; i++)
			{
				_page07Obstacles.Parts[i] = Obstacles.Parts[i];
				_page07Obstacles.Parts[i].IsExcluded = false;

				//if (_page4Index >= 0)
				//{
				//    int indx = GetIndex(_obstaclesPage4[_page4Index], Obstacles[i]);
				//    if (indx >= 0)
				//        _obstaclesPage4[_page4Index][indx].IsExcluded = true;
				//}

				DataGridViewRow row = new DataGridViewRow();
				row.Tag = _page07Obstacles.Parts[i];

				DataGridViewTextBoxCell cell = new DataGridViewTextBoxCell();
				cell.Value = Obstacles.Obstacles[Obstacles.Parts[i].Owner].TypeName;
				row.Cells.Add(cell);

				cell = new DataGridViewTextBoxCell();
				cell.Value = Obstacles.Obstacles[Obstacles.Parts[i].Owner].UnicalName;
				row.Cells.Add(cell);

				row.Cells.Add(new DataGridViewTextBoxCell());
                
				row.Cells[2].Value = Env.Current.UnitContext.UnitConverter.HeightToDisplayUnits(Obstacles.Parts[i].Height, eRoundMode.NEAREST).ToString();

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[3].Value = Env.Current.UnitContext.UnitConverter.HeightToDisplayUnits(Obstacles.Parts[i].Elev, eRoundMode.NEAREST).ToString();

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[4].Value = Env.Current.UnitContext.UnitConverter.HeightToDisplayUnits(Obstacles.Parts[i].MOC, eRoundMode.NEAREST).ToString();

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[5].Value = Env.Current.UnitContext.UnitConverter.HeightToDisplayUnits(Obstacles.Parts[i].ReqH, eRoundMode.NEAREST).ToString();

				dataGridView07.Rows.Add(row);

                //if (Obstacles.Obstacles[Obstacles.Parts[i].Owner].UnicalName == Obstacles.Obstacles[DetObs.Owner].UnicalName)
                //{
                //    row.Cells[0].Style.ForeColor = System.Drawing.Color.Red;
                //    row.Cells[0].Style.Font = new System.Drawing.Font(Font, System.Drawing.FontStyle.Bold);

                //    for (int j = 1; j < row.Cells.Count; j++)
                //    {
                //        row.Cells[j].Style.ForeColor = row.Cells[0].Style.ForeColor;
                //        row.Cells[j].Style.Font = row.Cells[0].Style.Font;
                //    }
                //}

				row.Cells[3].Style.Font = new System.Drawing.Font(Font, System.Drawing.FontStyle.Bold);
			}

			//SetTabVisible(2, true);
			if (ReportBtn.Checked && !Visible)
			    Show(Env.Current.SystemContext.Win32Window);

            //if(numericUpDown1.Value == _page4Index + 1)
            //	FillPage4((int)numericUpDown1.Value - 1);
            //if (_page4Index < 0)	_page4Index = 0;

            if (numericUpDown1.Value - 1 == _page08Index)
				FillPage08(_page08Index);
		}

		private void dataGridView3_RowEnter(object sender, DataGridViewCellEventArgs e)
		{
			Env.Current.AranGraphics.SafeDeleteGraphic(pPointElem);
			Env.Current.AranGraphics.SafeDeleteGraphic(pGeomElem);

			System.Windows.Forms.DataGridView dataGridView = (System.Windows.Forms.DataGridView)sender;

			if (!this.Visible || e.RowIndex < 0 || dataGridView.Tag == null)
				return;

			ObstacleContainer pageObstacles = (ObstacleContainer)dataGridView.Tag;
			if (pageObstacles.Parts.Length == 0)
				return;

			ObstacleData obs = (ObstacleData)dataGridView.Rows[e.RowIndex].Tag;

			int Index = obs.Owner;

			Geometry pGeometry;
			Aran.Geometries.Point pPtTmp;
			pGeometry = pageObstacles.Obstacles[Index].pGeomPrj;
			pPtTmp = pageObstacles.Parts[Index].pPtPrj;

			if (pGeometry.Type == GeometryType.MultiLineString)
				pGeomElem = Env.Current.AranGraphics.DrawMultiLineString((MultiLineString)pGeometry, 2, 255);
			else if (pGeometry.Type == GeometryType.MultiPolygon)
				pGeomElem = Env.Current.AranGraphics.DrawMultiPolygon((MultiPolygon)pGeometry, AranEnvironment.Symbols.eFillStyle.sfsDiagonalCross, 255);

			pPointElem = Env.Current.AranGraphics.DrawPointWithText(pPtTmp, pageObstacles.Obstacles[Index].UnicalName, 255);
		}

		#endregion

		#region Page VIII
		public ObstacleContainer[] Obstacles
		{
			get { return _page08Obstacles; }
		}

		public int ObstacleLists
		{
			get { return _page08Count; }
		}

		public void AddPage08(ObstacleContainer Obstacles, ObstacleData DetObs)
		{
			if (_page08Count > MaxLegsCount)
				throw new Exception("Maximum legs count exceeded.");

			int m = Obstacles.Obstacles.Length;
			int n = Obstacles.Parts.Length;

			_page08Obstacles[_page08Count].Obstacles = new Obstacle[m];
			_page08Obstacles[_page08Count].Parts = new ObstacleData[n];

			Array.Copy(Obstacles.Obstacles, _page08Obstacles[_page08Count].Obstacles, m);
			Array.Copy(Obstacles.Parts, _page08Obstacles[_page08Count].Parts, n);

			_page08DetObs[_page08Count] = DetObs;

			_page08Count++;
			numericUpDown1.Maximum = _page08Count;

			if (_page08Index < 0)
				_page08Index = 0;
			FillPage08(_page08Index);

			//SetTabVisible(3, true);
		}

		public void RemoveLastLeg()
		{
			//if (_page08Index < 0)
			//    throw new Exception("Legs index out of bounds.");
			_page08Index = 0;		//????
			_page08Count--;
			numericUpDown1.Maximum = _page08Count;

			if (_page08Count <= 0)
			{
				//SetTabVisible(3, false);
			}
		}

		public void FillPage08(int LegNum)
		{
			_page08Index = LegNum;

			if (mainTabControl.SelectedIndex == 8)
			{
				Env.Current.AranGraphics.SafeDeleteGraphic(pPointElem);
				Env.Current.AranGraphics.SafeDeleteGraphic(pGeomElem);
			}

			dataGridView08.RowCount = 0;
			if (LegNum < 0)
				return;

			ObstacleContainer Obstacles = _page08Obstacles[LegNum];
			dataGridView08.Tag = Obstacles;

			if (Obstacles.Parts == null)
				return;

			ObstacleData DetObs = _page08DetObs[LegNum];

			int m = Obstacles.Obstacles.Length;
			int n = Obstacles.Parts.Length;

			for (int i = 0; i < n; i++)
			{
				if (Obstacles.Parts[i].IsExcluded)
					continue;

				DataGridViewRow row = new DataGridViewRow();
				row.Tag = _page08Obstacles[LegNum].Parts[i];

				DataGridViewTextBoxCell cell = new DataGridViewTextBoxCell();
				cell.Value = Obstacles.Obstacles[Obstacles.Parts[i].Owner].TypeName;
				row.Cells.Add(cell);

				cell = new DataGridViewTextBoxCell();
				cell.Value = Obstacles.Obstacles[Obstacles.Parts[i].Owner].UnicalName;
				row.Cells.Add(cell);

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[2].Value = Env.Current.UnitContext.UnitConverter.HeightToDisplayUnits(Obstacles.Parts[i].Height, eRoundMode.NEAREST).ToString();

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[3].Value = Env.Current.UnitContext.UnitConverter.HeightToDisplayUnits(Obstacles.Parts[i].Elev, eRoundMode.NEAREST).ToString();

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[4].Value = Env.Current.UnitContext.UnitConverter.HeightToDisplayUnits(Obstacles.Parts[i].MOC, eRoundMode.NEAREST).ToString();

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[5].Value = Env.Current.UnitContext.UnitConverter.HeightToDisplayUnits(Obstacles.Parts[i].ReqH, eRoundMode.NEAREST).ToString();

				dataGridView08.Rows.Add(row);

				if (Obstacles.Obstacles[Obstacles.Parts[i].Owner].UnicalName == Obstacles.Obstacles[DetObs.Owner].UnicalName)
				{
					row.Cells[0].Style.ForeColor = System.Drawing.Color.Red;
					row.Cells[0].Style.Font = new System.Drawing.Font(Font, System.Drawing.FontStyle.Bold);

					for (int j = 1; j < row.Cells.Count; j++)
					{
						row.Cells[j].Style.ForeColor = row.Cells[0].Style.ForeColor;
						row.Cells[j].Style.Font = row.Cells[0].Style.Font;
					}
				}
				row.Cells[3].Style.Font = new System.Drawing.Font(Font, System.Drawing.FontStyle.Bold);
			}

			//SetTabVisible(4, true);
			//if (_reportBtn.Checked && !Visible)
			//	Show(GlobalVars.Win32Window);
		}

		private void dataGridView4_RowEnter(object sender, DataGridViewCellEventArgs e)
		{
			Env.Current.AranGraphics.SafeDeleteGraphic(pPointElem);
			Env.Current.AranGraphics.SafeDeleteGraphic(pGeomElem);

			if (!this.Visible || e.RowIndex < 0 || _page08Count < 0)
				return;

			System.Windows.Forms.DataGridView dataGridView = (System.Windows.Forms.DataGridView)sender;
			if (dataGridView.Tag == null)
				return;
			
			ObstacleContainer pageObstacles = (ObstacleContainer)dataGridView.Tag;
			if (pageObstacles.Parts.Length == 0)
				return;

			ObstacleData obs = (ObstacleData)dataGridView.Rows[e.RowIndex].Tag;

			int Index = obs.Owner;

			Geometry pGeometry;
			Aran.Geometries.Point pPtTmp;
			pGeometry = pageObstacles.Obstacles[Index].pGeomPrj;
			pPtTmp = pageObstacles.Parts[Index].pPtPrj;

			if (pGeometry.Type == GeometryType.MultiLineString)
				pGeomElem = Env.Current.AranGraphics.DrawMultiLineString((MultiLineString)pGeometry, 2, 255);
			else if (pGeometry.Type == GeometryType.MultiPolygon)
				pGeomElem = Env.Current.AranGraphics.DrawMultiPolygon((MultiPolygon)pGeometry, AranEnvironment.Symbols.eFillStyle.sfsDiagonalCross, 255);

			pPointElem = Env.Current.AranGraphics.DrawPointWithText(pPtTmp, pageObstacles.Obstacles[Index].UnicalName, 255);
		}

		private void numericUpDown1_ValueChanged(object sender, EventArgs e)
		{
			FillPage08((int)numericUpDown1.Value - 1);
		}

		#endregion

	}
}
