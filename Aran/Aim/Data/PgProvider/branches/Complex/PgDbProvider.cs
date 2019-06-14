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
    public class PgDbProvider : DbProvider
    {
        #region Fields

        private Reader _reader;
        private Writer _writer;
        private IDbConnection _connection;
        private EffectiveDateChangedEventHandler _effectiveDateChangeHandler;

        #endregion


        public PgDbProvider()
        {
            _connection = CommonData.CreateConnection();
            UserManagement = new UserManagement(_connection);
            _reader = new Reader(_connection);
            _writer = new Writer(_connection);
        }

        public override DbProviderType GetProviderType(ref string otherName)
        {
            return DbProviderType.Aran;
        }

        public override void Open(string connectionString)
        {
            _connection.ConnectionString = connectionString;
            _connection.Open();

            CheckAndCreateMetadata();
        }

        public override bool Login(string userName, string md5Password)
        {
            var result = false;
            var cmd = _connection.CreateCommand();
            cmd.CommandText = "SELECT \"Id\", privilege FROM aim_user WHERE name=:name AND password=:password ";

            var param = cmd.CreateParameter();
            param.ParameterName = "name";
            param.Value = userName;
            cmd.Parameters.Add(param);

            param = cmd.CreateParameter();
            param.ParameterName = "password";
            param.Value = md5Password;
            cmd.Parameters.Add(param);

            var dr = cmd.ExecuteReader();

            User user = null;

            if (dr.Read())
            {
                result = true;

                user = new User()
                {
                    Id = dr.GetInt64(0),
                    Name = userName,
                    Password = md5Password,
                    Privilege = (Privilige)dr.GetInt32(1)
                };
            }
            dr.Close();

            if (user != null)
            {
                if (user.Id == 1)
                {
                    foreach (var ft in typeof(FeatureType).GetEnumValues())
                        user.FeatureTypes.Add((int)ft);
                }
                else {
                    cmd.Parameters.Clear();
                    cmd.CommandText = "SELECT feat_type FROM aim_user_feat_types WHERE user_id=" + user.Id;
                    dr = cmd.ExecuteReader();
                    while (dr.Read())
                        user.FeatureTypes.Add(dr.GetInt32(0));
                    dr.Close();
                }
            }

            _writer.User = user;

            return result;
        }

        public override void Close()
        {
            _connection.Close();
        }

        public override int BeginTransaction()
        {
            if (CurrentUser == null)
                throw new Exception("Please, Login...");

            return _writer.BeginTransaction();
        }

        public override InsertingResult Insert(Feature feature, bool insertAnyway, bool asCorrection)
        {
            if (CurrentUser == null)
                return new InsertingResult("Please, Login...");

            if (!CurrentUser.ContainsFeatType(feature.FeatureType))
            {
                return new InsertingResult(false, CurrentUser.Name + " has no privilege to insert " + feature.FeatureType);
            }
            int newTransactionId = BeginTransaction();
            var insertResult = Insert(feature, newTransactionId, insertAnyway, asCorrection);

            if (!insertResult.IsSucceed)
            {
                Rollback(newTransactionId);
                return insertResult;
            }

            return Commit(newTransactionId);
        }

        public override InsertingResult Insert(Feature feature, int transactionId, bool insertAnyway, bool asCorrection)
        {
            if (CurrentUser == null)
                return new InsertingResult("Please, Login...");

            if (!CurrentUser.ContainsFeatType(feature.FeatureType))
                return new InsertingResult(false, CurrentUser.Name + " has no privilege to insert " + feature.FeatureType);

            return _writer.Insert(feature, transactionId, insertAnyway, asCorrection);
        }

        public override InsertingResult Update(Feature feature)
        {
            if (CurrentUser == null)
                return new InsertingResult("Please, Login...");

            if (!CurrentUser.ContainsFeatType(feature.FeatureType))
                return new InsertingResult(false, CurrentUser.Name + " has no privilege to update " + feature.FeatureType);

            var transactionId = _writer.BeginTransaction();
            var result = _writer.Update(feature, transactionId);

            if (!result.IsSucceed)
            {
                _writer.Rollback(transactionId);
                return result;
            }

            return _writer.Commit(transactionId);
        }

        public override InsertingResult Update(Feature feature, int transactionId)
        {
            if (CurrentUser == null)
                return new InsertingResult("Please, Login...");

            if (!CurrentUser.ContainsFeatType(feature.FeatureType))
                return new InsertingResult(false, CurrentUser.Name + " has no privilege to update " + feature.FeatureType);

            return _writer.Update(feature, transactionId);
        }

        public override InsertingResult Delete(Feature feature)
        {
            if (CurrentUser == null)
                return new InsertingResult("Please, Login...");

            if (!CurrentUser.ContainsFeatType(feature.FeatureType))
                return new InsertingResult(false, CurrentUser.Name + " has no privilege to edit " + feature.FeatureType);

            return _writer.Delete(feature);
        }

        public override InsertingResult Commit(int transactionId)
        {
            return _writer.Commit(transactionId);
        }

        public override InsertingResult Rollback(int transactionId)
        {
            return _writer.Rollback(transactionId);
        }

        public override GettingResult GetVersionsOf(FeatureType featType,
            TimeSliceInterpretationType interpretation,
            DataTypes.TimePeriod submissionTime,
            Guid identifier = default(Guid),
            bool loadComplexProps = false,
            TimeSliceFilter timeSlicefilter = null,
            List<string> propList = null,
            Filters.Filter filter = null)
        {
            if (CurrentUser == null)
                return new GettingResult("Please, Login...");

            if (!CurrentUser.ContainsFeatType(featType))
                return new GettingResult(false, CurrentUser.Name + " has no privilege to read " + featType.ToString());

            return _reader.VersionsOf(featType, interpretation, identifier, false, timeSlicefilter, propList, filter);
        }

        public override GettingResult GetAllStoredFeatTypes()
        {
            if (CurrentUser == null)
                return new GettingResult("Please, Login...");

            return _reader.GetAllStoredFeatTypes();
        }

        public override GettingResult GelAllStoredIdentifiers()
        {
            if (CurrentUser == null)
                return new GettingResult("Please, Login...");

            return _reader.GelAllStoredIdentifiers();
        }

        public override GettingResult DeleteFeatIdentifiers(List<Guid> identifierList)
        {
            if (CurrentUser == null)
                return new GettingResult("Please, Login...");

            return _writer.DeleteFeatIdentifiers(identifierList);
        }

        public override bool IsExists(Guid guid)
        {
            return _reader.IsExists(guid);
        }

        public override ConnectionState State
        {
            get { return _connection.State; }
            protected set { }
        }

        public override User CurrentUser
        {
            get { return _writer.User; }
            protected set { }
        }

        public override string GetConnectionInfo
        {
            get
            {
                var host = "";
                var port = 0;
                var db = "";

                //if (CommonData.GetConnectionInfo(_connection, out host, out port, out db)) 
                //    return string.Format("{0}:{1};{2};{3};{4:yyyy-MM-hh}", host, port, db, _writer.User.Name, DefaultEffectiveDate);

                if (CommonData.GetConnectionInfo(_connection, out host, out port, out db))
                    return string.Format("{0}:{1};{2}", host, port, db);

                return string.Empty;
            }
        }

        public override DateTime DefaultEffectiveDate
        {
            get
            {
                return base.DefaultEffectiveDate;
            }
            set
            {
                base.DefaultEffectiveDate = value;
                _reader.TimeSliceFilter = new TimeSliceFilter(value);

                if (_effectiveDateChangeHandler != null)
                    _effectiveDateChangeHandler(this, new EffectiveDateChangedEventArgs());
            }
        }

        public override bool SetEffectiveDateChangedEventHandler(EffectiveDateChangedEventHandler handler)
        {
            _effectiveDateChangeHandler = handler;
            return true;
        }

        public override List<string> GetAllDBList(string host, int port)
        {
            try
            {

                var predefineDbName = "postgres";

                var conn = new Npgsql.NpgsqlConnection(string.Format(
                    "Server={0};Port={1};User Id={2}; Password={3}; Database={4}",
                    host, port, "aran", "airnav2012", predefineDbName));

                conn.Open();

                var cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT datname FROM pg_database";

                var dr = cmd.ExecuteReader();

                var list = new List<string>();

                while (dr.Read())
                {
                    var s = dr[0].ToString();
                    if (!string.IsNullOrEmpty(s) && s != predefineDbName)
                        list.Add(s);
                }
                dr.Close();

                conn.Close();

                list.Sort();

                return list;

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return null;
            }
        }

        public override List<FeatureReport> GetFeatureReport(Guid identifier)
        {
            var cmd = _connection.CreateCommand();
            cmd.CommandText = "SELECT EXISTS (SELECT 1 FROM information_schema.tables WHERE table_schema='public' AND table_name = 'feature_report')";
            var isTableExists = (bool)cmd.ExecuteScalar();

            if (!isTableExists)
                return new List<FeatureReport>();

            cmd.CommandText = "SELECT report_type, report_html, datetime FROM feature_report WHERE identifier = '" + identifier + "'";
            var dr = cmd.ExecuteReader();

            var list = new List<FeatureReport>();

            while (dr.Read())
            {
                if (!dr.IsDBNull(0) && !dr.IsDBNull(1))
                {
                    list.Add(new FeatureReport
                    {
                        Identifier = identifier,
                        ReportType = (FeatureReportType)dr.GetInt32(0),
                        HtmlZipped = (byte[])dr.GetValue(1),
                        DateTime = dr.GetDateTime(2)
                    });
                }
            }
            dr.Close();
            dr.Dispose();

            return list;
        }

        public override void SetFeatureReport(FeatureReport report)
        {
            var cmd = _connection.CreateCommand();
            cmd.CommandText = "SELECT EXISTS (SELECT 1 FROM information_schema.tables WHERE table_schema='public' AND table_name = 'feature_report')";
            var isTableExists = (bool)cmd.ExecuteScalar();

            if (!isTableExists)
            {
                cmd.CommandText =
                    "CREATE TABLE feature_report (" +
                        "id bigserial PRIMARY KEY," +
                        "identifier uuid NOT NULL," +
                        "report_type int," +
                        "report_html bytea," +
                        "datetime timestamp without time zone)";
                cmd.ExecuteNonQuery();
            }

            cmd.CommandText = "INSERT INTO feature_report (identifier, report_type, report_html, datetime) VALUES (:identifier, :report_type, :report_html, :date_time)";

            var param = cmd.CreateParameter();
            param.ParameterName = "identifier";
            param.Value = report.Identifier;
            cmd.Parameters.Add(param);

            param = cmd.CreateParameter();
            param.ParameterName = "report_type";
            param.Value = (int)report.ReportType;
            cmd.Parameters.Add(param);

            param = cmd.CreateParameter();
            param.ParameterName = "report_html";
            param.Value = report.HtmlZipped;
            cmd.Parameters.Add(param);

            report.DateTime = DateTime.Now;
            param = cmd.CreateParameter();
            param.ParameterName = "date_time";
            param.Value = report.DateTime;
            cmd.Parameters.Add(param);

            cmd.ExecuteNonQuery();
        }

		public override void SetFeatureScreenshot ( Screenshot screenshot )
		{
			var cmd = _connection.CreateCommand ( );
			cmd.CommandText = "SELECT EXISTS (SELECT 1 FROM information_schema.tables WHERE table_schema='public' AND table_name = 'feature_screenshot')";
			var isTableExists = ( bool ) cmd.ExecuteScalar ( );

			if ( !isTableExists )
			{
				cmd.CommandText =
					"CREATE TABLE feature_screenshot (" +
						"id bigserial PRIMARY KEY," +
						"identifier uuid NOT NULL," +
						"payload bytea," +
						"datetime timestamp without time zone)";
				cmd.ExecuteNonQuery ( );
			}

			cmd.CommandText = "INSERT INTO feature_screenshot (identifier, payload, datetime) VALUES (:identifier, :payload, :date_time)";

			var param = cmd.CreateParameter ( );
			param.ParameterName = "identifier";
			param.Value = screenshot.Identifier;
			cmd.Parameters.Add ( param );

			param = cmd.CreateParameter ( );
			param.ParameterName = "payload";
			param.Value = screenshot.Images;
			cmd.Parameters.Add ( param );

			screenshot.DateTime = DateTime.Now;
			param = cmd.CreateParameter ( );
			param.ParameterName = "date_time";
			param.Value = screenshot.DateTime;
			cmd.Parameters.Add ( param );

			cmd.ExecuteNonQuery ( );
		}

		public override List<Screenshot> GetFeatureScreenshot ( Guid identifier )
		{
			var cmd = _connection.CreateCommand ( );
			cmd.CommandText = "SELECT EXISTS (SELECT 1 FROM information_schema.tables WHERE table_schema='public' AND table_name = 'feature_screenshot')";
			var isTableExists = ( bool ) cmd.ExecuteScalar ( );

			if ( !isTableExists )
				return new List<Screenshot> ( );

			cmd.CommandText = "SELECT  payload, datetime FROM feature_screenshot WHERE identifier = '" + identifier + "'";
			var dr = cmd.ExecuteReader ( );

			var list = new List<Screenshot> ( );

			while ( dr.Read ( ) )
			{
				if ( !dr.IsDBNull ( 0 ) && !dr.IsDBNull ( 1 ) )
				{
					list.Add ( new Screenshot
					{
						Identifier = identifier,
						Images = ( byte[] ) dr.GetValue ( 0 ),
						DateTime = dr.GetDateTime ( 1 )
					} );
				}
			}
			dr.Close ( );

			return list;
		}

		private void CheckAndCreateMetadata()
        {
            var cmd = _connection.CreateCommand();
            cmd.CommandText = "SELECT EXISTS (SELECT * FROM information_schema.tables WHERE table_name = 'bl_feat_metadata')";
            var isMetadataExists = (bool)cmd.ExecuteScalar();

            if (!isMetadataExists)
            {
                cmd.CommandText =
                    "CREATE TABLE bl_feat_metadata (" +
                    "id bigserial NOT NULL PRIMARY KEY," +
                    "bl_feat_id bigint NOT NULL," +
                    "feat_index int NOT NULL," +
                    "md_data bytea)";
                cmd.ExecuteNonQuery();
            }
        }
    }
}
