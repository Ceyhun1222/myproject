using Aran.PANDA.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ag = Aran.Geometries;

namespace Aran.PANDA.Vss
{
    public class MapLocation
    {
        private Ag.Point _geo;

        public MapLocation()
        {

        }

        public MapLocation(Ag.Point geo)
        {
            Geo = geo;
        }

        public Ag.Point Geo
        {
            get { return _geo; }
            set
            {
                _geo = value;
                Prj = Globals.ToProject(value) as Ag.Point;
            }
        }

        public Ag.Point Prj { get; private set; }
    }

    public class MapLine
    {
        public MapLine()
        {

        }

        public MapLine(MapLocation start, MapLocation end)
        {
            Start = start;
            End = end;
        }

        public MapLocation Start { get; set; }

        public MapLocation End { get; set; }

        public Ag.LineString ToPrjLineString()
        {
            if (Start == null || End == null)
                return null;

            var line = new Ag.LineString();
            line.Add(Start.Prj);
            line.Add(End.Prj);
            return line;
        }

        public Ag.Line ToPrjLine()
        {
            if (Start == null || End == null)
                return null;

            return new Ag.Line(Start.Prj, End.Prj);
        }

        public double GetAngleInRad()
        {
            return ARANFunctions.ReturnAngleInRadians(Start.Prj, End.Prj);
        }
    }

    public class PrjLine
    {
        public Ag.Point Start { get; set; }

        public Ag.Point End { get; set; }

        public Ag.LineString ToLineString()
        {
            if (Start == null || End == null)
                return null;

            var line = new Ag.LineString();
            line.Add(Start);
            line.Add(End);
            return line;
        }

        public Ag.Line ToLine()
        {
            if (Start == null || End == null)
                return null;

            return new Ag.Line(Start, End);
        }

        public double GetAngleInRad()
        {
            return ARANFunctions.ReturnAngleInRadians(Start, End);
        }
    }
}
