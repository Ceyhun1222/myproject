using Aran.Geometries;
using Aran.Temporality.Common.Abstract.Event;
using Aran.Temporality.Common.Abstract.State;
using Aran.Temporality.Common.Aim.MetaData;
using Aran.Temporality.Common.Config;
using Aran.Temporality.Common.Entity;
using Aran.Temporality.Common.Entity.Enum;
using Aran.Temporality.Common.Entity.Util;
using Aran.Temporality.Common.Id;
using Aran.Temporality.Common.Interface;
using Aran.Temporality.Common.Logging;
using Aran.Temporality.Common.OperationResult;
using Aran.Temporality.Common.Util;
using Aran.Temporality.Internal.Attribute;
using Aran.Temporality.Internal.WorkFlow;
using System;
using System.Collections.Generic;
using Aran.Aim.Data;
using Aran.Aim.Data.Filters;
using Aran.Temporality.Common.Enum;
using Aran.Temporality.Internal.Service.Validation;
using TimeSlice = Aran.Temporality.Common.MetaData.TimeSlice;
using User = Aran.Temporality.Common.Entity.User;

namespace Aran.Temporality.Internal.Service
{
    internal class AimTemporalityServiceProxy : INoAixmDataService, ITemporalityService<AimFeature>
    {
        private readonly AimTemporalityService _aimTemporalityService;
        private readonly SlotValidation _slotValidation;
        private readonly TimeSliceValidation _timeSliceValidation;
        private readonly TemporalityValidation _temporalityValidation;


        public AimTemporalityServiceProxy(String path)
        {
            _aimTemporalityService = new AimTemporalityService(path);
            _slotValidation = new SlotValidation(_aimTemporalityService);
            _timeSliceValidation = new TimeSliceValidation(_aimTemporalityService);
            _temporalityValidation = new TemporalityValidation(_aimTemporalityService);
        }

        #region ITemporalityService


        public void Dispose()
        {
            _aimTemporalityService.Dispose();
        }

        public OperationStatus GetCurrentOperationStatus()
        {
            return _aimTemporalityService.GetCurrentOperationStatus();
        }

        public TemporalityLogicOptions Options
        {
            get => _aimTemporalityService.Options;
            set => _aimTemporalityService.Options = value;
        }

        public string StorageName
        {
            get => _aimTemporalityService.StorageName;
            set => _aimTemporalityService.StorageName = value;
        }

        #region Write operations


        [SecureOperation(DataOperation.WriteData)]
        public CommonOperationResult CancelSequence(TimeSliceId myEvent, Interpretation interpretation, DateTime? cancelDate = null)
        {
            _slotValidation.PrivateSlotWriteValidation(myEvent);
            _temporalityValidation.IsCancelValid(myEvent);
            return _aimTemporalityService.CancelSequence(myEvent, interpretation, cancelDate);
        }

        //[SecureOperation(DataOperation.WriteData)]
        //public CommonOperationResult CommitEvent(AbstractEvent<AimFeature> myEvent)
        //{
        //    _slotValidation.PrivateSlotWriteValidation(myEvent);
        //    _timeSliceValidation.IsTimeSliceCorrect(myEvent);
        //    return _aimTemporalityService.CommitEvent(myEvent);
        //}

        [SecureOperation(DataOperation.WriteData)]
        public FeatureOperationResult CommitNewEvent(AbstractEvent<AimFeature> myEvent)
        {
            _slotValidation.PrivateSlotWriteValidation(myEvent);
            _timeSliceValidation.IsTimeSliceCorrect(myEvent);
            _temporalityValidation.IsNewEventValid(myEvent);
            return _aimTemporalityService.CommitNewEvent(myEvent);
        }

        [SecureOperation(DataOperation.WriteData)]
        public FeatureOperationResult CommitCorrection(AbstractEvent<AimFeature> myEvent)
        {
            _slotValidation.PrivateSlotWriteValidation(myEvent);
            _timeSliceValidation.IsTimeSliceCorrect(myEvent);
            _temporalityValidation.IsCorrectionValid(myEvent);
            return _aimTemporalityService.CommitCorrection(myEvent);
        }


        [SecureOperation(DataOperation.WriteData)]
        public bool Decommission(IFeatureId featureId, DateTime airacDate)
        {
            _slotValidation.PrivateSlotWriteValidation(featureId);
            _temporalityValidation.IsDecomissionValid(featureId);
            return _aimTemporalityService.Decommission(featureId, airacDate);
        }

        #endregion

