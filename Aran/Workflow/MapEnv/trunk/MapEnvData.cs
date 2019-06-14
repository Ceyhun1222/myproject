using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aran.Package;
using System.IO;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.esriSystem;
using Aran.Aim.Data;
using Aran.Geometries;
using ESRI.ArcGIS.Carto;
using Aran.Controls;
using MapEnv.Toc;
using Aran.AranEnvironment;
using MapEnv.Layers;

namespace MapEnv
{
    public class MapEnvData
    {
        public MapEnvData ()
        {
            _pluginDataDict = new Dictionary<string, byte []> ();
            _mapEnvelope = new Box ();
            AranPlugins = new List<Guid>();
            EffectiveDate = DateTime.Now;

            RasterPathList = new List<string> ();
            EsriLayers = new List<ILayer> ();

            _layerInfoList = new List<MapEnvLayerInfo> ();

            SetLastVersion ();
        }

        public List<MapEnvLayerInfo> LayerInfoList
        {
            get { return _layerInfoList; }
        }

        public List<string> RasterPathList { get; private set; }

        public List<ILayer> EsriLayers { get; private set; }

        public ISpatialReference MapSpatialReference
        {
            get { return _mapSpatialReference; }
            set { _mapSpatialReference = value; }
        }

        public Dictionary<string, byte []> PluginDataDict
        {
            get { return _pluginDataDict; }
        }

        public List<Guid> AranPlugins {get; private set; }

        public DateTime EffectiveDate { get; set; }

        public Box MapEnvelope
        {
            get { return _mapEnvelope; }
        }
        
        public AiracDateTime AiracDate
        {
            get { return _airacDate; }
            set { _airacDate = value; }
        }

        
        public void Save(string fileName)
        {
            var ms = new MemoryStream();
            var writer = new BinaryPackageWriter(ms);
            bool notNull;

            SetLastVersion();

            writer.PutInt32(DataVersion);

            #region Connection
            notNull = (_connection != null);
            writer.PutBool(notNull);
            if (notNull) {
                _connection.DataVersion = DataVersion;
                _connection.Pack(writer);
            }
            #endregion

            #region ThumbnailImageBytes

            notNull = (ThumbnailImageBytes != null);
            writer.PutBool(notNull);
            if (notNull) {
                writer.PutInt32(ThumbnailImageBytes.Length);
                foreach (var bt in ThumbnailImageBytes)
                    writer.PutByte(bt);
            }

            #endregion

            #region MapSpatialReference

            IPersistStream perStream = _mapSpatialReference as IPersistStream;
            notNull = (perStream != null);
            writer.PutBool(notNull);
            if (notNull)
                perStream.Pack(writer);

            #endregion

            #region Layers

            writer.PutInt32(_layerInfoList.Count);
            foreach (MapEnvLayerInfo layerInfo in _layerInfoList) {
                layerInfo.Pack(writer);
            }

            #endregion

            #region PluginData
            writer.PutInt32(_pluginDataDict.Count);
            foreach (string pluginDataKey in _pluginDataDict.Keys) {
                writer.PutString(pluginDataKey);
                byte[] ba = _pluginDataDict[pluginDataKey];
                writer.PutInt32(ba.Length);
                for (int i = 0; i < ba.Length; i++)
                    writer.PutByte(ba[i]);
            }
            #endregion

            #region Enabled AranPlugins

            writer.PutInt32(AranPlugins.Count);
            foreach (var pluginGuid in AranPlugins)
                writer.PutString(pluginGuid.ToString());

            #endregion

            writer.PutDateTime(EffectiveDate);

            _mapEnvelope.Pack(writer);

            _airacDate.Pack(writer);

            ms.Seek(0, SeekOrigin.Begin);
            var fileStream = new System.IO.FileStream(fileName, FileMode.Create);
            byte[] buffer = ms.ToArray();
            fileStream.Write(buffer, 0, buffer.Length);
            fileStream.Close();
            writer.Dispose();
            ms.Close();
        }

        public void Load(string fileName)
        {
            var fileStream = new System.IO.FileStream(fileName, FileMode.Open);
            var buffer = new byte[fileStream.Length];
            fileStream.Read(buffer, 0, buffer.Length);
            fileStream.Close();

            var reader = new BinaryPackageReader(buffer);
            bool notNull;
            int count;

            DataVersion = reader.GetInt32();
            if (DataVersion < 3300)
                throw new Exception("Old project file does not supported!");

            #region Connection
            notNull = reader.GetBool();
            if (notNull) {
                _connection = new Connection();
                _connection.DataVersion = DataVersion;
                _connection.Unpack(reader);
            }
            #endregion

            #region ThumbnailImageBytes

            notNull = reader.GetBool();
            if (notNull) {
                count = reader.GetInt32();
                ThumbnailImageBytes = new byte[count];
                for (int i = 0; i < count; i++)
                    ThumbnailImageBytes[i] = reader.GetByte();
            }

            #endregion

            #region MapSpatialReference

            notNull = reader.GetBool();
            if (notNull)
                _mapSpatialReference = LayerPackage.UnpackPersistStream(reader) as ISpatialReference;

            #endregion

            #region Layers

            count = reader.GetInt32();
            _layerInfoList.Clear();
            for (int i = 0; i < count; i++) {
                var layerInfo = new MapEnvLayerInfo();
                layerInfo.Unpack(reader);
                _layerInfoList.Add(layerInfo);
            }

            #endregion

            #region PluginData

            var pluginDataDictCount = reader.GetInt32();
            for (int i = 0; i < pluginDataDictCount; i++) {
                string pluginDataKey = reader.GetString();
                int byteArrayCount = reader.GetInt32();
                byte[] ba = new byte[byteArrayCount];
                for (int j = 0; j < byteArrayCount; j++)
                    ba[j] = reader.GetByte();

                _pluginDataDict.Add(pluginDataKey, ba);
            }

            #endregion

            #region Enabled AranPlugins

            AranPlugins.Clear();
            var pluginCount = reader.GetInt32();
            for (int i = 0; i < pluginCount; i++)
                AranPlugins.Add(new Guid(reader.GetString()));

            #endregion

            EffectiveDate = reader.GetDateTime();

            _mapEnvelope.Unpack(reader);

            _airacDate.Unpack(reader);

            reader.Dispose();
        }

