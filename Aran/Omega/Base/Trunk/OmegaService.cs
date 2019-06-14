using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aran.AranEnvironment;
using System.Windows.Forms;
using System.Windows.Interop;
using System.Windows.Forms.Integration;
using System.Windows;
using Aran.Omega.Properties;
using Aran.Omega.View;
using Aran.Aim;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading;
using Aran.Aim.Data;


namespace Aran.Omega
{
    public class OmegaService : AranPlugin
    {
        public static AranTool ByClickToolButton;

        public override void Startup(IAranEnvironment aranEnv)
        {
            var menuItem = new ToolStripMenuItem { Text = Resources._Omega };

            var olsMenuItem = new ToolStripMenuItem { Text = Resources._Annex_14___OLS_ };
            olsMenuItem.Click += new EventHandler(OlsMenuItem_Click);
            menuItem.DropDownItems.Add(olsMenuItem);

            var etodMenuItem = new ToolStripMenuItem();
            etodMenuItem.Text = Resources._eTOD;
            etodMenuItem.Click += new EventHandler(EtodMenuItem_Click);
            //#if (!Riga)
            //                menuItem.DropDownItems.Add(etodMenuItem);

            //#endif

#if Etod
            menuItem.DropDownItems.Add(etodMenuItem);
#endif

            var aboutMenuItem = new ToolStripMenuItem("About");
            aboutMenuItem.Click += new EventHandler(about_onClick);
            menuItem.DropDownItems.Add(aboutMenuItem);

            aranEnv.AranUI.AddMenuItem(AranMapMenu.Applications, menuItem);

            var win32Window = new Win32Windows(aranEnv.Win32Window.Handle.ToInt32());
            GlobalParams.AranEnvironment = aranEnv;
        }

        private void about_onClick(object sender, EventArgs e)
        {
            var window = new About();
            var helper = new WindowInteropHelper(window) { Owner = GlobalParams.AranEnvironment.Win32Window.Handle };
            ElementHost.EnableModelessKeyboardInterop(window);
            window.ShowInTaskbar = false; // hide from taskbar and alt-tab list
            window.Show();
        }

