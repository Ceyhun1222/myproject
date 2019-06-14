using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aran.Package;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.esriSystem;
using MapEnv.Layers;

namespace Aran.Delta.Settings.model
{
    public class SymbolModel : IPackable
    {
        public SymbolModel()
        {
            PointSymbol = Globals.CreateDefaultSymbol(ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryPoint);
            LineCourseSymbol = Globals.CreateDefaultSymbol(ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryPolyline);
            LineDistanceSymbol = Globals.CreateDefaultSymbol(ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryPolyline);
            TextSymbol = Globals.CreateDefaultTextSymbol() as ISymbol;
            ResultPointSymbol = Globals.CreateDefaultSymbol(ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryPoint);
            BufferSymbol = Globals.CreateDefaultSymbol(ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryPolygon);
        }
        public ISymbol PointSymbol { get; set; }
        public ISymbol LineCourseSymbol { get; set; }
        public ISymbol LineDistanceSymbol { get; set; }
        public ISymbol TextSymbol { get; set; }
        public ISymbol ResultPointSymbol { get; set; }
        public ISymbol BufferSymbol { get; set; }

        public void Pack(PackageWriter writer)
        {
            IPersistStream perStream = PointSymbol as IPersistStream;
            perStream.Pack(writer);

            perStream = LineCourseSymbol as IPersistStream;
            perStream.Pack(writer);

            perStream = LineDistanceSymbol as IPersistStream;
            perStream.Pack(writer);

            perStream = TextSymbol as IPersistStream;
            perStream.Pack(writer);

            perStream = ResultPointSymbol as IPersistStream;
            perStream.Pack(writer);

            perStream = BufferSymbol as IPersistStream;
            perStream.Pack(writer);
        }

        public void Unpack(PackageReader reader)
        {
            try
            {
                PointSymbol = LayerPackage.UnpackPersistStream(reader) as ISymbol;
                LineCourseSymbol = LayerPackage.UnpackPersistStream(reader) as ISymbol;
                LineDistanceSymbol = LayerPackage.UnpackPersistStream(reader) as ISymbol;
                TextSymbol = LayerPackage.UnpackPersistStream(reader) as ISymbol;
                ResultPointSymbol = LayerPackage.UnpackPersistStream(reader) as ISymbol;
                BufferSymbol = LayerPackage.UnpackPersistStream(reader) as ISymbol;
            }
            catch (Exception)
            {

            }
        }
    }
}
