using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Framework;
using ESRI.ArcGIS.ADF.CATIDs;
using ESRI.ArcGIS.Geodatabase;
using EsriWorkEnvironment;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.ArcMapUI;
using ArenaStatic;
using System.IO;
using System.Windows.Forms;
using ESRI.ArcGIS.Controls;
using ARENA;
using ARENA.Enums_Const;
using DataModule;
using PDM;
using System.Linq;
using Encryptor;

namespace SigmaChart
{
    [Guid("15d7e945-8209-4e1d-a8f1-8aa81d09ed57")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("SigmaChart.SigmaExtension")]
    public class SigmaExtension : IExtension, IPersistVariant
    {
        private string arenaProjectIdentifier;


        private ESRI.ArcGIS.Carto.IActiveViewEvents_AfterDrawEventHandler m_ActiveViewEventsAfterDraw;
        private ESRI.ArcGIS.Carto.IActiveViewEvents_AfterItemDrawEventHandler m_ActiveViewEventsAfterItemDraw;
        private ESRI.ArcGIS.Carto.IActiveViewEvents_ContentsChangedEventHandler m_ActiveViewEventsContentsChanged;
        private ESRI.ArcGIS.Carto.IActiveViewEvents_ContentsClearedEventHandler m_ActiveViewEventsContentsCleared;
        private ESRI.ArcGIS.Carto.IActiveViewEvents_FocusMapChangedEventHandler m_ActiveViewEventsFocusMapChanged;
        private ESRI.ArcGIS.Carto.IActiveViewEvents_ItemAddedEventHandler m_ActiveViewEventsItemAdded;
        private ESRI.ArcGIS.Carto.IActiveViewEvents_ItemDeletedEventHandler m_ActiveViewEventsItemDeleted;
        private ESRI.ArcGIS.Carto.IActiveViewEvents_ItemReorderedEventHandler m_ActiveViewEventsItemReordered;
        private ESRI.ArcGIS.Carto.IActiveViewEvents_SelectionChangedEventHandler m_ActiveViewEventsSelectionChanged;
        private ESRI.ArcGIS.Carto.IActiveViewEvents_SpatialReferenceChangedEventHandler m_ActiveViewEventsSpatialReferenceChanged;
        private ESRI.ArcGIS.Carto.IActiveViewEvents_ViewRefreshedEventHandler m_ActiveViewEventsViewRefreshed;
        IDocumentEvents_Event m_docEvents;

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
            MxExtension.Register(regKey);

        }
        /// <summary>
        /// Required method for ArcGIS Component Category unregistration -
        /// Do not modify the contents of this method with the code editor.
        /// </summary>
        private static void ArcGISCategoryUnregistration(Type registerType)
        {
            string regKey = string.Format("HKEY_CLASSES_ROOT\\CLSID\\{{{0}}}", registerType.GUID);
            MxExtension.Unregister(regKey);

        }

        #endregion
        #endregion
        private IApplication m_application;

        #region IExtension Members

        /// <summary>
        /// Name of extension. Do not exceed 31 characters
        /// </summary>
        public string Name
        {
            get
            {
                //TODO: Modify string to uniquely identify extension
                return "SigmaExtension";
            }
        }

        public void Shutdown()
        {
            //TODO: Clean up resources
            //RemoveActiveViewEvents(((IMxDocument)m_application.Document).FocusMap);
            m_application = null;
        }

        public void Startup(ref object initializationData)
        {
            m_application = initializationData as IApplication;
            if (m_application == null)
                return;

          
            
        }

        #endregion

        public UID ID
        {
            get
            {
                UID typeID = new UIDClass();
                typeID.Value = GetType().GUID.ToString("B");
                return typeID;
            }
        }

