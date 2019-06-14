using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.ServiceModel;
using Aran.Temporality.Common.Entity;
using Aran.Temporality.Common.Entity.Enum;
using Aran.Temporality.Common.Entity.Util;
using Aran.Temporality.Common.Enum;
using Aran.Temporality.Common.Interface;
using Aran.Temporality.Common.Logging;
using Aran.Temporality.Common.Util;
using Aran.Temporality.Common.Util.TypeUtil;
using Aran.Temporality.Internal.Abstract.Repository;
using Aran.Temporality.Internal.Implementation.Repository.NHibernate.Repository;
using Aran.Temporality.Internal.Implementation.Storage.NHibernateStorage;
using Aran.Temporality.Internal.Interface;
using Aran.Temporality.Internal.Interface.Storage;
using Aran.Temporality.Internal.Remote.Util;
using NHibernate.Criterion;
#if TEST
using OperationContext = System.ServiceModel.Web.MockedOperationContext;
#endif

namespace Aran.Temporality.Internal.Implementation.Storage
{
    internal class StorageService
    {
        #region NoAixmDataService

        #region Static properties

        private static INoAixmDataService _noAixmDataService;

        #endregion

        public static INoAixmDataService GetNonDataService()
        {
            return _noAixmDataService ?? (_noAixmDataService = new NoAixmDataService());
        }

        public static bool NeedSetup;

        private static AbstractNHibernateRepository<INHibernateEntity> _hibernateRepository;
        public static AbstractNHibernateRepository<INHibernateEntity> HibernateRepository
        {
            get
            {
                if (_hibernateRepository == null)
                {
                    _hibernateRepository = new PostgreSqlRepository<INHibernateEntity>("postgre", "common");
                    _hibernateRepository.Open(NeedSetup);
                    NeedSetup = false;
                }

                return _hibernateRepository;
            }
        }

        private static IUserStorage _userStorage;
        private static IAccessRightStorage _accessRightStorage;
        private static IStorageStorage _storageStorage;
        private static IPublicSlotStorage _publicSlotStorage;
        private static IPrivateSlotStorage _privateSlotStorage;
        private static IBusinessRulesStorage _rulesStorage;
        private static IProblemReportStorage _problemReportStorage;
        private static IFeatureDependencyConfigurationStorage _featureDependencyStorage;
        private static IConfigurationStorage _configurationStorage;
        private static ILogEntryStorage _logEntryStorage;
        private static IDataSourceTemplateStorage _dataSourceTemplateStorage;
        private static IWorkPackageDataStorage _workPackageDataStorage;
        private static IUserGroupStorage _userGroupStorage;
        private static ISlotValidationOptionStorage _slotValidationOptionStorage;
        private static IUserModuleVersionStorage _userModuleVersionStorage;
        private static IFeatureReportStorage _featureReportStorage;
        private static IFeatureScreenshotStorage _featureScreenshotStorage;
        private static IAimslStorage _aimslStrorage;
        private static INotamStorage _notamStorage;
        private static IEventConsistencyStorage _eventConsistencyStorage;

        public static void PreStart()
        {
            _privateSlotStorage.PreStart();
        }

