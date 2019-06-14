//3186801021

using System;
using System.Collections; 
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geometry;

namespace ETOD.Forms
{
	public partial class ReportFrm : Form
	{
		// Sorts ListViewGroup objects by header value.
		private class ListViewGroupSorter : IComparer
		{
			private SortOrder order;

			// Stores the sort order.
			public ListViewGroupSorter(SortOrder theOrder)
			{
				order = theOrder;
			}

			// Compares the groups by header value, using the saved sort
			// order to return the correct value.
			public int Compare(object x, object y)
			{
				int result = String.Compare(((ListViewGroup)x).Header,
					((ListViewGroup)y).Header);

				if (order == SortOrder.Ascending)
					return result;
				else
					return -result;
			}
		}

		#region declerations
		private int _sortF1;
		private int _sortF2;
		private int _sortF3;
		private int _sortF4;


		ObstacleType[] LArea1ObstList;

		List<Area2DATA> area2Data;
		Area2DATA CurrArea2Data;
		int CurrArea2Index = 0;

		ObstacleType[] LArea2ObstList;

		ObstacleType[] LArea3ObstList;
		ObstacleType[] LArea4ObstList;

		private IElement _pointElem;

		private CheckBox _reportBtn;
		private int _helpContextID;

		#endregion

		#region Common part
		public ReportFrm()
		{
			InitializeComponent();
		}

		private void ReportFrm_FormClosing(object sender, FormClosingEventArgs e)
		{
			Functions.DeleteGraphicsElement(_pointElem);

			if (e.CloseReason == CloseReason.UserClosing)
			{
				e.Cancel = true;
				Hide();
				_reportBtn.Checked = false;
			}
		}

		private void HelpBtn_Click(object sender, EventArgs e)
		{
			NativeMethods.HtmlHelp(0, GlobalVars.HelpFile, GlobalVars.HH_HELP_CONTEXT, _helpContextID);
		}

		private void CloseBtn_Click(object sender, EventArgs e)
		{
			Functions.DeleteGraphicsElement(_pointElem);
			GlobalVars.GetActiveView().PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);

			_reportBtn.Checked = false;
			Hide();
		}

		public void SetBtn(CheckBox Btn, int HelpContext)
		{
			_reportBtn = Btn;
			_helpContextID = HelpContext;
		}

		#endregion

		#region Area 1

		public void FillArea1ObstacleList(ObstacleType[] ObstList)
		{
			int n = ObstList.Length;
			LArea1ObstList = new ObstacleType[n];
			ObstList.CopyTo(LArea1ObstList, 0);
			ListViewItem itmX;

			listView1.Items.Clear();

			for (int i = 0; i < n; i++)
			{
				itmX = listView1.Items.Add(LArea1ObstList[i].Group.ToString());

				itmX.SubItems.Insert(1, new ListViewItem.ListViewSubItem(null, LArea1ObstList[i].Name));
				itmX.SubItems.Insert(2, new ListViewItem.ListViewSubItem(null, LArea1ObstList[i].PartName));
				itmX.SubItems.Insert(3, new ListViewItem.ListViewSubItem(null, LArea1ObstList[i].ID.ToString()));

				if (LArea1ObstList[i].Height > -9990)
					itmX.SubItems.Insert(4, new ListViewItem.ListViewSubItem(null, Functions.ConvertHeight(LArea1ObstList[i].Height, eRoundMode.rmCEIL).ToString()));
				else
					itmX.SubItems.Insert(4, new ListViewItem.ListViewSubItem(null, ""));

				itmX.SubItems.Insert(5, new ListViewItem.ListViewSubItem(null, Functions.ConvertHeight(LArea1ObstList[i].Elevation, eRoundMode.rmCEIL).ToString()));

				itmX.SubItems.Insert(6, new ListViewItem.ListViewSubItem(null, Functions.ConvertHeight(LArea1ObstList[i].HorAccuracy, eRoundMode.rmCEIL).ToString()));
				itmX.SubItems.Insert(7, new ListViewItem.ListViewSubItem(null, Functions.ConvertHeight(LArea1ObstList[i].VertAccuracy, eRoundMode.rmCEIL).ToString()));

				itmX.SubItems.Insert(8, new ListViewItem.ListViewSubItem(null, Functions.ConvertHeight(LArea1ObstList[i].Hsurface, eRoundMode.rmCEIL).ToString()));
				itmX.SubItems.Insert(9, new ListViewItem.ListViewSubItem(null, Functions.ConvertHeight(LArea1ObstList[i].hPent, eRoundMode.rmCEIL).ToString()));
				//itmX.SubItems.Insert(10, new ListViewItem.ListViewSubItem(null, LArea1ObstList[i].codeObstacleArea.ToString() ));	//.Substring(6)
				//if (LArea1ObstList[i].Ignored)
				//    itmX.SubItems.Insert(11, new ListViewItem.ListViewSubItem(null, "Ignored"));
				//else
				//    itmX.SubItems.Insert(11, new ListViewItem.ListViewSubItem(null, ""));
			}

			if (_reportBtn.Checked && !Visible)
				Show(GlobalVars.m_Win32Window);

		}

		private void ListView1_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (((ListView)sender).SelectedItems.Count == 0)
				return;

			ListViewItem Item = ((ListView)sender).SelectedItems[0];
			IGraphicsContainer pGraphics = null;

			if ((LArea1ObstList.Length == 0) || (Item == null))
				return;

			pGraphics = GlobalVars.GetActiveView().GraphicsContainer;

			Functions.DeleteGraphicsElement(_pointElem);

