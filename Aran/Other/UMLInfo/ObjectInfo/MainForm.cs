using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ParseMDL;
using System.Xml;

namespace UMLInfo
{
    public partial class MainForm : Form
    {
        private List<ObjectInfo> _objInfoList;
        private PropForm _propForm;
        private TextForm _textForm;
        private CodeGenerator _genCode;

        public MainForm ()
        {
            InitializeComponent ();

            foreach (ToolStripItem item in showToolStripMenuItem.DropDownItems)
            {
                item.Click += new EventHandler (ShowObjectInfoType_Click);
            }
        }

        private void exitToolStripMenuItem_Click (object sender, EventArgs e)
        {
            this.Close ();
        }

        private void MainForm_Load (object sender, EventArgs e)
        {
            StoreUmlObject suo = new StoreUmlObject ();
            UmlObject uo = suo.LoadObject(@"Files/UmlObject.dat");

            ClassCategory cc = ClassCategory.Parse ((UmlObject) UmlObjectExtension.GetPropertyValue (uo, "root_category"), null);

            #region LoadUml

            List<UmlClass> classList  = new List<UmlClass> ();
            List<Association> assocList = new List<Association> ();
            
            FillClassList (cc, classList, assocList);

            foreach (Association assoc in assocList)
            {
                if (assoc.AssociationClass != null)
                {
                    foreach (var item in classList)
                    {
                        if (item.Namespace + "." + item.Name == assoc.AssociationClass)
                        {
                            assoc.AssociationClass = item.Id;
                            break;
                        }
                    }
                }
            }

            #endregion

            ObjectInfoParser parser = new ObjectInfoParser (classList, assocList);

            _objInfoList = parser.GetObjectInfoList ();

            FillClassesDataGrid ();
            ShowRowCount ();

            _propForm = new PropForm (_objInfoList, this);
            _textForm = new TextForm (this);
            _genCode = new CodeGenerator (_objInfoList);

            if (parser.ReplacedPropertyNameList.Count > 0)
            {
                string s = "Replaced property names:\r\n\r\n";
                foreach (string rpn in parser.ReplacedPropertyNameList)
                {
                    s += rpn + "\r\n";
                }
                
                //MessageBox.Show (s);
            }
        }

        private void FillClassList (ClassCategory cc, List<UmlClass> classList, List<Association> assocList)
        {
            foreach (var item in cc.LogicalModels)
            {
                if (item is ClassCategory)
                {
                    FillClassList (item as ClassCategory, classList, assocList);
                }
                else if (item is UmlClass)
                    classList.Add (item as UmlClass);
                else if (item is Association)
                    assocList.Add (item as Association);
            }
        }

        private void FillClassesDataGrid ()
        {
            objInfoDGV.Columns.Clear ();
            objInfoDGV.Rows.Clear ();

            DataGridViewColumn col = new DataGridViewTextBoxColumn ();
            col.HeaderText = "Id";
            col.Name = "Id";
            col.Visible = false;
            objInfoDGV.Columns.Add (col);

            col = new DataGridViewTextBoxColumn ();
            col.HeaderText = "Name";
            col.Name = "Name";
            col.Width = 150;
            objInfoDGV.Columns.Add (col);

            col = new DataGridViewTextBoxColumn ();
            col.HeaderText = "Namespace";
            col.Name = "Namespace";
            objInfoDGV.Columns.Add (col);

            col = new DataGridViewTextBoxColumn ();
            col.HeaderText = "StereoType";
            col.Name = "StereoType";
            objInfoDGV.Columns.Add (col);

            col = new DataGridViewCheckBoxColumn ();
            col.SortMode = DataGridViewColumnSortMode.Automatic;
            col.HeaderText = "Is Choice";
            col.Name = "IsChoice";
            objInfoDGV.Columns.Add (col);

            col = new DataGridViewCheckBoxColumn ();
            col.SortMode = DataGridViewColumnSortMode.Automatic;
            col.HeaderText = "Is Abstract";
            col.Name = "IsAbstract";
            objInfoDGV.Columns.Add (col);

            col = new DataGridViewLinkColumn ();
            col.HeaderText = "SuperClassId";
            col.Name = "SuperClassId";
            col.SortMode = DataGridViewColumnSortMode.Automatic;
            objInfoDGV.Columns.Add (col);

            col = new DataGridViewCheckBoxColumn ();
            col.SortMode = DataGridViewColumnSortMode.Automatic;
            col.HeaderText = "Is Used";
            col.Name = "IsUsed";
            objInfoDGV.Columns.Add (col);

            col = new DataGridViewTextBoxColumn ();
            col.HeaderText = "Association Between";
            col.Name = "AssocBetween";
            objInfoDGV.Columns.Add (col);

            col = new DataGridViewLinkColumn ();
            col.HeaderText = "Replace With";
            col.Name = "ReplaceWith";
            col.SortMode = DataGridViewColumnSortMode.Automatic;
            objInfoDGV.Columns.Add (col);
            
            foreach (ObjectInfo item in _objInfoList)
            {
                int index = objInfoDGV.Rows.Add (
                    item.Id,
                    item.Name,
                    item.Namespace,
                    item.Type,
                    item.IsChoice,
                    item.IsAbstract,
                    (item.Base != null ? item.Base.Name : ""),
                    item.IsUsed,
                    item.AssocBetween,
                    item.ReplaceWith);
                objInfoDGV.Rows [index].Tag = item;
            }

            //objInfoDGV.Sort (objInfoDGV.Columns [1], ListSortDirection.Ascending);
        }

