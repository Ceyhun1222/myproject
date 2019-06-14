using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
using UMLInfo;

namespace ParseMDL
{
    public partial class ConvertForm : Form
    {
        private List<UmlClass> _classList;
        private List<Association> _assocList;

        public ConvertForm ()
        {
            InitializeComponent ();
        }

        private void fillClassesButton_Click (object sender, EventArgs e)
        {
            StoreUmlObject suo = new StoreUmlObject ();
            UmlObject uo = suo.LoadObject(@"Files/UmlObject.dat");

            ClassCategory cc = ClassCategory.Parse ((UmlObject) UmlObjectExtension.GetPropertyValue (uo, "root_category"), null);

            _classList  = new List<UmlClass> ();
            _assocList = new List<Association> ();
            
            FillClassList (cc);

            FillClassesDataGrid ();

            GroupByStereoCount (_classList);


        }

        private void GroupByStereoCount (List<UmlClass> classList)
        {
            Hashtable hashtable = new Hashtable ();

            foreach (UmlClass item in classList)
            {
                if (string.IsNullOrWhiteSpace (item.StereoType))
                    continue;

                if (hashtable.ContainsKey (item.StereoType))
                {
                    hashtable [item.StereoType] = (int) hashtable [item.StereoType] + 1;
                }
                else
                {
                    hashtable.Add (item.StereoType, 0);
                }
            }

            string infoText = "";
            foreach (string key in hashtable.Keys)
            {
                comboBox1.Items.Add(key);
                infoText += key + ":    " + hashtable [key] + "\r\n";
            }
            stereoTypeLabel.Text = infoText;
        }

        private void ShowStatusInfo (string message)
        {
            infoStatusLabel.Text = message;
        }

        private void FillClassesDataGrid ()
        {
            DataGridViewColumn col = new DataGridViewTextBoxColumn ();
            col.HeaderText = "Id";
            col.Name = "Id";
            classesDGV.Columns.Add (col);

            col = new DataGridViewTextBoxColumn ();
            col.HeaderText = "Name";
            col.Name = "Name";
            classesDGV.Columns.Add (col);

            col = new DataGridViewTextBoxColumn ();
            col.HeaderText = "Namespace";
            col.Name = "Namespace";
            classesDGV.Columns.Add (col);

            col = new DataGridViewTextBoxColumn ();
            col.HeaderText = "StereoType";
            col.Name = "StereoType";
            classesDGV.Columns.Add (col);

            col = new DataGridViewCheckBoxColumn ();
            col.SortMode = DataGridViewColumnSortMode.Automatic;
            col.HeaderText = "Is Abstract";
            col.Name = "IsAbstract";
            col.ContextMenuStrip = contextMenuStrip1;
            classesDGV.Columns.Add (col);

            col = new DataGridViewTextBoxColumn ();
            col.HeaderText = "Documentation";
            col.Name = "Documentation";
            classesDGV.Columns.Add (col);

            col = new DataGridViewTextBoxColumn ();
            col.HeaderText = "SuperClassId";
            col.Name = "SuperClassId";
            col.ContextMenuStrip = contextMenuStrip1;
            classesDGV.Columns.Add (col);

            foreach (UmlClass item in _classList)
            {
                int index = classesDGV.Rows.Add (item.Id, item.Name, item.Namespace, item.StereoType,
                    item.IsAbstract, item.Documentation, item.SuperClassId);
                classesDGV.Rows [index].Tag = item;
            }
        }


        private void FillClassList (ClassCategory cc)
        {
            foreach (var item in cc.LogicalModels)
            {
                if (item is ClassCategory)
                {
                    FillClassList (item as ClassCategory);
                }
                else if (item is UmlClass)
                    _classList.Add (item as UmlClass);
                else if (item is Association)
                    _assocList.Add (item as Association);
            }
        }

        private void goToToolStripMenuItem_Click (object sender, EventArgs e)
        {
            if (classesDGV.CurrentRow == null)
                return;
            string baseGuid = (classesDGV.CurrentRow.Tag as UmlClass).SuperClassId;

            for (int i = 0; i < classesDGV.Rows.Count; i++)
            {
                var idCell = classesDGV.Rows [i].Cells ["Id"];
                if (baseGuid.Equals (idCell.Value))
                {
                    classesDGV.CurrentCell = idCell;
                    break;
                }
            }
        }


