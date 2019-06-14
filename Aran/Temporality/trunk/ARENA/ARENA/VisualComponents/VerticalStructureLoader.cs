using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using PDM.PropertyExtension;
using ARENA.Util;
using System.Reflection;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using Accent.MapElements;

namespace ARENA
{
    public partial class VerticalStructureLoader : UserControl
    {
        public Dictionary<Type, ITable> _AIRTRACK_TableDic;

        public List<PDM.PDMObject> _PdmObjectList;

        public VerticalStructureLoader()
        {
            InitializeComponent();
        }

        private AranSupport.Utilitys ArnUtil;

        private void VerticalStructureLoader_Load(object sender, EventArgs e)
        {
            CreateComponentsPanel(ref this.panelContainer, new Intermediate_VerticalStrucrure());
        }

        private int CreateComponentsPanel(ref Panel panel, Intermediate_VerticalStrucrure Obj)
        {
            this.panelContainer.SuspendLayout();

            int cntrlCntr = 0;
            bool flagMandatory = false;

            #region Sort Properties by PropertyOrderAttribute

            PropertyDescriptorCollection pdc = TypeDescriptor.GetProperties(Obj);
            ArrayList orderedProperties = new ArrayList();

            foreach (PropertyDescriptor pd in pdc)
            {

                Attribute attribute = pd.Attributes[typeof(PropertyOrderAttribute)];

                if (attribute != null)
                {
                    // атрибут есть - используем номер п/н из него
                    PropertyOrderAttribute poa = (PropertyOrderAttribute)attribute;
                    orderedProperties.Add(new PropertyOrderPair(pd.Name, poa.Order));
                }
                else
                {
                    // атрибута нет – считаем, что 0
                    orderedProperties.Add(new PropertyOrderPair(pd.Name, 0));
                }
            }

            orderedProperties.Sort();

            // формируем список имен свойств
            ArrayList propertyNames = new ArrayList();

            foreach (PropertyOrderPair pop in orderedProperties)
                propertyNames.Add(pop.Name);

            PropertyDescriptorCollection pdcsort = pdc.Sort((string[])propertyNames.ToArray(typeof(string)));


            #endregion

            var list = new List<string>();

            for (int i = pdcsort.Count - 1; i >= 0; i--)
            {

                flagMandatory = false;

                PropertyDescriptor pd = pdcsort[i];
                Attribute BrowsableFlag = pd.Attributes[typeof(BrowsableAttribute)];
                Attribute MandatoryFalg = pd.Attributes[typeof(Mandatory)];

                if (BrowsableFlag != null)
                {
                    if (!((BrowsableAttribute)BrowsableFlag).Browsable) continue;
                }

                if (MandatoryFalg != null) flagMandatory = ((Mandatory)MandatoryFalg).MandatoryFlag;

                VisualComponents.EnumProperty cntrl = new VisualComponents.EnumProperty(flagMandatory);

                cntrl.Dock = System.Windows.Forms.DockStyle.Top;
                cntrl.PropertyName = pd.Name;
                cntrl.PropertyValueCmbBx.DropDownStyle = ComboBoxStyle.DropDown;
                cntrl.PropertyValueCmbBx.FlatStyle = FlatStyle.Flat;

                //cntrl.PropertyValueCmbBx.Items.Add("");
                if (pd.PropertyType.IsEnum)
                {
                    #region

                    string[] itms = Enum.GetNames(pd.PropertyType);

                    cntrl.PropertyType = "Enum";
                    cntrl.PropertyValueCmbBx.Items.AddRange(itms);
                    cntrl.PropertyValueCmbBx.Tag = pd.Name;
                    //cntrl.PropertyValueCmbBx.Tag = Activator.CreateInstance(pd.PropertyType);
                    cntrl.PropertyValueCmbBx.SelectedIndexChanged += new EventHandler(PropertyValueCmbBx_SelectedIndexChanged);

                    #endregion
                }

                else if (!pd.PropertyType.Name.StartsWith("Boolean"))
                {

                    #region

                    cntrl.PropertyType = "Value";
                    cntrl.PropertyValueCmbBx.SelectedIndexChanged += new EventHandler(PropertyValueCmbBx_SelectedIndexChanged);
                    cntrl.PropertyValueCmbBx.Tag = pd.Name;
                    //cntrl.PropertyValueCmbBx.Tag = Activator.CreateInstance(pd.PropertyType);
                    //list.Insert(0,pd.Name);


                    #endregion

                }
                else if (pd.PropertyType.Name.StartsWith("Boolean"))
                {

                    #region 

                    List<string> YesNo = new List<string>();
                    YesNo.Add(@"YES");
                    YesNo.Add(@"NO");

                    cntrl.PropertyType = "Boolean";
                    cntrl.PropertyValueCmbBx.SelectedIndexChanged += new EventHandler(PropertyValueCmbBx_SelectedIndexChanged);
                    cntrl.PropertyValueCmbBx.Tag = pd.Name;
                    cntrl.PropertyValueCmbBx.Items.AddRange(YesNo.ToArray());
                    //cntrl.PropertyValueCmbBx.Tag = Activator.CreateInstance(pd.PropertyType);

                    #endregion
                    
                }

                list.Insert(0, pd.Name);
                panel.Controls.Add(cntrl);


                cntrlCntr++;

            }

            dataGridView1.Tag = list;

            this.panelContainer.ResumeLayout();


            comboBox1.SelectedIndex = 0;
            return cntrlCntr;
        }