        public static void Init()
        {
            //set type util
            if (!(TypeUtil.CurrentTypeUtil is AimTypeUtil))
            {
                TypeUtil.CurrentTypeUtil = new AimTypeUtil();
            }
            //set common storages

            if (_featureReportStorage == null)
            {
                _featureReportStorage = new FeatureReportStorage { Repository = HibernateRepository };
            }

            if (_featureScreenshotStorage == null)
            {
                _featureScreenshotStorage = new FeatureScreenshotStorage { Repository = HibernateRepository };
            }

            if (_configurationStorage == null)
            {
                _configurationStorage = new ConfigurationStorage { Repository = HibernateRepository };
            }

            if (_userModuleVersionStorage == null)
            {
                _userModuleVersionStorage = new UserModuleVersionStorage { Repository = HibernateRepository };
            }

            if (_slotValidationOptionStorage == null)
            {
                _slotValidationOptionStorage = new SlotValidationOptionStorage { Repository = HibernateRepository };
            }


            if (_userGroupStorage == null)
            {
                _userGroupStorage = new UserGroupStorage { Repository = HibernateRepository };
            }

            if (_logEntryStorage == null)
            {
                _logEntryStorage = new LogEntryStorage { Repository = HibernateRepository };
            }

            if (_dataSourceTemplateStorage == null)
            {
                _dataSourceTemplateStorage = new DataSourceTemplateStorage { Repository = HibernateRepository };
            }
            if (_workPackageDataStorage == null)
            {
                _workPackageDataStorage = new WorkPackageDataStorage { Repository = HibernateRepository };
            }

            if (_featureDependencyStorage == null)
            {
                _featureDependencyStorage = new FeatureDependencyConfigurationStorage { Repository = HibernateRepository };
            }
            if (_userStorage == null)
            {
                _userStorage = new UserStorage { Repository = HibernateRepository };
            }
            if (_problemReportStorage == null)
            {
                _problemReportStorage = new ProblemReportStorage { Repository = HibernateRepository };
            }
            if (_accessRightStorage == null)
            {
                _accessRightStorage = new AccessRightStorage { Repository = HibernateRepository };
            }
            if (_storageStorage == null)
            {
                _storageStorage = new StorageStorage { Repository = HibernateRepository };
            }
            if (_publicSlotStorage == null)
            {
                _publicSlotStorage = new PublicSlotStorage { Repository = HibernateRepository };
            }
            if (_privateSlotStorage == null)
            {
                _privateSlotStorage = new PrivateSlotStorage { Repository = HibernateRepository };
            }
            if (_rulesStorage == null)
            {
                _rulesStorage = new BusinessRulesStorage { Repository = HibernateRepository };
            }

            if (_aimslStrorage == null)
            {
                _aimslStrorage = new AimslStorage { Repository = HibernateRepository };
            }

            if (_notamStorage == null)
            {
                _notamStorage = new NotamStorage { Repository = HibernateRepository };
            }

            if (_eventConsistencyStorage == null)
            {
                _eventConsistencyStorage = new EventConsistencyStorage { Repository = HibernateRepository };
            }
        }

        #endregion

        #region Access 

        public static IIdentity CurrentIdentity()
        {
            if (OperationContext.Current == null) return null;
            if (OperationContext.Current.ServiceSecurityContext == null) return null;
            return OperationContext.Current.ServiceSecurityContext.PrimaryIdentity;
        }

        public static bool AccessGranted(int workpackageId, int featureTypeId, InternalOperation operation)
        {
            User user = null;
            if (WcfOperationContext.Current != null && WcfOperationContext.Current.Items.ContainsKey("user"))
            {
                user = (User)WcfOperationContext.Current.Items["user"];
            }

            if (user == null) return true;//it is local call, no restrictions

            //get access rights
            IList<AccessRightZipped> accessRights;
            if (WcfOperationContext.Current != null && WcfOperationContext.Current.Items.ContainsKey("access"))
            {
                accessRights = (IList<AccessRightZipped>)WcfOperationContext.Current.Items["access"];
            }
            else
            {
                accessRights = user.UserGroup == null ? new List<AccessRightZipped>() : _accessRightStorage.GetUserRights(user.UserGroup.Id);
                WcfOperationContext.Current.Items["access"] = accessRights;
            }

            var rightsZipped = accessRights.Where(t => t.StorageId == -1 &&
                (t.WorkPackage == -1 || workpackageId == t.WorkPackage)).ToList();

            return rightsZipped.Select(AccessRightUtil.DecodeRights).Any(
                list => list.Any(t => t.MyFeatureTypeId == featureTypeId &&
                                      (t.MyOperationFlag & (int)operation) != 0));
        }

