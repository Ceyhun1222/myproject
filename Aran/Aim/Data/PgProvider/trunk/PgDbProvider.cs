using System;
using System.Collections.Generic;
using System.Data;
using Aran.Aim.Data.Filters;
using System.Collections;
using Aran.Aim.DataTypes;
using Aran.Aim.Data;
using Aran.Aim.Features;
using Aran.Aim.Enums;

namespace Aran.Aim.Data
{
    internal class PgDbProvider : IDbProvider
    {
        public PgDbProvider ()
        {
            _connection = CommonData.CreateConnection ();
            _reader = new Reader (_connection);
        }

        #region IDbProvider Members

        public string Open ( string pgConnStr )
        {
			_connection.ConnectionString = pgConnStr;

            try
            {
				_connection.Open();
            }
            catch (Exception ex)
            {
				return ex.Message;
            }

			_writer = new Writer ( _connection );
			return "";
        }

        public void Close ( )
        {
            _connection.Close ();
        }

        public ConnectionState State
        {
            get
            {
                return _connection.State;
            }
        }

        public int BeginTransaction ()
        {
			return _writer.BeginTransaction ( );
        }

		public InsertingResult Insert ( Feature feature )
		{
			return Insert ( feature, false, false );
		}

		public InsertingResult Insert ( Feature feature, bool insertAnyway )
		{
			return Insert ( feature, insertAnyway, false );
		}

		public InsertingResult Insert ( Feature feature, bool insertAnyway , bool asCorrection)
		{
			int newTransactionId = BeginTransaction ( );
			var insertResult = Insert ( feature, newTransactionId, insertAnyway, asCorrection );
            
            if (!insertResult.IsSucceed)
            {
                Rollback (newTransactionId);
                return insertResult;
            }

			return Commit ( newTransactionId );
		}

		public InsertingResult Insert ( Feature feature, int transactionId )
		{
			return Insert ( feature, transactionId, false, false );
		}

		public InsertingResult Insert ( Feature feature, int transactionId, bool insertAnyway )
		{
			return Insert ( feature, transactionId, insertAnyway, false );
		}

        public InsertingResult Insert (Feature feature, int transactionId, bool insertAnyway, bool asCorrection)
        {
			return _writer.Insert ( feature, transactionId, insertAnyway, asCorrection );
        }

		public InsertingResult Commit ( int transactionId )
        {
			return _writer.Commit ( transactionId );
        }

		public InsertingResult Rollback ( int transactionId )
        {
			return _writer.Rollback ( transactionId );
        }

		/// <summary>
		/// Created just import mass of data 
		/// </summary>
		/// <param name="feature"></param>
		/// <returns></returns>
		public InsertingResult Update ( Feature feature )
		{
			int transactionId = _writer.BeginTransaction ( );
			InsertingResult result = _writer.Update ( feature, transactionId );
			if ( !result.IsSucceed )
			{
				Rollback ( transactionId );
				return result;
			}
			return Commit ( transactionId );

		}

		/// <summary>
		/// Created just import mass of data 
		/// </summary>
		/// <param name="feature"></param>
		/// <returns></returns>
		public InsertingResult Update ( Feature feature, int transactionId )
		{
			return _writer.Update ( feature, transactionId );
		}

        ///// <summary>
        ///// Gets version(s) of feature for given identifer
        ///// </summary>
        ///// <typeparam name="T">Wich feature type you want to get</typeparam>
        ///// <param name="identifier">Identifier of given feature(T)</param>
        ///// <param name="loadComplexProp">Determines to load complex types or not</param>
        ///// <param name="propInfoList">Which properties you want to be fulled.If there is object property to get then loadComplexTypes have to be true</param>
        ///// <param name="timeSliceFilter">Condition that define your query</param>
        ///// <returns>Returns list of T feature for given identifier and condition</returns>
        public GettingResult GetVersionsOf ( FeatureType featType,
                                    TimeSliceInterpretationType interpretation, 
                                    Guid identifier = default(Guid),
                                    bool loadComplexProps = false,
                                    TimeSliceFilter timeSlicefilter = null,
                                    List<string> propList = null,
                                    Filter filter = null )
        {
			return _reader.VersionsOf ( featType, interpretation, identifier, loadComplexProps, false, timeSlicefilter, propList, filter );
        }

        public GettingResult GetVersionsOf ( IAbstractFeatureRef absFeatRef,
                                    TimeSliceInterpretationType interpretation, 
                                    bool loadComplexProps = false,
                                    TimeSliceFilter timeSliceFilter = null,
                                    List<string> propList = null,
                                    Filter filter = null )
        {

            return GetVersionsOf ( ( FeatureType ) absFeatRef.FeatureTypeIndex,
                                                interpretation,
                                                absFeatRef.Identifier,
                                                loadComplexProps,
                                                timeSliceFilter,
                                                propList,
                                                filter );
        }