        public void Load(IVariantStream Stream)
        {
            if (DateTime.Now > SigmaEncryptor.GetEncryptedDate()) return;

            arenaProjectIdentifier = Convert.ToString(Stream.Read());
            try
            {
                //if (arenaProjectIdentifier != null && arenaProjectIdentifier.Length > 0)
                {
                    SigmaDataCash.ChartElementList = new List<object>();

                    ArenaStaticProc.BringToFrontToc((IMxDocument)m_application.Document, "ANCORTOCLayerView");


                   

                    //TODO: Add code to initialize the extension
                    SetupActiveViewEvents(((IMxDocument)m_application.Document).FocusMap);
                }
                //else return;

                Application.DoEvents();




                switch (SigmaDataCash.UpdateState)
                {
                    case (1):

                        if (SigmaDataCash.ChartElementList != null)
                        {

                            

                            SigmaDataCash.environmentWorkspaceEdit.StartEditing(false);
                            SigmaDataCash.environmentWorkspaceEdit.StartEditOperation();

                            IHookHelper m_hookHelper = new HookHelperClass();
                            m_hookHelper.Hook = m_application;

                            EnrouteChartClass _Chart = new EnrouteChartClass { SigmaHookHelper = m_hookHelper, SigmaApplication = m_application };
                            _Chart.UpdateChart(SigmaDataCash.ChangeList.FindAll(t => t.IsChecked), SigmaDataCash.oldPdmList, SigmaDataCash.newPdmList);


                            SigmaDataCash.environmentWorkspaceEdit.StopEditOperation();
                            SigmaDataCash.environmentWorkspaceEdit.StopEditing(true);


                            string destPath2 = ((ESRI.ArcGIS.Carto.IDocumentInfo2)(m_application.Document)).Folder;
                            ChartsHelperClass.SaveSourcePDMFiles(destPath2, SigmaDataCash.newPdmList);

                            for (int i = 0; i < ((IMxDocument)m_application.Document).ContentsViewCount; i++)
                            {
                                IContentsView cnts = ((IMxDocument)m_application.Document).get_ContentsView(i);

                                string cntxtName = ((IMxDocument)m_application.Document).ContentsView[i].Name;

                                if (cntxtName.StartsWith("ANCORTOCLayerView")) ((IMxDocument)m_application.Document).ContentsView[i].Refresh(cntxtName);

                            }

                            Application.DoEvents();


                            SigmaDataCash.UpdateState = 0;

                            ArenaMessageForm msgFrm = new ArenaMessageForm("SIGMA", "The Chart is updated successfully", global::SigmaChart.Properties.Resources.SigmaMessage);
                            msgFrm.TopMost = true;
                            msgFrm.checkBox1.Visible = false;
                            msgFrm.ShowDialog();

                        }
                        break;
                    default:
                        break;
                }


               

            }

            catch { }
            finally { if (SigmaDataCash.UpdateState == 1) { SigmaDataCash.UpdateState = 0; SigmaDataCash.ChangeList = null; } }

        }

