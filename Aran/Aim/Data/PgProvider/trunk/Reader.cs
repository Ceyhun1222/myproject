using System;
using System.Collections.Generic;
using Aran.Aim.Features;
using Aran.Aim.DB;
using System.Collections;
using Aran.Aim.DataTypes;
using Aran.Aim.Enums;
using Aran.Aim.Objects;
using Aran.Aim.PropertyEnum;
using System.Data;
using System.Reflection;
using Aran.Geometries.IO;
using Aran.Aim.Data.Filters;

namespace Aran.Aim.Data
{
    internal class Reader : IDataListener
    {
        public static string ForDebugCmdText = "";

        public Reader (IDbConnection connection) 
        {
            _connection = connection;
            _columnIndices = new Dictionary<AimPropInfo, int> ();
        }

        public TimeSliceFilter TimeSliceFilter
        {
            get { return _timeSliceFilter; }
            set { _timeSliceFilter = value; }
        }

        public GettingResult VersionsOf (	FeatureType featType,
											TimeSliceInterpretationType interpretation, 
											Guid identifier = default(Guid),
											bool loadComplexProps = false,
											bool onlyBaseComplexProps = false,
											TimeSliceFilter timeSlicefilter = null,
											List<string> propertyList = null,
											Filter filter = null )
        {
			IDataReader dataReader = null;

			try
			{
				if ( interpretation != TimeSliceInterpretationType.BASELINE )
					return new GettingResult ( false, interpretation + " interpretation is not implemented yet :(!" );

                IDbCommand command = _connection.CreateCommand ();

				int featTypeIndex = ( int ) featType;

                #region GetFeatureType If type index is FeatureRef

                if (featTypeIndex == (int) DataType.FeatureRef)
                {
                    command.CommandText =
                        "SELECT type_index " +
                        "FROM features WHERE \"Identifier\"='" + identifier + "'";
                    featTypeIndex = (int) command.ExecuteScalar ();
                }

                #endregion

                string featName = "bl_" + AimMetadata.GetAimTypeName ( featTypeIndex );
				TimeSliceFilter tmpTimeSliceFilter = ( timeSlicefilter == null ? _timeSliceFilter : timeSlicefilter );
				if ( tmpTimeSliceFilter == null )
					return new GettingResult ( false, "Time Slice Filter is null !" );

				AimPropInfoList simplePropList = new AimPropInfoList ( );
				AimPropInfoList complexPropList = new AimPropInfoList ( );
				AimPropInfoList propInfoList = new AimPropInfoList ( );

				AimPropInfo [ ] propInfoArray = AimMetadata.GetAimPropInfos ( featTypeIndex );
				if ( propertyList == null )
				{
					propInfoList.AddRange ( propInfoArray );
				}
				else
				{
					foreach ( string propertyName in propertyList )
					{
						AimPropInfo propInfo = GetPropInfo ( propInfoArray, propertyName.ToLower ( ) );
						if ( propInfo == null )
							return new GettingResult ( false, "Not found property !" );
						propInfoList.Add ( propInfo );
					}
				}
				string columns = SeparateProperties ( propInfoList, simplePropList, complexPropList, featName );
				string mandatCols = AddFeatMandatoryColumns ( featTypeIndex, simplePropList );
				if ( mandatCols != "" )
					columns += ",\r\n" + mandatCols;

				if ( identifier != Guid.Empty )
					command.CommandText = string.Format ( "SELECT {0} FROM \"{1}\", features " +
														"WHERE \"{1}\".feat_id = features.\"Id\" AND " +
														"features.\"Identifier\" = '{2}' AND " +
														"{3}",
														columns, featName,
														identifier, TimeSliceFilterToSqlCommand ( featName, tmpTimeSliceFilter ) );
				else
				{
					command.CommandText = string.Format ( "SELECT {0} FROM \"{1}\", features " +
															"WHERE \"{1}\".feat_id = features.\"Id\" AND " +
															"features.type_index = {2} AND " +
															"{3}",
															 columns, featName,
															 featTypeIndex, TimeSliceFilterToSqlCommand ( featName, tmpTimeSliceFilter ) );
				}
				if ( filter != null )
				{
					PgFilterImplementation pgFilterImp = new PgFilterImplementation ( );
					byte [ ] geomByteA = null;
					command.CommandText += " AND " + pgFilterImp.GetSqlString ( filter.Operation, featTypeIndex, ref geomByteA );
					if ( geomByteA != null )
					{   
						AddParameterToDBCommand ( command, geomByteA );
					}
				}
				dataReader = command.ExecuteReader ( );

				IList result = GetDBEntityList ( featTypeIndex, dataReader, simplePropList );
				if ( result.Count > 0 )
				{
					if ( loadComplexProps )
					{
						LoadComplexPropertiesOfFeature ( result, complexPropList, onlyBaseComplexProps );
					}
				}
				dataReader.Close ( );
				GettingResult getResult = new GettingResult ( true );
				getResult.List = result;
				return getResult;
			}
			catch ( Exception exc )
			{
				if (dataReader != null)
					dataReader.Close ( );
				_connection.Close ( );
				_connection.Open ( );
				return new GettingResult ( false, CommonData.GetErrorMessage ( exc ) );
			}
        }


		public GettingResult GetAllStoredFeatTypes ( )
		{
			IDataReader dataReader = null;
			try
			{
				string sqlString = "SELECT type_index FROM features GROUP BY type_index";
				IDbCommand command = _connection.CreateCommand ( );
				command.CommandText = sqlString;
				dataReader = command.ExecuteReader ( );
				List<FeatureType> indices = new List<FeatureType> ( );
				while ( dataReader.Read ( ) )
				{
					indices.Add ( ( FeatureType ) ( int ) dataReader[ 0 ] );
				}
				dataReader.Close ( );
				GettingResult getResult = new GettingResult ( true );
				getResult.List = indices;
				return getResult;
			}
			catch ( Exception exc )
			{
				if ( dataReader != null )
					dataReader.Close ( );
				_connection.Close ( );
				_connection.Open ( );
				return new GettingResult ( false, CommonData.GetErrorMessage ( exc ) );
			}
		}

