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
using System.Text;

namespace Aran.Aim.Data
{
	internal class Reader
	{
		public Reader(IDbConnection connection)
		{
			_connection = connection;
			_timeSliceFilter = new TimeSliceFilter(DateTime.Now);
		}

		public TimeSliceFilter TimeSliceFilter
		{
			get { return _timeSliceFilter; }
			set { _timeSliceFilter = value; }
		}

		public GettingResult VersionsOf(FeatureType featType,
											TimeSliceInterpretationType interpretation,
											Guid identifier = default(Guid),
											bool onlyBaseComplexProps = false,
											TimeSliceFilter timeSlicefilter = null,
											List<string> propertyList = null,
											Filter filter = null)
		{
			IDataReader dataReader = null;

			bool loadAllForExport = false;
			if (propertyList != null &&
				propertyList.Count == 1 &&
				propertyList[0] == "<LOAD_ALL_FOR_EXPORT>")
			{
				loadAllForExport = true;
				propertyList = null;
			}

			try
			{
				if (interpretation != TimeSliceInterpretationType.BASELINE)
					return new GettingResult(false, interpretation + " interpretation is not implemented yet :(!");

				IDbCommand command = _connection.CreateCommand();

				int featTypeIndex = (int)featType;

				#region GetFeatureType If type index is FeatureRef

				if (featTypeIndex == (int)DataType.FeatureRef)
				{
					command.CommandText =
						"SELECT type_index " +
						"FROM features WHERE \"Identifier\"='" + identifier + "'";
					featTypeIndex = (int)command.ExecuteScalar();
				}

				#endregion

				string featName = "bl_" + AimMetadata.GetAimTypeName(featTypeIndex);
				TimeSliceFilter tmpTimeSliceFilter = timeSlicefilter ?? _timeSliceFilter;
				if (tmpTimeSliceFilter == null)
					return new GettingResult(false, "Time Slice Filter is null !");

				AimPropInfoList simplePropList = new AimPropInfoList();
				AimPropInfoList complexPropList = new AimPropInfoList();
				AimPropInfoList propInfoList = new AimPropInfoList();

				AimPropInfo[] propInfoArray = AimMetadata.GetAimPropInfos(featTypeIndex);

				if (propertyList == null)
				{
					propInfoList.AddRange(propInfoArray);
				}
				else
				{
					foreach (string propertyName in propertyList)
					{
						AimPropInfo propInfo = GetPropInfo(propInfoArray, propertyName.ToLower());
						if (propInfo == null)
							return new GettingResult(false, "Not found property !");
						propInfoList.Add(propInfo);
					}
				}

				Dictionary<AimPropInfo, int> columnIndices;
				string columns = SeparateProperties(propInfoList, simplePropList, complexPropList, featName, out columnIndices);
				string mandatCols = AddFeatMandatoryColumns(featTypeIndex, simplePropList, columnIndices);
				if (mandatCols != "")
					columns += (string.IsNullOrWhiteSpace(columns) ? "" : ",") + "\r\n" + mandatCols;

				var tsfs = (loadAllForExport ? "" : " AND " + TimeSliceFilterToSqlCommand(featName, tmpTimeSliceFilter));

				if (identifier != Guid.Empty)
					command.CommandText = string.Format("SELECT {0} FROM \"{1}\", features " +
														"WHERE \"{1}\".feat_id = features.\"Id\" AND " +
														"features.\"Identifier\" = '{2}' " +
														"{3}",
														columns, featName,
														identifier, tsfs);
				else
				{
					command.CommandText = string.Format("SELECT {0} FROM \"{1}\", features " +
															"WHERE \"{1}\".feat_id = features.\"Id\" AND " +
															"features.type_index = {2}" +
															"{3}",
															 columns, featName,
															 featTypeIndex, tsfs);
				}

				if (filter != null)
				{
					PgFilterImplementation pgFilterImp = new PgFilterImplementation();
					byte[] geomByteA = null;
					string s = pgFilterImp.GetSqlString(filter.Operation, featTypeIndex, ref geomByteA);
					if (!string.IsNullOrEmpty(s))
						command.CommandText += " AND " + s;
					if (geomByteA != null)
					{
						AddParameterToDBCommand(command, geomByteA);

						{
							var ss = string.Empty;

							//ss = string.Join<byte>(",", geomByteA);

							var x = (geomByteA.Length / 4) * 4;
							for (int n = 0; n < x; n += 4)
							{
								var hx = BitConverter.ToInt32(geomByteA, n);
								ss = string.Format("{0:X}", hx) + ss;
							}

							var q = geomByteA.Length - x;
							if (q > 0)
							{
								var newba = new byte[] { 0, 0, 0, 0 };
								for (int i = 0; i < q; i++)
								{
									newba[i] = geomByteA[x + i];
								}

								var hx = BitConverter.ToInt32(newba, 0);
								ss = string.Format("{0:X}", hx) + ss;
							}
						}
					}
				}

				dataReader = command.ExecuteReader();

				IList result = GetDBEntityList(featTypeIndex, dataReader, simplePropList, "",
												columnIndices, _connection, null);

				LoadComplexPropertiesOfFeature(result, complexPropList);

				LoadMetadata(result);

				dataReader.Close();
			    command.Dispose();

				//foreach (DBEntity item in result)
				//    (item as IEditDBEntity).Listener = null;

				var getResult = new GettingResult(true);
				getResult.List = result;
				return getResult;
			}
			catch (Exception exc)
			{
				dataReader?.Close();
				_connection.Close();
				_connection.Open();
				return new GettingResult(false, CommonData.GetErrorMessage(exc));
			}
            finally
            {
                dataReader?.Dispose();
            }
        }