        void PropertyValueCmbBx_SelectedIndexChanged(object sender, EventArgs e)
        {
           if (((ComboBox)sender).Items.Count<=0) return;

           string Coltext =((ComboBox)sender).Text;

           DataGridViewCellStyle dataGridViewCellStyle_new = new System.Windows.Forms.DataGridViewCellStyle();
           dataGridViewCellStyle_new.BackColor = Color.Gray;

           DataGridViewCellStyle dataGridViewCellStyle_default = new System.Windows.Forms.DataGridViewCellStyle();
           dataGridViewCellStyle_default = dataGridView1.Columns[Coltext].DefaultCellStyle;

           dataGridView1.Columns[Coltext].HeaderText = Coltext + " " + ((ComboBox)sender).Tag.ToString();


        }

        private void button1_Click(object sender, EventArgs e)
        {

            DataTable dtData;
            

            using (OpenFileDialog ofdOpen = new OpenFileDialog() { Filter = @"Excell files|*.xlsx|Excell files 97-2003|*.xls" })
            {
                if (ofdOpen.ShowDialog() != DialogResult.OK) return;

                textBox1.Text = ofdOpen.FileName;
                cExcel.ExcelFileToDataTable(out dtData, ofdOpen.FileName, "SELECT * FROM $SHEETS$");

                BindingSource Excel_BindingSource = new System.Windows.Forms.BindingSource();

                ((System.ComponentModel.ISupportInitialize)(Excel_BindingSource)).BeginInit();
                Excel_BindingSource.DataMember = dtData.TableName;
                Excel_BindingSource.DataSource = dtData.DataSet;
                dataGridView1.DataSource = Excel_BindingSource;
                ((System.ComponentModel.ISupportInitialize)(Excel_BindingSource)).EndInit();

                List<string> dataGrigColumnsName = new List<string>();

                foreach (DataGridViewColumn col in dataGridView1.Columns)
                {
                    col.HeaderCell = new DataGridViewAutoFilterColumnHeaderCell(col.HeaderCell);
                    col.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
                    (col.HeaderCell as DataGridViewAutoFilterColumnHeaderCell).ColumnHeaderFilterChanged += new DataGridViewAutoFilterColumnHeaderCell.ColumnHeaderFilterDelegate(VerticalStructureLoader_ColumnHeaderFilterChanged);
                    col.Name = col.HeaderText;

                    dataGrigColumnsName.Add(col.HeaderText);
                }

                if (dataGrigColumnsName.Count > 0)
                {
                    foreach (Control cntrl in panelContainer.Controls)
                    {
                        if (cntrl is VisualComponents.EnumProperty)
                        {

                            if ((((VisualComponents.EnumProperty)cntrl).PropertyType.StartsWith("Enum")) || (((VisualComponents.EnumProperty)cntrl).PropertyType.StartsWith("Boolean")))
                            {
                                ClearEnumsCmbBx(((VisualComponents.EnumProperty)cntrl).PropertyValueCmbBx.Items);
                                ((VisualComponents.EnumProperty)cntrl).PropertyValueCmbBx.Items.Add("- - - - - - - - -");
                            }
                            else
                                ((VisualComponents.EnumProperty)cntrl).PropertyValueCmbBx.Items.Clear();

                            ((VisualComponents.EnumProperty)cntrl).PropertyValueCmbBx.Items.AddRange(dataGrigColumnsName.ToArray());
                            

                        }

                    }
                }
            }

        }

