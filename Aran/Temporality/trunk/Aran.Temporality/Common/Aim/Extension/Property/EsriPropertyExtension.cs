using System;
using ESRI.ArcGIS.Geometry;

namespace Aran.Temporality.Common.Aim.Extension.Property
{
    [Serializable]
    public class EsriPropertyExtension : PropertyExtension
    {
        public int[] PropertyPath { get; set; }
        public byte[] EsriData { get; set; }

        [NonSerialized] 
        public IGeometry EsriObject;
    }
}
