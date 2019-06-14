using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using PDM.PropertyExtension;
using ESRI.ArcGIS.Geodatabase;
using System.Xml.Serialization;

namespace PDM
{
    public class AircraftCharacteristic
    {

        private AircraftCategoryType _aircraftLandingCategory;

        public AircraftCategoryType AircraftLandingCategory
        {
            get { return _aircraftLandingCategory; }
            set { _aircraftLandingCategory = value; }
        }


        public AircraftCharacteristic()
        {
        }


    }
}
