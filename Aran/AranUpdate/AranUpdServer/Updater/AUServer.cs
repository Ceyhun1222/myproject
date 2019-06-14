using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using AranUpdManager;
using AranUpdServer;

namespace AranUpdater
{
    public class AUServer : IAUServer
    {
        #region Updater Server

        public AResponse<int> Register(string userName)
        {
            var cmd = Global.CreateCommand("SELECT id FROM users WHERE user_name = :userName");

            try
            {
                cmd.Parameters.AddWithValue("userName", userName);
                var idObj = cmd.ExecuteScalar();

                if (idObj == null)
                {
                    cmd.CommandText = "INSERT INTO users (user_name, full_name) VALUES (:userName, '');";
                    cmd.ExecuteNonQuery();

                    cmd.Parameters.Clear();
                    cmd.CommandText = "SELECT last_insert_rowid()";
                    idObj = cmd.ExecuteScalar();
                }

                return new AResponse<int>(Convert.ToInt32(idObj));
            }
            catch (Exception ex)
            {
                return AResponse<int>.CreateError(Global.MakeServerError(ex));
            }
            finally
            {
                cmd.Connection.Close();
            }
        }

        public AResponse<AranVersionInfo> GetNewVersion(int userId)
        {
            var vInfo = new AranVersionInfo();
            vInfo.UpdateIntervalSec = 30.0;

            var cmd = Global.CreateCommand();

            try
            {
                cmd.CommandText = "SELECT " +
                    "ug.current_version_id, u.last_updated_version " +
                    "FROM  users u, user_group ug, version v " +
                    "WHERE u.group_id = ug.id AND v.id = ug.current_version_id AND u.id = :userId";
                cmd.Parameters.AddWithValue("userId", userId);

                var dr = cmd.ExecuteReader();

                int lastUpdatedVersionId = -1;

                if (dr.Read())
                {
                    vInfo.VersionId = Convert.ToInt32(dr[0]);
                    if (!dr.IsDBNull(1))
                        lastUpdatedVersionId = Convert.ToInt32(dr[1]);
                }
                dr.Close();

                cmd.Parameters.Clear();
                cmd.CommandText = "SELECT key FROM version WHERE id=" + lastUpdatedVersionId;

                vInfo.CurrVersionName = (string)cmd.ExecuteScalar();

                cmd.CommandText = "SELECT value FROM common_settings WHERE key = 'update-interval-sec'";
                var tmpObj = cmd.ExecuteScalar();
                if (tmpObj != null)
                    vInfo.UpdateIntervalSec = Convert.ToDouble(tmpObj);

                #region Load File Data

                if (vInfo.VersionId != null && vInfo.VersionId.Value != lastUpdatedVersionId)
                {
                    var versionBinFilePath = Global.GetVersionBinFilePath(vInfo.VersionId.Value);
                    if (File.Exists(versionBinFilePath))
                        vInfo.Data = File.ReadAllBytes(versionBinFilePath);
                }

                #endregion

                return new AResponse<AranVersionInfo>(vInfo);
            }
            catch (Exception ex)
            {
                return AResponse<AranVersionInfo>.CreateError(Global.MakeServerError(ex));
            }
            finally
            {
                cmd.Connection.Close();
            }
        }

        public AResponse AddLog(UserRequest<string> req)
        {
            var userId = req.UserId;
            var message = req.Value;
            var now = DateTime.Now;

            var cmd = Global.CreateCommand(
                "UPDATE user_log SET date_time=:dateTime, is_read = 0 " +
                "WHERE user_id = :userId AND date(date_time) = date(:dateTime) AND message_text = :message");

            try
            {
                cmd.Parameters.AddWithValue("dateTime", now);
                cmd.Parameters.AddWithValue("userId", userId);
                cmd.Parameters.AddWithValue("message", message);

                var affectedRowCount = cmd.ExecuteNonQuery();

                if (affectedRowCount == 0)
                {
                    cmd.CommandText = "INSERT INTO user_log (user_id, date_time, message_text) VALUES (:userId, :dateTime, :message)";
                    cmd.ExecuteNonQuery();
                }

                return new AResponse();
            }
            catch (Exception ex)
            {
                return new AResponse(Global.MakeServerError(ex));
            }
            finally
            {
                cmd.Connection.Close();
            }
        }

