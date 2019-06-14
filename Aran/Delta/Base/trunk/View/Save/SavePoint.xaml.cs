using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Aran.Delta.Model;
using ARENA;
using PDM;

namespace Aran.Delta.View
{
    /// <summary>
    /// Interaction logic for SavePoint.xaml
    /// </summary>
    public partial class SavePoint : Window
    {
        private PDMObject _pdmObject;
        public SavePoint(PDMObject pdmObject)
        {
            InitializeComponent();
            _pdmObject = pdmObject;

            if (_pdmObject is Airspace)
                TxtName.Text =(_pdmObject as Airspace).TxtName;
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TxtName.Text))
            {
                Messages.Error("Name can not be empty!");
                return;
            }

            if (_pdmObject is WayPoint)
            {
                (_pdmObject as WayPoint).Designator = TxtName.Text;
                (_pdmObject as WayPoint).Name = TxtName.Text;
            }
            else if (_pdmObject is Airspace)
            {
                var airspace = _pdmObject as Airspace;
                airspace.TxtName = TxtName.Text;
                airspace.CodeID = TxtName.Text;

                foreach (var airspaceVolume in airspace.AirspaceVolumeList)
                {
                    airspaceVolume.CodeId = TxtName.Text;
                    airspaceVolume.TxtName = TxtName.Text;
                }
            }
            else if (_pdmObject is RouteSegment)
            {
                var routeSegment = _pdmObject as RouteSegment;
                //routeSegment.
            }

            if (DataCash.StorePDMobject(_pdmObject))
            {
                Messages.Info("Feature saved database successfully");
                Close();
            }

        }
    }
}