        private void OlsMenuItem_Click(object sender, EventArgs e)
        {
            var slotSelector = true;

            var dbProvider = GlobalParams.AranEnvironment.DbProvider as DbProvider;
            if (dbProvider != null && dbProvider.ProviderType == DbProviderType.TDB)
            {
                dynamic methodResult = new System.Dynamic.ExpandoObject();
                dbProvider.CallSpecialMethod("SelectSlot", methodResult);
                slotSelector = methodResult.Result;
            }

            if (!slotSelector)
            {
                System.Windows.MessageBox.Show("Please first select slot!", Resources._Omega, MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!InitOmega.InitCommand()) return;

            var aranToolItem = new AranTool { Visible = false, Cursor = Cursors.Cross };
            ByClickToolButton = aranToolItem;
            var parentHandle = GlobalParams.AranEnvironment.Win32Window.Handle; // the ArcMap window handle

            try
            {
                GlobalParams.OLSWindow = new OmegaMainForm();
                var window = GlobalParams.OLSWindow;
                

                var helper = new WindowInteropHelper(window) { Owner = parentHandle };
                ElementHost.EnableModelessKeyboardInterop(window);
                GlobalParams.HWND = parentHandle.ToInt32();
                window.ShowInTaskbar = false; // hide from taskbar and alt-tab list
                window.Show();
            }
            catch (Exception ex)
            {
                GlobalParams.AranEnvironment?.GetLogger("Omega").Error(ex, ex.Message);
                System.Windows.MessageBox.Show(ex.Message, Resources._Omega, MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private void EtodMenuItem_Click(object sender, EventArgs e)
        {
            var slotSelector = true;

            var dbProvider = GlobalParams.AranEnvironment.DbProvider as DbProvider;
            if (dbProvider != null && dbProvider.ProviderType == DbProviderType.TDB)
            {
                dynamic methodResult = new System.Dynamic.ExpandoObject();
                dbProvider.CallSpecialMethod("SelectSlot", methodResult);
                slotSelector = methodResult.Result;
            }

            if (!slotSelector)
            {
                System.Windows.MessageBox.Show("Please first select slot!", Resources._Omega, MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!InitOmega.InitCommand()) return;

            var aranToolItem = new AranTool { Visible = false, Cursor = Cursors.Cross };

            ByClickToolButton = aranToolItem;
            var parentHandle = GlobalParams.AranEnvironment.Win32Window.Handle; // the ArcMap window handle

            try
            {
                GlobalParams.ETODWindow = new Aran.Omega.View.EtodForm();
                var window = GlobalParams.ETODWindow;

                

                var helper = new WindowInteropHelper(window) { Owner = parentHandle };
                ElementHost.EnableModelessKeyboardInterop(window);
                GlobalParams.HWND = parentHandle.ToInt32();
                window.ShowInTaskbar = false; // hide from taskbar and alt-tab list
                window.Show();

            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Error happened!Please have a look logs!");
                GlobalParams.Logger.Error(ex.Message);
            }
        }

        public override Guid Id
        {
            get { return new Guid("0eaf1c3f-951e-4246-a6aa-5f55a9489163"); }
        }

        public override string Name
        {
            get { return "OMEGA"; }
        }

        public override List<FeatureType> GetLayerFeatureTypes()
        {
            var list = new List<FeatureType>();

            list.Add(FeatureType.AirportHeliport);
            list.Add(FeatureType.RunwayCentrelinePoint);
            list.Add(FeatureType.VerticalStructure);
            list.Add(FeatureType.ObstacleArea);

            return list;
        }

        public void MakeAssembly(AssemblyName myAssemblyName, string fileName)
        {
            // Get the assembly builder from the application domain associated with the current thread.
            AssemblyBuilder myAssemblyBuilder = Thread.GetDomain().DefineDynamicAssembly(myAssemblyName, AssemblyBuilderAccess.RunAndSave);
            // Create a dynamic module in the assembly.
            ModuleBuilder myModuleBuilder = myAssemblyBuilder.DefineDynamicModule("MyModule", fileName);
            // Create a type in the module.
            TypeBuilder myTypeBuilder = myModuleBuilder.DefineType("MyType");
            // Create a method called 'Main'.
            MethodBuilder myMethodBuilder = myTypeBuilder.DefineMethod("Main", MethodAttributes.Public | MethodAttributes.HideBySig |
               MethodAttributes.Static, typeof(void), null);
            // Get the Intermediate Language generator for the method.
            ILGenerator myILGenerator = myMethodBuilder.GetILGenerator();
            // Use the utility method to generate the IL instructions that print a string to the console.
            myILGenerator.EmitWriteLine("Hello World!");
            // Generate the 'ret' IL instruction.
            myILGenerator.Emit(OpCodes.Ret);
            // End the creation of the type.
            myTypeBuilder.CreateType();
            // Set the method with name 'Main' as the entry point in the assembly.
            myAssemblyBuilder.SetEntryPoint(myMethodBuilder);
            myAssemblyBuilder.Save(fileName);
        }

        public void CreateAutomaticBuilder()
        {
            // Create a dynamic assembly with name 'MyAssembly' and build version '1.0.0.2001'.
            var myAssemblyName = new AssemblyName { Name = "Aran.Omega", Version = new Version("5.1.0.0") };
            MakeAssembly(myAssemblyName, "Aran.Omega.dll");

            // Get all the assemblies currently loaded in the application domain.
            var myAssemblies = Thread.GetDomain().GetAssemblies();

            // Get the dynamic assembly named 'MyAssembly'. 
            Assembly myAssembly = null;
            foreach (var assesAssembly in myAssemblies)
            {
                if (System.String.CompareOrdinal(assesAssembly.GetName().Name, "Aran.Omega") == 0)
                    myAssembly = assesAssembly;
            }
            if (myAssembly != null)
            {
                Console.WriteLine(@"Displaying the assembly name");
                Console.WriteLine(myAssembly);
            }

        }
    }
}
