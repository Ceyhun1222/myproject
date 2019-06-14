using System;
using System.Collections.Generic;
using System.Text;
using ARAN.AIXMTypes;
using ARAN.Contracts.Registry;
using ARAN.Contracts.GeometryOperators;
using ARAN.Contracts.Settings;
using ARAN.GeometryClasses;
using ARAN.Common;
using ARAN.Collection;
using System.Runtime.InteropServices;

namespace ARAN.Contracts.ObjectDirectory
{
	public class ObjectDirectoryContract
	{
		public ObjectDirectoryContract()
		{
			_ahpList = new PandaList<Ahp>();

			_rwyList = new PandaList<Rwy>();

			_rwyDirectionList = new PandaList<RwyDirection>();

			_handle = Registry_Contract.GetInstance("ObjectDirectory");

			_significantPoints = new SignificantPointCollection();
			_obstacles = new PandaList<Obstacle>();
			_DMEList = new PandaList<Dme>();
		}

		private enum ObjectDirectoryCommands
		{
			objectDirectoryGetAedromeList = 0,		// 0
			objectDirectoryGetRWYList,				// 1
			objectDirectorySetToSpatialReference,	// 2
			objectDirectoryGetToSpatialReference,	// 3
			objectDirectoryConnect,					// 4
			objectDirectoryDisConnect,				// 5
			objectDirectoryGetConnectionInfo,		// 6
			objectDirectoryGetRWYDirectionList,		// 7
			objectDirectoryGetSignificantPoints,	// 8
			objectDirectoryGetDMEList,				// 9
			objectDirectoryGetObstacles				// 10
		};

		~ObjectDirectoryContract()
		{
			Disconnect();
		}

		public bool IsValid()
		{
			return (_handle != 0);
		}

		public void Connect(ConnectionInfo connectionInfo)
		{
			Registry_Contract.BeginMessage(_handle, (int)ObjectDirectoryCommands.objectDirectoryConnect);
			connectionInfo.Pack(_handle);
			Registry_Contract.EndMessage(_handle);
		}

		public void Disconnect()
		{
			Registry_Contract.BeginMessage(_handle, (int)ObjectDirectoryCommands.objectDirectoryDisConnect);
			Registry_Contract.EndMessage(_handle);
		}

		public ConnectionInfo GetConnectionInfo()
		{
			Registry_Contract.BeginMessage(_handle, (int)ObjectDirectoryCommands.objectDirectoryGetConnectionInfo);
			Registry_Contract.EndMessage(_handle);
			ConnectionInfo result = new ConnectionInfo();
			result.UnPack(_handle);
			return result;
		}

		public void SetToSpatialReference(SpatialReference toSpatialReference)
		{
			Registry_Contract.BeginMessage(_handle, (int)ObjectDirectoryCommands.objectDirectorySetToSpatialReference);
			toSpatialReference.Pack(_handle);
			Registry_Contract.EndMessage(_handle);
		}

		public SpatialReference GetToSpatialReference()
		{
			Registry_Contract.BeginMessage(_handle, (int)ObjectDirectoryCommands.objectDirectoryGetToSpatialReference);
			Registry_Contract.EndMessage(_handle);
			sptlReference = new SpatialReference();
			sptlReference.UnPack(_handle);
			return sptlReference;
		}

		public PandaList<Ahp> GetAerodomeList(int countryID)
		{
			Registry_Contract.BeginMessage(_handle, (int)ObjectDirectoryCommands.objectDirectoryGetAedromeList);
			Registry_Contract.PutInt32(_handle, countryID);
			Registry_Contract.EndMessage(_handle);
			_ahpList.UnPack(_handle);
			return _ahpList;
		}

		public PandaList<Rwy> GetRWYList(int aerodromeID)
		{
			Registry_Contract.BeginMessage(_handle, (int)ObjectDirectoryCommands.objectDirectoryGetRWYList);
			Registry_Contract.PutInt32(_handle, aerodromeID);
			Registry_Contract.EndMessage(_handle);

			_rwyList.UnPack(_handle);
			return _rwyList;
		}

		public PandaList<RwyDirection> GetRWYDirectionList(int rwy_ID)
		{
			Registry_Contract.BeginMessage(_handle, (int)ObjectDirectoryCommands.objectDirectoryGetRWYDirectionList);
			Registry_Contract.PutInt32(_handle, rwy_ID);
			Registry_Contract.EndMessage(_handle);

			_rwyDirectionList.UnPack(_handle);
			return _rwyDirectionList;
		}

		public SignificantPointCollection GetSignificantPoints(int countryID, Point ptCenter, double range)
		{
			Registry_Contract.BeginMessage(_handle, (int)ObjectDirectoryCommands.objectDirectoryGetSignificantPoints);
			Registry_Contract.PutInt32(_handle, countryID);
			ptCenter.Pack(_handle);
			Registry_Contract.PutDouble(_handle, range);
			Registry_Contract.EndMessage(_handle);
			_significantPoints.Unpack(_handle);
			return _significantPoints;
		}

		public PandaList<Dme> GetDMEList(int countryID)
		{
			Registry_Contract.BeginMessage(_handle, (int)ObjectDirectoryCommands.objectDirectoryGetDMEList);
			Registry_Contract.PutInt32(_handle, countryID);
			Registry_Contract.EndMessage(_handle);
			_DMEList.UnPack(_handle);
			return _DMEList;
		}

		public PandaList<Obstacle> GetObstacleList(Point ptCenter, double range)
		{
			Registry_Contract.BeginMessage(_handle, (int)ObjectDirectoryCommands.objectDirectoryGetObstacles);
			ptCenter.Pack(_handle);
			Registry_Contract.PutDouble(_handle, range);
			Registry_Contract.EndMessage(_handle);
			_obstacles.UnPack(_handle);
			return _obstacles;
		}

		#region Private data section

		private int _handle;
		private PandaList<Ahp> _ahpList;
		private PandaList<Rwy> _rwyList;

		private PandaList<RwyDirection> _rwyDirectionList;

		private SignificantPointCollection _significantPoints;
		private PandaList<Dme> _DMEList;
		private PandaList<Obstacle> _obstacles;
		private SpatialReference sptlReference;

		#endregion
	}
}
