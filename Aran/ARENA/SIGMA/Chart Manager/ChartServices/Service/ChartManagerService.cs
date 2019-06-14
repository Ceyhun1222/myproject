using ChartServices.DataContract;
using ChartServices.Helpers;
using ChartServices.Repositories;
using ChartServices.Services_Contract;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Xml;
using ArenaStatic;
using ChartServices.Logging;
using AutoMapper;
using ChartManagerServices.Helpers;
using NHibernate.Criterion;

namespace ChartServices.Service
{
    [MainErrorHandlerBehavior(typeof(MainErrorHandler))]
    [AutomapServiceBehavior]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
    public class ChartManagerService : IChartManagerService
    {
        private readonly ILogger _logger;
        private readonly RepositoryContext _dbContext;
        private string _previewFileName = "preview.jpg";
        private string _sourceFileName = "source.zip";
        private string _updateFolderName = "UpdateData";

        public ChartManagerService()
        {
            _logger = LogManager.GetLogger(this);
            _dbContext = new RepositoryContext();
        }

        public void Login()
        {          
        }
        
        public IList<ChartUser> GetAllUser()
        {
            ChartUser currentUser = TryGetCurrentUser();           
            //if (currentUser.IsAdmin)
                return _dbContext.Repository<ChartUser>().GetAll().ToList();
        }

        public IList<string> GetOrganizations()
        {
            ChartUser currentUser = TryGetCurrentUser();
            return _dbContext.Repository<Chart>().GetAll().Select(k => k.Organization).Distinct().ToList();
        }

        public IList<string> GetAerodromes(string organization)
        {
            var lst = _dbContext.Repository<Chart>().GetAll();
            if (!string.IsNullOrEmpty(organization))
                lst = lst.Where(chart => chart.Organization == organization);
            return lst.Select(t => t.Airport).Distinct().ToList();
        }

        public IList<string> GetRunways(string aerodrome)
        {
            var lst = _dbContext.Repository<Chart>().GetAll();
            if (!string.IsNullOrEmpty(aerodrome))
                lst = lst.Where(chart => chart.Airport == aerodrome);
            return lst.Select(t => t.RunwayDirection).Distinct().ToList();
        }

        public ChartUser GetUser(long id)
        {
            ChartUser currentUser = TryGetCurrentUser();           
            if (!currentUser.IsAdmin && currentUser.Id != id)
                throw new FaultException("You don't have privileges to get another user's data");

            return _dbContext.Repository<ChartUser>().GetById(id);
        }

        public ChartUser TryGetCurrentUser()
        {
            ChartUser currentUser = null;
            if (WcfOperationContext.Current != null && WcfOperationContext.Current.Items.ContainsKey("user"))
                currentUser = (ChartUser)WcfOperationContext.Current.Items["user"];

            if (currentUser == null)
                throw new FaultException("User not found in database");

            if (currentUser.Disabled)
                throw new FaultException("Your account is disabled");

            return currentUser;
        }

        public void DeleteUser(long id)
        {
            ChartUser currentUser = TryGetCurrentUser();                     
            if (!currentUser.IsAdmin)
                throw new FaultException("You don't have privileges to delete user's data");

            ChartUser user = _dbContext.Repository<ChartUser>().GetById(id);

            var sessionsByUser = SessionStorage.Sessions.Where(s => s.Key.UserName == user.UserName);

            _dbContext.Repository<ChartUser>().Delete(user);

            foreach (var session in sessionsByUser)
            {
                IChartManagerServiceCallback callback = SessionStorage.Sessions[session.Key];
                callback.UserChanged(UserCallbackType.Deleted);
            }
        }

        public void CreateUser(ChartUser user)
        {
            ChartUser currentUser = TryGetCurrentUser();           
            if (!currentUser.IsAdmin)
                throw new FaultException("You don't have privileges to create user data");

            if(user.UserName.Equals(""))
                throw new FaultException("Username is null");

            ChartUser checkUsername = _dbContext.Repository<ChartUser>()
                .GetAll()
                .FirstOrDefault(u => u.UserName == user.UserName);
            if (checkUsername != null)
                throw new FaultException("Username is already exist");

            if (!user.Email.Equals(""))
            {
                ChartUser checkEmail = _dbContext.Repository<ChartUser>()
                    .GetAll()
                    .FirstOrDefault(u => u.Email == user.Email);
                if (checkEmail != null)
                    throw new FaultException("Email is already exist");
            }

            user.LatestLoginAt = DateTime.Now;
            user.Password = HelperMethods.Sha256_hash(user.Password);

            _dbContext.Repository<ChartUser>().Create(user);
        }

