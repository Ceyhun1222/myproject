using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ParseMDL;
using System.Collections;

namespace UMLInfo
{
    public partial class NewPropertiesForm : Form
    {
        private List<UmlClass> _clList;
        private int _sortBtnCount;

        public NewPropertiesForm ()
        {
            InitializeComponent ();
            unvisibleList = new List<int> ();
        }

        public void PropertyGridFill ()
        {
            DataGridViewTextBoxColumn col = new DataGridViewTextBoxColumn ();

            col = new DataGridViewTextBoxColumn ();
            col.Width = 255;
            col.HeaderText = "Name";
            col.Name = "Name";
            propDGV.Columns.Add (col);

            //col = new DataGridViewTextBoxColumn();
            //col.HeaderText = "Type";
            //col.Name = "Type";
            //propDGV.Columns.Add(col);

            col = new DataGridViewTextBoxColumn ();
            col.Width = 255;
            col.HeaderText = "Type Name";
            col.Name = "TypeName";
            propDGV.Columns.Add (col);

            col = new DataGridViewTextBoxColumn ();
            col.Width = 255;
            col.HeaderText = "Base Type";
            col.Name = "BaseType";
            propDGV.Columns.Add (col);

            col = new DataGridViewTextBoxColumn ();
            col.Width = 255;
            col.HeaderText = "Simple Type";
            col.Name = "SimpleType";
            propDGV.Columns.Add (col);

            col = new DataGridViewTextBoxColumn ();
            col.Width = 255;
            col.HeaderText = "Documentation";
            col.Name = "Documentation";
            propDGV.Columns.Add (col);


        }

        public void propertyView (UmlClass propUml, List<Association> _assocList, List<UmlClass> _classList)
        {
            _clList = _classList;
            foreach (Association assoc in _assocList)
            {
                UmlClass suppClass = _classList.GetClassById (assoc.Role2.SupplierId);
                UmlClass suppClass1 = _classList.GetClassById (assoc.Role1.SupplierId);
                if (assoc.Role1.Label != null && suppClass.Name == propUml.Name)
                {
                    string documentation = null;
                    if (assoc.Role1.Documentation != null)
                        documentation = assoc.Role1.Documentation;
                    else if (assoc.Role2.Documentation != null)
                        documentation = assoc.Role2.Documentation;
                    else { documentation = assoc.Documentation; }
                    int index = propDGV.Rows.Add (assoc.Role1.Label, suppClass1.Name, suppClass1.StereoType, "", documentation);
                    propDGV.Rows [index].Tag = assoc;
                }
            }

            foreach (ClassAttribute atr in propUml.Attributes)
            {
                bool contDive = false;
                string attributeTypeName = atr.TypeName;
                string stereoType = null;
                string typeName = null;
                string name = atr.Name;
                string documentation = atr.Documentation;
                UmlClass suppClassNext = _classList.GetClassById (atr.TypeId);
                UmlClass suppClassPrev = new UmlClass ();
                
                do
                {
                    switch (suppClassNext.StereoType)
                    {
                        case "datatype":
                            suppClassPrev = suppClassNext;
                            suppClassNext = _classList.GetClassById (suppClassNext.SuperClassId);
                            contDive = true;
                            break;
                        case "codelist":
                            typeName = _classList.GetClassById (suppClassNext.SuperClassId).Name;
                            attributeTypeName = attributeTypeName.Remove (attributeTypeName.Length - 4, 4);
                            stereoType = suppClassPrev.StereoType;
                            typeName = "bool";
                            contDive = false;
                            break;
                        case "XSDsimpleType":
                            stereoType = suppClassPrev.StereoType;
                            typeName = suppClassNext.Name;
                            contDive = false;
                            break;

                    }

                }
                while (contDive);

                int index = propDGV.Rows.Add (name, attributeTypeName, stereoType, typeName, documentation);
                propDGV.Rows [index].Tag = atr;
            }
        }

        private void propDGV_CellMouseClick (object sender, DataGridViewCellMouseEventArgs e)
        {
            NewAttributeProperties ap = new NewAttributeProperties ();
            if (propDGV.Rows [e.RowIndex].Tag is ClassAttribute)
            {
                ClassAttribute ca = propDGV.Rows [e.RowIndex].Tag as ClassAttribute;
                fillClassAttribute (ca, ap);

            }
            if (ap.MinLength > 0)
                minLength.Text = Convert.ToString (ap.MinLength);
            else
                minLength.Text = "";
            if (ap.Pattern != null)
                pattern.Text = ap.Pattern;
            else
                pattern.Text = "";
            nilReason.Text = Convert.ToString (ap.NilReason);
            if (ap.MaxLength > 0)
                maxLength.Text = Convert.ToString (ap.MaxLength);
            else
                maxLength.Text = "";
        }

        private void sort_Click (object sender, EventArgs e)
        {
            if (_sortBtnCount == 0)
            {
                _sortBtnCount = 1;

                for (int i = 0; i < propDGV.Rows.Count - 1; i++)
                {
                    ClassAttribute ca = propDGV.Rows [i].Tag as ClassAttribute;
                    NewAttributeProperties ap = new NewAttributeProperties ();
                    if (ca != null)
                        fillClassAttribute (ca, ap);

                    if ((ap.Pattern == null) && (ap.MinLength == 0) && (ap.MaxLength == 0))
                    {
                        propDGV.Rows [i].Visible = false;
                        unvisibleList.Add (i);
                    }
                }
            }
            else if (_sortBtnCount == 1)
            {
                _sortBtnCount = 0;

                foreach (var item in unvisibleList)
                {
                    propDGV.Rows [item].Visible = true;
                }
                unvisibleList.Clear ();
            }

        }

        private List<int> unvisibleList;

        private void fillClassAttribute (ClassAttribute ca, NewAttributeProperties ap)
        {

            UmlClass suppClassNext = _clList.GetClassById (ca.TypeId);
            UmlClass suppClassPrev = new UmlClass ();
            do
            {

                foreach (ClassAttribute cla in suppClassNext.Attributes)
                {
                    switch (cla.Name)
                    {
                        case "nilReason":
                            {
                                ap.NilReason = true;
                                break;
                            }

                        case "minLength":
                            {
                                ap.MinLength = Convert.ToInt32 (cla.InitialValue);
                                break;
                            }

                        case "maxLength":
                            {
                                ap.MaxLength = Convert.ToInt32 (cla.InitialValue);
                                break;
                            }

                        case "pattern":
                            {
                                ap.Pattern = cla.InitialValue;
                                break;
                            }
                    }
                }
                suppClassPrev = suppClassNext;
                suppClassNext = _clList.GetClassById (suppClassNext.SuperClassId);
            }
            while (suppClassNext.StereoType == "datatype");
        }

        private void methodShow_Click (object sender, EventArgs e)
        {
            MethodText mt = new MethodText ();

            foreach (var item in unvisibleList)
            {
                ClassAttribute ca = propDGV.Rows [item].Tag as ClassAttribute;
            }
            mt.Show ();
        }
    }

    public class NewAttributeProperties
    {
        private bool _nilReason;
        private int _minLength;
        private int _maxLength;
        private string _pattern;
        
        public bool NilReason
        {
            get { return _nilReason; }
            set { _nilReason = value; }
        }
        public int MinLength
        {
            get { return _minLength; }
            set { _minLength = value; }
        }
        public int MaxLength
        {
            get { return _maxLength; }
            set { _maxLength = value; }
        }
        public string Pattern
        {
            get { return _pattern; }
            set { _pattern = value; }
        }

    }
}

