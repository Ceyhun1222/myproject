using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using AranUpdManager;

namespace AranUpdater
{
    [ServiceContract]
    public interface IAUServer
    {
        #region Updater Server

        [OperationContract]
        [WebInvoke(UriTemplate = "/Updater/Register", BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Json)]
        AResponse<int> Register(string userName);

        [OperationContract]
        [WebInvoke(UriTemplate = "/Updater/GetNewVersion", BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Json)]
        AResponse<AranVersionInfo> GetNewVersion(int userId);

        [OperationContract]
        [WebInvoke(UriTemplate = "/Updater/AddLog", BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Json)]
        AResponse AddLog(UserRequest<string> req);

        [OperationContract]
        [WebInvoke(UriTemplate = "/Updater/SetLastVersion", BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Json)]
        AResponse SetLastVersion(UserRequest<int, LastVersionType> req);

        #endregion

        #region Manager

        [OperationContract]
        [WebInvoke(UriTemplate = "/Manager/GetVersions", Method = "GET", BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Json)]
        AResponse<List<AranVersion>> GetVersions();

        [OperationContract]
        [WebInvoke(UriTemplate = "/Manager/WriteNewVersion", BodyStyle = WebMessageBodyStyle.Bare, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        AResponse WriteNewVersion(NewVersion newVersion);

        #region User and User Group

        [OperationContract]
        [WebInvoke(UriTemplate = "/Manager/GetUserGroups", Method = "GET", BodyStyle = WebMessageBodyStyle.Bare, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        AResponse<List<UserGroup>> GetUserGroups();

        [OperationContract]
        [WebInvoke(UriTemplate = "/Manager/SetUserGroup", BodyStyle = WebMessageBodyStyle.Bare, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        AResponse SetUserGroup(UserGroup userGroup);

        [OperationContract]
        [WebInvoke(UriTemplate = "/Manager/DeleteUserGroup", BodyStyle = WebMessageBodyStyle.Bare, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        AResponse DeleteUserGroup(long id);

        [OperationContract]
        [WebInvoke(UriTemplate = "/Manager/MoveUser", BodyStyle = WebMessageBodyStyle.Bare, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        AResponse MoveUser(ARequest<long, long> req);

        [OperationContract]
        [WebInvoke(UriTemplate = "/Manager/GetUsers", BodyStyle = WebMessageBodyStyle.Bare, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        AResponse<List<User>> GetUsers(long parentId);

        [OperationContract]
        [WebInvoke(UriTemplate = "/Manager/SetUser", BodyStyle = WebMessageBodyStyle.Bare, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        AResponse SetUser(User user);

        [OperationContract]
        [WebInvoke(UriTemplate = "/Manager/DeleteUser", BodyStyle = WebMessageBodyStyle.Bare, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        AResponse DeleteUser(long id);

        #endregion

        [OperationContract]
        [WebInvoke(UriTemplate = "/Manager/GetUserLogDates", BodyStyle = WebMessageBodyStyle.Bare, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        AResponse<List<Tuple<long, DateTime>>> GetUserLogDates(ARequest<long, string> req);

        [OperationContract]
        [WebInvoke(UriTemplate = "/Manager/GetUserLogText", BodyStyle = WebMessageBodyStyle.Bare, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        AResponse<string> GetUserLogText(long userLogId);

        [OperationContract]
        [WebInvoke(UriTemplate = "/Manager/GetUserGroupsNotInVersion", BodyStyle = WebMessageBodyStyle.Bare, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        AResponse<List<RefItem>> GetUserGroupsNotInVersion(long versionId);

        [OperationContract]
        [WebInvoke(UriTemplate = "/Manager/GetVersionUserGroupDocs", BodyStyle = WebMessageBodyStyle.Bare, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        AResponse<List<VersionUserGroupDoc>> GetVersionUserGroupDocs(long versionId);

        [OperationContract]
        [WebInvoke(UriTemplate = "/Manager/SetVersionUserGroup", BodyStyle = WebMessageBodyStyle.Bare, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        AResponse SetVersionUserGroup(VersionUserGroupDoc vug);

        #endregion
    }
}
