using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Excel;
using System.Diagnostics;
using ArenaStatic;
using PDM;
using System.IO;
using ARENA;

namespace ChartCodingTable
{
    public class ProcedureTabulation
    {
        private int legCount; // = 3; кол-во легов в процедуре
        private int colC = 12;
        
        private int pointsCount; // = 3;  кол-во Dpn впроцедуре
        //private string ProcedureName;// = "ProcedureNameZZZ";

        private int distanceBetweenTableArea = 5; //количесвтво строк между таблицами содержащими описание ОДИНАКОВЫХ процедур
        public int DistanceBetweenTableArea
        {
            get { return distanceBetweenTableArea; }
            set { distanceBetweenTableArea = value; }
        }

        private int distanceBetwwenTabulationAndWaypointsList = 3; // количесвтво строк между таблицами proceduretabulation - WaypointsList
        public int DistanceBetwwenTabulationAndWaypointsList
        {
            get { return distanceBetwwenTabulationAndWaypointsList; }
            set { distanceBetwwenTabulationAndWaypointsList = value; }
        }

        private string templateName;
        public string TemplateName
        {
            get { return templateName; }
            set { templateName = value; }
        }

        private string newCodingTableName;
        public string NewCodingTableName
        {
            get { return newCodingTableName; }
            set { newCodingTableName = value; }
        }

        //private string _newRoutetingTableName;
        //public string NewRoutetingTableName
        //{
        //    get { return _newRoutetingTableName; }
        //    set { _newRoutetingTableName = value; }
        //}

        //private string _routeingTemplatename;
        //public string RouteingTemplatename
        //{
        //    get { return _routeingTemplatename; }
        //    set { _routeingTemplatename = value; }
        //}

        private double? _magVar;
        public double? MagVar
        {
            get { return _magVar; }
            set { _magVar = value; }
        }

        private string SpeedUOM;
        public string DistanceUOM;
        public string AltitudeUOM;

        public ProcedureTabulation()
        {


        }

        public void CreateCodingTable(List<PDM.PDMObject> procList)
        {

            Type officeType = Type.GetTypeFromProgID("Excel.Application");
            if (officeType == null)
            {
                // Excel is not installed.
                // Show message or alert that Excel is not installed.
                System.Windows.Forms.MessageBox.Show("Microsoft Excel is not installed! It is impossible to create  Procuderes tabulation document.", "SIGMA", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Asterisk);

                return;
            }

            Application m_objExcel = null;
            Workbooks m_objBooks = null;
            _Workbook m_objBook = null;
            Sheets m_objSheets = null;
            _Worksheet m_objSheetTabular = null;
            _Worksheet m_objSheetWaypointlist = null;
            _Worksheet m_objSheetprocIntstruction = null;
            _Worksheet m_objSheetRoute = null;
            Range m_objRange = null;
            object m_objOpt = System.Reflection.Missing.Value;



         
            Range m_objRangeTabDescription = null;
            Range m_objRangeWayponts = null;
            Range m_objRangeRouteting = null;
            Range m_objRangeProcInctruct = null;
            

            try
            {
                m_objExcel = new Application();
                m_objExcel.DisplayAlerts = false;
                m_objBooks = (Workbooks)m_objExcel.Workbooks;
                m_objBook = (_Workbook)(m_objBooks.Add(templateName));
                m_objSheets = (Sheets)m_objBook.Worksheets;
                m_objSheetTabular = m_objSheets.Cast<Worksheet>().SingleOrDefault(w => w.Name == "Tabular Description");//(_Worksheet)(m_objSheets.get_Item(1));
                m_objSheetWaypointlist = m_objSheets.Cast<Worksheet>().SingleOrDefault(w => w.Name == "Routeing");//(_Worksheet)(m_objSheets.get_Item(2));
                m_objSheetprocIntstruction = m_objSheets.Cast<Worksheet>().SingleOrDefault(w => w.Name == "Instruction");//(_Worksheet)(m_objSheets.get_Item(3));


                m_objSheetRoute = m_objSheets.Cast<Worksheet>().SingleOrDefault(w => w.Name == "Routeing");//(_Worksheet)(m_objSheets.get_Item(2));


                m_objRangeTabDescription = m_objSheetTabular.get_Range("Tabular", m_objOpt);
                m_objRangeWayponts = m_objSheetWaypointlist.get_Range("Waypoint", m_objOpt);
                m_objRangeRouteting = m_objSheetRoute.get_Range("RouteTable", m_objOpt);
                m_objRangeProcInctruct = m_objSheetprocIntstruction.get_Range("ProcInstruction", m_objOpt);


                #region Coding

                SpeedUOM = DistanceUOM.CompareTo("NM") == 0 ? "KT" : "KM/H";

                string CelCursor = "ProcedureName";

                m_objRange = m_objSheetTabular.get_Range(CelCursor, m_objOpt);
                m_objRange.Value2 = "";
                string ad = m_objRange.AddressLocal;

                string[] firstCell = ad.Split('$');

                int Inx = Convert.ToInt32(firstCell[2]);


                for (int STEP = 0; STEP < procList.Count; STEP++)
                {
                    if (!(procList[STEP] is PDM.Procedure))continue;

                    PDM.Procedure curProc = (PDM.Procedure)procList[STEP];
                    string ProcedureName = curProc.ProcedureIdentifier;


                    CelCursor = "B" + Inx.ToString();
                    m_objRange = m_objSheetTabular.get_Range(CelCursor, m_objOpt);
                    m_objRange.Value2 = ProcedureName; // header

                    Inx += 2;
                    CelCursor = "H" + Inx.ToString();
                    m_objRange = m_objSheetTabular.get_Range(CelCursor, m_objOpt);
                    m_objRange.Value2 = ((string)m_objRange.Value2).EndsWith(DistanceUOM) ? m_objRange.Value2 : m_objRange.Value2 + " " + DistanceUOM;

                    CelCursor = "J" + Inx.ToString();
                    m_objRange = m_objSheetTabular.get_Range(CelCursor, m_objOpt);
                    m_objRange.Value2 = ((string)m_objRange.Value2).EndsWith(AltitudeUOM) ? m_objRange.Value2 : m_objRange.Value2 + " " + AltitudeUOM;

                    CelCursor = "K" + Inx.ToString();
                    m_objRange = m_objSheetTabular.get_Range(CelCursor, m_objOpt);
                    m_objRange.Value2 = ((string)m_objRange.Value2).EndsWith(SpeedUOM) ? m_objRange.Value2 : m_objRange.Value2 + " " + SpeedUOM;

                    Inx += 1;
                    CelCursor = "B" + Inx.ToString();

                   // foreach (var trans in curProc.Transitions)
                    {


                        object[,] tabData = getLegsData(curProc.Transitions, curProc.ProcedureType);


                        m_objRange = m_objSheetTabular.get_Range(CelCursor, m_objOpt);
                        m_objRange = m_objRange.get_Resize(legCount, colC);
                        m_objRange.ShrinkToFit = false;
                        m_objRange.WrapText = false;
                        m_objRange.HorizontalAlignment = XlHAlign.xlHAlignCenter;
                        m_objRange.Borders.LineStyle = XlLineStyle.xlContinuous;

                        m_objRange.Value2 = tabData; // data


                        object[,] waypntsData = GetWayPoinstData(curProc.Transitions, ANCOR.MapCore.coordtype.DDMMSS_SS_2);
                        Inx = Inx + legCount + distanceBetwwenTabulationAndWaypointsList;

                        #region waypointslist
                        if (pointsCount > 0)
                        {

                            CelCursor = "B" + Inx.ToString();
                            m_objRange = m_objSheetTabular.get_Range(CelCursor, m_objOpt);

                            m_objRangeWayponts.Copy(m_objRange);
                            CelCursor = "B" + (Inx + 2).ToString();
                            m_objRange = m_objSheetTabular.get_Range(CelCursor, m_objOpt);
                            m_objRange.Value2 = ProcedureName;


                            Inx = Inx + 5;


                            for (int i = 0; i < pointsCount; i++)
                            {
                                CelCursor = "C" + (Inx + i).ToString() + ":" + "E" + (Inx + i).ToString();
                                m_objRange = m_objSheetTabular.get_Range(CelCursor);
                                m_objRange.Merge(m_objOpt);
                                m_objRange.ShrinkToFit = false;
                                m_objRange.WrapText = false;
                                m_objRange.Borders.LineStyle = XlLineStyle.xlContinuous;
                            }

                            CelCursor = "B" + (Inx).ToString();

                            m_objRange = m_objSheetTabular.get_Range(CelCursor, m_objOpt);
                            m_objRange = m_objRange.get_Resize(pointsCount, 2);
                            m_objRange.ShrinkToFit = false;
                            m_objRange.WrapText = false;
                            m_objRange.Borders.LineStyle = XlLineStyle.xlContinuous;

                            m_objRange.Value2 = waypntsData;


                            Inx = Inx + pointsCount + distanceBetweenTableArea;
                        }
                        #endregion

                        CelCursor = "B" + Inx.ToString();
                        m_objRange = m_objSheetTabular.get_Range(CelCursor, m_objOpt);


                        if (STEP != procList.Count - 1)
                            m_objRangeTabDescription.Copy(m_objRange);

                        Inx = Inx + 2;

                    }
                }

                m_objRangeWayponts.Delete(Type.Missing);
               
                //m_objRangeUOM.Delete(Type.Missing);
                //m_objSheetWaypointlist.Cells.Clear();
                #endregion

                #region Routeing
               

                CelCursor = "B3";
                m_objRange = m_objSheetRoute.get_Range(CelCursor, m_objOpt);
                m_objRangeRouteting.Cut(m_objRange);

                CelCursor = "B4";
              
                int rIndx = 0;
                object[,] tabDataRoute = new Object[procList.Count, 2];
                foreach (var item in procList)
                {
                    Procedure prc = (Procedure)item;
                    tabDataRoute[rIndx, 0] = prc.ProcedureIdentifier;
                    tabDataRoute[rIndx, 1] = CreateRouteString(prc);
                    rIndx++;
                }

                
                m_objRange = m_objSheetRoute.get_Range(CelCursor, m_objOpt);
                m_objRange = m_objRange.get_Resize(procList.Count, 2);
                m_objRange.ShrinkToFit = false;
                m_objRange.WrapText = false;
                //m_objRange.HorizontalAlignment = XlHAlign.xlHAlignCenter;
                m_objRange.Borders.LineStyle = XlLineStyle.xlContinuous;

                m_objRange.Value2 = tabDataRoute; // data

                rIndx++;
                CelCursor = "B" + (3 + rIndx).ToString() + ":D" + (10 + rIndx).ToString();
                m_objRange = m_objSheetRoute.get_Range(CelCursor, m_objOpt);
                m_objRange.ClearFormats();

                #endregion

                #region ProcInstruction


                CelCursor = "B3";
                m_objRange = m_objSheetprocIntstruction.get_Range(CelCursor, m_objOpt);
                m_objRangeProcInctruct.Cut(m_objRange);

                CelCursor = "B4";

                rIndx = 0;
                tabDataRoute = new Object[procList.Count, 3];
                foreach (var item in procList)
                {
                    Procedure prc = (Procedure)item;
                    tabDataRoute[rIndx, 0] = prc.ProcedureIdentifier;
                    tabDataRoute[rIndx, 1] = prc.Instruction;
                    tabDataRoute[rIndx, 2] = "";
                    rIndx++;
                }


                m_objRange = m_objSheetprocIntstruction.get_Range(CelCursor, m_objOpt);
                m_objRange = m_objRange.get_Resize(procList.Count, 3);
                m_objRange.ShrinkToFit = false;
                m_objRange.WrapText = true;
                m_objRange.VerticalAlignment = XlVAlign.xlVAlignCenter;
                //m_objRange.HorizontalAlignment = XlHAlign.xlHAlignCenter;
                m_objRange.Borders.LineStyle = XlLineStyle.xlContinuous;

                m_objRange.Value2 = tabDataRoute; // data

                rIndx++;
                CelCursor = "B" + (3 + rIndx).ToString() + ":D" + (10 + rIndx).ToString();
                m_objRange = m_objSheetprocIntstruction.get_Range(CelCursor, m_objOpt);
                m_objRange.ClearFormats();

                #endregion


                while (FileOpened(newCodingTableName))
                    System.Windows.Forms.MessageBox.Show("The file " + newCodingTableName + " already opened" + System.Environment.NewLine + "Please, close the file " + newCodingTableName, "Info", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);

                // Save the Workbook and quit Excel.
                m_objBook.SaveAs(newCodingTableName, XlFileFormat.xlWorkbookNormal,
                    Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                    XlSaveAsAccessMode.xlNoChange, Type.Missing,
                    Type.Missing, Type.Missing, Type.Missing, Type.Missing);

            }
            finally
            {
                m_objBook.Close(false, m_objOpt, m_objOpt);
                m_objExcel.Quit();

                //System.GC.Collect();
                GC.WaitForPendingFinalizers();
                //GC.Collect();
            }

            Process.Start(newCodingTableName);
            
        }

