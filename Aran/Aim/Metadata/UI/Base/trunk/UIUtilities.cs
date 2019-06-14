using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Aran.Aim.Features;
using Aran.Aim.DataTypes;
using Aran.Converters;

namespace Aran.Aim.Metadata.UI
{
    public class UIUtilities
    {
        public static void FillColumns (AimClassInfo classInfo, DataGridView dgv)
        {
            FillDataGridColumns (classInfo, dgv);
        }

        public static void SetRow (DataGridView dgv, Feature feature, int rowIndex = -1)
        {
            bool isNewRow = (rowIndex == -1);
            DataGridViewRow row;

            if (isNewRow)
                row = new DataGridViewRow ();
            else
                row = dgv.Rows [rowIndex];

            row.Tag = feature;

            var ts = feature.TimeSlice;
            if (ts != null)
            {
                var cells = new DataGridViewCell [3];
                if (isNewRow)
                {
                    for (int i = 0; i < cells.Length; i++)
                        cells [i] = (DataGridViewCell) dgv.Columns [i].CellTemplate.Clone ();
                }
                else
                {
                    for (int i = 0; i < cells.Length; i++)
                        cells [i] = row.Cells [i];
                }

                cells [0].Value = ts.ValidTime.BeginPosition.ToString ("yyyy-MM-dd");
                if (ts.ValidTime.EndPosition != null)
                    cells [1].Value = ts.ValidTime.EndPosition.Value.ToString ("yyyy-MM-dd");
                cells [2].Value = string.Format ("{0}, {1}", ts.SequenceNumber, ts.CorrectionNumber);

                if (isNewRow)
                {
                    for (int i = 0; i < cells.Length; i++)
                        row.Cells.Add (cells [i]);
                }

                if (feature.WorksPackageId > 0) {
                    cells[0].Style.BackColor = System.Drawing.Color.GreenYellow;
                    cells[0].Style.SelectionBackColor = System.Drawing.Color.OliveDrab;
                }
            }

            for (int i = 3; i < dgv.Columns.Count; i++)
            {
                DataGridViewCell cell;
                if (isNewRow)
                    cell = (DataGridViewCell) dgv.Columns [i].CellTemplate.Clone ();
                else
                    cell = row.Cells [i];

                AimPropInfo propInfo = dgv.Columns [i].Tag as AimPropInfo;
                if (propInfo != null)
                {

                    IAimProperty aimPropValue = ((IAimObject) feature).GetValue (propInfo.Index);

                    if (aimPropValue != null)
                    {
                        if (aimPropValue.PropertyType == AimPropertyType.AranField)
                        {
                            cell.Value = ((IEditAimField) aimPropValue).FieldValue;
                        }
                        else if (aimPropValue.PropertyType == AimPropertyType.DataType)
                        {
                            if (propInfo.PropType.SubClassType == AimSubClassType.ValClass)
                            {
                                IEditValClass editValClass = (IEditValClass) aimPropValue;
                                AimPropInfo uomPropInfo = propInfo.PropType.Properties ["Uom"];
                                string uomValueText = AimMetadata.GetEnumValueAsString (editValClass.Uom, uomPropInfo.TypeIndex);
                                cell.Value = editValClass.Value + ", " + uomValueText;
                                cell.Tag = editValClass;
                            }

                            if (propInfo.TypeIndex == (int)DataType.FeatureRef || propInfo.PropType.SubClassType == AimSubClassType.AbstractFeatureRef) {
                                cell.Value = (aimPropValue as FeatureRef).Identifier;
                            }
                        }
                    }
                }

                if (isNewRow)
                    row.Cells.Add (cell);
            }

            if (isNewRow)
                dgv.Rows.Add (row);
        }

        public static void ShowFieldsContextMenu (DataGridView dgv, AimClassInfo classInfo, EventHandler refreshFunction)
        {
            ContextMenuStrip cms = new ContextMenuStrip ();
            cms.Tag = new object [] { dgv, refreshFunction };

            foreach (AimPropInfo propInfo in classInfo.Properties)
            {
                if (propInfo.PropType.AimObjectType == AimObjectType.Field ||
                    
                    propInfo.PropType.SubClassType == AimSubClassType.ValClass ||

                    (propInfo.TypeIndex == (int)DataType.FeatureRef ||
                    propInfo.PropType.SubClassType == AimSubClassType.AbstractFeatureRef))
                {
                    var tsmi = new ToolStripMenuItem();
                    tsmi.Tag = propInfo;
                    tsmi.Text = propInfo.Name;
                    tsmi.CheckOnClick = true;
                    tsmi.Click += AddOrRemoveColumnMenuItem_Click;
                    cms.Items.Add (tsmi);

                    foreach (DataGridViewColumn dgvCol in dgv.Columns)
                    {
                        if (propInfo.Equals (dgvCol.Tag))
                        {
                            tsmi.Checked = true;
                            break;
                        }
                    }
                }
            }

            cms.Show (dgv, 1, 1);
        }