        public void Save(IVariantStream Stream)
        {
            if (DateTime.Now > SigmaEncryptor.GetEncryptedDate()) return;

            //arenaProjectIdentifier = "1";
            Stream.Write(arenaProjectIdentifier);
            //string fn = (m_application.Document as IDocumentInfo2).Path;
            //try
            //{
            //    ((IMapDocument)m_application.Document).SaveAs(fn);

            //}
            //catch (Exception ex)
            //{
            //    System.Diagnostics.Debug.WriteLine("");
            //}


            switch (SigmaDataCash.UpdateState)
            {
                case (11):

                    if (SigmaDataCash.ChartElementList != null)
                    {

                        string destFolder = ((ESRI.ArcGIS.Carto.IDocumentInfo2)(m_application.Document)).Folder;



                        foreach (var FC in SigmaDataCash.AnnotationLinkedGeometryList.Keys)
                        {
                            string _oleCmd = "delete FROM " + FC +" where FeatureGUID IS NULL OR FeatureGUID = ''";
                            ArenaDataModule.ExecuteSqlCommand(System.IO.Path.Combine(destFolder, "pdm.mdb"), _oleCmd);

                        }

                        //EsriWorkEnvironment.EsriUtils.CompactDataBase(destFolder + @"\pdm.mdb");


                        SigmaDataCash.environmentWorkspaceEdit.StartEditing(false);
                        SigmaDataCash.environmentWorkspaceEdit.StartEditOperation();

                        IHookHelper m_hookHelper = new HookHelperClass();
                        m_hookHelper.Hook = m_application;

                        EnrouteChartClass _Chart = new EnrouteChartClass { SigmaHookHelper = m_hookHelper, SigmaApplication = m_application };

                        _Chart.UpdateChart(SigmaDataCash.ChangeList.FindAll(t => t.IsChecked), SigmaDataCash.oldPdmList, SigmaDataCash.newPdmList);


                        SigmaDataCash.environmentWorkspaceEdit.StopEditOperation();
                        SigmaDataCash.environmentWorkspaceEdit.StopEditing(true);


                        string destPath2 = ((ESRI.ArcGIS.Carto.IDocumentInfo2)(m_application.Document)).Folder;

                        #region Reload PdmObjectList

                        ArenaProjectType prjType = ArenaProjectType.ARENA;
                        DataCash.ProjectEnvironment = new ARENA.Environment.Environment { mxApplication = m_application, pMap = ((IMxDocument)m_application.Document).ActiveView.FocusMap };

                        if (DataCash.ProjectEnvironment.Data.PdmObjectList != null) DataCash.ProjectEnvironment.Data.PdmObjectList.Clear();
                        DataCash.ProjectEnvironment.Data.PdmObjectList.AddRange(ArenaDataModule.GetObjectsFromPdmFile(destPath2, ref prjType));

                        // refresh PdmObjectList

                        var changeList = SigmaDataCash.ChangeList.FindAll(t => t.IsChecked);

                        foreach (var item in changeList)
                        {
                            switch (item.ChangedStatus)
                            {
                                case ChartCompare.Status.New:
                                    DataCash.ProjectEnvironment.Data.PdmObjectList.Add(item.Feature);
                                    break;

                                case ChartCompare.Status.Changed:
                                    DataCash.ProjectEnvironment.Data.PdmObjectList.RemoveAll(ft => ft.ID.CompareTo(item.Feature.ID) ==0);
                                    DataCash.ProjectEnvironment.Data.PdmObjectList.Add(item.Feature);
                                    break;

                                case ChartCompare.Status.Deleted:
                                    DataCash.ProjectEnvironment.Data.PdmObjectList.Remove(item.Feature);
                                    break;

                                case ChartCompare.Status.Missing:
                                    break;

                                default:
                                    break;
                            }

                        }

                        #endregion


                        for (int i = 0; i < ((IMxDocument)m_application.Document).ContentsViewCount; i++)
                        {
                            IContentsView cnts = ((IMxDocument)m_application.Document).get_ContentsView(i);

                            string cntxtName = ((IMxDocument)m_application.Document).ContentsView[i].Name;

                            if (cntxtName.StartsWith("ANCORTOCLayerView")) ((IMxDocument)m_application.Document).ContentsView[i].Refresh(cntxtName);
                            if (cntxtName.StartsWith("TOCLayerFilter")) ((IMxDocument)m_application.Document).ContentsView[i].Refresh(cntxtName);

                        }

                        #region Save updated PdmObjectList to disc

                        string projName = destPath2 + @"\pdm.obj";

                        string[] FN = Directory.GetFiles(System.IO.Path.GetDirectoryName(projName), "*.obj");
                        foreach (var fl in FN)
                        {
                            System.IO.File.Delete(fl);
                        }

                        if (DataCash.ProjectEnvironment.Data.PdmObjectList.Count > 0)
                        {

                            int MaxSize = 2000000;
                            int Steps = 0;
                            int part = 0;
                            int index = 0;
                            int curSize = 0;
                            foreach (var obj in DataCash.ProjectEnvironment.Data.PdmObjectList)
                            {

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
                                    string newFN = projName.Replace("pdm.obj", (part + 1).ToString() + "pdm.obj");
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
                                PDM_ObjectsList ResPart = new PDM_ObjectsList(newList.ToList(), DataCash.ProjectEnvironment.Data.CurrentProjectType.ToString());
                                string newFN = projName.Replace("pdm.obj", (part + 1).ToString() + "pdm.obj");
                                bool res = ArenaStatic.ArenaStaticProc.Serialize(ResPart, newFN);
                            }


                        }



                        #endregion


                        Application.DoEvents();


                        SigmaDataCash.UpdateState = 0;

                        ArenaMessageForm msgFrm = new ArenaMessageForm("SIGMA", "The Chart is updated successfully", global::SigmaChart.Properties.Resources.SigmaMessage);
                        msgFrm.TopMost = true;
                        msgFrm.checkBox1.Visible = false;
                        msgFrm.ShowDialog();



                    }
                    break;
                default:
                    break;
            }

        }