        private void analyseTSB_Click (object sender, EventArgs e)
        {
            string s = "";

            foreach (ObjectInfo objInfo in _objInfoList)
            {
                string s2 = "";

                List<ObjectInfo> eoi = new List<ObjectInfo> ();

                foreach (PropInfo propInfo in objInfo.Properties)
                {
                    if (propInfo.PropType != null &&
                        propInfo.PropType.Type == ObjectInfoType.Feature) 
                    {
                        if (eoi.Where (oi => oi.Equals (propInfo.PropType)).Count () > 0)
                        {
                            s2 += propInfo.Name;
                        }
                        else
                        {
                            eoi.Add (propInfo.PropType);
                        }
                    }
                }

                if (s2.Length > 0)
                {
                    s += objInfo.Name + "\r\n" + s2 + "\r\n";
                }
            }
        }

        private void searchTB_Leave (object sender, EventArgs e)
        {
            searchPanel.Visible = false;
        }

        private void searchToolStripMenuItem_Click (object sender, EventArgs e)
        {
            searchPanel.Visible = true;
            searchTB.Text = "";
            searchTB.Select ();
        }

        private void searchTB_TextChanged (object sender, EventArgs e)
        {
            string s = searchTB.Text.ToLower ();
            if (string.IsNullOrWhiteSpace (s))
                return;

            foreach (DataGridViewRow row in objInfoDGV.Rows)
            {
                if (row.Cells [1].Value.ToString ().ToLower ().StartsWith (s))
                {
                    objInfoDGV.CurrentCell = row.Cells [1];
                    break;
                }
            }
        }

        private void searchTB_KeyUp (object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                searchPanel.Visible = false;
            }
        }

        private void ShowObjectInfoType_Click (object sender, EventArgs e)
        {
            List <ObjectInfoType> typeList = new List<ObjectInfoType> ();

            foreach (ToolStripItem item in showToolStripMenuItem.DropDownItems)
            {
                if (item.Tag == null || !(item as ToolStripMenuItem).Checked)
                    continue;

                typeList.Add ((ObjectInfoType) Enum.Parse (typeof (ObjectInfoType),
                    item.Tag.ToString (), true));
            }

            foreach (DataGridViewRow row in objInfoDGV.Rows)
            {
                row.Visible = typeList.Contains (((ObjectInfo) row.Tag).Type);
            }

            ShowRowCount ();
        }

        private void ShowRowCount ()
        {
            int count = 0;
            foreach (DataGridViewRow row in objInfoDGV.Rows)
            {
                if (row.Visible)
                    count++;
            }

            toolStripStatusLabel1.Text = "Row count: " + count;
        }

