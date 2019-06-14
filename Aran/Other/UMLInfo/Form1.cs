using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ParseMDL;

namespace UMLInfo
{
    
    public partial class Form1 : Form
    {
        public bool filter;
        public Form1()
        {
            
            InitializeComponent();
        }

        public void assocView(List<Association> _assocList, UmlClass _umlClass, List<UmlClass> _classList)
        {
            foreach (Association assoc in _assocList)
            {
                filter = (_umlClass.Id == assoc.Role1.SupplierId) || (_umlClass.Id == assoc.Role2.SupplierId);
                if (filter)
                {
                    UmlClass suppClass = _classList.GetClassById (assoc.Role1.SupplierId);
                string role1SuppName = (suppClass != null ? suppClass.Name : "");
                suppClass = _classList.GetClassById (assoc.Role2.SupplierId);
                string role2SuppName = (suppClass != null ? suppClass.Name : "");

                int index = assocDGV.Rows.Add("", assoc.Role1.Label, role1SuppName, assoc.Role1.ClientCardinality, assoc.Role1.Containment,
                "", assoc.Role2.Label, role2SuppName, assoc.Role2.ClientCardinality, assoc.Role2.Containment);

                assocDGV.Rows[index].Tag = assoc;
                }
                rowCountlabel.Text = "Row Count: " + assocDGV.Rows.Count;
            }
        }
    }
}
