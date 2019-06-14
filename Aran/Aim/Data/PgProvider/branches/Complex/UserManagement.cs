using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Aran.Aim.Data
{
    public class UserManagement : IUserManagement
    {
        private IDbConnection _connection;


        public UserManagement(IDbConnection connection)
        {
            _connection = connection;
        }


        public InsertingResult InsertUser(User user)
        {
            IDbCommand dbCommand = null;

            try {
                dbCommand = _connection.CreateCommand();
                dbCommand.Transaction = _connection.BeginTransaction();

                dbCommand.CommandText = string.Format(
                    "INSERT INTO aim_user (name, password, privilege) VALUES('{0}','{1}',{2});", 
                    user.Name, user.Password, (int)user.Privilege);
                dbCommand.ExecuteScalar();
                user.Id = CurrValOfSequence("aim_user", dbCommand.Transaction);

                dbCommand.CommandText = "INSERT INTO aim_user_feat_types(user_id,feat_type) VALUES ";
                StringBuilder stringBuilder = new StringBuilder();
                foreach (int featType in user.FeatureTypes) {
                    stringBuilder.Append(" (" + user.Id + ", " + featType + ") ,");
                }
                dbCommand.CommandText += stringBuilder.Remove(stringBuilder.Length - 1, 1).ToString();
                dbCommand.ExecuteScalar();
                dbCommand.Transaction.Commit();
                return new InsertingResult(true);
            }
            catch (Exception ex) {
                dbCommand.Transaction.Rollback();
                string message = CommonData.GetErrorMessage(ex);
                return new InsertingResult(false, message);
            }
        }

        public InsertingResult UpdateUser(User user)
        {
            IDbCommand dbCommand = null;

            try {
                IDbTransaction transaction = _connection.BeginTransaction();
                dbCommand = _connection.CreateCommand();
                dbCommand.Transaction = transaction;

                dbCommand.CommandText = "DELETE FROM aim_user_feat_types WHERE user_id = " + user.Id + ";";
                dbCommand.ExecuteScalar();

                dbCommand.CommandText = string.Format(
                    "UPDATE aim_user SET name = '{0}', password = '{1}', privilege = '{2}' WHERE \"Id\" = {3}", 
                    user.Name, user.Password, (int)user.Privilege, user.Id);
                dbCommand.ExecuteNonQuery();


                dbCommand.CommandText = "INSERT INTO aim_user_feat_types(user_id,feat_type) VALUES ";
                StringBuilder stringBuilder = new StringBuilder();
                foreach (FeatureType featType in user.FeatureTypes) {
                    stringBuilder.Append(" (" + user.Id + ", " + ((int)featType).ToString() + "),");
                }
                dbCommand.CommandText += stringBuilder.Remove(stringBuilder.Length - 1, 1).ToString();
                dbCommand.ExecuteScalar();
                dbCommand.Transaction.Commit();
                return new InsertingResult(true);
            }
            catch (Exception ex) {
                dbCommand.Transaction.Rollback();
                string message = CommonData.GetErrorMessage(ex);
                return new InsertingResult(false, message);
            }
        }

        public InsertingResult DeleteUser(User user)
        {
            IDbCommand dbCommand = null;

            try {
                dbCommand = _connection.CreateCommand();
                dbCommand.CommandText = string.Format("DELETE FROM aim_user WHERE \"Id\" = {0}", user.Id);
                dbCommand.ExecuteScalar();
                return new InsertingResult(true);
            }
            catch (Exception ex) {
                string message = CommonData.GetErrorMessage(ex);
                return new InsertingResult(false, message);
            }
        }

        public GettingResult ReadUsers()
        {
            return GetAllUsers();
        }

        public GettingResult ReadUserByName(string userName)
        {
            return GetAllUsers("WHERE name = '" + userName + "'");
        }

        public GettingResult ReadUsersById(long userId)
        {
            return GetAllUsers("where \"Id\" = " + userId);
        }


        private long CurrValOfSequence(string tableName, IDbTransaction transaction)
        {
            object val = CreateCommand(transaction, string.Format("SELECT currval ('\"{0}_Id_seq\"')", tableName)).ExecuteScalar();
            return Convert.ToInt64(val);
        }

        private IDbCommand CreateCommand(IDbTransaction transaction, string commandText = null)
        {
            IDbCommand command = _connection.CreateCommand();
            command.Transaction = transaction;
            if (commandText != null)
                command.CommandText = commandText;
            return command;
        }

        private GettingResult GetAllUsers(string whereSqlCommand = "")
        {
            IDataReader dataReader = null;
            try {
                string sqlString = "select \"Id\", name, password, privilege from aim_user " + whereSqlCommand;

                IDbCommand command = _connection.CreateCommand();
                command.CommandText = sqlString;
                dataReader = command.ExecuteReader();
                List<User> userList = new List<User>();
                while (dataReader.Read()) {
                    User user = new User();
                    user.Id = (long)dataReader[0];
                    user.Name = dataReader[1].ToString();
                    user.Password = dataReader[2].ToString();
                    user.Privilege = (Privilige)dataReader[3];
                    userList.Add(user);
                }
                if (userList.Count > 0) {
                    command.CommandText = "select user_id, feat_type from aim_user_feat_types where user_id in (";
                    foreach (var item in userList) {
                        command.CommandText += item.Id + ", ";
                    }
                    command.CommandText = command.CommandText.Remove(command.CommandText.Length - 2) + ")";

                    dataReader = command.ExecuteReader();
                    long currId;
                    while (dataReader.Read()) {
                        currId = (long)dataReader[0];
                        userList.Find(user => user.Id == currId).FeatureTypes.Add((int)dataReader[1]);
                    }
                    dataReader.Close();
                }
                GettingResult getResult = new GettingResult(true);
                getResult.List = userList;
                return getResult;
            }
            catch (Exception exc) {
                if (dataReader != null)
                    dataReader.Close();
                _connection.Close();
                _connection.Open();
                return new GettingResult(false, CommonData.GetErrorMessage(exc));
            }
        }

    }
}