        private void checkAllToolStripMenuItem_Click (object sender, EventArgs e)
        {
            foreach (ToolStripItem item in showToolStripMenuItem.DropDownItems)
            {
                if (item.Tag != null)
                {
                    (item as ToolStripMenuItem).Checked = true;
                }
            }
        }

        private void uncheckAllToolStripMenuItem_Click (object sender, EventArgs e)
        {
            foreach (ToolStripItem item in showToolStripMenuItem.DropDownItems)
            {
                if (item.Tag != null)
                {
                    (item as ToolStripMenuItem).Checked = false;
                }
            }
        }

        private void showPropTSB_Click (object sender, EventArgs e)
        {
            if (objInfoDGV.CurrentRow == null)
                return;

            ObjectInfo objInfo = objInfoDGV.CurrentRow.Tag as ObjectInfo;
            _propForm.ShowForm (objInfo);
        }

        private void MainForm_FormClosing (object sender, FormClosingEventArgs e)
        {
            _propForm.Dispose ();
            _textForm.Dispose ();
            e.Cancel = false;
        }

        private void objInfoDGV_CurrentCellChanged (object sender, EventArgs e)
        {
            if (objInfoDGV.CurrentRow == null)
            {
                docTextBox.Text = "";
                return;
            }

            ObjectInfo oi = (objInfoDGV.CurrentRow.Tag as ObjectInfo);
            if (oi == null)
            {
                docTextBox.Text = "";
                return;
            }
            docTextBox.Text = oi.Documentation;

            int visibleRowIndex = 1;
            for (int i = 0; i < objInfoDGV.Rows.Count; i++)
            {
                if (objInfoDGV.CurrentCell.RowIndex == i)
                    break;
                if (objInfoDGV.Rows [i].Visible)
                    visibleRowIndex++;
            }

            curRowStatusLabel.Text = "Cur Row Index: " + (visibleRowIndex);
        }

        private void genEnumTSB_Click (object sender, EventArgs e)
        {
            _genCode.GenerateEnums ();
        }

        private void objInfoDGV_CellContentClick (object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1)
                return;
            DataGridView dgv = (DataGridView) sender;
            ObjectInfo objInfo = dgv.Rows [e.RowIndex].Tag as ObjectInfo;
            if (objInfo == null)
                return;

            ObjectInfo lookingObjInfo = null;

            if (dgv.Columns [e.ColumnIndex].Name == "SuperClassId")
            {
                lookingObjInfo = objInfo.Base;
            }
            else if (dgv.Columns [e.ColumnIndex].Name == "ReplaceWith")
            {
                lookingObjInfo = objInfo.ReplaceWith;
            }

            if (lookingObjInfo == null)
                return;