		public GettingResult Changes ( Guid featIdentifier, FeatureType featType, int sequence, int correction)
		{			
			IDbCommand dbCommand = _connection.CreateCommand ( );
			dbCommand.CommandText = string.Format ( "select  t.\"Id\", " +
															"t.interpretation, " +
															"t.sequence, " +
															"t.correction, " +
															"t.begin_valid, " +
															"t.end_valid, " +
															"t.created_on, " +
															"t.created_by " +
															"from feat_delta as t " +
															#warning Set Sequence and Correction greater than not equal
																"where t.sequence = {0} and " +
 																"t.correction = {1} and " + 
																//"t.correction = (select max(correction) from feat_delta where sequence = t.sequence) and " +
																"t.feat_id in (select \"Id\" from features where \"Identifier\" = '{2}')", 
													sequence, 
													correction, 
													featIdentifier );
			IDataReader dataReader = null;
			try
			{
				dataReader = dbCommand.ExecuteReader ( );
				List<ChangeIdentifier> list = new List<ChangeIdentifier> ( );
				ReadDeltaTable ( dataReader, list );
				dataReader.Close ( );

				if ( list.Count > 0 )
				{
					string idList;
					idList = list [ 0 ].Id.ToString ( );
					for ( int i = 1; i < list.Count; i++ )
					{
						idList += ", " + list[i].Id.ToString ( );
					}
					dbCommand.CommandText = string.Format ( "select feat_delta_id, key, value from feat_delta_properties " +
																"where feat_delta_id in ({0}) order by feat_delta_id", idList );
					dataReader = dbCommand.ExecuteReader ( );
					ReadDeltaPropertiesTable ( dataReader, list, ( int ) featType );
				}

				GettingResult result = new GettingResult ( true );
				result.List = list;
				return result;
			}
			catch ( Exception exc)
			{
				if (dataReader != null)
					dataReader.Close ( );
				_connection.Close ( );
				_connection.Open ( );
				return new GettingResult ( false, CommonData.GetErrorMessage ( exc ) );
			}			
		}

		private void ReadDeltaPropertiesTable ( IDataReader dataReader, List<ChangeIdentifier> list, int featType )
		{
			long id;
			ChangeIdentifier changeIdentifier = list [ 0 ];
			changeIdentifier.Properties = new SortedDictionary<int, IAimProperty> ( );
			int propIndex;
			DeltaProp deltaProp = new DeltaProp ( );
			byte[] byteArray;
			AimClassInfo classInfo = AimMetadata.GetClassInfoByIndex ( featType );
			AimPropInfo propInfo;
			while ( dataReader.Read ( ) )
			{
				id = ( long ) dataReader [ 0 ];
				if ( id != changeIdentifier.Id )
				{
					foreach ( var item in list )
					{
						if ( item.Id == id )
						{
							changeIdentifier = item;
							if (changeIdentifier.Properties == null)
								changeIdentifier.Properties = new SortedDictionary<int,IAimProperty>();
							break;
						}
					}
				}
				propIndex = (int) dataReader[1];
				byteArray = (byte[]) dataReader[2];
				propInfo = classInfo.Properties[propIndex - 1];
				IAimProperty aimProp;
				if ( deltaProp.FromByteArray ( propInfo, byteArray, out aimProp ) )
					changeIdentifier.Properties.Add ( propIndex, aimProp );
				else
				{
					GetComplexPropById ( propInfo, aimProp );
				}
			}
		}

		private void GetComplexPropById ( AimPropInfo propInfo, IAimProperty aimProp)
		{
			if ( propInfo.IsList )
			{
				IList propList = ( IList ) aimProp;
				if ( AimMetadata.IsChoice ( propInfo.TypeIndex ) )
				{
				}

				if ( propInfo.PropType.AimObjectType == AimObjectType.Object )
				{
					AimPropInfoList allPropInfoList = new AimPropInfoList ( );
					allPropInfoList.AddRange ( AimMetadata.GetAimPropInfos ( propInfo.TypeIndex ) );
					AimPropInfoList simplePropList = new AimPropInfoList ( );
					AimPropInfoList complexPropList = new AimPropInfoList ( );
					string columns = SeparateProperties ( allPropInfoList, simplePropList, complexPropList );
					IDbCommand dbCommand = _connection.CreateCommand();
					string idList = ( propList [ 0 ] as DBEntity ).Id.ToString ( );
					for ( int i = 1; i < propList.Count; i++ )
					{
						idList += ", " + ( propList [ i ] as DBEntity ).Id.ToString ( );
					}
					dbCommand.CommandText = string.Format ( "SELECT {0} FROM \"obj_{1}\" WHERE \"Id\" in ({2})", 
															columns, AimMetadata.GetAimTypeName ( propInfo.TypeIndex ), idList );
					IDataReader dataReader = dbCommand.ExecuteReader ( );
					int index = 0;
					propList.Clear ( );
					while ( dataReader.Read ( ) )
					{
						AimObject aimObj = AimObjectFactory.Create ( propInfo.TypeIndex );
						ReadSimpleProperties ( ( DBEntity ) propList [ index ], dataReader, simplePropList );
						propList.Add ( aimObj );
					}
				}
			}
			else
			{

			}
		}

