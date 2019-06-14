using Aran.Aim.DataTypes;
using Aran.Aim.Enums;
using Aran.Aim.Features;
using Aran.Geometries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IsolineToVS
{
    class VSConverter
    {
        public TimeSlice DefaultTimeSlice { get; set; }

        public ValDistance HorizontalAccuracy { get; set; }

        public VerticalStructure CreateVs(int n, double contour, MultiLineString line, double z)
        {
            var vs = new VerticalStructure
            {
                Identifier = Guid.NewGuid(),
                Name = "CONTOUR_" + n.ToString("00000"),
                TimeSlice = DefaultTimeSlice
            };

            foreach (var item in line)
            {
                foreach (Aran.Geometries.Point pt in item)
                {
                    pt.Z = z;
                }
            }

            var le = new ElevatedCurve
            {
                Elevation = new ValDistanceVertical(contour, UomDistanceVertical.M),
                HorizontalAccuracy = this.HorizontalAccuracy
            };

            le.Geo.Assign(line);


            vs.Part.Add(new VerticalStructurePart { HorizontalProjection = new VerticalStructurePartGeometry { LinearExtent = le } });

            return vs;
        }
    }
}
