using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ChartManagerWeb.Controllers;
using ChartManagerWeb.Models.ViewModel;

namespace ChartManagerWeb.Helper.Attribute
{
    public class SessionExpireAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {

            var baseController = (BaseController) filterContext.Controller;
            HttpContext ctx = HttpContext.Current;

            if (ctx.Request.Cookies[nameof(AccountController.Login)]?.Value != null)
            {
                baseController.Session[baseController._sessionUserName] =
                    ctx.Request.Cookies[nameof(AccountController.Login)][nameof(LoginViewModel.Username)];
                baseController.Session[baseController._sessionPswrd] = ctx.Request.Cookies[nameof(AccountController.Login)][nameof(LoginViewModel.Password)];
            }

            if (baseController.Session != null && baseController.Session[baseController._sessionUserName] != null &&
                baseController.Session[baseController._sessionPswrd] != null)
            {
                baseController.SetCredentialsInSession(baseController.Session[baseController._sessionUserName].ToString(),
                    baseController.Session[baseController._sessionPswrd].ToString());
            }

            filterContext.Controller.ViewBag.IsAuthenticated = baseController.Authenticated;
            filterContext.Controller.ViewBag.IsAdmin = false;
            filterContext.Controller.ViewBag.Username = string.Empty;
            filterContext.Controller.ViewBag.UserId = 0;
            if (!baseController.Authenticated)
            {
                string actionName = (string)filterContext.RouteData.Values["action"];
                if (actionName == nameof(AccountController.Login) && baseController is AccountController)
                    return;
                filterContext.Result = new RedirectResult("~/Account/Login");
                return;
            }

            filterContext.Controller.ViewBag.Username =
                $"{baseController.CurrUser.FirstName} {baseController.CurrUser.LastName}";
            filterContext.Controller.ViewBag.UserId = baseController.CurrUser.Id;
            filterContext.Controller.ViewBag.IsAdmin = baseController.CurrUser.IsAdmin;
            base.OnActionExecuting(filterContext);
        }
    }
}