using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Forms;
using Aran.Aim.Data;
using Aran.AranEnvironment;
//using Aran.Temporality.CommonUtil.Util;
using Cursors = System.Windows.Input.Cursors;
using Aran.PANDA.Common;
using ESRI.ArcGIS.Controls;
using System.Windows.Interop;
using System.Windows.Forms.Integration;
using Aran.Aim;
using Aran.Delta.View;
using Unity;

namespace Aran.Delta
{
    public class DeltaService : AranPlugin
    {
        public static AranTool ByClickToolButton;
        public IAranEnvironment AranEnv;
        private IntPtr _parentHandle;

        public override void Startup(IAranEnvironment aranEnv)
        {
            var deltaItem = new ToolStripMenuItem();
            deltaItem.Text = "Delta";

            AranEnv = aranEnv;
            var calculatorItem = new ToolStripMenuItem {Text = "Point Creation "};

            var inverseAzimuth = new ToolStripMenuItem { Text = "Direct and Inverse" };
            inverseAzimuth.Click += inverseAzimuth_Click;
            calculatorItem.DropDownItems.Add(inverseAzimuth);

            var distanceDistance = new ToolStripMenuItem { Text = "Distance and Distance" };
            distanceDistance.Click += distanceDistance_Click;
            calculatorItem.DropDownItems.Add(distanceDistance);

            var courseCourseItem = new ToolStripMenuItem { Text = "Course and Course" };
            courseCourseItem.Click += courseCourseItem_Click;
            calculatorItem.DropDownItems.Add(courseCourseItem);
            
            var courseDistance = new ToolStripMenuItem { Text = "Course and Distance" };
            courseDistance.Click += courseDistance_Click;
            calculatorItem.DropDownItems.Add(courseDistance);

            var bufferItem = new ToolStripMenuItem { Text = "Buffer" };
            
            var routeBuffer = new ToolStripMenuItem { Text = "Route" };
            routeBuffer.Click += routeBuffer_Click;

            var airspaceBuffer = new ToolStripMenuItem { Text = "Airspace" };
            airspaceBuffer.Click += airspaceBuffer_Click;

            bufferItem.DropDownItems.Add(routeBuffer);
            bufferItem.DropDownItems.Add(airspaceBuffer);

            var createItem = new ToolStripMenuItem { Text = "Create" };
            var areaItem = new ToolStripMenuItem { Text = "Airspace" };
            areaItem.Click += areaItem_Click;
            createItem.DropDownItems.Add(areaItem);

            var notamItem = new ToolStripMenuItem { Text = "Airspace from text format" };
            notamItem.Click += notamItem_Click;
          //  createItem.DropDownItems.Add(notamItem);

            var routeItem = new ToolStripMenuItem { Text = "Route" };
            routeItem.Click += routeItem_Click;
            createItem.DropDownItems.Add(routeItem);

            var intersectionItem = new ToolStripMenuItem { Text = "Intersection" };
            var threeDIntersectionItem = new ToolStripMenuItem { Text = "3D Intersection" };
            threeDIntersectionItem.Click += threeDIntersectionItem_Click;
            intersectionItem.DropDownItems.Add(threeDIntersectionItem);

            var intersectPointItem = new ToolStripMenuItem { Text = "Intersection Points" };
            intersectPointItem.Click += intersectPointItem_Click;
            intersectionItem.DropDownItems.Add(intersectPointItem);

            var aboutItem = new ToolStripMenuItem { Text = "About" };
            aboutItem.Click += aboutItem_Click;
            createItem.DropDownItems.Add(areaItem);

            deltaItem.DropDownItems.Add(calculatorItem);
            deltaItem.DropDownItems.Add(bufferItem);
            deltaItem.DropDownItems.Add(createItem);
            deltaItem.DropDownItems.Add(intersectionItem);
            deltaItem.DropDownItems.Add(aboutItem);

            aranEnv.AranUI.AddMenuItem(AranMapMenu.Applications, deltaItem);
            GlobalParams.AranEnv = aranEnv;
        }

        void intersectPointItem_Click(object sender, EventArgs e)
        {
            try
            {
                

                if (Init())
                {
                    var window = new Intersection();
                    var helper = new WindowInteropHelper(window);
                    helper.Owner = _parentHandle;
                    ElementHost.EnableModelessKeyboardInterop(window);
                    GlobalParams.HWND = _parentHandle.ToInt32();
                    window.ShowInTaskbar = false; // hide from taskbar and alt-tab list

                    window.Show();
                }
            }
            catch(Exception exception)
            {
                GlobalParams.AranEnv.GetLogger("Delta").Error(exception.Message);
                GlobalParams.AranEnv.GetLogger("Delta").Trace(exception.StackTrace);
                Aran.Delta.Model.Messages.Error(exception.Message);
                Console.WriteLine(exception);
            }
        }

