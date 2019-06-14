using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICAO015.Draw_Remove_Lists
{
    public class Draw_RemoveMarker_NDB_DMEN_DVOR_CVORList
    {
        string parentnode;
        string childnode;
        int smallradius;
        int middleradius;
        int largeradius;


        public string ParentNode
        {
            get { return parentnode; }
            set { parentnode = value; }
        }
        public string ChildNode
        {
            get { return childnode; }
            set { childnode = value; }
        }

        public int SmallRadius
        {
            get { return smallradius; }
            set { smallradius = value; }
        }
        public int MiddleRadius
        {
            get { return middleradius; }
            set { middleradius = value; }
        }
        public int LargeRadius
        {
            get { return largeradius; }
            set { largeradius = value; }
        }
    }
    public class Draw_Remove_ILS_List
    {
        string parentnode;
        string childnode;
        int square;
        int firstcornerpolygon;
        int secondcornerpolygon;
        int segmentpolygon;


        public string ParentNode
        {
            get { return parentnode; }
            set { parentnode = value; }
        }
        public string ChildNode
        {
            get { return childnode; }
            set { childnode = value; }
        }
        public int Square
        {
            get { return square; }
            set { square = value; }
        }
        public int FirstCornerPolygon
        {

            get { return firstcornerpolygon; }
            set { firstcornerpolygon = value; }
        }
        public int SecondCornerPolygon
        {
            get { return secondcornerpolygon; }
            set { secondcornerpolygon = value; }
        }
        public int SegmentPolygon
        {
            get { return segmentpolygon; }
            set { segmentpolygon = value; }
        }
    }
}
