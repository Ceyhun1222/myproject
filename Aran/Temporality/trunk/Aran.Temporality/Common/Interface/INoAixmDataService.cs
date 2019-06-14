using System;
using System.Collections.Generic;
using Aran.Temporality.Common.Entity;
using Aran.Temporality.Common.Entity.Util;
using Aran.Temporality.Common.Logging;

namespace Aran.Temporality.Common.Interface
{
    public interface INoAixmDataService
    {
        #region User

        string GetUserName(int userId);
        bool IsUserSecured(int userId);

        User Login(int userId, string password);

        //admin calls 
        int CreateUser(string name);
        int ResetPassword(int userId, string userName);//set password equal to name
        int ResetPasswordById(int userId);//set password equal to name
        bool DeleteUserById(int userId);//returns true if success
        bool DeleteUser(int userId, string userName);//returns true if success
        bool SetUserName(int userId, string name);

        //superadmin calls
        bool SetRole(int userId, int role, string userName, string roleName);
        bool SetUserRole(int userId, int role);
        bool SetUserModules(int id, int currentModuleFlag);
        bool SetModules(int id, int currentModuleFlag, string userName, string moduleName);

        //
        IList<User> GetAllUsers();

        bool SetUserActiveSlotId(int userId, int privateSlotId);

        bool ChangeMyPassword(int userId, string oldPassword, string newPassword);
  
        #endregion 

        #region AccessRight

        bool SetUserRights(AccessRightZipped currentAccessRightZipped);
        AccessRightZipped GetDefaultUserRights(int userId);
        IList<AccessRightZipped> GetUserRights(int userId);
        bool SetRights(AccessRightZipped currentAccessRightZipped, string groupName);

        //workpackage operations 
        void DeleteAccessRightsByWorkPackage(int packageId);

        #endregion

        #region Storage

        Storage GetStorageByName(string storage);
        Storage GetStorageById(int storageId);

        #endregion

        #region Slots

        IList<PublicSlot> GetPublicSlots();
        IList<PrivateSlot> GetPrivateSlots(int publicSlotId, int userId);
        int CreatePublicSlot(PublicSlot publicSlot);
        PublicSlot GetPublicSlotById(int id);
        bool DeletePublicSlot(int id);
        bool UpdatePublicSlot(PublicSlot publicSlot);

        int CreatePrivateSlot(PrivateSlot privateSlot);
        PrivateSlot GetPrivateSlotById(int id);
        bool UpdatePrivateSlot(PrivateSlot privateSlot);
     
        #endregion

        #region Business Rules

        IList<BusinessRuleUtil> GetBusinessRules();
        void ActivateRule(int ruleId, bool active);

        #endregion

        #region ProblemReport
        
        void UpdateProblemReport(ProblemReport problemReport);

        ProblemReport GetProblemReport(int publicSlotId, int privateSlotId, int configId, ReportType reportType);



        #endregion


        #region AIMSL

        AimslOperation AimslUploadAixmFile(byte[] zippedfile, string fileName);
        List<AimslOperation> AimslGetAllAimslOperations();
        bool AimslTestConnection();

        #endregion

        #region Notam

        int SaveNotam(Notam notam);
        bool UpdateNotam(Notam notam);

        Notam GetNotamById(int id);

        IList<Notam> GetAllNotams();

        #endregion

        #region FeatureReport

        void SaveFeatureReport(FeatureReportZipped zippedReport);

        IList<FeatureReportZipped> GetFeatureReportsByIdentifier(string identifier);

        #endregion

        #region FeatureScreenshot

        void SaveFeatureScreenshot(FeatureScreenshot screenshot);

        IList<FeatureScreenshot> GetFeatureScreenshotsByIdentifier(string identifier);

        #endregion

        #region FeatureDependency

        IList<FeatureDependencyConfiguration> GetFeatureDependencies();
        bool DeleteFeatureDependencyConfiguration(int id);
        int CreateFeatureDependencyConfiguration(FeatureDependencyConfiguration entity);
        bool UpdateFeatureDependencyConfiguration(FeatureDependencyConfiguration entity);

        #endregion

        #region Configuration

        int UpdateConfiguration(Configuration configuration);
        IList<Configuration> GetAllConfigurations();
        Configuration GetConfigurationByName(string selectedConfiguration);
        bool LogConfigured();
        bool SetLogLevel(LogLevel level);
        LogLevel GetLogLevel();
        #endregion

        IList<DataSourceTemplate> GetAllDataSourceTemplates();
        int CreateDataSourceTemplate(DataSourceTemplate entity);
        bool DeleteDataSourceTemplate(int id);
        DataSourceTemplate GetDataSourceTemplateById(int id);
        bool UpdateDataSourceTemplate(DataSourceTemplate entity);
        IList<FeatureDependencyConfiguration> GetFeatureDependenciesByTemplate(int templateId);
        IList<UserGroup> GetAllUserGroups();
        int CreateGroup(string groupName);
        bool DeleteGroupById(int id);
        bool SetUserGroup(int userId, int groupId);
        bool SetGroupName(int id, string currentGroupName);


        IList<int> GetLogIds(DateTime fromDate, DateTime toDate,
            string storageMask,
            string applicationMask,
            string userMask, 
            string addressMask, 
            string actionMask,
            string parameterMask,
            bool? accessGranted);

        IList<LogEntry> GetLogByIds(List<int> ids);

        IList<object> GetLogValues(string field);
        SlotValidationOption GetSlotValidationOption(int slotId);
        bool UpdateSlotValidationOption(int slotId, SlotValidationOption newOption);
        IList<UserModuleVersion> GetUserModuleVersions(int userId);
        byte[] GetUpdate(string aplicationVersion);
        string GetUpdateVersion(string s);
    }
}
