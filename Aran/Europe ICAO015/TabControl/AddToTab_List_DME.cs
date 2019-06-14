using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICAO015.TabControl
{
    public class AddToTab_List_DME
    {
        string typeofnavigation;
        string checkednavaid;
        double radiusrcylinder;
        double alphacone;
        double radiusrcone;
        double radiusjcylinder;
        string heightofcylinderwindturbine;

        public string TypeOFNavigation
        {
            get { return typeofnavigation; }
            set { typeofnavigation = value; }
        }
        public string CheckedNavaid
        {
            get { return checkednavaid; }
            set { checkednavaid = value; }
        }
        public double RadiusRCylinder
        {
            get { return radiusrcylinder; }
            set { radiusrcylinder = value; }
        }
        public double AlphaCone
        {
            get { return alphacone; }
            set { alphacone = value; }
        }
        public double RadiusRCone
        {
            get { return radiusrcone; }
            set { radiusrcone = value; }
        }
        public double RadiusJCylinder
        {
            get { return radiusjcylinder; }
            set { radiusjcylinder = value; }
        }
        public string HeightOfCylndrWindTurbine
        {
            get { return heightofcylinderwindturbine; }
            set { heightofcylinderwindturbine = value; }
        }


    }
}
