using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Runtime.InteropServices;
using Aran.Package;
using ESRI.ArcGIS.ArcMapUI;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Framework;
using ESRI.ArcGIS.ADF.CATIDs;
using Microsoft.Win32;

namespace Aran.Delta.Settings
{
    [Guid("5770f7c2-b0ca-4012-bff0-6057c1c1018a")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("Aran.Delta.Settings.DeltaExtension")]
    public class DeltaExtension : IExtension, IExtensionConfig, IPersistVariant
    {
      private const string Key = "Delta Extension";

        public int Height { get; set; }
        public DeltaExtension()
        {
            dictionary = new Dictionary<string, Stream> ();
            m_docEvents = null;
        }


		#region COM Registration Function(s)
		[ComRegisterFunction()]
		[ComVisible(false)]
		static void RegisterFunction(Type registerType)
		{
			// Required for ArcGIS Component Category Registrar support
			ArcGISCategoryRegistration(registerType);

			//
			// TODO: Add any COM registration code here
			//
		}

		[ComUnregisterFunction()]
		[ComVisible(false)]
		static void UnregisterFunction(Type registerType)
		{
			// Required for ArcGIS Component Category Registrar support
			ArcGISCategoryUnregistration(registerType);

			//
			// TODO: Add any COM unregistration code here
			//
		}

		#region ArcGIS Component Category Registrar generated code
		/// <summary>
		/// Required method for ArcGIS Component Category registration -
		/// Do not modify the contents of this method with the code editor.
		/// </summary>
		private static void ArcGISCategoryRegistration(Type registerType)
		{
			string regKey = string.Format("HKEY_CLASSES_ROOT\\CLSID\\{{{0}}}", registerType.GUID);
			MxExtension.Register(regKey);

		}
		/// <summary>
		/// Required method for ArcGIS Component Category unregistration -
		/// Do not modify the contents of this method with the code editor.
		/// </summary>
		private static void ArcGISCategoryUnregistration(Type registerType)
		{
			string regKey = string.Format("HKEY_CLASSES_ROOT\\CLSID\\{{{0}}}", registerType.GUID);
			MxExtension.Unregister(regKey);

		}

		#endregion
		#endregion

		#region Attribute

		/// <summary>
		/// the global unique identifier of this class, used for persistence
		/// </summary>

		//private MemoryStream ms = new MemoryStream();
		private int PersistenceVersion = 0;

		private const int persistenceVersion1 = 1; // if anything changes concerning persistence, increment this attribute
		private const int persistenceVersion2 = 2;
		private const int CurrentPersistenceVersion = persistenceVersion1;

		//private string scriptFile = "";
		//private string configFile = "";
		//private string mapFileName = "";
		//private string dateTime;
		private string UserName;

		#endregion

		#region Document events handling
        [DispId(8)]
        void OnNewDocument()
		{
            Settings = new DeltaSettings();
            Globals.Settings = Settings;
        }

		private void OnOpenDocument()
		{
            Settings = new DeltaSettings();
            Globals.Settings = Settings;
        }

		private void OnCloseDocument()
		{
            
		}

		//Wiring.
		private void SetUpDocumentEvent(IDocument myDocument)
		{
           // m_docEvents = myDocument as IDocumentEvents_Event;
           //// m_docEvents.NewDocument += delegate() { OnNewDocument(); };
           // m_docEvents.NewDocument += delegate() { OnNewDocument(); }; 
           // m_docEvents.OpenDocument += delegate() {OnOpenDocument();};
			//m_docEvents.CloseDocument += new IDocumentEvents_CloseDocumentEventHandler(OnCloseDocument);
		}

		#endregion

		#region IExtension Members

		/// <summary>
		/// Name of extension. Do not exceed 31 characters
		/// </summary>
		public string Name
		{
			get
			{
				return "Delta Extension";
			}
		}

		public void Shutdown()
		{
			m_application = null;
			m_appDocument = null;
		}

		public void Startup(ref object initializationData)
		{
			m_application = initializationData as IApplication;
			if (m_application == null)
				return;

			m_appDocument = m_application.Document as MxDocument;
			SetUpDocumentEvent(m_appDocument);

            if (Settings != null)
            {
                Settings = new DeltaSettings();
                Globals.Settings = Settings;

                UserName = Environment.UserName;
            }
		}

		#endregion

		#region IExtensionConfig Members

		public string Description
		{
			get
			{
				return "Delta (Build 00.004)\r\n" +
						"(c) R.I.S.K. 2015\r\n\r\n" +
						"Provides a complete set of tools for real-time Aeronautical analysis.";
			}
		}

		/// <summary>
		/// Friendly name shown in the Extension dialog
		/// </summary>
		public string ProductName
		{
			get
			{
				return "Delta Extension";
			}
		}

