using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data;


namespace ICAO015
{
    public class ShowGridObstacleInputReportForRadiusCalculated
    {
        public void ShowGridObstclInputReportForRadiusCalc(List<AddReportListObstInputForOnlyRadiusCalculated> ReportList, DataGridView Datagrid, DataTable datatable)
        {

            for (int i = 0; i < ReportList.Count; i++)
            {
                if (ReportList[i].TypeOfNAvigation != null)
                {
                    DataRow row = datatable.NewRow();
                    row["TypeOfNavigation"] = ReportList[i].TypeOfNAvigation;
                    row["Navaid Name"] = ReportList[i].NavaidName;
                    row["Radius"] = ReportList[i].Radius;
                    row["Obstacle Distance"] = ReportList[i].Distance;
                    row["Penetrate"] = ReportList[i].Penetrate;
                    datatable.Rows.Add(row);
                }
            }

            Datagrid.DataSource = datatable;

        }
    }
    public class ShowGridObstacleInputReportFor_2D_Graphic_Calculated
    {
        public void ShowGridObstclInputReportFor_2DGraphic(List<AddReportListObstInputForOnly2DGraphicCalculated> ReportList2DGrpahic, DataGridView Datagrid, DataTable datatable)
        {
            for (int i = 0; i < ReportList2DGrpahic.Count; i++)
            {
                if (ReportList2DGrpahic[i].TypeOfNavigation != null)
                {
                    DataRow row = datatable.NewRow();
                    row["Type of Navigation"] = ReportList2DGrpahic[i].TypeOfNavigation;
                    row["Navaid Name"] = ReportList2DGrpahic[i].NavaidName;
                    row["Polygon Type"] = ReportList2DGrpahic[i].Polygon;
                    row["Obstacle Distance"] = ReportList2DGrpahic[i].ObstclDistance;
                    row["Penetrate"] = ReportList2DGrpahic[i].Penetrate;
                    datatable.Rows.Add(row);
                }
            }

            Datagrid.DataSource = datatable;
        }
    }
}
