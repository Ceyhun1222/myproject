using System;
using System.IO;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geodatabase;

namespace TOSSM.Util
{
    public partial class GeoDbUtil
    {

        #region Create Workspace

        public static IWorkspace OpenScratchWorkspace()
        {
            Type factoryType = Type.GetTypeFromProgID("esriDataSourcesGDB.ScratchWorkspaceFactory");
            IScratchWorkspaceFactory scratchWorkspaceFactory = (IScratchWorkspaceFactory)
            Activator.CreateInstance(factoryType);
            IWorkspace scratchWorkspace = scratchWorkspaceFactory.DefaultScratchWorkspace;
            return scratchWorkspace;
        }

        public static IWorkspace OpenFileGdbScratchWorkspace()
        {
            // Create a file scratch workspace factory.
            Type factoryType = Type.GetTypeFromProgID("esriDataSourcesGDB.FileGDBScratchWorkspaceFactory");
            IScratchWorkspaceFactory scratchWorkspaceFactory = (IScratchWorkspaceFactory)
            Activator.CreateInstance(factoryType);
            IWorkspace scratchWorkspace = scratchWorkspaceFactory.DefaultScratchWorkspace;
            return scratchWorkspace;
        }

        public static IWorkspace CreateInMemoryWorkspace(string folder, string databaseName)
        {
            Type factoryType = Type.GetTypeFromProgID("esriDataSourcesGDB.InMemoryWorkspaceFactory");
            IWorkspaceFactory workspaceFactory = (IWorkspaceFactory) Activator.CreateInstance
                (factoryType);
            IWorkspaceName workspaceName = workspaceFactory.Create(null, databaseName, null, 0);
            IName name = (IName) workspaceName;
            IWorkspace workspace = (IWorkspace) name.Open();

            return workspace;
        }

        public static IWorkspace CreateAccessWorkspace( string folder, string databaseName)
        {
            Directory.CreateDirectory(folder);

            // Instantiate an Access workspace factory and create a personal geodatabase.
            // The Create method returns a workspace name object.
            Type factoryType = Type.GetTypeFromProgID("esriDataSourcesGDB.AccessWorkspaceFactory");
            IWorkspaceFactory workspaceFactory = (IWorkspaceFactory)Activator.CreateInstance(factoryType);
            IWorkspaceName workspaceName = workspaceFactory.Create(folder, databaseName, null, 0);

            // Cast the workspace name object to the IName interface and open the workspace.
            IName name = (IName)workspaceName;
            IWorkspace workspace = (IWorkspace)name.Open();
            return workspace;
        }

        public static IWorkspace OpenAccessWorkspace(string databasePath)
        {
            // Instantiate an Access workspace factory and create a personal geodatabase.
            // The Create method returns a workspace name object.
            Type factoryType = Type.GetTypeFromProgID("esriDataSourcesGDB.AccessWorkspaceFactory");
            IWorkspaceFactory workspaceFactory = (IWorkspaceFactory)Activator.CreateInstance(factoryType);
            IWorkspace workspace = workspaceFactory.OpenFromFile(databasePath, 0);
            return workspace;
        }

        public static IWorkspace OpenFileGdbWorkspace(string databasePath)
        {
            // Instantiate a file geodatabase workspace factory and create a file geodatabase.
            // The Create method returns a workspace name object.
            Type factoryType = Type.GetTypeFromProgID("esriDataSourcesGDB.FileGDBWorkspaceFactory");
            IWorkspaceFactory workspaceFactory = (IWorkspaceFactory)Activator.CreateInstance(factoryType);
            IWorkspace workspace = workspaceFactory.OpenFromFile(databasePath, 0);
            return workspace;
        }

        public static IWorkspace CreateFileGdbWorkspace(string folder, string databaseName)
        {
            Directory.CreateDirectory(folder);

            // Instantiate a file geodatabase workspace factory and create a file geodatabase.
            // The Create method returns a workspace name object.
            Type factoryType = Type.GetTypeFromProgID("esriDataSourcesGDB.FileGDBWorkspaceFactory");
            IWorkspaceFactory workspaceFactory = (IWorkspaceFactory)Activator.CreateInstance(factoryType);
            IWorkspaceName workspaceName = workspaceFactory.Create(folder, databaseName, null, 0);

            // Cast the workspace name object to the IName interface and open the workspace.
            IName name = (IName)workspaceName;
            IWorkspace workspace = (IWorkspace)name.Open();
            return workspace;
        }

        #endregion

     

    }
}
