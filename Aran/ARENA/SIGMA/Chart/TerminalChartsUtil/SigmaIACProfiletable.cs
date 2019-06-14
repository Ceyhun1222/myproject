using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SigmaChart
{
    public class SigmaIACProfileTable
    {
        public int TableColCount { get; set; }
        public int TableRowCount { get; set; }

        public SigmaIACProfileTable()
        {
            this.TableColCount = 0;
            this.TableRowCount = 0;
        }

        public SigmaIACProfileTable(int colCount, int rowCount)
        {
            this.Content = new List<SigmaProfileTableRow>();

            for (int i = 0; i < rowCount; i++)
            {
                SigmaProfileTableRow row = new SigmaProfileTableRow();
                for (int j = 0; j < colCount; j++)
                {
                    row.ProfileRowCell.Add("");
                }

                this.Content.Add(row);
            }

            this.TableColCount = colCount;
            this.TableRowCount = rowCount;
        }

        public List<SigmaProfileTableRow> Content { get; set; }

        public void fillRow(int rowIndex, List<object> rowContent)
        {
            SigmaProfileTableRow row = this.Content[rowIndex];
            for (int i = 0; i < this.TableColCount; i++)
            {
                if (i > rowContent.Count - 1) break;
                row.ProfileRowCell[i] = rowContent[i].ToString();
            }
        }

        public void addColumn()
        {
            foreach (SigmaProfileTableRow row in this.Content)
            {
                row.ProfileRowCell.Add("");
            }
        }

        public void deleteColumn(int colIndex)
        {
            if (colIndex > this.TableColCount) return;

            foreach (SigmaProfileTableRow row in this.Content)
            {
               string cell = row.ProfileRowCell[colIndex];
               row.ProfileRowCell.Remove(cell);
            }

            this.TableColCount--;
        }

        public void addRow()
        {
            SigmaProfileTableRow row = new SigmaProfileTableRow();
            for (int j = 0; j < this.TableColCount; j++)
            {
                row.ProfileRowCell.Add("");
            }

            this.Content.Add(row);

        }

        public void deleteRow(int rowIndex)
        {
            if (rowIndex > this.TableRowCount) return;


            SigmaProfileTableRow row = this.Content[rowIndex];
            this.Content.Remove(row);
            this.TableRowCount--;
        }
    }

    public class SigmaProfileTableRow
    {
        public SigmaProfileTableRow()
        {
            this.ProfileRowCell = new List<string>();
        }

        public List<string> ProfileRowCell { get; set; }
    }
}
