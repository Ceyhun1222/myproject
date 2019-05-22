using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aran.Geometries
{
    public abstract class SpecialGeometry : Geometry
    {
        public override GeometryType Type
        {
            get
            {
                return Geometries.GeometryType.SpecialGeometry;
            }
        }

        public static SpecialGeometry Create ( SpecialGeometryType specGeomType )
        {
            SpecialGeometry result = null;
            switch ( specGeomType )
            {
                case SpecialGeometryType.Box:
                    result = new Box ();
                    break;
                case SpecialGeometryType.Circle:
                    result = new Circle ();
                    break;
                default :
                    throw new NotImplementedException ();
            }
            return result;
        }

        public abstract SpecialGeometryType SpecGeomType { get; }
    }

    public enum SpecialGeometryType
    {
        Box,
        Circle
    }
}
