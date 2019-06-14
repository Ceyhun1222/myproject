using ChartingManagerWeb.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml;

namespace ChartingManagerWeb.Controllers
{
    public class MyHomeController : Controller
    {
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public ActionResult EditLoad()
        {

            if (Session["username"] == null)
            {
                //return PartialView("~/Views/Shared/_myLoginLayOut.cshtml");
                return RedirectToAction("LoadLogin", "MyHome");
            }

            return View("~/Views/Home/EditView.cshtml");
        }
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public ActionResult InsertViewLoad()
        {

            if (Session["username"] == null)
            {
                //return PartialView("~/Views/Shared/_myLoginLayOut.cshtml");
                return RedirectToAction("LoadLogin", "MyHome");
            }

            return View("~/Views/Home/InsertView.cshtml");
        }

        public ActionResult LoadLogin()
        {
            return View("~/Views/Shared/_myLoginLayOut.cshtml");
        }
        [HttpPost]
        public JsonResult FileDownload()
        {
            string csv = "Make it downloadable ";
            var filresult = File(new System.Text.UTF8Encoding().GetBytes(csv), "application/csv", "downloaddocuments.csv");
            // return filresult;

            Response.Clear();
            Response.Buffer = true;
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("content-disposition", "attachment; filename=Statement_" + "Downloadfile" + ".csv");
            Response.Write(csv);
            Response.Flush();
            return Json(filresult);

        }
        [HttpPost]
        public JsonResult LoginChecking(string user, string passw)
        {
            ServiceConnectionStart services = new ServiceConnectionStart();
            ServiceConnectionStart.connected = services.ConnectionLoad();
            LoginControl login = new LoginControl();
            login = login.CheckFunction(user, passw, ServiceConnectionStart.connected);
            Session["username"] = login.Username;
            return Json(login);
        }
        public ActionResult HomePageLoad()
        {
            if (Session["username"] == null)
            {
                //return PartialView("~/Views/Shared/_myLoginLayOut.cshtml");
                return RedirectToAction("LoadLogin", "MyHome");
            }
            else
                return RedirectToAction("LoadHome", "MyHome");
        }
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        //[HttpGet]
        public ActionResult LoadHome()
        {
            if (Session["username"] == null)
            {
                return RedirectToAction("LoadLogin", "MyHome");
            }
            else
            {
                LoadHomePageList newload1 = new LoadHomePageList();
                return View("~/Views/Shared/_mylayoutPage.cshtml", newload1.UserList(ServiceConnectionStart.connected).OrderByDescending(x => x.Id));
            }
        }

        [HttpPost]
        public JsonResult InsertUser(string firstname, string lastname, string privilege, string username, string passw, string email, bool disabled, string position)
        {
            InsertUsrers insrtuser = new InsertUsrers();
            insrtuser.UserTInsert(ServiceConnectionStart.connected, firstname, lastname, privilege, username, passw, email, disabled, position);
            return Json("");
        }
        [HttpPost]
        public JsonResult EditUser(long id, string firstname, string lastname, string privilege, string username, string email, bool disabled, string position)
        {
            EditUser edituser = new EditUser();
            edituser.Edituser(ServiceConnectionStart.connected, id, firstname, lastname, privilege, username, email, disabled, position);
            return Json("");
        }
        [HttpPost]
        public JsonResult DeleteUser(long ID)
        {
            DeleteUser delete = new DeleteUser();
            delete.DeletUser(ServiceConnectionStart.connected, ID);
            return Json("");
        }
        //[OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        [HttpPost]
        public JsonResult DisableUser(long ID, bool disable)
        {
            DisableUser disabled = new DisableUser();
            disabled.DisableUsers(ID, disable, ServiceConnectionStart.connected);
            return Json("");
        }
        [HttpPost]
        public JsonResult ChangePassword(long ID, string oldpassw, string newpassw)
        {
            ChangePassword cnhgpsw = new ChangePassword();
            cnhgpsw.ChangePasswrd(ServiceConnectionStart.connected, ID, oldpassw, newpassw);
            return Json("");
        }
        [HttpPost]
        public JsonResult ChangePasswordOnlyAdmin(string OnlyAdmin, string oldpsw, string newpsw)
        {
            ChangePassword chg = new ChangePassword();
            chg.OnlyAdminChangePsw(ServiceConnectionStart.connected, OnlyAdmin, oldpsw, newpsw);
            return Json("");
        }
        [HttpPost]
        public JsonResult ChangedPrivilege(long id, string firstname, string lastname, string privilege, string username, string email, bool disabled, string position)
        {
            UpdatePrivilege upprivilege = new UpdatePrivilege();
            upprivilege.ChangePrivilege(ServiceConnectionStart.connected, id, firstname, lastname, privilege, username, email, disabled, position);
            return Json("");
        }
        [HttpGet]
        public ActionResult SaveConfigFilem(long ID)
        {
            SaveConfigFile svcfig = new SaveConfigFile();
            byte[] filebytes = svcfig.ConfigSaveAs(ID, ServiceConnectionStart.connected);
            var file = "Config.xml";
            return File(filebytes, System.Net.Mime.MediaTypeNames.Application.Octet, file);
        }
        [HttpPost]
        public JsonResult SessionAbandon()
        {
            Session.Abandon();
            //Session.Clear();
            return Json("");
        }

        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public ActionResult PswChanegLink()
        {
            if (Session["username"] == null)
            {
                return RedirectToAction("LoadLogin", "MyHome");
            }

            return View("~/Views/Home/ChangePswordView.cshtml");

        }

    }
}