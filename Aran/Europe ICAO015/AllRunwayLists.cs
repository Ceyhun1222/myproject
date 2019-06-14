using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Aran.AranEnvironment;
using System.Globalization;
using Aran.PANDA.Conventional.Racetrack;
using ChoosePointNS;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geometry;
using Aran.Aim.Features;
using System.Collections.ObjectModel;
using Aran.AranEnvironment.Symbols;
using MahApps.Metro.Controls;
using Aran.Aim.Enums;
using Aran.Queries.Common;
using Aran.Queries;
using Aran.Queries.Omega;
using Aran.Geometries.Operators;
using System.Reflection;
using System.Reflection.Emit;
using Aran.Omega.Properties;
using Aran.Omega.View;
using System.Threading;
using Aran.Aim;
using Aran.Aim.Data;
using System.Data.OleDb;
using ESRI.ArcGIS.SystemUI;
using ESRI.ArcGIS.ADF.CATIDs;
using System.Runtime.InteropServices;
using ESRI.ArcGIS.ArcMapUI;
using System.Diagnostics;
using Aran.Geometries;
using Aran.PANDA.Common;
using Aran.PANDA.RNAV.Departure;
using ICAO015;


namespace ICAO015
{
    public class AllRunwayLists
    {
        public List<Runway> RunwayList;
        public List<RunwayDirection> RwyDirectionList;
        DataTable table = new DataTable();



        public void RunwayListFillDataGridView(List<Runway> rwylist, DataGridView GridRunway)
        {
            table.Columns.Add("Identifier");
            table.Columns.Add("RUNWAY");
            table.Columns.Add("ID");
            table.Columns.Add("LENGTH");

            //RunwayList = rwylist;

            for (int i = 0; i < rwylist.Count; i++)
            {
                DataRow row = table.NewRow();
                row["Identifier"] = rwylist[i].Identifier;
                row["Runway"] = rwylist[i].Designator;
                row["Id"] = rwylist[i].Id;
                row["Length"] = rwylist[i].NominalLength.Value;
                table.Rows.Add(row);
            }



            GridRunway.DataSource = table;

            GridRunway.Columns[0].Visible = false;

            foreach (DataGridViewRow row in GridRunway.Rows)
            {
                row.DefaultCellStyle.BackColor = Color.White;
            }

            GridRunway.ClearSelection();

            GridRunway.Rows[0].DefaultCellStyle.BackColor = Color.WhiteSmoke;
        }
        public void RunwayDirectionListFillCombobx(ComboBox comborunwaydir, List<RunwayDirection> rwydirlist, DataGridView GridRunway)
        {
            var identifyvalue = GridRunway.Rows[0].Cells[0].Value.ToString();

            RwyDirectionList = rwydirlist.Where(identify => identify.UsedRunway.Identifier.ToString() == identifyvalue.ToString()).ToList();

            comborunwaydir.Items.Clear();

            for (int i = 0; i < RwyDirectionList.Count; i++)
            {
                comborunwaydir.Items.Add(RwyDirectionList[i].Designator);
            }
        }
    }
}