		private void ReadDeltaTable ( IDataReader dataReader, List<ChangeIdentifier> list )
		{
			while ( dataReader.Read ( ) )
			{
				ChangeIdentifier changeIdentifier = new ChangeIdentifier ( );
				changeIdentifier.Id = ( long ) dataReader [ 0 ];

				changeIdentifier.TimeSlice = new TimeSlice ( );
				changeIdentifier.TimeSlice.Interpretation = ( TimeSliceInterpretationType ) ( ( int ) ( dataReader [ 1 ] ) );
				changeIdentifier.TimeSlice.SequenceNumber = ( int ) dataReader [ 2 ];
				changeIdentifier.TimeSlice.CorrectionNumber = ( int ) dataReader [ 3 ];
				
				changeIdentifier.TimeSlice.ValidTime = new TimePeriod ( );
				changeIdentifier.TimeSlice.ValidTime.BeginPosition = ( DateTime ) dataReader [ 4 ];
				object obj = dataReader [ 5 ];
				if ( !DBNull.Value.Equals ( obj ) )
				{
					changeIdentifier.TimeSlice.ValidTime.EndPosition = ( DateTime ) obj;
				}

				obj = dataReader [ 6 ];
				if ( !DBNull.Value.Equals ( obj ) )
				{
					changeIdentifier.CreatedOn = ( DateTime ) obj;
				}

				obj = dataReader [ 7 ];
				if ( !DBNull.Value.Equals ( obj ) )
				{
					changeIdentifier.UserName = ( string ) obj;
				}

				list.Add ( changeIdentifier );
			}
		}

        private void AddParameterToDBCommand ( IDbCommand command, byte [] geomByteArray )
        {
            IDataParameter dataParam = command.CreateParameter ();
            dataParam.DbType = DbType.Binary;
            dataParam.ParameterName = "geomByteA";
            dataParam.Value = geomByteArray;
            command.Parameters.Add ( dataParam );
        }

		private void LoadComplexPropertiesOfFeature ( IList featList, AimPropInfoList complexPropList, bool onlyBaseComplexProps )                      
        {
            // Get complex (object) proprties to be fulled
            for ( int i = 0; i < featList.Count; i++ )
            {
                object featVersion = featList [i];
				LoadComplexPropsViaReflection ( featVersion, complexPropList, onlyBaseComplexProps );
            }
        }
		
		private void LoadAllComplexPropsOfObj ( object obj, AimPropInfo propInfo)
		{
			if ( obj == null || ( propInfo.IsList && ( ( IList ) obj ).Count == 0 ) )
				return;

			if ( propInfo.PropType.Properties.Count == 0 || AimMetadata.IsChoice ( propInfo.TypeIndex ) )
				return;

			var complexPropList = new AimPropInfoList ( );
			foreach ( var item in propInfo.PropType.Properties )
			{
				if ( item.PropType.AimObjectType != AimObjectType.Field && item.PropType.AimObjectType != AimObjectType.DataType)
					complexPropList.Add ( item );
			}
			
			if ( propInfo.IsList )
			{
				IList objList = ( IList ) obj;
				foreach ( var item in objList )
				{
					LoadComplexPropsViaReflection ( item, complexPropList, false );
				}
			}
			else
			{
				LoadComplexPropsViaReflection ( obj, complexPropList, false );
			}
		}

		private void LoadComplexPropsViaReflection ( object obj, AimPropInfoList complexPropList, bool onlyBaseComplexProps )
		{
			foreach ( var prop in complexPropList )
			{
				PropertyInfo netPropInfo = obj.GetType ( ).GetProperty ( prop.Name );
				object complexObj = netPropInfo.GetValue ( obj, null );
				if ( ! onlyBaseComplexProps )
					LoadAllComplexPropsOfObj ( complexObj, prop );
			}
		}

        #region IDataListener Members

        bool IDataListener.GetObject( AObject aObject, Aran.Aim.AObjectFilter refInfo )
        {
            string dbEntityName;
            IAimObject refObj = AimObjectFactory.Create ( refInfo.Type );
            if ( refObj.AimObjectType == AimObjectType.Object )
                dbEntityName = "obj_" + ( refObj as AObject ).ObjectType.ToString ();
            else
                dbEntityName = "bl_" + ( refObj as Feature ).FeatureType.ToString ();
            AimPropInfo propInfo = GetPropInfo ( refInfo.Type, refInfo.Property );
            string colName = propInfo.Name;
            string sql = string.Format ( "SELECT \"{0}\" FROM \"{1}\" " + 
                                            "WHERE \"Id\" = {2}",
                                            colName, dbEntityName, refInfo.Id );
            IDbCommand command = _connection.CreateCommand ();
            IDataReader dataReader = null;
            try
            {
                if ( AimMetadata.IsChoice ( propInfo.TypeIndex ) )
                {
                    dbEntityName = aObject.ObjectType.ToString ();
                    command.CommandText = string.Format ( "SELECT prop_type, choice_type, target_guid, target_id FROM \"{0}\" " + 
                                                            "WHERE \"Id\" = ({1})",
                                                            dbEntityName, sql );

                    ForDebugCmdText += command.CommandText + "\r\n----------------\r\n";

                    dataReader = command.ExecuteReader ();
                    ChoiceRef choiceRef = null;
                    if ( dataReader.Read () )
                        choiceRef = GetChoiceRef ( dataReader );
                    dataReader.Close ();
                    if ( choiceRef == null )
                    {
                        aObject = null;
                        return false;
                    }
                    FillChoiceClass ( choiceRef, aObject );
                }
                else
                {
                    dbEntityName = "obj_" + aObject.ObjectType.ToString ();
                    AimPropInfoList allPropInfoList = new AimPropInfoList ();
                    allPropInfoList.AddRange ( AimMetadata.GetAimPropInfos ( aObject ) );
                    AimPropInfoList simplePropList = new AimPropInfoList ();
                    AimPropInfoList complexPropList = new AimPropInfoList ();
                    string columns = SeparateProperties ( allPropInfoList, simplePropList, complexPropList );
                    command.CommandText = string.Format ( "SELECT {0} FROM \"{1}\" " + 
                                                            "WHERE \"Id\" = ({2})",
                                                            columns, dbEntityName, sql );

                    ForDebugCmdText += command.CommandText + "\r\n----------------\r\n";

                    dataReader = command.ExecuteReader ();
                    if ( dataReader.Read () )
                        ReadSimpleProperties ( aObject, dataReader, simplePropList );
					dataReader.Close ( );
                }				
                return true;
            }
            catch ( Exception ex )
            {
				dataReader.Close ( );
                string message = CommonData.GetErrorMessage ( ex );
                throw new Exception ( message );
            }
        }

