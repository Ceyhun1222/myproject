using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Aran.Aim.Features;
using Aran.Geometries;
using Aran.Panda.Common;
using Newtonsoft.Json.Linq;

namespace Aran.Panda.VisualManoeuvring
{
    public class VM_TrackSegment
    {
        public VM_TrackSegment() { }

        public string Name { get; set; }
        public double Length { get; set; }
        public string Description { get; set; }
        public LineString Centreline { get; set; }
        public MultiPolygon BufferPoly { get; set; }
        public double FlightAltitude { get; set; }
        public double FlightHeight { get; set; }
        public bool IsFinalSegment { get; set; }
        public Point StartPointPrj { get; set; }
        public double InitialDirectionDir { get; set; }
        public double DistanceTillDivergence { get; set; }
        public TurnDirection DivergenceSide { get; set; }
        public double IntermediateDirectionDir { get; set; }
        public double DistanceTillConvergence { get; set; }
        public TurnDirection ConvergenceSide { get; set; }
        public double FinalDirectionDir { get; set; }
        public double DistanceTillEndPoint { get; set; }
        public Point EndPointPrj { get; set; }

        //For GUI
        public double Length_GUI { get; set; }
        public double FlightAltitude_GUI { get; set; }
        public double FlightHeight_GUI { get; set; }
        public double InitialDirectionAzt { get; set; }
        public double IntermediateDirectionAzt { get; set; }
        public double FinalDirectionAzt { get; set; }

        public JObject ToJson()
        {
            if (StartPointPrj == null)
                return null;

            if (EndPointPrj == null)
                return null;

            return new JObject(
                new JProperty("name", Name),
                new JProperty("length", Length),
                new JProperty("description", Description),                
                new JProperty("start", new JObject(
                    new JProperty("x", StartPointPrj.X),
                    new JProperty("y", StartPointPrj.Y))),
                new JProperty("initialDirectionDir", InitialDirectionDir),
                new JProperty("distanceTillDivergence", DistanceTillDivergence),
                new JProperty("divergenceSide", (int) DivergenceSide),
                new JProperty("intermediateDirectionDir", IntermediateDirectionDir),
                new JProperty("distanceTillConvergence", DistanceTillConvergence),
                new JProperty("convergenceSide", (int) ConvergenceSide),
                new JProperty("finalDirectionDir", FinalDirectionDir),
                new JProperty("distanceTillEndPoint", DistanceTillEndPoint),
                new JProperty("end", new JObject(
                    new JProperty("x", EndPointPrj.X),
                    new JProperty("y", EndPointPrj.Y))),
                new JProperty("isFinalSegmentStep", IsFinalSegment)
            );

        }

        public void FromJson(JObject jo)
        {
            Name = jo["name"].Value<string>();
            Length = jo["length"].Value<double>();
            Description = jo["description"].Value<string>();
            var startJO = jo["start"].Value<JObject>();
            StartPointPrj = new Point(startJO["x"].Value<double>(), startJO["y"].Value<double>());
            InitialDirectionDir = jo["initialDirectionDir"].Value<double>();
            DistanceTillDivergence = jo["distanceTillDivergence"].Value<double>();
            DivergenceSide = (TurnDirection) jo["divergenceSide"].Value<int>();
            IntermediateDirectionDir = jo["intermediateDirectionDir"].Value<double>();
            DistanceTillConvergence = jo["distanceTillConvergence"].Value<double>();
            ConvergenceSide = (TurnDirection) jo["convergenceSide"].Value<int>();
            FinalDirectionDir = jo["finalDirectionDir"].Value<double>();
            DistanceTillEndPoint = jo["distanceTillEndPoint"].Value<double>();
            var endJO = jo["end"].Value<JObject>();
            EndPointPrj = new Point(endJO["x"].Value<double>(), endJO["y"].Value<double>());
            FinalDirectionDir = jo["finalDirectionDir"].Value<double>();
            IsFinalSegment = jo["isFinalSegmentStep"].Value<bool>();

            Length_GUI = GlobalVars.unitConverter.DistanceToDisplayUnits(Length, eRoundMode.NERAEST);
            InitialDirectionAzt = Math.Round(GlobalVars.pspatialReferenceOperation.DirToAztPrj(StartPointPrj, InitialDirectionDir));
            IntermediateDirectionAzt = Math.Round(GlobalVars.pspatialReferenceOperation.DirToAztPrj(StartPointPrj, IntermediateDirectionDir));
            FinalDirectionAzt = Math.Round(GlobalVars.pspatialReferenceOperation.DirToAztPrj(EndPointPrj, FinalDirectionDir));            
        }
    }
}