		internal GettingResult GetAllStoredFeatTypes()
		{
			IDataReader dataReader = null;
			try
			{
				string sqlString = "SELECT type_index FROM features GROUP BY type_index";
				IDbCommand command = _connection.CreateCommand();
				command.CommandText = sqlString;
				dataReader = command.ExecuteReader();
				List<FeatureType> indices = new List<FeatureType>();
				while (dataReader.Read())
				{
					indices.Add((FeatureType)(int)dataReader[0]);
				}
				dataReader.Close();
				GettingResult getResult = new GettingResult(true);
				getResult.List = indices;
				return getResult;
			}
			catch (Exception exc)
			{
			    dataReader?.Close();
			    _connection.Close();
				_connection.Open();
				return new GettingResult(false, CommonData.GetErrorMessage(exc));
			}
		    finally
			{
			    dataReader?.Dispose();
			}
		}

		internal GettingResult GelAllStoredIdentifiers()
		{
			IDataReader dataReader = null;
			try
			{
				string sqlString = @"SELECT ""Identifier"" FROM features order by ""Id"" asc";
				IDbCommand command = _connection.CreateCommand();
				command.CommandText = sqlString;
				dataReader = command.ExecuteReader();
				List<Guid> identifiers = new List<Guid>();
				while (dataReader.Read())
				{
					identifiers.Add((Guid)dataReader[0]);
				}
				dataReader.Close();
                command.Dispose();

                GettingResult getResult = new GettingResult(true);
				getResult.List = identifiers;
				return getResult;
			}
			catch (Exception exc)
			{
			    dataReader?.Close();
				_connection.Close();
				_connection.Open();
				return new GettingResult(false, CommonData.GetErrorMessage(exc));
			}
            finally
            {
                dataReader?.Dispose();
            }
        }

		internal bool IsExists(Guid guid)
		{
			var command = _connection.CreateCommand();
			command.CommandText = string.Format("SELECT EXISTS (SELECT * FROM features WHERE \"Identifier\" = '{0}')", guid);
			return (bool)command.ExecuteScalar();
		}


		internal FeatureType GetFeatureType(Guid identifier)
		{
			var command = _connection.CreateCommand();
			command.CommandText =
						"SELECT type_index " +
						"FROM features WHERE \"Identifier\"='" + identifier + "'";
			return (FeatureType)command.ExecuteScalar();

		}