        #region Read operations

        [SecureOperation(DataOperation.ReadData)]
        public TimeSliceVersion CreateNewVersion(IFeatureId featureId, Interpretation interpretation)
        {
            return _aimTemporalityService.CreateNewVersion(featureId, interpretation);
        }

        public DateTime GetServerTime()
        {
            return _aimTemporalityService.GetServerTime();
        }

        public SystemType GetServerType()
        {
            return _aimTemporalityService.GetServerType();
        }

        [SecureOperation(DataOperation.ReadData)]
        public AbstractEvent<AimFeature> GetEvent(IFeatureId featureId, TimeSliceVersion version)
        {
            return _aimTemporalityService.GetEvent(featureId, version);
        }

        [SecureOperation(DataOperation.ReadData)]
        public IList<AbstractEventMetaData> GetActualEventMeta(IFeatureId featureId, TimeSlice impactInterval, TimeSlice submitInterval)
        {
            return _aimTemporalityService.GetActualEventMeta(featureId, impactInterval, submitInterval);
        }

        [SecureOperation(DataOperation.ReadData)]
        public IList<AbstractEventMetaData> GetCancelledEventMeta(IFeatureId featureId, TimeSlice impactInterval, TimeSlice submitInterval)
        {
            return _aimTemporalityService.GetCancelledEventMeta(featureId, impactInterval, submitInterval);
        }


        [SecureOperation(DataOperation.ReadData)]
        public IList<AbstractState<AimFeature>> GetActualDataByDate(IFeatureId featureId, bool slotOnly, DateTime dateTime, Interpretation interpretation = Interpretation.Snapshot,
            DateTime? currentDate = null, Filter filter = null, Projection projection = null)
        {
            return _aimTemporalityService.GetActualDataByDate(featureId, slotOnly, dateTime, interpretation, currentDate, filter, projection);
        }

        [SecureOperation(DataOperation.ReadData)]
        public IList<AbstractEvent<AimFeature>> GetEventsByDate(IFeatureId featureId, bool slotOnly, DateTime fromDateTime, DateTime toDateTime,
            Interpretation? interpretation = null, DateTime? currentDate = null)
        {
            return _aimTemporalityService.GetEventsByDate(featureId, slotOnly, fromDateTime, toDateTime, interpretation, currentDate);
        }


        [SecureOperation(DataOperation.ReadData)]
        public IList<AbstractState<AimFeature>> GetActualDataByGeo(IFeatureId mask, bool b, DateTime effectiveDate, Geometry geo, double distance)
        {
            return _aimTemporalityService.GetActualDataByGeo(mask, b, effectiveDate, geo, distance);
        }

        [SecureOperation(DataOperation.ReadData)]
        public IList<AbstractState<AimFeature>> GetStatesInRange(IFeatureId featureId, bool slotOnly, DateTime dateTimeStart, DateTime dateTimeEnd)
        {
            return _aimTemporalityService.GetStatesInRange(featureId, slotOnly, dateTimeStart, dateTimeEnd);
        }

        [SecureOperation(DataOperation.ReadData)]
        public IList<AbstractState<AimFeature>> GetStatesInRangeByInterpretation(IFeatureId featureId, bool slotOnly, DateTime dateTimeStart,
            DateTime dateTimeEnd, Interpretation interpretation)
        {
            return _aimTemporalityService.GetStatesInRangeByInterpretation(featureId, slotOnly, dateTimeStart, dateTimeEnd, interpretation);
        }


        public IList<AbstractEvent<AimFeature>> GetEvolution(IFeatureId featureId)
        {
            return _aimTemporalityService.GetEvolution(featureId);
        }

        public IList<AbstractEvent<AimFeature>> GetChangesInInterval(IFeatureId featureId, DateTime startDate, DateTime endDate, bool slotOnly)
        {
            return _aimTemporalityService.GetChangesInInterval(featureId, startDate, endDate, slotOnly);
        }

        public IList<AbstractEventMetaData> GetChangesMetaInInterval(IFeatureId featureIdParam, DateTime startDateParam, DateTime endDateParam,
            bool slotOnlyParam)
        {
            return _aimTemporalityService.GetChangesMetaInInterval(featureIdParam, startDateParam, endDateParam, slotOnlyParam);
        }


        public StateWithDelta<AimFeature> GetActualDataForEditing(IFeatureId featureId, DateTime actualDate, Interpretation interpretation = Interpretation.Snapshot, DateTime? endDate = null)
        {
            return _aimTemporalityService.GetActualDataForEditing(featureId, actualDate, interpretation, endDate);
        }

