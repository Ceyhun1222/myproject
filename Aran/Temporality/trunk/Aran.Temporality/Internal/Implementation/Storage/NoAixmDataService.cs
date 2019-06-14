using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using AIMSLServiceClient.Services;
using Aran.Temporality.Common.Config;
using Aran.Temporality.Common.Entity;
using Aran.Temporality.Common.Entity.Enum;
using Aran.Temporality.Common.Entity.Util;
using Aran.Temporality.Common.Exceptions;
using Aran.Temporality.Common.Interface;
using Aran.Temporality.Common.Logging;
using Aran.Temporality.Internal.Abstract;
using Aran.Temporality.Internal.Attribute;
using Aran.Temporality.Internal.Remote.Protocol;

namespace Aran.Temporality.Internal.Implementation.Storage
{
    internal class NoAixmDataService : INoAixmDataService
    {
        private readonly object _lockObject = new object();
        private IService _aimslService;
        public IService AimslService
        {
            get
            {
                lock (_lockObject)
                {
                    if (_aimslService == null)
                    {
                        AIMSLServiceClient.Services.AimslService.Init();
                        _aimslService = AIMSLServiceClient.Services.AimslService.GetService();
                    }
                }
               
                return _aimslService;
            }
        }

        public static void PreprocessCall(CommunicationRequest request)
        {
            var paramList = request.Params?.ToList() ?? new List<Object>();

            if (request.MethodName == "ResetPasswordById")
            {
                request.MethodName = "ResetPassword";
                paramList.Add(StorageService.GetUserName((int)paramList[0]));
                request.Params = paramList.ToArray();
            }
            if (request.MethodName == "DeleteUserById")
            {
                request.MethodName = "DeleteUser";
                paramList.Add(StorageService.GetUserName((int)paramList[0]));
                request.Params = paramList.ToArray();
            }
            if (request.MethodName == "SetUserRole")
            {
                request.MethodName = "SetRole";
                paramList.Add(StorageService.GetUserName((int)paramList[0]));
                paramList.Add(GetRoleName((int)paramList[1]));
                request.Params = paramList.ToArray();
            }
            if (request.MethodName == "SetUserModules")
            {
                request.MethodName = "SetModules";
                paramList.Add(StorageService.GetUserName((int)paramList[0]));
                paramList.Add(GetModuleName((int)paramList[1]));
                request.Params = paramList.ToArray();
            }
            if (request.MethodName == "SetUserRights")
            {
                request.MethodName = "SetRights";
                paramList.Add(StorageService.GetGroupName(((AccessRightZipped)paramList[0]).UserGroupId));
                request.Params = paramList.ToArray();
            }
        }

        public static string GetRoleName(int role)
        {
            var result = "";
            if (((int)UserRole.Admin & role) > 0)
            {
                result += " Administrator";
            }
            if (((int)UserRole.Tester & role) > 0)
            {
                result += " Editor";
            }
            if (((int)UserRole.Observer & role) > 0)
            {
                result += " Viewer";
            }

            if (string.IsNullOrEmpty(result)) result = "no role";
            return result;
        }

        public static string GetModuleName(int role)
        {
            var result = "";
            if (((int)Module.Arena & role) > 0)
            {
                result += " ARENA";
            }
            if (((int)Module.Delta & role) > 0)
            {
                result += " DELTA";
            }
            if (((int)Module.External & role) > 0)
            {
                result += " External";
            }
            if (((int)Module.Omega & role) > 0)
            {
                result += " OMEGA";
            }
            if (((int)Module.Panda & role) > 0)
            {
                result += " PANDA";
            }
            if (((int)Module.Tossm & role) > 0)
            {
                result += " TOSSM";
            }

            if (string.IsNullOrEmpty(result)) result = "no role";
            return result;
        }


        public int ResetPasswordById(int userId)
        {
            return ResetPassword(userId, StorageService.GetUserName(userId));
        }

        public bool DeleteUserById(int userId)
        {
            return DeleteUser(userId, StorageService.GetUserName(userId));
        }

        public bool SetUserRole(int userId, int role)
        {
            return SetRole(userId, role, StorageService.GetUserName(userId), GetRoleName(role));
        }

