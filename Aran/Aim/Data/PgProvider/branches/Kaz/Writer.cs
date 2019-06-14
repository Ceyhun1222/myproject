using System;
using System.Collections.Generic;
using Aran.Aim.Features;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Enums;
using Aran.Aim.DataTypes;
using System.Collections;
using Aran.Aim.Objects;
using System.Data;
using Aran.Geometries.IO;
using Aran.Geometries;
using Aran.Converters;
using Aran.Aim.Data.Filters;

namespace Aran.Aim.Data
{
	internal class Writer
	{
		internal Writer ( IDbConnection connection )
		{
			_connection = connection;
			byteaGeomParamName = "geomByteA";
			_dbCommandDictionary = new Dictionary<int, List<IDbCommand>> ( );
			_transactionIndex = 0;
		}

		/// <summary>
		/// Inserts feature into AIM Database.
		/// </summary>
		/// <param name="insertAnyway">If INSERTAnyway is true then tries to INSERT it INTO AIM_DB as 
		/// new sequence number even though it is incompatible with time slice in database
		/// </param>
		/// <param name="asCorrection">If asCorrection is true then tries INSERT it as correction of last sequence number in database
		/// </param>
		internal InsertingResult Insert ( Feature feat, int transactionId, bool asCorrection )
		{
			IDbCommand dbCommand = null;
			try
			{
				if ( !_dbCommandDictionary.ContainsKey ( transactionId ) )
				{
					return new InsertingResult ( false, "Transaction Id is not exists !" );
				}
				List<IDbCommand> cmdList = _dbCommandDictionary [ transactionId ];
				if ( cmdList.Count == 0 )
				{
					dbCommand = CreateCommand ( _connection.BeginTransaction ( ) );
					cmdList.Add ( dbCommand );
				}
				else
				{
					dbCommand = cmdList [ 0 ];
				}

				dbCommand.Parameters.Clear ( );

				//int sequence = 0;
				//int correction = 0;
				//string result = GetCurrVersion ( feat, ref sequence, ref correction );
				string result;
				//if ( result != string.Empty )
				//{
				//    return new InsertingResult ( false, result );
				//}
				//result = AdaptFeatTimeSlice ( feat, insertAnyway, asCorrection, sequence, correction );
				//if ( result != string.Empty )
				//{
				//    return new InsertingResult ( false, result );
				//}

				//List<int> diffIndexList = new List<int> ( );
				//result = GetDifferences ( feat, diffIndexList );
				//if ( result != string.Empty )
				//{
				//    return new InsertingResult ( false, result );
				//}

				IAimObject aimObject = feat as IAimObject;
				string sqlText = string.Empty;
				List<string> sqlTextInsertObjs_in2_LinkTable = new List<string> ( );
				List<string> sqlTextInsertFeats_in2_LinkTable = new List<string> ( );

				//if ( feat.TimeSlice.SequenceNumber == 1 )
				{
					if ( !asCorrection )
					{
						result = InsertIntofeaturesTable ( feat, dbCommand.Transaction );
						if ( result != string.Empty )
						{
							dbCommand.Transaction.Rollback ( );
							return new InsertingResult ( false, result );
						}
					}
				}

				byte [] geomByteArray;
				//sqlText = CreateSqlText_4_DBEntity ( aimObject, diffIndexList, feat.TimeSlice.CorrectionNumber,
				//                                    sqlTextInsertObjs_in2_LinkTable, sqlTextInsertFeats_in2_LinkTable,
				//                                    dbCommand.Transaction, out geomByteArray );

				sqlText = CreateSqlText_4_DBEntity ( aimObject, asCorrection, sqlTextInsertObjs_in2_LinkTable, sqlTextInsertFeats_in2_LinkTable,
													dbCommand.Transaction, out geomByteArray );

				//if ( feat.TimeSlice.Interpretation == TimeSliceInterpretationType.PERMDELTA ||
				//        feat.TimeSlice.Interpretation == TimeSliceInterpretationType.BASELINE )
				{
					dbCommand.CommandText = sqlText;
					dbCommand.ExecuteNonQuery ( );

					//string WHEREClause = string.Format ( " sequence = {0} AND feat_id = (SELECT \"Id\" FROM features WHERE \"Identifier\" = '{1}')",
					//                                        feat.TimeSlice.SequenceNumber, feat.Identifier );
					string WHEREClause = string.Format ( " feat_id = (SELECT \"Id\" FROM features WHERE \"Identifier\" = '{0}')",
										feat.Identifier );
					long newFeatureId = GetFeatId ( "bl_" + feat.FeatureType, WHEREClause, dbCommand.Transaction );
					feat.Id = newFeatureId;

					if ( sqlTextInsertObjs_in2_LinkTable.Count > 0 || sqlTextInsertFeats_in2_LinkTable.Count > 0 )
					{
						InserLinkTable ( newFeatureId, "bl_" + feat.FeatureType,
							sqlTextInsertObjs_in2_LinkTable, sqlTextInsertFeats_in2_LinkTable, dbCommand.Transaction );
					}
				}
				//_dbCommandDictionary [ transactionId ].Add ( dbCommand );
				return new InsertingResult ( true, string.Empty );
			}
			catch ( Exception except )
			{
				if ( dbCommand != null )
					dbCommand.Transaction.Rollback ( );
				string message = CommonData.GetErrorMessage ( except );
				return new InsertingResult ( false, message );
			}
		}

		internal string Delete ( Feature feature )
		{
			IDbCommand dbCommand = null;
			try
			{
				dbCommand = CreateCommand ( _connection.BeginTransaction ( ) );
				string sqlText = "DELETE FROM features WHERE \"Identifier\" = '" + feature.Identifier + "';" + GetDeletSqlText ( feature as IAimObject );
				dbCommand.CommandText = sqlText;
				dbCommand.ExecuteNonQuery ( );
				dbCommand.Transaction.Commit ( );
				return string.Empty;
			}
			catch ( Exception except )
			{
				if ( dbCommand != null )
					dbCommand.Transaction.Rollback ( );
				string message = CommonData.GetErrorMessage ( except );
				return message;
			}
		}

		internal int BeginTransaction ( )
		{
			_transactionIndex++;
			List<IDbCommand> dbCommandList = new List<IDbCommand> ( );
			_dbCommandDictionary.Add ( _transactionIndex, dbCommandList );
			return _transactionIndex;
		}

		internal InsertingResult Commit ( int transactionId )
		{
			if ( _dbCommandDictionary.ContainsKey ( transactionId ) )
			{
				foreach ( var dbCommand in _dbCommandDictionary [ transactionId ] )
				{
					dbCommand.Transaction.Commit ( );
				}

				return new InsertingResult ( true );
			}
			return new InsertingResult ( false, "Transaction Id is not exists !" );
		}