        public void CreateCodingTable_IAC(List<PDM.PDMObject> procList)
        {
            Application m_objExcel = null;
            Workbooks m_objBooks = null;
            _Workbook m_objBook = null;
            Sheets m_objSheets = null;
            _Worksheet m_objSheetTabular = null;
            _Worksheet m_objSheetWaypointlist = null;
            _Worksheet m_objSheetRoute = null;
            Range m_objRange = null;
            object m_objOpt = System.Reflection.Missing.Value;

            _Worksheet m_objSheetprocIntstruction = null;


            Range m_objRangeTabDescription = null;
            Range m_objRangeWayponts = null;
            Range m_objRangeRouteting = null;
            Range m_objRangeTableHeader = null;
            Range m_objRangeProcInctruct = null;


            try
            {
                m_objExcel = new Application();
                m_objExcel.DisplayAlerts = false;
                m_objBooks = (Workbooks)m_objExcel.Workbooks;
                m_objBook = (_Workbook)(m_objBooks.Add(templateName));
                m_objSheets = (Sheets)m_objBook.Worksheets;
                m_objSheetTabular = (_Worksheet)(m_objSheets.get_Item("Tabular Description"));
                m_objSheetWaypointlist = (_Worksheet)(m_objSheets.get_Item("Routeing"));
                m_objSheetprocIntstruction = (_Worksheet)(m_objSheets.get_Item("Instruction"));

                m_objSheetRoute = (_Worksheet)(m_objSheets.get_Item(2));


                m_objRangeTabDescription = m_objSheetTabular.get_Range("Tabular", m_objOpt);
                m_objRangeWayponts = m_objSheetWaypointlist.get_Range("Waypoint", m_objOpt);
                m_objRangeRouteting = m_objSheetRoute.get_Range("RouteTable", m_objOpt);
                m_objRangeTableHeader = m_objSheetTabular.get_Range("TabDescription", m_objOpt);
                m_objRangeProcInctruct = m_objSheetprocIntstruction.get_Range("ProcInstruction", m_objOpt);


                #region Coding

                SpeedUOM = DistanceUOM.CompareTo("NM") == 0 ? "KT" : "KM/H";

                string CelCursor = "ProcedureName";

                m_objRange = m_objSheetTabular.get_Range(CelCursor, m_objOpt);
                m_objRange.Value2 = "";
                string ad = m_objRange.AddressLocal;

                string[] firstCell = ad.Split('$');

                int Inx = Convert.ToInt32(firstCell[2]);


                for (int STEP = 0; STEP < procList.Count; STEP++)
                {
                    if (!(procList[STEP] is PDM.Procedure)) continue;

                    PDM.Procedure curProc = (PDM.Procedure)procList[STEP];
                    string ProcedureName = curProc.ProcedureIdentifier;


                    CelCursor = "B" + Inx.ToString();
                    m_objRange = m_objSheetTabular.get_Range(CelCursor, m_objOpt);
                    m_objRange.Value2 = ProcedureName; // header

                    Inx += 2;
                    CelCursor = "H" + Inx.ToString();
                    m_objRange = m_objSheetTabular.get_Range(CelCursor, m_objOpt);
                    m_objRange.Value2 = ((string)m_objRange.Value2).EndsWith(DistanceUOM) ? m_objRange.Value2 : m_objRange.Value2 + " " + DistanceUOM;

                    CelCursor = "J" + Inx.ToString();
                    m_objRange = m_objSheetTabular.get_Range(CelCursor, m_objOpt);
                    m_objRange.Value2 = ((string)m_objRange.Value2).EndsWith(AltitudeUOM) ? m_objRange.Value2 : m_objRange.Value2 + " " + AltitudeUOM;

                    CelCursor = "K" + Inx.ToString();
                    m_objRange = m_objSheetTabular.get_Range(CelCursor, m_objOpt);
                    m_objRange.Value2 = ((string)m_objRange.Value2).EndsWith(SpeedUOM) ? m_objRange.Value2 : m_objRange.Value2 + " " + SpeedUOM;

                    CelCursor = "L" + Inx.ToString();
                    m_objRange = m_objSheetTabular.get_Range(CelCursor, m_objOpt);
                    m_objRange.Value2 = ((string)m_objRange.Value2).EndsWith(AltitudeUOM) ? m_objRange.Value2 : m_objRange.Value2 + " " + AltitudeUOM;

                    Inx += 1;
                    CelCursor = "B" + Inx.ToString();


                    List<ProcedureTransitions> sortedTransitions = curProc.Transitions;//sortTransitions(curProc);
                    //foreach (var trans in curProc.Transitions)

                    int tc = 0;//sortedTransitions.Count;
                    foreach (ProcedureTransitions trans in sortedTransitions)
                    {

                        object[,] tabData = getLegsData(trans, curProc.ProcedureType);

                        m_objRange = m_objSheetTabular.get_Range(CelCursor, m_objOpt);
                        m_objRange = m_objRange.get_Resize(legCount, colC);
                        m_objRange.ShrinkToFit = false;
                        m_objRange.WrapText = false;
                        m_objRange.HorizontalAlignment = XlHAlign.xlHAlignCenter;
                        m_objRange.Borders.LineStyle = XlLineStyle.xlContinuous;

                        m_objRange.Value2 = tabData; // data

                        //object[,] waypntsData = GetWayPoinstData(curProc.Transitions, ANCOR.MapCore.coordtype.DDMMSS_SS_2);
                        object[,] waypntsData = GetWayPoinstData(trans, ANCOR.MapCore.coordtype.DDMMSS_SS_2);
                        Inx = Inx + legCount + distanceBetwwenTabulationAndWaypointsList;

                        #region waypointslist
                        if (pointsCount > 0)
                        {

                            CelCursor = "B" + Inx.ToString();
                            m_objRange = m_objSheetTabular.get_Range(CelCursor, m_objOpt);

                            m_objRangeWayponts.Copy(m_objRange);
                            CelCursor = "B" + (Inx + 2).ToString();
                            m_objRange = m_objSheetTabular.get_Range(CelCursor, m_objOpt);
                            m_objRange.Value2 = ProcedureName;


                            Inx = Inx + 5;


                            for (int i = 0; i < pointsCount; i++)
                            {
                                CelCursor = "C" + (Inx + i).ToString() + ":" + "E" + (Inx + i).ToString();
                                m_objRange = m_objSheetTabular.get_Range(CelCursor);
                                m_objRange.Merge(m_objOpt);
                                m_objRange.ShrinkToFit = false;
                                m_objRange.WrapText = false;
                                m_objRange.Borders.LineStyle = XlLineStyle.xlContinuous;
                            }

                            CelCursor = "B" + (Inx).ToString();

                            m_objRange = m_objSheetTabular.get_Range(CelCursor, m_objOpt);
                            m_objRange = m_objRange.get_Resize(pointsCount, 2);
                            m_objRange.ShrinkToFit = false;
                            m_objRange.WrapText = false;
                            m_objRange.Borders.LineStyle = XlLineStyle.xlContinuous;

                            m_objRange.Value2 = waypntsData;


                            Inx = Inx + pointsCount + distanceBetweenTableArea;
                        }
                        #endregion

                        CelCursor = "B" + Inx.ToString();
                        m_objRange = m_objSheetTabular.get_Range(CelCursor, m_objOpt);

                        if (tc != sortedTransitions.Count - 1)
                        {
                            m_objRangeTabDescription.Copy(m_objRange);
                            Inx = Inx + 5;
                        }
                        tc++;

                        CelCursor = "B" + Inx.ToString();

                        if (STEP != procList.Count - 1)
                            m_objRangeTabDescription.Copy(m_objRange);

                        Inx = Inx + 2;

                    }
                }

                m_objRangeWayponts.Delete(Type.Missing);

                //m_objRangeUOM.Delete(Type.Missing);
                //m_objSheetWaypointlist.Cells.Clear();
                #endregion

                #region Routeing


                CelCursor = "B3";
                m_objRange = m_objSheetRoute.get_Range(CelCursor, m_objOpt);
                m_objRangeRouteting.Cut(m_objRange);

                CelCursor = "B4";

                int rIndx = 0;
                object[,] tabDataRoute = new Object[procList.Count, 2];
                foreach (var item in procList)
                {
                    Procedure prc = (Procedure)item;
                    tabDataRoute[rIndx, 0] = prc.ProcedureIdentifier;
                    tabDataRoute[rIndx, 1] = CreateRouteString(prc);
                    rIndx++;
                }


                m_objRange = m_objSheetRoute.get_Range(CelCursor, m_objOpt);
                m_objRange = m_objRange.get_Resize(procList.Count, 2);
                m_objRange.ShrinkToFit = false;
                m_objRange.WrapText = false;
                //m_objRange.HorizontalAlignment = XlHAlign.xlHAlignCenter;
                m_objRange.Borders.LineStyle = XlLineStyle.xlContinuous;

                m_objRange.Value2 = tabDataRoute; // data

                rIndx++;
                CelCursor = "B" + (3 + rIndx).ToString() + ":D" + (10 + rIndx).ToString();
                m_objRange = m_objSheetRoute.get_Range(CelCursor, m_objOpt);
                m_objRange.ClearFormats();

                #endregion

                #region ProcInstruction


                CelCursor = "B3";
                m_objRange = m_objSheetprocIntstruction.get_Range(CelCursor, m_objOpt);
                m_objRangeProcInctruct.Cut(m_objRange);

                CelCursor = "B4";

                rIndx = 0;
                tabDataRoute = new Object[procList.Count, 3];
                foreach (var item in procList)
                {
                    Procedure prc = (Procedure)item;
                    tabDataRoute[rIndx, 0] = prc.ProcedureIdentifier;
                    tabDataRoute[rIndx, 1] = prc.Instruction;
                    tabDataRoute[rIndx, 2] = "";
                    rIndx++;
                }


                m_objRange = m_objSheetprocIntstruction.get_Range(CelCursor, m_objOpt);
                m_objRange = m_objRange.get_Resize(procList.Count, 3);
                m_objRange.ShrinkToFit = false;
                m_objRange.WrapText = true;
                m_objRange.VerticalAlignment = XlVAlign.xlVAlignCenter;
                //m_objRange.HorizontalAlignment = XlHAlign.xlHAlignCenter;
                m_objRange.Borders.LineStyle = XlLineStyle.xlContinuous;

                m_objRange.Value2 = tabDataRoute; // data

                rIndx++;
                CelCursor = "B" + (3 + rIndx).ToString() + ":D" + (10 + rIndx).ToString();
                m_objRange = m_objSheetprocIntstruction.get_Range(CelCursor, m_objOpt);
                m_objRange.ClearFormats();

                #endregion


                while (FileOpened(newCodingTableName))
                    System.Windows.Forms.MessageBox.Show("The file " + newCodingTableName + " already opened" + System.Environment.NewLine + "Please, close the file " + newCodingTableName, "Info", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);

                // Save the Workbook and quit Excel.
                m_objBook.SaveAs(newCodingTableName, XlFileFormat.xlWorkbookNormal,
                    Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                    XlSaveAsAccessMode.xlNoChange, Type.Missing,
                    Type.Missing, Type.Missing, Type.Missing, Type.Missing);

            }
            finally
            {
                m_objBook.Close(false, m_objOpt, m_objOpt);
                m_objExcel.Quit();

                //System.GC.Collect();
                GC.WaitForPendingFinalizers();
                //GC.Collect();
            }

            Process.Start(newCodingTableName);

        }