        public bool ExistsUser(string username, string password)
        {
            ChartUser currentUser = TryGetCurrentUser();
            if (currentUser.IsAdmin)
                return _dbContext.Repository<ChartUser>().GetAll()
                    .Any(t => t.UserName == username && t.Password == password);
            throw new FaultException(@"You don't have privileges to call ""ExistsUser"" method");
        }

        public void UpdateUser(ChartUser user)
        {
            ChartUser currentUser = TryGetCurrentUser();
            if (!currentUser.IsAdmin && currentUser.Id != user.Id)
                throw new FaultException("You don't have privileges to update another user's data");

            var userForUpdate = GetUser(user.Id);
            if (userForUpdate == null)
                throw new FaultException("User not found");

            if (userForUpdate.UserName != user.UserName)
            {
                ChartUser checkUsername = _dbContext.Repository<ChartUser>()
                    .GetAll()
                    .FirstOrDefault(u => u.UserName == user.UserName);
                if (checkUsername != null)
                    throw new FaultException("Username is already exist");
                userForUpdate.UserName = user.UserName;
            }

            if (userForUpdate.Email != user.Email)
            {
                ChartUser checkEmail = _dbContext.Repository<ChartUser>()
                    .GetAll()
                    .FirstOrDefault(u => u.Email == user.Email);
                if (checkEmail != null)
                    throw new FaultException("Email is already exist");
                userForUpdate.Email = user.Email;
            }

            userForUpdate.FirstName = user.FirstName;
            userForUpdate.LastName = user.LastName;
            userForUpdate.Position = user.Position;
            userForUpdate.Privilege = user.Privilege;
            userForUpdate.Disabled = user.Disabled;
            userForUpdate.IsAdmin = user.IsAdmin;

            user.LatestLoginAt = DateTime.Now;
            _dbContext.Repository<ChartUser>().Update(userForUpdate);

            var sessionsByUser = SessionStorage.Sessions.Where(s => s.Key.UserName == user.UserName);

            foreach (var session in sessionsByUser)
            {
                IChartManagerServiceCallback callback = SessionStorage.Sessions[session.Key];
                callback.UserChanged(UserCallbackType.Updated);
            }
        }

        public void DisableUser(long id, bool disabled)
        {
            ChartUser currentUser = TryGetCurrentUser();
            ChartUser user = _dbContext.Repository<ChartUser>().GetById(id);

            if (!currentUser.IsAdmin && currentUser.Id != user.Id)
                throw new FaultException("You don't have privileges to update another user's data");

            user.Disabled = disabled;
            _dbContext.Repository<ChartUser>().Update(user);

            var sessionsByUser = SessionStorage.Sessions.Where(s => s.Key.UserName == user.UserName);
            var callbackType = disabled ? UserCallbackType.Disabled : UserCallbackType.Enabled;

            foreach (var session in sessionsByUser)
            {
                IChartManagerServiceCallback callback = SessionStorage.Sessions[session.Key];
                callback.UserChanged(callbackType);
            }
        }

        public void ChangePassword(long id, string oldPassword, string newPassword)
        {
            ChartUser currentUser = TryGetCurrentUser();
            ChartUser user = _dbContext.Repository<ChartUser>().GetById(id);

            if (currentUser.Id == user.Id && !currentUser.IsAdmin)
            {
                if (user.Password != HelperMethods.Sha256_hash(oldPassword))
                    throw new FaultException("Old password is incorrect");

                user.Password = HelperMethods.Sha256_hash(newPassword);
                _dbContext.Repository<ChartUser>().Update(user);
            }
            else
            {
                if (!currentUser.IsAdmin)
                    throw new FaultException("You don't have privileges to change another user's data");

                user.Password = HelperMethods.Sha256_hash(newPassword);
                _dbContext.Repository<ChartUser>().Update(user);

                var sessionsByUser = SessionStorage.Sessions.Where(s => s.Key.UserName == user.UserName);

                foreach (var session in sessionsByUser)
                {
                    IChartManagerServiceCallback callback = SessionStorage.Sessions[session.Key];
                    callback.UserChanged(UserCallbackType.ChangedPassword);
                }

            }
        }

