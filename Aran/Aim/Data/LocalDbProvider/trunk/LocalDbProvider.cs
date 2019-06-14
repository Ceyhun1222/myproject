using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aran.Aim;
using Aran.Aim.Data;
using Aran.Aim.Data.Filters;
using Aran.Aim.DataTypes;
using Aran.Aim.Enums;
using Aran.Aim.Features;

namespace Aran.Aim.Data.Local
{
    public class LocalDbProvider : DbProvider
    {
        private SQLiteConnection _conn;
        private Dictionary<int, SQLiteTransaction> _transactionDict;
        private int _newTransactionId;

        public LocalDbProvider()
        {
            _conn = new SQLiteConnection();
            _transactionDict = new Dictionary<int, SQLiteTransaction>();
            _newTransactionId = 1;

            CurrentUser = new User();
            CurrentUser.Name = string.Empty;
            CurrentUser.FeatureTypes.AddRange(Enum.GetValues(typeof(FeatureType)).Cast<int>());
        }

        public override DbProviderType GetProviderType(ref string otherName)
        {
            otherName = "LocalDb";
            return DbProviderType.Other;
        }

        public override void Open(string connectionString)
        {
            var fileName = connectionString;
            _conn.ConnectionString = "Data Source=" + fileName;

            try
            {
                _conn.Open();

                CreateFeaturesTableIfNotExists();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                State = _conn.State;
            }
        }

        public override bool Login(string userName, string md5Password)
        {
            return true;
        }

        public override void Close()
        {
            _conn.Close();
            _transactionDict.Clear();
            _newTransactionId = 1;
            State = _conn.State;
        }

        public override int BeginTransaction()
        {
            var trans = _conn.BeginTransaction();
            var trId = _newTransactionId;
            _transactionDict[_newTransactionId] = trans;
            _newTransactionId++;
            return trId;
        }

        public override InsertingResult Commit(int transactionId)
        {
            SQLiteTransaction trans;
            if (!_transactionDict.TryGetValue(transactionId, out trans))
                return new InsertingResult(string.Format("Transaction N: {0} has not been created!", transactionId));

            trans.Commit();
            _transactionDict.Remove(transactionId);
            return new InsertingResult();
        }

        public override InsertingResult Rollback(int transactionId)
        {
            SQLiteTransaction trans;
            if (!_transactionDict.TryGetValue(transactionId, out trans))
                return new InsertingResult(string.Format("Transaction N: {0} has not been created!", transactionId));

            trans.Rollback();
            _transactionDict.Remove(transactionId);
            return new InsertingResult();
        }

        public override InsertingResult Insert(Feature feature, bool insertAnyway, bool asCorrection)
        {
            return Insert(feature, 0, insertAnyway, asCorrection);
        }

        public override InsertingResult Insert(Feature feature, int transactionId, bool insertAnyway, bool asCorrection)
        {
            var cmd = _conn.CreateCommand();

            if (transactionId > 0)
            {
                SQLiteTransaction trans;
                if (!_transactionDict.TryGetValue(transactionId, out trans))
                    return new InsertingResult(string.Format("Transaction N: {0} has not been begined!", transactionId));

                cmd.Transaction = trans;
            }

            string tableName;
            CreateTableIfNotExists(feature.FeatureType, out tableName, cmd);

            var data = Packer.ToBytes(feature);

            cmd.Parameters.AddWithValue(":identifier", GuidToString(feature.Identifier));
            cmd.Parameters.AddWithValue(":data", data);

            cmd.CommandText = string.Format("UPDATE {0} SET data=:data WHERE identifier=:identifier", tableName);

            var chCount = cmd.ExecuteNonQuery();

            if (chCount == 0)
            {
                cmd.CommandText = string.Format("INSERT INTO {0} (identifier, data) VALUES (:identifier, :data)", tableName);
                cmd.ExecuteNonQuery();

                cmd.CommandText = "INSERT INTO features (identifier, feature_type) VALUES (:identifier, :featureType)";
                cmd.Parameters.AddWithValue(":featureType", (int)feature.FeatureType);
                cmd.ExecuteNonQuery();
            }

            return new InsertingResult();
        }

        public override GettingResult GetVersionsOf(
            FeatureType featType, TimeSliceInterpretationType interpretation, 
            TimePeriod submissionTime, Guid identifier = default(Guid), 
            bool loadComplexProps = false, TimeSliceFilter timeSlicefilter = null, 
            List<string> propList = null, Filter filter = null)
        {
            if (interpretation != TimeSliceInterpretationType.BASELINE)
                return new GettingResult("CacheDbProvider does not support other than BASELINE");

            if (_conn.State != System.Data.ConnectionState.Open)
                _conn.Open();

            var cmd = _conn.CreateCommand();
            string tableName;

            if (!IsTableExists(featType, cmd, out tableName))
                return new GettingResult(new List<Feature>());

            var identifierWhereText = string.Empty;
            if (identifier != Guid.Empty)
                identifierWhereText = " WHERE identifier = '" + GuidToString(identifier) + "'";

            cmd.CommandText = string.Format("SELECT data FROM {0} {1}", tableName, identifierWhereText);
            var dr = cmd.ExecuteReader();

            var list = new List<Feature>();

            while (dr.Read())
            {
                try
                {
                    var data = (byte[])dr[0];

                    if (filter != null)
                    {
                        var isFiltered = Packer.IsConditionSatisfied(data, filter.Operation);
                        if (!isFiltered)
                            continue;
                    }

                    var feature = Packer.FromBytes(data) as Feature;
                    list.Add(feature);
                }
                catch (Exception ex)
                {
                    dr.Close();
                    return new GettingResult(ex.Message);
                }
                
            }
            dr.Close();

            return new GettingResult(list);
        }