		internal InsertingResult Rollback ( int transactionId )
		{
			if ( _dbCommandDictionary.ContainsKey ( transactionId ) )
			{
				foreach ( var dbCommand in _dbCommandDictionary [ transactionId ] )
				{
					dbCommand.Transaction.Rollback ( );
				}
			}
			return new InsertingResult ( false, "Transaction Id is not exists !" );
		}

		//private string GetDifferences ( Feature feat, List<int> diffIndexList )
		//{
		//    if ( feat.TimeSlice.SequenceNumber > 1 )
		//    {
		//        TimeSliceFilter timeSliceFilter = new TimeSliceFilter ( feat.TimeSlice.SequenceNumber - 1 );
		//        Reader reader = new Reader ( _connection );
		//        reader.TimeSliceFilter = timeSliceFilter;


		//        GettingResult getResult = reader.VersionsOf ( feat.FeatureType,
		//                                        TimeSliceInterpretationType.BASELINE,
		//                                        feat.Identifier,
		//                                        true,
		//                                        false,
		//                                        null,
		//                                        null,
		//                                        null );
		//        if ( getResult.IsSucceed )
		//        {
		//            if ( getResult.List.Count > 0 )
		//            {
		//                diffIndexList.AddRange ( ( getResult.List [ 0 ] as AimObject ).GetDifferences ( feat, true ) );
		//                if ( AimObject.IsEquals ( ( getResult.List [ 0 ] as Feature ).TimeSlice.FeatureLifetime, feat.TimeSlice.FeatureLifetime ) )
		//                    diffIndexList.Remove ( ( int ) PropertyFeature.TimeSlice );
		//                return string.Empty;
		//            }
		//            return "There is no previous version inserted to compare !";
		//        }
		//        return getResult.Message;

		//    }
		//    else
		//    {
		//        int [] indices = ( feat as IAimObject ).GetPropertyIndexes ( );
		//        diffIndexList.AddRange ( indices );
		//        return string.Empty;
		//    }
		//}

		/// <summary>
		/// Returns error message if something went wrong
		/// Else returns empty string
		/// </summary>
		/// <param name="feat"></param>
		/// <param name="InsertAnyway"></param>
		/// <param name="asCorrection"></param>
		/// <param name="seq"></param>
		/// <param name="corr"></param>
		/// <returns></returns>
		//private string AdaptFeatTimeSlice ( Feature feat, bool InsertAnyway, bool asCorrection, int seq, int corr )
		//{
		//    if ( InsertAnyway )
		//    {
		//        if ( asCorrection )
		//        {
		//            if ( seq != 0 )
		//            {
		//                feat.TimeSlice.SequenceNumber = seq;
		//                feat.TimeSlice.CorrectionNumber = corr + 1;
		//            }
		//            else
		//            {
		//                feat.TimeSlice.SequenceNumber = 1;
		//                feat.TimeSlice.CorrectionNumber = 0;
		//            }
		//        }
		//        else
		//        {
		//            feat.TimeSlice.SequenceNumber = seq + 1;
		//            feat.TimeSlice.CorrectionNumber = 0;
		//        }
		//    }
		//    else if ( !
		//               (
		//                ( feat.TimeSlice.SequenceNumber - seq == 1 && feat.TimeSlice.CorrectionNumber == 0 ) ||
		//                ( feat.TimeSlice.SequenceNumber > 0 && feat.TimeSlice.SequenceNumber == seq && feat.TimeSlice.CorrectionNumber - corr == 1 )
		//               )
		//            )
		//    {
		//        return "TimeSlice property is not compatible with database!";
		//    }
		//    return string.Empty;
		//}