        public byte[] GetConfigFile(long id)
        {
            ChartUser currentUser = TryGetCurrentUser();
            var count = SessionStorage.Sessions.Count;
          
            if (!currentUser.IsAdmin)
                throw new FaultException("You don't have privileges to get config data");

            ChartUser user = _dbContext.Repository<ChartUser>().GetById(id);

            var doc = new XmlDocument();
            doc.LoadXml(
                "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                "<Config>" +
                "</Config>");
            doc.PreserveWhitespace = true;
            
            XmlElement elem = doc.DocumentElement;

            var userName = doc.CreateElement("Username");
            userName.InnerText = user.UserName;
            elem.AppendChild(userName);

            var pass = doc.CreateElement("Password");
            pass.InnerText = user.Password;
            elem.AppendChild(pass);

            //var addressEthernet = HelperMethods.GetAllLocalIPv4(System.Net.NetworkInformation.NetworkInterfaceType.Ethernet);
            //var addressWifi = HelperMethods.GetAllLocalIPv4(System.Net.NetworkInformation.NetworkInterfaceType.Wireless80211);

            var addressTcp = doc.CreateElement("AddressTCP");
            addressTcp.InnerText = "net.tcp://" + HelperMethods.GetLocalIpAddress() + Common.TcpPort + "/ChartServices";
            elem.AppendChild(addressTcp);

            var addressHttp = doc.CreateElement("AddressHTTP");
            addressHttp.InnerText = "http://" + HelperMethods.GetLocalIpAddress() + Common.HttpPort+ "/ChartServices";
            elem.AppendChild(addressHttp);

            return Encoding.Default.GetBytes(HelperMethods.Beautify(doc.OuterXml));
        }

        public Chart GetLatestChartVersion(Guid identifier, DateTime dateTime)
        {
            ChartUser currentUser = TryGetCurrentUser();
            var chart = _dbContext.Repository<Chart>().GetAll()
                .Where(d => d.Identifier == identifier && d.BeginEffectiveDate == dateTime).OrderByDescending(c => c.Version)
                .FirstOrDefault();
            //chart.ReferenceIdList?.Clear();
            return chart;
        }
        
        public byte[] GetPreviewOf(long id)
        {
            ChartUser currentUser = TryGetCurrentUser();
            Chart chart = _dbContext.Repository<Chart>().GetById(id);

            if (chart == null)
            {
                throw new NullReferenceException("Chart is null");
            }
            //chartData.Source = null;
            var chartPath = Path.Combine(_dbContext.FolderPath, id.ToString());
            string previewPath = Path.Combine(chartPath, _previewFileName);
            if (File.Exists(previewPath))
                return File.ReadAllBytes(previewPath);
            throw new NullReferenceException("Chart Preview is not exists");
        }

        public byte[] GetUpdateSource(long id)
        {
            _logger.InfoWithMemberName($"Started ({id})");
            ChartUser currentUser = TryGetCurrentUser();
            ChartUpdateData chartUpdate;
            if (id == default(long))
                chartUpdate = _dbContext.Repository<ChartUpdateData>().GetAll().Where(t => t.References.Any(k => !k.Done)).FirstOrDefault();
            else
                chartUpdate = _dbContext.Repository<ChartUpdateData>().GetById(id);

            if (chartUpdate == null)
                throw new NullReferenceException("Chart update is deleted");

            var updatePath = Path.Combine(_dbContext.FolderPath, _updateFolderName, chartUpdate.Id.ToString());
            if (!Directory.Exists(updatePath))
                throw new FaultException("Directory of update data  is deleted");
            string zipFileName = Path.Combine(updatePath, _sourceFileName);

            if (File.Exists(zipFileName))
                return File.ReadAllBytes(zipFileName);
            throw new NullReferenceException("Source of update data is deleted");
        }

        public byte[] GetSourceOf(long id, bool locked = false)
        {
            ChartUser currentUser = TryGetCurrentUser();
            var chart = _dbContext.Repository<ChartWithReference>().GetById(id);

            if (chart == null)
            {
                throw new NullReferenceException("Chart is null");
            }
            if (locked)
                LockChart(id, true);

            var chartPath = Path.Combine(_dbContext.FolderPath, id.ToString());
            string previewPath = Path.Combine(chartPath, _sourceFileName);
            if (File.Exists(previewPath))
                return File.ReadAllBytes(previewPath);
            throw new NullReferenceException("Chart source is not exists");
        }