		private void ReadDeltaTable(IDataReader dataReader, List<ChangeIdentifier> list)
		{
			while (dataReader.Read())
			{
				ChangeIdentifier changeIdentifier = new ChangeIdentifier();
				changeIdentifier.Id = (long)dataReader[0];

				changeIdentifier.TimeSlice = new TimeSlice();
				changeIdentifier.TimeSlice.Interpretation = (TimeSliceInterpretationType)((int)(dataReader[1]));
				changeIdentifier.TimeSlice.SequenceNumber = (int)dataReader[2];
				changeIdentifier.TimeSlice.CorrectionNumber = (int)dataReader[3];

				changeIdentifier.TimeSlice.ValidTime = new TimePeriod();
				changeIdentifier.TimeSlice.ValidTime.BeginPosition = (DateTime)dataReader[4];
				object obj = dataReader[5];
				if (!DBNull.Value.Equals(obj))
				{
					changeIdentifier.TimeSlice.ValidTime.EndPosition = (DateTime)obj;
				}

				obj = dataReader[6];
				if (!DBNull.Value.Equals(obj))
				{
					changeIdentifier.CreatedOn = (DateTime)obj;
				}

				obj = dataReader[7];
				if (!DBNull.Value.Equals(obj))
				{
					changeIdentifier.UserName = (string)obj;
				}

				list.Add(changeIdentifier);
			}
		}

		private void AddParameterToDBCommand(IDbCommand command, byte[] geomByteArray)
		{
			IDataParameter dataParam = command.CreateParameter();
			dataParam.DbType = DbType.Binary;
			dataParam.ParameterName = "geomByteA";
			dataParam.Value = geomByteArray;
			command.Parameters.Add(dataParam);
		}

		private void LoadComplexPropertiesOfFeature(IList featList, AimPropInfoList complexPropList)
		{
			foreach (Feature feat in featList)
				LoadComplexPropsViaReflection(feat, complexPropList);
		}

		private void LoadAllComplexPropsOfObj(object obj, AimPropInfo propInfo)
		{
			if (obj == null || (propInfo.IsList && ((IList)obj).Count == 0))
				return;

			if (propInfo.PropType.Properties.Count == 0 || AimMetadata.IsChoice(propInfo.TypeIndex))
				return;

			var complexPropList = new AimPropInfoList();
			foreach (var item in propInfo.PropType.Properties)
			{
				if (item.PropType.AimObjectType != AimObjectType.Field && item.PropType.AimObjectType != AimObjectType.DataType)
					complexPropList.Add(item);
			}

			if (propInfo.IsList)
			{
				IList objList = (IList)obj;
				foreach (var item in objList)
				{
					LoadComplexPropsViaReflection(item, complexPropList);
				}
			}
			else
			{
				LoadComplexPropsViaReflection(obj, complexPropList);
			}
		}

		private void LoadComplexPropsViaReflection(object obj, AimPropInfoList complexPropList)
		{
			foreach (var prop in complexPropList)
			{
				PropertyInfo netPropInfo = obj.GetType().GetProperty(prop.Name);
				object complexObj = netPropInfo.GetValue(obj, null);
				LoadAllComplexPropsOfObj(complexObj, prop);
			}
		}

		private void LoadMetadata(IList featList)
		{
			if (featList.Count == 0)
				return;

			var dict = new Dictionary<long, Feature>();

			var featItem = featList[0] as Feature;
			var featIndex = (int)featItem.FeatureType;
			//var baselineIdsText = "(" + featItem.Id;


			StringBuilder baselineIdsText = new StringBuilder("(" + featItem.Id);

			dict.Add(featItem.Id, featItem);

			for (int i = 1; i < featList.Count; i++)
			{
				featItem = featList[i] as Feature;
				if (featItem == null) continue;

				baselineIdsText.Append("," + featItem.Id);
				dict.Add(featItem.Id, featItem);
			}
			baselineIdsText.Append(")");

			var cmd = _connection.CreateCommand();
			cmd.CommandText = "SELECT bl_feat_id, md_data FROM bl_feat_metadata " +
				"WHERE feat_index = " + featIndex + " AND bl_feat_id IN " + baselineIdsText;

			var dr = cmd.ExecuteReader();

			while (dr.Read())
			{
				if (dr.IsDBNull(0) || dr.IsDBNull(1))
					continue;

				var blFeatId = (long)dr[0];

				if (dict.TryGetValue(blFeatId, out featItem))
				{
					var metadataBuffer = (byte[])dr[1];
					var md = Global.GetMetadata(metadataBuffer);
					featItem.TimeSliceMetadata = md;
				}
			}
			dr.Close();
		}

