using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PDM;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Carto;
using Aran.Temporality.Common.ArcGis;
using Aran.Temporality.Common.Aim.Extension.Property;
using ESRI.ArcGIS.Geometry;
using Aran.Temporality.Common.Aim.MetaData;
using Aran.Aim.PropertyEnum;
using EsriWorkEnvironment;
using System.Windows.Forms;
using Aran.Aim;
using Aran.Aim.Enums;
using TimeSlice = Aran.Temporality.Common.MetaData.TimeSlice;
using Aran.Temporality.Common.Enum;
using AranSupport;
using System.Globalization;
using System.Threading;
using System.Xml.Serialization;
using System.IO;
using ArenaLogManager;
using System.Reflection;
using NHibernate;
using Aran.Temporality.CommonUtil.Context;
using ArenaToolBox;
using ARENA.Enums_Const;

namespace ARENA.DataLoaders
{
    public class AIXM51_DataConverter : IARENA_DATA_Converter
    {
        private Environment.Environment _CurEnvironment;

        public Environment.Environment CurEnvironment
        {
            get { return _CurEnvironment; }
            set { _CurEnvironment = value; }
        }

        
        public bool getDataFromDB { get; set; }
        public AIXM51_DataConverter()
        {
        }

        public AIXM51_DataConverter(Environment.Environment env)
        {
            this.CurEnvironment = env;
        }

        //private readonly IDictionary<string, Assembly> additional = new Dictionary<string, Assembly>();

        private System.Reflection.Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            if (args.Name.Contains("Hiber"))
                return typeof(NHibernate.Connection.ConnectionProvider).Assembly;
            return typeof(Aran.Temporality.Common.Aim.MetaData.AimFeature).Assembly;
        }



        public bool Convert_Data(IFeatureClass _FeatureClass)
        {

            
            string  dataSourceName = "";
            CultureInfo oldCI = Thread.CurrentThread.CurrentCulture;
            string EfectiveDate_CRC = "";
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");

            AlertForm alrtForm = new AlertForm();
            alrtForm.FormBorderStyle = FormBorderStyle.None;
            alrtForm.Opacity = 0.5;
            alrtForm.BackgroundImage = ArenaToolBox.Properties.Resources.ArenaSplash;

            

            alrtForm.progressBar1.ForeColor = System.Drawing.Color.FromArgb(255, 22, 76, 108);
            alrtForm.progressBar1.Maximum = 20;
            alrtForm.progressBar1.Value = 0;

            alrtForm.label1.BackColor = System.Drawing.Color.FromArgb(255, 23, 76, 107);
            alrtForm.label1.Text = "Data initialization";
            alrtForm.label1.Visible = true;
            Application.DoEvents();

            if (!getDataFromDB)
            {

                var openFileDialog1 = new OpenFileDialog { Filter = @"AIXM 5.1 snapshot (*.xml)|*.xml" };

                if (openFileDialog1.ShowDialog() != DialogResult.OK) return false;

                if (!System.Diagnostics.Debugger.IsAttached) alrtForm.Show();
                LogManager.GetLogger(GetType().Name).Info($"----------Convert data from AIXM 5.1 XML to PDM is started; Snapshot path: " + openFileDialog1.FileName);

                ////////////////////
                using (StreamReader sr = File.OpenText(openFileDialog1.FileName))
                {
                    string s = String.Empty;
                    int counter = 0;
                    while ((s = sr.ReadLine()) != null && counter <= 5)
                    {
                        if (s.Contains("effective-date"))
                        {
                            s = s.Replace("<!--", "");
                            s = s.Replace("->", "");
                            EfectiveDate_CRC = s.Trim();
                            break;
                        }
                        counter++;
                    }
                }
                ///////////////////

               

                if (!this.CurEnvironment.Data.ReadAIXM51Data(openFileDialog1.FileName))
                {
                    System.Threading.Thread.CurrentThread.CurrentCulture = oldCI;
                    alrtForm.Close();
                    return false;
                }

                alrtForm.Close();

                dataSourceName = "File name " + System.IO.Path.GetFileName(openFileDialog1.FileName) + " (AIXM 5.1)";
                this.CurEnvironment.Data.ProjectLog.Add("Snapshot AIXM 5.1" + (char)9 + openFileDialog1.FileName);

            } // get data from file
            else // get data from DB
            {
                if (!System.Diagnostics.Debugger.IsAttached) alrtForm.Show();

                AppDomain currentDomain = AppDomain.CurrentDomain;
                currentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
                if (!CurEnvironment.Data.OpenToss())
                {
                    System.Threading.Thread.CurrentThread.CurrentCulture = oldCI;
                    alrtForm.Close();
                }


                var publicSlotList = CurrentDataContext.CurrentNoAixmDataService.GetPublicSlots();

                if (publicSlotList != null)
                {

                    FormTOSS_Slots frm = new FormTOSS_Slots(publicSlotList);

                    if (frm.ShowDialog() == DialogResult.OK && frm.SelectedPublicSlot != null && frm.SelectedPrivateSlot != null)
                    {
                        Aran.Temporality.Common.Entity.PrivateSlot prSlote = frm.SelectedPrivateSlot;

                        LogManager.GetLogger(GetType().Name).Info($"----------Convert data from TOSSM AIXM 5.1 XML to PDM is started; Current User: " + CurrentDataContext.CurrentUser.Name);

                        CurrentDataContext.CurrentNoAixmDataService.SetUserActiveSlotId(CurrentDataContext.CurrentUser.Id, prSlote.Id);


                        EfectiveDate_CRC = frm.SelectedPublicSlot.EndEffectiveDate.ToString();

                        if (!this.CurEnvironment.Data.ReadAIXM51Data(frm.SelectedPublicSlot))
                        {
                            System.Threading.Thread.CurrentThread.CurrentCulture = oldCI;
                            alrtForm.Close();
                            return false;
                        }

                        alrtForm.Close();

                        CurEnvironment.Data.CloseToss();

                        dataSourceName = "TOSSM AIXM 5.1. Public slot " + frm.SelectedPublicSlot.Name + "; Private slot " + prSlote.Name;
                    }
                    else
                    {
                        System.Threading.Thread.CurrentThread.CurrentCulture = oldCI;
                        return false;
                    }
                }
            }



            this.CurEnvironment.Data.SpatialReference = ((IGeoDataset)_FeatureClass).SpatialReference;
            //// Workspace Geo
            var workspaceEdit = (IWorkspaceEdit)_FeatureClass.FeatureDataset.Workspace;

            workspaceEdit.DisableUndoRedo();

            workspaceEdit.StartEditing(false);
            workspaceEdit.StartEditOperation();

            #region ProjectInfo


            CurEnvironment.FillAirtrackTableDic(workspaceEdit);
            ITable noteTable = EsriUtils.getTableByname((IFeatureWorkspace)workspaceEdit, "Notes");

            ITable projectInfo = EsriUtils.getTableByname((IFeatureWorkspace)workspaceEdit, "ProjectInfo");
            if (projectInfo != null)
            {
                IRow row = projectInfo.CreateRow();

                int findx = -1;

                findx = row.Fields.FindField("source"); if (findx >= 0) row.set_Value(findx, dataSourceName);
                if (EfectiveDate_CRC.Length > 0)
                {
                    string[] words = EfectiveDate_CRC.Split(';');
                    findx = row.Fields.FindField("efective_date"); if (findx >= 0) row.set_Value(findx, words[0]);
                    findx = row.Fields.FindField("crs"); if (findx >= 0 && words.Length > 1) row.set_Value(findx, words[1]);
                }

                row.Store();
            }

            #endregion

            alrtForm.Close();

           if (this.CurEnvironment.Data.ProjectLog == null)  this.CurEnvironment.Data.ProjectLog = new List<string>();

            DataProcessing(); // конвертация aixm -> PDM


            alrtForm = new AlertForm();
            alrtForm.FormBorderStyle = FormBorderStyle.None;
            alrtForm.Opacity = 0.5;
            alrtForm.BackgroundImage = ArenaToolBox.Properties.Resources.ArenaSplash;
            //alrtForm.TopMost = true;

            if (!System.Diagnostics.Debugger.IsAttached) alrtForm.Show();

            alrtForm.progressBar1.ForeColor = System.Drawing.Color.FromArgb(255, 22, 76, 108);
            alrtForm.progressBar1.Maximum = 20;
            alrtForm.progressBar1.Value = 0;

            alrtForm.label1.BackColor = System.Drawing.Color.FromArgb(255, 23, 76, 107);
            alrtForm.label1.Text = "Finalization";
            alrtForm.label1.Visible = true;



            if (!System.Diagnostics.Debugger.IsAttached) alrtForm.Show();

            if (CurEnvironment.Data.PdmObjectList != null && CurEnvironment.Data.PdmObjectList.Count > 0)
                CurEnvironment.Data.PdmObjectList.RemoveAll(pdmObj => pdmObj.Lat.StartsWith("removeMe"));


            #region Construct AirspaceGeometry

            var arspList = CurEnvironment.Data.PdmObjectList.FindAll(arsp => arsp.PDM_Type == PDM_ENUM.Airspace && ((Airspace)arsp).VolumeGeometryComponents != null).ToList();

            foreach (Airspace item in arspList)
            {

                System.Diagnostics.Debug.WriteLine(item.GetObjectLabel() + " " +item.ID);
                try
                {
                    IGeometry gm = ConstructVolumeGeometry(item);

                  
                    if (gm != null)
                    {
                        item.AirspaceVolumeList[0].Geo = gm;
                        item.AirspaceVolumeList[0].BrdrGeometry = HelperClass.SetObjectToBlob(item.AirspaceVolumeList[0].Geo, "Border");
                    }
                    if (item.AirspaceVolumeList.Count > 1) item.AirspaceVolumeList.RemoveRange(1, item.AirspaceVolumeList.Count - 1);
                }
                catch (Exception ex)
                {
                    LogManager.GetLogger(ex.TargetSite.Name).Error(ex, item.GetType().ToString() + " ID: " + item.ID);

                }

            }

            #endregion

            Application.DoEvents();
           


            alrtForm.progressBar1.Maximum = CurEnvironment.Data.PdmObjectList.Count;
            alrtForm.progressBar1.Value = 0;
            alrtForm.progressBar1.Visible = true;

            foreach (var item in CurEnvironment.Data.PdmObjectList)
             {
                
               //if (alrtForm.progressBar1.Value !=30) { alrtForm.progressBar1.Value++; continue; }

                item.SourceDetail = dataSourceName;

                try
                {
                    //workspaceEdit.StartEditing(false);
                    //workspaceEdit.StartEditOperation();

                    alrtForm.label1.Text = "Finalization " + item.GetObjectLabel();
                    Application.DoEvents();
                    item.StoreToDB(CurEnvironment.Data.TableDictionary);

                    //workspaceEdit.StopEditOperation();
                    //workspaceEdit.StopEditing(true);
                }
                catch (Exception ex)
                {
                    LogManager.GetLogger(ex.TargetSite.Name).Error(ex, item.GetType().ToString() + " ID: " + item.ID);
                    
                }


                alrtForm.progressBar1.Value++;
                Application.DoEvents();

            }
            
            alrtForm.progressBar1.Maximum = CurEnvironment.Data.PdmObjectList.Count;
            alrtForm.progressBar1.Value = 0;
            var e_notes = (from element in CurEnvironment.Data.PdmObjectList where (element.Notes != null) && (element.Notes.Count > 0) select element).ToList();

            foreach (var _pdmObj in e_notes)
            {
                try
                {
                    _pdmObj.StoreNotes(noteTable);
                    alrtForm.progressBar1.Value++;
                }
                catch (Exception ex)
                {
                    LogManager.GetLogger(ex.TargetSite.Name).Error(ex, _pdmObj.GetType().ToString() + " ID: " + _pdmObj.ID + " Error in StoreNotes()");
                    
                }
            }

            alrtForm.progressBar1.Visible = false;
            alrtForm.label1.Text = "Saving... ";

            workspaceEdit.StopEditOperation();
            workspaceEdit.StopEditing(true);

            workspaceEdit.EnableUndoRedo();

            foreach (var pair in CurEnvironment.Data.AirdromeHeliportDictionary)
            {
                var adhpID = pair.Key;
                var adhp = pair.Value;

                _FeatureClass.FeatureDataset.Workspace.ExecuteSQL("UPDATE WayPoint SET WayPoint.ID_AirportHeliport = '" + adhpID + "' WHERE (WayPoint.ID_AirportHeliport='" + adhp.Designator + "')");
                _FeatureClass.FeatureDataset.Workspace.ExecuteSQL("UPDATE NavaidSystem SET NavaidSystem.ID_AirportHeliport = '" + adhpID + "' WHERE (NavaidSystem.ID_AirportHeliport='" + adhp.Designator + "')");

                if (adhp.RunwayList != null)
                {
                    foreach (Runway rwy in adhp.RunwayList)
                    {
                        if (rwy.RunwayDirectionList != null)
                        {
                            foreach (RunwayDirection rdn in rwy.RunwayDirectionList)
                            {
                                _FeatureClass.FeatureDataset.Workspace.ExecuteSQL("UPDATE NavaidSystem SET NavaidSystem.ID_RunwayDirection = '" + rdn.ID + "' WHERE (NavaidSystem.ID_RunwayDirection='" + rdn.Designator + "')");

                            }
                        }
                    }
                }

            }

            
            CurEnvironment.SetCenter_and_Projection(CurEnvironment.Data.CurrentProjectType);

            alrtForm.Close();

            System.Threading.Thread.CurrentThread.CurrentCulture = oldCI;


            return true;
        }

        public bool Convert_Permdelta(IFeatureClass _FeatureClass)
        {


            string dataSourceName = "";
            CultureInfo oldCI = Thread.CurrentThread.CurrentCulture;
            string EfectiveDate_CRC = "";
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");

            AlertForm alrtForm = new AlertForm();
            alrtForm.FormBorderStyle = FormBorderStyle.None;
            alrtForm.Opacity = 0.5;
            alrtForm.BackgroundImage = ArenaToolBox.Properties.Resources.ArenaSplash;



            alrtForm.progressBar1.ForeColor = System.Drawing.Color.FromArgb(255, 22, 76, 108);
            alrtForm.progressBar1.Maximum = 20;
            alrtForm.progressBar1.Value = 0;

            alrtForm.label1.BackColor = System.Drawing.Color.FromArgb(255, 23, 76, 107);
            alrtForm.label1.Text = "Data initialization";
            alrtForm.label1.Visible = true;
            Application.DoEvents();

            #region Load date from permdelts file

            var openFileDialog1 = new OpenFileDialog { Filter = @"AIXM 5.1 snapshot (*.xml)|*.xml" };

            if (openFileDialog1.ShowDialog() != DialogResult.OK) return false;

            if (!System.Diagnostics.Debugger.IsAttached) alrtForm.Show();
            LogManager.GetLogger(GetType().Name).Info($"----------Convert data from AIXM 5.1 XML Permdelta to PDM is started; Snapshot path: " + openFileDialog1.FileName);

            ////////////////////
            using (StreamReader sr = File.OpenText(openFileDialog1.FileName))
                {
                    string s = String.Empty;
                    int counter = 0;
                    while ((s = sr.ReadLine()) != null && counter <= 5)
                    {
                        if (s.Contains("effective-date"))
                        {
                            s = s.Replace("<!--", "");
                            s = s.Replace("->", "");
                            EfectiveDate_CRC = s.Trim();
                            break;
                        }
                        counter++;
                    }
                }
            ///////////////////


            if (!this.CurEnvironment.Data.ReadAIXM51Data(openFileDialog1.FileName))
            {
                System.Threading.Thread.CurrentThread.CurrentCulture = oldCI;
                alrtForm.Close();
                return false;
            }

            alrtForm.Close();


            // проверка что загружаемый файл PERMDELTA
            var idsLst = (from ftrst in this.CurEnvironment.Data.Intermediate_AIXM51_Arena_Features
                          where ftrst.AIXM51_Feature != null && ftrst.AIXM51_Feature.TimeSlice.Interpretation == TimeSliceInterpretationType.SNAPSHOT
                          select ftrst.AIXM51_Feature).ToList();
            if (idsLst != null && idsLst.Count > 0) return false;

            dataSourceName = "File name " + System.IO.Path.GetFileName(openFileDialog1.FileName) + " (AIXM 5.1)";

            #endregion

            this.CurEnvironment.Data.SpatialReference = ((IGeoDataset)_FeatureClass).SpatialReference;
            //// Workspace Geo
            var workspaceEdit = (IWorkspaceEdit)_FeatureClass.FeatureDataset.Workspace;

            workspaceEdit.DisableUndoRedo();

            workspaceEdit.StartEditing(false);
            workspaceEdit.StartEditOperation();

            #region ProjectInfo


            CurEnvironment.FillAirtrackTableDic(workspaceEdit);
            ITable noteTable = EsriUtils.getTableByname((IFeatureWorkspace)workspaceEdit, "Notes");

            ITable projectInfo = EsriUtils.getTableByname((IFeatureWorkspace)workspaceEdit, "ProjectInfo");
            if (projectInfo != null)
            {
                IRow row = projectInfo.CreateRow();

                int findx = -1;

                findx = row.Fields.FindField("source"); if (findx >= 0) row.set_Value(findx, dataSourceName);
                if (EfectiveDate_CRC.Length > 0)
                {
                    string[] words = EfectiveDate_CRC.Split(';');
                    findx = row.Fields.FindField("efective_date"); if (findx >= 0) row.set_Value(findx, words[0]);
                    findx = row.Fields.FindField("crs"); if (findx >= 0 && words.Length > 1) row.set_Value(findx, words[1]);
                }

                row.Store();
            }

            #endregion

            alrtForm.Close();

            DataProcessing_Permdelta(); // конвертация aixm -> PDM


            alrtForm = new AlertForm();
            alrtForm.FormBorderStyle = FormBorderStyle.None;
            alrtForm.Opacity = 0.5;
            alrtForm.BackgroundImage = ArenaToolBox.Properties.Resources.ArenaSplash;
            //alrtForm.TopMost = true;

            if (!System.Diagnostics.Debugger.IsAttached) alrtForm.Show();

            alrtForm.progressBar1.ForeColor = System.Drawing.Color.FromArgb(255, 22, 76, 108);
            alrtForm.progressBar1.Maximum = 20;
            alrtForm.progressBar1.Value = 0;

            alrtForm.label1.BackColor = System.Drawing.Color.FromArgb(255, 23, 76, 107);
            alrtForm.label1.Text = "Finalization";
            alrtForm.label1.Visible = true;



            if (!System.Diagnostics.Debugger.IsAttached) alrtForm.Show();

            if (CurEnvironment.Data.PdmObjectList != null && CurEnvironment.Data.PdmObjectList.Count > 0)
                CurEnvironment.Data.PdmObjectList.RemoveAll(pdmObj => pdmObj.Lat.StartsWith("removeMe"));


            #region Construct AirspaceGeometry

            var arspList = CurEnvironment.Data.PdmObjectList.FindAll(arsp => arsp.PDM_Type == PDM_ENUM.Airspace && ((Airspace)arsp).VolumeGeometryComponents != null).ToList();

            if (arspList != null && arspList.Count > 0)
            {
                foreach (Airspace item in arspList)
                {

                    try
                    {
                        IGeometry gm = ConstructVolumeGeometry(item);
                        if (gm != null)
                        {
                            item.AirspaceVolumeList[0].Geo = gm;
                            item.AirspaceVolumeList[0].BrdrGeometry = HelperClass.SetObjectToBlob(item.AirspaceVolumeList[0].Geo, "Border");
                        }
                        //if (item.AirspaceVolumeList.Count > 1) item.AirspaceVolumeList.RemoveRange(1, item.AirspaceVolumeList.Count - 1);
                    }
                    catch (Exception ex)
                    {
                        LogManager.GetLogger(ex.TargetSite.Name).Error(ex, item.GetType().ToString() + " ID: " + item.ID);

                    }

                }

            }
            #endregion

            Application.DoEvents();


            alrtForm.progressBar1.Maximum = CurEnvironment.Data.PdmObjectList.Count;
            alrtForm.progressBar1.Value = 0;
            alrtForm.progressBar1.Visible = true;

      
            foreach (var item in CurEnvironment.Data.PdmObjectList)
            {

                item.SourceDetail = dataSourceName;

                try
                {

                    alrtForm.label1.Text = "Finalization " + item.GetObjectLabel();
                    Application.DoEvents();
                    item.StoreToDB(CurEnvironment.Data.TableDictionary);

                }
                catch (Exception ex)
                {
                    LogManager.GetLogger(ex.TargetSite.Name).Error(ex, item.GetType().ToString() + " ID: " + item.ID);

                }


                alrtForm.progressBar1.Value++;
                Application.DoEvents();

            }

            alrtForm.progressBar1.Maximum = CurEnvironment.Data.PdmObjectList.Count;
            alrtForm.progressBar1.Value = 0;
            var e_notes = (from element in CurEnvironment.Data.PdmObjectList where (element.Notes != null) && (element.Notes.Count > 0) select element).ToList();

            foreach (var _pdmObj in e_notes)
            {
                try
                {
                    _pdmObj.StoreNotes(noteTable);
                    alrtForm.progressBar1.Value++;
                }
                catch (Exception ex)
                {
                    LogManager.GetLogger(ex.TargetSite.Name).Error(ex, _pdmObj.GetType().ToString() + " ID: " + _pdmObj.ID + " Error in StoreNotes()");

                }
            }

            alrtForm.progressBar1.Visible = false;
            alrtForm.label1.Text = "Saving... ";

            workspaceEdit.StopEditOperation();
            workspaceEdit.StopEditing(true);

            workspaceEdit.EnableUndoRedo();

     
            CurEnvironment.SetCenter_and_Projection(CurEnvironment.Data.CurrentProjectType);

            alrtForm.Close();

            System.Threading.Thread.CurrentThread.CurrentCulture = oldCI;


            return true;
        }