        public static User Login(int userId, string password)
        {
            Init();
            return _userStorage.Login(userId, password);
        }


        #endregion

        #region Users and rights

        public static User GetUserById(int id)
        {
            Init();
            return _userStorage.GetEntityById(id);
        }

        public static bool ChangeMyPassword(int userId, string oldPassword, string newPassword)
        {
            Init();
            return _userStorage.ChangeMyPassword(userId, oldPassword, newPassword);
        }

        public static string GetUserName(int userId)
        {
            Init();
            var user = _userStorage.GetEntityById(userId);
            return user?.Name;
        }

        public static bool IsUserSecured(int userId)
        {
            Init();
            var user = _userStorage.GetEntityById(userId);
            return !string.IsNullOrEmpty(user?.Password);
        }

        public static int CreateUser(string name)
        {
            Init();
            return _userStorage.CreateUser(name);
        }

        public static void DeleteAccessRightsByWorkPackage(int packageId)
        {
            Init();
            _accessRightStorage.DeleteAccessRightsByWorkPackage(packageId);
        }


        public static AccessRightZipped GetDefaultUserRights(int userId)
        {
            Init();
            return _accessRightStorage.GetUserRights(userId, -1, -1);
        }

        public static bool SetUserRights(AccessRightZipped currentAccessRightZipped)
        {
            Init();
            return _accessRightStorage.SetUserRights(currentAccessRightZipped);
        }

        public static IList<AccessRightZipped> GetUserRights(int userId)
        {
            Init();
            return _accessRightStorage.GetUserRights(userId);
        }

        public static bool SetUserActiveSlotId(int userId, int privateSlotId)
        {
            Init();
            if (privateSlotId == 0)
            {
                return _userStorage.SetUserActiveSlot(userId, null);
            }

            var privateSlot = _privateSlotStorage.GetEntityById(privateSlotId);
            return privateSlot != null && _userStorage.SetUserActiveSlot(userId, privateSlot);
        }

        public static IList<User> GetAllUsers()
        {
            Init();
            return _userStorage.GetAllEntities();
        }

        public static bool SetUserModules(int userId, int currentModuleFlag)
        {
            Init();
            return _userStorage.SetUserModules(userId, currentModuleFlag);
        }

        public static bool SetUserRole(int userId, int role)
        {
            Init();
            return _userStorage.SetUserRole(userId, role);
        }

        public static bool SetUserName(int userId, string name)
        {
            Init();
            return _userStorage.SetUserName(userId, name);
        }

        public static bool DeleteUserById(int userId)
        {
            Init();
            return _userStorage.DeleteUserById(userId);
        }

        public static int ResetPasswordById(int userId)
        {
            Init();
            return _userStorage.ResetPasswordById(userId);
        }

        #endregion

        #region Storage

        public static Common.Entity.Storage GetStorageById(int storageId)
        {
            Init();
            return _storageStorage.GetEntityById(storageId);
        }

        public static Common.Entity.Storage GetStorageByName(string storage)
        {
            Init();
            return _storageStorage.GetStorageByName(storage);
        }

        #endregion

        #region Slots

        public static void ResetSlotStatus()
        {
            Init();
            _privateSlotStorage.ResetSlotStatus();
            _publicSlotStorage.ResetSlotStatus();
        }

        public static List<string> GetUsersOfPrivateSlot(int id)
        {
            Init();
            var users = _userStorage.GetUsersOfPrivateSlot(id).ToList();
            return users.Count == 0 ? new List<string>() : users.OrderBy(t => t.Name).Select(t => t.Name).ToList();
        }

        public static IList<PublicSlot> GetPublicSlots()
        {
            Init();
            return _publicSlotStorage.GetAllEntities();
        }

        public static IList<PrivateSlot> GetPrivateSlots(int id, int userId)
        {
            Init();
            return _privateSlotStorage.GetPrivateSlots(id, userId);
        }