        public long UploadUpdateData(ChartUpdateData updateData, byte[] source, IList<Guid> chartIdentifierList)
        {
            ChartUser currentUser = TryGetCurrentUser();
            if (currentUser.Privilege != UserPrivilege.Full)
                throw new FaultException("User does not have the necessary privileges");

            updateData.CreatedAt = DateTime.Now;
            updateData.CreatedBy = currentUser;
            updateData.References = new List<ChartUpdateReference>();

            var existingUpdateList = _dbContext.Repository<ChartUpdateData>().GetAll().Where(t => t.EffectiveDate == updateData.EffectiveDate).ToList(); //&& t.References.Any(k => k.RefId.Equals(item)));
            int maxIndex;
            foreach (var item in chartIdentifierList)
            {
                var refUpdates = existingUpdateList.SelectMany(k => k.References.Where(b => b.RefId == item));
                maxIndex = 0;
                if (refUpdates.Any())
                {
                    maxIndex = refUpdates.Max(k => k.Index); 
                    //existingUpdateList.Where(t=>t.References.Any(b=>b.RefId == item)).Select(c=>c.References).Max(k => k.References.Where(b => b.RefId == item).Max(t => t.Index));
                }
                updateData.References.Add(new ChartUpdateReference() { RefId = item, Index = ++maxIndex });
            }
            _dbContext.Repository<ChartUpdateData>().Create(updateData);


            //var chartUpdateData = new ChartUpdateData()
            //{
            //    Source = source,
            //    MetadataId = result.Id
            //};
            //_dbContext.Repository<ChartUpdateData>().Create(chartUpdateData);
            //foreach (var id in chartIdList)
            //{
            //    var chart = _dbContext.Repository<ChartWithReference>().GetById(id);
            //    chart.HasUpdate = true;
            //    _dbContext.Repository<ChartWithReference>().Update(chart);

            //    var maxIndex = _dbContext.Repository<ChartUpdateReference>().GetAll().Where(t => t.ChartId == id).ToList().Max(k => k.Index);
            //    if(maxIndex>=0)
            //    {
            //        var updateRef = new ChartUpdateReference()
            //        {
            //            ChartId = id,
            //            Index = ++maxIndex,
            //            MetadataId = result.Id
            //        };
            //        _dbContext.Repository<ChartUpdateReference>().Create(updateRef);
            //    }
            //}
            SaveChartUpdateDataFiles(updateData.Id, source);
            //AddUpdateReference2Chart(chartIdList, result);
            return updateData.Id;
        }

        //private void AddUpdateReference2Chart(IList<long> chartIdList, ChartUpdateMetadata updateData)
        //{
        //    var result = _dbContext.Repository<Chart>().GetAll().Where(t => chartIdList.Contains(t.Id)).ToList();
        //    result.ForEach(k => {
        //        k.HasUpdate = true;
        //        var maxIndex = k.UpdateList.Max(t=>t.Index);
        //        maxIndex++;
        //        k.UpdateList.Add(new ReferenceUpdateData() { Index = maxIndex, Data = updateData });
        //        _dbContext.Repository<Chart>().Update(k);
        //    });
        //}

        public void Upload(ChartWithReference chartWithReference, byte[] preview, byte[] source)
        {
            UploadWithUpdate(chartWithReference, preview, source, 0);
        }

        public void UploadWithUpdate(ChartWithReference chartWithReference, byte[] preview, byte[] source, long updateDataId)
        {
            ChartUser currentUser = TryGetCurrentUser();
            if (currentUser.Privilege != UserPrivilege.Full)
                throw new FaultException("User does not have the necessary privileges");

            var previousCharts = _dbContext.Repository<ChartWithReference>()
                .GetAll().Where(c => c.Identifier == chartWithReference.Identifier && c.BeginEffectiveDate == chartWithReference.BeginEffectiveDate)
                .OrderBy((ch) => ch.Version).ToList();

            chartWithReference.CreatedBy = currentUser;
            chartWithReference.CreatedAt = DateTime.Now;
            if (updateDataId != default(long))
                chartWithReference.UpdatedBasedOn = updateDataId;
            if (previousCharts.Count > 0)
            {
                var latestChart = previousCharts.Last();
                if (!latestChart.IsLocked)
                    throw new FaultException("Actual compatible server version is not locked");

                if (latestChart.LockedBy.Id != currentUser.Id)
                    throw new FaultException("This chart is locked by another user");

                if (latestChart.Version != chartWithReference.Version)
                    throw new FaultException("Version is wrong.Please, get the latest one and then try to publish.");
                chartWithReference.Version = (Int32.Parse(latestChart.Version) + 1).ToString();
                LockChart(latestChart.Id, false);

                if (chartWithReference.IsLocked)
                    chartWithReference.LockedBy = currentUser;

                _dbContext.Repository<ChartWithReference>().Create(chartWithReference);
            }
            else
            {
                if (chartWithReference.IsLocked)
                    chartWithReference.LockedBy = currentUser;
                chartWithReference.Version = 1.ToString();
                _dbContext.Repository<ChartWithReference>().Create(chartWithReference);
            }
            if (updateDataId != default(long))
            {
                var updateData = _dbContext.Repository<ChartUpdateData>().GetById(updateDataId);
                if (updateData == null)
                    throw new FaultException($"System couldn't find associated UpdateData ( Id: {updateDataId})");
                var updateRef = updateData.References.FirstOrDefault(t => t.RefId.Equals(chartWithReference.Identifier));
                if (updateRef == null)
                    throw new FaultException($"System couldn't find reference to ({chartWithReference.Identifier}) in UpdateData table (Id: {updateDataId})");
                //updateData.References.FirstOrDefault(t => t.RefId.Equals(chartWithReference.Identifier)).Done = true;
                updateRef.Done = true;
                _dbContext.Repository<ChartUpdateData>().Update(updateData);
            }
            SaveChartFiles(chartWithReference.Id, preview, source);

            NormalizeEndDates(chartWithReference);
            foreach (var key in SessionStorage.Sessions.Keys)
            {
                if (key.UserName == currentUser.UserName)
                    continue;
                IChartManagerServiceCallback callback = SessionStorage.Sessions[key];
                callback.ChartChanged(chartWithReference, ChartCallBackType.Created);
            }
        }