		/// <summary>
		/// Seperate property list into simple and complex (object) property list and returns sql column names. 
		/// </summary>
		/// <param name="allPropInfoList"></param>
		/// <param name="simplePropList"></param>
		/// <param name="complexPropList"></param>
		/// <param name="dbEntityName">Table name. Add this prefix to columns to identify the table which column belongs to</param>
		/// <returns>Returns sql column names for simple properties</returns>
		internal static string SeparateProperties(AimPropInfoList allPropInfoList, AimPropInfoList simplePropList,
			AimPropInfoList complexPropList, string dbEntityName,
			out Dictionary<AimPropInfo, int> columnIndices)
		{
			List<string> columnList = new List<string>();
			string colName;
			string lowerCasePropName;

			columnIndices = new Dictionary<AimPropInfo, int>();

			foreach (AimPropInfo propInfo in allPropInfoList)
			{
				#region Find complex (object) properties and return to beginning
				if (propInfo.IsList || AimMetadata.IsAbstract(propInfo.TypeIndex))
				{
					complexPropList.Add(propInfo);
					continue;
				}
				IAimProperty aimProp = (IAimProperty)AimObjectFactory.Create(propInfo.TypeIndex);
				if (aimProp.PropertyType == AimPropertyType.Object)
				{
					complexPropList.Add(propInfo);
					continue;
				}
				#endregion

				lowerCasePropName = propInfo.Name.ToLower();
				// These are mandatory fields and that is why these will be added later
				// Also these are only feature properties
				if (lowerCasePropName == "identifier" || lowerCasePropName == "timeslice")
					continue;

				simplePropList.Add(propInfo);
				if (lowerCasePropName == "geo")
				{
					if (dbEntityName != "")
						columnList.Add(string.Format("ST_asEWKB(\"{0}\".\"Geo\"::geometry)", dbEntityName));
					else
						columnList.Add(string.Format("ST_asEWKB(\"Geo\"::geometry)"));
					columnIndices.Add(propInfo, columnList.Count - 1);
				}
				else if (AimMetadata.IsAbstractFeatureRef(propInfo.TypeIndex))
				{
					colName = "";
					if (dbEntityName != "")
						colName += string.Format("\"{0}\".", dbEntityName);
					colName += string.Format("\"ref_{0}\"", propInfo.Name);
					columnList.Add(colName);

					colName = "";
					if (dbEntityName != "")
						colName += string.Format("\"{0}\".", dbEntityName);
					colName += string.Format("\"{0}\"", propInfo.Name);
					columnList.Add(colName);
					columnIndices.Add(propInfo, columnList.Count - 1);
				}
				else if (AimMetadata.IsValClass(propInfo.TypeIndex))
				{
					colName = "";
					if (dbEntityName != "")
						colName += string.Format("\"{0}\".", dbEntityName);
					colName += string.Format("\"{0}\"", propInfo.Name + "_Uom");
					columnList.Add(colName);

					colName = "";
					if (dbEntityName != "")
						colName += string.Format("\"{0}\".", dbEntityName);
					colName += string.Format("\"{0}\"", propInfo.Name + "_Value");
					columnList.Add(colName);
					columnIndices.Add(propInfo, columnList.Count - 1);
				}
				else if (propInfo.TypeIndex == (int)DataType.TextNote)
				{
					// this is TextNote case
					colName = "";
					if (dbEntityName != "")
						colName += string.Format("\"{0}\".", dbEntityName);
					colName += string.Format("\"{0}\"", propInfo.Name + "_Lang");
					columnList.Add(colName);

					colName = "";
					if (dbEntityName != "")
						colName += string.Format("\"{0}\".", dbEntityName);
					colName += string.Format("\"{0}\"", propInfo.Name + "_Value");
					columnList.Add(colName);
					columnIndices.Add(propInfo, columnList.Count - 1);
				}
				else
				{
					colName = "";
					if (dbEntityName != "")
						colName += string.Format("\"{0}\".", dbEntityName);
					colName += string.Format("\"{0}\"", propInfo.Name);
					columnList.Add(colName);
					columnIndices.Add(propInfo, columnList.Count - 1);
				}
			}
			if (columnList.Count > 0)
			{
				string result = "";
				for (int i = 0; i < columnList.Count; i++)
				{
					result += columnList[i] + ",\r\n";
				}
				return result.Remove(result.Length - 3);
			}
			return "";
		}