        public static bool GetConnectionAndThumbnail(string fileName, out Connection conn, out byte[] thumbnailImageBytes)
        {
            var fileStream = new System.IO.FileStream(fileName, FileMode.Open);
            var buffer = new byte[fileStream.Length];
            fileStream.Read(buffer, 0, buffer.Length);
            fileStream.Close();

            var reader = new BinaryPackageReader(buffer);
            bool notNull;
            int count;

            conn = null;
            thumbnailImageBytes = null;

            var dataVersion = reader.GetInt32();
            if (dataVersion < 3300)
                return false;

            #region Connection
            notNull = reader.GetBool();
            if (notNull) {
                conn = new Connection();
                conn.DataVersion = dataVersion;
                conn.Unpack(reader);
            }
            #endregion

            #region ThumbnailImageBytes

            notNull = reader.GetBool();
            if (notNull) {
                count = reader.GetInt32();
                thumbnailImageBytes = new byte[count];
                for (int i = 0; i < count; i++)
                    thumbnailImageBytes[i] = reader.GetByte();
            }

            #endregion

            return true;
        }

        public int DataVersion { get; private set; }

        public byte[] ThumbnailImageBytes { get; set; }

        internal Connection Connection
        {
            get { return _connection; }
            set { _connection = value; }
        }

		internal string SelectedAirport
		{
			get;
			set;
		}

        private void SetLastVersion ()
        {
            #region Version
            //--- 2 Added RasterLayer
            //--- 2.1 Added QueryLayer
            //--- 2.2 Added EsriLayer Support
			//--- 2.3 Connection doesn't save password
			//--- 2.4 SelectedAirportHeliport is saved
			//--- 2.5 SelectedAirportHeliport can be empty
            //--- 3.0 ChangedLayerType

			//--- Older versions does not supported more.

			//--- 3.1 Password Saved.
            ///--- 3.2
            ///         * Removed AimTable (Old version of AimFeatureLayer)
            ///         * CreateNewProject Changed. Enabled Plugins saved.
            ///         * EffectiveDate saved.
            ///         * Thumbnail Image saved.

            ///--- 3.2.1
            ///         * EffectiveDate replaced to AiracDate
            
            ///--- 3.3
            ///         * CurrentAirport (for KAZ) removed.
            ///         * Thumbnail Image save before to reduse load speed.


            #endregion

            DataVersion = 3300;
        }

        private ISpatialReference _mapSpatialReference;
        private Connection _connection;
        private Dictionary<string, byte []> _pluginDataDict;
        private Box _mapEnvelope;
        private List<MapEnvLayerInfo> _layerInfoList;
        private AiracDateTime _airacDate;
    }

    public class MapEnvLayerInfo : IPackable
    {
        public MapEnvLayerInfo ()
        {

        }

        public MapEnvLayerInfo (ILayer layer)
        {
            SetLayer (layer);
        }

        public void SetLayer (ILayer layer)
        {
			if (layer is AimFeatureLayer)
			{
				LayerType = MapEnvLayerType.SimpleShapefile;
				MyFeatureLayer = layer as AimFeatureLayer;
			}
			else
			{
				PersistStream = layer as IPersistStream;
			}
        }

        public MapEnvLayerType LayerType { get; private set; }

		public AimFeatureLayer MyFeatureLayer { get; private set; }

        public IPersistStream PersistStream
        {
            get { return _persistStream; }
            set
            {
                _persistStream = value;
                LayerType = MapEnvLayerType.Esri;
            }
        }

        public void Pack (PackageWriter writer)
        {
            writer.PutEnum<MapEnvLayerType> (LayerType);

			if (LayerType == MapEnvLayerType.SimpleShapefile)
				(MyFeatureLayer as IPackable).Pack (writer);
			else if (LayerType == MapEnvLayerType.Esri)
				PersistStream.Pack (writer);
        }

        public void Unpack (PackageReader reader)
        {
            LayerType = reader.GetEnum<MapEnvLayerType> ();

			if (LayerType == MapEnvLayerType.SimpleShapefile)
			{
				MyFeatureLayer = new AimFeatureLayer ();
				(MyFeatureLayer as IPackable).Unpack (reader);
			}
			else if (LayerType == MapEnvLayerType.Esri)
			{
				_persistStream = LayerPackage.UnpackPersistStream (reader);
			}
        }

        private IPersistStream _persistStream;
    }

    public enum MapEnvLayerType
    {
        SimpleShapefile,
        ComplexShapefile,
        Esri
    }
}