        AObjectList<T> IDataListener.GetObjects<T> ( Aran.Aim.AObjectFilter refInfo )
        {
            T obj = new T ();
            string refDbEntityName;
            IAimObject refObj = AimObjectFactory.Create ( refInfo.Type );
			AimPropInfo propInfo = GetPropInfo ( refInfo.Type, refInfo.Property );
            if ( refObj.AimObjectType == AimObjectType.Object )
            {
                refDbEntityName = "obj_" + ( refObj as AObject ).ObjectType.ToString ();
            }
            else
            {
				refDbEntityName = "bl_" + ( refObj as Feature ).FeatureType.ToString ( );
            }
            if ( propInfo == null )
                return new AObjectList<T> ();

            IList list = AimObjectFactory.CreateList ( propInfo.TypeIndex );            
            IDbCommand command = _connection.CreateCommand ();
			IDataReader dataReader = null;
			try
			{
				if ( AimMetadata.IsAbstractFeatureRefObject ( propInfo.TypeIndex ) )
				{
					command.CommandText = string.Format ( "SELECT targetTableIndex, target_guid FROM \"{0}_link\" " +
														"WHERE prop_Index = {1} AND \"{0}_id\" = {2}",
															 refDbEntityName,
															 refInfo.Property,
															 refInfo.Id );

                    ForDebugCmdText += command.CommandText + "\r\n----------------\r\n";

					dataReader = command.ExecuteReader ( );

					while ( dataReader.Read ( ) )
					{
						T absFeatRefObj = new T ( );

						AimPropInfo [ ] propInfos = AimMetadata.GetAimPropInfos ( absFeatRefObj );
						int featureRefTypeIndex = -1;

						foreach ( var item in propInfos )
						{
							if ( item.Index == ( int ) PropertyAbstractFeatureRefObject.Feature )
							{
								featureRefTypeIndex = item.TypeIndex;
								break;
							}
						}

						AimObject frAimObj = AimObjectFactory.Create ( featureRefTypeIndex );
						IAbstractFeatureRef absFeatRef = ( frAimObj as IAbstractFeatureRef );
						absFeatRef.FeatureTypeIndex = Convert.ToInt32 ( dataReader [ 0 ] );
						absFeatRef.Identifier = dataReader.GetGuid ( 1 );
						IAimProperty aimProp = ( IAimProperty ) frAimObj;
						( ( IAimObject ) absFeatRefObj ).SetValue ( ( int ) PropertyAbstractFeatureRefObject.Feature, aimProp );
						list.Add ( absFeatRefObj );
					}
				}
				else if ( propInfo.IsFeatureReference )
				{
					command.CommandText = string.Format ( "SELECT target_guid FROM \"{0}_link\" " +
														"WHERE prop_Index = {1} AND \"{0}_id\" = {2}",
															   refDbEntityName,
															   refInfo.Property,
															   refInfo.Id );

                    ForDebugCmdText += command.CommandText + "\r\n----------------\r\n";

					dataReader = command.ExecuteReader ( );
					while ( dataReader.Read ( ) )
					{
						FeatureRefObject featRef = new FeatureRefObject ( );
						featRef.Feature = new FeatureRef ( );
						featRef.Feature.Identifier = ( Guid ) dataReader [ 0 ];
						list.Add ( featRef );
					}
				}
				else if ( AimMetadata.IsChoice ( propInfo.TypeIndex ) )
				{
					command.CommandText = string.Format ( "SELECT prop_type, choice_type, target_guid, target_id FROM \"{0}\" " +
														"WHERE \"Id\" in (SELECT target_id FROM \"{1}_link\" " +
																			"WHERE \"{1}_id\" = {2}  AND prop_index = {3})",
																				obj.ObjectType,
																				refDbEntityName,
																				refInfo.Id,
																				refInfo.Property );

                    ForDebugCmdText += command.CommandText + "\r\n----------------\r\n";

					dataReader = command.ExecuteReader ( );
					ChoiceRef choiceRef;
					while ( dataReader.Read ( ) )
					{
						choiceRef = GetChoiceRef ( dataReader );
						AObject aObj = ( AObject ) AimObjectFactory.Create ( propInfo.TypeIndex );
						FillChoiceClass ( choiceRef, aObj );
						list.Add ( aObj );
					}
				}
				else
				{
					string tableName = "obj_" + obj.ObjectType.ToString ( );

					AimPropInfoList allPropInfoList = new AimPropInfoList ( );
					allPropInfoList.AddRange ( AimMetadata.GetAimPropInfos ( obj ) );
					AimPropInfoList simplePropList = new AimPropInfoList ( );
					AimPropInfoList complexPropList = new AimPropInfoList ( );

                    #region Replaced
                    //string columns = SeparateProperties ( allPropInfoList, simplePropList, complexPropList );

                    //command.CommandText = string.Format ( "SELECT {0} FROM \"{1}\" " +
                    //                                    "WHERE \"Id\" in (SELECT target_id FROM \"{2}_link\" " +
                    //                                                        "WHERE prop_Index = {3} AND " +
                    //                                                                "\"{2}_id\" = {4} ) order by \"Id\" asc",
                    //                                                                    columns,
                    //                                                                    tableName,
                    //                                                                    refDbEntityName,
                    //                                                                    refInfo.Property,
                    //                                                                    refInfo.Id );
                    #endregion

                    string columns = SeparateProperties (allPropInfoList, simplePropList, complexPropList, tableName);

                    command.CommandText = string.Format (
                        "SELECT {0} FROM \"{1}\", \"{2}_link\" WHERE " +
                        "\"{1}\".\"Id\" = \"{2}_link\".target_id AND " +
                        "\"{2}_link\".prop_Index = {3} AND " +
                        "\"{2}_link\".\"{2}_id\" = {4}",
                        columns,
                        tableName,
                        refDbEntityName,
                        refInfo.Property,
                        refInfo.Id);

                    ForDebugCmdText += command.CommandText + "\r\n----------------\r\n";

                    dataReader = command.ExecuteReader ( );
					list = GetDBEntityList ( ( int ) obj.ObjectType, dataReader, simplePropList );
				}
				dataReader.Close ( );
				return ( list as AObjectList<T> );
			}
			catch ( Exception ex )
			{
				dataReader.Close ( );
				string message = CommonData.GetErrorMessage ( ex );
				throw new Exception ( message );
			}
        }