        public static int CreatePublicSlot(PublicSlot publicSlot)
        {
            Init();
            return _publicSlotStorage.CreateEntity(publicSlot);
        }

        public static PublicSlot GetPublicSlotById(int id)
        {
            Init();
            return _publicSlotStorage.GetEntityById(id);
        }

        public static bool DeletePublicSlot(int id)
        {
            Init();
            return _publicSlotStorage.DeleteEntityById(id);
        }

        public static bool UpdatePublicSlot(PublicSlot publicSlot)
        {
            Init();
            return _publicSlotStorage.UpdatePublicSlot(publicSlot);
        }

        public static int CreatePrivateSlot(PrivateSlot privateSlot)
        {
            Init();
            return _privateSlotStorage.CreateEntity(privateSlot);
        }

        public static PrivateSlot GetPrivateSlotById(int id)
        {
            Init();
            return _privateSlotStorage.GetEntityById(id);
        }

        public static bool UpdatePrivateSlot(PrivateSlot privateSlot)
        {
            Init();
            return _privateSlotStorage.UpdatePrivateSlot(privateSlot);
        }

        public static bool DeletePrivateSlot(int id)
        {
            Init();
            return _privateSlotStorage.DeleteById(id);
        }

        public static PrivateSlot GetFirstPrivateSlotAndSetStatus(SlotStatus initialStatus, SlotStatus nextStatus)
        {
            Init();
            return _privateSlotStorage.GetFirstAndSetStatus(initialStatus, nextStatus);
        }

        public static PublicSlot GetFirstPublicSlotAndSetStatus(SlotStatus initialStatus, SlotStatus nextStatus)
        {
            Init();
            return _publicSlotStorage.GetFirstAndSetStatus(initialStatus, nextStatus);
        }

        #endregion

        #region Business rules

        public static IList<BusinessRuleUtil> GetBusinessRules()
        {
            Init();
            return _rulesStorage.GetBusinessRules();
        }

        public static void ActivateRule(int ruleId, bool active)
        {
            Init();
            _rulesStorage.ActivateRule(ruleId, active);
        }

        #endregion

        #region ProblemReport

        public static void UpdateProblemReport(ProblemReport problemReport)
        {
            Init();
            _problemReportStorage.UpdateProblemReport(problemReport);
        }

        public static ProblemReport GetProblemReport(int publicSlotId, int privateSlotId, int configId, ReportType reportType)
        {
            Init();
            return _problemReportStorage.GetProblemReport(publicSlotId, privateSlotId, configId, reportType);
        }

        #endregion

        #region FeatureReport

        public static void SaveFeatureReport(FeatureReportZipped zippedReport)
        {
            Init();
            _featureReportStorage.CreateEntity(zippedReport);
        }

        public static IList<FeatureReportZipped> GetFeatureReportsByIdentifier(string identifier)
        {
            Init();
            return _featureReportStorage.GetFeatureReportsByIdentifier(identifier);
        }

        #endregion

        #region AIMSL

        public static int CreateAimslOperation(AimslOperation aimsloperation)
        {
            Init();
            return _aimslStrorage.CreateOperation(aimsloperation);
        }
        public static int CreateAimslOperation(string jobId, string fileName, DateTime creationTime)
        {
            Init();
            return _aimslStrorage.CreateOperation(jobId, fileName, creationTime);
        }

        public static bool AimslAddPullPoint(int id, string pullPoint)
        {
            Init();
            return _aimslStrorage.AddPullPoint(id, pullPoint);
        }

        public static bool AimslAddSubscription(int id, string subscription)
        {
            Init();
            return _aimslStrorage.AddSubscription(id, subscription);
        }

        public static bool AimslAppendMesages(int id, List<string> messages, string status, DateTime lastchangeTime, bool closed)
        {
            Init();
            return _aimslStrorage.AppendMesages(id, messages, status, lastchangeTime, closed);
        }