		private string TimeSliceFilterToSqlCommand(string featName, TimeSliceFilter timeSliceFilter)
		{
			string sqlText = "";
			switch (timeSliceFilter.QueryType)
			{
				case QueryType.ByEffectiveDate:
					sqlText = string.Format(
						"(features.begin_life <= '{1}') AND " +
						"(features.end_life IS NULL OR features.end_life > '{1}') AND " +
						"\"{0}\".begin_valid <= '{1}' AND " +
						"(\"{0}\".end_valid > '{1}' OR " +
						"\"{0}\".end_valid is null)",
						featName, Global.DateTimeToString(timeSliceFilter.EffectiveDate));
					break;

				case QueryType.ByTimePeriod:

					sqlText = string.Format("(" +
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
												Global.DateTimeToString(timeSliceFilter.ValidTime.BeginPosition),
												Global.DateTimeToString(timeSliceFilter.ValidTime.EndPosition.Value));
					break;

				case QueryType.BySequenceNumber:
					sqlText = string.Format("\"{0}\".sequence = {1}", featName, timeSliceFilter.SequenceNumber);
					break;
				default:
					throw new Exception("Not found QueryType !");
			}
			return sqlText;
		}

		/// <summary>
		/// Adds mandatry columns to feature sql command as "Identifier", "Id", "TimeSlice"(properties) and returns result string
		/// </summary>
		/// <param name="featTypeIndex"></param>
		/// <param name="simplePropList"></param>
		/// <returns></returns>
		internal static string AddFeatMandatoryColumns(int featTypeIndex, AimPropInfoList simplePropList,
			Dictionary<AimPropInfo, int> columnIndices)
		{
			AimPropInfo[] allPropInfos = AimMetadata.GetAimPropInfos(featTypeIndex);
			string columns = "";

			int lastIndexOfColumn = 0;

			if (simplePropList.Count > 0)
			{
				lastIndexOfColumn = columnIndices[simplePropList[simplePropList.Count - 1]];
			}

			if (!simplePropList.Exists(propInfo => propInfo.Name.ToLower() == "id"))
			{
				AimPropInfo idPropInfo = GetPropInfo(allPropInfos, "id");
				lastIndexOfColumn++;
				columnIndices.Add(idPropInfo, lastIndexOfColumn);
				simplePropList.Add(idPropInfo);
				columns += string.Format("\"bl_{0}\".\"Id\",\r\n", AimMetadata.GetAimTypeName(featTypeIndex));
			}

			if (!simplePropList.Exists(propInfo => propInfo.Name.ToLower() == "identifier"))
			{
				AimPropInfo identifierPropInfo = GetPropInfo(allPropInfos, "identifier");
				lastIndexOfColumn++;
				columnIndices.Add(identifierPropInfo, lastIndexOfColumn);
				simplePropList.Add(identifierPropInfo);
				columns += "features.\"Identifier\",\r\n";
			}

			if (!simplePropList.Exists(propInfo => propInfo.Name.ToLower() == "timeslice"))
			{
				AimPropInfo timeSlicePropInfo = GetPropInfo(allPropInfos, "timeslice");
				lastIndexOfColumn++;
				columnIndices.Add(timeSlicePropInfo, lastIndexOfColumn);
				simplePropList.Add(timeSlicePropInfo);
				columns += string.Format("features.begin_life," +
											"\r\nfeatures.end_life," +
											"\r\n\"bl_{0}\".sequence," +
											"\r\n\"bl_{0}\".correction," +
											"\r\n\"bl_{0}\".begin_valid," +
											"\r\n\"bl_{0}\".end_valid,\r\n", AimMetadata.GetAimTypeName(featTypeIndex));
			}
			if (columns != "")
			{
				return columns.Remove(columns.Length - 3);
			}
			return "";
		}

