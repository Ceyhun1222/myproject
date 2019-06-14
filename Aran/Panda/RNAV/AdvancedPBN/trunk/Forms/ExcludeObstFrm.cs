using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;

using System.Linq;
using System.Text;
using System.Windows.Forms;
using Aran.Panda.Common;
using Aran.Geometries;

namespace Aran.PANDA.RNAV.SGBAS
{
	public partial class ExcludeObstFrm : Form
	{
		private int pPointElem;
		private int pGeomElem;

		private ObstacleContainer LObstaclesPage1;
		//private int SortF1;
		private int DlgResult;

		public ExcludeObstFrm()
		{
			InitializeComponent();
		}

		public bool ExcludeOstacles(ref ObstacleContainer Obstacles, Form mainForm)
		{
			ListView1.Items.Clear();

			int M = Obstacles.Obstacles.Length;
			int N = Obstacles.Parts.Length;

			LObstaclesPage1.Obstacles = new Obstacle[M];
			LObstaclesPage1.Parts = new ObstacleData[N];
			if (M == 0)
				return false;

			int I;
			for (I = 0; I < N; I++)
				LObstaclesPage1.Parts[I] = Obstacles.Parts[I];

			System.Windows.Forms.ListViewItem itmX;
			for (I = 0; I < M; I++)
			{
				LObstaclesPage1.Obstacles[I] = Obstacles.Obstacles[I];
				itmX = ListView1.Items.Add(Obstacles.Obstacles[I].TypeName);
				itmX.SubItems.Insert(1, new System.Windows.Forms.ListViewItem.ListViewSubItem(null, Obstacles.Obstacles[I].UnicalName));
				itmX.SubItems.Insert(2, new System.Windows.Forms.ListViewItem.ListViewSubItem(null, GlobalVars.unitConverter.HeightToDisplayUnits(Obstacles.Obstacles[I].Height, eRoundMode.NERAEST).ToString()));
				itmX.Checked = Obstacles.Obstacles[I].IgnoredByUser;
			}

			//SortF1 = -1;
			//if(SortF1 != 0)
			//{
			//    System.Windows.Forms.ColumnHeader pColumnHeader = ListView1.ColumnHeaders[Abs(SortF1)];
			//    SortF1 = -SortF1;
			//    ListView1_ColumnClick(pColumnHeader)
			//}

			DlgResult = 0;
			bool result = false;

			this.ShowDialog(mainForm);

			if (DlgResult > 0)
			{
				for (I = 0; I < M; I++)
				{
					itmX = ListView1.Items[I];
					Obstacles.Obstacles[I].IgnoredByUser = itmX.Checked;
				}
				result = true;
			}

			GlobalVars.gAranGraphics.SafeDeleteGraphic(pPointElem);
			return result;
		}


