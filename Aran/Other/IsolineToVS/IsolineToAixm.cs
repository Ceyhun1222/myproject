using Aran.Aim.AixmMessage;
using ESRI.ArcGIS.ArcMapUI;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IsolineToVS
{
    class IsolineToAixm : IWin32Window, IFormListener
    {
        private IntPtr _handle = IntPtr.Zero;

        public IsolineToAixm()
        {

        }

        public IntPtr Handle
        {
            get
            {
                if (_handle == IntPtr.Zero)
                    _handle = new IntPtr(ArcMap.Application.hWnd);
                return _handle;
            }
        }

        private IMap Map
        {
            get
            {
                var mxDoc = ArcMap.Application.Document as IMxDocument;
                return mxDoc?.FocusMap;
            }
        }

        public void Open()
        {
            var form = new IsolineToVSForm(this);
            form.Show(this);
        }

        private IFeatureLayer GetFeatureLayer(string layerName)
        {
            for(var i = 0; i < Map.LayerCount; i++)
            {
                var fLayer = Map.Layer[i] as IFeatureLayer;
                if (fLayer != null && fLayer.Name == layerName)
                    return fLayer;
            }
            return null;
        }
    
        IEnumerable<string> IFormListener.GetLayerNames()
        {
            var layerCount = Map.LayerCount;
            for(var i = 0; i < layerCount; i++)
            {
                var featLayer = Map.Layer[i] as IFeatureLayer;
                if (featLayer != null)
                    yield return featLayer.Name;
            }
        }

        string IFormListener.GenerateMessage(
            string layerName, 
            string fileName, 
            double horizontalAccuracy, 
            bool write3DCoordinateIfExists, 
            out bool isOk)
        {
            isOk = false;

            var layer = GetFeatureLayer(layerName);
            if (layer == null)
                return "Layer not found";

            var aixmMessage = new AixmBasicMessageForward(MessageReceiverType.Eurocontrol, SrsNameType.CRS84);
            aixmMessage.Write3DCoordinateIfExists = write3DCoordinateIfExists;
            var hasMember = GetHasMemberList(layer, horizontalAccuracy);
            aixmMessage.SetHasMember(hasMember);

            var xmlWriter = System.Xml.XmlWriter.Create(fileName);
            aixmMessage.WriteXml(xmlWriter);
            xmlWriter.Close();

            isOk = true;
            return $"AIXM Message created, feature count: {aixmMessage.WriteCount}, file {fileName}";
        }

        private IEnumerable<AixmFeatureList> GetHasMemberList(IFeatureLayer layer, double horizontalAccuracy)
        {
            var featClass = layer.FeatureClass;
            var featCursor = featClass.Search(null, false);
            IFeature feat = null;
            var contourFieldIndex = featClass.FindField("Contour");
            var dateTimeNow = DateTime.Now;

            var vsCreator = new VSConverter
            {
                DefaultTimeSlice = new Aran.Aim.DataTypes.TimeSlice
                {
                    SequenceNumber = 1,
                    CorrectionNumber = 0,
                    Interpretation = Aran.Aim.Enums.TimeSliceInterpretationType.BASELINE,
                    FeatureLifetime = new Aran.Aim.DataTypes.TimePeriod(dateTimeNow),
                    ValidTime = new Aran.Aim.DataTypes.TimePeriod(dateTimeNow)
                },
                HorizontalAccuracy = new Aran.Aim.DataTypes.ValDistance(horizontalAccuracy, Aran.Aim.Enums.UomDistance.M)
            };

            var n = 1;
            while ((feat = featCursor.NextFeature()) != null)
            {
                var esriPolyline = feat.Shape as IPolyline;
                var aranPolyline = Aran.Converters.ConvertFromEsriGeom.ToPolyline(esriPolyline, true);
                var contour = (double)feat.Value[contourFieldIndex];

                var vs = vsCreator.CreateVs(n++, contour, aranPolyline, contour);
                yield return new AixmFeatureList { vs };
            }
        }
    }
}
