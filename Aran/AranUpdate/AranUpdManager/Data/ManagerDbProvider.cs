using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AranUpdater;
using AranUpdater.CommonData;
using AranUpdManager;

namespace AranUpdateManager.Data
{
    class ManagerDbProvider : CommonDbProvider
    {
        public void Open(string server, int port)
        {
            base.Open(server, port, "Manager");
        }

        public List<AranVersion> GetVersions()
        {
            return GetResponseGET<List<AranVersion>>("GetVersions");
        }

        public void WriteNewVersion(NewVersion newVersion)
        {
            GetResponse("WriteNewVersion", newVersion);
        }

        #region User and User Group

        public List<UserGroup> GetUserGroups()
        {
            return GetResponseGET<List<UserGroup>>("GetUserGroups");
        }

        public void SetUserGroup(UserGroup userGroup)
        {
            GetResponse("SetUserGroup", userGroup);
        }

        public void DeleteUserGroup(long id)
        {
            GetResponse("DeleteUserGroup", id);
        }

        public void MoveUser(long userId, long groupId)
        {
            GetResponse("MoveUser", new ARequest<long, long>(userId, groupId));
        }

        public List<User> GetUsers(long parentId)
        {
            return GetResponse<List<User>>("GetUsers", parentId);
        }

        public void SetUser(User user)
        {
            GetResponse("SetUser", user);
        }

        public void DeleteUser(long id)
        {
            GetResponse("DeleteUser", id);
        }

        #endregion

        public List<Tuple<long, DateTime>> GetUserLogDates(long userId, string read)
        {
            return GetResponse<List<Tuple<long, DateTime>>>("GetUserLogDates", new ARequest<long, string>(userId, read));
        }

        public string GetUserLogText(long userLogId)
        {
            return GetResponse<string>("GetUserLogText", userLogId);
        }

        public List<RefItem> GetUserGroupsNotInVersion(long versionId)
        {
            return GetResponse<List<RefItem>>("GetUserGroupsNotInVersion", versionId);
        }

        public List<VersionUserGroupDoc> GetVersionUserGroupDocs(long versionId)
        {
            return GetResponse<List<VersionUserGroupDoc>>("GetVersionUserGroupDocs", versionId);
        }

        public void SetVersionUserGroup(VersionUserGroupDoc vug)
        {
            GetResponse("SetVersionUserGroup", vug);
        }
    }
}