		private void ListView1_ColumnClick(object sender, ColumnClickEventArgs e)
		{
			//System.Windows.Forms.ColumnHeader ColumnHdr = ListView1.Columns[e.Column];
			//string[] ZoneNames = new string[] { "Approach", "Missed approach" };

			//ListView1.Sorting = SortOrder.None;
			//int I, N = LObstaclesPage1.Obstacles.Length;

			//if (System.Math.Abs(SortF1) - 1 == ColumnHdr.Index)
			//    SortF1 = -SortF1;
			//else
			//{
			//    if (SortF1 != 0)
			//        ListView1.Columns[System.Math.Abs(SortF1) - 1].ImageIndex = -1;
			//    SortF1 = ColumnHdr.Index + 1;
			//}

			//if (SortF1 > 0)
			//    ColumnHdr.ImageIndex = 0;
			//else
			//    ColumnHdr.ImageIndex = 1;

			//if (ColumnHdr.Index == 2)
			//{
			//    for (I = 0; I < N; I++)
			//        LObstaclesPage1.Obstacles[I].fSort = LObstaclesPage1.Obstacles[I].Height;

			//    if (SortF1 > 0)
			//        Functions.shall_SortfSort(LObstaclesPage1);
			//    else
			//        Functions.shall_SortfSortD(LObstaclesPage1);
			//}
			//else
			//{
			//    for (I = 0; I < N; I++)
			//        switch (ColumnHdr.Index)
			//        {
			//            case 0:
			//                LObstaclesPage1.Obstacles[I].sSort = LObstaclesPage1.Obstacles[I].TypeName;
			//                break;
			//            case 1:
			//                LObstaclesPage1.Obstacles[I].sSort = LObstaclesPage1.Obstacles[I].UnicalName;
			//                break;
			//        }

			//    if (SortF1 > 0)
			//        Functions.shall_SortsSort(LObstaclesPage1);
			//    else
			//        Functions.shall_SortsSortD(LObstaclesPage1);
			//}

			//int M = ListView1.Items.Count;
			//for (I = N + 1; I < M; I++)
			//    ListView1.Items.RemoveAt(0);
			//System.Windows.Forms.ListViewItem itmX;

			//for (I = 0; I < N; I++)
			//{
			//    M = ListView1.Items.Count;

			//    if (M <= I)
			//        itmX = ListView1.Items.Add(LObstaclesPage1.Obstacles[I].TypeName);
			//    else
			//    {
			//        itmX = ListView1.Items[I];
			//        itmX.Text = LObstaclesPage1.Obstacles[I].TypeName;
			//    }

			//    if (itmX.SubItems.Count > 1)
			//        itmX.SubItems[1].Text = LObstaclesPage1.Obstacles[I].UnicalName;
			//    else
			//        itmX.SubItems.Insert(1, new System.Windows.Forms.ListViewItem.ListViewSubItem(null, LObstaclesPage1.Obstacles[I].UnicalName));

			//    if (itmX.SubItems.Count > 2)
			//        itmX.SubItems[2].Text = GlobalVars.unitConverter.HeightToDisplayUnits(LObstaclesPage1.Obstacles[I].Height, eRoundMode.NERAEST).ToString();
			//    else
			//        itmX.SubItems.Insert(2, new System.Windows.Forms.ListViewItem.ListViewSubItem(null, GlobalVars.unitConverter.HeightToDisplayUnits(LObstaclesPage1.Obstacles[I].Height, eRoundMode.NERAEST).ToString()));


			//    itmX.Checked = LObstaclesPage1.Obstacles[I].IgnoredByUser;
			//}

			//ListView1_SelectedIndexChanged(ListView1, null);
		}

		private void ListView1_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (ListView1.SelectedItems.Count == 0)
				return;

			System.Windows.Forms.ListViewItem Item = ListView1.SelectedItems[0];
			if (LObstaclesPage1.Obstacles.Length == 0 || Item == null)
				return;

			GlobalVars.gAranGraphics.SafeDeleteGraphic(pPointElem);
			GlobalVars.gAranGraphics.SafeDeleteGraphic(pGeomElem);

			Point pPtTmp;

			if (LObstaclesPage1.Obstacles[Item.Index].pGeomPrj.Type == GeometryType.Point)
			{
				pPtTmp = (Point)LObstaclesPage1.Obstacles[Item.Index].pGeomPrj;
				pGeomElem = -1;
			}
			else if (LObstaclesPage1.Obstacles[Item.Index].pGeomPrj.Type == GeometryType.MultiLineString)
			{
				MultiLineString multiLine = (MultiLineString)LObstaclesPage1.Obstacles[Item.Index].pGeomPrj;
				pPtTmp = multiLine.HalfPoint;
				pGeomElem = GlobalVars.gAranGraphics.DrawMultiLineString(multiLine, 255, 2);
			}
			else //if LObstaclesPage11[Item.Index].pGeomPrj.GeometryType = ArcGIS.Geometry.esriGeometryType.esriGeometryPolygon)
			{
				MultiPolygon multiPolygon = (MultiPolygon)LObstaclesPage1.Obstacles[Item.Index].pGeomPrj;
				pPtTmp = multiPolygon.Centroid;
				pGeomElem = GlobalVars.gAranGraphics.DrawMultiPolygon(multiPolygon, 255, AranEnvironment.Symbols.eFillStyle.sfsDiagonalCross);
			}

			pPointElem = GlobalVars.gAranGraphics.DrawPointWithText(pPtTmp, 255, LObstaclesPage1.Obstacles[Item.Index].UnicalName);
		}

		private void OKbtn_Click(object sender, EventArgs e)
		{
			DlgResult = 1;
			Hide();
		}

		private void CancelBtn_Click(object sender, EventArgs e)
		{
			DlgResult = 0;
			Hide();
		}
	}
}