		internal static void ReadSimpleProperties(DBEntity dbEntity, IDataReader dataReader,
			AimPropInfoList simplePropInfoList,
			Dictionary<AimPropInfo, int> columnIndices)
		{
			string fieldName;
			IAimObject dbEntityObj = (dbEntity as IAimObject);
			object tmpVal;
			bool hasFoundProp;
			int columnIndex;

			foreach (AimPropInfo propInfo in simplePropInfoList)
			{
				AimObject aimObj = AimObjectFactory.Create(propInfo.TypeIndex);
				IAimProperty aimProp = (aimObj as IAimProperty);
				fieldName = propInfo.Name;
				hasFoundProp = false;
				if (aimProp.PropertyType == AimPropertyType.AranField)
				{
					AimField aimField = (aimProp as AimField);
					IEditAimField editAimField = (aimProp as IEditAimField);
					columnIndex = columnIndices[propInfo];
					tmpVal = dataReader[(int)columnIndex];
					if (fieldName == "Geo")
					{
						if (!DBNull.Value.Equals(tmpVal))
						{
							byte[] bytea = (byte[])tmpVal;
							GeometryWKBReader wkbReader = new GeometryWKBReader();
							Aran.Geometries.Geometry geom = wkbReader.Create(bytea);
							editAimField.FieldValue = geom;
							dbEntityObj.SetValue(propInfo.Index, aimProp);
						}
					}
					else
					{
						if (!DBNull.Value.Equals(tmpVal))
						{
							if (propInfo.TypeIndex == (int)AimFieldType.SysUInt32)
								tmpVal = Convert.ToUInt32(tmpVal);
							editAimField.FieldValue = tmpVal;
							dbEntityObj.SetValue(propInfo.Index, aimProp);
						}
					}
					hasFoundProp = true;
				}
				else if (aimProp.PropertyType == AimPropertyType.DataType)
				{
					if (propInfo.AixmName == "timeSlice")
					{
						TimeSlice timeSlice = (aimProp as TimeSlice);
						columnIndex = columnIndices[propInfo];
						// columnIndex is begin_life field index
						// index of end_life is columnIndex + 1
						// index of sequence is columnIndex + 2
						// index of correction is columnIndex + 3
						// index of begin_valid is columnIndex + 4
						// index of end_valid is columnIndex + 5
						timeSlice.SequenceNumber = (int)dataReader[columnIndex + 2];
						timeSlice.CorrectionNumber = (int)dataReader[columnIndex + 3];
#warning BASELINE always set as satatic value.
						timeSlice.Interpretation = TimeSliceInterpretationType.BASELINE;
						timeSlice.ValidTime = new TimePeriod();
						timeSlice.ValidTime.BeginPosition = (DateTime)dataReader[columnIndex + 4];
						tmpVal = dataReader[columnIndex + 5];
						if (!DBNull.Value.Equals(tmpVal))
							timeSlice.ValidTime.EndPosition = (DateTime)tmpVal;
						timeSlice.FeatureLifetime = new TimePeriod();
						timeSlice.FeatureLifetime.BeginPosition = (DateTime)dataReader[columnIndex];
						tmpVal = dataReader[columnIndex + 1];
						if (!DBNull.Value.Equals(tmpVal))
							timeSlice.FeatureLifetime.EndPosition = (DateTime)tmpVal;
						dbEntityObj.SetValue(propInfo.Index, timeSlice);
						hasFoundProp = true;
					}
					else if (AimMetadata.IsAbstractFeatureRef(propInfo.TypeIndex))
					{
						IAbstractFeatureRef absFeatRef = (aimProp as IAbstractFeatureRef);
						columnIndex = columnIndices[propInfo];
						// columnIndex is column index and columnIndex - 1 is ref_ColumnName(column of type of abstract)
						tmpVal = dataReader[(int)columnIndex];
						if (!DBNull.Value.Equals(tmpVal))
						{
							absFeatRef.Identifier = (Guid)tmpVal;
							tmpVal = dataReader[columnIndex - 1];
							absFeatRef.FeatureTypeIndex = (int)tmpVal;
							dbEntityObj.SetValue(propInfo.Index, aimProp);
						}
						hasFoundProp = true;
					}
					else if (aimProp is IEditValClass)
					{
						IEditValClass valClass = (aimProp as IEditValClass);
						columnIndex = columnIndices[propInfo];
						int foundDataTypeProp = 0;
						tmpVal = dataReader[(int)columnIndex];
						if (!DBNull.Value.Equals(tmpVal))
						{
							valClass.Value = Convert.ToDouble(tmpVal);
							foundDataTypeProp++;
						}
						tmpVal = dataReader[(int)columnIndex - 1];
						if (!DBNull.Value.Equals(tmpVal))
						{
							valClass.Uom = (int)tmpVal;
							foundDataTypeProp++;
						}
						if (foundDataTypeProp == 2)
							dbEntityObj.SetValue(propInfo.Index, aimProp);
						hasFoundProp = true;
					}
					else if (propInfo.IsFeatureReference)
					{
						FeatureRef featRef = (aimProp as FeatureRef);
						columnIndex = columnIndices[propInfo];
						tmpVal = dataReader[(int)columnIndex];
						if (!DBNull.Value.Equals(tmpVal))
						{
							featRef.Identifier = (Guid)tmpVal;
							dbEntityObj.SetValue(propInfo.Index, featRef);
						}
						hasFoundProp = true;
					}
					else
					{
						TextNote txtNote = (aimProp as TextNote);
						columnIndex = columnIndices[propInfo];
						tmpVal = dataReader[(int)columnIndex];
						if (!DBNull.Value.Equals(tmpVal))
						{
							txtNote.Value = (string)tmpVal;
						}
						tmpVal = dataReader[(int)columnIndex - 1];
						if (!DBNull.Value.Equals(tmpVal))
						{
							txtNote.Lang = (language)tmpVal;
						}
						dbEntityObj.SetValue(propInfo.Index, txtNote);
						hasFoundProp = true;
					}
				}
				if (!hasFoundProp)
					throw new Exception("This property has not found (" + propInfo.Name + ")");
			}
		}