		/// <summary>
		/// Creates commont sql commands for INSERTing to table AND returns its linkTable's INSERTing sql command
		/// </summary>
		/// <param name="sqlInsertObjs_in2_linkTable">Sql texts for INSERTing objects INTO link table.
		/// <param name="sqlInsertFeats_in2_linkTable">Sql texts for INSERTing features INTO link table.
		/// <returns></returns>
		//private string CreateSqlText_4_DBEntity ( IAimObject aimObj,
		//                                        List<int> diffIndexList,
		//                                        int correction,
		//                                        List<string> sqlInsertObj_in2_linkTable,
		//                                        List<string> sqlInsertFeats_in2_linkTable,
		//                                        IDbTransaction transaction,
		//                                        out byte [] byteaGeom )
		private string CreateSqlText_4_DBEntity ( IAimObject aimObj,
												bool doUpdate,
												List<string> sqlInsertObj_in2_linkTable,
												List<string> sqlInsertFeats_in2_linkTable,
												IDbTransaction transaction,
												out byte [] byteaGeom )
		{
			byteaGeom = null;
			List<AimPropInfo> filledPropInfos = GetFilledPropInfos ( aimObj );
			if ( filledPropInfos.Count == 0 )
				return string.Empty;
			string primaryTableName = string.Empty;
			bool isFeature = false;
			Guid guid = new Guid ( );
			if ( aimObj is Feature )
				isFeature = true;
			primaryTableName = CommonData.GetTableName ( aimObj );
			List<string> primaryColumns = new List<string> ( );
			List<string> primaryValues = new List<string> ( );
			int targetTableIndex;
			IAimProperty property;
			object value;
			IEditAimField propEditAimField;
			AimField propAimField;
			//int interpretation = 0, sequence = 0;
			//DateTime begin_valid = DateTime.MaxValue;
			//DateTime? end_valid = null;
			List<long> insertedIds = new List<long> ( );

			//SortedDictionary<int, byte []> deltaPropValues = new SortedDictionary<int, byte []> ( );

			Guid ref_Guid;
			long ref_ID;
			long id = 0;
			DeltaProp deltaProp = new DeltaProp ( );
			foreach ( AimPropInfo propInfo in filledPropInfos )
			{
				property = aimObj.GetValue ( propInfo.Index );
				value = string.Empty;
				if ( propInfo.Name.ToLower ( ) == "onrunway" )
				{
				}
				switch ( property.PropertyType )
				{
					case AimPropertyType.AranField:
						#region AranField

						propEditAimField = property as IEditAimField;
						if ( propInfo.Name.ToLower ( ) == "id" )
						{
							id = ( long ) propEditAimField.FieldValue;
							continue;
						}

						if ( propInfo.AixmName == "identifier" )
						{
							guid = ( Guid ) propEditAimField.FieldValue;
							//if ( diffIndexList.Contains ( propInfo.Index ) )
							//{
							//    deltaPropValues.Add ( propInfo.Index, deltaProp.ToByteArray ( guid ) );
							//}
						}
						else if ( propInfo.Name == "Geo" )
						{
							primaryColumns.Add ( propInfo.Name );
							Geometry geom = ( Aran.Geometries.Geometry ) propEditAimField.FieldValue;
							string pntSql = "ST_GeomFromEWKB(:" + byteaGeomParamName + ")";
							GeometryWKBWriter geomWriter = new Aran.Geometries.IO.GeometryWKBWriter ( );
							geomWriter.Write ( geom, ByteOrder.LittleEndian );
							byteaGeom = geomWriter.GetByteArray ( );
							primaryValues.Add ( pntSql );
						}
						else
						{
							primaryColumns.Add ( propInfo.Name );
							value = propEditAimField.FieldValue;
							propAimField = ( property as AimField );

							//byte [] deltaVal = new byte [ 0 ];
							if ( ( property as AimField ).FieldType == AimFieldType.SysEnum )
							{
								value = ( int ) value;
							}

							//if ( isFeature && diffIndexList.Contains ( propInfo.Index ) )
							//{
							//    deltaPropValues.Add ( propInfo.Index, deltaProp.ToByteArray ( value ) );
							//}

							if ( propAimField.FieldType == AimFieldType.SysString )
								value = "'" + AdaptQuotedStrIntoPg ( ( string ) value ) + "'";
							else if ( propAimField.FieldType == AimFieldType.SysDateTime )
								value = "'" + value + "'";

							primaryValues.Add ( value.ToString ( ) );
						}
						#endregion
						break;

					case AimPropertyType.DataType:
						#region DataType
						if ( AimMetadata.IsAbstractFeatureRef ( propInfo.TypeIndex ) )
						{
							primaryColumns.Add ( propInfo.Name );
							ref_Guid = ( property as FeatureRef ).Identifier;
							primaryValues.Add ( "'" + ref_Guid + "'" );

							primaryColumns.Add ( "ref_" + propInfo.Name );
							primaryValues.Add ( ( property as IAbstractFeatureRef ).FeatureTypeIndex.ToString ( ) );

							//if ( isFeature && diffIndexList.Contains ( propInfo.Index ) )
							//{
							//    deltaPropValues.Add ( propInfo.Index, deltaProp.ToByteArray ( property ) );
							//}
						}
						else if ( propInfo.AixmName == "timeSlice" )
						{
							//TimeSlice timeSlice = ( property as TimeSlice );

							//// It will write featureLifeTime property into deltaProperties table as TimeSlice index 
							//// Because this is changable property
							//if ( diffIndexList.Contains ( propInfo.Index ) )
							//{
							//    deltaPropValues.Add ( propInfo.Index, deltaProp.ToByteArray ( timeSlice.FeatureLifetime ) );
							//}

							//primaryColumns.Add ( "begin_valid" );
							//begin_valid = timeSlice.ValidTime.BeginPosition;
							//primaryValues.Add ( "'" + begin_valid.ToString ( ) + "'" );


							//end_valid = timeSlice.ValidTime.EndPosition;
							//if ( end_valid.HasValue )
							//{
							//    primaryColumns.Add ( "end_valid" );
							//    primaryValues.Add ( "'" + end_valid.Value.ToString ( ) + "'" );
							//}

							//primaryColumns.Add ( "sequence" );
							//sequence = timeSlice.SequenceNumber;
							//primaryValues.Add ( sequence.ToString ( ) );

							//primaryColumns.Add ( "correction" );
							//primaryValues.Add ( correction.ToString ( ) );

							//interpretation = ( int ) timeSlice.Interpretation;
						}
						else if ( property is IEditValClass )
						{
							primaryColumns.Add ( propInfo.Name + "_Value" );
							double valClassValue = ( property as IEditValClass ).Value;
							primaryValues.Add ( valClassValue.ToString ( ) );

							primaryColumns.Add ( propInfo.Name + "_Uom" );
							int valClassUom = ( property as IEditValClass ).Uom;
							primaryValues.Add ( valClassUom.ToString ( ) );

							double valClassSIValue = ConverterToSI.Convert ( property, double.NaN );
							if ( double.IsNaN ( valClassSIValue ) )
							{
								throw new Exception ( "Cann't convert " + propInfo.Name + " !" );
							}
							primaryColumns.Add ( propInfo.Name + "_SIValue" );
							primaryValues.Add ( valClassSIValue.ToString ( ) );

							//if ( isFeature && diffIndexList.Contains ( propInfo.Index ) )
							//{
							//    deltaPropValues.Add ( propInfo.Index, deltaProp.ToByteArray ( property ) );
							//}
						}
						else if ( propInfo.IsFeatureReference )
						{
							primaryColumns.Add ( propInfo.Name );
							ref_Guid = ( property as FeatureRef ).Identifier;
							//if ( isFeature && diffIndexList.Contains ( propInfo.Index ) )
							//{
							//    deltaPropValues.Add ( propInfo.Index, deltaProp.ToByteArray ( ref_Guid ) );
							//}
							primaryValues.Add ( "'" + ref_Guid + "'" );
						}
						else
						{

							primaryColumns.Add ( propInfo.Name + "_Value" );
							string valClassValue = AdaptQuotedStrIntoPg ( ( property as TextNote ).Value.ToString ( ) );

							primaryValues.Add ( "'" + valClassValue + "'" );

							primaryColumns.Add ( propInfo.Name + "_Lang" );
							int valClassLang = ( int ) ( property as TextNote ).Lang;
							primaryValues.Add ( valClassLang.ToString ( ) );

							//if ( isFeature && diffIndexList.Contains ( propInfo.Index ) )
							//{
							//    deltaPropValues.Add ( propInfo.Index, deltaProp.ToByteArray ( property ) );
							//}
						}
						#endregion
						break;

					case AimPropertyType.Object:
						#region Property Type is Object
						if ( AimMetadata.IsChoice ( propInfo.TypeIndex ) )
						{
							//ref_ID = InsertChoice ( ( property as AimObject ), correction, transaction );
							ref_ID = InsertChoice ( ( property as AimObject ), doUpdate, transaction );
							( property as DBEntity ).Id = ref_ID;
							if ( ref_ID != -1 )
							{
								primaryColumns.Add ( propInfo.Name );
								primaryValues.Add ( ref_ID.ToString ( ) );
								//if ( isFeature && diffIndexList.Contains ( propInfo.Index ) )
								//{
								//    deltaPropValues.Add ( propInfo.Index, deltaProp.ToByteArray ( ref_ID ) );
								//}
							}
						}
						else if ( AimMetadata.IsAbstract ( propInfo.TypeIndex ) )
						{
							//ref_ID = InsertObject ( ( property as AimObject ), correction, transaction );
							ref_ID = InsertObject ( ( property as AimObject ), doUpdate, transaction );
							( property as DBEntity ).Id = ref_ID;
							if ( ref_ID != -1 )
							{
								primaryColumns.Add ( propInfo.Name );
								primaryValues.Add ( ref_ID.ToString ( ) );

								primaryColumns.Add ( "ref_" + propInfo.Name );
								primaryValues.Add ( AimMetadata.GetAimTypeIndex ( property as AimObject ).ToString ( ) );

								//if ( isFeature && diffIndexList.Contains ( propInfo.Index ) )
								//{
								//    deltaPropValues.Add ( propInfo.Index, deltaProp.ToByteArray ( property ) );
								//}
							}
						}
						else
						{
							//ref_ID = InsertObject ( ( property as AimObject ), correction, transaction );
							ref_ID = InsertObject ( ( property as AimObject ), doUpdate, transaction );
							( property as DBEntity ).Id = ref_ID;
							if ( ref_ID != -1 )
							{
								primaryColumns.Add ( propInfo.Name );
								primaryValues.Add ( ref_ID.ToString ( ) );
								//if ( isFeature && diffIndexList.Contains ( propInfo.Index ) )
								//{
								//    deltaPropValues.Add ( propInfo.Index, deltaProp.ToByteArray ( ref_ID ) );
								//}
							}
						}
						#endregion
						break;

					case AimPropertyType.List:
						#region Property Type is List

						IList list = ( IList ) property;

						if ( AimMetadata.IsAbstractFeatureRefObject ( propInfo.TypeIndex ) )
						{
							#region AbstractFeatureRefObject
							if ( list.Count > 0 )
							{
								if ( sqlInsertFeats_in2_linkTable.Count == 0 )
									sqlInsertFeats_in2_linkTable.Add (
												string.Format ( "INSERT INTO \"{0}_link\" " +
													"(\"{0}_id\", prop_index, targetTableIndex, target_guid) VALUES",
														primaryTableName ) );

								for ( int j = 0; j <= list.Count - 1; j++ )
								{
									IAimProperty absFeatRefProp = ( ( IAimObject ) list [ j ] ).GetValue ( ( int ) PropertyAbstractFeatureRefObject.Feature );
									IAbstractFeatureRef absFeatRef = ( IAbstractFeatureRef ) absFeatRefProp;
									sqlInsertFeats_in2_linkTable.Add ( propInfo.Index.ToString ( ) + ", " + absFeatRef.FeatureTypeIndex.ToString ( ) + ", '" +
											absFeatRef.Identifier.ToString ( ) + "'" );
								}
								//if ( isFeature && diffIndexList.Contains ( propInfo.Index ) )
								//{
								//    deltaPropValues.Add ( propInfo.Index, deltaProp.ToByteArray ( list ) );
								//}
							}
							#endregion
						}
						else
						{
							if ( propInfo.IsFeatureReference )
							{
								#region FeatureRef
								if ( list.Count > 0 )
								{
									if ( sqlInsertFeats_in2_linkTable.Count == 0 )
										sqlInsertFeats_in2_linkTable.Add (
												string.Format ( "INSERT INTO \"{0}_link\"" +
												"(\"{0}_id\", prop_index, targetTableIndex, target_guid) VALUES", primaryTableName ) );
									for ( int j = 0; j <= list.Count - 1; j++ )
									{
										sqlInsertFeats_in2_linkTable.Add ( propInfo.Index.ToString ( ) + ", " + propInfo.TypeIndex + ", '" +
										( list [ j ] as FeatureRefObject ).Feature.Identifier.ToString ( ) + "'" );
									}

									//if ( isFeature && diffIndexList.Contains ( propInfo.Index ) )
									//{
									//    deltaPropValues.Add ( propInfo.Index, deltaProp.ToByteArray ( list ) );
									//}
								}
								#endregion
							}
							else if ( AimMetadata.IsChoice ( propInfo.TypeIndex ) )
							{
								#region Choice
								//insertedIds = InsertChoiceList ( list, correction, transaction );
								insertedIds = InsertChoiceList ( list, doUpdate, transaction );
								if ( insertedIds.Count > 0 )
								{
									if ( sqlInsertObj_in2_linkTable.Count == 0 )
										sqlInsertObj_in2_linkTable.Add (
												string.Format ( "INSERT INTO \"{0}_link\"" +
												"(\"{0}_id\", prop_index, targetTableIndex, target_id) VALUES", primaryTableName ) );
									for ( int j = 0; j <= insertedIds.Count - 1; j++ )
									{
										sqlInsertObj_in2_linkTable.Add ( propInfo.Index.ToString ( ) + ", " + propInfo.TypeIndex.ToString ( ) + ", " +
										insertedIds [ j ].ToString ( ) );
									}

									//if ( isFeature && diffIndexList.Contains ( propInfo.Index ) )
									//{
									//    foreach ( int choiceId in insertedIds )
									//    {
									//        deltaPropValues.Add ( propInfo.Index, deltaProp.ToByteArray ( insertedIds ) );
									//    }
									//}
								}
								#endregion
							}
							else
							{
								#region Object
								//insertedIds = InsertObjectList ( list, correction, transaction );
								insertedIds = InsertObjectList ( list, doUpdate, transaction );
								if ( insertedIds.Count > 0 )
								{
									if ( sqlInsertObj_in2_linkTable.Count == 0 )
										sqlInsertObj_in2_linkTable.Add (
												string.Format ( "INSERT INTO \"{0}_link\"" +
												"(\"{0}_id\", prop_index, targetTableIndex, target_id) VALUES", primaryTableName ) );
									for ( int j = 0; j <= insertedIds.Count - 1; j++ )
									{
										targetTableIndex = AimMetadata.GetAimTypeIndex ( list [ j ] as IAimObject );
										sqlInsertObj_in2_linkTable.Add ( propInfo.Index.ToString ( ) + ", " + targetTableIndex.ToString ( ) + ", " +
										insertedIds [ j ].ToString ( ) );
									}

									//if ( isFeature && diffIndexList.Contains ( propInfo.Index ) )
									//{
									//    deltaPropValues.Add ( propInfo.Index, deltaProp.ToByteArray ( insertedIds ) );
									//}
								}
								#endregion
							}
						}
						#endregion
						break;
				}
			}
			string updatePropsToNullSqlText = "";
			string setNullSql4ListProp = "";
			if ( doUpdate )
			{
				if ( isFeature )
				{
				}
				updatePropsToNullSqlText = GetNullPropsSqlText ( aimObj, filledPropInfos, out setNullSql4ListProp );
			}

			if ( isFeature )
			{
				#region AimObject is Feature Case WHERE properties should be INSERTed INTO delta AND deltaProperties table

				string WHEREClause = string.Format ( " \"Identifier\"='{0}'", guid );
				long featId = GetFeatId ( "features", WHEREClause, transaction );
				primaryColumns.Add ( "feat_id" );
				primaryValues.Add ( featId.ToString ( ) );

				//InsertFeatToDeltaTable ( featId, ( aimObj as Feature ).TimeSlice, transaction, diffIndexList, deltaPropValues );

				string result = string.Empty;
				//if ( ( TimeSliceInterpretationType ) interpretation == TimeSliceInterpretationType.PERMDELTA ||
				//        ( TimeSliceInterpretationType ) interpretation == TimeSliceInterpretationType.BASELINE )
				{
					result = FeatInsertCommandToBl_Table ( transaction, id, primaryTableName, doUpdate, featId,
															primaryColumns, primaryValues, updatePropsToNullSqlText, setNullSql4ListProp );
				}
				return result;
				#endregion
			}

			#region Inserting object part of property
			if ( primaryColumns.Count > 0 )
			{
				return CreateInsertSqlText ( primaryTableName, id, primaryColumns, primaryValues, doUpdate, updatePropsToNullSqlText, setNullSql4ListProp);
			}
			else
			{
				if ( sqlInsertFeats_in2_linkTable.Count > 0 || sqlInsertObj_in2_linkTable.Count > 0 )
				{
					long nextValOfId = NextValOfSequence ( primaryTableName, transaction );
					return string.Format ( "INSERT INTO \"{0}\"(\"Id\") VALUES ({1});", primaryTableName, nextValOfId );
				}
				return string.Empty;
			}
			#endregion
		}

