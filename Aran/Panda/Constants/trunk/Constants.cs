using System;
using System.Data.OleDb;
using Aran.PANDA.Common;
using Microsoft.Win32;
using System.IO;

namespace Aran.PANDA.Constants
{
	public class Constants
	{
		public Constants()
		{
			Pansops = null;
			GNSS = null;
			AircraftCategory = null;
			Pbn_Rnp_Apch = null;
			Fte_ConstantsList = null;

            bool isExists;
            InstallDir = RegFuncs.GetConstantsDir(out isExists);

			if (!isExists)
				throw new Exception("Path registry key is corrupted !");
		}

		public void Reset()
		{
			Pansops = null;
			GNSS = null;
			Pbn_Rnp_Apch = null;
			Fte_ConstantsList = null;
			AircraftCategory = null;
		}

		private void LoadPansopsConstants()
		{
			PANSOPSConstantListLoader loader = new PANSOPSConstantListLoader();
			loader.LoadFromFile(InstallDir + "\\pansops.dat");

			PANSOPSConstantListLoader tmpPansopsConstantListLoader = new PANSOPSConstantListLoader();
			tmpPansopsConstantListLoader.LoadFromFile(InstallDir + "\\arpansops.dat");

			loader.Merge(tmpPansopsConstantListLoader, ePANSOPSData.arBufferMSA, ePANSOPSData.enTurnIAS);

			tmpPansopsConstantListLoader.LoadFromFile(InstallDir + "\\enroute.dat");
			loader.Merge(tmpPansopsConstantListLoader, ePANSOPSData.enTurnIAS);

			Pansops = loader;
		}

		private void LoadGnssConstants()
		{
			SensorContantsListLoader loader = new SensorContantsListLoader();
			loader.LoadFromFile(InstallDir + "\\gnss.dat");
			GNSS = loader;
		}

		private void LoadPbnRnpApchConstants()
		{
			SensorContantsListLoader loader = new SensorContantsListLoader();
			loader.LoadFromFile(InstallDir + "\\PBN RNP APCH.dat");
			Pbn_Rnp_Apch = loader;
		}

		private void LoadAircraftCategoryConstants()
		{
			AircraftCategoryListLoader aircraftConstLoader = new AircraftCategoryListLoader();
			aircraftConstLoader.LoadFromFile(InstallDir + "\\categories.dat");
			AircraftCategory = aircraftConstLoader;
		}

		private void LoadFTECategoryConstants()
		{
			FteConstantListLoader fTEConstLoader = new FteConstantListLoader();
			fTEConstLoader.LoadFromFile(InstallDir + "\\FTE.dat");
			Fte_ConstantsList = fTEConstLoader;
		}

		private void LoadNavaidConstans()
		{
			_navaid_Contants = new NavaidsConstant();
			_navaid_Contants.LoadFromFile(InstallDir + "\\Navaids\\");
		}

        private void LoadPansopsCoreDatabaseConstants()
        {
            _pansopsCoreDBConstants = new PansOpsCoreDatabase();
            _pansopsCoreDBConstants.LoadFromFile(InstallDir);
        }

		private bool OpenDbfConnection(string pathFolder)
		{
			try
			{
				_conn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + pathFolder + ";Extended Properties=dBASE IV;");
				_conn.Open();
				return true;
			}
			catch (Exception)
			{
				return false;
			}

		}

		private void CloseConnection()
		{
			_conn.Close();
		}

		public string InstallDir
		{
			get;
			set;
		}

		public PANSOPSConstantList Pansops
		{
			get
			{
				if (_pansopsConstList == null)
					LoadPansopsConstants();
				return _pansopsConstList;
			}

			private set
			{
				_pansopsConstList = value;
			}
		}

        public PansOpsCoreDatabase PansopsCoreDB
        {
            get
            {
                if (_pansopsCoreDBConstants == null)
                    LoadPansopsCoreDatabaseConstants();
                return _pansopsCoreDBConstants;
            }
            private set
            {
                _pansopsCoreDBConstants = value;
            }
        }

		public SensorConstantList GNSS
		{
			get
			{
				if (_gnssConstList == null)
					LoadGnssConstants();
				return _gnssConstList;
			}
			private set
			{
				_gnssConstList = value;
			}
		}

		public AircraftCategoryList AircraftCategory
		{
			get
			{
				if (_aircraftCategoryList == null)
					LoadAircraftCategoryConstants();
				return _aircraftCategoryList;
			}
			private set
			{
				_aircraftCategoryList = value;
			}
		}

		public SensorConstantList Pbn_Rnp_Apch
		{
			get
			{
				if (_pbn_Rnp_Apch == null)
					LoadPbnRnpApchConstants();
				return _pbn_Rnp_Apch;
			}
			private set
			{
				_pbn_Rnp_Apch = value;
			}
		}

		public FteConstantList Fte_ConstantsList
		{
			get
			{
				if (_fte_ConstList == null)
					LoadFTECategoryConstants();
				return _fte_ConstList;
			}
			private set
			{
				_fte_ConstList = value;
			}
		}

		public NavaidsConstant NavaidConstants
		{
			get
			{
				if (_navaid_Contants == null)
					LoadNavaidConstans();
				return _navaid_Contants;
			}
			private set
			{
				_navaid_Contants = value;
			}
		}

		public FlightConditionConstant FlightCondConstant
		{
			get
			{
				if (_flightConditionConstant == null)
				{
					_flightConditionConstant = new FlightConditionConstant();
					_flightConditionConstant.LoadFromFile(InstallDir + "\\FlightCondition.s3db\\");
				}
				return _flightConditionConstant;
			}
			private set
			{
				_flightConditionConstant = value;
			}
		}

		public RunwayConstansList RunwayConstants
		{
			get
			{
				if (_runwayConstantList == null)
				{
					_runwayConstantList = new RunwayConstansList(InstallDir);
					_runwayConstantList.Load();
				}
				return _runwayConstantList;
			}

		}

		private PANSOPSConstantList _pansopsConstList;
        private PansOpsCoreDatabase _pansopsCoreDBConstants;
		private SensorConstantList _gnssConstList;
		private AircraftCategoryList _aircraftCategoryList;
		private SensorConstantList _pbn_Rnp_Apch;
		private FteConstantList _fte_ConstList;
		private NavaidsConstant _navaid_Contants;
		private FlightConditionConstant _flightConditionConstant;
		private RunwayConstansList _runwayConstantList;
		private OleDbConnection _conn;
	}
}
