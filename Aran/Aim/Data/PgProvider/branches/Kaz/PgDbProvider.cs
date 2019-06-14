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
		public PgDbProvider ( )
		{
			_connection = CommonData.CreateConnection ( );
			_reader = new Reader ( _connection );
		}

		#region IDbProvider Members

		public void Open ( string pgConnStr, out string errorString )
		{
			_connection.ConnectionString = pgConnStr;
			try
			{
				_connection.Open ( );
			}
			catch ( Exception ex )
			{
				errorString = ex.Message;
				return;
			}

			_writer = new Writer ( _connection );
			errorString = "";
		}

		public void Close ( )
		{
			_connection.Close ( );
		}

		public ConnectionState State
		{
			get
			{
				return _connection.State;
			}
		}

		public int BeginTransaction ( )
		{
			return _writer.BeginTransaction ( );
		}

		public InsertingResult Insert ( Feature feature )
		{
			int newTransactionId = BeginTransaction ( );
			InsertingResult result = Insert ( feature, newTransactionId );
			if ( result.IsSucceed )
				return Commit ( newTransactionId );
			return result;
		}

		public InsertingResult Insert ( Feature feature, int transactionId )
		{
			bool isCorrection = ( feature.Id > 0 );
			return Insert ( feature, transactionId, isCorrection);
		}

		//public InsertingResult Insert ( Feature feature, bool insertAnyway )
		//{
		//    return Insert ( feature, insertAnyway, false );
		//}

		//public InsertingResult Insert ( Feature feature, bool insertAnyway, bool asCorrection )
		//{
		//    int newTransactionId = BeginTransaction ( );
		//    Insert ( feature, newTransactionId, insertAnyway, asCorrection );
		//    return Commit ( newTransactionId );
		//}

		private InsertingResult Insert ( Feature feature, int transactionId, bool asCorrection )
		{
			return _writer.Insert ( feature, transactionId, asCorrection);
		}

		public string Delete ( Feature feature )
		{
			return _writer.Delete ( feature );
		}

		public InsertingResult Commit ( int transactionId )
		{
			return _writer.Commit ( transactionId );
		}

		public InsertingResult Roollback ( int transactionId )
		{
			return _writer.Rollback ( transactionId );
		}

		public GettingResult GetFeat ( FeatureType featType,
					  Guid identifier = default(Guid),
					  bool loadComplexProps = false,
					  List<string> propList = null,
					  Filter filter = null )
		{
			return _reader.GetFeat ( featType, identifier, loadComplexProps, false, null, propList, filter );
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
		//public GettingResult GetVersionsOf ( FeatureType featType,
		//    //TimeSliceInterpretationType interpretation, 
		//                            Guid identifier = default(Guid),
		//                            bool loadComplexProps = false,
		//                            TimeSliceFilter timeSlicefilter = null,
		//                            List<string> propList = null,
		//                            Filter filter = null )
		//{
		//    //return _reader.VersionsOf ( featType, interpretation, identifier, loadComplexProps, false, timeSlicefilter, propList, filter );
		//    return _reader.VersionsOf ( featType, identifier, loadComplexProps, false, timeSlicefilter, propList, filter );
		//}

		public GettingResult GetFeat ( IAbstractFeatureRef absFeatRef,
									bool loadComplexProps = false,
									List<string> propList = null,
									Filter filter = null )
		{
			return GetFeat ( ( FeatureType ) absFeatRef.FeatureTypeIndex,
									absFeatRef.Identifier,
									loadComplexProps,									
									propList,
									filter );
		}

		public GettingResult GetAllStoredFeatIndices ( )
		{
			return _reader.GetAllStoredFeatIndices ( );
		}


		//public GettingResult GetVersionsOf ( AbstractFeatureType absFeatType,
		//    //TimeSliceInterpretationType interpretation, 
		//                                            bool loadComplexProps = false,
		//                                            TimeSliceFilter timeSlicefilter = null,
		//                                            List<string> propList = null,
		//                                            Filter filter = null )
		//{
		//    List<AimClassInfo> descentClassInfoList = AimMetadata.AimClassInfoList.FindAll (
		//                                                        aimClassInfo =>
		//                                                                aimClassInfo.Parent != null &&
		//                                                                aimClassInfo.Parent.Name == absFeatType.ToString ( ) );
		//    Feature protoType;
		//    GettingResult result = new GettingResult ( true );
		//    result.List = AimObjectFactory.CreateList ( ( int ) absFeatType );
		//    GettingResult getResult;
		//    foreach ( AimClassInfo aimClassInfo in descentClassInfoList )
		//    {
		//        protoType = ( Feature ) AimObjectFactory.Create ( aimClassInfo.Index );
		//        //getResult = GetVersionsOf ( ( FeatureType ) aimClassInfo.Index, 
		//        //                        interpretation, 
		//        //                        default ( Guid ), 
		//        //                        loadComplexProps, 
		//        //                        timeSlicefilter,
		//        //                        propList, 
		//        //                        filter );
		//        getResult = GetVersionsOf ( ( FeatureType ) aimClassInfo.Index,
		//                default ( Guid ),
		//                loadComplexProps,
		//                timeSlicefilter,
		//                propList,
		//                filter );
		//        if ( !getResult.IsSucceed )
		//            return getResult;
		//        foreach ( var feat in getResult.List )
		//            result.List.Add ( feat );
		//    }
		//    return result;
		//}

		//public GettingResult Changes ( Guid featIdentifier, FeatureType featType, int sequence, int correction )
		//{
		//    return _reader.Changes ( featIdentifier, featType, sequence, correction );
		//}

		//public TimeSliceFilter TimeSliceFilter
		//{
		//    get
		//    {
		//        return _reader.TimeSliceFilter;
		//    }
		//    set
		//    {
		//        _reader.TimeSliceFilter = value;
		//    }
		//}

		public string [] GetLastErrors ( )
		{
			return new string [ 0 ];
		}

		#endregion

		private Reader _reader;
		private Writer _writer;
		private IDbConnection _connection;
	}
}