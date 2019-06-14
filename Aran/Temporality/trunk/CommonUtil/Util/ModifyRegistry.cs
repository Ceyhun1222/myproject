using System;
using System.Windows;
using Aran.Temporality.Common.Logging;
using Microsoft.Win32;

namespace Aran.Temporality.CommonUtil.Util
{
    public class ModifyRegistry
    {
        private bool _showError;
        public bool ShowError
        {
            get { return _showError; }
            set { _showError = value; }
        }

        private string _subKey = "SOFTWARE\\RISK";
        public string SubKey
        {
            get { return _subKey; }
            set { _subKey = value; }
        }

        private RegistryKey _baseRegistryKey = Registry.LocalMachine;
        public RegistryKey BaseRegistryKey
        {
            get { return _baseRegistryKey; }
            set { _baseRegistryKey = value; }
        }


        public string Read(string keyName)
        {

			var folder = _subKey.ToLower ( );
			var s = "software\\";
			if ( folder.StartsWith ( s ) )
				folder = folder.Substring ( s.Length );

			try
			{
				return CommonUtils.Config.ReadConfig<string> ( folder, keyName, null );
			}
			catch ( Exception e )
			{
				ShowErrorMessage ( e, "Reading appdata " + keyName.ToUpper ( ) );
				return null;
			}


			//var rk = _baseRegistryKey;
			//var sk1 = rk.OpenSubKey(_subKey);

			//if (sk1 == null)
			//{
			//	return null;
			//}

			//try
			//{
			//	return (string)sk1.GetValue(keyName.ToUpper());
			//}
			//catch (Exception e)
			//{
			//	ShowErrorMessage(e, "Reading registry " + keyName.ToUpper());
			//	return null;
			//}
        }

        public bool Write(string keyName, object value)
        {
			try
			{
				var folder = _subKey.ToLower ( );
				var s = "software\\";
				if ( folder.StartsWith ( s ) )
					folder = folder.Substring ( s.Length );

				CommonUtils.Config.WriteConfig ( folder, keyName, value );
				return true;
			}
			catch ( Exception e )
			{
				ShowErrorMessage ( e, "Writing appdata " + keyName.ToUpper ( ) );
				return false;
			}			
			
			//try
			//{
				

			//	var rk = _baseRegistryKey;
			//	var sk1 = rk.CreateSubKey(_subKey);
			//	if (sk1 != null)
			//	{
			//		sk1.SetValue(keyName.ToUpper(), value);
			//		return true;
			//	}
			//	return false;
			//}
			//catch (Exception e)
			//{
			//	ShowErrorMessage(e, "Writing registry " + keyName.ToUpper());
			//	return false;
			//}
        }

        public bool DeleteKey(string keyName)
        {
            try
            {
                var rk = _baseRegistryKey;
                var sk1 = rk.CreateSubKey(_subKey);
                if (sk1 == null)
                {
                    return true;
                }

                sk1.DeleteValue(keyName);

                return true;
            }
            catch (Exception e)
            {
                ShowErrorMessage(e, "Deleting SubKey " + _subKey);
                return false;
            }
        }

        public bool DeleteSubKeyTree()
        {
            try
            {
                var rk = _baseRegistryKey;
                var sk1 = rk.OpenSubKey(_subKey);
                if (sk1 != null)
                {
                    rk.DeleteSubKeyTree(_subKey);
                }
                return true;
            }
            catch (Exception e)
            {
                ShowErrorMessage(e, "Deleting SubKey " + _subKey);
                return false;
            }
        }


        public int SubKeyCount()
        {
            try
            {
                var rk = _baseRegistryKey;
                var sk1 = rk.OpenSubKey(_subKey);
                return sk1 != null ? sk1.SubKeyCount : 0;
            }
            catch (Exception e)
            {
                ShowErrorMessage(e, "Retriving subkeys of " + _subKey);
                return 0;
            }
        }


        public int ValueCount()
        {
            try
            {
                var rk = _baseRegistryKey;
                var sk1 = rk.OpenSubKey(_subKey);
                return sk1 != null ? sk1.ValueCount : 0;
            }
            catch (Exception e)
            {
                ShowErrorMessage(e, "Retriving keys of " + _subKey);
                return 0;
            }
        }


        private void ShowErrorMessage(Exception e, string Title)
        {
            if (_showError)
            {
                LogManager.GetLogger(typeof(ModifyRegistry)).Error(e, Title);
                MessageBox.Show(e.Message,
                                Title,
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
            }
        }
    }
}


