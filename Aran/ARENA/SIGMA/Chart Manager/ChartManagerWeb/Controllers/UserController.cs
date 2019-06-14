using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Web;
using System.Web.Mvc;
using ChartManagerWeb.ChartServiceReference;
using ChartManagerWeb.Helper;
using ChartManagerWeb.Helper.Attribute;
using ChartManagerWeb.Models.ViewModel;

namespace ChartManagerWeb.Controllers
{
    [LogActionFilter]
    [SessionExpire]
    public class UserController : BaseController
    {
        public ActionResult Index()
        {
            if (!CurrUser.IsAdmin)
            {
                return RedirectToAction("Index", "Chart");
            }

            List<ChartUser> userList = ServiceClient.GetAllUser();
            return View(userList);
        }

        [HttpGet]
        public ActionResult Create()
        {
            if (!CurrUser.IsAdmin)
                RedirectToAction("Index", "Chart");            
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ChartUser chartUser)
        {
            if (!CurrUser.IsAdmin)
                RedirectToAction("Index", "Chart");
            if (ModelState.IsValid)
            {
                try
                {
                    var mailAddress = new System.Net.Mail.MailAddress(chartUser.Email);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Invalid email address");
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        //chartUser.Password = collection[nameof(ChartUser.Password)];
                        ServiceClient.CreateUser(chartUser);
                        Success($"{chartUser.FirstName} {chartUser.LastName} was successfully created", true);
                        return RedirectToAction("Index");
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("", "Error occured !");
                    }
                }
            }
            Danger ("Looks like something went wrong. Please check your form.");
            return View(chartUser);
        }

        //[HttpGet]
        //public ActionResult ChangePassword(long id)
        //{
        //    if (!CurrUser.IsAdmin)
        //    {
        //        if (CurrUser.Id != id)
        //            return RedirectToAction(nameof(Index), "Chart");
        //    }
        //    var user = ServiceClient.GetUser(id);
        //    var viewModel = new UserPasswordViewModel()
        //    {
        //        Id = user.Id,
        //        FullName = $"{user.FirstName} {user.LastName}"
        //    };
        //    return View(viewModel);
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult ChangePassword(UserPasswordViewModel viewModel)
        //{
        //    if (!CurrUser.IsAdmin)
        //    {
        //        if (CurrUser.Id != viewModel.Id)
        //            return RedirectToAction(nameof(Index), "Chart");
        //    }
        //    if (!ModelState.IsValid)
        //    {
        //        return View(viewModel);
        //    }

        //    var oldPassword = Sha256_hash(viewModel.OldPassword);
        //    var newPassword = Sha256_hash(viewModel.NewPassword);

        //    try
        //    {
        //        ServiceClient.ChangePassword(viewModel.Id, oldPassword, newPassword);
        //    }
        //    catch (Exception e)
        //    {
        //        ModelState.AddModelError("", "Invalid username or password");
        //        return View(viewModel);
        //    }

        //    return RedirectToAction(nameof(Index));
        //}

        [HttpGet]
        public ActionResult ResetPassword(long id)
        {
            if (!CurrUser.IsAdmin)
            {
                if (CurrUser.Id != id)
                    return RedirectToAction(nameof(Index), "Chart");
            }

            var user = ServiceClient.GetUser(id);
            var viewModel = new ResetPasswordViewModel()
            {
                Id = id,
                FullName = $"{user.FirstName} {user.LastName}"
            };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword(ResetPasswordViewModel viewModel)
        {
            if (!CurrUser.IsAdmin)
            {
                if (CurrUser.Id != viewModel.Id)
                    return RedirectToAction(nameof(Index), "Chart");
            }
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            var newPassword = Sha256_hash(viewModel.NewPassword);

            try
            {
                ServiceClient.ChangePassword(viewModel.Id, string.Empty, viewModel.NewPassword);
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e);
                return View(viewModel);
            }

            
            SetPasswordInSession(newPassword);
            var cookie = Request.Cookies[nameof(AccountController.Login)];
            if (cookie != null)
            {
                //cookie
                //HttpCookie cookie = new HttpCookie(nameof(AccountController.Login));
                //cookie.Values.Add(nameof(LoginViewModel.Username), userName);
                cookie.Values[nameof(LoginViewModel.Password)] = newPassword;
                //cookie.Expires = DateTime.Now.AddDays(1);
                Response.SetCookie(cookie);
            }


            //var cookie = Response.Cookies[nameof(AccountController.Login)];
            
            //Request.Cookies[nameof(AccountController.Login)].Values[nameof(LoginViewModel.Password)] = newPassword;

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public ActionResult Edit(long id)
        {
            if (!CurrUser.IsAdmin)
            {
                if (CurrUser.Id != id)
                    return RedirectToAction(nameof(Index), "Chart");
            }
            return View(ServiceClient.GetUser(id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ChartUser chartUser)
        {
            if (!CurrUser.IsAdmin)
            {
                if (CurrUser.Id != chartUser.Id)
                    return RedirectToAction(nameof(Index), "Chart");
            }

            try
            {
                // TODO: Add update logic here
                ServiceClient.UpdateUser(chartUser);
                if (!CurrUser.IsAdmin)
                {
                    return RedirectToAction(nameof(Index), "Chart");
                }
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        [HttpGet]
        public ActionResult Download(long id)
        {
            if (!CurrUser.IsAdmin)
            {
                if (CurrUser.Id != id)
                    return RedirectToAction(nameof(Index), "Chart");
            }
            var bytes = ServiceClient.GetConfigFile(id);
            return File(bytes, System.Net.Mime.MediaTypeNames.Application.Octet, "Config.xml");
        }

        [HttpGet]
        public ActionResult Delete(long id)
        {
            if (!CurrUser.IsAdmin)
                return RedirectToAction("Login", "Account");
            return View(ServiceClient.GetUser(id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(ChartUser chartUser)
        {
            if (!CurrUser.IsAdmin)
                return RedirectToAction("Login", "Account");
            try
            {
                // TODO: Add delete logic here
                ServiceClient.DeleteUser(chartUser.Id);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //private ChartUser GetUser(FormCollection collection)
        //{
        //    var user = new ChartUser
        //    {
        //        FirstName = collection[nameof(ChartUser.FirstName)],
        //        LastName = collection[nameof(ChartUser.LastName)],
        //        Email = collection[nameof(ChartUser.Email)],
        //        Position = collection[nameof(ChartUser.Position)],
        //        Privilege = Enum.TryParse<UserPrivilege>(collection[nameof(ChartUser.Privilege)], out var res) ? res : UserPrivilege.ReadOnly,
        //        UserName = collection[nameof(ChartUser.UserName)],
        //        //Password =  Global.Sha256_hash(collection[nameof(ChartUser.Password)]),
        //        Disabled = Convert.ToBoolean(collection[nameof(ChartUser.Disabled)].Split(',')[0]),
        //        IsAdmin = Convert.ToBoolean(collection[nameof(ChartUser.IsAdmin)].Split(',')[0]),
        //    };
        //    return user;
        //}
    }
}