        public AResponse SetLastVersion(UserRequest<int, LastVersionType> req)
        {
            var versionId = req.Value1;
            var versionType = req.Value2;

            var colName = versionType == LastVersionType.Downloaded ? "last_downloaded_version" : "last_updated_version";

            var cmd = Global.CreateCommand("UPDATE users SET " + colName + "=:versionId WHERE id=:userId");
            cmd.Parameters.AddWithValue("versionId", versionId);
            cmd.Parameters.AddWithValue("userId", req.UserId);
            cmd.ExecuteNonQuery();
            
            return new AResponse();
        }

        #endregion

        #region Manager

        public AResponse<List<AranVersion>> GetVersions()
        {
            var cmd = Global.CreateCommand("SELECT id, key, released_date, is_last_version, changes_rtf FROM version");
            var dr = cmd.ExecuteReader();

            var list = new List<AranVersion>();

            while (dr.Read())
            {
                var item = new AranVersion();

                item.Id = Convert.ToInt32(dr[0]);
                item.Key = dr.GetString(1);
                item.ReleasedDate = dr.GetDateTime(2);
                item.IsDefault = Convert.ToBoolean(dr[3]);
                item.ChangesRtf = dr[4].ToString();

                list.Add(item);
            }
            dr.Close();
            cmd.Connection.Close();

            return new AResponse<List<AranVersion>>(list);
        }

        public AResponse WriteNewVersion(NewVersion newVersion)
        {
            var cmd = Global.CreateCommand();
            cmd.CommandText = "SELECT EXISTS (SELECT 1 FROM version WHERE key = :key)";
            cmd.Parameters.AddWithValue(":key", newVersion.Key);

            var isKeyExists = (Convert.ToInt32(cmd.ExecuteScalar()) == 1);

            if (isKeyExists)
            {
                var r = new AResponse("Version name already exists!");
                cmd.Connection.Close();
                return r;
            }

            try
            {
                cmd.Transaction = cmd.Connection.BeginTransaction();

                cmd.Parameters.Clear();
                cmd.CommandText = "UPDATE version SET is_last_version = 0";
                cmd.ExecuteNonQuery();

                cmd.CommandText = "INSERT INTO version (key, released_date, changes_rtf) VALUES (:key, :released_date, :changes_rtf)";
                cmd.Parameters.AddWithValue(":key", newVersion.Key);
                cmd.Parameters.AddWithValue(":released_date", newVersion.ReleasedDate);
                cmd.Parameters.AddWithValue(":changes_rtf", newVersion.ChangesRtf);
                cmd.ExecuteNonQuery();

                cmd.Parameters.Clear();
                cmd.CommandText = "SELECT last_insert_rowid()";
                var versionId = Convert.ToInt32(cmd.ExecuteScalar());

                var versionBinFilePath = Global.GetVersionBinFilePath(versionId);
                if (File.Exists(versionBinFilePath))
                    File.Delete(versionBinFilePath);

                var dir = Path.GetDirectoryName(versionBinFilePath);
                if (!Directory.Exists(dir))
                    Directory.CreateDirectory(dir);

                File.WriteAllBytes(versionBinFilePath, newVersion.Data);

                cmd.Transaction.Commit();
                cmd.Connection.Close();
            }
            catch (Exception ex)
            {
                cmd.Transaction.Rollback();
                cmd.Connection.Close();
                return new AResponse(Global.MakeServerError(ex));
            }

            return new AResponse();
        }