        private void ClearEnumsCmbBx(ComboBox.ObjectCollection objectCollection)
        {

            int ind = objectCollection.IndexOf("- - - - - - - - -");
            if (ind < 0) return;
            //ind++;
            List<object> res = new List<object>();

            foreach (var item in objectCollection)
            {
                if (objectCollection.IndexOf(item) < ind)
                    res.Add(item);
            }

            if (res.Count > 0)
            {
                objectCollection.Clear();
                objectCollection.AddRange(res.ToArray());
            }

        }

        void VerticalStructureLoader_ColumnHeaderFilterChanged(string ColFilter)
        {
            string[] WORDS = ColFilter.Split();
            if (WORDS.Length < 2) return;
            string propertyName = WORDS[WORDS.Length-1];
            string gridColumnHeader = WORDS[0];
            for (int i = 1; i < WORDS.Length-1; i++)
            {
                gridColumnHeader = gridColumnHeader+" "+WORDS[i];
            }

            foreach (Control cntrl in panelContainer.Controls)
            {
                //if (cntrl is VisualComponents.EnumProperty)
                {
                    if (((VisualComponents.EnumProperty)cntrl).PropertyName.CompareTo(propertyName)!=0) continue;

                    ((VisualComponents.EnumProperty)cntrl).PropertyValueCmbBx.Text = gridColumnHeader;
                    break;

                }

            }

        }

        private void panel2_Resize(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            List<AdressLink> _list = GetAdressList();

            if (_list.Count > 0)
            {
                string vsName = "";
                List<Intermediate_VerticalStrucrure> vsList = new List<Intermediate_VerticalStrucrure>();

                progressBar1.Visible = true;
                progressBar1.Maximum = dataGridView1.RowCount;
                progressBar1.Value = 0;
                ArnUtil = new AranSupport.Utilitys();

                foreach (var item in ((BindingSource)dataGridView1.DataSource))
                {
                    Intermediate_VerticalStrucrure vs = new Intermediate_VerticalStrucrure();
                    DataRowView DR = (DataRowView)item;

                    foreach (var link in _list)
                    {
                        var _intermediateValue = link.PropVal;
                        if (link.LinkToFile) _intermediateValue = DR[link.PropVal].ToString();

                        if (vs.GetType().GetProperty(link.PropName).PropertyType.IsEnum)
                        {
                            string _val = _intermediateValue.ToString();
                            var res = Enum.Parse(vs.GetType().GetProperty(link.PropName).PropertyType, _val);
                            SetObjectValue(vs, link.PropName, res);
                        }
                        else if (Type.GetTypeCode(vs.GetType().GetProperty(link.PropName).PropertyType) == TypeCode.Double)
                        {
                            try
                            {
                                double dVal = Double.Parse(_intermediateValue);
                                SetObjectValue(vs, link.PropName, dVal);
                            }
                            catch { }
                        }
                        else if (Type.GetTypeCode(vs.GetType().GetProperty(link.PropName).PropertyType) == TypeCode.Int32)
                        {
                            try{
                            int dVal = Int32.Parse(_intermediateValue);
                            SetObjectValue(vs, link.PropName, dVal);
                            }
                            catch { }
                        }
                        else if (Type.GetTypeCode(vs.GetType().GetProperty(link.PropName).PropertyType) == TypeCode.Boolean)
                        {
                            try
                            {
                                bool dVal = _intermediateValue.CompareTo("YES")==0;
                                SetObjectValue(vs, link.PropName, dVal);
                            }
                            catch { }
                        }
                        else
                        SetObjectValue(vs, link.PropName, _intermediateValue);
                       
                    }

                    string _valueFromDataRow = GetObjectValue(vs, "VertcalStructureName");

                    if ((_valueFromDataRow.CompareTo(vsName) != 0) && (vsName.Length>0))
                    {

                        PDM.PDMObject _obj = CreateVerticalStructure(vsList, (VerticalStructureGeometryType)comboBox1.SelectedIndex);
                        if (_obj != null)
                        {
                            _obj.StoreToDB(_AIRTRACK_TableDic);
                            _PdmObjectList.Add(_obj);
                        }

                        vsList = new List<Intermediate_VerticalStrucrure>();
                    }
                    vsName = _valueFromDataRow;
                    vsList.Add(vs);

                    progressBar1.Value++;

                    
                }

                progressBar1.Visible = false;

               
            }

        }