        AObject IDataListener.GetAbstractObject ( Aran.Aim.AbstractType abstractType, Aran.Aim.AObjectFilter refInfo )
        {
            AimPropInfo propInfo = GetPropInfo ( refInfo.Type, refInfo.Property );
            string tableName;
            if ( AimMetadata.GetAimObjectType ( refInfo.Type ) == AimObjectType.Feature )
                tableName = "bl_";
            else
                tableName = "obj_";
            tableName += AimMetadata.GetAimTypeName ( refInfo.Type );
            IDbCommand command = _connection.CreateCommand ();
            command.CommandText = string.Format ( "SELECT \"{0}\", \"ref_{0}\" FROM \"{1}\" " +
                                                    "WHERE \"Id\" = {2}",
                                                    propInfo.Name,
                                                    tableName,
                                                    refInfo.Id );
			IDataReader dataReader = null;
			try
            {
                dataReader = command.ExecuteReader ();
                if ( dataReader.Read () )
                {
                    int refId, refType;
                    object obj = dataReader [0];
                    if ( DBNull.Value.Equals ( obj ) )
                    {
                        dataReader.Close ();
                        return null;
                    }
                    refId = ( int ) obj;
                    obj = dataReader [1];
                    if ( DBNull.Value.Equals ( obj ) )
                    {
                        dataReader.Close ();
                        return null;
                    }
                    refType = ( int ) obj;
                    dataReader.Close ();

                    AimObject result = AimObjectFactory.Create ( refType );
                    tableName = "obj_" + ( ( AObject ) result ).ObjectType;

                    AimPropInfoList allPropInfoList = new AimPropInfoList ();
                    allPropInfoList.AddRange ( AimMetadata.GetAimPropInfos ( refType ) );
                    AimPropInfoList simplePropList = new AimPropInfoList ();
                    AimPropInfoList complexPropList = new AimPropInfoList ();
                    string columns = SeparateProperties ( allPropInfoList, simplePropList, complexPropList );

                    command.CommandText = string.Format ( "SELECT {0} FROM \"{1}\" " + 
                                                            "WHERE \"Id\" = {2}",
                                                            columns,
                                                            tableName,
                                                            refId );
                    dataReader = command.ExecuteReader ();
                    if ( dataReader.Read () )
                        ReadSimpleProperties ( ( DBEntity ) result, dataReader, simplePropList );
                    dataReader.Close ();
                    return ( AObject ) result;
                }
                return null;
            }
            catch ( Exception ex )
            {
				dataReader.Close ( );
                string message = CommonData.GetErrorMessage ( ex );
                throw new Exception ( message );
            }
        }

        IList IDataListener.GetAbstractObjects ( Aran.Aim.AbstractType abstractType, Aran.Aim.AObjectFilter refInfo )
        {
            throw new NotImplementedException ();
        }

        #endregion

