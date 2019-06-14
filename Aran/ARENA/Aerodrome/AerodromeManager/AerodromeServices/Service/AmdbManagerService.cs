using AerodromeServices.DataContract;
using AerodromeServices.Helpers;
using AerodromeServices.Repositories;
using AerodromeServices.Services_Contract;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Xml;
using ArenaStatic;

namespace AerodromeServices.Service
{
    [MainErrorHandlerBehavior(typeof(MainErrorHandler))]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
    public class AmdbManagerService : IAmdbManagerService
    {
        private readonly RepositoryContext _dbContext;
        private string _previewJpg = "preview.jpg";
        private string _sourceZip = "source.zip";

        public AmdbManagerService()
        {
            _dbContext = new RepositoryContext();
        }

        public void Login()
        {          
        }
        
        public List<User> GetAllUser()
        {
            User currentUser = GetCurrentUser();           
            //if (currentUser.IsAdmin)
                return _dbContext.Repository<User>().GetAll().ToList();

            //return _dbContext.Repository<ChartUser>().GetAll().Where(u => u.Id == currentUser.Id).ToList();

        }

        public List<string> GetOrganizations()
        {
            User currentUser = GetCurrentUser();
            return _dbContext.Repository<AmdbMetadata>().GetAll().Select(k => k.Organization).Distinct().ToList();
        }

        public List<string> GetAerodromes(string organization)
        {
            var lst = _dbContext.Repository<AmdbMetadata>().GetAll();
            if (!string.IsNullOrEmpty(organization))
                lst = lst.Where(chart => chart.Organization == organization);
            return lst.Select(t => t.Airport).Distinct().ToList();
        }

        public User GetUser(long id)
        {
            User currentUser = GetCurrentUser();           
            if (!currentUser.IsAdmin && currentUser.Id != id)
                throw new FaultException("You don't have privileges to get another user's data");

            return _dbContext.Repository<User>().GetById(id);
        }

        public User GetCurrentUser()
        {
            User currentUser = null;
            if (WcfOperationContext.Current != null && WcfOperationContext.Current.Items.ContainsKey("user"))
                currentUser = (User)WcfOperationContext.Current.Items["user"];

            if (currentUser == null)
                throw new FaultException("User not found in database");

            if (currentUser.Disabled)
                throw new FaultException("Your account is disabled");

            return currentUser;
        }


        public void DeleteUser(long id)
        {
            User currentUser = GetCurrentUser();                     
            if (!currentUser.IsAdmin)
                throw new FaultException("You don't have privileges to delete user's data");

            User user = _dbContext.Repository<User>().GetById(id);

            var sessionsByUser = SessionStorage.Sessions.Where(s => s.Key.UserName == user.UserName);

            _dbContext.Repository<User>().Delete(id);

            foreach (var session in sessionsByUser)
            {
                IAmdbManagerServiceCallback callback = SessionStorage.Sessions[session.Key];
                callback.UserChanged(UserCallbackType.Deleted);
            }
        }

        public void CreateUser(User user)
        {
            User currentUser = GetCurrentUser();           
            if (!currentUser.IsAdmin)
                throw new FaultException("You don't have privileges to create user data");

            if(user.UserName.Equals(""))
                throw new FaultException("Username is null");

            User checkUsername = _dbContext.Repository<User>()
                .GetAll()
                .FirstOrDefault(u => u.UserName == user.UserName);
            if (checkUsername != null)
                throw new FaultException("Username is already exist");

            if (!user.Email.Equals(""))
            {
                User checkEmail = _dbContext.Repository<User>()
                    .GetAll()
                    .FirstOrDefault(u => u.Email == user.Email);
                if (checkEmail != null)
                    throw new FaultException("Email is already exist");
            }

            user.LatestLoginAt = DateTime.Now;
            user.Password = HelperMethods.Sha256_hash(user.Password);

            _dbContext.Repository<User>().Create(user);
        }

        public bool ExistsUser(string username, string password)
        {
            User currentUser = GetCurrentUser();
            if (currentUser.IsAdmin)
                return _dbContext.Repository<User>().GetAll()
                    .Any(t => t.UserName == username && t.Password == password);
            throw new FaultException(@"You don't have privileges to call ""ExistsUser"" method");
        }