        private PDM.PDMObject CreateVerticalStructure(List<Intermediate_VerticalStrucrure> vsList, VerticalStructureGeometryType VSGeometryTYPE)
        {
            try
            {
                PDM.VerticalStructure vs = new PDM.VerticalStructure ();
                vs.Group = vsList.Count > 1;
                vs.Length = vsList[0].Length;
                vs.Length_UOM = vsList[0].Length_UOM;
                vs.Name = vsList[0].VertcalStructureName;
                //vs.Type = vsList[0].Type;
                if (vsList[0] != null) vs.Lighted = vsList[0].Lighted;
                if (vsList[0] != null) vs.LightingICAOStandard = vsList[0].LightingICAOStandard;
                if (vsList[0] != null) vs.SynchronisedLighting = vsList[0].SynchronisedLighting;
                if (vsList[0] != null) vs.MarkingICAOStandard = vsList[0].MarkingICAOStandard;
                vs.Radius = vsList[0].Radius;
                vs.Radius_UOM = vsList[0].Radius_UOM;
                vs.Width = vsList[0].Width;
                vs.Width_UOM = vsList[0].Width_UOM;

                vs.Parts = new List<PDM.VerticalStructurePart>();

                switch (VSGeometryTYPE)
                {
                    case VerticalStructureGeometryType.POINT:

                        #region 

                        foreach (var item in vsList)
                        {

                            PDM.VerticalStructurePart partPnt = new PDM.VerticalStructurePart();
                            partPnt.ConstructionStatus = item.PartConstructionStatus;
                            partPnt.Designator = item.PartDesignator;
                            partPnt.Frangible = item.PartFrangible;
                            partPnt.Height = item.PartHeight;
                            partPnt.Height_UOM = item.PartHeight_UOM;
                            partPnt.MarkingFirstColour = item.PartMarkingFirstColour;
                            partPnt.MarkingPattern = item.PartMarkingPattern;
                            partPnt.MarkingSecondColour = item.PartMarkingSecondColour;
                            partPnt.Type = item.PartType;
                            partPnt.VerticalExtent = item.PartVerticalExtent;
                            partPnt.VerticalExtent_UOM = item.PartVerticalExtent_UOM;
                            partPnt.VerticalExtentAccuracy = item.PartVerticalExtentAccuracy;
                            partPnt.VerticalExtentAccuracy_UOM = item.PartVerticalExtentAccuracy_UOM;
                            partPnt.VisibleMaterial = item.PartVisibleMaterial;
                            partPnt.VerticalStructure_ID = vs.ID;

                            if ((item.Lat.Trim().Length > 0) && (item.Lon.Trim().Length > 0))
                            {
                                partPnt.Lat = item.Lat;
                                partPnt.Lon = item.Lon;

                                IPoint pnt = ArnUtil.Create_ESRI_POINT(partPnt.Lat.Trim(), partPnt.Lon.Trim(), partPnt.Elev.ToString(), partPnt.Elev_UOM.ToString());
                                if (pnt != null)
                                {
                                    partPnt.Geo = pnt;
                                    partPnt.Vertex = HelperClass.SetObjectToBlob(partPnt.Geo, "Vertex");
                                }


                            }

                            if (partPnt != null) vs.Parts.Add(partPnt);
                        }

                        #endregion

                        break;

                    case VerticalStructureGeometryType.LINE:

                        #region 
                        
                            Intermediate_VerticalStrucrure IntermediatePartLine = vsList[0];

                            PDM.VerticalStructurePart partLine = new PDM.VerticalStructurePart();
                            partLine.ConstructionStatus = IntermediatePartLine.PartConstructionStatus;
                            partLine.Designator = IntermediatePartLine.PartDesignator;
                            partLine.Frangible = IntermediatePartLine.PartFrangible;
                            partLine.Height = IntermediatePartLine.PartHeight;
                            partLine.Height_UOM = IntermediatePartLine.PartHeight_UOM;
                            partLine.MarkingFirstColour = IntermediatePartLine.PartMarkingFirstColour;
                            partLine.MarkingPattern = IntermediatePartLine.PartMarkingPattern;
                            partLine.MarkingSecondColour = IntermediatePartLine.PartMarkingSecondColour;
                            partLine.Type = IntermediatePartLine.PartType;
                            partLine.VerticalExtent = IntermediatePartLine.PartVerticalExtent;
                            partLine.VerticalExtent_UOM = IntermediatePartLine.PartVerticalExtent_UOM;
                            partLine.VerticalExtentAccuracy = IntermediatePartLine.PartVerticalExtentAccuracy;
                            partLine.VerticalExtentAccuracy_UOM = IntermediatePartLine.PartVerticalExtentAccuracy_UOM;
                            partLine.VisibleMaterial = IntermediatePartLine.PartVisibleMaterial;
                            partLine.VerticalStructure_ID = vs.ID;


                            ESRI.ArcGIS.Geometry.IPointCollection4 partLineGeom = new ESRI.ArcGIS.Geometry.PolylineClass();

                            foreach (var vsPrt in vsList)
                            {
                                if ((vsPrt.Lat.Trim().Length > 0) && (vsPrt.Lon.Trim().Length > 0))
                                {
                                    partLine.Lat = vsPrt.Lat;
                                    partLine.Lon = vsPrt.Lon;

                                    IPoint pnt = ArnUtil.Create_ESRI_POINT(partLine.Lat.Trim(), partLine.Lon.Trim(), partLine.Elev.ToString(), partLine.Elev_UOM.ToString());
                                    if (pnt != null)
                                    {
                                        partLineGeom.AddPoint(pnt);
                                        
                                    }

                                }
                            }


                            var zAware = partLineGeom as IZAware;
                            zAware.ZAware = true;


                            var mAware = partLineGeom as IMAware;
                            mAware.MAware = true;

                            partLine.Geo = (IGeometry)partLineGeom;
                            partLine.Vertex = HelperClass.SetObjectToBlob(partLine.Geo, "Vertex");
                            if (partLine != null) vs.Parts.Add(partLine);

                        #endregion

                        break;
                    case VerticalStructureGeometryType.POLYGON:

                        #region 
                        
                            Intermediate_VerticalStrucrure IntermediatePartPoly = vsList[0];

                            PDM.VerticalStructurePart partPoly = new PDM.VerticalStructurePart();
                            partPoly.ConstructionStatus = IntermediatePartPoly.PartConstructionStatus;
                            partPoly.Designator = IntermediatePartPoly.PartDesignator;
                            partPoly.Frangible = IntermediatePartPoly.PartFrangible;
                            partPoly.Height = IntermediatePartPoly.PartHeight;
                            partPoly.Height_UOM = IntermediatePartPoly.PartHeight_UOM;
                            partPoly.MarkingFirstColour = IntermediatePartPoly.PartMarkingFirstColour;
                            partPoly.MarkingPattern = IntermediatePartPoly.PartMarkingPattern;
                            partPoly.MarkingSecondColour = IntermediatePartPoly.PartMarkingSecondColour;
                            partPoly.Type = IntermediatePartPoly.PartType;
                            partPoly.VerticalExtent = IntermediatePartPoly.PartVerticalExtent;
                            partPoly.VerticalExtent_UOM = IntermediatePartPoly.PartVerticalExtent_UOM;
                            partPoly.VerticalExtentAccuracy = IntermediatePartPoly.PartVerticalExtentAccuracy;
                            partPoly.VerticalExtentAccuracy_UOM = IntermediatePartPoly.PartVerticalExtentAccuracy_UOM;
                            partPoly.VisibleMaterial = IntermediatePartPoly.PartVisibleMaterial;
                            partPoly.VerticalStructure_ID = vs.ID;


                            ESRI.ArcGIS.Geometry.IPointCollection4 partPolygonGeom = new ESRI.ArcGIS.Geometry.PolygonClass();

                            foreach (var vsPrt in vsList)
                            {
                                if ((vsPrt.Lat.Trim().Length > 0) && (vsPrt.Lon.Trim().Length > 0))
                                {
                                    partPoly.Lat = vsPrt.Lat;
                                    partPoly.Lon = vsPrt.Lon;

                                    IPoint pnt = ArnUtil.Create_ESRI_POINT(partPoly.Lat.Trim(), partPoly.Lon.Trim(), partPoly.Elev.ToString(), partPoly.Elev_UOM.ToString());
                                    if (pnt != null)
                                    {
                                        partPolygonGeom.AddPoint(pnt);
                                        
                                    }

                                }
                            }

                            ((PolygonClass)partPolygonGeom).Simplify();


                            var zAware1 = partPolygonGeom as IZAware;
                            zAware1.ZAware = true;
                            ((PolygonClass)partPolygonGeom).SetConstantZ(0);

                            var mAware1 = partPolygonGeom as IMAware;
                            mAware1.MAware = true;

                            partPoly.Geo = (IGeometry)partPolygonGeom;
                            partPoly.Vertex = HelperClass.SetObjectToBlob(partPoly.Geo, "Vertex");
                            if (partPoly != null) vs.Parts.Add(partPoly);

                        #endregion

                        break;

                    default:
                        break;
                }





                return vs;


            }
            catch { return null; }
        }