            foreach (DataGridViewRow row in dgv.Rows)
            {
                if (row.Tag.Equals (lookingObjInfo))
                {
                    dgv.CurrentCell = row.Cells [1];
                    return;
                }
            }
        }

        private void createClassTSB_Click (object sender, EventArgs e)
        {
            //ObjectInfo objInfo = null;
            //if (objInfoDGV.CurrentRow == null ||
            //    (objInfo = objInfoDGV.CurrentRow.Tag as ObjectInfo) == null)
            //    return;

            //List<string> codeLines = _genCode.CreateClass (objInfo);
            //_textForm.ShowForm (codeLines.ToArray ());

            _genCode.Errors.Clear ();

            _genCode.CodeFolder = @"..\..\Results\Model_TSB";
            _genCode.CreateClasses ();
            _textForm.ShowForm (_genCode.Errors.ToArray ());
        }

        private void createEnumAranObTSB_Click (object sender, EventArgs e)
        {
            List<string> lines = _genCode.CreateEnum_AllObjectType ();
            _textForm.ShowForm (lines.ToArray ());
        }

        private void genAllTSB_Click (object sender, EventArgs e)
        {
            _genCode.Errors.Clear ();
            
            _genCode.CodeFolder = @"..\..\Results\Model";
            _genCode.GenerateDataTypes = true;

            _genCode.CreateClasses ();
            _genCode.GenerateEnums ();
            _genCode.CreateEnum_AllObjectType ();
            _genCode.CreateFactoryFunc ();
            _genCode.CreateExtensionClass ();

            string message = "";

            if (_genCode.Errors.Count == 0)
            {
                message = "Successfully completed.";
            }
            else
            {
                message = "Completed.\r\n" + _genCode.Errors .Count + " errors:\r\n";
                foreach (string err in _genCode.Errors)
                    message += err + "\r\n";
            }

            MessageBox.Show (message);
        }

        private void createTable_DataTypeTSB_Click (object sender, EventArgs e)
        {
            ObjectInfo objInfo = null;
            if (objInfoDGV.CurrentRow == null ||
                (objInfo = objInfoDGV.CurrentRow.Tag as ObjectInfo) == null)
                return;

            List<string> lines
                //= _genCode.CreateTable_FeatureOrObject ();
                = _genCode.Create_RelationTables ();

            _textForm.ShowForm (lines.ToArray ());
        }

        private void getChoiceNamesToolStripMenuItem_Click (object sender, EventArgs e)
        {
            List<string> list = new List<string> ();

            foreach (ObjectInfo objInfo in _objInfoList)
            {
                if (objInfo.IsChoice)
                {
                    list.Add (objInfo.Name);
                }
            }

            string s = "";
            foreach (string item in list)
                s += "case AllAimObjectType." + item + ":\r\n";
        }

		private void CreateDocumentInfoFile_Click(object sender, EventArgs e)
		{
			var sfd = new SaveFileDialog ();
			if (sfd.ShowDialog () != DialogResult.OK)
				return;

			var xmlDoc = new XmlDocument ();
			xmlDoc.LoadXml ("<AIXMDescription Version=\"11\"></AIXMDescription>");

			foreach (var objInfo in _objInfoList)
			{
				
				if (objInfo.Type == ObjectInfoType.Feature ||
					objInfo.Type == ObjectInfoType.Object ||
					objInfo.Type == ObjectInfoType.Codelist)
				{
					var elemName = objInfo.Type == ObjectInfoType.Feature ? "F" : 
						(objInfo.Type == ObjectInfoType.Object ? "O" : "E");
					var elem = xmlDoc.CreateElement (elemName);

					elem.SetAttribute ("Name", objInfo.Name.Trim());
					elem.SetAttribute ("Doc", (objInfo.Documentation != null ? objInfo.Documentation.Trim () : string.Empty));

					xmlDoc.DocumentElement.AppendChild (elem);

					SavePropDocumentationInfo (objInfo.Properties, elem, objInfo.Type != ObjectInfoType.Codelist);
				}
			}

			xmlDoc.Save (sfd.FileName);

			MessageBox.Show ("Done!");
		}

		private void SavePropDocumentationInfo(List<PropInfo> list, XmlElement elem, bool isNotCodelist)
		{
			var doc = elem.OwnerDocument;

			foreach (var pi in list)
			{
				var propElem = doc.CreateElement ("P");
				propElem.SetAttribute ("Name", pi.Name.Trim());
				propElem.SetAttribute ("Doc", (pi.Documentation != null ? pi.Documentation.Trim() : string.Empty));
				elem.AppendChild (propElem);

                if (isNotCodelist && pi.Restriction != null) {
                    var restElem = doc.CreateElement("Restriction");

                    CreateResctictionElement(restElem, pi.Restriction);
                    propElem.AppendChild(restElem);
                }
			}
		}

		private void CreateResctictionElement(XmlElement elem, ValueRestriction vr)
		{
            if (vr.Patter != null) {

            }

            elem.SetAttribute("Max", double.IsNaN(vr.Max) ? "" : vr.Max.ToString());
            elem.SetAttribute("Min", double.IsNaN(vr.Min) ? "" : vr.Min.ToString());

            var patternElem = elem.OwnerDocument.CreateElement("Pattern");
            patternElem.InnerText = (vr.Patter ?? "");
            elem.AppendChild(patternElem);
		}

        private void test1ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //foreach (var oi in _objInfoList)
            //{
            //    oi.Properties.Where(pi =>  pi.PropType)
            //}

        }
    }
}
 