        public bool SetUserModules(int id, int currentModuleFlag)
        {
            return SetModules(id, currentModuleFlag, StorageService.GetUserName(id),
                GetModuleName(currentModuleFlag));
        }


        public bool SetUserRights(AccessRightZipped currentAccessRightZipped)
        {
            return SetRights(currentAccessRightZipped,
                StorageService.GetGroupName(currentAccessRightZipped.UserGroupId));
        }





        #region Implementation of INoAixmDataService

        [LogOperation(Action = LogActions.CreateUser, Arguments = new int[] { 0 })]

        //[SecureOperation(AccessOperation.ChangeUsers)]
        public int CreateUser(string name)
        {
            return StorageService.CreateUser(name);
        }

        [LogOperation(Action = LogActions.ResetPassword, Arguments = new int[] { 1 })]
        //[SecureOperation(AccessOperation.ChangeUsers)]
        public int ResetPassword(int userId, string userName)
        {
            return StorageService.ResetPasswordById(userId);
        }

        [LogOperation(Action = LogActions.DeleteUser, Arguments = new int[] { 1 })]
        //[SecureOperation(AccessOperation.ChangeUsers)]
        public bool DeleteUser(int userId, string userName)
        {
            return StorageService.DeleteUserById(userId);
        }

        //[SecureOperation(AccessOperation.ChangeUsers)]
        [LogOperation(Action = LogActions.ChangeRole, Arguments = new int[] { 2, 3 })]
        public bool SetRole(int userId, int role, string userName, string roleName)
        {
            return StorageService.SetUserRole(userId, role);
        }

        //[SecureOperation(AccessOperation.ChangeUsers)]

        [LogOperation(Action = LogActions.ChangeModuleAccessRights, Arguments = new int[] { 2, 3 })]
        public bool SetModules(int id, int currentModuleFlag, string userName, string moduleName)
        {
            return StorageService.SetUserModules(id, currentModuleFlag);
        }

        //[SecureOperation(AccessOperation.ChangeUsers)]

        [LogOperation(Action = LogActions.EditUser, Arguments = new[] { 0, 1 })]
        public bool SetUserName(int userId, string name)
        {
            return StorageService.SetUserName(userId, name);
        }

        public IList<User> GetAllUsers()
        {
            return StorageService.GetAllUsers();
        }

        public bool SetUserActiveSlotId(int userId, int privateSlotId)
        {
            return StorageService.SetUserActiveSlotId(userId, privateSlotId);
        }

        //[SecureOperation(AccessOperation.ChangeUsers)]

        public bool ChangeMyPassword(int userId, string oldPassword, string newPassword)
        {
            return StorageService.ChangeMyPassword(userId, oldPassword, newPassword);
        }

        #endregion

        #region Access

        public AccessRightZipped GetDefaultUserRights(int userId)
        {
            return StorageService.GetDefaultUserRights(userId);
        }

        public IList<AccessRightZipped> GetUserRights(int userId)
        {
            return StorageService.GetUserRights(userId);
        }

        //[SecureOperation(AccessOperation.ChangeGroups)]

        [LogOperation(Action = LogActions.ChangeAccessRights, Arguments = new[] { 1 })]
        public bool SetRights(AccessRightZipped currentAccessRightZipped, string groupName)
        {
            return StorageService.SetUserRights(currentAccessRightZipped);
        }

        //[SecureOperation(AccessOperation.ChangeGroups)]        
        public void DeleteAccessRightsByWorkPackage(int packageId)
        {
            StorageService.DeleteAccessRightsByWorkPackage(packageId);
        }

        public Common.Entity.Storage GetStorageByName(string storage)
        {
            return StorageService.GetStorageByName(storage);
        }

        public Common.Entity.Storage GetStorageById(int storageId)
        {
            return StorageService.GetStorageById(storageId);
        }

        public IList<PublicSlot> GetPublicSlots()
        {
            return StorageService.GetPublicSlots();
        }

        public IList<PrivateSlot> GetPrivateSlots(int publicSlotId, int userId)
        {
            return StorageService.GetPrivateSlots(publicSlotId, userId);
        }