        private List<ProcedureTransitions> sortTransitions(Procedure curProc)
        {
            /// 
            /// шаг 1
            ///  Анализируй Transitions, найди FinalTransitions и MissedTransitions
            /// Леги этих Transitions собери в отдельный лист ListEnd
            /// 
            ///шаг 2
            ///перебирай Transitions(FinalTransitions и MissedTransitions не принимай во внимание)
            ///Леги этих Transitions собери в отдельный лист ListProc
            ///добавь в ListProc леги из ListEnd
            ///
            ///шаг 3
            ///полученый TrList оформи как ProcedureTransitions и сохрани результат
            ///

            ProcedurePhaseType[] ProcedurePhaseTypeArray = new ProcedurePhaseType[] { ProcedurePhaseType.FINAL, ProcedurePhaseType.MISSED, ProcedurePhaseType.MISSED_P, ProcedurePhaseType.MISSED_S };

            List<ProcedureTransitions> res = new List<ProcedureTransitions>();

            List<ProcedureLeg> ListEnd = new List<ProcedureLeg>();

            foreach (ProcedureTransitions trans in curProc.Transitions)
            {
                if (ProcedurePhaseTypeArray.Contains(trans.RouteType))
                    ListEnd.AddRange(trans.Legs);
            }

            foreach (ProcedureTransitions trans in curProc.Transitions)
            {
                if (ProcedurePhaseTypeArray.Contains(trans.RouteType)) continue;

                ProcedureTransitions Tr = new ProcedureTransitions { ID_procedure = curProc.ID, RouteType = ProcedurePhaseType.OTHER, Legs = new List<ProcedureLeg>() };

                Tr.Legs.AddRange(trans.Legs);
                if (ListEnd.Count > 0) Tr.Legs.AddRange(ListEnd);

                res.Add(Tr);

            }

            return res;

        }