        public GettingResult GetVersionsOf ( AbstractFeatureType absFeatType,
                                                    TimeSliceInterpretationType interpretation, 
                                                    bool loadComplexProps = false,
                                                    TimeSliceFilter timeSlicefilter = null,
                                                    List<string> propList = null,
                                                    Filter filter = null )
        {
            List<AimClassInfo> descentClassInfoList = AimMetadata.AimClassInfoList.FindAll (
                                                                aimClassInfo => 
                                                                        aimClassInfo.Parent != null && 
                                                                        aimClassInfo.Parent.Name == absFeatType.ToString () );
            Feature protoType;
            GettingResult result = new GettingResult ( true );
            result.List = AimObjectFactory.CreateList((int)absFeatType);
            GettingResult getResult;
            foreach ( AimClassInfo aimClassInfo in descentClassInfoList )
            {
                protoType = ( Feature ) AimObjectFactory.Create ( aimClassInfo.Index );
                getResult = GetVersionsOf ( ( FeatureType ) aimClassInfo.Index, 
                                        interpretation, 
                                        default ( Guid ), 
                                        loadComplexProps, 
                                        timeSlicefilter,
                                        propList, 
                                        filter );
                if ( !getResult.IsSucceed )
                    return getResult;
                foreach ( var feat in getResult.List )
                    result.List.Add ( feat );
            }
            return result;
        }

        public List<Feature> GetAllFeatuers(FeatureType featType, Guid identifier = new Guid())
        {
            throw new NotImplementedException();
        }

        public GettingResult GetAllStoredFeatTypes ( )
		{
			return _reader.GetAllStoredFeatTypes ( );
		}

		public GettingResult Changes ( Guid featIdentifier, FeatureType featType, int sequence, int correction )
		{
			return _reader.Changes ( featIdentifier, featType, sequence, correction );
		}

		public TimeSliceFilter TimeSliceFilter
		{
			get
			{
                return _reader.TimeSliceFilter;
			}
			set
			{
				_reader.TimeSliceFilter = value;
			}
		}

        public string [] GetLastErrors ()
        {
            return new string [0];
        }

		public InsertingResult Delete ( Feature feature )
		{
			throw new NotImplementedException ( );
		}

		public InsertingResult InsertUser ( User user )
		{
			throw new NotImplementedException ( );
		}

		public InsertingResult UpdateUser ( User user )
		{
			throw new NotImplementedException ( );
		}

		public GettingResult ReadUsers ( )
		{
			throw new NotImplementedException ( );
		}

		public InsertingResult DeleteUser ( User user )
		{
			throw new NotImplementedException ( );
		}

		public GettingResult ReadUserByName ( string userName )
		{
			throw new NotImplementedException ( );
		}

		public User User
		{
			get
			{
				throw new NotImplementedException ( );
			}
			set
			{
				throw new NotImplementedException ( );
			}
		}

		public GettingResult ReadUsersById ( long userId )
		{
			throw new NotImplementedException ( );
		}

        public bool IsExists(Guid guid)
        {
            throw new NotImplementedException();
        }

        #endregion

        private Reader _reader;
        private Writer _writer;
		private IDbConnection _connection;

		#region IDbProvider Members


		public GettingResult GelAllStoredIdentifiers ( )
		{
			throw new NotImplementedException ( );
		}

		public GettingResult DeleteFeatIdentifiers ( List<Guid> notExportedIdentifiers )
		{
			throw new NotImplementedException ( );
		}


		void IDbProvider.Open(string connectionString)
		{
			throw new NotImplementedException();
		}

		public bool Login(string userName, string md5Password)
		{
			throw new NotImplementedException();
		}

		public DateTime DefaultEffectiveDate
		{
			get
			{
				throw new NotImplementedException();
			}
			set
			{
				throw new NotImplementedException();
			}
		}

		public bool SetEffectiveDateChangedEventHandler(EffectiveDateChangedEventHandler handler)
		{
			throw new NotImplementedException();
		}

		public User CurrentUser
		{
			get { throw new NotImplementedException(); }
		}

		public IUserManagement UserManagement
		{
			get { throw new NotImplementedException(); }
		}

		public bool UseCache
		{
			get
			{
				throw new NotImplementedException();
			}
			set
			{
				throw new NotImplementedException();
			}
		}
		#endregion

        void IDbProvider.Open(string connectionString)
        {
            throw new NotImplementedException();
        }

        public bool Login(string userName, string md5Password)
        {
            throw new NotImplementedException();
        }

        public DateTime DefaultEffectiveDate
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public bool SetEffectiveDateChangedEventHandler(EffectiveDateChangedEventHandler handler)
        {
            throw new NotImplementedException();
        }

        public User CurrentUser
        {
            get { throw new NotImplementedException(); }
        }

        public IUserManagement UserManagement
        {
            get { throw new NotImplementedException(); }
        }

        public bool UseCache
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }
    }
}