        //[SecureOperation(PackageOperation.CreatePackage)]
        public int CreatePublicSlot(PublicSlot publicSlot)
        {
            if (publicSlot.EffectiveDate.Date.CompareTo(DateTime.Today) <= 0)
                throw new OperationException("Can't create public slot in the past.");
            return StorageService.CreatePublicSlot(publicSlot);
        }


        public PublicSlot GetPublicSlotById(int id)
        {
            return StorageService.GetPublicSlotById(id);
        }

        //[SecureOperation(PackageOperation.DeletePackage)]

        public bool DeletePublicSlot(int id)
        {
            return StorageService.DeletePublicSlot(id);
        }

        public bool UpdatePublicSlot(PublicSlot publicSlot)
        {
            return StorageService.UpdatePublicSlot(publicSlot);
        }

        //[SecureOperation(PackageOperation.CreatePackage)]
        public int CreatePrivateSlot(PrivateSlot privateSlot)
        {
            privateSlot.StatusChangeDate = DateTime.Now;
            privateSlot.CreationDate = DateTime.Now;
            var publicSlot = StorageService.GetPublicSlotById(privateSlot.PublicSlot.Id);
            if (publicSlot.Frozen)
                throw new OperationException("Can't create private slot in published slot.");
            return StorageService.CreatePrivateSlot(privateSlot);
        }

        public PrivateSlot GetPrivateSlotById(int id)
        {
            return StorageService.GetPrivateSlotById(id);
        }

        public bool UpdatePrivateSlot(PrivateSlot privateSlot)
        {
            return StorageService.UpdatePrivateSlot(privateSlot);
        }



        public string GetUserName(int userId)
        {
            return StorageService.GetUserName(userId);
        }

        public bool IsUserSecured(int userId)
        {
            return StorageService.IsUserSecured(userId);
        }

        [LogOperation(Action = LogActions.Login)]
        public User Login(int userId, string password)
        {
            return StorageService.Login(userId, password);
        }


        #endregion

        #region Rules

        public IList<BusinessRuleUtil> GetBusinessRules()
        {
            return StorageService.GetBusinessRules();
        }

        public void ActivateRule(int ruleId, bool active)
        {
            StorageService.ActivateRule(ruleId, active);
        }

        #endregion


        public void UpdateProblemReport(ProblemReport problemReport)
        {
            StorageService.UpdateProblemReport(problemReport);
        }

        public ProblemReport GetProblemReport(int publicSlotId, int privateSlotId, int configId, ReportType reportType)
        {
            return StorageService.GetProblemReport(publicSlotId, privateSlotId, configId, reportType);
        }



        public IList<FeatureDependencyConfiguration> GetFeatureDependencies()
        {
            return StorageService.GetFeatureDependencies();
        }

        public bool DeleteFeatureDependencyConfiguration(int id)
        {
            return StorageService.DeleteFeatureDependencyConfiguration(id);
        }

        public int CreateFeatureDependencyConfiguration(FeatureDependencyConfiguration entity)
        {
            return StorageService.CreateFeatureDependencyConfiguration(entity);
        }

        public bool UpdateFeatureDependencyConfiguration(FeatureDependencyConfiguration entity)
        {
            return StorageService.UpdateFeatureDependencyConfiguration(entity);
        }

        public int UpdateConfiguration(Configuration configuration)
        {
            return StorageService.UpdateConfiguration(configuration);
        }

        public IList<Configuration> GetAllConfigurations()
        {
            return StorageService.GetAllConfigurations();
        }

        public Configuration GetConfigurationByName(string selectedConfiguration)
        {
            return StorageService.GetConfigurationByName(selectedConfiguration);
        }

        public bool LogConfigured()
        {
            return LogManager.Configured;
        }

        public LogLevel GetLogLevel()
        {
            return LogManager.GetLogLevel();
        }

        [SecureOperation(AccessOperation.ChangeLogLevel)]
        public bool SetLogLevel(LogLevel level)
        {
            return LogManager.SetLogLevel(level);
        }

        public IList<DataSourceTemplate> GetAllDataSourceTemplates()
        {
            return StorageService.GetAllDataSourceTemplates();
        }