		private string GetNullPropsSqlText ( IAimObject aimObj, List<AimPropInfo> filledPropInfos, out string setNullSql4ListProps )
		{
			AimPropInfo [] propInfos = AimMetadata.GetAimPropInfos ( aimObj );
			List<AimPropInfo> nullPropInfos = new List<AimPropInfo> ( );
			nullPropInfos.AddRange ( propInfos );
			int index;
			foreach ( AimPropInfo propInfo in filledPropInfos )
			{
				index = nullPropInfos.FindIndex ( item => item.AixmName == propInfo.AixmName );
				if ( index != -1 )
				{
					nullPropInfos.RemoveAt ( index );
				}
			}
			IAimProperty property;
			string result = "";
			string tableName = CommonData.GetTableName ( aimObj );
			long id = ( aimObj as DBEntity ).Id;
			setNullSql4ListProps = "";
			foreach ( AimPropInfo propInfo in nullPropInfos )
			{
				if ( propInfo.TypeIndex == ( int ) DataType.TimeSlice )
					continue;
				if ( propInfo.IsList )
				{
					setNullSql4ListProps += string.Format ( "delete FROM \"{0}_link\" " +
											"WHERE \"{0}_id\" = {1} AND prop_index = {2}; ",											
											tableName,
											id,
											propInfo.Index );
				}
				else
				{
					property = ( IAimProperty ) AimObjectFactory.Create ( propInfo.TypeIndex );
					if ( property.PropertyType == AimPropertyType.DataType && property is IEditValClass )
					{
						result += "\"" + propInfo.Name + "_Value\" = DEFAULT, ";
					}
					else
					{
						result += "\"" + propInfo.Name + "\" = DEFAULT, ";
					}
				}
			}
			return result;
		}

