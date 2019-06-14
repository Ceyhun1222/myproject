using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICAO015.TabControl
{
    public class AddToTabList_ILS
    {
        string typeofnavigation;
        string checkednavaid;
        string a;
        double b;
        double segmenth;
        string radius;
        double d;
        double cornerpolygonsH;
        double l;
        double f;

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
        public string A
        {
            get { return a; }
            set { a = value; }
        }
        public double B
        {
            get { return b; }
            set { b = value; }
        }
        public double Segmenth
        {
            get { return segmenth; }
            set { segmenth = value; }
        }
        public string Radius
        {
            get { return radius; }
            set { radius = value; }
        }
        public double D
        {
            get { return d; }
            set { d = value; }
        }
        public double CornerPolygonsH
        {
            get { return cornerpolygonsH; }
            set { cornerpolygonsH = value; }
        }
        public double L
        {
            get { return l; }
            set { l = value; }
        }
        public double F
        {
            get { return f; }
            set { f = value; }
        }
    }
}
