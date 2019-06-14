using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Carto;

namespace MapEnv
{
    public partial class TestForm : Form
    {
        public TestForm ()
        {
            InitializeComponent ();
        }

        public void SetMap (IMap map)
        {
            var focusMap = ui_esriPageLayoutControl.ActiveView.FocusMap;
            focusMap.SpatialReference = map.SpatialReference;


            for (int i = 0; i < map.LayerCount; i++)
                focusMap.AddLayer (map.Layer [i]);
        }

        private void TestForm_Load (object sender, EventArgs e)
        {
            //ui_esriPageLayoutControl.LoadMxFile ("");
        }
    }
}
