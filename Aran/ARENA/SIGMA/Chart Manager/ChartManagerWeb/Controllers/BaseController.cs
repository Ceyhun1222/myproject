using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.ServiceModel;
using System.Text;
using System.Web;
using System.Web.Mvc;
using ChartManagerWeb.ChartServiceReference;
using ChartManagerWeb.Helper;
using ChartManagerWeb.Helper.Attribute;
using ChartManagerWeb.Models.ViewModel;

namespace ChartManagerWeb.Controllers
{
    [LogActionFilter]
    public class BaseController : Controller
    {
        private ChartManagerServiceClient _client;

        public ChartManagerServiceClient ServiceClient
        {
            get
            {
                if (_client.State == CommunicationState.Faulted)
                    CreateService();
                return _client;
            }
        }

        public BaseController()
        {
            CreateService();
        }

        //protected void CheckSession()
        //{
        //    //if (Request.Cookies[nameof(AccountController.Login)]?.Value != null)
        //    //{
        //    //    Session[_sessionUserName] = Request.Cookies[nameof(AccountController.Login)][nameof(LoginViewModel.Username)];
        //    //    Session[_sessionPswrd] = Request.Cookies[nameof(AccountController.Login)][nameof(LoginViewModel.Password)];
        //    //}

        //    //if (Session != null && Session[_sessionUserName] != null && Session[_sessionPswrd] != null)
        //    //{
        //    //    SetCredentials(Session[_sessionUserName].ToString(), Session[_sessionPswrd].ToString());
        //    //    try
        //    //    {
        //    //        CurrUser = ServiceClient.GetCurrentUser();
        //    //    }
        //    //    catch (Exception e)
        //    //    {
        //    //        CurrUser = null;
        //    //    }

        //    //}
        //    //ViewBag.IsAuthenticated = IsAuthenticated;
        //    //ViewBag.IsAdmin = false;
        //    //ViewBag.Username = "";
        //    //ViewBag.UserId = 0;
        //    //if (!IsAuthenticated) return;
        //    //ViewBag.Username = $"{CurrUser.FirstName} {CurrUser.LastName}";
        //    //ViewBag.UserId = CurrUser.Id;
        //    //ViewBag.IsAdmin = IsAdmin();

        //}

        private void CreateService()
        {
            var callback = new ChartServiceCallBackListener();
            var instanceContext = new InstanceContext(callback);
            _client = new ChartManagerServiceClient(instanceContext, _bindingName);            
            _client.Endpoint.Address = new EndpointAddress(_client.Endpoint.Address.Uri,
                EndpointIdentity.CreateDnsIdentity(_certificateName));
        }

        public ChartUser CurrUser { get; set; }

        public bool Authenticated => CurrUser != null;

        public void SetCredentialsInSession(string username, string password)
        {
            _client.ClientCredentials.UserName.UserName = username;
            Session[_sessionUserName] = username;
            SetPasswordInSession(password);
            try
            {
                CurrUser = ServiceClient.TryGetCurrentUser();
            }
            catch (Exception e)
            {
                CurrUser = null;
            }

        }

        public void SetPasswordInSession(string password)
        {
            if (_client.State == CommunicationState.Opened)
            {
                var userName = _client.ClientCredentials.UserName.UserName;
                CreateService();
                SetCredentialsInSession(userName, password);
                return;
            }
            _client.ClientCredentials.UserName.Password = password;
            Session[_sessionPswrd] = password;
        }

        //public bool IsAdmin()
        //{
        //    try
        //    {
        //        return _client.GetCurrentUser().IsAdmin;
        //    }
        //    catch (Exception e)
        //    {
        //        return false;
        //    }

        //}

        public void Success(string message, bool dismissable = false)
        {
            AddAlert(AlertStyles.Success, message, dismissable);
        }

        public void Information(string message, bool dismissable = false)
        {
            AddAlert(AlertStyles.Information, message, dismissable);
        }

        public void Warning(string message, bool dismissable = false)
        {
            AddAlert(AlertStyles.Warning, message, dismissable);
        }

        public void Danger(string message, bool dismissable = false)
        {
            AddAlert(AlertStyles.Danger, message, dismissable);
        }

        private void AddAlert(string alertStyle, string message, bool dismissable)
        {
            var alerts = TempData.ContainsKey(Alert.TempDataKey)
                ? (List<Alert>)TempData[Alert.TempDataKey]
                : new List<Alert>();

            alerts.Add(new Alert
            {
                AlertStyle = alertStyle,
                Message = message,
                Dismissable = dismissable
            });

            TempData[Alert.TempDataKey] = alerts;
        }

        public string Sha256_hash(String value)
        {
            var sb = new StringBuilder();

            using (SHA256 hash = SHA256.Create())
            {
                Encoding enc = Encoding.UTF8;
                Byte[] result = hash.ComputeHash(enc.GetBytes(value));

                foreach (Byte b in result)
                    sb.Append(b.ToString("x2"));
            }

            return sb.ToString();
        }

        private string _bindingName => "netTcp_ChartService";
        private string _certificateName => "ChartManagerTempCert";
        public string _sessionUserName = "username";
        public string _sessionPswrd = "password";
    }
}