        /// <summary>
        /// Seperate property list into simple and complex (object) property list and returns sql column names. 
        /// </summary>
        /// <param name="allPropInfoList"></param>
        /// <param name="simplePropList"></param>
        /// <param name="complexPropList"></param>
        /// <param name="dbEntityName">Table name. Add this prefix to columns to identify the table which column belongs to</param>
        /// <returns>Returns sql column names for simple properties</returns>
        private string SeparateProperties ( AimPropInfoList allPropInfoList, AimPropInfoList simplePropList,
                                                            AimPropInfoList complexPropList, string dbEntityName = "" )
        {
            List<string> columnList=  new List<string> ();
            string colName;
            string lowerCasePropName;
            _columnIndices.Clear ();
            foreach ( AimPropInfo propInfo in allPropInfoList )
            {
                #region Find complex (object) properties and return to beginning
                if ( propInfo.IsList || AimMetadata.IsAbstract ( propInfo.TypeIndex ) )
                {
                    complexPropList.Add ( propInfo );
                    continue;
                }
                IAimProperty aimProp = ( IAimProperty ) AimObjectFactory.Create ( propInfo.TypeIndex );
                if ( aimProp.PropertyType == AimPropertyType.Object )
                {
                    complexPropList.Add ( propInfo );
                    continue;
                }
                #endregion

                lowerCasePropName = propInfo.Name.ToLower ();
                // These are mandatory fields and that is why these will be added later
                // Also these are only feature properties
                if ( lowerCasePropName == "identifier" || lowerCasePropName == "timeslice")
                    continue;

                simplePropList.Add ( propInfo );
                if ( lowerCasePropName == "geo")
                {
                    if ( dbEntityName != "" )
                        columnList.Add ( string.Format ( "ST_asEWKB(\"{0}\".\"Geo\"::geometry)", dbEntityName ) );
                    else
                        columnList.Add ( string.Format ( "ST_asEWKB(\"Geo\"::geometry)" ) );
                    _columnIndices.Add ( propInfo, columnList.Count - 1 );
                }
                else if ( AimMetadata.IsAbstractFeatureRef ( propInfo.TypeIndex ) )
                {
                    colName = "";
                    if ( dbEntityName != "" )
                        colName += string.Format ( "\"{0}\".", dbEntityName );
                    colName += string.Format ( "\"ref_{0}\"", propInfo.Name );
                    columnList.Add ( colName );

                    colName = "";
                    if ( dbEntityName != "" )
                        colName += string.Format ( "\"{0}\".", dbEntityName );
                    colName += string.Format ( "\"{0}\"", propInfo.Name );
                    columnList.Add ( colName );
                    _columnIndices.Add ( propInfo, columnList.Count - 1 );
                }
                else if ( AimMetadata.IsValClass ( propInfo.TypeIndex ) )
                {
                    colName = "";
                    if ( dbEntityName != "" )
                        colName  += string.Format ( "\"{0}\".", dbEntityName );
                    colName += string.Format ( "\"{0}\"", propInfo.Name + "_Uom");                    
                    columnList.Add ( colName );

                    colName = "";
                    if ( dbEntityName != "" )
                        colName  += string.Format ( "\"{0}\".", dbEntityName );
                    colName += string.Format ( "\"{0}\"", propInfo.Name + "_Value" );
                    columnList.Add ( colName );
                    _columnIndices.Add ( propInfo, columnList.Count - 1 );
                }
                else if ( propInfo.TypeIndex == (int) DataType.TextNote )
                {
                    // this is TextNote case
                    colName = "";
                    if ( dbEntityName != "" )
                        colName  += string.Format ( "\"{0}\".", dbEntityName );
                    colName += string.Format ( "\"{0}\"", propInfo.Name + "_Lang" );
                    columnList.Add ( colName );

                    colName = "";
                    if ( dbEntityName != "" )
                        colName  += string.Format ( "\"{0}\".", dbEntityName );
                    colName += string.Format ( "\"{0}\"", propInfo.Name + "_Value" );
                    columnList.Add ( colName );
                    _columnIndices.Add ( propInfo, columnList.Count - 1 );
                }
                else
                {
                    colName = "";
                    if ( dbEntityName != "" )
                        colName  += string.Format ( "\"{0}\".", dbEntityName );
                    colName += string.Format ( "\"{0}\"", propInfo.Name );
                    columnList.Add ( colName );
                    _columnIndices.Add ( propInfo, columnList.Count - 1 );
                }                
            }
            if ( columnList.Count > 0 )
            {
                string result = "";
                for ( int i = 0; i < columnList.Count; i++ )
                {
                    result += columnList [i] + ",\r\n";
                }
                return result.Remove ( result.Length - 3 );
            }
            return "";
        }

        private string TimeSliceFilterToSqlCommand ( string featName, TimeSliceFilter timeSliceFilter )
        {
            string sqlText = "";
            switch ( timeSliceFilter.QueryType )
            {
                case QueryType.ByEffectiveDate:
                    sqlText = string.Format ( "\"{0}\".begin_valid<'{1}' AND " +
                                                "(\"{0}\".end_valid>'{1}' OR " +
                                                "\"{0}\".end_valid is null)",
                                                featName, timeSliceFilter.EffectiveDate );
                    break;

                case QueryType.ByTimePeriod:

                    sqlText = string.Format ( "(" +
                                                    "(" + 
                                                        "\"{0}\".begin_valid <= '{1}' AND " +
                                                            "(" + 
                                                                "\"{0}\".end_valid >= '{2}' OR " +
                                                                "\"{0}\".end_valid >= '{1}' OR " +
                                                                "\"{0}\".end_valid is null" + 
                                                            ")" + 
                                                    ")" +
                                                    "OR" + 
                                                    "(" +
                                                        "\"{0}\".begin_valid >= '{1}' AND " +
                                                            "(" +
                                                                "\"{0}\".end_valid <= '{2}' OR " +
                                                                "\"{0}\".end_valid is null" + 
                                                            ")" +
                                                    ")" +                         
                                               ")",
                                                featName,
                                                timeSliceFilter.ValidTime.BeginPosition,
                                                timeSliceFilter.ValidTime.EndPosition );
                    break;

                case QueryType.BySequenceNumber:
					sqlText = string.Format ( "\"{0}\".sequence = {1}", featName, timeSliceFilter.SequenceNumber );
                    break;
                default:
                    throw new Exception ( "Not found QueryType !" );
            }
            return sqlText;
        }

        /// <summary>
        /// Adds mandatry columns to feature sql command as "Identifier", "Id", "TimeSlice"(properties) and returns result string
        /// </summary>
        /// <param name="featTypeIndex"></param>
        /// <param name="simplePropList"></param>
        /// <returns></returns>
        private string AddFeatMandatoryColumns ( int featTypeIndex, AimPropInfoList simplePropList)
        {
            AimPropInfo[] allPropInfos = AimMetadata.GetAimPropInfos ( featTypeIndex );
            string columns = "";
            int lastIndexOfColumn = _columnIndices [simplePropList [simplePropList.Count-1]];
            if ( !simplePropList.Exists ( propInfo => propInfo.Name.ToLower () == "id" ) )
            {
                AimPropInfo idPropInfo = GetPropInfo ( allPropInfos, "id" );
                lastIndexOfColumn++;
                _columnIndices.Add ( idPropInfo, lastIndexOfColumn );
                simplePropList.Add ( idPropInfo );
                columns += string.Format ( "\"bl_{0}\".\"Id\",\r\n", AimMetadata.GetAimTypeName ( featTypeIndex ) );
            }

            if ( !simplePropList.Exists ( propInfo => propInfo.Name.ToLower () == "identifier" ) )
            {
                AimPropInfo identifierPropInfo = GetPropInfo ( allPropInfos, "identifier" );
                lastIndexOfColumn++;
                _columnIndices.Add ( identifierPropInfo, lastIndexOfColumn );
                simplePropList.Add ( identifierPropInfo );
                columns += "features.\"Identifier\",\r\n";
            }

            if ( !simplePropList.Exists ( propInfo => propInfo.Name.ToLower () == "timeslice" ) )
            {
                AimPropInfo timeSlicePropInfo = GetPropInfo ( allPropInfos, "timeslice" );
                lastIndexOfColumn++;
                _columnIndices.Add ( timeSlicePropInfo, lastIndexOfColumn );
                simplePropList.Add ( timeSlicePropInfo );
                columns +=  string.Format ( "features.begin_life," + 
                                            "\r\nfeatures.end_life," + 
                                            "\r\n\"bl_{0}\".sequence," + 
                                            "\r\n\"bl_{0}\".correction," + 
                                            "\r\n\"bl_{0}\".begin_valid," + 
                                            "\r\n\"bl_{0}\".end_valid,\r\n", AimMetadata.GetAimTypeName ( featTypeIndex ) );
            }
            if ( columns != "" )
            {
                return columns.Remove ( columns.Length - 3 );
            }
            return "";
        }

