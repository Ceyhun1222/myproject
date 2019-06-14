using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICAO015
{
    //CordinateLists  {
    public class ParameterForDmeN
    {
        double heightdistance;
        Aran.Geometries.Point coordinate;
        string typeofnavigation;
        string navaidname;
        double dmeforsmallradius;
        double dmeforlargeradius;
        double dmeforalpha;

        public double HeightDistance
        {
            get { return heightdistance; }
            set { heightdistance = value; }
        }
        public Aran.Geometries.Point Coordinate
        {
            get { return coordinate; }
            set { coordinate = value; }
        }
        public string NavaidName
        {
            get { return navaidname; }
            set { navaidname = value; }
        }
        public string TypeofNavigation
        {
            get { return typeofnavigation; }
            set { typeofnavigation = value; }
        }
        public double DmeForSmallRadius
        {
            get { return dmeforsmallradius; }
            set { dmeforsmallradius = value; }
        }
        public double DmeForLargeRadius
        {
            get
            {
                return dmeforlargeradius;
            }
            set
            {
                dmeforlargeradius = value;
            }
        }
        public double DmeForAlpha
        {
            get
            {
                return dmeforalpha;
            }
            set
            {
                dmeforalpha = value;
            }
        }
    }
    public class ParameterForCVOR
    {
        double heightdistance;
        Aran.Geometries.Point coordinate;
        string navaidname;
        string typeOfnavigation;
        double cvorforsmallradius;
        double cvorformiddleradius;
        double cvorforlargeradius;
        double cvorforalpha;
        double windturbineheight;

        public double HeightDistance
        {
            get { return heightdistance; }
            set { heightdistance = value; }
        }
        public Aran.Geometries.Point Coordinate
        {
            get { return coordinate; }
            set { coordinate = value; }
        }
        public string NavaidName
        {
            get { return navaidname; }
            set { navaidname = value; }
        }
        public string TypeOfNavigation
        {
            get { return typeOfnavigation; }
            set { typeOfnavigation = value; }
        }
        public double CVORForSmallRadius
        {
            get
            {
                return cvorforsmallradius;
            }
            set
            {
                cvorforsmallradius = value;
            }
        }
        public double CVORForLargeRadius
        {
            get
            {
                return cvorforlargeradius;
            }
            set
            {
                cvorforlargeradius = value;
            }
        }
        public double CVORForMiddleRadius
        {
            get { return cvorformiddleradius; }
            set { cvorformiddleradius = value; }
        }
        public double CVORForAlpha
        {
            get
            {
                return cvorforalpha;
            }
            set
            {
                cvorforalpha = value;
            }
        }
        public double WindTurbineHeight
        {
            get { return windturbineheight; }
            set { windturbineheight = value; }
        }
    }
    public class ParameterForDVOR
    {
        double heightdistance;
        Aran.Geometries.Point coordinate;
        string navaidname;
        string typeofnavigation;
        double dvorforsmallradius;
        double dvorformiddleradius;
        double dvorforlargeradius;
        double dvorforalpha;
        double windturbineheight;

        public double HeightDistance
        {
            get { return heightdistance; }
            set { heightdistance = value; }
        }
        public Aran.Geometries.Point Coordinate
        {
            get { return coordinate; }
            set { coordinate = value; }
        }
        public string NavaidName
        {
            get { return navaidname; }
            set { navaidname = value; }
        }
        public string TypeOfNavigation
        {
            get { return typeofnavigation; }
            set { typeofnavigation = value; }
        }
        public double DVORForSmallRadius
        {
            get
            {
                return dvorforsmallradius;
            }
            set
            {
                dvorforsmallradius = value;
            }
        }
        public double DVORForLargeRadius
        {
            get
            {
                return dvorforlargeradius;
            }
            set
            {
                dvorforlargeradius = value;
            }
        }
        public double DVORForMiddleRadius
        {
            get { return dvorformiddleradius; }
            set { dvorformiddleradius = value; }
        }
        public double DVORForAlpha
        {
            get
            {
                return dvorforalpha;
            }
            set
            {
                dvorforalpha = value;
            }
        }
        public double WindTurbineHeight
        {
            get { return windturbineheight; }
            set { windturbineheight = value; }
        }
    }
    public class ParameterForMarkers
    {
        double heightdistance;
        Aran.Geometries.Point coordinate;
        //string id;
        double markerforsmallradius;
        double markerforlargeradius;
        double markerforalpha;
        string navaidname;
        string typeofnavigation;

        public string NavaidName
        {
            get { return navaidname; }
            set { navaidname = value; }
        }
        public string TypeOfNavigation
        {
            get { return typeofnavigation; }
            set { typeofnavigation = value; }
        }
        public double HeightDistance
        {
            get { return heightdistance; }
            set { heightdistance = value; }
        }
        public Aran.Geometries.Point Coordinate
        {
            get { return coordinate; }
            set { coordinate = value; }
        }
        public double MarkerForSmallRadius
        {
            get { return markerforsmallradius; }
            set { markerforsmallradius = value; }
        }
        public double MarkerForLargeRadius
        {
            get
            {
                return markerforlargeradius;
            }
            set
            {
                markerforlargeradius = value;
            }
        }
        public double MarkerForAlpha
        {
            get
            {
                return markerforalpha;
            }
            set
            {
                markerforalpha = value;
            }
        }
    }
    public class ParameterForNDB
    {
        double heightdistance;
        Aran.Geometries.Point coordinate;
        string navaidname;
        string typeofnavigation;
        double ndbforsmallradius;
        double ndbforlargeradius;
        double ndbforalpha;

        public double HeightDistance
        {
            get { return heightdistance; }
            set { heightdistance = value; }
        }
        public Aran.Geometries.Point Coordinate
        {
            get { return coordinate; }
            set { coordinate = value; }
        }
        public string NavaidName
        {
            get { return navaidname; }
            set { navaidname = value; }
        }
        public string TypeofNavigation
        {
            get { return typeofnavigation; }
            set { typeofnavigation = value; }
        }
        public double NDBForSmallRadius
        {
            get { return ndbforsmallradius; }
            set { ndbforsmallradius = value; }
        }
        public double NDBForLargeRadius
        {
            get
            {
                return ndbforlargeradius;
            }
            set
            {
                ndbforlargeradius = value;
            }
        }
        public double NDBForAlpha
        {
            get
            {
                return ndbforalpha;
            }
            set
            {
                ndbforalpha = value;
            }
        }
    }
    //CoordinateLists  }   
    //Calculate_Parameters_FOR_2DGraphic {
    public class CoordinateListsForGP_OR_LOC_DME
    {
        public double heightdistance;
        public Aran.Geometries.Point coordinate;
        public string id;

        public double HeightDistance
        {
            get { return heightdistance; }
            set { heightdistance = value; }
        }
        public Aran.Geometries.Point Coordinate
        {
            get { return coordinate; }
            set { coordinate = value; }
        }
        public string ID
        {
            get { return id; }
            set { id = value; }
        }
    }
    //Calculate_Parameters_FOR_2DGraphic }
    public class AddCheckedNavaids
    {
        string secondparentname;
        string childname;
        string firstparentname;
        public string ChildName
        {
            get { return childname; }
            set { childname = value; }
        }
        public string SecondParentName
        {
            get { return secondparentname; }
            set { secondparentname = value; }
        }
        public string FirstParentName
        {
            get { return firstparentname; }
            set { firstparentname = value; }
        }
    }
}