        private object[,] getLegsData(PDM.ProcedureTransitions Transition, PROC_TYPE_code procType)
        {

            List<PDM.ProcedureLeg> LegList = new List<PDM.ProcedureLeg>();

            LegList.AddRange(Transition.Legs);


            legCount = LegList.Count;


            object[,] tabData = new Object[legCount, colC];

            for (int i = 0; i < legCount; i++)
            {

                tabData[i, 0] = (i + 1).ToString(); //Serial Number
                tabData[i, 1] = LegList[i].LegTypeARINC.ToString(); //Path Descriptor

                if (LegList[i].LegTypeARINC == PDM.CodeSegmentPath.IF)
                {
                    tabData[i, 2] = LegList[i].StartPoint != null ? LegList[i].StartPoint.SegmentPointDesignator : "-"; //Waypoint Identifier
                    tabData[i, 3] = LegList[i].StartPoint != null && LegList[i].StartPoint.FlyOver ? "Y" : "-"; //Fly - over
                }
                else
                {
                    tabData[i, 2] = LegList[i].EndPoint != null ? LegList[i].EndPoint.SegmentPointDesignator : "-"; //Waypoint Identifier
                    tabData[i, 3] = LegList[i].EndPoint != null && LegList[i].EndPoint.FlyOver ? "Y" : "-"; //Fly - over
                }

                string txtCourse = "-";
                if (LegList[i].CourseType == PDM.CodeCourse.MAG_TRACK && LegList[i].Course.HasValue && MagVar.HasValue)
                {
                    string mCourse = Math.Round(LegList[i].Course.Value, 0).ToString();
                    string tCourse = "(" + Math.Round(LegList[i].Course.Value + MagVar.Value, 1).ToString() + ")";
                    if (mCourse.StartsWith("NaN")) mCourse = "";
                    if (tCourse.StartsWith("(NaN)")) tCourse = "";
                    txtCourse = mCourse + " (" + tCourse + ")";
                }
                else if (LegList[i].CourseType == PDM.CodeCourse.TRUE_TRACK && LegList[i].Course.HasValue && MagVar.HasValue)
                {
                    string mCourse = Math.Round(LegList[i].Course.Value - MagVar.Value, 0).ToString();
                    string tCourse = "(" + Math.Round(LegList[i].Course.Value, 1).ToString() + ")";
                    if (mCourse.StartsWith("NaN")) mCourse = "";
                    if (tCourse.StartsWith("(NaN)")) tCourse = "";
                    txtCourse = mCourse + tCourse;
                }
                tabData[i, 4] = txtCourse; //Course °M(°T)

                tabData[i, 5] = MagVar.HasValue ? MagVar.Value.ToString() : "-"; //Magnetic Variation

                tabData[i, 6] = LegList[i].Length.HasValue ? GetLengthValue(LegList[i]) : "-"; //Distance

                tabData[i, 7] = "-"; //Turn Direction
                if (LegList[i].TurnDirection == PDM.DirectionTurnType.RIGHT) tabData[i, 7] = "R";
                else if (LegList[i].TurnDirection == PDM.DirectionTurnType.LEFT) tabData[i, 7] = "L";


                string txtVal = "-";

                switch (LegList[i].AltitudeInterpretation)
                {
                    case PDM.AltitudeUseType.ABOVE_LOWER:
                        txtVal = LegList[i].LowerLimitAltitude.HasValue && LegList[i].LowerLimitAltitude.Value != Double.NaN ? "+" + GetAltitudeValue(LegList[i].LowerLimitAltitude.Value, LegList[i].LowerLimitAltitudeUOM) : "-";
                        break;
                    case PDM.AltitudeUseType.AT_LOWER:
                        txtVal = LegList[i].LowerLimitAltitude.HasValue && LegList[i].LowerLimitAltitude.Value != Double.NaN ? GetAltitudeValue(LegList[i].LowerLimitAltitude.Value, LegList[i].LowerLimitAltitudeUOM) : "-";
                        break;
                    case PDM.AltitudeUseType.BELOW_UPPER:
                        txtVal = LegList[i].UpperLimitAltitude.HasValue && LegList[i].UpperLimitAltitude.Value != Double.NaN ? "-" + GetAltitudeValue(LegList[i].UpperLimitAltitude.Value, LegList[i].UpperLimitAltitudeUOM) : "-";
                        break;
                    case PDM.AltitudeUseType.BETWEEN:
                        txtVal = LegList[i].UpperLimitAltitude.HasValue && LegList[i].UpperLimitAltitude.Value != Double.NaN && LegList[i].LowerLimitAltitude.HasValue && LegList[i].LowerLimitAltitude.Value != Double.NaN ?
                                                                        txtVal = "+" + GetAltitudeValue(LegList[i].LowerLimitAltitude.Value, LegList[i].LowerLimitAltitudeUOM) +
                                                                                 "/" + GetAltitudeValue(LegList[i].UpperLimitAltitude.Value, LegList[i].UpperLimitAltitudeUOM) : "-";
                        break;
                    case PDM.AltitudeUseType.RECOMMENDED:
                        txtVal = LegList[i].AltitudeOverrideATC.HasValue && LegList[i].AltitudeOverrideATC.Value != Double.NaN ? "@" + GetAltitudeValue(LegList[i].AltitudeOverrideATC.Value, LegList[i].AltitudeUOM) : "-";
                        break;
                    case PDM.AltitudeUseType.AS_ASSIGNED:
                    case PDM.AltitudeUseType.OTHER:
                    default:
                        txtVal = "";
                        break;
                }

                if (txtVal.ToUpper().Contains("NAN")) txtVal = "-";
                if (txtVal.ToUpper().CompareTo("@") == 0) txtVal = "-";
                if (txtVal.ToUpper().CompareTo("+") == 0) txtVal = "-";
                if (txtVal.ToUpper().CompareTo("-") == 0) txtVal = "-";
                if (txtVal.ToUpper().CompareTo("/") == 0) txtVal = "-";
                tabData[i, 8] = txtVal; //Altitude



                txtVal = "-";
                switch (LegList[i].SpeedInterpritation)
                {
                    case PDM.AltitudeUseType.ABOVE_LOWER:
                        txtVal = LegList[i].SpeedLimit.HasValue && LegList[i].SpeedLimit.Value != Double.NaN ? "+" + GetSpeedValue(LegList[i]) : "-";
                        break;

                    case PDM.AltitudeUseType.AT_LOWER:

                    case PDM.AltitudeUseType.RECOMMENDED:
                    case AltitudeUseType.EXPECT_LOWER:
                    case AltitudeUseType.AS_ASSIGNED:
                    case AltitudeUseType.OTHER:

                        txtVal = LegList[i].SpeedLimit.HasValue && LegList[i].SpeedLimit.Value != Double.NaN ? GetSpeedValue(LegList[i]) : "-";
                        break;

                    case PDM.AltitudeUseType.BELOW_UPPER:
                        txtVal = LegList[i].SpeedLimit.HasValue && LegList[i].SpeedLimit.Value != Double.NaN ? "-" + GetSpeedValue(LegList[i]) : "-";
                        break;

                    case PDM.AltitudeUseType.BETWEEN:
                        txtVal = LegList[i].SpeedLimit.HasValue && LegList[i].SpeedLimit.Value != Double.NaN && LegList[i].SpeedLimit.HasValue && LegList[i].SpeedLimit.Value != Double.NaN ?
                                                                        txtVal = "+" + GetSpeedValue(LegList[i]) +
                                                                                 "/" + GetSpeedValue(LegList[i]) : "-";
                        break;
                    //case PDM.AltitudeUseType.RECOMMENDED:
                    //case AltitudeUseType.EXPECT_LOWER:
                    //case AltitudeUseType.AS_ASSIGNED:
                    //case AltitudeUseType.OTHER:
                    //    break;
                    default:
                        break;
                }

                if (txtVal.ToUpper().Contains("NAN")) txtVal = "-";
                if (txtVal.ToUpper().CompareTo("@") == 0) txtVal = "-";
                if (txtVal.ToUpper().CompareTo("+") == 0) txtVal = "-";
                if (txtVal.ToUpper().CompareTo("-") == 0) txtVal = "-";
                if (txtVal.ToUpper().CompareTo("/") == 0) txtVal = "-";
                tabData[i, 9] = txtVal; //Speed
                //tabData[i, 9] = LegList[i].SpeedLimit.HasValue && LegList[i].SpeedLimit.Value > 0 ? GetSpeedValue(LegList[i]): "-"; //Speed


                if (procType == PROC_TYPE_code.Approach) //VPA...
                {
                    string VA = LegList[i].VerticalAngle.HasValue && LegList[i].VerticalAngle.Value != 0 ? Math.Round(LegList[i].VerticalAngle.Value, 1).ToString() : "";
                    if (VA.ToUpper().Contains("NAN")) tabData[i, 10] = "-";
                    tabData[i, 10] = VA;


                    if (LegList[i].LegSpecialization == SegmentLegSpecialization.FinalLeg && LegList[i].VerticalAngle.HasValue &&
                        (((FinalLeg)LegList[i]).LandingSystemCategory != CodeApproachGuidance.NON_PRECISION || ((FinalLeg)LegList[i]).LandingSystemCategory != CodeApproachGuidance.OTHER))
                    {
                        if (((FinalLeg)LegList[i]).StartPoint != null && ((FinalLeg)LegList[i]).StartPoint.PointRole == ProcedureFixRoleType.FAF && ((FinalLeg)LegList[i]).StartPoint.PointFacilityMakeUp != null && ((FinalLeg)LegList[i]).StartPoint.PointFacilityMakeUp.AngleIndication != null)
                        {
                            string navId = ((FinalLeg)LegList[i]).StartPoint.PointFacilityMakeUp.AngleIndication.SignificantPointID;

                            var nav = DataCash.GetNavaidByID(navId);
                            if (nav != null)
                            {
                                //loc = ((NavaidSystem)nav).Components.FindAll(eq => eq.PDM_Type == PDM_ENUM.Localizer).FirstOrDefault();
                                var gp = ((NavaidSystem)nav).Components.FindAll(eq => eq.PDM_Type == PDM_ENUM.GlidePath).FirstOrDefault();
                                if (gp != null && ((GlidePath)gp).ThresholdCrossingHeight.HasValue)
                                    tabData[i, 10] = tabData[i, 10] + " / " + GetAltitudeValue(((GlidePath)gp).ThresholdCrossingHeight.Value, ((GlidePath)gp).UomDistVer);
                            }
                        }

                    }
                    else if (LegList[i].LegSpecialization == SegmentLegSpecialization.FinalLeg && LegList[i].VerticalAngle.HasValue &&
                        (((FinalLeg)LegList[i]).LandingSystemCategory == CodeApproachGuidance.NON_PRECISION ))
                    {
                        tabData[i, 10] = (AltitudeUOM == "M") ? tabData[i, 10] + " / 15" : tabData[i, 10] + " / 50";


                    }
                }
                else
                {
                    tabData[i, 10] = LegList[i].VerticalAngle.HasValue && LegList[i].VerticalAngle.Value != 0 ? Math.Round(LegList[i].VerticalAngle.Value, 1).ToString() : "-";
                    if (tabData[i, 10].ToString().ToUpper().Contains("NAN")) tabData[i, 10] = "-";
                }

                tabData[i, 11] = LegList[i].RequiredNavigationPerformance.HasValue ? "RNP " + (int)LegList[i].RequiredNavigationPerformance.Value : "-"; //Navigation Specification
            }

