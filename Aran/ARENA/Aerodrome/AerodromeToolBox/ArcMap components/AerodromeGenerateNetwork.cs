using Aerodrome.Features;
using Aerodrome.Network;
using ESRI.ArcGIS.ADF.BaseClasses;
using ESRI.ArcGIS.ADF.CATIDs;
using ESRI.ArcGIS.ArcMapUI;
using ESRI.ArcGIS.Framework;
using ESRI.ArcGIS.Geodatabase;
using Framework.Stasy;
using Framework.Stasy.Context;
using Framework.Stasy.Core;
using HelperDialog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Interop;

namespace AerodromeToolBox.ArcMap_components
{
    [Guid("36DA9895-209E-4A2B-B77B-22C7D7152449")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("Aerodrome.AerodromeGenerateNetwork")]
    public class AerodromeGenerateNetwork:BaseCommand
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

        public AerodromeGenerateNetwork()
        {
            //
            // TODO: Define values for the public properties
            //
            base.m_category = "Generate network"; //localizable text
            base.m_caption = "Generate network";  //localizable text
            base.m_message = "Generate network";  //localizable text 
            base.m_toolTip = "Generate network";  //localizable text 
            base.m_name = "AerodromeGenerateNetwork";   //unique id, non-localizable (e.g. "MyCategory_ArcMapCommand")

            try
            {
                //
                // TODO: change bitmap name if necessary
                //
                //base.m_bitmap = global::ArenaToolBox.Properties.Resources.Analyze16;
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

            if (AerodromeDataCash.ProjectEnvironment is null)
            {
                System.Windows.MessageBox.Show("Empty project", "Aerodrome", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
                return;
            }

            System.Windows.MessageBoxResult result = System.Windows.MessageBox.Show("Generate a network?", "Aerodrome", System.Windows.MessageBoxButton.OKCancel, System.Windows.MessageBoxImage.Question);
            if (result == System.Windows.MessageBoxResult.Cancel)
            {
                return;
            }

            Splasher.Splash = new SplashScreen();
            var parentHandle = new IntPtr(m_application.hWnd);
            var helper = new WindowInteropHelper(Splasher.Splash) { Owner = parentHandle };
            MessageListener.Instance.ReceiveMessage("");
            Splasher.ShowSplash();

            try
            {

                MessageListener.Instance.ReceiveMessage("Delete old network features...");

                var idList = ((CompositeCollection<AM_AsrnNode>)AerodromeDataCash.ProjectEnvironment.Context.FeatureCollections[typeof(AM_AsrnNode)]).Select(f => f.featureID);
                AerodromeDataCash.ProjectEnvironment.GeoDbProvider.DeleteSelectedRows(typeof(AM_AsrnNode), idList);

                var edgeIdList = ((CompositeCollection<AM_AsrnEdge>)AerodromeDataCash.ProjectEnvironment.Context.FeatureCollections[typeof(AM_AsrnEdge)]).Select(f => f.featureID);
                AerodromeDataCash.ProjectEnvironment.GeoDbProvider.DeleteSelectedRows(typeof(AM_AsrnEdge), edgeIdList);

                var nodeList = ((CompositeCollection<AM_AsrnNode>)AerodromeDataCash.ProjectEnvironment.Context.FeatureCollections[typeof(AM_AsrnNode)]).ToList();
                var edgeList = ((CompositeCollection<AM_AsrnEdge>)AerodromeDataCash.ProjectEnvironment.Context.FeatureCollections[typeof(AM_AsrnEdge)]).ToList();

                AerodromeDataCash.ProjectEnvironment.Context._syncProvider.Delete(nodeList);

                AerodromeDataCash.ProjectEnvironment.Context._syncProvider.Delete(edgeList);

                foreach (var node in nodeList)
                {
                    ((CompositeCollection<AM_AsrnNode>)AerodromeDataCash.ProjectEnvironment.Context.FeatureCollections[typeof(AM_AsrnNode)]).Remove(node);
                }

                foreach (var edge in edgeList)
                {
                    ((CompositeCollection<AM_AsrnEdge>)AerodromeDataCash.ProjectEnvironment.Context.FeatureCollections[typeof(AM_AsrnEdge)]).Remove(edge);
                }

                AerodromeDataCash.ProjectEnvironment.Context._entityContextManager.ResetEntitiesState();

                NetworkGenerationHelper networkGenerator = new NetworkGenerationHelper();

                var ws = ((IDataset)AerodromeDataCash.ProjectEnvironment.pMap.Layer[0]).Workspace;

                networkGenerator.CompactDatabase(ws);
                MessageListener.Instance.ReceiveMessage("Generate AsrnEdge...");
                networkGenerator.GenerateEdge();
                MessageListener.Instance.ReceiveMessage("Generate AsrnNode...");
                networkGenerator.GenerateNode();

                var stateChangedList = AerodromeDataCash.ProjectEnvironment.Context._entityContextManager.GetEntitiesWithState();

                var insertedList = stateChangedList.Where<KeyValuePair<SynchronizationOperation, object>>((Func<KeyValuePair<SynchronizationOperation, object>, bool>)(kvp => kvp.Key == SynchronizationOperation.Inserted)).Select<KeyValuePair<SynchronizationOperation, object>, object>((Func<KeyValuePair<SynchronizationOperation, object>, object>)(kvp => kvp.Value));

                AerodromeDataCash.ProjectEnvironment.Context._syncProvider.Insert(insertedList);

                foreach (var entity in insertedList)
                {
                    AerodromeDataCash.ProjectEnvironment.GeoDbProvider.Insert(AerodromeDataCash.ProjectEnvironment.TableDictionary, (AM_AbstractFeature)entity);
                }

                AerodromeDataCash.ProjectEnvironment.ProjectNeedSaved = true;
                AerodromeDataCash.ProjectEnvironment.Context._entityContextManager.ResetEntitiesState();

                Splasher.CloseSplash();
                MessageScreen messageScreen = new MessageScreen();
                var messageScreeenHelper = new WindowInteropHelper(messageScreen) { Owner = parentHandle };
                messageScreen.MessageText = "Network generated";
                messageScreen.ShowDialog();

            }
            catch (Exception ex)
            {
                Splasher.CloseSplash();
                MessageScreen messageScreen = new MessageScreen();
                var messageScreeenHelper = new WindowInteropHelper(messageScreen) { Owner = parentHandle };
                messageScreen.MessageText = "Сompleted with errors";
                messageScreen.ShowDialog();
            }
            finally
            {
                MessageListener.Instance.ReceiveMessage("");
            }

        }

        public override bool Enabled
        {
            get
            {
                //if editing is started, then enable the command.  
                if (AerodromeDataCash.ProjectEnvironment is null)
                    return false;
                else
                    return true;
            }
        }

        #endregion


    }
}
