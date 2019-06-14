using System.Collections;
using System.Windows.Forms;

namespace Aran.PANDA.Departure
{
	//public struct ListViewsTag
	//{
	//	public ObstacleContainer obstacles;
	//	public int SortField;
	//}

	public class ListViewColumnSorter : IComparer
	{
		public ListViewColumnSorter()
		{
			ColumnToSort = 0;
			Order = SortOrder.Descending;
			_objectComparer = new CaseInsensitiveComparer();
		}

		public int Compare(object x, object y)
		{
			if (Order == SortOrder.None)
				return 0;

			try
			{
				ListViewItem listviewX = (ListViewItem)x;
				ListViewItem listviewY = (ListViewItem)y;

				if (ColumnToSort > listviewX.SubItems.Count || ColumnToSort > listviewY.SubItems.Count)
					return 0;

				string textX = listviewX.SubItems[ColumnToSort].Text;
				string textY = listviewY.SubItems[ColumnToSort].Text;
				int compareResult;

				if (double.TryParse(textX, out double valX) && double.TryParse(textY, out double valY))
					compareResult = valX.CompareTo(valY);
				else
					compareResult = _objectComparer.Compare(textX, textY);

				if (Order == SortOrder.Ascending)
					return compareResult;

				return -compareResult;
			}
			catch
			{
				return 0;
			}
		}

		public void SortListView(int columnIndex, ListView listview)
		{
			ColumnToSort = System.Math.Abs((int)listview.Tag) - 1;
			listview.Tag = columnIndex + 1;

			if (columnIndex != ColumnToSort)
			{
				listview.Columns[ColumnToSort].ImageIndex = 2;
				ColumnToSort = columnIndex;

				Order = SortOrder.Descending;
				listview.Columns[ColumnToSort].ImageIndex = 1;
			}
			else if (Order == SortOrder.Ascending)
			{
				Order = SortOrder.Descending;
				listview.Columns[ColumnToSort].ImageIndex = 1;
			}
			else
			{
				Order = SortOrder.Ascending;
				listview.Columns[ColumnToSort].ImageIndex = 0;
			}

			listview.Sort();
		}

		private CaseInsensitiveComparer _objectComparer;
		public SortOrder Order { get; set; }
		public int ColumnToSort { get; set; }
	}
}