            return tabData;
        }

        private object[,] GetWayPoinstData(PDM.ProcedureTransitions Transition, ANCOR.MapCore.coordtype coordStringType)
        {
            Dictionary<string, PDM.SegmentPoint> wypnts = new Dictionary<string, PDM.SegmentPoint>();
            object[,] waypntsData;
            try
            {



                foreach (PDM.ProcedureLeg leg in Transition.Legs)
                {
                    if (leg.StartPoint != null && leg.StartPoint.PointChoiceID != null && !wypnts.ContainsKey(leg.StartPoint.PointChoiceID))
                        wypnts.Add(leg.StartPoint.PointChoiceID, leg.StartPoint);
                    if (leg.EndPoint != null && leg.EndPoint.PointChoiceID != null && !wypnts.ContainsKey(leg.EndPoint.PointChoiceID))
                        wypnts.Add(leg.EndPoint.PointChoiceID, leg.EndPoint);


                }

                pointsCount = wypnts.Count;

                waypntsData = new Object[pointsCount, 2];

                //for (int i = 0; i < pointsCount; i++)
                int i = 0;
                foreach (KeyValuePair<string, PDM.SegmentPoint> pair in wypnts)
                {
                    if (pair.Value.Geo == null) pair.Value.RebuildGeo();

                    waypntsData[i, 0] = pair.Value.SegmentPointDesignator;

                    if (pair.Value.Geo != null)
                        waypntsData[i, 1] = ArenaStaticProc.LatToDDMMSS(((ESRI.ArcGIS.Geometry.IPoint)pair.Value.Geo).Y.ToString(), coordStringType) + "         " + ArenaStaticProc.LonToDDMMSS(((ESRI.ArcGIS.Geometry.IPoint)pair.Value.Geo).X.ToString(), coordStringType);
                    else
                        waypntsData[i, 1] = " - - " + "         " + " - - ";

                    i++;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.InnerException);
                return null;
            }

