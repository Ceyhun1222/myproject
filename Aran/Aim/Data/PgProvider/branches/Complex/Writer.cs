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
using System.Text;
using System.Linq;

namespace Aran.Aim.Data
{
    internal class Writer
    {
        internal Writer (IDbConnection connection)
        {
            _connection = connection;
            byteaGeomParamName = "geomByteA";
            _dbCommandDict = new Dictionary<int, IDbCommand> ();
            _transGuidDict = new Dictionary<int, Guid> ();
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
        /// 

        internal InsertingResult Insert (Feature feat, int transactionId, bool insertAnyway, bool asCorrection)
        {
            IDbCommand dbCommand = null;
            try
            {
                if (!_dbCommandDict.ContainsKey (transactionId))
                {
                    return new InsertingResult (false, "Transaction Id doesn't exists !");
                }

                dbCommand = _dbCommandDict [transactionId];

                if (dbCommand == null)
                {
                    dbCommand = _connection.CreateCommand ();
                    dbCommand.Transaction = _connection.BeginTransaction ();
                    _dbCommandDict [transactionId] = dbCommand;
                }

                dbCommand.Parameters.Clear ();

                var tableName = "bl_" + feat.FeatureType;

                dbCommand.CommandText = string.Format("SELECT \"Id\" FROM features WHERE \"Identifier\"='{0}'", feat.Identifier);
                var featId = (long?)dbCommand.ExecuteScalar();

                var isDecommision = (feat.TimeSlice.FeatureLifetime.EndPosition != null);
                var beginValid = (isDecommision ? (DateTime?)DateTime.Now : null);

                var seqNum = feat.TimeSlice.SequenceNumber;
                var corrNum = GetNexSeqOrCorrNumber(tableName, featId, asCorrection, ref seqNum, ref beginValid);
                
                feat.TimeSlice.SequenceNumber = seqNum;
                feat.TimeSlice.CorrectionNumber = corrNum;

                if (isDecommision) {
                    var endLife = feat.TimeSlice.FeatureLifetime.EndPosition.Value;

                    if (endLife < beginValid)
                        return new InsertingResult("Decomissioning date can not be before begin of valid time");

                    return DecommissionFeature(featId.Value, seqNum, endLife, dbCommand, tableName);
                }

                if (feat.TimeSlice.SequenceNumber == 1) {
                    if (feat.TimeSlice.ValidTime.BeginPosition != feat.TimeSlice.FeatureLifetime.BeginPosition) {
                        return new InsertingResult("Begin of lifetime must be equal to begin of valid time for commisioning Feature.");
                    }
                }
                else {
                    //if (feat.TimeSlice.ValidTime.BeginPosition <= feat.TimeSlice.FeatureLifetime.BeginPosition)
                    //    return new InsertingResult("Begin of valid time cannot be before or equal to begin of Lifetime.");
                }


                List<int> diffIndexList = new List<int> ();
                Feature prevFeat = GetStoredLastVersion (feat);
                var result = GetDifferencesWithLastSequence (feat, diffIndexList);
            
                if (result != string.Empty)
                    return new InsertingResult (false, result);

                IAimObject aimObject = feat as IAimObject;
                string sqlText = string.Empty;
                List<string> sqlTextInsertObjs_in2_LinkTable = new List<string> ();
                List<string> sqlTextInsertFeats_in2_LinkTable = new List<string> ();

                if (feat.TimeSlice.SequenceNumber == 1)
                {
                    var transGuid = _transGuidDict [transactionId];
                    result = InsertIntofeaturesTable (feat, dbCommand.Transaction, false, transGuid);
                    if (result != string.Empty)
                    {
                        return new InsertingResult (false, result);
                    }
                }

                byte [] geomByteArray;
                sqlText = CreateSqlText_4_DBEntity (aimObject, prevFeat as IAimObject, diffIndexList, feat.TimeSlice.CorrectionNumber,
                                                    sqlTextInsertObjs_in2_LinkTable, sqlTextInsertFeats_in2_LinkTable,
                                                    dbCommand.Transaction, out geomByteArray);

                if (feat.TimeSlice.Interpretation == TimeSliceInterpretationType.PERMDELTA || 
                        feat.TimeSlice.Interpretation == TimeSliceInterpretationType.BASELINE)
                {
                    dbCommand.CommandText = sqlText + ";";
                    dbCommand.CommandText += _sqlTextRunAtLast;
                    _sqlTextRunAtLast = "";
                    dbCommand.ExecuteNonQuery ();

                    string WHEREClause = string.Format (" sequence = {0} AND feat_id = (SELECT \"Id\" FROM features WHERE \"Identifier\" = '{1}')",
                                                            feat.TimeSlice.SequenceNumber, feat.Identifier);
                    feat.Id = GetId (tableName, WHEREClause, dbCommand.Transaction);

                    if (sqlTextInsertObjs_in2_LinkTable.Count > 0 || sqlTextInsertFeats_in2_LinkTable.Count > 0)
                    {
                        InserLinkTable(feat.Id, tableName,
                            sqlTextInsertObjs_in2_LinkTable, sqlTextInsertFeats_in2_LinkTable, dbCommand.Transaction);
					}

                    SetEndOfValid(tableName, featId, feat.Identifier);

					SetFeatureMetadata (feat, dbCommand.Transaction);
				}

                return new InsertingResult (true, string.Empty);
            }
            catch (Exception except)
            {
                string message = CommonData.GetErrorMessage (except);
                return new InsertingResult (false, message);
            }
        }

		internal void SetFeatureMetadata (Feature feat, IDbTransaction trans)
		{
			var metadataBuffer = Global.GetBytes (feat.TimeSliceMetadata);

			var cmd = _connection.CreateCommand ();
			cmd.Transaction = trans;

			cmd.CommandText = string.Format ("SELECT EXISTS (" +
				"SELECT * FROM bl_feat_metadata " +
				"WHERE feat_index = {0} AND bl_feat_id = {1})", (int) feat.FeatureType, feat.Id);
			var isExists = (bool) cmd.ExecuteScalar ();

			if (isExists)
			{
				cmd.CommandText = "UPDATE bl_feat_metadata SET md_data = :md_data " +
					"WHERE bl_feat_id = :bl_feat_id AND feat_index = :feat_index";
			}
			else
			{
				cmd.CommandText = "INSERT INTO bl_feat_metadata " +
						"( bl_feat_id,  feat_index,  md_data) VALUES " +
						"(:bl_feat_id, :feat_index, :md_data)";
			}

			var npgCmd = cmd as Npgsql.NpgsqlCommand;
			npgCmd.Parameters.AddWithValue ("bl_feat_id", feat.Id);
			npgCmd.Parameters.AddWithValue ("feat_index", (int) feat.FeatureType);
			var param = new Npgsql.NpgsqlParameter ("md_data", NpgsqlTypes.NpgsqlDbType.Bytea);
			param.Value = metadataBuffer;
			npgCmd.Parameters.Add (param);

			cmd.ExecuteNonQuery ();
		}

        internal InsertingResult Update (Feature feat, int transactionId)
        {
            IDbCommand dbCommand = null;
            try
            {
                if (!_dbCommandDict.ContainsKey (transactionId))
                {
                    return new InsertingResult (false, "Transaction Id doesn't exists !");
                }

                dbCommand = _dbCommandDict [transactionId];
                if (dbCommand == null)
                {
                    dbCommand = _connection.CreateCommand ();
                    dbCommand.Transaction = _connection.BeginTransaction ();
                    _dbCommandDict [transactionId] = dbCommand;
                }

                dbCommand.Parameters.Clear ();

                //int sequence = 0;
                //int correction = 0;
                string result;
                //result = GetCurrVersion ( feat, ref sequence, ref correction );
                //if ( result != string.Empty )
                //{
                //    return new InsertingResult ( false, result );
                //}
                //result = AdaptFeatTimeSlice ( feat, insertAnyway, asCorrection, sequence, correction );
                //if ( result != string.Empty )
                //{
                //    return new InsertingResult ( false, result );
                //}

                List<int> diffIndexList = new List<int> ();
                Feature prevFeat = GetStoredLastVersion (feat);
                diffIndexList.AddRange ((prevFeat as AimObject).GetDifferences (feat, true));
                if (AimObject.IsEquals ((prevFeat as Feature).TimeSlice.FeatureLifetime, feat.TimeSlice.FeatureLifetime))
                    diffIndexList.Remove ((int) PropertyFeature.TimeSlice);

                IAimObject aimObject = feat as IAimObject;
                string sqlText = string.Empty;
                List<string> sqlTextInsertObjs_in2_LinkTable = new List<string> ();
                List<string> sqlTextInsertFeats_in2_LinkTable = new List<string> ();

                if (feat.TimeSlice.SequenceNumber == 1)
                {
                    result = InsertIntofeaturesTable (feat, dbCommand.Transaction, true, Guid.Empty);
                    if (result != string.Empty)
                    {
                        return new InsertingResult (false, result);
                    }
                }

                byte [] geomByteArray;

                sqlText = CreateSqlText_4_DBEntity (aimObject, prevFeat as IAimObject, diffIndexList, feat.TimeSlice.CorrectionNumber,
                                                    sqlTextInsertObjs_in2_LinkTable, sqlTextInsertFeats_in2_LinkTable,
                                                    dbCommand.Transaction, out geomByteArray, true);

                if (feat.TimeSlice.Interpretation == TimeSliceInterpretationType.PERMDELTA ||
						feat.TimeSlice.Interpretation == TimeSliceInterpretationType.BASELINE)
                {
                    dbCommand.CommandText = sqlText + ";" + _sqlTextRunAtLast;
                    _sqlTextRunAtLast = "";
                    dbCommand.ExecuteNonQuery ();

                    string WHEREClause = string.Format (" sequence = {0} AND feat_id = (SELECT \"Id\" FROM features WHERE \"Identifier\" = '{1}')",
                                                            feat.TimeSlice.SequenceNumber, feat.Identifier);
                    long newFeatureId = GetId ("bl_" + feat.FeatureType, WHEREClause, dbCommand.Transaction);
                    feat.Id = newFeatureId;

                    if (sqlTextInsertObjs_in2_LinkTable.Count > 0 || sqlTextInsertFeats_in2_LinkTable.Count > 0)
                    {
                        InserLinkTable (newFeatureId, "bl_" + feat.FeatureType,
                            sqlTextInsertObjs_in2_LinkTable, sqlTextInsertFeats_in2_LinkTable, dbCommand.Transaction);
                    }
                }

                //_dbCommandDictionary [ transactionId ].Add ( dbCommand );
                return new InsertingResult (true, string.Empty);
            }
            catch (Exception except)
            {
                string message = CommonData.GetErrorMessage (except);
                return new InsertingResult (false, message);
            }
        }

		internal GettingResult DeleteFeatIdentifiers ( List<Guid> identifierList )
		{
			IDataReader dataReader = null;
			try
			{
				StringBuilder sqlString = new StringBuilder( @"DELETE FROM features WHERE ""Identifier"" in (");
				foreach ( Guid identifier in identifierList )
				{
					sqlString.Append ( "'" + identifier + "'," );
				}
				sqlString.Remove ( sqlString.Length - 1, 1 );
				sqlString.Append ( ")" );

				IDbCommand command = _connection.CreateCommand ( );
				command.CommandText = sqlString.ToString ( );
				int countRow = command.ExecuteNonQuery ( );
				GettingResult getResult = new GettingResult ( );
				getResult.List = new List<int> ( );
				getResult.List.Add ( countRow );
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

        internal InsertingResult Delete (Feature feature)
        {
            throw new NotImplementedException ();
        }

        internal int BeginTransaction ()
        {
            _transactionIndex++;
            _dbCommandDict.Add (_transactionIndex, null);
            _transGuidDict.Add (_transactionIndex, Guid.NewGuid ());
            return _transactionIndex;
        }

        internal InsertingResult Commit (int transactionId)
        {
            if (_dbCommandDict.ContainsKey (transactionId))
            {
                var dbCommand = _dbCommandDict [transactionId];
                dbCommand.Transaction.Commit ();

                return new InsertingResult (true);
            }
            return new InsertingResult (false, "Transaction Id doesn't exists !");
        }

        internal InsertingResult Rollback (int transactionId)
        {
            if (_dbCommandDict.ContainsKey (transactionId))
            {
                _dbCommandDict [transactionId].Transaction.Rollback ();
            }
            return new InsertingResult (false, "Transaction Id doesn't exists !");
        }

        internal InsertingResult InsertUser (User user, IDbCommand dbCommand = null, IDbTransaction transaction = null)
        {
            try
            {
                if (transaction == null)
                {
                    transaction = _connection.BeginTransaction ();
                }

                if (dbCommand == null)
                {
                    dbCommand = _connection.CreateCommand ();
                    dbCommand.Transaction = transaction;
                }
                dbCommand.CommandText = string.Format ("INSERT INTO aim_user (name, password, privilege) VALUES('{0}','{1}',{2});", user.Name, user.Password, (int) user.Privilege);
                dbCommand.ExecuteScalar ();
                user.Id = CurrValOfSequence ("aim_user", transaction);

                dbCommand.CommandText = "INSERT INTO aim_user_feat_types(user_id,feat_type) VALUES ";
                StringBuilder stringBuilder = new StringBuilder ();
                foreach (int featType in user.FeatureTypes)
                {
                    stringBuilder.Append (" (" + user.Id + ", " + featType + ") ,");
                }
                dbCommand.CommandText += stringBuilder.Remove (stringBuilder.Length - 1, 1).ToString ();
                dbCommand.ExecuteScalar ();
                dbCommand.Transaction.Commit ();
                return new InsertingResult (true);
            }
            catch (Exception ex)
            {
                dbCommand.Transaction.Rollback ();
                string message = CommonData.GetErrorMessage (ex);
                return new InsertingResult (false, message);
            }
        }

        internal InsertingResult UpdateUser (User user)
        {
            IDbCommand dbCommand = null;
            try
            {
                IDbTransaction transaction = _connection.BeginTransaction ();
                dbCommand = _connection.CreateCommand ();
                dbCommand.Transaction = transaction;
                //dbCommand.CommandText = "DELETE FROM aim_user WHERE \"Id\" = " + user.Id;
                //dbCommand.ExecuteScalar ( );
                //InsertUser ( user, dbCommand, transaction );


                dbCommand.CommandText = "DELETE FROM aim_user_feat_types WHERE user_id = " + user.Id + ";";
                dbCommand.ExecuteScalar ();

                dbCommand.CommandText = string.Format ("UPDATE aim_user SET name = '{0}', password = '{1}', privilege = '{2}' WHERE \"Id\" = {3}", user.Name, user.Password, (int) user.Privilege, user.Id);
                dbCommand.ExecuteNonQuery ();


                dbCommand.CommandText = "INSERT INTO aim_user_feat_types(user_id,feat_type) VALUES ";
                StringBuilder stringBuilder = new StringBuilder ();
                foreach (FeatureType featType in user.FeatureTypes)
                {
                    stringBuilder.Append (" (" + user.Id + ", " + ((int) featType).ToString () + "),");
                }
                dbCommand.CommandText += stringBuilder.Remove (stringBuilder.Length - 1, 1).ToString ();
                dbCommand.ExecuteScalar ();
                dbCommand.Transaction.Commit ();
                return new InsertingResult (true);
            }
            catch (Exception ex)
            {
                dbCommand.Transaction.Rollback ();
                string message = CommonData.GetErrorMessage (ex);
                return new InsertingResult (false, message);
            }
        }

        internal InsertingResult DeleteUser (User user)
        {
            IDbCommand dbCommand = null;
            try
            {
                dbCommand = _connection.CreateCommand ();
                dbCommand.CommandText = string.Format ("DELETE FROM aim_user WHERE \"Id\" = {0}", user.Id);
                dbCommand.ExecuteScalar ();
                return new InsertingResult (true);
            }
            catch (Exception ex)
            {
                string message = CommonData.GetErrorMessage (ex);
                return new InsertingResult (false, message);
            }
        }

        public User User
        {
            get;
            set;
        }

        private Feature GetStoredLastVersion (Feature feat)
        {
            //if ( feat.TimeSlice.SequenceNumber == 1 && feat.TimeSlice.CorrectionNumber == 0 )
            //    return null;

            TimeSliceFilter timeSliceFilter = new TimeSliceFilter (feat.TimeSlice.SequenceNumber);

            Reader reader = new Reader (_connection);
            reader.TimeSliceFilter = timeSliceFilter;


            Feature prevFeat;
            GettingResult getResult = reader.VersionsOf (feat.FeatureType,
                                            TimeSliceInterpretationType.BASELINE,
                                            feat.Identifier,
                                            true,
                                            null,
                                            null,
                                            null);
            if (getResult.IsSucceed)
            {
                if (getResult.List.Count > 0)
                {
                    prevFeat = (getResult.List [0] as Feature);
                    return prevFeat;
                }
                return null;
            }
            throw new Exception (getResult.Message);
        }

        private string GetDifferencesWithLastSequence (Feature feat, List<int> diffIndexList)
        {
            if (feat.TimeSlice.SequenceNumber > 1)
            {
                TimeSliceFilter timeSliceFilter = new TimeSliceFilter (feat.TimeSlice.SequenceNumber - 1);
                return GetDifferences (feat, diffIndexList, timeSliceFilter);
            }
            else
            {
                int [] indices = (feat as IAimObject).GetPropertyIndexes ();
                diffIndexList.AddRange (indices);
                return string.Empty;
            }
        }

        private string GetDifferences (Feature feat, List<int> diffIndexList, TimeSliceFilter timeSliceFilter)
        {
            Reader reader = new Reader (_connection);
            reader.TimeSliceFilter = timeSliceFilter;
            GettingResult getResult = reader.VersionsOf (feat.FeatureType,
                                            TimeSliceInterpretationType.BASELINE,
                                            feat.Identifier,
                                            true,
                                            null,
                                            null,
                                            null);
            if (getResult.IsSucceed)
            {
                if (getResult.List.Count > 0)
                {
                    diffIndexList.AddRange ((getResult.List [0] as AimObject).GetDifferences (feat, true));
                    if (AimObject.IsEquals ((getResult.List [0] as Feature).TimeSlice.FeatureLifetime, feat.TimeSlice.FeatureLifetime))
                        diffIndexList.Remove ((int) PropertyFeature.TimeSlice);
                    return string.Empty;
                }
                int [] indices = (feat as IAimObject).GetPropertyIndexes ();
                diffIndexList.AddRange (indices);
                return string.Empty;
            }
            return getResult.Message;
        }

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
        private string AdaptFeatTimeSlice(Feature feat, bool InsertAnyway, bool asCorrection, int seq, int corr)
        {
            if (InsertAnyway) {
                if (asCorrection) {
                    if (seq != 0) {
                        //feat.TimeSlice.SequenceNumber = seq;
                        feat.TimeSlice.CorrectionNumber = corr + 1;
                    }
                    else {
                        feat.TimeSlice.SequenceNumber = 1;
                        feat.TimeSlice.CorrectionNumber = 0;
                    }
                }
                else {
                    feat.TimeSlice.SequenceNumber = seq + 1;
                    feat.TimeSlice.CorrectionNumber = 0;
                }
            }
            else if (!
                       (
                        (feat.TimeSlice.SequenceNumber - seq == 1 && feat.TimeSlice.CorrectionNumber == 0) ||
                        (feat.TimeSlice.SequenceNumber > 0 && feat.TimeSlice.SequenceNumber == seq && feat.TimeSlice.CorrectionNumber - corr == 1)
                       )
                    ) {
                return "TimeSlice property is not compatible with database!";
            }
            return string.Empty;
        }

        private int GetNexSeqOrCorrNumber(string tableName, long? featId, bool asCorrection, ref int seq, ref DateTime? beginValid)
        {
            if (featId == null) {
                seq = 1;
                return 0;
            }
            var cmd = _connection.CreateCommand();

            if (asCorrection) {
                cmd.CommandText = string.Format(
                    "SELECT MAX(correction) FROM \"{0}\" WHERE feat_id = {1} AND sequence = {2}", 
                    tableName, featId, seq);

                return (int)cmd.ExecuteScalar() + 1;
            }
            else {
                cmd.CommandText = string.Format(
                    "SELECT MAX(sequence) FROM \"{0}\" WHERE feat_id = {1}",
                    tableName, featId);
                
                seq = (int)cmd.ExecuteScalar();

                if (beginValid != null) {
                    cmd.CommandText = string.Format(
                        "SELECT begin_valid FROM \"{0}\" WHERE feat_id = {1} AND sequence = {2} AND end_valid IS NULL",
                        tableName, featId, seq);
                    beginValid = (DateTime)cmd.ExecuteScalar();
                }

                seq++;
                return 0;
            }
            
        }

        /// <summary>
        /// Creates commont sql commands for INSERTing to table AND returns its linkTable's INSERTing sql command
        /// </summary>
        /// <param name="sqlInsertObjs_in2_linkTable">Sql texts for INSERTing objects INTO link table.
        /// <param name="sqlInsertFeats_in2_linkTable">Sql texts for INSERTing features INTO link table.
        /// <returns></returns>
        private string CreateSqlText_4_DBEntity (IAimObject aimObj,
                                                IAimObject prevObj,
                                                List<int> diffIndexList,
                                                int correction,
                                                List<string> sqlInsertObj_in2_linkTable,
                                                List<string> sqlInsertFeats_in2_linkTable,
                                                IDbTransaction transaction,
                                                out byte [] byteaGeom,
                                                bool isUpdate = false)
        {
            byteaGeom = null;
            List<AimPropInfo> filledPropInfoList = GetFilledPropInfos (aimObj);
            if (filledPropInfoList.Count == 0)
                return string.Empty;

            string primaryTableName = string.Empty;
            bool isFeature = false;
            Guid guid = new Guid ();

            if (aimObj is Feature)
            {
                isFeature = true;
                primaryTableName = "bl_" + (aimObj as Feature).FeatureType.ToString();
            }
            else
            {
                primaryTableName = CommonData.GetTableName(aimObj);
            }

            List<string> primaryColumns = new List<string> ();
            List<string> primaryValues =  new List<string> ();
            int targetTableIndex;
            IAimProperty property;
            object value;
            IEditAimField propEditAimField;
            AimField propAimField;
            int interpretation = 0, sequence = 0;
            DateTime begin_valid = DateTime.MaxValue;
            DateTime? end_valid = null;
            List<long> insertedIds = new List<long> ();

            SortedDictionary<int, byte []> deltaPropValues = new SortedDictionary<int, byte []> ();

            Guid ref_Guid;
            long ref_ID;
            DeltaProp deltaProp = new DeltaProp ();
            long id = 0;
            AimObject prevPropObj = null;

            foreach (AimPropInfo propInfo in filledPropInfoList)
            {
                property = aimObj.GetValue (propInfo.Index);
                value = string.Empty;
				if ( property is PointExtension )
					continue;

				switch ( property.PropertyType)
                {
                    case AimPropertyType.AranField:

                        #region AranField

                        propEditAimField = property as IEditAimField;
                        if (propInfo.Name.ToLower () == "id")
                        {
                            id = (long) propEditAimField.FieldValue;
                            continue;
                        }

                        if (propInfo.AixmName == "identifier")
                        {
                            guid = (Guid) propEditAimField.FieldValue;
                            if (diffIndexList.Contains (propInfo.Index))
                            {
                                deltaPropValues.Add (propInfo.Index, deltaProp.ToByteArray (guid));
                            }
                        }
                        else if (propInfo.Name == "Geo")
                        {
                            primaryColumns.Add (propInfo.Name);
                            Geometry geom = (Aran.Geometries.Geometry) propEditAimField.FieldValue;
                            switch (((AimField) propEditAimField).FieldType)
                            {
                                case AimFieldType.GeoPoint:

                                    break;
                                case AimFieldType.GeoPolyline:
                                    MultiLineString mltLnString = (MultiLineString) geom;
                                    if (mltLnString.Count == 0)
                                        throw new Exception("GeoPolyline requires minimum one LineString");
                                    foreach (LineString lnString in mltLnString)
                                    {
                                        if (lnString.Count < 2)
                                            throw new Exception ("LineString in GeoPolyline requires minimum 2 points");
                                    }
                                    break;
                                case AimFieldType.GeoPolygon:
                                    MultiPolygon mltPolygon = (MultiPolygon) geom;
                                    if (mltPolygon.Count == 0)
                                        throw new Exception ("GeoPolygon requires minimum one Polygon");
                                    foreach (Polygon polygon in mltPolygon)
                                    {
                                        if (polygon.ExteriorRing == null || polygon.ExteriorRing.Count< 3)
                                            throw new Exception ("Exterior ring in Polygon (in MultiPolygon) requires minimum 3 points");
                                    }
                                    break;
                                default:
                                    break;
                            }

                            string pntSql = "ST_GeomFromEWKB(:" + byteaGeomParamName + ")";
                            GeometryWKBWriter geomWriter = new Aran.Geometries.IO.GeometryWKBWriter ();
                            geomWriter.Write (geom, ByteOrder.LittleEndian);
                            byteaGeom = geomWriter.GetByteArray ();
                            primaryValues.Add (pntSql);
                        }
                        else
                        {
                            primaryColumns.Add (propInfo.Name);
                            value = propEditAimField.FieldValue;
                            propAimField = (property as AimField);

                            byte [] deltaVal  = new byte [0];
                            if ((property as AimField).FieldType == AimFieldType.SysEnum)
                            {
                                value = (int) value;
                            }

                            if (isFeature && diffIndexList.Contains (propInfo.Index))
                            {
                                deltaPropValues.Add (propInfo.Index, deltaProp.ToByteArray (value));
                            }

                            if (propAimField.FieldType == AimFieldType.SysString)
                                value = "'" + AdaptQuotedStrIntoPg ((string) value) + "'";
                            else if (propAimField.FieldType == AimFieldType.SysDateTime)
                            {
                                value = "'" + Global.DateTimeToString ((DateTime) value) + "'";
                            }

                            primaryValues.Add (value.ToString ());
                        }
                        #endregion
                        break;

                    case AimPropertyType.DataType:

                        #region DataType
                        if (AimMetadata.IsAbstractFeatureRef (propInfo.TypeIndex))
                        {
                            primaryColumns.Add (propInfo.Name);
                            ref_Guid =  (property as FeatureRef).Identifier;
                            primaryValues.Add ("'" + ref_Guid + "'");

                            primaryColumns.Add ("ref_" + propInfo.Name);
                            primaryValues.Add ((property as IAbstractFeatureRef).FeatureTypeIndex.ToString ());

                            if (isFeature && diffIndexList.Contains (propInfo.Index))
                            {
                                deltaPropValues.Add (propInfo.Index, deltaProp.ToByteArray (property));
                            }
                        }
                        else if (propInfo.AixmName == "timeSlice")
                        {
                            TimeSlice timeSlice = (property as TimeSlice);

                            // It will write featureLifeTime property into deltaProperties table as TimeSlice index 
                            // Because this is changable property
                            if (diffIndexList.Contains (propInfo.Index))
                            {
                                deltaPropValues.Add (propInfo.Index, deltaProp.ToByteArray (timeSlice.FeatureLifetime));
                            }

                            primaryColumns.Add ("begin_valid");
                            begin_valid = timeSlice.ValidTime.BeginPosition;
                            primaryValues.Add ("'" + Global.DateTimeToString (begin_valid) + "'");


                            end_valid = timeSlice.ValidTime.EndPosition;
                            if (end_valid.HasValue)
                            {
                                primaryColumns.Add ("end_valid");
                                primaryValues.Add ("'" + Global.DateTimeToString (end_valid.Value) + "'");
                            }

                            primaryColumns.Add ("sequence");
                            sequence = timeSlice.SequenceNumber;
                            primaryValues.Add (sequence.ToString ());

                            primaryColumns.Add ("correction");
                            primaryValues.Add (correction.ToString ());

                            interpretation = (int) timeSlice.Interpretation;
                        }
                        else if (property is IEditValClass)
                        {
                            primaryColumns.Add (propInfo.Name + "_Value");
                            double valClassValue = (property as IEditValClass).Value;
                            primaryValues.Add (valClassValue.ToString ());

                            primaryColumns.Add (propInfo.Name + "_Uom");
                            int valClassUom = (property as IEditValClass).Uom;
                            primaryValues.Add (valClassUom.ToString ());

                            double valClassSIValue =  ConverterToSI.Convert (property, double.NaN);
                            if (double.IsNaN (valClassSIValue))
                            {
                                throw new Exception ("Cann't convert " + propInfo.Name + " !");
                            }
                            primaryColumns.Add (propInfo.Name + "_SIValue");
                            primaryValues.Add (valClassSIValue.ToString ());

                            if (isFeature && diffIndexList.Contains (propInfo.Index))
                            {
                                deltaPropValues.Add (propInfo.Index, deltaProp.ToByteArray (property));
                            }
                        }
                        else if (propInfo.IsFeatureReference)
                        {
                            primaryColumns.Add (propInfo.Name);
                            ref_Guid = (property as FeatureRef).Identifier;
                            if (isFeature && diffIndexList.Contains (propInfo.Index))
                            {
                                deltaPropValues.Add (propInfo.Index, deltaProp.ToByteArray (ref_Guid));
                            }
                            primaryValues.Add ("'" + ref_Guid + "'");
                        }
                        else
                        {

                            primaryColumns.Add (propInfo.Name + "_Value");
                            string valClassValue = AdaptQuotedStrIntoPg ((property as TextNote).Value.ToString ());

                            primaryValues.Add ("'" + valClassValue + "'");

                            primaryColumns.Add (propInfo.Name + "_Lang");
                            int valClassLang = (int) (property as TextNote).Lang;
                            primaryValues.Add (valClassLang.ToString ());

                            if (isFeature && diffIndexList.Contains (propInfo.Index))
                            {
                                deltaPropValues.Add (propInfo.Index, deltaProp.ToByteArray (property));
                            }
                        }
                        #endregion
                        break;
                    case AimPropertyType.Object:

                        #region Property Type is Object
                        if (prevObj != null)
                            prevPropObj = (AimObject) prevObj.GetValue (propInfo.Index);
                        if (AimMetadata.IsChoice (propInfo.TypeIndex))
                        {
                            ref_ID = InsertChoice ((property as AimObject), correction, transaction);
                            if (ref_ID != -1)
                            {
                                primaryColumns.Add (propInfo.Name);
                                primaryValues.Add (ref_ID.ToString ());
                                if (isFeature && diffIndexList.Contains (propInfo.Index))
                                {
                                    deltaPropValues.Add (propInfo.Index, deltaProp.ToByteArray (ref_ID));
                                }
                            }
                        }
                        else if (AimMetadata.IsAbstract (propInfo.TypeIndex))
                        {
                            ref_ID = InsertObject ((property as AimObject), prevPropObj, correction, transaction);
                            if (ref_ID != -1)
                            {
                                primaryColumns.Add (propInfo.Name);
                                primaryValues.Add (ref_ID.ToString ());

                                primaryColumns.Add ("ref_" + propInfo.Name);
                                primaryValues.Add (AimMetadata.GetAimTypeIndex (property as AimObject).ToString ());

                                if (isFeature && diffIndexList.Contains (propInfo.Index))
                                {
                                    deltaPropValues.Add (propInfo.Index, deltaProp.ToByteArray (property));
                                }
                            }
                        }
                        else
                        {
 
                            ref_ID = InsertObject ((property as AimObject), prevPropObj, correction, transaction);
                            if (ref_ID != -1)
                            {
                                primaryColumns.Add (propInfo.Name);
                                primaryValues.Add (ref_ID.ToString ());
                                if (isFeature && diffIndexList.Contains (propInfo.Index))
                                {
                                    deltaPropValues.Add (propInfo.Index, deltaProp.ToByteArray (ref_ID));
                                }
                            }
                        }
                        #endregion
                        break;
                    case AimPropertyType.List:

                        #region Property Type is List
                        IList list = (IList) property;

                        if (AimMetadata.IsAbstractFeatureRefObject (propInfo.TypeIndex))
                        {
                            #region AbstractFeatureRefObject
                            if (list.Count > 0)
                            {
                                if (sqlInsertFeats_in2_linkTable.Count == 0)
                                    sqlInsertFeats_in2_linkTable.Add (
                                                string.Format ("INSERT INTO \"{0}_link\" " +
                                                    "(\"{0}_id\", prop_index, targetTableIndex, target_guid) VALUES",
                                                        primaryTableName));

                                for (int j = 0; j <= list.Count - 1; j++)
                                {
                                    IAimProperty absFeatRefProp = ((IAimObject) list [j]).GetValue ((int) PropertyAbstractFeatureRefObject.Feature);
                                    IAbstractFeatureRef absFeatRef = (IAbstractFeatureRef) absFeatRefProp;
                                    sqlInsertFeats_in2_linkTable.Add (propInfo.Index.ToString () + ", " + absFeatRef.FeatureTypeIndex.ToString () + ", '" + 
                                            absFeatRef.Identifier.ToString () + "'");
                                }
                                if (isFeature && diffIndexList.Contains (propInfo.Index))
                                {
                                    deltaPropValues.Add (propInfo.Index, deltaProp.ToByteArray (list));
                                }
                            }
                            #endregion
                        }
                        else
                        {
                            if (propInfo.IsFeatureReference)
                            {
                                #region FeatureRef
                                if (list.Count > 0)
                                {
                                    if (sqlInsertFeats_in2_linkTable.Count == 0)
                                        sqlInsertFeats_in2_linkTable.Add (
                                                string.Format ("INSERT INTO \"{0}_link\"" +
                                                "(\"{0}_id\", prop_index, targetTableIndex, target_guid) VALUES", primaryTableName));
                                    for (int j = 0; j <= list.Count - 1; j++)
                                    {
                                        sqlInsertFeats_in2_linkTable.Add (propInfo.Index.ToString () + ", " + propInfo.TypeIndex + ", '" + 
                                        (list [j] as FeatureRefObject).Feature.Identifier.ToString () + "'");
                                    }

                                    if (isFeature && diffIndexList.Contains (propInfo.Index))
                                    {
                                        deltaPropValues.Add (propInfo.Index, deltaProp.ToByteArray (list));
                                    }
                                }
                                #endregion
                            }
                            else if (AimMetadata.IsChoice (propInfo.TypeIndex))
                            {
                                #region Choice
                                insertedIds = InsertChoiceList (list, correction, transaction);
                                if (insertedIds.Count > 0)
                                {
                                    if (sqlInsertObj_in2_linkTable.Count == 0)
                                        sqlInsertObj_in2_linkTable.Add (
                                                string.Format ("INSERT INTO \"{0}_link\"" +
                                                "(\"{0}_id\", prop_index, targetTableIndex, target_id) VALUES", primaryTableName));
                                    for (int j = 0; j <= insertedIds.Count - 1; j++)
                                    {
                                        sqlInsertObj_in2_linkTable.Add (propInfo.Index.ToString () + ", " + propInfo.TypeIndex.ToString () + ", " + 
                                        insertedIds [j].ToString ());
                                    }

                                    if (isFeature && diffIndexList.Contains (propInfo.Index))
                                    {
                                        foreach (int choiceId in insertedIds)
                                        {
                                            deltaPropValues.Add (propInfo.Index, deltaProp.ToByteArray (insertedIds));
                                        }
                                    }
                                }
                                #endregion
                            }
                            else
                            {
                                #region Object
                                insertedIds = InsertObjectList (list, correction, transaction);
                                if (insertedIds.Count > 0)
                                {
                                    if (sqlInsertObj_in2_linkTable.Count == 0)
                                        sqlInsertObj_in2_linkTable.Add (
                                                string.Format ("INSERT INTO \"{0}_link\"" +
                                                "(\"{0}_id\", prop_index, targetTableIndex, target_id) VALUES", primaryTableName));
                                    for (int j = 0; j <= insertedIds.Count - 1; j++)
                                    {
                                        targetTableIndex = AimMetadata.GetAimTypeIndex (list [j] as IAimObject);
                                        sqlInsertObj_in2_linkTable.Add (propInfo.Index.ToString () + ", " + targetTableIndex.ToString () + ", " + 
                                        insertedIds [j].ToString ());
                                    }

                                    if (isFeature && diffIndexList.Contains (propInfo.Index))
                                    {
                                        deltaPropValues.Add (propInfo.Index, deltaProp.ToByteArray (insertedIds));
                                    }
                                }
                                #endregion
                            }
                        }
                        #endregion
                        break;
                }
            }

            if (prevObj != null)
            {
                var allPropInfos = AimMetadata.GetAimPropInfos(aimObj);
                var filledPropIndexList = aimObj.GetPropertyIndexes().ToList();
                var prevFilledPropIndexList = prevObj.GetPropertyIndexes().ToList();

                List<AimPropInfo> clearedPropInfoList = allPropInfos.Where(pi =>
                    !filledPropIndexList.Contains(pi.Index) && 
                    prevFilledPropIndexList.Contains(pi.Index) &&
                    pi.TypeIndex != (int)ObjectType.MdMetadata).ToList();
				clearedPropInfoList.RemoveAll ( item => item.Name == "Id" );

                foreach (AimPropInfo propInfo in clearedPropInfoList)
                {
                    switch (AimMetadata.GetAimObjectType(propInfo.Index))
                    {
                        case AimObjectType.Field:

                            #region AranField
                            primaryColumns.Add(propInfo.Name);
                            primaryValues.Add("NULL");
                            #endregion
                            break;

                        case AimObjectType.DataType:

                            #region DataType
                            if (AimMetadata.IsAbstractFeatureRef(propInfo.TypeIndex))
                            {
                                primaryColumns.Add(propInfo.Name);
                                primaryValues.Add("NULL");

                                primaryColumns.Add("ref_" + propInfo.Name);
                                primaryValues.Add("NULL");
                            }
                            else if (propInfo.AixmName == "timeSlice")
                            {
                                primaryColumns.Add("begin_valid");
                                primaryValues.Add("NULL");

                                primaryColumns.Add("end_valid");
                                primaryValues.Add("NULL");

                                primaryColumns.Add("sequence");
                                primaryValues.Add("NULL");

                                primaryColumns.Add("correction");
                                primaryValues.Add("NULL");
                            }
                            else if (propInfo.PropType.SubClassType == AimSubClassType.ValClass)
                            {
                                primaryColumns.Add(propInfo.Name + "_Value");
                                primaryValues.Add("NULL");

                                primaryColumns.Add(propInfo.Name + "_Uom");
                                primaryValues.Add("NULL");

                                primaryColumns.Add(propInfo.Name + "_SIValue");
                                primaryValues.Add("NULL");
                            }
                            else if (propInfo.IsFeatureReference)
                            {
                                primaryColumns.Add(propInfo.Name);
                                primaryValues.Add("NULL");
                            }
                            else
                            {
                                primaryColumns.Add(propInfo.Name + "_Value");
                                primaryValues.Add("NULL");

                                primaryColumns.Add(propInfo.Name + "_Lang");
                                primaryValues.Add("NULL");
                            }
                            #endregion
                            break;
                        case AimObjectType.Object:

                            #region Property Type is Object

                            if (propInfo.PropType.IsAbstract)
                            {
                                primaryColumns.Add(propInfo.Name);
                                primaryValues.Add("NULL");

                                primaryColumns.Add("ref_" + propInfo.Name);
                                primaryValues.Add("NULL");
                            }
                            else if (propInfo.IsList)
                            {
                                #region Property Type is List
                                //IList list = (IList)property;

                                //if (AimMetadata.IsAbstractFeatureRefObject(propInfo.TypeIndex))
                                //{
                                //    #region AbstractFeatureRefObject
                                //    if (list.Count > 0)
                                //    {
                                //        if (sqlInsertFeats_in2_linkTable.Count == 0)
                                //            sqlInsertFeats_in2_linkTable.Add(
                                //                        string.Format("INSERT INTO \"{0}_link\" " +
                                //                            "(\"{0}_id\", prop_index, targetTableIndex, target_guid) VALUES",
                                //                                primaryTableName));

                                //        for (int j = 0; j <= list.Count - 1; j++)
                                //        {
                                //            IAimProperty absFeatRefProp = ((IAimObject)list[j]).GetValue((int)PropertyAbstractFeatureRefObject.Feature);
                                //            IAbstractFeatureRef absFeatRef = (IAbstractFeatureRef)absFeatRefProp;
                                //            sqlInsertFeats_in2_linkTable.Add(propInfo.Index.ToString() + ", " + absFeatRef.FeatureTypeIndex.ToString() + ", '" +
                                //                    absFeatRef.Identifier.ToString() + "'");
                                //        }
                                //        if (isFeature && diffIndexList.Contains(propInfo.Index))
                                //        {
                                //            deltaPropValues.Add(propInfo.Index, deltaProp.ToByteArray(list));
                                //        }
                                //    }
                                //    #endregion
                                //}
                                //else
                                //{
                                //    if (propInfo.IsFeatureReference)
                                //    {
                                //        #region FeatureRef
                                //        if (list.Count > 0)
                                //        {
                                //            if (sqlInsertFeats_in2_linkTable.Count == 0)
                                //                sqlInsertFeats_in2_linkTable.Add(
                                //                        string.Format("INSERT INTO \"{0}_link\"" +
                                //                        "(\"{0}_id\", prop_index, targetTableIndex, target_guid) VALUES", primaryTableName));
                                //            for (int j = 0; j <= list.Count - 1; j++)
                                //            {
                                //                sqlInsertFeats_in2_linkTable.Add(propInfo.Index.ToString() + ", " + propInfo.TypeIndex + ", '" +
                                //                (list[j] as FeatureRefObject).Feature.Identifier.ToString() + "'");
                                //            }

                                //            if (isFeature && diffIndexList.Contains(propInfo.Index))
                                //            {
                                //                deltaPropValues.Add(propInfo.Index, deltaProp.ToByteArray(list));
                                //            }
                                //        }
                                //        #endregion
                                //    }
                                //    else if (AimMetadata.IsChoice(propInfo.TypeIndex))
                                //    {
                                //        #region Choice
                                //        insertedIds = InsertChoiceList(list, correction, transaction);
                                //        if (insertedIds.Count > 0)
                                //        {
                                //            if (sqlInsertObj_in2_linkTable.Count == 0)
                                //                sqlInsertObj_in2_linkTable.Add(
                                //                        string.Format("INSERT INTO \"{0}_link\"" +
                                //                        "(\"{0}_id\", prop_index, targetTableIndex, target_id) VALUES", primaryTableName));
                                //            for (int j = 0; j <= insertedIds.Count - 1; j++)
                                //            {
                                //                sqlInsertObj_in2_linkTable.Add(propInfo.Index.ToString() + ", " + propInfo.TypeIndex.ToString() + ", " +
                                //                insertedIds[j].ToString());
                                //            }

                                //            if (isFeature && diffIndexList.Contains(propInfo.Index))
                                //            {
                                //                foreach (int choiceId in insertedIds)
                                //                {
                                //                    deltaPropValues.Add(propInfo.Index, deltaProp.ToByteArray(insertedIds));
                                //                }
                                //            }
                                //        }
                                //        #endregion
                                //    }
                                //    else
                                //    {
                                //        #region Object
                                //        insertedIds = InsertObjectList(list, correction, transaction);
                                //        if (insertedIds.Count > 0)
                                //        {
                                //            if (sqlInsertObj_in2_linkTable.Count == 0)
                                //                sqlInsertObj_in2_linkTable.Add(
                                //                        string.Format("INSERT INTO \"{0}_link\"" +
                                //                        "(\"{0}_id\", prop_index, targetTableIndex, target_id) VALUES", primaryTableName));
                                //            for (int j = 0; j <= insertedIds.Count - 1; j++)
                                //            {
                                //                targetTableIndex = AimMetadata.GetAimTypeIndex(list[j] as IAimObject);
                                //                sqlInsertObj_in2_linkTable.Add(propInfo.Index.ToString() + ", " + targetTableIndex.ToString() + ", " +
                                //                insertedIds[j].ToString());
                                //            }

                                //            if (isFeature && diffIndexList.Contains(propInfo.Index))
                                //            {
                                //                deltaPropValues.Add(propInfo.Index, deltaProp.ToByteArray(insertedIds));
                                //            }
                                //        }
                                //        #endregion
                                //    }
                                //}
                                #endregion
                            }
                            else
                            {
                                primaryColumns.Add(propInfo.Name);
                                primaryValues.Add("NULL");
                            }


                            #endregion
                            break;
                    }
                }
            }


            string updatePropsToNullSqlText = "";
            string deletePrevObjProps_ThatNowNull = "";
            if (isUpdate)
            {
                updatePropsToNullSqlText = GetNullPropsSqlText (aimObj, prevObj, filledPropInfoList, out deletePrevObjProps_ThatNowNull);
            }

            if (isFeature)
            {
                #region AimObject is Feature Case WHERE properties should be INSERTed INTO feat_delta AND feat_delta_properties table

                string WHEREClause = string.Format (" \"Identifier\"='{0}'", guid);
                long featId = GetId ("features", WHEREClause, transaction);
                primaryColumns.Add ("feat_id");
                primaryValues.Add (featId.ToString ());

                InsertFeatToDeltaTable (featId, (aimObj as Feature).TimeSlice, transaction, diffIndexList, deltaPropValues, isUpdate);

                string result = string.Empty;
                if ((TimeSliceInterpretationType) interpretation == TimeSliceInterpretationType.PERMDELTA || 
                        (TimeSliceInterpretationType) interpretation == TimeSliceInterpretationType.BASELINE)
                {
                    result = FeatInsertCommandToBl_Table (transaction, primaryTableName, featId,
                                                            (aimObj as Feature).TimeSlice, primaryColumns, primaryValues, isUpdate, updatePropsToNullSqlText, deletePrevObjProps_ThatNowNull);
                }
                return result;
                #endregion
            }
            if (primaryColumns.Count > 0)
            {
                if (primaryValues.Count == 0)
                    return string.Empty;
                if (id == 0 && prevObj != null)
                {
                    IAimProperty idProp = prevObj.GetValue ((int) PropertyDBEntity.Id);
                    id = (long) (idProp as IEditAimField).FieldValue;
                }
                else
                    id = 0;

                return CreateInsertSqlCommand (primaryTableName, primaryColumns, primaryValues, id);
            }
            else
            {
                if (sqlInsertFeats_in2_linkTable.Count > 0 || sqlInsertObj_in2_linkTable.Count > 0)
                {
                    long nextValOfId = NextValOfSequence (primaryTableName, transaction);
                    return string.Format ("INSERT INTO \"{0}\"(\"Id\") VALUES ({1});", primaryTableName, nextValOfId);
                }
                return string.Empty;
            }
        }

        private string GetNullPropsSqlText (IAimObject aimObj, IAimObject prevObj, List<AimPropInfo> filledPropInfos, out string deletePrevObjProps_ThatNowNull)
        {
            AimPropInfo [] propInfos = AimMetadata.GetAimPropInfos (aimObj);
            List<AimPropInfo> nullPropInfos = new List<AimPropInfo> ();
            nullPropInfos.AddRange (propInfos);

            for(var i = 0; i < nullPropInfos.Count; i++)
            {
                if (nullPropInfos[i].TypeIndex == (int)ObjectType.MdMetadata)
                {
                    nullPropInfos.RemoveAt(i);
                    break;
                }
            }

            int index;
            foreach (AimPropInfo propInfo in filledPropInfos)
            {
                index = nullPropInfos.FindIndex (item => item.AixmName == propInfo.AixmName);
                if (index != -1)
                {
                    nullPropInfos.RemoveAt (index);
                }
            }
            IAimProperty property;
            string result = "";
            string tableName = CommonData.GetTableName (aimObj);
            long id = (aimObj as DBEntity).Id;
            deletePrevObjProps_ThatNowNull = "";
            long prevObjId;
            IAimProperty idProp;
            foreach (AimPropInfo propInfo in nullPropInfos)
            {
                if (propInfo.TypeIndex == (int) DataType.TimeSlice)
                    continue;
                if (propInfo.IsList)
                {
                    deletePrevObjProps_ThatNowNull += string.Format ("delete FROM \"{0}_link\" " +
											"WHERE \"{0}_id\" = {1} AND prop_index = {2}; ",
                                            tableName,
                                            id,
                                            propInfo.Index);
                }
                else
                {
					if ( propInfo.PropType.IsAbstract )
						result += "\"" + propInfo.Name + "\" = DEFAULT, ";
					else
					{
						property = ( IAimProperty ) AimObjectFactory.Create ( propInfo.TypeIndex );
						if ( property.PropertyType == AimPropertyType.DataType && property is IEditValClass )
						{
							result += "\"" + propInfo.Name + "_Value\" = DEFAULT, ";
						}
						else
						{

							if ( property.PropertyType == AimPropertyType.Object )
							{
								property = prevObj.GetValue ( propInfo.Index );
								if ( property != null )
								{
									idProp = ( ( IAimObject ) property ).GetValue ( ( int ) PropertyDBEntity.Id );
									if ( idProp != null )
									{
										prevObjId = ( long ) ( idProp as IEditAimField ).FieldValue;
										_sqlTextRunAtLast += string.Format ( "DELETE FROM \"{0}\" WHERE \"Id\"={1};", "obj_" + AimMetadata.GetAimTypeName ( ( IAimObject ) property ), prevObjId );
									}
								}
							}
							result += "\"" + propInfo.Name + "\" = DEFAULT, ";
						}
					}                    
                }
            }
            return result;
        }

        private string AdaptQuotedStrIntoPg (string sourceStr)
        {
            if (sourceStr.Contains ("'"))
            {
                int indexOfQuote = sourceStr.IndexOf ("'");
                while (indexOfQuote > 0)
                {
                    sourceStr = sourceStr.Insert (indexOfQuote, "'");
                    indexOfQuote = sourceStr.IndexOf ("'", indexOfQuote + 2);
                }
            }
            return sourceStr;
        }

        private string InsertIntofeaturesTable (Feature feat, IDbTransaction transaction, bool isUpdate, Guid transGuid)
        {
            int index = (int) feat.FeatureType;
            if (feat.TimeSlice.FeatureLifetime == null)
            {
                return "LifeTime property is empty!";
            }
            else
            {
                bool isCorrection = false;
                string sqlText;

                if (feat.TimeSlice.CorrectionNumber > 0)
                    isCorrection = true;

                if (!isCorrection && !isUpdate)
                {
                    var endPos = feat.TimeSlice.FeatureLifetime.EndPosition;

                    sqlText = string.Format (
                            "INSERT INTO features(" +
							"\"Identifier\", begin_life, end_life, type_index)\r\n" +
							"\r\nVALUES (" + 
							"'{0}', '{1}', {2}, {3})",
                            feat.Identifier,
                            Global.DateTimeToString (feat.TimeSlice.FeatureLifetime.BeginPosition),
                            (endPos.HasValue ?  "'" + Global.DateTimeToString (endPos.Value) + "'" : "NULL"),
                            index);

                    CreateCommand (transaction, sqlText).ExecuteNonQuery ();
                }
                else
                {
                    sqlText = string.Format ("UPDATE features SET begin_life = '{0}'",
                        Global.DateTimeToString (feat.TimeSlice.FeatureLifetime.BeginPosition));

                    if (feat.TimeSlice.FeatureLifetime.EndPosition.HasValue)
                        sqlText += string.Format (", end_life = '{0}' ", Global.DateTimeToString (feat.TimeSlice.FeatureLifetime.EndPosition.Value));

                    sqlText += string.Format (" WHERE \"Identifier\"='{0}'", feat.Identifier);
                    CreateCommand (transaction, sqlText).ExecuteNonQuery ();
                }

                sqlText = string.Format (
                    "INSERT INTO features_transaction_list (feat_identifier, transaction_uid) VALUES ('{0}', '{1}')",
                    feat.Identifier, transGuid);
                CreateCommand (transaction, sqlText).ExecuteNonQuery ();

                return string.Empty;
            }
        }

        private void InsertFeatToDeltaTable (long featId, TimeSlice timeSlice, IDbTransaction transaction,
                                                List<int> diffIndexList, SortedDictionary<int, byte []> deltaPropValues, bool isUpdate)
        {
            int interpretation = (int) timeSlice.Interpretation;
            int sequence = timeSlice.SequenceNumber;
            int correction = timeSlice.CorrectionNumber;
            DateTime begin_valid = timeSlice.ValidTime.BeginPosition;
            DateTime? end_valid = timeSlice.ValidTime.EndPosition;

            // Add Null properties to feat_delta_properties table
            // Which value column in table is just false means it has no value
            //foreach ( var item in diffIndexList )
            //{
            //    if ( ! deltaPropValues.ContainsKey(item ) )
            //    {
            //        deltaPropValues.Add ( item, BitConverter.GetBytes ( false ) );
            //    }
            //}

            string sqlTextDelta, sqlTextVals;
            IDbCommand command;
            long feat_delta_id;
            if (!isUpdate)
            {
                sqlTextDelta = "INSERT INTO feat_delta(feat_id, interpretation, sequence, correction, begin_valid, created_by";
                sqlTextVals = featId + ", " + interpretation + ", " + sequence + ", " + correction + ", '" +
					Global.DateTimeToString (begin_valid) + "', " + User.Id;

                if (end_valid.HasValue)
                {
                    sqlTextDelta += ", end_valid";
                    sqlTextVals += ", '" + Global.DateTimeToString (end_valid.Value) + "'";
                }

                command = CreateCommand (transaction, sqlTextDelta + ") VALUES (" + sqlTextVals + ");");
                command.ExecuteNonQuery ();

                feat_delta_id = CurrValOfSequence ("feat_delta", transaction);
            }
            else
            {
                if (timeSlice.Interpretation == TimeSliceInterpretationType.BASELINE)
                {
                    int permIndex = (int) TimeSliceInterpretationType.PERMDELTA;
                    sqlTextDelta = string.Format ("SELECT \"Id\" FROM feat_delta WHERE feat_id={0} AND (interpretation={1} OR interpretation={2})  AND sequence={3} AND correction={4};", featId, interpretation, permIndex, sequence, correction);
                }
                else
                    sqlTextDelta = string.Format ("SELECT \"Id\" FROM feat_delta WHERE feat_id={0} AND interpretation={1} AND sequence={2} AND correction={3};", featId, interpretation, sequence, correction);
                command = CreateCommand (transaction, sqlTextDelta);
                feat_delta_id = (long) command.ExecuteScalar ();
                if (diffIndexList.Count == 0)
                    return;

                sqlTextDelta = string.Format ("SELECT \"Id\" FROM feat_delta_properties WHERE feat_delta_id = {0} AND key in (", feat_delta_id);
                foreach (var propIndex in diffIndexList)
                {
                    sqlTextDelta += propIndex + ", ";
                }
                sqlTextDelta = sqlTextDelta.Remove (sqlTextDelta.Length - 2) + ")";
                command = CreateCommand (transaction, sqlTextDelta);
                IDataReader dataReader = command.ExecuteReader ();
                sqlTextDelta = "";
                object obj;
                while (dataReader.Read ())
                {
                    obj = dataReader [0];
                    if (!DBNull.Value.Equals (obj))
                        sqlTextDelta += obj.ToString () + ", ";
                }
                if (sqlTextDelta != "")
                {
                    sqlTextDelta = sqlTextDelta.Remove (sqlTextDelta.Length - 2);
                    _sqlTextRunAtLast += "DELETE FROM feat_delta_properties WHERE \"Id\" in (" + sqlTextDelta + ");";
                }
            }

            if (deltaPropValues.Count == 0)
                return;

            sqlTextDelta = "INSERT INTO feat_delta_properties (feat_delta_id, key, value) VALUES ";
            int i = 0;
            foreach (KeyValuePair<int, byte []> deltaValue in deltaPropValues)
            {
                i++;
                sqlTextDelta += "(" + feat_delta_id + ", " + deltaValue.Key + ", :bytea" + i + "),";
                IDataParameter dataParam = command.CreateParameter ();
                dataParam.DbType = DbType.Binary;
                dataParam.ParameterName = "bytea" + i;
                dataParam.Value = deltaValue.Value;
                command.Parameters.Add (dataParam);
            }
            sqlTextDelta = sqlTextDelta.Remove (sqlTextDelta.Length - 1);
            command.CommandText  = sqlTextDelta;
            command.ExecuteNonQuery ();

        }

        private string FeatInsertCommandToBl_Table (IDbTransaction transaction, string primaryTableName,
                                                    long featId, TimeSlice timeSlice,
                                                    List<string> primaryColumns,
                                                    List<string> primaryValues, bool isUpdate, string updatePropsToNullSqlText, string setNullSql4ListProp)
        {
            int sequence = timeSlice.SequenceNumber;
            int correction = timeSlice.CorrectionNumber;
            DateTime begin_valid = timeSlice.ValidTime.BeginPosition;

            IDbCommand command = CreateCommand (transaction);
            if (sequence > 1)
            {
                command.CommandText = string.Format ("UPDATE \"{0}\" SET end_valid='{1}' WHERE " +
                                                                    "feat_id={2} AND sequence={3}",
                                                        primaryTableName,
                                                        Global.DateTimeToString (begin_valid),
                                                        featId,
                                                        sequence - 1);
                command.ExecuteNonQuery ();
            }
            bool isCorrection = (correction > 0);
            if (isCorrection || isUpdate)
            {
                string result = string.Format ("{0} UPDATE \"{1}\" SET", setNullSql4ListProp, primaryTableName);
                for (int i = 0; i <= primaryColumns.Count - 1; i++)
                {
                    result += "\"" + primaryColumns [i] + "\" = " + primaryValues [i] + ", ";
                }
                result += updatePropsToNullSqlText;
                result = result.Remove (result.Length - 2) + " WHERE sequence = " + sequence + " AND feat_id = " + featId;


                //string result = string.Format ( "UPDATE \"{0}\" SET", primaryTableName );
                //for ( int i = 0; i <= primaryColumns.Count - 1; i++ )
                //{
                //    result += "\"" + primaryColumns [i] + "\" = " + primaryValues [i] + ",";
                //}
                //result = result.Remove ( result.Length - 1 );
                //result += " WHERE sequence = " + sequence + " AND feat_id = " + featId;
                return result;
            }
            return CreateInsertSqlCommand (primaryTableName, primaryColumns, primaryValues);
        }

        private string CreateInsertSqlCommand (string tableName, List<string> columns, List<string> values, long id = 0)
        {
            string result = "";
            if (id > 0 && values.Count > 0)
            {
                _sqlTextRunAtLast += "DELETE FROM \"" + tableName + "\" WHERE \"Id\" = " + id + ";";
            }
            result += "INSERT INTO \"" + tableName + "\"(";
            foreach (string strCol in columns)
            {
                result += "\"" + strCol + "\",";
            }
            result = result.Remove (result.Length - 1) + ")\r\n VALUES(";
            foreach (string strVal in values)
            {
                result += strVal + ",";
            }
            result = result.Remove (result.Length - 1) + ")";
            return result;
        }

        private List<long> InsertObjectList (IList aimObjList, int correction, IDbTransaction transaction)
        {
            List<long> result = new List<long> ();
            AimObject aimObj;
            long objId;
            for (int i = 0; i <= aimObjList.Count - 1; i++)
            {
                aimObj = (aimObjList [i] as AimObject);
                objId = InsertObject (aimObj, null, correction, transaction);
                if (objId != -1)
                    result.Add (objId);
            }
            return result;
        }

        private long InsertObject (AimObject aimObj, AimObject prevObj, int correction, IDbTransaction transaction)
        {
            if (aimObj is Metadata.ISO.MdMetadata)
                return -1;

            string insertSqlText;
            List<string> sqlInsertObj_in2_LinkTable = new List<string> ();
            List<string> sqlInsertFeat_in2_LinkTable = new List<string> ();
            long objId;

            if (aimObj is ElevatedPoint || aimObj is AixmPoint)
            {
                CalculateCRC (aimObj as AixmPoint);
            }

            List<AimPropInfo> propInfos = GetFilledPropInfos (aimObj);
            sqlInsertObj_in2_LinkTable.Clear ();
            byte [] geomByteArray;
            insertSqlText = CreateSqlText_4_DBEntity (aimObj, prevObj, null, correction, sqlInsertObj_in2_LinkTable, sqlInsertFeat_in2_LinkTable, transaction, out geomByteArray);
            if (insertSqlText == string.Empty)
                return -1;
            IDbCommand command = CreateCommand (transaction, insertSqlText);
            if (geomByteArray != null)
            {
                AddParameterToDBCommand (command, geomByteArray);
            }
            command.ExecuteNonQuery ();
            string tableName = "obj_" + (aimObj as AObject).ObjectType.ToString ();
            objId = CurrValOfSequence (tableName, transaction);
            InserLinkTable (objId, tableName, sqlInsertObj_in2_LinkTable, sqlInsertFeat_in2_LinkTable, transaction);
            return objId;
        }

        private void CalculateCRC (AixmPoint aixmPoint)
        {
            if (double.IsNaN (aixmPoint.Geo.X) || double.IsNaN (aixmPoint.Geo.Y))
                return;

            if (aixmPoint.Extension == null)
                aixmPoint.Extension = new PointExtension ();

            string crcItems = "Lat/Lon";
            string crcValue = aixmPoint.Geo.X +""+aixmPoint.Geo.Y;
            if (aixmPoint is ElevatedPoint)
            {
                ElevatedPoint elevPoint = aixmPoint as ElevatedPoint;
                if (elevPoint.Elevation != null)
                {
                    if (!double.IsNaN (elevPoint.Elevation.Value))
                    {
                        crcItems += "/Elev Value/Uom";
                        crcValue += elevPoint.Elevation.Value + "" + elevPoint.Elevation.Uom;
                    }
                }
            }
            ICRCExtension crcExtension = aixmPoint.Extension;
            crcExtension.SetCRCItems (crcItems);
            crcExtension.SetCRCValue (Aran.PANDA.Common.CRC32.CalcCRC32 (crcValue));

        }

        private void AddParameterToDBCommand (IDbCommand command, byte [] geomByteArray)
        {
            IDataParameter dataParam = command.CreateParameter ();
            dataParam.DbType = DbType.Binary;
            dataParam.ParameterName = byteaGeomParamName;
            dataParam.Value = geomByteArray;
            command.Parameters.Add (dataParam);
        }

        private List<long> InsertChoiceList (IList aimObjList, int correction, IDbTransaction transaction)
        {
            List<long> result = new List<long> ();
            AimObject aimObj;
            long objId;
            for (int i = 0; i <= aimObjList.Count - 1; i++)
            {
                aimObj = (AimObject) aimObjList [i];
                objId = InsertChoice (aimObj, correction, transaction);
                if (objId != -1)
                    result.Add (objId);
            }
            return result;
        }

        private long InsertChoice (AimObject aimObj, int correction, IDbTransaction transaction)
        {
            string choiceTableName = CommonData.GetTableName (aimObj);
            string sqlText = string.Empty;
            IEditChoiceClass editChoiceObj = (IEditChoiceClass) aimObj;
            AimObjectType objType = AimMetadata.GetAimObjectType (editChoiceObj.RefType);
            long insertedObjId;

            if (AimMetadata.IsAbstractFeatureRef (editChoiceObj.RefType))
            {
                sqlText = "INSERT INTO \"" + choiceTableName + "\"(prop_type, choice_type,target_guid)";
                Guid identifier = (editChoiceObj.RefValue as FeatureRef).Identifier;
                int refFeatTypeIndex = (editChoiceObj.RefValue as IAbstractFeatureRef).FeatureTypeIndex;
                if (identifier != null && refFeatTypeIndex != 0)
                    sqlText += " values(" + editChoiceObj.RefType + ", " + refFeatTypeIndex.ToString () + ", '" + 
                                        identifier.ToString () + "')";
            }
            else if (editChoiceObj.RefValue is FeatureRef)
            {
                sqlText = "INSERT INTO \"" + choiceTableName + "\"(prop_type, choice_type, target_guid)";
                Guid identifier = (editChoiceObj.RefValue as FeatureRef).Identifier;
                if (identifier != null)
                    sqlText += " values(" + editChoiceObj.RefType + ", " + editChoiceObj.RefType + ", '" + 
                            identifier.ToString () + "')";
            }
            else if (AimMetadata.IsAbstract (editChoiceObj.RefType))
            {
                insertedObjId = InsertObject ((editChoiceObj.RefValue as AimObject), null, correction, transaction);
                // if there is nothing to insert into obj table then should insert just Id
                if (insertedObjId == -1)
                {
                    string tableName = CommonData.GetTableName (editChoiceObj.RefValue as AimObject);
                    insertedObjId = NextValOfSequence (tableName, transaction);
                    sqlText = string.Format ("INSERT INTO \"{0}\"(\"Id\") VALUES ({1});", tableName, insertedObjId);
                }
                sqlText += "INSERT INTO \"" + choiceTableName + "\"(prop_type, choice_type, target_id)";
                sqlText += " values(" + editChoiceObj.RefType + ", " + AimMetadata.GetAimTypeIndex (editChoiceObj.RefValue as IAimObject).ToString () + ", " + insertedObjId + ")";
            }
            else
            {
                insertedObjId = InsertObject ((editChoiceObj.RefValue as AimObject), null, correction, transaction);
                // if there is nothing to insert into obj table then should insert just Id
                if (insertedObjId == -1)
                {
                    string tableName = CommonData.GetTableName (editChoiceObj.RefValue as AimObject);
                    insertedObjId = NextValOfSequence (tableName, transaction);
                    sqlText = string.Format ("INSERT INTO \"{0}\"(\"Id\") VALUES ({1});", tableName, insertedObjId);
                }
                sqlText += "INSERT INTO \"" + choiceTableName + "\"(prop_type, choice_type,target_id)";
                sqlText += " values(" + editChoiceObj.RefType + ", " + editChoiceObj.RefType + ", " + insertedObjId + ")";
            }

            int countInsertedRows = CreateCommand (transaction, sqlText).ExecuteNonQuery ();

            if (countInsertedRows > 0)
                return CurrValOfSequence (choiceTableName, transaction);
            return -1;
        }

        /// <summary>
        /// Returns error message if something went wrong 
        /// else returns empty string and sets sequence and correction 		
        /// </summary>
        private string GetCurrVersion (Feature feat, ref int sequence, ref int correction)
        {
            if (feat.Identifier == null)
                return "Identifier property is empty!";

            if (feat.TimeSlice == null)
                return "TimeSlice property is empty!";

            if (!HasFeature (feat.Identifier))
                return string.Empty;

            var pgCommand = _connection.CreateCommand ();
            string tableName;
            
            if (feat.TimeSlice.Interpretation == TimeSliceInterpretationType.PERMDELTA || 
				feat.TimeSlice.Interpretation == TimeSliceInterpretationType.BASELINE)
            {
                tableName = "bl_" + feat.FeatureType;
                pgCommand.CommandText = string.Format (
                    "SELECT \"Id\", sequence, correction FROM \"{0}\" " +
                    "WHERE feat_id = (SELECT \"Id\" FROM features WHERE \"Identifier\" = '{1}') " +
                    "ORDER BY \"Id\" DESC LIMIT 1",
                    tableName, feat.Identifier);
            }
            else
            {
                // Not implemented yet 
                tableName = "feat_delta";
                string WHEREClause = string.Format (" \"Identifier\"='{}'", feat.Identifier);
                long featId = GetId ("feat_delta", WHEREClause);
                pgCommand.CommandText = "SELECT sequence, correction FROM " + tableName + " WHERE feat_id=" + featId;
            }

            var res = "";
            var dataReader = pgCommand.ExecuteReader ();
            
            if (dataReader.Read()) {
                sequence = Convert.ToInt32(dataReader[1]);
                correction = Convert.ToInt32(dataReader[2]);
            }
            else {
                res = "It does not have inserted version!";
            }
            
            dataReader.Close ();

            return res;
        }

        private long GetId (string tableName, string WHEREClause, IDbTransaction transaction  = null)
        {
            string sqlCommand = string.Format ("SELECT \"Id\" FROM \"{0}\" WHERE {1}",
                                                    tableName, WHEREClause);
            IDbCommand dbCommand = CreateCommand (transaction, sqlCommand);
            var o = dbCommand.ExecuteScalar ();
            long id = (long) o;
            return id;
        }

        private long CurrValOfSequence (string tableName, IDbTransaction transaction)
        {
            object val = CreateCommand (transaction, string.Format ("SELECT currval ('\"{0}_Id_seq\"')", tableName)).ExecuteScalar ();
            return Convert.ToInt64 (val);
        }

        private long NextValOfSequence (string tableName, IDbTransaction transaction)
        {
            object val = CreateCommand (transaction, string.Format ("SELECT nextval ('\"{0}_Id_seq\"')", tableName)).ExecuteScalar ();
            return Convert.ToInt64 (val);
        }

        private void InserLinkTable (long primaryId, string tableName,
                                    List<string> sqlTexts_4_ObjInserting,
                                    List<string> sqlTexts_4_FeatInserting,
                                    IDbTransaction transaction)
        {
            string prop_indices = "(";
            int prop_index;
            string sqlText;
            IDbCommand command = CreateCommand (transaction);

            #region Insert Objects INTO Link Table
            if (sqlTexts_4_ObjInserting.Count > 0)
            {

                for (int i = 1; i <= sqlTexts_4_ObjInserting.Count - 1; i++)
                {
                    int.TryParse (sqlTexts_4_ObjInserting [i].Substring (0, sqlTexts_4_ObjInserting [i].IndexOf (',')), out prop_index);
                    prop_indices += prop_index + ",";
                }
                prop_indices = prop_indices.Remove (prop_indices.Length - 1) + ")";

                sqlText = string.Format ("delete FROM \"{0}_link\" " +
                                                "WHERE \"{0}_id\" = {1} AND prop_index in {2}; " +
                                                "{3}",
                                                tableName,
                                                primaryId,
                                                prop_indices,
                                                sqlTexts_4_ObjInserting [0]);
                for (int i = 1; i <= sqlTexts_4_ObjInserting.Count - 1; i++)
                {
                    sqlText += "(" + primaryId.ToString () + ", " + sqlTexts_4_ObjInserting [i] + "),";
                }
                sqlText = sqlText.Remove (sqlText.Length - 1);

                command.CommandText = sqlText;
                command.ExecuteNonQuery ();
            }
            #endregion

            #region Insert Features INTO Link Table
            if (sqlTexts_4_FeatInserting.Count > 0)
            {
                prop_indices = "(";
                for (int i = 1; i <= sqlTexts_4_FeatInserting.Count - 1; i++)
                {
                    int.TryParse (sqlTexts_4_FeatInserting [i].Substring (0, sqlTexts_4_FeatInserting [i].IndexOf (',')), out prop_index);
                    prop_indices += prop_index + ",";
                }
                prop_indices = prop_indices.Remove (prop_indices.Length - 1) + ")";

                sqlText = string.Format ("delete FROM \"{0}_link\" " +
                                                "WHERE \"{0}_id\" = {1} AND prop_index in {2}; " +
                                                "{3}",
                                                tableName,
                                                primaryId,
                                                prop_indices,
                                                sqlTexts_4_FeatInserting [0]);
                for (int i = 1; i <= sqlTexts_4_FeatInserting.Count - 1; i++)
                {
                    sqlText += "(" + primaryId.ToString () + ", " + sqlTexts_4_FeatInserting [i] + "),";
                }
                sqlText = sqlText.Remove (sqlText.Length - 1);

                command.CommandText = sqlText;
                command.ExecuteNonQuery ();
            }
            #endregion
        }

        private bool HasFeature (Guid guid)
        {
            IDbCommand command = _connection.CreateCommand ();
            command.CommandText = string.Format ("SELECT * FROM features WHERE \"Identifier\" = '{0}'", guid);
            IDataReader dataReader = command.ExecuteReader ();
            bool result = dataReader.Read ();
            dataReader.Close ();
            return result;
        }

        private InsertingResult DecommissionFeature(long featId, int seqNum, DateTime endLife, IDbCommand cmd, string tableName)
        {
            var endLifeText = Global.DateTimeToString(endLife);

            cmd.CommandText = string.Format(
                "INSERT INTO \"{0}\" (begin_valid, end_valid, sequence, correction, feat_id) " +
                "VALUES ('{1}', '{2}', {3}, 0, {4})",

                tableName,
                endLifeText,
                endLifeText,
                seqNum,
                featId);

            cmd.ExecuteNonQuery();

            cmd.CommandText = string.Format(
                "UPDATE features SET end_life = '{0}' WHERE \"Id\" = {1}",
                endLifeText, featId);

            cmd.ExecuteNonQuery();

            SetEndOfValid(tableName, featId, Guid.Empty);

            cmd.CommandText = string.Format(
                "UPDATE \"{0}\" SET end_valid = '{1}' WHERE feat_id = {2} AND sequence = {3}",
                tableName, endLifeText, featId, seqNum);

            cmd.ExecuteNonQuery();

            return new InsertingResult();
        }

        private IDbCommand CreateCommand (IDbTransaction transaction, string commandText = null)
        {
            IDbCommand command = _connection.CreateCommand ();
            command.Transaction = transaction;
            if (commandText != null)
                command.CommandText = commandText;
            return command;
        }

        private List<AimPropInfo> GetFilledPropInfos (IAimObject aimObj)
        {
            List<AimPropInfo> result = new List<AimPropInfo> ();
            AimPropInfo [] propInfos = AimMetadata.GetAimPropInfos (aimObj);
            int [] indexes = aimObj.GetPropertyIndexes ();
            for (int i = 0; i <= indexes.Length - 1; i++)
            {
                for (int j = 0; j <= propInfos.Length - 1; j++)
                {
                    if (indexes [i] == propInfos [j].Index)
                        result.Add (propInfos [j]);
                }
            }
            return result;
        }

        private void SetEndOfValid(string tableName, long? featId, Guid identifier)
        {
            var cmd = _connection.CreateCommand();

            if (featId == null) {
                cmd.CommandText = string.Format("SELECT \"Id\" FROM features WHERE \"Identifier\"='{0}'", identifier);
                featId = (long?)cmd.ExecuteScalar();
            }

            cmd.CommandText = string.Format(
                "UPDATE \"{0}\" " +
                    "SET end_valid = (SELECT MIN(t.begin_valid) FROM \"{0}\" t WHERE t.feat_id = {1} AND t.begin_valid > \"{0}\".begin_valid) " +
                "WHERE feat_id = {1}",

                tableName, featId.Value);

            cmd.ExecuteNonQuery();
        }


        private IDbConnection _connection;
        private string byteaGeomParamName;
        private Dictionary<int, IDbCommand> _dbCommandDict;
        private Dictionary<int, Guid> _transGuidDict;
        private int _transactionIndex;
        private string _sqlTextRunAtLast;
    }
}