        public void UpdateUser(User user)
        {
            User currentUser = GetCurrentUser();
            if (!currentUser.IsAdmin && currentUser.Id != user.Id)
                throw new FaultException("You don't have privileges to update another user's data");

            var userForUpdate = GetUser(user.Id);
            if (userForUpdate == null)
                throw new FaultException("User not found");

            if (userForUpdate.UserName != user.UserName)
            {
                User checkUsername = _dbContext.Repository<User>()
                    .GetAll()
                    .FirstOrDefault(u => u.UserName == user.UserName);
                if (checkUsername != null)
                    throw new FaultException("Username is already exist");
                userForUpdate.UserName = user.UserName;
            }

            if (userForUpdate.Email != user.Email)
            {
                User checkEmail = _dbContext.Repository<User>()
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
            _dbContext.Repository<User>().Update(userForUpdate);

            var sessionsByUser = SessionStorage.Sessions.Where(s => s.Key.UserName == user.UserName);

            foreach (var session in sessionsByUser)
            {
                IAmdbManagerServiceCallback callback = SessionStorage.Sessions[session.Key];
                callback.UserChanged(UserCallbackType.Updated);
            }
        }

        public void DisableUser(long id, bool disabled)
        {
            User currentUser = GetCurrentUser();
            User user = _dbContext.Repository<User>().GetById(id);

            if (!currentUser.IsAdmin && currentUser.Id != user.Id)
                throw new FaultException("You don't have privileges to update another user's data");

            user.Disabled = disabled;
            _dbContext.Repository<User>().Update(user);

            var sessionsByUser = SessionStorage.Sessions.Where(s => s.Key.UserName == user.UserName);
            var callbackType = disabled ? UserCallbackType.Disabled : UserCallbackType.Enabled;

            foreach (var session in sessionsByUser)
            {
                IAmdbManagerServiceCallback callback = SessionStorage.Sessions[session.Key];
                callback.UserChanged(callbackType);
            }
        }

        public void ChangePassword(long id, string oldPassword, string newPassword)
        {
            User currentUser = GetCurrentUser();
            User user = _dbContext.Repository<User>().GetById(id);

            if (currentUser.Id == user.Id && !currentUser.IsAdmin)
            {
                if (user.Password != HelperMethods.Sha256_hash(oldPassword))
                    throw new FaultException("Old password is incorrect");

                user.Password = HelperMethods.Sha256_hash(newPassword);
                _dbContext.Repository<User>().Update(user);
            }
            else
            {
                if (!currentUser.IsAdmin)
                    throw new FaultException("You don't have privileges to change another user's data");

                user.Password = HelperMethods.Sha256_hash(newPassword);
                _dbContext.Repository<User>().Update(user);

                var sessionsByUser = SessionStorage.Sessions.Where(s => s.Key.UserName == user.UserName);

                foreach (var session in sessionsByUser)
                {
                    IAmdbManagerServiceCallback callback = SessionStorage.Sessions[session.Key];
                    callback.UserChanged(UserCallbackType.ChangedPassword);
                }

            }
        }

        public byte[] GetConfigFile(long id)
        {
            User currentUser = GetCurrentUser();
            var count = SessionStorage.Sessions.Count;
          
            if (!currentUser.IsAdmin)
                throw new FaultException("You don't have privileges to get config data");

            User user = _dbContext.Repository<User>().GetById(id);

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
            addressTcp.InnerText = "net.tcp://" + HelperMethods.GetLocalIpAddress() + Common.TcpPort + "/AerodromeServices";
            elem.AppendChild(addressTcp);

            var addressHttp = doc.CreateElement("AddressHTTP");
            addressHttp.InnerText = "http://" + HelperMethods.GetLocalIpAddress() + Common.HttpPort+ "/AerodromeServices";
            elem.AppendChild(addressHttp);

            return Encoding.Default.GetBytes(HelperMethods.Beautify(doc.OuterXml));
        }

        public AmdbMetadata GetLatestAmdbVersion(Guid identifier)
        {
            User currentUser = GetCurrentUser();
            var chartData = _dbContext.Repository<AmdbMetadata>().GetAll()
                .Where(d => d.Identifier == identifier).OrderByDescending(c => c.Version)
                .FirstOrDefault();
            if (chartData == null)
                return null;
            AmdbMetadata amdbMeta = new AmdbMetadata()
            {
                Airport = chartData.Airport,               
                CreatedAt = chartData.CreatedAt,
                CreatedBy = chartData.CreatedBy,                
                Id = chartData.Id,
                Identifier = chartData.Identifier,
                IsLocked = chartData.IsLocked,
                LockedBy = chartData.LockedBy,
                Name = chartData.Name,
                Organization = chartData.Organization,         
                Version = chartData.Version
            };
            return amdbMeta;
        }

        
        public byte[] GetPreviewOf(long id)
        {
            User currentUser = GetCurrentUser();
            AmdbMetadata chart = _dbContext.Repository<AmdbMetadata>().GetAll().FirstOrDefault(d => d.Id == id);

            if (chart == null)
                throw new NullReferenceException("Chart Preview is null");
            //chartData.Source = null;
            var chartPath = Path.Combine(_dbContext.FolderPath, id.ToString());
            string previewPath = Path.Combine(chartPath, _previewJpg);            
            return File.ReadAllBytes(previewPath);
        }

        public byte[] GetSourceOf(long id, bool locked = false)
        {
            User currentUser = GetCurrentUser();
            if (locked)
                LockAmdb(id, true);
            AmdbMetadata chartData = _dbContext.Repository<AmdbMetadata>().GetAll().FirstOrDefault(d => d.Id == id);

            if (chartData == null)
                throw new NullReferenceException("Chart Source is null");
            //chartData.Preview = null;
            var chartPath = Path.Combine(_dbContext.FolderPath, id.ToString());
            string previewPath = Path.Combine(chartPath, _sourceZip);
            return File.ReadAllBytes(previewPath);

        }

        public void Upload(AmdbMetadata amdbMeta, byte[] source)
        {
            User currentUser = GetCurrentUser();            
            if (currentUser.Privilege != UserPrivilege.Full)
                throw new FaultException("User does not have the necessary privileges");

            var previousCharts = _dbContext.Repository<AmdbMetadata>()
                .GetAll().Where(c => c.Identifier == amdbMeta.Identifier)
                .OrderBy((ch) => ch.Version).ToList();

            amdbMeta.CreatedBy = currentUser;
            amdbMeta.CreatedAt = DateTime.Now;

            if (previousCharts.Count > 0)
            {
                var latestChart = previousCharts.Last();
                if (!latestChart.IsLocked)
                    throw new FaultException("Actual compatible server version is not locked");

                if (latestChart.LockedBy.Id != currentUser.Id)
                    throw new FaultException("This chart is locked by another user");

                if (latestChart.Version != amdbMeta.Version)
                    throw new FaultException("Version is wrong.Please, get the latest one and then try to publish.");
                amdbMeta.Version = (Int32.Parse(latestChart.Version) + 1).ToString();
                LockAmdb(latestChart.Id, false);

                if (amdbMeta.IsLocked)
                    amdbMeta.LockedBy = currentUser;

                 _dbContext.Repository<AmdbMetadata>().Create(amdbMeta as AmdbMetadata);
            }
            else
            {
                if (amdbMeta.IsLocked)
                    amdbMeta.LockedBy = currentUser;
                amdbMeta.Version = "1";
                _dbContext.Repository<AmdbMetadata>().Create(amdbMeta as AmdbMetadata);
            }

            SaveFiles(amdbMeta.Id, source);

            foreach (var key in SessionStorage.Sessions.Keys)
            {
                if (key.UserName == currentUser.UserName)
                    continue;
                IAmdbManagerServiceCallback callback = SessionStorage.Sessions[key];
                callback.AmdbChanged(amdbMeta, AmdbCallBackType.Created);
            }
        }

        private void SaveFiles(long id, byte[] sourece)
        {
            if (!Directory.Exists(_dbContext.FolderPath))
            {
                Directory.CreateDirectory(_dbContext.FolderPath);
            }

            var chartPath = Path.Combine(_dbContext.FolderPath, id.ToString());
            Directory.CreateDirectory(chartPath);

            string zipFileName = Path.Combine(chartPath, _sourceZip);
            //LogManager.GetLogger(this).InfoWithMemberName($"Creating file ({zipFileName})");
            File.Create(zipFileName).Close();
            //LogManager.GetLogger(this).InfoWithMemberName($"Writing into file");
            File.WriteAllBytes(zipFileName,
                sourece);          
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
        public List<AmdbMetadata> GetAllAmdbFiles(string name, long? userId,
            String organization,
            String aerodrome, bool? locked,
            DateTime? createdBeginning,
            DateTime? createdEnding)
        {
            var currentUser = GetCurrentUser();
            var nw = DateTime.Now;
            
            var chartList = _dbContext.Repository<AmdbMetadata>().GetAll();
            
            if (userId.HasValue && userId != Int64.MaxValue)
                chartList = chartList.Where(c => c.CreatedBy.Id == userId);

            if (!string.IsNullOrEmpty(organization))
                chartList = chartList.Where(c => c.Organization == organization);

            if (!string.IsNullOrEmpty(name))
                chartList = chartList.Where(c => c.Name != null &&  c.Name.ToLower().Contains(name.ToLower()));

            if (!string.IsNullOrEmpty(aerodrome))
                chartList = chartList.Where(c => c.Airport == aerodrome);

            if (locked.HasValue)
                chartList = chartList.Where(c => c.IsLocked == locked);

            if (createdBeginning.HasValue)
                chartList = chartList.Where(c => c.CreatedAt >= createdBeginning);

            if (createdEnding.HasValue)
                chartList = chartList.Where(c => c.CreatedAt <= createdEnding);

            var result = new List<AmdbMetadata>();
            var st = new Stopwatch();
            st.Start();
            foreach (var group in chartList.GroupBy(c => c.Identifier))
            {
                var groupsByArp = group.GroupBy(k => k.Airport);
                result.AddRange(groupsByArp
                    .Select(groupByDate => groupByDate.OrderByDescending(c => c.Version).First()));
                //.Select(chartData => CreateChartFrom(chartData as ChartData)));
            }
            st.Stop();
            var elaps = st.Elapsed;
            return result;            
        }

        private AmdbMetadata CreateAmdbFrom(AmdbData chartData)
        {
            return new AmdbMetadata()
            {
                Airport = chartData.Airport,                
                CreatedAt = chartData.CreatedAt,
                CreatedBy = chartData.CreatedBy,
                Id = chartData.Id,
                Identifier = chartData.Identifier,
                IsLocked = chartData.IsLocked,
                LockedBy = chartData.LockedBy,
                Name = chartData.Name,
                Organization = chartData.Organization,
                Version = chartData.Version
            };
        }

        public List<AmdbMetadata> GetHistoryOf(Guid identifier)
        {
            User currentUser = GetCurrentUser();
            DateTime nw = DateTime.Now;
            Stopwatch st = new Stopwatch();
            st.Start();
            
            var chartList = _dbContext.Repository<AmdbMetadata>().GetAll().Where(ch => ch.Identifier == identifier).ToList()
                .OrderByDescending(t => t.Version);

            var result = new List<AmdbMetadata>();
            foreach (var chartData in chartList)
            {
                result.Add(new AmdbMetadata()
                {
                    Airport = chartData.Airport,                    
                    CreatedAt = chartData.CreatedAt,
                    CreatedBy = chartData.CreatedBy,
                    Id = chartData.Id,
                    Identifier = chartData.Identifier,
                    IsLocked = chartData.IsLocked,
                    LockedBy = chartData.LockedBy,
                    Name = chartData.Name,
                    Organization = chartData.Organization,         
                    Version = chartData.Version
                });
            }
            st.Stop();
            var elapsed = st.Elapsed;
            return result;
        }

        public AmdbMetadata GetAmdb(Guid identifier, string version)
        {
            User currentUser = GetCurrentUser();            
            var chartData = _dbContext.Repository<AmdbMetadata>().GetAll().FirstOrDefault(t =>
                t.Identifier == identifier  && t.Version == version);
            if (chartData == null)
                return null;
            AmdbMetadata amdbMeta = new AmdbMetadata()
            {
                Airport = chartData.Airport,               
                CreatedAt = chartData.CreatedAt,
                CreatedBy = chartData.CreatedBy,            
                Id = chartData.Id,
                Identifier = chartData.Identifier,
                IsLocked = chartData.IsLocked,
                LockedBy = chartData.LockedBy,
                Name = chartData.Name,
                Organization = chartData.Organization,            
                Version = chartData.Version
            };
            return amdbMeta;
        }

        public AmdbMetadata GetAmdbById(long id)
        {
            User currentUser = GetCurrentUser();
            var chartData = _dbContext.Repository<AmdbMetadata>().GetById(id);
            if (chartData == null)
                return null;
            AmdbMetadata amdbMeta = new AmdbMetadata()
            {
                Airport = chartData.Airport,
                CreatedAt = chartData.CreatedAt,
                CreatedBy = chartData.CreatedBy,
                Id = chartData.Id,
                Identifier = chartData.Identifier,
                IsLocked = chartData.IsLocked,
                LockedBy = chartData.LockedBy,
                Name = chartData.Name,
                Organization = chartData.Organization,       
                Version = chartData.Version
            };
            return amdbMeta;
        }

        public void LockAmdb(long amdbId, bool locked)
        {
            var currentUser = GetCurrentUser();
            if (currentUser.Privilege != UserPrivilege.Full)
                throw new FaultException("User does not have the necessary privileges");

            var chart = _dbContext.Repository<AmdbMetadata>().GetById(amdbId);
            if (chart.IsLocked && !locked)
            {
                if (chart.LockedBy.Id == currentUser.Id || currentUser.IsAdmin)
                {
                    chart.LockedBy = null;
                    chart.IsLocked = false;
                    _dbContext.Repository<AmdbMetadata>().Update(chart);

                    foreach (var key in SessionStorage.Sessions.Keys)
                    {
                        if (key.UserName == currentUser.UserName)
                            continue;
                        IAmdbManagerServiceCallback callback = SessionStorage.Sessions[key];
                        callback.AmdbChanged( chart, AmdbCallBackType.Unlocked);
                    }
                    return;
                }
                throw new FaultException("This chart is already locked by another user");

            }
            else if (!chart.IsLocked && locked)
            {
                chart.LockedBy = currentUser;
                chart.IsLocked = true;
                _dbContext.Repository<AmdbMetadata>().Update(chart);

                foreach (var key in SessionStorage.Sessions.Keys)
                {
                    if (key.UserName == currentUser.UserName)
                        continue;
                    IAmdbManagerServiceCallback callback = SessionStorage.Sessions[key];
                    callback.AmdbChanged(chart, AmdbCallBackType.Locked);
                }
                return;
            }
            if (locked)
                throw new FaultException("Chart has already been locked");
            else
                throw new FaultException("Chart has already been unlocked");
        }

        public void DeleteAllAmdbVersions(Guid identifier)
        {
            User currentUser = GetCurrentUser();
            var charts = _dbContext.Repository<AmdbMetadata>().GetAll()
                .Where(c => c.Identifier == identifier);
            
            if (!currentUser.IsAdmin)
            {
                foreach (var chart in charts)
                    CheckUserRightsForDeletingAmdb(currentUser, chart);
            }

            foreach (var chart in charts)
                DeleteFolder(chart);
            // _dbContext.Repository<ChartData>().Delete(chart.Id);
            foreach (var key in SessionStorage.Sessions.Keys)
            {
                if (key.UserName == currentUser.UserName)
                    continue;

                IAmdbManagerServiceCallback callback = SessionStorage.Sessions[key];
                callback.AllChartVersionsDeleted(identifier);
            }
        }

        public void DeleteAmdbByVersion(Guid identifier, string version)
        {
            User currentUser = GetCurrentUser();
            var amdbFiles = _dbContext.Repository<AmdbMetadata>().GetAll()
                .Where(c => c.Identifier == identifier && c.Version == version);

            if (!currentUser.IsAdmin)
            {
                foreach (var chart in amdbFiles)
                    CheckUserRightsForDeletingAmdb(currentUser, chart);
            }

            foreach (var chart in amdbFiles)
                DeleteFolder(chart);
                //_dbContext.Repository<ChartData>().Delete(chart.Id);
           

            foreach (var key in SessionStorage.Sessions.Keys)
            {
                if (key.UserName == currentUser.UserName)
                    continue;

                IAmdbManagerServiceCallback callback = SessionStorage.Sessions[key];
                callback.ChartsByEffectiveDateDeleted(identifier, version);
            }
        }

        public void DeleteAmdbById(long id)
        {
            User currentUser = GetCurrentUser();
            var chart = _dbContext.Repository<AmdbMetadata>().GetById(id);

            if (!currentUser.IsAdmin)
                CheckUserRightsForDeletingAmdb(currentUser, chart);
            
            DeleteFolder(chart);

            foreach (var key in SessionStorage.Sessions.Keys)
            {
                if (key.UserName == currentUser.UserName)
                    continue;

                IAmdbManagerServiceCallback callback = SessionStorage.Sessions[key];
                callback.AmdbChanged(chart, AmdbCallBackType.Deleted);
            }
        }

        private void DeleteFolder(AmdbMetadata chart)
        {
            _dbContext.Repository<AmdbMetadata>().Delete(chart.Id);
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

        private void CheckUserRightsForDeletingAmdb(User currentUser, AmdbMetadata chart)
        {
            if (chart.CreatedBy.Id != currentUser.Id)
                throw new FaultException(
                    $"You are not the owner of chart that begins at {chart.Identifier} and version is {chart.Version}.Please, contact your administrator for further information");

            if (chart.IsLocked && chart.LockedBy.Id != currentUser.Id)
                throw new FaultException($"You can not delete a chart that begins at {chart.Identifier} and version is {chart.Version}.Because it is locked by {chart.LockedBy.FirstName} {chart.LockedBy.LastName}.Please, contact your administrator for further information");
        }


        private string GetCaller([System.Runtime.CompilerServices.CallerMemberName] string memberName = "")
        {
            return memberName;
        }
    }
}