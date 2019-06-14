using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aran.PANDA.Conventional.Settings
{
	public enum SettingsCommands
	{
		settingsGetString,
		settingsGetDouble,
		settingsGetInt32,
		settingsGetBoolean,
		settingsSetString,
		settingsSetDouble,
		settingsSetInt32,
		settingsSetBoolean,
		settingsIsFileNameEmpty,
		settingsGetFileName,
		settingsSave,
		settingsSaveAs


		//		settingsGetValue,
		//		settingsSetValue,
		//		settingsIsFileNameEmpty,
		//		settingsGetFileName
	};

	public enum ConnectionType
	{
		connectionTypePostgres,
		connectionTypeMDB
	};
	public enum HorisontalDistanceUnit
	{
		hduMeter,
		hduKM,
		hduNM
	};
	public enum VerticalDistanceUnit
	{
		vduMeter,
		vduFeet,
		vduFL,
		vduSM
	};
	public enum HorisontalSpeedUnit
	{
		hsuMeterInSec,
		hsuKMInHour,
		hsuKnot
	};
	public enum VerticalSpeedUnit
	{
		vsuMeterInMin,
		vsuFeetInMin
	};
	public enum LanguageCode
	{
		langCodeEng = 1033,
		langCodeRus = 1049,
		langCodePortuguese = 1046
	};
}