		internal static IList GetDBEntityList(int dbEntityTypeIndex, IDataReader dataReader,
										AimPropInfoList propInfoList, string geogTableName,
										Dictionary<AimPropInfo, int> columnIndices,
										IDbConnection connection,
										List<long> refObjectIdList)
		{
			IList result = AimObjectFactory.CreateList(dbEntityTypeIndex);

			ComplexLoaderDataListener myListener = new ComplexLoaderDataListener();
			myListener.List = result;
			myListener.Connection = connection;

			int fieldCount = dataReader.FieldCount;

			while (dataReader.Read())
			{
				AimObject aObj = AimObjectFactory.Create(dbEntityTypeIndex);
				(aObj as IEditDBEntity).Listener = myListener;

				ReadSimpleProperties((DBEntity)aObj, dataReader, propInfoList, columnIndices);
				result.Add(aObj);

				if (refObjectIdList != null)
				{
					long refId = dataReader.GetInt64(fieldCount - 1);
					refObjectIdList.Add(refId);
				}
			}
			return result;
		}

		internal static ChoiceRef GetChoiceRef(IDataReader dataReader)
		{
			ChoiceRef result = new ChoiceRef();
			result.PropType = (int)dataReader["prop_type"];
			result.ValueType = (int)dataReader["choice_type"];
			if (AimMetadata.GetAimObjectType(result.ValueType) == AimObjectType.Feature)
			{
				result.IsFeature = true;
				Guid identifier = (Guid)dataReader["target_guid"];
				if (AimMetadata.IsAbstractFeatureRef(result.PropType))
				{
					result.AimObj = AimObjectFactory.Create(result.PropType);
					(result.AimObj as IAbstractFeatureRef).Identifier = identifier;
					(result.AimObj as IAbstractFeatureRef).FeatureTypeIndex = result.ValueType;
				}
				else
					result.AimObj = new FeatureRef(identifier);
			}
			else
			{
				result.AimObj = AimObjectFactory.Create(result.ValueType);
				result.IsFeature = false;
				result.Id = (long)dataReader["target_id"];
			}
			return result;
		}

		/// <summary>
		/// Returns AimPropInfo whose name is "lowerCaseName"
		/// </summary>
		/// <param name="propInfos"></param>
		/// <param name="lowerCaseName"></param>
		/// <returns></returns>
		internal static AimPropInfo GetPropInfo(AimPropInfo[] propInfos, string lowerCaseName)
		{
			foreach (AimPropInfo propInfo in propInfos)
			{
				string tmp = propInfo.Name.ToLower();
				if (tmp == lowerCaseName)
					return propInfo;
			}
			return null;
		}

		internal static AimPropInfo GetPropInfo(int aimObjIndex, int propInfoIndex)
		{
			AimPropInfo[] propInfos = AimMetadata.GetAimPropInfos(aimObjIndex);
			foreach (AimPropInfo propInfo in propInfos)
			{
				if (propInfo.Index == propInfoIndex)
				{
					return propInfo;
				}
			}
			return null;
		}

		private IDbConnection _connection;
		private TimeSliceFilter _timeSliceFilter;
	}
}