            return waypntsData;

        }

        private bool FileOpened(string _fileName)
        {
            bool res = false;
            if (!File.Exists(_fileName)) return false;

            try
            {
                using (var fs = File.Open(_fileName, FileMode.Open, FileAccess.Read, FileShare.None))
                {
                    //Console.WriteLine("файл свободен");
                    res = false;
                }
            }
            catch (IOException ioex)
            {
                //Console.WriteLine("файл занят");
                res = true;
            }

            return res;
        }

        private object CreateRouteString(Procedure prc)
        {
            string res = "";

            int cntr = 0;
            foreach (ProcedureTransitions trans in prc.Transitions)
            {
                foreach (ProcedureLeg leg in trans.Legs)
                {
                    string p1 = "";
                    string p2 = "";
                    if (leg.StartPoint != null && cntr ==0 && leg.LegTypeARINC != CodeSegmentPath.IF)
                    {
                        p1 = leg.StartPoint.SegmentPointDesignator != null ? leg.StartPoint.SegmentPointDesignator : ""; // StartPointDesig
                        if (p1.ToUpper().StartsWith("COORD")) p1 = "";

                    }
                    else if (leg.StartPoint == null && cntr == 0 && leg.LegTypeARINC != CodeSegmentPath.IF)
                    {
                       if ( prc.RNAV)
                           p1 = leg.Course.HasValue && !leg.Course.Value.ToString().StartsWith("NaN") ? "T" + Math.Round(leg.Course.Value, 1).ToString()  : "";
                       else
                           p1 = leg.Course.HasValue && !leg.Course.Value.ToString().StartsWith("NaN") ? Math.Round(leg.Course.Value, 0).ToString()  : "";

                       string alt = GetAltString(leg);

                       p1 = "[" + p1 + alt + "]";
                       if (p1.EndsWith("[]")) p1 = p1.Remove(p1.Length - 2, 2);


                    }


                    ////////////////////////////////////////
                    if (leg.EndPoint != null)
                    {
                        p2 = leg.EndPoint.SegmentPointDesignator != null ? leg.EndPoint.SegmentPointDesignator : "";
                        if (p2.ToUpper().StartsWith("COORD")) p2 = "";
                        
                        string alt = GetAltString(leg);
                        string turn = "";
                        if (leg.TurnDirection != DirectionTurnType.OTHER && leg.TurnDirection != DirectionTurnType.EITHER)
                            turn = leg.TurnDirection == DirectionTurnType.LEFT? "L" : "R";
                        string spd = GetSpeedStr(leg);
                        string crs = "";
                        if (leg.LegTypeARINC.ToString().StartsWith("C") || leg.LegTypeARINC.ToString().EndsWith("A"))
                        {
                            if (prc.RNAV)
                                crs = leg.Course.HasValue && !leg.Course.Value.ToString().StartsWith("NaN") ? "T" + Math.Round(leg.Course.Value, 1).ToString() : "";
                            else
                                crs = leg.Course.HasValue && !leg.Course.Value.ToString().StartsWith("NaN") ? Math.Round(leg.Course.Value, 0).ToString()  : "";
                        }

                        List<string> resStr = new List<string>();
                        if (turn.Trim().Length >0) resStr.Add(turn);
                        if (crs.Trim().Length > 0) resStr.Add(crs);
                        if (alt.Trim().Length > 0) resStr.Add(alt);
                        if (spd.Trim().Length > 0) resStr.Add(spd);
                        string RoteStr = "";
                        for (int i = 0; i < resStr.Count; i++)
                        {
                            RoteStr = RoteStr + resStr[i];
                            if (i < resStr.Count - 1) RoteStr = RoteStr + ";";
                        }
                        p2 = p2 + "[" + RoteStr + "]";
                        if (p2.EndsWith("[]")) p2 = p2.Remove(p2.Length - 2, 2);
                    }
                    else
                    {
                        if (prc.RNAV)
                            p2 = leg.Course.HasValue &&  !leg.Course.Value.ToString().StartsWith("NaN") ? "T" + Math.Round(leg.Course.Value, 1).ToString()  : "";
                        else
                            p2 = leg.Course.HasValue && !leg.Course.Value.ToString().StartsWith("NaN") ? Math.Round(leg.Course.Value, 0).ToString() : "";

                        string alt = GetAltString(leg);
                        string turn = "";
                        if (leg.TurnDirection != DirectionTurnType.OTHER && leg.TurnDirection != DirectionTurnType.EITHER)
                            turn = leg.TurnDirection == DirectionTurnType.LEFT ? "L" : "R";
                        string spd = GetSpeedStr(leg);
                        List<string> resStr = new List<string>();
                        if (turn.Trim().Length > 0) resStr.Add(turn);
                        if (alt.Trim().Length > 0) resStr.Add(alt);
                        if (spd.Trim().Length > 0) resStr.Add(spd);
                        string RoteStr = "";
                        for (int i = 0; i < resStr.Count; i++)
                        {
                            RoteStr = RoteStr + resStr[i];
                            if (i < resStr.Count - 1) RoteStr = RoteStr + ";";
                        }
                        p2 = p2 + "[" + RoteStr + "]";
                        if (p2.EndsWith("[]")) p2 = p2.Remove(p2.Length - 2, 2);

                    }

                    if (cntr == 0)
                    {
                        res = p1.Length > 0 ? res + p1 + "-" : "";
                    }
                    res = p2.Length > 0 ? res +  p2 + "-" : "";
                    cntr++;
                }
            }


            if (res.EndsWith("-")) res = res.Remove(res.Length - 1, 1);
            return res;
        }

        private string GetSpeedStr(ProcedureLeg leg)
        {
            string spd = "";

            switch (leg.SpeedInterpritation)
            {
                case AltitudeUseType.AT_LOWER:
                    spd = leg.SpeedLimit.HasValue ? "K" + leg.SpeedLimit.Value.ToString() + "-" : "";
                    break;
                case AltitudeUseType.ABOVE_LOWER:
                    spd = leg.SpeedLimit.HasValue ? "K" + leg.SpeedLimit.Value.ToString() + "+" : "";
                    break;
                case AltitudeUseType.BELOW_UPPER:
                    spd = leg.SpeedLimit.HasValue ? "-" + leg.SpeedLimit.Value.ToString(): "";
                    break;
                case AltitudeUseType.OTHER:
                case AltitudeUseType.BETWEEN:
                case AltitudeUseType.RECOMMENDED:
                case AltitudeUseType.EXPECT_LOWER:
                case AltitudeUseType.AS_ASSIGNED:
                default:
                    spd = "";
                    break;
            }

            if (spd.CompareTo("K") == 0) spd = "";
            if (spd.CompareTo("-") == 0) spd = "";
            if (spd.CompareTo("K-") == 0) spd = "";
            if (spd.CompareTo("K+") == 0) spd = "";
            if (spd.CompareTo("") == 0) spd = "";

            spd = spd.Length > 0 ?  spd : "";

            return spd;
        }

