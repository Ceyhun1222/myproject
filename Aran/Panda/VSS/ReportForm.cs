using Aran.AranEnvironment.Symbols;
using Aran.PANDA.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace Aran.PANDA.Vss
{
	public partial class ReportForm : Form
	{
		private int _selectedElementId;
		private PointSymbol _pointSymbol;
		private FillSymbol _polygonFillSymbol;
		private int _sortIndex;
		private SortOrder _sortOrder;


		public ReportForm()
		{
			InitializeComponent();

			Obstacles = new List<VssObstacle>();

			_pointSymbol = new PointSymbol(ePointStyle.smsCircle, Color.Red.ToAranRgb(), 8);
			_polygonFillSymbol = new FillSymbol();
			_polygonFillSymbol.Color = _pointSymbol.Color;
			_polygonFillSymbol.Outline = new LineSymbol(eLineStyle.slsDash,
				Aran.PANDA.Common.ARANFunctions.RGB(255, 0, 0), 4);
			_polygonFillSymbol.Style = eFillStyle.sfsNull;

			var pi = ui_dgv.GetType().GetProperty("DoubleBuffered",
				BindingFlags.NonPublic | BindingFlags.Instance);
			pi.SetValue(ui_dgv, true, null);

			var distUnitText = " ( " + Globals.UnitConverter.DistanceUnit + ")";
			var heightUnitText = " (" + Globals.UnitConverter.HeightUnit + ")";

			ui_colX.HeaderText += distUnitText;
			ui_colHSurface.HeaderText += heightUnitText;
			ui_colHeight.HeaderText += heightUnitText;
			ui_colHPenetrate.HeaderText += heightUnitText;
		}


		public List<VssObstacle> Obstacles { get; private set; }


		public void RefreshGrid()
		{
			ui_dgv.RowCount = Obstacles.Count;
			ui_dgv.Refresh();
		}

		private void Close_Click(object sender, EventArgs e)
		{
			Close();
		}

		protected override void OnFormClosed(FormClosedEventArgs e)
		{
			Globals.AranEnv.Graphics.DeleteGraphic(_selectedElementId);

			base.OnFormClosed(e);
		}

		private void DGV_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
		{
			if (e.RowIndex < 0 || e.RowIndex >= Obstacles.Count)
				return;

			var obs = Obstacles[e.RowIndex];

			switch (e.ColumnIndex)
			{
				case 0:
					e.Value = obs.Name;
					break;
				case 1:
					e.Value = Globals.UnitConverter.DistanceToDisplayUnits(obs.VssX);
					break;
				case 2:
					e.Value = Globals.UnitConverter.HeightToDisplayUnits(obs.HSurface);
					break;
				case 3:
					e.Value = Globals.UnitConverter.HeightToDisplayUnits(obs.Height);
					break;
				case 4:
					e.Value = Globals.UnitConverter.HeightToDisplayUnits(obs.HPenetrate);
					break;
				case 5:
					if (obs.NeededSlope != null)
						e.Value = ARANMath.RoundToStr(obs.NeededSlope.Value, 0.01, eRoundMode.CEIL);
					break;
			}
		}

		private void DGV_CurrentCellChanged(object sender, EventArgs e)
		{
			Globals.AranEnv.Graphics.DeleteGraphic(_selectedElementId);

			if (ui_dgv.CurrentRow == null)
				return;

			var obs = Obstacles[ui_dgv.CurrentRow.Index];
			if (obs.LocationPrj is Aran.Geometries.Point)
				_selectedElementId = Globals.AranEnv.Graphics.DrawPointWithText(obs.LocationPrj as Aran.Geometries.Point, obs.Name, _pointSymbol);
			else if (obs.LocationPrj is Aran.Geometries.MultiLineString)
				_selectedElementId = Globals.AranEnv.Graphics.DrawMultiLineString(obs.LocationPrj as Aran.Geometries.MultiLineString, 4, _pointSymbol.Color);
			else if (obs.LocationPrj is Aran.Geometries.MultiPolygon)
				_selectedElementId = Globals.AranEnv.Graphics.DrawMultiPolygon(obs.LocationPrj as Aran.Geometries.MultiPolygon, _polygonFillSymbol);
		}

		private void DGV_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
		{
			if (e.RowIndex < 0 || e.RowIndex >= Obstacles.Count)
				return;

			if (e.ColumnIndex == ui_colHPenetrate.Index)
			{
				var obs = Obstacles[e.RowIndex];

				if (obs.HPenetrate > 0)
				{
					e.CellStyle.ForeColor = Color.Red;
					e.CellStyle.SelectionForeColor = Color.Red;
				}
			}
		}

		private void DGV_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
		{
			var col = ui_dgv.Columns[e.ColumnIndex];

			foreach (DataGridViewColumn colItem in ui_dgv.Columns)
			{
				if (colItem != col)
					colItem.HeaderCell.SortGlyphDirection = SortOrder.None;
			}


			if (col.HeaderCell.SortGlyphDirection != SortOrder.Ascending)
				col.HeaderCell.SortGlyphDirection = SortOrder.Ascending;
			else
				col.HeaderCell.SortGlyphDirection = SortOrder.Descending;

			_sortIndex = e.ColumnIndex;
			_sortOrder = col.HeaderCell.SortGlyphDirection;

			Obstacles.Sort(CompareObstacle);

			ui_dgv.Refresh();
		}

		private int CompareObstacle(VssObstacle obs1, VssObstacle obs2)
		{
			var res = 0;

			switch (_sortIndex)
			{
				case 0:
					res = obs1.Name.CompareTo(obs2.Name);
					break;
				case 1:
					res = obs1.VssX.CompareTo(obs2.VssX);
					break;
				case 2:
					res = obs1.HSurface.CompareTo(obs2.HSurface);
					break;
				case 3:
					res = obs1.Height.CompareTo(obs2.Height);
					break;
				case 4:
					res = obs1.HPenetrate.CompareTo(obs2.HPenetrate);
					break;
				case 5:
					if (obs1.NeededSlope == null)
					{
						res = -1;
						break;
					}
					if (obs2.NeededSlope == null)
					{
						res = 1;
						break;
					}

					res = obs1.NeededSlope.Value.CompareTo(obs2.NeededSlope.Value);
					break;
			}

			if (_sortOrder == SortOrder.Descending)
				res = -1 * res;

			return res;
		}

	}

}