        #region SetupActiveViewEvents


        private void SetupActiveViewEvents(ESRI.ArcGIS.Carto.IMap map)
        {
            //parameter check
            if (map == null)
            {
                return;

            }
            ESRI.ArcGIS.Carto.IActiveViewEvents_Event activeViewEvents = map as ESRI.ArcGIS.Carto.IActiveViewEvents_Event;
            // Create an instance of the delegate, add it to AfterDraw event
            m_ActiveViewEventsAfterDraw = new ESRI.ArcGIS.Carto.IActiveViewEvents_AfterDrawEventHandler(OnActiveViewEventsAfterDraw);
            activeViewEvents.AfterDraw += m_ActiveViewEventsAfterDraw;

            // Create an instance of the delegate, add it to AfterItemDraw event
            m_ActiveViewEventsAfterItemDraw = new ESRI.ArcGIS.Carto.IActiveViewEvents_AfterItemDrawEventHandler(OnActiveViewEventsItemDraw);
            activeViewEvents.AfterItemDraw += m_ActiveViewEventsAfterItemDraw;

            // Create an instance of the delegate, add it to ContentsChanged event
            m_ActiveViewEventsContentsChanged = new ESRI.ArcGIS.Carto.IActiveViewEvents_ContentsChangedEventHandler(OnActiveViewEventsContentsChanged);
            activeViewEvents.ContentsChanged += m_ActiveViewEventsContentsChanged;

            // Create an instance of the delegate, add it to ContentsCleared event
            m_ActiveViewEventsContentsCleared = new ESRI.ArcGIS.Carto.IActiveViewEvents_ContentsClearedEventHandler(OnActiveViewEventsContentsCleared);
            activeViewEvents.ContentsCleared += m_ActiveViewEventsContentsCleared;

            // Create an instance of the delegate, add it to FocusMapChanged event
            m_ActiveViewEventsFocusMapChanged = new ESRI.ArcGIS.Carto.IActiveViewEvents_FocusMapChangedEventHandler(OnActiveViewEventsFocusMapChanged);
            activeViewEvents.FocusMapChanged += m_ActiveViewEventsFocusMapChanged;

            // Create an instance of the delegate, add it to ItemAdded event
            m_ActiveViewEventsItemAdded = new ESRI.ArcGIS.Carto.IActiveViewEvents_ItemAddedEventHandler(OnActiveViewEventsItemAdded);
            activeViewEvents.ItemAdded += m_ActiveViewEventsItemAdded;

            // Create an instance of the delegate, add it to ItemDeleted event
            m_ActiveViewEventsItemDeleted = new ESRI.ArcGIS.Carto.IActiveViewEvents_ItemDeletedEventHandler(OnActiveViewEventsItemDeleted);
            activeViewEvents.ItemDeleted += m_ActiveViewEventsItemDeleted;

            // Create an instance of the delegate, add it to ItemReordered event
            m_ActiveViewEventsItemReordered = new ESRI.ArcGIS.Carto.IActiveViewEvents_ItemReorderedEventHandler(OnActiveViewEventsItemReordered);
            activeViewEvents.ItemReordered += m_ActiveViewEventsItemReordered;

            // Create an instance of the delegate, add it to SelectionChanged event
            m_ActiveViewEventsSelectionChanged = new ESRI.ArcGIS.Carto.IActiveViewEvents_SelectionChangedEventHandler(OnActiveViewEventsSelectionChanged);
            activeViewEvents.SelectionChanged += m_ActiveViewEventsSelectionChanged;

            // Create an instance of the delegate, add it to SpatialReferenceChanged event
            m_ActiveViewEventsSpatialReferenceChanged = new ESRI.ArcGIS.Carto.IActiveViewEvents_SpatialReferenceChangedEventHandler(OnActiveViewEventsSpatialReferenceChanged);
            activeViewEvents.SpatialReferenceChanged += m_ActiveViewEventsSpatialReferenceChanged;

            // Create an instance of the delegate, add it to ViewRefreshed event
            m_ActiveViewEventsViewRefreshed = new ESRI.ArcGIS.Carto.IActiveViewEvents_ViewRefreshedEventHandler(OnActiveViewEventsViewRefreshed);
            activeViewEvents.ViewRefreshed += m_ActiveViewEventsViewRefreshed;

            m_docEvents = m_application.Document as IDocumentEvents_Event;
            m_docEvents.CloseDocument += new IDocumentEvents_CloseDocumentEventHandler(OnCloseDocument);
            m_docEvents.OpenDocument += new IDocumentEvents_OpenDocumentEventHandler(OnOpenDocument);
        }