        private List<AdressLink> GetAdressList()
        {
            List<AdressLink> _list =  new List<AdressLink>();

            foreach (var cntrl in panelContainer.Controls)
            {

                if (cntrl is VisualComponents.EnumProperty)
                {
                    var cmbx = (VisualComponents.EnumProperty)cntrl;
                    if (cmbx.PropertyValueCmbBx.SelectedIndex < 0) continue;

                    AdressLink adress = new AdressLink { PropVal = cmbx.PropertyValueCmbBx.Text, LinkToFile = true, PropName = cmbx.PropertyName };

                    if ((cmbx.PropertyType.StartsWith("Enum")) || (cmbx.PropertyType.StartsWith("Boolean")))
                    {

                        if (cmbx.PropertyValueCmbBx.SelectedIndex < cmbx.PropertyValueCmbBx.Items.IndexOf("- - - - - - - - -"))
                        {
                            adress.LinkToFile = false;
                        }
                        else
                        {
                            adress.LinkToFile = true;
                        }

                    }

                    _list.Add(adress);
                }

            }

            return _list;

        }

        private bool SetObjectValue(object EditedObject, string PropertyName, object Value)
        {

            object objVal2 = EditedObject;

            string[] sa = PropertyName.Split('.');
            if (sa.Length == 0) return false;

            foreach (string s in sa)
            {
                PropertyInfo propInfo = EditedObject.GetType().GetProperty(s);

                if (propInfo == null) continue;
                try
                {
                    propInfo.SetValue(EditedObject, Value, null);
                }
                catch(Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    return false;
                }

            }
            return true;


        }

        private string GetObjectValue(object obj, string propName)
        {

            object objVal = obj;

            string[] sa = propName.Split('.');
            if (sa.Length == 0)
                return null;

            foreach (string s in sa)
            {
                PropertyInfo propInfo = objVal.GetType().GetProperty(s);

                if (propInfo == null)
                    return null;

                object objPropVal = propInfo.GetValue(objVal, null);


                if (objPropVal is IList)
                {
                    objPropVal = (objPropVal as IList)[0];
                }

                objVal = objPropVal;

                if (objVal == null)
                    return null;
            }

            ////System.Diagnostics.Debug.WriteLine(propName + " - " + objVal.GetType().ToString());

            return (objVal == null ? "<null>" : objVal.ToString());
        }

        private enum VerticalStructureGeometryType
        {
            POINT = 0,
            LINE = 1,
            POLYGON = 2
        }

    }

    public class AdressLink
    {
        public bool LinkToFile;
        public string PropVal;
        public string PropName;

        public AdressLink()
        {
        }
    }



}
