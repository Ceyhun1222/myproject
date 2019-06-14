using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data;

namespace ICAO015
{
    public class ObstacleReport_Add_Or_Remove_From_Grid
    {
        public void VORORDME_StepbyStep_Check_Show_ObstacleReportAdd(DataGridView GridObstclData, DataTable table, TreeNode Childnode, TreeNode secondparent, List<ReportForDVOR600R> ListforDvor600, List<ReportForDVOR3000R> ListforDvor3000, List<ReportForDVOR10000R> ListforDvor10000, List<ReportDvorForWindTurbine> ListforDvorForWindTurbine,
           List<ReportForCVOR600r> ListforCvor600, List<ReportForCVOR3000r> ListforCvor3000, List<ReportForCVOR15000r> ListforCvor15000, List<ReportCvorForWindTurbine> ListforCvorForWindTurbine,
            List<ReportForDme300r> ListforDmen300, List<ReportForDme3000r> ListforDmen3000)
        {
            if (secondparent.Text == "DVOR")
            {
                if (Childnode.Text == "Radius for 600")
                {

                    for (int i = 0; i < GridObstclData.Rows.Count; i++)
                    {
                        if (GridObstclData.Rows[i].Cells[3].Value.ToString() == null)
                            continue;
                        if (GridObstclData.Rows[i].Cells[3].Value.ToString() == "600" && GridObstclData.Rows[i].Cells[1].Value.ToString() == Childnode.Parent.Text)
                        {
                            GridObstclData.Rows.RemoveAt(GridObstclData.Rows[i].Index);
                            i--;
                            //GridObstclData.Refresh();
                        }
                    }




                    if (ListforDvor600.Count > 0)
                    {
                        for (int q = 0; q < ListforDvor600.Count; q++)
                        {
                            if (ListforDvor600[q].NavaidName == Childnode.Parent.Text)
                            {
                                DataRow row = table.NewRow();
                                row["TypeOfNavigation"] = ListforDvor600[q].TypeOFNAvigation.ToString();
                                row["NavaidName"] = ListforDvor600[q].NavaidName.ToString();
                                row["ID"] = ListforDvor600[q].ID.ToString();
                                row["Radius"] = ListforDvor600[q].Radius.ToString();
                                row["Obstacle"] = ListforDvor600[q].Obstacle.ToString();
                                row["Elevation"] = ListforDvor600[q].Elevation.ToString();
                                row["Distance(meter)"] = Convert.ToInt32(ListforDvor600[q].Distance).ToString();
                                row["Penetrate"] = ListforDvor600[q].Penetrate.ToString();
                                row["GeoType"] = ListforDvor600[q].TypeGeo.ToString();

                                table.Rows.Add(row);
                            }
                        }
                    }


                }
                if (Childnode.Text == "Radius for 3000")
                {


                    for (int i = 0; i < GridObstclData.Rows.Count; i++)
                    {
                        if (GridObstclData.Rows[i].Cells[3].Value.ToString() == null)
                            continue;
                        if (GridObstclData.Rows[i].Cells[3].Value.ToString() == "3000" && GridObstclData.Rows[i].Cells[1].Value.ToString() == Childnode.Parent.Text)
                        {
                            GridObstclData.Rows.RemoveAt(GridObstclData.Rows[i].Index);
                            i--;
                            //GridObstclData.Refresh();
                        }
                    }



                    if (ListforDvor3000.Count > 0)
                    {
                        for (int i = 0; i < ListforDvor3000.Count; i++)
                        {
                            if (ListforDvor3000[i].NavaidName == Childnode.Parent.Text)
                            {
                                DataRow row = table.NewRow();
                                row["TypeOfNavigation"] = ListforDvor3000[i].TypeOFNAvigation.ToString();
                                row["ID"] = ListforDvor3000[i].ID.ToString();
                                row["Radius"] = ListforDvor3000[i].Radius.ToString();
                                row["Obstacle"] = ListforDvor3000[i].Obstacle.ToString();
                                row["Elevation"] = ListforDvor3000[i].Elevation.ToString();
                                row["Distance(meter)"] = Convert.ToInt32(ListforDvor3000[i].Distance).ToString();
                                row["Penetrate"] = ListforDvor3000[i].Penetrate.ToString();
                                row["GeoType"] = ListforDvor3000[i].TypeGeo.ToString();
                                row["NavaidName"] = ListforDvor3000[i].NavaidName.ToString();
                                table.Rows.Add(row);
                            }
                        }
                    }
                }
                if (Childnode.Text == "Radius for 10000")
                {

                    for (int i = 0; i < GridObstclData.Rows.Count; i++)
                    {
                        if (GridObstclData.Rows[i].Cells[3].Value.ToString() == null)
                            continue;
                        if (GridObstclData.Rows[i].Cells[3].Value.ToString() == "10000" && GridObstclData.Rows[i].Cells[1].Value.ToString() == Childnode.Parent.Text)
                        {
                            GridObstclData.Rows.RemoveAt(GridObstclData.Rows[i].Index);
                            i--;
                            //GridObstclData.Refresh();
                        }
                    }



                    if (ListforDvor10000.Count > 0)
                    {
                        for (int i = 0; i < ListforDvor10000.Count; i++)
                        {
                            if (ListforDvor10000[i].NavaidName == Childnode.Parent.Text)
                            {
                                DataRow row = table.NewRow();
                                row["TypeOfNavigation"] = ListforDvor10000[i].TypeOFNAvigation.ToString();
                                row["ID"] = ListforDvor10000[i].ID.ToString();
                                row["Radius"] = ListforDvor10000[i].Radius.ToString();
                                row["Obstacle"] = ListforDvor10000[i].Obstacle.ToString();
                                row["Elevation"] = ListforDvor10000[i].Elevation.ToString();
                                row["Distance(meter)"] = Convert.ToInt32(ListforDvor10000[i].Distance).ToString();
                                row["Penetrate"] = ListforDvor10000[i].Penetrate.ToString();
                                row["GeoType"] = ListforDvor10000[i].TypeGeo.ToString();
                                row["NavaidName"] = ListforDvor10000[i].NavaidName.ToString();
                                table.Rows.Add(row);
                            }
                        }
                    }
                }
                if (Childnode.Text == "Wind Turbine")
                {

                    for (int i = 0; i < GridObstclData.Rows.Count; i++)
                    {
                        if (GridObstclData.Rows[i].Cells[1].Value.ToString() == null)
                            continue;
                        if (GridObstclData.Rows[i].Cells[4].Value.ToString() == "Wind Turbine") // it will be fix soon
                        {
                            GridObstclData.Rows.RemoveAt(GridObstclData.Rows[i].Index);
                            i--;
                            //GridObstclData.Refresh();
                        }
                    }


                    if (ListforDvorForWindTurbine.Count > 0)
                    {
                        for (int i = 0; i < ListforDvorForWindTurbine.Count; i++)
                        {
                            DataRow row = table.NewRow();
                            row["TypeOfNavigation"] = ListforDvorForWindTurbine[i].TypeOfNavigation.ToString();
                            row["ID"] = "Empty";
                            row["Radius"] = "Empty";
                            row["Obstacle"] = ListforDvorForWindTurbine[i].Obstacle_name.ToString();
                            row["Elevation"] = "Empty";
                            row["Distance(meter)"] = "Empty";
                            row["Penetrate"] = ListforDvorForWindTurbine[i].Penetrate.ToString();
                            row["GeoType"] = "Empty";
                            row["NavaidName"] = ListforDvorForWindTurbine[i].NavaidName.ToString();
                            table.Rows.Add(row);
                        }
                    }
                }
            }
            if (secondparent.Text == "CVOR")
            {
                if (Childnode.Text == "Radius for 600")
                {



                    for (int i = 0; i < GridObstclData.Rows.Count; i++)
                    {
                        if (GridObstclData.Rows[i].Cells[3].Value.ToString() == null)
                            continue;
                        if (GridObstclData.Rows[i].Cells[3].Value.ToString() == "600" && GridObstclData.Rows[i].Cells[1].Value.ToString() == Childnode.Parent.Text)
                        {
                            GridObstclData.Rows.RemoveAt(GridObstclData.Rows[i].Index);
                            i--;
                            //GridObstclData.Refresh();
                        }
                    }




                    if (ListforCvor600.Count > 0)
                    {
                        for (int q = 0; q < ListforCvor600.Count; q++)
                        {
                            if (ListforCvor600[q].NavaidName == Childnode.Parent.Text)
                            {
                                DataRow row = table.NewRow();
                                row["TypeOfNavigation"] = ListforCvor600[q].TypeOFNAvigation.ToString();
                                row["NavaidName"] = ListforCvor600[q].NavaidName.ToString();
                                row["ID"] = ListforCvor600[q].ID.ToString();
                                row["Radius"] = ListforCvor600[q].Radius.ToString();
                                row["Obstacle"] = ListforCvor600[q].Obstacle.ToString();
                                row["Elevation"] = ListforCvor600[q].Elevation.ToString();
                                row["Distance(meter)"] = Convert.ToInt32(ListforCvor600[q].Distance).ToString();
                                row["Penetrate"] = ListforCvor600[q].Penetrate.ToString();
                                row["GeoType"] = ListforCvor600[q].TypeGeo.ToString();

                                table.Rows.Add(row);
                            }
                        }
                    }
                }
                if (Childnode.Text == "Radius for 3000")
                {



                    for (int i = 0; i < GridObstclData.Rows.Count; i++)
                    {
                        if (GridObstclData.Rows[i].Cells[3].Value.ToString() == null)
                            continue;
                        if (GridObstclData.Rows[i].Cells[3].Value.ToString() == "3000" && GridObstclData.Rows[i].Cells[1].Value.ToString() == Childnode.Parent.Text)
                        {
                            GridObstclData.Rows.RemoveAt(GridObstclData.Rows[i].Index);
                            i--;
                            //GridObstclData.Refresh();
                        }
                    }




                    if (ListforCvor3000.Count > 0)
                    {
                        for (int q = 0; q < ListforCvor3000.Count; q++)
                        {
                            if (ListforCvor3000[q].NavaidName == Childnode.Parent.Text)
                            {
                                DataRow row = table.NewRow();
                                row["TypeOfNavigation"] = ListforCvor3000[q].TypeOFNAvigation.ToString();
                                row["NavaidName"] = ListforCvor3000[q].NavaidName.ToString();
                                row["ID"] = ListforCvor3000[q].ID.ToString();
                                row["Radius"] = ListforCvor3000[q].Radius.ToString();
                                row["Obstacle"] = ListforCvor3000[q].Obstacle.ToString();
                                row["Elevation"] = ListforCvor3000[q].Elevation.ToString();
                                row["Distance(meter)"] = Convert.ToInt32(ListforCvor3000[q].Distance).ToString();
                                row["Penetrate"] = ListforCvor3000[q].Penetrate.ToString();
                                row["GeoType"] = ListforCvor3000[q].TypeGeo.ToString();

                                table.Rows.Add(row);
                            }
                        }
                    }
                }
                if (Childnode.Text == "Radius for 15000")
                {



                    for (int i = 0; i < GridObstclData.Rows.Count; i++)
                    {
                        if (GridObstclData.Rows[i].Cells[3].Value.ToString() == null)
                            continue;
                        if (GridObstclData.Rows[i].Cells[3].Value.ToString() == "15000" && GridObstclData.Rows[i].Cells[1].Value.ToString() == Childnode.Parent.Text)
                        {
                            GridObstclData.Rows.RemoveAt(GridObstclData.Rows[i].Index);
                            i--;
                            //GridObstclData.Refresh();
                        }
                    }




                    if (ListforCvor15000.Count > 0)
                    {
                        for (int q = 0; q < ListforCvor15000.Count; q++)
                        {
                            if (ListforCvor15000[q].NavaidName == Childnode.Parent.Text)
                            {
                                DataRow row = table.NewRow();
                                row["TypeOfNavigation"] = ListforCvor15000[q].TypeOFNAvigation.ToString();
                                row["NavaidName"] = ListforCvor15000[q].NavaidName.ToString();
                                row["ID"] = ListforCvor15000[q].ID.ToString();
                                row["Radius"] = ListforCvor15000[q].Radius.ToString();
                                row["Obstacle"] = ListforCvor15000[q].Obstacle.ToString();
                                row["Elevation"] = ListforCvor15000[q].Elevation.ToString();
                                row["Distance(meter)"] = Convert.ToInt32(ListforCvor15000[q].Distance).ToString();
                                row["Penetrate"] = ListforCvor15000[q].Penetrate.ToString();
                                row["GeoType"] = ListforCvor15000[q].TypeGeo.ToString();

                                table.Rows.Add(row);
                            }
                        }
                    }
                }
                if (Childnode.Text == "Wind Turbine")
                {


                    for (int i = 0; i < GridObstclData.Rows.Count; i++)
                    {
                        if (GridObstclData.Rows[i].Cells[4].Value.ToString() == null)
                            continue;
                        if (GridObstclData.Rows[i].Cells[4].Value.ToString() == "Wind Turbine") // it will be fix soon
                        {
                            GridObstclData.Rows.RemoveAt(GridObstclData.Rows[i].Index);
                            i--;
                            //GridObstclData.Refresh();
                        }
                    }


                    if (ListforCvorForWindTurbine.Count > 0)
                    {
                        for (int i = 0; i < ListforCvorForWindTurbine.Count; i++)
                        {
                            DataRow row = table.NewRow();
                            row["TypeOfNavigation"] = ListforCvorForWindTurbine[i].TypeOfNavigation.ToString();
                            row["ID"] = "Empty";
                            row["Radius"] = "Empty";
                            row["Obstacle"] = ListforCvorForWindTurbine[i].Obstacle_name.ToString();
                            row["Elevation"] = "Empty";
                            row["Distance(meter)"] = "Empty";
                            row["Penetrate"] = ListforCvorForWindTurbine[i].Penetrate.ToString();
                            row["GeoType"] = "Empty";
                            row["NavaidName"] = ListforCvorForWindTurbine[i].NavaidName.ToString();
                            table.Rows.Add(row);
                        }
                    }
                }
            }
            if (secondparent.Text == "DMEN")
            {
                if (Childnode.Text == "Radius for 300")
                {


                    for (int i = 0; i < GridObstclData.Rows.Count; i++)
                    {
                        if (GridObstclData.Rows[i].Cells[3].Value.ToString() == null)
                            continue;
                        if (GridObstclData.Rows[i].Cells[3].Value.ToString() == "300" && GridObstclData.Rows[i].Cells[1].Value.ToString() == Childnode.Parent.Text)
                        {
                            GridObstclData.Rows.RemoveAt(GridObstclData.Rows[i].Index);
                            i--;
                            //GridObstclData.Refresh();
                        }
                    }




                    if (ListforDmen300.Count > 0)
                    {
                        for (int q = 0; q < ListforDmen300.Count; q++)
                        {
                            if (ListforDmen300[q].NavaidName == Childnode.Parent.Text)
                            {
                                DataRow row = table.NewRow();
                                row["TypeOfNavigation"] = ListforDmen300[q].TypeOFNAvigation.ToString();
                                row["NavaidName"] = ListforDmen300[q].NavaidName.ToString();
                                row["ID"] = ListforDmen300[q].ID.ToString();
                                row["Radius"] = ListforDmen300[q].Radius.ToString();
                                row["Obstacle"] = ListforDmen300[q].Obstacle.ToString();
                                row["Elevation"] = ListforDmen300[q].Elevation.ToString();
                                row["Distance(meter)"] = Convert.ToInt32(ListforDmen300[q].Distance).ToString();
                                row["Penetrate"] = ListforDmen300[q].Penetrate.ToString();
                                row["GeoType"] = ListforDmen300[q].TypeGeo.ToString();

                                table.Rows.Add(row);
                            }
                        }
                    }
                }
                if (Childnode.Text == "Radius for 3000")
                {


                    for (int i = 0; i < GridObstclData.Rows.Count; i++)
                    {
                        if (GridObstclData.Rows[i].Cells[3].Value.ToString() == null)
                            continue;
                        if (GridObstclData.Rows[i].Cells[3].Value.ToString() == "3000" && GridObstclData.Rows[i].Cells[1].Value.ToString() == Childnode.Parent.Text)
                        {
                            GridObstclData.Rows.RemoveAt(GridObstclData.Rows[i].Index);
                            i--;
                            //GridObstclData.Refresh();
                        }

                    }


                    if (ListforDmen3000.Count > 0)
                    {
                        for (int q = 0; q < ListforDmen3000.Count; q++)
                        {
                            if (ListforDmen3000[q].NavaidName == Childnode.Parent.Text)
                            {
                                DataRow row = table.NewRow();
                                row["TypeOfNavigation"] = ListforDmen3000[q].TypeOFNAvigation.ToString();
                                row["NavaidName"] = ListforDmen3000[q].NavaidName.ToString();
                                row["ID"] = ListforDmen3000[q].ID.ToString();
                                row["Radius"] = ListforDmen3000[q].Radius.ToString();
                                row["Obstacle"] = ListforDmen3000[q].Obstacle.ToString();
                                row["Elevation"] = ListforDmen3000[q].Elevation.ToString();
                                row["Distance(meter)"] = Convert.ToInt32(ListforDmen3000[q].Distance).ToString();
                                row["Penetrate"] = ListforDmen3000[q].Penetrate.ToString();
                                row["GeoType"] = ListforDmen3000[q].TypeGeo.ToString();

                                table.Rows.Add(row);
                            }
                        }
                    }
                }
            }

            GridObstclData.DataSource = table;

        }
        public void VORORDME_StepbyStep_Check_Show_ObstacleReportRemove(DataGridView GridObstclData, TreeNode Childnode, TreeNode secondparent)
        {

            try
            {
                if (secondparent.Text == "DVOR")
                {
                    if (Childnode.Text == "Radius for 600")
                    {
                        for (int i = 0; i < GridObstclData.Rows.Count; i++)
                        {
                            if (GridObstclData.Rows[i].Cells[3].Value.ToString() == null)
                                continue;
                            if (GridObstclData.Rows[i].Cells[3].Value.ToString() == "600" && GridObstclData.Rows[i].Cells[1].Value.ToString() == Childnode.Parent.Text)
                            {
                                GridObstclData.Rows.RemoveAt(GridObstclData.Rows[i].Index);
                                i--;
                                //GridObstclData.Refresh();
                            }
                        }
                    }
                    if (Childnode.Text == "Radius for 3000")
                    {
                        for (int i = 0; i < GridObstclData.Rows.Count; i++)
                        {
                            if (GridObstclData.Rows[i].Cells[3].Value.ToString() == null)
                                continue;
                            if (GridObstclData.Rows[i].Cells[3].Value.ToString() == "3000" && GridObstclData.Rows[i].Cells[1].Value.ToString() == Childnode.Parent.Text)
                            {
                                GridObstclData.Rows.RemoveAt(GridObstclData.Rows[i].Index);
                                i--;
                                //GridObstclData.Refresh();
                            }
                        }
                    }
                    if (Childnode.Text == "Radius for 10000")
                    {
                        for (int i = 0; i < GridObstclData.Rows.Count; i++)
                        {
                            if (GridObstclData.Rows[i].Cells[3].Value.ToString() == null)
                                continue;
                            if (GridObstclData.Rows[i].Cells[3].Value.ToString() == "10000" && GridObstclData.Rows[i].Cells[1].Value.ToString() == Childnode.Parent.Text)
                            {
                                GridObstclData.Rows.RemoveAt(GridObstclData.Rows[i].Index);
                                i--;
                                //GridObstclData.Refresh();
                            }
                        }
                    }
                    if (Childnode.Text == "Wind Turbine")
                    {
                        for (int i = 0; i < GridObstclData.Rows.Count; i++)
                        {
                            if (GridObstclData.Rows[i].Cells[1].Value.ToString() == null)
                                continue;
                            if (GridObstclData.Rows[i].Cells[4].Value.ToString() == "Wind Turbine") // it will be fix soon
                            {
                                GridObstclData.Rows.RemoveAt(GridObstclData.Rows[i].Index);
                                i--;
                                //GridObstclData.Refresh();
                            }
                        }
                    }
                }
                if (secondparent.Text == "CVOR")
                {
                    if (Childnode.Text == "Radius for 600")
                    {
                        for (int i = 0; i < GridObstclData.Rows.Count; i++)
                        {
                            if (GridObstclData.Rows[i].Cells[3].Value.ToString() == null)
                                continue;
                            if (GridObstclData.Rows[i].Cells[3].Value.ToString() == "600" && GridObstclData.Rows[i].Cells[1].Value.ToString() == Childnode.Parent.Text)
                            {
                                GridObstclData.Rows.RemoveAt(GridObstclData.Rows[i].Index);
                                i--;
                                //GridObstclData.Refresh();
                            }
                        }
                    }
                    if (Childnode.Text == "Radius for 3000")
                    {
                        for (int i = 0; i < GridObstclData.Rows.Count; i++)
                        {
                            if (GridObstclData.Rows[i].Cells[3].Value.ToString() == null)
                                continue;
                            if (GridObstclData.Rows[i].Cells[3].Value.ToString() == "3000" && GridObstclData.Rows[i].Cells[1].Value.ToString() == Childnode.Parent.Text)
                            {
                                GridObstclData.Rows.RemoveAt(GridObstclData.Rows[i].Index);
                                i--;
                                //GridObstclData.Refresh();
                            }
                        }
                    }
                    if (Childnode.Text == "Radius for 15000")
                    {
                        for (int i = 0; i < GridObstclData.Rows.Count; i++)
                        {
                            if (GridObstclData.Rows[i].Cells[3].Value.ToString() == null)
                                continue;
                            if (GridObstclData.Rows[i].Cells[3].Value.ToString() == "15000" && GridObstclData.Rows[i].Cells[1].Value.ToString() == Childnode.Parent.Text)
                            {
                                GridObstclData.Rows.RemoveAt(GridObstclData.Rows[i].Index);
                                i--;
                                //GridObstclData.Refresh();
                            }
                        }
                    }
                    if (Childnode.Text == "Wind Turbine")
                    {
                        for (int i = 0; i < GridObstclData.Rows.Count; i++)
                        {
                            if (GridObstclData.Rows[i].Cells[4].Value.ToString() == null)
                                continue;
                            if (GridObstclData.Rows[i].Cells[4].Value.ToString() == "Wind Turbine") // it will be fix soon
                            {
                                GridObstclData.Rows.RemoveAt(GridObstclData.Rows[i].Index);
                                i--;
                                //GridObstclData.Refresh();
                            }
                        }
                    }
                }
                if (secondparent.Text == "DMEN")
                {
                    if (Childnode.Text == "Radius for 300")
                    {
                        for (int i = 0; i < GridObstclData.Rows.Count; i++)
                        {
                            if (GridObstclData.Rows[i].Cells[3].Value.ToString() == null)
                                continue;
                            if (GridObstclData.Rows[i].Cells[3].Value.ToString() == "300" && GridObstclData.Rows[i].Cells[1].Value.ToString() == Childnode.Parent.Text)
                            {
                                GridObstclData.Rows.RemoveAt(GridObstclData.Rows[i].Index);
                                i--;
                                //GridObstclData.Refresh();
                            }
                        }
                    }
                    if (Childnode.Text == "Radius for 3000")
                    {
                        for (int i = 0; i < GridObstclData.Rows.Count; i++)
                        {
                            if (GridObstclData.Rows[i].Cells[3].Value.ToString() == null)
                                continue;
                            if (GridObstclData.Rows[i].Cells[3].Value.ToString() == "3000" && GridObstclData.Rows[i].Cells[1].Value.ToString() == Childnode.Parent.Text)
                            {
                                GridObstclData.Rows.RemoveAt(GridObstclData.Rows[i].Index);
                                i--;
                                //GridObstclData.Refresh();
                            }
                        }
                    }
                }

                //GridObstclData.DataSource = GridObstclData.DataBindings;
                //GridObstclData.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
        public void NDB_OR_Markers_Check_Show_ObstacleReportADD(DataGridView GridObstclData, DataTable table, TreeNode Childnode, TreeNode FirstParent, TreeNode SecondParent, List<ReportForMarkers50r> ListforMarker50, List<ReportForMarkers200r> ListforMarker200, List<ReportForNDB200r> ListforNDB200, List<ReportForNDB1000r> ListforNDB1000)
        {
            if (FirstParent.Text == "Markers")
            {
                if (Childnode.Text == "Radius for 50")
                {



                    for (int i = 0; i < GridObstclData.Rows.Count; i++)
                    {
                        if (GridObstclData.Rows[i].Cells[3].Value.ToString() == null)
                            continue;
                        if (GridObstclData.Rows[i].Cells[3].Value.ToString() == "50" && GridObstclData.Rows[i].Cells[1].Value.ToString() == SecondParent.Text && GridObstclData.Rows[i].Cells[0].Value.ToString() == FirstParent.Text)
                        {
                            GridObstclData.Rows.RemoveAt(GridObstclData.Rows[i].Index);
                            i--;
                            //GridObstclData.Refresh();
                        }
                    }


                    for (int q = 0; q < ListforMarker50.Count; q++)
                    {
                        if (ListforMarker50[q].NavaidName == SecondParent.Text)
                        {
                            DataRow row = table.NewRow();
                            row["TypeOfNavigation"] = ListforMarker50[q].TypeOFNAvigation.ToString();
                            row["NavaidName"] = ListforMarker50[q].NavaidName.ToString();
                            row["ID"] = ListforMarker50[q].ID.ToString();
                            row["Radius"] = ListforMarker50[q].Radius.ToString();
                            row["Obstacle"] = ListforMarker50[q].Obstacle.ToString();
                            row["Elevation"] = ListforMarker50[q].Elevation.ToString();
                            row["Distance(meter)"] = Convert.ToInt32(ListforMarker50[q].Distance).ToString();
                            row["Penetrate"] = ListforMarker50[q].Penetrate.ToString();
                            row["GeoType"] = ListforMarker50[q].TypeGeo.ToString();

                            table.Rows.Add(row);
                        }
                    }
                }
                if (Childnode.Text == "Radius for 200")
                {

                    for (int i = 0; i < GridObstclData.Rows.Count; i++)
                    {
                        if (GridObstclData.Rows[i].Cells[3].Value.ToString() == null)
                            continue;
                        if (GridObstclData.Rows[i].Cells[3].Value.ToString() == "200" && GridObstclData.Rows[i].Cells[1].Value.ToString() == SecondParent.Text && GridObstclData.Rows[i].Cells[0].Value.ToString() == FirstParent.Text)
                        {
                            GridObstclData.Rows.RemoveAt(GridObstclData.Rows[i].Index);
                            i--;
                            //GridObstclData.Refresh();
                        }
                    }

                    for (int q = 0; q < ListforMarker200.Count; q++)
                    {
                        if (ListforMarker200[q].NavaidName == SecondParent.Text)
                        {
                            DataRow row = table.NewRow();
                            row["TypeOfNavigation"] = ListforMarker200[q].TypeOFNAvigation.ToString();
                            row["NavaidName"] = ListforMarker200[q].NavaidName.ToString();
                            row["ID"] = ListforMarker200[q].ID.ToString();
                            row["Radius"] = ListforMarker200[q].Radius.ToString();
                            row["Obstacle"] = ListforMarker200[q].Obstacle.ToString();
                            row["Elevation"] = ListforMarker200[q].Elevation.ToString();
                            row["Distance(meter)"] = Convert.ToInt32(ListforMarker200[q].Distance).ToString();
                            row["Penetrate"] = ListforMarker200[q].Penetrate.ToString();
                            row["GeoType"] = ListforMarker200[q].TypeGeo.ToString();

                            table.Rows.Add(row);
                        }
                    }
                }
            }
            if (FirstParent.Text == "NDB")
            {
                if (Childnode.Text == "Radius for 200")
                {


                    for (int i = 0; i < GridObstclData.Rows.Count; i++)
                    {
                        if (GridObstclData.Rows[i].Cells[3].Value.ToString() == null)
                            continue;
                        if (GridObstclData.Rows[i].Cells[3].Value.ToString() == "200" && GridObstclData.Rows[i].Cells[1].Value.ToString() == SecondParent.Text && GridObstclData.Rows[i].Cells[0].Value.ToString() == FirstParent.Text)
                        {
                            GridObstclData.Rows.RemoveAt(GridObstclData.Rows[i].Index);
                            i--;
                            //GridObstclData.Refresh();
                        }
                    }





                    for (int q = 0; q < ListforNDB200.Count; q++)
                    {
                        if (ListforNDB200[q].NavaidName == SecondParent.Text)
                        {
                            DataRow row = table.NewRow();
                            row["TypeOfNavigation"] = ListforNDB200[q].TypeOFNAvigation.ToString();
                            row["NavaidName"] = ListforNDB200[q].NavaidName.ToString();
                            row["ID"] = ListforNDB200[q].ID.ToString();
                            row["Radius"] = ListforNDB200[q].Radius.ToString();
                            row["Obstacle"] = ListforNDB200[q].Obstacle.ToString();
                            row["Elevation"] = ListforNDB200[q].Elevation.ToString();
                            row["Distance(meter)"] = Convert.ToInt32(ListforNDB200[q].Distance).ToString();
                            row["Penetrate"] = ListforNDB200[q].Penetrate.ToString();
                            row["GeoType"] = ListforNDB200[q].TypeGeo.ToString();

                            table.Rows.Add(row);
                        }
                    }
                }
                if (Childnode.Text == "Radius for 1000")
                {

                    for (int i = 0; i < GridObstclData.Rows.Count; i++)
                    {
                        if (GridObstclData.Rows[i].Cells[3].Value.ToString() == null)
                            continue;
                        if (GridObstclData.Rows[i].Cells[3].Value.ToString() == "1000" && GridObstclData.Rows[i].Cells[1].Value.ToString() == SecondParent.Text && GridObstclData.Rows[i].Cells[0].Value.ToString() == FirstParent.Text)
                        {
                            GridObstclData.Rows.RemoveAt(GridObstclData.Rows[i].Index);
                            i--;
                            //GridObstclData.Refresh();
                        }
                    }

                    for (int q = 0; q < ListforNDB1000.Count; q++)
                    {
                        if (ListforNDB1000[q].NavaidName == SecondParent.Text)
                        {
                            DataRow row = table.NewRow();
                            row["TypeOfNavigation"] = ListforNDB1000[q].TypeOFNAvigation.ToString();
                            row["NavaidName"] = ListforNDB1000[q].NavaidName.ToString();
                            row["ID"] = ListforNDB1000[q].ID.ToString();
                            row["Radius"] = ListforNDB1000[q].Radius.ToString();
                            row["Obstacle"] = ListforNDB1000[q].Obstacle.ToString();
                            row["Elevation"] = ListforNDB1000[q].Elevation.ToString();
                            row["Distance(meter)"] = Convert.ToInt32(ListforNDB1000[q].Distance).ToString();
                            row["Penetrate"] = ListforNDB1000[q].Penetrate.ToString();
                            row["GeoType"] = ListforNDB1000[q].TypeGeo.ToString();

                            table.Rows.Add(row);
                        }
                    }
                }
            }

            GridObstclData.DataSource = table;
        }
        public void NDB_OR_Markers_Check_Remove_FromGrid(DataGridView GridObstclData, TreeNode enode, TreeNode SecondParent, TreeNode firstdparent)
        {
            if (firstdparent.Text == "Markers")
            {
                if (enode.Text == "Radius for 50")
                {
                    for (int i = 0; i < GridObstclData.Rows.Count; i++)
                    {
                        if (GridObstclData.Rows[i].Cells[3].Value.ToString() == null)
                            continue;
                        if (GridObstclData.Rows[i].Cells[3].Value.ToString() == "50" && GridObstclData.Rows[i].Cells[1].Value.ToString() == SecondParent.Text && GridObstclData.Rows[i].Cells[0].Value.ToString() == firstdparent.Text)
                        {
                            GridObstclData.Rows.RemoveAt(GridObstclData.Rows[i].Index);
                            i--;
                            //GridObstclData.Refresh();
                        }
                    }
                }
                if (enode.Text == "Radius for 200")
                {
                    for (int i = 0; i < GridObstclData.Rows.Count; i++)
                    {
                        if (GridObstclData.Rows[i].Cells[3].Value.ToString() == null)
                            continue;
                        if (GridObstclData.Rows[i].Cells[3].Value.ToString() == "200" && GridObstclData.Rows[i].Cells[1].Value.ToString() == SecondParent.Text && GridObstclData.Rows[i].Cells[0].Value.ToString() == firstdparent.Text)
                        {
                            GridObstclData.Rows.RemoveAt(GridObstclData.Rows[i].Index);
                            i--;
                            //GridObstclData.Refresh();
                        }
                    }
                }
            }
            if (firstdparent.Text == "NDB")
            {
                if (enode.Text == "Radius for 200")
                {
                    for (int i = 0; i < GridObstclData.Rows.Count; i++)
                    {
                        if (GridObstclData.Rows[i].Cells[3].Value.ToString() == null)
                            continue;
                        if (GridObstclData.Rows[i].Cells[3].Value.ToString() == "200" && GridObstclData.Rows[i].Cells[1].Value.ToString() == SecondParent.Text && GridObstclData.Rows[i].Cells[0].Value.ToString() == firstdparent.Text)
                        {
                            GridObstclData.Rows.RemoveAt(GridObstclData.Rows[i].Index);
                            i--;
                            //GridObstclData.Refresh();
                        }
                    }
                }
                if (enode.Text == "Radius for 1000")
                {
                    for (int i = 0; i < GridObstclData.Rows.Count; i++)
                    {
                        if (GridObstclData.Rows[i].Cells[3].Value.ToString() == null)
                            continue;
                        if (GridObstclData.Rows[i].Cells[3].Value.ToString() == "1000" && GridObstclData.Rows[i].Cells[1].Value.ToString() == SecondParent.Text && GridObstclData.Rows[i].Cells[0].Value.ToString() == firstdparent.Text)
                        {
                            GridObstclData.Rows.RemoveAt(GridObstclData.Rows[i].Index);
                            i--;
                            //GridObstclData.Refresh();
                        }
                    }
                }

            }
        }

        public void ILS_StepbyStep_Check_Show_ObstacleReportAdd(DataGridView GridObstclData, DataTable table, TreeNode Childnode, TreeNode SecondParent, TreeNode Thirdparent, List<Lists_FOR_2DGraphics> List2DGraphic)
        {
            if (SecondParent.Text == "GP")
            {
                if (Childnode.Text == "Polygon Segment")
                {
                    for (int r = 0; r < GridObstclData.Rows.Count; r++)
                    {
                        if (GridObstclData.Rows[r].Cells[3].Value.ToString() == null)
                            continue;
                        if (GridObstclData.Rows[r].Cells[8].Value.ToString() == "Polygon Segment" && GridObstclData.Rows[r].Cells[1].Value.ToString() == Thirdparent.Text && GridObstclData.Rows[r].Cells[0].Value.ToString() == SecondParent.Text)
                        {
                            GridObstclData.Rows.RemoveAt(GridObstclData.Rows[r].Index);
                            r--;
                            //GridObstclData.Refresh();
                        }
                    }
                }
                if (Childnode.Text == "First Corner Polygon")
                {
                    for (int r = 0; r < GridObstclData.Rows.Count; r++)
                    {
                        if (GridObstclData.Rows[r].Cells[3].Value.ToString() == null)
                            continue;
                        if (GridObstclData.Rows[r].Cells[8].Value.ToString() == "First Corner Polygon" && GridObstclData.Rows[r].Cells[1].Value.ToString() == Thirdparent.Text && GridObstclData.Rows[r].Cells[0].Value.ToString() == SecondParent.Text)
                        {
                            GridObstclData.Rows.RemoveAt(GridObstclData.Rows[r].Index);
                            r--;
                            //GridObstclData.Refresh();
                        }
                    }
                }
                if (Childnode.Text == "Second Corner Polygon")
                {
                    for (int r = 0; r < GridObstclData.Rows.Count; r++)
                    {
                        if (GridObstclData.Rows[r].Cells[3].Value.ToString() == null)
                            continue;
                        if (GridObstclData.Rows[r].Cells[8].Value.ToString() == "Second Corner Polygon" && GridObstclData.Rows[r].Cells[1].Value.ToString() == Thirdparent.Text && GridObstclData.Rows[r].Cells[0].Value.ToString() == SecondParent.Text)
                        {
                            GridObstclData.Rows.RemoveAt(GridObstclData.Rows[r].Index);
                            r--;
                            //GridObstclData.Refresh();
                        }
                    }
                }
                for (int i = 0; i < List2DGraphic.Count; i++)
                {
                    if (List2DGraphic[i].ParentTxt == SecondParent.Text && List2DGraphic[i].ChildTxt == Thirdparent.Text)
                    {
                        if (Childnode.Text == "Polygon Segment")
                        {

                            if (List2DGraphic[i].Polygon_Type == "Polygon Segment")
                            {
                                DataRow row = table.NewRow();
                                row["TypeOFNAvigation"] = List2DGraphic[i].ParentTxt;
                                row["NavaidName"] = List2DGraphic[i].ChildTxt;
                                row["ID"] = List2DGraphic[i].ID;
                                row["Obstacle"] = List2DGraphic[i].Obstacle;
                                row["Elevation"] = List2DGraphic[i].Elevation;
                                row["Distance(meter)"] = List2DGraphic[i].Distance;
                                row["Penetrate"] = List2DGraphic[i].Penetrate;
                                row["GeoType"] = List2DGraphic[i].TypeGeo;
                                row["PolygonType"] = List2DGraphic[i].Polygon_Type;
                                table.Rows.Add(row);

                            }
                        }
                        if (Childnode.Text == "First Corner Polygon")
                        {

                            if (List2DGraphic[i].Polygon_Type == "First Corner Polygon")
                            {


                                DataRow row = table.NewRow();
                                row["TypeOFNAvigation"] = List2DGraphic[i].ParentTxt;
                                row["NavaidName"] = List2DGraphic[i].ChildTxt;
                                row["ID"] = List2DGraphic[i].ID;
                                row["Obstacle"] = List2DGraphic[i].Obstacle;
                                row["Elevation"] = List2DGraphic[i].Elevation;
                                row["Distance(meter)"] = List2DGraphic[i].Distance;
                                row["Penetrate"] = List2DGraphic[i].Penetrate;
                                row["GeoType"] = List2DGraphic[i].TypeGeo;
                                row["PolygonType"] = List2DGraphic[i].Polygon_Type;
                                table.Rows.Add(row);

                            }
                        }
                        if (Childnode.Text == "Second Corner Polygon")
                        {




                            if (List2DGraphic[i].Polygon_Type == "Second Corner Polygon")
                            {


                                DataRow row = table.NewRow();
                                row["TypeOFNAvigation"] = List2DGraphic[i].ParentTxt;
                                row["NavaidName"] = List2DGraphic[i].ChildTxt;
                                row["ID"] = List2DGraphic[i].ID;
                                row["Obstacle"] = List2DGraphic[i].Obstacle;
                                row["Elevation"] = List2DGraphic[i].Elevation;
                                row["Distance(meter)"] = List2DGraphic[i].Distance;
                                row["Penetrate"] = List2DGraphic[i].Penetrate;
                                row["GeoType"] = List2DGraphic[i].TypeGeo;
                                row["PolygonType"] = List2DGraphic[i].Polygon_Type;
                                table.Rows.Add(row);

                            }
                        }
                    }
                }
            }

            if (SecondParent.Text == "LOC(Single Frequency)")
            {


                if (Childnode.Text == "Polygon Segment")
                {
                    for (int r = 0; r < GridObstclData.Rows.Count; r++)
                    {
                        if (GridObstclData.Rows[r].Cells[3].Value.ToString() == null)
                            continue;
                        if (GridObstclData.Rows[r].Cells[8].Value.ToString() == "Polygon Segment" && GridObstclData.Rows[r].Cells[1].Value.ToString() == Thirdparent.Text && GridObstclData.Rows[r].Cells[0].Value.ToString() == SecondParent.Text)
                        {
                            GridObstclData.Rows.RemoveAt(GridObstclData.Rows[r].Index);
                            r--;
                            //GridObstclData.Refresh();
                        }
                    }
                }
                if (Childnode.Text == "First Corner Polygon")
                {
                    for (int r = 0; r < GridObstclData.Rows.Count; r++)
                    {
                        if (GridObstclData.Rows[r].Cells[3].Value.ToString() == null)
                            continue;
                        if (GridObstclData.Rows[r].Cells[8].Value.ToString() == "First Corner Polygon" && GridObstclData.Rows[r].Cells[1].Value.ToString() == Thirdparent.Text && GridObstclData.Rows[r].Cells[0].Value.ToString() == SecondParent.Text)
                        {
                            GridObstclData.Rows.RemoveAt(GridObstclData.Rows[r].Index);
                            r--;
                            //GridObstclData.Refresh();
                        }
                    }
                }
                if (Childnode.Text == "Second Corner Polygon")
                {
                    for (int r = 0; r < GridObstclData.Rows.Count; r++)
                    {
                        if (GridObstclData.Rows[r].Cells[3].Value.ToString() == null)
                            continue;
                        if (GridObstclData.Rows[r].Cells[8].Value.ToString() == "Second Corner Polygon" && GridObstclData.Rows[r].Cells[1].Value.ToString() == Thirdparent.Text && GridObstclData.Rows[r].Cells[0].Value.ToString() == SecondParent.Text)
                        {
                            GridObstclData.Rows.RemoveAt(GridObstclData.Rows[r].Index);
                            r--;
                            //GridObstclData.Refresh();
                        }
                    }
                }

                for (int i = 0; i < List2DGraphic.Count; i++)
                {
                    if (List2DGraphic[i].ParentTxt == SecondParent.Text && List2DGraphic[i].ChildTxt == Thirdparent.Text)
                    {
                        if (Childnode.Text == "Polygon Segment")
                        {
                            if (List2DGraphic[i].Polygon_Type == "Polygon Segment")
                            {
                                DataRow row = table.NewRow();
                                row["TypeOFNAvigation"] = List2DGraphic[i].ParentTxt;
                                row["NavaidName"] = List2DGraphic[i].ChildTxt;
                                row["ID"] = List2DGraphic[i].ID;
                                row["Obstacle"] = List2DGraphic[i].Obstacle;
                                row["Elevation"] = List2DGraphic[i].Elevation;
                                row["Distance(meter)"] = List2DGraphic[i].Distance;
                                row["Penetrate"] = List2DGraphic[i].Penetrate;
                                row["GeoType"] = List2DGraphic[i].TypeGeo;
                                row["PolygonType"] = List2DGraphic[i].Polygon_Type;
                                table.Rows.Add(row);

                            }
                        }
                        if (Childnode.Text == "First Corner Polygon")
                        {
                            if (List2DGraphic[i].Polygon_Type == "First Corner Polygon")
                            {
                                DataRow row = table.NewRow();
                                row["TypeOFNAvigation"] = List2DGraphic[i].ParentTxt;
                                row["NavaidName"] = List2DGraphic[i].ChildTxt;
                                row["ID"] = List2DGraphic[i].ID;
                                row["Obstacle"] = List2DGraphic[i].Obstacle;
                                row["Elevation"] = List2DGraphic[i].Elevation;
                                row["Distance(meter)"] = List2DGraphic[i].Distance;
                                row["Penetrate"] = List2DGraphic[i].Penetrate;
                                row["GeoType"] = List2DGraphic[i].TypeGeo;
                                row["PolygonType"] = List2DGraphic[i].Polygon_Type;
                                table.Rows.Add(row);

                            }
                        }
                        if (Childnode.Text == "Second Corner Polygon")
                        {
                            if (List2DGraphic[i].Polygon_Type == "Second Corner Polygon")
                            {
                                DataRow row = table.NewRow();
                                row["TypeOFNAvigation"] = List2DGraphic[i].ParentTxt;
                                row["NavaidName"] = List2DGraphic[i].ChildTxt;
                                row["ID"] = List2DGraphic[i].ID;
                                row["Obstacle"] = List2DGraphic[i].Obstacle;
                                row["Elevation"] = List2DGraphic[i].Elevation;
                                row["Distance(meter)"] = List2DGraphic[i].Distance;
                                row["Penetrate"] = List2DGraphic[i].Penetrate;
                                row["GeoType"] = List2DGraphic[i].TypeGeo;
                                row["PolygonType"] = List2DGraphic[i].Polygon_Type;
                                table.Rows.Add(row);

                            }
                        }
                    }
                }
            }

            if (SecondParent.Text == "LOC(Dual Frequency)")
            {


                if (Childnode.Text == "Polygon Segment")
                {
                    for (int r = 0; r < GridObstclData.Rows.Count; r++)
                    {
                        if (GridObstclData.Rows[r].Cells[3].Value.ToString() == null)
                            continue;
                        if (GridObstclData.Rows[r].Cells[8].Value.ToString() == "Polygon Segment" && GridObstclData.Rows[r].Cells[1].Value.ToString() == Thirdparent.Text && GridObstclData.Rows[r].Cells[0].Value.ToString() == SecondParent.Text)
                        {
                            GridObstclData.Rows.RemoveAt(GridObstclData.Rows[r].Index);
                            r--;
                            //GridObstclData.Refresh();
                        }
                    }
                }
                if (Childnode.Text == "First Corner Polygon")
                {
                    for (int r = 0; r < GridObstclData.Rows.Count; r++)
                    {
                        if (GridObstclData.Rows[r].Cells[3].Value.ToString() == null)
                            continue;
                        if (GridObstclData.Rows[r].Cells[8].Value.ToString() == "First Corner Polygon" && GridObstclData.Rows[r].Cells[1].Value.ToString() == Thirdparent.Text && GridObstclData.Rows[r].Cells[0].Value.ToString() == SecondParent.Text)
                        {
                            GridObstclData.Rows.RemoveAt(GridObstclData.Rows[r].Index);
                            r--;
                            //GridObstclData.Refresh();
                        }
                    }
                }
                if (Childnode.Text == "Second Corner Polygon")
                {
                    for (int r = 0; r < GridObstclData.Rows.Count; r++)
                    {
                        if (GridObstclData.Rows[r].Cells[3].Value.ToString() == null)
                            continue;
                        if (GridObstclData.Rows[r].Cells[8].Value.ToString() == "Second Corner Polygon" && GridObstclData.Rows[r].Cells[1].Value.ToString() == Thirdparent.Text && GridObstclData.Rows[r].Cells[0].Value.ToString() == SecondParent.Text)
                        {
                            GridObstclData.Rows.RemoveAt(GridObstclData.Rows[r].Index);
                            r--;
                            //GridObstclData.Refresh();
                        }
                    }
                }

                for (int i = 0; i < List2DGraphic.Count; i++)
                {
                    if (List2DGraphic[i].ParentTxt == SecondParent.Text && List2DGraphic[i].ChildTxt == Thirdparent.Text)
                    {
                        if (Childnode.Text == "Polygon Segment")
                        {
                            if (List2DGraphic[i].Polygon_Type == "Polygon Segment")
                            {
                                DataRow row = table.NewRow();
                                row["TypeOFNAvigation"] = List2DGraphic[i].ParentTxt;
                                row["NavaidName"] = List2DGraphic[i].ChildTxt;
                                row["ID"] = List2DGraphic[i].ID;
                                row["Obstacle"] = List2DGraphic[i].Obstacle;
                                row["Elevation"] = List2DGraphic[i].Elevation;
                                row["Distance(meter)"] = List2DGraphic[i].Distance;
                                row["Penetrate"] = List2DGraphic[i].Penetrate;
                                row["GeoType"] = List2DGraphic[i].TypeGeo;
                                row["PolygonType"] = List2DGraphic[i].Polygon_Type;
                                table.Rows.Add(row);

                            }
                        }
                        if (Childnode.Text == "First Corner Polygon")
                        {
                            if (List2DGraphic[i].Polygon_Type == "First Corner Polygon")
                            {
                                DataRow row = table.NewRow();
                                row["TypeOFNAvigation"] = List2DGraphic[i].ParentTxt;
                                row["NavaidName"] = List2DGraphic[i].ChildTxt;
                                row["ID"] = List2DGraphic[i].ID;
                                row["Obstacle"] = List2DGraphic[i].Obstacle;
                                row["Elevation"] = List2DGraphic[i].Elevation;
                                row["Distance(meter)"] = List2DGraphic[i].Distance;
                                row["Penetrate"] = List2DGraphic[i].Penetrate;
                                row["GeoType"] = List2DGraphic[i].TypeGeo;
                                row["PolygonType"] = List2DGraphic[i].Polygon_Type;
                                table.Rows.Add(row);

                            }
                        }
                        if (Childnode.Text == "Second Corner Polygon")
                        {
                            if (List2DGraphic[i].Polygon_Type == "Second Corner Polygon")
                            {
                                DataRow row = table.NewRow();
                                row["TypeOFNAvigation"] = List2DGraphic[i].ParentTxt;
                                row["NavaidName"] = List2DGraphic[i].ChildTxt;
                                row["ID"] = List2DGraphic[i].ID;
                                row["Obstacle"] = List2DGraphic[i].Obstacle;
                                row["Elevation"] = List2DGraphic[i].Elevation;
                                row["Distance(meter)"] = List2DGraphic[i].Distance;
                                row["Penetrate"] = List2DGraphic[i].Penetrate;
                                row["GeoType"] = List2DGraphic[i].TypeGeo;
                                row["PolygonType"] = List2DGraphic[i].Polygon_Type;
                                table.Rows.Add(row);

                            }
                        }
                    }
                }
            }
            if (SecondParent.Text == "DME")
            {


                if (Childnode.Text == "Polygon Segment")
                {
                    for (int r = 0; r < GridObstclData.Rows.Count; r++)
                    {
                        if (GridObstclData.Rows[r].Cells[3].Value.ToString() == null)
                            continue;
                        if (GridObstclData.Rows[r].Cells[8].Value.ToString() == "Polygon Segment" && GridObstclData.Rows[r].Cells[1].Value.ToString() == Thirdparent.Text && GridObstclData.Rows[r].Cells[0].Value.ToString() == SecondParent.Text)
                        {
                            GridObstclData.Rows.RemoveAt(GridObstclData.Rows[r].Index);
                            r--;
                            //GridObstclData.Refresh();
                        }
                    }
                }
                if (Childnode.Text == "First Corner Polygon")
                {
                    for (int r = 0; r < GridObstclData.Rows.Count; r++)
                    {
                        if (GridObstclData.Rows[r].Cells[3].Value.ToString() == null)
                            continue;
                        if (GridObstclData.Rows[r].Cells[8].Value.ToString() == "First Corner Polygon" && GridObstclData.Rows[r].Cells[1].Value.ToString() == Thirdparent.Text && GridObstclData.Rows[r].Cells[0].Value.ToString() == SecondParent.Text)
                        {
                            GridObstclData.Rows.RemoveAt(GridObstclData.Rows[r].Index);
                            r--;
                            //GridObstclData.Refresh();
                        }
                    }
                }
                if (Childnode.Text == "Second Corner Polygon")
                {
                    for (int r = 0; r < GridObstclData.Rows.Count; r++)
                    {
                        if (GridObstclData.Rows[r].Cells[3].Value.ToString() == null)
                            continue;
                        if (GridObstclData.Rows[r].Cells[8].Value.ToString() == "Second Corner Polygon" && GridObstclData.Rows[r].Cells[1].Value.ToString() == Thirdparent.Text && GridObstclData.Rows[r].Cells[0].Value.ToString() == SecondParent.Text)
                        {
                            GridObstclData.Rows.RemoveAt(GridObstclData.Rows[r].Index);
                            r--;
                            //GridObstclData.Refresh();
                        }
                    }
                }

                for (int i = 0; i < List2DGraphic.Count; i++)
                {
                    if (List2DGraphic[i].ParentTxt == SecondParent.Text && List2DGraphic[i].ChildTxt == Thirdparent.Text)
                    {
                        if (Childnode.Text == "Polygon Segment")
                        {





                            if (List2DGraphic[i].Polygon_Type == "Polygon Segment")
                            {
                                DataRow row = table.NewRow();
                                row["TypeOFNAvigation"] = List2DGraphic[i].ParentTxt;
                                row["NavaidName"] = List2DGraphic[i].ChildTxt;
                                row["ID"] = List2DGraphic[i].ID;
                                row["Obstacle"] = List2DGraphic[i].Obstacle;
                                row["Elevation"] = List2DGraphic[i].Elevation;
                                row["Distance(meter)"] = List2DGraphic[i].Distance;
                                row["Penetrate"] = List2DGraphic[i].Penetrate;
                                row["GeoType"] = List2DGraphic[i].TypeGeo;
                                row["PolygonType"] = List2DGraphic[i].Polygon_Type;
                                table.Rows.Add(row);

                            }
                        }
                        if (Childnode.Text == "First Corner Polygon")
                        {




                            if (List2DGraphic[i].Polygon_Type == "First Corner Polygon")
                            {
                                DataRow row = table.NewRow();
                                row["TypeOFNAvigation"] = List2DGraphic[i].ParentTxt;
                                row["NavaidName"] = List2DGraphic[i].ChildTxt;
                                row["ID"] = List2DGraphic[i].ID;
                                row["Obstacle"] = List2DGraphic[i].Obstacle;
                                row["Elevation"] = List2DGraphic[i].Elevation;
                                row["Distance(meter)"] = List2DGraphic[i].Distance;
                                row["Penetrate"] = List2DGraphic[i].Penetrate;
                                row["GeoType"] = List2DGraphic[i].TypeGeo;
                                row["PolygonType"] = List2DGraphic[i].Polygon_Type;
                                table.Rows.Add(row);

                            }
                        }
                        if (Childnode.Text == "Second Corner Polygon")
                        {




                            if (List2DGraphic[i].Polygon_Type == "Second Corner Polygon")
                            {
                                DataRow row = table.NewRow();
                                row["TypeOFNAvigation"] = List2DGraphic[i].ParentTxt;
                                row["NavaidName"] = List2DGraphic[i].ChildTxt;
                                row["ID"] = List2DGraphic[i].ID;
                                row["Obstacle"] = List2DGraphic[i].Obstacle;
                                row["Elevation"] = List2DGraphic[i].Elevation;
                                row["Distance(meter)"] = List2DGraphic[i].Distance;
                                row["Penetrate"] = List2DGraphic[i].Penetrate;
                                row["GeoType"] = List2DGraphic[i].TypeGeo;
                                row["PolygonType"] = List2DGraphic[i].Polygon_Type;
                                table.Rows.Add(row);

                            }
                        }
                    }
                }
            }
            GridObstclData.DataSource = table;


        }
        public void ILS_StepbyStep_Check_Remove_From_Grid(DataGridView GridObstclData, TreeNode enode, TreeNode SecondParent, TreeNode thirdparent)
        {
            if (SecondParent.Text == "DME")
            {
                if (enode.Text == "Second Corner Polygon")
                {
                    for (int i = 0; i < GridObstclData.Rows.Count; i++)
                    {
                        if (GridObstclData.Rows[i].Cells[3].Value.ToString() == null)
                            continue;
                        if (GridObstclData.Rows[i].Cells[8].Value.ToString() == "Second Corner Polygon" && GridObstclData.Rows[i].Cells[1].Value.ToString() == thirdparent.Text && GridObstclData.Rows[i].Cells[0].Value.ToString() == SecondParent.Text)
                        {
                            GridObstclData.Rows.RemoveAt(GridObstclData.Rows[i].Index);
                            i--;
                            //GridObstclData.Refresh();
                        }
                    }

                }
                if (enode.Text == "First Corner Polygon")
                {
                    for (int i = 0; i < GridObstclData.Rows.Count; i++)
                    {
                        if (GridObstclData.Rows[i].Cells[3].Value.ToString() == null)
                            continue;
                        if (GridObstclData.Rows[i].Cells[8].Value.ToString() == "First Corner Polygon" && GridObstclData.Rows[i].Cells[1].Value.ToString() == thirdparent.Text && GridObstclData.Rows[i].Cells[0].Value.ToString() == SecondParent.Text)
                        {
                            GridObstclData.Rows.RemoveAt(GridObstclData.Rows[i].Index);
                            i--;
                            //GridObstclData.Refresh();
                        }
                    }

                }
                if (enode.Text == "Polygon Segment")
                {
                    for (int i = 0; i < GridObstclData.Rows.Count; i++)
                    {
                        if (GridObstclData.Rows[i].Cells[3].Value.ToString() == null)
                            continue;
                        if (GridObstclData.Rows[i].Cells[8].Value.ToString() == "Polygon Segment" && GridObstclData.Rows[i].Cells[1].Value.ToString() == thirdparent.Text && GridObstclData.Rows[i].Cells[0].Value.ToString() == SecondParent.Text)
                        {
                            GridObstclData.Rows.RemoveAt(GridObstclData.Rows[i].Index);
                            i--;
                            //GridObstclData.Refresh();
                        }
                    }

                }
            }
            if (SecondParent.Text == "GP")
            {
                if (enode.Text == "Second Corner Polygon")
                {
                    for (int i = 0; i < GridObstclData.Rows.Count; i++)
                    {
                        if (GridObstclData.Rows[i].Cells[3].Value.ToString() == null)
                            continue;
                        if (GridObstclData.Rows[i].Cells[8].Value.ToString() == "Second Corner Polygon" && GridObstclData.Rows[i].Cells[1].Value.ToString() == thirdparent.Text && GridObstclData.Rows[i].Cells[0].Value.ToString() == SecondParent.Text)
                        {
                            GridObstclData.Rows.RemoveAt(GridObstclData.Rows[i].Index);
                            i--;
                            //GridObstclData.Refresh();
                        }
                    }
                }
                if (enode.Text == "First Corner Polygon")
                {
                    for (int i = 0; i < GridObstclData.Rows.Count; i++)
                    {
                        if (GridObstclData.Rows[i].Cells[3].Value.ToString() == null)
                            continue;
                        if (GridObstclData.Rows[i].Cells[8].Value.ToString() == "First Corner Polygon" && GridObstclData.Rows[i].Cells[1].Value.ToString() == thirdparent.Text && GridObstclData.Rows[i].Cells[0].Value.ToString() == SecondParent.Text)
                        {
                            GridObstclData.Rows.RemoveAt(GridObstclData.Rows[i].Index);
                            i--;
                            //GridObstclData.Refresh();
                        }
                    }

                }
                if (enode.Text == "Polygon Segment")
                {
                    for (int i = 0; i < GridObstclData.Rows.Count; i++)
                    {
                        if (GridObstclData.Rows[i].Cells[3].Value.ToString() == null)
                            continue;
                        if (GridObstclData.Rows[i].Cells[8].Value.ToString() == "Polygon Segment" && GridObstclData.Rows[i].Cells[1].Value.ToString() == thirdparent.Text && GridObstclData.Rows[i].Cells[0].Value.ToString() == SecondParent.Text)
                        {
                            GridObstclData.Rows.RemoveAt(GridObstclData.Rows[i].Index);
                            i--;
                            //GridObstclData.Refresh();
                        }
                    }

                }
            }
            if (SecondParent.Text == "LOC(Single Frequency)")
            {
                if (enode.Text == "Polygon Segment")
                {
                    for (int i = 0; i < GridObstclData.Rows.Count; i++)
                    {
                        if (GridObstclData.Rows[i].Cells[3].Value.ToString() == null)
                            continue;
                        if (GridObstclData.Rows[i].Cells[8].Value.ToString() == "Polygon Segment" && GridObstclData.Rows[i].Cells[1].Value.ToString() == thirdparent.Text && GridObstclData.Rows[i].Cells[0].Value.ToString() == SecondParent.Text)
                        {
                            GridObstclData.Rows.RemoveAt(GridObstclData.Rows[i].Index);
                            i--;
                            //GridObstclData.Refresh();
                        }
                    }
                }
                if (enode.Text == "First Corner Polygon")
                {
                    for (int i = 0; i < GridObstclData.Rows.Count; i++)
                    {
                        if (GridObstclData.Rows[i].Cells[3].Value.ToString() == null)
                            continue;
                        if (GridObstclData.Rows[i].Cells[8].Value.ToString() == "First Corner Polygon" && GridObstclData.Rows[i].Cells[1].Value.ToString() == thirdparent.Text && GridObstclData.Rows[i].Cells[0].Value.ToString() == SecondParent.Text)
                        {
                            GridObstclData.Rows.RemoveAt(GridObstclData.Rows[i].Index);
                            i--;
                            //GridObstclData.Refresh();
                        }
                    }
                }
                if (enode.Text == "Second Corner Polygon")
                {
                    for (int i = 0; i < GridObstclData.Rows.Count; i++)
                    {
                        if (GridObstclData.Rows[i].Cells[3].Value.ToString() == null)
                            continue;
                        if (GridObstclData.Rows[i].Cells[8].Value.ToString() == "Second Corner Polygon" && GridObstclData.Rows[i].Cells[1].Value.ToString() == thirdparent.Text && GridObstclData.Rows[i].Cells[0].Value.ToString() == SecondParent.Text)
                        {
                            GridObstclData.Rows.RemoveAt(GridObstclData.Rows[i].Index);
                            i--;
                            //GridObstclData.Refresh();
                        }
                    }
                }

            }
            if (SecondParent.Text == "LOC(Dual Frequency)")
            {
                if (enode.Text == "Polygon Segment")
                {
                    for (int i = 0; i < GridObstclData.Rows.Count; i++)
                    {
                        if (GridObstclData.Rows[i].Cells[3].Value.ToString() == null)
                            continue;
                        if (GridObstclData.Rows[i].Cells[8].Value.ToString() == "Polygon Segment" && GridObstclData.Rows[i].Cells[1].Value.ToString() == thirdparent.Text && GridObstclData.Rows[i].Cells[0].Value.ToString() == SecondParent.Text)
                        {
                            GridObstclData.Rows.RemoveAt(GridObstclData.Rows[i].Index);
                            i--;
                            //GridObstclData.Refresh();
                        }
                    }
                }
                if (enode.Text == "First Corner Polygon")
                {
                    for (int i = 0; i < GridObstclData.Rows.Count; i++)
                    {
                        if (GridObstclData.Rows[i].Cells[3].Value.ToString() == null)
                            continue;
                        if (GridObstclData.Rows[i].Cells[8].Value.ToString() == "First Corner Polygon" && GridObstclData.Rows[i].Cells[1].Value.ToString() == thirdparent.Text && GridObstclData.Rows[i].Cells[0].Value.ToString() == SecondParent.Text)
                        {
                            GridObstclData.Rows.RemoveAt(GridObstclData.Rows[i].Index);
                            i--;
                            //GridObstclData.Refresh();
                        }
                    }
                }
                if (enode.Text == "Second Corner Polygon")
                {
                    for (int i = 0; i < GridObstclData.Rows.Count; i++)
                    {
                        if (GridObstclData.Rows[i].Cells[3].Value.ToString() == null)
                            continue;
                        if (GridObstclData.Rows[i].Cells[8].Value.ToString() == "Second Corner Polygon" && GridObstclData.Rows[i].Cells[1].Value.ToString() == thirdparent.Text && GridObstclData.Rows[i].Cells[0].Value.ToString() == SecondParent.Text)
                        {
                            GridObstclData.Rows.RemoveAt(GridObstclData.Rows[i].Index);
                            i--;
                            //GridObstclData.Refresh();
                        }
                    }
                }
            }




        }
    }
}
