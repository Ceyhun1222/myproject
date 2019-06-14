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
    public partial class PropertiesForm : Form
    {
        List<UmlClass> _clList;
        int sortBtnCount;
        public PropertiesForm()
        {
            InitializeComponent();
            unvisibleList = new List<int>();
        }

        public void PropertyGridFill()
        {
            DataGridViewTextBoxColumn col = new DataGridViewTextBoxColumn();
            
            col = new DataGridViewTextBoxColumn();
            col.Width = 255;
            col.HeaderText = "Name";
            col.Name = "Name";
            propDGV.Columns.Add(col);

            //col = new DataGridViewTextBoxColumn();
            //col.HeaderText = "Type";
            //col.Name = "Type";
            //propDGV.Columns.Add(col);

            col = new DataGridViewTextBoxColumn();
            col.Width = 255;
            col.HeaderText = "Type Name";
            col.Name = "TypeName";
            propDGV.Columns.Add(col);

            col = new DataGridViewTextBoxColumn();
            col.Width = 255;
            col.HeaderText = "Base Type";
            col.Name = "BaseType";
            propDGV.Columns.Add(col);

            col = new DataGridViewTextBoxColumn();
            col.Width = 255;
            col.HeaderText = "Simple Type";
            col.Name = "SimpleType";
            propDGV.Columns.Add(col);

            col = new DataGridViewTextBoxColumn();
            col.Width = 255;
            col.HeaderText = "Documentation";
            col.Name = "Documentation";
            propDGV.Columns.Add(col);


        }

        public void propertyView(UmlClass propUml, List<Association> _assocList, List<UmlClass> _classList)
        {
            _clList = _classList;
            foreach (Association assoc in _assocList)
            {
                UmlClass suppClass = _classList.GetClassById(assoc.Role2.SupplierId);
                UmlClass suppClass1 = _classList.GetClassById(assoc.Role1.SupplierId);
                if (assoc.Role1.Label != null && suppClass.Name == propUml.Name)
                {
                    string documentation = null;
                    if (assoc.Role1.Documentation != null)
                        documentation = assoc.Role1.Documentation;
                    else if (assoc.Role2.Documentation != null)
                        documentation = assoc.Role2.Documentation;
                    else { documentation = assoc.Documentation; }
                    int index = propDGV.Rows.Add(assoc.Role1.Label, suppClass1.Name, suppClass1.StereoType, "", documentation);
                    propDGV.Rows[index].Tag = assoc;
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
                UmlClass suppClassNext = _classList.GetClassById(atr.TypeId);
                UmlClass suppClassPrev = new UmlClass();
                do
                {
                    switch (suppClassNext.StereoType)
                    {
                        case "datatype":
                            suppClassPrev = suppClassNext;
                            suppClassNext = _classList.GetClassById(suppClassNext.SuperClassId);
                            contDive = true;
                            break;
                        case "codelist":
                            typeName = _classList.GetClassById(suppClassNext.SuperClassId).Name;
                            attributeTypeName = attributeTypeName.Remove(attributeTypeName.Length - 4, 4);
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



                int index = propDGV.Rows.Add(name, attributeTypeName, stereoType, typeName, documentation);
                propDGV.Rows[index].Tag = atr;


            }
        }


        private void propDGV_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            AttributeProperties ap = new AttributeProperties();
            if (propDGV.Rows[e.RowIndex].Tag is ClassAttribute)
            {
                ClassAttribute ca = propDGV.Rows[e.RowIndex].Tag as ClassAttribute;
                fillClassAttribute(ca, ap);
               
            }
            if (ap.minLength > 0)
                minLength.Text = Convert.ToString(ap.minLength);
            else minLength.Text = "";
            if (ap.pattern != null)
                pattern.Text = ap.pattern;
            else pattern.Text = "";
            nilReason.Text = Convert.ToString(ap.nilReason);
            if (ap.maxLength > 0)
                maxLength.Text = Convert.ToString(ap.maxLength);
            else maxLength.Text = "";
        }

        private void sort_Click(object sender, EventArgs e)
        {
            int i;
           
            if (sortBtnCount == 0)
            {
                for (i = 0; i < propDGV.Rows.Count - 1; i++)
                {
                    ClassAttribute ca = propDGV.Rows[i].Tag as ClassAttribute;
                    AttributeProperties ap = new AttributeProperties();
                    if (ca != null)
                        fillClassAttribute(ca, ap);

                    if ((ap.pattern == null) && (ap.minLength == 0) && (ap.maxLength == 0))
                    {

                        
                        propDGV.Rows[i].Visible = false;
                        unvisibleList.Add(i);
                        sortBtnCount = 1;

                    }
                }
            }
            else if (sortBtnCount == 1)
            {
                foreach (var item in unvisibleList)
                {
                    propDGV.Rows[item].Visible = true;
                    sortBtnCount = 0;
                }
                unvisibleList.Clear();
            }

        }

        private List<int> unvisibleList;

        private void fillClassAttribute(ClassAttribute ca, AttributeProperties ap)
        {

            UmlClass suppClassNext = _clList.GetClassById(ca.TypeId);
                UmlClass suppClassPrev = new UmlClass(); 
           do
                {

                    foreach (ClassAttribute cla in suppClassNext.Attributes)
                    {
                        switch (cla.Name)
                        {
                            case "nilReason":
                                {
                                    ap.nilReason = true;
                                    break;
                                }

                            case "minLength":
                                {
                                    ap.minLength = Convert.ToInt32(cla.InitialValue);
                                    break;
                                }

                            case "maxLength":
                                {
                                    ap.maxLength = Convert.ToInt32(cla.InitialValue);
                                    break;
                                }

                            case "pattern":
                                {
                                    ap.pattern = cla.InitialValue;
                                    break;
                                }
                        }
                    }
                    suppClassPrev = suppClassNext;
                    suppClassNext = _clList.GetClassById(suppClassNext.SuperClassId);
                }
                while (suppClassNext.StereoType == "datatype");
        }

        private void methodShow_Click(object sender, EventArgs e)
        {
            MethodText mt = new MethodText();

            foreach (var item in unvisibleList)
            {
                ClassAttribute ca = propDGV.Rows[item].Tag as ClassAttribute;
            }
            mt.Show();
        }
       
    }
        
    }

    public class AttributeProperties
    {

        bool nil;
        int  minl;
        int  maxl;
        string pat = null;
        public bool nilReason {  get { return nil; }  set {nil = value;} }
        public int minLength { get { return minl; } set { minl = value; } }
        public int maxLength {  get { return maxl; } set {maxl = value;} }
        public string pattern { get { return pat ; } set { pat = value; } }

     }