		private string AdaptQuotedStrIntoPg ( string sourceStr )
		{
			if ( sourceStr.Contains ( "'" ) )
			{
				int indexOfQuote = sourceStr.IndexOf ( "'" );
				while ( indexOfQuote > 0 )
				{
					sourceStr = sourceStr.Insert ( indexOfQuote, "'" );
					indexOfQuote = sourceStr.IndexOf ( "'", indexOfQuote + 2 );
				}
			}
			return sourceStr;
		}

		private string InsertIntofeaturesTable ( Feature feat, IDbTransaction transaction )
		{
			int index = ( int ) feat.FeatureType;
			//if ( feat.TimeSlice.FeatureLifetime == null )
			//{
			//    return "LifeTime property is empty!";
			//}
			//else
			{
				//bool isCorrection = false;
				//if ( feat.TimeSlice.CorrectionNumber > 0 )
				//    isCorrection = true;
				string sqlText;
				//if ( !isCorrection )
				{
					//if ( feat.TimeSlice.FeatureLifetime.EndPosition.HasValue )
					//sqlText = string.Format (
					//    "INSERT INTO features(" +
					//    "\"Identifier\", begin_life, end_life, type_index)\r\n" +
					//    "\r\nVALUES (" +
					//    "'{0}', '{1}', '{2}', {3})",
					//    feat.Identifier,
					//    feat.TimeSlice.FeatureLifetime.BeginPosition,
					//    feat.TimeSlice.FeatureLifetime.EndPosition.Value,
					//    index );
					sqlText = string.Format (
						"INSERT INTO features(" +
						"\"Identifier\", type_index)\r\n" +
						"\r\nVALUES (" +
						"'{0}', {1})",
						feat.Identifier,
						index );
					//else
					//    sqlText = string.Format (
					//        "INSERT INTO features(" +
					//        "\"Identifier\", begin_life, type_index)" +
					//        "\r\nVALUES(" +
					//        "'{0}', '{1}', {2})",
					//        feat.Identifier,
					//        feat.TimeSlice.FeatureLifetime.BeginPosition,
					//        index );
					CreateCommand ( transaction, sqlText ).ExecuteNonQuery ( );
					return string.Empty;
				}

				sqlText = string.Format ( "UPDATE features SET begin_life = '{0}'", feat.TimeSlice.FeatureLifetime.BeginPosition );
				if ( feat.TimeSlice.FeatureLifetime.EndPosition.HasValue )
					sqlText += string.Format ( ", end_life = '{0}' ", feat.TimeSlice.FeatureLifetime.EndPosition.Value );
				sqlText += string.Format ( " WHERE \"Identifier\"='{0}'", feat.Identifier );
				CreateCommand ( transaction, sqlText ).ExecuteNonQuery ( );
				return string.Empty;
			}
		}

		//private void InsertFeatToDeltaTable ( long featId, TimeSlice timeSlice, IDbTransaction transaction,
		//                                        List<int> diffIndexList, SortedDictionary<int, byte []> deltaPropValues )
		//{
		//    int interpretation = ( int ) timeSlice.Interpretation;
		//    int sequence = timeSlice.SequenceNumber;
		//    int correction = timeSlice.CorrectionNumber;
		//    DateTime begin_valid = timeSlice.ValidTime.BeginPosition;
		//    DateTime? end_valid = timeSlice.ValidTime.EndPosition;

		//    // Add Null properties to feat_delta_properties table
		//    // Which value column in table is just false means it has no value
		//    foreach ( var item in diffIndexList )
		//    {
		//        if ( !deltaPropValues.ContainsKey ( item ) )
		//        {
		//            deltaPropValues.Add ( item, BitConverter.GetBytes ( false ) );
		//        }
		//    }

		//    string sqlTextDelta = "INSERT INTO feat_delta(feat_id, interpretation, sequence, correction, begin_valid";
		//    string sqlTextVals = featId + ", " + interpretation + ", " + sequence + ", " + correction + ", '" + begin_valid + "'";
		//    if ( end_valid.HasValue )
		//    {
		//        sqlTextDelta += ", end_valid";
		//        sqlTextVals += ", '" + end_valid.Value + "'";
		//    }

		//    IDbCommand command = CreateCommand ( transaction, sqlTextDelta + ") VALUES (" + sqlTextVals + ");" );
		//    command.ExecuteNonQuery ( );

		//    long feat_delta_id = CurrValOfSequence ( "feat_delta", transaction );
		//    sqlTextDelta = "INSERT INTO feat_delta_properties (feat_delta_id, key, value) VALUES ";