		public static string GetFeatureDescription (Feature feature, out bool hasDesc)
		{
            hasDesc = false;

			AimClassInfo classInfo = UIMetadata.Instance.GetClassInfo ((int) feature.FeatureType);
			UIClassInfo uiClassInfo = classInfo.UiInfo ();

			string s = uiClassInfo.DescriptionFormat;

			if (string.IsNullOrEmpty (s))
				return feature.Identifier.ToString ();

			string resultStr = "";

			int startIndex = -1;

			for (int i = 0; i < s.Length; i++)
			{
				char c = s [i];

				if (startIndex == -1)
				{
					if (c == '<')
					{
						startIndex = i;
					}
					else
					{
						resultStr += c;
					}
				}
				else
				{
					if (c == '>')
					{
						var tmp = s.Substring (startIndex + 1, i - startIndex - 1);
                        var propNamesArr = tmp.Split('|');
                        int propNameIndex = 0;

                        while (propNameIndex < propNamesArr.Length)
                        {
                            var propName = propNamesArr[propNameIndex];
                            propNameIndex++;

                            #region GetValue

                            var propInfo = classInfo.Properties[propName];
                            if (propInfo != null)
                            {
                                IAimProperty aimProp = (feature as IAimObject).GetValue(propInfo.Index);
                                if (aimProp != null)
                                {
                                    if (aimProp.PropertyType == AimPropertyType.AranField)
                                    {
                                        var aimField = aimProp as AimField;
                                        if (aimField.FieldType == AimFieldType.SysEnum)
                                        {
                                            string enumText = AimMetadata.GetEnumValueAsString(
                                                (int)((IEditAimField)aimField).FieldValue, propInfo.TypeIndex);

                                            resultStr += enumText;
                                        }
                                        else
                                        {
                                            var val = ((IEditAimField)aimField).FieldValue;
                                            if (val is double)
                                            {
                                                double d;
                                                if (double.TryParse(val.ToString(), out d))
                                                    val = d.ToString("#.00");
                                            }

                                            resultStr += val.ToString();
                                        }
                                    }
                                    else if (aimProp.PropertyType == AimPropertyType.DataType)
                                    {
                                        resultStr = aimProp.ToString();
                                    }
                                }
                            }

                            #endregion

                        }

						startIndex = -1;
					}
				}
			}

            resultStr = resultStr.Trim(",/-".ToCharArray());

			if (string.IsNullOrWhiteSpace (resultStr))
				return feature.Identifier.ToString ();

			hasDesc = true;
			return resultStr;
		}

        public static string GetFeatureDescription(Feature feature)
        {
			bool hasDesc;
			return GetFeatureDescription (feature, out hasDesc);
        }

        public static void DGV_SortCompare(object sender, DataGridViewSortCompareEventArgs e)
        {
            var dgv = sender as DataGridView;

            var aimPropInfo = e.Column.Tag as AimPropInfo;
            if (aimPropInfo != null && aimPropInfo.PropType.SubClassType == AimSubClassType.ValClass) {
                var cell1 = dgv.Rows[e.RowIndex1].Cells[e.Column.Index];
                var cell2 = dgv.Rows[e.RowIndex2].Cells[e.Column.Index];

                var valClass1 = cell1.Tag as Aran.Aim.DataTypes.IEditValClass;
                var valClass2 = cell2.Tag as Aran.Aim.DataTypes.IEditValClass;

                var val1 = ConverterToSI.Convert(valClass1, valClass1.Value);
                var val2 = ConverterToSI.Convert(valClass2, valClass1.Value);

                e.SortResult = (int)(val1 - val2);
                e.Handled = true;
            }
        }

        
        private static void AddOrRemoveColumnMenuItem_Click (object sender, EventArgs e)
        {
            ToolStripMenuItem tsmi = sender as ToolStripMenuItem;

            bool showGridView = tsmi.Checked;

            AimPropInfo propInfo = tsmi.Tag as AimPropInfo;
            if (propInfo == null)
                return;

            propInfo.UiPropInfo ().ShowGridView = showGridView;

            object [] tagObjArr = tsmi.Owner.Tag as object [];
            if (tagObjArr == null)
                return;

            DataGridView dgv = tagObjArr [0] as DataGridView;
            EventHandler refreshEventHandler = tagObjArr [1] as EventHandler;

            if (showGridView)
            {
                DataGridViewColumn dgCol = ToColumn (propInfo);
                if (dgCol != null)
                {
                    dgv.Columns.Add (dgCol);
                    if (refreshEventHandler != null)
                        refreshEventHandler (dgv, null);
                }
            }
            else
            {
                for (int i = 0; i <dgv.Columns.Count; i++)
                {
                    if (propInfo.Equals (dgv.Columns [i].Tag))
                    {
                        dgv.Columns.RemoveAt (i);
                        break;
                    }
                }
            }
        }

