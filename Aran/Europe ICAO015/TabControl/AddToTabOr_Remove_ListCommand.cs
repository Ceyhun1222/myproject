using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data;


namespace ICAO015.TabControl
{
    public class AddToTabOr_Remove_ListCommand
    {
        DataTable tableforDME = new DataTable();
        DataTable tableforILS = new DataTable();

        //List<AddToTab_List_DME> AddToList = new List<AddToTab_List_DME>();

        public AddToTabOr_Remove_ListCommand()
        {
            //tableforDME
            tableforDME.Columns.Add("Type of Navigation");
            tableforDME.Columns.Add("Navaid Name");
            tableforDME.Columns.Add("Radius R Cylinder");
            tableforDME.Columns.Add("Alpha Cone");
            tableforDME.Columns.Add("Radius R Cone");
            tableforDME.Columns.Add("Radius J Cylinder");
            tableforDME.Columns.Add("Height of Cylinder Wind Turbine");
            tableforDME.Columns.Add("Origin of cone and axis of cylinder");
            //
            //
            //tableforILS-------
            tableforILS.Columns.Add("Type OF Navigation");
            tableforILS.Columns.Add("Navaid Name");
            tableforILS.Columns.Add("A");
            tableforILS.Columns.Add("B");
            tableforILS.Columns.Add("Segment h");
            tableforILS.Columns.Add("Radius");
            tableforILS.Columns.Add("D");
            tableforILS.Columns.Add("Corner Polygons H");
            tableforILS.Columns.Add("L");
            tableforILS.Columns.Add("F");
            tableforILS.Columns.Add("True Bearing");
            //
        }

        public void AddToTabList_FOR_DME(DataGridView Grid, string TypeofNavigation, string CheckedNavaid, string RadiusrCylinder, string Alphacone, string RadiusRCone, string RadiusJCylinder, string HeightofCylinderWindTurbine, string OriginofConeandAxisofCylinders)
        {
            //AddToTab_List_DME ListOFDME = new AddToTab_List_DME();
            //ListOFDME.TypeOFNavigation = TypeofNavigation;
            //ListOFDME.CheckedNavaid = CheckedNavaid;
            //ListOFDME.RadiusRCylinder = RadiusrCylinder;
            //ListOFDME.AlphaCone = Alphacone;
            //ListOFDME.RadiusRCone = RadiusRCone;
            //ListOFDME.RadiusJCylinder = RadiusJCylinder;
            //ListOFDME.HeightOfCylndrWindTurbine = HeightofCylinderWindTurbine;

            //AddToList.Add(ListOFDME);

            DataRow row = tableforDME.NewRow();
            row["Type of Navigation"] = TypeofNavigation;
            row["Navaid Name"] = CheckedNavaid;
            row["Radius R Cylinder"] = RadiusrCylinder;
            row["Alpha Cone"] = Alphacone;
            row["Radius R Cone"] = RadiusRCone;
            row["Radius J Cylinder"] = RadiusJCylinder;
            row["Height of Cylinder Wind Turbine"] = HeightofCylinderWindTurbine;
            row["Origin of cone and axis of cylinder"] = OriginofConeandAxisofCylinders;
            tableforDME.Rows.Add(row);

            Grid.DataSource = tableforDME;
        }
        public void AddToTabList_ILS(DataGridView GridFor_ILS, string typeofnavigation, string checkednavaid, string a, string b, string segmenth, string radius, string d, string cornerpolygonh, string L, string f, string truebearing)
        {
            DataRow row = tableforILS.NewRow();
            row["Type OF Navigation"] = typeofnavigation;
            row["Navaid Name"] = checkednavaid;
            row["A"] = a;
            row["B"] = b;
            row["Segment h"] = segmenth;
            row["Radius"] = radius;
            row["D"] = d;
            row["Corner Polygons H"] = cornerpolygonh;
            row["L"] = L;
            row["F"] = f;
            row["True Bearing"] = truebearing;
            tableforILS.Rows.Add(row);

            GridFor_ILS.DataSource = tableforILS;
        }
        public void RemoveTabList_ILSORDME(DataGridView Grid, TreeNode UncheckNode)
        {
            TreeNode ParentNode = UncheckNode.Parent;

            for (int i = 0; i < Grid.Rows.Count; i++)
            {
                if (Grid.Rows[i].Cells[0].Value.ToString() == ParentNode.Text && Grid.Rows[i].Cells[1].Value.ToString() == UncheckNode.Text)
                {
                    Grid.Rows.RemoveAt(Grid.Rows[i].Index);
                    i--;
                    //Grid.Refresh();
                }
            }

        }
    }
}
