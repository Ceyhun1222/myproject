using AerodromeServices.DataContract;
using AerodromeServices.Helpers;
using System;
using System.Collections.Generic;
using System.ServiceModel;

namespace AerodromeServices.Services_Contract
{
    [ServiceContract(CallbackContract = typeof(IAmdbManagerServiceCallback), SessionMode = SessionMode.Required)]
    interface IAmdbManagerService
    {
        [CustomParameterInspector]
        [OperationContract]
        void Login();

        [CustomParameterInspector]
        [OperationContract]
        List<User> GetAllUser();

        [CustomParameterInspector]
        [OperationContract]
        List<string> GetOrganizations();

        [CustomParameterInspector]
        [OperationContract]
        List<string> GetAerodromes(string organization);

        [CustomParameterInspector]
        [OperationContract]
        User GetUser(long id);

        [CustomParameterInspector]
        [OperationContract]
        User GetCurrentUser();

        [CustomParameterInspector]
        [OperationContract]
        void DeleteUser(long id);

        [CustomParameterInspector]
        [OperationContract]
        void CreateUser(User user);

        [CustomParameterInspector]
        [OperationContract]
        bool ExistsUser(string username, string password);

        [CustomParameterInspector]
        [OperationContract]
        void UpdateUser(User user);

        [CustomParameterInspector]
        [OperationContract]
        void DisableUser(long id, bool disabled);

        [CustomParameterInspector]
        [OperationContract]
        void ChangePassword(long id, string oldPassword, string newPassword);

        [CustomParameterInspector]
        [OperationContract]
        byte[] GetConfigFile(long id);

        [CustomParameterInspector]
        [OperationContract]
        void Upload(AmdbMetadata chart, byte[] sourece);

        [CustomParameterInspector]
        [OperationContract]
        AmdbMetadata GetLatestAmdbVersion(Guid identifier);

        [CustomParameterInspector]
        [OperationContract]
        byte[] GetPreviewOf(long id);

        [CustomParameterInspector]
        [OperationContract]
        byte[] GetSourceOf(long id, bool locked = false);

        [CustomParameterInspector]
        [OperationContract]
        List<AmdbMetadata> GetAllAmdbFiles(string name, long? userId,
            String organization, String aerodrome,
            bool? locked, DateTime? createdBeginning,
            DateTime? createdEnding);

        [CustomParameterInspector]
        [OperationContract]
        List<AmdbMetadata> GetHistoryOf(Guid identifier);

        [CustomParameterInspector]
        [OperationContract]
        AmdbMetadata GetAmdb(Guid identifier, string version);

        [CustomParameterInspector]
        [OperationContract]
        AmdbMetadata GetAmdbById(long id);

        [CustomParameterInspector]
        [OperationContract]
        void LockAmdb(long id, bool locked);

        [CustomParameterInspector]
        [OperationContract]
        void DeleteAllAmdbVersions(Guid identifier);

        [CustomParameterInspector]
        [OperationContract]
        void DeleteAmdbById(long id);

        [CustomParameterInspector]
        [OperationContract]
        void DeleteAmdbByVersion(Guid id, string version);
    }
}
