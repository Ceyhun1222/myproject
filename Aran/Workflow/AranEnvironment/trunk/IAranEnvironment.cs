using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Aran.Aim.Features;
using Aran.Package;

namespace Aran.AranEnvironment
{
    public interface IAranEnvironment
    {
        event EventHandler MapSpatialReferenceChanged;
		event EventHandler EffectiveDateChanged;
        object HookObject { get; }
        void SaveDocumentAs (string fileName, bool copy);
        string DocumentFileName { get; }
        IWin32Window Win32Window { get; }

        bool UseWebApi { get; }
        
        #region ExtentionData

        bool PutExtData(string key, IPackable value);
        bool GetExtData(string key, IPackable value);
        bool HasExtKey(string key);
        void RemoveExtData(string key);

        #endregion

        object DbProvider { get; }
        IAranUI AranUI { get; }
        IAranGraphics Graphics { get; }
        IAranLayoutViewGraphics LayoutGraphics { get; }
        void ShowLogs (IEnumerable<string> logs, bool clearPrev = true);
        void ShowError (string message, bool clearPrev = true);
        AranPlugin GetPlugin (string name);
        IFeatureViewer GetViewer ();
        void RefreshAllAimLayers ();
        void ShowFeatureInfo (Feature feature);
		Connection ConnectionInfo { get; }
        AiracDateTime CurrentAiracDateTime { get; }
        CommonData CommonData { get; }
        object MapControl { get; }
        IScreenCapture GetScreenCapture(string nm);
        ILogger GetLogger(string name);
        List<object> GetAimLayers { get; }
        List<object> GetAllLayers { get; }

        T ReadConfig<T>(string folder, string name, T defaultValue);
        void WriteConfig(string folder, string name, object value);
    }

    public class StringExtData : IPackable
    {
        public string Value { get; set; }

        public void Pack(PackageWriter writer)
        {
            var isNotNull = (Value != null);
            writer.PutBool(isNotNull);
            if (isNotNull)
                writer.PutString(Value);
        }

        public void Unpack(PackageReader reader)
        {
            Value = null;

            var isNotNull = reader.GetBool();
            if (isNotNull)
                Value = reader.GetString();
        }
    }
}
