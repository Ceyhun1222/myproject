using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using ObstacleManagementSystem.Models;

namespace ObstacleManagementSystem.Controllers
{
    public abstract class BaseController : Controller
    {
        private string _currentLanguage;

        public ActionResult RedirectToDefaultLanguage()
        {
            var lang = CurrentLanguage;
            return RedirectToAction("Index", new { lang = lang });
        }

        private string CurrentLanguage
        {
            get
            {
                if (string.IsNullOrEmpty(_currentLanguage))
                {
                    var feature = HttpContext.Features.Get<IRequestCultureFeature>();
                    _currentLanguage = feature.RequestCulture.Culture.TwoLetterISOLanguageName.ToLower();
                }

                return _currentLanguage;
            }
        }

        internal void AddBreadcrumb(string displayName, string urlPath)
        {
            List<Message> messages;

            if (ViewBag.Breadcrumb == null)
            {
                messages = new List<Message>();
            }
            else
            {
                messages = (List<Message>)ViewBag.Breadcrumb;
            }

            messages.Add(new Message { DisplayName = displayName, URLPath = urlPath });
            ViewBag.Breadcrumb = messages;
        }

        internal void AddPageHeader(string pageHeader = "", string pageDescription = "")
        {
            ViewBag.PageHeader = Tuple.Create(pageHeader, pageDescription);
        }

        internal enum PageAlertType
        {
            Error,
            Info,
            Warning,
            Success
        }

        internal void AddPageAlerts(PageAlertType pageAlertType, string description)
        {
            List<Message> messages;

            if (ViewBag.PageAlerts == null)
            {
                messages = new List<Message>();
            }
            else
            {
                messages = ViewBag.PageAlerts as List<Message>;
            }

            messages.Add(new Message { Type = pageAlertType.ToString().ToLower(), ShortDesc = description });
            ViewBag.PageAlerts = messages;
        }

        public string GetConrollerName(string contollerName = null)
        {
            if (string.IsNullOrEmpty(contollerName))
                contollerName = this.GetType().Name;
            return contollerName.Replace(nameof(Controller), "");
        }
    }
}