        public int GetFeatureTypeById(Guid id)
        {
            return _aimTemporalityService.GetFeatureTypeById(id);
        }

        #endregion

        #region Slot operations
        public void PublishPublicSlot(PublicSlot publicSlot)
        {
            _aimTemporalityService.PublishPublicSlot(publicSlot);
        }

        public IList<int> GetPrivateSlotFeatureTypes(int slotId)
        {
            return _aimTemporalityService.GetPrivateSlotFeatureTypes(slotId);
        }

        public bool DeletePrivateSlot(int id)
        {
            return _aimTemporalityService.DeletePrivateSlot(id);
        }

        public List<string> GetUsersOfPrivateSlot(int id)
        {
            return _aimTemporalityService.GetUsersOfPrivateSlot(id);
        }
        #endregion

        #region Server operations

        public void GetConfigurationData(int id)
        {
            _aimTemporalityService.GetConfigurationData(id);
        }

        #endregion

        #region Optimization

        [SecureOperation(StorageOperation.TruncateStorage)]
        public void Truncate()
        {
            _aimTemporalityService.Truncate();
        }


        public bool Optimize()
        {
            return _aimTemporalityService.Optimize();
        }

        #endregion


        #endregion

        #region INoAixmDataService

        #region Logged operations

        [LogOperation(Action = LogActions.Login)]
        public User Login(int userId, string password)
        {
            return _aimTemporalityService.Login(userId, password);
        }

        [LogOperation(Action = LogActions.CreateUser, Arguments = new[] { 0 })]
        public int CreateUser(string name)
        {
            return _aimTemporalityService.CreateUser(name);
        }

        [LogOperation(Action = LogActions.ResetPassword, Arguments = new[] { 1 })]
        public int ResetPassword(int userId, string userName)
        {
            return _aimTemporalityService.ResetPassword(userId, userName);
        }

        [LogOperation(Action = LogActions.ChangeRole, Arguments = new[] { 2, 3 })]
        public bool SetRole(int userId, int role, string userName, string roleName)
        {
            return _aimTemporalityService.SetUserRole(userId, role);
        }

        [LogOperation(Action = LogActions.DeleteUser, Arguments = new[] { 1 })]
        public bool DeleteUser(int userId, string userName)
        {
            return _aimTemporalityService.DeleteUser(userId, userName);
        }

        [LogOperation(Action = LogActions.ChangeModuleAccessRights, Arguments = new[] { 2, 3 })]
        public bool SetModules(int id, int currentModuleFlag, string userName, string moduleName)
        {
            return _aimTemporalityService.SetUserModules(id, currentModuleFlag);
        }

        [LogOperation(Action = LogActions.EditUser, Arguments = new[] { 0, 1 })]
        public bool SetUserName(int userId, string name)
        {
            return _aimTemporalityService.SetUserName(userId, name);
        }

        [LogOperation(Action = LogActions.ChangeAccessRights, Arguments = new[] { 1 })]
        public bool SetRights(AccessRightZipped currentAccessRightZipped, string groupName)
        {
            return _aimTemporalityService.SetUserRights(currentAccessRightZipped);
        }

        #endregion

        public int ResetPasswordById(int userId)
        {
            return _aimTemporalityService.ResetPasswordById(userId);
        }

        public bool DeleteUserById(int userId)
        {
            return _aimTemporalityService.DeleteUserById(userId);
        }




        public bool SetUserRole(int userId, int role)
        {
            return _aimTemporalityService.SetUserRole(userId, role);
        }

        public bool SetUserModules(int id, int currentModuleFlag)
        {
            return _aimTemporalityService.SetUserModules(id, currentModuleFlag);
        }

        public IList<User> GetAllUsers()
        {
            return _aimTemporalityService.GetAllUsers();
        }

        public bool SetUserActiveSlotId(int userId, int privateSlotId)
        {
            return _aimTemporalityService.SetUserActiveSlotId(userId, privateSlotId);
        }

        public bool ChangeMyPassword(int userId, string oldPassword, string newPassword)
        {
            return _aimTemporalityService.ChangeMyPassword(userId, oldPassword, newPassword);
        }

        public bool SetUserRights(AccessRightZipped currentAccessRightZipped)
        {
            return _aimTemporalityService.SetUserRights(currentAccessRightZipped);
        }

        public AccessRightZipped GetDefaultUserRights(int userId)
        {
            return _aimTemporalityService.GetDefaultUserRights(userId);
        }

