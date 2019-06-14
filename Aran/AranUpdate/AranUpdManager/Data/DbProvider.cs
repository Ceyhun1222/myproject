using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AranUpdManager;
using Npgsql;
using NpgsqlTypes;

namespace AranUpdateManager.Data
{
    class DbProvider
    {
        NpgsqlConnection _conn;

        public DbProvider()
        {
            _conn = new NpgsqlConnection();
        }

        public void Open(string server, int port)
        {
            _conn.ConnectionString = string.Format(
                "Server={0}; Port={1}; Database=aran-upd; User Id=aranupd; Password=aranupdAdmin", 
                server, port);
            _conn.Open();
        }

        public void Close()
        {
            _conn.Close();
        }

        public List<AranVersion> GetVersions(List<AranVersion> list = null)
        {
            if (list == null)
                list = new List<AranVersion>();

            var cmd = _conn.CreateCommand();
            cmd.CommandText = "SELECT id, key, released_date, is_last_version, changes_rtf FROM version";

            var dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                var item = new AranVersion();

                item.Id = dr.GetInt32(0);
                item.Key = dr.GetString(1);
                item.ReleasedDate = dr.GetDateTime(2);
                item.IsDefault = dr.GetBoolean(3);
                item.ChangesRtf = Encoding.UTF8.GetString((byte[])dr[4]);

                list.Add(item);
            }
            dr.Close();

            return list;
        }

        #region User Group

        public List<UserGroup> GetUserGroups(List<UserGroup> list = null)
        {
            if (list == null)
                list = new List<UserGroup>();

            var cmd = _conn.CreateCommand();
            cmd.CommandText = 
                "SELECT " + 
                "ug.id, ug.name, ug.description, ug.note, ug.current_version_id, v.key AS current_version_name " +
                "FROM user_group ug " +
                "LEFT JOIN version v ON v.id = ug.current_version_id " +
                "ORDER BY ug.name";

            var dr = cmd.ExecuteReader();

            while(dr.Read())
            {
                var item = new UserGroup();

                item.Id = dr.GetInt32(0);
                item.Name = dr[1].ToString();
                item.SetDescription(dr[2].ToString());
                item.Note = dr[3].ToString();
                
                if (!dr.IsDBNull(4))
                {
                    item.Version.Id = dr.GetInt64(4);
                    item.Version.Text = dr[5].ToString();
                }

                list.Add(item);
            }
            dr.Close();

            return list;
        }

        public void SetUserGroup(UserGroup userGroup)
        {
            var cmd = _conn.CreateCommand();
            cmd.Transaction = _conn.BeginTransaction();

            MakeCommandText("user_group", userGroup.Id, cmd,
                "name", userGroup.Name,
                "description", userGroup.Description,
                "note", userGroup.Note,
                "current_version_id", userGroup.Version.IsNew ? null : (object)userGroup.Version.Id);

            try
            {
                cmd.ExecuteNonQuery();

                if (userGroup.IsNew)
                {
                    cmd.Parameters.Clear();
                    cmd.CommandText = "SELECT currval ('user_group_id_seq')";
                    userGroup.Id = Convert.ToInt64(cmd.ExecuteScalar());
                }

                cmd.Transaction.Commit();
            }
            catch (Exception ex)
            {
                cmd.Transaction.Rollback();
                throw ex;
            }
        }

        public void DeleteUserGroup(long id)
        {
            var cmd = _conn.CreateCommand();
            cmd.CommandText = "SELECT delete_user_group(" + id + ")";
            var isOk = (bool)cmd.ExecuteScalar();

            if (!isOk)
                throw new AranException("User Group is not empty. Please, first clear all users in this group");
        }

        #endregion

        #region User

        public List<User> GetUsers(long parentId, List<User> list = null)
        {
            if (list == null)
                list = new List<User>();

            var cmd = _conn.CreateCommand();
            cmd.CommandText =
                "SELECT " +
                "u.id, u.user_name, u.full_name, u.note, " +
                "vd.key AS ldv, vu.key AS luv, u.group_id, ug.name AS group_name, " +
                "EXISTS(SELECT 1 FROM user_log ul WHERE ul.user_id = u.id AND NOT ul.is_read) " +
                "FROM users u " +
                "LEFT JOIN version vd ON u.last_downloaded_version = vd.id " +
                "LEFT JOIN version vu ON u.last_updated_version = vu.id " +
                "LEFT JOIN user_group ug ON ug.id = u.group_id " +
                "WHERE u.group_id " + (parentId == -1 ? " IS NULL" : "=" + parentId);

            var dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                var item = new User();

                item.Id = dr.GetInt32(0);
                item.UserName = dr.GetString(1);
                item.FullName = dr.GetString(2);
                item.Note = dr[3].ToString();
                item.LastDownloadedVersion = dr[4].ToString();
                item.LastUpdatedVersion = dr[5].ToString();
                
                if (!dr.IsDBNull(6))
                {
                    item.Group.Id = dr.GetInt32(6);
                    item.Group.Text = dr[7].ToString();
                }

                item.HasLog = dr.GetBoolean(8);

                list.Add(item);
            }
            dr.Close();