		//    int i = 0;
		//    foreach ( KeyValuePair<int, byte []> deltaValue in deltaPropValues )
		//    {
		//        i++;
		//        sqlTextDelta += "(" + feat_delta_id + ", " + deltaValue.Key + ", :bytea" + i + "),";
		//        IDataParameter dataParam = command.CreateParameter ( );
		//        dataParam.DbType = DbType.Binary;
		//        dataParam.ParameterName = "bytea" + i;
		//        dataParam.Value = deltaValue.Value;
		//        command.Parameters.Add ( dataParam );
		//    }
		//    sqlTextDelta = sqlTextDelta.Remove ( sqlTextDelta.Length - 1 );
		//    command.CommandText = sqlTextDelta;
		//    command.ExecuteNonQuery ( );

		//}

		//private string FeatInsertCommandToBl_Table ( IDbTransaction transaction, string primaryTableName,
		//                                            long featId, TimeSlice timeSlice,
		//                                            List<string> primaryColumns, List<string> primaryValues )
		private string FeatInsertCommandToBl_Table ( IDbTransaction transaction, long id,
													string primaryTableName, bool doUpdate,
													long featId, List<string> primaryColumns,
													List<string> primaryValues, string updatePropsToNull, string setNullSql4ListProp )
		{
			//int sequence = timeSlice.SequenceNumber;
			//int correction = timeSlice.CorrectionNumber;
			//DateTime begin_valid = timeSlice.ValidTime.BeginPosition;

			IDbCommand command = CreateCommand ( transaction );
			//if ( sequence > 1 )
			//{
			//    command.CommandText = string.Format ( "UPDATE \"{0}\" SET end_valid='{1}' WHERE " +
			//                                                        "feat_id={2} AND sequence={3}",
			//                                            primaryTableName,
			//                                            begin_valid,
			//                                            featId,
			//                                            sequence - 1 );
			//    command.ExecuteNonQuery ( );
			//}
			//bool isCorrection = ( correction > 0 );
			if ( doUpdate )
			{
				string result = string.Format ( "{0} UPDATE \"{1}\" SET", setNullSql4ListProp, primaryTableName );
				for ( int i = 0; i <= primaryColumns.Count - 1; i++ )
				{
					result += "\"" + primaryColumns [ i ] + "\" = " + primaryValues [ i ] + ", ";
				}
				result += updatePropsToNull;
				result = result.Remove ( result.Length - 2 ) + " WHERE feat_id = " + featId;
				//result += " WHERE sequence = " + sequence + " AND feat_id = " + featId;				
				return result;
			}
			return CreateInsertSqlText ( primaryTableName, id, primaryColumns, primaryValues, false,"", setNullSql4ListProp );
		}

		private string CreateInsertSqlText ( string tableName, long id, List<string> columns, List<string> values, bool doUpdate, string updatePropsToNull = "", string setNullSql4ListProp = "" )
		{
			string result = "";
			// When feature is read from other database its properties' Id greater than zero
			// But doUpdate depends on feature
			if ( id > 0 && doUpdate)
			{
				result = setNullSql4ListProp + "UPDATE \"" + tableName + "\" SET ";
				for ( int i = 0; i <= columns.Count - 1; i++ )
				{
					result += "\"" + columns [ i ] + "\" = " + values [ i ] + ", ";
				}
				result += updatePropsToNull;
				result = result.Remove ( result.Length - 2 ) + " WHERE \"Id\" = " + id + "; ";
				return result;
			}
			//result = "DELETE FROM \"" + tableName + "\" WHERE \"Id\" = " + id + "; " + 
			result = "INSERT INTO \"" + tableName + "\"(";
			foreach ( string strCol in columns )
			{
				result += "\"" + strCol + "\",";
			}
			result = result.Remove ( result.Length - 1 ) + ")\r\n VALUES(";
			foreach ( string strVal in values )
			{
				result += strVal + ",";
			}
			result = result.Remove ( result.Length - 1 ) + ")";
			return result;
		}

		//private List<long> InsertObjectList ( IList aimObjList, int correction, IDbTransaction transaction )
		private List<long> InsertObjectList ( IList aimObjList, bool doUpdate, IDbTransaction transaction )
		{
			List<long> result = new List<long> ( );
			AimObject aimObj;
			long objId;
			for ( int i = 0; i <= aimObjList.Count - 1; i++ )
			{
				aimObj = ( aimObjList [ i ] as AimObject );
				//objId = InsertObject ( aimObj, correction, transaction );
				objId = InsertObject ( aimObj, doUpdate, transaction );
				if ( objId != -1 )
					result.Add ( objId );
			}
			return result;
		}

		//private long InsertObject ( AimObject aimObj, int correction, IDbTransaction transaction )
		private long InsertObject ( AimObject aimObj, bool doUpdate, IDbTransaction transaction )
		{
			string insertSqlText;
			List<string> sqlInsertObj_in2_LinkTable = new List<string> ( );
			List<string> sqlInsertFeat_in2_LinkTable = new List<string> ( );
			long objId;
			List<AimPropInfo> propInfos = GetFilledPropInfos ( aimObj );
			byte [] geomByteArray;

			//insertSqlText = CreateSqlText_4_DBEntity ( aimObj, null, correction, sqlInsertObj_in2_LinkTable, sqlInsertFeat_in2_LinkTable, transaction, out geomByteArray );
			insertSqlText = CreateSqlText_4_DBEntity ( aimObj, doUpdate, sqlInsertObj_in2_LinkTable, sqlInsertFeat_in2_LinkTable, transaction, out geomByteArray );
			if ( insertSqlText == string.Empty )
				return -1;
			IDbCommand command = CreateCommand ( transaction, insertSqlText );
			if ( geomByteArray != null )
			{
				AddParameterToDBCommand ( command, geomByteArray );
			}
			command.ExecuteNonQuery ( );
			string tableName = CommonData.GetTableName ( aimObj );

			if ( doUpdate && ( aimObj as DBEntity ).Id > 0 )
				objId = ( ( aimObj as DBEntity ).Id );
			else
				objId = CurrValOfSequence ( tableName, transaction );
			InserLinkTable ( objId, tableName, sqlInsertObj_in2_LinkTable, sqlInsertFeat_in2_LinkTable, transaction );
			return objId;
		}

		private void AddParameterToDBCommand ( IDbCommand command, byte [] geomByteArray )
		{
			IDataParameter dataParam = command.CreateParameter ( );
			dataParam.DbType = DbType.Binary;
			dataParam.ParameterName = byteaGeomParamName;
			dataParam.Value = geomByteArray;
			command.Parameters.Add ( dataParam );
		}

