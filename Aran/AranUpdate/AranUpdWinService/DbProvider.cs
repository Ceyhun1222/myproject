//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.IO;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Npgsql;
//using NpgsqlTypes;

//namespace AranUpdWinService
//{
//    public class DbProvider
//    {
//        private NpgsqlConnection _conn;
//        private string _userName;

//        public DbProvider()
//        {
//            _conn = new NpgsqlConnection();
//        }

//        public void Open(string server, int port)
//        {
//            _conn.ConnectionString = string.Format("Server={0}; Port={1}; Database=aran-upd; User Id=aranupd; Password=aranupdAdmin",
//                server, port);
//            _conn.Open();
//        }

//        public void Reopen()
//        {
//            _conn.Close();
//            _conn.Open();
//        }

//        public void Register(string userName)
//        {
//            var cmd = _conn.CreateCommand();
//            cmd.CommandText = "SELECT register(:userName);";
//            cmd.Parameters.AddWithValue(":userName", userName);
//            cmd.ExecuteNonQuery();

//            _userName = userName;
//        }

//        public void Close()
//        {
//            _conn.Close();
//        }

//        public bool IsOpen
//        {
//            get { return _conn.State == ConnectionState.Open; }
//        }

//        public AranVersionInfo GetNewVersion(out double? updateIntervalSec, out string currVersionName)
//        {
//            AranVersionInfo vInfo = null;
//            updateIntervalSec = null;
//            currVersionName = string.Empty;

//            if (string.IsNullOrEmpty(_userName))
//                return null;

//            var cmd = _conn.CreateCommand();
//            cmd.CommandText = "SELECT a_new_version, a_loid, a_update_interval_sec, a_curr_version FROM get_new_version(:userName)";
//            cmd.Parameters.AddWithValue("userName", _userName);

//            var dr = cmd.ExecuteReader();
//            long loid = -1;
//            var newVersion = -1;

//            if (dr.Read())
//            {
//                if (!dr.IsDBNull(0))
//                    newVersion = Convert.ToInt32(dr[0]);

//                if (!dr.IsDBNull(1))
//                    loid = Convert.ToInt64(dr[1]);

//                if (!dr.IsDBNull(2))
//                    updateIntervalSec = Convert.ToDouble(dr[2]);

//                if (!dr.IsDBNull(3))
//                    currVersionName = dr[3].ToString();
//            }
//            dr.Close();

//            if (loid == -1)
//                return null;

//            vInfo = new AranVersionInfo();
//            vInfo.VersionId = newVersion;

//            vInfo.Data = ReadFile((int)loid);

//            SetLastVersion(vInfo.VersionId, LastVersionType.Downloaded);

//            return vInfo;
//        }

//        public void SendMessage(string message)
//        {
//            var cmd = _conn.CreateCommand();
//            cmd.CommandText = "SELECT add_log(:userName, :dateTime, :messageText)";
//            cmd.Parameters.AddWithValue("userName", _userName);
//            cmd.Parameters.AddWithValue("dateTime", DateTime.Now);
//            cmd.Parameters.AddWithValue("messageText", message);

//            cmd.ExecuteNonQuery();
//        }

//        public void SetLastVersion(int versionId, LastVersionType lvt)
//        {
//            var cmd = _conn.CreateCommand();
//            cmd.CommandText = 
//                "SELECT " +
//                (lvt == LastVersionType.Downloaded ? "set_downloaded_version" : "set_updated_version") +
//                "(:userName, :versionId)";
//            cmd.Parameters.AddWithValue(":userName", _userName);
//            cmd.Parameters.AddWithValue(":versionId", versionId);
//            cmd.ExecuteNonQuery();
//        }


//        private byte[] ReadFile(int loId)
//        {
//            var trans = _conn.BeginTransaction();
//            var dataList = new List<byte>();

//            try
//            {
//                var lom = new LargeObjectManager(_conn);
//                var lo = lom.Open(loId);

//                byte[] block = new byte[1024];
//                int readCount = 0;
//                while ((readCount = lo.Read(block, 0, block.Length)) > 0)
//                {
//                    for (int i = 0; i < readCount; i++)
//                        dataList.Add(block[i]);
//                }

//                lo.Close();

//                trans.Commit();
//            }
//            catch (Exception)
//            {
//                trans.Rollback();
//                throw;
//            }

//            return dataList.ToArray();
//        }
//    }

//    public enum LastVersionType { Downloaded, Updated }
//}
