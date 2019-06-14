using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICAO015
{
    public class AddReportListObstInputForOnlyRadiusCalculated
    {
        double distance;
        double penetrate;
        double radius;
        string navaidname;
        string equation;
        string typeofnavigation;

        public string TypeOfNAvigation
        {
            get { return typeofnavigation; }
            set { typeofnavigation = value; }
        }
        public double Distance
        {
            get { return distance; }
            set { distance = value; }
        }
        public double Penetrate
        {
            get { return penetrate; }
            set { penetrate = value; }
        }
        public string Equation
        {
            get { return equation; }
            set { equation = value; }
        }
        public string NavaidName
        {
            get { return navaidname; }
            set { navaidname = value; }
        }
        public double Radius
        {
            get { return radius; }
            set { radius = value; }
        }
    }
    public class AddReportListObstInputForOnly2DGraphicCalculated
    {
        string typeofnavigationfacility;
        string navaidname;
        string polygon;
        double penetrate;
        double distance;
        string equation;

        public string TypeOfNavigation
        {
            get { return typeofnavigationfacility; }
            set { typeofnavigationfacility = value; }
        }
        public string NavaidName
        {
            get { return navaidname; }
            set { navaidname = value; }
        }
        public string Polygon
        {
            get { return polygon; }
            set { polygon = value; }
        }
        public string Equation
        {
            get { return equation; }
            set { equation = value; }
        }
        public double Penetrate
        {
            get { return penetrate; }
            set { penetrate = value; }
        }
        public double ObstclDistance
        {
            get { return distance; }
            set { distance = value; }
        }
    }
}