        private string GetAltString(ProcedureLeg leg)
        {
            string alt = "";

            switch (leg.AltitudeInterpretation)
            {
                case PDM.AltitudeUseType.ABOVE_LOWER:
                    alt = leg.LowerLimitAltitude.HasValue && leg.LowerLimitAltitude.Value != Double.NaN ? GetAltitudeValue(leg.LowerLimitAltitude.Value, leg.LowerLimitAltitudeUOM) + "+" : "-";
                    break;
                case PDM.AltitudeUseType.AT_LOWER:
                    alt = leg.LowerLimitAltitude.HasValue && leg.LowerLimitAltitude.Value != Double.NaN ? GetAltitudeValue(leg.LowerLimitAltitude.Value, leg.LowerLimitAltitudeUOM) : "-";
                    break;
                case PDM.AltitudeUseType.BELOW_UPPER:
                    alt = leg.UpperLimitAltitude.HasValue && leg.UpperLimitAltitude.Value != Double.NaN ? GetAltitudeValue(leg.UpperLimitAltitude.Value, leg.UpperLimitAltitudeUOM) + "-" : "-";
                    break;
                case PDM.AltitudeUseType.BETWEEN:
                    alt = leg.UpperLimitAltitude.HasValue && leg.UpperLimitAltitude.Value != Double.NaN && leg.LowerLimitAltitude.HasValue && leg.LowerLimitAltitude.Value != Double.NaN ?
                                                                    alt = GetAltitudeValue(leg.LowerLimitAltitude.Value, leg.LowerLimitAltitudeUOM) + "+" +
                                                                             "/" + GetAltitudeValue(leg.UpperLimitAltitude.Value, leg.UpperLimitAltitudeUOM) : "-";
                    break;
                case PDM.AltitudeUseType.RECOMMENDED:
                    alt = leg.AltitudeOverrideATC.HasValue && leg.AltitudeOverrideATC.Value != Double.NaN ? "@" + GetAltitudeValue(leg.AltitudeOverrideATC.Value, leg.AltitudeUOM) : "-";
                    break;
                case PDM.AltitudeUseType.AS_ASSIGNED:
                case PDM.AltitudeUseType.OTHER:
                default:
                    alt = "";
                    break;
            }

            if (alt.CompareTo("+") == 0) alt = "";
            if (alt.CompareTo("-") == 0) alt = "";
            if (alt.CompareTo("+/") == 0) alt = "";
            if (alt.CompareTo("@") == 0) alt = "";
            if (alt.CompareTo("") == 0) alt = "";

            alt =alt.Length > 0 ? "A" + alt + "" : "";

            return alt;
        }
  
        private object[,] getLegsData(List<PDM.ProcedureTransitions> listTransition, PROC_TYPE_code procType )
        {

            List<PDM.ProcedureLeg> LegList = new List<PDM.ProcedureLeg>();
            foreach (PDM.ProcedureTransitions trans in listTransition)
            {
                LegList.AddRange(trans.Legs);
            }

            legCount = LegList.Count;


            object[,] tabData = new Object[legCount, colC];

            for (int i = 0; i < legCount; i++)
            {

                tabData[i, 0] = ((i + 1) *10).ToString(); //Serial Number
                tabData[i, 1] = LegList[i].LegTypeARINC.ToString(); //Path Descriptor

                if (LegList[i].LegTypeARINC == PDM.CodeSegmentPath.IF)
                {
                    tabData[i, 2] = LegList[i].StartPoint != null ? LegList[i].StartPoint.SegmentPointDesignator : "-"; //Waypoint Identifier
                    tabData[i, 3] = LegList[i].StartPoint != null && LegList[i].StartPoint.FlyOver ? "Y" : "-"; //Fly - over
                }
                else
                {
                    tabData[i, 2] = LegList[i].EndPoint != null ? LegList[i].EndPoint.SegmentPointDesignator : "-"; //Waypoint Identifier
                    tabData[i, 3] = LegList[i].EndPoint != null && LegList[i].EndPoint.FlyOver ? "Y" : "-"; //Fly - over
                }

                string txtCourse = "-";
                if (LegList[i].CourseType == PDM.CodeCourse.MAG_TRACK && LegList[i].Course.HasValue && MagVar.HasValue)
                {
                    string mCourse = Math.Round(LegList[i].Course.Value,0).ToString();
                    string tCourse = "(" + Math.Round(LegList[i].Course.Value + MagVar.Value, 1).ToString() + ")";
                    if (mCourse.StartsWith("NaN")) mCourse = "";
                    if (tCourse.StartsWith("(NaN)")) tCourse = "";
                    txtCourse = mCourse + " (" + tCourse + ")";
                }
                else if (LegList[i].CourseType == PDM.CodeCourse.TRUE_TRACK && LegList[i].Course.HasValue && MagVar.HasValue)
                {
                    string mCourse = Math.Round(LegList[i].Course.Value - MagVar.Value, 0).ToString();
                    string tCourse = "(" +Math.Round(LegList[i].Course.Value, 1).ToString()+")";
                    if (mCourse.StartsWith("NaN")) mCourse = "";
                    if (tCourse.StartsWith("(NaN)")) tCourse = "";
                    txtCourse = mCourse+ tCourse;
                }
                tabData[i, 4] = txtCourse; //Course °M(°T)

                tabData[i, 5] = MagVar.HasValue ? MagVar.Value.ToString() : "-"; //Magnetic Variation

                tabData[i, 6] = LegList[i].Length.HasValue ? GetLengthValue(LegList[i]) : "-"; //Distance

                tabData[i, 7] = "-"; //Turn Direction
                if (LegList[i].TurnDirection == PDM.DirectionTurnType.RIGHT) tabData[i, 7] = "R";
                else if (LegList[i].TurnDirection == PDM.DirectionTurnType.LEFT) tabData[i, 7] = "L";
               

                string txtVal ="-";

                switch (LegList[i].AltitudeInterpretation)
                {
                    case PDM.AltitudeUseType.ABOVE_LOWER:
                        txtVal = LegList[i].LowerLimitAltitude.HasValue && LegList[i].LowerLimitAltitude.Value != Double.NaN ? "+" + GetAltitudeValue(LegList[i].LowerLimitAltitude.Value, LegList[i].LowerLimitAltitudeUOM) : "-";
                        break;
                    case PDM.AltitudeUseType.AT_LOWER:
                        txtVal = LegList[i].LowerLimitAltitude.HasValue && LegList[i].LowerLimitAltitude.Value != Double.NaN ? GetAltitudeValue(LegList[i].LowerLimitAltitude.Value, LegList[i].LowerLimitAltitudeUOM) : "-";
                        break;
                    case PDM.AltitudeUseType.BELOW_UPPER:
                        txtVal = LegList[i].UpperLimitAltitude.HasValue && LegList[i].UpperLimitAltitude.Value != Double.NaN ? "-" + GetAltitudeValue(LegList[i].UpperLimitAltitude.Value, LegList[i].UpperLimitAltitudeUOM) : "-";
                        break;
                    case PDM.AltitudeUseType.BETWEEN:
                        txtVal = LegList[i].UpperLimitAltitude.HasValue && LegList[i].UpperLimitAltitude.Value != Double.NaN && LegList[i].LowerLimitAltitude.HasValue && LegList[i].LowerLimitAltitude.Value != Double.NaN ?
                                                                        txtVal = "+" + GetAltitudeValue(LegList[i].LowerLimitAltitude.Value, LegList[i].LowerLimitAltitudeUOM) +
                                                                                 "/" + GetAltitudeValue(LegList[i].UpperLimitAltitude.Value, LegList[i].UpperLimitAltitudeUOM) : "-";
                        break;
                    case PDM.AltitudeUseType.RECOMMENDED:
                        txtVal = LegList[i].AltitudeOverrideATC.HasValue && LegList[i].AltitudeOverrideATC.Value != Double.NaN ? "@" + GetAltitudeValue(LegList[i].AltitudeOverrideATC.Value, LegList[i].AltitudeUOM) : "-";
                        break;
                    case PDM.AltitudeUseType.AS_ASSIGNED:
                    case PDM.AltitudeUseType.OTHER:
                    default:
                        txtVal = "";
                        break;
                }

                if (txtVal.ToUpper().Contains("NAN")) txtVal = "-";
                if (txtVal.ToUpper().CompareTo("@") == 0) txtVal = "-";
                if (txtVal.ToUpper().CompareTo("+") == 0) txtVal = "-";
                if (txtVal.ToUpper().CompareTo("-") == 0) txtVal = "-";
                if (txtVal.ToUpper().CompareTo("/") == 0) txtVal = "-";
                tabData[i, 8] = txtVal; //Altitude

                

                txtVal = "-";
                switch (LegList[i].SpeedInterpritation)
                {
                    case PDM.AltitudeUseType.ABOVE_LOWER:
                        txtVal = LegList[i].SpeedLimit.HasValue && LegList[i].SpeedLimit.Value != Double.NaN ? "+" + GetSpeedValue(LegList[i]) : "-";
                        break;

                    case PDM.AltitudeUseType.AT_LOWER:

                    case PDM.AltitudeUseType.RECOMMENDED:
                    case AltitudeUseType.EXPECT_LOWER:
                    case AltitudeUseType.AS_ASSIGNED:
                    case AltitudeUseType.OTHER:

                        txtVal = LegList[i].SpeedLimit.HasValue && LegList[i].SpeedLimit.Value != Double.NaN ? GetSpeedValue(LegList[i]) : "-";
                        break;

                    case PDM.AltitudeUseType.BELOW_UPPER:
                        txtVal = LegList[i].SpeedLimit.HasValue && LegList[i].SpeedLimit.Value != Double.NaN ? "-" + GetSpeedValue(LegList[i]) : "-";
                        break;

                    case PDM.AltitudeUseType.BETWEEN:
                        txtVal = LegList[i].SpeedLimit.HasValue && LegList[i].SpeedLimit.Value != Double.NaN && LegList[i].SpeedLimit.HasValue && LegList[i].SpeedLimit.Value != Double.NaN ?
                                                                        txtVal = "+" + GetSpeedValue(LegList[i]) +
                                                                                 "/" + GetSpeedValue(LegList[i]) : "-";
                        break;
                    //case PDM.AltitudeUseType.RECOMMENDED:
                    //case AltitudeUseType.EXPECT_LOWER:
                    //case AltitudeUseType.AS_ASSIGNED:
                    //case AltitudeUseType.OTHER:
                    //    break;
                    default:
                        break;
                }

                if (txtVal.ToUpper().Contains("NAN")) txtVal = "-";
                if (txtVal.ToUpper().CompareTo("@") == 0) txtVal = "-";
                if (txtVal.ToUpper().CompareTo("+") == 0) txtVal = "-";
                if (txtVal.ToUpper().CompareTo("-") == 0) txtVal = "-";
                if (txtVal.ToUpper().CompareTo("/") == 0) txtVal = "-";
                tabData[i, 9] = txtVal; //Speed
                //tabData[i, 9] = LegList[i].SpeedLimit.HasValue && LegList[i].SpeedLimit.Value > 0 ? GetSpeedValue(LegList[i]): "-"; //Speed


                if (procType == PROC_TYPE_code.Approach) //VPA
                {
                    string VA = LegList[i].VerticalAngle.HasValue && LegList[i].VerticalAngle.Value != 0? Math.Round(LegList[i].VerticalAngle.Value, 1).ToString() : "";
                    if (VA.ToUpper().Contains("NAN")) tabData[i, 10] = "-"; 
                    tabData[i, 10] = VA;
                    VA = LegList[i].BankAngle.HasValue && LegList[i].BankAngle.Value != 0 ? "/" + Math.Round(LegList[i].BankAngle.Value, 1).ToString() : "";
                    if (VA.ToUpper().Contains("NAN")) tabData[i, 10] = "-";
                    tabData[i, 10] = tabData[i, 10] + VA;
                }
                else
                {
                    tabData[i, 10] = LegList[i].VerticalAngle.HasValue && LegList[i].VerticalAngle.Value != 0? Math.Round(LegList[i].VerticalAngle.Value, 1).ToString() : "-";
                    if (tabData[i, 10].ToString().ToUpper().Contains("NAN")) tabData[i, 10] = "-";
                }
                if (tabData[i, 10].ToString().CompareTo("-/NaN") == 0) tabData[i, 10] = "-";

                tabData[i, 11] = LegList[i].RequiredNavigationPerformance.HasValue ? "RNP " + (int)LegList[i].RequiredNavigationPerformance.Value : "-"; //Navigation Specification
            }

