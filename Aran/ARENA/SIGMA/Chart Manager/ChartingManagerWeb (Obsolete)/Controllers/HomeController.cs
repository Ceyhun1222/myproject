using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ChartingManagerWeb.Controllers
{
    public class HomeController : Controller
    {
        //[HttpGet]
        //[OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public ActionResult Index()
        {
            return View("~/Views/Shared/_myLoginLayOut.cshtml");
        }
        public ActionResult EditView()
        {
            return RedirectToAction("LoadLogin", "MyHome");
        }
        public ActionResult InsertView()
        {
            return RedirectToAction("LoadLogin", "MyHome");
        }
        public ActionResult ChangePswordView()
        {
            return RedirectToAction("LoadLogin", "MyHome");
        }
    }
}