        /// <summary>
        /// SECTION: Remove the event handlers for all of the IActiveViewEvents
        /// </summary>
        /// <param name="map">An IMap interface for which Event Handlers to remove.</param>
        /// <remarks>You do not have to remove Event Handlers for every event. You can pick and 
        /// choose which ones you want to use.</remarks>
        private void RemoveActiveViewEvents(ESRI.ArcGIS.Carto.IMap map)
        {

            //parameter check
            if (map == null)
            {
                return;

            }
            ESRI.ArcGIS.Carto.IActiveViewEvents_Event activeViewEvents = map as ESRI.ArcGIS.Carto.IActiveViewEvents_Event;

            // Remove AfterDraw Event Handler
            activeViewEvents.AfterDraw -= m_ActiveViewEventsAfterDraw;

            // Remove AfterItemDraw Event Handler
            activeViewEvents.AfterItemDraw -= m_ActiveViewEventsAfterItemDraw;

            // Remove ContentsChanged Event Handler
            activeViewEvents.ContentsChanged -= m_ActiveViewEventsContentsChanged;

            // Remove ContentsCleared Event Handler
            activeViewEvents.ContentsCleared -= m_ActiveViewEventsContentsCleared;

            // Remove FocusMapChanged Event Handler
            activeViewEvents.FocusMapChanged -= m_ActiveViewEventsFocusMapChanged;

            // Remove ItemAdded Event Handler
            activeViewEvents.ItemAdded -= m_ActiveViewEventsItemAdded;

            // Remove ItemDeleted Event Handler
            activeViewEvents.ItemDeleted -= m_ActiveViewEventsItemDeleted;

            // Remove ItemReordered Event Handler
            activeViewEvents.ItemReordered -= m_ActiveViewEventsItemReordered;

            // Remove SelectionChanged Event Handler
            activeViewEvents.SelectionChanged -= m_ActiveViewEventsSelectionChanged;

            // Remove SpatialReferenceChanged Event Handler
            activeViewEvents.SpatialReferenceChanged -= m_ActiveViewEventsSpatialReferenceChanged;

            // Remove ViewRefreshed Event Handler
            activeViewEvents.ViewRefreshed -= m_ActiveViewEventsViewRefreshed;

            m_docEvents.OpenDocument -= OnOpenDocument;
            m_docEvents.CloseDocument -= OnCloseDocument;
        }


        /// <summary>
        /// AfterDraw Event handler
        /// </summary>
        /// <param name="Display"></param>
        /// <param name="phase"></param>
        /// <remarks>SECTION: Custom Functions that you write to add additionally functionality for the events</remarks>
        private void OnActiveViewEventsAfterDraw(ESRI.ArcGIS.Display.IDisplay display, ESRI.ArcGIS.Carto.esriViewDrawPhase phase)
        {
            // TODO: Add your code here
            ////System.Windows.Forms.MessageBox.Show("AfterDraw"); 
        }

        /// <summary>
        /// ItemDraw Event handler
        /// </summary>
        /// <param name="Index"></param>
        /// <param name="Display"></param>
        /// <param name="phase"></param>
        /// <remarks></remarks>
        private void OnActiveViewEventsItemDraw(short Index, ESRI.ArcGIS.Display.IDisplay display, ESRI.ArcGIS.esriSystem.esriDrawPhase phase)
        {
            // TODO: Add your code here
            //System.Windows.Forms.MessageBox.Show("ItemDraw"); 
        }