            return tabData;
        }

        private string GetLengthValue(ProcedureLeg procedureLeg)
        {
            double res = procedureLeg.Length.Value;

            switch (DistanceUOM)
            {
                case "KM":
                    res = procedureLeg.ConvertValueToMeter(res, procedureLeg.LengthUOM.ToString()) *1000;
                    break;
                case "NM":
                    res = procedureLeg.ConvertValueToMeter(res, procedureLeg.LengthUOM.ToString()) / 1852;
                    break;
                default:
                    break;
            }

            res = Math.Round(res,1);
            return res.ToString();
        }

        private string GetAltitudeValue(double AltitudeValue, UOM_DIST_VERT uOM_DIST_VERT)
        {
            PDMObject p = new PDMObject();
            double? res = AltitudeValue;
            switch (AltitudeUOM)
            {
                case "FT":
                    res = p.ConvertValueToFeet(res.Value,uOM_DIST_VERT.ToString());
                    break;
                case "M":
                    res = p.ConvertValueToMeter(res.Value,uOM_DIST_VERT.ToString());
                    break;

               
            }
            p = null;
            res = (AltitudeUOM == "M") ? ((int)Math.Round(res.Value / 50.0)) * 50 : ((int)Math.Round(res.Value / 100.0)) * 100;
            return res!=0? res.ToString() : "";

        }

        private string GetSpeedValue(ProcedureLeg procedureLeg)
        {
            double res = procedureLeg.SpeedLimit.Value;

            switch (SpeedUOM)
            {
                case "KM/H":
                    res = procedureLeg.ConvertSpeedToKilometresPerHour(res, procedureLeg.SpeedUOM.ToString());
                    break;
                case "KT":
                    res = procedureLeg.ConvertSpeedToKNOT(res, procedureLeg.SpeedUOM.ToString());
                    break;
                default:
                    break;
            }



            res =  ((int)Math.Round(res / 10.0)) * 10;
            return res.ToString();

        }

        private object[,] GetWayPoinstData(List<PDM.ProcedureTransitions> listTransition, ANCOR.MapCore.coordtype coordStringType)
        {
            Dictionary<string, PDM.SegmentPoint> wypnts = new Dictionary<string, PDM.SegmentPoint>();
            object[,] waypntsData;
            try
            {


                foreach (PDM.ProcedureTransitions trans in listTransition)
                {
                    foreach (PDM.ProcedureLeg leg in trans.Legs)
                    {
                        if (leg.StartPoint != null && leg.StartPoint.PointChoiceID!=null && !wypnts.ContainsKey(leg.StartPoint.PointChoiceID))
                            wypnts.Add(leg.StartPoint.PointChoiceID, leg.StartPoint);
                        if (leg.EndPoint != null && leg.EndPoint.PointChoiceID!=null && !wypnts.ContainsKey(leg.EndPoint.PointChoiceID))
                            wypnts.Add(leg.EndPoint.PointChoiceID, leg.EndPoint);
                    }
                }

                pointsCount = wypnts.Count;

                waypntsData = new Object[pointsCount, 2];

                //for (int i = 0; i < pointsCount; i++)
                int i = 0;
                foreach (KeyValuePair<string, PDM.SegmentPoint> pair in wypnts)
                {
                    if (pair.Value.Geo == null) pair.Value.RebuildGeo();

                    waypntsData[i, 0] = pair.Value.SegmentPointDesignator;
                    waypntsData[i, 1] = ArenaStaticProc.LatToDDMMSS(((ESRI.ArcGIS.Geometry.IPoint)pair.Value.Geo).Y.ToString(), coordStringType) + "         " + ArenaStaticProc.LonToDDMMSS(((ESRI.ArcGIS.Geometry.IPoint)pair.Value.Geo).X.ToString(), coordStringType);

                    i++;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.InnerException);
                return null;
            }
          
                return waypntsData;
            
        }


    }

   
}