        void threeDIntersectionItem_Click(object sender, EventArgs e)
        {
            try
            {
               

                if (Init())
                {
                    var window = new ThreeDIntersection();
                    var helper = new WindowInteropHelper(window);
                    helper.Owner = _parentHandle;
                    ElementHost.EnableModelessKeyboardInterop(window);
                    GlobalParams.HWND = _parentHandle.ToInt32();
                    window.ShowInTaskbar = false; // hide from taskbar and alt-tab list

                    window.Show();
                }
            }
            catch (Exception exception)
            {
                GlobalParams.AranEnv.GetLogger("Delta").Error(exception, exception.Message);
                Aran.Delta.Model.Messages.Error(exception.Message);
                Console.WriteLine(exception);
            }
        }

        void airspaceBuffer_Click(object sender, EventArgs e)
        {
            try
            {
                if (Init())
                {
                    var window = new AirspaceBuffer();
                    var helper = new WindowInteropHelper(window);
                    helper.Owner = _parentHandle;
                    ElementHost.EnableModelessKeyboardInterop(window);
                    GlobalParams.HWND = _parentHandle.ToInt32();
                    window.ShowInTaskbar = false; // hide from taskbar and alt-tab list

                    window.Show();
                }
            }
            catch (Exception exception)
            {
                Aran.Delta.Model.Messages.Error(exception.Message);
                GlobalParams.AranEnv.GetLogger("Delta").Error(exception, exception.Message);
            }
        }

        private void aboutItem_Click(object sender, EventArgs e)
        {
            var window = new About();
            var helper = new WindowInteropHelper(window);
            helper.Owner = _parentHandle;
            ElementHost.EnableModelessKeyboardInterop(window);
            GlobalParams.HWND = _parentHandle.ToInt32();
            window.ShowInTaskbar = false; // hide from taskbar and alt-tab list
            window.Show();
        }

        private void routeItem_Click(object sender, EventArgs e)
        {
            try
            {
                var unityContainer = new UnityContainer();
                ContainerBootstrapperForAran.RegisterTypesForCreateRoutes(unityContainer, AranEnv);

                if (Init())
                {
                    var window = unityContainer.Resolve<CreateRoute>();
                    var helper = new WindowInteropHelper(window);
                    helper.Owner = _parentHandle;
                    ElementHost.EnableModelessKeyboardInterop(window);
                    GlobalParams.HWND = _parentHandle.ToInt32();
                    window.ShowInTaskbar = false; // hide from taskbar and alt-tab list

                    window.Show();
                }
            }
            catch (Exception exception)
            {
                GlobalParams.AranEnv.GetLogger("Delta").Error(exception, exception.Message);
                Aran.Delta.Model.Messages.Error(exception.StackTrace);
            }
        }

        void notamItem_Click(object sender, EventArgs e)
        {
            if (Init())
            {
                var window = new CreateNotam();
                var helper = new WindowInteropHelper(window);
                helper.Owner = _parentHandle;
                ElementHost.EnableModelessKeyboardInterop(window);
                GlobalParams.HWND = _parentHandle.ToInt32();
                window.ShowInTaskbar = false; // hide from taskbar and alt-tab list
                window.Show();
            } 
        }

        void areaItem_Click(object sender, EventArgs e)
        {
            try
            {
                InitializeContainer();

                if (Init())
                {
                    var accuracyWindow = new AccuracyInit
                    {
                        ShowInTaskbar = false
                    };
                    bool? dialogResult = accuracyWindow.ShowDialog();
                    accuracyWindow.Close();

                    if (dialogResult == true)
                    {
                        var window = new CreateAirspace();
                        var helper = new WindowInteropHelper(window)
                        {
                            Owner = _parentHandle
                        };

                        ElementHost.EnableModelessKeyboardInterop(window);
                        GlobalParams.HWND = _parentHandle.ToInt32();
                        window.ShowInTaskbar = false; // hide from taskbar and alt-tab list
                        window.Show();
                    }
                    else
                    {
                        Model.Messages.Error("Accuracy must be entered to proceed work");
                    }
                }
            }
            catch (Exception exception)
            {
                GlobalParams.AranEnv.GetLogger("Delta").Error(exception, exception.Message);
                Aran.Delta.Model.Messages.Error(exception.Message);
            }
        }

        void routeBuffer_Click(object sender, EventArgs e)
        {
            try
            {
                InitializeContainer();

                if (Init())
                {
                    var window = new RouteBuffer();
                    var helper = new WindowInteropHelper(window);
                    helper.Owner = _parentHandle;
                    ElementHost.EnableModelessKeyboardInterop(window);
                    GlobalParams.HWND = _parentHandle.ToInt32();
                    window.ShowInTaskbar = false; // hide from taskbar and alt-tab list
                    window.Show();
                }
            }
            catch (Exception ex)
            {
                GlobalParams.AranEnv.GetLogger("Delta").Error(ex, ex.Message);
                Aran.Delta.Model.Messages.Error(ex.Message);
            }
        }