        private void ReadSimpleProperties ( DBEntity dbEntity, IDataReader dataReader, AimPropInfoList simplePropInfoList ) 
        {
            string fieldName;
            IAimObject dbEntityObj = ( dbEntity as IAimObject );
            object tmpVal;
            ( dbEntity as IEditDBEntity ).Listener = this;
            bool hasFoundProp;
            int columnIndex;
            foreach ( AimPropInfo propInfo in simplePropInfoList )
            {
                AimObject aimObj = AimObjectFactory.Create ( propInfo.TypeIndex );
                IAimProperty aimProp = ( aimObj as IAimProperty );
                fieldName = propInfo.Name;
                hasFoundProp = false;
                if ( aimProp.PropertyType == AimPropertyType.AranField )
                {
                    AimField aimField = ( aimProp as AimField );
                    IEditAimField editAimField = ( aimProp as IEditAimField );
                    columnIndex = _columnIndices [propInfo];
                    tmpVal = dataReader [( int ) columnIndex];
                    if ( fieldName == "Geo" )
                    {
                        if ( !DBNull.Value.Equals ( tmpVal ) )
                        {
                            byte[] bytea = ( byte [] ) tmpVal;
                            GeometryWKBReader wkbReader = new GeometryWKBReader ();
                            Aran.Geometries.Geometry geom = wkbReader.Create ( bytea );
                            editAimField.FieldValue = geom;
                            dbEntityObj.SetValue ( propInfo.Index, aimProp );
                        }
                    }
                    else
                    {
                        if ( !DBNull.Value.Equals ( tmpVal ) )
                        {
							if ( propInfo.TypeIndex == ( int ) AimFieldType.SysUInt32 )
								tmpVal = Convert.ToUInt32 ( tmpVal );
							editAimField.FieldValue =  tmpVal;
                            dbEntityObj.SetValue ( propInfo.Index, aimProp );
                        }
                    }
                    hasFoundProp = true;
                }
                else if ( aimProp.PropertyType == AimPropertyType.DataType )
                {
                    if ( propInfo.AixmName == "timeSlice" )
                    {
                        TimeSlice timeSlice = ( aimProp as TimeSlice );
                        columnIndex = _columnIndices [propInfo];
                        // columnIndex is begin_life field index
                        // index of end_life is columnIndex + 1
                        // index of sequence is columnIndex + 2
                        // index of correction is columnIndex + 3
                        // index of begin_valid is columnIndex + 4 
                        // index of end_valid is columnIndex + 5
                        timeSlice.SequenceNumber = ( int ) dataReader [ columnIndex + 2 ];
                        timeSlice.CorrectionNumber = ( int ) dataReader [ columnIndex + 3];
                        timeSlice.ValidTime = new TimePeriod ();
                        timeSlice.ValidTime.BeginPosition = ( DateTime ) dataReader [ columnIndex + 4];
                        tmpVal = dataReader [ columnIndex + 5];
                        if ( !DBNull.Value.Equals ( tmpVal ) )
                            timeSlice.ValidTime.EndPosition = ( DateTime ) tmpVal;
                        timeSlice.FeatureLifetime = new TimePeriod ();
                        timeSlice.FeatureLifetime.BeginPosition = ( DateTime ) dataReader [columnIndex];
                        tmpVal = dataReader [ columnIndex + 1];
                        if ( !DBNull.Value.Equals ( tmpVal ) )
                            timeSlice.FeatureLifetime.EndPosition = ( DateTime ) tmpVal;
                        dbEntityObj.SetValue ( propInfo.Index, timeSlice );
                        hasFoundProp = true;
                    }
                    else if ( AimMetadata.IsAbstractFeatureRef ( propInfo.TypeIndex ) )
                    {
                        IAbstractFeatureRef absFeatRef = ( aimProp as IAbstractFeatureRef );
                        columnIndex = _columnIndices [propInfo];
                        // columnIndex is column index and columnIndex - 1 is ref_ColumnName(column of type of abstract)
                        tmpVal = dataReader [( int ) columnIndex];
                        if ( !DBNull.Value.Equals ( tmpVal ) )
                        {
                            absFeatRef.Identifier = ( Guid ) tmpVal;
                            tmpVal = dataReader [columnIndex - 1];
                            absFeatRef.FeatureTypeIndex = ( int ) tmpVal;
                            dbEntityObj.SetValue ( propInfo.Index, aimProp );
                        }
                        hasFoundProp = true;
                    }
                    else if ( aimProp is IEditValClass )
                    {
                        IEditValClass valClass = ( aimProp as IEditValClass );
                        columnIndex = _columnIndices [propInfo];
                        int foundDataTypeProp = 0;
                        tmpVal = dataReader [(int) columnIndex];
                        if ( !DBNull.Value.Equals ( tmpVal ) )
                        {
							valClass.Value = Convert.ToDouble ( tmpVal );
                            foundDataTypeProp++;
                        }
                        tmpVal = dataReader [( int ) columnIndex-1];
                        if ( !DBNull.Value.Equals ( tmpVal ) )
                        {
                            valClass.Uom = ( int ) tmpVal;
                            foundDataTypeProp++;
                        }
                        if (foundDataTypeProp == 2)
                            dbEntityObj.SetValue ( propInfo.Index, aimProp );
                        hasFoundProp = true;
                    }
                    else if ( propInfo.IsFeatureReference )
                    {
                        FeatureRef featRef = ( aimProp as FeatureRef );
                        columnIndex = _columnIndices [propInfo];
                        tmpVal = dataReader [( int ) columnIndex];
                        if ( !DBNull.Value.Equals ( tmpVal ) )
                        {
                            featRef.Identifier = ( Guid ) tmpVal;
                            dbEntityObj.SetValue ( propInfo.Index, featRef );
                        }
                        hasFoundProp = true;
                    }
                    else
                    {
                        TextNote txtNote = ( aimProp as TextNote );
                        columnIndex = _columnIndices [propInfo];
                        tmpVal = dataReader [( int ) columnIndex];
                        if ( !DBNull.Value.Equals ( tmpVal ) )
                        {
                            txtNote.Value = ( string ) tmpVal;
                        }
                        tmpVal = dataReader [( int ) columnIndex-1];
                        if ( !DBNull.Value.Equals ( tmpVal ) )
                        {
                            txtNote.Lang = ( language ) tmpVal;
                        }
                        dbEntityObj.SetValue ( propInfo.Index, txtNote );
                        hasFoundProp = true;
                    }
                }
                if ( !hasFoundProp )
                    throw new Exception ( "This property has not found (" + propInfo.Name + ")" );
            }
        }