        private void SaveChartFiles(long id, byte[] preview, byte[] sourece)
        {
            if (!Directory.Exists(_dbContext.FolderPath))
            {
                Directory.CreateDirectory(_dbContext.FolderPath);
            }

            var chartPath = Path.Combine(_dbContext.FolderPath, id.ToString());
            Directory.CreateDirectory(chartPath);

            string zipFileName = Path.Combine(chartPath, _sourceFileName);
            //_logger.InfoWithMemberName($"Creating file ({zipFileName})");
            File.Create(zipFileName).Close();
            //_logger.InfoWithMemberName($"Writing into file");
            File.WriteAllBytes(zipFileName,
                sourece);
            string previewPath = Path.Combine(chartPath, _previewFileName);
            File.WriteAllBytes(previewPath, preview);
            //_logger.InfoWithMemberName($"Decompressing file");
            //ArenaStaticProc.DecompressToDirectory(zipFileName, dirName);
            //File.Delete(zipFileName);
        }

        private void SaveChartUpdateDataFiles(long id, byte[] source)
        {
            var updatePath = Path.Combine(_dbContext.FolderPath, _updateFolderName, id.ToString());
            if (!Directory.Exists(updatePath))
            {
                Directory.CreateDirectory(updatePath);
            }

            string zipFileName = Path.Combine(updatePath, _sourceFileName);
            _logger.InfoWithMemberName($"Creating file ({zipFileName})");
            File.Create(zipFileName).Close();
            _logger.InfoWithMemberName($"Writing into file ({zipFileName})");
            File.WriteAllBytes(zipFileName, source);
            _logger.InfoWithMemberName($"Finished writing into file ({zipFileName})");
        }


        public IList<Chart> GetAffectedCharts(IList<string> idList, DateTime effectiveDate)
        {
            var affectedChartList = _dbContext.Repository<ChartWithReference>().GetAll().Where(t => t.Type == ChartType.Enroute && t.BeginEffectiveDate == effectiveDate && t.FeatureIdList.Any(k => idList.Contains(k))).ToList();
            var chartList = Mapper.Map<List<ChartWithReference>, List<Chart>>(affectedChartList);
            var result = SelectLatestVersions(chartList);
            return result;
        }

        public IList<Chart> GetPendingUpdateList()
        {
            var updateList = _dbContext.Repository<ChartUpdateData>().GetAll().Where(t => t.References.Any(k => !k.Done));
            var resultList = new List<Chart>();
            foreach (var update in updateList)
            {
                foreach (var updateRef in update.References)
                {
                    if (!updateRef.Done)
                    {
                        var foundItem = _dbContext.Repository<Chart>().GetAll().Where(t => t.Identifier == updateRef.RefId && t.BeginEffectiveDate == update.EffectiveDate).FirstOrDefault();
                        if (foundItem != null)
                            resultList.Add(foundItem);
                    }
                }
            }
            return resultList;
        }


        public bool HasPendingUpdate()
        {
            var result = _dbContext.Repository<ChartUpdateReference>().GetAll().Any(t => !t.Done);
            return result;
        }