        private void DataProcessing()
        {


            if (this.CurEnvironment.Data.Intermediate_AIXM51_Arena_Features != null)
            {

                #region MyRegion

                var queryObjectsGroup = from MyGroup in this.CurEnvironment.Data.Intermediate_AIXM51_Arena_Features group MyGroup by new { pdm_Type = MyGroup.AIXM51_Feature.FeatureType } into GroupOfObjects orderby GroupOfObjects.Key.pdm_Type select GroupOfObjects;

                if (this.CurEnvironment.Data.ProjectLog == null)  this.CurEnvironment.Data.ProjectLog = new List<string>();

                foreach (var _Group in queryObjectsGroup)
                {

                    this.CurEnvironment.Data.ProjectLog.Add("Input feature type "  + _Group.Key.pdm_Type.ToString() + " " + _Group.Count().ToString());

                }

                #endregion

                AlertForm alrtForm = new AlertForm();
                alrtForm.FormBorderStyle = FormBorderStyle.None;
                alrtForm.Opacity = 0.5;
                alrtForm.BackgroundImage = ArenaToolBox.Properties.Resources.ArenaSplash;
                //alrtForm.TopMost = true;

                alrtForm.progressBar1.ForeColor = System.Drawing.Color.FromArgb(255, 22, 76, 108);
                alrtForm.progressBar1.Maximum = 20;
                alrtForm.progressBar1.Value = 0;

                alrtForm.label1.BackColor = System.Drawing.Color.FromArgb(255, 23, 76, 107);
                alrtForm.label1.Visible = true;


                if (!System.Diagnostics.Debugger.IsAttached) alrtForm.Show();

                ExistingGuids = new List<string>();

                alrtForm.label1.Text = "MSA";
                Application.DoEvents();

           

                alrtForm.label1.Text = "Airport, Runway, Navaids";
                Application.DoEvents();


                #region Airports/RWY/THR/NAVAID

                var adhpList = (from element in this.CurEnvironment.Data.Intermediate_AIXM51_Arena_Features
                                where (element != null) &&
                                    (element.AIXM51_Feature.FeatureType == Aran.Aim.FeatureType.AirportHeliport)
                                select element).ToList();



                var adhp_chanel = (from element in this.CurEnvironment.Data.Intermediate_AIXM51_Arena_Features
                                   where (element != null) &&
                                    (element.AIXM51_Feature.FeatureType == Aran.Aim.FeatureType.AirTrafficControlService) &&
                                   (((Aran.Aim.Features.AirTrafficControlService)element.AIXM51_Feature).ClientAirport != null) &&
                                   (((Aran.Aim.Features.AirTrafficControlService)element.AIXM51_Feature).ClientAirport.Count > 0)
                                   select element).ToList();

                var grnd_chanel = (from element in this.CurEnvironment.Data.Intermediate_AIXM51_Arena_Features
                                   where (element != null) &&
                                    (element.AIXM51_Feature.FeatureType == Aran.Aim.FeatureType.GroundTrafficControlService) &&
                                   (((Aran.Aim.Features.GroundTrafficControlService)element.AIXM51_Feature).ClientAirport != null)
                                   select element).ToList();

                var proc_chanel = (from element in this.CurEnvironment.Data.Intermediate_AIXM51_Arena_Features
                                   where (element != null) &&
                                    (element.AIXM51_Feature.FeatureType == Aran.Aim.FeatureType.AirTrafficControlService) &&
                                   (((Aran.Aim.Features.AirTrafficControlService)element.AIXM51_Feature).ClientProcedure != null)
                                   select element).ToList();

                var informationService_chanel_adhp = (from element in this.CurEnvironment.Data.Intermediate_AIXM51_Arena_Features
                                                      where (element != null) &&
                                                       (element.AIXM51_Feature.FeatureType == Aran.Aim.FeatureType.InformationService) &&
                                                      (((Aran.Aim.Features.InformationService)element.AIXM51_Feature).ClientAirport != null) &&
                                                      (((Aran.Aim.Features.InformationService)element.AIXM51_Feature).ClientAirport.Count > 0)
                                                      select element).ToList();

                ///////////////////////////////////

                var rwy_elements = (from element in CurEnvironment.Data.Intermediate_AIXM51_Arena_Features
                                    where (element != null) &&
                                        (element.AIXM51_Feature.FeatureType == Aran.Aim.FeatureType.RunwayElement) && (((Aran.Aim.Features.RunwayElement)element.AIXM51_Feature).AssociatedRunway != null)
                                    select element).ToList();



                alrtForm.progressBar1.Maximum = adhpList.Count;
                alrtForm.progressBar1.Value = 0;
                foreach (var aimADHP in adhpList)
                {
                    try
                    {
                        AirportHeliport pdmADHP = (AirportHeliport)AIM_PDM_Converter.AIM_Object_Convert(aimADHP.AIXM51_Feature, aimADHP.AixmGeo);

                        if (aimADHP.AIXM51_Feature.Identifier.ToString().StartsWith("27b53447-a161-496b-93a5-e6be9547f36e"))
                            System.Diagnostics.Debug.WriteLine("");

                        if (pdmADHP == null) continue;

                        var org = (from element in this.CurEnvironment.Data.Intermediate_AIXM51_Arena_Features
                                   where (element != null) &&
                                    (element.AIXM51_Feature.FeatureType == Aran.Aim.FeatureType.OrganisationAuthority) &&
                                   (((Aran.Aim.Features.OrganisationAuthority)element.AIXM51_Feature).Identifier != null) &&
                                       (((Aran.Aim.Features.OrganisationAuthority)element.AIXM51_Feature).Identifier.ToString().StartsWith(pdmADHP.OrganisationAuthority))
                                   select element).FirstOrDefault();
                        if (org != null)
                        {
                            pdmADHP.OrganisationAuthority = ((Aran.Aim.Features.OrganisationAuthority)org.AIXM51_Feature).Name;
                        }



                        #region NavigationSystemCheckpoint

                        var navSystemCheckPnt = (from element in CurEnvironment.Data.Intermediate_AIXM51_Arena_Features
                                                 where (element != null) &&
                                                       (element.AIXM51_Feature.FeatureType == Aran.Aim.FeatureType.CheckpointINS || element.AIXM51_Feature.FeatureType == Aran.Aim.FeatureType.CheckpointVOR)
                                                 select element).ToList();

                        if ((navSystemCheckPnt != null) && (navSystemCheckPnt.Count > 0))
                        {
                            pdmADHP.NavSystemCheckpoints = new List<NavigationSystemCheckpoint>();

                            foreach (var item in navSystemCheckPnt)
                            {
                                try
                                {
                                    if (((Aran.Aim.Features.NavigationSystemCheckpoint)item.AIXM51_Feature).AirportHeliport == null) continue;
                                    if (((Aran.Aim.Features.NavigationSystemCheckpoint)item.AIXM51_Feature).AirportHeliport.Identifier == null) continue;
                                    if (((Aran.Aim.Features.NavigationSystemCheckpoint)item.AIXM51_Feature).AirportHeliport.Identifier != aimADHP.AIXM51_Feature.Identifier) continue;
                                    var pdmNavSystem = GetNavSystem(item.AIXM51_Feature.Identifier);
                                    if (pdmNavSystem != null)
                                    {
                                        if ((pdmNavSystem as CheckpointVOR).ID_VOR != null)
                                        {
                                            var checkpointVor = pdmNavSystem as CheckpointVOR;

                                            var vor = (from element in CurEnvironment.Data.Intermediate_AIXM51_Arena_Features
                                                       where checkpointVor != null && ((element != null) &&
                                                                                                       (element.AIXM51_Feature.FeatureType == Aran.Aim.FeatureType.VOR) &&
                                                                                                       (((Aran.Aim.Features.VOR)element.AIXM51_Feature).Identifier.ToString() == checkpointVor.ID_VOR))
                                                       select element).FirstOrDefault();

                                            if (vor != null)
                                            {
                                                Aran.Aim.Features.VOR aimObj = (Aran.Aim.Features.VOR)vor.AIXM51_Feature;
                                                if (aimObj.Frequency != null)
                                                {
                                                    (pdmNavSystem as CheckpointVOR).Frequency = aimObj.Frequency.Value;
                                                    UOM_FREQ uom_freq;
                                                    if (Enum.TryParse<UOM_FREQ>(aimObj.Frequency.Uom.ToString(), out uom_freq)) (pdmNavSystem as CheckpointVOR).Frequency_UOM = uom_freq;
                                                }
                                                (pdmNavSystem as CheckpointVOR).DesignatorVOR = aimObj.Designator;
                                            }
                                        }

                                        ((NavigationSystemCheckpoint)pdmNavSystem).ID_AirportHeliport = pdmADHP.ID;

                                        pdmADHP.NavSystemCheckpoints.Add(pdmNavSystem);
                                    }

                                }
                                catch (Exception ex)
                                {
                                    LogManager.GetLogger(ex.TargetSite.Name).Error(ex, item.AIXM51_Feature.GetType().ToString() + " ID: " + item.AIXM51_Feature.Identifier);
                                }

                            }

                            if (pdmADHP.NavSystemCheckpoints != null && pdmADHP.NavSystemCheckpoints.Count <= 0)
                                pdmADHP.NavSystemCheckpoints = null;
                        }

                        #endregion

                        #region Road

                        var road = (from element in this.CurEnvironment.Data.Intermediate_AIXM51_Arena_Features
                                    where (element != null) &&
                                          (element.AIXM51_Feature.FeatureType == Aran.Aim.FeatureType.Road) &&
                                          (((Aran.Aim.Features.Road)element.AIXM51_Feature).AssociatedAirport != null) &&
                                          (((Aran.Aim.Features.Road)element.AIXM51_Feature).AssociatedAirport.Identifier == aimADHP.AIXM51_Feature.Identifier)
                                    select element).ToList();

                        if ((road != null) && (road.Count > 0))
                        {
                            pdmADHP.RoadList = new List<Road>();

                            foreach (var featureRoad in road)
                            {
                                try
                                {
                                    var aimRoad = featureRoad.AIXM51_Feature;
                                    Road pdmRoad = (Road)AIM_PDM_Converter.AIM_Object_Convert(aimRoad, featureRoad.AixmGeo);

                                    if (pdmRoad != null)
                                    {
                                        pdmRoad.ID_AirportHeliport = pdmADHP.ID;


                                        ((AirportHeliport)pdmADHP).RoadList.Add(pdmRoad);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    LogManager.GetLogger(ex.TargetSite.Name).Error(ex, featureRoad.AIXM51_Feature.GetType().ToString() + " ID: " + featureRoad.AIXM51_Feature.Identifier);
                                }
                            }

                        }

                        #endregion

                        #region NonMovementArea

                        var nonMove = (from element in this.CurEnvironment.Data.Intermediate_AIXM51_Arena_Features
                                       where (element != null) &&
                                             (element.AIXM51_Feature.FeatureType == Aran.Aim.FeatureType.NonMovementArea) &&
                                             (((Aran.Aim.Features.NonMovementArea)element.AIXM51_Feature).AssociatedAirportHeliport != null) &&
                                             (((Aran.Aim.Features.NonMovementArea)element.AIXM51_Feature).AssociatedAirportHeliport.Identifier == aimADHP.AIXM51_Feature.Identifier)
                                       select element).ToList();

                        if ((nonMove != null) && (nonMove.Count > 0))
                        {
                            pdmADHP.NonMovementAreaList = new List<NonMovementArea>();

                            foreach (var featureNonMove in nonMove)
                            {
                                try
                                {
                                    var aimNonMove = featureNonMove.AIXM51_Feature;
                                    NonMovementArea pdmNonMove = (NonMovementArea)AIM_PDM_Converter.AIM_Object_Convert(aimNonMove, featureNonMove.AixmGeo);

                                    if (pdmNonMove != null)
                                    {
                                        pdmNonMove.ID_AirportHeliport = pdmADHP.ID;


                                        ((AirportHeliport)pdmADHP).NonMovementAreaList.Add(pdmNonMove);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    LogManager.GetLogger(ex.TargetSite.Name).Error(ex, featureNonMove.AIXM51_Feature.GetType().ToString() + " ID: " + featureNonMove.AIXM51_Feature.Identifier);
                                }
                            }

                        }

                        #endregion

                        #region Unit

                        var unit = (from element in this.CurEnvironment.Data.Intermediate_AIXM51_Arena_Features
                                    where (element != null) &&
                                          (element.AIXM51_Feature.FeatureType == Aran.Aim.FeatureType.Unit) &&
                                          (((Aran.Aim.Features.Unit)element.AIXM51_Feature).AirportLocation != null) &&
                                          (((Aran.Aim.Features.Unit)element.AIXM51_Feature).AirportLocation.Identifier == aimADHP.AIXM51_Feature.Identifier)
                                    select element).ToList();

                        if ((unit != null) && (unit.Count > 0))
                        {
                            pdmADHP.UnitList = new List<Unit>();

                            foreach (var featureUnit in unit)
                            {
                                try
                                {
                                    var aimUnit = featureUnit.AIXM51_Feature;
                                    Unit pdmUnit = (Unit)AIM_PDM_Converter.AIM_Object_Convert(aimUnit, featureUnit.AixmGeo);

                                    if (pdmUnit != null)
                                    {
                                        pdmUnit.ID_AirportHeliport = pdmADHP.ID;


                                        ((AirportHeliport)pdmADHP).UnitList.Add(pdmUnit);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    LogManager.GetLogger(ex.TargetSite.Name).Error(ex, featureUnit.AIXM51_Feature.GetType().ToString() + " ID: " + featureUnit.AIXM51_Feature.Identifier);
                                }
                            }

                        }

                        #endregion

                        #region TouchDownLiftOff

                        var touchDownList = (from element in this.CurEnvironment.Data.Intermediate_AIXM51_Arena_Features
                                             where (element != null) &&
                                                   (element.AIXM51_Feature.FeatureType == Aran.Aim.FeatureType.TouchDownLiftOff) &&
                                                   (((Aran.Aim.Features.TouchDownLiftOff)element.AIXM51_Feature).AssociatedAirportHeliport != null) &&
                                                   (((Aran.Aim.Features.TouchDownLiftOff)element.AIXM51_Feature).AssociatedAirportHeliport.Identifier == aimADHP.AIXM51_Feature.Identifier)
                                             select element).ToList();

                        if ((touchDownList != null) && (touchDownList.Count > 0))
                        {
                            pdmADHP.TouchDownLiftOffList = new List<TouchDownLiftOff>();

                            foreach (var featureTouchDown in touchDownList)
                            {
                                try
                                {
                                    var aimTouchDown = featureTouchDown.AIXM51_Feature;
                                    TouchDownLiftOff pdmTouchDown = (TouchDownLiftOff)AIM_PDM_Converter.AIM_Object_Convert(aimTouchDown, featureTouchDown.AixmGeo);

                                    if (pdmTouchDown != null)
                                    {
                                        pdmTouchDown.ID_AirportHeliport = pdmADHP.ID;

                                        #region TouchDownLiftOffMarking

                                        var touchDownMarking = (from element in CurEnvironment.Data.Intermediate_AIXM51_Arena_Features
                                                                where (element != null) &&
                                                                      (element.AIXM51_Feature.FeatureType == Aran.Aim.FeatureType.TouchDownLiftOffMarking) && (((Aran.Aim.Features.TouchDownLiftOffMarking)element.AIXM51_Feature).MarkedTouchDownLiftOff != null) &&
                                                                      (((Aran.Aim.Features.TouchDownLiftOffMarking)element.AIXM51_Feature).MarkedTouchDownLiftOff.Identifier == aimTouchDown.Identifier)
                                                                select element).ToList();

                                        if ((touchDownMarking != null) && (touchDownMarking.Count > 0))
                                        {
                                            pdmTouchDown.TouchDownLiftOffMarkingList = new List<TouchDownLiftOffMarking>();

                                            foreach (var featureMark in touchDownMarking)
                                            {
                                                try
                                                {
                                                    var aimTouchMarking = featureMark.AIXM51_Feature;
                                                    TouchDownLiftOffMarking pdmTouchMark = (TouchDownLiftOffMarking)AIM_PDM_Converter.AIM_Object_Convert(aimTouchMarking, featureMark.AixmGeo);
                                                    if (pdmTouchMark != null)
                                                    {
                                                        pdmTouchMark.ID_TouchDownLiftOff = aimTouchDown.Identifier.ToString();
                                                        pdmTouchMark.ID = aimTouchMarking.Identifier.ToString();

                                                        pdmTouchDown.TouchDownLiftOffMarkingList.Add(pdmTouchMark);
                                                    }
                                                }
                                                catch (Exception ex)
                                                {
                                                    LogManager.GetLogger(ex.TargetSite.Name).Error(ex, featureMark.AIXM51_Feature.GetType().ToString() + " ID: " + featureMark.AIXM51_Feature.Identifier);
                                                }
                                            }
                                        }

                                        #endregion

                                        #region TouchDownLiftOffLightSystem

                                        var touchDownLightSystem = (from element in this.CurEnvironment.Data.Intermediate_AIXM51_Arena_Features
                                                                    where (element != null) &&
                                                                          (element.AIXM51_Feature.FeatureType == Aran.Aim.FeatureType.TouchDownLiftOffLightSystem) &&
                                                                          (((Aran.Aim.Features.TouchDownLiftOffLightSystem)element.AIXM51_Feature).LightedTouchDownLiftOff != null) &&
                                                                          (((Aran.Aim.Features.TouchDownLiftOffLightSystem)element.AIXM51_Feature).LightedTouchDownLiftOff.Identifier == aimTouchDown.Identifier)
                                                                    select element).FirstOrDefault();

                                        if (touchDownLightSystem != null)
                                        {
                                            try
                                            {
                                                TouchDownLiftOffLightSystem pdmLightSystem = (TouchDownLiftOffLightSystem)AIM_PDM_Converter.AIM_Object_Convert(touchDownLightSystem.AIXM51_Feature, touchDownLightSystem.AixmGeo);
                                                if (pdmLightSystem != null)
                                                {
                                                    pdmLightSystem.ID_TouchDownLiftOff = pdmTouchDown.ID;
                                                    pdmTouchDown.LightSystem = pdmLightSystem;
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                LogManager.GetLogger(ex.TargetSite.Name).Error(ex, touchDownLightSystem.AIXM51_Feature.GetType().ToString() + " ID: " + touchDownLightSystem.AIXM51_Feature.Identifier);
                                            }
                                        }

                                        #endregion

                                        #region GuidanceLine from TouchDownLiftOff                                

                                        var guidanceLineTD = (from element in CurEnvironment.Data.Intermediate_AIXM51_Arena_Features
                                                              where (element != null) &&
                                                                    (element.AIXM51_Feature.FeatureType == Aran.Aim.FeatureType.GuidanceLine) && (((Aran.Aim.Features.GuidanceLine)element.AIXM51_Feature).ConnectedApron != null) &&
                                                                    (((Aran.Aim.Features.GuidanceLine)element.AIXM51_Feature).ConnectedTouchDownLiftOff.Exists(t => t.Feature.Identifier == aimTouchDown.Identifier))
                                                              select element).ToList();

                                        if ((guidanceLineTD != null) && (guidanceLineTD.Count > 0))
                                        {
                                            pdmTouchDown.GuidanceLineList = new List<GuidanceLine>();

                                            foreach (var featureGuidLine in guidanceLineTD)
                                            {
                                                try
                                                {
                                                    var aimGuidLine = featureGuidLine.AIXM51_Feature;
                                                    GuidanceLine pdmGuidLine = (GuidanceLine)AIM_PDM_Converter.AIM_Object_Convert(aimGuidLine, featureGuidLine.AixmGeo);
                                                    if (pdmGuidLine != null)
                                                    {
                                                        pdmGuidLine.ID = aimGuidLine.Identifier.ToString();

                                                        pdmTouchDown.GuidanceLineList.Add(pdmGuidLine);
                                                    }
                                                }
                                                catch (Exception ex)
                                                {
                                                    LogManager.GetLogger(ex.TargetSite.Name).Error(ex, featureGuidLine.AIXM51_Feature.GetType().ToString() + " ID: " + featureGuidLine.AIXM51_Feature.Identifier);
                                                }
                                            }
                                        }

                                        #endregion

                                        #region TouchDownLiftOffSafeArea

                                        var touchDownSafeArea = (from element in this.CurEnvironment.Data.Intermediate_AIXM51_Arena_Features
                                                                 where (element != null) &&
                                                                       (element.AIXM51_Feature.FeatureType == Aran.Aim.FeatureType.TouchDownLiftOffSafeArea) &&
                                                                       (((Aran.Aim.Features.TouchDownLiftOffSafeArea)element.AIXM51_Feature).ProtectedTouchDownLiftOff != null) &&
                                                                       (((Aran.Aim.Features.TouchDownLiftOffSafeArea)element.AIXM51_Feature).ProtectedTouchDownLiftOff.Identifier == aimTouchDown.Identifier)
                                                                 select element).FirstOrDefault();

                                        if (touchDownSafeArea != null)
                                        {
                                            try
                                            {
                                                TouchDownLiftOffSafeArea pdmSafeArea = (TouchDownLiftOffSafeArea)AIM_PDM_Converter.AIM_Object_Convert(touchDownSafeArea.AIXM51_Feature, touchDownSafeArea.AixmGeo);
                                                if (pdmSafeArea != null)
                                                {
                                                    pdmSafeArea.ID_TouchDownLiftOff = pdmTouchDown.ID;
                                                    pdmTouchDown.TouchDownSafeArea = pdmSafeArea;
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                LogManager.GetLogger(ex.TargetSite.Name).Error(ex, touchDownSafeArea.AIXM51_Feature.GetType().ToString() + " ID: " + touchDownSafeArea.AIXM51_Feature.Identifier);
                                            }
                                        }

                                        #endregion

                                        ((AirportHeliport)pdmADHP).TouchDownLiftOffList.Add(pdmTouchDown);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    LogManager.GetLogger(ex.TargetSite.Name).Error(ex, featureTouchDown.AIXM51_Feature.GetType().ToString() + " ID: " + featureTouchDown.AIXM51_Feature.Identifier);
                                }
                            }

                        }

                        #endregion

                        #region AirportHotSpot

                        var airHotSpot = (from element in this.CurEnvironment.Data.Intermediate_AIXM51_Arena_Features
                                          where (element != null) &&
                                                (element.AIXM51_Feature.FeatureType == Aran.Aim.FeatureType.AirportHotSpot) &&
                                                (((Aran.Aim.Features.AirportHotSpot)element.AIXM51_Feature).AffectedAirport != null) &&
                                                (((Aran.Aim.Features.AirportHotSpot)element.AIXM51_Feature).AffectedAirport.Identifier == aimADHP.AIXM51_Feature.Identifier)
                                          select element).ToList();

                        if ((airHotSpot != null) && (airHotSpot.Count > 0))
                        {
                            pdmADHP.AirportHotSpotList = new List<AirportHotSpot>();

                            foreach (var featureAHS in airHotSpot)
                            {
                                try
                                {
                                    var aimAHS = featureAHS.AIXM51_Feature;
                                    AirportHotSpot pdmAHS = (AirportHotSpot)AIM_PDM_Converter.AIM_Object_Convert(aimAHS, featureAHS.AixmGeo);

                                    if (pdmAHS != null)
                                    {
                                        pdmAHS.ID_AirportHeliport = pdmADHP.ID;
                                        ((AirportHeliport)pdmADHP).AirportHotSpotList.Add(pdmAHS);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    LogManager.GetLogger(ex.TargetSite.Name).Error(ex, featureAHS.AIXM51_Feature.GetType().ToString() + " ID: " + featureAHS.AIXM51_Feature.Identifier);
                                }
                            }

                        }

                        #endregion

                        #region Communication chanels

                        try
                        {

                            if (adhp_chanel != null)
                            {
                                List<RadioCommunicationChanel> radioChanels = CreateADHPRadioCommunicationChanels_RadioFrequencyArea(adhp_chanel, aimADHP, pdmADHP.ID);

                                if (radioChanels != null && radioChanels.Count > 0)
                                {
                                    pdmADHP.CommunicationChanels = new List<RadioCommunicationChanel>();
                                    pdmADHP.CommunicationChanels.AddRange(radioChanels);
                                }
                                //else pdmADHP.CommunicationChanels = null;

                            }
                            if (grnd_chanel != null)
                            {

                                List<RadioCommunicationChanel> radioChanels = CreateADHPRadioCommunicationChanels_RadioFrequencyArea(grnd_chanel, aimADHP, pdmADHP.ID);

                                if (radioChanels != null && radioChanels.Count > 0)
                                {
                                    if (pdmADHP.CommunicationChanels == null) pdmADHP.CommunicationChanels = new List<RadioCommunicationChanel>();
                                    pdmADHP.CommunicationChanels.AddRange(radioChanels);
                                }
                                //else pdmADHP.CommunicationChanels = null;

                            }

                            if (informationService_chanel_adhp != null)
                            {

                                List<RadioCommunicationChanel> radioChanels = CreateADHPRadioCommunicationChanels_RadioFrequencyArea(informationService_chanel_adhp, aimADHP, pdmADHP.ID);

                                if (radioChanels != null && radioChanels.Count > 0)
                                {
                                    if (pdmADHP.CommunicationChanels == null) pdmADHP.CommunicationChanels = new List<RadioCommunicationChanel>();
                                    pdmADHP.CommunicationChanels.AddRange(radioChanels);
                                }
                                //else pdmADHP.CommunicationChanels = null;

                            }

                            if (pdmADHP.CommunicationChanels?.Count <= 0) pdmADHP.CommunicationChanels = null;


                        }
                        catch (Exception ex)
                        {
                            LogManager.GetLogger(ex.TargetSite.Name).Error(ex, "Communication chanels region");
                        }

                        #endregion

                        #region WorkArea

                        var workAreaList = (from element in this.CurEnvironment.Data.Intermediate_AIXM51_Arena_Features
                                            where (element != null) &&
                                                  (element.AIXM51_Feature.FeatureType == Aran.Aim.FeatureType.WorkArea) &&
                                                  (((Aran.Aim.Features.WorkArea)element.AIXM51_Feature).AssociatedAirportHeliport != null) &&
                                                  (((Aran.Aim.Features.WorkArea)element.AIXM51_Feature).AssociatedAirportHeliport.Identifier == aimADHP.AIXM51_Feature.Identifier)
                                            select element).ToList();

                        if ((workAreaList != null) && (workAreaList.Count > 0))
                        {
                            pdmADHP.WorkAreaList = new List<WorkArea>();

                            foreach (var featureWorkArea in workAreaList)
                            {
                                try
                                {
                                    var aimWorkArea = featureWorkArea.AIXM51_Feature;
                                    WorkArea pdmWorkArea = (WorkArea)AIM_PDM_Converter.AIM_Object_Convert(aimWorkArea, featureWorkArea.AixmGeo);

                                    if (pdmWorkArea != null)
                                    {
                                        pdmWorkArea.ID_AirportHeliport = pdmADHP.ID;
                                        ((AirportHeliport)pdmADHP).WorkAreaList.Add(pdmWorkArea);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    LogManager.GetLogger(ex.TargetSite.Name).Error(ex, featureWorkArea.AIXM51_Feature.GetType().ToString() + " ID: " + featureWorkArea.AIXM51_Feature.Identifier);
                                }
                            }

                        }

                        #endregion

                        #region RWY/THR/NAVAID/TWY/Apron

                        var adhp_rwy = (from element in this.CurEnvironment.Data.Intermediate_AIXM51_Arena_Features
                                        where (element != null) &&
                                         (element.AIXM51_Feature.FeatureType == Aran.Aim.FeatureType.Runway) &&
                                        (((Aran.Aim.Features.Runway)element.AIXM51_Feature).AssociatedAirportHeliport != null) &&
                                            (((Aran.Aim.Features.Runway)element.AIXM51_Feature).AssociatedAirportHeliport.Identifier == aimADHP.AIXM51_Feature.Identifier)
                                        select element).ToList();


                        #region RWY/THR/NAVAID/

                        if ((adhp_rwy != null) && (adhp_rwy.Count > 0))
                        {
                            pdmADHP.RunwayList = new List<Runway>();

                            foreach (var featureRWY in adhp_rwy)
                            {
                                try
                                {
                                    var aimRWY = featureRWY.AIXM51_Feature;
                                    Runway pdmRWY = (Runway)AIM_PDM_Converter.AIM_Object_Convert(aimRWY, null);

                                    if (pdmRWY != null)
                                    {

                                        pdmRWY.ID_AirportHeliport = pdmADHP.ID;


                                        #region THR (RunwayDirection)

                                        var rwy_thr = (from element in CurEnvironment.Data.Intermediate_AIXM51_Arena_Features
                                                       where (element != null) &&
                                                           (element.AIXM51_Feature.FeatureType == Aran.Aim.FeatureType.RunwayDirection) && (((Aran.Aim.Features.RunwayDirection)element.AIXM51_Feature).UsedRunway != null) &&
                                                           (((Aran.Aim.Features.RunwayDirection)element.AIXM51_Feature).UsedRunway.Identifier == aimRWY.Identifier)
                                                       select element).ToList();

                                        if ((rwy_thr != null) && (rwy_thr.Count > 0))
                                        {
                                            pdmRWY.RunwayDirectionList = new List<RunwayDirection>();

                                            foreach (var featureTHR in rwy_thr)
                                            {
                                                try
                                                {
                                                    var aimTHR = featureTHR.AIXM51_Feature;

                                                    if (aimTHR.Identifier.ToString().StartsWith("f1a8f8b7-f874-4ef8-9715-b13739a64d80"))
                                                        System.Diagnostics.Debug.WriteLine("");


                                                    #region Center Line Points (RunwayDirection Geometry)

                                                    var thr_clp = (from element in CurEnvironment.Data.Intermediate_AIXM51_Arena_Features
                                                                   where (element != null) &&
                                                                       (element.AIXM51_Feature.FeatureType == Aran.Aim.FeatureType.RunwayCentrelinePoint) &&
                                                                        (((Aran.Aim.Features.RunwayCentrelinePoint)element.AIXM51_Feature).OnRunway != null) &&
                                                                       (((Aran.Aim.Features.RunwayCentrelinePoint)element.AIXM51_Feature).OnRunway.Identifier == aimTHR.Identifier) &&
                                                                       (((Aran.Aim.Features.RunwayCentrelinePoint)element.AIXM51_Feature).Role != null) &&
                                                                       (((Aran.Aim.Features.RunwayCentrelinePoint)element.AIXM51_Feature).Role == Aran.Aim.Enums.CodeRunwayPointRole.THR)
                                                                   select element).FirstOrDefault();

                                                    //if (thr_clp == null) continue;

                                                    #endregion

                                                    RunwayDirection pdmTHR = null;

                                                    if ((thr_clp != null) && (thr_clp.AixmGeo != null) && (thr_clp.AixmGeo.Count > 0) && (thr_clp.AixmGeo[0].GeometryType == esriGeometryType.esriGeometryPoint))
                                                    {
                                                        pdmTHR = (RunwayDirection)AIM_PDM_Converter.AIM_Object_Convert(aimTHR, thr_clp.AixmGeo);

                                                        Aran.Aim.Features.RunwayCentrelinePoint aimClp = (Aran.Aim.Features.RunwayCentrelinePoint)thr_clp.AIXM51_Feature;
                                                        pdmTHR.Elev = aimClp.Location == null || aimClp.Location.Elevation == null ? 0 : aimClp.Location.Elevation.Value;

                                                        UOM_DIST_VERT uom_dist;
                                                        if ((aimClp.Location != null && aimClp.Location.Elevation != null) && (Enum.TryParse<UOM_DIST_VERT>(aimClp.Location.Elevation.Uom.ToString(), out uom_dist))) pdmTHR.Elev_UOM = uom_dist;

                                                    }
                                                    else
                                                    {
                                                        pdmTHR = (RunwayDirection)AIM_PDM_Converter.AIM_Object_Convert(aimTHR, null);
                                                    }

                                                    if (pdmTHR != null)
                                                    {
                                                        if (pdmTHR.Geo != null)
                                                        {
                                                            pdmTHR.Lat = ((IPoint)pdmTHR.Geo).Y > 0 ? ((IPoint)pdmTHR.Geo).Y.ToString() + "N" : ((IPoint)pdmTHR.Geo).Y.ToString() + "S";
                                                            pdmTHR.Lon = ((IPoint)pdmTHR.Geo).X > 0 ? ((IPoint)pdmTHR.Geo).X.ToString() + "E" : ((IPoint)pdmTHR.Geo).X.ToString() + "W";
                                                        }

                                                        pdmTHR.ClearWayLength = getClearWayLength(aimTHR.Identifier);
                                                        pdmTHR.ClearWayWidth = getClearWayWidth(aimTHR.Identifier);
                                                        pdmTHR.Stopway = getStopWaylength(aimTHR.Identifier);
                                                        pdmTHR.Uom = UOM_DIST_HORZ.M;

                                                        pdmTHR.ID_AirportHeliport = pdmADHP.ID;
                                                        pdmTHR.ID_Runway = pdmRWY.ID;

                                                        #region Navaids


                                                        var thr_nvd = (from element in CurEnvironment.Data.Intermediate_AIXM51_Arena_Features
                                                                       where (element != null) &&
                                                                           (element.AIXM51_Feature.FeatureType == Aran.Aim.FeatureType.Navaid) &&
                                                                            (((Aran.Aim.Features.Navaid)element.AIXM51_Feature).RunwayDirection != null) &&
                                                                            (((Aran.Aim.Features.Navaid)element.AIXM51_Feature).RunwayDirection.Count > 0) &&
                                                                           (((Aran.Aim.Features.Navaid)element.AIXM51_Feature).RunwayDirection.Exists(t => t.Feature.Identifier == aimTHR.Identifier))
                                                                       select element).ToList();

                                                        if ((thr_nvd != null) && (thr_nvd.Count > 0))
                                                        {
                                                            pdmTHR.Related_NavaidSystem = new List<NavaidSystem>();

                                                            foreach (var featureNav in thr_nvd)
                                                            {
                                                                try
                                                                {
                                                                    var aimnav = featureNav.AIXM51_Feature;
                                                                    NavaidSystem pdmNavSys = (NavaidSystem)AIM_PDM_Converter.AIM_Object_Convert(aimnav, null);
                                                                    if (pdmNavSys != null)
                                                                    {
                                                                        pdmNavSys.ID_AirportHeliport = pdmADHP.ID;
                                                                        pdmNavSys.ID_RunwayDirection = pdmTHR.ID;
                                                                        pdmNavSys.Components = new List<NavaidComponent>();

                                                                        #region navaidEquipment

                                                                        if ((((Aran.Aim.Features.Navaid)aimnav).NavaidEquipment != null) && (((Aran.Aim.Features.Navaid)aimnav).NavaidEquipment.Count > 0))
                                                                        {
                                                                            foreach (var item in ((Aran.Aim.Features.Navaid)aimnav).NavaidEquipment)
                                                                            {
                                                                                try
                                                                                {
                                                                                    if (item.TheNavaidEquipment != null)
                                                                                    {
                                                                                        var pdm_navEqpnt = GetComponent(item.TheNavaidEquipment.Identifier);
                                                                                        if (pdm_navEqpnt != null)
                                                                                        {
                                                                                            ((NavaidComponent)pdm_navEqpnt).ID_NavaidSystem = pdmNavSys.ID;
                                                                                            ((NavaidComponent)pdm_navEqpnt).Designator = pdmNavSys.Designator;
                                                                                            pdmNavSys.Components.Add(pdm_navEqpnt);
                                                                                        }
                                                                                    }
                                                                                }
                                                                                catch (Exception ex)
                                                                                {
                                                                                    LogManager.GetLogger(ex.TargetSite.Name).Error(ex, item.GetType().ToString() + " ID: " + item.Id);
                                                                                }
                                                                            }
                                                                        }
                                                                        #endregion


                                                                        pdmTHR.Related_NavaidSystem.Add(pdmNavSys);

                                                                    }
                                                                }
                                                                catch (Exception ex)
                                                                {
                                                                    LogManager.GetLogger(ex.TargetSite.Name).Error(ex, featureNav.AIXM51_Feature.GetType().ToString() + " ID: " + featureNav.AIXM51_Feature.Identifier);
                                                                }
                                                            }
                                                        }

                                                        #endregion

                                                        #region Center Line Points THR

                                                        var _clp = (from element in CurEnvironment.Data.Intermediate_AIXM51_Arena_Features
                                                                    where (element != null) &&
                                                                        (element.AIXM51_Feature.FeatureType == Aran.Aim.FeatureType.RunwayCentrelinePoint) &&
                                                                         (((Aran.Aim.Features.RunwayCentrelinePoint)element.AIXM51_Feature).OnRunway != null) &&
                                                                        (((Aran.Aim.Features.RunwayCentrelinePoint)element.AIXM51_Feature).OnRunway.Identifier == aimTHR.Identifier)
                                                                    select element).ToList();

                                                        if ((_clp != null) && (_clp.Count > 0))
                                                        {
                                                            pdmTHR.CenterLinePoints = new List<RunwayCenterLinePoint>();

                                                            foreach (var featureCLP in _clp)
                                                            {
                                                                try
                                                                {
                                                                    var aimCLP = featureCLP.AIXM51_Feature;


                                                                    if ((featureCLP.AixmGeo.Count > 0) && (featureCLP.AixmGeo[0].GeometryType == esriGeometryType.esriGeometryPoint))
                                                                    {
                                                                        RunwayCenterLinePoint pdmCLP = (RunwayCenterLinePoint)AIM_PDM_Converter.AIM_Object_Convert(aimCLP, featureCLP.AixmGeo);
                                                                        if (pdmCLP != null)
                                                                        {

                                                                            pdmCLP.ID_RunwayDirection = pdmTHR.ID;
                                                                            string designator = pdmTHR.Designator;
                                                                            string designSub;
                                                                            if (designator.Length > 0)
                                                                            {
                                                                                switch (designator.Length)
                                                                                {
                                                                                    case 1:
                                                                                        designSub = designator.Substring(0, 1);
                                                                                        break;
                                                                                    case 2:
                                                                                        designSub = designator.Substring(0, 2);
                                                                                        break;
                                                                                    default:
                                                                                        designSub = designator.Substring(designator.Length - 2, 2);
                                                                                        break;
                                                                                }

                                                                                int designInt;
                                                                                var res = Int32.TryParse(designSub, out designInt);
                                                                                if (res)
                                                                                {
                                                                                    if (designInt <= 18)
                                                                                    {
                                                                                        pdmCLP.Direction = CodeRVRDirectionType.LEFT;
                                                                                    }
                                                                                    else
                                                                                    {
                                                                                        pdmCLP.Direction = CodeRVRDirectionType.RIGHT;
                                                                                    }
                                                                                }
                                                                            }


                                                                            pdmTHR.CenterLinePoints.Add(pdmCLP);


                                                                            #region GuidanceLine from RwyCntrLinePoints                                

                                                                            var guidanceLine = (from element in CurEnvironment.Data.Intermediate_AIXM51_Arena_Features
                                                                                                where (element != null) &&
                                                                                                      (element.AIXM51_Feature.FeatureType == Aran.Aim.FeatureType.GuidanceLine) && (((Aran.Aim.Features.GuidanceLine)element.AIXM51_Feature).ConnectedRunwayCentrelinePoint != null) &&
                                                                                                      (((Aran.Aim.Features.GuidanceLine)element.AIXM51_Feature).ConnectedRunwayCentrelinePoint.Exists(t => t.Feature.Identifier == aimCLP.Identifier))
                                                                                                select element).ToList();

                                                                            if ((guidanceLine != null) && (guidanceLine.Count > 0))
                                                                            {
                                                                                pdmCLP.GuidanceLineList = new List<GuidanceLine>();

                                                                                foreach (var featureGuidLine in guidanceLine)
                                                                                {
                                                                                    try
                                                                                    {
                                                                                        var aimGuidLine = featureGuidLine.AIXM51_Feature;
                                                                                        GuidanceLine pdmGuidLine = (GuidanceLine)AIM_PDM_Converter.AIM_Object_Convert(aimGuidLine, featureGuidLine.AixmGeo);
                                                                                        if (pdmGuidLine != null)
                                                                                        {

                                                                                            pdmGuidLine.ID = aimGuidLine.Identifier.ToString();


                                                                                            pdmCLP.GuidanceLineList.Add(pdmGuidLine);
                                                                                        }
                                                                                    }
                                                                                    catch (Exception ex)
                                                                                    {
                                                                                        LogManager.GetLogger(ex.TargetSite.Name).Error(ex, featureGuidLine.AIXM51_Feature.GetType().ToString() + " ID: " + featureGuidLine.AIXM51_Feature.Identifier);
                                                                                    }
                                                                                }
                                                                            }

                                                                            #endregion


                                                                            if ((pdmCLP.DeclDist != null) && (pdmCLP.DeclDist.Count > 0))
                                                                                pdmTHR.RdnDeclaredDistance = pdmCLP.DeclDist;
                                                                        }
                                                                    }
                                                                }
                                                                catch (Exception ex)
                                                                {
                                                                    LogManager.GetLogger(ex.TargetSite.Name).Error(ex, featureCLP.AIXM51_Feature.GetType().ToString() + " ID: " + featureCLP.AIXM51_Feature.Identifier);
                                                                }
                                                            }

                                                        }
                                                        #endregion

                                                        #region RunwayProtectArea

                                                        var rwyProtectArea = (from element in CurEnvironment.Data.Intermediate_AIXM51_Arena_Features
                                                                              where (element != null) &&
                                                                                    (element.AIXM51_Feature.FeatureType == Aran.Aim.FeatureType.RunwayProtectArea) &&
                                                                                    (((Aran.Aim.Features.RunwayProtectArea)element.AIXM51_Feature).ProtectedRunwayDirection != null) &&
                                                                                    (((Aran.Aim.Features.RunwayProtectArea)element.AIXM51_Feature).ProtectedRunwayDirection.Identifier == aimTHR.Identifier)
                                                                              select element).ToList();

                                                        if ((rwyProtectArea != null) && (rwyProtectArea.Count > 0))
                                                        {
                                                            pdmTHR.RwyProtectArea = new List<RunwayProtectArea>();

                                                            foreach (var featureRPA in rwyProtectArea)
                                                            {
                                                                try
                                                                {
                                                                    var aimRPA = featureRPA.AIXM51_Feature;

                                                                    if ((featureRPA.AixmGeo.Count > 0) && (featureRPA.AixmGeo[0].GeometryType == esriGeometryType.esriGeometryPolygon))
                                                                    {
                                                                        RunwayProtectArea pdmRPA = (RunwayProtectArea)AIM_PDM_Converter.AIM_Object_Convert(aimRPA, featureRPA.AixmGeo);
                                                                        if (pdmRPA != null)
                                                                        {
                                                                            pdmRPA.ID_RunwayDirection = pdmTHR.ID;

                                                                            #region RunwayProtectAreaLightSystem

                                                                            var rpa_light_system = (from element in this.CurEnvironment.Data.Intermediate_AIXM51_Arena_Features
                                                                                                    where (element != null) &&
                                                                                                          (element.AIXM51_Feature.FeatureType == Aran.Aim.FeatureType.RunwayProtectAreaLightSystem) &&
                                                                                                          (((Aran.Aim.Features.RunwayProtectAreaLightSystem)element.AIXM51_Feature).LightedArea != null) &&
                                                                                                          (((Aran.Aim.Features.RunwayProtectAreaLightSystem)element.AIXM51_Feature).LightedArea.Identifier == aimRPA.Identifier)
                                                                                                    select element).FirstOrDefault();

                                                                            if (rpa_light_system != null)
                                                                            {

                                                                                RunwayProtectAreaLightSystem pdmLightSystem = (RunwayProtectAreaLightSystem)AIM_PDM_Converter.AIM_Object_Convert(rpa_light_system.AIXM51_Feature, rpa_light_system.AixmGeo);
                                                                                if (pdmLightSystem != null)
                                                                                {
                                                                                    pdmLightSystem.ID_RunwayProtectArea = pdmRPA.ID;
                                                                                    pdmRPA.LightSystem = pdmLightSystem;
                                                                                }

                                                                            }

                                                                            #endregion

                                                                            pdmTHR.RwyProtectArea.Add(pdmRPA);

                                                                        }
                                                                    }
                                                                }
                                                                catch (Exception ex)
                                                                {
                                                                    LogManager.GetLogger(ex.TargetSite.Name).Error(ex, featureRPA.AIXM51_Feature.GetType().ToString() + " ID: " + featureRPA.AIXM51_Feature.Identifier);
                                                                }
                                                            }

                                                        }

                                                        #endregion

                                                        #region RunwayVisualRange

                                                        var rwyVisualRange = (from element in CurEnvironment.Data.Intermediate_AIXM51_Arena_Features
                                                                              where (element != null) &&
                                                                                    (element.AIXM51_Feature.FeatureType == Aran.Aim.FeatureType.RunwayVisualRange) && (((Aran.Aim.Features.RunwayVisualRange)element.AIXM51_Feature).AssociatedRunwayDirection != null) &&
                                                                                    (((Aran.Aim.Features.RunwayVisualRange)element.AIXM51_Feature).AssociatedRunwayDirection.Exists(t => t.Feature.Identifier == aimTHR.Identifier))
                                                                              select element).ToList();


                                                        if ((rwyVisualRange != null) && (rwyVisualRange.Count > 0))
                                                        {
                                                            pdmTHR.RwyVisualRange = new List<RunwayVisualRange>();

                                                            foreach (var featureRVR in rwyVisualRange)
                                                            {
                                                                try
                                                                {
                                                                    var aimRVR = featureRVR.AIXM51_Feature;

                                                                    if ((featureRVR.AixmGeo.Count > 0) && (featureRVR.AixmGeo[0].GeometryType == esriGeometryType.esriGeometryPoint))
                                                                    {
                                                                        RunwayVisualRange pdmRVR = (RunwayVisualRange)AIM_PDM_Converter.AIM_Object_Convert(aimRVR, featureRVR.AixmGeo);
                                                                        if (pdmRVR != null)
                                                                        {
                                                                            pdmRVR.ID_RunwayDirection = pdmTHR.ID;
                                                                            pdmRVR.AngleDir = pdmTHR.TrueBearing;

                                                                            string designator = pdmTHR.Designator;
                                                                            string designSub = designator.Substring(0, 2);
                                                                            int designInt = Int32.Parse(designSub);
                                                                            if (designInt <= 18)
                                                                            {
                                                                                pdmRVR.Direction = CodeRVRDirectionType.LEFT;
                                                                            }
                                                                            else
                                                                            {
                                                                                pdmRVR.Direction = CodeRVRDirectionType.RIGHT;
                                                                            }
                                                                            pdmTHR.RwyVisualRange.Add(pdmRVR);

                                                                        }
                                                                    }
                                                                }
                                                                catch (Exception ex)
                                                                {
                                                                    LogManager.GetLogger(ex.TargetSite.Name).Error(ex, featureRVR.AIXM51_Feature.GetType().ToString() + " ID: " + featureRVR.AIXM51_Feature.Identifier);
                                                                }

                                                            }

                                                        }

                                                        #endregion

                                                        #region Approach Lighting system

                                                        var app_light_system = (from element in this.CurEnvironment.Data.Intermediate_AIXM51_Arena_Features
                                                                                where (element != null) &&
                                                                                 (element.AIXM51_Feature.FeatureType == Aran.Aim.FeatureType.ApproachLightingSystem) &&
                                                                                (((Aran.Aim.Features.ApproachLightingSystem)element.AIXM51_Feature).ServedRunwayDirection != null) &&
                                                                                (((Aran.Aim.Features.ApproachLightingSystem)element.AIXM51_Feature).ServedRunwayDirection.Identifier == aimTHR.Identifier)
                                                                                select element).FirstOrDefault();

                                                        if (app_light_system != null)
                                                        {

                                                            ApproachLightingSystem pdmLightSystem = (ApproachLightingSystem)AIM_PDM_Converter.AIM_Object_Convert(app_light_system.AIXM51_Feature, app_light_system.AixmGeo);
                                                            if (pdmLightSystem != null)
                                                            {
                                                                pdmLightSystem.RunwayDirection_ID = pdmTHR.ID;
                                                                pdmTHR.RdnLightSystem = pdmLightSystem;
                                                            }

                                                        }


                                                        #endregion

                                                        #region VisualGlideSlopeIndicator

                                                        var visualGlideSlope = (from element in this.CurEnvironment.Data.Intermediate_AIXM51_Arena_Features
                                                                                where (element != null) &&
                                                                                      (element.AIXM51_Feature.FeatureType == Aran.Aim.FeatureType.VisualGlideSlopeIndicator) &&
                                                                                      (((Aran.Aim.Features.VisualGlideSlopeIndicator)element.AIXM51_Feature).RunwayDirection != null) &&
                                                                                      (((Aran.Aim.Features.VisualGlideSlopeIndicator)element.AIXM51_Feature).RunwayDirection.Identifier == aimTHR.Identifier)
                                                                                select element).FirstOrDefault();

                                                        //var visualGlideSlope = (from element in this.CurEnvironment.Data.Intermediate_AIXM51_Arena_Features
                                                        //                        where (element != null) &&
                                                        //                              (element.AIXM51_Feature.FeatureType == Aran.Aim.FeatureType.VisualGlideSlopeIndicator) &&
                                                        //                              (((Aran.Aim.Features.VisualGlideSlopeIndicator)element.AIXM51_Feature).RunwayDirection != null) 
                                                        //                        select element).FirstOrDefault();

                                                        if (visualGlideSlope != null)
                                                        {

                                                            VisualGlideSlopeIndicator pdmVisualGlideSlope = (VisualGlideSlopeIndicator)AIM_PDM_Converter.AIM_Object_Convert(visualGlideSlope.AIXM51_Feature, visualGlideSlope.AixmGeo);
                                                            if (pdmVisualGlideSlope != null)
                                                            {
                                                                pdmVisualGlideSlope.ID_RunwayDirection = pdmTHR.ID;
                                                                pdmTHR.VisualGlideSlope = pdmVisualGlideSlope;
                                                            }

                                                        }


                                                        #endregion


                                                        pdmRWY.RunwayDirectionList.Add(pdmTHR);

                                                    }

                                                }
                                                catch (Exception ex)
                                                {
                                                    LogManager.GetLogger(ex.TargetSite.Name).Error(ex, featureTHR.AIXM51_Feature.GetType().ToString() + " ID: " + featureTHR.AIXM51_Feature.Identifier);
                                                }
                                            }
                                        }

                                        #endregion

                                        var runwayGeoPnt = (from elem in CurEnvironment.Data.Intermediate_AIXM51_Arena_Features
                                                            where (elem != null) &&
                                                                  (elem.AIXM51_Feature.FeatureType == Aran.Aim.FeatureType.RunwayCentrelinePoint) &&
                                                                  (((Aran.Aim.Features.RunwayCentrelinePoint)elem.AIXM51_Feature).OnRunway != null) &&
                                                                  (pdmRWY.RunwayDirectionList.Exists(t => t.ID == ((Aran.Aim.Features.RunwayCentrelinePoint)elem.AIXM51_Feature).OnRunway.Identifier.ToString())) &&
                                                                  (((Aran.Aim.Features.RunwayCentrelinePoint)elem.AIXM51_Feature).Role == CodeRunwayPointRole.THR)
                                                            select elem).ToList();

                                        if (runwayGeoPnt != null && runwayGeoPnt.Count == 2)
                                        {
                                            IPolyline rwyLine = new PolylineClass();
                                            rwyLine.FromPoint = runwayGeoPnt[0].AixmGeo[0] as IPoint;

                                            //var zAware = rwyLine.FromPoint as IZAware;
                                            //zAware.ZAware = true;
                                            //rwyLine.FromPoint.Z = 0;
                                            //var mAware = rwyLine.FromPoint as IMAware;
                                            //mAware.MAware = true;

                                            rwyLine.ToPoint = runwayGeoPnt[1].AixmGeo[0] as IPoint;

                                            //zAware = rwyLine.ToPoint as IZAware;
                                            //zAware.ZAware = true;
                                            //rwyLine.ToPoint.Z = 0;
                                            //mAware = rwyLine.ToPoint as IMAware;
                                            //mAware.MAware = true;


                                            pdmRWY.Geo = rwyLine;


                                            if (pdmRWY.Geo != null)
                                            {
                                                pdmRWY.RwyGeometry = HelperClass.SetObjectToBlob(rwyLine, "RwyCenterLine");
                                            }


                                        }

                                        #region RwyElemeent

                                        if (rwy_elements != null)
                                        {
                                            foreach (var rwyEl in rwy_elements)
                                            {
                                                try
                                                {
                                                    Aran.Aim.Features.RunwayElement _rwyEl = (Aran.Aim.Features.RunwayElement)rwyEl.AIXM51_Feature;

                                                    foreach (var _AssociatedRunway in _rwyEl.AssociatedRunway)
                                                    {

                                                        if (_AssociatedRunway.Feature.Identifier == aimRWY.Identifier)
                                                        {
                                                            if (pdmRWY.RunwayElementsList == null) pdmRWY.RunwayElementsList = new List<RunwayElement>();

                                                            RunwayElement pdmRunwayElement = (RunwayElement)AIM_PDM_Converter.AIM_Object_Convert(_rwyEl, rwyEl.AixmGeo);


                                                            if (pdmRunwayElement != null)
                                                            {
                                                                pdmRunwayElement.RwyGeometry = HelperClass.SetObjectToBlob(pdmRunwayElement.Geo, "Border");

                                                                pdmRunwayElement.Designator = pdmRWY.Designator;


                                                                pdmRWY.RunwayElementsList.Add(pdmRunwayElement);
                                                            }
                                                            break;
                                                        }
                                                    }
                                                }
                                                catch (Exception ex)
                                                {
                                                    LogManager.GetLogger(ex.TargetSite.Name).Error(ex, rwyEl.AIXM51_Feature.GetType().ToString() + " ID: " + rwyEl.AIXM51_Feature.Identifier);
                                                }
                                            }
                                        }
                                        #endregion

                                        #region RunwayMarking

                                        var runwayMarking = (from element in CurEnvironment.Data.Intermediate_AIXM51_Arena_Features
                                                             where (element != null) &&
                                                                 (element.AIXM51_Feature.FeatureType == Aran.Aim.FeatureType.RunwayMarking) && (((Aran.Aim.Features.RunwayMarking)element.AIXM51_Feature).MarkedRunway != null) &&
                                                                 (((Aran.Aim.Features.RunwayMarking)element.AIXM51_Feature).MarkedRunway.Identifier == aimRWY.Identifier)
                                                             select element).ToList();

                                        if ((runwayMarking != null) && (runwayMarking.Count > 0))
                                        {
                                            pdmRWY.RunwayMarkingList = new List<RunwayMarking>();

                                            foreach (var featureApr in runwayMarking)
                                            {
                                                try
                                                {
                                                    var aimRwyMarking = featureApr.AIXM51_Feature;
                                                    RunwayMarking pdmRwyMark = (RunwayMarking)AIM_PDM_Converter.AIM_Object_Convert(aimRwyMarking, featureApr.AixmGeo);
                                                    if (pdmRwyMark != null)
                                                    {
                                                        pdmRwyMark.ID_Runway = aimRWY.Identifier.ToString();
                                                        pdmRwyMark.ID = aimRwyMarking.Identifier.ToString();

                                                        pdmRWY.RunwayMarkingList.Add(pdmRwyMark);
                                                    }
                                                }
                                                catch (Exception ex)
                                                {
                                                    LogManager.GetLogger(ex.TargetSite.Name).Error(ex, featureApr.AIXM51_Feature.GetType().ToString() + " ID: " + featureApr.AIXM51_Feature.Identifier);

                                                }
                                            }
                                        }

                                        #endregion

                                        #region TaxiHoldingPosition

                                        var taxiholPosition = (from element in CurEnvironment.Data.Intermediate_AIXM51_Arena_Features
                                                               where (element != null) &&
                                                                     (element.AIXM51_Feature.FeatureType == Aran.Aim.FeatureType.TaxiHoldingPosition) && (((Aran.Aim.Features.TaxiHoldingPosition)element.AIXM51_Feature).ProtectedRunway != null) &&
                                                                     (((Aran.Aim.Features.TaxiHoldingPosition)element.AIXM51_Feature).ProtectedRunway.Exists(t => t.Feature.Identifier == aimRWY.Identifier))
                                                               select element).ToList();

                                        if ((taxiholPosition != null) && (taxiholPosition.Count > 0))
                                        {
                                            pdmRWY.TaxiHoldingPositionList = new List<TaxiHoldingPosition>();

                                            foreach (var featureTaxiHolPosition in taxiholPosition)
                                            {
                                                try
                                                {
                                                    var aimTaxiHolding = featureTaxiHolPosition.AIXM51_Feature;
                                                    TaxiHoldingPosition pdmTaxiHolding = (TaxiHoldingPosition)AIM_PDM_Converter.AIM_Object_Convert(aimTaxiHolding, featureTaxiHolPosition.AixmGeo);
                                                    if (pdmTaxiHolding != null)
                                                    {
                                                        pdmTaxiHolding.ID_Runway = pdmRWY.ID.ToString();
                                                        pdmTaxiHolding.RwyDesignator = pdmRWY.Designator;
                                                        pdmTaxiHolding.ID = aimTaxiHolding.Identifier.ToString();

                                                        #region TaxiHoldingPositionLightSystem

                                                        var taxiHolding_light_system = (from element in this.CurEnvironment.Data.Intermediate_AIXM51_Arena_Features
                                                                                        where (element != null) &&
                                                                                              (element.AIXM51_Feature.FeatureType == Aran.Aim.FeatureType.TaxiHoldingPositionLightSystem) &&
                                                                                              (((Aran.Aim.Features.TaxiHoldingPositionLightSystem)element.AIXM51_Feature).TaxiHolding != null) &&
                                                                                              (((Aran.Aim.Features.TaxiHoldingPositionLightSystem)element.AIXM51_Feature).TaxiHolding.Identifier == aimTaxiHolding.Identifier)
                                                                                        select element).FirstOrDefault();

                                                        if (taxiHolding_light_system != null)
                                                        {

                                                            TaxiHoldingPositionLightSystem pdmLightSystem = (TaxiHoldingPositionLightSystem)AIM_PDM_Converter.AIM_Object_Convert(taxiHolding_light_system.AIXM51_Feature, taxiHolding_light_system.AixmGeo);
                                                            if (pdmLightSystem != null)
                                                            {
                                                                pdmLightSystem.ID_TaxiHolding = pdmTaxiHolding.ID;
                                                                pdmTaxiHolding.LightSystem = pdmLightSystem;
                                                            }

                                                        }

                                                        #endregion

                                                        #region TaxiHoldingMarking

                                                        var taxiHoldingMarking = (from element in CurEnvironment.Data.Intermediate_AIXM51_Arena_Features
                                                                                  where (element != null) &&
                                                                                        (element.AIXM51_Feature.FeatureType == Aran.Aim.FeatureType.TaxiHoldingPositionMarking) && (((Aran.Aim.Features.TaxiHoldingPositionMarking)element.AIXM51_Feature).MarkedTaxiHold != null) &&
                                                                                        (((Aran.Aim.Features.TaxiHoldingPositionMarking)element.AIXM51_Feature).MarkedTaxiHold.Identifier == aimTaxiHolding.Identifier)
                                                                                  select element).ToList();

                                                        if ((taxiHoldingMarking != null) && (taxiHoldingMarking.Count > 0))
                                                        {
                                                            pdmTaxiHolding.TaxiHoldingMarkingList = new List<TaxiHoldingPositionMarking>();

                                                            foreach (var featureTaxiHoldingMark in taxiHoldingMarking)
                                                            {

                                                                var aimTaxiHoldMarking = featureTaxiHoldingMark.AIXM51_Feature;
                                                                TaxiHoldingPositionMarking pdmTaxiHoldMark = (TaxiHoldingPositionMarking)AIM_PDM_Converter.AIM_Object_Convert(aimTaxiHoldMarking, featureTaxiHoldingMark.AixmGeo);
                                                                if (pdmTaxiHoldMark != null)
                                                                {
                                                                    pdmTaxiHoldMark.ID_TaxiHolding = aimTaxiHolding.Identifier.ToString();
                                                                    pdmTaxiHoldMark.ID = aimTaxiHoldMarking.Identifier.ToString();

                                                                    pdmTaxiHolding.TaxiHoldingMarkingList.Add(pdmTaxiHoldMark);
                                                                }
                                                            }
                                                        }
                                                        #endregion

                                                        pdmRWY.TaxiHoldingPositionList.Add(pdmTaxiHolding);
                                                    }
                                                }
                                                catch (Exception ex)
                                                {
                                                    LogManager.GetLogger(ex.TargetSite.Name).Error(ex, featureTaxiHolPosition.AIXM51_Feature.GetType().ToString() + " ID: " + featureTaxiHolPosition.AIXM51_Feature.Identifier);
                                                }
                                            }
                                        }
                                        #endregion

                                        ((AirportHeliport)pdmADHP).RunwayList.Add(pdmRWY as Runway);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    LogManager.GetLogger(ex.TargetSite.Name).Error(ex, featureRWY.AIXM51_Feature.GetType().ToString() + " ID: " + featureRWY.AIXM51_Feature.Identifier);
                                }
                            }

                        }

                        #endregion

                        #region TWY

                        var adhp_twy = (from element in this.CurEnvironment.Data.Intermediate_AIXM51_Arena_Features
                                        where (element != null) &&
                                         (element.AIXM51_Feature.FeatureType == Aran.Aim.FeatureType.Taxiway) &&
                                        (((Aran.Aim.Features.Taxiway)element.AIXM51_Feature).AssociatedAirportHeliport != null) &&
                                            (((Aran.Aim.Features.Taxiway)element.AIXM51_Feature).AssociatedAirportHeliport.Identifier == aimADHP.AIXM51_Feature.Identifier)
                                        select element).ToList();

                        if ((adhp_twy != null) && (adhp_twy.Count > 0))
                        {
                            pdmADHP.TaxiwayList = new List<Taxiway>();

                            foreach (var featureTWY in adhp_twy)
                            {
                                try
                                {
                                    var aimTWY = featureTWY.AIXM51_Feature;
                                    Taxiway pdmTWY = (Taxiway)AIM_PDM_Converter.AIM_Object_Convert(aimTWY, null);

                                    if (pdmTWY != null)
                                    {
                                        pdmTWY.ID_AirportHeliport = pdmADHP.ID;

                                        #region TwyElemeent

                                        var twy_elements = (from element in CurEnvironment.Data.Intermediate_AIXM51_Arena_Features
                                                            where (element != null) &&
                                                                (element.AIXM51_Feature.FeatureType == Aran.Aim.FeatureType.TaxiwayElement) &&
                                                                (((Aran.Aim.Features.TaxiwayElement)element.AIXM51_Feature).AssociatedTaxiway != null) &&
                                                                (((Aran.Aim.Features.TaxiwayElement)element.AIXM51_Feature).AssociatedTaxiway.Identifier == aimTWY.Identifier)
                                                            select element).ToList();

                                        if (twy_elements != null)
                                        {
                                            foreach (var twyEl in twy_elements)
                                            {
                                                try
                                                {
                                                    Aran.Aim.Features.TaxiwayElement _twyEl = (Aran.Aim.Features.TaxiwayElement)twyEl.AIXM51_Feature;

                                                    var _AssociatedTaxiway = _twyEl.AssociatedTaxiway;

                                                    //if (_AssociatedTaxiway.Identifier == aimTWY.Identifier)
                                                    {
                                                        if (pdmTWY.TaxiWayElementsList == null) pdmTWY.TaxiWayElementsList = new List<TaxiwayElement>();

                                                        TaxiwayElement pdmTaxiwayElement = (TaxiwayElement)AIM_PDM_Converter.AIM_Object_Convert(_twyEl, twyEl.AixmGeo);

                                                        if (pdmTaxiwayElement != null)
                                                        {
                                                            pdmTaxiwayElement.ID_Taxiway = pdmTWY.ID;
                                                            pdmTaxiwayElement.Designator = pdmTWY.Designator;
                                                            if (pdmTaxiwayElement.Geo != null) pdmTaxiwayElement.TwyGeometry = HelperClass.SetObjectToBlob(pdmTaxiwayElement.Geo, "Border");
                                                            pdmTWY.TaxiWayElementsList.Add(pdmTaxiwayElement);
                                                        }
                                                    }
                                                }
                                                catch (Exception ex)
                                                {
                                                    LogManager.GetLogger(ex.TargetSite.Name).Error(ex, twyEl.AIXM51_Feature.GetType().ToString() + " ID: " + twyEl.AIXM51_Feature.Identifier);
                                                }

                                            }
                                        }
                                        #endregion

                                        #region TaxiwayMarking

                                        var taxiwayMarking = (from element in CurEnvironment.Data.Intermediate_AIXM51_Arena_Features
                                                              where (element != null) &&
                                                                    (element.AIXM51_Feature.FeatureType == Aran.Aim.FeatureType.TaxiwayMarking) && (((Aran.Aim.Features.TaxiwayMarking)element.AIXM51_Feature).MarkedTaxiway != null) &&
                                                                    (((Aran.Aim.Features.TaxiwayMarking)element.AIXM51_Feature).MarkedTaxiway.Identifier == aimTWY.Identifier)
                                                              select element).ToList();

                                        if ((taxiwayMarking != null) && (taxiwayMarking.Count > 0))
                                        {
                                            pdmTWY.TaxiwayMarkingList = new List<TaxiwayMarking>();

                                            foreach (var featureApr in taxiwayMarking)
                                            {

                                                var aimTwyMarking = featureApr.AIXM51_Feature;
                                                TaxiwayMarking pdmTwyMark = (TaxiwayMarking)AIM_PDM_Converter.AIM_Object_Convert(aimTwyMarking, featureApr.AixmGeo);
                                                if (pdmTwyMark != null)
                                                {
                                                    pdmTwyMark.ID_Taxiway = aimTWY.Identifier.ToString();
                                                    pdmTwyMark.ID = aimTwyMarking.Identifier.ToString();

                                                    pdmTWY.TaxiwayMarkingList.Add(pdmTwyMark);
                                                }
                                            }
                                        }

                                        #endregion

                                        #region TaxiwayLightSystem

                                        var taxiway_light_system = (from element in this.CurEnvironment.Data.Intermediate_AIXM51_Arena_Features
                                                                    where (element != null) &&
                                                                          (element.AIXM51_Feature.FeatureType == Aran.Aim.FeatureType.TaxiwayLightSystem) &&
                                                                          (((Aran.Aim.Features.TaxiwayLightSystem)element.AIXM51_Feature).LightedTaxiway != null) &&
                                                                          (((Aran.Aim.Features.TaxiwayLightSystem)element.AIXM51_Feature).LightedTaxiway.Identifier == aimTWY.Identifier)
                                                                    select element).FirstOrDefault();

                                        if (taxiway_light_system != null)
                                        {

                                            TaxiwayLightSystem pdmLightSystem = (TaxiwayLightSystem)AIM_PDM_Converter.AIM_Object_Convert(taxiway_light_system.AIXM51_Feature, taxiway_light_system.AixmGeo);
                                            if (pdmLightSystem != null)
                                            {
                                                pdmLightSystem.Taxiway_ID = pdmTWY.ID;
                                                pdmTWY.LightSystem = pdmLightSystem;
                                            }

                                        }

                                        #endregion

                                        #region DeicingArea from Taxiway

                                        var deicingArea = (from element in CurEnvironment.Data.Intermediate_AIXM51_Arena_Features
                                                           where (element != null) &&
                                                               (element.AIXM51_Feature.FeatureType == Aran.Aim.FeatureType.DeicingArea) && (((Aran.Aim.Features.DeicingArea)element.AIXM51_Feature).TaxiwayLocation != null) &&
                                                               (((Aran.Aim.Features.DeicingArea)element.AIXM51_Feature).TaxiwayLocation.Identifier == aimTWY.Identifier)
                                                           select element).ToList();

                                        if ((deicingArea != null) && (deicingArea.Count > 0))
                                        {
                                            pdmTWY.DeicingAreaList = new List<DeicingArea>();

                                            foreach (var featureDeiArea in deicingArea)
                                            {

                                                var aimDeicingArea = featureDeiArea.AIXM51_Feature;
                                                DeicingArea pdmDeicArea = (DeicingArea)AIM_PDM_Converter.AIM_Object_Convert(aimDeicingArea, featureDeiArea.AixmGeo);
                                                if (pdmDeicArea != null)
                                                {

                                                    pdmDeicArea.ID = aimDeicingArea.Identifier.ToString();

                                                    pdmTWY.DeicingAreaList.Add(pdmDeicArea);
                                                }
                                            }
                                        }


                                        #endregion

                                        #region GuidanceLine from Taxiway                                

                                        var guidanceLine = (from element in CurEnvironment.Data.Intermediate_AIXM51_Arena_Features
                                                            where (element != null) &&
                                                                  (element.AIXM51_Feature.FeatureType == Aran.Aim.FeatureType.GuidanceLine) && (((Aran.Aim.Features.GuidanceLine)element.AIXM51_Feature).ConnectedTaxiway != null) &&
                                                                  (((Aran.Aim.Features.GuidanceLine)element.AIXM51_Feature).ConnectedTaxiway.Exists(t => t.Feature.Identifier == aimTWY.Identifier))
                                                            select element).ToList();

                                        if ((guidanceLine != null) && (guidanceLine.Count > 0))
                                        {
                                            pdmTWY.GuidanceLineList = new List<GuidanceLine>();

                                            foreach (var featureGuidLine in guidanceLine)
                                            {
                                                try
                                                {
                                                    var aimGuidLine = featureGuidLine.AIXM51_Feature;
                                                    GuidanceLine pdmGuidLine = (GuidanceLine)AIM_PDM_Converter.AIM_Object_Convert(aimGuidLine, featureGuidLine.AixmGeo);
                                                    if (pdmGuidLine != null)
                                                    {


                                                        var taxiwayList = pdmGuidLine.ParentList.Where(t => t.Value == PDM_ENUM.Taxiway.ToString()).ToList();

                                                        if (taxiwayList.Count == 1)
                                                        {
                                                            var taxiway = (from element in CurEnvironment.Data.Intermediate_AIXM51_Arena_Features
                                                                           where (element != null) &&
                                                                                 (element.AIXM51_Feature.FeatureType == Aran.Aim.FeatureType.Taxiway) && (((Aran.Aim.Features.Taxiway)element.AIXM51_Feature).Identifier.ToString() == taxiwayList[0].Key)
                                                                           select element).FirstOrDefault();

                                                            var aimTaxiway = taxiway?.AIXM51_Feature;
                                                            Aran.Aim.Features.Taxiway aimTaxiObj = (Aran.Aim.Features.Taxiway)aimTaxiway;

                                                            pdmGuidLine.TaxiwayName = aimTaxiObj.Designator;
                                                        }


                                                        pdmGuidLine.ID = aimGuidLine.Identifier.ToString();

                                                        pdmTWY.GuidanceLineList.Add(pdmGuidLine);
                                                    }
                                                }
                                                catch (Exception ex)
                                                {
                                                    LogManager.GetLogger(ex.TargetSite.Name).Error(ex, featureGuidLine.AIXM51_Feature.GetType().ToString() + " ID: " + featureGuidLine.AIXM51_Feature.Identifier);
                                                }
                                            }
                                        }

                                        #endregion

                                        pdmADHP.TaxiwayList.Add(pdmTWY);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    LogManager.GetLogger(ex.TargetSite.Name).Error(ex, featureTWY.AIXM51_Feature.GetType().ToString() + " ID: " + featureTWY.AIXM51_Feature.Identifier);
                                }
                            }
                        }

                        #endregion

                        #region Apron

                        var adhp_apron = (from element in this.CurEnvironment.Data.Intermediate_AIXM51_Arena_Features
                                          where (element != null) &&
                                           (element.AIXM51_Feature.FeatureType == Aran.Aim.FeatureType.Apron) &&
                                          (((Aran.Aim.Features.Apron)element.AIXM51_Feature).AssociatedAirportHeliport != null) &&
                                              (((Aran.Aim.Features.Apron)element.AIXM51_Feature).AssociatedAirportHeliport.Identifier == aimADHP.AIXM51_Feature.Identifier)
                                          select element).ToList();

                        if ((adhp_apron != null) && (adhp_apron.Count > 0))
                        {
                            pdmADHP.ApronList = new List<Apron>();

                            foreach (var featureApron in adhp_apron)
                            {
                                var aimApron = featureApron.AIXM51_Feature;
                                Apron pdmApron = (Apron)AIM_PDM_Converter.AIM_Object_Convert(aimApron, null);

                                if (pdmApron != null)
                                {
                                    pdmApron.ID_AirportHeliport = pdmADHP.ID;


                                    #region ApronMarking

                                    var apronMarking = (from element in CurEnvironment.Data.Intermediate_AIXM51_Arena_Features
                                                        where (element != null) &&
                                                            (element.AIXM51_Feature.FeatureType == Aran.Aim.FeatureType.ApronMarking) && (((Aran.Aim.Features.ApronMarking)element.AIXM51_Feature).MarkedApron != null) &&
                                                            (((Aran.Aim.Features.ApronMarking)element.AIXM51_Feature).MarkedApron.Identifier == aimApron.Identifier)
                                                        select element).ToList();

                                    if ((apronMarking != null) && (apronMarking.Count > 0))
                                    {
                                        pdmApron.ApronMarkingList = new List<ApronMarking>();

                                        foreach (var featureApr in apronMarking)
                                        {

                                            var aimAprMarking = featureApr.AIXM51_Feature;
                                            ApronMarking pdmAprMark = (ApronMarking)AIM_PDM_Converter.AIM_Object_Convert(aimAprMarking, featureApr.AixmGeo);
                                            if (pdmAprMark != null)
                                            {
                                                pdmAprMark.ID_Apron = aimApron.Identifier.ToString();
                                                pdmAprMark.ID = aimAprMarking.Identifier.ToString();

                                                pdmApron.ApronMarkingList.Add(pdmAprMark);
                                            }
                                        }
                                    }
                                    #endregion

                                    #region ApronElement

                                    var apronElement = (from element in CurEnvironment.Data.Intermediate_AIXM51_Arena_Features
                                                        where (element != null) &&
                                                            (element.AIXM51_Feature.FeatureType == Aran.Aim.FeatureType.ApronElement) && (((Aran.Aim.Features.ApronElement)element.AIXM51_Feature).AssociatedApron != null) &&
                                                            (((Aran.Aim.Features.ApronElement)element.AIXM51_Feature).AssociatedApron.Identifier == aimApron.Identifier)
                                                        select element).ToList();

                                    if ((apronElement != null) && (apronElement.Count > 0))
                                    {
                                        pdmApron.ApronElementList = new List<ApronElement>();

                                        foreach (var featureApr in apronElement)
                                        {

                                            var aimAprElement = featureApr.AIXM51_Feature;
                                            //if (featureApr.AixmGeo.Count == 0)
                                            //    continue;
                                            ApronElement pdmAprElem = (ApronElement)AIM_PDM_Converter.AIM_Object_Convert(aimAprElement, featureApr.AixmGeo);
                                            if (pdmAprElem != null)
                                            {
                                                pdmAprElem.Apron_ID = aimApron.Identifier.ToString();
                                                pdmAprElem.ID = aimAprElement.Identifier.ToString();

                                                #region AircraftStand

                                                var airtcraftStand = (from element in CurEnvironment.Data.Intermediate_AIXM51_Arena_Features
                                                                      where (element != null) &&
                                                                            (element.AIXM51_Feature.FeatureType == Aran.Aim.FeatureType.AircraftStand) && (((Aran.Aim.Features.AircraftStand)element.AIXM51_Feature).ApronLocation != null) &&
                                                                            (((Aran.Aim.Features.AircraftStand)element.AIXM51_Feature).ApronLocation.Identifier == aimAprElement.Identifier)
                                                                      select element).ToList();

                                                if ((airtcraftStand != null) && (airtcraftStand.Count > 0))
                                                {
                                                    pdmAprElem.AircrafrStandList = new List<AircraftStand>();

                                                    foreach (var featureAircraftEl in airtcraftStand)
                                                    {
                                                        var aimAirStand = featureAircraftEl.AIXM51_Feature;

                                                        AircraftStand pdmAirStand = (AircraftStand)AIM_PDM_Converter.AIM_Object_Convert(aimAirStand, featureAircraftEl.AixmGeo);

                                                        if (pdmAirStand != null)
                                                        {
                                                            pdmAirStand.ID_ApronElement = aimAprElement.Identifier.ToString();
                                                            pdmAirStand.ID = aimAirStand.Identifier.ToString();

                                                            #region DeicingArea from AircraftStand

                                                            var deicingAreaAirStand = (from element in CurEnvironment.Data.Intermediate_AIXM51_Arena_Features
                                                                                       where (element != null) &&
                                                                                             (element.AIXM51_Feature.FeatureType == Aran.Aim.FeatureType.DeicingArea) && (((Aran.Aim.Features.DeicingArea)element.AIXM51_Feature).StandLocation != null) &&
                                                                                             (((Aran.Aim.Features.DeicingArea)element.AIXM51_Feature).StandLocation.Identifier == aimAirStand.Identifier)
                                                                                       select element).ToList();

                                                            if ((deicingAreaAirStand != null) && (deicingAreaAirStand.Count > 0))
                                                            {
                                                                pdmAirStand.DeicingAreaList = new List<DeicingArea>();

                                                                foreach (var featureDeiArea in deicingAreaAirStand)
                                                                {

                                                                    var aimDeicingArea = featureDeiArea.AIXM51_Feature;
                                                                    DeicingArea pdmDeicArea = (DeicingArea)AIM_PDM_Converter.AIM_Object_Convert(aimDeicingArea, featureDeiArea.AixmGeo);
                                                                    if (pdmDeicArea != null)
                                                                    {

                                                                        pdmDeicArea.ID = aimDeicingArea.Identifier.ToString();

                                                                        pdmAirStand.DeicingAreaList.Add(pdmDeicArea);
                                                                    }
                                                                }
                                                            }


                                                            #endregion

                                                            #region StandMarking

                                                            var standMarking = (from element in CurEnvironment.Data.Intermediate_AIXM51_Arena_Features
                                                                                where (element != null) &&
                                                                                      (element.AIXM51_Feature.FeatureType == Aran.Aim.FeatureType.StandMarking) && (((Aran.Aim.Features.StandMarking)element.AIXM51_Feature).MarkedStand != null) &&
                                                                                      (((Aran.Aim.Features.StandMarking)element.AIXM51_Feature).MarkedStand.Identifier == aimAirStand.Identifier)
                                                                                select element).ToList();

                                                            if ((standMarking != null) && (standMarking.Count > 0))
                                                            {
                                                                pdmAirStand.StandMarkingList = new List<StandMarking>();

                                                                foreach (var featureStandMark in standMarking)
                                                                {

                                                                    var aimStandMarking = featureStandMark.AIXM51_Feature;
                                                                    StandMarking pdmStandMark = (StandMarking)AIM_PDM_Converter.AIM_Object_Convert(aimStandMarking, featureStandMark.AixmGeo);
                                                                    if (pdmStandMark != null)
                                                                    {
                                                                        pdmStandMark.ID_AircraftStand = aimAirStand.Identifier.ToString();
                                                                        pdmStandMark.ID = aimStandMarking.Identifier.ToString();

                                                                        pdmAirStand.StandMarkingList.Add(pdmStandMark);
                                                                    }
                                                                }
                                                            }
                                                            #endregion

                                                            #region GuidanceLine from AircraftStand                                

                                                            var guidanceLineStand = (from element in CurEnvironment.Data.Intermediate_AIXM51_Arena_Features
                                                                                     where (element != null) &&
                                                                                           (element.AIXM51_Feature.FeatureType == Aran.Aim.FeatureType.GuidanceLine) && (((Aran.Aim.Features.GuidanceLine)element.AIXM51_Feature).ConnectedStand != null) &&
                                                                                           (((Aran.Aim.Features.GuidanceLine)element.AIXM51_Feature).ConnectedStand.Exists(t => t.Feature.Identifier == aimAirStand.Identifier))
                                                                                     select element).ToList();

                                                            if ((guidanceLineStand != null) && (guidanceLineStand.Count > 0))
                                                            {
                                                                pdmAirStand.GuidanceLineList = new List<GuidanceLine>();

                                                                foreach (var featureGuidLine in guidanceLineStand)
                                                                {

                                                                    var aimGuidLine = featureGuidLine.AIXM51_Feature;
                                                                    GuidanceLine pdmGuidLine = (GuidanceLine)AIM_PDM_Converter.AIM_Object_Convert(aimGuidLine, featureGuidLine.AixmGeo);
                                                                    if (pdmGuidLine != null)
                                                                    {

                                                                        pdmGuidLine.ID = aimGuidLine.Identifier.ToString();
                                                                        pdmAirStand.GuidanceLineList.Add(pdmGuidLine);
                                                                    }
                                                                }
                                                            }

                                                            #endregion



                                                            pdmAprElem.AircrafrStandList.Add(pdmAirStand);
                                                        }

                                                    }


                                                }

                                                #endregion


                                                pdmApron.ApronElementList.Add(pdmAprElem);
                                            }
                                        }
                                    }

                                    #endregion

                                    #region ApronLightSystem

                                    var apron_light_system = (from element in this.CurEnvironment.Data.Intermediate_AIXM51_Arena_Features
                                                              where (element != null) &&
                                                               (element.AIXM51_Feature.FeatureType == Aran.Aim.FeatureType.ApronLightSystem) &&
                                                              (((Aran.Aim.Features.ApronLightSystem)element.AIXM51_Feature).LightedApron != null) &&
                                                              (((Aran.Aim.Features.ApronLightSystem)element.AIXM51_Feature).LightedApron.Identifier == aimApron.Identifier)
                                                              select element).FirstOrDefault();

                                    if (apron_light_system != null)
                                    {

                                        ApronLightSystem pdmLightSystem = (ApronLightSystem)AIM_PDM_Converter.AIM_Object_Convert(apron_light_system.AIXM51_Feature, apron_light_system.AixmGeo);
                                        if (pdmLightSystem != null)
                                        {
                                            pdmLightSystem.Apron_ID = pdmApron.ID;
                                            pdmApron.LightSystem = pdmLightSystem;
                                        }

                                    }

                                    #endregion

                                    #region DeicingArea from Apron

                                    var deicingArea = (from element in CurEnvironment.Data.Intermediate_AIXM51_Arena_Features
                                                       where (element != null) &&
                                                           (element.AIXM51_Feature.FeatureType == Aran.Aim.FeatureType.DeicingArea) && (((Aran.Aim.Features.DeicingArea)element.AIXM51_Feature).AssociatedApron != null) &&
                                                           (((Aran.Aim.Features.DeicingArea)element.AIXM51_Feature).AssociatedApron.Identifier == aimApron.Identifier)
                                                       select element).ToList();

                                    if ((deicingArea != null) && (deicingArea.Count > 0))
                                    {
                                        pdmApron.DeicingAreaList = new List<DeicingArea>();

                                        foreach (var featureDeiArea in deicingArea)
                                        {

                                            var aimDeicingArea = featureDeiArea.AIXM51_Feature;
                                            DeicingArea pdmDeicArea = (DeicingArea)AIM_PDM_Converter.AIM_Object_Convert(aimDeicingArea, featureDeiArea.AixmGeo);
                                            if (pdmDeicArea != null)
                                            {

                                                pdmDeicArea.ID = aimDeicingArea.Identifier.ToString();

                                                pdmApron.DeicingAreaList.Add(pdmDeicArea);
                                            }
                                        }
                                    }

                                    #endregion

                                    #region GuidanceLine from Apron                                

                                    var guidanceLine = (from element in CurEnvironment.Data.Intermediate_AIXM51_Arena_Features
                                                        where (element != null) &&
                                                              (element.AIXM51_Feature.FeatureType == Aran.Aim.FeatureType.GuidanceLine) && (((Aran.Aim.Features.GuidanceLine)element.AIXM51_Feature).ConnectedApron != null) &&
                                                              (((Aran.Aim.Features.GuidanceLine)element.AIXM51_Feature).ConnectedApron.Exists(t => t.Feature.Identifier == aimApron.Identifier))
                                                        select element).ToList();

                                    if ((guidanceLine != null) && (guidanceLine.Count > 0))
                                    {
                                        pdmApron.GuidanceLineList = new List<GuidanceLine>();

                                        foreach (var featureGuidLine in guidanceLine)
                                        {

                                            var aimGuidLine = featureGuidLine.AIXM51_Feature;
                                            GuidanceLine pdmGuidLine = (GuidanceLine)AIM_PDM_Converter.AIM_Object_Convert(aimGuidLine, featureGuidLine.AixmGeo);
                                            if (pdmGuidLine != null)
                                            {
                                                pdmGuidLine.ID = aimGuidLine.Identifier.ToString();

                                                pdmApron.GuidanceLineList.Add(pdmGuidLine);
                                            }
                                        }
                                    }

                                    #endregion



                                    pdmADHP.ApronList.Add(pdmApron);
                                }
                            }
                        }

                        #endregion

                        #endregion


                        //if (pdmADHP.StoreToDB(CurEnvironment.Data.TableDictionary))
                        {
                            CurEnvironment.Data.PdmObjectList.Add(pdmADHP);
                        }
                    }
                    catch (Exception ex)
                    {
                        LogManager.GetLogger(ex.TargetSite.Name).Error(ex, aimADHP.AIXM51_Feature.GetType().ToString() + " ID: " + ((Aran.Aim.Features.Feature)aimADHP.AIXM51_Feature).Identifier);
                    }

                    alrtForm.progressBar1.Value++;
                    Application.DoEvents();
                }

                #endregion

                #region GuidanceLine All                               

                var guidanceLineAll = (from element in CurEnvironment.Data.Intermediate_AIXM51_Arena_Features
                                       where (element != null) &&
                                             (element.AIXM51_Feature.FeatureType == Aran.Aim.FeatureType.GuidanceLine)
                                       select element).ToList();

                if ((guidanceLineAll != null) && (guidanceLineAll.Count > 0))
                {

                    foreach (var featureGuidLine in guidanceLineAll)
                    {
                        try
                        {
                            var aimGuidLine = featureGuidLine.AIXM51_Feature;
                            GuidanceLine pdmGuidLine = (GuidanceLine)AIM_PDM_Converter.AIM_Object_Convert(aimGuidLine, featureGuidLine.AixmGeo);

                            if (pdmGuidLine != null)
                            {
                                if (pdmGuidLine.ParentList == null || pdmGuidLine.ParentList.Count <= 0) continue;
                                var taxiwayList = pdmGuidLine.ParentList.Where(t => t.Value == PDM_ENUM.Taxiway.ToString()).ToList();

                                if (taxiwayList.Count == 1)
                                {
                                    var taxiway = (from element in CurEnvironment.Data.Intermediate_AIXM51_Arena_Features
                                                   where (element != null) &&
                                                         (element.AIXM51_Feature.FeatureType == Aran.Aim.FeatureType.Taxiway) && (((Aran.Aim.Features.Taxiway)element.AIXM51_Feature).Identifier.ToString() == taxiwayList[0].Key)
                                                   select element).FirstOrDefault();

                                    var aimTaxiway = taxiway?.AIXM51_Feature;
                                    Aran.Aim.Features.Taxiway aimTaxiObj = (Aran.Aim.Features.Taxiway)aimTaxiway;

                                    pdmGuidLine.TaxiwayName = aimTaxiObj.Designator;
                                }

                                pdmGuidLine.ID = aimGuidLine.Identifier.ToString();

                                #region GuidanceLineLightSystem

                                var guidanceLine_light_system = (from element in this.CurEnvironment.Data.Intermediate_AIXM51_Arena_Features
                                                                 where (element != null) &&
                                                                       (element.AIXM51_Feature.FeatureType == Aran.Aim.FeatureType.GuidanceLineLightSystem) &&
                                                                       (((Aran.Aim.Features.GuidanceLineLightSystem)element.AIXM51_Feature).LightedGuidanceLine != null) &&
                                                                       (((Aran.Aim.Features.GuidanceLineLightSystem)element.AIXM51_Feature).LightedGuidanceLine.Identifier == aimGuidLine.Identifier)
                                                                 select element).FirstOrDefault();

                                if (guidanceLine_light_system != null)
                                {

                                    GuidanceLineLightSystem pdmLightSystem = (GuidanceLineLightSystem)AIM_PDM_Converter.AIM_Object_Convert(guidanceLine_light_system.AIXM51_Feature, guidanceLine_light_system.AixmGeo);
                                    if (pdmLightSystem != null)
                                    {
                                        pdmLightSystem.GuidanceLine_ID = pdmGuidLine.ID;
                                        pdmGuidLine.LightSystem = pdmLightSystem;
                                    }

                                }

                                #endregion

                                #region GuidanceLineMarking

                                var guidanceLineMarking = (from element in CurEnvironment.Data.Intermediate_AIXM51_Arena_Features
                                                           where (element != null) &&
                                                                 (element.AIXM51_Feature.FeatureType == Aran.Aim.FeatureType.GuidanceLineMarking) && (((Aran.Aim.Features.GuidanceLineMarking)element.AIXM51_Feature).MarkedGuidanceLine != null) &&
                                                                 (((Aran.Aim.Features.GuidanceLineMarking)element.AIXM51_Feature).MarkedGuidanceLine.Identifier == aimGuidLine.Identifier)
                                                           select element).ToList();

                                if ((guidanceLineMarking != null) && (guidanceLineMarking.Count > 0))
                                {
                                    pdmGuidLine.GuidanceLineMarkingList = new List<GuidanceLineMarking>();

                                    foreach (var featureGuidanceMark in guidanceLineMarking)
                                    {

                                        var aimGuidanceMarking = featureGuidanceMark.AIXM51_Feature;
                                        GuidanceLineMarking pdmGuidanceMark = (GuidanceLineMarking)AIM_PDM_Converter.AIM_Object_Convert(aimGuidanceMarking, featureGuidanceMark.AixmGeo);
                                        if (pdmGuidanceMark != null)
                                        {
                                            pdmGuidanceMark.GuidanceLine_ID = aimGuidLine.Identifier.ToString();
                                            pdmGuidanceMark.ID = aimGuidanceMarking.Identifier.ToString();

                                            pdmGuidLine.GuidanceLineMarkingList.Add(pdmGuidanceMark);
                                        }
                                    }
                                }
                                #endregion


                                CurEnvironment.Data.PdmObjectList.Add(pdmGuidLine);
                            }
                        }
                        catch (Exception ex)
                        {
                            LogManager.GetLogger(ex.TargetSite.Name).Error(ex, featureGuidLine.AIXM51_Feature.GetType().ToString() + " ID: " + featureGuidLine.AIXM51_Feature.Identifier);
                        }
                    }
                }

                #endregion

                #region DeicingArea All

                var deicingAreaAll = (from element in CurEnvironment.Data.Intermediate_AIXM51_Arena_Features
                                      where (element != null) &&
                                            (element.AIXM51_Feature.FeatureType == Aran.Aim.FeatureType.DeicingArea)
                                      select element).ToList();

                if ((deicingAreaAll != null) && (deicingAreaAll.Count > 0))
                {

                    foreach (var featureDeiArea in deicingAreaAll)
                    {
                        try
                        {
                            var aimDeicingArea = featureDeiArea.AIXM51_Feature;
                            DeicingArea pdmDeicArea = (DeicingArea)AIM_PDM_Converter.AIM_Object_Convert(aimDeicingArea, featureDeiArea.AixmGeo);
                            if (pdmDeicArea != null)
                            {

                                pdmDeicArea.ID = aimDeicingArea.Identifier.ToString();

                                #region DeicingAreaMarking

                                var deicAreaMarking = (from element in CurEnvironment.Data.Intermediate_AIXM51_Arena_Features
                                                       where (element != null) &&
                                                             (element.AIXM51_Feature.FeatureType == Aran.Aim.FeatureType.DeicingAreaMarking) && (((Aran.Aim.Features.DeicingAreaMarking)element.AIXM51_Feature).MarkedDeicingArea != null) &&
                                                             (((Aran.Aim.Features.DeicingAreaMarking)element.AIXM51_Feature).MarkedDeicingArea.Identifier == aimDeicingArea.Identifier)
                                                       select element).ToList();

                                if (deicAreaMarking != null && deicAreaMarking.Count > 0)
                                {
                                    pdmDeicArea.DeicingAreaMarkingList = new List<DeicingAreaMarking>();

                                    foreach (var featureDeiAreaMark in deicAreaMarking)
                                    {
                                        var aimDeicAreaMark = featureDeiAreaMark.AIXM51_Feature;

                                        DeicingAreaMarking pdmDeicAreaMark = (DeicingAreaMarking)AIM_PDM_Converter.AIM_Object_Convert(aimDeicAreaMark, featureDeiAreaMark.AixmGeo);
                                        if (pdmDeicAreaMark != null)
                                        {
                                            pdmDeicAreaMark.DeicingArea_ID = aimDeicingArea.Identifier.ToString();
                                            pdmDeicAreaMark.ID = aimDeicAreaMark.Identifier.ToString();

                                            pdmDeicArea.DeicingAreaMarkingList.Add(pdmDeicAreaMark);
                                        }
                                    }
                                }
                                #endregion

                                CurEnvironment.Data.PdmObjectList.Add(pdmDeicArea);
                            }
                        }
                        catch (Exception ex)
                        {
                            LogManager.GetLogger(ex.TargetSite.Name).Error(ex, featureDeiArea.AIXM51_Feature.GetType().ToString() + " ID: " + featureDeiArea.AIXM51_Feature.Identifier);
                        }
                    }
                }

                #endregion

                #region Navaids

                var _nvd = (from element in CurEnvironment.Data.Intermediate_AIXM51_Arena_Features
                            where (element != null) &&
                                (element.AIXM51_Feature.FeatureType == Aran.Aim.FeatureType.Navaid) &&
                                 ((((Aran.Aim.Features.Navaid)element.AIXM51_Feature).RunwayDirection == null) || (((Aran.Aim.Features.Navaid)element.AIXM51_Feature).RunwayDirection.Count == 0))
                            select element).ToList();

                if ((_nvd != null) && (_nvd.Count > 0))
                {
                    alrtForm.progressBar1.Maximum = _nvd.Count;
                    alrtForm.progressBar1.Value = 0;

                    foreach (var featureNav in _nvd)
                    {
                        try
                        {
                            var aimnav = featureNav.AIXM51_Feature;

                            NavaidSystem pdmNavSys = (NavaidSystem)AIM_PDM_Converter.AIM_Object_Convert(aimnav, null);
                            if (pdmNavSys != null)
                            {
                                pdmNavSys.Components = new List<NavaidComponent>();

                                #region navaidEquipment

                                if ((((Aran.Aim.Features.Navaid)aimnav).NavaidEquipment != null) && (((Aran.Aim.Features.Navaid)aimnav).NavaidEquipment.Count > 0))
                                {
                                    foreach (var item in ((Aran.Aim.Features.Navaid)aimnav).NavaidEquipment)
                                    {

                                        try
                                        {
                                            if (item.TheNavaidEquipment == null) continue;
                                            var pdm_navEqpnt = GetComponent(item.TheNavaidEquipment.Identifier);
                                            if (pdm_navEqpnt != null)
                                            {
                                                ((NavaidComponent)pdm_navEqpnt).ID_NavaidSystem = pdmNavSys.ID;
                                                pdmNavSys.Components.Add(pdm_navEqpnt);
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            LogManager.GetLogger(ex.TargetSite.Name).Error(ex, item.GetType().ToString() + " ID: " + item.Id);
                                        }
                                    }
                                }
                                #endregion

                                //if (pdmNavSys.StoreToDB(CurEnvironment.Data.TableDictionary))
                                {
                                    CurEnvironment.Data.PdmObjectList.Add(pdmNavSys);
                                }

                            }
                            else
                            {
                                System.Diagnostics.Debug.WriteLine("null " + aimnav.Identifier.ToString());
                            }

                            alrtForm.progressBar1.Value++;
                            Application.DoEvents();
                        }
                        catch (Exception ex)
                        {
                            LogManager.GetLogger(ex.TargetSite.Name).Error(ex, featureNav.AIXM51_Feature.GetType().ToString() + " ID: " + featureNav.AIXM51_Feature.Identifier);
                        }
                    }
                }

                #endregion

                #region SafeAiltitudeArea

                var MSAList = (from element in this.CurEnvironment.Data.Intermediate_AIXM51_Arena_Features
                               where (element != null) &&
                                   (element.AIXM51_Feature.FeatureType == Aran.Aim.FeatureType.SafeAltitudeArea)
                               select element).ToList();

                alrtForm.progressBar1.Maximum = MSAList.Count;
                alrtForm.progressBar1.Value = 0;

                foreach (var featureMSA in MSAList)
                {
                    try
                    {
                        #region

                        var aimMSA = featureMSA.AIXM51_Feature;

                        SafeAltitudeArea pdmMSA = (SafeAltitudeArea)AIM_PDM_Converter.AIM_Object_Convert(aimMSA, null);
                        if (pdmMSA == null) continue;
                        PDMObject msaCentrPnt = GetSegmentPoint(((Aran.Aim.Features.SafeAltitudeArea)aimMSA).CentrePoint.Choice.ToString(), ((Aran.Aim.Features.SafeAltitudeArea)aimMSA).CentrePoint.AimingPoint.Identifier);

                        if (msaCentrPnt != null)
                        {
                            PDMObject _obj = DefinePointSegmentDesignator(((SegmentPoint)msaCentrPnt).PointChoice, msaCentrPnt.ID);

                            string vorTp = "";
                            if (_obj != null && _obj.PDM_Type == PDM_ENUM.NavaidSystem)
                            {
                                NavaidSystem ns = (NavaidSystem)_obj;

                                foreach (var cmpnts in ns.Components)
                                {
                                    if (cmpnts.PDM_Type == PDM_ENUM.VOR)
                                    {
                                        vorTp = ((VOR)cmpnts).VorType.HasValue ? ((VOR)cmpnts).VorType.ToString() : "";
                                        break;
                                    }
                                }
                            }

                            pdmMSA.CentrePoint = new RouteSegmentPoint
                            {
                                ID = Guid.NewGuid().ToString(),
                                Route_LEG_ID = msaCentrPnt.ID,
                                PointRole = ProcedureFixRoleType.OTHER,
                                PointUse = ProcedureSegmentPointUse.ARC_CENTER,
                                IsWaypoint = false,
                                PointChoice = ((SegmentPoint)msaCentrPnt).PointChoice,
                                SegmentPointDesignator = ((SegmentPoint)msaCentrPnt).PointChoiceID + " " + vorTp,

                            };
                        }
                        CurEnvironment.Data.PdmObjectList.Add(pdmMSA);

                        #endregion

                    }
                    catch (Exception ex)
                    {
                        LogManager.GetLogger(ex.TargetSite.Name).Error(ex, featureMSA.AIXM51_Feature.GetType().ToString() + " ID: " + featureMSA.AIXM51_Feature.Identifier);
                    }

                    alrtForm.progressBar1.Value++;
                    Application.DoEvents();
                }

                #endregion


                alrtForm.label1.Text = "Designated Points";
                Application.DoEvents();

                #region Waypoint

                var wypList = (from element in this.CurEnvironment.Data.Intermediate_AIXM51_Arena_Features
                               where (element != null) &&
                                   (element.AIXM51_Feature.FeatureType == Aran.Aim.FeatureType.DesignatedPoint)
                               select element).ToList();

                alrtForm.progressBar1.Maximum = wypList.Count;
                alrtForm.progressBar1.Value = 0;

                foreach (var featureWYP in wypList)
                {

                    try
                    {
                        var aimDPN = featureWYP.AIXM51_Feature;

                        if ((featureWYP.AixmGeo?.Count > 0) && (featureWYP.AixmGeo[0].GeometryType == esriGeometryType.esriGeometryPoint))
                        {
                            WayPoint pdmWYP = (WayPoint)AIM_PDM_Converter.AIM_Object_Convert(aimDPN, featureWYP.AixmGeo);

                            if (pdmWYP == null) continue;

                            //if (pdmWYP.StoreToDB(CurEnvironment.Data.TableDictionary))
                            {
                                CurEnvironment.Data.PdmObjectList.Add(pdmWYP);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        LogManager.GetLogger(ex.TargetSite.Name).Error(ex, featureWYP.AIXM51_Feature.GetType().ToString() + " ID: " + featureWYP.AIXM51_Feature.Identifier);

                    }

                    alrtForm.progressBar1.Value++;
                    Application.DoEvents();

                }


                #endregion

                alrtForm.label1.Text = "Holdings";
                #region Holding 


                var holdingList = (from element in this.CurEnvironment.Data.Intermediate_AIXM51_Arena_Features
                                   where (element != null) &&
                                       (element.AIXM51_Feature.FeatureType == Aran.Aim.FeatureType.HoldingPattern)
                                   select element).ToList();

                alrtForm.progressBar1.Maximum = holdingList.Count;
                alrtForm.progressBar1.Value = 0;

                foreach (var hlng in holdingList)
                {

                    try
                    {
                        var aimHLNG = (Aran.Aim.Features.HoldingPattern)hlng.AIXM51_Feature;
                        //if (aimHLNG.Identifier.ToString().StartsWith("85da273e-08a8-4d3e-9619-21046c1b325b"))
                        //    System.Diagnostics.Debug.WriteLine("");

                        HoldingPattern pdmHolding = (HoldingPattern)AIM_PDM_Converter.AIM_Object_Convert(aimHLNG, hlng.AixmGeo);

                        if (pdmHolding != null)
                        {
                            pdmHolding.HodingBorder = HelperClass.SetObjectToBlob(pdmHolding.Geo, "Border");
                            CreateHoldingPoint(aimHLNG, hlng.AixmGeo, ref pdmHolding);

                            CurEnvironment.Data.PdmObjectList.Add(pdmHolding);
                        }
                    }
                    catch (Exception ex)
                    {
                        LogManager.GetLogger(ex.TargetSite.Name).Error(ex, hlng.AIXM51_Feature.GetType().ToString() + " ID: " + hlng.AIXM51_Feature.Identifier);

                    }

                    alrtForm.progressBar1.Value++;
                    Application.DoEvents();
                }

                #endregion


                alrtForm.label1.Text = "Vertical Structures";
                Application.DoEvents();

                #region Vertical structure

                var obsList = (from element in this.CurEnvironment.Data.Intermediate_AIXM51_Arena_Features
                               where (element != null) &&
                                   (element.AIXM51_Feature.FeatureType == Aran.Aim.FeatureType.VerticalStructure)
                               select element).ToList();

                var area1List = (from element in this.CurEnvironment.Data.Intermediate_AIXM51_Arena_Features
                                 where (element != null) &&
                                     (element.AIXM51_Feature.FeatureType == Aran.Aim.FeatureType.ObstacleArea) &&
                                     (((Aran.Aim.Features.ObstacleArea)element.AIXM51_Feature).Type.HasValue) &&
                                     (((Aran.Aim.Features.ObstacleArea)element.AIXM51_Feature).Type.Value == CodeObstacleArea.AREA1)
                                 select element).ToList();

                var area2List = (from element in this.CurEnvironment.Data.Intermediate_AIXM51_Arena_Features
                                 where (element != null) &&
                                     (element.AIXM51_Feature.FeatureType == Aran.Aim.FeatureType.ObstacleArea) &&
                                     (((Aran.Aim.Features.ObstacleArea)element.AIXM51_Feature).Type.HasValue) &&
                                     (((Aran.Aim.Features.ObstacleArea)element.AIXM51_Feature).Type.Value == CodeObstacleArea.AREA2)
                                 select element).ToList();

                var area2AList = (from element in this.CurEnvironment.Data.Intermediate_AIXM51_Arena_Features
                                  where (element != null) &&
                                      (element.AIXM51_Feature.FeatureType == Aran.Aim.FeatureType.ObstacleArea) &&
                                      (((Aran.Aim.Features.ObstacleArea)element.AIXM51_Feature).Type.HasValue) &&
                                      (((Aran.Aim.Features.ObstacleArea)element.AIXM51_Feature).Type.Value == CodeObstacleArea.OTHER_AREA2A)
                                  select element).ToList();

                var area2BList = (from element in this.CurEnvironment.Data.Intermediate_AIXM51_Arena_Features
                                  where (element != null) &&
                                      (element.AIXM51_Feature.FeatureType == Aran.Aim.FeatureType.ObstacleArea) &&
                                      (((Aran.Aim.Features.ObstacleArea)element.AIXM51_Feature).Type.HasValue) &&
                                      (((Aran.Aim.Features.ObstacleArea)element.AIXM51_Feature).Type.Value == CodeObstacleArea.OTHER_AREA2B)
                                  select element).ToList();

                var area2CList = (from element in this.CurEnvironment.Data.Intermediate_AIXM51_Arena_Features
                                  where (element != null) &&
                                      (element.AIXM51_Feature.FeatureType == Aran.Aim.FeatureType.ObstacleArea) &&
                                      (((Aran.Aim.Features.ObstacleArea)element.AIXM51_Feature).Type.HasValue) &&
                                      (((Aran.Aim.Features.ObstacleArea)element.AIXM51_Feature).Type.Value == CodeObstacleArea.OTHER_AREA2C)
                                  select element).ToList();

                var area2DList = (from element in this.CurEnvironment.Data.Intermediate_AIXM51_Arena_Features
                                  where (element != null) &&
                                      (element.AIXM51_Feature.FeatureType == Aran.Aim.FeatureType.ObstacleArea) &&
                                      (((Aran.Aim.Features.ObstacleArea)element.AIXM51_Feature).Type.HasValue) &&
                                      (((Aran.Aim.Features.ObstacleArea)element.AIXM51_Feature).Type.Value == CodeObstacleArea.OTHER_AREA2D)
                                  select element).ToList();

                var areaTAKEOFFCLIMBList = (from element in this.CurEnvironment.Data.Intermediate_AIXM51_Arena_Features
                                            where (element != null) &&
                                                (element.AIXM51_Feature.FeatureType == Aran.Aim.FeatureType.ObstacleArea) &&
                                                (((Aran.Aim.Features.ObstacleArea)element.AIXM51_Feature).Type.HasValue) &&
                                                (((Aran.Aim.Features.ObstacleArea)element.AIXM51_Feature).Type.Value == CodeObstacleArea.OTHER_TAKEOFFCLIMB)
                                            select element).ToList();

                var areaTAKEOFFFLIGHTPATHAREAList = (from element in this.CurEnvironment.Data.Intermediate_AIXM51_Arena_Features
                                                     where (element != null) &&
                                                         (element.AIXM51_Feature.FeatureType == Aran.Aim.FeatureType.ObstacleArea) &&
                                                         (((Aran.Aim.Features.ObstacleArea)element.AIXM51_Feature).Type.HasValue) &&
                                                         (((Aran.Aim.Features.ObstacleArea)element.AIXM51_Feature).Type.Value == CodeObstacleArea.OTHER_TAKEOFFFLIGHTPATHAREA)
                                                     select element).ToList();



                alrtForm.progressBar1.Maximum = obsList.Count;
                alrtForm.progressBar1.Value = 0;

                foreach (var featureObs in obsList)
                {
                    //continue;

                    try
                    {
                        var aimObs = featureObs.AIXM51_Feature;

                        if (aimObs.Identifier.ToString().StartsWith("37800d87-d464-47da-8770-26b147319cd9"))
                            System.Diagnostics.Debug.WriteLine("");

                        if (!ItisNeedObject(aimObs)) continue;

                        if (featureObs.AixmGeo.Count > 0)
                        {


                            VerticalStructure pdmOBS = (VerticalStructure)AIM_PDM_Converter.AIM_Object_Convert(aimObs, featureObs.AixmGeo);
                            if (pdmOBS == null)
                                continue;


                            foreach (var item in pdmOBS.Parts)
                            {
                                if (item.Geo != null)
                                {
                                    item.VertexType = item.Geo.GeometryType.ToString();
                                    //item.Vertex = HelperClass.SetObjectToBlob(item.Geo, "Vertex");

                                }

                            }

                            #region SetObstacleAreaType

                            if (area1List != null && area1List.Count > 0)
                            {
                                CodeObstacleAreaType obsareaTp = SetObstacleAreaValue(area1List, CodeObstacleAreaType.AREA1, pdmOBS.ID);
                                if (obsareaTp != CodeObstacleAreaType.OTHER)
                                {
                                    if (pdmOBS.ObstacleAreaType == null) pdmOBS.ObstacleAreaType = new List<CodeObstacleAreaType>();
                                    pdmOBS.ObstacleAreaType.Add(obsareaTp);
                                }
                            }

                            if (area2List != null && area2List.Count > 0)
                            {
                                CodeObstacleAreaType obsareaTp = SetObstacleAreaValue(area2List, CodeObstacleAreaType.AREA2, pdmOBS.ID);
                                if (obsareaTp != CodeObstacleAreaType.OTHER)
                                {
                                    if (pdmOBS.ObstacleAreaType == null) pdmOBS.ObstacleAreaType = new List<CodeObstacleAreaType>();
                                    pdmOBS.ObstacleAreaType.Add(obsareaTp);
                                }
                            }

                            if (area2AList != null && area2AList.Count > 0)
                            {
                                CodeObstacleAreaType obsareaTp = SetObstacleAreaValue(area2AList, CodeObstacleAreaType.AREA2A, pdmOBS.ID);
                                if (obsareaTp != CodeObstacleAreaType.OTHER)
                                {
                                    if (pdmOBS.ObstacleAreaType == null) pdmOBS.ObstacleAreaType = new List<CodeObstacleAreaType>();
                                    pdmOBS.ObstacleAreaType.Add(obsareaTp);
                                }
                            }

                            if (area2BList != null && area2BList.Count > 0)
                            {
                                CodeObstacleAreaType obsareaTp = SetObstacleAreaValue(area2BList, CodeObstacleAreaType.AREA2B, pdmOBS.ID);
                                if (obsareaTp != CodeObstacleAreaType.OTHER)
                                {
                                    if (pdmOBS.ObstacleAreaType == null) pdmOBS.ObstacleAreaType = new List<CodeObstacleAreaType>();
                                    pdmOBS.ObstacleAreaType.Add(obsareaTp);
                                }
                            }

                            if (area2CList != null && area2CList.Count > 0)
                            {
                                CodeObstacleAreaType obsareaTp = SetObstacleAreaValue(area2CList, CodeObstacleAreaType.AREA2C, pdmOBS.ID);
                                if (obsareaTp != CodeObstacleAreaType.OTHER)
                                {
                                    if (pdmOBS.ObstacleAreaType == null) pdmOBS.ObstacleAreaType = new List<CodeObstacleAreaType>();
                                    pdmOBS.ObstacleAreaType.Add(obsareaTp);
                                }
                            }

                            if (area2DList != null && area2DList.Count > 0)
                            {
                                CodeObstacleAreaType obsareaTp = SetObstacleAreaValue(area2DList, CodeObstacleAreaType.AREA2D, pdmOBS.ID);
                                if (obsareaTp != CodeObstacleAreaType.OTHER)
                                {
                                    if (pdmOBS.ObstacleAreaType == null) pdmOBS.ObstacleAreaType = new List<CodeObstacleAreaType>();
                                    pdmOBS.ObstacleAreaType.Add(obsareaTp);
                                }
                            }

                            if (areaTAKEOFFCLIMBList != null && areaTAKEOFFCLIMBList.Count > 0)
                            {
                                CodeObstacleAreaType obsareaTp = SetObstacleAreaValue(areaTAKEOFFCLIMBList, CodeObstacleAreaType.TAKEOFFCLIMB, pdmOBS.ID);
                                if (obsareaTp != CodeObstacleAreaType.OTHER)
                                {
                                    if (pdmOBS.ObstacleAreaType == null) pdmOBS.ObstacleAreaType = new List<CodeObstacleAreaType>();
                                    pdmOBS.ObstacleAreaType.Add(obsareaTp);
                                }
                            }

                            if (areaTAKEOFFFLIGHTPATHAREAList != null && areaTAKEOFFFLIGHTPATHAREAList.Count > 0)
                            {
                                CodeObstacleAreaType obsareaTp = SetObstacleAreaValue(areaTAKEOFFFLIGHTPATHAREAList, CodeObstacleAreaType.TAKEOFFFLIGHTPATHAREA, pdmOBS.ID);
                                if (obsareaTp != CodeObstacleAreaType.OTHER)
                                {
                                    if (pdmOBS.ObstacleAreaType == null) pdmOBS.ObstacleAreaType = new List<CodeObstacleAreaType>();
                                    pdmOBS.ObstacleAreaType.Add(obsareaTp);
                                }
                            }

                            #endregion

                            //if (pdmOBS.StoreToDB(CurEnvironment.Data.TableDictionary))
                            {
                                CurEnvironment.Data.PdmObjectList.Add(pdmOBS);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        LogManager.GetLogger(ex.TargetSite.Name).Error(ex, featureObs.AIXM51_Feature.GetType().ToString() + " ID: " + featureObs.AIXM51_Feature.Identifier);
                    }

                    alrtForm.progressBar1.Value++;
                    Application.DoEvents();
                }


                #endregion


                alrtForm.label1.Text = "Airspaces";
                Application.DoEvents();
                #region Airspace

                var arspsList = (from element in this.CurEnvironment.Data.Intermediate_AIXM51_Arena_Features
                                 where (element != null) &&
                                     (element.AIXM51_Feature.FeatureType == Aran.Aim.FeatureType.Airspace)
                                 select element).ToList();

                var arsps_chanel = (from element in this.CurEnvironment.Data.Intermediate_AIXM51_Arena_Features
                                    where (element != null) &&
                                     (element.AIXM51_Feature.FeatureType == Aran.Aim.FeatureType.AirTrafficControlService) &&
                                    (((Aran.Aim.Features.AirTrafficControlService)element.AIXM51_Feature).ClientAirspace != null) &&
                                    (((Aran.Aim.Features.AirTrafficControlService)element.AIXM51_Feature).ClientAirspace.Count > 0)
                                    select element).ToList();

                var informationService_chanel = (from element in this.CurEnvironment.Data.Intermediate_AIXM51_Arena_Features
                                                 where (element != null) &&
                                                  (element.AIXM51_Feature.FeatureType == Aran.Aim.FeatureType.InformationService) &&
                                                 (((Aran.Aim.Features.InformationService)element.AIXM51_Feature).ClientAirspace != null) &&
                                                 (((Aran.Aim.Features.InformationService)element.AIXM51_Feature).ClientAirspace.Count > 0)
                                                 select element).ToList();

                alrtForm.progressBar1.Maximum = arspsList.Count;
                alrtForm.progressBar1.Value = 0;

                foreach (var featureARSPS in arspsList)
                {

                    try
                    {

                        //if (featureARSPS.AIXM51_Feature.Identifier.ToString().StartsWith("61ef4f81-4451-4b44-90cf-b9f58d3788bb"))
                        //    System.Diagnostics.Debug.WriteLine("");
                        //if (featureARSPS.AIXM51_Feature.Identifier.ToString().StartsWith("5b00efaa-b763-4c93-9394-a9bcc0807792"))
                        //    System.Diagnostics.Debug.WriteLine("");

                        var aimARSP = featureARSPS.AIXM51_Feature;
                        if (!ItisNeedObject(aimARSP)) continue;

                        Airspace pdmARSPS = (Airspace)AIM_PDM_Converter.AIM_Object_Convert(aimARSP, null);

                        if (pdmARSPS == null) continue;

                        #region Volume

                        pdmARSPS.AirspaceVolumeList = new List<AirspaceVolume>();

                        //for (int i = 0; i <= featureARSPS.AixmGeo.Count - 1; i++)
                        for (int i = 0; i <= ((Aran.Aim.Features.Airspace)aimARSP).GeometryComponent?.Count - 1; i++)
                        {
                            Aran.Aim.Features.AirspaceVolume aimArsp_vol = null;
                            try
                            {
                                var esri_gm = featureARSPS.AixmGeo != null && featureARSPS.AixmGeo.Count > 0 && i <= featureARSPS.AixmGeo.Count - 1 ? featureARSPS.AixmGeo[i] : new PointClass { X = 0, Y = 0 };

                                //if ((esri_gm != null) && (esri_gm.GeometryType == esriGeometryType.esriGeometryPolygon))
                                {
                                    aimArsp_vol = (((Aran.Aim.Features.Airspace)aimARSP).GeometryComponent[i].TheAirspaceVolume);
                                    List<IGeometry> geoList = new List<IGeometry>();
                                    geoList.Add(esri_gm);
                                    AirspaceVolume pdmARSPS_Vol = (AirspaceVolume)AIM_PDM_Converter.AIM_Object_Convert(aimArsp_vol, geoList);

                                    if (pdmARSPS_Vol == null) continue;
                                    pdmARSPS_Vol.ID = pdmARSPS.ID + "-" + (i + 1).ToString();

                                    pdmARSPS_Vol.ID_Airspace = pdmARSPS.ID;
                                    pdmARSPS_Vol.TxtName = ((Aran.Aim.Features.Airspace)aimARSP).Name;
                                    pdmARSPS_Vol.CodeId = ((Aran.Aim.Features.Airspace)aimARSP).Designator;
                                    //if (pdmARSPS_Vol.Geo != null && pdmARSPS_Vol.Geo.GeometryType == esriGeometryType.esriGeometryPolygon && ((IPointCollection)pdmARSPS_Vol.Geo).PointCount < 500)
                                    //    pdmARSPS_Vol.BrdrGeometry = HelperClass.SetObjectToBlob(pdmARSPS_Vol.Geo, "Border");

                                    if (pdmARSPS_Vol.Geo != null)
                                    {
                                        pdmARSPS_Vol.BrdrGeometry = HelperClass.SetObjectToBlob(pdmARSPS_Vol.Geo, "Border");
                                    }

                                    if (pdmARSPS.Class != null && pdmARSPS.Class.Count > 0)
                                    {
                                        foreach (var cls in pdmARSPS.Class)
                                        {
                                            pdmARSPS_Vol.CodeClass = pdmARSPS_Vol.CodeClass + cls + " ";
                                        }

                                    }
                                    pdmARSPS.Lat = "";

                                    pdmARSPS.AirspaceVolumeList.Add(pdmARSPS_Vol);

                                    if (((Aran.Aim.Features.Airspace)aimARSP).Type.HasValue)
                                    {
                                        AirspaceType _uom = AirspaceType.OTHER;
                                        var cType = ArenaStatic.ArenaStaticProc.airspaceCodeType_to_AirspaceType(((Aran.Aim.Features.Airspace)aimARSP).Type.Value.ToString());

                                        Enum.TryParse<AirspaceType>(cType, out _uom);
                                        pdmARSPS_Vol.CodeType = _uom;
                                    }
                                    else
                                        pdmARSPS_Vol.CodeType = AirspaceType.OTHER;



                                }
                            }
                            catch (Exception ex)
                            {
                                LogManager.GetLogger(ex.TargetSite.Name).Error(ex, aimArsp_vol.GetType().ToString() + " ID: " + aimArsp_vol.Id);

                            }
                        }

                        #endregion

                        #region Communication chanels

                        if (arsps_chanel != null)
                        {

                            foreach (var cnl in arsps_chanel)
                            {
                                try
                                {

                                    Aran.Aim.Features.AirTrafficControlService _chanel = (Aran.Aim.Features.AirTrafficControlService)cnl.AIXM51_Feature;

                                    foreach (var clnArspc in _chanel.ClientAirspace)
                                    {

                                        if (clnArspc.Feature.Identifier.CompareTo(aimARSP.Identifier) != 0) continue;

                                        foreach (var _radioComm in _chanel.RadioCommunication)
                                        {

                                            var radio_com = (from element in this.CurEnvironment.Data.Intermediate_AIXM51_Arena_Features
                                                             where (element != null) &&
                                                              (element.AIXM51_Feature.FeatureType == Aran.Aim.FeatureType.RadioCommunicationChannel) &&
                                                                 (((Aran.Aim.Features.RadioCommunicationChannel)element.AIXM51_Feature).Identifier.CompareTo(_radioComm.Feature.Identifier) == 0)
                                                             select element).ToList();

                                            if (radio_com != null && radio_com.Count > 0)
                                            {
                                                pdmARSPS.CommunicationChanels = new List<RadioCommunicationChanel>();

                                                foreach (var item in radio_com)
                                                {
                                                    var aimRadioComChnl = item.AIXM51_Feature;
                                                    RadioCommunicationChanel pdmRadioComChnl = (RadioCommunicationChanel)AIM_PDM_Converter.AIM_Object_Convert(aimRadioComChnl, item.AixmGeo);

                                                    if (pdmRadioComChnl != null)
                                                    {
                                                        pdmRadioComChnl.ChanelType = _chanel.Type.ToString();
                                                        pdmRadioComChnl.CallSign =
                                                            _chanel.CallSign != null && _chanel.CallSign.Count > 0
                                                                ? _chanel.CallSign[0].CallSign
                                                                : "";

                                                        pdmARSPS.CommunicationChanels.Add(pdmRadioComChnl);
                                                    }

                                                }
                                            }

                                        }

                                    }
                                }
                                catch (Exception ex)
                                {
                                    LogManager.GetLogger(ex.TargetSite.Name).Error(ex, cnl.AIXM51_Feature.GetType().ToString() + " ID: " + cnl.AIXM51_Feature.Identifier);

                                }
                            }
                            if (pdmARSPS.CommunicationChanels != null && pdmARSPS.CommunicationChanels.Count == 0) pdmARSPS.CommunicationChanels = null;

                        }

                        if (informationService_chanel != null)
                        {

                            foreach (var cnl in informationService_chanel)
                            {
                                try
                                {

                                    Aran.Aim.Features.InformationService _chanel = (Aran.Aim.Features.InformationService)cnl.AIXM51_Feature;

                                    foreach (var clnArspc in _chanel.ClientAirspace)
                                    {

                                        if (clnArspc.Feature.Identifier.CompareTo(aimARSP.Identifier) != 0) continue;

                                        foreach (var _radioComm in _chanel.RadioCommunication)
                                        {

                                            var radio_com = (from element in this.CurEnvironment.Data.Intermediate_AIXM51_Arena_Features
                                                             where (element != null) &&
                                                              (element.AIXM51_Feature.FeatureType == Aran.Aim.FeatureType.RadioCommunicationChannel) &&
                                                                 (((Aran.Aim.Features.RadioCommunicationChannel)element.AIXM51_Feature).Identifier.CompareTo(_radioComm.Feature.Identifier) == 0)
                                                             select element).ToList();

                                            if (radio_com != null && radio_com.Count > 0)
                                            {
                                                if (pdmARSPS.CommunicationChanels == null)
                                                    pdmARSPS.CommunicationChanels = new List<RadioCommunicationChanel>();

                                                foreach (var item in radio_com)
                                                {
                                                    var aimRadioComChnl = item.AIXM51_Feature;
                                                    RadioCommunicationChanel pdmRadioComChnl = (RadioCommunicationChanel)AIM_PDM_Converter.AIM_Object_Convert(aimRadioComChnl, item.AixmGeo);

                                                    if (pdmRadioComChnl != null)
                                                    {
                                                        pdmRadioComChnl.ChanelType = _chanel.Type.ToString();
                                                        pdmRadioComChnl.CallSign =
                                                            _chanel.CallSign != null && _chanel.CallSign.Count > 0
                                                                ? _chanel.CallSign[0].CallSign
                                                                : "";

                                                        pdmARSPS.CommunicationChanels.Add(pdmRadioComChnl);
                                                    }

                                                }
                                            }

                                        }

                                    }
                                }
                                catch (Exception ex)
                                {
                                    LogManager.GetLogger(ex.TargetSite.Name).Error(ex, cnl.AIXM51_Feature.GetType().ToString() + " ID: " + cnl.AIXM51_Feature.Identifier);

                                }
                            }
                            if (pdmARSPS.CommunicationChanels != null && pdmARSPS.CommunicationChanels.Count == 0) pdmARSPS.CommunicationChanels = null;

                        }


                        #endregion

                        {
                            CurEnvironment.Data.PdmObjectList.Add(pdmARSPS);
                        }
                    }
                    catch (Exception ex)
                    {
                        LogManager.GetLogger(ex.TargetSite.Name).Error(ex, featureARSPS.AIXM51_Feature.GetType().ToString() + " ID: " + featureARSPS.AIXM51_Feature.Identifier);
                    }
                    alrtForm.progressBar1.Value++;
                    Application.DoEvents();

                }

                #endregion


                alrtForm.label1.Text = "Enroutes ";
                Application.DoEvents();
                #region Enroute/RouteSegment/RouteSegmentPoint

                var routeList = (from element in this.CurEnvironment.Data.Intermediate_AIXM51_Arena_Features
                                 where (element != null) &&
                                     (element.AIXM51_Feature.FeatureType == Aran.Aim.FeatureType.Route)
                                 select element).ToList();

                var route_chanel = (from element in this.CurEnvironment.Data.Intermediate_AIXM51_Arena_Features
                                    where (element != null) &&
                                     (element.AIXM51_Feature.FeatureType == Aran.Aim.FeatureType.AirTrafficControlService) &&
                                    (((Aran.Aim.Features.AirTrafficControlService)element.AIXM51_Feature).ClientRoute != null) &&
                                    (((Aran.Aim.Features.AirTrafficControlService)element.AIXM51_Feature).ClientRoute.Count > 0)
                                    select element).ToList();



                alrtForm.progressBar1.Maximum = routeList.Count;
                alrtForm.progressBar1.Value = 0;

                //List<string> ttt = new List<string>();

                foreach (var featureARSPS in routeList)
                {

                    try
                    {
                        var aimENRT = featureARSPS.AIXM51_Feature;

                        //if (aimENRT.Identifier.ToString().StartsWith("5217216f-08e2-4026-8f96-09b3b329fb4b"))
                        //    System.Diagnostics.Debug.WriteLine("");

                        Enroute pdmENRT = (Enroute)AIM_PDM_Converter.AIM_Object_Convert(aimENRT, null);


                        if (pdmENRT == null) continue;


                        var org = (from element in this.CurEnvironment.Data.Intermediate_AIXM51_Arena_Features
                                   where (element != null) &&
                                    (element.AIXM51_Feature.FeatureType == Aran.Aim.FeatureType.OrganisationAuthority) &&
                                   (((Aran.Aim.Features.OrganisationAuthority)element.AIXM51_Feature).Identifier != null) &&
                                       (((Aran.Aim.Features.OrganisationAuthority)element.AIXM51_Feature).Identifier.ToString().StartsWith(pdmENRT.OrganisationAuthority))
                                   select element).FirstOrDefault();
                        if (org != null)
                        {
                            pdmENRT.OrganisationAuthority = ((Aran.Aim.Features.OrganisationAuthority)org.AIXM51_Feature).Name;
                        }

                        #region Communication chanels


                        if (route_chanel != null)
                        {
                            pdmENRT.CommunicationChanels = new List<RadioCommunicationChanel>();
                            foreach (var cnl in route_chanel)
                            {
                                try
                                {
                                    Aran.Aim.Features.AirTrafficControlService _chanel = (Aran.Aim.Features.AirTrafficControlService)cnl.AIXM51_Feature;

                                    foreach (var clnRoute in _chanel.ClientRoute)
                                    {

                                        if (clnRoute.ReferencedRoute.Identifier.CompareTo(aimENRT.Identifier) != 0) continue;

                                        foreach (var _radioComm in _chanel.RadioCommunication)
                                        {

                                            var radio_com = (from element in this.CurEnvironment.Data.Intermediate_AIXM51_Arena_Features
                                                             where (element != null) &&
                                                              (element.AIXM51_Feature.FeatureType == Aran.Aim.FeatureType.RadioCommunicationChannel) &&
                                                                 (((Aran.Aim.Features.RadioCommunicationChannel)element.AIXM51_Feature).Identifier.CompareTo(_radioComm.Feature.Identifier) == 0)
                                                             select element).ToList();
                                            if (radio_com != null)
                                            {
                                                foreach (var item in radio_com)
                                                {

                                                    RadioCommunicationChanel pdm_chanel = new RadioCommunicationChanel
                                                    {
                                                        ChanelType = _chanel.Type.ToString(),
                                                        CallSign = _chanel.CallSign != null && _chanel.CallSign.Count > 0 ? _chanel.CallSign[0].CallSign : "",
                                                    };

                                                    pdm_chanel.FrequencyReception = ((Aran.Aim.Features.RadioCommunicationChannel)item.AIXM51_Feature).FrequencyReception != null ?
                                                        ((Aran.Aim.Features.RadioCommunicationChannel)item.AIXM51_Feature).FrequencyReception.Value : 0;

                                                    pdm_chanel.FrequencyTransmission = ((Aran.Aim.Features.RadioCommunicationChannel)item.AIXM51_Feature).FrequencyTransmission != null ?
                                                        ((Aran.Aim.Features.RadioCommunicationChannel)item.AIXM51_Feature).FrequencyTransmission.Value : 0;

                                                    UOM_FREQ frqUom;
                                                    if (((Aran.Aim.Features.RadioCommunicationChannel)item.AIXM51_Feature).FrequencyReception != null)
                                                    {
                                                        Enum.TryParse<UOM_FREQ>(((Aran.Aim.Features.RadioCommunicationChannel)item.AIXM51_Feature).FrequencyReception.Uom.ToString(), out frqUom);
                                                        pdm_chanel.ReceptionFrequencyUOM = frqUom;
                                                    }

                                                    if (((Aran.Aim.Features.RadioCommunicationChannel)item.AIXM51_Feature).FrequencyTransmission != null)
                                                    {
                                                        Enum.TryParse<UOM_FREQ>(((Aran.Aim.Features.RadioCommunicationChannel)item.AIXM51_Feature).FrequencyTransmission.Uom.ToString(), out frqUom);
                                                        pdm_chanel.TransmissionFrequencyUOM = frqUom;
                                                    }

                                                    PDM.CodeFacilityRanking rnk;
                                                    if (((Aran.Aim.Features.RadioCommunicationChannel)item.AIXM51_Feature).FrequencyReception != null)
                                                    {
                                                        Enum.TryParse<PDM.CodeFacilityRanking>(((Aran.Aim.Features.RadioCommunicationChannel)item.AIXM51_Feature).Rank.ToString(), out rnk);
                                                        pdm_chanel.Rank = rnk;
                                                    }

                                                    pdmENRT.CommunicationChanels.Add(pdm_chanel);

                                                }
                                            }

                                        }

                                    }
                                }
                                catch (Exception ex)
                                {
                                    LogManager.GetLogger(ex.TargetSite.Name).Error(ex, cnl.AIXM51_Feature.GetType().ToString() + " ID: " + cnl.AIXM51_Feature.Identifier);

                                }
                            }

                            if (pdmENRT.CommunicationChanels.Count == 0) pdmENRT.CommunicationChanels = null;

                        }

                        #endregion

                        


                        #region RouteSegment


                        var enrt_rts = (from element in CurEnvironment.Data.Intermediate_AIXM51_Arena_Features
                                        where (element != null) &&
                                            (element.AIXM51_Feature.FeatureType == Aran.Aim.FeatureType.RouteSegment) && (((Aran.Aim.Features.RouteSegment)element.AIXM51_Feature).RouteFormed != null) &&
                                            (((Aran.Aim.Features.RouteSegment)element.AIXM51_Feature).RouteFormed.Identifier == aimENRT.Identifier)
                                        select element).ToList();



                        if (enrt_rts != null)
                        {
                            //System.Diagnostics.Debug.WriteLine(enrt_rts.Count.ToString());

                            pdmENRT.Routes = new List<RouteSegment>();

                            foreach (var featureROUTE in enrt_rts)
                            {
                                try
                                {
                                    var aimRoute = featureROUTE.AIXM51_Feature;

                                    //if (aimRoute.Identifier.ToString().StartsWith("ed1af420-b394-4ee4-93ee-773dc90b6f47"))
                                    //    System.Diagnostics.Debug.WriteLine("");
                                    //if (featureROUTE.AixmGeo.Count > 0)
                                    {
                                        RouteSegment pdmRouteSeg = (RouteSegment)AIM_PDM_Converter.AIM_Object_Convert(aimRoute, featureROUTE.AixmGeo);
                                        if (pdmRouteSeg != null)
                                        {

                                            #region StartPoint

                                            if ((((Aran.Aim.Features.RouteSegment)aimRoute).Start != null) &&
                                                (((Aran.Aim.Features.RouteSegment)aimRoute).Start.PointChoice != null) &&
                                                (((Aran.Aim.Features.RouteSegment)aimRoute).Start.PointChoice.AimingPoint != null))
                                            {
                                                try
                                                {



                                                    PDMObject segStartPnt = GetSegmentPoint(((Aran.Aim.Features.RouteSegment)aimRoute).Start.PointChoice.Choice.ToString(), ((Aran.Aim.Features.RouteSegment)aimRoute).Start.PointChoice.AimingPoint.Identifier);
                                                    if (segStartPnt != null)
                                                    {
                                                        PDMObject _obj = DefinePointSegmentDesignator(((SegmentPoint)segStartPnt).PointChoice, segStartPnt.ID);

                                                        pdmRouteSeg.StartPoint = new RouteSegmentPoint
                                                        {
                                                            ID = segStartPnt.ID, //Guid.NewGuid().ToString(),
                                                            Route_LEG_ID = pdmRouteSeg.ID,
                                                            PointRole = ProcedureFixRoleType.ENRT,
                                                            PointUse = ProcedureSegmentPointUse.START_POINT,
                                                            IsWaypoint = ((SegmentPoint)segStartPnt).IsWaypoint,
                                                            PointChoice = ((SegmentPoint)segStartPnt).PointChoice,
                                                            SegmentPointDesignator = ((SegmentPoint)segStartPnt).PointChoiceID,
                                                            Geo = _obj.Geo,
                                                        };

                                                        pdmRouteSeg.StartPoint.IsWaypoint = ((Aran.Aim.Features.RouteSegment)aimRoute).Start.Waypoint.HasValue ? ((Aran.Aim.Features.RouteSegment)aimRoute).Start.Waypoint.Value : false;

                                                        if (((Aran.Aim.Features.RouteSegment)aimRoute).Start.RadarGuidance.HasValue) { pdmRouteSeg.StartPoint.RadarGuidance = ((Aran.Aim.Features.RouteSegment)aimRoute).Start.RadarGuidance.Value; }

                                                        if (((Aran.Aim.Features.RouteSegment)aimRoute).Start.ReportingATC.HasValue)
                                                        {
                                                            PDM.CodeATCReporting _uomATCRRep;
                                                            Enum.TryParse<PDM.CodeATCReporting>(((Aran.Aim.Features.RouteSegment)aimRoute).Start.ReportingATC.Value.ToString(), out _uomATCRRep);
                                                            pdmRouteSeg.StartPoint.ReportingATC = _uomATCRRep;

                                                        }


                                                        if (((Aran.Aim.Features.RouteSegment)aimRoute).Start.FlyOver.HasValue) { pdmRouteSeg.StartPoint.FlyOver = ((Aran.Aim.Features.RouteSegment)aimRoute).Start.FlyOver.Value; }


                                                        if (_obj != null)
                                                        {
                                                            pdmRouteSeg.StartPoint.PointChoiceID = segStartPnt.ID;
                                                            pdmRouteSeg.StartPoint.Lat = _obj.Lat;
                                                            pdmRouteSeg.StartPoint.Lon = _obj.Lon;
                                                        }
                                                    }
                                                }
                                                catch (Exception ex)
                                                {
                                                    LogManager.GetLogger(ex.TargetSite.Name).Error(ex, ((Aran.Aim.Features.RouteSegment)aimRoute).Start.PointChoice.Choice.ToString() + " ID: " + ((Aran.Aim.Features.RouteSegment)aimRoute).Start.PointChoice.AimingPoint.Identifier);
                                                    continue;
                                                }
                                            }

                                            #endregion


                                            #region EndPoint

                                            if ((((Aran.Aim.Features.RouteSegment)aimRoute).End != null) &&
                                                (((Aran.Aim.Features.RouteSegment)aimRoute).End.PointChoice != null) &&
                                                (((Aran.Aim.Features.RouteSegment)aimRoute).End.PointChoice.AimingPoint != null))
                                            {
                                                try
                                                {
                                                    PDMObject segEndPnt = GetSegmentPoint(((Aran.Aim.Features.RouteSegment)aimRoute).End.PointChoice.Choice.ToString(), ((Aran.Aim.Features.RouteSegment)aimRoute).End.PointChoice.AimingPoint.Identifier);


                                                    if (segEndPnt != null)
                                                    {
                                                        PDMObject _obj = DefinePointSegmentDesignator(((SegmentPoint)segEndPnt).PointChoice, segEndPnt.ID);

                                                        pdmRouteSeg.EndPoint = new RouteSegmentPoint
                                                        {
                                                            ID = segEndPnt.ID,//Guid.NewGuid().ToString(),
                                                            Route_LEG_ID = pdmRouteSeg.ID,
                                                            PointRole = ProcedureFixRoleType.ENRT,
                                                            PointUse = ProcedureSegmentPointUse.END_POINT,
                                                            IsWaypoint = ((SegmentPoint)segEndPnt).IsWaypoint,
                                                            PointChoice = ((SegmentPoint)segEndPnt).PointChoice,
                                                            SegmentPointDesignator = ((SegmentPoint)segEndPnt).PointChoiceID,
                                                            Geo = _obj.Geo,

                                                        };

                                                        pdmRouteSeg.EndPoint.IsWaypoint = ((Aran.Aim.Features.RouteSegment)aimRoute).End.Waypoint.HasValue ? ((Aran.Aim.Features.RouteSegment)aimRoute).End.Waypoint.Value : false;

                                                        if (((Aran.Aim.Features.RouteSegment)aimRoute).End.RadarGuidance.HasValue) { pdmRouteSeg.EndPoint.RadarGuidance = ((Aran.Aim.Features.RouteSegment)aimRoute).End.RadarGuidance.Value; }

                                                        if (((Aran.Aim.Features.RouteSegment)aimRoute).End.ReportingATC.HasValue)
                                                        {
                                                            PDM.CodeATCReporting _uomATCRRep;
                                                            Enum.TryParse<PDM.CodeATCReporting>(((Aran.Aim.Features.RouteSegment)aimRoute).End.ReportingATC.Value.ToString(), out _uomATCRRep);
                                                            pdmRouteSeg.EndPoint.ReportingATC = _uomATCRRep;

                                                            //if (pdmRouteSeg.EndPoint.PointChoice == PointChoice.DesignatedPoint)
                                                            //{
                                                            //    UpdateWayPointReportingATCvalue(segEndPnt.ID, _uomATCRRep);
                                                            //}
                                                        }


                                                        if (((Aran.Aim.Features.RouteSegment)aimRoute).End.FlyOver.HasValue) { pdmRouteSeg.EndPoint.FlyOver = ((Aran.Aim.Features.RouteSegment)aimRoute).End.FlyOver.Value; }


                                                        if (_obj != null)
                                                        {
                                                            pdmRouteSeg.EndPoint.PointChoiceID = segEndPnt.ID;
                                                            pdmRouteSeg.EndPoint.Lat = _obj.Lat;
                                                            pdmRouteSeg.EndPoint.Lon = _obj.Lon;
                                                        }
                                                    }
                                                }
                                                catch (Exception ex)
                                                {
                                                    LogManager.GetLogger(ex.TargetSite.Name).Error(ex, ((Aran.Aim.Features.RouteSegment)aimRoute).End.PointChoice.Choice.ToString() + " ID: " + ((Aran.Aim.Features.RouteSegment)aimRoute).End.PointChoice.AimingPoint.Identifier);
                                                    continue;
                                                }
                                            }

                                            #endregion

                                            if (pdmRouteSeg.Geo == null && pdmRouteSeg.StartPoint != null && pdmRouteSeg.StartPoint.Geo != null && pdmRouteSeg.EndPoint != null && pdmRouteSeg.EndPoint.Geo != null)
                                            {
                                                IPolyline ln = new PolylineClass();
                                                IPoint ppSt = new PointClass();
                                                ppSt.PutCoords(((IPoint)pdmRouteSeg.StartPoint.Geo).X, ((IPoint)pdmRouteSeg.StartPoint.Geo).Y);

                                                var zAware = ppSt as IZAware;
                                                zAware.ZAware = true;
                                                ppSt.Z = 0;
                                                var mAware = ppSt as IMAware;
                                                mAware.MAware = true;
                                                ln.FromPoint = ppSt;

                                                IPoint ppEn = new PointClass();
                                                ppEn.PutCoords(((IPoint)pdmRouteSeg.EndPoint.Geo).X, ((IPoint)pdmRouteSeg.EndPoint.Geo).Y);
                                                zAware = ppSt as IZAware;
                                                zAware.ZAware = true;
                                                ppEn.Z = 0;
                                                mAware = ppSt as IMAware;
                                                mAware.MAware = true;

                                                ln.ToPoint = ppEn;

                                                pdmRouteSeg.Geo = ln;


                                                zAware = pdmRouteSeg.Geo as IZAware;
                                                zAware.ZAware = true;
                                                mAware = pdmRouteSeg.Geo as IMAware;
                                                mAware.MAware = true;

                                                pdmRouteSeg.ExeptionDetails = new ExeptionMessage { Message = "Reconstructed Geometry", Source = pdmRouteSeg.ToString(), StackTrace = pdmRouteSeg.ID };
                                                pdmRouteSeg.SourceDetail = "Reconstructed Geometry";

                                            }


                                            if (pdmRouteSeg.Geo != null)
                                                pdmRouteSeg.LegBlobGeometry = HelperClass.SetObjectToBlob(pdmRouteSeg.Geo, "Leg");
                                            else
                                                pdmRouteSeg.ExeptionDetails = new ExeptionMessage { Message = "Empty Geometry", Source = pdmRouteSeg.ToString(), StackTrace = pdmRouteSeg.ID };

                                            //if ((pdmRouteSeg.StartPoint != null) && (pdmRouteSeg.EndPoint != null))
                                            {
                                                pdmRouteSeg.ID_Route = pdmENRT.ID;
                                                pdmENRT.RouteLength = pdmENRT.RouteLength + Math.Round(pdmRouteSeg.ConvertValueToMeter(pdmRouteSeg.ValLen.Value, pdmRouteSeg.UomValLen.ToString()), 1);

                                                pdmENRT.Routes.Add(pdmRouteSeg);
                                            }

                                        }

                                    }
                                }
                                catch (Exception ex)
                                {
                                    LogManager.GetLogger(ex.TargetSite.Name).Error(ex, featureROUTE.AIXM51_Feature.GetType().ToString() + " ID: " + featureROUTE.AIXM51_Feature.Identifier);

                                }

                                //ttt.Add(featureROUTE.AIXM51_Feature.Identifier.ToString());
                            }

                        }


                        #endregion

                        //if (pdmENRT.StoreToDB(CurEnvironment.Data.TableDictionary))
                        {
                            CurEnvironment.Data.PdmObjectList.Add(pdmENRT);
                        }
                    }
                    catch (Exception ex)
                    {
                        LogManager.GetLogger(ex.TargetSite.Name).Error(ex, featureARSPS.AIXM51_Feature.GetType().ToString() + " ID: " + featureARSPS.AIXM51_Feature.Identifier);
                    }

                    alrtForm.progressBar1.Value++;
                    Application.DoEvents();


                }

                #endregion



                //var enrt_rts_nill = (from element in CurEnvironment.Data.Intermediate_AIXM51_Arena_Features
                //                     where (element != null) &&
                //                         (element.AIXM51_Feature.FeatureType == Aran.Aim.FeatureType.RouteSegment)
                //                     select element).ToList();

                //foreach (var item in enrt_rts_nill)
                //{
                //    if (item.AIXM51_Feature != null)
                //    {
                //        if (ttt.IndexOf(item.AIXM51_Feature.Identifier.ToString()) < 0)
                //            System.Diagnostics.Debug.WriteLine(item.AIXM51_Feature.Identifier.ToString());
                //    }
                //    else
                //    {
                //        System.Diagnostics.Debug.WriteLine("null");
                //    }
                //}


                alrtForm.label1.Text = "Procedures ";
                Application.DoEvents();
                #region Instrument Approach Procedures

                var iapList = (from element in this.CurEnvironment.Data.Intermediate_AIXM51_Arena_Features
                               where (element != null) &&
                                   (element.AIXM51_Feature.FeatureType == Aran.Aim.FeatureType.InstrumentApproachProcedure)
                               select element).ToList();

                alrtForm.progressBar1.Maximum = iapList.Count;
                alrtForm.progressBar1.Value = 0;

                foreach (var featureProc in iapList)
                {

                    try
                    {

                        var aimPROC = featureProc.AIXM51_Feature;


                        InstrumentApproachProcedure pdmIAP = (InstrumentApproachProcedure)AIM_PDM_Converter.AIM_Object_Convert(aimPROC, null);

                        if (pdmIAP == null) continue;
                        pdmIAP.Airport_ICAO_Code = GetAirportCode(pdmIAP.AirportIdentifier);
                        pdmIAP.Lon = GetAirportName(pdmIAP.AirportIdentifier);
                        if (pdmIAP.LandingArea != null)
                        {
                            List<PDMObject> lst = new List<PDMObject>();
                            foreach (var item in pdmIAP.LandingArea)
                            {
                                string Id = item.ID;
                                var rdn = LandingTakeoffArea(Id);
                                if (rdn != null) lst.Add(rdn);
                            }

                            pdmIAP.LandingArea.Clear();
                            if (lst != null && lst.Count > 0) pdmIAP.LandingArea.AddRange(lst);
                        }


                        #region Create Transitions

                        if (((Aran.Aim.Features.InstrumentApproachProcedure)aimPROC).FlightTransition != null)
                        {
                            pdmIAP.Transitions = new List<ProcedureTransitions>();


                            foreach (var aimFlightTransition in ((Aran.Aim.Features.InstrumentApproachProcedure)aimPROC).FlightTransition)
                            {
                                try
                                {
                                    ProcedureTransitions pdmProcedureTransitions = CreateProcedureTransition(aimFlightTransition, pdmIAP.ID);

                                    if (pdmProcedureTransitions != null && pdmProcedureTransitions.Legs != null && pdmProcedureTransitions.Legs.Count > 0)
                                    {
                                        pdmIAP.Transitions.Add(pdmProcedureTransitions);

                                        if (pdmIAP.Profile != null && pdmProcedureTransitions.RouteType == ProcedurePhaseType.FINAL && pdmProcedureTransitions.Legs != null && pdmProcedureTransitions.Legs.Count > 0)
                                        {

                                            pdmIAP.Profile.ApproachMinimaTable = new List<ApproachMinima>();
                                            int step = 1;
                                            foreach (ProcedureLeg leg in pdmProcedureTransitions.Legs)
                                            {
                                                if (leg.LegSpecialization == SegmentLegSpecialization.MissedApproachLeg) break;
                                                //if (step < pdmProcedureTransitions.Legs.Count - 1)
                                                {
                                                    if (leg.AssessmentArea != null && leg.AssessmentArea[0].AssessedAltitude.HasValue)
                                                    {
                                                        ApproachMinima appMin = new ApproachMinima { Minima = leg.AssessmentArea[0].AssessedAltitude.Value, MinimaUom = leg.AssessmentArea[0].AssessedAltitudeUOM };
                                                        appMin.ProfileSegmnetDesignator = leg.StartPoint.PointRole.ToString() + ":" + pdmProcedureTransitions.Legs[step].StartPoint.PointRole.ToString();

                                                        pdmIAP.Profile.ApproachMinimaTable.Add(appMin);
                                                    }
                                                }


                                                step++;
                                            }

                                        }

                                        if (pdmProcedureTransitions.Description != null)
                                        {
                                            string[] ids = pdmProcedureTransitions.Description.Split(':');
                                            if (ids.Length > 0)
                                            {
                                                foreach (var rdnId in ids)
                                                {
                                                    if (rdnId.Length <= 0) continue;
                                                    var _objRnd = (from element in CurEnvironment.Data.Intermediate_AIXM51_Arena_Features
                                                                   where (element != null) &&
                                                                       (element.AIXM51_Feature.FeatureType == Aran.Aim.FeatureType.RunwayDirection) && (((Aran.Aim.Features.RunwayDirection)element.AIXM51_Feature).UsedRunway != null) &&
                                                                       (((Aran.Aim.Features.RunwayDirection)element.AIXM51_Feature).Identifier.ToString().StartsWith(rdnId))
                                                                   select element).FirstOrDefault();

                                                    if (_objRnd != null)
                                                    {
                                                        pdmProcedureTransitions.Description = ((Aran.Aim.Features.RunwayDirection)_objRnd.AIXM51_Feature).Designator;
                                                        if (pdmIAP.LandingArea == null) pdmIAP.LandingArea = new List<PDMObject>();
                                                        var rdn = LandingTakeoffArea(((Aran.Aim.Features.RunwayDirection)_objRnd.AIXM51_Feature).Identifier.ToString());
                                                        if (rdn != null)
                                                            if (pdmIAP.LandingArea.IndexOf(rdn) < 0) pdmIAP.LandingArea.Add(rdn);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    LogManager.GetLogger(ex.TargetSite.Name).Error(ex, aimFlightTransition.GetType().ToString() + " ID: " + aimFlightTransition.Id);

                                }
                            }

                        }

                        #endregion

                        //if (pdmIAP.StoreToDB(CurEnvironment.Data.TableDictionary))
                        {
                            LinkCommunicationChanels(pdmIAP, proc_chanel);
                            if (pdmIAP.Transitions != null && pdmIAP.Transitions.Count > 0) CurEnvironment.Data.PdmObjectList.Add(pdmIAP);
                        }
                    }
                    catch (Exception ex)
                    {
                        LogManager.GetLogger(ex.TargetSite.Name).Error(ex, featureProc.AIXM51_Feature.GetType().ToString() + " ID: " + featureProc.AIXM51_Feature.Identifier);

                    }
                    alrtForm.progressBar1.Value++;
                    Application.DoEvents();
                }

                #endregion


                #region Standard Instrument Departure Procedures

                var sidList = (from element in this.CurEnvironment.Data.Intermediate_AIXM51_Arena_Features
                               where (element != null) &&
                                   (element.AIXM51_Feature.FeatureType == Aran.Aim.FeatureType.StandardInstrumentDeparture)
                               select element).ToList();

                alrtForm.progressBar1.Maximum = sidList.Count;
                alrtForm.progressBar1.Value = 0;

                foreach (var featureProc in sidList)
                {

                    try
                    {
                        var aimProc = featureProc.AIXM51_Feature;

                        //if (aimProc.Identifier.ToString().StartsWith("8c0d5006-4fc7-40d5-a522-cf9d69c2f793"))
                        //    System.Diagnostics.Debug.WriteLine("8c0d5006-4fc7-40d5-a522-cf9d69c2f793");
                        //SID
                        StandardInstrumentDeparture pdmSID = (StandardInstrumentDeparture)AIM_PDM_Converter.AIM_Object_Convert(aimProc, null);

                        if (pdmSID == null) continue;
                        pdmSID.Airport_ICAO_Code = GetAirportCode(pdmSID.AirportIdentifier);
                        pdmSID.Lon = GetAirportName(pdmSID.AirportIdentifier);

                        if (pdmSID.LandingArea != null)
                        {
                            List<PDMObject> lst = new List<PDMObject>();
                            foreach (var item in pdmSID.LandingArea)
                            {
                                string Id = item.ID;
                                var rdn = LandingTakeoffArea(Id);
                                if (rdn != null) lst.Add(rdn);
                            }

                            pdmSID.LandingArea.Clear();
                            if (lst != null && lst.Count > 0) pdmSID.LandingArea.AddRange(lst);

                        }

                        #region Create Transitions

                        if (((Aran.Aim.Features.StandardInstrumentDeparture)aimProc).FlightTransition != null)
                        {
                            pdmSID.Transitions = new List<ProcedureTransitions>();


                            foreach (var aimFlightTransition in ((Aran.Aim.Features.StandardInstrumentDeparture)aimProc).FlightTransition)
                            {
                                try
                                {
                                    ProcedureTransitions pdmProcedureTransitions = CreateProcedureTransition(aimFlightTransition, pdmSID.ID);

                                    if (pdmProcedureTransitions != null && pdmProcedureTransitions.Legs != null && pdmProcedureTransitions.Legs.Count > 0)
                                    {
                                        if (pdmProcedureTransitions.Description != null)
                                        {
                                            string[] ids = pdmProcedureTransitions.Description.Split(':');
                                            if (ids.Length > 0)
                                            {
                                                foreach (var rdnId in ids)
                                                {
                                                    if (rdnId.Length <= 0) continue;
                                                    var _objRnd = (from element in CurEnvironment.Data.Intermediate_AIXM51_Arena_Features
                                                                   where (element != null) &&
                                                                       (element.AIXM51_Feature.FeatureType == Aran.Aim.FeatureType.RunwayDirection) && (((Aran.Aim.Features.RunwayDirection)element.AIXM51_Feature).UsedRunway != null) &&
                                                                       (((Aran.Aim.Features.RunwayDirection)element.AIXM51_Feature).Identifier.ToString().StartsWith(rdnId))
                                                                   select element).FirstOrDefault();

                                                    if (_objRnd != null)
                                                    {
                                                        pdmProcedureTransitions.Description = ((Aran.Aim.Features.RunwayDirection)_objRnd.AIXM51_Feature).Designator;
                                                        if (pdmSID.LandingArea == null) pdmSID.LandingArea = new List<PDMObject>();
                                                        var rdn = LandingTakeoffArea(((Aran.Aim.Features.RunwayDirection)_objRnd.AIXM51_Feature).Identifier.ToString());
                                                        if (rdn != null)
                                                            if (pdmSID.LandingArea.IndexOf(rdn) < 0) pdmSID.LandingArea.Add(rdn);
                                                    }
                                                }
                                            }
                                        }
                                        pdmSID.Transitions.Add(pdmProcedureTransitions);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    LogManager.GetLogger(ex.TargetSite.Name).Error(ex, aimFlightTransition.GetType().ToString() + " ID: " + aimFlightTransition.Id);

                                }
                            }




                            if (pdmSID.Transitions != null && pdmSID.Transitions.Count > 0)
                            {
                                foreach (var _trans in pdmSID.Transitions)
                                {
                                    if (_trans.Legs != null && _trans.Legs.Count > 0)
                                    {
                                        foreach (var _leg in _trans.Legs)
                                        {
                                            if (_leg.Length.HasValue)
                                            {
                                                pdmSID.TotalLength = pdmSID.TotalLength + _leg.Length.Value;
                                                pdmSID.TotalLengthUOM = _leg.LengthUOM;
                                            }
                                        }
                                    }
                                }

                                foreach (var _trans in pdmSID.Transitions)
                                {
                                    if (_trans.Legs != null && _trans.Legs.Count > 0)
                                    {
                                        foreach (var _leg in _trans.Legs)
                                        {
                                            if (pdmSID.TotalLength > 0)
                                                _leg.Y = pdmSID.TotalLength;
                                        }
                                    }
                                }

                            }
                        }

                        #endregion

                        //if (pdmSID.StoreToDB(CurEnvironment.Data.TableDictionary))
                        {
                            LinkCommunicationChanels(pdmSID, proc_chanel);
                            if (pdmSID.Transitions != null && pdmSID.Transitions.Count > 0) CurEnvironment.Data.PdmObjectList.Add(pdmSID);
                        }
                    }
                    catch (Exception ex)
                    {
                        LogManager.GetLogger(ex.TargetSite.Name).Error(ex, featureProc.AIXM51_Feature.GetType().ToString() + " ID: " + featureProc.AIXM51_Feature.Identifier);

                    }

                    alrtForm.progressBar1.Value++;
                    Application.DoEvents();
                }


                #endregion


                #region Standard Instrument Arrival Procedures

                var starList = (from element in this.CurEnvironment.Data.Intermediate_AIXM51_Arena_Features
                                where (element != null) &&
                                    (element.AIXM51_Feature.FeatureType == Aran.Aim.FeatureType.StandardInstrumentArrival)
                                select element).ToList();

                alrtForm.progressBar1.Maximum = starList.Count;
                alrtForm.progressBar1.Value = 0;

                foreach (var featureProc in starList)
                {

                    try
                    {
                        var aimProc = featureProc.AIXM51_Feature;

                        if (aimProc.Identifier.ToString().StartsWith("0a2f36f8-ee4d-4e44-b09b-a1a16876e68e"))
                            System.Diagnostics.Debug.WriteLine("");
                        //STAR
                        StandardInstrumentArrival pdmSTAR = (StandardInstrumentArrival)AIM_PDM_Converter.AIM_Object_Convert(aimProc, null);

                        if (pdmSTAR == null) continue;
                        pdmSTAR.Airport_ICAO_Code = GetAirportCode(pdmSTAR.AirportIdentifier);
                        pdmSTAR.Lon = GetAirportName(pdmSTAR.AirportIdentifier);

                        if (pdmSTAR.LandingArea != null)
                        {
                            List<PDMObject> lst = new List<PDMObject>();
                            foreach (var item in pdmSTAR.LandingArea)
                            {
                                string Id = item.ID;
                                var rdn = LandingTakeoffArea(Id);
                                if (rdn != null) lst.Add(rdn);
                            }

                            pdmSTAR.LandingArea.Clear();
                            if (lst != null && lst.Count > 0) pdmSTAR.LandingArea.AddRange(lst);
                        }

                        #region Create Transitions

                        if (((Aran.Aim.Features.StandardInstrumentArrival)aimProc).FlightTransition != null)
                        {
                            pdmSTAR.Transitions = new List<ProcedureTransitions>();


                            foreach (var aimFlightTransition in ((Aran.Aim.Features.StandardInstrumentArrival)aimProc).FlightTransition)
                            {
                                try
                                {
                                    ProcedureTransitions pdmProcedureTransitions = CreateProcedureTransition(aimFlightTransition, pdmSTAR.ID);

                                    if (pdmProcedureTransitions != null && pdmProcedureTransitions.Legs != null && pdmProcedureTransitions.Legs.Count > 0)
                                    {

                                        if (pdmProcedureTransitions.Description != null)
                                        {
                                            string[] ids = pdmProcedureTransitions.Description.Split(':');
                                            if (ids.Length > 0)
                                            {
                                                foreach (var rdnId in ids)
                                                {
                                                    if (rdnId.Length <= 0) continue;
                                                    var _objRnd = (from element in CurEnvironment.Data.Intermediate_AIXM51_Arena_Features
                                                                   where (element != null) &&
                                                                       (element.AIXM51_Feature.FeatureType == Aran.Aim.FeatureType.RunwayDirection) && (((Aran.Aim.Features.RunwayDirection)element.AIXM51_Feature).UsedRunway != null) &&
                                                                       (((Aran.Aim.Features.RunwayDirection)element.AIXM51_Feature).Identifier.ToString().StartsWith(rdnId))
                                                                   select element).FirstOrDefault();

                                                    if (_objRnd != null)
                                                    {
                                                        pdmProcedureTransitions.Description = ((Aran.Aim.Features.RunwayDirection)_objRnd.AIXM51_Feature).Designator;
                                                        if (pdmSTAR.LandingArea == null) pdmSTAR.LandingArea = new List<PDMObject>();
                                                        var rdn = LandingTakeoffArea(((Aran.Aim.Features.RunwayDirection)_objRnd.AIXM51_Feature).Identifier.ToString());
                                                        if (rdn != null)
                                                            if (pdmSTAR.LandingArea.IndexOf(rdn) < 0) pdmSTAR.LandingArea.Add(rdn);
                                                    }
                                                }
                                            }
                                        }

                                        pdmSTAR.Transitions.Add(pdmProcedureTransitions);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    LogManager.GetLogger(ex.TargetSite.Name).Error(ex, aimFlightTransition.GetType().ToString() + " ID: " + aimFlightTransition.Id);

                                }
                            }

                        }

                        #endregion

                        //if (pdmSTAR.StoreToDB(CurEnvironment.Data.TableDictionary))
                        {
                            LinkCommunicationChanels(pdmSTAR, proc_chanel);
                            if (pdmSTAR.Transitions != null && pdmSTAR.Transitions.Count > 0) CurEnvironment.Data.PdmObjectList.Add(pdmSTAR);

                        }
                    }
                    catch (Exception ex)
                    {
                        LogManager.GetLogger(ex.TargetSite.Name).Error(ex, featureProc.AIXM51_Feature.GetType().ToString() + " ID: " + featureProc.AIXM51_Feature.Identifier);

                    }

                    alrtForm.progressBar1.Value++;
                    Application.DoEvents();
                }



                #endregion


                alrtForm.label1.Text = "Geo Borders";
                Application.DoEvents();

                #region GeoBorder

                var GeoBorderList = (from element in this.CurEnvironment.Data.Intermediate_AIXM51_Arena_Features
                                     where (element != null) &&
                                         (element.AIXM51_Feature.FeatureType == Aran.Aim.FeatureType.GeoBorder)
                                     select element).ToList();

                alrtForm.progressBar1.Maximum = GeoBorderList.Count;
                alrtForm.progressBar1.Value = 0;

                foreach (var featureGeoBorder in GeoBorderList)
                {

                    try
                    {

                        var aimGeoBorder = featureGeoBorder.AIXM51_Feature;


                        GeoBorder pdmBorder = (GeoBorder)AIM_PDM_Converter.AIM_Object_Convert(aimGeoBorder, featureGeoBorder.AixmGeo);
                        if (pdmBorder.Geo != null)
                        {

                            pdmBorder.BorderBlobGeometry = HelperClass.SetObjectToBlob(pdmBorder.Geo, "Border");
                        }


                        //if (pdmENRT.StoreToDB(CurEnvironment.Data.TableDictionary))
                        {
                            CurEnvironment.Data.PdmObjectList.Add(pdmBorder);
                        }

                    }
                    catch (Exception ex)
                    {
                        LogManager.GetLogger(ex.TargetSite.Name).Error(ex, featureGeoBorder.AIXM51_Feature.GetType().ToString() + " ID: " + featureGeoBorder.AIXM51_Feature.Identifier);

                    }

                    alrtForm.progressBar1.Value++;
                    Application.DoEvents();
                }

                #endregion

                alrtForm.Close();


            }

        }

        private void DataProcessing_Permdelta()
        {
            //MessageBox.Show("DataProcessing_Permdelta");

            if (this.CurEnvironment.Data.Intermediate_AIXM51_Arena_Features != null)
            {

                //var idsLst = (from ftrst in this.CurEnvironment.Data.Intermediate_AIXM51_Arena_Features
                //              where ftrst.AIXM51_Feature != null
                //              select ftrst.AIXM51_Feature.Identifier).ToList();

                

                AlertForm alrtForm = new AlertForm();
                alrtForm.FormBorderStyle = FormBorderStyle.None;
                alrtForm.Opacity = 0.5;
                alrtForm.BackgroundImage = ArenaToolBox.Properties.Resources.ArenaSplash;
                //alrtForm.TopMost = true;

                alrtForm.progressBar1.ForeColor = System.Drawing.Color.FromArgb(255, 22, 76, 108);
                alrtForm.progressBar1.Maximum = this.CurEnvironment.Data.Intermediate_AIXM51_Arena_Features.Count;
                alrtForm.progressBar1.Value = 0;

                alrtForm.label1.BackColor = System.Drawing.Color.FromArgb(255, 23, 76, 107);
                alrtForm.label1.Visible = true;


                if (!System.Diagnostics.Debugger.IsAttached) alrtForm.Show();

                ExistingGuids = new List<string>();



                //var pdmList = (from element in this.CurEnvironment.Data.Intermediate_AIXM51_Arena_Features where (element != null) && (element.AIXM51_Feature.FeatureType == FeatureType.DesignatedPoint) select element).ToList();
                List<string> finishedFeaturesList = new List<string>();

                foreach (var aimFeat in this.CurEnvironment.Data.Intermediate_AIXM51_Arena_Features)
                {
                    if (aimFeat.AIXM51_Feature.FeatureType == FeatureType.InstrumentApproachProcedure) continue;
                    if (aimFeat.AIXM51_Feature.FeatureType == FeatureType.StandardInstrumentArrival) continue;
                    if (aimFeat.AIXM51_Feature.FeatureType == FeatureType.StandardInstrumentDeparture) continue;
                    if (aimFeat.AIXM51_Feature.FeatureType.ToString().ToUpper().EndsWith("LEG")) continue;
                    if (finishedFeaturesList.Contains(aimFeat.AIXM51_Feature.Identifier.ToString())) continue;

                    alrtForm.label1.Text = aimFeat.AIXM51_Feature.FeatureType.ToString();
                    Application.DoEvents();

                  
                    var pdmFeat = AIM_PDM_Converter.AIM_Object_Convert(aimFeat.AIXM51_Feature, aimFeat.AixmGeo);

                    if (pdmFeat != null)
                    {

                        #region Airspace

                        if (pdmFeat.PDM_Type == PDM_ENUM.Airspace)
                        {

                            #region Volume

                            Airspace pdmARSPS = (Airspace)pdmFeat;
                            Aran.Aim.Features.Airspace aimARSP = (Aran.Aim.Features.Airspace)aimFeat.AIXM51_Feature;

                            pdmARSPS.AirspaceVolumeList = new List<AirspaceVolume>();

                            for (int i = 0; i <= ((Aran.Aim.Features.Airspace)aimARSP).GeometryComponent?.Count - 1; i++)
                            {
                                Aran.Aim.Features.AirspaceVolume aimArsp_vol = null;
                                try
                                {
                                    var esri_gm = aimFeat.AixmGeo != null && aimFeat.AixmGeo.Count > 0 && i <= aimFeat.AixmGeo.Count - 1 ? aimFeat.AixmGeo[i] : new PointClass { X = 0, Y = 0 };


                                    aimArsp_vol = (((Aran.Aim.Features.Airspace)aimARSP).GeometryComponent[i].TheAirspaceVolume);
                                    List<IGeometry> geoList = new List<IGeometry>();
                                    geoList.Add(esri_gm);
                                    //if (geoList == null)
                                    //    MessageBox.Show("Null geo" + pdmARSPS.GetObjectLabel());
                                    AirspaceVolume pdmARSPS_Vol = (AirspaceVolume)AIM_PDM_Converter.AIM_Object_Convert(aimArsp_vol, geoList);
                                    pdmARSPS_Vol.Interpritation = pdmARSPS.Interpritation;

                                    if (pdmARSPS_Vol == null) continue;
                                    pdmARSPS_Vol.ID = pdmARSPS.ID + "-" + (i + 1).ToString();

                                    pdmARSPS_Vol.ID_Airspace = pdmARSPS.ID;
                                    pdmARSPS_Vol.TxtName = ((Aran.Aim.Features.Airspace)aimARSP).Designator;
                                    pdmARSPS_Vol.CodeId = ((Aran.Aim.Features.Airspace)aimARSP).Designator;

                                    if (pdmARSPS_Vol.Geo != null)
                                    {
                                        pdmARSPS_Vol.BrdrGeometry = HelperClass.SetObjectToBlob(pdmARSPS_Vol.Geo, "Border");
                                    }

                                    if (pdmARSPS.Class != null && pdmARSPS.Class.Count > 0)
                                    {
                                        foreach (var cls in pdmARSPS.Class)
                                        {
                                            pdmARSPS_Vol.CodeClass = pdmARSPS_Vol.CodeClass + cls + " ";
                                        }

                                    }
                                    pdmARSPS.Lat = "";

                                    pdmARSPS.AirspaceVolumeList.Add(pdmARSPS_Vol);

                                    if (((Aran.Aim.Features.Airspace)aimARSP).Type.HasValue)
                                    {
                                        AirspaceType _uom = AirspaceType.OTHER;
                                        var cType = ArenaStatic.ArenaStaticProc.airspaceCodeType_to_AirspaceType(((Aran.Aim.Features.Airspace)aimARSP).Type.Value.ToString());

                                        Enum.TryParse<AirspaceType>(cType, out _uom);
                                        pdmARSPS_Vol.CodeType = _uom;
                                    }
                                    else
                                        pdmARSPS_Vol.CodeType = AirspaceType.OTHER;



                                }
                                catch (Exception ex)
                                {
                                    LogManager.GetLogger(ex.TargetSite.Name).Error(ex, aimArsp_vol.GetType().ToString() + " ID: " + aimArsp_vol.Id);
                                    //MessageBox.Show(aimArsp_vol.GetType().ToString() + " ID: " + aimArsp_vol.Id);
                                    //MessageBox.Show("StackTrace" + ex.StackTrace);
                                    //MessageBox.Show("Source" + ex.Source);
                                    //MessageBox.Show("Message" + ex.Message);

                                }
                            }

                            #endregion

                        }

                        #endregion

                        else if (pdmFeat.PDM_Type == PDM_ENUM.RouteSegment)
                        {
                            RouteSegment pdmRouteSeg = (RouteSegment)pdmFeat;
                            Aran.Aim.Features.Feature aimRoute = aimFeat.AIXM51_Feature;
                            PDMObject _obj = null;
                            if (pdmRouteSeg != null)
                            {

                                #region StartPoint

                                if ((((Aran.Aim.Features.RouteSegment)aimRoute).Start != null) &&
                                    (((Aran.Aim.Features.RouteSegment)aimRoute).Start.PointChoice != null) &&
                                    (((Aran.Aim.Features.RouteSegment)aimRoute).Start.PointChoice.AimingPoint != null))
                                {
                                    try
                                    {
                                        PDMObject segStartPnt = GetSegmentPoint(((Aran.Aim.Features.RouteSegment)aimRoute).Start.PointChoice.Choice.ToString(), ((Aran.Aim.Features.RouteSegment)aimRoute).Start.PointChoice.AimingPoint.Identifier);


                                        if(segStartPnt != null)
                                            _obj = DefinePointSegmentDesignator(((SegmentPoint)segStartPnt).PointChoice, segStartPnt.ID);


                                        pdmRouteSeg.StartPoint = new RouteSegmentPoint
                                        {
                                            //ID = ((Aran.Aim.Features.RouteSegment)aimRoute).Start.PointChoice.AimingPoint.Identifier.ToString(),
                                            //Route_LEG_ID = pdmRouteSeg.ID,
                                            //PointRole = ProcedureFixRoleType.ENRT,
                                            //PointUse = ProcedureSegmentPointUse.START_POINT,
                                            ID = segStartPnt.ID, //Guid.NewGuid().ToString(),
                                            Route_LEG_ID = pdmRouteSeg.ID,
                                            PointRole = ProcedureFixRoleType.ENRT,
                                            PointUse = ProcedureSegmentPointUse.START_POINT,
                                            PointChoice = segStartPnt!=null ? ((SegmentPoint)segStartPnt).PointChoice : PointChoice.DesignatedPoint,
                                            SegmentPointDesignator = segStartPnt!=null ? ((SegmentPoint)segStartPnt).PointChoiceID : null,
                                            Geo = _obj !=null ? _obj.Geo : null,

                                        };

                                        pdmRouteSeg.StartPoint.IsWaypoint = ((Aran.Aim.Features.RouteSegment)aimRoute).Start.Waypoint.HasValue ? ((Aran.Aim.Features.RouteSegment)aimRoute).Start.Waypoint.Value : false;

                                        if (((Aran.Aim.Features.RouteSegment)aimRoute).Start.RadarGuidance.HasValue) { pdmRouteSeg.StartPoint.RadarGuidance = ((Aran.Aim.Features.RouteSegment)aimRoute).Start.RadarGuidance.Value; }

                                        if (((Aran.Aim.Features.RouteSegment)aimRoute).Start.ReportingATC.HasValue)
                                        {
                                            PDM.CodeATCReporting _uomATCRRep;
                                            Enum.TryParse<PDM.CodeATCReporting>(((Aran.Aim.Features.RouteSegment)aimRoute).Start.ReportingATC.Value.ToString(), out _uomATCRRep);
                                            pdmRouteSeg.StartPoint.ReportingATC = _uomATCRRep;
                                        }


                                        if (((Aran.Aim.Features.RouteSegment)aimRoute).Start.FlyOver.HasValue) { pdmRouteSeg.StartPoint.FlyOver = ((Aran.Aim.Features.RouteSegment)aimRoute).Start.FlyOver.Value; }

                                        if (_obj != null)
                                        {
                                            pdmRouteSeg.StartPoint.PointChoiceID = segStartPnt.ID;
                                            pdmRouteSeg.StartPoint.Lat = _obj.Lat;
                                            pdmRouteSeg.StartPoint.Lon = _obj.Lon;
                                        }

                                    }
                                    catch (Exception ex)
                                    {
                                        //LogManager.GetLogger(ex.TargetSite.Name).Error(ex, ((Aran.Aim.Features.RouteSegment)aimRoute).Start.PointChoice.Choice.ToString() + " ID: " + ((Aran.Aim.Features.RouteSegment)aimRoute).Start.PointChoice.AimingPoint.Identifier);
                                        continue;
                                    }
                                }

                                #endregion


                                #region EndPoint

                                if ((((Aran.Aim.Features.RouteSegment)aimRoute).End != null) &&
                                    (((Aran.Aim.Features.RouteSegment)aimRoute).End.PointChoice != null) &&
                                    (((Aran.Aim.Features.RouteSegment)aimRoute).End.PointChoice.AimingPoint != null))
                                {
                                    try
                                    {
                                        PDMObject segEndPnt = GetSegmentPoint(((Aran.Aim.Features.RouteSegment)aimRoute).End.PointChoice.Choice.ToString(), ((Aran.Aim.Features.RouteSegment)aimRoute).End.PointChoice.AimingPoint.Identifier);

                                        _obj = null;

                                        if (segEndPnt != null)
                                             _obj = DefinePointSegmentDesignator(((SegmentPoint)segEndPnt).PointChoice, segEndPnt.ID);

                                        pdmRouteSeg.EndPoint = new RouteSegmentPoint
                                        {
                                            ID = ((Aran.Aim.Features.RouteSegment)aimRoute).End.PointChoice.AimingPoint.Identifier.ToString(),
                                            //Route_LEG_ID = pdmRouteSeg.ID,
                                            //PointRole = ProcedureFixRoleType.ENRT,
                                            //PointUse = ProcedureSegmentPointUse.END_POINT,
                                            Route_LEG_ID = pdmRouteSeg.ID,
                                            PointRole = ProcedureFixRoleType.ENRT,
                                            PointUse = ProcedureSegmentPointUse.END_POINT,
                                            PointChoice = segEndPnt != null ? ((SegmentPoint)segEndPnt).PointChoice : PointChoice.DesignatedPoint,
                                            SegmentPointDesignator = segEndPnt!=null ? ((SegmentPoint)segEndPnt).PointChoiceID : null,
                                            Geo = _obj !=null ? _obj.Geo : null,

                                        };

                                        pdmRouteSeg.EndPoint.IsWaypoint = ((Aran.Aim.Features.RouteSegment)aimRoute).End.Waypoint.HasValue ? ((Aran.Aim.Features.RouteSegment)aimRoute).End.Waypoint.Value : false;

                                        if (((Aran.Aim.Features.RouteSegment)aimRoute).End.RadarGuidance.HasValue) { pdmRouteSeg.EndPoint.RadarGuidance = ((Aran.Aim.Features.RouteSegment)aimRoute).End.RadarGuidance.Value; }

                                        if (((Aran.Aim.Features.RouteSegment)aimRoute).End.ReportingATC.HasValue)
                                        {
                                            PDM.CodeATCReporting _uomATCRRep;
                                            Enum.TryParse<PDM.CodeATCReporting>(((Aran.Aim.Features.RouteSegment)aimRoute).End.ReportingATC.Value.ToString(), out _uomATCRRep);
                                            pdmRouteSeg.EndPoint.ReportingATC = _uomATCRRep;
                                        }


                                        if (((Aran.Aim.Features.RouteSegment)aimRoute).End.FlyOver.HasValue) { pdmRouteSeg.EndPoint.FlyOver = ((Aran.Aim.Features.RouteSegment)aimRoute).End.FlyOver.Value; }

                                        if (_obj != null)
                                        {
                                            pdmRouteSeg.EndPoint.PointChoiceID = segEndPnt.ID;
                                            pdmRouteSeg.EndPoint.Lat = _obj.Lat;
                                            pdmRouteSeg.EndPoint.Lon = _obj.Lon;
                                        }

                                    }
                                    catch (Exception ex)
                                    {
                                        //LogManager.GetLogger(ex.TargetSite.Name).Error(ex, ((Aran.Aim.Features.RouteSegment)aimRoute).End.PointChoice.Choice.ToString() + " ID: " + ((Aran.Aim.Features.RouteSegment)aimRoute).End.PointChoice.AimingPoint.Identifier);
                                        continue;
                                    }
                                }

                                #endregion

                                if (pdmRouteSeg.Geo != null)
                                    pdmRouteSeg.LegBlobGeometry = HelperClass.SetObjectToBlob(pdmRouteSeg.Geo, "Leg");
                            }

                            
                        }
                        
                        else if (pdmFeat.PDM_Type == PDM_ENUM.Enroute)
                        {

                            #region RouteSegment


                            var enrt_rts = (from element in CurEnvironment.Data.Intermediate_AIXM51_Arena_Features
                                            where (element != null) &&
                                                (element.AIXM51_Feature.FeatureType == Aran.Aim.FeatureType.RouteSegment) && (((Aran.Aim.Features.RouteSegment)element.AIXM51_Feature).RouteFormed != null) &&
                                                (((Aran.Aim.Features.RouteSegment)element.AIXM51_Feature).RouteFormed.Identifier == aimFeat.AIXM51_Feature.Identifier)
                                            select element).ToList();


                            if (enrt_rts != null)
                            {
                                Enroute pdmENRT = (Enroute)pdmFeat;
                                pdmENRT.Routes = new List<RouteSegment>();

                                foreach (var featureROUTE in enrt_rts)
                                {
                                    try
                                    {
                                        var aimRoute = featureROUTE.AIXM51_Feature;

                                        {
                                            RouteSegment pdmRouteSeg = (RouteSegment)AIM_PDM_Converter.AIM_Object_Convert(aimRoute, featureROUTE.AixmGeo);
                                            if (pdmRouteSeg != null)
                                            {

                                                #region StartPoint

                                                if ((((Aran.Aim.Features.RouteSegment)aimRoute).Start != null) &&
                                                    (((Aran.Aim.Features.RouteSegment)aimRoute).Start.PointChoice != null) &&
                                                    (((Aran.Aim.Features.RouteSegment)aimRoute).Start.PointChoice.AimingPoint != null))
                                                {
                                                    try
                                                    {



                                                        PDMObject segStartPnt = GetSegmentPoint(((Aran.Aim.Features.RouteSegment)aimRoute).Start.PointChoice.Choice.ToString(), ((Aran.Aim.Features.RouteSegment)aimRoute).Start.PointChoice.AimingPoint.Identifier);

                                                        if (segStartPnt != null)
                                                        {

                                                            var _obj = (from element in CurEnvironment.Data.Intermediate_AIXM51_Arena_Features
                                                                            where (element != null) &&
                                                                                (element.AIXM51_Feature.FeatureType == Aran.Aim.FeatureType.DesignatedPoint) && 
                                                                                (element.AIXM51_Feature.Identifier.ToString().CompareTo(segStartPnt.ID) == 0) && (element.AixmGeo != null)
                                                                        select element).FirstOrDefault();

                                                            pdmRouteSeg.StartPoint = new RouteSegmentPoint
                                                            {
                                                                ID = segStartPnt.ID, //Guid.NewGuid().ToString(),
                                                                Route_LEG_ID = pdmRouteSeg.ID,
                                                                PointRole = ProcedureFixRoleType.ENRT,
                                                                PointUse = ProcedureSegmentPointUse.START_POINT,
                                                                IsWaypoint = ((SegmentPoint)segStartPnt).IsWaypoint,
                                                                PointChoice = ((SegmentPoint)segStartPnt).PointChoice,
                                                                SegmentPointDesignator = ((SegmentPoint)segStartPnt).PointChoiceID,
                                                            };

                                                            pdmRouteSeg.StartPoint.IsWaypoint = ((Aran.Aim.Features.RouteSegment)aimRoute).Start.Waypoint.HasValue ? ((Aran.Aim.Features.RouteSegment)aimRoute).Start.Waypoint.Value : false;

                                                            if (((Aran.Aim.Features.RouteSegment)aimRoute).Start.RadarGuidance.HasValue) { pdmRouteSeg.StartPoint.RadarGuidance = ((Aran.Aim.Features.RouteSegment)aimRoute).Start.RadarGuidance.Value; }

                                                            if (((Aran.Aim.Features.RouteSegment)aimRoute).Start.ReportingATC.HasValue)
                                                            {
                                                                PDM.CodeATCReporting _uomATCRRep;
                                                                Enum.TryParse<PDM.CodeATCReporting>(((Aran.Aim.Features.RouteSegment)aimRoute).Start.ReportingATC.Value.ToString(), out _uomATCRRep);
                                                                pdmRouteSeg.StartPoint.ReportingATC = _uomATCRRep;

                                                            }


                                                            if (((Aran.Aim.Features.RouteSegment)aimRoute).Start.FlyOver.HasValue) { pdmRouteSeg.StartPoint.FlyOver = ((Aran.Aim.Features.RouteSegment)aimRoute).Start.FlyOver.Value; }


                                                            if (_obj != null)
                                                            {
                                                                pdmRouteSeg.StartPoint.PointChoiceID = segStartPnt.ID;
                                                                pdmRouteSeg.StartPoint.Lat = _obj.AixmGeo != null && _obj.AixmGeo.Count > 0 ? ((IPoint)_obj.AixmGeo[0]).Y.ToString() : "";
                                                                pdmRouteSeg.StartPoint.Lon = _obj.AixmGeo != null && _obj.AixmGeo.Count > 0 ? ((IPoint)_obj.AixmGeo[0]).X.ToString() : "";
                                                            }
                                                        }
                                                        else
                                                        {
                                                            pdmRouteSeg.StartPoint = new RouteSegmentPoint
                                                            {
                                                                ID = ((Aran.Aim.Features.RouteSegment)aimRoute).Start.PointChoice.AimingPoint.Identifier.ToString(),
                                                                Route_LEG_ID = pdmRouteSeg.ID,
                                                                PointRole = ProcedureFixRoleType.ENRT,
                                                                PointUse = ProcedureSegmentPointUse.START_POINT,
                                                                PointChoice = PointChoice.DesignatedPoint,
                                                                SegmentPointDesignator = ((Aran.Aim.Features.RouteSegment)aimRoute).Start.PointChoice.AimingPoint.Identifier.ToString(),
                                                            };
                                                        }

                                                        
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        LogManager.GetLogger(ex.TargetSite.Name).Error(ex, ((Aran.Aim.Features.RouteSegment)aimRoute).Start.PointChoice.Choice.ToString() + " ID: " + ((Aran.Aim.Features.RouteSegment)aimRoute).Start.PointChoice.AimingPoint.Identifier);
                                                        continue;
                                                    }
                                                }

                                                #endregion


                                                #region EndPoint

                                                if ((((Aran.Aim.Features.RouteSegment)aimRoute).End != null) &&
                                                    (((Aran.Aim.Features.RouteSegment)aimRoute).End.PointChoice != null) &&
                                                    (((Aran.Aim.Features.RouteSegment)aimRoute).End.PointChoice.AimingPoint != null))
                                                {
                                                    try
                                                    {
                                                        PDMObject segEndPnt = GetSegmentPoint(((Aran.Aim.Features.RouteSegment)aimRoute).End.PointChoice.Choice.ToString(), ((Aran.Aim.Features.RouteSegment)aimRoute).End.PointChoice.AimingPoint.Identifier);


                                                        if (segEndPnt != null)
                                                        {
                                                            var _obj = (from element in CurEnvironment.Data.Intermediate_AIXM51_Arena_Features
                                                                        where (element != null) &&
                                                                            (element.AIXM51_Feature.FeatureType == Aran.Aim.FeatureType.DesignatedPoint) &&
                                                                            (element.AIXM51_Feature.Identifier.ToString().CompareTo(segEndPnt.ID) == 0) && (element.AixmGeo != null)
                                                                        select element).FirstOrDefault();

                                                            pdmRouteSeg.EndPoint = new RouteSegmentPoint
                                                            {
                                                                ID = segEndPnt.ID,//Guid.NewGuid().ToString(),
                                                                Route_LEG_ID = pdmRouteSeg.ID,
                                                                PointRole = ProcedureFixRoleType.ENRT,
                                                                PointUse = ProcedureSegmentPointUse.END_POINT,
                                                                IsWaypoint = ((SegmentPoint)segEndPnt).IsWaypoint,
                                                                PointChoice = ((SegmentPoint)segEndPnt).PointChoice,
                                                                SegmentPointDesignator = ((SegmentPoint)segEndPnt).PointChoiceID,

                                                            };

                                                            pdmRouteSeg.EndPoint.IsWaypoint = ((Aran.Aim.Features.RouteSegment)aimRoute).End.Waypoint.HasValue ? ((Aran.Aim.Features.RouteSegment)aimRoute).End.Waypoint.Value : false;

                                                            if (((Aran.Aim.Features.RouteSegment)aimRoute).End.RadarGuidance.HasValue) { pdmRouteSeg.EndPoint.RadarGuidance = ((Aran.Aim.Features.RouteSegment)aimRoute).End.RadarGuidance.Value; }

                                                            if (((Aran.Aim.Features.RouteSegment)aimRoute).End.ReportingATC.HasValue)
                                                            {
                                                                PDM.CodeATCReporting _uomATCRRep;
                                                                Enum.TryParse<PDM.CodeATCReporting>(((Aran.Aim.Features.RouteSegment)aimRoute).End.ReportingATC.Value.ToString(), out _uomATCRRep);
                                                                pdmRouteSeg.EndPoint.ReportingATC = _uomATCRRep;

                                                            }


                                                            if (((Aran.Aim.Features.RouteSegment)aimRoute).End.FlyOver.HasValue) { pdmRouteSeg.EndPoint.FlyOver = ((Aran.Aim.Features.RouteSegment)aimRoute).End.FlyOver.Value; }


                                                            if (_obj != null)
                                                            {
                                                                pdmRouteSeg.StartPoint.PointChoiceID = segEndPnt.ID;
                                                                pdmRouteSeg.EndPoint.Lat = _obj.AixmGeo != null && _obj.AixmGeo.Count > 0 ? ((IPoint)_obj.AixmGeo[0]).Y.ToString() : "";
                                                                pdmRouteSeg.EndPoint.Lon = _obj.AixmGeo != null && _obj.AixmGeo.Count > 0 ? ((IPoint)_obj.AixmGeo[0]).X.ToString() : "";
                                                            }
                                                        }
                                                        else
                                                        {
                                                            pdmRouteSeg.EndPoint = new RouteSegmentPoint
                                                            {
                                                                ID = ((Aran.Aim.Features.RouteSegment)aimRoute).End.PointChoice.AimingPoint.Identifier.ToString(),
                                                                Route_LEG_ID = pdmRouteSeg.ID,
                                                                PointRole = ProcedureFixRoleType.ENRT,
                                                                PointUse = ProcedureSegmentPointUse.END_POINT,
                                                                PointChoice = PointChoice.DesignatedPoint,
                                                                SegmentPointDesignator = ((Aran.Aim.Features.RouteSegment)aimRoute).End.PointChoice.AimingPoint.Identifier.ToString(),
                                                            };
                                                        }

                                                       
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        LogManager.GetLogger(ex.TargetSite.Name).Error(ex, ((Aran.Aim.Features.RouteSegment)aimRoute).End.PointChoice.Choice.ToString() + " ID: " + ((Aran.Aim.Features.RouteSegment)aimRoute).End.PointChoice.AimingPoint.Identifier);
                                                        continue;
                                                    }
                                                }

                                                #endregion

                                                if (pdmRouteSeg.Geo == null && pdmRouteSeg.StartPoint != null && pdmRouteSeg.StartPoint.Geo != null && pdmRouteSeg.EndPoint != null && pdmRouteSeg.EndPoint.Geo != null)
                                                {
                                                    IPolyline ln = new PolylineClass();
                                                    IPoint ppSt = new PointClass();
                                                    ppSt.PutCoords(((IPoint)pdmRouteSeg.StartPoint.Geo).X, ((IPoint)pdmRouteSeg.StartPoint.Geo).Y);

                                                    var zAware = ppSt as IZAware;
                                                    zAware.ZAware = true;
                                                    ppSt.Z = 0;
                                                    var mAware = ppSt as IMAware;
                                                    mAware.MAware = true;
                                                    ln.FromPoint = ppSt;

                                                    IPoint ppEn = new PointClass();
                                                    ppEn.PutCoords(((IPoint)pdmRouteSeg.EndPoint.Geo).X, ((IPoint)pdmRouteSeg.EndPoint.Geo).Y);
                                                    zAware = ppSt as IZAware;
                                                    zAware.ZAware = true;
                                                    ppEn.Z = 0;
                                                    mAware = ppSt as IMAware;
                                                    mAware.MAware = true;

                                                    ln.ToPoint = ppEn;

                                                    pdmRouteSeg.Geo = ln;


                                                    zAware = pdmRouteSeg.Geo as IZAware;
                                                    zAware.ZAware = true;
                                                    mAware = pdmRouteSeg.Geo as IMAware;
                                                    mAware.MAware = true;

                                                    pdmRouteSeg.ExeptionDetails = new ExeptionMessage { Message = "Reconstructed Geometry", Source = pdmRouteSeg.ToString(), StackTrace = pdmRouteSeg.ID };
                                                    pdmRouteSeg.SourceDetail = "Reconstructed Geometry";

                                                }


                                                if (pdmRouteSeg.Geo != null)
                                                    pdmRouteSeg.LegBlobGeometry = HelperClass.SetObjectToBlob(pdmRouteSeg.Geo, "Leg");
                                                else
                                                    pdmRouteSeg.ExeptionDetails = new ExeptionMessage { Message = "Empty Geometry", Source = pdmRouteSeg.ToString(), StackTrace = pdmRouteSeg.ID };


                                                pdmRouteSeg.ID_Route = pdmENRT.ID;
                                                pdmENRT.RouteLength = pdmENRT.RouteLength + Math.Round(pdmRouteSeg.ConvertValueToMeter(pdmRouteSeg.ValLen.Value, pdmRouteSeg.UomValLen.ToString()), 1);

                                                pdmENRT.Routes.Add(pdmRouteSeg);

                                                finishedFeaturesList.Add(pdmRouteSeg.ID);

                                                CurEnvironment.Data.PdmObjectList.RemoveAll(r => r.ID.CompareTo(pdmRouteSeg.ID) == 0);
                                            }

                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        LogManager.GetLogger(ex.TargetSite.Name).Error(ex, featureROUTE.AIXM51_Feature.GetType().ToString() + " ID: " + featureROUTE.AIXM51_Feature.Identifier);

                                    }
                                }

                            }


                            #endregion

                        }


                        CurEnvironment.Data.PdmObjectList.Add(pdmFeat);
                    }

                    alrtForm.progressBar1.Value++;
                }

                alrtForm.label1.Text = "Enroutes";

                #region Enroute

                var rtsegList = (from pdm_element in this.CurEnvironment.Data.PdmObjectList
                                 where (pdm_element != null) &&
                                  (pdm_element.PDM_Type == PDM_ENUM.RouteSegment) &&
                                  String.IsNullOrEmpty(((RouteSegment)pdm_element).RouteFormed)
                                 select pdm_element).ToList();

                var aixmrtSegList = (from aixm_element in this.CurEnvironment.Data.Intermediate_AIXM51_Arena_Features
                                     where (aixm_element != null) &&
                                      (aixm_element.AIXM51_Feature.FeatureType == FeatureType.RouteSegment) 
                                     select aixm_element).ToList(); 

                if (rtsegList != null && rtsegList.Count > 0 && aixmrtSegList != null)
                {
                    Enroute enrtPerm = new Enroute { TxtDesig = "permdeltaRoutes", Interpritation = dataInterpretation.PERMDELTA, ID = Guid.Empty.ToString() };
                    enrtPerm.Routes = new List<RouteSegment>();
                    foreach (var item in rtsegList)
                    {
                        var aixmRt = (from aixm_element in aixmrtSegList
                                      where (aixm_element != null) && (aixm_element.AIXM51_Feature  !=null) && 
                                              (aixm_element.AIXM51_Feature.Identifier.ToString().CompareTo(item.ID) == 0)
                                             select aixm_element).FirstOrDefault();
                        
                        if (aixmRt != null && ((Aran.Aim.Features.RouteSegment)aixmRt.AIXM51_Feature).RouteFormed != null)
                        {
                            ((RouteSegment)item).RouteFormed = ((Aran.Aim.Features.RouteSegment)aixmRt.AIXM51_Feature).RouteFormed.Identifier.ToString();
                        }


                        enrtPerm.Routes.Add((RouteSegment)item);
                        CurEnvironment.Data.PdmObjectList.Remove(item);
                        

                    }

                    CurEnvironment.Data.PdmObjectList.Add(enrtPerm);
                }

                #endregion



                alrtForm.label1.Text = "Procedures ";
                Application.DoEvents();


                var proc_chanel = (from element in this.CurEnvironment.Data.Intermediate_AIXM51_Arena_Features
                                   where (element != null) &&
                                    (element.AIXM51_Feature.FeatureType == Aran.Aim.FeatureType.AirTrafficControlService) &&
                                   (((Aran.Aim.Features.AirTrafficControlService)element.AIXM51_Feature).ClientProcedure != null)
                                   select element).ToList();

                #region Instrument Approach Procedures

                var iapList = (from element in this.CurEnvironment.Data.Intermediate_AIXM51_Arena_Features
                               where (element != null) &&
                                   (element.AIXM51_Feature.FeatureType == Aran.Aim.FeatureType.InstrumentApproachProcedure)
                               select element).ToList();

                alrtForm.progressBar1.Maximum = iapList.Count;
                alrtForm.progressBar1.Value = 0;

                foreach (var featureProc in iapList)
                {


                    try
                    {

                        //if (featureProc.AIXM51_Feature.Identifier.ToString().StartsWith("7261ceda-3610-4975-a8c8-ac522bd2b52c"))
                        //    System.Diagnostics.Debug.WriteLine("7261ceda-3610-4975-a8c8-ac522bd2b52c");

                        var aimPROC = featureProc.AIXM51_Feature;


                        InstrumentApproachProcedure pdmIAP = (InstrumentApproachProcedure)AIM_PDM_Converter.AIM_Object_Convert(aimPROC, null);

                        if (pdmIAP == null) continue;
                        pdmIAP.Airport_ICAO_Code = GetAirportCode(pdmIAP.AirportIdentifier);
                        if (pdmIAP.LandingArea != null)
                        {
                            List<PDMObject> lst = new List<PDMObject>();
                            foreach (var item in pdmIAP.LandingArea)
                            {
                                string Id = item.ID;
                                var rdn = LandingTakeoffArea(Id);
                                if (rdn != null) lst.Add(rdn);
                            }

                            pdmIAP.LandingArea.Clear();
                            if (lst != null && lst.Count > 0) pdmIAP.LandingArea.AddRange(lst);
                        }


                        #region Create Transitions

                        if (((Aran.Aim.Features.InstrumentApproachProcedure)aimPROC).FlightTransition != null)
                        {
                            pdmIAP.Transitions = new List<ProcedureTransitions>();


                            foreach (var aimFlightTransition in ((Aran.Aim.Features.InstrumentApproachProcedure)aimPROC).FlightTransition)
                            {
                                try
                                {
                                    ProcedureTransitions pdmProcedureTransitions = CreateProcedureTransition(aimFlightTransition, pdmIAP.ID);

                                    if (pdmProcedureTransitions != null && pdmProcedureTransitions.Legs != null && pdmProcedureTransitions.Legs.Count > 0)
                                    {
                                        pdmIAP.Transitions.Add(pdmProcedureTransitions);

                                        if (pdmIAP.Profile != null && pdmProcedureTransitions.RouteType == ProcedurePhaseType.FINAL && pdmProcedureTransitions.Legs != null && pdmProcedureTransitions.Legs.Count > 0)
                                        {

                                            pdmIAP.Profile.ApproachMinimaTable = new List<ApproachMinima>();
                                            int step = 1;
                                            foreach (ProcedureLeg leg in pdmProcedureTransitions.Legs)
                                            {
                                                if (leg.LegSpecialization == SegmentLegSpecialization.MissedApproachLeg) break;
                                                //if (step < pdmProcedureTransitions.Legs.Count - 1)
                                                {
                                                    if (leg.AssessmentArea != null && leg.AssessmentArea[0].AssessedAltitude.HasValue)
                                                    {
                                                        ApproachMinima appMin = new ApproachMinima { Minima = leg.AssessmentArea[0].AssessedAltitude.Value, MinimaUom = leg.AssessmentArea[0].AssessedAltitudeUOM };
                                                        appMin.ProfileSegmnetDesignator = leg.StartPoint.PointRole.ToString() + ":" + pdmProcedureTransitions.Legs[step].StartPoint.PointRole.ToString();

                                                        pdmIAP.Profile.ApproachMinimaTable.Add(appMin);
                                                    }
                                                }
                                                step++;
                                            }

                                        }

                                        if (pdmProcedureTransitions.Description != null)
                                        {
                                            string[] ids = pdmProcedureTransitions.Description.Split(':');
                                            if (ids.Length > 0)
                                            {
                                                foreach (var rdnId in ids)
                                                {
                                                    if (rdnId.Length <= 0) continue;
                                                    var _objRnd = (from element in CurEnvironment.Data.Intermediate_AIXM51_Arena_Features
                                                                   where (element != null) &&
                                                                       (element.AIXM51_Feature.FeatureType == Aran.Aim.FeatureType.RunwayDirection) && (((Aran.Aim.Features.RunwayDirection)element.AIXM51_Feature).UsedRunway != null) &&
                                                                       (((Aran.Aim.Features.RunwayDirection)element.AIXM51_Feature).Identifier.ToString().StartsWith(rdnId))
                                                                   select element).FirstOrDefault();

                                                    if (_objRnd != null)
                                                    {
                                                        pdmProcedureTransitions.Description = ((Aran.Aim.Features.RunwayDirection)_objRnd.AIXM51_Feature).Designator;
                                                        if (pdmIAP.LandingArea == null) pdmIAP.LandingArea = new List<PDMObject>();
                                                        var rdn = LandingTakeoffArea(((Aran.Aim.Features.RunwayDirection)_objRnd.AIXM51_Feature).Identifier.ToString());
                                                        if (rdn != null)
                                                            if (pdmIAP.LandingArea.IndexOf(rdn) < 0) pdmIAP.LandingArea.Add(rdn);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    LogManager.GetLogger(ex.TargetSite.Name).Error(ex, aimFlightTransition.GetType().ToString() + " ID: " + aimFlightTransition.Id);

                                }
                            }

                        }

                        #endregion

                        //if (pdmIAP.StoreToDB(CurEnvironment.Data.TableDictionary))
                        {
                            LinkCommunicationChanels(pdmIAP, proc_chanel);
                            if (pdmIAP.Transitions != null && pdmIAP.Transitions.Count > 0) CurEnvironment.Data.PdmObjectList.Add(pdmIAP);
                        }
                    }
                    catch (Exception ex)
                    {
                        LogManager.GetLogger(ex.TargetSite.Name).Error(ex, featureProc.AIXM51_Feature.GetType().ToString() + " ID: " + featureProc.AIXM51_Feature.Identifier);

                    }
                    alrtForm.progressBar1.Value++;
                    Application.DoEvents();
                }

                #endregion


                #region Standard Instrument Departure Procedures

                var sidList = (from element in this.CurEnvironment.Data.Intermediate_AIXM51_Arena_Features
                               where (element != null) &&
                                   (element.AIXM51_Feature.FeatureType == Aran.Aim.FeatureType.StandardInstrumentDeparture)
                               select element).ToList();

                alrtForm.progressBar1.Maximum = sidList.Count;
                alrtForm.progressBar1.Value = 0;

                foreach (var featureProc in sidList)
                {

                    try
                    {
                        var aimProc = featureProc.AIXM51_Feature;

                        //if (aimProc.Identifier.ToString().StartsWith("8c0d5006-4fc7-40d5-a522-cf9d69c2f793"))
                        //    System.Diagnostics.Debug.WriteLine("8c0d5006-4fc7-40d5-a522-cf9d69c2f793");
                        //SID
                        StandardInstrumentDeparture pdmSID = (StandardInstrumentDeparture)AIM_PDM_Converter.AIM_Object_Convert(aimProc, null);

                        if (pdmSID == null) continue;
                        pdmSID.Airport_ICAO_Code = GetAirportCode(pdmSID.AirportIdentifier);

                        if (pdmSID.LandingArea != null)
                        {
                            List<PDMObject> lst = new List<PDMObject>();
                            foreach (var item in pdmSID.LandingArea)
                            {
                                string Id = item.ID;
                                var rdn = LandingTakeoffArea(Id);
                                if (rdn != null) lst.Add(rdn);
                            }

                            pdmSID.LandingArea.Clear();
                            if (lst != null && lst.Count > 0) pdmSID.LandingArea.AddRange(lst);

                        }

                        #region Create Transitions

                        if (((Aran.Aim.Features.StandardInstrumentDeparture)aimProc).FlightTransition != null)
                        {
                            pdmSID.Transitions = new List<ProcedureTransitions>();


                            foreach (var aimFlightTransition in ((Aran.Aim.Features.StandardInstrumentDeparture)aimProc).FlightTransition)
                            {
                                try
                                {
                                    ProcedureTransitions pdmProcedureTransitions = CreateProcedureTransition(aimFlightTransition, pdmSID.ID);

                                    if (pdmProcedureTransitions != null && pdmProcedureTransitions.Legs != null && pdmProcedureTransitions.Legs.Count > 0)
                                    {
                                        if (pdmProcedureTransitions.Description != null)
                                        {
                                            string[] ids = pdmProcedureTransitions.Description.Split(':');
                                            if (ids.Length > 0)
                                            {
                                                foreach (var rdnId in ids)
                                                {
                                                    if (rdnId.Length <= 0) continue;
                                                    var _objRnd = (from element in CurEnvironment.Data.Intermediate_AIXM51_Arena_Features
                                                                   where (element != null) &&
                                                                       (element.AIXM51_Feature.FeatureType == Aran.Aim.FeatureType.RunwayDirection) && (((Aran.Aim.Features.RunwayDirection)element.AIXM51_Feature).UsedRunway != null) &&
                                                                       (((Aran.Aim.Features.RunwayDirection)element.AIXM51_Feature).Identifier.ToString().StartsWith(rdnId))
                                                                   select element).FirstOrDefault();

                                                    if (_objRnd != null)
                                                    {
                                                        pdmProcedureTransitions.Description = ((Aran.Aim.Features.RunwayDirection)_objRnd.AIXM51_Feature).Designator;
                                                        if (pdmSID.LandingArea == null) pdmSID.LandingArea = new List<PDMObject>();
                                                        var rdn = LandingTakeoffArea(((Aran.Aim.Features.RunwayDirection)_objRnd.AIXM51_Feature).Identifier.ToString());
                                                        if (rdn != null)
                                                            if (pdmSID.LandingArea.IndexOf(rdn) < 0) pdmSID.LandingArea.Add(rdn);
                                                    }
                                                }
                                            }
                                        }
                                        pdmSID.Transitions.Add(pdmProcedureTransitions);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    LogManager.GetLogger(ex.TargetSite.Name).Error(ex, aimFlightTransition.GetType().ToString() + " ID: " + aimFlightTransition.Id);

                                }
                            }

                        }

                        #endregion

                        //if (pdmSID.StoreToDB(CurEnvironment.Data.TableDictionary))
                        {
                            LinkCommunicationChanels(pdmSID, proc_chanel);
                            if (pdmSID.Transitions != null && pdmSID.Transitions.Count > 0) CurEnvironment.Data.PdmObjectList.Add(pdmSID);
                        }
                    }
                    catch (Exception ex)
                    {
                        LogManager.GetLogger(ex.TargetSite.Name).Error(ex, featureProc.AIXM51_Feature.GetType().ToString() + " ID: " + featureProc.AIXM51_Feature.Identifier);

                    }

                    alrtForm.progressBar1.Value++;
                    Application.DoEvents();
                }


                #endregion


                #region Standard Instrument Arrival Procedures

                var starList = (from element in this.CurEnvironment.Data.Intermediate_AIXM51_Arena_Features
                                where (element != null) &&
                                    (element.AIXM51_Feature.FeatureType == Aran.Aim.FeatureType.StandardInstrumentArrival)
                                select element).ToList();

                alrtForm.progressBar1.Maximum = starList.Count;
                alrtForm.progressBar1.Value = 0;

                foreach (var featureProc in starList)
                {

                    try
                    {
                        var aimProc = featureProc.AIXM51_Feature;


                        //STAR
                        StandardInstrumentArrival pdmSTAR = (StandardInstrumentArrival)AIM_PDM_Converter.AIM_Object_Convert(aimProc, null);

                        if (pdmSTAR == null) continue;
                        pdmSTAR.Airport_ICAO_Code = GetAirportCode(pdmSTAR.AirportIdentifier);
                        if (pdmSTAR.LandingArea != null)
                        {
                            List<PDMObject> lst = new List<PDMObject>();
                            foreach (var item in pdmSTAR.LandingArea)
                            {
                                string Id = item.ID;
                                var rdn = LandingTakeoffArea(Id);
                                if (rdn != null) lst.Add(rdn);
                            }

                            pdmSTAR.LandingArea.Clear();
                            if (lst != null && lst.Count > 0) pdmSTAR.LandingArea.AddRange(lst);
                        }

                        #region Create Transitions

                        if (((Aran.Aim.Features.StandardInstrumentArrival)aimProc).FlightTransition != null)
                        {
                            pdmSTAR.Transitions = new List<ProcedureTransitions>();


                            foreach (var aimFlightTransition in ((Aran.Aim.Features.StandardInstrumentArrival)aimProc).FlightTransition)
                            {
                                try
                                {
                                    ProcedureTransitions pdmProcedureTransitions = CreateProcedureTransition(aimFlightTransition, pdmSTAR.ID);

                                    if (pdmProcedureTransitions != null && pdmProcedureTransitions.Legs != null && pdmProcedureTransitions.Legs.Count > 0)
                                    {

                                        if (pdmProcedureTransitions.Description != null)
                                        {
                                            string[] ids = pdmProcedureTransitions.Description.Split(':');
                                            if (ids.Length > 0)
                                            {
                                                foreach (var rdnId in ids)
                                                {
                                                    if (rdnId.Length <= 0) continue;
                                                    var _objRnd = (from element in CurEnvironment.Data.Intermediate_AIXM51_Arena_Features
                                                                   where (element != null) &&
                                                                       (element.AIXM51_Feature.FeatureType == Aran.Aim.FeatureType.RunwayDirection) && (((Aran.Aim.Features.RunwayDirection)element.AIXM51_Feature).UsedRunway != null) &&
                                                                       (((Aran.Aim.Features.RunwayDirection)element.AIXM51_Feature).Identifier.ToString().StartsWith(rdnId))
                                                                   select element).FirstOrDefault();

                                                    if (_objRnd != null)
                                                    {
                                                        pdmProcedureTransitions.Description = ((Aran.Aim.Features.RunwayDirection)_objRnd.AIXM51_Feature).Designator;
                                                        if (pdmSTAR.LandingArea == null) pdmSTAR.LandingArea = new List<PDMObject>();
                                                        var rdn = LandingTakeoffArea(((Aran.Aim.Features.RunwayDirection)_objRnd.AIXM51_Feature).Identifier.ToString());
                                                        if (rdn != null)
                                                            if (pdmSTAR.LandingArea.IndexOf(rdn) < 0) pdmSTAR.LandingArea.Add(rdn);
                                                    }
                                                }
                                            }
                                        }

                                        pdmSTAR.Transitions.Add(pdmProcedureTransitions);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    LogManager.GetLogger(ex.TargetSite.Name).Error(ex, aimFlightTransition.GetType().ToString() + " ID: " + aimFlightTransition.Id);

                                }
                            }

                        }

                        #endregion

                        //if (pdmSTAR.StoreToDB(CurEnvironment.Data.TableDictionary))
                        {
                            LinkCommunicationChanels(pdmSTAR, proc_chanel);
                            if (pdmSTAR.Transitions != null && pdmSTAR.Transitions.Count > 0) CurEnvironment.Data.PdmObjectList.Add(pdmSTAR);

                        }
                    }
                    catch (Exception ex)
                    {
                        LogManager.GetLogger(ex.TargetSite.Name).Error(ex, featureProc.AIXM51_Feature.GetType().ToString() + " ID: " + featureProc.AIXM51_Feature.Identifier);

                    }

                    alrtForm.progressBar1.Value++;
                    Application.DoEvents();
                }



                #endregion



                alrtForm.Close();

            }

        }

        private bool ItisNeedObject(Aran.Aim.Features.Feature aimObj)
        {
            bool res = this.CurEnvironment.Data.CurrentProjectType == ArenaProjectType.ARENA;

            if (aimObj.FeatureType == FeatureType.VerticalStructure && this.CurEnvironment.Data.CurrentProjectType == ArenaProjectType.AERODROME)
            {
                Aran.Aim.Features.VerticalStructure aimVs = (Aran.Aim.Features.VerticalStructure)aimObj;

                res = ((aimVs.HostedPassengerService != null && aimVs.HostedPassengerService.Count > 0) ||
                    (aimVs.SupportedGroundLight != null && aimVs.SupportedGroundLight.Count > 0) ||
                    (aimVs.HostedNavaidEquipment != null && aimVs.HostedNavaidEquipment.Count > 0) ||
                  (aimVs.HostedSpecialNavStation != null && aimVs.HostedSpecialNavStation.Count > 0) ||
                  (aimVs.HostedUnit != null && aimVs.HostedUnit.Count > 0) ||
                  //(aimVs.HostedOrganisation != null && aimVs.HostedOrganisation.Count > 0) ||
                  (aimVs.SupportedService != null && aimVs.SupportedService.Count > 0));
            }
            else if (aimObj.FeatureType == FeatureType.Airspace && this.CurEnvironment.Data.CurrentProjectType == ArenaProjectType.AERODROME)
            {
                Aran.Aim.Features.Airspace aimArspc = (Aran.Aim.Features.Airspace)aimObj;
                if (aimArspc.Activation != null && aimArspc.Activation.Count > 0)
                {
                    foreach (var act in aimArspc.Activation)
                    {
                        if (act.Activity!=null &&  act.Activity.HasValue)
                        {
                            res = act.Activity.Value.ToString().CompareTo("BIRD") == 0;
                            if (res) break;
                        }
                    }
                }
            }

            return res;
        }

        private IGeometry ConstructVolumeGeometry(Airspace item)
        {
            IGeometry baseGeom = null;
            try
            {
                if (item.AirspaceVolumeList != null && item.AirspaceVolumeList.Count > 0)
                {
                    if (item.AirspaceVolumeList[0].Geo == null) item.AirspaceVolumeList[0].RebuildGeo2();
                    if (item.AirspaceVolumeList[0].Geo == null)
                    {
                        var theArsps = CurEnvironment.Data.PdmObjectList.FindAll(arsp => arsp.PDM_Type == PDM_ENUM.Airspace && ((Airspace)arsp).ID.CompareTo(item.VolumeGeometryComponents[0].theAirspace) == 0).FirstOrDefault();
                        if (theArsps.Geo == null) { ((Airspace)theArsps).AirspaceVolumeList[0].RebuildGeo(); }
                        if (theArsps.Geo == null) { ((Airspace)theArsps).AirspaceVolumeList[0].RebuildGeo2(); }
                        if (((Airspace)theArsps).AirspaceVolumeList[0].Geo == null && ((Airspace)theArsps).VolumeGeometryComponents!=null)
                        {
                            ConstructVolumeGeometry((Airspace)theArsps);
                        }
                        if (((Airspace)theArsps).AirspaceVolumeList[0].Geo == null)
                        {
                            ConstructVolumeGeometry((Airspace)theArsps);
                        }
                        baseGeom = ((Airspace)theArsps).AirspaceVolumeList[0].Geo;
                    }

                    if (baseGeom == null) baseGeom = item.AirspaceVolumeList[0].Geo;
                }

                IGeometry sndGeom = null;
                PDMObject arspBase = null;

                if (item.VolumeGeometryComponents != null)
                {
                    List<VolumeGeometryComponent> orderedGeoComp =
                        item.VolumeGeometryComponents.OrderBy(order => order.operationSequence).ToList();

                    if (baseGeom == null)
                    {
                        var baseGeoComp = orderedGeoComp.FindAll(g => g.operation == PDM.CodeAirspaceAggregation.BASE)
                            .FirstOrDefault();

                        if (baseGeoComp != null)
                        {
                            arspBase = CurEnvironment.Data.PdmObjectList.FindAll(arsp =>
                                    arsp.PDM_Type == PDM_ENUM.Airspace && arsp.ID.StartsWith(baseGeoComp.theAirspace))
                                .FirstOrDefault();
                            if (arspBase != null)
                            {
                                if (((Airspace) arspBase).AirspaceVolumeList[0].Geo == null)
                                    ((Airspace) arspBase).AirspaceVolumeList[0].RebuildGeo();
                                baseGeom = ((Airspace) arspBase).AirspaceVolumeList[0].Geo;
                                if (baseGeom == null) return null;
                            }
                        }
                    }

                    if (baseGeom == null) return null;

                    baseGeom.SpatialReference = this.CurEnvironment.Data.SpatialReference;
                    ITopologicalOperator2 topoOper2 = (ITopologicalOperator2) baseGeom;

                    foreach (var geoCom in orderedGeoComp)
                    {
                        if (geoCom.airspaceDependency != PDM.CodeAirspaceDependency.FULL_GEOMETRY) continue;
                        arspBase = CurEnvironment.Data.PdmObjectList.FindAll(arsp =>
                                arsp.PDM_Type == PDM_ENUM.Airspace && arsp.ID.StartsWith(geoCom.theAirspace))
                            .FirstOrDefault();

                        if (arspBase != null)
                        {
                            if (((Airspace) arspBase).AirspaceVolumeList[0].Geo == null)
                                ((Airspace) arspBase).AirspaceVolumeList[0].RebuildGeo();
                            sndGeom = ((Airspace) arspBase).AirspaceVolumeList[0].Geo;
                            if (sndGeom == null) geoCom.operation = PDM.CodeAirspaceAggregation.OTHER;
                            else sndGeom.SpatialReference = this.CurEnvironment.Data.SpatialReference;
                            geoCom.theAirspaceName =
                                ((Airspace) arspBase).TxtName != null && ((Airspace) arspBase).TxtName.Length > 0
                                    ? ((Airspace) arspBase).TxtName
                                    : "";
                        }

                        if (arspBase == null) continue;


                        switch (geoCom.operation)
                        {
                            case PDM.CodeAirspaceAggregation.OTHER:
                            case PDM.CodeAirspaceAggregation.BASE:
                            case PDM.CodeAirspaceAggregation.INTERS:
                            default:
                                break;

                            case PDM.CodeAirspaceAggregation.UNION:
                                baseGeom = topoOper2.Union(sndGeom);
                                break;

                            case PDM.CodeAirspaceAggregation.SUBTR:
                                baseGeom = topoOper2.Difference(sndGeom);
                                break;

                        }

                        if (baseGeom != null)
                        {
                            ITopologicalOperator2 simpTopoOper2 = (ITopologicalOperator2) baseGeom;
                            simpTopoOper2.IsKnownSimple_2 = false;
                            simpTopoOper2.Simplify();
                            topoOper2 = (ITopologicalOperator2) baseGeom;
                        }
                    }

                }

                return baseGeom;
            }
            catch (Exception ex)
            {
                LogManager.GetLogger(ex.TargetSite.Name).Error(ex, item.GetType().ToString() + " ID: " + item.ID  );
                return null;
            }
        }
   
        private List<RadioCommunicationChanel> CreateADHPRadioCommunicationChanels_RadioFrequencyArea(List<Intermediate_AIXM51_Arena> _chanel, Intermediate_AIXM51_Arena aimADHP, string pdmADHP_ID)
        {
            List<RadioCommunicationChanel> res = new List<RadioCommunicationChanel>();

            foreach (var cnl in _chanel)
            {
                try
                {                   
                    if (cnl.AIXM51_Feature.FeatureType == FeatureType.AirTrafficControlService)
                    {
                        Aran.Aim.Features.AirTrafficControlService aim_chanel = (Aran.Aim.Features.AirTrafficControlService)cnl.AIXM51_Feature;

                        foreach (var clnArp in aim_chanel.ClientAirport)
                        {

                            if (clnArp.Feature.Identifier.CompareTo(aimADHP.AIXM51_Feature.Identifier) != 0) continue;


                            foreach (var _radioComm in aim_chanel.RadioCommunication)
                            {

                                GetRadioChanels(pdmADHP_ID, _radioComm.Feature.Identifier, aim_chanel.Name, aim_chanel.Type.HasValue ? aim_chanel.Type.Value.ToString() : "", aim_chanel.CallSign, ref res);

                            }

                        }
                    }

                    else if (cnl.AIXM51_Feature.FeatureType == FeatureType.GroundTrafficControlService)
                    {
                        Aran.Aim.Features.GroundTrafficControlService aim_chanel = (Aran.Aim.Features.GroundTrafficControlService)cnl.AIXM51_Feature;


                        if (aim_chanel.ClientAirport.Identifier.CompareTo(aimADHP.AIXM51_Feature.Identifier) != 0) continue;


                        foreach (var _radioComm in aim_chanel.RadioCommunication)
                        {

                            GetRadioChanels(pdmADHP_ID, _radioComm.Feature.Identifier, aim_chanel.Name, aim_chanel.Type.HasValue ? aim_chanel.Type.Value.ToString() : "", aim_chanel.CallSign, ref res);

                        }



                    }
                    else if (cnl.AIXM51_Feature.FeatureType == FeatureType.InformationService)
                    {
                        Aran.Aim.Features.InformationService aim_chanel = (Aran.Aim.Features.InformationService)cnl.AIXM51_Feature;

                        foreach (var clnArp in aim_chanel.ClientAirport)
                        {

                            if (clnArp.Feature.Identifier.CompareTo(aimADHP.AIXM51_Feature.Identifier) != 0) continue;


                            foreach (var _radioComm in aim_chanel.RadioCommunication)
                            {

                                GetRadioChanels(pdmADHP_ID, _radioComm.Feature.Identifier, aim_chanel.Name, aim_chanel.Type.HasValue ? aim_chanel.Type.Value.ToString() : "", aim_chanel.CallSign, ref res);

                            }

                        }



                    }


                }
                catch (Exception ex)
                {
                    LogManager.GetLogger(ex.TargetSite.Name).Error(ex, cnl.AIXM51_Feature.GetType().ToString() + " ID: " + cnl.AIXM51_Feature.Identifier);
                    
                }
            }

            return res;
        }

        private void GetRadioChanels(string ADHP_ID, Guid _radioComm_Feature_Identifier, string aim_chanel_Name, string aim_chanel_Type, List<Aran.Aim.Features.CallsignDetail> aim_chanel_CallSign, ref List<RadioCommunicationChanel> chanelsList)
        {
            var radio_com = (from element in this.CurEnvironment.Data.Intermediate_AIXM51_Arena_Features
                             where (element != null) &&
                              (element.AIXM51_Feature.FeatureType == Aran.Aim.FeatureType.RadioCommunicationChannel) &&
                                 (((Aran.Aim.Features.RadioCommunicationChannel)element.AIXM51_Feature).Identifier.CompareTo(_radioComm_Feature_Identifier) == 0)
                             select element).ToList();
            if (radio_com != null)
            {
                foreach (var item in radio_com)
                {
                    try
                    {
                        var aimRadioComChnl = item.AIXM51_Feature;
                        RadioCommunicationChanel pdmRadioComChnl = (RadioCommunicationChanel)AIM_PDM_Converter.AIM_Object_Convert(aimRadioComChnl, item.AixmGeo);

                        if (pdmRadioComChnl != null)
                        {
                            pdmRadioComChnl.ChanelType = aim_chanel_Type;
                            pdmRadioComChnl.CallSign =
                                aim_chanel_CallSign != null && aim_chanel_CallSign.Count > 0
                                    ? aim_chanel_CallSign[0].CallSign
                                    : "";
                            if (aim_chanel_Name != null)
                                pdmRadioComChnl.Name = aim_chanel_Name;

                            pdmRadioComChnl.ID_AirportHeliport = ADHP_ID;

                            #region RadioFrequencyArea

                            var radioFrequencyArea = (from element in CurEnvironment.Data.Intermediate_AIXM51_Arena_Features
                                                      where (element != null) &&
                                                            (element.AIXM51_Feature.FeatureType == Aran.Aim.FeatureType.RadioFrequencyArea) &&
                                                            (((Aran.Aim.Features.RadioFrequencyArea)element.AIXM51_Feature).Equipment.Frequency.Identifier.ToString().StartsWith(pdmRadioComChnl.ID))
                                                      select element).ToList();

                            if ((radioFrequencyArea != null) && (radioFrequencyArea.Count > 0))
                            {

                                foreach (var featureFreqArea in radioFrequencyArea)
                                {
                                    try
                                    {
                                        var aimFreqArea = featureFreqArea.AIXM51_Feature;

                                        RadioFrequencyArea pdmFreqArea = (RadioFrequencyArea)AIM_PDM_Converter.AIM_Object_Convert(aimFreqArea, featureFreqArea.AixmGeo);
                                        if (pdmFreqArea != null)
                                        {
                                            pdmFreqArea.AirportHeliport_ID = pdmRadioComChnl.ID_AirportHeliport;
                                            pdmFreqArea.FrequencyReception = pdmRadioComChnl.FrequencyReception;
                                            pdmFreqArea.ReceptionFrequencyUOM = pdmRadioComChnl.ReceptionFrequencyUOM;
                                            pdmFreqArea.FrequencyTransmission = pdmRadioComChnl.FrequencyTransmission;
                                            pdmFreqArea.TransmissionFrequencyUOM = pdmRadioComChnl.TransmissionFrequencyUOM;

                                            var airTrafControlService = (from element in CurEnvironment.Data.Intermediate_AIXM51_Arena_Features
                                                                         where (element != null) &&
                                                                               (element.AIXM51_Feature.FeatureType == Aran.Aim.FeatureType.AirTrafficControlService) &&
                                                                               (((Aran.Aim.Features.Service)element.AIXM51_Feature).RadioCommunication.FirstOrDefault(t => t.Feature.Identifier.ToString().CompareTo(pdmRadioComChnl.ID) == 0) != null)
                                                                         select element).FirstOrDefault();

                                            pdmFreqArea.ChannelName =
                                            ((Aran.Aim.Features.Service)
                                                airTrafControlService.AIXM51_Feature).Name;

                                            CurEnvironment.Data.PdmObjectList.Add(pdmFreqArea);
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        LogManager.GetLogger(ex.TargetSite.Name).Error(ex, featureFreqArea.AIXM51_Feature.GetType().ToString() + " ID: " + featureFreqArea.AIXM51_Feature.Identifier);                                       
                                    }
                                }
                            }

                            #endregion

                            chanelsList.Add(pdmRadioComChnl);
                        }
                    }
                    catch (Exception ex)
                    {
                        LogManager.GetLogger(ex.TargetSite.Name).Error(ex, item.AIXM51_Feature.GetType().ToString() + " ID: " + item.AIXM51_Feature.Identifier);                       
                    }
                }
            }
        }

        private CodeObstacleAreaType SetObstacleAreaValue(List<Intermediate_AIXM51_Arena> area1List, CodeObstacleAreaType codeObstacleAreaType, string pdmOBS_ID)
        {
            CodeObstacleAreaType res = CodeObstacleAreaType.OTHER;

            foreach (var areaItem in area1List)
            {
                var areaFlag = ((Aran.Aim.Features.ObstacleArea)areaItem.AIXM51_Feature).Obstacle.FindAll(obs => obs.Feature.Identifier.ToString().StartsWith(pdmOBS_ID)).FirstOrDefault();
                if (areaFlag != null)
                {
                    res = codeObstacleAreaType;
                    break;
                }

            }

            return res;
        }

        private void LinkCommunicationChanels(Procedure pdmProc, List<Intermediate_AIXM51_Arena> proc_chanel)
        {
            #region Communication chanels

            if (proc_chanel != null)
            {
                pdmProc.CommunicationChanels = new List<RadioCommunicationChanel>();

                foreach (var cnl in proc_chanel)
                {
                    Aran.Aim.Features.AirTrafficControlService _chanel =
                        (Aran.Aim.Features.AirTrafficControlService) cnl.AIXM51_Feature;

                    foreach (var clnProc in _chanel.ClientProcedure)
                    {
                        if (clnProc.Feature== null || clnProc.Feature.Identifier == null || clnProc.Feature.Identifier.ToString().CompareTo(pdmProc.ID) != 0) continue;

                        foreach (var _radioComm in _chanel.RadioCommunication)
                        {
                            try
                            {
                                RadioCommunicationChanel pdm_chanel = new RadioCommunicationChanel
                                {
                                    ChanelType = _chanel.Type.ToString(),
                                    CallSign = _chanel.CallSign != null && _chanel.CallSign.Count > 0
                                        ? _chanel.CallSign[0].CallSign
                                        : "",
                                };

                                var radio_com =
                                    (from element in this.CurEnvironment.Data.Intermediate_AIXM51_Arena_Features
                                        where (element != null) &&
                                              (element.AIXM51_Feature.FeatureType ==
                                               Aran.Aim.FeatureType.RadioCommunicationChannel) &&
                                              (((Aran.Aim.Features.RadioCommunicationChannel) element.AIXM51_Feature
                                               )
                                               .Identifier.CompareTo(_radioComm.Feature.Identifier) == 0)
                                        select element).FirstOrDefault();
                                if (radio_com != null)
                                {
                                    pdm_chanel.FrequencyReception =
                                        ((Aran.Aim.Features.RadioCommunicationChannel) radio_com.AIXM51_Feature)
                                        .FrequencyReception != null
                                            ? ((Aran.Aim.Features.RadioCommunicationChannel) radio_com
                                                .AIXM51_Feature)
                                            .FrequencyReception.Value
                                            : 0;

                                    UOM_FREQ frqUom;
                                    if (((Aran.Aim.Features.RadioCommunicationChannel) radio_com.AIXM51_Feature)
                                        .FrequencyReception != null)
                                    {
                                        Enum.TryParse<UOM_FREQ>(
                                            ((Aran.Aim.Features.RadioCommunicationChannel) radio_com.AIXM51_Feature)
                                            .FrequencyReception.Uom.ToString(), out frqUom);
                                        pdm_chanel.ReceptionFrequencyUOM = frqUom;
                                    }

                                    PDM.CodeFacilityRanking rnk;
                                    if (((Aran.Aim.Features.RadioCommunicationChannel) radio_com.AIXM51_Feature)
                                        .FrequencyReception != null)
                                    {
                                        Enum.TryParse<PDM.CodeFacilityRanking>(
                                            ((Aran.Aim.Features.RadioCommunicationChannel) radio_com.AIXM51_Feature)
                                            .Rank.ToString(), out rnk);
                                        pdm_chanel.Rank = rnk;
                                    }
                                }

                                pdmProc.CommunicationChanels.Add(pdm_chanel);
                            }
                            catch (Exception ex)
                            {
                                LogManager.GetLogger(ex.TargetSite.Name).Error(ex, _radioComm.Feature.GetType().ToString() + " ID: " + _radioComm.Feature.Identifier);

                            }
                        }

                    }

                }

                if (pdmProc.CommunicationChanels.Count == 0) pdmProc.CommunicationChanels = null;
            }

            #endregion

        }

        private List<string> ExistingGuids = new List<string>();

        private string GetAirportCode(string _id)
        {
            string res = "";
            PDMObject adhp = (from element in CurEnvironment.Data.PdmObjectList
                             where (element != null) &&
                                 (element is AirportHeliport) && (((AirportHeliport)element).ID.CompareTo(_id)==0)
                             select element).FirstOrDefault();
            if (adhp != null) res = ((AirportHeliport)adhp).Designator;

            return res;

        }

        private string GetAirportName(string _id)
        {
            string res = "";
            PDMObject adhp = (from element in CurEnvironment.Data.PdmObjectList
                              where (element != null) &&
                                  (element is AirportHeliport) && (((AirportHeliport)element).ID.CompareTo(_id) == 0)
                              select element).FirstOrDefault();
            if (adhp != null) res = ((AirportHeliport)adhp).Name;

            return res;

        }


        private PDMObject LandingTakeoffArea(string _id)
        {
            PDMObject res = null;
            List<PDMObject> _objAdhp = (from element in CurEnvironment.Data.PdmObjectList
                             where (element != null) &&
                                 (element is AirportHeliport) && (((AirportHeliport)element).RunwayList != null) && (((AirportHeliport)element).RunwayList.Count>0) 
                             select element).ToList();

            foreach (AirportHeliport item in _objAdhp)
            {
                if (item.RunwayList == null) continue;

                foreach (var rwy in item.RunwayList)
                {
                    if (rwy.RunwayDirectionList == null) continue;


                    res = (from element in rwy.RunwayDirectionList
                             where (element != null) && (element.ID.CompareTo(_id)==0)
                             select element).FirstOrDefault();

                    if (res != null) return res;
                }

              
            }

            return res;
        }

        private double getStopWaylength(Guid aimThrIdentifier)
        {
            PDMObject temp = new PDMObject();

            var swy_thr = (from element in CurEnvironment.Data.Intermediate_AIXM51_Arena_Features
                           where (element != null) && (element.AIXM51_Feature.FeatureType == Aran.Aim.FeatureType.RunwayProtectArea) &&
                                (((Aran.Aim.Features.RunwayProtectArea)element.AIXM51_Feature).Type == CodeRunwayProtectionArea.STOPWAY) &&
                               (((Aran.Aim.Features.RunwayProtectArea)element.AIXM51_Feature).ProtectedRunwayDirection.Identifier == aimThrIdentifier)
                           select element).FirstOrDefault();


            if (swy_thr != null)
            {
                return temp.ConvertValueToMeter(((Aran.Aim.Features.RunwayProtectArea)swy_thr.AIXM51_Feature).Length.Value, ((Aran.Aim.Features.RunwayProtectArea)swy_thr.AIXM51_Feature).Length.Uom.ToString());
            }
            else return Double.NaN;
        }

        private double getClearWayLength(Guid aimThrIdentifier)
        {
            PDMObject temp = new PDMObject();

            var cwy_thr = (from element in CurEnvironment.Data.Intermediate_AIXM51_Arena_Features
                           where (element != null) && (element.AIXM51_Feature.FeatureType == Aran.Aim.FeatureType.RunwayProtectArea) &&
                                (((Aran.Aim.Features.RunwayProtectArea)element.AIXM51_Feature).Type == CodeRunwayProtectionArea.CWY) &&
                               (((Aran.Aim.Features.RunwayProtectArea)element.AIXM51_Feature).ProtectedRunwayDirection.Identifier == aimThrIdentifier)
                           select element).FirstOrDefault();

            if (cwy_thr != null && ((Aran.Aim.Features.RunwayProtectArea)cwy_thr.AIXM51_Feature).Length !=null)
            {
                return temp.ConvertValueToMeter(((Aran.Aim.Features.RunwayProtectArea)cwy_thr.AIXM51_Feature).Length.Value, ((Aran.Aim.Features.RunwayProtectArea)cwy_thr.AIXM51_Feature).Length.Uom.ToString());
            }
            else return Double.NaN;
        }

        private double getClearWayWidth(Guid aimThrIdentifier)
        {
            PDMObject temp = new PDMObject();

            var cwy_thr = (from element in CurEnvironment.Data.Intermediate_AIXM51_Arena_Features
                           where (element != null) && (element.AIXM51_Feature.FeatureType == Aran.Aim.FeatureType.RunwayProtectArea) &&
                                (((Aran.Aim.Features.RunwayProtectArea)element.AIXM51_Feature).Type == CodeRunwayProtectionArea.CWY) &&
                               (((Aran.Aim.Features.RunwayProtectArea)element.AIXM51_Feature).ProtectedRunwayDirection.Identifier == aimThrIdentifier)
                           select element).FirstOrDefault();


            if (cwy_thr != null && ((Aran.Aim.Features.RunwayProtectArea)cwy_thr.AIXM51_Feature).Width != null)
            {
                return temp.ConvertValueToMeter(((Aran.Aim.Features.RunwayProtectArea)cwy_thr.AIXM51_Feature).Width.Value, ((Aran.Aim.Features.RunwayProtectArea)cwy_thr.AIXM51_Feature).Width.Uom.ToString());
            }
            else return Double.NaN;
        }

        private IList<Intermediate_AIXM51_Arena> Define_AIM_List(string navEqType, Guid _Identifier)
        {
            IList<Intermediate_AIXM51_Arena> _result = null;

            switch (navEqType)
            {
                case ("DME"):

                    _result = (from element in CurEnvironment.Data.Intermediate_AIXM51_Arena_Features
                               where (element != null) &&
                                   (element.AIXM51_Feature.FeatureType == Aran.Aim.FeatureType.DME) &&
                                   (((Aran.Aim.Features.DME)element.AIXM51_Feature).Identifier == _Identifier)
                               select element).ToList();
                    break;

                case ("VOR"):

                    _result = (from element in CurEnvironment.Data.Intermediate_AIXM51_Arena_Features
                               where (element != null) &&
                                   (element.AIXM51_Feature.FeatureType == Aran.Aim.FeatureType.VOR) &&
                                   (((Aran.Aim.Features.VOR)element.AIXM51_Feature).Identifier == _Identifier)
                               select element).ToList();

                    break;
                case ("NDB"):

                    _result = (from element in CurEnvironment.Data.Intermediate_AIXM51_Arena_Features
                               where (element != null) &&
                                   (element.AIXM51_Feature.FeatureType == Aran.Aim.FeatureType.NDB) &&
                                   (((Aran.Aim.Features.NDB)element.AIXM51_Feature).Identifier == _Identifier)
                               select element).ToList();

                    break;
                case ("TACAN"):

                    _result = (from element in CurEnvironment.Data.Intermediate_AIXM51_Arena_Features
                               where (element != null) &&
                                   (element.AIXM51_Feature.FeatureType == Aran.Aim.FeatureType.TACAN) &&
                                   (((Aran.Aim.Features.TACAN)element.AIXM51_Feature).Identifier == _Identifier)
                               select element).ToList();

                    break;

                case ("Localizer"):

                    _result = (from element in CurEnvironment.Data.Intermediate_AIXM51_Arena_Features
                               where (element != null) &&
                                   (element.AIXM51_Feature.FeatureType == Aran.Aim.FeatureType.Localizer) &&
                                   (((Aran.Aim.Features.Localizer)element.AIXM51_Feature).Identifier == _Identifier)
                               select element).ToList();

                    break;

                case ("Glidepath"):

                    _result = (from element in CurEnvironment.Data.Intermediate_AIXM51_Arena_Features
                               where (element != null) &&
                                   (element.AIXM51_Feature.FeatureType == Aran.Aim.FeatureType.Glidepath) &&
                                   (((Aran.Aim.Features.Glidepath)element.AIXM51_Feature).Identifier == _Identifier)
                               select element).ToList();

                    break;

                case ("Navaid"):

                    _result = (from element in CurEnvironment.Data.Intermediate_AIXM51_Arena_Features
                               where (element != null) &&
                                   (element.AIXM51_Feature.FeatureType == Aran.Aim.FeatureType.Navaid) &&
                                   (((Aran.Aim.Features.Navaid)element.AIXM51_Feature).Identifier == _Identifier)
                               select element).ToList();

                    break;

                case ("DesignatedPoint"):

                    _result = (from element in CurEnvironment.Data.Intermediate_AIXM51_Arena_Features
                               where (element != null) &&
                                   (element.AIXM51_Feature.FeatureType == Aran.Aim.FeatureType.DesignatedPoint) &&
                                   (((Aran.Aim.Features.DesignatedPoint)element.AIXM51_Feature).Identifier == _Identifier)
                               select element).ToList();

                    break;

                case ("AirportHeliport"):
                    _result = (from element in CurEnvironment.Data.Intermediate_AIXM51_Arena_Features
                               where (element != null) &&
                                   (element.AIXM51_Feature.FeatureType == Aran.Aim.FeatureType.AirportHeliport) &&
                                   (((Aran.Aim.Features.AirportHeliport)element.AIXM51_Feature).Identifier == _Identifier)
                               select element).ToList();
                                        break;

            }

            return _result;
        }

        private IList<Intermediate_AIXM51_Arena> Define_AIM_List(Guid _Identifier)
        {
            IList<Intermediate_AIXM51_Arena> _result = null;



                    _result = (from element in CurEnvironment.Data.Intermediate_AIXM51_Arena_Features
                               where (element != null) &&
                                   ((element.AIXM51_Feature).Identifier == _Identifier)
                               select element).ToList();

                    return _result;
           
        }

        private PDMObject GetComponent(string navEqType, Guid _Identifier)
        {
            IList<Intermediate_AIXM51_Arena> _list = Define_AIM_List(navEqType, _Identifier);
            PDMObject res = null;

            if (_list != null)
            {

                foreach (var feature in _list)
                {
                    var aimFeature = feature.AIXM51_Feature;

                    if ((feature.AixmGeo.Count > 0) && (feature.AixmGeo[0].GeometryType == esriGeometryType.esriGeometryPoint))
                    {
                        res = AIM_PDM_Converter.AIM_Object_Convert(aimFeature, feature.AixmGeo);
                    }
                }
            }

            return res;
        }

        private NavaidComponent GetComponent(Guid _Identifier)
        {
            IList<Intermediate_AIXM51_Arena> _list = Define_AIM_List( _Identifier);
            PDMObject res = null;

            if (_list != null)
            {

                foreach (var feature in _list)
                {
                    var aimFeature = feature.AIXM51_Feature;

                  
                    if ((feature.AixmGeo?.Count > 0) && (feature.AixmGeo[0].GeometryType == esriGeometryType.esriGeometryPoint))
                    {
                        res = AIM_PDM_Converter.AIM_Object_Convert(aimFeature, feature.AixmGeo);
                    }
                }
            }

            return (NavaidComponent)res;
        }

        private NavigationSystemCheckpoint GetNavSystem(Guid _Identifier)
        {
            IList<Intermediate_AIXM51_Arena> _list = Define_AIM_List(_Identifier);
            PDMObject res = null;

            if (_list != null)
            {

                foreach (var feature in _list)
                {
                    var aimFeature = feature.AIXM51_Feature;


                    if ((feature.AixmGeo?.Count > 0) && (feature.AixmGeo[0].GeometryType == esriGeometryType.esriGeometryPoint))
                    {
                        res = AIM_PDM_Converter.AIM_Object_Convert(aimFeature, feature.AixmGeo);
                    }
                }
            }

            return (NavigationSystemCheckpoint)res;
        }

        private PDMObject DefinePointSegmentDesignator(PointChoice pointChoice, string Identifier)
        {
            PDMObject res = new PDMObject();
            IGeometry pntgeo = null;
            try
            {
                switch (pointChoice)
                {
                    case PointChoice.DesignatedPoint:

                        PDMObject _dpn = (from element in CurEnvironment.Data.PdmObjectList where (element != null) && (element is WayPoint) && (((WayPoint)element).ID.CompareTo(Identifier) == 0) select element).FirstOrDefault();

                        if (_dpn != null)
                        {
                            res.ID = ((WayPoint)_dpn).Designator;

                            if ((((WayPoint)_dpn).Geo != null) && (((WayPoint)_dpn).Geo.GeometryType == esriGeometryType.esriGeometryPoint))
                            {
                                res.Lon = ((IPoint)(_dpn.Geo)).X.ToString();
                                res.Lat = ((IPoint)(_dpn.Geo)).Y.ToString();
                                pntgeo = _dpn.Geo;
                            }


                        }

                        break;

                    case PointChoice.Navaid:

                        res = (from element in CurEnvironment.Data.PdmObjectList where (element != null) && (element is NavaidSystem) && (((NavaidSystem)element).ID.CompareTo(Identifier) == 0 || ((NavaidSystem)element).ID_Feature.CompareTo(Identifier) == 0) select element).FirstOrDefault();

                        if (res == null)
                        {
                            bool nav_flag = false;
                            var _adhpLst = (from element in CurEnvironment.Data.PdmObjectList where (element != null) && (element is AirportHeliport) && (((AirportHeliport)element).RunwayList != null) select element).ToList();

                            if (_adhpLst != null)
                            {
                                foreach (AirportHeliport adhp in _adhpLst)
                                {
                                    if (adhp.RunwayList.Count <= 0) continue;
                                    foreach (Runway rwy in adhp.RunwayList)
                                    {
                                        if (rwy.RunwayDirectionList == null || rwy.RunwayDirectionList.Count <= 0) continue;
                                        foreach (RunwayDirection rdn in rwy.RunwayDirectionList)
                                        {
                                            if (rdn.Related_NavaidSystem == null || rdn.Related_NavaidSystem.Count <= 0) continue;
                                            
                                            res = (from element in rdn.Related_NavaidSystem where (element != null) && (element is NavaidSystem) && (element.ID.CompareTo(Identifier) == 0 || ((NavaidSystem)element).ID_Feature.CompareTo(Identifier) == 0) select element).FirstOrDefault();

                                            if (res != null) { nav_flag = true; break; }
                                        }

                                        if (nav_flag) break;
                                    }
                                    if (nav_flag) break;
                                }

                            }
                        }

                        if (res != null)
                        {
                            //res.ID = ((NavaidSystem)res).Designator;
                            if ((((NavaidSystem)res).Components != null) && (((NavaidSystem)res).Components.Count > 0))
                            {
                                res.Lon = ((IPoint)(((NavaidSystem)res).Components[0].Geo)).X.ToString();
                                res.Lat = ((IPoint)(((NavaidSystem)res).Components[0].Geo)).Y.ToString();
                                pntgeo = ((NavaidSystem)res).Components[0].Geo;
                                if (((NavaidSystem)res).Designator !=null) res.ID = ((NavaidSystem)res).Designator;
                            }
                        }

                        break;

                    case PointChoice.RunwayCentrelinePoint:

                        PDMObject _rcp = null;
                        bool flag = false;
                        var _objLst = (from element in CurEnvironment.Data.PdmObjectList where (element != null) && (element is AirportHeliport) && (((AirportHeliport)element).RunwayList != null) select element).ToList();

                        if (_objLst != null)
                        {
                            foreach (AirportHeliport adhp in _objLst)
                            {
                                if (adhp.RunwayList.Count <= 0) continue;
                                foreach (Runway rwy in adhp.RunwayList)
                                {
                                    if (rwy.RunwayDirectionList == null || rwy.RunwayDirectionList.Count <= 0) continue;
                                    foreach (RunwayDirection rdn in rwy.RunwayDirectionList)
                                    {
                                        if (rdn.CenterLinePoints == null || rdn.CenterLinePoints.Count <= 0) continue;

                                        _rcp = (from element in rdn.CenterLinePoints where (element != null) && (element is RunwayCenterLinePoint) && (((RunwayCenterLinePoint)element).ID.CompareTo(Identifier) == 0) select element).FirstOrDefault();

                                        if (_rcp != null) { flag = true; break; }
                                    }

                                    if (flag) break;
                                }
                                if (flag) break;
                            }

                        }

                        if (_rcp != null)
                        {
                            res.ID = ((RunwayCenterLinePoint)_rcp).Designator;
                            if ((((RunwayCenterLinePoint)_rcp).Geo != null) && (((RunwayCenterLinePoint)_rcp).Geo.GeometryType == esriGeometryType.esriGeometryPoint))
                            {
                                res.Lon = ((IPoint)(_rcp.Geo)).X.ToString();
                                res.Lat = ((IPoint)(_rcp.Geo)).Y.ToString();
                                pntgeo = _rcp.Geo;

                            }
                        }

                        break;

                    case PointChoice.AirportHeliport:

                        PDMObject _ahp = (from element in CurEnvironment.Data.PdmObjectList where (element != null) && (element is AirportHeliport) && (((AirportHeliport)element).ID.CompareTo(Identifier) == 0) select element).FirstOrDefault();

                        if (_ahp != null)
                        {
                            res.ID = ((AirportHeliport)_ahp).Designator;
                            if ((((AirportHeliport)_ahp).Geo != null) && (((AirportHeliport)_ahp).Geo.GeometryType == esriGeometryType.esriGeometryPoint))
                            {
                                res.Lon = ((IPoint)(_ahp.Geo)).X.ToString();
                                res.Lat = ((IPoint)(_ahp.Geo)).Y.ToString();
                                pntgeo = _ahp.Geo;

                            }
                        }

                        break;

                    default:
                        res.ID = "";
                        res.Lon = "0";
                        res.Lat = "0";
                        break;
                }

                res.Geo = (pntgeo != null) ? pntgeo : null;

                return res;
            }
            catch (Exception ex)
            {
                LogManager.GetLogger(ex.TargetSite.Name).Error(ex, pointChoice + " ID: " + Identifier);
                return null;
            }
        }

        private PDMObject GetSegmentPoint(string navEqType, Guid _Identifier)
        {
            IList<Intermediate_AIXM51_Arena> _list = Define_AIM_List(navEqType, _Identifier);
            SegmentPoint res = null;

            if (_list != null)
            {

                foreach (var feature in _list)
                {
                    try
                    {
                        var aimFeature = feature.AIXM51_Feature;

                        switch (navEqType)
                        {
                            case ("DME"):
                            case ("VOR"):
                            case ("NDB"):
                            case ("TACAN"):
                            case ("Localizer"):
                            case ("Glidepath"):
                                res = new SegmentPoint
                                {
                                    ID = ((Aran.Aim.Features.NavaidEquipment)aimFeature).Identifier.ToString(),
                                    IsWaypoint = false,
                                    PointChoice = PointChoice.Navaid,
                                    PointChoiceID = ((Aran.Aim.Features.NavaidEquipment)aimFeature).Designator,

                                };

                                break;
                            case ("Navaid"):

                                res = new SegmentPoint
                                {
                                    ID = ((Aran.Aim.Features.Navaid)aimFeature).Identifier.ToString(),
                                    IsWaypoint = false,
                                    PointChoice = PointChoice.Navaid,
                                    PointChoiceID = ((Aran.Aim.Features.Navaid)aimFeature).Designator,
                                    SegmentPointDesignator = ((Aran.Aim.Features.Navaid)aimFeature).Type.HasValue? ((Aran.Aim.Features.Navaid)aimFeature).Type.Value.ToString() : "",
                                };

                                break;

                            case ("DesignatedPoint"):

                                res = new SegmentPoint
                                {
                                    ID = ((Aran.Aim.Features.DesignatedPoint)aimFeature).Identifier.ToString(),
                                    IsWaypoint = false,
                                    PointChoice = PointChoice.DesignatedPoint,
                                    PointChoiceID = ((Aran.Aim.Features.DesignatedPoint)aimFeature).Designator,
                                };

                                break;

                            case ("AirportHeliport"):
                                res = new SegmentPoint
                                {
                                    ID = ((Aran.Aim.Features.AirportHeliport)aimFeature).Identifier.ToString(),
                                    IsWaypoint = false,
                                    PointChoice = PointChoice.AirportHeliport,
                                    PointChoiceID = ((Aran.Aim.Features.AirportHeliport)aimFeature).Designator,
                                };
                                break;


                        }
                    }
                    catch (Exception ex)
                    {
                        LogManager.GetLogger(ex.TargetSite.Name).Error(ex, navEqType + " ID: " + _Identifier);                        
                    }
                }
            }

            return res;

        }

        private ProcedureTransitions CreateProcedureTransition(Aran.Aim.Features.ProcedureTransition aimFlightTransition, string ProcID)
        {
            ProcedureTransitions pdmProcedureTransitions = (ProcedureTransitions)AIM_PDM_Converter.AIM_Object_Convert(aimFlightTransition, null);
            if (pdmProcedureTransitions != null)
            {
                pdmProcedureTransitions.ID_procedure = ProcID;
                pdmProcedureTransitions.Legs = new List<ProcedureLeg>();


                int indx = 0;
                foreach (Aran.Aim.Features.ProcedureTransitionLeg aimLeg in aimFlightTransition.TransitionLeg)
                {
                    try
                    {
                        if (aimLeg.TheSegmentLeg == null) continue;

                        #region ProcedureLeg

                        Intermediate_AIXM51_Arena aimFeatureLeg = null;

                        //if (aimLeg.TheSegmentLeg.Identifier.ToString().StartsWith("66e012ec-82f1-40ff-ae7a-f52d2b62bd67"))
                        //    System.Diagnostics.Debug.WriteLine("");

                        switch (aimLeg.TheSegmentLeg.Type)
                        {
                            #region

                            case Aran.Aim.SegmentLegType.ArrivalFeederLeg:

                                aimFeatureLeg = (from element in CurEnvironment.Data.Intermediate_AIXM51_Arena_Features
                                                 where (element != null) &&
                                                     (element.AIXM51_Feature.FeatureType == Aran.Aim.FeatureType.ArrivalFeederLeg) &&
                                                     (((Aran.Aim.Features.ArrivalFeederLeg)element.AIXM51_Feature).Identifier == aimLeg.TheSegmentLeg.Identifier)
                                                 select element).FirstOrDefault();

                                break;

                            case Aran.Aim.SegmentLegType.FinalLeg:

                                aimFeatureLeg = (from element in CurEnvironment.Data.Intermediate_AIXM51_Arena_Features
                                                 where (element != null) &&
                                                     (element.AIXM51_Feature.FeatureType == Aran.Aim.FeatureType.FinalLeg) &&
                                                     (((Aran.Aim.Features.FinalLeg)element.AIXM51_Feature).Identifier == aimLeg.TheSegmentLeg.Identifier)
                                                 select element).FirstOrDefault();
                                break;

                            case Aran.Aim.SegmentLegType.InitialLeg:

                                aimFeatureLeg = (from element in CurEnvironment.Data.Intermediate_AIXM51_Arena_Features
                                                 where (element != null) &&
                                                     (element.AIXM51_Feature.FeatureType == Aran.Aim.FeatureType.InitialLeg) &&
                                                     (((Aran.Aim.Features.InitialLeg)element.AIXM51_Feature).Identifier == aimLeg.TheSegmentLeg.Identifier)
                                                 select element).FirstOrDefault();

                                break;

                            case Aran.Aim.SegmentLegType.IntermediateLeg:

                                aimFeatureLeg = (from element in CurEnvironment.Data.Intermediate_AIXM51_Arena_Features
                                                 where (element != null) &&
                                                     (element.AIXM51_Feature.FeatureType == Aran.Aim.FeatureType.IntermediateLeg) &&
                                                     (((Aran.Aim.Features.IntermediateLeg)element.AIXM51_Feature).Identifier == aimLeg.TheSegmentLeg.Identifier)
                                                 select element).FirstOrDefault();

                                break;

                            case Aran.Aim.SegmentLegType.MissedApproachLeg:

                                aimFeatureLeg = (from element in CurEnvironment.Data.Intermediate_AIXM51_Arena_Features
                                                 where (element != null) &&
                                                     (element.AIXM51_Feature.FeatureType == Aran.Aim.FeatureType.MissedApproachLeg) &&
                                                     (((Aran.Aim.Features.MissedApproachLeg)element.AIXM51_Feature).Identifier == aimLeg.TheSegmentLeg.Identifier)
                                                 select element).FirstOrDefault();

                                break;

                            case Aran.Aim.SegmentLegType.ArrivalLeg:

                                aimFeatureLeg = (from element in CurEnvironment.Data.Intermediate_AIXM51_Arena_Features
                                                 where (element != null) &&
                                                     (element.AIXM51_Feature.FeatureType == Aran.Aim.FeatureType.ArrivalLeg) &&
                                                     (((Aran.Aim.Features.ArrivalLeg)element.AIXM51_Feature).Identifier == aimLeg.TheSegmentLeg.Identifier)
                                                 select element).FirstOrDefault();

                                break;

                            case Aran.Aim.SegmentLegType.DepartureLeg:

                                aimFeatureLeg = (from element in CurEnvironment.Data.Intermediate_AIXM51_Arena_Features
                                                 where (element != null) &&
                                                     (element.AIXM51_Feature.FeatureType == Aran.Aim.FeatureType.DepartureLeg) &&
                                                     (((Aran.Aim.Features.DepartureLeg)element.AIXM51_Feature).Identifier == aimLeg.TheSegmentLeg.Identifier)
                                                 select element).FirstOrDefault();

                                break;

                            default:
                                break;

                                #endregion
                        }

                        if (aimFeatureLeg != null)
                        {


                            //if (aimFeatureLeg.AixmGeo.Count > 0)
                            {

                                var geoDataList = GetGeometry(aimFeatureLeg.AixmGeo);
                                ProcedureLeg pdmLeg = (ProcedureLeg)AIM_PDM_Converter.AIM_Object_Convert(aimFeatureLeg.AIXM51_Feature, geoDataList);
                                while (ExistingGuids.IndexOf(pdmLeg.ID) >= 0) pdmLeg.ID = Guid.NewGuid().ToString();
                                ExistingGuids.Add(pdmLeg.ID);


                                if (pdmLeg != null)
                                {
                                    pdmLeg.ProcedureIdentifier = ProcID;
                                    pdmLeg.TransitionIdentifier = pdmProcedureTransitions.ID;
                                    pdmLeg.SeqNumberARINC = aimLeg.SeqNumberARINC.HasValue ? Convert.ToInt32(aimLeg.SeqNumberARINC.Value) : 0;
                                    if (pdmLeg.Geo != null)
                                        pdmLeg.LegBlobGeometry = "";//HelperClass.SetObjectToBlob(pdmLeg.Geo, "Leg");

                                    #region StartPoint

                                    if (pdmLeg.StartPoint != null)
                                    {
                                        PDMObject _obj = DefinePointSegmentDesignator(pdmLeg.StartPoint.PointChoice, pdmLeg.StartPoint.PointChoiceID);
                                        pdmLeg.StartPoint.PointChoiceID = pdmLeg.StartPoint.PointChoiceID;
                                        pdmLeg.StartPoint.Lat = _obj.Lat;
                                        pdmLeg.StartPoint.Lon = _obj.Lon;
                                        pdmLeg.StartPoint.SegmentPointDesignator = _obj.ID;

                                        if (pdmLeg.StartPoint.PointFacilityMakeUp != null)
                                        {
                                            FillPointFacilityMakeUpProperies(pdmLeg.StartPoint.PointFacilityMakeUp);
                                        }

                                    }

                                    #endregion

                                    #region EndPoint

                                    if (pdmLeg.EndPoint != null)
                                    {
                                        PDMObject _obj = DefinePointSegmentDesignator(pdmLeg.EndPoint.PointChoice, pdmLeg.EndPoint.PointChoiceID);
                                        pdmLeg.EndPoint.PointChoiceID = pdmLeg.EndPoint.PointChoiceID;
                                        pdmLeg.EndPoint.Lat = _obj.Lat;
                                        pdmLeg.EndPoint.Lon = _obj.Lon;
                                        pdmLeg.EndPoint.SegmentPointDesignator = _obj.ID;

                                        if (pdmLeg.EndPoint.PointFacilityMakeUp != null)
                                        {
                                            FillPointFacilityMakeUpProperies(pdmLeg.EndPoint.PointFacilityMakeUp);
                                        }

                                    }

                                    #endregion

                                    #region ArcCentre

                                    if (pdmLeg.ArcCentre != null)
                                    {
                                        PDMObject _obj = DefinePointSegmentDesignator(pdmLeg.ArcCentre.PointChoice, pdmLeg.ArcCentre.PointChoiceID);
                                        pdmLeg.ArcCentre.PointChoiceID = pdmLeg.ArcCentre.PointChoiceID;
                                        pdmLeg.ArcCentre.Lat = _obj.Lat;
                                        pdmLeg.ArcCentre.Lon = _obj.Lon;
                                        pdmLeg.ArcCentre.SegmentPointDesignator = _obj.ID;

                                        if (pdmLeg.ArcCentre.PointFacilityMakeUp != null)
                                        {
                                            FillPointFacilityMakeUpProperies(pdmLeg.ArcCentre.PointFacilityMakeUp);
                                        }

                                    }

                                    #endregion

                                    var aimSegmentLeg = (Aran.Aim.Features.SegmentLeg)aimFeatureLeg.AIXM51_Feature;
                                    #region Holding

                                    if (aimSegmentLeg != null && aimSegmentLeg.Holding != null && aimSegmentLeg.Holding.TheHoldingPattern != null && aimSegmentLeg.Holding.TheHoldingPattern.Identifier != null)
                                    {
                                        //    var holdingList = (from element in this.CurEnvironment.Data.Intermediate_AIXM51_Arena_Features
                                        //                       where (element != null) &&
                                        //                           (element.AIXM51_Feature.FeatureType == Aran.Aim.FeatureType.HoldingPattern) &&
                                        //                           (((Aran.Aim.Features.HoldingPattern)element.AIXM51_Feature).Identifier == aimSegmentLeg.Holding.TheHoldingPattern.Identifier)
                                        //                       select element).FirstOrDefault();


                                        var holdingList = (from element in this.CurEnvironment.Data.PdmObjectList
                                                           where (element != null) &&
                                                               (element.PDM_Type == PDM_ENUM.HoldingPattern) &&
                                                               (element.ID.CompareTo(aimSegmentLeg.Holding.TheHoldingPattern.Identifier.ToString()) == 0)
                                                           select element).FirstOrDefault();


                                        if (holdingList != null)
                                        {

                                            pdmLeg.HoldingId = aimSegmentLeg.Holding.TheHoldingPattern.Identifier.ToString();


                                            HoldingPattern pdmHolding_O = (HoldingPattern)holdingList; //AIM_PDM_Converter.AIM_Object_Convert(holdingList.AIXM51_Feature, holdingList.AixmGeo);

                                            if (pdmHolding_O != null)
                                            {
                                                HoldingPattern pdmHolding = (HoldingPattern)pdmHolding_O.Clone(true);
                                                pdmHolding.ID_Transition = pdmProcedureTransitions.ID;
                                                pdmHolding.ProcedureLegID = pdmLeg.ID;
                                                pdmHolding.HodingBorder = HelperClass.SetObjectToBlob(pdmHolding.Geo, "Border");
                                                pdmLeg.HoldingUse = pdmHolding;

                                                pdmHolding_O.Lat = "removeMe";

                                                //this.CurEnvironment.Data.PdmObjectList.Remove(pdmHolding_O);
                                            }


                                        }
                                    }

                                    #endregion

                                    #region Angle

                                    if (aimSegmentLeg.Angle != null)
                                    {
                                        var _andleInd = (from element in this.CurEnvironment.Data.Intermediate_AIXM51_Arena_Features
                                                         where (element != null) &&
                                                             (element.AIXM51_Feature.FeatureType == Aran.Aim.FeatureType.AngleIndication) &&
                                                             (((Aran.Aim.Features.AngleIndication)element.AIXM51_Feature).Identifier == aimSegmentLeg.Angle.Identifier)
                                                         select element).FirstOrDefault();

                                        if (_andleInd != null)
                                        {
                                            var aimAngleInd = (Aran.Aim.Features.AngleIndication)_andleInd.AIXM51_Feature;
                                            pdmLeg.AngleIndication = new AngleIndication();
                                            pdmLeg.AngleIndication.ID = Guid.NewGuid().ToString();
                                            if (aimAngleInd.Angle.HasValue) pdmLeg.AngleIndication.Angle = aimAngleInd.Angle.Value;

                                            if (aimAngleInd.AngleType.HasValue)
                                            {
                                                PDM.BearingType _uomAnglType;
                                                Enum.TryParse<PDM.BearingType>(aimAngleInd.AngleType.Value.ToString(), out _uomAnglType);
                                                pdmLeg.AngleIndication.AngleType = _uomAnglType;
                                            }

                                            if (aimAngleInd.IndicationDirection.HasValue)
                                            {
                                                PDM.CodeDirectionReference _uomDir;
                                                Enum.TryParse<PDM.CodeDirectionReference>(aimAngleInd.IndicationDirection.Value.ToString(), out _uomDir);
                                                pdmLeg.AngleIndication.IndicationDirection = _uomDir;
                                            }

                                            if (aimAngleInd.TrueAngle.HasValue) pdmLeg.AngleIndication.TrueAngle = aimAngleInd.TrueAngle.Value;

                                            if (aimAngleInd.CardinalDirection.HasValue)
                                            {
                                                PDM.CodeCardinalDirection _uomDir;
                                                Enum.TryParse<PDM.CodeCardinalDirection>(aimAngleInd.CardinalDirection.Value.ToString(), out _uomDir);
                                                pdmLeg.AngleIndication.CardinalDirection = _uomDir;
                                            }

                                            if (aimAngleInd.MinimumReceptionAltitude != null)
                                            {
                                                pdmLeg.AngleIndication.MinimumReceptionAltitude = aimAngleInd.MinimumReceptionAltitude.Value;

                                                UOM_DIST_VERT _uomVert;
                                                Enum.TryParse<UOM_DIST_VERT>(aimAngleInd.MinimumReceptionAltitude.Uom.ToString(), out _uomVert);
                                                pdmLeg.AngleIndication.MinimumReceptionAltitudeUOM = _uomVert;
                                            }

                                            if ((aimAngleInd.Fix != null) && (aimAngleInd.Fix.Identifier != null)) pdmLeg.AngleIndication.FixID = aimAngleInd.Fix.Identifier.ToString();

                                            if (aimAngleInd.PointChoice != null)
                                            {
                                                pdmLeg.AngleIndication.SignificantPointID = DefineSignifantPointID(aimAngleInd.PointChoice);
                                                pdmLeg.AngleIndication.SignificantPointType = aimAngleInd.PointChoice.Choice.ToString();

                                            }


                                        }
                                    }

                                    #endregion

                                    #region Distance

                                    if (aimSegmentLeg.Distance != null)
                                    {
                                        var _distInd = (from element in this.CurEnvironment.Data.Intermediate_AIXM51_Arena_Features
                                                        where (element != null) &&
                                                            (element.AIXM51_Feature.FeatureType == Aran.Aim.FeatureType.DistanceIndication) &&
                                                            (((Aran.Aim.Features.DistanceIndication)element.AIXM51_Feature).Identifier == aimSegmentLeg.Distance.Identifier)
                                                        select element).FirstOrDefault();

                                        if (_distInd != null)
                                        {
                                            var aimDistInd = (Aran.Aim.Features.DistanceIndication)_distInd.AIXM51_Feature;
                                            pdmLeg.DistanceIndication = new DistanceIndication();
                                            pdmLeg.DistanceIndication.ID = Guid.NewGuid().ToString();

                                            if (aimDistInd.Distance != null)
                                            {
                                                pdmLeg.DistanceIndication.Distance = aimDistInd.Distance.Value;

                                                UOM_DIST_HORZ _uomHor;
                                                Enum.TryParse<UOM_DIST_HORZ>(aimDistInd.Distance.Uom.ToString(), out _uomHor);
                                                pdmLeg.DistanceIndication.DistanceUOM = _uomHor;

                                            }

                                            if (aimDistInd.MinimumReceptionAltitude != null)
                                            {
                                                pdmLeg.DistanceIndication.MinimumReceptionAltitude = aimDistInd.MinimumReceptionAltitude.Value;

                                                UOM_DIST_VERT _uomVer;
                                                Enum.TryParse<UOM_DIST_VERT>(aimDistInd.MinimumReceptionAltitude.Uom.ToString(), out _uomVer);
                                                pdmLeg.DistanceIndication.MinimumReceptionAltitudeUOM = _uomVer;
                                            }

                                            if (aimDistInd.PointChoice != null)
                                            {
                                                pdmLeg.DistanceIndication.SignificantPointID = DefineSignifantPointID(aimDistInd.PointChoice);
                                                pdmLeg.DistanceIndication.SignificantPointType = aimDistInd.PointChoice.Choice.ToString();

                                            }

                                            if (aimDistInd.Fix != null)
                                            {
                                                pdmLeg.DistanceIndication.FixID = aimDistInd.Fix.Identifier.ToString();
                                            }



                                        }
                                    }

                                    #endregion


                                    pdmLeg.PositionFlag = indx;

                                    if (indx == aimFlightTransition.TransitionLeg.Count - 1 && aimFlightTransition.TransitionLeg.Count > 1)
                                    {
                                        pdmLeg.PositionFlag = 111;
                                    }

                                    pdmProcedureTransitions.Legs.Add(pdmLeg);

                                }

                                else
                                {
                                    System.Diagnostics.Debug.WriteLine("pdmLeg = null");
                                }
                            }
                            //else
                            //{
                            //    System.Diagnostics.Debug.WriteLine("geoDataList.Count = 0");
                            //}


                        }

                        #endregion

                        indx++;
                    }
                    catch (Exception ex)
                    {
                        LogManager.GetLogger(ex.TargetSite.Name).Error(ex, aimLeg.GetType().ToString() + " ID: " + aimLeg.Id);                       
                    }
                }


            }

            #region leg.LegTypeARINC == IF (empty geometry)
            //  добавлено 01.09.2016
            //если первый лег - точка IF, он не содержит геометрии. Для избежания сложностей в картографии, создадим ему геометрию
            //для этого берем следующий после IF лег, берем его точку старт и создаем из нее псевдо геометрию для первого лега IF

            try
            {
                if (pdmProcedureTransitions.Legs != null && pdmProcedureTransitions.Legs.Count >= 2)
                {
                    for (int i = 0; i < pdmProcedureTransitions.Legs.Count; i++)
                    {
                        ProcedureLeg legCur = pdmProcedureTransitions.Legs[i];
                        if (legCur != null && legCur.LegTypeARINC == PDM.CodeSegmentPath.IF && legCur.Geo == null && i == 0)
                        {
                            ProcedureLeg legNext = pdmProcedureTransitions.Legs[1];
                            if (legNext.Geo == null) legNext.RebuildGeo();
                            if (legNext.Geo == null) break;

                            IPointCollection gmLg = (IPointCollection)legNext.Geo;
                            IPoint start = gmLg.get_Point(0);
                            IPoint end = gmLg.get_Point(0);
                            IPolyline ln = new PolylineClass();
                            ln.FromPoint = start;
                            ln.ToPoint = end;

                            var zAware = ln as IZAware;
                            zAware.ZAware = true;
                            var mAware = ln as IMAware;
                            mAware.MAware = true;

                            legCur.Geo = ln;

                            legCur.LegBlobGeometry = "";//HelperClass.SetObjectToBlob(legCur.Geo, "Leg");

                            break;
                        }
                    }

                }
                
            }
            catch (Exception ex)
            {
                LogManager.GetLogger(ex.TargetSite.Name).Error(ex, aimFlightTransition.GetType().ToString() + " ID: " + aimFlightTransition.Id + "(Создание псевдо геометрии для точки IF)");
                return pdmProcedureTransitions;
            }

            #endregion

            return pdmProcedureTransitions;
        }

        private void CreateHoldingPoint(Aran.Aim.Features.HoldingPattern aimHLNG, List<IGeometry> aimGeo, ref HoldingPattern pdmHolding)
        {
            #region HoldingPoint

            if ((aimHLNG.HoldingPoint != null) &&
                (aimHLNG.HoldingPoint.PointChoice != null) &&
                (aimHLNG.HoldingPoint.PointChoice.AimingPoint != null))
            {
                PDMObject holdingPnt = GetSegmentPoint(aimHLNG.HoldingPoint.PointChoice.Choice.ToString(), aimHLNG.HoldingPoint.PointChoice.AimingPoint.Identifier);
                if (holdingPnt != null)
                {
                    PDMObject _obj = DefinePointSegmentDesignator(((SegmentPoint)holdingPnt).PointChoice, holdingPnt.ID);

                    pdmHolding.HoldingPoint = new PDM.SegmentPoint
                    {
                        ID = Guid.NewGuid().ToString(),
                        Route_LEG_ID = pdmHolding.ID,
                        PointRole = ProcedureFixRoleType.ENRT_HLDNG,
                        PointUse = ProcedureSegmentPointUse.START_POINT,
                        IsWaypoint = ((SegmentPoint)holdingPnt).IsWaypoint,
                        PointChoice = ((SegmentPoint)holdingPnt).PointChoice,
                        SegmentPointDesignator = ((SegmentPoint)holdingPnt).PointChoiceID,

                    };

                    pdmHolding.HoldingPoint.IsWaypoint = aimHLNG.HoldingPoint.Waypoint.HasValue ? aimHLNG.HoldingPoint.Waypoint.Value : false;

                    if (aimHLNG.HoldingPoint.RadarGuidance.HasValue) { pdmHolding.HoldingPoint.RadarGuidance = aimHLNG.HoldingPoint.RadarGuidance.Value; }

                    if (aimHLNG.HoldingPoint.ReportingATC.HasValue)
                    {
                        PDM.CodeATCReporting _uomATCRRep;
                        Enum.TryParse<PDM.CodeATCReporting>(aimHLNG.HoldingPoint.ReportingATC.Value.ToString(), out _uomATCRRep);
                        pdmHolding.HoldingPoint.ReportingATC = _uomATCRRep;

                        if (pdmHolding.HoldingPoint.PointChoice == PointChoice.DesignatedPoint)
                        {
                            //UpdateWayPointReportingATCvalue(holdingPnt.ID, _uomATCRRep);
                        }

                    }


                    if (aimHLNG.HoldingPoint.FlyOver.HasValue) { pdmHolding.HoldingPoint.FlyOver = aimHLNG.HoldingPoint.FlyOver.Value; }


                    if (_obj != null)
                    {
                        pdmHolding.HoldingPoint.PointChoiceID = holdingPnt.ID;
                        pdmHolding.HoldingPoint.Lat = _obj.Lat;
                        pdmHolding.HoldingPoint.Lon = _obj.Lon;
                    }
                }
            }

            #endregion

            #region EndPoint

            if ((aimHLNG.OutboundLegSpan != null) &&
                (aimHLNG.OutboundLegSpan.Choice == Aran.Aim.HoldingPatternLengthChoice.SegmentPoint))
            {

                pdmHolding.EndPoint = new SegmentPoint();
                pdmHolding.EndPoint.PointFacilityMakeUp = new FacilityMakeUp();
                pdmHolding.EndPoint.PointFacilityMakeUp.AngleIndication = new AngleIndication();
                pdmHolding.EndPoint.PointFacilityMakeUp.AngleIndication.ID = aimHLNG.OutboundLegSpan.EndPoint.FacilityMakeup[0].FacilityAngle[0].TheAngleIndication.Identifier.ToString();


                pdmHolding.EndPoint.PointFacilityMakeUp.DistanceIndication = new DistanceIndication();
                pdmHolding.EndPoint.PointFacilityMakeUp.DistanceIndication.ID = aimHLNG.OutboundLegSpan.EndPoint.FacilityMakeup[0].FacilityDistance[0].Feature.Identifier.ToString();

                for (int i = 0; i < aimGeo.Count; i++)
                {
                    if (aimGeo[i].GeometryType == esriGeometryType.esriGeometryPoint)
                    {

                        pdmHolding.EndPoint.X = ((IPoint)aimGeo[i]).X;
                        pdmHolding.EndPoint.Y = ((IPoint)aimGeo[i]).Y;

                        pdmHolding.EndPoint.Lat = ((IPoint)aimGeo[i]).X.ToString();
                        pdmHolding.EndPoint.Lon = ((IPoint)aimGeo[i]).Y.ToString();


                        break;
                    }
                }

                FillPointFacilityMakeUpProperies(pdmHolding.EndPoint.PointFacilityMakeUp);


                #region MyRegion


                //PDMObject EndP = GetSegmentPoint(aimHLNG.OutboundLegSpan.EndPoint.PointChoice.Choice.ToString(), aimHLNG.OutboundLegSpan.EndPoint.PointChoice.AimingPoint.Identifier);
                //if (EndP != null)
                //{
                //    PDMObject _obj = DefinePointSegmentDesignator(((SegmentPoint)EndP).PointChoice, EndP.ID);

                //    pdmHolding.EndPoint = new PDM.SegmentPoint
                //    {
                //        ID = Guid.NewGuid().ToString(),
                //        Route_LEG_ID = pdmHolding.ID,
                //        PointRole = ProcedureFixRoleType.ENRT_HLDNG,
                //        PointUse = ProcedureSegmentPointUse.END_POINT,
                //        IsWaypoint = ((SegmentPoint)EndP).IsWaypoint,
                //        PointChoice = ((SegmentPoint)EndP).PointChoice,
                //        //SegmentPointDesignator = ((SegmentPoint)EndP).PointChoiceID,

                //    };

                //pdmHolding.EndPoint.IsWaypoint = aimHLNG.HoldingPoint.Waypoint.HasValue ? aimHLNG.HoldingPoint.Waypoint.Value : false;

                //if (aimHLNG.HoldingPoint.RadarGuidance.HasValue) { pdmHolding.EndPoint.RadarGuidance = aimHLNG.HoldingPoint.RadarGuidance.Value; }

                //if (aimHLNG.HoldingPoint.ReportingATC.HasValue)
                //{
                //    PDM.CodeATCReporting _uomATCRRep;
                //    Enum.TryParse<PDM.CodeATCReporting>(aimHLNG.HoldingPoint.ReportingATC.Value.ToString(), out _uomATCRRep);
                //    pdmHolding.EndPoint.ReportingATC = _uomATCRRep;

                //    if (pdmHolding.EndPoint.PointChoice == PointChoice.DesignatedPoint)
                //    {
                //        //UpdateWayPointReportingATCvalue(segStartPnt.ID, _uomATCRRep);
                //    }

                //}


                //if (aimHLNG.HoldingPoint.FlyOver.HasValue) { pdmHolding.HoldingPoint.FlyOver = aimHLNG.HoldingPoint.FlyOver.Value; }


                //if (_obj != null)
                //{
                //    pdmHolding.HoldingPoint.PointChoiceID = EndP.ID;
                //    pdmHolding.HoldingPoint.Lat = _obj.Lat;
                //    pdmHolding.HoldingPoint.Lon = _obj.Lon;
                //}
                //}

                #endregion
            }

            #endregion
        }    

        private void FillPointFacilityMakeUpProperies(FacilityMakeUp facilityMakeUp)
        {
            #region AngleIndication

            if (facilityMakeUp.AngleIndication != null) 
            {
                var _andleInd = (from element in CurEnvironment.Data.Intermediate_AIXM51_Arena_Features
                                 where (element != null) &&
                                     (element.AIXM51_Feature.FeatureType == Aran.Aim.FeatureType.AngleIndication) &&
                                      (((Aran.Aim.Features.AngleIndication)element.AIXM51_Feature).Identifier.ToString().CompareTo(facilityMakeUp.AngleIndication.ID) == 0)
                                 select element).FirstOrDefault();

                facilityMakeUp.AngleIndication.ID = Guid.NewGuid().ToString();

                if (_andleInd != null)
                {
                    #region fill AngleIndication properies

                    var aimAngleInd = (Aran.Aim.Features.AngleIndication)_andleInd.AIXM51_Feature;
                    if (aimAngleInd.Angle.HasValue) facilityMakeUp.AngleIndication.Angle = aimAngleInd.Angle.Value;

                    if (aimAngleInd.AngleType.HasValue)
                    {
                        PDM.BearingType _uomAnglType;
                        Enum.TryParse<PDM.BearingType>(aimAngleInd.AngleType.Value.ToString(), out _uomAnglType);
                        facilityMakeUp.AngleIndication.AngleType = _uomAnglType;
                    }

                    if (aimAngleInd.IndicationDirection.HasValue)
                    {
                        PDM.CodeDirectionReference _uomDir;
                        Enum.TryParse<PDM.CodeDirectionReference>(aimAngleInd.IndicationDirection.Value.ToString(), out _uomDir);
                        facilityMakeUp.AngleIndication.IndicationDirection = _uomDir;
                    }

                    if (aimAngleInd.TrueAngle.HasValue) facilityMakeUp.AngleIndication.TrueAngle = aimAngleInd.TrueAngle.Value;

                    if (aimAngleInd.CardinalDirection.HasValue)
                    {
                        PDM.CodeCardinalDirection _uomDir;
                        Enum.TryParse<PDM.CodeCardinalDirection>(aimAngleInd.CardinalDirection.Value.ToString(), out _uomDir);
                        facilityMakeUp.AngleIndication.CardinalDirection = _uomDir;
                    }

                    if (aimAngleInd.MinimumReceptionAltitude != null)
                    {
                        facilityMakeUp.AngleIndication.MinimumReceptionAltitude = aimAngleInd.MinimumReceptionAltitude.Value;

                        UOM_DIST_VERT _uomVert;
                        Enum.TryParse<UOM_DIST_VERT>(aimAngleInd.MinimumReceptionAltitude.Uom.ToString(), out _uomVert);
                        facilityMakeUp.AngleIndication.MinimumReceptionAltitudeUOM = _uomVert;
                    }

                    if ((aimAngleInd.Fix!=null)&&(aimAngleInd.Fix.Identifier != null)) 
                        facilityMakeUp.AngleIndication.FixID = aimAngleInd.Fix.Identifier.ToString();

                    if (aimAngleInd.PointChoice != null)
                    {
                        facilityMakeUp.AngleIndication.SignificantPointID = DefineSignifantPointID(aimAngleInd.PointChoice);
                        facilityMakeUp.AngleIndication.SignificantPointType = aimAngleInd.PointChoice.Choice.ToString();

                    }

                    

                    #endregion
                }
            }

            #endregion

            #region DistanceIndication

            if (facilityMakeUp.DistanceIndication != null)
            {

                var _distInd = (from element in CurEnvironment.Data.Intermediate_AIXM51_Arena_Features
                                where (element != null) &&
                                    (element.AIXM51_Feature.FeatureType == Aran.Aim.FeatureType.DistanceIndication) &&
                                     (((Aran.Aim.Features.DistanceIndication)element.AIXM51_Feature).Identifier.ToString().CompareTo(facilityMakeUp.DistanceIndication.ID) == 0)
                                select element).FirstOrDefault();

                facilityMakeUp.DistanceIndication.ID = Guid.NewGuid().ToString();

                if (_distInd != null)
                {
                    #region fill DistanceIndication properties

                    var aimDistInd = (Aran.Aim.Features.DistanceIndication)_distInd.AIXM51_Feature;
                    if (aimDistInd.Distance != null)
                    {
                        facilityMakeUp.DistanceIndication.Distance = aimDistInd.Distance.Value;

                        UOM_DIST_HORZ _uomHor;
                        Enum.TryParse<UOM_DIST_HORZ>(aimDistInd.Distance.Uom.ToString(), out _uomHor);
                        facilityMakeUp.DistanceIndication.DistanceUOM = _uomHor;

                    }

                    if (aimDistInd.MinimumReceptionAltitude != null)
                    {
                        facilityMakeUp.DistanceIndication.MinimumReceptionAltitude = aimDistInd.MinimumReceptionAltitude.Value;

                        UOM_DIST_VERT _uomVer;
                        Enum.TryParse<UOM_DIST_VERT>(aimDistInd.MinimumReceptionAltitude.Uom.ToString(), out _uomVer);
                        facilityMakeUp.DistanceIndication.MinimumReceptionAltitudeUOM = _uomVer;
                    }

                    if (aimDistInd.PointChoice != null)
                    {
                        facilityMakeUp.DistanceIndication.SignificantPointID = DefineSignifantPointID(aimDistInd.PointChoice);
                        facilityMakeUp.DistanceIndication.SignificantPointType = aimDistInd.PointChoice.Choice.ToString();

                    }

                    if (aimDistInd.Fix != null)
                    {
                        facilityMakeUp.DistanceIndication.FixID = aimDistInd.Fix.Identifier.ToString();
                    }

                    #endregion
                }


            }

            #endregion

        }

        private string DefineSignifantPointID(Aran.Aim.Features.SignificantPoint pointChoice)
        {
            string res = "";

            switch (pointChoice.Choice)
            {
                case Aran.Aim.SignificantPointChoice.DesignatedPoint:
                    res = pointChoice.FixDesignatedPoint.Identifier.ToString();
                    break;
                case Aran.Aim.SignificantPointChoice.Navaid:
                    res = pointChoice.NavaidSystem.Identifier.ToString();
                    break;
                case Aran.Aim.SignificantPointChoice.AirportHeliport:
                    res = pointChoice.AirportReferencePoint.Identifier.ToString();
                    break;
                default:
                    break;
            }

            return res;
        }

        private List<IGeometry> GetGeometry(List<IGeometry> GeomList)
        {
            // Filter by type
            return (from element in GeomList where (element != null) && (element.GeometryType == esriGeometryType.esriGeometryPolyline) select element).ToList();
            
        }


    }
}