        public int CreateDataSourceTemplate(DataSourceTemplate entity)
        {
            return StorageService.CreateDataSourceTemplate(entity);
        }

        public bool DeleteDataSourceTemplate(int id)
        {
            return StorageService.DeleteDataSourceTemplate(id);
        }

        public DataSourceTemplate GetDataSourceTemplateById(int id)
        {
            return StorageService.GetDataSourceTemplateById(id);
        }

        public bool UpdateDataSourceTemplate(DataSourceTemplate entity)
        {
            return StorageService.UpdateDataSourceTemplate(entity);
        }

        public IList<FeatureDependencyConfiguration> GetFeatureDependenciesByTemplate(int templateId)
        {
            return StorageService.GetFeatureDependenciesByTemplate(templateId);
        }

        public IList<UserGroup> GetAllUserGroups()
        {
            return StorageService.GetAllUserGroups();
        }

        public int CreateGroup(string groupName)
        {
            return StorageService.CreateGroup(groupName);
        }

        public bool DeleteGroupById(int id)
        {
            return StorageService.DeleteGroupById(id);
        }

        public bool SetUserGroup(int userId, int groupId)
        {
            return StorageService.SetUserGroup(userId, groupId);
        }

        public bool SetGroupName(int id, string currentGroupName)
        {
            return StorageService.SetGroupName(id, currentGroupName);
        }

        public IList<int> GetLogIds(DateTime fromDate, DateTime toDate, string storageMask, string applicationMask,
            string userMask,
            string addressMask, string actionMask, string parameterMask, bool? accessGranted)
        {
            return StorageService.GetLogIds(fromDate, toDate, storageMask, applicationMask, userMask, addressMask,
                actionMask, parameterMask, accessGranted);
        }


        public IList<LogEntry> GetLogByIds(List<int> ids)
        {
            return StorageService.GetLogByIds(ids);
        }

        public IList<object> GetLogValues(string field)
        {
            return StorageService.GetLogValues(field);
        }

        public SlotValidationOption GetSlotValidationOption(int slotId)
        {
            return StorageService.GetSlotValidationOption(slotId);
        }

        public bool UpdateSlotValidationOption(int slotId, SlotValidationOption newOption)
        {
            return StorageService.UpdateSlotValidationOption(slotId, newOption);
        }

        public IList<UserModuleVersion> GetUserModuleVersions(int userId)
        {
            return StorageService.GetUserModuleVersions(userId);
        }

        public List<string> GetUsersOfPrivateSlot(int id)
        {
            return StorageService.GetUsersOfPrivateSlot(id);
        }



