using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aran.AranEnvironment;
using ARAN.Contracts.Registry;
using System.Runtime.InteropServices;
using ARAN.Contracts.Settings;
using System.Windows.Forms;

namespace SettingsService
{
    public class SettingsServicePlugin:IAranPlugin
    {
        public SettingsServicePlugin()
        {
            _entryPointMethod = new Registry_Contract.Method(EntryPoint);
            _entryPointHandle = GCHandle.Alloc(_entryPointMethod);
        }
        public void Startup(IAranEnvironment aranEnv)
        {
            Global.Env = aranEnv;
            Global.AranExtension = aranEnv.PandaAranExt;
            Registry_Contract.RegisterClass("SettingsService", 0, _entryPointMethod);
        }
        public int EntryPoint(Int32 _this, Int32 command, Int32 inOut)
        {
            Int32 result = Registry_Contract.rcOK;
            try
            {
                switch (command)
                {
                    case Registry_Contract.svcGetInstance:
                        settings = new Aran.PANDA.Common.Settings();
			            settings.Load(Global.AranExtension);
                        break;
                    case Registry_Contract.svcFreeInstance:
                        _entryPointHandle.Free();
                        break;
                    case (int)SettingsCommands.settingsGetFileName:
                            Registry_Contract.PutString(inOut, 
                                (Global.Env.DocumentFileName == null ? string.Empty : Global.Env.DocumentFileName));
                        break;
                    case (int)SettingsCommands.settingsIsFileNameEmpty:
                            Registry_Contract.PutBool(inOut,
                                (Global.Env.DocumentFileName == null)?true:false);
                        break;
                    case (int)SettingsCommands.settingsGetLanguageCode:
                        Registry_Contract.PutInt32(inOut,(int)settings.Language);
                        break;
                    case (int)SettingsCommands.settingsGetDistanceUnit:
                        Registry_Contract.PutInt32(inOut,(int)settings.DistanceUnit);
                        break;
                    case (int)SettingsCommands.settingsGetElevationUnit:
                        Registry_Contract.PutInt32(inOut, (int)settings.HeightUnit);
                        break;
                    case (int)SettingsCommands.settingsGetSpeedUnit:
                        Registry_Contract.PutInt32(inOut,(int) settings.SpeedUnit);
                        break;
                    case (int)SettingsCommands.settingsGetDistanceAccuracy:
                        Registry_Contract.PutDouble(inOut, settings.DistancePrecision);
                        break;
                    case (int)SettingsCommands.settingsGetElevationAccuracy:
                        Registry_Contract.PutDouble(inOut, settings.HeightPrecision);
                        break;
                    case (int)SettingsCommands.settingsGetSpeedAccuracy:
                        Registry_Contract.PutDouble(inOut, settings.SpeedPrecision);
                        break;
                    case (int)SettingsCommands.settingsGetAngleAccuracy:
                        //must be look again
                        Registry_Contract.PutDouble(inOut, 0.1);
                        break;
                    case (int)SettingsCommands.settingsGetGradientAccuracy:
                        Registry_Contract.PutDouble(inOut, 0.1);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception)
            {
                return Registry_Contract.rcException;
            }

            return result;
        }


        private Registry_Contract.Method _entryPointMethod;
        private GCHandle _entryPointHandle;
        private Aran.PANDA.Common.Settings settings;

        public void AddChildSubMenu(List<string> hierarcy)
        {
            throw new NotImplementedException();
        }

        public string Name
        {
            get { return "SettingsService"; }
        }
    }
}