        public static bool AimslChangeStatus(int id, string status, string description, bool closed)
        {
            Init();
            return _aimslStrorage.ChangeStatus(id, status, description, closed);
        }

        public static bool AimslDestroy(int id, bool timeout = false)
        {
            Init();
            return _aimslStrorage.Destroy(id, timeout);
        }

        public static List<AimslOperation> GetAllAimslOperations()
        {
            Init();
            return _aimslStrorage.GetAllEntities().ToList();
        }

        public static IList<AimslOperation> GetAllActiveAimslOperations()
        {
            Init();
            return _aimslStrorage.GetAllActiveAimslOperations();
        }

        #endregion

        #region Notam

        public static int SaveNotam(Notam notam)
        {
            Init();
            return _notamStorage.SaveNotam(notam);
        }

        public static bool UpdateNotam(Notam notam)
        {
            Init();
            return _notamStorage.UpdateNotam(notam);
        }

        public static Notam GetNotamById(int id)
        {
            Init();
            return _notamStorage.GetEntityById(id);
        }

        public static IList<Notam> GetAllNotams()
        {
            Init();
            return _notamStorage.GetAllEntities();
        }



        #endregion

        #region EventConsistency

        public static int SaveEventConsistency(EventConsistency eventConsistency)
        {
            return _eventConsistencyStorage.Save(eventConsistency);
        }

        public static bool SaveEventConsistencies(List<EventConsistency> eventConsistency)
        {
            return _eventConsistencyStorage.Save(eventConsistency);
        }

        public static string GetConsistency(EventConsistency eventConsistency)
        {
            return _eventConsistencyStorage.Get(eventConsistency);
        }

        public static List<EventConsistency> GetConsistencies(RepositoryType repositoryType, string storageName, int workPackage)
        {
            return _eventConsistencyStorage.Get(repositoryType, storageName, workPackage);
        }

        public static void ClearConsistency(RepositoryType repositoryType, string storageName)
        {
            _eventConsistencyStorage.Clear(repositoryType, storageName);
        }

        #endregion

        #region FeatureScreenshot

        public static void SaveFeatureScreenshot(FeatureScreenshot screenshot)
        {
            Init();
            _featureScreenshotStorage.CreateEntity(screenshot);
        }

        public static IList<FeatureScreenshot> GetFeatureScreenshotsByIdentifier(string identifier)
        {
            Init();
            return _featureScreenshotStorage.GetFeatureScreenshotsByIdentifier(identifier);
        }

        #endregion

        #region Feature Dependency 

        public static IList<FeatureDependencyConfiguration> GetFeatureDependencies()
        {
            Init();
            return _featureDependencyStorage.GetAllEntities().Where(x => x.DataSourceTemplate != null).ToList();
        }

        public static bool DeleteFeatureDependencyConfiguration(int id)
        {
            Init();
            return _featureDependencyStorage.DeleteEntityById(id);
        }

        public static int CreateFeatureDependencyConfiguration(FeatureDependencyConfiguration entity)
        {
            Init();
            return _featureDependencyStorage.CreateEntity(entity);
        }

        public static bool UpdateFeatureDependencyConfiguration(FeatureDependencyConfiguration entity)
        {
            Init();
            return _featureDependencyStorage.UpdateFeatureDependencyConfiguration(entity);
        }

        public static FeatureDependencyConfiguration GetFeatureDependencyById(int id)
        {
            Init();
            return _featureDependencyStorage.GetEntityById(id);
        }

        #endregion

        #region Configuration

        public static Configuration GetConfigurationById(int id)
        {
            Init();
            return _configurationStorage.GetEntityById(id);
        }

        public static Configuration GetConfigurationByName(string name)
        {
            Init();
            return _configurationStorage.GetConfigurationByName(name);
        }

        public static int UpdateConfiguration(Configuration configuration)
        {
            Init();
            return _configurationStorage.UpdateConfiguration(configuration);
        }