        private void assocShowBtn_Click(object sender, EventArgs e)
        {
            Form1 assocForm = new Form1();
            assocForm.Show();
            if (classesDGV.CurrentRow == null)
                return;
            
            UmlClass umlClass = classesDGV.CurrentRow.Tag as UmlClass;
            assocForm.assocView(_assocList, umlClass, _classList);
        }

        private void propShowBtn_Click(object sender, EventArgs e)
        {
            
            if (classesDGV.CurrentRow != null)
            {
                PropertiesForm propForm = new PropertiesForm();
                propForm.PropertyGridFill();
                propForm.Show();

                
                UmlClass propUml = classesDGV.CurrentRow.Tag as UmlClass;
                propForm.propertyView(propUml, _assocList, _classList);
            }
            else { MessageBox.Show("Please first select the UmlClass"); }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        { 
            //UmlClass uml = classesDGV.CurrentRow.Tag as UmlClass;
                foreach (DataGridViewRow row in classesDGV.Rows)
                {
                    UmlClass uml = row.Tag as UmlClass;
                    row.Visible = (uml.StereoType == Convert.ToString(comboBox1.SelectedItem));
                        
                }
        }
    }

    public static class UmlObjectExtension
    {
        public static IUmlPropertyValue GetPropertyValue (this UmlObject uo, string propName)
        {
            foreach (UmlProperty item in uo.Propreties)
            {
                if (item.Name.Trim () == propName)
                {
                    return item.Value;
                }
            }
            return null;
        }

        public static string GetStringProperty (this UmlObject uo, string propName)
        {
            IUmlPropertyValue upv = GetPropertyValue (uo, propName);
            if (upv == null)
                return null;

            return RemoveStringQuote  (((UmlStringItem) upv).Value);
        }

        public static UmlList GetListProperty (this UmlObject uo, string propName)
        {
            return (UmlList) GetPropertyValue (uo, propName);
        }

        public static UmlObject GetObjectProperty (this UmlObject uo, string propName)
        {
            return (UmlObject) GetPropertyValue (uo, propName);
        }

        public static string RemoveStringQuote (string value)
        {
            if (value != null && value.Length > 2 && value [0] == '"' && value [value.Length - 1] == '"')
                return value.Substring (1, value.Length - 2);
            return value;
        }
    }

    public abstract class ModelObject
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Documentation { get; set; }

        public virtual void ParseObject (UmlObject umlObject)
        {
            Name = umlObject.Name;
            Id = umlObject.GetStringProperty ("quid");
            Documentation = umlObject.GetStringProperty ("documentation");
        }
    }

    public class ClassCategory : ModelObject
    {
        private ClassCategory _parent;

        public ClassCategory ()
        {
            LogicalModels = new List<object> ();
        }

        public List<object> LogicalModels { get; private set; }
        public ClassCategory Parent 
        { 
            get { return _parent; }
            set
            {
                _parent = value;
                if (_parent != null)
                {
                    Name = _parent.Name + "." + Name;
                }
            }
        }

        public static ClassCategory Parse (UmlObject umlObject, ClassCategory parent)
        {
            ClassCategory cc = new ClassCategory ();
            cc.ParseObject (umlObject);
            cc.Parent = parent;

            UmlList logicalModels = umlObject.GetListProperty ("logical_models");
            if (logicalModels != null)
            {
                foreach (IUmlListItem item in logicalModels)
                {
                    if (item.ItemType == ListItemType.UmlObject)
                    {
                        UmlObject childObject = (UmlObject) item;
                        if (childObject.Type == "Class_Category")
                        {
                            ClassCategory childCC = ClassCategory.Parse (childObject, cc);
                            cc.LogicalModels.Add (childCC);
                        }
                        else if (childObject.Type == "Class")
                        {
                            UmlClass childClass = UmlClass.Parse (childObject);
                            childClass.Namespace = cc.Name;
                            cc.LogicalModels.Add (childClass);
                        }
                        else if (childObject.Type == "Association")
                        {
                            Association assoc = Association.Parse (childObject);
                            cc.LogicalModels.Add (assoc);
                        }
                    }
                }
            }

            return cc;
        }
    }

