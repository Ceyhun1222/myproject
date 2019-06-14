using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SigmaChart
{
    public class ListViewItemComparer : IComparer
    {
        private int _columnIndex;
        public int ColumnIndex
        {
            get
            {
                return _columnIndex;
            }
            set
            {
                _columnIndex = value;
            }
        }

        private SortOrder _sortDirection;
        public SortOrder SortDirection
        {
            get
            {
                return _sortDirection;
            }
            set
            {
                _sortDirection = value;
            }
        }

     

        public ListViewItemComparer()
        {
            _sortDirection = SortOrder.None;
        }

        public int Compare(object x, object y)
        {
            ListViewItem lviX = x as ListViewItem;
            ListViewItem lviY = y as ListViewItem;

            int result;

            if (lviX == null && lviY == null)
            {
                result = 0;
            }
            else if (lviX == null)
            {
                result = -1;
            }

            else if (lviY == null)
            {
                result = 1;
            }

            result = string.Compare(
                        lviX.SubItems[ColumnIndex].Text,
                        lviY.SubItems[ColumnIndex].Text,
                        false);

            if (SortDirection == SortOrder.Descending)
            {
                return -result;
            }
            else
            {
                return result;
            }
        }
    }
}