        private static void FillDataGridColumns (AimClassInfo classInfo, DataGridView dgv)
        {
            dgv.Rows.Clear ();
            dgv.Columns.Clear ();

            #region ValidTime Columns
            var col = new DataGridViewTextBoxColumn ();
            col.HeaderText = "Begin of ValidTime";
            dgv.Columns.Add (col);

            col = new DataGridViewTextBoxColumn ();
            col.HeaderText = "End of ValidTime";
            dgv.Columns.Add (col);

            col = new DataGridViewTextBoxColumn ();
            col.HeaderText = "Seq&Corr Number";
            dgv.Columns.Add (col);
            #endregion

            foreach (AimPropInfo propInfo in classInfo.Properties)
            {
                UIPropInfo uiPropInfo = propInfo.UiPropInfo ();
                if (uiPropInfo.ShowGridView)
                {
                    DataGridViewColumn dgvColumn = ToColumn (propInfo);
                    if (dgvColumn != null)
                    {
                        dgv.Columns.Add (dgvColumn);
                    }
                }
            }
        }

        private static DataGridViewColumn ToColumn (AimPropInfo propInfo)
        {
            DataGridViewColumn dgvColumn = null;

            AimClassInfo propTypeClassInfo = propInfo.PropType;

            bool b = AimMetadata.IsEnum (propTypeClassInfo.Index);

            if (propTypeClassInfo.AimObjectType == AimObjectType.Field)
            {
                AimFieldType aimFieldType = (AimFieldType) propTypeClassInfo.Index;

                switch (aimFieldType)
                {
                    case AimFieldType.SysString:
                    case AimFieldType.SysGuid:
                    case AimFieldType.SysDateTime:
                    case AimFieldType.SysInt32:
                    case AimFieldType.SysInt64:
                    case AimFieldType.SysUInt32:
                        {
                            DataGridViewTextBoxColumn tbCol = new DataGridViewTextBoxColumn ();
                            dgvColumn = tbCol;
                        }
                        break;
                    case AimFieldType.SysBool:
                        DataGridViewCheckBoxColumn chbCol = new DataGridViewCheckBoxColumn ();
                        dgvColumn = chbCol;
                        break;
                    case AimFieldType.SysDouble:
                        {
                            DataGridViewTextBoxColumn tbCol = new DataGridViewTextBoxColumn ();
                            tbCol.DefaultCellStyle.Format = "#.####";
                            dgvColumn = tbCol;
                        }
                        break;
                    default:
                        {
                            if (propTypeClassInfo.SubClassType == AimSubClassType.Enum)
                            {
                                DataGridViewTextBoxColumn tbCol = new DataGridViewTextBoxColumn ();
                                dgvColumn = tbCol;
                            }
                        }
                        break;
                }
            }
            else if (propTypeClassInfo.AimObjectType == AimObjectType.DataType)
            {
                if (propTypeClassInfo.SubClassType == AimSubClassType.ValClass)
                {
                    var tbCol = new DataGridViewTextBoxColumn ();
                    dgvColumn = tbCol;
                }
                else if (propInfo.TypeIndex == (int) DataType.FeatureRef ||
                    propTypeClassInfo.SubClassType == AimSubClassType.AbstractFeatureRef) {
                    var tbCol = new DataGridViewTextBoxColumn();
                    dgvColumn = tbCol;
                }
            }

            if (dgvColumn != null)
            {
                dgvColumn.Name = propInfo.Name;
                dgvColumn.Tag = propInfo;
            }

            return dgvColumn;
        }
    }
}