            return list;
        }

        public void SetUser(User user)
        {
            var cmd = _conn.CreateCommand();
            cmd.Transaction = _conn.BeginTransaction();

            MakeCommandText("users", user.Id, cmd,
                "user_name", user.UserName,
                "full_name", user.FullName,
                "group_id", user.Group.Id,
                "note", user.Note);

            try
            {
                cmd.ExecuteNonQuery();

                if (user.IsNew)
                {
                    cmd.Parameters.Clear();
                    cmd.CommandText = "SELECT currval ('users_id_seq')";
                    user.Id = Convert.ToInt64(cmd.ExecuteScalar());
                }

                cmd.Transaction.Commit();
            }
            catch (Exception ex)
            {
                cmd.Transaction.Rollback();
                throw ex;
            }
        }

        public void DeleteUser(long id)
        {
            var cmd = _conn.CreateCommand();
            cmd.CommandText = "SELECT delete_user(" + id + ")";
            var isOk = (bool)cmd.ExecuteScalar();

            if (!isOk)
                throw new AranException("User has reference.");
        }

        #endregion

        public void WriteNewVersion(string key, DateTime releasedDate, byte[] data, byte[] changesRtfData)
        {
            var cmd = _conn.CreateCommand();
            cmd.CommandText = "SELECT EXISTS (SELECT 1 FROM version WHERE key = :key)";
            cmd.Parameters.AddWithValue(":key", key);

            var isKeyExists = (bool)cmd.ExecuteScalar();

            if (isKeyExists)
                throw new Exception("Version name already exists!");

            cmd.Parameters.Clear();

            var trans = _conn.BeginTransaction();

            try
            {
                var lom = new LargeObjectManager(_conn);
                var loId = lom.Create(LargeObjectManager.WRITE);
                var lo = lom.Open(loId, LargeObjectManager.WRITE);

                lo.Write(data);
                lo.Close();

                cmd.Transaction = trans;

                cmd.CommandText = "UPDATE version SET is_last_version = false";
                cmd.ExecuteNonQuery();

                cmd.CommandText = "INSERT INTO version (key, released_date, lo_id, changes_rtf) VALUES (:key, :released_date, :lo_id, :changes_rtf)";
                cmd.Parameters.AddWithValue(":key", key);
                cmd.Parameters.AddWithValue(":released_date", releasedDate);
                cmd.Parameters.AddWithValue(":lo_id", loId);
                cmd.Parameters.AddWithValue(":changes_rtf", changesRtfData);

                cmd.ExecuteNonQuery();

                trans.Commit();
                return;
            }
            catch (Exception)
            {
                trans.Rollback();
                throw;
            }
        }

        public void MoveUser(long userId, long groupId)
        {
            var cmd = _conn.CreateCommand();
            cmd.CommandText = "UPDATE users SET group_id = " + groupId + " WHERE id = " + userId;
            cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="read">{'unread', 'read', 'all' }</param>
        /// <returns></returns>
        public List<Tuple<long, DateTime>> GetUserLogDates(long userId, string read, List<Tuple<long, DateTime>> list = null)
        {
            var readFilter = string.Empty;
            if (read == "unread")
                readFilter = " AND is_read = false";
            else if (read == "read")
                readFilter = " AND is_read = true";

            var cmd = _conn.CreateCommand();
            cmd.CommandText = "SELECT id, date_time FROM user_log WHERE user_id=" + userId + readFilter;
            var dr = cmd.ExecuteReader();

            if (list == null)
                list = new List<Tuple<long, DateTime>>();

            while (dr.Read())
                list.Add(new Tuple<long, DateTime>(dr.GetInt64(0), dr.GetDateTime(1)));

            dr.Close();

            return list;
        }

        public string GetUserLogText(long userLogId)
        {
            var cmd = _conn.CreateCommand();
            cmd.CommandText = "SELECT message_text FROM user_log WHERE id = " + userLogId;
            var dr = cmd.ExecuteReader();

            var message = string.Empty;

            if (dr.Read())
                message = dr[0].ToString();

            dr.Close();

            cmd.CommandText = "UPDATE user_log SET is_read = true WHERE id = " + userLogId;
            cmd.ExecuteNonQuery();

            return message;
        }

        public List<RefItem> GetUserGroupsNotInVersion(long versionId)
        {
            var cmd = _conn.CreateCommand();
            cmd.CommandText = "SELECT id, name FROM user_group WHERE current_version_id <> " + versionId;
            var dr = cmd.ExecuteReader();
            var list = new List<RefItem>();

            while(dr.Read())
            {
                var item = new RefItem();
                item.Id = Convert.ToInt32(dr[0]);
                item.Text = dr[1].ToString();
                list.Add(item);
            }
            dr.Close();

            return list;
        }

        public List<VersionUserGroupDoc> GetVersionUserGroupDocs(long versionId)
        {
            var cmd = _conn.CreateCommand();
            cmd.CommandText =
                "SELECT " +
                "vug.id, vug.date_time, vug.note, " +
                "vug.version_id, v.key as version_name, " +
                "vug.user_group_id, ug.name as user_group_name " +
                "FROM version_user_group vug " +
                "LEFT JOIN version v ON v.id = vug.version_id " +
                "LEFT JOIN user_group ug ON ug.id = vug.user_group_id " +
                (versionId > 0 ? "WHERE vug.version_id = " + versionId : "");

            var dr = cmd.ExecuteReader();

            var list = new List<VersionUserGroupDoc>();

            while (dr.Read())
            {
                var item = new VersionUserGroupDoc();

                item.Id = dr.GetInt64(0);
                item.DateTime = dr.GetDateTime(1);
                item.Note = dr[2].ToString();
                item.Version.Id = dr.GetInt64(3);
                item.Version.Text = dr[4].ToString();
                item.UserGroup.Id = dr.GetInt64(5);
                item.UserGroup.Text = dr[6].ToString();

                list.Add(item);
            }

            return list;
        }

        public void SetVersionUserGroup(long versionId, long userGropId, string note)
        {
            var cmd = _conn.CreateCommand();
            cmd.Transaction = _conn.BeginTransaction();
            cmd.CommandText =
                "INSERT INTO version_user_group " +
                "(version_id, user_group_id, note) VALUES " +
                "(:version, :user_group, :note)";
            cmd.Parameters.AddWithValue("version", versionId);
            cmd.Parameters.AddWithValue("user_group", userGropId);
            cmd.Parameters.AddWithValue("note", note);

            try
            {
                cmd.ExecuteNonQuery();

                cmd.CommandText = "UPDATE user_group SET current_version_id = :version WHERE id = :user_group";
                cmd.Parameters.RemoveAt(2);

                cmd.ExecuteNonQuery();

                cmd.Transaction.Commit();
            }
            catch (Exception ex)
            {
                cmd.Transaction.Rollback();
                throw ex;
            }

        }



        private void MakeCommandText(string tableName, long id, NpgsqlCommand command, params object[] cmdParams)
        {
            command.Parameters.Clear();

            for (int i = 0; i < cmdParams.Length; i += 2)
            {
                var prm = new NpgsqlParameter();
                prm.ParameterName = cmdParams[i].ToString();
                prm.Value = cmdParams[i + 1];
                command.Parameters.Add(prm);
            }

            command.CommandText = MakeCommandText(tableName, id, command.Parameters);
        }

        private string MakeCommandText(string tableName, long id, NpgsqlParameterCollection paramColl)
        {
            string cmdText = string.Empty;

            if (id < 1)
            {
                cmdText = "INSERT INTO " + tableName + " (" + paramColl[0].ParameterName;
                for (int i = 1; i < paramColl.Count; i++)
                    cmdText += "," + paramColl[i].ParameterName;
                cmdText += ") VALUES (:" + paramColl[0].ParameterName;
                for (int i = 1; i < paramColl.Count; i++)
                    cmdText += ",:" + paramColl[i].ParameterName;
                cmdText += ")";
            }
            else
            {
                cmdText = "UPDATE " + tableName + " SET " + paramColl[0].ParameterName + "=:" + paramColl[0].ParameterName;
                for (int i = 1; i < paramColl.Count; i++)
                    cmdText += "," + paramColl[i].ParameterName + "=:" + paramColl[i].ParameterName;
                cmdText += " WHERE id=" + id;
            }

            return cmdText;
        }

    }
}