        /// <summary>
        /// ContentsChanged Event handler
        /// </summary>
        /// <remarks></remarks>
        private void OnActiveViewEventsContentsChanged()
        {
            // TODO: Add your code here
            //System.Windows.Forms.MessageBox.Show("ContentsChanged"); 
        }

        /// <summary>
        /// ContentsCleared Event handler
        /// </summary>
        /// <remarks></remarks>
        private void OnActiveViewEventsContentsCleared()
        {
            // TODO: Add your code here
            //System.Windows.Forms.MessageBox.Show("ContentsCleared"); 
        }

        /// <summary>
        /// FocusMapChanged Event handler
        /// </summary>
        /// <remarks></remarks>
        private void OnActiveViewEventsFocusMapChanged()
        {
            // TODO: Add your code here
            //System.Windows.Forms.MessageBox.Show("FocusMapChanged"); 
        }

        /// <summary>
        /// ItemAdded Event handler
        /// </summary>
        /// <param name="Item"></param>
        /// <remarks></remarks>
        private void OnActiveViewEventsItemAdded(System.Object Item)
        {
            // TODO: Add your code here
            //System.Windows.Forms.MessageBox.Show("ItemAdded"); 
        }

        /// <summary>
        /// ItemDeleted Event handler
        /// </summary>
        /// <param name="Item"></param>
        /// <remarks></remarks>
        private void OnActiveViewEventsItemDeleted(System.Object Item)
        {
            // TODO: Add your code here
            //System.Windows.Forms.MessageBox.Show("ItemDeleted"); 
        }

        /// <summary>
        /// ItemReordered Event handler
        /// </summary>
        /// <param name="Item"></param>
        /// <param name="toIndex"></param>
        /// <remarks></remarks>
        private void OnActiveViewEventsItemReordered(System.Object Item, System.Int32 toIndex)
        {
            // TODO: Add your code here
            //System.Windows.Forms.MessageBox.Show("ItemReordered"); 
        }

        /// <summary>
        /// SelectionChanged Event handler
        /// </summary>
        /// <remarks></remarks>
        private void OnActiveViewEventsSelectionChanged()
        {
            // TODO: Add your code here
            //System.Windows.Forms.MessageBox.Show("SelectionChanged"); 
        }

        /// <summary>
        /// SpatialReferenceChanged Event handler
        /// </summary>
        /// <remarks></remarks>
        private void OnActiveViewEventsSpatialReferenceChanged()
        {
            // TODO: Add your code here
            //System.Windows.Forms.MessageBox.Show("SpatialReferenceChanged"); 
        }

        /// <summary>
        /// ViewRefreshed Event handler
        /// </summary>
        /// <param name="view"></param>
        /// <param name="phase"></param>
        /// <param name="data"></param>
        /// <param name="envelope"></param>
        /// <remarks></remarks>
        private void OnActiveViewEventsViewRefreshed(ESRI.ArcGIS.Carto.IActiveView view, ESRI.ArcGIS.Carto.esriViewDrawPhase phase, System.Object data, ESRI.ArcGIS.Geometry.IEnvelope envelope)
        {
            // TODO: Add your code here
            ////System.Windows.Forms.MessageBox.Show("ViewRefreshed");
        }


        void OnCloseDocument()
        {
            var str = (m_application.Document as IDocumentInfo2).Path;
            if (string.IsNullOrEmpty(str))
                return;
            string FN = System.IO.Path.GetDirectoryName((m_application.Document as IDocumentInfo2).Path) + @"\ContentImage.jpg";

            //if (File.Exists(FN))
            {
                EsriWorkEnvironment.EsriUtils.CreateJPEG_FromActiveView(((IMxDocument)m_application.Document).ActiveView, FN);

                SigmaDataCash.ChartElementList = null;
                SigmaDataCash.environmentWorkspaceEdit = null;
                SigmaDataCash.AnnotationFeatureClassList = null;
                SigmaDataCash.SigmaChartType = -1;
            }


            FN = System.IO.Path.GetDirectoryName((m_application.Document as IDocumentInfo2).Path) + @"\pdm.mdb";

            if (File.Exists(FN))
            {
                EsriWorkEnvironment.EsriUtils.CompactDataBase(FN);

               
            }
        }

        void OnOpenDocument()
        {
            

        }

            #endregion




        }
}
