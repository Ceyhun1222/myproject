using Aran.Aim.Features;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aran.Aim.BusinessRules
{
    public class FeatureSearcher : IDisposable
    {
        private SQLiteConnection _conn;
        private int _providerKey;

        public FeatureSearcher()
        {
            _conn = new SQLiteConnection();
        }

        public void Open(IFeatureProvider featPro)
        {
            _conn.ConnectionString = "Data Source=:memory:";
            _conn.Open();

            SqliteProviderStack.RegisterFunctions();

            try
            {
                _providerKey = SqliteProviderStack.AddProvider(featPro);

                InsertFeatures();
            }
            catch (Exception parseEx)
            {
                throw parseEx;
            }
        }

        public void Close()
        {
            _conn.Close();
            if (_providerKey > 0)
                SqliteProviderStack.RemoveProvider(_providerKey);
        }

        public void Dispose()
        {
            _conn.Dispose();
            if (_providerKey > 0)
                SqliteProviderStack.RemoveProvider(_providerKey);
        }

        public List<Tuple<FeatureType, Guid>> Check(BRule rule, out int checkCount)
        {
            checkCount = 0;
            var cmdValList = BRuleSqlConverter.ToSqlCommand(rule);

            var result = new List<Tuple<FeatureType, Guid>>();

            using (var cmd = _conn.CreateCommand())
            {
                foreach (var cmdVal in cmdValList)
                {
                    var valueStackKeyList = new List<int>();

                    cmd.CommandText = cmdVal.Command
                        .Replace("$AimEqualTo$(", "AimEqualTo(:providerKey, :featType, identifier_p1, identifier_p2, ")
                        .Replace("$AimEqualToProp$(", "AimEqualToProp(:providerKey, :featType, identifier_p1, identifier_p2, ")
                        .Replace("$AimIsAssigned$(", "AimIsAssigned(:providerKey, :featType, identifier_p1, identifier_p2, ")
                        .Replace("$AimRefCount$(", "AimRefCount(:providerKey, :featType, identifier_p1, identifier_p2, ")
                        .Replace("$AimPropCount$(", "AimPropCount(:providerKey, :featType, identifier_p1, identifier_p2, ")
                        .Replace("$AimHigherLowerEqualProp$(", "AimHigherLowerEqualProp(:providerKey, :featType, identifier_p1, identifier_p2, ")
                        .Replace("$GetConcatPropValue$(", "GetConcatPropValue(:providerKey, :featType, identifier_p1, identifier_p2, ")
                        .Replace("$AimHigherLowerEqual$(", "AimHigherLowerEqual(:providerKey, :featType, identifier_p1, identifier_p2, ");

                    cmd.Parameters.AddWithValue("providerKey", _providerKey);
                    cmd.Parameters.AddWithValue("featType", cmdVal.FeatureType);

                    for (var i = 0; i < cmdVal.CommandValues.Count; i++)
                    {
                        object paramVal = cmdVal.CommandValues[i];
                        if (paramVal == null)
                            continue;

                        if (paramVal.GetType().IsArray)
                        {
                            var valueStackKey = SqliteQueryValueStack.AddValue(paramVal);
                            paramVal = valueStackKey;
                            valueStackKeyList.Add(valueStackKey);
                        }

                        cmd.Parameters.AddWithValue("v" + i, paramVal);
                    }

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var idenP1 = reader.GetInt64(0);
                            var idenP2 = reader.GetInt64(1);
                            var identifier = GuidConverter.ToGuid(idenP1, idenP2);

                            result.Add(new Tuple<FeatureType, Guid>(cmdVal.FeatureType, identifier));
                        }
                        reader.Close();
                    }

                    SqliteQueryValueStack.RemoveValues(valueStackKeyList);

                    cmd.Parameters.Clear();

                    #region Get Feature Count

                    cmd.CommandText = "SELECT count(*) FROM features WHERE feat_type = :featType ";
                    cmd.Parameters.AddWithValue("featType", cmdVal.FeatureType);

                    if (int.TryParse(cmd.ExecuteScalar().ToString(), out int featCount))
                        checkCount += featCount;

                    #endregion
                }
            }

            return result;
        }

        public List<Tuple<FeatureType, Guid>> Check(List<CommandInfo> cmdInfoList, out int checkCount)
        {
            checkCount = 0;

            try
            {
                var result = new List<Tuple<FeatureType, Guid>>();

                using (var cmd = _conn.CreateCommand())
                {
                    foreach (var cmdInfo in cmdInfoList)
                    {
                        var valueStackKeyList = new List<int>();

                        cmd.CommandText = cmdInfo.Command
                            .Replace("$AimEqualTo$(", "AimEqualTo(:providerKey, feat_type, identifier_p1, identifier_p2, ")
                            .Replace("$AimEqualToProp$(", "AimEqualToProp(:providerKey, feat_type, identifier_p1, identifier_p2, ")
                            .Replace("$AimIsAssigned$(", "AimIsAssigned(:providerKey, feat_type, identifier_p1, identifier_p2, ")
                            .Replace("$AimRefCount$(", "AimRefCount(:providerKey, feat_type, identifier_p1, identifier_p2, ")
                            .Replace("$AimPropCount$(", "AimPropCount(:providerKey, feat_type, identifier_p1, identifier_p2, ")
                            .Replace("$AimHigherLowerEqualProp$(", "AimHigherLowerEqualProp(:providerKey, feat_type, identifier_p1, identifier_p2, ")
                            .Replace("$GetConcatPropValue$(", "GetConcatPropValue(:providerKey, feat_type, identifier_p1, identifier_p2, ")
                            .Replace("$AimHigherLowerEqual$(", "AimHigherLowerEqual(:providerKey, feat_type, identifier_p1, identifier_p2, ");

                        cmd.Parameters.AddWithValue("providerKey", _providerKey);
                        cmd.Parameters.AddWithValue("featType", cmdInfo.FeatureType);

                        for (var i = 0; i < cmdInfo.CommandValues.Count; i++)
                        {
                            object paramVal = cmdInfo.CommandValues[i];
                            if (paramVal == null)
                                continue;

                            if (paramVal.GetType().IsArray)
                            {
                                var valueStackKey = SqliteQueryValueStack.AddValue(paramVal);
                                paramVal = valueStackKey;
                                valueStackKeyList.Add(valueStackKey);
                            }
                            cmd.Parameters.AddWithValue("v" + i, paramVal);
                        }

                        try
                        {
                            using (var reader = cmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    var idenP1 = reader.GetInt64(0);
                                    var idenP2 = reader.GetInt64(1);
                                    var identifier = GuidConverter.ToGuid(idenP1, idenP2);

                                    result.Add(new Tuple<FeatureType, Guid>(cmdInfo.FeatureType, identifier));
                                }
                                reader.Close();
                            }

                            cmd.Parameters.Clear();

                            #region Get Feature Count

                            cmd.CommandText = "SELECT count(*) FROM features WHERE feat_type = :featType ";
                            cmd.Parameters.AddWithValue("featType", cmdInfo.FeatureType);

                            if (int.TryParse(cmd.ExecuteScalar().ToString(), out int featCount))
                                checkCount += featCount;

                            #endregion
                        }
                        catch (Exception sqlEx)
                        {
                            Debug.WriteLine("Error on sql execute, details: " + sqlEx.Message);
                        }

                        SqliteQueryValueStack.RemoveValues(valueStackKeyList);
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error: " + ex.Message);
                return null;
            }
        }


        public List<Guid> Test(FeatureType featType, string command, object[] commandVals)
        {
            var resultList = new List<Guid>();

            using (var cmd = _conn.CreateCommand())
            {
                var valueStackKeyList = new List<int>();

                cmd.CommandText =
                    "SELECT identifier_p1, identifier_p2 " +
                    "FROM features " +
                    "WHERE " +
                    "   feat_type = :featType ";

                var s = command
                    .Replace("$AimEqualTo$(", "AimEqualTo(:providerKey, :featType, identifier_p1, identifier_p2, ")
                    .Replace("$AimEqualToProp$(", "AimEqualToProp(:providerKey, :featType, identifier_p1, identifier_p2, ")
                    .Replace("$AimIsAssigned$(", "AimIsAssigned(:providerKey, :featType, identifier_p1, identifier_p2, ")
                    .Replace("$AimRefCount$(", "AimRefCount(:providerKey, :featType, identifier_p1, identifier_p2, ")
                    .Replace("$AimPropCount$(", "AimPropCount(:providerKey, :featType, identifier_p1, identifier_p2, ")
                    .Replace("$AimHigherLowerEqualProp$(", "AimHigherLowerEqualProp(:providerKey, :featType, identifier_p1, identifier_p2, ")
                    .Replace("$AimHigherLowerEqual$(", "AimHigherLowerEqual(:providerKey, :featType, identifier_p1, identifier_p2, ");

                if (s.Length > 0)
                    cmd.CommandText += " AND (" + s + " )";

                cmd.Parameters.AddWithValue("providerKey", _providerKey);
                cmd.Parameters.AddWithValue("featType", (int)featType);

                if (commandVals != null)
                {
                    for (var i = 0; i < commandVals.Length; i++)
                    {
                        object paramVal = commandVals[i];
                        if (paramVal == null)
                            continue;

                        if (paramVal.GetType().IsArray)
                        {
                            var valueStackKey = SqliteQueryValueStack.AddValue(paramVal);
                            paramVal = valueStackKey;
                            valueStackKeyList.Add(valueStackKey);
                        }

                        cmd.Parameters.AddWithValue("v" + i, paramVal);
                    }
                }

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var idenP1 = reader.GetInt64(0);
                        var idenP2 = reader.GetInt64(1);
                        var identifier = GuidConverter.ToGuid(idenP1, idenP2);

                        resultList.Add(identifier);
                    }
                    reader.Close();
                }

                SqliteQueryValueStack.RemoveValues(valueStackKeyList);
            }

            return resultList;
        }


        private void InsertFeatures()
        {
            var dbPro = SqliteProviderStack.GetProvider(_providerKey);
            if (dbPro == null)
                return;

            using (var cmd = _conn.CreateCommand())
            {
                try
                {
                    cmd.Transaction = _conn.BeginTransaction();

                    cmd.CommandText = "CREATE TABLE features (feat_type INT, identifier_p1 INTEGER, identifier_p2 INTEGER)";
                    cmd.ExecuteNonQuery();

                    cmd.CommandText = "INSERT INTO features (feat_type, identifier_p1, identifier_p2) VALUES (:featType, :idenP1, :idenP2)";
                    cmd.Parameters.AddWithValue("featType", null);
                    cmd.Parameters.AddWithValue("idenP1", null);
                    cmd.Parameters.AddWithValue("idenP2", null);


                    foreach (var featInfo in dbPro.GetAllIdentifiers())
                    {
                        GuidConverter.ToInt64s(featInfo.Item2, out Int64 idenP1, out Int64 idenP2);

                        cmd.Parameters[0].Value = (int)featInfo.Item1;
                        cmd.Parameters[1].Value = idenP1;
                        cmd.Parameters[2].Value = idenP2;

                        cmd.ExecuteNonQuery();
                    }

                    cmd.Transaction.Commit();
                }
                catch (Exception)
                {
                    cmd.Transaction.Rollback();
                    throw;
                }
            }
        }
    }
}