        /// <summary>
        /// Gets latest chart versions that match for filters
        /// </summary>
        /// <param name="type"></param>
        /// <param name="userId"></param>
        /// <param name="organization"></param>
        /// <param name="aerodrome"></param>
        /// <param name="rwyDir"></param>
        /// <param name="locked"></param>
        /// <param name="createdBeginning"></param>
        /// <param name="createdEnding"></param>
        /// <param name="airacDateBeginning"></param>
        /// <param name="airacDateEnding"></param>
        /// <returns></returns>
        public IList<Chart> GetAllCharts(ChartType? type, string name, long? userId,
            String organization,
            String aerodrome, String rwyDir, bool? locked,
            DateTime? createdBeginning,
            DateTime? createdEnding, DateTime? airacDateBeginning, DateTime? airacDateEnding)
        {
            var currentUser = TryGetCurrentUser();

            var chartList = _dbContext.Repository<Chart>().GetAll();

            if (type.HasValue && type != ChartType.All)
                chartList = chartList.Where(c => c.Type == type);

            if (userId.HasValue && userId != Int64.MaxValue)
                chartList = chartList.Where(c => c.CreatedBy.Id == userId);

            if (!string.IsNullOrEmpty(organization))
                chartList = chartList.Where(c => c.Organization == organization);

            if (!string.IsNullOrEmpty(name))
                chartList = chartList.Where(c => c.Name != null && c.Name.ToLower().Contains(name.ToLower()));

            if (!string.IsNullOrEmpty(aerodrome))
                chartList = chartList.Where(c => c.Airport == aerodrome);

            if (!string.IsNullOrEmpty(rwyDir))
                chartList = chartList.Where(c => c.RunwayDirection == rwyDir);

            if (locked.HasValue)
                chartList = chartList.Where(c => c.IsLocked == locked);

            if (createdBeginning.HasValue)
                chartList = chartList.Where(c => c.CreatedAt >= createdBeginning);

            if (createdEnding.HasValue)
                chartList = chartList.Where(c => c.CreatedAt <= createdEnding);

            if (airacDateBeginning.HasValue)
                chartList = chartList.Where(c => c.BeginEffectiveDate >= airacDateBeginning);

            if (airacDateEnding.HasValue)
                chartList = chartList.Where(c => !c.EndEffectiveDate.HasValue || c.EndEffectiveDate <= airacDateEnding);

            var st = new Stopwatch();
            st.Start();
            var result = SelectLatestVersions(chartList.ToList());
            st.Stop();
            var elaps = st.Elapsed;
            //var updateList = _dbContext.Repository<ChartUpdateData>().GetAll().Where(t => t.EffectiveDate.IsIn(result));
            //result.Any(k => k.BeginEffectiveDate == t.EffectiveDate)).ToList();
            foreach (var chart in result)
            {
                var firstUpdateData = _dbContext.Repository<ChartUpdateData>().GetAll().Where(t => t.EffectiveDate == chart.BeginEffectiveDate).ToList();
                //if(firstUpdateData != null)
                {
                    if (firstUpdateData.Any(k => k.References.Any(t => t.RefId.Equals(chart.Identifier) && !t.Done)))
                        chart.HasUpdate = true;
                }
            }
            return result;
        }

        private static IList<Chart> SelectLatestVersions(IList<Chart> chartList)
        {
            var result = new List<Chart>();
            foreach (var group in chartList.GroupBy(c => c.Identifier))
            {
                var groupsByDates = group.GroupBy(k => k.BeginEffectiveDate);
                result.AddRange(groupsByDates.Select(groupByDate => groupByDate.OrderByDescending(c => c.Version).First()));
            }
            return result;
        }

        public IList<Chart> GetHistoryOf(Guid identifier)
        {
            ChartUser currentUser = TryGetCurrentUser();
            DateTime nw = DateTime.Now;
            Stopwatch st = new Stopwatch();
            st.Start();

            var result = _dbContext.Repository<Chart>().GetAll().Where(ch => ch.Identifier == identifier).OrderByDescending(t => t.BeginEffectiveDate).ToList();
            //result.ForEach(t => t.ReferenceIdList?.Clear());
            return result;
        }

        public Chart GetChart(Guid identifier, DateTime dateTime, int version)
        {
            ChartUser currentUser = TryGetCurrentUser();
            string versionStr = version.ToString();
            var result = _dbContext.Repository<Chart>().GetAll().FirstOrDefault(t =>
                t.Identifier == identifier && t.BeginEffectiveDate == dateTime && t.Version == versionStr);
            //result.ReferenceIdList?.Clear();
            return result;
        }

        public Chart GetChartById(long id)
        {
            ChartUser currentUser = TryGetCurrentUser();
            var result = _dbContext.Repository<Chart>().GetById(id);
            //result.ReferenceIdList?.Clear();
            return result;
        }