        private bool IsTableExists(string tableName, SQLiteCommand cmd)
        {
            if (cmd == null)
                cmd = _conn.CreateCommand();

            cmd.CommandText = string.Format("SELECT EXISTS (SELECT 1 FROM sqlite_master WHERE type='table' AND name='{0}');", tableName);
            var isExistsObj = cmd.ExecuteScalar();
            return Convert.ToBoolean(isExistsObj);
        }

        private bool IsTableExists(FeatureType featType, SQLiteCommand cmd, out string tableName)
        {
            tableName = "bl_" + featType;
            return IsTableExists(tableName, cmd);
        }

        private void CreateTableIfNotExists(FeatureType featType, out string tableName, SQLiteCommand cmd = null)
        {
            if (cmd == null)
                cmd = _conn.CreateCommand();

            if (IsTableExists(featType, cmd, out tableName))
                return;

            cmd.CommandText = string.Format(
                "CREATE TABLE {0} (" +
                    "id INTEGER PRIMARY KEY AUTOINCREMENT," +
                    "identifier VARCHAR(36) NOT NULL," +
                    "data BLOB NOT NULL)",
                tableName);

            cmd.ExecuteNonQuery();
        }

        private void CreateFeaturesTableIfNotExists()
        {
            var cmd = _conn.CreateCommand();

            if (IsTableExists("features", cmd))
                return;

            cmd.CommandText =
                "CREATE TABLE features (" +
                    "id INTEGER PRIMARY KEY AUTOINCREMENT," +
                    "identifier VARCHAR(36) NOT NULL," +
                    "feature_type INTEGER NOT NULL)";

            cmd.ExecuteNonQuery();
        }

        public override void SetParameter(string key, object value)
        {
            if (key == "central-meridian")
                Packer.CentralMeridian = (double?)value;
        }

        public override object GetParameter(string key)
        {
            if (key == "central-meridian")
                return Packer.CentralMeridian;

            return base.GetParameter(key);
        }

        private static string GuidToString(Guid guid)
        {
            return guid.ToString("D");
        }

        public override GettingResult GetAllStoredFeatTypes()
        {
            var featList = new List<FeatureType>();

            var cmd = _conn.CreateCommand();
            cmd.CommandText = "SELECT name FROM sqlite_master WHERE type='table' AND name  LIKE 'bl_%'";
            var dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                var tableName = dr[0].ToString();
                var featTypeName = tableName.Substring(3);

                FeatureType featType;
                if (Enum.TryParse<FeatureType>(featTypeName, out featType))
                    featList.Add(featType);
            }
            dr.Close();

            return new GettingResult(featList);
        }

        

        public InsertingResult ReInsert(FeatureType featType, IEnumerable<Feature> features, int transactionId = -1)
        {
            var cmd = _conn.CreateCommand();

            if (transactionId > 0)
            {
                SQLiteTransaction trans;
                if (!_transactionDict.TryGetValue(transactionId, out trans))
                    return new InsertingResult(string.Format("Transaction N: {0} has not been begined!", transactionId));

                cmd.Transaction = trans;
            }

            string tableName;
            CreateTableIfNotExists(featType, out tableName, cmd);

            var isTransCreated = (cmd.Transaction == null);
            if (isTransCreated)
                cmd.Transaction = _conn.BeginTransaction();

            try
            {
                cmd.CommandText = string.Format("DELETE FROM {0}", tableName);
                cmd.ExecuteNonQuery();

                cmd.Parameters.Add(":identifier", System.Data.DbType.String);
                cmd.Parameters.Add(":data", System.Data.DbType.Binary);

                foreach (var feature in features)
                {
                    cmd.CommandText = string.Format("INSERT INTO {0} (identifier, data) VALUES (:identifier, :data)", tableName);

                    var data = Packer.ToBytes(feature);

                    cmd.Parameters[0].Value = GuidToString(feature.Identifier);
                    cmd.Parameters[1].Value = data;

                    cmd.ExecuteNonQuery();
                }

                if (isTransCreated)
                    cmd.Transaction.Commit();

                return new InsertingResult();
            }
            catch (Exception ex)
            {
                if (isTransCreated)
                    cmd.Transaction.Rollback();

                return new InsertingResult(ex.Message);
            }
        }

        public void RemoveTable(FeatureType featType)
        {
            var cmd = _conn.CreateCommand();
            cmd.CommandText = string.Format("DROP TABLE bl_{0}", featType.ToString());
            cmd.ExecuteNonQuery();
        }

        public override bool IsExists(Guid guid)
        {
            var cmd = _conn.CreateCommand();
            cmd.CommandText = "SELECT EXISTS (SELECT 1 FROM features WHERE identifier = :identifier)";
            cmd.Parameters.AddWithValue(":identifier", GuidToString(guid));

            return (Convert.ToInt32(cmd.ExecuteScalar()) == 1);
        }

        public override InsertingResult Update(Feature feature)
        {
            return Insert(feature);
        }

        public override InsertingResult Update(Feature feature, int transactionId)
        {
            return Insert(feature, transactionId);
        }
    }
}