        public IList<AccessRightZipped> GetUserRights(int userId)
        {
            return _aimTemporalityService.GetUserRights(userId);
        }

        public void DeleteAccessRightsByWorkPackage(int packageId)
        {
            _aimTemporalityService.DeleteAccessRightsByWorkPackage(packageId);
        }

        public Storage GetStorageByName(string storage)
        {
            return _aimTemporalityService.GetStorageByName(storage);
        }

        public Storage GetStorageById(int storageId)
        {
            return _aimTemporalityService.GetStorageById(storageId);
        }

        public IList<PublicSlot> GetPublicSlots()
        {
            return _aimTemporalityService.GetPublicSlots();
        }

        public IList<PrivateSlot> GetPrivateSlots(int publicSlotId, int userId)
        {
            return _aimTemporalityService.GetPrivateSlots(publicSlotId, userId);
        }

        public int CreatePublicSlot(PublicSlot publicSlot)
        {
            return _aimTemporalityService.CreatePublicSlot(publicSlot);
        }

        public PublicSlot GetPublicSlotById(int id)
        {
            return _aimTemporalityService.GetPublicSlotById(id);
        }

        public bool DeletePublicSlot(int id)
        {
            return _aimTemporalityService.DeletePublicSlot(id);
        }

        public bool UpdatePublicSlot(PublicSlot publicSlot)
        {
            return _aimTemporalityService.UpdatePublicSlot(publicSlot);
        }

        public int CreatePrivateSlot(PrivateSlot privateSlot)
        {
            return _aimTemporalityService.CreatePrivateSlot(privateSlot);
        }

        public PrivateSlot GetPrivateSlotById(int id)
        {
            return _aimTemporalityService.GetPrivateSlotById(id);
        }

    
        public bool UpdatePrivateSlot(PrivateSlot privateSlot)
        {
            return _aimTemporalityService.UpdatePrivateSlot(privateSlot);
        }

        public IList<BusinessRuleUtil> GetBusinessRules()
        {
            return _aimTemporalityService.GetBusinessRules();
        }

        public void ActivateRule(int ruleId, bool active)
        {
            _aimTemporalityService.ActivateRule(ruleId, active);
        }

        public void UpdateProblemReport(ProblemReport problemReport)
        {
            _aimTemporalityService.UpdateProblemReport(problemReport);
        }

        public ProblemReport GetProblemReport(int publicSlotId, int privateSlotId, int configId, ReportType reportType)
        {
            return _aimTemporalityService.GetProblemReport(publicSlotId, privateSlotId, configId, reportType);
        }

        public IList<Notam> GetAllNotams()
        {
            return _aimTemporalityService.GetAllNotams();
        }

        public void SaveFeatureReport(FeatureReportZipped zippedReport)
        {
            _aimTemporalityService.SaveFeatureReport(zippedReport);
        }

        public IList<FeatureReportZipped> GetFeatureReportsByIdentifier(string identifier)
        {
            return _aimTemporalityService.GetFeatureReportsByIdentifier(identifier);
        }

        public void SaveFeatureScreenshot(FeatureScreenshot screenshot)
        {
            _aimTemporalityService.SaveFeatureScreenshot(screenshot);
        }

        public IList<FeatureScreenshot> GetFeatureScreenshotsByIdentifier(string identifier)
        {
            return _aimTemporalityService.GetFeatureScreenshotsByIdentifier(identifier);
        }

        public IList<FeatureDependencyConfiguration> GetFeatureDependencies()
        {
            return _aimTemporalityService.GetFeatureDependencies();
        }

        public bool DeleteFeatureDependencyConfiguration(int id)
        {
            return _aimTemporalityService.DeleteFeatureDependencyConfiguration(id);
        }

        public int CreateFeatureDependencyConfiguration(FeatureDependencyConfiguration entity)
        {
            return _aimTemporalityService.CreateFeatureDependencyConfiguration(entity);
        }

        public bool UpdateFeatureDependencyConfiguration(FeatureDependencyConfiguration entity)
        {
            return _aimTemporalityService.UpdateFeatureDependencyConfiguration(entity);
        }

        public int UpdateConfiguration(Configuration configuration)
        {
            return _aimTemporalityService.UpdateConfiguration(configuration);
        }

        public IList<Configuration> GetAllConfigurations()
        {
            return _aimTemporalityService.GetAllConfigurations();
        }

        public Configuration GetConfigurationByName(string selectedConfiguration)
        {
            return _aimTemporalityService.GetConfigurationByName(selectedConfiguration);
        }