        #endregion

        public static IList<Configuration> GetAllConfigurations()
        {
            Init();
            return _configurationStorage.GetAllEntities();
        }

        public static IList<DataSourceTemplate> GetAllDataSourceTemplates()
        {
            Init();
            return _dataSourceTemplateStorage.GetAllEntities();
        }

        public static int CreateDataSourceTemplate(DataSourceTemplate entity)
        {
            Init();
            return _dataSourceTemplateStorage.CreateEntity(entity);
        }

        public static bool DeleteDataSourceTemplate(int id)
        {
            Init();
            _featureDependencyStorage.DeleteFeatureDependenciesByTemplateId(id);
            return _dataSourceTemplateStorage.DeleteEntityById(id);
        }

        public static DataSourceTemplate GetDataSourceTemplateById(int id)
        {
            Init();
            return _dataSourceTemplateStorage.GetEntityById(id);
        }

        public static bool UpdateDataSourceTemplate(DataSourceTemplate entity)
        {
            Init();
            return _dataSourceTemplateStorage.UpdateDataSourceTemplate(entity);
        }

        public static IList<FeatureDependencyConfiguration> GetFeatureDependenciesByTemplate(int templateId)
        {
            Init();
            return _featureDependencyStorage.GetFeatureDependenciesByTemplate(templateId);
        }

        public static int Log(LogEntry entry)
        {
            Init();
            return _logEntryStorage.CreateEntity(entry);
        }


        public static IList<UserGroup> GetAllUserGroups()
        {
            Init();
            return _userGroupStorage.GetAllEntities();
        }

        public static int CreateGroup(string groupName)
        {
            Init();
            return _userGroupStorage.CreateGroup(groupName);
        }

        public static bool DeleteGroupById(int id)
        {
            Init();
            _userStorage.DeleteGroupById(id);
            return _userGroupStorage.DeleteEntityById(id);
        }

        public static bool SetUserGroup(int userId, int groupId)
        {
            Init();
            return _userStorage.SetUserGroup(userId, groupId);
        }

        public static string GetGroupName(int id)
        {
            Init();
            var group = _userGroupStorage.GetEntityById(id);
            return group == null ? "no group" : group.Name;
        }


        public static bool SetGroupName(int id, string currentGroupName)
        {
            Init();
            return _userGroupStorage.SetGroupName(id, currentGroupName);
        }

        public static IList<int> GetLogIds(DateTime fromDate, DateTime toDate, string storageMask, string applicationMask, string userMask, string addressMask, string actionMask, string parameterMask, bool? accessGranted)
        {
            Init();
            return _logEntryStorage.GetLogIds(fromDate, toDate, storageMask, applicationMask, userMask, addressMask, actionMask, parameterMask, accessGranted);
        }


        public static IList<LogEntry> GetLogByIds(List<int> ids)
        {
            Init();
            return _logEntryStorage.GetLogByIds(ids);
        }

        public static IList<object> GetLogValues(string field)
        {
            Init();
            return _logEntryStorage.GetLogValues(field);
        }

        public static SlotValidationOption GetSlotValidationOption(int slotId)
        {
            Init();
            return _slotValidationOptionStorage.GetOptionBySlotId(slotId);
        }

        public static bool UpdateSlotValidationOption(int slotId, SlotValidationOption newOption)
        {
            Init();
            return _slotValidationOptionStorage.UpdateSlotValidationOption(slotId, newOption);
        }

        public static bool UpdateUserModuleVersion(UserModuleVersion userModuleVersion)
        {
            Init();
            return _userModuleVersionStorage.UpdateUserModuleVersion(userModuleVersion);
        }

        public static IList<UserModuleVersion> GetUserModuleVersions(int userId)
        {
            Init();
            return _userModuleVersionStorage.GetUserModuleVersions(userId);
        }
    }
}
