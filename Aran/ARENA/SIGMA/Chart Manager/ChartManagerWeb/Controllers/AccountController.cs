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
using ChartManagerWeb.Models;
using ChartManagerWeb.Models.ViewModel;

namespace ChartManagerWeb.Controllers
{
    [LogActionFilter]
    public class AccountController : BaseController
    {
        [HttpGet]
        [AllowAnonymous]
        [SessionExpire]
        public ActionResult Login()
        {
            //CheckSession();

            if (Request.Cookies[nameof(Login)]?.Value == null || !Authenticated)
            {                
                return View();
            }
            return RedirectToAction("Index", "Chart");
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel loginViewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var userName = loginViewModel.Username;
                    var password = Sha256_hash(loginViewModel.Password);
                    SetCredentialsInSession(userName, password);
                    if (!Authenticated)
                        return View(loginViewModel);

                    if (loginViewModel.RememberMe)
                    {
                        HttpCookie cookie = new HttpCookie(nameof(Login));
                        cookie.Values.Add(nameof(loginViewModel.Username), userName);
                        cookie.Values.Add(nameof(loginViewModel.Password), password);
                        cookie.Expires = DateTime.Now.AddDays(1);
                        Response.SetCookie(cookie);
                    }
                    else
                    {
                        Response.Cookies.Clear();
                    }
                    return RedirectToAction("Index", "Chart");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Invalid username or password");                    
                }
            }

            return View(loginViewModel);
        }

        [HttpGet]
        public ActionResult Logout()
        {
            Response.Cookies[nameof(Login)].Expires = DateTime.Now.AddDays(-1);
            Session.Clear();
            //ServiceClient.Abort();
            return RedirectToAction(nameof(Login));
        }
    }
}