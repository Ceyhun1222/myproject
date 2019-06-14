using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Aran.Aim.Features;
using Aran.Converters;
using Aran.Geometries;
using Aran.Aim;
using GeoAPI.Geometries;
using Aran.Converters.ConverterJtsGeom;

namespace Aran.Panda.VisualManoeuvring
{
    public class VM_VerticalStructure
    {
        private VerticalStructure verticalStructure;
        public VerticalStructure VerticalStructure
        {
            get { return verticalStructure; }
            private set
            {
                verticalStructure = value;
                int i = -1;
                foreach (VerticalStructurePart vsPart in VerticalStructure.Part)
                {
                    if (vsPart.HorizontalProjection == null)
                        continue;
                    i++;
                    if (vsPart.HorizontalProjection.Choice == Aim.VerticalStructurePartGeometryChoice.ElevatedPoint)
                    {
                        if (VMManager.Instance.GeomOper.CurrentGeometry != null && VMManager.Instance.GeomOper.Disjoint(PartGeoPrjList[i]/*GlobalVars.pspatialReferenceOperation.ToPrj(vsPart.HorizontalProjection.Location.Geo)*/))
                            continue;
                        PartElevations.Add(ConverterToSI.Convert(vsPart.HorizontalProjection.Location.Elevation, 0));
                        PartGeometries.Add(PartGeoPrjList[i]/*GlobalVars.pspatialReferenceOperation.ToPrj(vsPart.HorizontalProjection.Location.Geo)*/);
                    }
                    else if (vsPart.HorizontalProjection.Choice == Aim.VerticalStructurePartGeometryChoice.ElevatedCurve)
                    {
                        if (VMManager.Instance.GeomOper.CurrentGeometry != null && VMManager.Instance.GeomOper.Disjoint(PartGeoPrjList[i]/*GlobalVars.pspatialReferenceOperation.ToPrj(vsPart.HorizontalProjection.LinearExtent.Geo)*/))
                            continue;
                        PartElevations.Add(ConverterToSI.Convert(vsPart.HorizontalProjection.LinearExtent.Elevation, 0));
                        PartGeometries.Add(PartGeoPrjList[i]/*GlobalVars.pspatialReferenceOperation.ToPrj(vsPart.HorizontalProjection.LinearExtent.Geo)*/);
                    }
                    else
                    {
                        if (VMManager.Instance.GeomOper.CurrentGeometry != null && VMManager.Instance.GeomOper.Disjoint(PartGeoPrjList[i]/*GlobalVars.pspatialReferenceOperation.ToPrj(vsPart.HorizontalProjection.SurfaceExtent.Geo)*/))
                            continue;
                        PartElevations.Add(ConverterToSI.Convert(vsPart.HorizontalProjection.SurfaceExtent.Elevation, 0));
                        PartGeometries.Add(PartGeoPrjList[i]/*GlobalVars.pspatialReferenceOperation.ToPrj(vsPart.HorizontalProjection.SurfaceExtent.Geo)*/);
                    }

                    if (Elevation < PartElevations[PartElevations.Count - 1])
                        Elevation = PartElevations[PartElevations.Count - 1];
                }
            }
        }
        public List<double> PartElevations { get; private set; }
        public List<Geometries.Geometry> PartGeometries { get; private set; }
        public double Elevation { get; set; }
        public VerticalStructurePartGeometryChoice choice;
        public List<Geometry> PartGeoPrjList { get; private set; }
        public List<IGeometry> PartTopoGeoPrjList { get; private set; }
        public int StepNumber { get; set; }

        public VM_VerticalStructure(VerticalStructure vs, List<Geometry> partGeoPrjList, int stepNumber = 0)
        {
            StepNumber = stepNumber;
            PartElevations = new List<double>();
            PartGeometries = new List<Geometries.Geometry>();
            this.PartGeoPrjList = partGeoPrjList;
            if (this.PartGeoPrjList == null)
            {
                this.PartGeoPrjList = new List<Geometry>();
                foreach (var vsPart in vs.Part)
                {
                    if (vsPart.HorizontalProjection == null)
                        continue;

                    if (vsPart.HorizontalProjection.Choice == Aim.VerticalStructurePartGeometryChoice.ElevatedPoint)
                    {
                        this.PartGeoPrjList.Add(GlobalVars.pspatialReferenceOperation.ToPrj(vsPart.HorizontalProjection.Location.Geo));                        
                    }
                    else if (vsPart.HorizontalProjection.Choice == Aim.VerticalStructurePartGeometryChoice.ElevatedCurve)
                    {
                        this.PartGeoPrjList.Add(GlobalVars.pspatialReferenceOperation.ToPrj(vsPart.HorizontalProjection.LinearExtent.Geo));
                    }
                    else
                    {
                        this.PartGeoPrjList.Add(GlobalVars.pspatialReferenceOperation.ToPrj(vsPart.HorizontalProjection.SurfaceExtent.Geo));
                    }
                    //this.PartTopoGeoPrjList.Add(ConvertToJtsGeo.FromGeometry(this.PartGeoPrjList[this.PartGeoPrjList.Count - 1]));
                }
            }
            this.VerticalStructure = vs;
        }
    }
}