		//private List<long> InsertChoiceList ( IList aimObjList, int correction, IDbTransaction transaction )
		private List<long> InsertChoiceList ( IList aimObjList, bool doUpdate, IDbTransaction transaction )
		{
			List<long> result = new List<long> ( );
			AimObject aimObj;
			long objId;
			for ( int i = 0; i <= aimObjList.Count - 1; i++ )
			{
				aimObj = ( AimObject ) aimObjList [ i ];
				//objId = InsertChoice ( aimObj, correction, transaction );
				objId = InsertChoice ( aimObj, doUpdate, transaction );
				if ( objId != -1 )
					result.Add ( objId );
			}
			return result;
		}

		//private long InsertChoice ( AimObject aimObj, int correction, IDbTransaction transaction )
		private long InsertChoice ( AimObject aimObj, bool doUpdate, IDbTransaction transaction )
		{
			string choiceTableName = CommonData.GetTableName ( aimObj );
			IEditChoiceClass editChoiceObj = ( IEditChoiceClass ) aimObj;
			AimObjectType objType = AimMetadata.GetAimObjectType ( editChoiceObj.RefType );
			long id = ( aimObj as DBEntity ).Id;
			string sqlText = "DELETE FROM \"" + choiceTableName + "\" WHERE \"Id\" = " + id + ";";

			if ( AimMetadata.IsAbstractFeatureRef ( editChoiceObj.RefType ) )
			{
				sqlText += "INSERT INTO \"" + choiceTableName + "\"(prop_type, choice_type,target_guid)";
				Guid identifier = ( editChoiceObj.RefValue as FeatureRef ).Identifier;
				int refFeatTypeIndex = ( editChoiceObj.RefValue as IAbstractFeatureRef ).FeatureTypeIndex;
				if ( identifier != null && refFeatTypeIndex != 0 )
					sqlText += " values(" + editChoiceObj.RefType + ", " + refFeatTypeIndex.ToString ( ) + ", '" +
										identifier.ToString ( ) + "')";
			}
			else if ( editChoiceObj.RefValue is FeatureRef )
			{
				sqlText += "INSERT INTO \"" + choiceTableName + "\"(prop_type, choice_type, target_guid)";
				Guid identifier = ( editChoiceObj.RefValue as FeatureRef ).Identifier;
				if ( identifier != null )
					sqlText += " values(" + editChoiceObj.RefType + ", " + editChoiceObj.RefType + ", '" +
							identifier.ToString ( ) + "')";
			}
			else if ( AimMetadata.IsAbstract ( editChoiceObj.RefType ) )
			{
				//insertedObjId = InsertObject ( ( editChoiceObj.RefValue as AimObject ), correction, transaction );
				id = InsertObject ( ( editChoiceObj.RefValue as AimObject ), doUpdate, transaction );
				// if there is nothing to insert into obj table then should insert just Id
				if ( id == -1 )
				{
					string tableName = CommonData.GetTableName ( editChoiceObj.RefValue as AimObject );
					id = NextValOfSequence ( tableName, transaction );
					sqlText += string.Format ( "INSERT INTO \"{0}\"(\"Id\") VALUES ({1});", tableName, id );
				}
				sqlText += "INSERT INTO \"" + choiceTableName + "\"(prop_type, choice_type, target_id)";
				sqlText += " values(" + editChoiceObj.RefType + ", " + AimMetadata.GetAimTypeIndex ( editChoiceObj.RefValue as IAimObject ).ToString ( ) + ", " + id + ")";
			}
			else
			{
				//insertedObjId = InsertObject ( ( editChoiceObj.RefValue as AimObject ), correction, transaction );
				id = InsertObject ( ( editChoiceObj.RefValue as AimObject ), doUpdate, transaction );
				// if there is nothing to insert into obj table then should insert just Id
				if ( id == -1 )
				{
					string tableName = CommonData.GetTableName ( editChoiceObj.RefValue as AimObject );
					id = NextValOfSequence ( tableName, transaction );
					sqlText += string.Format ( "INSERT INTO \"{0}\"(\"Id\") VALUES ({1});", tableName, id );
				}
				sqlText += "INSERT INTO \"" + choiceTableName + "\"(prop_type, choice_type,target_id)";
				sqlText += " values(" + editChoiceObj.RefType + ", " + editChoiceObj.RefType + ", " + id + ")";
			}

			int countInsertedRows = CreateCommand ( transaction, sqlText ).ExecuteNonQuery ( );

			if ( countInsertedRows > 0 )
			{
				id = CurrValOfSequence ( choiceTableName, transaction );
				( aimObj as DBEntity ).Id = id;
				return id;
			}
			return -1;
		}

		/// <summary>
		/// Returns error message if something went wrong 
		/// else returns empty string and sets sequence and correction 		
		/// </summary>
		//private string GetCurrVersion ( Feature feat, ref int sequence, ref int correction )
		//{
		//    if ( feat.Identifier == null )
		//        return "Identifier property is empty!";

		//    if ( feat.TimeSlice == null )
		//        return "TimeSlice property is empty!";

		//    //if ( !HasFeature ( feat.Identifier ) )
		//    //{
		//    //    return string.Empty;
		//    //}
		//    IDbCommand pgCommand = _connection.CreateCommand ( );
		//    string tableName;
		//    if ( feat.TimeSlice.Interpretation == TimeSliceInterpretationType.PERMDELTA ||
		//        feat.TimeSlice.Interpretation == TimeSliceInterpretationType.BASELINE )
		//    {
		//        tableName = "bl_" + feat.FeatureType;
		//        pgCommand.CommandText = string.Format ( "SELECT \"Id\", sequence, correction FROM \"{0}\" " +
		//                                                    "WHERE feat_id = (SELECT \"Id\" FROM features WHERE \"Identifier\" = '{1}') " +
		//                                                        "ORDER BY \"Id\" DESC LIMIT 1",
		//                                                tableName, feat.Identifier );
		//    }
		//    else
		//    {
		//        // Not implemented yet 
		//        tableName = "feat_delta";
		//        string WHEREClause = string.Format ( " \"Identifier\"='{}'", feat.Identifier );
		//        long featId = GetFeatId ( "feat_delta", WHEREClause );
		//        pgCommand.CommandText = "SELECT sequence, correction FROM " + tableName + " WHERE feat_id=" + featId;
		//    }
		//    IDataReader dataReader = pgCommand.ExecuteReader ( );
		//    if ( !dataReader.Read ( ) )
		//        return "It does not have inserted version!";
		//    // So it cann't be empty 
		//    sequence = Convert.ToInt32 ( dataReader [ 1 ] );
		//    correction = Convert.ToInt32 ( dataReader [ 2 ] );
		//    dataReader.Close ( );
		//    return string.Empty;
		//}

		private long GetFeatId ( string tableName, string WHEREClause, IDbTransaction transaction = null )
		{
			string sqlCommand = string.Format ( "SELECT \"Id\" FROM \"{0}\" WHERE {1}",
													tableName, WHEREClause );
			long id = ( long ) CreateCommand ( transaction, sqlCommand ).ExecuteScalar ( );
			return id;
		}

