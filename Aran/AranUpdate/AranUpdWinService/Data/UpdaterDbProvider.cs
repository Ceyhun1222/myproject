using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AranUpdater;
using AranUpdater.CommonData;

namespace AranUpdWinService.Data
{
    class UpdaterDbProvider : CommonDbProvider
    {
        private int _userId;

        public UpdaterDbProvider()
        {

        }

        public void Open(string server, int port)
        {
            base.Open(server, port, "Updater");
        }

        public bool Register(string userName)
        {
            _userId = GetResponse<int>("Register", userName);
            return IsRegistered;
        }

        public AranVersionInfo GetNewVersion()
        {
            return GetResponse<AranVersionInfo>("GetNewVersion", _userId);
        }

        public void AddLog(string log)
        {
            GetResponse("AddLog", CreateUserRequest<string>(log));
        }

        public void SetLastVersion(int versionId, LastVersionType versionType)
        {
            GetResponse(
                "SetLastVersion",
                CreateUserRequest<int, LastVersionType>(versionId, versionType));
        }

        public byte[] ReadFile(int fileId)
        {
            return GetResponse<byte[]>("ReadFile", fileId);
        }
        public bool IsRegistered
        {
            get { return _userId > 0; }
        }


        
        private UserRequest<T> CreateUserRequest<T>(T value)
        {
            return new UserRequest<T> { UserId = _userId, Value = value };
        }

        private UserRequest<T1, T2> CreateUserRequest<T1, T2>(T1 value1, T2 value2)
        {
            return new UserRequest<T1, T2> { UserId = _userId, Value1 = value1, Value2 = value2 };
        }
    }
}