        public bool LogConfigured()
        {
            return _aimTemporalityService.LogConfigured();
        }

        public bool SetLogLevel(LogLevel level)
        {
            return _aimTemporalityService.SetLogLevel(level);
        }

        public LogLevel GetLogLevel()
        {
            return _aimTemporalityService.GetLogLevel();
        }

        public IList<DataSourceTemplate> GetAllDataSourceTemplates()
        {
            return _aimTemporalityService.GetAllDataSourceTemplates();
        }

        public int CreateDataSourceTemplate(DataSourceTemplate entity)
        {
            return _aimTemporalityService.CreateDataSourceTemplate(entity);
        }

        public bool DeleteDataSourceTemplate(int id)
        {
            return _aimTemporalityService.DeleteDataSourceTemplate(id);
        }

        public DataSourceTemplate GetDataSourceTemplateById(int id)
        {
            return _aimTemporalityService.GetDataSourceTemplateById(id);
        }

        public bool UpdateDataSourceTemplate(DataSourceTemplate entity)
        {
            return _aimTemporalityService.UpdateDataSourceTemplate(entity);
        }

        public IList<FeatureDependencyConfiguration> GetFeatureDependenciesByTemplate(int templateId)
        {
            return _aimTemporalityService.GetFeatureDependenciesByTemplate(templateId);
        }

        public IList<UserGroup> GetAllUserGroups()
        {
            return _aimTemporalityService.GetAllUserGroups();
        }

        public int CreateGroup(string groupName)
        {
            return _aimTemporalityService.CreateGroup(groupName);
        }

        public bool DeleteGroupById(int id)
        {
            return _aimTemporalityService.DeleteGroupById(id);
        }

        public bool SetUserGroup(int userId, int groupId)
        {
            return _aimTemporalityService.SetUserGroup(userId, groupId);
        }

        public bool SetGroupName(int id, string currentGroupName)
        {
            return _aimTemporalityService.SetGroupName(id, currentGroupName);
        }

        public IList<int> GetLogIds(DateTime fromDate, DateTime toDate, string storageMask, string applicationMask, string userMask,
            string addressMask, string actionMask, string parameterMask, bool? accessGranted)
        {
            return _aimTemporalityService.GetLogIds(fromDate, toDate, storageMask, applicationMask, userMask, addressMask, actionMask, parameterMask, accessGranted);
        }

        public IList<LogEntry> GetLogByIds(List<int> ids)
        {
            return _aimTemporalityService.GetLogByIds(ids);
        }

        public IList<object> GetLogValues(string field)
        {
            return _aimTemporalityService.GetLogValues(field);
        }

        public SlotValidationOption GetSlotValidationOption(int slotId)
        {
            return _aimTemporalityService.GetSlotValidationOption(slotId);
        }

        public bool UpdateSlotValidationOption(int slotId, SlotValidationOption newOption)
        {
            return _aimTemporalityService.UpdateSlotValidationOption(slotId, newOption);
        }

        public IList<UserModuleVersion> GetUserModuleVersions(int userId)
        {
            return _aimTemporalityService.GetUserModuleVersions(userId);
        }

        public byte[] GetUpdate(string aplicationVersion)
        {
            return _aimTemporalityService.GetUpdate(aplicationVersion);
        }

        public string GetUpdateVersion(string s)
        {
            return _aimTemporalityService.GetUpdateVersion(s);
        }

        public string GetUserName(int userId)
        {
            return _aimTemporalityService.GetUserName(userId);
        }

        public bool IsUserSecured(int userId)
        {
            return _aimTemporalityService.IsUserSecured(userId);
        }

        public AimslOperation AimslUploadAixmFile(byte[] zippedfile, string fileName)
        {
            return _aimTemporalityService.AimslUploadAixmFile(zippedfile, fileName);
        }

        public List<AimslOperation> AimslGetAllAimslOperations()
        {
            return _aimTemporalityService.AimslGetAllAimslOperations();
        }

        public bool AimslTestConnection()
        {
            return _aimTemporalityService.AimslTestConnection();
        }

        public int SaveNotam(Notam notam)
        {
            return _aimTemporalityService.SaveNotam(notam);
        }


        public bool UpdateNotam(Notam notam)
        {
            return _aimTemporalityService.UpdateNotam(notam);
        }

        public Notam GetNotamById(int id)
        {
            return _aimTemporalityService.GetNotamById(id);
        }

        #endregion


    }
}