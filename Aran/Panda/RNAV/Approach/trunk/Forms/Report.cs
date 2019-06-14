using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Aran.PANDA.Common;
using Aran.Geometries;

namespace Aran.PANDA.RNAV.Approach
{
	public partial class Report : Form
	{
		private int _pointElem;
		private int _geomElem;

		private CheckBox _reportBtn;
		ObstacleContainer _obstaclesPage1;

		//private double MOC01;
		//private long Ix01ID;
		//private sbyte SortF01;

		public Report()
		{
			InitializeComponent();
		}

		internal void Init(CheckBox reportBtn)          //, int HelpContext = 0
		{
			_reportBtn = reportBtn;             //_helpContextID = HelpContext;
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


		internal void FillPage01(ObstacleContainer Obstacles, double MOC, int Ix)
		{
			if (mainTabControl.SelectedIndex == 0)
			{
				GlobalVars.gAranGraphics.SafeDeleteGraphic(_pointElem);
				GlobalVars.gAranGraphics.SafeDeleteGraphic(_geomElem);
			}

			dataGridView01.RowCount = 0;


			int n = Obstacles.Parts.Length;
			_obstaclesPage1.Parts = new ObstacleData[n];

			lblCount.Text = "Count : " + n;

			if (n <= 0)
				return;

			int m = Obstacles.Obstacles.Length;
			_obstaclesPage1.Obstacles = new Obstacle[m];
			Array.Copy(Obstacles.Obstacles, _obstaclesPage1.Obstacles, m);

			for (int i = 0; i < n; i++)
			{
				DataGridViewRow row = new DataGridViewRow();
				row.Tag = _obstaclesPage1.Parts[i];

				DataGridViewTextBoxCell cell = new DataGridViewTextBoxCell();
				cell.Value = Obstacles.Obstacles[Obstacles.Parts[i].Owner].TypeName;
				row.Cells.Add(cell);

				cell = new DataGridViewTextBoxCell();
				cell.Value = Obstacles.Obstacles[Obstacles.Parts[i].Owner].UnicalName;
				row.Cells.Add(cell);

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[2].Value = GlobalVars.unitConverter.HeightToDisplayUnits(Obstacles.Obstacles[i].Height , eRoundMode.NEAREST).ToString();

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[3].Value = GlobalVars.unitConverter.HeightToDisplayUnits(MOC, eRoundMode.NEAREST).ToString();

				double ReqH = Obstacles.Obstacles[i].Height + MOC;
				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[4].Value = GlobalVars.unitConverter.HeightToDisplayUnits(ReqH, eRoundMode.NEAREST).ToString();

				//row.Cells.Add(new DataGridViewTextBoxCell());
				//row.Cells[5].Value = GlobalVars.unitConverter.HeightToDisplayUnits(Obstacles.Obstacles[i].HorAccuracy, eRoundMode.NEAREST).ToString();

				//row.Cells.Add(new DataGridViewTextBoxCell());
				//row.Cells[6].Value = GlobalVars.unitConverter.HeightToDisplayUnits(Obstacles.Obstacles[i].VertAccuracy, eRoundMode.NEAREST).ToString();

				//row.Cells.Add(new DataGridViewTextBoxCell());

				//var obstacleGeoType = Obstacles.Obstacles[Obstacles.Parts[i].Owner].pGeomPrj.Type;
				//if (obstacleGeoType == Geometries.GeometryType.Point)
				//	row.Cells[7].Value = "Point";
				//else if (obstacleGeoType == Geometries.GeometryType.LineString || obstacleGeoType == Geometries.GeometryType.MultiLineString)
				//	row.Cells[7].Value = "Line";
				//else
				//	row.Cells[7].Value = "Polygon";


				dataGridView01.Rows.Add(row);

				//if (i == Ix)
				//{
				//	row.Cells[0].Style.ForeColor = System.Drawing.Color.FromArgb(0XFF0000);
				//	row.Cells[0].Style.Font = new System.Drawing.Font(Font, System.Drawing.FontStyle.Bold);

				//	for (int j = 1; j < row.Cells.Count; j++)
				//	{
				//		row.Cells[j].Style.ForeColor = row.Cells[0].Style.ForeColor;
				//		row.Cells[j].Style.Font = row.Cells[0].Style.Font;
				//	}
				//}

				//row.Cells[3].Style.Font = new System.Drawing.Font(Font, System.Drawing.FontStyle.Bold);
			}

			//	st.Stop();
			//	MessageBox.Show(st.Elapsed.ToString());

			//SetTabVisible(0, true);
			if (_reportBtn.Checked && !Visible)
				Show(GlobalVars.Win32Window);

		}

		private void dataGridView01_RowEnter(object sender, DataGridViewCellEventArgs e)
		{
			if (e.RowIndex < 0 || _obstaclesPage1.Parts.Length == 0 || !Visible)
				return;

			GlobalVars.gAranGraphics.SafeDeleteGraphic(_pointElem);
			GlobalVars.gAranGraphics.SafeDeleteGraphic(_geomElem);

			ObstacleData obsData = (ObstacleData)dataGridView01.Rows[e.RowIndex].Tag;
			Obstacle owner = _obstaclesPage1.Obstacles[obsData.Owner];

			Geometry pGeometry = owner.pGeomPrj;

			if (pGeometry.Type == Geometries.GeometryType.Point)
				_pointElem = GlobalVars.gAranGraphics.DrawPointWithText(obsData.pPtPrj, owner.UnicalName, 255);
			else
			{
				//if (!obsData.Prima)
				//	_pointElem = GlobalVars.gAranGraphics.DrawPointWithText(obsData.pPtPrj, owner.UnicalName, _exactVertexSymbol);
				//else
					_pointElem = GlobalVars.gAranGraphics.DrawPointWithText(obsData.pPtPrj, owner.UnicalName, 255);

				if (pGeometry.Type == Geometries.GeometryType.LineString)
					_geomElem = GlobalVars.gAranGraphics.DrawLineString((LineString)pGeometry, 2, 255);
				else if (pGeometry.Type == Geometries.GeometryType.MultiLineString)
					_geomElem = GlobalVars.gAranGraphics.DrawMultiLineString((MultiLineString)pGeometry, 2, 255);
				else if (pGeometry.Type == Geometries.GeometryType.Polygon)
					_geomElem = GlobalVars.gAranGraphics.DrawPolygon((Polygon)pGeometry,
						AranEnvironment.Symbols.eFillStyle.sfsDiagonalCross, 255);
				else if (pGeometry.Type == Geometries.GeometryType.MultiPolygon)
					_geomElem = GlobalVars.gAranGraphics.DrawMultiPolygon((MultiPolygon)pGeometry,
						AranEnvironment.Symbols.eFillStyle.sfsDiagonalCross, 255);
			}

		}
	}
}
