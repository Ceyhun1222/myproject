using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Linq;
using ESRI.ArcGIS.ADF.BaseClasses;
using ESRI.ArcGIS.ADF.CATIDs;
using ESRI.ArcGIS.Framework;
using ESRI.ArcGIS.ArcMapUI;
using System.IO;
using ZzArchivator;
using ArenaStatic;
using PDM;
using ARENA.Project;
using System.Xml.Serialization;
using System.Collections.Generic;
using AranSupport;
using ARENA.Enums_Const;
using ArenaLogManager;

namespace ARENA
{
    /// <summary>
    /// Summary description for ArenaSaveCmd.
    /// </summary>
    [Guid("da3b537a-ce02-44bf-be27-98194abc713a")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("Arena.ArenaSaveCmd")]
    public sealed class ArenaSaveCmd : BaseCommand
    {
        #region COM Registration Function(s)
        [ComRegisterFunction()]
        [ComVisible(false)]
        static void RegisterFunction(Type registerType)
        {
            // Required for ArcGIS Component Category Registrar support
            ArcGISCategoryRegistration(registerType);

            //
            // TODO: Add any COM registration code here
            //
        }

        [ComUnregisterFunction()]
        [ComVisible(false)]
        static void UnregisterFunction(Type registerType)
        {
            // Required for ArcGIS Component Category Registrar support
            ArcGISCategoryUnregistration(registerType);

            //
            // TODO: Add any COM unregistration code here
            //
        }

        #region ArcGIS Component Category Registrar generated code
        /// <summary>
        /// Required method for ArcGIS Component Category registration -
        /// Do not modify the contents of this method with the code editor.
        /// </summary>
        private static void ArcGISCategoryRegistration(Type registerType)
        {
            string regKey = string.Format("HKEY_CLASSES_ROOT\\CLSID\\{{{0}}}", registerType.GUID);
            MxCommands.Register(regKey);

        }
        /// <summary>
        /// Required method for ArcGIS Component Category unregistration -
        /// Do not modify the contents of this method with the code editor.
        /// </summary>
        private static void ArcGISCategoryUnregistration(Type registerType)
        {
            string regKey = string.Format("HKEY_CLASSES_ROOT\\CLSID\\{{{0}}}", registerType.GUID);
            MxCommands.Unregister(regKey);

        }

        #endregion
        #endregion

        private IApplication m_application;
       
        public ArenaSaveCmd()
        {
            //
            // TODO: Define values for the public properties
            //
            base.m_category = "Save arena project file"; //localizable text
            base.m_caption = "Save Project";  //localizable text
            base.m_message = "Save arena project file";  //localizable text 
            base.m_toolTip = "Save arena project file";  //localizable text 
            base.m_name = "ArenaSaveCmd";   //unique id, non-localizable (e.g. "MyCategory_ArcMapCommand")

            try
            {
                //
                // TODO: change bitmap name if necessary
                //
                base.m_bitmap = global::ArenaToolBox.Properties.Resources.Analyze16; 
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(ex.Message, "Invalid Bitmap");
            }
        }

        #region Overridden Class Methods

        /// <summary>
        /// Occurs when this command is created
        /// </summary>
        /// <param name="hook">Instance of the application</param>
        public override void OnCreate(object hook)
        {
            if (hook == null)
                return;

            m_application = hook as IApplication;

            //Disable if it is not ArcMap
            if (hook is IMxApplication)
                base.m_enabled = true;
            else
                base.m_enabled = false;

            // TODO:  Add other initialization code
        }

        

        /// <summary>
        /// Occurs when this command is clicked
        /// </summary>
        public override void OnClick()
        {

            string projname = "";
            bool NewprojectFile = false;
            if (DataCash.ProjectEnvironment == null  || DataCash.ProjectEnvironment.Data.CurProjectName == null)//(m_application.Caption.StartsWith("Untitled"))
            {

                var saveFileDialog1 = new SaveFileDialog
                {
                    Filter = @"Panda type files (*.pdm)|*.pdm",
                    RestoreDirectory = true
                };

                if (saveFileDialog1.ShowDialog() != DialogResult.OK) return;
                projname = saveFileDialog1.FileName;
                m_application.Caption = System.IO.Path.GetFileNameWithoutExtension(projname);
                NewprojectFile = true;
            }
            else
                projname = DataCash.ProjectEnvironment.Data.CurProjectName; //m_application.Caption;

            LogManager.GetLogger(GetType().Name).Info(string.Format(" ----------Save {0} project; Path: {1}", DataCash.ProjectEnvironment.Data.CurrentProjectType, projname) );
            switch (DataCash.ProjectEnvironment.Data.CurrentProjectType)
            {
                case ArenaProjectType.AERODROME:
                    SaveAerodromeProject(projname, NewprojectFile);
                    break;

                case ArenaProjectType.ARENA:
                    saveProject(projname,NewprojectFile);
                    break;
            }
            

            
        }

