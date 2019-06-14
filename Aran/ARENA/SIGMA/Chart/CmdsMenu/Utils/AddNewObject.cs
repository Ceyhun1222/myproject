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
    public partial class AddNewObject : Form
    {
        public List<PDM.PDMObject> OBJ_LIST { get; set; }
        private PDM.PDM_ENUM _objType;
        public AddNewObject()
        {
            InitializeComponent();
        }

        public AddNewObject(string CaptionText, List<PDM.PDMObject> _objLst, PDM.PDM_ENUM objTp)
        {
            InitializeComponent();
            this.Text = CaptionText;
            this.TopMost = true;

            this.OBJ_LIST = _objLst;

            comboBox1.Items.Clear();

            switch (objTp)
            {

                case PDM.PDM_ENUM.Airspace:
                    foreach (var item in _objLst)
                    {

                        if (((PDM.Airspace)item).AirspaceVolumeList != null && ((PDM.Airspace)item).AirspaceVolumeList.Count > 0)
                        {
                            string s = ((PDM.Airspace)item).TxtName != null ? ((PDM.Airspace)item).TxtName : "";
                            if (((PDM.Airspace)item).CodeID != null) s = s = " " + ((PDM.Airspace)item).CodeID ;
                            checkedListBox1.Items.Add(s);
                            comboBox1.Items.Add(s);
                        }
                    }
                    break;
                case PDM.PDM_ENUM.VerticalStructure:
                    foreach (var item in _objLst)
                    {
                        string s = item.GetObjectLabel();

                        if (((PDM.VerticalStructure)item).Parts != null && ((PDM.VerticalStructure)item).Parts.Count > 0)
                        {
                            if (((PDM.VerticalStructure)item).Parts[0].Elev.HasValue) s = s + " ( elev " + Math.Round(((PDM.VerticalStructure)item).Parts[0].Elev.Value, 2).ToString() + ")";
                        }


                        checkedListBox1.Items.Add(s);
                        comboBox1.Items.Add(s);

                    }
                    break;
                case PDM.PDM_ENUM.NavaidSystem:
                    foreach (var item in _objLst)
                    {

                        if (((PDM.NavaidSystem)item).Components != null && ((PDM.NavaidSystem)item).Components.Count > 0)
                        {
                            string s = item.GetObjectLabel();
                            checkedListBox1.Items.Add(s);
                            comboBox1.Items.Add(s);
                        }

                    }
                    break;
                case PDM.PDM_ENUM.AirportHeliport:
                    foreach (var item in _objLst)
                    {
                        string s = item.GetObjectLabel();
                        checkedListBox1.Items.Add(s);
                        comboBox1.Items.Add(s);

                    }
                    break;
                default:
                    break;
            }

            _objType = objTp;


        }

        private void button3_Click(object sender, EventArgs e)
        {
            OBJ_LIST[checkedListBox1.SelectedIndex].GetObjectLabel();
            checkedListBox1.SetItemChecked(comboBox1.SelectedIndex, true);
        }

        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (checkedListBox1.SelectedIndex >=0)
            {
                propertyGrid1.SelectedObject = OBJ_LIST[checkedListBox1.SelectedIndex];

                //switch (_objType)
                //{

                //    case PDM.PDM_ENUM.Airspace:
                //        propertyGrid1.SelectedObject = (PDM.Airspace)OBJ_LIST[checkedListBox1.SelectedIndex];

                //        break;
                //    case PDM.PDM_ENUM.VerticalStructure:
                        
                        
                //        break;
                //    case PDM.PDM_ENUM.NavaidSystem:
                //        propertyGrid1.SelectedObject = (PDM.NavaidSystem)OBJ_LIST[checkedListBox1.SelectedIndex];

                //        break;
                //    case PDM.PDM_ENUM.AirportHeliport:
                        
                //        break;
                //    default:
                //        propertyGrid1.SelectedObject = OBJ_LIST[checkedListBox1.SelectedIndex];
                //        break;
                //}
                
            }
        }
    }
}