		private long CurrValOfSequence ( string tableName, IDbTransaction transaction )
		{
			//object val = CreateCommand ( transaction, string.Format ( "SELECT currval ('\"{0}_Id_seq\"')", tableName ) ).ExecuteScalar ( );
			object val = CreateCommand ( transaction, string.Format ( "SELECT \"Id\" FROM \"{0}\" ORDER BY \"Id\" DESC ", tableName ) ).ExecuteScalar ( );
			return Convert.ToInt64 ( val );
		}

		private long NextValOfSequence ( string tableName, IDbTransaction transaction )
		{
			object val = CreateCommand ( transaction, string.Format ( "SELECT nextval ('\"{0}_Id_seq\"')", tableName ) ).ExecuteScalar ( );
			return Convert.ToInt64 ( val );
		}

		private void InserLinkTable ( long primaryId, string tableName,
									List<string> sqlTexts_4_ObjInserting,
									List<string> sqlTexts_4_FeatInserting,
									IDbTransaction transaction )
		{
			string prop_indices = "(";
			int prop_index;
			string sqlText;
			IDbCommand command = CreateCommand ( transaction );

			#region Insert Objects INTO Link Table
			if ( sqlTexts_4_ObjInserting.Count > 0 )
			{

				for ( int i = 1; i <= sqlTexts_4_ObjInserting.Count - 1; i++ )
				{
					int.TryParse ( sqlTexts_4_ObjInserting [ i ].Substring ( 0, sqlTexts_4_ObjInserting [ i ].IndexOf ( ',' ) ), out prop_index );
					prop_indices += prop_index + ",";
				}
				prop_indices = prop_indices.Remove ( prop_indices.Length - 1 ) + ")";

				sqlText = string.Format ( "delete FROM \"{0}_link\" " +
												"WHERE \"{0}_id\" = {1} AND prop_index in {2}; " +
												"{3}",
												tableName,
												primaryId,
												prop_indices,
												sqlTexts_4_ObjInserting [ 0 ] );
				for ( int i = 1; i <= sqlTexts_4_ObjInserting.Count - 1; i++ )
				{
					sqlText += "(" + primaryId.ToString ( ) + ", " + sqlTexts_4_ObjInserting [ i ] + "),";
				}
				sqlText = sqlText.Remove ( sqlText.Length - 1 );

				command.CommandText = sqlText;
				command.ExecuteNonQuery ( );
			}
			#endregion

			#region Insert Features INTO Link Table
			if ( sqlTexts_4_FeatInserting.Count > 0 )
			{
				prop_indices = "(";
				for ( int i = 1; i <= sqlTexts_4_FeatInserting.Count - 1; i++ )
				{
					int.TryParse ( sqlTexts_4_FeatInserting [ i ].Substring ( 0, sqlTexts_4_FeatInserting [ i ].IndexOf ( ',' ) ), out prop_index );
					prop_indices += prop_index + ",";
				}
				prop_indices = prop_indices.Remove ( prop_indices.Length - 1 ) + ")";

				sqlText = string.Format ( "delete FROM \"{0}_link\" " +
												"WHERE \"{0}_id\" = {1} AND prop_index in {2}; " +
												"{3}",
												tableName,
												primaryId,
												prop_indices,
												sqlTexts_4_FeatInserting [ 0 ] );
				for ( int i = 1; i <= sqlTexts_4_FeatInserting.Count - 1; i++ )
				{
					sqlText += "(" + primaryId.ToString ( ) + ", " + sqlTexts_4_FeatInserting [ i ] + "),";
				}
				sqlText = sqlText.Remove ( sqlText.Length - 1 );

				command.CommandText = sqlText;
				command.ExecuteNonQuery ( );
			}
			#endregion
		}

		//private bool HasFeature ( Guid guid )
		//{
		//    IDbCommand command = _connection.CreateCommand ( );
		//    command.CommandText = string.Format ( "SELECT * FROM features WHERE \"Identifier\" = '{0}'", guid );
		//    IDataReader dataReader = command.ExecuteReader ( );
		//    bool result = dataReader.Read ( );
		//    dataReader.Close ( );
		//    return result;
		//}

		private IDbCommand CreateCommand ( IDbTransaction transaction, string commandText = null )
		{
			IDbCommand command = _connection.CreateCommand ( );
			command.Transaction = transaction;
			if ( commandText != null )
				command.CommandText = commandText;
			return command;
		}

		private List<AimPropInfo> GetFilledPropInfos ( IAimObject aimObj )
		{
			List<AimPropInfo> result = new List<AimPropInfo> ( );
			AimPropInfo [] propInfos = AimMetadata.GetAimPropInfos ( aimObj );
			int [] indexes = aimObj.GetPropertyIndexes ( );
			for ( int i = 0; i <= indexes.Length - 1; i++ )
			{
				for ( int j = 0; j <= propInfos.Length - 1; j++ )
				{
					if ( indexes [ i ] == propInfos [ j ].Index )
						result.Add ( propInfos [ j ] );
				}
			}
			return result;
		}

		private string GetDeletSqlText ( IAimObject aimObj )
		{
			AimPropInfo [] propInfoArray = AimMetadata.GetAimPropInfos ( aimObj );
			IAimProperty aimProp;
			IAimObject propAimObj;
			string sqlText = "";
			string tableName ;
			FeatureRef featRef;
			foreach ( AimPropInfo propInfo in propInfoArray )
			{
				//if ( propInfo.IsList )
				//{
					//continue;
				//}
				propAimObj = AimObjectFactory.Create ( propInfo.TypeIndex );
				aimProp = ( IAimProperty ) propAimObj;
				if ( aimProp.PropertyType == AimPropertyType.Object )
				{
					aimProp = aimObj.GetValue ( propInfo.Index );
					if ( aimProp != null )
					{
						if ( propInfo.IsList )
						{
							IList listProp = ( IList ) aimProp;
							foreach ( var item in listProp )
							{
								if ( propInfo.IsFeatureReference )
								{
									featRef = ( ( FeatureRefObject ) propAimObj ).Feature ;
									if ( featRef != null )
									{
										tableName = CommonData.GetTableName ( aimObj );
										sqlText += "DELETE FROM \"" + tableName + "_link\" WHERE target_guid = '" +
													featRef.Identifier + "';";
									}
								}
								else
								{
									propAimObj = ( IAimObject ) item;
									sqlText += GetDeletSqlText ( propAimObj );
								}
							}
						}
						else
						{
							propAimObj = ( IAimObject ) aimProp;
							sqlText += GetDeletSqlText ( propAimObj );
						}
					}
				}
			}
			if ( !( aimObj is Feature ) )
			{
				tableName = CommonData.GetTableName ( aimObj );
				sqlText += "DELETE FROM \"" + tableName + "\" WHERE \"Id\" = " + ( aimObj as DBEntity ).Id + ";";
			}
			return sqlText;
		}

		private IDbConnection _connection;
		private string byteaGeomParamName;
		private Dictionary<int, List<IDbCommand>> _dbCommandDictionary;
		private int _transactionIndex;
	}
}