			if (LArea1ObstList[Item.Index].pGeoPrj.GeometryType == ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryPoint)
				_pointElem = Functions.DrawPointWithText(LArea1ObstList[Item.Index].pGeoPrj as IPoint, LArea1ObstList[Item.Index].ID.ToString(), 255);
			else if (LArea1ObstList[Item.Index].pGeoPrj.GeometryType == ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryPolygon)
				_pointElem = Functions.DrawPolygon(LArea1ObstList[Item.Index].pGeoPrj as IPolygon, 255);
			else
				_pointElem = Functions.DrawPolyline(LArea1ObstList[Item.Index].pGeoPrj as IPolyline, 255);
		}

		private void ListView1_ColumnClick(object sender, ColumnClickEventArgs e)
		{
			ColumnHeader ColumnHeader = listView1.Columns[e.Column];
			ListViewItem itmX = null;

			int i;
			int n = LArea1ObstList.Length;
			int AbsF1 = System.Math.Abs(_sortF1);

			//FontStyle RecordFontStyle = 0;
			//System.Drawing.Color RecordColor = new System.Drawing.Color();

			if (AbsF1 - 1 == ColumnHeader.Index)
				_sortF1 = -_sortF1;
			else
			{
				if (_sortF1 != 0)
					listView1.Columns[AbsF1 - 1].ImageIndex = -1;

				_sortF1 = ColumnHeader.Index + 1;
			}

			if (_sortF1 > 0)
				ColumnHeader.ImageIndex = 0;
			else
				ColumnHeader.ImageIndex = 1;

			if (ColumnHeader.Index > 0 && ColumnHeader.Index <= 2)
			{
				for (i = 0; i < n; i++)
				{
					switch (ColumnHeader.Index)
					{
						case 0:
							LArea1ObstList[i].sSort = LArea1ObstList[i].Name;
							break;
						case 1:
							LArea1ObstList[i].sSort = LArea1ObstList[i].PartName;
							break;
					}
				}

				if (_sortF1 > 0)
					Functions.shall_SortsSort(LArea1ObstList);
				else
					Functions.shall_SortsSortD(LArea1ObstList);
			}
			else
			{
				for (i = 0; i < n; i++)
				{
					switch (ColumnHeader.Index)
					{
						case 0:
							LArea1ObstList[i].fSort = LArea1ObstList[i].Group;
							break;
						case 3:
							LArea1ObstList[i].fSort = LArea1ObstList[i].ID;
							break;
						case 4:
							LArea1ObstList[i].fSort = LArea1ObstList[i].Height;
							break;
						case 5:
							LArea1ObstList[i].fSort = LArea1ObstList[i].Elevation;
							break;
						case 6:
							LArea1ObstList[i].fSort = LArea1ObstList[i].HorAccuracy;
							break;
						case 7:
							LArea1ObstList[i].fSort = LArea1ObstList[i].VertAccuracy;
							break;
						case 8:
							LArea1ObstList[i].fSort = LArea1ObstList[i].Hsurface;
							break;
						case 9:
							LArea1ObstList[i].fSort = LArea1ObstList[i].hPent;
							break;
						//case 10:
						//    LArea1ObstList[i].fSort = System.Convert.ToInt32(LArea1ObstList[i].codeObstacleArea);
						//    break;
						//case 11:
						//    LArea1ObstList[i].fSort = System.Convert.ToInt32(LArea1ObstList[i].Ignored);
						//    break;
					}
				}

				if (_sortF1 > 0)
					Functions.shall_SortfSort(LArea1ObstList);
				else
					Functions.shall_SortfSortD(LArea1ObstList);
			}

			for (i = 0; i < n; i++)
			{
				itmX = listView1.Items[i];

				//if (LObstList[i].ID == 1)
				//{
				//    RecordFontStyle = FontStyle.Bold;
				//    RecordColor = System.Drawing.Color.FromArgb(0XFF0000);
				//}
				//else
				//{
				//    RecordFontStyle = FontStyle.Regular;
				//    RecordColor = System.Drawing.Color.FromArgb(0);
				//}
				itmX.Text = LArea1ObstList[i].Group.ToString();

				itmX.SubItems[1].Text = LArea1ObstList[i].Name;
				itmX.SubItems[2].Text = LArea1ObstList[i].PartName;
				itmX.SubItems[3].Text = LArea1ObstList[i].ID.ToString();

				if (LArea1ObstList[i].Height > -9990)
					itmX.SubItems[4].Text = Functions.ConvertHeight(LArea1ObstList[i].Height, eRoundMode.rmCEIL).ToString();
				else
					itmX.SubItems[4].Text = "";

				itmX.SubItems[5].Text = Functions.ConvertHeight(LArea1ObstList[i].Elevation, eRoundMode.rmCEIL).ToString();
				itmX.SubItems[6].Text = Functions.ConvertHeight(LArea1ObstList[i].HorAccuracy, eRoundMode.rmCEIL).ToString();
				itmX.SubItems[7].Text = Functions.ConvertHeight(LArea1ObstList[i].VertAccuracy, eRoundMode.rmCEIL).ToString();

				itmX.SubItems[8].Text = Functions.ConvertHeight(LArea1ObstList[i].Hsurface, eRoundMode.rmCEIL).ToString();

				itmX.SubItems[9].Text = Functions.ConvertHeight(LArea1ObstList[i].hPent, eRoundMode.rmCEIL).ToString();
				//itmX.SubItems[10].Text = LArea1ObstList[i].codeObstacleArea.ToString().Substring(6);

				//if (LArea1ObstList[i].Ignored)
				//    itmX.SubItems[11].Text = "Ignored";
				//else
				//    itmX.SubItems[11].Text = "";
			}

			if (_reportBtn.Checked)
				ListView1_SelectedIndexChanged(listView1, new System.EventArgs());
		}

		#endregion

		#region Area 2

		public void FillArea2ObstacleList(List<Area2DATA> pArea2Data)
		{
			area2Data = pArea2Data;

			comboBox1.Items.Clear();
			int i, n = area2Data.Count;

			for (i = 0; i < n; i++)
				comboBox1.Items.Add(area2Data[i].RWYName);

			if (0 < n)
				comboBox1.SelectedIndex = 0;
			else
				listView2.Items.Clear();
		}

		public void FillArea2ObstacleList(List<ObstacleType> ObstList)
		{
			int n = ObstList.Count;
			LArea2ObstList = new ObstacleType[n];
			ObstList.CopyTo(LArea2ObstList, 0);
			ListViewItem itmX;

			listView2.Items.Clear();

			listView2.Groups.Clear();
			ListViewGroup commonGroup = new ListViewGroup("adi", HorizontalAlignment.Center);
			listView2.Groups.Add(commonGroup);

			//======================================
			for (int i = 0; i < n; i++)
			{
				int m = LArea2ObstList[i].Releation.Count;
				if (m > 1)
				{
					ListViewGroup group = new ListViewGroup(LArea2ObstList[i].Name, HorizontalAlignment.Center);

					listView2.Groups.Add(group);
					for (int j = 0; j < m; j++)
					{
						itmX = listView2.Items.Add(LArea2ObstList[i].Group.ToString());

						itmX.SubItems.Insert(1, new ListViewItem.ListViewSubItem(null, LArea2ObstList[i].Name));
						itmX.SubItems.Insert(2, new ListViewItem.ListViewSubItem(null, LArea2ObstList[i].PartName));
						itmX.SubItems.Insert(3, new ListViewItem.ListViewSubItem(null, LArea2ObstList[i].ID.ToString()));

						if (LArea2ObstList[i].Height > -9990)
							itmX.SubItems.Insert(4, new ListViewItem.ListViewSubItem(null, Functions.ConvertHeight(LArea2ObstList[i].Height, eRoundMode.rmCEIL).ToString()));
						else
							itmX.SubItems.Insert(4, new ListViewItem.ListViewSubItem(null, ""));

						itmX.SubItems.Insert(5, new ListViewItem.ListViewSubItem(null, Functions.ConvertHeight(LArea2ObstList[i].Elevation, eRoundMode.rmCEIL).ToString()));
						itmX.SubItems.Insert(6, new ListViewItem.ListViewSubItem(null, Functions.ConvertHeight(LArea2ObstList[i].HorAccuracy, eRoundMode.rmCEIL).ToString()));
						itmX.SubItems.Insert(7, new ListViewItem.ListViewSubItem(null, Functions.ConvertHeight(LArea2ObstList[i].VertAccuracy, eRoundMode.rmCEIL).ToString()));

						ObstaclePlaneReleation opReleation = LArea2ObstList[i].Releation[j];

						itmX.SubItems.Insert(8, new ListViewItem.ListViewSubItem(null, Functions.ConvertHeight(opReleation.Hsurface, eRoundMode.rmCEIL).ToString()));
						itmX.SubItems.Insert(9, new ListViewItem.ListViewSubItem(null, Functions.ConvertHeight(opReleation.hPent, eRoundMode.rmCEIL).ToString()));
						itmX.SubItems.Insert(10, new ListViewItem.ListViewSubItem(null, opReleation.obstacleArea2.ToString()));//.Substring(6)
						if (opReleation.Ignored)
							itmX.SubItems.Insert(11, new ListViewItem.ListViewSubItem(null, "Ignored"));
						else
							itmX.SubItems.Insert(11, new ListViewItem.ListViewSubItem(null, ""));

						itmX.Group = group;
						itmX.Tag = LArea2ObstList[i];
					}
				}
				else
				{
					//opReleation
					itmX = listView2.Items.Add(LArea2ObstList[i].Group.ToString());


					itmX.SubItems.Insert(1, new ListViewItem.ListViewSubItem(null, LArea2ObstList[i].Name));
					itmX.SubItems.Insert(2, new ListViewItem.ListViewSubItem(null, LArea2ObstList[i].PartName));
					itmX.SubItems.Insert(3, new ListViewItem.ListViewSubItem(null, LArea2ObstList[i].ID.ToString()));

					if (LArea2ObstList[i].Height > -9990)
						itmX.SubItems.Insert(4, new ListViewItem.ListViewSubItem(null, Functions.ConvertHeight(LArea2ObstList[i].Height, eRoundMode.rmCEIL).ToString()));
					else
						itmX.SubItems.Insert(4, new ListViewItem.ListViewSubItem(null, ""));

					itmX.SubItems.Insert(5, new ListViewItem.ListViewSubItem(null, Functions.ConvertHeight(LArea2ObstList[i].Elevation, eRoundMode.rmCEIL).ToString()));
					itmX.SubItems.Insert(6, new ListViewItem.ListViewSubItem(null, Functions.ConvertHeight(LArea2ObstList[i].HorAccuracy, eRoundMode.rmCEIL).ToString()));
					itmX.SubItems.Insert(7, new ListViewItem.ListViewSubItem(null, Functions.ConvertHeight(LArea2ObstList[i].VertAccuracy, eRoundMode.rmCEIL).ToString()));

					itmX.SubItems.Insert(8, new ListViewItem.ListViewSubItem(null, Functions.ConvertHeight(LArea2ObstList[i].Hsurface, eRoundMode.rmCEIL).ToString()));
					itmX.SubItems.Insert(9, new ListViewItem.ListViewSubItem(null, Functions.ConvertHeight(LArea2ObstList[i].hPent, eRoundMode.rmCEIL).ToString()));
					itmX.SubItems.Insert(10, new ListViewItem.ListViewSubItem(null, LArea2ObstList[i].obstacleArea2.ToString()));//.Substring(6)
					if (LArea2ObstList[i].Ignored)
						itmX.SubItems.Insert(11, new ListViewItem.ListViewSubItem(null, "Ignored"));
					else
						itmX.SubItems.Insert(11, new ListViewItem.ListViewSubItem(null, ""));

					itmX.Group = commonGroup;
					itmX.Tag = LArea2ObstList[i];
				}
			}

			if (_reportBtn.Checked && !Visible)
				Show(GlobalVars.m_Win32Window);
		}

		private void ListView2_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (((ListView)sender).SelectedItems.Count == 0)
				return;

			ListViewItem Item = ((ListView)sender).SelectedItems[0];
			if ((LArea2ObstList.Length == 0) || (Item == null))
				return;

			ObstacleType obst = (ObstacleType)Item.Tag;
			IGraphicsContainer pGraphics = GlobalVars.GetActiveView().GraphicsContainer;
			Functions.DeleteGraphicsElement(_pointElem);

			if (obst.pGeoPrj.GeometryType == ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryPoint)
				_pointElem = Functions.DrawPointWithText(obst.pGeoPrj as IPoint, obst.ID.ToString(), 255);
			else if (obst.pGeoPrj.GeometryType == ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryPolygon)
				_pointElem = Functions.DrawPolygon(obst.pGeoPrj as IPolygon, 255);
			else
				_pointElem = Functions.DrawPolyline(obst.pGeoPrj as IPolyline, 255);
		}

		private void ListView2_ColumnClick(object sender, ColumnClickEventArgs e)
		{
			ColumnHeader ColumnHeader = listView2.Columns[e.Column];
			ListViewItem itmX = null;

			int i;
			int n = LArea2ObstList.Length;
			int AbsF2 = System.Math.Abs(_sortF2);

			//FontStyle RecordFontStyle = 0;
			//System.Drawing.Color RecordColor = new System.Drawing.Color();

			if (AbsF2 - 1 == ColumnHeader.Index)
				_sortF2 = -_sortF2;
			else
			{
				if (_sortF2 != 0)
					listView2.Columns[AbsF2 - 1].ImageIndex = -1;

				_sortF2 = ColumnHeader.Index + 1;
			}

			if (_sortF2 > 0)
				ColumnHeader.ImageIndex = 0;
			else
				ColumnHeader.ImageIndex = 1;

			if (ColumnHeader.Index > 0 && ColumnHeader.Index <= 2)
			{
				for (i = 0; i < n; i++)
				{
					switch (ColumnHeader.Index)
					{
						case 1:
							LArea2ObstList[i].sSort = LArea2ObstList[i].Name;
							break;
						case 2:
							LArea2ObstList[i].sSort = LArea2ObstList[i].PartName;
							break;
					}
				}

				if (_sortF2 > 0)
					Functions.shall_SortsSort(LArea2ObstList);
				else
					Functions.shall_SortsSortD(LArea2ObstList);
			}
			else
			{
				for (i = 0; i < n; i++)
				{
					switch (ColumnHeader.Index)
					{
						case 0:
							LArea2ObstList[i].fSort = LArea2ObstList[i].Group;
							break;
						case 3:
							LArea2ObstList[i].fSort = LArea2ObstList[i].ID;
							break;
						case 4:
							LArea2ObstList[i].fSort = LArea2ObstList[i].Height;
							break;
						case 5:
							LArea2ObstList[i].fSort = LArea2ObstList[i].Elevation;
							break;
						case 6:
							LArea2ObstList[i].fSort = LArea2ObstList[i].HorAccuracy;
							break;
						case 7:
							LArea2ObstList[i].fSort = LArea2ObstList[i].VertAccuracy;
							break;
						case 8:
							LArea2ObstList[i].fSort = LArea2ObstList[i].Hsurface;
							break;
						case 9:
							LArea2ObstList[i].fSort = LArea2ObstList[i].hPent;
							break;
						case 10:
							LArea2ObstList[i].fSort = System.Convert.ToInt32(LArea2ObstList[i].obstacleArea2);
							break;
						case 11:
							LArea2ObstList[i].fSort = System.Convert.ToInt32(LArea2ObstList[i].Ignored);
							break;
					}
				}

				if (_sortF2 > 0)
					Functions.shall_SortfSort(LArea2ObstList);
				else
					Functions.shall_SortfSortD(LArea2ObstList);
			}

			listView2.Groups.Clear();
			ListViewGroup commonGroup = new ListViewGroup("adi", HorizontalAlignment.Center);
			listView2.Groups.Add(commonGroup);

			int k = 0;

			for (i = 0; i < n; i++)
			{
				int m = LArea2ObstList[i].Releation.Count;
				if (m > 1)
				{
					ListViewGroup group = new ListViewGroup(LArea2ObstList[i].Name, HorizontalAlignment.Center);

					listView2.Groups.Add(group);
					for (int j = 0; j < m; j++)
					{
						itmX = listView2.Items[k++];
						itmX.Text = LArea2ObstList[i].Group.ToString();
						itmX.SubItems[1].Text = LArea2ObstList[i].Name;
						itmX.SubItems[2].Text = LArea2ObstList[i].PartName;
						itmX.SubItems[3].Text = LArea2ObstList[i].ID.ToString();

						if (LArea2ObstList[i].Height > -9990)
							itmX.SubItems[4].Text = Functions.ConvertHeight(LArea2ObstList[i].Height, eRoundMode.rmCEIL).ToString();
						else
							itmX.SubItems[4].Text = "";

						itmX.SubItems[5].Text = Functions.ConvertHeight(LArea2ObstList[i].Elevation, eRoundMode.rmCEIL).ToString();
						itmX.SubItems[6].Text = Functions.ConvertHeight(LArea2ObstList[i].HorAccuracy, eRoundMode.rmCEIL).ToString();
						itmX.SubItems[7].Text = Functions.ConvertHeight(LArea2ObstList[i].VertAccuracy, eRoundMode.rmCEIL).ToString();

						ObstaclePlaneReleation opReleation = LArea2ObstList[i].Releation[j];

						itmX.SubItems[8].Text = Functions.ConvertHeight(opReleation.Hsurface, eRoundMode.rmCEIL).ToString();
						itmX.SubItems[9].Text = Functions.ConvertHeight(opReleation.hPent, eRoundMode.rmCEIL).ToString();
						itmX.SubItems[10].Text = opReleation.obstacleArea2.ToString();//.Substring(6)
						if (opReleation.Ignored)
							itmX.SubItems[11].Text =  "Ignored";
						else
							itmX.SubItems[11].Text =  "";

						itmX.Group = group;
						itmX.Tag = LArea2ObstList[i];
					}
				}
				else
				{
					itmX = listView2.Items[k++];

					//if (LObstList[i].ID == 1)
					//{
					//    RecordFontStyle = FontStyle.Bold;
					//    RecordColor = System.Drawing.Color.FromArgb(0XFF0000);
					//}
					//else
					//{
					//    RecordFontStyle = FontStyle.Regular;
					//    RecordColor = System.Drawing.Color.FromArgb(0);
					//}

					itmX.Text = LArea2ObstList[i].Group.ToString();
					itmX.SubItems[1].Text = LArea2ObstList[i].Name;
					itmX.SubItems[2].Text = LArea2ObstList[i].PartName;
					itmX.SubItems[3].Text = LArea2ObstList[i].ID.ToString();

					if (LArea2ObstList[i].Height > -9990)
						itmX.SubItems[4].Text = Functions.ConvertHeight(LArea2ObstList[i].Height, eRoundMode.rmCEIL).ToString();
					else
						itmX.SubItems[4].Text = "";

					itmX.SubItems[5].Text = Functions.ConvertHeight(LArea2ObstList[i].Elevation, eRoundMode.rmCEIL).ToString();
					itmX.SubItems[6].Text = Functions.ConvertHeight(LArea2ObstList[i].HorAccuracy, eRoundMode.rmCEIL).ToString();
					itmX.SubItems[7].Text = Functions.ConvertHeight(LArea2ObstList[i].VertAccuracy, eRoundMode.rmCEIL).ToString();

					itmX.SubItems[8].Text = Functions.ConvertHeight(LArea2ObstList[i].Hsurface, eRoundMode.rmCEIL).ToString();
					itmX.SubItems[9].Text = Functions.ConvertHeight(LArea2ObstList[i].hPent, eRoundMode.rmCEIL).ToString();
					itmX.SubItems[10].Text = LArea2ObstList[i].obstacleArea2.ToString();

					if (LArea2ObstList[i].Ignored)
						itmX.SubItems[11].Text = "Ignored";
					else
						itmX.SubItems[11].Text = "";

					itmX.Group = commonGroup;
					itmX.Tag = LArea2ObstList[i];
				}
			}

			if (_reportBtn.Checked)
				ListView2_SelectedIndexChanged(listView2, new System.EventArgs());
		}

		#endregion

		#region Area 3

		public void FillArea3ObstacleList(ObstacleType[] ObstList)
		{
			int n = ObstList.Length;
			LArea3ObstList = new ObstacleType[n];
			ObstList.CopyTo(LArea3ObstList, 0);
			ListViewItem itmX;

			listView3.Items.Clear();

			for (int i = 0; i < n; i++)
			{
				itmX = listView3.Items.Add(LArea3ObstList[i].Group.ToString());

				itmX.SubItems.Insert(1, new ListViewItem.ListViewSubItem(null, LArea3ObstList[i].Name));
				itmX.SubItems.Insert(2, new ListViewItem.ListViewSubItem(null, LArea3ObstList[i].PartName));
				itmX.SubItems.Insert(3, new ListViewItem.ListViewSubItem(null, LArea3ObstList[i].ID.ToString()));

				if (LArea3ObstList[i].Height > -9990)
					itmX.SubItems.Insert(4, new ListViewItem.ListViewSubItem(null, Functions.ConvertHeight(LArea3ObstList[i].Height, eRoundMode.rmCEIL).ToString()));
				else
					itmX.SubItems.Insert(4, new ListViewItem.ListViewSubItem(null, ""));

				itmX.SubItems.Insert(5, new ListViewItem.ListViewSubItem(null, Functions.ConvertHeight(LArea3ObstList[i].Elevation, eRoundMode.rmCEIL).ToString()));
				itmX.SubItems.Insert(6, new ListViewItem.ListViewSubItem(null, Functions.ConvertHeight(LArea3ObstList[i].HorAccuracy, eRoundMode.rmCEIL).ToString()));
				itmX.SubItems.Insert(7, new ListViewItem.ListViewSubItem(null, Functions.ConvertHeight(LArea3ObstList[i].VertAccuracy, eRoundMode.rmCEIL).ToString()));

				itmX.SubItems.Insert(8, new ListViewItem.ListViewSubItem(null, Functions.ConvertHeight(LArea3ObstList[i].Hsurface, eRoundMode.rmCEIL).ToString()));
				itmX.SubItems.Insert(9, new ListViewItem.ListViewSubItem(null, Functions.ConvertHeight(LArea3ObstList[i].hPent, eRoundMode.rmCEIL).ToString()));
				//itmX.SubItems.Insert(10, new ListViewItem.ListViewSubItem(null, LArea3ObstList[i].codeObstacleArea.ToString().Substring(6)));
				//if (LArea3ObstList[i].Ignored)
				//    itmX.SubItems.Insert(11, new ListViewItem.ListViewSubItem(null, "Ignored"));
				//else
				//    itmX.SubItems.Insert(11, new ListViewItem.ListViewSubItem(null, ""));
			}

			if (_reportBtn.Checked && !Visible)
				Show(GlobalVars.m_Win32Window);
		}

		private void ListView3_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (((ListView)sender).SelectedItems.Count == 0)
				return;

			ListViewItem Item = ((ListView)sender).SelectedItems[0];
			IGraphicsContainer pGraphics = null;

			if ((LArea3ObstList.Length == 0) || (Item == null))
				return;

			pGraphics = GlobalVars.GetActiveView().GraphicsContainer;

			Functions.DeleteGraphicsElement(_pointElem);

			if (LArea3ObstList[Item.Index].pGeoPrj.GeometryType == ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryPoint)
				_pointElem = Functions.DrawPointWithText(LArea3ObstList[Item.Index].pGeoPrj as IPoint, LArea3ObstList[Item.Index].ID.ToString(), 255);
			else if (LArea3ObstList[Item.Index].pGeoPrj.GeometryType == ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryPolygon)
				_pointElem = Functions.DrawPolygon(LArea3ObstList[Item.Index].pGeoPrj as IPolygon, 255);
			else
				_pointElem = Functions.DrawPolyline(LArea3ObstList[Item.Index].pGeoPrj as IPolyline, 255);
		}

		private void ListView3_ColumnClick(object sender, ColumnClickEventArgs e)
		{
			ColumnHeader ColumnHeader = listView3.Columns[e.Column];
			ListViewItem itmX = null;

			int i;
			int n = LArea3ObstList.Length;
			int AbsF3 = System.Math.Abs(_sortF3);

			//FontStyle RecordFontStyle = 0;
			//System.Drawing.Color RecordColor = new System.Drawing.Color();

			if (AbsF3 - 1 == ColumnHeader.Index)
				_sortF3 = -_sortF3;
			else
			{
				if (_sortF3 != 0)
					listView3.Columns[AbsF3 - 1].ImageIndex = -1;

				_sortF3 = ColumnHeader.Index + 1;
			}

			if (_sortF3 > 0)
				ColumnHeader.ImageIndex = 0;
			else
				ColumnHeader.ImageIndex = 1;

			if (ColumnHeader.Index > 0 && ColumnHeader.Index <= 2)
			{
				for (i = 0; i < n; i++)
				{
					switch (ColumnHeader.Index)
					{
						case 1:
							LArea3ObstList[i].sSort = LArea3ObstList[i].Name;
							break;
						case 2:
							LArea3ObstList[i].sSort = LArea3ObstList[i].PartName;
							break;
					}
				}

				if (_sortF3 > 0)
					Functions.shall_SortsSort(LArea3ObstList);
				else
					Functions.shall_SortsSortD(LArea3ObstList);
			}
			else
			{
				for (i = 0; i < n; i++)
				{
					switch (ColumnHeader.Index)
					{
						case 0:
							LArea3ObstList[i].fSort = LArea3ObstList[i].Group;
							break;
						case 3:
							LArea3ObstList[i].fSort = LArea3ObstList[i].ID;
							break;
						case 4:
							LArea3ObstList[i].fSort = LArea3ObstList[i].Height;
							break;
						case 5:
							LArea3ObstList[i].fSort = LArea3ObstList[i].Elevation;
							break;
						case 6:
							LArea3ObstList[i].fSort = LArea3ObstList[i].HorAccuracy;
							break;
						case 7:
							LArea3ObstList[i].fSort = LArea3ObstList[i].VertAccuracy;
							break;
						case 8:
							LArea3ObstList[i].fSort = LArea3ObstList[i].Hsurface;
							break;
						case 9:
							LArea3ObstList[i].fSort = LArea3ObstList[i].hPent;
							break;
						//case 10:
						//    LArea3ObstList[i].fSort = System.Convert.ToInt32(LArea3ObstList[i].codeObstacleArea);
						//    break;
						//case 11:
						//    LArea3ObstList[i].fSort = System.Convert.ToInt32(LArea3ObstList[i].Ignored);
						//    break;
					}
				}

				if (_sortF3 > 0)
					Functions.shall_SortfSort(LArea3ObstList);
				else
					Functions.shall_SortfSortD(LArea3ObstList);
			}

			for (i = 0; i < n; i++)
			{
				itmX = listView3.Items[i];

				//if (LObstList[i].ID == 1)
				//{
				//    RecordFontStyle = FontStyle.Bold;
				//    RecordColor = System.Drawing.Color.FromArgb(0XFF0000);
				//}
				//else
				//{
				//    RecordFontStyle = FontStyle.Regular;
				//    RecordColor = System.Drawing.Color.FromArgb(0);
				//}

				itmX.Text = LArea3ObstList[i].Group.ToString();
				itmX.SubItems[1].Text = LArea3ObstList[i].Name;
				itmX.SubItems[2].Text = LArea3ObstList[i].PartName;
				itmX.SubItems[3].Text = LArea3ObstList[i].ID.ToString();

				if (LArea3ObstList[i].Height > -9990)
					itmX.SubItems[4].Text = Functions.ConvertHeight(LArea3ObstList[i].Height, eRoundMode.rmCEIL).ToString();
				else
					itmX.SubItems[4].Text = "";

				itmX.SubItems[5].Text = Functions.ConvertHeight(LArea3ObstList[i].Elevation, eRoundMode.rmCEIL).ToString();
				itmX.SubItems[6].Text = Functions.ConvertHeight(LArea3ObstList[i].HorAccuracy, eRoundMode.rmCEIL).ToString();
				itmX.SubItems[7].Text = Functions.ConvertHeight(LArea3ObstList[i].VertAccuracy, eRoundMode.rmCEIL).ToString();

				itmX.SubItems[8].Text = Functions.ConvertHeight(LArea3ObstList[i].Hsurface, eRoundMode.rmCEIL).ToString();
				itmX.SubItems[9].Text = Functions.ConvertHeight(LArea3ObstList[i].hPent, eRoundMode.rmCEIL).ToString();
				//itmX.SubItems[10].Text = LArea3ObstList[i].codeObstacleArea.ToString().Substring(6);

				//if (LArea3ObstList[i].Ignored)
				//    itmX.SubItems[11].Text = "Ignored";
				//else
				//    itmX.SubItems[11].Text = "";
			}

			if (_reportBtn.Checked)
				ListView3_SelectedIndexChanged(listView3, new System.EventArgs());
		}

		#endregion

		#region Area 4

		public void FillArea4ObstacleList(List<ObstacleType> ObstList)
		{
			int n = ObstList.Count;
			LArea4ObstList = new ObstacleType[n];
			ObstList.CopyTo(LArea4ObstList);
			//LArea4ObstList.AddRange(ObstList);

			ListViewItem itmX;

			listView4.Items.Clear();

			for (int i = 0; i < n; i++)
			{
				//LArea4ObstList[i] = ObstList[i];
				itmX = listView4.Items.Add(LArea4ObstList[i].Group.ToString());

				itmX.SubItems.Insert(1, new ListViewItem.ListViewSubItem(null, LArea4ObstList[i].Name));
				itmX.SubItems.Insert(2, new ListViewItem.ListViewSubItem(null, LArea4ObstList[i].PartName));
				itmX.SubItems.Insert(3, new ListViewItem.ListViewSubItem(null, LArea4ObstList[i].ID.ToString()));

				if (LArea4ObstList[i].Height > -9990)
					itmX.SubItems.Insert(4, new ListViewItem.ListViewSubItem(null, Functions.ConvertHeight(LArea4ObstList[i].Height, eRoundMode.rmCEIL).ToString()));
				else
					itmX.SubItems.Insert(4, new ListViewItem.ListViewSubItem(null, ""));

				itmX.SubItems.Insert(5, new ListViewItem.ListViewSubItem(null, Functions.ConvertHeight(LArea4ObstList[i].Elevation, eRoundMode.rmCEIL).ToString()));
				itmX.SubItems.Insert(6, new ListViewItem.ListViewSubItem(null, Functions.ConvertHeight(LArea4ObstList[i].HorAccuracy, eRoundMode.rmCEIL).ToString()));
				itmX.SubItems.Insert(7, new ListViewItem.ListViewSubItem(null, Functions.ConvertHeight(LArea4ObstList[i].VertAccuracy, eRoundMode.rmCEIL).ToString()));

				itmX.SubItems.Insert(8, new ListViewItem.ListViewSubItem(null, Functions.ConvertHeight(LArea4ObstList[i].Hsurface, eRoundMode.rmCEIL).ToString()));
				itmX.SubItems.Insert(9, new ListViewItem.ListViewSubItem(null, Functions.ConvertHeight(LArea4ObstList[i].hPent, eRoundMode.rmCEIL).ToString()));
				itmX.SubItems.Insert(10, new ListViewItem.ListViewSubItem(null, LArea4ObstList[i].codeObstacleArea.ToString() + " - RWY " + ((RWYType)LArea4ObstList[i].Tag).Name));
				//itmX.SubItems.Insert(10, new ListViewItem.ListViewSubItem(null, LArea4ObstList[i].codeObstacleArea.ToString() + "- RWY " + ((RWYType)LArea4ObstList[i].Tag).Name));
				//if (LArea4ObstList[i].Ignored)
				//    itmX.SubItems.Insert(7, new ListViewItem.ListViewSubItem(null, "Ignored"));
				//else
				//	itmX.SubItems.Insert(11, new ListViewItem.ListViewSubItem(null, ""));
			}

			if (_reportBtn.Checked && !Visible)
				Show(GlobalVars.m_Win32Window);
		}

		private void ListView4_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (((ListView)sender).SelectedItems.Count == 0)
				return;

			ListViewItem Item = ((ListView)sender).SelectedItems[0];
			IGraphicsContainer pGraphics = null;

			if (LArea4ObstList.Length == 0 || (Item == null))
				return;

			pGraphics = GlobalVars.GetActiveView().GraphicsContainer;

			Functions.DeleteGraphicsElement(_pointElem);

			if (LArea4ObstList[Item.Index].pGeoPrj.GeometryType == ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryPoint)
				_pointElem = Functions.DrawPointWithText(LArea4ObstList[Item.Index].pGeoPrj as IPoint, LArea4ObstList[Item.Index].ID.ToString(), 255);
			else if (LArea4ObstList[Item.Index].pGeoPrj.GeometryType == ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryPolygon)
				_pointElem = Functions.DrawPolygon(LArea4ObstList[Item.Index].pGeoPrj as IPolygon, 255);
			else
				_pointElem = Functions.DrawPolyline(LArea4ObstList[Item.Index].pGeoPrj as IPolyline, 255);
		}

		private void ListView4_ColumnClick(object sender, ColumnClickEventArgs e)
		{
			ColumnHeader ColumnHeader = listView4.Columns[e.Column];
			ListViewItem itmX = null;

			int i;
			int n = LArea4ObstList.Length;
			int AbsF4 = System.Math.Abs(_sortF4);

			//FontStyle RecordFontStyle = 0;
			//System.Drawing.Color RecordColor = new System.Drawing.Color();

			if (AbsF4 - 1 == ColumnHeader.Index)
				_sortF4 = -_sortF4;
			else
			{
				if (_sortF4 != 0)
					listView4.Columns[AbsF4 - 1].ImageIndex = -1;

				_sortF4 = ColumnHeader.Index + 1;
			}

			if (_sortF4 > 0)
				ColumnHeader.ImageIndex = 0;
			else
				ColumnHeader.ImageIndex = 1;

			if ((ColumnHeader.Index > 0 && ColumnHeader.Index <= 2) || ColumnHeader.Index == 10)
			{
				for (i = 0; i < n; i++)
				{
					switch (ColumnHeader.Index)
					{
						case 1:
							LArea4ObstList[i].sSort = LArea4ObstList[i].Name;
							break;
						case 2:
							LArea4ObstList[i].sSort = LArea4ObstList[i].PartName;
							break;
						case 10:
							LArea4ObstList[i].sSort = LArea4ObstList[i].codeObstacleArea.ToString() + " - RWY " + ((RWYType)LArea4ObstList[i].Tag).Name;
							break;
					}
				}

				if (_sortF4 > 0)
					Functions.shall_SortsSort(LArea4ObstList);
				else
					Functions.shall_SortsSortD(LArea4ObstList);
			}
			else
			{
				for (i = 0; i < n; i++)
				{
					switch (ColumnHeader.Index)
					{
						case 0:
							LArea4ObstList[i].fSort = LArea4ObstList[i].Group;
							break;
						case 3:
							LArea4ObstList[i].fSort = LArea4ObstList[i].ID;
							break;
						case 4:
							LArea4ObstList[i].fSort = LArea4ObstList[i].Height;
							break;
						case 5:
							LArea4ObstList[i].fSort = LArea4ObstList[i].Elevation;
							break;
						case 6:
							LArea4ObstList[i].fSort = LArea4ObstList[i].HorAccuracy;
							break;
						case 7:
							LArea4ObstList[i].fSort = LArea4ObstList[i].VertAccuracy;
							break;
						case 8:
							LArea4ObstList[i].fSort = LArea4ObstList[i].Hsurface;
							break;
						case 9:
							LArea4ObstList[i].fSort = LArea4ObstList[i].hPent;
							break;
						case 11:
							LArea4ObstList[i].fSort = System.Convert.ToInt32(LArea4ObstList[i].Ignored);
							break;
					}
				}

				if (_sortF4 > 0)
					Functions.shall_SortfSort(LArea4ObstList);
				else
					Functions.shall_SortfSortD(LArea4ObstList);
			}

			for (i = 0; i < n; i++)
			{
				itmX = listView4.Items[i];

				//if (LObstList[i].ID == 1)
				//{
				//    RecordFontStyle = FontStyle.Bold;
				//    RecordColor = System.Drawing.Color.FromArgb(0XFF0000);
				//}
				//else
				//{
				//    RecordFontStyle = FontStyle.Regular;
				//    RecordColor = System.Drawing.Color.FromArgb(0);
				//}

				itmX.Text = LArea4ObstList[i].Group.ToString();
				itmX.SubItems[1].Text = LArea4ObstList[i].Name;
				itmX.SubItems[2].Text = LArea4ObstList[i].PartName;
				itmX.SubItems[3].Text = LArea4ObstList[i].ID.ToString();

				if (LArea4ObstList[i].Height > -9990)
					itmX.SubItems[4].Text = Functions.ConvertHeight(LArea4ObstList[i].Height, eRoundMode.rmCEIL).ToString();
				else
					itmX.SubItems[4].Text = "";

				itmX.SubItems[5].Text = Functions.ConvertHeight(LArea4ObstList[i].Elevation, eRoundMode.rmCEIL).ToString();
				itmX.SubItems[6].Text = Functions.ConvertHeight(LArea4ObstList[i].HorAccuracy, eRoundMode.rmCEIL).ToString();
				itmX.SubItems[7].Text = Functions.ConvertHeight(LArea4ObstList[i].VertAccuracy, eRoundMode.rmCEIL).ToString();

				itmX.SubItems[8].Text = Functions.ConvertHeight(LArea4ObstList[i].Hsurface, eRoundMode.rmCEIL).ToString();
				itmX.SubItems[9].Text = Functions.ConvertHeight(LArea4ObstList[i].hPent, eRoundMode.rmCEIL).ToString();
				itmX.SubItems[10].Text = LArea4ObstList[i].codeObstacleArea.ToString() + "- RWY " + ((RWYType)LArea4ObstList[i].Tag).Name;

				//if (LArea4ObstList[i].Ignored)
				//    itmX.SubItems[11].Text = "Ignored";
				//else
				//	itmX.SubItems[11].Text = "";
			}

			if (_reportBtn.Checked)
				ListView4_SelectedIndexChanged(listView4, new System.EventArgs());
		}

		#endregion

		private void ReportFrm_Load(object sender, EventArgs e)
		{

		}

		private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
		{
			label1.Visible = tabControl1.SelectedIndex == 1;
			comboBox1.Visible = tabControl1.SelectedIndex == 1;

			//if (tabControl1.SelectedIndex == 1)
			//{
			//    comboBox1.Items.Clear();
			//    int i, n = area2Data.Count;

			//    for (i = 0; i < n; i++)
			//        comboBox1.Items.Add(area2Data[i].RWYName);
			//    if (CurrArea2Index < n)
			//        comboBox1.SelectedIndex = CurrArea2Index;
			//}
		}

		private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
		{
			CurrArea2Index = comboBox1.SelectedIndex;
			if (CurrArea2Index >= 0)
			{
				FillArea2ObstacleList(area2Data[CurrArea2Index].ObstacleList);
			}
		}

		//=====================================================================================================
		private ListView myListView;
		// Determine whether Windows XP or a later
		// operating system is present.
		private bool isRunningXPOrLater = OSFeature.Feature.IsPresent(OSFeature.Themes);

		// Declare a Hashtable array in which to store the groups.
		private Hashtable[] groupTables;
		// Declare a variable to store the current grouping column.
		int groupColumn = 0;

		public void ListViewGroupsExample()
		{
			// Initialize myListView.
			myListView = new ListView();
			myListView.Dock = DockStyle.Fill;
			myListView.View = View.Details;
			myListView.Sorting = SortOrder.Ascending;

			// Create and initialize column headers for myListView.
			ColumnHeader columnHeader0 = new ColumnHeader();
			columnHeader0.Text = "Title";
			columnHeader0.Width = -1;
			ColumnHeader columnHeader1 = new ColumnHeader();
			columnHeader1.Text = "Author";
			columnHeader1.Width = -1;
			ColumnHeader columnHeader2 = new ColumnHeader();
			columnHeader2.Text = "Year";
			columnHeader2.Width = -1;

			// Add the column headers to myListView.
			myListView.Columns.AddRange(new ColumnHeader[] { columnHeader0, columnHeader1, columnHeader2 });

			// Add a handler for the ColumnClick event.
			myListView.ColumnClick += new ColumnClickEventHandler(myListView_ColumnClick);

			// Create items and add them to myListView.
			ListViewItem item0 = new ListViewItem(new string[] 
            {"Programming Windows", 
            "Petzold, Charles", 
            "1998"});
			ListViewItem item1 = new ListViewItem(new string[] 
            {"Code: The Hidden Language of Computer Hardware and Software", 
            "Petzold, Charles", 
            "2000"});
			ListViewItem item2 = new ListViewItem(new string[] 
            {"Programming Windows with C#", 
            "Petzold, Charles", 
            "2001"});
			ListViewItem item3 = new ListViewItem(new string[] 
            {"Coding Techniques for Microsoft Visual Basic .NET", 
            "Connell, John", 
            "2001"});
			ListViewItem item4 = new ListViewItem(new string[] 
            {"C# for Java Developers", 
            "Jones, Allen & Freeman, Adam", 
            "2002"});
			ListViewItem item5 = new ListViewItem(new string[] 
            {"Microsoft .NET XML Web Services Step by Step", 
            "Jones, Allen & Freeman, Adam", 
            "2002"});
			myListView.Items.AddRange(
				new ListViewItem[] { item0, item1, item2, item3, item4, item5 });

			if (isRunningXPOrLater)
			{
				// Create the groupsTable array and populate it with one
				// hash table for each column.
				groupTables = new Hashtable[myListView.Columns.Count];
				for (int column = 0; column < myListView.Columns.Count; column++)
				{
					// Create a hash table containing all the groups // needed for a single column.
					groupTables[column] = CreateGroupsTable(column);
				}

				// Start with the groups created for the Title column.
				SetGroups(0);
			}

			// Initialize the form.this.Controls.Add(myListView);
			this.Size = new System.Drawing.Size(550, 330);
			this.Text = "ListView Groups Example";
		}

		//[STAThread]
		//static void Main()
		//{
		//    Application.EnableVisualStyles();
		//    Application.Run(new ListViewGroupsExample());
		//}

		// Groups the items using the groups created for the clicked
		// column.
		private void myListView_ColumnClick(object sender, ColumnClickEventArgs e)
		{
			// Set the sort order to ascending when changing
			// column groups; otherwise, reverse the sort order.
			if (myListView.Sorting == SortOrder.Descending || (isRunningXPOrLater && (e.Column != groupColumn)))
				myListView.Sorting = SortOrder.Ascending;
			else
				myListView.Sorting = SortOrder.Descending;

			groupColumn = e.Column;

			// Set the groups to those created for the clicked column.
			if (isRunningXPOrLater)
				SetGroups(e.Column);
		}

		// Sets myListView to the groups created for the specified column.
		private void SetGroups(int column)
		{
			// Remove the current groups.
			myListView.Groups.Clear();

			// Retrieve the hash table corresponding to the column.
			Hashtable groups = (Hashtable)groupTables[column];

			// Copy the groups for the column to an array.
			ListViewGroup[] groupsArray = new ListViewGroup[groups.Count];
			groups.Values.CopyTo(groupsArray, 0);

			// Sort the groups and add them to myListView.
			Array.Sort(groupsArray, new ListViewGroupSorter(myListView.Sorting));
			myListView.Groups.AddRange(groupsArray);

			// Iterate through the items in myListView, assigning each
			// one to the appropriate group.
			foreach (ListViewItem item in myListView.Items)
			{
				// Retrieve the subitem text corresponding to the column.
				string subItemText = item.SubItems[column].Text;

				// For the Title column, use only the first letter.
				if (column == 0)
					subItemText = subItemText.Substring(0, 1);

				// Assign the item to the matching group.
				item.Group = (ListViewGroup)groups[subItemText];
			}
		}

		// Creates a Hashtable object with one entry for each unique
		// subitem value (or initial letter for the parent item)
		// in the specified column.
		private Hashtable CreateGroupsTable(int column)
		{
			// Create a Hashtable object.
			Hashtable groups = new Hashtable();

			// Iterate through the items in myListView.
			foreach (ListViewItem item in myListView.Items)
			{
				// Retrieve the text value for the column.
				string subItemText = item.SubItems[column].Text;

				// Use the initial letter instead if it is the first column.
				if (column == 0)
					subItemText = subItemText.Substring(0, 1);

				// If the groups table does not already contain a group
				// for the subItemText value, add a new group using the
				// subItemText value for the group header and Hashtable key.
				if (!groups.Contains(subItemText))
				{
					groups.Add(subItemText, new ListViewGroup(subItemText,
						HorizontalAlignment.Left));
				}
			}

			// Return the Hashtable object.
			return groups;
		}

		//=====================================================================================================
	}
}