        #region User and User Group

        public AResponse<List<UserGroup>> GetUserGroups()
        {
            var list = new List<UserGroup>();

            var cmd = Global.CreateCommand();
            cmd.CommandText =
                "SELECT " +
                "ug.id, ug.name, ug.description, ug.note, ug.current_version_id, v.key AS current_version_name " +
                "FROM user_group ug " +
                "LEFT JOIN version v ON v.id = ug.current_version_id " +
                "ORDER BY ug.name";

            var dr = cmd.ExecuteReader();

            while (dr.Read())
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
            cmd.Connection.Close();

            return new AResponse<List<UserGroup>>(list);
        }

        public AResponse SetUserGroup(UserGroup userGroup)
        {
            var cmd = Global.CreateCommand();
            cmd.Transaction = cmd.Connection.BeginTransaction();

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
                    cmd.CommandText = "SELECT last_insert_rowid()";
                    userGroup.Id = Convert.ToInt64(cmd.ExecuteScalar());
                }

                cmd.Transaction.Commit();
            }
            catch (Exception ex)
            {
                cmd.Transaction.Rollback();
                return new AResponse(Global.MakeServerError(ex));
            }
            finally
            {
                cmd.Connection.Close();
            }

            return new AResponse();
        }

        public AResponse MoveUser(ARequest<long, long> req)
        {
            long userId = req.Value1;
            long groupId = req.Value2;

            var cmd = Global.CreateCommand("UPDATE users SET group_id = " + groupId + " WHERE id = " + userId);
            cmd.ExecuteNonQuery();
            cmd.Connection.Close();
            return new AResponse();
        }

        public AResponse DeleteUserGroup(long id)
        {
            var cmd = Global.CreateCommand("DELETE FROM user_group WHERE id=" + id);
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch
            {
                return new AResponse("User Group is not empty. Please, first clear all users in this group");
            }

            return new AResponse();
        }

        public AResponse<List<User>> GetUsers(long parentId)
        {
            var list = new List<User>();

            var cmd = Global.CreateCommand();
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

            return new AResponse<List<User>>(list);
        }

        public AResponse SetUser(User user)
        {
            var cmd = Global.CreateCommand();
            cmd.Transaction = cmd.Connection.BeginTransaction();

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
                    cmd.CommandText = "SELECT last_insert_rowid()";
                    user.Id = Convert.ToInt64(cmd.ExecuteScalar());
                }

                cmd.Transaction.Commit();
            }
            catch (Exception ex)
            {
                cmd.Transaction.Rollback();
                return new AResponse(Global.MakeServerError(ex));
            }
            finally
            {
                cmd.Connection.Close();
            }

            return new AResponse();
        }

        public AResponse DeleteUser(long id)
        {
            var cmd = Global.CreateCommand();
            cmd.CommandText = "DELETE FROM users WHERE id=" + id;

            try
            {
                cmd.ExecuteNonQuery();
            }
            catch
            {
                return new AResponse("User has reference.");
            }

            return new AResponse();
        }

        #endregion

        public AResponse<List<Tuple<long, DateTime>>> GetUserLogDates(ARequest<long, string> req)
        {
            try
            {
                var userId = req.Value1;
                var read = req.Value2;

                var readFilter = string.Empty;
                if (read == "unread")
                    readFilter = " AND is_read = 0";
                else if (read == "read")
                    readFilter = " AND is_read = 1";

                var cmd = Global.CreateCommand("SELECT id, date_time FROM user_log WHERE user_id=" + userId + readFilter);
                var dr = cmd.ExecuteReader();

                var list = new List<Tuple<long, DateTime>>();
                while (dr.Read())
                    list.Add(new Tuple<long, DateTime>(dr.GetInt64(0), dr.GetDateTime(1)));
                dr.Close();

                return new AResponse<List<Tuple<long, DateTime>>>(list);
            }
            catch (Exception ex)
            {
                return AResponse<List<Tuple<long, DateTime>>>.CreateError(Global.MakeServerError(ex));
            }
        }

        public AResponse<string> GetUserLogText(long userLogId)
        {
            try
            {
                var cmd = Global.CreateCommand("SELECT message_text FROM user_log WHERE id = " + userLogId);
                var dr = cmd.ExecuteReader();

                var message = string.Empty;

                if (dr.Read())
                    message = dr[0].ToString();

                dr.Close();

                cmd.CommandText = "UPDATE user_log SET is_read = 1 WHERE id = " + userLogId;
                cmd.ExecuteNonQuery();

                return new AResponse<string>(message);
            }
            catch (Exception ex)
            {
                return AResponse<string>.CreateError(Global.MakeServerError(ex));
            }
        }

        public AResponse<List<RefItem>> GetUserGroupsNotInVersion(long versionId)
        {
            try
            {
                var cmd = Global.CreateCommand("SELECT id, name FROM user_group WHERE current_version_id IS NULL OR current_version_id <> " + versionId);
                var dr = cmd.ExecuteReader();
                var list = new List<RefItem>();

                while (dr.Read())
                {
                    var item = new RefItem();
                    item.Id = Convert.ToInt32(dr[0]);
                    item.Text = dr[1].ToString();
                    list.Add(item);
                }
                dr.Close();

                return new AResponse<List<RefItem>>(list);
            }
            catch (Exception ex)
            {
                return AResponse<List<RefItem>>.CreateError(Global.MakeServerError(ex));
            }
        }

        public AResponse<List<VersionUserGroupDoc>> GetVersionUserGroupDocs(long versionId)
        {
            var cmd = Global.CreateCommand();

            try
            {
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

                return new AResponse<List<VersionUserGroupDoc>>(list);
            }
            catch (Exception ex)
            {
                return AResponse<List<VersionUserGroupDoc>>.CreateError(Global.MakeServerError(ex));
            }
            finally
            {
                cmd.Connection.Close();
            }
        }

        public AResponse SetVersionUserGroup(VersionUserGroupDoc vug)
        {
            var versionId = vug.Version.Id;
            var userGropId = vug.UserGroup.Id;
            var note = vug.Note;

            var cmd = Global.CreateCommand();
            cmd.Transaction = cmd.Connection.BeginTransaction();

            try
            {
                cmd.CommandText =
                    "INSERT INTO version_user_group " +
                    "(version_id, user_group_id, note) VALUES " +
                    "(:version, :user_group, :note)";
                cmd.Parameters.AddWithValue("version", versionId);
                cmd.Parameters.AddWithValue("user_group", userGropId);
                cmd.Parameters.AddWithValue("note", note);

                cmd.ExecuteNonQuery();

                cmd.CommandText = "UPDATE user_group SET current_version_id = :version WHERE id = :user_group";
                cmd.Parameters.RemoveAt(2);

                cmd.ExecuteNonQuery();

                cmd.Transaction.Commit();

                return new AResponse();
            }
            catch (Exception ex)
            {
                cmd.Transaction.Rollback();
                return new AResponse(Global.MakeServerError(ex));
            }
            finally
            {
                cmd.Connection.Close();
            }
        }


        //****************
        //****************
        //****************




        private void MakeCommandText(string tableName, long id, SQLiteCommand command, params object[] cmdParams)
        {
            command.Parameters.Clear();

            for (int i = 0; i < cmdParams.Length; i += 2)
            {
                var prm = new SQLiteParameter();
                prm.ParameterName = cmdParams[i].ToString();
                prm.Value = cmdParams[i + 1];
                command.Parameters.Add(prm);
            }

            command.CommandText = MakeCommandText(tableName, id, command.Parameters);
        }

        private string MakeCommandText(string tableName, long id, SQLiteParameterCollection paramColl)
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

        #endregion
    }
}