        private void inverseAzimuth_Click(object sender, EventArgs e)
        {
            try
            {
                InitializeContainer();

                if (Init())
                {
                    var window = new DirectInverse(false);

                    var helper = new WindowInteropHelper(window);
                    helper.Owner = _parentHandle;
                    ElementHost.EnableModelessKeyboardInterop(window);
                    GlobalParams.HWND = _parentHandle.ToInt32();
                    window.ShowInTaskbar = false; // hide from taskbar and alt-tab list
                    window.Show();
                }
            }
            catch (Exception exception)
            {
                GlobalParams.AranEnv.GetLogger("Delta").Error(exception, exception.Message);
                Aran.Delta.Model.Messages.Error(exception.Message);
            }
        }

        void InitializeContainer()
        {
            var unityContainer = new UnityContainer();
            ContainerBootstrapperForAran.RegisterTypesForCreateRoutes(unityContainer, AranEnv);
        }

        void distanceDistance_Click(object sender, EventArgs e)
        {
            try
            {
                if (Init())
                {
                    var window = new DistanceDistance();

                    var helper = new WindowInteropHelper(window);
                    helper.Owner = _parentHandle;
                    ElementHost.EnableModelessKeyboardInterop(window);
                    GlobalParams.HWND = _parentHandle.ToInt32();
                    window.ShowInTaskbar = false; // hide from taskbar and alt-tab list
                    window.Show();
                }
            }
            catch (Exception exception)
            {
                GlobalParams.AranEnv.GetLogger("Delta").Error(exception, exception.Message);
                Aran.Delta.Model.Messages.Error(exception.Message);
            }
        }

        void courseCourseItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (Init())
                {
                    var window = new CourseCourse();

                    var helper = new WindowInteropHelper(window);
                    helper.Owner = _parentHandle;
                    ElementHost.EnableModelessKeyboardInterop(window);
                    GlobalParams.HWND = _parentHandle.ToInt32();
                    window.ShowInTaskbar = false; // hide from taskbar and alt-tab list
                    window.Show();
                }
            }
            catch (Exception exception)
            {
                GlobalParams.AranEnv.GetLogger("Delta").Error(exception, exception.Message);
                Aran.Delta.Model.Messages.Error(exception.Message);
            }
        }

        void courseDistance_Click(object sender, EventArgs e)
        {
            try
            {
                if (Init())
                {
                    var window = new CourseDistance();

                    var helper = new WindowInteropHelper(window);
                    helper.Owner = _parentHandle;
                    ElementHost.EnableModelessKeyboardInterop(window);
                    GlobalParams.HWND = _parentHandle.ToInt32();
                    window.ShowInTaskbar = false; // hide from taskbar and alt-tab list
                    window.Show();
                }
            }
            catch (Exception exception)
            {
                GlobalParams.AranEnv.GetLogger("Delta").Error(exception, exception.Message);
                Aran.Delta.Model.Messages.Error(exception.Message);
            }
        }

        private bool Init() 
        {
            if (InitDelta.InitCommand())
            {
                AranTool aranToolItem = new AranTool();
                aranToolItem.Visible = false;
                aranToolItem.Cursor = System.Windows.Forms.Cursors.Cross;
                ByClickToolButton = aranToolItem;
                _parentHandle = GlobalParams.AranEnv.Win32Window.Handle; // the ArcMap window handle
                var canEdit = true;

                bool slotSelector = true;
                var dbProvider = GlobalParams.AranEnv.DbProvider as DbProvider;
                if (dbProvider != null && dbProvider.ProviderType == DbProviderType.TDB)
                {
                    dynamic methodResult = new System.Dynamic.ExpandoObject();
                    dbProvider.CallSpecialMethod("SelectSlot", methodResult);
                    slotSelector = methodResult.Result;
                }

                if (!slotSelector)
                {
                    System.Windows.MessageBox.Show("Please first select slot!", "Delta", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return false;
                }
                return canEdit;
            }
            return false;
        }

        public override string Name
        {
            get { return "Delta"; }
        }

        public void AddChildSubMenu(List<string> hierarcy)
        {
            throw new NotImplementedException();
        }

        public override Guid Id
        {
            get { return new Guid("d86ab787-4232-422c-b2ec-8f155b936c7b"); }
        }

        public override List<Aim.FeatureType> GetLayerFeatureTypes()
        {
            var list = new List<FeatureType>();

            list.Add(FeatureType.AirportHeliport);
            list.Add(FeatureType.DesignatedPoint);
            list.Add(FeatureType.Navaid);
            list.Add(FeatureType.Airspace);
            list.Add(FeatureType.RouteSegment);

            return list;
        }
    }

}
