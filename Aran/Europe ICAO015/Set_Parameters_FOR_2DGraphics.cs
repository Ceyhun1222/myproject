using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICAO015
{

    public class Set_Parameters_FOR_PolygonS
    {
        Aran.Geometries.MultiLineString polygon_pegment_palcforline;
        double h;
        double r;
        double a;
        double H;
        double delta_l;
        Aran.Geometries.Point line_pt_second;
        Aran.Geometries.Point line_pt_third;

        public Aran.Geometries.MultiLineString Polygon_Segment_CalcFORLine
        {
            get { return polygon_pegment_palcforline; }
            set { polygon_pegment_palcforline = value; }
        }
        public Aran.Geometries.Point Line_PT_Second
        {
            get { return line_pt_second; }
            set { line_pt_second = value; }
        }
        public Aran.Geometries.Point Line_PT_Third
        {
            get { return line_pt_third; }
            set { line_pt_third = value; }
        }
        public double h_par
        {
            get { return h; }
            set { h = value; }
        }
        public double a_par
        {
            get { return a; }
            set { a = value; }
        }
        public double r_par
        {
            get { return r; }
            set { r = value; }
        }
        public double H_par
        {
            get { return H; }
            set { H = value; }
        }
        public double Delta_L
        {
            get { return delta_l; }
            set { delta_l = value; }
        }
    }
}