		public esriExtensionState State
		{
			get
			{
                return m_enableState;
			}
			set
			{
				if (m_enableState != 0 && value == m_enableState)
					return;

				//Check if ok to enable or disable extension
				esriExtensionState requestState = value;
				if (requestState == esriExtensionState.esriESEnabled)
				{
					//Cannot enable if it's already in unavailable state
					if (m_enableState == esriExtensionState.esriESUnavailable)
						throw new COMException("Cannot enable extension");

					//Determine if state can be changed
					esriExtensionState checkState = StateCheck(true);
					m_enableState = checkState;
				}
				else if (requestState == 0 || requestState == esriExtensionState.esriESDisabled)
				{
					//Determine if state can be changed
					esriExtensionState checkState = StateCheck(false);
					if (checkState != m_enableState)
						m_enableState = checkState;
				}
			}
		}

		#endregion

		/// <summary>
		/// Determine extension state 
		/// </summary>
		/// <param name="requestEnable">true if to enable; false to disable</param>
		private esriExtensionState StateCheck(bool requestEnable)
		{
			if (requestEnable)
				return esriExtensionState.esriESEnabled;
			else
				return esriExtensionState.esriESDisabled;
		}

		private void LoadProjectDataV1(IVariantStream Stream)
		{
			int i, count = Convert.ToInt32(Stream.Read());

			for (i = 0; i < count; i++)
			{
				string key = Convert.ToString(Stream.Read());

				int l = (int)Stream.Read();

				byte[] bArray = new byte[l];
				for (l = 0; l < bArray.Length; l++)
					bArray[l] = (byte)Stream.Read();
 
				MemoryStream ms = new MemoryStream(bArray);
                if ( dictionary.ContainsKey ( key ) )
                {
                    dictionary [key].Dispose ();
                    dictionary [key] = ms;
                }
                else
                    dictionary.Add ( key, ms );
			}
		}

		private void LoadProjectDataV2(IVariantStream Stream)
		{
			//configFile = Convert.ToString(Stream.Read());
			//scriptFile = Convert.ToString(Stream.Read());
			//mapFileName = Convert.ToString(Stream.Read());
		}

		private void SaveProjectData(IVariantStream Stream)
		{
			Stream.Write(dictionary.Count);
           // Stream.Write(5);

			foreach (KeyValuePair<string, Stream> kvp in dictionary)
			{
				Stream.Write(kvp.Key);

				byte[] bArray = new byte[kvp.Value.Length];
                kvp.Value.Position = 0;
				kvp.Value.Read(bArray, 0, bArray.Length);

				Stream.Write(bArray.Length);
				for (int i = 0; i < bArray.Length; i++)
					Stream.Write(bArray[i]);
			}
		}

		#region IPersistVariant Members

		public UID ID
		{
			get
			{
				UID typeID = new UIDClass();
                typeID.Value = "{ADEE2E73-1EDC-4860-8EB1-977E098D4A05}";
				return typeID;
			}
		}

		public void Load(IVariantStream Stream)
		{
            if (Globals.Settings == null)
                Globals.Settings = new DeltaSettings();

			int PersistenceVersion = Convert.ToInt32(Stream.Read());
            
            Globals.Stream = Stream;
            if (PersistenceVersion == 1)
            {
                int size = Convert.ToInt32(Stream.Read());
                var buffer = new byte[size];
                for (int i = 0; i < size; i++)
                {
                    var b = Stream.Read();
                    buffer[i] = (byte)b;
                }
                
                using (var bpr = new BinaryPackageReader(buffer))
                {
                    Globals.Settings.Unpack(bpr);
                }
            }
            Settings = Globals.Settings;
            Height = 5;
			Marshal.ReleaseComObject(Stream);
		}

		public void Save(IVariantStream Stream)
		{
            if (Globals.Settings == null)
                Globals.Settings = new DeltaSettings();
            
            PersistenceVersion = 1;
			if (PersistenceVersion > 0)
			{
				Stream.Write(PersistenceVersion);

			    using (var ms = new MemoryStream())
			    {
			        var bpw = new BinaryPackageWriter(ms);
			        Globals.Settings.Pack(bpw);
			        var buffer = ms.ToArray();

                    Stream.Write(buffer.Length);
			        foreach (var b in buffer)
			        {
                        Stream.Write(b);    
			        }
                    
			    }
			}

			Marshal.ReleaseComObject(Stream);
		}

        public DeltaSettings Settings { get; set; }
        
		#endregion

        private IApplication m_application;
        private IDocumentEvents_Event m_docEvents;
        private MxDocument m_appDocument;
        private esriExtensionState m_enableState;
        private Dictionary<string, Stream> dictionary;
        //private FeatureLoader _featLoader;
        private string _fileName;
    }
}
