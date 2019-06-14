using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geometry;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SigmaChart
{
    public partial class IACProfilesTable : Form
    {
        public IElement DistanceTbl;
        public IElement SpeedTbl;
        //public IElement CircleTbl;
        public IElement OCHTbl;


        public IACProfilesTable()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //dataGridView1.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "new_Column", Name = "Column" + Guid.NewGuid().ToString() });
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //var adrs = dataGridView1.CurrentCellAddress;
            //if (!dataGridView1.Columns[adrs.X].Name.StartsWith("ComboColumn"))
            //    dataGridView1.Columns.Remove(dataGridView1.Columns[adrs.X]);

        }

        public DialogResult BuildForm()
        {
            if (DistanceTbl != null)
                FillGridView(DistanceTbl, Distance_dataGridView,true);
            if (SpeedTbl != null)
                FillGridView(SpeedTbl, Speed_DataGridView, true);
            //if (CircleTbl != null)
            //    FillGridView(CircleTbl, Circl_DataGridView,false);
            if (OCHTbl != null)
                FillOCHGridView(OCHTbl, OCH_DataGridView, false);

            removeColumnButton.Enabled = OCH_DataGridView.ColumnCount > 6;

            return this.ShowDialog();
        }

        private void FillOCHGridView(IElement _Tbl, DataGridView _gridView, bool p)
        {
            ClearGrid(_gridView);

            CreateColumns(6, _gridView, false);
            CreateRows(5, _gridView);

            IGroupElement3 grpEl = (IGroupElement3)_Tbl;
            for (int i = 0; i < grpEl.ElementCount; i++)
            {
                IElement InnerEl = grpEl.get_Element(i);

                IElementProperties3 prp = (IElementProperties3)InnerEl;
                string[] words = prp.Name.Split('|');
                switch (words[0])
                {
                    case ("SigmaTable_OCHHeader"):
                        FillData(InnerEl, _gridView,1);
                        break;
                    case ("Sigma_OCHTable_Column_0_1"):
                        ITextElement txtEl = (ITextElement)InnerEl;
                        string[] values = txtEl.Text.Split('\r');
                        _gridView.Rows[1].Cells[0].Value = values[0];
                        if (values.Length > 1) 
                            _gridView.Rows[2].Cells[0].Value = values[1];
                        if (values.Length > 2)
                        {
                            for (int j = 2; j < values.Length; j++)
                            {
                                _gridView.Rows[3].Cells[0].Value = _gridView.Rows[3].Cells[0].Value + " " + values[j];
                            }
                        }
                        break;
                    case ("SigmaTable_Circling"):
                        FillData(InnerEl, _gridView,1,4);
                        break;
                    default:
                        break;
                }

                if (prp.Name.StartsWith("Cell_"))
                {
                    string[] adress = prp.Name.Split('_');
                    int colInx = Int32.Parse(adress[adress.Length - 2]);
                    int rowInx = Int32.Parse(adress[adress.Length - 1]);

                    ITextElement txtEl = (ITextElement)InnerEl;
                    _gridView.Rows[rowInx].Cells[colInx].Value = txtEl.Text;
                }

            }


            _gridView.AllowUserToAddRows = false;
           
            
           

        }

        private void FillGridView(IElement _Tbl, DataGridView _gridView, bool readOnlyFlag)
        {
  
            string[] words = (_Tbl as IElementProperties3).Name.Split('|');
            int rowCnt = Int16.Parse(words[words.Length - 1]);
            int colCnt = Int16.Parse(words[words.Length - 2]);

            ClearGrid(_gridView);
            CreateColumns(colCnt, _gridView, readOnlyFlag);
            CreateRows(rowCnt, _gridView);

            FillData(_Tbl, _gridView);
        }

        private void ClearGrid(DataGridView _gridView)
        {
            _gridView.Rows.Clear();
            _gridView.Columns.Clear();
        }

        private void CreateColumns(int colCnt, DataGridView _gridView, bool readOnlyFlag, int colWidth =100)
        {
           

            DataGridViewTextBoxColumn First_dataGridViewTextBoxColumn1 = new DataGridViewTextBoxColumn();
            First_dataGridViewTextBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            First_dataGridViewTextBoxColumn1.HeaderText = "Editable";
            First_dataGridViewTextBoxColumn1.Name = "FirstDataGridViewTextBoxColumn";
            First_dataGridViewTextBoxColumn1.Width = 100;

            _gridView.Columns.Add(First_dataGridViewTextBoxColumn1);

            for (int i = 1; i < colCnt; i++)
            {

                DataGridViewTextBoxColumn value_dataGridViewTextBoxColumn1 = new DataGridViewTextBoxColumn();
                //value_dataGridViewTextBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
                value_dataGridViewTextBoxColumn1.HeaderText = "ReadOnly " + (i + 1).ToString();
                value_dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn" + (i + 1).ToString();
                value_dataGridViewTextBoxColumn1.ReadOnly = readOnlyFlag;
                value_dataGridViewTextBoxColumn1.Width = 100;

                _gridView.Columns.Add(value_dataGridViewTextBoxColumn1);

            }

        }

        private void CreateRows(int rowCnt, DataGridView _gridView)
        {
            for (int i = 0; i < rowCnt; i++)
            {
                _gridView.Rows.Add();
            }

        }

        private void FillData(IElement _Tbl, DataGridView _gridView, int colIndxShift = 0, int rowInxShift = 0)
        {
            IGroupElement3 grpEl = (IGroupElement3)_Tbl;
            for (int i = 0; i < grpEl.ElementCount; i++)
            {
                IElement elval = grpEl.get_Element(i);
                IElementProperties3 prp = (IElementProperties3)elval;
                string[] words = prp.Name.Split('_');
                int colInx = Int32.Parse(words[words.Length - 2]);
                int rowInx = Int32.Parse(words[words.Length - 1]);
                
                ITextElement txtEl = (ITextElement)elval;
                _gridView.Rows[rowInx + rowInxShift].Cells[colInx + colIndxShift].Value = txtEl.Text;
            }

            _gridView.AllowUserToAddRows = false;
        }

        private void Distance_dataGridView_CellLeave(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            UpdateTableElement(DistanceTbl,Distance_dataGridView);
            UpdateTableElement(SpeedTbl,Speed_DataGridView);
            UpdateTableElement2(OCHTbl, OCH_DataGridView);

           
        }

        private void UpdateTableElement2(IElement OCHTbl, DataGridView _grid)
        {
            IGroupElement3 _table = OCHTbl as IGroupElement3;

            ///// step 1 рассчитать доступную площадь
            ///// 
            //IPolygon _PrevTableEnvelope = new PolygonClass();
            //IPointCollection pointArr2 = (IPointCollection)_PrevTableEnvelope;


            //pointArr2.AddPoint(new PointClass { X = OCHTbl.Geometry.Envelope.UpperRight.X, Y = OCHTbl.Geometry.Envelope.UpperRight.Y });
            //pointArr2.AddPoint(new PointClass { X = OCHTbl.Geometry.Envelope.LowerRight.X, Y = OCHTbl.Geometry.Envelope.LowerRight.Y });
            //pointArr2.AddPoint(new PointClass { X = OCHTbl.Geometry.Envelope.LowerLeft.X, Y = OCHTbl.Geometry.Envelope.LowerLeft.Y });
            //pointArr2.AddPoint(new PointClass { X = OCHTbl.Geometry.Envelope.UpperLeft.X, Y = OCHTbl.Geometry.Envelope.UpperLeft.Y });

           
            /// найти прототипы таблицы
            /// 
            List<IElement> protoList = new List<IElement>();
            IElement Prototype1 = Get_PrototypeCell_ByName((IGroupElement3)OCHTbl, "SigmaTable_OCHHeader");
            IElement OCHHeader_Col1 = Get_PrototypeCell_ByName((IGroupElement3)Prototype1, "Cell_0_0");
            protoList.Add(OCHHeader_Col1);

            IElement OCHHeader_Col2 = Get_PrototypeCell_ByName((IGroupElement3)Prototype1, "Cell_1_0");
            protoList.Add(OCHHeader_Col2);

            IElement OCHTable_Column_0_1 = Get_PrototypeCell_ByName((IGroupElement3)OCHTbl, "Sigma_OCHTable_Column_0_1");
            protoList.Add(OCHTable_Column_0_1);

            IElement OCHTable_Cell_1_1 = Get_PrototypeCell_ByName((IGroupElement3)OCHTbl, "Cell_1_1");
            protoList.Add(OCHTable_Cell_1_1);

            IElement OCHTable_Cell_2_1 = Get_PrototypeCell_ByName((IGroupElement3)OCHTbl, "Cell_2_1");
            protoList.Add(OCHTable_Cell_2_1);

            IElement Prototype2 = Get_PrototypeCell_ByName((IGroupElement3)OCHTbl, "SigmaTable_Circling");
            IElement OCHCircling_Col0 = Get_PrototypeCell_ByName((IGroupElement3)Prototype2, "Cell_0_0");
            protoList.Add(OCHCircling_Col0);

            IElement OCHCircling_Col1 = Get_PrototypeCell_ByName((IGroupElement3)Prototype2, "Cell_1_0");
            protoList.Add(OCHCircling_Col1);

            /// рассчитать коэффициент "сжатия"
            /// 
            ///  
            string[] wordsDim = (OCHTbl as IElementProperties3).Name.Split('|');
            int rowCnt = Int16.Parse(wordsDim[wordsDim.Length - 1]);
            int colCnt = Int16.Parse(wordsDim[wordsDim.Length - 2]);

            double oldW = OCHTbl.Geometry.Envelope.Envelope.Width;
            double newW = oldW + (OCH_DataGridView.ColumnCount - colCnt -1 ) * OCHTable_Cell_1_1.Geometry.Envelope.Width;
            double wScale = oldW / newW;

            foreach (var _cell in protoList)
            {

                ITransform2D cell_resize = _cell as ITransform2D;
                cell_resize.Scale(_cell.Geometry.Envelope.LowerLeft, wScale, 1);

                double dW = newW > oldW ? oldW - newW : newW - oldW;
                cell_resize.Move(dW, 0);

                //cell_resize = _col as ITransform2D;
                //cell_resize.Move(dW, 0);
            }

            for (int i = 0; i < _table.ElementCount; i++)
            {
                IElement curElement = _table.get_Element(i);

                if (((IElementProperties)curElement).Name.StartsWith("SigmaTable_OCHHeader"))
                {
                    Set_TableCell_ByName((IGroupElement3)curElement, "Cell_0_0", _grid.Rows[0].Cells[1].Value.ToString());
                    for (int colIndx = 2; colIndx < _grid.Columns.Count; colIndx++)
                    {
                        int col = colIndx - 1;
                        _grid.Rows[0].Cells[colIndx].Value = _grid.Rows[0].Cells[colIndx].Value == null ? " " : _grid.Rows[0].Cells[colIndx].Value;
                        Set_TableCell_ByName((IGroupElement3)curElement, "Cell_" + col.ToString() + "_0", _grid.Rows[0].Cells[colIndx].Value.ToString());

                    }
                   
                }
                else if (((IElementProperties)curElement).Name.StartsWith("Sigma_OCHTable_Column_0_1"))
                {
                    ITextElement txtEl = (ITextElement)curElement;
                    txtEl.Text = _grid.Rows[1].Cells[0].Value.ToString() + (char)10 + (char)13;
                    if (_grid.Rows[2].Cells[0].Value.ToString().Length > 0) txtEl.Text += _grid.Rows[2].Cells[0].Value.ToString() +(char)10 + (char)13;
                    if (_grid.Rows[3].Cells[0].Value.ToString().Length > 0) txtEl.Text += _grid.Rows[3].Cells[0].Value.ToString();
                }
                else if (((IElementProperties)curElement).Name.StartsWith("SigmaTable_Circling"))
                {
                    for (int colIndx = 1; colIndx < _grid.Columns.Count; colIndx++)
                    {
                        int col = colIndx - 1;
                        _grid.Rows[4].Cells[colIndx].Value = _grid.Rows[4].Cells[colIndx].Value == null ? " " : _grid.Rows[4].Cells[colIndx].Value;
                        Set_TableCell_ByName((IGroupElement3)curElement, "Cell_" + col.ToString() + "_0", _grid.Rows[4].Cells[colIndx].Value.ToString());

                    }
                }
                else if (((IElementProperties)curElement).Name.StartsWith("Cell_"))
                {
                    ITextElement txtEl = (ITextElement)curElement;
                    string[] words = ((IElementProperties)curElement).Name.Split('_');
                    int colInx = Int32.Parse(words[words.Length - 2]);
                    int rowInx = Int32.Parse(words[words.Length - 1]);
                    _grid.Rows[rowInx].Cells[colInx].Value = _grid.Rows[rowInx].Cells[colInx].Value == null ? " " : _grid.Rows[rowInx].Cells[colInx].Value;
                    txtEl.Text = _grid.Rows[rowInx].Cells[colInx].Value.ToString();
                }
            }
        }

        private static void Set_TableCell_ByName(IGroupElement3 dinamic_el, string textName, string cellValue)
        {
            IGroupElement3 grpEl = (IGroupElement3)dinamic_el;
            for (int i = 0; i < grpEl.ElementCount; i++)
            {
                IElement elval = grpEl.get_Element(i);
                IElementProperties3 prp = (IElementProperties3)elval;
                if (!prp.Name.StartsWith(textName)) continue;
                ITextElement txtEl = (ITextElement)elval;
                txtEl.Text = cellValue;
                break;
            }
        }


        private void UpdateTableElement(IElement IEement_Tbl, DataGridView _grid)
        {
            IGroupElement3 grpEl = (IGroupElement3)IEement_Tbl;

            for (int RowIndex = 0; RowIndex < _grid.Rows.Count; RowIndex++)
            {
                for (int ColumnIndex = 0; ColumnIndex < _grid.Columns.Count; ColumnIndex++)
                {
                    int col = ColumnIndex;
                    int row = RowIndex;
                    Set_TableCell_ByName((IGroupElement3)IEement_Tbl, "Cell_" + col.ToString() + "_" + RowIndex.ToString(), _grid.Rows[RowIndex].Cells[ColumnIndex].Value.ToString());

                }
            }
        }

        private void OCH_DataGridView_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.ColumnIndex == 0 && e.RowIndex == 0)
                e.AdvancedBorderStyle.Right = DataGridViewAdvancedCellBorderStyle.None;
            else if (e.ColumnIndex == 1 && e.RowIndex == 0)
                e.AdvancedBorderStyle.Left = DataGridViewAdvancedCellBorderStyle.None;

            else if (e.ColumnIndex == 0 && e.RowIndex == 1)
                e.AdvancedBorderStyle.Bottom = DataGridViewAdvancedCellBorderStyle.None;
            else if (e.ColumnIndex == 0 && e.RowIndex == 2)
            {
                e.AdvancedBorderStyle.Top = DataGridViewAdvancedCellBorderStyle.None;
                e.AdvancedBorderStyle.Bottom = DataGridViewAdvancedCellBorderStyle.None;
            }
            else if (e.ColumnIndex == 0 && e.RowIndex == 3)
                e.AdvancedBorderStyle.Top = DataGridViewAdvancedCellBorderStyle.None;

            else if (e.ColumnIndex == 0 && e.RowIndex == 4)
                e.AdvancedBorderStyle.Right = DataGridViewAdvancedCellBorderStyle.None;
            else if (e.ColumnIndex == 1 && e.RowIndex == 4)
                e.AdvancedBorderStyle.Left = DataGridViewAdvancedCellBorderStyle.None;
            else
                e.AdvancedBorderStyle.All = OCH_DataGridView.AdvancedCellBorderStyle.All;

        }

        private void OCH_DataGridView_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {

            if (e.ColumnIndex == 0 && e.RowIndex == 0)
            {
                //OCH_DataGridView.Rows[0].Cells[0].Value = e.Value;

                e.Value = "";
                e.FormattingApplied = true;

            }
            else if (e.ColumnIndex == 0 && e.RowIndex == 4)
            {
                //OCH_DataGridView.Rows[4].Cells[0].Value = e.Value;

                e.Value = "";
                e.FormattingApplied = true;
            }

            
        }

        private void IACProfilesTable_Load(object sender, EventArgs e)
        {
            OCH_DataGridView.AutoGenerateColumns = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            DataGridViewTextBoxColumn value_dataGridViewTextBoxColumn1 = new DataGridViewTextBoxColumn();
            //value_dataGridViewTextBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            value_dataGridViewTextBoxColumn1.HeaderText = "";
            value_dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn";
            value_dataGridViewTextBoxColumn1.Width = 100;

            OCH_DataGridView.Columns.Add(value_dataGridViewTextBoxColumn1);

            removeColumnButton.Enabled = OCH_DataGridView.ColumnCount > 6;

        }

        private void removeColumnButton_Click(object sender, EventArgs e)
        {
            OCH_DataGridView.Columns.Remove(OCH_DataGridView.Columns[OCH_DataGridView.Columns.Count - 1]);
            removeColumnButton.Enabled = OCH_DataGridView.ColumnCount > 6;

        }

        private static IElement Get_PrototypeCell_ByName(IGroupElement3 dinamic_el, string textName)
        {
            IElement res = null;
            IGroupElement3 grpEl = (IGroupElement3)dinamic_el;
            for (int i = 0; i < grpEl.ElementCount; i++)
            {
                IElement elval = grpEl.get_Element(i);
                IElementProperties3 prp = (IElementProperties3)elval;
                if (!prp.Name.StartsWith(textName)) continue;
                res = elval;
                break;
            }

            return res;
        }

    }
}
