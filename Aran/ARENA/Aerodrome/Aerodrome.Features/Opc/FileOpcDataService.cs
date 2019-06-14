using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;
using System.Windows.Threading;

namespace Aerodrome.Opc
{
	public class FileOpcDataService : IOpcDataService
	{
		public FileOpcDataService(Dispatcher dispatcher, string fileName)
		{
			_dispatcher = dispatcher;
			_fileName = fileName;
			_cache = ReadAndParseFile();

			Thread pollingThread = new Thread(new ThreadStart(RefreshThreadProc));
			pollingThread.IsBackground = true;
			pollingThread.Start();
			Thread.Sleep(50);
		}

		private readonly string _fileName;

		// HACK: Es ist nicht gut, den Dispatcher hier benutzen zu müssen (wg. OPC-Sachen)
		private readonly Dispatcher _dispatcher;

		private void RefreshThreadProc()
		{
			while (true)
			{
				Thread.Sleep(500);

				try
				{
					var dict = ReadAndParseFile();
					foreach (var item in dict)
					{
						string value;
						if (_cache.TryGetValue(item.Key, out value))
						{
							if (value != item.Value)
							{
								OnDataChanged(item.Key, item.Value);
								_cache[item.Key] = item.Value;
							}
						}
						else
						{
							_cache.Add(item.Key, item.Value);
							OnDataChanged(item.Key, item.Value);
						}
					}
				}
				catch { } // TODO: Logging, etc.
			}
		}

		private Dictionary<string, string> ReadAndParseFile()
		{
			var dict = new Dictionary<string, string>();

			if (!File.Exists(_fileName))
				return dict;

			using (var sr = File.OpenText(_fileName))
			{
				while (!sr.EndOfStream)
				{
					var kv = sr.ReadLine().Split(new[] { ";" }, StringSplitOptions.None);

					string variableName = null;
					string value = string.Empty;
					if (kv.Length >= 1)
						variableName = kv[0];
					if (kv.Length >= 2)
						value = kv[1];

					if (!string.IsNullOrEmpty(variableName))
						dict.Add(variableName, value);
				}
			}

			return dict;
		}

		#region IOpcDataService Members

		public event EventHandler<OpcDataChangedEventArgs> DataChanged;
		private void OnDataChanged(string variableName, string value)
		{
			if (DataChanged != null)
			{
				_dispatcher.BeginInvoke(new Action(() =>
					DataChanged(this, new OpcDataChangedEventArgs(variableName, value))),
					DispatcherPriority.DataBind);
			}
		}

		private readonly Dictionary<string, string> _cache;
		public IEnumerable<KeyValuePair<string, string>> Cache
		{
			get
			{
				return _cache;
			}
		}

		#endregion
	}
}