    public class UmlClass : ModelObject
    {
        public UmlClass ()
        {
            Attributes = new List<ClassAttribute> ();
        }

        public string StereoType { get; set; }
        public string SuperClassId { get; set; }
        public string Namespace { get; set; }
        public bool IsAbstract { get; set; }
        public List<ClassAttribute> Attributes { get; private set; }

        public static UmlClass Parse (UmlObject umlObject)
        {
            UmlClass cl = new UmlClass ();
            cl.ParseObject (umlObject);
            cl.StereoType = umlObject.GetStringProperty ("stereotype");

            UmlList list = umlObject.GetListProperty ("superclasses");
            if (list != null && list.Count > 0)
            {
                UmlObject listItem = (UmlObject) list [0];
                cl.SuperClassId = listItem.GetStringProperty ("quidu");
            }

            UmlList classAttrList = umlObject.GetListProperty ("class_attributes");
            if (classAttrList != null)
            {
                foreach (IUmlListItem listItem in classAttrList)
                {
                    UmlObject objectItem = (UmlObject) listItem;
                    cl.Attributes.Add (ClassAttribute.Parse (objectItem));
                }
            }

            string isAbstractText = umlObject.GetStringProperty ("abstract");
            cl.IsAbstract = (isAbstractText != null && isAbstractText == "TRUE");

            return cl;
        }
    }

    public class ClassAttribute : ModelObject
    {
        public string TypeName { get; set; }
        public string TypeId { get; set; }
        public string InitialValue { get; set; }

        public static ClassAttribute Parse (UmlObject umlObject)
        {
            ClassAttribute ca = new ClassAttribute ();
            ca.ParseObject (umlObject);
            ca.TypeName = umlObject.GetStringProperty ("type");
            ca.TypeId = umlObject.GetStringProperty ("quidu");
            ca.InitialValue = umlObject.GetStringProperty ("initv");
            return ca;
        }
    }

    public class Association : ModelObject
    {
        public Role Role1 { get; set; }
        public Role Role2 { get; set; }
        public string AssociationClass { get; set; }

        public static Association Parse (UmlObject umlObject)
        {
            Association assoc = new Association ();
            assoc.ParseObject (umlObject);

            UmlList roleList = umlObject.GetListProperty ("roles");
            if (roleList != null && roleList.Count > 1)
            {
                assoc.Role1 = Role.Parse ((UmlObject) roleList [0]);
                assoc.Role2 = Role.Parse ((UmlObject) roleList [1]);
            }

            assoc.AssociationClass = umlObject.GetStringProperty ("AssociationClass");
            if (assoc.AssociationClass != null)
            {
                assoc.AssociationClass = assoc.AssociationClass.Replace ("::", ".");
            }

            return assoc;
        }
    }

    public class Role : ModelObject
    {
        public string Label { get; set; }
        public string SupplierId { get; set; }
        public string ClientCardinality { get; set; }
        public string Containment { get; set; }
        public bool? IsNavigable { get; set; }

        public static Role Parse (UmlObject umlObject)
        {
            string temp;

            Role role = new Role ();
            role.ParseObject (umlObject);
            temp = umlObject.GetStringProperty ("label");
            if (!string.IsNullOrWhiteSpace (temp))
                role.Label = temp;
            role.SupplierId = umlObject.GetStringProperty ("quidu");

            temp = umlObject.GetStringProperty ("client_cardinality");
            if (temp != null)
                role.ClientCardinality = temp.Replace ("(value cardinality \"", "").Replace ("\")", "");

            temp = umlObject.GetStringProperty ("Containment");
            if (temp == "By Value")
                role.Containment = "VAL";
            else if (temp == "By Reference")
                role.Containment = "REF";
            
            temp = umlObject.GetStringProperty ("is_navigable");
            if (temp != null)
                role.IsNavigable = (temp == "TRUE");
            return role;
        }
    }
}
