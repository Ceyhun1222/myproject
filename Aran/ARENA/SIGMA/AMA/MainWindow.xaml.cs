using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ESRI.ArcGIS.ADF;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.DataSourcesRaster;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geometry;
using Aran.PANDA.Common;
using ESRI.ArcGIS.Geodatabase;
using ANCOR.MapElements;
using ESRI.ArcGIS.Framework;
using ESRI.ArcGIS.ArcMapUI;
using System.Timers;
using System.Diagnostics;


namespace SigmaChart
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Timer _timer = new Timer(50000);
        private Stopwatch st;
        private GeoDatabaseRepository _geoDbRepository;
        private LayerHelper _layerHelper;

        public MainWindow(IMap focusMap,LayerHelper layerHelper)
        {
            InitializeComponent();

            _layerHelper = layerHelper;

            _geoDbRepository = new GeoDatabaseRepository(_layerHelper);

            DataContext = this;

            st = new Stopwatch();

            SetInitData();

            
        }


        public List<IRasterLayer> RasterLayers { get; set; }
        public List<IMapGrid> GridList { get; set; }
        public List<double> MOCList { get; set; }

        private void InitizlizeSettings()
        {
            var settings = new Settings();
            settings.UIIntefaceData.DistanceUnit = HorizantalDistanceType.NM;
            settings.UIIntefaceData.DistancePrecision = 1;
            settings.UIIntefaceData.HeightUnit = VerticalDistanceType.Ft;
            settings.UIIntefaceData.HeightPrecision = 100;
            GlobalParams.UnitConverter = new Aran.PANDA.Common.UnitConverter(settings);
        }

        private void LoadLayers()
        {
            RasterLayers = _layerHelper.GetRasterLayers();
            GridList = _layerHelper.GetMapGrid();

            foreach (var rasterLayer in RasterLayers)
                CmbRaster.Items.Add(rasterLayer.Name);

            foreach (var mapGrid in GridList)
                CmbGrid.Items.Add(mapGrid.Name);

            if (RasterLayers.Count > 0)
                CmbRaster.SelectedIndex = 0;

            if (GridList.Count > 0)
                CmbGrid.SelectedIndex = 0;
        }

        private void SetInitData()
        {
            InitizlizeSettings();
          
            GlobalParams.DbModule = new DbModule();

            LoadLayers();

            //If settings is foot
            MOCList = new List<double> { 1000, 2000 };
            CmbMOC.ItemsSource = MOCList;
            CmbMOC.SelectedIndex = 0;
        }

        private  void CreateAma_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                IRaster selRaster = null;

                Cursor = Cursors.Wait;

                st.Start();

                if (CmbRaster.SelectedIndex > -1)
                    selRaster = RasterLayers[CmbRaster.SelectedIndex].Raster;

                if (CmbGrid.SelectedIndex == -1)
                    throw new Exception("There are not any Grids!");

                IMapGrid selGrid = GridList[CmbGrid.SelectedIndex];

                var createAma = new CreateAMA(GlobalParams.HookHelper.ActiveView,
                    GlobalParams.HookHelper.PageLayout, selGrid, selRaster);

                createAma.ProgressHandler += changeTimer;
                if (createAma.Terrain != null)
                    createAma.Terrain.ProgressHandler += changeTimer;

                var gridMuraList = createAma.GetAmaListSync(true, true);
                _geoDbRepository.SaveGrids(gridMuraList,MOCList[CmbMOC.SelectedIndex]);

                lblCurPorition.Dispatcher.Invoke((Action) (() => lblCurPorition.Text = "Done"));
                st.Stop();
                changeTimer(st.Elapsed.ToString(), null);
                ProgBar.Dispatcher.Invoke((Action) (() => ProgBar.Value = ProgBar.Maximum));

                MessageBox.Show("AMEA is saved successfully!", "Area Minimum Enroute Altitude", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Area Minumum Altitude", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void changeTimer(object sender, EventArgs e)
        {
            var message = sender as string;
            ProgBar.Dispatcher.Invoke((Action)(() =>
            {
                if (ProgBar.Value + 5 < ProgBar.Maximum)
                    ProgBar.Value += 5;
                lblCurPorition.Dispatcher.Invoke((Action)(() => lblCurPorition.Text = message));
            }));
        }

        void _timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            //ProgBar.Dispatcher.Invoke((Action)(() => ProgBar.Value += 10));
        }

    }
}