        public void LockChart(long id, bool locked)
        {
            var currentUser = TryGetCurrentUser();
            if (currentUser.Privilege != UserPrivilege.Full)
                throw new FaultException("User does not have the necessary privileges");

            var chart = _dbContext.Repository<ChartWithReference>().GetById(id);
            if (chart.IsLocked && !locked)
            {
                if (chart.LockedBy.Id == currentUser.Id || currentUser.IsAdmin)
                {
                    chart.LockedBy = null;
                    chart.IsLocked = false;
                    _dbContext.Repository<ChartWithReference>().Update(chart);

                    foreach (var key in SessionStorage.Sessions.Keys)
                    {
                        if (key.UserName == currentUser.UserName)
                            continue;
                        IChartManagerServiceCallback callback = SessionStorage.Sessions[key];
                        callback.ChartChanged( chart, ChartCallBackType.Unlocked);
                    }
                    return;
                }
                throw new FaultException("This chart is already locked by another user");

            }
            else if (!chart.IsLocked && locked)
            {
                chart.LockedBy = currentUser;
                chart.IsLocked = true;
                _dbContext.Repository<ChartWithReference>().Update(chart);

                foreach (var key in SessionStorage.Sessions.Keys)
                {
                    if (key.UserName == currentUser.UserName)
                        continue;
                    IChartManagerServiceCallback callback = SessionStorage.Sessions[key];
                    callback.ChartChanged(chart, ChartCallBackType.Locked);
                }
                return;
            }
            if (locked)
                throw new FaultException("Chart has already been locked");
            else
                throw new FaultException("Chart has already been unlocked");
        }

        public void DeleteAllChartVersions(Guid identifier)
        {
            ChartUser currentUser = TryGetCurrentUser();
            var charts = _dbContext.Repository<ChartWithReference>().GetAll()
                .Where(c => c.Identifier == identifier);
            
            if (!currentUser.IsAdmin)
            {
                foreach (var chart in charts)
                    CheckUserRightsForDeletingChart(currentUser, chart);
            }

            foreach (var chart in charts)
                DeleteChart(chart);
            // _dbContext.Repository<ChartData>().Delete(chart.Id);
            foreach (var key in SessionStorage.Sessions.Keys)
            {
                if (key.UserName == currentUser.UserName)
                    continue;

                IChartManagerServiceCallback callback = SessionStorage.Sessions[key];
                callback.AllChartVersionsDeleted(identifier);
            }
        }

        public void DeleteChartByEffectiveDate(Guid identifier, DateTime dateTime)
        {
            ChartUser currentUser = TryGetCurrentUser();
            var charts = _dbContext.Repository<ChartWithReference>().GetAll()
                .Where(c => c.Identifier == identifier && c.BeginEffectiveDate == dateTime);

            if (!currentUser.IsAdmin)
            {
                foreach (var chart in charts)
                    CheckUserRightsForDeletingChart(currentUser, chart);
            }

            foreach (var chart in charts)
                DeleteChart(chart);
                //_dbContext.Repository<ChartData>().Delete(chart.Id);

            var nextVersion =_dbContext.Repository<ChartWithReference>().GetAll()
                .Where(c => c.Identifier == identifier && c.BeginEffectiveDate > dateTime).OrderBy(k => k.BeginEffectiveDate).FirstOrDefault();

            List<ChartWithReference> prevVersions = new List<ChartWithReference>();
            foreach (var group in _dbContext.Repository<ChartWithReference>()
                .GetAll().Where(c => c.Identifier == identifier && c.BeginEffectiveDate < dateTime).GroupBy(c => c.BeginEffectiveDate))
            {
                var firstItem = group.OrderByDescending(c => c.Version).First();
                prevVersions.Add(firstItem);
            }
            var latestPrevVersion = prevVersions.OrderBy(c => c.BeginEffectiveDate).LastOrDefault();
            if (latestPrevVersion != null)
            {
                if (nextVersion != null)
                    latestPrevVersion.EndEffectiveDate = nextVersion.BeginEffectiveDate;
                else
                    latestPrevVersion.EndEffectiveDate = null;
                _dbContext.Repository<ChartWithReference>().Update(latestPrevVersion);
            }

            foreach (var key in SessionStorage.Sessions.Keys)
            {
                if (key.UserName == currentUser.UserName)
                    continue;

                IChartManagerServiceCallback callback = SessionStorage.Sessions[key];
                callback.ChartsByEffectiveDateDeleted(identifier, dateTime);
            }
        }

