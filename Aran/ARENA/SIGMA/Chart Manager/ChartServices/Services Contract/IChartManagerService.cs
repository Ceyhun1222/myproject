using ChartServices.DataContract;
using ChartServices.Helpers;
using System;
using System.Collections.Generic;
using System.ServiceModel;

namespace ChartServices.Services_Contract
{
    [ServiceContract(CallbackContract = typeof(IChartManagerServiceCallback), SessionMode = SessionMode.Required)]
    interface IChartManagerService
    {
        [CustomParameterInspector]
        [OperationContract]
        void Login();

        [CustomParameterInspector]
        [OperationContract]
        IList<ChartUser> GetAllUser();

        [CustomParameterInspector]
        [OperationContract]
        IList<string> GetOrganizations();

        [CustomParameterInspector]
        [OperationContract]
        IList<string> GetAerodromes(string organization);

        [CustomParameterInspector]
        [OperationContract]
        IList<string> GetRunways(string aerodrome);

        [CustomParameterInspector]
        [OperationContract]
        ChartUser GetUser(long id);

        [CustomParameterInspector]
        [OperationContract]
        ChartUser TryGetCurrentUser();

        [CustomParameterInspector]
        [OperationContract]
        void DeleteUser(long id);

        [CustomParameterInspector]
        [OperationContract]
        void CreateUser(ChartUser user);

        [CustomParameterInspector]
        [OperationContract]
        bool ExistsUser(string username, string password);

        [CustomParameterInspector]
        [OperationContract]
        void UpdateUser(ChartUser user);

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
        void Upload(ChartWithReference chart, byte[] preview, byte[] source);

        [CustomParameterInspector]
        [OperationContract]
        void UploadWithUpdate(ChartWithReference chart, byte[] preview, byte[] source, long updateDataId);

        [CustomParameterInspector]
        [OperationContract]
        long UploadUpdateData(ChartUpdateData metadata, byte[] source, IList<Guid> chartIdentifierList);

        [CustomParameterInspector]
        [OperationContract]
        Chart GetLatestChartVersion(Guid identifier, DateTime dateTime);

        [CustomParameterInspector]
        [OperationContract]
        byte[] GetPreviewOf(long id);

        [CustomParameterInspector]
        [OperationContract]
        byte[] GetUpdateSource(long id);

        [CustomParameterInspector]
        [OperationContract]
        byte[] GetSourceOf(long id, bool locked = false);

        [CustomParameterInspector]
        [OperationContract]
        IList<Chart> GetAllCharts(ChartType? type, string name, long? userId,
            String organization, String aerodrome, String rwyDir,
            bool? locked, DateTime? createdBeginning,
            DateTime? createdEnding, DateTime? airacDateBeginning,
            DateTime? airacDateEnding);

        [CustomParameterInspector]
        [OperationContract]
        IList<Chart> GetAffectedCharts(IList<string> idList, DateTime effectiveDate);

        [CustomParameterInspector]
        [OperationContract]
        bool HasPendingUpdate();

        [CustomParameterInspector]
        [OperationContract]
        IList<Chart> GetPendingUpdateList();

        [CustomParameterInspector]
        [OperationContract]
        IList<Chart> GetHistoryOf(Guid identifier);

        [CustomParameterInspector]
        [OperationContract]
        Chart GetChart(Guid identifier, DateTime dateTime, int version);

        [CustomParameterInspector]
        [OperationContract]
        Chart GetChartById(long id);

        [CustomParameterInspector]
        [OperationContract]
        void LockChart(long id, bool locked);

        [CustomParameterInspector]
        [OperationContract]
        void DeleteAllChartVersions(Guid identifier);

        [CustomParameterInspector]
        [OperationContract]
        void DeleteChartById(long id);

        [CustomParameterInspector]
        [OperationContract]
        void DeleteChartByEffectiveDate(Guid id, DateTime dateTime);
    }
}