        private IList GetDBEntityList ( int dbEntityTypeIndex, IDataReader dataReader, 
                                        AimPropInfoList propInfoList, string geogTableName = "") 
        {
            IList result = AimObjectFactory.CreateList ( dbEntityTypeIndex );
            while ( dataReader.Read())
            {
                AimObject aObj = AimObjectFactory.Create ( dbEntityTypeIndex );
                ReadSimpleProperties ( ( DBEntity ) aObj, dataReader, propInfoList );
                result.Add ( aObj );
            }
            return result;
        }

        private void FillChoiceClass ( ChoiceRef choiceRef, AObject aObject ) 
        {
            IEditChoiceClass editChoiceObj = ( IEditChoiceClass ) aObject;
            editChoiceObj.RefType = choiceRef.PropType;
            if ( choiceRef.IsFeature )
            {
                editChoiceObj.RefValue = ( IAimProperty ) choiceRef.AimObj;
            }
            else
            {
                editChoiceObj.RefValue = ( IAimProperty ) choiceRef.AimObj;
                string tableName = "obj_" + AimMetadata.GetAimTypeName ( choiceRef.ValueType );
                IDbCommand command = _connection.CreateCommand ();

                AimPropInfoList allPropInfoList = new AimPropInfoList ();
                allPropInfoList.AddRange ( AimMetadata.GetAimPropInfos ( choiceRef.ValueType ) );
                AimPropInfoList simplePropList = new AimPropInfoList ();
                AimPropInfoList complexPropList = new AimPropInfoList ();
                string columns = SeparateProperties ( allPropInfoList, simplePropList, complexPropList );

                command.CommandText = string.Format ( "SELECT {0} FROM \"{1}\" " + 
                                                        "WHERE \"Id\" = {2}",
                                                        columns, tableName, choiceRef.Id );
                IDataReader dataReader = command.ExecuteReader ();
                if ( dataReader.Read () )
                    ReadSimpleProperties ( ( DBEntity ) editChoiceObj.RefValue, dataReader, simplePropList );
                dataReader.Close ();
            }
        }

        private ChoiceRef GetChoiceRef (IDataReader dataReader ) 
        {
            ChoiceRef result = new ChoiceRef ();
            result.PropType = ( int ) dataReader ["prop_type"];
            result.ValueType = ( int ) dataReader ["choice_type"];
            if ( AimMetadata.GetAimObjectType ( result.ValueType ) == AimObjectType.Feature )
            {
                result.IsFeature = true;
                Guid identifier = ( Guid ) dataReader ["target_guid"];
                if ( AimMetadata.IsAbstractFeatureRef ( result.PropType ) )
                {
                    result.AimObj = AimObjectFactory.Create ( result.PropType );
                    ( result.AimObj as IAbstractFeatureRef ).Identifier = identifier;
                    ( result.AimObj as IAbstractFeatureRef ).FeatureTypeIndex = result.ValueType;
                }
                else
                    result.AimObj = new FeatureRef ( identifier );
            }
            else
            {
                result.AimObj = AimObjectFactory.Create ( result.ValueType );                    
                result.IsFeature = false;                    
                result.Id = ( long ) dataReader ["target_id"];
            }
            return result;
        }

        /// <summary>
        /// Returns AimPropInfo whose name is "lowerCaseName"
        /// </summary>
        /// <param name="propInfos"></param>
        /// <param name="lowerCaseName"></param>
        /// <returns></returns>
        private AimPropInfo GetPropInfo ( AimPropInfo [] propInfos, string lowerCaseName )
        {
            foreach ( AimPropInfo propInfo in propInfos )
            {
                if ( propInfo.Name.ToLower () == lowerCaseName )
                    return propInfo;
            }
            return null;
        }

        private AimPropInfo GetPropInfo ( int aimObjIndex, int propInfoIndex ) 
        {
            AimPropInfo[] propInfos = AimMetadata.GetAimPropInfos ( aimObjIndex );
            foreach ( AimPropInfo propInfo in propInfos )
            {
                if ( propInfo.Index == propInfoIndex )
                {
                    return propInfo;
                }
            }            
            return null;
        }

        private IDbConnection _connection;
        private TimeSliceFilter _timeSliceFilter;
        private Dictionary<AimPropInfo, int> _columnIndices;
	}
}