        public byte[] GetUpdate(string aplicationVersion)
        {
            try
            {
                var j = aplicationVersion.IndexOf(":", StringComparison.Ordinal);

                var application = aplicationVersion;
                var version = aplicationVersion;

                if (j > -1)
                {
                    application = aplicationVersion.Substring(0, j);
                    version = aplicationVersion.Substring(j + 1);
                }

                var dllRepo = ConfigUtil.DllRepo;

                var currentDataFile = dllRepo + "\\" + application + "\\data.zip";
                var currentVersionFile = dllRepo + "\\" + application + "\\version.txt";

                if (File.Exists(currentDataFile) && File.Exists(currentVersionFile))
                {
                    var currentVersion = File.ReadAllText(currentVersionFile).Trim();

                    if (!Equals(currentVersion, version))
                    {

                        var info = new FileInfo(currentDataFile);
                        byte[] data = new byte[info.Length];
                        using (var reader = new FileStream(currentDataFile, FileMode.Open))
                        {
                            reader.Read(data, 0, data.Length);
                        }
                        return data;

                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.GetLogger(typeof(NoAixmDataService)).Error(ex, $"Error on getting update. Application version {aplicationVersion}");
            }


            return null;
        }

        public string GetUpdateVersion(string aplicationVersion)
        {
            try
            {
                var j = aplicationVersion.IndexOf(":", StringComparison.Ordinal);

                var application = aplicationVersion;

                if (j > -1)
                {
                    application = aplicationVersion.Substring(0, j);
                }

                var dllRepo = ConfigUtil.DllRepo;

                var currentDataFile = dllRepo + "\\" + application + "\\data.zip";
                var currentVersionFile = dllRepo + "\\" + application + "\\version.txt";

                if (File.Exists(currentDataFile) && File.Exists(currentVersionFile))
                {
                    var version = File.ReadAllText(currentVersionFile);
                    version = version.Trim();
                    return version;
                }
            }
            catch (Exception ex)
            {
                LogManager.GetLogger(typeof(NoAixmDataService)).Error(ex, $"Error on getting update version. Application version {aplicationVersion}");
            }


            return null;
        }


        public AimslOperation AimslUploadAixmFile(byte[] zippedfile, string fileName)
        {

            AimslOperation aimslOperation;
            try
            {
                var result = AimslService.UploadZippedFile(zippedfile, fileName);
                aimslOperation = new AimslOperation
                {
                    Username = GetUser(),
                    JobId = result.Uuid,
                    Status = result.Status,
                    FileName = fileName,
                    CreationTime = DateTime.Now,
                    LastChangeTime = DateTime.Now,
                    InternalStatus = AimslOperationStatusType.Opened
                };

                if (aimslOperation.Status.Equals("FAILED") || aimslOperation.JobId == null)
                {
                    aimslOperation.InternalStatus = AimslOperationStatusType.Destroyed;
                    LogManager.GetLogger(typeof(NoAixmDataService)).Error($"{fileName} aimsl upload failed. Status {result.Status}");
                }
                else
                {
                    LogManager.GetLogger(typeof(NoAixmDataService)).Debug($"{fileName} aimsl uploaded. JobId: {aimslOperation.JobId}");
                }
                
            }
            catch (Exception ex)
            {
                var innerEx = ex;
                while (innerEx.InnerException != null)
                {
                    innerEx = innerEx.InnerException;
                }

                aimslOperation = new AimslOperation
                {
                    Username = GetUser(),
                    Status = "ERROR",
                    FileName = fileName,
                    CreationTime = DateTime.Now,
                    LastChangeTime = DateTime.Now,
                    InternalStatus = AimslOperationStatusType.Destroyed,
                    Description = innerEx.Message
                };

                LogManager.GetLogger(typeof(NoAixmDataService)).Error(ex, $"{fileName} aimsl upload failed.");
            }


            if (StorageService.CreateAimslOperation(aimslOperation) != -1)
                return aimslOperation;
            return null;
        }

        public void AimslAddPullPoint(AimslOperation operation)
        {

            string pullpoint = null;
            try
            {
                pullpoint = AimslService.CreatePullPoint();
            }
            catch (Exception ex)
            {

                LogManager.GetLogger(typeof(NoAixmDataService)).Error(ex, $"aimsl add pullpoint failed. JobId {operation.JobId}  FileName {operation.FileName} ");
            }
            LogManager.GetLogger(typeof(NoAixmDataService)).Debug($"aimsl add pullpoint succeed. JobId {operation.JobId}  FileName {operation.FileName} PullPoint {pullpoint}");
            if (pullpoint != null)
            {
                LogManager.GetLogger(typeof(NoAixmDataService)).Debug($"aimsl add pullpoint succeed. JobId {operation.JobId}  FileName {operation.FileName} PullPoint {pullpoint}");
                operation.PullPoint = pullpoint;
                StorageService.AimslAddPullPoint(operation.Id, pullpoint);
            }
        }

        public void AimslAddSubscription(AimslOperation operation)
        {


            string subscription = null;
            try
            {
                subscription = AimslService.Subscribe(operation.PullPoint, operation.JobId);
            }
            catch (Exception ex)
            {
                LogManager.GetLogger(typeof(NoAixmDataService)).Error(ex, $"aimsl add subscription failed. JobId {operation.JobId}  FileName {operation.FileName} Pullpoint {operation.PullPoint}");
            }

            if (subscription != null)
            {
                LogManager.GetLogger(typeof(NoAixmDataService)).Debug($"aimsl add subscription succeed. JobId {operation.JobId}  FileName {operation.FileName} Pullpoint {operation.PullPoint} Subscription {subscription}");
                operation.Subscription = subscription;
                StorageService.AimslAddSubscription(operation.Id, subscription);
            }
        }

        public void AimslGetMessages(AimslOperation operation)
        {

            try
            {
                string pullPoointId = operation.PullPoint?.Split('=')[1];

                var messages = AimslService.GetMessages(pullPoointId);
                if (messages != null)
                    foreach (var message in messages)
                    {
                        if (message.Message != null)
                        {
                            if (message.Message.Status == "FINISHED" || message.Message.Status == "FAILED")
                            {
                                operation.InternalStatus = AimslOperationStatusType.Closed;

                            }
                            StorageService.AimslAppendMesages(operation.Id,
                                message.Message.ProcessingMessages ?? new List<string>(), message.Message.Status,
                                message.Message.LastChangeTime, operation.InternalStatus == AimslOperationStatusType.Closed);
                        }
                    }

                LogManager.GetLogger(typeof(NoAixmDataService)).Debug($"aimsl getmessages succeed. JobId {operation.JobId}  FileName {operation.FileName} Pullpoint {operation.PullPoint} Subscription {operation.Subscription}");
            }
            catch (Exception ex)
            {
                LogManager.GetLogger(typeof(NoAixmDataService)).Error(ex, $"aimsl getmessages failed. JobId {operation.JobId}  FileName {operation.FileName} Pullpoint {operation.PullPoint} Subscription {operation.Subscription}");
            }
        }

        public void AimslDestroyPullPoint(AimslOperation operation, bool timeout = false)
        {

            try
            {
                string pullPoointId = operation.PullPoint?.Split('=')[1];
                AimslService.DestroyPullPoint(pullPoointId);
                StorageService.AimslDestroy(operation.Id, timeout);
                LogManager.GetLogger(typeof(NoAixmDataService)).Debug($"aimsl destroy succeed. JobId {operation.JobId}  FileName {operation.FileName} Pullpoint {operation.PullPoint} Subscription {operation.Subscription}");
            }
            catch (Exception ex)
            {
                LogManager.GetLogger(typeof(NoAixmDataService)).Error(ex, $"aimsl destroy failed. JobId {operation.JobId}  FileName {operation.FileName} Pullpoint {operation.PullPoint} Subscription {operation.Subscription}");
            }
        }

        public void AimslDestroy(AimslOperation operation, bool timeout = false)
        {
            StorageService.AimslDestroy(operation.Id, timeout);
        }

        public List<AimslOperation> AimslGetAllAimslOperations()
        {
            return StorageService.GetAllAimslOperations();
        }

        public bool AimslTestConnection()
        {
            try
            {
                AimslService.GetAllTopics("*");
                return true;
            }
            catch (Exception ex)
            {
                LogManager.GetLogger(typeof(NoAixmDataService)).Error(ex, $"Connection to aimsl failed.");
                return false;
            }
        }

        public int SaveNotam(Notam notam)
        {
            return StorageService.SaveNotam(notam);
        }

        public bool UpdateNotam(Notam notam)
        {
            return StorageService.UpdateNotam(notam);
        }

        public Notam GetNotamById(int id)
        {
            return StorageService.GetNotamById(id);
        }

        public IList<Notam> GetAllNotams()
        {
            return StorageService.GetAllNotams();
        }


        public void SaveFeatureReport(FeatureReportZipped zippedReport)
        {
            StorageService.SaveFeatureReport(zippedReport);
        }

        public IList<FeatureReportZipped> GetFeatureReportsByIdentifier(string identifier)
        {
            return StorageService.GetFeatureReportsByIdentifier(identifier);
        }

        public void SaveFeatureScreenshot(FeatureScreenshot screenshot)
        {
            StorageService.SaveFeatureScreenshot(screenshot);
        }

        public IList<FeatureScreenshot> GetFeatureScreenshotsByIdentifier(string identifier)
        {
            return StorageService.GetFeatureScreenshotsByIdentifier(identifier);
        }

        private static string GetUser()
        {
            LogManager.GetLogger(typeof(NoAixmDataService)).Trace("Getting user parameters.");

            if (ConfigUtil.ExternalApplication != null)
                return ConfigUtil.ExternalApplicationUserName;

            return OperationContext.Current?.ServiceSecurityContext?.PrimaryIdentity?.Name;
        }
    }

}