        public void DeleteChartById(long id)
        {
            ChartUser currentUser = TryGetCurrentUser();
            var chart = _dbContext.Repository<ChartWithReference>().GetById(id);

            if (!currentUser.IsAdmin)
                CheckUserRightsForDeletingChart(currentUser, chart);

            if (int.Parse(chart.Version) == 1)
            {
                NormalizeEndDates(chart, true);
            }
            else
            {
                var latestVersion =
                    _dbContext.Repository<ChartWithReference>().GetAll()
                        .Where(c => c.Identifier == chart.Identifier &&
                                    c.BeginEffectiveDate == chart.BeginEffectiveDate)
                        .OrderByDescending(k => k.Version).FirstOrDefault();
                if (latestVersion != null)
                {
                    latestVersion.EndEffectiveDate = chart.EndEffectiveDate;
                    _dbContext.Repository<ChartWithReference>().Update(latestVersion);
                }
                else
                {
                    throw new FaultException("Not found previous version");
                }
            }

            DeleteChart(chart);

            foreach (var key in SessionStorage.Sessions.Keys)
            {
                if (key.UserName == currentUser.UserName)
                    continue;

                IChartManagerServiceCallback callback = SessionStorage.Sessions[key];
                callback.ChartChanged(chart, ChartCallBackType.Deleted);
            }
        }

        private void DeleteChart(ChartWithReference chart)
        {
            _dbContext.Repository<ChartWithReference>().Delete(chart);
            var chartPath = Path.Combine(this._dbContext.FolderPath, chart.Id.ToString());
            if (Directory.Exists(chartPath))
            {
                try
                {
                    Directory.Delete(chartPath, true);
                }
                catch (Exception e)
                {

                }
            }
        }

        private void CheckUserRightsForDeletingChart(ChartUser currentUser, Chart chart)
        {
            if (chart.CreatedBy.Id != currentUser.Id)
                throw new FaultException(
                    $"You are not the owner of chart that begins at {chart.BeginEffectiveDate} and version is {chart.Version}.Please, contact your administrator for further information");

            if (chart.IsLocked && chart.LockedBy.Id != currentUser.Id)
                throw new FaultException($"You can not delete a chart that begins at {chart.BeginEffectiveDate} and version is {chart.Version}.Because it is locked by {chart.LockedBy.FirstName} {chart.LockedBy.LastName}.Please, contact your administrator for further information");
        }

        private void NormalizeEndDates(ChartWithReference chart, bool isDelete = false)
        {
            List<ChartWithReference> allLatestVersions = new List<ChartWithReference>();
            DateTime nextDateTime = DateTime.MaxValue, prevDateTime = DateTime.MinValue;
            List<ChartWithReference> allNextVersions = new List<ChartWithReference>();
            List<ChartWithReference> allPrevVersions = new List<ChartWithReference>(); 
            foreach (var group in _dbContext.Repository<ChartWithReference>()
                .GetAll().Where(c => c.Identifier == chart.Identifier).GroupBy(c => c.BeginEffectiveDate))
            {
                if (group.Key > chart.BeginEffectiveDate && group.Key < nextDateTime)
                {
                    nextDateTime = group.Key;
                    allNextVersions.Clear();
                    allNextVersions.AddRange(group.ToList<ChartWithReference>());
                }
                else if(group.Key < chart.BeginEffectiveDate && group.Key > prevDateTime)
                {
                    prevDateTime = group.Key;
                    allPrevVersions.Clear();
                    allPrevVersions.AddRange(group.ToList<ChartWithReference>());
                }
            }
            if (!isDelete)
            {
                foreach (var nextVersion in allNextVersions)
                {
                    chart.EndEffectiveDate = nextVersion.BeginEffectiveDate;
                    _dbContext.Repository<ChartWithReference>().Update(chart);
                }

                foreach (var prevVersion in allPrevVersions)
                {
                    prevVersion.EndEffectiveDate = chart.BeginEffectiveDate;
                    _dbContext.Repository<ChartWithReference>().Update(prevVersion);
                }
            }
            else
            {
                foreach (var prevVersion in allPrevVersions)
                {
                    if (allNextVersions.Any())
                        prevVersion.EndEffectiveDate = allNextVersions[0].BeginEffectiveDate;
                    else
                        prevVersion.EndEffectiveDate = null;
                    _dbContext.Repository<ChartWithReference>().Update(prevVersion);
                }
            }
        }

        private string GetCaller([System.Runtime.CompilerServices.CallerMemberName] string memberName = "")
        {
            return memberName;
        }
    }
}