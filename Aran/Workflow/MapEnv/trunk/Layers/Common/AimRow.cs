using System;
using System.Collections.Generic;
using System.Text;
using ESRI.ArcGIS.Geometry;
using Aran.Aim.Features;
using ESRI.ArcGIS.esriSystem;
using Aran.Package;

namespace MapEnv.Layers
{
    public class AimRow
    {
        public AimRow()
        {
            RowShapeList = new List<AimRowShape>();
            IsVisible = true;
            IsSelected = false;
        }

        public Feature AimFeature { get; set; }
        public List<AimRowShape> RowShapeList { get; private set; }
        public bool IsVisible { get; set; }
        public bool IsSelected { get; set; }


        public void ToProject(ISpatialReference mapSpatialReference)
        {
            foreach (AimRowShape rowShape in RowShapeList) {
                foreach (ShapePair geoPair in rowShape.Shapes) {
                    geoPair.Prj = geoPair.Geo.Clone();
                    geoPair.Prj.Project(mapSpatialReference);
                }
            }
        }
    }

    public class AimRowShape
    {
        public AimRowShape()
        {
            Shapes = new List<ShapePair>();
        }

        public List<ShapePair> Shapes { get; private set; }
        public string Text { get; set; }
        public string SymbolValue { get; set; }
    }

    public class ShapePair
    {
        public IGeometry Geo { get; set; }
        public IGeometry Prj { get; set; }
    }
}
