using System;
using System.Net.Mime;
using System.Threading;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Threading;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
using VisualizerEnvironment.Properties;
using Application = System.Windows.Application;
using MessageBox = System.Windows.MessageBox;

namespace VisualizerEnvironment.View
{
    public sealed partial class MapForm : Form
    {
        #region class public members
        public IMapControl3 MapControl = null;
        public string MapDocumentName = string.Empty;
        public Action<string> MapChangedAction { get; set; }

        #endregion

        #region class constructor
        public MapForm()
        {
            InitializeComponent();
        }
        #endregion


        public void LoadMap()
        {
            MapControl = (IMapControl3)axMapControl1.Object;

            var mapDocument = Settings.Default["MapDocument"].ToString();
            if (!string.IsNullOrEmpty(mapDocument))
            {
                try
                {
                    MapControl.LoadMxFile(mapDocument);
                    (MapControl.Map as IActiveView).Refresh();
                    Invalidate();
                    Refresh();
                }
                catch (System.Exception)
                {
                    MessageBox.Show("There was error at loading map file " + mapDocument + ".", "Map can not be loaded",
                                    MessageBoxButton.OK, MessageBoxImage.Error);
                    MapDocumentName = string.Empty;
                }
            }
        }

        private void MainFormLoad(object sender, EventArgs e)
        {
            Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal,(Action)(LoadMap));
        }

      //listen to MapReplaced evant in order to update the statusbar and the Save menu
        private void axMapControl1_OnMapReplaced(object sender, IMapControlEvents2_OnMapReplacedEvent e)
        {
            //get the current document name from the MapControl
            MapDocumentName = MapControl.DocumentFilename;

            //if there is no MapDocument, diable the Save menu and clear the statusbar
            if (MapDocumentName == string.Empty)
            {
              
            }
            else
            {
               
            }

            if (MapChangedAction!=null)
            {
                MapChangedAction(MapDocumentName);
            }
        }


    }
}