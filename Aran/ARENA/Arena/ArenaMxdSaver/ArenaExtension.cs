using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Framework;
using ESRI.ArcGIS.ADF.CATIDs;
using ESRI.ArcGIS.ArcMapUI;
using ESRI.ArcGIS.Carto;

namespace ARENA
{
    [Guid("c6e6a8f5-d366-4aec-b6ab-8a8521ea80af")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("Arena.ArenaExtension")]
    public class ArenaExtension : IExtension, IPersistVariant
    {
        private string arenaProjectIdentifier;

        #region IActiveViewEventsHandler

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

        #endregion

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
                return "ArenaExtension";
            }
        }

        public void Shutdown()
        {
            //TODO: Clean up resources

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

            IMxDocument pNewDocument = (IMxDocument)m_application.Document;
            IMap pMap = pNewDocument.FocusMap;

            //SetupActiveViewEvents(pMap);
            arenaProjectIdentifier = Convert.ToString(Stream.Read());

        }

        public void Save(IVariantStream Stream)
        {
            arenaProjectIdentifier = "ARENA";
            Stream.Write(arenaProjectIdentifier);
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


        #endregion

        
    }
}