        #endregion


      
        private void saveProject(string ProjectName, bool NewProjectFlag)
        {
            var m_filePaths = ArenaStaticProc.GetRecentFiles_Pdm();

            var lastPath = m_filePaths!=null && m_filePaths.Length > 0?  m_filePaths[0] : "";

            AlertForm alrtForm = new AlertForm();
            try
            {
                var tempDirName = System.IO.Path.GetTempPath();
                string mxdName = System.IO.Path.GetDirectoryName(tempDirName) + @"\PDM\arena_PDM.mxd";
                string projName = System.IO.Path.GetDirectoryName(tempDirName) + @"\pdm.pdm";
                string mdbName = System.IO.Path.GetDirectoryName(tempDirName) + @"\PDM\pdm.mdb";

                DirectoryInfo dInf = System.IO.Directory.CreateDirectory(tempDirName + @"\PDM\" + "arenaData");
                tempDirName = dInf.FullName;


                string targetDB = ArenaStaticProc.GetTargetDB();
                bool existingproject = (targetDB.Length > 0) && (targetDB.CompareTo(@"\pdm.mdb") != 0);

                if (existingproject)
                {
                    tempDirName = System.IO.Path.GetDirectoryName(targetDB);
                    mxdName = System.IO.Path.GetDirectoryName(targetDB) + @"\arena_PDM.mxd";
                    projName = System.IO.Path.GetDirectoryName(targetDB) + @"\pdm.pdm";
                    mdbName = System.IO.Path.GetDirectoryName(targetDB) + @"\pdm.mdb";


                    dInf = System.IO.Directory.CreateDirectory(tempDirName + @"\arenaData");
                    tempDirName = dInf.FullName;

                }


                string[] FN = Directory.GetFiles(System.IO.Path.GetDirectoryName(projName), "*.pdm");
                //if (System.Diagnostics.Debugger.IsAttached) MessageBox.Show("File.Delete " + projName);
                //foreach (var fl in FN)
                //{
                //    System.IO.File.Delete(fl);
                //}

                #region PDM objects files
                
                alrtForm.FormBorderStyle = FormBorderStyle.None;
                alrtForm.Opacity = 0.5;
                alrtForm.BackgroundImage = ArenaToolBox.Properties.Resources.ArenaSplash;

                if (!System.Diagnostics.Debugger.IsAttached) alrtForm.Show();

                alrtForm.progressBar1.Visible = true;
                alrtForm.progressBar1.ForeColor = System.Drawing.Color.FromArgb(255, 22, 76, 108);
                alrtForm.label1.BackColor = System.Drawing.Color.FromArgb(255, 23, 76, 107);
                alrtForm.label1.Text = "Data saving";
                alrtForm.label1.Visible = true;

                if (DataCash.ProjectEnvironment.Data.PdmObjectList.Count > 0)
                {

                    alrtForm.progressBar1.Maximum = DataCash.ProjectEnvironment.Data.PdmObjectList.Count;
                    alrtForm.progressBar1.Value = 0;


                    int MaxSize = 2000000;

                    int Steps = 0;
                    int part = 0;
                    int index = 0;
                    int curSize = 0;
                    //List<PDMObject> ll = (from element in DataCash.ProjectEnvironment.Data.PdmObjectList where (element != null) && (element.PDM_Type == PDM_ENUM.VerticalStructure) select element).ToList();
                    foreach (var obj in DataCash.ProjectEnvironment.Data.PdmObjectList)
                    //foreach (var obj in ll)
                    {
                        alrtForm.progressBar1.Value++;
                        Application.DoEvents();


                        curSize = curSize + ArenaStaticProc.getObjectSize(obj);
                        if (curSize < MaxSize)
                        {
                            Steps++;
                        }
                        else
                        {
                            if (Steps <= 0)
                                Steps = 1;
                            PDMObject[] newList = new PDMObject[Steps];
                            DataCash.ProjectEnvironment.Data.PdmObjectList.CopyTo(index, newList, 0, Steps);
                            //ll.CopyTo(index, newList, 0, Steps);
                            PDM_ObjectsList ResPart = new PDM_ObjectsList(newList.ToList(), DataCash.ProjectEnvironment.Data.CurrentProjectType.ToString());
                            string newFN = projName.Replace("pdm.pdm", (part + 1).ToString() + "pdm.pdm");
                            bool res = ArenaStatic.ArenaStaticProc.Serialize(ResPart, newFN);
                            index = index + Steps;
                            Steps = 1;
                            //curSize = 0;
                            part++;
                        }
                    }
                    if (curSize > 0)
                    {
                        if (Steps <= 0) Steps = 1;
                        PDMObject[] newList = new PDMObject[Steps];
                        DataCash.ProjectEnvironment.Data.PdmObjectList.CopyTo(index, newList, 0, Steps);
                        //ll.CopyTo(index, newList, 0, Steps);
                        PDM_ObjectsList ResPart = new PDM_ObjectsList(newList.ToList(), DataCash.ProjectEnvironment.Data.CurrentProjectType.ToString());
                        string newFN = projName.Replace("pdm.pdm", (part + 1).ToString() + "pdm.pdm");
                        bool res = ArenaStatic.ArenaStaticProc.Serialize(ResPart, newFN);
                    }


                }

                #endregion


                #region MXD File


                alrtForm.label1.Text = "Finalazing";

                IMxDocument pNewDocument = (IMxDocument)m_application.Document;
                pNewDocument.RelativePaths = true;
                m_application.SaveDocument(mxdName);
                Application.DoEvents();
                System.IO.File.Copy(mxdName, System.IO.Path.Combine(tempDirName, "arena_PDM.mxd"), true);
                Application.DoEvents();

                #endregion


                try
                {
                    #region Settings

                    ArenaStatic.ArenaStaticProc.Serialize(
                        ((ArenaProject)DataCash.ProjectEnvironment.Data.CurrentProject).ProjectSettings,
                        tempDirName + @"\ProjectSettings");
                    //System.IO.File.Copy(tempDirName + @"\ProjectSettings", System.IO.Path.Combine(tempDirName, System.IO.Path.GetFileName(tempDirName + @"\ProjectSettings")), true);

                    #endregion

                    #region Project log
                    if (DataCash.ProjectEnvironment.Data.ProjectLog != null)
                    {
                        string filePath = tempDirName + @"\ProjectLog.txt";
                        if (!File.Exists(filePath))  System.IO.File.WriteAllLines(filePath, DataCash.ProjectEnvironment.Data.ProjectLog.ToArray());
                    }
                    #endregion
                }
                catch (Exception ex)
                {
                    LogManager.GetLogger(ex.TargetSite.Name)
                        .Error(ex, "Settings error when saving Arena project");
                }

                FN = Directory.GetFiles(System.IO.Path.GetDirectoryName(projName), "*.pdm");
                foreach (var fl in FN)
                {
                    System.IO.File.Copy(fl, System.IO.Path.Combine(tempDirName, System.IO.Path.GetFileName(fl)), true);
                }


                System.IO.File.Copy(mdbName, System.IO.Path.Combine(tempDirName, System.IO.Path.GetFileName(mdbName)), true);

                string sCompressedFile = NewProjectFlag? ProjectName : lastPath; /////

                if (existingproject)
                {
                    //sCompressedFile = sCompressedFile+".pdm";
                    if (System.Diagnostics.Debugger.IsAttached) MessageBox.Show("File.Delete " + sCompressedFile);

                    if (File.Exists(sCompressedFile))
                    {

                        System.IO.File.Delete(sCompressedFile);
                        Application.DoEvents();
                    }
                }

                ArenaStaticProc.CompressDirectory(tempDirName, sCompressedFile);


                //if (System.Diagnostics.Debugger.IsAttached) MessageBox.Show("File.Delete " + tempDirName);
                //ArenaStaticProc.DeleteFilesFromDirectory(tempDirName + @"\");
                //Directory.Delete(tempDirName);

                DataCash.ProjectEnvironment.Data.CurProjectName = ProjectName;

                ArenaStatic.ArenaStaticProc.SaveRecentFileName(NewProjectFlag ? ProjectName : lastPath); /////////


                alrtForm.Close();
                
                ArenaMessageForm msgFrm = new ArenaMessageForm("ARENA", "Project saved", global::ArenaToolBox.Properties.Resources.ArenaMessage);

                msgFrm.ShowDialog();
            }
            catch (Exception ex)
            {
                LogManager.GetLogger(ex.TargetSite.Name)
                    .Error(ex, "Error when saving project; Project Name: " + ProjectName);

                alrtForm.Close();
                ArenaMessageForm msgFrm = new ArenaMessageForm("ARENA", "Error when saving project", global::ArenaToolBox.Properties.Resources.ArenaMessage);
                msgFrm.ShowDialog();
            }
            
        }

        private void SaveAerodromeProject(string ProjectName, bool NewProjectFlag)
        {
            var m_filePaths = ArenaStaticProc.GetRecentFiles_Pdm();

            var lastPath = m_filePaths != null && m_filePaths.Length > 0 ? m_filePaths[0] : "";

            AlertForm alrtForm = new AlertForm();
            try
            {
                var tempDirName = System.IO.Path.GetTempPath();
                string mxdName = System.IO.Path.GetDirectoryName(tempDirName) + @"\PDM\aerodrome.mxd";
                string projName = System.IO.Path.GetDirectoryName(tempDirName) + @"\aerodrome.pdm";
                string mdbName = System.IO.Path.GetDirectoryName(tempDirName) + @"\PDM\aerodrome.mdb";

                DirectoryInfo dInf = System.IO.Directory.CreateDirectory(tempDirName + @"\PDM\" + "arenaData");
                tempDirName = dInf.FullName;


                string targetDB = ArenaStaticProc.GetTargetDB();
                bool existingproject = (targetDB.Length > 0) && (targetDB.CompareTo(@"\aerodrome.mdb") != 0);

                if (existingproject)
                {
                    tempDirName = System.IO.Path.GetDirectoryName(targetDB);
                    mxdName = System.IO.Path.GetDirectoryName(targetDB) + @"\aerodrome.mxd";
                    projName = System.IO.Path.GetDirectoryName(targetDB) + @"\aerodrome.pdm";
                    mdbName = System.IO.Path.GetDirectoryName(targetDB) + @"\aerodrome.mdb";


                    dInf = System.IO.Directory.CreateDirectory(tempDirName + @"\arenaData");
                    tempDirName = dInf.FullName;

                }


                //PDM_ObjectsList Tmp_PdmObjectList = new PDM_ObjectsList(DataCash.ProjectEnvironment.Data.PdmObjectList, DataCash.ProjectEnvironment.Data.CurrentProjectType.ToString());



                string[] FN = Directory.GetFiles(System.IO.Path.GetDirectoryName(projName), "*.pdm");
                if (System.Diagnostics.Debugger.IsAttached) MessageBox.Show("projName");
                foreach (var fl in FN)
                {
                    System.IO.File.Delete(fl);
                }

                #region PDM objects files

                alrtForm.FormBorderStyle = FormBorderStyle.None;
                alrtForm.Opacity = 0.5;
                alrtForm.BackgroundImage = ArenaToolBox.Properties.Resources.ArenaSplash;

                if (!System.Diagnostics.Debugger.IsAttached) alrtForm.Show();

                alrtForm.progressBar1.Visible = true;
                alrtForm.progressBar1.ForeColor = System.Drawing.Color.FromArgb(255, 22, 76, 108);
                alrtForm.label1.BackColor = System.Drawing.Color.FromArgb(255, 23, 76, 107);
                alrtForm.label1.Text = "Data saving";
                alrtForm.label1.Visible = true;

                if (DataCash.ProjectEnvironment.Data.PdmObjectList.Count > 0)
                {

                    alrtForm.progressBar1.Maximum = DataCash.ProjectEnvironment.Data.PdmObjectList.Count;
                    alrtForm.progressBar1.Value = 0;


                    int MaxSize = 2000000;

                    int Steps = 0;
                    int part = 0;
                    int index = 0;
                    int curSize = 0;
                    //List<PDMObject> ll = (from element in DataCash.ProjectEnvironment.Data.PdmObjectList where (element != null) && (element.PDM_Type == PDM_ENUM.VerticalStructure) select element).ToList();
                    foreach (var obj in DataCash.ProjectEnvironment.Data.PdmObjectList)
                    //foreach (var obj in ll)
                    {
                        alrtForm.progressBar1.Value++;
                        Application.DoEvents();


                        curSize = curSize + ArenaStaticProc.getObjectSize(obj);
                        if (curSize < MaxSize)
                        {
                            Steps++;
                        }
                        else
                        {
                            if (Steps <= 0)
                                Steps = 1;
                            PDMObject[] newList = new PDMObject[Steps];
                            DataCash.ProjectEnvironment.Data.PdmObjectList.CopyTo(index, newList, 0, Steps);
                            //ll.CopyTo(index, newList, 0, Steps);
                            PDM_ObjectsList ResPart = new PDM_ObjectsList(newList.ToList(), DataCash.ProjectEnvironment.Data.CurrentProjectType.ToString());
                            string newFN = projName.Replace("aerodrome.pdm", (part + 1).ToString() + "aerodrome.pdm");
                            bool res = ArenaStatic.ArenaStaticProc.Serialize(ResPart, newFN);
                            index = index + Steps;
                            Steps = 1;
                            curSize = 0;
                            part++;
                        }
                    }
                    if (curSize > 0)
                    {
                        if (Steps <= 0) Steps = 1;
                        PDMObject[] newList = new PDMObject[Steps];
                        DataCash.ProjectEnvironment.Data.PdmObjectList.CopyTo(index, newList, 0, Steps);
                        //ll.CopyTo(index, newList, 0, Steps);
                        PDM_ObjectsList ResPart = new PDM_ObjectsList(newList.ToList(), DataCash.ProjectEnvironment.Data.CurrentProjectType.ToString());
                        string newFN = projName.Replace("aerodrome.pdm", (part + 1).ToString() + "aerodrome.pdm");
                        bool res = ArenaStatic.ArenaStaticProc.Serialize(ResPart, newFN);
                    }


                }

                #endregion


                #region MXD File


                alrtForm.label1.Text = "Finalazing";

                IMxDocument pNewDocument = (IMxDocument)m_application.Document;
                pNewDocument.RelativePaths = true;
                m_application.SaveDocument(mxdName);
                Application.DoEvents();
                System.IO.File.Copy(mxdName, System.IO.Path.Combine(tempDirName, "aerodrome.mxd"), true);
                Application.DoEvents();

                #endregion


                try
                {
                    #region Settings
                    ArenaStatic.ArenaStaticProc.Serialize(((ArenaProject)DataCash.ProjectEnvironment.Data.CurrentProject).ProjectSettings, tempDirName + @"\ProjectSettings");
                    //System.IO.File.Copy(tempDirName + @"\ProjectSettings", System.IO.Path.Combine(tempDirName, System.IO.Path.GetFileName(tempDirName + @"\ProjectSettings")), true);
                    #endregion
                }
                catch (Exception ex)
                {
                    LogManager.GetLogger(ex.TargetSite.Name)
                        .Error(ex, "Settings error when saving Aerodrome project");
                }

                FN = Directory.GetFiles(System.IO.Path.GetDirectoryName(projName), "*.pdm");
                foreach (var fl in FN)
                {
                    System.IO.File.Copy(fl, System.IO.Path.Combine(tempDirName, System.IO.Path.GetFileName(fl)), true);
                }


                System.IO.File.Copy(mdbName, System.IO.Path.Combine(tempDirName, System.IO.Path.GetFileName(mdbName)), true);

                string sCompressedFile = NewProjectFlag ? ProjectName : lastPath; /////

                if (existingproject)
                {
                    //sCompressedFile = sCompressedFile+".pdm";
                    if (File.Exists(sCompressedFile))
                    {
                        System.IO.File.Delete(sCompressedFile);
                        Application.DoEvents();
                    }
                }

                ArenaStaticProc.CompressDirectory(tempDirName, sCompressedFile);

                ArenaStaticProc.DeleteFilesFromDirectory(tempDirName + @"\");
                Directory.Delete(tempDirName);

                DataCash.ProjectEnvironment.Data.CurProjectName = ProjectName;

                ArenaStatic.ArenaStaticProc.SaveRecentFileName(NewProjectFlag ? ProjectName : lastPath); /////////

                alrtForm.Close();

                ArenaMessageForm msgFrm = new ArenaMessageForm("Aerodrome", "Projects saved", global::ArenaToolBox.Properties.Resources.ArenaMessage);

                msgFrm.ShowDialog();
            }
            catch (Exception ex)
            {
                LogManager.GetLogger(ex.TargetSite.Name)
                    .Error(ex, "Error when saving Aerodrome project; Project Name: " + ProjectName);

                alrtForm.Close();
                ArenaMessageForm msgFrm = new ArenaMessageForm("Aerodrome", "Error when saving project", global::ArenaToolBox.Properties.Resources.ArenaMessage);
                msgFrm.ShowDialog();
            }
            
        }


    }
}
