using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aran.Geometries;

namespace ICAO015
{
    public class Obstacle_ParamListPolygons
    {
        Point point;
        double nav_height;
        string parentnodetxt;
        string childnodetxt;
        double a;
        double b;
        double segment_poly_h;
        double radius;
        double corners_polygon_H;
        double l;
        double f;
        double delta_l;
        Polygon first_corner_polygon;
        Polygon second_corner_polygon;
        Polygon segment_polygon;
        MultiLineString first_cornerpolygon_line;
        MultiLineString second_cornerpolygon_line;
        MultiLineString segmentpolygon_line;

        public Point Navaid_Point
        {
            get { return point; }
            set { point = value; }
        }
        public double Navaid_Height
        {
            get { return nav_height; }
            set { nav_height = value; }
        }
        public string Parent_NodeTxt
        {
            get { return parentnodetxt; }
            set { parentnodetxt = value; }
        }
        public string Child_NodeTxt
        {
            get { return childnodetxt; }
            set { childnodetxt = value; }
        }
        public double A
        {
            get { return a; }
            set { a = value; }
        }
        public double B
        {
            get { return b; }
            set { b = value; }
        }
        public double Segment_Polygon_h
        {
            get { return segment_poly_h; }
            set { segment_poly_h = value; }
        }
        public double Radius
        {
            get { return radius; }
            set { radius = value; }
        }
        public double Corners_Polygon_H
        {
            get { return corners_polygon_H; }
            set { corners_polygon_H = value; }
        }
        public double F
        {
            get { return f; }
            set { f = value; }
        }
        public double Delta_L
        {
            get { return delta_l; }
            set { delta_l = value; }
        }
        public double L
        {
            get { return l; }
            set { l = value; }
        }
        public Polygon First_Corner_Polygon
        {
            get { return first_corner_polygon; }
            set { first_corner_polygon = value; }
        }
        public Polygon Second_Corner_Polygon
        {
            get { return second_corner_polygon; }
            set { second_corner_polygon = value; }
        }
        public Polygon Segment_Polygon
        {
            get { return segment_polygon; }
            set { segment_polygon = value; }
        }
        public MultiLineString FirstCornerPoly_Line
        {
            get { return first_cornerpolygon_line; }
            set { first_cornerpolygon_line = value; }
        }
        public MultiLineString SecondCornerPoly_Line
        {
            get { return second_cornerpolygon_line; }
            set { second_cornerpolygon_line = value; }
        }
        public MultiLineString SegmentPoly_Line
        {
            get { return segmentpolygon_line; }
            set { segmentpolygon_line = value; }
        }
    }
}
